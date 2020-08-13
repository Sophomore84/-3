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
using Maticsoft.BLL;
using Maticsoft.Model;
using Maticsoft.DBUtility;
using 养生池;

namespace 自动化库存管理
{
    public partial class ManageAccountForm : Form
    {
        private string loginName = "";
        Maticsoft.BLL.tb_User UserBLL = new Maticsoft.BLL.tb_User();
        Maticsoft.Model.tb_User UserModel = new Maticsoft.Model.tb_User();
        public ManageAccountForm()
        {
            InitializeComponent();
            dataGridViewUser.Font = new Font("华文楷体", 12);
            
        }

        private void btnCloseAccount_Click(object sender, EventArgs e)
        {
            this.Close();                 //关闭窗体
            this.Dispose();               //释放资源
        }
        private void FreshGrid()
        {
            try
            {
                if (DbHelperSQL.IsConnected())
                {
                    DataSet ds;
                    if (RuChuKuForm.LoginType == "系统管理员")
                    {
                        string strSQL = string.Format(@"select * from  tb_User ORDER BY ChangeTime DESC");
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(strSQL, DbHelperSQL.connectionString);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet, "tb_User");
                        dataGridViewUser.DataSource = dataSet.Tables["tb_User"];
                    }
                    else
                    {
                        string strSQL = string.Format(@"select * from  tb_User where UserID = '{0}' ORDER BY ChangeTime DESC", RuChuKuForm.loginname);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(strSQL, DbHelperSQL.connectionString);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet, "tb_User");
                        dataGridViewUser.DataSource = dataSet.Tables["tb_User"];
                    }
                    dataGridViewUser.Columns[0].HeaderText = "帐号";
                    dataGridViewUser.Columns[1].HeaderText = "密码";
                    dataGridViewUser.Columns[2].HeaderText = "工种";
                    dataGridViewUser.Columns[3].HeaderText = "创建时间";
                    dataGridViewUser.Columns[4].HeaderText = "更新时间";
                    dataGridViewUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridViewUser.ClearSelection();
                }
                else
                    MessageBox.Show("数据库连接失败！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ManageAccountForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (DbHelperSQL.IsConnected())
                {
                    comboBoxWorkType.Items.Clear();
                    comboBoxWorkType.Items.Add("系统管理员");
                    comboBoxWorkType.Items.Add("操作工");
                    comboBoxWorkType.Items.Add("维修工");
                    comboBoxWorkType.Items.Add("设备管理员");
                    if (RuChuKuForm.LoginType != "系统管理员")
                    {
                        string sql = string.Format(@"select * from  tb_User where UserID = '{0}' ORDER BY ChangeTime DESC", RuChuKuForm.loginname);
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, DbHelperSQL.connectionString);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet, "tb_User");
                        dataGridViewUser.DataSource = dataSet.Tables["tb_User"];
                        txtAccount.Enabled = false;
                        comboBoxWorkType.Enabled = false;
                        btn_AddUser.Visible = false;
                        btn_DeleteUser.Visible = false;
                        btnReset.Visible = false;
                    }
                    else
                    {
                        string sql = string.Format(@"select * from  tb_User ORDER BY ChangeTime DESC");
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, DbHelperSQL.connectionString);
                        DataSet dataSet = new DataSet();
                        dataAdapter.Fill(dataSet, "tb_User");
                        dataGridViewUser.DataSource = dataSet.Tables["tb_User"];
                    }
                    dataGridViewUser.Columns[0].HeaderText = "帐号";
                    dataGridViewUser.Columns[1].HeaderText = "密码";
                    dataGridViewUser.Columns[2].HeaderText = "工种";
                    dataGridViewUser.Columns[3].HeaderText = "创建时间";
                    dataGridViewUser.Columns[4].HeaderText = "更新时间";
                    dataGridViewUser.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
                    dataGridViewUser.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            this.Resize += ManageAccountForm_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);

        }

        private void ManageAccountForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
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

        private void btn_AddUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAccount.Text.Trim() == "" || txtPassWord.Text.Trim() == "" || comboBoxWorkType.Text.Trim() == "")
                {
                    MessageBox.Show("信息不完整，请重新输入！");
                    return;
                }
                if (DbHelperSQL.IsConnected())
                {
                    string whereSQL = string.Format(@"UserID = '{0}'", txtAccount.Text.Trim());
                    DataSet ds = UserBLL.GetList(whereSQL);
                    if (ds.Tables[0].Rows.Count > 0) //先查询数据库是否有该帐号
                    {
                        MessageBox.Show("该用户名已存在，请输入其他用户名！");
                    }
                    else
                    {
                        //新帐号写入数据库
                        UserModel.UserID = txtAccount.Text.Trim();
                        UserModel.Password = txtPassWord.Text.Trim();
                        UserModel.WorkType = comboBoxWorkType.Text.Trim();
                        UserModel.ChangeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        UserModel.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        UserBLL.Add(UserModel);
                        FreshGrid();
                    }
                }
                else
                    MessageBox.Show("数据库连接失败！");
                ////先查询数据库是否有重复帐号
                //string strSQL = string.Format("select * from  tb_User where UserID = '{0}'", txtAccount.Text.Trim());
                //SqlCommand thisCommand = new SqlCommand(strSQL, mainForm.myconnection);
                //SqlDataReader reader = thisCommand.ExecuteReader();
                //if (reader.HasRows)
                //{
                //    MessageBox.Show("该用户名已存在，请输入其他用户名！");
                //    return;
                //}
                //写入数据库
                //strSQL = string.Format(@"insert into tb_User values('{0}','{1}','{2}','{3}','{4}')", txtAccount.Text.Trim(), txtPassWord.Text.Trim(), comboBoxWorkType.Text.Trim(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //SqlCommand thisCommand1 = new SqlCommand(strSQL, mainForm.myconnection);
                //thisCommand1.ExecuteNonQuery();
                //FreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_ModifyUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAccount.Text == "" || txtPassWord.Text == "" || comboBoxWorkType.Text == "")
                {
                    MessageBox.Show("信息不完整，请重新输入！");
                    return;
                }
                if (DbHelperSQL.IsConnected())
                {
                    //先查询数据库是否有该帐号
                    if (UserBLL.Exists(txtAccount.Text.Trim()))
                    {
                        //更新帐号信息
                        UserModel.UserID = txtAccount.Text.Trim();
                        UserModel.Password = txtPassWord.Text.Trim();
                        UserModel.WorkType = comboBoxWorkType.Text.Trim();
                        UserModel.ChangeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        UserModel.CreateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        UserBLL.Update(UserModel);
                    }
                    else
                    {
                        MessageBox.Show("该帐号不存在，请核对帐号是否正确！");
                        return;
                    }
                    FreshGrid();
                }
                else
                    MessageBox.Show("数据库连接失败！");

                //先查询数据库是否有该帐号
                //string strSQL = string.Format("select * from  tb_User where UserID = '{0}'", txtAccount.Text.Trim());
                //SqlCommand thisCommand = new SqlCommand(strSQL, mainForm.myconnection);
                //SqlDataReader reader = thisCommand.ExecuteReader();
                //if (reader.HasRows)
                //{
                //    //更新数据库
                //    strSQL = string.Format(@"update tb_User set Password = '{0}' , WorkType = '{1}' 
                //, ChangeTime = '{2}' where UserID = '{3}'", txtPassWord.Text.Trim(),
                //    comboBoxWorkType.Text.Trim(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                //    txtAccount.Text.Trim());
                //    SqlCommand thisCommand1 = new SqlCommand(strSQL, mainForm.myconnection);
                //    thisCommand1.ExecuteNonQuery();
                //}
                //else
                //{
                //    MessageBox.Show("该帐号不存在，请核对帐号是否正确！");
                //    return;
                //}
                //FreshGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_DeleteUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAccount.Text == "" || txtPassWord.Text == "" || comboBoxWorkType.Text == "")
                {
                    MessageBox.Show("信息不完整，请重新选择！");
                    return;
                }
                if (loginName == "")
                {
                    MessageBox.Show("请选择用户！");
                    return;
                }
                if (loginName == RuChuKuForm.loginname)
                {
                    MessageBox.Show("当前用户不能删除！");
                    return;
                }
                //从数据库中删除数据
                UserBLL.Delete(loginName);
                //从数据库中删除数据
                //string strSQL = string.Format(@"delete from tb_User where UserID = '{0}'", loginName);
                //SqlCommand thisCommand = new SqlCommand(strSQL, mainForm.myconnection);
                //thisCommand.ExecuteNonQuery();
                FreshGrid();
                Reset();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            txtAccount.Text = "";
            txtPassWord.Text = "";
            loginName = "";
            comboBoxWorkType.SelectedIndex = -1;
            dataGridViewUser.ClearSelection();
        }
        private void dataGridViewUser_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                loginName = dataGridViewUser.SelectedRows[0].Cells[0].Value.ToString();
                if (loginName != "")
                    txtAccount.Text = loginName;
                string passWord = dataGridViewUser.SelectedRows[0].Cells[1].Value.ToString();
                if (passWord != "")
                    txtPassWord.Text = passWord;
                string workType = dataGridViewUser.SelectedRows[0].Cells[2].Value.ToString();
                if (workType != "")
                    comboBoxWorkType.Text = workType;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x0400011C RID: 284
        private float X;

        // Token: 0x0400011D RID: 285
        private float Y;
        private void ManageAccountForm_SizeChanged(object sender, EventArgs e)
        {
            float newx = (float)base.Width / this.X;
            float newy = (float)base.Height / this.Y;
            this.asc.setControls(newx, newy, this);
        }
    }
}
