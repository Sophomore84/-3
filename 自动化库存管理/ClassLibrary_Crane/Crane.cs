using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HslCommunication;
using HslCommunication.Profinet.Siemens;
using System.Threading;
using HslCommunication.LogNet;

namespace ClassLibrary_Crane
{
    public class Crane
    {

        //变频器故障类
        public BPQ_Error bpq_Error = new BPQ_Error();

        /// <summary>
        /// 改变行车功能标识信号
        /// </summary>
        public Byte Crane_WorkFlagChange_Signal { get; set; }

        //行车功能标识
        public string Crane_Work_Flag { get; set; }

        //行车入池或出池操作
        public string Crane_InorOut_Operate { get; set; }

        //行车是否是本地行车(默认是true)
        public bool IsLocalCrane { get; set; } 

        //行车PLC是否是只读(默认是false)
        public bool bPlcReadOnly { get; set; }





        ////创建与行车绑定的PLC的通讯记录日志器
        //public ILogNet Crane_PlcCommu_Log { get; set; }

        //行车运行日志器，在构造函数中进行初始化赋值
        private ILogNet Crane_Run_Log { get; set; }



        //多次检测后判断PLC是否离线
        public bool IsOnLine { get; set; }

        //当前正在执行任务的密排链编号
        public string Curent_Working_MiPaiNumber { get; set; }


        //有执行任务的池位编号
        public string WorKing_PoolName1 { get; set; }

        //有执行任务的池位编号
        public string WorKing_PoolName2 { get; set; }

        //是否会发生碰撞（行车运行时是否会与临车碰撞）
        public bool ISPengZhunag { get; set; }


        /// <summary>
        /// 判断行车正在执行任务状态（执行任务期间不接收其他指令）
        /// </summary>
        public bool IsBusy { get; set; }

        //与PLC通讯心跳检测
        public byte HeartBeat { get; set; }
       

        //行车起升的安全高度
        public int Lift_Safety_Height { get; set; }
        //行车切换到自动运行模式。
        public bool AutoRun_Model { get; set; }

        /// <summary>
        /// 行车切换到全自动运行模式。
        /// </summary>
        public bool AllAutoRun_Model { get; set; }

        //行车当前所在区域
        public string Crane_Area_Current { get; set; }
        //行车将去往的目的区域
        public string Crane_Area_Dest { get; set; }

        //刷新HMI养生池状态信息的信号。
        public bool Update_YSCMessages_Signal { get; set; }



        //行车接收到重启上位机软件信号。
        public bool Restart_Signal { get; set; }


        //行车接收到的MES信息。
        public string strRecived_From_Mes { get; set; }

        //行车接收到读触摸屏的信号
        public bool Recived_From_HMIOrMES { get; set; }


        //行车开始接收SICK
        public bool bRecived_From_SICK { get; set; }




        





        //读回行车当前坐标X
        public int Current_X_ReadBack { get; set; }

        //读回行车当前坐标Y
        public int Current_Y_ReadBack { get; set; }

        //读回行车当前坐标Z
        public int Current_Z_ReadBack { get; set; }

        //读回旋转小车（吊具）实际角度值
        public float Current_Angle_ReadBack { get; set; }

        /// <summary>
        /// 读回当前夹具动作状态 1是闭合状态，2是打开状态
        /// </summary>
        public Byte Current_Sling_State_ReadBack { get; set; }


        //读回目的坐标X
        public int Dest_X_ReadBack { get; set; }

        //读回目的坐标Y
        public int Dest_Y_ReadBack { get; set; }

        //读回目的坐标Z
        public int Dest_Z_ReadBack { get; set; }

        //读回目的角度
        public float Dest_Angle_ReadBack { get; set; }

        //读回写入吊具抓放动作
        public Byte Dest_Sling_Action_ReadBack { get; set; }


        //写入的目的坐标X
        public int Dest_X_Write { get; set; }

        //写入的目的坐标Y
        public int Dest_Y_Write { get; set; }

        //写入的目的坐标Z
        public int Dest_Z_Write { get; set; }

        //写入的目的角度
        public float Dest_Angle_Write { get; set; }

        //写入的吊具抓放动作
        public Byte Dest_Sling_Action_Write { get; set; }







        //读回坐标X的使能信号
        public bool Enable_X_ReadBack { get; set; }

        //读回坐标Y的使能信号
        public bool Enable_Y_ReadBack { get; set; }

