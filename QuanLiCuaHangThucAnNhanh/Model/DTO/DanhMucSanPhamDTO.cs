using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.DTO
{
    public class DanhMucSanPhamDTO : INotifyPropertyChanged
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

        private string tenDanhMuc;
        public string TenDanhMuc
        {
            get => tenDanhMuc;
            set
            {
                tenDanhMuc = value;
                OnPropertyChanged(nameof(TenDanhMuc));
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
