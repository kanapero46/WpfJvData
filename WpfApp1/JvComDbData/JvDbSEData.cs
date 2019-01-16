using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.dbAccess;

namespace WpfApp1.JvComDbData
{

    class JvDbSEData
    {
        dbConnect db = new dbConnect();
        private const String MAGIC_STR = "A";
        String statHorce = "";

        public JvDbSEData(String buff, int DateKind)
        {
            JVData_Struct.JV_SE_RACE_UMA JV_SE_UMA = new JVData_Struct.JV_SE_RACE_UMA();
            String tmp = "";
            int OldRaceCounter = 0;
   
            JV_SE_UMA.SetDataB(ref buff);

            //SE_MSTはKey+○走目をキーとするため、前回と馬名が一致するかを検索
            if (statHorce == JV_SE_UMA.Bamei.Trim())
            {
                OldRaceCounter++;
            }
            else
            {
                OldRaceCounter = 1;
                statHorce = JV_SE_UMA.Bamei.Trim();
            }

            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + JV_SE_UMA.id.JyoCD + JV_SE_UMA.id.Kaiji +
                JV_SE_UMA.id.Nichiji + JV_SE_UMA.id.RaceNum + JV_SE_UMA.Umaban + String.Format("{0:00}", OldRaceCounter) + ",";
            tmp += JV_SE_UMA.id.Year + JV_SE_UMA.id.MonthDay + ",";
            tmp += JV_SE_UMA.id.JyoCD + ",";
            tmp += JV_SE_UMA.id.Kaiji + ",";
            tmp += JV_SE_UMA.id.Nichiji + ",";
            tmp += JV_SE_UMA.Wakuban + ",";
            tmp += JV_SE_UMA.Umaban + ",";
            tmp += JV_SE_UMA.KettoNum + String.Format("{0:00}", OldRaceCounter) + ",";
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
        }

    }
}
