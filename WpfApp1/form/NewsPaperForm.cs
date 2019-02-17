using LibJvConv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.dbCom1;
using WpfApp1.JvComDbData;

namespace WpfApp1.form
{
    public partial class NewsPaperForm : Form
    {
        private System.Windows.Forms.Label[] BameiArray;
        private System.Windows.Forms.Label[] FNameArray;
        private System.Windows.Forms.Label[] MNameArray;
        private System.Windows.Forms.Label[] FFNameArray;
        private System.Windows.Forms.Label[] MFNameArray;
        private System.Windows.Forms.Label[] JnameArray;
        private System.Windows.Forms.Label[] JfutanArray;
        private System.Windows.Forms.Label[] UmaKigoArray;

        private System.Windows.Forms.Label[] labelArray;
        private System.Windows.Forms.Label[] KaisaiArray;
        private System.Windows.Forms.Label[] RankArray;
        private System.Windows.Forms.Label[] DateArray;
        private System.Windows.Forms.Label[] GradeArray;
        private System.Windows.Forms.Label[] NinkiArray;
        private System.Windows.Forms.Label[] JockeyArray;
        private System.Windows.Forms.Label[] TosuArray;
        private System.Windows.Forms.Label[] TrackArray;
        private System.Windows.Forms.Label[] TimeArray;
        private System.Windows.Forms.Label[] TukaArray;
        private System.Windows.Forms.Label[] Time1Array;
        private System.Windows.Forms.Label[] Time2Array;
        private System.Windows.Forms.Label[] Time3Array;
        private System.Windows.Forms.Label[] AiteUmaArray;
        private System.Windows.Forms.Label[] TimeDiffArray;
        private System.Windows.Forms.Panel[] PanelArray;
        private System.Windows.Forms.Panel[] Panel2Array;
        private readonly int tmpStartPotision;

        JvDbRaData raceData; //レース情報
        dbAccess.dbConnect db = new dbAccess.dbConnect();
        dbCom dbCom = new dbCom();
        static String RA_Key;


        public NewsPaperForm()
        {
            InitializeComponent();
        }

