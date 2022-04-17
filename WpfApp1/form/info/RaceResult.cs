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

namespace WpfApp1.form.info
{
    public partial class RaceResult : Form
    {
        /* *********************** */
        private String Key;
        private JvComDbData.JvDbRaData RaData;
        JvComClass LOG = new JvComClass();
        List<JvComDbData.JvDbSEData> AllSeData = new List<JvComDbData.JvDbSEData>();
        dbAccess.dbConnect db = new dbAccess.dbConnect();
        int color;
        /* *********************** */

        private const String SPEC = "RS";
        private String RaceStatus = ""; /* レース確定情報 */

        public RaceResult()
        {
            InitializeComponent();
        }

        public RaceResult(String RaKey)
        {
            InitializeComponent();
            this.Key = RaKey;
            RaData = new JvComDbData.JvDbRaData();

        }

        //場別カラー対応
        public RaceResult(String RaKey, int Color)
        {
            InitializeComponent();
            this.Key = RaKey;
            this.color = Color;
            RaData = new JvComDbData.JvDbRaData();

            switch (color)
            {
                case 1:
                    flowLayoutPanel4.BackColor = ColorTranslator.FromHtml("#FF0000FF");
                    panel1.BackColor = ColorTranslator.FromHtml("#FF0000FF");
                    break;
                case 2:
                    flowLayoutPanel4.BackColor = ColorTranslator.FromHtml("#FF006400");
                    panel1.BackColor = ColorTranslator.FromHtml("#FF006400");
                    break;
                case 3:
                    flowLayoutPanel4.BackColor = ColorTranslator.FromHtml("#FF800080");
                    panel1.BackColor = ColorTranslator.FromHtml("#FF800080");
                    break;
                default:
                    break;
            }

        }

        private void RaceResult_Load(object sender, EventArgs e)
        {
            SetData(); 
            InitFormLabels();
            WriteRaSeData();
            SetPayOutInfo();
        }

        public int SetData()
        {
            
            //インプットチェック
            if (Key == "" || RaData == null)
            {
                LOG.CONSOLE_TIME_MD("RESULT", "RaceResult InputParam Error!!");
                //MessageBox.Show("レース確定情報がありませんでした。");
                this.Close();
                return -1; //パタメータエラー
            }
            String tmp = "";

            //データ取得
            //RaData.RaGetRTRaData(this.Key, ref tmp);
            int ret = GetRaceInfo(this.Key);
            List<String> RaceResult = new List<string>();

            //SEデータの取得（確定情報）
            if(ret == 1)
            {
                //RAデータ
                if(db.TextReader_Col(this.Key, SPEC, 0, ref RaceResult, this.Key) != 0)
                {
                    //RAデータをグローバル変数に保存
                    RaData.setData(ref RaceResult);

                }
                else
                {
                    //データが取得できない
                    return 0;
                }

                RaceResult.Clear();
                JvComDbData.JvDbSEData tmpSeData;

                //SEデータ
                for (int idx = 1; idx <= 18; idx++)
                {
                    tmpSeData = new JvComDbData.JvDbSEData();
                    if (db.TextReader_Col(this.Key, SPEC, 0, ref RaceResult, this.Key + String.Format("{0:00}", idx) ) != 0)
                    {
                        //SEデータをグローバル変数に追加
                        tmpSeData.SetSEData(RaceResult);
                        AllSeData.Add(tmpSeData);
                    }
                    else
                    {
                        //データが取得できない。全頭未確定時は３頭・５頭のみしかデータがない可能性あり。
                        continue;
                    }
                }

                ret = ExecRaData();

                if (ret != 1)
                {
                    return 0;   //SEデータなし：確定データなし
                }
                else
                {

                }
            }
            else
            {
                //確定なし
                return 0;
                
            }


            

            return 1;
        }

        #region 確定成績書き出し
        public void WriteRaSeData()
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> retArray = new List<string>();
            JvComDbData.JvDbSEData SE = new JvComDbData.JvDbSEData();
            int DataKubun = 0;

