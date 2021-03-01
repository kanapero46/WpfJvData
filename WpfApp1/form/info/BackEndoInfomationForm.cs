using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.Class;
using WpfApp1.Class.com;
using WpfApp1.form.info.backClass;
using WpfApp1.JvComDbData;

namespace WpfApp1.form.info
{
    /* InfomationFormのバックエンドクラス */
    public class BackEndInfomationForm
    {
        //イベント発表メソッド指定定数
        public const int JV_RT_EVENT_PAY = 1;
        public const int JV_RT_EVENT_JOCKEY_CHANGE = 2;
        public const int JV_RT_EVENT_WEATHER = 3;
        public const int JV_RT_EVENT_COURCE_CHANGE = 4;
        public const int JV_RT_EVENT_AVOID = 5;
        public const int JV_RT_EVENT_TIME_CHANGE = 6;
        public const int JV_RT_EVENT_WEIGHT = 7;

        //天候・馬場状態構造体
        List<baclClassInfo> backInfo = new List<baclClassInfo>();

        Class.com.JvComClass LOG = new JvComClass();

        JvComDbData.JvDbHRData HR = new JvComDbData.JvDbHRData();
        JvDbWEData WE = new JvDbWEData();
        JvDbW5Data W5 = new JvDbW5Data();
        JvDbTcData TC = new JvDbTcData();
        JvDbWhData WH = new JvDbWhData();
        JvDbJcData JC = new JvDbJcData();

        public int JvInfoBackMain(int kind, String Key)
        {
            int ret = 0;
            LOG.CONSOLE_MODULE("BE_INFO", "[" + kind + "]" + Key);
            switch(kind)
            {
                case JV_RT_EVENT_PAY:   //払戻確定情報
                    ret = JvInfoBackJvRead("0B15", Key);
                    break;
                case JV_RT_EVENT_JOCKEY_CHANGE: //騎手変更
                case JV_RT_EVENT_WEATHER: //天候・馬場状態変更情報
                case JV_RT_EVENT_TIME_CHANGE:
                    ret = JvInfoBackJvRead("0B16", Key);
                    break;
                case JV_RT_EVENT_WEIGHT:
                    ret = WH.JvDbWhLinkData(Key);
                    break;
            }


            return ret;
        }


        //速報系バックエンド処理
        private int JvInfoBackJvRead(String Spec, String Key)
        {
            JVData_Struct.JV_HR_PAY Pay = new JVData_Struct.JV_HR_PAY();
            JVData_Struct.JV_WE_WEATHER Weather = new JVData_Struct.JV_WE_WEATHER();
            JVData_Struct.JV_JC_INFO Jc = new JVData_Struct.JV_JC_INFO();


            JVForm JvForm = new JVForm();
            int ret = 0;

            Boolean HRWriteFlag = false;
            Boolean TCWriteFlag = false;

            JvForm.JvForm_JvInit();

            ret = JvForm.JvForm_JvRTOpen(Spec, Key);

            if(ret != 0)
            {
                LOG.CONSOLE_MODULE("BE_INFO", "JVRTOPEN ERROR! JvInfoBackJvRead["+ Spec +"](" + ret + ")");
                JvForm.JvForm_JvClose();
                return ret;
            }

            ret = 1;
            String buff = "";
            int size = 20000;
            String filename = "";

            //インスタンスの初期化が必要なものはここで初期化する。
            TC = new JvDbTcData();
            JvDbJcData JcData = new JvDbJcData();

            while(ret >= 1)
            {
                ret = JvForm.JvForm_JvRead(ref buff, out size, out filename);
                if(ret >= 1)
                {
                    switch(buff.Substring(0,2))
                    {
                        case "HR":
                            HR.SetHrData(ref buff);
                            HRWriteFlag = true;
                            break;
                        case "WE":  //天候・馬場状態
                            WE.JvDbWeSetData(ref buff);
                            break;
                        case "TC":
                            TC.SetJvTcData(ref buff);
                            TCWriteFlag = true;
                            break;
                        case "JC":
                            JC.RestrictJCData(ref buff);
                            break;

                    }
                }
                else if(ret == 0 || buff == "")
                {
                    //全ファイル読み込み終了
                    break;
                }
                else if(ret == -1)
                {
                    ret = 1;
                    continue;
                }
                else
                {

                }

                if(TCWriteFlag)
                {
                    TC.ExecJvTcData();
                }

            }
            JvForm.JvForm_JvClose();

            //DBに書き込み
            if(HRWriteFlag)
            {
                ret = HR.JvWriteHrData();
            }
            else if(TCWriteFlag)
            {
                ret = TC.WriteJvDbData();
            }

            return 1;
        }

