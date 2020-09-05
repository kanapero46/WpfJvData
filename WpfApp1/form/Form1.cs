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
    public partial class InitSettingForm : Form
    {

        JVForm JV_FORM = new JVForm();
        dbConnect db = new dbConnect();

        public InitSettingForm()
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
            JVData_Struct.JV_BT_KEITO JVData = new JVData_Struct.JV_BT_KEITO();
            JVData_Struct.JV_HN_HANSYOKU JV_HNSYOKU = new JVData_Struct.JV_HN_HANSYOKU();

            if (ret != 0) { return ret; }

            int rdCount = 0;
            int dlCount = 0;
            String lastTimeCount = "";

            ret = JV_FORM.JvForm_JvOpen("BLOD", "19900101000000", 3, ref rdCount, ref dlCount, ref lastTimeCount);

            /* JvOpenのエラーハンドリング */
            if (ret != 0)
            {
                if (ret == -202)
                {
                    JV_FORM.JvForm_JvClose();
                    ret = JV_FORM.JvForm_JvOpen("BLOD", "19900101000000", 3, ref rdCount, ref dlCount, ref lastTimeCount);
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
            int size = 80000;
            String fname = "";
            int DBreturn = 1;

            progressBar1.Value = 0;
            ret = 1;
            db.DeleteCsv("BT_MST");
            db.DeleteCsv("HN_MST");

            String tmp = "";
            String statspec = "";

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
                    //progressBar1.Maximum = ret;
                    //progressBar1.Value++;

                    switch (buff.Substring(0, 2))
                    {
                        case "HN":　//繁殖馬マスタ
                            JV_HNSYOKU = new JVData_Struct.JV_HN_HANSYOKU();
                            JV_HNSYOKU.SetDataB(ref buff);
                            tmp = "";
                            tmp += JV_HNSYOKU.HansyokuNum + ",";
                            tmp += JV_HNSYOKU.Bamei + ",";
                            tmp += JV_HNSYOKU.SanchiName + ",";
                            tmp += JV_HNSYOKU.KettoNum + ",";
                            tmp += JV_HNSYOKU.HansyokuFNum + ",";
                            tmp += JV_HNSYOKU.HansyokuMNum + ",";
                            db = new dbConnect("0", JV_HNSYOKU.head.RecordSpec, ref tmp, ref DBreturn);
                            break;
                        case "BT":
                            JVData.SetDataB(ref buff);
                            tmp = "";
                            tmp = JVData.HansyokuNum + ",";
                            tmp += JVData.KeitoId + ",";
                            tmp += JVData.KeitoName.Trim() + ",";
                            db = new dbConnect("0",JVData.head.RecordSpec, ref tmp, ref DBreturn);
                            break;
                        default:
                            JV_FORM.JvForm_JvSkip();
                            break;
                    }
                }

                if(DBreturn == 0)
                {
                    break;
                }

                if (ret == 0)
                {
                    continue;
                }
            }
            JV_FORM.JvForm_JvClose();
            return 1;
        }

        private void InitSettingForm_Load(object sender, EventArgs e)
        {
            String LibVer = "";
            WpfApp1.Class.com.JvComClass LOG = new Class.com.JvComClass();


            statusBar1.Text = "library Version [" + LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.GET_VERSION, ref LibVer) + "]";
        }
    }
}
