using ClassLibrary_Crane;
using HslCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace 自动化库存管理.Command
{
    public class HostComputerCommand
    {
        public struct P
        {
            public int X;
            public int Y;
            public int Z;
        }
        // Create a new dictionary of strings, with string keys.
        //
        static Dictionary<string, string> dbRun =
            new Dictionary<string, string>();
       public static Dictionary<string, int> dbData =
            new Dictionary<string, int>();
        static  bool stopFlag=false;
        private static void InitDbRun()
        {
            if (dbRun.Count<1)
            {
                //添加运行指令DB块信息
                dbRun.Add("指令号", "DB100.00");
                dbRun.Add("指令类型", "DB100.04");
                dbRun.Add("X1", "DB100.08");
                dbRun.Add("Y1", "DB100.12");
                dbRun.Add("Z1", "DB100.16");
                dbRun.Add("X2", "DB100.20");
                dbRun.Add("Y2", "DB100.24");
                dbRun.Add("Z2", "DB100.28");
                dbRun.Add("X3", "DB100.32");
                dbRun.Add("Y3", "DB100.36");
                dbRun.Add("Z3", "DB100.40");
                dbRun.Add("X4", "DB100.44");
                dbRun.Add("Y4", "DB100.48");
                dbRun.Add("Z4", "DB100.52");
                dbRun.Add("取", "DB100.56");
                dbRun.Add("放", "DB100.56");
                dbRun.Add("左", "DB100.60");
                dbRun.Add("右", "DB100.60");
                dbRun.Add("运行指令", "DB100.64");
                //dbRun.Add("备用", "DB100.68");
                //dbRun.Add("备用", "DB100.72");
                //dbRun.Add("备用", "DB100.76");
                //dbRun.Add("备用", "DB100.80");
                //添加反馈DB块信息
                dbRun.Add("执行完成", "DB101.00");
                dbRun.Add("X", "DB101.04");
                dbRun.Add("Y", "DB101.08");
                dbRun.Add("Z", "DB101.12");
                dbRun.Add("货叉位置", "DB101.16");
                dbRun.Add("夹具开闭", "DB101.20");
                dbRun.Add("X速度", "DB101.24");
                dbRun.Add("Y速度", "DB101.28");
                dbRun.Add("Z速度", "DB101.32");
                //添加故障DB块信息
                dbRun.Add("起升变频器故障", "DB103.1.3");
                dbRun.Add("小车变频器故障", "DB103.1.4");
                dbRun.Add("大车变频器故障", "DB103.1.5");
                dbRun.Add("货叉变频器故障", "DB103.1.6");
                dbRun.Add("自动模式", "DB103.2.6");
                //添加复位DB块信息
                dbRun.Add("复位", "DB104.04");
                //添加急停DB块信息
                dbRun.Add("急停", "DB104.00");
                //添加启动DB块信息
                dbRun.Add("启动", "DB104.00");
                //添加停止DB块信息
                dbRun.Add("停止", "DB104.00");
                //添加心跳DB块信息
                dbRun.Add("心跳", "DB102.00.0");
                //添加夹具动作3：回原位    1：左  2：右
                dbData.Add("原位", 3);
                dbData.Add("前", 1);
                dbData.Add("后", 2);
            }
            
        }
        // Add some elements to the dictionary. There are no 
        // duplicate keys, but some of the values are duplicates.
        public HostComputerCommand()
        {
           
        }
        public enum Clamp:int
        {
            Front = 1,//前面
            Behind = 2,//后面
            Median = 3,//原位
            Fetch = 1,//取
            Rlease = 2//放
        }
        /// <summary>
        /// 指令类型
        /// </summary>
        public enum CommandTye : int
        {
            OneZeroOne = 101,//
            OneZeroTwo = 102,// 2代表连续运行的2个位置点位 
            OneZeroThree = 103,//3代表连续运行的3个位置点位
            TwoZeroOne = 201,//2代表货物取放指令   ，   1代表最终的目标点位置   ，在此位置才能动夹具 
            ThreeZeroOne = 301//货叉伸缩
        }
        string[] commandRunAddress = new string[] {
            "DB100.00", 
            "DB100.04", 
            "DB100.08", "DB100.12", "DB100.16", 
            "DB100.20", "DB100.24", "DB100.28", 
            "DB100.32", "DB100.36","DB100.40",
            "DB100.44", "DB100.48", "DB100.52",
            "DB100.56",
            "DB100.60",
            "DB100.64.0",
        };
        enum CommandRun
        {
            CommandNum,
            CommandType,
            X1,Y1,Z1,
            X2,Y2,Z2,
            X3,Y3,Z3,
            X4,Y4,Z4,
            place,
            action,
            run

        }
        /// <summary>
        /// 指令地址
        /// </summary>
        //enum CommandAddress 
        //{
        //    [EnumDescription("星期一")]
        //    Monday,
        //}
        /// <summary>
        /// 写入101指令
        /// </summary>
        /// <param name="commandNum">指令号</param>
        /// <param name="commandType">指令类型</param>
        /// <param name="x">大车位置</param>
        /// <param name="y">小车位置</param>
        /// <param name="z">起升位置</param>
        /// <returns></returns>
        public  bool WriteOneZeroOneCommand(SiemensS7Net_New siemensTcpNet,int commandNum,int commandType,int x,int y,int z)
        {
            InitDbRun();
            if (
                siemensTcpNet.Write(dbRun["指令号"], commandNum).IsSuccess &&
                siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
               siemensTcpNet.Write(dbRun["X1"], x).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], z).IsSuccess
                )
            {
                Thread.Sleep(1000);
                if (WriteRunCommand(siemensTcpNet))
                {
                    return true;
                }
                
                
            }
            return false;
        }
        /// <summary>
        /// 写入102指令
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <param name="commandNum"></param>
        /// <param name="commandType"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="z1"></param>
        /// <returns></returns>
        public  bool WriteOneZeroTwoCommand(SiemensS7Net_New siemensTcpNet, int commandNum, int commandType, int x, int y, int z, int x1, int y1, int z1)
        {
            InitDbRun();
            if (
                 siemensTcpNet.Write(dbRun["指令号"], commandNum).IsSuccess &&
                siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
               siemensTcpNet.Write(dbRun["X1"], x).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], z).IsSuccess &&
               siemensTcpNet.Write(dbRun["X2"], x1).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y2"], y1).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z2"], z1).IsSuccess
                )
            {
                Thread.Sleep(1000);
                if (WriteRunCommand(siemensTcpNet))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 写入103指令
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <param name="commandNum"></param>
        /// <param name="commandType"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="z1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="z2"></param>
        /// <returns></returns>
        public  bool WriteOneZeroThreeCommand(SiemensS7Net_New siemensTcpNet, int commandNum, int commandType, int x, int y, int z, int x1, int y1, int z1, int x2, int y2, int z2)
        {
            InitDbRun();
            if (
              siemensTcpNet.Write(dbRun["指令号"], commandNum).IsSuccess &&
                siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
               siemensTcpNet.Write(dbRun["X1"], x).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], z).IsSuccess &&
               siemensTcpNet.Write(dbRun["X2"], x1).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y2"], y1).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z2"], z1).IsSuccess &&
               siemensTcpNet.Write(dbRun["X3"], x2).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y3"], y2).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z3"], z2).IsSuccess 
               )
            {
                Thread.Sleep(1000);
                if (WriteRunCommand(siemensTcpNet))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 写入104指令
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <param name="commandNum"></param>
        /// <param name="commandType"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="z1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        /// <param name="z2"></param>
        /// <param name="x3"></param>
        /// <param name="y3"></param>
        /// <param name="z3"></param>
        /// <returns></returns>
        public  bool WriteOneZeroFourCommand(SiemensS7Net_New siemensTcpNet, int commandNum, int commandType, int x, int y, int z, int x1, int y1, int z1, int x2, int y2, int z2, int x3, int y3, int z3)
        {
            InitDbRun();
            if (
              siemensTcpNet.Write(dbRun["指令号"], commandNum).IsSuccess &&
                siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
               siemensTcpNet.Write(dbRun["X1"], x).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], z).IsSuccess &&
               siemensTcpNet.Write(dbRun["X2"], x1).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y2"], y1).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z2"], z1).IsSuccess &&
               siemensTcpNet.Write(dbRun["X3"], x2).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y3"], y2).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z3"], z2).IsSuccess&&
               siemensTcpNet.Write(dbRun["X4"], x3).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y4"], y3).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z4"], z3).IsSuccess 
               )
            {
                Thread.Sleep(1000);
                if (WriteRunCommand(siemensTcpNet))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 写入1类指令
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <param name="commandNum"></param>
        /// <param name="commandType"></param>
        /// <param name="listP">坐标集合</param>
        /// <returns></returns>
        public static bool WriteOneCommand(SiemensS7Net_New siemensTcpNet, int commandNum, List<P> listP,out int commandType)
        {
            InitDbRun();
            
            if (
              siemensTcpNet.Write(dbRun["指令号"], commandNum).IsSuccess
                )
            {
                if (listP.Count == 1)//101
                {
                    commandType = 101;
                    if (
                        siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
                        siemensTcpNet.Write(dbRun["X1"], listP[0].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], listP[0].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], listP[0].Z).IsSuccess)
                    {
                        return true;
                    }
                }
                else if (listP.Count == 2)//102
                {
                    commandType = 102;
                    if (
                        siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
                        siemensTcpNet.Write(dbRun["X1"], listP[0].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], listP[0].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], listP[0].Z).IsSuccess &&
               siemensTcpNet.Write(dbRun["X2"], listP[1].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y2"], listP[1].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z2"], listP[1].Z).IsSuccess
               )
                    {
                        return true;
                    }
                }
                else if (listP.Count == 3)//103
                {
                    commandType = 103;
                    if (
                        siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
                        siemensTcpNet.Write(dbRun["X1"], listP[0].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], listP[0].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], listP[0].Z).IsSuccess &&
               siemensTcpNet.Write(dbRun["X2"], listP[1].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y2"], listP[1].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z2"], listP[1].Z).IsSuccess &&
                siemensTcpNet.Write(dbRun["X3"], listP[2].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y3"], listP[2].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z3"], listP[2].Z).IsSuccess
               )
                    {
                        return true;
                    }
                }
                else if (listP.Count == 4)//104
                {
                    commandType = 104;
                    if (
                        siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
                        siemensTcpNet.Write(dbRun["X1"], listP[0].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], listP[0].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], listP[0].Z).IsSuccess &&
               siemensTcpNet.Write(dbRun["X2"], listP[1].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y2"], listP[1].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z2"], listP[1].Z).IsSuccess &&
                siemensTcpNet.Write(dbRun["X3"], listP[2].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y3"], listP[2].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z3"], listP[2].Z).IsSuccess &&
               siemensTcpNet.Write(dbRun["X4"], listP[3].X).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y4"], listP[3].Y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z4"], listP[3].Z).IsSuccess
               )
                    {
                        return true;
                    }
                }
                else
                {
                    commandType = 101;
                    return false;
                }
            }
            commandType = 101;
            return false;
        }
        /// <summary>
        /// 写入201指令 --- 1：取   2：放  
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <param name="commandNum"></param>
        /// <param name="commandType"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <param name="place">取放</param>
        /// <returns></returns>
        public  bool WriteTwoZeroOneCommand(SiemensS7Net_New siemensTcpNet, int commandNum, int commandType, int x, int y, int z,int place)
        {
            //InitDbRun();
            if (
              siemensTcpNet.Write(dbRun["指令号"], commandNum).IsSuccess &&
                siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
               siemensTcpNet.Write(dbRun["X1"], x).IsSuccess &&
               siemensTcpNet.Write(dbRun["Y1"], y).IsSuccess &&
               siemensTcpNet.Write(dbRun["Z1"], z).IsSuccess &&
               siemensTcpNet.Write(dbRun["取"], place).IsSuccess
               )
            {
                Thread.Sleep(1000);
                if (WriteRunCommand(siemensTcpNet))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 写入301指令:3：回原位    1：左  2：右  （针对货叉两侧货架均可操作的情况）
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <param name="commandNum"></param>
        /// <param name="commandType"></param>
        /// <param name="telescopic">伸缩</param>
        /// <returns></returns>
        public  bool WriteThreeZeroOneCommand(SiemensS7Net_New siemensTcpNet, int commandNum, int commandType, int telescopic)
        {
            InitDbRun();
            if (
                siemensTcpNet.Write(dbRun["指令号"], commandNum).IsSuccess &&
                siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
                siemensTcpNet.Write(dbRun["左"], telescopic).IsSuccess
                

                )
            {
                

                Thread.Sleep(1000);
                if (WriteRunCommand(siemensTcpNet))
                {
                    return true;
                }
            }
            return false;
        }
        public static bool WriteResetCommand(SiemensS7Net_New siemensTcpNet)
        {
            int writeData = 1,counter = 1;
            InitDbRun();
            if (siemensTcpNet.Write(dbRun["复位"], writeData).IsSuccess)
            {
                Thread.Sleep(1000);//延时3s
                writeData = 0;
                if (siemensTcpNet.Write(dbRun["复位"], writeData).IsSuccess)
                {
                    
                    siemensTcpNet.Write(dbRun["复位"], writeData);
                    siemensTcpNet.Write(dbRun["X2"], writeData);
                    siemensTcpNet.Write(dbRun["Y2"], writeData);
                    siemensTcpNet.Write(dbRun["Z2"], writeData);
                    siemensTcpNet.Write(dbRun["X3"], writeData);
                    siemensTcpNet.Write(dbRun["Y3"], writeData);
                    siemensTcpNet.Write(dbRun["Z3"], writeData);
                    siemensTcpNet.Write(dbRun["X4"], writeData);
                    siemensTcpNet.Write(dbRun["Y4"], writeData);
                    siemensTcpNet.Write(dbRun["Z4"], writeData);
                    siemensTcpNet.Write(dbRun["取"], writeData);
                    siemensTcpNet.Write(dbRun["左"], writeData);
                    siemensTcpNet.Write(dbRun["运行指令"], writeData);
                    
                }
                writeData = 101;
                
                if (siemensTcpNet.Write(dbRun["指令号"], counter).IsSuccess)
                {

                    siemensTcpNet.Write(dbRun["指令类型"], writeData);
                    OperateResult<byte[]> buffRead = siemensTcpNet.Read(dbRun["执行完成"], 36);//读取PLC反馈的数据
                    writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 4);
                    siemensTcpNet.Write(dbRun["X1"], writeData);
                    writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 8);
                    siemensTcpNet.Write(dbRun["Y1"], writeData);
                    writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 12);
                    siemensTcpNet.Write(dbRun["Z1"], writeData);


                }
            }
            
            return false;
        }
        /// <summary>
        /// 运行指令1：运行，延时500ms置0
        /// </summary>
        /// <param name="siemensTcpNe"></param>
        /// <returns></returns>
        public   bool WriteRunCommand(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            int writeData = 1;
            //string Info = "运行成功";
            //if (siemensTcpNet.Write("DB100.64", writeData).IsSuccess)
            if (siemensTcpNet.Write(dbRun["运行指令"], writeData).IsSuccess)
            {
                Thread.Sleep(1000);
                writeData = 0;
                ////if (siemensTcpNet.Write("DB100.64", writeData).IsSuccess)
                if (siemensTcpNet.Write(dbRun["运行指令"], writeData).IsSuccess)
                {
                    //LogNet.WriteDebug(Info);
                    return true;
                }
                //this.toolStripStatusLabelSwitch.Text = Info;
            }
            //else
            //{
            //    //this.toolStripStatusLabelSwitch.Text = "运行失败";
            //    return false;
            //}
            return false;
        }
       /// <summary>
       /// 启动指令2
       /// </summary>
       /// <param name="siemensTcpNet"></param>
       /// <returns></returns>
        public static bool WriteOpenCommand(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            int writeData = 2;
            if (siemensTcpNet.Write(dbRun["启动"], writeData).IsSuccess)
            {

                return true;
            }
            return false;
        }
       /// <summary>
       /// 停止指令1
       /// </summary>
       /// <param name="siemensTcpNet"></param>
       /// <returns></returns>
        public static bool WriteShutCommand(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            int writeData = 1;
            if (siemensTcpNet.Write(dbRun["启动"], writeData).IsSuccess)
            {

                return true;
            }
            return false;
        }
        /// <summary>
        /// 急停3
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  bool WriteStopCommand(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            int writeData = 3;
            if (stopFlag)
            {
                writeData = 0;
                if (siemensTcpNet.Write(dbRun["启动"], writeData).IsSuccess)
                {

                    stopFlag = false;
                    return true;
                }
            }
            else
            {
                if (siemensTcpNet.Write(dbRun["启动"], writeData).IsSuccess)
                {
                    stopFlag = true;
                    return true;
                }
            }
            
            return false;
        }
        /// <summary>
        /// 读取夹具位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadComplete(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            Thread.Sleep(500);
            if (siemensTcpNet.ReadInt32(dbRun["执行完成"]).Content==1|| siemensTcpNet.ReadInt32(dbRun["执行完成"]).Content==3)
            {
                WriteResetCommand(siemensTcpNet);//复位
            }
            return siemensTcpNet.ReadInt32(dbRun["执行完成"]).Content;
        }
        /// <summary>
        /// 读取大车位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadX(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["X"]).Content;
        }
        /// <summary>
        /// 读取小车位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadY(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Y"]).Content;
        }
        /// <summary>
        /// 读取起升位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadZ(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Z"]).Content;
        }
        /// <summary>
        /// 读取夹具位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadClamp(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["夹具开闭"]).Content;
        }
        /// <summary>
        /// 读取写入的指令号
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrComNum(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["指令号"]).Content;
        }
        /// <summary>
        /// 读取写入的指令类型
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrComTy(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["指令类型"]).Content;
        }
        /// <summary>
        /// 读取写入的X1
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrXOne(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["X1"]).Content;
        }
        /// <summary>
        /// 读取写入的Y1
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrYOne(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Y1"]).Content;
        }
        /// <summary>
        /// 读取写入的Z1
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrZOne(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Z1"]).Content;
        }
        /// <summary>
        /// 读取写入的X2
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrXTwo(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["X2"]).Content;
        }
        /// <summary>
        /// 读取写入的Y2
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrYTwo(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Y2"]).Content;
        }
        /// <summary>
        /// 读取写入的Z2
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrZTwo(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Z2"]).Content;
        }
        /// <summary>
        /// 读取写入的X3
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrXThree(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["X3"]).Content;
        }
        /// <summary>
        /// 读取写入的Y3
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrYThree(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Y3"]).Content;
        }
        /// <summary>
        /// 读取写入的Z3
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrZThree(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Z3"]).Content;
        }
        /// <summary>
        /// 读取写入的夹具位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrClamp(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["左"]).Content;
        }
        /// <summary>
        /// 读取写入的运行指令
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public static int ReadWrRun(SiemensS7Net_New siemensTcpNet)
        {
            InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["运行指令"]).Content;
        }
    }
}
