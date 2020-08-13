using HslCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary_Crane
{
    public class PlcData
    {
        //在Hslcommunication的最新版V7.0.1里，新增了一个非常特殊的类，
        //那就是 HslDeviceAddress特性类。然后给所有plc和modbus的基类，
        //NetworkDeviceBase 实现了一个直接基于对象的读取操作。

        //如果我要读取西门子PLC的三个数据内容。
        //DB1.0，开始的200个字节
        //DB2.0, 开始的256个字节（string类型，前两个字节是字符串长度）（用来接收MES信息）
        //DB3.0, 开始的256个字节（string类型，前两个字节是字符串长度）（用来回复MES信息）
        //DB8.200 开始的60个字节。
        //M100 开始的10个字节。


        //行车的控制信息（包含X,Y,Z,及相关控制信号） 
        [HslDeviceAddress("DB110.00", 45)]
        public byte[] Data1 { get; set; }


        //从mes接收数据
        [HslDeviceAddress("DB111.00", 256)]
        public byte[] DataMesRecieve { get; set; }

        //发送给Mes数据
        [HslDeviceAddress("DB112.0", 256)]
        public byte[] DataMesSend { get; set; }


        //从SICK处获取的数据
        [HslDeviceAddress("DB113.0", 256)]
        public byte[] DataSickRecieve { get; set; }

        ////发送给SICK处获取的数据
        //[HslDeviceAddress("DB114.0", 256)]
        //public byte[] DataSickSend { get; set; }

        //从密排链处获取的数据
        [HslDeviceAddress("DB115.0", 256)]
        public byte[] DataMiPaiRecieve { get; set; }

        ////发送给密排链的数据
        //[HslDeviceAddress("DB116.0", 256)]
        //public byte[] DataMiPaiSend { get; set; }

        //从HMI处获取的数据
        [HslDeviceAddress("DB117.0", 91)]
        public byte[] DataHMI { get; set; }


        //新的半自动操作+池内全自动操作数据
        [HslDeviceAddress("DB134.0", 1)]
        public byte[] DataNewHalfAuto { get; set; }

    }
}