        public NewsPaperForm(String RaKey, int ColorNum)
        {
            InitializeComponent();

            if (RaKey == "")
            {
                MessageBox.Show("レースが選択されていません。");
                return;
            }

            RA_Key = RaKey;
            InitRaceData(RaKey);

            switch (ColorNum)
            {
                case 1:
                    panel1.BackColor = Color.Blue;
                    flowLayoutPanel4.BackColor = Color.Blue;
                    break;
                case 2:
                    panel1.BackColor = Color.Green;
                    flowLayoutPanel4.BackColor = Color.Green;
                    break;
                case 3:
                    panel1.BackColor = Color.Purple;
                    flowLayoutPanel4.BackColor = Color.Purple;
                    break;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        unsafe private void InitRaceData(String RaKey)
        {
            raceData = new JvDbRaData();
            List<String> tmp = new List<string>();
            int libCode = 0;

            if(db.TextReader_Col(RA_Key.Substring(0, 8), "RA", 0, ref tmp, RaKey) != 0)
            {
                raceData.setData(ref tmp);
            }
            else
            {
                return;
            }

            String libStr = "";

            //表示処理
            libCode = 2002;
            LibJvConvFuncClass.jvSysConvFunction(&libCode, raceData.getWeekDay(), ref libStr);
            this.Date.Text = raceData.ConvertDateToDate(raceData.getRaceDate());
            this.Date.Text += "(" + libStr + ")";

            libCode = LibJvConvFuncClass.COURCE_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&libCode, raceData.getRaceCource(), ref libStr);
            this.Kaisai.Text = "第" + raceData.getRaceKaiji() + "回" + libStr + raceData.getRaceNichiji() + "日目";

            this.label4.Text = raceData.getRaceStartTime().Substring(0, 2) + "時" + raceData.getRaceStartTime().Substring(2, 2) + "分";
            this.DistanceLabel.Text = raceData.getDistance();

            libCode = LibJvConvFuncClass.TRACK_CODE ;
            LibJvConvFuncClass.jvSysConvFunction(&libCode, raceData.getCourceTrack(), ref libStr);
            this.TrackNameLabel.Text = "（" + libStr + "）";

            libCode = 2007;
            LibJvConvFuncClass.jvSysConvFunction(&libCode, raceData.getRaceClass(), ref libStr);
            this.ClassLabel.Text = libStr;

            libCode = 2006;
            LibJvConvFuncClass.jvSysConvFunction(&libCode, raceData.getRaceKindKigo(), ref libStr);
            this.KigoLabel.Text = libStr;

            libCode = LibJvConvFuncClass.RACE_NAME;
            LibJvConvFuncClass.jvSysConvFunction(&libCode, raceData.getRaceName(), ref libStr);

            if(raceData.getRaceGrade() == "一般" || raceData.getRaceGrade() == "特別" || raceData.getRaceGrade() == "")
            {
                LibJvConvFuncClass.jvSysConvFunction(&libCode, raceData.getRaceName(), ref libStr);
                this.racename.Text = libStr;
            }
            else
            {
                this.racename.Text = libStr + "（" + raceData.getRaceGrade() + "）";
            }

            

            this.kaiji.Text = (raceData.getRaceGradeKai() == 0 ? "" : "第" + raceData.getRaceGradeKai() + "回");
            this.raceNameEng.Text = raceData.getRaceNameEng();
            this.RaceNum.Text = Int32.Parse(raceData.getRaceNum()) + "R";
         }


        private String BameiToLength(String inStr)
        {
            String tmp = "";
            String tmp2 = "";
            for(int i=0; i<inStr.Length; i++)
            {
                tmp2 = inStr.Substring(i, 1);
                if(tmp2 == "ー")
                {
                    tmp += "｜";
                }
                else
                {
                    tmp += tmp2;
                }
                tmp += "\n";
            }
            return tmp;
        }

        private unsafe void NewsPaperForm_Load(object sender, EventArgs e)
        {
            if (RA_Key == "" || RA_Key == null)
            {
                MessageBox.Show("レースが選択されていません。");
                return;
            }

            const int MAX_TOSU = 18;
            this.labelArray = new System.Windows.Forms.Label[MAX_TOSU];
            RankArray = new Label[MAX_TOSU];
            this.DateArray = new Label[MAX_TOSU];
            GradeArray = new Label[MAX_TOSU];
            NinkiArray = new Label[MAX_TOSU];
            KaisaiArray = new Label[MAX_TOSU];
            JockeyArray = new Label[MAX_TOSU];
            TosuArray = new Label[MAX_TOSU];
            TukaArray = new Label[MAX_TOSU];
            TimeArray = new Label[MAX_TOSU];
            Time1Array = new Label[MAX_TOSU];
            Time2Array = new Label[MAX_TOSU];
            Time3Array = new Label[MAX_TOSU];
            AiteUmaArray = new Label[MAX_TOSU];
            TimeDiffArray = new Label[MAX_TOSU];
            TrackArray = new Label[MAX_TOSU];
            PanelArray = new Panel[MAX_TOSU];
            Panel2Array = new Panel[MAX_TOSU];

            BameiArray = new Label[MAX_TOSU];
            FNameArray = new Label[MAX_TOSU];
            MNameArray = new Label[MAX_TOSU];
            FFNameArray = new Label[MAX_TOSU];
            MFNameArray = new Label[MAX_TOSU];
            JnameArray = new Label[MAX_TOSU];
            JfutanArray = new Label[MAX_TOSU];
            UmaKigoArray = new Label[MAX_TOSU];

            int Heaf = 99;
            int MarginLoc = 850;
            const int tmpStartPotision = 540;
            const int YPosition = 150;
            const int XPosition = 318;
            const int StartYPosition = 10;

            int gStartPotision = tmpStartPotision;

            int libNum = LibJvConvFuncClass.HOUCE_KIND;
            String libstr = "";

            /* DB読み込み用の配列宣言 */
            List<String> strArray = new List<string>();
            List<String> str2Array = new List<string>();

            List<String> KettoNumArray = new List<string>();

            /* DB処理用のインスタンス宣言 */
            JvComDbData.JvDbSEData SEdata = new JvComDbData.JvDbSEData();
            

            for (int k = 0; k < MAX_TOSU; k++)
            {
                //DB読み込み
                strArray.Clear();
                str2Array.Clear();
                //db.TextReader_Row("201902110501051101", "RA", 0, ref strArray);
                db.TextReader_Col(RA_Key.Substring(0, 8), "SE", 0, ref strArray, RA_Key + String.Format("{0:00}", k + 1));

                if(strArray.Count == 0)
                {
                    break;
                }
                else
                {
                    SEdata.SetSEData(strArray);
                }

                db.TextReader_Col("0", "UM", 0, ref str2Array, SEdata.KettoNum1.ToString());
                
                if(str2Array.Count == 0)
                {
                    break;
                }

                //血統番号登録
                KettoNumArray.Add(SEdata.KettoNum1.ToString());


                //枠線
                this.PanelArray[k] = new Panel();
                //プロパティ設定
                this.PanelArray[k].Name = "Panel1_" + k.ToString();
                this.PanelArray[k].Size = new Size(new Point(YPosition, 530));
                this.PanelArray[k].BorderStyle = BorderStyle.FixedSingle;
                //this.PanelArray[k].BackColor = Color.Transparent;
                this.PanelArray[k].Location = new Point(StartYPosition + k * YPosition, 220);

                //枠線2
                this.Panel2Array[k] = new Panel();
                //プロパティ設定
                this.Panel2Array[k].Name = "Panel2_" + k.ToString();
                this.Panel2Array[k].Size = new Size(new Point(YPosition, 50));
                this.Panel2Array[k].BorderStyle = BorderStyle.FixedSingle;
                this.Panel2Array[k].BackColor = dbCom.WakubanToColor(0, SEdata.Waku1);
                this.Panel2Array[k].Location = new Point(StartYPosition + k * YPosition, 180);


                this.BameiArray[k] = new Label();
                //プロパティ設定
                this.BameiArray[k].Name = "Bamei" + k.ToString();
                this.BameiArray[k].Size = new Size(40, 430);
                this.BameiArray[k].Font = new Font("メイリオ", 12, FontStyle.Bold);
                this.BameiArray[k].Text = BameiToLength(SEdata.Name1);
                this.BameiArray[k].TextAlign = ContentAlignment.TopCenter;
                this.BameiArray[k].Location = new Point((StartYPosition + 58) + (k * YPosition), 250);
                //this.BameiArray[k].BackColor = Color.AliceBlue;
               
                //父
                this.FNameArray[k] = new Label();
                //プロパティ設定
                this.FNameArray[k].Name = "Fname" + k.ToString();
                this.FNameArray[k].Size = new Size(30, 430);
                this.FNameArray[k].Font = new Font("Meiryo UI", 7);
                this.FNameArray[k].Text = BameiToLength(str2Array[6]);
                this.FNameArray[k].TextAlign = ContentAlignment.TopCenter;
                this.FNameArray[k].Location = new Point((StartYPosition + 95) + (k * YPosition), 250);
                
                //父父
                this.FFNameArray[k] = new Label();
                //プロパティ設定
                this.FFNameArray[k].Name = "FFname" + k.ToString();
                this.FFNameArray[k].Size = new Size(30, 400);
                this.FFNameArray[k].Font = new Font("Meiryo UI", 7);
                this.FFNameArray[k].Text = BameiToLength(str2Array[8]);
                this.FFNameArray[k].TextAlign = ContentAlignment.TopCenter;
                this.FFNameArray[k].Location = new Point((StartYPosition + 117) + (k * YPosition), 270);

                this.Controls.AddRange(this.FFNameArray);

                //母
                this.MNameArray[k] = new Label();
                //プロパティ設定
                this.MNameArray[k].Name = "Mname" + k.ToString();
                this.MNameArray[k].Size = new Size(30, 430);
                this.MNameArray[k].Font = new Font("Meiryo UI", 7);
                this.MNameArray[k].Text = BameiToLength(str2Array[7]);
                this.MNameArray[k].TextAlign = ContentAlignment.TopCenter;
                this.MNameArray[k].Location = new Point((StartYPosition +30) + (k * YPosition), 250);

                //母父
                this.MFNameArray[k] = new Label();
                //プロパティ設定
                this.MFNameArray[k].Name = "Mname" + k.ToString();
                this.MFNameArray[k].Size = new Size(30, 430);
                this.MFNameArray[k].Font = new Font("Meiryo UI", 7);
                this.MFNameArray[k].Text = BameiToLength(str2Array[9]);
                this.MFNameArray[k].TextAlign = ContentAlignment.TopCenter;
                this.MFNameArray[k].Location = new Point((StartYPosition+2) + (k * YPosition), 270);

                //負担
                this.JfutanArray[k] = new Label();
                //プロパティ設定
                this.JfutanArray[k].Name = "JFutan" + k.ToString();
                this.JfutanArray[k].Size = new Size(YPosition - 3, 30);
                this.JfutanArray[k].Font = new Font("MS P ゴシック", 7);
                this.JfutanArray[k].Text = "("+ SEdata.Futan1.Substring(0,2) + "." + SEdata.Futan1.Substring(2,1) +"kg)";
                this.JfutanArray[k].TextAlign = ContentAlignment.MiddleCenter;
                this.JfutanArray[k].Location = new Point((StartYPosition + 2) + (k * YPosition), 685);
                //this.JfutanArray[k].BackColor = Color.AliceBlue;
                this.Controls.AddRange(this.JfutanArray);

                //騎手
                this.JnameArray[k] = new Label();
                //プロパティ設定
                this.JnameArray[k].Name = "Jname" + k.ToString();
                this.JnameArray[k].Size = new Size(YPosition - 3, 30);
                this.JnameArray[k].Font = new Font("MS P ゴシック", 8);
                this.JnameArray[k].Text = SEdata.Jockey1;
                this.JnameArray[k].TextAlign = ContentAlignment.MiddleCenter;
                this.JnameArray[k].Location = new Point((StartYPosition + 2) + (k * YPosition), 715);
                //this.JnameArray[k].BackColor = Color.AliceBlue;
                this.Controls.AddRange(this.JnameArray);

                //馬記号
                this.UmaKigoArray[k] = new Label();
                //プロパティ設定
                this.UmaKigoArray[k].Name = "Umakigo" + k.ToString();
                this.UmaKigoArray[k].Size = new Size(YPosition -3, 30);
                this.UmaKigoArray[k].Font = new Font("MS P ゴシック", 8);
                LibJvConvFuncClass.jvSysConvFunction(&libNum, SEdata.UmaKigou1, ref libstr);
                this.UmaKigoArray[k].Text = libstr; 
                this.UmaKigoArray[k].TextAlign = ContentAlignment.MiddleCenter;
                this.UmaKigoArray[k].Location = new Point((StartYPosition + 2) + (k * YPosition), 222);
                //this.UmaKigoArray[k].BackColor = Color.AliceBlue;

                //馬番
                this.TosuArray[k] = new Label();
                //プロパティ設定
                this.TosuArray[k].Name = "Umaban" + k.ToString();
                this.TosuArray[k].Size = new Size(YPosition - 3, 25);
                this.TosuArray[k].Font = new Font("MS P ゴシック", 10);
                this.TosuArray[k].Text = Int32.Parse(SEdata.Umaban1).ToString();
                this.TosuArray[k].TextAlign = ContentAlignment.MiddleCenter;
                this.TosuArray[k].BackColor = dbCom.WakubanToColor(0, SEdata.Waku1);
                this.TosuArray[k].ForeColor = dbCom.WakubanToColor(1, SEdata.Waku1);
                this.TosuArray[k].Location = new Point((StartYPosition + 2) + (k * YPosition), 190);
                //this.UmaKigoArray[k].BackColor = Color.AliceBlue;



            }

            this.Controls.AddRange(this.BameiArray);
            this.Controls.AddRange(this.UmaKigoArray);
            this.Controls.AddRange(this.MNameArray);
            this.Controls.AddRange(this.FNameArray);
            this.Controls.AddRange(this.MFNameArray);
            this.Controls.AddRange(this.TosuArray);
            this.Controls.AddRange(this.Panel2Array);
            this.Controls.AddRange(this.PanelArray);
            




            this.labelArray = new System.Windows.Forms.Label[MAX_TOSU];
            int tmpRank = 0;
            Color RankColor = new Color();

            for (int i = 0; i < 3; i++)
            {
                switch(i)
                {
                    case 0:
                        //MarginLoc = 850;
                        break;
                    case 1:
                        MarginLoc = MarginLoc - XPosition;
                        break;
                    case 2:
                        MarginLoc = MarginLoc - XPosition;
                        break;
                }
               
                for (int k = 0, j = 0; k < KettoNumArray.Count; k++)
                {
                    
                    /* DBから過去走データ取得 */
                    strArray.Clear();
                    SEdata = new JvComDbData.JvDbSEData();

                    if(dbCom.DbComGetOldRunDataMapping(KettoNumArray[k], ref strArray, i+1) == 0)
                    {
                        continue;
                    }
                    
                    SEdata.SetSEMSTData(strArray);

                    //着順
                    this.RankArray[k] = new Label();
                    //プロパティ設定
                    this.RankArray[k].Name = "Rank" + k.ToString() + i.ToString();
                    this.RankArray[k].Top = 0;
                    this.RankArray[k].Width = 50;
                    this.RankArray[k].Size = new Size(75, 75);
                    this.RankArray[k].Font = new Font("Meiryo UI", 14, FontStyle.Bold);
                    this.RankArray[k].Text = Int32.Parse(SEdata.RaceHist1.rank).ToString();
                    this.RankArray[k].TextAlign = ContentAlignment.BottomCenter;

                    if(this.RankArray[k].Text == "0")
                    {
                        this.RankArray[k].Text = "外";
                    }

                    //this.RankArray[k].BackColor = Color.Ivory;
                    this.RankArray[k].Location = new Point(StartYPosition + 70 + k * YPosition, MarginLoc + gStartPotision + 30);
                    
                    /* 着順はこのあとの工程で利用するためローカル変数に保持する */
                    if(Int32.TryParse(SEdata.RaceHist1.rank, out tmpRank))
                    {
                        
                    }
                    else
                    {
                        tmpRank = 99;       //取消・除外などはここに来る
                    }

                    this.labelArray[k] = new Label();
                    //プロパティ設定
                    this.labelArray[k].Name = "Name" + k.ToString() + i.ToString();
                    this.labelArray[k].Size = new Size(147, 30);
                    this.labelArray[k].Font = new Font("Meiryo UI", 10, FontStyle.Bold);
                    if(SEdata.RaceHist1.raceName10 == "")
                    {
                        libNum = 2007;
                        LibJvConvFuncClass.jvSysConvFunction(&libNum, SEdata.RaceHist1.JyokenName.ToString(), ref libstr);
                        this.labelArray[k].Text = libstr;                     //TODO 10文字→6文字に変更予定
                    }
                    else
                    {
                        this.labelArray[k].Text = SEdata.RaceHist1.raceName10;                        //TODO 10文字→6文字に変更予定
                    }
                    
                    //this.labelArray[k].BackColor = Color.AliceBlue;
                    this.labelArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc + gStartPotision + 23);
                    

                    //日付
                    this.DateArray[k] = new Label();
                    this.DateArray[k].Name = "Date" + k.ToString() + i.ToString();
                    this.DateArray[k].Size = new Size(105, 30);
                    this.DateArray[k].Font = new Font("Meiryo UI", 7);
                    this.DateArray[k].Text = SEdata.RaceHist1.RaceDate;
                    //this.DateArray[k].BackColor = Color.Aqua;
                    this.DateArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc  + gStartPotision);
                    
                    //開催
                    this.KaisaiArray[k] = new Label();
                    this.KaisaiArray[k].Name = "Date" + k.ToString() + i.ToString();
                    this.KaisaiArray[k].Size = new Size(45, 30);
                    this.KaisaiArray[k].Font = new Font("Meiryo UI", 7);
                    libNum = LibJvConvFuncClass.COURCE_CODE;
                    LibJvConvFuncClass.jvSysConvFunction(&libNum, SEdata.RaceHist1.Cource, ref libstr);
                    this.KaisaiArray[k].Text = libstr;
                    this.KaisaiArray[k].Location = new Point(StartYPosition + 103 + k * YPosition, MarginLoc  + gStartPotision);

                    //グレード
                    this.GradeArray[k] = new Label();
                    //プロパティ設定
                    this.GradeArray[k].Name = "Grade" + k.ToString() + i.ToString();
                    this.GradeArray[k].Font = new Font("Meiryo UI", 8);
                    this.GradeArray[k].Size = new Size(70, 30);
                    this.GradeArray[k].Text = SEdata.RaceHist1.grade;
                    this.GradeArray[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 53);

                    //人気
                    this.NinkiArray[k] = new Label();
                    //プロパティ設定
                    this.NinkiArray[k].Name = "Ninki" + k.ToString() + i.ToString();
                    this.NinkiArray[k].Font = new Font("メイリオ", 8);
                    this.NinkiArray[k].Size = new Size(70, 30);
                    if(SEdata.RaceHist1.Ninki == "00")
                    {

                    }
                    else
                    {
                        this.NinkiArray[k].Text = SEdata.RaceHist1.Ninki + "人";
                    }
                    
                    this.NinkiArray[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 78);
            
                    //騎手
                    this.JockeyArray[k] = new Label();
                    //プロパティ設定
                    this.JockeyArray[k].Name = "Jockey" + k.ToString() + i.ToString();
                    this.JockeyArray[k].Size = new Size(147, 30);
                    this.JockeyArray[k].Font = new Font("Meiryo UI", 8);
                    libNum = LibJvConvFuncClass.JOCKEY_MINARAI_CD;
                    LibJvConvFuncClass.jvSysConvFunction(&libNum, SEdata.RaceHist1.MinaraiCd, ref libstr);
                    this.JockeyArray[k].Text = libstr + SEdata.RaceHist1.jockey;
                    this.JockeyArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc  + gStartPotision + 153);

                    //頭数
                    this.TosuArray[k] = new Label();
                    //プロパティ設定
                    this.TosuArray[k].Name = "Tosu" + k.ToString() + i.ToString();
                    this.TosuArray[k].Size = new Size(147, 30);
                    this.TosuArray[k].Font = new Font("Meiryo UI", 8);
                    this.TosuArray[k].Text = SEdata.RaceHist1.tousuu + "ト" + SEdata.RaceHist1.umaban ;
                    this.TosuArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc  + gStartPotision + 257);

