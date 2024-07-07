using QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuanLiCuaHangThucAnNhanh.View.NguoiDung
{
    /// <summary>
    /// Interaction logic for MainNguoiDungView.xaml
    /// </summary>
    public partial class MainNguoiDungView : Window
    {
        public MainNguoiDungView()
        {
            InitializeComponent();
            DataContext = new MainNguoiDungVM();
        }
        private void Overlay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            BeginStoryboard((Storyboard)Resources["MenuClose"]);
        }

        private void AdminWD_Closed(object sender, System.EventArgs e)
        {
            this.Owner.Visibility = Visibility.Visible;
        }
    }
}
