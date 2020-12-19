using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Balances")]
	[Serializable]
    public class Balance : DBO<Balance>
    {
		#region Fields
		
		private string _WareHouseID;
		private string _Site;
		private string _ItemID;
		private decimal? _Quantity;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Balance()
		{
			
		}
		
		/// <summary>
		/// Constructor with WareHouseID , WareHouseID , WareHouseID 
		/// </summary>
		/// <param name="WareHouseID">The WareHouseID</param>
		/// <param name="Site">The Site</param>
		/// <param name="ItemID">The ItemID</param>
		public Balance(string WareHouseID , string Site, string ItemID)
		{
			this.WareHouseID = WareHouseID;
			this.Site = Site;
			this.ItemID = ItemID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="WareHouseID">Sets string value for WareHouseID</param>
		/// <param name="Site">Sets string value for Site</param>
		/// <param name="ItemID">Sets string value for ItemID</param>
		/// <param name="Quantity">Sets decimal? value for Quantity</param>
		public Balance(string wareHouseID, string site, string itemID, decimal? quantity)
		{
			this.WareHouseID = wareHouseID;
			this.Site = site;
			this.ItemID = itemID;
			this.Quantity = quantity;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for WareHouseID
		/// </summary>
		[ColumnAttribute("WareHouseID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string WareHouseID
		{
			set
			{
				this._WareHouseID = value;
			}
			get
			{
				return this._WareHouseID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Site
		/// </summary>
		[ColumnAttribute("Site", SqlDbType.NVarChar , 50 , true, false, false)]
		public string Site
		{
			set
			{
				this._Site = value;
			}
			get
			{
				return this._Site;
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
		/// Gets or sets decimal? value for Quantity
		/// </summary>
		[ColumnAttribute("Quantity", SqlDbType.Decimal , 9 , false, false, false)]
		public decimal? Quantity
		{
			set
			{
				this._Quantity = value;
			}
			get
			{
				return this._Quantity;
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
            /// Get the name of column [WareHouseID]
            /// </summary>
            public string WareHouseID
            {
                get
                {
                    return "WareHouseID";
                }
            }
            /// <summary>
            /// Get the name of column [Site]
            /// </summary>
            public string Site
            {
                get
                {
                    return "Site";
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
            /// Get the name of column [Quantity]
            /// </summary>
            public string Quantity
            {
                get
                {
                    return "Quantity";
                }
            }
        }
        #endregion
    }
}
