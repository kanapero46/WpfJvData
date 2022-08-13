using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Class;
using WpfApp1.dbAccess;

namespace WpfApp1.JvComDbData
{

    public class JvDbSEData : MainDataHorceClass
    {
        const int RA_MAX = 34;
        dbConnect db = new dbConnect();
        private const String MAGIC_STR = "B";
        private const String SPEC = "SE";
        String statHorce = "";

        struct SE_DB_WRITE_STRUCT
        {
            public String Date;
            public String WriteStr;
        }

        SE_DB_WRITE_STRUCT SeStruct = new SE_DB_WRITE_STRUCT();
        Class.com.JvComClass LOG = new Class.com.JvComClass();
       
        public JvDbSEData()
        {
            //引数なしのインスタンス生成時はDB書き込み用の文字列を初期化
            SeStruct = new SE_DB_WRITE_STRUCT();
        }

        #region 当回SEデータ保存用マッピング関数
        public JvDbSEData(ref String buff)
        {
            JvDbSeComFunction(-1, ref buff);    //当回データ
        }
        #endregion

        #region マスターSEデータ保存用マッピング関数
        public JvDbSEData(int Master, ref String buff, int DiffName)
        {
            JvDbSeComFunction(Master, ref buff);
        }
        #endregion

        #region SEデータ用マッピング関数
        public void JvDbSeComMappingFunction(int ret, ref String buff)
        {
            JvDbSeComFunction(ret, ref buff);
        }
        #endregion

