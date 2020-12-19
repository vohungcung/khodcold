using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("ModifyLogs")]
	[Serializable]
    public class ModifyLog : DBO<ModifyLog>
    {
		#region Fields
		
		private DateTime? _ModifyDate;
		private int? _ModifyUserID;
		private string _CustomerID;
		private string _OrderID;
		private string _Note;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ModifyLog()
		{
			
		}
		
		/// <summary>
		/// Constructor with ModifyDate , ModifyDate , ModifyDate , ModifyDate 
		/// </summary>
		/// <param name="ModifyDate">The ModifyDate</param>
		/// <param name="ModifyUserID">The ModifyUserID</param>
		/// <param name="CustomerID">The CustomerID</param>
		/// <param name="OrderID">The OrderID</param>
		public ModifyLog(DateTime? ModifyDate , int? ModifyUserID, string CustomerID, string OrderID)
		{
			this.ModifyDate = ModifyDate;
			this.ModifyUserID = ModifyUserID;
			this.CustomerID = CustomerID;
			this.OrderID = OrderID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="ModifyDate">Sets DateTime? value for ModifyDate</param>
		/// <param name="ModifyUserID">Sets int? value for ModifyUserID</param>
		/// <param name="CustomerID">Sets string value for CustomerID</param>
		/// <param name="OrderID">Sets string value for OrderID</param>
		/// <param name="Note">Sets string value for Note</param>
		public ModifyLog(DateTime? modifyDate, int? modifyUserID, string customerID, string orderID, string note)
		{
			this.ModifyDate = modifyDate;
			this.ModifyUserID = modifyUserID;
			this.CustomerID = customerID;
			this.OrderID = orderID;
			this.Note = note;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets DateTime? value for ModifyDate
		/// </summary>
		[ColumnAttribute("ModifyDate", SqlDbType.DateTime , 8 , true, false, false)]
		public DateTime? ModifyDate
		{
			set
			{
				this._ModifyDate = value;
			}
			get
			{
				return this._ModifyDate;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for ModifyUserID
		/// </summary>
		[ColumnAttribute("ModifyUserID", SqlDbType.Int , 4 , true, false, false)]
		public int? ModifyUserID
		{
			set
			{
				this._ModifyUserID = value;
			}
			get
			{
				return this._ModifyUserID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for CustomerID
		/// </summary>
		[ColumnAttribute("CustomerID", SqlDbType.NVarChar , 50 , true, false, false)]
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
		/// Gets or sets string value for Note
		/// </summary>
		[ColumnAttribute("Note", SqlDbType.NVarChar , 50 , false, false, false)]
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
            /// Get the name of column [ModifyDate]
            /// </summary>
            public string ModifyDate
            {
                get
                {
                    return "ModifyDate";
                }
            }
            /// <summary>
            /// Get the name of column [ModifyUserID]
            /// </summary>
            public string ModifyUserID
            {
                get
                {
                    return "ModifyUserID";
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
            /// Get the name of column [Note]
            /// </summary>
            public string Note
            {
                get
                {
                    return "Note";
                }
            }
        }
        #endregion
    }
}
