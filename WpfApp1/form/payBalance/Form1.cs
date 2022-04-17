using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.Class.com;
using System.Linq;

namespace WpfApp1.form.payBalance
{
    public partial class Form1 : Form
    {
        Class.com.JvComMain LOG = new Class.com.JvComMain();
        JvComDbData.JvDbOzAllClass PayAllData = new JvComDbData.JvDbOzAllClass();
        Class.com.prog_def DEF = new Class.com.prog_def();
        dbAccess.dbConnect db;

        //RAデータ
        JvComDbData.JvDbRaData RA = new JvComDbData.JvDbRaData();

        //SEデータ
        List<String> HorseData = new List<string>();

        const String MD = "PB";
        
        String Key = "";

        int[] DataGridSelectMode = new int[3]; //３つ分のモードを保持しておく
        const int DG_NON_SELECT = 0;    //未選択
        const int DG_CHECK_MODE = 1;    //チェックボックスモード(フォーメーション)
        const int DG_RADIO_MODE = 2;    //ラジオボタンモード(ながし・ボックス)

        //チェックボックス判定用フラグ
        bool Check1 = false;
        bool Check2 = false;
        bool Check3 = false;


        public Form1()
        {
            InitializeComponent();
        }

        public Form1(String iKey)
        {
            InitializeComponent();
            Key = iKey;
        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitData();
        }

        private void InitData()
        {
            //通常はありあえないが、キーが設定されているかチェックする。
            if (Key == "")
            {
                MessageBox.Show("必須データが設定されませんでした。");
                LOG.ASSERT(MD); //ログにアサート
                return;
            }

            //レース情報取得
            int ret = GetRaceData();
            if(ret == 0)
            {
                MessageBox.Show("レース情報の取得に失敗しました。");
                return;
            }

            //オッズデータ取得
            GetAllOdzzData();

            //Formにセット
            SetFormOutPutData();

            //オッズエリアのフォントを変更
            dataOdzz.DefaultCellStyle.Font = new Font("Meiryo UI", 12);

        }

        //レース情報取得
        private int GetRaceData()
        {
            List<String> tmpArray = new List<string>();
            db = new dbAccess.dbConnect();
            
            db.TextReader_Col(Key.Substring(0, 8), "RA", 0, ref tmpArray, Key);
            if(tmpArray.Count == 0)
            {
                LOG.CONSOLE_MODULE(MD, "GetRaceData NotData??");
                return 0;
            }

            //レースデータセット
            RA.setData(ref tmpArray);

            String tmpStr = RA.getRaceCource();
            Date.Text = RA.ConvertDateToDate(RA.getRaceDate());
            Kaisai.Text = "第" + RA.getRaceKaiji() + "回" + LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.COURCE_CODE, ref tmpStr) + "競馬" + RA.getRaceNichiji() + "日目";
            label4.Text = RA.ConvertTimeToString(RA.getRaceStartTime());
            weLabel.Text = RA.getWeather();
            TrackDistance.Text = RA.getTrackStatus();

            RaceNum.Text = LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.COURCE_CODE, ref tmpStr);
            rNum.Text = Int32.Parse(RA.getRaceNum()) + "R";

            if (RA.getRaceGradeKai() != 0)
            {
                kaiji.Visible = true;
                kaiji.Text = "第" + RA.getRaceGradeKai() + "回";
            }

            //レース開催中止
            if (RA.DataKubun1 == "9")
            {
                this.racename.Text = "【中止】 " + RA.getRaceNameFukus() + (RA.getRaceNameFukus().Length >= 1 ? " " : "") + RA.getRaceName() + (RA.getRaceNameEnd() == "" ? "" : "(" + RA.getRaceNameEnd() + ")");
            }
            else
            {
                this.racename.Text = RA.getRaceNameFukus() + (RA.getRaceNameFukus().Length >= 1 ? " " : "") + RA.getRaceName() + (RA.getRaceNameEnd() == "" ? "" : "(" + RA.getRaceNameEnd() + ")");
            }

            this.raceNameEng.Text = " " + RA.getRaceNameEng();


            //2019年6月～クラス名称変更対応
            if (RA.getRaceGrade() == "一般" &&
                (RA.getRaceClass() == "005" || RA.getRaceClass() == "010" || RA.getRaceClass() == "016"))
            {
                tmpStr = RA.getRaceClass();
                racename.Text += "（" + LOG.JvSysMappingFunction(20071, ref tmpStr) + "）";
            }

            if (RA.getRaceGradeKai() != 0 || RA.getRaceGrade() == "Ｌ") //リステッド競走対応
            {
                racename.Text += " （" + RA.getRaceGrade() + "）";
            }