                    //トラック・距離
                    this.TrackArray[k] = new Label();
                    //プロパティ設定
                    this.TrackArray[k].Name = "Track" + k.ToString() + i.ToString();
                    this.TrackArray[k].Size = new Size(145, 30);
                    this.TrackArray[k].Font = new Font("Meiryo UI", 8);
                    libNum = LibJvConvFuncClass.TRACK_CODE_SHORT;
                    LibJvConvFuncClass.jvSysConvFunction(&libNum, SEdata.RaceHist1.track, ref libstr);
                    this.TrackArray[k].Text = libstr + " " + SEdata.RaceHist1.distance + "m";
                    this.TrackArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc  + gStartPotision + 128);
                    
                    //勝ちタイム：レコードの場合は先頭に「R」をつける
                    this.TimeArray[k] = new Label();
                    //プロパティ設定
                    this.TimeArray[k].Name = "Time" + k.ToString() + i.ToString();
                    this.TimeArray[k].Size = new Size(120, 30);
                    this.TimeArray[k].Font = new Font("Meiryo UI", 8);
                    this.TimeArray[k].Text = (SEdata.RaceHist1.RecornUpdateFlag ? "R " : "") + SEdata.RaceHist1.time.Substring(0,1) + ":" + SEdata.RaceHist1.time.Substring(1,2) + "." + SEdata.RaceHist1.time.Substring(3,1);
                    //this.TimeArray[k].BackColor = Color.IndianRed;
                    this.TimeArray[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 103);

