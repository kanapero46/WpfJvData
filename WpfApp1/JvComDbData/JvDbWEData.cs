using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Class;

namespace WpfApp1.JvComDbData
{
    class JvDbWEData
    {
        dbAccess.dbConnect db = new dbAccess.dbConnect();
        JVData_Struct.JV_WE_WEATHER JV_WEATHER = new JVData_Struct.JV_WE_WEATHER();

        public void JvDbInitData()
        {
            db.DeleteCsv("WE");
            String buff = "#天候・馬場状態保持クラス";
            int ret = 0;
            db = new dbAccess.dbConnect("WE", ref buff, ref ret);
        }

        public void JvDbWeSetData(String buff)
        {

            String tmp = "";
            Boolean Flag = false;
            List<Class.WeatherCourceStatus> WCstatus = new List<WeatherCourceStatus>();

            JV_WEATHER.SetDataB(ref buff);
            
            tmp = JV_WEATHER.id.Year + JV_WEATHER.id.MonthDay + JV_WEATHER.id.JyoCD + JV_WEATHER.id.Kaiji + JV_WEATHER.id.Nichiji;
            //tmp = "";
            //tmp += JV_WEATHER.id.Year + JV_WEATHER.id.MonthDay + JV_WEATHER.id.JyoCD + JV_WEATHER.id.Kaiji + JV_WEATHER.id.Nichiji + ",";
            //tmp += JV_WEATHER.TenkoBaba.TenkoCD + ",";
            //tmp += JV_WEATHER.TenkoBaba.SibaBabaCD + ",";
            //tmp += JV_WEATHER.TenkoBaba.DirtBabaCD + ",";
            //tmp += JV_WEATHER.HappyoTime.Month + JV_WEATHER.HappyoTime.Day + JV_WEATHER.HappyoTime.Hour + JV_WEATHER.HappyoTime.Minute + ",";
            Flag = false;
            for (int i = 0; i < WCstatus.Count(); i++)
            {
                if (WCstatus[i].Key == tmp)
                {
                    WCstatus[i].SetAllStatus(tmp,
                        JV_WEATHER.TenkoBaba.TenkoCD, JV_WEATHER.TenkoBaba.SibaBabaCD, JV_WEATHER.TenkoBaba.DirtBabaCD,
                        JV_WEATHER.HappyoTime.Month + JV_WEATHER.HappyoTime.Day + JV_WEATHER.HappyoTime.Hour + JV_WEATHER.HappyoTime.Minute);
                    Flag = true;
                    break;   //次のループへ
                }
            }

            if (Flag == false)
            {
                //ここに来たらデータなしのため配列を作成
                WeatherCourceStatus tmpClass = new WeatherCourceStatus();
                tmpClass.SetAllStatus(tmp,
                            JV_WEATHER.TenkoBaba.TenkoCD, JV_WEATHER.TenkoBaba.SibaBabaCD, JV_WEATHER.TenkoBaba.DirtBabaCD,
                            JV_WEATHER.HappyoTime.Month + JV_WEATHER.HappyoTime.Day + JV_WEATHER.HappyoTime.Hour + JV_WEATHER.HappyoTime.Minute);
                WCstatus.Add(tmpClass);
            }

    
        }
    }
}
