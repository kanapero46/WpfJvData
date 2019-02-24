using LibJvConv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;   
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.dbAccess;
using WpfApp1.form;
using WpfApp1.Properties;
using Microsoft.VisualBasic;
using WpfApp1.dbCom1;

namespace WpfApp1.form
{
    public partial class Syutsuba : Form
    {
        /* DB書き込みクラス */
        dbConnect db;
        dbCom dbCom = new dbCom();
        Class.MainDataClass DataClass = new Class.MainDataClass();
        JvComDbData.JvDbRaData RaClassData = new JvComDbData.JvDbRaData();
        static String Cource;
        int CourceColor;
        MainWindow main = new MainWindow();

        /* 競走馬データ保存用 */
        List<Class.MainDataHorceClass> horceClasses;

        /* プログレスバー用ステータス */
        int ProgressStatus = 0;

        /* SEデータ取得用定数 */
        const int SE_WAKU = 5;
        const int SE_UMA = 6;
        const int SE_KETTO = 7;
        const int SE_NAME = 8;
        const int SE_CODE = 9;
        const int SE_FUTAN = 13;
        const int SE_JOCKEY = 16;
        const int SE_MINARA = 17;

        /* 定数定義 */
        const int MAX_TOSU = 19;

        /* 表切り替えフラグ */
        int OddsOnFlag = 0;
        int MainingOnFlag = 0;

        /* レース開催中止（雪や台風)フラグ */
        Boolean RaceHapning = false;

        public Syutsuba()
        {
            InitializeComponent();

        }

