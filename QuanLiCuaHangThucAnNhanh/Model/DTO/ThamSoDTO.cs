using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.DTO
{
    public class ThamSoDTO : INotifyPropertyChanged
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

        private Nullable<double> heSoBan;
        public Nullable<double> HeSoBan
        {
            get => heSoBan;
            set
            {
                heSoBan = value;
                OnPropertyChanged(nameof(HeSoBan));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
