using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastReport;
using FastReport.Export.PdfSimple;
using Microsoft.EntityFrameworkCore;
using Cau2_QuanCaPhe.DTO;
using Cau2_QuanCaPhe.Models;

namespace Cau2_QuanCaPhe
{
    public partial class FormQuanCaPhe : Form
    {
        private int? _selectedMaHD = null;
        private List<Mon> _danhSachMon = new List<Mon>();

        // Lớp ViewModel hiển thị danh sách hóa đơn bên trái
        private class HoaDonViewModel
        {
            public int MaHD { get; set; }
            public string MaHDHienThi => $"HD{MaHD:D6}";
            public DateTime NgayLap { get; set; }
            public int SoBan { get; set; }
            public string HinhThucTT { get; set; } = string.Empty;
            public decimal TongTien { get; set; }
            public string? GhiChu { get; set; }
        }

        // Lớp ViewModel hiển thị chi tiết hóa đơn bên phải
        private class ChiTietViewModel
        {
            public int MaCT { get; set; }
            public int STT { get; set; }
            public int MaMon { get; set; }
            public string TenMon { get; set; } = string.Empty;
            public int SoLuong { get; set; }
            public decimal DonGia { get; set; }
            public decimal ThanhTien => SoLuong * DonGia;
        }

        public FormQuanCaPhe()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Thiết lập giá trị mặc định cho điều kiện tìm kiếm
            dtpTuNgay.Value = new DateTime(2026, 10, 1);
            dtpDenNgay.Value = new DateTime(2026, 10, 31, 23, 59, 59);

            cboLocBan.Items.Add("Tất cả");
            for (int i = 1; i <= 10; i++) cboLocBan.Items.Add(i.ToString());
            cboLocBan.SelectedIndex = 0;

            cboLocHinhThuc.Items.AddRange(new object[] { "Tất cả", "Tiền mặt", "Chuyển khoản" });
            cboLocHinhThuc.SelectedIndex = 0;

            // Số bàn cho hóa đơn
            for (int i = 1; i <= 10; i++) cboSoBan.Items.Add(i.ToString());
            cboSoBan.SelectedIndex = 0;

            // Gắn sự kiện
            Load += async (s, e) =>
            {
                await LoadDanhMucMonAsync();
                await LoadDanhSachHoaDonAsync();
            };

            btnTim.Click += async (s, e) => await LoadDanhSachHoaDonAsync();
            dgvHoaDon.CellClick += async (s, e) => await DgvHoaDon_SelectionChangedAsync();
            dgvHoaDon.SelectionChanged += async (s, e) => await DgvHoaDon_SelectionChangedAsync();

            btnHoaDonMoi.Click += async (s, e) => await TaoHoaDonMoiAsync();
            btnSuaHoaDon.Click += async (s, e) => await SuaHoaDonAsync();
            btnXoaHoaDon.Click += async (s, e) => await XoaHoaDonAsync();

            btnThemMon.Click += async (s, e) => await ThemMonVaoHDAsync();
            btnSuaSL.Click += async (s, e) => await SuaSoLuongMonAsync();
            btnXoaMon.Click += async (s, e) => await XoaMonKhoiHDAsync();

            dgvChiTiet.SelectionChanged += DgvChiTiet_SelectionChanged;

            btnXemHoaDon.Click += async (s, e) => await InHoacXemHoaDonAsync(ReportAction.Preview);
            btnXuatPdf.Click += async (s, e) => await InHoacXemHoaDonAsync(ReportAction.ExportPdf);
            btnIn.Click += async (s, e) => await InHoacXemHoaDonAsync(ReportAction.Print);
        }

        #region Tải dữ liệu

