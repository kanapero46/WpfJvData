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
    public partial class Kettou : Form
    {
        public Kettou()
        {
            InitializeComponent();
            InitComponentText();
        }

        /** **********private*********** */
        private void InitComponentText()
        {
            pictureBox1.Controls.Add(BloodHorceName);
            pictureBox1.Controls.Add(FBooldName);
            pictureBox1.Controls.Add(BMSHorceName);
            pictureBox1.Controls.Add(FFBloodName);
            pictureBox1.Controls.Add(BMSType);
            pictureBox1.Controls.Add(MMFBooldName);
            pictureBox1.Controls.Add(FTypeName);
            pictureBox1.Controls.Add(FFMTypeName);
            BloodHorceName.BackColor = Color.Transparent;
            FBooldName.BackColor = Color.Transparent;
            BMSHorceName.BackColor = Color.Transparent;
            FFBloodName.BackColor = Color.Transparent;
            BMSType.BackColor = Color.Transparent;
            MMFBooldName.BackColor = Color.Transparent;
            FTypeName.BackColor = Color.Transparent;
            FFMTypeName.BackColor = Color.Transparent;
            this.BloodHorceName.Top = (this.pictureBox1.Height - this.BloodHorceName.Height) / 2 - 10;
            this.BloodHorceName.Left = (this.pictureBox1.Width - this.BloodHorceName.Width) / 15 - 19;
            this.FBooldName.Top = (this.pictureBox1.Height - this.FBooldName.Height) / 3 - 5;
            this.FBooldName.Left = (this.pictureBox1.Width - this.FBooldName.Width) / 2 + 12;
            this.BMSHorceName.Top = (this.pictureBox1.Height - this.BMSHorceName.Height) - ((this.pictureBox1.Height - this.BMSHorceName.Height) / 3 - 10);
            this.BMSHorceName.Left = (this.pictureBox1.Width - this.BMSHorceName.Width) / 2 + 12;
            this.FFBloodName.Top = (this.pictureBox1.Height - this.FFBloodName.Height) / 10;
            this.FFBloodName.Left = (this.pictureBox1.Width - this.FFBloodName.Width);
            this.BMSType.Top = (this.pictureBox1.Height - this.FFBloodName.Height) / 2;
            this.BMSType.Left = (this.pictureBox1.Width - this.FFBloodName.Width);
            this.MMFBooldName.Top = (this.pictureBox1.Height - this.MMFBooldName.Height) - ((this.pictureBox1.Height - this.MMFBooldName.Height) / 4 - 25);
            this.MMFBooldName.Left = (this.pictureBox1.Width - this.MMFBooldName.Width);
            this.FTypeName.Top = this.FFBloodName.Top + 43;
            this.FTypeName.Left = (this.pictureBox1.Width - this.FTypeName.Width);
            this.FFMTypeName.Top = this.MMFBooldName.Top + 41;
            this.FFMTypeName.Left = (this.pictureBox1.Width - this.FFMTypeName.Width);
        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void Kettou_Load(object sender, EventArgs e)
        {


        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BloodHorceName_Click(object sender, EventArgs e)
        {

        }
    }
}
