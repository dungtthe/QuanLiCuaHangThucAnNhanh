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

        public ICommand FirstLoadCMD { get; set; }
        public ICommand OpenAddWindowCM { get; set; }
        public ICommand CloseAddWindowCM { get; set; }
        public ICommand AcceptAddDanhMucCM { get; set; }

        public HeThongVM()
        {
            FirstLoadCMD = new RelayCommand<object>(null, async (p) =>
            {
                ThamSoDTO = await ThamSoDA.gI().GetThamSoCur();
                if (ThamSoDTO == null)
                {
                    MessageBoxCustom.Show(MessageBoxCustom.Error, "Có lỗi xảy ra!");
                    return;
                }

                ComboList = new ObservableCollection<DanhMucSanPhamDTO>(await DanhMucSanPhamDA.gI().GetAllDanhMucSanPham());

            });

            OpenAddWindowCM = new RelayCommand<Page>((p) => { return true; }, (p) =>
            {

                AddDanhMuc addDanhMuc = new AddDanhMuc();
                addDanhMuc.ShowDialog();
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

        }

    }
}