        //读回坐标Z的使能信号      
        public bool Enable_Z_ReadBack { get; set; }

        //读回旋转小车角度使能
        public bool Enable_Angle_ReadBack { get; set; }

        //读回写入吊具抓放动作使能
        public Byte Enable_Sling_Action_ReadBack { get; set; }




       









        //使能信号
        //坐标X的使能信号
        private Byte _Enable_X;
        //坐标Y的使能信号
        private Byte _Enable_Y;
        //坐标Z的使能信号      
        private Byte _Enable_Z;
        //旋转小车角度使能
        private Byte _Enable_Angle;
        //夹具动作使能
        private Byte _Enable_Sling_Action;

        //X到位信号
        private bool _X_Arrive_Signal_ReadBack;
        //Y到位信号
        private bool _Y_Arrive_Signal_ReadBack;
        //Z到位信号
        private bool _Z_Arrive_Signal_ReadBack;
        //角度到位信号
        private bool _Angle_Arrive_Signal_ReadBack;
        //当前夹具状态
        private Byte _Sling_Arrive_Signal_ReadBack;

        //XY等待
        public AutoResetEvent mEvent_XY;
        //XY及旋转等待
        public AutoResetEvent mEvent_XYAngle;
        //Z等待
        public AutoResetEvent mEvent_Z;
        //旋转角度等待
        public AutoResetEvent mEvent_Angle;
        //安全高度等待
        public AutoResetEvent mEvent_SafeHeight;
        //夹具动作等待
        public AutoResetEvent mEvent_Sling_Action;

        /// <summary>
        /// 吊钩在池位中下落到位时的当前层的X实际位置
        /// </summary>
        public int CurrentLayer_Arrived_X;
        /// <summary>
        /// 吊钩在池位中下落到位时的当前层的Y实际位置
        /// </summary>
        public int CurrentLayer_Arrived_Y;
        /// <summary>
        /// 吊钩在池位中下落到位时的当前层的Z实际位置
        /// </summary>
        public int CurrentLayer_Arrived_Z;




        //X到位标志
        public bool X_Arrive_Signal_ReadBack
        {
            get
            {
                return _X_Arrive_Signal_ReadBack;
            }
            set
            {
                if (_X_Arrive_Signal_ReadBack != value)//这里是文本改变时的处理
                {
                    _X_Arrive_Signal_ReadBack = value;
                }
                if (_X_Arrive_Signal_ReadBack)
                {
                    //到位撤使能
                    Repeal_Enable_X();
                }
                XY_NormalWakeUP();
                XYAngle_NormalWakeup();
            }
        }
        /// <summary>
        /// X到位撤销X使能
        /// </summary>
        private void Repeal_Enable_X()
        {
            if (bPlcReadOnly)
                return;
            if (Enable_X_ReadBack)
            //撤销使能
            X_Enable(false);
            
           
        }
        //Y到位标志
        public bool Y_Arrive_Signal_ReadBack
        {
            get
            {
                return _Y_Arrive_Signal_ReadBack;
            }
            set
            {
                if (_Y_Arrive_Signal_ReadBack != value)//这里是文本改变时的处理
                {
                    _Y_Arrive_Signal_ReadBack = value;
                }
                if (_Y_Arrive_Signal_ReadBack)
                {
                    //撤销使能
                    Repeal_Enable_Y();
                }
                XY_NormalWakeUP();
                XYAngle_NormalWakeup();
            }
        }
        /// <summary>
        /// Y到位撤销X使能
        /// </summary>
        private void Repeal_Enable_Y()
        {
            if (bPlcReadOnly)
                return;
            if (Enable_Y_ReadBack)
                //撤销使能
                Y_Enable(false);
        }
        //Z到位标志
        public bool Z_Arrive_Signal_ReadBack
        {
            get
            {
                return _Z_Arrive_Signal_ReadBack;
            }
            set
            {
                if (_Z_Arrive_Signal_ReadBack != value)//这里是文本改变时的处理
                {
                    _Z_Arrive_Signal_ReadBack = value;

                    GetCurrentLayerZ();
                }
                if (_Z_Arrive_Signal_ReadBack)
                {
                    //撤销使能
                    Repeal_Enable_Z();
                }
                Z_NormalWakeUp();
                SafeHeight_NormalWakeUp();
            }
        }
        /// <summary>
        /// Z到位撤销X使能
        /// </summary>
        private void Repeal_Enable_Z()
        {
            if (bPlcReadOnly)
                return;
            if (Enable_Z_ReadBack)
                //撤销使能
                Z_Enable(false);
        }
        /// <summary>
        /// 池中Z到位时需要记录的当前层的Z值
        /// </summary>
        private void  GetCurrentLayerZ()
        {
            this.CurrentLayer_Arrived_X = this.Current_X_ReadBack;
            this.CurrentLayer_Arrived_Y = this.Current_Y_ReadBack;
            this.CurrentLayer_Arrived_Z = this.Current_Z_ReadBack;
        }


