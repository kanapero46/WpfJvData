using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Class
{
    class GetOddsComClass
    {
        public int GetOddsCom(String Odds, String Key)
        {
            int ret;
            WpfApp1.JVForm Jv = new JVForm();
            MainWindow main = new MainWindow();

            if(Jv.JvForm_JvInit() != 0)
            {
                return -1;
            }

            ret = Jv.JvForm_JVWatchEvent();
            ret = Jv.JvForm_JvRTOpen(Odds, Key);

            if (ret != 0)
            {
                System.Windows.MessageBox.Show("DataLabサーバに接続出来ませんでした。\nSC-" + ret, "JVRTOpenエラー2");
                Jv.JvForm_JVWatchEventClose();     //速報系スレッドの終了
                Jv.JvForm_JvClose();
                return 0;
            }
            
            ret = 1;
            String buff = "";
            int size = 20000;
            String fname = "";

            /* オッズ */
            JVData_Struct.JV_O1_ODDS_TANFUKUWAKU O1 = new JVData_Struct.JV_O1_ODDS_TANFUKUWAKU();

            while (ret >= 1)
            {
                ret = Jv.JvForm_JvRead(ref buff, out size, out fname);

                if (ret > 0)
                {
                    if (buff == "")
                    {
                        continue;
                    }

                    switch (buff.Substring(0, 2))
                    {
                        case "O1":
                            
                            break;
                    }
                }

            }





                return 1;
        }
    }
}
