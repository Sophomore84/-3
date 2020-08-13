using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// tb_UserLoginRecords:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tb_UserLoginRecords
	{
		public tb_UserLoginRecords()
		{}
		#region Model
		private string _userid;
		private string _action;
		private string _datetime;
		private string _worktype;
		/// <summary>
		/// 
		/// </summary>
		public string UserID
		{
			set{ _userid=value;}
			get{return _userid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Action
		{
			set{ _action=value;}
			get{return _action;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DateTime
		{
			set{ _datetime=value;}
			get{return _datetime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WorkType
		{
			set{ _worktype=value;}
			get{return _worktype;}
		}
		#endregion Model

	}
}

