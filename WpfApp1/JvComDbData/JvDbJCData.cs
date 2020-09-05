using System;
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
                SetDataAV(ref buff, idx);
            }
            else
            {
                SetDataAV(ref buff, idx);
            }
        }

        public JvDbJcData(ref String buff, String Date, Boolean Init)
        {
            //インデックスがわからない場合の処理(ただし処理はおそそう)
            int idx = 0;
            List<String> tmpArray = new List<string>();

            //DBから全データを読み込む
        //    db.TextReader_Col(Date, "JC", 0, ref tmpAqrray, buff.Substring(2, 12));

        }

        private void SetDataAV(ref String buff, int idx)
        {
            
            String tmp = "";
            int ret = 0;

            /* DB書き込み */
            tmp = "";
            tmp += idx + ",";
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

            db = new dbConnect(JvData.id.Year + JvData.id.MonthDay, JvData.head.RecordSpec, ref tmp, ref ret);
        }

        public int ReadData_AV(ref List<String> inParam)
        {
            JV_JC_DATA JockeyInfo;

            try
            {
                Key1 = inParam[1];
                RaceNum1 = Int32.Parse(inParam[2]);
                Umaban1 = Int32.Parse(inParam[3]);
                Bamei1 = inParam[4];
                Time1 = inParam[5];
                JockeyInfo.Name = inParam[6];
                JockeyInfo.JcokeyCode = inParam[7];
                JockeyInfo.MinaraiCd = inParam[8];
                JockeyInfo.Futan = inParam[9];
                BeforeInfo1 = JockeyInfo;
                JockeyInfo = new JV_JC_DATA();

                JockeyInfo.Name = inParam[10];
                JockeyInfo.JcokeyCode = inParam[11];
                JockeyInfo.MinaraiCd = inParam[12];
                JockeyInfo.Futan = inParam[13];
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
