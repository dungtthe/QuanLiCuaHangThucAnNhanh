using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.DTO
{
    public class HoaDonBanDTO : INotifyPropertyChanged
    {
        private int id;
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }

        private Nullable<System.DateTime> ngayTao;
        public Nullable<System.DateTime> NgayTao
        {
            get => ngayTao;
            set
            {
                ngayTao = value;
                OnPropertyChanged(nameof(NgayTao));
            }
        }

        private decimal tongTienBan;
        public decimal TongTienBan
        {
            get => tongTienBan;
            set
            {
                tongTienBan = value;
                OnPropertyChanged(nameof(TongTienBan));
            }
        }

        private int nguoiDungID;
        public int NguoiDungID
        {
            get => nguoiDungID;
            set
            {
                nguoiDungID = value;
                OnPropertyChanged(nameof(NguoiDungID));
            }
        }

        private int khachHangID;
        public int KhachHangID
        {
            get => khachHangID;
            set
            {
                khachHangID = value;
                OnPropertyChanged(nameof(KhachHangID));
            }
        }

        private Nullable<bool> isDeleted;
        public Nullable<bool> IsDeleted
        {
            get => isDeleted;
            set
            {
                isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }

        private NguoiDungDTO nguoiDungDTO;
        public NguoiDungDTO NguoiDungDTO
        {
            get => nguoiDungDTO;
            set
            {
                nguoiDungDTO = value;
                OnPropertyChanged(nameof(NguoiDungDTO));
            }
        }

        private KhachHangDTO khachHangDTO;
        public KhachHangDTO KhachHangDTO
        {
            get => khachHangDTO;
            set
            {
                khachHangDTO = value;
                OnPropertyChanged(nameof(KhachHangDTO));
            }
        }

        private ObservableCollection<ChiTietHoaDonBanDTO> listChiTietHoaDonBanDTO;
        public ObservableCollection<ChiTietHoaDonBanDTO> ListChiTietHoaDonBanDTO
        {
            get => listChiTietHoaDonBanDTO;
            set
            {
                listChiTietHoaDonBanDTO = value;
                OnPropertyChanged(nameof(ListChiTietHoaDonBanDTO));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
