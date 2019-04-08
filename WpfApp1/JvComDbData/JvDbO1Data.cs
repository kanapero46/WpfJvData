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
        private const String SPEC1 = "O1";
        private const String SPEC2 = "O15";

        struct RA_DB_WRITE_STRUCT
        {
            public String Date;
            public String WriteStr;
        }

        RA_DB_WRITE_STRUCT O1Struct = new RA_DB_WRITE_STRUCT();
        RA_DB_WRITE_STRUCT O15Struct = new RA_DB_WRITE_STRUCT();

        public Boolean PayFlag = false;
        public String RacceNum = "";

        public JvDbO1Data()
        {

        }

        unsafe public JvDbO1Data(String Date)
        {
            int ret = 0;

            String tmp = "#単勝オッズ：" + Date;
            db = new dbAccess.dbConnect("O1", ref tmp, ref ret);
        }

        public void InitJvDbO1Data(String Date)
        {
            int ret = 0;

            //String tmp = "#単勝オッズ：" + Date;
            //db = new dbAccess.dbConnect("O1", ref tmp, ref ret);
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
                tmp += o1.HappyoTime.Month + o1.HappyoTime.Day + o1.HappyoTime.Hour + o1.HappyoTime.Minute + ",";
                tmp += o1.TorokuTosu + ",";
                tmp += o1.TansyoFlag + ","; //単勝発売フラグ
                tmp += o1.FukusyoFlag + ",";
                tmp += o1.FukuChakuBaraiKey + ",";
                tmp += o1.TotalHyosuTansyo + ",";
                tmp += o1.TotalHyosuFukusyo + ",";
                tmp += o1.head.DataKubun + ",";

                RacceNum = o1.id.JyoCD;                              //競馬場コード
                PayFlag = (o1.head.DataKubun == "1" ? true : false); //発売フラグをセット
                O1Struct.Date = o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum;
                O1Struct.WriteStr += tmp + "\r\n";
                //db = new dbAccess.dbConnect(o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum, "O1", ref tmp, ref ret);

                //単勝・複勝（複勝は未発売でもロギングする）
                if (o1.TansyoFlag == "7")
                {
                    for (int i = 1; i <= Int32.Parse(o1.TorokuTosu); i++)
                    {
                        if (o1.OddsTansyoInfo[i - 1].Umaban == "    ")
                        {
                            break;
                        }

                        tmp = "";
                        tmp += o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum + o1.OddsTansyoInfo[i - 1].Umaban + ",";
                        tmp += o1.OddsTansyoInfo[i - 1].Umaban + ",";
                        tmp += o1.OddsTansyoInfo[i - 1].Odds + ",";
                        tmp += o1.OddsTansyoInfo[i - 1].Ninki + ",";
                        tmp += o1.OddsFukusyoInfo[i - 1].OddsLow + ",";
                        tmp += o1.OddsFukusyoInfo[i - 1].OddsHigh + ",";
                        tmp += o1.OddsFukusyoInfo[i - 1].Ninki + ",";
                        O1Struct.WriteStr += tmp + "\r\n";
                    }
                }

                //枠連
                tmp2 += o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum + ",";
                tmp2 += o1.TorokuTosu + ",";
                tmp2 += o1.WakurenFlag + ",";
                tmp2 += o1.TotalHyosuWakuren + ",";
                tmp2 += "\r\n";
                O15Struct.Date = o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum;
                O15Struct.WriteStr += tmp2;
                //db = new dbAccess.dbConnect(o1.id.Year + o1.id.MonthDay + o1.id.JyoCD + o1.id.Kaiji + o1.id.Nichiji + o1.id.RaceNum, "O15", ref tmp2, ref ret);

                if (o1.WakurenFlag == "7")
                {
                    for (int j = 0; j < 32; j++) //枠連最大32
                    {
                        if (o1.OddsWakurenInfo[j].Odds == "    ")
                        {
                            j = 99;
                            break;
                        }
                        tmp2 = "";
                        tmp2 += o1.OddsWakurenInfo[j].Kumi + ",";
                        tmp2 += o1.OddsWakurenInfo[j].Odds + ",";
                        tmp2 += o1.OddsWakurenInfo[j].Ninki + ",";
                        tmp2 += "\r\n";
                        O15Struct.WriteStr += tmp2;
                    }

                }
            }
            catch(Exception)
            {

            }
        }

#region 取得したレースの発売・締切フラグ
        public Boolean GetPayStatus()
        {
            // true 発売中
            return PayFlag;
        }
        #endregion

        #region 取得したレースの競馬場コード
        public String GetRaceCourceCode()
        {
            // true 発売中
            return RacceNum;
        }
        #endregion

        #region オッズデータ書き込み処理
        public int ExecO1DbWriter()
        {
            int DbRetrurn = 0;
            int ret = 0;

            Class.com.JvComClass LOG = new Class.com.JvComClass();
            LOG.CONSOLE_TIME_MD("O1", "JvDbExecO1DbWriter!!");

            if(O1Struct.Date == "" || O15Struct.Date == "")
            {
                return -1;
            }
            O1Struct.WriteStr = "#単勝オッズ："+ O1Struct.Date +"\r\n" + O1Struct.WriteStr;
            db = new dbAccess.dbConnect(O1Struct.Date, SPEC1, ref O1Struct.WriteStr, ref DbRetrurn);
            if(DbRetrurn != 0)
            {
                ret++;
            }

            O15Struct.WriteStr = "#枠連オッズ：" + O15Struct.Date + "\r\n" + O15Struct.WriteStr;
            db = new dbAccess.dbConnect(O15Struct.Date, SPEC2, ref O15Struct.WriteStr, ref DbRetrurn);

            if (DbRetrurn != 0)
            {
                ret++;
            }

            return ret;
        }
        #endregion
    }
}
