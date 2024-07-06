using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Utils;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.StaffManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.StaffVM
{
    public class StaffManagementVM : BaseViewModel
    {
        public static List<NguoiDungDTO> staffList;
        private ObservableCollection<NguoiDungDTO> _staffList;

        public ObservableCollection<NguoiDungDTO> StaffList
        {
            get { return _staffList; }
            set { _staffList = value; OnPropertyChanged(); }
        }
        private ObservableCollection<NguoiDungDTO> staffObservation; //ListView source
        public ObservableCollection<NguoiDungDTO> StaffObservation
        {
            get { return staffObservation; }
            set
            {
                staffObservation = value; OnPropertyChanged(nameof(StaffObservation));
                OnPropertyChanged(nameof(Count));
            }
        }

        //Add Staff
        private string displayName;

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }


        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string passWord;

        public string PassWord
        {
            get { return passWord; }
            set { passWord = value; }
        }

        private string confirmPassWord;

        public string ConfirmPassWord
        {
            get { return confirmPassWord; }
            set { confirmPassWord = value; }
        }

        private string phoneNumber;

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }

        private DateTime birthDay;
        public DateTime BirthDay
        {
            get { return birthDay; }
            set { birthDay = value; }
        }

        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        private string role;

        public string Role
        {
            get { return role; }
            set { role = value; }
        }
        private string address;
        public string Address
        {
            get { return address; }
            set { address = value; }
        }
        private NguoiDungDTO selectedItem;

        public NguoiDungDTO SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; }
        }

        // Edit Staff
        private string editDisplayName;

        public string EditDisplayName
        {
            get { return editDisplayName; }
            set { editDisplayName = value; }
        }


        private string editUserName;

        public string EditUserName
        {
            get { return editUserName; }
            set { editUserName = value; }
        }

        private string editPassWord;

        public string EditPassWord
        {
            get { return editPassWord; }
            set { editPassWord = value; }
        }

        private string editPhoneNumber;

        public string EditPhoneNumber
        {
            get { return editPhoneNumber; }
            set { editPhoneNumber = value; }
        }

        private string editBirthDay;

        public string EditBirthDay
        {
            get { return editBirthDay; }
            set { editBirthDay = value; }
        }

        private string editEmail;
        public string EditEmail
        {
            get { return editEmail; }
            set { editEmail = value; }
        }

        private int editRole;
        public int EditRole
        {
            get { return editRole; }
            set { editRole = value; }
        }
        private string editAddress;
        public string EditAddress
        {
            get { return editAddress; }
            set { editAddress = value; }
        }
        private string showEditRole;
        public string ShowEditRole
        {
            get { return showEditRole; }
            set { showEditRole = value; }
        }

        public int Count => StaffObservation?.Count ?? 0;
        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchStaff { get; }
        public ICommand DeleteStaffCommand { get; }
        public ICommand EditStaffCommand { get; }
        public ICommand OpenEditStaffCommand { get; }
        public ICommand CloseEditStaffCommand { get; }
        public ICommand OpenAddWindowCommand { get; }
        public ICommand CloseAddWindowCommand { get; }
        public ICommand AddStaffCommand { get; }


        public StaffManagementVM()
        {
            int soLuong = 0;
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {

                StaffList = new ObservableCollection<NguoiDungDTO>(await Task.Run(() => NguoiDungDA.gI().GetAllUser()));
                if (StaffList != null)
                {
                    staffList = new List<NguoiDungDTO>(StaffList);
                    soLuong = staffList.Count();
                }

            });

            SearchStaff = new RelayCommand<TextBox>(null, (p) =>
            {
                if (p.Text != null)
                {
                    if (staffList != null)
                        StaffList = new ObservableCollection<NguoiDungDTO>(staffList.FindAll(x => x.HoTen.ToLower().Contains(p.Text.ToLower())));
                }
            });
            OpenAddWindowCommand = new RelayCommand<Page>((mainStaffWindow) => { return true; }, (mainStaffWindow) =>
            {
                DisplayName = null;
                UserName = null;
                PassWord = null;
                PhoneNumber = null;
                BirthDay = DateTime.Now;
                Email = null;
                Role = "Nhân viên";
                ConfirmPassWord = null;
                Address = null;
                StaffAddingView addStaffWindow = new StaffAddingView();
                addStaffWindow.ShowDialog();
            });
            CloseAddWindowCommand = new RelayCommand<Window>((mainStaffWindow) => { return true; }, (mainStaffWindow) =>
            {
                mainStaffWindow.Close();
            });
            AddStaffCommand = new RelayCommand<Window>(null, async (p) =>
            {
                if (DisplayName == null  || PhoneNumber == null  || Email == null
                || Role == null|| Address == null || DisplayName == ""|| PhoneNumber == "" || Email == ""
                || Role == ""||Address == "")
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
                    if (!Regex.IsMatch(PhoneNumber, phonePattern))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Số điện thoại không hợp lệ (phải có 10 chữ số và số bắt đầu là 0)");
                        return;
                    }
                    if (DateTime.Compare(BirthDay, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare(BirthDay, DateTime.Now) > 0)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày sinh không hợp lệ");
                        return;
                    }
                    else if (DateTime.Now.Year - (BirthDay).Year < 16)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Đảm bảo nhân viên vào làm trên 16 tuổi");
                        return;
                    }
                    string pass = MotSoPhuongThucBoTro.MD5Hash(this.PassWord);

                    NguoiDung newStaff = new NguoiDung
                    {
                        HoTen = this.DisplayName,
                        TenTaiKhoan = this.UserName,
                        MatKhau = pass,
                        SoDienThoai = this.PhoneNumber,
                        NgaySinh = this.BirthDay,
                        Email = this.Email,
                        Loai = Int32.Parse( this.Role),
                        IsDeleted = false
                    };
                    (bool IsAdded, string messageAdd) = await NguoiDungDA.Ins.AddNewStaff(newStaff);
                    if (IsAdded)
                    {
                        StaffObservation = new ObservableCollection<NguoiDungDTO>(await NguoiDungDA.Ins.GetAllUser());
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã thêm thành công nhân viên");
                        p.Close();
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageAdd);
                    }
                }

            });
            OpenEditStaffCommand = new RelayCommand<object>(null, (p) =>
            {
                EditBirthDay = SelectedItem.NgaySinh.ToString();
                EditDisplayName = SelectedItem.HoTen;
                EditEmail = SelectedItem.Email;
                EditPassWord = null;
                EditPhoneNumber = SelectedItem.SoDienThoai;
                EditRole = (int)SelectedItem.Loai;
                if (EditRole == 0)
                {
                    showEditRole = "Nhân viên";
                }
                EditUserName = SelectedItem.TenTaiKhoan;
                EditAddress = SelectedItem.DiaChi;
                StaffEditingView staffEditingView = new StaffEditingView();
                staffEditingView.ShowDialog();
            });
            CloseEditStaffCommand = new RelayCommand<Window>(null, (p) =>
            {
                p.Close();
            });
            EditStaffCommand = new RelayCommand<Window>(null, async (p) =>
            {
                if (EditDisplayName == null || EditPhoneNumber == null
                || EditBirthDay == null || EditEmail == null || EditAddress == null
                || EditDisplayName == "" || EditPhoneNumber == ""
                || EditEmail == "" || EditBirthDay == "" || EditAddress == "")
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                }

                else
                {
                    string mailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                    string phonePattern = @"^0\d{9}$";
                    if (!Regex.IsMatch(EditEmail, mailPattern))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Email không hợp lệ (phải có dạng @gmail.com)");
                        return;
                    }
                    if (!Regex.IsMatch(EditPhoneNumber, phonePattern))
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Số điện thoại không hợp lệ (phải có 10 chữ số và số bắt đầu là 0)");
                        return;
                    }
                    DateTime tempBirthDay;
                    //chuyen Birthday tu chuoi sang datetime
                    tempBirthDay = MotSoPTBoTro.formatDate(EditBirthDay);
                    if (tempBirthDay == DateTime.MinValue)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra trong việc chuyển đổi ngày sinh");
                        return;
                    }
                    if (EditDisplayName == SelectedItem.HoTen && EditEmail == SelectedItem.Email
                        && tempBirthDay == SelectedItem.NgaySinh && EditPhoneNumber == SelectedItem.SoDienThoai
                        && EditAddress == SelectedItem.DiaChi)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Không có gì mới để chỉnh sửa");
                        p.Close();
                        return;
                    }
                    if (DateTime.Compare(tempBirthDay, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare(tempBirthDay, DateTime.Now) > 0)
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày sinh không hợp lệ");

                    else
                    {
                        string pass;
                        if (EditPassWord == null || EditPassWord == "")
                        {
                            EditPassWord = SelectedItem.MatKhau;
                            pass = EditPassWord;
                        }
                        else
                            pass = MotSoPhuongThucBoTro.MD5Hash(EditPassWord);
                        NguoiDung newStaff = new NguoiDung
                        {
                            ID = SelectedItem.ID,
                            HoTen = this.EditDisplayName,
                            Email = this.EditEmail,
                            TenTaiKhoan = this.EditUserName,
                            MatKhau = pass,
                            NgaySinh = tempBirthDay,
                            SoDienThoai = this.EditPhoneNumber,
                            Loai = this.EditRole,
                            DiaChi = this.EditAddress,
                            IsDeleted = false
                        };
                        (bool success, string messageEdit) = await NguoiDungDA.gI().EditStaff(newStaff);
                        if (success)
                        {
                            StaffList = new ObservableCollection<NguoiDungDTO>(await NguoiDungDA.gI().GetAllUser());
                            MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã chỉnh sửa thành công");
                            p.Close();
                        }
                        else
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, messageEdit);
                        }
                    }

                }
            });
            DeleteStaffCommand = new RelayCommand<object>(null, async (p) =>
            {
                DeleteMessage deleteMessage = new DeleteMessage("Bạn có chắc chắn muốn xóa không?");
                deleteMessage.ShowDialog();
                if (deleteMessage.DialogResult == true)
                {
                    (bool IsDeleted, string messageDelete) = await NguoiDungDA.Ins.DeleteStaff(selectedItem.ID);
                    if (IsDeleted)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã xóa thành công nhân viên");
                        StaffList = new ObservableCollection<NguoiDungDTO>(await NguoiDungDA.Ins.GetAllUser());
                    }
                    else
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                }
            });
        }
    }
}