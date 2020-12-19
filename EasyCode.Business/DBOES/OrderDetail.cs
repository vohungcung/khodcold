using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("OrderDetail")]
	[Serializable]
    public class OrderDetail : DBO<OrderDetail>
    {
		#region Fields
		
		private int? _OrderNo;
		private string _OrderID;
		private string _ItemID;
		private string _UoM;
		private decimal? _UnitPrice;
		private decimal? _Quantity;
		private decimal? _Discount;
		private decimal? _TotalAmount;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public OrderDetail()
		{
			
		}
		
		/// <summary>
		/// Constructor with OrderID , OrderID 
		/// </summary>
		/// <param name="OrderID">The OrderID</param>
		/// <param name="ItemID">The ItemID</param>
		public OrderDetail(string OrderID , string ItemID)
		{
			this.OrderID = OrderID;
			this.ItemID = ItemID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="OrderNo">Sets int? value for OrderNo</param>
		/// <param name="OrderID">Sets string value for OrderID</param>
		/// <param name="ItemID">Sets string value for ItemID</param>
		/// <param name="UoM">Sets string value for UoM</param>
		/// <param name="UnitPrice">Sets decimal? value for UnitPrice</param>
		/// <param name="Quantity">Sets decimal? value for Quantity</param>
		/// <param name="Discount">Sets decimal? value for Discount</param>
		/// <param name="TotalAmount">Sets decimal? value for TotalAmount</param>
		public OrderDetail(int? orderNo, string orderID, string itemID, string uoM, decimal? unitPrice, decimal? quantity, decimal? discount, decimal? totalAmount)
		{
			this.OrderNo = orderNo;
			this.OrderID = orderID;
			this.ItemID = itemID;
			this.UoM = uoM;
			this.UnitPrice = unitPrice;
			this.Quantity = quantity;
			this.Discount = discount;
			this.TotalAmount = totalAmount;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets int? value for OrderNo
		/// </summary>
		[ColumnAttribute("OrderNo", SqlDbType.Int , 4 , false, false, false)]
		public int? OrderNo
		{
			set
			{
				this._OrderNo = value;
			}
			get
			{
				return this._OrderNo;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for OrderID
		/// </summary>
		[ColumnAttribute("OrderID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string OrderID
		{
			set
			{
				this._OrderID = value;
			}
			get
			{
				return this._OrderID;
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
		
		/// <summary>
		/// Gets or sets decimal? value for Discount
		/// </summary>
		[ColumnAttribute("Discount", SqlDbType.Decimal , 5 , false, false, false)]
		public decimal? Discount
		{
			set
			{
				this._Discount = value;
			}
			get
			{
				return this._Discount;
			}
		}
		
		/// <summary>
		/// Gets or sets decimal? value for TotalAmount
		/// </summary>
		[ColumnAttribute("TotalAmount", SqlDbType.Money , 8 , false, false, false)]
		public decimal? TotalAmount
		{
			set
			{
				this._TotalAmount = value;
			}
			get
			{
				return this._TotalAmount;
			}
		}
		
		
		/// <summary>
		/// Get a Order of current OrderDetail object base on OrderID
		/// </summary>
		public Order OrderForOrder
		{
			get
			{
				if (this.OrderID == null)
					return null;
	
				Order condition = new Order(this.OrderID);
				return OrderController.FindItem(condition);
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
            /// Get the name of column [OrderNo]
            /// </summary>
            public string OrderNo
            {
                get
                {
                    return "OrderNo";
                }
            }
            /// <summary>
            /// Get the name of column [OrderID]
            /// </summary>
            public string OrderID
            {
                get
                {
                    return "OrderID";
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
            /// Get the name of column [Quantity]
            /// </summary>
            public string Quantity
            {
                get
                {
                    return "Quantity";
                }
            }
            /// <summary>
            /// Get the name of column [Discount]
            /// </summary>
            public string Discount
            {
                get
                {
                    return "Discount";
                }
            }
            /// <summary>
            /// Get the name of column [TotalAmount]
            /// </summary>
            public string TotalAmount
            {
                get
                {
                    return "TotalAmount";
                }
            }
        }
        #endregion
    }
}
