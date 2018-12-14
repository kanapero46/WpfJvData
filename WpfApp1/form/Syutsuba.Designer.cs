namespace WpfApp1.form
{
    partial class Syutsuba
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.枠 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bamei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Minarai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Jockey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Futan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.LabeCource = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.枠,
            this.Uma,
            this.Bamei,
            this.Minarai,
            this.Jockey,
            this.Futan,
            this.M});
            this.dataGridView1.Location = new System.Drawing.Point(12, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1125, 405);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // 枠
            // 
            this.枠.Frozen = true;
            this.枠.HeaderText = "枠";
            this.枠.MaxInputLength = 3;
            this.枠.Name = "枠";
            this.枠.ReadOnly = true;
            this.枠.Width = 50;
            // 
            // Uma
            // 
            this.Uma.HeaderText = "馬";
            this.Uma.MaxInputLength = 3;
            this.Uma.Name = "Uma";
            this.Uma.ReadOnly = true;
            this.Uma.Width = 50;
            // 
            // Bamei
            // 
            this.Bamei.HeaderText = "馬名";
            this.Bamei.Name = "Bamei";
            this.Bamei.ReadOnly = true;
            this.Bamei.Width = 300;
            // 
            // Minarai
            // 
            this.Minarai.HeaderText = "";
            this.Minarai.Name = "Minarai";
            this.Minarai.ReadOnly = true;
            this.Minarai.Width = 50;
            // 
            // Jockey
            // 
            this.Jockey.HeaderText = "騎手";
            this.Jockey.Name = "Jockey";
            this.Jockey.ReadOnly = true;
            this.Jockey.Width = 200;
            // 
            // Futan
            // 
            this.Futan.HeaderText = "負担重量";
            this.Futan.Name = "Futan";
            this.Futan.ReadOnly = true;
            // 
            // M
            // 
            this.M.HeaderText = "父";
            this.M.Name = "M";
            this.M.ReadOnly = true;
            this.M.Width = 330;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayoutPanel1.Controls.Add(this.LabeCource);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(735, 32);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // LabeCource
            // 
            this.LabeCource.AutoSize = true;
            this.LabeCource.Location = new System.Drawing.Point(3, 0);
            this.LabeCource.Name = "LabeCource";
            this.LabeCource.Size = new System.Drawing.Size(35, 12);
            this.LabeCource.TabIndex = 0;
            this.LabeCource.Text = "label1";
            // 
            // Syutsuba
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1141, 459);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Syutsuba";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 枠;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uma;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bamei;
        private System.Windows.Forms.DataGridViewTextBoxColumn Minarai;
        private System.Windows.Forms.DataGridViewTextBoxColumn Jockey;
        private System.Windows.Forms.DataGridViewTextBoxColumn Futan;
        private System.Windows.Forms.DataGridViewTextBoxColumn M;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label LabeCource;
    }
}