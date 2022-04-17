using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WpfApp1.Class.com;
namespace WpfApp1.JvComDbData
{
    class JvDbOzData
    {
        String tmpSpec = "";
        String DbWrite = "";
        String OddzTime = "";
        String Key = "";

        const String Br = "\n";
        const String InitFormat = "#---全オッズデータ\n# キー|オッズ発表時間|登録頭数|単勝発売フラグ|複勝・枠連フラグ(発売あり→7)|\n# 識別子|組番|オッズ|人気順|";

        public struct OZ_COM
        {
            int Umaban;
            String Odds;
            int Pay;
        }

        //O1Formから参照
        public struct PAY_FLAG
        {
            public String OddzTime;         //オッズ発表時間
            public int PayStatus;
            public Boolean TanshoPayFlag;
            public Boolean Ren2PayFlag;     //馬連・馬単発売フラグ（３頭以上）
            public Boolean Ren3PayFlag;     //４頭以上発売フラグ（ワイド・３連複・３連単）
            public Boolean FukushoPayFlag;  //複勝発売フラグ（5頭以上）
            public int FukushoTosuInfo;     //複勝○着払い
            public Boolean WakurenPayFlag;  //枠連発売フラグ(9頭以上）
        }

        public struct JV_OZ_DATA_STRUCTS
        {
            public OZ_COM Tan;
            public OZ_COM[] Fuku;
            public OZ_COM[] Wide;
            public OZ_COM Umaren;
            public OZ_COM Umatan;
            public OZ_COM SanPuku;
            public OZ_COM SanTan;
        };

        JvComClass LOG = new JvComClass();
        public JV_OZ_DATA_STRUCTS OZ_DATA;
        PAY_FLAG ExtPayInfo = new PAY_FLAG();

        public JvDbOzData()
        {
            InitDbData();
        }
               
        public void JvDbOzGetStart(ref JV_OZ_DATA_STRUCTS OutParam)
        {
            Thread thread = null;

            JVData_Struct.JV_O1_ODDS_TANFUKUWAKU O1;
            JVData_Struct.JV_O2_ODDS_UMAREN O2;
            JVData_Struct.JV_O3_ODDS_WIDE O3;
            JVData_Struct.JV_O4_ODDS_UMATAN O4;
            JVData_Struct.JV_O5_ODDS_SANREN O5;
            JVData_Struct.JV_O6_ODDS_SANRENTAN O6;

            InitStruct();

            /* 単勝スレッド起動 */
            thread = new Thread(new ParameterizedThreadStart(JvDbOzGetO1Data));
            thread.Start();

            Thread thread2 = new Thread(new ParameterizedThreadStart(JvDbOzGetO1Data));
            thread2.Start();

            Thread thread3 = new Thread(new ParameterizedThreadStart(JvDbOzGetO1Data));
            thread2.Start();

            Thread thread4 = new Thread(new ParameterizedThreadStart(JvDbOzGetO1Data));
            thread2.Start();


            Thread thread5 = new Thread(new ParameterizedThreadStart(JvDbOzGetO1Data));
            thread2.Start();


            Thread thread6 = new Thread(new ParameterizedThreadStart(JvDbOzGetO1Data));
            thread2.Start();


            Thread thread7 = new Thread(new ParameterizedThreadStart(JvDbOzGetO1Data));
            thread2.Start();

        }

        private void JvDbOzGetO1Data(object Param)
        {

        }

        private void InitStruct()
        {
            OZ_DATA = new JV_OZ_DATA_STRUCTS();
            OZ_DATA.Fuku = new OZ_COM[3];
            OZ_DATA.Wide = new OZ_COM[3];
        }

