using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Class
{
    class MainDataHorceClass
    {
        public struct JV_DATA_RACE_HIST
        {
            public String RA_KEY;
            public String RaceName;
            public String RaceName10;
            public String Grade;
            public String Track;
            public String Distance;
            public int Tousuu;
            public String Weather;
            public String BabaStat;
            public String LapTime;
            public String Last3f;
            public String Courner;
            public String SE_KEY;
            public int Umaban;
            public String KettouNum;
            public String Futan;
            public String Jockey;
            public int Bataiju;
            public String ZougenCD;
            public String Diff;
            public String Rank;
            public String Time;
            public String CounerRank;
            public int TansyoRank;
            public String MyLast3f;
            public String Aiteuma;
            public String TimeDiff;       
        };

        
        private String KEY;
        private String Waku;
        private String Umaban;
        private String Name;
        private String Jockey;
        private String Futan;
        private String MinaraiCd;
        private String F;
        private String F_NUM;
        private String M;
        private String FM;
        private String FM_NUM;　//母父の番号
        private String FFM;
        private String FFM_NUM;　//母母父の番号
        private int KettoNum;
        private String FMM_NUM; //母父父の番号
        private String FF_NUM;  //父父の番号
        private String FFF_NUM; //父父父の番号
        private String UmaKigou;
        private JV_DATA_RACE_HIST RaceHist;
        private String EngName;




        public string KEY1 { get => KEY; set => KEY = value; }
        public string Waku1 { get => Waku; set => Waku = value; }
        public string Umaban1 { get => Umaban; set => Umaban = value; }
        public string Name1 { get => Name; set => Name = value; }
        public string Jockey1 { get => Jockey; set => Jockey = value; }
        public string Futan1 { get => Futan; set => Futan = value; }
        public string MinaraiCd1 { get => MinaraiCd; set => MinaraiCd = value; }
        public string F1 { get => F; set => F = value; }
        public string M1 { get => M; set => M = value; }
        public string FM1 { get => FM; set => FM = value; }
        public string FFM1 { get => FFM; set => FFM = value; }
        public int KettoNum1 { get => KettoNum; set => KettoNum = value; }
        public JV_DATA_RACE_HIST RaceHist1 { get => RaceHist; set => RaceHist = value; }
        public string F_NUM1 { get => F_NUM; set => F_NUM = value; }
        public string FM_NUM1 { get => FM_NUM; set => FM_NUM = value; }
        public string FFM_NUM1 { get => FFM_NUM; set => FFM_NUM = value; }
        public string FF_NUM1 { get => FF_NUM; set => FF_NUM = value; }
        public string FFF_NUM1 { get => FFF_NUM; set => FFF_NUM = value; }
        public string FMM_NUM1 { get => FMM_NUM; set => FMM_NUM = value; }
        public string UmaKigou1 { get => UmaKigou; set => UmaKigou = value; }
        public string EngName1 { get => EngName; set => EngName = value; }

        public void SetSEData(List<String> inParam)
        {
            KEY = inParam[0];
            Waku = inParam[4];
            Umaban = inParam[5];
            KettoNum = Int32.Parse(inParam[6]);
            Name = inParam[7];
            Jockey = inParam[12];
            MinaraiCd = inParam[13];
            UmaKigou = inParam[8];
        }


    }
}
