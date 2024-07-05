using MaterialDesignColors;
using Microsoft.Win32;
using OfficeOpenXml;
using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Utils;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.QuanLi.SanPhamManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.QuanLi.ProductVM
{
    public partial class ProductViewModel : BaseViewModel
    {
        private ThamSoDTO thamSoDTO;



        //danh sách sản phẩm 

        private List<SanPhamDTO> listSanPhamAll;

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

        private string chuoiSearch;
        public string ChuoiSearch
        {
            get => chuoiSearch;
            set
            {
                chuoiSearch = value; OnPropertyChanged(nameof(ChuoiSearch));
            }
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

        public ICommand FirstLoadCM { get; set; }
        public ICommand NhapKhoCM { get; set; }
        public ICommand Search { get; set; }
        public ICommand OpenEditWdCM { get; set; }
        public ICommand CloseWdCM { get; set; }
        public ICommand AcceptEditCM { get; set; }
        public ICommand EditImageCM { get; set; }


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

                string filePath = openFileDialog.FileName;

                Warning wd = new Warning("Vui lòng xác nhận lại!");
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    try
                    {
                        List<SanPhamDTO> sanPhamDTOs = new List<SanPhamDTO>();

                        List<SanPham> listSanPhamInsert = new List<SanPham>();
                        List<SanPham> listSanPhamUpdate = new List<SanPham>();

                        List<DanhMucSanPhamDTO> listDanhMuc = await DanhMucSanPhamDA.gI().GetAllDanhMucSanPham();

                        if (listDanhMuc == null)
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                            return;
                        }

                        using (var package = new ExcelPackage(new FileInfo(filePath)))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // dữ liệu nằm ở worksheet đầu tiên
                            int rowCount = worksheet.Dimension.Rows;

                            for (int row = 2; row <= rowCount; row++)
                            {
                                string tenDanhMuc = worksheet.Cells[row, 4].Value.ToString();

                                int idDanhMucSP = GetIdDanhMucSanPhamByTenDanhMuc(tenDanhMuc, listDanhMuc);
                                if (idDanhMucSP == -1)
                                {
                                    MessageBoxCustom.Show(MessageBoxCustom.Error, $"Không tồn tại danh mục sản phẩm có tên {tenDanhMuc}");
                                    return;
                                }

                                SanPham sanPham = new SanPham
                                {
                                    TenSP = worksheet.Cells[row, 1].Value.ToString(),
                                    SoLuongTon = int.Parse(worksheet.Cells[row, 2].Value.ToString()),
                                    GiaNhap = decimal.Parse(worksheet.Cells[row, 3].Value.ToString()),
                                    DanhMucSanPhamID = idDanhMucSP,
                                    Image = null,
                                    IsDeleted = false,
                                };

                                int idProductOld = IsSanPhamOld(sanPham);
                                if (idProductOld == -1)
                                {
                                    listSanPhamInsert.Add(sanPham);
                                }
                                else
                                {
                                    sanPham.ID = idProductOld;
                                    sanPham.Image = GetImage(sanPham.ID);
                                    listSanPhamUpdate.Add(sanPham);

                                }

                            }

                            foreach (var item in listSanPhamInsert)
                            {
                                await SanPhamDA.gI().AddNewProduct(item);
                            }

                            foreach (var item in listSanPhamUpdate)
                            {
                                await SanPhamDA.gI().UpdateProductQuantity(item);
                            }


                            MessageBoxCustom.Show(MessageBoxCustom.Success, "Nhập kho thành công!");


                            List<SanPhamDTO> sanPhams = await SanPhamDA.gI().GetAllSanPham();
                            listSanPhamAll = new List<SanPhamDTO>(sanPhams);

                            SetGia(sanPhams);

                            ProductList = new ObservableCollection<SanPhamDTO>(sanPhams);
                            if (ProductList != null)
                            {
                                prdList = new List<SanPhamDTO>(ProductList);
                            }

                            DanhMucSelect = "Tất cả thể loại";
                            UpdateCb();
                        }
                    }
                    catch
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    }
                }
            });
            #endregion


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


            #region edit sản phẩm
            OpenEditWdCM = new RelayCommand<SanPhamDTO>((p) => { return true; }, (p) =>
            {
                SelectedItem = p;
                if (p == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }
                ComboList.RemoveAt(0);
                ProductEditingView wd = new ProductEditingView();
                wd.ShowDialog();
            });
            #endregion

            #region close window
            CloseWdCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                CloseEditWindow(p);
            });
            #endregion

            #region chấp nhận edit
            AcceptEditCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (p != null && SelectedItem != null)
                {
                    List<DanhMucSanPhamDTO> listDanhMuc = await DanhMucSanPhamDA.gI().GetAllDanhMucSanPham();


                    SanPham sanPham = new SanPham()
                    {
                        ID=SelectedItem.ID,
                        TenSP = SelectedItem.TenSP,
                        DanhMucSanPhamID = GetIdDanhMucSanPhamByTenDanhMuc(SelectedItem.DanhMucSanPhamDTO.TenDanhMuc, listDanhMuc),
                        Image = SelectedItem.Image,
                    };

                    await SanPhamDA.gI().UpdateProduct(sanPham);
                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Cập nhật sản phẩm thành công!");

                }
                else
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                }

                CloseEditWindow(p);


            });
            #endregion


            #region chỉnh sửa hình ảnh
            EditImageCM = new RelayCommand<object>((p) => { return true; }, (p) =>
            {

                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.png;*.jpeg;*.webp;*.gif|All Files|*.*";
                if (openFileDialog.ShowDialog() == true)
                {
                    string path = openFileDialog.FileName;
                    if (path != null)
                    {
                        SelectedItem.Image = MotSoPhuongThucBoTro.ImagePathToByteArray(path);
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Tải ảnh lên thất bại!");
                    }
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


        void UpdateCb()
        {
            List<SanPhamDTO> sanPhams = new List<SanPhamDTO>(listSanPhamAll);
            ProductList = new ObservableCollection<SanPhamDTO>(sanPhams);
            prdList = new List<SanPhamDTO>(ProductList);
            if (DanhMucSelect != "Tất cả thể loại")
            {
                ProductList = new ObservableCollection<SanPhamDTO>(prdList.FindAll(x => (x.DanhMucSanPhamDTO.TenDanhMuc == DanhMucSelect)));
            }
            ChuoiSearch = "";
        }

    }
}
