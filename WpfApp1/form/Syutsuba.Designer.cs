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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DMStatus = new System.Windows.Forms.Label();
            this.HappyoTime = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.TrackDistance = new System.Windows.Forms.Label();
            this.TrackLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.DistanceLabel = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.TrackNameLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label16 = new System.Windows.Forms.Label();
            this.KigoLabel = new System.Windows.Forms.Label();
            this.OldYear = new System.Windows.Forms.Label();
            this.ClassLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.RaceNum = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.kaiji = new System.Windows.Forms.Label();
            this.racename = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.weLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Kaisai = new System.Windows.Forms.Label();
            this.Date = new System.Windows.Forms.Label();
            this.raceNameEng = new System.Windows.Forms.Label();
            this.OddzTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.FFM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MMFColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MFColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.M = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FColor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Futan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Jockey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Minarai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OddsRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Odds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TMRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VMRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bamei = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.枠 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button4 = new System.Windows.Forms.Button();
            this.tableLayoutPanel3.SuspendLayout();
            this.flowLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
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
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button2.ForeColor = System.Drawing.Color.Black;
            this.button2.Location = new System.Drawing.Point(7, 610);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 32);
            this.button2.TabIndex = 7;
            this.button2.Text = "DM取得\r\n";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Enabled = false;
            this.label1.Location = new System.Drawing.Point(84, 616);
            this.label1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "DM情報：";
            this.label1.Visible = false;
            // 
            // DMStatus
            // 
            this.DMStatus.AutoSize = true;
            this.DMStatus.Enabled = false;
            this.DMStatus.Location = new System.Drawing.Point(130, 616);
            this.DMStatus.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.DMStatus.Name = "DMStatus";
            this.DMStatus.Size = new System.Drawing.Size(113, 12);
            this.DMStatus.TabIndex = 9;
            this.DMStatus.Text = "天候馬場状態発表後";
            this.DMStatus.Visible = false;
            // 
            // HappyoTime
            // 
            this.HappyoTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.HappyoTime.AutoSize = true;
            this.HappyoTime.Font = new System.Drawing.Font("メイリオ", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.HappyoTime.Location = new System.Drawing.Point(786, 32);
            this.HappyoTime.Name = "HappyoTime";
            this.HappyoTime.Size = new System.Drawing.Size(52, 17);
            this.HappyoTime.TabIndex = 50;
            this.HappyoTime.Text = "発表時間";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel3.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel3.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 66F));
            this.tableLayoutPanel3.Controls.Add(this.TrackDistance, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.TrackLabel, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(878, 12);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(102, 21);
            this.tableLayoutPanel3.TabIndex = 49;
            // 
            // TrackDistance
            // 
            this.TrackDistance.AutoSize = true;
            this.TrackDistance.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TrackDistance.Location = new System.Drawing.Point(55, 1);
            this.TrackDistance.Name = "TrackDistance";
            this.TrackDistance.Size = new System.Drawing.Size(30, 19);
            this.TrackDistance.TabIndex = 6;
            this.TrackDistance.Text = "---";
            this.TrackDistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TrackLabel
            // 
            this.TrackLabel.AutoSize = true;
            this.TrackLabel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TrackLabel.Location = new System.Drawing.Point(4, 1);
            this.TrackLabel.Name = "TrackLabel";
            this.TrackLabel.Size = new System.Drawing.Size(43, 19);
            this.TrackLabel.TabIndex = 6;
            this.TrackLabel.Text = "ダート";
            this.TrackLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel4.BackColor = System.Drawing.Color.SlateGray;
            this.flowLayoutPanel4.Location = new System.Drawing.Point(-1, 130);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(1145, 5);
            this.flowLayoutPanel4.TabIndex = 48;
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flowLayoutPanel5.Controls.Add(this.DistanceLabel);
            this.flowLayoutPanel5.Controls.Add(this.label18);
            this.flowLayoutPanel5.Controls.Add(this.TrackNameLabel);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(745, 102);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(251, 29);
            this.flowLayoutPanel5.TabIndex = 47;
            // 
            // DistanceLabel
            // 
            this.DistanceLabel.AutoSize = true;
            this.DistanceLabel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.DistanceLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DistanceLabel.Location = new System.Drawing.Point(3, 0);
            this.DistanceLabel.Name = "DistanceLabel";
            this.DistanceLabel.Size = new System.Drawing.Size(49, 19);
            this.DistanceLabel.TabIndex = 16;
            this.DistanceLabel.Text = "9999";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label18.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label18.Location = new System.Drawing.Point(58, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 19);
            this.label18.TabIndex = 14;
            this.label18.Text = "m";
            // 
            // TrackNameLabel
            // 
            this.TrackNameLabel.AutoSize = true;
            this.TrackNameLabel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.TrackNameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.TrackNameLabel.Location = new System.Drawing.Point(87, 0);
            this.TrackNameLabel.Name = "TrackNameLabel";
            this.TrackNameLabel.Size = new System.Drawing.Size(77, 19);
            this.TrackNameLabel.TabIndex = 16;
            this.TrackNameLabel.Text = "（芝・左）";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel2.ColumnCount = 4;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30.05249F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 69.94751F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 177F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 59F));
            this.tableLayoutPanel2.Controls.Add(this.label16, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.KigoLabel, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.OldYear, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.ClassLabel, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(156, 102);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(588, 29);
            this.tableLayoutPanel2.TabIndex = 46;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label16.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label16.Location = new System.Drawing.Point(531, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(35, 29);
            this.label16.TabIndex = 12;
            this.label16.Text = "コース：";
            this.label16.Click += new System.EventHandler(this.Label16_Click);
            // 
            // KigoLabel
            // 
            this.KigoLabel.AutoSize = true;
            this.KigoLabel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.KigoLabel.Location = new System.Drawing.Point(354, 0);
            this.KigoLabel.Name = "KigoLabel";
            this.KigoLabel.Size = new System.Drawing.Size(159, 19);
            this.KigoLabel.TabIndex = 12;
            this.KigoLabel.Text = "（国際）（指定）定量";
            // 
            // OldYear
            // 
            this.OldYear.AutoSize = true;
            this.OldYear.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.OldYear.Location = new System.Drawing.Point(3, 0);
            this.OldYear.Name = "OldYear";
            this.OldYear.Size = new System.Drawing.Size(46, 19);
            this.OldYear.TabIndex = 8;
            this.OldYear.Text = "サラ系";
            // 
            // ClassLabel
            // 
            this.ClassLabel.AutoSize = true;
            this.ClassLabel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.ClassLabel.Location = new System.Drawing.Point(108, 0);
            this.ClassLabel.Name = "ClassLabel";
            this.ClassLabel.Size = new System.Drawing.Size(54, 19);
            this.ClassLabel.TabIndex = 10;
            this.ClassLabel.Text = "オープン";
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.BackColor = System.Drawing.Color.Green;
            this.panel1.Controls.Add(this.RaceNum);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Location = new System.Drawing.Point(28, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(119, 37);
            this.panel1.TabIndex = 45;
            // 
            // RaceNum
            // 
            this.RaceNum.Font = new System.Drawing.Font("メイリオ", 19.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.RaceNum.ForeColor = System.Drawing.Color.White;
            this.RaceNum.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RaceNum.Location = new System.Drawing.Point(-2, 3);
            this.RaceNum.Name = "RaceNum";
            this.RaceNum.Size = new System.Drawing.Size(121, 32);
            this.RaceNum.TabIndex = 10;
            this.RaceNum.Text = "東京10R";
            this.RaceNum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(38, 43);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 22);
            this.label10.TabIndex = 6;
            this.label10.Text = "11R";
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flowLayoutPanel6.Controls.Add(this.kaiji);
            this.flowLayoutPanel6.Controls.Add(this.racename);
            this.flowLayoutPanel6.Location = new System.Drawing.Point(156, 52);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(921, 35);
            this.flowLayoutPanel6.TabIndex = 44;
            // 
            // kaiji
            // 
            this.kaiji.AutoSize = true;
            this.kaiji.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.kaiji.Location = new System.Drawing.Point(3, 5);
            this.kaiji.Margin = new System.Windows.Forms.Padding(3, 5, 3, 0);
            this.kaiji.Name = "kaiji";
            this.kaiji.Size = new System.Drawing.Size(54, 19);
            this.kaiji.TabIndex = 5;
            this.kaiji.Text = "第０回";
            // 
            // racename
            // 
            this.racename.AutoSize = true;
            this.racename.Font = new System.Drawing.Font("メイリオ", 16.125F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.racename.Location = new System.Drawing.Point(3, 24);
            this.racename.Name = "racename";
            this.racename.Size = new System.Drawing.Size(880, 66);
            this.racename.TabIndex = 8;
            this.racename.Text = "ウオッカ追悼競走 ニュージーランドトロフィー（ＧⅡ）（ＮＨＫマイルカップトライアル）";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tableLayoutPanel1.Controls.Add(this.weLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(782, 12);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(94, 21);
            this.tableLayoutPanel1.TabIndex = 43;
            // 
            // weLabel
            // 
            this.weLabel.AutoSize = true;
            this.weLabel.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.weLabel.Location = new System.Drawing.Point(51, 1);
            this.weLabel.Name = "weLabel";
            this.weLabel.Size = new System.Drawing.Size(30, 19);
            this.weLabel.TabIndex = 6;
            this.weLabel.Text = "---";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.label5.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(4, 1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 19);
            this.label5.TabIndex = 5;
            this.label5.Text = "天候";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Meiryo UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(625, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 20);
            this.label4.TabIndex = 42;
            this.label4.Text = "15時25分";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(548, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 19);
            this.label3.TabIndex = 41;
            this.label3.Text = "発走時刻：";
            // 
            // Kaisai
            // 
            this.Kaisai.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Kaisai.AutoSize = true;
            this.Kaisai.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Kaisai.Location = new System.Drawing.Point(368, 14);
            this.Kaisai.Name = "Kaisai";
            this.Kaisai.Size = new System.Drawing.Size(117, 19);
            this.Kaisai.TabIndex = 40;
            this.Kaisai.Text = "第0回場名0日目";
            // 
            // Date
            // 
            this.Date.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.Date.AutoSize = true;
            this.Date.Font = new System.Drawing.Font("Meiryo UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Date.Location = new System.Drawing.Point(142, 14);
            this.Date.Name = "Date";
            this.Date.Size = new System.Drawing.Size(170, 19);
            this.Date.TabIndex = 39;
            this.Date.Text = "2099年99月99日(日曜)";
            // 
            // raceNameEng
            // 
            this.raceNameEng.AutoSize = true;
            this.raceNameEng.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.raceNameEng.Location = new System.Drawing.Point(218, 85);
            this.raceNameEng.Name = "raceNameEng";
            this.raceNameEng.Size = new System.Drawing.Size(0, 15);
            this.raceNameEng.TabIndex = 51;
            // 
            // OddzTime
            // 
            this.OddzTime.AutoSize = true;
            this.OddzTime.Enabled = false;
            this.OddzTime.Location = new System.Drawing.Point(392, 616);
            this.OddzTime.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.OddzTime.Name = "OddzTime";
            this.OddzTime.Size = new System.Drawing.Size(113, 12);
            this.OddzTime.TabIndex = 54;
            this.OddzTime.Text = "天候馬場状態発表後";
            this.OddzTime.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(338, 616);
            this.label6.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 53;
            this.label6.Text = "発表時間：";
            this.label6.Visible = false;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button3.ForeColor = System.Drawing.Color.Black;
            this.button3.Location = new System.Drawing.Point(261, 610);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 32);
            this.button3.TabIndex = 52;
            this.button3.Text = "オッズ取得";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FFM
            // 
            this.FFM.HeaderText = "母母父";
            this.FFM.Name = "FFM";
            this.FFM.ReadOnly = true;
            this.FFM.Width = 350;
            // 
            // MMFColor
            // 
            this.MMFColor.HeaderText = "";
            this.MMFColor.Name = "MMFColor";
            this.MMFColor.ReadOnly = true;
            this.MMFColor.Width = 20;
            // 
            // FM
            // 
            this.FM.HeaderText = "母父";
            this.FM.Name = "FM";
            this.FM.ReadOnly = true;
            this.FM.Width = 350;
            // 
            // MFColor
            // 
            this.MFColor.HeaderText = "";
            this.MFColor.Name = "MFColor";
            this.MFColor.ReadOnly = true;
            this.MFColor.Width = 20;
            // 
            // M
            // 
            this.M.HeaderText = "父";
            this.M.Name = "M";
            this.M.ReadOnly = true;
            this.M.Width = 350;
            // 
            // FColor
            // 
            this.FColor.HeaderText = "";
            this.FColor.Name = "FColor";
            this.FColor.ReadOnly = true;
            this.FColor.Width = 20;
            // 
            // Futan
            // 
            this.Futan.HeaderText = "負担重量";
            this.Futan.Name = "Futan";
            this.Futan.ReadOnly = true;
            this.Futan.Width = 200;
            // 
            // Jockey
            // 
            this.Jockey.HeaderText = "騎手";
            this.Jockey.Name = "Jockey";
            this.Jockey.ReadOnly = true;
            this.Jockey.Width = 200;
            // 
            // Minarai
            // 
            this.Minarai.HeaderText = "";
            this.Minarai.Name = "Minarai";
            this.Minarai.ReadOnly = true;
            this.Minarai.Width = 75;
            // 
            // OddsRank
            // 
            this.OddsRank.HeaderText = "人";
            this.OddsRank.Name = "OddsRank";
            this.OddsRank.ReadOnly = true;
            this.OddsRank.Visible = false;
            this.OddsRank.Width = 75;
            // 
            // Odds
            // 
            this.Odds.HeaderText = "単勝";
            this.Odds.Name = "Odds";
            this.Odds.ReadOnly = true;
            this.Odds.Visible = false;
            this.Odds.Width = 150;
            // 
            // TMRank
            // 
            this.TMRank.HeaderText = "";
            this.TMRank.Name = "TMRank";
            this.TMRank.ReadOnly = true;
            this.TMRank.Visible = false;
            this.TMRank.Width = 75;
            // 
            // TM
            // 
            this.TM.HeaderText = "タイム型";
            this.TM.Name = "TM";
            this.TM.ReadOnly = true;
            this.TM.Visible = false;
            this.TM.Width = 159;
            // 
            // VMRank
            // 
            this.VMRank.HeaderText = "";
            this.VMRank.Name = "VMRank";
            this.VMRank.ReadOnly = true;
            this.VMRank.Visible = false;
            this.VMRank.Width = 75;
            // 
            // DM
            // 
            this.DM.HeaderText = "対戦型DM";
            this.DM.Name = "DM";
            this.DM.ReadOnly = true;
            this.DM.Visible = false;
            this.DM.Width = 150;
            // 
            // Bamei
            // 
            this.Bamei.HeaderText = "馬名";
            this.Bamei.Name = "Bamei";
            this.Bamei.ReadOnly = true;
            this.Bamei.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Bamei.Width = 350;
            // 
            // Uma
            // 
            this.Uma.HeaderText = "馬";
            this.Uma.MaxInputLength = 3;
            this.Uma.Name = "Uma";
            this.Uma.ReadOnly = true;
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
            this.VMRank,
            this.TM,
            this.TMRank,
            this.Odds,
            this.OddsRank,
            this.Minarai,
            this.Jockey,
            this.Futan,
            this.FColor,
            this.M,
            this.MFColor,
            this.FM,
            this.MMFColor,
            this.FFM});
            this.dataGridView1.Location = new System.Drawing.Point(6, 140);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 21;
            this.dataGridView1.Size = new System.Drawing.Size(1125, 456);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Meiryo UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.button4.ForeColor = System.Drawing.Color.Black;
            this.button4.Location = new System.Drawing.Point(1054, 610);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 32);
            this.button4.TabIndex = 55;
            this.button4.Text = "確定成績";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.Button4_Click);
            // 
            // Syutsuba
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(1141, 696);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.OddzTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.HappyoTime);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.raceNameEng);
            this.Controls.Add(this.flowLayoutPanel4);
            this.Controls.Add(this.flowLayoutPanel5);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.flowLayoutPanel6);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Kaisai);
            this.Controls.Add(this.Date);
            this.Controls.Add(this.DMStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Syutsuba";
            this.Text = "Form2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form2_Load);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.flowLayoutPanel5.ResumeLayout(false);
            this.flowLayoutPanel5.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DMStatus;
        private System.Windows.Forms.Label HappyoTime;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label TrackDistance;
        private System.Windows.Forms.Label TrackLabel;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel5;
        private System.Windows.Forms.Label DistanceLabel;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label TrackNameLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label KigoLabel;
        private System.Windows.Forms.Label OldYear;
        private System.Windows.Forms.Label ClassLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label RaceNum;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel6;
        private System.Windows.Forms.Label kaiji;
        private System.Windows.Forms.Label racename;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label weLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Kaisai;
        private System.Windows.Forms.Label Date;
        private System.Windows.Forms.Label raceNameEng;
        private System.Windows.Forms.Label OddzTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridViewTextBoxColumn FFM;
        private System.Windows.Forms.DataGridViewTextBoxColumn MMFColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn FM;
        private System.Windows.Forms.DataGridViewTextBoxColumn MFColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn M;
        private System.Windows.Forms.DataGridViewTextBoxColumn FColor;
        private System.Windows.Forms.DataGridViewTextBoxColumn Futan;
        private System.Windows.Forms.DataGridViewTextBoxColumn Jockey;
        private System.Windows.Forms.DataGridViewTextBoxColumn Minarai;
        private System.Windows.Forms.DataGridViewTextBoxColumn OddsRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn Odds;
        private System.Windows.Forms.DataGridViewTextBoxColumn TMRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn TM;
        private System.Windows.Forms.DataGridViewTextBoxColumn VMRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn DM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bamei;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uma;
        private System.Windows.Forms.DataGridViewTextBoxColumn 枠;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button4;
    }
}