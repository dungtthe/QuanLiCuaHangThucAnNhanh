using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class KhachHangMapper
    {
        public static KhachHangDTO MapToDTO(KhachHang kh)
        {
            if (kh == null) return null;
            return new KhachHangDTO
            {
                ID = kh.ID,
                HoTen = kh.HoTen,
                NgaySinh = kh.NgaySinh,
                SoDienThoai = kh.SoDienThoai,
                Email = kh.Email,
                DiaChi = kh.DiaChi,
                DiemTichLuy = kh.DiemTichLuy,
                IsDelete = kh.IsDeleted
            };
        }
    }
}
