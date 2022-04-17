using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    /* 馬体重情報保持クラス(WH) */
    class JvWhData
    {

        /**
         *   tmp += JvWh.id.Year + JvWh.id.MonthDay + JvWh.id.JyoCD + JvWh.id.Kaiji + JvWh.id.RaceNum + JvWh.BataijyuInfo[i].Umaban + ",";
                    tmp += JvWh.BataijyuInfo[i].Umaban + ",";
                    tmp += JvWh.BataijyuInfo[i].BaTaijyu + ","; //馬体重
                    tmp += JvWh.BataijyuInfo[i].ZogenFugo + ",";//増減符号
                    tmp += JvWh.BataijyuInfo[i].ZogenSa + ",";  //数値じゃない場合もあり→スペース(出走取消・初出走)
                    tmp += JvWh.crlf;
    */

        private String Key;
        private int Umaban;
        private int Bataiju;
        private String Fugo;
        private String Zogensa;

        public string Key1 { get => Key; set => Key = value; }
        public int Umaban1 { get => Umaban; set => Umaban = value; }
        public int Bataiju1 { get => Bataiju; set => Bataiju = value; }
        public string Fugo1 { get => Fugo; set => Fugo = value; }
        public string Zogensa1 { get => Zogensa; set => Zogensa = value; }
    }
}
