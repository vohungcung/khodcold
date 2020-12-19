using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Orders")]
	[Serializable]
    public class Order : DBO<Order>
    {
		#region Fields
		
		private string _OrderID;
		private string _OrderType;
		private string _CustomerID;
		private string _DeliveryAddress;
		private string _SAPOrderNumber;
		private string _Ref2;
		private int? _Status;
		private decimal? _TotalAmount;
		private DateTime? _CreateDate;
		private int? _CreateBy;
		private string _MarketingID;
		private string _Note;
		private string _Description;
		private string _District;
		private bool? _IsTran;
		private string _HNID;
		private bool? _Confirmed;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Order()
		{
			
		}
		
		/// <summary>
		/// Constructor with OrderID 
		/// </summary>
		/// <param name="OrderID">The OrderID</param>
		public Order(string OrderID )
		{
			this.OrderID = OrderID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="OrderID">Sets string value for OrderID</param>
		/// <param name="OrderType">Sets string value for OrderType</param>
		/// <param name="CustomerID">Sets string value for CustomerID</param>
		/// <param name="DeliveryAddress">Sets string value for DeliveryAddress</param>
		/// <param name="SAPOrderNumber">Sets string value for SAPOrderNumber</param>
		/// <param name="Ref2">Sets string value for Ref2</param>
		/// <param name="Status">Sets int? value for Status</param>
		/// <param name="TotalAmount">Sets decimal? value for TotalAmount</param>
		/// <param name="CreateDate">Sets DateTime? value for CreateDate</param>
		/// <param name="CreateBy">Sets int? value for CreateBy</param>
		/// <param name="MarketingID">Sets string value for MarketingID</param>
		/// <param name="Note">Sets string value for Note</param>
		/// <param name="Description">Sets string value for Description</param>
		/// <param name="District">Sets string value for District</param>
		/// <param name="IsTran">Sets bool? value for IsTran</param>
		/// <param name="HNID">Sets string value for HNID</param>
		/// <param name="Confirmed">Sets bool? value for Confirmed</param>
		public Order(string orderID, string orderType, string customerID, string deliveryAddress, string sAPOrderNumber, string ref2, int? status, decimal? totalAmount, DateTime? createDate, int? createBy, string marketingID, string note, string description, string district, bool? isTran, string hNID, bool? confirmed)
		{
			this.OrderID = orderID;
			this.OrderType = orderType;
			this.CustomerID = customerID;
			this.DeliveryAddress = deliveryAddress;
			this.SAPOrderNumber = sAPOrderNumber;
			this.Ref2 = ref2;
			this.Status = status;
			this.TotalAmount = totalAmount;
			this.CreateDate = createDate;
			this.CreateBy = createBy;
			this.MarketingID = marketingID;
			this.Note = note;
			this.Description = description;
			this.District = district;
			this.IsTran = isTran;
			this.HNID = hNID;
			this.Confirmed = confirmed;
		}
		
		#endregion
		
		#region Properties
		
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
		/// Gets or sets string value for OrderType
		/// </summary>
		[ColumnAttribute("OrderType", SqlDbType.NVarChar , 50 , false, false, false)]
		public string OrderType
		{
			set
			{
				this._OrderType = value;
			}
			get
			{
				return this._OrderType;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for CustomerID
		/// </summary>
		[ColumnAttribute("CustomerID", SqlDbType.NVarChar , 50 , false, false, false)]
		public string CustomerID
		{
			set
			{
				this._CustomerID = value;
			}
			get
			{
				return this._CustomerID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for DeliveryAddress
		/// </summary>
		[ColumnAttribute("DeliveryAddress", SqlDbType.NVarChar , 250 , false, false, false)]
		public string DeliveryAddress
		{
			set
			{
				this._DeliveryAddress = value;
			}
			get
			{
				return this._DeliveryAddress;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for SAPOrderNumber
		/// </summary>
		[ColumnAttribute("SAPOrderNumber", SqlDbType.NVarChar , 50 , false, false, false)]
		public string SAPOrderNumber
		{
			set
			{
				this._SAPOrderNumber = value;
			}
			get
			{
				return this._SAPOrderNumber;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Ref2
		/// </summary>
		[ColumnAttribute("Ref2", SqlDbType.NVarChar , 50 , false, false, false)]
		public string Ref2
		{
			set
			{
				this._Ref2 = value;
			}
			get
			{
				return this._Ref2;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for Status
		/// </summary>
		[ColumnAttribute("Status", SqlDbType.Int , 4 , false, false, false)]
		public int? Status
		{
			set
			{
				this._Status = value;
			}
			get
			{
				return this._Status;
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
		/// Gets or sets DateTime? value for CreateDate
		/// </summary>
		[ColumnAttribute("CreateDate", SqlDbType.DateTime , 8 , false, false, false)]
		public DateTime? CreateDate
		{
			set
			{
				this._CreateDate = value;
			}
			get
			{
				return this._CreateDate;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for CreateBy
		/// </summary>
		[ColumnAttribute("CreateBy", SqlDbType.Int , 4 , false, false, false)]
		public int? CreateBy
		{
			set
			{
				this._CreateBy = value;
			}
			get
			{
				return this._CreateBy;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for MarketingID
		/// </summary>
		[ColumnAttribute("MarketingID", SqlDbType.NVarChar , 50 , false, false, true)]
		public string MarketingID
		{
			set
			{
				this._MarketingID = value;
			}
			get
			{
				return this._MarketingID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Note
		/// </summary>
		[ColumnAttribute("Note", SqlDbType.NVarChar , 250 , false, false, true)]
		public string Note
		{
			set
			{
				this._Note = value;
			}
			get
			{
				return this._Note;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Description
		/// </summary>
		[ColumnAttribute("Description", SqlDbType.NVarChar , 250 , false, false, true)]
		public string Description
		{
			set
			{
				this._Description = value;
			}
			get
			{
				return this._Description;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for District
		/// </summary>
		[ColumnAttribute("District", SqlDbType.NVarChar , 250 , false, false, true)]
		public string District
		{
			set
			{
				this._District = value;
			}
			get
			{
				return this._District;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for IsTran
		/// </summary>
		[ColumnAttribute("IsTran", SqlDbType.Bit , 1 , false, false, true)]
		public bool? IsTran
		{
			set
			{
				this._IsTran = value;
			}
			get
			{
				return this._IsTran;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for HNID
		/// </summary>
		[ColumnAttribute("HNID", SqlDbType.NVarChar , 50 , false, false, true)]
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
		/// Gets or sets bool? value for Confirmed
		/// </summary>
		[ColumnAttribute("Confirmed", SqlDbType.Bit , 1 , false, false, true)]
		public bool? Confirmed
		{
			set
			{
				this._Confirmed = value;
			}
			get
			{
				return this._Confirmed;
			}
		}
		
		
		/// <summary>
		/// Get a list OrderDetail of current Order object base on OrderID
		/// </summary>
		public List<OrderDetail> OrderDetailListForOrderID
		{
			get
			{
				if (this.OrderID == null)
					return null;
				OrderDetail condition = new OrderDetail();
				condition.OrderID = this.OrderID;
				return OrderDetailController.FindItems(condition);
			}
		}
		
		/// <summary>
		/// Get a OrderType of current Order object base on OrderType
		/// </summary>
		public OrderType OrderTypeForOrderType
		{
			get
			{
				if (this.OrderType == null)
					return null;
	
				OrderType condition = new OrderType(this.OrderType);
				return OrderTypeController.FindItem(condition);
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
            /// Get the name of column [OrderType]
            /// </summary>
            public string OrderType
            {
                get
                {
                    return "OrderType";
                }
            }
            /// <summary>
            /// Get the name of column [CustomerID]
            /// </summary>
            public string CustomerID
            {
                get
                {
                    return "CustomerID";
                }
            }
            /// <summary>
            /// Get the name of column [DeliveryAddress]
            /// </summary>
            public string DeliveryAddress
            {
                get
                {
                    return "DeliveryAddress";
                }
            }
            /// <summary>
            /// Get the name of column [SAPOrderNumber]
            /// </summary>
            public string SAPOrderNumber
            {
                get
                {
                    return "SAPOrderNumber";
                }
            }
            /// <summary>
            /// Get the name of column [Ref2]
            /// </summary>
            public string Ref2
            {
                get
                {
                    return "Ref2";
                }
            }
            /// <summary>
            /// Get the name of column [Status]
            /// </summary>
            public string Status
            {
                get
                {
                    return "Status";
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
            /// <summary>
            /// Get the name of column [CreateDate]
            /// </summary>
            public string CreateDate
            {
                get
                {
                    return "CreateDate";
                }
            }
            /// <summary>
            /// Get the name of column [CreateBy]
            /// </summary>
            public string CreateBy
            {
                get
                {
                    return "CreateBy";
                }
            }
            /// <summary>
            /// Get the name of column [MarketingID]
            /// </summary>
            public string MarketingID
            {
                get
                {
                    return "MarketingID";
                }
            }
            /// <summary>
            /// Get the name of column [Note]
            /// </summary>
            public string Note
            {
                get
                {
                    return "Note";
                }
            }
            /// <summary>
            /// Get the name of column [Description]
            /// </summary>
            public string Description
            {
                get
                {
                    return "Description";
                }
            }
            /// <summary>
            /// Get the name of column [District]
            /// </summary>
            public string District
            {
                get
                {
                    return "District";
                }
            }
            /// <summary>
            /// Get the name of column [IsTran]
            /// </summary>
            public string IsTran
            {
                get
                {
                    return "IsTran";
                }
            }
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
            /// Get the name of column [Confirmed]
            /// </summary>
            public string Confirmed
            {
                get
                {
                    return "Confirmed";
                }
            }
        }
        #endregion
    }
}
