using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.KhachHangVM
{
    public class CustomerManagementViewModel : BaseViewModel
    {
        public static List<KhachHangDTO> cusList;
        private ObservableCollection<KhachHangDTO> _customerList;

        public ObservableCollection<KhachHangDTO> CustomerList
        {
            get { return _customerList; }
            set { _customerList = value; OnPropertyChanged(); }
        }


        public ICommand FirstLoadCM { get; set; }


        public CustomerManagementViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                CustomerList = new ObservableCollection<KhachHangDTO>(await Task.Run(() => KhachHangDA.gI().GetAllCus()));
                if (CustomerList != null)
                    cusList = new List<KhachHangDTO>(CustomerList);
            });
        }
    }
}
