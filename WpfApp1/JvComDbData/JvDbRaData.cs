using LibJvConv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Class;
using WpfApp1.dbAccess;

namespace WpfApp1.JvComDbData
{
    public class JvDbRaData : MainDataClass
    {
        dbConnect db = new dbConnect();

        const int RA_MAX = 25;

        /* DBへの書き込み処理を変更した場合はマジックStrを書き換え、初期化を行う */
        const String MAGIC = "A";

        public JvDbRaData()
        {
            //引数なしのインスタンス生成はなにもしない
        }

        #region RAデータDB書き込み共通処理
        unsafe public JvDbRaData(int kind, ref String buff)
        {
            JVData_Struct.JV_RA_RACE JV_RACE = new JVData_Struct.JV_RA_RACE();
            String tmp = "";
            String Libtmp = "";

            int DbReturn = 0;

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
            
            if(kind == 0)
            {
                //マスターデータ
                db = new dbConnect("0", JV_RACE.head.RecordSpec, ref tmp, ref DbReturn);
            }
            else
            {
                db = new dbConnect(JV_RACE.id.Year + JV_RACE.id.MonthDay, JV_RACE.head.RecordSpec, ref tmp, ref DbReturn);
            }
        }
        #endregion

        #region RAキーの自動生成
        public int GET_AUTO_RA_KEY(ref String inParam)
        {
            if (getRaceDate() == "" || getRaceCource() == "" || getRaceKaiji() == "" || getRaceNichiji() == "" || getRaceNum() == "")
            {
                return 0;
            }

            inParam = getRaceDate() + getRaceCource() + getRaceKaiji() + getRaceNichiji() + getRaceNum();
            return 1;
        }
        #endregion

        #region TextReader_Rowから読み込んだ配列からデータ・セット
        public void setData(ref List<String> inParam)
        {
            SET_RA_KEY(inParam[0]);
            setRaceDate(inParam[1]);
            setRaceCource(inParam[2]);
            setRaceKaiji(inParam[3]);
            setRaceNichiji(inParam[4]);
            setRaceNum(inParam[5]);
            setWeekDay(inParam[6]);
            setRaceName(inParam[7]);
            setRaceNameEnd(inParam[10]);
            setRaceNameFuku(inParam[9]);
            setRaceNameEng(inParam[11] + (inParam[12] == ""?"":"(" + inParam[12] + ")"));
            setOldYear(inParam[13]);
            setRaceClass(inParam[14]);
            setRaceGradeKai(inParam[15]);
            setRaceGrade(inParam[16]);
            setCourceTrack(inParam[17]);
            setDistance(inParam[18]);
            setRaceKindKigo(inParam[20]);
            setRaceHandCap(inParam[21]);
            setRaceStartTime(inParam[22]);
            setTrackStatus(inParam[23]);
        }
        #endregion

        #region　レース開催情報リアルタイムデータ取得

        public int RaGetRTRaData(String Key, ref String refBuff)
        {
            JVForm jVForm = new JVForm();
            MainWindow main = new MainWindow();

            jVForm.JvForm_JvInit();

            //速報系スレッド起動
            jVForm.JvForm_JVWatchEvent();

            int ret = jVForm.JvForm_JvRTOpen("0B15", Key);

            if(ret != 0)
            {
                if (ret != -202)
                {
                    //System.Windows.MessageBox.Show("DataLabサーバに接続出来ませんでした。\nSC-" + ret, "JVRTOpenエラー");
                    jVForm.JvForm_JVWatchEventClose();     //速報系スレッドの終了
                    return 0;
                }
                ret = jVForm.JvForm_JvRTOpen("OB15", Key);
                if (ret != 0)
                {
                    //System.Windows.MessageBox.Show("DataLabサーバに接続出来ませんでした。\nSC-" + ret, "JVRTOpenエラー2");
                    jVForm.JvForm_JVWatchEventClose();     //速報系スレッドの終了
                    return 0;
                }
            }

            JVData_Struct.JV_RA_RACE RaData = new JVData_Struct.JV_RA_RACE();
            ret = 1;

            String buff = "";
            int size = 20000;
            String fname = "";

            String retBuff = "";

            while (ret >= 1)
            {
                ret = jVForm.JvForm_JvRead(ref buff, out size, out fname);

                if (ret > 0)
                {
                    if (buff == "")
                    {
                        continue;
                    }

                    switch (buff.Substring(0, 2))
                    {
                        case "RA":
                            RaData.SetDataB(ref buff);
                            refBuff = RaData.head.DataKubun;
                            break;
                    }
                }
            }

            jVForm.JvForm_JVWatchEventClose();
            jVForm.JvForm_JvClose();

            return 1;
        }
        #endregion

    }
}
