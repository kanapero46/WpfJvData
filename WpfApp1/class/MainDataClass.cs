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
    }
}
