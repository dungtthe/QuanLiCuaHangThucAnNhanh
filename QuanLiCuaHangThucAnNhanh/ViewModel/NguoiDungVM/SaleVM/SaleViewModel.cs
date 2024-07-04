using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.SaleVM
{
    public class SaleViewModel : BaseViewModel
    {
        public static List<SanPhamDTO> prdList;
        private ObservableCollection<SanPhamDTO> _productList;

        public ObservableCollection<SanPhamDTO> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(nameof(ProductList)); }
        }
        private ObservableCollection<string> combogenrelist;
        public ObservableCollection<string> ComboList
        {
            get { return combogenrelist; }
            set { combogenrelist = value; OnPropertyChanged(nameof(ComboList)); }
        }



        private KhachHangDTO khachHangForBill;
        public KhachHangDTO KhachHangForBill
        {
            get => khachHangForBill;
            set
            {
                khachHangForBill = value;
                OnPropertyChanged(nameof(KhachHangForBill));
            }
        }


        private KhachHangDTO khachVangLai;

        //text tìm kiếm
        private string _cusInfo;
        public string CusInfo
        {
            get { return _cusInfo; }
            set { _cusInfo = value; OnPropertyChanged(); }
        }


        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchCusCM { get; set; }



        public SaleViewModel()
        {
            KhachHangForBill = new KhachHangDTO();
            KhachHangForBill.HoTen = "Khách vãng lai";
            khachVangLai = new KhachHangDTO();
            khachVangLai.HoTen= "Khách vãng lai";

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                ProductList = new ObservableCollection<SanPhamDTO>(await SanPhamDA.gI().GetAllSanPham());
                if (ProductList != null)
                {
                    prdList = new List<SanPhamDTO>(ProductList);
                }


                ComboList = new ObservableCollection<string>(await DanhMucSanPhamDA.gI().GetAllTenDanhMucSanPham());
                ComboList.Insert(0, "Tất cả thể loại");

            });


            SearchCusCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                KhachHangDTO temp = null;


                temp = await KhachHangDA.gI().FindNguoiDungBySoDienThoai(CusInfo);

                if (temp == null)
                {
                    temp = await KhachHangDA.gI().FindNguoiDungByEmail(CusInfo);
                }

                if (temp == null)
                {
                    Error wd1 = new Error("Khách hàng không tồn tại");
                    temp = khachVangLai;
                    wd1.ShowDialog();

                }
                KhachHangForBill = temp;

               
            });
        }
    }
}
