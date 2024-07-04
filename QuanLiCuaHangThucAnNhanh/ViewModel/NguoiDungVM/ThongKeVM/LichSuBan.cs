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


        //phục vụ xem chi tiết hóa đơn
        private HoaDonBanDTO _selectedItemForChiTietHoaDonBan;
        public HoaDonBanDTO SelectedItemForChiTietHoaDonBan
        {
            get { return _selectedItemForChiTietHoaDonBan; }
            set { _selectedItemForChiTietHoaDonBan = value; OnPropertyChanged(nameof(SelectedItemForChiTietHoaDonBan)); }
        }


        private ObservableCollection<ChiTietHoaDonBanDTO> _chiTietHoaDonBanList;
        public ObservableCollection<ChiTietHoaDonBanDTO> ChiTietHoaDonBanList
        {
            get { return _chiTietHoaDonBanList; }
            set { _chiTietHoaDonBanList = value; OnPropertyChanged(nameof(ChiTietHoaDonBanList)); }
        }
    }
}