        //角度到位标志
        public bool Angle_Arrive_Signal_ReadBack
        {
            get
            {
                return _Angle_Arrive_Signal_ReadBack;
            }
            set
            {
                if (_Angle_Arrive_Signal_ReadBack != value)//这里是文本改变时的处理
                {
                    _Angle_Arrive_Signal_ReadBack = value;
                }
                if (_Angle_Arrive_Signal_ReadBack)
                {
                    //撤销使能
                    Repeal_Enable_Angle();
                }
                Angle_NormalWakeUp();
                XYAngle_NormalWakeup();
            }
        }
        /// <summary>
        /// 旋转角度到位撤销角度使能
        /// </summary>
        private void Repeal_Enable_Angle()
        {
            if (bPlcReadOnly)
                return;
            if (Enable_Angle_ReadBack)
                //撤销使能
                Angle_Enable(false);
        }

        //夹具动作状态标志
        public Byte Sling_Arrive_Signal_ReadBack
        {
            get
            {
                return _Sling_Arrive_Signal_ReadBack;
            }
            set
            {
                if (_Sling_Arrive_Signal_ReadBack != value)//这里是文本改变时的处理
                {
                    _Sling_Arrive_Signal_ReadBack = value;

                }
                Sling_NormalWakeUp();
            }
        }

        


       



        //XY联动到位时正常唤醒
        private void XY_NormalWakeUP()
        {
            if (bPlcReadOnly)
                return;
            if (!AutoRun_Model)
                return;
            if (mEvent_XY == null)
                return;
            if (_X_Arrive_Signal_ReadBack == true && _Y_Arrive_Signal_ReadBack == true)
            {
                if (Math.Abs(Current_X_ReadBack - Dest_X_Write) <= 20 && Math.Abs(Current_Y_ReadBack - Dest_Y_Write) <= 20)
                {
                    NewDebugMessage = "大、小车(XY)已到位,坐标值: {Current_X_ReadBack},{Current_Y_ReadBack}";
                    Normal_WakeUp = true;
                    //先关使能
                    X_Enable(false);
                    Y_Enable(false);
                    //调用发通知方法
                    mEvent_XY.Set();
                    while (mEvent_XY != null)
                    {
                        if (mEvent_XY == null)
                            break;
                    }

                }
            }

        }
        //X,Y,旋转角度三轴联动到位时，唤醒线程
        private void XYAngle_NormalWakeup()
        {
            if (bPlcReadOnly)
                return;
            if(!AutoRun_Model)
                return;
            if (mEvent_XYAngle == null)
                return;
            if (this._Angle_Arrive_Signal_ReadBack && this._X_Arrive_Signal_ReadBack && this._Y_Arrive_Signal_ReadBack)
            {
                if (Math.Abs(Current_Angle_ReadBack - Dest_Angle_Write) <= 10 && Math.Abs(Current_X_ReadBack - Dest_X_Write) <= 20 && Math.Abs(Current_Y_ReadBack - Dest_Y_Write) <= 20)
                {
                    //调用发通知方法
                    NewDebugMessage = "旋转角度,X,Y都已到位,角度值: {Current_Angle_ReadBack},X值：{Current_X_ReadBack},Y值：{Current_Y_ReadBack}";
                    Normal_WakeUp = true;
                    //先关使能
                    Angle_Enable(false);
                    //先关使能
                    X_Enable(false);
                    Y_Enable(false);
                    mEvent_XYAngle.Set();
                    while (mEvent_XYAngle != null)
                    {
                        if (mEvent_XYAngle == null)
                            break;
                    }
                    
                }
            }
        }
        //Z到位正常唤醒
        private void Z_NormalWakeUp()
        {
            if (bPlcReadOnly)
                return;
            if (!AutoRun_Model)
                return;
            if (mEvent_Z == null)
                return;
            if (_Z_Arrive_Signal_ReadBack == true)
            {
                if (Math.Abs(Current_Z_ReadBack - Dest_Z_Write) <= 20)
                {
                    //调用发通知方法
                    NewDebugMessage = "起升(Z)已到位,坐标值: {Current_Z_ReadBack}";
                    Normal_WakeUp = true;
                    //先关使能
                    Z_Enable(false);
                    mEvent_Z.Set();
                    while (mEvent_Z != null)
                    {
                        if (mEvent_Z == null)
                            break;
                    }
                    
                }
            }

        }

