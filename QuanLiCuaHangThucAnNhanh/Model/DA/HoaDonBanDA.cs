using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Model.Mapper;
using QuanLiCuaHangThucAnNhanh.View.MessageBox;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;

namespace QuanLiCuaHangThucAnNhanh.Model.DA
{
    public class HoaDonBanDA
    {
        private static HoaDonBanDA instance;

        public static HoaDonBanDA gI()
        {
            if (instance == null)
            {
                instance = new HoaDonBanDA();
            }
            return instance;
        }


        public async Task<List<HoaDonBanDTO>> GetAllHoaDonBan()
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var billEntities = await (from c in context.HoaDonBans
                                              where c.IsDeleted == false
                                              select c).OrderByDescending(m => m.NgayTao).ToListAsync();

                    var billList = billEntities.Select(c => new HoaDonBanDTO
                    {
                        ID = c.ID,
                        KhachHangID = c.KhachHangID,
                        NguoiDungID = c.NguoiDungID,
                        NgayTao = c.NgayTao,
                        TongTienBan = c.TongTienBan,
                        KhachHangDTO = KhachHangMapper.MapToDTO(c.KhachHang),
                        NguoiDungDTO = NguoiDungMapper.MapToDTO(c.NguoiDung),
                        ListChiTietHoaDonBanDTO = c.ChiTietHoaDonBans
                                                  .Where(x => x.IsDeleted == false)
                                                  .Select(x => new ChiTietHoaDonBanDTO
                                                  {
                                                      HoaDonBanID = x.HoaDonBanID,
                                                      DonGia = x.DonGia,
                                                      SanPhamID = x.SanPhamID,
                                                      SoLuong = x.SoLuong,
                                                      HoaDonBanDTO = HoaDonBanMapper.MapToDTO(x.HoaDonBan),
                                                      SanPhamDTO = SanPhamMapper.MapToDTO(x.SanPham),
                                                  }).ToList(),
                        IsDeleted = c.IsDeleted
                    }).ToList();

                    return billList;
                }
            }
            catch
            {
                return null;
            }
        }

        //trả về tổng tiền thu được trong 1 ngày
        public async Task<int> GetDoanhThuByDate(DateTime date)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {
                    var billTotalPaid = await context.HoaDonBans
                        .Where(p => p.NgayTao.HasValue
                                    && p.NgayTao.Value.Day == date.Day
                                    && p.NgayTao.Value.Month == date.Month
                                    && p.NgayTao.Value.Year == date.Year
                                    && p.IsDeleted == false)
                        .SumAsync(bill => (int?)bill.TongTienBan);

                    return billTotalPaid ?? 0;
                }
            }
            catch
            {
                return -1;
            }
        }



        public async Task<bool> AddNewBill(HoaDonBanDTO newBill)
        {
            try
            {
                using (var context = new QuanLiCuaHangThucAnNhanhEntities())
                {

                    int? maxID = await context.HoaDonBans.MaxAsync(b => (int?)b.ID);
                    int curID = 0;
                    if (maxID.HasValue)
                    {
                        curID = (int)maxID + 1;
                    }
                    else
                    {
                        curID = 1;
                    }

                    newBill.ID = curID;


                    HoaDonBan hoaDonBan = new HoaDonBan {
                        ID = newBill.ID,
                        NgayTao = newBill.NgayTao,
                        TongTienBan = newBill.TongTienBan,
                        NguoiDungID = newBill.NguoiDungID,
                        KhachHangID = newBill.KhachHangID,
                        IsDeleted = false
                    };


                    List<ChiTietHoaDonBan> listChiTiet = new List<ChiTietHoaDonBan>();
                    foreach (var item in newBill.ListChiTietHoaDonBanDTO)
                    {
                        ChiTietHoaDonBan chiTietHoaDonBan = new ChiTietHoaDonBan
                        {
                            HoaDonBanID = curID,
                            SanPhamID=item.SanPhamID,
                            SoLuong=item.SoLuong,
                            DonGia=item.DonGia,
                            IsDeleted=false
                        };
                    }



                    context.ChiTietHoaDonBans.AddRange(listChiTiet);
                    context.HoaDonBans.Add(hoaDonBan);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }
    }
}
