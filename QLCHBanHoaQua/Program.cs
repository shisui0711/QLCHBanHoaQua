using System.Text;
using System;
using Microsoft.Extensions.DependencyInjection;
namespace QLCHBanHoaQua
{
    using Controllers;
    using Framework;
    using Models;
    using Views;

    enum Role
    {
        Manager,
        Staff
    }
    class Program
    {
        private static Account Session = null;
        private static void Main(string[] args)
        {

            #region ConsoleSetting
            
            Console.OutputEncoding = Encoding.Unicode;
            Console.InputEncoding = Encoding.Unicode;
            Console.Title = "Quản lý cửa hàng bán hoa quả";
            Console.CursorVisible = false;
            #endregion
            bool IsIncorrect = false;
            var services = new ServiceCollection();
            services.AddSingleton<NCCController>();
            services.AddSingleton<KhachHangController>();
            services.AddSingleton<NhanVienController>();
            services.AddSingleton<SanPhamController>();
            services.AddSingleton<AuthController>();
            services.AddSingleton<HoaDonController>((IServiceProvider serviceprovider) => {
                var sv_khachHang = serviceprovider.GetRequiredService<KhachHangController>();
                var sv_sanPham = serviceprovider.GetRequiredService<SanPhamController>();
                var sv_nhanVien = serviceprovider.GetRequiredService<NhanVienController>();
                var sv_hoaDon = new HoaDonController(sv_khachHang, sv_sanPham, sv_nhanVien, Session);
                return sv_hoaDon;
            });
            services.AddSingleton<ThongKeController>();
            var provider = services.BuildServiceProvider();
        Login:
            var nCCController = provider.GetService<NCCController>();
            var khachHangController = provider.GetService<KhachHangController>();
            var nhanVienController = provider.GetService<NhanVienController>();
            var sanPhamController = provider.GetService<SanPhamController>();
            var auth = provider.GetRequiredService<AuthController>();
            Session = auth.Login();
            var hoaDonController = provider.GetService<HoaDonController>();
            var thongKeController = provider.GetService<ThongKeController>();
            if (Session.VaiTro == Role.Manager)
            {
            ManagerMenu:
                IsIncorrect = false;
                Menu.MainMenuForManager();
            ManagerReadKey:
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                {
                    NhanVienFeature(nhanVienController);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
                {
                    KhachHangFeature(khachHangController);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
                {
                    NhaCungCapFeature(nCCController);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
                {
                    SanPhamFeature(sanPhamController);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
                {
                    HoaDonFeature(hoaDonController);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad6 || keyInfo.Key == ConsoleKey.D6)
                {
                    ThongKeFeature(thongKeController);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad7 || keyInfo.Key == ConsoleKey.D7)
                {
                    QuanLyTaiKhoanFeature(auth);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad0 || keyInfo.Key == ConsoleKey.D0)
                {
                    Session = null;
                    goto Login;
                }
                else
                {
                    if (IsIncorrect)
                    {
                        ViewHelper.ClearPreviousLine(3);
                    }
                    IsIncorrect = true;
                    ViewHelper.PrintError("Phím bấm không hợp lệ. Có thể bạn đã bấm nhầm");
                    goto ManagerReadKey;
                }
            }
            else if (Session.VaiTro == Role.Staff)
            {
            StaffMenu:
                IsIncorrect = false;
                Menu.MainMenuForStaff();
            StaffReadKey:
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                {
                    KhachHangFeature(khachHangController);
                    goto StaffMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
                {
                    SanPhamFeature(sanPhamController);
                    goto StaffMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
                {
                    HoaDonFeature(hoaDonController);
                    goto StaffMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad0 || keyInfo.Key == ConsoleKey.D0)
                {
                    Session = null;
                    goto Login;
                }
                else
                {
                    if (IsIncorrect)
                    {
                        ViewHelper.ClearPreviousLine(3);
                    }
                    IsIncorrect = true;
                    ViewHelper.PrintError("Phím bấm không hợp lệ. Có thể bạn đã bấm nhầm");
                    goto StaffReadKey;
                }
            }
            else
            {
                Environment.Exit(0);
            }
        }
        private static void KhachHangFeature(KhachHangController khachHangController)
        {
            bool IsIncorrect = false;
        Menu:
            Menu.KhachHangMenu();
        ReadKey:
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
            {
                khachHangController.ThemKH();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {

                khachHangController.SuaKH();
                goto Menu;

            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                khachHangController.XoaKH();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm khách hàng theo mã KH");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                try
                {
                    var pos_makh = ViewHelper.PrintInput("Nhập mã khách hàng");
                    var pos_end = Console.GetCursorPosition();
                MaKH:
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    string maKH = ViewHelper.ReadLine();
                    if (!khachHangController.Exist(maKH))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Mã khách hàng không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", maKH.Length)));
                        goto MaKH;
                    }
                    Console.Clear();
                    khachHangController.ShowKH(khachHangController.FindKH(maKH));
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (Exception)
                {
                    goto Menu;
                }

            }
            else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm khách hàng theo tên");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");

                try
                {
                    var pos_ten = ViewHelper.PrintInput("Nhập tên khách hàng");
                    var pos_end = Console.GetCursorPosition();
                TenKH:
                    Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                    string tenKH = ViewHelper.ReadLine();
                    if (tenKH == "")
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Tên không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", tenKH.Length)));
                        goto TenKH;
                    }
                    Console.Clear();
                    khachHangController.ShowKH(khachHangController.FindByName(tenKH));
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (Exception)
                {
                    goto Menu;
                }

            }
            else if (keyInfo.Key == ConsoleKey.NumPad6 || keyInfo.Key == ConsoleKey.D6)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm khách hàng theo địa chỉ");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                try
                {
                    var pos_diachi = ViewHelper.PrintInput("Nhập địa chỉ khách hàng");
                    var pos_end = Console.GetCursorPosition();
                DiaChi:
                    Console.SetCursorPosition(pos_diachi.Left, pos_diachi.Top);
                    string diaChi = ViewHelper.ReadLine();
                    if (diaChi == "")
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Địa chỉ không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_diachi.Left, pos_diachi.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", diaChi.Length)));
                        goto DiaChi;
                    }
                    Console.Clear();
                    khachHangController.ShowKH(khachHangController.FindByAddress(diaChi));
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }


            }
            else if (keyInfo.Key == ConsoleKey.NumPad7 || keyInfo.Key == ConsoleKey.D7)
            {
                Console.Clear();
                khachHangController.ShowKH();
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
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
        private static void NhanVienFeature(NhanVienController nhanVienController)
        {
            bool IsIncorrect = false;
        Menu:
            Menu.NhanVienMenu();
        ReadKey:
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
            {

                nhanVienController.ThemNV();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {
                nhanVienController.SuaNV();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                nhanVienController.XoaNV();
                goto Menu;

            }
            else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm nhân viên theo mã NV");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                try
                {
                    var pos_manv = ViewHelper.PrintInput("Nhập mã nhân viên");
                    var pos_end = Console.GetCursorPosition();
                MaNV:
                    Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                    string maNV = ViewHelper.ReadLine();
                    if (!nhanVienController.Exist(maNV))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Mã nhân viên không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", maNV.Length)));
                        goto MaNV;
                    }
                    Console.Clear();
                    nhanVienController.ShowNV(nhanVienController.FindNV(maNV));
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (Exception)
                {
                    goto Menu;
                }
            }
            else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm nhân viên theo tên");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");

                try
                {
                    var pos_ten = ViewHelper.PrintInput("Nhập tên nhân viên");
                    var pos_end = Console.GetCursorPosition();
                TenNV:
                    Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                    string tenNV = ViewHelper.ReadLine();
                    if (tenNV == "")
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Tên không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", tenNV.Length)));
                        goto TenNV;
                    }
                    Console.Clear();
                    var nhanViens = nhanVienController.FindByName(tenNV);
                    if(nhanViens.Count > 0)
                    {
                        nhanVienController.ShowNV(nhanViens);
                    }
                    else
                    {
                        ViewHelper.PrintError($"Không tìm thầy nhân viên nào tên {tenNV}");
                    }
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }

            }
            else if (keyInfo.Key == ConsoleKey.NumPad6 || keyInfo.Key == ConsoleKey.D6)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm khách hàng theo quê quán");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                try
                {
                    var pos_diachi = ViewHelper.PrintInput("Nhập quê quán nhân viên");
                    var pos_end = Console.GetCursorPosition();
                DiaChi:
                    Console.SetCursorPosition(pos_diachi.Left, pos_diachi.Top);
                    string diaChi = ViewHelper.ReadLine();
                    if (diaChi == "")
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Địa chỉ không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_diachi.Left, pos_diachi.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", diaChi.Length)));
                        goto DiaChi;
                    }
                    Console.Clear();
                    var nhanViens = nhanVienController.FindByAddress(diaChi);
                    if (nhanViens.Count > 0)
                    {
                        nhanVienController.ShowNV(nhanViens);
                    }
                    else
                    {
                        ViewHelper.PrintError($"Không tìm thầy nhân viên nào có quê quán ở {diaChi}");
                    }
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }
            }
            else if (keyInfo.Key == ConsoleKey.NumPad7 || keyInfo.Key == ConsoleKey.D7)
            {
                Console.Clear();
                nhanVienController.ShowNV();
                ViewHelper.PrintWarning("Bấm phím bấm bất kỳ để trở về menu");
                Console.ReadKey(true);
                goto Menu;

            }
            else if (keyInfo.Key == ConsoleKey.NumPad8 || keyInfo.Key == ConsoleKey.D8)
            {
                Console.Clear();
                nhanVienController.ShowNV(nhanVienController.MaleFilter());
                ViewHelper.PrintWarning("Bấm phím bấm bất kỳ để trở về menu");
                Console.ReadKey(true);
                goto Menu;

            }
            else if (keyInfo.Key == ConsoleKey.NumPad9 || keyInfo.Key == ConsoleKey.D9)
            {
                Console.Clear();
                nhanVienController.ShowNV(nhanVienController.FemaleFilter());
                ViewHelper.PrintWarning("Bấm phím bấm bất kỳ để trở về menu");
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
        private static void SanPhamFeature(SanPhamController sanPhamController)
        {
            bool IsIncorrect = false;
            if (Session.VaiTro == Role.Manager)
            {
            Menu:
                Menu.SanPhamMenu();
            ReadKey:
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                {
                    sanPhamController.ThemSP();
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
                {
                    sanPhamController.SuaThongTinSP();
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
                {
                    sanPhamController.ThemSLSP();
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
                {

                    sanPhamController.XoaSP();
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Tìm kiếm hoa quả theo mã");
                    ViewHelper.PrintWarning("Bấm ESC để quay lại");
                    try
                    {
                        var pos_masp = ViewHelper.PrintInput("Nhập mã hoa quả");
                        var pos_end = Console.GetCursorPosition();
                    MaSP:
                        Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                        string maSP = ViewHelper.ReadLine();
                        if (!sanPhamController.Exist(maSP))
                        {
                            Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                            ViewHelper.PrintError("Mã hoa quả không tồn tại");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(3);
                            Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                            Console.Write(string.Concat(Enumerable.Repeat(" ", maSP.Length)));
                            goto MaSP;
                        }

                        Console.Clear();
                        sanPhamController.ShowSP(sanPhamController.FindSP(maSP));
                        ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                        Console.ReadKey(true);
                        goto Menu;
                    }
                    catch (Exception)
                    {
                        goto Menu;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.NumPad6 || keyInfo.Key == ConsoleKey.D6)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Tìm kiếm hoa quả theo tên");
                    ViewHelper.PrintWarning("Bấm ESC để quay lại");
                
                    try
                    {
                        var pos_ten = ViewHelper.PrintInput("Nhập tên hoa quả");
                        var pos_end = Console.GetCursorPosition();
                    TenSP:
                        Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                        string tenSP = ViewHelper.ReadLine();
                        if (tenSP == "")
                        {
                            Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                            ViewHelper.PrintError("Tên không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(3);
                            Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                            Console.Write(string.Concat(Enumerable.Repeat(" ", tenSP.Length)));
                            goto TenSP;
                        }
                        Console.Clear();
                        sanPhamController.ShowSP(sanPhamController.FindByName(tenSP));
                        ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                        Console.ReadKey(true);
                        goto Menu;
                    }
                    catch (Exception)
                    {
                        goto Menu;
                    }

                }
                else if (keyInfo.Key == ConsoleKey.NumPad7 || keyInfo.Key == ConsoleKey.D7)
                {
                    sanPhamController.ShowSP(sanPhamController.DescendingdFilter());
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad8 || keyInfo.Key == ConsoleKey.D8)
                {
                    sanPhamController.ShowSP(sanPhamController.ZeroFilter());
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad9 || keyInfo.Key == ConsoleKey.D9)
                {
                    Console.Clear();
                    sanPhamController.ShowLS();
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
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
            else if (Session.VaiTro == Role.Staff)
            {
            Menu:
                Menu.SanPhamMenuForStaff();
            ReadKey:
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Tìm kiếm hoa quả theo mã");
                    ViewHelper.PrintWarning("Bấm ESC để quay lại");
                    try
                    {
                        var pos_masp = ViewHelper.PrintInput("Nhập mã hoa quả");
                        var pos_end = Console.GetCursorPosition();
                    MaSP:
                        Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                        string maSP = ViewHelper.ReadLine();
                        if (!sanPhamController.Exist(maSP))
                        {
                            Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                            ViewHelper.PrintError("Mã hoa quả không tồn tại");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(3);
                            Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                            Console.Write(string.Concat(Enumerable.Repeat(" ", maSP.Length)));
                            goto MaSP;
                        }

                        Console.Clear();
                        sanPhamController.ShowSP(sanPhamController.FindSP(maSP));
                        ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                        Console.ReadKey(true);
                        goto Menu;
                    }
                    catch (Exception)
                    {
                        goto Menu;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Tìm kiếm hoa quả theo tên");
                    ViewHelper.PrintWarning("Bấm ESC để quay lại");

                    try
                    {
                        var pos_ten = ViewHelper.PrintInput("Nhập tên hoa quả");
                        var pos_end = Console.GetCursorPosition();
                    TenSP:
                        Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                        string tenSP = ViewHelper.ReadLine();
                        if (tenSP == "")
                        {
                            Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                            ViewHelper.PrintError("Tên không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(3);
                            Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                            Console.Write(string.Concat(Enumerable.Repeat(" ", tenSP.Length)));
                            goto TenSP;
                        }
                        Console.Clear();
                        sanPhamController.ShowSP(sanPhamController.FindByName(tenSP));
                        ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                        Console.ReadKey(true);
                        goto Menu;
                    }
                    catch (Exception)
                    {
                        goto Menu;
                    }

                }
                else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
                {
                    sanPhamController.ShowSP(sanPhamController.DescendingdFilter());
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
                {
                    sanPhamController.ShowSP(sanPhamController.ZeroFilter());
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
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

        }
        private static void HoaDonFeature(HoaDonController hoaDonController)
        {
            bool IsIncorrect = false;
        Menu:
            Menu.HoaDonMenu();
        ReadKey:
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
            {
                
                hoaDonController.GhiHD();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {
                hoaDonController.XoaHD();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm hóa đơn theo mã hóa đơn");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                try
                {
                    var pos_mahd = ViewHelper.PrintInput("Nhập mã hóa đơn");
                    var pos_end = Console.GetCursorPosition();
                MaHD:
                    Console.SetCursorPosition(pos_mahd.Left, pos_mahd.Top);
                    string maHD = ViewHelper.ReadLine();
                    if (!hoaDonController.Exist(maHD))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Mã hóa đơn không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_mahd.Left, pos_mahd.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", maHD.Length)));
                        goto MaHD;
                    }
                    Console.Clear();
                    hoaDonController.ShowHD(hoaDonController.FindHD(maHD));
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (Exception)
                {
                    goto Menu;
                }
            }
            else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm hóa đơn theo mã hoa quả");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                try
                {
                    var pos_masp = ViewHelper.PrintInput("Nhập mã hoa quả");
                    var pos_end = Console.GetCursorPosition();
                MaSP:
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    string maSP = ViewHelper.ReadLine();
                    if (!hoaDonController.sanPhamController.Exist(maSP))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Mã hoa quả không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", maSP.Length)));
                        goto MaSP;
                    }
                    Console.Clear();
                    hoaDonController.ShowHD(hoaDonController.FindByMaSP(maSP));
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (Exception)
                {
                    goto Menu;
                }
            }
            else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm hóa đơn theo mã khách hàng");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                try
                {
                    var pos_makh = ViewHelper.PrintInput("Nhập mã khách hàng");
                    var pos_end = Console.GetCursorPosition();
                MaKH:
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    string maKH = ViewHelper.ReadLine();
                    if (!hoaDonController.khachHangController.Exist(maKH))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Mã khách hàng không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", maKH.Length)));
                        goto MaKH;
                    }
                    Console.Clear();
                    hoaDonController.ShowHD(hoaDonController.FindByMaKH(maKH));
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (Exception)
                {
                    goto Menu;
                }
            }
            else if (keyInfo.Key == ConsoleKey.NumPad6 || keyInfo.Key == ConsoleKey.D6)
            {
                hoaDonController.ShowHD();
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
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
        private static void ThongKeFeature(ThongKeController thongKeController)
        {
            bool IsIncorrect = false;
        Menu:
            Menu.MenuThongKe();
        ReadKey:
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
            {
                Console.Clear();
                thongKeController.ThongKeDoanhThu();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {
                Console.Clear();
                thongKeController.ThongKeSanPham();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                Console.Clear();
                thongKeController.ThongKeKhachHang();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
            {
                Console.Clear();
                thongKeController.ThongKeNhanVien();
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
        private static void QuanLyTaiKhoanFeature(AuthController authController)
        {
            bool IsIncorrect = false;
        Menu:
            Menu.MenuQuanTri();
        ReadKey:
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
            {
                authController.ShowAccountStaff();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {
                authController.Register(Role.Staff);
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                authController.ChangePassword();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
            {
                authController.DeleteAccount();
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
        private static void NhaCungCapFeature(NCCController nCCController)
        {
            bool IsIncorrect = false;
        Menu:
            Menu.NhaCungCapMenu();
        ReadKey:
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
            {
                nCCController.ThemNCC();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {
                nCCController.SuaNCC();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                nCCController.XoaNCC();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm nhà cung cấp theo mã nhà cung cấp");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                try
                {
                    var pos_ma = ViewHelper.PrintInput("Nhập mã nhà cung cấp");
                    var pos_end = Console.GetCursorPosition();
                MaNCC:
                    Console.SetCursorPosition(pos_ma.Left, pos_ma.Top);
                    string maNCC = ViewHelper.ReadLine();
                    if (!nCCController.Exist(maNCC))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Mã nhà cung cấp không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_ma.Left, pos_ma.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", maNCC.Length)));
                        goto MaNCC;
                    }
                    var nCC = nCCController.FindNCC(maNCC);
                    Console.Clear();
                    nCCController.ShowNCC(nCC);
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (Exception)
                {
                    goto Menu;
                }

            }
            else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm nhà cung cấp theo tên");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");

                try
                {
                    var pos_ten = ViewHelper.PrintInput("Nhập tên nhà cung cấp");
                    var pos_end = Console.GetCursorPosition();
                    Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                    string tenNCC = ViewHelper.ReadLine();
                    var nCC = nCCController.FindByName(tenNCC);
                    Console.Clear();
                    nCCController.ShowNCC(nCC);
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (Exception)
                {
                    goto Menu;
                }

            }
            else if (keyInfo.Key == ConsoleKey.NumPad6 || keyInfo.Key == ConsoleKey.D6)
            {
                try
                {
                    Console.Clear();
                    nCCController.ShowNCC();
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }


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
    }
}