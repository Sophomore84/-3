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
using Maticsoft.Common;
using LTP.Accounts.Bus;
namespace Maticsoft.Web.tb_KuCunRecords
{
    public partial class Modify : Page
    {       

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
		this.txtBianMa.Text=model.BianMa;
		this.txtMingCheng.Text=model.MingCheng;
		this.txtGuiGe.Text=model.GuiGe;
		this.txtXingHao.Text=model.XingHao;
		this.txtHeTongHao.Text=model.HeTongHao;
		this.txtX.Text=model.X;
		this.txtY.Text=model.Y;
		this.txtZ.Text=model.Z;
		this.txtRuKuTime.Text=model.RuKuTime;
		this.txtChuKuTime.Text=model.ChuKuTime;
		this.txtCaoZuoYuan.Text=model.CaoZuoYuan;

	}

		public void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(this.txtBianMa.Text.Trim().Length==0)
			{
				strErr+="BianMa不能为空！\\n";	
			}
			if(this.txtMingCheng.Text.Trim().Length==0)
			{
				strErr+="MingCheng不能为空！\\n";	
			}
			if(this.txtGuiGe.Text.Trim().Length==0)
			{
				strErr+="GuiGe不能为空！\\n";	
			}
			if(this.txtXingHao.Text.Trim().Length==0)
			{
				strErr+="XingHao不能为空！\\n";	
			}
			if(this.txtHeTongHao.Text.Trim().Length==0)
			{
				strErr+="HeTongHao不能为空！\\n";	
			}
			if(this.txtX.Text.Trim().Length==0)
			{
				strErr+="X不能为空！\\n";	
			}
			if(this.txtY.Text.Trim().Length==0)
			{
				strErr+="Y不能为空！\\n";	
			}
			if(this.txtZ.Text.Trim().Length==0)
			{
				strErr+="Z不能为空！\\n";	
			}
			if(this.txtRuKuTime.Text.Trim().Length==0)
			{
				strErr+="RuKuTime不能为空！\\n";	
			}
			if(this.txtChuKuTime.Text.Trim().Length==0)
			{
				strErr+="ChuKuTime不能为空！\\n";	
			}
			if(this.txtCaoZuoYuan.Text.Trim().Length==0)
			{
				strErr+="CaoZuoYuan不能为空！\\n";	
			}

			if(strErr!="")
			{
				MessageBox.Show(this,strErr);
				return;
			}
			string HuoJiaHao=this.lblHuoJiaHao.Text;
			string CengHao=this.lblCengHao.Text;
			string LieHao=this.lblLieHao.Text;
			string BianMa=this.txtBianMa.Text;
			string MingCheng=this.txtMingCheng.Text;
			string GuiGe=this.txtGuiGe.Text;
			string XingHao=this.txtXingHao.Text;
			string HeTongHao=this.txtHeTongHao.Text;
			string X=this.txtX.Text;
			string Y=this.txtY.Text;
			string Z=this.txtZ.Text;
			string RuKuTime=this.txtRuKuTime.Text;
			string ChuKuTime=this.txtChuKuTime.Text;
			string CaoZuoYuan=this.txtCaoZuoYuan.Text;


			Maticsoft.Model.tb_KuCunRecords model=new Maticsoft.Model.tb_KuCunRecords();
			model.HuoJiaHao=HuoJiaHao;
			model.CengHao=CengHao;
			model.LieHao=LieHao;
			model.BianMa=BianMa;
			model.MingCheng=MingCheng;
			model.GuiGe=GuiGe;
			model.XingHao=XingHao;
			model.HeTongHao=HeTongHao;
			model.X=X;
			model.Y=Y;
			model.Z=Z;
			model.RuKuTime=RuKuTime;
			model.ChuKuTime=ChuKuTime;
			model.CaoZuoYuan=CaoZuoYuan;

			Maticsoft.BLL.tb_KuCunRecords bll=new Maticsoft.BLL.tb_KuCunRecords();
			bll.Update(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","list.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
