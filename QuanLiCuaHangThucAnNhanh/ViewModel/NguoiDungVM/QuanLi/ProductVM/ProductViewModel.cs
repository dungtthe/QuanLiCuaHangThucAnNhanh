using MaterialDesignColors;
using Microsoft.Win32;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Utils;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.QuanLi.SanPhamManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
        public ICommand DeleteSanPhamListCM {  get; set; }
        public ICommand ExportExcel { get; set; }

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

                            //foreach (var item in listSanPhamInsert)
                            //{
                            //    await SanPhamDA.gI().AddNewProduct(item);
                            //}

                            //foreach (var item in listSanPhamUpdate)
                            //{
                            //    await SanPhamDA.gI().UpdateProductQuantity(item);
                            //}

                            bool flag = await SanPhamDA.gI().AddNewProductsForNhapKho(listSanPhamInsert,MainNguoiDungVM.nguoiDungDTOCur.ID);
                            flag = await SanPhamDA.gI().UpdateProductQuantitiesForNhapKho(listSanPhamUpdate, MainNguoiDungVM.nguoiDungDTOCur.ID);



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

            #region xóa sản phẩm
            DeleteSanPhamListCM = new RelayCommand<SanPhamDTO>((p) => { return true; }, async (p) =>
            {
                SelectedItem = p;
                SanPhamDTO a = new SanPhamDTO();
                a = SelectedItem;

                Warning wd = new Warning();
                wd.ShowDialog();
                if (wd.DialogResult == true)
                {
                    //string deleteImg = SelectedItem.Image;
                    //await CloudService.Ins.DeleteImage(deleteImg);
                    (bool sucess, string messageDelete) = await SanPhamDA.Ins.DeletePrD(SelectedItem.ID);
                    if (sucess)
                    {
                        ProductList.Remove(SelectedItem);
                        prdList = new List<SanPhamDTO>(ProductList);
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã xóa thành công");
                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, messageDelete);
                    }
                }
            });
            #endregion
            ExportExcel = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                string path = MotSoPhuongThucBoTro.SelectFolder();
                if(path == null)
                {
                    return;
                }
                else
                {
                    string selectedFolderPath = path;
                    // tạo excel
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (ExcelPackage package = new ExcelPackage())
                    {
                        // Tạo một worksheet mới
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Product");

                        // Merge hàng từ hàng 1,2 từ cột A -> L
                        var mergedCells = worksheet.Cells["A1:E2"];
                        mergedCells.Merge = true;
                        //set text 
                        mergedCells.Value = "Danh sách sản phẩm";
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
                        string[] columnHeaders = { "STT", "Tên sản phẩm", "Số lượng tồn", "Giá nhập", "Tên danh mục" };
                        for (int i = 0; i < columnHeaders.Length; i++)
                        {
                            worksheet.Cells[3, i + 1].Value = columnHeaders[i];
                            worksheet.Cells[3, i + 1].Style.Font.Size = 14; // Thiết lập cỡ chữ 14
                            worksheet.Cells[3, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[3, i + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightSkyBlue);
                        }


                        // Ghi dữ liệu từ danh sách vào worksheet
                        for (int i = 0; i < ProductList.Count; i++)
                        {
                            var product = ProductList[i];
                            worksheet.Cells[i + 4, 1].Value = i + 1;
                            worksheet.Cells[i + 4, 2].Value = product.TenSP;
                            worksheet.Cells[i + 4, 3].Value = product.SoLuongTon;
                            worksheet.Cells[i + 4, 4].Value = product.GiaNhap;
                            worksheet.Cells[i + 4, 5].Value = product.DanhMucSanPhamDTO.TenDanhMuc;

                            //can trai cho all noi dung cua cot 
                            for (int j = 1; j <= 5; j++)
                            {
                                worksheet.Cells[i + 4, j].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                            }
                        }

                        worksheet.Cells.AutoFitColumns();

                        DateTime date = DateTime.Now;
                        string filePath = Path.Combine(selectedFolderPath, "ListProduct.xlsx");
                        package.SaveAs(new FileInfo(filePath));
                        Success wd = new Success($"Tệp Excel đã được lưu vào: {filePath}");
                        wd.ShowDialog();
                    }
                    /*string selectedFolderPath = path;


                    string fileExcelName = "";//tên file export
                    string pathFile = "";//đường dẫn tuyệt đối của file export

                    bool check = false;
                    while (true)
                    {
                        fileExcelName = "ListProduct_" + ".xlsx";
                        pathFile = selectedFolderPath + @"\" + fileExcelName;
                        if (!File.Exists(pathFile))
                        {
                            check = true;
                            break;
                        }
                    }

                    if (check)
                    {
                        DataTable table = new DataTable();
                        table.Columns.Add("STT", typeof(string));
                        table.Columns.Add("Tên sản phẩm", typeof(string));
                        table.Columns.Add("Số lượng tồn", typeof(string));
                        table.Columns.Add("Giá nhập", typeof(string));
                        table.Columns.Add("Tên danh mục", typeof(string));


                        int stt = 1;
                        foreach (var item in listSanPhamAll)
                        {
                            if (item.TenSP != null && item.SoLuongTon.ToString() != null && item.GiaNhap.ToString() != null && item.DanhMucSanPhamDTO.TenDanhMuc != null)
                            {
                                table.Rows.Add(stt.ToString(), item.TenSP, item.SoLuongTon.ToString(), item.GiaNhap.ToString(), item.DanhMucSanPhamDTO.TenDanhMuc);
                                stt++;
                            }
                        }


                        try
                        {
                            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                            using (var package = new ExcelPackage())
                            {
                                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                                // Thêm tiêu đề và tháng
                                worksheet.Cells["A1:E1"].Merge = true;
                                worksheet.Cells["A1:E1"].Value = "Danh sách sản phẩm";
                                worksheet.Cells["A1:E1"].Style.Font.Size = 16;
                                worksheet.Cells["A1:E1"].Style.Font.Bold = true;
                                worksheet.Cells["A1:E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                worksheet.Cells["A2:E2"].Merge = true;
                                worksheet.Cells["A2:E2"].Value = "Tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                                worksheet.Cells["A2:E2"].Style.Font.Size = 12;
                                worksheet.Cells["A2:E2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                                // Load dữ liệu từ DataTable vào Excel, bắt đầu từ dòng thứ 4
                                worksheet.Cells["A4"].LoadFromDataTable(table, true);

                                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                                File.WriteAllBytes(pathFile, package.GetAsByteArray());

                                MessageBoxCustom.Show(MessageBoxCustom.Success, "Thành công!");

                                checkThaoTac = true;
                            }
                        }
                        catch
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                            checkThaoTac = true;

                        }

                    }
                    else
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                        checkThaoTac = true;

                    }*/
                }
                //// Hiển thị hộp thoại chọn thư mục và lấy kết quả
                //System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                //if (result == System.Windows.Forms.DialogResult.OK)
                //{
                //    string selectedFolderPath = dialog.SelectedPath;


                //    string fileExcelName = "";//tên file export
                //    string pathFile = "";//đường dẫn tuyệt đối của file export

                //    bool check = false;
                //    while (true)
                //    {
                //        fileExcelName = "ListProduct_"+".xlsx";
                //        pathFile = selectedFolderPath + @"\" + fileExcelName;
                //        if (!File.Exists(pathFile))
                //        {
                //            check = true;
                //            break;
                //        }
                //    }

                //    if (check)
                //    {
                //        DataTable table = new DataTable();
                //        table.Columns.Add("STT", typeof(string));
                //        table.Columns.Add("Tên sản phẩm", typeof(string));
                //        table.Columns.Add("Số lượng tồn", typeof(string));
                //        table.Columns.Add("Giá nhập", typeof(string));
                //        table.Columns.Add("Tên danh mục", typeof(string));


                //        int stt = 1;
                //        foreach (var item in listSanPhamAll)
                //        {
                //            if (item.TenSP != null && item.SoLuongTon.ToString()!=null && item.GiaNhap.ToString() != null && item.DanhMucSanPhamDTO.TenDanhMuc != null)
                //            {
                //                table.Rows.Add(stt.ToString(), item.TenSP, item.SoLuongTon.ToString(), item.GiaNhap.ToString(), item.DanhMucSanPhamDTO.TenDanhMuc);
                //                stt++;
                //            }
                //        }


                //        try
                //        {
                //            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                //            using (var package = new ExcelPackage())
                //            {
                //                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                //                // Thêm tiêu đề và tháng
                //                worksheet.Cells["A1:E1"].Merge = true;
                //                worksheet.Cells["A1:E1"].Value = "Danh sách sản phẩm";
                //                worksheet.Cells["A1:E1"].Style.Font.Size = 16;
                //                worksheet.Cells["A1:E1"].Style.Font.Bold = true;
                //                worksheet.Cells["A1:E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //                worksheet.Cells["A2:E2"].Merge = true;
                //                worksheet.Cells["A2:E2"].Value = "Tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year;
                //                worksheet.Cells["A2:E2"].Style.Font.Size = 12;
                //                worksheet.Cells["A2:E2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                //                // Load dữ liệu từ DataTable vào Excel, bắt đầu từ dòng thứ 4
                //                worksheet.Cells["A4"].LoadFromDataTable(table, true);

                //                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                //                File.WriteAllBytes(pathFile, package.GetAsByteArray());

                //                MessageBoxCustom.Show(MessageBoxCustom.Success, "Thành công!");

                //                checkThaoTac = true;
                //            }
                //        }
                //        catch
                //        {
                //            MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                //            checkThaoTac = true;

                //        }

                //    }
                //    else
                //    {
                //        MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                //        checkThaoTac = true;

                //    }
                //}
            });
        }

        bool checkThaoTac = false;
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
