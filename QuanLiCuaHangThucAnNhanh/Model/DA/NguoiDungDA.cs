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
    }
}
