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
        dbConnect db = new dbConnect();
        Class.MainDataClass DataClass = new Class.MainDataClass();

        public Syutsuba()
        {
            InitializeComponent();

        }

        public Syutsuba(String RA)
        {
            String tmp = "";

            //DBからレース名を検索
            db.Read_KeyData("RA", RA, RA.Substring(0, 6), 5, ref tmp);
            DataClass.SET_RA_KEY(RA);
            DataClass.setRaceDate(RA.Substring(0, 6));
            DataClass.setRaceCoutce(RA.Substring(6, 2));
            DataClass.setRaceKaiji(RA.Substring(8, 2));
            DataClass.setRaceNichiji(RA.Substring(10, 2));
            DataClass.setRaceNum(int.Parse(RA.Substring(12, 2)));
        }

        /************** private **************/
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        unsafe private void Form2_Load(object sender, EventArgs e)
        {
            String LibTmp = "";
            int CODE = LibJvConvFuncClass.RACE_NAME;

            String tmp = DataClass.getRaceCource();
            LibJvConvFuncClass.jvSysConvFunction(&CODE, tmp, ref LibTmp);
            LabeCource.Text = LibTmp;

            DataClass.getRaceNum().ToString();

        }
    }
}
