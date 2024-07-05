﻿using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
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

        private string tenTaiKhoan;
        public string TenTaiKhoan
        {
            get => tenTaiKhoan;
            set
            {
                tenTaiKhoan = value;
                OnPropertyChanged(nameof(TenTaiKhoan));
            }
        }

        private string matKhau;
        public string MatKhau
        {
            get => matKhau;
            set
            {
                matKhau = value;
                OnPropertyChanged(nameof(MatKhau));
            }
        }

        private Nullable<int> loai;
        public Nullable<int> Loai
        {
            get => loai;
            set
            {
                loai = value;
                OnPropertyChanged(nameof(Loai));
                if (loai != null)
                {
                    if (loai == 0)
                    {
                        ChucVu = "Nhân viên";
                    }
                    else
                    {
                        ChucVu = "Quản lí";
                    }
                }
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

        private Nullable<bool> isDeleted;
        public Nullable<bool> IsDeleted
        {
            get => isDeleted;
            set
            {
                isDeleted = value;
                OnPropertyChanged(nameof(IsDeleted));
            }
        }

        private string chucVu;
        public string ChucVu
        {
            get => chucVu;
            set
            {
                chucVu = value;
                OnPropertyChanged(nameof(ChucVu));
            }
        }


        public ICommand FirstLoadCM { get; set; }
        public ICommand OpenAddWindowCommand { get; }
        public ICommand CloseAddWindowCommand { get; }
        public ICommand SearchStaff { get; }
        public ICommand AddStaffCommand { get; }
        public ICommand ImportStaffCommand { get; }
        public ICommand ExportStaffCommand { get; }
        public ICommand PasswordChangedCommand { get; }
        public ICommand ConfirmPasswordChangedCommand { get; }
        public ICommand DeleteStaffCommand { get; }
        public ICommand EditStaffCommand { get; }
        public ICommand OpenEditStaffCommand { get; }
        public ICommand CloseEditStaffCommand { get; }
        public ICommand EditPasswordChangedCommand { get; }
        public StaffManagementVM()
        {
            FirstLoadCM = new RelayCommand<Page>((p) => { return true; }, async (p) =>
            {
                StaffList = new ObservableCollection<NguoiDungDTO>(await Task.Run(() => NguoiDungDA.gI().GetAllUser()));
                if (StaffList != null)
                    staffList = new List<NguoiDungDTO>(StaffList);
            });
            //Tìm kiếm
            SearchStaff = new RelayCommand<TextBox>(null, (p) =>
            {
                if (p.Text != null)
                {
                    if (staffList != null)
                        StaffList = new ObservableCollection<NguoiDungDTO>(staffList.FindAll(x => x.HoTen.ToLower().Contains(p.Text.ToLower())));
                }
            });
            //đóng mở thêm nv
            OpenAddWindowCommand = new RelayCommand<Page>((mainStaffWindow) => { return true; }, (mainStaffWindow) =>
            {
                HoTen = null;
                Email = null;
                SoDienThoai = null;
                DiaChi = null;
                NgaySinh = DateTime.Now;
                StaffAddingView addStaffWindow = new StaffAddingView();
                addStaffWindow.ShowDialog();
            });

            CloseAddWindowCommand = new RelayCommand<Window>((mainStaffWindow) => { return true; }, (mainStaffWindow) =>
            {
                mainStaffWindow.Close();
            });

        }
    }
}
