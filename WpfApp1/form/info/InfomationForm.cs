using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.Class;
using WpfApp1.Class.com;

/**　このクラスだけ、別でJVClassを定義しています、
 * 
 * */
namespace WpfApp1.form.info
{
    public partial class InfomationForm : Form
    {
        struct RC_STRUCT
        {
            public Boolean KaisaiFlag;
            public String Key;
            public int OutputLebel;     //表示レベル
            public int MaxRaceCount;    //最大レース数
            public int CourceCd;        //競馬場コード
            public String CourceStr;    //競馬場名（日本語）
            public int PayEndRaceNum;   //発売締切レース番号
            public Boolean RaceCanceldFlg;//開催中止情報
            public MainRaceInfo MainRaceNamme;  //当日メインレース(クラスが一番高い)
        }


        //表示レベル
        const int SAPPORO = 5;
        const int HAKODATE = 5;
        const int NIIGATA = 3;
        const int FUKUSHIMA = 3;
        const int CHUKYO = 4;
        const int KOKURA = 4;
        const int TOKYO = 1;
        const int NAKAYAMA = 1;
        const int KYOTO = 2;
        const int HANSHIN = 2;

        const int MAX_RACE_CNT = 3;

        private System.Windows.Forms.Label[] InfomationLabel;

        Boolean InitFuncEnable = false;
        
        RC_STRUCT[] gCacheParamRaceData = new RC_STRUCT[MAX_RACE_CNT];
        int[] gCacheIntCourceArray = new int[MAX_RACE_CNT];  //競馬場コードから表示エリア格納配列（左→中→右）

        InfomationFormSettingClass SettingClass = new InfomationFormSettingClass();
        BackEndInfomationForm BackEnd = new BackEndInfomationForm();    //バックエンドクラス   
        JvComClass COM = new JvComClass();
        backClass.baclClassInfo[] WeatherCond = new backClass.baclClassInfo[MAX_RACE_CNT];

        DataGrid Rows = new DataGrid();

        List<InfoData> ArrayInfoData = new List<InfoData>();
        InfoData[] InfoDatas = new InfoData[3];

        String DateParam;

        //デバッグフラグ
        Boolean DebugPayFlg = false;

        //文字列定義
        const String IF_TEXT_ID_CAN_1 = "レース以降は開催取り止めとなりました。";
        const String IF_TEXT_ID_CAN_2_1 = "本日の";
        const String IF_TEXT_ID_CAN_2_2 = "競馬は開催中止となりました。";


        public InfomationForm()
        {
            InitializeComponent();
        }

        public InfomationForm(String Date)
        {
            InitializeComponent();
            DateParam = Date;
            COM.CONSOLE_TIME_MD("INFO", "InfomatioForm Date = " + Date);
        }

        public InfomationForm(String Date, Boolean DebugFlg)
        {
            InitializeComponent();
            DateParam = Date;
            COM.CONSOLE_TIME_MD("INFO", "InfomatioForm Date = " + Date + " debug " + DebugFlg.ToString());
            DebugPayFlg = DebugFlg;
        }


        #region フォーム読み込み処理

        private void InfomationForm_Load(object sender, EventArgs e)
        {
            Class.GetOddsComClass getOdds = new Class.GetOddsComClass();
            int ret = 0;

            if(DateParam == "")
            {
                DateParam = DateTime.Today.ToString("yyyyMMdd");
  //              DateParam = "20190303";
            }

            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> ArrayStr = new List<string>();
            db.TextReader_Row(DateParam, "RA", 0, ref ArrayStr);

            MainWindow main = new MainWindow();
                       
            if(gCacheParamRaceData[0].Key == "" && gCacheParamRaceData[1].Key == "" && gCacheParamRaceData[2].Key == "")
            {
                ShowErrorMessage("設定データにエラーが発生しました。");
                return;
            }
            
            axJVLink1.JVInit("UNKNOWN");
            axJVLink1.JVWatchEvent();

            main.SetKaisaiInfo(DateParam, ref ArrayStr);

            //変更情報を一括取得


            if(ArrayStr.Count == 0)
            {
                return;
            }

            String CourceCode = "";

            for (int j=0; j< MAX_RACE_CNT; j++)
            {
                for(int i=1; i<=12; i++)
                {
                    if(ArrayStr.Count <= j)
                    {
                        //第２場開催の場合はエラー
                        continue;
                    }

                    ret = BackEnd.BackEndGetOddsInfo(DateParam + ArrayStr[j] + String.Format("{0:00}", i));
                    //ret = getOdds.GetOddsCom("0B31", DateParam + ArrayStr[j] + String.Format("{0:00}", i)); //単勝オッズ
                    if(ret == -1)
                    {
                        //取得エラー
                        break;
                    }

                    gCacheParamRaceData[j].KaisaiFlag = (ret == 1 ? true:false);    //締切フラグ設定
                    gCacheParamRaceData[j].CourceCd = Int32.Parse(ArrayStr[j]);
                    gCacheParamRaceData[j].MaxRaceCount++;
                    
                    if(ret == 1)
                    {
                        gCacheParamRaceData[j].KaisaiFlag = true;
                        break;
                    }
                    else if(ret == 3)
                    {
                        //開催中止
                        gCacheParamRaceData[j].KaisaiFlag = true;
                        gCacheParamRaceData[j].RaceCanceldFlg = true;
                        break;
                    }
                    else
                    {   //締切
                        gCacheParamRaceData[j].KaisaiFlag = false;
                        gCacheParamRaceData[j].PayEndRaceNum++;
                    }
                }

                gCacheParamRaceData[j].OutputLebel = GetOutPutLebel(gCacheParamRaceData[j].CourceCd);

                //gCacheParamRaceData[j].MainRaceNamme;
            }

            GetTodayMainRaceName(DateParam);

#if DEBUG
            if (DebugPayFlg)
            {
                gCacheParamRaceData[0].KaisaiFlag = DebugPayFlg;
                gCacheParamRaceData[1].KaisaiFlag = DebugPayFlg;
                gCacheParamRaceData[2].KaisaiFlag = DebugPayFlg;
            }
#endif

            //競馬場フィールド有効化のため、再ループ
            //ここが一つでも有効にならないと開催レースなしと表示される。
            //デバッグ時はここをtrueにする
            if (gCacheParamRaceData[0].KaisaiFlag || gCacheParamRaceData[1].KaisaiFlag|| gCacheParamRaceData[2].KaisaiFlag)
            {
                //フォーム開始時の処理
                SetPanelEnable();
                SetWeatherInfo();
                OutPutMainRaceName();
                //SetJockeyInfo();
                Win5KaisaiInfo();   //WIN5
                SetKaisaiCanceld();//開催中止のチェック
                CommonAllGetData();

            }
            else
            {
                ShowErrorMessage("発売中のレースはありませんでした。");
            }

            //起動完了
            InitFuncEnable = true;
            WriteTaskBar("準備完了");

            axJVLink1.JVClose();
        }
        #endregion

        #region 天候・馬場状態設定
        private void SetWeatherInfo()
        {
            List<backClass.baclClassInfo> backInfo = new List<backClass.baclClassInfo>();
            if(BackEnd.BackEndWeatherCondInfo("0B14", DateParam, ref backInfo) != 1)
            {
                return;
            }

            for (int i=0; i < backInfo.Count;i++)
            {
                WeatherCond[i] = backInfo[i];

            }

            if(backInfo.Count != 0)
            {
                //Windowsへ通知は起動時には行わない
                WriteWeatherInfo(); //書き込み 
            }
            
        }

        private void InfoWeatherInfo(String msg)
        {
            List<backClass.baclClassInfo> backInfo = new List<backClass.baclClassInfo>();
            if (BackEnd.BackEndWeatherCondInfo("0B14", DateParam, ref backInfo) != 1)
            {
                return;
            }

            for (int i = 0; i < backInfo.Count; i++)
            {
                WeatherCond[i] = backInfo[i];

            }

            if (backInfo.Count != 0)
            {

                //Windowsへ通知する
                if (InitFuncEnable)
                {
                    CallWeatherCondInfoReq(msg);
                }
                WriteWeatherInfo(); //書き込み 

            }
        }
        #endregion

