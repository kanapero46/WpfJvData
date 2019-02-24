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

        InfomationFormSettingClass SettingClass = new InfomationFormSettingClass();
        BackEndInfomationForm BackEnd = new BackEndInfomationForm();    //バックエンドクラス   
        String[] CourceArray = new string[3];

        public InfomationForm()
        {
            InitializeComponent();
        }

        public InfomationForm(String Key)
        {
            InitializeComponent();
            SettingClass.RaKey = Key;
            Console.WriteLine("InfomatioForm SetKey = " + Key);

        }

        private void InfomationForm_Load(object sender, EventArgs e)
        {
            Class.GetOddsComClass getOdds = new Class.GetOddsComClass();
            int ret = 0;

            axJVLink1.JVInit("UNKNOWN");
            axJVLink1.JVWatchEvent();

            for(int i=1; i<=12; i++)
            {
                ret = getOdds.GetOddsCom("0B30", SettingClass.RaKey.Substring(0, 14) + String.Format("{0:00}", i));
            }

            if (db.TextReader_Col(RaClassData.GET_RA_KEY(), "O1", 0, ref O1, RaClassData.GET_RA_KEY() + string.Format("{0:00}", i)) != 0)
            {

            }

        }

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
