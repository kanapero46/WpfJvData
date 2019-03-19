using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    class JvDbHRData
    {

        public int RaceNum { get; set; }
        public String Cource { get; set; }

        public void SetHrData(ref String buff)
        {
            String tmp = "";
            String tmp2 = "";
            JVData_Struct.JV_HR_PAY JvPay = new JVData_Struct.JV_HR_PAY();
            int tmpInteger = 0;

            JvPay.SetDataB(ref buff);
            tmp += JvPay.id.Year + JvPay.id.MonthDay + JvPay.id.JyoCD + JvPay.id.Kaiji + JvPay.id.Nichiji + JvPay.id.RaceNum + ",";
            tmp += JvPay.head.DataKubun + ",";
            if (JvPay.head.DataKubun == "3")
            {
                //レース中止
            }
            else
            {
                for (int i = 0; i < JvPay.TokubaraiFlag.Length; i++)
                {
                    tmp += JvPay.TokubaraiFlag[i];
                }
                tmp += ",";

                for (int j = 0; j < JvPay.HenkanUma.Length; j++)
                {
                    tmp2 = JvPay.HenkanUma[j];
                }
                if (Int32.TryParse(tmp2, out tmpInteger))
                {
                    if (tmpInteger == 0)
                    {
                        //取消なし
                        tmp += "0";
                    }
                    else
                    {
                        //取消馬あり
                        tmp += tmp2;
                    }
                }
                else
                {
                    //エラーの場合はそのまま
                    tmp += tmp2;
                }
                tmp += ",";
            }
            Cource = JvPay.id.JyoCD;
            RaceNum = Int32.Parse(JvPay.id.RaceNum);
            dbAccess.dbConnect db = new dbAccess.dbConnect(JvPay.id.Year + JvPay.id.MonthDay, JvPay.head.RecordSpec, ref tmp, ref tmpInteger);
               
        }

        public void GetPayInfo(ref String inCource, ref int inRacenNum)
        {
            inCource = this.Cource;
            inRacenNum = this.RaceNum;
        }
    }
}
