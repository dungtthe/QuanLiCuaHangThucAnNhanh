using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class DanhMucSanPhamMapper
    {
        public static DanhMucSanPhamDTO MapToDTO(DanhMucSanPham dmsp)
        {
            if (dmsp == null) return null;
            return new DanhMucSanPhamDTO
            {
                ID = dmsp.ID,
                TenDanhMuc = dmsp.TenDanhMuc,
                IsDeleted = dmsp.IsDeleted
            };
        }
    }
}
