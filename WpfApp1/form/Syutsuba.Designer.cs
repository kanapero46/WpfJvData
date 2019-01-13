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
            this.LabelRaceName = new System.Windows.Forms.Label();
            this.LabelTrack = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.button2 = new System.Windows.Forms.Button();
            this.枠 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bamei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Minarai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Jockey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Futan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MFColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MMFColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FFM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.DMStatus = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.枠,
            this.Uma,
            this.Bamei,
            this.DM,
            this.TM,
            this.Minarai,
            this.Jockey,
            this.Futan,
            this.FColor,
            this.M,
            this.MFColor,
            this.FM,
            this.MMFColor,
            this.FFM});
            this.dataGridView1.Location = new System.Drawing.Point(9, 146);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(2438, 980);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.flowLayoutPanel1.Controls.Add(this.LabelCource);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(9, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(620, 134);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // LabelCource
            // 
            this.LabelCource.AutoSize = true;
            this.LabelCource.Font = new System.Drawing.Font("メイリオ", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LabelCource.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.LabelCource.Location = new System.Drawing.Point(7, 0);
            this.LabelCource.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LabelCource.Name = "LabelCource";
            this.LabelCource.Size = new System.Drawing.Size(540, 144);
            this.LabelCource.TabIndex = 0;
            this.LabelCource.Text = "東京１１Ｒ";
            this.LabelCource.Click += new System.EventHandler(this.LabelCource_Click);
            // 
            // LabelRaceName
            // 
            this.LabelRaceName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelRaceName.AutoSize = true;
            this.LabelRaceName.Cursor = System.Windows.Forms.Cursors.Cross;
            this.LabelRaceName.Font = new System.Drawing.Font("メイリオ", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LabelRaceName.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.LabelRaceName.Location = new System.Drawing.Point(7, 0);
            this.LabelRaceName.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LabelRaceName.Name = "LabelRaceName";
            this.LabelRaceName.Size = new System.Drawing.Size(169, 72);
            this.LabelRaceName.TabIndex = 3;
            this.LabelRaceName.Text = "label1";
            this.LabelRaceName.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.LabelRaceName.Click += new System.EventHandler(this.LabelRaceName_Click);
            // 
            // LabelTrack
            // 
            this.LabelTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LabelTrack.AutoSize = true;
            this.LabelTrack.Font = new System.Drawing.Font("Meiryo UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.LabelTrack.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.LabelTrack.Location = new System.Drawing.Point(7, 0);
            this.LabelTrack.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.LabelTrack.Name = "LabelTrack";
            this.LabelTrack.Size = new System.Drawing.Size(166, 61);
            this.LabelTrack.TabIndex = 4;
            this.LabelTrack.Text = "label1";
            this.LabelTrack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button1.ForeColor = System.Drawing.Color.Red;
            this.button1.Location = new System.Drawing.Point(2284, 24);
            this.button1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 64);
            this.button1.TabIndex = 2;
            this.button1.Text = "✕";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel2.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.flowLayoutPanel2.Controls.Add(this.LabelRaceName);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(611, 0);
            this.flowLayoutPanel2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(1300, 64);
            this.flowLayoutPanel2.TabIndex = 5;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.flowLayoutPanel3.Controls.Add(this.LabelTrack);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(611, 64);
            this.flowLayoutPanel3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(1300, 70);
            this.flowLayoutPanel3.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(26, 1138);
            this.button2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(163, 64);
            this.button2.TabIndex = 7;
            this.button2.Text = "DM取得\r\n";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            // 
            // Bamei
            // 
            this.Bamei.HeaderText = "馬名";
            this.Bamei.Name = "Bamei";
            this.Bamei.ReadOnly = true;
            this.Bamei.Width = 350;
            // 
            // DM
            // 
            this.DM.HeaderText = "対戦型DM";
            this.DM.Name = "DM";
            this.DM.ReadOnly = true;
            this.DM.Visible = false;
            this.DM.Width = 200;
            // 
            // TM
            // 
            this.TM.HeaderText = "タイム型";
            this.TM.Name = "TM";
            this.TM.ReadOnly = true;
            this.TM.Visible = false;
            this.TM.Width = 200;
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
            this.Jockey.Width = 200;
            // 
            // Futan
            // 
            this.Futan.HeaderText = "負担重量";
            this.Futan.Name = "Futan";
            this.Futan.ReadOnly = true;
            this.Futan.Width = 200;
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
            this.M.Width = 250;
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
            this.FM.Width = 250;
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
            this.FFM.Width = 250;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(1921, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 24);
            this.label1.TabIndex = 8;
            this.label1.Text = "DM情報：";
            this.label1.Visible = false;
            // 
            // DMStatus
            // 
            this.DMStatus.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.DMStatus.AutoSize = true;
            this.DMStatus.Enabled = false;
            this.DMStatus.Location = new System.Drawing.Point(2021, 13);
            this.DMStatus.Name = "DMStatus";
            this.DMStatus.Size = new System.Drawing.Size(226, 24);
            this.DMStatus.TabIndex = 9;
            this.DMStatus.Text = "天候馬場状態発表後";
            this.DMStatus.Visible = false;
            // 
            // Syutsuba
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(2472, 1226);
            this.Controls.Add(this.DMStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.flowLayoutPanel3);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.dataGridView1);
            this.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.Name = "Syutsuba";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label LabelCource;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label LabelTrack;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
        private System.Windows.Forms.Label LabelRaceName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 枠;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uma;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bamei;
        private System.Windows.Forms.DataGridViewTextBoxColumn DM;
        private System.Windows.Forms.DataGridViewTextBoxColumn TM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Minarai;
        private System.Windows.Forms.DataGridViewTextBoxColumn Jockey;
        private System.Windows.Forms.DataGridViewTextBoxColumn Futan;
        private System.Windows.Forms.DataGridViewTextBoxColumn FColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn M;
        private System.Windows.Forms.DataGridViewTextBoxColumn MFColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FM;
        private System.Windows.Forms.DataGridViewTextBoxColumn MMFColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FFM;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DMStatus;
    }
}