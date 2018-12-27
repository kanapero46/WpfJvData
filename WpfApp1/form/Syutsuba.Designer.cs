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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.LabelCource = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.LabelRaceName = new System.Windows.Forms.Label();
            this.LabelTrack = new System.Windows.Forms.Label();
            this.枠 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bamei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Minarai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Jockey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Futan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MFColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MMFColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FFM = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.FColor,
            this.M,
            this.MFColor,
            this.FM,
            this.MMFColor,
            this.FFM});
            this.dataGridView1.Location = new System.Drawing.Point(12, 50);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1125, 405);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayoutPanel1.Controls.Add(this.LabelCource);
            this.flowLayoutPanel1.Controls.Add(this.LabelRaceName);
            this.flowLayoutPanel1.Controls.Add(this.LabelTrack);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 12);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(974, 32);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // LabelCource
            // 
            this.LabelCource.AutoSize = true;
            this.LabelCource.Font = new System.Drawing.Font("Meiryo UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LabelCource.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.LabelCource.Location = new System.Drawing.Point(3, 0);
            this.LabelCource.Name = "LabelCource";
            this.LabelCource.Size = new System.Drawing.Size(89, 30);
            this.LabelCource.TabIndex = 0;
            this.LabelCource.Text = "label1";
            this.LabelCource.Click += new System.EventHandler(this.LabelCource_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(1054, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "✕";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LabelRaceName
            // 
            this.LabelRaceName.AutoSize = true;
            this.LabelRaceName.Font = new System.Drawing.Font("Meiryo UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LabelRaceName.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.LabelRaceName.Location = new System.Drawing.Point(98, 0);
            this.LabelRaceName.Name = "LabelRaceName";
            this.LabelRaceName.Size = new System.Drawing.Size(83, 30);
            this.LabelRaceName.TabIndex = 3;
            this.LabelRaceName.Text = "label1";
            // 
            // LabelTrack
            // 
            this.LabelTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelTrack.AutoSize = true;
            this.LabelTrack.Font = new System.Drawing.Font("Meiryo UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LabelTrack.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.LabelTrack.Location = new System.Drawing.Point(187, 0);
            this.LabelTrack.Name = "LabelTrack";
            this.LabelTrack.Size = new System.Drawing.Size(83, 30);
            this.LabelTrack.TabIndex = 4;
            this.LabelTrack.Text = "label1";
            this.LabelTrack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // 枠
            // 
            this.枠.Frozen = true;
            this.枠.HeaderText = "枠";
            this.枠.MaxInputLength = 3;
            this.枠.Name = "枠";
            this.枠.ReadOnly = true;
            this.枠.Width = 30;
            // 
            // Uma
            // 
            this.Uma.HeaderText = "馬";
            this.Uma.MaxInputLength = 3;
            this.Uma.Name = "Uma";
            this.Uma.ReadOnly = true;
            this.Uma.Width = 30;
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
            this.Minarai.Width = 30;
            // 
            // Jockey
            // 
            this.Jockey.HeaderText = "騎手";
            this.Jockey.Name = "Jockey";
            this.Jockey.ReadOnly = true;
            // 
            // Futan
            // 
            this.Futan.HeaderText = "負担重量";
            this.Futan.Name = "Futan";
            this.Futan.ReadOnly = true;
            // 
            // FColor
            // 
            this.FColor.HeaderText = "";
            this.FColor.Name = "FColor";
            this.FColor.ReadOnly = true;
            this.FColor.Width = 10;
            // 
            // M
            // 
            this.M.HeaderText = "父";
            this.M.Name = "M";
            this.M.ReadOnly = true;
            this.M.Width = 150;
            // 
            // MFColor
            // 
            this.MFColor.HeaderText = "";
            this.MFColor.Name = "MFColor";
            this.MFColor.ReadOnly = true;
            this.MFColor.Width = 10;
            // 
            // FM
            // 
            this.FM.HeaderText = "母父";
            this.FM.Name = "FM";
            this.FM.ReadOnly = true;
            this.FM.Width = 150;
            // 
            // MMFColor
            // 
            this.MMFColor.HeaderText = "";
            this.MMFColor.Name = "MMFColor";
            this.MMFColor.ReadOnly = true;
            this.MMFColor.Width = 10;
            // 
            // FFM
            // 
            this.FFM.HeaderText = "母母父";
            this.FFM.Name = "FFM";
            this.FFM.ReadOnly = true;
            this.FFM.Width = 150;
            // 
            // Syutsuba
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1141, 459);
            this.Controls.Add(this.button1);
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
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label LabelCource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label LabelRaceName;
        private System.Windows.Forms.Label LabelTrack;
        private System.Windows.Forms.DataGridViewTextBoxColumn 枠;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uma;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bamei;
        private System.Windows.Forms.DataGridViewTextBoxColumn Minarai;
        private System.Windows.Forms.DataGridViewTextBoxColumn Jockey;
        private System.Windows.Forms.DataGridViewTextBoxColumn Futan;
        private System.Windows.Forms.DataGridViewTextBoxColumn FColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn M;
        private System.Windows.Forms.DataGridViewTextBoxColumn MFColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FM;
        private System.Windows.Forms.DataGridViewTextBoxColumn MMFColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FFM;
    }
}