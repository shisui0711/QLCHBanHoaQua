using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua.Views
{
    using Framework;
    public class Menu
    {
        public static void MainMenuForOperator()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                         亗 QUẢN TRỊ HỆ THỐNG 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Hiển thị thông tin toàn bộ tài khoản của hệ thống             ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Hiển thị thông tin toàn bộ tài khoản người quản lý            ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Hiển thị thông tin toàn bộ tài khoản nhân viên                ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Tạo tài khoản cho quản lý                                     ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Tạo tài khoản cho nhân viên                                   ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Thay đổi mật khẩu cho tài khoản                               ║");
            Console.WriteLine($"{"",30}║ ➽ [7] : Xóa tài khoản                                                 ║");
            Console.WriteLine($"{"",30}║ ➽ [0] : Đăng xuất                                                     ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
        public static void MainMenuForManager()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║             亗 CHƯƠNG TRÌNH QUẢN LÝ CỬA HÀNG BÁN HOA QUẢ 亗           ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Quản lý nhân viên                                             ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Quản lý khách hàng                                            ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Quản lý hoa quả                                               ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Quản lý hóa đơn                                               ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Thống kê                                                      ║");
            Console.WriteLine($"{"",30}║ ➽ [0] : Đăng xuất                                                     ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
        public static void MenuThongKe()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                        亗 THỐNG KÊ DOANH THU 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Thống kê doanh thu                                            ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Thống kê những loại hoa quả bán chạy                          ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Thống kê những khách hàng mua hàng nhiều                      ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Thống kê những nhân viên bán được nhiều đơn                   ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
        public static void MainMenuForStaff()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║             亗 CHƯƠNG TRÌNH QUẢN LÝ CỬA HÀNG BÁN HOA QUẢ 亗           ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Quản lý khách hàng                                            ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Quản lý hoa quả                                               ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Quản lý hóa đơn                                               ║");
            Console.WriteLine($"{"",30}║ ➽ [0] : Đăng xuất                                                     ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }

        public static void NhanVienMenu()
        {
            Console.Clear();
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                         亗 QUẢN LÝ NHÂN VIÊN 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Thêm nhân viên                                                ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Cập nhật thông tin nhân viên                                  ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Xóa nhân viên                                                 ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Tìm kiếm nhân viên theo mã nhân viên                          ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Tìm kiếm nhân viên theo tên                                   ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Tìm kiếm nhân viên theo quê quán                              ║");
            Console.WriteLine($"{"",30}║ ➽ [7] : Hiển thị danh sách tất cả nhân viên                           ║");
            Console.WriteLine($"{"",30}║ ➽ [8] : Hiên thị danh sách tất cả nhân viên có giới tính nam          ║");
            Console.WriteLine($"{"",30}║ ➽ [9] : Hiên thị danh sách tất cả nhân viên có giới tính nữ           ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
        public static void KhachHangMenu()
        {
            Console.Clear();
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                         亗 QUẢN LÝ KHÁCH HÀNG 亗                      ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Thêm khách hàng                                               ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Cập nhật thông tin khách hàng                                 ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Xóa khách hàng                                                ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Tìm kiếm khách hàng theo mã khách hàng                        ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Tìm kiếm khách hàng theo tên                                  ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Tìm kiếm khách hàng theo địa chỉ                              ║");
            Console.WriteLine($"{"",30}║ ➽ [7] : Hiển thị danh sách tất cả khách hàng                          ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
        public static void SanPhamMenu()
        {
            Console.Clear();
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                           亗 QUẢN LÝ HOA QUẢ 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Thêm loại hoa quả                                             ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Cập nhật thông tin hoa quả                                    ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Thêm số lượng hoa quả                                         ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Xóa loại hoa quả                                              ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Tìm kiếm hoa quả theo mã hoa quả                              ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Tìm kiếm hoa quả theo tên                                     ║");
            Console.WriteLine($"{"",30}║ ➽ [7] : Hiển thị danh sách hoa quả sắp xếp theo số lượng              ║");
            Console.WriteLine($"{"",30}║ ➽ [8] : Hiển thị danh sách hoa quả có số lượng bằng 0                 ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
        public static void SanPhamMenuForStaff()
        {
            Console.Clear();
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                           亗 QUẢN LÝ HOA QUẢ 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Tìm kiếm hoa quả theo mã hoa quả                              ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Tìm kiếm hoa quả theo tên                                     ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Hiển thị danh sách hoa quả sắp xếp theo số lượng              ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Hiển thị danh sách hoa quả có số lượng bằng 0                 ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
        public static void HoaDonMenu()
        {
            Console.Clear();
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                           亗 QUẢN LÝ HÓA ĐƠN 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Ghi hóa đơn                                                   ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Xóa hóa đơn                                                   ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Tìm kiếm hóa đơn theo mã hóa đơn                              ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Tìm kiếm hóa đơn theo mã hoa quả                              ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Tìm kiếm hóa đơn theo mã khách hàng                           ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Hiển thị các hóa đơn đã ghi                                   ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
    }
}