        //安全高度到位正常唤醒
        private void SafeHeight_NormalWakeUp()
        {
            if (bPlcReadOnly)
                return;
            if (!AutoRun_Model)
                return;
            if (mEvent_SafeHeight == null)
                return;
            if (_Z_Arrive_Signal_ReadBack == true)
            {
                if (Math.Abs(Current_Z_ReadBack - Dest_Z_Write) <= 20)
                {
                    //调用发通知方法
                    NewDebugMessage = "起升(Z)已到位,坐标值: {Current_Z_ReadBack}";
                    Normal_WakeUp = true;
                    //先关使能
                    Z_Enable(false);
                    mEvent_SafeHeight.Set();
                    while (mEvent_SafeHeight != null)
                    {
                        if (mEvent_SafeHeight == null)
                            break;
                    }

                }
            }

        }
        //旋转角度到位正常唤醒
        private void Angle_NormalWakeUp()
        {
            if (bPlcReadOnly)
                return;
            if (!AutoRun_Model)
                return;
            if (mEvent_Angle == null)
                return;
            if (_Angle_Arrive_Signal_ReadBack == true)
            {
                if (Math.Abs(Current_Angle_ReadBack - Dest_Angle_Write) <= 10)
                {
                    NewDebugMessage =  "旋转角度已到位,角度值: {Current_Angle_ReadBack}";
                    Normal_WakeUp = true;
                    //先关使能
                    Angle_Enable(false);
                    mEvent_Angle.Set();
                    while (mEvent_Angle != null)
                    {
                        if (mEvent_Angle == null)
                            break;
                    }
                }
            }
        }
        //夹具抓放到位正常唤醒
        private void Sling_NormalWakeUp()
        {
            if (bPlcReadOnly)
                return;
            if (!AutoRun_Model)
                return;
            if (mEvent_Sling_Action == null)
                return;
            //判断当前动作与状态一致
            if (Dest_Sling_Action_Write == Current_Sling_State_ReadBack)
            {
                //调用发通知方法
                NewDebugMessage =  "吊具动作已到位,吊具状态: {Current_Sling_State_ReadBack}";
                Normal_WakeUp = true;
                mEvent_Sling_Action.Set();
                while (mEvent_Sling_Action != null)
                {
                    if (mEvent_Sling_Action == null)
                        break;
                }
                   
            }

        }


        //唤醒等待的线程是正常还是异常
        private bool _Normal_WakeUp=true;
        /// <summary>
        /// 唤醒等待的线程是正常还是异常(默认是正常true，异常时为false)
        /// </summary>
        public bool Normal_WakeUp
        {
            get
            {
                return _Normal_WakeUp;
            }
            set
            {
                if (_Normal_WakeUp != value)//这里是文本改变时的处理
                {
                    _Normal_WakeUp = value;
                }
                if (!_Normal_WakeUp)//异常
                {
                    CallUP_UnNormal();
                }
            }
        }


        //大车X使能
        public bool X_Enable(bool enable_x)
        {
            if (this.PLC_Siemens.Write_Enable_X(enable_x))
            {
                NewDebugMessage =  "已成功写入X使能: {enable_x}";
                return true;
            }
            else
            {
                NewDebugMessage =  "写入X使能: {enable_x} 失败！";
                return false;
            }
        }
        //小车Y使能
        public bool Y_Enable(bool enable_y)
        {
            if (this.PLC_Siemens.Write_Enable_Y(enable_y))
            {
                NewDebugMessage =  "已成功写入Y使能: {enable_y}";
                return true;
            }
            else
            {
                NewDebugMessage =  "写入Y使能: {enable_y} 失败！";
                return false;
            }
        }
        //吊钩升降Z使能
        public bool Z_Enable(bool enable_z)
        {
            if (this.PLC_Siemens.Write_Enable_Z(enable_z))
            {
                NewDebugMessage =  "已成功写入Z使能: {enable_z}";
                return true;
            }
            else
            {
                NewDebugMessage =  "写入Z使能: {enable_z} 失败！";
                return false;
            }
        }
        //旋转角度使能
        public bool Angle_Enable(bool enable_angle)
        {
            if (this.PLC_Siemens.Write_Enable_Angle(enable_angle))
            {
                NewDebugMessage =  "已成功写入角度使能: {enable_angle}";
                return true;
            }
            else
            {
                NewDebugMessage =  "写入角度使能: {enable_angle} 失败！";
                return false;
            }
        }


