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

        //phục vụ xem chi tiết hóa đơn
        private HoaDonNhapDTO _selectedItemForChiTietHoaDonNhap;
        public HoaDonNhapDTO SelectedItemForChiTietHoaDonNhap
        {
            get { return _selectedItemForChiTietHoaDonNhap; }
            set { _selectedItemForChiTietHoaDonNhap = value; OnPropertyChanged(nameof(SelectedItemForChiTietHoaDonNhap)); }
        }


        private ObservableCollection<ChiTietHoaDonNhapDTO> _chiTietHoaDonNhapList;
        public ObservableCollection<ChiTietHoaDonNhapDTO> ChiTietHoaDonNhapList
        {
            get { return _chiTietHoaDonNhapList; }
            set { _chiTietHoaDonNhapList = value; OnPropertyChanged(nameof(ChiTietHoaDonNhapList)); }
        }
    }
}
