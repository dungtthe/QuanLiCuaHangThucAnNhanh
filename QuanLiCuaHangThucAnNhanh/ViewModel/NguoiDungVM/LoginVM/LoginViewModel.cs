using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Utils;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.LoginVM
{
    public class LoginViewModel:BaseViewModel
    {

        private string _Username;

        public string Username
        {
            get { return _Username; }
            set { _Username = value; OnPropertyChanged(); }
        }
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { _Password = value; OnPropertyChanged(); }
        }

     


        public ICommand LoginCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }

        private bool IsLogin = false;


        public LoginViewModel()
        {
            LoginCommand = new RelayCommand<Window>((p) =>
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    return false;
                }
                return true;
            },
           async (p) =>
           {
               if (IsLogin == false)
               {
                   IsLogin = true;
                   await Login(p);
                   IsLogin = false;
               }

           });

            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) =>
            {
                return true;
            }, (p) =>
            {
               Password = p.Password;
            });
        }


        async Task Login(Window p)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {

                    NguoiDungDTO nguoidung = await NguoiDungDA.gI().FindNguoiDungByUsernameAndPassword(Username, MotSoPhuongThucBoTro.MD5Hash(Password));

                    if (nguoidung != null)
                    {
                        p.Visibility = Visibility.Collapsed;


                        PasswordBox a = (PasswordBox)p.FindName("passwordBox");
                        a.Password = "";

                        MainNguoiDungVM.nguoiDungDTOCur = nguoidung;
                        MainNguoiDungView ad = new MainNguoiDungView();
                        ad.Owner = p;
                        ad.Show();
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Sai tài khoản hoặc mật khẩu, vui lòng nhập lại!");
                    }
                }
            }
            catch 
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra khi đăng nhập");
            }
        }

    }
}