        #region 各競馬場データを書き込み
        private void WriteWeatherInfo()
        {
            int OutParamLevel = -1;
            for(int i=0; i<MAX_RACE_CNT; i++)
            {
                if (WeatherCond[i] == null)
                {
                    continue;
                }

                for (OutParamLevel = 0; OutParamLevel < gCacheParamRaceData.Length; OutParamLevel++)
                {
                    if (Int32.Parse(WeatherCond[i].Key1.Substring(8, 2)) == gCacheIntCourceArray[OutParamLevel])
                    {
                        break;
                    }
                }

                switch(OutParamLevel)
                {
                    case 0:
                        label5.Text = ConvWeatherString(WeatherCond[i].Weather);
                        label5.BackColor = ConvWeatherKigo(WeatherCond[i].Weather);
                        label12.Text = BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus);
                        label23.Text = BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus);
                        break;
                    case 1:
                        label32.Text = ConvWeatherString(WeatherCond[i].Weather);
                        label32.BackColor = ConvWeatherKigo(WeatherCond[i].Weather);
                        label29.Text = BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus);
                        label31.Text = BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus);
                        break;
                    case 2:
                        label10.Text = ConvWeatherString(WeatherCond[i].Weather);
                        label10.BackColor = ConvWeatherKigo(WeatherCond[i].Weather);
                        label6.Text = BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus);
                        label9.Text = BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus);
                        break;
                        
                }
            }
        }
        #endregion

        #region 天候による背景色の指定
        private Color ConvWeatherKigo(String WeatherCd)
        {
            switch(WeatherCd)
            {
                case "0": return Color.White;
                case "1": return Color.Orange;
                case "2": return Color.Silver;
                case "3": return Color.DodgerBlue;
                case "4": return Color.LightSkyBlue;
                case "5": return Color.LightGray;
                case "6": return Color.DimGray;
                default: return Color.White;
            }
        }
        #endregion

        #region 天候による文字の指定
        private String ConvWeatherString(String WeatherCd)
        {
            switch (WeatherCd)
            {
                case "0": return "";
                case "1": return "☀";
                case "2": return "☁";
                case "3": 
                case "4": return "☂";
                case "5":
                case "6": return "☃";
                default: return "";
            }
        }
        #endregion

        #region 競馬場コードから表示レベルを取得
        private int GetOutPutLebel(int JyoCd)
        {
            switch(JyoCd)
            {
                case 1:
                    return SAPPORO;
                case 2:
                    return HAKODATE;
                case 3:
                    return FUKUSHIMA;
                case 4:
                    return NIIGATA;
                case 5:
                    return TOKYO;
                case 6:
                    return NAKAYAMA;
                case 7:
                    return CHUKYO;
                case 8:
                    return KYOTO;
                case 9:
                    return HANSHIN;
                case 10:
                    return KOKURA;
            }
            return 0;
        }
        #endregion

        #region パネルの有効化（確定レースあり）
        unsafe private void SetPanelEnable()
        {
            int KaisaiCourceCount = 0;
            Boolean Flag1 = false;
            Boolean Flag2 = false;
            Boolean Flag3 = false;

            int LibCode = LibJvConv.LibJvConvFuncClass.COURCE_CODE;

            int KaisaiCource1 = 0, KaisaiCource2 = 0, KaisaiCource3 = 0;
            
            Boolean EAST = false;
            Boolean WEST = false;

            JvRcInfo RC_INFO = new JvRcInfo();
            JvComRcInfo RcFunc = new JvComRcInfo();

            RcFunc.JvComGetRcInfo(DateParam, ref RC_INFO);

            String BackEndReturnStr = "";
            String tmp = "";

            if (RC_INFO.EAST1)
            {
                LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&LibCode, String.Format("{0:00}", RC_INFO.EastRcNum1), ref tmp);
                KaisaiCource1 = RC_INFO.EastRcNum1;
                gCacheIntCourceArray[0] = KaisaiCource1;
                BackEndReturnStr = BackEnd.BackEndGetKaijiNichi(DateParam, KaisaiCource1);　//開催回次取得
                label2.Text = "第" + Int32.Parse(BackEndReturnStr.Substring(10, 2)) + "回 " + tmp + "競馬 " + Int32.Parse(BackEndReturnStr.Substring(12, 2)) + "日目";
                EnablePanelFunction(1);
                OutPutPayRaceLabel(KaisaiCource1, 1);  //確定レース表示
            }

            if (RC_INFO.WEST1)
            {
                LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&LibCode, String.Format("{0:00}", RC_INFO.WestRcNum1), ref tmp);
                KaisaiCource2 = RC_INFO.WestRcNum1;
                gCacheIntCourceArray[1] = KaisaiCource2;
                BackEndReturnStr = BackEnd.BackEndGetKaijiNichi(DateParam, KaisaiCource2);　//開催回次取得
                label22.Text = "第" + Int32.Parse(BackEndReturnStr.Substring(10, 2)) + "回 " + tmp + "競馬 " + Int32.Parse(BackEndReturnStr.Substring(12, 2)) + "日目";
                EnablePanelFunction(2);
                OutPutPayRaceLabel(KaisaiCource2, 2);  //確定レース表示
            }

            if (RC_INFO.LOCAL1)
            {
                LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&LibCode, String.Format("{0:00}", RC_INFO.LocalRcNum1), ref tmp);
                KaisaiCource3 = RC_INFO.LocalRcNum1;
                gCacheIntCourceArray[2] = KaisaiCource3;
                BackEndReturnStr = BackEnd.BackEndGetKaijiNichi(DateParam, KaisaiCource3);　//開催回次取得
                label25.Text = "第" + Int32.Parse(BackEndReturnStr.Substring(10, 2)) + "回 " + tmp + "競馬 " + Int32.Parse(BackEndReturnStr.Substring(12, 2)) + "日目";
                EnablePanelFunction(3);
                OutPutPayRaceLabel(KaisaiCource3, 3);  //確定レース表示
            }




#if false


            for (int i=0; i< MAX_RACE_CNT; i++)
            {
                if (gCacheParamRaceData[i].KaisaiFlag)
                {
                    KaisaiCourceCount++;    //開催コース数をインクリメント
                }
                else
                {
                    
                }

                switch (gCacheParamRaceData[i].CourceCd)
                {
                    case 5:
                    case 6:
                        EAST = true;
                        break;
                    case 8:
                    case 9:
                        WEST = true;
                        break;
                }
            }
            
            if(KaisaiCourceCount == 0)
            {
                return;
            }


            String tmp = "";
            for(int i = 0; i<MAX_RACE_CNT; i++)
            {
                LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&LibCode, String.Format("{0:00}",gCacheParamRaceData[i].CourceCd), ref tmp);
                gCacheParamRaceData[i].CourceStr = tmp;

                //ピンクゾーンを函館・札幌ならすぐに有効
                if (gCacheParamRaceData[i].CourceCd == SAPPORO || gCacheParamRaceData[i].CourceCd == HAKODATE)
                {
                    Flag3 = true;
                    KaisaiCource3 = gCacheParamRaceData[i].CourceCd;
                    label25.Text = tmp;
                    gCacheParamRaceData[i].OutputLebel = 3;
                    EnablePanelFunction(3);
                    continue;
                }

                //1が有効でないかつ新潟・福島なら青ゾーンを有効
                if (!EAST && (gCacheParamRaceData[i].CourceCd == NIIGATA || gCacheParamRaceData[i].CourceCd == FUKUSHIMA))
                {
                    Flag1 = true;
                    KaisaiCource1 = gCacheParamRaceData[i].CourceCd;
                    label2.Text = tmp;
                    gCacheParamRaceData[i].OutputLebel = 1;
                    EnablePanelFunction(1);
                    continue;
                }
                               
                //青ゾーンを東京・中山ならすぐに有効
                if (gCacheParamRaceData[i].OutputLebel == TOKYO || gCacheParamRaceData[i].OutputLebel == NAKAYAMA)
                {
                    Flag1 = true;
                    KaisaiCource1 = gCacheParamRaceData[i].CourceCd;
                    label2.Text = tmp;
                    gCacheParamRaceData[i].OutputLebel = 1;
                    EnablePanelFunction(1);
                    continue;
                }

                //緑ゾーンを京都・阪神ならすぐに有効
                if (gCacheParamRaceData[i].OutputLebel == KYOTO || gCacheParamRaceData[i].OutputLebel == HANSHIN)
                {
                    Flag2 = true;
                    KaisaiCource2 = gCacheParamRaceData[i].CourceCd;
                    label22.Text = tmp;
                    gCacheParamRaceData[i].OutputLebel = 2;
                    EnablePanelFunction(2);
                    continue;
                }
                
                //2が有効でないかつ中京・小倉なら緑ゾーンを有効
                if (!WEST && (gCacheParamRaceData[i].OutputLebel == KOKURA || gCacheParamRaceData[i].OutputLebel == CHUKYO))
                {
                    Flag2 = true;
                    KaisaiCource2 = gCacheParamRaceData[i].CourceCd;
                    label22.Text = tmp;
                    gCacheParamRaceData[i].OutputLebel = 2;
                    EnablePanelFunction(2);
                    continue;
                }
                else
                {
                    Flag3 = true;
                    KaisaiCource3 = gCacheParamRaceData[i].CourceCd;
                    label25.Text = tmp;
                    gCacheParamRaceData[i].OutputLebel = 3;
                    
                }

            }

            String BackEndReturnStr = "";

            //開催情報表示
            //表示エリア順はここで決定
            if(Flag1)
            {
                gCacheIntCourceArray[0] = KaisaiCource1;  //表示エリアの競馬場コードを入れる
                BackEndReturnStr = BackEnd.BackEndGetKaijiNichi(DateParam, KaisaiCource1);　//開催回次取得
                label2.Text = "第" + Int32.Parse(BackEndReturnStr.Substring(10, 2)) + "回 " + label2.Text + "競馬 " +Int32.Parse(BackEndReturnStr.Substring(12, 2)) + "日目";
                OutPutPayRaceLabel(KaisaiCource1, 1);  //確定レース表示
            }
            
            if(Flag2)
            {
                gCacheIntCourceArray[1] = KaisaiCource2;
                BackEndReturnStr = BackEnd.BackEndGetKaijiNichi(DateParam, KaisaiCource2);
                label22.Text = "第" + Int32.Parse(BackEndReturnStr.Substring(10, 2)) + "回 " + label22.Text + "競馬 " + Int32.Parse(BackEndReturnStr.Substring(12, 2)) + "日目";
                OutPutPayRaceLabel(KaisaiCource2, 2);  //確定レース表示
            }

            if (Flag3)
            {
                if(KaisaiCource3 == 0)
                {
                    
                }
                else
                {
                    EnablePanelFunction(3);
                    gCacheIntCourceArray[2] = KaisaiCource3;
                    BackEndReturnStr = BackEnd.BackEndGetKaijiNichi(DateParam, KaisaiCource3);
                    label25.Text = "第" + Int32.Parse(BackEndReturnStr.Substring(10, 2)) + "回 " + label25.Text + "競馬 " + Int32.Parse(BackEndReturnStr.Substring(12, 2)) + "日目";
                    OutPutPayRaceLabel(KaisaiCource3, 3);  //確定レース表示
                }
            }

            //GetRealTimeInfo();