            /* 確定成績 */
            if (Int32.TryParse(RaData.DataKubun1, out DataKubun))
            {
                if(DataKubun == 1 || DataKubun == 2)
                {
                    textBox1.Text = RaData.JvRaDataKubun();
                    return;
                }
                else if (DataKubun <= 4 || DataKubun == 9)
                {
                    textBox1.Text = RaData.JvRaDataKubun();
                }
                else
                {
                    textBox1.Visible = false;
                    label3.Visible = false;
                }
            }
            else
            {
                textBox1.Visible = false;
                label3.Visible = false;
                //払戻パネル追加
                flowLayoutPanel1.Visible = true;
            }


            /* フォントの変更 */
            dataGridView1.DefaultCellStyle.Font = new Font("Meiryo UI", 12);

            if (AllSeData.Count == 0)
            {
                return;
            }

            List<String> DM = new List<string>();
            List<String> TM = new List<string>();
            db.DbReadAllData(this.Key, "DM", 0, ref DM, this.Key, 0);
            db.DbReadAllData(this.Key, "TM", 0, ref TM, this.Key, 0);

            String[] DmValue = new string[5];
            int DmValue2 = 0;

            String[] TmValue1 = new string[5];
            int TmValue2 = 0;

            for (int i = 1; i <= 18; i++)
            {
                retArray.Clear();
                // if (db.TextReader_Col(this.Key, SPEC, 18, ref retArray, String.Format("{0:00}", i)) != 0) →　同着がひろえない
                if (db.TextReader_Col(this.Key, SPEC, 0, ref retArray, this.Key + String.Format("{0:00}", i)) != 0)
                {
                    if (retArray.Count == 0)
                    {
                        break;
                    }

                    SE.SetSEData(retArray);

                    String TimeDiff = "";
                    String NinkiRank = "";
                    String Rank = "";
                    String Ninki = "";
                    String Minarai = "";

                    String tmp = "";


                    if (SE.Rank1 == 1)
                    {
                        TimeDiff = (RaData.getRecordFlag() ? "R " : "") + RaData.ConvertRunTimeToString(SE.Time1);
                        Rank = String.Format("{0:00}", SE.Rank1);
                    }
                    else if (SE.Rank1 == 0)
                    {
                        tmp = SE.TorikeshiCd1.ToString();
                        TimeDiff = LOG.JvSysMappingFunction(2012, ref tmp);
                        if (SE.TorikeshiCd1 == "1") { Rank = "取消"; }
                        else if (SE.TorikeshiCd1 == "2" || SE.TorikeshiCd1 == "3") { Rank = "除外"; }
                        else if (SE.TorikeshiCd1 == "4") { Rank = "中止"; }
                    }
                    else
                    {
                        tmp = SE.Chakusa1;
                        TimeDiff = LOG.JvSysMappingFunction(2102, ref tmp);
                        Rank = String.Format("{0:00}", SE.Rank1);
                    }

                    try
                    {
                        NinkiRank = Int32.Parse(SE.Ninki1).ToString();

                    }
                    catch (Exception)
                    {
                        NinkiRank = "**";
                    }

                    tmp = SE.MinaraiCd1;
                    Minarai = LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.JOCKEY_MINARAI_CD, ref tmp);

                    if (DM.Count != 0) DmValue = DM[i - 1].Split(',');
                    if (TM.Count != 0) TmValue1 = TM[i - 1].Split(',');

                    try
                    {
                        if (Rank != "*" && Rank != "")
                        {
                            dataGridView1.Rows.Add
                            (
                                Rank,
                                SE.Waku1,
                                Int32.Parse(SE.Umaban1).ToString(),
                                SE.Name1,
                                TimeDiff,
                                (DM.Count != 0 ? DmValue[4] : ""),
                                "",
                                (TM.Count != 0 ? TmValue1[4] : ""),
                                "",
                                (Int32.Parse(SE.Oddz1.Substring(0, 3)) + "." + Int32.Parse(SE.Oddz1.Substring(3, 1))).ToString(),
                                NinkiRank,
                                Minarai,
                                SE.Jockey1,
                                SE.Futan1.Substring(0, 2) + "." + SE.Futan1.Substring(2, 1) + "kg",
                                SE.F1
                            );

                            //枠番の色
                            switch (SE.Waku1)
                            {
                                case "1":
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.ForeColor = Color.Black;
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.BackColor = Color.White;
                                    break;
                                case "2":
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.ForeColor = Color.White;
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.BackColor = Color.Black;
                                    break;
                                case "3":
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.ForeColor = Color.White;
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.BackColor = Color.Red;
                                    break;  
                                case "4":
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.ForeColor = Color.White;
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.BackColor = Color.Blue;
                                    break;
                                case "5":
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.ForeColor = Color.Black;
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.BackColor = Color.Yellow;
                                    break;
                                case "6":
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.ForeColor = Color.White;
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.BackColor = Color.Green;
                                    break;
                                case "7":
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.ForeColor = Color.Black;
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.BackColor = Color.Orange;
                                    break;
                                case "8":
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.ForeColor = Color.Black;
                                    dataGridView1[1, dataGridView1.Rows.Count - 1].Style.BackColor = Color.Pink;
                                    break;

                            }
                        }
                    }
                    catch (Exception)
                    {
                        LOG.CONSOLE_MODULE("HR", "ResultOutPutException?? idx>" + i);
                    }
                }
            }

