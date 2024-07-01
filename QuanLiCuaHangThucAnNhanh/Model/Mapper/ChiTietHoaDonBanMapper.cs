using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class ChiTietHoaDonBanMapper
    {
        public static ChiTietHoaDonBanDTO MapToDTO(ChiTietHoaDonBan cthdb)
        {
            if (cthdb == null) return null;
            return new ChiTietHoaDonBanDTO
            {
                HoaDonBanID = cthdb.HoaDonBanID,
                SanPhamID = cthdb.SanPhamID,
                SoLuong = cthdb.SoLuong,
                DonGia = cthdb.DonGia,
                IsDeleted = cthdb.IsDeleted,
            };
        }
    }
}
