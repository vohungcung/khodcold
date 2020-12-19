using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Items")]
	[Serializable]
    public class Item : DBO<Item>
    {
		#region Fields
		
		private string _ItemID;
		private string _ItemName;
		private decimal? _StockSAP;
		private decimal? _OrderQuantity;
		private decimal? _UnitPrice;
		private string _UoM;
		private string _ItemType;
		private string _ThumbImage;
		private bool? _Sex;
		private bool? _IsNew;
		private string _MC;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Item()
		{
			
		}
		
		/// <summary>
		/// Constructor with ItemID 
		/// </summary>
		/// <param name="ItemID">The ItemID</param>
		public Item(string ItemID )
		{
			this.ItemID = ItemID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="ItemID">Sets string value for ItemID</param>
		/// <param name="ItemName">Sets string value for ItemName</param>
		/// <param name="StockSAP">Sets decimal? value for StockSAP</param>
		/// <param name="OrderQuantity">Sets decimal? value for OrderQuantity</param>
		/// <param name="UnitPrice">Sets decimal? value for UnitPrice</param>
		/// <param name="UoM">Sets string value for UoM</param>
		/// <param name="ItemType">Sets string value for ItemType</param>
		/// <param name="ThumbImage">Sets string value for ThumbImage</param>
		/// <param name="Sex">Sets bool? value for Sex</param>
		/// <param name="IsNew">Sets bool? value for IsNew</param>
		/// <param name="MC">Sets string value for MC</param>
		public Item(string itemID, string itemName, decimal? stockSAP, decimal? orderQuantity, decimal? unitPrice, string uoM, string itemType, string thumbImage, bool? sex, bool? isNew, string mC)
		{
			this.ItemID = itemID;
			this.ItemName = itemName;
			this.StockSAP = stockSAP;
			this.OrderQuantity = orderQuantity;
			this.UnitPrice = unitPrice;
			this.UoM = uoM;
			this.ItemType = itemType;
			this.ThumbImage = thumbImage;
			this.Sex = sex;
			this.IsNew = isNew;
			this.MC = mC;
		}
		
		#endregion
		
		#region Properties
		
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
		/// Gets or sets string value for ItemName
		/// </summary>
		[ColumnAttribute("ItemName", SqlDbType.NVarChar , 250 , false, false, false)]
		public string ItemName
		{
			set
			{
				this._ItemName = value;
			}
			get
			{
				return this._ItemName;
			}
		}
		
		/// <summary>
		/// Gets or sets decimal? value for StockSAP
		/// </summary>
		[ColumnAttribute("StockSAP", SqlDbType.Decimal , 9 , false, false, false)]
		public decimal? StockSAP
		{
			set
			{
				this._StockSAP = value;
			}
			get
			{
				return this._StockSAP;
			}
		}
		
		/// <summary>
		/// Gets or sets decimal? value for OrderQuantity
		/// </summary>
		[ColumnAttribute("OrderQuantity", SqlDbType.Decimal , 9 , false, false, false)]
		public decimal? OrderQuantity
		{
			set
			{
				this._OrderQuantity = value;
			}
			get
			{
				return this._OrderQuantity;
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
		/// Gets or sets string value for UoM
		/// </summary>
		[ColumnAttribute("UoM", SqlDbType.NVarChar , 50 , false, false, false)]
		public string UoM
		{
			set
			{
				this._UoM = value;
			}
			get
			{
				return this._UoM;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for ItemType
		/// </summary>
		[ColumnAttribute("ItemType", SqlDbType.NVarChar , 50 , false, false, false)]
		public string ItemType
		{
			set
			{
				this._ItemType = value;
			}
			get
			{
				return this._ItemType;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for ThumbImage
		/// </summary>
		[ColumnAttribute("ThumbImage", SqlDbType.NVarChar , 250 , false, false, false)]
		public string ThumbImage
		{
			set
			{
				this._ThumbImage = value;
			}
			get
			{
				return this._ThumbImage;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for Sex
		/// </summary>
		[ColumnAttribute("Sex", SqlDbType.Bit , 1 , false, false, false)]
		public bool? Sex
		{
			set
			{
				this._Sex = value;
			}
			get
			{
				return this._Sex;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for IsNew
		/// </summary>
		[ColumnAttribute("IsNew", SqlDbType.Bit , 1 , false, false, false)]
		public bool? IsNew
		{
			set
			{
				this._IsNew = value;
			}
			get
			{
				return this._IsNew;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for MC
		/// </summary>
		[ColumnAttribute("MC", SqlDbType.NVarChar , 50 , false, false, true)]
		public string MC
		{
			set
			{
				this._MC = value;
			}
			get
			{
				return this._MC;
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
            /// Get the name of column [ItemName]
            /// </summary>
            public string ItemName
            {
                get
                {
                    return "ItemName";
                }
            }
            /// <summary>
            /// Get the name of column [StockSAP]
            /// </summary>
            public string StockSAP
            {
                get
                {
                    return "StockSAP";
                }
            }
            /// <summary>
            /// Get the name of column [OrderQuantity]
            /// </summary>
            public string OrderQuantity
            {
                get
                {
                    return "OrderQuantity";
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
            /// <summary>
            /// Get the name of column [UoM]
            /// </summary>
            public string UoM
            {
                get
                {
                    return "UoM";
                }
            }
            /// <summary>
            /// Get the name of column [ItemType]
            /// </summary>
            public string ItemType
            {
                get
                {
                    return "ItemType";
                }
            }
            /// <summary>
            /// Get the name of column [ThumbImage]
            /// </summary>
            public string ThumbImage
            {
                get
                {
                    return "ThumbImage";
                }
            }
            /// <summary>
            /// Get the name of column [Sex]
            /// </summary>
            public string Sex
            {
                get
                {
                    return "Sex";
                }
            }
            /// <summary>
            /// Get the name of column [IsNew]
            /// </summary>
            public string IsNew
            {
                get
                {
                    return "IsNew";
                }
            }
            /// <summary>
            /// Get the name of column [MC]
            /// </summary>
            public string MC
            {
                get
                {
                    return "MC";
                }
            }
        }
        #endregion
    }
}
