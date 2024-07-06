using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.CustomerMangament;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.StaffManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
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
        private KhachHangDTO _selectedItem;

        public KhachHangDTO SelectedItem
        {
            get { return _selectedItem; }
            set { _selectedItem = value; OnPropertyChanged(); }
        }
        private ObservableCollection<KhachHangDTO> cusObservation; //ListView source
        public ObservableCollection<KhachHangDTO> CusObservation
        {
            get { return cusObservation; }
            set
            {
                cusObservation = value; OnPropertyChanged(nameof(CusObservation));

            }
        }
        private int id;
        public int ID
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged(nameof(ID));
            }
        }


        private string hoTen;
        public string HoTen
        {
            get => hoTen;
            set
            {
                hoTen = value;
                OnPropertyChanged(nameof(HoTen));
            }
        }


        private Nullable<System.DateTime> ngaySinh;
        public Nullable<System.DateTime> NgaySinh
        {
            get => ngaySinh;
            set
            {
                ngaySinh = value;
                OnPropertyChanged(nameof(NgaySinh));
            }
        }


        private string soDienThoai;
        public string SoDienThoai
        {
            get => soDienThoai;
            set
            {
                soDienThoai = value;
                OnPropertyChanged(nameof(SoDienThoai));
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


        private string diaChi;
        public string DiaChi
        {
            get => diaChi;
            set
            {
                diaChi = value;
                OnPropertyChanged(nameof(DiaChi));
            }
        }


        private Nullable<int> diemTichLuy;
        public Nullable<int> DiemTichLuy
        {
            get => diemTichLuy;
            set
            {
                diemTichLuy = value;
                OnPropertyChanged(nameof(DiemTichLuy));
            }
        }


        private Nullable<bool> isDelete;
        public Nullable<bool> IsDelete
        {
            get => isDelete;
            set
            {
                isDelete = value;
                OnPropertyChanged(nameof(IsDelete));
            }
        }
        //Edit
        private string _editname;

        public string EditName
        {
            get { return _editname; }
            set { _editname = value; }
        }
        private string _editemail;

        public string EditEmail
        {
            get { return _editemail; }
            set { _editemail = value; }
        }
        private string _editphoneNumber;

        public string EditPhoneNumber
        {
            get { return _editphoneNumber; }
            set { _editphoneNumber = value; }
        }
        private string _editAddress;

        public string EditAddress
        {
            get { return _editAddress; }
            set { _editAddress = value; }
        }
       
        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchCus { get; }
        public ICommand OpenAddWindowCommand { get; }
        public ICommand CloseAddWindowCommand { get; }
        public ICommand AddCusCommand { get; }
        public ICommand CloseWdCM { get; set; }
        public ICommand OpenEditWdCM { get; set; }
        public ICommand EditCusListCM { get; set; }
        public ICommand DeleteCusListCM { get; set; }
        public CustomerManagementViewModel()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                CustomerList = new ObservableCollection<KhachHangDTO>(await Task.Run(() => KhachHangDA.gI().GetAllCus()));
                if (CustomerList != null)
                    cusList = new List<KhachHangDTO>(CustomerList);
            });
            //Tìm kiếm
            SearchCus = new RelayCommand<TextBox>(null, (p) =>
            {
                if (p.Text != null)
                {
                    if (cusList != null)
                        CustomerList = new ObservableCollection<KhachHangDTO>(cusList.FindAll(x => x.HoTen.ToLower().Contains(p.Text.ToLower())));
                }
            });
            // Them KH

            AddCusCommand = new RelayCommand<Window>(null, async (p) =>
            {

                if (HoTen == null || Email == null || SoDienThoai == null
                || DiaChi == null || NgaySinh == null)
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
                    if (!Regex.IsMatch(SoDienThoai, phonePattern))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Số điện thoại không hợp lệ (phải có 10 chữ số và số bắt đầu là 0)");
                        return;
                    }

                    if (NgaySinh.HasValue && (NgaySinh.Value < new DateTime(1900, 1, 1) || NgaySinh.Value > DateTime.Now))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày sinh không hợp lệ");
                        return;
                    }

                    KhachHang newCus = new KhachHang
                    {
                        HoTen = this.HoTen,
                        Email = this.Email,
                        SoDienThoai = this.SoDienThoai,
                        DiaChi = this.DiaChi,
                        NgaySinh = this.NgaySinh,
                        DiemTichLuy = 0,
                        IsDeleted = false
                    };

                    (bool IsAdded, string messageAdd) = await KhachHangDA.Ins.AddNewCus(newCus);
                    if (IsAdded)
                    {
                        CusObservation = new ObservableCollection<KhachHangDTO>(await KhachHangDA.Ins.GetAllCus());
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã thêm thành công khách hàng");
                        p.Close();
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                    }
                }

            });
            //đóng mở thêm KH
            OpenAddWindowCommand = new RelayCommand<Page>((mainStaffWindow) => { return true; }, (mainStaffWindow) =>
            {
                HoTen = null;
                Email = null;
                SoDienThoai = null;
                DiaChi = null;
                NgaySinh = DateTime.Now;
                DiemTichLuy = null;
                CustomerAddingView addCusWindow = new CustomerAddingView();
                addCusWindow.ShowDialog();
            });

            CloseAddWindowCommand = new RelayCommand<Window>((mainStaffWindow) => { return true; }, (mainStaffWindow) =>
            {
                mainStaffWindow.Close();
                
            });
            // Mở edit và edit KH
            OpenEditWdCM = new RelayCommand<object>((p) => { return true; }, (p) => {
                EditName = SelectedItem.HoTen;
                EditEmail = SelectedItem.Email;
                EditPhoneNumber = SelectedItem.SoDienThoai;
                EditAddress = SelectedItem.DiaChi;
                CustomerEditingView wd = new CustomerEditingView();
                wd.ShowDialog();
            });
            EditCusListCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {

                if (this.EditName == null || this.EditPhoneNumber == null || this.EditEmail == null || this.EditName.Trim() == ""
                || this.EditAddress.Trim() == "" || this.EditAddress == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Không nhập đủ dữ liệu");
                }
                else
                {
                    string mailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                    string phonePattern = @"^0\d{9}$";
                    if (!Regex.IsMatch(EditPhoneNumber, phonePattern))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Số điện thoại không hợp lệ");
                    }
                    else
                    {
                        if (!Regex.IsMatch(EditEmail, mailPattern))
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, "Địa chỉ mail không hợp lệ");
                        }
                        else
                        {
                            
                            KhachHang newCus = new KhachHang
                            {
                                ID = SelectedItem.ID,                   
                                SoDienThoai = EditPhoneNumber,
                                Email = EditEmail,
                                HoTen = EditName,
                                DiaChi = EditAddress,
                                IsDeleted = false,
                            };
                            (bool success, string messageEdit) = await KhachHangDA.Ins.EditCusList(newCus, SelectedItem.ID);
                            if (success)
                            {
                                p.Close();
                                CustomerList = new ObservableCollection<KhachHangDTO>(await KhachHangDA.Ins.GetAllCus());
                                MessageBoxCustom.Show(MessageBoxCustom.Success, "Đã sửa khách hàng thành công");
                            }
                            else
                                MessageBox.Show(messageEdit);
                        }
                    }
                }

            });
            DeleteCusListCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                Warning wd = new Warning();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    (bool sucess, string messageDelete) = await KhachHangDA.Ins.DeleteCustomer(SelectedItem.ID);
                    if (sucess)
                    {
                        CustomerList.Remove(SelectedItem);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Đã xóa khách hàng thành công");
                    }
                    else
                    {
                        MessageBox.Show(messageDelete);
                    }
                }

            });
            CloseWdCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });
        }
    }
}
