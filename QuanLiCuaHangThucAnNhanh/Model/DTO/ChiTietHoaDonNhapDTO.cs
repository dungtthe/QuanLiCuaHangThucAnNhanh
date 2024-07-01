using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.DTO
{
    public class ChiTietHoaDonNhapDTO : INotifyPropertyChanged
    {
        private int hoaDonNhapID;
        public int HoaDonNhapID
        {
            get => hoaDonNhapID;
            set
            {
                hoaDonNhapID = value;
                OnPropertyChanged(nameof(HoaDonNhapID));
            }
        }

        private int sanPhamID;
        public int SanPhamID
        {
            get => sanPhamID;
            set
            {
                sanPhamID = value;
                OnPropertyChanged(nameof(SanPhamID));
            }
        }

        private int soLuong;
        public int SoLuong
        {
            get => soLuong;
            set
            {
                soLuong = value;
                OnPropertyChanged(nameof(SoLuong));
            }
        }

        private decimal donGia;
        public decimal DonGia
        {
            get => donGia;
            set
            {
                donGia = value;
                OnPropertyChanged(nameof(DonGia));
            }
        }

        private bool isDeleted;
        public bool IsDeleted
        {
            get => isDeleted;
            set
            {
                isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }


        private HoaDonNhapDTO hoaDonNhapDTO;
        public HoaDonNhapDTO HoaDonNhapDTO
        {
            get => hoaDonNhapDTO;
            set
            {
                hoaDonNhapDTO = value;
                OnPropertyChanged(nameof(HoaDonNhapDTO));
            }
        }

        private SanPhamDTO sanPhamDTO;
        public SanPhamDTO SanPhamDTO
        {
            get => sanPhamDTO;
            set
            {
                sanPhamDTO = value;
                OnPropertyChanged(nameof(SanPhamDTO));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

