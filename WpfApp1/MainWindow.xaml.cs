using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibJvConv;
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
            int ret;
            ret = JVForm.JvForm_JvInit();

            String fromtime = CngDataToString(DateText.SelectedDate.Value.ToShortDateString());
            int op = 2;
            int rdCount = 0;
            int dlCount = 0;
            String lastStamp = "";            

            if (ret != 0) {
                MessageBox.Show("sidが正しくありません");
                return 0;
            }

            LogText.Visibility = Visibility.Collapsed;
            LogingText("JvInit・・・OK\n");
            ret = JVForm.JvForm_JvOpen("RACE", fromtime + "000000", op, ref rdCount, ref dlCount, ref lastStamp);
           
            /* JvOpenエラーハンドリング */
            if (ret != 0)
            {
                ret = CheckJvOpenRetErr(ret);

                if (ret != 1)
                {
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
                        JVForm.JvForm_JvClose();
                        return 0; /* エラー */
                    }

                }
            }
                /* JvRead用変数の初期化 */
                String buff = "";
                ret = 1;
                int size = 80000;
                String fname = "";

                /* JRA-VAN DataLab構造体 */
                JVData_Struct.JV_RA_RACE JV_RACE;
            JVData_Struct.JV_SE_RACE_UMA JV_SE_UMA;
                String tmp;
                String LibTmp = "";
                int CODE;
            int DbReturn = 1;

            /* DB初期化 */
            db = new dbConnect();
            db.DeleteCsv("RA");
            db.DeleteCsv("SE");

            /* ロギング */
            LogingText("JvRead・・・OK\n");

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
                            LogingText("*");
                            JV_RACE = new JVData_Struct.JV_RA_RACE();
                            tmp = "";
                            JV_RACE.SetDataB(ref buff);
                            tmp = JV_RACE.id.Year + JV_RACE.id.MonthDay + ",";
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
                            db = new dbConnect((JV_RACE.id.Year + JV_RACE.id.MonthDay), JV_RACE.head.RecordSpec, ref tmp, ref DbReturn);
                            break;
                        case "SE":
                            LogingText("@");
                            JV_SE_UMA = new JVData_Struct.JV_SE_RACE_UMA();
                            tmp = "";
                            JV_SE_UMA.SetDataB(ref buff);
                            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + ",";
                            tmp += JV_SE_UMA.id.JyoCD + ",";
                            tmp += JV_SE_UMA.id.Kaiji + ",";
                            tmp += JV_SE_UMA.id.Nichiji + ",";
                            tmp += JV_SE_UMA.Wakuban + ",";
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
                            db = new dbConnect((JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay), JV_SE_UMA.head.RecordSpec, ref tmp, ref DbReturn);

                            break;
                        default:
                            JVForm.JvForm_JvSkip();
                            break;
                    }

                    if(DbReturn == 0)
                    {
                        break;
                    }  
                }
                else if (ret == 0)
                {
                    /* 全ファイルデータ読み込み終了 */
                    ProgressStatus.Visibility = Visibility.Hidden;
                    MainBack.Fill = System.Windows.Media.Brushes.SeaGreen;
                    LabelRaceNum.Content = "OK"; 
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

            /* 場名リスト更新 */
            AddComboBox(fromtime);

       JVForm.JvForm_JvClose();

            return 0;
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
            db.TextReader(fromtime, "RA", 6, ref RaceName);
            db.TextReader(fromtime, "RA", 1, ref Cource);
            db.TextReader(fromtime, "RA", 4, ref RaceNum);

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
                    MessageBox.Show("取得したデータがありませんでした。");
                    return 0;
                case -2:
                    MessageBox.Show("キャンセルしました。");
                    return 0;
                case -111:
                case -112:
                case -114:
                case -115:
                case -116:
                    MessageBox.Show("Jvopenでのパラメータが不正です。[E:" + ret + "]");
                    return 0;
                case -201:
                    MessageBox.Show("JvInitが行われていない");
                    return 0;
                case -202:
                    JVForm.JvForm_JvClose();
                    return 1;   /* 処理続行 */
                case -211:
                    MessageBox.Show("DataLab内部エラーです。[E:211]");
                    break;
                case -301:
                    MessageBox.Show("認証に失敗しました。[E:301]。");
                    return 0;
                case -302:
                    MessageBox.Show("DataLabサービスキーの有効期限が切れています。\nサービス権を購入してください。");
                    return 0;
                case -303:
                    MessageBox.Show("DataLabサービスキーが設定されていません。");
                    return 0;
                case -401:
                    MessageBox.Show("JV-Link内部エラーです。");
                    return 0;
                case -411:
                case -412:
                case -413:
                case -421:
                case -431:
                    MessageBox.Show("サーバーエラーです。[E:" + ret + "]");
                    return 0;
                case -501:
                    MessageBox.Show("スタートキットが無効です。");
                    return 0;
                case -504:
                    MessageBox.Show("JRA-VANサーバーがメンテナンス中です。");
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
            SolidColorBrush solidColor = new SolidColorBrush();
            String RaceCource = RaceListBox.Text.Substring(0, 2);
            
            switch(RaceCource)
            {
                case "中山":
                    MainBack.Fill = System.Windows.Media.Brushes.Blue;
                    break;
                case "阪神":
                    MainBack.Fill = System.Windows.Media.Brushes.Green;
                    break;
            }
        }
        
        private void ComboBoxAddList(String[] RaceName)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SolidColorBrush solidColor = new SolidColorBrush();
            String RaceCource ="";
            Boolean East = false;
            Boolean West = false;
            int Select = RaceListBox.SelectedIndex;
            try
            {
                RaceCource = RaceListBox.SelectedItem.ToString().Substring(0, 2);
            }
            catch (NullReferenceException ex) { }
            
            String fromtime = CngDataToString(DateText.SelectedDate.Value.ToShortDateString());
            List<String> TextArray = new List<string>();
            db = new dbConnect();


            /* 夏競馬開催「 */
            db.TextReader(fromtime, "RA", 1, ref TextArray);
            for(int i = 0; i < TextArray.Count; i++)
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



            /* レース開催日は+1する */
            //fromtime = (Int32.Parse(fromtime) + 1).ToString();

            try
            {
                /* 競馬場名入力 */
                LabelCource.Content = RaceCource;

                /* レース番号入力 */
                db.TextReader(fromtime, "RA", 4, ref TextArray);
                LabelRaceNum.Content = TextArray[Select].ToUpper() + "Ｒ";

                TextArray.Clear();

                /* レース名 */
                db.TextReader(fromtime, "RA", 6, ref TextArray);
                LabelRaceName.Content = TextArray[Select];

                /* グレード */
                TextArray.Clear();
                db.TextReader(fromtime, "RA", 15, ref TextArray);
                if (!(TextArray[Select] == "一般" || TextArray[Select] == "特別"))
                {
                    LabelRaceName.Content += "（" + TextArray[Select] + "）";
                }
            }catch(ArgumentOutOfRangeException)
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (OffLineFlag.IsVisible)
            {
                if(DateText.SelectedDate == null) { MessageBox.Show("レース日を入力してください。"); return; }
                AddComboBox(CngDataToString(DateText.SelectedDate.Value.ToShortDateString()));
            }
            else
            {
                InitMain();
            }
        }

        private void Button_Click_InitData(object sender, RoutedEventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private int JvMainErrorFunction(int res)
        {
            ProgressStatus.Visibility = Visibility.Hidden;

            return res;
        }

        private void LogingText(String Log)
        {
            LogText.Text += Log;
        }
    }

}
