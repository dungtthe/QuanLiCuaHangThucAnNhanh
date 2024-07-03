using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.BanHang;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.CustomerMangament;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.ThongKe;
using QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.SaleVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM
{
    public class MainNguoiDungVM : BaseViewModel
    {
        public static NguoiDungDTO nguoiDungDTOCur;

        public ICommand FirstLoadCM { get; set; }
        public ICommand LoadNhanVienPage { get; }
        public ICommand LoadKhachHangPage { get; set; }
        public ICommand LoadSanPhamPage { get; set; }
        public ICommand LoadThongKePage { get; set; }
        public ICommand LoadHeThongPage { get; set; }
        public ICommand LoadBanHangPage { get; set; }

        public MainNguoiDungVM()
        {
            LoadThongKePage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ThongKeMainPage();
            });

            LoadBanHangPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new SalePage();
            });
            LoadKhachHangPage = new RelayCommand<Frame>((p) => { return true; }, (p) => { p.Content = new CustomerManagementView(); });

        }
    }
}
