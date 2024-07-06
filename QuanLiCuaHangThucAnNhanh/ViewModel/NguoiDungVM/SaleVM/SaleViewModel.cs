﻿using MaterialDesignColors;
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
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.SaleVM
{
    public class SaleViewModel : BaseViewModel
    {
        private ThamSoDTO thamSoDTO;
        private List<SanPhamDTO> listSanPhamAll;
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

        //khách hàng cho bill

        private KhachHangDTO khachHangForBill;
        public KhachHangDTO KhachHangForBill
        {
            get => khachHangForBill;
            set
            {
                khachHangForBill = value;
                OnPropertyChanged(nameof(KhachHangForBill));
            }
        }

        private KhachHangDTO khachVangLai;



        //text tìm kiếm khách hàng
        private string _cusInfo;
        public string CusInfo
        {
            get { return _cusInfo; }
            set { _cusInfo = value; OnPropertyChanged(); }
        }



        //chọn item bên danh sách sản phẩm
        private SanPhamDTO _selectedItemSanPhamDTO;

        public SanPhamDTO SelectedItemSanPhamDTO
        {
            get { return _selectedItemSanPhamDTO; }
            set { _selectedItemSanPhamDTO = value; OnPropertyChanged(nameof(SelectedItemSanPhamDTO)); }
        }

        //chọn item bên đơn hàng
        private ChiTietHoaDonBanDTO _selectedItemChiTietHoaDonBan;
        public ChiTietHoaDonBanDTO SelectedItemChiTietHoaDonBan
        {
            get => _selectedItemChiTietHoaDonBan;
            set
            {
                _selectedItemChiTietHoaDonBan = value;
                OnPropertyChanged(nameof(SelectedItemChiTietHoaDonBan));
            }
        }

        //danh sách sản phẩm trong đơn hàng
        private ObservableCollection<ChiTietHoaDonBanDTO> listChiTietHoaDonBan;
        public ObservableCollection<ChiTietHoaDonBanDTO> ListChiTietHoaDonBan
        {
            get => listChiTietHoaDonBan;
            set
            {
                listChiTietHoaDonBan = value;
                OnPropertyChanged(nameof(ListChiTietHoaDonBan));
            }
        }

        private decimal tongTien;
        public decimal TongTien
        {
            get => tongTien;
            set
            {
                tongTien = value;
                OnPropertyChanged(nameof(TongTien));
            }
        }


        private bool _payEnabled;
        public bool PayEnabled
        {
            get { return _payEnabled; }
            set { _payEnabled = value; OnPropertyChanged(); }
        }
        private SanPhamDTO selectedItem;
        public SanPhamDTO SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
            }
        }
        private string chuoiSearch;
        public string ChuoiSearch
        {
            get => chuoiSearch;
            set
            {
                chuoiSearch = value; OnPropertyChanged(nameof(ChuoiSearch));
            }
        }

        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchCusCM { get; set; }

        //chọn item cho sản phẩm 
        public ICommand SelectSanPhamDTOCM { get; set; }
        public ICommand PayBill { get; set; }
        public ICommand Search { get; set; }
        public ICommand RemoveSanPhamDTOCM { get; set; }
        
        public SaleViewModel()
        {
            khachVangLai = new KhachHangDTO(1, "Khách vãng lai");
            KhachHangForBill = khachVangLai;
            ListChiTietHoaDonBan = new ObservableCollection<ChiTietHoaDonBanDTO>();
            TongTien = 0;
            PayEnabled = false;

            FirstLoadCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {
                thamSoDTO = await ThamSoDA.gI().GetThamSoCur();
                if (thamSoDTO == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }


                List<SanPhamDTO> sanPhams = await SanPhamDA.gI().GetAllSanPham();
                listSanPhamAll = new List<SanPhamDTO>(sanPhams);
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


            SearchCusCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                if (string.IsNullOrEmpty(CusInfo))
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Vui lòng nhập số điện thoại hoặc Email!");
                    return;
                }

                KhachHangDTO temp = null;


                temp = await KhachHangDA.gI().FindNguoiDungBySoDienThoai(CusInfo);

                if (temp == null)
                {
                    temp = await KhachHangDA.gI().FindNguoiDungByEmail(CusInfo);
                }

                if (temp == null)
                {
                    Error wd1 = new Error("Khách hàng không tồn tại");
                    temp = khachVangLai;
                    CusInfo = "";
                    wd1.ShowDialog();

                }
                KhachHangForBill = temp;
            });



            SelectSanPhamDTOCM = new RelayCommand<SanPhamDTO>((p) => { return true; }, (p) =>
            {
                if (SelectedItemSanPhamDTO != null)
                {
                    AddSanPhamDTOToListChiTietHoaDon(SelectedItemSanPhamDTO);
                }
            });

            PayBill = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                Warning wd = new Warning("Xác nhận thanh toán hóa đơn?");
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    HoaDonBanDTO hoaDonBanDTO = new HoaDonBanDTO();
                    hoaDonBanDTO.ListChiTietHoaDonBanDTO = new List<ChiTietHoaDonBanDTO>(ListChiTietHoaDonBan);
                    hoaDonBanDTO.NgayTao = DateTime.Now;
                    hoaDonBanDTO.TongTienBan = TongTien;
                    hoaDonBanDTO.NguoiDungID = MainNguoiDungVM.nguoiDungDTOCur.ID;
                    hoaDonBanDTO.KhachHangID = KhachHangForBill.ID;
                    hoaDonBanDTO.IsDeleted = false;

                    bool flag = await HoaDonBanDA.gI().AddNewBill(hoaDonBanDTO);

                    if (flag)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Thanh toán đơn hàng thành công!");
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    }


                }

            });

            #region tìm kiếm sản phẩm
            Search = new RelayCommand<TextBox>((p) => { return true; }, async (p) =>
            {
                ChuoiSearch = p.Text;
                if (p.Text == "")
                {
                    UpdateCb();
                }
                else
                {
                    ProductList = new ObservableCollection<SanPhamDTO>(listSanPhamAll);
                    prdList = new List<SanPhamDTO>(ProductList);
                    if (DanhMucSelect != "Tất cả thể loại")
                    {
                        ProductList = new ObservableCollection<SanPhamDTO>(prdList.FindAll(x => x.TenSP.ToLower().Contains(p.Text.ToLower()) && x.DanhMucSanPhamDTO.TenDanhMuc == DanhMucSelect));
                    }
                    else
                    {
                        ProductList = new ObservableCollection<SanPhamDTO>(prdList.FindAll(x => x.TenSP.ToLower().Contains(p.Text.ToLower())));
                    }
                }

            });
            #endregion

            RemoveSanPhamDTOCM = new RelayCommand<ChiTietHoaDonBanDTO>((p) => { return true; }, (p) =>
            {
                if (SelectedItemChiTietHoaDonBan != null)
                {
                    RemoveSanPhamDTOFromDonHang(SelectedItemChiTietHoaDonBan);
                }
            });
        }



        private void AddSanPhamDTOToListChiTietHoaDon(SanPhamDTO sanPhamDTO)
        {

            bool flag = false;
            foreach(var item in ListChiTietHoaDonBan)
            {
                if(item.SanPhamDTO.ID == sanPhamDTO.ID)
                {
                    flag = true;
                    if (item.SoLuong >= sanPhamDTO.SoLuongTon)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Số lượng sản phẩm đã hết!");
                    }
                    else
                    {
                        item.SoLuong++;
                    }
                    break;
                }
            }

            if (!flag)
            {
                ChiTietHoaDonBanDTO chiTietHoaDonBanDTO = new ChiTietHoaDonBanDTO();
                chiTietHoaDonBanDTO.SanPhamID = sanPhamDTO.ID;
                chiTietHoaDonBanDTO.SanPhamDTO = sanPhamDTO;
                chiTietHoaDonBanDTO.SoLuong = 1;
                chiTietHoaDonBanDTO.DonGia = sanPhamDTO.GiaBan;
                chiTietHoaDonBanDTO.IsDeleted = false;
                chiTietHoaDonBanDTO.ThanhTien = sanPhamDTO.GiaBan;
                ListChiTietHoaDonBan.Add(chiTietHoaDonBanDTO);

            }
            SetTongTien();
        }




        private void RemoveSanPhamDTOFromDonHang(ChiTietHoaDonBanDTO chiTietHoaDonBanDTO)
        {
            ListChiTietHoaDonBan.Remove(chiTietHoaDonBanDTO);
            List<ChiTietHoaDonBanDTO> listTemp = new List<ChiTietHoaDonBanDTO>();
            foreach(var item in ListChiTietHoaDonBan)
            {
                if (item != chiTietHoaDonBanDTO)
                {
                    listTemp.Add(item);
                }
            }
            ListChiTietHoaDonBan = new ObservableCollection<ChiTietHoaDonBanDTO>(listTemp);
            SetTongTien();
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


        void SetTongTien()
        {
            TongTien = 0;
            foreach(var item in ListChiTietHoaDonBan)
            {
                TongTien +=item.ThanhTien;
            }

            if (TongTien != 0)
            {
                PayEnabled = true;
            }
            else
            {
                PayEnabled = false;
            }
        }
    }
}
