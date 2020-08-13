using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 自动化库存管理
{
    public partial class ExForm : Form
    {
        #region 库位
        KuWeiForm kuweiForm;
        #endregion
        #region 库位数据库
        Maticsoft.Model.LocationRecord locationRecordModel = new Maticsoft.Model.LocationRecord();
        Maticsoft.BLL.LocationRecord locationRecordBLL = new Maticsoft.BLL.LocationRecord();
        #endregion
        public ExForm()
        {
            InitializeComponent();
        }

        private  async void btnYes_Click(object sender, EventArgs e)
        {

            //判断两个库位的状态
            if (locationRecordBLL.GetLocNameNum(textBoxWeiZhi.Text) == 0&& locationRecordBLL.GetLocNameNum(textBoxWeiZhi1.Text) == 0)//如果两个库位都是空位
            {
                MessageBox.Show("两个库位都是空位，不需要调库");
            }
            else if (locationRecordBLL.GetLocNameNum(textBoxWeiZhi.Text) == 0 && locationRecordBLL.GetLocNameNum(textBoxWeiZhi1.Text) != 0)//库位1是空位，库位2是有货
            {
                await DoExAsync(textBoxWeiZhi.Text, textBoxWeiZhi1.Text,"01");//异步执行调库任务
                
            }
            else if (locationRecordBLL.GetLocNameNum(textBoxWeiZhi.Text) != 0 && locationRecordBLL.GetLocNameNum(textBoxWeiZhi1.Text) == 0)//库位2是空位，库位1是有货
            {
                await DoExAsync(textBoxWeiZhi.Text, textBoxWeiZhi1.Text,"10");//异步执行调库任务
                
            }
            else if (locationRecordBLL.GetLocNameNum(textBoxWeiZhi.Text) != 0 && locationRecordBLL.GetLocNameNum(textBoxWeiZhi1.Text) != 0)//库位1是有货，库位2是有货
            {
                //得到一个空位
                //一个库位上的货物到空位
                //另一个库位上货物到一个库位
                //空位上的货物到另一个库位
                List<string> locNames = locationRecordBLL.GetListLocName();
                //string[] result;
                //string[] stringSeparators = new string[] { "-" };
                foreach (string locName in locNames)
                {
                    if (locationRecordBLL.GetLocNameNum(locName) == 0)//空位
                    {
                        await DoExAsync(textBoxWeiZhi.Text, textBoxWeiZhi1.Text, locName);//异步执行调库任务
                        
                        return;
                    }
                    //else
                    //{

                    //}

                }

            }
            else
            {

            }
            this.Close();
        }
        private async Task DoExAsync(string loca1,string loca2,string flag)
        {
            await Task.Run(() =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    this.Close();
                }));
                
                switch (flag)
                {
                    case "01"://
                              //库位2的货物到库位1的空位
                        RuChuKuForm.hostComputerCommand.RunPlan(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, loca1, loca2, flag) ;
                        break;
                    case "10":
                        //库位1的货物到库位2的空位
                        RuChuKuForm.hostComputerCommand.RunPlan(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, loca1, loca2, flag);
                        break;
                    default:
                        //库位1的货物到查询的空位 库位2-库位1，空位-库位2
                        RuChuKuForm.hostComputerCommand.RunPlan(RuChuKuForm.siemensTcpNet, ++RuChuKuForm.counter, loca1, loca2, flag);
                        break;
                    
                }
                //到库位1
                //RuChuKuForm.hostComputerCommand.RunPlan(RuChuKuForm.siemensTcpNet,++RuChuKuForm.counter,loca1,loca2,str);

            }
            );
        }
        private void textBoxWeiZhi_Click(object sender, EventArgs e)
        {
            kuweiForm = new KuWeiForm();
            kuweiForm.SetWeiZhiEvent += KuweiForm_SetWeiZhiEvent;
            kuweiForm.ShowDialog();
        }

        private void KuweiForm_SetWeiZhiEvent()
        {
            textBoxWeiZhi.Text = RuChuKuForm.m_WeiZhi;
        }

        private void textBoxWeiZhi1_Click(object sender, EventArgs e)
        {
            kuweiForm = new KuWeiForm();
            kuweiForm.SetWeiZhiEvent += KuweiForm_SetWeiZhiEvent1;
            kuweiForm.ShowDialog();
        }

        private void KuweiForm_SetWeiZhiEvent1()
        {
            textBoxWeiZhi1.Text = RuChuKuForm.m_WeiZhi;
        }
    }
}
