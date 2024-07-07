using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.Model.Mapper
{
    public class ThamSoMapper
    {
        public static ThamSoDTO MapToDTO(THAMSO ts)
        {
            if (ts == null) return null;
            return new ThamSoDTO
            {
                ID = ts.ID,
                HeSoBan = ts.HeSoBan,
                OnePointToMoney = ts.OnePointToMoney,
                MoneyToOnePoint = ts.MoneyToOnePoint,
            };
        }
    }
}
