using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("VOrders")]
	[Serializable]
    public class VOrder : DBOView<VOrder>
    {
		#region Fields
		
		private string _OrderID;
		private string _OrderType;
		private string _CustomerID;
		private string _CustomerName;
		private string _DeliveryAddress;
		private string _SAPOrderNumber;
		private string _Ref2;
		private int? _Status;
		private decimal? _TotalAmount;
		private DateTime? _CreateDate;
		private int? _CreateBy;
		private string _ZipCode;
		private decimal? _Quantity;
		private bool? _ZO1;
		private bool? _ZOD;
		private bool? _ZO2;
		private bool? _ZOC;
		private string _MarketingID;
		private string _Description;
		private string _Note;
		private string _District;
		private bool? _IsTran;
		private string _HNID;
		private string _Site;
		private bool? _Confirmed;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public VOrder()
		{
			
		}
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for OrderID
		/// </summary>
		[ColumnAttribute("OrderID", SqlDbType.NVarChar , 50 , false, false, false)]
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
		/// Gets or sets string value for CustomerName
		/// </summary>
		[ColumnAttribute("CustomerName", SqlDbType.NVarChar , 250 , false, false, true)]
		public string CustomerName
		{
			set
			{
				this._CustomerName = value;
			}
			get
			{
				return this._CustomerName;
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
		/// Gets or sets string value for ZipCode
		/// </summary>
		[ColumnAttribute("ZipCode", SqlDbType.NVarChar , 50 , false, false, true)]
		public string ZipCode
		{
			set
			{
				this._ZipCode = value;
			}
			get
			{
				return this._ZipCode;
			}
		}
		
		/// <summary>
		/// Gets or sets decimal? value for Quantity
		/// </summary>
		[ColumnAttribute("Quantity", SqlDbType.Decimal , 17 , false, false, true)]
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
		/// Gets or sets bool? value for ZO1
		/// </summary>
		[ColumnAttribute("ZO1", SqlDbType.Bit , 1 , false, false, true)]
		public bool? ZO1
		{
			set
			{
				this._ZO1 = value;
			}
			get
			{
				return this._ZO1;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for ZOD
		/// </summary>
		[ColumnAttribute("ZOD", SqlDbType.Bit , 1 , false, false, true)]
		public bool? ZOD
		{
			set
			{
				this._ZOD = value;
			}
			get
			{
				return this._ZOD;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for ZO2
		/// </summary>
		[ColumnAttribute("ZO2", SqlDbType.Bit , 1 , false, false, true)]
		public bool? ZO2
		{
			set
			{
				this._ZO2 = value;
			}
			get
			{
				return this._ZO2;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for ZOC
		/// </summary>
		[ColumnAttribute("ZOC", SqlDbType.Bit , 1 , false, false, true)]
		public bool? ZOC
		{
			set
			{
				this._ZOC = value;
			}
			get
			{
				return this._ZOC;
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
		/// Gets or sets string value for Site
		/// </summary>
		[ColumnAttribute("Site", SqlDbType.NVarChar , 50 , false, false, true)]
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
            /// Get the name of column [CustomerName]
            /// </summary>
            public string CustomerName
            {
                get
                {
                    return "CustomerName";
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
            /// Get the name of column [ZipCode]
            /// </summary>
            public string ZipCode
            {
                get
                {
                    return "ZipCode";
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
            /// Get the name of column [ZO1]
            /// </summary>
            public string ZO1
            {
                get
                {
                    return "ZO1";
                }
            }
            /// <summary>
            /// Get the name of column [ZOD]
            /// </summary>
            public string ZOD
            {
                get
                {
                    return "ZOD";
                }
            }
            /// <summary>
            /// Get the name of column [ZO2]
            /// </summary>
            public string ZO2
            {
                get
                {
                    return "ZO2";
                }
            }
            /// <summary>
            /// Get the name of column [ZOC]
            /// </summary>
            public string ZOC
            {
                get
                {
                    return "ZOC";
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
