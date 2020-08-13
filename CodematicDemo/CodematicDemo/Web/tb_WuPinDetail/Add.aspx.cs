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
namespace Maticsoft.Web.tb_WuPinDetail
{
    public partial class Add : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                       
        }

        		protected void btnSave_Click(object sender, EventArgs e)
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

			if(strErr!="")
			{
				MessageBox.Show(this,strErr);
				return;
			}
			string BianMa=this.txtBianMa.Text;
			string MingCheng=this.txtMingCheng.Text;
			string GuiGe=this.txtGuiGe.Text;
			string XingHao=this.txtXingHao.Text;
			string HeTongHao=this.txtHeTongHao.Text;

			Maticsoft.Model.tb_WuPinDetail model=new Maticsoft.Model.tb_WuPinDetail();
			model.BianMa=BianMa;
			model.MingCheng=MingCheng;
			model.GuiGe=GuiGe;
			model.XingHao=XingHao;
			model.HeTongHao=HeTongHao;

			Maticsoft.BLL.tb_WuPinDetail bll=new Maticsoft.BLL.tb_WuPinDetail();
			bll.Add(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","add.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
