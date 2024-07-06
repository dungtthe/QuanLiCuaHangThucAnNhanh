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
using System.Linq;
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
                if (EditDisplayName == null ||  EditPhoneNumber == null
                || EditBirthDay == null ||  EditEmail == null||EditAddress == null
                || EditDisplayName == "" || EditPhoneNumber == ""
                || EditEmail == ""|| EditBirthDay== ""||EditAddress=="")
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
                            DiaChi=this.EditAddress,
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
        }
    }
}
