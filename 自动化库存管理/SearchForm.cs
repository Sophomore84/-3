using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maticsoft.BLL;
using Maticsoft.Model;
using Maticsoft.DBUtility;
using 养生池;

namespace 自动化库存管理
{
    public partial class SearchForm : Form
    {
        bool bScroll = true;
        int JiaZaiNumber;
        DataSet ds;
        Maticsoft.BLL.tb_WuPinDetail WuPinDetail;//库存数据操作类
        public SearchForm()
        {
            InitializeComponent();
            this.Resize += SearchForm_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);
            WuPinDetail = new Maticsoft.BLL.tb_WuPinDetail();
            this.dataGridViewSearch.Font = new Font("华文楷体", 15);
            this.dataGridViewSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            ds = new DataSet();
            RuChuKuForm.m_WeiZhi = "";
            //this.asc.setTag(this);

        }

        private void SearchForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
        }

        public void SetSearchType(string type)
        {
            if (type == "bianma")
                radioButtonBianMa.Checked = true;
            else if (type == "mingcheng")
                radioButtonMingCheng.Checked = true;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (this.textBoxSearch.Text.Trim() == "")
            {
                MessageBox.Show("请输入查找内容！");
                this.textBoxSearch.Focus();
                return;
            }
            else
            {
                JiaZaiNumber = 100;
                try
                {
                    if (DbHelperSQL.IsConnected())
                    {
                        string col = "";
                        if (radioButtonBianMa.Checked)
                            col = "BianMa";
                        if (radioButtonMingCheng.Checked)
                            col = "MingCheng";
                        //string whereSQL = col + " like '%" + textBoxSearch.Text + "%'";
                        string whereSQL = col + " like '" + textBoxSearch.Text + "%'";
                        ds = WuPinDetail.GetList(JiaZaiNumber, whereSQL, "BianMa ASC");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            dataGridViewSearch.DataSource = ds.Tables[0];
                            dataGridViewSearch.Columns[0].HeaderText = "编码";
                            dataGridViewSearch.Columns[1].HeaderText = "名称";
                            dataGridViewSearch.Columns[2].HeaderText = "规格";
                            dataGridViewSearch.Columns[3].HeaderText = "型号";
                            dataGridViewSearch.Columns[4].HeaderText = "合同号";
                            dataGridViewSearch.ClearSelection();
                        }
                        else
                            dataGridViewSearch.DataSource = null;
                        if (JiaZaiNumber > ds.Tables[0].Rows.Count)
                            bScroll = false;
                        else
                            bScroll = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void SearchForm_Shown(object sender, EventArgs e)
        {
            this.textBoxSearch.Focus();
        }

        private void radioButtonMingCheng_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxSearch.Focus();
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(sender, e as EventArgs);
            }
        }

        public delegate void SetTextBoxDelegate();
        public event SetTextBoxDelegate SetTextBoxEvent;

        private void dataGridViewSearch_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                //事件
                RuChuKuForm.m_BianMa = dataGridViewSearch.Rows[e.RowIndex].Cells[0].Value.ToString();
                RuChuKuForm.m_MingCheng = dataGridViewSearch.Rows[e.RowIndex].Cells[1].Value.ToString();
                RuChuKuForm.m_GuiGe = dataGridViewSearch.Rows[e.RowIndex].Cells[2].Value.ToString();
                RuChuKuForm.m_XingHao = dataGridViewSearch.Rows[e.RowIndex].Cells[3].Value.ToString();
                RuChuKuForm.m_HeTongHao = dataGridViewSearch.Rows[e.RowIndex].Cells[4].Value.ToString();
                this.textBoxSearch.Text = "";
                this.dataGridViewSearch.DataSource = null;
                this.Close();
                SetTextBoxEvent();
            }
        }

        private void dataGridViewSearch_Scroll(object sender, ScrollEventArgs e)
        {
            if(bScroll)
            {
                if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                {
                    if (e.NewValue + dataGridViewSearch.DisplayedRowCount(false) == dataGridViewSearch.Rows.Count)
                    {
                        //这里面写加载数据的相关操作逻辑
                        JiaZaiNumber += 100;
                        try
                        {
                            if (DbHelperSQL.IsConnected())
                            {
                                string col = "";
                                if (radioButtonBianMa.Checked)
                                    col = "BianMa";
                                if (radioButtonMingCheng.Checked)
                                    col = "MingCheng";
                                string whereSQL = col + " like '" + textBoxSearch.Text + "%'";
                                ds = WuPinDetail.GetList(JiaZaiNumber, whereSQL, "BianMa ASC");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    dataGridViewSearch.DataSource = ds.Tables[0];
                                }
                                else
                                    dataGridViewSearch.DataSource = null;
                                if (JiaZaiNumber > ds.Tables[0].Rows.Count)
                                    bScroll = false;
                                else
                                    bScroll = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }             
        }

        private void SearchForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }
        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x0400011C RID: 284
        private float X;

        // Token: 0x0400011D RID: 285
        private float Y;
        private void SearchForm_SizeChanged(object sender, EventArgs e)
        {

        }

        private void SearchForm_Load(object sender, EventArgs e)
        {
            //this.Resize += SearchForm_Resize;
            //X = this.Width;
            //Y = this.Height;
            //setTag(this);
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

    }
}