            //すべて終わったら列を昇順に並び替え
            dataGridView1.Sort(dataGridView1.Columns[0], ListSortDirection.Ascending);

            //並び替え後、1位にレコード更新区分
            if (RaData.getRecordFlag())
            {
                dataGridView1[4, 0].Style.BackColor = Color.Red;
                dataGridView1[4, 0].Style.ForeColor = Color.White;
            }

        }
        #endregion


        public int ExecRaData()
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> tmpArray = new List<string>();
            int Dbret = 0;

            if(db.TextReader_Col(this.Key, "RS", 0, ref tmpArray, Key) != 0)
            {
                RaData.setData(ref tmpArray);
                return 1;
            }
            else
            {
                return 0;
            }

            
        }

        public int GetRaceInfo(String Key)
        {
            int ret = 0;

            JVForm JvForm = new JVForm();
            JvForm.JvForm_JvInit();

            ret = JvForm.JvForm_JvRTOpen("0B15", Key);

            if(ret != 0)
            {
                LOG.CONSOLE_MODULE("RS", "JvRTOpenError! [" + ret + "]");
                JvForm.JvForm_JvClose();
                return 0;
            }

            ret = 1;
            String buff = "";
            String fname = "";
            int size = 2000;

            String DataKubun = "";

            JvComDbData.JvDbSEData Sedata = new JvComDbData.JvDbSEData();
            JvComDbData.JvDbRaData Radata = new JvComDbData.JvDbRaData();
            JvComDbData.JvDbHRData Hrdata = new JvComDbData.JvDbHRData();

            Boolean SE_flag = false;
            Boolean RA_flag = false;
            Boolean HR_flag = false;

            while(ret >= 1)
            {
                ret = JvForm.JvForm_JvRead(ref buff, out size, out fname);

                if (buff == "")
                {
                    continue;
                }

                if(ret == -1)
                {
                    //ファイルなし
                    break;
                }

                switch(buff.Substring(0,2))
                {
                    case "SE":
                        Sedata.JvDbSeComMappingFunction(-1, ref buff);
                        SE_flag = true;
                        break;
                    case "RA":
                        DataKubun = Radata.JvDbRaWriteData(0, ref buff);
                        RA_flag = true;
                        break;
                    case "HR":
                        Hrdata.SetHrData(ref buff);
                        HR_flag = true;
                        break;

                }
            }

            JvForm.JvForm_JvClose();

            List<String> RaceDataArray = new List<string>();
            String RaceData = "";
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            if(SE_flag && RA_flag)
            {
                //SEデータとRAデータがともに存在する場合は確定済み
                if(db.TextReader_Col(this.Key, SPEC, 0, ref RaceDataArray, this.Key) == 0)
                {
                    //ファイルが見つからない
                    RaceData = Radata.JvRaDataStr();
                    RaceData += Sedata.JvSeDataStr();
                    db = new dbAccess.dbConnect(this.Key, SPEC, ref RaceData, ref ret);
                }
                else
                {

                    if (RaceDataArray.Count == 0)
                    {
                        //ファイルエラー：取得に失敗
                        return 0;
                    }

                    //データが存在する場合
                    RaceData = Radata.JvRaDataStr();
                    RaData.setData(ref RaceDataArray);

                    if (RaData.DataKubun1 == DataKubun)
                    {
                        //今回取得したデータと前回のデータが同じ場合は更新しない
                        LOG.CONSOLE_TIME_MD("RS", "ResultData Not Update " + this.Key);
                    }
                    else
                    {
                        //異なるため、既存データを削除して更新
                        db.DeleteCsv(SPEC, this.Key.Substring(0, 8) + "/" + SPEC + this.Key + ".csv");
                        RaceData += Sedata.JvSeDataStr();
                        db = new dbAccess.dbConnect(this.Key, "RS", ref RaceData, ref ret);
                    }
                }
            }
 
            if(HR_flag)
            {
                //DB書き込み
                Hrdata.JvWriteHrData();
            }

            //書き込み対象外は戻り値0で戻す
            if(!HR_flag && !RA_flag && !SE_flag)
            {
                return 0;
            }
            return 1;
        }

        #region レース情報書き込み
        public void InitFormLabels()
        {
            //先に競馬場名取得
            String InParam = RaData.getRaceCource();
            String RaceCource = "";
            LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.COURCE_CODE, ref InParam, ref RaceCource);
            //ヘッダー書き込み
            Date.Text = RaData.ConvertDateToDate(RaData.getRaceDate());
            Kaisai.Text = "第" + Int32.Parse(RaData.getRaceKaiji()) + "回" + RaceCource + "競馬" + Int32.Parse(RaData.getRaceKaiji()) + "日目";
            RaceNum.Text = RaceCource;
            rNum.Text = Int32.Parse(RaData.getRaceNum()) + "R";
            racename.Text = RaData.getRaceNameFukus().Trim() + RaData.getRaceName();
            if (RaData.getRaceGradeKai() >= 1)
            {
                kaiji.Text = "第" + RaData.getRaceGradeKai().ToString() + "回";
                racename.Text += "（" + RaData.getRaceGrade() + "）";
            }
            else if (RaData.getRaceGrade() == "L")
            {
                //リステッド競走の場合はLだけレース名のあとにつける。
                racename.Text += "（" + RaData.getRaceGrade() + "）";
            }
            raceNameEng.Text = RaData.getRaceNameEng();

            InParam = RaData.getRaceClass();
            ClassLabel.Text = LOG.JvSysMappingFunction(2007, ref InParam);

            InParam = RaData.getCourceTrack();
            TrackLabel.Text = LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.TRACK_CODE_SHORT, ref InParam);

            InParam = RaData.getOldYear();
            OldYear.Text = LOG.JvSysMappingFunction(20051, ref InParam);

            InParam = RaData.getRaceKindKigo();
            KigoLabel.Text = LOG.JvSysMappingFunction(2006, ref InParam);

            InParam = RaData.getRaceHandCap();
            KigoLabel.Text += LOG.JvSysMappingFunction(2008, ref InParam);

            DistanceLabel.Text = RaData.getDistance();

            InParam = RaData.getCourceTrack();
            TrackNameLabel.Text = LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.TRACK_CODE, ref InParam);

            InParam = RaData.getWeather();
            weLabel.Text = LOG.JvSysMappingFunction(2011, ref InParam);

            InParam = RaData.getTrackStatus();
            TrackDistance.Text = LOG.JvSysMappingFunction(2010, ref InParam);

            //Windowsタイトルバー
            this.Text = "【レース結果】" + RaceNum.Text + Int32.Parse(RaData.getRaceNum()) + "R" + racename.Text;
        }
        #endregion

        #region 払戻金情報の取得と設定
        private void SetPayOutInfo()
        {
            List<String> tmpPayData = new List<string>();

            //払戻データ
            List<int> result_Shubetsu = new List<int>();　
            List<String> result_Kumiban = new List<string>();
            List<int> result_PayOut = new List<int>();
            List<int> result_HenkanBan = new List<int>();
            List<int> result_HenkanWaku = new List<int>();
            List<int> result_HenkanDowaku = new List<int>();

            LOG.CONSOLE_TIME_MD("HR", "SetPayOutInfo>>");

            //払戻パネル有効化
            int DataKubun = 0;
            Int32.TryParse(RaData.DataKubun1, out DataKubun);
            if (DataKubun < 3)
            {
                LOG.CONSOLE_MODULE("HR", ">> NotResultData -> End");
                //払戻データ未確定
                flowLayoutPanel1.Visible = false;
                return;
            }

            db.TextReader_Col(RaData.GET_RA_KEY(), "HR", 0, ref tmpPayData, "P10");
            if(tmpPayData.Count == 0)
            {
                LOG.CONSOLE_MODULE("HR", ">> DataReadFailed -> End");
                //払戻データ取得失敗
                flowLayoutPanel1.Visible = false;
                return;
            }


            int idx = 1;
            int jdx = 0;
            while(idx <= 9)
            {
                if (tmpPayData.Count >= 1 && tmpPayData[3] != "")
                { 
                
                    result_Shubetsu.Add(idx);
                    result_Kumiban.Add(tmpPayData[2]);
                    result_PayOut.Add(Int32.Parse(tmpPayData[3]));

                    //次のデータへインクリメント
                    jdx++;
                }
                else
                {
                    //次の払戻データへ(単勝→複勝へ)
                    idx++;
                    jdx = 0;
                }

                ;

                tmpPayData.Clear();

                //次のデータを読み込み
                db.TextReader_Col(RaData.GET_RA_KEY(), "HR", 0, ref tmpPayData, "P" + idx + jdx);

                if(idx >= 10 && tmpPayData.Count != 0)
                {
                    //返還情報あり
                    LOG.CONSOLE_MODULE("HR", "ReturnPayOut Eneble");
                    Henkan.Text = "返還馬番：";
                    Henkan.Visible = true;
                    for (int i = 1; i < 20; i++)
                    {
                        try { result_HenkanBan.Add(Int32.Parse(tmpPayData[i])); Henkan.Text += Int32.Parse(tmpPayData[i]) + "番 "; }
                        catch(Exception){ break; }
                    }

                    jdx++; tmpPayData.Clear();
                    //次のデータを読み込み：返還枠番(単枠の場合：10頭立ての1枠返還の場合)
                    db.TextReader_Col(RaData.GET_RA_KEY(), "HR", 0, ref tmpPayData, "P" + idx + jdx);
                    if(tmpPayData.Count != 0)
                    {
                        Henkan.Text += "返還枠番：";
                        for (int i = 1; i < 20; i++)
                        {
                            try { result_HenkanWaku.Add(Int32.Parse(tmpPayData[i])); Henkan.Text += Int32.Parse(tmpPayData[i]) + "枠 "; }
                            catch (Exception) { break; }
                        }
                    }

                    jdx++; tmpPayData.Clear();
                    //次のデータを読み込み：返還同枠(8-8)
                    db.TextReader_Col(RaData.GET_RA_KEY(), "HR", 0, ref tmpPayData, "P" + idx + jdx);
                    if (tmpPayData.Count != 0)
                    {
                        Henkan.Text += "返還同枠：";
                        for (int i = 1; i < 20; i++)
                        {
                            try { result_HenkanDowaku.Add(Int32.Parse(tmpPayData[i])); Henkan.Text += Int32.Parse(tmpPayData[i]) + "枠 "; }
                            catch (Exception) { break; }
                        }
                    }
                }
            }

            //前回の馬券種別(複勝やワイド、同着の場合は馬券種別を非表示にする)
            int tmpKind = 0;

            //データ書き込み
            if(result_PayOut.Count != 0 || result_Kumiban.Count != 0 || result_Shubetsu.Count != 0)
            {
#if false
                for(int i = 0; i < result_Shubetsu.Count; i++)
                {
                    if(tmpKind == result_Shubetsu[i])
                    {
                        dataGridView2.Rows.Add("", result_Kumiban[i], result_PayOut[i], "", "", "", "", "", "", "", "");

                    }
                    else
                    {
                        dataGridView2.Rows.Add(WrapShubtsuStr(result_Shubetsu[i]), result_Kumiban[i], result_PayOut[i], "", "", "", "", "", "", "", "");
                    }
                    tmpKind = result_Shubetsu[i];
                }
#endif

                int[] param = new int[4] { 0, 0, 0, 0 };    //4つ目をLoop用のインクリメントに使う
                int tmp = 0;

                //空白行を追加しておく
                dataGridView2.Rows.Add();

                while (param[3] < result_Shubetsu.Count)
                {
                    //パラメーターセット→行数
                    tmp = param[WrapDataLoc(result_Shubetsu[param[3]])];

                    //追加する行が不足していたら、行を追加する
                    if (LOG.MAX(tmp) >= dataGridView2.Rows.Count)
                    {
                        dataGridView2.Rows.Add();
                    }

                    //3かけておく
                    //馬券種類：
                    dataGridView2.Rows[tmp].Cells[WrapDataLoc(result_Shubetsu[param[3]]) * 4].Value
                        = (tmpKind == result_Shubetsu[param[3]] ? "" : WrapShubtsuStr(result_Shubetsu[param[3]]));


                    //組番
                    dataGridView2.Rows[tmp].Cells[(WrapDataLoc(result_Shubetsu[param[3]]) * 4) + 1].Value
                        = ConvKumiban(result_Shubetsu[param[3]], result_Kumiban[param[3]]);

                    //払戻金
                    dataGridView2.Rows[tmp].Cells[(WrapDataLoc(result_Shubetsu[param[3]]) * 4) + 2].Value
                        = (result_PayOut[param[3]] <= 1 ? "0" : String.Format("{0:#,0}円", result_PayOut[param[3]]));

                    //色を変える
                    AddCellsStyle(result_Shubetsu[param[3]], tmp, WrapDataLoc(result_Shubetsu[param[3]]) * 4);
                    AddCellsStyle(result_Shubetsu[param[3]], tmp, WrapDataLoc(result_Shubetsu[param[3]]) * 4 + 1);
                    AddCellsStyle(result_Shubetsu[param[3]], tmp, WrapDataLoc(result_Shubetsu[param[3]]) * 4 + 2);

                    //前回の馬券種別を保持しておく
                    tmpKind = result_Shubetsu[param[3]];

                    //インクリメント
                    param[WrapDataLoc(result_Shubetsu[param[3]])] = tmp + 1;
                    param[3]++;
                }
            }

            LOG.CONSOLE_TIME_MD("HR", "SetPayOutInfo Finish!");
        }
        #endregion

        //払戻用のdatagridの文字色と背景色を変える
        private void AddCellsStyle(int inparam, int Rows, int Cells)
        {
            DEF_HR_DATA param = new DEF_HR_DATA();
            dataGridView2.Rows[Rows].Cells[Cells].Style.BackColor = param.AddBackColor(inparam);
            dataGridView2.Rows[Rows].Cells[Cells].Style.ForeColor = param.AddFontColor(inparam);

            //フォントを変える
            dataGridView2.Rows[Rows].Cells[Cells].Style.Font = new Font("Meiryo UI", 11);
        }

        private String WrapShubtsuStr(int ShubetsuKind)
        {
            DEF_HR_DATA param = new DEF_HR_DATA();
            return param.Str2Shubetsu(ShubetsuKind);
            
        }

        private int WrapDataLoc(int ShubetsuKind)
        {
            DEF_HR_DATA param = new DEF_HR_DATA();
            return param.DataLoc(ShubetsuKind);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //
        private String ConvKumiban(int Shubetsukind, String kumiban)
        {
            switch(Shubetsukind)
            {
                case 1:
                case 2:
                    return kumiban;
                case 3:
                    //枠連は2バイト文字列
                    return Int32.Parse(kumiban.Substring(0, 1)) + " - " + Int32.Parse(kumiban.Substring(1, 1));
                case 4:
                case 5:
                    //連複系はハイフン
                    return Int32.Parse(kumiban.Substring(0, 2)) + " - " + Int32.Parse(kumiban.Substring(2, 2));
                case 6:
                    //馬単は矢印
                    return Int32.Parse(kumiban.Substring(0, 2)) + " → " + Int32.Parse(kumiban.Substring(2, 2));
                case 7:
                    //3連複はハイフンを２つ
                    return Int32.Parse(kumiban.Substring(0, 2)) + " - " + Int32.Parse(kumiban.Substring(2, 2)) + " - " + Int32.Parse(kumiban.Substring(4, 2));
                case 8:
                    //3連複はハイフンを２つ
                    return Int32.Parse(kumiban.Substring(0, 2)) + " → " + Int32.Parse(kumiban.Substring(2, 2)) + " → " + Int32.Parse(kumiban.Substring(4, 2));
            }
            return kumiban;
        }
    }
}

