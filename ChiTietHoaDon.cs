namespace Cau2_QuanCaPhe.Models
{
    public class ChiTietHoaDon
    {
        public int MaCT { get; set; }
        public int MaHD { get; set; }
        public int MaMon { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }

        public virtual HoaDon HoaDon { get; set; } = null!;
        public virtual Mon Mon { get; set; } = null!;
    }
}
