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
using WpfApp1.lib.site;
using WpfApp1.JvComDbData;
using AngleSharp.Html.Dom;
using System.Net.Http;
using AngleSharp.Html.Parser;
using WpfApp1.form.chokyo;
using LibRaceAnalyze;

namespace WpfApp1.form
{
    public partial class Syutsuba : Form
    {
        /* このクラスのログ定義 */
        WpfApp1.Class.com.JvComClass LOG = new Class.com.JvComClass();
        const String SPEC = "SYB";

        /* DB書き込みクラス */
        dbConnect db;
        dbCom dbCom = new dbCom();
      //  Class.MainRaClassData RaClassData = new Class.MainRaClassData();
        JvComDbData.JvDbRaData RaClassData = new JvComDbData.JvDbRaData();
        static String Cource;
        int CourceColor;
        MainWindow main = new MainWindow();
        JvFamilyNoData FnDataClass;

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
        const int SE_JOCKEY_CODE = 15;
        const int SE_JOCKEY = 16;
        const int SE_MINARA = 17;
        const int SE_TRANER_NAME = 31;
        const int SE_TRANER_CODE = 32;

        /* 定数定義 */
        const int MAX_TOSU = 19;

        /* 表切り替えフラグ */
        int OddsOnFlag = 0;
        int MainingOnFlag = 0;

        /* レース開催中止（雪や台風)フラグ */
        Boolean RaceHapning = false;

        /* 馬体重データ */
        Boolean BataijuFlg = false;

        /* 人気パラメーター */
        int RankParam = 0;

        /* データ分析用インスタンス */
        RaceAnalyzeMain Analyze;

        enum DT
        {
            DT_BUTTON = 0,
            DT_WAKU,
            DT_UMABAN,
            DT_BAMEI,
            DT_VSDM,
            DT_VSDM_RANK,
            DT_TMDM,
            DT_TMDM_RANK,
            DT_O1,
            DT_O1_RANK,
            DT_J_MINARAI,
            DT_J_NAME,
            DT_J_FUTAN,
            DT_H_TAIJU,
            //DT_H_KIGO,
            DT_H_ZOGEN,
            DT_F_COLOR,
            DT_F_NAME,
            DT_MF_COLOR,
            DT_MF_NAME,
            DT_MMF_COLOR,
            DT_MMF_NAME,
            DT_FNO_COLOR,
            DT_FNO_NAME,
        };

        public Syutsuba()
        {
            InitializeComponent();

        }

        public Syutsuba(String RA, int Clr, ref RaceAnalyzeMain Ana)
        {
            Analyze = Ana;
            InitializeComponent();
            Syutsuba_common(RA, Clr);
        }

        public Syutsuba(String RA, int Clr)
        {
            InitializeComponent();
            Syutsuba_common(RA, Clr);
        }

        void Syutsuba_common(String RA, int Clr)
        {
            String tmp = "";
            List<String> Radata = new List<string>();

            db = new dbConnect();
            //DBからレース名を検索
            // db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 5, ref tmp);
            this.Refresh();
            db.TextReader_Col(RA.Substring(0, 8), "RA", 0, ref Radata, RA);
            RaClassData.setData(ref Radata);

#if false
            RaClassData.SET_RA_KEY(RA);
            RaClassData.setRaceDate(RA.Substring(0, 8));
            RaClassData.setRaceCource(RA.Substring(8, 2));
            RaClassData.setRaceKaiji(RA.Substring(10, 2));
            RaClassData.setRaceNichiji(RA.Substring(12, 2));
            RaClassData.setRaceNum(RA.Substring(14,2));
            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 7, ref tmp);
            RaClassData.setRaceName(tmp);
            /* グレード */
            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 16, ref tmp);
            if (!(tmp == "")){ RaClassData.setRaceGrade(tmp); }

            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 17, ref tmp);
            RaClassData.setCourceTrack(tmp);
            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 18, ref tmp);
            RaClassData.setDistance(tmp);
            
#endif
            CourceColor = Clr;

            int ret = main.JvGetRTData(RaClassData.getRaceDate()); //速報データ取得
            SetCourceStatusWrite();

            String refBuff = "";
            /* データ情報を取得し、中止情報読み込み #41 */
            //ret = RaClassData.RaGetRTRaData(RaClassData.getRaceDate() + RaClassData.getRaceCource() + RaClassData.getRaceNum(), ref refBuff);
            ret = RaClassData.RaGetRTRaData(RaClassData.GET_RA_KEY() + RaClassData.getRaceCource() + RaClassData.getRaceNum(), ref refBuff);
            this.Text = "【出馬表】" + tmp + RaClassData.getRaceNum() + "R：" + RaClassData.getRaceName();

            if (ret != 0)
            {
                switch (refBuff)
                {
                    case "9":
                        Console.WriteLine("RAKEY = " + RaClassData.GET_RA_KEY() + " refBuff = " + refBuff);
                        MessageBox.Show("このレースは「中止」となりました。\n詳細はJRAホームページで確認してください。", "レース中止情報");
                        this.racename.Text = "【中止】" + this.racename.Text;
                        RaceHapning = true;

                        break;
                }
            }

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
            if (RaceHapning)
            {
                panel1.BackColor = Color.Red;
            }

#if false   //見た目高速化のため、Form1_Show()の処理に移動(レース情報だけ読み込み)
            /* レース情報表示 */
            InitForm();

            /* 競走馬データ */
            InitHorceData();

            /* 馬体重データ */
            SetbataijuData();
