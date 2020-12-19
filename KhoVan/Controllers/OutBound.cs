using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiOrder.Models
{
    public class OutBoundClass
    {
        public string ItemID { get; set; } //đầu 8
        public string OutBound { get; set; } //đầu SO/STO
        public int Quantity { get; set; } //mã hàng
      
    }
}