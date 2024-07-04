using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.QuanLi.HeThongVM
{
    public partial class HeThongVM : BaseViewModel
    {
        public ICommand FirstLoadCMD { get; set; }


        public HeThongVM()
        {
            FirstLoadCMD = new RelayCommand<object>(null, async (p) =>
            {
                ThamSoDTO = await ThamSoDA.gI().GetThamSoCur();
                if (ThamSoDTO == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }

                ComboList = new ObservableCollection<DanhMucSanPhamDTO>(await DanhMucSanPhamDA.gI().GetAllDanhMucSanPham());

            });
        }

    }
}
