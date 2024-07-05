using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QuanLiCuaHangThucAnNhanh.Model.DTO
{
    public class NguoiDungDTO : INotifyPropertyChanged
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

        private string hoTen;
        public string HoTen
        {
            get => hoTen;
            set
            {
                hoTen = value;
                OnPropertyChanged(nameof(HoTen));
            }
        }

        private Nullable<System.DateTime> ngaySinh;
        public Nullable<System.DateTime> NgaySinh
        {
            get => ngaySinh;
            set
            {
                ngaySinh = value;
                OnPropertyChanged(nameof(NgaySinh));
            }
        }

        private string soDienThoai;
        public string SoDienThoai
        {
            get => soDienThoai;
            set
            {
                soDienThoai = value;
                OnPropertyChanged(nameof(SoDienThoai));
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string diaChi;
        public string DiaChi
        {
            get => diaChi;
            set
            {
                diaChi = value;
                OnPropertyChanged(nameof(DiaChi));
            }
        }

        private string tenTaiKhoan;
        public string TenTaiKhoan
        {
            get => tenTaiKhoan;
            set
            {
                tenTaiKhoan = value;
                OnPropertyChanged(nameof(TenTaiKhoan));
            }
        }

        private string matKhau;
        public string MatKhau
        {
            get => matKhau;
            set
            {
                matKhau = value;
                OnPropertyChanged(nameof(MatKhau));
            }
        }

        private Nullable<int> loai;
        public Nullable<int> Loai
        {
            get => loai;
            set
            {
                loai = value;
                OnPropertyChanged(nameof(Loai));
                if (loai != null)
                {
                    if (loai == 0)
                    {
                        ChucVu = "Nhân viên";
                    }
                    else
                    {
                        ChucVu = "Quản lí";
                    }
                }
            }
        }

        private byte[] image;
        public byte[] Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
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

        private string chucVu;
        public string ChucVu
        {
            get => chucVu;
            set
            {
                chucVu = value;
                OnPropertyChanged(nameof(ChucVu));
            }
        }
       

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
