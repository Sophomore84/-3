using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Maticsoft.BLL;
using Maticsoft.Model;
using Maticsoft.DBUtility;
using ClassLibrary_Crane;
using System.Text.RegularExpressions;
using HslCommunication.Profinet.Siemens;
using HslCommunication;
using HslCommunication.LogNet;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using 自动化库存管理.Model;
using 自动化库存管理.Command;
using 养生池;

namespace 自动化库存管理
{

    //struct P
    //{
    //    public int X;
    //    public int Y;
    //    public int Z;
    //}
    public delegate void ChangeTaskTxt(string txt);
    public partial class DetailOperationRuKuForm : Form
    {
        #region 任务标记
        //任务标记
        CancellationTokenSource tokenSource = new CancellationTokenSource();
        //CancellationToken token;

        #endregion
        #region 库位缓存
       public static List<string> listLoca = new List<string>();
        #endregion
        //P A;
        //P B;
        //P C;
        //P D;
        //P E;
        //P S;
        int num;
        public event ChangeTaskTxt changeTxt;
        List<P> listP0 = new List<P>();
        List<P> listP = new List<P>();
        List<P> listP1 = new List<P>();
        List<P> listP2 = new List<P>();
        List<P> listP3 = new List<P>();
        List<P> listP4 = new List<P>();
        List<P> listP5 = new List<P>();
        P p0, p1, p2, p3, p4, p5,p6,p7,p8,p9,p10,p11,p12;
        int commandType, commandType1;

        Maticsoft.Model.LocationRecord locationRecordModel = new Maticsoft.Model.LocationRecord();
        Maticsoft.BLL.LocationRecord locationRecordBLL = new Maticsoft.BLL.LocationRecord();

        Maticsoft.Model.tb_DingDian tb_DingDianModel = new Maticsoft.Model.tb_DingDian();
        Maticsoft.BLL.tb_DingDian tb_DingDianBLL = new Maticsoft.BLL.tb_DingDian();

        Maticsoft.Model.tb_KuWei tb_KuWeiModel = new Maticsoft.Model.tb_KuWei();
        Maticsoft.BLL.tb_KuWei tb_KuWeiBLL = new Maticsoft.BLL.tb_KuWei();
        List<Assignment> assignments = new List<Assignment>();
        Assignment assignment = new Assignment();
        private ILogNet executeCeShiLogNet { get; set; }//执行测试日志
        private Thread threadExecute;//执行任务的线程
        bool executeFlag = false;//执行标志
        bool executeInFlag = false;//执行标志
        bool clampFlag = false;//夹具标志
        bool oneFlag = false;//201指令运行标志
        DataGridViewRowCollection dataGridViewRow;

        bool Type;//true: 出库；false: 入库
        KuWeiForm kuweiForm;
        //SearchForm searchFor;
        string QuHuoFangShi = "";
        bool resizeEnable = false;
        int orignalparentHeight;
        int orignalparentWidth;
        List<string> Data = new List<string>();
        Maticsoft.BLL.tb_KuCun KuCunBLL;//库存数据操作类
        //Maticsoft.BLL.tb_KuCun KuCunBLL;//库存数据操作类
        //public SiemensS7Net_New siemensTcpNet;//创建PLC连接
        //int failed = 0;//PLC连接失败的次数
        //bool longConnection = false;
        //public static string IP;
        //private bool bExecuting;

        public DetailOperationRuKuForm()
        {
            InitializeComponent();
            this.Resize += DetailOperationRuKuForm_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);
            //searchFor = new SearchForm();
            //searchFor.SetTextBoxEvent += SearchFor_SetTextBoxEvent;
            KuCunBLL = new Maticsoft.BLL.tb_KuCun();
            intidataGridView();
            
            //临时设置A/B/C/D/E/S 六个点位 $$    

            //LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\通讯异常", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件    
            //IP = "";
            //bExecuting = false;
            //InitPLC();
            //taskDo = new Thread(new System.Threading.ThreadStart(ExecuteMission));
            //taskDo.IsBackground = true;
        }
       
