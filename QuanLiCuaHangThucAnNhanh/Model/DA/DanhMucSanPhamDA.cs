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
    public class DanhMucSanPhamDA
    {
        private static DanhMucSanPhamDA instance;

        public static DanhMucSanPhamDA gI()
        {
            if (instance == null)
            {
                instance = new DanhMucSanPhamDA();
            }
            return instance;
        }


        public async Task<List<DanhMucSanPhamDTO>> GetAllDanhMucSanPham()
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var danhMucList = (from c in context.DanhMucSanPhams
                                       where c.IsDeleted == false
                                       select new DanhMucSanPhamDTO
                                       {
                                           ID = c.ID,
                                           TenDanhMuc = c.TenDanhMuc,
                                           IsDeleted = c.IsDeleted,
                                       }).ToListAsync();
                    return await danhMucList;
                }
            }
            catch 
            {
                return null;
            }
        }

        public async Task<List<string>> GetAllTenDanhMucSanPham()
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var danhMucList = await (from c in context.DanhMucSanPhams
                                             where c.IsDeleted != true
                                             select c.TenDanhMuc).ToListAsync();
                    return danhMucList;
                }
            }
            catch
            {
                return null;
            }
        }


    }
}
