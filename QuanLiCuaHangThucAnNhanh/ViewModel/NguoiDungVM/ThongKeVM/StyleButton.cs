using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace QuanLiCuaHangThucAnNhanh.ViewModel.NguoiDungVM.ThongKeVM
{
    public partial class ThongKeViewModel
    {
        private int caseNav;
        public int CaseNav
        {
            get => caseNav;
            set
            {
                caseNav = value;
                UpdateButtonColors();
            }
        }



        private SolidColorBrush lichSuBanColor;
        public SolidColorBrush LichSuBanColor
        {
            get => lichSuBanColor;
            set
            {
                lichSuBanColor = value;
                OnPropertyChanged(nameof(LichSuBanColor));
            }
        }

        private SolidColorBrush lichSuNhapColor;
        public SolidColorBrush LichSuNhapColor
        {
            get => lichSuNhapColor;
            set
            {
                lichSuNhapColor = value;
                OnPropertyChanged(nameof(LichSuNhapColor));
            }
        }

        private SolidColorBrush doanhThuColor;
        public SolidColorBrush DoanhThuColor
        {
            get => doanhThuColor;
            set
            {
                doanhThuColor = value;
                OnPropertyChanged(nameof(DoanhThuColor));
            }
        }




        private static SolidColorBrush colorSelect = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xF4, 0xF4));
        private void UpdateButtonColors()
        {

            ResetColors();
            switch (caseNav)
            {
                case 0:
                    LichSuBanColor = colorSelect;
                    break;
                case 1:
                    LichSuNhapColor = colorSelect;
                    break;
                case 2:
                    DoanhThuColor = colorSelect;
                    break;

            }
        }


        private static SolidColorBrush defaultColor = new SolidColorBrush(Colors.Transparent);
        private void ResetColors()
        {
            LichSuBanColor = defaultColor;
            DoanhThuColor = defaultColor;

            LichSuNhapColor = defaultColor;
        }
    }
}
