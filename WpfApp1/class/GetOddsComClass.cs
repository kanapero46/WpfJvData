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
            dbAccess.dbConnect db = new dbAccess.dbConnect();

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

            Boolean InitO1Flag = true;

            /* オッズ */
            JvComDbData.JvDbO1Data O1 = new JvComDbData.JvDbO1Data(Key.Substring(0,8));

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
                            if(InitO1Flag)
                            {
                                db.DeleteCsv("O1", Key.Substring(0, 8) + ".csv", false);
                                db.DeleteCsv("O15", Key.Substring(0, 8) + ".csv", false);
                                db.DeleteCsv("O1", Key.Substring(0, 8) + "/" + "O1" + Key + ".csv");
                                db.DeleteCsv("O15", Key.Substring(0, 8) + "/" + "O15" + Key + ".csv");
                                O1.SetJvDbO1Data(ref buff);

                            }
                            else
                            {
                                O1.SetJvDbO1Data(ref buff);
                            }
                            
                            break;
                    }

            
                }
                else if(ret == 0)
                {

                }
                else if(ret == -1)
                {
                    //ファイル切り替わり
                    ret = 1;
                    continue;
                }
                else
                {
                    break;
                }

            }



            Jv.JvForm_JVWatchEventClose();
            Jv.JvForm_JvClose();

                return 1;
        }
    }
}
