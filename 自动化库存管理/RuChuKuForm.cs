using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using ClassLibrary_Crane;
using HslCommunication.Profinet.Siemens;
using HslCommunication;
using HslCommunication.LogNet;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using 自动化库存管理.Command;
using HslCommunication.Enthernet;
using HslCommunication.Core.Net;
using Newtonsoft.Json.Linq;
using SocketClient;
using 养生池;
using System.Windows.Input;
using 自动化库存管理.SerialPortHum;
using System.IO.Ports;
using ConsoleApp1.Command;
using System.IO;

namespace 自动化库存管理
{

    public struct P
    {
        public int X;
        public int Y;
        public int Z;
    }
    public partial class RuChuKuForm : Form
    {
        //private string selectedDName;
        public static TextBox textTasks;
        bool xinTiaoFlag = false;
        DateTime dateTimeBegin, dateTimeEnd;
        TimeSpan timeSpan;
        HslCommunication.OperateResult<byte[]> buffReadFanKui;
        Maticsoft.Model.tb_DingDian tb_DingDianModelFanKui = new Maticsoft.Model.tb_DingDian();
        Maticsoft.BLL.tb_DingDian tb_DingDianBLLFanKui = new Maticsoft.BLL.tb_DingDian();

        bool jiTingFlag = true, zanTingFlag = true;
        public static ToolStripStatusLabel statusLabel;
        public static int counter = 0;//自增量指令

        Maticsoft.Model.tb_KuCunRecords tb_KuCunRecordsModel = new Maticsoft.Model.tb_KuCunRecords();
        Maticsoft.BLL.tb_KuCunRecords tb_KuCunRecordsBLL = new Maticsoft.BLL.tb_KuCunRecords();

        Maticsoft.Model.tb_DingDian tb_DingDianModel = new Maticsoft.Model.tb_DingDian();
        Maticsoft.BLL.tb_DingDian tb_DingDianBLL = new Maticsoft.BLL.tb_DingDian();

        Maticsoft.Model.tb_KuWei tb_KuWeiModel = new Maticsoft.Model.tb_KuWei();
        Maticsoft.BLL.tb_KuWei tb_KuWeiBLL = new Maticsoft.BLL.tb_KuWei();

        HslCommunication.OperateResult<byte[]> buffRead;

        System.Threading.Timer ruchuStateTimer;

        public static string shangHuoTai;//上货台
        public static string m_BianMa;
        public static string m_MingCheng;
        public static string m_GuiGe;
        public static string m_XingHao;
        public static string m_HeTongHao;
        public static string m_WeiZhi;
        public static bool m_RuKuChuKuFlag;//true:入库；false:出库

        public static string loginname = "";//用户登录名
        public static bool isLogin = false;//是否登录成功，登录成功后才能进行搬库操作
        public static string LoginType = "";//登录类型：系统管理员/操作员/维修工等（与114-1数据库一致）

        public static SiemensS7Net_New siemensTcpNet;//创建PLC连接
        public static int failed = 0;//PLC连接失败的次数
        public static bool longConnection = false;
        public static string IP;
        public static bool bExecuting;

        public static int HuoChaWuCha;
        public static int HuoJiaWuCha;

        public static P A;
        public static P B;
        public static P C;
        public static P D;
        public static P E;
        public static P S;

        bool resizeEnable = false;
        int orignalparentHeight;
        int orignalparentWidth;

