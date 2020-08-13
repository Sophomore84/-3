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
using System.Threading;
using System.Threading.Tasks;
using 自动化库存管理.Command;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SocketServer
{
    // State object for reading client data asynchronously  
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class AsynchronousSocketListener
    {
        #region PLC运行指令
     public static   HostComputerCommand hostComputerCommand ;
        #endregion
        #region 日志
        /// <summary>
        /// 异步的服务端日志记录器
        /// </summary>
        private static ILogNet LogNet { get; set; }
        #endregion
        #region PLC配置
        public static SiemensS7Net_New siemensTcpNet;//创建PLC连接
        public static bool longConnection = false;
        public static string IP="";
        public static int counter = 0;//指令号
        private static  void InitPLC()
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

        #region IP配置
        
        #endregion
        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public AsynchronousSocketListener()
        {
            //LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\服务端", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            //InitPLC();
        }

        public static void StartListening()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPAddress ipAddress = IPAddress.Parse("192.168.0.195");
            //IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 502);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);
            //IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 51000);
            // Create a TCP/IP socket.  
            Socket listener = new Socket(ipAddress.AddressFamily,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            try
            {
                String content = String.Empty;

                // Retrieve the state object and the handler socket  
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket handler = state.workSocket;

                // Read data from the client socket.
                int bytesRead = handler.EndReceive(ar);

                if (bytesRead > 0)
                {
                    // There  might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(
                        state.buffer, 0, bytesRead));

                    // Check for end-of-file tag. If it is not there, read
                    // more data.  
                    content = state.sb.ToString();
                    //if (content.IndexOf("<EOF>") > -1)
                    //{
                    //    // All the data has been read from the
                    //    // client. Display it on the console.  
                    //    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    //        content.Length, content);
                    //    // Echo the data back to the client.  
                    //    Send(handler, content);
                    //}
                    if (!string.IsNullOrEmpty(content))
                    {

                        // All the data has been read from the
                        // client. Display it on the console.  
                        Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                            content.Length, content);
                        // Echo the data back to the client.  
                        //Send(handler, content);
                        //对接收到的客户端数据进行处理
                        switch (content)
                        {
                            case "301":
                                Console.WriteLine("开始301");
                                while (!hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNet, ++counter, 301, 1))
                                {
                                    Console.WriteLine("写入301");
                                }
                                while (hostComputerCommand.ReadComplete(siemensTcpNet) != 3)
                                {
                                    Console.WriteLine("等待301完成");
                                }
                                Console.WriteLine("301完成");
                                //Send(handler, "3");
                                break;
                            case "302":
                                Console.WriteLine("开始302");
                                while (!hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNet, ++counter, 301, 2))
                                {
                                    Console.WriteLine("写入302");
                                }
                                while (hostComputerCommand.ReadComplete(siemensTcpNet) != 3)
                                {
                                    Console.WriteLine("等待302完成");
                                }
                                Console.WriteLine("302完成");
                                //Send(handler, "3");
                                break;
                            case "303":
                                Console.WriteLine("开始303");

                                while (!hostComputerCommand.WriteThreeZeroOneCommand(siemensTcpNet, ++counter, 301, 3))
                                {
                                    Console.WriteLine("写入303");
                                }
                                while (hostComputerCommand.ReadComplete(siemensTcpNet) != 3)
                                {
                                    Console.WriteLine("等待303完成");
                                }
                                Console.WriteLine("303完成");
                                //Send(handler, "3");
                                break;
                            case "启动":
                                Console.WriteLine("开始启动");
                                while (!hostComputerCommand.WriteOpenCommand(siemensTcpNet))
                                {
                                    Console.WriteLine("写入启动成功");
                                }
                                break;
                            case "停止":
                                Console.WriteLine("开始停止");
                                while (!hostComputerCommand.WriteShutCommand(siemensTcpNet))
                                {
                                    Console.WriteLine("写入停止成功");
                                }
                                break;
                            case "急停":
                                Console.WriteLine("开始急停");
                                while (!hostComputerCommand.WriteStopCommand(siemensTcpNet))
                                {
                                    Console.WriteLine("写入急停成功");
                                }
                                break;
                            case "复位":
                                Console.WriteLine("开始复位 ");
                                while (!hostComputerCommand.WriteResetCommand(siemensTcpNet))
                                {
                                    Console.WriteLine("写入复位成功");
                                }
                                break;
                            default:
                                //Console.WriteLine("开始默认");
                                // All the data has been read from the
                                // client. Display it on the console.  
                                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                                    content.Length, content);
                                LogNet.WriteInfo(content);
                                content = byteToHexStr(state.buffer);
                                content = content + "\r\n";
                                string Str = Regex.Replace(content, @"(?<=[0-9A-F]{2})[0-9A-F]{2}", " $0");
                                MessageBox.Show(Str);
                                // Echo the data back to the client.  
                                //Send(handler, content);
                                break;
                        }
                        Send(handler, content);

                    }
                    else
                    {
                        // Not all data received. Get more.  
                        handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReadCallback), state);
                    }
                }
            }
            catch (Exception e)
            {

                LogNet.WriteException("ReadCallback", e);
            }
            
        }
        //字节数组转16进制字符串
        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public static int Main(String[] args)
        {
            LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\服务端", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
            hostComputerCommand = new HostComputerCommand();
            InitPLC();
            StartListening();
            return 0;
        }
    }
}
