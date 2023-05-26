using Framework;
using QLCHBanHoaQua.Models;
using QLCHBanHoaQua.Views;
using System.Text.RegularExpressions;

namespace QLCHBanHoaQua.Controllers
{
    class ThongKeController
    {
        private const string LichSuFile = @"data\lichsunhap.txt";
        public HoaDonController hoaDonController { get; private set; }
        public SanPhamController sanPhamController { get; private set; }
        public NhanVienController nhanVienController { get; private set; }
        public KhachHangController khachHangController { get; private set; }
        public static List<LichSuNhap> lichSuNhaps { get; private set; }
        public ThongKeController(HoaDonController _hoaDonController, SanPhamController _sanPhamController, NhanVienController _nhanVienController, KhachHangController _khachHangController)
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
                        lichSuNhap.SoLuong = int.Parse(tmp[1]);
                        lichSuNhap.ThoiGian = DateTime.Parse(tmp[2]);
                    }
                    catch (Exception)
                    {
                        Console.Clear();
                        ViewHelper.PrintError("Dữ liệu trong file sai định dạng.");
                        Thread.Sleep(4000);
                        Environment.Exit(0);
                    }

                }
            }
        }
        public void ThongKeDoanhThu()
        {
            try
            {
                bool IsIncorrect = false;
            Menu:
                Menu.MenuLuaChonThongKe();
            ReadKey:
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    var pos_day = ViewHelper.PrintInput("Nhập ngày cần thống kê (dd/MM/yyyy)");
                    var pos_end = Console.GetCursorPosition();
                    Console.SetCursorPosition(pos_day.Left, pos_day.Top);
                Day:
                    string day = ViewHelper.ReadLine();
                    DateTime dayDate;
                    if (!DateTime.TryParse(day, out dayDate))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Dữ liệu không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_day.Left, pos_day.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", day.Length)));
                        goto Day;
                    }
                    Console.Clear();
                    Load(lichSuNhaps.DistinctBy(x => x.ThoiGian).ToList());
                    Console.Clear();
                    int soLuongNhap = lichSuNhaps.Where(x => x.ThoiGian.ToShortDateString().Equals(dayDate.ToShortDateString())).Sum(x => x.SoLuong);
                    int soLuongBan = HoaDonController.hoaDons.Where(x => x.NgayLap.ToShortDateString().Equals(dayDate.ToShortDateString())).Sum(x => x.Soluong);
                    int soLuongConLai = SanPhamController.sanPhams.Sum(x => x.SoLuong);
                    double doanhThu = HoaDonController.hoaDons.Where(x => x.NgayLap.ToShortDateString().Equals(dayDate.ToShortDateString())).Sum(x => x.TongTien);
                    double tienVon = ThongKeController.lichSuNhaps.Where(x => x.ThoiGian.ToShortDateString().Equals(dayDate.ToShortDateString())).Sum(x => sanPhamController.FindSP(x.MaSP).GiaNhap * x.SoLuong);
                    double tienLai = doanhThu - tienVon;
                    tienLai = (tienLai > 0) ? tienLai : 0;
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả đã nhập là {soLuongNhap}");
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả đã bán là {soLuongBan}");
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả còn lại trong kho là {soLuongConLai}");
                    ViewHelper.PrintSuccess($"Tổng doanh thu của cửa hàng là {doanhThu:c0}");
                    ViewHelper.PrintSuccess($"Tổng tiền nhập các loại hoa quả của cửa hàng là {tienVon:c0}");
                    ViewHelper.PrintSuccess($"Tổng tiền lãi của cửa hàng là {tienLai:c0}");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
                    Console.ReadKey(true);
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
                {
                    Console.Clear();
                    var pos_day = ViewHelper.PrintInput("Nhập tháng cần thống kê (MM/yyyy)");
                    var pos_end = Console.GetCursorPosition();
                    Console.SetCursorPosition(pos_day.Left, pos_day.Top);
                Day:
                    string day = ViewHelper.ReadLine();

                    if (!Regex.IsMatch(day, @"^0?[1-9]|1[012]/\d{4}$"))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Dữ liệu không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_day.Left, pos_day.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", day.Length)));
                        goto Day;
                    }
                    var temp = day.Split('/');
                    DateTime dayDate = new DateTime(int.Parse(temp[1]), int.Parse(temp[0]), 1);
                    Console.Clear();
                    Load(lichSuNhaps.DistinctBy(x => x.ThoiGian).ToList());
                    Console.Clear();
                    int soLuongNhap = lichSuNhaps.Where(x => x.ThoiGian.Month == dayDate.Month && x.ThoiGian.Year == dayDate.Year).Sum(x => x.SoLuong);
                    int soLuongBan = HoaDonController.hoaDons.Where(x => x.NgayLap.Month == dayDate.Month && x.NgayLap.Year == dayDate.Year).Sum(x => x.Soluong);
                    int soLuongConLai = SanPhamController.sanPhams.Sum(x => x.SoLuong);
                    double doanhThu = HoaDonController.hoaDons.Where(x => x.NgayLap.Month == dayDate.Month && x.NgayLap.Year == dayDate.Year).Sum(x => x.TongTien);
                    double tienVon = ThongKeController.lichSuNhaps.Where(x => x.ThoiGian.Month == dayDate.Month && x.ThoiGian.Year == dayDate.Year).Sum(x => sanPhamController.FindSP(x.MaSP).GiaNhap * x.SoLuong);
                    double tienLai = doanhThu - tienVon;
                    tienLai = (tienLai > 0) ? tienLai : 0;
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả đã nhập là {soLuongNhap}");
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả đã bán là {soLuongBan}");
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả còn lại trong kho là {soLuongConLai}");
                    ViewHelper.PrintSuccess($"Tổng doanh thu của cửa hàng là {doanhThu:c0}");
                    ViewHelper.PrintSuccess($"Tổng tiền nhập các loại hoa quả của cửa hàng là {tienVon:c0}");
                    ViewHelper.PrintSuccess($"Tổng tiền lãi của cửa hàng là {tienLai:c0}");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
                    Console.ReadKey(true);
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
                {
                    Console.Clear();
                    var pos_day = ViewHelper.PrintInput("Nhập năm cần thống kê (yyyy)");
                    var pos_end = Console.GetCursorPosition();
                    Console.SetCursorPosition(pos_day.Left, pos_day.Top);
                Day:
                    string day = ViewHelper.ReadLine();
                    if (!Regex.IsMatch(day, @"^\d{4}$"))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Dữ liệu không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_day.Left, pos_day.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", day.Length)));
                        goto Day;
                    }
                    Console.Clear();
                    Load(lichSuNhaps.DistinctBy(x => x.ThoiGian).ToList());
                    Console.Clear();
                    int soLuongNhap = lichSuNhaps.Where(x => x.ThoiGian.Year.ToString() == day).Sum(x => x.SoLuong);
                    int soLuongBan = HoaDonController.hoaDons.Where(x => x.NgayLap.Year.ToString() == day).Sum(x => x.Soluong);
                    int soLuongConLai = SanPhamController.sanPhams.Sum(x => x.SoLuong);
                    double doanhThu = HoaDonController.hoaDons.Where(x => x.NgayLap.Year.ToString() == day).Sum(x => x.TongTien);
                    double tienVon = ThongKeController.lichSuNhaps.Where(x => x.ThoiGian.Year.ToString() == day).Sum(x => sanPhamController.FindSP(x.MaSP).GiaNhap * x.SoLuong);
                    double tienLai = doanhThu - tienVon;
                    tienLai = (tienLai > 0) ? tienLai : 0;
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả đã nhập là {soLuongNhap}");
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả đã bán là {soLuongBan}");
                    ViewHelper.PrintSuccess($"Tống số lượng hoa quả còn lại trong kho là {soLuongConLai}");
                    ViewHelper.PrintSuccess($"Tổng doanh thu của cửa hàng là {doanhThu:c0}");
                    ViewHelper.PrintSuccess($"Tổng tiền nhập các loại hoa quả của cửa hàng là {tienVon:c0}");
                    ViewHelper.PrintSuccess($"Tổng tiền lãi của cửa hàng là {tienLai:c0}");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
                    Console.ReadKey(true);
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else
                {
                    if (IsIncorrect)
                    {
                        ViewHelper.ClearPreviousLine(3);
                    }
                    IsIncorrect = true;
                    ViewHelper.PrintError("Phím bấm không hợp lệ. Có thể bạn đã bấm nhầm");
                    goto ReadKey;
                }
            }
            catch (ExitException)
            {
                return;
            }
        }
        public void ThongKeSanPham()
        {
            Load(lichSuNhaps.DistinctBy(x => x.ThoiGian).ToList());
            Console.Clear();
            if (HoaDonController.hoaDons != null && HoaDonController.hoaDons.Count > 0)
            {
                var sanPhams = HoaDonController.hoaDons.GroupBy(x => x.MaSP).Select(y => new
                {
                    MaSP = y.Key,
                    SoLuongDaBan = y.Sum(z => z.Soluong)
                }).ToList();
                Console.WriteLine("╔══════════╦════════════════════╦═══════════════╗");
                Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Số lượng Đã Bán",-15}║");
                Console.WriteLine("╠══════════╬════════════════════╬═══════════════╣");
                var last = sanPhams.OrderByDescending(x => x.SoLuongDaBan).Last();
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
            }
            else
            {
                ViewHelper.PrintError("Hệ thống chưa ghi nhận hóa đơn nào");
            }
            ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
            Console.ReadKey(true);
        }
        public void ThongKeNhanVien()
        {
            Load(lichSuNhaps.DistinctBy(x => x.ThoiGian).ToList());
            Console.Clear();
            if (HoaDonController.hoaDons != null && HoaDonController.hoaDons.Count > 0)
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
                        Console.WriteLine($"║{nhanVien.MaNV,-15}║{nhanVienController.FindNV(nhanVien.MaNV).TenNV,-30}║{nhanVien.SoDonDaGhi,-15}║");
                        Console.WriteLine("╠═══════════════╬══════════════════════════════╬═══════════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Hệ thống chưa ghi nhận hóa đơn nào");
            }
            ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
            Console.ReadKey(true);
        }
        public void ThongKeKhachHang()
        {
            Load(lichSuNhaps);
            Console.Clear();
            if (HoaDonController.hoaDons != null && HoaDonController.hoaDons.Count > 0)
            {
                var khachHangs = HoaDonController.hoaDons.GroupBy(x => x.MaKH).Select(y =>
                {
                    var sanPham = y.GroupBy(z => z.MaSP).Select(t => new
                    {
                        MaSp = t.Key,
                        SoLuong = t.Sum(ta => ta.Soluong)
                    }).ToList().MaxBy(soluong => soluong.SoLuong).MaSp;
                    return new
                    {
                        MaKH = y.Key,
                        SanPhamMuaNhieuNhat = sanPham,
                        SoLuongDonDaMua = y.Count(z => z.MaKH == y.Key)
                    };
                }).ToList();
                Console.WriteLine("╔═══════════════╦════════════════════╦═══════════════╦═════════════════════════╗");
                Console.WriteLine($"║{"Mã Khách Hàng",-15}║{"Tên Khách Hàng",-20}║{"Số Đơn Đã Mua",-15}║{"Sản phẩm mua nhiều nhất",-25}║");
                Console.WriteLine("╠═══════════════╬════════════════════╬═══════════════╬═════════════════════════╣");
                var last = khachHangs.Last();
                foreach (var khachHang in khachHangs)
                {
                    if (khachHang.Equals(last))
                    {
                        Console.WriteLine($"║{khachHang.MaKH,-15}║{khachHangController.FindKH(khachHang.MaKH).TenKH,-20}║{khachHang.SoLuongDonDaMua,-15}║{khachHang.SanPhamMuaNhieuNhat+"-"+sanPhamController.FindSP(khachHang.SanPhamMuaNhieuNhat).TenSP,-25}║");
                        Console.WriteLine("╚═══════════════╩════════════════════╩═══════════════╩═════════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{khachHang.MaKH,-15}║{khachHangController.FindKH(khachHang.MaKH).TenKH,-20}║{khachHang.SoLuongDonDaMua,-15}║{khachHang.SanPhamMuaNhieuNhat + "-" + sanPhamController.FindSP(khachHang.SanPhamMuaNhieuNhat).TenSP,-25}║");
                        Console.WriteLine("╠═══════════════╬════════════════════╬═══════════════╬═════════════════════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Hệ thống chưa ghi nhận hóa đơn nào");
            }
            ViewHelper.PrintWarning("Bấm phím bất kỳ để quay lại");
            Console.ReadKey(true);
        }

    }
}
