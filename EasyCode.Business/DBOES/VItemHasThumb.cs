using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("VItemHasThumb")]
	[Serializable]
    public class VItemHasThumb : DBOView<VItemHasThumb>
    {
		#region Fields
		
		private string _ItemID;
		private string _ItemName;
		private string _ThumbImage;
		private string _Size;
		private decimal? _Unitprice;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public VItemHasThumb()
		{
			
		}
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for ItemID
		/// </summary>
		[ColumnAttribute("ItemID", SqlDbType.NVarChar , 9 , false, false, true)]
		public string ItemID
		{
			set
			{
				this._ItemID = value;
			}
			get
			{
				return this._ItemID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for ItemName
		/// </summary>
		[ColumnAttribute("ItemName", SqlDbType.NVarChar , 250 , false, false, true)]
		public string ItemName
		{
			set
			{
				this._ItemName = value;
			}
			get
			{
				return this._ItemName;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for ThumbImage
		/// </summary>
		[ColumnAttribute("ThumbImage", SqlDbType.NVarChar , 250 , false, false, true)]
		public string ThumbImage
		{
			set
			{
				this._ThumbImage = value;
			}
			get
			{
				return this._ThumbImage;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for Size
		/// </summary>
		[ColumnAttribute("Size", SqlDbType.NVarChar , 2 , false, false, true)]
		public string Size
		{
			set
			{
				this._Size = value;
			}
			get
			{
				return this._Size;
			}
		}
		
		/// <summary>
		/// Gets or sets decimal? value for Unitprice
		/// </summary>
		[ColumnAttribute("Unitprice", SqlDbType.Money , 8 , false, false, true)]
		public decimal? Unitprice
		{
			set
			{
				this._Unitprice = value;
			}
			get
			{
				return this._Unitprice;
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
            /// Get the name of column [ItemID]
            /// </summary>
            public string ItemID
            {
                get
                {
                    return "ItemID";
                }
            }
            /// <summary>
            /// Get the name of column [ItemName]
            /// </summary>
            public string ItemName
            {
                get
                {
                    return "ItemName";
                }
            }
            /// <summary>
            /// Get the name of column [ThumbImage]
            /// </summary>
            public string ThumbImage
            {
                get
                {
                    return "ThumbImage";
                }
            }
            /// <summary>
            /// Get the name of column [Size]
            /// </summary>
            public string Size
            {
                get
                {
                    return "Size";
                }
            }
            /// <summary>
            /// Get the name of column [Unitprice]
            /// </summary>
            public string Unitprice
            {
                get
                {
                    return "Unitprice";
                }
            }
        }
        #endregion
    }
}
