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

        Class.com.JvComClass LOG = new Class.com.JvComClass();  //ログ出力用クラス

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

            String[] PayNameArray = new string[36];
            InitCsvTopString(ref PayNameArray);
            int ArrayIdx = 0;   //配列の添字

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
                    tmp += PayNameArray[ArrayIdx] + ",";
                    tmp += (i + 1).ToString() + ",";
                    tmp += JvPay.PayTansyo[i].Umaban.Trim() + ",";
                    tmp += JvPay.PayTansyo[i].Pay.Trim() + ",";
                    tmp += JvPay.PayTansyo[i].Ninki.Trim() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[0] + ",";
                        tmp += JvPay.TokubaraiFlag[0] + ",";
                        tmp += JvPay.HenkanFlag[0] + ",";
                    }
                    tmp += EnterCode;
                    ArrayIdx++;
                }

                for (i = 0; i < JvPay.PayFukusyo.Length; i++)
                {
                    tmp += PayNameArray[ArrayIdx] + ",";
                    tmp += (i + 1).ToString() + ",";
                    tmp += JvPay.PayFukusyo[i].Umaban.Trim() + ",";
                    tmp += JvPay.PayFukusyo[i].Pay.Trim() + ",";
                    tmp += JvPay.PayFukusyo[i].Ninki.Trim() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[1] + ",";
                        tmp += JvPay.TokubaraiFlag[1] + ",";
                        tmp += JvPay.HenkanFlag[1] + ",";
                    }
                    tmp += EnterCode;
                    ArrayIdx++;
                }

                for (i = 0; i < JvPay.PayWakuren.Length; i++)
                {
                    tmp += PayNameArray[ArrayIdx] + ",";
                    tmp += (i + 1).ToString() + ",";
                    tmp += JvPay.PayWakuren[i].Umaban.Trim() + ",";
                    tmp += JvPay.PayWakuren[i].Pay.Trim() + ",";
                    tmp += JvPay.PayWakuren[i].Ninki.Trim() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[2] + ",";
                        tmp += JvPay.TokubaraiFlag[2] + ",";
                        tmp += JvPay.HenkanFlag[2] + ",";
                    }
                    tmp += EnterCode;
                    ArrayIdx++;
                }

                for (i = 0; i < JvPay.PayUmaren.Length; i++)
                {
                    tmp += PayNameArray[ArrayIdx] + ",";
                    tmp += (i + 1).ToString() + ",";
                    tmp += JvPay.PayUmaren[i].Kumi.Trim() + ",";
                    tmp += JvPay.PayUmaren[i].Pay.Trim() + ",";
                    tmp += JvPay.PayUmaren[i].Ninki.Trim() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[3] + ",";
                        tmp += JvPay.TokubaraiFlag[3] + ",";
                        tmp += JvPay.HenkanFlag[3] + ",";
                    }
                    tmp += EnterCode;
                    ArrayIdx++;
                }

                for (i = 0; i < JvPay.PayWide.Length; i++)
                {
                    tmp += PayNameArray[ArrayIdx] + ",";
                    tmp += (i + 1).ToString() + ",";
                    tmp += JvPay.PayWide[i].Kumi.Trim() + ",";
                    tmp += JvPay.PayWide[i].Pay.Trim() + ",";
                    tmp += JvPay.PayWide[i].Ninki.Trim() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[4] + ",";
                        tmp += JvPay.TokubaraiFlag[4] + ",";
                        tmp += JvPay.HenkanFlag[4] + ",";
                    }
                    tmp += EnterCode;
                    ArrayIdx++;
                }
                
                for (i = 0; i < JvPay.PayUmatan.Length; i++)
                {
                    tmp += PayNameArray[ArrayIdx] + ",";
                    tmp += (i + 1).ToString() + ",";
                    tmp += JvPay.PayUmatan[i].Kumi.Trim() + ",";
                    tmp += JvPay.PayUmatan[i].Pay.Trim() + ",";
                    tmp += JvPay.PayUmatan[i].Ninki.Trim() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[6] + ",";
                        tmp += JvPay.TokubaraiFlag[6] + ",";
                        tmp += JvPay.HenkanFlag[6] + ",";
                    }
                    tmp += EnterCode;
                    ArrayIdx++;
                }

                for (i = 0; i < JvPay.PaySanrenpuku.Length; i++)
                {
                    tmp += PayNameArray[ArrayIdx] + ",";
                    tmp += (i + 1).ToString() + ",";
                    tmp += JvPay.PaySanrenpuku[i].Kumi.Trim() + ",";
                    tmp += JvPay.PaySanrenpuku[i].Pay.Trim() + ",";
                    tmp += JvPay.PaySanrenpuku[i].Ninki.Trim() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[7] + ",";
                        tmp += JvPay.TokubaraiFlag[7] + ",";
                        tmp += JvPay.HenkanFlag[7] + ",";
                    }
                    tmp += EnterCode;
                    ArrayIdx++;
                }

                for (i = 0; i < JvPay.PaySanrentan.Length; i++)
                {
                    tmp += PayNameArray[ArrayIdx] + ",";
                    tmp += (i + 1).ToString() + ",";
                    tmp += JvPay.PaySanrentan[i].Kumi.Trim() + ",";
                    tmp += JvPay.PaySanrentan[i].Pay.Trim() + ",";
                    tmp += JvPay.PaySanrentan[i].Ninki.Trim() + ",";
                    if (i == 0)
                    {
                        //初回だけ、不成立フラグ・特払フラグ・返還フラグを入れる
                        tmp += JvPay.FuseirituFlag[8] + ",";
                        tmp += JvPay.TokubaraiFlag[8] + ",";
                        tmp += JvPay.HenkanFlag[8] + ",";
                    }
                    tmp += EnterCode;
                    ArrayIdx++;
                }
            }
            Cource = JvPay.id.JyoCD;
            RaceNum = Int32.Parse(JvPay.id.RaceNum);
            DateSpec = Int32.Parse(JvPay.head.DataKubun);
            DbWriteStr = tmp + "\r\n";
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
            List<String> tmpArray = new List<string>();
            String[] CsvTopString = new string[37];
            JvDbHRInfoData Info = new JvDbHRInfoData();

            Boolean SameFlag = false;   //同着フラグ

            List<EXT_PARAM> cachePayInfo = new List<EXT_PARAM>();

            //払戻情報のリセット
            InitJvDbHrInfoData();

            //払戻読み込み
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            InitCsvTopString(ref CsvTopString);
            for(int i=0; i<CsvTopString.Length; i++)
            {
                tmpArray.Clear();
                db.TextReader_Col(DbWriteKey, "HR", 0, ref tmpArray, CsvTopString[i]);
                SetPayInfo(ref tmpArray);
                //Console.WriteLine("PayInfo Count = " + i);

            }

            EXT_PARAM Param;
            for (int i=0; i<8; i++)
            {
                Param = new EXT_PARAM();
                if(i==0)
                {
                    GetPayInfo((PAY_BASE)i, 0, ref Param);   //単勝払戻取得
                }
                else if(i == (int)PAY_BASE.UMAREN)
                {
                    GetPayInfo((PAY_BASE)i, 0, ref Param);   //馬連
                }
                else if (i == (int)PAY_BASE.SANRENPUKU)
                {
                    GetPayInfo(PAY_BASE.SANRENPUKU, 0, ref Param);
                }
                else if(i == 7)
                {
                    GetPayInfo(PAY_BASE.SANRENTAN, 0, ref Param);
                }

                
                cachePayInfo.Add(Param);
            }

            if(cachePayInfo.Count >= 9)
            {
                SameFlag = true;    //同着あり
            }

            //文字列生成：着順は三連単の払戻から生成する
            msg += "[Ⅰ]" + ConvToRankStrToNumber(1, cachePayInfo[(int)PAY_BASE.SANRENTAN].PayInfo.Kumiban) + "\t";
            msg += "[Ⅱ]" + ConvToRankStrToNumber(2, cachePayInfo[(int)PAY_BASE.SANRENTAN].PayInfo.Kumiban) + "\t";
            msg += "[Ⅲ]" + ConvToRankStrToNumber(3, cachePayInfo[(int)PAY_BASE.SANRENTAN].PayInfo.Kumiban) + "\t";
            msg += "\r\n";

            //払戻情報生成
            if(!cachePayInfo[(int)PAY_BASE.TANSHO].ExtFuseirituFlag)
            {
                msg += "【単勝】 " + Int32.Parse(cachePayInfo[0].PayInfo.Kumiban) + " (\\" + Int32.Parse(cachePayInfo[0].PayInfo.Pay) + ")" + "\r\n";
            }

            if (!cachePayInfo[(int)PAY_BASE.UMAREN].ExtFuseirituFlag)
            {
                msg += "【馬連】 " +
                    ConvToRankStrToNumber(1, cachePayInfo[(int)PAY_BASE.UMAREN].PayInfo.Kumiban) + " - " +
                    ConvToRankStrToNumber(2, cachePayInfo[(int)PAY_BASE.UMAREN].PayInfo.Kumiban) +
                    " (\\" + Int32.Parse(cachePayInfo[(int)PAY_BASE.UMAREN].PayInfo.Pay) + ")" + "\r\n";
            }

            if (!cachePayInfo[(int)PAY_BASE.SANRENPUKU].ExtFuseirituFlag)
            {
                msg += "【3連複】 " +
                    ConvToRankStrToNumber(1, cachePayInfo[(int)PAY_BASE.SANRENPUKU].PayInfo.Kumiban) + " - " +
                    ConvToRankStrToNumber(2, cachePayInfo[(int)PAY_BASE.SANRENPUKU].PayInfo.Kumiban) + " - " +
                    ConvToRankStrToNumber(3, cachePayInfo[(int)PAY_BASE.SANRENPUKU].PayInfo.Kumiban) +
                    " (\\" + Int32.Parse(cachePayInfo[(int)PAY_BASE.SANRENPUKU].PayInfo.Pay) + ")" + "\r\n";
            }



            tmpArray.Clear();   //再利用
            JvDbRaData RA = new JvDbRaData();
            if (db.TextReader_Col(DbWriteKey.Substring(0, 8), "RA", 0, ref tmpArray, DbWriteKey) != 0)
            {
                RA.setData(ref tmpArray);
                title = "【払戻通知：" + JvDbHRLibRaceCource(RA.getRaceCource()) + Int32.Parse(RA.getRaceNum()) + "R】";
                title += RA.getRaceName6().Trim();
                if(RA.getRaceGrade() == "一般" || RA.getRaceGrade() == "特別")
                {
                    //なにもしない
                }
                else if(RA.getRaceGrade() == "Ｊ・ＧⅠ" || RA.getRaceGrade() == "Ｊ・ＧⅡ" | RA.getRaceGrade() == "Ｊ・ＧⅢ")
                {
                    //なにもしない
                }
                else
                {
                    title += "(" + RA.getRaceGrade() + ")";
                }
            }

            if(title == "" || msg == "")
            {
                //どちらかが空だとExceptionをはくため、エラーチェック
                return;
            }
            else
            {
                //Windowsのポップアップ通知を出力する
                WindowsNotice.JvComNoticeShow(kind, title, msg);
            }
            
        }
        #endregion

        public void GetPayInfo(ref String inKey, ref String inCource, ref int inRacenNum)
        {
            inKey = this.DbWriteKey;
            inCource = this.Cource;
            inRacenNum = this.RaceNum;
        }

        #region ３連単払戻情報から着順を生成
        private int ConvToRankStrToNumber(int rank, String Str)
        {
            int ret = 0;

            if(Str.Length == 0)
            {
                return 0;
            }

            try
            {
                switch (rank)
                {
                    case 1:
                        return Int32.Parse(Str.Substring(0, 2));
                    case 2:
                        return Int32.Parse(Str.Substring(2, 2));
                    case 3:
                        return Int32.Parse(Str.Substring(4, 2));
                }
            }
            catch(Exception)
            {
                LOG.CONSOLE_MODULE("HR_CLASS", "ConvToRankStrToNumber Error!! Rank(" + rank + ") Length(" + Str.Length + ")");
            }
            
            return 0;
        }
        #endregion

        #region Jｖライブラリマッピング関数
        unsafe private String JvDbHRLibRaceCource(String JyoCd)
        {
            int Code = LibJvConv.LibJvConvFuncClass.COURCE_CODE;
            String ret = "";

            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&Code, JyoCd, ref ret);
            return ret;
        }
        #endregion

    }
}
