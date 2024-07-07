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
using System.Windows.Shapes;

namespace QuanLiCuaHangThucAnNhanh.View.NguoiDung
{
    /// <summary>
    /// Interaction logic for InformationPersonalView.xaml
    /// </summary>
    public partial class InformationPersonalView : Window
    {
        public InformationPersonalView()
        {
            InitializeComponent();
            DataContext = MainNguoiDungView.mainNguoiDungVM;
        }
        private void HuyButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
