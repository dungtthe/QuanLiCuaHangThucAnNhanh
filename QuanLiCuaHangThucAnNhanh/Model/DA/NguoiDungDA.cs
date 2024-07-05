using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Threading.Tasks;
using QuanLiCuaHangThucAnNhanh.Model.Mapper;
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
    }
}