        public int JvDbSetOzData(String Spec, ref String buff)
        {
            JVData_Struct.JV_O1_ODDS_TANFUKUWAKU O1;
            JVData_Struct.JV_O2_ODDS_UMAREN O2;
            JVData_Struct.JV_O3_ODDS_WIDE O3;
            JVData_Struct.JV_O4_ODDS_UMATAN O4;
            JVData_Struct.JV_O5_ODDS_SANREN O5;
            JVData_Struct.JV_O6_ODDS_SANRENTAN O6;

            JvDbO1Data O1DataClass;

            if (buff == "" || Spec.Length != 2)
            {
                return 0;
            }

            String tmp = "";
            tmpSpec = Spec;

            object JvDataObj = null;

            if(buff.Length <= 2)
            {
                return 0;
            }
            else
            {
                switch (tmpSpec)
                {
                    case "O1":
                        //共通ヘッダ
                        JvOzDataForCommon(Spec, ref buff);

                        //単勝・複勝・枠連オッズ
                        O1 = new JVData_Struct.JV_O1_ODDS_TANFUKUWAKU();
                        O1DataClass = new JvDbO1Data();
                        //単勝・複勝・枠連
                        O1DataClass.SetJvDbO1Data(ref buff);

                        //キーは単勝取得時に設定
                        O1.SetDataB(ref buff);
                        Key = O1.id.Year + O1.id.MonthDay + O1.id.JyoCD + O1.id.Kaiji + O1.id.Nichiji + O1.id.RaceNum;

                        //この中でもセットする。
                        JvOzDataForO1(ref O1);
                        break;
                    case "O2":
                        //馬連
                        O2 = new JVData_Struct.JV_O2_ODDS_UMAREN();
                        O2.SetDataB(ref buff);
                        ExtPayInfo.Ren2PayFlag = (O2.UmarenFlag == "7" ? true:false);
                        JvOzDataForO2(ref O2);
                        break;
                    case "O3":
                        //ワイド
                        O3 = new JVData_Struct.JV_O3_ODDS_WIDE();
                        O3.SetDataB(ref buff);
                        ExtPayInfo.Ren3PayFlag = (O3.WideFlag == "7" ? true : false);
                        JvOzDataForO3(ref O3);
                        break;
                    case "O4":
                        //馬単
                        O4 = new JVData_Struct.JV_O4_ODDS_UMATAN();
                        O4.SetDataB(ref buff);
                        JvOzDataForO4(ref O4);
                        break;
                    case "O5":
                        //３連複
                        O5 = new JVData_Struct.JV_O5_ODDS_SANREN();
                        O5.SetDataB(ref buff);
                        JvOzDataForO5(ref O5);
                        break;
                    case "O6":
                        //３連単
                        O6 = new JVData_Struct.JV_O6_ODDS_SANRENTAN();
                        O6.SetDataB(ref buff);
                        JvOzDataForO6(ref buff, ref O6);
                        break;
                    default:
                        LOG.CONSOLE_MODULE("OZ", "SPEC ERROR!!" + tmpSpec);
                        return 0;
                }

                //共通データのセット

            }

            return 1;
        }

        public String JvOzDataGetOddzTime(Boolean Convert)
        {
            try
            {
                if (Convert)
                {
                    return OddzTime.Substring(0, 2) + "月" + OddzTime.Substring(2, 2) + "日 " + OddzTime.Substring(4, 2) + "時" + OddzTime.Substring(6, 2) + OddzTime.Substring(8, 2) + "分";
                }
                else
                {
                    //そのまま
                    return OddzTime;
                }
            }
            catch(Exception)
            {
                return "";
            }
        }

        //発売情報を外部にわたす
        public PAY_FLAG JvOzDataGetPayInfo()
        {
            return ExtPayInfo;
        }

        private void JvOzDataForO1(ref String buff)
        {
            JVData_Struct.JV_O1_ODDS_TANFUKUWAKU OZ = new JVData_Struct.JV_O1_ODDS_TANFUKUWAKU();
        }

