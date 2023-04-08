using Framework;
using QLCHBanHoaQua.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua.Controllers
{
    class ThongKeController
    {
        private const string LichSuFile = @"data\lichsunhap.txt";
        public HoaDonController hoaDonController { get;private set; }
        public SanPhamController sanPhamController { get; set; }
        public NhanVienController nhanVienController { get; set; }
        public KhachHangController khachHangController { get; set; }
        public static List<LichSuNhap> lichSuNhaps { get; private set; }
        public ThongKeController(HoaDonController _hoaDonController,SanPhamController _sanPhamController,NhanVienController _nhanVienController,KhachHangController _khachHangController)
        {
            hoaDonController = _hoaDonController;
            sanPhamController = _sanPhamController;
            nhanVienController = _nhanVienController;
            khachHangController = _khachHangController;
            lichSuNhaps = new List<LichSuNhap>();
            Load(lichSuNhaps);
        }
        private void Load(List<LichSuNhap> lichSuNhaps)
        {
            using (var fs = new FileStream(LichSuFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var tmp = line.Trim().Split("|");
                        LichSuNhap lichSuNhap = new LichSuNhap();
                        lichSuNhap.MaSP = tmp[0];
                        lichSuNhap.TenSP = tmp[1];
                        lichSuNhap.SoLuong = int.Parse(tmp[2]);
                        lichSuNhap.GiaNhap = double.Parse(tmp[3]);
                        lichSuNhap.ThoiGian = DateTime.Parse(tmp[4]);
                        lichSuNhaps.Add(lichSuNhap);
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        ViewHelper.PrintError("Dữ liệu trong file sai định dạng.");
                        Environment.Exit(0);
                    }

                }
            }
        }
        public void ThongKeDoanhThu()
        {
            Console.Clear();
            int soLuongNhap = lichSuNhaps.Sum(x=>x.SoLuong);
            int soLuongBan = HoaDonController.hoaDons.Sum(x => x.Soluong);
            int soLuongConLai = SanPhamController.sanPhams.Sum(x => x.SoLuong);
            double doanhThu = HoaDonController.hoaDons.Sum(x => x.TongTien);
            double tienVon = ThongKeController.lichSuNhaps.Sum(x => x.GiaNhap);
            double tienLai = doanhThu - tienVon;
            ViewHelper.PrintSuccess($"Tống số lượng hoa quả đã nhâp là {soLuongNhap}");
            ViewHelper.PrintSuccess($"Tống số lượng hoa quả đã bán là {soLuongBan}");
            ViewHelper.PrintSuccess($"Tống số lượng hoa quả còn lại trong kho là {soLuongConLai}");
            ViewHelper.PrintSuccess($"Tổng doanh thu của cửa hàng là {doanhThu:c0}");
            ViewHelper.PrintSuccess($"Tổng tiền nhập các loại hoa quả của cửa hàng là {tienVon:c0}");
            ViewHelper.PrintSuccess($"Tổng tiền lãi của cửa hàng là {tienLai:c0}");
            ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
            Console.ReadKey(true);
        }
        public void ThongKeSanPham()
        {
            var sanPhams = HoaDonController.hoaDons.GroupBy(x => x.MaSP).Select(y => new
            {
                MaSP = y.Key,
                SoLuongDaBan = y.Sum(z=>z.Soluong)
            }).ToList();
            Console.WriteLine("╔══════════╦════════════════════╦═══════════════╗");
            Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Số lượng Đã Bán",-15}║");
            Console.WriteLine("╠══════════╬════════════════════╬═══════════════╣");
            var last = sanPhams.OrderByDescending(x=>x.SoLuongDaBan).Last();
            foreach (var sanPham in sanPhams.OrderByDescending(x => x.SoLuongDaBan))
            {
                if (sanPham.Equals(last))
                {
                    Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPhamController.FindSP(sanPham.MaSP).TenSP,-20}║{sanPham.SoLuongDaBan,-15}║");
                    Console.WriteLine("╚══════════╩════════════════════╩═══════════════╝");
                }
                else
                {
                    Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPhamController.FindSP(sanPham.MaSP).TenSP,-20}║{sanPham.SoLuongDaBan,-15}║");
                    Console.WriteLine("╠══════════╬════════════════════╬═══════════════╣");
                }
            }
            ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
            Console.ReadKey(true);
        }
        public void ThongKeNhanVien()
        {
            var nhanViens = HoaDonController.hoaDons.GroupBy(x => x.MaNV).Select(y => new
            {
                MaNV = y.Key,
                SoDonDaGhi = y.Count(z => z.MaNV == y.Key)
            }).ToList();
            Console.WriteLine("╔═══════════════╦══════════════════════════════╦═══════════════╗");
            Console.WriteLine($"║{"Mã Nhân Viên",-15}║{"Tên Nhân Viên",-30}║{"Số Đơn Đã Ghi",-15}║");
            Console.WriteLine("╠═══════════════╬══════════════════════════════╬═══════════════╣");
            var last = nhanViens.OrderByDescending(x => x.SoDonDaGhi).Last();
            foreach (var nhanVien in nhanViens.OrderByDescending(x => x.SoDonDaGhi))
            {
                if (nhanVien.Equals(last))
                {
                    Console.WriteLine($"║{nhanVien.MaNV,-15}║{nhanVienController.FindNV(nhanVien.MaNV).TenNV,-30}║{nhanVien.SoDonDaGhi,-15}║");
                    Console.WriteLine("╚═══════════════╩══════════════════════════════╩═══════════════╝");
                }
                else
                {
                    Console.WriteLine($"║{nhanVien.MaNV,-15}║{sanPhamController.FindSP(nhanVien.MaNV).TenSP,-30}║{nhanVien.SoDonDaGhi,-15}║");
                    Console.WriteLine("╠═══════════════╬══════════════════════════════╬═══════════════╣");
                }
            }
            ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
            Console.ReadKey(true);
        }
        public void ThongKeKhachHang()
        {
            var khachHangs = HoaDonController.hoaDons.GroupBy(x => x.MaKH).Select(y =>
            {
                var sanPham = y.GroupBy(z => z.MaSP).Select(t => new { 
                    MaSp = t.Key,
                    SoLuong = t.Sum(ta => ta.Soluong) }).ToList().MaxBy(soluong=>soluong.SoLuong).MaSp;
                return new
                {
                    MaKH = y.Key,
                    SanPhamMuaNhieuNhat = sanPham,
                    SoLuongDonDaMua = y.Count(z => z.MaKH == y.Key)
                };
            }).ToList();
            Console.WriteLine("╔═══════════════╦════════════════════╦═══════════════╦═════════════════════════╗");
            Console.WriteLine($"║{"Mã Khách Hàng",-15}║{"Tên Khách Hàng",-20}║{"Số Đơn Đã Mua",-15}║{"Sản phâm mua nhiều nhất",-25}║");
            Console.WriteLine("╠═══════════════╬════════════════════╬═══════════════╬═════════════════════════╣");
            var last = khachHangs.Last();
            foreach (var khachHang in khachHangs)
            {
                if (khachHang.Equals(last))
                {
                    Console.WriteLine($"║{khachHang.MaKH,-15}║{khachHangController.FindKH(khachHang.MaKH).TenKH,-20}║{khachHang.SoLuongDonDaMua,-15}║{khachHang.SanPhamMuaNhieuNhat,-25}║");
                    Console.WriteLine("╚═══════════════╩════════════════════╩═══════════════╩═════════════════════════╝");
                }
                else
                {
                    Console.WriteLine($"║{khachHang.MaKH,-15}║{khachHangController.FindKH(khachHang.MaKH).TenKH,-20}║{khachHang.SoLuongDonDaMua,-15}║{khachHang.SanPhamMuaNhieuNhat,-25}║");
                    Console.WriteLine("╠═══════════════╬════════════════════╬═══════════════╬═════════════════════════╣");
                }
            }
            ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
            Console.ReadKey(true);
        }

    }
}
