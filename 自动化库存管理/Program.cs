using HslCommunication.LogNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自动化库存管理
{
    static class Program
    {
        //系统运行日志
        public static ILogNet sysLog = new LogNetDateTime(Application.StartupPath + "\\Logs\\软件系统启动关闭日志", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
        public static System.Threading.Mutex Run;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault(false);
        //    //Thread.Sleep(5000);
        //    Application.Run(new RuChuKuForm());
        //}
        static void Main()
        {
            bool noRun = false;
            Run = new System.Threading.Mutex(true, "HumControl", out noRun);
            //检测是否已经运行
            if (noRun)//未运行
            {
                try
                {
                    Run.ReleaseMutex();
                    Application.ThreadException += new ThreadExceptionEventHandler(UIThreadException);
                    Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                    AppDomain.CurrentDomain.UnhandledException +=
                    new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    sysLog.WriteAnyString("---------------------------------------------------------------------------------");
                    sysLog.WriteDebug("系统开始启动！");
                    Application.Run(new RuChuKuForm());

                }
                catch
                {
                    Application.Restart();
                }
            }
            else//已经运行
            {
                //MessageBox.Show("系统正在运行\r\n请不要重复开启!", "警告！", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //MessageBox.Show("已经有一个实例正在运行!");
                //切换到已打开的实例
            }

        }
        //日志
        static ILogNet errorLogNet = new LogNetDateTime(Application.StartupPath + "\\Logs\\窗体和线程异常日志", GenerateMode.ByEveryDay); // 创建日志器，按每天存储不同的文件
        private static void UIThreadException(object sender, ThreadExceptionEventArgs t)
        {
            try
            {
                string errorMsg = "Windows窗体线程异常 : \r\n";
                errorLogNet.WriteDebug(errorMsg + t.Exception.Message + Environment.NewLine + t.Exception.StackTrace);
            }
            catch
            {
                errorLogNet.WriteDebug("不可恢复的Windows窗体异常，应用程序将退出！");
            }
            finally
            {

                //MessageBox.Show("不可恢复的Windows窗体异常！");
                //发生异常后的两种处理方式
                //一、重新启动应用程序
                sysLog.WriteDebug("不可恢复的Windows窗体异常，应用程序将重启！");
                Program.Run.Close();
                Application.Restart();
                //二、退出应用程序
                //Program.sysLog.WriteAnyString("不可恢复的Windows窗体异常，应用程序将退出！");
                //Application.Exit();
                ////日志记录必须放在环境退出之前
                //Environment.Exit(0);
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;
                string errorMsg = "非窗体线程异常 : \n\n";
                errorLogNet.WriteDebug(errorMsg + ex.Message + Environment.NewLine + ex.StackTrace);
            }
            catch
            {
                errorLogNet.WriteDebug("不可恢复的非Windows窗体线程异常，应用程序将退出！");
            }
            finally
            {

                //MessageBox.Show("不可恢复的非Windows窗体线程异常！");
                //发生异常后的两种处理方式
                //一、重新启动应用程序
                sysLog.WriteDebug("不可恢复的非Windows窗体线程异常，应用程序将重启！");
                Program.Run.Close();
                Application.Restart();

                ////二、退出应用程序
                //Program.sysLog.WriteAnyString("不可恢复的Windows窗体异常，应用程序将退出！");
                //Application.Exit();
                ////日志记录必须放在环境退出之前
                //Environment.Exit(0);
            }
        }
    }
}
