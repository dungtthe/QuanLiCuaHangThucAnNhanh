using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class HoaDonNhapMapper
    {
        public static HoaDonNhapDTO MapToDTO(HoaDonNhap hdn)
        {
            if (hdn == null) return null;

            return new HoaDonNhapDTO
            {
                ID = hdn.ID,
                NgayTao = hdn.NgayTao,
                TongTienNhap = hdn.TongTienNhap,
                NguoiDungID = hdn.NguoiDungID,
                IsDeleted = hdn.IsDeleted,
            };
        }
    }
}
