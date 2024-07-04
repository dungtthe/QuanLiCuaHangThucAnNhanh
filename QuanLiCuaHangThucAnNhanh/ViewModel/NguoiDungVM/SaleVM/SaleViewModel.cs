using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


        public ICommand FirstLoadCM { get; set; }



        public SaleViewModel()
        {

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



        }
    }
}
