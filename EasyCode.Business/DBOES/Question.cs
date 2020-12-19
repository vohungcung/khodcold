using System;
using System.Data;
using System.Collections.Generic;
using EasyCode.Core;
using EasyCode.Utility;

namespace EasyCode.Business
{
    [TableAttribute("Questions")]
	[Serializable]
    public class Question : DBO<Question>
    {
		#region Fields
		
		private int? _TopicID;
		private int? _QID;
		private string _Title;
		private string _Content;
		private int? _Pos;
		private int? _QT;
		private bool? _Require;
		private int? _ParentID;
		
		#endregion
		
		#region Constructors
		/// <summary>
		/// Default constructor
		/// </summary>
		public Question()
		{
			
		}
		
		/// <summary>
		/// Constructor with QID 
		/// </summary>
		/// <param name="QID">The QID</param>
		public Question(int? QID )
		{
			this.QID = QID;
		}
		
		/// <summary>
		/// Constructor with all parameters
		/// </summary>
		/// <param name="TopicID">Sets int? value for TopicID</param>
		/// <param name="QID">Sets int? value for QID</param>
		/// <param name="Title">Sets string value for Title</param>
		/// <param name="Content">Sets string value for Content</param>
		/// <param name="Pos">Sets int? value for Pos</param>
		/// <param name="QT">Sets int? value for QT</param>
		/// <param name="Require">Sets bool? value for Require</param>
		/// <param name="ParentID">Sets int? value for ParentID</param>
		public Question(int? topicID, int? qID, string title, string content, int? pos, int? qT, bool? require, int? parentID)
		{
			this.TopicID = topicID;
			this.QID = qID;
			this.Title = title;
			this.Content = content;
			this.Pos = pos;
			this.QT = qT;
			this.Require = require;
			this.ParentID = parentID;
		}
		
		#endregion
		
		#region Properties
		
		/// <summary>
		/// Gets or sets int? value for TopicID
		/// </summary>
		[ColumnAttribute("TopicID", SqlDbType.Int , 4 , false, false, false)]
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
		/// Gets or sets int? value for QID
		/// </summary>
		[ColumnAttribute("QID", SqlDbType.Int , 4 , true, true, false)]
		public int? QID
		{
			set
			{
				this._QID = value;
			}
			get
			{
				return this._QID;
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
		/// Gets or sets string value for Content
		/// </summary>
		[ColumnAttribute("Content", SqlDbType.NText , 16 , false, false, false)]
		public string Content
		{
			set
			{
				this._Content = value;
			}
			get
			{
				return this._Content;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for Pos
		/// </summary>
		[ColumnAttribute("Pos", SqlDbType.Int , 4 , false, false, false)]
		public int? Pos
		{
			set
			{
				this._Pos = value;
			}
			get
			{
				return this._Pos;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for QT
		/// </summary>
		[ColumnAttribute("QT", SqlDbType.Int , 4 , false, false, false)]
		public int? QT
		{
			set
			{
				this._QT = value;
			}
			get
			{
				return this._QT;
			}
		}
		
		/// <summary>
		/// Gets or sets bool? value for Require
		/// </summary>
		[ColumnAttribute("Require", SqlDbType.Bit , 1 , false, false, true)]
		public bool? Require
		{
			set
			{
				this._Require = value;
			}
			get
			{
				return this._Require;
			}
		}
		
		/// <summary>
		/// Gets or sets int? value for ParentID
		/// </summary>
		[ColumnAttribute("ParentID", SqlDbType.Int , 4 , false, false, true)]
		public int? ParentID
		{
			set
			{
				this._ParentID = value;
			}
			get
			{
				return this._ParentID;
			}
		}
		
		
		/// <summary>
		/// Get a list Answer of current Question object base on QID
		/// </summary>
		public List<Answer> AnswerListForQID
		{
			get
			{
				if (this.QID == null)
					return null;
				Answer condition = new Answer();
				condition.QID = this.QID;
				return AnswerController.FindItems(condition);
			}
		}
		
		/// <summary>
		/// Get a Topic of current Question object base on TopicID
		/// </summary>
		public Topic TopicForTopic
		{
			get
			{
				if (this.TopicID == null)
					return null;
	
				Topic condition = new Topic(this.TopicID);
				return TopicController.FindItem(condition);
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
            /// Get the name of column [QID]
            /// </summary>
            public string QID
            {
                get
                {
                    return "QID";
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
            /// Get the name of column [Content]
            /// </summary>
            public string Content
            {
                get
                {
                    return "Content";
                }
            }
            /// <summary>
            /// Get the name of column [Pos]
            /// </summary>
            public string Pos
            {
                get
                {
                    return "Pos";
                }
            }
            /// <summary>
            /// Get the name of column [QT]
            /// </summary>
            public string QT
            {
                get
                {
                    return "QT";
                }
            }
            /// <summary>
            /// Get the name of column [Require]
            /// </summary>
            public string Require
            {
                get
                {
                    return "Require";
                }
            }
            /// <summary>
            /// Get the name of column [ParentID]
            /// </summary>
            public string ParentID
            {
                get
                {
                    return "ParentID";
                }
            }
        }
        #endregion
    }
}
