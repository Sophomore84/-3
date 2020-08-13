using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自动化库存管理
{
    public partial class SheZhiForm : Form
    {
        bool resizeEnable = false;
        int orignalparentHeight;
        int orignalparentWidth;
        public SheZhiForm()
        {
            InitializeComponent();
        }
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        private void ReadINI()
        {
            StringBuilder temp = new StringBuilder();
            GetPrivateProfileString("设置点位", "SX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtSX.Text = "0";
            else
                txtSX.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "SY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtSY.Text = "0";
            else
                txtSY.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "SZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtSZ.Text = "0";
            else
                txtSZ.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "AX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtAX.Text = "0";
            else
                txtAX.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "AY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtAY.Text = "0";
            else
                txtAY.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "AZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtAZ.Text = "0";
            else
                txtAZ.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "BX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtBX.Text = "0";
            else
                txtBX.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "BY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtBY.Text = "0";
            else
                txtBY.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "BZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtBZ.Text = "0";
            else
                txtBZ.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "CX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtCX.Text = "0";
            else
                txtCX.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "CY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtCY.Text = "0";
            else
                txtCY.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "CZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtCZ.Text = "0";
            else
                txtCZ.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "DX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtDX.Text = "0";
            else
                txtDX.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "DY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtDY.Text = "0";
            else
                txtDY.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "DZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtDZ.Text = "0";
            else
                txtDZ.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "EX", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtEX.Text = "0";
            else
                txtEX.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "EY", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtEY.Text = "0";
            else
                txtEY.Text = temp.ToString();
            GetPrivateProfileString("设置点位", "EZ", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtEZ.Text = "0";
            else
                txtEZ.Text = temp.ToString();
            GetPrivateProfileString("PLC", "IP", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtIP.Text = "0";
            else
                txtIP.Text = temp.ToString();
            GetPrivateProfileString("误差校正", "HuoChaBianMaQi", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtHuoCha.Text = "0";
            else
                txtHuoCha.Text = temp.ToString();
            GetPrivateProfileString("误差校正", "HuoJiaGaoDu", "读取异常", temp, 128, Application.StartupPath + "\\config.ini");
            if (temp.ToString() == "读取异常")
                txtHuoJia.Text = "0";
            else
                txtHuoJia.Text = temp.ToString();
        }
        private bool IsNumberic(string oText)
        {
            try
            {
                int var1 = Convert.ToInt32(oText);
                return true;
            }
            catch
            {
                return false;
            }
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
        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtSX.Text.Trim() == "" || txtSY.Text.Trim() == "" || txtSZ.Text.Trim() == ""
                || txtAX.Text.Trim() == "" || txtAY.Text.Trim() == "" || txtAZ.Text.Trim() == ""
                || txtBX.Text.Trim() == "" || txtBY.Text.Trim() == "" || txtBZ.Text.Trim() == ""
                || txtCX.Text.Trim() == "" || txtCY.Text.Trim() == "" || txtCZ.Text.Trim() == ""
                || txtDX.Text.Trim() == "" || txtDY.Text.Trim() == "" || txtDZ.Text.Trim() == ""
                || txtEX.Text.Trim() == "" || txtEY.Text.Trim() == "" || txtEZ.Text.Trim() == "")
            {
                MessageBox.Show("请设置点位！");
                return;
            }
            if (!IsNumberic(txtSX.Text.Trim()) || !IsNumberic(txtSY.Text.Trim()) || !IsNumberic(txtSZ.Text.Trim())
                 || !IsNumberic(txtAX.Text.Trim()) || !IsNumberic(txtAY.Text.Trim()) || !IsNumberic(txtAZ.Text.Trim())
                  || !IsNumberic(txtBX.Text.Trim()) || !IsNumberic(txtBY.Text.Trim()) || !IsNumberic(txtBZ.Text.Trim())
                   || !IsNumberic(txtCX.Text.Trim()) || !IsNumberic(txtCY.Text.Trim()) || !IsNumberic(txtCZ.Text.Trim())
                    || !IsNumberic(txtDX.Text.Trim()) || !IsNumberic(txtDY.Text.Trim()) || !IsNumberic(txtDZ.Text.Trim())
                     || !IsNumberic(txtEX.Text.Trim()) || !IsNumberic(txtEY.Text.Trim()) || !IsNumberic(txtEZ.Text.Trim())
                     || !IsNumberic(txtHuoJia.Text.Trim()) || !IsNumberic(txtHuoCha.Text.Trim()))
            {
                MessageBox.Show("格式错误！");
                return;
            }
            RuChuKuForm.A.X = Convert.ToInt32(txtAX.Text.Trim());
            RuChuKuForm.A.Y = Convert.ToInt32(txtAY.Text.Trim());
            RuChuKuForm.A.Z = Convert.ToInt32(txtAZ.Text.Trim());
            RuChuKuForm.B.X = Convert.ToInt32(txtBX.Text.Trim());
            RuChuKuForm.B.Y = Convert.ToInt32(txtBY.Text.Trim());
            RuChuKuForm.B.Z = Convert.ToInt32(txtBZ.Text.Trim());
            RuChuKuForm.C.X = Convert.ToInt32(txtCX.Text.Trim());
            RuChuKuForm.C.Y = Convert.ToInt32(txtCY.Text.Trim());
            RuChuKuForm.C.Z = Convert.ToInt32(txtCZ.Text.Trim());
            RuChuKuForm.D.X = Convert.ToInt32(txtDX.Text.Trim());
            RuChuKuForm.D.Y = Convert.ToInt32(txtDY.Text.Trim());
            RuChuKuForm.D.Z = Convert.ToInt32(txtDZ.Text.Trim());
            RuChuKuForm.E.X = Convert.ToInt32(txtEX.Text.Trim());
            RuChuKuForm.E.Y = Convert.ToInt32(txtEY.Text.Trim());
            RuChuKuForm.E.Z = Convert.ToInt32(txtEZ.Text.Trim());
            RuChuKuForm.S.X = Convert.ToInt32(txtSX.Text.Trim());
            RuChuKuForm.S.Y = Convert.ToInt32(txtSY.Text.Trim());
            RuChuKuForm.S.Z = Convert.ToInt32(txtSZ.Text.Trim());
            if (txtIP.Text.Trim() != "")
                RuChuKuForm.IP = txtIP.Text.Trim();
            RuChuKuForm.HuoChaWuCha = Convert.ToInt32(txtHuoCha.Text.Trim());
            RuChuKuForm.HuoJiaWuCha = Convert.ToInt32(txtHuoJia.Text.Trim());

            WritePrivateProfileString("设置点位", "SX", txtSX.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "SY", txtSY.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "SZ", txtSZ.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "AX", txtAX.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "AY", txtAY.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "AZ", txtAZ.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "BX", txtBX.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "BY", txtBY.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "BZ", txtBZ.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "CX", txtCX.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "CY", txtCY.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "CZ", txtCZ.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "DX", txtDX.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "DY", txtDY.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "DZ", txtDZ.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "EX", txtEX.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "EY", txtEY.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("设置点位", "EZ", txtEZ.Text.Trim(), Application.StartupPath + "\\config.ini");

            WritePrivateProfileString("PLC", "IP", txtIP.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("误差校正", "HuoChaBianMaQi", txtHuoCha.Text.Trim(), Application.StartupPath + "\\config.ini");
            WritePrivateProfileString("误差校正", "HuoJiaGaoDu", txtHuoJia.Text.Trim(), Application.StartupPath + "\\config.ini");

            this.Hide();
        }

        private void SheZhiForm_Resize(object sender, EventArgs e)
        {
            if (resizeEnable)
            {
                if (this.WindowState == FormWindowState.Minimized)
                    return;
                if (this.ClientSize.Width == 0)
                    return;
                int currentParentHeight = this.Height;//this.ClientSize.Height;
                int currentParentWidth = this.Width;//this.ClientSize.Width;
                float rateX = (float)currentParentWidth / orignalparentWidth;
                float rateY = (float)currentParentHeight / orignalparentHeight;
                orignalparentHeight = currentParentHeight;
                orignalparentWidth = currentParentWidth;
                setControls(rateX, rateY, this);
                setTag(this);
            }
        }

        private void SheZhiForm_Load(object sender, EventArgs e)
        {
            ReadINI();
            setTag(this);
            resizeEnable = true;
            orignalparentHeight = this.Height;
            orignalparentWidth = this.Width;
            this.WindowState = FormWindowState.Maximized;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
