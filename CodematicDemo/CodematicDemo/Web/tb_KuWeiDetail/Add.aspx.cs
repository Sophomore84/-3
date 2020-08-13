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
    public partial class Add : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                       
        }

        		protected void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(this.txtHuoJiaHao.Text.Trim().Length==0)
			{
				strErr+="HuoJiaHao不能为空！\\n";	
			}
			if(this.txtCengHao.Text.Trim().Length==0)
			{
				strErr+="CengHao不能为空！\\n";	
			}
			if(this.txtLieHao.Text.Trim().Length==0)
			{
				strErr+="LieHao不能为空！\\n";	
			}
			if(this.txtXuHao.Text.Trim().Length==0)
			{
				strErr+="XuHao不能为空！\\n";	
			}
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
			string HuoJiaHao=this.txtHuoJiaHao.Text;
			string CengHao=this.txtCengHao.Text;
			string LieHao=this.txtLieHao.Text;
			string XuHao=this.txtXuHao.Text;
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
			bll.Add(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","add.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
