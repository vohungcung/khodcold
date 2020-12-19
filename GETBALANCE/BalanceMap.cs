using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GetBalance
{
    class BalanceMap
    {
        string _Site, _ItemId, _Sloc;
        decimal _TonKho;
        public string Site { get { return _Site; } set { _Site = value; } }
        public string ItemId { get { return _ItemId; } set { _ItemId = value; } }
        public string Sloc { get { return _Sloc; } set { _Sloc = value; } }
        public decimal TonKho { get { return _TonKho; } set { _TonKho = value; } }


    }
}
