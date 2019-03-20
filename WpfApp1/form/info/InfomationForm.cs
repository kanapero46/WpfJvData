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

        private System.Windows.Forms.Label[] InfomationLabel;

        String DateParam;

        const int MAX_RACE_CNT = 3;

        RC_STRUCT[] gCacheParamRaceData = new RC_STRUCT[MAX_RACE_CNT];
        int[] gCacheIntCourceArray = new int[MAX_RACE_CNT];  //競馬場コードから表示エリア格納配列（左→中→右）

        InfomationFormSettingClass SettingClass = new InfomationFormSettingClass();
        BackEndInfomationForm BackEnd = new BackEndInfomationForm();    //バックエンドクラス   
       

        JvComClass COM = new JvComClass();

        backClass.baclClassInfo[] WeatherCond = new backClass.baclClassInfo[MAX_RACE_CNT];

        public InfomationForm()
        {
            InitializeComponent();
        }

        public InfomationForm(String Date)
        {
            InitializeComponent();
            DateParam = Date;
            COM.CONSOLE_MODULE("INFO", "InfomatioForm Date = " + Date);
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

            if(ArrayStr.Count == 0)
            {
                return;
            }

            String CourceCode = "";

            for (int j=0; j< MAX_RACE_CNT; j++)
            {
                for(int i=1; i<=12; i++)
                {
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
                    else
                    {   //締切
                        gCacheParamRaceData[j].KaisaiFlag = false;
                        gCacheParamRaceData[j].PayEndRaceNum++;
                    }
                }

                gCacheParamRaceData[j].OutputLebel = GetOutPutLebel(gCacheParamRaceData[j].CourceCd);
            }

            //競馬場フィールド有効化のため、再ループ
            //ここが一つでも有効にならないと開催レースなしと表示される。
            //デバッグ時はここをtrueにする
            if(gCacheParamRaceData[0].KaisaiFlag || gCacheParamRaceData[1].KaisaiFlag|| gCacheParamRaceData[2].KaisaiFlag)
            {

            }
            else
            {
                ShowErrorMessage("発売中のレースはありませんでした。");
            }

            SetPanelEnable();
            SetWeatherInfo();


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
                for(OutParamLevel = 0; OutParamLevel < gCacheParamRaceData.Length; OutParamLevel++)
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
            
            for(int i=0; i< MAX_RACE_CNT; i++)
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
                    EnablePanelFunction(3);
                    continue;
                }

                //1が有効でないかつ新潟・福島なら青ゾーンを有効
                if (!EAST && (gCacheParamRaceData[i].CourceCd == NIIGATA || gCacheParamRaceData[i].CourceCd == FUKUSHIMA))
                {
                    Flag1 = true;
                    KaisaiCource1 = gCacheParamRaceData[i].CourceCd;
                    label2.Text = tmp;
                    EnablePanelFunction(1);
                    continue;
                }
                               
                //青ゾーンを東京・中山ならすぐに有効
                if (gCacheParamRaceData[i].OutputLebel == TOKYO || gCacheParamRaceData[i].OutputLebel == NAKAYAMA)
                {
                    Flag1 = true;
                    KaisaiCource1 = gCacheParamRaceData[i].CourceCd;
                    label2.Text = tmp;                   
                    EnablePanelFunction(1);
                    continue;
                }

                //緑ゾーンを京都・阪神ならすぐに有効
                if (gCacheParamRaceData[i].OutputLebel == KYOTO || gCacheParamRaceData[i].OutputLebel == HANSHIN)
                {
                    Flag2 = true;
                    KaisaiCource2 = gCacheParamRaceData[i].CourceCd;
                    label22.Text = tmp;
                    EnablePanelFunction(2);
                    continue;
                }
                
                //2が有効でないかつ中京・小倉なら緑ゾーンを有効
                if (!WEST && (gCacheParamRaceData[i].OutputLebel == KOKURA || gCacheParamRaceData[i].OutputLebel == CHUKYO))
                {
                    Flag2 = true;
                    KaisaiCource2 = gCacheParamRaceData[i].CourceCd;
                    label22.Text = tmp;
                    EnablePanelFunction(2);
                    continue;
                }
                else
                {
                    Flag3 = true;
                    KaisaiCource3 = gCacheParamRaceData[i].CourceCd;
                    label25.Text = tmp;
                    EnablePanelFunction(3);
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
                gCacheIntCourceArray[2] = KaisaiCource3;
                BackEndReturnStr = BackEnd.BackEndGetKaijiNichi(DateParam, KaisaiCource3);
                label25.Text = "第" + Int32.Parse(BackEndReturnStr.Substring(10, 2)) + "回 " + label25.Text + "競馬 " + Int32.Parse(BackEndReturnStr.Substring(12, 2)) + "日目";
                OutPutPayRaceLabel(KaisaiCource3, 3);  //確定レース表示
            }

            //GetRealTimeInfo();
            
        }
        #endregion


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



                    switch (Kind)
                    {




                        
                    }
                    break;
                }
            }         
        }

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
            dbAccess.dbConnect db = new dbAccess.dbConnect("RT", ref e.bstr, ref ret);
        }
        #endregion

        private void CallBackEndSyncFunction(object obj)
        {
            
        }

        #region 払戻金確定イベントハンドラー
        private void axJVLink1_JVEvtPay(object sender, AxJVDTLabLib._IJVLinkEvents_JVEvtPayEvent e)
        {
            String Cource = "";
            int RaceNum = 0;
            int ret = BackEnd.JvInfoBackMain(BackEndInfomationForm.JV_RT_EVENT_PAY, e.bstr);
            COM.CONSOLE_MODULE("INFO_HDL", e.bstr + "(" + ret + ")");
            if(ret == 1)
            {
                BackEnd.BackEndoGetPayCource(ref Cource, ref RaceNum);
                Cource = BackEnd.BackendMappingCourceName(Cource);
                COM.CONSOLE_MODULE("INFO_HDL", Cource + RaceNum);
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

        }

        private void flowLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void flowLayoutPanel10_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
