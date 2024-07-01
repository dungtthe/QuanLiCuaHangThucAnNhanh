using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace QuanLiCuaHangThucAnNhanh.Model.DTO
{
    public class SanPhamDTO : INotifyPropertyChanged
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

        private string tenSP;
        public string TenSP
        {
            get => tenSP;
            set
            {
                tenSP = value;
                OnPropertyChanged(nameof(TenSP));
            }
        }

        private int soLuongTon;
        public int SoLuongTon
        {
            get => soLuongTon;
            set
            {
                soLuongTon = value;
                OnPropertyChanged(nameof(SoLuongTon));
            }
        }

        private decimal giaNhap;
        public decimal GiaNhap
        {
            get => giaNhap;
            set
            {
                giaNhap = value;
                OnPropertyChanged(nameof(GiaNhap));
            }
        }

        private int danhMucSanPhamID;
        public int DanhMucSanPhamID
        {
            get => danhMucSanPhamID;
            set
            {
                danhMucSanPhamID = value;
                OnPropertyChanged(nameof(DanhMucSanPhamID));
            }
        }

        private DanhMucSanPhamDTO danhMucSanPhamDTO;
        public DanhMucSanPhamDTO DanhMucSanPhamDTO
        {
            get => danhMucSanPhamDTO;
            set
            {
                danhMucSanPhamDTO= value;
                OnPropertyChanged(nameof(DanhMucSanPhamDTO));
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
