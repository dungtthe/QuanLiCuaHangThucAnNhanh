using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.StaffVM
{
    public class MotSoPTBoTro
    {
        public static DateTime formatDate(string date)
        {

            try
            {
                string[] s = date.Split('/');
                string[] s2 = s[2].Split(' ');
                return new DateTime(int.Parse(s2[0]), int.Parse(s[0]), int.Parse(s[1]));
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