#endif
        }
        #endregion

        #region gCacheIntArrayとgCacheParamRaceDataとの紐付け(RaceDataのインデックスを返す)
        private int CheckPanelLevel( int JyoCd )
        {
            int idx = 0;   
            

            for(idx = 0; idx < gCacheParamRaceData.Length; idx++)
            {
                if(JyoCd == gCacheParamRaceData[idx].CourceCd)
                {
                    return idx;
                }
            }

            return 0;
        }

        private void sort(ref int n1, ref int n2, ref int n3)
        {
            if (n1 > n2) swap(ref n1, ref n2);
            if (n2 > n3) swap(ref n2, ref n3);
            if (n1 > n2) swap(ref n1, ref n2);
        }

        private void swap(ref int nx, ref int ny)
        {
            int temp = nx;
            nx = ny;
            ny = temp;
        }
        #endregion

        #region 確定レース数の表示
        private void OutPutPayRaceLabel(int RcCode,int Kind)
        {

            for (int i = 0; i < MAX_RACE_CNT; i++)
            {
                if(gCacheParamRaceData[i].CourceCd == RcCode)
                {
                       switch(Kind)
                    {
                        case 1:
                            if (gCacheParamRaceData[i].KaisaiFlag && gCacheParamRaceData[i].PayEndRaceNum != 0)
                            {
                                PayInfo1.Visible = true;
                                PayInfo1.Text = gCacheParamRaceData[i].CourceStr + " " + gCacheParamRaceData[i].PayEndRaceNum + "レースまで確定";
                            }
                            else
                            {
                                PayInfo1.Visible = false;
                            }
                            break;
                        case 2:
                            if (gCacheParamRaceData[i].KaisaiFlag && gCacheParamRaceData[i].PayEndRaceNum != 0)
                            {
                                PayInfo2.Visible = true;
                                PayInfo2.Text = gCacheParamRaceData[i].CourceStr + " " + gCacheParamRaceData[i].PayEndRaceNum + "レースまで確定";
                            }
                            else
                            {
                                PayInfo2.Visible = false;
                            }
                            break;
                        case 3:
                            if (gCacheParamRaceData[i].KaisaiFlag && gCacheParamRaceData[i].PayEndRaceNum != 0)
                            {
                                PayInfo3.Visible = true;
                                PayInfo3.Text = gCacheParamRaceData[i].CourceStr + " " + gCacheParamRaceData[i].PayEndRaceNum + "レースまで確定";
                            }
                            else
                            {
                                PayInfo3.Visible = false;
                            }
                            break;
                    }

                }
            }         
        }
#endregion

        private void EnablePanelFunction(int num)
        {
            //表示するパネルやラベルはここに定義
            switch(num)
            {
                case 1:
                    flowMain1.Visible = true;
                    label2.TextAlign = ContentAlignment.MiddleCenter;
                    panel.Visible = true;
                    break;
                case 2:
                    flowMain2.Visible = true;
                    label22.TextAlign = ContentAlignment.MiddleCenter;
                    panel26.Visible = true;
                    break;
                case 3:
                    flowMain3.Visible = true;
                    label25.TextAlign = ContentAlignment.MiddleCenter;
                    panel30.Visible = true;

                    break;
            }
        }

        private void SetJockeyInfo()
        {

        }

#region エラー時のメッセージ表示
        private void ShowErrorMessage(String msg)
        {
            InfomationLabel = new Label[1];
            
            InfomationLabel[0] = new Label();
            InfomationLabel[0].Text = msg;
            InfomationLabel[0].Font = new Font("Meiryo UI", 15);
            InfomationLabel[0].Size = new Size(new Point(700, 250));
            InfomationLabel[0].Location = new Point(this.Size.Width / 4, this.Size.Height/3);
            InfomationLabel[0].TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.AddRange(this.InfomationLabel);
        }
#endregion

        private void label9_Click(object sender, EventArgs e)
        {

        }

           private void label20_Click(object sender, EventArgs e)
        {

        }

        private void panel23_Paint(object sender, PaintEventArgs e)
        {

        }

        int ret = 0;

        /* 天候情報イベントハンドラー */
        private void axJVLink1_JVEvtWeather(object sender, AxJVDTLabLib._IJVLinkEvents_JVEvtWeatherEvent e)
        {
            Console.WriteLine(e.bstr);
            COM.CONSOLE_MODULE("INFO_HDL", e.bstr);
            //int ret = BackEnd.JvInfoBackMain(BackEndInfomationForm.JV_RT_EVENT_WEATHER, e.bstr);
            InfoWeatherInfo(e.bstr);
            // dbAccess.dbConnect db = new dbAccess.dbConnect("RT", ref e.bstr, ref ret);
        }

        private void label20_Click_1(object sender, EventArgs e)
        {

        }

#region フォームを閉じたときのイベントハンドラー
        private void InfomationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            axJVLink1.JVWatchEventClose();
            axJVLink1.JVClose();
        }
#endregion

#region 馬体重発表情報ハンドラー
        private void axJVLink1_JVEvtWeight(object sender, AxJVDTLabLib._IJVLinkEvents_JVEvtWeightEvent e)
        {
            COM.CONSOLE_MODULE("INFO_HDL", e.bstr);  //201902170511の形で入ってくる
            JvWhInfoHdler(e.bstr);
        }

        private void JvWhInfoHdler( String inParam )
        {
            Class.com.windows.JvComWindowsForm WinNoice = new Class.com.windows.JvComWindowsForm();
            String title = "【馬体重発表：";
            String msg = "";
            JVData_Struct.JV_WH_BATAIJYU Wh = new JVData_Struct.JV_WH_BATAIJYU();
            int ret = BackEnd.JvInfoBackMain(7, inParam);

            if(ret == 1)
            {
                String tmp = inParam.Substring(8,2);
                title += COM.JvSysMappingFunction(2001, ref tmp);
                title += Int32.Parse(inParam.Substring(10,2)) + "R】";

                msg += COM.JvSysMappingFunction(2001, ref tmp) + Int32.Parse(inParam.Substring(10, 2)) + "R";

                WinNoice.JvComNoticeShow(1, title, msg);
            }
        }
