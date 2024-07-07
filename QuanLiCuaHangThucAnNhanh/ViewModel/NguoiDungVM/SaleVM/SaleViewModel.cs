using MaterialDesignColors;
using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Utils;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.BanHang;
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
                UpdateInfoKhachHang();
            }
        }

        private KhachHangDTO khachVangLai;


        private int pointKhachHang;
        public int PointKhachHang
        {
            get => pointKhachHang;
            set
            {
                pointKhachHang = value;
                OnPropertyChanged(nameof(PointKhachHang));
            }
        }

        private string pointUse;
        public string PointUse
        {
            get => pointUse;
            set
            {
                try
                {
                    //if (string.IsNullOrEmpty(value))
                    //{
                    //    MsgErrorUsePoint = "Không hợp lệ!";
                    //    return;
                    //}
                    //MsgErrorUsePoint = "";
                    pointUse = value;



                    if (!string.IsNullOrEmpty(pointUse))
                    {
                        if (int.Parse(pointUse) > PointKhachHang)
                        {
                            pointUse = pointKhachHang + "";
                        }
                    }

                    OnPropertyChanged(nameof(PointUse));
                    SetTongTien();
                }
                catch { }
            }
        }

        private bool canUsePoint;
        public bool CanUsePoint
        {
            get => canUsePoint;
            set
            {
                canUsePoint = value;
                OnPropertyChanged(nameof(CanUsePoint));
            }
        }

        private string msgErrorUsePoint;
        public string MsgErrorUsePoint
        {
            get => msgErrorUsePoint;
            set
            {
                msgErrorUsePoint = value;
                OnPropertyChanged(nameof(MsgErrorUsePoint));
            }
        }

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

        private HoaDonBanDTO hoaDonThanhToan;
        public HoaDonBanDTO HoaDonThanhToan
        {
            get => hoaDonThanhToan;
            set
            {
                hoaDonThanhToan = value;
                OnPropertyChanged(nameof(HoaDonThanhToan));
            }
        }

        private KhachHangDTO khachHangDTONew;
        public KhachHangDTO KhachHangDTONew
        {
            get => khachHangDTONew;
            set
            {
                khachHangDTONew = value;
                OnPropertyChanged(nameof(KhachHangDTONew));
            }
        }



        public ICommand FirstLoadCM { get; set; }
        public ICommand SearchCusCM { get; set; }

        //chọn item cho sản phẩm 
        public ICommand SelectSanPhamDTOCM { get; set; }
        public ICommand PayBill { get; set; }
        public ICommand Search { get; set; }
        public ICommand RemoveSanPhamDTOCM { get; set; }
        public ICommand EndBill {  get; set; }
        public ICommand ChangeCountCM {  get; set; }
        public ICommand AddCustomerCM {  get; set; }
        public ICommand AcceptAddCusCM { get; set; }
        public ICommand CloseAddCusCM {  get; set; }
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
                SetTongTien();
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
                    HoaDonThanhToan = hoaDonBanDTO;

                    bool flag = false;
                    if (KhachHangForBill.ID != 1)
                    {
                        KhachHang khachHang = new KhachHang();
                        khachHang.ID = khachHangForBill.ID;
                        int point = 0;

                        try
                        {
                            point = int.Parse(PointUse);
                        }
                        catch { }
                        khachHang.DiemTichLuy = (int)(TongTien / thamSoDTO.MoneyToOnePoint.Value) + khachHangForBill.DiemTichLuy - point;
                         flag = await KhachHangDA.gI().EditCusBySell(khachHang);
                    }

                    flag = await HoaDonBanDA.gI().AddNewBill(hoaDonBanDTO);

                    if (flag)
                    {
                     MessageBoxCustom.Show(MessageBoxCustom.Success, "Thanh toán đơn hàng thành công!");


                        //List<SanPhamDTO> listTemp = new List<SanPhamDTO>(listSanPhamAll);
                        //foreach(var item in ListChiTietHoaDonBan)
                        //{
                        //    foreach(SanPhamDTO sanPhamDTO in listTemp)
                        //    {
                        //        if (sanPhamDTO.ID == item.SanPhamID)
                        //        {
                        //            item.SanPhamDTO = sanPhamDTO;
                        //        }
                        //    }
                        //}

                        new InvoicePrint().ShowDialog();

                        List<SanPhamDTO> sanPhams = await SanPhamDA.gI().GetAllSanPham();
                        listSanPhamAll = new List<SanPhamDTO>(sanPhams);
                        SetGia(sanPhams);

                        ProductList = new ObservableCollection<SanPhamDTO>(sanPhams);
                        if (ProductList != null)
                        {
                            prdList = new List<SanPhamDTO>(ProductList);
                        }

                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    }

                    KhachHangForBill = khachVangLai;
                    ListChiTietHoaDonBan = new ObservableCollection<ChiTietHoaDonBanDTO>();
                    TongTien = 0;
                    CusInfo = "";
                    PayEnabled = false;
                }

            });


            EndBill = new RelayCommand<Frame>((p) => { return true; }, async (p) =>
            {
                KhachHangForBill = khachVangLai;
                ListChiTietHoaDonBan = new ObservableCollection<ChiTietHoaDonBanDTO>();
                TongTien = 0;
                CusInfo = "";
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


            ChangeCountCM = new RelayCommand<ChiTietHoaDonBanDTO>((p) => { return true; }, (p) =>
            {
                if (p == null) return;

                SelectedItemChiTietHoaDonBan = p;

                try
                {
                    if (SelectedItemChiTietHoaDonBan.SoLuong < 0)
                    {
                        SelectedItemChiTietHoaDonBan.SoLuong = 0;
                    }
                    else
                    {
                        if (SelectedItemChiTietHoaDonBan.SoLuong > SelectedItemChiTietHoaDonBan.SanPhamDTO.SoLuongTon)
                        {
                            SelectedItemChiTietHoaDonBan.SoLuong = SelectedItemChiTietHoaDonBan.SanPhamDTO.SoLuongTon;
                        }
                    }
                    SetTongTien();
                }
                catch
                {
                }

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
                SetTongTien();
            });

            AddCustomerCM = new RelayCommand<object>((p) => { return true; }, async (p) =>
            {

                if (p == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }
                KhachHangDTONew = new KhachHangDTO
                {
                    HoTen = "Unknown",
                    NgaySinh=DateTime.Now,
                    DiaChi = "Unknown",
                    DiemTichLuy=0,
                    IsDelete=false,
                };



            });
        }



       

        private void AddSanPhamDTOToListChiTietHoaDon(SanPhamDTO sanPhamDTO)
        {

            bool flag = false;
            foreach (var item in ListChiTietHoaDonBan)
            {
                if (item.SanPhamDTO.ID == sanPhamDTO.ID)
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
            foreach (var item in ListChiTietHoaDonBan)
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
            if (ListChiTietHoaDonBan == null) return;
            TongTien = 0;
            foreach (var item in ListChiTietHoaDonBan)
            {
                TongTien += item.ThanhTien;
            }


            if (thamSoDTO.OnePointToMoney.HasValue)
            {
                try
                {
                    if (CanUsePoint )
                    {
                       TongTien = TongTien - thamSoDTO.OnePointToMoney.Value * int.Parse(PointUse);
                    }
                    
                }
                catch{ }

            }

            if (TongTien > 0)
            {
                PayEnabled = true;
            }
            else
            {
                PayEnabled = false;
            }

            if (CanUsePoint)
            {
                if (string.IsNullOrEmpty(PointUse))
                {
                    PayEnabled = false;
                }
            }
        }

        void UpdateInfoKhachHang()
        {
            PointKhachHang = 0;
            PointUse = "0";
            if (khachHangForBill == khachVangLai)
            {
                CanUsePoint = false;
            }
            else
            {
                if (khachHangForBill.DiemTichLuy == 0)
                {
                    CanUsePoint = false;
                }
                else
                {
                    CanUsePoint = true;

                    if (KhachHangForBill.DiemTichLuy.HasValue)
                    {
                        PointKhachHang = KhachHangForBill.DiemTichLuy.Value;
                    }
                }
            }


        }
    }
}
