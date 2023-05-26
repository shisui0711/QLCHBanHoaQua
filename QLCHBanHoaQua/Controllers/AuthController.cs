namespace QLCHBanHoaQua.Controllers
{
    using Framework;
    using Models;
    using System.IO;

    class AuthController
    {
        public NhanVienController nhanVienController { get; set; }
        public AuthController(NhanVienController nhanVienController)
        {
            this.nhanVienController = nhanVienController;
            accounts = new List<Account>();
            Load();
        }
        private const string AccountFile = @"data\account.txt";
        public List<Account> accounts { get; private set; }
        private void Load()
        {
            accounts.Clear();
            bool flag = false;
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
                        account.Username = tmp[0];
                        account.Password = tmp[1];
                        Enum.TryParse(tmp[2], out role);
                        account.VaiTro = role;
                        if (account.Username != "quanly")
                        {
                            if (!NhanVienController.nhanViens.Any(x=>x.MaNV == account.Username))
                            {
                                flag = true;
                                continue;
                            }
                        }
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
            if(flag) Sync();
        }
        private void Save(Account account)
        {
            var username = account.Username;
            var password = Security.GetMd5Hash(account.Password);
            var role = account.VaiTro.ToString();
            File.AppendAllText(AccountFile, $"{username}|{password}|{role}\n");
        }
        private void Sync()
        {
            File.WriteAllText(AccountFile, "");
            foreach (Account account in accounts)
            {
                File.AppendAllText(AccountFile,
                    $"{account.Username}|" +
                    $"{account.Password}|" +
                    $"{account.VaiTro.ToString()}\n"
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
        public bool IsPassword(string username, string password)
        {
            var temp = accounts.Find(x => x.Username == username);
            if (temp != null)
            {
                if (temp.Password == Security.GetMd5Hash(password)) return true;
            }
            return false;
        }
        public Account FindAccount(string username)
        {
            return accounts.Find(x => x.Username == username);
        }
        public void ShowAccount(Account account)
        {
            Console.WriteLine("╔═══════════════╦═══════════════════════════════════╦════════════════════╗");
            Console.WriteLine($"║{"Tên Tài Khoản",-15}║{"Mật khẩu",-35}║{"Vai trò",-20}║");
            Console.WriteLine("╠═══════════════╬═══════════════════════════════════╬════════════════════╣");
            Console.WriteLine($"║{account.Username,-15}║{account.Password,-35}║{"Nhân Viên",-20}║");
            Console.WriteLine("╚═══════════════╩═══════════════════════════════════╩════════════════════╝");
        }
        public void ShowAccountStaff()
        {
            Load();
            Console.Clear();
            if (accounts != null && accounts.Count > 0)
            {
                Console.WriteLine("╔═══════════════╦═══════════════════════════════════╦════════════════════╗");
                Console.WriteLine($"║{"Tên Tài Khoản",-15}║{"Mật khẩu",-35}║{"Vai trò",-20}║");
                Console.WriteLine("╠═══════════════╬═══════════════════════════════════╬════════════════════╣");
                var last = accounts.Where(x => x.VaiTro == Role.Staff).ToList().Last();
                foreach (var account in accounts.Where(x => x.VaiTro == Role.Staff).ToList())
                {
                    if (account.Equals(last))
                    {
                        Console.WriteLine($"║{account.Username,-15}║{account.Password,-35}║{"Nhân Viên",-20}║");
                        Console.WriteLine("╚═══════════════╩═══════════════════════════════════╩════════════════════╝");

                    }
                    else
                    {
                        Console.WriteLine($"║{account.Username,-15}║{account.Password,-35}║{"Nhân Viên",-20}║");
                        Console.WriteLine("╠═══════════════╬═══════════════════════════════════╬════════════════════╣");
                    }

                }
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                Console.ReadKey(true);
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy tài khoản nào");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                Console.ReadKey(true);
            }
        }
        private Role CheckRole(string username)
        {
            var temp = accounts.Find(x => x.Username == username);
            return temp.VaiTro;
        }
        public Account Login()
        {
        Start:
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"",30}╔══════════════════════════════════════════════╗");
            Console.WriteLine($"{"",30}║ ➽ Sinh viên thực hiện : Nguyễn Văn Khang     ║");
            Console.WriteLine($"{"",30}║ ➽ Mã SV : 12522046                           ║");
            Console.WriteLine($"{"",30}║ ➽ Mã Lớp : 125222                            ║");
            Console.WriteLine($"{"",30}║ ➽ Người Hướng Dẫn : Thầy Bùi Đức Thọ         ║");
            Console.WriteLine($"{"",30}╚══════════════════════════════════════════════╝");
            Console.WriteLine();
            ViewHelper.PrintWarning("Đăng nhập để sử dụng phần mềm");
            ViewHelper.PrintWarning("Bấm ESC để nhập lại từ đầu");

            try
            {
                var pos_username = ViewHelper.PrintInput("Nhập tài khoản");
                var pos_pass = ViewHelper.PrintInput("Nhập mật khẩu");
                var pos_end = Console.GetCursorPosition();
            Username:
                Console.SetCursorPosition(pos_username.Left, pos_username.Top);
                string username = ViewHelper.ReadLine();
                if (!Exist(username))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tài khoản không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_username.Left, pos_username.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", username.Length)));
                    goto Username;
                }
            Passsword:
                Console.SetCursorPosition(pos_pass.Left, pos_pass.Top);
                string password = ViewHelper.ReadLine(true);
                if (!IsPassword(username, password))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mật khẩu không chính xác");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_pass.Left, pos_pass.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", password.Length)));
                    goto Passsword;
                }

                return new Account() { Username = username, Password = password, VaiTro = CheckRole(username) };
            }
            catch (ExitException)
            {
                goto Start;
            }

        }
        public void Register(Role role)
        {
            Console.Clear();
            ViewHelper.PrintWarning("Tạo tài khoản");
            try
            {
                var pos_username = ViewHelper.PrintInput("Nhập tài khoản");
                var pos_pass = ViewHelper.PrintInput("Nhập mật khẩu");
                var pos_repass = ViewHelper.PrintInput("Nhập lại mật khẩu");
                var pos_end = Console.GetCursorPosition();
            Username:
                Console.SetCursorPosition(pos_username.Left, pos_username.Top);
                string username = ViewHelper.ReadLine();
                if (role == Role.Staff)
                {
                    if (!nhanVienController.Exist(username))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Vui lòng nhập tài khoản là mã nhân viên");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_username.Left, pos_username.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", username.Length)));
                        goto Username;

                    }
                }
                if (Exist(username))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tài khoản đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_username.Left, pos_username.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", username.Length)));
                    goto Username;
                }
            Password:
                Console.SetCursorPosition(pos_pass.Left, pos_pass.Top);
                string password = ViewHelper.ReadLine(true);
                if (password == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mật khẩu không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_pass.Left, pos_pass.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", password.Length)));
                    goto Password;
                }
                else if (password.Length<6)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mật khẩu quá ngắn");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_pass.Left, pos_pass.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", password.Length)));
                }
            Repassword:
                Console.SetCursorPosition(pos_repass.Left, pos_repass.Top);
                string repassword = ViewHelper.ReadLine(true);
                if (repassword != password)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mật khẩu không khớp. Vui lòng nhập lại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_repass.Left, pos_repass.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", repassword.Length)));
                    goto Repassword;
                }
                var temp = new Account() { Username = username, Password = Security.GetMd5Hash(password), VaiTro = role };
                accounts.Add(temp);
                Sync();
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess("Tạo tài khoản thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }

        }
        public void ChangePassword()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Đổi mật khẩu");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            try
            {
                var pos_username = ViewHelper.PrintInput("Nhập tài khoản");
                var pos_oldpass = ViewHelper.PrintInput("Nhập mật khẩu cũ");
                var pos_newpass = ViewHelper.PrintInput("Nhập mật khẩu mới");
                var pos_repass = ViewHelper.PrintInput("Nhập lại mật khẩu mới");
                var pos_end = Console.GetCursorPosition();
            Username:
                Console.SetCursorPosition(pos_username.Left, pos_username.Top);
                string username = ViewHelper.ReadLine();
                if (!Exist(username))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tài khoản không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_username.Left, pos_username.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", username.Length)));
                    goto Username;
                }
            OldPassword:
                Console.SetCursorPosition(pos_oldpass.Left, pos_oldpass.Top);
                string old_password = ViewHelper.ReadLine(true);
                if (!IsPassword(username, old_password))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mật khẩu không chính xác");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_oldpass.Left, pos_oldpass.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", old_password.Length)));
                    goto OldPassword;
                }
            NewPassword:
                Console.SetCursorPosition(pos_newpass.Left, pos_newpass.Top);
                string new_password = ViewHelper.ReadLine(true);
                if (new_password == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mật khẩu không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_newpass.Left, pos_newpass.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", new_password.Length)));
                    goto NewPassword;
                }
                if (new_password.Length < 5)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mật khẩu quá ngắn");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_newpass.Left, pos_newpass.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", new_password.Length)));
                    goto NewPassword;
                }
            Repassword:
                Console.SetCursorPosition(pos_repass.Left, pos_repass.Top);
                string repassword = ViewHelper.ReadLine(true);
                if (repassword != new_password)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mật khẩu không khớp. Vui lòng nhập lại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_repass.Left, pos_repass.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", repassword.Length)));
                    goto Repassword;
                }
                var account = FindAccount(username);
                account.Password = Security.GetMd5Hash(repassword);
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess("Thay đổi mật khẩu thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu.");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }
        }
        public void DeleteAccount()
        {
            try
            {
                Console.Clear();
                ViewHelper.PrintWarning("Xóa tài khoản");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                var pos_masp = ViewHelper.PrintInput("Nhập tài khoản:");
                var pos_end = Console.GetCursorPosition();

            Username:
                Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                string username = ViewHelper.ReadLine();
                if (!Exist(username))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tài khoản không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", username.Length)));
                    goto Username;
                }
                if (username == "quanly")
                {
                    return;
                }
                var account = accounts.Find(x => x.Username == username);
                Console.Clear();
                ShowAccount(account);
                ViewHelper.PrintWarning("Bạn có chắc chắn muốn xóa tài khoản trên.");
                ViewHelper.PrintWarning("Bấm Y để xác nhận. Bấm N để hủy.");
            XacNhan:
                var xacNhan = Console.ReadKey(true);
                if (xacNhan.Key == ConsoleKey.Y)
                {
                    accounts.Remove(account);
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
    }
}
