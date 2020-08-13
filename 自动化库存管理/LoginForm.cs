using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO.Ports;
using System.Runtime.InteropServices;
using Maticsoft.BLL;
using Maticsoft.Model;
using Maticsoft.DBUtility;
using 养生池;

namespace 自动化库存管理
{
    public delegate void ChangeVisible(bool visible);
    public partial class loginForm : Form
    {
        // Token: 0x040000A1 RID: 161
        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x040000A2 RID: 162
        private float X;

        // Token: 0x040000A3 RID: 163
        private float Y;
        KuWeiForm kuweiForm;
        #region 交换窗体
        ExForm exForm;
        #endregion
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        Maticsoft.BLL.tb_User UserBLL = new Maticsoft.BLL.tb_User();
        Maticsoft.Model.tb_User UserModel = new Maticsoft.Model.tb_User();
        Maticsoft.BLL.tb_UserLoginRecords LoginRecordsBLL = new Maticsoft.BLL.tb_UserLoginRecords();
        Maticsoft.Model.tb_UserLoginRecords LoginRecordsModel = new Maticsoft.Model.tb_UserLoginRecords();
        private ManageAccountForm manageAccForm;

        bool resizeEnable = false;
        int orignalparentHeight;
        int orignalparentWidth;
        public event ChangeVisible changeVisible;
        public loginForm()
        {
            InitializeComponent();
            //this.X = (float)base.Width;
            //this.Y = (float)base.Height;
            //this.asc.setTag(this);
        }
        public void Show(bool flag)
        {
            if (flag)//登录成功
            {

            }
            else//退出登录
            {
                TuiChuDengLu(); 
            }

        }
        //public loginForm(int nx, int ny)
        //{
        //    InitializeComponent();
        //    this.X = (float)base.Width;
        //    this.Y = (float)base.Height;
        //    this.asc.setTag(this);
        //    base.Width = nx;
        //    base.Height = ny;
        //    float newx = (float)nx / this.X;
        //    float newy = (float)ny / this.Y;
        //    this.asc.setControls(newx, newy, this);

