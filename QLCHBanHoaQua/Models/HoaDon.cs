using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua.Models
{
    class HoaDon
    {
        public string MaHD { get; set; }
        public string MaNV { get; set; }
        public string TenNV { get; set; }
        public DateTime NgayLap { get; set; }
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public int Soluong { get; set; }
        public double  DonGia { get; set; }
        public double TongTien { get; set; }
    }
}