        private void JvOzDataForCommon(String Spec, ref String buff)
        {
            const String O0_Key = "O0";
            DbWrite += O0_Key + buff.Substring(11, 16) + ",";    //レース日時開催レース番号競馬場コード
            DbWrite += buff.Substring(27, 8) + ",";     //オッズ発表時間
            DbWrite += buff.Substring(35, 2) + ",";     //登録頭数
            DbWrite += buff.Substring(39, 1) + ",";     //発売フラグ（単勝複勝枠連は単勝のみ９

            OddzTime = buff.Substring(27, 8) + ",";     //オッズ発表時間をグローバル変数に保持
            ExtPayInfo.OddzTime = buff.Substring(27, 8);
            ExtPayInfo.PayStatus = Int32.Parse(buff.Substring(2, 1));

            //O1データは複勝と枠連フラグがあるため、ここで処理
            if (Spec == "O1")
            {
                ExtPayInfo.TanshoPayFlag = (buff.Substring(39, 1) == "7" ? true : false);   //単勝発売フラグ
                ExtPayInfo.FukushoPayFlag = (buff.Substring(40, 1) == "7" ? true : false);   //複勝発売フラグ
                ExtPayInfo.WakurenPayFlag = (buff.Substring(41, 1) == "7" ? true : false);   //枠連発売フラグ
                ExtPayInfo.FukushoTosuInfo = Int32.Parse(buff.Substring(42, 1)); 
                DbWrite += buff.Substring(40, 1) + ",";       //複勝フラグ
                DbWrite += buff.Substring(41, 1) + ",";       //枠連フラグ
                DbWrite += buff.Substring(42, 1) + ",";       //複勝払い戻しキー

            }
            DbWrite += buff.Substring(2, 1) + ",";  //データ区分
            DbWrite += Br;
        }

        //01：単勝・複勝・枠連
        private void JvOzDataForO1(ref JVData_Struct.JV_O1_ODDS_TANFUKUWAKU O1)
        {
            //SetDataBをコールしてから呼ぶ
            for (int i = 0; i < O1.OddsTansyoInfo.Length; i++)
            {
                //空データはスキップ(時間短縮のため)
                if (O1.OddsTansyoInfo[i].Umaban.Trim() == "")
                {
                    continue;
                }

                DbWrite += O1.head.RecordSpec + O1.OddsTansyoInfo[i].Umaban + ",";
                DbWrite += O1.OddsTansyoInfo[i].Umaban + ",";
                DbWrite += O1.OddsTansyoInfo[i].Odds + ",";
                DbWrite += O1.OddsTansyoInfo[i].Ninki + ",";
                DbWrite += O1.OddsFukusyoInfo[i].OddsHigh + ",";
                DbWrite += O1.OddsFukusyoInfo[i].OddsLow + ",";
                DbWrite += O1.OddsFukusyoInfo[i].Ninki + ",";
                DbWrite += Br;
            }

            if (O1.WakurenFlag != "7") return;  //枠連の発売がない場合、スキップ(７：発売あり)
            for (int i = 0; i < O1.OddsWakurenInfo.Length; i++)
            {
                //空データはスキップ(時間短縮のため)
                if (O1.OddsWakurenInfo[i].Kumi.Trim() == "")
                {
                    continue;
                }

                //枠連にはO7をセット
                DbWrite += "O7" + O1.OddsWakurenInfo[i].Kumi + ",";
                DbWrite += O1.OddsWakurenInfo[i].Kumi + ",";
                DbWrite += O1.OddsWakurenInfo[i].Odds + ",";
                DbWrite += O1.OddsWakurenInfo[i].Ninki + ",";
                DbWrite += Br;
            }
        }

        //02:馬連
        private void JvOzDataForO2(ref JVData_Struct.JV_O2_ODDS_UMAREN O2)
        {
            //SetDataBをコールしてから呼ぶ
            for(int i=0; i<O2.OddsUmarenInfo.Length; i++)
            {
                //空データはスキップ(時間短縮のため)
                if(O2.OddsUmarenInfo[i].Kumi.Trim() == "")
                {
                    continue;
                }

                DbWrite += O2.head.RecordSpec + O2.OddsUmarenInfo[i].Kumi + ",";
                DbWrite += O2.OddsUmarenInfo[i].Kumi + ",";
                DbWrite += O2.OddsUmarenInfo[i].Odds + ",";
                DbWrite += O2.OddsUmarenInfo[i].Ninki + ",";
                DbWrite += Br;
            }
        }

        private void JvOzDataForO3(ref JVData_Struct.JV_O3_ODDS_WIDE O3)
        {
           
            for(int i=0; i<O3.OddsWideInfo.Length; i++)
            {                
                //空データはスキップ(時間短縮のため)
                if (O3.OddsWideInfo[i].Kumi.Trim() == "")
                {
                    continue;
                }

                DbWrite += O3.head.RecordSpec + O3.OddsWideInfo[i].Kumi + ",";
               DbWrite += O3.OddsWideInfo[i].Kumi + ",";
                DbWrite += O3.OddsWideInfo[i].OddsHigh + ",";
                DbWrite += O3.OddsWideInfo[i].OddsLow + ",";
                DbWrite += O3.OddsWideInfo[i].Ninki + ",";
                DbWrite += Br;
            }
            
        }

