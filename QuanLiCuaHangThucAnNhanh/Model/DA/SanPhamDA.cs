using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Model.Mapper;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.DA
{
    public class SanPhamDA
    {
        private static SanPhamDA instance;

        public static SanPhamDA gI()
        {
            if (instance == null)
            {
                instance = new SanPhamDA();
            }
            return instance;
        }


        public async Task<List<SanPhamDTO>> GetAllSanPham()
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var productList = await (from c in context.SanPhams
                                             where c.IsDeleted == false
                                             select new
                                             {
                                                 c.ID,
                                                 c.TenSP,
                                                 c.GiaNhap,
                                                 c.SoLuongTon,
                                                 c.DanhMucSanPham,
                                                 c.Image,
                                                 c.IsDeleted,
                                             }).ToListAsync();

                    var productDTOList = productList.Select(c => new SanPhamDTO
                    {
                        ID = c.ID,
                        TenSP = c.TenSP,
                        GiaNhap = c.GiaNhap,
                        SoLuongTon = c.SoLuongTon,
                        DanhMucSanPhamID = c.DanhMucSanPham.ID,
                        DanhMucSanPhamDTO = DanhMucSanPhamMapper.MapToDTO(c.DanhMucSanPham),
                        Image = c.Image,
                        IsDeleted = c.IsDeleted,
                    }).ToList();

                    return productDTOList;
                }
            }
            catch
            {
                return null;
            }
        }

    }
}
