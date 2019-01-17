using LibJvConv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.dbAccess;

namespace WpfApp1.JvComDbData
{
    public class JvDbRaData
    {
        dbConnect db = new dbConnect();
        const String MAGIC = "A";

        public JvDbRaData()
        {
            //引数なしのインスタンスはなにもしない
        }

        unsafe public JvDbRaData(ref String buff)
        {
            JVData_Struct.JV_RA_RACE JV_RACE = new JVData_Struct.JV_RA_RACE();
            String tmp = "";
            String Libtmp = "";

            JV_RACE.SetDataB(ref buff);

            tmp += JV_RACE.id.Year + JV_RACE.id.MonthDay + JV_RACE.id.JyoCD + JV_RACE.id.Kaiji + JV_RACE.id.Nichiji +
                JV_RACE.id.RaceNum + ",";
            tmp += JV_RACE.id.Year + JV_RACE.id.MonthDay + ",";
            tmp += JV_RACE.id.JyoCD + ",";
            tmp += JV_RACE.id.Kaiji + "," + JV_RACE.id.Nichiji + "," + JV_RACE.id.RaceNum + ",";
            tmp += JV_RACE.RaceInfo.YoubiCD + ",";

            int CODE = LibJvConvFuncClass.RACE_NAME;
            //レース名
            if (JV_RACE.RaceInfo.Hondai.Trim() == "")
            {
                LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.JyokenInfo.SyubetuCD + JV_RACE.JyokenInfo.JyokenCD[4], ref Libtmp);
                if (JV_RACE.JyokenInfo.JyokenCD[4] == "701")
                {
                    CODE = LibJvConvFuncClass.COURCE_CODE;
                    LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.id.JyoCD, ref Libtmp);
                    Libtmp = "メイクデビュー" + Libtmp;
                }

                tmp += Libtmp;
            }
            else
            {
                tmp += JV_RACE.RaceInfo.Hondai.Trim();
            }
            tmp += ",";
            tmp += JV_RACE.RaceInfo.Ryakusyo10.Trim() + ",";
            tmp += JV_RACE.RaceInfo.Fukudai.Trim() + ",";
            tmp += JV_RACE.RaceInfo.Kakko.Trim() + ",";
            tmp += JV_RACE.RaceInfo.HondaiEng.Trim() + ",";
            tmp += JV_RACE.RaceInfo.FukudaiEng.Trim() + ",";
            tmp += JV_RACE.JyokenInfo.SyubetuCD + ",";
            tmp += JV_RACE.JyokenInfo.JyokenCD[4] + ",";
            tmp += JV_RACE.RaceInfo.Nkai + ",";
            CODE = LibJvConvFuncClass.GRACE_CODE;
            LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.GradeCD, ref Libtmp);
            tmp += Libtmp + ",";
            tmp += JV_RACE.TrackCD + ",";
            tmp += JV_RACE.Kyori + ",";
            tmp += JV_RACE.TorokuTosu + ",";
            tmp += JV_RACE.JyokenInfo.KigoCD + ",";
            tmp += JV_RACE.JyokenInfo.JyuryoCD + ",";
            tmp += JV_RACE.HassoTime + ",";
            tmp += JV_RACE.TenkoBaba.TenkoCD + ",";
            tmp += JV_RACE.TenkoBaba.SibaBabaCD + JV_RACE.TenkoBaba.DirtBabaCD + ",";

        }

    }
}
