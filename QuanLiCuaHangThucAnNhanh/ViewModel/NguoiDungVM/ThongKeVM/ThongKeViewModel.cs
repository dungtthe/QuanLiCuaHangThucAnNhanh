using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.ThongKe.DoanhThu;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.ThongKeVM
{
    public partial class ThongKeViewModel:BaseViewModel
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
        bool checkThaoTac = false;
        public ICommand LichSuBanCM { get; set; }
        public ICommand LichSuNhapCM { get; set; }
        public ICommand RevenueCM { get; set; }


        #endregion


        bool checkLanDau = false;
        public ThongKeViewModel()
        {

            #region Lịch sử bán

            LichSuBanCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                if (p == null) return;


                if (!checkThaoTac && checkLanDau)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thao tác quá nhanh!");
                    return;
                }
                checkThaoTac = false;

                DanhSachHoaDon = new ObservableCollection<HoaDonBanDTO>(await Task.Run(() => HoaDonBanDA.gI().GetAllHoaDonBan()));
                if (DanhSachHoaDon != null)
                {
                    danhSachHoaDon = new List<HoaDonBanDTO>(DanhSachHoaDon);
                    // Lọc hóa đơn theo ngày tháng năm
                    DanhSachHoaDon = new ObservableCollection<HoaDonBanDTO>(
                        danhSachHoaDon.FindAll(x => x.NgayTao.HasValue && x.NgayTao.Value.Date >= SelectedDateFrom.Date && x.NgayTao.Value.Date <= SelectedDateTo.Date)
                    );
                }


                p.Content = new View.NguoiDung.ThongKe.LichSuBan.LichSuTable();

                checkThaoTac = true;
                checkLanDau = true;
                CaseNav = 0;
                await loadDataForDateChange();

            });
            #endregion


            #region Lịch sử nhập

            LichSuNhapCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                if (p == null) return;
                if (!checkThaoTac)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thao tác quá nhanh!");
                    return;
                }
                checkThaoTac = false;



                p.Content = new View.NguoiDung.ThongKe.LichSuNhap.LichSuTable();
                checkThaoTac = true;
                CaseNav = 1;

            });
            #endregion

            #region DoanhThu
            RevenueCM = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                if (p == null) return;
                if (!checkThaoTac)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Thao tác quá nhanh!");
                    return;
                }
                checkThaoTac = false;


                await LoadRevenueData(p);
                checkThaoTac = true;
                CaseNav = 2;
                await loadDataForDateChange();
            });
            #endregion


        }
        private async Task loadDataForDateChange()
        {
            if (CaseNav == -1) return;

            //lịch sử bán
            if (CaseNav == 1)
            {

                DanhSachHoaDon = new ObservableCollection<HoaDonBanDTO>(await Task.Run(() => HoaDonBanDA.gI().GetAllHoaDonBan()));
                if (DanhSachHoaDon != null)
                {
                    danhSachHoaDon = new List<HoaDonBanDTO>(DanhSachHoaDon);
                    // Lọc hóa đơn theo ngày tháng năm
                    DanhSachHoaDon = new ObservableCollection<HoaDonBanDTO>(
                        danhSachHoaDon.FindAll(x => x.NgayTao.HasValue && x.NgayTao.Value.Date >= SelectedDateFrom.Date && x.NgayTao.Value.Date <= SelectedDateTo.Date)
                    );
                }
                return;
            }
        }


        private async Task LoadRevenueData(Frame p = null)
        {
            if (p == null) return;
            p.Content = new DoanhThuTable();
        }
    }
}