class DEF_HR_DATA
{
    const String TANSHO_O1  = "単　勝";
    const String FUKUSHI_O1 = "複　勝";
    const String WAKUREN_O1 = "枠　連";
    const String WIDE_O1    = "ワイド";
    const String UMAREN_O1  = "馬　連";
    const String UMATAN_O1  = "馬　単";
    const String TRIO_O1    = "３連複";
    const String TRECRTA_O1 = "３連単";

    //配置
    const int LOC_O1 = 0;   //単勝
    const int LOC_O2 = 0;   //複勝
    const int LOC_O3 = 1;   //枠連
    const int LOC_O5 = 1;   //ワイド
    const int LOC_O4 = 2;   //馬連
    const int LOC_O6 = 2;   //馬単
    const int LOC_O7 = 2;   //３連複
    const int LOC_O8 = 2;   //３連単

    //色：背景
    readonly int[] O1_COLOR = { 0, 0, 204 };    //単勝
    readonly int[] O2_COLOR = { 204, 0, 0 };    //複勝
    readonly int[] O3_COLOR = { 0, 204, 0 };    //枠連
    readonly int[] O5_COLOR = { 0, 204, 255 };    //ワイド
    readonly int[] O4_COLOR = { 204, 0, 153 };    //馬連
    readonly int[] O6_COLOR = { 255, 255, 0 };    //馬単
    readonly int[] O7_COLOR = { 0, 0, 255 };    //3連複
    readonly int[] O8_COLOR = { 204, 102, 0 };    //3連単

