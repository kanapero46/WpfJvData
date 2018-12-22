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

        public Syutsuba()
        {
            InitializeComponent();

        }

        public Syutsuba(String RA)
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

            InitForm();
            
        }


        /************** private **************/
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        unsafe private void Form2_Load(object sender, EventArgs e)
        {
            /* レース名書き込み */
            String Grade = DataClass.getRaceGrade();

            if(Grade == "一般"||Grade == "特別"||Grade == "")
            {
                LabelCource.Text = Cource + DataClass.getRaceNum() + "Ｒ　" + DataClass.getRaceName();
            }
            else
            {
                LabelCource.Text = Cource + DataClass.getRaceNum() + "Ｒ　" + DataClass.getRaceName() + 
                    "(" + DataClass.getRaceGrade() + ")";
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
    }
}
