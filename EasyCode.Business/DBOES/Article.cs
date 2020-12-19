using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Article")]
	[Serializable]
    public class Article : DBO<Article>
    {
		#region Fields
		
		private string _MaBC;
		private string _MaHH;
		private string _TenHH;
		private string _MaNhom;
		private string _Ma_Nhom;
		private string _HangCham;
		private string _DVT;
		private decimal? _DonGia;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Article()
		{
			
		}
		
		/// <summary>
		/// Constructor with MaHH 
		/// </summary>
		/// <param name="MaHH">The MaHH</param>
		public Article(string MaHH )
		{
			this.MaHH = MaHH;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="MaBC">Sets string value for MaBC</param>
		/// <param name="MaHH">Sets string value for MaHH</param>
		/// <param name="TenHH">Sets string value for TenHH</param>
		/// <param name="MaNhom">Sets string value for MaNhom</param>
		/// <param name="Ma_Nhom">Sets string value for Ma_Nhom</param>
		/// <param name="HangCham">Sets string value for HangCham</param>
		/// <param name="DVT">Sets string value for DVT</param>
		/// <param name="DonGia">Sets decimal? value for DonGia</param>
		public Article(string maBC, string maHH, string tenHH, string maNhom, string ma_Nhom, string hangCham, string dVT, decimal? donGia)
		{
			this.MaBC = maBC;
			this.MaHH = maHH;
			this.TenHH = tenHH;
			this.MaNhom = maNhom;
			this.Ma_Nhom = ma_Nhom;
			this.HangCham = hangCham;
			this.DVT = dVT;
			this.DonGia = donGia;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for MaBC
		/// </summary>
		[ColumnAttribute("MaBC", SqlDbType.VarChar , 18 , false, false, false)]
		public string MaBC
		{
			set
			{
				this._MaBC = value;
			}
			get
			{
				return this._MaBC;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for MaHH
		/// </summary>
		[ColumnAttribute("MaHH", SqlDbType.NVarChar , 14 , true, false, false)]
		public string MaHH
		{
			set
			{
				this._MaHH = value;
			}
			get
			{
				return this._MaHH;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for TenHH
		/// </summary>
		[ColumnAttribute("TenHH", SqlDbType.NVarChar , 250 , false, false, true)]
		public string TenHH
		{
			set
			{
				this._TenHH = value;
			}
			get
			{
				return this._TenHH;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for MaNhom
		/// </summary>
		[ColumnAttribute("MaNhom", SqlDbType.VarChar , 3 , false, false, true)]
		public string MaNhom
		{
			set
			{
				this._MaNhom = value;
			}
			get
			{
				return this._MaNhom;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Ma_Nhom
		/// </summary>
		[ColumnAttribute("Ma_Nhom", SqlDbType.VarChar , 2 , false, false, true)]
		public string Ma_Nhom
		{
			set
			{
				this._Ma_Nhom = value;
			}
			get
			{
				return this._Ma_Nhom;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for HangCham
		/// </summary>
		[ColumnAttribute("HangCham", SqlDbType.VarChar , 2 , false, false, true)]
		public string HangCham
		{
			set
			{
				this._HangCham = value;
			}
			get
			{
				return this._HangCham;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for DVT
		/// </summary>
		[ColumnAttribute("DVT", SqlDbType.VarChar , 3 , false, false, true)]
		public string DVT
		{
			set
			{
				this._DVT = value;
			}
			get
			{
				return this._DVT;
			}
		}
		
		/// <summary>
		/// Gets or sets decimal? value for DonGia
		/// </summary>
		[ColumnAttribute("DonGia", SqlDbType.Decimal , 9 , false, false, true)]
		public decimal? DonGia
		{
			set
			{
				this._DonGia = value;
			}
			get
			{
				return this._DonGia;
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
            /// Get the name of column [MaBC]
            /// </summary>
            public string MaBC
            {
                get
                {
                    return "MaBC";
                }
            }
            /// <summary>
            /// Get the name of column [MaHH]
            /// </summary>
            public string MaHH
            {
                get
                {
                    return "MaHH";
                }
            }
            /// <summary>
            /// Get the name of column [TenHH]
            /// </summary>
            public string TenHH
            {
                get
                {
                    return "TenHH";
                }
            }
            /// <summary>
            /// Get the name of column [MaNhom]
            /// </summary>
            public string MaNhom
            {
                get
                {
                    return "MaNhom";
                }
            }
            /// <summary>
            /// Get the name of column [Ma_Nhom]
            /// </summary>
            public string Ma_Nhom
            {
                get
                {
                    return "Ma_Nhom";
                }
            }
            /// <summary>
            /// Get the name of column [HangCham]
            /// </summary>
            public string HangCham
            {
                get
                {
                    return "HangCham";
                }
            }
            /// <summary>
            /// Get the name of column [DVT]
            /// </summary>
            public string DVT
            {
                get
                {
                    return "DVT";
                }
            }
            /// <summary>
            /// Get the name of column [DonGia]
            /// </summary>
            public string DonGia
            {
                get
                {
                    return "DonGia";
                }
            }
        }
        #endregion
    }
}
