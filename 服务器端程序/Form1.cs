﻿using ClassLibrary_Crane;
using HslCommunication;
using HslCommunication.Core.Net;
using HslCommunication.Enthernet;
using HslCommunication.LogNet;
using HslCommunication.Profinet.Siemens;
using Newtonsoft.Json.Linq;
using SocketServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using 自动化库存管理.Command;

namespace 服务器端程序
{
    public partial class Form1 : Form
    {
        #region Constructor
        public SiemensS7Net_New siemensTcpNetNew;//创建PLC连接
        public string IP;
        int counter=0;
        public Form1()
        {
            InitializeComponent();
            hybirdLock = new HslCommunication.Core.SimpleHybirdLock();      // 锁的实例化
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            InsLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\接收指令测试", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            LogNet.BeforeSaveToFile += LogNet_BeforeSaveToFile;              // 设置存储日志前的一些额外操作
            //NetComplexInitialization();                                     // 初始化网络服务
            //NetSimplifyInitialization();                                    // 初始化同步网络服务
            //NetPushServerInitialization();
            //TimerInitialization();                                          // 定时器初始化
            //SiemensTcpNetInitialization();                                  // PLC读取初始化
            InitPLC();
            socketListener.StartListening();
            
        }
        #endregion
        #region 在线网络服务端支持


        /*****************************************************************************************************
         * 
         *    特别说明：在线网络的模块的代码主要是为了支持服务器对客户端在线的情况进行管理
         *    
         *             当客户端刚上线的时候，服务器也可以发送一些初始数据给客户端
         * 
         *****************************************************************************************************/


        //高性能的异步网络服务器类，适合搭建局域网聊天程序，消息推送程序
        private NetComplexServer netComplex;                            // 在线网络管理核心

        private void NetComplexInitialization()
        {
            netComplex = new NetComplexServer();                        // 实例化
            netComplex.AcceptString += NetComplex_AcceptString;          // 绑定字符串接收事件
            netComplex.ClientOnline += NetComplex_ClientOnline;          // 客户端上线的时候触发
            netComplex.ClientOffline += NetComplex_ClientOffline;        // 客户端下线的时候触发
            netComplex.LogNet = LogNet;                                  // 设置日志
            netComplex.ServerStart(23456);                             // 启动网络服务

        }

        private void NetComplex_ClientOffline(AppSession session, string object2)
        {
            // 客户端下线的时候触发方法
            RemoveOnLine(session.ClientUniqueID);
        }

        private void NetComplex_ClientOnline(AppSession session)
        {
            // 回发一条初始化数据的信息
            netComplex.Send(session, 2, GetHistory());
            // 有客户端上限时触发方法
            NetAccount account = new NetAccount()
            {
                Guid = session.ClientUniqueID,
                Ip = session.IpAddress,
                Name = session.LoginAlias,
                OnlineTime = DateTime.Now,
            };

            AddOnLine(account);
        }




        private void NetComplex_AcceptString(AppSession stateone, HslCommunication.NetHandle handle, string data)
        {
            // 接收到客户端发来的数据时触发
            switch (data)
            {
                case "启动":
                    hostComputerCommand.WriteRunCommand(siemensTcpNetNew);
                    break;
                case "停止":
                    hostComputerCommand.WriteStopCommand(siemensTcpNetNew); ;
                    break;
                case "夹具":
                    MessageBox.Show("收到夹具命令") ;
                    break;
                default:
                    MessageBox.Show("收到命令");
                    break;
            }
        }


        #endregion

        #region 同步网络服务器支持

        /*****************************************************************************************************
         * 
         *    特别说明：同步网络模块，用来支持远程的写入操作，特点是支持是否成功的反馈，这个信号对客户端来说是至关重要的
         *    
         *             不仅仅支持客户端的操作，还支持web端的操作。
         * 
         *****************************************************************************************************/

        private NetSimplifyServer netSimplify;                                     // 同步网络访问的服务支持

        private void NetSimplifyInitialization()
        {
            netSimplify = new NetSimplifyServer();                                // 实例化
            netSimplify.ReceiveStringEvent += NetSimplify_ReceiveStringEvent;      // 服务器接收字符串信息的时候，触发
            netSimplify.LogNet = LogNet;                                           // 设置日志
            netSimplify.ServerStart(23457);                                      // 启动服务
        }

