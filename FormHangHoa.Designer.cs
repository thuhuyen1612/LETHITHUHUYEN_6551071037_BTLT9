using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Cau1_TiemTapHoa
{
    partial class FormHangHoa
    {
        private IContainer components = null!;

        private GroupBox grpThongTin = null!;
        private Label lblTenHang = null!;
        private TextBox txtTenHang = null!;
        private Label lblLoaiHang = null!;
        private ComboBox cboLoaiHang = null!;
        private Label lblDonVi = null!;
        private ComboBox cboDonVi = null!;
        private Label lblGiaBan = null!;
        private NumericUpDown numGiaBan = null!;
        private Label lblSoLuongTon = null!;
        private NumericUpDown numSoLuongTon = null!;
        private Label lblNgayNhap = null!;
        private DateTimePicker dtpNgayNhap = null!;
        private CheckBox chkConKinhDoanh = null!;

        private GroupBox grpTimKiem = null!;
        private Label lblTimTen = null!;
        private TextBox txtTimKiem = null!;
        private ComboBox cboLocLoaiHang = null!;
        private CheckBox chkLocConKinhDoanh = null!;

        private DataGridView dgvHangHoa = null!;
        private StatusStrip statusStrip = null!;
        private ToolStripStatusLabel lblTrangThai = null!;

        private Button btnThem = null!;
        private Button btnSua = null!;
        private Button btnXoa = null!;
        private Button btnLamMoi = null!;
        private Button btnXemBangGia = null!;
        private Button btnXuatPdf = null!;
        private Button btnXuatExcelLuoi = null!;
        private Button btnXuatExcelTonKho = null!;

        private System.Windows.Forms.Timer tmrSearch = null!;

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
            components = new Container();
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            DataGridViewCellStyle rightAlignStyle = new DataGridViewCellStyle();

            grpThongTin = new GroupBox();
            lblTenHang = new Label();
            txtTenHang = new TextBox();
            lblLoaiHang = new Label();
            cboLoaiHang = new ComboBox();
            lblDonVi = new Label();
            cboDonVi = new ComboBox();
            lblGiaBan = new Label();
            numGiaBan = new NumericUpDown();
            lblSoLuongTon = new Label();
            numSoLuongTon = new NumericUpDown();
            lblNgayNhap = new Label();
            dtpNgayNhap = new DateTimePicker();
            chkConKinhDoanh = new CheckBox();

            grpTimKiem = new GroupBox();
            lblTimTen = new Label();
            txtTimKiem = new TextBox();
            cboLocLoaiHang = new ComboBox();
            chkLocConKinhDoanh = new CheckBox();

            dgvHangHoa = new DataGridView();
            statusStrip = new StatusStrip();
            lblTrangThai = new ToolStripStatusLabel();

            btnThem = new Button();
            btnSua = new Button();
            btnXoa = new Button();
            btnLamMoi = new Button();
            btnXemBangGia = new Button();
            btnXuatPdf = new Button();
            btnXuatExcelLuoi = new Button();
            btnXuatExcelTonKho = new Button();

            tmrSearch = new System.Windows.Forms.Timer(components);

            ((ISupportInitialize)numGiaBan).BeginInit();
            ((ISupportInitialize)numSoLuongTon).BeginInit();
            ((ISupportInitialize)dgvHangHoa).BeginInit();
            grpThongTin.SuspendLayout();
            grpTimKiem.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();

            // FormHangHoa
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1008, 620);
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Quản lý hàng hóa - Tạp hóa UTC2";
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);

            // GroupBox Thông tin hàng hóa
            grpThongTin.Location = new Point(12, 12);
            grpThongTin.Name = "grpThongTin";
            grpThongTin.Size = new Size(570, 165);
            grpThongTin.TabIndex = 0;
            grpThongTin.TabStop = false;
            grpThongTin.Text = "Thông tin hàng hóa";

            // lblTenHang & txtTenHang
            lblTenHang.Location = new Point(15, 25);
            lblTenHang.Name = "lblTenHang";
            lblTenHang.Size = new Size(70, 23);
            lblTenHang.Text = "Tên hàng:";
            lblTenHang.TextAlign = ContentAlignment.MiddleLeft;

            txtTenHang.Location = new Point(90, 25);
            txtTenHang.Name = "txtTenHang";
            txtTenHang.Size = new Size(465, 23);
            txtTenHang.TabIndex = 0;

            // lblLoaiHang & cboLoaiHang
            lblLoaiHang.Location = new Point(15, 58);
            lblLoaiHang.Name = "lblLoaiHang";
            lblLoaiHang.Size = new Size(70, 23);
            lblLoaiHang.Text = "Loại hàng:";
            lblLoaiHang.TextAlign = ContentAlignment.MiddleLeft;

            cboLoaiHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiHang.FormattingEnabled = true;
            cboLoaiHang.Location = new Point(90, 58);
            cboLoaiHang.Name = "cboLoaiHang";
            cboLoaiHang.Size = new Size(180, 23);
            cboLoaiHang.TabIndex = 1;

            // lblDonVi & cboDonVi
            lblDonVi.Location = new Point(15, 93);
            lblDonVi.Name = "lblDonVi";
            lblDonVi.Size = new Size(70, 23);
            lblDonVi.Text = "Đơn vị:";
            lblDonVi.TextAlign = ContentAlignment.MiddleLeft;

            cboDonVi.DropDownStyle = ComboBoxStyle.DropDownList;
            cboDonVi.FormattingEnabled = true;
            cboDonVi.Location = new Point(90, 93);
            cboDonVi.Name = "cboDonVi";
            cboDonVi.Size = new Size(180, 23);
            cboDonVi.TabIndex = 2;

            // lblGiaBan & numGiaBan
            lblGiaBan.Location = new Point(15, 128);
            lblGiaBan.Name = "lblGiaBan";
            lblGiaBan.Size = new Size(70, 23);
            lblGiaBan.Text = "Giá bán:";
            lblGiaBan.TextAlign = ContentAlignment.MiddleLeft;

            numGiaBan.Increment = new decimal(new int[] { 500, 0, 0, 0 });
            numGiaBan.Location = new Point(90, 128);
            numGiaBan.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            numGiaBan.Name = "numGiaBan";
            numGiaBan.Size = new Size(180, 23);
            numGiaBan.TabIndex = 3;
            numGiaBan.ThousandsSeparator = true;

            // lblSoLuongTon & numSoLuongTon
            lblSoLuongTon.Location = new Point(290, 58);
            lblSoLuongTon.Name = "lblSoLuongTon";
            lblSoLuongTon.Size = new Size(85, 23);
            lblSoLuongTon.Text = "Số lượng tồn:";
            lblSoLuongTon.TextAlign = ContentAlignment.MiddleLeft;

            numSoLuongTon.Location = new Point(380, 58);
            numSoLuongTon.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            numSoLuongTon.Name = "numSoLuongTon";
            numSoLuongTon.Size = new Size(175, 23);
            numSoLuongTon.TabIndex = 4;
            numSoLuongTon.ThousandsSeparator = true;

            // lblNgayNhap & dtpNgayNhap
            lblNgayNhap.Location = new Point(290, 93);
            lblNgayNhap.Name = "lblNgayNhap";
            lblNgayNhap.Size = new Size(85, 23);
            lblNgayNhap.Text = "Ngày nhập:";
            lblNgayNhap.TextAlign = ContentAlignment.MiddleLeft;

            dtpNgayNhap.CustomFormat = "dd/MM/yyyy";
            dtpNgayNhap.Format = DateTimePickerFormat.Custom;
            dtpNgayNhap.Location = new Point(380, 93);
            dtpNgayNhap.Name = "dtpNgayNhap";
            dtpNgayNhap.Size = new Size(175, 23);
            dtpNgayNhap.TabIndex = 5;

            // chkConKinhDoanh
            chkConKinhDoanh.AutoSize = true;
            chkConKinhDoanh.Checked = true;
            chkConKinhDoanh.CheckState = CheckState.Checked;
            chkConKinhDoanh.Location = new Point(380, 130);
            chkConKinhDoanh.Name = "chkConKinhDoanh";
            chkConKinhDoanh.Size = new Size(111, 19);
            chkConKinhDoanh.TabIndex = 6;
            chkConKinhDoanh.Text = "Còn kinh doanh";
            chkConKinhDoanh.UseVisualStyleBackColor = true;

            grpThongTin.Controls.Add(lblTenHang);
            grpThongTin.Controls.Add(txtTenHang);
            grpThongTin.Controls.Add(lblLoaiHang);
            grpThongTin.Controls.Add(cboLoaiHang);
            grpThongTin.Controls.Add(lblDonVi);
            grpThongTin.Controls.Add(cboDonVi);
            grpThongTin.Controls.Add(lblGiaBan);
            grpThongTin.Controls.Add(numGiaBan);
            grpThongTin.Controls.Add(lblSoLuongTon);
            grpThongTin.Controls.Add(numSoLuongTon);
            grpThongTin.Controls.Add(lblNgayNhap);
            grpThongTin.Controls.Add(dtpNgayNhap);
            grpThongTin.Controls.Add(chkConKinhDoanh);

            // GroupBox Tìm kiếm
            grpTimKiem.Location = new Point(595, 12);
            grpTimKiem.Name = "grpTimKiem";
            grpTimKiem.Size = new Size(401, 165);
            grpTimKiem.TabIndex = 1;
            grpTimKiem.TabStop = false;
            grpTimKiem.Text = "Tìm kiếm";

            txtTimKiem.Location = new Point(15, 25);
            txtTimKiem.Name = "txtTimKiem";
            txtTimKiem.PlaceholderText = "Nhập tên hàng cần tìm...";
            txtTimKiem.Size = new Size(370, 23);
            txtTimKiem.TabIndex = 0;

            cboLocLoaiHang.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLocLoaiHang.FormattingEnabled = true;
            cboLocLoaiHang.Location = new Point(15, 60);
            cboLocLoaiHang.Name = "cboLocLoaiHang";
            cboLocLoaiHang.Size = new Size(370, 23);
            cboLocLoaiHang.TabIndex = 1;

            chkLocConKinhDoanh.AutoSize = true;
            chkLocConKinhDoanh.Location = new Point(15, 100);
            chkLocConKinhDoanh.Name = "chkLocConKinhDoanh";
            chkLocConKinhDoanh.Size = new Size(177, 19);
            chkLocConKinhDoanh.TabIndex = 2;
            chkLocConKinhDoanh.Text = "Chỉ hiện hàng còn kinh doanh";
            chkLocConKinhDoanh.UseVisualStyleBackColor = true;

            grpTimKiem.Controls.Add(txtTimKiem);
            grpTimKiem.Controls.Add(cboLocLoaiHang);
            grpTimKiem.Controls.Add(chkLocConKinhDoanh);

            // DataGridView dgvHangHoa
            dgvHangHoa.AllowUserToAddRows = false;
            dgvHangHoa.AllowUserToDeleteRows = false;
            dgvHangHoa.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvHangHoa.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvHangHoa.BackgroundColor = Color.White;
            dgvHangHoa.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvHangHoa.Location = new Point(12, 190);
            dgvHangHoa.MultiSelect = false;
            dgvHangHoa.Name = "dgvHangHoa";
            dgvHangHoa.ReadOnly = true;
            dgvHangHoa.RowHeadersWidth = 35;
            dgvHangHoa.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvHangHoa.Size = new Size(984, 335);
            dgvHangHoa.TabIndex = 2;

            // Buttons
            int btnY = 540;
            int btnH = 32;

            btnThem.Location = new Point(12, btnY);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(80, btnH);
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;

            btnSua.Location = new Point(98, btnY);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(80, btnH);
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;

            btnXoa.Location = new Point(184, btnY);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(80, btnH);
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;

            btnLamMoi.Location = new Point(270, btnY);
            btnLamMoi.Name = "btnLamMoi";
            btnLamMoi.Size = new Size(85, btnH);
            btnLamMoi.Text = "Làm mới";
            btnLamMoi.UseVisualStyleBackColor = true;

            btnXemBangGia.Location = new Point(380, btnY);
            btnXemBangGia.Name = "btnXemBangGia";
            btnXemBangGia.Size = new Size(110, btnH);
            btnXemBangGia.Text = "Xem bảng giá";
            btnXemBangGia.UseVisualStyleBackColor = true;

            btnXuatPdf.Location = new Point(496, btnY);
            btnXuatPdf.Name = "btnXuatPdf";
            btnXuatPdf.Size = new Size(95, btnH);
            btnXuatPdf.Text = "Xuất PDF";
            btnXuatPdf.UseVisualStyleBackColor = true;

            btnXuatExcelLuoi.Location = new Point(620, btnY);
            btnXuatExcelLuoi.Name = "btnXuatExcelLuoi";
            btnXuatExcelLuoi.Size = new Size(140, btnH);
            btnXuatExcelLuoi.Text = "Xuất Excel (lưới)";
            btnXuatExcelLuoi.UseVisualStyleBackColor = true;

            btnXuatExcelTonKho.Location = new Point(766, btnY);
            btnXuatExcelTonKho.Name = "btnXuatExcelTonKho";
            btnXuatExcelTonKho.Size = new Size(150, btnH);
            btnXuatExcelTonKho.Text = "Xuất Excel tồn kho";
            btnXuatExcelTonKho.UseVisualStyleBackColor = true;

            // StatusStrip
            statusStrip.Items.AddRange(new ToolStripItem[] { lblTrangThai });
            statusStrip.Location = new Point(0, 598);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(1008, 22);
            statusStrip.TabIndex = 3;

            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(118, 17);
            lblTrangThai.Text = "Sẵn sàng";

            // Controls add
            Controls.Add(grpThongTin);
            Controls.Add(grpTimKiem);
            Controls.Add(dgvHangHoa);
            Controls.Add(btnThem);
            Controls.Add(btnSua);
            Controls.Add(btnXoa);
            Controls.Add(btnLamMoi);
            Controls.Add(btnXemBangGia);
            Controls.Add(btnXuatPdf);
            Controls.Add(btnXuatExcelLuoi);
            Controls.Add(btnXuatExcelTonKho);
            Controls.Add(statusStrip);

            ((ISupportInitialize)numGiaBan).EndInit();
            ((ISupportInitialize)numSoLuongTon).EndInit();
            ((ISupportInitialize)dgvHangHoa).EndInit();
            grpThongTin.ResumeLayout(false);
            grpThongTin.PerformLayout();
            grpTimKiem.ResumeLayout(false);
            grpTimKiem.PerformLayout();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