        public static List<Maticsoft.Model.LocationRecord> locationRecordList;
        public static List<Maticsoft.Model.tb_KuCun> KuCunList;
        public static DetailOperationRuKuForm detailRuKuForm;
        public static DetailOperationForm detailChuKuForm;
        public static loginForm logForm;
        public static RenGongGuanLi renGongGuanLi;
        public static SheBeiCanShu sheBeiCanShu;
        public static SheZhiForm shezhiFor;
        delegate void SetButtonDelegate(Button Ctrl, bool Text);
       
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
        /// <summary>
        /// 跨线程设置控件Text
        /// </summary>
        /// <param name="Ctrl">待设置的控件</param>
        /// <param name="Text">Text</param>
        public static void SetEnable(Button Ctrl, bool Text)
        {
            if (Ctrl.InvokeRequired == true)
            {
                Ctrl.Invoke(new SetButtonDelegate(SetEnable), Ctrl, Text);
            }
            else
            {
                Ctrl.Enabled = Text;
            }
        }
        /// <summary>
        /// 跨线程设置控件Text
        /// </summary>
        /// <param name="Ctrl">待设置的控件</param>
        /// <param name="Text">Text</param>
        public static void SetText(Control Ctrl, string Text)
        {
            if (Ctrl.InvokeRequired == true)
            {
                Ctrl.Invoke(new SetButtonDelegate(SetEnable), Ctrl, Text);
            }
            else
            {
                Ctrl.Text = Text;
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
        public RuChuKuForm()
        {

            InitializeComponent();
            this.Resize += RuChuKuForm_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);
            
            KuCunList = new List<Maticsoft.Model.tb_KuCun>();
            LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\通讯异常", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            XinTiaoLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\心跳记录", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            WeightLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\重量记录", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
           SerialExLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\串口异常记录", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            IP = "";
            bExecuting = false;
            InitPLC();
            //InitSerialPort();
            label1.Text = "KRC公司自动行车\n\r库存管理系统";
            SerialPortSys = this.serialPort1;
            //串口重量接收
            //ReceiveWeight();//暂时不用

            //this.X = (float)base.Width;
            //this.Y = (float)base.Height;
            //this.asc.setTag(this);
            //taskDo = new Thread(new System.Threading.ThreadStart(ExecuteMission));
            //taskDo.IsBackground = true;
            //taskDo.Start();

            //taskXinTiao = new Thread(new System.Threading.ThreadStart(ExecuteXinTiao));
            //taskXinTiao.IsBackground = true;
            //taskXinTiao.Start();//开始心跳检测线程
        }

        #region 读取串口重量
        private string _weight;
        public static string weight;
        public string Weight
        {
            get { return _weight; }
            set
            {
                if (_weight != value)
                {
                    _weight = value;
                    weight = value;
                    WeightLogNet.WriteInfo($"串口通信数据校验成功，获取电子秤重量数据：{_weight}");

                }
            }
        }
        //private int _weight;
        //public int Weight
        //{
        //    get { return _weight; }
        //    set
        //    {
        //        if (_weight != value)
        //        {
        //            _weight = value;

        //            WeightLogNet.WriteInfo($"串口通信数据校验成功，获取电子秤重量数据：{_weight}");

        //        }
        //    }
        //}
        public static SerialPort SerialPortSys;
        private void InitSerialPort()
        {
            try
            {
                if (SerialPort.GetPortNames().Length>0)
                {
                    foreach (string port in SerialPort.GetPortNames())
                    {
                        serialPort1.PortName = port;
                        serialPort1.Open();
                        int n = serialPort1.BytesToRead;
                        if (serialPort1.IsOpen && n > 0)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                toolStripStatusLabelSerialState.Text = "串口正常";
                                return;
                            }));
                        }
                        else
                        {
                            //this.BeginInvoke(new Action(() =>
                            //{
                                //toolStripStatusLabelSerialState.Text = "串口关闭";
                                serialPort1.Close();
                                Thread.Sleep(10);
                            this.BeginInvoke(new Action(() =>
                            {
                                toolStripStatusLabelSerialState.Text = "串口";
                                toolStripStatusLabelSerialPort.Text = "实时重量：";
                                
                            }));
                           
                            //}));
                        }
                    }
                    
                    
                }
                else
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        toolStripStatusLabelSerialState.Text = "未能查询串行端口名称";
                        toolStripStatusLabelSerialPort.Text = "实时重量：";
                    }));
                }
                
            }
            catch(UnauthorizedAccessException e)
            {
                this.BeginInvoke(new Action(() =>
                {
                    toolStripStatusLabelSerialState.Text = "对端口的访问被拒绝";
                    toolStripStatusLabelSerialPort.Text="实时重量：";
                }));
            }
            catch(ArgumentOutOfRangeException e)
            {
                this.BeginInvoke(new Action(() =>
                {
                    toolStripStatusLabelSerialState.Text = "此实例的一个或多个属性无效";
                    toolStripStatusLabelSerialPort.Text = "实时重量：";
                }));
            }
            catch (ArgumentException e)
            {
                this.BeginInvoke(new Action(() =>
                {
                    toolStripStatusLabelSerialState.Text = "端口名称不是以“COM”开始的";
                    toolStripStatusLabelSerialPort.Text = "实时重量：";
                }));
            }
            catch (IOException e)
            {
                this.BeginInvoke(new Action(() =>
                {
                    toolStripStatusLabelSerialState.Text = "此端口处于无效状态";
                    toolStripStatusLabelSerialPort.Text = "实时重量：";
                }));
            }
            catch (InvalidOperationException e)
            {
                this.BeginInvoke(new Action(() =>
                {
                    toolStripStatusLabelSerialState.Text = "端口已经打开";
                    toolStripStatusLabelSerialPort.Text = "实时重量：";
                }));
            }
            catch (Exception e)
            {
                //this.BeginInvoke(new Action(() =>
                //{
                //    toolStripStatusLabelSerialState.Text = "串口打开异常";
                //}));
                //serialPort1.Close();
                Thread.Sleep(1000);
                SerialExLogNet.WriteException("串口打开异常",e); 

            }
            
        }
        //private async Task ReceiveWeight()
        //private void ReceiveWeight()
        //{
        // //await   Task.Run(
        //      Task.Run(
        //        () =>
        //        {
        //            List<byte> buffer = new List<byte>();
        //            while (true)
        //            {
        //                try
        //                {
        //                    #region 串口接收
        //                    if (serialPort1.IsOpen)
        //                    {
        //                        this.BeginInvoke(new Action(() =>
        //                        {
        //                            toolStripStatusLabelSerialPort.Text = "串口打开成功";
        //                        }));
        //                        //按协议格式接收数据
        //                        int n = serialPort1.BytesToRead;
        //                        byte[] buf = new byte[n];


        //                        serialPort1.Read(buf, 0, n);
        //                        buffer.AddRange(buf);//将指定集合的元素添加到集合buffer的末尾
        //                                             //int index = 0;
        //                        while (buffer.Count >= 13)//while (buffer.Count >= 13)
        //                        {


        //                            if (buffer[0] != 0xFF)
        //                            {

        //                                buffer.RemoveAt(0);//删除buffer的第一项，继续往下判断
        //                                continue;
        //                            }
        //                            //数据区尚未接收完整  小于长度15
        //                            if (buffer.Count < 15) { break; }


        //                            byte CKC = (byte)(buffer[1] ^ buffer[2] ^ buffer[3] ^ buffer[4] ^
        //                                buffer[5] ^ buffer[6] ^ buffer[7]);
        //                            if (CKC == buffer[12])//BBC校验通过
        //                            {

        //                                string strCharacter = "";
        //                                for (int i = 1; i < 8; i++)
        //                                {
        //                                    if (i == 4)
        //                                        continue;
        //                                    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
        //                                    byte[] byteArray = new byte[] { buffer[i] };
        //                                    strCharacter += asciiEncoding.GetString(byteArray);
        //                                }
        //                                string qian3 = strCharacter.Substring(0, 3);
        //                                string hou3 = strCharacter.Substring(3, 3);
        //                                if (qian3[0] == '0')
        //                                {
        //                                    if (qian3[1] == '0')
        //                                    {
        //                                        qian3 = qian3.Substring(2, 1);
        //                                    }
        //                                    else
        //                                    {
        //                                        qian3 = qian3.Substring(1, 2);
        //                                    }
        //                                }
        //                                //t转成Kg
        //                                Weight = Convert.ToInt32(qian3 + hou3);

        //                                ////将重量数据写入PLC中
        //                                //siemensTcpNet.Write("DB4.00", recData.ZongZhongLiang);
        //                                //ZhongLiangCaiJi();
        //                                buffer.RemoveRange(0, 15);
        //                            }
        //                            else
        //                            {

        //                                buffer.RemoveAt(0);//删除buffer的第一项，继续往下判断 
        //                            }


        //                        }
        //                        // _serialPort.Close();
        //                    }
        //                    else
        //                    {
        //                        //LogNet.WriteInfo("串口连接失败", "串口再连接");

        //                        //_serialPort.Close();



        //                        //_serialPort = new SerialPort();
        //                        //SerialPortInit();
                             
        //                            this.BeginInvoke(new Action(() =>
        //                            {
        //                                toolStripStatusLabelSerialPort.Text = "串口关闭";
        //                            }));
        //                        InitSerialPort();


        //                    }

        //                    #endregion

        //                }
        //                catch (Exception e)
        //                {

        //                    SerialExLogNet.WriteException("重量数据接收异常", e);
        //                    //serialPort1.Close();
        //                }


        //            }
        //        }
        //        );
        //}
      
        
        #endregion
        #region 计数
        int num;
        #endregion
        public async Task ExecuteXinTiao()
        //public void ExecuteXinTiao()
        {


            await Task.Run(() =>
            //Task.Run(() =>
            {
                //while (true)
                //{
                try
                {
                   

                    dateTimeBegin = DateTime.Now;
                    XinTiaoLogNet.WriteInfo("开始", dateTimeBegin.ToString());
                    buffReadFanKui = siemensTcpNet.Read("DB101.00", 36);//读取PLC反馈的数据
                    if (!xinTiaoFlag && buffReadFanKui.IsSuccess)
                    {
                        tb_DingDianModelFanKui = tb_DingDianBLLFanKui.GetModel("实时位置");
                        //buffReadFanKui = siemensTcpNet.Read("DB101.00", 36);//读取PLC反馈的数据
                        if (
                        (Math.Abs(int.Parse(tb_DingDianModelFanKui.X) - siemensTcpNet.ByteTransform.TransInt32(buffReadFanKui.Content, 4)) < 10)
                        && (Math.Abs(int.Parse(tb_DingDianModelFanKui.Y) - siemensTcpNet.ByteTransform.TransInt32(buffReadFanKui.Content, 8)) < 30)
                        && (Math.Abs(int.Parse(tb_DingDianModelFanKui.Z) - siemensTcpNet.ByteTransform.TransInt32(buffReadFanKui.Content, 12)) < 10)
                        )
                        {
                            xinTiaoFlag = true;
                        }
                        else
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                toolStripStatusLabelWeiZhi.Text = "请手动开回起点位置";
                            }));
                        }

                    }

                    //心跳部分  
                    if (siemensTcpNet.Write("DB102.00.0", 1).IsSuccess)
                    {
                        if (xinTiaoFlag)
                        {
                            //buffReadFanKui = siemensTcpNet.Read("DB101.00", 36);//读取PLC反馈的数据
                            tb_DingDianModelFanKui.DingDianName = "实时位置";
                            tb_DingDianModelFanKui.X = siemensTcpNet.ByteTransform.TransInt32(buffReadFanKui.Content, 4).ToString();
                            tb_DingDianModelFanKui.Y = siemensTcpNet.ByteTransform.TransInt32(buffReadFanKui.Content, 8).ToString();
                            tb_DingDianModelFanKui.Z = siemensTcpNet.ByteTransform.TransInt32(buffReadFanKui.Content, 12).ToString();
                            if (tb_DingDianBLLFanKui.Update(tb_DingDianModelFanKui))
                            {
                                XinTiaoLogNet.WriteInfo("更新实时坐标成功");
                            }
                        }


                        SetEnable(btnTongDian, true);
                        SetEnable(btnDuanDian, true);
                        SetEnable(btnFuWei, true);
                        SetEnable(btnJiTing, true);
                        SetEnable(btnErrorRest, true);
                        this.BeginInvoke(new Action(() =>
                        {
                            toolStripStatusLabel2.Text = "PLC连接成功！";
                        }));

                        //this.toolStripStatusLabel2.Text = "PLC连接成功！";
                    }
                    else
                    {
                        SetEnable(btnTongDian, false);
                        SetEnable(btnDuanDian, false);
                        SetEnable(btnFuWei, false);
                        SetEnable(btnJiTing, false);
                        SetEnable(btnErrorRest, false);
                        this.BeginInvoke(new Action(() =>
                        {
                            toolStripStatusLabel2.Text = "PLC连接失败！";
                        }));
                        //this.toolStripStatusLabel2.Text = "PLC连接失败！";
                        DisconnectPLC();
                        siemensTcpNet.ConnectServer();

                    }
                    dateTimeEnd = DateTime.Now;
                    XinTiaoLogNet.WriteInfo("结束", dateTimeEnd.ToString());
                    timeSpan = dateTimeEnd - dateTimeBegin;
                    XinTiaoLogNet.WriteInfo("时间间隔", timeSpan.Milliseconds.ToString());
                    Thread.Sleep(200);
                    if (hostComputerCommand.ReadStop(siemensTcpNet)==3)//急停状态，则闪烁
                    {
                        btnJiTing.BackColor = Color.Red;
                        btnJiTing.ForeColor = Color.Black;
                        num++;
                        if (num>1)
                        {
                            num = 0;
                            btnJiTing.ForeColor = Color.Red;
                        }
                    }
                    else if (hostComputerCommand.ReadStop(siemensTcpNet) == 0)//非急停状态
                    {
                        //btnJiTing.ForeColor = Color.Black;
                        btnJiTing.BackColor = Color.Green;
                        btnJiTing.ForeColor = Color.Black;
                    }
                    else
                    {

                        //btnJiTing.BackColor = Color.Green;
                    }
                    if (hostComputerCommand.ReadZanTing(siemensTcpNet))//暂停状态，则闪烁
                    {
                        btnZanTing.BackColor = Color.Red;
                        btnZanTing.ForeColor = Color.Black;
                        num++;
                        if (num > 1)
                        {
                            num = 0;
                            btnZanTing.ForeColor = Color.Red;
                        }
                    }
                    else //非暂停状态
                    {
                        //btnJiTing.ForeColor = Color.Black;
                        btnZanTing.BackColor = Color.Green;
                        btnZanTing.ForeColor = Color.Black;
                    }
                    //if (serialPort1.IsOpen)
                    //{
                    //    this.BeginInvoke(new Action(() =>
                    //    {
                    //        toolStripStatusLabelSerialState.Text = "串口正常";
                    //    }));
                    //}
                    //else
                    //{
                    //    this.BeginInvoke(new Action(() =>
                    //    {
                    //        toolStripStatusLabelSerialState.Text = "串口关闭";
                    //    }));
                    //}

                }
                catch (Exception e)
                {

                    XinTiaoLogNet.WriteException("ExecuteXinTiao", e);
                }
                    
                }



            //}
            );
        }
        public void ExecuteMission()
        {
            while (true)
            {
                //Task<string> t = new Task<string>(x =>
                //{
                    //if (siemensTcpNet.Write("DB102.00.0", 1).IsSuccess)
                    //{
                    //    SetEnable(btnTongDian, true);
                    //    SetEnable(btnDuanDian, true);
                    //    SetEnable(btnFuWei, true);
                    //    SetEnable(btnJiTing, true);
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    this.toolStripStatusLabel2.Text = "PLC连接成功！";
                    //}));
                    ////this.toolStripStatusLabel2.Text = "PLC连接成功！";
                    //    XinTiaoLogNet.WriteInfo("连接成功");
                    //    //return "连接成功";
                    //}
                    //else
                    //{
                    //    SetEnable(btnTongDian, false);
                    //    SetEnable(btnDuanDian, false);
                    //    SetEnable(btnFuWei, false);
                    //    SetEnable(btnJiTing, false);
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    this.toolStripStatusLabel2.Text = "PLC连接失败！";
                    //}));
                    ////this.toolStripStatusLabel2.Text = "PLC连接失败！";
                    //    DisconnectPLC();
                    //    siemensTcpNet.ConnectServer();
                    //    XinTiaoLogNet.WriteInfo("连接失败");
                    //    //return "连接失败";
                        
                    //}
                //    return x.ToString();
                //}, "hello world!");
                //t.Start();
                //XinTiaoLogNet.WriteInfo(t.Result);
                
                while (bExecuting)
                {
                    tb_DingDianModel = tb_DingDianBLL.GetModel("上货台"+shangHuoTai);
                    A.X = int.Parse(tb_DingDianModel.X);
                    A.Y = int.Parse(tb_DingDianModel.Y);
                    A.Z = int.Parse(tb_DingDianModel.Z);
                    tb_DingDianModel = tb_DingDianBLL.GetModel("起点");
                    S.X = int.Parse(tb_DingDianModel.X);
                    S.Y = int.Parse(tb_DingDianModel.Y);
                    S.Z = int.Parse(tb_DingDianModel.Z);

                    tb_KuWeiModel = tb_KuWeiBLL.GetModel(m_WeiZhi);
                    E.X = int.Parse(tb_KuWeiModel.X);
                    E.Y = int.Parse(tb_KuWeiModel.Y);
                    E.Z= int.Parse(tb_KuWeiModel.Z);

                    C.X = A.X;
                    C.Y = S.Y;
                    C.Z = A.Z;

                    D.X = E.X;
                    D.Y = S.Y;
                    D.Z = A.Z;

                    bool firstFlag = true;
                    int WriteData = 0;
                    string tipInfo = "";
                    bool flag = true;
                    bool isSuccess = false;
                    HslCommunication.OperateResult<byte[]> buffWrite;
                    HslCommunication.OperateResult<byte[]> buff2;
                    
                    if (firstFlag)
                    {
                        //读取PLC自增量指令号
                        // buff2 = siemensTcpNet.Read("DB100.00", 4);//此处应为80个字节
                        buffWrite = siemensTcpNet.Read("DB100.00", 80);//读取PLC运行的数据
                        buff2 = siemensTcpNet.Read("DB101.00", 36);//读取PLC反馈的数据
                        isSuccess = buffWrite.IsSuccess;
                        if (isSuccess)
                            counter = siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0);
                        else
                            counter = 0;
                        
                            
                        isSuccess = buff2.IsSuccess;
                        if (isSuccess)
                        {
                            ///**************************PLC反馈数据**************************/
                            //int CurrentX = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                            //int CurrentY = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 4);
                            //int CurrentZ = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 8);
                            int CurrentX = siemensTcpNet.ByteTransform.TransInt32(buff2.Content,4);
                            int CurrentY = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 8);
                            int CurrentZ = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 12);
                            //int CurrentHuoChaWeiZhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 16);
                            int CurrentHuoChaWeiZhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 20);
                            //siemensTcpNet.Write("DB4.00", recData.ZongZhongLiang);
                            tipInfo = "当前位置： " + CurrentX + " " + CurrentY + " " + CurrentZ;
                            LogNet.WriteDebug(tipInfo);
                            this.BeginInvoke(new Action(() =>
                            {
                                this.toolStripStatusLabel2.Text = tipInfo;
                            }));
                            //this.toolStripStatusLabel2.Text = tipInfo;
                            Thread.Sleep(2000);
                            if (CurrentHuoChaWeiZhi != 4)//货叉未归原位
                            {
                                WriteData = ++counter;
                                siemensTcpNet.Write("DB100.00", WriteData);
                                WriteData = 301;
                                siemensTcpNet.Write("DB100.04", WriteData);
                                WriteData = 3;
                                siemensTcpNet.Write("DB100.60", WriteData);
                                tipInfo = "货叉归原位......";
                                LogNet.WriteDebug(tipInfo);
                                this.BeginInvoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel2.Text = tipInfo;
                                }));
                                //this.toolStripStatusLabel2.Text = tipInfo;
                                //开始接收PLC反馈指令
                                
                                while (flag)
                                {
                                    //休眠2秒
                                    Thread.Sleep(2000);
                                    buff2 = siemensTcpNet.Read(" DB101.00", 24);
                                    isSuccess = buff2.IsSuccess;
                                    if (isSuccess)
                                    {
                                        int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                        int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 20);
                                        if (zhiling == 3 && huochaweizhi == 4)
                                            flag = false;
                                    }
                                }
                            }

                            if (CurrentX != S.X || CurrentY != S.Y || CurrentZ != S.Z)//航车不在起始点S位置（0，0，Z）
                            {
                                
                                //返回起始点S位置,此处暂时用来单点运动
                                tipInfo = "航车返回起始点S......";
                                LogNet.WriteDebug(tipInfo);
                                this.BeginInvoke(new Action(() =>
                                {
                                    this.toolStripStatusLabel2.Text = tipInfo;
                                }));
                                //this.toolStripStatusLabel2.Text = tipInfo;
                                WriteData = ++counter;
                                siemensTcpNet.Write("DB100.00", WriteData);
                                WriteData = 102;//修改为102指令
                                siemensTcpNet.Write("DB100.04", WriteData);

                                WriteData = CurrentX;
                                siemensTcpNet.Write("DB100.08", WriteData);

                                WriteData = S.Y;
                                siemensTcpNet.Write("DB100.12", WriteData);

                                WriteData = S.Z;
                                siemensTcpNet.Write("DB100.16", WriteData);

                                WriteData = S.X;
                                siemensTcpNet.Write("DB100.08", WriteData);
                                
                                WriteData = S.Y;
                                siemensTcpNet.Write("DB100.12", WriteData);
                                
                                WriteData = S.Z;
                                siemensTcpNet.Write("DB100.16", WriteData);
                                //接收PLC反馈
                                bool flagT = true;
                                while (flagT)
                                {
                                    //休眠2秒
                                    Thread.Sleep(2000);
                                    buff2 = siemensTcpNet.Read("DB101.00", 16);
                                    isSuccess = buff2.IsSuccess;
                                    if (isSuccess)
                                    {
                                        int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                        int X = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 4);
                                        int Y = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 8);
                                        int Z = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 12);
                                        if (zhiling == 1)
                                            if (X == S.X && Y == S.Y && Z == S.Z)
                                                flagT = false;
                                    }
                                }

                            }
                        }
                       
                        firstFlag = false;
                    }
                    else
                    {
                        //开始入库
                        if (RuChuKuForm.m_RuKuChuKuFlag)//航车走到理货台
                        {
                            //S-C-B-A点//修改为S-C-A
                            WriteData = ++counter;
                            siemensTcpNet.Write("DB100.00", WriteData);
                            WriteData = 102;
                            siemensTcpNet.Write("DB100.04", WriteData);
                            //C.X = Properties.Settings.Default.C_X;
                            
                            WriteData = C.X;
                            siemensTcpNet.Write("DB100.08", WriteData);
                            //C.Y = Properties.Settings.Default.C_Y;
                            WriteData = C.Y;
                            siemensTcpNet.Write("DB100.12", WriteData);
                            //C.Z = Properties.Settings.Default.C_Z;
                            WriteData = C.Z;
                            siemensTcpNet.Write("DB100.16", WriteData);
                            //B.X = Properties.Settings.Default.B_X;
                            //WriteData = B.X;
                            //siemensTcpNet.Write("DB100.20", WriteData);
                            ////B.Y = Properties.Settings.Default.B_Y;
                            //WriteData = B.Y;
                            //siemensTcpNet.Write("DB100.24", WriteData);
                            ////B.Z = Properties.Settings.Default.B_Z;
                            //WriteData = B.Z;
                            //siemensTcpNet.Write("DB100.28", WriteData);

                            //WriteData = int.Parse(tb_DingDianModel.X);
                            WriteData = A.X;
                            siemensTcpNet.Write("DB100.32", WriteData);
                            //WriteData = int.Parse(tb_DingDianModel.Y);
                            WriteData = A.Y;
                            siemensTcpNet.Write("DB100.36", WriteData);
                            //WriteData = int.Parse(tb_DingDianModel.Z);
                            //A.Z = Properties.Settings.Default.A_Z;
                            WriteData = A.Z;
                            siemensTcpNet.Write("DB100.40", WriteData);
                            tipInfo = "航车行驶至理货台......";
                            this.BeginInvoke(new Action(() =>
                            {
                                this.toolStripStatusLabel2.Text = tipInfo;
                            }));
                            //this.toolStripStatusLabel2.Text = tipInfo;
                            tipInfo = string.Format("航车行驶至理货台C('{0}','{1}','{2}')-B('{3}','{4}','{5}')-A('{6}','{7}','{8}')", C.X, C.Y, C.Z, B.X, B.Y, B.Z, A.X, A.Y, A.Z);
                            LogNet.WriteDebug(tipInfo);
                            //接收PLC反馈
                            bool flagT = true;
                            while (flagT)
                            {
                                //休眠2秒
                                Thread.Sleep(2000);
                                buff2 = siemensTcpNet.Read("DB101.00", 16);
                                isSuccess = buff2.IsSuccess;
                                if (isSuccess)
                                {
                                    int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                    int X = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 4);
                                    int Y = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 8);
                                    int Z = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 12);
                                    if (zhiling == 1)
                                        if (X == A.X && Y == A.Y && Z == A.Z)
                                            flagT = false;
                                }
                            }
                            //发送货叉伸出指令 
                            WriteData = ++counter;
                            siemensTcpNet.Write("DB100.00", WriteData);
                            WriteData = 301;
                            siemensTcpNet.Write("DB100.04", WriteData);
                            WriteData = 1;
                            siemensTcpNet.Write("DB100.56", WriteData);
                            WriteData = 1;
                            siemensTcpNet.Write("DB100.60", WriteData);//$$ 暂定向左
                            tipInfo = "货叉取货伸出......";
                            LogNet.WriteDebug(tipInfo);
                            this.toolStripStatusLabel2.Text = tipInfo;

                            //开始接收PLC反馈指令
                            flagT = true;
                            while (flagT)
                            {
                                //休眠2秒
                                Thread.Sleep(2000);
                                buff2 = siemensTcpNet.Read("DB101.00", 24);
                                isSuccess = buff2.IsSuccess;
                                if (isSuccess)
                                {
                                    int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                    int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 20);
                                    if (zhiling == 3 && huochaweizhi == 1)// $$ 假设货叉全部伸出为500
                                        flagT = false;
                                }
                            }
                            //货叉抬升
                            WriteData = ++counter;
                            siemensTcpNet.Write("DB100.00", WriteData);
                            WriteData = 201;
                            siemensTcpNet.Write("DB100.04", WriteData);
                            WriteData = A.X;
                            siemensTcpNet.Write("DB100.08", WriteData);
                            WriteData = A.Y;
                            siemensTcpNet.Write("DB100.12", WriteData);
                            WriteData = A.Z - 100;
                            siemensTcpNet.Write("DB100.16", WriteData);// $$
                            tipInfo = "货叉取货抬升......";
                            LogNet.WriteDebug(tipInfo);
                            this.toolStripStatusLabel2.Text = tipInfo;

                            //开始接收PLC反馈指令
                            flagT = true;
                            while (flagT)
                            {
                                //休眠2秒
                                Thread.Sleep(2000);
                                buff2 = siemensTcpNet.Read("DB101.00", 4);
                                isSuccess = buff2.IsSuccess;
                                if (isSuccess)
                                {
                                    int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                    if (zhiling == 2)
                                        flagT = false;

                                }
                                //发送货叉缩指令
                                WriteData = ++counter;
                                siemensTcpNet.Write("DB100.00", WriteData);
                                WriteData = 301;
                                siemensTcpNet.Write("DB100.04", WriteData);
                                WriteData = 3;
                                siemensTcpNet.Write("DB100.60", WriteData);
                                tipInfo = "货叉取货回缩......";
                                LogNet.WriteDebug(tipInfo);
                                this.toolStripStatusLabel2.Text = tipInfo;

                                //开始接收PLC反馈指令
                                flagT = true;
                                while (flagT)
                                {
                                    //休眠2秒
                                    Thread.Sleep(2000);
                                    buff2 = siemensTcpNet.Read("DB101.00", 24);
                                    isSuccess = buff2.IsSuccess;
                                    if (isSuccess)
                                    {
                                        int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                        int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 20);
                                        if (zhiling == 3 && huochaweizhi == 4)// $$ 假设货叉全部缩回为100
                                            flagT = false;
                                    }
                                }
                                //A-B-C-D点
                                WriteData = ++counter;
                                siemensTcpNet.Write("DB100.00", WriteData);
                                WriteData = 103;
                                siemensTcpNet.Write("DB100.04", WriteData);
                                WriteData = B.X;
                                siemensTcpNet.Write("DB100.08", WriteData);
                                WriteData = B.Y;
                                siemensTcpNet.Write("DB100.12", WriteData);
                                WriteData = B.Z;
                                siemensTcpNet.Write("DB100.16", WriteData);
                                WriteData = C.X;
                                siemensTcpNet.Write("DB100.20", WriteData);
                                WriteData = C.Y;
                                siemensTcpNet.Write("DB100.24", WriteData);
                                WriteData = C.Z;
                                siemensTcpNet.Write("DB100.28", WriteData);
                                WriteData = D.X;
                                siemensTcpNet.Write("DB100.32", WriteData);
                                WriteData = D.Y;
                                siemensTcpNet.Write("DB100.36", WriteData);
                                WriteData = D.Z;
                                siemensTcpNet.Write("DB100.40", WriteData);

                                tipInfo = "航车入库行驶A-B-C-D......";
                                LogNet.WriteDebug(tipInfo);
                                this.toolStripStatusLabel2.Text = tipInfo;

                                //接收PLC反馈
                                flagT = true;
                                while (flagT)
                                {
                                    //休眠2秒
                                    Thread.Sleep(2000);
                                    buff2 = siemensTcpNet.Read("DB101.00", 4);
                                    isSuccess = buff2.IsSuccess;
                                    if (isSuccess)
                                    {
                                        int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                        if (zhiling == 1)
                                            flagT = false;
                                    }
                                }
                                //D-E点
                                WriteData = ++counter;
                                siemensTcpNet.Write("DB100.00", WriteData);
                                WriteData = 201;
                                siemensTcpNet.Write("DB100.04", WriteData);
                                WriteData = E.X;
                                siemensTcpNet.Write("DB100.08", WriteData);
                                WriteData = E.Y;
                                siemensTcpNet.Write("DB100.12", WriteData);
                                WriteData = E.Z;
                                siemensTcpNet.Write("DB100.16", WriteData);
                                tipInfo = "航车入库行驶至货架D-E......";
                                LogNet.WriteDebug(tipInfo);
                                this.toolStripStatusLabel2.Text = tipInfo;

                                //接收PLC反馈
                                flagT = true;
                                while (flagT)
                                {
                                    //休眠2秒
                                    Thread.Sleep(2000);
                                    buff2 = siemensTcpNet.Read("DB101.00", 16);
                                    isSuccess = buff2.IsSuccess;
                                    if (isSuccess)
                                    {
                                        int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                        int X = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 4);
                                        int Y = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 8);
                                        int Z = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 12);
                                        if (zhiling == 2)
                                            if (X == E.X && Y == E.Y && Z == E.Z)
                                                flagT = false;
                                    }
                                }
                                //发送货叉伸出指令 
                                WriteData = ++counter;
                                siemensTcpNet.Write("DB100.00", WriteData);
                                WriteData = 301;
                                siemensTcpNet.Write("DB100.04", WriteData);
                                WriteData = 2;
                                siemensTcpNet.Write("DB100.56", WriteData);
                                WriteData = 1;
                                siemensTcpNet.Write("DB100.60", WriteData);//$$ 暂定向左
                                tipInfo = "货叉放货伸......";
                                LogNet.WriteDebug(tipInfo);
                                this.toolStripStatusLabel2.Text = tipInfo;

                                //开始接收PLC反馈指令
                                flagT = true;
                                while (flagT)
                                {
                                    //休眠2秒
                                    Thread.Sleep(2000);
                                    buff2 = siemensTcpNet.Read("DB101.00", 24);
                                    isSuccess = buff2.IsSuccess;
                                    if (isSuccess)
                                    {
                                        int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                        int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 20);
                                        if (zhiling == 3 && huochaweizhi == 1)// $$ 假设货叉全部伸出为500
                                            flagT = false;
                                    }
                                }
                                //货叉降 
                                WriteData = ++counter;
                                siemensTcpNet.Write("DB100.00", WriteData);
                                WriteData = 201;
                                siemensTcpNet.Write("DB100.04", WriteData);
                                WriteData = E.X;
                                siemensTcpNet.Write("DB100.08", WriteData);
                                WriteData = E.Y;
                                siemensTcpNet.Write("DB100.12", WriteData);
                                WriteData = E.Z + 100;
                                siemensTcpNet.Write("DB100.16", WriteData);// $$

                                tipInfo = "货叉放货降......";
                                LogNet.WriteDebug(tipInfo);
                                this.toolStripStatusLabel2.Text = tipInfo;

                                //开始接收PLC反馈指令
                                flagT = true;
                                while (flagT)
                                {
                                    //休眠2秒
                                    Thread.Sleep(2000);
                                    buff2 = siemensTcpNet.Read("DB101.00", 4);
                                    isSuccess = buff2.IsSuccess;
                                    if (isSuccess)
                                    {
                                        int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                        if (zhiling == 2)
                                            flagT = false;
                                    }
                                }
                                //发送货叉缩指令
                                WriteData = ++counter;
                                siemensTcpNet.Write("DB100.00", WriteData);
                                WriteData = 301;
                                siemensTcpNet.Write("DB100.04", WriteData);
                                WriteData = 3;
                                siemensTcpNet.Write("DB100.60", WriteData);

                                tipInfo = "货叉放货回缩......";
                                LogNet.WriteDebug(tipInfo);
                                this.toolStripStatusLabel2.Text = tipInfo;

                                //开始接收PLC反馈指令
                                flagT = true;
                                while (flagT)
                                {
                                    //休眠2秒
                                    Thread.Sleep(2000);
                                    buff2 = siemensTcpNet.Read("DB101.00", 24);
                                    isSuccess = buff2.IsSuccess;
                                    if (isSuccess)
                                    {
                                        int zhiling = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 0);
                                        int huochaweizhi = siemensTcpNet.ByteTransform.TransInt32(buff2.Content, 20);
                                        if (zhiling == 3 && huochaweizhi == 4)// $$ 假设货叉全部缩回为100
                                            flagT = false;
                                        //把入库数据写入数据库表
                                    }
                                }
                            }
                        }
                        else//开始出库
                        {
                            //先走到对应库位 $$

                            //发送货叉伸出指令 $$
                        }
                    }
                    //else
                    //    MessageBox.Show("读取失败!");
                    Thread.Sleep(500);
                }
            }
        }
        /// <summary>
        /// 系统的日志记录器
        /// </summary>
        private ILogNet LogNet { get; set; }
        /// <summary>
        /// 心跳的日志记录器
        /// </summary>
        private ILogNet XinTiaoLogNet { get; set; }
        /// <summary>
        /// 重量的日志记录器
        /// </summary>
        private ILogNet WeightLogNet { get; set; }
        /// <summary>
        /// 串口异常的日志记录器
        /// </summary>
        private ILogNet SerialExLogNet { get; set; }
       

        private Thread taskDo;
        private Thread taskXinTiao;
        private void InitPLC()
        {
            if (IP == "")
                IP = "192.168.0.1";
            siemensTcpNet = new SiemensS7Net_New(SiemensPLCS.S1500, IP) { ConnectTimeOut = 1000 };
            ConnectPLC();
        }
        private bool ConnectPLC()
        {
            OperateResult connect = siemensTcpNet.ConnectServer();
            if (connect.IsSuccess)
            {
                longConnection = true;
                LogNet.WriteDebug("PLC连接成功！");
                this.toolStripStatusLabel2.Text =  "PLC连接成功！";
                btnTongDian.Enabled = true;
                btnDuanDian.Enabled = true;
                btnFuWei.Enabled = true;
                btnJiTing.Enabled = true;
                btnErrorRest.Enabled = true; ;
                return true;
            }
            else
            {
                DisconnectPLC();
                btnTongDian.Enabled = false;
                btnDuanDian.Enabled = false;
                btnFuWei.Enabled = false;
                btnJiTing.Enabled = false;
                btnErrorRest.Enabled = false;
                longConnection = false;

                LogNet.WriteDebug("PLC连接失败！");
                this.toolStripStatusLabel2.Text =  "PLC连接失败！";
                //ConnectPLC();
                return false;
            }
        }

        private void DisconnectPLC()
        {
            if (siemensTcpNet != null)
                siemensTcpNet.ConnectClose();
            //MessageBox.Show(siemensTcpNet.ConnectClose().IsSuccess.ToString());
            longConnection = false;
        }
        private void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //asynchronousClient.ClientClose();
            this.Close();                 //关闭窗体
            this.Dispose();               //释放资源
            Application.Exit();           //关闭应用程序窗体
        }

        private async void btnTongDian_Click(object sender, EventArgs e)
        {
            #region 启动
            //通电
            //TongDianDuanDianFuWei = 2;
            //tb_DingDianModel = tb_DingDianBLL.GetModel("实时坐标");
            //if (true)
            //{

            //}
            int writeData = 2;
            if (siemensTcpNet.Write("DB104.00", writeData).IsSuccess)
            {

                LogNet.WriteDebug("启动");
                //this.toolStripStatusLabelState.Text = "启动";
                this.toolStripStatusLabelSwitch.Text = "启动";
                //RuChuKuForm.hostComputerCommand.WriteResetCommand(siemensTcpNet);//复位
            }
            // writeData = 101;

            //if (siemensTcpNet.Write("DB100.00", ++counter).IsSuccess)
            //{

            //    siemensTcpNet.Write("DB100.04", writeData);
            //    buffRead = RuChuKuForm.siemensTcpNet.Read("DB101.00", 36);//读取PLC反馈的数据
            //    writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 4);
            //    siemensTcpNet.Write("DB100.08", writeData);
            //    writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 8);
            //    siemensTcpNet.Write("DB100.12", writeData);
            //    writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 12);
            //    siemensTcpNet.Write("DB100.16", writeData);

            //}

            else
            {
                counter = 0;
                //Info = "启动失败";
                LogNet.WriteDebug("启动失败");
                this.toolStripStatusLabelSwitch.Text = "启动失败";
            }
            #endregion

            //#region 平板启动
           //await asynchronousClient.StartClient("启动");
            //#endregion
        }

        private async void btnDuanDian_Click(object sender, EventArgs e)
        {
            #region 停止
            //断电
            // TongDianDuanDianFuWei = 1;
            int writeData = 1;
            string Info = "停止";
            if (siemensTcpNet.Write("DB104.00", writeData).IsSuccess)
            {
                LogNet.WriteDebug(Info);
                this.toolStripStatusLabelSwitch.Text = Info;
            }
            else
            {
                Info = "停止失败";
                LogNet.WriteDebug(Info);
                this.toolStripStatusLabelSwitch.Text = Info;
            }
            #endregion

            #region 平板停止
            //await asynchronousClient.StartClient("停止");
            #endregion

        }

        private async void btnFuWei_Click(object sender, EventArgs e)
        {
            #region 复位
            //复位
            //TongDianDuanDianFuWei = 4;

            //int writeData = 1;
            //string Info = "复位";
            //if (siemensTcpNet.Write("DB104.04", writeData).IsSuccess)
            //{
            //    Thread.Sleep(1000);//延时3s
            //    writeData = 0;
            //    if (siemensTcpNet.Write("DB104.04", writeData).IsSuccess)
            //    {
            //        LogNet.WriteDebug(Info);
            //        this.toolStripStatusLabelSwitch.Text = Info;
            //        siemensTcpNet.Write("DB100.20", writeData);
            //        siemensTcpNet.Write("DB100.24", writeData);
            //        siemensTcpNet.Write("DB100.28", writeData);
            //        siemensTcpNet.Write("DB100.32", writeData);
            //        siemensTcpNet.Write("DB100.36", writeData);
            //        siemensTcpNet.Write("DB100.40", writeData);
            //        siemensTcpNet.Write("DB100.44", writeData);
            //        siemensTcpNet.Write("DB100.48", writeData);
            //        siemensTcpNet.Write("DB100.52", writeData);
            //        siemensTcpNet.Write("DB100.56", writeData);
            //        siemensTcpNet.Write("DB100.60", writeData);
            //        siemensTcpNet.Write("DB100.64", writeData);
            //        siemensTcpNet.Write("DB100.68", writeData);
            //        siemensTcpNet.Write("DB100.72", writeData);
            //        siemensTcpNet.Write("DB100.76", writeData);
            //        //if (siemensTcpNet.Write("DB104.00", writeData).IsSuccess)
            //        //{
            //        //    Info = "全部复位";
            //        //    this.toolStripStatusLabelState.Text = Info;
            //        //}
            //    }
            //    writeData = 101;
            //    counter = 1;
            //    if (siemensTcpNet.Write("DB100.00", counter).IsSuccess)
            //    {

            //        siemensTcpNet.Write("DB100.04", writeData);
            //        buffRead = RuChuKuForm.siemensTcpNet.Read("DB101.00", 36);//读取PLC反馈的数据
            //        writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 4);
            //        siemensTcpNet.Write("DB100.08", writeData);
            //        writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 8);
            //        siemensTcpNet.Write("DB100.12", writeData);
            //        writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 12);
            //        siemensTcpNet.Write("DB100.16", writeData);


            //    }
            //}
            //else
            //{
            //    Info = "复位失败";
            //    this.toolStripStatusLabelSwitch.Text = Info;
            //}
            #endregion

            #region 平板复位
            //await asynchronousClient.StartClient("复位");
            #endregion
            if(RuChuKuForm.hostComputerCommand.CommandReset(RuChuKuForm.siemensTcpNet))
            {
                //MessageBox.Show("指令复位成功");
            }
        }
      
        private void RuChuKuForm_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;
            //asynchronousClient.ClientInit();//初始化socket客户端
            //NetComplexInitialization();
            //pushClient.CreatePush(PushCallBack);                          // 创建数据订阅器
            statusLabel = toolStripStatusLabelState;
            textTasks = textTask;
            //ruchuStateTimer = new System.Threading.Timer(timerChuRuTick,
            //                        null, 0, 100);

            //初始化登录界面
            logForm = new loginForm();
            logForm.changeVisible += LogForm_changeVisible;
            logForm.TopLevel = false;
            logForm.Dock = DockStyle.Fill;
            this.Controls.Add(logForm);
            mainPanel.Controls.Add(logForm);
            logForm.Show();

            detailChuKuForm = new DetailOperationForm();//出库
            detailChuKuForm.changeTxt += DetailChuKuForm_changeTxt;
            detailChuKuForm.TopLevel = false;
            detailChuKuForm.Dock = DockStyle.Fill;
            this.Controls.Add(detailChuKuForm);
            mainPanel.Controls.Add(detailChuKuForm);

            detailRuKuForm = new DetailOperationRuKuForm();//入库
            detailRuKuForm.changeTxt += DetailRuKuForm_changeTxt;
            detailRuKuForm.TopLevel = false;
            detailRuKuForm.Dock = DockStyle.Fill;
            this.Controls.Add(detailRuKuForm);
            mainPanel.Controls.Add(detailRuKuForm);

            renGongGuanLi = new RenGongGuanLi();//位置管理
            renGongGuanLi.TopLevel = false;
            renGongGuanLi.Dock = DockStyle.Fill;
            this.Controls.Add(renGongGuanLi);
            mainPanel.Controls.Add(renGongGuanLi);

            sheBeiCanShu = new SheBeiCanShu();//设备参数管理
            sheBeiCanShu.TopLevel = false;
            sheBeiCanShu.Dock = DockStyle.Fill;
            this.Controls.Add(sheBeiCanShu);
            mainPanel.Controls.Add(sheBeiCanShu);

            //初始化设置界面
            shezhiFor = new SheZhiForm();
            shezhiFor.TopLevel = false;
            shezhiFor.Dock = DockStyle.Fill;
            this.Controls.Add(shezhiFor);
            mainPanel.Controls.Add(shezhiFor);
            //this.Resize += RuChuKuForm_Resize;
            //X = this.Width;
            //Y = this.Height;
            //setTag(this);

            //setTag(this);
            //resizeEnable = true;
            //orignalparentHeight = this.Height;
            //orignalparentWidth = this.Width;
            //初始化时间显示
            timerDingShi.Start();
            WeightReceived();
            //ExecuteXinTiao();
        }
        /// <summary>
        /// 通过串口接收到的重量数据
        /// </summary>
        private void WeightReceived()
        {
            Task.Run(

                () =>
                {
                    List<byte> buffer = new List<byte>();
                    while (true)
                    {
                        try
                        {
                            #region 串口接收
                            if (serialPort1.IsOpen)
                            {
                                //this.BeginInvoke(new Action(() =>
                                //{
                                //    toolStripStatusLabelSerialState.Text = "串口打开";

                                //}));
                                //按协议格式接收数据
                                int n = serialPort1.BytesToRead;
                                //if (n==0)//如果接收到的数据字节数为0，则换个串口
                                //{

                                //}
                                byte[] buf = new byte[n];


                                serialPort1.Read(buf, 0, n);
                                buffer.AddRange(buf);//将指定集合的元素添加到集合buffer的末尾
                                                     //int index = 0;
                                while (buffer.Count >= 3)//while (buffer.Count >= 13)
                                {

                                    //if (buffer[0] != 1 || buffer[1] != 3 || buffer[2] != 4)
                                    if (buffer[0] != 0x01 || buffer[1] != 0x03 || buffer[2] != 0x04)
                                    {

                                        //buffer.RemoveRange(0,3);//删除buffer的第一项，继续往下判断
                                        buffer.RemoveAt(0);
                                        //buffer.RemoveAt(2);
                                        continue;
                                    }
                                    //数据区尚未接收完整  小于长度15
                                    if (buffer.Count < 9) { break; }
                                    List<byte> bufferData = buffer.GetRange(0, 7);
                                    List<byte> bufferCrc = CRC16Util.CRC16(bufferData);
         
                                    if (bufferCrc[0] == buffer[8]&& bufferCrc[1] == buffer[7])//CRC16校验通过
                                    {
                                        List<byte> bufferWeight = buffer.GetRange(3, 4);
                                        //byte[] b = bufferWeight.ToArray();
                                        //foreach (byte item in bufferWeight)
                                        //{

                                        //}
                                        Weight = (bufferWeight[0] * 2304 + bufferWeight[1] * 768 + bufferWeight[2] * 256 + bufferWeight[3]).ToString();
                                       //Weight = CRC16Util.ByteToString(bufferWeight.ToArray()).TrimStart();
                                        //Weight = CRC16Util.ByteToString(bufferWeight.ToArray());
                                        this.BeginInvoke(new Action(() =>
                                        {
                                            toolStripStatusLabelSerialPort.Text = "实时重量：" + Weight + "KG";

                                        }));
                                        //t转成Kg
                                        //recData.ZongZhongLiang = Convert.ToInt32(qian3 + hou3);

                                        ////将重量数据写入PLC中
                                        //siemensTcpNet.Write("DB4.00", recData.ZongZhongLiang);
                                        //ZhongLiangCaiJi();
                                        buffer.RemoveRange(0, 8);
                                    }
                                    else
                                    {

                                        //buffer.RemoveRange(0, 8);//删除buffer的第一项，继续往下判断
                                        buffer.RemoveAt(0);
                                        //buffer.RemoveAt(2);
                                        continue;
                                    }


                                }
                                // _serialPort.Close();
                            }
                            else
                            {
                                //LogNet.WriteInfo("串口连接失败", "串口再连接");

                                //_serialPort.Close();


                                //this.BeginInvoke(new Action(() =>
                                //{
                                //    toolStripStatusLabelSerialState.Text = "串口关闭";

                                //}));
                                //_serialPort = new SerialPort();
                                InitSerialPort();
                            }

                            #endregion

                        }
                        catch (Exception e)
                        {
                            //this.BeginInvoke(new Action(() =>
                            //{
                            //    toolStripStatusLabelSerialState.Text = "串口异常";

                            //}));
                            SerialExLogNet.WriteException("重量数据接收异常", e);
                        }


                    }
                }
                );
              }

        private void RuChuKuForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
        }

        private void DetailChuKuForm_changeTxt(string txt)
        {
            this.BeginInvoke(new Action(() =>
            {
                textTask.Text = txt;
            }));
        }

        private void DetailRuKuForm_changeTxt(string txt)
        {
            this.BeginInvoke(new Action(() =>
            {
                textTask.Text = txt;
            }));
        }

        private void LogForm_changeVisible(bool visible)
        {
            btnDuanDian.Visible = visible;
            btnFuWei.Visible = visible;
            btnTongDian.Visible = visible;
            btnZanTing.Visible = visible;
            btnErrorRest.Visible = visible;
            //btnSheZhi.Visible = visible;
        }

        //private void RuChuKuForm_Resize(object sender, EventArgs e)
        //{
        //    //if (resizeEnable)
        //    //{
        //    //    if (this.WindowState == FormWindowState.Minimized)
        //    //        return;
        //    //    if (this.ClientSize.Width == 0)
        //    //        return;
        //    //    int currentParentHeight = this.Height;//this.ClientSize.Height;
        //    //    int currentParentWidth = this.Width;//this.ClientSize.Width;
        //    //    float rateX = (float)currentParentWidth / orignalparentWidth;
        //    //    float rateY = (float)currentParentHeight / orignalparentHeight;
        //    //    orignalparentHeight = currentParentHeight;
        //    //    orignalparentWidth = currentParentWidth;
        //    //    setControls(rateX, rateY, this);
        //    //    setTag(this);
        //    //}
        //}

        private void timer_Tick(object sender, EventArgs e)
        {
            //int writeData = 1;
            //if (TongDianDuanDianFuWei ==1)
            //{
            //    writeData = 1;
            //    siemensTcpNet.Write("DB100.64", writeData);
            //}
            //else if(TongDianDuanDianFuWei ==2)
            //{
            //    writeData = 2;
            //    siemensTcpNet.Write("DB100.64", writeData);
            //}
            //else if(TongDianDuanDianFuWei==4)
            //{
            //    writeData = 4;
            //    siemensTcpNet.Write("DB100.64", writeData);
            //}
        }

        private async void timerDingShi_Tick(object sender, EventArgs e)
        {
            //string x = "";
            toolStripStatusLabelTime.Text = DateTime.Now.ToString();
            await ExecuteXinTiao();
            //if (siemensTcpNet.Write("DB102.00.0", 1).IsSuccess)
            //{
            //    SetEnable(btnTongDian, true);
            //    SetEnable(btnDuanDian, true);
            //    SetEnable(btnFuWei, true);
            //    SetEnable(btnJiTing, true);

            //    this.toolStripStatusLabel2.Text = "PLC连接成功！";
            //}
            //else
            //{
            //    SetEnable(btnTongDian, false);
            //    SetEnable(btnDuanDian, false);
            //    SetEnable(btnFuWei, false);
            //    SetEnable(btnJiTing, false);

            //    this.toolStripStatusLabel2.Text = "PLC连接失败！";
            //    DisconnectPLC();
            //    siemensTcpNet.ConnectServer();

            //}

        }

        private void timerChuRuTick(object sender)
        {
            //this.BeginInvoke(new Action(() =>
            //{
            //    this.toolStripStatusLabelTime.Text = DateTime.Now.ToString();
            //    XinTiaoLogNet.WriteInfo("时间");
            //}));
            this.toolStripStatusLabelTime.Text = DateTime.Now.ToString();
            //if (siemensTcpNet.Write("DB102.00.0", 1).IsSuccess)
            //{
            //    XinTiaoLogNet.WriteInfo("通信正常");
            //    SetEnable(btnTongDian, true);
            //    SetEnable(btnDuanDian, true);
            //    SetEnable(btnFuWei, true);
            //    SetEnable(btnJiTing, true);
            //    //this.Invoke(new Action(() => {
            //    //    statusStrip1.Items["toolStripStatusLabel2"].Text = "PLC连接成功！";
            //    //}));
            //    this.BeginInvoke(new Action(() =>
            //    {
            //        toolStripStatusLabel2.Text = "PLC连接成功！";
            //    }));
            //    //this.toolStripStatusLabel2.Text = "PLC连接成功！";
            //}
            //else
            //{
            //    XinTiaoLogNet.WriteInfo("通信失败");
            //    SetEnable(btnTongDian, false);
            //    SetEnable(btnDuanDian, false);
            //    SetEnable(btnFuWei, false);
            //    SetEnable(btnJiTing, false);
            //    //this.Invoke(new Action(() => {
            //    //    statusStrip1.Items["toolStripStatusLabel2"].Text = "PLC连接失败！";
            //    //}));
            //    this.BeginInvoke(new Action(() =>
            //    {
            //        toolStripStatusLabel2.Text = "PLC连接失败！";
            //    }));
            //    //this.toolStripStatusLabel2.Text = "PLC连接失败！";
            //    DisconnectPLC();
            //    siemensTcpNet.ConnectServer();

            //}


        }
        private  int Sum(object i)
        {
            var sum = 0;
            if (siemensTcpNet.Write("DB102.00.0", 1).IsSuccess)
            {
                XinTiaoLogNet.WriteInfo("通信正常");
                SetEnable(btnTongDian, true);
                SetEnable(btnDuanDian, true);
                SetEnable(btnFuWei, true);
                SetEnable(btnJiTing, true);
                SetEnable(btnErrorRest, true);
                //this.Invoke(new Action(() => {
                //    statusStrip1.Items["toolStripStatusLabel2"].Text = "PLC连接成功！";
                //}));
                this.BeginInvoke(new Action(() =>
                {
                    toolStripStatusLabel2.Text = "PLC连接成功！";
                }));
                //this.toolStripStatusLabel2.Text = "PLC连接成功！";
            }
            else
            {
                XinTiaoLogNet.WriteInfo("通信失败");
                SetEnable(btnTongDian, false);
                SetEnable(btnDuanDian, false);
                SetEnable(btnFuWei, false);
                SetEnable(btnJiTing, false);
                SetEnable(btnErrorRest, false);
                //this.Invoke(new Action(() => {
                //    statusStrip1.Items["toolStripStatusLabel2"].Text = "PLC连接失败！";
                //}));
                this.BeginInvoke(new Action(() =>
                {
                    toolStripStatusLabel2.Text = "PLC连接失败！";
                }));
                //this.toolStripStatusLabel2.Text = "PLC连接失败！";
                DisconnectPLC();
                siemensTcpNet.ConnectServer();

            }

            return sum;
        }

        private void btnSheZhi_Click(object sender, EventArgs e)
        {
            shezhiFor.Show();
        }

        private async void btnJiTing_Click(object sender, EventArgs e)
        {
            #region 急停
            int writeData = 3;
            string Info = "急停";


            if (siemensTcpNet.Write("DB104.00", writeData).IsSuccess && jiTingFlag)
            {
                LogNet.WriteDebug(Info);
                this.toolStripStatusLabelSwitch.Text = Info;
                //btnJiTing.BackColor = Color.Green;
                jiTingFlag = false;

            }
            else if (!jiTingFlag)
            {
                Info = "急停复位";
                writeData = 0;
                if (siemensTcpNet.Write("DB104.00", writeData).IsSuccess)
                {

                    this.toolStripStatusLabelSwitch.Text = Info;
                    //btnJiTing.BackColor = Color.Red;
                    jiTingFlag = true;

                }
            }
            else
            {
                Info = "急停失败";
                LogNet.WriteDebug(Info);
                this.toolStripStatusLabelSwitch.Text = Info;

            }
            #endregion

            #region 平板发送急停
            //await asynchronousClient.StartClient("急停");
            #endregion

        }

        private void btnState_Click(object sender, EventArgs e)
        {

        }
        
        private void btnRun_Click(object sender, EventArgs e)
        {
            bool writeData = true;
            string Info = "运行成功";
            if (siemensTcpNet.Write("DB100.64.0", writeData).IsSuccess)
            {
                Thread.Sleep(1000);//延时3s
                writeData = false;
                if (siemensTcpNet.Write("DB100.64.0", writeData).IsSuccess)
                {
                    LogNet.WriteDebug(Info);
                }
                this.toolStripStatusLabelSwitch.Text = Info;
            }
            else
            {
                this.toolStripStatusLabelSwitch.Text = "运行失败";
            }
        }

        private void btnZanTing_Click(object sender, EventArgs e)
        {
            int writeData = 0;
            string Info = "暂停";


            if (siemensTcpNet.Write("DB104.08", writeData).IsSuccess && zanTingFlag)
            {
                LogNet.WriteDebug(Info);
                this.toolStripStatusLabelSwitch.Text = Info;
                btnZanTing.BackColor = Color.Red;
                zanTingFlag = false;

            }
            else if (!zanTingFlag)
            {
                Info = "暂停后启动";
                writeData = 1;
                if (siemensTcpNet.Write("DB104.08", writeData).IsSuccess)
                {

                    this.toolStripStatusLabelSwitch.Text = Info;
                    btnZanTing.BackColor = Color.Green;
                    zanTingFlag = true;

                }
            }
            else
            {
                Info = "暂停失败";
                LogNet.WriteDebug(Info);
                this.toolStripStatusLabelSwitch.Text = Info;

            }
        }


        #region Complex Client

        //===========================================================================================================
        // 网络通讯的客户端块，负责接收来自服务器端推送的数据

        private NetComplexClient complexClient;
        private bool isClientIni = false;                       // 客户端是否进行初始化过数据

        private void NetComplexInitialization()
        {
            complexClient = new NetComplexClient();
            complexClient.EndPointServer = new System.Net.IPEndPoint(
                System.Net.IPAddress.Parse("127.0.0.1"), 23456);
            complexClient.AcceptByte += ComplexClient_AcceptByte;
            complexClient.AcceptString += ComplexClient_AcceptString;
            complexClient.ClientStart();
        }

        private void ComplexClient_AcceptString(AppSession session, HslCommunication.NetHandle handle, string data)
        {
            // 接收到服务器发送过来的字符串数据时触发
        }

        private void ComplexClient_AcceptByte(AppSession session, HslCommunication.NetHandle handle, byte[] buffer)
        {
            // 接收到服务器发送过来的字节数据时触发
            if (handle == 1)
            {
                // 该buffer是读取到的西门子数据
                //if (isClientIni)
                //{
                //    ShowReadContent( buffer );
                //}
            }
            else if (handle == 2)
            {
                // 初始化的数据
                //ShowHistory(buffer);

                isClientIni = true;
            }
        }


        #endregion


        #region Push NetClient

        private NetPushClient pushClient = new NetPushClient("127.0.0.1", 23467, "A");                          // 数据订阅器，负责订阅主要的数据

        private void PushCallBack(NetPushClient pushClient, string value)
        {
            JObject content = JObject.Parse(value);

            //if (isClientIni)
            //{
            //    ShowReadContent(content);
            //}
        }

        #endregion

        #region 异步客户端套接字
        public static AsynchronousClient asynchronousClient = new AsynchronousClient();
        #endregion
        #region 同步客户端套接字
        public static SynchronousSocketClient synchronousClient = new SynchronousSocketClient();
        #endregion

        #region plc命令
        public static HostComputerCommand hostComputerCommand = new HostComputerCommand();
        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x0400011C RID: 284
        private float X;

        // Token: 0x0400011D RID: 285
        private float Y;

        private void label1_Click(object sender, EventArgs e)
        {
            //如果是单击的是左键
            //if (Mouse.LeftButton == MouseButtonState.Pressed)
            //{
            if (isLogin)
            {
                contextMenuStrip1.Show(); //在你单击的地方弹出菜单
            }
                
            //}

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            renGongGuanLi.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //this.Hide();
            sheBeiCanShu.Show();
        }

        private void 退出登录ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TuiChuDengLu();
            //changeVisible(false);
        }
        #region
        Maticsoft.BLL.tb_UserLoginRecords LoginRecordsBLL = new Maticsoft.BLL.tb_UserLoginRecords();
        Maticsoft.Model.tb_UserLoginRecords LoginRecordsModel = new Maticsoft.Model.tb_UserLoginRecords();
        #endregion
        private void TuiChuDengLu()
        {
            //显示登录界面
            detailRuKuForm.Hide();
            detailChuKuForm.Hide();
            renGongGuanLi.Hide();//人工管理界面
            sheBeiCanShu.Hide();
            logForm.Show(false);
            logForm.Show();
            //写入数据库退出登录动作
            //LoginRecordsModel.UserID = textBoxUserID.Text.Trim();
            //LoginRecordsModel.Action = "退出登录";
            //LoginRecordsModel.WorkType = RuChuKuForm.LoginType;
            //LoginRecordsModel.DateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //LoginRecordsBLL.Add(LoginRecordsModel);
            //写入用户登录记录数据库 
            //string strSQL = string.Format(@"insert into tb_UserLoginRecords values('{0}','{1}','{2}','{3}')", textBoxUserID.Text.Trim(), "退出登录", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),mainForm.LoginType);
            //SqlCommand thisCommand = new SqlCommand(strSQL, mainForm.myconnection);
            //thisCommand.ExecuteNonQuery();

            //this.textBoxUserID.Text = "";
            //this.textBoxPassword.Text = "";

            //RuChuKuForm.loginname = "";
            //RuChuKuForm.isLogin = false;
            //RuChuKuForm.LoginType = "";

            //this.labelYongHuDengLu.Visible = true;
            //this.labelYongHuMing1.Visible = true;
            //this.labelMiMa.Visible = true;
            //this.textBoxUserID.Visible = true;
            //this.textBoxPassword.Visible = true;
            //this.btn_Login.Visible = true;
            //this.btn_Reset.Visible = true;

            //this.labelDengLuChengGong.Visible = false;
            //this.labelYongHuMing2.Visible = false;
            //this.labelDangQianYongHu.Visible = false;
            //this.btnTuiChuDengLu.Visible = false;
            //this.btnManageUser.Visible = false;
            //this.btnChuKu.Visible = false;
            //this.btnRuKu.Visible = false;

            //btnExchange.Visible = false;
            //btnSheBeiCanShu.Visible = false;
            //btnWeiZhi.Visible = false;
        }

        private void RuChuKuForm_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
           
                //if (e.Button.ToString() == "Left")
                //{
                //    this.contextMenuStrip1.Show(this.Left + e.X, this.Top + e.Y);
                //}
            
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
        }

        private void 串口配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SerialSet serialSet = new SerialSet();
            serialSet.Show();
        }

        private void 测试toolStripMenuItem3_Click(object sender, EventArgs e)
        {

        }

        private void 用户管理toolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageAccountForm  manageAccForm = new ManageAccountForm();
            manageAccForm.ShowDialog();
        }
        //故障复位
        private void btnErrorRest_Click(object sender, EventArgs e)
        {
            RuChuKuForm.hostComputerCommand.ErrorReset(RuChuKuForm.siemensTcpNet);
        }

        #endregion

        #region 出入库记录
        public static   Maticsoft.Model.LocationRecord locationRecordModel = new Maticsoft.Model.LocationRecord();
      public static  Maticsoft.BLL.LocationRecord locationRecordBLL = new Maticsoft.BLL.LocationRecord();
        #endregion
    }
}
