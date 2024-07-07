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

        public async Task<(bool, string)> DeleteGenre(DanhMucSanPham selectedGenre)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var genreToDelete = await context.DanhMucSanPhams.FirstOrDefaultAsync(g => g.ID == selectedGenre.ID && g.TenDanhMuc == selectedGenre.TenDanhMuc);
                    if (genreToDelete == null)
                    {
                        return (false, "Không tìm thấy thể danh mục để xóa.");
                    }

                    // Perform soft delete (mark as deleted)
                    genreToDelete.IsDeleted = true;

                    await context.SaveChangesAsync();
                    return (true, "Xóa thể danh mục thành công.");
                }
            }
            catch (Exception)
            {
                return (false, "Xảy ra lỗi khi xóa thể danh mục.");
            }
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

        public async Task<List<string>> GetAllGenreDanhMucc()
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var productGenreList = await (from c in context.DanhMucSanPhams
                                                  where c.IsDeleted != true
                                                  select c.TenDanhMuc).ToListAsync();
                    return productGenreList;
                }
            }
            catch (Exception ex)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return null;
            }
        }
        public async Task<bool> AddNewDanhMuc(DanhMucSanPham danhMucSanPham)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    context.DanhMucSanPhams.Add(danhMucSanPham);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        public async Task<(int, DanhMucSanPham)> FindGenrePrD(string name)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var prD = await context.DanhMucSanPhams.Where(p => p.TenDanhMuc == name && p.IsDeleted != true).FirstOrDefaultAsync();
                    if (prD == null)
                    {
                        return (-1, null);
                    }
                    return ((int)prD.ID, prD);
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (-1, null);
            }

        }
        public async Task<(bool, string)> EditGenre(DanhMucSanPham selectedGenre)
        {

            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    bool IsExistID = await context.DanhMucSanPhams.AnyAsync(p => p.TenDanhMuc == selectedGenre.TenDanhMuc && p.ID == selectedGenre.ID && p.IsDeleted != true);

                    if (IsExistID)
                    {
                        return (false, "Danh mục đã tồn tại.");
                    }
                    var genre = await context.DanhMucSanPhams.Where(p => p.ID == selectedGenre.ID).FirstOrDefaultAsync();
                    genre.TenDanhMuc = selectedGenre.TenDanhMuc;
                    await context.SaveChangesAsync();
                    return (true, "Cập nhật thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi!");
                return (false, null);
            }
        }
    }
}
