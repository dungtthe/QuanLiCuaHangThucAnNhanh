﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLiCuaHangThucAnNhanh.Converter
{
    public class SalePriceConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is decimal price && values[1] is double multiplier)
            {
                string valuePrice;
                int b = (int)Math.Round((decimal)(price * (decimal)(1 + multiplier)));
                string c = b.ToString("N");
                valuePrice = c.Substring(0, c.Length - 3);
                return valuePrice;
            }
            return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
