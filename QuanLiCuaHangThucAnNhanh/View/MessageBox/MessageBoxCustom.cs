﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QuanLiCuaHangThucAnNhanh.View.MessageBox
{
    public class MessageBoxCustom
    {
        public static int Success = 1;
        public static int Error = 2;
        public static int Warning = 3;
        public static void Show(int type, string message)
        {
            if (type == Success)
            {
                new Success(message).ShowDialog();
            }
            else if (type == Error)
            {
                new Error(message).ShowDialog();
            }
            else if(type == Warning)
            {
                new Warning(message).ShowDialog();
            }
        }
    }
}
