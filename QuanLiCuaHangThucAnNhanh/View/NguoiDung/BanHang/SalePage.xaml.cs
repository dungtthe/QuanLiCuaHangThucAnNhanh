﻿using QuanLiCuaHangThucAnNhanh.View.MessageBox;
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

namespace QuanLiCuaHangThucAnNhanh.View.NguoiDung.BanHang
{
    /// <summary>
    /// Interaction logic for SalePage.xaml
    /// </summary>
    public partial class SalePage : Page
    {
        public SalePage()
        {
            InitializeComponent();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer.ScrollToVerticalOffset(ScrollViewer.VerticalOffset - e.Delta);
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

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

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                e.Handled = true;
            }
        }

        private void NumericTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!int.TryParse(NumericTextBox.Text, out int result))
            {

            }
           
        }

        private void NumericTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private static bool IsTextAllowed(string text)
        {
            foreach (char c in text)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }


    

        private void txtSl_TextChanged(object sender, TextChangedEventArgs e)
        {

            //TextBox textBox = sender as TextBox;

            //if (string.IsNullOrEmpty(textBox.Text))
            //{
            //    Pay_btn.IsEnabled = false;
            //}
        }
    }
}
