using Framework;
using QLCHBanHoaQua.Models;
using System.Globalization;
using System.Text.RegularExpressions;

namespace QLCHBanHoaQua.Controllers
{
    class SanPhamController
    {
        private const string SanPhamFile = @"data\sanpham.txt";
        private const string LichSuFile = @"data\lichsunhap.txt";
        public static List<LichSuNhap> lichSuNhaps { get; private set; }
        public static List<SanPham> sanPhams { get; private set; }
        public NCCController nCCController { get; }
        public SanPhamController(NCCController _nCCController)
        {
            this.nCCController = _nCCController;
            sanPhams = new List<SanPham>();
            lichSuNhaps = new List<LichSuNhap>();
            Load();
            LoadLS();
        }
        private void LoadLS()
        {
            using (var fs = new FileStream(LichSuFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var tmp = line.Trim().Split("|");
                        LichSuNhap lichSuNhap = new LichSuNhap();
                        lichSuNhap.MaSP = tmp[0];
                        lichSuNhap.SoLuong = int.Parse(tmp[1]);
                        lichSuNhap.ThoiGian = DateTime.Parse(tmp[2]);
                        
                        lichSuNhaps.Add(lichSuNhap);
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
        private void GhiLS(SanPham sanPham, int soLuong)
        {
            File.AppendAllText(LichSuFile,
                $"{sanPham.MaSP}|{sanPham.TenSP}|{soLuong}|{sanPham.GiaNhap}|" +
                $"{DateTime.Now}\n");
        }
        private void SyncLS()
        {
            File.WriteAllText(LichSuFile, "");
            foreach (LichSuNhap lichSuNhap in lichSuNhaps)
            {
                File.AppendAllText(LichSuFile,
                $"{lichSuNhap.MaSP}|{FindSP(lichSuNhap.MaSP).TenSP}|{FindSP(lichSuNhap.MaSP).SoLuong}|" +
                $"{FindSP(lichSuNhap.MaSP).GiaNhap}|{lichSuNhap.ThoiGian}\n");
            }
        }
        private void Load()
        {
            sanPhams.Clear();
            bool flag = false;
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
                        sanPham.MaNCC = tmp[2];
                        sanPham.SoLuong = int.Parse(tmp[3]);
                        sanPham.GiaNhap = double.Parse(tmp[4]);
                        sanPham.GiaBan = double.Parse(tmp[5]);
                        if (!NCCController.DanhSachNCC.Any(x => x.MaNCC == sanPham.MaNCC))
                        {
                            flag = true;
                            continue;
                        }
                        sanPhams.Add(sanPham);
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
            if(flag) Sync();
        }
        private void Save(SanPham sanPham)
        {
            File.AppendAllText(SanPhamFile,
                $"{sanPham.MaSP}|{sanPham.TenSP}|{sanPham.MaNCC}|{sanPham.SoLuong}|" +
                $"{sanPham.GiaNhap}|{sanPham.GiaBan}\n");
        }
        public void Sync()
        {
            File.WriteAllText(SanPhamFile, "");
            foreach (SanPham sanPham in sanPhams)
            {
                File.AppendAllText(SanPhamFile,
                $"{sanPham.MaSP}|{sanPham.TenSP}|{sanPham.MaNCC}|{sanPham.SoLuong}|" +
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
            Load();
            return sanPhams.Find(x => x.MaSP == maSP);
        }
        public List<SanPham> FindByName(string name)
        {
            Load();
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
            Console.Clear();
            ViewHelper.PrintWarning("Thêm Loại Hoa Quả");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            SanPham sanPham = new SanPham();

            try
            {
                var pos_masp = ViewHelper.PrintInput("Nhập Mã Hoa Quả");
                var pos_ten = ViewHelper.PrintInput("Nhập Tên Hoa Quả");
                var pos_mancc = ViewHelper.PrintInput("Nhập Mã Nhà Cung Cấp");
                var pos_gianhap = ViewHelper.PrintInput("Nhập Giá Nhập");
                var pos_giaban = ViewHelper.PrintInput("Nhập Giá Bán");
                var pos_end = Console.GetCursorPosition();
            MaSP:
                Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                sanPham.MaSP = ViewHelper.ReadLine();
                if (Exist(sanPham.MaSP))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hoa quả đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", sanPham.MaSP.Length)));
                    goto MaSP;
                }
                else if (sanPham.MaSP == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hoa quả không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", sanPham.MaSP.Length)));
                    goto MaSP;
                }
            TenSP:
                Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                string tenSP = ViewHelper.ReadLine();
                if (tenSP == "")
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Tên hoa quả không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", tenSP.Length)));
                    goto TenSP;
                }
                else
                {
                    sanPham.TenSP = Format.Name(tenSP);
                }
            MaNCC:
                Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                sanPham.MaNCC = ViewHelper.ReadLine();
                if (!nCCController.Exist(sanPham.MaNCC))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã nhà cung cấp không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", sanPham.MaNCC.Length)));
                    goto MaNCC;
                }
            GiaNhap:
                Console.SetCursorPosition(pos_gianhap.Left, pos_gianhap.Top);
                var giaNhap = ViewHelper.ReadLine();
                try
                {
                    sanPham.GiaNhap = double.Parse(giaNhap, CultureInfo.CurrentCulture);
                    if (sanPham.GiaNhap < 0)
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Giá nhập không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_gianhap.Left, pos_gianhap.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", giaNhap.Length)));
                        goto GiaNhap;
                    }
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Giá nhập không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_gianhap.Left, pos_gianhap.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", giaNhap.Length)));
                    goto GiaNhap;
                }

            GiaBan:
                Console.SetCursorPosition(pos_giaban.Left, pos_giaban.Top);
                var giaBan = ViewHelper.ReadLine();
                try
                {
                    sanPham.GiaBan = double.Parse(giaBan, CultureInfo.CurrentCulture);
                    if (sanPham.GiaBan < sanPham.GiaNhap)
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Giá bán phải lớn hơn giá nhập");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_giaban.Left, pos_giaban.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", giaBan.Length)));
                        goto GiaBan;
                    }
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Giá bán không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_giaban.Left, pos_giaban.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", giaBan.Length)));
                    goto GiaBan;
                }

