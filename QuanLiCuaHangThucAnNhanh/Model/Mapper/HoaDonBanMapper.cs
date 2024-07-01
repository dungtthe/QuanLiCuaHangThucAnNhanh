using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class HoaDonBanMapper
    {
        public static HoaDonBanDTO MapToDTO(HoaDonBan hdb)
        {
            if (hdb == null) return null;

            return new HoaDonBanDTO
            {
                ID = hdb.ID,
                NgayTao = hdb.NgayTao,
                TongTienBan = hdb.TongTienBan,
                NguoiDungID = hdb.NguoiDungID,
                KhachHangID = hdb.KhachHangID,
                IsDeleted = hdb.IsDeleted,
            };
        }
    }
}
