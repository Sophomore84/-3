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
using System.Text.RegularExpressions;
using 自动化库存管理.Model;
using HslCommunication.LogNet;
using 自动化库存管理.Command;
using 养生池;
using System.Threading;

namespace 自动化库存管理
{
    public delegate void ChangeChuTaskTxt(string txt);
    public partial class DetailOperationForm : Form
    {
        public event ChangeChuTaskTxt changeTxt;
        bool Type;//true: 出库；false: 入库
        KuWeiForm kuweiForm ;
        SearchForm searchFor;
        string QuHuoFangShi = "";
        bool resizeEnable = false;
        int orignalparentHeight;
        int orignalparentWidth;
        List<string> Data = new List<string>();
        Maticsoft.BLL.tb_KuCun KuCunBLL;//库存数据操作类        

        DataGridViewRowCollection dataGridViewRow;
        List<Assignment> assignments = new List<Assignment>();
        Assignment assignment = new Assignment();
        bool executeFlag = false;//执行标志

        Maticsoft.Model.LocationRecord locationRecordModel = new Maticsoft.Model.LocationRecord();
        Maticsoft.BLL.LocationRecord locationRecordBLL = new Maticsoft.BLL.LocationRecord();

        Maticsoft.Model.tb_DingDian tb_DingDianModel = new Maticsoft.Model.tb_DingDian();
        Maticsoft.BLL.tb_DingDian tb_DingDianBLL = new Maticsoft.BLL.tb_DingDian();

        Maticsoft.Model.tb_KuWei tb_KuWeiModel = new Maticsoft.Model.tb_KuWei();
        Maticsoft.BLL.tb_KuWei tb_KuWeiBLL = new Maticsoft.BLL.tb_KuWei();
        List<P> listP0 = new List<P>();
        List<P> listP = new List<P>();
        List<P> listP1 = new List<P>();
        List<P> listP2 = new List<P>();
        List<P> listP3 = new List<P>();
        List<P> listP4 = new List<P>();
        List<P> listP5 = new List<P>();
        P p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12;
        int commandType, commandType1;

        public DetailOperationForm()
        {
            InitializeComponent();
            this.Resize += DetailOperationForm_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);
            searchFor = new SearchForm();
            searchFor.SetTextBoxEvent += SearchFor_SetTextBoxEvent;
            KuCunBLL = new Maticsoft.BLL.tb_KuCun();
            intidataGridView();
            
        }
        
        private void intidataGridView()
        {
            //初始化任务列表
            
            this.dataGridViewRenWu.Font = new Font("华文楷体", 15);
            DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
            acCode.Name = "序号";
            acCode.DataPropertyName = "列0";
            acCode.HeaderText = "序号";
            this.dataGridViewRenWu.Columns.Add(acCode);
            DataGridViewTextBoxColumn acCode1 = new DataGridViewTextBoxColumn();
            acCode1.Name = "编码";
            acCode1.DataPropertyName = "列1";
            acCode1.HeaderText = "编码";
            this.dataGridViewRenWu.Columns.Add(acCode1);
            DataGridViewTextBoxColumn acCode2 = new DataGridViewTextBoxColumn();
            acCode2.Name = "名称";
            acCode2.DataPropertyName = "列2";
            acCode2.HeaderText = "名称";
            this.dataGridViewRenWu.Columns.Add(acCode2);
            DataGridViewTextBoxColumn acCode3 = new DataGridViewTextBoxColumn();
            acCode3.Name = "规格";
            acCode3.DataPropertyName = "列3";
            acCode3.HeaderText = "规格";
            this.dataGridViewRenWu.Columns.Add(acCode3);
            DataGridViewTextBoxColumn acCode4 = new DataGridViewTextBoxColumn();
            acCode4.Name = "型号";
            acCode4.DataPropertyName = "列4";
            acCode4.HeaderText = "型号";
            this.dataGridViewRenWu.Columns.Add(acCode4);
            DataGridViewTextBoxColumn acCode5 = new DataGridViewTextBoxColumn();
            acCode5.Name = "数量";
            acCode5.DataPropertyName = "列5";
            acCode5.HeaderText = "数量";
            this.dataGridViewRenWu.Columns.Add(acCode5);
            DataGridViewTextBoxColumn acCode6 = new DataGridViewTextBoxColumn();
            acCode6.Name = "合同号";
            acCode6.DataPropertyName = "列6";
            acCode6.HeaderText = "合同号";
            this.dataGridViewRenWu.Columns.Add(acCode6);
            DataGridViewTextBoxColumn acCode7 = new DataGridViewTextBoxColumn();
            acCode7.Name = "位置";
            acCode7.DataPropertyName = "列7";
            acCode7.HeaderText = "位置";
            this.dataGridViewRenWu.Columns.Add(acCode7);
            DataGridViewTextBoxColumn acCode8 = new DataGridViewTextBoxColumn();
            acCode8.Name = "理货台";
            acCode8.DataPropertyName = "列8";
            acCode8.HeaderText = "理货台";
            this.dataGridViewRenWu.Columns.Add(acCode8);
            DataGridViewTextBoxColumn acCode9 = new DataGridViewTextBoxColumn();
            acCode9.Name = "取货方式";
            acCode9.DataPropertyName = "列8";
            acCode9.HeaderText = "取货方式";
            this.dataGridViewRenWu.Columns.Add(acCode9);
        }