#endregion

        private void CallBackEndSyncFunction(object obj)
        {
            
        }

        private void CallWeatherCondInfoReq(String inParam)
        {
            Class.com.windows.JvComWindowsForm WinNoice = new Class.com.windows.JvComWindowsForm();
            String title = "";
            String msg = "";
            //            int DiffCource = 0;
            String Cource = inParam.Substring(10, 2);
            String DiffCource = COM.JvSysMappingFunction(2001, ref Cource);
            int CourceIndex = 0;



            JvRcInfo RC_INFO = new JvRcInfo();
            JvComRcInfo RcFunc = new JvComRcInfo();

            RcFunc.JvComGetRcInfo(DateParam, ref RC_INFO);
            int i = 0;

            for (i = 0; i < gCacheParamRaceData.Length; i++)
            {
                if (Int32.Parse(Cource) == gCacheParamRaceData[i].CourceCd)
                {
                    CourceIndex = gCacheParamRaceData[i].OutputLebel;
                    
                    title = "【天候・馬場状態変更：" + DiffCource + "】";
                    if(gCacheParamRaceData[i].CourceCd == RC_INFO.EastRcNum1)
                    {
                        CourceIndex = 0;
                        break;
                    }
                    else if (gCacheParamRaceData[i].CourceCd == RC_INFO.WestRcNum1)
                    {
                        CourceIndex = 1;
                        break;
                    }
                    else if(gCacheParamRaceData[i].CourceCd == RC_INFO.LocalRcNum1)
                    {
                        CourceIndex = 2;
                        break;
                    }
                    else
                    {
                        COM.CONSOLE_TIME_MD("IF", "UnDefined CourceIndex");
                        return;
                    }
                }
            }

            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> tmpArrayParam = new List<string>();
            db.TextReader_Col(inParam.Substring(2, 8), "WE", 0, ref tmpArrayParam, gCacheParamRaceData[i].Key);
           
            JvComDbData.JvDbWEData WeData = new JvComDbData.JvDbWEData();
            WeatherCourceStatus weData = new WeatherCourceStatus();
            int ret = WeData.JvDbWeInitData(inParam, ref weData);
            
            if(ret < 0)
            {
                COM.CONSOLE_TIME_MD("IF", "CallWeatherCondInfoReq() RTError!");
                return;
            }


            switch (CourceIndex)
            {
                case 0:
                    if (label5.Text != ConvWeatherString(weData.Weather) && weData.Weather != "0")
                    {
                        msg += "【天候】 " + label5.Text + "→" + ConvWeatherString(weData.Weather) + "\r\n";
                    }

                    if (label12.Text != BackEnd.BackEndLibMappingFunction(2010, weData.Turf) && weData.Turf != "0")
                    {
                        msg += "【芝】 " + label12.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, weData.Turf) + "\r\n";
                    }

                    if (label23.Text != BackEnd.BackEndLibMappingFunction(2010, weData.Dirt) && weData.Dirt != "0")
                    {
                        msg += "【ダート】 " + label23.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, weData.Dirt) + "\r\n";
                    }
                    break;
                case 1:
                    if (label32.Text != ConvWeatherString(weData.Weather) && weData.Weather != "0")
                    {
                        msg += "【天候】 " + label32.Text + "→" + ConvWeatherString(weData.Weather) + "\r\n";
                    }

                    if (label29.Text != BackEnd.BackEndLibMappingFunction(2010, weData.Turf) && weData.Turf != "0")
                    {
                        msg += "【芝】 " + label29.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, weData.Turf) + "\r\n";
                    }

                    if (label31.Text != BackEnd.BackEndLibMappingFunction(2010, weData.Dirt) && weData.Dirt != "0")
                    {
                        msg += "【ダート】 " + label31.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, weData.Dirt) + "\r\n";
                    }
                    break;
                case 2:
                    if (label10.Text != ConvWeatherString(weData.Weather) && weData.Weather != "0")
                    {
                        msg += "【天候】 " + label10.Text + "→" + ConvWeatherString(weData.Weather) + "\r\n";
                    }

                    if (label6.Text != BackEnd.BackEndLibMappingFunction(2010, weData.Turf) && weData.Turf != "0")
                    {
                        msg += "【芝】 " + label6.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, weData.Turf) + "\r\n";
                    }

                    if (label9.Text != BackEnd.BackEndLibMappingFunction(2010, weData.Dirt) && weData.Dirt != "0")
                    {
                        msg += "【ダート】 " + label9.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, weData.Dirt) + "\r\n";
                    }
                    break;
            }

#if false
            for (int i=0; i<WeatherCond.Length; i++)
            {

                if(WeatherCond[i] == null ) { break; }

                switch(i)
                {
                    case 0:
                        if (label5.Text != ConvWeatherString(WeatherCond[i].Weather))
                        {
                            DiffCource = i;
                            msg += "【天候】 " + label5.Text + "→" + ConvWeatherString(WeatherCond[i].Weather) + "\r\n";
                        }

                        if (label12.Text != BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus))
                        {
                            DiffCource = i;
                            msg += "【芝】 " + label12.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus) + "\r\n";
                        }

                        if (label23.Text != BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus))
                        {
                            DiffCource = i;
                            msg += "【ダート】 " + label23.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus) + "\r\n";
                        }
                        break;
                    case 1:
                        if (label32.Text != ConvWeatherString(WeatherCond[i].Weather))
                        {
                            DiffCource = i;
                            msg += "【天候】 " + label32.Text + "→" + ConvWeatherString(WeatherCond[i].Weather) + "\r\n";
                        }

                        if (label29.Text != BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus))
                        {
                            DiffCource = i;
                            msg += "【芝】 " + label29.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus) + "\r\n";
                        }

                        if (label31.Text != BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus))
                        {
                            DiffCource = i;
                            msg += "【ダート】 " + label31.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus) + "\r\n";
                        }
                        break;
                    case 2:
                        if (label10.Text != ConvWeatherString(WeatherCond[i].Weather))
                        {
                            DiffCource = i;
                            msg += "【天候】 " + label10.Text + "→" + ConvWeatherString(WeatherCond[i].Weather) + "\r\n";
                        }

                        if (label6.Text != BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus))
                        {
                            DiffCource = i;
                            msg += "【芝】 " + label6.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].TurfStatus) + "\r\n";
                        }

                        if (label9.Text != BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus))
                        {
                            DiffCource = i;
                            msg += "【ダート】 " + label9.Text + "→" + BackEnd.BackEndLibMappingFunction(2010, WeatherCond[i].DirtStatus) + "\r\n";
                        }

                }
            }

            if(DiffCource == "0")
            {
                COM.CONSOLE_MODULE("INFO", "WeatherCond NotDiff!!");
                return;
            }
            else
            {
                for (int i = 0; i < gCacheParamRaceData.Length; i++)
                {
                    if (gCacheParamRaceData[i].OutputLebel == i)
                    {
                        title = "【天候・馬場状態変更：" + gCacheParamRaceData[i].CourceStr + "】";
                    }
                }                
            }
#endif

            WinNoice.JvComNoticeShow(1, title, msg);

        }


        private int SearchOutPutLebel(int JyoCd, String name)
        {
            Boolean fLocal = false;
            Boolean East = false;
            Boolean West = false;

                        /* 夏競馬開催判定 */
            for (int i = 0; i < gCacheParamRaceData.Length; i++)
            {
                switch (gCacheParamRaceData[i].CourceCd)
                {
                    case 5:
                    case 6:
                        East = true;
                        break;
                    case 8:
                    case 9:
                        West = true;
                        break;
                }

            }

            switch (JyoCd)
            {
                case 5: //東京
                case 6: //中山
                    return 1;
                case 8: //京都
                case 9: //阪神
                    return 2;
                case 7: //中京
                case 10://小倉
                    if (West) { return 3; }
                    else { return 2; }
                case 3: //福島
                case 4: //新潟
                    if (East) { return 3; }
                    else { return 1; }
                case 1:
                case 2:
                    return 3;
            }

            return 0;

        }

        private int SearchCourceParamIndex(int JyoCd)
        {
            for (　int i = 0; i < gCacheParamRaceData.Length; i++)
            {
                if (JyoCd == gCacheIntCourceArray[i])
                {
                    return i;
                }
            }
            return 0;
        }

        private void WriteTaskBar(String msg)
        {
            statusBar1.Text = msg;
        }

#region WIN5情報取得関数
        private void Win5KaisaiInfo()
        {
            JvComDbData.JvDbW5Data W5data = new JvComDbData.JvDbW5Data();
            if(BackEnd.BackEndGetWin5Info(DateParam) == 1)
            {
                panel6.Visible = true;
                GetWin5Data();
            }

            label46.Text = DateTime.Now.ToShortDateString() + " " +  DateTime.Now.ToShortTimeString() + "現在";
        }
#endregion

