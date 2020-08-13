using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1.Command;

namespace ConsoleApp1
{
    [Flags]
    public enum Days
    {
        None = 0b_0000_0000,  // 0
        Monday = 0b_0000_0001,  // 1
        Tuesday = 0b_0000_0010,  // 2
        Wednesday = 0b_0000_0100,  // 4
        Thursday = 0b_0000_1000,  // 8
        Friday = 0b_0001_0000,  // 16
        Saturday = 0b_0010_0000,  // 32
        Sunday = 0b_0100_0000,  // 64
        Weekend = Saturday | Sunday
    }
    class Program
    {
        static Thread thread1, thread2;
        private static Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;

            // Get host related information.
            hostEntry = Dns.GetHostEntry(server);

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
            // an exception that occurs when the host IP Address is not compatible with the address family
            // (typical in the IPv6 case).
            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket =
                    new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return s;
        }

        // This method requests the home page content for the specified server.
        private static string SocketSendReceive(string server, int port)
        {
            string request = "GET / HTTP/1.1\r\nHost: " + server +
                "\r\nConnection: Close\r\n\r\n";
            Byte[] bytesSent = Encoding.ASCII.GetBytes(request);
            Byte[] bytesReceived = new Byte[256];
            string page = "";

            // Create a socket connection with the specified server and port.
            using (Socket s = ConnectSocket(server, port))
            {

                if (s == null)
                    return ("Connection failed");

                // Send request to the server.
                s.Send(bytesSent, bytesSent.Length, 0);

                // Receive the server home page content.
                int bytes = 0;
                page = "Default HTML page on " + server + ":\r\n";

                // The following will block until the page is transmitted.
                do
                {
                    bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                    page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                }
                while (bytes > 0);
            }

            return page;
        }
        #region AutoResetEvent
        private static AutoResetEvent event_1 = new AutoResetEvent(true);
        private static AutoResetEvent event_2 = new AutoResetEvent(false);

        static void Main8()
        {
            Console.WriteLine("Press Enter to create three threads and start them.\r\n" +
                              "The threads wait on AutoResetEvent #1, which was created\r\n" +
                              "in the signaled state, so the first thread is released.\r\n" +
                              "This puts AutoResetEvent #1 into the unsignaled state.");
            Console.ReadLine();

            for (int i = 1; i < 4; i++)
            {
                Thread t = new Thread(ThreadProc);
                t.Name = "Thread_" + i;
                t.Start();
            }
            Thread.Sleep(250);

            for (int i = 0; i < 2; i++)
            {
                Console.WriteLine("Press Enter to release another thread.");
                Console.ReadLine();
                event_1.Set();
                Thread.Sleep(250);
            }

            Console.WriteLine("\r\nAll threads are now waiting on AutoResetEvent #2.");
            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine("Press Enter to release a thread.");
                Console.ReadLine();
                event_2.Set();
                Thread.Sleep(250);
            }

