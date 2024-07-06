using QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.StaffVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuanLiCuaHangThucAnNhanh.View.NguoiDung.StaffManagement
{
    /// <summary>
    /// Interaction logic for StaffManagementView.xaml
    /// </summary>
    public partial class StaffManagementView : Page
    {
        public StaffManagementView()
        {
            InitializeComponent();
        }
        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as StaffManagementVM).OpenEditStaffCommand.Execute(new object());
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as StaffManagementVM).DeleteStaffCommand.Execute(new object());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
