﻿using QuanLiCuaHangThucAnNhanh.Model.DTO;
using QuanLiCuaHangThucAnNhanh.Model.Mapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
