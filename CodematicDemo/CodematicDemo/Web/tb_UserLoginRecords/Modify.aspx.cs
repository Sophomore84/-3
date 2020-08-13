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
namespace Maticsoft.Web.tb_UserLoginRecords
{
    public partial class Modify : Page
    {       

        		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				#warning 代码生成提示：显示页面,请检查确认该语句是否正确
				ShowInfo();
			}
		}
			
	private void ShowInfo()
	{
		Maticsoft.BLL.tb_UserLoginRecords bll=new Maticsoft.BLL.tb_UserLoginRecords();
		Maticsoft.Model.tb_UserLoginRecords model=bll.GetModel();
		this.txtUserID.Text=model.UserID;
		this.txtAction.Text=model.Action;
		this.txtDateTime.Text=model.DateTime;
		this.txtWorkType.Text=model.WorkType;

	}

		public void btnSave_Click(object sender, EventArgs e)
		{
			
			string strErr="";
			if(this.txtUserID.Text.Trim().Length==0)
			{
				strErr+="UserID不能为空！\\n";	
			}
			if(this.txtAction.Text.Trim().Length==0)
			{
				strErr+="Action不能为空！\\n";	
			}
			if(this.txtDateTime.Text.Trim().Length==0)
			{
				strErr+="DateTime不能为空！\\n";	
			}
			if(this.txtWorkType.Text.Trim().Length==0)
			{
				strErr+="WorkType不能为空！\\n";	
			}

			if(strErr!="")
			{
				MessageBox.Show(this,strErr);
				return;
			}
			string UserID=this.txtUserID.Text;
			string Action=this.txtAction.Text;
			string DateTime=this.txtDateTime.Text;
			string WorkType=this.txtWorkType.Text;


			Maticsoft.Model.tb_UserLoginRecords model=new Maticsoft.Model.tb_UserLoginRecords();
			model.UserID=UserID;
			model.Action=Action;
			model.DateTime=DateTime;
			model.WorkType=WorkType;

			Maticsoft.BLL.tb_UserLoginRecords bll=new Maticsoft.BLL.tb_UserLoginRecords();
			bll.Update(model);
			Maticsoft.Common.MessageBox.ShowAndRedirect(this,"保存成功！","list.aspx");

		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
