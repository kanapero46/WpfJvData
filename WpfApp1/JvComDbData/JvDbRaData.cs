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
        const int LAP_COUNT_MAX = 25;

        /* DBへの書き込み処理を変更した場合はマジックStrを書き換え、初期化を行う */
        const String MAGIC = "A";
        const String SPEC = "RA";

        struct RA_DB_WRITE_STRUCT
        {
            public String Date;
            public String WriteStr;
        }

        RA_DB_WRITE_STRUCT RaStruct = new RA_DB_WRITE_STRUCT();
        Class.com.JvComClass LOG = new Class.com.JvComClass();

        String WriteStr = "";

        public JvDbRaData()
        {
            //引数なしのインスタンス生成時はDB書き込み用の文字列を初期化
            RaStruct.Date = "";
            RaStruct.WriteStr = "";

            //クラスの初期化が必要なものはここで初期化
            LapTime = new List<string>();
        }

        public JvDbRaData(int kind, ref String buff)
        {
            JvDbRaWriteData(kind, ref buff);
        }

        #region RAデータDB書き込み共通処理
        unsafe public String JvDbRaWriteData(int kind, ref String buff)
        {
            JVData_Struct.JV_RA_RACE JV_RACE = new JVData_Struct.JV_RA_RACE();
            String tmp = "";
            String Libtmp = "";

            int DbReturn = 0;

            JV_RACE.SetDataB(ref buff);

            //tmp += JV_RACE.id.Year + JV_RACE.id.MonthDay + JV_RACE.id.JyoCD + JV_RACE.id.Kaiji + JV_RACE.id.Nichiji +
            //    JV_RACE.id.RaceNum + ",";
            //tmp += JV_RACE.id.Year + JV_RACE.id.MonthDay + ",";
            //tmp += JV_RACE.id.JyoCD + ",";
            //tmp += JV_RACE.id.Kaiji + "," + JV_RACE.id.Nichiji + "," + JV_RACE.id.RaceNum + ",";
            //tmp += JV_RACE.RaceInfo.YoubiCD + ",";

            //int CODE = LibJvConvFuncClass.RACE_NAME;
            ////レース名
            //if (JV_RACE.RaceInfo.Hondai.Trim() == "")
            //{
            //    LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.JyokenInfo.SyubetuCD + JV_RACE.JyokenInfo.JyokenCD[4], ref Libtmp);
            //    if (JV_RACE.JyokenInfo.JyokenCD[4] == "701")
            //    {
            //        CODE = LibJvConvFuncClass.COURCE_CODE;
            //        LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.id.JyoCD, ref Libtmp);
            //        Libtmp = "メイクデビュー" + Libtmp;
            //    }

            //    tmp += Libtmp;
            //}
            //else
            //{
            //    tmp += JV_RACE.RaceInfo.Hondai.Trim();
            //}
            //tmp += ",";
            //tmp += JV_RACE.RaceInfo.Ryakusyo10.Trim() + ",";
            //tmp += JV_RACE.RaceInfo.Fukudai.Trim() + ",";
            //tmp += JV_RACE.RaceInfo.Kakko.Trim() + ",";
            //tmp += JV_RACE.RaceInfo.HondaiEng.Trim() + ",";
            //tmp += JV_RACE.RaceInfo.FukudaiEng.Trim() + ",";
            //tmp += JV_RACE.JyokenInfo.SyubetuCD + ",";
            //tmp += JV_RACE.JyokenInfo.JyokenCD[4] + ",";
            //tmp += JV_RACE.RaceInfo.Nkai + ",";
            //CODE = LibJvConvFuncClass.GRACE_CODE;
            //LibJvConvFuncClass.jvSysConvFunction(&CODE, JV_RACE.GradeCD, ref Libtmp);
            //tmp += Libtmp + ",";
            //tmp += JV_RACE.TrackCD + ",";
            //tmp += JV_RACE.Kyori + ",";
            //tmp += JV_RACE.TorokuTosu + ",";
            //tmp += JV_RACE.JyokenInfo.KigoCD + ",";
            //tmp += JV_RACE.JyokenInfo.JyuryoCD + ",";
            //tmp += JV_RACE.HassoTime + ",";
            //tmp += JV_RACE.TenkoBaba.TenkoCD + ",";
            //tmp += JV_RACE.TenkoBaba.SibaBabaCD + JV_RACE.TenkoBaba.DirtBabaCD + ",";
            //tmp += JV_RACE.RecordUpKubun + ",";
            //tmp += JV_RACE.RaceInfo.Ryakusyo6 + ",";

            //for(int i=0; i<JV_RACE.LapTime.Length; i++)
            //{
            //    tmp += JV_RACE.LapTime[i] + ",";
            //}

            tmp = ConvRaData(ref buff);

            /* ここでは書き込まないように変更する、そのため別途書き込み処理をコールする必要あり。 */
            if (RaStruct.Date == "")
            {
                RaStruct.Date = JV_RACE.id.Year + JV_RACE.id.MonthDay;
            }
            
            RaStruct.WriteStr += tmp + "\n";
            return JV_RACE.head.DataKubun;
         
         /*    if(kind == 0)
            {
                //マスターデータ
                db = new dbConnect("0", JV_RACE.head.RecordSpec, ref tmp, ref DbReturn);
            }
            else
            {
                db = new dbConnect(JV_RACE.id.Year + JV_RACE.id.MonthDay, JV_RACE.head.RecordSpec, ref tmp, ref DbReturn);
            } */
        }
        #endregion

        #region RAデータをDBに書き込み処理
        public int ExecRADataWriteDb(int kind)
        {
            int DbReturn = 0;

            /* エラーチェック */
            if(RaStruct.Date == "" || RaStruct.WriteStr.Length == 0)
            {
                return 0;
            }

            if(kind == 0)
            {
                //マスターデータ
                db.DeleteCsv("RA_MST");
                db = new dbConnect("0", SPEC, ref RaStruct.WriteStr, ref DbReturn);
            }
            else
            {
               // db.DeleteCsv("RA");
                db = new dbConnect(RaStruct.Date, SPEC, ref RaStruct.WriteStr, ref DbReturn);
            }

            LOG.CONSOLE_TIME_MD("RA", "JvDbRaData DB Write -> " + RaStruct.Date +" ret(" + DbReturn + ")");
            RaStruct.Date = "";
            RaStruct.WriteStr = "";
            
            return DbReturn;
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
            setWeather(inParam[23]);
            setTrackStatus(inParam[24]);
            setRecordFlag(inParam[25]);
            setRaceName6(inParam[26]);

            for (int i = 0; i < LAP_COUNT_MAX; i++)
            {
                if(inParam[27 + i].Trim() == "")
                {
                    break;
                }
                else
                {
                    SetLapTime2(inParam[27 + i]);
                }
            }

            DataKubun1 = inParam[27 + LAP_COUNT_MAX];

            //ここからは上のラップタイムを考慮した添字にする必要あり

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
            String tmp = "";

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
                            tmp = ConvRaData(ref buff);
                            refBuff = RaData.head.DataKubun;
                            break;                            
                    }
                }
            }


            List<String> tmpArray = new List<string>();
            if(tmp != "")
            {
                var raRtData = tmp.Split(',');
               foreach(String value in raRtData)
                {
                    tmpArray.Add(value);
                }
                setData(ref tmpArray);

            }

            jVForm.JvForm_JVWatchEventClose();
            jVForm.JvForm_JvClose();

            return 1;
        }
        #endregion

        #region 前半タイム計測（ref ALL）
        public String JvDbRaConvInTimer(int Distance, ref List<String> In)
        {
            long ret = 0;
            const int StartPosition = 56;

            if (In.Count == 0)
            {
                return "00.0";
            }
            try
            {
                if (Distance <= 1600 && (Distance % 200) == 0)
                {
                    //200mで割り切れる距離：前半600mタイムを返す
                    for (int i = 0; i < 3; i++)
                    {
                        ret += long.Parse(In[StartPosition + i]);
                    }
                }
                else if (Distance <= 1700)
                {
                    //200mで割り切れない1700m以下(1300m、1150m)：最初の半端な距離＋前半600mタイムを返す
                    for (int i = 0; i < 4; i++)
                    {
                        ret += long.Parse(In[StartPosition + i]);
                    }
                }
                else if ((Distance % 200) == 0)
                {
                    //200mで割り切れる距離1800m～：前半1000mタイムを返す
                    for (int i = 0; i < 5; i++)
                    {
                        ret += long.Parse(In[StartPosition + i]);
                    }
                }
                else
                {
                    //200mで割り切れる距離1800m～：前半1000mタイムを返す
                    for (int i = 0; i < 6; i++)
                    {
                        ret += long.Parse(In[StartPosition + i]);
                    }
                }
            }
            catch(Exception)
            {
                LOG.CONSOLE_MODULE("RA", "JvDbRaConvInTimer Error!!");
                return "00.0";
            }

            if(ret == 0)
            {
                //海外・地方競馬ではラップタイムがないため
                return "00.0";
            }
            else
            {
                return ret.ToString().Substring(0, 2) + "." + ret.ToString().Substring(2, 1);
            }
            
        }
        #endregion

        #region RAデータのDB書き込みデータ生成
        private unsafe String ConvRaData(ref String buff)
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
            tmp += JV_RACE.RecordUpKubun + ",";
            tmp += JV_RACE.RaceInfo.Ryakusyo6 + ",";

            for (int i = 0; i < JV_RACE.LapTime.Length; i++)
            {
                tmp += JV_RACE.LapTime[i] + ",";
            }
            tmp += JV_RACE.head.DataKubun + ",";

            return tmp;
        }
        #endregion

        #region サーバから取得したものをList<String>型で返す。
        public List<String> JvRaDataList()
        {
            List<String> retArray = new List<string>();

            if(WriteStr == "")
            {
                return retArray;
            }

            if (WriteStr != "")
            {
                var raRtData = WriteStr.Split(',');
                foreach (String value in raRtData)
                {
                    retArray.Add(value);
                }
                setData(ref retArray);

            }

            return retArray;
        }
        #endregion

        #region サーバから取得したものをList<String>型で返す。
        public String JvRaDataStr()
        {
            return RaStruct.WriteStr;
        }
        #endregion

        #region データ区分をレース確定情報として取得する。
        public String JvRaDataKubun()
        {
            switch(DataKubun1)
            {
                case "0":
                case "A":
                case "B":
                    return "";
                case "1":
                case "2":
                    return "未確定";
                case "3":
                    return "3着まで確定";
                case "4":
                    return "5着まで確定";
                case "5":
                case "6":
                case "7":
                    return "確定";
                case "9":
                    return "レース中止";
                default:
                    return "";

            }
        }
        #endregion
    }
}
