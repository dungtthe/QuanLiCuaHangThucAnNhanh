using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using QuanLiCuaHangThucAnNhanh.View.NguoiDung.QuanLi.HeThong;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.QuanLi.HeThongVM
{
    public partial class HeThongVM : BaseViewModel
    {

        private string tenDanhMucNew;
        public string TenDanhMucNew
        {
            get => tenDanhMucNew;
            set
            {
                tenDanhMucNew = value;
                OnPropertyChanged(TenDanhMucNew);
            }
        }
        private DanhMucSanPhamDTO selectedItem;
        public DanhMucSanPhamDTO SelectedItem
        {
            get { return selectedItem; }
            set { selectedItem = value; OnPropertyChanged(); }
        }
        private List<string> genreBookList; //Instance of database
        public List<string> GenreBookList
        {
            get { return genreBookList; }
            set { genreBookList = value; OnPropertyChanged(); }
        }
        private string editDisplayName;
        public string EditDisplayName
        {
            get { return editDisplayName; }
            set { editDisplayName = value; OnPropertyChanged(); }
        }
        private ObservableCollection<DanhMucSanPhamDTO> genreBookObservation; //ListView source
        public ObservableCollection<DanhMucSanPhamDTO> GenreBookObservation
        {
            get { return genreBookObservation; }
            set
            {
                genreBookObservation = value; OnPropertyChanged();
               
            }
        }
        private ThamSoDTO thamSoDTO;
        public ThamSoDTO ThamSoDTO
        {
            get => thamSoDTO;
            set
            {
                thamSoDTO = value;
                OnPropertyChanged(nameof(ThamSoDTO));
            }
        }
        private string editOnePointToMoney;
        public string EditOnePointToMoney
        {
            get { return editOnePointToMoney; }
            set { editOnePointToMoney = value; OnPropertyChanged(); }
        }

        private string editHeSoBan;
        public string EditHeSoBan
        {
            get { return editHeSoBan; }
            set { editHeSoBan = value; OnPropertyChanged(); }
        }

        private string editMoneyOneToPoint;
        public string EditMoneyOneToPoint
        {
            get { return editMoneyOneToPoint; }
            set { editMoneyOneToPoint = value; OnPropertyChanged(); }
        }
        public ICommand SaveCMD { get; set; }
        public ICommand FirstLoadCMD { get; set; }
        public ICommand OpenAddWindowCM { get; set; }
        public ICommand CloseAddWindowCM { get; set; }
        public ICommand AcceptAddDanhMucCM { get; set; }
        public ICommand EditDanhMucCMD { get; set; }
        public ICommand OpenEditWidowCMD { get; set; }
        public ICommand CloseEditWindowCMD { get; set; }
        public ICommand RemoveDanhMucCMD { get; set; }
        public HeThongVM()
        {
            FirstLoadCMD = new RelayCommand<object>(null, async (p) =>
            {
                genreBookList = new List<string>(await DanhMucSanPhamDA.gI().GetAllGenreBook());
                ThamSoDTO = await ThamSoDA.gI().GetThamSoCur();
                if (ThamSoDTO == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }
                EditHeSoBan = ThamSoDTO.HeSoBan.ToString();
                EditOnePointToMoney = ThamSoDTO.OnePointToMoney.ToString();
                EditMoneyOneToPoint = ThamSoDTO.MoneyToOnePoint.ToString();
                ComboList = new ObservableCollection<DanhMucSanPhamDTO>(await DanhMucSanPhamDA.gI().GetAllDanhMucSanPham());

            });

            OpenAddWindowCM = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {

                AddDanhMuc addDanhMuc = new AddDanhMuc();
                addDanhMuc.ShowDialog();
            });
            SaveCMD = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {   
                
                if (EditHeSoBan == null || EditOnePointToMoney == null
                || EditMoneyOneToPoint == null || EditHeSoBan == "" || EditOnePointToMoney == ""
                || EditMoneyOneToPoint == "")
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                }
                else
                {
                    if(Double.Parse(EditHeSoBan) == thamSoDTO.HeSoBan && decimal.Parse(EditOnePointToMoney) == thamSoDTO.OnePointToMoney && Int32.Parse(EditMoneyOneToPoint) == thamSoDTO.MoneyToOnePoint)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Không có gì mới để chỉnh sửa");
                        return;
                    }
                    if(CheckValidation(EditHeSoBan)==false|| CheckValidation(EditOnePointToMoney) == false || CheckValidation(EditMoneyOneToPoint) == false)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Hệ số bán,điểm quy đổi sang tiền và ngược lại không được là chuỗi");
                        return;
                    }
                    if (Double.Parse(EditHeSoBan) < 1)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Hệ số bán không được bé hơn 1");
                        return;
                    }
                    if(Int32.Parse(EditMoneyOneToPoint) <= 0 || decimal.Parse(EditOnePointToMoney) <= 0)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, "Điểm quy đổi sang tiền và ngược lại bắt buộc phải lớn hơn 0");
                        return;
                    }
                    THAMSO tHAMSO = new THAMSO
                    (Double.Parse(EditHeSoBan), decimal.Parse(EditOnePointToMoney), Int32.Parse(EditMoneyOneToPoint));
                    ThamSoDA.gI().EditThamSo(tHAMSO);
                    MessageBoxCustom.Show(MessageBoxCustom.Success, "Đã cập nhật thành công!");

                }
            });

            CloseAddWindowCM = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                if (p == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }
                p.Close();
            });

            EditDanhMucCMD = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (EditDisplayName == null || EditDisplayName == "")
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Bạn đang nhập thiếu hoặc sai thông tin");
                }
                else
                {
                    if (EditDisplayName == SelectedItem.TenDanhMuc)
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Success, "Không có gì mới để chỉnh sửa");
                        p.Close();
                        return;
                    }
                    else
                    {
                        DanhMucSanPham newGenre = new DanhMucSanPham
                        {
                            ID = SelectedItem.ID,
                            TenDanhMuc = EditDisplayName,
                            IsDeleted = false
                        };

                        (bool success, string messageEdit) = await DanhMucSanPhamDA.gI().EditGenre(newGenre);
                        if (success)
                        {
                            //genreBookObservation == "need to be modified with the info of newGenre"
                            MessageBoxCustom.Show(MessageBoxCustom.Success, "Bạn đã chỉnh sửa thành công");
                            genreBookList = new List<string>(await DanhMucSanPhamDA.gI().GetAllGenreBook());
                            // Create the observation collection of DTOs
                            GenreBookObservation = new ObservableCollection<DanhMucSanPhamDTO>();

                            var bookList = new List<SanPhamDTO>(await SanPhamDA.gI().GetAllSanPham());

                            for (int i = 0; i < genreBookList.Count; i++)
                            {
                                // Create a new DTO instance
                                (int _, DanhMucSanPham currentGenre) = await DanhMucSanPhamDA.gI().FindGenrePrD(genreBookList[i]);

                                var genreDTO = new DanhMucSanPhamDTO
                                {

                                    ID = (int)currentGenre.ID,
                                    TenDanhMuc = currentGenre.TenDanhMuc,
                                    IsDeleted = false
                                };

                                // Add the DTO to the observation collection
                                GenreBookObservation.Add(genreDTO);
                            }
                            p.Close();
                        }
                        else
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, messageEdit);
                        }
                    }
                }
            });

            OpenEditWidowCMD = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {
                EditDisplayName = SelectedItem.TenDanhMuc;
                EditDanhMuc editDanhMuc = new EditDanhMuc();
                editDanhMuc.ShowDialog();
            });
            CloseEditWindowCMD = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                p.Close();
            });


            AcceptAddDanhMucCM = new RelayCommand<Window>((p) => { return true; }, async (p) =>
            {
                if (p == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }


                foreach (var item in ComboList)
                {
                    if (item.TenDanhMuc.ToLower() == TenDanhMucNew.ToLower())
                    {
                        MessageBoxCustom.Show(MessageBoxCustom.Error, $"Đã tồn tại danh mục {TenDanhMucNew}!");
                        return;
                    }
                }

                DanhMucSanPham danhMucSanPham = new DanhMucSanPham()
                {
                    TenDanhMuc = TenDanhMucNew,
                    IsDeleted = false,

                };

                bool check = await DanhMucSanPhamDA.gI().AddNewDanhMuc(danhMucSanPham);

                if (!check)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }
                ComboList = new ObservableCollection<DanhMucSanPhamDTO>(await DanhMucSanPhamDA.gI().GetAllDanhMucSanPham());
                MessageBoxCustom.Show(MessageBoxCustom.Success, "Thêm danh mục thành công!");
                p.Close();

            });

            RemoveDanhMucCMD = new RelayCommand<DanhMucSanPhamDTO>((p) => { return true; }, async (p) =>
            {
                Warning wd = new Warning("Bạn có muốn xóa danh mục này?");
                wd.ShowDialog();

                try
                {
                    if (wd.DialogResult == true)
                    {
                        genreBookList.Remove(SelectedItem.TenDanhMuc);
                        //if (SelectedItem.Quantity > 0)
                        //{
                        //    throw new Exception("Không thể xóa vì tồn tại sản phẩm thuộc danh mục");
                        //}
                        var newGenre = new DanhMucSanPham
                        {
                            ID = SelectedItem.ID,
                            TenDanhMuc = SelectedItem.TenDanhMuc,
                            IsDeleted = true
                        };
                        (bool success, string messageEdit) = await DanhMucSanPhamDA.gI().DeleteGenre(newGenre);
                        if (success)
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Success, "Đã xóa thành công!");
                            genreBookList = new List<string>(await DanhMucSanPhamDA.gI().GetAllGenreBook());
                            // Create the observation collection of DTOs
                            GenreBookObservation = new ObservableCollection<DanhMucSanPhamDTO>();

                            var bookList = new List<SanPhamDTO>(await SanPhamDA.gI().GetAllSanPham());

                            for (int i = 0; i < genreBookList.Count; i++)
                            {
                                // Create a new DTO instance
                                (int _, DanhMucSanPham currentGenre) = await DanhMucSanPhamDA.gI().FindGenrePrD(genreBookList[i]);

                                var genreDTO = new DanhMucSanPhamDTO
                                {
                                    ID = (int)currentGenre.ID,
                                    TenDanhMuc = currentGenre.TenDanhMuc,
                                };

                                // Add the DTO to the observation collection
                                GenreBookObservation.Add(genreDTO);
                            }

                        }
                        else
                        {
                            MessageBoxCustom.Show(MessageBoxCustom.Error, messageEdit);
                        }

                    }
                }
                catch (Exception ex)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, ex.Message);
                }
            });

        }

        public static bool CheckValidation(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if ((s[i] < '0' || s[i] > '9') && s[i]!= '.')
                {
                    return false;
                }
            }
            return true;
        }
    }
}