        /// <summary>
        /// 向行车PLC中写入大车X坐标值
        /// </summary>
        /// <param name="dest_x"></param>
        /// <returns></returns>
        public bool X_Run(int dest_x)
        {
            if (dest_x == 0)
            {
                NewDebugMessage =  "异常！X坐标: {dest_x}";
                this.Normal_WakeUp = false;
                return false;
            }
            if (this.PLC_Siemens.Write_X(dest_x))
            {
                this.Dest_X_Write = dest_x;
                NewDebugMessage =  "已成功写入X坐标: {dest_x}";
                return true;
            }

            else
            {
                NewDebugMessage =  "写入X坐标: {dest_x} 失败！";
                return false;
            }
                
        }
        
        /// <summary>
        /// 向行车PLC中写入小车Y坐标值
        /// </summary>
        /// <param name="dest_y"></param>
        /// <returns></returns>
        public bool Y_Run(int dest_y)
        {
            if (dest_y == 0)
            {
                NewDebugMessage =  "异常！Y坐标: {dest_y}";
                this.Normal_WakeUp = false;
                return false;
            }
            if (this.PLC_Siemens.Write_Y(dest_y))
            {
                this.Dest_Y_Write = dest_y;
                NewDebugMessage =  "已成功写入Y坐标: {dest_y}";
                return true;
            }

            else
            {
                NewDebugMessage =  "写入Y坐标: {dest_y} 失败！";
                return false;
            }
           
        }

        /// <summary>
        /// 向行车PLC中写入起升Z坐标值
        /// </summary>
        /// <param name="dest_z"></param>
        /// <returns></returns>
        public bool Z_Run(int dest_z)
        {
            if (dest_z == 0)
            {
                NewDebugMessage =  "异常！Y坐标: {dest_z}";
                this.Normal_WakeUp = false;
                return false;
            }
            if (this.PLC_Siemens.Write_Z(dest_z))
            {
                this.Dest_Z_Write = dest_z;
                NewDebugMessage =  "已成功写入Z坐标: {dest_z}";
                return true;
            }

            else
            {
                NewDebugMessage =  "写入Z坐标: {dest_z} 失败！";
                return false;
            }

        }

        /// <summary>
        /// 向行车PLC中写入旋转角度值
        /// </summary>
        /// <param name="dest_angle"></param>
        /// <returns></returns>
        public bool Angle_Run(float dest_angle)
        {
            if (this.PLC_Siemens.Write_Angle(dest_angle))
            {
                this.Dest_Angle_Write = dest_angle;
                NewDebugMessage =  "已成功写入旋转角度: {dest_angle}";
                return true;
            }
            else
            {
                NewDebugMessage =  "写入旋转角度: {dest_angle} 失败！";
                return false;
            }
        }


        /// <summary>
        /// 向行车PLC中写入吊钩抓放动作值
        /// 1,是夹具闭合 2,是夹具打开
        /// </summary>
        /// <param name="dest_sling_action"></param>
        /// <returns></returns>
        public bool Sling_Action_Run(byte dest_sling_action)
        {
            ////写入抓放动作之前先判断下当前的夹具状态是否正确
            ////抓取前，需要夹具状态是打开状态
            //if (dest_sling_action == 1)
            //{
            //    if (this.Current_Sling_State_ReadBack != 2)
            //    {
            //        NewDebugMessage =  "请先确认夹具为打开状态: {this.Current_Sling_State_ReadBack}";
            //        return false;
            //    }
            //}
            //else
            //{
            //    //写张开时，先不进行判定
            //}
            string strAction;
            if (dest_sling_action == 1)
            {
                strAction = "夹紧";
            }
            else if (dest_sling_action == 2)
            {
                strAction = "打开";
            }
            else
            {
                strAction = "无动作";
            }
            if (this.PLC_Siemens.Write_Sling_Action(dest_sling_action))
            {
                this.Dest_Sling_Action_Write = dest_sling_action;
                
                NewDebugMessage =  "已成功写入抓放动作: {strAction}";
                return true;
            }
            else
            {
                NewDebugMessage =  "写入抓放动作: {strAction} 失败！";
                return false;
            }
        }




