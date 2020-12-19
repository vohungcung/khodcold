using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Keys")]
	[Serializable]
    public class Key : DBO<Key>
    {
		#region Fields
		
		private DateTime? _CreateDate;
		private int? _MaxNumber;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Key()
		{
			
		}
		
		/// <summary>
		/// Constructor with CreateDate 
		/// </summary>
		/// <param name="CreateDate">The CreateDate</param>
		public Key(DateTime? CreateDate )
		{
			this.CreateDate = CreateDate;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="CreateDate">Sets DateTime? value for CreateDate</param>
		/// <param name="MaxNumber">Sets int? value for MaxNumber</param>
		public Key(DateTime? createDate, int? maxNumber)
		{
			this.CreateDate = createDate;
			this.MaxNumber = maxNumber;
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
		/// Gets or sets int? value for MaxNumber
		/// </summary>
		[ColumnAttribute("MaxNumber", SqlDbType.Int , 4 , false, false, false)]
		public int? MaxNumber
		{
			set
			{
				this._MaxNumber = value;
			}
			get
			{
				return this._MaxNumber;
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
            /// Get the name of column [MaxNumber]
            /// </summary>
            public string MaxNumber
            {
                get
                {
                    return "MaxNumber";
                }
            }
        }
        #endregion
    }
}
