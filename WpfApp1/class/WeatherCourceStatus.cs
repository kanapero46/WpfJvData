using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Class
{
    #region 天候・馬場状態保持クラス
    class WeatherCourceStatus
    {
        private String key;
        private String weather;
        private String turf;
        private String dirt;
        private String latestTime = "発売前";

        public string Weather { get => weather; set => weather = value; }
        public string Turf { get => turf; set => turf = value; }
        public string Dirt { get => dirt; set => dirt = value; }
        public string LatestTime { get => latestTime; set => latestTime = value; }
        public string Key { get => key; set => key = value; }

        public void SetAllStatus(String Key, String Weather, String Turf, String Dirt, String Time)
        {

            key = Key;

            if(Weather != "0")
            {
                SetWeather(Weather);
            }

            if(Turf != "0")
            {
                SetTurf(Turf);
            }

            if(Dirt != "0")
            {
                SetDirt(Dirt);
            }

            if (Time != "")
            {
                SetTime(Time);
            }
        }

        public void SetWeather(String inparam)
        {
            this.Weather = inparam;
        }

        public void SetTurf(String inparam)
        {
            this.Turf = inparam;
        }

         public void SetDirt(String inparam)
        {
            this.Dirt = inparam;
        }

        public void SetTime(String inparam)
        {
            this.LatestTime = inparam;
        }

         

    }
}
#endregion