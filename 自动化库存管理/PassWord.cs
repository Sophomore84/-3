using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 养生池.F3_Order
{
    public partial class PassWord : Form
    {
        public PassWord()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtPassWord.Text == "999")
            {
                F3_Order.orderShowForm.passWordSetFlag = true;
                this.Close();
            }
            else
            {
                label1.Text = "密码错误！";
                F3_Order.orderShowForm.passWordSetFlag = false;
                txtPassWord.Text = "";
                txtPassWord.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            F3_Order.orderShowForm.passWordSetFlag = false;
            this.Close();
        }
    }
}
