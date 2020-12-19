using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("ApproveLogs")]
	[Serializable]
    public class ApproveLog : DBO<ApproveLog>
    {
		#region Fields
		
		private DateTime? _CreateDate;
		private int? _CreateBy;
		private string _OrderID;
		private string _Note;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ApproveLog()
		{
			
		}
		
		/// <summary>
		/// Constructor with CreateDate , CreateDate , CreateDate 
		/// </summary>
		/// <param name="CreateDate">The CreateDate</param>
		/// <param name="CreateBy">The CreateBy</param>
		/// <param name="OrderID">The OrderID</param>
		public ApproveLog(DateTime? CreateDate , int? CreateBy, string OrderID)
		{
			this.CreateDate = CreateDate;
			this.CreateBy = CreateBy;
			this.OrderID = OrderID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="CreateDate">Sets DateTime? value for CreateDate</param>
		/// <param name="CreateBy">Sets int? value for CreateBy</param>
		/// <param name="OrderID">Sets string value for OrderID</param>
		/// <param name="Note">Sets string value for Note</param>
		public ApproveLog(DateTime? createDate, int? createBy, string orderID, string note)
		{
			this.CreateDate = createDate;
			this.CreateBy = createBy;
			this.OrderID = orderID;
			this.Note = note;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets DateTime? value for CreateDate
		/// </summary>
		[ColumnAttribute("CreateDate", SqlDbType.DateTime , 8 , true, false, false)]
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
		[ColumnAttribute("CreateBy", SqlDbType.Int , 4 , true, false, false)]
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
