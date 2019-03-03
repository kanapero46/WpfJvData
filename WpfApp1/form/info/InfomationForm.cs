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
            public int PayEndRaceNum;
        }

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

        RC_STRUCT[] RCNameArray = new RC_STRUCT[3];
        InfomationFormSettingClass SettingClass = new InfomationFormSettingClass();
        BackEndInfomationForm BackEnd = new BackEndInfomationForm();    //バックエンドクラス   
        String[] CourceArray = new string[3];

        public InfomationForm()
        {
            InitializeComponent();
        }

        public InfomationForm(String Date)
        {
            InitializeComponent();
            DateParam = Date;
            Console.WriteLine("InfomatioForm Date = " + Date);

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
            DateParam = DateTime.Today.ToString("yyyyMMdd");


            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> ArrayStr = new List<string>();
            db.TextReader_Row(DateParam, "RA", 0, ref ArrayStr);

            MainWindow main = new MainWindow();
                       
            if(RCNameArray[0].Key == "" && RCNameArray[1].Key == "" && RCNameArray[2].Key == "")
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

            for (int j=0; j<3; j++)
            {
                for(int i=1; i<=12; i++)
                {
                    ret = getOdds.GetOddsCom("0B31", DateParam + ArrayStr[j] + String.Format("{0:00}", i)); //単勝オッズ
                    if(ret != 1)
                    {
                        //エラー
                        break;
                    }

                    if(!getOdds.MappingGetPayFlag())
                    {
                        RCNameArray[j].PayEndRaceNum++;
                        continue; //締切済み・未発売
                    }  
                    else
                    {
                        break;
                    }
                }
            }

            SetPayOutRaceNumForLabel();


        }
        #endregion

        #region 締切済みレース名を表示
        private void SetPayOutRaceNumForLabel()
        {

        }
        #endregion

        #region エラー時のメッセージ表示
        private void ShowErrorMessage(String msg)
        {
            InfomationLabel = new Label[1];
            InfomationLabel[0].Text = msg;
            InfomationLabel[0].Font = new Font("Meiryo UI", 15);
            InfomationLabel[0].Size = new Size(new Point(250, 250));
            InfomationLabel[0].TextAlign = ContentAlignment.MiddleCenter;
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
            Console.WriteLine(e.bstr);  //201902170511の形で入ってくる
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
            Console.WriteLine(e.bstr + "(" + ret + ")");
            if(ret == 1)
            {
                BackEnd.BackEndoGetPayCource(ref Cource, ref RaceNum);
                Cource = BackEnd.BackendMappingCourceName(Cource);

                Console.WriteLine(Cource + RaceNum);
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
    }
}
