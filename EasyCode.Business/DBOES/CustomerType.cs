using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("CustomerTypes")]
	[Serializable]
    public class CustomerType : DBO<CustomerType>
    {
		#region Fields
		
		private string _CustomerTypeID;
		private string _CustomerTypeName;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public CustomerType()
		{
			
		}
		
		/// <summary>
		/// Constructor with CustomerTypeID 
		/// </summary>
		/// <param name="CustomerTypeID">The CustomerTypeID</param>
		public CustomerType(string CustomerTypeID )
		{
			this.CustomerTypeID = CustomerTypeID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="CustomerTypeID">Sets string value for CustomerTypeID</param>
		/// <param name="CustomerTypeName">Sets string value for CustomerTypeName</param>
		public CustomerType(string customerTypeID, string customerTypeName)
		{
			this.CustomerTypeID = customerTypeID;
			this.CustomerTypeName = customerTypeName;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for CustomerTypeID
		/// </summary>
		[ColumnAttribute("CustomerTypeID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string CustomerTypeID
		{
			set
			{
				this._CustomerTypeID = value;
			}
			get
			{
				return this._CustomerTypeID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for CustomerTypeName
		/// </summary>
		[ColumnAttribute("CustomerTypeName", SqlDbType.NVarChar , 250 , false, false, false)]
		public string CustomerTypeName
		{
			set
			{
				this._CustomerTypeName = value;
			}
			get
			{
				return this._CustomerTypeName;
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
            /// Get the name of column [CustomerTypeID]
            /// </summary>
            public string CustomerTypeID
            {
                get
                {
                    return "CustomerTypeID";
                }
            }
            /// <summary>
            /// Get the name of column [CustomerTypeName]
            /// </summary>
            public string CustomerTypeName
            {
                get
                {
                    return "CustomerTypeName";
                }
            }
        }
        #endregion
    }
}