        private void intidataGridView()
        {
            //初始化任务列表
            this.dataGridViewRenWu.Font = new Font("华文楷体", 15);
            DataGridViewTextBoxColumn acCode = new DataGridViewTextBoxColumn();
            acCode.Name = "列0";
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
            acCode9.DataPropertyName = "列9";
            acCode9.HeaderText = "取货方式";
            this.dataGridViewRenWu.Columns.Add(acCode9);

            DataGridViewTextBoxColumn acCode10 = new DataGridViewTextBoxColumn();
            acCode10.Name = "状态";
            acCode10.DataPropertyName = "列10";
            acCode10.HeaderText = "状态";
            this.dataGridViewRenWu.Columns.Add(acCode10);
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
                    if (count > 0)
                    {
                        for (int i = 0; i < count; i++)
                        {

                            //zonggeshu += Convert.ToInt32(RuChuKuForm.KuCunList[i].ShuLiang);
                            //weizhi += RuChuKuForm.KuCunList[i].HuoJiaHao + "-" + RuChuKuForm.KuCunList[i].CengHao + "-" + RuChuKuForm.KuCunList[i].LieHao + ",";
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
            executeCeShiLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\入库指令", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            RuChuKuForm.m_BianMa = string.Empty;
            //this.Resize += DetailOperationRuKuForm_Resize;
            //X = this.Width;
            //Y = this.Height;
            //setTag(this);
            //resizeEnable = true;
            //orignalparentHeight = this.Height;
            //orignalparentWidth = this.Width;
            this.WindowState = FormWindowState.Maximized;
            #region 任务标记
            //任务标记
            
             //token = tokenSource.Token;

            #endregion

            //threadExecute = new Thread(new System.Threading.ThreadStart(DoExecute));//开启连续运动线程
            //threadExecute.IsBackground = true;
            ////Thread threadYD = new Thread(DoYD);//开启连续运动线程
            //threadExecute.Start();
        }

        private void DetailOperationRuKuForm_Resize(object sender, EventArgs e)
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

        private void DetailOperationForm_Resize(object sender, EventArgs e)
        {
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

        private void comboBoxQuHuoFangShi_DropDown(object sender, EventArgs e)
        {
            comboBoxQuHuoFangShi.Items.Clear();
            comboBoxQuHuoFangShi.Items.Add("自动");
            comboBoxQuHuoFangShi.Items.Add("人工");
        }

        private void comboBoxQuHuoFangShi_SelectedIndexChanged(object sender, EventArgs e)
        {
            QuHuoFangShi = comboBoxQuHuoFangShi.Text;
        }

        private void comboBoxQuHuoFangShi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;//取消输入事件
        }

        private void textBoxWeiZhi_Click(object sender, EventArgs e)
        {
            kuweiForm = new KuWeiForm();
            kuweiForm.SetWeiZhiEvent += KuweiForm_SetWeiZhiEvent;
            kuweiForm.ShowDialog();
        }

        private void KuweiForm_SetWeiZhiEvent()
        {
            textBoxWeiZhi .Text = RuChuKuForm.m_WeiZhi;
            

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tokenSource.Cancel();
        }
        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x0400011C RID: 284
        private float X;

        // Token: 0x0400011D RID: 285
        private float Y;
        private void DetailOperationRuKuForm_SizeChanged(object sender, EventArgs e)
        {
            float newx = (float)base.Width / this.X;
            float newy = (float)base.Height / this.Y;
            this.asc.setControls(newx, newy, this);
        }

        private void btnSuspend_Click(object sender, EventArgs e)
        {
            //RuChuKuForm.hostComputerCommand.WriteResetCommand(RuChuKuForm.siemensTcpNet);

        }

        private void txtBianMa_Click(object sender, EventArgs e)
        {
            SearchForm searchFor = new SearchForm();
            searchFor.SetTextBoxEvent += SearchFor_SetTextBoxEvent;
            searchFor.SetSearchType("bianma");
            //searchFor.ShowDialog();
            searchFor.Show();
        }

        private async void btnIn_Click(object sender, EventArgs e)
        {
            //dataGridViewRow = dataGridViewRenWu.Rows;
            //foreach (DataGridViewRow row in dataGridViewRow)
            //{

            //    assignments.Add(new Assignment()
            //    {
            //        Location= row.Cells["位置"].Value.ToString(),
            //        Code = row.Cells["编码"].Value.ToString(),
            //        Platform = row.Cells["理货台"].Value.ToString(),
            //        Number = row.Cells["数量"].Value.ToString(),
            //        Operation = row.Cells["取货方式"].Value.ToString(),
            //        Name = row.Cells["名称"].Value.ToString(),
            //        ContractNum = row.Cells["合同号"].Value.ToString()


            //    }

            //        );


            //}

            //事件
            try
            {
                if (executeInFlag)
                {
                    //if (btnIn.Text == "暂停")
                    //{

                    //}
                    executeInFlag = false;
                    btnIn.BackColor = Color.Navy;
                    //btnIn.Text = "测试入库";
                }
                else
                {
                    dataGridViewRenWu.Rows.Clear();
                    executeInFlag = true;
                    //btnIn.Text = "暂停";
                    btnIn.BackColor = Color.Red;
                    await DoExecuteInAsync(tokenSource.Token);


                }
            }
            catch (OperationCanceledException ex)
            {
                executeCeShiLogNet.WriteException("撤消操作",ex);
                RuChuKuForm.hostComputerCommand.WriteResetCommand(RuChuKuForm.siemensTcpNet);
            }
            catch (Exception ex)
            {

                executeCeShiLogNet.WriteException("入库异常", ex);
            }
            finally
            {
                tokenSource.Dispose();
            }
           
           

        }

        private void textBoxMingCheng_Click(object sender, EventArgs e)
        {
            SearchForm searchFor = new SearchForm();
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
            if (txtShuLiang.Text.Trim() == "" || textBoxKuCunZongLiang.Text.Trim() == "")
                return;
            if (!IsNumberic(txtShuLiang.Text.Trim()))
            {
                MessageBox.Show("格式错误！");
                txtShuLiang.Focus();
                return;
            }
        }

        private void btnAddMission_Click(object sender, EventArgs e)
        {
            if (txtBianMa.Text.Trim() != "" && textBoxMingCheng.Text.Trim() != "" &&
                txtShuLiang.Text.Trim() != "" && comboBoxQuHuoFangShi.Text.Trim() != ""
                && textBoxGuiGe.Text.Trim() != "" && textBoxXingHao.Text.Trim() != ""
                 && textBoxWeiZhi.Text.Trim() != "" && comboBoxLiHuoTai.Text.Trim() != "")
            {
                DataGridViewRow dr = new DataGridViewRow();
                dr.CreateCells(dataGridViewRenWu);
                dr.Cells[0].Value = dataGridViewRenWu.Rows.Count + 1;
                dr.Cells[1].Value = txtBianMa.Text;
                dr.Cells[2].Value = textBoxMingCheng.Text;
                dr.Cells[3].Value = textBoxGuiGe.Text;
                dr.Cells[4].Value = textBoxXingHao.Text;
                dr.Cells[5].Value = txtShuLiang.Text;
                dr.Cells[6].Value = textBoxHeTongHao.Text;
                dr.Cells[7].Value = textBoxWeiZhi.Text;
                dr.Cells[8].Value = comboBoxLiHuoTai.Text;
                dr.Cells[9].Value = comboBoxQuHuoFangShi.Text;
                dr.Cells[10].Value = "等待执行";
                RuChuKuForm.shangHuoTai = comboBoxLiHuoTai.Text;
                //添加的行作为最后一行
                dataGridViewRenWu.Rows.Add(dr);
                //listLoca.Add(RuChuKuForm.m_WeiZhi);
                listLoca.Add(textBoxWeiZhi.Text);
                


                assignments.Add(new Assignment()
                {
                    Id = (++num).ToString(),
                    Location = textBoxWeiZhi.Text,
                    Code = txtBianMa.Text,
                    Platform = comboBoxLiHuoTai.Text,
                    Number = txtShuLiang.Text,
                    Operation = comboBoxQuHuoFangShi.Text,
                    Name = textBoxMingCheng.Text,
                    ContractNum = textBoxHeTongHao.Text


                }

                    ) ;

                txtBianMa.Text = "";
                textBoxMingCheng.Text = "";
                txtShuLiang.Text = "";
                comboBoxQuHuoFangShi.Text = "";
                textBoxGuiGe.Text = "";
                textBoxXingHao.Text = "";
                textBoxWeiZhi.Text = "";
                textBoxKuCunZongLiang.Text = "";
                textBoxHeTongHao.Text = "";
                comboBoxLiHuoTai.Text = "";

            }
            else
            {
                MessageBox.Show("信息不完整！");
            }
        }
        private void dataGridViewRenWu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DialogResult RSS = MessageBox.Show(this, "确定要删除选中行数据码？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                switch (RSS)
                {
                    case DialogResult.Yes:
                        for (int i = this.dataGridViewRenWu.SelectedRows.Count; i > 0; i--)
                        {
                            dataGridViewRenWu.Rows.RemoveAt(dataGridViewRenWu.SelectedRows[i - 1].Index);
                            //assignments.RemoveAt();//任务列表中删除
                            //ListLoc//从任务缓存中删除
                            --num;
                        }
                        assignments.Clear();
                        listLoca.Clear();
                        //num = 0;
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
                            listLoca.Add(row.Cells["位置"].Value.ToString());


                        }
                        RuChuKuForm.shangHuoTai = null;
                        MessageBox.Show("成功删除选中行数据！");
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }
        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (RuChuKuForm.bExecuting)
            {
                //int index = comboBoxLiHuoTai.SelectedIndex;//获取或设置指定当前选定项的索引。
                
                RuChuKuForm.bExecuting = false;
                btnExecute.Text = "执行";
            }                
            else
            {
                RuChuKuForm.bExecuting = true;
                btnExecute.Text = "暂停";
            }
            //taskDo.Start();        
        }
        //public void ExecuteMission()
        //{
        //    while(bExecuting)
        //    {
        //        try
        //        {
        //            string tipInfo = "";
        //            //读取PLC地址块信息
        //            HslCommunication.OperateResult<byte[]> buff2 = siemensTcpNet.Read("DB101.04", 16);
        //            bool isSuccess = buff2.IsSuccess;
        //            if (isSuccess)
        //            {
        //                ///**************************PLC反馈数据**************************/
        //                int CurrentX = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                int CurrentY = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 4);
        //                int CurrentZ = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 8);
        //                int CurrentHuoChaWeiZhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 12);
        //                //siemensTcpNet.Write("DB4.00", recData.ZongZhongLiang);
        //                tipInfo = "当前位置： " + CurrentX + " " + CurrentY + " " + CurrentZ;
        //                LogNet.WriteDebug(tipInfo);
        //                //this.toolStripStatusLabel2.Text = tipInfo;
        //                Thread.Sleep(2000);
        //                if (CurrentHuoChaWeiZhi != 100)//货叉未归原位
        //                {
        //                    siemensTcpNet.Write("DB100.04", 300);
        //                    siemensTcpNet.Write("DB100.60", 3);
        //                    tipInfo = "货叉归原位......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;
        //                    //开始接收PLC反馈指令
        //                    bool flag = true;
        //                    while (flag)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 20);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 16);
        //                            if (zhiling == 3 && huochaweizhi == 100)
        //                                flag = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                }
        //                if (CurrentX != 0 || CurrentY != 0)//航车不在起始点S位置（0，0，Z）
        //                {
        //                    //返回起始点S位置
        //                    tipInfo = "航车返回起始点S......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;
        //                }
        //                //开始入库
        //                if (RuChuKuForm.m_RuKuChuKuFlag)//航车走到理货台
        //                {
        //                    //S-C-B-A点
        //                    siemensTcpNet.Write("DB100.04", 103);
        //                    siemensTcpNet.Write("DB100.08", C.X);
        //                    siemensTcpNet.Write("DB100.12", C.Y);
        //                    siemensTcpNet.Write("DB100.16", C.Z);
        //                    siemensTcpNet.Write("DB100.20", B.X);
        //                    siemensTcpNet.Write("DB100.24", B.Y);
        //                    siemensTcpNet.Write("DB100.28", B.Z);
        //                    siemensTcpNet.Write("DB100.32", A.X);
        //                    siemensTcpNet.Write("DB100.36", A.Y);
        //                    siemensTcpNet.Write("DB100.40", A.Z);
        //                    tipInfo = "航车行驶至理货台......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;
        //                    //接收PLC反馈
        //                    bool flagT = true;
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 16);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            int X = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 4);
        //                            int Y = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 8);
        //                            int Z = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 12);
        //                            if (zhiling == 1)
        //                                if (X == A.X && Y == A.Y && Z == A.Z)
        //                                    flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                    //发送货叉伸出指令 
        //                    siemensTcpNet.Write("DB100.04", 300);
        //                    siemensTcpNet.Write("DB100.56", 1);
        //                    siemensTcpNet.Write("DB100.60", 1);//$$ 暂定向左
        //                    tipInfo = "货叉取货伸出......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;

        //                    //开始接收PLC反馈指令
        //                    flagT = true;
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 20);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 16);
        //                            if (zhiling == 3 && huochaweizhi == 500)// $$ 假设货叉全部伸出为500
        //                                flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                    //货叉抬升
        //                    siemensTcpNet.Write("DB100.04", 201);
        //                    siemensTcpNet.Write("DB100.08", A.X);
        //                    siemensTcpNet.Write("DB100.12", A.Y);
        //                    siemensTcpNet.Write("DB100.16", A.Z - 100);// $$
        //                    tipInfo = "货叉取货抬升......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;

        //                    //开始接收PLC反馈指令
        //                    flagT = true;
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 4);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            if (zhiling == 2)
        //                                flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                    //发送货叉缩指令
        //                    siemensTcpNet.Write("DB100.04", 300);
        //                    siemensTcpNet.Write("DB100.60", 3);
        //                    tipInfo = "货叉取货回缩......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;

        //                    //开始接收PLC反馈指令
        //                    flagT = true;
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 20);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 16);
        //                            if (zhiling == 3 && huochaweizhi == 100)// $$ 假设货叉全部缩回为100
        //                                flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                    //A-B-C-D点
        //                    siemensTcpNet.Write("DB100.04", 103);
        //                    siemensTcpNet.Write("DB100.08", B.X);
        //                    siemensTcpNet.Write("DB100.12", B.Y);
        //                    siemensTcpNet.Write("DB100.16", B.Z);
        //                    siemensTcpNet.Write("DB100.20", C.X);
        //                    siemensTcpNet.Write("DB100.24", C.Y);
        //                    siemensTcpNet.Write("DB100.28", C.Z);
        //                    siemensTcpNet.Write("DB100.32", D.X);
        //                    siemensTcpNet.Write("DB100.36", D.Y);
        //                    siemensTcpNet.Write("DB100.40", D.Z);
        //                    tipInfo = "航车入库行驶A-B-C-D......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;

        //                    //接收PLC反馈
        //                    flagT = true;
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 4);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            if (zhiling == 1)
        //                                flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                    //D-E点
        //                    siemensTcpNet.Write("DB100.04", 201);
        //                    siemensTcpNet.Write("DB100.08", E.X);
        //                    siemensTcpNet.Write("DB100.12", E.Y);
        //                    siemensTcpNet.Write("DB100.16", E.Z);
        //                    tipInfo = "航车入库行驶至货架D-E......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;

        //                    //接收PLC反馈
        //                    flagT = true;
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 16);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            int X = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 4);
        //                            int Y = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 8);
        //                            int Z = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 12);
        //                            if (zhiling == 2)
        //                                if (X == E.X && Y == E.Y && Z == E.Z)
        //                                    flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                    //发送货叉伸出指令 
        //                    siemensTcpNet.Write("DB100.04", 300);
        //                    siemensTcpNet.Write("DB100.56", 2);
        //                    siemensTcpNet.Write("DB100.60", 1);//$$ 暂定向左
        //                    tipInfo = "货叉放货伸......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;

        //                    //开始接收PLC反馈指令
        //                    flagT = true;
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 20);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 16);
        //                            if (zhiling == 3 && huochaweizhi == 500)// $$ 假设货叉全部伸出为500
        //                                flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                    //货叉降
        //                    siemensTcpNet.Write("DB100.04", 201);
        //                    siemensTcpNet.Write("DB100.08", E.X);
        //                    siemensTcpNet.Write("DB100.12", E.Y);
        //                    siemensTcpNet.Write("DB100.16", E.Z + 100);// $$

        //                    tipInfo = "货叉放货降......";
        //                    LogNet.WriteDebug(tipInfo);
        //                    //this.toolStripStatusLabel2.Text = tipInfo;

        //                    //开始接收PLC反馈指令
        //                    flagT = true; 
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 4);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            if (zhiling == 2)
        //                                flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                    //发送货叉缩指令
        //                    siemensTcpNet.Write("DB100.04", 300);
        //                    siemensTcpNet.Write("DB100.60", 3);

        //                    tipInfo = "货叉放货回缩......";
        //                    LogNet.WriteDebug(tipInfo);
        //                   // this.toolStripStatusLabel2.Text = tipInfo;

        //                    //开始接收PLC反馈指令
        //                    flagT = true;
        //                    while (flagT)
        //                    {
        //                        buff2 = siemensTcpNet.Read("DB101.00", 20);
        //                        isSuccess = buff2.IsSuccess;
        //                        if (isSuccess)
        //                        {
        //                            int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
        //                            int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 16);
        //                            if (zhiling == 3 && huochaweizhi == 100)// $$ 假设货叉全部缩回为100
        //                                flagT = false;
        //                        }
        //                        //休眠2秒
        //                        Thread.Sleep(2000);
        //                    }
        //                }
        //                else//开始出库
        //                {
        //                    //先走到对应库位 $$

        //                    //发送货叉伸出指令 $$
        //                }
        //            }
        //            else
        //                MessageBox.Show("读取失败!");
        //        }
        //        catch (Exception e)
        //        {
        //            MessageBox.Show(e.Message);
        //        }
        //    }            
        //}
        ///// <summary>
        ///// 系统的日志记录器
        ///// </summary>
        //private ILogNet LogNet { get; set; }

        //private Thread taskDo;
        
        //private void InitPLC()
        //{
        //    if (IP == "")
        //        IP = "192.168.0.1";
        //    siemensTcpNet = new SiemensS7Net_New(SiemensPLCS.S1500, IP) { ConnectTimeOut = 1000 };
        //    ConnectPLC();
        //}
        //private bool ConnectPLC()
        //{
        //    OperateResult connect = siemensTcpNet.ConnectServer();
        //    if (connect.IsSuccess)
        //    {
        //        longConnection = true;
        //        LogNet.WriteDebug("PLC连接成功！");
        //        // this.toolStripStatusLabel2.Text = StringParse("PLC", this.toolStripStatusLabel2.Text, "PLC连接成功！");
        //        return true;
        //    }
        //    else
        //    {
        //        longConnection = false;
        //        LogNet.WriteDebug("PLC连接失败！");
        //        // this.toolStripStatusLabel2.Text = StringParse("PLC", this.toolStripStatusLabel2.Text, "PLC连接失败！");
        //        return false;
        //    }
        //}

        //private void DisconnectPLC()
        //{
        //    if (siemensTcpNet != null)
        //        siemensTcpNet.ConnectClose();
        //    longConnection = false;
        //}
        public string StringParse(string specialString, string text, string addtext)
        {
            string retString = "";
            int index = text.IndexOf(specialString);
            if (index == -1)
            {
                retString = text + addtext;
                return retString;
            }
            if (index == 0)
            {
                index = text.IndexOf('！');
                if (index >= 0 && index < text.Length - 1)
                    retString = text.Substring(index + 1);
                retString += addtext;
            }
            else
            {
                if (index >= 0 && index <= text.Length)
                    retString = text.Substring(0, index);
                retString += addtext;
            }
            return retString;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            
            dataGridViewRow = dataGridViewRenWu.Rows;
            foreach (DataGridViewRow row in dataGridViewRow)
            {
                assignment.Location = row.Cells["位置"].Value.ToString();
                assignment.Code = row.Cells["编码"].Value.ToString();
                assignment.Platform = row.Cells["理货台"].Value.ToString();
                assignment.Number= row.Cells["数量"].Value.ToString();
                assignment.Operation= row.Cells["取货方式"].Value.ToString();
                assignment.Name= row.Cells["名称"].Value.ToString();
                assignment.ContractNum= row.Cells["合同号"].Value.ToString();
                assignments.Add(assignment);
                //MessageBox.Show(row.Cells["列7"].Value.ToString());
            }
            if (executeFlag)
            {
                executeFlag = false;
                //btnTest.BackColor = Color.Navy;
            }
            else
            {
                executeFlag = true;
                //btnTest.BackColor = Color.Red;
                await DoExecuteAsync();
                
            }
        }
        public async Task DoExecuteAsync()
        {

            
           await Task.Run(()=>
            {
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
                    RuChuKuForm.locationRecordModel.GoodsNum = "+" + assi.Number;
                    RuChuKuForm.locationRecordModel.LocationName = assi.Location;
                    RuChuKuForm.locationRecordModel.Action = assi.Operation;

                    #region 上货台：301 1，301 3，104....
                    if (RuChuKuForm.hostComputerCommand.ReadClamp(RuChuKuForm.siemensTcpNet) != 4)//如果夹具不在原位，回到原位
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
                            p3.Z = int.Parse(tb_KuWeiModel.Z) - 500;

                            p4.X = p3.X;
                            p4.Y = p3.Y;
                            p4.Z = p3.Z + 500;



                            p6.X = int.Parse(tb_DingDianModel.X);
                            p6.Y = int.Parse(tb_DingDianModel.Y);
                            p6.Z = int.Parse(tb_DingDianModel.Z) - 500;

                            p5.X = p6.X;
                            p5.Y = p1.Y;
                            p5.Z = p1.Z;



                            p7.X = p6.X;
                            p7.Y = p6.Y;
                            p7.Z = p6.Z + 500;




                            //第一步：103，过道，过道，库位
                            listP.Add(p1);
                            listP.Add(p2);
                            listP.Add(p3);
                            //第三步：101，降
                            listP1.Add(p4);
                            //第五步：103，过道，过道，理货台
                            listP2.Add(p2);
                            listP2.Add(p5);
                            listP2.Add(p6);
                            //第七步：101，升
                            listP3.Add(p7);

                            executeCeShiLogNet.WriteAnyString("去库位");
                            if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP, out commandType))//到库位
                            {

                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + ListString(listP));
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

                            if (RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Front))//夹具伸出
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

                            if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP1, out commandType1))
                            {
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + ListString(listP1));
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

                            //listP.Reverse();//list中元素顺序翻转
                            executeCeShiLogNet.WriteInfo("到理货台");
                            if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP2, out commandType))//到库位
                            {
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + ListString(listP2));
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

                            if (RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Front))//夹具伸出
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
                            if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP3, out commandType1))
                            {
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + ListString(listP3));
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
                            //update或delete一条数据

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

                        }
                        #endregion

                        executeCeShiLogNet.WriteAnyString("测试完成");


                        listP.Clear();
                        listP1.Clear();
                        listP2.Clear();
                        listP3.Clear();
                        listP0.Clear();
                    }
                    //写入1指令回到起点
                    tb_DingDianModel = tb_DingDianBLL.GetModel("起点");
                    p8.X = int.Parse(tb_DingDianModel.X);
                    p8.Y = int.Parse(tb_DingDianModel.Y);
                    p8.Z = int.Parse(tb_DingDianModel.Z);

                    p9.X = p7.X;
                    p9.Y = p1.Y;
                    p9.Z = p1.Z;

                    p10.X = p8.X;
                    p10.Y = p1.Y;
                    p10.Z = p1.Z;
                    listP4.Add(p9);
                    listP4.Add(p10);
                    listP4.Add(p8);

                    if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP4, out commandType))//到库位
                    {

                        executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + ListString(listP4));
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
                    executeCeShiLogNet.WriteAnyString("到达起点");
                    listP4.Clear();


                //}
                //else
                //{
                //    MessageBox.Show("请手动开回起点位置");
                //}

            });
            

        }
        public string  ListString(List<P> list)
        {
            string str = "";
            foreach (P p in list)
            {
                str = p.X.ToString() + " " + p.Y.ToString() + " " + p.Z.ToString() + " ";
            }
            return str;
        }

        public async Task DoExecuteInAsync(CancellationToken ct)
        {
            try
            {
                await Task.Run(() =>
                {
                    
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                    //判断是否是起点
                    //if (
                    //Math.Abs(RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet)-int.Parse(tb_DingDianBLL.GetModel("起点").X))<50
                    //&&Math.Abs(RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet) - int.Parse(tb_DingDianBLL.GetModel("起点").Y)) < 50
                    //&& Math.Abs(RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet) - int.Parse(tb_DingDianBLL.GetModel("起点").Z)) < 50
                    //)
                    //{
                    //int i = 0;
                    foreach (Assignment assi in assignments)
                    {
                        //dataGridViewRenWu.Rows[0].Cells[10].Value = "正在执行";
                        RuChuKuForm.locationRecordModel.GoodsCode = assi.Code;
                        RuChuKuForm.locationRecordModel.GoodsName = assi.Name;
                        RuChuKuForm.locationRecordModel.GoodsContract = assi.ContractNum;
                        RuChuKuForm.locationRecordModel.GoodsNum = "+" + assi.Number;
                        RuChuKuForm.locationRecordModel.LocationName = assi.Location;
                        RuChuKuForm.locationRecordModel.Action = assi.Operation;
                        //locationRecordModel.UserName=assi.
                        int clamp = RuChuKuForm.hostComputerCommand.QueryClamp(assi.Location);
                        //executeCeShiLogNet.WriteAnyString("测试开始");
                        tb_KuWeiModel = tb_KuWeiBLL.GetModel(assi.Location);
                        //switch (assi.Platform)
                        //{
                        //    case "1":
                        //    case "2":
                        //    case "3":
                        //    case "4":
                        //        tb_DingDianModel = tb_DingDianBLL.GetModel("上货台" + assi.Platform);
                        //        break;
                        //    case "5"://缩回，到库位，放下，伸出
                        //        RuChuKuForm.hostComputerCommand.InAtAnyLocation(RuChuKuForm.siemensTcpNet);
                        //        return;
                        //        //货叉缩回
                        //        //103:实时位置到库位
                        //        //货叉伸
                        //        //货叉缩回
                        //        //break;
                        //    default:
                        //        break;
                        //}
                        tb_DingDianModel = tb_DingDianBLL.GetModel("上货台" + assi.Platform);

                        changeTxt("入库 上货台：" + assi.Platform + " 库位:" + assi.Location);
                        executeCeShiLogNet.WriteInfo("入库 上货台：" + assi.Platform + " 库位:" + assi.Location);

                        #region 当前位置到上货台，到库位，最后到起点
                        if (RuChuKuForm.hostComputerCommand.ReadClamp(RuChuKuForm.siemensTcpNet) == 1 || RuChuKuForm.hostComputerCommand.ReadClamp(RuChuKuForm.siemensTcpNet) == 2)//如果夹具不在原位，回到原位
                        {
                            if (ct.IsCancellationRequested)
                            {
                                // Clean up here, then...
                                ct.ThrowIfCancellationRequested();
                            }

                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "写入301指令";

                                }));

                            }


                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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

                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Front))//夹具原位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "写入301指令";

                                }));
                            }


                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "写入301指令";

                                }));
                            }


                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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


                        if (Math.Abs(RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet) - int.Parse(tb_DingDianModel.X)) > 10)//如果大车实时位置与目标位置不相等//103,过道2，过道1，库位
                        {

                            p1.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet);
                            p1.Y = int.Parse(tb_DingDianBLL.GetModel("过道").Y);
                            p1.Z = int.Parse(tb_DingDianBLL.GetModel("过道").Z);

                            p2.X = int.Parse(tb_DingDianModel.X);
                            p2.Y = p1.Y;
                            p2.Z = p1.Z;

                            p3.X = p2.X;
                            p3.Y = int.Parse(tb_DingDianModel.Y);
                            p3.Z = int.Parse(tb_DingDianModel.Z);

                            p4.X = p3.X;
                            p4.Y = p3.Y;
                            p4.Z = p3.Z + int.Parse(tb_DingDianModel.Precision);



                            p6.X = int.Parse(tb_DingDianModel.X);
                            p6.Y = int.Parse(tb_DingDianModel.Y);
                            p6.Z = int.Parse(tb_DingDianModel.Z);

                            p5.X = p6.X;
                            p5.Y = p1.Y;
                            p5.Z = p1.Z;

                            p11.X = int.Parse(tb_KuWeiModel.X);
                            p11.Y = p1.Y;
                            p11.Z = p1.Z;

                            p12.X = p11.X;
                            p12.Y = int.Parse(tb_KuWeiModel.Y);
                            p12.Z = int.Parse(tb_KuWeiModel.Z) + int.Parse(tb_KuWeiModel.Precision);

                            p0.X = p12.X;
                            p0.Y = p12.Y;
                            p0.Z = int.Parse(tb_KuWeiModel.Z);





                            //第一步：103，起点，过道，过道，上货台
                            listP.Add(p1);
                            listP.Add(p2);
                            listP.Add(p3);
                            //第二步：301，伸出，方向待定
                            //第三步：101，升起//第四步:301，夹具原位。//夹具位置，p3为低点,p4为高点
                            listP1.Add(p4);

                            //第五部：103，上货台，过道，过道，库位
                            listP5.Add(p2);
                            listP5.Add(p11);
                            listP5.Add(p12);
                            //第六步：301，夹具伸出
                            //第七步：101，降下//第八步，301，夹具原位
                            listP0.Add(p0);






                            executeCeShiLogNet.WriteAnyString("到理货台");
                            //第一步：103，起点，过道，过道，上货台
                            while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP, out commandType))//到库位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + ListString(listP));
                                executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                          RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                          RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                          RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrYTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrYThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            //第二步：301，伸出，方向待定
                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Behind))//夹具伸出
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            //第三步：101，升起
                            while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP1, out commandType1))
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + ListString(listP1));
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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

                            //第四步：301，夹具回到原位
                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "写入301指令";

                                }));
                            }


                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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


                            executeCeShiLogNet.WriteInfo("理货台到库位");
                            //第五部：103，上货台，过道，过道，库位
                            while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP5, out commandType))//到库位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + ListString(listP5));
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令运行中";

                                }));
                            }
                            RuChuKuForm.locationRecordModel.Weight = RuChuKuForm.weight;
                            executeCeShiLogNet.WriteAnyString(commandType.ToString() + "执行完成");
                            this.BeginInvoke(new Action(() =>
                            {

                                RuChuKuForm.statusLabel.Text = commandType.ToString() + "指令执行完成";

                            }));

                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, clamp))//夹具伸出
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                //executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Front);
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            //第七步：101，降下
                            while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP0, out commandType1))
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + ListString(listP0));
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            RuChuKuForm.locationRecordModel.InTime = DateTime.Now.ToString();
                            RuChuKuForm.locationRecordModel.OutTime = null;
                            RuChuKuForm.locationRecordBLL.Add(RuChuKuForm.locationRecordModel);//update或delete一条数据

                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "写入301指令";

                                }));
                            }


                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            executeCeShiLogNet.WriteInfo("入库 上货台：" + assi.Platform + " 库位:" + assi.Location + "完成");
                            //dataGridViewRenWu.Rows[0].Cells[10].Value = "执行完成";
                            //dataGridViewRenWu.Rows.RemoveAt(0);
                        }
                        else//
                        {


                            p1.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet);
                            p1.Y = int.Parse(tb_DingDianBLL.GetModel("过道").Y);
                            p1.Z = int.Parse(tb_DingDianBLL.GetModel("过道").Z);

                            p2.X = int.Parse(tb_DingDianModel.X);
                            p2.Y = p1.Y;
                            p2.Z = p1.Z;

                            p3.X = p2.X;
                            p3.Y = int.Parse(tb_DingDianModel.Y);
                            p3.Z = int.Parse(tb_DingDianModel.Z);

                            p4.X = p3.X;
                            p4.Y = p3.Y;
                            p4.Z = p3.Z + int.Parse(tb_DingDianModel.Precision);



                            p6.X = int.Parse(tb_DingDianModel.X);
                            p6.Y = int.Parse(tb_DingDianModel.Y);
                            p6.Z = int.Parse(tb_DingDianModel.Z);

                            p5.X = p6.X;
                            p5.Y = p1.Y;
                            p5.Z = p1.Z;

                            p11.X = int.Parse(tb_KuWeiModel.X);
                            p11.Y = p1.Y;
                            p11.Z = p1.Z;

                            p12.X = p11.X;
                            p12.Y = int.Parse(tb_KuWeiModel.Y);
                            p12.Z = int.Parse(tb_KuWeiModel.Z) + int.Parse(tb_KuWeiModel.Precision);

                            p0.X = p12.X;
                            p0.Y = p12.Y;
                            p0.Z = p12.Z - int.Parse(tb_KuWeiModel.Precision);





                            //第一步：103，起点，过道，过道，上货台
                            //listP.Add(p1);
                            //listP.Add(p2);
                            listP.Add(p3);
                            //第二步：301，伸出，方向待定
                            //第三步：101，升起//第四步:301，夹具原位。//夹具位置，p3为低点,p4为高点
                            listP1.Add(p4);

                            //第五部：103，上货台，过道，过道，库位
                            listP5.Add(p2);
                            listP5.Add(p11);
                            listP5.Add(p12);
                            //第六步：301，夹具伸出
                            //第七步：101，降下//第八步，301，夹具原位
                            listP0.Add(p0);






                            executeCeShiLogNet.WriteAnyString("到理货台");
                            //第一步：103，起点，过道，过道，上货台
                            while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP, out commandType))//到库位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + ListString(listP));
                                //executeCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                //          RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //          RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //          RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //           RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //           RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //           RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //           RuChuKuForm.hostComputerCommand.ReadWrYTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //           RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //           RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //           RuChuKuForm.hostComputerCommand.ReadWrYThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                //           RuChuKuForm.hostComputerCommand.ReadWrZThree(RuChuKuForm.siemensTcpNet).ToString() + " "

                                //          );
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            //第二步：301，伸出，方向待定
                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Behind))//夹具伸出
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            //第三步：101，升起
                            while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP1, out commandType1))
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + ListString(listP1));
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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

                            //第四步：301，夹具回到原位
                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "写入301指令";

                                }));
                            }


                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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


                            executeCeShiLogNet.WriteInfo("理货台到库位");
                            //第五部：103，上货台，过道，过道，库位
                            while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP5, out commandType))//到库位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + ListString(listP5));
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            RuChuKuForm.locationRecordModel.Weight = RuChuKuForm.weight;
                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, clamp))//夹具伸出
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                //executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Front);
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            //第七步：101，降下
                            while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP0, out commandType1))
                            {
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType1.ToString() + "  " + ListString(listP0));
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
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            RuChuKuForm.locationRecordModel.OutTime = null;
                            RuChuKuForm.locationRecordModel.InTime = DateTime.Now.ToString();
                            RuChuKuForm.locationRecordBLL.Add(RuChuKuForm.locationRecordModel);//update或delete一条数据

                            while (!RuChuKuForm.hostComputerCommand.WriteThreeZeroOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, (int)HostComputerCommand.CommandTye.ThreeZeroOne, (int)HostComputerCommand.Clamp.Median))//夹具原位
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
                                }
                                executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + HostComputerCommand.CommandTye.ThreeZeroOne + " " + HostComputerCommand.Clamp.Median);
                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "写入301指令";

                                }));
                            }


                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                            {
                                if (ct.IsCancellationRequested)
                                {
                                    // Clean up here, then...
                                    ct.ThrowIfCancellationRequested();
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
                            executeCeShiLogNet.WriteInfo("入库 上货台：" + assi.Platform + " 库位:" + assi.Location + "完成");
                            //dataGridViewRenWu.Rows.RemoveAt(0);
                            //i++;
                        }
                        executeInFlag = false;
                        btnIn.BackColor = Color.Navy;
                        #endregion

                        executeCeShiLogNet.WriteAnyString("测试完成");


                        listP.Clear();
                        listP1.Clear();
                        listP5.Clear();
                        listP0.Clear();
                    }

                    this.BeginInvoke(new Action(() =>
                    {
                        RuChuKuForm.statusLabel.Text = "任务执行完成";
                        //listLoca.Clear();
                    }));
                    //第九步，103，库位，过道，过道，起点
                    //写入1指令回到起点
                    //tb_DingDianModel = tb_DingDianBLL.GetModel("起点");
                    //p8.X = int.Parse(tb_DingDianModel.X);
                    //p8.Y = int.Parse(tb_DingDianModel.Y);
                    //p8.Z = int.Parse(tb_DingDianModel.Z);

                    //p9.X = p0.X;
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

                    //    executeCeShiLogNet.WriteInfo(RuChuKuForm.counter.ToString() + "  " + commandType.ToString() + "  " + ListString(listP4));
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
                    changeTxt("任务运行结束");

                }

           );//.ContinueWith(task => executeCeShiLogNet.WriteInfo(task.Status.ToString()))

            }
            catch (OperationCanceledException ex)
            {
                //ContinueWith(task => executeCeShiLogNet.WriteInfo(task.Status.ToString()));
                executeCeShiLogNet.WriteInfo("撤消操作");
                RuChuKuForm.hostComputerCommand.WriteResetCommand(RuChuKuForm.siemensTcpNet);
                changeTxt("任务撤消");
                executeInFlag = false;
                btnIn.BackColor = Color.Navy;
            }
            catch (Exception ex)
            {

                executeCeShiLogNet.WriteException("入库异常", ex);
            }
            finally
            {
                tokenSource.Dispose();
            }

        }
}
}
