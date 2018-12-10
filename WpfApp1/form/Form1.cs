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
    public partial class Form1 : Form
    {

        JVForm JV_FORM = new JVForm();
        dbConnect db = new dbConnect();

        public Form1()
        {
            InitializeComponent();
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ret = 1;
            statusBar1.Text = "DataLabに接続中です、";
            if(checkBox1.Checked)
            {
                ret = GetMasterDataYS();
            }
            
            if(ret != 1)
            {
                return;
            }

            if(checkBox2.Checked)
            {
                ret = GetMasterDataBT();
            }
            statusBar1.Text = "選択データを選んでください。";
        }

        private int GetMasterDataYS()
        {
            int ret;
            int LibRet = 0;
            ret = JV_FORM.JvForm_JvInit();
            JVData_Struct.JV_YS_SCHEDULE jVData = new JVData_Struct.JV_YS_SCHEDULE();

            if (ret != 0) { return ret; }

            int rdCount = 0;
            int dlCount = 0;
            String lastTimeCount = "";

            ret = JV_FORM.JvForm_JvOpen("YSCH", "20180101000000", 1, ref rdCount, ref dlCount, ref lastTimeCount);

            /* JvOpenのエラーハンドリング */
            if (ret != 0)
            {
                if(ret == -202)
                {
                    JV_FORM.JvForm_JvClose();
                    ret = JV_FORM.JvForm_JvOpen("YSCH", "20180101000000", 1, ref rdCount, ref dlCount, ref lastTimeCount);
                    if (ret != 0)
                    {
                        JV_FORM.JvForm_JvClose();
                        return 0; /* エラー */
                    }
                }
                else
                {
                    JV_FORM.JvForm_JvClose();
                    return 0; /* エラー */
                }
            }

            String buff = "";
            int size = 20000;
            String fname = "";


            progressBar1.Value = 0;
            ret = 1;
            db.DeleteCsv("YS");

            String tmp = "";

            while (ret >= 1)
            {
                ret = JV_FORM.JvForm_JvRead(ref buff, out size, out fname);

                if (buff == "")
                {
                    continue;
                }

                if (ret > 0)
                {
                    progressBar1.Maximum = ret;
                    progressBar1.Value++;

                    switch (buff.Substring(0, 2))
                    {
                        case "YS":
                            jVData.SetDataB(ref buff);
                            tmp += jVData.id.Year + jVData.id.MonthDay + ",";
                            tmp += jVData.id.Kaiji + ",";
                            tmp += jVData.id.JyoCD + ",";
                            tmp += jVData.id.Kaiji + ",";
                            tmp += jVData.crlf;
                            db = new dbConnect(jVData.id.Year + jVData.id.MonthDay, "YS", ref tmp, ref ret);
                            break;
                    }
                }

                if(ret == 0)
                {
                    break;
                }
            }
            JV_FORM.JvForm_JvClose();
                return 1;
        }


        private int GetMasterDataBT()
        {
            int ret;
            int LibRet = 0;
            ret = JV_FORM.JvForm_JvInit();
            JVData_Struct.JV_BT_KEITO jVData = new JVData_Struct.JV_BT_KEITO();

            if (ret != 0) { return ret; }

            int rdCount = 0;
            int dlCount = 0;
            String lastTimeCount = "";

            ret = JV_FORM.JvForm_JvOpen("BLOD", "00000000000000", 1, ref rdCount, ref dlCount, ref lastTimeCount);

            /* JvOpenのエラーハンドリング */
            if (ret != 0)
            {
                if (ret == -202)
                {
                    JV_FORM.JvForm_JvClose();
                    ret = JV_FORM.JvForm_JvOpen("BLOD", "00000000000000", 1, ref rdCount, ref dlCount, ref lastTimeCount);
                    if (ret != 0)
                    {
                        JV_FORM.JvForm_JvClose();
                        return 0; /* エラー */
                    }
                }
                else
                {
                    JV_FORM.JvForm_JvClose();
                    return 0; /* エラー */
                }
            }

            String buff = "";
            int size = 1000;
            String fname = "";


            progressBar1.Value = 0;
            ret = 1;
            db.DeleteCsv("BT");

            String tmp = "";

            while (ret >= 1)
            {
                ret = JV_FORM.JvForm_JvRead(ref buff, out size, out fname);

                if (buff == "")
                {
                    JV_FORM.JvForm_JvSkip();
                    continue;
                }

                if (ret > 0)
                {
                    progressBar1.Maximum = ret;
                    progressBar1.Value++;

                    switch (buff.Substring(0, 2))
                    {
                        case "BT":
                            jVData.SetDataB(ref buff);
                            tmp += jVData.HansyokuNum + ",";
                            tmp += jVData.KeitoId + ",";
                            tmp += jVData.KeitoName + ",";
                            tmp += jVData.crlf;
                            db = new dbConnect("BT", ref tmp, ref ret);
                            break;
                        default:
                            JV_FORM.JvForm_JvSkip();
                            break;
                    }
                }

                if (ret == 0)
                {
                    continue;
                }
            }
            JV_FORM.JvForm_JvClose();
            return 1;
        }


    }
}
