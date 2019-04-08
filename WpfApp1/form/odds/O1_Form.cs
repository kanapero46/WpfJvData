using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp1.form.odds
{
    public partial class O1_Form : Form
    {
        private String Key;

        public O1_Form()
        {
            InitializeComponent();
        }

        public O1_Form(String RaKey)
        {
            InitializeComponent();
            Key = RaKey;
        }

        private void O1_Form_Load(object sender, EventArgs e)
        {

        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
