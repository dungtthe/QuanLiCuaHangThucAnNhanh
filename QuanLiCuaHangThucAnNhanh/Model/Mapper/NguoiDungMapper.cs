using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class NguoiDungMapper
    {
        public static NguoiDungDTO MapToDTO(NguoiDung nd)
        {
            if (nd == null) return null;
            return new NguoiDungDTO
            {
                ID = nd.ID,
                HoTen = nd.HoTen,
                NgaySinh = nd.NgaySinh,
                SoDienThoai = nd.SoDienThoai,
                Email = nd.Email,
                DiaChi = nd.DiaChi,
                TenTaiKhoan = nd.TenTaiKhoan,
                MatKhau = nd.MatKhau,
                Loai = nd.Loai,
                Image = nd.Image,
                IsDeleted = nd.IsDeleted
            };
        }
    }
}