    //色：文字色
    readonly int[] O1_CHAR_COLOR = { 255, 255, 255 };    //単勝
    readonly int[] O2_CHAR_COLOR = { 255, 255, 255 };    //複勝
    readonly int[] O3_CHAR_COLOR = { 0, 0, 0 };    //枠連
    readonly int[] O5_CHAR_COLOR = { 0, 0, 0 };    //ワイド
    readonly int[] O4_CHAR_COLOR = { 255, 255, 255 };    //馬連
    readonly int[] O6_CHAR_COLOR = { 0, 0, 0 };    //馬単
    readonly int[] O7_CHAR_COLOR = { 255, 255, 255 };    //3連複
    readonly int[] O8_CHAR_COLOR = { 255, 255, 255 };    //3連単

    //払戻種別から文字列を取得(１→単勝)
    public String Str2Shubetsu(int inParam)
    {
        switch(inParam)
        {
            case 1:
                return TANSHO_O1;
            case 2:
                return FUKUSHI_O1;
            case 3:
                return WAKUREN_O1;
            case 4:
                return UMAREN_O1;
            case 5:
                return WIDE_O1;
            case 6:
                return UMATAN_O1; 
            case 7:
                return TRIO_O1;
            case 8:
                return TRECRTA_O1;
        }
        return "";
    }

