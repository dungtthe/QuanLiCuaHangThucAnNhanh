using QuanLiCuaHangThucAnNhanh.Model;
using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.QuanLi.ProductVM
{
    public partial class ProductViewModel 
    {
        private int GetIdDanhMucSanPhamByTenDanhMuc(string tendanhmuc,List<DanhMucSanPhamDTO> list)
        {
            foreach(var danhMuc in list)
            {
                if (tendanhmuc == danhMuc.TenDanhMuc)
                {
                    return danhMuc.ID;
                }
            }
            return -1;
        }


        private int IsSanPhamOld(SanPham sanPham)
        {
            foreach(var item in listSanPhamAll)
            {
                if (item.TenSP == sanPham.TenSP)
                {
                    if (ConvertDecimalToInt(item.GiaNhap) == ConvertDecimalToInt(sanPham.GiaNhap))
                    {
                        if (item.DanhMucSanPhamID == sanPham.DanhMucSanPhamID)
                        {
                            return item.ID;
                        }
                    }
                }
            }
            return -1;
        }


        private int ConvertDecimalToInt(decimal temp)
        {
            string s=temp.ToString();
            if (!s.Contains("."))
            {
                return (int)temp;
            }

            return int.Parse(s.Split('.')[0]);
        }

        private byte[] GetImage(int idProduct)
        {
            foreach(var item in listSanPhamAll)
            {
                if (item.ID == idProduct)
                {
                    return item.Image;
                }
            }
            return null;
        }
    }
}
