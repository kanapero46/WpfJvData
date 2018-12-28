using LibJvConv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;   
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.dbAccess;
using WpfApp1.form;

namespace WpfApp1.form
{
    public partial class Syutsuba : Form
    {
        /* DB書き込みクラス */
        dbConnect db;
        Class.MainDataClass DataClass = new Class.MainDataClass();
        static String Cource;
        int CourceColor;

        /* 競走馬データ保存用 */
        List<Class.MainDataHorceClass> horceClasses;

        /* プログレスバー用ステータス */
        int ProgressStatus = 0;

        /* SEデータ取得用定数 */
        const int SE_WAKU = 5;
        const int SE_UMA = 6;
        const int SE_KETTO = 7;
        const int SE_NAME = 8;
        const int SE_FUTAN = 13;
        const int SE_JOCKEY = 16;
        const int SE_MINARA = 17;

        public Syutsuba()
        {
            InitializeComponent();

        }

        public Syutsuba(String RA, int Color)
        {
            InitializeComponent();
            String tmp = "";
            db = new dbConnect();
            //DBからレース名を検索
            db.TextReader_aCell("RA", RA, RA.Substring(0, 8), 5, ref tmp);
            DataClass.SET_RA_KEY(RA);
            DataClass.setRaceDate(RA.Substring(0, 8));
            DataClass.setRaceCoutce(RA.Substring(8, 2));
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

            CourceColor = Color;
            InitForm();
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

            String SE_KEY = DataClass.GET_RA_KEY();
            String tmp = "";
            List<String> Arraytmp = new List<string>();

            //RA初回読み込み時にエラーチェック ０もエラー
            if (db.TextReader_aCell("RA", SE_KEY, SE_KEY.Substring(0, 8), 19, ref tmp) != 1)
            {
                return;
            }

            t.Start(tmp);

            for (int i = 1; ;i++)
            {
                pHorceClasses = new Class.MainDataHorceClass();
                String covData = String.Format("{0:00}", i);


                //SE初回読み込み時にエラーチェック ０もエラー
                if (db.TextReader_aCell("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), 0, ref tmp)!=1)
                {
                    MessageBox.Show("DB読み込み中にエラーが発生しました。\nファイルが存在しないか、DBファイルが別プロセスで実行中です。");
                    break;
                }

                if (tmp == "0"|| tmp == "") { break; }
                else if (tmp.Substring(0,16) != SE_KEY) { break; }
                pHorceClasses.KEY1 = tmp;

                if(db.TextReader_Col("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), ref Arraytmp)!=1)
                {
                    break;
                }

                pHorceClasses.Waku1 = Arraytmp[SE_WAKU];
                pHorceClasses.Umaban1 = Arraytmp[SE_UMA];
                pHorceClasses.KettoNum1 = Int32.Parse(Arraytmp[SE_KETTO]);
                pHorceClasses.Name1 = Arraytmp[SE_NAME];
                pHorceClasses.Futan1 = Arraytmp[SE_FUTAN].Substring(0,2) + "." + Arraytmp[SE_FUTAN].Substring(2,1);
                pHorceClasses.Jockey1 = Arraytmp[SE_JOCKEY];

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

                /* 血統登録番号からマスタを取得 */
                if(db.TextReader_Col("UM", pHorceClasses.KettoNum1.ToString(), "0", ref Arraytmp)!=1)
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
                dataGridView1.Rows.Add(pHorceClasses.Waku1, pHorceClasses.Umaban1, pHorceClasses.Name1, pHorceClasses.MinaraiCd1,
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
                dataGridView1[6, i - 1].Style.BackColor = FuncBloodColor(pHorceClasses.F_NUM1, pHorceClasses.FF_NUM1, pHorceClasses.FFF_NUM1);
                dataGridView1[8, i - 1].Style.BackColor = FuncBloodColor(pHorceClasses.FM_NUM1, pHorceClasses.FMM_NUM1, null);
                dataGridView1[10, i - 1].Style.BackColor = FuncBloodColor(pHorceClasses.FFM_NUM1, null, null);

                /* プログレスバー更新 */
                ProgressStatus++;

            }

            t.Abort();
        }

        /*  DataGridView1[0, 0].Style.BackColor =*/

        unsafe private void Form2_Load(object sender, EventArgs e)
        {
            /* レース名書き込み */
        String Grade = DataClass.getRaceGrade();

            this.Text = Int32.Parse(DataClass.getRaceKaiji()) + Cource + Int32.Parse(DataClass.getRaceNichiji()) + " " 
                + DataClass.getRaceNum() + "R:" +DataClass.getRaceName();

            LabelCource.Text = Cource + DataClass.getRaceNum() + "Ｒ";

            if (Grade == "一般"||Grade == "特別"||Grade == "")
            {
                LabelRaceName.Text = DataClass.getRaceName();
            }
            else
            {
                LabelRaceName.Text = DataClass.getRaceName() + "(" + DataClass.getRaceGrade() + ")";
            }

            switch(CourceColor)
            {
                case 1:
                    flowLayoutPanel1.BackColor = Color.Blue;
                    break;
                case 2:
                    flowLayoutPanel1.BackColor = Color.Green;
                    break;
                case 3:
                    flowLayoutPanel1.BackColor = Color.Purple;
                    break;
            }

            /* トラック */

            int CODE = 2009;
            String tmp = DataClass.getCourceTrack();
            String LibTmp = "";
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            DataClass.setCourceTrack(LibTmp);

            LabelTrack.Text = DataClass.getCourceTrack() + "：" + DataClass.getDistance() + "m";

            /* フォントの変更 */
            dataGridView1.Font = new Font("Meiryo UI", 11);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9);

        }

        unsafe private void InitForm()
        {
            String LibTmp = "";
            int CODE = LibJvConvFuncClass.COURCE_CODE;

            String tmp = DataClass.getRaceCource();
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            Cource = LibTmp;

            /* DataGridViewの高さの変更を出来ないようにする */
            dataGridView1.AllowUserToResizeRows = false;

            /* レース名 */
            LabelCource.Text = Cource;
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

        /* 新種牡馬カラー取得関数 */
        private Color FuncBloodColor(String Key, String Key2, String Key3)
        {
            String tmp = "";
            String tmpColor = "";
            String Serach = "";
            int cnt = 0;
            Color ret;
            db = new dbConnect();
            if(Key == null) { return Color.White; }

            /* 1：設定データ(ST)からデータ取得する。 */
            db.TextReader_aCell("ST", Key, "0", 3, ref tmp);
            if(tmp != "" )
            {
                cnt++;
                 ret = g_FuncHorceKindColor(tmp);
                if(ret != Color.White)
                {
                    return ret;
                }
            }

            /* 1：設定データ(ST)からデータ取得する(3代)。 */
            db.TextReader_aCell("ST", Key2, "0", 3, ref tmp);
            if (Key2 != null && tmp != "")
            {
                cnt++;
                ret = g_FuncHorceKindColor(tmp);
                if (ret != Color.White)
                {
                    return ret;
                }
            }

            /* 1：設定データ(ST)からデータ取得する(2代)。 */
            db.TextReader_aCell("ST", Key3, "0", 3, ref tmp);
            if (Key3 != null && tmp != "")
            {
                cnt++;
                ret = g_FuncHorceKindColor(tmp);
                if (ret != Color.White)
                {
                    return ret;
                }
            }

            Serach = Key;

            /* ２：父の父を探す。５代まで */
            while (cnt < 5)
            {
                db.TextReader_aCell("HN", Serach, "0", 4, ref tmp);
                if (tmp != "")
                {
                    db.TextReader_aCell("ST", tmp, "0", 3, ref tmpColor);
                    if(tmpColor == "")
                    {
                        /* 検索に引っかからない場合はもう一度 */
                        Serach = tmp;
                        cnt++;
                        continue;
                    }

                    ret = g_FuncHorceKindColor(tmpColor);

                    if (ret != Color.White)
                    {
                        return ret;
                    }
                }
                else
                {

                }
                cnt++;
            }
            return Color.White;
        }

        private Color FuncHorceKindColor(String F, String FF, String FFF)
        {
            int All = 3;
            int ret = 0;
            String tmp = "";
            db = new dbConnect();
            Color clr;

            if (F == "")
            {
                return Color.White;
            }

            if(FF == "")
            {
                All = 1;
            }
            else if(FFF == "")
            {
                All = 2;
            }

            db.TextReader_aCell("ST", F, "0", 3, ref tmp);
            if(tmp == "") {  }
            else
            {
                return g_FuncHorceKindColor(tmp);
            }

            if (All == 1) { return Color.White; }

            db.TextReader_aCell("ST", FF, "0", 3, ref tmp);
            if (tmp == "") { }
            else
            {
                return g_FuncHorceKindColor(tmp);
            }

            if (All == 2) { return Color.White; }

            db.TextReader_aCell("ST", FFF, "0", 3, ref tmp);
            if (tmp == "") { }
            else
            {
                return g_FuncHorceKindColor(tmp);
            }

            return Color.White;
        }

        private Color g_FuncHorceKindColor(String Kind)
        {
            /** 種牡馬色定義　0：その他、１：ノーザンテースト系、２：ナスルーラ、３：ヘイロー系、４：サンデー系、５：ネイティブダンサー系
             * ６：セントサイモン、７：ハンプトン系、８：テディ系、９：マンノウォー 、１０：マッチェム*/
            db = new dbConnect();

            switch(Kind)
            {
                case "0":
                    return Color.Brown;
                case "1":
                    return Color.SkyBlue;
                case "2":
                    return Color.DeepPink;
                case "3":
                    return Color.LightGreen;
                case "4":
                    return Color.Yellow;
                case "5":
                    return Color.Orange;
                case "6":
                    return Color.MediumPurple;
                case "7":
                    return Color.DarkGreen;
                case "8":
                    return Color.LightGray;
                case "9":
                    return Color.DarkGoldenrod;
                case "10":
                    return Color.DarkGray;
                default:
                    return Color.White;
            }

        }


    }
}
