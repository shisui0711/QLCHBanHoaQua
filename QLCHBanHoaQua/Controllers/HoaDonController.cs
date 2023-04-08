using Framework;
using Newtonsoft.Json.Linq;
using QLCHBanHoaQua.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLCHBanHoaQua.Controllers
{
    class HoaDonController
    {
        private const string HoaDonFile = @"data\hoadon.txt";
        public KhachHangController khachHangController { get; private set; }
        public SanPhamController sanPhamController { get; private set; }
        public NhanVienController nhanVienController { get; private set; }
        public Account Session { get; private set; }
        public static List<HoaDon> hoaDons { get; private set; }
        public HoaDonController(KhachHangController _khachHangController,SanPhamController _sanPhamController,NhanVienController _nhanVienController,Account session)
        {
            this.khachHangController = _khachHangController;
            this.sanPhamController = _sanPhamController;
            this.nhanVienController = _nhanVienController;
            this.Session = session;
            hoaDons = new List<HoaDon>();
            Load(hoaDons);
        }
        private void Load(List<HoaDon> hoaDons)
        {
            using (var fs = new FileStream(HoaDonFile, FileMode.OpenOrCreate, FileAccess.Read))
            {
                var reader = new StreamReader(fs);
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    try
                    {
                        var tmp = line.Trim().Split("|");
                        HoaDon hoaDon = new HoaDon();
                        hoaDon.MaHD = tmp[0];
                        hoaDon.MaNV = tmp[1];
                        hoaDon.TenNV = tmp[2];
                        hoaDon.NgayLap = DateTime.Parse(tmp[3]);
                        hoaDon.MaKH = tmp[4];
                        hoaDon.TenKH = tmp[5];
                        hoaDon.MaSP = tmp[6];
                        hoaDon.TenSP = tmp[7];
                        hoaDon.Soluong = int.Parse(tmp[8]);
                        hoaDon.DonGia = double.Parse(tmp[9]);
                        hoaDon.TongTien = double.Parse(tmp[10]);
                        hoaDons.Add(hoaDon);
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
        private void Save(HoaDon hoaDon)
        {
            File.AppendAllText(HoaDonFile, 
                $"{hoaDon.MaHD}|{hoaDon.MaNV}|{hoaDon.TenNV}|{hoaDon.NgayLap.ToShortDateString()}|{hoaDon.MaKH}|" +
                $"{hoaDon.TenKH}|{hoaDon.MaSP}|{hoaDon.TenSP}|{hoaDon.Soluong}|{hoaDon.DonGia}|{hoaDon.TongTien}\n");
        }
        private void Sync(List<HoaDon> hoaDons)
        {
            File.WriteAllText(HoaDonFile, "");
            foreach (HoaDon hoaDon in hoaDons)
            {
                File.AppendAllText(HoaDonFile,
                    $"{hoaDon.MaHD}|{hoaDon.MaNV}|{hoaDon.TenNV}|{hoaDon.NgayLap.ToShortDateString()}|{hoaDon.MaKH}|" +
                $"{hoaDon.TenKH}|{hoaDon.MaSP}|{hoaDon.TenSP}|{hoaDon.Soluong}|{hoaDon.DonGia}|{hoaDon.TongTien}\n");
            }
        }
        public bool Exist(string maHD)
        {
            
            if (Session.VaiTro == Role.Manager)
            {
                try
                {
                    var temp = hoaDons.Find(x => x.MaHD == maHD);
                    if (temp != null) return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            else if (Session.VaiTro == Role.Staff)
            {
                try
                {
                    var temp = hoaDons.FindAll(y=>y.MaNV == Session.Username).Find(x => x.MaHD == maHD);
                    if (temp != null) return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
            return false;
        }
        public HoaDon FindHD(string maHD)
        {
            if (Session.VaiTro == Role.Manager)
            {
                return hoaDons.Find(x => x.MaHD == maHD);
            }
            else if (Session.VaiTro == Role.Staff)
            {
                return hoaDons.FindAll(x => x.MaNV == Session.Username).Find(y => y.MaHD == maHD);
            }
            else
            {
                return null;
            }
        }
        public List<HoaDon> FindByMaSP(string maSP)
        {
            if(Session.VaiTro == Role.Manager)
            {
                return hoaDons.FindAll(x => x.MaSP == maSP);
            }
            else if (Session.VaiTro == Role.Staff)
            {
                return hoaDons.FindAll(x => x.MaNV == Session.Username).FindAll(y => y.MaSP == maSP);
            }
            else
            {
                return null;
            }
            
        }
        public List<HoaDon> FindByMaKH(string maKH)
        {
            if (Session.VaiTro == Role.Manager)
            {
                return hoaDons.FindAll(x => x.MaKH == maKH);
            }
            else if (Session.VaiTro == Role.Staff)
            {
                return hoaDons.FindAll(x => x.MaNV == Session.Username).FindAll(y => y.MaKH == maKH);
            }
            else
            {
                return null;
            }
        }
        public void GhiHD()
        {
            HoaDon hoaDon = new HoaDon();

            try
            {
            MaHD:
                hoaDon.MaHD = ViewHelper.Input<string>("Nhập Mã Hóa Đơn");
                if (Exist(hoaDon.MaHD))
                {
                    ViewHelper.PrintError("Mã hóa đơn đã tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaHD;
                }
                else if (hoaDon.MaHD == "")
                {
                    ViewHelper.PrintError("Mã hóa đơn không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaHD;
                }
            MaKH:
                hoaDon.MaKH = ViewHelper.Input<string>("Nhập Mã Khách Hàng Mua Hàng:");
                if (!khachHangController.Exist(hoaDon.MaKH))
                {
                    ViewHelper.PrintError("Mã khách hàng không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaKH;
                }
                else if (hoaDon.MaKH == "")
                {
                    ViewHelper.PrintError("Mã khách hàng không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaKH;
                }
            MaSP:
                hoaDon.MaSP = ViewHelper.Input<string>("Nhập Mã Hoa Quả Mà Khách Hàng Mua:");
                if (!sanPhamController.Exist(hoaDon.MaSP))
                {
                    ViewHelper.PrintError("Mã hoa quả không tồn tại");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaSP;
                }
                else if (hoaDon.MaSP == "")
                {
                    ViewHelper.PrintError("Mã hoa quả không được để trống");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto MaSP;
                }
            SoLuong:
                hoaDon.Soluong = ViewHelper.Input<int>("Nhập số lượng mà khách hàng mua:");
                if (hoaDon.Soluong <=0)
                {
                    ViewHelper.PrintError("Số lượng không hợp lệ");
                    Thread.Sleep(1000);
                    ViewHelper.ClearPreviousLine(6);
                    goto SoLuong;
                }
                else if (hoaDon.Soluong > sanPhamController.FindSP(hoaDon.MaSP).SoLuong)
                {
                    ViewHelper.PrintError("Cửa hàng không còn đủ số lượng. Vui lòng quay lại sau.");
                    Thread.Sleep(3000);
                    return;
                }
                hoaDon.TenKH = khachHangController.FindKH(hoaDon.MaKH).TenKH;
                hoaDon.TenSP = sanPhamController.FindSP(hoaDon.MaSP).TenSP;
                hoaDon.NgayLap = DateTime.Now;
                hoaDon.DonGia = sanPhamController.FindSP(hoaDon.MaSP).GiaBan;
                hoaDon.TongTien = hoaDon.DonGia * hoaDon.Soluong;
                if (Session.VaiTro == Role.Staff)
                {
                    hoaDon.MaNV = Session.Username;
                    hoaDon.TenNV = nhanVienController.FindNV(Session.Username).TenNV;
                }
                else
                {
                    hoaDon.MaNV = "Quản lý";
                    hoaDon.TenNV = "Quản lý";
                }
                sanPhamController.FindSP(hoaDon.MaSP).SoLuong -= hoaDon.Soluong;
                hoaDons.Add(hoaDon);
                Save(hoaDon);
                ViewHelper.PrintSuccess($"Số tiền cần phải trả là {hoaDon.TongTien}");
                ViewHelper.PrintSuccess("Ghi hóa đơn thành công");
                ViewHelper.PrintWarning("Bấm phím bất kỳ để trờ về menu");
                Console.ReadKey(true);
            }
            catch (ExitException)
            {
                return;
            }

        }
        public bool XoaHD(string maHD)
        {
            var hoaDon = FindHD(maHD);
            if(hoaDon != null)
            {
                var sanPham = sanPhamController.FindSP(hoaDon.MaSP);
                sanPham.SoLuong += hoaDon.Soluong;
                hoaDons.Remove(hoaDon);
                Sync(hoaDons);
                return true;
            }
            return false;
        }
        public void ShowHD(HoaDon hoaDon)
        {
            Console.WriteLine("╔══════════╦════════════════════╦══════════╦════════════════════╦════════════════════╦══════════╦══════════╦══════════╗");
            Console.WriteLine($"║{"Mã HD",-10}║{"Người lập",-20}║{"Ngày lập",-10}║{"Khách hàng",-20}║{"Sản phẩm",-20}║{"Số lượng",-10}║{"Đơn giá",-10}║{"Thành Tiền",-10}║");
            Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV+"-"+hoaDon.TenNV,-20}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                $"{hoaDon.MaKH+"-"+hoaDon.TenKH,-20}║{hoaDon.MaSP+"-"+hoaDon.TenSP,-20}║{hoaDon.Soluong,-5}║{hoaDon.DonGia,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
            Console.WriteLine("╚══════════╩════════════════════╩══════════╩════════════════════╩════════════════════╩══════════╩══════════╩══════════╝");
        }
        public void ShowHD(List<HoaDon> hoaDons)
        {
            Console.Clear();
            if (hoaDons != null && hoaDons.Count > 0)
            {
                Console.WriteLine("╔══════════╦════════════════════╦══════════╦════════════════════╦════════════════════╦══════════╦══════════╦══════════╗");
                Console.WriteLine($"║{"Mã HD",-10}║{"Người lập",-20}║{"Ngày lập",-10}║{"Khách hàng",-20}║{"Sản phẩm",-20}║{"Số lượng",-10}║{"Đơn giá",-10}║{"Thành Tiền",-10}║");
                Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                var last = hoaDons.Last();
                foreach (HoaDon hoaDon in hoaDons)
                {
                    if (hoaDon.Equals(last))
                    {
                        Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + hoaDon.TenNV,-20}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                          $"{hoaDon.MaKH + "-" + hoaDon.TenKH,-20}║{hoaDon.MaSP + "-" + hoaDon.TenSP,-20}║" +
                                          $"{hoaDon.Soluong,-10}║{hoaDon.DonGia,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                        Console.WriteLine("╚══════════╩════════════════════╩══════════╩════════════════════╩════════════════════╩══════════╩══════════╩══════════╝");
                    }
                    else
                    {
                        Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + hoaDon.TenNV,-20}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                        $"{hoaDon.MaKH + "-" + hoaDon.TenKH,-20}║{hoaDon.MaSP + "-" + hoaDon.TenSP,-20}║" +
                                        $"{hoaDon.Soluong,-10}║{hoaDon.DonGia,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                        Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                    }
                }
            }
            else
            {
                ViewHelper.PrintError("Không tìm thấy khách hàng nào hợp lệ");
            }
        }
        public void ShowHD()
        {
            if(Session.VaiTro == Role.Manager)
            {
                Console.Clear();
                if (hoaDons != null && hoaDons.Count > 0)
                {
                    Console.WriteLine("╔══════════╦════════════════════╦══════════╦════════════════════╦════════════════════╦══════════╦══════════╦══════════╗");
                    Console.WriteLine($"║{"Mã HD",-10}║{"Người lập",-20}║{"Ngày lập",-10}║{"Khách hàng",-20}║{"Sản phẩm",-20}║{"Số lượng",-10}║{"Đơn giá",-10}║{"Thành Tiền",-10}║");
                    Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                    var last = hoaDons.Last();
                    foreach (HoaDon hoaDon in hoaDons)
                    {
                        if (hoaDon.Equals(last))
                        {
                            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + hoaDon.TenNV,-20}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                              $"{hoaDon.MaKH + "-" + hoaDon.TenKH,-20}║{hoaDon.MaSP + "-" + hoaDon.TenSP,-20}║" +
                                              $"{hoaDon.Soluong,-10}║{hoaDon.DonGia,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                            Console.WriteLine("╚══════════╩════════════════════╩══════════╩════════════════════╩════════════════════╩══════════╩══════════╩══════════╝");
                        }
                        else
                        {
                            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + hoaDon.TenNV,-20}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                            $"{hoaDon.MaKH + "-" + hoaDon.TenKH,-20}║{hoaDon.MaSP + "-" + hoaDon.TenSP,-20}║" +
                                            $"{hoaDon.Soluong,-10}║{hoaDon.DonGia,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                            Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                        }
                    }
                }
                else
                {
                    ViewHelper.PrintError("Không tìm thấy khách hàng nào hợp lệ");
                }
            }
            else if (Session.VaiTro == Role.Staff)
            {
                var hoaDons_2 = hoaDons.FindAll(x=>x.MaNV ==Session.Username);
                Console.Clear();
                if (hoaDons_2 != null && hoaDons_2.Count > 0)
                {
                    Console.WriteLine("╔══════════╦════════════════════╦══════════╦════════════════════╦════════════════════╦══════════╦══════════╦══════════╗");
                    Console.WriteLine($"║{"Mã HD",-10}║{"Người lập",-20}║{"Ngày lập",-10}║{"Khách hàng",-20}║{"Sản phẩm",-20}║{"Số lượng",-10}║{"Đơn giá",-10}║{"Thành Tiền",-10}║");
                    Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                    var last = hoaDons_2.Last();
                    foreach (HoaDon hoaDon in hoaDons_2)
                    {
                        if (hoaDon.Equals(last))
                        {
                            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + hoaDon.TenNV,-20}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                              $"{hoaDon.MaKH + "-" + hoaDon.TenKH,-20}║{hoaDon.MaSP + "-" + hoaDon.TenSP,-20}║" +
                                              $"{hoaDon.Soluong,-10}║{hoaDon.DonGia,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                            Console.WriteLine("╚══════════╩════════════════════╩══════════╩════════════════════╩════════════════════╩══════════╩══════════╩══════════╝");
                        }
                        else
                        {
                            Console.WriteLine($"║{hoaDon.MaHD,-10}║{hoaDon.MaNV + "-" + hoaDon.TenNV,-20}║{hoaDon.NgayLap.ToShortDateString(),-10}║" +
                                            $"{hoaDon.MaKH + "-" + hoaDon.TenKH,-20}║{hoaDon.MaSP + "-" + hoaDon.TenSP,-20}║" +
                                            $"{hoaDon.Soluong,-10}║{hoaDon.DonGia,-10:c0}║{Math.Round(hoaDon.TongTien),-10:c0}║");
                            Console.WriteLine("╠══════════╬════════════════════╬══════════╬════════════════════╬════════════════════╬══════════╬══════════╬══════════╣");
                        }
                    }
                }
                else
                {
                    ViewHelper.PrintError("Không tìm thấy khách hàng nào hợp lệ");
                }
            }
            
        }

    }
}
