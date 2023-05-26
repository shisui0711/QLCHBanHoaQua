using Framework;
using QLCHBanHoaQua.Models;

namespace QLCHBanHoaQua.Controllers
{
    class HoaDonController
    {
        private const string HoaDonFile = @"data\hoadon.txt";
        public KhachHangController khachHangController { get; private set; }
        public SanPhamController sanPhamController { get; private set; }
        public NhanVienController nhanVienController { get; private set; }
        public Account Session { get; private set; }
        public static List<HoaDon> hoaDons { get; private set; }
        public HoaDonController(KhachHangController _khachHangController, SanPhamController _sanPhamController, NhanVienController _nhanVienController, Account session)
        {
            this.khachHangController = _khachHangController;
            this.sanPhamController = _sanPhamController;
            this.nhanVienController = _nhanVienController;
            this.Session = session;
            hoaDons = new List<HoaDon>();
            Load();
        }
        private void Load()
        {
            hoaDons.Clear();
            bool flag = false;
            using (var fs = new FileStream(HoaDonFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while (!string.IsNullOrEmpty((line = reader.ReadLine())))
                {
                    try
                    {
                        var tmp = line.Trim().Split("|");
                        HoaDon hoaDon = new HoaDon();
                        hoaDon.MaHD = tmp[0];
                        hoaDon.MaNV = tmp[1];
                        hoaDon.NgayLap = DateTime.Parse(tmp[2]);
                        hoaDon.MaKH = tmp[3];
                        hoaDon.MaSP = tmp[4];
                        hoaDon.Soluong = int.Parse(tmp[5]);
                        hoaDon.TongTien = double.Parse(tmp[6]);
                        if(!NhanVienController.nhanViens.Any(x=>x.MaNV == hoaDon.MaNV) || !KhachHangController.khachHangs.Any(x=>x.MaKH == hoaDon.MaKH) || !SanPhamController.sanPhams.Any(x=>x.MaSP == hoaDon.MaSP))
                        {
                            flag = true;
                            continue;
                        }
                        hoaDons.Add(hoaDon);
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
            if (flag) Sync();
        }
        private void Save(HoaDon hoaDon)
        {
            File.AppendAllText(HoaDonFile,
                $"{hoaDon.MaHD}|{hoaDon.MaNV}|{hoaDon.NgayLap.ToShortDateString()}|{hoaDon.MaKH}|" +
                $"{hoaDon.MaSP}|{hoaDon.Soluong}|{hoaDon.TongTien}\n");
        }
        private void Sync()
        {
            File.WriteAllText(HoaDonFile, "");
            foreach (HoaDon hoaDon in hoaDons)
            {
                File.AppendAllText(HoaDonFile,
                $"{hoaDon.MaHD}|{hoaDon.MaNV}|{hoaDon.NgayLap.ToShortDateString()}|{hoaDon.MaKH}|" +
                $"{hoaDon.MaSP}|{hoaDon.Soluong}|{hoaDon.TongTien}\n");
            }
        }
        public bool Exist(string maHD)
        {

            if (Session.VaiTro == Role.Manager)
            {
                try
                {
                    var temp = hoaDons.Find(x => x.MaHD == maHD);
                    if (temp != null) return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            else if (Session.VaiTro == Role.Staff)
            {
                try
                {
                    var temp = hoaDons.FindAll(y => y.MaNV == Session.Username).Find(x => x.MaHD == maHD);
                    if (temp != null) return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            return false;
        }
        public HoaDon FindHD(string maHD)
        {
            Load();
            if (Session.VaiTro == Role.Manager)
            {
                return hoaDons.Find(x => x.MaHD == maHD);
            }
            else if (Session.VaiTro == Role.Staff)
            {
                return hoaDons.FindAll(x => x.MaNV == Session.Username).Find(y => y.MaHD == maHD);
            }
            else
            {
                return null;
            }
        }
        public List<HoaDon> FindByMaSP(string maSP)
        {
            if (Session.VaiTro == Role.Manager)
            {
                return hoaDons.FindAll(x => x.MaSP == maSP);
            }
            else if (Session.VaiTro == Role.Staff)
            {
                return hoaDons.FindAll(x => x.MaNV == Session.Username).FindAll(y => y.MaSP == maSP);
            }
            else
            {
                return null;
            }

        }
        public List<HoaDon> FindByMaKH(string maKH)
        {
            if (Session.VaiTro == Role.Manager)
            {
                return hoaDons.FindAll(x => x.MaKH == maKH);
            }
            else if (Session.VaiTro == Role.Staff)
            {
                return hoaDons.FindAll(x => x.MaNV == Session.Username).FindAll(y => y.MaKH == maKH);
            }
            else
            {
                return null;
            }
        }
        public void GhiHD()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Ghi hóa đơn");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            HoaDon hoaDon = new HoaDon();

            try
            {
                var pos_mahd = ViewHelper.PrintInput("Nhập Mã Hóa Đơn");
                var pos_makh = ViewHelper.PrintInput("Nhập Mã Khách Hàng Mua Hàng");
                var pos_masp = ViewHelper.PrintInput("Nhập Mã Hoa Quả Mà Khách Hàng Mua");
                var pos_sl = ViewHelper.PrintInput("Nhập số lượng mà khách hàng mua");
                var pos_end = Console.GetCursorPosition();
            MaHD:
                Console.SetCursorPosition(pos_mahd.Left, pos_mahd.Top);
                hoaDon.MaHD = ViewHelper.ReadLine();
                if (Exist(hoaDon.MaHD))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hóa đơn đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_mahd.Left, pos_mahd.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", hoaDon.MaHD.Length)));
                    goto MaHD;
                }
                else if (hoaDon.MaHD == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hóa đơn không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_mahd.Left, pos_mahd.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", hoaDon.MaHD.Length)));
                    goto MaHD;
                }
            MaKH:
                Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                hoaDon.MaKH = ViewHelper.ReadLine();
                if (!khachHangController.Exist(hoaDon.MaKH))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã khách hàng không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", hoaDon.MaKH.Length)));
                    goto MaKH;
                }
                else if (hoaDon.MaKH == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã khách hàng không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", hoaDon.MaKH.Length)));
                    goto MaKH;
                }
            MaSP:
                Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                hoaDon.MaSP = ViewHelper.ReadLine();
                if (!sanPhamController.Exist(hoaDon.MaSP))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hoa quả không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", hoaDon.MaSP.Length)));
                    goto MaSP;
                }
                else if (hoaDon.MaSP == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hoa quả không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", hoaDon.MaSP.Length)));
                    goto MaSP;
                }
            SoLuong:
                Console.SetCursorPosition(pos_sl.Left, pos_sl.Top);
                string soLuong = ViewHelper.ReadLine();
                try
                {
                    hoaDon.Soluong = int.Parse(soLuong);
                    if (hoaDon.Soluong <= 0)
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Số lượng không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_sl.Left, pos_sl.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", soLuong.Length)));
                        goto SoLuong;
                    }
                    else if (hoaDon.Soluong > sanPhamController.FindSP(hoaDon.MaSP).SoLuong)
                    {

                        ViewHelper.PrintError("Cửa hàng không còn đủ số lượng. Vui lòng quay lại sau.");
                        Thread.Sleep(3000);
                        return;
                    }
                    else
                    {
                        var sanPham = sanPhamController.FindSP(hoaDon.MaSP);
                        sanPham.SoLuong -= hoaDon.Soluong;
                        sanPhamController.Sync();
                    }
                }
                catch (Exception)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Số lượng không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_sl.Left, pos_sl.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", soLuong.Length)));
                    goto SoLuong;
                }
                hoaDon.NgayLap = DateTime.Now;
                hoaDon.TongTien = sanPhamController.FindSP(hoaDon.MaSP).GiaBan * hoaDon.Soluong;
                if (Session.VaiTro == Role.Staff)
                {
                    hoaDon.MaNV = Session.Username;
                }
                else
                {
                    hoaDon.MaNV = "Quản lý";
                }
                sanPhamController.FindSP(hoaDon.MaSP).SoLuong -= hoaDon.Soluong;
                hoaDons.Add(hoaDon);
                Save(hoaDon);
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess($"Số tiền cần phải trả là {hoaDon.TongTien:c0}");
                ViewHelper.PrintSuccess("Ghi hóa đơn thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }

        }
        public void XoaHD()
        {
            try
            {
                Console.Clear();
                ViewHelper.PrintWarning("Hủy hóa đơn và khôi phục số lượng hoa quả");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                var pos_mahd = ViewHelper.PrintInput("Nhập Mã Hóa Đơn:");
                var pos_end = Console.GetCursorPosition();

            MaHD:
                Console.SetCursorPosition(pos_mahd.Left, pos_mahd.Top);
                string maHD = ViewHelper.ReadLine();
                if (!Exist(maHD))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hóa đơn không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_mahd.Left, pos_mahd.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maHD.Length)));
                    goto MaHD;
                }
                var hoaDon = hoaDons.Find(x => x.MaHD == maHD);
                Console.Clear();
                ShowHD(hoaDon);
                ViewHelper.PrintWarning("Bạn có chắc chắn muốn xóa hóa đơn trên.");
                ViewHelper.PrintWarning("Bấm Y để xác nhận. Bấm N để hủy.");
            XacNhan:
                var xacNhan = Console.ReadKey(true);
                if (xacNhan.Key == ConsoleKey.Y)
                {
                    sanPhamController.FindSP(hoaDon.MaSP).SoLuong += hoaDon.Soluong;
                    sanPhamController.Sync();
                    hoaDons.Remove(hoaDon);
                    Sync();
                    ViewHelper.PrintSuccess("Xóa thành công");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                }
                else if (xacNhan.Key == ConsoleKey.N)
                {
                    return;
                }
                else
                {
                    goto XacNhan;
                }

            }
            catch (ExitException)
            {
                return;
            }
        }
        public void ShowHD(HoaDon hoaDon)
        {
            Console.WriteLine("╔══════════╦═════════════════════════╦══════════╦═════════════════════════╦════════════════════╦══════════╦══════════╦══════════╗");
            Console.WriteLine($"║{"Mã HD",-10}║{"Người lập",-25}║{"Ngày lập",-10}║{"Khách hàng",-25}║{"Sản phẩm",-20}║{"Số lượng",-10}║{"Đơn giá",-10}║{"Thành Tiền",-10}║");
            Console.WriteLine("╠══════════╬═════════════════════════╬══════════╬═════════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + nhanVienController.FindNV(hoaDon.MaNV).TenNV,-25}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                              $"{hoaDon.MaKH + "-" + khachHangController.FindKH(hoaDon.MaKH).TenKH,-25}║{hoaDon.MaSP + "-" + sanPhamController.FindSP(hoaDon.MaSP).TenSP,-20}║" +
                                              $"{hoaDon.Soluong,-10}║{sanPhamController.FindSP(hoaDon.MaSP).GiaBan,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
            Console.WriteLine("╚══════════╩═════════════════════════╩══════════╩═════════════════════════╩════════════════════╩══════════╩══════════╩══════════╝");
        }
        public void ShowHD(List<HoaDon> hoaDons)
        {
            Console.Clear();
            if (hoaDons != null && hoaDons.Count > 0)
            {
                Console.WriteLine("╔══════════╦═════════════════════════╦══════════╦═════════════════════════╦════════════════════╦══════════╦══════════╦══════════╗");
                Console.WriteLine($"║{"Mã HD",-10}║{"Người lập",-25}║{"Ngày lập",-10}║{"Khách hàng",-25}║{"Sản phẩm",-20}║{"Số lượng",-10}║{"Đơn giá",-10}║{"Thành Tiền",-10}║");
                Console.WriteLine("╠══════════╬═════════════════════════╬══════════╬═════════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                var last = hoaDons.Last();
                foreach (HoaDon hoaDon in hoaDons)
                {
                    if (hoaDon.Equals(last))
                    {
                        Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + nhanVienController.FindNV(hoaDon.MaNV).TenNV,-25}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                              $"{hoaDon.MaKH + "-" + khachHangController.FindKH(hoaDon.MaKH).TenKH,-25}║{hoaDon.MaSP + "-" + sanPhamController.FindSP(hoaDon.MaSP).TenSP,-20}║" +
                                              $"{hoaDon.Soluong,-10}║{sanPhamController.FindSP(hoaDon.MaSP).GiaBan,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                        Console.WriteLine("╚══════════╩═════════════════════════╩══════════╩═════════════════════════╩════════════════════╩══════════╩══════════╩══════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + nhanVienController.FindNV(hoaDon.MaNV).TenNV,-25}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                              $"{hoaDon.MaKH + "-" + khachHangController.FindKH(hoaDon.MaKH).TenKH,-25}║{hoaDon.MaSP + "-" + sanPhamController.FindSP(hoaDon.MaSP).TenSP,-20}║" +
                                              $"{hoaDon.Soluong,-10}║{sanPhamController.FindSP(hoaDon.MaSP).GiaBan,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                        Console.WriteLine("╠══════════╬═════════════════════════╬══════════╬═════════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy khách hàng nào hợp lệ");
            }
        }
        public void ShowHD()
        {
            if (Session.VaiTro == Role.Manager)
            {
                Console.Clear();
                if (hoaDons != null && hoaDons.Count > 0)
                {
                    Console.WriteLine("╔══════════╦═════════════════════════╦══════════╦═════════════════════════╦════════════════════╦══════════╦══════════╦══════════╗");
                    Console.WriteLine($"║{"Mã HD",-10}║{"Người lập",-25}║{"Ngày lập",-10}║{"Khách hàng",-25}║{"Sản phẩm",-20}║{"Số lượng",-10}║{"Đơn giá",-10}║{"Thành Tiền",-10}║");
                    Console.WriteLine("╠══════════╬═════════════════════════╬══════════╬═════════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                    var last = hoaDons.Last();
                    foreach (HoaDon hoaDon in hoaDons)
                    {
                        if (hoaDon.Equals(last))
                        {
                            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + nhanVienController.FindNV(hoaDon.MaNV).TenNV,-25}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                              $"{hoaDon.MaKH + "-" + khachHangController.FindKH(hoaDon.MaKH).TenKH,-25}║{hoaDon.MaSP + "-" + sanPhamController.FindSP(hoaDon.MaSP).TenSP,-20}║" +
                                              $"{hoaDon.Soluong,-10}║{sanPhamController.FindSP(hoaDon.MaSP).GiaBan,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                            Console.WriteLine("╚══════════╩═════════════════════════╩══════════╩═════════════════════════╩════════════════════╩══════════╩══════════╩══════════╝");
                        }
                        else
                        {
                            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + nhanVienController.FindNV(hoaDon.MaNV).TenNV,-25}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                               $"{hoaDon.MaKH + "-" + khachHangController.FindKH(hoaDon.MaKH).TenKH,-25}║{hoaDon.MaSP + "-" + sanPhamController.FindSP(hoaDon.MaSP).TenSP,-20}║" +
                                               $"{hoaDon.Soluong,-10}║{sanPhamController.FindSP(hoaDon.MaSP).GiaBan,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                            Console.WriteLine("╠══════════╬═════════════════════════╬══════════╬═════════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                        }
                    }
                }
                else
                {
                    ViewHelper.PrintError("Không tìm thấy khách hàng nào hợp lệ");
                }
            }
            else if (Session.VaiTro == Role.Staff)
            {
                var hoaDons_2 = hoaDons.FindAll(x => x.MaNV == Session.Username);
                Console.Clear();
                if (hoaDons_2 != null && hoaDons_2.Count > 0)
                {
                    Console.WriteLine("╔══════════╦═════════════════════════╦══════════╦═════════════════════════╦════════════════════╦══════════╦══════════╦══════════╗");
                    Console.WriteLine($"║{"Mã HD",-10}║{"Người lập",-25}║{"Ngày lập",-10}║{"Khách hàng",-25}║{"Sản phẩm",-20}║{"Số lượng",-10}║{"Đơn giá",-10}║{"Thành Tiền",-10}║");
                    Console.WriteLine("╠══════════╬═════════════════════════╬══════════╬═════════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                    var last = hoaDons_2.Last();
                    foreach (HoaDon hoaDon in hoaDons_2)
                    {
                        if (hoaDon.Equals(last))
                        {
                            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + nhanVienController.FindNV(hoaDon.MaNV).TenNV,-25}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                               $"{hoaDon.MaKH + "-" + khachHangController.FindKH(hoaDon.MaKH).TenKH,-25}║{hoaDon.MaSP + "-" + sanPhamController.FindSP(hoaDon.MaSP).TenSP,-20}║" +
                                               $"{hoaDon.Soluong,-10}║{sanPhamController.FindSP(hoaDon.MaSP).GiaBan,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                            Console.WriteLine("╚══════════╩═════════════════════════╩══════════╩═════════════════════════╩════════════════════╩══════════╩══════════╩══════════╝");
                        }
                        else
                        {
                            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + nhanVienController.FindNV(hoaDon.MaNV).TenNV,-25}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                              $"{hoaDon.MaKH + "-" + khachHangController.FindKH(hoaDon.MaKH).TenKH,-25}║{hoaDon.MaSP + "-" + sanPhamController.FindSP(hoaDon.MaSP).TenSP,-20}║" +
                                              $"{hoaDon.Soluong,-10}║{sanPhamController.FindSP(hoaDon.MaSP).GiaBan,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                            Console.WriteLine("╠══════════╬═════════════════════════╬══════════╬═════════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                        }
                    }
                }
                else
                {
                    ViewHelper.PrintError("Không tìm thấy khách hàng nào hợp lệ");
                }
            }

        }

    }
}
