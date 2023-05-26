namespace QLCHBanHoaQua.Views
{
    using Framework;
    public class Menu
    {
        public static void MenuQuanTri()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                         亗 QUẢN LÝ TÀI KHOẢN 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Hiển thị thông tin toàn bộ tài khoản nhân viên                ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Tạo tài khoản cho nhân viên                                   ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Thay đổi mật khẩu cho tài khoản                               ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Xóa tài khoản                                                 ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
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
            Console.WriteLine($"{"",30}║ ➽ [3] : Quản lý nhà cung cấp                                          ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Quản lý kho                                                   ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Quản lý hóa đơn                                               ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Thống kê                                                      ║");
            Console.WriteLine($"{"",30}║ ➽ [7] : Quản lý tài khoản                                             ║");
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
            Console.WriteLine($"{"",30}║                              亗 THỐNG KÊ  亗                          ║");
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
        public static void MenuLuaChonThongKe()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                              亗 THỐNG KÊ  亗                          ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Thống kê theo ngày                                            ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Thống kê theo tháng                                           ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Thống kê theo năm                                             ║");
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
            Console.WriteLine($"{"",30}║ ➽ [2] : Quản lý kho                                                   ║");
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
            Console.WriteLine($"{"",30}║ ➽ [8] : Hiển thị danh sách các nhân viên có giới tính nam             ║");
            Console.WriteLine($"{"",30}║ ➽ [9] : Hiển thị danh sách các nhân viên có giới tính nữ              ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
        public static void NguonGocMenu()
        {
            Console.Clear();
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                         亗 QUẢN LÝ NGUỒN GỐC 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Thêm nguồn gốc                                                ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Cập nhật thông tin nguồn gốc                                  ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Xóa nguồn gốc                                                 ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Tìm kiếm nhân viên theo mã nhân viên                          ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Tìm kiếm nhân viên theo tên                                   ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Tìm kiếm nhân viên theo quê quán                              ║");
            Console.WriteLine($"{"",30}║ ➽ [7] : Hiển thị danh sách tất cả nhân viên                           ║");
            Console.WriteLine($"{"",30}║ ➽ [8] : Hiển thị danh sách các nhân viên có giới tính nam             ║");
            Console.WriteLine($"{"",30}║ ➽ [9] : Hiển thị danh sách các nhân viên có giới tính nữ              ║");
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
            Console.WriteLine($"{"",30}║                             亗 QUẢN LÝ KHO 亗                         ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Thêm loại hoa quả                                             ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Cập nhật thông tin hoa quả                                    ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Thêm số lượng hoa quả                                         ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Xóa loại hoa quả                                              ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Tìm kiếm hoa quả theo mã hoa quả                              ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Tìm kiếm hoa quả theo tên                                     ║");
            Console.WriteLine($"{"",30}║ ➽ [7] : Hiển thị danh sách hoa quả sắp xếp theo số lượng              ║");
            Console.WriteLine($"{"",30}║ ➽ [8] : Hiển thị danh sách hoa quả có số lượng bằng 0                 ║");
            Console.WriteLine($"{"",30}║ ➽ [9] : Hiển thị lịch sử thêm số lượng hoa quả                        ║");
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
            Console.WriteLine($"{"",30}║                             亗 QUẢN LÝ KHO 亗                         ║");
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
            Console.WriteLine($"{"",30}║ ➽ [2] : Hủy hóa đơn                                                   ║");
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
        public static void NhaCungCapMenu()
        {
            Console.Clear();
            Console.WriteLine($"{"",30}╔═══════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║                      亗 QUẢN LÝ NHÀ CUNG CẤP 亗                       ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}║ ➽ [1] : Thêm nhà cung cấp                                             ║");
            Console.WriteLine($"{"",30}║ ➽ [2] : Cập nhật thông tin nhà cung cấp                               ║");
            Console.WriteLine($"{"",30}║ ➽ [3] : Xóa nhà cung cấp                                              ║");
            Console.WriteLine($"{"",30}║ ➽ [4] : Tìm kiếm nhà cung cấp theo mã nhà cung cấp                    ║");
            Console.WriteLine($"{"",30}║ ➽ [5] : Tìm kiếm nhà cung cấp theo tên nhà cung cấp                   ║");
            Console.WriteLine($"{"",30}║ ➽ [6] : Hiển thị danh sách tất cả nhà cung cấp                        ║");
            Console.WriteLine($"{"",30}║ ➽ [ESC] : Về Menu Chính                                               ║");
            Console.WriteLine($"{"",30}║                                                                       ║");
            Console.WriteLine($"{"",30}╚═══════════════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintSuccess("Bấm Phím Để Chọn Chức Năng");
        }
    }
}