                    //負担
                    this.Time1Array[k] = new Label();
                    //プロパティ設定
                    this.Time1Array[k].Name = "Futan" + k.ToString() + i.ToString();
                    this.Time1Array[k].Size = new Size(146, 30);
                    this.Time1Array[k].Font = new Font("Meiryo UI", 7);
                    this.Time1Array[k].Text = "(" + SEdata.RaceHist1.futan.Substring(0,2) + "." + SEdata.RaceHist1.futan.Substring(2,1) + ")";
                    this.Time1Array[k].TextAlign = ContentAlignment.BottomCenter;
                    this.Time1Array[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc + gStartPotision + 178);

                    //中タイム
                    this.Time2Array[k] = new Label();
                    //プロパティ設定
                    this.Time2Array[k].Name = "Time2" + k.ToString() + i.ToString();
                    this.Time2Array[k].Size = new Size(90, 30);
                    this.Time2Array[k].Font = new Font("Meiryo UI", 8);
                    this.Time2Array[k].Text = "47.2 →";

                    if (i < Heaf)
                    {
                        this.Time2Array[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 203);
                    }
                    else
                    {
                        this.Time2Array[k].Location = new Point(830, MarginLoc * j + gStartPotision + 131);
                    }

                    //上がり3F
                    this.Time3Array[k] = new Label();
                    //プロパティ設定
                    this.Time3Array[k].Name = "Time3" + k.ToString() + i.ToString();
                    this.Time3Array[k].Size = new Size(59, 30);
                    this.Time3Array[k].Font = new Font("Meiryo UI", 8);
                    this.Time3Array[k].Text = SEdata.RaceHist1.myLast3f.Substring(0,2) + "." + SEdata.RaceHist1.myLast3f.Substring(2,1);
                    this.Time3Array[k].Location = new Point(StartYPosition + 90 + k * YPosition, MarginLoc  + gStartPotision + 203);