            // Visual Studio: Uncomment the following line.
            //Console.Readline();
        }

        static void ThreadProc()
        {
            string name = Thread.CurrentThread.Name;

            Console.WriteLine("{0} waits on AutoResetEvent #1.", name);
            event_1.WaitOne();
            Console.WriteLine("{0} is released from AutoResetEvent #1.", name);

            Console.WriteLine("{0} waits on AutoResetEvent #2.", name);
            event_2.WaitOne();
            Console.WriteLine("{0} is released from AutoResetEvent #2.", name);

            Console.WriteLine("{0} ends.", name);
        }
        #endregion
        #region crc校验
        public static void Main31()
        {
            //string msg = "01 2C 00 00 00 00 00 00 0A 4F 4F 05 93 00 07 06 15 00 00 00 23 3D F5 7C 45 71 20 00 45 71 20 00 00 00 00 00 40 DF 70 E3 00 00 00 00 BF 8F D6 63 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3D F9 00 45 71 20 00 45 71 20 00 00 00 00 00 00 00 00 00 00 00 00 00 BF 8E 47 99 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3D FC 84 45 71 20 00 45 71 20 00 00 00 00 00 00 00 00 00 00 00 00 00 BF 8A FD 76 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3E 00 08 45 71 20 00 45 71 20 00 00 00 00 00 00 00 00 00 00 00 00 00 BF 92 C7 B9 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3E 03 8C 45 71 20 00 45 71 20 00 00 00 00 00 00 00 00 00 00 00 00 00 BF 8A A4 FD 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3E 07 10 45 71 20 00 45 71 20 00 00 00 00 00 00 00 00 00 00 00 00 00 BF 8D EE CC 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3E 0A 94 45 71 20 00 45 71 20 00 00 00 00 00 00 00 00 00 00 00 00 00 BF 88 38 B0 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3E 0E 18 45 71 40 00 45 71 50 00 00 00 00 00 41 1F D4 4B 00 00 00 00 BF 7C 04 97 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3E 11 9C 45 71 70 00 45 71 70 00 00 00 00 00 41 15 7B 74 00 00 00 00 BF 8E A0 12 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00 23 3E 15 20 45 71 90 00 45 71 90 00 00 00 00 00 40 E1 D9 FE 00 00 00 00 BF 8C E5 0D 00 00 00 00 00 00 00 00 10 20 00 00 41 30 00 00";
            string msg = "01 03 04 00 00 01 18";
            var a = CRC16Util.ToCRC16(msg, true);          //结果为：
            var b = CRC16Util.ToCRC16(msg, false);           //结果为：
            var c = CRC16Util.ToModbusCRC16(msg, true);      //结果为：
            var e = CRC16Util.ToModbusCRC16(msg, false);
            var d = CRC16Util.ToCRC16("你好，我们测试一下CRC16算法", true);   //结果为：
            Console.WriteLine(c);
            Console.WriteLine(e);
            Console.ReadKey();
        }
        #region  CRC16
        public static byte[] CRC16(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置

                return new byte[] { hi, lo };
            }
            return new byte[] { 0, 0 };
        }
        #endregion

        #region  ToCRC16
        public static string ToCRC16(string content)
        {
            return ToCRC16(content, Encoding.UTF8);
        }

        public static string ToCRC16(string content, bool isReverse)
        {
            return ToCRC16(content, Encoding.UTF8, isReverse);
        }

        public static string ToCRC16(string content, Encoding encoding)
        {
            return ByteToString(CRC16(encoding.GetBytes(content)), true);
        }

        public static string ToCRC16(string content, Encoding encoding, bool isReverse)
        {
            return ByteToString(CRC16(encoding.GetBytes(content)), isReverse);
        }

        public static string ToCRC16(byte[] data)
        {
            return ByteToString(CRC16(data), true);
        }

        public static string ToCRC16(byte[] data, bool isReverse)
        {
            return ByteToString(CRC16(data), isReverse);
        }
        #endregion

        #region  ToModbusCRC16
        public static string ToModbusCRC16(string s)
        {
            return ToModbusCRC16(s, true);
        }

        public static string ToModbusCRC16(string s, bool isReverse)
        {
            return ByteToString(CRC16(StringToHexByte(s)), isReverse);
        }

        public static string ToModbusCRC16(byte[] data)
        {
            return ToModbusCRC16(data, true);
        }

        public static string ToModbusCRC16(byte[] data, bool isReverse)
        {
            return ByteToString(CRC16(data), isReverse);
        }
        #endregion

        #region  ByteToString
        public static string ByteToString(byte[] arr, bool isReverse)
        {
            try
            {
                byte hi = arr[0], lo = arr[1];
                return Convert.ToString(isReverse ? hi + lo * 0x100 : hi * 0x100 + lo, 16).ToUpper().PadLeft(4, '0');
            }
            catch (Exception ex) { throw (ex); }
        }

        public static string ByteToString(byte[] arr)
        {
            try
            {
                return ByteToString(arr, true);
            }
            catch (Exception ex) { throw (ex); }
        }
        #endregion

        #region  StringToHexString
        public static string StringToHexString(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                s.Append(c.ToString("X4"));
            }
            return s.ToString();
        }
        #endregion

        #region  StringToHexByte
        private static string ConvertChinese(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c <= 0 || c >= 127)
                {
                    s.Append(c.ToString("X4"));
                }
                else
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }

        private static string FilterChinese(string str)
        {
            StringBuilder s = new StringBuilder();
            foreach (short c in str.ToCharArray())
            {
                if (c > 0 && c < 127)
                {
                    s.Append((char)c);
                }
            }
            return s.ToString();
        }

        /// <summary>
        /// 字符串转16进制字符数组
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public static byte[] StringToHexByte(string str)
        {
            return StringToHexByte(str, false);
        }

        /// <summary>
        /// 字符串转16进制字符数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="isFilterChinese">是否过滤掉中文字符</param>
        /// <returns></returns>
        public static byte[] StringToHexByte(string str, bool isFilterChinese)
        {
            string hex = isFilterChinese ? FilterChinese(str) : ConvertChinese(str);

            //清除所有空格
            hex = hex.Replace(" ", "");
            //若字符个数为奇数，补一个0
            hex += hex.Length % 2 != 0 ? "0" : "";

            byte[] result = new byte[hex.Length / 2];
            for (int i = 0, c = result.Length; i < c; i++)
            {
                result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return result;
        }
        #endregion
        #endregion
        #region 串口
        static bool _continue;
        static SerialPort _serialPort;
        public static void Main6()
        {
            string name;
            string message;
            StringComparer stringComparer = StringComparer.OrdinalIgnoreCase;
            Thread readThread = new Thread(Read);

            // Create a new SerialPort object with default settings.
            _serialPort = new SerialPort();

            // Allow the user to set the appropriate properties.
            _serialPort.PortName = SetPortName(_serialPort.PortName);
            _serialPort.BaudRate = SetPortBaudRate(_serialPort.BaudRate);
            _serialPort.Parity = SetPortParity(_serialPort.Parity);
            _serialPort.DataBits = SetPortDataBits(_serialPort.DataBits);
            _serialPort.StopBits = SetPortStopBits(_serialPort.StopBits);
            _serialPort.Handshake = SetPortHandshake(_serialPort.Handshake);

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            _serialPort.Open();
            _continue = true;
            readThread.Start();

            Console.Write("Name: ");
            name = Console.ReadLine();

            Console.WriteLine("Type QUIT to exit");

            while (_continue)
            {
                message = Console.ReadLine();

                if (stringComparer.Equals("quit", message))
                {
                    _continue = false;
                }
                else
                {
                    _serialPort.WriteLine(
                        String.Format("<{0}>: {1}", name, message));
                }
            }

            readThread.Join();
            _serialPort.Close();
        }

        public static void Read()
        {
            while (_continue)
            {
                try
                {
                    string message = _serialPort.ReadLine();
                    Console.WriteLine(message);
                }
                catch (TimeoutException) { }
            }
        }

        // Display Port values and prompt user to enter a port.
        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }
        // Display BaudRate values and prompt user to enter a value.
        public static int SetPortBaudRate(int defaultPortBaudRate)
        {
            string baudRate;

            Console.Write("Baud Rate(default:{0}): ", defaultPortBaudRate);
            baudRate = Console.ReadLine();

            if (baudRate == "")
            {
                baudRate = defaultPortBaudRate.ToString();
            }

            return int.Parse(baudRate);
        }

        // Display PortParity values and prompt user to enter a value.
        public static Parity SetPortParity(Parity defaultPortParity)
        {
            string parity;

            Console.WriteLine("Available Parity options:");
            foreach (string s in Enum.GetNames(typeof(Parity)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Parity value (Default: {0}):", defaultPortParity.ToString(), true);
            parity = Console.ReadLine();

            if (parity == "")
            {
                parity = defaultPortParity.ToString();
            }

            return (Parity)Enum.Parse(typeof(Parity), parity, true);
        }
        // Display DataBits values and prompt user to enter a value.
        public static int SetPortDataBits(int defaultPortDataBits)
        {
            string dataBits;

            Console.Write("Enter DataBits value (Default: {0}): ", defaultPortDataBits);
            dataBits = Console.ReadLine();

            if (dataBits == "")
            {
                dataBits = defaultPortDataBits.ToString();
            }

            return int.Parse(dataBits.ToUpperInvariant());
        }

        // Display StopBits values and prompt user to enter a value.
        public static StopBits SetPortStopBits(StopBits defaultPortStopBits)
        {
            string stopBits;

            Console.WriteLine("Available StopBits options:");
            foreach (string s in Enum.GetNames(typeof(StopBits)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter StopBits value (None is not supported and \n" +
             "raises an ArgumentOutOfRangeException. \n (Default: {0}):", defaultPortStopBits.ToString());
            stopBits = Console.ReadLine();

            if (stopBits == "")
            {
                stopBits = defaultPortStopBits.ToString();
            }

            return (StopBits)Enum.Parse(typeof(StopBits), stopBits, true);
        }
        public static Handshake SetPortHandshake(Handshake defaultPortHandshake)
        {
            string handshake;

            Console.WriteLine("Available Handshake options:");
            foreach (string s in Enum.GetNames(typeof(Handshake)))
            {
                Console.WriteLine("   {0}", s);
            }

            Console.Write("Enter Handshake value (Default: {0}):", defaultPortHandshake.ToString());
            handshake = Console.ReadLine();

            if (handshake == "")
            {
                handshake = defaultPortHandshake.ToString();
            }

            return (Handshake)Enum.Parse(typeof(Handshake), handshake, true);
        }
        #endregion
        #region 串口数据接收
        public static void Main5()
        {
            SerialPort mySerialPort = new SerialPort(SerialPort.GetPortNames()[0]);

            mySerialPort.BaudRate = 9600;
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.RtsEnable = true;

            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);

            mySerialPort.Open();

            Console.WriteLine("Press any key to continue...");
            Console.WriteLine();
            Console.ReadKey();
            mySerialPort.Close();
        }

        private static void DataReceivedHandler(
                            object sender,
                            SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            Console.WriteLine("Data Received:");
            Console.Write(indata);
        }
        #endregion
        #region enum
        enum Days { Saturday, Sunday, Monday, Tuesday, Wednesday, Thursday, Friday };
        enum BoilingPoints { Celsius = 100, Fahrenheit = 212 };
        [Flags]
        enum Colors { Red = 1, Green = 2, Blue = 4, Yellow = 8 };

        public static void Main7()
        {
            Console.WriteLine("{0,-11}= {1}", "d", "d", "d");
            Type weekdays = typeof(Days);
            Type boiling = typeof(BoilingPoints);

            Console.WriteLine("The days of the week, and their corresponding values in the Days Enum are:");

            foreach (string s in Enum.GetNames(weekdays))
                Console.WriteLine("{0,-11}= {1}", s, Enum.Format(weekdays, Enum.Parse(weekdays, s), "d"));

            Console.WriteLine();
            Console.WriteLine("Enums can also be created which have values that represent some meaningful amount.");
            Console.WriteLine("The BoilingPoints Enum defines the following items, and corresponding values:");

            foreach (string s in Enum.GetNames(boiling))
                Console.WriteLine("{0,-11}= {1}", s, Enum.Format(boiling, Enum.Parse(boiling, s), "d"));

            Colors myColors = Colors.Red | Colors.Blue | Colors.Yellow;
            Console.WriteLine();
            Console.WriteLine("myColors holds a combination of colors. Namely: {0}", myColors);
            Console.ReadKey();
        }
        #endregion
        #region 链表
        public static void Main9()
        {
            // Create the link list.
            string[] words =
                { "the", "fox", "jumps", "over", "the", "dog" };
            LinkedList<string> sentence = new LinkedList<string>(words);
            Display(sentence, "The linked list values:");
            Console.WriteLine("sentence.Contains(\"jumps\") = {0}",
                sentence.Contains("jumps"));

            // Add the word 'today' to the beginning of the linked list.
            sentence.AddFirst("today");
            Display(sentence, "Test 1: Add 'today' to beginning of the list:");

            // Move the first node to be the last node.
            LinkedListNode<string> mark1 = sentence.First;
            sentence.RemoveFirst();
            sentence.AddLast(mark1);
            Display(sentence, "Test 2: Move first node to be last node:");

            // Change the last node to 'yesterday'.
            sentence.RemoveLast();
            sentence.AddLast("yesterday");
            Display(sentence, "Test 3: Change the last node to 'yesterday':");

            // Move the last node to be the first node.
            mark1 = sentence.Last;
            sentence.RemoveLast();
            sentence.AddFirst(mark1);
            Display(sentence, "Test 4: Move last node to be first node:");

            // Indicate the last occurence of 'the'.
            sentence.RemoveFirst();
            LinkedListNode<string> current = sentence.FindLast("the");
            IndicateNode(current, "Test 5: Indicate last occurence of 'the':");

            // Add 'lazy' and 'old' after 'the' (the LinkedListNode named current).
            sentence.AddAfter(current, "old");
            sentence.AddAfter(current, "lazy");
            IndicateNode(current, "Test 6: Add 'lazy' and 'old' after 'the':");

            // Indicate 'fox' node.
            current = sentence.Find("fox");
            IndicateNode(current, "Test 7: Indicate the 'fox' node:");

            // Add 'quick' and 'brown' before 'fox':
            sentence.AddBefore(current, "quick");
            sentence.AddBefore(current, "brown");
            IndicateNode(current, "Test 8: Add 'quick' and 'brown' before 'fox':");

            // Keep a reference to the current node, 'fox',
            // and to the previous node in the list. Indicate the 'dog' node.
            mark1 = current;
            LinkedListNode<string> mark2 = current.Previous;
            current = sentence.Find("dog");
            IndicateNode(current, "Test 9: Indicate the 'dog' node:");

            // The AddBefore method throws an InvalidOperationException
            // if you try to add a node that already belongs to a list.
            Console.WriteLine("Test 10: Throw exception by adding node (fox) already in the list:");
            try
            {
                sentence.AddBefore(current, mark1);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Exception message: {0}", ex.Message);
            }
            Console.WriteLine();

            // Remove the node referred to by mark1, and then add it
            // before the node referred to by current.
            // Indicate the node referred to by current.
            sentence.Remove(mark1);
            sentence.AddBefore(current, mark1);
            IndicateNode(current, "Test 11: Move a referenced node (fox) before the current node (dog):");

            // Remove the node referred to by current.
            sentence.Remove(current);
            IndicateNode(current, "Test 12: Remove current node (dog) and attempt to indicate it:");

            // Add the node after the node referred to by mark2.
            sentence.AddAfter(mark2, current);
            IndicateNode(current, "Test 13: Add node removed in test 11 after a referenced node (brown):");

            // The Remove method finds and removes the
            // first node that that has the specified value.
            sentence.Remove("old");
            Display(sentence, "Test 14: Remove node that has the value 'old':");


            // When the linked list is cast to ICollection(Of String),
            // the Add method adds a node to the end of the list.
            sentence.RemoveLast();
            ICollection<string> icoll = sentence;
            icoll.Add("rhinoceros");
            Display(sentence, "Test 15: Remove last node, cast to ICollection, and add 'rhinoceros':");

            Console.WriteLine("Test 16: Copy the list to an array:");
            // Create an array with the same number of
            // elements as the inked list.
            string[] sArray = new string[sentence.Count];
            sentence.CopyTo(sArray, 0);

            foreach (string s in sArray)
            {
                Console.WriteLine(s);
            }

            // Release all the nodes.
            sentence.Clear();

            Console.WriteLine();
            Console.WriteLine("Test 17: Clear linked list. Contains 'jumps' = {0}",
                sentence.Contains("jumps"));

            Console.ReadLine();
        }

        private static void Display(LinkedList<string> words, string test)
        {
            Console.WriteLine(test);
            foreach (string word in words)
            {
                Console.Write(word + " ");
            }
            Console.WriteLine();
            Console.WriteLine();
        }

        private static void IndicateNode(LinkedListNode<string> node, string test)
        {
            Console.WriteLine(test);
            if (node.List == null)
            {
                Console.WriteLine("Node '{0}' is not in the list.\n",
                    node.Value);
                return;
            }

            StringBuilder result = new StringBuilder("(" + node.Value + ")");
            LinkedListNode<string> nodeP = node.Previous;

            while (nodeP != null)
            {
                result.Insert(0, nodeP.Value + " ");
                nodeP = nodeP.Previous;
            }

            node = node.Next;
            while (node != null)
            {
                result.Append(" " + node.Value);
                node = node.Next;
            }

            Console.WriteLine(result);
            Console.WriteLine();
        }
        #endregion
        #region EventHandler 委托
  
            static void Main11(string[] args)
            {
                Counter c = new Counter(new Random().Next(10));
                c.ThresholdReached += c_ThresholdReached;

                Console.WriteLine("press 'a' key to increase total");
                while (Console.ReadKey(true).KeyChar == 'a')
                {
                    Console.WriteLine("adding one");
                    c.Add(1);
                }
            }

            static void c_ThresholdReached(object sender, ThresholdReachedEventArgs e)
            {
                Console.WriteLine("The threshold of {0} was reached at {1}.", e.Threshold, e.TimeReached);
                Environment.Exit(0);
            }
       

        class Counter
        {
            private int threshold;
            private int total;

            public Counter(int passedThreshold)
            {
                threshold = passedThreshold;
            }

            public void Add(int x)
            {
                total += x;
                if (total >= threshold)
                {
                    ThresholdReachedEventArgs args = new ThresholdReachedEventArgs();
                    args.Threshold = threshold;
                    args.TimeReached = DateTime.Now;
                    OnThresholdReached(args);
                }
            }

            protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
            {
                EventHandler<ThresholdReachedEventArgs> handler = ThresholdReached;
                if (handler != null)
                {
                    handler(this, e);
                }
            }

            public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;
        }

        public class ThresholdReachedEventArgs : EventArgs
        {
            public int Threshold { get; set; }
            public DateTime TimeReached { get; set; }
        }

        #endregion

        #region Task
        //static void Main(string[] args)
        public static async Task Main12()
        {
           await DoTaskAsync();
            if (Console.ReadKey().Key == ConsoleKey.Enter) 
            {
                await DoTaskAsync();
            }
        }
        public static async Task DoTaskAsync()
        {
            await Task.Run(() => {
                // Just loop.
                int ctr = 0;
                for (ctr = 0; ctr <= 1000000; ctr++)
                { }
                Console.WriteLine("Finished {0} loop iterations",
                                  ctr);
                Console.ReadKey();
            });
        }
        #endregion
        #region  Wait() 
        static Random rand = new Random();

        static void Main13()
        {
            // Wait on a single task with no timeout specified.
            Task taskA = Task.Run(() => Thread.Sleep(2000));
            Console.WriteLine("taskA Status: {0}", taskA.Status);
            try
            {
                taskA.Wait();
                Console.WriteLine("taskA Status: {0}", taskA.Status);
                Console.ReadKey();
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in taskA.");
            }
        }
        #endregion
        #region TaskStatus 枚举
        public static void Main14()
        {
            
            var tasks = new List<Task<int>>();
            var source = new CancellationTokenSource();
            var token = source.Token;
            int completedIterations = 0;

            for (int n = 0; n <= 19; n++)
                tasks.Add(Task.Run(() => {
                    int iterations = 0;
                    for (int ctr = 1; ctr <= 2000000; ctr++)
                    {
                        //Console.ReadKey();
                        token.ThrowIfCancellationRequested();
                        iterations++;
                    }
                    Interlocked.Increment(ref completedIterations);
                    if (completedIterations >= 10)
                        source.Cancel();
                    return iterations;
                }, token));

            Console.WriteLine("Waiting for the first 10 tasks to complete...\n");
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException)
            {
                Console.WriteLine("Status of tasks:\n");
                Console.WriteLine("{0,10} {1,20} {2,14:N0}", "Task Id",
                                  "Status", "Iterations");
                foreach (var t in tasks)
                    Console.WriteLine("{0,10} {1,20} {2,14}",
                                      t.Id, t.Status,
                                      t.Status != TaskStatus.Canceled ? t.Result.ToString("N0") : "n/a");
            }
        }
        #endregion
         #region ConcurrentBagDemo
        static void Main16()
        {
            // Add to ConcurrentBag concurrently
            ConcurrentBag<int> cb = new ConcurrentBag<int>();
            List<Task> bagAddTasks = new List<Task>();
            for (int i = 0; i < 500; i++)
            {
                var numberToAdd = i;
                bagAddTasks.Add(Task.Run(() => cb.Add(numberToAdd)));
            }

            // Wait for all tasks to complete
            Task.WaitAll(bagAddTasks.ToArray());

            // Consume the items in the bag
            List<Task> bagConsumeTasks = new List<Task>();
            int itemsInBag = 0;
            while (!cb.IsEmpty)
            {
                bagConsumeTasks.Add(Task.Run(() =>
                {
                    int item;
                    if (cb.TryTake(out item))
                    {
                        Console.WriteLine(item);
                        itemsInBag++;
                    }
                }));
            }
            Task.WaitAll(bagConsumeTasks.ToArray());

            Console.WriteLine($"There were {itemsInBag} items in the bag");

            // Checks the bag for an item
            // The bag should be empty and this should not print anything
            int unexpectedItem;
            if (cb.TryPeek(out unexpectedItem))
                Console.WriteLine("Found an item in the bag when it should be empty");
        }
        #endregion

        #region  确定 List<T> 是否包含与指定谓词所定义的条件相匹配的元素。
        public class Part : IEquatable<Part>
        {
            public string PartName { get; set; }
            public int PartId { get; set; }

            public override string ToString()
            {
                return "ID: " + PartId + "   Name: " + PartName;
            }
            public override bool Equals(object obj)
            {
                if (obj == null) return false;
                Part objAsPart = obj as Part;
                if (objAsPart == null) return false;
                else return Equals(objAsPart);
            }
            public override int GetHashCode()
            {
                return PartId;
            }
            public bool Equals(Part other)
            {
                if (other == null) return false;
                return (this.PartId.Equals(other.PartId));
            }
            // Should also override == and != operators.
        }
        public static void Main18()
        {
            // Create a list of parts.
            List<Part> parts = new List<Part>();

            // Add parts to the list.
            parts.Add(new Part() { PartName = "crank arm", PartId = 1234 });
            parts.Add(new Part() { PartName = "chain ring", PartId = 1334 });
            parts.Add(new Part() { PartName = "regular seat", PartId = 1434 });
            parts.Add(new Part() { PartName = "banana seat", PartId = 1444 });
            parts.Add(new Part() { PartName = "cassette", PartId = 1534 });
            parts.Add(new Part() { PartName = "shift lever", PartId = 1634 }); ;

            // Write out the parts in the list. This will call the overridden ToString method
            // in the Part class.
            Console.WriteLine();
            foreach (Part aPart in parts)
            {
                Console.WriteLine(aPart);
            }

            // Check the list for part #1734. This calls the IEquatable.Equals method
            // of the Part class, which checks the PartId for equality.
            Console.WriteLine("\nContains: Part with Id=1734: {0}",
                parts.Contains(new Part { PartId = 1734, PartName = "" }));

            // Find items where name contains "seat".
            Console.WriteLine("\nFind: Part where name contains \"seat\": {0}",
                parts.Find(x => x.PartName.Contains("seat")));

            // Check if an item with Id 1444 exists.
            Console.WriteLine("\nExists: Part with Id=1444: {0}",
                parts.Exists(x => x.PartId == 1444));
            Console.ReadKey();
            /*This code example produces the following output:

            ID: 1234   Name: crank arm
            ID: 1334   Name: chain ring
            ID: 1434   Name: regular seat
            ID: 1444   Name: banana seat
            ID: 1534   Name: cassette
            ID: 1634   Name: shift lever

            Contains: Part with Id=1734: False

            Find: Part where name contains "seat": ID: 1434   Name: regular seat

            Exists: Part with Id=1444: True
             */
        }
        #endregion
        #region TcpListener
        public static void Main19()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 11000;//Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("192.168.0.195");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }
        #endregion

        #region SynchronousSocketListener
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
            IPAddress ipAddress = ipHostInfo.AddressList[2];
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
                    while (true)
                    {
                        int bytesRec = handler.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

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

        public static int Main19(String[] args)
        {
            StartListening();
            return 0;
        }
        #endregion

        #region  AsynchronousSocketListener
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

        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        

        public static void StartListening1()
        {
            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[1];
            IPAddress ipAddress = IPAddress.Parse("192.168.0.195");//以本机作测试
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 12000);

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
                Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                    content.Length, content);
                //if (content.IndexOf("<EOF>") > -1)
                //{
                //    // All the data has been read from the
                //    // client. Display it on the console.  
                //    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                //        content.Length, content);
                //    // Echo the data back to the client.  
                //    Send(handler, content);
                //}
                //else
                //{
                //    // Not all data received. Get more.  
                //    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                //    new AsyncCallback(ReadCallback), state);
                //}

            }
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

        public static int Main20(String[] args)
        {
            StartListening1();
            return 0;
        }
        #endregion

        #region ManualResetEvent 
        // mre is used to block and release threads manually. It is
        // created in the unsignaled state.
        private static ManualResetEvent mre = new ManualResetEvent(false);

        static void Main2()
        {
            Console.WriteLine("\nStart 3 named threads that block on a ManualResetEvent:\n");

            for (int i = 0; i <= 2; i++)
            {
                Thread t = new Thread(ThreadProc1);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            Console.WriteLine("\nWhen all three threads have started, press Enter to call Set()" +
                              "\nto release all the threads.\n");
            Console.ReadLine();

            mre.Set();

            Thread.Sleep(500);
            Console.WriteLine("\nWhen a ManualResetEvent is signaled, threads that call WaitOne()" +
                              "\ndo not block. Press Enter to show this.\n");
            Console.ReadLine();

            for (int i = 3; i <= 4; i++)
            {
                Thread t = new Thread(ThreadProc1);
                t.Name = "Thread_" + i;
                t.Start();
            }

            Thread.Sleep(500);
            Console.WriteLine("\nPress Enter to call Reset(), so that threads once again block" +
                              "\nwhen they call WaitOne().\n");
            Console.ReadLine();

            mre.Reset();

            // Start a thread that waits on the ManualResetEvent.
            Thread t5 = new Thread(ThreadProc1);
            t5.Name = "Thread_5";
            t5.Start();

            Thread.Sleep(500);
            Console.WriteLine("\nPress Enter to call Set() and conclude the demo.");
            Console.ReadLine();

            mre.Set();

            // If you run this example in Visual Studio, uncomment the following line:
            //Console.ReadLine();
        }

        private static void ThreadProc1()
        {
            string name = Thread.CurrentThread.Name;

            Console.WriteLine(name + " starts and calls mre.WaitOne()");

            mre.WaitOne();

            Console.WriteLine(name + " ends.");
        }
        #endregion

        #region 取消任务
        public static async void Foo1()
        {
            var source = new CancellationTokenSource();

            Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine("task 运行结束");
            }, source.Token);
            await Task.Delay(TimeSpan.FromSeconds(3));
            source.Cancel();
            Console.WriteLine("取消任务");
        }
        public static async void Foo2()
        {
            var source = new CancellationTokenSource();

            Task.Run(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine("task 运行结束");
            }, source.Token).ContinueWith(task => Console.WriteLine(task.Status));
            await Task.Delay(TimeSpan.FromSeconds(3));
            source.Cancel();
            Console.WriteLine("取消任务");
        }
        public static async void Foo3()
        {
            var source = new CancellationTokenSource();

            Task.Run(() =>
            {
                Console.WriteLine("task 运行开始");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                Console.WriteLine("task 运行结束");
            }, source.Token).ContinueWith(task => Console.WriteLine(task.Status));
            source.Cancel();
            Console.WriteLine("取消任务");
        }
        public static async void Foo()
        {
            var source = new CancellationTokenSource();

            Task.Run(() =>
            {
                Console.WriteLine("task 运行开始");
                Thread.Sleep(TimeSpan.FromSeconds(5));
                source.Token.ThrowIfCancellationRequested();
                Console.WriteLine("task 运行结束");
            }, source.Token).ContinueWith(task => Console.WriteLine(task.Status));
            await Task.Delay(TimeSpan.FromSeconds(3));
            source.Cancel();
            Console.WriteLine("取消任务");
        }
        public static void MainT(String[] args)
        {
            Foo();
            Console.ReadKey();
            
        }
        #endregion
        #region 取消任务令牌
        static async Task Main()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var task = Task.Run(() =>
            {
                // Were we already canceled?
                ct.ThrowIfCancellationRequested();

                bool moreToDo = true;
                while (moreToDo)
                {
                    // Poll on this property if you have to do
                    // other cleanup before throwing.
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                }
            }, tokenSource2.Token); // Pass same token to Task.Run.

            tokenSource2.Cancel();

            // Just continue on this thread, or await with try-catch:
            try
            {
                await task;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            }
            finally
            {
                tokenSource2.Dispose();
            }

            Console.ReadKey();
        }
        #endregion
        #region 取消任务
        public static async Task Maina()
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            // Store references to the tasks so that we can wait on them and
            // observe their status after cancellation.
            Task t;
            var tasks = new ConcurrentBag<Task>();

            Console.WriteLine("Press any key to begin tasks...");
            Console.ReadKey(true);
            Console.WriteLine("To terminate the example, press 'c' to cancel and exit...");
            Console.WriteLine();

            // Request cancellation of a single task when the token source is canceled.
            // Pass the token to the user delegate, and also to the task so it can
            // handle the exception correctly.
            t = Task.Run(() => DoSomeWork(1, token), token).ContinueWith(task => Console.WriteLine(task.Status.ToString()));
            Console.WriteLine("Task {0} executing", t.Id);
            tasks.Add(t);

            // Request cancellation of a task and its children. Note the token is passed
            // to (1) the user delegate and (2) as the second argument to Task.Run, so
            // that the task instance can correctly handle the OperationCanceledException.
            t = Task.Run(() =>
            {
                // Create some cancelable child tasks.
                Task tc;
                for (int i = 3; i <= 10; i++)
                {
                    // For each child task, pass the same token
                    // to each user delegate and to Task.Run.
                    tc = Task.Run(() => DoSomeWork(i, token), token);
                    Console.WriteLine("Task {0} executing", tc.Id);
                    tasks.Add(tc);
                    // Pass the same token again to do work on the parent task.
                    // All will be signaled by the call to tokenSource.Cancel below.
                    DoSomeWork(2, token);
                }
            }, token);

            Console.WriteLine("Task {0} executing", t.Id);
            tasks.Add(t);

            // Request cancellation from the UI thread.
            char ch = Console.ReadKey().KeyChar;
            if (ch == 'c' || ch == 'C')
            {
                tokenSource.Cancel();
                Console.WriteLine("\nTask cancellation requested.");

                // Optional: Observe the change in the Status property on the task.
                // It is not necessary to wait on tasks that have canceled. However,
                // if you do wait, you must enclose the call in a try-catch block to
                // catch the TaskCanceledExceptions that are thrown. If you do
                // not wait, no exception is thrown if the token that was passed to the
                // Task.Run method is the same token that requested the cancellation.
            }

            try
            {
                await Task.WhenAll(tasks.ToArray());
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine($"\n{nameof(OperationCanceledException)} thrown\n");
            }
            finally
            {
                tokenSource.Dispose();
            }

            // Display status of all tasks.
            foreach (var task in tasks)
                Console.WriteLine("Task {0} status is now {1}", task.Id, task.Status);
            Console.ReadKey();
        }

        static void DoSomeWork(int taskNum, CancellationToken ct)
        {
            // Was cancellation already requested?
            Console.WriteLine("taskNum:",taskNum);
            if (ct.IsCancellationRequested)
            {
                Console.WriteLine("Task {0} was cancelled before it got started.",
                                  taskNum);
                ct.ThrowIfCancellationRequested();
            }

            int maxIterations = 100;

            // NOTE!!! A "TaskCanceledException was unhandled
            // by user code" error will be raised here if "Just My Code"
            // is enabled on your computer. On Express editions JMC is
            // enabled and cannot be disabled. The exception is benign.
            // Just press F5 to continue executing your code.
            for (int i = 0; i <= maxIterations; i++)
            {
                // Do a bit of work. Not too much.
                var sw = new SpinWait();
                for (int j = 0; j <= 100; j++)
                    sw.SpinOnce();

                if (ct.IsCancellationRequested)
                {
                    Console.WriteLine("Task {0} cancelled", taskNum);
                    ct.ThrowIfCancellationRequested();
                }
            }
        }
        #endregion
        public bool IsUnique(string astr)
        {

            return !astr.ToList().GroupBy(e => e).Any(e => e.Count() > 1);
        }

        public static void Main3()
        {
            // Declare a Func variable and assign a lambda expression to the  
            // variable. The method takes a string and converts it to uppercase.
            Func<string, string> selector = str => str.ToUpper();

            // Create an array of strings.
            string[] words = { "orange", "apple", "Article", "elephant" };
            // Query the array and select strings according to the selector method.
            IEnumerable<String> aWords = words.Select(selector);

            // Output the results to the console.
            foreach (String word in aWords)
                Console.WriteLine(word);
            Console.ReadKey();

            /*
            This code example produces the following output:

            ORANGE
            APPLE
            ARTICLE
            ELEPHANT

            */
        }
       
         static async Task Main4()
        {
            var tokenSource2 = new CancellationTokenSource();
            CancellationToken ct = tokenSource2.Token;

            var task = Task.Run(() =>
            {
                // Were we already canceled?
                ct.ThrowIfCancellationRequested();

                bool moreToDo = true;
                while (moreToDo)
                {
                    // Poll on this property if you have to do
                    // other cleanup before throwing.
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        ct.ThrowIfCancellationRequested();
                    }
                }
            }, tokenSource2.Token); // Pass same token to Task.Run.

            tokenSource2.Cancel();

            // Just continue on this thread, or await with try-catch:
            try
            {
                await task;
            }
            catch (OperationCanceledException e)
            {
                Console.WriteLine($"{nameof(OperationCanceledException)} thrown with message: {e.Message}");
            }
            finally
            {
                tokenSource2.Dispose();
            }

            Console.ReadKey();
        }
        public static void Main2(string[] args)
        {
            string host;
            int port = 80;

            if (args.Length == 0)
                // If no server name is passed as argument to this program, 
                // use the current host name as the default.
                host = Dns.GetHostName();
            else
                host = args[0];

            string result = SocketSendReceive(host, port);
            Console.WriteLine(result);
            Console.ReadKey();
        }
        static void Main1(string[] args)
        {
            var tasks = new List<Task<int>>();
            var source = new CancellationTokenSource();
            var token = source.Token;
            int completedIterations = 0;

            for (int n = 0; n <= 19; n++)
                tasks.Add(Task.Run(() => {
                    int iterations = 0;
                    for (int ctr = 1; ctr <= 2000000; ctr++)
                    {
                        token.ThrowIfCancellationRequested();
                        iterations++;
                    }
                    Interlocked.Increment(ref completedIterations);
                    if (completedIterations >= 10)
                        source.Cancel();
                    return iterations;
                }, token));

            Console.WriteLine("Waiting for the first 10 tasks to complete...\n");
            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (AggregateException)
            {
                Console.WriteLine("Status of tasks:\n");
                Console.WriteLine("{0,10} {1,20} {2,14:N0}", "Task Id",
                                  "Status", "Iterations");
                foreach (var t in tasks)
                    Console.WriteLine("{0,10} {1,20} {2,14}",
                                      t.Id, t.Status,
                                      t.Status != TaskStatus.Canceled ? t.Result.ToString("N0") : "n/a");
            }

            //Action<object> action = (object obj) =>
            //{
            //    Console.WriteLine("Task={0}, obj={1}, Thread={2}",
            //    Task.CurrentId, obj,
            //    Thread.CurrentThread.ManagedThreadId);
            //};

            //// Create a task but do not start it.
            //Task t1 = new Task(action, "alpha");

            //// Construct a started task
            //Task t2 = Task.Factory.StartNew(action, "beta");
            //// Block the main thread to demonstrate that t2 is executing
            //t2.Wait();

            //// Launch t1 
            //t1.Start();
            //Console.WriteLine("t1 has been launched. (Main Thread={0})",
            //                  Thread.CurrentThread.ManagedThreadId);
            //// Wait for the task to finish.
            //t1.Wait();

            //// Construct a started task using Task.Run.
            //String taskData = "delta";
            //Task t3 = Task.Run(() => {
            //    Console.WriteLine("Task={0}, obj={1}, Thread={2}",
            //                      Task.CurrentId, taskData,
            //                       Thread.CurrentThread.ManagedThreadId);
            //});
            //// Wait for the task to finish.
            //t3.Wait();

            //// Construct an unstarted task
            //Task t4 = new Task(action, "gamma");
            //// Run it synchronously
            //t4.RunSynchronously();
            //// Although the task was run synchronously, it is a good practice
            //// to wait for it in the event exceptions were thrown by the task.
            //t4.Wait();
            //Days meetingDays = Days.Monday | Days.Wednesday | Days.Friday;
            //Console.WriteLine(meetingDays);
            //// Output:
            //// Monday, Wednesday, Friday

            //Days workingFromHomeDays = Days.Thursday | Days.Friday;
            //Console.WriteLine($"Join a meeting by phone on {meetingDays & workingFromHomeDays}");
            //// Output:
            //// Join a meeting by phone on Friday

            //bool isMeetingOnTuesday = (meetingDays & Days.Tuesday) == Days.Tuesday;
            //Console.WriteLine($"Is there a meeting on Tuesday: {isMeetingOnTuesday}");
            //// Output:
            //// Is there a meeting on Tuesday: False

            //var a = (Days)37;
            //Console.WriteLine(a);
            // Output:
            // Monday, Wednesday, Saturday
            //Console.WriteLine("开始进行计算");
            //// ThreadPool.QueueUserWorkItem(Sum, 10);
            //Task<int> task = new Task<int>(Sum, 100);
            //task.Start();
            //Console.WriteLine("开始进行计算");
            ////显示等待获取结果
            //task.Wait();
            //Console.WriteLine("开始进行计算");
            ////调用Result时，等待返回结果
            //Console.WriteLine("程序结果为 Sum = {0}", task.Result);
            //Console.WriteLine("程序结束");
            //Console.ReadLine();


            //Console.WriteLine("获取百度数据");
            //ExecuteAsync();
            //Console.WriteLine("线程结束");
            //Console.ReadLine();

            //thread1 = new Thread(ThreadProc);
            //thread1.Name = "Thread1";
            //thread1.Start();

            //thread2 = new Thread(ThreadProc);
            //thread2.Name = "Thread2";
            //thread2.Start();
        }
        public static async void ExecuteAsync()
        {
            string text = await DownloadStringWithRetries("http://wwww.baidu.com");
            Console.WriteLine(text);
        }
        private static async Task<string> DownloadStringWithRetries(string uri)
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
        public static int Sum(object i)
        {
            var sum = 0;
            for (var j = 0; j <= (int)i; j++)
            {
                Console.Write("{0} + ", sum);
                sum += j;
            }
            Console.WriteLine(" = {0}", sum);
            return sum;
        }
        private static void ThreadProc2()
        {
            
            Console.WriteLine("\nCurrent thread: {0}", Thread.CurrentThread.Name);
            if (Thread.CurrentThread.Name == "Thread1" &&
                thread2.ThreadState != ThreadState.Unstarted)
                thread2.Join();

            Thread.Sleep(4000);
            Console.WriteLine("\nCurrent thread: {0}", Thread.CurrentThread.Name);
            Console.WriteLine("Thread1: {0}", thread1.ThreadState);
            Console.WriteLine("Thread2: {0}\n", thread2.ThreadState);
        }
    }
}
