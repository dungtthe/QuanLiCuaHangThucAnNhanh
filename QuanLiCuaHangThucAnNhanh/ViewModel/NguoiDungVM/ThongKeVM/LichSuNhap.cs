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
        private List<HoaDonNhapDTO> danhSachNhap;
        private ObservableCollection<HoaDonNhapDTO> _danhSachNhap;
        public ObservableCollection<HoaDonNhapDTO> DanhSachNhap
        {
            get { return _danhSachNhap; }
            set { _danhSachNhap = value; OnPropertyChanged(nameof(DanhSachNhap)); }
        }
    }
}
