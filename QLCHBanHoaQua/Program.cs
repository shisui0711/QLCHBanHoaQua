using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua
{
    using Framework;
    using Models;
    using Controllers;
    using Views;

    enum Role
    {
        Manager,
        Staff,
        Operator
    }
    class Program
    {
        private static Account Session = null;
        private static void Main(string[] args)
        {
            #region ConsoleSetting
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.Unicode;
            Console.CursorVisible = false;
            #endregion
            bool IsIncorrect = false;
            
        Login:
            var khachHangController = new KhachHangController();
            var nhanVienController = new NhanVienController();
            var sanPhamController = new SanPhamController();
            var auth = new AuthController(nhanVienController);
            Session = auth.Login();
            var hoaDonController = new HoaDonController(khachHangController, sanPhamController, nhanVienController, Session);
            var thongKeController = new ThongKeController(hoaDonController, sanPhamController, nhanVienController,khachHangController);
            if (Session.VaiTro == Role.Manager)
            {
            ManagerMenu:
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
                    SanPhamFeature(sanPhamController);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
                {
                    HoaDonFeature(hoaDonController);
                    goto ManagerMenu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
                {
                    ThongKeFeature(thongKeController);
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
            else if (Session.VaiTro == Role.Operator)
            {
                var operatorController = new OperatorController(auth);
            OperatorController:
                Menu.MainMenuForOperator();
            OperatorReadKey:
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                {
                    operatorController.ShowAllAccount();
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                    Console.ReadKey(true);
                    goto OperatorController;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
                {
                    operatorController.ShowAccountManager();
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                    Console.ReadKey(true);
                    goto OperatorController;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
                {
                    operatorController.ShowAccountStaff();
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                    Console.ReadKey(true);
                    goto OperatorController;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
                {
                    operatorController.CreateAccount(Role.Manager);
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                    Console.ReadKey(true);
                    goto OperatorController;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
                {
                    operatorController.CreateAccount(Role.Staff);
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                    Console.ReadKey(true);
                    goto OperatorController;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad6 || keyInfo.Key == ConsoleKey.D6)
                {
                    operatorController.ChangePassword();
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                    Console.ReadKey(true);
                    goto OperatorController;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad7 || keyInfo.Key == ConsoleKey.D7)
                {
                    operatorController.DeleteAccount();
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                    Console.ReadKey(true);
                    goto OperatorController;
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
                    goto OperatorReadKey;
                }
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
                Console.Clear();
                ViewHelper.PrintWarning("Thêm khách hàng");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                khachHangController.ThemKH();

                goto Menu;

            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Cập nhật thông tin khách hàng");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
            MaKH:
                try
                {
                    string maKH = ViewHelper.Input<string>("Nhập mã khách hàng:");
                    if (!khachHangController.Exist(maKH))
                    {
                        ViewHelper.PrintError("Mã khách hàng không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto MaKH;
                    }
                    khachHangController.SuaKH(maKH);
                    ViewHelper.PrintSuccess($"Cập nhật thành công khách hàng {maKH}");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }

            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Xóa khách hàng");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                MaKH:
                    string maKH = ViewHelper.Input<string>("Nhập mã khách hàng:");
                    if (!khachHangController.Exist(maKH))
                    {
                        ViewHelper.PrintError("Mã khách hàng không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto MaKH;
                    }
                    if (khachHangController.XoaKH(maKH)) ViewHelper.PrintSuccess("Xóa thành công");
                    else ViewHelper.PrintError("Xóa thất bại");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }


            }
            else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm khách hàng theo mã KH");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                MaKH:
                    string maKH = ViewHelper.Input<string>("Nhập mã khách hàng:");
                    if (!khachHangController.Exist(maKH))
                    {
                        ViewHelper.PrintError("Mã khách hàng không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
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
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
            TenKH:
                try
                {
                    string tenKH = ViewHelper.Input<string>("Nhập tên khách hàng:");
                    if (tenKH == "")
                    {
                        ViewHelper.PrintError("Tên không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
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
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                DiaChi:
                    string diaChi = ViewHelper.Input<string>("Nhập địa chỉ khách hàng:");
                    if (diaChi == "")
                    {
                        ViewHelper.PrintError("Địa chỉ không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
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
                Console.Clear();
                ViewHelper.PrintWarning("Thêm nhân viên");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                nhanVienController.ThemNV();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Cập nhật thông tin nhân viên");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
            MaNV:
                try
                {
                    string maNV = ViewHelper.Input<string>("Nhập mã nhân viên:");
                    if (!nhanVienController.Exist(maNV))
                    {
                        ViewHelper.PrintError("Mã nhân viên không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto MaNV;
                    }
                    nhanVienController.SuaNV(maNV);
                    ViewHelper.PrintSuccess($"Cập nhật thành công nhân viên {maNV}");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }
            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Xóa nhân viên");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                MaNV:
                    string maNV = ViewHelper.Input<string>("Nhập mã nhân viên:");
                    if (!nhanVienController.Exist(maNV))
                    {
                        ViewHelper.PrintError("Mã nhân viên không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto MaNV;
                    }
                    if (nhanVienController.XoaNV(maNV)) ViewHelper.PrintSuccess("Xóa thành công");
                    else ViewHelper.PrintError("Xóa thất bại");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }
            }
            else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm nhân viên theo mã NV");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                MaNV:
                    string maNV = ViewHelper.Input<string>("Nhập mã nhân viên:");
                    if (!nhanVienController.Exist(maNV))
                    {
                        ViewHelper.PrintError("Mã nhân viên không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
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
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
            TenNV:
                try
                {
                    string tenKH = ViewHelper.Input<string>("Nhập tên nhân viên:");
                    if (tenKH == "")
                    {
                        ViewHelper.PrintError("Tên không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto TenNV;
                    }
                    Console.Clear();
                    nhanVienController.ShowNV(nhanVienController.FindByName(tenKH));
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
                ViewHelper.PrintWarning("Tìm kiếm khách hàng theo quê quán");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                DiaChi:
                    string diaChi = ViewHelper.Input<string>("Nhập quê quán nhân viên:");
                    if (diaChi == "")
                    {
                        ViewHelper.PrintError("Địa chỉ không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto DiaChi;
                    }
                    Console.Clear();
                    nhanVienController.ShowNV(nhanVienController.FindByAddress(diaChi));
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
            if(Session.VaiTro == Role.Manager)
            {
            Menu:
                Menu.SanPhamMenu();
            ReadKey:
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                if (keyInfo.Key == ConsoleKey.NumPad1 || keyInfo.Key == ConsoleKey.D1)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Thêm Loại Hoa Quả");
                    ViewHelper.PrintWarning("Nhập Exit để quay lại");
                    sanPhamController.ThemSP();
                    goto Menu;
                }
                else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Cập nhật thông tin hoa quả");
                    ViewHelper.PrintWarning("Nhập Exit để quay lại");
                MaSP:
                    try
                    {
                        string maSP = ViewHelper.Input<string>("Nhập mã hoa quả:");
                        if (!sanPhamController.Exist(maSP))
                        {
                            ViewHelper.PrintError("Mã hoa quả không tồn tại");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
                            goto MaSP;
                        }
                        sanPhamController.SuaThongTinSP(maSP);
                        ViewHelper.PrintSuccess($"Cập nhật thông tin thành công {maSP}");
                        ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                        Console.ReadKey(true);
                        goto Menu;
                    }
                    catch (ExitException)
                    {
                        goto Menu;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Cập nhật số lượng hoa quả");
                    ViewHelper.PrintWarning("Nhập Exit để quay lại");
                MaSP:
                    try
                    {
                        string maSP = ViewHelper.Input<string>("Nhập mã hoa quả:");
                        if (!sanPhamController.Exist(maSP))
                        {
                            ViewHelper.PrintError("Mã hoa quả không tồn tại");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
                            goto MaSP;
                        }
                        sanPhamController.ThemSLSP(maSP);
                        goto Menu;
                    }
                    catch (ExitException)
                    {
                        goto Menu;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.NumPad4 || keyInfo.Key == ConsoleKey.D4)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Xóa hoa quả");
                    ViewHelper.PrintWarning("Nhập Exit để quay lại");
                    try
                    {
                    MaSP:
                        string maSP = ViewHelper.Input<string>("Nhập mã hoa quả:");
                        if (!sanPhamController.Exist(maSP))
                        {
                            ViewHelper.PrintError("Mã hoa quả không tồn tại");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
                            goto MaSP;
                        }
                        if (sanPhamController.XoaSP(maSP)) ViewHelper.PrintSuccess("Xóa thành công");
                        else ViewHelper.PrintError("Xóa thất bại");
                        ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                        Console.ReadKey(true);
                        goto Menu;
                    }
                    catch (ExitException)
                    {
                        goto Menu;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.NumPad5 || keyInfo.Key == ConsoleKey.D5)
                {
                    Console.Clear();
                    ViewHelper.PrintWarning("Tìm kiếm hoa quả theo mã");
                    ViewHelper.PrintWarning("Nhập Exit để quay lại");
                    try
                    {
                    MaSP:
                        string maSP = ViewHelper.Input<string>("Nhập mã hoa quả:");
                        if (!sanPhamController.Exist(maSP))
                        {
                            ViewHelper.PrintError("Mã hoa quả không tồn tại");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
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
                    ViewHelper.PrintWarning("Nhập Exit để quay lại");
                TenSP:
                    try
                    {
                        string tenSP = ViewHelper.Input<string>("Nhập tên hoa quả:");
                        if (tenSP == "")
                        {
                            ViewHelper.PrintError("Tên không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
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
                    ViewHelper.PrintWarning("Nhập Exit để quay lại");
                    try
                    {
                    MaSP:
                        string maSP = ViewHelper.Input<string>("Nhập mã hoa quả:");
                        if (!sanPhamController.Exist(maSP))
                        {
                            ViewHelper.PrintError("Mã hoa quả không tồn tại");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
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
                    ViewHelper.PrintWarning("Nhập Exit để quay lại");
                TenSP:
                    try
                    {
                        string tenSP = ViewHelper.Input<string>("Nhập tên hoa quả:");
                        if (tenSP == "")
                        {
                            ViewHelper.PrintError("Tên không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
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
                Console.Clear();
                ViewHelper.PrintWarning("Ghi hóa đơn");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                hoaDonController.GhiHD();
                goto Menu;
            }
            else if (keyInfo.Key == ConsoleKey.NumPad2 || keyInfo.Key == ConsoleKey.D2)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Xóa hóa đơn và khôi phục số lượng hoa quả");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                MaHD:
                    string maHD = ViewHelper.Input<string>("Nhập mã hóa đơn:");
                    if (!hoaDonController.Exist(maHD))
                    {
                        ViewHelper.PrintError("Mã hóa đơn không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto MaHD;
                    }
                    if (hoaDonController.XoaHD(maHD)) ViewHelper.PrintSuccess("Xóa thành công");
                    else ViewHelper.PrintError("Xóa thất bại");
                    ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                    Console.ReadKey(true);
                    goto Menu;
                }
                catch (ExitException)
                {
                    goto Menu;
                }
            }
            else if (keyInfo.Key == ConsoleKey.NumPad3 || keyInfo.Key == ConsoleKey.D3)
            {
                Console.Clear();
                ViewHelper.PrintWarning("Tìm kiếm hóa đơn theo mã hóa đơn");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                MaHD:
                    string maHD = ViewHelper.Input<string>("Nhập mã hóa đơn:");
                    if (!hoaDonController.Exist(maHD))
                    {
                        ViewHelper.PrintError("Mã hóa đơn không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
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
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                MaSP:
                    string maSP = ViewHelper.Input<string>("Nhập mã hoa quả:");
                    if (!hoaDonController.sanPhamController.Exist(maSP))
                    {
                        ViewHelper.PrintError("Mã hoa quả không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
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
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                try
                {
                MaKH:
                    string maKH = ViewHelper.Input<string>("Nhập mã khách hàng:");
                    if (!hoaDonController.khachHangController.Exist(maKH))
                    {
                        ViewHelper.PrintError("Mã khách hàng không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto MaKH;
                    }
                    Console.Clear();
                    hoaDonController.ShowHD(hoaDonController.FindByMaSP(maKH));
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
    }
}