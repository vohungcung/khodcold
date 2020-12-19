using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("OrderTypes")]
	[Serializable]
    public class OrderType : DBO<OrderType>
    {
		#region Fields
		
		private string _ID;
		private string _SAPType;
		private string _Description;
		private string _WareHouseID;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public OrderType()
		{
			
		}
		
		/// <summary>
		/// Constructor with ID 
		/// </summary>
		/// <param name="ID">The ID</param>
		public OrderType(string ID )
		{
			this.ID = ID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="ID">Sets string value for ID</param>
		/// <param name="SAPType">Sets string value for SAPType</param>
		/// <param name="Description">Sets string value for Description</param>
		/// <param name="WareHouseID">Sets string value for WareHouseID</param>
		public OrderType(string iD, string sAPType, string description, string wareHouseID)
		{
			this.ID = iD;
			this.SAPType = sAPType;
			this.Description = description;
			this.WareHouseID = wareHouseID;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for ID
		/// </summary>
		[ColumnAttribute("ID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string ID
		{
			set
			{
				this._ID = value;
			}
			get
			{
				return this._ID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for SAPType
		/// </summary>
		[ColumnAttribute("SAPType", SqlDbType.NVarChar , 50 , false, false, false)]
		public string SAPType
		{
			set
			{
				this._SAPType = value;
			}
			get
			{
				return this._SAPType;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Description
		/// </summary>
		[ColumnAttribute("Description", SqlDbType.NVarChar , 250 , false, false, false)]
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
		/// Gets or sets string value for WareHouseID
		/// </summary>
		[ColumnAttribute("WareHouseID", SqlDbType.NVarChar , 50 , false, false, true)]
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
		/// Get a list Order of current OrderType object base on ID
		/// </summary>
		public List<Order> OrderListForOrderType
		{
			get
			{
				if (this.ID == null)
					return null;
				Order condition = new Order();
				condition.OrderType = this.ID;
				return OrderController.FindItems(condition);
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
            /// Get the name of column [ID]
            /// </summary>
            public string ID
            {
                get
                {
                    return "ID";
                }
            }
            /// <summary>
            /// Get the name of column [SAPType]
            /// </summary>
            public string SAPType
            {
                get
                {
                    return "SAPType";
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
            /// Get the name of column [WareHouseID]
            /// </summary>
            public string WareHouseID
            {
                get
                {
                    return "WareHouseID";
                }
            }
        }
        #endregion
    }
}
