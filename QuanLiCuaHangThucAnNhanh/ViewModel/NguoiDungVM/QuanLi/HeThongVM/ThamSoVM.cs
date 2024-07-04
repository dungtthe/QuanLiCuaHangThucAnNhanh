using QuanLiCuaHangThucAnNhanh.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.QuanLi.HeThongVM
{
    public partial class HeThongVM
    {
        private ThamSoDTO thamSoDTO;
        public ThamSoDTO ThamSoDTO
        {
            get => thamSoDTO;
            set
            {
                thamSoDTO = value;
                OnPropertyChanged(nameof(ThamSoDTO));
            }
        }
    }
}