#endif
        }

        public void LogOutPutFormThread(object Max)
        {
            Log LogForm = new Log(Int32.Parse(Max.ToString()));
            //LogForm.Show();

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

            LibCode = 20071;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, RaClassData.getRaceClass(), ref tmp);
            ClassLabel.Text = tmp;

            //2019年6月～クラス名称変更対応
            if (RaClassData.getRaceClass() == "005" || RaClassData.getRaceClass() == "010" || RaClassData.getRaceClass() == "016")
            {
                LibCode = 20071;
                LibJvConvFuncClass.jvSysConvFunction(&LibCode, RaClassData.getRaceClass(), ref tmp);
                racename.Text += "（" + tmp + "）";
            }


            LibCode = 2006;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, RaClassData.getRaceKindKigo(), ref tmp);
            KigoLabel.Text = tmp;

            DistanceLabel.Text = RaClassData.getDistance();

            LibCode = LibJvConvFuncClass.TRACK_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&LibCode, RaClassData.getCourceTrack(), ref tmp);
            TrackNameLabel.Text = tmp;



        }

        private void SetbataijuData()
        {
#if SW_LOGTIME
            LOG.CONSOLE_TIME("<<<馬体重データ START！>>>");
#endif
            JvDbWhData JvWh = new JvDbWhData();
            JvWhData WhData = new JvWhData();
            List<JvWhData> ArrayWhData = new List<JvWhData>();
            String Libtmp = "";
            ArrayWhData = JvWh.JvWhGetAllData(RaClassData.GET_RA_KEY());
            if (ArrayWhData.Count != 0)
            {
                LOG.CONSOLE_MODULE(SPEC, "Find! JvWhData");

                dataGridView1.Columns["Bataiju"].Visible = true;
                dataGridView1.Columns["Zogen"].Visible = true;

                for (int i = 0; i < ArrayWhData.Count; i++)
                {
                    try
                    {
                        Libtmp = ArrayWhData[i].Zogensa1;
                        dataGridView1.Rows[i].Cells[(int)DT.DT_H_TAIJU].Value = ArrayWhData[i].Bataiju1;

                        dataGridView1.Rows[i].Cells[(int)DT.DT_H_ZOGEN].Value =
                            "(" + ArrayWhData[i].Fugo1 + LOG.JvSysMappingFunction(6002, ref Libtmp) + ")";
                    }
                    catch(Exception)
                    {

                    }
                }
                BataijuFlg = true;
            }
#if SW_LOGTIME
            LOG.CONSOLE_TIME("<<<馬体重データ END！>>>");
#endif
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        /* 競走馬データ */
        unsafe private void InitHorceData()
        {
#if SW_LOGTIME
            LOG.CONSOLE_TIME("<<<競走馬データ START！>>>");
            toolStripStatusLabel1.Text = "競走馬データ取得中";
#endif
            /* 他クラス共有用のクラスデータの初期化 */
            horceClasses = new List<Class.MainDataHorceClass>();

            /* 自クラス用クラスデータの定義 */
            Class.MainDataHorceClass pHorceClasses;

            /* ライブラリ用呼び出し指定変数 */
            int CODE;

            /* スレッド起動 */
#if !SW_THREAD
            Thread t = new Thread(new ParameterizedThreadStart(LogOutPutFormThread));
            t.SetApartmentState(ApartmentState.STA);
            main.LogMainCancelFlagChanger(true);        //スレッド開始処理
#else
            StartProgresBar();
#endif
            String SE_KEY = RaClassData.GET_RA_KEY();
            String tmp = "";
            List<String> Arraytmp;

            //RA初回読み込み時にエラーチェック ０もエラー
            if (db.TextReader_aCell("RA", SE_KEY, SE_KEY.Substring(0, 8), 19, ref tmp) == 0)
            {
                main.LogMainCancelFlagChanger(false);        //スレッド開始処理
                return;
            }

#if !SW_THREAD
            t.Start(tmp);
#endif

            String[] tmpSlData = new string[18];
            String SlData = ""; //for内で使う
            Boolean SlFlg = GetShirushiToDb(ref tmpSlData);

            //出馬表表示高速化対応
            List<DataGridViewRow> rowsArray = new List<DataGridViewRow>();
            List<String> UM_Master = new List<string>();
            bool fUmSetCompFlg = false;
            //UMマスタデータを取得しておく
            db.DbReadAllData("0", "UM", 0, ref UM_Master, SE_KEY, 0);

            for (int i = 1; ;i++)
            {
#if SW_LOGTIME
                LOG.CONSOLE_TIME(">>>>>出走馬：" + i);
#endif
                pHorceClasses = new Class.MainDataHorceClass();

                //出馬表高速化
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);

                String covData = String.Format("{0:00}", i);
                Arraytmp = new List<string>();


                //SE初回読み込み時にエラーチェック ０もエラー
#if false
                if (db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), 0, ref tmp) == 0)
                {
                    break;
                }

                if (tmp == "0"|| tmp == "") { break; }
                else if (tmp.Substring(0,16) != SE_KEY) { break; }
                pHorceClasses.KEY1 = tmp;
#endif
                if(db.TextReader_Col(SE_KEY.Substring(0, 8), "SE", 0, ref Arraytmp, SE_KEY + covData)== 0)
                {
                    break;
                }

                if (Arraytmp.Count == 0) { break; }
                pHorceClasses.KEY1 = Arraytmp[0];
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
#if SW_DEVELOP
                fUmSetCompFlg = false;
                for (int j=0; j < UM_Master.Count && !fUmSetCompFlg; j++)
                {
                    //カンマ区切りにする
                    var param = UM_Master[j].Split(',');
                    if(param[0] == pHorceClasses.KettoNum1.ToString())
                    {
                    //String[]型とList<String>となっているため、ArrayTmpにいれるように変換する
                        for(int idx = 0; idx < param.Length; idx++)
                        {
                            Arraytmp.Add(param[idx]);

                        }
                        fUmSetCompFlg = true;
                        break;
                    }
                }
#else
                if(db.TextReader_Col("0", "UM", 0, ref Arraytmp, pHorceClasses.KettoNum1.ToString())== 0)
                {
                    
                    break;
                }
#endif
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

                /* 印が設定済みなら読み込む */
                if(SlFlg)
                {
                    SlData = tmpSlData[i-1];
                }


                /* 書き込み */
#if SW_DEVELOP
                row.SetValues(new object[] { SlData, pHorceClasses.Waku1, pHorceClasses.Umaban1, pHorceClasses.UmaKigou1 + pHorceClasses.Name1, "", "", "", "", "", "", pHorceClasses.MinaraiCd1,
                    pHorceClasses.Jockey1, pHorceClasses.Futan1 + "kg", "", "", "",pHorceClasses.F1, "", pHorceClasses.FM1, "", pHorceClasses.FFM1, "", "" });
#else
                dataGridView1.Rows.Add(SlData, pHorceClasses.Waku1, pHorceClasses.Umaban1, pHorceClasses.UmaKigou1 + pHorceClasses.Name1, "", "", "", "", "", "", pHorceClasses.MinaraiCd1,
                    pHorceClasses.Jockey1, pHorceClasses.Futan1 + "kg", "", "", "",pHorceClasses.F1, "", pHorceClasses.FM1, "", pHorceClasses.FFM1);
#endif
                switch(pHorceClasses.Waku1)
                {
                    case "1":
#if SW_DEVELOP
                        row.Cells[(int)DT.DT_WAKU].Style.BackColor = Color.White;
                        row.Cells[(int)DT.DT_WAKU].Style.ForeColor = Color.Black;
#else
                        dataGridView1[(int)DT.DT_WAKU, i-1].Style.ForeColor = Color.Black;
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.BackColor = Color.White;
#endif
                        break;
                    case "2":
#if SW_DEVELOP
                        row.Cells[(int)DT.DT_WAKU].Style.BackColor = Color.Black;
                        row.Cells[(int)DT.DT_WAKU].Style.ForeColor = Color.White;
#else
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.ForeColor = Color.Black;
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.BackColor = Color.White;
#endif
                        break;            
                    case "3":
#if SW_DEVELOP
                        row.Cells[(int)DT.DT_WAKU].Style.BackColor = Color.Red;
                        row.Cells[(int)DT.DT_WAKU].Style.ForeColor = Color.White;
#else
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.ForeColor = Color.White;
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.BackColor = Color.Red;
#endif
                        break;            
                    case "4":
#if SW_DEVELOP
                        row.Cells[(int)DT.DT_WAKU].Style.BackColor = Color.Blue;
                        row.Cells[(int)DT.DT_WAKU].Style.ForeColor = Color.White;
#else
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.ForeColor = Color.White;
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.BackColor = Color.Blue;
#endif
                        break;           
                    case "5":
#if SW_DEVELOP
                        row.Cells[(int)DT.DT_WAKU].Style.BackColor = Color.Yellow;
                        row.Cells[(int)DT.DT_WAKU].Style.ForeColor = Color.Black;
#else
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.ForeColor = Color.Black;
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.BackColor = Color.Yellow;
#endif
                        break;            
                    case "6":
#if SW_DEVELOP
                        row.Cells[(int)DT.DT_WAKU].Style.BackColor = Color.Green;
                        row.Cells[(int)DT.DT_WAKU].Style.ForeColor = Color.White;
#else
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.ForeColor = Color.White;
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.BackColor = Color.Green;
#endif
                        break;            
                    case "7":
#if SW_DEVELOP
                        row.Cells[(int)DT.DT_WAKU].Style.BackColor = Color.Orange;
                        row.Cells[(int)DT.DT_WAKU].Style.ForeColor = Color.Black;
#else
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.ForeColor = Color.Black;
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.BackColor = Color.Orange;
#endif
                        break;            
                    case "8":
#if SW_DEVELOP
                        row.Cells[(int)DT.DT_WAKU].Style.BackColor = Color.Pink;
                        row.Cells[(int)DT.DT_WAKU].Style.ForeColor = Color.Black;
#else
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.ForeColor = Color.Black;
                        dataGridView1[(int)DT.DT_WAKU, i - 1].Style.BackColor = Color.Pink;
#endif
                        break;

                }