#region WIN5データ取得
        private void GetWin5Data()
        {
            JvComDbData.JvDbW5Data W5 = new JvComDbData.JvDbW5Data();
            int w5BoteCount = 0;
            if(W5.Win5Status == 0|| W5.Win5Status == 1 || W5.Win5Status == 2|| W5.Win5Status == 3 || W5.Win5Status == 7 )
            {
                BackEnd.BackEndGetWin5(ref W5);
                textBox6.Text = W5.JvDbW5Satus(W5.Win5Status.ToString());
                if(Int32.TryParse(W5.BoteCount, out w5BoteCount) && w5BoteCount != 0)
                {
                    //発売票数は全レース確定時に出てくるため、発売中・レース中は非表示とする。
                    HatubaiHyosuLabel.Visible = true;
                    label52.Visible = true;
                    label52.Text = w5BoteCount + "票";
                }
                //if(W5.CaleeOver.Trim() == "" || W5.CaleeOver.Trim() == "0" || Int32.Parse(W5.CaleeOver) == 0)
                //{
                //    //キャリーオーバーがない場合は、「前回からのキャリーオーバー」は表示しない
                //    label34.Visible = false;
                //    label35.Visible = false;
                //}
                //else
                //{
                //    label34.Text = Int32.Parse(W5.CaleeOver) + "円";
                //}
                label34.TextAlign = ContentAlignment.MiddleRight;
                label34.Text = Int32.Parse(W5.CaleeOver) + "円";

                if (W5.Win5Status == 1 || W5.Win5Status == 2 || W5.Win5Status == 9 || W5.Win5Status == 0)
                {
                    //WIN5の的中票数と払戻金は確定まで表示しない
                    label41.Visible = false;
                    label42.Visible = false;
                    label43.Visible = false;
                    label44.Visible = false;
                }
                else
                {
                    //確定時に払戻情報を表示する
                    //次回へのキャリーオーバーチェック
                    if (W5.NextCo.Trim() == "")
                    {
                        //エラーチェック（未確定時）：基本的には確定時は0が入るため、ここに入らない
                    }
                    else if (Int32.Parse(W5.NextCo) == 0)
                    {
                        //確定：キャリーオーバーなし
                        label43.Visible = false;
                        label44.Visible = false;
                    }
                    else
                    {
                        //確定：キャリーオーバーあり
                        label43.Visible = true;
                        label44.Visible = true;
                        label43.Text = "次回へのキャリーオーバー";
                        label43.TextAlign = ContentAlignment.MiddleRight;
                        label44.Text = Int32.Parse(W5.NextCo) + "円";
                        label44.TextAlign = ContentAlignment.MiddleRight;
                    }
                    if (W5.HitCount.Trim() == "")
                    {
                        //エラーチェック
                    }
                    else if (Int32.Parse(W5.HitCount) >= 1)
                    {


                        //払戻金情報
                        label35.Visible = true;
                        label34.Visible = true;
                        label35.Text = "的中票数";
                        label34.Text = Int32.Parse(W5.HitCount) + "票";
                        label34.TextAlign = ContentAlignment.MiddleRight;

                        label41.Visible = true;
                        label42.Visible = true;
                        label41.Text = "払戻金";
                        label42.Text = Int32.Parse(W5.PayMoney) + "円";
                        label42.TextAlign = ContentAlignment.MiddleRight;

                    }
                    else if (Int32.Parse(W5.HitCount) == 0)
                    {
                        label35.Visible = true;
                        label34.Visible = true;
                        label35.Text = "的中票数";
                        label34.Text = Int32.Parse(W5.HitCount) + "票";
                        label34.TextAlign = ContentAlignment.MiddleRight;

                        label41.Visible = true;
                        label42.Visible = true;
                        label41.Text = "払戻金";
                        label42.Text = "的中なし";
                        label42.TextAlign = ContentAlignment.MiddleRight;
                    }
                }

                for(int i=0; i<W5.Umaban.Length; i++)
                {
                    if(W5.Umaban[0].Trim() == "")
                    {
                        break;
                    }
                    else
                    {
                        try
                        {
                            switch (i)
                            {
                                case 0:
                                    textBox1.Text = Int32.Parse(W5.Umaban[0].Substring(0, 2)).ToString();
                                    break;
                                case 1:
                                    textBox2.Text = Int32.Parse(W5.Umaban[0].Substring(2, 2)).ToString();
                                    break;
                                case 2:
                                    textBox3.Text = Int32.Parse(W5.Umaban[0].Substring(4, 2)).ToString();
                                    break;
                                case 3:
                                    textBox4.Text = Int32.Parse(W5.Umaban[0].Substring(6, 2)).ToString();
                                    break;
                                case 4:
                                    textBox5.Text = Int32.Parse(W5.Umaban[0].Substring(8, 2)).ToString();
                                    break;
                            }
                        }
                        catch(Exception)
                        {

                        }
                    }
                }
            }


            dbAccess.dbConnect db = new dbAccess.dbConnect();
            JvComDbData.JvDbRaData RA = new JvComDbData.JvDbRaData();
            List<String> tmpLib = new List<string>();

            for(int i=0; i<W5.Key1.Length; i++)
            {
                if(W5.Key1[i] == "")
                {
                    break;
                }
                tmpLib.Clear();
                if (db.TextReader_Col(W5.Date, "RA", 0, ref tmpLib, W5.Key1[i]) != 0)
                {
                    RA.setData(ref tmpLib);  
                    
                    switch(i)
                    {
                        case 0:
                            label4.Text = BackEnd.BackendMappingCourceName(RA.getRaceCource()) + RA.getRaceNum() + "R";
                            label4.ForeColor = OutputLebelToColor(SearchOutPutLebel(Int32.Parse(RA.getRaceCource()), ""));
                            label36.Text = RA.getRaceName6();
                            label36.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                        case 1:
                            label7.Text = BackEnd.BackendMappingCourceName(RA.getRaceCource()) + RA.getRaceNum() + "R";
                            label7.ForeColor = OutputLebelToColor(SearchOutPutLebel(Int32.Parse(RA.getRaceCource()), ""));
                            label37.Text = RA.getRaceName6();
                            label37.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                        case 2:
                            label16.Text = BackEnd.BackendMappingCourceName(RA.getRaceCource()) + RA.getRaceNum() + "R";
                            label16.ForeColor = OutputLebelToColor(SearchOutPutLebel(Int32.Parse(RA.getRaceCource()), ""));
                            label38.Text = RA.getRaceName6();
                            label38.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                        case 3:
                            label17.Text = BackEnd.BackendMappingCourceName(RA.getRaceCource()) + RA.getRaceNum() + "R";
                            label17.ForeColor = OutputLebelToColor(SearchOutPutLebel(Int32.Parse(RA.getRaceCource()), ""));
                            label39.Text = RA.getRaceName6();
                            label39.TextAlign = ContentAlignment.MiddleCenter;
                            break;
                        case 4:
                            label18.Text = BackEnd.BackendMappingCourceName(RA.getRaceCource()) + RA.getRaceNum() + "R";
                            label18.ForeColor = OutputLebelToColor(SearchOutPutLebel(Int32.Parse(RA.getRaceCource()), ""));
                            label40.Text = RA.getRaceName6();
                            label40.TextAlign = ContentAlignment.MiddleCenter;
                            break;

                    }
                }
            }

        }
#endregion

        private void statusBar1_PanelClick(object sender, StatusBarPanelClickEventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void PayInfo1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Win5KaisaiInfo();
        }

        private void flowLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }

#region 払戻金確定ハンドラー
        private void aaxJVLink1_JVEvtPay(object sender, AxJVDTLabLib._IJVLinkEvents_JVEvtPayEvent e)
        {
            String Cource = "";
            int RaceNum = 0;
            String Key = "";
            int ret = BackEnd.JvInfoBackMain(BackEndInfomationForm.JV_RT_EVENT_PAY, e.bstr);
            COM.CONSOLE_MODULE("INFO_HDL", e.bstr + "(" + ret + ")");
            WriteTaskBar("払戻金情報取得" + e.bstr);
            if (ret == 1)
            {
                BackEnd.BackEndoGetPayCource(ref Key, ref Cource, ref RaceNum);                                      //取得した払戻情報をBackEndクラス取得
                gCacheParamRaceData[CheckPanelLevel(Int32.Parse(Cource))].PayEndRaceNum = RaceNum;   //締切済みレース番号を更新
                OutPutPayRaceLabel(Int32.Parse(Cource), SearchOutPutLebel(Int32.Parse(Cource), ""));        //締切情報を更新
                BackEnd.BackEndPayInfoNotice();
            }
            WriteTaskBar("準備完了");
        }
