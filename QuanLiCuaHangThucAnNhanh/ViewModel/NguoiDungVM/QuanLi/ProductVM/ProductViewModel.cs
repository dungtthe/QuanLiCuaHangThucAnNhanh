﻿using Microsoft.Win32;
using OfficeOpenXml;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.QuanLi.ProductVM
{
    public partial class ProductViewModel:BaseViewModel
    {
        private ThamSoDTO thamSoDTO;


        //danh sách sản phẩm 
        private List<SanPhamDTO> prdList;
        private ObservableCollection<SanPhamDTO> _productList;

        public ObservableCollection<SanPhamDTO> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged(nameof(ProductList)); }
        }


        //danh sách tên danh mục sản phẩm
        private ObservableCollection<string> combogenrelist;
        public ObservableCollection<string> ComboList
        {
            get { return combogenrelist; }
            set { combogenrelist = value; OnPropertyChanged(nameof(ComboList)); }
        }

        //danh mục sản phẩm đang chọn
        private string _danhMucSelect;
        public string DanhMucSelect
        {
            get { return _danhMucSelect; }
            set { _danhMucSelect = value; OnPropertyChanged(); UpdateCb(); }
        }

        public ICommand FirstLoadCM { get; set; }
        public ICommand NhapKhoCM { get; set; }


        public ProductViewModel()
        {
            #region load list product
            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                if (p == null) return;

                thamSoDTO = await ThamSoDA.gI().GetThamSoCur();
                if (thamSoDTO == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }


                List<SanPhamDTO> sanPhams = await SanPhamDA.gI().GetAllSanPham();
                SetGia(sanPhams);

                ProductList = new ObservableCollection<SanPhamDTO>(sanPhams);
                if (ProductList != null)
                {
                    prdList = new List<SanPhamDTO>(ProductList);
                }


                ComboList = new ObservableCollection<string>(await DanhMucSanPhamDA.gI().GetAllTenDanhMucSanPham());
                ComboList.Insert(0, "Tất cả thể loại");
                DanhMucSelect = "Tất cả thể loại";
            });
            #endregion



            #region nhập kho
            NhapKhoCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Excel Files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == false) return;

                string FilePath = openFileDialog.FileName;

                Warning wd = new Warning("Vui lòng xác nhận lại!");
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {

                }
               

            });
            #endregion
        }


        void SetGia(List<SanPhamDTO> sanPhams)
        {
            if (sanPhams == null)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                return;
            }

            foreach (SanPhamDTO item in sanPhams)
            {
                double? heso = thamSoDTO.HeSoBan;
                if (heso.HasValue)
                {
                    decimal hesoDecimal = (decimal)heso.Value;
                    item.GiaBan = item.GiaNhap * hesoDecimal;
                }
            }
        }


        async void UpdateCb()
        {
            List<SanPhamDTO> sanPhams = await SanPhamDA.gI().GetAllSanPham();
            SetGia(sanPhams);
            ProductList = new ObservableCollection<SanPhamDTO>(sanPhams);
            prdList = new List<SanPhamDTO>(ProductList);
            if (DanhMucSelect != "Tất cả thể loại")
            {
                ProductList = new ObservableCollection<SanPhamDTO>(prdList.FindAll(x => (x.DanhMucSanPhamDTO.TenDanhMuc == DanhMucSelect)));
            }
        }

    }
}
