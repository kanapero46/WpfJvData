using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private readonly int tmpStartPotision;

        dbAccess.dbConnect db = new dbAccess.dbConnect();

        public NewsPaperForm()
        {
            InitializeComponent();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

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

        private void NewsPaperForm_Load(object sender, EventArgs e)
        {
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

            /* DB読み込み用の配列宣言 */
            List<String> strArray = new List<string>();

            /* DB処理用のインスタンス宣言 */
            JvComDbData.JvDbRaData data = new JvComDbData.JvDbRaData();

            for (int k = 0; k < MAX_TOSU; k++)
            {
                //DB読み込み
                strArray.Clear();
                db.TextReader_Row("201902110501051101", "RA", 0, ref strArray);
                data.setData(ref strArray);

                //枠線
                this.PanelArray[k] = new Panel();
                //プロパティ設定
                this.PanelArray[k].Name = "Panel1_" + k.ToString();
                this.PanelArray[k].Size = new Size(new Point(YPosition, 530));
                this.PanelArray[k].BorderStyle = BorderStyle.FixedSingle;
                //this.PanelArray[k].ForeColor = Color.Transparent;
                this.PanelArray[k].Location = new Point(StartYPosition + k * YPosition, 220);
                
                this.BameiArray[k] = new Label();
                //プロパティ設定
                this.BameiArray[k].Name = "Bamei" + k.ToString();
                this.BameiArray[k].Size = new Size(40, 430);
                this.BameiArray[k].Font = new Font("メイリオ", 12, FontStyle.Bold);
                this.BameiArray[k].Text = BameiToLength("ブラストワンピース");
                this.BameiArray[k].TextAlign = ContentAlignment.TopCenter;
                this.BameiArray[k].Location = new Point((StartYPosition + 58) + (k * YPosition), 250);
                this.BameiArray[k].BackColor = Color.AliceBlue;
               
                //父
                this.FNameArray[k] = new Label();
                //プロパティ設定
                this.FNameArray[k].Name = "Fname" + k.ToString();
                this.FNameArray[k].Size = new Size(30, 430);
                this.FNameArray[k].Font = new Font("MS P ゴシック", 7);
                this.FNameArray[k].Text = BameiToLength("ハービンジャー");
                this.FNameArray[k].TextAlign = ContentAlignment.TopCenter;
                this.FNameArray[k].Location = new Point((StartYPosition + 95) + (k * YPosition), 250);
                
                //父父
                this.FFNameArray[k] = new Label();
                //プロパティ設定
                this.FFNameArray[k].Name = "Fname" + k.ToString();
                this.FFNameArray[k].Size = new Size(30, 400);
                this.FFNameArray[k].Font = new Font("MS P ゴシック", 7);
                this.FFNameArray[k].Text = BameiToLength("Danehill");
                this.FFNameArray[k].TextAlign = ContentAlignment.TopCenter;
                this.FFNameArray[k].Location = new Point((StartYPosition + 115) + (k * YPosition), 270);

                this.Controls.AddRange(this.FFNameArray);

                //母
                this.MNameArray[k] = new Label();
                //プロパティ設定
                this.MNameArray[k].Name = "Mname" + k.ToString();
                this.MNameArray[k].Size = new Size(30, 430);
                this.MNameArray[k].Font = new Font("MS P ゴシック", 7);
                this.MNameArray[k].Text = BameiToLength("ツルマルワンピース");
                this.MNameArray[k].TextAlign = ContentAlignment.TopCenter;
                this.MNameArray[k].Location = new Point((StartYPosition - 120) + (k * YPosition), 250);

                //母父
                this.MFNameArray[k] = new Label();
                //プロパティ設定
                this.MFNameArray[k].Name = "Mname" + k.ToString();
                this.MFNameArray[k].Size = new Size(30, 430);
                this.MFNameArray[k].Font = new Font("MS P ゴシック", 7);
                this.MFNameArray[k].Text = BameiToLength("サンデーサイレンス");
                this.MFNameArray[k].TextAlign = ContentAlignment.TopCenter;
                this.MFNameArray[k].Location = new Point((StartYPosition - 145) + (k * YPosition), 270);

                //負担
                this.JfutanArray[k] = new Label();
                //プロパティ設定
                this.JfutanArray[k].Name = "JFutan" + k.ToString();
                this.JfutanArray[k].Size = new Size(YPosition - 3, 30);
                this.JfutanArray[k].Font = new Font("MS P ゴシック", 7);
                this.JfutanArray[k].Text = "(53.5kg)";
                this.JfutanArray[k].TextAlign = ContentAlignment.MiddleCenter;
                this.JfutanArray[k].Location = new Point((StartYPosition + 2) + (k * YPosition), 685);
                this.JfutanArray[k].BackColor = Color.AliceBlue;
                this.Controls.AddRange(this.JfutanArray);

                //騎手
                this.JnameArray[k] = new Label();
                //プロパティ設定
                this.JnameArray[k].Name = "Jname" + k.ToString();
                this.JnameArray[k].Size = new Size(YPosition - 3, 30);
                this.JnameArray[k].Font = new Font("MS P ゴシック", 8);
                this.JnameArray[k].Text = "☆池添 謙一";
                this.JnameArray[k].TextAlign = ContentAlignment.MiddleCenter;
                this.JnameArray[k].Location = new Point((StartYPosition + 2) + (k * YPosition), 715);
                this.JnameArray[k].BackColor = Color.AliceBlue;
                this.Controls.AddRange(this.JnameArray);
            }




            this.Controls.AddRange(this.BameiArray);
            this.Controls.AddRange(this.MNameArray);
            this.Controls.AddRange(this.FNameArray);
            this.Controls.AddRange(this.MFNameArray);
            this.Controls.AddRange(this.PanelArray);

           
            this.labelArray = new System.Windows.Forms.Label[MAX_TOSU];

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
               
                for (int k = 0, j = 0; k < MAX_TOSU; k++)
                {
                    this.labelArray[k] = new Label();
                    //プロパティ設定
                    this.labelArray[k].Name = "Name" + k.ToString() + i.ToString();
                    this.labelArray[k].Size = new Size(147, 30);
                    this.labelArray[k].Font = new Font("Meiryo UI", 10, FontStyle.Bold);
                    this.labelArray[k].Text = "フェブラリー";
                    //this.labelArray[k].BackColor = Color.AliceBlue;
                    this.labelArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc + gStartPotision + 23);
                    
                    //着順
                    this.RankArray[k] = new Label();
                    //プロパティ設定
                    this.RankArray[k].Name = "Rank" + k.ToString() + i.ToString();
                    this.RankArray[k].Top = 0;
                    this.RankArray[k].Width = 50;
                    this.RankArray[k].Size = new Size(75, 75);
                    this.RankArray[k].Font = new Font("Meiryo UI", 14, FontStyle.Bold);
                    this.RankArray[k].Text = "18";
                    this.RankArray[k].TextAlign = ContentAlignment.BottomCenter;
                    //this.RankArray[k].BackColor = Color.Ivory;
                    this.RankArray[k].Location = new Point(StartYPosition + 70 + k * YPosition, MarginLoc + gStartPotision + 30);
                    
                    //日付
                    this.DateArray[k] = new Label();
                    this.DateArray[k].Name = "Date" + k.ToString() + i.ToString();
                    this.DateArray[k].Size = new Size(105, 30);
                    this.DateArray[k].Font = new Font("Meiryo UI", 7);
                    this.DateArray[k].Text = "20190105";
                    //this.DateArray[k].BackColor = Color.Aqua;
                    if (i < Heaf)
                    {
                        this.DateArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc  + gStartPotision);
                    }
                    else
                    {
                        this.DateArray[k].Location = new Point(820, MarginLoc * j + gStartPotision);
                        j++;
                    }

                    //開催
                    this.KaisaiArray[k] = new Label();
                    this.KaisaiArray[k].Name = "Date" + k.ToString() + i.ToString();
                    this.KaisaiArray[k].Size = new Size(45, 30);
                    this.KaisaiArray[k].Font = new Font("Meiryo UI", 7);
                    this.KaisaiArray[k].Text = "東京";
                    if (i < Heaf)
                    {
                        this.KaisaiArray[k].Location = new Point(StartYPosition + 103 + k * YPosition, MarginLoc  + gStartPotision);
                    }
                    else
                    {
                        this.KaisaiArray[k].Location = new Point(1050, MarginLoc * j + gStartPotision);
                    }


                    //グレード
                    this.GradeArray[k] = new Label();
                    //プロパティ設定
                    this.GradeArray[k].Name = "Grade" + k.ToString() + i.ToString();
                    this.GradeArray[k].Font = new Font("Meiryo UI", 8);
                    this.GradeArray[k].Size = new Size(70, 30);
                    this.GradeArray[k].Text = "特別";
                    if (i < Heaf)
                    {
                        this.GradeArray[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 53);
                    }
                    else
                    {
                        this.GradeArray[k].Location = new Point(830, MarginLoc * j + gStartPotision);
                    }

                    //人気
                    this.NinkiArray[k] = new Label();
                    //プロパティ設定
                    this.NinkiArray[k].Name = "Ninki" + k.ToString() + i.ToString();
                    this.NinkiArray[k].Font = new Font("メイリオ", 8);
                    this.NinkiArray[k].Size = new Size(70, 30);
                    this.NinkiArray[k].Text = "18人";

                    if (i < Heaf)
                    {
                        this.NinkiArray[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 78);
                    }
                    else
                    {
                        this.NinkiArray[k].Location = new Point(890, MarginLoc * j + gStartPotision + 53);
                    }

                    //騎手
                    this.JockeyArray[k] = new Label();
                    //プロパティ設定
                    this.JockeyArray[k].Name = "Jockey" + k.ToString() + i.ToString();
                    this.JockeyArray[k].Size = new Size(147, 30);
                    this.JockeyArray[k].Font = new Font("Meiryo UI", 8);
                    this.JockeyArray[k].Text = "☆藤田 菜七";

                    if (i < Heaf)
                    {
                        this.JockeyArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc  + gStartPotision + 153);
                    }
                    else
                    {
                        this.JockeyArray[k].Location = new Point(890, MarginLoc * j + gStartPotision + 104);
                    }

                    //頭数
                    this.TosuArray[k] = new Label();
                    //プロパティ設定
                    this.TosuArray[k].Name = "Tosu" + k.ToString() + i.ToString();
                    this.TosuArray[k].Size = new Size(147, 30);
                    this.TosuArray[k].Font = new Font("Meiryo UI", 8);
                    this.TosuArray[k].Text = "18ト18";

                    if (i < Heaf)
                    {
                        this.TosuArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc  + gStartPotision + 257);
                    }
                    else
                    {
                        this.TosuArray[k].Location = new Point(830, MarginLoc * j + gStartPotision + 53);
                    }

                    //頭数
                    this.TrackArray[k] = new Label();
                    //プロパティ設定
                    this.TrackArray[k].Name = "Tosu" + k.ToString() + i.ToString();
                    this.TrackArray[k].Size = new Size(145, 30);
                    this.TrackArray[k].Font = new Font("Meiryo UI", 8);
                    this.TrackArray[k].Text = "ダート 2400m";

                    if (i < Heaf)
                    {
                        this.TrackArray[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc  + gStartPotision + 128);
                    }
                    else
                    {
                        this.TrackArray[k].Location = new Point(830, MarginLoc * j + gStartPotision + 53);
                    }

                    //勝ちタイム
                    this.TimeArray[k] = new Label();
                    //プロパティ設定
                    this.TimeArray[k].Name = "Time" + k.ToString() + i.ToString();
                    this.TimeArray[k].Size = new Size(120, 30);
                    this.TimeArray[k].Font = new Font("Meiryo UI", 8);
                    this.TimeArray[k].Text = "R 2:21.6";
                    //this.TimeArray[k].BackColor = Color.IndianRed;
                    if (i < Heaf)
                    {
                        this.TimeArray[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 103);
                    }
                    else
                    {
                        this.TimeArray[k].Location = new Point(830, MarginLoc * j + gStartPotision + 78);
                    }

                    //負担
                    this.Time1Array[k] = new Label();
                    //プロパティ設定
                    this.Time1Array[k].Name = "Futan" + k.ToString() + i.ToString();
                    this.Time1Array[k].Size = new Size(146, 30);
                    this.Time1Array[k].Font = new Font("Meiryo UI", 7);
                    this.Time1Array[k].Text = "(53.5kg)";
                    this.Time1Array[k].TextAlign = ContentAlignment.BottomCenter;

                    if (i < Heaf)
                    {
                        this.Time1Array[k].Location = new Point(StartYPosition + 2 + k * YPosition, MarginLoc + gStartPotision + 178);
                    }
                    else
                    {
                        this.Time1Array[k].Location = new Point(830, MarginLoc * j + 390);
                    }

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
                    this.Time3Array[k].Text = "32.3";

                    if (i < Heaf)
                    {
                        this.Time3Array[k].Location = new Point(StartYPosition + 90 + k * YPosition, MarginLoc  + gStartPotision + 203);
                    }
                    else
                    {
                        this.Time3Array[k].Location = new Point(830, MarginLoc * j + gStartPotision + 78);
                    }

                    //通過順
                    this.TukaArray[k] = new Label();
                    //プロパティ設定
                    this.TukaArray[k].Name = "Tuka" + k.ToString() + i.ToString();
                    this.TukaArray[k].Size = new Size(147, 30);
                    this.TukaArray[k].Font = new Font("Meiryo UI", 8);
                    //this.TukaArray[k].BackColor = Color.Ivory;
                    this.TukaArray[k].Text = "18-18-18-18";
                    //this.TukaArray[k].TextAlign = ContentAlignment.MiddleCenter;

                    if (i < Heaf)
                    {
                        this.TukaArray[k].Location = new Point(StartYPosition + 1 + k * YPosition, MarginLoc  + gStartPotision + 230);
                    }
                    else
                    {
                        this.TukaArray[k].Location = new Point(890, MarginLoc * j + gStartPotision + 104);
                    }

                    //相手馬
                    this.AiteUmaArray[k] = new Label();
                    //プロパティ設定
                    this.AiteUmaArray[k].Name = "AiteUma" + k.ToString() + i.ToString();
                    this.AiteUmaArray[k].Size = new Size(144, 27);
                    this.AiteUmaArray[k].Font = new Font("Meiryo UI", 8);
                    this.AiteUmaArray[k].Text = "テイクオーバータ";
                    //this.AiteUmaArray[k].BackColor = Color.Bisque;
                    if (i < Heaf)
                    {
                        this.AiteUmaArray[k].Location = new Point(StartYPosition + 5 + k * YPosition, MarginLoc  + gStartPotision + 282);
                    }
                    else
                    {
                        this.AiteUmaArray[k].Location = new Point(890, MarginLoc * j + gStartPotision + 131);
                    }

                    //着差
                    this.TimeDiffArray[k] = new Label();
                    //プロパティ設定
                    this.TimeDiffArray[k].Name = "TimeDiff" + k.ToString() + i.ToString();
                    this.TimeDiffArray[k].Size = new Size(60, 27);
                    this.TimeDiffArray[k].BackColor = Color.Transparent;
                    this.TimeDiffArray[k].Font = new Font("Meiryo UI", 8);
                    this.TimeDiffArray[k].Text = "+99";
                    this.TimeDiffArray[k].TextAlign = ContentAlignment.MiddleRight;
                    //this.TimeDiffArray[k].BackColor = Color.BlueViolet;

                    if (i < Heaf)
                    {
                        this.TimeDiffArray[k].Location = new Point(StartYPosition + 85 + k * YPosition, MarginLoc  + gStartPotision + 257);
                    }
                    else
                    {
                        this.TimeDiffArray[k].Location = new Point(890, MarginLoc * j + gStartPotision + 131);
                    }

                    //枠線
                    this.PanelArray[k] = new Panel();
                    //プロパティ設定
                    this.PanelArray[k].Name = "Panel" + k.ToString() + i.ToString();
                    this.PanelArray[k].Size = new Size(new Point(YPosition, XPosition));
                    this.PanelArray[k].BorderStyle = BorderStyle.FixedSingle;
                    //this.PanelArray[k].ForeColor = Color.Transparent;
                    if (i < Heaf)
                    {
                        this.PanelArray[k].Location = new Point(StartYPosition + k * YPosition, MarginLoc + gStartPotision - 5);
                    }
                    else
                    {
                        this.PanelArray[k].Location = new Point(890, MarginLoc * j + gStartPotision);
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
        
        private void NewsPaperForm_Load_1(object sender, EventArgs e)
        {
            NewsPaperForm_Load(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }


}
