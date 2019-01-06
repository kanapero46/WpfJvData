using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Class
{
    class MainDataClass
    {

        //レース情報キー
        private String RA_key;
        public String GET_RA_KEY() { return RA_key; }
        public void SET_RA_KEY(String RA_KEY) { this.RA_key = RA_KEY; }

        //日付
        private String RaceDate;
        public String getRaceDate() { return RaceDate; }
        public void setRaceDate(String inParam) { this.RaceDate = inParam; }

        //競馬場コード！！
        private String RaceCource = "";
        public String getRaceCource() { return RaceCource; }
        public void setRaceCoutce(String rc) { this.RaceCource = rc; }

        //レース番号
        private String RaceNum = "";
        public String getRaceNum() { return RaceNum; }
        public void setRaceNum(String inParam) { this.RaceNum = inParam; }

        //回次
        private String RaceKaiji;
        public String getRaceKaiji() { return RaceKaiji; }
        public void setRaceKaiji(String inParam) { this.RaceKaiji = inParam; }
       
        //日次
        private String RaceNichiji;
        public String getRaceNichiji() { return RaceNichiji; }
        public void setRaceNichiji(String inParam) { this.RaceNichiji = inParam; }

        //レース名(任意)
        private String RaceName;
        public String getRaceName() { return RaceName; }
        public void setRaceName(String inParam) { this.RaceName = inParam; }

        //レースグレード(任意)
        private String RaceGrade;
        public String getRaceGrade() { return RaceGrade; }
        public void setRaceGrade(String inParam) { this.RaceGrade = inParam; }

        //コース
        private String CourceTrack;
        public String getCourceTrack() { return CourceTrack; }
        public void setCourceTrack(String inParam) { this.CourceTrack = inParam; }

        //距離
        private String Distance;
        public String getDistance() { return Distance; }
        public void setDistance(String inParam) { this.Distance = inParam; }
        
        //曜日
        private String WeekDay;
        public String getWeekDay() { return WeekDay; }
        public void setWeekDay(String inParam) { this.WeekDay = inParam; }

        //出走年齢
        private String OldYear;
        public String getOldYear() { return OldYear; }
        public void setOldYear(String inParam) { this.OldYear = inParam; }

        //クラス
        private String RaceClass;
        public String getRaceClass() { return RaceClass; }
        public void setRaceClass(String inParam) { this.RaceClass = inParam; }

        //第○回
        private int RaceGradeKai;
        public int getRaceGradeKai() { return RaceGradeKai; }
        public void setRaceGradeKai(String inParam)
        {
            try
            {
                this.RaceGradeKai = Int32.Parse(inParam);
            }
            catch (Exception)
            {

            }
        }

        //競走記号
        private String RaceKindKigo;
        public String getRaceKindKigo() { return RaceKindKigo; }
        public void setRaceKindKigo(String inParam) { this.RaceKindKigo = inParam; }

        //重量コード
        private String RaceHandCap;
        public String getRaceHandCap() { return RaceHandCap; }
        public void setRaceHandCap(String inParam) { this.RaceHandCap = inParam; }

        //発走時間
        private String RaceStartTime;
        public String getRaceStartTime() { return RaceStartTime; }
        public void setRaceStartTime(String inParam) { this.RaceStartTime = inParam; }



        //RAキーの自動生成
        public int GET_AUTO_RA_KEY(ref String inParam)
        {
            if(RaceDate == "" || RaceCource == "" || RaceKaiji == "" || RaceNichiji == ""|| RaceNum == "")
            {
                return 0;
            }

            inParam = RaceDate + RaceCource + RaceKaiji + RaceNichiji + RaceNum;
            return 1;
        }

        //TextReader_Rowから読み込んだ配列からデータ・セット
        public void setData(ref List<String> inParam)
        {
            RA_key = inParam[0];
            RaceDate = inParam[1];
            RaceCource = inParam[2];
            RaceKaiji = inParam[3];
            RaceNichiji = inParam[4];
            RaceNum = inParam[5];
            WeekDay = inParam[6];
            RaceName = inParam[7];
            OldYear = inParam[13];
            RaceClass = inParam[14];
            RaceGradeKai = Int32.Parse(inParam[15]);
            RaceGrade = inParam[16];
            CourceTrack = inParam[17];
            Distance = inParam[18];
            RaceKindKigo = inParam[20];
            RaceHandCap = inParam[21];
            RaceStartTime = inParam[22];
        }
    }
}