        private void NetSimplify_ReceiveStringEvent(AppSession session, HslCommunication.NetHandle handle, string msg)
        {
            #region 之前
            //if (handle == 0)
            //{
            //    hostComputerCommand.WriteOpenCommand(siemensTcpNetNew);//启动PLC
            //}
            //else if (handle == 1)
            //{
            //    string tmp = StartPLC();
            //    LogNet?.WriteInfo(tmp);
            //    // 远程启动设备
            //    netSimplify.SendMessage(session, handle, tmp);
            //}
            //else if (handle == 2)
            //{
            //    string tmp = StopPLC();
            //    LogNet?.WriteInfo(tmp);
            //    // 远程停止设备
            //    netSimplify.SendMessage(session, handle, tmp);
            //}
            //else if (handle == 301)
            //{
            //    MessageBox.Show("夹具");
            //}
            //else
            //{
            //    netSimplify.SendMessage(session, handle, msg);
            //}
            #endregion
            #region 收到客户端发送的指令代码
           
            switch (handle)
            {
                case 301:
                    //MessageBox.Show("开始");
                    InsLogNet.WriteInfo("301");
                    //netSimplify.SendMessage(session, handle, "301");
                    //hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNetNew, ++counter, 301, 1);
                    //MessageBox.Show("结束");
                    break;
                case 302:
                    InsLogNet.WriteInfo("302");
                    hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNetNew, ++counter, 301, 2);
                    break;
                case 303:
                    InsLogNet.WriteInfo("303");
                    hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNetNew, ++counter, 301, 3);
                    break;
                default:
                    InsLogNet.WriteInfo("无指令");
                    break;
            }

