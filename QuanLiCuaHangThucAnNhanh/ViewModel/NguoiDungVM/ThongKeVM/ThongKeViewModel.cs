using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.ThongKe.DoanhThu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.ThongKeVM
{
    public class ThongKeViewModel:BaseViewModel
    {

        #region chọn ngày
        private DateTime _selectedDateFrom = DateTime.Now.AddDays(-2);
        public DateTime SelectedDateFrom
        {
            get { return _selectedDateFrom; }
            set
            {
                _selectedDateFrom = value;
                OnPropertyChanged(nameof(SelectedDateFrom));

                Task.Run(async () => { await loadDataForDateChange(); });
            }
        }

        private DateTime _selectedDateTo = DateTime.Now;
        public DateTime SelectedDateTo
        {
            get { return _selectedDateTo; }
            set
            {
                _selectedDateTo = value;
                OnPropertyChanged(nameof(SelectedDateTo));

                Task.Run(async () => { await loadDataForDateChange(); });
            }
        }

        #endregion


        #region các Icommand 
        public ICommand LichSuBanCM { get; set; }
        public ICommand LichSuNhapCM { get; set; }
        public ICommand RevenueCM { get; set; }


        #endregion


        public ThongKeViewModel()
        {

            #region Lịch sử bán

            LichSuBanCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                if (p == null) return;

                p.Content = new View.NguoiDung.ThongKe.LichSuBan.LichSuTable();


            });
            #endregion


            #region Lịch sử nhập

            LichSuNhapCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                if (p == null) return;

                p.Content = new View.NguoiDung.ThongKe.LichSuNhap.LichSuTable();

            });
            #endregion

            #region DoanhThu
            RevenueCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                if (p == null) return;

                await LoadRevenueData(p);
                await loadDataForDateChange();
            });
            #endregion


        }
        private async Task loadDataForDateChange()
        {

        }


        private async Task LoadRevenueData(Frame p = null)
        {
            if (p == null) return;
            p.Content = new DoanhThuTable();
        }
    }
}
