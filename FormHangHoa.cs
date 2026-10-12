using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.EntityFrameworkCore;
using Cau1_TiemTapHoa.Models;

namespace Cau1_TiemTapHoa
{
    public partial class FormHangHoa : Form
    {
        private int? _selectedMaHang = null;
        private List<HangHoa> _allHangHoa = new List<HangHoa>();

        public FormHangHoa()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Populate ComboBoxes
            cboLoaiHang.Items.AddRange(new object[] { "Đồ uống", "Thực phẩm khô", "Gia vị", "Đồ gia dụng" });
            cboLoaiHang.SelectedIndex = 0;

            cboDonVi.Items.AddRange(new object[] { "Chai", "Gói", "Hộp", "Túi" });
            cboDonVi.SelectedIndex = 0;

            cboLocLoaiHang.Items.AddRange(new object[] { "Tất cả các loại", "Đồ uống", "Thực phẩm khô", "Gia vị", "Đồ gia dụng" });
            cboLocLoaiHang.SelectedIndex = 0;

            // Timer debounce for search
            tmrSearch.Interval = 350;
            tmrSearch.Tick += async (s, e) =>
            {
                tmrSearch.Stop();
                await ApplyFilterAsync();
            };

            // Event wiring
            Load += async (s, e) => await LoadDataAsync();
            dgvHangHoa.CellClick += DgvHangHoa_CellClick;
            dgvHangHoa.SelectionChanged += DgvHangHoa_SelectionChanged;

            btnThem.Click += async (s, e) => await ThemHangHoaAsync();
            btnSua.Click += async (s, e) => await SuaHangHoaAsync();
            btnXoa.Click += async (s, e) => await XoaHangHoaAsync();
            btnLamMoi.Click += (s, e) => ResetForm();

            txtTimKiem.TextChanged += (s, e) =>
            {
                tmrSearch.Stop();
                tmrSearch.Start();
            };

            cboLocLoaiHang.SelectedIndexChanged += async (s, e) => await ApplyFilterAsync();
            chkLocConKinhDoanh.CheckedChanged += async (s, e) => await ApplyFilterAsync();

            btnXemBangGia.Click += async (s, e) => await XemBangGiaReportAsync();
            btnXuatPdf.Click += async (s, e) => await XuatPdfReportAsync();
            btnXuatExcelLuoi.Click += BtnXuatExcelLuoi_Click;
            btnXuatExcelTonKho.Click += async (s, e) => await XuatExcelTonKhoAsync();
        }

        #region Data Operations (EF Core Async)