        private async Task LoadDanhMucMonAsync()
        {
            try
            {
                using var db = new QuanCaPheDbContext();
                _danhSachMon = await db.Mons.AsNoTracking().OrderBy(m => m.MaMon).ToListAsync();

                cboChonMon.DisplayMember = "DisplayText";
                cboChonMon.ValueMember = "MaMon";
                cboChonMon.DataSource = _danhSachMon.Select(m => new
                {
                    m.MaMon,
                    m.TenMon,
                    m.DonGia,
                    DisplayText = $"{m.TenMon} - {m.DonGia:N0} đ"
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh mục món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadDanhSachHoaDonAsync()
        {
            try
            {
                using var db = new QuanCaPheDbContext();
                var query = db.HoaDons
                    .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.Mon)
                    .AsNoTracking()
                    .AsQueryable();

                // Lọc theo khoảng ngày
                DateTime tuNgay = dtpTuNgay.Value.Date;
                DateTime denNgay = dtpDenNgay.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(h => h.NgayLap >= tuNgay && h.NgayLap <= denNgay);

                // Lọc theo số bàn
                if (cboLocBan.SelectedIndex > 0 && int.TryParse(cboLocBan.SelectedItem?.ToString(), out int soBanLoc))
                {
                    query = query.Where(h => h.SoBan == soBanLoc);
                }

                // Lọc theo hình thức thanh toán
                if (cboLocHinhThuc.SelectedIndex > 0)
                {
                    string ht = cboLocHinhThuc.SelectedItem?.ToString() ?? "";
                    query = query.Where(h => h.HinhThucTT == ht);
                }

                // Lọc theo tên món
                string tenMonLoc = txtLocTenMon.Text.Trim();
                if (!string.IsNullOrEmpty(tenMonLoc))
                {
                    query = query.Where(h => h.ChiTietHoaDons.Any(ct => ct.Mon.TenMon.Contains(tenMonLoc)));
                }

                var listHD = await query.OrderByDescending(h => h.MaHD).ToListAsync();

                var viewList = listHD.Select(h => new HoaDonViewModel
                {
                    MaHD = h.MaHD,
                    NgayLap = h.NgayLap,
                    SoBan = h.SoBan,
                    HinhThucTT = h.HinhThucTT,
                    TongTien = h.ChiTietHoaDons.Sum(ct => ct.SoLuong * ct.DonGia),
                    GhiChu = h.GhiChu
                }).ToList();

                DisplayHoaDonGrid(viewList);

                decimal tongDoanhThu = viewList.Sum(v => v.TongTien);
                lblTongDoanhThu.Text = $"Tổng doanh thu đang hiển thị: {tongDoanhThu:N0} đ";

                // Tự động chọn hóa đơn đầu tiên nếu có
                if (viewList.Count > 0)
                {
                    if (_selectedMaHD.HasValue && viewList.Any(v => v.MaHD == _selectedMaHD.Value))
                    {
                        SelectRowByMaHD(_selectedMaHD.Value);
                    }
                    else
                    {
                        SelectRowByMaHD(viewList[0].MaHD);
                    }
                }
                else
                {
                    _selectedMaHD = null;
                    dgvChiTiet.DataSource = null;
                    lblTongCong.Text = "Tổng cộng: 0 đ";
                    grpChiTiet.Text = "Chi tiết hóa đơn";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayHoaDonGrid(List<HoaDonViewModel> list)
        {
            dgvHoaDon.DataSource = null;
            dgvHoaDon.AutoGenerateColumns = false;
            dgvHoaDon.Columns.Clear();

            // Cột Mã HD
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MaHDHienThi",
                HeaderText = "Mã HD",
                Width = 85,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Ngày lập
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "NgayLap",
                HeaderText = "Ngày lập",
                Width = 125,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Format = "dd/MM/yyyy HH:mm"
                }
            });

            // Cột Số bàn
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoBan",
                HeaderText = "Số bàn",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Hình thức TT
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "HinhThucTT",
                HeaderText = "Hình thức TT",
                Width = 95,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Cột Tổng tiền
            dgvHoaDon.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TongTien",
                HeaderText = "Tổng tiền",
                Width = 95,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0"
                }
            });

            dgvHoaDon.DataSource = list;
        }

        private void SelectRowByMaHD(int maHd)
        {
            for (int i = 0; i < dgvHoaDon.Rows.Count; i++)
            {
                if (dgvHoaDon.Rows[i].DataBoundItem is HoaDonViewModel vm && vm.MaHD == maHd)
                {
                    dgvHoaDon.Rows[i].Selected = true;
                    dgvHoaDon.CurrentCell = dgvHoaDon.Rows[i].Cells[0];
                    break;
                }
            }
        }

