using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.DTO
{
    public class KhachHangDTO : INotifyPropertyChanged
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


        private Nullable<int> diemTichLuy;
        public Nullable<int> DiemTichLuy
        {
            get => diemTichLuy;
            set
            {
                diemTichLuy = value;
                OnPropertyChanged(nameof(DiemTichLuy));
            }
        }


        private Nullable<bool> isDelete;
        public Nullable<bool> IsDelete
        {
            get=> isDelete;
            set
            {
                isDelete = value;
                OnPropertyChanged(nameof(IsDelete));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
