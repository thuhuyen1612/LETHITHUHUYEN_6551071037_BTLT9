using System.Collections.Generic;

namespace Cau2_QuanCaPhe.Models
{
    public class Mon
    {
        public int MaMon { get; set; }
        public string TenMon { get; set; } = string.Empty;
        public string? NhomMon { get; set; }
        public decimal DonGia { get; set; }

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
    }
}
