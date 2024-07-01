﻿using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM
{
    public class MainNguoiDungVM:BaseViewModel
    {
        public static NguoiDungDTO nguoiDungDTOCur;

        public ICommand FirstLoadCM { get; set; }
        public ICommand LoadNhanVienPage { get; }
        public ICommand LoadKhachHangPage { get; set; }
        public ICommand LoadSanPhamPage { get; set; }
        public ICommand LoadThongKePage { get; set; }
        public ICommand LoadHeThongPage { get; set; }
        public ICommand LoadBanHangPage { get; set; }

        public MainNguoiDungVM()
        {

            MessageBoxCustom.Show(MessageBoxCustom.Success,nguoiDungDTOCur.HoTen);
          
        }
    }
}
