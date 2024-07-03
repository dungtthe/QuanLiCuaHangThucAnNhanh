using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.ThongKeVM
{
    public partial class ThongKeViewModel
    {
        private List<HoaDonBanDTO> danhSachHoaDon;
        private ObservableCollection<HoaDonBanDTO> _danhSachHoaDon;
        public ObservableCollection<HoaDonBanDTO> DanhSachHoaDon
        {
            get { return _danhSachHoaDon; }
            set { _danhSachHoaDon = value; OnPropertyChanged(nameof(DanhSachHoaDon)); }
        }
    }
}
