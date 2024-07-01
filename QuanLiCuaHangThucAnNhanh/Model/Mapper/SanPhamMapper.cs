using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class SanPhamMapper
    {
        public static SanPhamDTO MapToDTO(SanPham sp)
        {
            if (sp == null) return null;
            return new SanPhamDTO
            {
                ID = sp.ID,
                TenSP = sp.TenSP,
                SoLuongTon = sp.SoLuongTon,
                GiaNhap = sp.GiaNhap,
                DanhMucSanPhamID = sp.DanhMucSanPhamID,
                Image = sp.Image,
                IsDeleted = sp.IsDeleted,
            };
        }
    }
}
