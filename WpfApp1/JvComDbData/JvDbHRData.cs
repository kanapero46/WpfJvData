using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    class JvDbHRData : JvDbHRInfoData
    {
        String DbWriteKey = "";
        String DbWriteStr = "";
        int DateSpec = 0;

        public int RaceNum { get; set; }
        public String Cource { get; set; }

        public void SetHrData(ref String buff)
        {
            String tmp = "";
            String tmp2 = "";
            String EnterCode = "\r\n";

            JVData_Struct.JV_HR_PAY JvPay = new JVData_Struct.JV_HR_PAY();
            int tmpInteger = 0;
            int i = 0;

            JvPay.SetDataB(ref buff);
            tmp += JvPay.id.Year + JvPay.id.MonthDay + JvPay.id.JyoCD + JvPay.id.Kaiji + JvPay.id.Nichiji + JvPay.id.RaceNum + ",";
            tmp += JvPay.head.DataKubun + EnterCode;
            DbWriteKey = JvPay.id.Year + JvPay.id.MonthDay + JvPay.id.JyoCD + JvPay.id.Kaiji + JvPay.id.Nichiji + JvPay.id.RaceNum;

            if (JvPay.head.DataKubun == "3")
            {
                //レース中止
            }
            else
            {

                for(i = 0; i<JvPay.PayTansyo.Length; i++)
                {
                    tmp += "単勝" + i.ToString() + ",";
                    tmp += (i + 1).ToString() + ",";
                    if(i==0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[0] + ",";
                        tmp += JvPay.TokubaraiFlag[0] + ",";
                        tmp += JvPay.HenkanFlag[0] + ",";
                    }
                    tmp += JvPay.PayTansyo[i].Umaban + ",";
                    tmp += JvPay.PayTansyo[i].Pay + ",";
                    tmp += JvPay.PayTansyo[i].Ninki + ",";
                    tmp += EnterCode;
                }

                for (i = 0; i < JvPay.PayFukusyo.Length; i++)
                {
                    tmp += "複勝" + i.ToString() + ",";
                    tmp += (i + 1).ToString() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[1] + ",";
                        tmp += JvPay.TokubaraiFlag[1] + ",";
                        tmp += JvPay.HenkanFlag[1] + ",";
                    }
                    tmp += JvPay.PayFukusyo[i].Umaban + ",";
                    tmp += JvPay.PayFukusyo[i].Pay + ",";
                    tmp += JvPay.PayFukusyo[i].Ninki + ",";
                    tmp += EnterCode;
                }

                for (i = 0; i < JvPay.PayWakuren.Length; i++)
                {
                    tmp += "枠連" + i.ToString() + ",";
                    tmp += (i + 1).ToString() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[2] + ",";
                        tmp += JvPay.TokubaraiFlag[2] + ",";
                        tmp += JvPay.HenkanFlag[2] + ",";
                    }
                    tmp += JvPay.PayWakuren[i].Umaban + ",";
                    tmp += JvPay.PayWakuren[i].Pay + ",";
                    tmp += JvPay.PayWakuren[i].Ninki + ",";
                    tmp += EnterCode;
                }

                for (i = 0; i < JvPay.PayUmaren.Length; i++)
                {
                    tmp += "馬連" + i.ToString() + ",";
                    tmp += (i + 1).ToString() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[3] + ",";
                        tmp += JvPay.TokubaraiFlag[3] + ",";
                        tmp += JvPay.HenkanFlag[3] + ",";
                    }
                    tmp += JvPay.PayUmaren[i].Kumi + ",";
                    tmp += JvPay.PayUmaren[i].Pay + ",";
                    tmp += JvPay.PayUmaren[i].Ninki + ",";
                    tmp += EnterCode;
                }

                for (i = 0; i < JvPay.PayWide.Length; i++)
                {
                    tmp += "ワイド" + i.ToString() + ",";
                    tmp += (i + 1).ToString() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[4] + ",";
                        tmp += JvPay.TokubaraiFlag[4] + ",";
                        tmp += JvPay.HenkanFlag[4] + ",";
                    }
                    tmp += JvPay.PayWide[i].Kumi + ",";
                    tmp += JvPay.PayWide[i].Pay + ",";
                    tmp += JvPay.PayWide[i].Ninki + ",";
                    tmp += EnterCode;
                }
                
                for (i = 0; i < JvPay.PayUmatan.Length; i++)
                {
                    tmp += "馬単" + i.ToString() + ",";
                    tmp += (i + 1).ToString() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[6] + ",";
                        tmp += JvPay.TokubaraiFlag[6] + ",";
                        tmp += JvPay.HenkanFlag[6] + ",";
                    }
                    tmp += JvPay.PayWide[i].Kumi + ",";
                    tmp += JvPay.PayWide[i].Pay + ",";
                    tmp += JvPay.PayWide[i].Ninki + ",";
                    tmp += EnterCode;
                }

                for (i = 0; i < JvPay.PaySanrenpuku.Length; i++)
                {
                    tmp += "3連複" + i.ToString() + ",";
                    tmp += (i + 1).ToString() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[7] + ",";
                        tmp += JvPay.TokubaraiFlag[7] + ",";
                        tmp += JvPay.HenkanFlag[7] + ",";
                    }
                    tmp += JvPay.PaySanrenpuku[i].Kumi + ",";
                    tmp += JvPay.PaySanrenpuku[i].Pay + ",";
                    tmp += JvPay.PaySanrenpuku[i].Ninki + ",";
                    tmp += EnterCode;
                }

                for (i = 0; i < JvPay.PaySanrentan.Length; i++)
                {
                    tmp += "3連単" + i.ToString() + ",";
                    tmp += (i + 1).ToString() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[8] + ",";
                        tmp += JvPay.TokubaraiFlag[8] + ",";
                        tmp += JvPay.HenkanFlag[8] + ",";
                    }
                    tmp += JvPay.PaySanrentan[i].Kumi + ",";
                    tmp += JvPay.PaySanrentan[i].Pay + ",";
                    tmp += JvPay.PaySanrentan[i].Ninki + ",";
                    tmp += EnterCode;
                }
            }
            Cource = JvPay.id.JyoCD;
            RaceNum = Int32.Parse(JvPay.id.RaceNum);
            DateSpec = Int32.Parse(JvPay.head.RecordSpec);
            DbWriteStr += tmp + "\r\n";
            //dbAccess.dbConnect db = new dbAccess.dbConnect(JvPay.id.Year + JvPay.id.MonthDay, JvPay.head.RecordSpec, ref tmp, ref tmpInteger);
               
        }

        public int JvWriteHrData()
        {
            int tmpInteger = 0;
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> tmpArray = new List<string>();
            if(db.TextReader_Col(DbWriteKey, "HR", 0, ref tmpArray, DbWriteKey) ==0)
            {
                db.DeleteCsv("HR", DbWriteKey);
            }           
                
            db = new dbAccess.dbConnect(DbWriteKey, "HR", ref DbWriteStr, ref tmpInteger);
            return tmpInteger;
        }

        #region 払戻通知を読み込みWindowsへ通知する
        public void JvHrNoticeWindows()
        {
            Class.com.windows.JvComWindowsForm WindowsNotice = new Class.com.windows.JvComWindowsForm();
            String title = "";
            String msg = "";
            int kind = Class.com.windows.JvComWindowsForm.INFO_STATUS;
            List<String> tmpArary = new List<string>();
            String[] CsvTopString = new string[37];

            //払戻読み込み
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            InitCsvTopString(ref CsvTopString);
            for(int i=0; i<CsvTopString.Length; i++)
            {
                db.TextReader_Col(DbWriteKey, "HR", 0, ref tmpArary, CsvTopString[i]);
            }


            


            //Windowsのポップアップ通知を出力する
            WindowsNotice.JvComNoticeShow(kind, "【払戻通知：中山11R】スプリングＳ(Ⅱ)", "[Ⅰ]9\t[Ⅱ]10\t[Ⅲ]1\r【単勝】9 (\\2710)");
        }
        #endregion

        public void GetPayInfo(ref String inKey, ref String inCource, ref int inRacenNum)
        {
            inKey = this.DbWriteKey;
            inCource = this.Cource;
            inRacenNum = this.RaceNum;
        }

 
    }
}
