namespace 自动化库存管理
{
    partial class KuWeiForm
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
            this.components = new System.ComponentModel.Container();
            this.btnWanCheng = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnMin = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTipKuCun = new System.Windows.Forms.ToolTip(this.components);
            this.labelKuWei = new System.Windows.Forms.Label();
            this.labelWeight = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnWanCheng
            // 
            this.btnWanCheng.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnWanCheng.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnWanCheng.Font = new System.Drawing.Font("楷体", 13.85714F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnWanCheng.ForeColor = System.Drawing.SystemColors.Control;
            this.btnWanCheng.Location = new System.Drawing.Point(930, 660);
            this.btnWanCheng.Margin = new System.Windows.Forms.Padding(2);
            this.btnWanCheng.Name = "btnWanCheng";
            this.btnWanCheng.Size = new System.Drawing.Size(174, 25);
            this.btnWanCheng.TabIndex = 14;
            this.btnWanCheng.Text = "完成";
            this.btnWanCheng.UseVisualStyleBackColor = false;
            this.btnWanCheng.Click += new System.EventHandler(this.btnWanCheng_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("楷体", 13.85714F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.Control;
            this.btnCancel.Location = new System.Drawing.Point(747, 660);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(174, 25);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnDelete.Location = new System.Drawing.Point(1103, 0);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(2);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(30, 30);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "×";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnMin
            // 
            this.btnMin.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnMin.Location = new System.Drawing.Point(1074, 0);
            this.btnMin.Margin = new System.Windows.Forms.Padding(2);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(30, 30);
            this.btnMin.TabIndex = 11;
            this.btnMin.Text = "—";
            this.btnMin.UseVisualStyleBackColor = true;
            this.btnMin.Click += new System.EventHandler(this.btnMin_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("楷体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(433, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 35);
            this.label1.TabIndex = 15;
            this.label1.Text = "库位示意图";
            // 
            // labelKuWei
            // 
            this.labelKuWei.AutoSize = true;
            this.labelKuWei.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelKuWei.Location = new System.Drawing.Point(431, 50);
            this.labelKuWei.Name = "labelKuWei";
            this.labelKuWei.Size = new System.Drawing.Size(0, 20);
            this.labelKuWei.TabIndex = 16;
            // 
            // labelWeight
            // 
            this.labelWeight.AutoSize = true;
            this.labelWeight.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelWeight.Location = new System.Drawing.Point(624, 50);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(0, 20);
            this.labelWeight.TabIndex = 17;
            // 
            // KuWeiForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1134, 700);
            this.Controls.Add(this.labelWeight);
            this.Controls.Add(this.labelKuWei);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnWanCheng);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnMin);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "KuWeiForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "库位示意图";
            this.Load += new System.EventHandler(this.KuWeiForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnWanCheng;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTipKuCun;
        private System.Windows.Forms.Label labelKuWei;
        private System.Windows.Forms.Label labelWeight;
    }
}