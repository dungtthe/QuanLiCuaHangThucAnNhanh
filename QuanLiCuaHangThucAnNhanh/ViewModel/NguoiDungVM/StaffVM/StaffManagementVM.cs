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
using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using OfficeOpenXml;
using System.Globalization;
using System.IO;
using OfficeOpenXml.Style;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.StaffVM
{
    public class StaffManagementVM : BaseViewModel
    {
        public static List<NguoiDungDTO> staffList;
        private ObservableCollection<NguoiDungDTO> _staffList;

        public ObservableCollection<NguoiDungDTO> StaffList
        {
            get { return _staffList; }
            set { _staffList = value; OnPropertyChanged(nameof(StaffList)); OnPropertyChanged(nameof(Count)); }
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

        private ObservableCollection<NguoiDungDTO> ListImportStaff; //ListView source
        public ObservableCollection<NguoiDungDTO> listImportStaff
        {
            get { return ListImportStaff; }
            set { ListImportStaff = value; OnPropertyChanged(); }
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

        public int Count => StaffList?.Count ?? 0;
        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchStaff { get; }
        public ICommand DeleteStaffCommand { get; }
        public ICommand EditStaffCommand { get; }
        public ICommand OpenEditStaffCommand { get; }
        public ICommand CloseEditStaffCommand { get; }
        public ICommand OpenAddWindowCommand { get; }
        public ICommand CloseAddWindowCommand { get; }
        public ICommand AddStaffCommand { get; }
        public ICommand ImportStaffCommand { get; }
        public ICommand ExportStaffCommand { get; }

        public StaffManagementVM()
        {
            int soLuong = 0;
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {

                StaffList = new ObservableCollection<NguoiDungDTO>(await Task.Run(() => NguoiDungDA.gI().GetAllUser()));
                if (StaffList != null)
                {
                    staffList = new List<NguoiDungDTO>(StaffList);
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

                    string passchuamahoa = MotSoPhuongThucBoTro.RandomPassword();
                    string tentk = this.PhoneNumber; 
                    string pass = MotSoPhuongThucBoTro.MD5Hash(passchuamahoa);

                    NguoiDung newStaff = new NguoiDung
                    {
                        HoTen = this.DisplayName,
                        TenTaiKhoan = tentk,
                        MatKhau = pass,
                        SoDienThoai = this.PhoneNumber,
                        NgaySinh = this.BirthDay,
                        Email = this.Email,
                        Loai = 0,
                        DiaChi = this.Address,
                        IsDeleted = false
                    };
                    (bool IsAdded, string messageAdd) = await NguoiDungDA.Ins.AddNewStaff(newStaff);
                    if (IsAdded)
                    {
                        StaffList = new ObservableCollection<NguoiDungDTO>(await NguoiDungDA.Ins.GetAllUser());
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã thêm thành công nhân viên");
                        p.Close();
                        //Gửi mail tài khoản                      
                        string email = "privateclinicse104@gmail.com";
                            string frompass = "ibap lpjv sqrf vrsq";
                            MailMessage mailMessage = new MailMessage();
                            try
                            {
                                mailMessage.From = new MailAddress(email);
                                mailMessage.Subject = "Tài khoản đăng nhập";
                                mailMessage.To.Add(new MailAddress(this.Email));
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Lỗi khi tạo thông tin email: " + ex.Message, "Thông báo");
                                return;
                            }
                            GuiTaiKhoan(tentk, passchuamahoa, mailMessage, email, frompass);
                            
                           
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
            ImportStaffCommand = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                if (openFileDialog.ShowDialog() == true)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
                            int rowCount = worksheet.Dimension.Rows;
                            int colCount = worksheet.Dimension.Columns;
                            // Duyệt qua từng dòng để đọc dữ liệu
                            listImportStaff = new ObservableCollection<NguoiDungDTO>();
                            for (int row = 5; row <= rowCount; row++) // Bắt đầu từ hàng 5 để bỏ qua header
                            {
                                try
                                {
                                    NguoiDungDTO staff = new NguoiDungDTO();
                                    staff.HoTen = worksheet.Cells[row, 2].Value?.ToString();
                                    staff.Email = worksheet.Cells[row, 3].Value?.ToString();
                                    staff.SoDienThoai = worksheet.Cells[row, 4].Value?.ToString();
                                    string Date = worksheet.Cells[row, 5].Value?.ToString();
                                    DateTime dateBirthDay;
                                    String[] DateFormat = { "dd/MM/yyyy", "d/M/yyyy", "d/MM/yyyy", "dd/M/yyyy", "dd/MM/yyyy h:mm:ss tt", "d/M/yyyy h:mm:ss tt",
                                    "d/MM/yyyy h:mm:ss tt", "dd/M/yyyy h:mm:ss tt","M/d/yyyy","M/dd/yyyy","MM/d/yyyy","MM/dd/yyyy","M/d/yyyy h:mm:ss tt"
                                    ,"M/dd/yyyy h:mm:ss tt","MM/d/yyyy h:mm:ss tt","MM/dd/yyyy h:mm:ss tt"};
                                    if (DateTime.TryParseExact(Date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateBirthDay))
                                    {
                                        staff.NgaySinh = dateBirthDay;
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid Birthday format");
                                        return;
                                    }
                                    staff.DiaChi = worksheet.Cells[row, 6].Value?.ToString();
                                    listImportStaff.Add(staff);
                                }
                                catch
                                {
                                    Error wd5 = new Error("Dữ liệu trong File tải lên không đúng");
                                    wd5.ShowDialog();
                                    listImportStaff = new ObservableCollection<NguoiDungDTO>();
                                    return;
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi import dữ liệu từ tệp Excel: " + ex.Message);
                    }
                    try
                    {
                        string EmailError = "Email đã tồn tại";
                        string PhoneNumberError = "Số điện thoại đã tồn tại";
                        string UserNameError = "Tài khoản đã tồn tại";
                        for (int i = 0; i < listImportStaff.Count; i++)
                        {
                            //gan du lieu cho viewmodel bang null de dam bao k check sai
                            DisplayName = null;
                            UserName = null;
                            PassWord = null;
                            PhoneNumber = null;
                            BirthDay = DateTime.Now;
                            Email = null;
                            Role = "Nhân viên";
                            //gan du lieu tung phan tu vao de thao tac
                            DisplayName = listImportStaff[i].HoTen;
                            PhoneNumber = listImportStaff[i].SoDienThoai;
                            try
                            {
                                if (listImportStaff[i].NgaySinh is DateTime date)
                                {
                                    BirthDay = date;
                                }
                                else
                                {
                                    MessageBox.Show("Invalid date format or type. Please check the input data.");
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("An unexpected error occurred: " + ex.Message);
                            }
                            Email = listImportStaff[i].Email;
                            Role = "Nhân viên";
                            Address = listImportStaff[i].DiaChi;
                            //check ngoai le
                            int iWage = 0;
                            if (DisplayName == null  || PhoneNumber == null  || Email == null
                                || Role == null|| Address == null || DisplayName == ""|| PhoneNumber == "" || Email == ""
                                || Role == ""||Address == "")
                            {
                                MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin của nhân viên thứ " + (i + 1) + " trong file Excel");
                                listImportStaff = new ObservableCollection<NguoiDungDTO>();
                                return;
                            }
                            else
                            {
                                string mailPattern = @"^[a-zA-Z0-9._%+-]+@gmail\.com$";
                                string phonePattern = @"^0\d{9}$";
                                if (!Regex.IsMatch(Email, mailPattern))
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Email nhân viên thứ " + (i + 1) + " trong excel không hợp lệ (phải có dạng @gmail.com)");
                                    return;
                                }
                                if (!Regex.IsMatch(PhoneNumber, phonePattern))
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Số điện thoại nhân viên thứ " + (i + 1) + " trong excel không hợp lệ (phải có 10 chữ số và số bắt đầu là 0)");
                                    return;
                                }
                                if (DateTime.Compare(BirthDay, new DateTime(1900, 1, 1)) < 0 || DateTime.Compare(BirthDay, DateTime.Now) > 0)
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Ngày sinh không hợp lệ của nhân viên thứ " + (i + 1) + " trong file Excel");
                                    listImportStaff = new ObservableCollection<NguoiDungDTO>();
                                    return;
                                }

                                else if (DateTime.Now.Year - (BirthDay).Year < 16)
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Đảm bảo nhân viên thứ " + (i + 1) + " vào làm trên 16 tuổi");
                                    listImportStaff = new ObservableCollection<NguoiDungDTO>();
                                    return;
                                }
                                string passchuamahoa = MotSoPhuongThucBoTro.RandomPassword();
                                string tentk = this.PhoneNumber;
                                string pass = MotSoPhuongThucBoTro.MD5Hash(passchuamahoa);
                                NguoiDung newStaff = new NguoiDung
                                {
                                    HoTen = this.DisplayName,
                                    TenTaiKhoan = tentk,
                                    MatKhau = pass,
                                    SoDienThoai = this.PhoneNumber,
                                    NgaySinh = this.BirthDay,
                                    Email = this.Email,
                                    Loai = 0,
                                    DiaChi = this.Address,
                                    IsDeleted = false
                                };
                                (bool IsAdded, string messageAdd) = await NguoiDungDA.Ins.AddNewStaff(newStaff);
                                if (!IsAdded && messageAdd.Equals(EmailError))
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Nhân viên thứ " + (i + 1) + " trong file excel có " + messageAdd);
                                    return;
                                }
                                else if (!IsAdded && messageAdd.Equals(PhoneNumberError))
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Nhân viên thứ " + (i + 1) + " trong file excel có " + messageAdd);
                                    return;
                                }
                                else if (!IsAdded && messageAdd.Equals(UserNameError))
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Nhân viên thứ " + (i + 1) + " trong file excel có " + messageAdd);
                                    return;
                                }

                                //Gửi mail tài khoản                      
                                string email = "privateclinicse104@gmail.com";
                                string frompass = "ibap lpjv sqrf vrsq";
                                MailMessage mailMessage = new MailMessage();
                                try
                                {
                                    mailMessage.From = new MailAddress(email);
                                    mailMessage.Subject = "Tài khoản đăng nhập";
                                    mailMessage.To.Add(new MailAddress(this.Email));
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Lỗi khi tạo thông tin email: " + ex.Message, "Thông báo");
                                    return;
                                }
                                GuiTaiKhoan(tentk, passchuamahoa, mailMessage, email, frompass);
                            }
                        }
                        StaffList = new ObservableCollection<NguoiDungDTO>(await NguoiDungDA.Ins.GetAllUser());
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã thêm danh sách thành công nhân viên từ Excel");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi import dữ liệu từ tệp Excel: " + ex.Message);
                    }
                }
            });
            ExportStaffCommand = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                string path = MotSoPhuongThucBoTro.SelectFolder();
                if (path == null || StaffList.Count <=0)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn chưa chọn nơi lưu file hoặc danh sách nhân viên không có ai");
                    return;
                }
                else
                {
                    string selectedFolderPath = path;
                    // tạo excel
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    try
                    {
                        using (ExcelPackage package = new ExcelPackage())
                        {
                            // Tạo một worksheet mới
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Staff");

                            // Merge hàng từ hàng 1,2 từ cột A -> L
                            var mergedCells = worksheet.Cells["A1:F2"];
                            mergedCells.Merge = true;
                            //set text 
                            mergedCells.Value = "Danh sách nhân viên";
                            mergedCells.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            mergedCells.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            // Thiết lập cỡ chữ và in đậm
                            mergedCells.Style.Font.Size = 24;
                            mergedCells.Style.Font.Bold = true;
                            mergedCells.Style.Font.Color.SetColor(System.Drawing.Color.Red);

                            //thiet lap mau nen
                            mergedCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            mergedCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Yellow);

                            //ghi tieu de cho cot
                            string[] columnHeaders = { "ID", "Họ tên", "Email", "Số điện thoại", "Ngày sinh", "Địa chỉ" };
                            for (int i = 0; i < columnHeaders.Length; i++)
                            {
                                worksheet.Cells[3, i + 1].Value = columnHeaders[i];
                                worksheet.Cells[3, i + 1].Style.Font.Size = 14; // Thiết lập cỡ chữ 14
                                worksheet.Cells[3, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                            }

                            // Ghi dữ liệu từ danh sách vào worksheet
                            for (int i = 0; i < StaffList.Count; i++)
                            {
                                var employee = StaffList[i];
                                worksheet.Cells[i + 4, 1].Value = employee.ID;
                                worksheet.Cells[i + 4, 2].Value = employee.HoTen;
                                worksheet.Cells[i + 4, 3].Value = employee.Email;
                                worksheet.Cells[i + 4, 4].Value = employee.SoDienThoai;
                                DateTime? birthDate = employee.NgaySinh;
                                if (birthDate.HasValue)
                                {
                                    worksheet.Cells[i + 4, 5].Value = birthDate.Value.ToString("dd/MM/yyyy");
                                }
                                else
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Nhân viên có mã " + employee.ID + " không có ngày sinh");
                                    return;
                                }
                                worksheet.Cells[i + 4, 6].Value = employee.DiaChi;

                                //can trai cho all noi dung cua cot 
                                for (int j = 1; j <= 6; j++)
                                {
                                    worksheet.Cells[i + 4, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                                }
                            }

                            // Định dạng bảng
                            worksheet.Cells.AutoFitColumns();

                            DateTime date = DateTime.Now;
                            string filePath = Path.Combine(selectedFolderPath, "ListStaff.xlsx");
                            package.SaveAs(new FileInfo(filePath));
                            Success wd = new Success($"Tệp Excel đã được lưu vào: {filePath}");
                            wd.ShowDialog();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Error saving file: " + ex.Message);
                    }
                }
            });

        }
        //Hàm tự động gửi mật khẩu qua email
        void GuiTaiKhoan(string tendangnhap, string matkhau,
                        MailMessage mailMessage, string fromMail, string fromPassword)
        {
            string noidung =
                            "Thông tin tài khoản đăng nhập của bạn là: " + "<br>"
                            + "Tên đăng nhập: " + "<b>" + tendangnhap + "</b>" + "<br>"
                            + "Mật khẩu: " + "<b>" + matkhau + "</b>" + "<br>"
                            + "Thân mến!";

            mailMessage.Body = "<html><body>" + noidung + "</body></html>";
            mailMessage.IsBodyHtml = true;
            //Gửi mail
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(fromMail, fromPassword),
                    EnableSsl = true
                };
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch
                {
                    // Xử lý lỗi khi gửi email
                    MessageBox.Show("Lỗi khi gửi tài khoản qua mail\n" +
                                  "Tên đăng nhập: " + tendangnhap + "\n"
                                   + "Mật khẩu: " + matkhau, "Thông báo");
                }
            }
            catch
            {
                // Xử lý lỗi khi khởi tạo SmtpClient
                MessageBox.Show("Lỗi khi gửi tài khoản qua mail\n" +
                                  "Tên đăng nhập: " + tendangnhap + "\n"
                                   + "Mật khẩu: " + matkhau, "Thông báo");
            }
        }
    }
}