                    //通過順
                    this.TukaArray[k] = new Label();
                    //プロパティ設定
                    this.TukaArray[k].Name = "Tuka" + k.ToString() + i.ToString();
                    this.TukaArray[k].Size = new Size(147, 30);
                    this.TukaArray[k].Font = new Font("Meiryo UI", 8);
                    //this.TukaArray[k].BackColor = Color.Ivory;
                    this.TukaArray[k].Text = ConvToCournerInfo(SEdata.RaceHist1.courner);
                    //this.TukaArray[k].TextAlign = ContentAlignment.MiddleCenter;
                    this.TukaArray[k].Location = new Point(StartYPosition + 1 + k * YPosition, MarginLoc  + gStartPotision + 230);

                    //相手馬
                    this.AiteUmaArray[k] = new Label();
                    //プロパティ設定
                    this.AiteUmaArray[k].Name = "AiteUma" + k.ToString() + i.ToString();
                    this.AiteUmaArray[k].Size = new Size(144, 27);
                    this.AiteUmaArray[k].Font = new Font("Meiryo UI", 8);
                    this.AiteUmaArray[k].Text = SEdata.RaceHist1.aiteuma;
                    this.AiteUmaArray[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 282);
                    //this.AiteUmaArray[k].BackColor = Color.Bisque;
                    
