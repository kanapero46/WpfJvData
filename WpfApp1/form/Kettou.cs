using LibJvConv;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WpfApp1.Class;
using WpfApp1.dbAccess;
using WpfApp1.dbCom1;

namespace WpfApp1.form
{
    public partial class Kettou : Form
    {
        String Key;
        MainDataClass raceData; //レース情報
        List<MainDataHorceClass> ArrayHorceData = new List<MainDataHorceClass>(); //18頭分すべて
        dbConnect db = new dbConnect(); //DB読み書きクラス
        dbCom dbCom = new dbCom();
        MainWindow main = new MainWindow(); //MainWindowsクラス

        /* 現在表示している馬番のグローバル変数 */
        int NowPictureNum = 1;

        public Kettou()
        {
            InitializeComponent();
            InitComponentText();
        }

        public Kettou(String RA_Key)
        {
            InitializeComponent();
            InitComponentText();
            this.Key = RA_Key;
        }

        /** **********private*********** */
        private void InitComponentText()
        {
            pictureBox1.Controls.Add(BloodHorceName);
            pictureBox1.Controls.Add(FBooldName);
            pictureBox1.Controls.Add(BMSHorceName);
            pictureBox1.Controls.Add(FFBloodName);
            pictureBox1.Controls.Add(BMSType);
            pictureBox1.Controls.Add(MMFBooldName);
            pictureBox1.Controls.Add(FTypeName);
            pictureBox1.Controls.Add(FFMTypeName);
            BloodHorceName.BackColor = Color.Transparent;
            FBooldName.BackColor = Color.Transparent;
            BMSHorceName.BackColor = Color.Transparent;
            FFBloodName.BackColor = Color.Transparent;
            BMSType.BackColor = Color.Transparent;
            MMFBooldName.BackColor = Color.Transparent;
            FTypeName.BackColor = Color.Transparent;
            FFMTypeName.BackColor = Color.Transparent;
            this.BloodHorceName.Top = (this.pictureBox1.Height - this.BloodHorceName.Height) / 2 - 10;
            this.BloodHorceName.Left = (this.pictureBox1.Width - this.BloodHorceName.Width) / 25 - 30;
            this.FBooldName.Top = (this.pictureBox1.Height - this.FBooldName.Height) / 3 - 5;
            this.FBooldName.Left = (this.pictureBox1.Width - this.FBooldName.Width) / 2 + 40;
            this.BMSHorceName.Top = (this.pictureBox1.Height - this.BMSHorceName.Height) - ((this.pictureBox1.Height - this.BMSHorceName.Height) / 3 - 20);
            this.BMSHorceName.Left = (this.pictureBox1.Width - this.BMSHorceName.Width) / 2 + 40;
            this.FFBloodName.Top = (this.pictureBox1.Height - this.FFBloodName.Height) / 10;
            this.FFBloodName.Left = (this.pictureBox1.Width - this.FFBloodName.Width) - 25;
            this.BMSType.Top = (this.pictureBox1.Height - this.FFBloodName.Height) / 2;
            this.BMSType.Left = (this.pictureBox1.Width - this.FFBloodName.Width) - 30;
            this.MMFBooldName.Top = (this.pictureBox1.Height - this.MMFBooldName.Height) - ((this.pictureBox1.Height - this.MMFBooldName.Height) / 4 - 60);
            this.MMFBooldName.Left = (this.pictureBox1.Width - this.MMFBooldName.Width) - 20;
            this.FTypeName.Top = this.FFBloodName.Top + 110;
            this.FTypeName.Left = (this.pictureBox1.Width - this.FFBloodName.Width) + 15;
            this.FFMTypeName.Top = this.MMFBooldName.Top + 100;
            this.FFMTypeName.Left = (this.pictureBox1.Width - this.MMFBooldName.Width) +20;
            textBox6.Focus();
        }


