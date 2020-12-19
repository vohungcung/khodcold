using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("ResultMs")]
	[Serializable]
    public class ResultM : DBO<ResultM>
    {
		#region Fields
		
		private string _ResultID;
		private DateTime? _CreateDate;
		private string _EmployeeID;
		private int? _TopicID;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ResultM()
		{
			
		}
		
		/// <summary>
		/// Constructor with ResultID 
		/// </summary>
		/// <param name="ResultID">The ResultID</param>
		public ResultM(string ResultID )
		{
			this.ResultID = ResultID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="ResultID">Sets string value for ResultID</param>
		/// <param name="CreateDate">Sets DateTime? value for CreateDate</param>
		/// <param name="EmployeeID">Sets string value for EmployeeID</param>
		/// <param name="TopicID">Sets int? value for TopicID</param>
		public ResultM(string resultID, DateTime? createDate, string employeeID, int? topicID)
		{
			this.ResultID = resultID;
			this.CreateDate = createDate;
			this.EmployeeID = employeeID;
			this.TopicID = topicID;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for ResultID
		/// </summary>
		[ColumnAttribute("ResultID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string ResultID
		{
			set
			{
				this._ResultID = value;
			}
			get
			{
				return this._ResultID;
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
		/// Gets or sets string value for EmployeeID
		/// </summary>
		[ColumnAttribute("EmployeeID", SqlDbType.NVarChar , 50 , false, false, false)]
		public string EmployeeID
		{
			set
			{
				this._EmployeeID = value;
			}
			get
			{
				return this._EmployeeID;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for TopicID
		/// </summary>
		[ColumnAttribute("TopicID", SqlDbType.Int , 4 , false, false, false)]
		public int? TopicID
		{
			set
			{
				this._TopicID = value;
			}
			get
			{
				return this._TopicID;
			}
		}
		
		
		/// <summary>
		/// Get a list ResultDetail of current ResultM object base on ResultID
		/// </summary>
		public List<ResultDetail> ResultDetailListForResultID
		{
			get
			{
				if (this.ResultID == null)
					return null;
				ResultDetail condition = new ResultDetail();
				condition.ResultID = this.ResultID;
				return ResultDetailController.FindItems(condition);
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
            /// Get the name of column [ResultID]
            /// </summary>
            public string ResultID
            {
                get
                {
                    return "ResultID";
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
            /// Get the name of column [EmployeeID]
            /// </summary>
            public string EmployeeID
            {
                get
                {
                    return "EmployeeID";
                }
            }
            /// <summary>
            /// Get the name of column [TopicID]
            /// </summary>
            public string TopicID
            {
                get
                {
                    return "TopicID";
                }
            }
        }
        #endregion
    }
}