            #endregion
        }

        #endregion

        #region 在线客户端实现块

        private List<NetAccount> all_accounts = new List<NetAccount>();
        private object obj_lock = new object();

        // 新增一个用户账户到在线客户端
        private void AddOnLine(NetAccount item)
        {
            lock (obj_lock)
            {
                all_accounts.Add(item);
            }
            UpdateOnlineClients();
        }

        // 移除在线账户并返回相应的在线信息
        private void RemoveOnLine(string guid)
        {
            lock (obj_lock)
            {
                for (int i = 0; i < all_accounts.Count; i++)
                {
                    if (all_accounts[i].Guid == guid)
                    {
                        all_accounts.RemoveAt(i);
                        break;
                    }
                }
            }
            UpdateOnlineClients();
        }

        /// <summary>
        /// 更新客户端在线信息
        /// </summary>
        private void UpdateOnlineClients()
        {
            if (InvokeRequired && IsHandleCreated)
            {
                Invoke(new Action(UpdateOnlineClients));
                return;
            }

            lock (obj_lock)
            {
                //listBox1.DataSource = all_accounts.ToArray();
            }
        }

        #endregion

        #region 数据的订阅发布实现

        /****************************************************************************************************************
         * 
         *    本模块主要负责进行数据的发布。只要客户端订阅了相关的数据，服务器端进行推送后，客户端就可以收到数据
         *    
         *    因为本订阅器目前只支持字符串的数据订阅，所以在这里需要将byts[]转化成base64编码的数据，相关的知识请自行百度，此处不再说明
         * 
         *****************************************************************************************************************/

        private NetPushServer pushServer = null;                 // 订阅发布核心服务器

        private void NetPushServerInitialization()
        {
            pushServer = new NetPushServer();
            pushServer.LogNet = LogNet;
            pushServer.ServerStart(23467);
        }


        #endregion

        #region PLC 数据读取块

        /***************************************************************************************************************
         * 
         *    以下演示了西门子的读取类，对于三菱和欧姆龙，或是modbustcp来说，逻辑都是一样的，你也可以尝试着换成三菱的类，来加深理解
         * 
         *****************************************************************************************************************/


        private SiemensS7Net siemensTcpNet;                                                // 西门子的网络访问器
        private bool isReadingPlc = false;                                                 // 是否启动的标志，可以用来暂停项目
        private int failed = 0;                                                            // 连续失败此处，连续三次失败就报警
        private Thread threadReadPlc = null;                                               // 后台读取PLC的线程

        private void SiemensTcpNetInitialization()
        {
            siemensTcpNet = new SiemensS7Net(SiemensPLCS.S1500);                          // 实例化西门子的对象
            siemensTcpNet.IpAddress = "192.168.0.1";                                       // 设置IP地址
            siemensTcpNet.LogNet = LogNet;                                                  // 设置统一的日志记录器
            siemensTcpNet.ConnectTimeOut = 1000;                                            // 超时时间为1秒

            // 启动后台读取的线程
            threadReadPlc = new Thread(new System.Threading.ThreadStart(ThreadBackgroundReadPlc));
            threadReadPlc.IsBackground = true;
            threadReadPlc.Priority = ThreadPriority.AboveNormal;
            threadReadPlc.Start();
        }

        private Random random = new Random();
        private bool isReadRandom = false;

        private void ThreadBackgroundReadPlc()
        {

            // 此处假设我们读取的是西门子PLC的数据，其实三菱的数据读取原理是一样的，可以仿照西门子的开发

            /**************************************************************************************************
             * 
             *    假设一：M100，M101存储了一个温度值，举例，100.5℃数据为1005
             *    假设二：M102存储了设备启停信号，0为停止，1为启动
             *    假设三：M103-M106存储了一个产量值，举例：12345678
             * 
             **************************************************************************************************/

            double temperature = 100f;

            while (true)
            {
                if (isReadingPlc)
                {

                    // 这里仅仅演示了西门子的数据读取
                    // 事实上你也可以改成三菱的，无非解析数据的方式不一致而已，其他数据推送代码都是一样的



                    HslCommunication.OperateResult<JObject> read = null; //siemensTcpNet.Read( "M100", 7 );

                    if (isReadRandom)
                    {
                        temperature = Math.Round(temperature + random.Next(100) / 10f - 5f, 1);
                        if (temperature < 0 || temperature > 200) temperature = 100;

                        // 当没有测试的设备的时候，此处就演示读取随机数的情况
                        read = HslCommunication.OperateResult.CreateSuccessResult(new JObject()
                        {
                            {"temp",new JValue(temperature) },
                            {"enable",new JValue(random.Next(100)>10) },
                            {"product",new JValue(random.Next(10000)) }
                        });
                    }
                    else
                    {
                        HslCommunication.OperateResult<byte[]> tmp = siemensTcpNet.Read("M100", 7);
                        if (tmp.IsSuccess)
                        {
                            double temp1 = siemensTcpNet.ByteTransform.TransInt16(tmp.Content, 0) / 10.0;
                            bool machineEnable = tmp.Content[2] != 0x00;
                            int product = siemensTcpNet.ByteTransform.TransInt32(tmp.Content, 3);

                            read = HslCommunication.OperateResult.CreateSuccessResult(new JObject()
                            {
                                {"temp",new JValue(temp1) },
                                {"enable",new JValue(machineEnable) },
                                {"product",new JValue(product) }
                            });
                        }
                        else
                        {
                            read = HslCommunication.OperateResult.CreateFailedResult<JObject>(tmp);
                        }
                    }


                    if (read.IsSuccess)
                    {
                        failed = 0;                                                              // 读取失败次数清空
                        pushServer.PushString("A", read.Content.ToString());    // 推送数据，关键字为A
                        ShowReadContent(read.Content);                                         // 在主界面进行显示，此处仅仅是测试，实际项目中不建议在服务端显示数据信息


                    }
                    else
                    {
                        failed++;
                        ShowFailedMessage(failed);                             // 显示出来读取失败的情况
                    }
                }

                Thread.Sleep(500);                            // 两次读取的时间间隔
            }
        }

        // 只是用来显示连接失败的错误信息
        private void ShowFailedMessage(int failed)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<int>(ShowFailedMessage), failed);
                return;
            }

            textBox1.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff ") + "第" + failed + "次读取失败！" + Environment.NewLine);
        }

        // 读取成功时，显示结果数据
        private void ShowReadContent(JObject content)
        {
            // 本方法是考虑了后台线程调用的情况
            if (InvokeRequired)
            {
                // 如果是后台调用显示UI，那么就使用委托来切换到前台显示
                Invoke(new Action<JObject>(ShowReadContent), content);
                return;
            }

            // 提取数据
            double temp1 = content["temp"].ToObject<double>();
            bool machineEnable = content["enable"].ToObject<bool>();
            int product = content["product"].ToObject<int>();


            // 实际项目的时候应该在此处进行将数据存储到数据库，你可以选择MySql,SQL SERVER,ORACLE等等
            // SaveDataSqlServer( temp1 );         // 此处演示写入了SQL 数据库的方式

            // 开始显示
            //label2.Text = temp1.ToString();
            //label2.BackColor = temp1 > 100d ? Color.Tomato : Color.Transparent;  // 如果温度超100℃就把背景改为红色
            //label3.Text = product.ToString();

            // 添加到缓存数据
            AddDataHistory((float)temp1);

            //label5.Text = machineEnable ? "运行中" : "未启动";
        }


        #endregion

        #region PLC 启动逻辑块

        private string StartPLC()
        {
            if (isReadRandom) return "启动成功";   // 测试模式专用

            HslCommunication.OperateResult write = siemensTcpNet.Write("M102", (byte)1);
            return write.IsSuccess ? "成功启动" : "启动失败:" + write.Message;
        }

        private string StopPLC()
        {
            if (isReadRandom) return "停止成功"; // 测试模式专用

            HslCommunication.OperateResult write = siemensTcpNet.Write("M102", (byte)0);
            return write.IsSuccess ? "成功停止" : "停止失败:" + write.Message;
        }

        #endregion

        #region 定时器块


        /*********************************************************************************************
         * 
         *    功能说明：
         *    定时器块实现的功能是当连续3次读取PLC数据失败时，就将窗口进行闪烁。
         *    重新连接上时，就显示信号成功。
         * 
         *********************************************************************************************/


        private System.Windows.Forms.Timer timer = null;
        private bool m_isRedBackColor = false;

        private void TimerInitialization()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 每秒执行的代码
            if (failed > 3)
            {
                // 交替闪烁界面
                m_isRedBackColor = !m_isRedBackColor;
                if (m_isRedBackColor)
                {
                    BackColor = Color.Tomato;
                }
                else
                {
                    BackColor = SystemColors.Control;
                }
            }
            else
            {
                // 复原颜色
                BackColor = SystemColors.Control;
                m_isRedBackColor = false;
            }

            UpdateOnlineClients();
        }



        #endregion

        #region 日志块

        /// <summary>
        /// 系统的日志记录器
        /// </summary>
        private ILogNet LogNet { get; set; }
        /// <summary>
        /// 指令测试的日志记录器
        /// </summary>
        private ILogNet InsLogNet { get; set; }
        private void LogNet_BeforeSaveToFile(object sender, HslEventArgs e)
        {
            if (textBox1.InvokeRequired)
            {
                textBox1.Invoke(new Action<object, HslEventArgs>(LogNet_BeforeSaveToFile), sender, e);
                return;
            }

            textBox1.AppendText(GetStringFromLogMessage(e) + Environment.NewLine);
            e.HslMessage.Cancel = true; // 取消保存
        }

        private string GetStringFromLogMessage(HslEventArgs e)
        {
            return $"[{e.HslMessage.Degree}] {e.HslMessage.Time.ToString("yyyy-MM-dd HH:mm:ss.fff")} Thread[{e.HslMessage.ThreadId.ToString("D2")}] {e.HslMessage.Text}";
        }

        #endregion

        #region 温度数据缓存

        private float[] arrayTemp = new float[0];                     // 缓存1000个长度的数据
        private HslCommunication.Core.SimpleHybirdLock hybirdLock;    // 数据操作的锁

        private void AddDataHistory(float value)
        {
            hybirdLock.Enter();
            HslCommunication.BasicFramework.SoftBasic.AddArrayData(ref arrayTemp, new float[] { value }, 1000);
            hybirdLock.Leave();
        }

        /// <summary>
        /// 获取所有的历史数据的序列化数据
        /// </summary>
        /// <returns></returns>
        private byte[] GetHistory()
        {
            byte[] buffer = null;

            hybirdLock.Enter();

            buffer = new byte[arrayTemp.Length * 4];
            for (int i = 0; i < arrayTemp.Length; i++)
            {
                BitConverter.GetBytes(arrayTemp[i]).CopyTo(buffer, 4 * i);
            }

            hybirdLock.Leave();

            return buffer;
        }

        #endregion

        #region 温度数据存储数据库


        // 此处示例，将温度数据存储到SQL SERVER的数据库里
        // 数据库名字为 myDatabase
        // 数据库中有一张表，Data.Table 
        // 该表有三列，第一列自增标识序列，第二列为温度，第三列时间

        private void SaveDataSqlServer(double tmp)
        {
            try
            {
                string conStr = "server = 127.0.0.1; database = myDatabase; uid = sa; pwd = 123456";   // 取决于你实际安装的数据库
                using (SqlConnection conn = new SqlConnection(conStr))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO DBO.Data VALUES('" + tmp + "',GETDATE())", conn))
                    {
                        cmd.ExecuteNonQuery();         // 执行数据库语句
                    }
                }
                // 成功写入数据库
            }
            catch (Exception ex)
            {
                LogNet.WriteException("数据库写入失败", ex);         // 写入日志
            }
        }



        #endregion
        #region plc配置 
        private void InitPLC()
        {
            if (IP == "")
                IP = "192.168.0.1";
            siemensTcpNetNew = new SiemensS7Net_New(SiemensPLCS.S1500, IP) { ConnectTimeOut = 1000 };
            ConnectPLC();
        }
        private bool ConnectPLC()
        {
            OperateResult connect = siemensTcpNetNew.ConnectServer();
            if (connect.IsSuccess)
            {
                //longConnection = true;
                //LogNet.WriteDebug("PLC连接成功！");
                //this.toolStripStatusLabel2.Text = "PLC连接成功！";
                //btnTongDian.Enabled = true;
                //btnDuanDian.Enabled = true;
                //btnFuWei.Enabled = true;
                //btnJiTing.Enabled = true;
                return true;
            }
            else
            {
                DisconnectPLC();
                //btnTongDian.Enabled = false;
                //btnDuanDian.Enabled = false;
                //btnFuWei.Enabled = false;
                //btnJiTing.Enabled = false;
                //longConnection = false;

                //LogNet.WriteDebug("PLC连接失败！");
                //this.toolStripStatusLabel2.Text = "PLC连接失败！";
                //ConnectPLC();
                return false;
            }
        }

        private void DisconnectPLC()
        {
            if (siemensTcpNetNew != null)
                siemensTcpNetNew.ConnectClose();
            //MessageBox.Show(siemensTcpNet.ConnectClose().IsSuccess.ToString());
            //longConnection = false;
        }

        #endregion

        private void userButton1_Click(object sender, EventArgs e)
        {

        }

        private void userButton2_Click(object sender, EventArgs e)
        {
            // 启动运行，修改M102为1
            MessageBox.Show(StartPLC());
        }

        private void userButton3_Click(object sender, EventArgs e)
        {
            // 停止运行，修改M102为0
            MessageBox.Show(StopPLC());
        }

        #region 异步服务器套接字
        private AsynchronousSocketListener socketListener = new AsynchronousSocketListener();
        #endregion

        #region plc命令
        public static HostComputerCommand hostComputerCommand = new HostComputerCommand();
        #endregion
    }


    /// <summary>
    /// 用于在线控制的网络类
    /// </summary>
    public class NetAccount
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public string Guid { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime OnlineTime { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }



        private string GetOnlineTime()
        {
            TimeSpan ts = DateTime.Now - OnlineTime;
            if (ts.TotalSeconds < 60)
            {
                return ts.Seconds + " 秒";
            }
            else if (ts.TotalHours < 1)
            {
                return ts.Minutes + "分" + ts.Seconds + "秒";
            }
            else if (ts.TotalDays < 1)
            {
                return ts.Hours + "时" + ts.Minutes + "分";
            }
            else
            {
                return ts.Days + "天" + ts.Hours + "时";
            }
        }

        /// <summary>
        /// 字符串标识形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[" + Ip + "] : 在线时间 " + GetOnlineTime();
        }
    }

}
