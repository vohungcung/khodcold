using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Topics")]
	[Serializable]
    public class Topic : DBO<Topic>
    {
		#region Fields
		
		private int? _TopicID;
		private string _Title;
		private string _Description;
		private bool? _Used;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Topic()
		{
			
		}
		
		/// <summary>
		/// Constructor with TopicID 
		/// </summary>
		/// <param name="TopicID">The TopicID</param>
		public Topic(int? TopicID )
		{
			this.TopicID = TopicID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="TopicID">Sets int? value for TopicID</param>
		/// <param name="Title">Sets string value for Title</param>
		/// <param name="Description">Sets string value for Description</param>
		/// <param name="Used">Sets bool? value for Used</param>
		public Topic(int? topicID, string title, string description, bool? used)
		{
			this.TopicID = topicID;
			this.Title = title;
			this.Description = description;
			this.Used = used;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets int? value for TopicID
		/// </summary>
		[ColumnAttribute("TopicID", SqlDbType.Int , 4 , true, true, false)]
		public int? TopicID
		{
			set
			{
				this._TopicID = value;
			}
			get
			{
				return this._TopicID;
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
		/// Gets or sets string value for Description
		/// </summary>
		[ColumnAttribute("Description", SqlDbType.NText , 16 , false, false, false)]
		public string Description
		{
			set
			{
				this._Description = value;
			}
			get
			{
				return this._Description;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for Used
		/// </summary>
		[ColumnAttribute("Used", SqlDbType.Bit , 1 , false, false, false)]
		public bool? Used
		{
			set
			{
				this._Used = value;
			}
			get
			{
				return this._Used;
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
            /// Get the name of column [TopicID]
            /// </summary>
            public string TopicID
            {
                get
                {
                    return "TopicID";
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
            /// Get the name of column [Description]
            /// </summary>
            public string Description
            {
                get
                {
                    return "Description";
                }
            }
            /// <summary>
            /// Get the name of column [Used]
            /// </summary>
            public string Used
            {
                get
                {
                    return "Used";
                }
            }
        }
        #endregion
    }
}
