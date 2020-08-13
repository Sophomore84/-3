using System;
namespace Maticsoft.Model
{
	/// <summary>
	/// tb_WuPinDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class tb_WuPinDetail
	{
		public tb_WuPinDetail()
		{}
		#region Model
		private string _bianma;
		private string _mingcheng;
		private string _guige;
		private string _xinghao;
		private string _hetonghao;
		/// <summary>
		/// 
		/// </summary>
		public string BianMa
		{
			set{ _bianma=value;}
			get{return _bianma;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MingCheng
		{
			set{ _mingcheng=value;}
			get{return _mingcheng;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string GuiGe
		{
			set{ _guige=value;}
			get{return _guige;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string XingHao
		{
			set{ _xinghao=value;}
			get{return _xinghao;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HeTongHao
		{
			set{ _hetonghao=value;}
			get{return _hetonghao;}
		}
		#endregion Model

	}
}

