using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Model.Mapper;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace QuanLiCuaHangThucAnNhanh.Model.DA
{
    public class KhachHangDA
    {
        private static KhachHangDA instance;

        public static KhachHangDA gI()
        {
            if (instance == null)
            {
                instance = new KhachHangDA();
            }
            return instance;
        }
        private static KhachHangDA _ins;

        public static KhachHangDA Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new KhachHangDA();
                }
                return _ins;
            }
            private set { _ins = value; }
        }// test

        public async Task<List<KhachHangDTO>> GetAllCus()
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var cusList = (from s in context.KhachHangs
                                   where s.IsDeleted == false && s.HoTen != "Khách vãng lai"
                                   select new KhachHangDTO
                                   {
                                       ID = s.ID,
                                       HoTen = s.HoTen,
                                       Email = s.Email,
                                       SoDienThoai = s.SoDienThoai,
                                       DiaChi = s.DiaChi,
                                       DiemTichLuy = s.DiemTichLuy,
                                       IsDelete = s.IsDeleted
                                   }).ToListAsync();
                    return await cusList;
                }
            }
            catch
            {
                return null;
            }

        }


        public async Task<KhachHangDTO> FindNguoiDungBySoDienThoai(string SoDienThoai)
        {

            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var cus = await context.KhachHangs
                        .Where(nd => nd.SoDienThoai == SoDienThoai && nd.IsDeleted == false)
                        .FirstOrDefaultAsync();
                    return KhachHangMapper.MapToDTO(cus);
                }
            }
            catch
            {
                return null;
            }
        }
        public async Task<KhachHangDTO> FindNguoiDungByEmail(string Email)
        {

            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var cus = await context.KhachHangs
                        .Where(nd => nd.Email == Email && nd.IsDeleted == false)
                        .FirstOrDefaultAsync();
                    return KhachHangMapper.MapToDTO(cus);
                }
            }
            catch
            {
                return null;
            }
        }

        //Add cus
        public async Task<(bool, string)> AddNewCus(KhachHang newCus)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    bool IsEsixtEmail = await context.KhachHangs.AnyAsync(p => p.Email == newCus.Email);
                    bool IsExistPhone = await context.KhachHangs.AnyAsync(p => p.SoDienThoai == newCus.SoDienThoai);

                    var cus = await context.KhachHangs.Where(p => p.Email == newCus.Email || p.SoDienThoai == newCus.SoDienThoai).FirstOrDefaultAsync();
                    if (cus != null)
                    {
                        if (cus.IsDeleted == true)
                        {
                            cus.HoTen = newCus.HoTen;
                            cus.Email = newCus.Email;
                            cus.SoDienThoai = newCus.SoDienThoai;
                            cus.DiaChi = newCus.DiaChi;
                            cus.NgaySinh = newCus.NgaySinh;
                            cus.IsDeleted = false;
                            await context.SaveChangesAsync();
                            return (true, "Them thanh cong");
                        }
                        else
                        {
                            if (IsEsixtEmail)
                            {
                                return (false, "Email đã tồn tại");
                            }
                            if (IsExistPhone)
                            {
                                return (false, "Số điện thoại đã tồn tại");
                            }

                        }
                    }
                    newCus.DiemTichLuy = 0;
                    context.KhachHangs.Add(newCus);
                    await context.SaveChangesAsync();
                    return (true, "Them thanh cong");
                }

            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, null);
            }

        }

        public async Task<(bool, string)> EditCusList(KhachHang newCus, int ID)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var cus = await context.KhachHangs.Where(p => p.ID == ID).FirstOrDefaultAsync();
                    if (cus == null) return (false, "Không tìm thấy ID");
                    cus.Email = newCus.Email;
                    cus.SoDienThoai = newCus.SoDienThoai;
                    cus.HoTen = newCus.HoTen;
                    cus.DiaChi = newCus.DiaChi;
                    await context.SaveChangesAsync();
                    return (true, "Chỉnh sửa thành công");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi khi chỉnh sửa khách hàng");
                return (false, null);
            }


        }


        public async Task<bool> EditCusBySell(KhachHang newCus)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var cus = await context.KhachHangs.Where(p => p.ID == newCus.ID).FirstOrDefaultAsync();
                    if (cus == null) return false;
                    cus.DiemTichLuy = newCus.DiemTichLuy;
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }


        }

        public async Task<(bool, string)> DeleteCustomer(int ID)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var cus = await context.KhachHangs.Where(p => p.ID == ID).FirstOrDefaultAsync();
                    if (cus.IsDeleted == true) return (false, "Đã xóa khách hàng này rồi");
                    cus.IsDeleted = true;
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

    }
}
