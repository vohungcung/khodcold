using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetBalance
{
    class CustomerMap
    {
        string _KUNNR, _NAME1, _ADDRESS_CUST, _TEL_NUMBER, _VTEXT, _BZIRK, _VWERK, _QUANHUYEN;
        string _LIFNR, _NAME2;
        public string KUNNR { get { return _KUNNR; } set { _KUNNR = value; } }//ma khach
        public string NAME1 { get { return _NAME1; } set { _NAME1 = value; } }//ten khach
        public string ADDRESS_CUST { get { return _ADDRESS_CUST; } set { _ADDRESS_CUST = value; } }//dia chi
        public string TEL_NUMBER { get { return _TEL_NUMBER; } set { _TEL_NUMBER = value; } }//dien thoai
        public string KONDA { get { return _VTEXT; } set { _VTEXT = value; } }//loai khach
        public string BZIRK { get { return _BZIRK; } set { _BZIRK = value; } }//khu vuc
        public string VWERK { get { return _VWERK; } set { _VWERK = value; } }//site
        public string QUANHUYEN { get { return _QUANHUYEN; } set { _QUANHUYEN = value; } }
        public string LIFNR { get { return _LIFNR; } set { _LIFNR = value; } }//ma tuyen
        public string NAME2 { get { return _NAME2; } set { _NAME2 = value; } }//ten tuyen
        public string EMAIL { get; set; }
    }
}
