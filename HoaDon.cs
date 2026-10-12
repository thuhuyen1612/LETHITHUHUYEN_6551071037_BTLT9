using System;
using System.Collections.Generic;

namespace Cau2_QuanCaPhe.Models
{
    public class HoaDon
    {
        public int MaHD { get; set; }
        public DateTime NgayLap { get; set; }
        public int SoBan { get; set; }
        public string HinhThucTT { get; set; } = "Tiền mặt";
        public string? GhiChu { get; set; }

        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
    }
}
