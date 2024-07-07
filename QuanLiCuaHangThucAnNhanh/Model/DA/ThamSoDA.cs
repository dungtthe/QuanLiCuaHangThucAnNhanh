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
    public class ThamSoDA
    {
        private static ThamSoDA instance;

        public static ThamSoDA gI()
        {
            if (instance == null)
            {
                instance = new ThamSoDA();
            }
            return instance;
        }

        public async Task<ThamSoDTO> GetThamSoCur()
        {

            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var t = await context.THAMSOes
                        .Where(nd => nd.ID == 1)
                        .FirstOrDefaultAsync();
                    return ThamSoMapper.MapToDTO(t);
                }
            }
            catch(Exception e)
            {
                return null;
            }
        }
       /* public async Task<(bool, string)> EditThamSo(THAMSO newTHAMSO)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var thamSo = await context.THAMSOes.Where(p => p.ID == 1).FirstOrDefaultAsync();
                    thamSo.HeSoBan = newTHAMSO.HeSoBan;
                    thamSo.OnePointToMoney = newTHAMSO.OnePointToMoney;
                    thamSo.MoneyToOnePoint = newTHAMSO.MoneyToOnePoint;
                    await context.SaveChangesAsync();
                    return (true, "Cap that thanh cong");
                }
            }
            catch
            {
                MessageBoxCustom.Show(MessageBoxCustom.Error, "Xảy ra lỗi");
                return (false, null);
            }

        }*/

    }
}
