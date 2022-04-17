namespace WpfApp1.form.data
{
    partial class DataList
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
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Race = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Jomei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Baba = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Rank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ninki = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bamei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Jockey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Futan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Father = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mother = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FnoColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MFType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BloodNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.button3 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "レースデータ検索";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(112, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(861, 19);
            this.textBox1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader;
            this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.date,
            this.Race,
            this.Jomei,
            this.Distance,
            this.Baba,
            this.Rank,
            this.Ninki,
            this.Bamei,
            this.Jockey,
            this.Futan,
            this.FType,
            this.Father,
            this.Mother,
            this.FnoColor,
            this.FNo,
            this.MFType,
            this.MF,
            this.BloodNumber});
            this.dataGridView1.Location = new System.Drawing.Point(7, 44);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1250, 424);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // Number
            // 
            this.Number.HeaderText = "No";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Visible = false;
            this.Number.Width = 5;
            // 
            // date
            // 
            this.date.HeaderText = "日付";
            this.date.Name = "date";
            this.date.ReadOnly = true;
            this.date.Width = 21;
            // 
            // Race
            // 
            this.Race.HeaderText = "レース名";
            this.Race.Name = "Race";
            this.Race.ReadOnly = true;
            this.Race.Width = 21;
            // 
            // Jomei
            // 
            this.Jomei.HeaderText = "場名";
            this.Jomei.Name = "Jomei";
            this.Jomei.ReadOnly = true;
            this.Jomei.Width = 21;
            // 
            // Distance
            // 
            this.Distance.HeaderText = "距離S";
            this.Distance.Name = "Distance";
            this.Distance.ReadOnly = true;
            this.Distance.Width = 21;
            // 
            // Baba
            // 
            this.Baba.HeaderText = "馬場";
            this.Baba.Name = "Baba";
            this.Baba.ReadOnly = true;
            this.Baba.Width = 21;
            // 
            // Rank
            // 
            this.Rank.HeaderText = "着順";
            this.Rank.Name = "Rank";
            this.Rank.ReadOnly = true;
            this.Rank.Width = 21;
            // 
            // Ninki
            // 
            this.Ninki.HeaderText = "人気";
            this.Ninki.Name = "Ninki";
            this.Ninki.ReadOnly = true;
            this.Ninki.Width = 21;
            // 
            // Bamei
            // 
            this.Bamei.HeaderText = "馬名";
            this.Bamei.Name = "Bamei";
            this.Bamei.ReadOnly = true;
            this.Bamei.Width = 21;
            // 
            // Jockey
            // 
            this.Jockey.HeaderText = "騎手";
            this.Jockey.Name = "Jockey";
            this.Jockey.ReadOnly = true;
            this.Jockey.Width = 21;
            // 
            // Futan
            // 
            this.Futan.HeaderText = "負担";
            this.Futan.Name = "Futan";
            this.Futan.ReadOnly = true;
            this.Futan.Width = 21;
            // 
            // FType
            // 
            this.FType.HeaderText = "";
            this.FType.Name = "FType";
            this.FType.ReadOnly = true;
            this.FType.Width = 21;
            // 
            // Father
            // 
            this.Father.HeaderText = "父";
            this.Father.Name = "Father";
            this.Father.ReadOnly = true;
            this.Father.Width = 21;
            // 
            // Mother
            // 
            this.Mother.HeaderText = "母";
            this.Mother.Name = "Mother";
            this.Mother.ReadOnly = true;
            this.Mother.Width = 21;
            // 
            // FnoColor
            // 
            this.FnoColor.HeaderText = "";
            this.FnoColor.Name = "FnoColor";
            this.FnoColor.ReadOnly = true;
            this.FnoColor.Visible = false;
            this.FnoColor.Width = 21;
            // 
            // FNo
            // 
            this.FNo.HeaderText = "F-No.";
            this.FNo.Name = "FNo";
            this.FNo.ReadOnly = true;
            this.FNo.Visible = false;
            this.FNo.Width = 21;
            // 
            // MFType
            // 
            this.MFType.HeaderText = "";
            this.MFType.Name = "MFType";
            this.MFType.ReadOnly = true;
            this.MFType.Width = 21;
            // 
            // MF
            // 
            this.MF.HeaderText = "母父";
            this.MF.Name = "MF";
            this.MF.ReadOnly = true;
            this.MF.Width = 21;
            // 
            // BloodNumber
            // 
            this.BloodNumber.HeaderText = "血統登録番号";
            this.BloodNumber.Name = "BloodNumber";
            this.BloodNumber.ReadOnly = true;
            this.BloodNumber.Visible = false;
            this.BloodNumber.Width = 21;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 35);
            this.button1.TabIndex = 3;
            this.button1.Text = "閉じる";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(84, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 35);
            this.button2.TabIndex = 4;
            this.button2.Text = "F-No取得";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 534);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1260, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(92, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(7, 488);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(600, 43);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(992, 2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(131, 35);
            this.button3.TabIndex = 5;
            this.button3.Text = "ファイル指定";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(1158, 12);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(67, 16);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Fno編集";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // DataList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1260, 556);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.Name = "DataList";
            this.Text = "DataList";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Activated += new System.EventHandler(this.DataList_Activated);
            this.Deactivate += new System.EventHandler(this.DataList_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DataList_FormClosing);
            this.Load += new System.EventHandler(this.DataList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Race;
        private System.Windows.Forms.DataGridViewTextBoxColumn Jomei;
        private System.Windows.Forms.DataGridViewTextBoxColumn Distance;
        private System.Windows.Forms.DataGridViewTextBoxColumn Baba;
        private System.Windows.Forms.DataGridViewTextBoxColumn Rank;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ninki;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bamei;
        private System.Windows.Forms.DataGridViewTextBoxColumn Jockey;
        private System.Windows.Forms.DataGridViewTextBoxColumn Futan;
        private System.Windows.Forms.DataGridViewTextBoxColumn FType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Father;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mother;
        private System.Windows.Forms.DataGridViewTextBoxColumn FnoColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn MFType;
        private System.Windows.Forms.DataGridViewTextBoxColumn MF;
        private System.Windows.Forms.DataGridViewTextBoxColumn BloodNumber;
    }
}