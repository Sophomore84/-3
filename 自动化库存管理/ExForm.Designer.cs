namespace 自动化库存管理
{
    partial class ExForm
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
            this.textBoxWeiZhi = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxWeiZhi1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnYes = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxWeiZhi
            // 
            this.textBoxWeiZhi.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxWeiZhi.Location = new System.Drawing.Point(100, 55);
            this.textBoxWeiZhi.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxWeiZhi.Name = "textBoxWeiZhi";
            this.textBoxWeiZhi.Size = new System.Drawing.Size(101, 30);
            this.textBoxWeiZhi.TabIndex = 13;
            this.textBoxWeiZhi.Click += new System.EventHandler(this.textBoxWeiZhi_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Lavender;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(54, 63);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 14);
            this.label9.TabIndex = 12;
            this.label9.Text = "位置:";
            // 
            // textBoxWeiZhi1
            // 
            this.textBoxWeiZhi1.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxWeiZhi1.Location = new System.Drawing.Point(277, 55);
            this.textBoxWeiZhi1.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxWeiZhi1.Name = "textBoxWeiZhi1";
            this.textBoxWeiZhi1.Size = new System.Drawing.Size(100, 30);
            this.textBoxWeiZhi1.TabIndex = 15;
            this.textBoxWeiZhi1.Click += new System.EventHandler(this.textBoxWeiZhi1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Lavender;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(231, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 14);
            this.label1.TabIndex = 14;
            this.label1.Text = "位置:";
            // 
            // btnYes
            // 
            this.btnYes.Location = new System.Drawing.Point(198, 109);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 16;
            this.btnYes.Text = "确认调库";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // ExForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumSlateBlue;
            this.ClientSize = new System.Drawing.Size(490, 163);
            this.Controls.Add(this.btnYes);
            this.Controls.Add(this.textBoxWeiZhi1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxWeiZhi);
            this.Controls.Add(this.label9);
            this.Name = "ExForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "调库";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxWeiZhi;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxWeiZhi1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnYes;
    }
}