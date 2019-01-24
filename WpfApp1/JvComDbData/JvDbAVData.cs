using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.dbAccess;

namespace WpfApp1.JvComDbData
{
    class JvDbAVData
    {
        /* 出走取消・競走除外データクラス */
        private String Key;
        private int RaceNum;
        private int Kubun;  //１：出走取消　２：競走除外
        private int Umaban;
        private String Bamei;
        private int Riyuu;

        dbConnect db = new dbConnect();
        JVData_Struct.JV_AV_INFO JvData = new JVData_Struct.JV_AV_INFO();

        public string Key1 { get => Key; set => Key = value; }
        public int RaceNum1 { get => RaceNum; set => RaceNum = value; }
        public int Kubun1 { get => Kubun; set => Kubun = value; }
        public int Umaban1 { get => Umaban; set => Umaban = value; }
        public string Bamei1 { get => Bamei; set => Bamei = value; }
        public int Riyuu1 { get => Riyuu; set => Riyuu = value; }

        public JvDbAVData()
        {
            /* 引数なしはなにもしない */
        }

        public JvDbAVData(ref String buff, Boolean Init)
        {
            int ret = 0;
            String tmp = "#出走取消・競走除外情報";
            JvData.SetDataB(ref buff);

            /* イニシャライズ時に引数にtrueをセットするとDBを初期化する */
            if (Init)
            {
                db.DeleteCsv("AV");
                /* ヘッダー書き込み */
                db = new dbConnect(JvData.id.Year + JvData.id.MonthDay, JvData.head.RecordSpec, ref tmp, ref ret);
                SetDataAV(ref buff);
            }
            else
            {
                SetDataAV(ref buff);
            }
        }

        public void SetDataAV(ref String buff)
        {
            
            String tmp = "";
            int ret = 0;
            JvData.SetDataB(ref buff);
            Key = JvData.id.Year + JvData.id.MonthDay + JvData.id.JyoCD + JvData.id.Kaiji + JvData.id.Nichiji + JvData.id.RaceNum;
            RaceNum = Int32.Parse(JvData.id.RaceNum);
            Kubun = Int32.Parse(JvData.head.DataKubun);
            Umaban = Int32.Parse(JvData.Umaban);
            Bamei = JvData.Bamei.Trim();
            Riyuu = Int32.Parse(JvData.JiyuKubun);

            /* DB書き込み */
            tmp = "";
            tmp += Key + "," + RaceNum + "," + Kubun + "," + Umaban + "," + Bamei + "," + Riyuu + ","  ;

            db = new dbConnect(JvData.id.Year + JvData.id.MonthDay, JvData.head.RecordSpec, ref tmp, ref ret);
        }

        public int ReadData_AV(ref List<String> inParam)
        {
            try
            {
                Key = inParam[0];
                RaceNum = Int32.Parse(inParam[1]);
                Kubun = Int32.Parse(inParam[2]);
                Umaban = Int32.Parse(inParam[3]);
                Bamei = inParam[4];
                Riyuu = Int32.Parse(inParam[5]);
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
