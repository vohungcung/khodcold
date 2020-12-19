using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Customers")]
	[Serializable]
    public class Customer : DBO<Customer>
    {
		#region Fields
		
		private string _CustomerID;
		private string _CustomerName;
		private string _Address;
		private string _CustomerType;
		private string _Password;
		private string _Phone;
		private string _Email;
		private string _Area;
		private string _ZipCode;
		private bool? _ZO1;
		private bool? _ZOD;
		private bool? _ZO2;
		private bool? _ZOC;
		private bool? _IsMarketing;
		private string _District;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Customer()
		{
			
		}
		
		/// <summary>
		/// Constructor with CustomerID 
		/// </summary>
		/// <param name="CustomerID">The CustomerID</param>
		public Customer(string CustomerID )
		{
			this.CustomerID = CustomerID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="CustomerID">Sets string value for CustomerID</param>
		/// <param name="CustomerName">Sets string value for CustomerName</param>
		/// <param name="Address">Sets string value for Address</param>
		/// <param name="CustomerType">Sets string value for CustomerType</param>
		/// <param name="Password">Sets string value for Password</param>
		/// <param name="Phone">Sets string value for Phone</param>
		/// <param name="Email">Sets string value for Email</param>
		/// <param name="Area">Sets string value for Area</param>
		/// <param name="ZipCode">Sets string value for ZipCode</param>
		/// <param name="ZO1">Sets bool? value for ZO1</param>
		/// <param name="ZOD">Sets bool? value for ZOD</param>
		/// <param name="ZO2">Sets bool? value for ZO2</param>
		/// <param name="ZOC">Sets bool? value for ZOC</param>
		/// <param name="IsMarketing">Sets bool? value for IsMarketing</param>
		/// <param name="District">Sets string value for District</param>
		public Customer(string customerID, string customerName, string address, string customerType, string password, string phone, string email, string area, string zipCode, bool? zO1, bool? zOD, bool? zO2, bool? zOC, bool? isMarketing, string district)
		{
			this.CustomerID = customerID;
			this.CustomerName = customerName;
			this.Address = address;
			this.CustomerType = customerType;
			this.Password = password;
			this.Phone = phone;
			this.Email = email;
			this.Area = area;
			this.ZipCode = zipCode;
			this.ZO1 = zO1;
			this.ZOD = zOD;
			this.ZO2 = zO2;
			this.ZOC = zOC;
			this.IsMarketing = isMarketing;
			this.District = district;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for CustomerID
		/// </summary>
		[ColumnAttribute("CustomerID", SqlDbType.NVarChar , 250 , true, false, false)]
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
		
		/// <summary>
		/// Gets or sets string value for District
		/// </summary>
		[ColumnAttribute("District", SqlDbType.NVarChar , 250 , false, false, true)]
		public string District
		{
			set
			{
				this._District = value;
			}
			get
			{
				return this._District;
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
            /// Get the name of column [IsMarketing]
            /// </summary>
            public string IsMarketing
            {
                get
                {
                    return "IsMarketing";
                }
            }
            /// <summary>
            /// Get the name of column [District]
            /// </summary>
            public string District
            {
                get
                {
                    return "District";
                }
            }
        }
        #endregion
    }
}
