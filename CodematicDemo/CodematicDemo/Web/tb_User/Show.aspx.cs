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
namespace Maticsoft.Web.tb_User
{
    public partial class Show : Page
    {        
        		public string strid=""; 
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
		Maticsoft.BLL.tb_User bll=new Maticsoft.BLL.tb_User();
		Maticsoft.Model.tb_User model=bll.GetModel();
		this.lblUserID.Text=model.UserID;
		this.lblPassword.Text=model.Password;
		this.lblWorkType.Text=model.WorkType;
		this.lblCreateTime.Text=model.CreateTime;
		this.lblChangeTime.Text=model.ChangeTime;

	}


    }
}
