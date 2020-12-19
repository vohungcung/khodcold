using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication5.Controllers
{
    public class PalletAndItem
    {
        public string PalletID { get; set; }
        public string ItemID { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public string LSX { get; set; }
    }
}
