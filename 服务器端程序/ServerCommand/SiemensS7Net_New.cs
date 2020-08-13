using HslCommunication;
using HslCommunication.BasicFramework;
using HslCommunication.Core;
using HslCommunication.Profinet.Siemens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_Crane
{
    /// <summary>
    /// 继承了西门子读写的类，重写一些读写方法
    /// </summary>
    public class SiemensS7Net_New : SiemensS7Net
    {
        private SiemensPLCS CurrentPlc = SiemensPLCS.S1200;

        private new HslCommunication.Core.IByteTransform ByteTransform = new HslCommunication.Core.ReverseBytesTransform();

        public SiemensS7Net_New(SiemensPLCS siemens) : base(siemens)
        {

        }
        public SiemensS7Net_New(SiemensPLCS siemens, string ipAddress) : base(siemens, ipAddress)
        {
            this.CurrentPlc = siemens;
        }


        /// <summary>
        /// 读取西门子的地址的字符串信息，这个信息是和西门子绑定在一起，长度随西门子的信息动态变化的
        /// </summary>
        /// <param name="address">数据地址，具体的格式需要参照类的说明文档</param>
        /// <returns>带有是否成功的字符串结果类对象</returns>
        public new OperateResult<string> ReadString(string address)
        {
            if (CurrentPlc != SiemensPLCS.S200Smart)
            {
                var read = Read(address, 2);
                if (!read.IsSuccess) return OperateResult.CreateFailedResult<string>(read);

                if (read.Content[0] == 0 || read.Content[0] == 255) return new OperateResult<string>("Value in plc is not string type");


                var readString = Read(address, (ushort)(2 + read.Content[1]));
                if (!readString.IsSuccess) return OperateResult.CreateFailedResult<string>(readString);

                return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(readString.Content, 2, readString.Content.Length - 2));
            }
            else
            {
                var read = Read(address, 1);
                if (!read.IsSuccess) return OperateResult.CreateFailedResult<string>(read);

                var readString = Read(address, (ushort)(1 + read.Content[0]));
                if (!readString.IsSuccess) return OperateResult.CreateFailedResult<string>(readString);

                return OperateResult.CreateSuccessResult(Encoding.ASCII.GetString(readString.Content, 1, readString.Content.Length - 1));
            }
        }

        /// <summary>
        /// 写入西门子的地址的字符串信息，这个信息是和西门子绑定在一起，长度随西门子的信息动态变化的
        /// </summary>
        /// <param name="address">数据地址，具体的格式需要参照类的说明文档</param>
        /// <returns>带有是否成功的字符串结果类对象</returns>
        public OperateResult WriteString(string address, string value)
        {
            OperateResult WriteResult;

            //首先调用一次读，根据读取的信息，再进行相应的写
            OperateResult<byte[]> ReadResult = this.Read(address, 2);
            bool flag2 = !ReadResult.IsSuccess;
            if (flag2)
            {
                WriteResult = OperateResult.CreateFailedResult<string>(ReadResult);
            }
            else
            {
                bool flag3 = ReadResult.Content[0] == 255;
                if (flag3)
                {
                    WriteResult = new OperateResult("Value in plc is not string type");
                }
                else
                {
                    //plc中未定义string
                    if (ReadResult.Content[0] == 0)
                    {
                        //写入个默认长度
                        ReadResult.Content[0] = 254;
                    }

                    if (value == null) value = string.Empty;

                    byte[] buffer = Encoding.ASCII.GetBytes(value);
                    //判断要写入的值的长度
                    if (buffer.Length >= 255)
                    {
                        WriteResult = new OperateResult("The string length exceeds the maximum length 254");
                    }
                    else
                    {
                        //超过定义的最大长度
                        if (buffer.Length > ReadResult.Content[0])
                        {
                            WriteResult = new OperateResult("The string length exceeds the maximum length defined in PLC");
                        }
                        else
                        {
                            //再判断PLC中原字符串值的实际长度，进行覆盖写入
                            //此次写入的串的长度比原值长度大或相等，自动覆盖掉原值，
                            //若此次写入长度小于原值的长度，左对齐方式补空值。
                            if (buffer.Length >= ReadResult.Content[1])
                            {
                                if (CurrentPlc != SiemensPLCS.S200Smart)
                                {
                                    WriteResult = Write(address, HslCommunication.BasicFramework.SoftBasic.SpliceTwoByteArray(new byte[] { ReadResult.Content[0], (byte)buffer.Length }, buffer));
                                }
                                else
                                {
                                    WriteResult = Write(address, HslCommunication.BasicFramework.SoftBasic.SpliceTwoByteArray(new byte[] { (byte)buffer.Length }, buffer));
                                }
                            }
                            else
                            {
                                byte[] Newbuffer = new byte[(Byte)ReadResult.Content[1]];
                                Array.Copy(buffer, Newbuffer, buffer.Length);

                                if (CurrentPlc != SiemensPLCS.S200Smart)
                                {
                                    WriteResult = Write(address, HslCommunication.BasicFramework.SoftBasic.SpliceTwoByteArray(new byte[] { ReadResult.Content[0], (byte)buffer.Length }, Newbuffer));
                                }
                                else
                                {
                                    WriteResult = Write(address, HslCommunication.BasicFramework.SoftBasic.SpliceTwoByteArray(new byte[] { (byte)buffer.Length }, buffer));
                                }
                            }
                        }


                    }

                }

            }

            return WriteResult;


        }

        /// <summary>
        /// 读取西门子的地址的Wstring类型 Unicode字符串信息，这个信息是和西门子绑定在一起，长度随西门子的信息动态变化的
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public OperateResult<string> ReadUnicodeWString(string address)
        {
            //最大字长度
            UInt16 MaxWordLength = 0;
            //当前字实际长度
            UInt16 CurrentWordLength = 0;
            var read = Read(address, 4);
            if (!read.IsSuccess) return OperateResult.CreateFailedResult<string>(read);
            MaxWordLength = (UInt16)(read.Content[0] * 256 + read.Content[1]);
            CurrentWordLength = (UInt16)(read.Content[2] * 256 + read.Content[3]);

            if (MaxWordLength == 0 || MaxWordLength == 65535) return new OperateResult<string>("Value in plc is not Wstring type");
            //此时判断实际字长，最大不能超过98,
            if (CurrentWordLength > 98) return new OperateResult<string>("The Wstring length exceeds the maximum length(100Word) defined in PLC");
            //实际长度在200字节以下
            UInt16 CurrentByteLength = (UInt16)(CurrentWordLength * 2);

            //读取实际长度
            var readWString = Read(address, (UInt16)(CurrentByteLength + 4));
            if (!readWString.IsSuccess) return OperateResult.CreateFailedResult<string>(readWString);


            return ByteTransformHelper.GetResultFromBytes<string>(this.Read(address, (UInt16)(CurrentByteLength + 4)), delegate (byte[] m)
            {

                return ByteTransform.TransString(m, 4, m.Length - 4, Encoding.BigEndianUnicode);
            });
        }
        /// <summary>
        /// 写入西门子的地址的Wstring类型 Unicode字符串信息，这个信息是和西门子绑定在一起，长度随西门子的信息动态变化的
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public OperateResult WriteUnicodeWString(string address, string value)
        {
            //最大字长度
            UInt16 MaxWordLength = 0;
            //当前字实际长度
            UInt16 CurrentWordLength = 0;

            OperateResult WriteResult;

            //首先调用一次读，根据读取的信息，再进行相应的写
            OperateResult<byte[]> ReadResult = this.Read(address, 4);
            bool flag2 = !ReadResult.IsSuccess;
            if (flag2)
            {
                WriteResult = OperateResult.CreateFailedResult<string>(ReadResult);
            }
            else
            {
                MaxWordLength = (UInt16)(ReadResult.Content[0] * 256 + ReadResult.Content[1]);
                CurrentWordLength = (UInt16)(ReadResult.Content[2] * 256 + ReadResult.Content[3]);


                bool flag3 = MaxWordLength == 65535;
                if (flag3)
                {
                    WriteResult = new OperateResult("Value in plc is not string type");
                }
                else
                {
                    //plc中未定义Wstring
                    if (MaxWordLength == 0)
                    {
                        //写入个默认最大长度
                        MaxWordLength = 65534;
                        ReadResult.Content[0] = 0xFE;
                        ReadResult.Content[1] = 0xFF;
                    }


                    if (value == null) value = string.Empty;

                    //判断要写入的值的字长度(根据默认长度限制了写入的字长）
                    if (value.Length > 65534)
                    {
                        WriteResult = new OperateResult("The string length exceeds the maximum length(254) ");
                    }
                    else
                    {
                        //采用大端Unicode
                        byte[] buffer = ByteTransform.TransByte(value, Encoding.BigEndianUnicode);
                        //再判断PLC中原字符串值的实际长度，进行覆盖写入
                        //此次写入的串的长度比原值长度大或相等，自动覆盖掉原值,
                        if (value.Length >= CurrentWordLength)
                        {
                            WriteResult = Write(address, HslCommunication.BasicFramework.SoftBasic.SpliceTwoByteArray(new byte[] { ReadResult.Content[0], ReadResult.Content[1], (byte)((value.Length << 8) & 0xff00), (byte)(value.Length & 0xff) }, buffer));

                        }
                        else
                        {
                            //原实际长度对应的字节个数
                            int CurrentByteLength = (int)(CurrentWordLength * 2);
                            //若此次写入长度小于原值的长度，左对齐方式补空值。
                            byte[] Newbuffer = new byte[CurrentByteLength];
                            Array.Copy(buffer, Newbuffer, buffer.Length);

                            WriteResult = Write(address, HslCommunication.BasicFramework.SoftBasic.SpliceTwoByteArray(new byte[] { ReadResult.Content[0], ReadResult.Content[1], (byte)((value.Length << 8) & 0xff00), (byte)(value.Length & 0xff) }, Newbuffer));
                        }


                    }

                }

            }

            return WriteResult;

        }

    }
}