#if SW_DEVELOP
                row.Cells[(int)DT.DT_F_COLOR].Style.BackColor = dbCom.DbComSearchBloodColor(pHorceClasses.F_NUM1, pHorceClasses.FF_NUM1, pHorceClasses.FFF_NUM1);
                row.Cells[(int)DT.DT_MF_COLOR].Style.BackColor = dbCom.DbComSearchBloodColor(pHorceClasses.FM_NUM1, pHorceClasses.FMM_NUM1);
                row.Cells[(int)DT.DT_MMF_COLOR].Style.BackColor = dbCom.DbComSearchBloodColor(pHorceClasses.FFM_NUM1);
                //DataGrid配列に保存(後にまとめて書き込み)
                rowsArray.Add(row);
#else
                dataGridView1[(int)DT.DT_F_COLOR, i - 1].Style.BackColor =  dbCom.DbComSearchBloodColor(pHorceClasses.F_NUM1, pHorceClasses.FF_NUM1, pHorceClasses.FFF_NUM1);
                dataGridView1[(int)DT.DT_MF_COLOR, i - 1].Style.BackColor = dbCom.DbComSearchBloodColor(pHorceClasses.FM_NUM1, pHorceClasses.FMM_NUM1);
                dataGridView1[(int)DT.DT_MMF_COLOR, i - 1].Style.BackColor = dbCom.DbComSearchBloodColor(pHorceClasses.FFM_NUM1);
#endif
                /* プログレスバー更新 */
                ProgressStatus++;
                CountUpProgressBar(ProgressStatus);

            }

#if DEBUG
            LOG.CONSOLE_TIME("<<<競走馬 ループ終了>>>");
#endif

#if SW_DEVELOP
            //書き込み
            // DataGridViewに格納
            if (rowsArray.Count != 0)
                {
                    DataGridViewRow[] rows = new DataGridViewRow[rowsArray.Count];
                    //List型に持っているものを配列型に入れる
                    for (int i = 0; i < rowsArray.Count; i++) rows[i] = rowsArray[i];
                    //DatagridViewに書き込む
                    dataGridView1.Rows.AddRange(rows);
                }
#endif

            //騎手変更情報反映
            List<JvComDbData.JvDbJcData> AfterJockey = new List<JvComDbData.JvDbJcData>();
            int ArrayNum = 0;
            CODE = LibJvConv.LibJvConvFuncClass.JOCKEY_MINARAI_CD;

            if (dbCom.GetJockeyChangeInfo(RaClassData.GET_RA_KEY(), ref AfterJockey))
            {
                for(int j = 0; j < AfterJockey.Count; j++)
                {
                    LibJvConvFuncClass.jvSysConvFunction(&CODE, AfterJockey[j].AfterInfo1.MinaraiCd, ref tmp);
                    pHorceClasses.MinaraiCd1 = tmp;
                    ArrayNum = AfterJockey[j].Umaban1 - 1;
                    dataGridView1[(int)DT.DT_J_MINARAI, ArrayNum].Value = tmp;
                    dataGridView1[(int)DT.DT_J_NAME, ArrayNum].Value = AfterJockey[j].AfterInfo1.Name;
                    if(AfterJockey[j].AfterInfo1.Name != "未定")
                    {
                        dataGridView1[(int)DT.DT_J_FUTAN, ArrayNum].Value = AfterJockey[j].AfterInfo1.Futan.Substring(0, 2) + "." + AfterJockey[j].AfterInfo1.Futan.Substring(2, 1) + "kg";
                    }
                    else
                    {
                        //騎手未定の場合、負担重量欄を空にしておく
                        dataGridView1[(int)DT.DT_J_FUTAN, ArrayNum].Value = " ";
                    }

                    //フォント
                    dataGridView1[(int)DT.DT_J_MINARAI, ArrayNum].Style.ForeColor = Color.Red;
                    dataGridView1[(int)DT.DT_J_NAME, ArrayNum].Style.ForeColor = Color.Red;

                    if(AfterJockey[j].AfterInfo1.Futan != AfterJockey[j].BeforeInfo1.Futan)
                    {
                        //負担重量が変わった場合は、負担重量欄も赤字にする
                        dataGridView1[(int)DT.DT_J_FUTAN, ArrayNum].Style.ForeColor = Color.Red;
                    }
                }
            }                                                     

            //レコード情報
            SetRecordData(RaClassData.GET_RA_KEY());
            if(RaClassData.getRaceGrade() == "ＧⅠ" || RaClassData.getRaceGrade() == "Ｊ・ＧⅠ")
            {
                SetRaceRecordData(RaClassData.GET_RA_KEY());
            }

#if !SW_THREAD
            main.LogMainCancelFlagChanger(false);        //スレッド開始処理
            
            try {
                t.Abort(); 
                t.Join();
            }
            catch (Exception){  }
#endif
#if SW_LOGTIME
            LOG.CONSOLE_TIME("<<<競走馬データ START！>>>");