#endregion

        private void axJvLink1_JvComHenader(object sender, object e)
        {
            
        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

#region Labelのレベルによるカラー判定
        private Color OutputLebelToColor(int Lebel)
        {
            switch(Lebel)
            {
                case 1:
                    return Color.DarkBlue;
                case 2:
                    return Color.DarkGreen;
                case 3:
                    return Color.DarkMagenta;
                default:
                    return Color.White;
            }
        }
#endregion

#region 発走時刻変更通知イベントハンドラー
        private void axJVLink1_JVEvtTimeChange(object sender, AxJVDTLabLib._IJVLinkEvents_JVEvtTimeChangeEvent e)
        {
            String Cource = "";
            int RaceNum = 0;
            String Key = "";
            List<String> tmp = new List<string>();

            int ret = BackEnd.JvInfoBackMain(BackEndInfomationForm.JV_RT_EVENT_TIME_CHANGE, e.bstr);
            
            COM.CONSOLE_MODULE("INFO_HDL", e.bstr + "(" + ret + ")");
            WriteTaskBar("発走時刻変更通知" + e.bstr);
            if (ret == 1)
            {
                if (BackEnd.BackEndHassouTimeChangeInfo(BackEnd.BackEndHassouTimeChangeInfoCounter(DateParam), DateParam, ref tmp) == 0)
                {
                    
                }
                else
                {
                    
                }

            }
            WriteTaskBar("準備完了");
        }
#endregion

#region 騎手変更情報ハンドラー
        private void axJVLink1_JVEvtJockeyChange(object sender, AxJVDTLabLib._IJVLinkEvents_JVEvtJockeyChangeEvent e)
        {
            int ret = 0;
            ret = BackEnd.JvInfoBackMain(BackEndInfomationForm.JV_RT_EVENT_JOCKEY_CHANGE, e.bstr);
            CommonAllGetData();
            BackEnd.BackEndJockeyChange(e.bstr);
            COM.CONSOLE_MODULE("INFO_HDL", "JC EventHandler:" + e.bstr + "(" + ret + ")");

        }
        #endregion

        #region 出走取消・競走除外ハンドラー
        private void axJVLink1_JVEvtCourseChange(object sender, AxJVDTLabLib._IJVLinkEvents_JVEvtCourseChangeEvent e)
        {

        }
        #endregion

        #region 開催中止情報
        private void SetKaisaiCanceld()
        {
            if(gCacheParamRaceData.Length == 0)
            {
                return;
            }

            int idx = 0;
            String RaceStr = "";
            for(idx = 0; idx<gCacheParamRaceData.Length; idx++)
            {
                //開催中止情報の取得
                if(gCacheParamRaceData[idx].KaisaiFlag && gCacheParamRaceData[idx].RaceCanceldFlg)
                {
                    assertPanel.Visible = true;
                    RaceStr = BackEnd.BackendMappingCourceName(gCacheParamRaceData[idx].CourceCd.ToString());

                    if (gCacheParamRaceData[idx].PayEndRaceNum == 0)
                    {
                        //1レースを開催せずに中止
                        cancelLabel.Text = "【" + RaceStr + "競馬】 ";
                        cancelLabel.Text += RaceStr + IF_TEXT_ID_CAN_2_2;
                    }
                    else
                    {
                        //1レース以降で中止
                        cancelLabel.Text = "【" + RaceStr + "競馬】 ";
                        cancelLabel.Text += (gCacheParamRaceData[idx].PayEndRaceNum+1) + IF_TEXT_ID_CAN_1;
                    }

                    COM.CONSOLE_MODULE("INFO", "RaceCanceld -> true!!");
                    COM.CONSOLE_MODULE("INFO", "Cource->" + gCacheParamRaceData[idx].CourceCd);
                }
            }


        }

        #endregion

        private void flowMain1_Paint(object sender, PaintEventArgs e)
        {

        }

        //共通で変更情報を取得して更新する。
        private void CommonAllGetData()
        {

            //騎手変更情報取得
            //JCデータ読み込み
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            InfoData tmpInfo = new InfoData();

            List<String> Param = new List<string>();

            //フォントの変更
            dataGridView1.DefaultCellStyle.Font = new Font("Meiryo UI", 10);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9);
            dataGridView2.DefaultCellStyle.Font = new Font("Meiryo UI", 10);
            dataGridView2.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9);
            dataGridView3.DefaultCellStyle.Font = new Font("Meiryo UI", 10);
            dataGridView3.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9);

            //スクロールバーを非表示
            dataGridView1.ScrollBars = ScrollBars.None;
            dataGridView2.ScrollBars = ScrollBars.None;
            dataGridView3.ScrollBars = ScrollBars.None;

            db.DbReadAllData(DateParam, "JC", 0, ref Param, "0", 0);
            if(Param.Count != 0)
            {
                //データあり
                for(int i=0; i < Param.Count; i++)
                {
                    tmpInfo = new InfoData();
                    var value = Param[i].Split(',');
                    tmpInfo.Type1 = InfoData.ID_TYPE_JOCKEY_CHANGE;
                    tmpInfo.InfoRaceKey1 = value[0];
                    tmpInfo.InfoName1 = value[5] + "(" + value[8].Substring(0,2) + "." + value[8].Substring(2,1) + "kg)";
                    tmpInfo.ChangeAfterName1 = value[9] + "(" + value[12].Substring(0, 2) + "." + value[12].Substring(2, 1) + "kg)";
                    tmpInfo.Time1 = value[4];

                    //このときには重複も許す(未定も含まれる)
                    ArrayInfoData.Add(tmpInfo);
                }
            }

            //発走時刻変更
            Param.Clear();
            db.DbReadAllData(DateParam, "TC", 0, ref Param, "0", 0);
            if (Param.Count != 0)
            {
                //データあり
                for (int i = 0; i < Param.Count; i++)
                {
                    tmpInfo = new InfoData();
                    var value = Param[i].Split(',');
                    tmpInfo.Type1 = InfoData.ID_TYPE_RACE_START_CHANGE;
                    tmpInfo.InfoRaceKey1 = value[0];    //キー
                    tmpInfo.OldTime1 = value[2];
                    tmpInfo.NewTime1 = value[3];
                    tmpInfo.Time1 = value[1];               

                    //このときには重複も許す(未定も含まれる)
                    ArrayInfoData.Add(tmpInfo);
                }
            }

            //出走取消・競走除外
            Param.Clear();
            db.DbReadAllData(DateParam, "AV", 0, ref Param, "0", 0);
            if(Param.Count != 0)
            {
                for(int i=0; i < Param.Count; i++)
                {
                    //処理を入れる
                    tmpInfo = new InfoData();
                    var value = Param[i].Split(',');
                    if(Int32.Parse(value[2]) == 1) tmpInfo.Type1 = InfoData.ID_TYPE_TORIKESI;
                    if(Int32.Parse(value[2]) == 2) tmpInfo.Type1 = InfoData.ID_TYPE_JOGAI;

                    tmpInfo.InfoRaceKey1 = value[0];    //キー
                    tmpInfo.InfoName1 = value[3] + "番 " + value[4];   //馬番 + 馬名
                    

                    //このときには重複も許す(未定も含まれる)
                    ArrayInfoData.Add(tmpInfo);
                }
            }


            ChangeInfoDataGridWriteAll(ref ArrayInfoData);
        }

        private void ChangeInfoDataGridWriteAll(ref List<InfoData> InData)
        {
            bool[] JockeyChange = new bool[3];
            bool[] StartTimeChange = new bool[3];
            bool[] TorikeshiChange = new bool[3];
            bool[] JogaiChange = new bool[3];

            int CourceIndex = 0;

            DataGridView CommonsData = new DataGridView();

            bool CommonFlg = false;

            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();

            for (int i = 0; i < InData.Count; i++)
            {
                //datagridviewを切り替える
                CourceIndex = SearchCourceParamIndex(Int32.Parse(InData[i].InfoRaceKey1.Substring(8, 2)));
                switch (CourceIndex)
                {
                    case 0:
                        CommonsData = dataGridView1;
                        break;
                    case 1:
                        CommonsData = dataGridView3;
                        break;
                    case 2:
                        CommonsData = dataGridView2;
                        break;
                    default:
                        continue;
                }

                switch (InData[i].Type1)
                {
                    case 1: //出走取消
                        CourceIndex = SearchCourceParamIndex(Int32.Parse(InData[i].InfoRaceKey1.Substring(8, 2)));
                        if (TorikeshiChange[CourceIndex])
                        {
                            CommonFlg = false;
                            //すでにヘッダー記載済み
                            for (int j = 0; j < CommonsData.Rows.Count; j++)
                            {
                                //ヘッダー、もしくは、空セルの場合
                                if (CommonsData.Rows[j].Cells[1].Value == null || (String)CommonsData.Rows[j].Cells[1].Value == "")
                                {
                                    continue;
                                }

                                if (CommonsData.Rows[j].Cells[1].Value.ToString() == "TK" + InData[i].InfoRaceKey1)
                                {
                                    //上書き対象
                                    CommonsData.Rows[j].Cells[0].Value = InData[i].InfoRaceKey1.Substring(14, 2) + "R";
                                    CommonsData.Rows[j].Cells[1].Value = "TK" + InData[i].InfoRaceKey1;
                                    CommonsData.Rows[j].Cells[2].Value = InData[i].InfoName1;
                                    CommonFlg = true;
                                    break;
                                }
                            }

                            //初データ
                            if (!CommonFlg)
                            {
                                CommonsData.Rows.Add(InData[i].InfoRaceKey1.Substring(14, 2) + "R", "TK" + InData[i].InfoRaceKey1, InData[i].InfoName1);
                            }

                        }
                        else
                        {
                            CommonsData.Rows.Add("", "TK", "＜出走取消＞");
                            CommonsData.Rows.Add(InData[i].InfoRaceKey1.Substring(14, 2) + "R", "TK" + InData[i].InfoRaceKey1, InData[i].InfoName1);
                            TorikeshiChange[CourceIndex] = true;
                        }
                        break;
                    case 2: //競走除外
                        CourceIndex = SearchCourceParamIndex(Int32.Parse(InData[i].InfoRaceKey1.Substring(8, 2)));
                        if (JogaiChange[CourceIndex])
                        {
                            CommonFlg = false;
                            //すでにヘッダー記載済み
                            for (int j = 0; j < CommonsData.Rows.Count; j++)
                            {
                                //ヘッダー、もしくは、空セルの場合
                                if (CommonsData.Rows[j].Cells[1].Value == null || (String)CommonsData.Rows[j].Cells[1].Value == "")
                                {
                                    continue;
                                }

                                if (CommonsData.Rows[j].Cells[1].Value.ToString() == "JG" + InData[i].InfoRaceKey1)
                                {
                                    //上書き対象
                                    CommonsData.Rows[j].Cells[0].Value = InData[i].InfoRaceKey1.Substring(14, 2) + "R";
                                    CommonsData.Rows[j].Cells[1].Value = "JG" + InData[i].InfoRaceKey1;
                                    CommonsData.Rows[j].Cells[2].Value = InData[i].InfoName1;
                                    CommonFlg = true;
                                    break;
                                }
                            }

                            //初データ
                            if (!CommonFlg)
                            {
                                CommonsData.Rows.Add(InData[i].InfoRaceKey1.Substring(14, 2) + "R", "JG" + InData[i].InfoRaceKey1, InData[i].InfoName1);
                            }
                        }
                        else
                        {
                            CommonsData.Rows.Add("", "JG", "＜競走除外＞");
                            CommonsData.Rows.Add(InData[i].InfoRaceKey1.Substring(14, 2) + "R", "JG" + InData[i].InfoRaceKey1, InData[i].InfoName1);
                            JogaiChange[CourceIndex] = true;
                        }
                        break;
                    case 3: //騎手変更
                        CourceIndex = SearchCourceParamIndex(Int32.Parse(InData[i].InfoRaceKey1.Substring(8, 2)));
                        if (JockeyChange[CourceIndex])
                        {
                            CommonFlg = false;
                            //すでにヘッダー記載済み
                            for (int j = 0; j < CommonsData.Rows.Count; j++)
                            {
                                //ヘッダー、もしくは、空セルの場合
                                if (CommonsData.Rows[j].Cells[1].Value == null || (String)CommonsData.Rows[j].Cells[1].Value == "")
                                {
                                    continue;
                                }

                                Console.WriteLine(CommonsData.Rows[j].Cells[1].Value.ToString() + " " + InData[i].InfoRaceKey1 + ":" + InData[i].ChangeAfterName1);
                                if (CommonsData.Rows[j].Cells[1].Value.ToString() == "JC" + InData[i].InfoRaceKey1)
                                {
                                    //上書き対象
                                    CommonsData.Rows[j].Cells[0].Value = InData[i].InfoRaceKey1.Substring(14, 2) + "R";
                                    CommonsData.Rows[j].Cells[1].Value = "JC" + InData[i].InfoRaceKey1;
                                    CommonsData.Rows[j].Cells[2].Value = InData[i].InfoName1 + " → " + InData[i].ChangeAfterName1;
                                    CommonFlg = true;
                                    break;
                                }
                            }

                            //初データ
                            if (!CommonFlg)
                            {
                                CommonsData.Rows.Add(InData[i].InfoRaceKey1.Substring(14, 2) + "R", "JC" + InData[i].InfoRaceKey1, InData[i].InfoName1 + " → " + InData[i].ChangeAfterName1);
                            }

                        }
                        else
                        {
                            CommonsData.Rows.Add("", "JC", "＜騎手変更＞");
                            CommonsData.Rows.Add(InData[i].InfoRaceKey1.Substring(14, 2) + "R", "JC" + InData[i].InfoRaceKey1, InData[i].InfoName1 + " → " + InData[i].ChangeAfterName1);
                            JockeyChange[CourceIndex] = true;
                        }
                        break;
                    case 4: //発走時刻変更
                        CourceIndex = SearchCourceParamIndex(Int32.Parse(InData[i].InfoRaceKey1.Substring(8, 2)));
                        if(StartTimeChange[CourceIndex])
                        {
                            CommonFlg = false;
                            //すでにヘッダー記載済み
                            for (int j = 0; j < CommonsData.Rows.Count; j++)
                            {
                                //ヘッダー、もしくは、空セルの場合
                                if(CommonsData.Rows[j].Cells[1].Value == null || (String)CommonsData.Rows[j].Cells[1].Value == "")
                                {
                                    continue;
                                }
                                
                                if (CommonsData.Rows[j].Cells[1].Value.ToString() == "TC" + InData[i].InfoRaceKey1)
                                {
                                    //上書き対象
                                    CommonsData.Rows[j].Cells[0].Value = InData[i].InfoRaceKey1.Substring(14, 2) + "R";
                                    CommonsData.Rows[j].Cells[1].Value = "TC" + InData[i].InfoRaceKey1;
                                    CommonsData.Rows[j].Cells[2].Value = InData[i].OldTime1 + " → " + InData[i].NewTime1; ;
                                    CommonFlg = true;
                                    break;
                                }
                            }

                            //初データ
                            if(!CommonFlg)
                            {
                                CommonsData.Rows.Add(InData[i].InfoRaceKey1.Substring(14, 2) + "R", "TC" + InData[i].InfoRaceKey1, InData[i].OldTime1 + " → " + InData[i].NewTime1);
                            }

                        }
                        else
                        {
                            CommonsData.Rows.Add("", "TC", "＜発走時刻変更＞");
                            CommonsData.Rows.Add(InData[i].InfoRaceKey1.Substring(14,2) + "R", "TC" + InData[i].InfoRaceKey1, InData[i].OldTime1 + " → " + InData[i].NewTime1);
                            StartTimeChange[CourceIndex] = true;
                        }
                        break;

                    default:
                        break;
                }

                //各列をソート
                //並び替える列を決める
     //           DataGridViewColumn sortColumn = dataGridView1.CurrentCell.OwningColumn;

                //並び替えの方向（昇順か降順か）を決める
                ListSortDirection sortDirection = ListSortDirection.Ascending;


                //並び替えを行う
                dataGridView1.Sort(dataGridView1.Columns[1], sortDirection);
                dataGridView2.Sort(dataGridView2.Columns[1], sortDirection);
                dataGridView3.Sort(dataGridView3.Columns[1], sortDirection);

            }
        }

        #region 本日のメインレースを取得
        private void GetTodayMainRaceName(String Date)
        {
            MainRaceInfo[] ret = new MainRaceInfo[3];
            List<String> tmpArray = new List<string>();

            dbAccess.dbConnect db = new dbAccess.dbConnect();

            Boolean SetFlg = false;

            //レース情報を取得
            db.DbReadAllData(Date, "RA", 0, ref tmpArray, Date, 0);
            if(tmpArray.Count != 0)
            {
                ret[0] = new MainRaceInfo();
                ret[1] = new MainRaceInfo();
                ret[2] = new MainRaceInfo();

                //取得した件数分ループ
                for (int j = 0; j < tmpArray.Count; j++)
                {
                    var value = tmpArray[j].Split(',');

                    for(int i = 0; i < MAX_RACE_CNT; i++)
                    {

                        //一般競走はメインレースにしない
                        if(value[16] == "一般")
                        {
                            continue;
                        }

                        //最初は入ってくる
                        if(gCacheParamRaceData[i].CourceCd == Int32.Parse(value[2]))
                        {
                            //クラスが高い方を置き換える
                            if (ret[i].Class1 <= GredeRankJudge(value[14], value[16]))
                            {
                                SetFlg = true;
                                ret[i].Key1 = Int32.Parse(value[2]);
                                ret[i].Racename1 = value[8];
                                ret[i].Class1 = GredeRankJudge(value[14], value[16]);
                                ret[i].Grade1 = value[16];
                                ret[i].RaceKey1 = value[0];
                            }
                        }

#if false
                        if (ret[i].Key1 == Int32.Parse(value[2]))
                        {
                            //クラスが高い方を置き換える
                            if (ret[i].Class1 <= GredeJudge(value[16]))
                            {
                                SetFlg = true;
                                ret[i].Key1 = Int32.Parse(value[2]);
                                ret[i].Racename1 = value[8];
                                if (value[16] != "一般" && value[16] != "特別")
                                {
                                    ret[i].Racename1 += "(" + value[16] + ")";
                                }
                                ret[i].Class1 = GredeJudge(value[16]);
                            }
                        }
#endif
                    }



                }

                //全部終わったら入れる
                int l = 0;
                for (int n = 0; n < MAX_RACE_CNT; n++)
                {
                    gCacheParamRaceData[n].MainRaceNamme = new MainRaceInfo();

                    if (gCacheParamRaceData[n].CourceCd == ret[0].Key1)
                    {
                        gCacheParamRaceData[n].MainRaceNamme.Racename1 = ret[0].Racename1;
                        gCacheParamRaceData[n].MainRaceNamme.Grade1 = ret[0].Grade1;
                        gCacheParamRaceData[n].MainRaceNamme.RaceKey1 = ret[0].RaceKey1;
                    }

                    if (gCacheParamRaceData[n].CourceCd == ret[1].Key1)
                    {
                        gCacheParamRaceData[n].MainRaceNamme.Racename1 = ret[1].Racename1;
                        gCacheParamRaceData[n].MainRaceNamme.Grade1 = ret[1].Grade1;
                        gCacheParamRaceData[n].MainRaceNamme.RaceKey1 = ret[1].RaceKey1;
                    }

                    if (gCacheParamRaceData[n].CourceCd == ret[2].Key1)
                    {
                        gCacheParamRaceData[n].MainRaceNamme.Racename1 = ret[2].Racename1;
                        gCacheParamRaceData[n].MainRaceNamme.Grade1 = ret[2].Grade1;
                        gCacheParamRaceData[n].MainRaceNamme.RaceKey1 = ret[2].RaceKey1;
                    }
                }
            }
           
        }

        private void OutPutMainRaceName()
        {
            int OutParamLevel = 0;
            Label label = new Label();
            Button button = new Button();

            for (int i = 0; i < MAX_RACE_CNT; i++)
            {
                for (OutParamLevel = 0; OutParamLevel < gCacheParamRaceData.Length; OutParamLevel++)
                {
                    if (gCacheParamRaceData[i].CourceCd == gCacheIntCourceArray[OutParamLevel])
                    {
                        break;
                    }
                }

                switch (OutParamLevel)
                {
                    case 0:
                        label = label11;
                        button = Main1;
                        break;
                    case 1:
                        label = label26;
                        button = Main3;
                        break;
                    case 2:
                        label = label21;
                        button = Main2;
                        break;

                }

                label.Text = gCacheParamRaceData[i].MainRaceNamme.Racename1;
                button.Text = gCacheParamRaceData[i].MainRaceNamme.Racename1;
                if (gCacheParamRaceData[i].MainRaceNamme.Grade1 != "一般" && gCacheParamRaceData[i].MainRaceNamme.Grade1 != "特別")
                {
                    label.Text += "(" + gCacheParamRaceData[i].MainRaceNamme.Grade1 + ")";
                    button.Text += "(" + gCacheParamRaceData[i].MainRaceNamme.Grade1 + ")";
                }
                label.ForeColor = GradeForeColor(gCacheParamRaceData[i].MainRaceNamme.Grade1);
                button.ForeColor = GradeForeColor(gCacheParamRaceData[i].MainRaceNamme.Grade1);

            }
        }


        //グレードコードから、優先度を判断
        private int GredeRankJudge(String ClassCd, String Grade)
        {
            switch (ClassCd)
            {
                case "999": //オープン
                    return GredeRankDetailJudge(Grade);
                case "016": //3勝クラス
                    return 4;
                case "010": //2勝クラス
                    return 3;
                case "005": //1勝クラス
                    return 2;
                case "701": //メイクデビュー
                case "703": //未勝利
                    return 1;
                default:
                    return 0;
            }
        }

        private int GredeRankDetailJudge(String GradeCode)
        {
            switch (GradeCode)
            {
                case "ＧⅠ":
                case "Ｊ・ＧⅠ":
                    return 10;
                case "ＧⅡ":
                case "Ｊ・ＧⅡ":
                    return 9;
                case "ＧⅢ":
                case "Ｊ・ＧⅢ":
                    return 8;
                case "重賞":
                    return 7;
                case "Ｌ":
                    return 6;
                default:
                    break;
            }
            //オープン特別は5で返す
            return 5;
        }

        private Color GradeForeColor(String Grade)
        {
            switch (Grade)
            {
                case "ＧⅠ":
                case "Ｊ・ＧⅠ":
                    return Color.DarkBlue;
                case "ＧⅡ":
                case "Ｊ・ＧⅡ":
                    return Color.DarkRed;
                case "ＧⅢ":
                case "Ｊ・ＧⅢ":
                case "重賞":
                    return Color.DarkGreen;
                case "Ｌ":
                    return Color.DarkSlateGray;
                default:
                    return Color.Black;
            }
        }
