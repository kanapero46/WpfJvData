using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.form.info.backClass
{
    public class baclClassInfo
    {
        private Boolean WeatherFlag;
        private String turfStatus;
        private String dirtStatus;
        private String weather;

        public struct INFO_COND
        {
            public Boolean flag;
            public int InfoCount;
        }

        INFO_COND Jockey;       //騎手変更
        INFO_COND TimeChange;   //発走時刻
        INFO_COND CourceChange; //コース変更
        INFO_COND DNSInfo;      //出走取消・除外

        public string TurfStatus { get => turfStatus; set => turfStatus = value; }
        public string DirtStatus { get => dirtStatus; set => dirtStatus = value; }
        public string Weather { get => weather; set => weather = value; }
        internal INFO_COND Jockey1 { get => Jockey; set => Jockey = value; }
        internal INFO_COND TimeChange1 { get => TimeChange; set => TimeChange = value; }
        internal INFO_COND CourceChange1 { get => CourceChange; set => CourceChange = value; }
        internal INFO_COND DNSInfo1 { get => DNSInfo; set => DNSInfo = value; }
        public bool WeatherFlag1 { get => WeatherFlag; set => WeatherFlag = value; }

        public void SetJockeyInfo()
        {
            Jockey.flag = true;
            Jockey.InfoCount++;
        }

        public void SetTimeInfo()
        {
            TimeChange.flag  = true;
            TimeChange.InfoCount++;
        }

        public void SetCourceInfo()
        {
            CourceChange.flag = true;
            CourceChange.InfoCount++;
        }

        public void SetDNSInfo()
        {
            DNSInfo.flag = true;
            DNSInfo.InfoCount++;
        }
    }
}
