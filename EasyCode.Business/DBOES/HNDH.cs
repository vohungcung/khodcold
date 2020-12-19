using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("HNDH")]
	[Serializable]
    public class HNDH : DBO<HNDH>
    {
		#region Fields
		
		private string _HNID;
		private string _Title;
		private DateTime? _VoucherDate;
		private string _Site;
		private int? _Status;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public HNDH()
		{
			
		}
		
		/// <summary>
		/// Constructor with HNID 
		/// </summary>
		/// <param name="HNID">The HNID</param>
		public HNDH(string HNID )
		{
			this.HNID = HNID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="HNID">Sets string value for HNID</param>
		/// <param name="Title">Sets string value for Title</param>
		/// <param name="VoucherDate">Sets DateTime? value for VoucherDate</param>
		/// <param name="Site">Sets string value for Site</param>
		/// <param name="Status">Sets int? value for Status</param>
		public HNDH(string hNID, string title, DateTime? voucherDate, string site, int? status)
		{
			this.HNID = hNID;
			this.Title = title;
			this.VoucherDate = voucherDate;
			this.Site = site;
			this.Status = status;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for HNID
		/// </summary>
		[ColumnAttribute("HNID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string HNID
		{
			set
			{
				this._HNID = value;
			}
			get
			{
				return this._HNID;
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
		/// Gets or sets DateTime? value for VoucherDate
		/// </summary>
		[ColumnAttribute("VoucherDate", SqlDbType.DateTime , 8 , false, false, false)]
		public DateTime? VoucherDate
		{
			set
			{
				this._VoucherDate = value;
			}
			get
			{
				return this._VoucherDate;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Site
		/// </summary>
		[ColumnAttribute("Site", SqlDbType.NVarChar , 50 , false, false, false)]
		public string Site
		{
			set
			{
				this._Site = value;
			}
			get
			{
				return this._Site;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for Status
		/// </summary>
		[ColumnAttribute("Status", SqlDbType.Int , 4 , false, false, false)]
		public int? Status
		{
			set
			{
				this._Status = value;
			}
			get
			{
				return this._Status;
			}
		}
		
		
		/// <summary>
		/// Get a list ItemHN of current HNDH object base on HNID
		/// </summary>
		public List<ItemHN> ItemHNListForHNID
		{
			get
			{
				if (this.HNID == null)
					return null;
				ItemHN condition = new ItemHN();
				condition.HNID = this.HNID;
				return ItemHNController.FindItems(condition);
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
            /// Get the name of column [HNID]
            /// </summary>
            public string HNID
            {
                get
                {
                    return "HNID";
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
            /// Get the name of column [VoucherDate]
            /// </summary>
            public string VoucherDate
            {
                get
                {
                    return "VoucherDate";
                }
            }
            /// <summary>
            /// Get the name of column [Site]
            /// </summary>
            public string Site
            {
                get
                {
                    return "Site";
                }
            }
            /// <summary>
            /// Get the name of column [Status]
            /// </summary>
            public string Status
            {
                get
                {
                    return "Status";
                }
            }
        }
        #endregion
    }
}
