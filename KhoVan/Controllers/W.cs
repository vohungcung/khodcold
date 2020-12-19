using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication5.Controllers
{
    public class W
    {
        public string VoucherID { get; set; }
        public string VoucherDate { get; set; }
        public string ObjectID { get; set; }
        public string ObjectName { get; set; }
        public string Description { get; set; }
        public string SAP { get; set; }
        public string IW { get; set; }
        public string OW { get; set; }
        public WD[] WD { get; set; }
        public string[] Pallets { get; set; }
        public W()
        {
            VoucherID = "";
            VoucherDate = DateTime.Now.ToString("dd/MM/yyyy");
            ObjectID = "";
            ObjectName = "";
            Description = "";
            SAP = "";
            IW = "";
            OW = "";
            WD = new Controllers.WD[0];
            Pallets = new string[0];

        }
    }
}
