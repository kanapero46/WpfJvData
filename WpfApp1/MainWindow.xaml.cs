using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibJvConv;
using WpfApp1.Class;
using WpfApp1.dbAccess;
using WpfApp1.form;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        /* JvStructクラス */
        JVForm JVForm = new JVForm();

        /* DB書き込みクラス */
        dbConnect db;

        /* メインデータ書き込みクラス */
        MainDataClass mainDataClass = new MainDataClass();

        /* 別スレッドとのメッセージやり取り */
       
        Object msg;

        /* *******定数定義******* */
        const int RA_RACECOURCE = 2;
        const int RA_KAIJI = 3;
        const int RA_NICHIJI = 4;
        const int RA_RACE_NAME = 7;

        const int JV_SYS_CLOSE_COMP_NOTICE = 1;
        const int JV_SYS_CLOSE_FALUE_NOTICE = 2;
        
        /* *******グローバル変数定義******* */
        String StatusInfo = "";

        int ProgressStatus = 0;
        int MaxValue = 100;

        public MainWindow()
        {
            InitializeComponent();
            Initialized += MainWindow_Activated;
        }

        public void WpfApp1_Load()
        {
            DateText.DisplayDate = DateTime.Today.ToLocalTime();
            DateText.Focus();
        }

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            DateText.DisplayDate = DateTime.Today.ToLocalTime();
        }

        unsafe public int InitMain()
        {
            Thread thread = new Thread(new ParameterizedThreadStart(ThreadLogDataMain));
            /* 状態書き込みスレッド作成 */
            StatusInfo += "-----------------------\n";

            int ret = 0;
            msg = "JVFromの初期化終了[" + ret + "]\n";
            thread.Start(msg);
            thread.Abort();
            
            int op = 2;
            int rdCount = 0;
            int dlCount = 0;
            String lastStamp = "";
            //String FromTime = CngDataToString(DateText.SelectedDate.Value.ToShortDateString());
            //String WeekDay = DateText.SelectedDate.Value.DayOfWeek.ToString();
            //String lastStamp = ChgToDate(FromTime, WeekDay) + "000000";
            ret = JVForm.JvForm_JvOpen("RACE", "00000000000000", op, ref rdCount, ref dlCount, ref lastStamp);

            StatusWrite("JVサーバをオープンしています。[" + ret + "]\n");

            /* JvOpenエラーハンドリング */
            if (ret != 0)
            {
                ret = CheckJvOpenRetErr(ret);

                if (ret != 1)
                {
                    StatusWrite("続行不可のエラーが発生しました。\n");
                    JVForm.JvForm_JvClose();
                    return 0; /* エラー */
                }
                else
                {
                    /* JvCloseできていない場合、再度処理実施 */
                    ret = JVForm.JvForm_JvOpen("RACE", "00000000000000", op, ref rdCount, ref dlCount, ref lastStamp);
                    /* 2回目の失敗はエラー */
                    if (ret != 0)
                    {
                        StatusWrite("続行不可のエラーが発生しました。\n");
                        JVForm.JvForm_JvClose();
                        return 0; /* エラー */
                    }

                }
            }

            StatusWrite("DataKind\t" + op + rdCount + dlCount + "\n");
            StatusWrite("LastStamp\t" + lastStamp + "\n");
    
            /* JvRead用変数の初期化 */
            ret = 1;

            /* ロギング */
            LogingText("JvRead・・・OK\n");

            /* ロギングを別スレッドでスタートする */
            msg = "JVRead Start\n";
            
            ret = 1;
            String buff = "";
            int size = 80000;
            String fname = "";

            /* JRA-VAN DataLab構造体 */
            JVData_Struct.JV_RA_RACE JV_RACE;
            JVData_Struct.JV_SE_RACE_UMA JV_SE_UMA;
            JVData_Struct.JV_DM_INFO Jv_DT_MD;
            String tmp;
            String LibTmp = "";
            int CODE;
            int DbReturn = 1;

            /* DB初期化 */
            db = new dbConnect();
            db.DeleteCsv("RA");
            db.DeleteCsv("SE");
            db.DeleteCsv("DT");


            /* データリード */
            while (ret >= 1)
            {
                /* JvReadする */
                ret = JVForm.JvForm_JvRead(ref buff, out size, out fname);

                /* エラーチェック */
                if (ret > 0)
                {
                    if (buff == "")
                    {
                        continue;
                    }

                    switch (buff.Substring(0, 2))
                    {
                        case "RA":
                            StatusInfo += "*";
                            JV_RACE = new JVData_Struct.JV_RA_RACE();
                            tmp = "";
                            JV_RACE.SetDataB(ref buff);
                            tmp += JV_RACE.id.Year + JV_RACE.id.MonthDay + JV_RACE.id.JyoCD + JV_RACE.id.Kaiji + JV_RACE.id.Nichiji +
                                JV_RACE.id.RaceNum + ",";
                            tmp += JV_RACE.id.Year + JV_RACE.id.MonthDay + ",";
                            tmp += JV_RACE.id.JyoCD + ",";
                            tmp += JV_RACE.id.Kaiji + "," + JV_RACE.id.Nichiji + "," + JV_RACE.id.RaceNum + ",";
                            tmp += JV_RACE.RaceInfo.YoubiCD + ",";
                            CODE = LibJvConvFuncClass.RACE_NAME;
                            //レース名
                            if (JV_RACE.RaceInfo.Hondai.Trim() == "")
                            {
                                LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.JyokenInfo.SyubetuCD + JV_RACE.JyokenInfo.JyokenCD[4], ref LibTmp);
                                tmp += LibTmp;
                            }
                            else
                            {
                                tmp += JV_RACE.RaceInfo.Hondai.Trim();
                            }
                            tmp += ",";
                            tmp += JV_RACE.RaceInfo.Ryakusyo10.Trim() + ",";
                            tmp += JV_RACE.RaceInfo.Fukudai.Trim() + ",";
                            tmp += JV_RACE.RaceInfo.Kakko.Trim() + ",";
                            tmp += JV_RACE.RaceInfo.HondaiEng.Trim() + ",";
                            tmp += JV_RACE.RaceInfo.FukudaiEng.Trim() + ",";
                            tmp += JV_RACE.JyokenInfo.SyubetuCD + ",";
                            tmp += JV_RACE.JyokenInfo.JyokenCD[4] + ",";
                            tmp += JV_RACE.RaceInfo.Nkai + ",";
                            CODE = LibJvConvFuncClass.GRACE_CODE;
                            LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.GradeCD, ref LibTmp);
                            tmp += LibTmp + ",";
                            tmp += JV_RACE.TrackCD + ",";
                            tmp += JV_RACE.Kyori + ",";
                            tmp += JV_RACE.TorokuTosu + ",";
                            db = new dbConnect((JV_RACE.id.Year + JV_RACE.id.MonthDay), JV_RACE.head.RecordSpec, ref tmp, ref DbReturn);
                            break;
                        case "SE":
                            LogingText("@");
                            JV_SE_UMA = new JVData_Struct.JV_SE_RACE_UMA();
                            tmp = "";
                            JV_SE_UMA.SetDataB(ref buff);
                            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + JV_SE_UMA.id.JyoCD + JV_SE_UMA.id.Kaiji +
                                JV_SE_UMA.id.Nichiji + JV_SE_UMA.id.RaceNum + JV_SE_UMA.Umaban + ",";
                            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + ",";
                            tmp += JV_SE_UMA.id.JyoCD + ",";
                            tmp += JV_SE_UMA.id.Kaiji + ",";
                            tmp += JV_SE_UMA.id.Nichiji + ",";
                            tmp += JV_SE_UMA.Wakuban + ",";
                            tmp += JV_SE_UMA.Umaban + ",";
                            tmp += JV_SE_UMA.KettoNum + ",";
                            tmp += JV_SE_UMA.Bamei.Trim() + ",";
                            tmp += JV_SE_UMA.UmaKigoCD + ",";
                            tmp += JV_SE_UMA.SexCD + ",";
                            tmp += JV_SE_UMA.Barei + ",";
                            tmp += JV_SE_UMA.KeiroCD + ",";
                            tmp += JV_SE_UMA.Futan + ",";
                            tmp += JV_SE_UMA.Blinker + ",";
                            tmp += JV_SE_UMA.KisyuCode + ",";
                            tmp += JV_SE_UMA.KisyuRyakusyo + ",";
                            tmp += JV_SE_UMA.MinaraiCD + ",";
                            tmp += JV_SE_UMA.DMTime + ",";
                            tmp += JV_SE_UMA.DMJyuni + ",";
                            db = new dbConnect((JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay), JV_SE_UMA.head.RecordSpec, ref tmp, ref DbReturn);

                            break;
                        default:
                            JVForm.JvForm_JvSkip();
                            break;
                    }

                    if (DbReturn == 0)
                    {
                        break;
                    }
                }
                else if (ret == 0)
                {
                    /* 全ファイルデータ読み込み終了 */
                    ProgressStatus.Visibility = Visibility.Hidden;
                    this.MainBack.Fill = System.Windows.Media.Brushes.SeaGreen;
                    InitCompLabelText();    /* 障害Issue#3 */
                    LabelRaceNum.Content = "OK";
                    JvSysMain(JV_SYS_CLOSE_COMP_NOTICE);

                    break;

                }
                else if (ret == -1)
                {
                    /* ファイル切り替わり */
                    ret = 1;
                    continue;
                }
                else
                {
                    break;
                }
            }


            return 1;
        }

        public void JvSysMain(int msg)
        {
            switch(msg)
            {
                case JV_SYS_CLOSE_COMP_NOTICE: //JV_SYS_CLOSE_COMP_NOTICE
                    /* 場名リスト更新 */
                    AddComboBox(mainDataClass.getRaceDate());
                    JVForm.JvForm_JvClose();
                    break;
                case JV_SYS_CLOSE_FALUE_NOTICE:
                    JVForm.JvForm_JvClose();
                    break;

            }
        }


        private void InitForm()
        {
            
        }

        /* MainWindowsのComboBox更新 */ /* フォルダが生成されていなくても実行可能にする。 */
        unsafe private void AddComboBox(String fromtime)
        {
            /* インスタンス宣言 */
            db = new dbConnect();
            List<String> RaceName = new List<string>();
            List<String> Cource = new List<string>();
            List<String> RaceNum = new List<string>();

            String LibOutParam = "";

   

            /* レース開催日は+1する */
            //fromtime = (Int32.Parse(fromtime) + 1).ToString();

            /* DBから取得 */
            db.TextReader(fromtime, "RA", RA_RACE_NAME, ref RaceName);
            db.TextReader(fromtime, "RA", RA_RACECOURCE, ref Cource);
            db.TextReader(fromtime, "RA", 5, ref RaceNum);

            /* エラーチェック */
            if(Cource.Count == 0)
            {
                return;
            }
            else
            {
                /* リストの初期化 */
                RaceListBox.Items.Clear();

            }

            /* JVコード→文字列変換 */
            int Func = 2001;    // 2001：競馬場コード

            for (int i = 0; i< Cource.Count; i++)
            { 
                LibOutParam = "";
                LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&Func, Cource[i], ref LibOutParam);
                Cource[i] = LibOutParam;
             }

            for (int i = 0; i < RaceName.Count && i<Cource.Count && i < RaceNum.Count ; i++)
            {
                RaceListBox.Items.Add(Cource[i] + RaceNum[i].Trim() + "R :" + RaceName[i]);
            }
        }
       

        /* Jvopenエラーチェック */ /* 0：処理中断　1:処理続行 */
        private int CheckJvOpenRetErr(int ret)
        {
            switch (ret)
            {
                case -1:
                    StatusInfo += ("取得したデータがありませんでした。");
                    return 0;
                case -2:
                    StatusInfo +=("キャンセルしました。");
                    return 0;
                case -111:
                case -112:
                case -114:
                case -115:
                case -116:
                    StatusInfo +=("Jvopenでのパラメータが不正です。[E:" + ret + "]");
                    return 0;
                case -201:
                    StatusInfo +=("JvInitが行われていない");
                    return 0;
                case -202:
                    JVForm.JvForm_JvClose();
                    return 1;   /* 処理続行 */
                case -211:
                    StatusInfo +=("DataLab内部エラーです。[E:211]");
                    break;
                case -301:
                    StatusInfo +=("認証に失敗しました。[E:301]。");
                    return 0;
                case -302:
                    StatusInfo +=("DataLabサービスキーの有効期限が切れています。\nサービス権を購入してください。");
                    return 0;
                case -303:
                    StatusInfo +=("DataLabサービスキーが設定されていません。");
                    return 0;
                case -401:
                    StatusInfo +=("JV-Link内部エラーです。");
                    return 0;
                case -411:
                case -412:
                case -413:
                case -421:
                case -431:
                    StatusInfo +=("サーバーエラーです。[E:" + ret + "]");
                    return 0;
                case -501:
                    StatusInfo +=("スタートキットが無効です。");
                    return 0;
                case -504:
                    StatusInfo +=("JRA-VANサーバーがメンテナンス中です。");
                    return 0;
                default:
                    return 0;

            }
            return 0;
        }

        /* Date型→String型 */
        private String CngDataToString(String Date)
        {
            return (Date.Substring(0, 4) + Date.Substring(5, 2) + Date.Substring(8, 2));
        }

        /* リストの変更イベント */
        private void ComboBox_TextChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        
        private void ComboBoxAddList(String[] RaceName)
        {

        }

        /** **********************************
         * @func  表示レース切り替え処理
         * @event リストボックスが変更されたとき
         * @note  ここでのレース情報を今後のデータとして扱う
         * @inPrm
         * @OutPrm
         ********************************** */
        unsafe private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SolidColorBrush solidColor = new SolidColorBrush();
            String RaceCource ="";
            Boolean East = false;
            Boolean West = false;
            int Select = RaceListBox.SelectedIndex;

            if (Select == -1) { return; } /* 障害Issue#3 */

            try
            {
                RaceCource = RaceListBox.SelectedItem.ToString().Substring(0, 2);
            }
            catch (NullReferenceException ex) { }
            
            String fromtime = CngDataToString(DateText.SelectedDate.Value.ToShortDateString());
            List<String> TextArray = new List<string>();
            db = new dbConnect();

            /* 競馬場名取得（コード） */
            db.TextReader(fromtime, "RA", RA_RACECOURCE, ref TextArray);
            /* クラスにセット */
            mainDataClass.setRaceCoutce(TextArray[Select]);
            
            /* 夏競馬開催判定 */
            for (int i = 0; i < TextArray.Count; i++)
            {
                switch(TextArray[i])
                {
                    case "05":
                    case "06":
                        East = true;
                        break;
                    case "08":
                    case "09":
                        West = true;
                        break;
                }
            }

            TextArray.Clear();

            switch (RaceCource)
            {
                case "東京":
                case "中山":
                    MainBack.Fill = System.Windows.Media.Brushes.Blue;
                    break;
                case "阪神":
                case "京都":
                    MainBack.Fill = System.Windows.Media.Brushes.DarkGreen;
                    break;
                case "中京":
                case "小倉":
                    if (West) { MainBack.Fill = System.Windows.Media.Brushes.Purple; }
                    else { MainBack.Fill = System.Windows.Media.Brushes.DarkGreen; }
                    break;   
                case "福島":
                case "新潟":
                    if (East) { MainBack.Fill = System.Windows.Media.Brushes.Purple; }
                    else { MainBack.Fill = System.Windows.Media.Brushes.Blue; }
                    break;
                case "札幌":
                case "函館":
                    MainBack.Fill = System.Windows.Media.Brushes.Purple;
                    break;
            }

            int CODE;
            String tmp = "";
            try
            {
                /* 競馬場名入力 */
                CODE = LibJvConvFuncClass.COURCE_CODE;
                String inParam = mainDataClass.getRaceCource();
                LibJvConvFuncClass.jvSysConvFunction(&CODE, inParam, ref tmp);
                LabelCource.Content = tmp;
                /* クラスにセット */

                /* レース番号入力 */
                db.TextReader(fromtime, "RA", 5, ref TextArray);
                LabelRaceNum.Content = TextArray[Select].ToUpper() + "Ｒ";
                mainDataClass.setRaceNum(TextArray[Select]);

                TextArray.Clear();

                /* レース名 */
                db.TextReader(fromtime, "RA", RA_RACE_NAME, ref TextArray);
                LabelRaceName.Content = TextArray[Select];

                /* グレード */
                TextArray.Clear();
                db.TextReader(fromtime, "RA", 16, ref TextArray);
                if (!(TextArray[Select] == "一般" || TextArray[Select] == "特別"))
                {
                    LabelRaceName.Content += "（" + TextArray[Select] + "）";
                }

                /* 回次：非表示 */
                TextArray.Clear();
                db.TextReader(fromtime, "RA", RA_KAIJI, ref TextArray);
                mainDataClass.setRaceKaiji(TextArray[Select]);

                /* 日次：非表示 */
                TextArray.Clear();
                db.TextReader(fromtime, "RA", RA_NICHIJI, ref TextArray);
                mainDataClass.setRaceNichiji(TextArray[Select]);

            }
            catch (ArgumentOutOfRangeException)
            {

                /* ファイル更新中にコールされるため、Exceptionはスルーする */
            }

        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        /* 実行ボタンを押下したときの処理 */
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            int ret = 0;

            if(DateText.SelectedDate == null)
            {
                System.Windows.MessageBox.Show("レース開催日が選択されていません。\n処理を終了します。\nERR:500", "データエラー");
                return;
            }

            String fromtime = CngDataToString(DateText.SelectedDate.Value.ToShortDateString());

            //ロギングデータ
            StatusInfo = "";    //初期化
            StatusInfo += "処理開始時間\t\t：" + DateTime.Now.ToLongTimeString() + "\n";
            StatusInfo += "取得するレース開催日\t：" + fromtime + "\n";
            StatusInfo += "DpSw\t\t\t：" + ((bool)this.OffLineFlag.IsChecked ? "1" : "0") + "\n";

            mainDataClass.setRaceDate(fromtime);

            if ((bool)this.OffLineFlag.IsChecked)
            {
                AddComboBox(CngDataToString(DateText.SelectedDate.Value.ToShortDateString()));
                StatusInfo += "DataLabからのデータの取得をスキップしました。\n";
                StatusInfo += "処理が終了しました。\n";
                StatusWriter();   //スレッド開始せずに呼び出し
            }
            else
            {
                if(JVForm.JvForm_JvInit() != 0)
                {
                    StatusWrite("SIDがセットされていません。\n処理を終了します。\n");
                    return;
                }

                if((bool)this.ToWeekFlag.IsChecked){ InitMain(); }
                StatusInfo += "当週データの取得を終了しました。\n";
                StatusWriter();
                if ((bool)this.MasterFlag.IsChecked) { InitMSTMain(); }
            }
        }

        /** **********************************
         * @func  「出馬表」ボタンをクリックしたときの処理
         * @event リストボックスが変更されたとき
         * @inPrmMainDataGrid
         * @OutPrm
         ********************************** */
        unsafe private void SyutubaButton_Click(object sender, RoutedEventArgs e)
        {
            String key = "";
            String tmp = "";
            String RaceCource = mainDataClass.getRaceCource();
            String RaceNum = mainDataClass.getRaceNum();
            String Date = mainDataClass.getRaceDate();
            var horces = new ObservableCollection<HorceProgramClass>();
            HorceProgramClass _horce = new HorceProgramClass();

            if (RaceCource == "" || RaceNum == "")
            {
                /* 競馬場名・レース名が選択されていない場合 */
                return;
            }

            mainDataClass.GET_AUTO_RA_KEY(ref key);

            //TODO：競馬場種別カラーを渡したいなぁー
            System.Windows.Media.Brush brush = MainBack.Fill;
            String Color = brush.ToString();
            int JomeiColor = 0;
            switch (Color)
            {
                case "#FF0000FF":
                    JomeiColor = 1;
                    break;
                case "#FF006400":
                    JomeiColor = 2;
                    break;
                case "FF800080":
                    JomeiColor = 3;
                    break;
            }


            Syutsuba syutsuba = new Syutsuba(key, JomeiColor);
            syutsuba.Show();
            
        }


        private void Button_Click_InitData(object sender, RoutedEventArgs e)
        {
            InitSettingForm form1 = new InitSettingForm();
            form1.Show();
        }

        private int JvMainErrorFunction(int res)
        {
            //ProgressStatus.Visibility = Visibility.Hidden;

            return res;
        }

        private void LogingText(String Log)
        {
            LogText.Text += Log;
        }

        private void hogehoge()
        {
           
        }

        /** **********************************
         * @func  ステータス状態の初期化
         * @event
         * @note 
         * @inPrm
         * @OutPrm
         * @Issue  0003
         ********************************** */
         private void InitCompLabelText()
        {
            LabelCource.Content = "";
            LabelRaceNum.Content = "";
            LabelRaceName.Content = "";
        }

        /** **********************************
         * @func  ログ出力用の関数
         * @event リストボックスが変更されたとき
         * @note  ここでのレース情報を今後のデータとして扱う
         * @inPrm
         * @OutPrm
         ********************************** */
         public void StatusWrite(String status)
        {
            Status.Text = status;
        }


        public void StatusWriter(){
            try
            {
            }
            catch(InvalidOperationException)
            {

            }           
        }
        
        public void ThreadStatusWriter()
        {
            /* スレッド用 */
            StatusWriter();
            Thread.Sleep(20000);
            StatusInfo = "";
        }

        unsafe private void ThreadJvReadFunction()
        {
  
        }

        delegate void delegate1(String text1);

        void setFocus(String text1)
        {
            Status.Text = text1;
        }

        public void ThreadLogDataMain(object msg)
        {   
            Log LogForm = new Log();
        }

        /** **********************************
         * @func  蓄積用データ取得
         * @event 
         * @note         
         * @inPrm
         * @OutPrm
         ********************************** */
        unsafe private int InitMSTMain()
        {
            int ret;

            String FromTime = CngDataToString(DateText.SelectedDate.Value.ToShortDateString());
            String WeekDay = DateText.SelectedDate.Value.DayOfWeek.ToString();
            String lastStamp = ChgToDate(FromTime, WeekDay);
            int op = 2;
            int rdCount = 0;
            int dlCount = 0;

            ret = JVForm.JvForm_JvOpen("RCOV", "00000000000000", op, ref rdCount, ref dlCount, ref lastStamp);

            /* JvOpenエラーハンドリング */
            if (ret != 0)
            {
                ret = CheckJvOpenRetErr(ret);

                if (ret != 1)
                {
                    StatusWrite("続行不可のエラーが発生しました。\n");
                    JVForm.JvForm_JvClose();
                    return 0; /* エラー */
                }
                else
                {
                    /* JvCloseできていない場合、再度処理実施 */
                    ret = JVForm.JvForm_JvOpen("RCOV", "00000000000000", op, ref rdCount, ref dlCount, ref lastStamp);
                    /* 2回目の失敗はエラー */
                    if (ret != 0)
                    {
                        StatusWrite("続行不可のエラーが発生しました。\n");
                        JVForm.JvForm_JvClose();
                        return 0; /* エラー */
                    }

                }
            }

            StatusWrite("DataKind\t" + op + rdCount + dlCount + "\n");
            StatusWrite("LastStamp\t" + lastStamp + "\n");

            /* JvRead用変数の初期化 */
            ret = 1;

            /* ロギング */
            LogingText("JvRead・・・OK\n");

            /* ロギングを別スレッドでスタートする */
            msg = "JVRead Start\n";

            ret = 1;
            String buff = "";
            int size = 80000;
            String fname = "";

            String tmp = "";
            String strBuff = "";

            /* ライブラリ用変数 */
            int CODE;
            int DbReturn = 1;
            String LibTmp = "";
            
            /* データを追加するにはここに構造体を追加 */
            JVData_Struct.JV_RA_RACE JV_RACE = new JVData_Struct.JV_RA_RACE();
            JVData_Struct.JV_SE_RACE_UMA JV_SE_UMA = new JVData_Struct.JV_SE_RACE_UMA();
            JVData_Struct.JV_UM_UMA JV_UMA = new JVData_Struct.JV_UM_UMA();

            /* DB初期化 */
            db = new dbConnect();
            db.DeleteCsv("RA_MST");
            db.DeleteCsv("SE_MST");
            db.DeleteCsv("UM_MST");

            /* ログスレッド起動 */
            Thread t = new Thread(new ParameterizedThreadStart(LogMainOutPutFormThread));
            t.SetApartmentState(ApartmentState.STA);
            t.Start("100"); //仮で100を設定

            while (ret >= 1)
            {
                ret = JVForm.JvForm_JvRead(ref buff, out size, out fname);

                if (ret > 0)
                {
                    if (buff == "")
                    {
                        continue;
                    }
                    else if(strBuff == buff.Substring(0, 2))
                    {
                        /* 排他制御にする。 */
                        Interlocked.Exchange(MaxValue, ret);
                        Interlocked.Exchange(ProgressStatus, 0);
                    }

                    switch (buff.Substring(0, 2))
                    {
                        case "RA":
                            JV_RACE = new JVData_Struct.JV_RA_RACE();
                            tmp = "";
                            JV_RACE.SetDataB(ref buff);
                            tmp += JV_RACE.id.Year + JV_RACE.id.MonthDay + JV_RACE.id.JyoCD + JV_RACE.id.Kaiji + JV_RACE.id.Nichiji +
                                JV_RACE.id.RaceNum + ",";
                            tmp += JV_RACE.id.Year + JV_RACE.id.MonthDay + ",";
                            tmp += JV_RACE.id.JyoCD + ",";
                            tmp += JV_RACE.id.Kaiji + "," + JV_RACE.id.Nichiji + "," + JV_RACE.id.RaceNum + ",";
                            tmp += JV_RACE.RaceInfo.YoubiCD + ",";

                            CODE = LibJvConvFuncClass.RACE_NAME;
                            //レース名
                            if (JV_RACE.RaceInfo.Hondai.Trim() == "")
                            {
                                LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.JyokenInfo.SyubetuCD + JV_RACE.JyokenInfo.JyokenCD[4], ref LibTmp);
                                tmp += LibTmp;
                            }
                            else
                            {
                                tmp += JV_RACE.RaceInfo.Hondai.Trim();
                            }
                            tmp += ",";
                            tmp += JV_RACE.RaceInfo.Ryakusyo10.Trim() + ",";
                            tmp += JV_RACE.RaceInfo.Fukudai.Trim() + ",";
                            tmp += JV_RACE.RaceInfo.Kakko.Trim() + ",";
                            tmp += JV_RACE.RaceInfo.HondaiEng.Trim() + ",";
                            tmp += JV_RACE.RaceInfo.FukudaiEng.Trim() + ",";
                            tmp += JV_RACE.JyokenInfo.SyubetuCD + ",";
                            tmp += JV_RACE.JyokenInfo.JyokenCD[4] + ",";
                            tmp += JV_RACE.RaceInfo.Nkai + ",";
                            CODE = LibJvConvFuncClass.GRACE_CODE;
                            LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.GradeCD, ref LibTmp);
                            tmp += LibTmp + ",";
                            db = new dbConnect("0", JV_RACE.head.RecordSpec, ref tmp, ref DbReturn);
                            ProgressStatus++;
                            break;
                        case "SE":
                            JV_SE_UMA = new JVData_Struct.JV_SE_RACE_UMA();
                            tmp = "";
                            JV_SE_UMA.SetDataB(ref buff);
                            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + JV_SE_UMA.id.JyoCD + JV_SE_UMA.id.Kaiji +
                                JV_SE_UMA.id.Nichiji + JV_SE_UMA.id.RaceNum + JV_SE_UMA.Umaban + ",";
                            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + ",";
                            tmp += JV_SE_UMA.id.JyoCD + ",";
                            tmp += JV_SE_UMA.id.Kaiji + ",";
                            tmp += JV_SE_UMA.id.Nichiji + ",";
                            tmp += JV_SE_UMA.Wakuban + ",";
                            tmp += JV_SE_UMA.Umaban + ",";
                            tmp += JV_SE_UMA.KettoNum + ",";
                            tmp += JV_SE_UMA.Bamei.Trim() + ",";
                            tmp += JV_SE_UMA.UmaKigoCD + ",";
                            tmp += JV_SE_UMA.SexCD + ",";
                            tmp += JV_SE_UMA.Barei + ",";
                            tmp += JV_SE_UMA.KeiroCD + ",";
                            tmp += JV_SE_UMA.Futan + ",";
                            tmp += JV_SE_UMA.Blinker + ",";
                            tmp += JV_SE_UMA.KisyuCode + ",";
                            tmp += JV_SE_UMA.KisyuRyakusyo + ",";
                            tmp += JV_SE_UMA.MinaraiCD + ",";
                            db = new dbConnect("0", JV_SE_UMA.head.RecordSpec, ref tmp, ref DbReturn);
                            ProgressStatus++;
                            break;

                        case "UM": //競走馬マスタ
                            JV_UMA = new JVData_Struct.JV_UM_UMA();
                            JV_UMA.SetDataB(ref buff);
                            tmp = "";
                            tmp += JV_UMA.KettoNum + ","; //血統登録番号キー
                            tmp += JV_UMA.Bamei.Trim() + ",";
                            tmp += JV_UMA.BameiEng.Trim()+ ",";
                            tmp += JV_UMA.UmaKigoCD+ ",";
                            tmp += JV_UMA.SexCD + ",";
                            tmp += JV_UMA.KeiroCD + ",";
                            tmp += JV_UMA.Ketto3Info[0].Bamei.Trim() + ","; //父
                            tmp += JV_UMA.Ketto3Info[1].Bamei.Trim() + ","; //母
                            tmp += JV_UMA.Ketto3Info[2].Bamei.Trim() + ","; //父父
                            tmp += JV_UMA.Ketto3Info[4].Bamei.Trim() + ","; //母父
                            tmp += JV_UMA.Ketto3Info[12].Bamei.Trim() + ","; //母母父
                            tmp += JV_UMA.Kyakusitu[0] + ",";
                            tmp += JV_UMA.Kyakusitu[1] + ",";
                            tmp += JV_UMA.Kyakusitu[2] + ",";
                            tmp += JV_UMA.Kyakusitu[3] + ",";
                            tmp += JV_UMA.Ketto3Info[0].HansyokuNum + ",";  //父の系統
                            tmp += JV_UMA.Ketto3Info[4].HansyokuNum + ",";  //母父の系統
                            tmp += JV_UMA.Ketto3Info[12].HansyokuNum + ",";  //母母父の系統
                            tmp += JV_UMA.Ketto3Info[2].HansyokuNum + ",";  //父父の系統
                            tmp += JV_UMA.Ketto3Info[6].HansyokuNum + ",";  //父父父の系統
                            tmp += JV_UMA.Ketto3Info[10].HansyokuNum + ","; //母父父の血統
                            db = new dbConnect("0", JV_UMA.head.RecordSpec, ref tmp, ref DbReturn);
                            ProgressStatus++;
                            break;

                        default:
                            JVForm.JvForm_JvSkip();
                            break;
                    }

                    if (DbReturn == 0)
                    {
                        break;
                    }
                }
                else if (ret == 0)
                {
                    /* 全ファイルデータ読み込み終了 */
                    //ProgressStatus.Visibility = Visibility.Hidden;
                    this.MainBack.Fill = System.Windows.Media.Brushes.SeaGreen;
                    InitCompLabelText();    /* 障害Issue#3 */
                    LabelRaceNum.Content = "OK";
                    JvSysMain(JV_SYS_CLOSE_COMP_NOTICE);

                    break;

                }
                else if (ret == -1)
                {
                    /* ファイル切り替わり */
                    ret = 1;
                    continue;
                }
                else
                {
                    break;
                }

            }
            JVForm.JvForm_JvClose();
                return 1;
        }

        /* データマイニング情報取得(リアルタイム) */
        unsafe private int InitRealTimeDataMaining()
        {
            int ret;

            String FromTime = CngDataToString(DateText.SelectedDate.Value.ToShortDateString());
            String WeekDay = DateText.SelectedDate.Value.DayOfWeek.ToString();
            String lastStamp = ChgToDate(FromTime, WeekDay);
            String Dt = "MING";
            int op = 1;
            int rdCount = 0;
            int dlCount = 0;

            ret = JVForm.JvForm_JvRTOpen( Dt, FromTime);

            /* JvOpenエラーハンドリング */
            if (ret != 0)
            {
                ret = CheckJvOpenRetErr(ret);

                if (ret != 1)
                {
                    StatusWrite("続行不可のエラーが発生しました。\n");
                    JVForm.JvForm_JvClose();
                    return 0; /* エラー */
                }
                else
                {
                    /* JvCloseできていない場合、再度処理実施 */
                    ret = JVForm.JvForm_JvRTOpen( Dt, FromTime);
                    /* 2回目の失敗はエラー */
                    if (ret != 0)
                    {
                        StatusWrite("続行不可のエラーが発生しました。\n");
                        JVForm.JvForm_JvClose();
                        return 0; /* エラー */
                    }

                }
            }

            StatusWrite("DataKind\t" + op + rdCount + dlCount + "\n");
            StatusWrite("LastStamp\t" + lastStamp + "\n");

            /* JvRead用変数の初期化 */
            ret = 1;

            /* ライブラリ用変数 */
            int CODE;
            int DbReturn = 1;
            String LibTmp = "";
            String tmp = "";
            
            /* データを追加するにはここに構造体を追加 */
            JVData_Struct.Jv_DT_MD JV_DTMD = new JVData_Struct.Jv_DT_MD();
            
            /* DB初期化 */
            db = new dbConnect();
            db.DeleteCsv("DM");

            while (ret >= 1)
            {
                ret = JVForm.JvForm_JvRead(ref buff, out size, out fname);

                if (ret > 0)
                {
                    if (buff == "")
                    {
                        continue;
                    }

                    switch (buff.Substring(0, 2))
                    {
                        case "DT":  /* 対戦型 */
                        case "TM":  /* タイム型 */
                            JV_DTMD.SetDataB(ref buff);
                            for(i = 0; i < 18 - 1; i++)
                            {
                                tmp = "";
                                tmp += JV_DTMD.id.Year + JV_DTMD.id.MonthDay + JV_DTMD.id.JyoCD + JV_DTMD.id.Kaiji +
                                    JV_DTMD.id.Nichiji + JV_DTMD.id.RaceNum + JV_DTMD.DMInfo.Umaban[i] +  ",";
                                tmp += JV_DTMD.head.RecordSpec + ",";
                                tmp += JV_DTMD.head.DataKubun + ",";
                                tmp += JV_DTMD.DMInfo.Umaban[i] + ",";
                                tmp += JV_DTMD.DMInfo.DMTime + ",";
                                tmp += JV_DTMD.DMInfo.DMGosaP + ",";
                                tmp += JV_DTMD.DMInfo.DMGosaM + ",";
                                db = new dbConnect((JV_DTMD.id.Year + JV_DTMD.id.MonthDay), JV_DTMD.head.RecordSpec, ref tmp, ref DbReturn);
                            }
                            break;
                        default:
                            JVForm.JvForm_JvSkip();
                            break;

                    }

                    if (DbReturn == 0)
                    {
                        break;
                    }
                }
                else if (ret == 0)
                {
                    /* 全ファイルデータ読み込み終了 */
                    //ProgressStatus.Visibility = Visibility.Hidden;
                    this.MainBack.Fill = System.Windows.Media.Brushes.SeaGreen;
                    InitCompLabelText();    /* 障害Issue#3 */
                    LabelRaceNum.Content = "OK";
                    JvSysMain(JV_SYS_CLOSE_COMP_NOTICE);

                    break;

                }
                else if (ret == -1)
                {
                    /* ファイル切り替わり */
                    ret = 1;
                    continue;
                }
                else
                {
                    break;
                }

            }
            JVForm.JvForm_JvClose();
            return 1;




        } 

        private String ChgToDate(String time, String WeekDay)
        {
            int IntTime = Int32.Parse(time);
            switch(WeekDay)
            {
                case "Monday": //月曜日
                    IntTime = IntTime - 4;
                    break;
                case "Tuesday":
                    IntTime = IntTime - 5;
                    break;
                case "Wednesday":
                    IntTime = IntTime - 6;
                    break;
                case "Thursday":  //木曜日基準
                    break;
                case "Friday":
                    IntTime = IntTime - 1;
                    break;
                case "Saturday":
                    IntTime = IntTime - 2;
                    break;
                case "Sunday":
                    IntTime = IntTime - 3;
                    break;
            }
            return IntTime.ToString();
        }

        private void LogMainOutPutFormThread(object Max)
        {
            Log LogForm = new Log(Int32.Parse(Max.ToString()));
            LogForm.Show();

            int ret = 0;

            LogForm.InitLogData(ProgressStatus);

            while (true)
            {
                LogForm.MaxValue(MaxValue);     //ここでValueが0になる。
                ret = LogForm.LogCntUp(ProgressStatus);
                Thread.Sleep(500); //0.5秒待機

                if(ret != 0) //0は続行。それ以外は終了かエラー
                {
                    break;
                }
            }
        }


    }

    


}
