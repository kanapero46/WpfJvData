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
            private String rA_KEY;
            private String raceName;
            private String raceName10;
            private String grade;
            private String track;
            private String distance;
            private int tousuu;
            private String weather;
            private String babaStat;
            private String lapTime;
            private String last3f;
            private String courner;
            private String sE_KEY;
            private int umaban;
            private String kettouNum;
            private String futan;
            private String jockey;
            private int bataiju;
            private String zougenCD;
            private String diff;
            private String rank;
            private String time;
            private String counerRank;
            private int tansyoRank;
            private String myLast3f;
            private String aiteuma;
            private String timeDiff;
        };

        
        private String KEY;
        private String Waku;
        private String Umaban;
        private String Name;
        private String Jockey;
        private String Futan;
        private String MinaraiCd;
        private String F;
        private String FF;
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
        public string FF1 { get => FF; set => FF = value; }
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
            Waku = inParam[5];
            Umaban = inParam[6];
            KettoNum = Int32.Parse(inParam[7]);
            Name = inParam[8];
            Jockey = inParam[16];
            Futan = inParam[15];
            MinaraiCd = inParam[17];
            UmaKigou = inParam[9];
        }

        public void SetUMData(List<String> inParam)
        {
            F = inParam[6];
            M = inParam[7];
            FF = inParam[8];
            FM = inParam[9];
            FFM = inParam[10];
            F_NUM = inParam[15];
            FM_NUM = inParam[16];
            FFM_NUM = inParam[17];
            FF_NUM = inParam[18];
            FFF_NUM = inParam[19];
            FMM_NUM = inParam[20];
            EngName = inParam[2];
        }


    }
}
