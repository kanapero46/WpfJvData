using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    class JvDbO1Data
    {
        dbAccess.dbConnect db;

        unsafe public JvDbO1Data(String Date)
        {
            int ret = 0;
            String tmp = "#単勝オッズ：" + Date;
            db = new dbAccess.dbConnect("O1", ref tmp, ref ret);
        }

        public void SetJvDbO1Data(ref String buff)
        {
            int ret = 0;
            String tmp = "";
            String tmp2 = "";
            JVData_Struct.JV_O1_ODDS_TANFUKUWAKU o1 = new JVData_Struct.JV_O1_ODDS_TANFUKUWAKU();

            try
            {
                o1.SetDataB(ref buff);
                tmp += o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum + ",";
                tmp += o1.TorokuTosu + ",";
                tmp += o1.TansyoFlag + ","; //単勝発売フラグ
                tmp += o1.FukusyoFlag + ",";
                tmp += o1.FukuChakuBaraiKey + ",";
                tmp += o1.TotalHyosuTansyo + ",";
                tmp += o1.TotalHyosuFukusyo + ",";

                //一旦改行
                tmp += "\n";

                for (int i=1; i<=Int32.Parse(o1.TorokuTosu); i++)
                {
                    if (o1.OddsTansyoInfo[i - 1].Umaban == "    ")
                    {
                        break;
                    }

                    tmp += o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum + o1.OddsTansyoInfo[i - 1].Umaban + ",";
                    tmp += o1.OddsTansyoInfo[i -1].Umaban + ",";
                    tmp += o1.OddsTansyoInfo[i - 1].Odds + ",";
                    tmp += o1.OddsTansyoInfo[i - 1].Ninki + ",";
                    tmp += o1.OddsFukusyoInfo[i - 1].OddsLow + ",";
                    tmp += o1.OddsFukusyoInfo[i - 1].OddsHigh + ",";
                    tmp += o1.OddsFukusyoInfo[i - 1].Ninki + ",";
                    db = new dbAccess.dbConnect(o1.id.Year + o1.id.MonthDay, o1.head.DataKubun, ref tmp, ref ret);
                    if(ret == 0) { break; }
                    
                }

                //複勝
                tmp2 += o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum + ",";
                tmp2 += o1.TorokuTosu + ",";
                tmp2 += o1.WakurenFlag + ",";
                tmp2 += o1.TotalHyosuWakuren + ",";
                tmp += "\n";

                for(int j = 0; j < 32 ;j++) //枠連最大32
                {
                    if (o1.OddsWakurenInfo[j].Odds == "    ")
                    {
                        break;
                    }

                    tmp2 += o1.OddsWakurenInfo[j].Kumi + ",";
                    tmp2 += o1.OddsWakurenInfo[j].Odds + ",";
                    tmp2 += o1.OddsWakurenInfo[j].Ninki+ ",";
                    db = new dbAccess.dbConnect(o1.id.Year + o1.id.MonthDay, o1.head.DataKubun+"5", ref tmp2, ref ret);
                    if (ret == 0) { break; }
                }

            }
            catch(Exception)
            {

            }
        }
    }
}
