using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("l")]
	[Serializable]
    public class L : DBO<L>
    {
		#region Fields
		
		private int? _Id;
		private string _Content;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public L()
		{
			
		}
		
		/// <summary>
		/// Constructor with Id 
		/// </summary>
		/// <param name="Id">The Id</param>
		public L(int? id )
		{
			this.Id = id;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="id">Sets int? value for Id</param>
		/// <param name="content">Sets string value for Content</param>
		public L(int? id, string content)
		{
			this.Id = id;
			this.Content = content;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets int? value for Id
		/// </summary>
		[ColumnAttribute("Id", SqlDbType.Int , 4 , true, true, false)]
		public int? Id
		{
			set
			{
				this._Id = value;
			}
			get
			{
				return this._Id;
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
            /// Get the name of column [Id]
            /// </summary>
            public string Id
            {
                get
                {
                    return "Id";
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
