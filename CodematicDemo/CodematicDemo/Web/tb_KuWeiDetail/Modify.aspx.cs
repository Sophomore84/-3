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
namespace Maticsoft.Web.tb_KuWeiDetail
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
				string XuHao = "";
				if (Request.Params["id3"] != null && Request.Params["id3"].Trim() != "")
				{
					XuHao= Request.Params["id3"];
				}
				#warning 代码生成提示：显示页面,请检查确认该语句是否正确
				ShowInfo(HuoJiaHao,CengHao,LieHao,XuHao);
			}
		}
			
	private void ShowInfo(string HuoJiaHao,string CengHao,string LieHao,string XuHao)
	{
		Maticsoft.BLL.tb_KuWeiDetail bll=new Maticsoft.BLL.tb_KuWeiDetail();
		Maticsoft.Model.tb_KuWeiDetail model=bll.GetModel(HuoJiaHao,CengHao,LieHao,XuHao);
		this.lblHuoJiaHao.Text=model.HuoJiaHao;
		this.lblCengHao.Text=model.CengHao;
		this.lblLieHao.Text=model.LieHao;
		this.lblXuHao.Text=model.XuHao;
		this.txtRuKuTime.Text=model.RuKuTime;
		this.txtCaoZuoYuan.Text=model.CaoZuoYuan;

	}

		public void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(this.txtRuKuTime.Text.Trim().Length==0)
			{
				strErr+="RuKuTime不能为空！\\n";	
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
			string XuHao=this.lblXuHao.Text;
			string RuKuTime=this.txtRuKuTime.Text;
			string CaoZuoYuan=this.txtCaoZuoYuan.Text;


			Maticsoft.Model.tb_KuWeiDetail model=new Maticsoft.Model.tb_KuWeiDetail();
			model.HuoJiaHao=HuoJiaHao;
			model.CengHao=CengHao;
			model.LieHao=LieHao;
			model.XuHao=XuHao;
			model.RuKuTime=RuKuTime;
			model.CaoZuoYuan=CaoZuoYuan;

			Maticsoft.BLL.tb_KuWeiDetail bll=new Maticsoft.BLL.tb_KuWeiDetail();
			bll.Update(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","list.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
