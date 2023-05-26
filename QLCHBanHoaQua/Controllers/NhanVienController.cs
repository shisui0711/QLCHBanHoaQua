using Framework;
using QLCHBanHoaQua.Models;

namespace QLCHBanHoaQua.Controllers
{
    class NhanVienController
    {
        private const string NhanVienFile = @"data\nhanvien.txt";
        public static List<NhanVien> nhanViens { get; private set; }
        public NhanVienController()
        {
            nhanViens = new List<NhanVien>();
            Load(nhanViens);

        }
        private void Load(List<NhanVien> nhanViens)
        {
            using (var fs = new FileStream(NhanVienFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var tmp = line.Trim().Split("|");
                        NhanVien nhanVien = new NhanVien();
                        nhanVien.MaNV = tmp[0];
                        nhanVien.TenNV = tmp[1];
                        nhanVien.NgaySinh = DateTime.Parse(tmp[2]);
                        nhanVien.Phone = tmp[3];
                        nhanVien.GioiTinh = tmp[4];
                        nhanVien.QueQuan = tmp[5];
                        nhanViens.Add(nhanVien);
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
        private void Save(NhanVien nhanVien)
        {
            File.AppendAllText(NhanVienFile,
                $"{nhanVien.MaNV}|{nhanVien.TenNV}|{nhanVien.NgaySinh.ToShortDateString()}|" +
                $"{nhanVien.Phone}|{nhanVien.GioiTinh}|{nhanVien.QueQuan}\n");
        }
        private void Sync(List<NhanVien> nhanViens)
        {
            File.WriteAllText(NhanVienFile, "");
            foreach (NhanVien nhanVien in nhanViens)
            {
                File.AppendAllText(NhanVienFile,
                $"{nhanVien.MaNV}|{nhanVien.TenNV}|{nhanVien.NgaySinh.ToShortDateString()}|" +
                $"{nhanVien.Phone}|{nhanVien.GioiTinh}|{nhanVien.QueQuan}\n");
            }
        }
        public bool Exist(string maNV)
        {
            try
            {
                var temp = nhanViens.Find(x => x.MaNV == maNV);
                if (temp != null) return true;
            }
            catch (Exception)
            {

                return false;
            }

            return false;
        }
        public NhanVien FindNV(string maNV)
        {
            if(maNV == "Quản lý")
            {
                return new NhanVien() { MaNV = "Quản lý", TenNV = "Quản lý" };
            }
            return nhanViens.Find(x => x.MaNV == maNV);
        }
        public List<NhanVien> FindByName(string name)
        {
            if (name.Trim().Count(x => x == ' ') == 0)
            {
                return nhanViens.FindAll(x => Format.SplitLastName(x.TenNV).ToLower() == name.ToLower());
            }
            else
            {
                return nhanViens.FindAll(x => x.TenNV.ToLower() == name.ToLower());
            }
        }
        public List<NhanVien> FindByAddress(string address)
        {
            return nhanViens.FindAll(x => x.QueQuan.ToLower().Equals(address.ToLower()));
        }
        public List<NhanVien> MaleFilter()
        {
            return nhanViens.FindAll(x => x.GioiTinh.ToLower().Contains("nam"));
        }
        public List<NhanVien> FemaleFilter()
        {
            return nhanViens.FindAll(x => x.GioiTinh.ToLower().Contains("nữ") || x.GioiTinh.ToLower().Contains("nu"));
        }
        public void ThemNV()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Thêm nhân viên");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            NhanVien nhanVien = new NhanVien();
            try
            {

                var pos_manv = ViewHelper.PrintInput("Nhập Mã Nhân Viên:");
                var pos_ten = ViewHelper.PrintInput("Nhập Tên Nhân Viên:");
                var pos_gt = ViewHelper.PrintInput("Nhập giới tính:");
                var pos_ngaysinh = ViewHelper.PrintInput("Nhập ngày sinh định dạng (dd/mm/yyyy)");
                var pos_phone = ViewHelper.PrintInput("Nhập SĐT Nhân Viên");
                var pos_quequan = ViewHelper.PrintInput("Nhập Quê Quán Nhân Viên");
                var pos_end = Console.GetCursorPosition();
            MaNV:
                Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                nhanVien.MaNV = ViewHelper.ReadLine();
                if (Exist(nhanVien.MaNV))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhân viên đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", nhanVien.MaNV.Length)));
                    goto MaNV;
                }
                else if (nhanVien.MaNV == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhân viên không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", nhanVien.MaNV.Length)));
                    goto MaNV;
                }
            TenNV:
                Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                string tenNV = ViewHelper.ReadLine();
                if (tenNV == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tên nhân viên không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", tenNV.Length)));
                    goto TenNV;
                }
                else
                {
                    nhanVien.TenNV = Format.Name(tenNV);
                }
            GioiTinh:
                Console.SetCursorPosition(pos_gt.Left, pos_gt.Top);
                string gioiTinh = ViewHelper.ReadLine();
                if (Validate.Gender(gioiTinh))
                {
                    nhanVien.GioiTinh = gioiTinh;
                }
                else
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Giới tính không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_gt.Left, pos_gt.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", gioiTinh.Length)));
                    goto GioiTinh;
                }
            NgaySinh:
                Console.SetCursorPosition(pos_ngaysinh.Left, pos_ngaysinh.Top);
                string ngaySinh = ViewHelper.ReadLine();
                try
                {
                    nhanVien.NgaySinh = DateTime.Parse(ngaySinh);
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Ngày sinh không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_ngaysinh.Left, pos_ngaysinh.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", ngaySinh.Length)));
                    goto NgaySinh;
                }
            Phone:
                Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                nhanVien.Phone = ViewHelper.ReadLine();
                if (!Validate.Phone(nhanVien.Phone))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("SĐT không hợp lệ. Vui lòng nhập lại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", nhanVien.Phone.Length)));
                    goto Phone;
                }
            QueQuan:
                Console.SetCursorPosition(pos_quequan.Left, pos_quequan.Top);
                nhanVien.QueQuan = ViewHelper.ReadLine();
                if (nhanVien.QueQuan == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Quê quán không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_quequan.Left, pos_quequan.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", nhanVien.QueQuan.Length)));
                    goto QueQuan;
                }
                nhanViens.Add(nhanVien);
                Save(nhanVien);
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess("Thêm nhân viên thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }
        }
        public void SuaNV()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Sửa thông tin nhân viên");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            ViewHelper.PrintWarning("Bỏ trống nếu không muốn cập nhật");
            try
            {
                var pos_manv = ViewHelper.PrintInput("Nhập Mã Nhân Viên:");
                var pos_ten = ViewHelper.PrintInput("Nhập Tên Nhân Viên:");
                var pos_gt = ViewHelper.PrintInput("Nhập giới tính:");
                var pos_ngaysinh = ViewHelper.PrintInput("Nhập ngày sinh định dạng (dd/mm/yyyy)");
                var pos_phone = ViewHelper.PrintInput("Nhập SĐT Nhân Viên");
                var pos_quequan = ViewHelper.PrintInput("Nhập Quê Quán Nhân Viên");
                var pos_end = Console.GetCursorPosition();

            MaNV:
                Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                string maNV = ViewHelper.ReadLine();
                if (!Exist(maNV))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhân viên không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maNV.Length)));
                    goto MaNV;
                }
                var nhanVien = FindNV(maNV);
                Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                string tenNV = ViewHelper.ReadLine();
                if (tenNV != "")
                {
                    nhanVien.TenNV = Format.Name(tenNV);
                }
            GioiTinh:
                Console.SetCursorPosition(pos_gt.Left, pos_gt.Top);
                string gioiTinh = ViewHelper.ReadLine();
                if (gioiTinh != "")
                {
                    if (Validate.Gender(gioiTinh))
                    {
                        nhanVien.GioiTinh = gioiTinh;
                    }
                    else
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Giới tính không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_gt.Left, pos_gt.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", gioiTinh.Length)));
                        goto GioiTinh;
                    }
                }
            ngaySinh:
                Console.SetCursorPosition(pos_ngaysinh.Left, pos_ngaysinh.Top);
                string ngaySinh = ViewHelper.ReadLine();
                if (ngaySinh != "")
                {
                    try
                    {
                        nhanVien.NgaySinh = DateTime.Parse(ngaySinh);
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Ngày sinh không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_ngaysinh.Left, pos_ngaysinh.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", ngaySinh.Length)));
                        goto ngaySinh;
                    }
                }
            Phone:
                Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                string phone = ViewHelper.ReadLine();
                if (phone != "")
                {
                    if (!Validate.Phone(phone))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("SĐT không hợp lệ. Vui lòng nhập lại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", phone.Length)));
                        goto Phone;
                    }
                    else
                    {
                        nhanVien.Phone = phone;
                    }
                }
                Console.SetCursorPosition(pos_quequan.Left, pos_quequan.Top);
                string queQuan = ViewHelper.ReadLine();
                if (queQuan != "")
                {
                    nhanVien.QueQuan = queQuan;
                }
                Sync(nhanViens);
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess("Cập nhật thông tin thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }

        }
        public void XoaNV()
        {
            try
            {
                Console.Clear();
                ViewHelper.PrintWarning("Xóa nhân viên");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                var pos_manv = ViewHelper.PrintInput("Nhập Mã Nhân Viên:");
                var pos_end = Console.GetCursorPosition();

            MaNV:
                Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                string maNV = ViewHelper.ReadLine();
                if (!Exist(maNV))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhân viên không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_manv.Left, pos_manv.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maNV.Length)));
                    goto MaNV;
                }
                var nhanVien = nhanViens.Find(x => x.MaNV == maNV);
                Console.Clear();
                ShowNV(nhanVien);
                ViewHelper.PrintWarning("Bạn có chắc chắn muốn xóa nhân viên trên.");
                ViewHelper.PrintWarning("Bấm Y để xác nhận. Bấm N để hủy.");
            XacNhan:
                var xacNhan = Console.ReadKey(true);
                if (xacNhan.Key == ConsoleKey.Y)
                {
                    nhanViens.Remove(nhanVien);
                    Sync(nhanViens);
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
        public void ShowNV(List<NhanVien> nhanViens)
        {
          
            if (nhanViens != null && nhanViens.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════╦══════════╦═══════════════╦════════════════════╗");
                Console.WriteLine($"║{"Mã NV",-10}║{"Tên NV",-20}║{"Giới tính",-10}║{"Ngày sinh",-10}║{"Số điện thoại",-15}║{"Quê Quán",-20}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════╬══════════╬═══════════════╬════════════════════╣");
                var last = nhanViens.Last();
                foreach (NhanVien nhanVien in nhanViens)
                {
                    if (nhanVien.Equals(last))
                    {
                        Console.WriteLine($"║{nhanVien.MaNV,-10}║{nhanVien.TenNV,-20}║{nhanVien.GioiTinh,-10}║" +
                            $"{nhanVien.NgaySinh.ToShortDateString(),-10}║{nhanVien.Phone,-15}║{nhanVien.QueQuan,-20}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════╩══════════╩═══════════════╩════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{nhanVien.MaNV,-10}║{nhanVien.TenNV,-20}║{nhanVien.GioiTinh,-10}║" +
                            $"{nhanVien.NgaySinh.ToShortDateString(),-10}║{nhanVien.Phone,-15}║{nhanVien.QueQuan,-20}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════╬══════════╬═══════════════╬════════════════════╣");
                    }
                }
            }
        }
        public void ShowNV(NhanVien nhanVien)
        {
            
            Console.WriteLine("╔══════════╦════════════════════╦══════════╦══════════╦═══════════════╦════════════════════╗");
            Console.WriteLine($"║{"Mã NV",-10}║{"Tên NV",-20}║{"Giới tính",-10}║{"Ngày sinh",-10}║{"Số điện thoại",-15}║{"Quê Quán",-20}║");
            Console.WriteLine("╠══════════╬════════════════════╬══════════╬══════════╬═══════════════╬════════════════════╣");
            Console.WriteLine($"║{nhanVien.MaNV,-10}║{nhanVien.TenNV,-20}║{nhanVien.GioiTinh,-10}║" +
                            $"{nhanVien.NgaySinh.ToShortDateString(),-10}║{nhanVien.Phone,-15}║{nhanVien.QueQuan,-20}║");
            Console.WriteLine("╚══════════╩════════════════════╩══════════╩══════════╩═══════════════╩════════════════════╝");
        }
        public void ShowNV()
        {
            
            if (nhanViens != null && nhanViens.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════╦══════════╦═══════════════╦════════════════════╗");
                Console.WriteLine($"║{"Mã NV",-10}║{"Tên NV",-20}║{"Giới tính",-10}║{"Ngày sinh",-10}║{"Số điện thoại",-15}║{"Quê Quán",-20}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════╬══════════╬═══════════════╬════════════════════╣");
                var last = nhanViens.Last();
                foreach (NhanVien nhanVien in nhanViens)
                {
                    if (nhanVien.Equals(last))
                    {
                        Console.WriteLine($"║{nhanVien.MaNV,-10}║{nhanVien.TenNV,-20}║{nhanVien.GioiTinh,-10}║" +
                            $"{nhanVien.NgaySinh.ToShortDateString(),-10}║{nhanVien.Phone,-15}║{nhanVien.QueQuan,-20}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════╩══════════╩═══════════════╩════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{nhanVien.MaNV,-10}║{nhanVien.TenNV,-20}║{nhanVien.GioiTinh,-10}║" +
                            $"{nhanVien.NgaySinh.ToShortDateString(),-10}║{nhanVien.Phone,-15}║{nhanVien.QueQuan,-20}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════╬══════════╬═══════════════╬════════════════════╣");
                    }
                }
            }
        }

    }
}
