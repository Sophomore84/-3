using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// tb_User:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tb_User
	{
		public tb_User()
		{}
		#region Model
		private string _userid;
		private string _password;
		private string _worktype;
		private string _createtime;
		private string _changetime;
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
		public string Password
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string WorkType
		{
			set{ _worktype=value;}
			get{return _worktype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CreateTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ChangeTime
		{
			set{ _changetime=value;}
			get{return _changetime;}
		}
		#endregion Model

	}
}

