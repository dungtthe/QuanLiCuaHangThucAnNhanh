﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace QuanLiCuaHangThucAnNhanh.Converter
{
    public class PriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal price)
            {
                try
                {
                    string valuePrice;
                    int b = Decimal.ToInt32(price);
                    string c = b.ToString("N");
                    valuePrice = c.Substring(0, c.Length - 3);
                    return valuePrice;
                }
                catch
                {
                    return value;
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
