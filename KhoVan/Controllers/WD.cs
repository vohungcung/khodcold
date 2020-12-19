using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication5.Controllers
{
    public class WD
    {
        public string TransactionID { get; set; }
        public int OrderNo { get; set; }
        public string ItemID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int Q { get; set; }

        public string UnitID { get; set; }
        public string Note { get; set; }
        public  string PalletID { get; set; }
        public string Location { get; set; }
        public string LSX { get; set; }
        public string OB { get; set; }
        public WD()
        {
            TransactionID = "";
            OrderNo = 1;
            ItemID = "";
            ItemName = "";
            Quantity = 0;
            UnitID = "";
            Note = "";
            PalletID = "";
            Location = "";
            LSX = "";
            OB = "";
            Q = 0;
        }
    }
}
