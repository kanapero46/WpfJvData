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
        const int RA_MAX = 19;
        dbConnect db = new dbConnect();
        private const String MAGIC_STR = "A";
        String statHorce = "";

        public JvDbSEData()
        {
            //クラス宣言はなにもしない
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

        #region 共通
        private void JvDbSeComFunction(int Master, ref String buff)
        {
            JVData_Struct.JV_SE_RACE_UMA JV_SE_UMA = new JVData_Struct.JV_SE_RACE_UMA();
            String tmp = "";
            int OldRaceCounter = 0;
            int DbReturn = 0;

            JV_SE_UMA.SetDataB(ref buff);

            //SE_MSTはKey+○走目をキーとするため、前回と馬名が一致するかを検索
            if (Master == 0)
            {
                //前回保存したデータと不整合ない
                OldRaceCounter++;
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
            tmp += JV_SE_UMA.KakuteiJyuni + ",";
            tmp += JV_SE_UMA.DMTime + ",";
            tmp += JV_SE_UMA.DMJyuni + ",";

            if (Master == -1)
            {
                /* 当回データ */
                dbConnect db = new dbConnect(JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay, JV_SE_UMA.head.RecordSpec, ref tmp, ref DbReturn);
            }
            else
            {
                /* マスターデータ */
                dbConnect db = new dbConnect("0", JV_SE_UMA.head.RecordSpec, ref tmp, ref DbReturn);
            }
        }
        #endregion

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
            //ここまでindex [19]→空セル
            //ここからRAデータ
            tmpHist.rA_KEY = inParam[RA_MAX];
            tmpHist.RaceDate = inParam[RA_MAX + 1];
            tmpHist.Cource = inParam[RA_MAX + 2];
            tmpHist.Kaiji = Int32.Parse(inParam[RA_MAX + 3]);
            tmpHist.raceName = inParam[RA_MAX + 7];
            tmpHist.raceName10 = inParam[RA_MAX + 8];
            tmpHist.grade = inParam[RA_MAX + 16];
            tmpHist.distance = inParam[RA_MAX + 18];
            RaceHist1 = tmpHist;

        }



    }
}