    //dataGridViewの配置場所を指定
    /**
     * |   0   |   1   |   2   |
     * | 単勝  | 枠連   |  馬連 |
     * | 複勝  | ワイド |  馬単 |
     * |　　　 | 　　　 | 3連複 |
     * |　　　 | 　　　 | 3連単 |
     * */
    public int DataLoc(int inParam)
    {
        switch (inParam)
        {
            case 1:
                return LOC_O1;
            case 2:
                return LOC_O2;
            case 3:
                return LOC_O3;
            case 4:
                return LOC_O4;
            case 5:
                return LOC_O5;
            case 6:
                return LOC_O6;
            case 7:
                return LOC_O7;
            case 8:
                return LOC_O8;
        }
        return 0;
    }

    public Color AddBackColor(int inParam)
    {
        switch (inParam)
        {
            case 1:
                return Color.FromArgb(O1_COLOR[0], O1_COLOR[1], O1_COLOR[2]);
            case 2:
                return Color.FromArgb(O2_COLOR[0], O2_COLOR[1], O2_COLOR[2]);
            case 3:
                return Color.FromArgb(O3_COLOR[0], O3_COLOR[1], O3_COLOR[2]);
            case 4:
                return Color.FromArgb(O4_COLOR[0], O4_COLOR[1], O4_COLOR[2]);
            case 5:
                return Color.FromArgb(O5_COLOR[0], O5_COLOR[1], O5_COLOR[2]);
            case 6:
                return Color.FromArgb(O6_COLOR[0], O6_COLOR[1], O6_COLOR[2]);
            case 7:
                return Color.FromArgb(O7_COLOR[0], O7_COLOR[1], O7_COLOR[2]);
            case 8:
                return Color.FromArgb(O8_COLOR[0], O8_COLOR[1], O8_COLOR[2]);
        }
        return Color.White;
    }

