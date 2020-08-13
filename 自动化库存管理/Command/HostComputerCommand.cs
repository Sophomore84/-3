using ClassLibrary_Crane;
using HslCommunication;
using HslCommunication.LogNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自动化库存管理.Command
{
    public class HostComputerCommand
    {
        // Create a new dictionary of strings, with string keys.
        //
       static Dictionary<string, string> dbRun =
            new Dictionary<string, string>();
       public static Dictionary<string, int> dbData =
            new Dictionary<string, int>();
        static  bool stopFlag=false;
        Maticsoft.Model.tb_DingDian tb_DingDianModel = new Maticsoft.Model.tb_DingDian();
        Maticsoft.BLL.tb_DingDian tb_DingDianBLL = new Maticsoft.BLL.tb_DingDian();
        Maticsoft.Model.tb_KuWei tb_KuWeiModel = new Maticsoft.Model.tb_KuWei();
        Maticsoft.BLL.tb_KuWei tb_KuWeiBLL = new Maticsoft.BLL.tb_KuWei();
        #region 库位数据库
        Maticsoft.Model.LocationRecord locationRecordModel = new Maticsoft.Model.LocationRecord();
        Maticsoft.BLL.LocationRecord locationRecordBLL = new Maticsoft.BLL.LocationRecord();
        #endregion
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
                //添加自动情况下暂停DB块信息
                dbRun.Add("暂停", "DB104.12.00");
                //添加指令复位DB块信息
                dbRun.Add("指令复位", "DB104.14");
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
            InitDbRun();
            LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\PLC指令", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            //ResetLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\复位指令", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
        }
        /// <summary>
        /// 异步的客户端日志记录器
        /// </summary>
        private static ILogNet LogNet { get; set; }
        /// <summary>
        /// 异步的客户端日志记录器
        /// </summary>
        private static ILogNet ResetLogNet { get; set; }
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
            //InitDbRun();
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
            //InitDbRun();
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
                LogNet.WriteInfo(commandNum+" "+commandType + " " +x + " " +y + " " +z + " " +x1 + " " +y1 + " " +z1);
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
            //InitDbRun();
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
            //InitDbRun();
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
        public  bool WriteOneCommand(SiemensS7Net_New siemensTcpNet, int commandNum, List<P> listP,out int commandType)
        {
            //InitDbRun();
            //if (ReadClamp(siemensTcpNet)!=3)
            //{
            //    LogNet.WriteInfo("夹具不在中位，不能写入1类指令");
            //    commandType = 301;
            //    return false;
            //}
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
                        LogNet.WriteInfo($"{commandNum},{commandType},{listP[0].X}, {listP[0].Y}, {listP[0].Z}");
                        LogNet.WriteInfo($"读取写入的指令：{ReadWrComNum(siemensTcpNet)},{ReadWrComTy(siemensTcpNet)},{ReadWrXOne(siemensTcpNet)}, {ReadWrYOne(siemensTcpNet)}, {ReadWrZOne(siemensTcpNet)}");
                        if (WriteRunCommand(siemensTcpNet))
                        {
                            return true;
                        }
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
                        LogNet.WriteInfo($"{commandNum},{commandType},{listP[0].X}, {listP[0].Y}, {listP[0].Z},{listP[1].X}, {listP[1].Y}, {listP[1].Z}");
                        LogNet.WriteInfo($"读取写入的指令：{ReadWrComNum(siemensTcpNet)},{ReadWrComTy(siemensTcpNet)},{ReadWrXOne(siemensTcpNet)}, {ReadWrYOne(siemensTcpNet)}, {ReadWrZOne(siemensTcpNet)},{ReadWrXTwo(siemensTcpNet)}, {ReadWrYTwo(siemensTcpNet)}, {ReadWrZTwo(siemensTcpNet)}");
                        if (WriteRunCommand(siemensTcpNet))
                        {
                            return true;
                        }
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
                        LogNet.WriteInfo($"{commandNum},{commandType},{listP[0].X}, {listP[0].Y}, {listP[0].Z},{listP[1].X}, {listP[1].Y}, {listP[1].Z},{listP[2].X}, {listP[2].Y}, {listP[2].Z}");
                        LogNet.WriteInfo($"读取写入的指令：{ReadWrComNum(siemensTcpNet)},{ReadWrComTy(siemensTcpNet)},{ReadWrXOne(siemensTcpNet)}, {ReadWrYOne(siemensTcpNet)}, {ReadWrZOne(siemensTcpNet)},{ReadWrXTwo(siemensTcpNet)}, {ReadWrYTwo(siemensTcpNet)}, {ReadWrZTwo(siemensTcpNet)},{ReadWrXThree(siemensTcpNet)}, {ReadWrYThree(siemensTcpNet)}, {ReadWrZThree(siemensTcpNet)}");
                        if (WriteRunCommand(siemensTcpNet))
                        {
                            return true;
                        }
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
                        LogNet.WriteInfo($"{commandNum},{commandType},{listP[0].X}, {listP[0].Y}, {listP[0].Z},{listP[1].X}, {listP[1].Y}, {listP[1].Z},{listP[2].X}, {listP[2].Y}, {listP[2].Z},{listP[3].X}, {listP[3].Y}, {listP[3].Z}");
                        if (WriteRunCommand(siemensTcpNet))
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    commandType = 101;
                    return false;
                }
            }
            else
            {
                LogNet.WriteInfo("写入1类指令失败");
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
        public bool WriteTwoZeroOneCommand(SiemensS7Net_New siemensTcpNet, int commandNum, int commandType, int x, int y, int z,int place)
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
            //InitDbRun();
            if (
                siemensTcpNet.Write(dbRun["指令号"], commandNum).IsSuccess &&
                siemensTcpNet.Write(dbRun["指令类型"], commandType).IsSuccess &&
                siemensTcpNet.Write(dbRun["左"], telescopic).IsSuccess
                

                )
            {

                LogNet.WriteInfo($"{commandNum},{commandType},{telescopic}");
                LogNet.WriteInfo($"读出写入：{ReadWrComNum(siemensTcpNet)},{ReadWrComTy(siemensTcpNet)},{ReadWrClamp(siemensTcpNet)}");
                Thread.Sleep(1000);
                if (WriteRunCommand(siemensTcpNet))
                {
                    return true;
                }
            }
            return false;
        }
        public  bool WriteResetCommand(SiemensS7Net_New siemensTcpNet)
        {
            int writeData = 1, counter = 1;
            
            //InitDbRun();
            if (siemensTcpNet.Write(dbRun["复位"], writeData).IsSuccess)
            {

                Thread.Sleep(1000);//延时3s
                writeData = 0;
                if (
                    siemensTcpNet.Write(dbRun["复位"], writeData).IsSuccess
                    && siemensTcpNet.Write(dbRun["X2"], writeData).IsSuccess
                    && siemensTcpNet.Write(dbRun["Y2"], writeData).IsSuccess
                    && siemensTcpNet.Write(dbRun["Z2"], writeData).IsSuccess
                    && siemensTcpNet.Write(dbRun["X3"], writeData).IsSuccess
               && siemensTcpNet.Write(dbRun["Y3"], writeData).IsSuccess
               && siemensTcpNet.Write(dbRun["Z3"], writeData).IsSuccess
               && siemensTcpNet.Write(dbRun["X4"], writeData).IsSuccess
              && siemensTcpNet.Write(dbRun["Y4"], writeData).IsSuccess
               && siemensTcpNet.Write(dbRun["Z4"], writeData).IsSuccess
               && siemensTcpNet.Write(dbRun["取"], writeData).IsSuccess
               && siemensTcpNet.Write(dbRun["左"], writeData).IsSuccess
              && siemensTcpNet.Write(dbRun["运行指令"], writeData).IsSuccess
                    )
                {
                    LogNet.WriteInfo("复位成功");
                    writeData = 101;

                    if (siemensTcpNet.Write(dbRun["指令号"], counter).IsSuccess
                        && siemensTcpNet.Write(dbRun["指令类型"], writeData).IsSuccess
                        && siemensTcpNet.Write(dbRun["X1"], ReadX(siemensTcpNet)).IsSuccess
                        && siemensTcpNet.Write(dbRun["Y1"], ReadY(siemensTcpNet)).IsSuccess
                        && siemensTcpNet.Write(dbRun["Z1"], ReadZ(siemensTcpNet)).IsSuccess
                        )
                    {
                        LogNet.WriteInfo($"写入的当前位置：{counter},{writeData},{ReadX(siemensTcpNet)}，{ReadY(siemensTcpNet)}，{ReadZ(siemensTcpNet)}");
                        RuChuKuForm.counter = 1;
                        return true;
                        //siemensTcpNet.Write(dbRun["指令类型"], writeData);
                        //OperateResult<byte[]> buffRead = siemensTcpNet.Read(dbRun["执行完成"], 36);//读取PLC反馈的数据
                        //writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 4);
                        //siemensTcpNet.Write(dbRun["X1"], writeData);
                        //siemensTcpNet.Write(dbRun["X1"], ReadX(siemensTcpNet));
                        //writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 8);
                        //siemensTcpNet.Write(dbRun["Y1"], writeData);
                        //siemensTcpNet.Write(dbRun["Y1"], ReadY(siemensTcpNet));
                        //writeData = siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 12);
                        //siemensTcpNet.Write(dbRun["Z1"], writeData);
                        //siemensTcpNet.Write(dbRun["Z1"], ReadZ(siemensTcpNet));


                    }

                    else
                    {
                        LogNet.WriteInfo("写入当前位置失败");
                        return false;
                    }
                }
            }
            else
            {
                LogNet.WriteInfo("复位失败");
                return false;
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
            //InitDbRun();
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
                    LogNet.WriteDebug("运行指令写入成功");
                    return true;
                }
                else
                {
                    LogNet.WriteDebug("运行指令写0失败");
                    return false;
                }
                //this.toolStripStatusLabelSwitch.Text = Info;
            }
            else
            {
                LogNet.WriteDebug("运行指令写1失败");
                return false;
            }
            //else
            //{
            //    //this.toolStripStatusLabelSwitch.Text = "运行失败";
            //    return false;
            //}
            //return false;
        }
       /// <summary>
       /// 启动指令2
       /// </summary>
       /// <param name="siemensTcpNet"></param>
       /// <returns></returns>
        public  bool WriteOpenCommand(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            int writeData = 2;
            if (siemensTcpNet.Write(dbRun["启动"], writeData).IsSuccess)
            {
                if (WriteResetCommand(siemensTcpNet))
                {
                    LogNet.WriteInfo("启动成功");
                    return true;
                }
                else
                {
                    LogNet.WriteInfo("启动复位失败");
                    return false;
                }
                
            }
            else
            {
                LogNet.WriteInfo("启动失败");
                return false;
            }
            //return false;
        }
       /// <summary>
       /// 停止指令1
       /// </summary>
       /// <param name="siemensTcpNet"></param>
       /// <returns></returns>
        public  bool WriteShutCommand(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            int writeData = 1;
            if (siemensTcpNet.Write(dbRun["启动"], writeData).IsSuccess)
            {
                LogNet.WriteInfo("停止成功");
                return true;
            }
            else
            {
                LogNet.WriteInfo("停止失败");
                return false;
            }
            //return false;
        }
        /// <summary>
        /// 急停3
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  bool WriteStopCommand(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            int writeData = 3;
            if (stopFlag)
            {
                writeData = 0;
                if (siemensTcpNet.Write(dbRun["启动"], writeData).IsSuccess)
                {
                    LogNet.WriteInfo("急停上电");
                    stopFlag = false;
                    return true;
                }
                else
                {
                    LogNet.WriteInfo("急停上电失败");
                }
            }
            else
            {
                if (siemensTcpNet.Write(dbRun["启动"], writeData).IsSuccess)
                {
                    LogNet.WriteInfo("急停断电");
                    stopFlag = true;
                    return true;
                }
                else
                {
                    LogNet.WriteInfo("急停断电失败");
                }
            }
            
            return false;
        }
        /// <summary>
        /// 读取夹具位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadComplete(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            Thread.Sleep(2000);
            if (siemensTcpNet.ReadInt32(dbRun["执行完成"]).Content==1
                )
            {
                LogNet.WriteInfo("1类执行完成");
                //WriteResetCommand(siemensTcpNet);//复位
            }
            else if(siemensTcpNet.ReadInt32(dbRun["执行完成"]).Content == 3)
            {
                LogNet.WriteInfo("3类执行完成");
            }
            else
            {
                return 0;
            }
            LogNet.WriteInfo($"读取到的完成信号：{siemensTcpNet.ReadInt32(dbRun["执行完成"]).Content}");
            return siemensTcpNet.ReadInt32(dbRun["执行完成"]).Content;
        }
        /// <summary>
        /// 读取大车位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadX(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["X"]).Content;
        }
        /// <summary>
        /// 读取小车位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadY(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Y"]).Content;
        }
        /// <summary>
        /// 读取起升位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadZ(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Z"]).Content;
        }
        /// <summary>
        /// 读取夹具位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadClamp(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["夹具开闭"]).Content;
        }
        /// <summary>
        /// 读取写入的指令号
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrComNum(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["指令号"]).Content;
        }
        /// <summary>
        /// 读取写入的指令类型
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrComTy(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["指令类型"]).Content;
        }
        /// <summary>
        /// 读取写入的X1
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrXOne(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["X1"]).Content;
        }
        /// <summary>
        /// 读取写入的Y1
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public int ReadWrYOne(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Y1"]).Content;
        }
        /// <summary>
        /// 读取写入的Z1
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrZOne(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Z1"]).Content;
        }
        /// <summary>
        /// 读取写入的X2
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrXTwo(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["X2"]).Content;
        }
        /// <summary>
        /// 读取写入的Y2
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrYTwo(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Y2"]).Content;
        }
        /// <summary>
        /// 读取写入的Z2
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrZTwo(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Z2"]).Content;
        }
        /// <summary>
        /// 读取写入的X3
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrXThree(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["X3"]).Content;
        }
        /// <summary>
        /// 读取写入的Y3
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrYThree(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Y3"]).Content;
        }
        /// <summary>
        /// 读取写入的Z3
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrZThree(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["Z3"]).Content;
        }
        /// <summary>
        /// 读取写入的夹具位置
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrClamp(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["左"]).Content;
        }
        /// <summary>
        /// 读取写入的运行指令
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public  int ReadWrRun(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["运行指令"]).Content;
        }
        /// <summary>
        /// 读取写入的急停指令
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public int ReadStop(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadInt32(dbRun["急停"]).Content;
        }
        /// <summary>
        /// 读取暂停指令
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <returns></returns>
        public bool ReadZanTing(SiemensS7Net_New siemensTcpNet)
        {
            //InitDbRun();
            return siemensTcpNet.ReadBool(dbRun["暂停"]).Content;
        }
        public string ListString(List<P> list)
        {
            string str = "";
            foreach (P p in list)
            {
                str = p.X.ToString() + " " + p.Y.ToString() + " " + p.Z.ToString() + " ";
            }
            return str;
        }
        /// <summary>
        /// 根据货架号确定货叉方向
        /// </summary>
        /// <param name="location">货架号</param>
        /// <returns></returns>
        public int QueryClamp(string location)
        {
            //string pattern = @"-";
            location = location.Split('-')[0];
            switch (location)
            {
                case "1":
                case "3":
                    return  2;
                    //break;
                case "2":
                case "4":
                    return 1;
                    //break;
                default:
                    return 3;
                    //break;
            }
            //return false;
        }
        /// <summary>
        /// 调库时路径
        /// </summary>
        /// <param name="siemensTcpNet"></param>
        /// <param name="commandNum"></param>
        /// <param name="loca1">选中的第一个库位</param>
        /// <param name="loca2">选中的第二个库位</param>
        /// <param name="loca3">得到的中间空位</param>
        /// <returns></returns>
        public void RunPlan(SiemensS7Net_New siemensTcpNet, int commandNum, string loca1, string loca2,string loca3)
        {
            
            if (ReadClamp(siemensTcpNet) == 1 || ReadClamp(siemensTcpNet) == 2)//如果夹具不在原位，回到原位
            {

                if (WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具原位
                {
                    while (ReadComplete(siemensTcpNet) != 3)
                    {

                    }
                    LogNet.WriteInfo("夹具回到原位");
                }



            }

            if (ReadClamp(siemensTcpNet) == 0)//如果夹具不在原位，回到原位
            {

                if (WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Front))//夹具原位
                {



                    while (ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                    {

                    }

                    if (WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具原位
                    {
                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                        {

                        }

                        LogNet.WriteInfo("夹具回到原位");
                    }



                }


            }
            int commandType;
            int clamp1 = QueryClamp(loca1);
            int clamp2 = QueryClamp(loca2);

           
            if (loca3=="01")
            {
                #region 
                /* P p_shishiGuoDao,p_kuwei2GuoDao,p_kuwei2;*/ //坐标点：loca空位，koca2库位，实时过道，库位过道，库位，库位升，空位过道，空位升，空位
                                                               
                                                               //301：伸
                                                               //101：库位升
                                                               //302：缩回原位
                                                               //103：库位过道,空位过道，空位升
                                                               //301：伸
                                                               //101：空位
                                                               //301：缩回原位
                while (!WriteOneCommand(siemensTcpNet,++commandNum,GetPList_ShishiToKuWei(siemensTcpNet,loca2), out commandType))//103:实时过道，库位过道，库位2
                {
                    
                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp2))//夹具伸出 //301：伸
                {
                    
                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWeiUp(siemensTcpNet, loca2), out commandType))//101:库位2升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具原位
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                //loca2出库完成
                locationRecordModel = locationRecordBLL.GetModel(loca2);
                locationRecordModel.Action = "调库";
                locationRecordModel.OutTime = DateTime.Now.ToString();
                locationRecordModel.InTime = null;
                locationRecordModel.GoodsNum = "-" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWeiUp(siemensTcpNet, loca1), out commandType))//103:实时过道，库位过道，库位1空位升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp1))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWei(siemensTcpNet, loca1), out commandType))//101:库位1空位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具中位 //301：3
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                //loca1入库，调库
                //更新数据库：库位1的数据为库位2的数据，库位2出库
                //locationRecordModel=locationRecordBLL
                locationRecordModel.LocationName = loca1;
                locationRecordModel.InTime = DateTime.Now.ToString();
                locationRecordModel.OutTime = null;
                locationRecordModel.GoodsNum = "+" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);

                #endregion
                //return false;
            }
            else if (loca3 == "10")
            {
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWei(siemensTcpNet, loca1), out commandType))//103:实时过道，库位过道，库位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp1))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWeiUp(siemensTcpNet, loca1), out commandType))//101:库位1升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具原位
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                //loca1出库完成
                locationRecordModel = locationRecordBLL.GetModel(loca1);
                locationRecordModel.Action = "调库";
                locationRecordModel.OutTime = DateTime.Now.ToString();
                locationRecordModel.InTime = null;
                locationRecordModel.GoodsNum = "-" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWeiUp(siemensTcpNet, loca2), out commandType))//103:实时过道，库位过道，库位2空位升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp2))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWei(siemensTcpNet, loca2), out commandType))//101：库位2空位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具中位
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                locationRecordModel.LocationName = loca2;
                locationRecordModel.InTime = DateTime.Now.ToString();
                locationRecordModel.OutTime = null;
                locationRecordModel.GoodsNum = "+" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);
                //return false;
            }
            else
            {
                int clamp3 = QueryClamp(loca3);
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWei(siemensTcpNet, loca1), out commandType))//103:实时过道，库位过道，库位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp1))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWeiUp(siemensTcpNet, loca1), out commandType))//101:库位1升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具原位
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }

                locationRecordModel = locationRecordBLL.GetModel(loca1);
                locationRecordModel.Action = "调库";
                locationRecordModel.OutTime = DateTime.Now.ToString();
                locationRecordModel.InTime = null;
                locationRecordModel.GoodsNum = "-" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);

                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWeiUp(siemensTcpNet, loca3), out commandType))//103:实时过道，库位过道，库位3空位升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp3))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWei(siemensTcpNet, loca3), out commandType))//103:实时过道，库位过道，库位3空位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }

                locationRecordModel.LocationName = loca3;
                locationRecordModel.InTime = DateTime.Now.ToString();
                locationRecordModel.OutTime = null;
                locationRecordModel.GoodsNum = "+" + locationRecordModel.GoodsNum.Remove(0,1) ;
                locationRecordBLL.Add(locationRecordModel);

                //301：缩回原位
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWei(siemensTcpNet, loca2), out commandType))//103:实时过道，库位过道，库位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp2))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWeiUp(siemensTcpNet, loca2), out commandType))//101:库位2升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具原位
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }

                locationRecordModel.LocationName = loca2;
                locationRecordModel.OutTime = DateTime.Now.ToString();
                locationRecordModel.InTime = null;
                locationRecordModel.GoodsNum = "-" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);

                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWeiUp(siemensTcpNet, loca1), out commandType))//103:实时过道，库位过道，库位1空位升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp1))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWei(siemensTcpNet, loca1), out commandType))//103:实时过道，库位过道，库位1空位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }

                //301：缩回原位
                locationRecordModel.LocationName = loca1;
                locationRecordModel.InTime = DateTime.Now.ToString();
                locationRecordModel.OutTime = null;
                locationRecordModel.GoodsNum = "+" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);

                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWei(siemensTcpNet, loca3), out commandType))//103:实时过道，库位过道，库位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp3))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWeiUp(siemensTcpNet, loca3), out commandType))//101:库位3升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具原位
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }

                locationRecordModel.LocationName = loca3;
                locationRecordModel.OutTime = DateTime.Now.ToString();
                locationRecordModel.InTime = null;
                locationRecordModel.GoodsNum = "-" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_ShishiToKuWeiUp(siemensTcpNet, loca2), out commandType))//103:实时过道，库位过道，库位1空位升
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, clamp2))//夹具伸出 //301：伸
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                while (!WriteOneCommand(siemensTcpNet, ++commandNum, GetPList_KuWei(siemensTcpNet, loca2), out commandType))//103:实时过道，库位过道，库位2空位
                {

                }
                while (ReadComplete(siemensTcpNet) != 1)
                {

                }
                while (!WriteThreeZeroOneCommand(siemensTcpNet, ++commandNum, (int)CommandTye.ThreeZeroOne, (int)Clamp.Median))//夹具中位
                {

                }
                while (ReadComplete(siemensTcpNet) != 3)
                {

                }
                locationRecordModel.LocationName = loca2;
                locationRecordModel.InTime = DateTime.Now.ToString();
                locationRecordModel.OutTime = null;
                locationRecordModel.GoodsNum = "+" + locationRecordModel.GoodsNum.Remove(0, 1);
                locationRecordBLL.Add(locationRecordModel);
            }
           

           
        }
        //实时位置过道
        public P GetP_shishiGuodao(SiemensS7Net_New siemensTcpNet)
        {
            P p_shishiGuoDao;
            p_shishiGuoDao.X = ReadX(siemensTcpNet);
            p_shishiGuoDao.Y = int.Parse(tb_DingDianBLL.GetModel("过道").Y);
            p_shishiGuoDao.Z = int.Parse(tb_DingDianBLL.GetModel("过道").Z);
            return p_shishiGuoDao;
        }
        //库位
        public P GetP_KuWei(SiemensS7Net_New siemensTcpNet,string loca)
        {
            P p_kuWei;
            p_kuWei.X = int.Parse(tb_KuWeiBLL.GetModel(loca).X); 
            p_kuWei.Y = int.Parse(tb_KuWeiBLL.GetModel(loca).Y);
            p_kuWei.Z = int.Parse(tb_KuWeiBLL.GetModel(loca).Z);
            return p_kuWei;
        }
        //库位升
        public P GetP_KuWeiUp(SiemensS7Net_New siemensTcpNet,string str)
        {
            P p_kuWeiUp;
            p_kuWeiUp.X = int.Parse(tb_KuWeiBLL.GetModel(str).X);
            p_kuWeiUp.Y = int.Parse(tb_KuWeiBLL.GetModel(str).Y);
            p_kuWeiUp.Z = int.Parse(tb_KuWeiBLL.GetModel(str).Z)+ int.Parse(tb_KuWeiBLL.GetModel(str).Precision);
            return p_kuWeiUp;
        }
        //101:库位升
        public List<P> GetPList_KuWeiUp(SiemensS7Net_New siemensTcpNet, string str)
        {
            List<P> listP = new List<P>();
            listP.Add(GetP_KuWeiUp(siemensTcpNet, str));
            return listP;
        }
        //101:库位
        public List<P> GetPList_KuWei(SiemensS7Net_New siemensTcpNet, string str)
        {
            List<P> listP = new List<P>();
            listP.Add(GetP_KuWei(siemensTcpNet, str));
            return listP;
        }
        //库位过道
        public P GetP_KuWeiGuoDao(SiemensS7Net_New siemensTcpNet, string str)
        {
            
            P p_kuWeiGuoDao;
            p_kuWeiGuoDao.X = int.Parse(tb_KuWeiBLL.GetModel(str).X);
            p_kuWeiGuoDao.Y = int.Parse(tb_DingDianBLL.GetModel("过道").Y);
            p_kuWeiGuoDao.Z = int.Parse(tb_DingDianBLL.GetModel("过道").Z);
            return p_kuWeiGuoDao;
        }
        //上货台位置
        public P GetP_ShangHuoTai(SiemensS7Net_New siemensTcpNet, string str)
        {

            P p_ShangHuoTai;
            p_ShangHuoTai.X = int.Parse(tb_DingDianBLL.GetModel("上货台"+str).X);
            p_ShangHuoTai.Y = int.Parse(tb_DingDianBLL.GetModel("上货台" + str).Y);
            p_ShangHuoTai.Z = int.Parse(tb_DingDianBLL.GetModel("上货台" + str).Z);
            return p_ShangHuoTai;
        }
        //上货台升
        public P GetP_ShangHuoTaiUp(SiemensS7Net_New siemensTcpNet, string str)
        {

            P p_ShangHuoTai;
            p_ShangHuoTai.X = int.Parse(tb_DingDianBLL.GetModel("上货台" + str).X);
            p_ShangHuoTai.Y = int.Parse(tb_DingDianBLL.GetModel("上货台" + str).Y);
            p_ShangHuoTai.Z = int.Parse(tb_DingDianBLL.GetModel("上货台" + str).Z)+int.Parse(tb_DingDianBLL.GetModel("上货台" + str).Precision);
            return p_ShangHuoTai;
        }
        //上货台过道
        public P GetP_ShangHuoTaiguodao(SiemensS7Net_New siemensTcpNet, string str)
        {

            P p_ShangHuoTaiguodao;
            p_ShangHuoTaiguodao.X = int.Parse(tb_DingDianBLL.GetModel("上货台" + str).X);
            p_ShangHuoTaiguodao.Y = int.Parse(tb_DingDianBLL.GetModel("过道").Y);
            p_ShangHuoTaiguodao.Z = int.Parse(tb_DingDianBLL.GetModel("过道").Z);
            return p_ShangHuoTaiguodao;
        }
        //实时位置到库位
        public List<P> GetPList_ShishiToKuWei(SiemensS7Net_New siemensTcpNet, string str)
        {
            List<P> listP = new List<P>();
            if (Math.Abs(GetP_KuWei(siemensTcpNet, str).X-ReadX(siemensTcpNet))<15)
            {
                listP.Add(GetP_KuWei(siemensTcpNet, str));
            }
            else
            {
                listP.Add(GetP_shishiGuodao(siemensTcpNet));
                listP.Add(GetP_KuWeiGuoDao(siemensTcpNet, str));
                listP.Add(GetP_KuWei(siemensTcpNet, str));
            }
            
            return listP;
        }
        //实时位置到库位升
        public List<P> GetPList_ShishiToKuWeiUp(SiemensS7Net_New siemensTcpNet, string str)
        {
            List<P> listP = new List<P>();
            if (Math.Abs(GetP_KuWeiUp(siemensTcpNet, str).X-ReadX(siemensTcpNet)) <15)
            {
                listP.Add(GetP_KuWeiUp(siemensTcpNet, str));
            }
            else
            {
                listP.Add(GetP_shishiGuodao(siemensTcpNet));
                listP.Add(GetP_KuWeiGuoDao(siemensTcpNet, str));
                listP.Add(GetP_KuWeiUp(siemensTcpNet, str));
            }
            
            return listP;
        }
        //实时位置到上货台
        public List<P> GetPList_ShishiToShangHuoTai(SiemensS7Net_New siemensTcpNet, string str)
        {
            List<P> listP = new List<P>();
            if (Math.Abs(GetP_ShangHuoTai(siemensTcpNet, str).X- ReadX(siemensTcpNet)) < 15)
            {
                
                listP.Add(GetP_ShangHuoTai(siemensTcpNet, str));
            }
            else
            {
                listP.Add(GetP_shishiGuodao(siemensTcpNet));
                listP.Add(GetP_ShangHuoTaiguodao(siemensTcpNet, str));
                listP.Add(GetP_ShangHuoTai(siemensTcpNet, str));
            }
                
            return listP;
        }

        //实时位置到上货台升
        public List<P> GetPList_ShishiToShangHuoTaiUp(SiemensS7Net_New siemensTcpNet, string str)
        {
            List<P> listP = new List<P>();
            if (Math.Abs(GetP_ShangHuoTaiUp(siemensTcpNet, str).X - ReadX(siemensTcpNet)) < 15)
            {
                listP.Add(GetP_ShangHuoTaiUp(siemensTcpNet, str));
            }
            else
            {
                listP.Add(GetP_shishiGuodao(siemensTcpNet));
                listP.Add(GetP_ShangHuoTaiguodao(siemensTcpNet, str));
                listP.Add(GetP_ShangHuoTaiUp(siemensTcpNet, str));
            }
                
            return listP;
        }
        //上货台到上货台升
        public List<P> GetPList_ShangHuoTaiUp(SiemensS7Net_New siemensTcpNet, string str)
        {
            List<P> listP = new List<P>();
            
            listP.Add(GetP_ShangHuoTaiUp(siemensTcpNet, str));
            return listP;
        }

        //上货升到上货台
        public List<P> GetPList_ShangHuoTai(SiemensS7Net_New siemensTcpNet, string str)
        {
            List<P> listP = new List<P>();

            listP.Add(GetP_ShangHuoTai(siemensTcpNet, str));
            return listP;
        }
        //任意位置入库
        public bool InAtAnyLocation(SiemensS7Net_New siemensTcpNet)
        {
            DialogResult result;
            
            result = MessageBox.Show("任意位置出入库注意", "是否放货完成", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if(result==DialogResult.Yes)
            {

            }
                //是否放货完成，人工确认
            //缩回原位
            
            return false;
        }
        //故障复位
        public bool ErrorReset(SiemensS7Net_New siemensTcpNet)
        {
            int writeData = 1;

            //InitDbRun();
            if (siemensTcpNet.Write(dbRun["复位"], writeData).IsSuccess)
            {
                
                Thread.Sleep(1000);//延时3s
                writeData = 0;
                if (
                    siemensTcpNet.Write(dbRun["复位"], writeData).IsSuccess
              //      && siemensTcpNet.Write(dbRun["X2"], writeData).IsSuccess
              //      && siemensTcpNet.Write(dbRun["Y2"], writeData).IsSuccess
              //      && siemensTcpNet.Write(dbRun["Z2"], writeData).IsSuccess
              //      && siemensTcpNet.Write(dbRun["X3"], writeData).IsSuccess
              // && siemensTcpNet.Write(dbRun["Y3"], writeData).IsSuccess
              // && siemensTcpNet.Write(dbRun["Z3"], writeData).IsSuccess
              // && siemensTcpNet.Write(dbRun["X4"], writeData).IsSuccess
              //&& siemensTcpNet.Write(dbRun["Y4"], writeData).IsSuccess
              // && siemensTcpNet.Write(dbRun["Z4"], writeData).IsSuccess
              // && siemensTcpNet.Write(dbRun["取"], writeData).IsSuccess
              // && siemensTcpNet.Write(dbRun["左"], writeData).IsSuccess
              //&& siemensTcpNet.Write(dbRun["运行指令"], writeData).IsSuccess
                    )
                {
                    LogNet.WriteInfo("复位成功");
                    return true;
                }
            }
            else
            {
                LogNet.WriteInfo("复位失败");
                return false;
            }
            return false;
            
        }
        //指令复位
        public bool CommandReset(SiemensS7Net_New siemensTcpNet)
        {
            int value = 1;
            if(siemensTcpNet.Write(dbRun["指令复位"], value).IsSuccess)
            {
                Thread.Sleep(1000);
                value = 0;
                if (siemensTcpNet.Write(dbRun["指令复位"], value).IsSuccess)
                {
                    if (WriteOneZeroOneCommand(siemensTcpNet,1,101,ReadX(siemensTcpNet),ReadY(siemensTcpNet),ReadZ(siemensTcpNet)))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                   
                }
                else
                {
                    return false;
                }
                
            }
            else
            {
                return false;
            }
           
        }
    }
}
