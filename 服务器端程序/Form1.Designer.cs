namespace 服务器端程序
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.instruction = new System.Windows.Forms.TextBox();
            this.instruction1 = new System.Windows.Forms.Label();
            this.userButton3 = new HslCommunication.Controls.UserButton();
            this.userButton2 = new HslCommunication.Controls.UserButton();
            this.userButton1 = new HslCommunication.Controls.UserButton();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.White;
            this.groupBox2.Controls.Add(this.userButton3);
            this.groupBox2.Controls.Add(this.userButton2);
            this.groupBox2.Controls.Add(this.userButton1);
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Controls.Add(this.instruction);
            this.groupBox2.Controls.Add(this.instruction1);
            this.groupBox2.Font = new System.Drawing.Font("隶书", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(510, 273);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "接收的平板数据";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(75, 78);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(429, 112);
            this.textBox1.TabIndex = 14;
            // 
            // instruction
            // 
            this.instruction.Location = new System.Drawing.Point(75, 26);
            this.instruction.Name = "instruction";
            this.instruction.Size = new System.Drawing.Size(429, 23);
            this.instruction.TabIndex = 13;
            // 
            // instruction1
            // 
            this.instruction1.AutoSize = true;
            this.instruction1.Location = new System.Drawing.Point(6, 29);
            this.instruction1.Name = "instruction1";
            this.instruction1.Size = new System.Drawing.Size(42, 14);
            this.instruction1.TabIndex = 12;
            this.instruction1.Text = "指令:";
            // 
            // userButton3
            // 
            this.userButton3.BackColor = System.Drawing.Color.Transparent;
            this.userButton3.CustomerInformation = "";
            this.userButton3.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton3.Location = new System.Drawing.Point(140, 208);
            this.userButton3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton3.Name = "userButton3";
            this.userButton3.Size = new System.Drawing.Size(89, 34);
            this.userButton3.TabIndex = 17;
            this.userButton3.UIText = "停止运行";
            this.userButton3.Click += new System.EventHandler(this.userButton3_Click);
            // 
            // userButton2
            // 
            this.userButton2.BackColor = System.Drawing.Color.Transparent;
            this.userButton2.CustomerInformation = "";
            this.userButton2.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton2.Location = new System.Drawing.Point(45, 208);
            this.userButton2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton2.Name = "userButton2";
            this.userButton2.Size = new System.Drawing.Size(89, 34);
            this.userButton2.TabIndex = 16;
            this.userButton2.UIText = "启动运行";
            this.userButton2.Click += new System.EventHandler(this.userButton2_Click);
            // 
            // userButton1
            // 
            this.userButton1.BackColor = System.Drawing.Color.Transparent;
            this.userButton1.CustomerInformation = "";
            this.userButton1.EnableColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
            this.userButton1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.userButton1.Location = new System.Drawing.Point(235, 208);
            this.userButton1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.userButton1.Name = "userButton1";
            this.userButton1.Size = new System.Drawing.Size(89, 34);
            this.userButton1.TabIndex = 15;
            this.userButton1.UIText = "启动引擎";
            this.userButton1.Click += new System.EventHandler(this.userButton1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ClientSize = new System.Drawing.Size(746, 418);
            this.Controls.Add(this.groupBox2);
            this.Name = "Form1";
            this.Text = "服务器端";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox instruction;
        private System.Windows.Forms.Label instruction1;
        private System.Windows.Forms.TextBox textBox1;
        private HslCommunication.Controls.UserButton userButton3;
        private HslCommunication.Controls.UserButton userButton2;
        private HslCommunication.Controls.UserButton userButton1;
    }
}