                    //着差
                    this.TimeDiffArray[k] = new Label();
                    //プロパティ設定
                    this.TimeDiffArray[k].Name = "TimeDiff" + k.ToString() + i.ToString();
                    this.TimeDiffArray[k].Size = new Size(60, 27);
                    this.TimeDiffArray[k].BackColor = Color.Transparent;
                    this.TimeDiffArray[k].Font = new Font("Meiryo UI", 8);
                    this.TimeDiffArray[k].Text = SEdata.RaceHist1.timeDiff;
                    this.TimeDiffArray[k].TextAlign = ContentAlignment.MiddleRight;
                    //this.TimeDiffArray[k].BackColor = Color.BlueViolet;
                    this.TimeDiffArray[k].Location = new Point(StartYPosition + 85 + k * YPosition, MarginLoc  + gStartPotision + 257);
                    
                    //枠線
                    this.PanelArray[k] = new Panel();
                    //プロパティ設定
                    this.PanelArray[k].Name = "Panel" + k.ToString() + i.ToString();
                    this.PanelArray[k].Size = new Size(new Point(YPosition, XPosition));
                    this.PanelArray[k].BorderStyle = BorderStyle.FixedSingle;
                    //this.PanelArray[k].BackColor = ConvRankToColor(tmpRank);
                    
