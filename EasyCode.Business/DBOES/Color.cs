using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Colors")]
	[Serializable]
    public class Color : DBO<Color>
    {
		#region Fields
		
		private string _ColorID;
		private string _ColorName;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Color()
		{
			
		}
		
		/// <summary>
		/// Constructor with ColorID 
		/// </summary>
		/// <param name="ColorID">The ColorID</param>
		public Color(string ColorID )
		{
			this.ColorID = ColorID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="ColorID">Sets string value for ColorID</param>
		/// <param name="ColorName">Sets string value for ColorName</param>
		public Color(string colorID, string colorName)
		{
			this.ColorID = colorID;
			this.ColorName = colorName;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets string value for ColorID
		/// </summary>
		[ColumnAttribute("ColorID", SqlDbType.NVarChar , 50 , true, false, false)]
		public string ColorID
		{
			set
			{
				this._ColorID = value;
			}
			get
			{
				return this._ColorID;
			}
		}
		
		/// <summary>
		/// Gets or sets string value for ColorName
		/// </summary>
		[ColumnAttribute("ColorName", SqlDbType.NVarChar , 50 , false, false, false)]
		public string ColorName
		{
			set
			{
				this._ColorName = value;
			}
			get
			{
				return this._ColorName;
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
            /// Get the name of column [ColorID]
            /// </summary>
            public string ColorID
            {
                get
                {
                    return "ColorID";
                }
            }
            /// <summary>
            /// Get the name of column [ColorName]
            /// </summary>
            public string ColorName
            {
                get
                {
                    return "ColorName";
                }
            }
        }
        #endregion
    }
}