        /* フォーム読み込み処理 */
        unsafe private void Kettou_Load(object sender, EventArgs e)
        {
            int ret;
            String Key;
            ret = InitRaceInfo(); //レース情報(SetData)

            if(ret == 0)
            {
                MessageBox.Show("レース情報の取得に失敗しました。");
                Console.WriteLine("InitRaceInfo = " + ret.ToString());
                this.Close();
                return;
            }

            DisableButtoninNum();

            Key = raceData.getRaceDate() + raceData.getRaceCource() + raceData.getRaceKaiji() + raceData.getRaceNichiji();
            ret = main.JvGetRTData(raceData.getRaceDate()); //速報データ取得
            InitTrackStatusInfo(Key);  //馬場状態書き込み
            SetFormDataWriter();

            //競走馬データ書き込み
            ret = SetHorceData();

            //データ書き込み：高速化のためdefault１番のみ書き込み
            WriteHorceData(1);

            NowPictureNum = 1;


        }

        unsafe private void InitTrackStatusInfo(String Key)
        {
            int LibCode;
            int GetKind;
            int ret;
            String tmp = "";
            List<String> str = new List<string>();
            String Track = raceData.getCourceTrack();
            LibCode = LibJvConvFuncClass.TRACK_CODE_SHORT;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, Track, ref tmp);

            if (tmp == "")
            {
                return;
            }

            GetKind = (tmp == "芝" ? 2 : 3);

            //競走馬情報

            tmp = "";
            ret = db.TextReader_Col(raceData.getRaceDate(), "WE", 0, ref str, Key);
            if (ret == 0 || str.Count() == 0)
            {
                weLabel.Text = "---";
                TrackDistance.Text = "---";
                HappyoTime.Text = "発表前";
            }
            else
            {
                LibCode = 2011;
                LibJvConvFuncClass.jvSysConvFunction(&LibCode, str[1], ref tmp);
                weLabel.Text = tmp;

                HappyoTime.Text = ConvertDateToLongDate(str[4]);

                if (str[4] == "00000000")
                {
                    HappyoTime.Text = "発売前時点";
                }
                else
                {
                    HappyoTime.Text = ConvertDateToLongDate(str[4]);
                }


                LibCode = 2010;
                LibJvConvFuncClass.jvSysConvFunction(&LibCode, str[GetKind], ref tmp);
                TrackDistance.Text = tmp;

            }


            TrackLabel.Text = (GetKind == 2 ? "芝" : "ダート");
            TrackLabel.BackColor = (GetKind == 2 ? Color.LightGreen : Color.Tan);

            LibCode = 2007;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, raceData.getRaceClass(), ref tmp);
            ClassLabel.Text = tmp;

