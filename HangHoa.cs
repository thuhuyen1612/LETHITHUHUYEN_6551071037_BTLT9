using System;

namespace Cau1_TiemTapHoa.Models
{
    public class HangHoa
    {
        public int MaHang { get; set; }
        public string TenHang { get; set; } = string.Empty;
        public string? LoaiHang { get; set; }
        public string? DonVi { get; set; }
        public decimal GiaBan { get; set; }
        public int SoLuongTon { get; set; }
        public DateTime? NgayNhap { get; set; }
        public bool ConKinhDoanh { get; set; } = true;
    }
}