        /// <summary>
        /// XY向联动自动运行（不受高度的约束）,阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <returns></returns>
        public virtual bool XY_AutoRun(int dest_x,int dest_y)
        {
            if (!this.X_Run(dest_x))
                return false;
            if (!this.Y_Run(dest_y))
                return false;
            //写入使能(打开使能)
            if (!X_Enable(true))
                return false;
            //写入使能(打开使能)
            if (!Y_Enable(true))
                return false;
            NewDebugMessage =  "阻塞当前线程，等待唤醒继续";
            //创建实例(创建实例与调用waitone之间不要加代码，防止多线程调用时对象安全问题)
            mEvent_XY = new AutoResetEvent(false);
            //线程等待
            mEvent_XY.WaitOne();
            mEvent_XY = null;
            return IsNormalWakeUp();

        }



        /// <summary>
        /// XY和旋转三轴联动自动运行（不受高度的约束）,阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <param name="dest_angle"></param>
        /// <returns></returns>
        public virtual bool XYAngle_AutoRun(int dest_x, int dest_y, float dest_angle)
        {
            //写入旋转角度
            if(!this.Angle_Run(dest_angle))
                return false;
            if (!this.X_Run(dest_x))
                return false;
            if (!this.Y_Run(dest_y))
                return false;
            //写入角度使能(打开使能)
            if (!Angle_Enable(true))
                return false;
            //写入X使能(打开使能)
            if (!X_Enable(true))
                return false;
            //写入Y使能(打开使能)
            if (!Y_Enable(true))
                return false;
            NewDebugMessage =  "阻塞当前线程，等待唤醒继续";
            //创建实例(创建实例与调用waitone之间不要加代码，防止多线程调用时对象安全问题)
            mEvent_XYAngle = new AutoResetEvent(false);
            //线程等待
            mEvent_XYAngle.WaitOne();
            mEvent_XYAngle = null;
            return IsNormalWakeUp();

        }



        /// <summary>
        /// 吊钩升降Z向自动运行,阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_z"></param>
        /// <returns></returns>
        public virtual bool Z_AutoRun(int dest_z)
        {
            if (!this.Z_Run(dest_z))
                return false;
            //写入使能(打开使能)
            if (!Z_Enable(true))
                return false;
            NewDebugMessage =  "阻塞当前线程，等待唤醒继续";
            //创建实例(创建实例与调用waitone之间不要加代码，防止多线程调用时对象安全问题)
            mEvent_Z = new AutoResetEvent(false);
            //线程等待
            mEvent_Z.WaitOne();
            mEvent_Z = null;
            return IsNormalWakeUp();
        }



        /// <summary>
        /// 吊钩旋转向自动运行,阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_angle"></param>
        /// <returns></returns>
        public virtual bool Angle_AutoRun(int dest_angle)
        {
            if (!this.Angle_Run(dest_angle))
                return false;
            if(!Angle_Enable(true))
                return false;
            NewDebugMessage =  "阻塞当前线程，等待唤醒继续";
            //创建实例
            mEvent_Angle = new AutoResetEvent(false);
            //线程等待唤醒
            mEvent_Angle.WaitOne();
            mEvent_Angle = null;
            return IsNormalWakeUp();
        }

     

        /// <summary>
        /// 吊钩抓放动作自动运行,阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_sling_action"></param>
        /// <returns></returns>
        public virtual bool Sling_Action_AutoRun(byte dest_sling_action)
        {
            //写入抓放动作
            if (!this.Sling_Action_Run(dest_sling_action))
                return false;
            //抓放无使能信号？

            NewDebugMessage =  "阻塞当前线程，等待唤醒继续";
            //创建实例
            mEvent_Sling_Action = new AutoResetEvent(false);
            //线程等待
            mEvent_Sling_Action.WaitOne();
            mEvent_Sling_Action = null;
            return IsNormalWakeUp();
        }


