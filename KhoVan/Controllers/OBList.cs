using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiOrder.Models
{
    public class OBList
    {
        public string OB { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalTX { get; set; }
        public decimal TotalAmount { get; set; }
        public int Bag { get; set; }
        public int Box { get; set; }
        public string ScannerID { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public decimal M3 { get; set; }
        public string CustomerID { get; set; }
        public string PlanDate { get; set; }
        public string Note { get; set; }
    }
}