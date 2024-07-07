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
        private static SanPhamDA _ins;

        public static SanPhamDA Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new SanPhamDA();
                }
                return _ins;
            }
            private set { _ins = value; }
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


        public async Task<bool> AddNewProduct(SanPham sanPham)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    context.SanPhams.Add(sanPham);
                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }


        public async Task<bool> UpdateProductQuantity(SanPham sanPhamNew)
        {
            try
            {
                int productId = sanPhamNew.ID;
                int quantityAdd = sanPhamNew.SoLuongTon;

                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var sanPham = await context.SanPhams.FindAsync(productId);

                    if (sanPham == null)
                    {
                        return false;
                    }

                    sanPham.SoLuongTon = quantityAdd + sanPham.SoLuongTon;

                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }







        public async Task<bool> AddNewProductsForNhapKho(List<SanPham> listSanPham, int nguoiDungId)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            decimal tongTienNhap = listSanPham.Sum(sp => sp.GiaNhap * sp.SoLuongTon);

                            var hoaDonNhap = new HoaDonNhap
                            {
                                NguoiDungID = nguoiDungId,
                                TongTienNhap = tongTienNhap,
                                NgayTao = DateTime.Now,
                                IsDeleted = false
                            };
                            context.HoaDonNhaps.Add(hoaDonNhap);
                            await context.SaveChangesAsync();

                            foreach (var sanPham in listSanPham)
                            {
                                context.SanPhams.Add(sanPham);
                                await context.SaveChangesAsync();

                                var chiTietHoaDonNhap = new ChiTietHoaDonNhap
                                {
                                    HoaDonNhapID = hoaDonNhap.ID,
                                    SanPhamID = sanPham.ID,
                                    SoLuong = sanPham.SoLuongTon,
                                    DonGia = sanPham.GiaNhap,
                                    IsDeleted = false
                                };
                                context.ChiTietHoaDonNhaps.Add(chiTietHoaDonNhap);
                            }

                            await context.SaveChangesAsync();
                            transaction.Commit();

                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> UpdateProductQuantitiesForNhapKho(List<SanPham> listSanPham, int nguoiDungId)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            decimal tongTienNhap = 0;

                            foreach (var sanPhamNew in listSanPham)
                            {
                                var sanPham = await context.SanPhams.FindAsync(sanPhamNew.ID);
                                if (sanPham == null)
                                {
                                    continue;
                                }

                                int soLuongAdd = sanPhamNew.SoLuongTon;
                                sanPham.SoLuongTon += soLuongAdd;

                                tongTienNhap += sanPham.GiaNhap * soLuongAdd;

                                await context.SaveChangesAsync();
                            }

                            if (tongTienNhap > 0)
                            {
                                var hoaDonNhap = new HoaDonNhap
                                {
                                    NguoiDungID = nguoiDungId,
                                    TongTienNhap = tongTienNhap,
                                    NgayTao = DateTime.Now,
                                    IsDeleted = false
                                };
                                context.HoaDonNhaps.Add(hoaDonNhap);
                                await context.SaveChangesAsync();

                                foreach (var sanPhamNew in listSanPham)
                                {
                                    var sanPham = await context.SanPhams.FindAsync(sanPhamNew.ID);
                                    if (sanPham == null)
                                    {
                                        continue;
                                    }

                                    var chiTietHoaDonNhap = new ChiTietHoaDonNhap
                                    {
                                        HoaDonNhapID = hoaDonNhap.ID,
                                        SanPhamID = sanPham.ID,
                                        SoLuong = sanPhamNew.SoLuongTon,
                                        DonGia = sanPham.GiaNhap,
                                        IsDeleted = false
                                    };
                                    context.ChiTietHoaDonNhaps.Add(chiTietHoaDonNhap);
                                }

                                await context.SaveChangesAsync();
                            }

                            transaction.Commit();

                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }


        public async Task<bool> HandleProductImport(List<SanPham> listSanPhamInsert, List<SanPham> listSanPhamUpdate, int nguoiDungId)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    using (var transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            decimal tongTienNhap = 0;

                            foreach (var sanPham in listSanPhamInsert)
                            {
                                context.SanPhams.Add(sanPham);
                                tongTienNhap += sanPham.GiaNhap * sanPham.SoLuongTon;
                            }
                            await context.SaveChangesAsync();

                            foreach (var sanPhamNew in listSanPhamUpdate)
                            {
                                var sanPham = await context.SanPhams.FindAsync(sanPhamNew.ID);
                                if (sanPham == null)
                                {
                                    continue;
                                }

                                int soLuongAdd = sanPhamNew.SoLuongTon;
                                sanPham.SoLuongTon += soLuongAdd;

                                tongTienNhap += sanPham.GiaNhap * soLuongAdd;

                                await context.SaveChangesAsync();
                            }

                            if (tongTienNhap > 0)
                            {
                                var hoaDonNhap = new HoaDonNhap
                                {
                                    NguoiDungID = nguoiDungId,
                                    TongTienNhap = tongTienNhap,
                                    NgayTao = DateTime.Now,
                                    IsDeleted = false
                                };
                                context.HoaDonNhaps.Add(hoaDonNhap);
                                await context.SaveChangesAsync();

                                foreach (var sanPham in listSanPhamInsert)
                                {
                                    var chiTietHoaDonNhap = new ChiTietHoaDonNhap
                                    {
                                        HoaDonNhapID = hoaDonNhap.ID,
                                        SanPhamID = sanPham.ID,
                                        SoLuong = sanPham.SoLuongTon,
                                        DonGia = sanPham.GiaNhap,
                                        IsDeleted = false
                                    };
                                    context.ChiTietHoaDonNhaps.Add(chiTietHoaDonNhap);
                                }

                                foreach (var sanPhamNew in listSanPhamUpdate)
                                {
                                    var sanPham = await context.SanPhams.FindAsync(sanPhamNew.ID);
                                    if (sanPham == null)
                                    {
                                        continue;
                                    }

                                    var chiTietHoaDonNhap = new ChiTietHoaDonNhap
                                    {
                                        HoaDonNhapID = hoaDonNhap.ID,
                                        SanPhamID = sanPham.ID,
                                        SoLuong = sanPhamNew.SoLuongTon,
                                        DonGia = sanPham.GiaNhap,
                                        IsDeleted = false
                                    };
                                    context.ChiTietHoaDonNhaps.Add(chiTietHoaDonNhap);
                                }

                                await context.SaveChangesAsync();
                            }

                            transaction.Commit();
                            return true;
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }





        //Delete product
        public async Task<(bool, string)> DeletePrD(int ID)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var prD = await context.SanPhams.Where(p => p.ID == ID).FirstOrDefaultAsync();
                    if (prD.IsDeleted == false) prD.IsDeleted = true;
                    await context.SaveChangesAsync();
                    return (true, "Xóa thành công");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, null);
            }

        }

        public async Task<bool> UpdateProduct(SanPham sanPhamNew)
        {
            try
            {
                int productId = sanPhamNew.ID;

                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var sanPham = await context.SanPhams.FindAsync(productId);

                    if (sanPham == null)
                    {
                        return false;
                    }

                    sanPham.TenSP = sanPhamNew.TenSP;
                    sanPham.DanhMucSanPhamID = sanPhamNew.DanhMucSanPhamID;
                    sanPham.Image = sanPhamNew.Image;

                    await context.SaveChangesAsync();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}
