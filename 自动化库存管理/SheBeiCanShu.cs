using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Maticsoft.Model;
using HslCommunication.LogNet;
using System.Net.Http;
using 自动化库存管理.Command;
using HslCommunication.Enthernet;
using Newtonsoft.Json.Linq;
using HslCommunication.Core.Net;
using HslCommunication;
using SocketClient;
using System.Net;
using 养生池;
using System.Text.RegularExpressions;

namespace 自动化库存管理
{
    public partial class SheBeiCanShu : Form
    {
        bool lianXuYunDongFlag=false,ydFlag=false,btnRunFlag=false;
        P qd, gd1, gd2, shangHuoTai, kw,zuoBiao1, zuoBiao2, zuoBiao3;//起点，过道1，过道2，坐标1，坐标2，坐标3
        IList<P> kwList = new List<P>();//库位点集合
        bool lihuoTaiFlag = false, kuWeiFlag = false,first=false,sencond = false, third = false,four=false;//到达理货台和库位的标志
        IList<Maticsoft.Model.tb_KuWei> tb_KuWeiList;

        Maticsoft.Model.tb_DingDian tb_DingDianModel = new Maticsoft.Model.tb_DingDian();
        Maticsoft.BLL.tb_DingDian tb_DingDianBLL = new Maticsoft.BLL.tb_DingDian();

        Maticsoft.Model.tb_KuWei tb_KuWeiModel = new Maticsoft.Model.tb_KuWei();
        Maticsoft.BLL.tb_KuWei tb_KuWeiBLL = new Maticsoft.BLL.tb_KuWei();

        int zhiLingHao, zhilingLeiXing, X1, Y1, Z1, huoChaWeiZhi, huoChaCaoZuo;

        HslCommunication.OperateResult<byte[]> buffWrite;
        HslCommunication.OperateResult<byte[]> buffRead;
        HslCommunication.OperateResult<byte[]> buffReadXinTiao;
        HslCommunication.OperateResult<byte[]> buffReadShangDian;
        HslCommunication.OperateResult<byte[]> buffReadState;//读取故障状态
        System.Threading.Timer stateTimer;

