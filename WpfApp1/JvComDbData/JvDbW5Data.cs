using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    public class JvDbW5Data
    {
        const int MAX_WIN5 = 5;

        private String date;
        private String boteCount;
        private String caleeOver;

        private String hitCount;
        private String payMoney;
        private String nextCo;

        private Boolean hitFlag;
        private int win5Status;
        private int hitCombi;    //同着数（通常は１だが、同着等で的中が２組ある場合は2にする）


        private String[] Key;
        private int[] raceNum;
        private String[] umaban;

        public string Date { get => date; set => date = value; }
        public string BoteCount { get => boteCount; set => boteCount = value; }
        public string CaleeOver { get => caleeOver; set => caleeOver = value; }
        public string HitCount { get => hitCount; set => hitCount = value; }
        public string PayMoney { get => payMoney; set => payMoney = value; }
        public string NextCo { get => nextCo; set => nextCo = value; }
        public bool HitFlag { get => hitFlag; set => hitFlag = value; }
        public int Win5Status { get => win5Status; set => win5Status = value; }
        public int HitCombi { get => hitCombi; set => hitCombi = value; }
        public string[] Key1 { get => Key; set => Key = value; }
        public int[] RaceNum { get => raceNum; set => raceNum = value; }
        public String[] Umaban { get => umaban; set => umaban = value; }

        public JvDbW5Data()
        {
            Key = new string[MAX_WIN5];
            RaceNum = new int[MAX_WIN5];
            Umaban = new String[MAX_WIN5];
        }

        public void SetW5Data(ref String buff)
        {
            JVData_Struct.JV_WF_INFO W5 = new JVData_Struct.JV_WF_INFO();
            W5.SetDataB(ref buff);
            Date = W5.KaisaiDate.Year + W5.KaisaiDate.Month + W5.KaisaiDate.Day;
            boteCount = W5.Hatsubai_Hyo;
            caleeOver = W5.COShoki;
            hitCount = W5.WFPayInfo[0].Tekichu_Hyo;
            payMoney = W5.WFPayInfo[0].Pay;
            nextCo = W5.COZanDaka;
            hitFlag = (W5.TekichunashiFlag == "1" ? true : false );
            win5Status = Int32.Parse(W5.head.DataKubun);
           
            for(int i=0; i<W5.WFPayInfo.Length; i++)
            {
                try
                {
                    if (W5.WFPayInfo[i].Pay.Trim() == "")
                    {

                    }
                    else
                    {
                        hitCombi++;
                    }
                }
                catch(Exception)
                {

                }          
            }

            for (int i = 0; i < W5.WFRaceInfo.Length; i++)
            {
                Key[i] = Date + W5.WFRaceInfo[i].JyoCD + W5.WFRaceInfo[i].Kaiji + W5.WFRaceInfo[i].Nichiji + W5.WFRaceInfo[i].RaceNum;
                raceNum[i] = Int32.Parse(W5.WFRaceInfo[i].RaceNum);
                umaban[i] = W5.WFPayInfo[i].Kumiban;

            }

        }

        public String JvDbW5Satus(String status)
        {
            switch(status)
            {
                case "1":
                    return "発売中";
                case "2":
                    return "レース中";
                case "3":
                case "7":
                    return "確定";
                case "9":
                    return "発売中止";
                case "0":
                    return "";
                default:
                    return "";       　
            }

        }
    }
}
