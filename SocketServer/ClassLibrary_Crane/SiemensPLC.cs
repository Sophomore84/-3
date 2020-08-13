using HslCommunication;
using HslCommunication.LogNet;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ClassLibrary_Crane
{
    public class SiemensPlc
    {
        //创建一个西门子PLC读写对象绑定到行车
        public SiemensS7Net_New siemensTcpNet;

        //PLC（主机）的Ip地址
        public string IpAddress { get; set; }


        //长连接状态
        public bool LongConnected { get; set; }

        //创建与行车绑定的PLC的通讯记录日志器
        public ILogNet Plc_Commu_Log { get; set; }

        //PLC线程读取状态
        public bool IsReadOk { get; set; }

        //失败计时器
        private Stopwatch failedWatch;
        //计时器时间
        private TimeSpan failedts;
        //记录的间隔时间
        private int CurrentMinutes;
        //失败后的日志文本
        private string strFailedLog = string.Empty;
        //读取失败后的次数
        private UInt32 _ReadFailedCount;
        //通信读取失败次数
        public UInt32 ReadFailedCount
        {
            get { return _ReadFailedCount; }
            set
            {
                if (ReadFailedCount != value)
                {
                    //由离线转至在线
                    if (_ReadFailedCount > 0 && value == 0)
                    {
                        if (this.failedWatch != null)
                        {
                            //获取时间
                            this.failedts = failedWatch.Elapsed;
                            //记录转至在线的时间
                            strFailedLog =  "*****网络通信断开后又检测到通信正常，此时断线总时长： {failedts.ToString()}， 重读总次数： {_ReadFailedCount}*****";
                            this.Plc_Commu_Log.WriteError(strFailedLog);
                            this.failedWatch = null;
                            
                        }
                    }
                    _ReadFailedCount = value;
                }
                if (_ReadFailedCount > 0)
                {
                    //创建计时器
                    if (this.failedWatch == null)
                    {
                        this.failedWatch = new Stopwatch();
                        //开始计时
                        this.failedWatch.Start();
                        
                    }
                     
                    if (_ReadFailedCount <= 5)
                    {
                        strFailedLog = "第{_ReadFailedCount}次读取失败,检查网络通讯";
                        this.Plc_Commu_Log.WriteWarn(strFailedLog);
                        //读取失败后，间隔1s再去读
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        //获取时间
                        this.failedts = failedWatch.Elapsed;
                        //掉线1分钟以内，且重读次数大于5
                        if (failedts.Minutes < 1)
                        {
                            //5次重读仍失败，
                            //释放长连接
                            if (this.LongConnected)
                            {
                                DisconnectPLC();                                
                            }
                            //一份钟内每5s记录一次
                            if ((int)failedts.TotalSeconds % 5 == 0)
                            {
                                strFailedLog =  "1分钟内,多次读取后仍然失败,确认网络通讯异常！失败次数： {_ReadFailedCount}";
                                this.Plc_Commu_Log.WriteError(strFailedLog);
                            }
                            Thread.Sleep(1000);
                        }
                        else
                        {
                            if (((int)failedts.TotalMinutes) == 1)
                            {
                                if (CurrentMinutes != (int)failedts.TotalMinutes)
                                {
                                    CurrentMinutes = (int)failedts.TotalMinutes;
                                    strFailedLog =  "检测网络通讯异常时间已超： {failedts.Minutes}分钟！ 通讯失败次数： {_ReadFailedCount}";
                                    this.Plc_Commu_Log.WriteError(strFailedLog);
                                }

                            }
                           
                            //时间大于1分钟后，每隔5分钟记录一次
                            if (((int)failedts.TotalMinutes)%2==0)
                            {
                                if (CurrentMinutes != (int)failedts.TotalMinutes)
                                {
                                    CurrentMinutes = (int)failedts.TotalMinutes;
                                    strFailedLog =  "检测网络通讯异常时间已有： {failedts.Minutes}分钟！ 通讯失败次数： {_ReadFailedCount}";
                                    this.Plc_Commu_Log.WriteError(strFailedLog);
                                }
                                
                            }

                        }
                        
                    }
                }
            }

        }



        ////PLC中当前X的寄存器地址
        //public string Address_Current_X { get; set; }
        ////PLC中当前Y的寄存器地址
        //public string Address_Current_Y { get; set; }
        ////PLC中当前Z的寄存器地址
        //public string Address_Current_Z { get; set; }
        ////PLC中当前旋转角度值的寄存器地址
        //public string Address_Current_Angle { get; set; }
        ////PLC中当前夹具状态
        //public string Address_Current_Sling_State { get; set; }

        //PLC中目的X的寄存器地址
        public string Address_Dest_X { get; set; }
        //PLC中目的Y的寄存器地址
        public string Address_Dest_Y { get; set; }
        //PLC中目的Z的寄存器地址
        public string Address_Dest_Z { get; set; }
        //PLC中目的旋转角度值的寄存器地址
        public string Address_Dest_Angle { get; set; }

        //PLC中使能信号X的寄存器地址
        public string Address_Enable_X { get; set; }
        //PLC中使能信号Y的寄存器地址
        public string Address_Enable_Y { get; set; }
        //PLC中使能信号Z的寄存器地址
        public string Address_Enable_Z { get; set; }
        //PLC中使能信号吊具旋转角度的寄存器地址
        public string Address_Enable_Angle { get; set; }

        //PLC中夹具抓放动作的寄存器地址
        public string Address_Sling_Action { get; set; }

        //读回写入PLC目的坐标X
        public int ReadBack_Dest_X { get; set; }
        //读回写入PLC目的坐标Y
        public int ReadBack_Dest_Y { get; set; }
        //读回写入PLC目的坐标Z
        public int ReadBack_Dest_Z { get; set; }
        //读回写入PLC目的旋转角度值
        public int ReadBack_Dest_Angle { get; set; }

        //读回写入PLC目的坐标X的使能信号
        public bool ReadBack_Enable_X { get; set; }
        //读回写入PLC目的坐标Y的使能信号
        public bool ReadBack_Enable_Y { get; set; }
        //读回写入PLC目的坐标Z的使能信号      
        public bool ReadBack_Enable_Z { get; set; }
        //读回写入PLC目的角度的使能信号      
        public bool ReadBack_Enable_Angle { get; set; }

        //读回写入抓放动作
        public Byte ReadBack_Sling_Action { get; set; }


        //带参构造函数
        /// <summary>
        /// 创建与PLC的通信连接对象，需要手动建立长连接
        /// </summary>
        /// <param name="siemens"></param>
        /// <param name="ipAddress"></param>
        public SiemensPlc(SiemensPLCS siemens, string ipAddress)
        {
            this.siemensTcpNet = new SiemensS7Net_New(siemens, ipAddress) { ConnectTimeOut = 1000 };
            this.IpAddress = ipAddress;
        }


        /// <summary>
        /// 创建与PLC的通信连接对象，并自动建立长连接
        /// </summary>
        /// <param name="siemens"></param>
        /// <param name="ipAddress"></param>
        /// <param name="plcLog"></param>
        public SiemensPlc(SiemensPLCS siemens, string ipAddress, ILogNet plcLog) : this(siemens, ipAddress)
        {
            this.Plc_Commu_Log = plcLog;
            //建立与PLC的长连接
            Initialization();
        }


        //向PLC写入X坐标
        public virtual bool Write_X(int Write_Dest_X)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Dest_X, Write_Dest_X);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                      
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                      
                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                int ReadBack_Dest_X = this.siemensTcpNet.ReadInt32(Address_Dest_X).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Dest_X != Write_Dest_X)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //向PLC写入Y坐标
        public virtual bool Write_Y(int Write_Dest_Y)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Dest_Y, Write_Dest_Y);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                       

                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                int ReadBack_Dest_Y = this.siemensTcpNet.ReadInt32(Address_Dest_Y).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Dest_Y != Write_Dest_Y)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //向PLC写入Z坐标
        public virtual bool Write_Z(int Write_Dest_Z)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Dest_Z, Write_Dest_Z);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                        
                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                int ReadBack_Dest_Z = this.siemensTcpNet.ReadInt32(Address_Dest_Z).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Dest_Z != Write_Dest_Z)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //向PLC写入吊具旋转角度值
        public virtual bool Write_Angle(float Write_Dest_Angle)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Dest_Angle, Write_Dest_Angle);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                       

                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                float ReadBack_Dest_Angle = this.siemensTcpNet.ReadFloat(Address_Dest_Angle).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Dest_Angle != Write_Dest_Angle)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //向PLC写入X使能信号
        public virtual bool Write_Enable_X(bool Write_Enable_X)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Enable_X, Write_Enable_X);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
             
                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                bool ReadBack_Enable_X = this.siemensTcpNet.ReadBool(Address_Enable_X).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Enable_X != Write_Enable_X)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        //向PLC写入Y使能信号
        public virtual bool Write_Enable_Y(bool Write_Enable_Y)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Enable_Y, Write_Enable_Y);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                       

                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                bool ReadBack_Enable_Y = this.siemensTcpNet.ReadBool(Address_Enable_Y).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Enable_Y != Write_Enable_Y)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //向PLC写入吊具旋转角度使能信号
        public virtual bool Write_Enable_Angle(bool Write_Enable_Angle)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Enable_Angle, Write_Enable_Angle);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                       

                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                bool ReadBack_Enable_Angle = this.siemensTcpNet.ReadBool(Address_Enable_Angle).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Enable_Angle != Write_Enable_Angle)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        //向PLC写入Z使能信号
        public virtual bool Write_Enable_Z(bool Write_Enable_Z)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Enable_Z, Write_Enable_Z);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                      

                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                bool ReadBack_Enable_Z = this.siemensTcpNet.ReadBool(Address_Enable_Z).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Enable_Z != Write_Enable_Z)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }


        //向PLC写入夹具抓放动作
        public virtual bool Write_Sling_Action(Byte Write_Sling_Action)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = this.siemensTcpNet.Write(Address_Sling_Action, Write_Sling_Action);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                        
                    }
                }
                //写入成功后，再读取一次，对比是否写的是否正确
                Byte ReadBack_Sling_Action = this.siemensTcpNet.ReadByte(Address_Sling_Action).Content;
                //递归调用之前先延时一段时间，让西门子读取线程读回当前写入的值，再进行比较，看是否需要进行递归操作
                if (ReadBack_Sling_Action != Write_Sling_Action)
                {
                    Thread.Sleep(100);
                    retryCount++;
                    if (retryCount > 3)
                    {
                        //3次重写失败，返回false
                        return false;
                    }
                    goto Start;
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 从PLC处读取字符串信息的再次封装包含突然断线后的处理
        /// </summary>
        /// <param name="strWhere">字符串信息的地址</param>
        /// <returns></returns>
        public string readStringFromPLC(string strWhere)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult<string> result = siemensTcpNet.ReadString(strWhere);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return null;
                    }
                    else
                    {
                       
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return null;
                        }
                        goto Start;
                        

                    }

                }

                return result.Content.Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "").TrimEnd('\0');
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// 向PLC中写入字符串信息，包含突然断线后的处理
        /// </summary>
        /// <param name="strWhere">字符串写入的地址</param>
        /// <param name="strValue">字符串值</param>
        /// <returns></returns>
        public bool writeStringToPLC(string strWhere, string strValue)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = siemensTcpNet.WriteString(strWhere, strValue);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                       

                    }

                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// 封装向PLC中写入触摸屏中文字符提示信息，包含突然断线后的处理
        /// </summary>
        /// <param name="strWhere">字符串写入的地址</param>
        /// <param name="strValue">字符串值</param>
        /// <returns></returns>
        public bool writeChineseStringToPLC(string strWhere, string strValue)
        {
            bool isSuccess;
            //重试次数
            byte retryCount = 0;
            Start:
            try
            {
                OperateResult result = siemensTcpNet.WriteUnicodeWString(strWhere, strValue);
                isSuccess = result.IsSuccess;
                if (!isSuccess)
                {
                    //先尝试ping一下，如果ping不同就没必要再试着写入数据了，
                    //如果能ping通，就快速的关闭连接，再打开连接一次，再试着写入
                    if (!PingMethod(this.IpAddress))
                    {
                        //检查通讯问题
                        return false;
                    }
                    else
                    {
                        
                        retryCount++;
                        if (retryCount > 1)
                        {
                            //1次重写失败，返回false
                            return false;
                        }
                        goto Start;
                        

                    }

                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }



        /// <summary>
        /// 长连接(调用此方法就是使用了长连接，如果不调用直接读取数据，那就是短连接)
        /// </summary>
        /// <returns></returns>
        public bool ConnectPLC()
        {
            string strDebug = string.Empty;
            OperateResult connect = this.siemensTcpNet.ConnectServer();
            //Coresocket的connect属性可反馈网络连接的实时状况
            if (connect.IsSuccess)
            {
                this.Plc_Commu_Log.WriteAnyString("\r\n\r\n");
                this.Plc_Commu_Log.WriteDebug( "软件重新启动：");
                strDebug =  "与PLC： { this.IpAddress} 建立长连接成功！";
                this.Plc_Commu_Log.WriteDebug(strDebug);
                return LongConnected = true;
            }
            else
            {
                this.Plc_Commu_Log.WriteAnyString("\r\n\r\n");
                this.Plc_Commu_Log.WriteDebug( "软件重新启动：");
                strDebug =  "与PLC： { this.IpAddress} 建立长连接失败！";
                this.Plc_Commu_Log.WriteWarn(strDebug);
                return LongConnected = false;
            } 
        }
        /// <summary>
        /// 断开连接，也就是关闭了长连接，如果再去请求数据，就变成了短连接
        /// </summary>
        public void DisconnectPLC()
        {
            this.siemensTcpNet.ConnectClose();
            LongConnected = false;
        }

        /// <summary>
        /// ping命令
        /// </summary>
        /// <param name="ip">发送主机名或Ip地址</param>
        /// <returns></returns>
        public static bool PingMethod(string ip)
        {
            bool result;
            try
            {
                Ping ping = new Ping();
                //调用同步 send 方法发送数据,将返回结果保存至PingReply实例
                PingReply pingReply = ping.Send(hostNameOrAddress: ip, timeout: 1000);
                if (pingReply.Status == IPStatus.Success)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
            }
            return result;
        }


        /// <summary>
        /// 初始化方法
        /// </summary>
        private void Initialization()
        {
            //建立读写长连接
            ConnectPLC();
        }



    }
}
