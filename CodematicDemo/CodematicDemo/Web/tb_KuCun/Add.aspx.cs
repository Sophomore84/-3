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
namespace Maticsoft.Web.tb_KuCun
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
			if(this.txtShuLiang.Text.Trim().Length==0)
			{
				strErr+="ShuLiang不能为空！\\n";	
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
			string BianMa=this.txtBianMa.Text;
			string MingCheng=this.txtMingCheng.Text;
			string GuiGe=this.txtGuiGe.Text;
			string XingHao=this.txtXingHao.Text;
			string HeTongHao=this.txtHeTongHao.Text;
			string ShuLiang=this.txtShuLiang.Text;
			string X=this.txtX.Text;
			string Y=this.txtY.Text;
			string Z=this.txtZ.Text;
			string CaoZuoYuan=this.txtCaoZuoYuan.Text;

			Maticsoft.Model.tb_KuCun model=new Maticsoft.Model.tb_KuCun();
			model.HuoJiaHao=HuoJiaHao;
			model.CengHao=CengHao;
			model.LieHao=LieHao;
			model.BianMa=BianMa;
			model.MingCheng=MingCheng;
			model.GuiGe=GuiGe;
			model.XingHao=XingHao;
			model.HeTongHao=HeTongHao;
			model.ShuLiang=ShuLiang;
			model.X=X;
			model.Y=Y;
			model.Z=Z;
			model.CaoZuoYuan=CaoZuoYuan;

			Maticsoft.BLL.tb_KuCun bll=new Maticsoft.BLL.tb_KuCun();
			bll.Add(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","add.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
