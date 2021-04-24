﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.dbAccess;

namespace WpfApp1.JvComDbData
{
    public class JvDbJcData
    {

        public struct JV_JC_DATA
        {
            public String Name;
            public String JcokeyCode;
            public String MinaraiCd;
            public String Futan;
        }

        /* 出走取消・競走除外データクラス */
        private String Key;
        private int RaceNum;
        private int Umaban;
        private String Bamei;
        private String Time;
        private JV_JC_DATA BeforeInfo;  //変更前
        private JV_JC_DATA AfterInfo;   //変更後

        dbConnect db = new dbConnect();
        JVData_Struct.JV_JC_INFO JvData = new JVData_Struct.JV_JC_INFO();

        public string Key1 { get => Key; set => Key = value; }
        public int RaceNum1 { get => RaceNum; set => RaceNum = value; }
        public int Umaban1 { get => Umaban; set => Umaban = value; }
        public string Bamei1 { get => Bamei; set => Bamei = value; }
        public string Time1 { get => Time; set => Time = value; }
        public JV_JC_DATA BeforeInfo1 { get => BeforeInfo; set => BeforeInfo = value; }
        public JV_JC_DATA AfterInfo1 { get => AfterInfo; set => AfterInfo = value; }

        public JvDbJcData()
        {
            /* 引数なしはなにもしない */
        }

        public JvDbJcData(ref String buff, Boolean Init, int idx)
        {
            int ret = 0;
            String tmp = "#騎手変更";
            JvData.SetDataB(ref buff);

            /* イニシャライズ時に引数にtrueをセットするとDBを初期化する */
            if (Init)
            {
                db.DeleteCsv("JC", JvData.id.Year + JvData.id.MonthDay + "/" + JvData.head.RecordSpec + JvData.id.Year + JvData.id.MonthDay + ".csv", true);
                /* ヘッダー書き込み */
                db = new dbConnect(JvData.id.Year + JvData.id.MonthDay, JvData.head.RecordSpec, ref tmp, ref ret);
                SetDataAV(ref buff);
            }
            else
            {
                SetDataAV(ref buff);
            }
        }

        //DB初期化なし・重複あり
        public void RestrictJCData(ref String buff)
        {
            int ret = 0;
            String tmp = "#騎手変更2";
            JvData.SetDataB(ref buff);
            SetDataAV(ref buff);
        }

        public String GetKey()
        {
            return Key1;
        }

        public JvDbJcData(ref String buff, String Date, Boolean Init)
        {
            //インデックスがわからない場合の処理(ただし処理はおそそう)
            int idx = 0;
            List<String> tmpArray = new List<string>();

            //DBから全データを読み込む
        //    db.TextReader_Col(Date, "JC", 0, ref tmpAqrray, buff.Substring(2, 12));

        }

        private void SetDataAV(ref String buff)
        {
            
            String tmp = "";
            int ret = 0;

            /* DB書き込み */
            tmp = "";
            //tmp += idx + ","; →　いらない
            tmp += JvData.id.Year + JvData.id.MonthDay + JvData.id.JyoCD + JvData.id.Kaiji + JvData.id.Nichiji + JvData.id.RaceNum + JvData.Umaban + ",";
            tmp += JvData.id.RaceNum + ",";
            tmp += JvData.Umaban + ",";
            tmp += JvData.Bamei.Trim() + ",";
            tmp += JvData.HappyoTime.Month + JvData.HappyoTime.Day + JvData.HappyoTime.Hour + JvData.HappyoTime.Minute + ",";
            //変更前
            tmp += JvData.JCInfoBefore.KisyuName.Trim() + ","; //[5]
            tmp += JvData.JCInfoBefore.KisyuCode + ",";
            tmp += JvData.JCInfoBefore.MinaraiCD + ",";
            tmp += JvData.JCInfoBefore.Futan + ",";
            //変更後
            tmp += JvData.JCInfoAfter.KisyuName.Trim() + ","; //[9]
            tmp += JvData.JCInfoAfter.KisyuCode + ",";
            tmp += JvData.JCInfoAfter.MinaraiCD + ",";
            tmp += JvData.JCInfoAfter.Futan + ",";

            //ローカルクラスにも入れておく
            JV_JC_DATA JockeyInfo;
            Key1 = JvData.id.Year + JvData.id.MonthDay + JvData.id.JyoCD + JvData.id.Kaiji + JvData.id.Nichiji + JvData.id.RaceNum + JvData.Umaban;
            RaceNum1 = Int32.Parse(JvData.id.RaceNum);
            Umaban1 = Int32.Parse(JvData.Umaban);
            Bamei1 = JvData.Umaban.Trim();
            Time1 = JvData.HappyoTime.Month + JvData.HappyoTime.Day + JvData.HappyoTime.Hour + JvData.HappyoTime.Minute;
            JockeyInfo.Name = JvData.JCInfoBefore.KisyuName.Trim();
            JockeyInfo.JcokeyCode = JvData.JCInfoBefore.KisyuCode;
            JockeyInfo.MinaraiCd = JvData.JCInfoBefore.MinaraiCD;
            JockeyInfo.Futan = JvData.JCInfoBefore.Futan;
            BeforeInfo1 = JockeyInfo;
            JockeyInfo = new JV_JC_DATA();

            JockeyInfo.Name = JvData.JCInfoAfter.KisyuName.Trim();
            JockeyInfo.JcokeyCode = JvData.JCInfoAfter.KisyuCode;
            JockeyInfo.MinaraiCd = JvData.JCInfoAfter.MinaraiCD;
            JockeyInfo.Futan = JvData.JCInfoAfter.Futan;
            AfterInfo1 = JockeyInfo;


            db = new dbConnect(JvData.id.Year + JvData.id.MonthDay, JvData.head.RecordSpec, ref tmp, ref ret);
        }

        public int ReadData_AV(ref List<String> inParam)
        {
            JV_JC_DATA JockeyInfo;

            try
            {
                Key1 = inParam[0];
                RaceNum1 = Int32.Parse(inParam[1]);
                Umaban1 = Int32.Parse(inParam[2]);
                Bamei1 = inParam[3];
                Time1 = inParam[4];
                JockeyInfo.Name = inParam[5];
                JockeyInfo.JcokeyCode = inParam[6];
                JockeyInfo.MinaraiCd = inParam[7];
                JockeyInfo.Futan = inParam[8];
                BeforeInfo1 = JockeyInfo;
                JockeyInfo = new JV_JC_DATA();

                JockeyInfo.Name = inParam[9];
                JockeyInfo.JcokeyCode = inParam[10];
                JockeyInfo.MinaraiCd = inParam[11];
                JockeyInfo.Futan = inParam[12];
                AfterInfo1 = JockeyInfo;
                return 1;
            }
            catch(Exception e)
            {
                Console.WriteLine("ReadData_AV Error!!\n" + e.Message);
                return 0;
            }

        }
    }
}