        /// <summary>
        /// 吊钩提升到安高度全自动运行,阻塞当前线程的方式
        /// </summary>
        /// <returns></returns>
        public virtual bool SafeHeight_AutoRun()
        {
            //写入起升高度
            if (!this.Z_Run(this.Lift_Safety_Height))
                return false;
            //写入使能(打开使能)
            if (!Z_Enable(true))  
                return false;
            NewDebugMessage =  "阻塞当前线程，等待唤醒继续";

            //创建实例
            mEvent_SafeHeight = new AutoResetEvent(false);
            //线程等待
            mEvent_SafeHeight.WaitOne();
            //线程继续后，再置null
            mEvent_SafeHeight = null;
            return IsNormalWakeUp();
        }




        /// <summary>
        /// 判断线程是正常唤醒还是异常唤醒
        /// </summary>
        /// <returns></returns>
        private bool IsNormalWakeUp()
        {
            //正常到位唤醒
            if (Normal_WakeUp)
            {
                NewDebugMessage =  "##### 已正常唤醒当前线程!程序继续执行 #####";
                return true;
            }

            else //异常唤醒
            {
                NewDebugMessage =  "#####异常唤醒当前线程！任务结束返回#####";
                return false;
            }
        }

        /// <summary>
        /// 异常情况下唤醒等待线程
        /// </summary>
        private void CallUP_UnNormal()
        {
            if (mEvent_XY != null)
                mEvent_XY.Set();
            if (mEvent_XYAngle != null)
                mEvent_XYAngle.Set();
            if (mEvent_Z != null)
                mEvent_Z.Set();
            if (mEvent_Angle != null)
                mEvent_Angle.Set();
            if (mEvent_SafeHeight != null)
                mEvent_SafeHeight.Set();
            if (mEvent_Sling_Action != null)
            mEvent_Sling_Action.Set();
            //撤销使能信号
            this.X_Enable(false);
            this.Y_Enable(false);
            this.Z_Enable(false);
            this.Angle_Enable(false);
            //吊具动作
            this.Sling_Action_Run(0);
            this.IsBusy = false;
        }

        /// <summary>
        /// XY向联动自动运行（高度不在安全高度时，会首先回到安全高度）,阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_x"></param>
        /// <param name="dest_y"></param>
        /// <returns></returns>
        public virtual bool Crane_Auto_Run(int dest_x, int dest_y)
        {
            //Z不在安全高度
            if (this.Current_Z_ReadBack != this.Lift_Safety_Height)
            {
                if (!SafeHeight_AutoRun())
                    return false;
            }
            //去往目标X,Y
            if (!XY_AutoRun(dest_x, dest_y))
                return false;
            return true;
        }

        /// <summary>
        /// 行车自动运行（X,Y，旋转角度）(高度不在安全高度时，会首先回到安全高度),阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_x">目的X</param>
        /// <param name="dest_y">目的Y</param>
        /// <param name="dest_angle">目的角度</param>
        public virtual bool Crane_Auto_Run(int dest_x, int dest_y, float dest_angle)
        {
            //Z不在安全高度
            if (this.Current_Z_ReadBack != this.Lift_Safety_Height)
            {
                if (!SafeHeight_AutoRun())
                    return false;
            }
            //三轴联动
            if (!XYAngle_AutoRun(dest_x, dest_y, dest_angle))
                return false;

            return true;
        }

        /// <summary>
        /// 行车自动运行（X,Y，旋转角度）(高度不在安全高度时，会首先回到安全高度),阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_x">目的X</param>
        /// <param name="dest_y">目的Y</param>
        /// <param name="dest_angle">目的角度</param>
        public virtual bool Crane_Auto_Run(int dest_x, int dest_y, int dest_z, float dest_angle)
        {
            //首先判断当前的X，Y 及角度在不在需要写入的X，Y的及角度的位置
            if (Math.Abs(this.Current_X_ReadBack - dest_x) < 20 && Math.Abs(this.Current_Y_ReadBack - dest_y) < 20 && Math.Abs(this.Current_Angle_ReadBack - dest_angle) < 5)
            {
                //如果刚好在目标点范围内，不用再起升到安全高度了
                //直接下落Z
                if (!Z_AutoRun(dest_z))
                    return false;
                
            }
            else
            {
                //Z不在安全高度，先起Z
                if (Math.Abs(this.Current_Z_ReadBack - this.Lift_Safety_Height) > 20)
                {
                    //先起升到安全高度
                    if (!SafeHeight_AutoRun())
                        return false;
                }
                //三轴联动
                if (!XYAngle_AutoRun(dest_x, dest_y, dest_angle))
                    return false;
                //再下落Z
                if (!Z_AutoRun(dest_z))
                    return false;
            }

            return true;
        }


