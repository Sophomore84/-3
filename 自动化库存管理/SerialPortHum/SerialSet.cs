using HslCommunication.LogNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自动化库存管理.SerialPortHum
{
    public partial class SerialSet : Form
    {
        public SerialSet()
        {
            InitializeComponent();
            LogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\串口异常", GenerateMode.ByEveryDay);
        }
        /// <summary>
        /// 串口的日志记录器
        /// </summary>
        private static ILogNet LogNet { get; set; }
        private void buttonSetSerial_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (this.buttonSetSerial.Text=="打开串口")
                {
                    if (RuChuKuForm.SerialPortSys.IsOpen)
                    {
                        RuChuKuForm.SerialPortSys.Close();
                        RuChuKuForm.SerialPortSys.PortName = comboBoxPort.SelectedItem.ToString();
                        RuChuKuForm.SerialPortSys.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
                        RuChuKuForm.SerialPortSys.DataBits = int.Parse(comboBoxDataBits.SelectedItem.ToString());
                        RuChuKuForm.SerialPortSys.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBoxStopBits.SelectedItem.ToString(), true);
                        RuChuKuForm.SerialPortSys.Parity = (Parity)Enum.Parse(typeof(Parity), comboBoxParity.SelectedItem.ToString(), true);
                        RuChuKuForm.SerialPortSys.Open();
                        if (RuChuKuForm.SerialPortSys.IsOpen)
                        {
                            this.buttonSetSerial.Text = "关闭串口";
                            //RuChuKuForm.SerialPortSys.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
                            //RuChuKuForm.SerialPortSys.Parity


                        }
                        //this.buttonSetSerial.Text = "关闭串口";
                        //RuChuKuForm.SerialPortSys.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
                        //RuChuKuForm.SerialPortSys.Parity


                    }
                    else
                    {
                        RuChuKuForm.SerialPortSys.PortName = comboBoxPort.SelectedItem.ToString();
                        RuChuKuForm.SerialPortSys.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
                        RuChuKuForm.SerialPortSys.DataBits = int.Parse(comboBoxDataBits.SelectedItem.ToString());
                        RuChuKuForm.SerialPortSys.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBoxStopBits.SelectedItem.ToString(), true);
                        RuChuKuForm.SerialPortSys.Parity = (Parity)Enum.Parse(typeof(Parity), comboBoxParity.SelectedItem.ToString(), true);
                        RuChuKuForm.SerialPortSys.Open();
                        if (RuChuKuForm.SerialPortSys.IsOpen)
                        {
                            this.buttonSetSerial.Text = "关闭串口";
                            //RuChuKuForm.SerialPortSys.BaudRate = int.Parse(comboBoxBaudRate.SelectedItem.ToString());
                            //RuChuKuForm.SerialPortSys.Parity


                        }
                    }

                }
                else
                {
                    this.buttonSetSerial.Text = "打开串口";
                    RuChuKuForm.SerialPortSys.Close();
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                LogNet.WriteException("串口设置异常", ex);
                //this.BeginInvoke(new Action(() =>
                //{
                //    toolStripStatusLabelSerialState.Text = "对端口的访问被拒绝";
                //    toolStripStatusLabelSerialPort.Text = "实时重量：";
                //}));
            }
            catch (ArgumentOutOfRangeException ex)
            {
                LogNet.WriteException("串口设置异常", ex);
                //this.BeginInvoke(new Action(() =>
                //{
                //    toolStripStatusLabelSerialState.Text = "此实例的一个或多个属性无效";
                //    toolStripStatusLabelSerialPort.Text = "实时重量：";
                //}));
            }
            catch (ArgumentException ex)
            {
                LogNet.WriteException("串口设置异常", ex);
                //this.BeginInvoke(new Action(() =>
                //{
                //    toolStripStatusLabelSerialState.Text = "端口名称不是以“COM”开始的";
                //    toolStripStatusLabelSerialPort.Text = "实时重量：";
                //}));
            }
            catch (IOException ex)
            {
                LogNet.WriteException("串口设置异常", ex);
                //this.BeginInvoke(new Action(() =>
                //{
                //    toolStripStatusLabelSerialState.Text = "此端口处于无效状态";
                //    toolStripStatusLabelSerialPort.Text = "实时重量：";
                //}));
            }
            catch (InvalidOperationException ex)
            {
                //this.BeginInvoke(new Action(() =>
                //{
                    LogNet.WriteException("串口设置异常", ex);
                //}));
            }
            catch (Exception ex)
            {

                LogNet.WriteException("串口设置异常",ex);
            }
            
        }

        private void SerialSet_Load(object sender, EventArgs e)
        {
            if(SerialPort.GetPortNames().Length>0)
            {
                comboBoxPort.Items.Clear();
                comboBoxPort.Items.AddRange(SerialPort.GetPortNames());//添加所有的端口号
                comboBoxPort.SelectedIndex = 0;//
                //            foreach(string s in Enum.GetNames(typeof(StopBits)))
                //{
                //                comboBoxStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));
                //                Console.WriteLine("   {0}", s);
                //            }

            }
            comboBoxStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));
            comboBoxParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
           
            comboBoxBaudRate.SelectedIndex = 3;//9600
            comboBoxDataBits.SelectedIndex = 3;//8
            comboBoxStopBits.SelectedIndex = 1;//1
            comboBoxParity.SelectedIndex = 0;//None
            
        }

        private void comboBoxPort_DropDown(object sender, EventArgs e)
        {
            if (SerialPort.GetPortNames().Length > 0)
            {
                comboBoxPort.Items.Clear();
                comboBoxPort.Items.AddRange(SerialPort.GetPortNames());//添加所有的端口号
            }
        }
    }
}
