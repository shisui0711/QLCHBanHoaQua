using Framework;
using QLCHBanHoaQua.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

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
            if(name.Trim().Count(x=>x==' ') == 0)
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
            return khachHangs.FindAll(x => x.DiaChi.ToLower().Contains(address.ToLower()));
        }
        public void ShowKH(List<KhachHang> khachHangs)
        {
            Console.Clear();
            if(khachHangs != null && khachHangs.Count > 0)
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
            KhachHang khachHang = new KhachHang();
            
            try
            {
            MaKH:
                khachHang.MaKH = ViewHelper.Input<string>("Nhập Mã Khách Hàng");
                if (Exist(khachHang.MaKH))
                {
                    ViewHelper.PrintError("Mã khách hàng đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaKH;
                }
                else if (khachHang.MaKH == "")
                {
                    ViewHelper.PrintError("Mã khách hàng không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaKH;
                }
            TenKH:
                string tenKH = ViewHelper.Input<string>("Nhập Tên Khách Hàng");
                if (tenKH == "")
                {
                    ViewHelper.PrintError("Tên khách hàng không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto TenKH;
                }
                else
                {
                    khachHang.TenKH = Format.Name(tenKH);
                }
            DiaChi:
                khachHang.DiaChi = ViewHelper.Input<string>("Nhập Địa Chỉ Khách Hàng");
                if (khachHang.DiaChi == "")
                {
                    ViewHelper.PrintError("Địa chỉ không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto DiaChi;
                }
            Phone:
                khachHang.Phone = ViewHelper.Input<string>("Nhập SĐT Khách Hàng");
                if (!Validate.Phone(khachHang.Phone))
                {
                    ViewHelper.PrintError("SĐT không hợp lệ. Vui lòng nhập lại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Phone;
                }
                khachHangs.Add(khachHang);
                Save(khachHang);
                ViewHelper.PrintSuccess("Thêm khách hàng thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }
            
        }
        public void SuaKH(string maKH)
        {
            var khachHang = FindKH(maKH);
            if (khachHang != null)
            {
                Console.Clear();
                ViewHelper.PrintWarning($"Cập nhật thông tin cho khách hàng {maKH}");
                ViewHelper.PrintWarning("Bỏ trống nếu không muốn cập nhật");
                try
                {
                    string tenKH = ViewHelper.Input<string>("Nhập Tên Khách Hàng");
                    if(tenKH != "")
                    {
                        khachHang.TenKH = Format.Name(tenKH);
                    }
                Phone:
                    string phone = ViewHelper.Input<string>("Nhập SĐT Khách Hàng");
                    if (phone != "")
                    {
                        if (!Validate.Phone(phone))
                        {
                            ViewHelper.PrintError("SĐT không hợp lệ. Vui lòng nhập lại");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
                            goto Phone;
                        }
                        else
                        {
                            khachHang.Phone = phone;
                        }
                    }
                    string diaChi = ViewHelper.Input<string>("Nhập Địa Chỉ Khách Hàng");
                    if (diaChi != "")
                    {
                        khachHang.DiaChi = diaChi;
                    }
                    Sync(khachHangs);
                }
                catch (ExitException)
                {
                    return;
                }
            
            }
            else
            {
                ViewHelper.PrintError($"Không Tồn Tại Khách Hàng Có Mã Khách Hàng là {maKH}");
                Thread.Sleep(2000);
                return;
            }
        }
        public bool XoaKH(string maKH)
        {
            var khachHang = khachHangs.Find(x => x.MaKH == maKH);
            if(khachHang != null)
            {
                khachHangs.Remove(khachHang);
                return true;
            }
            return false;
        }
    }
}
