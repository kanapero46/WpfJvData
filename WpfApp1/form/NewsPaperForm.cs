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
        
            int Heaf = 18 / 2;
            int MarginLoc = 160;

            for (int i = 0, j = 0; i < MAX_TOSU; i++)
            {
                //枠線
                this.PanelArray[i] = new Panel();
                //プロパティ設定
                this.PanelArray[i].Name = "Panel" + i.ToString();
                this.PanelArray[i].Size = new Size(new Point(410, 320));
                this.PanelArray[i].BorderStyle = BorderStyle.FixedSingle;
                this.PanelArray[i].ForeColor = Color.Transparent;
                if (i < Heaf)
                {
                    this.PanelArray[i].Location = new Point(425, MarginLoc * i + 277);
                }
                else
                {
                    this.PanelArray[i].Location = new Point(890, MarginLoc * j + 360);
                }

                if (i > Heaf)
                {
                    j++;
                }
                
            }



            for (int i = 0,j=0; i < MAX_TOSU; i++)
            {
                this.labelArray[i] = new Label();
                //プロパティ設定
                this.labelArray[i].Name = "Name" + i.ToString();
                this.labelArray[i].Size = new Size(250, 30);
                this.labelArray[i].Font = new Font("Meiryo UI", 10, FontStyle.Bold);
                this.labelArray[i].Text = "ヴィクトリアマイル";
                if(i < Heaf)
                {
                    this.labelArray[i].Location = new Point(430, MarginLoc * i + 300);
                }
                else
                {
                    this.labelArray[i].Location = new Point(1000, MarginLoc * j + 390);
                }

                //着順
                this.RankArray[i] = new Label();
                //プロパティ設定
                this.RankArray[i].Name = "Rank" + i.ToString();
                this.RankArray[i].Top = 0;
                this.RankArray[i].Width = 50;
                this.RankArray[i].Size = new Size(110, 75);
                this.RankArray[i].Font = new Font("Meiryo UI", 20, FontStyle.Bold);
                this.RankArray[i].Text = "外" ;
                this.RankArray[i].TextAlign = ContentAlignment.BottomCenter;
                //this.RankArray[i].BackColor = Color.Ivory;
                if (i < Heaf)
                {
                    this.RankArray[i].Location = new Point(720, MarginLoc * i + 277);
                }
                else
                {
                    this.RankArray[i].Location = new Point(1000, MarginLoc * j + 270);

                }

                //日付
                this.DateArray[i] = new Label();
                this.DateArray[i].Name = "Date" + i.ToString();
                this.DateArray[i].Size = new Size(150, 30);
                this.DateArray[i].Font = new Font("Meiryo UI", 7);
                this.DateArray[i].Text = "20190105";
                //this.DateArray[i].BackColor = Color.Aqua;
                if (i < Heaf)
                {
                    this.DateArray[i].Location = new Point(430, MarginLoc * i + 277);
                }
                else
                {
                    this.DateArray[i].Location = new Point(820, MarginLoc * j + 275);
                    j++;
                }

                //開催
                this.KaisaiArray[i] = new Label();
                this.KaisaiArray[i].Name = "Date" + i.ToString();
                this.KaisaiArray[i].Size = new Size(150, 30);
                this.KaisaiArray[i].Font = new Font("Meiryo UI", 7);
                this.KaisaiArray[i].Text = "2東京12";
                if (i < Heaf)
                {
                    this.KaisaiArray[i].Location = new Point(560, MarginLoc * i + 277);
                }
                else
                {
                    this.KaisaiArray[i].Location = new Point(1050, MarginLoc * j + 275);
                }


                //グレード
                this.GradeArray[i] = new Label();
                //プロパティ設定
                this.GradeArray[i].Name = "Grade" + i.ToString();
                this.GradeArray[i].Font = new Font("Meiryo UI", 8);
                this.GradeArray[i].Size = new Size(70, 30);
                this.GradeArray[i].Text = "特別";
                if (i < Heaf)
                {
                    this.GradeArray[i].Location = new Point(670, MarginLoc * i + 277);
                }
                else
                {
                    this.GradeArray[i].Location = new Point(830, MarginLoc * j + 275);
                }

                //人気
                this.NinkiArray[i] = new Label();
                //プロパティ設定
                this.NinkiArray[i].Name = "Ninki" + i.ToString();
                this.NinkiArray[i].Font = new Font("メイリオ", 8);
                this.NinkiArray[i].Size = new Size(100, 30);
                this.NinkiArray[i].Text = "18人";

                if (i < Heaf)
                {
                    this.NinkiArray[i].Location = new Point(660, MarginLoc * i + 330);
                }
                else
                {
                    this.NinkiArray[i].Location = new Point(890, MarginLoc * j + 330);
                }

                //騎手
                this.JockeyArray[i] = new Label();
                //プロパティ設定
                this.JockeyArray[i].Name = "Jockey" + i.ToString();
                this.JockeyArray[i].Size = new Size(250, 30);
                this.JockeyArray[i].Font = new Font("Meiryo UI", 8);
                this.JockeyArray[i].Text = "☆藤田 菜七子 (53.5kg)";

                if (i < Heaf)
                {
                    this.JockeyArray[i].Location = new Point(430, MarginLoc * i + 381);
                }
                else
                {
                    this.JockeyArray[i].Location = new Point(890, MarginLoc * j + 360);
                }

                //頭数
                this.TosuArray[i] = new Label();
                //プロパティ設定
                this.TosuArray[i].Name = "Tosu" + i.ToString();
                this.TosuArray[i].Size = new Size(200, 30);
                this.TosuArray[i].Font = new Font("Meiryo UI", 8);
                this.TosuArray[i].Text = "18 ト 18";

                if (i < Heaf)
                {
                    this.TosuArray[i].Location = new Point(570, MarginLoc * i + 330);
                }
                else
                {
                    this.TosuArray[i].Location = new Point(830, MarginLoc * j + 390);
                }

                //頭数
                this.TrackArray[i] = new Label();
                //プロパティ設定
                this.TrackArray[i].Name = "Tosu" + i.ToString();
                this.TrackArray[i].Size = new Size(200, 30);
                this.TrackArray[i].Font = new Font("Meiryo UI", 8);
                this.TrackArray[i].Text = "ダート 2400m";

                if (i < Heaf)
                {
                    this.TrackArray[i].Location = new Point(430, MarginLoc * i + 330);
                }
                else
                {
                    this.TrackArray[i].Location = new Point(830, MarginLoc * j + 390);
                }

                //勝ちタイム
                this.TimeArray[i] = new Label();
                //プロパティ設定
                this.TimeArray[i].Name = "Time" + i.ToString();
                this.TimeArray[i].Size = new Size(120, 30);
                this.TimeArray[i].Font = new Font("Meiryo UI", 8);
                this.TimeArray[i].Text = "R 2:21.6";
                //this.TimeArray[i].BackColor = Color.IndianRed;
                if (i < Heaf)
                {
                    this.TimeArray[i].Location = new Point(430, MarginLoc * i + 355);
                }
                else
                {
                    this.TimeArray[i].Location = new Point(830, MarginLoc * j + 390);
                }

                //先3F/5F
                //this.Time1Array[i] = new Label();
                ////プロパティ設定
                //this.Time1Array[i].Name = "Time1" + i.ToString();
                //this.Time1Array[i].Size = new Size(90, 30);
                //this.Time1Array[i].Font = new Font("Meiryo UI", 8);
                //this.Time1Array[i].Text = "60.2 →";

                //if (i < Heaf)
                //{
                //    this.Time1Array[i].Location = new Point(560, MarginLoc * i + 355);
                //}
                //else
                //{
                //    this.Time1Array[i].Location = new Point(830, MarginLoc * j + 390);
                //}

                //中タイム
                this.Time2Array[i] = new Label();
                //プロパティ設定
                this.Time2Array[i].Name = "Time2" + i.ToString();
                this.Time2Array[i].Size = new Size(90, 30);
                this.Time2Array[i].Font = new Font("Meiryo UI", 8);
                this.Time2Array[i].Text = "47.2 →";

                if (i < Heaf)
                {
                    this.Time2Array[i].Location = new Point(645, MarginLoc * i + 355);
                }
                else
                {
                    this.Time2Array[i].Location = new Point(830, MarginLoc * j + 390);
                }

                //上がり3F
                this.Time3Array[i] = new Label();
                //プロパティ設定
                this.Time3Array[i].Name = "Time3" + i.ToString();
                this.Time3Array[i].Size = new Size(90, 30);
                this.Time3Array[i].Font = new Font("Meiryo UI", 8);
                this.Time3Array[i].Text = "32.3";

                if (i < Heaf)
                {
                    this.Time3Array[i].Location = new Point(735, MarginLoc * i + 355);
                }
                else
                {
                    this.Time3Array[i].Location = new Point(830, MarginLoc * j + 390);
                }

                //通過順
                this.TukaArray[i] = new Label();
                //プロパティ設定
                this.TukaArray[i].Name = "Tuka" + i.ToString();
                this.TukaArray[i].Size = new Size(150, 30);
                this.TukaArray[i].Font = new Font("Meiryo UI", 8);
                //this.TukaArray[i].BackColor = Color.Ivory;
                this.TukaArray[i].Text = "18-18-18-18";
                this.TukaArray[i].TextAlign = ContentAlignment.MiddleRight;

                if (i < Heaf)
                {
                    this.TukaArray[i].Location = new Point(681, MarginLoc * i + 381);
                }
                else
                {
                    this.TukaArray[i].Location = new Point(890, MarginLoc * j + 360);
                }

                //相手馬
                this.AiteUmaArray[i] = new Label();
                //プロパティ設定
                this.AiteUmaArray[i].Name = "AiteUma" + i.ToString();
                this.AiteUmaArray[i].Size = new Size(250, 27);
                this.AiteUmaArray[i].Font = new Font("Meiryo UI", 8);
                this.AiteUmaArray[i].Text = "テイクオーバーターゲット";
                //this.AiteUmaArray[i].BackColor = Color.Bisque;
                if (i < Heaf)
                {
                    this.AiteUmaArray[i].Location = new Point(430, MarginLoc * i + 408);
                }
                else
                {
                    this.AiteUmaArray[i].Location = new Point(890, MarginLoc * j + 360);
                }

                //着差
                this.TimeDiffArray[i] = new Label();
                //プロパティ設定
                this.TimeDiffArray[i].Name = "TimeDiff" + i.ToString();
                this.TimeDiffArray[i].Size = new Size(75, 27);
                this.TimeDiffArray[i].BackColor = Color.Transparent;
                this.TimeDiffArray[i].Font = new Font("Meiryo UI", 8);
                this.TimeDiffArray[i].Text = "+99";
                //this.TimeDiffArray[i].BackColor = Color.BlueViolet;
                
                if (i < Heaf)
                {
                    this.TimeDiffArray[i].Location = new Point(750, MarginLoc * i + 408);
                }
                else
                {
                    this.TimeDiffArray[i].Location = new Point(890, MarginLoc * j + 360);
                }

                //枠線
                this.PanelArray[i] = new Panel();
                //プロパティ設定
                this.PanelArray[i].Name = "Panel" + i.ToString();         
                this.PanelArray[i].Size = new Size(new Point(410, 320));
                this.PanelArray[i].BorderStyle = BorderStyle.FixedSingle;
                //this.PanelArray[i].ForeColor = Color.Transparent;
                if (i < Heaf)
                {
                    this.PanelArray[i].Location = new Point(425, MarginLoc * i + 277);
                }
                else
                {
                    this.PanelArray[i].Location = new Point(890, MarginLoc * j + 360);
                }

                PanelArray[i].Controls.Add(RankArray[i]);

                if (i > Heaf)
                {
                    j++;
                }
            }

            //フォームにコントロールを追加
            this.Controls.AddRange(this.TimeDiffArray);
            this.Controls.AddRange(this.RankArray);
            this.Controls.AddRange(this.GradeArray);
            this.Controls.AddRange(this.labelArray);
            this.Controls.AddRange(this.KaisaiArray);
            this.Controls.AddRange(this.TukaArray);
            this.Controls.AddRange(this.AiteUmaArray);
            this.Controls.AddRange(this.DateArray);
            this.Controls.AddRange(this.TimeArray);
            this.Controls.AddRange(this.NinkiArray);
            this.Controls.AddRange(this.JockeyArray);
            this.Controls.AddRange(this.TosuArray);
            this.Controls.AddRange(this.TrackArray);
           // this.Controls.AddRange(this.Time1Array);
            this.Controls.AddRange(this.Time2Array);
            this.Controls.AddRange(this.Time3Array);
            this.Controls.AddRange(this.Time3Array);

            this.Controls.AddRange(this.PanelArray);


            this.SuspendLayout();



            this.ResumeLayout(false);

        }

        private void NewsPaperForm_Load_1(object sender, EventArgs e)
        {
            NewsPaperForm_Load(sender, e);
        }
    }


}
