using Microsoft.Win32;
using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Utils;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.BanHang;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.CustomerMangament;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.QuanLi.HeThong;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.QuanLi.SanPhamManagement;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.StaffManagement;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.ThongKe;
using QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.SaleVM;
using QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.StaffVM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM
{
    public class MainNguoiDungVM : BaseViewModel
    {
        public static NguoiDungDTO nguoiDungDTOCur;

        public ICommand FirstLoadCM { get; set; }
        public ICommand LoadNhanVienPage { get; }
        public ICommand LoadKhachHangPage { get; set; }
        public ICommand LoadSanPhamPage { get; set; }
        public ICommand LoadThongKePage { get; set; }
        public ICommand LoadHeThongPage { get; set; }
        public ICommand LoadBanHangPage { get; set; }

        #region Phần command và Prop của hiển thị thông tin cá nhân
        public ICommand OpenAccountWindow {  get; set; }
        public ICommand PasswordChangedInformationCommand0 { get; set; }
        public ICommand PasswordChangedInformationCommand1 { get; set; }
        public ICommand PasswordChangedInformationCommand2 { get; set; }
        public ICommand DoiMatKhauCommand { get; set; }

        public ICommand editImageCommand { get; set; }
        public ICommand DoiMatKhau { get; set; }
        public ICommand EditTTCommand { get; set; }

        private string password0;
        public string Password0
        {
            get => password0;
            set
            {
                password0 = value;
                OnPropertyChanged(nameof(Password0));
            }
        }

        private string password1;
        public string Password1
        {
            get => password1;
            set
            {
                password1 = value;
                OnPropertyChanged(nameof(Password1));
            }
        }

        private string password2;
        public string Password2
        {
            get => password2;
            set
            {
                password2 = value;
                OnPropertyChanged(nameof(Password2));
            }
        }

        private string hoten;
        public string HoTen
        {
            get => hoten;
            set
            {
                hoten = value;
                OnPropertyChanged(nameof(HoTen));
            }
        }

        private string chucvu;
        public string ChucVu
        {
            get => chucvu;
            set
            {
                chucvu = value;
                OnPropertyChanged(nameof(ChucVu));
            }
        }

        private string email;
        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private string sdt;
        public string SDT
        {
            get => sdt;
            set
            {
                sdt = value;
                OnPropertyChanged(nameof(SDT));
            }
        }

        private string diachi;
        public string DiaChi
        {
            get => diachi;
            set
            {
                diachi = value;
                OnPropertyChanged(nameof(DiaChi));
            }
        }

        private string ngaysinh;
        public string NgaySinh
        {
            get => ngaysinh;
            set
            {
                ngaysinh = value;
                OnPropertyChanged(nameof(NgaySinh));
            }
        }

        private byte[] image;
        public byte[] Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }



        #endregion
        public MainNguoiDungVM()
        {
            LoadThongKePage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ThongKeMainPage();
            });

            LoadBanHangPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new SalePage();
            });
            LoadKhachHangPage = new RelayCommand<Frame>((p) => { return true; }, (p) => { p.Content = new CustomerManagementView(); });
            LoadNhanVienPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new StaffManagementView();
            });

            LoadSanPhamPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new ProductMain();
            });

            LoadHeThongPage = new RelayCommand<Frame>((p) => { return true; }, (p) =>
            {
                p.Content = new SystemPage();
            });

            //Load trang TTCN
            OpenAccountWindow = new RelayCommand<object>(null, (p) =>
            {
                
                HoTen = nguoiDungDTOCur.HoTen;
                Email = nguoiDungDTOCur.Email;
                SDT = nguoiDungDTOCur.SoDienThoai;
                DiaChi = nguoiDungDTOCur.DiaChi;
                NgaySinh= nguoiDungDTOCur.NgaySinh.ToString();
                ChucVu = nguoiDungDTOCur.ChucVu;
                Image = nguoiDungDTOCur.Image;
                InformationPersonalView view = new InformationPersonalView();
                view.ShowDialog();
            });
            
            PasswordChangedInformationCommand0 = new RelayCommand<PasswordBox>(null, (p) =>
            {
                Password0 = p.Password;
            });

            PasswordChangedInformationCommand1 = new RelayCommand<PasswordBox>(null, (p) =>
            {
                Password1 = p.Password;
            });

            PasswordChangedInformationCommand2 = new RelayCommand<PasswordBox>(null, (p) =>
            {
                Password2 = p.Password;
            });
            #region chỉnh sửa hình ảnh
            editImageCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.png;*.jpeg;*.webp;*.gif|All Files|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    string path = openFileDialog.FileName;
                    if (path != null)
                    {
                        Image = MotSoPhuongThucBoTro.ImagePathToByteArray(path);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Tải ảnh lên thất bại!");
                    }
                }
            });
            #endregion

            //Hàm accept sửa thông tin
            EditTTCommand = new RelayCommand<Window>(null, async (p) =>
            {
                if (HoTen == null || SDT == null
                || DiaChi == null || NgaySinh == null || Email == null
                || HoTen == "" || SDT == ""
                || DiaChi == "" || NgaySinh == "" || Email == "")
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                }

                else
                {
                    string mailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                    string phonePattern = @"^0\d{9}$";
                    if (!Regex.IsMatch(Email, mailPattern))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Email không hợp lệ (phải có dạng @gmail.com)");
                        return;
                    }
                    if (!Regex.IsMatch(SDT, phonePattern))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Số điện thoại không hợp lệ (phải có 10 chữ số và số bắt đầu là 0)");
                        return;
                    }
                    DateTime tempBirthDay;
                    //chuyen Birthday tu chuoi sang datetime
                    tempBirthDay = MotSoPTBoTro.formatDate(NgaySinh);
                    if (tempBirthDay == DateTime.MinValue)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra trong việc chuyển đổi ngày sinh");
                        return;
                    }


                    if (DateTime.Compare(tempBirthDay, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare(tempBirthDay, DateTime.Now) > 0)
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày sinh không hợp lệ");

                    else
                    {
                       
                        NguoiDung newStaff = new NguoiDung
                        {
                            ID = nguoiDungDTOCur.ID,
                            HoTen = this.HoTen,
                            Email = this.Email,
                            TenTaiKhoan = nguoiDungDTOCur.TenTaiKhoan,
                            MatKhau = nguoiDungDTOCur.MatKhau,
                            NgaySinh = tempBirthDay,
                            SoDienThoai = this.SDT,
                            Loai = nguoiDungDTOCur.Loai,
                            DiaChi = this.DiaChi,
                            IsDeleted = false,
                            Image=this.Image
                        };
                        (bool success, string messageEdit) = await NguoiDungDA.gI().EditStaff(newStaff);
                        if (success)
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã chỉnh sửa thành công");
                            nguoiDungDTOCur.Image= this.Image;
                            p?.Close();
                        }
                        else
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, messageEdit);
                        }
                    }

                }


            });
            //Hàm đổi mật khẩu
            // Command để đổi mật khẩu
            DoiMatKhauCommand = new RelayCommand<object>(null, async (p) =>
            {
                if (Password0 == null || Password0 == "" || Password1 == null || Password1 == ""
                    || Password2 == null || Password2 == "")
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa nhập đủ thông tin mật khẩu.");
                    return;
                }

                if (Password1 != Password2)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Mật khẩu mới không trùng khớp.");
                    return;
                }

                // Validate mật khẩu cũ
                string hashedOldPassword = MotSoPhuongThucBoTro.MD5Hash(Password0);
                if (hashedOldPassword != nguoiDungDTOCur.MatKhau)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Mật khẩu cũ không đúng.");
                    return;
                }

                // Hash mật khẩu mới
                string hashedNewPassword = MotSoPhuongThucBoTro.MD5Hash(Password1);

                // Tạo đối tượng người dùng mới với mật khẩu đã thay đổi
                NguoiDung newNguoiDung = new NguoiDung
                {
                    ID = nguoiDungDTOCur.ID,
                    HoTen = this.HoTen,
                    Email = this.Email,
                    TenTaiKhoan = nguoiDungDTOCur.TenTaiKhoan,
                    MatKhau = hashedNewPassword, // Sử dụng mật khẩu mới đã hash
                    NgaySinh = MotSoPTBoTro.formatDate(NgaySinh),
                    SoDienThoai = this.SDT,
                    Loai = nguoiDungDTOCur.Loai,
                    DiaChi = this.DiaChi,
                    IsDeleted = false,
                    Image = nguoiDungDTOCur.Image
                };

                // Gọi hàm chỉnh sửa thông tin người dùng với mật khẩu đã thay đổi
                (bool success, string messageEdit) = await NguoiDungDA.gI().EditStaff(newNguoiDung);
                if (success)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Đổi mật khẩu thành công.");
                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Đổi mật khẩu thất bại: " + messageEdit);
                }
            });

        }
    }
}
