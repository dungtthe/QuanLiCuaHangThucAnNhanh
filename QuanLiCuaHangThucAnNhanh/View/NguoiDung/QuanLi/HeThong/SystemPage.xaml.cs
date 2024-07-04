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

namespace QuanLiCuaHangThucAnNhanh.View.NguoiDung.QuanLi.HeThong
{
    /// <summary>
    /// Interaction logic for SystemPage.xaml
    /// </summary>
    public partial class SystemPage : Page
    {
        public SystemPage()
        {
            InitializeComponent();
        }

       
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) ||  
            (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||  
            e.Key == Key.Delete ||  
            e.Key == Key.Back ||  
            (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.A)))
            {
                e.Handled = true; 
            }
        }
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void tb5_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
