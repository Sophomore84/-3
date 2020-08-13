using HslCommunication.Enthernet;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自动化库存管理.Command
{
   public  class NetCommand
    {
        #region Simplify Client
        /// <summary>
        /// 参数
        ///ipAddress
        ///类型：System.String
        ///服务器的ip地址
        /// port
        ///类型：System.Int32
        /// 服务器的端口号
        /// </summary>
        public static NetSimplifyClient simplifyClient = new NetSimplifyClient("127.0.0.1", 23457);//同步访问数据的客户端类，用于向服务器请求一些确定的数据信息
        #endregion
        #region Push NetClient
        //        参数
        //ipAddress
        //类型：System.String
        //服务器的IP地址
        //port
        //类型：System.Int32
        //服务器的端口号
        //key
        //类型：System.String
        //订阅关键字
        public static NetPushClient pushClient = new NetPushClient("127.0.0.1", 23467, "A"); //发布订阅类的客户端，使用指定的关键订阅相关的数据推送信息                         // 数据订阅器，负责订阅主要的数据

        public static void PushCallBack(NetPushClient pushClient, string value)
        {
            JObject content = JObject.Parse(value);

            //if (isClientIni)
            //{
            //    ShowReadContent(content);
            //}
        }

        #endregion

        #region 显示结果

        // 接收到服务器传送过来的数据后需要对数据进行解析显示
        //private void ShowReadContent(JObject content)
        //{
        //    if (InvokeRequired && !IsDisposed)
        //    {
        //        try
        //        {
        //            Invoke(new Action<JObject>(ShowReadContent), content);
        //        }
        //        catch
        //        {

        //        }
        //        return;
        //    }

        //    double temp1 = content["temp"].ToObject<double>();           // 温度
        //    bool machineEnable = content["enable"].ToObject<bool>();     // 设备使能
        //    int product = content["product"].ToObject<int>();            // 产量数据


        //    hslLedDisplay1.DisplayText = temp1.ToString();

        //    // 如果温度超100℃就把背景改为红色
        //    hslLedDisplay1.ForeColor = temp1 > 100d ? Color.Tomato : Color.Lime;
        //    label3.Text = product.ToString();

        //    label5.Text = machineEnable ? "运行中" : "未启动";

        //    // 添加仪表盘显示
        //    hslGauge1.Value = (float)Math.Round(temp1, 1);

        //    // 添加温度控件显示
        //    hslThermometer1.Value = (float)Math.Round(temp1, 1);

        //    // 添加实时的数据曲线
        //    hslCurve1.AddCurveData("温度", (float)temp1);
        //}


        //private void ShowHistory(byte[] content)
        //{
        //    if (InvokeRequired && !IsDisposed)
        //    {
        //        Invoke(new Action<byte[]>(ShowHistory), content);
        //        return;
        //    }

        //    float[] value = new float[content.Length / 4];
        //    for (int i = 0; i < value.Length; i++)
        //    {
        //        value[i] = BitConverter.ToSingle(content, i * 4);
        //    }

        //    hslCurve1.AddCurveData("温度", value);

        //}


        #endregion


    }
}
