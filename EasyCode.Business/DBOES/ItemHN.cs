using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("ItemHN")]
	[Serializable]
    public class ItemHN : DBO<ItemHN>
    {
		#region Fields
		
		private string _HNID;
		private string _ItemID;
		private decimal? _UnitPrice;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ItemHN()
		{
			
		}
		
		/// <summary>
		/// Constructor with HNID , HNID 
		/// </summary>
		/// <param name="HNID">The HNID</param>
		/// <param name="ItemID">The ItemID</param>
		public ItemHN(string HNID , string ItemID)
		{
			this.HNID = HNID;
			this.ItemID = ItemID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="HNID">Sets string value for HNID</param>
		/// <param name="ItemID">Sets string value for ItemID</param>
		/// <param name="UnitPrice">Sets decimal? value for UnitPrice</param>
		public ItemHN(string hNID, string itemID, decimal? unitPrice)
		{
			this.HNID = hNID;
			this.ItemID = itemID;
			this.UnitPrice = unitPrice;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for HNID
		/// </summary>
		[ColumnAttribute("HNID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string HNID
		{
			set
			{
				this._HNID = value;
			}
			get
			{
				return this._HNID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for ItemID
		/// </summary>
		[ColumnAttribute("ItemID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string ItemID
		{
			set
			{
				this._ItemID = value;
			}
			get
			{
				return this._ItemID;
			}
		}
		
		/// <summary>
		/// Gets or sets decimal? value for UnitPrice
		/// </summary>
		[ColumnAttribute("UnitPrice", SqlDbType.Money , 8 , false, false, false)]
		public decimal? UnitPrice
		{
			set
			{
				this._UnitPrice = value;
			}
			get
			{
				return this._UnitPrice;
			}
		}
		
		
		/// <summary>
		/// Get a HNDH of current ItemHN object base on HNID
		/// </summary>
		public HNDH HNDHForHN
		{
			get
			{
				if (this.HNID == null)
					return null;
	
				HNDH condition = new HNDH(this.HNID);
				return HNDHController.FindItem(condition);
			}
		}
		
		/// <summary>
		/// Get a Item of current ItemHN object base on ItemID
		/// </summary>
		public Item ItemForItem
		{
			get
			{
				if (this.ItemID == null)
					return null;
	
				Item condition = new Item(this.ItemID);
				return ItemController.FindItem(condition);
			}
		}
		
		#endregion
        
        #region Other Properties
        private static ColumnNameStruct? _ColumnName;
        public static ColumnNameStruct ColumnName
        {
            get
            {
                if (!_ColumnName.HasValue)
                    _ColumnName = new ColumnNameStruct();
                return _ColumnName.Value;
            }
        }
        #endregion

        #region NestedType
        public  struct ColumnNameStruct
        {
            /// <summary>
            /// Get the name of column [HNID]
            /// </summary>
            public string HNID
            {
                get
                {
                    return "HNID";
                }
            }
            /// <summary>
            /// Get the name of column [ItemID]
            /// </summary>
            public string ItemID
            {
                get
                {
                    return "ItemID";
                }
            }
            /// <summary>
            /// Get the name of column [UnitPrice]
            /// </summary>
            public string UnitPrice
            {
                get
                {
                    return "UnitPrice";
                }
            }
        }
        #endregion
    }
}
