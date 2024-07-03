using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Model.Mapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.DA
{
    public class HoaDonNhapDA
    {
        private static HoaDonNhapDA instance;

        public static HoaDonNhapDA gI()
        {
            if (instance == null)
            {
                instance = new HoaDonNhapDA();
            }
            return instance;
        }

        public async Task<List<HoaDonNhapDTO>> GetAllHoaDonNhap()
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var billEntities = await (from c in context.HoaDonNhaps
                                              where c.IsDeleted == false
                                              select c).OrderByDescending(m => m.NgayTao).ToListAsync();

                    var billList = billEntities.Select(c => new HoaDonNhapDTO
                    {
                        ID = c.ID,
                        NguoiDungID = c.NguoiDungID,
                        NgayTao = c.NgayTao,
                        TongTienNhap = c.TongTienNhap,
                        NguoiDungDTO = NguoiDungMapper.MapToDTO(c.NguoiDung),
                        ListChiTietHoaDonNhapDTO = c.ChiTietHoaDonNhaps
                                                  .Where(x => x.IsDeleted == false)
                                                  .Select(x => new ChiTietHoaDonNhapDTO
                                                  {
                                                      HoaDonNhapID = x.HoaDonNhapID,
                                                      DonGia = x.DonGia,
                                                      SanPhamID = x.SanPhamID,
                                                      SoLuong = x.SoLuong,
                                                      HoaDonNhapDTO = HoaDonNhapMapper.MapToDTO(x.HoaDonNhap),
                                                      SanPhamDTO = SanPhamMapper.MapToDTO(x.SanPham),
                                                  }).ToList(),
                        IsDeleted = c.IsDeleted
                    }).ToList();

                    return billList;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