            tmpStr = RA.getOldYear();
            this.OldYear.Text = LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.RACE_SHUBETSU_LONG_CODE, ref tmpStr);

            tmpStr = RA.getCourceTrack();
            int GetKind = (LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.TRACK_CODE_SHORT, ref tmpStr) == "芝" ? 2 : 3);
            TrackLabel.Text = (GetKind == 2 ? "芝" : "ダート");
            TrackLabel.BackColor = (GetKind == 2 ? Color.LightGreen : Color.Tan);

            tmpStr = RA.getRaceClass();
            ClassLabel.Text = LOG.JvSysMappingFunction(2007, ref tmpStr);

            tmpStr = RA.getRaceKindKigo();
            KigoLabel.Text = LOG.JvSysMappingFunction(2006, ref tmpStr);

            tmpStr = RA.getRaceHandCap();
            KigoLabel.Text = LOG.JvSysMappingFunction(2008, ref tmpStr);

            DistanceLabel.Text = RA.getDistance();

            tmpStr = RA.getCourceTrack();
            TrackNameLabel.Text = "（" + LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.TRACK_CODE, ref tmpStr) + "）";

            //タイトルバー
            this.Text = "【オッズ】" + RaceNum.Text + rNum.Text + "：" + RA.getRaceName6();

            return 1;
        }

        private void GetAllOdzzData()
        {
            form.odds.O1_Form O1 = new odds.O1_Form();
            long key = 0;

            //オッズデータがすでに取得されているかをチェックする。
            //以前のデータがあれば、更新しない。
            //全オッズデータ取得

            toolStripStatusLabel1.Text = DEF.FILE_OPEN("STR_ODZZ_NOW") + "...";
            this.Refresh();

            PayAllData = new JvComDbData.JvDbOzAllClass(Key);
            int ret = PayAllData.JvDbAllSetALLOddzData();

            if (long.TryParse(Key, out key) && ret == 0)
            {
                //キーが変換できた場合、もしくは、オッズが取得されていない場合にDataLabに接続し、オッズを取得
                O1.GetAllOddzData(key);
                //PayAllDataに改めてセット
                PayAllData.JvDbAllSetALLOddzData();
            }
            else if(ret == 1)
            {
                //すでにオッズデータが存在する場合は自動更新しない
                LOG.CONSOLE_MODULE(MD, "Already Data Exist");
            }
            else
            {
                MessageBox.Show("パラメーターエラーのため、処理を終了します。", "Keyエラー");
                return;
            }
            toolStripStatusLabel1.Text = DEF.FILE_OPEN("STR_STANDBY");
        }

        private void SetFormOutPutData()
        {
            object[] param = new object[5];

            //出走馬データを出力
            SetHourceData();

            //全データ取得
            SetOddzData();

        }

        private void SetOddzData()
        {
            //オッズはPayAllDataに入っている
            if (PayAllData == null) return;

            //各種発売フラグをチェック
            JvComDbData.JvDbOzAllClass.JV_OZ_HEADER OzHeader = new JvComDbData.JvDbOzAllClass.JV_OZ_HEADER();
            PayAllData.GetJvOzHeader(ref OzHeader);

            flowLayoutPanel7.Visible = true;

            if(OzHeader.fukushoFlg)
            {
                flowFukusho.Visible = true;
                label10.Text = OzHeader.fukushoPayOutRank + "着払い";
            }

            if(!OzHeader.wakurenFlg)
            {
                //枠連は発売ないときに表示する
                flowWakuren.Visible = true;
                label13.Text = DEF.FILE_OPEN("STR_WAKU_NOTRELEASE2");   //発売なし(8頭以下)
                //枠連ボタンを非表示にする
                button2.Visible = false;
                LOG.CONSOLE_MODULE(MD, "枠連発売なし");
            }

            //オッズ発表時間
            flowOdzzTime.Visible = true;
            if(OzHeader.PayOutFlg)
            {
                label2.Text = DEF.FILE_OPEN("STR_FIN_ODZZ");    //最終オッズ
                //最終オッズであれば、更新ボタンを押させない
                button4.Enabled = false;
            }
            else
            {
                label2.Text = OzHeader.outReleaseTime.Substring(0, 2) + "月" + OzHeader.outReleaseTime.Substring(2, 2) + "日" + OzHeader.outReleaseTime.Substring(4, 2) + "時" + OzHeader.outReleaseTime.Substring(6, 2) + "分発表";
            }

        }

        private void SetHourceData()
        {
            dataGridView1.DefaultCellStyle.Font = new Font("Meiryo UI", 12);
            List<DataGridViewRow> rowsArray = new List<DataGridViewRow>();

            //DBから全データ取得

            int cnt = db.DbReadAllData(Key.Substring(0, 8), "SE", 0, ref HorseData, Key, 0);
            if(HorseData.Count == 0)
            {
                LOG.CONSOLE_TIME_MD(MD, "param error!?!?");
                return;
            }

            //単勝オッズを取得する
            String[,] O1Ozzu = new string[18, 5];
            PayAllData.JvOzAllGetData(ref O1Ozzu, 1);
            
            int idx = 0;
            int UmabanIndex = 0;
            String O1Oz = "";
            int O1Oddz = 0;
            //ループして出走馬のデータを書き込み
            for(idx = 0; idx < HorseData.Count; idx++)
            {
                //1行ごとカンマ区切りを配列化する
                var values = HorseData[idx].Split(',');
                if (values.Length == 0) continue;
                //キーと合致するかチェックする
                if (Key + String.Format("{0:00}", UmabanIndex+1) == values[0])
                {
                    // データを格納
                    DataGridViewRow row = new DataGridViewRow();
                    row.CreateCells(dataGridView1);

                    //オッズを整形・馬名などのデータを書き込み
                    row.SetValues(new object[] { values[6], values[8], LOG.OddzStrToString(O1Ozzu[UmabanIndex, 1]), new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE")), new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE")), new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE")), "" });
                    row.Height =　50;
                    row.Cells[0].Style.BackColor = LOG.NumberStrToColor(values[5]);
                    row.Cells[0].Style.ForeColor = LOG.NumberStrToForeColor(values[5]);

                    //単勝オッズだけ出力する
                 //   dataGridView1.Rows[0].Cells[0].Value = O1Ozzu[UmabanIndex, 1];
                    // リストに追加
                    rowsArray.Add(row);
                    UmabanIndex++;
                }

            }

#if false
            for (int index = 0; index < 18; index++)
            {
                // データを格納
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.SetValues(new object[] { index+1, "ディープインパクト", new Bitmap("ico/CheckEnable.jpg"), new Bitmap("ico/CheckDisable2.jpg"), new Bitmap("ico/CheckDisable.jpg"), "" });
                row.Height = 30;

                // リストに追加
                rows[index] = row;
            }
#endif

            int arrCount = rowsArray.Count;

            // DataGridViewに格納
            if(rowsArray.Count != 0)
            {
                DataGridViewRow[] rows = new DataGridViewRow[arrCount];
                //List型に持っているものを配列型に入れる
                for (int i = 0; i < arrCount; i++) rows[i] = rowsArray[i];
                //DatagridViewに書き込む
                dataGridView1.Rows.AddRange(rows);
            }

            //チェックボックスを一旦非表示に
            DataGridSelectMode[0] = DG_NON_SELECT;
            DataGridSelectMode[1] = DG_NON_SELECT;
            DataGridSelectMode[2] = DG_NON_SELECT;
            dataGridView1.Columns["Select1"].Visible = false;
            dataGridView1.Columns["Select2"].Visible = false;
            dataGridView1.Columns["Select3"].Visible = false;

        }
        const int MainButtonLayerMax = 3;
        int[] RecMainButtonType = new int[MainButtonLayerMax];
        
        //メインボタン押下時の共通イベント
        private void ProcCommonButtonMenus1(int Type)
        {
            int CheckBoxArea = 0;

            //Subボタンをすべてfalse(非表示)にする
            Sub1.Visible = false;
            Sub2.Visible = false;
            Sub3.Visible = false;
            Sub4.Visible = false;
            Sub5.Visible = false;
            //第1階層が押されたら、第3階層も一旦非表示にする
            flowLayoutPanel2.Visible = true;
            flowLayoutPanel3.Visible = false;
            //選択ボタン格納配列も初期化する
            RecMainButtonType = new int[MainButtonLayerMax];
            //人気順チェックを一旦非表示にする
            RankSortOption.Visible = false;


            switch (Type)
            {
                case 1: //単・複ボタン
                    Sub1.Visible = true;
                    Sub1.Text = DEF.FILE_OPEN("通常");
                    Sub1.Enabled = false;   //非活性：常時表示のため
                    Sub2.Visible = true;
                    Sub2.Text = DEF.FILE_OPEN("人気順");
                    CheckBoxArea = 1;
                    break;
                case 7: //応援馬券
                    //サブボタンを通常と人気順にする
                    Sub1.Visible = true;
                    Sub1.Text = DEF.FILE_OPEN("通常");
                    Sub2.Visible = true;
                    Sub2.Text = DEF.FILE_OPEN("人気順");
                    CheckBoxArea = 1;
                    break;
                case 2: //枠連ボタン
                case 3: //馬連・ワイドボタン
                case 4: //馬単ボタン
                    Sub1.Visible = true;
                    Sub2.Visible = true;
                    Sub3.Visible = true;
                    Sub4.Visible = true;
                    Sub5.Visible = true;
                    Sub1.Enabled = true;
                    Sub2.Enabled = true;
                    Sub3.Enabled = true;
                    Sub4.Enabled = true;
                    Sub5.Enabled = true;
                    Sub1.Text = DEF.FILE_OPEN("通常");
                    Sub2.Text = DEF.FILE_OPEN("人気順");
                    Sub3.Text = DEF.FILE_OPEN("フォーメーション");
                    Sub4.Text = DEF.FILE_OPEN("ボックス");
                    Sub5.Text = DEF.FILE_OPEN("ながし");
                    CheckBoxArea = 2;
                    break;
                case 5: //３連複
                case 6: //3連単
                    //サブボタンを通常・人気順・フォーメーション・ボックス・ながしにする。
                    Sub1.Visible = true;
                    Sub2.Visible = true;
                    Sub3.Visible = true;
                    Sub4.Visible = true;
                    Sub5.Visible = true;
                    Sub1.Enabled = true;
                    Sub2.Enabled = true;
                    Sub3.Enabled = true;
                    Sub4.Enabled = true;
                    Sub5.Enabled = true;
                    Sub1.Text = DEF.FILE_OPEN("通常");
                    Sub2.Text = DEF.FILE_OPEN("人気順");
                    Sub3.Text = DEF.FILE_OPEN("フォーメーション");
                    Sub4.Text = DEF.FILE_OPEN("ボックス");
                    Sub5.Text = DEF.FILE_OPEN("ながし");
                    CheckBoxArea = 3;
                    break;
                default:
                    return;
            }
            RecMainButtonType[0] = Type;

            //チェックボックス表示エリアを切り替える
            DesignChangeHourceSelectArea(CheckBoxArea);

        }
        
        private void ProcCommonSubButtonMenus1(int Type)
        {
            int CheckBoxArea = 0;
            if(RecMainButtonType[0] == 0)
            {
                //通常ありえない
                return;
            }

            //ボタンエリアを有効にするにはPanel3を有効にする
            //flowLayoutPanel3.Visible = false;

            //押したボタンで処理を変える
            switch(Type)
            {
                case 1: //通常(Sub1)
                case 2: //人気順(SUb2)
                    //次に進むメニューはないため、第3階層を表示しない
                    flowLayoutPanel3.Visible = false;
                    //人気順にする必要はないため非表示にする
                    RankSortOption.Visible = false;
                    
                    break;
                case 3: //フォーメーション
                case 4: //ボックス
                    //次に進むメニューはないため、第3階層を表示しない
                    flowLayoutPanel3.Visible = false;
                    //フォーメーション・ボックス・ながしの場合は人気順で表示するかメニューを表示する
                    RankSortOption.Visible = true;
                    break;
                case 5: //ながし
                    //ながし馬券の場合、次に進む(軸1頭、2頭、着順指定)
                    ProcWhell(Type);
                    //ながしの場合には人気順メニューを表示する
                    RankSortOption.Visible = true;
                    break;
            }
            //第2階層のボタンを入れておく。
            RecMainButtonType[1] = Type;
            CommonOddzOutputFunc();

        }

        //ボタン押下状況からながしに関連するボタンを表示する。
        private void ProcWhell(int Type)
        {
            if(Type != 5 || RecMainButtonType[0] == 0)
            {
                //異常なため、第3階層を表示しないようにする。
                flowLayoutPanel3.Visible = false;
            }

            flowLayoutPanel3.Visible = true;
            //馬券種類によって表示するボタンを変更する
            switch (RecMainButtonType[0])
            {
                case 2: //枠連
                case 3: //馬連・ワイド
                    //枠連・馬連・ワイドの場合は、ただの流し(通常)と表示し、押下できないようにする。
                    l3Button1.Visible = true;
                    l3Button1.Text = DEF.FILE_OPEN("通常");
                    l3Button1.Enabled = false;
                    //それ以外は非表示に設定
                    l3Button2.Visible = false;
                    l3Button3.Visible = false;
                    l3Button4.Visible = false;
                    l3Button5.Visible = false;
                    l3Button6.Visible = false;
                    //ながしの処理を呼ぶ
                    ProcLayer3Common(1);
                    break;
                case 4: //馬単
                        //馬単の場合、1着・2着固定があるため、1着固定・2着固定ボタンを表示させる
                    l3Button1.Visible = true;
                    l3Button1.Text = DEF.FILE_OPEN("1着固定ながし");
                    l3Button1.Enabled = true;
                    //2着固定
                    l3Button2.Visible = true;
                    l3Button2.Text = DEF.FILE_OPEN("2着固定ながし");
                    l3Button2.Enabled = true;
                    //あとは非表示
                    l3Button3.Visible = false;
                    l3Button4.Visible = false;
                    l3Button5.Visible = false;
                    l3Button6.Visible = false;
                    break;
                case 5: //3連複
                        //3連複は、軸1頭ながし、軸2頭ながしがある。
                    l3Button1.Visible = true;
                    l3Button1.Text = DEF.FILE_OPEN("軸1頭ながし");
                    l3Button1.Enabled = true;
                    //2着固定
                    l3Button2.Visible = true;
                    l3Button2.Text = DEF.FILE_OPEN("軸2頭ながし");
                    l3Button2.Enabled = true;
                    //あとは非表示
                    l3Button3.Visible = false;
                    l3Button4.Visible = false;
                    l3Button5.Visible = false;
                    l3Button6.Visible = false;
                    break;
                case 6: //3連単は1着固定、2着固定、3着固定、1・2着固定、1・3着固定、2・3着固定がある
                    l3Button1.Visible = true;
                    l3Button1.Text = DEF.FILE_OPEN("1着固定ながし");
                    l3Button1.Enabled = true;
                    //2着固定
                    l3Button2.Visible = true;
                    l3Button2.Text = DEF.FILE_OPEN("2着固定ながし");
                    l3Button2.Enabled = true;
                    //3着固定
                    l3Button3.Visible = true;
                    l3Button3.Text = DEF.FILE_OPEN("3着固定ながし");
                    l3Button3.Enabled = true;
                    //1・2着固定
                    l3Button4.Visible = true;
                    l3Button4.Text = DEF.FILE_OPEN("STR_12WHELL");
                    l3Button4.Enabled = true;
                    //1・3着固定
                    l3Button5.Visible = true;
                    l3Button5.Text = DEF.FILE_OPEN("STR_13WHELL");
                    l3Button5.Enabled = true;
                    //2・3着固定
                    l3Button6.Visible = true;
                    l3Button6.Text = DEF.FILE_OPEN("STR_23WHELL");
                    l3Button6.Enabled = true;
                    break;
                default:
                    return;
            }
            CommonOddzOutputFunc();
        }

        private void CommonOddzOutputFunc()
        {
            List<DataGridViewRow> Rows = new List<DataGridViewRow>();
            String[,] param;

            //使う方をクリアすること
            List<String> tmpStrArray = new List<string>();
            List<int[]> tmpIntArray = new List<int[]>();

            int[] tmpIntparam;

            //人気順のソート
            bool RankSort = false;
            //枠連の場合
            bool Wakuban = false;
            //必要な表示エリア(単勝なら1)
            int DataAreaNum = 0;
            //単複の場合
            bool FukushoArea = false;
            //オッズ格納用
            int OddzData = 0;
            //値格納用
            int RefParam = 0;

            //dataOdzzエリアが非表示であれば表示する
            dataOdzz.Visible = true;

            //1頭目・オッズ以外は一旦全部非表示に
            dataOdzz.Columns["Fukusho"].Visible = false;
            dataOdzz.Columns["tag1"].Visible = false;
            dataOdzz.Columns["content2"].Visible = false;
            dataOdzz.Columns["tag2"].Visible = false;
            dataOdzz.Columns["content3"].Visible = false;
            dataOdzz.Columns["RankNo"].Visible = false;

            //共通でデータテーブル削除
            
            dataOdzz.Rows.Clear();

            if (RecMainButtonType[0] == 1)
            {
                //単勝
                switch (RecMainButtonType[1])
                {
                    case 1:
                        //通常→押せない
                        break;
                    case 2:
                        //人気順
                        //オッズ取得
                        RankSort = true;
                        param = new String[18, 5];
                        DataAreaNum = 1;    //単勝と複勝
                        PayAllData.JvOzAllGetData(ref param, 1);
                        tmpIntArray.Clear();
                        //必要なエリアを表示する
                        dataOdzz.Columns["Fukusho"].Visible = true;
                        //dataOdzz.Columns["RankNo"].Visible = true
                        ;
                        for (int i = 0; i < param.GetLength(0); i++)
                        {
                            int[] tmpInt = new int[5];
                            if (param[i, 1] != null)
                            {
                                tmpInt[0] = i + 1;  //馬番
                                tmpInt[1] = Int32.TryParse(param[i, 1], out OddzData) == true ? OddzData : -1;
                                //                        tmpIntArray.Add(Int32.TryParse(param[i, 1], out int iOut) == true ? iOut : -1);
                                tmpInt[2] = Int32.TryParse(param[i, 4], out OddzData) == true ? OddzData : 99;
                                tmpInt[3] = Int32.TryParse(param[i, 1], out OddzData) == true ? OddzData : -1;
                                tmpInt[4] = Int32.TryParse(param[i, 1], out OddzData) == true ? OddzData : -1;
                                tmpIntArray.Add(tmpInt);
                            }
                            else
                            {
                                break;
                            }
                        }

                        //１つあたりのint配列数を保持
                        if (tmpIntArray.Count != 0)
                        {
                            //データエリアを初期化
                            for (int i = 0; i < tmpIntArray.Count; i++)
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataOdzz);
                                int[] intNum = tmpIntArray[i];
                                row.SetValues(new object[] { intNum[2], intNum[0], "", "", "", "", intNum[2], LOG.OddzStrToString(intNum[1].ToString()), (LOG.OddzStrToString(intNum[3].ToString()) + "-" + LOG.OddzStrToString(intNum[4].ToString())) });
                                row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(intNum[0]).ToString());
                                row.Cells[1].Style.ForeColor = LOG.NumberStrToForeColor(CheckWakuban(intNum[0]).ToString());
                                Rows.Add(row);
                            }
                        }
                        break; //人気順ここまで
                }
            }//単勝ここまで
            else if (RecMainButtonType[0] == 2)
            {
                //枠連
                switch (RecMainButtonType[1])
                {
                    case 1:
                    case 2:
                        //通常と通常
                        param = new String[36, 3];
                        PayAllData.JvOzAllGetData(ref param, 3);

                        //必要なエリアを表示する
                        dataOdzz.Columns["tag1"].Visible = true;
                        dataOdzz.Columns["content2"].Visible = true;
                        if (RecMainButtonType[1] == 2)
                        {
                            // dataOdzz.Columns["RankNo"].Visible = true;
                            RankSort = true;
                        }

                        for (int i = 0; i < param.GetLength(0); i++)
                        {                           
                            //nullチェック
                            if (param[i, 0] == null)
                            {
                                break;
                            }

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataOdzz);
                            if (RankSort)
                            {
                                row.SetValues(new object[] { param[i, 2], param[i, 0].Substring(0, 1), "▶", param[i, 0].Substring(1, 1), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }
                            else
                            {
                                row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 1), "▶", param[i, 0].Substring(1, 1), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }

                            row.Cells[1].Style.BackColor = LOG.NumberStrToColor(param[i, 0].Substring(0, 1));
                            row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(param[i, 0].Substring(0, 1));
                            row.Cells[3].Style.BackColor = LOG.NumberStrToColor(param[i, 0].Substring(1, 1));
                            row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(param[i, 0].Substring(1, 1));
                            Rows.Add(row);
                        }

                        break;//枠連の通常ここまで
                    case 3:
                    case 4:
                    case 5:
                        break;
                }

            }
            else if (RecMainButtonType[0] == 3)
            {
                //馬連
                switch (RecMainButtonType[1])
                {
                    case 1:
                    case 2:
                        //通常と通常
                        param = new String[36, 3];
                        PayAllData.JvOzAllGetData(ref param, 4);

                        //必要なエリアを表示する
                        dataOdzz.Columns["tag1"].Visible = true;
                        dataOdzz.Columns["content2"].Visible = true;
                        if (RecMainButtonType[1] == 2)
                        {
                            // dataOdzz.Columns["RankNo"].Visible = true;
                            RankSort = true;
                        }

                        for (int i = 0; i < param.GetLength(0); i++)
                        {
                            //nullチェック
                            if(param[i,0] == null)
                            {
                                break;
                            }
                            
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataOdzz);

                            if (RankSort)
                            {
                                row.SetValues(new object[] { param[i, 2], param[i, 0].Substring(0, 2), "－", param[i, 0].Substring(2, 2), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }
                            else
                            {
                                row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 2), "－", param[i, 0].Substring(2, 2), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }

                            row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(0, 2)).ToString());
                            row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(0, 2).ToString()));
                            row.Cells[3].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(2, 2)).ToString());
                            row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(2, 2).ToString()));
                            Rows.Add(row);
                        }
                        break;
                }
            }
            else if (RecMainButtonType[0] == 4)
            {
                //馬単
                switch (RecMainButtonType[1])
                {
                    case 1:
                    case 2:
                        //通常と通常
                        param = new String[306, 3];
                        PayAllData.JvOzAllGetData(ref param, 5);

                        //必要なエリアを表示する
                        dataOdzz.Columns["tag1"].Visible = true;
                        dataOdzz.Columns["content2"].Visible = true;
                        if (RecMainButtonType[1] == 2)
                        {
                            // dataOdzz.Columns["RankNo"].Visible = true;
                            RankSort = true;
                        }

                        for (int i = 0; i < param.GetLength(0); i++)
                        {
                            //nullチェック
                            if (param[i, 0] == null)
                            {
                                break;
                            }

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataOdzz);
                            if (RankSort)
                            {
                                row.SetValues(new object[] { param[i, 2], param[i, 0].Substring(0, 2), "▶", param[i, 0].Substring(2, 2), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }
                            else
                            {
                                row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 2), "▶", param[i, 0].Substring(2, 2), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }

                            row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(0, 2)).ToString());
                            row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(0, 2).ToString()));
                            row.Cells[3].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(2, 2)).ToString());
                            row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(2, 2).ToString()));
                            Rows.Add(row);
                        }
                        break;
                }
            }
            else if (RecMainButtonType[0] == 5)
            {
                //3連複
                switch (RecMainButtonType[1])
                {
                    case 1:
                    case 2:
                        //通常と通常
                        param = new String[816, 3];
                        PayAllData.JvOzAllGetData(ref param, 7);

                        //必要なエリアを表示する
                        dataOdzz.Columns["tag1"].Visible = true;
                        dataOdzz.Columns["content2"].Visible = true;
                        dataOdzz.Columns["tag2"].Visible = true;
                        dataOdzz.Columns["content3"].Visible = true;
                        if (RecMainButtonType[1] == 2)
                        {
                            // dataOdzz.Columns["RankNo"].Visible = true;
                            RankSort = true;
                        }

                        for (int i = 0; i < param.GetLength(0); i++)
                        {
                            //nullチェック
                            if (param[i, 0] == null)
                            {
                                break;
                            }

                            if (!Int32.TryParse(param[i, 2], out RefParam) || RefParam > 100)
                            {
                                //100番人気いないじゃなければやらない：オッズが表示されすぎて遅くなる
                                continue;
                            }

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataOdzz);
                            if (RankSort)
                            {
                                row.SetValues(new object[] { param[i, 2], param[i, 0].Substring(0, 2), "－", param[i, 0].Substring(2, 2), "－", param[i, 0].Substring(4, 2), param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }
                            else
                            {
                                row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 2), "－", param[i, 0].Substring(2, 2), "－", param[i, 0].Substring(4, 2), param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }

                            row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(0, 2)).ToString());
                            row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(0, 2).ToString()));
                            row.Cells[3].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(2, 2)).ToString());
                            row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(2, 2).ToString()));
                            row.Cells[5].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(4, 2)).ToString());
                            row.Cells[5].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(4, 2).ToString()));
                            Rows.Add(row);
                        }
                        break;
                }
            }
            else if (RecMainButtonType[0] == 6)
            {
                //3連単
                switch (RecMainButtonType[1])
                {
                    case 1:
                    case 2:
                        //通常と通常
                        param = new String[4896, 3];
                        PayAllData.JvOzAllGetData(ref param, 8);

                        //必要なエリアを表示する
                        dataOdzz.Columns["tag1"].Visible = true;
                        dataOdzz.Columns["content2"].Visible = true;
                        dataOdzz.Columns["tag2"].Visible = true;
                        dataOdzz.Columns["content3"].Visible = true;
                        if (RecMainButtonType[1] == 2)
                        {
                            // dataOdzz.Columns["RankNo"].Visible = true;
                            RankSort = true;
                        }

                        for (int i = 0; i < param.GetLength(0); i++)
                        {
                            //nullチェック
                            if (param[i, 0] == null)
                            {
                                break;
                            }

                            if (!Int32.TryParse(param[i, 2], out RefParam) || RefParam > 100)
                            {
                                //100番人気いないじゃなければやらない：オッズが表示されすぎて遅くなる
                                continue;
                            }

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(dataOdzz);
                            if (RankSort)
                            {
                                row.SetValues(new object[] { param[i, 2], param[i, 0].Substring(0, 2), "▶", param[i, 0].Substring(2, 2), "▶", param[i, 0].Substring(4, 2), param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }
                            else
                            {
                                row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 2), "▶", param[i, 0].Substring(2, 2), "▶", param[i, 0].Substring(4, 2), param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                            }

                            row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(0, 2)).ToString());
                            row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(0, 2).ToString()));
                            row.Cells[3].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(2, 2)).ToString());
                            row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(2, 2).ToString()));
                            row.Cells[5].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(4, 2)).ToString());
                            row.Cells[5].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(4, 2).ToString()));
                            Rows.Add(row);
                        }
                        break;
                }
            }
            else
            {
                //例外
            }

            DataGridViewRow[] ViewArray;
            int ArrayIndex = 0;
            //最終書き込み
            ViewArray = new DataGridViewRow[Rows.Count];
            ArrayIndex = Rows.Count;

            for (int i = 0; i < ArrayIndex; i++) ViewArray[i] = Rows[i];
            //List型に持っているものを配列型に入れる

            //DatagridViewに書き込む
            dataOdzz.Rows.AddRange(ViewArray);

            //色を染める
            if(Wakuban)
            {
                //枠連なら、枠番(1→白)
                if(HorseData.Count >= 1)
                {
                }
            }
            else
            {
                //馬番なら

            }

            //人気順ならソートする。
            if(RankSort)
            {
                //並び替えを行う
                dataOdzz.Sort(dataOdzz.Columns[0], ListSortDirection.Ascending);
            }
        }

        private int CheckWakuban(String Umaban)
            { return CheckWakuban(Int32.Parse(Umaban)); }

        private int CheckWakuban(int Umaban)
        {
            if(HorseData.Count == 0)
            {
                //SEデータ未セットがここで検知する
                LOG.CONSOLE_MODULE(MD, "SE Data Undefined");
                LOG.ASSERT(MD);
                return 0;
            }

            for(int i= 0; i < HorseData.Count; i++)
            {
                var values = HorseData[i].Split(',');
                if (values[0] == Key + String.Format("{0:00}",Umaban))
                {
                    return Int32.Parse(values[5]);   //枠データ
                }
            }
            return 0;
        }

        private void ProcLayer3Common(int type)
        {
            RecMainButtonType[2] = type;
            //ここに入るのはながし

            //一旦全部をチェックボックスに設定する。
            ChengeAllDataGridMode(DG_CHECK_MODE, false);

            //チェック状態をクリアする
            CheckStat = new bool[18, 3];

            //チェックボックスからラジオボタンに変更する
            //軸指定で変える
            switch (type)
            {
                case 1:
                    //3連複：軸1頭ながし
                    //枠連・馬連：通常
                    //馬単：1着固定
                    //3連単：1着固定ながし
                    ChengeDataGridMode(DG_RADIO_MODE, false, 0);    //1頭目をラジオボタンに設定する。(非選択)
                    ChengeDataGridMode(DG_CHECK_MODE, false, 1);    //1頭目をチェックボックスに設定する。(非選択)
                    ChengeDataGridMode(DG_CHECK_MODE, false, 2);    //2頭目をチェックボックスに設定する。(非選択)
                    break;
                case 2:
                    //3連複：軸2頭ながし
                    if(RecMainButtonType[0] == 5)
                    {
                        //3連複の場合は1/2着を選択状態にするため、別処理にする
                        ChengeDataGridMode(DG_RADIO_MODE, false, 0);    //1頭目をラジオボタンに設定する。(非選択)
                        ChengeDataGridMode(DG_RADIO_MODE, false, 1);    //2頭目をラジオボタンに設定する。(非選択)
                        ChengeDataGridMode(DG_CHECK_MODE, false, 2);    //3頭目をチェックボックスに設定する。(非選択)
                        break;
                    }
                    //枠連・馬連：なし
                    //馬単：2着固定
                    //3連単：2着固定
                    ChengeDataGridMode(DG_CHECK_MODE, false, 0);    //1頭目をチェックボックスに設定する。(非選択)
                    ChengeDataGridMode(DG_RADIO_MODE, false, 1);    //2頭目をラジオボタンに設定する。(非選択)
                    ChengeDataGridMode(DG_CHECK_MODE, false, 2);    //3頭目はチェックボックスに設定する。(非選択)
                    break;
                case 3:
                    //3連単：3着固定
                    ChengeDataGridMode(DG_CHECK_MODE, false, 0);    //1頭目をチェックボックスに設定する。(非選択)
                    ChengeDataGridMode(DG_CHECK_MODE, false, 1);    //2頭目をチェックボックスに設定する。(非選択)
                    ChengeDataGridMode(DG_RADIO_MODE, false, 2);    //3頭目をラジオボタンに設定する。(非選択)
                    break;
                case 4:
                    //3連単：1/2着固定
                    ChengeDataGridMode(DG_RADIO_MODE, false, 0);    //1頭目をラジオボタンに設定する。(非選択)
                    ChengeDataGridMode(DG_RADIO_MODE, false, 1);    //2頭目をラジオボタンに設定する。(非選択)
                    ChengeDataGridMode(DG_CHECK_MODE, false, 2);    //3頭目をチェックボックスに設定する。(非選択)
                    break;
                case 5:
                    //3連単：1/3着固定
                    ChengeDataGridMode(DG_RADIO_MODE, false, 0);    //1頭目をラジオボタンに設定する。(非選択)
                    ChengeDataGridMode(DG_RADIO_MODE, false, 2);    //3頭目をラジオボタンに設定する。(非選択)
                    ChengeDataGridMode(DG_CHECK_MODE, false, 1);    //2頭目をチェックボックスに設定する。(非選択)
                    break;
                case 6:
                    //3連単：2/3着固定
                    ChengeDataGridMode(DG_CHECK_MODE, false, 0);    //1頭目をチェックボックスに設定する。(非選択)
                    ChengeDataGridMode(DG_RADIO_MODE, false, 1);    //2頭目をラジオボタンに設定する。
                    ChengeDataGridMode(DG_RADIO_MODE, false, 2);    //3頭目をラジオボタンに設定する。
                    break;
                default:
                    break;
            }

            FormationOddzData();
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            

        }

        //セルがクリックされたときに呼ばれるイベントハンドラ
        bool[,] CheckStat = new bool[18, 3];
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Console.WriteLine(e.RowIndex + "," + e.ColumnIndex);

            //画像チェックボックスを選択したときに動作させる
            if (e.ColumnIndex >= 3 && e.ColumnIndex <= 5 && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                bool CheckModeFlg = DataGridSelectMode[e.ColumnIndex-3] == DG_CHECK_MODE ? true : false;

                if (!CheckModeFlg)
                {
                    //ラジオボタンの場合、１列１つしか選択できないようにする
                    //そのため、すでに選択されているものを一旦クリアする
                    for(int i=0; i < CheckStat.GetLength(0); i++)
                    {
                        CheckStat[i, e.ColumnIndex - 3] = false;
                    }

                    //チェックをすべてDISABLEにする
                    ChengeDataGridMode(DG_RADIO_MODE, false, e.ColumnIndex - 3);
                }


                //1頭目～3頭目すべてを共通処理とする。DataGridViewに列を追加した場合、修正が必要
                if (CheckStat[e.RowIndex, e.ColumnIndex - 3] == false)
                {
                    Bitmap BitMapData = CheckModeFlg ? new Bitmap(DEF.FILE_OPEN("PCX_FILE_ENABLE")) : new Bitmap(DEF.FILE_OPEN("PCX_FILE_RADIO_ENABLE")); ;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BitMapData;

                }
                else
                {
                    Bitmap BitMapData = CheckModeFlg ? new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE")) : new Bitmap(DEF.FILE_OPEN("PCX_FILE_RADIO_DISABLE")); ;
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = BitMapData;
                }

                //チェックボックスなのかラジオボタンなのかで単一式かどうかを切り替える
                CheckStat[e.RowIndex, e.ColumnIndex - 3] = !CheckStat[e.RowIndex, e.ColumnIndex - 3];



                //チェックリストを更新する。
                JudgeFormationFlg();

                //オッズエリアを反映させる
                FormationOddzData();

            }

        }

        private bool CheckSelectRefresh()
        {
            //フォーメーションで更新をするかチェック
            //trueの場合、dataoddzエリアを更新する
            if (!(RecMainButtonType[1] == 2 || RecMainButtonType[1] == 3 || RecMainButtonType[1] == 4))
            {
                return false;
            }

            return true;
        }
        
        //フォーメーション
        private void FormationOddzData()
        {
            //データ表示
            List<DataGridViewRow> Rows = new List<DataGridViewRow>();

            //人気順に表示するオプションが入った場合
            bool RankSort = RankSortOption.Checked;

            if(!(RecMainButtonType[1] == 3) && !(RecMainButtonType[1] == 5)) 
            {
                return; //なにもしない
            }

            //共通のインスタンス
            String[,] param;

            //ここも共通で
            //dataOdzzエリアが非表示であれば表示する
            dataOdzz.Visible = true;

            //1頭目・オッズ以外は一旦全部非表示に
            dataOdzz.Columns["Fukusho"].Visible = false;
            dataOdzz.Columns["tag1"].Visible = false;
            dataOdzz.Columns["content2"].Visible = false;
            dataOdzz.Columns["tag2"].Visible = false;
            dataOdzz.Columns["content3"].Visible = false;
            dataOdzz.Columns["RankNo"].Visible = false;

            if (RecMainButtonType[1] == 2)
            {
                // dataOdzz.Columns["RankNo"].Visible = true;
                RankSort = true;
            }

            if((!Check2) && Check3)
            {
                //2頭目が選択されず、3頭目にチェックが入った場合は処理できない
                return;
            }

            //組み合わせ・組番を作る
            List<String> Kumniban = new List<string>();
            if(!PickUpKumiban(ref Kumniban))
            {
                //組番取得に失敗
                LOG.CONSOLE_MODULE(MD, "PickUpError!");
            }

            //既存データをクリア
            dataOdzz.Rows.Clear();


            //選択されている馬券種類が正しいかを確認する。
            //ただし、この処理を呼ぶ場合には正しいかどうかを呼び出し元で確認する。
            //3連単で第3階層をチェックしないまま呼ばないこと！
            switch (RecMainButtonType[0])
            {
                //馬券種類チェック
                case 1://単勝
                case 7://応援馬券
                    //なにもしない？どうする？
                    break;
                case 2://枠連
                    if(!(Check1 && Check2))
                    {
                        //枠連：2頭選択されていない場合はなにもしない
                        return;
                    }

                    param = new string[36, 3];
                    PayAllData.JvOzAllGetData(ref param, 3);

                    //必要なエリアを表示する
                    dataOdzz.Columns["tag1"].Visible = true;
                    dataOdzz.Columns["content2"].Visible = true;

                    for (int i = 0; i < param.GetLength(0); i++)
                    {
                        //nullチェック
                        if (param[i, 0] == null)
                        {
                            break;
                        }

                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(dataOdzz);
                        if (RankSort)
                        {
                            row.SetValues(new object[] { param[i, 2], param[i, 0].Substring(0, 1), "▶", param[i, 0].Substring(1, 1), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                        }
                        else
                        {
                            row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 1), "▶", param[i, 0].Substring(1, 1), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                        }

                        row.Cells[1].Style.BackColor = LOG.NumberStrToColor(param[i, 0].Substring(0, 1));
                        row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(param[i, 0].Substring(0, 1));
                        row.Cells[3].Style.BackColor = LOG.NumberStrToColor(param[i, 0].Substring(1, 1));
                        row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(param[i, 0].Substring(1, 1));
                       // Rows.Add(row);
                    }

                    break;
                case 3://馬連
                    param = new string[153, 3];
                    PayAllData.JvOzAllGetData(ref param, 4);

                    //必要なエリアを表示する
                    dataOdzz.Columns["tag1"].Visible = true;
                    dataOdzz.Columns["content2"].Visible = true;

                    //組番情報から連単系・同じ組番を削除する
                    Kumniban = FuncAscendingForKumiban(Kumniban);

                    for (int i = 0; i < param.GetLength(0); i++)
                    {
                        //nullチェック
                        if (param[i, 0] == null)
                        {
                            break;
                        }

                        //選択している馬番のみ処理する
                        for(int i2 = 0; i2 < Kumniban.Count; i2++)
                        {
                            if(Kumniban[i2] == param[i, 0])
                            {
                                //合致した組番の場合は表示処理を行う
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataOdzz);
                                if (RankSort)
                                {
                                    row.SetValues(new object[] { Int32.Parse(param[i, 2]), param[i, 0].Substring(0, 2), "－", param[i, 0].Substring(2, 2), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                                }
                                else
                                {
                                    row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 2), "－", param[i, 0].Substring(2, 2), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                                }

                                row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(0, 2)).ToString());
                                row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(0, 2).ToString()));
                                row.Cells[3].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(2, 2)).ToString());
                                row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(2, 2).ToString()));
                                Rows.Add(row);
                            }
                        }
                    }
                    break;
                case 4://馬単
                    param = new string[386, 3];
                    PayAllData.JvOzAllGetData(ref param, 5);

                    //必要なエリアを表示する
                    dataOdzz.Columns["tag1"].Visible = true;
                    dataOdzz.Columns["content2"].Visible = true;


                    for (int i = 0; i < param.GetLength(0); i++)
                    {
                        //nullチェック
                        if (param[i, 0] == null)
                        {
                            break;
                        }

                        //選択している馬番のみ処理する
                        for (int i2 = 0; i2 < Kumniban.Count; i2++)
                        {
                            if (Kumniban[i2] == param[i, 0])
                            {
                                //合致した組番の場合は表示処理を行う
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataOdzz);
                                if (RankSort)
                                {
                                    row.SetValues(new object[] { Int32.Parse(param[i, 2]), param[i, 0].Substring(0, 2), "▶", param[i, 0].Substring(2, 2), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                                }
                                else
                                {
                                    row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 2), "▶", param[i, 0].Substring(2, 2), "", "", param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                                }

                                row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(0, 2)).ToString());
                                row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(0, 2).ToString()));
                                row.Cells[3].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(2, 2)).ToString());
                                row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(2, 2).ToString()));
                                Rows.Add(row);
                            }
                        }
                    }
                    break;
                case 5://3連複
                    param = new String[816, 3];
                    PayAllData.JvOzAllGetData(ref param, 7);

                    //必要なエリアを表示する
                    dataOdzz.Columns["tag1"].Visible = true;
                    dataOdzz.Columns["content2"].Visible = true;
                    dataOdzz.Columns["tag2"].Visible = true;
                    dataOdzz.Columns["content3"].Visible = true;

                    //組番情報から連単系・同じ組番を削除する
                    Kumniban = FuncAscendingForKumiban(Kumniban);

                    for (int i = 0; i < param.GetLength(0); i++)
                    {
                        //nullチェック
                        if (param[i, 0] == null)
                        {
                            break;
                        }

                        for (int i2 = 0; i2 < Kumniban.Count; i2++)
                        {
                            if (Kumniban[i2] == param[i, 0])
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataOdzz);
                                if (RankSort)
                                {
                                    row.SetValues(new object[] { Int32.Parse(param[i, 2]), param[i, 0].Substring(0, 2), "－", param[i, 0].Substring(2, 2), "－", param[i, 0].Substring(4, 2), param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                                }
                                else
                                {
                                    row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 2), "－", param[i, 0].Substring(2, 2), "－", param[i, 0].Substring(4, 2), param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                                }

                                row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(0, 2)).ToString());
                                row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(0, 2).ToString()));
                                row.Cells[3].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(2, 2)).ToString());
                                row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(2, 2).ToString()));
                                row.Cells[5].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(4, 2)).ToString());
                                row.Cells[5].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(4, 2).ToString()));
                                Rows.Add(row);
                            }
                        }
                    }
                    break;
                case 6://3連複
                    param = new String[4896, 3];
                    PayAllData.JvOzAllGetData(ref param, 8);

                    //必要なエリアを表示する
                    dataOdzz.Columns["tag1"].Visible = true;
                    dataOdzz.Columns["content2"].Visible = true;
                    dataOdzz.Columns["tag2"].Visible = true;
                    dataOdzz.Columns["content3"].Visible = true;

                    for (int i = 0; i < param.GetLength(0); i++)
                    {
                        //nullチェック
                        if (param[i, 0] == null)
                        {
                            break;
                        }

                        for (int i2 = 0; i2 < Kumniban.Count; i2++)
                        {
                            if (Kumniban[i2] == param[i, 0])
                            {
                                DataGridViewRow row = new DataGridViewRow();
                                row.CreateCells(dataOdzz);
                                if (RankSort)
                                {
                                    row.SetValues(new object[] { Int32.Parse(param[i, 2]), param[i, 0].Substring(0, 2), "▶", param[i, 0].Substring(2, 2), "▶", param[i, 0].Substring(4, 2), param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                                }
                                else
                                {
                                    row.SetValues(new object[] { i + 1, param[i, 0].Substring(0, 2), "▶", param[i, 0].Substring(2, 2), "▶", param[i, 0].Substring(4, 2), param[i, 2], LOG.OddzStrToString(param[i, 1].ToString()), "" });
                                }

                                row.Cells[1].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(0, 2)).ToString());
                                row.Cells[1].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(0, 2).ToString()));
                                row.Cells[3].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(2, 2)).ToString());
                                row.Cells[3].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(2, 2).ToString()));
                                row.Cells[5].Style.BackColor = LOG.NumberStrToColor(CheckWakuban(param[i, 0].Substring(4, 2)).ToString());
                                row.Cells[5].Style.ForeColor = LOG.NumberToForeColor(CheckWakuban(param[i, 0].Substring(4, 2).ToString()));
                                Rows.Add(row);
                            }
                        }
                    }
                    break;
            }


            DataGridViewRow[] ViewArray;
            int ArrayIndex = 0;
            //最終書き込み
            ViewArray = new DataGridViewRow[Rows.Count];
            ArrayIndex = Rows.Count;

            for (int i = 0; i < ArrayIndex; i++) ViewArray[i] = Rows[i];
            //List型に持っているものを配列型に入れる

            //DatagridViewに書き込む
            dataOdzz.Rows.AddRange(ViewArray);

            //人気順ならソートする。
            if (RankSort)
            {
                //並び替えを行う
                dataOdzz.Sort(dataOdzz.Columns[0], ListSortDirection.Ascending);
            }

        }

        //チェックボックスの選択を複連系に変換(030201を010203や、010101を削除する)
        //連複馬券のときのみコールすること！(枠連の考慮はない)
        private List<String> FuncAscendingForKumiban(List<String> inParam)
        {
            int tmpNumber = 0;

            int param1 = 0;
            int param2 = 0;
            int param3 = 0;

            List<String> ret = new List<string>();
            List<String> ret2 = new List<string>();

            for (int i=0; i < inParam.Count; i++)
            {
                if(Int32.TryParse(inParam[i], out tmpNumber) && inParam[i].Length == 4 )
                {
                    //数値に変換できること、かつ、文字数が4(2連系、3連系)が条件
                    param1 = Int32.Parse(inParam[i].Substring(0, 2));
                    param2 = Int32.Parse(inParam[i].Substring(2, 2));

                    if(param1 == param2)
                    {
                        //連複系馬券の対象ではないため、スキップ
                        continue;
                    }

                    //逆転
                    if(param1 >= param2)
                    {
                        tmpNumber = param1;
                        param1 = param2;
                        param2 = tmpNumber;
                    }

                    //問題なしと判断されたため、addする
                    ret.Add(ZeroString(param1) + ZeroString(param2));
                }
                else if(Int32.TryParse(inParam[i], out tmpNumber) && inParam[i].Length == 6)
                {
                    param1 = Int32.Parse(inParam[i].Substring(0, 2));
                    param2 = Int32.Parse(inParam[i].Substring(2, 2));
                    param3 = Int32.Parse(inParam[i].Substring(4, 2));

                    if ( (param1 == param2 || param1 == param3 || param2 == param3) )
                    {
                        //連複系馬券の対象ではないため、スキップ
                        continue;
                    }

                    //逆転
                    if (param2 >= param3)
                    {
                        tmpNumber = param2;
                        param2 = param3;
                        param3 = tmpNumber;
                    }

                    if (param1 >= param2)
                    {
                        tmpNumber = param1;
                        param1 = param2;
                        param2 = tmpNumber;
                    }

                    //問題なしと判断されたため、addする
                    ret.Add(ZeroString(param1) + ZeroString(param2) + ZeroString(param3));
                }
            }
            
            //重複を削除する(linq関数)
            var values = ret.Distinct();
            //一旦retを削除
            //ret.Clear();

            foreach(var item in values)
            {
                ret2.Add(item);
            }


            return ret2;
        }

        private void JudgeFormationFlg()
        {

            for (int j = 0; j < CheckStat.GetLength(0); j++)
            {
                //チェックボックスが選択できない状態であればtrueにしない
                if (CheckStat[j, 0])
                {
                    Check1 = (dataGridView1.Columns["Select1"].Visible ? true : false);
                }

                if (CheckStat[j, 1])
                {
                    Check2 = (dataGridView1.Columns["Select2"].Visible ? true : false);
                }

                if (CheckStat[j, 2])
                {
                    Check3 = (dataGridView1.Columns["Select3"].Visible ? true : false);
                }
            }
            
        }

        private bool PickUpKumiban(ref List<String> retParam)
        {
            List<String> ret = new List<string>();

            List<String> Param1 = new List<string>();
            List<String> Param2 = new List<string>();
            List<String> Param3 = new List<string>();

            int tmpCount = 0;

            bool Check3Flg = false;

            for (int i = 0; i < RA.Tosu1; i++)
            {
                if (!CheckStat[i, 0])
                {
                    //1頭目の選択は必須
                    continue;
                }

                for (int i2 = 0; i2 < RA.Tosu1; i2++)
                {
                    if (CheckStat[i2, 1])
                    {
                        //3頭目があればさらに入れる
                        if(Check3)
                        {
                            for (int i3 = 0; i3 < RA.Tosu1; i3++)
                            {
                                if (CheckStat[i3, 2])
                                {
                                    Check3Flg = true;
                                    ret.Add(ZeroString(i + 1) + ZeroString(i2 + 1) + ZeroString(i3 + 1));
                                }
                            }
                        }

                        if(!Check3Flg)
                        {
                            ret.Add(ZeroString(i + 1) + ZeroString(i2 + 1));
                        }
                    }
                }
            }
            retParam = ret;
            return ret.Count >= 1 ? true : false ;
        }

        //チェックボックスの表示・非表示切り替え
        private void DesignChangeHourceSelectArea(int param)
        {
            switch (param)
            {
                case 0:
                    //全エリア非表示
                    dataGridView1.Columns["Select1"].Visible = false;
                    dataGridView1.Columns["Select2"].Visible = false;
                    dataGridView1.Columns["Select3"].Visible = false;
                    //チェックボックス表示状態を変更する
                    DataGridSelectMode[0] = DG_NON_SELECT;
                    DataGridSelectMode[1] = DG_NON_SELECT;
                    DataGridSelectMode[2] = DG_NON_SELECT;
                    //すべて選択も連動して、表示非表示切り替え
                    All1.Enabled = true;
                    All2.Enabled = false;
                    All3.Enabled = false;
                    Clear1.Enabled = true;
                    Clear2.Enabled = false;
                    Clear3.Enabled = false;
                    break;
                case 1:
                    //2/3頭目選択エリアを非表示にする。
                    dataGridView1.Columns["Select1"].Visible = true;
                    dataGridView1.Columns["Select2"].Visible = false;
                    dataGridView1.Columns["Select3"].Visible = false;
                    //チェックボックス表示状態を変更する
                    DataGridSelectMode[0] = DG_CHECK_MODE;
                    DataGridSelectMode[1] = DG_NON_SELECT;
                    DataGridSelectMode[2] = DG_NON_SELECT;
                    //すべて選択も連動して、表示非表示切り替え
                    All1.Enabled = true;
                    All2.Enabled = false;
                    All3.Enabled = false;
                    Clear1.Enabled = true;
                    Clear2.Enabled = false;
                    Clear3.Enabled = false;
                    break;
                case 2:
                    //3頭目選択エリアを非表示にする。
                    dataGridView1.Columns["Select1"].Visible = true;
                    dataGridView1.Columns["Select2"].Visible = true;
                    dataGridView1.Columns["Select3"].Visible = false;
                    //チェックボックス表示状態を変更する
                    DataGridSelectMode[0] = DG_CHECK_MODE;
                    DataGridSelectMode[1] = DG_CHECK_MODE;
                    DataGridSelectMode[2] = DG_NON_SELECT;
                    //すべて選択も連動して、表示非表示切り替え
                    All1.Enabled = true;
                    All2.Enabled = true;
                    All3.Enabled = false;
                    Clear1.Enabled = true;
                    Clear2.Enabled = true;
                    Clear3.Enabled = false;
                    break;
                case 3:
                    //全エリア表示にする(3連系)
                    dataGridView1.Columns["Select1"].Visible = true;
                    dataGridView1.Columns["Select2"].Visible = true;
                    dataGridView1.Columns["Select3"].Visible = true;
                    //チェックボックス表示状態を変更する
                    DataGridSelectMode[0] = DG_CHECK_MODE;
                    DataGridSelectMode[1] = DG_CHECK_MODE;
                    DataGridSelectMode[2] = DG_CHECK_MODE;
                    //すべて選択も連動して、表示非表示切り替え
                    All1.Enabled = true;
                    All2.Enabled = true;
                    All3.Enabled = true;
                    Clear1.Enabled = true;
                    Clear2.Enabled = true;
                    Clear3.Enabled = true
                        ;
                    break;
                default:
                    break;
            }
        }

        //datagridをチェックボックス←→ラジオボックスを切り替える(表示・非表示は切り替えない)
        private void ChengeAllDataGridMode(int mode, bool OutPutmode)
        {
            Bitmap Map;

            switch (mode)
            {
                case DG_CHECK_MODE:
                    Map = OutPutmode ? new Bitmap(DEF.FILE_OPEN("PCX_FILE_ENABLE")) : new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE"));
                    break;
                case DG_RADIO_MODE:
                    Map = OutPutmode ? new Bitmap(DEF.FILE_OPEN("PCX_FILE_RADIO_ENABLE")) : new Bitmap(DEF.FILE_OPEN("PCX_FILE_RADIO_DISABLE"));
                    break;
                case DG_NON_SELECT:
                    //非表示にする
                    dataGridView1.Columns["Select1"].Visible = false;
                    dataGridView1.Columns["Select2"].Visible = false;
                    dataGridView1.Columns["Select3"].Visible = false;
                    DataGridSelectMode[0] = mode;
                    DataGridSelectMode[1] = mode;
                    DataGridSelectMode[2] = mode;
                    return;
                default:
                    //なにもしない
                    return;
            }

            for (int i=0; i<dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Select1"].Value = Map;
                dataGridView1.Rows[i].Cells["Select2"].Value = Map;
                dataGridView1.Rows[i].Cells["Select3"].Value = Map;
            }
        }

        //こちらは１列だけ指定する(表示・非表示は切り替えない) ComsNumは0＝1頭目・・・2=３頭目
        private void ChengeDataGridMode(int mode, bool CheckStat, int ComsNum)
        {
            Bitmap Map;
            String CellsName = "";

            if(ComsNum > 2)
            {
                return;
            }

            switch (mode)
            {
                case DG_CHECK_MODE:
                    Map = CheckStat ? new Bitmap(DEF.FILE_OPEN("PCX_FILE_ENABLE")) : new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE"));
                    break;
                case DG_RADIO_MODE:
                    Map = CheckStat ? new Bitmap(DEF.FILE_OPEN("PCX_FILE_RADIO_ENABLE")) : new Bitmap(DEF.FILE_OPEN("PCX_FILE_RADIO_DISABLE"));
                    break;
                case DG_NON_SELECT:
                default:
                    //なにもしない
                    return;
            }

            DataGridSelectMode[ComsNum] = mode;

            switch (ComsNum)
            {
                case 0:
                    CellsName = "Select1";
                    break;
                case 1:
                    CellsName = "Select2";
                    break;
                case 2:
                    CellsName = "Select3";
                    break;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[CellsName].Value = Map;
            }
        }

        private String ZeroString(int param)
        {
            return String.Format("{0:00}", param);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //単・複のボタンを押下したときのイベント
            ProcCommonButtonMenus1(1);
            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ProcCommonButtonMenus1(6);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProcCommonButtonMenus1(2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProcCommonButtonMenus1(3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProcCommonButtonMenus1(4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ProcCommonButtonMenus1(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ProcCommonButtonMenus1(5);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ProcCommonButtonMenus1(7);
        }

        private void Sub1_Click(object sender, EventArgs e)
        {
            ProcCommonSubButtonMenus1(1);
        }

        private void Sub2_Click(object sender, EventArgs e)
        {
            ProcCommonSubButtonMenus1(2);

        }

        private void Sub3_Click(object sender, EventArgs e)
        {
            ProcCommonSubButtonMenus1(3);
            JudgeFormationFlg();
        }

        private void Sub4_Click(object sender, EventArgs e)
        {
            ProcCommonSubButtonMenus1(4);
        }

        private void Sub5_Click(object sender, EventArgs e)
        {
            ProcCommonSubButtonMenus1(5);
        }

        private void l3Button1_Click(object sender, EventArgs e)
        {
            ProcLayer3Common(1);
        }

        private void l3Button2_Click(object sender, EventArgs e)
        {
            ProcLayer3Common(2);
        }

        private void l3Button3_Click(object sender, EventArgs e)
        {
            ProcLayer3Common(3);
        }

        private void l3Button4_Click(object sender, EventArgs e)
        {
            ProcLayer3Common(4);
        }

        private void l3Button5_Click(object sender, EventArgs e)
        {
            ProcLayer3Common(5);
        }

        private void l3Button6_Click(object sender, EventArgs e)
        {
            ProcLayer3Common(6);
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            //オッズ更新する
            RefreshOdzzData();
        }

        private void RefreshOdzzData()
        {
            form.odds.O1_Form O1 = new odds.O1_Form();
            //オッズ更新！
            LOG.CONSOLE_TIME_MD(MD, ">>>オッズ更新ボタン");

            //プログレスバーを表示する。
            toolStripStatusLabel1.Text = DEF.FILE_OPEN("STR_ODZZ_NOW") + "...";
            this.Refresh();

            O1.GetAllOddzData(long.Parse(Key));
            PayAllData.JvDbAllSetALLOddzData();

            String[,] O1Ozzu = new string[18, 5];
            PayAllData.JvOzAllGetData(ref O1Ozzu, 1);

            //並び替える列を決める
            DataGridViewColumn sortColumn = dataGridView1.CurrentCell.OwningColumn;

            //並び替えの方向（昇順か降順か）を決める
            ListSortDirection sortDirection = ListSortDirection.Ascending;
            if (dataGridView1.SortedColumn != null &&
                dataGridView1.SortedColumn.Equals(sortColumn))
            {
                sortDirection = ListSortDirection.Ascending;
            }

            //並び替えを行う
            dataGridView1.Sort(sortColumn, sortDirection);

            //並び替えたあとに更新オッズを入れる
            for(int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells[2].Value = LOG.OddzStrToString(O1Ozzu[i, 1]);
            }
            toolStripStatusLabel1.Text = DEF.FILE_OPEN("STR_STANDBY");

            //オッズ更新時間を更新する。
            SetOddzData();

            //表示中のオッズも更新させる

                       
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void RankSortOption_CheckedChanged(object sender, EventArgs e)
        {
            //チェックボッククに変更が入った場合のイベントハンドラー
            //データがあればソートする
            if(dataOdzz.Rows.Count == 0)
            {
                return;
            }


            if(RankSortOption.Checked)
            {
                //人気順にチェックが入った場合、オッズ昇順にする。
                dataOdzz.Sort(dataOdzz.Columns["Oddz"], ListSortDirection.Ascending);
            }
            else
            {
                //人気順にチェックが外れた場合、No昇順にする
                dataOdzz.Sort(dataOdzz.Columns["Number"], ListSortDirection.Ascending);
            }

        }

        //1全
        private void button17_Click(object sender, EventArgs e)
        {
            for(int i=0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Select1"].Value = new Bitmap(DEF.FILE_OPEN("PCX_FILE_ENABLE"));
                CheckStat[i, 0] = true;
            }

            //チェックリストを更新する。
            JudgeFormationFlg();

            //オッズエリアを反映させる
            FormationOddzData();
        }

        //1C
        private void button20_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Select1"].Value = new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE"));
                CheckStat[i, 0] = false;
            }
                    
            //チェックリストを更新する。
            JudgeFormationFlg();

            //オッズエリアを反映させる
            FormationOddzData();
        }

        //2全
        private void button18_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Select2"].Value = new Bitmap(DEF.FILE_OPEN("PCX_FILE_ENABLE"));
                CheckStat[i, 1] = true;
            }

            //チェックリストを更新する。
            JudgeFormationFlg();

            //オッズエリアを反映させる
            FormationOddzData();
        }

        //2全
        private void button19_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Select3"].Value = new Bitmap(DEF.FILE_OPEN("PCX_FILE_ENABLE"));
                CheckStat[i, 2] = true;
            }

            //チェックリストを更新する。
            JudgeFormationFlg();

            //オッズエリアを反映させる
            FormationOddzData();
        }

        //2C
        private void button21_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Select2"].Value = new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE"));
                CheckStat[i, 1] = false;
            }

            //チェックリストを更新する。
            JudgeFormationFlg();

            //オッズエリアを反映させる
            FormationOddzData();
        }

        //3C
        private void button22_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Select3"].Value = new Bitmap(DEF.FILE_OPEN("PCX_FILE_DISABLE"));
                CheckStat[i, 2] = false;
            }

            //チェックリストを更新する。
            JudgeFormationFlg();

            //オッズエリアを反映させる
            FormationOddzData();
        }
    }
}
