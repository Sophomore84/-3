using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
namespace Maticsoft.Web.tb_KuCunRecords
{
    public partial class Show : Page
    {        
        		public string strid=""; 
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				string HuoJiaHao = "";
				if (Request.Params["id0"] != null && Request.Params["id0"].Trim() != "")
				{
					HuoJiaHao= Request.Params["id0"];
				}
				string CengHao = "";
				if (Request.Params["id1"] != null && Request.Params["id1"].Trim() != "")
				{
					CengHao= Request.Params["id1"];
				}
				string LieHao = "";
				if (Request.Params["id2"] != null && Request.Params["id2"].Trim() != "")
				{
					LieHao= Request.Params["id2"];
				}
				#warning 代码生成提示：显示页面,请检查确认该语句是否正确
				ShowInfo(HuoJiaHao,CengHao,LieHao);
			}
		}
		
	private void ShowInfo(string HuoJiaHao,string CengHao,string LieHao)
	{
		Maticsoft.BLL.tb_KuCunRecords bll=new Maticsoft.BLL.tb_KuCunRecords();
		Maticsoft.Model.tb_KuCunRecords model=bll.GetModel(HuoJiaHao,CengHao,LieHao);
		this.lblHuoJiaHao.Text=model.HuoJiaHao;
		this.lblCengHao.Text=model.CengHao;
		this.lblLieHao.Text=model.LieHao;
		this.lblBianMa.Text=model.BianMa;
		this.lblMingCheng.Text=model.MingCheng;
		this.lblGuiGe.Text=model.GuiGe;
		this.lblXingHao.Text=model.XingHao;
		this.lblHeTongHao.Text=model.HeTongHao;
		this.lblX.Text=model.X;
		this.lblY.Text=model.Y;
		this.lblZ.Text=model.Z;
		this.lblRuKuTime.Text=model.RuKuTime;
		this.lblChuKuTime.Text=model.ChuKuTime;
		this.lblCaoZuoYuan.Text=model.CaoZuoYuan;

	}


    }
}
