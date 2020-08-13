namespace 自动化库存管理
{
    partial class ManageAccountForm
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
            this.dataGridViewUser = new System.Windows.Forms.DataGridView();
            this.lblAccount = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblWorkType = new System.Windows.Forms.Label();
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.txtPassWord = new System.Windows.Forms.TextBox();
            this.btn_AddUser = new System.Windows.Forms.Button();
            this.btn_ModifyUser = new System.Windows.Forms.Button();
            this.btn_DeleteUser = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxWorkType = new System.Windows.Forms.ComboBox();
            this.btnCloseAccount = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUser)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewUser
            // 
            this.dataGridViewUser.AllowUserToAddRows = false;
            this.dataGridViewUser.AllowUserToDeleteRows = false;
            this.dataGridViewUser.BackgroundColor = System.Drawing.Color.White;
            this.dataGridViewUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUser.Location = new System.Drawing.Point(2, 213);
            this.dataGridViewUser.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewUser.MultiSelect = false;
            this.dataGridViewUser.Name = "dataGridViewUser";
            this.dataGridViewUser.ReadOnly = true;
            this.dataGridViewUser.RowTemplate.Height = 33;
            this.dataGridViewUser.Size = new System.Drawing.Size(589, 241);
            this.dataGridViewUser.TabIndex = 0;
            this.dataGridViewUser.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewUser_RowHeaderMouseClick);
            // 
            // lblAccount
            // 
            this.lblAccount.AutoSize = true;
            this.lblAccount.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.lblAccount.ForeColor = System.Drawing.Color.LightYellow;
            this.lblAccount.Location = new System.Drawing.Point(20, 34);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(67, 21);
            this.lblAccount.TabIndex = 12;
            this.lblAccount.Text = "帐号：";
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.lblPassword.ForeColor = System.Drawing.Color.LightYellow;
            this.lblPassword.Location = new System.Drawing.Point(320, 58);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(67, 21);
            this.lblPassword.TabIndex = 13;
            this.lblPassword.Text = "密码：";
            // 
            // lblWorkType
            // 
            this.lblWorkType.AutoSize = true;
            this.lblWorkType.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.lblWorkType.ForeColor = System.Drawing.Color.LightYellow;
            this.lblWorkType.Location = new System.Drawing.Point(40, 107);
            this.lblWorkType.Name = "lblWorkType";
            this.lblWorkType.Size = new System.Drawing.Size(67, 21);
            this.lblWorkType.TabIndex = 14;
            this.lblWorkType.Text = "类型：";
            // 
            // txtAccount
            // 
            this.txtAccount.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.txtAccount.Location = new System.Drawing.Point(104, 31);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(157, 32);
            this.txtAccount.TabIndex = 22;
            // 
            // txtPassWord
            // 
            this.txtPassWord.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.txtPassWord.Location = new System.Drawing.Point(368, 31);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.Size = new System.Drawing.Size(157, 32);
            this.txtPassWord.TabIndex = 23;
            // 
            // btn_AddUser
            // 
            this.btn_AddUser.BackColor = System.Drawing.Color.Navy;
            this.btn_AddUser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_AddUser.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.btn_AddUser.ForeColor = System.Drawing.Color.LightYellow;
            this.btn_AddUser.Location = new System.Drawing.Point(20, 169);
            this.btn_AddUser.Name = "btn_AddUser";
            this.btn_AddUser.Size = new System.Drawing.Size(109, 29);
            this.btn_AddUser.TabIndex = 25;
            this.btn_AddUser.Text = "增加";
            this.btn_AddUser.UseVisualStyleBackColor = false;
            this.btn_AddUser.Click += new System.EventHandler(this.btn_AddUser_Click);
            // 
            // btn_ModifyUser
            // 
            this.btn_ModifyUser.BackColor = System.Drawing.Color.Navy;
            this.btn_ModifyUser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_ModifyUser.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.btn_ModifyUser.ForeColor = System.Drawing.Color.LightYellow;
            this.btn_ModifyUser.Location = new System.Drawing.Point(228, 169);
            this.btn_ModifyUser.Name = "btn_ModifyUser";
            this.btn_ModifyUser.Size = new System.Drawing.Size(109, 29);
            this.btn_ModifyUser.TabIndex = 26;
            this.btn_ModifyUser.Text = "更新";
            this.btn_ModifyUser.UseVisualStyleBackColor = false;
            this.btn_ModifyUser.Click += new System.EventHandler(this.btn_ModifyUser_Click);
            // 
            // btn_DeleteUser
            // 
            this.btn_DeleteUser.BackColor = System.Drawing.Color.Navy;
            this.btn_DeleteUser.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_DeleteUser.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.btn_DeleteUser.ForeColor = System.Drawing.Color.LightYellow;
            this.btn_DeleteUser.Location = new System.Drawing.Point(431, 169);
            this.btn_DeleteUser.Name = "btn_DeleteUser";
            this.btn_DeleteUser.Size = new System.Drawing.Size(109, 29);
            this.btn_DeleteUser.TabIndex = 27;
            this.btn_DeleteUser.Text = "删除";
            this.btn_DeleteUser.UseVisualStyleBackColor = false;
            this.btn_DeleteUser.Click += new System.EventHandler(this.btn_DeleteUser_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.Navy;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReset.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.btnReset.ForeColor = System.Drawing.Color.LightYellow;
            this.btnReset.Location = new System.Drawing.Point(368, 81);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(153, 29);
            this.btnReset.TabIndex = 28;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxWorkType);
            this.groupBox1.Controls.Add(this.btnReset);
            this.groupBox1.Controls.Add(this.txtPassWord);
            this.groupBox1.Controls.Add(this.txtAccount);
            this.groupBox1.Controls.Add(this.lblAccount);
            this.groupBox1.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.groupBox1.ForeColor = System.Drawing.Color.LightYellow;
            this.groupBox1.Location = new System.Drawing.Point(20, 25);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(547, 129);
            this.groupBox1.TabIndex = 29;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "编辑";
            // 
            // comboBoxWorkType
            // 
            this.comboBoxWorkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxWorkType.Font = new System.Drawing.Font("华文楷体", 14.25F);
            this.comboBoxWorkType.FormattingEnabled = true;
            this.comboBoxWorkType.Location = new System.Drawing.Point(104, 81);
            this.comboBoxWorkType.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxWorkType.Name = "comboBoxWorkType";
            this.comboBoxWorkType.Size = new System.Drawing.Size(158, 29);
            this.comboBoxWorkType.TabIndex = 33;
            // 
            // btnCloseAccount
            // 
            this.btnCloseAccount.Location = new System.Drawing.Point(571, 1);
            this.btnCloseAccount.Name = "btnCloseAccount";
            this.btnCloseAccount.Size = new System.Drawing.Size(20, 20);
            this.btnCloseAccount.TabIndex = 32;
            this.btnCloseAccount.Text = "×";
            this.btnCloseAccount.UseVisualStyleBackColor = true;
            this.btnCloseAccount.Click += new System.EventHandler(this.btnCloseAccount_Click);
            // 
            // ManageAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Green;
            this.ClientSize = new System.Drawing.Size(592, 450);
            this.Controls.Add(this.btnCloseAccount);
            this.Controls.Add(this.btn_DeleteUser);
            this.Controls.Add(this.btn_ModifyUser);
            this.Controls.Add(this.btn_AddUser);
            this.Controls.Add(this.lblWorkType);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.dataGridViewUser);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ManageAccountForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ManageAccountForm";
            this.Load += new System.EventHandler(this.ManageAccountForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUser)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewUser;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblWorkType;
        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.TextBox txtPassWord;
        private System.Windows.Forms.Button btn_AddUser;
        private System.Windows.Forms.Button btn_ModifyUser;
        private System.Windows.Forms.Button btn_DeleteUser;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnCloseAccount;
        private System.Windows.Forms.ComboBox comboBoxWorkType;
    }
}