                    if (i < Heaf)
                    {
                        this.PanelArray[k].Location = new Point(StartYPosition + k * YPosition, MarginLoc + gStartPotision - 5);
                    }
                    else
                    {
                        this.PanelArray[k].Location = new Point(890, MarginLoc * j + gStartPotision);
                    }
                  
                    /* 着順に合わせて背景色を変える */
                    RankColor = ConvRankToColor(SEdata.RaceHist1.rank);
                    this.RankArray[k].BackColor = RankColor;
                    this.labelArray[k].BackColor = RankColor;
                    this.DateArray[k].BackColor = RankColor;
                    this.KaisaiArray[k].BackColor = RankColor;
                    this.GradeArray[k].BackColor = RankColor;
                    this.NinkiArray[k].BackColor = RankColor;
                    this.JockeyArray[k].BackColor = RankColor;
                    this.TosuArray[k].BackColor = RankColor;
                    this.TrackArray[k].BackColor = RankColor;
                    this.TimeArray[k].BackColor = RankColor;
                    this.Time1Array[k].BackColor = RankColor;
                    this.Time2Array[k].BackColor = RankColor;
                    this.Time3Array[k].BackColor = RankColor;
                    this.TukaArray[k].BackColor = RankColor;
                    this.AiteUmaArray[k].BackColor = RankColor;
                    this.TimeDiffArray[k].BackColor = RankColor;
                    this.PanelArray[k].BackColor = RankColor;