        #region SEデータに書き込みデータ有り無し取得関数
        public Boolean JvDbSeComDataEnable()
        {
            if(SeStruct.Date == null || SeStruct.WriteStr == null)
            {
                return false;
            }
            else if(SeStruct.Date.Length == 0 || SeStruct.WriteStr.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region 共通
        private void JvDbSeComFunction(int Master, ref String buff)
        {
            JVData_Struct.JV_SE_RACE_UMA JV_SE_UMA = new JVData_Struct.JV_SE_RACE_UMA();
            String tmp = "";
            int OldRaceCounter = 0;
            int DbReturn = 0;

            JV_SE_UMA.SetDataB(ref buff);

            //SE_MSTはKey+○走目をキーとするため、前回と馬名が一致するかを検索
            if (Master >= 0)
            {
                //前回保存したデータと不整合ない
                OldRaceCounter = Master;
            }
            else if(Master == -1)
            {
                //当回データ→OldCounterを書き込まない
                OldRaceCounter = 0;
            }
            else
            {
                //前回保存したデータと不整合ない
                OldRaceCounter = 1;
                statHorce = JV_SE_UMA.Bamei.Trim();
            }

            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + JV_SE_UMA.id.JyoCD + JV_SE_UMA.id.Kaiji +
                JV_SE_UMA.id.Nichiji + JV_SE_UMA.id.RaceNum + JV_SE_UMA.Umaban + ( OldRaceCounter == 0 ? "" :String.Format("{0:00}", OldRaceCounter)) + ",";
            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + ",";
            tmp += JV_SE_UMA.id.JyoCD + ",";
            tmp += JV_SE_UMA.id.Kaiji + ",";
            tmp += JV_SE_UMA.id.Nichiji + ",";
            tmp += JV_SE_UMA.Wakuban + ",";
            tmp += JV_SE_UMA.Umaban + ",";
            tmp += JV_SE_UMA.KettoNum + (OldRaceCounter == 0 ? "" : String.Format("{0:00}", OldRaceCounter)) + ",";
            tmp += JV_SE_UMA.Bamei.Trim() + ",";
            tmp += JV_SE_UMA.UmaKigoCD + ",";
            tmp += JV_SE_UMA.SexCD + ",";
            tmp += JV_SE_UMA.Barei + ",";
            tmp += JV_SE_UMA.KeiroCD + ",";
            tmp += JV_SE_UMA.Futan + ",";
            tmp += JV_SE_UMA.Blinker + ",";
            tmp += JV_SE_UMA.KisyuCode + ",";
            tmp += JV_SE_UMA.KisyuRyakusyo + ",";
            tmp += JV_SE_UMA.MinaraiCD + ",";
            tmp += JV_SE_UMA.KakuteiJyuni + ","; //18
            tmp += JV_SE_UMA.DMTime + ",";
            tmp += JV_SE_UMA.DMJyuni + ",";      //20
            tmp += JV_SE_UMA.IJyoCD + ",";
            tmp += JV_SE_UMA.ChakuUmaInfo[0].Bamei + ",";
            tmp += JV_SE_UMA.Time + ",";
            tmp += JV_SE_UMA.TimeDiff + ",";
            tmp += JV_SE_UMA.Ninki + ",";        //25
            tmp += JV_SE_UMA.Jyuni1c + JV_SE_UMA.Jyuni2c + JV_SE_UMA.Jyuni3c + JV_SE_UMA.Jyuni4c + ",";
            tmp += JV_SE_UMA.HaronTimeL3 + ",";
            tmp += JV_SE_UMA.ChakusaCD + ",";
            tmp += JV_SE_UMA.ChakusaCDP + ",";
            tmp += JV_SE_UMA.Odds + ","; //30
            tmp += JV_SE_UMA.ChokyosiRyakusyo + ","; //31
            tmp += JV_SE_UMA.ChokyosiCode + ","; //32
            tmp += "\r\n";

            SeStruct.WriteStr += tmp;
            
            /* ここでは書き込まないように変更する、そのため別途書き込み処理をコールする必要あり。 */
            if(SeStruct.Date == "" || SeStruct.Date == null)
            {
                SeStruct.Date = JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay;
            }
            while(false)
            {
                if (Master == -1)
                {

                    dbConnect db = new dbConnect(JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay, JV_SE_UMA.head.RecordSpec, ref tmp, ref DbReturn);
                }
                else
                {
                    /* マスターデータ */
                    dbConnect db = new dbConnect("0", JV_SE_UMA.head.RecordSpec, ref tmp, ref DbReturn);
                }
            }


        }
        #endregion

        #region SEデータのDB書き込み指示
        public int ExecSEDataWriteDb(int kind)
        {
            int DbReturn = 0;

#if DEBUG
            LOG.CONSOLE_TIME_MD("SE", "JvDbSeData Write Start!!");
#endif

            if (SeStruct.Date == "" || SeStruct.WriteStr.Length == 0)
            {
                LOG.CONSOLE_MODULE("SE", "DataSetERror!");
                return 0;
            }
            
            if(kind == 0)
            {
                //マスターデータ
                db.DeleteCsv("SE_MST");
                db = new dbConnect("0", SPEC, ref SeStruct.WriteStr, ref DbReturn);
            }
            else
            {
               // db.DeleteCsv("RA");
                db = new dbConnect(SeStruct.Date, SPEC, ref SeStruct.WriteStr, ref DbReturn);
            }

            LOG.CONSOLE_TIME_MD("SE", "JvDbSeData DB Write -> " + SeStruct.Date +" ret(" + DbReturn + ")");
            SeStruct.Date = "";
            SeStruct.WriteStr = "";
            
            return DbReturn;
        }
        #endregion
        
        #region SEデータとRAデータを組み合わせ
        public void SetSEMSTData(List<String> inParam)
        {
            //新馬戦は前走成績がないため、returnする。
            if (inParam.Count == 0) { return; }

            JV_DATA_RACE_HIST tmpHist = new JV_DATA_RACE_HIST();
            tmpHist.Num = Int32.Parse(inParam[0].Substring(18, 2));　
            tmpHist.rA_KEY = inParam[0].Substring(0, 18);
            tmpHist.sE_KEY = inParam[0];
            tmpHist.wakuban = Int32.Parse(inParam[5]);
            tmpHist.umaban = Int32.Parse(inParam[6]);
            tmpHist.jockey = inParam[16];
            tmpHist.futan = inParam[13];
            tmpHist.Blincker = (inParam[14] == "1" ? true : false);
            tmpHist.MinaraiCd = inParam[17];
            tmpHist.rank = inParam[18];
            tmpHist.DMTime = inParam[19];
            tmpHist.DMRank = Int32.Parse(inParam[20]);
            tmpHist.TorikeshiCd = inParam[21];
            tmpHist.aiteuma = inParam[22];
            tmpHist.time = inParam[23];
            tmpHist.timeDiff = inParam[24];
            tmpHist.Ninki = inParam[25];
            tmpHist.courner = inParam[26];
            tmpHist.myLast3f = inParam[27];
            //ここまでindex [26]→空セル
            //ここからRAデータ
            tmpHist.rA_KEY = inParam[RA_MAX];
            tmpHist.RaceDate = inParam[RA_MAX + 1];
            tmpHist.Cource = inParam[RA_MAX + 2];
            tmpHist.Kaiji = Int32.Parse(inParam[RA_MAX + 3]);
            tmpHist.raceName = inParam[RA_MAX + 7];
            tmpHist.raceName10 = inParam[RA_MAX + 8];
            tmpHist.grade = inParam[RA_MAX + 16];
            tmpHist.track = inParam[RA_MAX + 17];
            tmpHist.distance = inParam[RA_MAX + 18];
            tmpHist.tousuu = Int32.Parse(inParam[RA_MAX + 19]);
            tmpHist.JyokenInfo = inParam[RA_MAX + 13];
            tmpHist.JyokenName = inParam[RA_MAX + 14];
            tmpHist.RecornUpdateFlag = (inParam[RA_MAX + 25] == "0" ? false : true);
            tmpHist.raceName6 = inParam[RA_MAX + 26].Trim();
            RaceHist1 = tmpHist;

        }
        #endregion

        #region サーバから取得したデータを返す
        public String JvSeDataStr()
        {
            try
            {
                return SeStruct.WriteStr;
            }
            catch(Exception)
            {
                return "";
            }
        }
        #endregion

        #region 着差コード1・着差コード2から最終的な着差を取得
        public String JvDbSeChakusaCdToFix(String In1, String In2)
        {
            String ret = "";
            String tmp = "";

            if(In2 != "   ")
            {
                ret = LOG.JvSysMappingFunction(2102, ref In1);
            }

            
            

            return "";
        }
        #endregion

    }
}