        private void JvOzDataForO4(ref JVData_Struct.JV_O4_ODDS_UMATAN O4)
        {
            for (int i = 0; i < O4.OddsUmatanInfo.Length; i++)
            {
                //空データはスキップ(時間短縮のため)
                if (O4.OddsUmatanInfo[i].Kumi.Trim() == "")
                {
                    continue;
                }

                DbWrite += O4.head.RecordSpec + O4.OddsUmatanInfo[i].Kumi + ",";
                DbWrite += O4.OddsUmatanInfo[i].Kumi + ",";
                DbWrite += O4.OddsUmatanInfo[i].Odds + ",";
                DbWrite += O4.OddsUmatanInfo[i].Ninki + ",";
                DbWrite += Br;
            }
        }

        private void JvOzDataForO5(ref JVData_Struct.JV_O5_ODDS_SANREN O5)
        {
            for (int i = 0; i < O5.OddsSanrenInfo.Length; i++)
            {
                //空データはスキップ(時間短縮のため)
                if (O5.OddsSanrenInfo[i].Kumi.Trim() == "")
                {
                    continue;
                }

                DbWrite += O5.head.RecordSpec + O5.OddsSanrenInfo[i].Kumi + ",";
                DbWrite += O5.OddsSanrenInfo[i].Kumi + ",";
                DbWrite += O5.OddsSanrenInfo[i].Odds + ",";
                DbWrite += O5.OddsSanrenInfo[i].Ninki + ",";
                DbWrite += Br;
            }           
        }

        //現在SetDataBでエラーとなる
        private void JvOzDataForO6(ref String buff, ref JVData_Struct.JV_O6_ODDS_SANRENTAN O6)
        {
            //いまだけ、ここでエンコードする。
            
            LOG.CONSOLW_DEBUG(buff.Length.ToString());
            for (int i = 0; i < 4896; i++)
            {
                //空データはスキップ(時間短縮のため)
                if (buff.Substring(40 + (17 * i), 6).Trim() == "")
                {
                    continue;
                }
#if false
                DbWrite += buff.Substring(0, 2) + buff.Substring(40 + (17 * i), 6) + ",";
                DbWrite += buff.Substring(40 + (17 * i), 6) + ",";
                DbWrite += buff.Substring(46 + (17 * i), 7) + ",";
                DbWrite += buff.Substring(52 + (17 * i), 4) + ",";
#else  //JvDataClassのSetDataでエラーになるため
                DbWrite += O6.head.RecordSpec + O6.OddsSanrentanInfo[i].Kumi + ",";
                DbWrite += O6.OddsSanrentanInfo[i].Kumi + ",";
                DbWrite += O6.OddsSanrentanInfo[i].Odds + ",";
                DbWrite += O6.OddsSanrentanInfo[i].Ninki + ",";
#endif
                DbWrite += Br;
            }            
        }

        public int JvOzDbWriteDbData()
        {
            int ret = 0;
            LOG.CONSOLE_TIME_OUT();
            dbAccess.dbConnect db0 = new dbAccess.dbConnect();
            db0.DeleteCsv("OZ", Key.Substring(0,8) + "\\OZ" + Key + ".csv");
            db0 = new dbAccess.dbConnect(Key, "OZ", ref DbWrite, ref ret);
            LOG.CONSOLE_TIME_MD("OZ", "<< DB_WRITE Result " + (ret == 0 ? "falut" : "success") + " >>");
            return ret;
        }

        private void InitDbData()
        {
            tmpSpec = "";
            DbWrite = InitFormat + Br;
            OddzTime = "";
        }

        public int JvOzDbReadOddzData(String Key, String oddzKind, ref List<String> outParam)
        {
            List<String> InParam = new List<string>();

            switch(oddzKind)
            {
    
            }


            return 1;
        }

        private int GetTanshoOddz(String Key, ref List<String> OutParam)
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            int ret = db.TextReader_Row(Key, "O1", 0, ref OutParam);


            return 0;
        }
    }
}
