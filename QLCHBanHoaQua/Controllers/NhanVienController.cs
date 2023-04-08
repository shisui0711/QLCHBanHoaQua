using Framework;
using QLCHBanHoaQua.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            return nhanViens.FindAll(x => x.QueQuan.ToLower().Contains(address.ToLower()));
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
            NhanVien nhanVien = new NhanVien();
            try
            {
            MaNV:
                nhanVien.MaNV = ViewHelper.Input<string>("Nhập Mã Nhân Viên");
                if (Exist(nhanVien.MaNV))
                {
                    ViewHelper.PrintError("Mã mã nhân viên đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaNV;
                }
                else if (nhanVien.MaNV == "")
                {
                    ViewHelper.PrintError("Mã nhân viên không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaNV;
                }
            TenNV:
                string tenNV = ViewHelper.Input<string>("Nhập Tên Nhân Viên");
                if (tenNV == "")
                {
                    ViewHelper.PrintError("Tên nhân viên không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto TenNV;
                }
                else
                {
                    nhanVien.TenNV = Format.Name(tenNV);
                }
            GioiTinh:
                string gioiTinh = ViewHelper.Input<string>("Nhập giới tinh");
                if (Validate.Gender(gioiTinh))
                {
                    nhanVien.GioiTinh = gioiTinh;
                }
                else
                {
                    ViewHelper.PrintError("Giới tính không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto GioiTinh;
                }
                nhanVien.NgaySinh = ViewHelper.Input<DateTime>("Nhập ngày sinh định dạng (dd/mm/yyyy)");
            Phone:
                nhanVien.Phone = ViewHelper.Input<string>("Nhập SĐT Nhân Viên");
                if (!Validate.Phone(nhanVien.Phone))
                {
                    ViewHelper.PrintError("SĐT không hợp lệ. Vui lòng nhập lại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto Phone;
                }
            QueQuan:
                nhanVien.QueQuan = ViewHelper.Input<string>("Nhập Quê Quán Nhân Viên");
                if (nhanVien.QueQuan == "")
                {
                    ViewHelper.PrintError("Quê quán không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto QueQuan;
                }
                nhanViens.Add(nhanVien);
                Save(nhanVien);
                ViewHelper.PrintSuccess("Thêm khách hàng thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }  
        }
        public void SuaNV(string maNV)
        {
            var nhanVien = FindNV(maNV);
            if (nhanVien != null)
            {
                Console.Clear();
                ViewHelper.PrintWarning($"Cập nhật thông tin cho khách hàng {maNV}");
                ViewHelper.PrintWarning("Bỏ trống nếu không muốn cập nhật");
                try
                {
                    string tenNV = ViewHelper.Input<string>("Nhập Tên Nhân Viên");
                    if (tenNV != "")
                    {
                        nhanVien.TenNV = Format.Name(tenNV);
                    }
                GioiTinh:
                    string gioiTinh = ViewHelper.Input<string>("Nhập giới tinh");
                    if(gioiTinh != "")
                    {
                        if (Validate.Gender(gioiTinh))
                        {
                            nhanVien.GioiTinh = gioiTinh;
                        }
                        else
                        {
                            ViewHelper.PrintError("Giới tính không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
                            goto GioiTinh;
                        }
                    }
                ngaySinh:
                    string ngaySinh = ViewHelper.Input<string>("Nhập ngày sinh định dạng (dd/mm/yyyy)");
                    if(ngaySinh != "")
                    {
                        DateTime temp_ns;
                        if(DateTime.TryParse(ngaySinh,out temp_ns))
                        {
                            nhanVien.NgaySinh = temp_ns;
                        }
                        else
                        {
                            ViewHelper.PrintError("Ngày sinh không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
                            goto ngaySinh;
                        }
                    }
                Phone:
                    string phone = ViewHelper.Input<string>("Nhập SĐT Nhân Viên");
                    if(phone != "")
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
                            nhanVien.Phone = phone;
                        }
                    }
                    string queQuan = ViewHelper.Input<string>("Nhập Quê Quán Nhân Viên");
                    if (queQuan != "")
                    {
                        nhanVien.QueQuan = queQuan;
                    }
                }
                catch (ExitException)
                {
                    return;
                }

            }
            else
            {
                ViewHelper.PrintError($"Không Tồn Tại Nhân Viên {maNV}");
                Thread.Sleep(2000);
                return;
            }
        }
        public bool XoaNV(string maMV)
        {
            var nhanVien = nhanViens.Find(x => x.MaNV == maMV);
            if (nhanVien != null)
            {
                nhanViens.Remove(nhanVien);
                return true;
            }
            return false;
        }
        public void ShowNV(List<NhanVien> nhanViens)
        {
            Console.Clear();
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
            else
            {
                ViewHelper.PrintError("Không tìm thấy nhân viên nào hợp lệ");
            }
        }
        public void ShowNV(NhanVien nhanVien)
        {
            Console.Clear();
            Console.WriteLine("╔══════════╦════════════════════╦══════════╦══════════╦═══════════════╦════════════════════╗");
            Console.WriteLine($"║{"Mã NV",-10}║{"Tên NV",-20}║{"Giới tính",-10}║{"Ngày sinh",-10}║{"Số điện thoại",-15}║{"Quê Quán",-20}║");
            Console.WriteLine("╠══════════╬════════════════════╬══════════╬══════════╬═══════════════╬════════════════════╣");
            Console.WriteLine($"║{nhanVien.MaNV,-10}║{nhanVien.TenNV,-20}║{nhanVien.GioiTinh,-10}║" +
                            $"{nhanVien.NgaySinh.ToShortDateString(),-10}║{nhanVien.Phone,-15}║{nhanVien.QueQuan,-20}║");
            Console.WriteLine("╚══════════╩════════════════════╩══════════╩══════════╩═══════════════╩════════════════════╝");
        }
        public void ShowNV()
        {
            Console.Clear();
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
            else
            {
                ViewHelper.PrintError("Không tìm thấy nhân viên nào hợp lệ");
            }
        }

    }
}
