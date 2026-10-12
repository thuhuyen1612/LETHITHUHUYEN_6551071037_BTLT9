using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cau2_QuanCaPhe
{
    partial class FormQuanCaPhe
    {
        private IContainer components = null!;

        // Vùng tìm kiếm
        private GroupBox grpTimKiem = null!;
        private Label lblTuNgay = null!;
        private DateTimePicker dtpTuNgay = null!;
        private Label lblDenNgay = null!;
        private DateTimePicker dtpDenNgay = null!;
        private Label lblLocBan = null!;
        private ComboBox cboLocBan = null!;
        private Label lblLocHinhThuc = null!;
        private ComboBox cboLocHinhThuc = null!;
        private Label lblLocTenMon = null!;
        private TextBox txtLocTenMon = null!;
        private Button btnTim = null!;

        // Vùng trái - Danh sách hóa đơn
        private GroupBox grpDanhSachHD = null!;
        private DataGridView dgvHoaDon = null!;
        private Label lblTongDoanhThu = null!;
        private Button btnHoaDonMoi = null!;
        private Button btnSuaHoaDon = null!;
        private Button btnXoaHoaDon = null!;

        // Vùng phải - Chi tiết hóa đơn
        private GroupBox grpChiTiet = null!;
        private Label lblNgayLap = null!;
        private DateTimePicker dtpNgayLap = null!;
        private Label lblSoBan = null!;
        private ComboBox cboSoBan = null!;
        private RadioButton radTienMat = null!;
        private RadioButton radChuyenKhoan = null!;
        private Label lblGhiChu = null!;
        private TextBox txtGhiChu = null!;

        private Label lblChonMon = null!;
        private ComboBox cboChonMon = null!;
        private Label lblSL = null!;
        private NumericUpDown numSL = null!;
        private Button btnThemMon = null!;

        private DataGridView dgvChiTiet = null!;
        private Button btnSuaSL = null!;
        private Button btnXoaMon = null!;
        private Label lblTongCong = null!;

        private Button btnXemHoaDon = null!;
        private Button btnXuatPdf = null!;
        private Button btnIn = null!;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            grpTimKiem = new GroupBox();
            lblTuNgay = new Label();
            dtpTuNgay = new DateTimePicker();
            lblDenNgay = new Label();
            dtpDenNgay = new DateTimePicker();
            lblLocBan = new Label();
            cboLocBan = new ComboBox();
            lblLocHinhThuc = new Label();
            cboLocHinhThuc = new ComboBox();
            lblLocTenMon = new Label();
            txtLocTenMon = new TextBox();
            btnTim = new Button();

            grpDanhSachHD = new GroupBox();
            dgvHoaDon = new DataGridView();
            lblTongDoanhThu = new Label();
            btnHoaDonMoi = new Button();
            btnSuaHoaDon = new Button();
            btnXoaHoaDon = new Button();

            grpChiTiet = new GroupBox();
            lblNgayLap = new Label();
            dtpNgayLap = new DateTimePicker();
            lblSoBan = new Label();
            cboSoBan = new ComboBox();
            radTienMat = new RadioButton();
            radChuyenKhoan = new RadioButton();
            lblGhiChu = new Label();
            txtGhiChu = new TextBox();

            lblChonMon = new Label();
            cboChonMon = new ComboBox();
            lblSL = new Label();
            numSL = new NumericUpDown();
            btnThemMon = new Button();

            dgvChiTiet = new DataGridView();
            btnSuaSL = new Button();
            btnXoaMon = new Button();
            lblTongCong = new Label();

            btnXemHoaDon = new Button();
            btnXuatPdf = new Button();
            btnIn = new Button();

            ((ISupportInitialize)dgvHoaDon).BeginInit();
            ((ISupportInitialize)dgvChiTiet).BeginInit();
            ((ISupportInitialize)numSL).BeginInit();
            grpTimKiem.SuspendLayout();
            grpDanhSachHD.SuspendLayout();
            grpChiTiet.SuspendLayout();
            SuspendLayout();

            // FormQuanCaPhe
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1140, 680);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quán cà phê Góc Phố - Quản lý hóa đơn";
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // ================= grpTimKiem =================
            grpTimKiem.Location = new Point(12, 12);
            grpTimKiem.Name = "grpTimKiem";
            grpTimKiem.Size = new Size(1116, 75);
            grpTimKiem.TabIndex = 0;
            grpTimKiem.TabStop = false;
            grpTimKiem.Text = "Tìm kiếm hóa đơn";

            lblTuNgay.Location = new Point(15, 20);
            lblTuNgay.Name = "lblTuNgay";
            lblTuNgay.Size = new Size(60, 20);
            lblTuNgay.Text = "Từ ngày";

            dtpTuNgay.CustomFormat = "dd/MM/yyyy";
            dtpTuNgay.Format = DateTimePickerFormat.Custom;
            dtpTuNgay.Location = new Point(15, 42);
            dtpTuNgay.Name = "dtpTuNgay";
            dtpTuNgay.Size = new Size(120, 23);

            lblDenNgay.Location = new Point(150, 20);
            lblDenNgay.Name = "lblDenNgay";
            lblDenNgay.Size = new Size(65, 20);
            lblDenNgay.Text = "Đến ngày";

            dtpDenNgay.CustomFormat = "dd/MM/yyyy";
            dtpDenNgay.Format = DateTimePickerFormat.Custom;
            dtpDenNgay.Location = new Point(150, 42);
            dtpDenNgay.Name = "dtpDenNgay";
            dtpDenNgay.Size = new Size(120, 23);

            lblLocBan.Location = new Point(285, 20);
            lblLocBan.Name = "lblLocBan";
            lblLocBan.Size = new Size(60, 20);
            lblLocBan.Text = "Số bàn";

            cboLocBan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocBan.Location = new Point(285, 42);
            cboLocBan.Name = "cboLocBan";
            cboLocBan.Size = new Size(110, 23);

            lblLocHinhThuc.Location = new Point(410, 20);
            lblLocHinhThuc.Name = "lblLocHinhThuc";
            lblLocHinhThuc.Size = new Size(70, 20);
            lblLocHinhThuc.Text = "Hình thức";

            cboLocHinhThuc.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocHinhThuc.Location = new Point(410, 42);
            cboLocHinhThuc.Name = "cboLocHinhThuc";
            cboLocHinhThuc.Size = new Size(130, 23);

            lblLocTenMon.Location = new Point(555, 20);
            lblLocTenMon.Name = "lblLocTenMon";
            lblLocTenMon.Size = new Size(70, 20);
            lblLocTenMon.Text = "Tên món";

            txtLocTenMon.Location = new Point(555, 42);
            txtLocTenMon.Name = "txtLocTenMon";
            txtLocTenMon.PlaceholderText = "Nhập món...";
            txtLocTenMon.Size = new Size(180, 23);

            btnTim.Location = new Point(750, 40);
            btnTim.Name = "btnTim";
            btnTim.Size = new Size(80, 26);
            btnTim.Text = "Tìm";
            btnTim.UseVisualStyleBackColor = true;

            grpTimKiem.Controls.Add(lblTuNgay);
            grpTimKiem.Controls.Add(dtpTuNgay);
            grpTimKiem.Controls.Add(lblDenNgay);
            grpTimKiem.Controls.Add(dtpDenNgay);
            grpTimKiem.Controls.Add(lblLocBan);
            grpTimKiem.Controls.Add(cboLocBan);
            grpTimKiem.Controls.Add(lblLocHinhThuc);
            grpTimKiem.Controls.Add(cboLocHinhThuc);
            grpTimKiem.Controls.Add(lblLocTenMon);
            grpTimKiem.Controls.Add(txtLocTenMon);
            grpTimKiem.Controls.Add(btnTim);

            // ================= grpDanhSachHD (Vùng trái) =================
            grpDanhSachHD.Location = new Point(12, 95);
            grpDanhSachHD.Name = "grpDanhSachHD";
            grpDanhSachHD.Size = new Size(490, 560);
            grpDanhSachHD.TabIndex = 1;
            grpDanhSachHD.TabStop = false;
            grpDanhSachHD.Text = "Danh sách hóa đơn";

            dgvHoaDon.AllowUserToAddRows = false;
            dgvHoaDon.AllowUserToDeleteRows = false;
            dgvHoaDon.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHoaDon.BackgroundColor = Color.White;
            dgvHoaDon.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHoaDon.Location = new Point(10, 25);
            dgvHoaDon.MultiSelect = false;
            dgvHoaDon.Name = "dgvHoaDon";
            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.RowHeadersWidth = 30;
            dgvHoaDon.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHoaDon.Size = new Size(470, 420);

            lblTongDoanhThu.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point);
            lblTongDoanhThu.ForeColor = Color.DarkRed;
            lblTongDoanhThu.Location = new Point(10, 455);
            lblTongDoanhThu.Name = "lblTongDoanhThu";
            lblTongDoanhThu.Size = new Size(470, 30);
            lblTongDoanhThu.Text = "Tổng doanh thu đang hiển thị: 0 đ";
            lblTongDoanhThu.TextAlign = ContentAlignment.MiddleLeft;

            btnHoaDonMoi.Location = new Point(10, 500);
            btnHoaDonMoi.Name = "btnHoaDonMoi";
            btnHoaDonMoi.Size = new Size(110, 32);
            btnHoaDonMoi.Text = "Hóa đơn mới";
            btnHoaDonMoi.UseVisualStyleBackColor = true;

            btnSuaHoaDon.Location = new Point(130, 500);
            btnSuaHoaDon.Name = "btnSuaHoaDon";
            btnSuaHoaDon.Size = new Size(110, 32);
            btnSuaHoaDon.Text = "Sửa hóa đơn";
            btnSuaHoaDon.UseVisualStyleBackColor = true;

            btnXoaHoaDon.Location = new Point(250, 500);
            btnXoaHoaDon.Name = "btnXoaHoaDon";
            btnXoaHoaDon.Size = new Size(110, 32);
            btnXoaHoaDon.Text = "Xóa hóa đơn";
            btnXoaHoaDon.UseVisualStyleBackColor = true;

            grpDanhSachHD.Controls.Add(dgvHoaDon);
            grpDanhSachHD.Controls.Add(lblTongDoanhThu);
            grpDanhSachHD.Controls.Add(btnHoaDonMoi);
            grpDanhSachHD.Controls.Add(btnSuaHoaDon);
            grpDanhSachHD.Controls.Add(btnXoaHoaDon);

            // ================= grpChiTiet (Vùng phải) =================
            grpChiTiet.Location = new Point(512, 95);
            grpChiTiet.Name = "grpChiTiet";
            grpChiTiet.Size = new Size(616, 560);
            grpChiTiet.TabIndex = 2;
            grpChiTiet.TabStop = false;
            grpChiTiet.Text = "Chi tiết hóa đơn";

            lblNgayLap.Location = new Point(15, 25);
            lblNgayLap.Name = "lblNgayLap";
            lblNgayLap.Size = new Size(60, 23);
            lblNgayLap.Text = "Ngày lập:";
            lblNgayLap.TextAlign = ContentAlignment.MiddleLeft;

            dtpNgayLap.CustomFormat = "dd/MM/yyyy HH:mm";
            dtpNgayLap.Format = DateTimePickerFormat.Custom;
            dtpNgayLap.Location = new Point(80, 25);
            dtpNgayLap.Name = "dtpNgayLap";
            dtpNgayLap.Size = new Size(140, 23);

            lblSoBan.Location = new Point(230, 25);
            lblSoBan.Name = "lblSoBan";
            lblSoBan.Size = new Size(50, 23);
            lblSoBan.Text = "Số bàn:";
            lblSoBan.TextAlign = ContentAlignment.MiddleLeft;

            cboSoBan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboSoBan.Location = new Point(285, 25);
            cboSoBan.Name = "cboSoBan";
            cboSoBan.Size = new Size(70, 23);

            radTienMat.AutoSize = true;
            radTienMat.Checked = true;
            radTienMat.Location = new Point(375, 27);
            radTienMat.Name = "radTienMat";
            radTienMat.Size = new Size(71, 19);
            radTienMat.Text = "Tiền mặt";
            radTienMat.UseVisualStyleBackColor = true;

            radChuyenKhoan.AutoSize = true;
            radChuyenKhoan.Location = new Point(460, 27);
            radChuyenKhoan.Name = "radChuyenKhoan";
            radChuyenKhoan.Size = new Size(102, 19);
            radChuyenKhoan.Text = "Chuyển khoản";
            radChuyenKhoan.UseVisualStyleBackColor = true;

            lblGhiChu.Location = new Point(15, 60);
            lblGhiChu.Name = "lblGhiChu";
            lblGhiChu.Size = new Size(60, 23);
            lblGhiChu.Text = "Ghi chú:";
            lblGhiChu.TextAlign = ContentAlignment.MiddleLeft;

            txtGhiChu.Location = new Point(80, 60);
            txtGhiChu.Multiline = true;
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.ScrollBars = ScrollBars.Vertical;
            txtGhiChu.Size = new Size(515, 45);

            // Controls thêm món
            lblChonMon.Location = new Point(15, 120);
            lblChonMon.Name = "lblChonMon";
            lblChonMon.Size = new Size(70, 23);
            lblChonMon.Text = "Chọn món:";
            lblChonMon.TextAlign = ContentAlignment.MiddleLeft;

            cboChonMon.DropDownStyle = ComboBoxStyle.DropDownList;
            cboChonMon.Location = new Point(85, 120);
            cboChonMon.Name = "cboChonMon";
            cboChonMon.Size = new Size(240, 23);

            lblSL.Location = new Point(335, 120);
            lblSL.Name = "lblSL";
            lblSL.Size = new Size(30, 23);
            lblSL.Text = "SL:";
            lblSL.TextAlign = ContentAlignment.MiddleLeft;

            numSL.Location = new Point(370, 120);
            numSL.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            numSL.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numSL.Name = "numSL";
            numSL.Size = new Size(55, 23);
            numSL.Value = new decimal(new int[] { 1, 0, 0, 0 });

            btnThemMon.Location = new Point(440, 118);
            btnThemMon.Name = "btnThemMon";
            btnThemMon.Size = new Size(90, 27);
            btnThemMon.Text = "Thêm món";
            btnThemMon.UseVisualStyleBackColor = true;

            // dgvChiTiet
            dgvChiTiet.AllowUserToAddRows = false;
            dgvChiTiet.AllowUserToDeleteRows = false;
            dgvChiTiet.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvChiTiet.BackgroundColor = Color.White;
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.Location = new Point(15, 160);
            dgvChiTiet.MultiSelect = false;
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.ReadOnly = true;
            dgvChiTiet.RowHeadersWidth = 30;
            dgvChiTiet.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvChiTiet.Size = new Size(580, 240);

            // Các nút thao tác chi tiết món
            btnSuaSL.Location = new Point(15, 410);
            btnSuaSL.Name = "btnSuaSL";
            btnSuaSL.Size = new Size(80, 30);
            btnSuaSL.Text = "Sửa SL";
            btnSuaSL.UseVisualStyleBackColor = true;

            btnXoaMon.Location = new Point(105, 410);
            btnXoaMon.Name = "btnXoaMon";
            btnXoaMon.Size = new Size(80, 30);
            btnXoaMon.Text = "Xóa món";
            btnXoaMon.UseVisualStyleBackColor = true;

            lblTongCong.Font = new Font("Segoe UI", 13F, FontStyle.Bold, GraphicsUnit.Point);
            lblTongCong.ForeColor = Color.DarkBlue;
            lblTongCong.Location = new Point(230, 410);
            lblTongCong.Name = "lblTongCong";
            lblTongCong.Size = new Size(365, 30);
            lblTongCong.Text = "Tổng cộng: 0 đ";
            lblTongCong.TextAlign = ContentAlignment.MiddleRight;

            // Nút báo cáo FastReport
            btnXemHoaDon.Location = new Point(15, 460);
            btnXemHoaDon.Name = "btnXemHoaDon";
            btnXemHoaDon.Size = new Size(110, 34);
            btnXemHoaDon.Text = "Xem hóa đơn";
            btnXemHoaDon.UseVisualStyleBackColor = true;

            btnXuatPdf.Location = new Point(135, 460);
            btnXuatPdf.Name = "btnXuatPdf";
            btnXuatPdf.Size = new Size(100, 34);
            btnXuatPdf.Text = "Xuất PDF";
            btnXuatPdf.UseVisualStyleBackColor = true;

            btnIn.Location = new Point(245, 460);
            btnIn.Name = "btnIn";
            btnIn.Size = new Size(80, 34);
            btnIn.Text = "In";
            btnIn.UseVisualStyleBackColor = true;

            grpChiTiet.Controls.Add(lblNgayLap);
            grpChiTiet.Controls.Add(dtpNgayLap);
            grpChiTiet.Controls.Add(lblSoBan);
            grpChiTiet.Controls.Add(cboSoBan);
            grpChiTiet.Controls.Add(radTienMat);
            grpChiTiet.Controls.Add(radChuyenKhoan);
            grpChiTiet.Controls.Add(lblGhiChu);
            grpChiTiet.Controls.Add(txtGhiChu);
            grpChiTiet.Controls.Add(lblChonMon);
            grpChiTiet.Controls.Add(cboChonMon);
            grpChiTiet.Controls.Add(lblSL);
            grpChiTiet.Controls.Add(numSL);
            grpChiTiet.Controls.Add(btnThemMon);
            grpChiTiet.Controls.Add(dgvChiTiet);
            grpChiTiet.Controls.Add(btnSuaSL);
            grpChiTiet.Controls.Add(btnXoaMon);
            grpChiTiet.Controls.Add(lblTongCong);
            grpChiTiet.Controls.Add(btnXemHoaDon);
            grpChiTiet.Controls.Add(btnXuatPdf);
            grpChiTiet.Controls.Add(btnIn);

            // Form Controls
            Controls.Add(grpTimKiem);
            Controls.Add(grpDanhSachHD);
            Controls.Add(grpChiTiet);

            ((ISupportInitialize)dgvHoaDon).EndInit();
            ((ISupportInitialize)dgvChiTiet).EndInit();
            ((ISupportInitialize)numSL).EndInit();
            grpTimKiem.ResumeLayout(false);
            grpTimKiem.PerformLayout();
            grpDanhSachHD.ResumeLayout(false);
            grpChiTiet.ResumeLayout(false);
            grpChiTiet.PerformLayout();
            ResumeLayout(false);
        }
    }
}