#endif
        }

        /*  DataGridView1[0, 0].Style.BackColor =*/

        unsafe private void Form2_Load(object sender, EventArgs e)
        {
            LOG.CONSOLE_TIME_MD("SY", "Syutsuba form Load Start");
            /* レース情報表示 */
            InitForm();

            /* 競走馬データ */
            InitHorceData();

            /* 馬体重データ */
            SetbataijuData();


            /* フォントの変更 */
            dataGridView1.DefaultCellStyle.Font = new Font("Meiryo UI", 12);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9);

            /* タイトル表示 */
            int Code = LibJvConvFuncClass.COURCE_CODE;
            String tmp = "";
            LibJvConvFuncClass.jvSysConvFunction(&Code, RaClassData.getRaceCource(), ref tmp);
            this.Text = "【出馬表】" + tmp + RaClassData.getRaceNum() + "R：" + RaClassData.getRaceName();

            //人気信頼ランク表示
            RankAverageOutput();
            toolStripStatusLabel1.Text = "準備完了";

            /* 分析ボタンの有効化 */
            button8.Enabled = false;
            if (Analyze != null)
            {
                //分析ボタンを有効にする
                LOG.CONSOLE_TIME_MD("SY", "Analyze Button Enable!!");
                button8.Enabled = true;
            }

            FinishProfresBar();
            LOG.CONSOLE_TIME_MD("SY", "Syutsuba form Load Finish!!");
        }

        unsafe private void InitForm()
        {
#if DEBUG
            LOG.CONSOLE_TIME("<<<レース情報取得 START！>>>");
#endif

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

            this.RaceNum.Text = Cource;
            this.rNum.Text = Int32.Parse(RaClassData.getRaceNum()) + "Ｒ";
            this.kaiji.Text = (RaClassData.getRaceGradeKai() == 0 ? "" : "第" + RaClassData.getRaceGradeKai() + "回");
            this.racename.Text = RaClassData.getRaceNameFukus() + (RaClassData.getRaceNameFukus().Length >= 1 ? " " : "") + RaClassData.getRaceName() + (RaClassData.getRaceNameEnd() == ""? "": "(" + RaClassData.getRaceNameEnd() + ")");
            this.raceNameEng.Text = " "+ RaClassData.getRaceNameEng();

            //2019年6月～クラス名称変更対応
            if (RaClassData.getRaceGrade() == "一般" &&
                (RaClassData.getRaceClass() == "005" || RaClassData.getRaceClass() == "010" || RaClassData.getRaceClass() == "016"))
            {
                CODE = 20071;
                LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getRaceClass(), ref LibTmp);
                racename.Text += "（" + LibTmp + "）";
            }

            if (RaClassData.getRaceGradeKai() != 0 || RaClassData.getRaceGrade() == "Ｌ") //リステッド競走対応
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
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getRaceClass(), ref LibTmp);
            ClassLabel.Text = LibTmp;


            CODE = 2006;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getRaceKindKigo(), ref LibTmp);
            KigoLabel.Text = LibTmp;

            CODE = 2008;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getRaceHandCap(), ref LibTmp);
            KigoLabel.Text += LibTmp;

            DistanceLabel.Text = RaClassData.getDistance();

            CODE = LibJvConvFuncClass.TRACK_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, RaClassData.getCourceTrack(), ref LibTmp);
            TrackNameLabel.Text = "（" + LibTmp + "）";

            //人気指数


#if DEBUG
            LOG.CONSOLE_TIME("<<<レース情報取得 END！>>>");
