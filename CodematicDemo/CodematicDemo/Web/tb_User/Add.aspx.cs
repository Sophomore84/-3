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
namespace Maticsoft.Web.tb_User
{
    public partial class Add : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                       
        }

        		protected void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(this.txtUserID.Text.Trim().Length==0)
			{
				strErr+="UserID不能为空！\\n";	
			}
			if(this.txtPassword.Text.Trim().Length==0)
			{
				strErr+="Password不能为空！\\n";	
			}
			if(this.txtWorkType.Text.Trim().Length==0)
			{
				strErr+="WorkType不能为空！\\n";	
			}
			if(this.txtCreateTime.Text.Trim().Length==0)
			{
				strErr+="CreateTime不能为空！\\n";	
			}
			if(this.txtChangeTime.Text.Trim().Length==0)
			{
				strErr+="ChangeTime不能为空！\\n";	
			}

			if(strErr!="")
			{
				MessageBox.Show(this,strErr);
				return;
			}
			string UserID=this.txtUserID.Text;
			string Password=this.txtPassword.Text;
			string WorkType=this.txtWorkType.Text;
			string CreateTime=this.txtCreateTime.Text;
			string ChangeTime=this.txtChangeTime.Text;

			Maticsoft.Model.tb_User model=new Maticsoft.Model.tb_User();
			model.UserID=UserID;
			model.Password=Password;
			model.WorkType=WorkType;
			model.CreateTime=CreateTime;
			model.ChangeTime=ChangeTime;

			Maticsoft.BLL.tb_User bll=new Maticsoft.BLL.tb_User();
			bll.Add(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","add.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
