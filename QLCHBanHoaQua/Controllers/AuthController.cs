using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua.Controllers
{
    using Framework;
    using Models;
    using System.IO;
    using System.Security.AccessControl;
    using System.Security.Principal;

    class AuthController
    {
        public NhanVienController NhanVienController { get; set; }
        public AuthController(NhanVienController nhanVienController)
        {
            this.NhanVienController = nhanVienController;
            accounts = new List<Account>();
            Load(accounts);
        }
        private const string AccountFile = @"data\account.txt";
        public List<Account> accounts { get;private set; }
        private void Load(List<Account> accounts)
        {
            using (var fs = new FileStream(AccountFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        Role role;
                        var tmp = line.Trim().Split("|");
                        Account account = new Account();
                        account.Username = Security.Base64Decode(tmp[0]);
                        account.Password = Security.Base64Decode(tmp[1]);
                        Enum.TryParse(Security.Base64Decode(tmp[2]), out role);
                        account.VaiTro = role;
                        accounts.Add(account);
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
        private void Save(Account account)
        {
            var username = Security.Base64Encode(account.Username);
            var password = Security.Base64Encode(account.Password);
            var role = Security.Base64Encode(account.VaiTro.ToString());
            File.AppendAllText(AccountFile,$"{username}|{password}|{role}\n");
        }
        private void Sync(List<Account> accounts)
        {
            File.WriteAllText(AccountFile, "");
            foreach (Account account in accounts)
            {
                File.AppendAllText(AccountFile,
                    $"{Security.Base64Encode(account.Username)}|" +
                    $"{Security.Base64Encode(account.Password)}|" +
                    $"{Security.Base64Encode(account.VaiTro.ToString())}\n"
                    );
            }
        }
        public bool Exist(string username)
        {
            try
            {
                var temp = accounts.Find(x => x.Username == username);
                if (temp != null) return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
            
        }
        private bool IsPassword(string username,string password)
        {
            var temp = accounts.Find(x => x.Username == username);
            if(temp != null)
            {
                if (temp.Password == password) return true;
            }
            return false;
        }
        public Account FindAccount(string username)
        {
            return accounts.Find(x => x.Username == username);
        }
        private Role CheckRole(string username)
        {
            var temp = accounts.Find(x => x.Username == username);
            return temp.VaiTro;
        }
        public Account Login()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"",30}╔══════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║ ➽ Sinh viên thực hiện : Nguyễn Văn Khang     ║");
            Console.WriteLine($"{"",30}║ ➽ Mã SV : 12522046                           ║");
            Console.WriteLine($"{"",30}║ ➽ Mã Lớp : 125222                            ║");
            Console.WriteLine($"{"",30}║ ➽ Người Hướng Dẫn :                          ║");
            Console.WriteLine($"{"",30}╚══════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintWarning("Đăng nhập để sử dụng phần mềm");
            ViewHelper.PrintWarning("Nhập Exit để nhập lại từ đầu");
        Username:
            try
            {
                string username = ViewHelper.Input<string>("Nhập tài khoản");
                if (!Exist(username))
                {
                    ViewHelper.PrintError("Tài khoản không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Username;
                }
            Passsword:
                string password = ViewHelper.Input<string>("Nhập mật khẩu");
                if (!IsPassword(username, password))
                {
                    ViewHelper.PrintError("Mật khẩu không chính xác");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Passsword;
                }

                return new Account() { Username = username, Password = password, VaiTro = CheckRole(username) };
            }
            catch (Exception)
            {
                Console.Write("\r" + new string(' ', Console.WindowWidth) + "\r");
                ViewHelper.ClearPreviousLine(5);
                goto Username;
            }
        

        }
        public void Register(Role role)
        {
            Console.Clear();
            ViewHelper.PrintWarning("Tạo tài khoản");
            try
            {
            Username:
                string username = ViewHelper.Input<string>("Nhập tài khoản");
                if(role == Role.Staff)
                {
                    if (!NhanVienController.Exist(username))
                    {
                        ViewHelper.PrintError("Vui lòng nhập tài khoản là mã nhân viên");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto Username;

                    }
                }
                if (username == "")
                {
                    ViewHelper.PrintError("Tài khoản không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Username;
                }
                if (Exist(username))
                {
                    ViewHelper.PrintError("Tài khoản đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Username;
                }
            Password:
                string password = ViewHelper.Input<string>("Nhập mật khẩu");
                if (password == "")
                {
                    ViewHelper.PrintError("Mật khẩu không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Password;
                }
            Repassword:
                string repassword = ViewHelper.Input<string>("Nhập lại mật khẩu");
                if (repassword != password)
                {
                    ViewHelper.PrintError("Mật khẩu không khớp. Vui lòng nhập lại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Repassword;
                }
                var temp = new Account() { Username = username, Password = password, VaiTro = role };
                accounts.Add(temp);
                Sync(accounts);
            }
            catch (ExitException)
            {
                return;
            }
            
        }
        public bool ChangePassword(Account account,string old_pass,string new_pass)
        {
            if(account.Password == old_pass)
            {
                account.Password = new_pass;
                return true;
            }
            return false;
        }
        public bool DeleteAccount(string username)
        {
            bool IsDeleted = false;
            if (username == "admin") return IsDeleted;
            var account = FindAccount(username);
            if(account != null)
            {
                IsDeleted = accounts.Remove(account);
                Sync(accounts);
            }
            return IsDeleted;
        }
    }
}
