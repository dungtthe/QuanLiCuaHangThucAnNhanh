//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLiCuaHangThucAnNhanh.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class THAMSO
    {
        public THAMSO(double HeSoBan, decimal OnePointToMoney, int MoneyToOnePoint) {
            this.HeSoBan = HeSoBan;
            this.OnePointToMoney = OnePointToMoney;
            this.MoneyToOnePoint = MoneyToOnePoint;
        }
        public int ID { get; set; }
        public Nullable<double> HeSoBan { get; set; }
        public Nullable<decimal> OnePointToMoney { get; set; }
        public Nullable<int> MoneyToOnePoint { get; set; }
    }
}
