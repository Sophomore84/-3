using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// tb_KuWeiDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tb_KuWeiDetail
	{
		public tb_KuWeiDetail()
		{}
		#region Model
		private string _huojiahao;
		private string _cenghao;
		private string _liehao;
		private string _xuhao;
		private string _rukutime;
		private string _caozuoyuan;
		/// <summary>
		/// 
		/// </summary>
		public string HuoJiaHao
		{
			set{ _huojiahao=value;}
			get{return _huojiahao;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CengHao
		{
			set{ _cenghao=value;}
			get{return _cenghao;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string LieHao
		{
			set{ _liehao=value;}
			get{return _liehao;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string XuHao
		{
			set{ _xuhao=value;}
			get{return _xuhao;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RuKuTime
		{
			set{ _rukutime=value;}
			get{return _rukutime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string CaoZuoYuan
		{
			set{ _caozuoyuan=value;}
			get{return _caozuoyuan;}
		}
		#endregion Model

	}
}

