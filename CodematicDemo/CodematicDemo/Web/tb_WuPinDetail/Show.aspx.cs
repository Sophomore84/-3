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
namespace Maticsoft.Web.tb_WuPinDetail
{
    public partial class Show : Page
    {        
        		public string strid=""; 
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Request.Params["id"] != null && Request.Params["id"].Trim() != "")
				{
					strid = Request.Params["id"];
					string BianMa= strid;
					ShowInfo(BianMa);
				}
			}
		}
		
	private void ShowInfo(string BianMa)
	{
		Maticsoft.BLL.tb_WuPinDetail bll=new Maticsoft.BLL.tb_WuPinDetail();
		Maticsoft.Model.tb_WuPinDetail model=bll.GetModel(BianMa);
		this.lblBianMa.Text=model.BianMa;
		this.lblMingCheng.Text=model.MingCheng;
		this.lblGuiGe.Text=model.GuiGe;
		this.lblXingHao.Text=model.XingHao;
		this.lblHeTongHao.Text=model.HeTongHao;

	}


    }
}