    public Color AddFontColor(int inParam)
    {
        switch (inParam)
        {
            case 1:
                return Color.FromArgb(O1_CHAR_COLOR[0], O1_CHAR_COLOR[1], O1_CHAR_COLOR[2]);
            case 2:
                return Color.FromArgb(O2_CHAR_COLOR[0], O2_CHAR_COLOR[1], O2_CHAR_COLOR[2]);
            case 3:
                return Color.FromArgb(O3_CHAR_COLOR[0], O3_CHAR_COLOR[1], O3_CHAR_COLOR[2]);
            case 4:
                return Color.FromArgb(O4_CHAR_COLOR[0], O4_CHAR_COLOR[1], O4_CHAR_COLOR[2]);
            case 5:
                return Color.FromArgb(O5_CHAR_COLOR[0], O5_CHAR_COLOR[1], O5_CHAR_COLOR[2]);
            case 6:
                return Color.FromArgb(O6_CHAR_COLOR[0], O6_CHAR_COLOR[1], O6_CHAR_COLOR[2]);
            case 7:
                return Color.FromArgb(O7_CHAR_COLOR[0], O7_CHAR_COLOR[1], O7_CHAR_COLOR[2]);
            case 8:
                return Color.FromArgb(O8_CHAR_COLOR[0], O8_CHAR_COLOR[1], O8_CHAR_COLOR[2]);
        }
        return Color.White;
    }
}