#endif
        }

        unsafe private String MappingGetRaceCource()
        {
            String LibTmp = "";
            int CODE = LibJvConvFuncClass.COURCE_CODE;

            String tmp = RaClassData.getRaceCource();
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            Cource = LibTmp;

            CODE = 2009;
            tmp = RaClassData.getCourceTrack();
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            RaClassData.setCourceTrack(LibTmp);


            return (Cource);
        }

        private void SetRankParam(int Count)
        {
            RankParam += Count;
        }

        private void RankAverageOutput()
        {
            int ret = 0;   //ランク保持

            if(RaClassData.GET_RA_KEY() == "")
            {
                //RAキーが未セット
                return;
            }

            //グレードのみここで適応
            switch (RaClassData.getRaceGrade())
            {
                case "ＧⅠ":
                    SetRankParam(1);
                    break;
                case "ＧⅡ":
                    SetRankParam(2);
                    break;
                case "ＧⅢ":
                case "重賞":  //格付けなし重賞はG3と同等にする
                    SetRankParam(5);
                    break;
                case "Ｌ":
                    SetRankParam(2);
                    break;
                default:
                    break;
            }

            //重賞以外はここで処理する(重賞は処理しない)
            switch (RaClassData.getRaceClass())
            {
                case "999": //オープン
                    //重賞もここに含まれるため、オープン特別のみ処理する
                    if(RaClassData.getRaceGrade() == "特別")
                    {
                        SetRankParam(2);
                    }
                    break;
                case "703": //未勝利
                case "701": //新馬
                case "005": //1勝クラス
                    SetRankParam(1);
                    break;
                case "010": //2勝クラス
                    SetRankParam(2);
                    break;
                case "016": //3勝クラス
                    SetRankParam(5);
                    break;
                default:
                    break;
            }

            //開催場
            switch (RaClassData.getRaceCource())
            {
                case "03": //福島
                case "10": //小倉
                    SetRankParam(5);
                    break;
                case "04": //新潟
                case "05": //東京
                case "06": //中山
                case "09": //阪神
                    SetRankParam(3);
                    break;
                case "08": //京都
                case "01": //札幌
                    SetRankParam(2);
                    break;
                case "02": //函館
                case "07": //中京
                    SetRankParam(1);
                    break;
            }

            //距離
            if(TrackLabel.Text == "ダート")
            {
                SetRankParam(4); //ダートは共通
            }
            else if(RaClassData.getOldYear() == "18" || RaClassData.getOldYear() == "19")
            {
                //障害
                SetRankParam(4);
            }
            else if((Int32.Parse(RaClassData.getDistance()) % 400) == 0 )
            {
                //芝・根幹
                SetRankParam(2);
            }
            else
            {
                //芝・非根幹
                SetRankParam(1);
            }
            Console.WriteLine(RankParam);

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

#if SW_THREAD
        private void StartProgresBar()
        {
            statProgressBar.Minimum = 0;    //固定値0を指定
            statProgressBar.Maximum = RaClassData.Tosu1;   //最大頭数を指定する。
            statProgressBar.Value = 0;      //現在値も0を指定する。
            statProgressBar.Visible = true;
        }
        
        private void CountUpProgressBar( int Cnt )
        {
            if(RaClassData.Tosu1 < Cnt)
            {
                return; //エラー
            }
            statProgressBar.Value = Cnt;
            this.Refresh();
        }

        //出馬表データ読み込み完了処理
        private void FinishProfresBar()
        {
            statProgressBar.Visible = false;
        }
#endif

        unsafe private void button2_Click(object sender, EventArgs e)
        {
            MainWindow main = new MainWindow();
            DataGridViewColumn column = new DataGridViewColumn();
            String Date = RaClassData.getRaceDate() + RaClassData.getRaceCource() + RaClassData.getRaceNum();
            List<String> tmp = new List<string>();
            List<int> TimeDMArray = new List<int>();//タイム型降順にソートするためのList型配列
            List<int> BattleDMArray = new List<int>();
            String str = "";
            String covData;
            int ret = 0;
            db = new dbConnect();

            ret = main.InitRealTimeDataMaining(Date);
            ret += main.InitRealBattleDataMaining(Date, RaClassData.GET_RA_KEY());

            int MaxTimeDM = 0;
            int MaxBattleDM = 99999;
            int SecondTimeDM = 0;
            int SecondBattleDM = 99999;
            int ThaadTimeDM = 0;
            int ThaadBattleDM = 99999;

            int Count = 0;

            int DataMainigNum = 0;

            Date = RaClassData.getRaceDate();

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
                if (db.TextReader_aCell("TM", RaClassData.GET_RA_KEY() + covData, RaClassData.GET_RA_KEY(), 4, ref str) == 0)
                {
                    Count = i;
                    break;
                }

                dataGridView1.Rows[i - 1].Cells[(int)DT.DT_VSDM].Value = Int32.Parse(str);
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
                if (db.TextReader_aCell("DM", RaClassData.GET_RA_KEY() + covData, RaClassData.GET_RA_KEY(), 4, ref str) == 0)
                {
                    break;
                }


                if (db.TextReader_Col(RaClassData.GET_RA_KEY(), "DM", 0, ref tmp, RaClassData.GET_RA_KEY() + covData) == 0)
                {

                }

                try
                {
                    DataMainigNum = Int32.Parse(tmp[4]) - Int32.Parse(tmp[5]) + Int32.Parse(tmp[6]);
                    dataGridView1.Rows[i - 1].Cells[(int)DT.DT_TMDM].Value = DataMainigNum;
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
                if (MaxTimeDM == (int)dataGridView1.Rows[j - 1].Cells[(int)DT.DT_VSDM].Value)
                {
                    dataGridView1[(int)DT.DT_VSDM, j - 1].Style.BackColor = Color.Pink;
                    dataGridView1[(int)DT.DT_VSDM_RANK, j - 1].Style.BackColor = Color.Pink;

                }
                else if(SecondTimeDM == (int)dataGridView1.Rows[j - 1].Cells[(int)DT.DT_VSDM].Value)
                {
                    dataGridView1[(int)DT.DT_VSDM, j - 1].Style.BackColor = Color.PowderBlue;
                    dataGridView1[(int)DT.DT_VSDM_RANK, j - 1].Style.BackColor = Color.PowderBlue;
                }
                else if (ThaadTimeDM == (int)dataGridView1.Rows[j - 1].Cells[(int)DT.DT_VSDM].Value)
                {
                    dataGridView1[(int)DT.DT_VSDM, j - 1].Style.BackColor = Color.LightCyan;
                    dataGridView1[(int)DT.DT_VSDM_RANK, j - 1].Style.BackColor = Color.LightCyan;
                }

                if (MaxBattleDM == Int32.Parse(dataGridView1.Rows[j - 1].Cells[(int)DT.DT_TMDM].Value.ToString()))
                {
                    dataGridView1[(int)DT.DT_TMDM, j - 1].Style.BackColor = Color.Pink;
                    dataGridView1[(int)DT.DT_TMDM_RANK, j - 1].Style.BackColor = Color.Pink;

                }
                else if (SecondBattleDM == Int32.Parse(dataGridView1.Rows[j - 1].Cells[(int)DT.DT_TMDM].Value.ToString()))
                {
                    dataGridView1[(int)DT.DT_TMDM, j - 1].Style.BackColor = Color.PowderBlue;
                    dataGridView1[(int)DT.DT_TMDM_RANK, j - 1].Style.BackColor = Color.PowderBlue;
                }
                else if (ThaadBattleDM == Int32.Parse(dataGridView1.Rows[j - 1].Cells[(int)DT.DT_TMDM].Value.ToString()))
                {
                    dataGridView1[(int)DT.DT_TMDM, j - 1].Style.BackColor = Color.LightCyan;
                    dataGridView1[(int)DT.DT_TMDM_RANK, j - 1].Style.BackColor = Color.LightCyan;
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
                    if(dataGridView1.Rows[k - 1].Cells[(int)DT.DT_VSDM].Value.ToString() == i.ToString())
                    {
                        dataGridView1.Rows[k - 1].Cells[(int)DT.DT_VSDM_RANK].Value = Rank;
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
                    if (dataGridView1.Rows[k - 1].Cells[(int)DT.DT_TMDM].Value.ToString() == i.ToString())
                    {
                        dataGridView1.Rows[k - 1].Cells[(int)DT.DT_TMDM_RANK].Value = Rank;
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
            db.TextReader_aCell("DM", RaClassData.GET_RA_KEY() + "01", RaClassData.GET_RA_KEY(), 2, ref str);
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
                        dataGridView1.Rows[i - 1].Cells[(int)DT.DT_O1].Value = tmpOddz.ToString().Substring(0, tmpOddz.ToString().Length - 1) + "." + tmpOddz.ToString().Substring(tmpOddz.ToString().Length - 1, 1);
                    }
                    else
                    {
                        dataGridView1.Rows[i - 1].Cells[(int)DT.DT_O1].Value = O1[2];
                    }
                    
                    
                    if(Int32.TryParse(O1[3],out tmpOddzRank))
                    {
                        dataGridView1.Rows[i - 1].Cells[(int)DT.DT_O1_RANK].Value = tmpOddzRank;

                        //人気によって色付け
                        if(tmpOddzRank == 1)
                        {
                            dataGridView1[(int)DT.DT_O1, i-1].Style.BackColor = Color.Pink;
                            dataGridView1[(int)DT.DT_O1_RANK, i-1].Style.BackColor = Color.Pink;
                        }
                        else if(tmpOddzRank == 2)
                        {
                            dataGridView1[(int)DT.DT_O1, i - 1].Style.BackColor = Color.PowderBlue;
                            dataGridView1[(int)DT.DT_O1_RANK, i - 1].Style.BackColor = Color.PowderBlue;
                        }
                        else if(tmpOddzRank == 3)
                        {
                            dataGridView1[(int)DT.DT_O1, i - 1].Style.BackColor = Color.LightCyan;
                            dataGridView1[(int)DT.DT_O1_RANK, i - 1].Style.BackColor = Color.LightCyan;
                        }
                        else
                        {
                            dataGridView1[(int)DT.DT_O1, i - 1].Style.BackColor = Color.White;
                            dataGridView1[(int)DT.DT_O1_RANK, i - 1].Style.BackColor = Color.White;
                        }
                    }
                    else
                    {
                        dataGridView1.Rows[i - 1].Cells[(int)DT.DT_O1_RANK].Value = O1[3];
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

            /* 馬体重データ取得 */
            if(!BataijuFlg)
            {
                JvDbWhData JvWh = new JvDbWhData();
                List<JvWhData> Whdata = new List<JvWhData>();
                if (JvWh.JvDbWhLinkData(RaClassData.GET_RA_KEY()) == 1)
                {
                    SetbataijuData();
                }
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            String key = "";
            RaClassData.GET_AUTO_RA_KEY(ref key);
            form.info.RaceResult result = new form.info.RaceResult(key, CourceColor);
            if (result.SetData() != 0)
            {
                result.Show();
            }
            else
            {
                MessageBox.Show("当該レースは確定していません。");
                return;
            }
        }

        private void Label16_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        Shirushi shirushi = new Shirushi();
        int tmpShirushiParam = 0;
        Boolean ShirushiWriteFlg = false;   //印書き込みフラグ

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == (int)DT.DT_BUTTON && e.RowIndex >= 0)
            {
                ShirushiWriteFlg = true;

                //右クリック？左クリック
                if (e.Button == MouseButtons.Right)
                {
                    //コンテキストメニューを表示する。
                    System.Drawing.Point p = System.Windows.Forms.Cursor.Position;
                    this.shirushiMenu.Show(p);
                    tmpShirushiParam = e.RowIndex + 1;
                }
                else
                {
                    //印を入れる
                    //セルの値を取得
                    String tmp = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = shirushi.ShirushiNextToString(tmp);
                }
            }
        }

        private void shirushiMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            String tmp = "";
            LOG.CONSOLW_DEBUG("ShirushiItemChiked");
            if(tmpShirushiParam != 0 )
            {
                //無印は空文字に変換する。
                if(e.ClickedItem.ToString() == "(無印)")
                {
                    tmp = "";
                }
                else
                {
                    tmp = e.ClickedItem.ToString();
                }

                dataGridView1.Rows[tmpShirushiParam - 1].Cells[(int)DT.DT_BUTTON].Value = tmp;
                tmpShirushiParam = 0;
            }
        }

        /* フォームが閉じることが確定したことを通知するためのハンドラー */
        private void Syutsuba_FormClosed(object sender, FormClosedEventArgs e)
        {
            //印の書き込み
            {
                if(ShirushiWriteFlg)
                {
                    String tmp = SetShirushiToDb();
                    int ret = 0;
//db.DeleteCsv("SL", );
                    db.DeleteCsv("SL", RaClassData.GET_RA_KEY().Substring(0, 8) + "/" + "SL" + RaClassData.GET_RA_KEY() + ".csv");
                    db = new dbConnect(RaClassData.GET_RA_KEY(), "SL", ref tmp, ref ret);
                    LOG.CONSOLE_TIME_MD(SPEC, "Shirushi WriteComp ret->" + ret);
                }
            }

        }

        /* 印をDB書き込み用に設定する。*/
        private String SetShirushiToDb()
        {
            String tmp = "";
            tmp = RaClassData.GET_RA_KEY() + "\r\n";
            for (int i=1; i<dataGridView1.Rows.Count; i++)
            {
                tmp += RaClassData.GET_RA_KEY() + String.Format("{0:00}", i) + "," + dataGridView1.Rows[i-1].Cells[(int)DT.DT_BUTTON].Value + "\r\n";
            }

            return tmp;
        }

        /* 印をDBから読み込む */
        private Boolean GetShirushiToDb( ref String[] inParam )
        {
            List<String> tmpData = new List<string>();
            db.TextReader_Row(RaClassData.GET_RA_KEY(), "SL", 1, ref tmpData);
            if( inParam == null || tmpData.Count == 0)
            {
                return false;
            }
            
            for(int i = 1; i< tmpData.Count; i++)
            {
                inParam[i - 1] = tmpData[i - 1]; 
            }
            return true;
        }

        //コースレコード処理
        private void SetRecordData( String Key )
        {
            if(Key == "")
            {
                return;
            }

            List<String> tmpArrayData = new List<string>();
            String tmpKey = "";

            switch (RaClassData.getOldYear())
            {
                case "11":
                    tmpKey = "A";
                    break;
                case "12":
                case "13":
                case "14":
                    tmpKey = "B";
                    break;
                case "18":
                case "19":
                    tmpKey = "C";
                    break;
                default:
                    return;
            }

            tmpKey += RaClassData.getRaceCource() + RaClassData.getCourceTrack() + RaClassData.getDistance();
            db.TextReader_Col("0", "RC", 0, ref tmpArrayData, tmpKey);
            if(tmpArrayData.Count == 0 )
            {
                return;
            }

            OutComRecordData(0, tmpArrayData.ToArray());

        }


        //コースレコード処理
        private void SetRaceRecordData(String Key)
        {
            if (Key == "")
            {
                return;
            }

            JvDbRcData Rc = new JvDbRcData();

            List<String> tmpArrayData = new List<string>();
            String tmpKey = "";

            tmpKey = "G" + String.Format("{0:00}", Rc.CheckRaceName6( RaClassData.getRaceName6() ));
            db.TextReader_Col("0", "RC", 0, ref tmpArrayData, tmpKey);
            if (tmpArrayData.Count == 0)
            {
                return;
            }

            OutComRecordData(1, tmpArrayData.ToArray());

        }

        private void OutComRecordData( int kind, String[] inParam )
        {
            String tmp = "";
            switch(kind)
            {
                case 0: //コースレコード
                    rcRecordPanel.Visible = true;
                    tmp = inParam[0].Substring(1, 2);
                    rcCource.Text = LOG.JvSysMappingFunction(2001, ref tmp);
                    tmp = inParam[0].Substring(3, 2);
                    rcCource.Text += LOG.JvSysMappingFunction(20091, ref tmp);
                    rcTime.Text = (inParam[10].Substring(0, 1) == "0" ? "" : inParam[10].Substring(0, 1) + ":") + inParam[10].Substring(1, 2) + "." + inParam[10].Substring(3, 1);
                    rcCource.Text += RaClassData.getDistance() + "m";
                    rcName.Text = inParam[11].Trim();
                    rcJockey.Text = inParam[15].Trim();
                    rcJockey.Text += " (" + inParam[14].Substring(0, 2) + "." + inParam[14].Substring(2, 1) + "kg)";
                    rcCond.Text = inParam[1].Substring(0, 4) + "/" + inParam[1].Substring(4, 2) + "/" + inParam[1].Substring(6, 2);
                    tmp = inParam[7];
                    rcCond.Text += " " + LOG.JvSysMappingFunction(2011, ref tmp);
                    tmp = inParam[8];
                    rcCond.Text += "・" + LOG.JvSysMappingFunction(2010, ref tmp);
                    rcRaceName.Text = inParam[3].Trim();
                    break;
                case 1: //G1レースレコード
                    rrRecordPanel.Visible = true;
                    tmp = inParam[0].Substring(1, 2);
                    rrRaceName.Text = RaClassData.getRaceName6();
                    rrTime.Text = (inParam[10].Substring(0, 1) == "0" ? "" : inParam[10].Substring(0, 1) + ":") + inParam[10].Substring(1, 2) + "." + inParam[10].Substring(3, 1);
                    rrName.Text = inParam[11].Trim();
                    rrJockey.Text = inParam[15].Trim();
                    rrJockey.Text += " (" + inParam[14].Substring(0, 2) + "." + inParam[14].Substring(2, 1) + "kg)";
                    rrCond.Text = inParam[1].Substring(0, 4) + "/" + inParam[1].Substring(4, 2) + "/" + inParam[1].Substring(6, 2);
                    tmp = inParam[7];
                    rrCond.Text += " " + LOG.JvSysMappingFunction(2011, ref tmp);
                    tmp = inParam[8];
                    rrCond.Text += "・" + LOG.JvSysMappingFunction(2010, ref tmp);
                    break;
            }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void FNoButton_Click(object sender, EventArgs e)
        {
            //レースキーを取得
            String SE_KEY = RaClassData.GET_RA_KEY();
            List<String> Arraytmp = new List<string>();
            List<JvFamilyNo> ret = new List<JvFamilyNo>();
            JvFamilyNo tmpFamliNoData = new JvFamilyNo();

            String tmpString = "";

            String outKettoNumber = "";
            String outFmailyNumber = "";

            //net競馬と接続するクラス
            siteNetKeiba siteNetClass = new siteNetKeiba();

            //ステータスを更新
            toolStripStatusLabel1.Text = "ファミリーナンバーを取得中...";
            statProgressBar.Maximum = RaClassData.Tosu1;
            statProgressBar.Visible = true;

            //ファミリーナンバーconfファイルが読めているかチェック：初回は読んで２回目はパス
            if(FnDataClass == null)
            {
                FnDataClass = new JvFamilyNoData();
            }

            //ファミリーナンバー取得
            //1回取得したら保持しておく？
            for (int i = 1; i <= RaClassData.Tosu1; i++)
            {
                statProgressBar.Value = i;
                Arraytmp.Clear();
                outFmailyNumber = "";

                //血統登録番号を取得
                if (db.TextReader_Col(SE_KEY.Substring(0, 8), "SE", 0, ref Arraytmp, SE_KEY + String.Format("{0:00}", i)) == 0)
                {

                    break;
                }

                if (siteNetClass.InitScraping() == 0) siteNetClass = new siteNetKeiba();

                //血統番号を渡してnetkeibaからファミリーナンバーを取得する。
                siteNetClass.GetFamilyNumber(Arraytmp[7]);


#if false
                if(res == 99)
                {
                    while(siteNetClass.CheckScrapingStat() == 99)
                    {
                        LOG.CONSOLE_WRITER("wait...");
                        //Thread.Sleep(100);
                    }
                }



                if (res < 0)
                {
                    //0を下回る(マイナス)の場合、取得に失敗した。
                    switch (res)
                    {
                        case -1: //パラメーターエラー
                            continue; //次のループへ
                        case -2:
                            return; //インターネットに接続できない。
                        case -3:
                            //NdData(ファミリーナンバーが書いてない)
                            //母馬を探して再度検索をかける。
                            String MotherName = SearchGrandmaToFamilyNo(Arraytmp[7]);
                            if (MotherName == "---")
                            {
                                siteNetClass.SetNodata(Arraytmp[7], "---");
                                //   ret.Add(tmpFamliNoData);
                                break;
                            }
                            continue;
                        default:
                            break;
                    }
                }
#endif
                Task<JvFamilyNo> resFamilyNo = Task.Run(() =>
                   {
                       return siteNetClass.GetFamilyNumberToReturn(Arraytmp[7]);
                   });


              //  Task<JvFamilyNo> resFamilyNo = siteNetClass.GetFamilyNumberToReturn(Arraytmp[7]);
                JvFamilyNo msg = resFamilyNo.Result;

                //結果を確認する。
                int res = siteNetClass.CheckScrapingStat();

                if (msg.FamilyNumber1 == null)
                {
                    //NdData(ファミリーナンバーが書いてない)
                    //母馬を探して再度検索をかける。
                    String MotherName = SearchGrandmaToFamilyNo(Arraytmp[7]);
                    
                    if (MotherName == "---")
                    {
                        //母馬が見つからない
                        //   ret.Add(tmpFamliNoData);
                        continue;
                    }

                    resFamilyNo = Task.Run(() =>
                    {
                        return siteNetClass.GetFamilyNumberToReturn(MotherName);
                    });

                    msg = resFamilyNo.Result;
                    if (msg.FamilyNumber1 == null)
                    {
                        siteNetClass.SetNodata(Arraytmp[7], "---");
                    }
                }


            }

            //ここから下は正常に成功

            dataGridView1.Columns["FNo"].Visible = true;
            dataGridView1.Columns["FnoColor"].Visible = true;


            //dataGridView1.Rows[n].Cells[(int)DT.DT_FNO_NAME].Value = ret[n].FamilyNumber1;

            for(int i= 0; i < dataGridView1.Rows.Count; i++)
            {
                siteNetClass.GetIndexData(i, ref outKettoNumber, ref outFmailyNumber);
                if (outFmailyNumber == "")
                {
                    outFmailyNumber = "---";
                }

                dataGridView1.Rows[i].Cells[(int)DT.DT_FNO_NAME].Value = outFmailyNumber;
                dataGridView1.Rows[i].Cells[(int)DT.DT_FNO_COLOR].Style.BackColor = (outFmailyNumber == "---" ? Color.White : FnDataClass.JvFnColorData(outFmailyNumber));
            }


#if false

                //ファミリーナンバーインスタンスを初期化
                tmpFamliNoData = new JvFamilyNo();







                //netkeibaサイトに接続し、ファミリーナンバーを取得する。
                /* 血統登録番号は10桁固定 */
                if (Arraytmp[7].Length != 10)
                {
                    return;
                }
                try
                {
                    var doc = default(IHtmlDocument);
                    using (var client = new HttpClient())
                    using (var stream = await client.GetStreamAsync(new Uri(JvFamilyNo.NETKEIBA_SITE_URL_HEAD + Arraytmp[7] + "/")))
                    {
                        // AngleSharp.Html.Parser.HtmlParserオブジェクトにHTMLをパースさせる
                        var parser = new HtmlParser();
                        //doc = await parser.ParseAsync(stream); →現在使用不可
                        doc = await parser.ParseDocumentAsync(stream);
                    }

                    var param = doc.QuerySelector(JvFamilyNo.SELECTER);

                    if(param == null)
                    {
                        LOG.CONSOLE_TIME_MD(SPEC, "NoData... index:" + i);
                        //netkeibaにデータが存在しないときはハイフンを入れておく
                        //母馬でリトライする。

                        String MotherName = SearchGrandmaToFamilyNo(Arraytmp[7]);

                        if(MotherName == "---")
                        {
                            tmpFamliNoData.FamilyNumber1 = "---";
                            ret.Add(tmpFamliNoData);
                            continue;
                        }

                        using (var client = new HttpClient())
                        using (var stream = await client.GetStreamAsync(new Uri(JvFamilyNo.NETKEIBA_SITE_URL_HEAD + Arraytmp[7] + "/")))
                        {
                            // AngleSharp.Html.Parser.HtmlParserオブジェクトにHTMLをパースさせる
                            var parser = new HtmlParser();
                            //doc = await parser.ParseAsync(stream); →現在使用不可
                            doc = await parser.ParseDocumentAsync(stream);
                        }

                        param = doc.QuerySelector(JvFamilyNo.SELECTER);

                        if(param != null)
                        {
                            //このあとの処理を継続させるため、なにもしない
                        }
                        else
                        {
                            tmpFamliNoData.FamilyNumber1 = "---";
                            ret.Add(tmpFamliNoData);
                            continue;
                        }
                    }

                    if (param.ParentElement.InnerHtml.Contains("FNo."))
                    {
                        int StrNumber = param.ParentElement.InnerHtml.IndexOf("FNo.");
                        int EndNumber = param.ParentElement.InnerHtml.IndexOf("]", StrNumber) - (StrNumber + 5);

                        
                        tmpString = param.ParentElement.InnerHtml.Substring(StrNumber + 5, EndNumber);
                        LOG.CONSOLE_TIME_MD(SPEC, i + ": Fno:" + tmpString);
                        tmpFamliNoData.FamilyNumber1 = tmpString;
                        ret.Add(tmpFamliNoData);
                    }
                    else
                    {
                        LOG.CONSOLE_MODULE(SPEC, "NotConnect");
                    }


                }
                catch (HttpRequestException)
                {
                    LOG.CONSOLE_TIME_MD(SPEC, "disconnect... DomeinNameServerError db.netkeiba.com");
                    return; //ネットにつながらない場合、即終了。
                }

            }
#endif
            toolStripStatusLabel1.Text = "準備完了";
            statProgressBar.Visible = false;
        }

        private String SearchGrandmaToFamilyNo(String KettoNumber)
        {
            String ret = "---";

            if(KettoNumber.Length != 10)
            {
                LOG.ASSERT("KettoNumber Length Error! -> " + KettoNumber);
                return ret;
            }

            //UMマスタから母馬を探す。
            JvDbUmData UM = new JvDbUmData(true, "UM", "0");

            //UM情報をすべてもらう
            List<String> Umdata = new List<string>();
            int res = UM.JvDbUmReadAllData(ref Umdata);

            //UMデータがセットされればresに1以上が返る。
            if(res == 0 || Umdata.Count == 0)
            {
                return ret;
            }

            //この馬の母馬を探す。
            for(int i=0; i<Umdata.Count; i++)
            {
                var param = Umdata[i].Split(',');
                if (KettoNumber == param[0])
                {
                    LOG.CONSOLE_MODULE(SPEC, param[21]);
                    return param[21];   //母馬繁殖馬情報を返す。
                }
            }
            return "---";

        }

        private void button6_Click(object sender, EventArgs e)
        {
            //指定距離の傾向を探す。

            data.DataList DataForm = new data.DataList();
            DataForm.Show();
        }

        private void Syutsuba_Activated(object sender, EventArgs e)
        {
            this.dataGridView1.ShowCellToolTips = true;
        }

        private void Syutsuba_Deactivate(object sender, EventArgs e)
        {
            this.dataGridView1.ShowCellToolTips = false;
        }

        #region 調教タイムボタン
        private void button7_Click(object sender, EventArgs e)
        {
            LOG.CONSOLE_TIME_MD("SY", "Chokyo Button On");
            Chokyo chokyo = new Chokyo(RaClassData.GET_RA_KEY(), CourceColor);
            chokyo.Show();
            LOG.CONSOLE_TIME_MD("SY", "Chokyo Show End");
        }
        #endregion


        #region 分析ボタン
        private void button8_Click(object sender, EventArgs e)
        {
            LOG.CONSOLE_TIME_MD("SY", "Analyze Button On");
            //ここではすでにインスタンスは有効化されている前提
            //Analyze.RaceAnalyzeExec(hc:, rc:, type:, ret)
            HorceInfo hc = new HorceInfo();
            RaceInfo rc = new RaceInfo();
            Details dt = new Details();
            String refString = "";
            double retValue = 0;
            List<String> Arraytmp;

            refString = RaClassData.getCourceTrack();
            rc.Baba1 = LOG.JvSysMappingFunction(20091, ref refString).Substring(0,1);

            rc.Distance1 = Int32.Parse(RaClassData.getDistance());

            rc.Cource1 = this.RaceNum.Text; //出馬表は正しく表示されている前提

            //レースキーは固定のため、先に入れておく
            String SE_KEY = RaClassData.GET_RA_KEY();

            //UMマスターを取得する
            List<String> UM_Master = new List<string>();
            db.DbReadAllData("0", "UM", 0, ref UM_Master, SE_KEY, 0);
            if(UM_Master.Count == 0)
            {
                //UMの取得に失敗
                LOG.CONSOLE_MODULE(SPEC, "UM..ReadErr...");
                return;
            }

            //SEマスターを取得する
            List<String> SE_Master = new List<string>();
            db.DbReadAllData("0", "SE", 0, ref SE_Master, SE_KEY, 0);
            if (SE_Master.Count == 0)
            {
                //UMの取得に失敗
                LOG.CONSOLE_MODULE(SPEC, "SE..ReadErr...");
                return;
            }


            //出走馬データの存在有無
            if (horceClasses.Count == 0)
            {
                LOG.CONSOLE_MODULE(SPEC, "HorceClasses...ReadErr");
                return;
            }

            //出走馬分のループを回す
            for (int i = 0; i < RaClassData.Tosu1; i++)
            {
                hc = new HorceInfo();
                Arraytmp = new List<string>();

                if (db.TextReader_Col(SE_KEY.Substring(0, 8), "SE", 0, ref Arraytmp, SE_KEY + String.Format("{0:00}", i + 1)) == 0)
                {
                    continue;
                }
                if (Arraytmp.Count == 0) { break; }

                hc.Umaban1 = Int32.Parse(Arraytmp[SE_UMA]);
                dt.Name = Arraytmp[SE_JOCKEY];
                dt.Value = Arraytmp[SE_JOCKEY_CODE];
                hc.SetJockey(dt);

                dt = new Details();
                dt.Name = Arraytmp[SE_TRANER_NAME];
                dt.Value = Arraytmp[SE_TRANER_CODE];
                hc.SetTrainer(dt);

                hc.Futan1 = float.Parse(Arraytmp[SE_FUTAN].Substring(0, 2) + "." + Arraytmp[SE_FUTAN].Substring(2, 1));

                //前走騎手
                dt = new Details();
                //String name = "";
                for(int j=0; j < SE_Master.Count; j++)
                {
                    //カンマ区切りにする
                    var param = SE_Master[j].Split(',');
                    if ( Arraytmp[7] == param[7].Substring(0,10) )
                    {
                        //name = param[7].Substring(0, 10);

                        //前走騎手の名前・コードを入れる
                        dt.Name = param[16];
                        dt.Value = param[15];
                        hc.SetOldJockey(dt);
                        break;
                    }
                }


                for (int j = 0; j < UM_Master.Count; j++)
                {
                    //カンマ区切りにする
                    var param = UM_Master[j].Split(',');
                    //horceClassesには出走馬順に入っている前提
                    if (param[0] == horceClasses[i].KettoNum1.ToString())
                    {
                        //String[]型とList<String>となっているため、ArrayTmpにいれるように変換する
                        for (int idx = 0; idx < param.Length; idx++)
                        {
                            dt = new Details();
                            dt.Name = param[6];
                            dt.Value = "";  //現状参照していないため
                            hc.SetSire(dt);
                            break;
                        }

                    }
                }


                //全部入れたら、ライブラリ関数コール
                if(Analyze.RaceAnalyzeExec(hc, rc, 1, ref retValue))
                {
                    LOG.CONSOLE_TIME_MD("SY", "Analyze num:" + i + "Value " + retValue);
                }
                else
                {
                    LOG.CONSOLE_TIME_MD("SY", "Analyze num:" + i + "Err");
                }

            }


            LOG.CONSOLE_TIME_MD("SY", "Analyze Button " + RaClassData.Tosu1 + " End");

        }
        #endregion
    }

    class Shirushi
    {
        struct SelectTable
        {
            public String Menu; //選択肢に表示する文字列
            public String Output; //出馬表に出力する文字列
        };

        const int selectMax = 10;

        const String ONE = "◎";
        const String TWO = "○";
        const String THR = "▲";
        const String FUR = "△";
        const String FIF = "×";
        const String SIX = "◇";
        const String SEV = "✓";
        const String EIG = "注";
        const String NIG = "消";
        const String TEN = "(無印)";

        SelectTable[] tmpSelect;

        public Shirushi()
        {
            tmpSelect = new SelectTable[selectMax];
            for(int i = 0; i < selectMax; i++)
            {
                switch(i)
                {
                    case 0:
                        tmpSelect[i].Menu = ONE;
                        tmpSelect[i].Output = ONE;
                        break;
                    case 1:
                        tmpSelect[i].Menu = TWO;
                        tmpSelect[i].Output = TWO;
                        break;
                    case 2:
                        tmpSelect[i].Menu = THR;
                        tmpSelect[i].Output = THR;
                        break;
                    case 3:
                        tmpSelect[i].Menu = FUR;
                        tmpSelect[i].Output = FUR;
                        break;
                    case 4:
                        tmpSelect[i].Menu = FIF;
                        tmpSelect[i].Output = FIF;
                        break;
                    case 5:
                        tmpSelect[i].Menu = SIX;
                        tmpSelect[i].Output = SIX;
                        break;
                    case 6:
                        tmpSelect[i].Menu = SEV;
                        tmpSelect[i].Output = SEV;
                        break;
                    case 7:
                        tmpSelect[i].Menu = EIG;
                        tmpSelect[i].Output = EIG;
                        break;
                    case 8:
                        tmpSelect[i].Menu = NIG;
                        tmpSelect[i].Output = NIG;
                        break;
                    case 9:
                        tmpSelect[i].Menu = TEN;
                        tmpSelect[i].Output = "";
                        break;

                }
            }
        }

        public String ShirushiNextToString( String Now )
        {
            int j = 0;
            for(int i=0; i<selectMax; i++)
            {
                if(tmpSelect[i].Output == Now)
                {
                    j = (i == (selectMax-1) ? 0 : i + 1);
                    return tmpSelect[j].Output;
                }
            }
            return "";
            
        }


    }


}