        public Syutsuba(String RA, int Color)
        {
            InitializeComponent();
            String tmp = "";
            List<String> Radata = new List<string>();

            db = new dbConnect();
            //DBからレース名を検索
            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 5, ref tmp);
            DataClass.SET_RA_KEY(RA);
            DataClass.setRaceDate(RA.Substring(0, 8));
            DataClass.setRaceCource(RA.Substring(8, 2));
            DataClass.setRaceKaiji(RA.Substring(10, 2));
            DataClass.setRaceNichiji(RA.Substring(12, 2));
            DataClass.setRaceNum(RA.Substring(14,2));
            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 7, ref tmp);
            DataClass.setRaceName(tmp);
            /* グレード */
            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 16, ref tmp);
            if (!(tmp == "")){ DataClass.setRaceGrade(tmp); }

            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 17, ref tmp);
            DataClass.setCourceTrack(tmp);
            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 18, ref tmp);
            DataClass.setDistance(tmp);
            
            db.TextReader_Col(RA.Substring(0, 8), "RA", 0, ref Radata, RA);
            RaClassData.setData(ref Radata);

            CourceColor = Color;

            int ret = main.JvGetRTData(RaClassData.getRaceDate()); //速報データ取得
            SetCourceStatusWrite();

            /* レース情報表示 */
            InitForm();
            
            String refBuff = "";
            /* データ情報を取得し、中止情報読み込み #41 */
            ret = RaClassData.RaGetRTRaData(DataClass.getRaceDate() + DataClass.getRaceCource() + DataClass.getRaceNum(), ref refBuff);

            if (ret != 0)
            {
                switch (refBuff)
                {
                    case "9":
                        Console.WriteLine("RAKEY = " + DataClass.GET_RA_KEY() + " refBuff = " + refBuff);
                        MessageBox.Show("このレースは「中止」となりました。\n詳細はJRAホームページで確認してください。", "レース中止情報");
                        this.racename.Text = "【中止】" + this.racename.Text;
                        RaceHapning = true;

                        break;
                }
            }


            /* 競走馬データ */
            InitHorceData();
        }

        public void LogOutPutFormThread(object Max)
        {
            Log LogForm = new Log(Int32.Parse(Max.ToString()));
            LogForm.Show();

            int ret = 0;

            LogForm.InitLogData(ProgressStatus);

            while (true)
            {
                ret = LogForm.LogCntUp(ProgressStatus);
                Thread.Sleep(500); //0.5秒待機

                if(ret != 0) //0は続行。それ以外は終了かエラー
                {
                    break;
                }
            }
        }
        
        /************** private **************/
        unsafe private void SetCourceStatusWrite()
        {
            int LibCode;
            int GetKind;
            int ret;
            String tmp = "";
            List<String> str = new List<string>();
            String Track = RaClassData.getCourceTrack();
            LibCode = LibJvConvFuncClass.TRACK_CODE_SHORT;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, Track, ref tmp);
            String Key = RaClassData.getRaceDate() + RaClassData.getRaceCource() + RaClassData.getRaceKaiji() + RaClassData.getRaceNichiji();

            if (tmp == "")
            {
                return;
            }

            GetKind = (tmp == "芝" ? 2 : 3);

            //競走馬情報

            tmp = "";
            ret = db.TextReader_Col(RaClassData.getRaceDate(), "WE", 0, ref str, Key);
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

                HappyoTime.Text = RaClassData.ConvertToHappyoTime(str[4]);

                if (str[4] == "00000000")
                {
                    HappyoTime.Text = "発売前時点";
                }
                else
                {
                    HappyoTime.Text = RaClassData.ConvertToHappyoTime(str[4]);
                }


                LibCode = 2010;
                LibJvConvFuncClass.jvSysConvFunction(&LibCode, str[GetKind], ref tmp);
                TrackDistance.Text = tmp;

            }


            TrackLabel.Text = (GetKind == 2 ? "芝" : "ダート");
            TrackLabel.BackColor = (GetKind == 2 ? Color.LightGreen : Color.Tan);

            LibCode = 2007;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, RaClassData.getRaceClass(), ref tmp);
            ClassLabel.Text = tmp;

            LibCode = 2006;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, RaClassData.getRaceKindKigo(), ref tmp);
            KigoLabel.Text = tmp;

            DistanceLabel.Text = RaClassData.getDistance();

            LibCode = LibJvConvFuncClass.TRACK_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, RaClassData.getCourceTrack(), ref tmp);
            TrackNameLabel.Text = tmp;



        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /* 競走馬データ */
        unsafe private void InitHorceData()
        {
            /* 他クラス共有用のクラスデータの初期化 */
            horceClasses = new List<Class.MainDataHorceClass>();

            /* 自クラス用クラスデータの定義 */
            Class.MainDataHorceClass pHorceClasses;

            /* ライブラリ用呼び出し指定変数 */
            int CODE;

            /* スレッド起動 */
            Thread t = new Thread(new ParameterizedThreadStart(LogOutPutFormThread));
            t.SetApartmentState(ApartmentState.STA);
            main.LogMainCancelFlagChanger(true);        //スレッド開始処理
            String SE_KEY = DataClass.GET_RA_KEY();
            String tmp = "";
            List<String> Arraytmp;

            //RA初回読み込み時にエラーチェック ０もエラー
            if (db.TextReader_aCell("RA", SE_KEY, SE_KEY.Substring(0, 8), 19, ref tmp) == 0)
            {
                main.LogMainCancelFlagChanger(false);        //スレッド開始処理
                return;
            }

            t.Start(tmp);

            for (int i = 1; ;i++)
            {
                pHorceClasses = new Class.MainDataHorceClass();
                String covData = String.Format("{0:00}", i);
                Arraytmp = new List<string>();


                //SE初回読み込み時にエラーチェック ０もエラー
                if (db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), 0, ref tmp) == 0)
                {
                    break;
                }

                if (tmp == "0"|| tmp == "") { break; }
                else if (tmp.Substring(0,16) != SE_KEY) { break; }
                pHorceClasses.KEY1 = tmp;

                if(db.TextReader_Col(SE_KEY.Substring(0, 8), "SE", 0, ref Arraytmp, SE_KEY + covData)== 0)
                {
                    break;
                }

                pHorceClasses.Waku1 = Arraytmp[SE_WAKU];
                pHorceClasses.Umaban1 = Arraytmp[SE_UMA];
                pHorceClasses.KettoNum1 = Int32.Parse(Arraytmp[SE_KETTO]);
                pHorceClasses.Name1 = Arraytmp[SE_NAME];
                pHorceClasses.Futan1 = Arraytmp[SE_FUTAN].Substring(0,2) + "." + Arraytmp[SE_FUTAN].Substring(2,1);
                pHorceClasses.Jockey1 = Arraytmp[SE_JOCKEY];

                CODE = LibJvConv.LibJvConvFuncClass.HOUCE_KIND;
                LibJvConvFuncClass.jvSysConvFunction(&CODE, Arraytmp[SE_CODE], ref tmp);
                pHorceClasses.UmaKigou1 = tmp;

                CODE = LibJvConv.LibJvConvFuncClass.JOCKEY_MINARAI_CD;
                LibJvConvFuncClass.jvSysConvFunction(&CODE, Arraytmp[SE_MINARA], ref tmp);
                pHorceClasses.MinaraiCd1 = tmp;

                /**
                                db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_WAKU, ref tmp);
                                db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_UMA, ref tmp);
                                db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_KETTO, ref tmp);
                                db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_NAME, ref tmp);
                                db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_FUTAN, ref tmp);
                                db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_JOCKEY, ref tmp);
*/

                Arraytmp = new List<string>();

                /* 血統登録番号からマスタを取得 */
                if(db.TextReader_Col("0", "UM", 0, ref Arraytmp, pHorceClasses.KettoNum1.ToString())== 0)
                {
                    
                    break;
                }

                pHorceClasses.F1 = Arraytmp[6];
                pHorceClasses.M1 = Arraytmp[7];
                pHorceClasses.FM1 = Arraytmp[9];
                pHorceClasses.FFM1 = Arraytmp[10];
                pHorceClasses.F_NUM1 = Arraytmp[15];
                pHorceClasses.FM_NUM1 = Arraytmp[16];
                pHorceClasses.FFM_NUM1 = Arraytmp[17];
                pHorceClasses.FF_NUM1 = Arraytmp[18]; //父父
                pHorceClasses.FFF_NUM1 = Arraytmp[19]; //父父父
                pHorceClasses.FMM_NUM1 = Arraytmp[20]; //母父父