        #region 払戻金確定競馬場・場名取得
        public void BackEndoGetPayCource(ref String Key, ref String Cource, ref int Racenum)
        {
            HR.GetPayInfo(ref Key, ref Cource, ref Racenum);
        }
        #endregion

        #region 払戻情報をWindowsへ通知するためのマッピング関数
        public void BackEndPayInfoNotice()
        {
            HR.JvHrNoticeWindows();
        }
        #endregion

        #region JvDataライブラリから競馬場名を取得
        unsafe public String BackendMappingCourceName(String JyoCd)
        {
            int libCode = 0;
            String retTmp = "";
            libCode = LibJvConv.LibJvConvFuncClass.COURCE_CODE;
            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&libCode, JyoCd, ref retTmp);
            return retTmp;
        }
        #endregion

        #region 発走時刻変更情報を取得 0:取得なし　1:取得成功
        public int BackEndHassouTimeChangeInfo(int idx, String Date, ref List<String> Out)
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> tmp = new List<string>();
            if (db.TextReader_Col(Date, "TC", 0, ref tmp, idx.ToString()) == 0)
            {
                return 0;
            }

            Out = tmp;
            return 1;           
        }
        #endregion

        #region 発走時刻の最後の件数を取得
        public int BackEndHassouTimeChangeInfoCounter(String Date)
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> tmp = new List<string>();
            int ret = 0;

            for(int i=1; ;i++)
            {
                if(db.TextReader_Col(Date, "TC", 0, ref tmp, i.ToString()) >= 1 )
                {
                    if(tmp.Count == 0)
                    {
                        return i;
                    }
                    else
                    {
                        ret++;
                    }
                }
                else
                {
                    break;
                }
            }
            return ret;
        }
        #endregion

        unsafe public void BackEndGetKaisaiInfo(String Date)
        {

            JVForm Jv = new JVForm();

            Jv.JvForm_JvInit();
            Jv.JvForm_JVWatchEvent();
            int ret = Jv.JvForm_JvRTOpen("0B14", Date);

            if(ret != 0)
            {
                LOG.CONSOLE_MODULE("BE_INFO", "KaisaiInfo Error RTOPEN[" + ret + "]");
                return;
            }

            String fileName = "";
            int size = 20000;
            String buff = "";
            ret = 1;

            while(ret >= 1)
            {
                ret = Jv.JvForm_JvRead(ref buff, out size, out fileName);

                if(ret == 0)
                {
                    break;
                }
                else if(ret == -3)
                {
                    ret = 1;
                    continue;
                }
                else if(ret == -202)
                {
                    break;
                }
                else if(ret == -1)
                {
                    ret = 1;
                    continue;
                }

                if (buff == "")
                {
                    ret = 0;
                    break;
                }

                switch(buff.Substring(0,2))
                {
                    case "WE":
                        break;
                }
            


            }
        }


        #region 単勝オッズ読み込み（DB書き込みなし）
        /* @return -1：エラー（取得に失敗）　0：未発売　1:発売中 2:締切 3：レース中止 */
        public int BackEndGetOddsInfo(String Key)
        {
            JVForm JVForm = new JVForm();

            if(Key == "")
            {
                return -1;
            }

            JVForm.JvForm_JvInit();
            int ret = JVForm.JvForm_JvRTOpen("0B31", Key);

            if(ret != 0)
            {
                JVForm.JvForm_JvClose();
                return -1;
            }

            ret = 1;
            String fname = "";
            String buff = "";
            int size = 20000;

            JVData_Struct.JV_O1_ODDS_TANFUKUWAKU O1 = new JVData_Struct.JV_O1_ODDS_TANFUKUWAKU();
            int res = -1;
                       
            while(ret >= 1)
            {
                ret = JVForm.JvForm_JvRead(ref buff, out size, out fname);

                if(ret == 0)
                {
                    //EOF
                    break;
                }
                else if(ret == -1)
                {
                    //ファイル切り替え
                    continue;
                }
                else if(ret == -3)
                {
                    //DL中
                    continue;
                }
                else if(buff == "")
                {
                    continue;
                }
                else if(ret <= 0)
                {
                    res = -1;
                    break;
                }

                switch(buff.Substring(0,2))
                {
                    case "O1":
                        O1.SetDataB(ref buff);
                        //レース中止情報を追加するため、if文に変更
                        if (O1.head.DataKubun == "1" || O1.head.DataKubun == "3") res = 1;  //発売中
                        else if (O1.head.DataKubun == "9") res = 3;                         //レース中止
                        else res = 2;                                                       //発売締切
                        break;
                }
      
            }

            JVForm.JvForm_JvClose();
            return res;
        }
        #endregion

        #region 開催回次・日次をDBから取得
        public String BackEndGetKaijiNichi(String Date, int JyoCd)
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();

            List<String> ArrayStr = new List<string>();

            if(db.TextReader_Row(Date, "RA", 0, ref ArrayStr) != 0)
            {
                for(int i=0; i<ArrayStr.Count; i++)
                {
                    if(ArrayStr[i].Substring(0,10) == (Date + String.Format("{0:00}",JyoCd)))
                    {
                        return ArrayStr[i];
                    }
                }
            }

            return "";
        }
        #endregion

        #region 変更情報取得
        public int BackEndWeatherCondInfo(String Spec, String Key, ref List<backClass.baclClassInfo> InfoClass)
        {
            int ret = 0;

            JVForm JvForm = new JVForm();
            backClass.baclClassInfo tmpWeatherCond = new baclClassInfo();


            JvDbWEData JvWeData = new JvDbWEData();

            JvForm.JvForm_JvInit();
            JvForm.JvForm_JVWatchEvent();
            ret = JvForm.JvForm_JvRTOpen(Spec, Key);

            if (ret != 0)
            {
                LOG.CONSOLE_MODULE("BE_INFO", "JVRTOPEN ERROR! WeatherCondeInfo[" + Spec + "](" + ret + ")");
                JvForm.JvForm_JVWatchEventClose();
                JvForm.JvForm_JvClose();
                return -1;
            }

            String buff = "";
            String fname = "";
            int size = 20000;
            ret = 1;

            JVData_Struct.JV_WE_WEATHER we = new JVData_Struct.JV_WE_WEATHER();
            //JVData_Struct.WF_PAY_INFO W5 = new JVData_Struct.WF_PAY_INFO();

            JvDbW5Data W5 = new JvDbW5Data();

            while (ret >= 1)
            {
                ret = JvForm.JvForm_JvRead(ref buff, out size, out fname);
                if (ret == 0)
                {
                    //EOF
                    break;
                }
                else if (ret == -1)
                {
                    //ファイル切り替え
                    continue;
                }
                else if (ret == -3)
                {
                    //DL中
                    continue;
                }
                else if (buff == "")
                {
                    continue;
                }
                else if (ret <= 0)
                {
                    ret = -1;
                    break;
                }

                switch(buff.Substring(0,2))
                {
                    case "WE":
                        we.SetDataB(ref buff);
                        JvWeData.JvDbWeSetData(ref buff);
                        break;
                    case "AV":
                        we.SetDataB(ref buff);
                        //InfoClass.SetDNSInfo();
                        break;
                    case "JC":  //騎手変更
                        //InfoClass.SetJockeyInfo();
                        // JcData = new JvDbJcData( ref buff, false,  )
                        break;
                    case "TC":  //発走時刻変更
                      //  InfoClass.SetTimeInfo();
                        break;
                    case "CC":  //コース変更情報
                     //   InfoClass.SetCourceInfo();
                        break;
                    case "WF":  //WIN5データ
                        W5.SetW5Data(ref buff);
                        break;
                    default:
                        break;
                }

               
            }

            //共通
            backClass.baclClassInfo tmpInfoClass;

            if (Spec == "0B14" || Spec == "0B16")
            {
                WeatherCourceStatus weatherStatus = new WeatherCourceStatus();
                

                for (int i = 0; i < JvWeData.JvDbWeGetCount(); i++)
                {
                    tmpInfoClass = new baclClassInfo();
                    ret = JvWeData.JvDbWeGetDataMapping(i, ref weatherStatus);
                    BackEndConvWeatherStatusClassInfo(ref weatherStatus, ref tmpInfoClass);
                    InfoClass.Add(tmpInfoClass);
                }
            }
            else if(Spec == "0B51")
            {
                tmpInfoClass = new baclClassInfo();
                tmpInfoClass.W51 = (JvDbW5Data)W5;
                InfoClass.Add(tmpInfoClass);
            }



            JvForm.JvForm_JVWatchEventClose();
            JvForm.JvForm_JvClose();
            return 1;
        }
        #endregion

        #region WIN5のバックエンド処理
        public int BackEndGetWin5Info(String key)
        {
            List<baclClassInfo> classInfo = new List<baclClassInfo>();
            if (BackEndWeatherCondInfo("0B51", key, ref classInfo) != 0)
            {
                if(classInfo.Count != 0)
                {
                    W5 = classInfo[0].W51;
                    LOG.CONSOLE_MODULE("BE_INFO", "WIN5 Enable Status(" + classInfo[0].W51.Win5Status + ")");
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        #region WIN5データの取得
        public void BackEndGetWin5(ref JvDbW5Data Out)
        {
            Out = W5;
        }
        #endregion

        public void GetJcData(ref InfoData info)
        {
            info.InfoRaceKey1 = JC.Key1;
            info.Type1 = InfoData.ID_TYPE_JOCKEY_CHANGE;
            info.Time1 = JC.Time1;

            //変更後
            String tmp = JC.AfterInfo1.MinaraiCd;
            info.ChangeAfterName1 = LOG.JvSysMappingFunction(2303, ref tmp);
            info.ChangeAfterName1 += JC.AfterInfo1.MinaraiCd + "(" + JC.AfterInfo1.Futan.Substring(0, 2) + "." + JC.AfterInfo1.Futan.Substring(2, 1) + "kg)";

            //変更前
            tmp = JC.BeforeInfo1.MinaraiCd;
            info.InfoName1 = LOG.JvSysMappingFunction(2303, ref tmp);
            info.InfoName1 += JC.BeforeInfo1.MinaraiCd + "(" + JC.BeforeInfo1.Futan.Substring(0, 2) + "." + JC.BeforeInfo1.Futan.Substring(2, 1) + "kg)";
        }

        unsafe public String BackEndLibMappingFunction(int Code, String Cd)
        {
            int LibCode = Code;
            String libStr = "";
            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&LibCode, Cd, ref libStr);
            return libStr;
        }

        private void BackEndConvWeatherStatusClassInfo(ref WeatherCourceStatus In, ref backClass.baclClassInfo Out)
        {
            try
            {
                //天候・馬場状態をInformationForm用に生成
                Out.Key1 = In.Key;
                Out.WeatherFlag1 = true;
                Out.Weather = In.Weather;
                Out.TurfStatus = In.Turf;
                Out.DirtStatus = In.Dirt;
            }
            catch(Exception e)
            {
                LOG.CONSOLE_MODULE("BE_INFO", "ConvWeatherStatusInfo ConvertError!");
                LOG.CONSOLE_MODULE("BE_INFO", e.Message);
            }
        }
    }
}
