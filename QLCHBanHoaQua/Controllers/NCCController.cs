using Framework;
using QLCHBanHoaQua.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua.Controllers
{
    class NCCController
    {
        private const string FileNguonGoc = @"data\nhacungcap.txt";
        public static List<NhaCC> DanhSachNCC { get; set; }
        public NCCController()
        {
            DanhSachNCC = new List<NhaCC>();
            Load();
        }
        private void Load()
        {
            using (var fs = new FileStream(FileNguonGoc, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var tmp = line.Trim().Split("|");
                        NhaCC nhaCC = new NhaCC();
                        nhaCC.MaNCC = tmp[0];
                        nhaCC.TenNCC = tmp[1];
                        nhaCC.DiaChi = tmp[2];
                        nhaCC.Phone = tmp[3];
                        DanhSachNCC.Add(nhaCC);
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
        private void Save(NhaCC nhaCC)
        {
            File.AppendAllText(FileNguonGoc,
                $"{nhaCC.MaNCC}|{nhaCC.TenNCC}|{nhaCC.DiaChi}|{nhaCC.Phone}\n");
        }
        private void Sync()
        {
            File.WriteAllText(FileNguonGoc, "");
            foreach (NhaCC nhaCC in DanhSachNCC)
            {
                File.AppendAllText(FileNguonGoc,
                $"{nhaCC.MaNCC}|{nhaCC.TenNCC}|{nhaCC.DiaChi}|{nhaCC.Phone}\n");
            }
        }
        public bool Exist(string maNCC)
        {
            return DanhSachNCC.Any(x => x.MaNCC == maNCC);
        }
        public NhaCC FindNCC(string maNCC)
        {
            return DanhSachNCC.Find(x => x.MaNCC == maNCC);
        }
        public NhaCC FindByName(string tenNCC)
        {
            return DanhSachNCC.Find(x => x.TenNCC.ToLower() == tenNCC.ToLower());
        }
        public void ThemNCC()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Thêm nhà cung cấp");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            NhaCC nhaCC = new NhaCC();
            try
            {

                var pos_mancc = ViewHelper.PrintInput("Nhập Mã Nhà Cung Cấp:");
                var pos_ten = ViewHelper.PrintInput("Nhập Tên Nhà Cung Cấp:");
                var pos_diachi = ViewHelper.PrintInput("Nhập Địa Chỉ Nhà Cung Cấp:");
                var pos_phone = ViewHelper.PrintInput("Nhập Số Điện Thoại Nhà Cung Cấp:");
                var pos_end = Console.GetCursorPosition();
            MaNCC:
                Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                nhaCC.MaNCC = ViewHelper.ReadLine();
                if (Exist(nhaCC.MaNCC))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhà cung cấp đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", nhaCC.MaNCC.Length)));
                    goto MaNCC;
                }
                else if (nhaCC.MaNCC == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhà cung cấp không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", nhaCC.MaNCC.Length)));
                    goto MaNCC;
                }
            TenNCC:
                Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                string tenNCC = ViewHelper.ReadLine();
                if (tenNCC == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tên nhà cung cấp không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", tenNCC.Length)));
                    goto TenNCC;
                }
                else
                {
                    nhaCC.TenNCC = Format.Name(tenNCC);
                }
            DiaChi:
                Console.SetCursorPosition(pos_diachi.Left, pos_diachi.Top);
                nhaCC.DiaChi = ViewHelper.ReadLine();
                if (nhaCC.DiaChi.Trim() == string.Empty)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tên nhà cung cấp không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_diachi.Left, pos_diachi.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", nhaCC.DiaChi.Length)));
                    goto DiaChi;
                }
            Phone:
                Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                nhaCC.Phone = ViewHelper.ReadLine();
                if (!Validate.Phone(nhaCC.Phone))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Số điện thoại không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", nhaCC.Phone.Length)));
                    goto Phone;
                }
                DanhSachNCC.Add(nhaCC);
                Save(nhaCC);
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess("Thêm nhà cung cấp thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }
        }
        public void SuaNCC()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Sửa thông tin nhà cung cấp");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            ViewHelper.PrintWarning("Bỏ trống nếu không muốn cập nhật");
            try
            {
                var pos_mancc = ViewHelper.PrintInput("Nhập Mã Nhà Cung Cấp:");
                var pos_ten = ViewHelper.PrintInput("Nhập Tên Nhà Cung Cấp:");
                var pos_diachi = ViewHelper.PrintInput("Nhập Địa Chỉ Nhà Cung Cấp:");
                var pos_phone = ViewHelper.PrintInput("Nhập Số Điện Thoại Nhà Cung Cấp:");
                var pos_end = Console.GetCursorPosition();

            MaNCC:
                Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                string maNCC = ViewHelper.ReadLine();
                if (!Exist(maNCC))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhà cung cấp không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maNCC.Length)));
                    goto MaNCC;
                }
                var nhaCC = FindNCC(maNCC);
                Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                string tenNCC = ViewHelper.ReadLine();
                if (tenNCC != "")
                {
                    nhaCC.TenNCC = Format.Name(tenNCC);
                }
                Console.SetCursorPosition(pos_diachi.Left, pos_diachi.Top);
                string diaChi = ViewHelper.ReadLine();
                if (diaChi != "")
                {
                    nhaCC.DiaChi = diaChi;
                }
            Phone:
                Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                string phone = ViewHelper.ReadLine();
                if (phone != "")
                {
                    if (!Validate.Phone(phone))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Số điện thoại không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_phone.Left, pos_phone.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", phone.Length)));
                        goto Phone;
                    }
                    else
                    {
                        nhaCC.Phone = phone;
                    }
                }
                Sync();
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
        public void XoaNCC()
        {
            try
            {
                Console.Clear();
                ViewHelper.PrintWarning("Xóa nhà cung cấp");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                var pos_macc = ViewHelper.PrintInput("Nhập Mã Nhà Cung Cấp:");
                var pos_end = Console.GetCursorPosition();

            MaNCC:
                Console.SetCursorPosition(pos_macc.Left, pos_macc.Top);
                string maNCC = ViewHelper.ReadLine();
                if (!Exist(maNCC))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhân viên không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_macc.Left, pos_macc.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maNCC.Length)));
                    goto MaNCC;
                }
                var nhaCC = DanhSachNCC.Find(x => x.MaNCC == maNCC);
                Console.Clear();
                ShowNCC(nhaCC);
                ViewHelper.PrintWarning("Bạn có chắc chắn muốn xóa nhà cung cấp trên.");
                ViewHelper.PrintWarning("Bấm Y để xác nhận. Bấm N để hủy.");
            XacNhan:
                var xacNhan = Console.ReadKey(true);
                if (xacNhan.Key == ConsoleKey.Y)
                {
                    DanhSachNCC.Remove(nhaCC);
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
        public void ShowNCC(List<NhaCC> DanhSachNCC)
        {

            if (DanhSachNCC != null && DanhSachNCC.Count > 0)
            {
                Console.WriteLine("╔════════════════════╦══════════════════════════════╦════════════════════╦════════════════════╗");
                Console.WriteLine($"║{"Mã Nhã Cung Cấp",-20}║{"Tên Nhà Cung Cấp",-30}║{"Địa Chỉ",-20}║{"Số điện thoại",-20}║");
                Console.WriteLine("╠════════════════════╬══════════════════════════════╬════════════════════╬════════════════════╣");
                var last = DanhSachNCC.Last();
                foreach (NhaCC nhaCC in DanhSachNCC)
                {
                    if (nhaCC.Equals(last))
                    {
                        Console.WriteLine($"║{nhaCC.MaNCC,-20}║{nhaCC.TenNCC,-30}║{nhaCC.DiaChi,-20}║{nhaCC.Phone,-20}║");
                        Console.WriteLine("╚════════════════════╩══════════════════════════════╩════════════════════╩════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{nhaCC.MaNCC,-20}║{nhaCC.TenNCC,-30}║{nhaCC.DiaChi,-20}║{nhaCC.Phone,-20}║");
                        Console.WriteLine("╠════════════════════╬══════════════════════════════╬════════════════════╬════════════════════╣");
                    }
                }
            }
        }
        public void ShowNCC(NhaCC nhaCC)
        {
            Console.WriteLine("╔════════════════════╦══════════════════════════════╦════════════════════╦════════════════════╗");
            Console.WriteLine($"║{"Mã Nhã Cung Cấp",-20}║{"Tên Nhà Cung Cấp",-30}║{"Địa Chỉ",-20}║{"Số điện thoại",-20}║");
            Console.WriteLine("╠════════════════════╬══════════════════════════════╬════════════════════╬════════════════════╣");
            Console.WriteLine($"║{nhaCC.MaNCC,-20}║{nhaCC.TenNCC,-30}║{nhaCC.DiaChi,-20}║{nhaCC.Phone,-20}║");
            Console.WriteLine("╚════════════════════╩══════════════════════════════╩════════════════════╩════════════════════╝");
        }
        public void ShowNCC()
        {

            if (DanhSachNCC != null && DanhSachNCC.Count > 0)
            {
                Console.WriteLine("╔════════════════════╦══════════════════════════════╦════════════════════╦════════════════════╗");
                Console.WriteLine($"║{"Mã Nhã Cung Cấp",-20}║{"Tên Nhà Cung Cấp",-30}║{"Địa Chỉ",-20}║{"Số điện thoại",-20}║");
                Console.WriteLine("╠════════════════════╬══════════════════════════════╬════════════════════╬════════════════════╣");
                var last = DanhSachNCC.Last();
                foreach (NhaCC nhaCC in DanhSachNCC)
                {
                    if (nhaCC.Equals(last))
                    {
                        Console.WriteLine($"║{nhaCC.MaNCC,-20}║{nhaCC.TenNCC,-30}║{nhaCC.DiaChi,-20}║{nhaCC.Phone,-20}║");
                        Console.WriteLine("╚════════════════════╩══════════════════════════════╩════════════════════╩════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{nhaCC.MaNCC,-20}║{nhaCC.TenNCC,-30}║{nhaCC.DiaChi,-20}║{nhaCC.Phone,-20}║");
                        Console.WriteLine("╠════════════════════╬══════════════════════════════╬════════════════════╬════════════════════╣");
                    }
                }
            }
        }
    }
}
