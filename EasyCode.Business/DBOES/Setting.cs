using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Settings")]
	[Serializable]
    public class Setting : DBO<Setting>
    {
		#region Fields
		
		private int? _ID;
		private string _SMTP;
		private string _EmailSender;
		private string _Password;
		private int? _Port;
		private string _EmailReceiver;
		private string _Domain;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Setting()
		{
			
		}
		
		/// <summary>
		/// Constructor with ID 
		/// </summary>
		/// <param name="ID">The ID</param>
		public Setting(int? ID )
		{
			this.ID = ID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="ID">Sets int? value for ID</param>
		/// <param name="SMTP">Sets string value for SMTP</param>
		/// <param name="EmailSender">Sets string value for EmailSender</param>
		/// <param name="Password">Sets string value for Password</param>
		/// <param name="Port">Sets int? value for Port</param>
		/// <param name="EmailReceiver">Sets string value for EmailReceiver</param>
		/// <param name="Domain">Sets string value for Domain</param>
		public Setting(int? iD, string sMTP, string emailSender, string password, int? port, string emailReceiver, string domain)
		{
			this.ID = iD;
			this.SMTP = sMTP;
			this.EmailSender = emailSender;
			this.Password = password;
			this.Port = port;
			this.EmailReceiver = emailReceiver;
			this.Domain = domain;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets int? value for ID
		/// </summary>
		[ColumnAttribute("ID", SqlDbType.Int , 4 , true, true, false)]
		public int? ID
		{
			set
			{
				this._ID = value;
			}
			get
			{
				return this._ID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for SMTP
		/// </summary>
		[ColumnAttribute("SMTP", SqlDbType.NVarChar , 250 , false, false, false)]
		public string SMTP
		{
			set
			{
				this._SMTP = value;
			}
			get
			{
				return this._SMTP;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for EmailSender
		/// </summary>
		[ColumnAttribute("EmailSender", SqlDbType.NVarChar , 50 , false, false, false)]
		public string EmailSender
		{
			set
			{
				this._EmailSender = value;
			}
			get
			{
				return this._EmailSender;
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
		/// Gets or sets int? value for Port
		/// </summary>
		[ColumnAttribute("Port", SqlDbType.Int , 4 , false, false, false)]
		public int? Port
		{
			set
			{
				this._Port = value;
			}
			get
			{
				return this._Port;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for EmailReceiver
		/// </summary>
		[ColumnAttribute("EmailReceiver", SqlDbType.NVarChar , 50 , false, false, false)]
		public string EmailReceiver
		{
			set
			{
				this._EmailReceiver = value;
			}
			get
			{
				return this._EmailReceiver;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Domain
		/// </summary>
		[ColumnAttribute("Domain", SqlDbType.NVarChar , 250 , false, false, false)]
		public string Domain
		{
			set
			{
				this._Domain = value;
			}
			get
			{
				return this._Domain;
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
            /// Get the name of column [ID]
            /// </summary>
            public string ID
            {
                get
                {
                    return "ID";
                }
            }
            /// <summary>
            /// Get the name of column [SMTP]
            /// </summary>
            public string SMTP
            {
                get
                {
                    return "SMTP";
                }
            }
            /// <summary>
            /// Get the name of column [EmailSender]
            /// </summary>
            public string EmailSender
            {
                get
                {
                    return "EmailSender";
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
            /// Get the name of column [Port]
            /// </summary>
            public string Port
            {
                get
                {
                    return "Port";
                }
            }
            /// <summary>
            /// Get the name of column [EmailReceiver]
            /// </summary>
            public string EmailReceiver
            {
                get
                {
                    return "EmailReceiver";
                }
            }
            /// <summary>
            /// Get the name of column [Domain]
            /// </summary>
            public string Domain
            {
                get
                {
                    return "Domain";
                }
            }
        }
        #endregion
    }
}
