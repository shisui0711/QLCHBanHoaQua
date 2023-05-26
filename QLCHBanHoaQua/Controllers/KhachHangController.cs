using Framework;
using QLCHBanHoaQua.Models;

namespace QLCHBanHoaQua.Controllers
{
    class KhachHangController
    {
        private const string KhachHangFile = @"data\khachhang.txt";
        public static List<KhachHang> khachHangs { get; private set; }
        public KhachHangController()
        {
            khachHangs = new List<KhachHang>();
            Load(khachHangs);
        }
        private void Load(List<KhachHang> khachHangs)
        {
            using (var fs = new FileStream(KhachHangFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var tmp = line.Trim().Split("|");
                        KhachHang khachHang = new KhachHang();
                        khachHang.MaKH = tmp[0];
                        khachHang.TenKH = tmp[1];
                        khachHang.DiaChi = tmp[2];
                        khachHang.Phone = tmp[3];
                        khachHangs.Add(khachHang);
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
        private void Save(KhachHang khachHang)
        {
            File.AppendAllText(KhachHangFile, $"{khachHang.MaKH}|{khachHang.TenKH}|{khachHang.DiaChi}|{khachHang.Phone}\n");
        }
        private void Sync(List<KhachHang> khachHangs)
        {
            File.WriteAllText(KhachHangFile, "");
            foreach (KhachHang khachHang in khachHangs)
            {
                File.AppendAllText(KhachHangFile,
                    $"{khachHang.MaKH}|{khachHang.TenKH}|" +
                    $"{khachHang.DiaChi}|{khachHang.Phone}\n"
                    );
            }
        }
        public bool Exist(string maKH)
        {
            try
            {
                var temp = khachHangs.Find(x => x.MaKH == maKH);
                if (temp != null) return true;
            }
            catch (Exception)
            {

                return false;
            }

            return false;
        }
        public KhachHang FindKH(string MaKH)
        {
            return khachHangs.Find(x => x.MaKH == MaKH);
        }
        public List<KhachHang> FindByName(string name)
        {
            if (name.Trim().Count(x => x == ' ') == 0)
            {
                return khachHangs.FindAll(x => Format.SplitLastName(x.TenKH).ToLower() == name.ToLower());
            }
            else
            {
                return khachHangs.FindAll(x => x.TenKH.ToLower() == name.ToLower());
            }
        }
        public List<KhachHang> FindByAddress(string address)
        {
            return khachHangs.FindAll(x => x.DiaChi.ToLower().Equals(address.ToLower()));
        }
        public void ShowKH(List<KhachHang> khachHangs)
        {
            Console.Clear();
            if (khachHangs != null && khachHangs.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════════════════════════╦═══════════════╗");
                Console.WriteLine($"║{"Mã KH",-10}║{"Tên KH",-20}║{"Địa chỉ",-30}║{"Số điện thoại",-15}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬═══════════════╣");
                var last = khachHangs.Last();
                foreach (KhachHang khachHang in khachHangs)
                {
                    if (khachHang.Equals(last))
                    {
                        Console.WriteLine($"║{khachHang.MaKH,-10}║{khachHang.TenKH,-20}║{khachHang.DiaChi,-30}║{khachHang.Phone,-15}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════════════════════════╩═══════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{khachHang.MaKH,-10}║{khachHang.TenKH,-20}║{khachHang.DiaChi,-30}║{khachHang.Phone,-15}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬═══════════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy khách hàng nào hợp lệ");
            }
        }
        public void ShowKH()
        {
            Console.Clear();
            if (khachHangs != null && khachHangs.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════════════════════════╦═══════════════╗");
                Console.WriteLine($"║{"Mã KH",-10}║{"Tên KH",-20}║{"Địa chỉ",-30}║{"Số điện thoại",-15}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬═══════════════╣");
                var last = khachHangs.Last();
                foreach (KhachHang khachHang in khachHangs)
                {
                    if (khachHang.Equals(last))
                    {
                        Console.WriteLine($"║{khachHang.MaKH,-10}║{khachHang.TenKH,-20}║{khachHang.DiaChi,-30}║{khachHang.Phone,-15}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════════════════════════╩═══════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{khachHang.MaKH,-10}║{khachHang.TenKH,-20}║{khachHang.DiaChi,-30}║{khachHang.Phone,-15}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬═══════════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Danh sách khách hàng trống");
            }
        }
        public void ShowKH(KhachHang khachHang)
        {
            Console.WriteLine("╔══════════╦════════════════════╦══════════════════════════════╦═══════════════╗");
            Console.WriteLine($"║{"Mã KH",-10}║{"Tên KH",-20}║{"Địa chỉ",-30}║{"Số điện thoại",-15}║");
            Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬═══════════════╣");
            Console.WriteLine($"║{khachHang.MaKH,-10}║{khachHang.TenKH,-20}║{khachHang.DiaChi,-30}║{khachHang.Phone,-15}║");
            Console.WriteLine("╚══════════╩════════════════════╩══════════════════════════════╩═══════════════╝");
        }

        public void ThemKH()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Thêm khách hàng");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            KhachHang khachHang = new KhachHang();

            try
            {
                var pos_makh = ViewHelper.PrintInput("Nhập Mã Khách Hàng");
                var pos_tenkh = ViewHelper.PrintInput("Nhập Tên Khách Hàng");
                var pos_dichi = ViewHelper.PrintInput("Nhập Địa Chỉ Khách Hàng");
                var pos_phone = ViewHelper.PrintInput("Nhập SĐT Khách Hàng");
                var pos_end = Console.GetCursorPosition();
            MaKH:
                Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                khachHang.MaKH = ViewHelper.ReadLine();
                if (Exist(khachHang.MaKH))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã khách hàng đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", khachHang.MaKH.Length)));
                    goto MaKH;
                }
                else if (khachHang.MaKH == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã khách hàng không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", khachHang.MaKH.Length)));
                    goto MaKH;
                }
            TenKH:
                Console.SetCursorPosition(pos_tenkh.Left, pos_tenkh.Top);
                string tenKH = ViewHelper.ReadLine();
                if (tenKH == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tên khách hàng không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", tenKH.Length)));
                    goto TenKH;
                }
                else
                {
                    khachHang.TenKH = Format.Name(tenKH);
                }
            DiaChi:
                Console.SetCursorPosition(pos_dichi.Left, pos_dichi.Top);
                khachHang.DiaChi = ViewHelper.ReadLine();
                if (khachHang.DiaChi == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Địa chỉ không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_dichi.Left, pos_dichi.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", khachHang.DiaChi.Length)));
                    goto DiaChi;
                }
            Phone:
                Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                khachHang.Phone = ViewHelper.ReadLine();
                if (!Validate.Phone(khachHang.Phone))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("SĐT không hợp lệ. Vui lòng nhập lại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", khachHang.Phone.Length)));
                    goto Phone;
                }
                khachHangs.Add(khachHang);
                Save(khachHang);
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess("Thêm khách hàng thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }

        }
        public void SuaKH()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Cập nhật thông tin khách hàng");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            ViewHelper.PrintWarning("Bỏ trống nếu không muốn cập nhật");
            try
            {
                var pos_makh = ViewHelper.PrintInput("Nhập Mã Khách Hàng");
                var pos_tenkh = ViewHelper.PrintInput("Nhập Tên Khách Hàng");
                var pos_dichi = ViewHelper.PrintInput("Nhập Địa Chỉ Khách Hàng");
                var pos_phone = ViewHelper.PrintInput("Nhập SĐT Khách Hàng");
                var pos_end = Console.GetCursorPosition();
            MaKH:
                Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                string maKH = ViewHelper.ReadLine();
                if (!Exist(maKH))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã khách hàng không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maKH.Length)));
                    goto MaKH;
                }
                var khachHang = FindKH(maKH);
                Console.SetCursorPosition(pos_tenkh.Left, pos_tenkh.Top);
                string tenKH = ViewHelper.ReadLine();
                if (tenKH != "")
                {
                    khachHang.TenKH = Format.Name(tenKH);
                }
                Console.SetCursorPosition(pos_dichi.Left, pos_dichi.Top);
                string diaChi = ViewHelper.ReadLine();
                if (diaChi != "")
                {
                    khachHang.DiaChi = diaChi;
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
                        khachHang.Phone = phone;
                    }
                }
                Sync(khachHangs);
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
        public void XoaKH()
        {

            try
            {
                Console.Clear();
                ViewHelper.PrintWarning("Xóa khách hàng");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                var pos_makh = ViewHelper.PrintInput("Nhập Mã Khách Hàng:");
                var pos_end = Console.GetCursorPosition();

            MaKH:
                Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                string maKH = ViewHelper.ReadLine();
                if (!Exist(maKH))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã khách hàng không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_makh.Left, pos_makh.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maKH.Length)));
                    goto MaKH;
                }
                var khachHang = khachHangs.Find(x => x.MaKH == maKH);
                Console.Clear();
                ShowKH(khachHang);
                ViewHelper.PrintWarning("Bạn có chắc chắn muốn xóa khách hàng trên.");
                ViewHelper.PrintWarning("Bấm Y để xác nhận. Bấm N để hủy.");
            XacNhan:
                var xacNhan = Console.ReadKey(true);
                if (xacNhan.Key == ConsoleKey.Y)
                {
                    khachHangs.Remove(khachHang);
                    Sync(khachHangs);
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
