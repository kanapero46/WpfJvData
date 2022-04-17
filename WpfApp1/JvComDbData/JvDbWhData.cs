using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.JvComDbData;

namespace WpfApp1.JvComDbData
{
    class JvDbWhData
    {
        const String SPEC = "WH";
        const String ID = "0B11";

        dbAccess.dbConnect db = new dbAccess.dbConnect();
        Class.com.JvComMain LOG = new Class.com.JvComMain();

        String dbData = "";
        String Key = "";

        public JvDbWhData()
        {

        }

        /* 馬体重データをJv-Linkから取得 */
        public int JvDbWhLinkData( String Key )
        {
            int ret;
            WpfApp1.JVForm Jv = new JVForm();
            MainWindow main = new MainWindow();
            dbAccess.dbConnect db = new dbAccess.dbConnect();

            if (Jv.JvForm_JvInit() != 0)
            {
                return 0;
            }

            ret = Jv.JvForm_JVWatchEvent();
            ret = Jv.JvForm_JvRTOpen(ID, Key);

            if (ret != 0)
            {
                //System.Windows.MessageBox.Show("DataLabサーバに接続出来ませんでした。\nSC-" + ret, "JVRTOpenエラー2");
                Jv.JvForm_JVWatchEventClose();     //速報系スレッドの終了
                Jv.JvForm_JvClose();
                return 0;
            }

            ret = 1;
            String buff = "";
            int size = 20000;
            String fname = "";

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
                        case SPEC:
                            JvDbWhSetData(ref buff);
                            break;
                    }
                }
                else if (ret == 0)
                {

                }
                else if (ret == -1)
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
            JvDbWhWriteData();
            Jv.JvForm_JVWatchEventClose();
            Jv.JvForm_JvClose();
            return 1;
        }

        public int JvDbWhSetData( ref String Data )
        {
            int ret = 0;
            JVData_Struct.JV_WH_BATAIJYU JvWh = new JVData_Struct.JV_WH_BATAIJYU();

            String tmp = "";

            if( Data == "" )
            {
                LOG.CONSOLE_TIME_MD(SPEC, "JvDbWhSetData InParam Error!!");
                return 0;
            }

            if(tmp.Length > (int.MaxValue-1000))
            {
                //一旦書き込み
                JvDbWhWriteData();
            }

            try
            {

                JvWh.SetDataB(ref Data);
                Key = JvWh.id.Year + JvWh.id.MonthDay + JvWh.id.JyoCD + JvWh.id.Kaiji + JvWh.id.Nichiji + JvWh.id.RaceNum;
                tmp += Key + ","; //キー
                tmp += JvWh.id.JyoCD + ",";
                tmp += JvWh.id.RaceNum + ",";
                tmp += String.Join(JvWh.HappyoTime.Month, JvWh.HappyoTime.Day, JvWh.HappyoTime.Month + JvWh.HappyoTime.Minute);
                tmp += JvWh.crlf;

                for(int i=0; i<JvWh.BataijyuInfo.Length; i++)
                {
                    tmp += JvWh.id.Year + JvWh.id.MonthDay + JvWh.id.JyoCD + JvWh.id.Kaiji + JvWh.id.Nichiji + JvWh.id.RaceNum + JvWh.BataijyuInfo[i].Umaban + ",";
                    tmp += JvWh.BataijyuInfo[i].Umaban + ",";
                    tmp += JvWh.BataijyuInfo[i].BaTaijyu + ","; //馬体重
                    if(JvWh.BataijyuInfo[i].ZogenSa == "000")
                    {
                        tmp += "±" + ",";//増減符号
                    }
                    else
                    {
                        tmp += JvWh.BataijyuInfo[i].ZogenFugo + ",";//増減符号
                    }
                    
                    tmp += JvWh.BataijyuInfo[i].ZogenSa + ",";  //数値じゃない場合もあり→スペース(出走取消・初出走)
                    tmp += JvWh.crlf;
                }

                dbData += tmp;
            }
            catch(Exception)
            {
                return 0;
            }

            return 1;
        }

         //DB書き込み
        public int JvDbWhWriteData()
        {
            int DbReturn = 0;
            int ret = 0;
            LOG.CONSOLE_TIME_MD(SPEC, "JvDbWhWriteData!!");

            if(dbData == "" || Key == "")
            {
                return 0;
            }

            int ret2 = db.DeleteCsv(SPEC, SPEC + Key + ".csv");
            db = new dbAccess.dbConnect(Key, SPEC, ref dbData, ref DbReturn);
            if (DbReturn != 0)
            {
                ret++;
            }
            LOG.CONSOLE_TIME_MD(SPEC, "dbRet->" + DbReturn);

            return ret;
        }

        /* キーで指定したレースの馬体重情報があるかをチェックする */
        public Boolean JvWhCheckData( String Key )
        {
            /* レース情報・各馬情報以外のキーはエラーとする */
            if(Key == "" || (Key.Length <= 14))
            {
                LOG.CONSOLE_MODULE(SPEC, "Assert! KeyError1!!;");
                return false;
            }

            List<String> tmpData = new List<string>();
            db.TextReader_Col(Key, SPEC, 0, ref tmpData, Key);
            
            if(tmpData.Count == 0)
            {
                return false;
            }
            return true;
        }

        /* 1頭分の馬体重データを取得 */
        public JvWhData JvWhGetData(String Key)
        {
            JvWhData ret = new JvWhData();
            /* レース情報・各馬情報以外のキーはエラーとする */
            if (Key == "" || (Key.Length >= 16))
            {
                LOG.CONSOLE_MODULE(SPEC, "Assert! KeyError2!!;");
                return ret;
            }

            List<String> tmpData = new List<string>();
            db.TextReader_Col(Key.Substring(0, 8), SPEC, 0, ref tmpData, Key);
            if(tmpData.Count == 0)
            {
                return ret;
            }

            ret.Key1 = tmpData[0];
            ret.Umaban1 = Int32.Parse(tmpData[1]);
            ret.Bataiju1 = Int32.Parse(tmpData[2]);
            ret.Fugo1 = tmpData[3];
            ret.Zogensa1 = tmpData[4];

            return ret;
        }

        /* 全頭分の馬体重データを取得 */
        public List<JvWhData> JvWhGetAllData( String RaceKey )
        {
            JvWhData[] tmpRet = new JvWhData[18];
            List<JvWhData> ret = new List<JvWhData>();
            /* レース情報以外のキーはエラーとする→馬番まで指定してもエラーとする。 */
            if (RaceKey == "" || RaceKey.Length != 16)
            {
                LOG.CONSOLE_MODULE(SPEC, "Assert! KeyError2!!;");
                return ret;
            }

            List<String> tmpData = new List<string>();

            for(int i = 1; i < 19; i++)
            {
                tmpData = new List<string>();
                db.TextReader_Col(RaceKey, SPEC, 0, ref tmpData, RaceKey + String.Format("{0:00}", i));
                if(tmpData.Count == 0)
                {
                    break;
                }

                tmpRet[i - 1] = new JvWhData();
                tmpRet[i-1].Umaban1 = Int32.Parse(tmpData[1]);
                tmpRet[i - 1].Bataiju1 = Int32.Parse(tmpData[2]);
                tmpRet[i - 1].Fugo1 = tmpData[3];
                tmpRet[i - 1].Zogensa1 = tmpData[4];

                ret.Add(tmpRet[i - 1]);
            }

            return ret;
        }

    }
}
