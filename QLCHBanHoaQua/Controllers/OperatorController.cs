using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua.Controllers
{
    class OperatorController
    {
        private AuthController auth;
        public OperatorController(AuthController _auth)
        {
            this.auth = _auth;
        }
        public void ShowAllAccount()
        {
            if(auth.accounts != null && auth.accounts.Count > 0)
            {
                Console.Clear();
                Console.WriteLine("╔═══════════════╦═══════════════╦════════════════════╗");
                Console.WriteLine($"║{"Tên Tài Khoản",-15}║{"Mật khẩu",-15}║{"Vai trò",-20}║");
                Console.WriteLine("╠═══════════════╬═══════════════╬════════════════════╣");
                var last = auth.accounts.Last();
                foreach (var account in auth.accounts)
                {
                    string role = "";
                    switch (account.VaiTro)
                    {
                        case Role.Manager:
                            role = "Quản Lý";
                            break;
                        case Role.Staff:
                            role = "Nhân Viên";
                            break;
                        case Role.Operator:
                            role = "Quản Trị Hệ Thống";
                            break;
                        default:
                            break;
                    }
                    if (account.Equals(last))
                    {
                        Console.WriteLine($"║{account.Username,-15}║{account.Password,-15}║{role,-20}║");
                        Console.WriteLine("╚═══════════════╩═══════════════╩════════════════════╝");

                    }
                    else
                    {
                        Console.WriteLine($"║{account.Username,-15}║{account.Password,-15}║{role,-20}║");
                        Console.WriteLine("╠═══════════════╬═══════════════╬════════════════════╣");
                    }
                    
                }
            }
        }
        public void ShowAccountManager()
        {
            Console.Clear();
            var accounts = auth.accounts.Where(x => x.VaiTro == Role.Manager).ToList();
            if (accounts != null && accounts.Count > 0)
            {
                Console.WriteLine("╔═══════════════╦═══════════════╦════════════════════╗");
                Console.WriteLine($"║{"Tên Tài Khoản",-15}║{"Mật khẩu",-15}║{"Vai trò",-20}║");
                Console.WriteLine("╠═══════════════╬═══════════════╬════════════════════╣");
                var last = accounts.Last();
                foreach (var account in accounts)
                {
                    if (account.Equals(last))
                    {
                        Console.WriteLine($"║{account.Username,-15}║{account.Password,-15}║{"Quản lý",-20}║");
                        Console.WriteLine("╚═══════════════╩═══════════════╩════════════════════╝");

                    }
                    else
                    {
                        Console.WriteLine($"║{account.Username,-15}║{account.Password,-15}║{"Quản lý",-20}║");
                        Console.WriteLine("╠═══════════════╬═══════════════╬════════════════════╣");
                    }

                }
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy tài khoản nào");
            }
        }
        public void ShowAccountStaff()
        {
            Console.Clear();
            var accounts = auth.accounts.Where(x => x.VaiTro == Role.Staff).ToList();
            if (accounts != null && accounts.Count > 0)
            {
                Console.WriteLine("╔═══════════════╦═══════════════╦════════════════════╗");
                Console.WriteLine($"║{"Tên Tài Khoản",-15}║{"Mật khẩu",-15}║{"Vai trò",-20}║");
                Console.WriteLine("╠═══════════════╬═══════════════╬════════════════════╣");
                var last = accounts.Last();
                foreach (var account in accounts)
                {
                    if (account.Equals(last))
                    {
                        Console.WriteLine($"║{account.Username,-15}║{account.Password,-15}║{"Nhân Viên",-20}║");
                        Console.WriteLine("╚═══════════════╩═══════════════╩════════════════════╝");

                    }
                    else
                    {
                        Console.WriteLine($"║{account.Username,-15}║{account.Password,-15}║{"Nhân Viên",-20}║");
                        Console.WriteLine("╠═══════════════╬═══════════════╬════════════════════╣");
                    }

                }
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy tài khoản nào");
            }
        }
        public void CreateAccount(Role role)
        {
            auth.Register(role);
        }
        public void ChangePassword()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Đổi mật khẩu");
            ViewHelper.PrintWarning("Nhập Exit để quay lại");
            try
            {
             Username:
                string username = ViewHelper.Input<string>("Nhập tài khoản");
                if (!auth.Exist(username))
                {
                    ViewHelper.PrintError("Tài khoản không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Username;
                }
                string old_password = ViewHelper.Input<string>("Nhập mật khẩu cũ");
            Password:
                string new_password = ViewHelper.Input<string>("Nhập mật khẩu mới");
                if(new_password == "")
                {
                    ViewHelper.PrintError("Mật khẩu không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Password;
                }
                if (new_password.Length<5)
                {
                    ViewHelper.PrintError("Mật khẩu quá ngắn");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                }
            Repassword:
                string repassword = ViewHelper.Input<string>("Nhập lại mật khẩu mới");
                if (repassword != new_password)
                {
                    ViewHelper.PrintError("Mật khẩu không khớp. Vui lòng nhập lại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Repassword;
                }
                if(auth.ChangePassword(auth.FindAccount(username), old_password, repassword))
                {
                    ViewHelper.PrintSuccess("Thay đổi mật khẩu thành công");
                }
                else
                {
                    ViewHelper.PrintError("Thay đổi mật khẩu thất bại. Mật khẩu cũ sai");
                }
            }
            catch (ExitException)
            {
                return;
            }
        }
        public void DeleteAccount()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Xóa tài khoản");
            ViewHelper.PrintWarning("Nhập Exit để quay lại");
            try
            {
            Username:
                string username = ViewHelper.Input<string>("Nhập tài khoản");
                if (!auth.Exist(username))
                {
                    ViewHelper.PrintError("Tài khoản không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Username;
                }
                if (auth.DeleteAccount(username))
                {
                    ViewHelper.PrintSuccess("Xóa tài khoản thành công");
                }
                else
                {
                    ViewHelper.PrintError("Xóa tài khoản thất bại");
                }
            }
            catch (ExitException)
            {
                return;
            }
        }
    }
}
