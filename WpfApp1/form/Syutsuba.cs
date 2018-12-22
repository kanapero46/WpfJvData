using LibJvConv;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;   
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.dbAccess;

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
            db.Read_KeyData("RA", RA, RA.Substring(0, 8), 5, ref tmp);
            DataClass.SET_RA_KEY(RA);
            DataClass.setRaceDate(RA.Substring(0, 8));
            DataClass.setRaceCoutce(RA.Substring(8, 2));
            DataClass.setRaceKaiji(RA.Substring(10, 2));
            DataClass.setRaceNichiji(RA.Substring(12, 2));
            DataClass.setRaceNum(RA.Substring(14,2));
            db.Read_KeyData("RA", RA, RA.Substring(0, 8), 7, ref tmp);
            DataClass.setRaceName(tmp);
            /* グレード */
            db.Read_KeyData("RA", RA, RA.Substring(0, 8), 16, ref tmp);
            if (!(tmp == "")){ DataClass.setRaceGrade(tmp); }
            CourceColor = Color;
            InitForm();
            InitHorceData();
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

            String SE_KEY = DataClass.GET_RA_KEY();
            String tmp = "";
            for(int i = 1; ;i++)
            {
                pHorceClasses = new Class.MainDataHorceClass();
                String covData = String.Format("{0:00}", i);

                db.Read_KeyData("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), 0, ref tmp);
                if (tmp == "0"|| tmp == "") { break; }
                else if (tmp.Substring(0,16) != SE_KEY) { break; }
                pHorceClasses.KEY1 = tmp;

                db.Read_KeyData("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_WAKU, ref tmp);
                pHorceClasses.Waku1 = tmp;

                db.Read_KeyData("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_UMA, ref tmp);
                pHorceClasses.Umaban1 = tmp;

                db.Read_KeyData("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_KETTO, ref tmp);
                pHorceClasses.KettoNum1 = Int32.Parse(tmp);

                db.Read_KeyData("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_NAME, ref tmp);
                pHorceClasses.Name1 = tmp;

                db.Read_KeyData("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_FUTAN, ref tmp);
                pHorceClasses.Futan1 = tmp.Substring(0,2) + "." + tmp.Substring(2,1);

                db.Read_KeyData("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_JOCKEY, ref tmp);
                pHorceClasses.Jockey1 = tmp;

                db.Read_KeyData("SE", SE_KEY + covData, SE_KEY.Substring(0, 8), SE_MINARA, ref tmp);
                
                CODE = LibJvConv.LibJvConvFuncClass.JOCKEY_MINARAI_CD;
                LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref tmp);
                pHorceClasses.MinaraiCd1 = tmp;
                



                /* 書き込み */
                dataGridView1.Rows.Add(pHorceClasses.Waku1, pHorceClasses.Umaban1, pHorceClasses.Name1, pHorceClasses.MinaraiCd1,
                    pHorceClasses.Jockey1, pHorceClasses.Futan1, "");
            }


        }

        unsafe private void Form2_Load(object sender, EventArgs e)
        {
            /* レース名書き込み */
            String Grade = DataClass.getRaceGrade();

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

        }

        unsafe private void InitForm()
        {
            String LibTmp = "";
            int CODE = LibJvConvFuncClass.COURCE_CODE;

            String tmp = DataClass.getRaceCource();
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            Cource = LibTmp;

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

            return (Cource);
        }

        private void LabelCource_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
