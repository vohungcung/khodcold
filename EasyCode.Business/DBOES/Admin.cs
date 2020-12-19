using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Admins")]
	[Serializable]
    public class Admin : DBO<Admin>
    {
		#region Fields
		
		private int? _AdminID;
		private string _FullName;
		private string _Phone;
		private string _UserName;
		private string _PassWord;
		private string _Email;
		private bool? _IsAdmin;
		private string _ZIPCode;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Admin()
		{
			
		}
		
		/// <summary>
		/// Constructor with AdminID 
		/// </summary>
		/// <param name="AdminID">The AdminID</param>
		public Admin(int? AdminID )
		{
			this.AdminID = AdminID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="AdminID">Sets int? value for AdminID</param>
		/// <param name="FullName">Sets string value for FullName</param>
		/// <param name="Phone">Sets string value for Phone</param>
		/// <param name="UserName">Sets string value for UserName</param>
		/// <param name="PassWord">Sets string value for PassWord</param>
		/// <param name="Email">Sets string value for Email</param>
		/// <param name="IsAdmin">Sets bool? value for IsAdmin</param>
		/// <param name="ZIPCode">Sets string value for ZIPCode</param>
		public Admin(int? adminID, string fullName, string phone, string userName, string passWord, string email, bool? isAdmin, string zIPCode)
		{
			this.AdminID = adminID;
			this.FullName = fullName;
			this.Phone = phone;
			this.UserName = userName;
			this.PassWord = passWord;
			this.Email = email;
			this.IsAdmin = isAdmin;
			this.ZIPCode = zIPCode;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets int? value for AdminID
		/// </summary>
		[ColumnAttribute("AdminID", SqlDbType.Int , 4 , true, true, false)]
		public int? AdminID
		{
			set
			{
				this._AdminID = value;
			}
			get
			{
				return this._AdminID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for FullName
		/// </summary>
		[ColumnAttribute("FullName", SqlDbType.NVarChar , 250 , false, false, false)]
		public string FullName
		{
			set
			{
				this._FullName = value;
			}
			get
			{
				return this._FullName;
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
		/// Gets or sets string value for UserName
		/// </summary>
		[ColumnAttribute("UserName", SqlDbType.NVarChar , 100 , false, false, false)]
		public string UserName
		{
			set
			{
				this._UserName = value;
			}
			get
			{
				return this._UserName;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for PassWord
		/// </summary>
		[ColumnAttribute("PassWord", SqlDbType.NVarChar , 1000 , false, false, false)]
		public string PassWord
		{
			set
			{
				this._PassWord = value;
			}
			get
			{
				return this._PassWord;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Email
		/// </summary>
		[ColumnAttribute("Email", SqlDbType.NVarChar , 255 , false, false, false)]
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
		/// Gets or sets bool? value for IsAdmin
		/// </summary>
		[ColumnAttribute("IsAdmin", SqlDbType.Bit , 1 , false, false, false)]
		public bool? IsAdmin
		{
			set
			{
				this._IsAdmin = value;
			}
			get
			{
				return this._IsAdmin;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for ZIPCode
		/// </summary>
		[ColumnAttribute("ZIPCode", SqlDbType.NVarChar , 50 , false, false, true)]
		public string ZIPCode
		{
			set
			{
				this._ZIPCode = value;
			}
			get
			{
				return this._ZIPCode;
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
            /// Get the name of column [AdminID]
            /// </summary>
            public string AdminID
            {
                get
                {
                    return "AdminID";
                }
            }
            /// <summary>
            /// Get the name of column [FullName]
            /// </summary>
            public string FullName
            {
                get
                {
                    return "FullName";
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
            /// Get the name of column [UserName]
            /// </summary>
            public string UserName
            {
                get
                {
                    return "UserName";
                }
            }
            /// <summary>
            /// Get the name of column [PassWord]
            /// </summary>
            public string PassWord
            {
                get
                {
                    return "PassWord";
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
            /// Get the name of column [IsAdmin]
            /// </summary>
            public string IsAdmin
            {
                get
                {
                    return "IsAdmin";
                }
            }
            /// <summary>
            /// Get the name of column [ZIPCode]
            /// </summary>
            public string ZIPCode
            {
                get
                {
                    return "ZIPCode";
                }
            }
        }
        #endregion
    }
}
