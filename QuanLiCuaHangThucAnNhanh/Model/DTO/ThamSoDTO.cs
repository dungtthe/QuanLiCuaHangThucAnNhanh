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


        private Nullable<decimal> onePointToMoney;
        public Nullable<decimal> OnePointToMoney
        {
            get => onePointToMoney;
            set
            {
                onePointToMoney = value;
                OnPropertyChanged(nameof(OnePointToMoney));
            }
        }

        private Nullable<int> moneyToOnePoint;

        public Nullable<int> MoneyToOnePoint
        {
            get => moneyToOnePoint;
            set
            {
                moneyToOnePoint = value;
                OnPropertyChanged(nameof(MoneyToOnePoint));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
