using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("ResultDetail")]
	[Serializable]
    public class ResultDetail : DBO<ResultDetail>
    {
		#region Fields
		
		private string _ResultID;
		private int? _AID;
		private string _Content;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public ResultDetail()
		{
			
		}
		
		/// <summary>
		/// Constructor with ResultID , ResultID 
		/// </summary>
		/// <param name="ResultID">The ResultID</param>
		/// <param name="AID">The AID</param>
		public ResultDetail(string ResultID , int? AID)
		{
			this.ResultID = ResultID;
			this.AID = AID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="ResultID">Sets string value for ResultID</param>
		/// <param name="AID">Sets int? value for AID</param>
		/// <param name="Content">Sets string value for Content</param>
		public ResultDetail(string resultID, int? aID, string content)
		{
			this.ResultID = resultID;
			this.AID = aID;
			this.Content = content;
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
		/// Gets or sets int? value for AID
		/// </summary>
		[ColumnAttribute("AID", SqlDbType.Int , 4 , true, false, false)]
		public int? AID
		{
			set
			{
				this._AID = value;
			}
			get
			{
				return this._AID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Content
		/// </summary>
		[ColumnAttribute("Content", SqlDbType.NText , 16 , false, false, false)]
		public string Content
		{
			set
			{
				this._Content = value;
			}
			get
			{
				return this._Content;
			}
		}
		
		
		/// <summary>
		/// Get a Answer of current ResultDetail object base on AID
		/// </summary>
		public Answer AnswerForA
		{
			get
			{
				if (this.AID == null)
					return null;
	
				Answer condition = new Answer(this.AID);
				return AnswerController.FindItem(condition);
			}
		}
		
		/// <summary>
		/// Get a ResultM of current ResultDetail object base on ResultID
		/// </summary>
		public ResultM ResultMForResult
		{
			get
			{
				if (this.ResultID == null)
					return null;
	
				ResultM condition = new ResultM(this.ResultID);
				return ResultMController.FindItem(condition);
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
            /// Get the name of column [AID]
            /// </summary>
            public string AID
            {
                get
                {
                    return "AID";
                }
            }
            /// <summary>
            /// Get the name of column [Content]
            /// </summary>
            public string Content
            {
                get
                {
                    return "Content";
                }
            }
        }
        #endregion
    }
}
