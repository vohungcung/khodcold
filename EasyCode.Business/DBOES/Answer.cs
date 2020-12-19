using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Answers")]
	[Serializable]
    public class Answer : DBO<Answer>
    {
		#region Fields
		
		private int? _QID;
		private int? _AID;
		private string _Title;
		private int? _Pos;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Answer()
		{
			
		}
		
		/// <summary>
		/// Constructor with AID 
		/// </summary>
		/// <param name="AID">The AID</param>
		public Answer(int? AID )
		{
			this.AID = AID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="QID">Sets int? value for QID</param>
		/// <param name="AID">Sets int? value for AID</param>
		/// <param name="Title">Sets string value for Title</param>
		/// <param name="Pos">Sets int? value for Pos</param>
		public Answer(int? qID, int? aID, string title, int? pos)
		{
			this.QID = qID;
			this.AID = aID;
			this.Title = title;
			this.Pos = pos;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets int? value for QID
		/// </summary>
		[ColumnAttribute("QID", SqlDbType.Int , 4 , false, false, false)]
		public int? QID
		{
			set
			{
				this._QID = value;
			}
			get
			{
				return this._QID;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for AID
		/// </summary>
		[ColumnAttribute("AID", SqlDbType.Int , 4 , true, true, false)]
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
		/// Gets or sets string value for Title
		/// </summary>
		[ColumnAttribute("Title", SqlDbType.NVarChar , 250 , false, false, false)]
		public string Title
		{
			set
			{
				this._Title = value;
			}
			get
			{
				return this._Title;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for Pos
		/// </summary>
		[ColumnAttribute("Pos", SqlDbType.Int , 4 , false, false, false)]
		public int? Pos
		{
			set
			{
				this._Pos = value;
			}
			get
			{
				return this._Pos;
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
            /// Get the name of column [QID]
            /// </summary>
            public string QID
            {
                get
                {
                    return "QID";
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
            /// Get the name of column [Title]
            /// </summary>
            public string Title
            {
                get
                {
                    return "Title";
                }
            }
            /// <summary>
            /// Get the name of column [Pos]
            /// </summary>
            public string Pos
            {
                get
                {
                    return "Pos";
                }
            }
        }
        #endregion
    }
}
