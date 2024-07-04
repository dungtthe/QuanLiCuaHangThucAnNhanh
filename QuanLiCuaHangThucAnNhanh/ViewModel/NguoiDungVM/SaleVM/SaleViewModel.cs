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

        //danh sách sản phẩm 
        private List<SanPhamDTO> prdList;
        private ObservableCollection<SanPhamDTO> _productList;

        public ObservableCollection<SanPhamDTO> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(nameof(ProductList)); }
        }


        //danh sách tên danh mục sản phẩm
        private ObservableCollection<string> combogenrelist;
        public ObservableCollection<string> ComboList
        {
            get { return combogenrelist; }
            set { combogenrelist = value; OnPropertyChanged(nameof(ComboList)); }
        }



        //khách hàng cho bill

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



        //text tìm kiếm khách hàng
        private string _cusInfo;
        public string CusInfo
        {
            get { return _cusInfo; }
            set { _cusInfo = value; OnPropertyChanged(); }
        }



        //chọn item bên danh sách sản phẩm
        private SanPhamDTO _selectedItemSanPhamDTO;

        public SanPhamDTO SelectedItemSanPhamDTO
        {
            get { return _selectedItemSanPhamDTO; }
            set { _selectedItemSanPhamDTO = value; OnPropertyChanged(nameof(SelectedItemSanPhamDTO)); }
        }

        //chọn item bên đơn hàng
        private ChiTietHoaDonBanDTO _selectedItemChiTietHoaDonBan;
        public ChiTietHoaDonBanDTO SelectedItemChiTietHoaDonBan
        {
            get => _selectedItemChiTietHoaDonBan;
            set
            {
                _selectedItemChiTietHoaDonBan = value;
                OnPropertyChanged(nameof(SelectedItemChiTietHoaDonBan));
            }
        }

        //danh sách sản phẩm trong đơn hàng
        private ObservableCollection<ChiTietHoaDonBanDTO> listChiTietHoaDonBan;
        public ObservableCollection<ChiTietHoaDonBanDTO> ListChiTietHoaDonBan
        {
            get => listChiTietHoaDonBan;
            set
            {
                listChiTietHoaDonBan = value;
                OnPropertyChanged(nameof(ListChiTietHoaDonBan));
            }
        }



        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchCusCM { get; set; }

        //chọn item cho sản phẩm 
        public ICommand SelectSanPhamDTOCM { get; set; }


        public SaleViewModel()
        {
            khachVangLai = new KhachHangDTO(1,"Khách vãng lai");
            KhachHangForBill = khachVangLai;

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

                if (string.IsNullOrEmpty(CusInfo))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error,"Vui lòng nhập số điện thoại hoặc Email!");
                    return;
                }

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
                    CusInfo = "";
                    wd1.ShowDialog();

                }
                KhachHangForBill = temp;
            });



            SelectSanPhamDTOCM = new RelayCommand<SanPhamDTO>((p) => { return true; }, (p) =>
            {
                if (SelectedItemSanPhamDTO != null)
                {
                    MessageBox.Show(SelectedItemSanPhamDTO.TenSP);
                }
            });
        }
    }
}
