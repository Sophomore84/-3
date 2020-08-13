using ClassLibrary_Crane;
using HslCommunication;
using HslCommunication.LogNet;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using 自动化库存管理.Command;

namespace SocketServer
{
    public class SynchronousSocketListener
    {

        // Incoming data from the client.  
        public static string data = null;

        public static void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and
            // listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start listening for connections.  
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");
                    // Program is suspended while waiting for an incoming connection.  
                    Socket handler = listener.Accept();
                    data = null;

                    // An incoming connection needs to be processed.  
                    //while (true)
                    if (true)
                    
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        //if (data.IndexOf("<EOF>") > -1)
                        //{
                        //    break;
                        //}
                        switch (data)
                        {
                            case "301":
                                Console.WriteLine("开始301");
                            //    if (hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNet, ++counter, 301, 1))
                            //    {
                            //        Console.WriteLine("写入301");
                            //    }
                            //    while (hostComputerCommand.ReadComplete(siemensTcpNet) != 3)
                            //    {
                            //        Console.WriteLine("等待301完成");
                            //    }
                            //    Console.WriteLine("301完成");
                            //    data = "3";
                            //    break;
                            //case "302":
                            //    Console.WriteLine("开始302");
                            //    if (hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNet, ++counter, 301, 2))
                            //    {
                            //        Console.WriteLine("写入302");
                            //    }
                            //    while (hostComputerCommand.ReadComplete(siemensTcpNet) != 3)
                            //    {
                            //        Console.WriteLine("等待302完成");
                            //    }
                            //    Console.WriteLine("302完成");
                                data = "3";
                                break;
                            case "303":
                                Console.WriteLine("开始303");

                                //if (hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNet, ++counter, 301, 3))
                                //{
                                //    Console.WriteLine("写入303");
                                //}
                                //while (hostComputerCommand.ReadComplete(siemensTcpNet) != 3)
                                //{
                                //    Console.WriteLine("等待303完成");
                                //}
                                //Console.WriteLine("303完成");
                                data = "3";
                                break;
                            default:
                                Console.WriteLine("开始默认");
                                // All the data has been read from the
                                // client. Display it on the console.  
                                data = "0";
                                // Echo the data back to the client.  

                                break;
                        }
                    }
                    //}
                    // Show the data on the console.  
                    Console.WriteLine("Text received : {0}", data);

                    // Echo the data back to the client.  
                    byte[] msg = Encoding.ASCII.GetBytes(data);

                    handler.Send(msg);
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
        #region PLC配置
        public static SiemensS7Net_New siemensTcpNet;//创建PLC连接
        public static bool longConnection = false;
        public static string IP = "";
        public static int counter = 0;//指令号
        private static void InitPLC()
        {
            if (IP == "")
                IP = "192.168.0.1";
            siemensTcpNet = new SiemensS7Net_New(SiemensPLCS.S1500, IP) { ConnectTimeOut = 1000 };
            ConnectPLC();
        }
        private static bool ConnectPLC()
        {
            OperateResult connect = siemensTcpNet.ConnectServer();
            if (connect.IsSuccess)
            {
                longConnection = true;

                return true;
            }
            else
            {
                DisconnectPLC();

                longConnection = false;


                return false;
            }
        }

        private static void DisconnectPLC()
        {
            if (siemensTcpNet != null)
                siemensTcpNet.ConnectClose();
            //MessageBox.Show(siemensTcpNet.ConnectClose().IsSuccess.ToString());
            longConnection = false;
        }
        #endregion
        #region 日志
        /// <summary>
        /// 异步的服务端日志记录器
        /// </summary>
        private static ILogNet LogNet { get; set; }
        #endregion


        public static int Main1(String[] args)
        {
            LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\服务端", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            InitPLC();
            StartListening();
            return 0;
        }
    }
}
