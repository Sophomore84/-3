namespace 自动化库存管理
{
    partial class SearchForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSearch = new System.Windows.Forms.Button();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.dataGridViewSearch = new System.Windows.Forms.DataGridView();
            this.radioButtonBianMa = new System.Windows.Forms.RadioButton();
            this.radioButtonMingCheng = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.Location = new System.Drawing.Point(419, 53);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(148, 46);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "搜索";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxSearch.Location = new System.Drawing.Point(12, 63);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(382, 30);
            this.textBoxSearch.TabIndex = 2;
            this.textBoxSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSearch_KeyDown);
            // 
            // dataGridViewSearch
            // 
            this.dataGridViewSearch.AllowUserToAddRows = false;
            this.dataGridViewSearch.AllowUserToDeleteRows = false;
            this.dataGridViewSearch.AllowUserToOrderColumns = true;
            this.dataGridViewSearch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSearch.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewSearch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSearch.Location = new System.Drawing.Point(12, 117);
            this.dataGridViewSearch.MultiSelect = false;
            this.dataGridViewSearch.Name = "dataGridViewSearch";
            this.dataGridViewSearch.ReadOnly = true;
            this.dataGridViewSearch.RowHeadersWidth = 4;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridViewSearch.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewSearch.RowTemplate.Height = 23;
            this.dataGridViewSearch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSearch.Size = new System.Drawing.Size(565, 405);
            this.dataGridViewSearch.TabIndex = 3;
            this.dataGridViewSearch.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewSearch_CellDoubleClick);
            this.dataGridViewSearch.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridViewSearch_Scroll);
            // 
            // radioButtonBianMa
            // 
            this.radioButtonBianMa.AutoSize = true;
            this.radioButtonBianMa.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButtonBianMa.Location = new System.Drawing.Point(13, 12);
            this.radioButtonBianMa.Name = "radioButtonBianMa";
            this.radioButtonBianMa.Size = new System.Drawing.Size(67, 24);
            this.radioButtonBianMa.TabIndex = 4;
            this.radioButtonBianMa.TabStop = true;
            this.radioButtonBianMa.Text = "编码";
            this.radioButtonBianMa.UseVisualStyleBackColor = true;
            this.radioButtonBianMa.CheckedChanged += new System.EventHandler(this.radioButtonMingCheng_CheckedChanged);
            // 
            // radioButtonMingCheng
            // 
            this.radioButtonMingCheng.AutoSize = true;
            this.radioButtonMingCheng.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.radioButtonMingCheng.Location = new System.Drawing.Point(121, 12);
            this.radioButtonMingCheng.Name = "radioButtonMingCheng";
            this.radioButtonMingCheng.Size = new System.Drawing.Size(67, 24);
            this.radioButtonMingCheng.TabIndex = 5;
            this.radioButtonMingCheng.TabStop = true;
            this.radioButtonMingCheng.Text = "名称";
            this.radioButtonMingCheng.UseVisualStyleBackColor = true;
            this.radioButtonMingCheng.CheckedChanged += new System.EventHandler(this.radioButtonMingCheng_CheckedChanged);
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(579, 522);
            this.Controls.Add(this.radioButtonMingCheng);
            this.Controls.Add(this.radioButtonBianMa);
            this.Controls.Add(this.dataGridViewSearch);
            this.Controls.Add(this.textBoxSearch);
            this.Controls.Add(this.btnSearch);
            this.Font = new System.Drawing.Font("楷体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "SearchForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "物品检索";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SearchForm_FormClosed);
            this.Load += new System.EventHandler(this.SearchForm_Load);
            this.Shown += new System.EventHandler(this.SearchForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox textBoxSearch;
        private System.Windows.Forms.DataGridView dataGridViewSearch;
        private System.Windows.Forms.RadioButton radioButtonBianMa;
        private System.Windows.Forms.RadioButton radioButtonMingCheng;
    }
}