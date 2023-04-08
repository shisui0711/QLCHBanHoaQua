using Framework;
using QLCHBanHoaQua.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua.Controllers
{
    class SanPhamController
    {
        private const string SanPhamFile = @"data\sanpham.txt";
        private const string LichSuFile = @"data\lichsunhap.txt";
        public static List<SanPham> sanPhams { get; private set; }
        public SanPhamController()
        {
            sanPhams = new List<SanPham>();
            Load(sanPhams);
        }
        private void GhiLS(SanPham sanPham,int soLuong)
        {
            File.AppendAllText(LichSuFile,
                $"{sanPham.MaSP}|{sanPham.TenSP}|{soLuong}|{sanPham.GiaNhap}" +
                $"{DateTime.Now}\n");
        }
        private void Load(List<SanPham> sanPhams)
        {
            using (var fs = new FileStream(SanPhamFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var tmp = line.Trim().Split("|");
                        SanPham sanPham = new SanPham();
                        sanPham.MaSP = tmp[0];
                        sanPham.TenSP = tmp[1];
                        sanPham.SoLuong = int.Parse(tmp[2]);
                        sanPham.GiaNhap = double.Parse(tmp[3]);
                        sanPham.GiaBan = double.Parse(tmp[4]);
                        sanPhams.Add(sanPham);
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
        private void Save(SanPham sanPham)
        {
            File.AppendAllText(SanPhamFile, 
                $"{sanPham.MaSP}|{sanPham.TenSP}|{sanPham.SoLuong}|" +
                $"{sanPham.GiaNhap}|{sanPham.GiaBan}\n");
        }
        private void Sync(List<SanPham> sanPhams)
        {
            File.WriteAllText(SanPhamFile, "");
            foreach (SanPham sanPham in sanPhams)
            {
                File.AppendAllText(SanPhamFile,
                $"{sanPham.MaSP}|{sanPham.TenSP}|{sanPham.SoLuong}|" +
                $"{sanPham.GiaNhap}|{sanPham.GiaBan}\n");
            }
        }
        public bool Exist(string maSP)
        {
            try
            {
                var temp = sanPhams.Find(x => x.MaSP == maSP);
                if (temp != null) return true;
            }
            catch (Exception)
            {

                return false;
            }

            return false;
        }
        public SanPham FindSP(string maSP)
        {
            return sanPhams.Find(x => x.MaSP == maSP);
        }
        public List<SanPham> FindByName(string name)
        {
            if (name.Trim().Count(x => x == ' ') == 0)
            {
                return sanPhams.FindAll(x => Format.SplitLastName(x.TenSP).ToLower() == name.ToLower());
            }
            else
            {
                return sanPhams.FindAll(x => x.TenSP.ToLower() == name.ToLower());
            }
        }
        public void ThemSP()
        {
            SanPham sanPham = new SanPham();

            try
            {
            MaSP:
                sanPham.MaSP = ViewHelper.Input<string>("Nhập Mã Hoa Quả");
                if (Exist(sanPham.MaSP))
                {
                    ViewHelper.PrintError("Mã hoa quả đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaSP;
                }
                else if (sanPham.MaSP == "")
                {
                    ViewHelper.PrintError("Mã hoa quả không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaSP;
                }
            TenSP:
                string tenSP = ViewHelper.Input<string>("Nhập Tên Hoa Quả:");
                if (tenSP == "")
                {
                    ViewHelper.PrintError("Tên hoa quả không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto TenSP;
                }
                else
                {
                    sanPham.TenSP = Format.Name(tenSP);
                }
            GiaNhap:
                sanPham.GiaNhap = ViewHelper.Input<double>($"Nhập Giá Nhập {tenSP}");
                if (sanPham.GiaNhap < 0)
                {
                    ViewHelper.PrintError("Giá nhập không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto GiaNhap;
                }
            GiaBan:
                sanPham.GiaBan = ViewHelper.Input<double>($"Nhập Giá Bán {tenSP}");
                if (sanPham.GiaBan < sanPham.GiaNhap)
                {
                    ViewHelper.PrintError("Giá bán phải lớn hơn giá nhập");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto GiaBan;
                }
                sanPhams.Add(sanPham);
                Save(sanPham);
                ViewHelper.PrintSuccess("Thêm hoa quả thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }

        }
        public void SuaThongTinSP(string maSP)
        {
            var sanPham = FindSP(maSP);
            if (sanPham != null)
            {
                Console.Clear();
                ViewHelper.PrintWarning($"Cập nhật thông tin cho {maSP}-{sanPham.TenSP}");
                ViewHelper.PrintWarning("Bỏ trống nếu không muốn cập nhật");
                try
                {
                    string tenSP = ViewHelper.Input<string>("Nhập Lại Tên Hoa Quả");
                    if (tenSP != "")
                    {
                        sanPham.TenSP = Format.Name(tenSP);
                    }
                GiaNhap:
                    string giaNhap = ViewHelper.Input<string>("Nhập Lại Giá Nhập");
                    if (giaNhap != "")
                    {
                        double temp_gia_nhap;
                        if(double.TryParse(giaNhap,out temp_gia_nhap))
                        {
                            if (temp_gia_nhap > 0)
                            {
                                sanPham.GiaNhap = temp_gia_nhap;
                            }
                            else
                            {
                                ViewHelper.PrintError("Giá nhập không hợp lệ");
                                Thread.Sleep(1000);
                                ViewHelper.ClearPreviousLine(6);
                                goto GiaNhap;
                            }
                        }
                        else
                        {
                            ViewHelper.PrintError("Giá nhập không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
                            goto GiaNhap;
                        }
                    }
                GiaBan:
                    string giaBan = ViewHelper.Input<string>("Nhập Lại Giá Bán");
                    if (giaNhap != "")
                    {
                        double temp_gia_ban;
                        if (double.TryParse(giaNhap, out temp_gia_ban))
                        {
                            if (temp_gia_ban > sanPham.GiaNhap)
                            {
                                sanPham.GiaNhap = temp_gia_ban;
                            }
                            else
                            {
                                ViewHelper.PrintError("Giá bán phải lớn hơn giá nhập");
                                Thread.Sleep(1000);
                                ViewHelper.ClearPreviousLine(6);
                                goto GiaBan;
                            }
                        }
                        else
                        {
                            ViewHelper.PrintError("Giá bán không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(6);
                            goto GiaBan;
                        }
                    }
                    Sync(sanPhams);
                }
                catch (ExitException)
                {
                    return;
                }

            }
            else
            {
                ViewHelper.PrintError($"Không Tồn Tại Hoa Quả Có Mã là {maSP}");
                Thread.Sleep(2000);
                return;
            }
        }
        public void ThemSLSP(string maSP)
        {
            var sanPham = FindSP(maSP);
            if (sanPham != null)
            {
                Console.Clear();
                ViewHelper.PrintWarning($"Thêm số lượng cho {maSP}-{sanPham.TenSP}");
                ViewHelper.PrintWarning("Nhập Exit để quay lại");
                ViewHelper.PrintSuccess($"{sanPham.MaSP}-{sanPham.TenSP} có số lượng là {sanPham.SoLuong}");
                try
                {
                    SoLuong:
                    int soLuong = ViewHelper.Input<int>("Nhập số lượng muốn thêm");
                    if (soLuong > 0)
                    {
                        sanPham.SoLuong += soLuong;
                        GhiLS(sanPham, soLuong);
                        ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        ViewHelper.PrintError("Số lượng không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(6);
                        goto SoLuong;
                    }
                }
                catch (ExitException)
                {
                    return;
                }

            }
            else
            {
                ViewHelper.PrintError($"Không Tồn Tại Hoa Quả Có Mã là {maSP}");
                Thread.Sleep(2000);
                return;
            }
        }
        public bool XoaSP(string maSP)
        {
            var sanPham = sanPhams.Find(x => x.MaSP == maSP);
            if (sanPham != null)
            {
                sanPhams.Remove(sanPham);
                return true;
            }
            return false;
        }
        public List<SanPham> ZeroFilter()
        {
            return sanPhams.FindAll(x => x.SoLuong == 0);
        }
        public List<SanPham> DescendingdFilter()
        {
            return sanPhams.OrderByDescending(x => x.SoLuong).ToList();
        }
        public void ShowSP(SanPham sanPham)
        {
            Console.WriteLine("╔══════════╦════════════════════╦══════════╦════════════════════╦════════════════════╗");
            Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Số lượng",-10}║{"Giá Nhập",-20:c}║{"Giá Bán",-20:c}║");
            Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╣");
            Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
            Console.WriteLine("╚══════════╩════════════════════╩═════════╩════════════════════╩════════════════════╝");
        }
        public void ShowSP(List<SanPham> sanPhams)
        {
            Console.Clear();
            if (sanPhams != null && sanPhams.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════╦════════════════════╦════════════════════╗");
                Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Số lượng",-10}║{"Giá Nhập",-20:c}║{"Giá Bán",-20:c}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╣");
                var last = sanPhams.Last();
                foreach (SanPham sanPham in sanPhams)
                {
                    if (sanPham.Equals(last))
                    {
                        Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════╩════════════════════╩════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy hoa quả hợp lệ");
            }
        }
        public void ShowSP()
        {
            Console.Clear();
            if (sanPhams != null && sanPhams.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════╦════════════════════╦════════════════════╗");
                Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Số lượng",-10}║{"Giá Nhập",-20:c}║{"Giá Bán",-20:c}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╣");
                var last = sanPhams.Last();
                foreach (SanPham sanPham in sanPhams)
                {
                    if (sanPham.Equals(last))
                    {
                        Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0} ║ {sanPham.GiaBan,-20:c0}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════╩════════════════════╩════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy hoa quả hợp lệ");
            }
        }

    }
}