        private async Task LoadDataAsync()
        {
            try
            {
                using var db = new TiemTapHoaDbContext();
                _allHangHoa = await db.HangHoas.AsNoTracking().OrderBy(h => h.MaHang).ToListAsync();
                await ApplyFilterAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu từ CSDL: " + ex.Message, "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task ApplyFilterAsync()
        {
            await Task.Yield();
            try
            {
                var query = _allHangHoa.AsEnumerable();

                // 1. Tên hàng
                string kw = txtTimKiem.Text.Trim();
                if (!string.IsNullOrEmpty(kw))
                {
                    query = query.Where(h => h.TenHang.Contains(kw, StringComparison.OrdinalIgnoreCase));
                }

                // 2. Loại hàng
                if (cboLocLoaiHang.SelectedIndex > 0)
                {
                    string loai = cboLocLoaiHang.SelectedItem?.ToString() ?? "";
                    query = query.Where(h => h.LoaiHang == loai);
                }

                // 3. Chỉ hiện hàng còn kinh doanh
                if (chkLocConKinhDoanh.Checked)
                {
                    query = query.Where(h => h.ConKinhDoanh);
                }

                var filteredList = query.ToList();
                DisplayGridView(filteredList);
                lblTrangThai.Text = $"Tìm thấy {filteredList.Count} mặt hàng";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lọc dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayGridView(List<HangHoa> list)
        {
            dgvHangHoa.DataSource = null;
            dgvHangHoa.AutoGenerateColumns = false;
            dgvHangHoa.Columns.Clear();

            // Cột Mã
            dgvHangHoa.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaHang",
                HeaderText = "Mã",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Tên hàng
            dgvHangHoa.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenHang",
                HeaderText = "Tên hàng",
                Width = 260
            });

            // Cột Loại hàng
            dgvHangHoa.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "LoaiHang",
                HeaderText = "Loại hàng",
                Width = 140
            });

            // Cột Đơn vị
            dgvHangHoa.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonVi",
                HeaderText = "Đơn vị",
                Width = 90,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Giá bán
            dgvHangHoa.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaBan",
                HeaderText = "Giá bán",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0"
                }
            });

            // Cột Tồn
            dgvHangHoa.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuongTon",
                HeaderText = "Tồn",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0"
                }
            });

            // Cột Ngày nhập
            dgvHangHoa.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayNhap",
                HeaderText = "Ngày nhập",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "dd/MM/yyyy"
                }
            });

            // Cột Kinh doanh
            dgvHangHoa.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = "ConKinhDoanh",
                HeaderText = "Kinh doanh",
                Width = 90
            });

            dgvHangHoa.DataSource = list;
        }

        private void DgvHangHoa_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvHangHoa.CurrentRow?.DataBoundItem is HangHoa item)
            {
                LoadItemToInputs(item);
            }
        }

        private void DgvHangHoa_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvHangHoa.Rows[e.RowIndex].DataBoundItem is HangHoa item)
            {
                LoadItemToInputs(item);
            }
        }

        private void LoadItemToInputs(HangHoa item)
        {
            _selectedMaHang = item.MaHang;
            txtTenHang.Text = item.TenHang;
            if (item.LoaiHang != null && cboLoaiHang.Items.Contains(item.LoaiHang))
                cboLoaiHang.SelectedItem = item.LoaiHang;
            if (item.DonVi != null && cboDonVi.Items.Contains(item.DonVi))
                cboDonVi.SelectedItem = item.DonVi;
            numGiaBan.Value = item.GiaBan;
            numSoLuongTon.Value = item.SoLuongTon;
            dtpNgayNhap.Value = item.NgayNhap ?? DateTime.Now;
            chkConKinhDoanh.Checked = item.ConKinhDoanh;
        }

        private void ResetForm()
        {
            _selectedMaHang = null;
            txtTenHang.Clear();
            if (cboLoaiHang.Items.Count > 0) cboLoaiHang.SelectedIndex = 0;
            if (cboDonVi.Items.Count > 0) cboDonVi.SelectedIndex = 0;
            numGiaBan.Value = 0;
            numSoLuongTon.Value = 0;
            dtpNgayNhap.Value = DateTime.Now;
            chkConKinhDoanh.Checked = true;
            dgvHangHoa.ClearSelection();
            txtTenHang.Focus();
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtTenHang.Text))
            {
                MessageBox.Show("Tên hàng không được để trống!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTenHang.Focus();
                return false;
            }
            if (numGiaBan.Value <= 0)
            {
                MessageBox.Show("Giá bán phải lớn hơn 0!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numGiaBan.Focus();
                return false;
            }
            return true;
        }

        private async Task ThemHangHoaAsync()
        {
            if (!ValidateInputs()) return;

            try
            {
                var item = new HangHoa
                {
                    TenHang = txtTenHang.Text.Trim(),
                    LoaiHang = cboLoaiHang.SelectedItem?.ToString(),
                    DonVi = cboDonVi.SelectedItem?.ToString(),
                    GiaBan = numGiaBan.Value,
                    SoLuongTon = (int)numSoLuongTon.Value,
                    NgayNhap = dtpNgayNhap.Value.Date,
                    ConKinhDoanh = chkConKinhDoanh.Checked
                };

                using var db = new TiemTapHoaDbContext();
                db.HangHoas.Add(item);
                await db.SaveChangesAsync();

                MessageBox.Show("Thêm hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ResetForm();
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SuaHangHoaAsync()
        {
            if (_selectedMaHang == null)
            {
                MessageBox.Show("Vui lòng chọn hàng hóa cần sửa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateInputs()) return;

            try
            {
                using var db = new TiemTapHoaDbContext();
                var item = await db.HangHoas.FindAsync(_selectedMaHang.Value);
                if (item == null)
                {
                    MessageBox.Show("Không tìm thấy hàng hóa trong CSDL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                item.TenHang = txtTenHang.Text.Trim();
                item.LoaiHang = cboLoaiHang.SelectedItem?.ToString();
                item.DonVi = cboDonVi.SelectedItem?.ToString();
                item.GiaBan = numGiaBan.Value;
                item.SoLuongTon = (int)numSoLuongTon.Value;
                item.NgayNhap = dtpNgayNhap.Value.Date;
                item.ConKinhDoanh = chkConKinhDoanh.Checked;

                await db.SaveChangesAsync();
                MessageBox.Show("Cập nhật hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task XoaHangHoaAsync()
        {
            if (_selectedMaHang == null)
            {
                MessageBox.Show("Vui lòng chọn hàng hóa cần xóa từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa hàng hóa '{txtTenHang.Text}'?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using var db = new TiemTapHoaDbContext();
                var item = await db.HangHoas.FindAsync(_selectedMaHang.Value);
                if (item != null)
                {
                    db.HangHoas.Remove(item);
                    await db.SaveChangesAsync();
                    MessageBox.Show("Đã xóa hàng hóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetForm();
                    await LoadDataAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa hàng hóa: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region FastReport Operations

        private async Task<Report?> PrepareBangGiaReportAsync()
        {
            try
            {
                using var db = new TiemTapHoaDbContext();
                var items = await db.HangHoas
                    .AsNoTracking()
                    .Where(h => h.ConKinhDoanh)
                    .OrderBy(h => h.MaHang)
                    .ToListAsync();

                // Chuyển kết quả sang DataTable theo yêu cầu task 7
                DataTable dt = new DataTable("HangHoa");
                dt.Columns.Add("MaHang", typeof(int));
                dt.Columns.Add("TenHang", typeof(string));
                dt.Columns.Add("LoaiHang", typeof(string));
                dt.Columns.Add("DonVi", typeof(string));
                dt.Columns.Add("GiaBan", typeof(decimal));
                dt.Columns.Add("SoLuongTon", typeof(int));
                dt.Columns.Add("NgayNhap", typeof(DateTime));
                dt.Columns.Add("ConKinhDoanh", typeof(bool));

                foreach (var item in items)
                {
                    dt.Rows.Add(item.MaHang, item.TenHang, item.LoaiHang ?? "", item.DonVi ?? "",
                        item.GiaBan, item.SoLuongTon, item.NgayNhap ?? DateTime.Now, item.ConKinhDoanh);
                }

                Report report = new Report();
                string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "BangGia.frx");
                if (!File.Exists(reportPath))
                {
                    // Fallback to project folder if running under VS debug
                    reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "BangGia.frx");
                }

                if (!File.Exists(reportPath))
                {
                    MessageBox.Show("Không tìm thấy file mẫu báo cáo BangGia.frx tại: " + reportPath,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                report.Load(reportPath);
                report.RegisterData(dt, "HangHoa");
                var ds = report.GetDataSource("HangHoa");
                if (ds != null)
                {
                    ds.Enabled = true; // Theo yêu cầu task 7: nhớ bật Enabled = true
                }

                // Truyền tham số Ngày in theo yêu cầu task 8
                report.SetParameterValue("NgayIn", DateTime.Now.ToString("dd/MM/yyyy"));

                report.Prepare();
                return report;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private async Task XemBangGiaReportAsync()
        {
            var report = await PrepareBangGiaReportAsync();
            if (report != null)
            {
                using var previewForm = new FormReportPreview(report, "FastReport Preview - BẢNG GIÁ HÀNG HÓA");
                previewForm.ShowDialog(this);
            }
        }

        private async Task XuatPdfReportAsync()
        {
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "PDF File (*.pdf)|*.pdf",
                Title = "Xuất PDF Bảng giá hàng hóa",
                FileName = "BangGia_HangHoa.pdf"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                var report = await PrepareBangGiaReportAsync();
                if (report == null) return;

                PDFSimpleExport export = new PDFSimpleExport();
                report.Export(export, sfd.FileName);

                var ans = MessageBox.Show("Xuất file PDF thành công! Bạn có muốn mở file không?",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (ans == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File PDF đang được mở bởi một chương trình khác. Vui lòng đóng file rồi thử lại!",
                    "Lỗi ghi file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất file PDF: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region ClosedXML Excel Operations

        // Task 10: Xuất Excel (lưới)
        private void BtnXuatExcelLuoi_Click(object? sender, EventArgs e)
        {
            if (dgvHangHoa.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu trên lưới để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Xuất dữ liệu lưới ra Excel",
                FileName = "DanhSachHangHoa_Luoi.xlsx"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("DanhSachHangHoa");

                // Headers
                for (int c = 0; c < dgvHangHoa.Columns.Count; c++)
                {
                    worksheet.Cell(1, c + 1).Value = dgvHangHoa.Columns[c].HeaderText;
                    worksheet.Cell(1, c + 1).Style.Font.Bold = true;
                    worksheet.Cell(1, c + 1).Style.Fill.BackgroundColor = XLColor.FromHtml("#E0E0E0");
                }

                // Rows (Số phải là số, không phải chuỗi)
                for (int r = 0; r < dgvHangHoa.Rows.Count; r++)
                {
                    var row = dgvHangHoa.Rows[r];
                    for (int c = 0; c < dgvHangHoa.Columns.Count; c++)
                    {
                        var val = row.Cells[c].Value;
                        var cell = worksheet.Cell(r + 2, c + 1);

                        if (val is int intVal)
                        {
                            cell.Value = intVal;
                            cell.Style.NumberFormat.Format = "#,##0";
                        }
                        else if (val is decimal decVal)
                        {
                            cell.Value = (double)decVal;
                            cell.Style.NumberFormat.Format = "#,##0";
                        }
                        else if (val is double dblVal)
                        {
                            cell.Value = dblVal;
                            cell.Style.NumberFormat.Format = "#,##0";
                        }
                        else if (val is DateTime dtVal)
                        {
                            cell.Value = dtVal;
                            cell.Style.DateFormat.Format = "dd/MM/yyyy";
                        }
                        else if (val is bool bVal)
                        {
                            cell.Value = bVal ? "Còn bán" : "Ngừng bán";
                        }
                        else
                        {
                            cell.Value = val?.ToString() ?? "";
                        }
                    }
                }

                worksheet.Columns().AdjustToContents();
                workbook.SaveAs(sfd.FileName);

                var ans = MessageBox.Show("Xuất Excel từ lưới thành công! Bạn có muốn mở file không?",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (ans == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File Excel đang được mở bởi một chương trình khác. Vui lòng đóng file rồi thử lại!",
                    "Lỗi file đang mở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Task 11: Xuất Excel tồn kho
        private async Task XuatExcelTonKhoAsync()
        {
            using SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                Title = "Xuất báo cáo tồn kho ra Excel",
                FileName = "BaoCaoTonKho.xlsx"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using var db = new TiemTapHoaDbContext();
                // Truy vấn lại bằng EF Core các hàng còn kinh doanh
                var items = await db.HangHoas
                    .AsNoTracking()
                    .Where(h => h.ConKinhDoanh)
                    .OrderBy(h => h.MaHang)
                    .ToListAsync();

                using var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("TonKho");

                // Tiêu đề
                string[] headers = { "STT", "Tên hàng", "Đơn vị", "Giá bán", "Số lượng tồn", "Giá trị tồn" };
                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = worksheet.Cell(1, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true; // Dòng tiêu đề in đậm
                    cell.Style.Fill.BackgroundColor = XLColor.FromHtml("#D9E1F2");
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                }

                // Dữ liệu
                int currentRow = 2;
                for (int i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    worksheet.Cell(currentRow, 1).Value = i + 1; // STT
                    worksheet.Cell(currentRow, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    worksheet.Cell(currentRow, 2).Value = item.TenHang; // Tên hàng

                    worksheet.Cell(currentRow, 3).Value = item.DonVi ?? ""; // Đơn vị
                    worksheet.Cell(currentRow, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Giá bán (cột D)
                    var cellGiaBan = worksheet.Cell(currentRow, 4);
                    cellGiaBan.Value = (double)item.GiaBan;
                    cellGiaBan.Style.NumberFormat.Format = "#,##0";
                    cellGiaBan.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    // Số lượng tồn (cột E)
                    var cellTon = worksheet.Cell(currentRow, 5);
                    cellTon.Value = item.SoLuongTon;
                    cellTon.Style.NumberFormat.Format = "#,##0";
                    cellTon.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    // Giá trị tồn (cột F) là công thức = D * E
                    var cellGiaTri = worksheet.Cell(currentRow, 6);
                    cellGiaTri.FormulaA1 = $"D{currentRow}*E{currentRow}";
                    cellGiaTri.Style.NumberFormat.Format = "#,##0";
                    cellGiaTri.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                    currentRow++;
                }

                // Dòng "Tổng"
                worksheet.Cell(currentRow, 2).Value = "Tổng";
                worksheet.Cell(currentRow, 2).Style.Font.Bold = true;
                worksheet.Cell(currentRow, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Công thức SUM cho cột Giá trị tồn: =SUM(F2:F{currentRow - 1})
                var cellTong = worksheet.Cell(currentRow, 6);
                cellTong.FormulaA1 = $"SUM(F2:F{currentRow - 1})";
                cellTong.Style.Font.Bold = true;
                cellTong.Style.NumberFormat.Format = "#,##0";
                cellTong.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;

                // Thêm viền bảng
                worksheet.Range(1, 1, currentRow, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Range(1, 1, currentRow, 6).Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // AutoFilter trên vùng tiêu đề
                worksheet.Range(1, 1, currentRow - 1, 6).SetAutoFilter();

                // Cố định dòng tiêu đề (Freeze)
                worksheet.SheetView.FreezeRows(1);

                // Auto-fit columns
                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(sfd.FileName);

                var ans = MessageBox.Show("Xuất báo cáo tồn kho thành công! Bạn có muốn mở file không?",
                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (ans == DialogResult.Yes)
                {
                    Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File Excel đang được mở bởi một chương trình khác. Vui lòng đóng file rồi thử lại!",
                    "Lỗi file đang mở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất Excel tồn kho: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
