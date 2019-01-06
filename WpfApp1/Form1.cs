using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp1
{
    public partial class JVForm : Form
    {
        public JVForm()
        {
            InitializeComponent();
        }

        public int JvForm_JvInit()
        {
            return (JvLinkClass.JVInit("UNKNOWN"));
        }

        public int JvForm_JvOpen(String DtSpec, String fromTime, int op, ref int rdCount, ref int downloadcount, ref string lastfiletimestamp)
        {
            return (JvLinkClass.JVOpen(DtSpec, fromTime, op, ref rdCount, ref downloadcount, out lastfiletimestamp));
        }

        public int JvForm_JvRTOpen(String DtSpec, String key)
        {
            return (JvLinkClass.JVRTOpen(DtSpec, key));
        }

        public int JvForm_JvRead(ref String buff, out int size, out String fname)
        {
            return (JvLinkClass.JVRead(out buff, out size, out fname));
        }

        public int JvForm_JvClose()
        {
            return (JvLinkClass.JVClose());
        }

        public void JvForm_JvSkip()
        {
            JvLinkClass.JVSkip();
        }

        public int JvForm_JVWatchEvent()
        {
            return (JvLinkClass.JVWatchEvent());
        }

        public int JvForm_JVWatchEventClose()
        {
            return (JvLinkClass.JVWatchEventClose());
        }

        private void JVForm_Load(object sender, EventArgs e)
        {

        }
    }
}
