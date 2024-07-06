using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using QuanLiCuaHangThucAnNhanh.Model.Mapper;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using System.Collections;
namespace QuanLiCuaHangThucAnNhanh.Model.DA
{
    public class NguoiDungDA
    {

        private static NguoiDungDA instance;

        public static NguoiDungDA gI()
        {
            if (instance == null)
            {
                instance = new NguoiDungDA();
            }
            return instance;
        }
        private static NguoiDungDA _ins;

        public static NguoiDungDA Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new NguoiDungDA();
                }
                return _ins;
            }
            private set { _ins = value; }
        }

        public async Task<NguoiDungDTO> FindNguoiDungByUsernameAndPassword(string username, string password)
        {

            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var nguoiDung = await context.NguoiDungs
                        .Where(nd => nd.TenTaiKhoan == username && nd.MatKhau == password && nd.IsDeleted == false)
                        .FirstOrDefaultAsync();
                    return NguoiDungMapper.MapToDTO(nguoiDung);
                }
            }
            catch 
            {
                return null;
            }
        }
        //Add staff
        public async Task<(bool, string)> AddNewStaff(NguoiDung newStaff)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    bool IsEsixtEmail = await context.NguoiDungs.AnyAsync(p => p.Email == newStaff.Email);
                    bool IsExistPhone = await context.NguoiDungs.AnyAsync(p => p.SoDienThoai == newStaff.SoDienThoai);
                    bool IsExistUsername = await context.NguoiDungs.AnyAsync(p => p.TenTaiKhoan == newStaff.TenTaiKhoan);
                    var staff = await context.NguoiDungs.Where(p => p.Email == newStaff.Email || p.SoDienThoai == newStaff.SoDienThoai || p.TenTaiKhoan == newStaff.TenTaiKhoan).FirstOrDefaultAsync();
                    if (staff != null)
                    {
                        if (staff.IsDeleted == true)
                        {
                            staff.HoTen = newStaff.HoTen;
                            staff.TenTaiKhoan = newStaff.TenTaiKhoan;
                            staff.MatKhau = newStaff.MatKhau;
                            staff.SoDienThoai = newStaff.SoDienThoai;
                            staff.NgaySinh = newStaff.NgaySinh;
                            staff.Email = newStaff.Email;
                            staff.Loai = newStaff.Loai;
                            staff.IsDeleted = false;
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
                            if (IsExistUsername)
                            {
                                return (false, "Tài khoản đã tồn tại");
                            }
                        }
                    }
                    context.NguoiDungs.Add(newStaff);
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

        public async Task<List<NguoiDungDTO>> GetAllUser()
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var staffList = (from s in context.NguoiDungs
                                   where s.IsDeleted == false && s.Loai == 0
                                   select new NguoiDungDTO
                                   {
                                       ID = s.ID,
                                       HoTen = s.HoTen,
                                       SoDienThoai = s.SoDienThoai,
                                       Email = s.Email,
                                       NgaySinh = s.NgaySinh,
                                       Loai = s.Loai,
                                       DiaChi = s.DiaChi,
                                       IsDeleted = s.IsDeleted
                                   }).ToListAsync();
                    return await staffList;
                }
            }
            catch
            {
                return null;
            }

        }

        public async Task<(bool, string)> EditStaff(NguoiDung newStaff)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var staff = await context.NguoiDungs.Where(p => p.ID == newStaff.ID).FirstOrDefaultAsync();
                    staff.HoTen = newStaff.HoTen;
                    staff.SoDienThoai = newStaff.SoDienThoai;
                    staff.NgaySinh = newStaff.NgaySinh;
                    staff.Email = newStaff.Email;
                    staff.DiaChi = newStaff.DiaChi;
                    await context.SaveChangesAsync();
                    return (true, "Cap that thanh cong");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, null);
            }

        }
        public async Task<(bool, string)> DeleteStaff(int ID)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var staff = await context.NguoiDungs.Where(p => p.ID == ID).FirstOrDefaultAsync();
                    if (staff.IsDeleted == false) staff.IsDeleted = true;
                    await context.SaveChangesAsync();
                    return (true, "Da xoa");
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
