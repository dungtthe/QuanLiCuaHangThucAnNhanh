using QuanLiCuaHangThucAnNhanh.Model.DA;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.QuanLi.ProductVM
{
    public partial class ProductViewModel
    {
        void CloseEditWindow(Window p)
        {
            if (p == null) return;
            var task = Task.Run(async () => {
                try
                {
                    List<SanPhamDTO> sanPhams = await SanPhamDA.gI().GetAllSanPham();

                    Application.Current.Dispatcher.Invoke(() => {
                        listSanPhamAll = new List<SanPhamDTO>(sanPhams);
                    });

                    SetGia(sanPhams);

                    Application.Current.Dispatcher.Invoke(() => {
                        ProductList = new ObservableCollection<SanPhamDTO>(sanPhams);
                        if (ProductList != null)
                        {
                            prdList = new List<SanPhamDTO>(ProductList);
                        }
                    });

                    Application.Current.Dispatcher.Invoke(() => {
                        if (!ComboList.Contains("Tất cả thể loại"))
                        {
                            ComboList.Insert(0, "Tất cả thể loại");
                        }
                        DanhMucSelect = "Tất cả thể loại";
                        UpdateCb();
                    });
                }
                catch
                {
                }
            });

            p.Close();

        }
    }
}
