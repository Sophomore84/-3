namespace 自动化库存管理
{
    partial class loginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelYongHuDengLu = new System.Windows.Forms.Label();
            this.labelYongHuMing1 = new System.Windows.Forms.Label();
            this.labelMiMa = new System.Windows.Forms.Label();
            this.textBoxUserID = new System.Windows.Forms.TextBox();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.btn_Reset = new System.Windows.Forms.Button();
            this.labelDengLuChengGong = new System.Windows.Forms.Label();
            this.labelDangQianYongHu = new System.Windows.Forms.Label();
            this.labelYongHuMing2 = new System.Windows.Forms.Label();
            this.btnTuiChuDengLu = new System.Windows.Forms.Button();
            this.btnManageUser = new System.Windows.Forms.Button();
            this.btnRuKu = new System.Windows.Forms.Button();
            this.btnChuKu = new System.Windows.Forms.Button();
            this.btnWeiZhi = new System.Windows.Forms.Button();
            this.btnSheBeiCanShu = new System.Windows.Forms.Button();
            this.btnExchange = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelYongHuDengLu
            // 
            this.labelYongHuDengLu.AutoSize = true;
            this.labelYongHuDengLu.Font = new System.Drawing.Font("华文楷体", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelYongHuDengLu.ForeColor = System.Drawing.Color.LightYellow;
            this.labelYongHuDengLu.Location = new System.Drawing.Point(310, 64);
            this.labelYongHuDengLu.Name = "labelYongHuDengLu";
            this.labelYongHuDengLu.Size = new System.Drawing.Size(153, 37);
            this.labelYongHuDengLu.TabIndex = 0;
            this.labelYongHuDengLu.Text = "用户登录";
            // 
            // labelYongHuMing1
            // 
            this.labelYongHuMing1.AutoSize = true;
            this.labelYongHuMing1.Font = new System.Drawing.Font("华文楷体", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelYongHuMing1.ForeColor = System.Drawing.Color.LightYellow;
            this.labelYongHuMing1.Location = new System.Drawing.Point(239, 122);
            this.labelYongHuMing1.Name = "labelYongHuMing1";
            this.labelYongHuMing1.Size = new System.Drawing.Size(104, 26);
            this.labelYongHuMing1.TabIndex = 1;
            this.labelYongHuMing1.Text = "用户名：";
            // 
            // labelMiMa
            // 
            this.labelMiMa.AutoSize = true;
            this.labelMiMa.Font = new System.Drawing.Font("华文楷体", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMiMa.ForeColor = System.Drawing.Color.LightYellow;
            this.labelMiMa.Location = new System.Drawing.Point(239, 187);
            this.labelMiMa.Name = "labelMiMa";
            this.labelMiMa.Size = new System.Drawing.Size(81, 26);
            this.labelMiMa.TabIndex = 2;
            this.labelMiMa.Text = "密码：";
            // 
            // textBoxUserID
            // 
            this.textBoxUserID.Font = new System.Drawing.Font("华文楷体", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxUserID.Location = new System.Drawing.Point(371, 122);
            this.textBoxUserID.Name = "textBoxUserID";
            this.textBoxUserID.Size = new System.Drawing.Size(143, 37);
            this.textBoxUserID.TabIndex = 1;
            this.textBoxUserID.Text = "admin";
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Font = new System.Drawing.Font("华文楷体", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxPassword.Location = new System.Drawing.Point(371, 183);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.Size = new System.Drawing.Size(143, 37);
            this.textBoxPassword.TabIndex = 4;
            this.textBoxPassword.Text = "admin";
            this.textBoxPassword.UseSystemPasswordChar = true;
            // 
            // btn_Login
            // 
            this.btn_Login.BackColor = System.Drawing.Color.Navy;
            this.btn_Login.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Login.Font = new System.Drawing.Font("华文楷体", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Login.ForeColor = System.Drawing.Color.LightYellow;
            this.btn_Login.Location = new System.Drawing.Point(244, 253);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(123, 39);
            this.btn_Login.TabIndex = 5;
            this.btn_Login.Text = "登录";
            this.btn_Login.UseVisualStyleBackColor = false;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // btn_Reset
            // 
            this.btn_Reset.BackColor = System.Drawing.Color.Navy;
            this.btn_Reset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_Reset.Font = new System.Drawing.Font("华文楷体", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_Reset.ForeColor = System.Drawing.Color.LightYellow;
            this.btn_Reset.Location = new System.Drawing.Point(407, 253);
            this.btn_Reset.Name = "btn_Reset";
            this.btn_Reset.Size = new System.Drawing.Size(123, 39);
            this.btn_Reset.TabIndex = 6;
            this.btn_Reset.Text = "重置";
            this.btn_Reset.UseVisualStyleBackColor = false;
            this.btn_Reset.Click += new System.EventHandler(this.btn_Reset_Click);
            // 
            // labelDengLuChengGong
            // 
            this.labelDengLuChengGong.AutoSize = true;
            this.labelDengLuChengGong.Font = new System.Drawing.Font("华文楷体", 24.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDengLuChengGong.ForeColor = System.Drawing.Color.LightYellow;
            this.labelDengLuChengGong.Location = new System.Drawing.Point(297, 27);
            this.labelDengLuChengGong.Name = "labelDengLuChengGong";
            this.labelDengLuChengGong.Size = new System.Drawing.Size(187, 37);
            this.labelDengLuChengGong.TabIndex = 7;
            this.labelDengLuChengGong.Text = "登录成功！";
            this.labelDengLuChengGong.Visible = false;
            // 
            // labelDangQianYongHu
            // 
            this.labelDangQianYongHu.Font = new System.Drawing.Font("华文楷体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelDangQianYongHu.ForeColor = System.Drawing.Color.LightYellow;
            this.labelDangQianYongHu.Location = new System.Drawing.Point(10, 339);
            this.labelDangQianYongHu.Name = "labelDangQianYongHu";
            this.labelDangQianYongHu.Size = new System.Drawing.Size(148, 30);
            this.labelDangQianYongHu.TabIndex = 8;
            this.labelDangQianYongHu.Text = "当前用户：";
            this.labelDangQianYongHu.Visible = false;
            // 
            // labelYongHuMing2
            // 
            this.labelYongHuMing2.Font = new System.Drawing.Font("华文楷体", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelYongHuMing2.ForeColor = System.Drawing.Color.LightYellow;
            this.labelYongHuMing2.Location = new System.Drawing.Point(164, 339);
            this.labelYongHuMing2.Name = "labelYongHuMing2";
            this.labelYongHuMing2.Size = new System.Drawing.Size(94, 30);
            this.labelYongHuMing2.TabIndex = 9;
            this.labelYongHuMing2.Text = "用户名";
            this.labelYongHuMing2.Visible = false;
            // 
            // btnTuiChuDengLu
            // 
            this.btnTuiChuDengLu.BackColor = System.Drawing.Color.Navy;
            this.btnTuiChuDengLu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTuiChuDengLu.Font = new System.Drawing.Font("华文楷体", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnTuiChuDengLu.ForeColor = System.Drawing.Color.LightYellow;
            this.btnTuiChuDengLu.Location = new System.Drawing.Point(357, 12);
            this.btnTuiChuDengLu.Name = "btnTuiChuDengLu";
            this.btnTuiChuDengLu.Size = new System.Drawing.Size(114, 36);
            this.btnTuiChuDengLu.TabIndex = 10;
            this.btnTuiChuDengLu.Text = "退出登录";
            this.btnTuiChuDengLu.UseVisualStyleBackColor = false;
            this.btnTuiChuDengLu.Visible = false;
            this.btnTuiChuDengLu.Click += new System.EventHandler(this.btnTuiChuDengLu_Click);
            // 
            // btnManageUser
            // 
            this.btnManageUser.BackColor = System.Drawing.Color.Navy;
            this.btnManageUser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnManageUser.Font = new System.Drawing.Font("华文楷体", 17.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnManageUser.ForeColor = System.Drawing.Color.LightYellow;
            this.btnManageUser.Location = new System.Drawing.Point(200, 12);
            this.btnManageUser.Name = "btnManageUser";
            this.btnManageUser.Size = new System.Drawing.Size(114, 36);
            this.btnManageUser.TabIndex = 24;
            this.btnManageUser.Text = "用户管理";
            this.btnManageUser.UseVisualStyleBackColor = false;
            this.btnManageUser.Visible = false;
            this.btnManageUser.Click += new System.EventHandler(this.btnManageUser_Click);
            // 
            // btnRuKu
            // 
            this.btnRuKu.BackColor = System.Drawing.Color.Navy;
            this.btnRuKu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnRuKu.Font = new System.Drawing.Font("华文楷体", 35F);
            this.btnRuKu.ForeColor = System.Drawing.Color.LightYellow;
            this.btnRuKu.Location = new System.Drawing.Point(183, 94);
            this.btnRuKu.Name = "btnRuKu";
            this.btnRuKu.Size = new System.Drawing.Size(101, 153);
            this.btnRuKu.TabIndex = 25;
            this.btnRuKu.Text = "入库";
            this.btnRuKu.UseVisualStyleBackColor = false;
            this.btnRuKu.Visible = false;
            this.btnRuKu.Click += new System.EventHandler(this.btnRuKu_Click);
            // 
            // btnChuKu
            // 
            this.btnChuKu.BackColor = System.Drawing.Color.Navy;
            this.btnChuKu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnChuKu.Font = new System.Drawing.Font("华文楷体", 35F);
            this.btnChuKu.ForeColor = System.Drawing.Color.LightYellow;
            this.btnChuKu.Location = new System.Drawing.Point(371, 94);
            this.btnChuKu.Name = "btnChuKu";
            this.btnChuKu.Size = new System.Drawing.Size(101, 153);
            this.btnChuKu.TabIndex = 26;
            this.btnChuKu.Text = "出库";
            this.btnChuKu.UseVisualStyleBackColor = false;
            this.btnChuKu.Visible = false;
            this.btnChuKu.Click += new System.EventHandler(this.btnChuKu_Click);
            // 
            // btnWeiZhi
            // 
            this.btnWeiZhi.BackColor = System.Drawing.Color.Navy;
            this.btnWeiZhi.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnWeiZhi.Font = new System.Drawing.Font("华文楷体", 17.25F);
            this.btnWeiZhi.ForeColor = System.Drawing.Color.LightYellow;
            this.btnWeiZhi.Location = new System.Drawing.Point(43, 12);
            this.btnWeiZhi.Name = "btnWeiZhi";
            this.btnWeiZhi.Size = new System.Drawing.Size(136, 39);
            this.btnWeiZhi.TabIndex = 27;
            this.btnWeiZhi.Text = "仓库管理";
            this.btnWeiZhi.UseVisualStyleBackColor = false;
            this.btnWeiZhi.Visible = false;
            this.btnWeiZhi.Click += new System.EventHandler(this.btnWeiZhi_Click);
            // 
            // btnSheBeiCanShu
            // 
            this.btnSheBeiCanShu.BackColor = System.Drawing.Color.Navy;
            this.btnSheBeiCanShu.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSheBeiCanShu.Font = new System.Drawing.Font("华文楷体", 17.25F);
            this.btnSheBeiCanShu.ForeColor = System.Drawing.Color.LightYellow;
            this.btnSheBeiCanShu.Location = new System.Drawing.Point(514, 12);
            this.btnSheBeiCanShu.Name = "btnSheBeiCanShu";
            this.btnSheBeiCanShu.Size = new System.Drawing.Size(114, 36);
            this.btnSheBeiCanShu.TabIndex = 28;
            this.btnSheBeiCanShu.Text = "设备参数";
            this.btnSheBeiCanShu.UseVisualStyleBackColor = false;
            this.btnSheBeiCanShu.Visible = false;
            this.btnSheBeiCanShu.Click += new System.EventHandler(this.btnSheBeiCanShu_Click);
            // 
            // btnExchange
            // 
            this.btnExchange.BackColor = System.Drawing.Color.Navy;
            this.btnExchange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnExchange.Font = new System.Drawing.Font("华文楷体", 35F);
            this.btnExchange.ForeColor = System.Drawing.Color.LightYellow;
            this.btnExchange.Location = new System.Drawing.Point(559, 94);
            this.btnExchange.Name = "btnExchange";
            this.btnExchange.Size = new System.Drawing.Size(101, 153);
            this.btnExchange.TabIndex = 29;
            this.btnExchange.Text = "调库";
            this.btnExchange.UseVisualStyleBackColor = false;
            this.btnExchange.Visible = false;
            this.btnExchange.Click += new System.EventHandler(this.btnExchange_Click);
            // 
            // loginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.ClientSize = new System.Drawing.Size(665, 374);
            this.Controls.Add(this.btnExchange);
            this.Controls.Add(this.btnSheBeiCanShu);
            this.Controls.Add(this.btnWeiZhi);
            this.Controls.Add(this.btnChuKu);
            this.Controls.Add(this.btnRuKu);
            this.Controls.Add(this.btnManageUser);
            this.Controls.Add(this.btnTuiChuDengLu);
            this.Controls.Add(this.labelYongHuMing2);
            this.Controls.Add(this.labelDangQianYongHu);
            this.Controls.Add(this.labelDengLuChengGong);
            this.Controls.Add(this.btn_Reset);
            this.Controls.Add(this.btn_Login);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxUserID);
            this.Controls.Add(this.labelMiMa);
            this.Controls.Add(this.labelYongHuMing1);
            this.Controls.Add(this.labelYongHuDengLu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "loginForm";
            this.Text = "loginForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.loginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelYongHuDengLu;
        private System.Windows.Forms.Label labelYongHuMing1;
        private System.Windows.Forms.Label labelMiMa;
        private System.Windows.Forms.TextBox textBoxUserID;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Button btn_Login;
        private System.Windows.Forms.Button btn_Reset;
        private System.Windows.Forms.Label labelDengLuChengGong;
        private System.Windows.Forms.Label labelDangQianYongHu;
        private System.Windows.Forms.Label labelYongHuMing2;
        private System.Windows.Forms.Button btnTuiChuDengLu;
        private System.Windows.Forms.Label lblIP;
        private System.Windows.Forms.Label lblPLCjiange;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.TextBox txtPLCjiange;
        private System.Windows.Forms.Timer timerLogin;
        private System.Windows.Forms.Button btnManageUser;
        private System.Windows.Forms.Button btnRuKu;
        private System.Windows.Forms.Button btnChuKu;
        private System.Windows.Forms.Button btnWeiZhi;
        private System.Windows.Forms.Button btnSheBeiCanShu;
        private System.Windows.Forms.Button btnExchange;
    }
}