        private void SearchFor_SetTextBoxEvent()
        {
            int zonggeshu = 0;
            int count = 0;
            string weizhi = "";
            try
            {
                if (DbHelperSQL.IsConnected())
                {
                    //string whereSQL = string.Format(@"BianMa = '{0}' ", RuChuKuForm.m_BianMa);
                    //RuChuKuForm.KuCunList = KuCunBLL.GetModelList(whereSQL);
                    //count = RuChuKuForm.KuCunList.Count;
                    #region 暂时不用
                    string whereSQL = string.Format(@"GoodsCode = '{0}' ", RuChuKuForm.m_BianMa);
                    RuChuKuForm.locationRecordList = locationRecordBLL.GetModelList(whereSQL);
                    count = RuChuKuForm.locationRecordList.Count;
                    if (count <= 0)
                    {
                        MessageBox.Show("仓库中无此物品！");
                    }
                    else
                    {
                        for (int i = 0; i < count; i++)
                        {
                            //zonggeshu += Convert.ToInt32(RuChuKuForm.KuCunList[i].ShuLiang);
                            //weizhi +=  RuChuKuForm.KuCunList[i].HuoJiaHao + "-" + RuChuKuForm.KuCunList[i].CengHao + "-" + RuChuKuForm.KuCunList[i].LieHao+",";
                            zonggeshu += Convert.ToInt32(RuChuKuForm.locationRecordList[i].GoodsNum);
                            //weizhi += RuChuKuForm.locationRecordList[i].LocationName + ",";
                        }
                        //weizhi = weizhi.Remove(weizhi.Length - 1);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            txtBianMa.Text = RuChuKuForm.m_BianMa;
            textBoxMingCheng.Text = RuChuKuForm.m_MingCheng;
            textBoxGuiGe.Text = RuChuKuForm.m_GuiGe;
            textBoxXingHao.Text = RuChuKuForm.m_XingHao;
            textBoxHeTongHao.Text = RuChuKuForm.m_HeTongHao;
            if (zonggeshu>0)
            {
                textBoxKuCunZongLiang.Text = zonggeshu.ToString();
            }
            else
            {
                textBoxKuCunZongLiang.Text = "0";
            }
            textBoxKuCunZongLiang.Text = zonggeshu.ToString();
            textBoxWeiZhi.Text = weizhi;
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
        private void DetailOperationForm_Load(object sender, EventArgs e)
        {
            executeCeShiLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\出库日志", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            RuChuKuForm.m_BianMa = string.Empty;
            //this.Resize += DetailOperationForm_Resize;
            //X = this.Width;
            //Y = this.Height;
            //setTag(this);
            //resizeEnable = true;
            //orignalparentHeight = this.Height;
            //orignalparentWidth = this.Width;
            //this.WindowState = FormWindowState.Maximized;

        }

        private void DetailOperationForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            this.Hide();
            RuChuKuForm.logForm.Show();
            //this.Close();                 //关闭窗体
            //this.Dispose();               //释放资源
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

       

        private void comboBoxQuHuoFangShi_DropDown(object sender, EventArgs e)
        {
            comboBoxQuHuoFangShi.Items.Clear();
            comboBoxQuHuoFangShi.Items.Add("机取");
            comboBoxQuHuoFangShi.Items.Add("自取");
        }

        private void comboBoxQuHuoFangShi_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuHuoFangShi = comboBoxQuHuoFangShi.Text;
        }

        private void txtBianMa_MouseLeave(object sender, EventArgs e)
        {

        }
        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x0400011C RID: 284
        private float X;

        // Token: 0x0400011D RID: 285
        private float Y;
        private void DetailOperationForm_SizeChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxQuHuoFangShi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;//取消输入事件
        }
        CancellationTokenSource source = new CancellationTokenSource();
        CancellationToken token ;
        private void btnCancel_Click(object sender, EventArgs e)
        {
            source.Cancel();
        }

        private void textBoxWeiZhi_Click(object sender, EventArgs e)
        {
            kuweiForm = new KuWeiForm();
            kuweiForm.SetWeiZhiEvent += KuweiForm_SetWeiZhiEvent;
            kuweiForm.ShowDialog();
        }

        private void KuweiForm_SetWeiZhiEvent()
        {
            textBoxWeiZhi.Text = RuChuKuForm.m_WeiZhi;
            //查询时间
            locationRecordModel = locationRecordBLL.GetModelByPicBox(RuChuKuForm.m_WeiZhi,true);
            //txtShuLiang1.Text = locationRecordModel.GoodsNum.Remove(0,1);
            txtShuLiang.Text = locationRecordModel.GoodsNum.Remove(0, 1);
            textBoxHeTongHao.Text = locationRecordModel.GoodsContract;

        }

        private void txtBianMa_Click(object sender, EventArgs e)
        {
            searchFor.SetSearchType("bianma");
            searchFor.ShowDialog();
        }

        private void textBoxMingCheng_Click(object sender, EventArgs e)
        {
            searchFor.SetSearchType("mingcheng");
            searchFor.ShowDialog();
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
        private void txtShuLiang_Leave(object sender, EventArgs e)
        {
            if (txtShuLiang1.Text.Trim() == "" ||  textBoxKuCunZongLiang.Text.Trim ()=="")
                return;  
            if(!IsNumberic(txtShuLiang1 .Text .Trim ()))
            {
                MessageBox.Show("格式错误！");
                txtShuLiang1.Focus();
                return;
            }          
            if ( Convert.ToInt32(txtShuLiang1.Text) > Convert.ToInt32(textBoxKuCunZongLiang.Text))
            {
                MessageBox.Show("库存数量不足！");
                txtShuLiang1.Focus();
            }
        }
        #region 库位缓存
        public static List<string> listLoca = new List<string>();
        #endregion
        private void btnAddMission_Click(object sender, EventArgs e)
        {
            // && textBoxXingHao.Text.Trim() != ""
            if (txtBianMa.Text.Trim() != "" && textBoxMingCheng.Text.Trim() != "" &&
                txtShuLiang.Text.Trim() != "" && comboBoxQuHuoFangShi.Text.Trim() != ""
                && textBoxGuiGe.Text.Trim() != ""
                 && textBoxWeiZhi.Text.Trim() != "")
            {
                DataGridViewRow dr = new DataGridViewRow();
                dr.CreateCells(dataGridViewRenWu);
                dr.Cells[0].Value = dataGridViewRenWu.Rows.Count+1;
                dr.Cells[1].Value = txtBianMa.Text;
                dr.Cells[2].Value = textBoxMingCheng.Text;
                dr.Cells[3].Value = textBoxGuiGe.Text;
                dr.Cells[4].Value = textBoxXingHao.Text;
                dr.Cells[5].Value = txtShuLiang.Text;
                dr.Cells[6].Value = textBoxHeTongHao.Text;
                dr.Cells[7].Value = textBoxWeiZhi.Text;
                dr.Cells[8].Value = comboBoxLiHuoTai.Text;
                dr.Cells[9].Value = comboBoxQuHuoFangShi.Text;
                RuChuKuForm.shangHuoTai = comboBoxLiHuoTai.Text;
                //添加的行作为最后一行
                dataGridViewRenWu.Rows.Add(dr);
                //listLoca.Add(RuChuKuForm.m_WeiZhi);
                listLoca.Add(textBoxWeiZhi.Text);
                txtBianMa.Text = "";
                textBoxMingCheng.Text = "";
                txtShuLiang1.Text = "";
                comboBoxQuHuoFangShi.Text = "";
                textBoxGuiGe.Text = "";
                textBoxXingHao.Text = "";
                textBoxWeiZhi.Text = "";
                textBoxKuCunZongLiang.Text = "";
                textBoxHeTongHao.Text = "";
            }
            else
            {
                MessageBox.Show("信息不完整！");
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {

        }

        private void dataGridViewRenWu_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                DialogResult RSS = MessageBox.Show(this, "确定要删除选中行数据码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                switch (RSS)
                {
                    case DialogResult.Yes:
                        for (int i = this.dataGridViewRenWu.SelectedRows.Count; i > 0; i--)
                        {
                            dataGridViewRenWu.Rows.RemoveAt(dataGridViewRenWu.SelectedRows[i - 1].Index);                            
                        }
                        MessageBox.Show("成功删除选中行数据！");
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            //token = source.Token;
            dataGridViewRow = dataGridViewRenWu.Rows;
            foreach (DataGridViewRow row in dataGridViewRow)
            {
                assignments.Add(new Assignment()
                {
                    Location = row.Cells["位置"].Value.ToString(),
                    Code = row.Cells["编码"].Value.ToString(),
                    Platform = row.Cells["理货台"].Value.ToString(),
                    Number = row.Cells["数量"].Value.ToString(),
                    Operation = row.Cells["取货方式"].Value.ToString(),
                    Name = row.Cells["名称"].Value.ToString(),
                    ContractNum = row.Cells["合同号"].Value.ToString()

                }
                  );
                //MessageBox.Show(row.Cells["列7"].Value.ToString());
            }
            dataGridViewRenWu.Rows.Clear();
            //事件
            if (executeFlag)
            {
                executeFlag = false;
                btnTest.BackColor = Color.Navy;
            }
            else
            {
                executeFlag = true;
                btnTest.BackColor = Color.Red;
                await DoExecuteAsync();

            }
        }

        public async Task DoExecuteAsync()
        {

            
            await Task.Run(() =>
            {
                changeTxt("任务正在运行");
                //判断是否是起点,不需要
                //if (
                //Math.Abs(RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet) - int.Parse(tb_DingDianBLL.GetModel("起点").X)) < 50
                //&& Math.Abs(RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet) - int.Parse(tb_DingDianBLL.GetModel("起点").Y)) < 50
                //&& Math.Abs(RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet) - int.Parse(tb_DingDianBLL.GetModel("起点").Z)) < 50
                //)
                //{
                foreach (Assignment assi in assignments)
                {
                    //第一步：103，过道，过道，库位
                    //第二步：301，伸出
                    //第三步：101，升
                    //第四步：301，夹具原位
                    //第五步：103，过道，过道，理货台
                    //第六步：301，伸出
                    //第七步：101，升
                    //第八步：301，夹具原位
                    //第九步：103，回到起点，不需要

                    executeCeShiLogNet.WriteAnyString("测试开始");
                    tb_KuWeiModel = tb_KuWeiBLL.GetModel(assi.Location);
                    tb_DingDianModel = tb_DingDianBLL.GetModel("上货台" + assi.Platform);

                    RuChuKuForm.locationRecordModel.GoodsCode = assi.Code;
                    RuChuKuForm.locationRecordModel.GoodsName = assi.Name;
                    RuChuKuForm.locationRecordModel.GoodsContract = assi.ContractNum;
                    RuChuKuForm.locationRecordModel.GoodsNum = "-" + assi.Number;
                    RuChuKuForm.locationRecordModel.LocationName = assi.Location;
                    RuChuKuForm.locationRecordModel.Action = assi.Operation;
                    int clamp = RuChuKuForm.hostComputerCommand.QueryClamp(assi.Location);
                    if (source.Token.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        source.Token.ThrowIfCancellationRequested();
                    }
                    #region 上货台：301 1，301 3，104....
                    if (RuChuKuForm.hostComputerCommand.ReadClamp(RuChuKuForm.siemensTcpNet) == 1 || RuChuKuForm.hostComputerCommand.ReadClamp(RuChuKuForm.siemensTcpNet) == 2)//如果夹具不在原位，回到原位
                    {

                        if (RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");
                    }

                    if (RuChuKuForm.hostComputerCommand.ReadClamp(RuChuKuForm.siemensTcpNet) == 0)//如果夹具不在原位，回到原位
                    {

                        if (RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Front))//夹具原位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");
                        if (RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");

                    }



                    if (Math.Abs(RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet) - int.Parse(tb_KuWeiModel.X)) > 10)//如果大车实时位置与目标位置不相等//103,过道2，过道1，库位
                    {

                        p1.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet);
                        p1.Y = int.Parse(tb_DingDianBLL.GetModel("过道").Y);
                        p1.Z = int.Parse(tb_DingDianBLL.GetModel("过道").Z);

                        p2.X = int.Parse(tb_KuWeiModel.X);
                        p2.Y = p1.Y;
                        p2.Z = p1.Z;

                        p3.X = p2.X;
                        p3.Y = int.Parse(tb_KuWeiModel.Y);
                        p3.Z = int.Parse(tb_KuWeiModel.Z);

                        p4.X = p3.X;
                        p4.Y = p3.Y;
                        p4.Z = p3.Z + int.Parse(tb_KuWeiModel.Precision);



                        p6.X = int.Parse(tb_DingDianModel.X);
                        p6.Y = int.Parse(tb_DingDianModel.Y);
                        p6.Z = int.Parse(tb_DingDianModel.Z) + int.Parse(tb_DingDianModel.Precision);

                        p5.X = p6.X;
                        p5.Y = p1.Y;
                        p5.Z = p1.Z;



                        p7.X = p6.X;
                        p7.Y = p6.Y;
                        p7.Z = int.Parse(tb_DingDianModel.Z);




                        //第一步：103，过道，过道，库位
                        listP.Add(p1);
                        listP.Add(p2);
                        listP.Add(p3);
                        //第三步：101，升
                        listP1.Add(p4);
                        //第五步：103，过道，过道，理货台
                        listP2.Add(p2);
                        listP2.Add(p5);
                        listP2.Add(p6);
                        //第七步：101，降
                        listP3.Add(p7);

                        executeCeShiLogNet.WriteAnyString("去库位");
                        while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP, out commandType))//到库位
                        {

                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP));
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                      RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                      RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                      RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrZThree(RuChuKuForm.siemensTcpNet).ToString() + " "

                                      );
                            //if (this.IsHandleCreated)
                            //{

                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入" + commandType.ToString() + "指令";

                            }));
                            //}

                        }
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令运行中";

                            }));
                        }
                        executeCeShiLogNet.WriteAnyString(commandType.ToString() + "执行完成");
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令执行完成";

                        }));

                        while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, clamp))//夹具伸出
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Front);
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                       RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +

                                        RuChuKuForm.hostComputerCommand.ReadWrClamp(RuChuKuForm.siemensTcpNet).ToString() + " "

                                       );
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");

                        while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP1, out commandType1))
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP1));
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                      RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                     RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                     RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " "


                                      );
                            //if (this.IsHandleCreated)
                            //{

                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入" + commandType1.ToString() + "指令";

                            }));
                            //}

                        }//101
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = commandType1.ToString() + "指令运行中";

                            }));
                        }
                        executeCeShiLogNet.WriteAnyString(commandType1.ToString() + "执行完成");
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = commandType1.ToString() + "指令执行完成";

                        }));


                        while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");
                        RuChuKuForm.locationRecordModel.Weight = RuChuKuForm.weight;
                        RuChuKuForm.locationRecordModel.InTime = null;
                        RuChuKuForm.locationRecordModel.OutTime = DateTime.Now.ToString();
                        RuChuKuForm.locationRecordBLL.Add(RuChuKuForm.locationRecordModel);//update或delete一条数据
                        //listP.Reverse();//list中元素顺序翻转
                        executeCeShiLogNet.WriteInfo("到理货台");
                        while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP2, out commandType))//到库位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP2));
                            //if (this.IsHandleCreated)
                            //{

                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入" + commandType.ToString() + "指令";

                            }));
                            //}

                        }
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令运行中";

                            }));
                        }
                        executeCeShiLogNet.WriteAnyString(commandType.ToString() + "执行完成");
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令执行完成";

                        }));

                        while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Behind))//夹具伸出
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Behind);
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                       RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +

                                        RuChuKuForm.hostComputerCommand.ReadWrClamp(RuChuKuForm.siemensTcpNet).ToString() + " "

                                       );
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");
                        while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP3, out commandType1))
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP3));
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                      RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                      RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                      RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                      RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " "


                                      );
                            //if (this.IsHandleCreated)
                            //{

                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入" + commandType1.ToString() + "指令";

                            }));
                            //}

                        }//101
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = commandType1.ToString() + "指令运行中";

                            }));
                        }
                        executeCeShiLogNet.WriteAnyString(commandType1.ToString() + "执行完成");
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = commandType1.ToString() + "指令执行完成";

                        }));
                        //RuChuKuForm.locationRecordModel.OutTime = DateTime.Now.ToString();
                        //RuChuKuForm.locationRecordBLL.Add(RuChuKuForm.locationRecordModel);//update或delete一条数据

                        while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");

                    }
                    else  
                    {
                        p1.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet);
                        p1.Y = int.Parse(tb_DingDianBLL.GetModel("过道").Y);
                        p1.Z = int.Parse(tb_DingDianBLL.GetModel("过道").Z);

                        p2.X = int.Parse(tb_KuWeiModel.X);
                        p2.Y = p1.Y;
                        p2.Z = p1.Z;

                        p3.X = int.Parse(tb_KuWeiModel.X);
                        p3.Y = int.Parse(tb_KuWeiModel.Y);
                        p3.Z = int.Parse(tb_KuWeiModel.Z);

                        p4.X = p3.X;
                        p4.Y = p3.Y;
                        p4.Z = p3.Z + int.Parse(tb_KuWeiModel.Precision);



                        p6.X = int.Parse(tb_DingDianModel.X);
                        p6.Y = int.Parse(tb_DingDianModel.Y);
                        p6.Z = int.Parse(tb_DingDianModel.Z) + int.Parse(tb_DingDianModel.Precision);

                        p5.X = p6.X;
                        p5.Y = p1.Y;
                        p5.Z = p1.Z;



                        p7.X = p6.X;
                        p7.Y = p6.Y;
                        p7.Z = int.Parse(tb_DingDianModel.Z);

                        //第一步：103，过道，过道，库位
                        //listP.Add(p1);
                        //listP.Add(p2);
                        listP.Add(p3);
                        //第三步：101，升
                        listP1.Add(p4);
                        //第五步：103，过道，过道，理货台
                        listP2.Add(p2);
                        listP2.Add(p5);
                        listP2.Add(p6);
                        //第七步：101，降
                        listP3.Add(p7);

                        executeCeShiLogNet.WriteAnyString("去库位");
                        while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP, out commandType))//到库位
                        {

                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP));
                            //executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                            //          RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                            //          RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                            //          RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                            //           RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       //RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       //RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       //RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       //RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       //RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       //RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       //RuChuKuForm.hostComputerCommand.ReadWrZThree(RuChuKuForm.siemensTcpNet).ToString() + " "

                                      //);
                            //if (this.IsHandleCreated)
                            //{

                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入" + commandType.ToString() + "指令";

                            }));
                            //}

                        }
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令运行中";

                            }));
                        }
                        executeCeShiLogNet.WriteAnyString(commandType.ToString() + "执行完成");
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令执行完成";

                        }));

                        while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, clamp))//夹具伸出
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Front);
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                       RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +

                                        RuChuKuForm.hostComputerCommand.ReadWrClamp(RuChuKuForm.siemensTcpNet).ToString() + " "

                                       );
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");

                        while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP1, out commandType1))
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP1));
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                      RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                     RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                     RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " "


                                      );
                            //if (this.IsHandleCreated)
                            //{

                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入" + commandType1.ToString() + "指令";

                            }));
                            //}

                        }//101
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = commandType1.ToString() + "指令运行中";

                            }));
                        }
                        executeCeShiLogNet.WriteAnyString(commandType1.ToString() + "执行完成");
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = commandType1.ToString() + "指令执行完成";

                        }));


                        while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");
                        RuChuKuForm.locationRecordModel.Weight = RuChuKuForm.weight;
                        RuChuKuForm.locationRecordModel.InTime = null;
                        RuChuKuForm.locationRecordModel.OutTime = DateTime.Now.ToString();
                        RuChuKuForm.locationRecordBLL.Add(RuChuKuForm.locationRecordModel);//update或delete一条数据

                        //listP.Reverse();//list中元素顺序翻转
                        executeCeShiLogNet.WriteInfo("到理货台");
                        while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP2, out commandType))//到库位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP2));
                            //if (this.IsHandleCreated)
                            //{

                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入" + commandType.ToString() + "指令";

                            }));
                            //}

                        }
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令运行中";

                            }));
                        }
                        executeCeShiLogNet.WriteAnyString(commandType.ToString() + "执行完成");
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令执行完成";

                        }));

                        while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Behind))//夹具伸出
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Behind);
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                       RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +

                                        RuChuKuForm.hostComputerCommand.ReadWrClamp(RuChuKuForm.siemensTcpNet).ToString() + " "

                                       );
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");
                        while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP3, out commandType1))
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP3));
                            executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                      RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                      RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                      RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                       RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                      RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " "


                                      );
                            //if (this.IsHandleCreated)
                            //{

                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入" + commandType1.ToString() + "指令";

                            }));
                            //}

                        }//101
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = commandType1.ToString() + "指令运行中";

                            }));
                        }
                        executeCeShiLogNet.WriteAnyString(commandType1.ToString() + "执行完成");
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = commandType1.ToString() + "指令执行完成";

                        }));

                        while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                        {
                            executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "写入301指令";

                            }));
                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {
                            if (source.Token.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                source.Token.ThrowIfCancellationRequested();
                            }
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "301指令运行中";

                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "301指令执行完成";

                        }));
                        executeCeShiLogNet.WriteAnyString("301执行完成");

                    }
                    #endregion

                    executeCeShiLogNet.WriteAnyString("测试完成");


                    listP.Clear();
                    listP1.Clear();
                    listP2.Clear();
                    listP3.Clear();
                    listP0.Clear();
                }
                executeFlag = false;
                btnTest.BackColor = Color.Navy;
                this.BeginInvoke(new Action(() =>
                {
                    RuChuKuForm.statusLabel.Text = "任务执行完成";

                }));
                //写入1指令回到起点
                //tb_DingDianModel = tb_DingDianBLL.GetModel("起点");
                //p8.X = int.Parse(tb_DingDianModel.X);
                //p8.Y = int.Parse(tb_DingDianModel.Y);
                //p8.Z = int.Parse(tb_DingDianModel.Z);

                //p9.X = p7.X;
                //p9.Y = p1.Y;
                //p9.Z = p1.Z;

                //p10.X = p8.X;
                //p10.Y = p1.Y;
                //p10.Z = p1.Z;
                //listP4.Add(p9);
                //listP4.Add(p10);
                //listP4.Add(p8);

                //if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP4, out commandType))//到库位
                //{

                //    executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP4));
                //    executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                //              RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //              RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //              RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrZThree(RuChuKuForm.siemensTcpNet).ToString() + " "

                //              );
                //    //if (this.IsHandleCreated)
                //    //{

                //    this.BeginInvoke(new Action(() =>
                //    {
                //        RuChuKuForm.statusLabel.Text = "写入" + commandType.ToString() + "指令";

                //    }));
                //    //}

                //}
                //while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                //{
                //    this.BeginInvoke(new Action(() =>
                //    {
                //        RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令运行中";

                //    }));
                //}
                //executeCeShiLogNet.WriteAnyString(commandType.ToString() + "执行完成");
                //this.BeginInvoke(new Action(() =>
                //{

                //    RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令执行完成";

                //}));
                //executeCeShiLogNet.WriteAnyString("到达起点");
                //listP4.Clear();                //tb_DingDianModel = tb_DingDianBLL.GetModel("起点");
                //p8.X = int.Parse(tb_DingDianModel.X);
                //p8.Y = int.Parse(tb_DingDianModel.Y);
                //p8.Z = int.Parse(tb_DingDianModel.Z);

                //p9.X = p7.X;
                //p9.Y = p1.Y;
                //p9.Z = p1.Z;

                //p10.X = p8.X;
                //p10.Y = p1.Y;
                //p10.Z = p1.Z;
                //listP4.Add(p9);
                //listP4.Add(p10);
                //listP4.Add(p8);

                //if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP4, out commandType))//到库位
                //{

                //    executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + RuChuKuForm.hostComputerCommand.ListString(listP4));
                //    executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                //              RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //              RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //              RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //               RuChuKuForm.hostComputerCommand.ReadWrZThree(RuChuKuForm.siemensTcpNet).ToString() + " "

                //              );
                //    //if (this.IsHandleCreated)
                //    //{

                //    this.BeginInvoke(new Action(() =>
                //    {
                //        RuChuKuForm.statusLabel.Text = "写入" + commandType.ToString() + "指令";

                //    }));
                //    //}

                //}
                //while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                //{
                //    this.BeginInvoke(new Action(() =>
                //    {
                //        RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令运行中";

                //    }));
                //}
                //executeCeShiLogNet.WriteAnyString(commandType.ToString() + "执行完成");
                //this.BeginInvoke(new Action(() =>
                //{

                //    RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令执行完成";

                //}));
                //executeCeShiLogNet.WriteAnyString("到达起点");
                //listP4.Clear();


                //}
                //else
                //{
                //    MessageBox.Show("请手动开回起点位置");
                //}
                changeTxt("任务结束");
            },source.Token).ContinueWith(task => {

                executeCeShiLogNet.WriteInfo(task.Status.ToString());
                if (task.Status==TaskStatus.Canceled)
                {
                    RuChuKuForm.hostComputerCommand.WriteResetCommand(RuChuKuForm.siemensTcpNet);
                    executeFlag = false;
                    btnTest.BackColor = Color.Navy;
                    changeTxt("任务撤消");
                }
            }
                );


        }

        private ILogNet executeCeShiLogNet { get; set; }//执行测试日志

    }
}