        //}
        private void ReadINI()
        {
            StringBuilder temp = new StringBuilder();
            GetPrivateProfileString("设置点位", "SX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.S.X = 0;
            else
                RuChuKuForm.S.X = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "SY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.S.Y = 0;
            else
                RuChuKuForm.S.Y = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "SZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.S.Z = 0;
            else
                RuChuKuForm.S.Z = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "AX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.A.X = 0;
            else
                RuChuKuForm.A.X = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "AY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.A.Y = 0;
            else
                RuChuKuForm.A.Y = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "AZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.A.Z = 0;
            else
                RuChuKuForm.A.Z = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "BX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.B.X = 0;
            else
                RuChuKuForm.B.X = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "BY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.B.Y = 0;
            else
                RuChuKuForm.B.Y = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "BZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.B.Z = 0;
            else
                RuChuKuForm.B.Z = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "CX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.C.X = 0;
            else
                RuChuKuForm.C.X = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "CY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.C.Y = 0;
            else
                RuChuKuForm.C.Y = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "CZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.C.Z = 0;
            else
                RuChuKuForm.C.Z = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "DX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.D.X = 0;
            else
                RuChuKuForm.D.X = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "DY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.D.Y = 0;
            else
                RuChuKuForm.D.Y = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "DZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.D.Z = 0;
            else
                RuChuKuForm.D.Z = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "EX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.E.X = 0;
            else
                RuChuKuForm.E.X = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "EY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.E.Y = 0;
            else
                RuChuKuForm.E.Y = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("设置点位", "EZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.E.Z = 0;
            else
                RuChuKuForm.E.Z = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("PLC", "IP", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.IP = "192.168.0.1";
            else
                RuChuKuForm.IP = temp.ToString();
            GetPrivateProfileString("误差校正", "HuoChaBianMaQi", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.HuoChaWuCha = 0;
            else
                RuChuKuForm.HuoChaWuCha = Convert.ToInt32(temp.ToString());
            GetPrivateProfileString("误差校正", "HuoJiaGaoDu", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                RuChuKuForm.HuoJiaWuCha = 0;
            else
                RuChuKuForm.HuoJiaWuCha = Convert.ToInt32(temp.ToString());
        }

        private void btn_Login_Click(object sender, EventArgs e)
        {
            try
            {
                UserModel.UserID = textBoxUserID.Text.Trim();
                UserModel.Password = textBoxPassword.Text.Trim();
                if (DbHelperSQL.IsConnected())
                {
                    string whereSQL = string.Format(@"UserID = '{0}' AND Password = '{1}' ", UserModel.UserID, UserModel.Password);
                    DataSet ds = UserBLL.GetList(whereSQL);
                    if (ds.Tables[0].Rows.Count > 0)//登录成功
                    {

                        RuChuKuForm.locationRecordModel.UserName = UserModel.UserID;

                        RuChuKuForm.isLogin = true;
                        RuChuKuForm.loginname = ds.Tables[0].Rows[0][0].ToString();
                        this.labelYongHuMing2.Text = RuChuKuForm.loginname;
                        RuChuKuForm.LoginType = ds.Tables[0].Rows[0][2].ToString();
                        //写入数据库登录动作
                        LoginRecordsModel.UserID = textBoxUserID.Text.Trim();
                        LoginRecordsModel.Action = "登录成功";
                        LoginRecordsModel.WorkType = RuChuKuForm.LoginType;
                        LoginRecordsModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        LoginRecordsBLL.Add(LoginRecordsModel);
                        //if (LoginRecordsModel.WorkType== "坐标位置管理员")//如果坐标管理员登录，则可以修改坐标
                        //{
                        //    this.btnManageUser.Visible = true;
                        //    this.labelYongHuDengLu.Visible = false;
                        //    this.labelYongHuMing1.Visible = false;
                        //    this.labelMiMa.Visible = false;
                        //    this.textBoxUserID.Visible = false;
                        //    this.textBoxPassword.Visible = false;
                        //    this.btn_Login.Visible = false;
                        //    this.btn_Reset.Visible = false;
                        //    this.btnChuKu.Visible = false;
                        //    this.btnRuKu.Visible = false;
                        //    this.labelDengLuChengGong.Visible = true;
                        //    this.labelYongHuMing2.Visible = true;
                        //    this.labelDangQianYongHu.Visible = true;
                        //    this.btnTuiChuDengLu.Visible = true;
                        //    this.btnWeiZhi.Visible = true;

                            
                        //}
                        //else
                        //{
                            
                            this.labelYongHuDengLu.Visible = false;
                            this.labelYongHuMing1.Visible = false;
                            this.labelMiMa.Visible = false;
                            this.textBoxUserID.Visible = false;
                            this.textBoxPassword.Visible = false;
                            this.btn_Login.Visible = false;
                            this.btn_Reset.Visible = false;
                        //this.btnWeiZhi.Visible = false;
                        //this.labelDengLuChengGong.Visible = true;
                        #region
                        //this.btnManageUser.Visible = true;
                        //this.labelYongHuMing2.Visible = true;
                        //    this.labelDangQianYongHu.Visible = true;
                        //    this.btnTuiChuDengLu.Visible = true;

                        this.btnChuKu.Visible = true;
                        this.btnRuKu.Visible = true;
                        #endregion
                        #region 
                        btnExchange.Visible = true;
                        //btnSheBeiCanShu.Visible = true;
                        //btnWeiZhi.Visible = true;

                        changeVisible(true);
                        #endregion
                        //}
                        //写入用户登录记录数据库 
                        //string strSQL2 = string.Format(@"insert into tb_UserLoginRecords values('{0}','{1}','{2}','{3}')", textBoxUserID.Text.Trim(), "登录成功", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), mainForm.LoginType);
                        //SqlCommand thisCommand1 = new SqlCommand(strSQL2, mainForm.myconnection);
                        //thisCommand1.ExecuteNonQuery();

                    }
                    else
                    {
                        MessageBox.Show("用户名或密码错误！");
                    }
                }
                else
                {
                    MessageBox.Show("数据库连接失败！");
                }
            }
            catch (Exception ex)
            {
                RuChuKuForm.loginname = "";
                RuChuKuForm.isLogin = false;
                RuChuKuForm.LoginType = "";
                MessageBox.Show("账号或密码错误！");
                textBoxUserID.Focus();
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            this.textBoxUserID.Text = "";
            this.textBoxPassword.Text = "";
        }
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }
        private void loginForm_Load(object sender, EventArgs e)
        {
            this.Resize += LoginForm_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);
            //setTag(this);
            //resizeEnable = true;
            //orignalparentHeight = this.Height;
            //orignalparentWidth = this.Width;
            //this.WindowState = FormWindowState.Maximized;
            textBoxUserID.Focus();
        }

        private void LoginForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
        }

        public void TuiChuDengLu()
        {
            //显示登录界面
            RuChuKuForm.detailRuKuForm.Hide();
            RuChuKuForm.detailChuKuForm.Hide();
            RuChuKuForm.renGongGuanLi.Hide();//人工管理界面
            this.Show();
            //写入数据库退出登录动作
            LoginRecordsModel.UserID = textBoxUserID.Text.Trim();
            LoginRecordsModel.Action = "退出登录";
            LoginRecordsModel.WorkType = RuChuKuForm.LoginType;
            LoginRecordsModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            LoginRecordsBLL.Add(LoginRecordsModel);
            //写入用户登录记录数据库 
            //string strSQL = string.Format(@"insert into tb_UserLoginRecords values('{0}','{1}','{2}','{3}')", textBoxUserID.Text.Trim(), "退出登录", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),mainForm.LoginType);
            //SqlCommand thisCommand = new SqlCommand(strSQL, mainForm.myconnection);
            //thisCommand.ExecuteNonQuery();

            this.textBoxUserID.Text = "";
            this.textBoxPassword.Text = "";

            RuChuKuForm.loginname = "";
            RuChuKuForm.isLogin = false;
            RuChuKuForm.LoginType = "";

            this.labelYongHuDengLu.Visible = true;
            this.labelYongHuMing1.Visible = true;
            this.labelMiMa.Visible = true;
            this.textBoxUserID.Visible = true;
            this.textBoxPassword.Visible = true;
            this.btn_Login.Visible = true;
            this.btn_Reset.Visible = true;

            this.labelDengLuChengGong.Visible = false;
            this.labelYongHuMing2.Visible = false;
            this.labelDangQianYongHu.Visible = false;
            this.btnTuiChuDengLu.Visible = false;
            this.btnManageUser.Visible = false;
            this.btnChuKu.Visible = false;
            this.btnRuKu.Visible = false;

            btnExchange.Visible = false;
            btnSheBeiCanShu.Visible = false;
            btnWeiZhi.Visible = false;
        }

        private void btnTuiChuDengLu_Click(object sender, EventArgs e)
        {
            TuiChuDengLu();
            changeVisible(false);
        }
        private void btnManageUser_Click(object sender, EventArgs e)
        {
            manageAccForm = new ManageAccountForm();
            manageAccForm.ShowDialog();
        }

        private void btnRuKu_Click(object sender, EventArgs e)
        {
            RuChuKuForm.m_RuKuChuKuFlag = true;
            this.Hide();
            RuChuKuForm.detailRuKuForm.Show();
            
        }

        private void btnChuKu_Click(object sender, EventArgs e)
        {
            RuChuKuForm.m_RuKuChuKuFlag = false;
            this.Hide();
            RuChuKuForm.detailChuKuForm.Show();
        }
        //public loginForm((int nx, int ny)
        //{
        //    InitializeComponent();
        //    this.X = (float)base.Width;
        //    this.Y = (float)base.Height;
        //    this.asc.setTag(this);
        //    base.Width = nx;
        //    base.Height = ny;
        //    float newx = (float)nx / this.X;
        //    float newy = (float)ny / this.Y;
        //    this.asc.setControls(newx, newy, this);

        //}
        private void loginForm_Resize(object sender, EventArgs e)
        {
            //float newx = (float)nx / this.X;
            //float newy = (float)ny / this.Y;
            //this.asc.setControls(newx, newy, this);
            //if (resizeEnable)
            //{
            //    if (this.WindowState == FormWindowState.Minimized)
            //        return;
            //    if (this.ClientSize.Width == 0)
            //        return;
            //    int currentParentHeight = this.Height;//this.ClientSize.Height;
            //    int currentParentWidth = this.Width;//this.ClientSize.Width;
            //    float rateX = (float)currentParentWidth / orignalparentWidth;
            //    float rateY = (float)currentParentHeight / orignalparentHeight;
            //    orignalparentHeight = currentParentHeight;
            //    orignalparentWidth = currentParentWidth;
            //    setControls(rateX, rateY, this);
            //    setTag(this);
            //}
        }

        private void btnWeiZhi_Click(object sender, EventArgs e)
        {
            this.Hide();
            RuChuKuForm.renGongGuanLi.Show();
        }

        private void btnSheBeiCanShu_Click(object sender, EventArgs e)
        {
            this.Hide();
            RuChuKuForm.sheBeiCanShu.Show();
            

        }

        private void btnExchange_Click(object sender, EventArgs e)
        {
            exForm = new ExForm();
            exForm.ShowDialog();
            //kuweiForm = new KuWeiForm();
            //kuweiForm.SetWeiZhiEvent += KuweiForm_SetWeiZhiEvent;
            //kuweiForm.ShowDialog();
        }

        private void KuweiForm_SetWeiZhiEvent()
        {
            //textBoxWeiZhi.Text = RuChuKuForm.m_WeiZhi;
        }

        
    }
}