        /// <summary>
        /// 行车自动运行，高度不在安全高度时会先回到安全高度,阻塞当前线程的方式
        /// </summary>
        /// <param name="dest_x">目的X</param>
        /// <param name="dest_y">目的Y</param>
        /// <param name="dest_Z">目的Z</param>
        /// <param name="dest_angle">目的角度</param>
        /// <param name="dest_Sling_Action">目的动作</param>
        public virtual bool Crane_Auto_Run(int dest_x,int dest_y,int dest_z,float dest_angle,byte dest_sling_action)
        {
            //去抓之前夹具应先处于打开状态（X，Y移动之前）
            if (dest_sling_action == 1)
            {
                while (this.Current_Sling_State_ReadBack != 2)
                {
                    NewDebugMessage =  "请先确认夹具为打开状态";
                    this.WriteChiniseMessages(NewDebugMessage);
                    if (this.Current_Sling_State_ReadBack == 2)
                        break;
                }
                NewDebugMessage =  "夹具已是打开状态";
                this.WriteChiniseMessages(NewDebugMessage);
                while (!this.AutoRun_Model)
                {
                    if (this.AutoRun_Model)
                    {
                        NewDebugMessage =  "夹具已打开且切回自动模式";
                        this.WriteChiniseMessages(NewDebugMessage);
                        break;
                    }
                }
            }
            NewDebugMessage =  "继续执行";
            this.WriteChiniseMessages(NewDebugMessage);

            //首先判断当前的X，Y 及角度在不在需要写入的X，Y的及角度的位置
            if (Math.Abs(this.Current_X_ReadBack - dest_x) < 20 && Math.Abs(this.Current_Y_ReadBack - dest_y) <20 && Math.Abs(this.Current_Angle_ReadBack- dest_angle)<5)
            {
                //如果刚好在目标点范围内，不用再起升到安全高度了
                //直接下落Z
                if (!Z_AutoRun(dest_z))
                    return false;
                //再执行动作
                if (!Sling_Action_AutoRun(dest_sling_action))
                    return false;
            }
            else
            {
                //Z不在安全高度，先起Z
                if (Math.Abs(this.Current_Z_ReadBack - this.Lift_Safety_Height) > 20)
                {
                    //先起升到安全高度
                    if (!SafeHeight_AutoRun())
                        return false;
                }
                //三轴联动
                if (!XYAngle_AutoRun(dest_x, dest_y, dest_angle))
                    return false;
                //再下落Z
                if (!Z_AutoRun(dest_z))
                    return false;
                //再执行动作
                if (!Sling_Action_AutoRun(dest_sling_action))
                    return false;
            }
            ////先调整旋转小车角度(旧代码)
            //if (!Angle_AutoRun(dest_angle))
            //    return false;
            ////再去往目标X,Y(旧代码)
            //if (!XY_AutoRun(dest_x, dest_y))
            //    return false;
            return true;
        }




        

        //实现接口的属性
        private string _NewDebugMessage = string.Empty;
        public string NewDebugMessage
        {
            get
            {
                return _NewDebugMessage;
            }

            set
            {
                if (_NewDebugMessage != value)
                {
                    _NewDebugMessage = value;
                    //添加新信息
                    //AddandRecorddebugMessage();

                }
                Crane_Run_Log.WriteDebug(_NewDebugMessage);

            }
        }

        //上位机提示信息的DB块地址
        public string SWJAddress { get; set; }
               

        public void WriteChiniseMessages(string value)
        {
            this.PLC_Siemens.writeChineseStringToPLC(this.SWJAddress, value);
        }


        ////构造函数
        //public Crane()
        //{

        //}

        public SiemensPlc PLC_Siemens;
        //构造函数
        public Crane(SiemensPlc PLC_Siemens,ILogNet log)
        {
            this.PLC_Siemens = PLC_Siemens;
            this.Crane_Run_Log = log;
        }


        public override string ToString()
        {
            return  "{this.Crane_Work_Flag}：{this.PLC_Siemens.IpAddress}";
        }

        //实现接口的方法
        //public void AddandRecorddebugMessage()
        //{
        //    //记录到统一的txt文件中并显示
        //    debugDeal.AddandRecordDebugMessage(this._NewDebugMessage);
        //}
    }
}
