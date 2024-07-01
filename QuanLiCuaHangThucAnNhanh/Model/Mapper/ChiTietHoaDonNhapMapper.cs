using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class ChiTietHoaDonNhapMapper
    {
        public static ChiTietHoaDonNhapDTO MapToDTO(ChiTietHoaDonNhap cthdn)
        {
            if (cthdn == null) return null;
            return new ChiTietHoaDonNhapDTO
            {
                HoaDonNhapID = cthdn.HoaDonNhapID,
                SanPhamID = cthdn.SanPhamID,
                SoLuong = cthdn.SoLuong,
                DonGia = cthdn.DonGia,
                IsDeleted = cthdn.IsDeleted,
                HoaDonNhapDTO = HoaDonNhapMapper.MapToDTO(cthdn.HoaDonNhap),
                SanPhamDTO = SanPhamMapper.MapToDTO(cthdn.SanPham)
            };
        }
    }
}