        private async void btnStart_Click(object sender, EventArgs e)
        {
            //IPHostEntry ipHostInfo = Dns.GetHostEntry("DESKTOP-4JE1M8G");
            await RuChuKuForm.asynchronousClient.StartClient(GeiJiaJu.Text);
            //await RuChuKuForm.synchronousClient.StartClient(GeiJiaJu.Text);
            //DaCheX.Text += "\n";

            //try
            //{
            //    int length = await ExampleMethodAsync();
            //    // Note that you could put "await ExampleMethodAsync()" in the next line where  
            //    // "length" is, but due to when '+=' fetches the value of ResultsTextBox, you  
            //    // would not see the global side effect of ExampleMethodAsync setting the text.  
            //    DaCheX.Text += String.Format("Length: {0:N0}\n", length);
            //}
            //catch (Exception)
            //{
            //    // Process the exception if one occurs.  
            //}
        }
        public async Task<int> ExampleMethodAsync()
        {
            var httpClient = new HttpClient();
            int exampleInt = (await httpClient.GetStringAsync("http://msdn.microsoft.com")).Length;
            DaCheX.Text += "Preparing to finish ExampleMethodAsync.\n";
            // After the following return statement, any method that's awaiting  
            // ExampleMethodAsync (in this case, StartButton_Click) can get the   
            // integer result.  
            return exampleInt;
        }
        public async Task RunTestAsync()
        {
            
            // After the following return statement, any method that's awaiting  
            // ExampleMethodAsync (in this case, StartButton_Click) can get the   
            // integer result.  
         await Task.Run(() =>
            {
                
                if (string.IsNullOrEmpty(GeiDaCheX.Text) && !string.IsNullOrEmpty(GeiJiaJu.Text)
                                )//货叉伸缩指令
                {
                    #region 夹具
                    zhilingLeiXing = 301;
                    RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                    RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing);//写入指令类型
                    huoChaCaoZuo = int.Parse(GeiJiaJu.Text);
                    RuChuKuForm.siemensTcpNet.Write("DB100.60", huoChaCaoZuo);//写入指令类型

                    this.BeginInvoke(new Action(() =>
                    {
                        RuChuKuForm.statusLabel.Text = "写入3类指令";//buffWrite = RuChuKuForm.siemensTcpNet.Read("DB100.00", 64);//读取PLC反馈的数据

                    }));
                    zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  301  " + GeiJiaJu.Text + "  \r\n");
                    zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                              RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                              RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                              RuChuKuForm.hostComputerCommand.ReadWrClamp(RuChuKuForm.siemensTcpNet).ToString() + " "

                                 );

                    RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);
                    while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 3)
                    {

                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "夹具运动中。。。";

                        }));
                    }
                    this.BeginInvoke(new Action(() =>
                    {
                        RuChuKuForm.statusLabel.Text = "夹具运动完成。。。";

                    }));

                    #endregion

                    #region 平板电脑发送的夹具指令
                    //RuChuKuForm.asynchronousClient.StartClient("301");
                    //NetHandle value= 0;
                    //switch (GeiJiaJu.Text)
                    //{
                    //    case "1":
                    //        value=301;
                    //        break;
                    //    case "2":
                    //        value = 302;
                    //        break;
                    //    case "3":
                    //        value = 303;
                    //        break;
                    //    default:
                    //        break;
                    //}
                    //HslCommunication.OperateResult<string> operate = NetCommand.simplifyClient.ReadFromServer(value, "夹具");
                    //if (operate.IsSuccess)
                    //{
                    //    MessageBox.Show(operate.Content);
                    //}
                    //else
                    //{
                    //    MessageBox.Show("启动失败！" + operate.Message);
                    //}
                    #endregion
                }

                else
                {

                    if (string.IsNullOrEmpty(GeiXiaoCheY.Text))
                    {

                        tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                        zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                        zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                        zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                        zhilingLeiXing = 101;
                        //RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                        if (RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter)).IsSuccess
                            && RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess
                            && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                            )
                        {
                            zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  101  "
                                    + zuoBiao1.X.ToString() + "  "
                                    + zuoBiao1.Y.ToString() + "  "
                                    + zuoBiao1.Z.ToString() + "  "
                                    + "  \r\n");
                            zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " "
                                   );
                            //发送到服务器
                            this.BeginInvoke(new Action(() =>
                            {

                                RuChuKuForm.statusLabel.Text = "写入101指令";

                            }));
                            if (RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet))
                            {
                                zhiLingCeShiLogNet.WriteAnyString("写入运行指令成功");


                            }
                            else
                            {
                                zhiLingCeShiLogNet.WriteAnyString("写入运行指令失败");
                                RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);
                            }


                        }

                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "101指令运行过程中。。。";
                            }));
                        }

                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = "101指令运行完成。。。";

                        }));

                        //    
                    }


                    //}



                    else if (string.IsNullOrEmpty(GeiQiShengZ.Text))
                    {

                        tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                        zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                        zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                        zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                        tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                        zuoBiao2.X = int.Parse(tb_DingDianModel.X);
                        zuoBiao2.Y = int.Parse(tb_DingDianModel.Y);
                        zuoBiao2.Z = int.Parse(tb_DingDianModel.Z);
                        zhilingLeiXing = 102;
                        RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                        if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess &&
                            RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                            && RuChuKuForm.siemensTcpNet.Write("DB100.20", zuoBiao2.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.24", zuoBiao2.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.28", zuoBiao2.Z).IsSuccess
                            )
                        {


                            this.BeginInvoke(new Action(() =>
                            {

                                RuChuKuForm.statusLabel.Text = "写入102指令";
                            }));
                            zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  102  "
                                  + zuoBiao1.X.ToString() + "  "
                                  + zuoBiao1.Y.ToString() + "  "
                                  + zuoBiao1.Z.ToString() + "  "
                                  + zuoBiao2.X.ToString() + "  "
                                  + zuoBiao2.Y.ToString() + "  "
                                  + zuoBiao2.Z.ToString() + "  "

                                  + "  \r\n");
                            zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrYTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " "
                                );

                            if (RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet))
                            {
                                this.BeginInvoke(new Action(() =>
                                {

                                    RuChuKuForm.statusLabel.Text = "写入运行信号";
                                }));
                                zhiLingCeShiLogNet.WriteAnyString("读取写入的运行指令 " + RuChuKuForm.hostComputerCommand.ReadWrRun(RuChuKuForm.siemensTcpNet).ToString());
                            }

                        }


                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "102指令运行过程中。。。";
                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = "102指令运行完成。。。";

                        }));



                    }

                    else
                    {

                        RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                        tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                        zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                        zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                        zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                        tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                        zuoBiao2.X = int.Parse(tb_DingDianModel.X);
                        zuoBiao2.Y = int.Parse(tb_DingDianModel.Y);
                        zuoBiao2.Z = int.Parse(tb_DingDianModel.Z);
                        tb_DingDianModel = tb_DingDianBLL.GetModel(GeiQiShengZ.Text);
                        zuoBiao3.X = int.Parse(tb_DingDianModel.X);
                        zuoBiao3.Y = int.Parse(tb_DingDianModel.Y);
                        zuoBiao3.Z = int.Parse(tb_DingDianModel.Z);
                        zhilingLeiXing = 103;

                        if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess &&
                            RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                            && RuChuKuForm.siemensTcpNet.Write("DB100.20", zuoBiao2.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.24", zuoBiao2.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.28", zuoBiao2.Z).IsSuccess
                            && RuChuKuForm.siemensTcpNet.Write("DB100.32", zuoBiao3.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.36", zuoBiao3.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.40", zuoBiao3.Z).IsSuccess
                            )
                        {
                            zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  103  "
                                   + zuoBiao1.X.ToString() + "  "
                                   + zuoBiao1.Y.ToString() + "  "
                                   + zuoBiao1.Z.ToString() + "  "
                                   + zuoBiao2.X.ToString() + "  "
                                   + zuoBiao2.Y.ToString() + "  "
                                   + zuoBiao2.Z.ToString() + "  "
                                   + zuoBiao3.X.ToString() + "  "
                                   + zuoBiao3.Y.ToString() + "  "
                                   + zuoBiao3.Z.ToString() + "  "
                                   + "  \r\n");
                            zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
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


                            this.BeginInvoke(new Action(() =>
                            {

                                RuChuKuForm.statusLabel.Text = "写入103指令";
                            }));
                            Thread.Sleep(600);
                            RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);


                        }

                        while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                        {
                            this.BeginInvoke(new Action(() =>
                            {
                                RuChuKuForm.statusLabel.Text = "103指令运行过程中。。。";
                            }));
                        }
                        this.BeginInvoke(new Action(() =>
                        {

                            RuChuKuForm.statusLabel.Text = "103指令运行完成。。。";

                        }));


                    }

                }

                //}

            });
            //await t;
           
        }
        private async void btnRun_Click(object sender, EventArgs e)
        {
            
            if (btnRunFlag)
            {
                btnRunFlag = false;
                btnRun.BackColor = Color.White;
                //取消当前正在执行的任务
            }
            else
            {
                btnRunFlag = true;
                btnRun.BackColor = Color.Red;

                await RunTestAsync();

                // 远程通知服务器启动设备 
                //HslCommunication.OperateResult<string> operate = NetCommand.simplifyClient.ReadFromServer(1, "");
                //if (operate.IsSuccess)
                //{
                //    MessageBox.Show(operate.Content);
                //}
                //else
                //{
                //    MessageBox.Show("启动失败！" + operate.Message);
                //}
                //Task t = Task.Factory.StartNew(() =>
                //{

                //    if (string.IsNullOrEmpty(GeiDaCheX.Text) && !string.IsNullOrEmpty(GeiJiaJu.Text)
                //                    )//货叉伸缩指令
                //    {

                //        zhilingLeiXing = 301;
                //        RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                //        RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing);//写入指令类型
                //        huoChaCaoZuo = int.Parse(GeiJiaJu.Text);
                //        RuChuKuForm.siemensTcpNet.Write("DB100.60", huoChaCaoZuo);//写入指令类型

                //        this.BeginInvoke(new Action(() =>
                //        {
                //            RuChuKuForm.statusLabel.Text = "写入3类指令";//buffWrite = RuChuKuForm.siemensTcpNet.Read("DB100.00", 64);//读取PLC反馈的数据

                //        }));
                //        zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  301  " + GeiJiaJu.Text + "  \r\n");
                //        zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                //                  RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                  RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                  RuChuKuForm.hostComputerCommand.ReadWrClamp(RuChuKuForm.siemensTcpNet).ToString() + " "

                //                     );

                //        RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);
                //        while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 3)
                //        {

                //            this.BeginInvoke(new Action(() =>
                //            {
                //                RuChuKuForm.statusLabel.Text = "夹具运动中。。。";

                //            }));
                //        }
                //        this.BeginInvoke(new Action(() =>
                //        {
                //            RuChuKuForm.statusLabel.Text = "夹具运动完成。。。";

                //        }));

                //    }

                //    else
                //    {

                //        if (string.IsNullOrEmpty(GeiXiaoCheY.Text))
                //        {

                //            tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                //            zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                //            zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                //            zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                //            zhilingLeiXing = 101;
                //            //RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                //            if (RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter)).IsSuccess
                //                && RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess
                //                && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                //                )
                //            {
                //                zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  101  "
                //                        + zuoBiao1.X.ToString() + "  "
                //                        + zuoBiao1.Y.ToString() + "  "
                //                        + zuoBiao1.Z.ToString() + "  "
                //                        + "  \r\n");
                //                zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " "
                //                       );

                //                this.BeginInvoke(new Action(() =>
                //                {

                //                    RuChuKuForm.statusLabel.Text = "写入101指令";

                //                }));
                //                if (RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet))
                //                {
                //                    zhiLingCeShiLogNet.WriteAnyString("写入运行指令成功");


                //                }
                //                else
                //                {
                //                    zhiLingCeShiLogNet.WriteAnyString("写入运行指令失败");
                //                    RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);
                //                }


                //            }

                //            while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 1)
                //            {
                //                this.BeginInvoke(new Action(() =>
                //                {
                //                    RuChuKuForm.statusLabel.Text = "101指令运行过程中。。。";
                //                }));
                //            }

                //            this.BeginInvoke(new Action(() =>
                //            {
                //                RuChuKuForm.statusLabel.Text = "101指令运行完成。。。";

                //            }));
                //            //    
                //        }


                //        //}



                //        else if (string.IsNullOrEmpty(GeiQiShengZ.Text))
                //        {

                //            tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                //            zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                //            zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                //            zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                //            tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                //            zuoBiao2.X = int.Parse(tb_DingDianModel.X);
                //            zuoBiao2.Y = int.Parse(tb_DingDianModel.Y);
                //            zuoBiao2.Z = int.Parse(tb_DingDianModel.Z);
                //            zhilingLeiXing = 102;
                //            RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                //            if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess &&
                //                RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                //                && RuChuKuForm.siemensTcpNet.Write("DB100.20", zuoBiao2.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.24", zuoBiao2.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.28", zuoBiao2.Z).IsSuccess
                //                )
                //            {


                //                this.BeginInvoke(new Action(() =>
                //                {

                //                    RuChuKuForm.statusLabel.Text = "写入102指令";
                //                }));
                //                zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  102  "
                //                      + zuoBiao1.X.ToString() + "  "
                //                      + zuoBiao1.Y.ToString() + "  "
                //                      + zuoBiao1.Z.ToString() + "  "
                //                      + zuoBiao2.X.ToString() + "  "
                //                      + zuoBiao2.Y.ToString() + "  "
                //                      + zuoBiao2.Z.ToString() + "  "

                //                      + "  \r\n");
                //                zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                    RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " "
                //                    );

                //                if (RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet))
                //                {
                //                    this.BeginInvoke(new Action(() =>
                //                    {

                //                        RuChuKuForm.statusLabel.Text = "写入运行信号";
                //                    }));
                //                    zhiLingCeShiLogNet.WriteAnyString("读取写入的运行指令 " + RuChuKuForm.hostComputerCommand.ReadWrRun(RuChuKuForm.siemensTcpNet).ToString());
                //                }

                //            }


                //            while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 1)
                //            {
                //                this.BeginInvoke(new Action(() =>
                //                {
                //                    RuChuKuForm.statusLabel.Text = "102指令运行过程中。。。";
                //                }));
                //            }
                //            this.BeginInvoke(new Action(() =>
                //            {

                //                RuChuKuForm.statusLabel.Text = "102指令运行完成。。。";

                //            }));



                //        }

                //        else
                //        {

                //            RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                //            tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                //            zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                //            zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                //            zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                //            tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                //            zuoBiao2.X = int.Parse(tb_DingDianModel.X);
                //            zuoBiao2.Y = int.Parse(tb_DingDianModel.Y);
                //            zuoBiao2.Z = int.Parse(tb_DingDianModel.Z);
                //            tb_DingDianModel = tb_DingDianBLL.GetModel(GeiQiShengZ.Text);
                //            zuoBiao3.X = int.Parse(tb_DingDianModel.X);
                //            zuoBiao3.Y = int.Parse(tb_DingDianModel.Y);
                //            zuoBiao3.Z = int.Parse(tb_DingDianModel.Z);
                //            zhilingLeiXing = 103;

                //            if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess &&
                //                RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                //                && RuChuKuForm.siemensTcpNet.Write("DB100.20", zuoBiao2.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.24", zuoBiao2.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.28", zuoBiao2.Z).IsSuccess
                //                && RuChuKuForm.siemensTcpNet.Write("DB100.32", zuoBiao3.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.36", zuoBiao3.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.40", zuoBiao3.Z).IsSuccess
                //                )
                //            {
                //                zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  103  "
                //                       + zuoBiao1.X.ToString() + "  "
                //                       + zuoBiao1.Y.ToString() + "  "
                //                       + zuoBiao1.Z.ToString() + "  "
                //                       + zuoBiao2.X.ToString() + "  "
                //                       + zuoBiao2.Y.ToString() + "  "
                //                       + zuoBiao2.Z.ToString() + "  "
                //                       + zuoBiao3.X.ToString() + "  "
                //                       + zuoBiao3.Y.ToString() + "  "
                //                       + zuoBiao3.Z.ToString() + "  "
                //                       + "  \r\n");
                //                zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                //                   RuChuKuForm.hostComputerCommand.ReadWrZThree(RuChuKuForm.siemensTcpNet).ToString() + " "
                //                   );


                //                this.BeginInvoke(new Action(() =>
                //                {

                //                    RuChuKuForm.statusLabel.Text = "写入103指令";
                //                }));
                //                Thread.Sleep(600);
                //                RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);


                //            }

                //            while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 1)
                //            {
                //                this.BeginInvoke(new Action(() =>
                //                {
                //                    RuChuKuForm.statusLabel.Text = "103指令运行过程中。。。";
                //                }));
                //            }
                //            this.BeginInvoke(new Action(() =>
                //            {

                //                RuChuKuForm.statusLabel.Text = "103指令运行完成。。。";

                //            }));


                //        }

                //    }

                //    //}

                //});
                //t.Wait();
            }
            
        }

        private async void timerParameter_Tick(object sender, EventArgs e)
        {
            await timerCanShuTick();
        }

        private async void CreateMultipleTasksAsync()
        {
            //Task<string> download1 =
            //   ProcessURLAsync("http://msdn.microsoft.com", client);


        }
        
        // The example displays the following output:  
        // Preparing to finish ExampleMethodAsync.  
        // Length: 53292 
        delegate void SetTextDelegate(Control Ctrl, string Text);
        delegate void SetBackgroundDelegate(Control Ctrl, Color Text);
        private Thread threadYD;
        //delegate void SetToolStripTextDelegate(ToolStripStatusLabel Ctrl, string Text);
        /// <summary>
        /// 系统的日志记录器
        /// </summary>
        private ILogNet sheBeiCanShuLogNet { get; set; }
        /// <summary>
        /// 系统的指令日志记录器
        /// </summary>
        private ILogNet zhiLingCeShiLogNet { get; set; }
        /// <summary>
        /// 系统的指令精度日志记录器
        /// </summary>
        private ILogNet precisionCeShiLogNet { get; set; }
        /// <summary>
        /// 跨线程设置控件Text
        /// </summary>
        /// <param name="Ctrl">待设置的控件</param>
        /// <param name="Text">Text</param>
        public static void SetText(Control Ctrl, string Text)
        {
            if (Ctrl.InvokeRequired == true)
            {
                Ctrl.Invoke(new SetTextDelegate(SetText), Ctrl, Text);
            }
            else
            {
                Ctrl.Text = Text;
            }
        }
        /// <summary>
        /// 跨线程设置控件Text
        /// </summary>
        /// <param name="Ctrl">待设置的控件</param>
        /// <param name="Text">Text</param>
        public static void SetBackgroundColor(Control Ctrl, Color Text)
        {
            if (Ctrl.InvokeRequired == true)
            {
                Ctrl.Invoke(new SetBackgroundDelegate(SetBackgroundColor), Ctrl, Text);
            }
            else
            {
                Ctrl.BackColor = Text;
            }
        }

        private void lxYD_Click(object sender, EventArgs e)
        {
            //tb_DingDianModel = tb_DingDianBLL.GetModel("起点");
            //qd.X = int.Parse(tb_DingDianModel.X);
            //qd.Y = int.Parse(tb_DingDianModel.Y);
            //qd.Z = int.Parse(tb_DingDianModel.Z);

            //tb_DingDianModel = tb_DingDianBLL.GetModel("过道2");
            //gd1.X = int.Parse(tb_DingDianModel.X);
            //gd1.Y = int.Parse(tb_DingDianModel.Y);
            //gd1.Z = int.Parse(tb_DingDianModel.Z);

            //tb_DingDianModel = tb_DingDianBLL.GetModel("过道3");
            //gd2.X = int.Parse(tb_DingDianModel.X);
            //gd2.Y = int.Parse(tb_DingDianModel.Y);
            //gd2.Z = int.Parse(tb_DingDianModel.Z);

            //tb_DingDianModel = tb_DingDianBLL.GetModel("上货台1");
            //shangHuoTai.X = int.Parse(tb_DingDianModel.X);
            //shangHuoTai.Y = int.Parse(tb_DingDianModel.Y);
            //shangHuoTai.Z = int.Parse(tb_DingDianModel.Z);

            //tb_KuWeiList = tb_KuWeiBLL.GetModelList("");
            //foreach (tb_KuWei kuWei in tb_KuWeiList)
            //{
            //    kw.X = int.Parse(kuWei.X);
            //    kw.Y = int.Parse(kuWei.Y);
            //    kw.Z = int.Parse(kuWei.Z);
            //    kwList.Add(kw);
            //}

            if (lianXuYunDongFlag)
            {
                lianXuYunDongFlag = false;
                lxYD.BackColor = Color.White;
            }
            else
            {
                lianXuYunDongFlag = true;
                lxYD.BackColor = Color.Red;
            }

            
            //Thread.Sleep(1000);
            
            //Thread threadYD  = new Thread(DoYD);//开启连续运动线程
            //threadYD.Start();
            //threadYD.IsBackground = true;
            //if ((threadYD.ThreadState & (ThreadState.Stopped | ThreadState.Unstarted)) == 0)
            //if (ydFlag)
            //{
            //    if (threadYD.ThreadState==ThreadState.Suspended)
            //    {
            //        threadYD.Resume();
            //    }
            //    else
            //    {
            //        threadYD.Start();
            //    }
            //    ydFlag = true;
            //}
            //else
            //{
            //    if (threadYD.IsAlive)
            //    {
            //        threadYD.Suspend();
            //    }
            //    ydFlag = false;
            //}
            //if (threadYD.ThreadState == ThreadState.Running)
            //{
            //    zhiLingCeShiLogNet.WriteInfo("\r\n线程正在运行\r\n");
            //    this.BeginInvoke(new Action(() =>
            //    {

            //        RuChuKuForm.statusLabel.Text = "有指令正在运行";

            //    }));
            //}
            //else if (threadYD.ThreadState == ThreadState.Unstarted)
            //{
            //    threadYD.Start();

            //}
            //else
            //{
            //    MessageBox.Show(threadYD.ThreadState.ToString());
            //    //threadYD.Resume();
            //}



        }
        /// <summary>
        /// //先走到起点
        //走到过道1
        //走到理货台
        //轮流走库位
        /// </summary>
        public void DoYD()
        {
            while (true)
            {
                try
                {
                    while(lianXuYunDongFlag)
                    {


                        //zhiLingCeShiLogNet.WriteInfo("\r\n开始测试\r\n");

                        //zhilingLeiXing = 301;
                        //RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                        //RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing);//写入指令类型
                        //huoChaCaoZuo = int.Parse(GeiJiaJu.Text);
                        //RuChuKuForm.siemensTcpNet.Write("DB100.60", huoChaCaoZuo);//写入指令类型
                        //this.BeginInvoke(new Action(() =>
                        //{
                        //    RuChuKuForm.statusLabel.Text = "写入3类指令";

                        //}));
                        //RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);
                        //while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) !=3)
                        //{
                        //    this.BeginInvoke(new Action(() =>
                        //    {
                        //        RuChuKuForm.statusLabel.Text = "301运行中";

                        //    }));
                        //}
                        //this.BeginInvoke(new Action(() =>
                        //{
                        //    RuChuKuForm.statusLabel.Text = "301运行完成";

                        //}));
                        //lianXuYunDongFlag = false;
                        if (string.IsNullOrEmpty(GeiDaCheX.Text) && !string.IsNullOrEmpty(GeiJiaJu.Text)
                            )//货叉伸缩指令
                        {

                            if (!first)
                            {
                                zhilingLeiXing = 301;
                                RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                                RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing);//写入指令类型
                                huoChaCaoZuo = int.Parse(GeiJiaJu.Text);
                                RuChuKuForm.siemensTcpNet.Write("DB100.60", huoChaCaoZuo);//写入指令类型
                                //buffWrite = RuChuKuForm.siemensTcpNet.Read("DB100.00", 80);//读取PLC反馈的数据

                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "写入3类指令";//buffWrite = RuChuKuForm.siemensTcpNet.Read("DB100.00", 64);//读取PLC反馈的数据

                                }));
                                zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  301  " + GeiJiaJu.Text + "  \r\n");
                                zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                          RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                          RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                          RuChuKuForm.hostComputerCommand.ReadWrClamp(RuChuKuForm.siemensTcpNet).ToString() + " " 
                                          
                                             );
                                //zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 4).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 60).ToString() + "  "
                                //    + "  \r\n");
                                RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);
                                first = true;

                                Thread.Sleep(500);
                            }

                            



                            //zhilingLeiXing = 301;

                            //RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing);//写入指令类型
                            //huoChaCaoZuo = int.Parse(GeiJiaJu.Text);
                            //RuChuKuForm.siemensTcpNet.Write("DB100.60", huoChaCaoZuo);//写入指令类型

                            //this.BeginInvoke(new Action(() =>
                            //{
                            //    RuChuKuForm.statusLabel.Text = "写入3类指令";//buffWrite = RuChuKuForm.siemensTcpNet.Read("DB100.00", 64);//读取PLC反馈的数据

                            //}));
                            //zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  301  " + GeiJiaJu.Text + "  \r\n");
                            //zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "
                            //    + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0).ToString() + "  "
                            //    + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 4).ToString() + "  "
                            //    + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 60).ToString() + "  "
                            //     + "  \r\n");
                            //while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 3)
                            //{

                            //    this.BeginInvoke(new Action(() =>
                            //    {
                            //        RuChuKuForm.statusLabel.Text = "夹具运动中。。。";

                            //    }));
                            //}
                            //this.BeginInvoke(new Action(() =>
                            //{
                            //    RuChuKuForm.statusLabel.Text = "夹具运动完成。。。";

                            //}));
                            //int i = 0;
                            //RuChuKuForm.siemensTcpNet.Write("DB101.00", i);//写入复位
                            //lianXuYunDongFlag = false;
                            //return;
                            
                            if (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) == 3)
                            {

                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "夹具运动完成";

                                }));
                                lianXuYunDongFlag = false;
                                first = false;
                            }
                            else
                            {

                                this.BeginInvoke(new Action(() =>
                                {
                                    RuChuKuForm.statusLabel.Text = "夹具运动中。。。";

                                }));
                                //int i = 0;
                                //RuChuKuForm.siemensTcpNet.Write("DB101.00", i);//写入复位

                                //return;
                            }
                        }
                        else
                        {

                            if (string.IsNullOrEmpty(GeiXiaoCheY.Text))
                            {
                                if (!sencond)
                                {
                                    tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                                    zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                                    zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                                    zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                                    zhilingLeiXing = 101;
                                    //RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                                    if (RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter)).IsSuccess
                                        && RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess
                                        && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                                        )
                                    {
                                        zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  101  "
                                                + zuoBiao1.X.ToString() + "  "
                                                + zuoBiao1.Y.ToString() + "  "
                                                + zuoBiao1.Z.ToString() + "  "
                                                + "  \r\n");
                                        zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                            RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " 
                                               );
                                        //zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 4).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 8).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 12).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 16).ToString() + "  "
                                        //   + "  \r\n");

                                        this.BeginInvoke(new Action(() =>
                                        {

                                            RuChuKuForm.statusLabel.Text = "写入101指令";

                                        }));
                                        if (RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet))
                                        {
                                            zhiLingCeShiLogNet.WriteAnyString("写入运行指令成功");


                                        }
                                        else
                                        {
                                            zhiLingCeShiLogNet.WriteAnyString("写入运行指令失败");
                                            RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);
                                        }

                                        sencond = true;
                                        Thread.Sleep(500);
                                    }

                                }
                                //tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                                //zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                                //zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                                //zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                                //zhilingLeiXing = 101;

                                //if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess
                                //    && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                                //    )
                                //{
                                //    zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  101  "
                                //            + zuoBiao1.X.ToString() + "  "
                                //            + zuoBiao1.Y.ToString() + "  "
                                //            + zuoBiao1.Z.ToString() + "  "
                                //            + "  \r\n");
                                //    //zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "
                                //    //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0).ToString() + "  "
                                //    //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 4).ToString() + "  "
                                //    //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 8).ToString() + "  "
                                //    //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 12).ToString() + "  "
                                //    //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 16).ToString() + "  "
                                //    //   + "  \r\n");

                                //    this.BeginInvoke(new Action(() =>
                                //    {

                                //        RuChuKuForm.statusLabel.Text = "写入101指令";

                                //    }));
                                //    while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 1)
                                //    {
                                //        this.BeginInvoke(new Action(() =>
                                //        {
                                //            RuChuKuForm.statusLabel.Text = "101指令运行过程中。。。";
                                //        }));
                                //    }
                                //    int i = 0;
                                //    RuChuKuForm.siemensTcpNet.Write("DB101.00", i);//写入复位//101完成复位0
                                //    lianXuYunDongFlag = false;
                                //    this.BeginInvoke(new Action(() =>
                                //    {
                                //        RuChuKuForm.statusLabel.Text = "101指令运行完成。。。";

                                //    }));
                                //    return;
                                if (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) == 1)
                                {
                                    this.BeginInvoke(new Action(() =>
                                    {
                                        RuChuKuForm.statusLabel.Text = "101指令运行完成";
                                    }));
                                    lianXuYunDongFlag = false;
                                    sencond = false;
                                }


                                else
                                {
                                    //int i = 0;
                                    //RuChuKuForm.siemensTcpNet.Write("DB101.00", i);//写入复位//101完成复位0
                                    if (this.IsHandleCreated)
                                    {
                                        this.BeginInvoke(new Action(() =>
                                        {
                                            RuChuKuForm.statusLabel.Text = "101指令运行中。。。";

                                        }));
                                    }
                                        

                                    //return;
                                }


                            }



                            else if (string.IsNullOrEmpty(GeiQiShengZ.Text))
                            {
                                if (!third)
                                {
                                    tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                                    zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                                    zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                                    zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                                    tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                                    zuoBiao2.X = int.Parse(tb_DingDianModel.X);
                                    zuoBiao2.Y = int.Parse(tb_DingDianModel.Y);
                                    zuoBiao2.Z = int.Parse(tb_DingDianModel.Z);
                                    zhilingLeiXing = 102;
                                    RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                                    if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess &&
                                        RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                                        && RuChuKuForm.siemensTcpNet.Write("DB100.20", zuoBiao2.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.24", zuoBiao2.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.28", zuoBiao2.Z).IsSuccess
                                        )
                                    {


                                        this.BeginInvoke(new Action(() =>
                                        {

                                            RuChuKuForm.statusLabel.Text = "写入102指令";
                                        }));
                                        zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  102  "
                                              + zuoBiao1.X.ToString() + "  "
                                              + zuoBiao1.Y.ToString() + "  "
                                              + zuoBiao1.Z.ToString() + "  "
                                              + zuoBiao2.X.ToString() + "  "
                                              + zuoBiao2.Y.ToString() + "  "
                                              + zuoBiao2.Z.ToString() + "  "

                                              + "  \r\n");
                                        zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "+
                                            RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString()+" "+
                                            RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                            RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " " 
                                            );
                                        //zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 4).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 8).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 12).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 16).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 20).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 24).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 28).ToString() + "  "
                                        //   + "  \r\n");
                                        if(RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet))
                                        {
                                            this.BeginInvoke(new Action(() =>
                                            {

                                                RuChuKuForm.statusLabel.Text = "写入运行信号";
                                            }));
                                            zhiLingCeShiLogNet.WriteAnyString("读取写入的运行指令 " + RuChuKuForm.hostComputerCommand.ReadWrRun(RuChuKuForm.siemensTcpNet).ToString());
                                        }
                                        third = true;
                                    }
                                }
                                //tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                                //zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                                //zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                                //zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                                //tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                                //zuoBiao2.X = int.Parse(tb_DingDianModel.X);
                                //zuoBiao2.Y = int.Parse(tb_DingDianModel.Y);
                                //zuoBiao2.Z = int.Parse(tb_DingDianModel.Z);
                                //zhilingLeiXing = 102;

                                //if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess &&
                                //    RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                                //    && RuChuKuForm.siemensTcpNet.Write("DB100.20", zuoBiao2.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.24", zuoBiao2.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.28", zuoBiao2.Z).IsSuccess
                                //    )
                                //{


                                //    this.BeginInvoke(new Action(() =>
                                //    {

                                //        RuChuKuForm.statusLabel.Text = "写入102指令";
                                //    }));
                                //    zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  102  "
                                //          + zuoBiao1.X.ToString() + "  "
                                //          + zuoBiao1.Y.ToString() + "  "
                                //          + zuoBiao1.Z.ToString() + "  "
                                //          + zuoBiao2.X.ToString() + "  "
                                //          + zuoBiao2.Y.ToString() + "  "
                                //          + zuoBiao2.Z.ToString() + "  "

                                //          + "  \r\n");

                                // while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 1)
                                // {
                                //     this.BeginInvoke(new Action(() =>
                                //     {
                                //         RuChuKuForm.statusLabel.Text = "102指令运行过程中。。。";
                                //     }));
                                // }
                                // this.BeginInvoke(new Action(() =>
                                // {

                                //     RuChuKuForm.statusLabel.Text = "102指令运行完成。。。";

                                // }));
                                // zhiLingCeShiLogNet.WriteInfo("\r\n读出完成指令：  "
                                //+ RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0).ToString() + "  "


                                // + "  \r\n");
                                // int i = 0;
                                // RuChuKuForm.siemensTcpNet.Write("DB101.00", i);//写入复位
                                // lianXuYunDongFlag = false;
                                // return;
                                //zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 4).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 8).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 12).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 16).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 20).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 24).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 28).ToString() + "  "
                                //   + "  \r\n");
                                if (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) == 1)
                                {
                                    this.BeginInvoke(new Action(() =>
                                    {
                                        RuChuKuForm.statusLabel.Text = "102指令运行完成";
                                    }));
                                    zhiLingCeShiLogNet.WriteInfo("\r\n读出完成指令：  "
                                  + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0).ToString() + "  "


                                   + "  \r\n");
                                    //int i = 0;
                                    //RuChuKuForm.siemensTcpNet.Write("DB101.00", i);//写入复位
                                    lianXuYunDongFlag = false;
                                    third = false;
                                }
                                else
                                {
                                    this.BeginInvoke(new Action(() =>
                                    {

                                        RuChuKuForm.statusLabel.Text = "102指令运行中。。。";

                                    }));

                                    //return;
                                }


                            }

                            else
                            {
                                if (!four)
                                {
                                    RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号
                                    tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                                    zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                                    zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                                    zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                                    tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                                    zuoBiao2.X = int.Parse(tb_DingDianModel.X);
                                    zuoBiao2.Y = int.Parse(tb_DingDianModel.Y);
                                    zuoBiao2.Z = int.Parse(tb_DingDianModel.Z);
                                    tb_DingDianModel = tb_DingDianBLL.GetModel(GeiQiShengZ.Text);
                                    zuoBiao3.X = int.Parse(tb_DingDianModel.X);
                                    zuoBiao3.Y = int.Parse(tb_DingDianModel.Y);
                                    zuoBiao3.Z = int.Parse(tb_DingDianModel.Z);
                                    zhilingLeiXing = 103;

                                    if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess &&
                                        RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                                        && RuChuKuForm.siemensTcpNet.Write("DB100.20", zuoBiao2.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.24", zuoBiao2.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.28", zuoBiao2.Z).IsSuccess
                                        && RuChuKuForm.siemensTcpNet.Write("DB100.32", zuoBiao3.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.36", zuoBiao3.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.40", zuoBiao3.Z).IsSuccess
                                        )
                                    {
                                        zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  103  "
                                               + zuoBiao1.X.ToString() + "  "
                                               + zuoBiao1.Y.ToString() + "  "
                                               + zuoBiao1.Z.ToString() + "  "
                                               + zuoBiao2.X.ToString() + "  "
                                               + zuoBiao2.Y.ToString() + "  "
                                               + zuoBiao2.Z.ToString() + "  "
                                               + zuoBiao3.X.ToString() + "  "
                                               + zuoBiao3.Y.ToString() + "  "
                                               + zuoBiao3.Z.ToString() + "  "
                                               + "  \r\n");
                                        zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  " +
                                           RuChuKuForm.hostComputerCommand.ReadWrComNum(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrComTy(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrXOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrYOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrZOne(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrXTwo(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrZTwo(RuChuKuForm.siemensTcpNet).ToString() + " "+
                                           RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrXThree(RuChuKuForm.siemensTcpNet).ToString() + " " +
                                           RuChuKuForm.hostComputerCommand.ReadWrZThree(RuChuKuForm.siemensTcpNet).ToString() + " "
                                           );
                                        //zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 4).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 8).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 12).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 16).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 20).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 24).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 28).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 32).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 36).ToString() + "  "
                                        //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 40).ToString() + "  "
                                        //   + "  \r\n");

                                        this.BeginInvoke(new Action(() =>
                                        {

                                            RuChuKuForm.statusLabel.Text = "写入103指令";
                                        }));
                                        Thread.Sleep(600);
                                        RuChuKuForm.hostComputerCommand.WriteRunCommand(RuChuKuForm.siemensTcpNet);
                                        four = true;

                                    }

                                }
                                //tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                                //zuoBiao1.X = int.Parse(tb_DingDianModel.X);
                                //zuoBiao1.Y = int.Parse(tb_DingDianModel.Y);
                                //zuoBiao1.Z = int.Parse(tb_DingDianModel.Z);
                                //tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                                //zuoBiao2.X = int.Parse(tb_DingDianModel.X);
                                //zuoBiao2.Y = int.Parse(tb_DingDianModel.Y);
                                //zuoBiao2.Z = int.Parse(tb_DingDianModel.Z);
                                //tb_DingDianModel = tb_DingDianBLL.GetModel(GeiQiShengZ.Text);
                                //zuoBiao3.X = int.Parse(tb_DingDianModel.X);
                                //zuoBiao3.Y = int.Parse(tb_DingDianModel.Y);
                                //zuoBiao3.Z = int.Parse(tb_DingDianModel.Z);
                                //zhilingLeiXing = 103;

                                //if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess &&
                                //    RuChuKuForm.siemensTcpNet.Write("DB100.08", zuoBiao1.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.12", zuoBiao1.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", zuoBiao1.Z).IsSuccess
                                //    && RuChuKuForm.siemensTcpNet.Write("DB100.20", zuoBiao2.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.24", zuoBiao2.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.28", zuoBiao2.Z).IsSuccess
                                //    && RuChuKuForm.siemensTcpNet.Write("DB100.32", zuoBiao3.X).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.36", zuoBiao3.Y).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.40", zuoBiao3.Z).IsSuccess
                                //    )
                                //{
                                //    zhiLingCeShiLogNet.WriteInfo("\r\n写入的指令： " + RuChuKuForm.counter.ToString() + "  103  "
                                //           + zuoBiao1.X.ToString() + "  "
                                //           + zuoBiao1.Y.ToString() + "  "
                                //           + zuoBiao1.Z.ToString() + "  "
                                //           + zuoBiao2.X.ToString() + "  "
                                //           + zuoBiao2.Y.ToString() + "  "
                                //           + zuoBiao2.Z.ToString() + "  "
                                //           + zuoBiao3.X.ToString() + "  "
                                //           + zuoBiao3.Y.ToString() + "  "
                                //           + zuoBiao3.Z.ToString() + "  "
                                //           + "  \r\n");
                                //zhiLingCeShiLogNet.WriteInfo("\r\n读出写入的指令：  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 0).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 4).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 8).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 12).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 16).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 20).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 24).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 28).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 32).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 36).ToString() + "  "
                                //   + RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffWrite.Content, 40).ToString() + "  "
                                //   + "  \r\n");
                                //this.BeginInvoke(new Action(() =>
                                //{

                                //    RuChuKuForm.statusLabel.Text = "写入103指令";
                                //}));
                                //while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 1)
                                //{
                                //    this.BeginInvoke(new Action(() =>
                                //    {
                                //        RuChuKuForm.statusLabel.Text = "103指令运行过程中。。。";
                                //    }));
                                //}
                                //this.BeginInvoke(new Action(() =>
                                //{

                                //    RuChuKuForm.statusLabel.Text = "103指令运行完成。。。";

                                //}));
                                //zhiLingCeShiLogNet.WriteInfo("\r\n读出完成指令：  "
                                //+ RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0).ToString() + "  "


                                // + "  \r\n");
                                //int i = 0;
                                //RuChuKuForm.siemensTcpNet.Write("DB101.00", i);//写入复位
                                //lianXuYunDongFlag = false;
                                //return;
                                else
                                {
                                    if (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) == 1)
                                    {
                                        this.BeginInvoke(new Action(() =>
                                        {

                                            RuChuKuForm.statusLabel.Text = "103指令运行完成。。。";

                                        }));
                                        //zhiLingCeShiLogNet.WriteInfo("\r\n读出完成指令：  "
                                        //+ RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0).ToString() + "  "


                                        // + "  \r\n");
                                        //int i = 0;
                                        //RuChuKuForm.siemensTcpNet.Write("DB101.00", i);//写入复位
                                        lianXuYunDongFlag = false;
                                        four = false;
                                        //return;
                                    }
                                    else
                                    {
                                        this.BeginInvoke(new Action(() =>
                                        {

                                            RuChuKuForm.statusLabel.Text = "103指令正在运行。。。";

                                        }));
                                    }
                                    //    //threadYD.Abort();
                                    //}

                                    //return;

                                }

                            }
                            //lianXuYunDongFlag = false;
                            //return;
                            //threadYD.Suspend();
                            //threadYD.Abort();
                        }

                    }


                }
                catch (Exception e)
                {

                    zhiLingCeShiLogNet.WriteException("DoYD", e);
                    //threadYD.Abort();
                }

            }

        }
        
        private void userButton3_Click(object sender, EventArgs e)
        {
            // 远程通知服务器停止设备
            HslCommunication.OperateResult<string> operate = NetCommand.simplifyClient.ReadFromServer(2, "");
            if (operate.IsSuccess)
            {
                MessageBox.Show(operate.Content);
            }
            else
            {
                MessageBox.Show("启动失败！" + operate.Message);
            }
        }
        private AutoSizeFormClass asc = new AutoSizeFormClass();

        // Token: 0x0400011C RID: 284
        private float X;

        // Token: 0x0400011D RID: 285
        private float Y;
        // Token: 0x0400008F RID: 143
        //private RuChuKuForm parentform = null;

        //private string userid = "";
      

        /// <summary>
        /// 跨线程设置控件Text
        /// </summary>
        /// <param name="Ctrl">待设置的控件</param>
        /// <param name="Text">Text</param>
        public static void SetText(ToolStripStatusLabel Ctrl, string Text)
        {
            //if (Ctrl.InvokeRequired == true)
            //{
            //    Ctrl.Invoke(new SetToolStripTextDelegate(SetText), Ctrl, Text);
            //}
            //else
            //{
            Ctrl.Text = Text;
            //}
        }
        public SheBeiCanShu()
        {
            sheBeiCanShuLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\设备参数", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件 
            zhiLingCeShiLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\指令测试", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件 
            precisionCeShiLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\精度测试", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件 
            InitializeComponent();
            this.Resize += SheBeiCanShu_Resize;
            X = this.Width;
            Y = this.Height;
            setTag(this);
            string[] tb_KuWeis = tb_KuWeiBLL.GetLocationList("").ToArray();
            comboBoxLocation.Items.AddRange(tb_KuWeis);
            string[] tb_DingDians = tb_DingDianBLL.GetLocationList("").ToArray();
            comboBoxLocation.Items.AddRange(tb_DingDians);
            comboBoxLocation.SelectedIndex = 0;
            //stateTimer = new System.Threading.Timer(timerCanShuTick,
            //                        null, 0, 100);
        }
        //public SheBeiCanShu(int nx, int ny)
        //{
        //    InitializeComponent();
        //    this.X = (float)base.Width;
        //    this.Y = (float)base.Height;
        //    this.asc.setTag(this);
        //    base.Width = nx;
        //    base.Height = ny;
        //    float newx = (float)nx / this.X;
        //    float newy = (float)ny / this.Y;
        //    this.asc.setControls(newx, newy, this);

        //}
        //public SheBeiCanShu(RuChuKuForm parent, int nx, int ny, string sendid)
        //{
        //    this.InitializeComponent();
        //    this.parentform = parent;
        //    this.userid = sendid;
        //    this.X = (float)base.Width;
        //    this.Y = (float)base.Height;
        //    this.asc.setTag(this);
        //    base.Width = nx;
        //    base.Height = ny;
        //    float newx = (float)nx / this.X;
        //    float newy = (float)ny / this.Y;
        //    this.asc.setControls(newx, newy, this);
        //}
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            RuChuKuForm.logForm.Show();
            //RuChuKuForm.logForm.Parent.Refresh();
            //pushClient?.ClosePush();
            //complexClient?.ClientClose();

            //System.Threading.Thread.Sleep(100);
        }
        /// <summary>
        /// 测试精度：两点之间，到为后，需确认，再执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnPrecesion_Click(object sender, EventArgs e)
        {
            await TestPrecision();
        }

        private async Task TestPrecision()
        {
            await Task.Run(() =>
            {
                //1.实时点，位置点1，过道点1，过道点2，精度点
                //2.精度点，过道点2，过道点1，位置点1
                //3.
                int commandType;
                List<P> listP = new List<P>();
                List<P> listP1 = new List<P>();
                P p1, p2, p3, p4, p5;
                tb_DingDianModel = tb_DingDianBLL.GetModel(GeiDaCheX.Text);
                p1.X = int.Parse(tb_DingDianModel.X);
                p1.Y = int.Parse(tb_DingDianModel.Y);
                p1.Z = int.Parse(tb_DingDianModel.Z);//位置点
                tb_DingDianModel = tb_DingDianBLL.GetModel(GeiXiaoCheY.Text);
                p2.X = int.Parse(tb_DingDianModel.X);
                p2.Y = int.Parse(tb_DingDianModel.Y);
                p2.Z = int.Parse(tb_DingDianModel.Z);//精度点

                tb_DingDianModel = tb_DingDianBLL.GetModel("过道");
                p3.X = p1.X;
                p3.Y = int.Parse(tb_DingDianModel.Y);
                p3.Z = int.Parse(tb_DingDianModel.Z);//位置1过道

                p4.X = p2.X;
                p4.Y = p3.Y;
                p4.Z = p3.Z;//精度点过道

                listP.Add(p3);
                listP.Add(p4);
                listP.Add(p2);

                while (true)
                {
                    
                    //RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet,++RuChuKuForm.counter,listP,out commandType);
                    if (Math.Abs(RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet) - p1.X) > 10)//如果大车实时位置与位置1不相等
                    {
                        p5.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet);
                        p5.Y = p3.Y;
                        p5.Z = p3.Z;//实时位置过道
                        listP1.Add(p5);
                        listP1.Add(p3);
                        listP1.Add(p1);//实时位置，实时位置过道，过道1，位置1
                                       //103：实时位置，实时位置过道，过道2，精度位置
                        if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP1, out commandType))
                        {
                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                            {

                            }
                            precisionCeShiLogNet.WriteInfo("位置1："+RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet).ToString()+" "+ RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet).ToString()+"  "+ RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet).ToString());
                            precisionCeShiLogNet.WriteInfo("位置1误差："+(p1.X- RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet)).ToString()+" "+
                                (p1.Y - RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet)).ToString() + " " +
                                (p1.Z - RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet)).ToString()
                                );
                            if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP, out commandType))
                            {
                                while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                                {

                                }
                                precisionCeShiLogNet.WriteInfo("精度点：" + RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet).ToString() + " " + RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet).ToString() + "  " + RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet).ToString());
                                precisionCeShiLogNet.WriteInfo("精度点误差：" + (p2.X - RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet)).ToString() + " " +
                               (p2.Y - RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet)).ToString() + " " +
                               (p2.Z - RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet)).ToString()
                               );
                            }
                        }

                    }
                   else//如果大车实时位置与位置1相等
                    {
                        //p5.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet);
                        //p5.Y = p3.Y;
                        //p5.Z = p3.Z;//实时位置过道
                        //listP1.Add(p5);
                        //listP1.Add(p3);
                        listP1.Add(p1);//实时位置，实时位置过道，过道1，位置1
                                       //103：实时位置，实时位置过道，过道2，精度位置
                        if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP1, out commandType))
                        {
                            while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                            {

                            }
                            precisionCeShiLogNet.WriteInfo("位置1：" + RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet).ToString() + " " + RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet).ToString() + "  " + RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet).ToString());
                            precisionCeShiLogNet.WriteInfo("位置1误差：" + (p1.X - RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet)).ToString() + " " +
                                (p1.Y - RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet)).ToString() + " " +
                                (p1.Z - RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet)).ToString()
                                );
                            if (RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, listP, out commandType))
                            {
                                while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                                {

                                }
                                precisionCeShiLogNet.WriteInfo("精度点：" + RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet).ToString() + " " + RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet).ToString() + "  " + RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet).ToString());
                                precisionCeShiLogNet.WriteInfo("精度点误差：" + (p2.X - RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet)).ToString() + " " +
                               (p2.Y - RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet)).ToString() + " " +
                               (p2.Z - RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet)).ToString()
                               );
                            }
                        }

                    }
                    
                    listP.Clear();
                    listP1.Clear();
                }
            }
            );

            }

        private async void buttonStep_Click(object sender, EventArgs e)
        {
            await DoPlanAsync(GeiDaCheX.Text);
        }
        private async Task DoPlanAsync(string loca1)
        {
            await Task.Run(() =>
            {
                int commandType;
                while (!RuChuKuForm.hostComputerCommand.WriteOneCommand(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, RuChuKuForm.hostComputerCommand.GetPList_ShishiToKuWei(RuChuKuForm.siemensTcpNet, loca1), out commandType))//103:实时过道，库位过道，库位2
                {

                }
                while (RuChuKuForm.hostComputerCommand.ReadComplete(RuChuKuForm.siemensTcpNet) != 1)
                {

                }


                //到库位1
                //RuChuKuForm.hostComputerCommand.RunPlan(RuChuKuForm.siemensTcpNet,++RuChuKuForm.counter,loca1,loca2,str);

            }
            );
        }
        #region 正则表达式
        Regex rgxKuWei = new Regex(@"^[0-9]");
        Regex rgxDingDian = new Regex(@"^[^0-9]");
        Regex rgxShangHuo = new Regex(@"[0-9]$");
        #endregion
        private void buttonWrite_Click(object sender, EventArgs e)
        {
            //if (comboBoxLocation.Text.StartsWith("上货台")|| comboBoxLocation.Text.StartsWith("精")|| comboBoxLocation.Text.StartsWith("过"))
            //{

            //}
            if (rgxKuWei.IsMatch(comboBoxLocation.Text))
            {
               
                tb_KuWeiModel.KuWeiName = comboBoxLocation.Text;
                tb_KuWeiModel.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet).ToString();
                tb_KuWeiModel.Y = RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet).ToString();
                tb_KuWeiModel.Z = RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet).ToString();
                if(tb_KuWeiBLL.UpdateRead(tb_KuWeiModel))
                {
                    MessageBox.Show("写入成功");
                }
                else
                {
                    MessageBox.Show("写入失败");
                }
                //MessageBox.Show("写入成功");
            }
            else if (rgxShangHuo.IsMatch(comboBoxLocation.Text))
            {
                tb_KuWeiModel.KuWeiName = comboBoxLocation.Text;
                tb_KuWeiModel.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet).ToString();
                tb_KuWeiModel.Y = RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet).ToString();
                tb_KuWeiModel.Z = RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet).ToString();
                //tb_KuWeiBLL.UpdateRead(tb_KuWeiModel);
                //tb_DingDianBLL.UpdateRead(tb_KuWeiModel);
                if(tb_KuWeiBLL.UpdateRead(tb_KuWeiModel)&& tb_DingDianBLL.UpdateRead(tb_KuWeiModel))
                {
                    MessageBox.Show("写入成功");
                }
                else
                {
                    MessageBox.Show("写入失败");
                }
            }
            else if(rgxDingDian.IsMatch(comboBoxLocation.Text))
            {
                tb_DingDianModel.DingDianName = comboBoxLocation.Text;
                tb_DingDianModel.X = RuChuKuForm.hostComputerCommand.ReadX(RuChuKuForm.siemensTcpNet).ToString();
                tb_DingDianModel.Y = RuChuKuForm.hostComputerCommand.ReadY(RuChuKuForm.siemensTcpNet).ToString();
                tb_DingDianModel.Z = RuChuKuForm.hostComputerCommand.ReadZ(RuChuKuForm.siemensTcpNet).ToString();
                //tb_DingDianBLL.UpdateRead(tb_DingDianModel);
                if(tb_DingDianBLL.UpdateRead(tb_DingDianModel))
                {
                    MessageBox.Show("写入成功");
                }
                else
                {
                    MessageBox.Show("写入失败");
                }
            }
            
            else
            {

            }

        }

        private void SheBeiCanShu_Load(object sender, EventArgs e)
        {
            timerParameter.Start();
            //this.Resize += SheBeiCanShu_Resize;
            //X = this.Width;
            //Y = this.Height;
            //setTag(this);
            pushClient.CreatePush(PushCallBack);                          // 创建数据订阅器
            //this.WindowState = FormWindowState.Maximized;
            //threadYD = new Thread(new System.Threading.ThreadStart(DoYD));//开启连续运动线程
            //threadYD.IsBackground = true;
            ////Thread threadYD = new Thread(DoYD);//开启连续运动线程
            //threadYD.Start();
            NetComplexInitialization();

            //hslCurve1.SetLeftCurve("温度", new float[0], Color.LimeGreen);   // 新增一条实时曲线
            //hslCurve1.AddLeftAuxiliary(100, Color.Tomato);                // 新增一条100度的辅助线

            
        }

        private void SheBeiCanShu_Resize(object sender, EventArgs e)
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
        bool falg = true;
        //public  async Task  timerCanShuTick(object sender)
        public async Task timerCanShuTick()
        {
            await Task.Run(() =>
            {
                try
                {
                    buffWrite = RuChuKuForm.siemensTcpNet.Read("DB100.00", 80);//读取PLC反馈的数据
                    buffRead = RuChuKuForm.siemensTcpNet.Read("DB101.00", 36);//读取PLC反馈的数据
                    buffReadXinTiao = RuChuKuForm.siemensTcpNet.Read("DB102.00", 1);//读取PLC反馈的数据
                    buffReadShangDian = RuChuKuForm.siemensTcpNet.Read("DB104.00", 12);//读取PLC反馈的数据
                    buffReadState = RuChuKuForm.siemensTcpNet.Read("DB103.00", 3);//读取PLC反馈的故障状态
                    if (buffRead.IsSuccess)
                    {
                        //SetText(DaCheX, buffRead.Content[4].ToString());
                        SetText(DaCheX, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 4).ToString());
                        SetText(XiaoCheY, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 8).ToString());
                        SetText(QiShengZ, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 12).ToString());
                        SetText(HuoChaWeiZhi, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 16).ToString());
                        SetText(JiaJu, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 20).ToString());
                        SetText(SuDuX, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 24).ToString());
                        SetText(SuDuY, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 28).ToString());
                        SetText(SuDuZ, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 32).ToString());


                    }
                    if (buffReadShangDian.IsSuccess)//读取实际的上电状态
                    {
                        SetText(ShangDian, RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffReadShangDian.Content, 8).ToString());

                    }
                    if (buffReadState.IsSuccess)
                    {
                        if ((0x40 & buffReadState.Content[0]) != 0)//复位
                        {
                            SetBackgroundColor(fuWei, Color.Red);
                        }
                        else
                        {
                            SetBackgroundColor(fuWei, Color.White);
                        }
                        if ((0x8 & buffReadState.Content[1]) != 0)//起升变频器故障
                        {
                            SetBackgroundColor(qsBPQGZ, Color.Red);
                        }
                        else
                        {
                            SetBackgroundColor(qsBPQGZ, Color.White);
                        }
                        if ((0x10 & buffReadState.Content[1]) != 0)//小车变频器故障
                        {
                            SetBackgroundColor(xcBPQGZ, Color.Red);
                        }
                        else
                        {
                            SetBackgroundColor(xcBPQGZ, Color.White);
                        }
                        if ((0x20 & buffReadState.Content[1]) != 0)//大车变频器故障
                        {
                            SetBackgroundColor(dcBPQGZ, Color.Red);
                        }
                        else
                        {
                            SetBackgroundColor(dcBPQGZ, Color.White);
                        }
                        if ((0x40 & buffReadState.Content[1]) != 0)//货叉变频器故障
                        {
                            SetBackgroundColor(hcBPQGZ, Color.Red);
                        }
                        else
                        {
                            SetBackgroundColor(hcBPQGZ, Color.White);
                        }
                        if ((0x40 & buffReadState.Content[2]) != 0)//手动模式
                        {
                            SetText(zdMS, "手动模式");

                            SetBackgroundColor(zdMS, Color.Red);
                            if (falg)
                            {
                                falg = false;
                                RuChuKuForm.hostComputerCommand.WriteResetCommand(RuChuKuForm.siemensTcpNet);
                                
                            }
                            //RuChuKuForm.hostComputerCommand.WriteResetCommand(RuChuKuForm.siemensTcpNet);
                        }
                        else
                        {
                            SetText(zdMS, "自动模式");
                            falg = true;
                            SetBackgroundColor(zdMS, Color.White);
                        }

                    }
                    if (buffReadXinTiao.IsSuccess)
                    {
                        SetText(xinTiao, buffReadXinTiao.Content[0].ToString());
                    }

                }
                catch (Exception e)
                {

                    sheBeiCanShuLogNet.WriteException("timerCanShuTick", e);
                }
            });


        }
        //public  int Sum(object i)
        //{
        //    var sum = 0;
        //    for (var j = 0; j <= (int)i; j++)
        //    {
        //        Console.Write("{0} + ", sum);
        //        sum += j;
        //    }
        //    Console.WriteLine(" = {0}", sum);
        //    return sum;
        //}

        //public  async void ExecuteAsync()
        //{
        //    string text = await DownloadStringWithRetries("http://wwww.baidu.com");
        //    //Console.WriteLine(text);
        //    sheBeiCanShuLogNet.WriteInfo(text);
        //}

        private void CanShuWrite_Click(object sender, EventArgs e)
        {


            string Info = "写入1类指令";
            zhiLingHao++;
            RuChuKuForm.siemensTcpNet.Write("DB100.00", ++(RuChuKuForm.counter));//写入指令号

            if (string.IsNullOrEmpty(GeiJiaJu.Text))
            {
                if (string.IsNullOrEmpty(GeiDaCheX.Text) || string.IsNullOrEmpty(GeiXiaoCheY.Text) || string.IsNullOrEmpty(GeiQiShengZ.Text))
                {
                    Info = "所写数据为空";
                    this.BeginInvoke(new Action(() =>
                    {
                        RuChuKuForm.statusLabel.Text = "所写数据为空。。。";
                    }));
                    //SetText(RuChuKuForm.statusLabel, Info);
                    return;
                }
                else
                {
                    zhilingLeiXing = 101;
                    X1 = int.Parse(GeiDaCheX.Text);
                    Y1 = int.Parse(GeiXiaoCheY.Text);
                    Z1 = int.Parse(GeiQiShengZ.Text);
                    if (RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.08", X1).IsSuccess
                        && RuChuKuForm.siemensTcpNet.Write("DB100.12", Y1).IsSuccess && RuChuKuForm.siemensTcpNet.Write("DB100.16", Z1).IsSuccess
                        )
                    {

                        sheBeiCanShuLogNet.WriteInfo(sender.ToString(), "");
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = Info;
                        }));
                        //SetText(RuChuKuForm.statusLabel, Info);
                        //while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0)!=1)
                        //{
                        //    Info = "到达目标位置";
                        //    SetText(RuChuKuForm.statusLabel, Info);
                        //}
                        //sheBeiCanShuLogNet.WriteInfo("开始101");
                        //Task<int> task = new Task<int>(Sum, 100);
                        //task.Start();
                        //task.Wait();
                        ////ExecuteAsyncOne();
                        //sheBeiCanShuLogNet.WriteInfo("结束101");
                        return;
                    }
                    else
                    {
                        Info = "写入1类指令失败";
                        this.BeginInvoke(new Action(() =>
                        {
                            RuChuKuForm.statusLabel.Text = Info;
                        }));
                        //SetText(RuChuKuForm.statusLabel, Info);
                        return;
                    }

                    //RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing);//写入指令类型

                    //RuChuKuForm.siemensTcpNet.Write("DB100.08", X1);//写入X

                    //RuChuKuForm.siemensTcpNet.Write("DB100.12", Y1);//写入Y

                    //RuChuKuForm.siemensTcpNet.Write("DB100.16", Z1);//写入Z
                    //                                                //toolStripStatusLabelState.Text = Info;

                }

            }
            else
            {
                Info = "写入3类指令";
                zhilingLeiXing = 301;

                RuChuKuForm.siemensTcpNet.Write("DB100.04", zhilingLeiXing);//写入指令类型
                huoChaCaoZuo = int.Parse(GeiJiaJu.Text);
                RuChuKuForm.siemensTcpNet.Write("DB100.60", huoChaCaoZuo);//写入指令类型
                this.BeginInvoke(new Action(() =>
                {
                    RuChuKuForm.statusLabel.Text = Info;
                }));
                //SetText(RuChuKuForm.statusLabel, Info);
                return;
            }


        }
        public  int Sum(object i)
        {
            //while (RuChuKuForm.siemensTcpNet.ByteTransform.TransInt32(buffRead.Content, 0) != 1)
            //{
            //    sheBeiCanShuLogNet.WriteInfo("运行。。。");

            //}
            //SetText(RuChuKuForm.statusLabel, "到达目标位置");
            return 1;
        }
        /// <summary>
        /// 执行单点运动，异步等待PLC返回结果
        /// </summary>
        public async void ExecuteAsyncOne()
        {
            await DoWorkOne("http://wwww.baidu.com");

        }
        private async Task<string> DoWorkOne1(string uri)
        {
            using (var client = new HttpClient())
            {
                // 第1 次重试前等1 秒，第2 次等2 秒，第3 次等4 秒。
                var nextDelay = TimeSpan.FromSeconds(1);
                for (int i = 0; i != 3; ++i)
                {
                    try
                    {
                        return await client.GetStringAsync(uri);
                    }
                    catch
                    {
                    }
                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }
                // 最后重试一次，以便让调用者知道出错信息。
                return await client.GetStringAsync(uri);
            }
        }
        private async Task<string> DoWorkOne(string uri)
        {
            using (var client = new HttpClient())
            {
                // 第1 次重试前等1 秒，第2 次等2 秒，第3 次等4 秒。
                var nextDelay = TimeSpan.FromSeconds(1);
                for (int i = 0; i != 3; ++i)
                {
                    try
                    {
                        return await client.GetStringAsync(uri);
                    }
                    catch
                    {
                    }
                    await Task.Delay(nextDelay);
                    nextDelay = nextDelay + nextDelay;
                }
                // 最后重试一次，以便让调用者知道出错信息。
                return await client.GetStringAsync(uri);
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
                ShowHistory(buffer);

                isClientIni = true;
            }
        }


        #endregion

        #region 显示结果

        // 接收到服务器传送过来的数据后需要对数据进行解析显示
        private void ShowReadContent(JObject content)
        {
            if (InvokeRequired && !IsDisposed)
            {
                try
                {
                    Invoke(new Action<JObject>(ShowReadContent), content);
                }
                catch
                {

                }
                return;
            }

            double temp1 = content["temp"].ToObject<double>();           // 温度
            bool machineEnable = content["enable"].ToObject<bool>();     // 设备使能
            int product = content["product"].ToObject<int>();            // 产量数据


            //hslLedDisplay1.DisplayText = temp1.ToString();

            //// 如果温度超100℃就把背景改为红色
            //hslLedDisplay1.ForeColor = temp1 > 100d ? Color.Tomato : Color.Lime;
            //label3.Text = product.ToString();

            //label5.Text = machineEnable ? "运行中" : "未启动";

            //// 添加仪表盘显示
            //hslGauge1.Value = (float)Math.Round(temp1, 1);

            //// 添加温度控件显示
            //hslThermometer1.Value = (float)Math.Round(temp1, 1);

            //// 添加实时的数据曲线
            //hslCurve1.AddCurveData("温度", (float)temp1);
        }


        private void ShowHistory(byte[] content)
        {
            if (InvokeRequired && !IsDisposed)
            {
                Invoke(new Action<byte[]>(ShowHistory), content);
                return;
            }

            float[] value = new float[content.Length / 4];
            for (int i = 0; i < value.Length; i++)
            {
                value[i] = BitConverter.ToSingle(content, i * 4);
            }

            //hslCurve1.AddCurveData("温度", value);

        }


        #endregion

        #region Simplify Client

        private NetSimplifyClient simplifyClient = new NetSimplifyClient("192.168.0.7", 23457);

        #endregion

        #region Push NetClient

        private NetPushClient pushClient = new NetPushClient("127.0.0.1", 23467, "A");                          // 数据订阅器，负责订阅主要的数据

        private void PushCallBack(NetPushClient pushClient, string value)
        {
            JObject content = JObject.Parse(value);

            if (isClientIni)
            {
                ShowReadContent(content);
            }
        }

        #endregion

    }
}
