namespace 自动化库存管理
{
    partial class RuChuKuForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RuChuKuForm));
            this.label1 = new System.Windows.Forms.Label();
            this.btnMin = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnTongDian = new System.Windows.Forms.Button();
            this.btnDuanDian = new System.Windows.Forms.Button();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelWeiZhi = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSwitch = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSerialState = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelSerialPort = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnFuWei = new System.Windows.Forms.Button();
            this.btnJiTing = new System.Windows.Forms.Button();
            this.btnZanTing = new System.Windows.Forms.Button();
            this.timerDingShi = new System.Windows.Forms.Timer(this.components);
            this.textTask = new System.Windows.Forms.TextBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.用户管理toolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.串口配置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PLC读写toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.退出登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.btnErrorRest = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("楷体", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(345, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "KRC公司自动化行车库存管理系统";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnMin
            // 
            this.btnMin.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnMin.Location = new System.Drawing.Point(704, 1);
            this.btnMin.Margin = new System.Windows.Forms.Padding(2);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(30, 30);
            this.btnMin.TabIndex = 1;
            this.btnMin.Text = "—";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.Location = new System.Drawing.Point(733, 1);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(30, 30);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "×";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnTongDian
            // 
            this.btnTongDian.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnTongDian.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTongDian.Font = new System.Drawing.Font("楷体", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTongDian.Location = new System.Drawing.Point(320, -2);
            this.btnTongDian.Margin = new System.Windows.Forms.Padding(2);
            this.btnTongDian.Name = "btnTongDian";
            this.btnTongDian.Size = new System.Drawing.Size(71, 71);
            this.btnTongDian.TabIndex = 3;
            this.btnTongDian.Text = "启动";
            this.btnTongDian.UseVisualStyleBackColor = false;
            this.btnTongDian.Visible = false;
            this.btnTongDian.Click += new System.EventHandler(this.btnTongDian_Click);
            // 
            // btnDuanDian
            // 
            this.btnDuanDian.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnDuanDian.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuanDian.Font = new System.Drawing.Font("楷体", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDuanDian.Location = new System.Drawing.Point(243, -2);
            this.btnDuanDian.Margin = new System.Windows.Forms.Padding(2);
            this.btnDuanDian.Name = "btnDuanDian";
            this.btnDuanDian.Size = new System.Drawing.Size(71, 71);
            this.btnDuanDian.TabIndex = 4;
            this.btnDuanDian.Text = "停止";
            this.btnDuanDian.UseVisualStyleBackColor = false;
            this.btnDuanDian.Visible = false;
            this.btnDuanDian.Click += new System.EventHandler(this.btnDuanDian_Click);
            // 
            // mainPanel
            // 
            this.mainPanel.Location = new System.Drawing.Point(0, 74);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(763, 450);
            this.mainPanel.TabIndex = 5;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.toolStripStatusLabelWeiZhi,
            this.toolStripStatusLabelSwitch,
            this.toolStripStatusLabelState,
            this.toolStripStatusLabelSerialState,
            this.toolStripStatusLabelSerialPort,
            this.toolStripStatusLabelTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 457);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(765, 36);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.BackColor = System.Drawing.Color.Navy;
            this.toolStripStatusLabel2.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabel2.Font = new System.Drawing.Font("华文楷体", 18F);
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(546, 31);
            this.toolStripStatusLabel2.Spring = true;
            // 
            // toolStripStatusLabelWeiZhi
            // 
            this.toolStripStatusLabelWeiZhi.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelWeiZhi.Font = new System.Drawing.Font("华文楷体", 18F);
            this.toolStripStatusLabelWeiZhi.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabelWeiZhi.Name = "toolStripStatusLabelWeiZhi";
            this.toolStripStatusLabelWeiZhi.Size = new System.Drawing.Size(4, 31);
            // 
            // toolStripStatusLabelSwitch
            // 
            this.toolStripStatusLabelSwitch.Font = new System.Drawing.Font("华文楷体", 18F);
            this.toolStripStatusLabelSwitch.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabelSwitch.Name = "toolStripStatusLabelSwitch";
            this.toolStripStatusLabelSwitch.Size = new System.Drawing.Size(0, 31);
            // 
            // toolStripStatusLabelState
            // 
            this.toolStripStatusLabelState.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelState.Font = new System.Drawing.Font("华文楷体", 18F);
            this.toolStripStatusLabelState.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabelState.Name = "toolStripStatusLabelState";
            this.toolStripStatusLabelState.Size = new System.Drawing.Size(4, 31);
            // 
            // toolStripStatusLabelSerialState
            // 
            this.toolStripStatusLabelSerialState.Font = new System.Drawing.Font("华文楷体", 18F);
            this.toolStripStatusLabelSerialState.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabelSerialState.Name = "toolStripStatusLabelSerialState";
            this.toolStripStatusLabelSerialState.Size = new System.Drawing.Size(60, 31);
            this.toolStripStatusLabelSerialState.Text = "串口";
            // 
            // toolStripStatusLabelSerialPort
            // 
            this.toolStripStatusLabelSerialPort.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelSerialPort.Font = new System.Drawing.Font("华文楷体", 18F);
            this.toolStripStatusLabelSerialPort.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabelSerialPort.Name = "toolStripStatusLabelSerialPort";
            this.toolStripStatusLabelSerialPort.Size = new System.Drawing.Size(136, 31);
            this.toolStripStatusLabelSerialPort.Text = "实时重量：";
            // 
            // toolStripStatusLabelTime
            // 
            this.toolStripStatusLabelTime.Font = new System.Drawing.Font("华文楷体", 18F);
            this.toolStripStatusLabelTime.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusLabelTime.Name = "toolStripStatusLabelTime";
            this.toolStripStatusLabelTime.Size = new System.Drawing.Size(0, 31);
            // 
            // btnFuWei
            // 
            this.btnFuWei.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnFuWei.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFuWei.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFuWei.Location = new System.Drawing.Point(474, -2);
            this.btnFuWei.Margin = new System.Windows.Forms.Padding(2);
            this.btnFuWei.Name = "btnFuWei";
            this.btnFuWei.Size = new System.Drawing.Size(71, 71);
            this.btnFuWei.TabIndex = 8;
            this.btnFuWei.Text = "指令复位";
            this.btnFuWei.UseVisualStyleBackColor = false;
            this.btnFuWei.Visible = false;
            this.btnFuWei.Click += new System.EventHandler(this.btnFuWei_Click);
            // 
            // btnJiTing
            // 
            this.btnJiTing.BackColor = System.Drawing.Color.Red;
            this.btnJiTing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJiTing.Font = new System.Drawing.Font("楷体", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnJiTing.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnJiTing.Location = new System.Drawing.Point(166, -2);
            this.btnJiTing.Margin = new System.Windows.Forms.Padding(2);
            this.btnJiTing.Name = "btnJiTing";
            this.btnJiTing.Size = new System.Drawing.Size(71, 71);
            this.btnJiTing.TabIndex = 11;
            this.btnJiTing.Text = "急停";
            this.btnJiTing.UseVisualStyleBackColor = false;
            this.btnJiTing.Click += new System.EventHandler(this.btnJiTing_Click);
            // 
            // btnZanTing
            // 
            this.btnZanTing.BackColor = System.Drawing.Color.Red;
            this.btnZanTing.Enabled = false;
            this.btnZanTing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZanTing.Font = new System.Drawing.Font("楷体", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnZanTing.Location = new System.Drawing.Point(552, -2);
            this.btnZanTing.Margin = new System.Windows.Forms.Padding(2);
            this.btnZanTing.Name = "btnZanTing";
            this.btnZanTing.Size = new System.Drawing.Size(71, 71);
            this.btnZanTing.TabIndex = 12;
            this.btnZanTing.Text = "暂停";
            this.btnZanTing.UseVisualStyleBackColor = false;
            this.btnZanTing.Visible = false;
            this.btnZanTing.Click += new System.EventHandler(this.btnZanTing_Click);
            // 
            // timerDingShi
            // 
            this.timerDingShi.Enabled = true;
            this.timerDingShi.Interval = 1000;
            this.timerDingShi.Tick += new System.EventHandler(this.timerDingShi_Tick);
            // 
            // textTask
            // 
            this.textTask.Location = new System.Drawing.Point(628, 33);
            this.textTask.Multiline = true;
            this.textTask.Name = "textTask";
            this.textTask.Size = new System.Drawing.Size(135, 36);
            this.textTask.TabIndex = 13;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.用户管理toolStripMenuItem,
            this.toolStripMenuItem2,
            this.串口配置ToolStripMenuItem,
            this.PLC读写toolStripMenuItem3,
            this.退出登录ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(241, 326);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(240, 50);
            this.toolStripMenuItem1.Text = "仓库管理";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // 用户管理toolStripMenuItem
            // 
            this.用户管理toolStripMenuItem.Name = "用户管理toolStripMenuItem";
            this.用户管理toolStripMenuItem.Size = new System.Drawing.Size(240, 50);
            this.用户管理toolStripMenuItem.Text = "用户管理";
            this.用户管理toolStripMenuItem.Click += new System.EventHandler(this.用户管理toolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(240, 50);
            this.toolStripMenuItem2.Text = "设备参数";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // 串口配置ToolStripMenuItem
            // 
            this.串口配置ToolStripMenuItem.Name = "串口配置ToolStripMenuItem";
            this.串口配置ToolStripMenuItem.Size = new System.Drawing.Size(240, 50);
            this.串口配置ToolStripMenuItem.Text = "串口配置";
            this.串口配置ToolStripMenuItem.Click += new System.EventHandler(this.串口配置ToolStripMenuItem_Click);
            // 
            // PLC读写toolStripMenuItem3
            // 
            this.PLC读写toolStripMenuItem3.Name = "PLC读写toolStripMenuItem3";
            this.PLC读写toolStripMenuItem3.Size = new System.Drawing.Size(240, 50);
            this.PLC读写toolStripMenuItem3.Text = "PLC读写";
            this.PLC读写toolStripMenuItem3.Click += new System.EventHandler(this.测试toolStripMenuItem3_Click);
            // 
            // 退出登录ToolStripMenuItem
            // 
            this.退出登录ToolStripMenuItem.Name = "退出登录ToolStripMenuItem";
            this.退出登录ToolStripMenuItem.Size = new System.Drawing.Size(240, 50);
            this.退出登录ToolStripMenuItem.Text = "退出登录";
            this.退出登录ToolStripMenuItem.Click += new System.EventHandler(this.退出登录ToolStripMenuItem_Click);
            // 
            // serialPort1
            // 
            this.serialPort1.PortName = "COM3";
            this.serialPort1.ReadTimeout = 500;
            this.serialPort1.WriteTimeout = 500;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // btnErrorRest
            // 
            this.btnErrorRest.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnErrorRest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnErrorRest.Font = new System.Drawing.Font("楷体", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnErrorRest.Location = new System.Drawing.Point(397, -2);
            this.btnErrorRest.Margin = new System.Windows.Forms.Padding(2);
            this.btnErrorRest.Name = "btnErrorRest";
            this.btnErrorRest.Size = new System.Drawing.Size(71, 71);
            this.btnErrorRest.TabIndex = 14;
            this.btnErrorRest.Text = "故障复位";
            this.btnErrorRest.UseVisualStyleBackColor = false;
            this.btnErrorRest.Visible = false;
            this.btnErrorRest.Click += new System.EventHandler(this.btnErrorRest_Click);
            // 
            // RuChuKuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Navy;
            this.ClientSize = new System.Drawing.Size(765, 493);
            this.Controls.Add(this.btnErrorRest);
            this.Controls.Add(this.textTask);
            this.Controls.Add(this.btnZanTing);
            this.Controls.Add(this.btnTongDian);
            this.Controls.Add(this.btnDuanDian);
            this.Controls.Add(this.btnJiTing);
            this.Controls.Add(this.btnFuWei);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnMin);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "RuChuKuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "自动化库存管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.RuChuKuForm_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.RuChuKuForm_MouseClick);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnTongDian;
        private System.Windows.Forms.Button btnDuanDian;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Button btnFuWei;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTime;
        private System.Windows.Forms.Button btnJiTing;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelState;
        private System.Windows.Forms.Button btnZanTing;
        private System.Windows.Forms.Timer timerDingShi;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelWeiZhi;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSwitch;
        private System.Windows.Forms.TextBox textTask;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 退出登录ToolStripMenuItem;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSerialPort;
        private System.Windows.Forms.ToolStripMenuItem 串口配置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PLC读写toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 用户管理toolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelSerialState;
        private System.Windows.Forms.Button btnErrorRest;
    }
}

