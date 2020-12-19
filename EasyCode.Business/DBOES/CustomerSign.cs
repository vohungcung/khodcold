using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("CustomerSigns")]
	[Serializable]
    public class CustomerSign : DBO<CustomerSign>
    {
		#region Fields
		
		private string _CustomerID;
		private byte[] _SignImage;
		private string _Ext;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public CustomerSign()
		{
			
		}
		
		/// <summary>
		/// Constructor with CustomerID 
		/// </summary>
		/// <param name="CustomerID">The CustomerID</param>
		public CustomerSign(string CustomerID )
		{
			this.CustomerID = CustomerID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="CustomerID">Sets string value for CustomerID</param>
		/// <param name="SignImage">Sets byte[] value for SignImage</param>
		/// <param name="Ext">Sets string value for Ext</param>
		public CustomerSign(string customerID, byte[] signImage, string ext)
		{
			this.CustomerID = customerID;
			this.SignImage = signImage;
			this.Ext = ext;
		}
		
		#endregion
		
		#region Properties
		
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
		/// Gets or sets byte[] value for SignImage
		/// </summary>
		[ColumnAttribute("SignImage", SqlDbType.Image , 16 , false, false, false)]
		public byte[] SignImage
		{
			set
			{
				this._SignImage = value;
			}
			get
			{
				return this._SignImage;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Ext
		/// </summary>
		[ColumnAttribute("Ext", SqlDbType.NVarChar , 5 , false, false, true)]
		public string Ext
		{
			set
			{
				this._Ext = value;
			}
			get
			{
				return this._Ext;
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
            /// Get the name of column [SignImage]
            /// </summary>
            public string SignImage
            {
                get
                {
                    return "SignImage";
                }
            }
            /// <summary>
            /// Get the name of column [Ext]
            /// </summary>
            public string Ext
            {
                get
                {
                    return "Ext";
                }
            }
        }
        #endregion
    }
}