        private async Task DgvHoaDon_SelectionChangedAsync()
        {
            if (dgvHoaDon.CurrentRow?.DataBoundItem is HoaDonViewModel vm)
            {
                _selectedMaHD = vm.MaHD;
                grpChiTiet.Text = $"Chi tiết hóa đơn {vm.MaHDHienThi}";
                dtpNgayLap.Value = vm.NgayLap;
                cboSoBan.SelectedItem = vm.SoBan.ToString();
                if (vm.HinhThucTT == "Tiền mặt") radTienMat.Checked = true;
                else radChuyenKhoan.Checked = true;
                txtGhiChu.Text = vm.GhiChu ?? "";

                await LoadChiTietHoaDonAsync(vm.MaHD);
            }
        }

        private async Task LoadChiTietHoaDonAsync(int maHd)
        {
            try
            {
                using var db = new QuanCaPheDbContext();
                var items = await db.ChiTietHoaDons
                    .Include(ct => ct.Mon)
                    .AsNoTracking()
                    .Where(ct => ct.MaHD == maHd)
                    .OrderBy(ct => ct.MaCT)
                    .ToListAsync();

                var listVM = new List<ChiTietViewModel>();
                for (int i = 0; i < items.Count; i++)
                {
                    listVM.Add(new ChiTietViewModel
                    {
                        MaCT = items[i].MaCT,
                        STT = i + 1,
                        MaMon = items[i].MaMon,
                        TenMon = items[i].Mon?.TenMon ?? "",
                        SoLuong = items[i].SoLuong,
                        DonGia = items[i].DonGia
                    });
                }

                DisplayChiTietGrid(listVM);

                decimal tongTien = listVM.Sum(x => x.ThanhTien);
                lblTongCong.Text = $"Tổng cộng: {tongTien:N0} đ";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayChiTietGrid(List<ChiTietViewModel> list)
        {
            dgvChiTiet.DataSource = null;
            dgvChiTiet.AutoGenerateColumns = false;
            dgvChiTiet.Columns.Clear();

            // STT
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "STT",
                HeaderText = "STT",
                Width = 45,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Tên món
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenMon",
                HeaderText = "Tên món",
                Width = 190
            });

            // SL
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SoLuong",
                HeaderText = "SL",
                Width = 50,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            // Đơn giá
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonGia",
                HeaderText = "Đơn giá",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0"
                }
            });

            // Thành tiền
            dgvChiTiet.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ThanhTien",
                HeaderText = "Thành tiền",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Format = "N0"
                }
            });

            dgvChiTiet.DataSource = list;
        }

        private void DgvChiTiet_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvChiTiet.CurrentRow?.DataBoundItem is ChiTietViewModel ct)
            {
                numSL.Value = ct.SoLuong;
                if (cboChonMon.Items.Count > 0)
                {
                    cboChonMon.SelectedValue = ct.MaMon;
                }
            }
        }

        #endregion

        #region Nghiệp vụ Hóa đơn

        private async Task TaoHoaDonMoiAsync()
        {
            try
            {
                int soBan = int.Parse(cboSoBan.SelectedItem?.ToString() ?? "1");
                string hinhThuc = radTienMat.Checked ? "Tiền mặt" : "Chuyển khoản";
                string? ghiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim();

                var hd = new HoaDon
                {
                    NgayLap = dtpNgayLap.Value,
                    SoBan = soBan,
                    HinhThucTT = hinhThuc,
                    GhiChu = ghiChu
                };

                using var db = new QuanCaPheDbContext();
                db.HoaDons.Add(hd);
                await db.SaveChangesAsync();

                _selectedMaHD = hd.MaHD;
                MessageBox.Show($"Tạo hóa đơn HD{hd.MaHD:D6} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDanhSachHoaDonAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo hóa đơn mới: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SuaHoaDonAsync()
        {
            if (_selectedMaHD == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new QuanCaPheDbContext();
                var hd = await db.HoaDons.FindAsync(_selectedMaHD.Value);
                if (hd == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn trong CSDL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                hd.NgayLap = dtpNgayLap.Value;
                hd.SoBan = int.Parse(cboSoBan.SelectedItem?.ToString() ?? "1");
                hd.HinhThucTT = radTienMat.Checked ? "Tiền mặt" : "Chuyển khoản";
                hd.GhiChu = string.IsNullOrWhiteSpace(txtGhiChu.Text) ? null : txtGhiChu.Text.Trim();

                await db.SaveChangesAsync();
                MessageBox.Show($"Cập nhật hóa đơn HD{hd.MaHD:D6} thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDanhSachHoaDonAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task XoaHoaDonAsync()
        {
            if (_selectedMaHD == null)
            {
                MessageBox.Show("Vui lòng chọn hóa đơn cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc chắn muốn xóa hóa đơn HD{_selectedMaHD.Value:D6}?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using var db = new QuanCaPheDbContext();
                // Theo yêu cầu task 5: Xóa các dòng chi tiết trước rồi mới xóa hóa đơn
                var chiTiets = await db.ChiTietHoaDons.Where(ct => ct.MaHD == _selectedMaHD.Value).ToListAsync();
                db.ChiTietHoaDons.RemoveRange(chiTiets);

                var hd = await db.HoaDons.FindAsync(_selectedMaHD.Value);
                if (hd != null)
                {
                    db.HoaDons.Remove(hd);
                }

                await db.SaveChangesAsync();
                _selectedMaHD = null;
                MessageBox.Show("Đã xóa hóa đơn thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await LoadDanhSachHoaDonAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa hóa đơn: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Nghiệp vụ Chi tiết món

        private async Task ThemMonVaoHDAsync()
        {
            if (_selectedMaHD == null)
            {
                MessageBox.Show("Vui lòng chọn hoặc tạo hóa đơn trước khi thêm món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cboChonMon.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn món!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int maMon = (int)cboChonMon.SelectedValue;
            int sl = (int)numSL.Value;

            try
            {
                using var db = new QuanCaPheDbContext();
                // Đơn giá lấy từ bảng Mon tại thời điểm thêm theo yêu cầu task 6
                var mon = await db.Mons.FindAsync(maMon);
                if (mon == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin món!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Kiểm tra xem món này đã có trong hóa đơn chưa
                var exist = await db.ChiTietHoaDons
                    .FirstOrDefaultAsync(ct => ct.MaHD == _selectedMaHD.Value && ct.MaMon == maMon);

                if (exist != null)
                {
                    exist.SoLuong += sl;
                }
                else
                {
                    var ct = new ChiTietHoaDon
                    {
                        MaHD = _selectedMaHD.Value,
                        MaMon = maMon,
                        SoLuong = sl,
                        DonGia = mon.DonGia
                    };
                    db.ChiTietHoaDons.Add(ct);
                }

                await db.SaveChangesAsync();

                // Cập nhật lại chi tiết và danh sách bên trái
                await LoadChiTietHoaDonAsync(_selectedMaHD.Value);
                await LoadDanhSachHoaDonAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi thêm món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task SuaSoLuongMonAsync()
        {
            if (dgvChiTiet.CurrentRow?.DataBoundItem is not ChiTietViewModel curItem)
            {
                MessageBox.Show("Vui lòng chọn dòng món trong bảng chi tiết cần sửa số lượng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int slMoi = (int)numSL.Value;
            if (slMoi <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new QuanCaPheDbContext();
                var ct = await db.ChiTietHoaDons.FindAsync(curItem.MaCT);
                if (ct != null)
                {
                    ct.SoLuong = slMoi;
                    await db.SaveChangesAsync();

                    await LoadChiTietHoaDonAsync(_selectedMaHD!.Value);
                    await LoadDanhSachHoaDonAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi sửa số lượng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task XoaMonKhoiHDAsync()
        {
            if (dgvChiTiet.CurrentRow?.DataBoundItem is not ChiTietViewModel curItem)
            {
                MessageBox.Show("Vui lòng chọn dòng món cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show($"Bạn có muốn xóa món '{curItem.TenMon}' khỏi hóa đơn?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            try
            {
                using var db = new QuanCaPheDbContext();
                var ct = await db.ChiTietHoaDons.FindAsync(curItem.MaCT);
                if (ct != null)
                {
                    db.ChiTietHoaDons.Remove(ct);
                    await db.SaveChangesAsync();

                    await LoadChiTietHoaDonAsync(_selectedMaHD!.Value);
                    await LoadDanhSachHoaDonAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi xóa món: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region FastReport Báo cáo Hóa đơn

        private enum ReportAction { Preview, ExportPdf, Print }

        private async Task InHoacXemHoaDonAsync(ReportAction action)
        {
            // Kiểm tra: nếu chưa chọn hóa đơn hoặc hóa đơn chưa có món thì báo MessageBox
            if (_selectedMaHD == null)
            {
                MessageBox.Show("Vui lòng chọn một hóa đơn từ danh sách!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using var db = new QuanCaPheDbContext();
                var hd = await db.HoaDons
                    .Include(h => h.ChiTietHoaDons)
                    .ThenInclude(ct => ct.Mon)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.MaHD == _selectedMaHD.Value);

                if (hd == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin hóa đơn trong CSDL!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (hd.ChiTietHoaDons == null || hd.ChiTietHoaDons.Count == 0)
                {
                    MessageBox.Show("Hóa đơn chưa có món nào! Không thể in trang trống.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo danh sách DTO ChiTietIn theo Task 8
                var listChiTietIn = new List<ChiTietIn>();
                int stt = 1;
                foreach (var ct in hd.ChiTietHoaDons.OrderBy(c => c.MaCT))
                {
                    listChiTietIn.Add(new ChiTietIn
                    {
                        STT = stt++,
                        TenMon = ct.Mon?.TenMon ?? "",
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        ThanhTien = ct.SoLuong * ct.DonGia
                    });
                }

                Report report = new Report();
                string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "HoaDon.frx");
                if (!File.Exists(reportPath))
                {
                    reportPath = Path.Combine(Directory.GetCurrentDirectory(), "Reports", "HoaDon.frx");
                }

                if (!File.Exists(reportPath))
                {
                    MessageBox.Show("Không tìm thấy mẫu báo cáo HoaDon.frx tại: " + reportPath, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                report.Load(reportPath);

                // Đăng ký danh sách các dòng chi tiết với tên ChiTiet theo Task 8
                report.RegisterData(listChiTietIn, "ChiTiet");
                var ds = report.GetDataSource("ChiTiet");
                if (ds != null)
                {
                    ds.Enabled = true;
                }

                // Truyền các tham số theo Task 9
                report.SetParameterValue("TenQuan", "QUÁN CÀ PHÊ GÓC PHỐ");
                report.SetParameterValue("SoHD", $"HD{hd.MaHD:D6}");
                report.SetParameterValue("NgayLap", hd.NgayLap.ToString("dd/MM/yyyy HH:mm"));
                report.SetParameterValue("SoBan", $"Bàn: {hd.SoBan}");
                report.SetParameterValue("HinhThucTT", $"Hình thức: {hd.HinhThucTT}");

                report.Prepare();

                string defaultPdfName = $"HoaDon_HD{hd.MaHD:D6}.pdf";

                switch (action)
                {
                    case ReportAction.Preview:
                        using (var previewForm = new FormReportPreview(report, "FastReport Preview - HÓA ĐƠN THANH TOÁN", defaultPdfName))
                        {
                            previewForm.ShowDialog(this);
                        }
                        break;

                    case ReportAction.ExportPdf:
                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {
                            sfd.Filter = "PDF Files (*.pdf)|*.pdf";
                            sfd.Title = "Xuất PDF Hóa đơn";
                            sfd.FileName = defaultPdfName;

                            if (sfd.ShowDialog() == DialogResult.OK)
                            {
                                PDFSimpleExport export = new PDFSimpleExport();
                                report.Export(export, sfd.FileName);

                                var ans = MessageBox.Show("Xuất file PDF thành công! Bạn có muốn mở file không?",
                                    "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (ans == DialogResult.Yes)
                                {
                                    Process.Start(new ProcessStartInfo(sfd.FileName) { UseShellExecute = true });
                                }
                            }
                        }
                        break;

                    case ReportAction.Print:
                        using (var previewForm = new FormReportPreview(report, "FastReport Preview - In Hóa đơn", defaultPdfName))
                        {
                            previewForm.ShowDialog(this);
                        }
                        break;
                }
            }
            catch (IOException)
            {
                MessageBox.Show("File PDF đang được mở bởi một chương trình khác. Vui lòng đóng file rồi thử lại!",
                    "Lỗi file đang mở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xử lý hóa đơn FastReport: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
    }
}