                sanPhams.Add(sanPham);
                Save(sanPham);
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess("Thêm hoa quả thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }

        }
        public void SuaThongTinSP()
        {
            Console.Clear();
            ViewHelper.PrintWarning("Cập nhật thông tin hoa quả");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            ViewHelper.PrintWarning("Bỏ trống nếu không muốn cập nhật");
            try
            {
                var pos_masp = ViewHelper.PrintInput("Nhập Mã Hoa Quả");
                var pos_ten = ViewHelper.PrintInput("Nhập Tên Hoa Quả");
                var pos_mancc = ViewHelper.PrintInput("Nhập Mã Nhà Cung Cấp");
                var pos_gianhap = ViewHelper.PrintInput("Nhập Giá Nhập");
                var pos_giaban = ViewHelper.PrintInput("Nhập Giá Bán");
                var pos_end = Console.GetCursorPosition();
            MaSP:
                Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                string maSP = ViewHelper.ReadLine();
                if (!Exist(maSP))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hoa quả không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maSP.Length)));
                    goto MaSP;
                }
                var sanPham = FindSP(maSP);
                Console.SetCursorPosition(pos_ten.Left, pos_ten.Top);
                string tenSP = ViewHelper.ReadLine();
                if (tenSP != "")
                {
                    sanPham.TenSP = Format.Name(tenSP);
                }
            MaNCC:
                Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                string maNCC = ViewHelper.ReadLine();
                if (maNCC != "")
                {
                    if (nCCController.Exist(maNCC))
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Mã nhà cung cấp không tồn tại");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_mancc.Left, pos_mancc.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", maNCC.Length)));
                        goto MaNCC;
                    }
                    else
                    {
                        sanPham.MaNCC = maNCC;
                    }
                }
            GiaNhap:
                Console.SetCursorPosition(pos_gianhap.Left, pos_gianhap.Top);
                string giaNhap = ViewHelper.ReadLine();
                if (giaNhap != "")
                {
                    try
                    {
                        sanPham.GiaNhap = double.Parse(giaNhap, CultureInfo.CurrentCulture);
                        if (sanPham.GiaNhap < 0)
                        {
                            Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                            ViewHelper.PrintError("Giá nhập không hợp lệ");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(3);
                            Console.SetCursorPosition(pos_gianhap.Left, pos_gianhap.Top);
                            Console.Write(string.Concat(Enumerable.Repeat(" ", giaNhap.Length)));
                            goto GiaNhap;
                        }
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Giá nhập không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_gianhap.Left, pos_gianhap.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", giaNhap.Length)));
                        goto GiaNhap;
                    }
                }
            GiaBan:
                Console.SetCursorPosition(pos_giaban.Left, pos_giaban.Top);
                string giaBan = ViewHelper.ReadLine();
                if (giaNhap != "")
                {
                    try
                    {
                        sanPham.GiaBan = double.Parse(giaBan, CultureInfo.CurrentCulture);
                        if (sanPham.GiaBan < sanPham.GiaNhap)
                        {
                            Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                            ViewHelper.PrintError("Giá bán phải lớn hơn giá nhập");
                            Thread.Sleep(1000);
                            ViewHelper.ClearPreviousLine(3);
                            Console.SetCursorPosition(pos_giaban.Left, pos_giaban.Top);
                            Console.Write(string.Concat(Enumerable.Repeat(" ", giaBan.Length)));
                            goto GiaBan;
                        }
                    }
                    catch (FormatException)
                    {
                        Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                        ViewHelper.PrintError("Giá bán không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_giaban.Left, pos_giaban.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", giaBan.Length)));
                        goto GiaBan;
                    }
                }
                Sync();
                SyncLS();
                Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                ViewHelper.PrintSuccess($"Cập nhật thông tin thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }

        }
        public void ThemSLSP()
        {


            Console.Clear();
            ViewHelper.PrintWarning("Cập nhật số lượng hoa quả");
            ViewHelper.PrintWarning("Bấm ESC để quay lại");
            try
            {
                var pos_masp = ViewHelper.PrintInput("Nhập Mã Hoa Quả");
                var pos_sl = ViewHelper.PrintInput("Nhập số lượng muốn thêm");
                var pos_end1 = Console.GetCursorPosition();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                var pos_end2 = Console.GetCursorPosition();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                var pos_end3 = Console.GetCursorPosition();
            MaSP:
                Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                string maSP = ViewHelper.ReadLine();
                if (!Exist(maSP))
                {
                    Console.SetCursorPosition(pos_end3.Left, pos_end3.Top);
                    ViewHelper.PrintError("Mã hoa quả không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maSP.Length)));
                    goto MaSP;
                }
                var sanPham = FindSP(maSP);
                Console.SetCursorPosition(pos_end1.Left, pos_end1.Top);
                ShowSP(sanPham);
            SoLuong:
                Console.SetCursorPosition(pos_sl.Left, pos_sl.Top);
                string soLuong = ViewHelper.ReadLine();
                try
                {
                    if (int.Parse(soLuong) > 0)
                    {
                        sanPham.SoLuong += int.Parse(soLuong);
                        Sync();
                        GhiLS(sanPham, int.Parse(soLuong));
                        Console.SetCursorPosition(pos_end2.Left, pos_end2.Top);
                        ShowSP(sanPham);
                        Console.SetCursorPosition(pos_end3.Left, pos_end3.Top);
                        ViewHelper.PrintSuccess($"Thêm thành công {soLuong} quả {FindSP(maSP).TenSP}");
                        ViewHelper.PrintWarning("Bấm phím bất kỳ để trở về menu");
                        Console.ReadKey(true);
                    }
                    else
                    {
                        Console.SetCursorPosition(pos_end3.Left, pos_end3.Top);
                        ViewHelper.PrintError("Số lượng không hợp lệ");
                        Thread.Sleep(1000);
                        ViewHelper.ClearPreviousLine(3);
                        Console.SetCursorPosition(pos_sl.Left, pos_sl.Top);
                        Console.Write(string.Concat(Enumerable.Repeat(" ", soLuong.Length)));
                        goto SoLuong;
                    }
                }
                catch (FormatException)
                {
                    Console.SetCursorPosition(pos_end3.Left, pos_end3.Top);
                    ViewHelper.PrintError("Số lượng không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_sl.Left, pos_sl.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", soLuong.Length)));
                    goto SoLuong;
                }
                
            }
            catch (ExitException)
            {
                return;
            }

        }
        public void XoaSP()
        {
            try
            {
                Console.Clear();
                ViewHelper.PrintWarning("Xóa hoa quả");
                ViewHelper.PrintWarning("Bấm ESC để quay lại");
                var pos_masp = ViewHelper.PrintInput("Nhập Mã Hoa Quả:");
                var pos_end = Console.GetCursorPosition();

            MaSP:
                Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                string maSP = ViewHelper.ReadLine();
                if (!Exist(maSP))
                {
                    Console.SetCursorPosition(pos_end.Left, pos_end.Top);
                    ViewHelper.PrintError("Mã hoa quả không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(3);
                    Console.SetCursorPosition(pos_masp.Left, pos_masp.Top);
                    Console.Write(string.Concat(Enumerable.Repeat(" ", maSP.Length)));
                    goto MaSP;
                }
                var sanPham = sanPhams.Find(x => x.MaSP == maSP);
                Console.Clear();
                ShowSP(sanPham);
                ViewHelper.PrintWarning("Bạn có chắc chắn muốn xóa hoa quả trên.");
                ViewHelper.PrintWarning("Bấm Y để xác nhận. Bấm N để hủy.");
            XacNhan:
                var xacNhan = Console.ReadKey(true);
                if (xacNhan.Key == ConsoleKey.Y)
                {
                    sanPhams.Remove(sanPham);
                    Sync();
                    lichSuNhaps.RemoveAll(x => x.MaSP == sanPham.MaSP);
                    SyncLS();
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
            Console.WriteLine("╔══════════╦════════════════════╦══════════════════════════════╦══════════╦════════════════════╦════════════════════╗");
            Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Nhà Cung Cấp",-30}║{"Số lượng",-10}║{"Giá Nhập",-20:c}║{"Giá Bán",-20:c}║");
            Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬══════════╬════════════════════╬════════════════════╣");
            Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.MaNCC+"-"+nCCController.FindNCC(sanPham.MaNCC).TenNCC,-30}" +
                $"║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
            Console.WriteLine("╚══════════╩════════════════════╩══════════════════════════════╩══════════╩════════════════════╩════════════════════╝");
        }
        public void ShowSP(List<SanPham> sanPhams)
        {
            Console.Clear();
            if (sanPhams != null && sanPhams.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════════════════════════╦══════════╦════════════════════╦════════════════════╗");
                Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Nhà Cung Cấp",-30}║{"Số lượng",-10}║{"Giá Nhập",-20:c}║{"Giá Bán",-20:c}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬══════════╬════════════════════╬════════════════════╣");
                var last = sanPhams.Last();
                foreach (SanPham sanPham in sanPhams)
                {
                    if (sanPham.Equals(last))
                    {
                        Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.MaNCC + "-" + nCCController.FindNCC(sanPham.MaNCC).TenNCC,-30}" +
                         $"║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════════════════════════╩══════════╩════════════════════╩════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.MaNCC + "-" + nCCController.FindNCC(sanPham.MaNCC).TenNCC,-30}" +
                         $"║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬══════════╬════════════════════╬════════════════════╣");
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
            Load();
            Console.Clear();
            if (sanPhams != null && sanPhams.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════════════════════════╦══════════╦════════════════════╦════════════════════╗");
                Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Nhà Cung Cấp",-30}║{"Số lượng",-10}║{"Giá Nhập",-20:c}║{"Giá Bán",-20:c}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬══════════╬════════════════════╬════════════════════╣");
                var last = sanPhams.Last();
                foreach (SanPham sanPham in sanPhams)
                {
                    if (sanPham.Equals(last))
                    {
                        Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.MaNCC + "-" + nCCController.FindNCC(sanPham.MaNCC).TenNCC,-30}" +
                         $"║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════════════════════════╩══════════╩════════════════════╩════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{sanPham.MaSP,-10}║{sanPham.TenSP,-20}║{sanPham.MaNCC + "-" + nCCController.FindNCC(sanPham.MaNCC).TenNCC,-30}" +
                         $"║{sanPham.SoLuong,-10}║{sanPham.GiaNhap,-20:c0}║{sanPham.GiaBan,-20:c0}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════════════════════════╬══════════╬════════════════════╬════════════════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy hoa quả hợp lệ");
            }
        }
        public void ShowLS()
        {
            if (lichSuNhaps.Count > 0)
            {
                lichSuNhaps = lichSuNhaps.OrderByDescending(x => (int)x.ThoiGian.Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToList();
                Console.WriteLine("╔══════════╦════════════════════╦══════════╦═════════════════════════╗");
                Console.WriteLine($"║{"Mã Hoa Quả",-10}║{"Tên Hoa Quả",-20}║{"Số lượng",-10}║{"Thời Gian",-25}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════╬═════════════════════════╣");
                var last = lichSuNhaps.Last();
                foreach (var lichsu in lichSuNhaps)
                {
                    if(lichsu.Equals(last))
                    {
                        Console.WriteLine($"║{lichsu.MaSP,-10}║{FindSP(lichsu.MaSP).TenSP,-20}║{lichsu.SoLuong,-10}║{lichsu.ThoiGian,-25}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════╩═════════════════════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{lichsu.MaSP,-10}║{FindSP(lichsu.MaSP).TenSP,-20}║{lichsu.SoLuong,-10}║{lichsu.ThoiGian,-25}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════╬═════════════════════════╣");
                    }
                    
                }
            }
            else
            {
                ViewHelper.PrintError("Lịch sử nhập trống");
            }
            
        }

    }
}
