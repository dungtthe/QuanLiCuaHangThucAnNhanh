using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.DTO
{
    public class HoaDonNhapDTO : INotifyPropertyChanged
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

        private decimal tongTienNhap;
        public decimal TongTienNhap
        {
            get => tongTienNhap;
            set
            {
                tongTienNhap = value;
                OnPropertyChanged(nameof(TongTienNhap));
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

        private ObservableCollection<ChiTietHoaDonNhapDTO> listChiTietHoaDonNhapDTO;
        public ObservableCollection<ChiTietHoaDonNhapDTO> ListChiTietHoaDonNhapDTO
        {
            get => listChiTietHoaDonNhapDTO;
            set
            {
                listChiTietHoaDonNhapDTO= value;
                OnPropertyChanged(nameof(ListChiTietHoaDonNhapDTO));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
