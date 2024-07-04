using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.QuanLi.HeThongVM
{
    public partial class HeThongVM
    {
        private ObservableCollection<DanhMucSanPhamDTO> combogenrelist;
        public ObservableCollection<DanhMucSanPhamDTO> ComboList
        {
            get { return combogenrelist; }
            set { combogenrelist = value; OnPropertyChanged(nameof(ComboList)); }
        }

        private DanhMucSanPhamDTO selectedItem;
        public DanhMucSanPhamDTO SelectedItem
        {
            get => selectedItem;
            set
            {
                SelectedItem = value; OnPropertyChanged(nameof(SelectedItem));
            }
        }

    }
}
