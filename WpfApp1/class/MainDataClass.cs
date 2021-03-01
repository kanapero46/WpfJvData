using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Class
{
    public class MainDataClass
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
        public void setRaceCource(String rc) { this.RaceCource = rc; }

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
        //レース名(英語)
        private String RaceNameEng;
        public String getRaceNameEng() { return RaceNameEng; }
        public void setRaceNameEng(String inParam) { this.RaceNameEng = inParam; }

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

        //馬場状態
        private String Weather;
        public String getWeather() { return Weather; }
        public void setWeather(String inParam) { this.Weather = inParam; }

        //馬場状態
        private String TrackStatus;
        public String getTrackStatus() { return TrackStatus; }
        public void setTrackStatus(String inParam)
        {
            String tmp = "";
            try
            {
                if (inParam.Substring(0, 1) == "0")
                {
                    this.TrackStatus = inParam.Substring(1, 1);
                }
                else
                {
                    this.TrackStatus = inParam.Substring(0, 1);
                }
            }
            catch (Exception)
            {
                this.TrackStatus = inParam;
            }
        }

        //レース名副題
        private String RaceNameFuku;
        public String getRaceNameFukus() { return RaceNameFuku; }
        public void setRaceNameFuku(String inParam) { this.RaceNameFuku = inParam; }

        //レース名副題
        private String RaceNameEnd;
        public String getRaceNameEnd() { return RaceNameEnd; }
        public void setRaceNameEnd(String inParam) { this.RaceNameEnd = inParam; }

        //レース名6文字
        private String RaceName6;
        public String getRaceName6() { return RaceName6; }
        public void setRaceName6(String inParam) { this.RaceName6 = inParam; }

        //レコード更新区分
        private Boolean RecordUp;
        public Boolean getRecordFlag() { return RecordUp; }
        public void setRecordFlag(String inParam) { this.RecordUp = (inParam == "0" ? false : true); }

        //データ区分
        private String DataKubun;
        public String DataKubun1 { get => DataKubun; set => DataKubun = value; }

        private int Tosu;
        public int Tosu1 { get => Tosu; set => Tosu = value; }


        //ラップタイム
        protected List<String> LapTime;

        public void SetLapTime1(String[] In)
        {
            for(int i=0; i<In.Length; i++)
            {
                LapTime.Add(In[i]);
            }
        }

        public void SetLapTime2(String In)
        {
            LapTime.Add(In);
        }


        public List<string> LapTime1 { get => LapTime; set => LapTime = value; }

        public String ConvertDateToDate(String Date)
        {
            return Date.Substring(0, 4) + "年" + Int32.Parse(Date.Substring(4, 2)) + "月" + Int32.Parse(Date.Substring(6, 2)) + "日";
        }

       public String ConvertToHappyoTime(String date)
        {
            return date.Substring(0, 2) + "月" + date.Substring(2, 2) + "日 " + date.Substring(4, 2) + "時" + date.Substring(6, 2) + "分";
        }

        public String ConvertTimeToString(String inparam)
        {
            try
            {
                return inparam.Substring(0, 2) + "時" + inparam.Substring(2, 2) + "分";
            }
            catch(Exception)
            {
                return "";
            }
        }

        public String ConvertRunTimeToString(String time)
        {
            try
            {
                return time.Substring(0, 1) + ":" + time.Substring(1, 2) + "." + time.Substring(3, 1);
            }
            catch(Exception)
            {
                return "0:00.0";
            }
        }

    }
}
