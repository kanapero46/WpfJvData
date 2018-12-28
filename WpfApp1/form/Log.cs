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

        public Log()
        {
            InitializeComponent();
        }

        public Log(int Max)
        {
            InitializeComponent();
            MaxValue = Max;
        }

        public void InitLogData(int InitValue)
        {
            progressBar1.Maximum = MaxValue;
            progressBar1.Value = InitValue;
        }

        public int LogCntUp(int Value)
        {
            if(progressBar1.Value > Value)
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

        private void Log_Load(object sender, EventArgs e)
        {

        }
    }
}
