using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("VCustomers")]
	[Serializable]
    public class VCustomer : DBOView<VCustomer>
    {
		#region Fields
		
		private string _CustomerID;
		private string _CustomerName;
		private string _Address;
		private string _CustomerType;
		private string _Password;
		private string _Phone;
		private string _CustomerTypeName;
		private string _Area;
		private string _ZipCode;
		private bool? _ZO1;
		private bool? _ZOD;
		private bool? _ZO2;
		private bool? _ZOC;
		private string _Email;
		private bool? _IsMarketing;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public VCustomer()
		{
			
		}
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for CustomerID
		/// </summary>
		[ColumnAttribute("CustomerID", SqlDbType.NVarChar , 250 , false, false, false)]
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
		/// Gets or sets string value for CustomerName
		/// </summary>
		[ColumnAttribute("CustomerName", SqlDbType.NVarChar , 250 , false, false, false)]
		public string CustomerName
		{
			set
			{
				this._CustomerName = value;
			}
			get
			{
				return this._CustomerName;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Address
		/// </summary>
		[ColumnAttribute("Address", SqlDbType.NVarChar , 250 , false, false, false)]
		public string Address
		{
			set
			{
				this._Address = value;
			}
			get
			{
				return this._Address;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for CustomerType
		/// </summary>
		[ColumnAttribute("CustomerType", SqlDbType.NVarChar , 50 , false, false, false)]
		public string CustomerType
		{
			set
			{
				this._CustomerType = value;
			}
			get
			{
				return this._CustomerType;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Password
		/// </summary>
		[ColumnAttribute("Password", SqlDbType.NVarChar , 50 , false, false, false)]
		public string Password
		{
			set
			{
				this._Password = value;
			}
			get
			{
				return this._Password;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Phone
		/// </summary>
		[ColumnAttribute("Phone", SqlDbType.NVarChar , 50 , false, false, false)]
		public string Phone
		{
			set
			{
				this._Phone = value;
			}
			get
			{
				return this._Phone;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for CustomerTypeName
		/// </summary>
		[ColumnAttribute("CustomerTypeName", SqlDbType.NVarChar , 250 , false, false, true)]
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
		
		/// <summary>
		/// Gets or sets string value for Area
		/// </summary>
		[ColumnAttribute("Area", SqlDbType.NVarChar , 50 , false, false, true)]
		public string Area
		{
			set
			{
				this._Area = value;
			}
			get
			{
				return this._Area;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for ZipCode
		/// </summary>
		[ColumnAttribute("ZipCode", SqlDbType.NVarChar , 50 , false, false, true)]
		public string ZipCode
		{
			set
			{
				this._ZipCode = value;
			}
			get
			{
				return this._ZipCode;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for ZO1
		/// </summary>
		[ColumnAttribute("ZO1", SqlDbType.Bit , 1 , false, false, true)]
		public bool? ZO1
		{
			set
			{
				this._ZO1 = value;
			}
			get
			{
				return this._ZO1;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for ZOD
		/// </summary>
		[ColumnAttribute("ZOD", SqlDbType.Bit , 1 , false, false, true)]
		public bool? ZOD
		{
			set
			{
				this._ZOD = value;
			}
			get
			{
				return this._ZOD;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for ZO2
		/// </summary>
		[ColumnAttribute("ZO2", SqlDbType.Bit , 1 , false, false, true)]
		public bool? ZO2
		{
			set
			{
				this._ZO2 = value;
			}
			get
			{
				return this._ZO2;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for ZOC
		/// </summary>
		[ColumnAttribute("ZOC", SqlDbType.Bit , 1 , false, false, true)]
		public bool? ZOC
		{
			set
			{
				this._ZOC = value;
			}
			get
			{
				return this._ZOC;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Email
		/// </summary>
		[ColumnAttribute("Email", SqlDbType.NVarChar , 50 , false, false, true)]
		public string Email
		{
			set
			{
				this._Email = value;
			}
			get
			{
				return this._Email;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for IsMarketing
		/// </summary>
		[ColumnAttribute("IsMarketing", SqlDbType.Bit , 1 , false, false, true)]
		public bool? IsMarketing
		{
			set
			{
				this._IsMarketing = value;
			}
			get
			{
				return this._IsMarketing;
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
            /// Get the name of column [CustomerName]
            /// </summary>
            public string CustomerName
            {
                get
                {
                    return "CustomerName";
                }
            }
            /// <summary>
            /// Get the name of column [Address]
            /// </summary>
            public string Address
            {
                get
                {
                    return "Address";
                }
            }
            /// <summary>
            /// Get the name of column [CustomerType]
            /// </summary>
            public string CustomerType
            {
                get
                {
                    return "CustomerType";
                }
            }
            /// <summary>
            /// Get the name of column [Password]
            /// </summary>
            public string Password
            {
                get
                {
                    return "Password";
                }
            }
            /// <summary>
            /// Get the name of column [Phone]
            /// </summary>
            public string Phone
            {
                get
                {
                    return "Phone";
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
            /// <summary>
            /// Get the name of column [Area]
            /// </summary>
            public string Area
            {
                get
                {
                    return "Area";
                }
            }
            /// <summary>
            /// Get the name of column [ZipCode]
            /// </summary>
            public string ZipCode
            {
                get
                {
                    return "ZipCode";
                }
            }
            /// <summary>
            /// Get the name of column [ZO1]
            /// </summary>
            public string ZO1
            {
                get
                {
                    return "ZO1";
                }
            }
            /// <summary>
            /// Get the name of column [ZOD]
            /// </summary>
            public string ZOD
            {
                get
                {
                    return "ZOD";
                }
            }
            /// <summary>
            /// Get the name of column [ZO2]
            /// </summary>
            public string ZO2
            {
                get
                {
                    return "ZO2";
                }
            }
            /// <summary>
            /// Get the name of column [ZOC]
            /// </summary>
            public string ZOC
            {
                get
                {
                    return "ZOC";
                }
            }
            /// <summary>
            /// Get the name of column [Email]
            /// </summary>
            public string Email
            {
                get
                {
                    return "Email";
                }
            }
            /// <summary>
            /// Get the name of column [IsMarketing]
            /// </summary>
            public string IsMarketing
            {
                get
                {
                    return "IsMarketing";
                }
            }
        }
        #endregion
    }
}