#endregion

        private void flowLayoutPanel10_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void axJVLink1_JVEvtAvoid(object sender, AxJVDTLabLib._IJVLinkEvents_JVEvtAvoidEvent e)
        {
            int ret = 0;
            ret = BackEnd.JvInfoBackMain(BackEndInfomationForm.JV_RT_EVENT_AVOID, e.bstr);
            CommonAllGetData();
            COM.CONSOLE_MODULE("INFO_HDL", "AV EventHanlder:" + e.bstr + "(" + ret + ")");
        }

        #region メインレースボタンを押下したときのイベント
        private void button5_Click(object sender, EventArgs e)
        {            
            Syutsuba syutsuba = new Syutsuba(gCacheParamRaceData[CheckPanelLevel(gCacheIntCourceArray[0])].MainRaceNamme.RaceKey1, 1);
            syutsuba.Show();
            
        }

        private void Main2_Click(object sender, EventArgs e)
        {
            Syutsuba syutsuba = new Syutsuba(gCacheParamRaceData[CheckPanelLevel(gCacheIntCourceArray[2])].MainRaceNamme.RaceKey1, 3);
            syutsuba.Show();
        }

        private void Main3_Click(object sender, EventArgs e)
        {
            Syutsuba syutsuba = new Syutsuba(gCacheParamRaceData[CheckPanelLevel(gCacheIntCourceArray[1])].MainRaceNamme.RaceKey1, 2);
            syutsuba.Show();
        }

        //現在の色を取得
        private int GetLineColor(int Area)
        {
            Color clr = new Color();
            switch (Area)
            {
                case 1:
                    clr = panel.BackColor;
                    break;
                case 2:
                    clr = panel30.BackColor;
                    break;
                case 3:
                    clr = panel26.BackColor;
                    break;
                default:
                    break;
            }

            int tmp = clr.ToArgb();

            return 0;
        }
        #endregion
    }

    public class InfoData
    {
        private String InfoRaceKey;     //式別キー
        private int Type;               //情報区分(出走取消・競走除外・騎手変更など)
        private String InfoName;        //騎手名・馬名
        private String ChangeAfterName; //変更後の騎手名・馬名
        private String Time;            //発表時間

        //ID_TYPE_RACE_START_CHANGE用
        private String OldTime;         //変更前時間
        private String NewTime;         //変更五時間
        private int Reason;             //変更理由(出走取消など)

        public const int ID_TYPE_TORIKESI = 1;
        public const int ID_TYPE_JOGAI = 2;
        public const int ID_TYPE_JOCKEY_CHANGE = 3;
        public const int ID_TYPE_RACE_START_CHANGE = 4;
        public const int ID_TYPE_COURCE_CHANGE = 5;

        public string InfoRaceKey1 { get => InfoRaceKey; set => InfoRaceKey = value; }
        public int Type1 { get => Type; set => Type = value; }
        public string InfoName1 { get => InfoName; set => InfoName = value; }
        public string ChangeAfterName1 { get => ChangeAfterName; set => ChangeAfterName = value; }
        public string Time1 { get => Time; set => Time = value; }
        public int Reason1 { get => Reason; set => Reason = value; }
        public string OldTime1 { get => OldTime; set => OldTime = value; }
        public string NewTime1 { get => NewTime; set => NewTime = value; }
    }

    class MainRaceInfo
    {
        private String Racename;    //レース名
        private int Key;            //競馬場コード
        private int Class;          //レースクラス
        private String Grade;
        private String RaceKey;     //レースキー

        public string Racename1 { get => Racename; set => Racename = value; }
        public int Key1 { get => Key; set => Key = value; }
        public int Class1 { get => Class; set => Class = value; }
        public string Grade1 { get => Grade; set => Grade = value; }
        public string RaceKey1 { get => RaceKey; set => RaceKey = value; }

        //Stringの初期化
        public MainRaceInfo()
        {
            Racename = "";
        }
    }
}