                    /* レコードが出たレースの場合はタイムを赤表示する */
                    if(SEdata.RaceHist1.RecornUpdateFlag)
                    {
                        this.TimeArray[k].BackColor = Color.Red;
                        this.TimeArray[k].BackColor = Color.White;
                    }
                    
                    
                }

                this.Controls.AddRange(this.DateArray);

                //フォームにコントロールを追加：上から優先
                this.Controls.AddRange(this.TimeDiffArray);
                this.Controls.AddRange(this.labelArray);
                this.Controls.AddRange(this.GradeArray);
                

                this.Controls.AddRange(this.TukaArray);
                this.Controls.AddRange(this.AiteUmaArray);

                this.Controls.AddRange(this.TimeArray);
                this.Controls.AddRange(this.NinkiArray);
                this.Controls.AddRange(this.JockeyArray);
                this.Controls.AddRange(this.TosuArray);
                this.Controls.AddRange(this.TrackArray);
                this.Controls.AddRange(this.Time1Array);
                this.Controls.AddRange(this.Time2Array);
                this.Controls.AddRange(this.Time3Array);
                this.Controls.AddRange(this.Time3Array);
                this.Controls.AddRange(this.RankArray);
                this.Controls.AddRange(this.KaisaiArray);
                this.Controls.AddRange(this.DateArray);


                this.Controls.AddRange(this.PanelArray);
            }
            this.SuspendLayout();



            this.ResumeLayout(false);


        }
        
        private String ConvToCournerInfo(String param)
        {
            switch(param.Length)
            {
                case 2:
                    return param;
                case 4:
                case 6:
                case 8:
                    return ConvToCournerInfo2(ref param);
                case 0:
                default:
                    return "";
            }
        }

        private String ConvToCournerInfo2(ref String param)
        {
            String ret = "";
            for(int i=0; i<param.Length; i = i + 2)
            {
                if(param.Substring(i,2) == "00")
                {
                    continue;
                }

                if(ret.Length != 0)
                {
                    ret += "-";
                }
                
                ret += param.Substring(i,2);
            }
            return ret;
        }

        private Color ConvRankToColor(String Rank)
        {
            switch(Rank)
            {
                case "01":
                    return Color.Plum;
                case "02":
                    return Color.LemonChiffon;
                case "03":
                    return Color.LightBlue;
                default:
                    return Color.Transparent;

            }
        }

        private void NewsPaperForm_Load_1(object sender, EventArgs e)
        {
            NewsPaperForm_Load(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }


}
