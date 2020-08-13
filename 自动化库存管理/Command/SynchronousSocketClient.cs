using HslCommunication.LogNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClient
{
    public class SynchronousSocketClient
    {
        /// <summary>
        /// 异步的客户端日志记录器
        /// </summary>
        private static ILogNet LogNet { get; set; }
        //public static void StartClient()
        //{
        //    // Data buffer for incoming data.  
        //    byte[] bytes = new byte[1024];

        //    // Connect to a remote device.  
        //    try
        //    {
        //        // Establish the remote endpoint for the socket.  
        //        // This example uses port 11000 on the local computer.  
        //        //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        //        IPHostEntry ipHostInfo = Dns.GetHostEntry("DESKTOP-4JE1M8G");
        //        IPAddress ipAddress = ipHostInfo.AddressList[0];
        //        IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

        //        // Create a TCP/IP  socket.  
        //        Socket sender = new Socket(ipAddress.AddressFamily,
        //            SocketType.Stream, ProtocolType.Tcp);

        //        // Connect the socket to the remote endpoint. Catch any errors.  
        //        try
        //        {
        //            sender.Connect(remoteEP);

        //            Console.WriteLine("Socket connected to {0}",
        //                sender.RemoteEndPoint.ToString());

        //            // Encode the data string into a byte array.  
        //            byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");

        //            // Send the data through the socket.  
        //            int bytesSent = sender.Send(msg);

        //            // Receive the response from the remote device.  
        //            int bytesRec = sender.Receive(bytes);
        //            Console.WriteLine("Echoed test = {0}",
        //                Encoding.ASCII.GetString(bytes, 0, bytesRec));

        //            // Release the socket.  
        //            sender.Shutdown(SocketShutdown.Both);
        //            sender.Close();

        //        }
        //        catch (ArgumentNullException ane)
        //        {
        //            Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
        //        }
        //        catch (SocketException se)
        //        {
        //            Console.WriteLine("SocketException : {0}", se.ToString());
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine("Unexpected exception : {0}", e.ToString());
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.ToString());
        //    }
        //}
        public  async Task  StartClient(string data)
        {
            await Task.Run(() =>
            {
                // Data buffer for incoming data.  
                byte[] bytes = new byte[1024];

                // Connect to a remote device.  
                try
                {
                    // Establish the remote endpoint for the socket.  
                    // This example uses port 11000 on the local computer.  
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                    //IPHostEntry ipHostInfo = Dns.GetHostEntry("DESKTOP-4JE1M8G");
                    IPAddress ipAddress = ipHostInfo.AddressList[0];
                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                    // Create a TCP/IP  socket.  
                    Socket sender = new Socket(ipAddress.AddressFamily,
                        SocketType.Stream, ProtocolType.Tcp);

                    // Connect the socket to the remote endpoint. Catch any errors.  
                    try
                    {
                        sender.Connect(remoteEP);

                        //Console.WriteLine("Socket connected to {0}",
                            //sender.RemoteEndPoint.ToString());
                        LogNet.WriteInfo("Socket connected to "+ sender.RemoteEndPoint.ToString());
                        // Encode the data string into a byte array.  
                        //byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
                        byte[] msg = Encoding.ASCII.GetBytes(data);
                        // Send the data through the socket.  
                        int bytesSent = sender.Send(msg);

                        // Receive the response from the remote device.  
                        int bytesRec = sender.Receive(bytes);
                        //Console.WriteLine("Echoed test = {0}",
                            //Encoding.ASCII.GetString(bytes, 0, bytesRec));
                        LogNet.WriteInfo("Echoed test = " + Encoding.ASCII.GetString(bytes, 0, bytesRec));
                        // Release the socket.  
                        sender.Shutdown(SocketShutdown.Both);
                        sender.Close();

                    }
                    catch (ArgumentNullException ane)
                    {
                        LogNet.WriteInfo("ArgumentNullException :  " + ane.ToString());
                        //Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    }
                    catch (SocketException se)
                    {
                        LogNet.WriteInfo("SocketException :  " + se.ToString());
                        //Console.WriteLine("SocketException : {0}", se.ToString());
                    }
                    catch (Exception e)
                    {
                        LogNet.WriteInfo("Unexpected exception  :  " + e.ToString());
                        //Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    }

                }
                catch (Exception e)
                {
                    LogNet.WriteInfo( e.ToString());
                    //Console.WriteLine(e.ToString());
                }
            });
            
        }
        public SynchronousSocketClient()
        {
            LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\客户端", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
        }
        //public static int Main(String[] args)
        //{
        //    while (true)
        //    {
        //        StartClient();
        //        Thread.Sleep(3000);
        //    }
        //    //StartClient();
        //    //return 0;
        //}
    }
}