            LibCode = 2006;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, raceData.getRaceKindKigo(), ref tmp);
            KigoLabel.Text = tmp;

            DistanceLabel.Text = raceData.getDistance();

            LibCode = LibJvConvFuncClass.TRACK_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, raceData.getCourceTrack(), ref tmp);
            TrackNameLabel.Text = tmp;



        }

        private int InitRaceInfo()
        {
            List<String> tmp = new List<string>();
            raceData = new MainDataClass();
            raceData.SET_RA_KEY(this.Key);

            /* レース未選択時はエラーで返す */
            if (Key == null || Key == "")
            {
                Console.WriteLine("InitRaceInfo Key NULL!!");
                return 0;
            }

            /* DBからレース情報読み込み */
            int ret = db.TextReader_Col(Key.Substring(0, 8), "RA", 0, ref tmp, Key);
            if (ret == 0)
            {
                Console.WriteLine("InitRaceInfo DB READ NULL!!");
                return 0;
            }

            raceData.setData(ref tmp);

            if (!(raceData.getRaceGrade() == "" || raceData.getRaceGrade() == "特別" || raceData.getRaceGrade() == "一般"))
            {
                /* 重賞レースの場合、レース名に加える */
                raceData.setRaceName(tmp[7] + "(" + tmp[16] + ")");
            }
            racename.Text = raceData.getRaceName();
            return 1;
        }

        unsafe private void SetFormDataWriter()
        {
            int Code;
            Code = 2002;
            String LibTmp = "";

            LibJvConvFuncClass.jvSysConvFunction(&Code, raceData.getWeekDay(), ref LibTmp);
            this.Date.Text = ConvertDateToDate(raceData.getRaceDate()) + "(" + (LibTmp == "祝" ?  LibTmp : LibTmp + "曜") +")";

            Code = LibJvConvFuncClass.COURCE_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&Code, raceData.getRaceCource(), ref LibTmp);
            this.Kaisai.Text = "第" + Int32.Parse(raceData.getRaceKaiji()) + "回" + LibTmp + Int32.Parse(raceData.getRaceNichiji()) + "日目";

            RaceNum.Text = (raceData.getRaceNum().Length ==　1 ? " " : "");
            RaceNum.Text += raceData.getRaceNum() + "R";

            if(raceData.getRaceGradeKai() != 0)
            {
                this.kaiji.Text = "第" + raceData.getRaceGradeKai() + "回";
            }
            else
            {
                this.kaiji.Text = "";
            }

            this.racename.Text = raceData.getRaceName();

            LibTmp = "";
            Code = LibJvConvFuncClass.RACE_SHUBETSU_LONG_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&Code, raceData.getOldYear(), ref LibTmp);
            this.OldYear.Text = LibTmp;

            //発走時間
            this.label4.Text = raceData.getRaceStartTime().Substring(0, 2) + "時" + raceData.getRaceStartTime().Substring(2, 2) + "分";


        }

        #region 競走馬データ読み込み処理
        private int SetHorceData()
        {
            MainDataHorceClass horceData; //馬情報
            String Key = raceData.GET_RA_KEY();
            String covData;
            String Libstr = "";
            List<String> LibArray = new List<string>();
            int All = 0;

            //ローカルデータ初期化
            ArrayHorceData.Clear();

            for (int i = 1; i<=18; i++)
            {
                horceData = new MainDataHorceClass();
                LibArray.Clear();
                covData = String.Format("{0:00}", i);
                //１頭分ずつ読み込み
                db.TextReader_Col(Key.Substring(0, 8), "SE", 0, ref LibArray, Key + covData);
                if (LibArray.Count() == 0)
                {
                    break;
                }

                horceData.SetSEData(LibArray); //SEデータをセット
                LibArray.Clear();              //tmpをクリア

                db.TextReader_Col("0", "UM", 0, ref LibArray, horceData.KettoNum1.ToString());
                if (LibArray.Count() == 0)
                {
                    break;
                }
                horceData.SetUMData(LibArray); //UMデータをセット
                All++;
                EnableButtoninNum(All);         //ボタンを有効化

                LibArray.Clear();           //tmpをクリア

                dbCom.DbComGetOldRunDataMapping(horceData.KettoNum1.ToString(), ref LibArray, 1);  //前走データを取得 
                horceData.SetSEMSTData(LibArray);

               // dbCom.DbComGetOldRunDataMapping


                ArrayHorceData.Add(horceData);  //SE・UMデータをグローバルに追加
            }

            return All;
        }
        #endregion


        #region 馬番ボタン有効化
        private void EnableButtoninNum(int num)
        {
            switch(num)
            {
                case 1: u1.Text = num.ToString(); u1.Visible = true; break;
                case 2: button24.Text = num.ToString(); button24.Visible = true; break;
                case 3: button27.Text = num.ToString(); button27.Visible = true; break;
                case 4: button26.Text = num.ToString(); button26.Visible = true; break;
                case 5: button23.Text = num.ToString(); button23.Visible = true; break;
                case 6: button20.Text = num.ToString(); button20.Visible = true; break;
                case 7: button19.Text = num.ToString(); button19.Visible = true; break;
                case 8: button22.Text = num.ToString(); button22.Visible = true; break;
                case 9: button21.Text = num.ToString(); button21.Visible = true; break;
                case 10: button35.Text = num.ToString(); button35.Visible = true; break;
                case 11: button34.Text = num.ToString(); button34.Visible = true; break;
                case 12: button1.Text = num.ToString(); button1.Visible = true; break;
                case 13: button36.Text = num.ToString(); button36.Visible = true; break;
                case 14: button33.Text = num.ToString(); button33.Visible = true; break;
                case 15: button29.Text = num.ToString(); button29.Visible = true; break;
                case 16: button28.Text = num.ToString(); button28.Visible = true; break;
                case 17: button32.Text = num.ToString(); button32.Visible = true; break;
                case 18: button31.Text = num.ToString(); button31.Visible = true; break;
                default: break;
            }
        }
        #endregion

        #region 馬番ボタン無効
        private void DisableButtoninNum()
        {
               u1.Visible = false;       
               button24.Visible = false; 
               button27.Visible = false; 
               button26.Visible = false; 
               button23.Visible = false; 
               button20.Visible = false; 
               button19.Visible = false; 
               button22.Visible = false; 
               button21.Visible = false; 
                button35.Visible = false;
                button34.Visible = false;
                button1.Visible = false; 
                button36.Visible = false;
                button33.Visible = false;
                button29.Visible = false;
                button28.Visible = false;
                button32.Visible = false;
                button31.Visible = false;

        }
        #endregion

        #region フォームへの書き込み・判定処理
        private int WriteHorceData(int num)
        {
            if (num == 0 || num >= 19 || ArrayHorceData.Count == 0)
            {
                return 0;
            }

            //配列添字用に馬番を-1に設定する
            int ArrayNum = num - 1;

            //テキスト書き込み
            this.label35.Text = num.ToString() + "：" + ArrayHorceData[ArrayNum].Name1;
            this.label35.Text += (ArrayHorceData[ArrayNum].M1 == "" ? "" : "（母：" + ArrayHorceData[ArrayNum].M1 + "）");
            this.label20.Text = ArrayHorceData[ArrayNum].Jockey1 + "(" + ArrayHorceData[ArrayNum].Futan1.Substring(0, 2) + "." + ArrayHorceData[ArrayNum].Futan1.Substring(2, 1) + "kg)";
            this.BloodHorceName.Text = ArrayHorceData[ArrayNum].Name1;
            this.textBox6.Text = num.ToString();
            this.FBooldName.Text = ArrayHorceData[ArrayNum].F1;
            this.BMSHorceName.Text = ArrayHorceData[ArrayNum].FM1;
            this.MMFBooldName.Text = ArrayHorceData[ArrayNum].FFM1;
            this.FFBloodName.Text = ArrayHorceData[ArrayNum].FF1;

            //血統タイプをDBから読み込み
            this.FTypeName.Text = dbCom.DbComSearchBloodType(ArrayHorceData[ArrayNum].F_NUM1, ArrayHorceData[ArrayNum].FF_NUM1, ArrayHorceData[ArrayNum].FFF_NUM1);
            this.BMSType.Text = dbCom.DbComSearchBloodType(ArrayHorceData[ArrayNum].FM_NUM1, ArrayHorceData[ArrayNum].FMM_NUM1);
            this.FFMTypeName.Text = dbCom.DbComSearchBloodType(ArrayHorceData[ArrayNum].FFM_NUM1);

            this.FTypeName.Text += (this.FTypeName.Text.Length >= 1 ? "系" : "");
            this.BMSType.Text += (this.BMSType.Text.Length >= 1 ? "系" : "");
            this.FFMTypeName.Text += (this.FFMTypeName.Text.Length >= 1 ? "系" : "");

            //出走条件
            if (JudgeNotDomestic(ArrayHorceData[ArrayNum].UmaKigou1))
            {
                this.textBox1.BackColor = Color.Yellow;
                this.textBox1.ForeColor = Color.Black;
            }
            else
            {
                this.textBox1.BackColor = Color.White;
                this.textBox1.ForeColor = Color.White;
            }

            if (ArrayHorceData[ArrayNum].RaceHist1.distance == null|| Int32.Parse(ArrayHorceData[ArrayNum].RaceHist1.distance) == Int32.Parse(raceData.getDistance()) ||
                ArrayHorceData[ArrayNum].RaceHist1.distance == "" )
            {
                //前走と距離が同じ・または前走距離が不明な場合・新馬戦などの前走成績がないとき
                this.textBox2.Text = "";
                this.textBox2.BackColor = Color.White;
                this.textBox2.ForeColor = Color.White;
            }
            else if(Int32.Parse(ArrayHorceData[ArrayNum].RaceHist1.distance) > Int32.Parse(raceData.getDistance()))
            {
                //前走から距離短縮
                this.textBox2.Text = "短";
                this.textBox2.BackColor = Color.MediumVioletRed;
                this.textBox2.ForeColor = Color.White;
            }
            else if(Int32.Parse(ArrayHorceData[ArrayNum].RaceHist1.distance) < Int32.Parse(raceData.getDistance()))
            {
                //前走から距離延長
                this.textBox2.Text = "長";
                this.textBox2.BackColor = Color.DarkBlue;
                this.textBox2.ForeColor = Color.White;
            }

            if(ArrayHorceData[ArrayNum].RaceHist1.jockey == null　|| ArrayHorceData[ArrayNum].RaceHist1.jockey == "")
            {
                //前走と距離が同じ・または前走距離が不明な場合・新馬戦などの前走成績がないとき
                this.textBox3.Text = "";
                this.textBox3.BackColor = Color.White;
                this.textBox3.ForeColor = Color.White;
            }
            else if(ArrayHorceData[ArrayNum].RaceHist1.jockey == ArrayHorceData[ArrayNum].Jockey1)
            {
                this.textBox3.Text = "";
                this.textBox3.BackColor = Color.White;
                this.textBox3.ForeColor = Color.White;
            }
            else
            {
                this.textBox3.Text = "乗替";
                this.textBox3.BackColor = Color.Yellow;
                this.textBox3.ForeColor = Color.Red;
            }

            #region 前走との間隔
            this.textBox4.Text = ConvertDateToDiff(raceData.getRaceDate(), ArrayHorceData[ArrayNum].RaceHist1.RaceDate);

            if(this.textBox4.Text == "")
            {
                this.textBox4.BackColor = Color.White;
            }
            else if(Int32.Parse(this.textBox4.Text) <= 14)
            {
                //15週～30週(３ヶ月～６ヶ月)ぶり
                this.textBox4.BackColor = Color.White;
                this.textBox4.ForeColor = Color.Black;
            }
            else if(Int32.Parse(this.textBox4.Text) >= 15 && 30 >= Int32.Parse(this.textBox4.Text))
            {
                //15週～30週(３ヶ月～６ヶ月)ぶり
                this.textBox4.BackColor = Color.Yellow;
                this.textBox4.ForeColor = Color.Black;
            }
            else
            {
                //30週以上（６が月以上）ぶり
                this.textBox4.BackColor = Color.DarkRed;
                this.textBox4.ForeColor = Color.White;
            }
            #endregion


            #region 前走成績更新

            SetOldRace(num);
            #endregion
            return num;
        }
        #endregion

        #region 外国産チェック（true：[外]or(外)）
        public Boolean JudgeNotDomestic(String UmaKigo)
        {
            switch(UmaKigo)
            {
                case "06":
                case "11":
                case "16":
                case "20":
                case "23":
                case "26":
                case "27":
                case "40":
                case "41":
                    return true;
            }
            return false;
        }
        #endregion

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BloodHorceName_Click(object sender, EventArgs e)
        {

        }

        private String ConvertDateToDate(String Date)
        {
            return Date.Substring(0, 4) + "年" + Int32.Parse(Date.Substring(4, 2)) + "月" + Int32.Parse(Date.Substring(6, 2)) + "日";
        }

        private String ConvertDateToLongDate(String DateTime)
        {
            return Int32.Parse(DateTime.Substring(0,2)) + "月" + Int32.Parse(DateTime.Substring(2,2)) + "日 " + Int32.Parse(DateTime.Substring(4, 2)) + "時" + Int32.Parse(DateTime.Substring(6, 2)) + "分";
        }

        #region レース引き算
        private String ConvertDateToDiff(String Date1, String Date2)
        {
            //エラーチェック
            if(Date1 == "" || Date1 == null) { return ""; }
            if(Date2 =="" || Date2 == null) { return ""; }

            int Num1 = Int32.Parse(Date1);
            int Num2 = Int32.Parse(Date2);

            DateTime d1 = new DateTime(Int32.Parse(Date1.Substring(0, 4)),
                                       Int32.Parse(Date1.Substring(4, 2)),
                                       Int32.Parse(Date1.Substring(6, 2))
                                       );
            DateTime d2 = new DateTime(Int32.Parse(Date2.Substring(0, 4)),
                                       Int32.Parse(Date2.Substring(4, 2)),
                                       Int32.Parse(Date2.Substring(6, 2))
                                       );

            if (Int32.Parse(Date1) > Int32.Parse(Date2))
            {
                TimeSpan ts = d1 - d2;
                return (Int32.Parse(ts.Days.ToString()) / 7).ToString();
            }
            else
            {
                TimeSpan ts = d2- d1;
                return(Int32.Parse(ts.Days.ToString()) / 7).ToString();
            }
        }
        #endregion

        private void button25_Click(object sender, EventArgs e)
        {

        }

        #region 馬番ボタン押下イベント
        private void ChangeNumDisplay_Click(object sender, EventArgs e)
        {
            /* 処理削減のため、同じ馬番は更新しない */
            if(NowPictureNum == Int32.Parse(((Button)sender).Text)){ return; }
            NowPictureNum= WriteHorceData(Int32.Parse(((Button)sender).Text));
             
        }
        #endregion

        #region 前走成績を更新（仕様変更#15対応）
        unsafe private void SetOldRace(int num)
        {
            if(num < 0|| num > 19)
            {
                return;
            }

            List<String> Libtmp = new List<string>();
            int CODE;
            int Arraynum = num - 1;
            String tmp = "";

            int res =
            dbCom.DbComGetOldRunDataMapping(ArrayHorceData[Arraynum].KettoNum1.ToString(), ref Libtmp, 1); //DBから前走データを取得

            if (res == 0) { return; } //DBから取得失敗・もしくはデータなし

            /* 前走データをクラスに書き込み */
            ArrayHorceData[Arraynum].SetSEMSTData(Libtmp);

            /* DB書き込み */
            oldDataView.DefaultCellStyle.Font = new Font("Meiryo UI", 12);
            oldDataView.RowTemplate.Height = 100;
            String Grade = ArrayHorceData[Arraynum].RaceHist1.grade;
            String RaceName = ArrayHorceData[Arraynum].RaceHist1.raceName10;

            if (Grade == "" || Grade == "一般")
            {
                /* 一般競走では10文字競走名が空白なため、本題を入れる */
                RaceName = ArrayHorceData[Arraynum].RaceHist1.raceName;
            }
            else if(Grade == "特別")
            {
                /* なにもしない */
            }
            else
            {
                /* 重賞レース */
                RaceName = RaceName + "(" + Grade + ")";
            }

            CODE = LibJvConvFuncClass.COURCE_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, ArrayHorceData[Arraynum].RaceHist1.Cource, ref tmp);
            String Cource = tmp;

            CODE = LibJvConvFuncClass.TRACK_CODE_SHORT;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, ArrayHorceData[Arraynum].RaceHist1.track, ref tmp);
            String Track = tmp;

            oldDataView.Rows[0].Cells[0].Value = "1";
            oldDataView.Rows[0].Cells[1].Value = ArrayHorceData[Arraynum].RaceHist1.RaceDate;
            oldDataView.Rows[0].Cells[2].Value = Cource;
            oldDataView.Rows[0].Cells[3].Value = RaceName;
            oldDataView.Rows[0].Cells[4].Value = Track;
            oldDataView.Rows[0].Cells[5].Value = ArrayHorceData[Arraynum].RaceHist1.distance;
            oldDataView.Rows[0].Cells[6].Value = ArrayHorceData[Arraynum].RaceHist1.rank;
            oldDataView.Rows[0].Cells[7].Value = ArrayHorceData[Arraynum].RaceHist1.jockey;
            oldDataView.Rows[0].Cells[8].Value = ArrayHorceData[Arraynum].RaceHist1.jockey;



            oldDataView.Rows.Add("1",
                                 ArrayHorceData[Arraynum].RaceHist1.RaceDate,
                                 Cource,
                                 RaceName,
                                 Track,
                                 ArrayHorceData[Arraynum].RaceHist1.distance,
                                 ArrayHorceData[Arraynum].RaceHist1.rank,
                                 ArrayHorceData[Arraynum].RaceHist1.jockey,
                                 ArrayHorceData[Arraynum].RaceHist1.futan.Substring(0, 2) + ArrayHorceData[Arraynum].RaceHist1.futan.Substring(2, 1) + (ArrayHorceData[Arraynum].RaceHist1.futan.Length >= 2 ? "kg" : "")
                              );

        }
        #endregion


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
