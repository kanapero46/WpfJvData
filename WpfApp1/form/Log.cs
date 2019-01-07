using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp1.form
{
    public partial class Log : Form
    {
        private int MaxValue = 100;
        private Boolean Cancel_Flag = false;

        MainWindow main = new MainWindow();

        public Log()
        {
            InitializeComponent();
        }

        public Log(int Max)
        {
            InitializeComponent();
            MaxValue = Max;
            Cancel_Flag = false;
        }

        /* 初期値に再設定します。 */
        public void SettingMaxValue(int MaxValue)
        {
            progressBar1.Maximum = MaxValue;
            progressBar1.Value = 0;
        }

        public void InitLogData(int InitValue)
        {
            if(InitValue > MaxValue) { return; }
            progressBar1.Maximum = MaxValue;
            progressBar1.Value = InitValue;
        }

        public int LogCntUp(int Value)
        {
            if(progressBar1.Maximum < Value ||progressBar1.Value > Value)
            {
                return -1;
            }

            progressBar1.Value = Value;

            if(progressBar1.Value == progressBar1.Maximum)
            {
                return 1;
            }

            return 0;
        }

        public void AddLogMsg(String msg)
        {
            
        }

        public Boolean CancelFlag()
        {
            return Cancel_Flag;
        }

        private void Log_Load(object sender, EventArgs e)
        {

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Cancel_Flag = false;
            main.LogMainCancelFlagChanger(false);
        }
    }
}
