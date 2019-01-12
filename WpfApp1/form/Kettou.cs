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
            this.FBooldName.Left = (this.pictureBox1.Width - this.FBooldName.Width) / 2 + 50;
            this.BMSHorceName.Top = (this.pictureBox1.Height - this.BMSHorceName.Height) - ((this.pictureBox1.Height - this.BMSHorceName.Height) / 3 - 20);
            this.BMSHorceName.Left = (this.pictureBox1.Width - this.BMSHorceName.Width) / 2 + 50;
            this.FFBloodName.Top = (this.pictureBox1.Height - this.FFBloodName.Height) / 10;
            this.FFBloodName.Left = (this.pictureBox1.Width - this.FFBloodName.Width) - 25;
            this.BMSType.Top = (this.pictureBox1.Height - this.FFBloodName.Height) / 2;
            this.BMSType.Left = (this.pictureBox1.Width - this.FFBloodName.Width) - 30;
            this.MMFBooldName.Top = (this.pictureBox1.Height - this.MMFBooldName.Height) - ((this.pictureBox1.Height - this.MMFBooldName.Height) / 4 - 60);
            this.MMFBooldName.Left = (this.pictureBox1.Width - this.MMFBooldName.Width) - 20;
            this.FTypeName.Top = this.FFBloodName.Top + 43;
            this.FTypeName.Left = (this.pictureBox1.Width - this.FTypeName.Width) - 10;
            this.FFMTypeName.Top = this.MMFBooldName.Top + 100;
            this.FFMTypeName.Left = (this.pictureBox1.Width - this.MMFBooldName.Width) +20;
        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        /* フォーム読み込み処理 */
        unsafe private void Kettou_Load(object sender, EventArgs e)
        {
            int ret;
            String Key;
            InitRaceInfo(); //レース情報(SetData)
            DisableButtoninNum();

            Key = raceData.getRaceDate() + raceData.getRaceCource() + raceData.getRaceKaiji() + raceData.getRaceNichiji();
            ret = main.JvGetRTData(raceData.getRaceDate()); //速報データ取得
            InitTrackStatusInfo(Key);  //馬場状態書き込み
            SetFormDataWriter();

            //競走馬データ書き込み
            ret = SetHorceData();

            //データ書き込み：高速化のためdefault１番のみ書き込み
            WriteHorceData(1);




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
            ret = db.TextReader_Col(raceData.getRaceDate(), "WE", 1, ref str, Key);
            if (ret != 1 || str.Count() == 0)
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

            /* DBからレース情報読み込み */
            int ret = db.TextReader_Col(Key.Substring(0, 8), "RA", 0, ref tmp, Key);
            if (ret != 1)
            {
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
            this.Date.Text = ConvertDateToDate(raceData.getRaceDate()) + "(" + LibTmp + "曜)";

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
            MainDataHorceClass horceData = new MainDataHorceClass(); //馬情報
            String Key = raceData.GET_RA_KEY();
            String covData;
            String Libstr = "";
            List<String> LibArray = new List<string>();
            int All = 0;

            //ローカルデータ初期化
            ArrayHorceData.Clear();

            for (int i = 1; i<=18; i++)
            {
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
                case 1: u1.Visible = true; break;
                case 2: button24.Visible = true; break;
                case 3: button27.Visible = true; break;
                case 4: button26.Visible = true; break;
                case 5: button23.Visible = true; break;
                case 6: button20.Visible = true; break;
                case 7: button19.Visible = true; break;
                case 8: button22.Visible = true; break;
                case 9: button21.Visible = true; break;
                case 10: button35.Visible = true; break;
                case 11: button34.Visible = true; break;
                case 12: button1.Visible = true; break;
                case 13: button36.Visible = true; break;
                case 14: button33.Visible = true; break;
                case 15: button29.Visible = true; break;
                case 16: button28.Visible = true; break;
                case 17: button32.Visible = true; break;
                case 18: button31.Visible = true; break;
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

        private void WriteHorceData(int num)
        {
            if(num == 0 || num >= 19)
            {
                return;
            }

            //テキスト書き込み
            this.label35.Text = num.ToString() + "：" + ArrayHorceData[num].Name1;
            this.label35.Text += (ArrayHorceData[num].M1 == "" ? "" : "（母：" + ArrayHorceData[num].M1 + "）");
            this.label20.Text = ArrayHorceData[num].Jockey1 + "(" + ArrayHorceData[num].Futan1.Substring(0,2) + "." + ArrayHorceData[num].Futan1.Substring(2, 1) + "kg)";
            this.BloodHorceName.Text = ArrayHorceData[num].Name1;
            this.textBox6.Text = num.ToString();
            this.FBooldName.Text = ArrayHorceData[num].F1;
            this.BMSHorceName.Text = ArrayHorceData[num].FM1;
            this.MMFBooldName.Text = ArrayHorceData[num].FFM1;
            this.FFBloodName.Text = ArrayHorceData[num].FF1;

            //血統タイプをDBから読み込み
            this.FTypeName.Text = dbCom.DbComSearchBloodType(ArrayHorceData[num].F_NUM1, ArrayHorceData[num].FF_NUM1, ArrayHorceData[num].FFF_NUM1);
            this.BMSType.Text = dbCom.DbComSearchBloodType(ArrayHorceData[num].FM_NUM1, ArrayHorceData[num].FMM_NUM1);
            this.FFMTypeName.Text = dbCom.DbComSearchBloodType(ArrayHorceData[num].FFM_NUM1);

            

        }

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

        private void button25_Click(object sender, EventArgs e)
        {

        }
    }
}