/** 
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 15, ref tmp);
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 7, ref tmp);
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 9, ref tmp);
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 16, ref tmp);
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 10, ref tmp);
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 17, ref tmp);
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 18, ref tmp);
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 19, ref tmp);
                db.TextReader_aCell("UM", pHorceClasses.KettoNum1.ToString(), "0", 20, ref tmp);
*/


                /* 他クラス共有用のクラスに書き込み */
                horceClasses.Add(pHorceClasses);

                /* 書き込み */
                dataGridView1.Rows.Add(pHorceClasses.Waku1, pHorceClasses.Umaban1, pHorceClasses.UmaKigou1 + pHorceClasses.Name1, "", "", "", "", "", "", pHorceClasses.MinaraiCd1,
                    pHorceClasses.Jockey1, pHorceClasses.Futan1 + "kg", "",pHorceClasses.F1, "", pHorceClasses.FM1, "", pHorceClasses.FFM1);
                
                switch(pHorceClasses.Waku1)
                {
                    case "1":
                        dataGridView1[0, i-1].Style.ForeColor = Color.Black;
                        dataGridView1[0, i - 1].Style.BackColor = Color.White;
                        break;
                    case "2":
                        dataGridView1[0, i-1].Style.ForeColor = Color.White;
                        dataGridView1[0, i-1].Style.BackColor = Color.Black;
                        break;            
                    case "3":             
                        dataGridView1[0, i-1].Style.ForeColor = Color.White;
                        dataGridView1[0, i-1].Style.BackColor = Color.Red;
                        break;            
                    case "4":             
                        dataGridView1[0, i-1].Style.ForeColor = Color.White;
                        dataGridView1[0, i-1].Style.BackColor = Color.Blue;
                        break;           
                    case "5":            
                        dataGridView1[0, i-1].Style.ForeColor = Color.Black;
                        dataGridView1[0, i-1].Style.BackColor = Color.Yellow;
                        break;            
                    case "6":              
                        dataGridView1[0, i-1].Style.ForeColor = Color.White;
                        dataGridView1[0, i-1].Style.BackColor = Color.Green;
                        break;            
                    case "7":              
                        dataGridView1[0, i-1].Style.ForeColor = Color.Black;
                        dataGridView1[0, i-1].Style.BackColor = Color.Orange;
                        break;            
                    case "8":              
                        dataGridView1[0, i-1].Style.ForeColor = Color.Black;
                        dataGridView1[0, i-1].Style.BackColor = Color.Pink;
                        break;

                }
                dataGridView1[12, i - 1].Style.BackColor =  dbCom.DbComSearchBloodColor(pHorceClasses.F_NUM1, pHorceClasses.FF_NUM1, pHorceClasses.FFF_NUM1);
                dataGridView1[14, i - 1].Style.BackColor = dbCom.DbComSearchBloodColor(pHorceClasses.FM_NUM1, pHorceClasses.FMM_NUM1);
                dataGridView1[16, i - 1].Style.BackColor = dbCom.DbComSearchBloodColor(pHorceClasses.FFM_NUM1);

                /* プログレスバー更新 */
                ProgressStatus++;

            }
                        
            main.LogMainCancelFlagChanger(false);        //スレッド開始処理
           // t.Join();
            t.Abort();
            t.Join();
        }

        /*  DataGridView1[0, 0].Style.BackColor =*/

        unsafe private void Form2_Load(object sender, EventArgs e)
        {
         


            switch (CourceColor)
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
            
            /* #41対応 */
            if(RaceHapning)
            {
                panel1.BackColor = Color.Red;
            }

            /* フォントの変更 */
            dataGridView1.DefaultCellStyle.Font = new Font("Meiryo UI", 12);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9);

            /* タイトル表示 */
            int Code = LibJvConvFuncClass.COURCE_CODE;
            String tmp = "";
            LibJvConvFuncClass.jvSysConvFunction(&Code, DataClass.getRaceCource(), ref tmp);
            this.Text = "【出馬表】" + tmp + DataClass.getRaceNum() + "R：" + DataClass.getRaceName();
        }

        unsafe private void InitForm()
        {
            String LibTmp = "";
            int CODE = LibJvConvFuncClass.COURCE_CODE;

            String tmp = RaClassData.getRaceCource();
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            Cource = LibTmp;

            /* DataGridViewの高さの変更を出来ないようにする */
            dataGridView1.AllowUserToResizeRows = false;


            CODE = 2002;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getWeekDay() , ref LibTmp);

            //仕様変更#27：レースヘッダ共通化対応
            this.Date.Text = RaClassData.ConvertDateToDate(RaClassData.getRaceDate()) + "(" + (LibTmp == "祝" ? LibTmp : LibTmp + "曜") + ")";       //日付
            this.Kaisai.Text = "第" + Int32.Parse(RaClassData.getRaceKaiji()) + "回" + Cource + Int32.Parse(RaClassData.getRaceNichiji()) + "日目"; //開催
            this.label4.Text = RaClassData.ConvertTimeToString(RaClassData.getRaceStartTime());

            this.RaceNum.Text = Int32.Parse(RaClassData.getRaceNum()) + "Ｒ";
            this.kaiji.Text = (RaClassData.getRaceGradeKai() == 0 ? "" : "第" + RaClassData.getRaceGradeKai() + "回");
            this.racename.Text = RaClassData.getRaceNameFukus() + (RaClassData.getRaceNameFukus().Length >= 1 ? " " : "") + RaClassData.getRaceName() + (RaClassData.getRaceNameEnd() == ""? "": "(" + RaClassData.getRaceNameEnd() + ")");
            this.raceNameEng.Text = " "+ RaClassData.getRaceNameEng();

            if(RaClassData.getRaceGradeKai() != 0)
            {
                racename.Text += " （" + RaClassData.getRaceGrade() + "）";
            }
            
            CODE = LibJvConvFuncClass.RACE_SHUBETSU_LONG_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getOldYear(), ref LibTmp);
            this.OldYear.Text = LibTmp;

            CODE = LibJvConvFuncClass.TRACK_CODE_SHORT;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getCourceTrack(), ref LibTmp);
            int GetKind = (LibTmp == "芝" ? 2 : 3);
            TrackLabel.Text = (GetKind == 2 ? "芝" : "ダート");
            TrackLabel.BackColor = (GetKind == 2 ? Color.LightGreen : Color.Tan);

            CODE = 2007;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getRaceClass() , ref LibTmp);
            ClassLabel.Text = LibTmp;

            CODE = 2006;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getRaceKindKigo(), ref LibTmp);
            KigoLabel.Text = LibTmp;

            DistanceLabel.Text = RaClassData.getDistance();

            CODE = LibJvConvFuncClass.TRACK_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getCourceTrack(), ref LibTmp);
            TrackNameLabel.Text = "（" + LibTmp + "）";
        }

        unsafe private String MappingGetRaceCource()
        {
            String LibTmp = "";
            int CODE = LibJvConvFuncClass.COURCE_CODE;

            String tmp = DataClass.getRaceCource();
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            Cource = LibTmp;

            CODE = 2009;
            tmp = DataClass.getCourceTrack();
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            DataClass.setCourceTrack(LibTmp);


            return (Cource);
        }

        private void LabelCource_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        

        private void LabelRaceName_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        unsafe private void button2_Click(object sender, EventArgs e)
        {
            MainWindow main = new MainWindow();
            DataGridViewColumn column = new DataGridViewColumn();
            String Date = DataClass.getRaceDate() + DataClass.getRaceCource() + DataClass.getRaceNum();
            List<String> tmp = new List<string>();
            List<int> TimeDMArray = new List<int>();//タイム型降順にソートするためのList型配列
            List<int> BattleDMArray = new List<int>();
            String str = "";
            String covData;
            int ret = 0;
            db = new dbConnect();

            ret = main.InitRealTimeDataMaining(Date);
            ret += main.InitRealBattleDataMaining(Date, DataClass.GET_RA_KEY());

            int MaxTimeDM = 0;
            int MaxBattleDM = 99999;
            int SecondTimeDM = 0;
            int SecondBattleDM = 99999;
            int ThaadTimeDM = 0;
            int ThaadBattleDM = 99999;

            int Count = 0;

            int DataMainigNum = 0;

            Date = DataClass.getRaceDate();

            dataGridView1.Columns["TM"].Visible = true;
            dataGridView1.Columns["DM"].Visible = true;
            dataGridView1.Columns["VMRank"].Visible = true;
            dataGridView1.Columns["TMRank"].Visible = true;

            for (int i = 1; i <= MAX_TOSU; i++)
            {
                covData = String.Format("{0:00}", i);
                DataMainigNum = 0;
                tmp.Clear();

                /* 対戦型データマイニング */
                if (db.TextReader_aCell("TM", DataClass.GET_RA_KEY() + covData, DataClass.GET_RA_KEY(), 4, ref str) == 0)
                {
                    Count = i;
                    break;
                }

                dataGridView1.Rows[i - 1].Cells[3].Value = Int32.Parse(str);
                BattleDMArray.Add(Int32.Parse(str));
                DataMainigNum = Int32.Parse(str);

                if (MaxTimeDM < DataMainigNum)
                {
                    ThaadTimeDM = SecondTimeDM;
                    SecondTimeDM = MaxTimeDM;
                    MaxTimeDM = DataMainigNum;
                }
                else if (SecondTimeDM < DataMainigNum)
                {
                    ThaadTimeDM = SecondTimeDM;
                    SecondTimeDM = DataMainigNum;
                }
                else if (ThaadTimeDM < DataMainigNum)
                {
                    ThaadTimeDM = DataMainigNum;
                }

                DataMainigNum = 0;
                tmp.Clear();

                /* タイム型データマイニング */
                if (db.TextReader_aCell("DM", DataClass.GET_RA_KEY() + covData, DataClass.GET_RA_KEY(), 4, ref str) == 0)
                {
                    break;
                }


                if (db.TextReader_Col(DataClass.GET_RA_KEY(), "DM", 0, ref tmp, DataClass.GET_RA_KEY() + covData) == 0)
                {

                }

                try
                {
                    DataMainigNum = Int32.Parse(tmp[4]) - Int32.Parse(tmp[5]) + Int32.Parse(tmp[6]);
                    dataGridView1.Rows[i - 1].Cells[5].Value = DataMainigNum;
                    TimeDMArray.Add(DataMainigNum);

                }
                catch (Exception)
                {

                }


                //dataGridView1.Rows[i - 1].Cells[5].Value = DataMainigNum.ToString();
                //TimeDMArray.Add(DataMainigNum);

                if (MaxBattleDM > DataMainigNum)
                {
                    ThaadBattleDM = SecondBattleDM;
                    SecondBattleDM = MaxBattleDM;
                    MaxBattleDM = DataMainigNum;
                }
                else if (SecondBattleDM > DataMainigNum)
                {
                    ThaadBattleDM = SecondBattleDM;
                    SecondBattleDM = DataMainigNum;
                }
                else if (ThaadBattleDM > DataMainigNum)
                {
                    ThaadBattleDM = DataMainigNum;
                }
            }

           // int[] ArrayRankTM = new int[Count];
            BattleDMArray.Sort((a, b) => b-a);
            TimeDMArray.Sort((a, b) => a - b);


            if(OddsOnFlag != 0)
            {

            }

            //TimeDMArray.AddRange(ArrayRankTM);
            //tmpArray1.Sort((a, b) => b - a);

            /* 表の色付け */
            for (int j =1; j < Count; j++)
            {
                if (MaxTimeDM == (int)dataGridView1.Rows[j - 1].Cells[3].Value)
                {
                    dataGridView1[3, j - 1].Style.BackColor = Color.Pink;
                    dataGridView1[4, j - 1].Style.BackColor = Color.Pink;

                }
                else if(SecondTimeDM == (int)dataGridView1.Rows[j - 1].Cells[3].Value)
                {
                    dataGridView1[3, j - 1].Style.BackColor = Color.PowderBlue;
                    dataGridView1[4, j - 1].Style.BackColor = Color.PowderBlue;
                }
                else if (ThaadTimeDM == (int)dataGridView1.Rows[j - 1].Cells[3].Value)
                {
                    dataGridView1[3, j - 1].Style.BackColor = Color.LightCyan;
                    dataGridView1[4, j - 1].Style.BackColor = Color.LightCyan;
                }

                if (MaxBattleDM == Int32.Parse(dataGridView1.Rows[j - 1].Cells[5].Value.ToString()))
                {
                    dataGridView1[5, j - 1].Style.BackColor = Color.Pink;
                    dataGridView1[6, j - 1].Style.BackColor = Color.Pink;

                }
                else if (SecondBattleDM == Int32.Parse(dataGridView1.Rows[j - 1].Cells[5].Value.ToString()))
                {
                    dataGridView1[5, j - 1].Style.BackColor = Color.PowderBlue;
                    dataGridView1[6, j - 1].Style.BackColor = Color.PowderBlue;
                }
                else if (ThaadBattleDM == Int32.Parse(dataGridView1.Rows[j - 1].Cells[5].Value.ToString()))
                {
                    dataGridView1[5, j - 1].Style.BackColor = Color.LightCyan;
                    dataGridView1[6, j - 1].Style.BackColor = Color.LightCyan;
                }
            }

            int Rank = 0;

            /* 順位付け */
            foreach(var i in BattleDMArray)
            {
                if(i.ToString() == "")
                {
                    continue;
                }

                Rank++;
                for(int k = 1; k < Count; k++)
                {
                    if(dataGridView1.Rows[k - 1].Cells[3].Value.ToString() == i.ToString())
                    {
                        dataGridView1.Rows[k - 1].Cells[4].Value = Rank;
                    }
                }
            }

            Rank = 0;
            
            foreach(var i in TimeDMArray)
            {
                if (i.ToString() == "")
                {
                    continue;
                }

                Rank++;
                for (int k = 1; k < Count; k++)
                {
                    if (dataGridView1.Rows[k - 1].Cells[5].Value.ToString() == i.ToString())
                    {
                        dataGridView1.Rows[k - 1].Cells[6].Value = Rank;
                    }
                }
            }



            label1.Visible = true;
            DMStatus.Visible = true;
            DMStatus.Text = GetDMStatus();
            MainingOnFlag = 4;



        }

        #region データマイニング区分読み込み
        unsafe private String GetDMStatus()
        {
            /* データ区分書き込み */
            String Libtmp = "", str = "";
            int CODE = LibJvConvFuncClass.DATA_MINING_STATUS;
            db.TextReader_aCell("DM", DataClass.GET_RA_KEY() + "01", DataClass.GET_RA_KEY(), 2, ref str);
            LibJvConvFuncClass.jvSysConvFunction(&CODE, str, ref Libtmp);
            return Libtmp;
        }

        #endregion

        private void button3_Click(object sender, EventArgs e)
        {
            Class.GetOddsComClass getOdds = new Class.GetOddsComClass();
            List<JvComDbData.JvDbO1Data> ArrayO1 = new List<JvComDbData.JvDbO1Data>();
            List<String> O1 = new List<string>();
            int tmpOddz = 0;
            int tmpOddzRank = 0;

            //オッズ取得
            int ret = getOdds.GetOddsCom("0B30", RaClassData.getRaceDate() + RaClassData.getRaceCource()+ RaClassData.getRaceKaiji() + RaClassData.getRaceNichiji() + RaClassData.getRaceNum());

            if(ret != 1)
            {
                Console.WriteLine("Syutsuba\t GetOddsCom return " + ret);
                label6.Visible = true;
                OddzTime.Visible = true;
                
                if(ret == -1)
                {
                    OddzTime.Text = "発売前";
                }
                else
                {
                    OddzTime.Text = "";
                }

                return;
            }

            dataGridView1.Columns["Odds"].Visible = true;
            dataGridView1.Columns["OddsRank"].Visible = true;

            for (int i = 1; i<MAX_TOSU; i++)
            {
                O1.Clear();
                if (db.TextReader_Col(RaClassData.GET_RA_KEY(), "O1", 0, ref O1, RaClassData.GET_RA_KEY() + string.Format("{0:00}", i)) != 0)
                {
                    if(O1.Count() == 0)
                    {
                        break;
                    }

                    if(Int32.TryParse(O1[2], out tmpOddz))
                    {
                        dataGridView1.Rows[i - 1].Cells[7].Value = tmpOddz.ToString().Substring(0, tmpOddz.ToString().Length - 1) + "." + tmpOddz.ToString().Substring(tmpOddz.ToString().Length - 1, 1);
                    }
                    else
                    {
                        dataGridView1.Rows[i - 1].Cells[7].Value = O1[2];
                    }
                    
                    
                    if(Int32.TryParse(O1[3],out tmpOddzRank))
                    {
                        dataGridView1.Rows[i - 1].Cells[8].Value = tmpOddzRank;

                        //人気によって色付け
                        if(tmpOddzRank == 1)
                        {
                            dataGridView1[7, i-1].Style.BackColor = Color.Pink;
                            dataGridView1[8, i-1].Style.BackColor = Color.Pink;
                        }
                        else if(tmpOddzRank == 2)
                        {
                            dataGridView1[7, i - 1].Style.BackColor = Color.PowderBlue;
                            dataGridView1[8, i - 1].Style.BackColor = Color.PowderBlue;
                        }
                        else if(tmpOddzRank == 3)
                        {
                            dataGridView1[7, i - 1].Style.BackColor = Color.LightCyan;
                            dataGridView1[8, i - 1].Style.BackColor = Color.LightCyan;
                        }
                        else
                        {
                            dataGridView1[7, i - 1].Style.BackColor = Color.White;
                            dataGridView1[8, i - 1].Style.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        dataGridView1.Rows[i - 1].Cells[8].Value = O1[3];
                    }

                   
                }
                else
                {
                    break;
                }
            }

            String Date = "";
            if(db.TextReader_aCell("O1", RaClassData.GET_RA_KEY(), RaClassData.GET_RA_KEY(),8, ref Date) != 0)
            {
                label6.Visible = true;
                OddzTime.Visible = true;

                switch(Date)
                {
                    case "1":
                        db.TextReader_aCell("O1", RaClassData.GET_RA_KEY(), RaClassData.GET_RA_KEY(), 1, ref Date);
                        Date = RaClassData.ConvertToHappyoTime(Date);
                        break;
                    case "2":
                        Date = "前日最終";
                        break;
                    case "3":
                        Date = "最終オッズ";
                        break;
                    case "4":
                    case "5":
                        Date = "確定";
                        break;
                    case "9":
                        Date = "レース中止";
                        break;
                }
                OddzTime.Text = Date;
            }

            Console.WriteLine(ret);
        }
    }


}
