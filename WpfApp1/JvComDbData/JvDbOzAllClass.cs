using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    //オッズ保持クラス
    class JvDbOzAllClass
    {
        private String[,] TANPUKU;
        private String[,] FUKUSHO;
        private String[,] WIDE;
        private String[,] UMAREN;
        private String[,] UMATAN;
        private String[,] SANRENPUKU;
        private String[,] SANRENTAN;
        private String[,] WAKUREN;

        public struct JV_OZ_WIDE_ODDZ_STRUCT
        {
            public double WideMixOddz;
            public double WideMaxOddz;
        }

    WpfApp1.Class.com.JvComClass LOG = new Class.com.JvComClass();
        const String MD = "AO";

        String Key = "";

        public struct JV_OZ_HEADER
        {
            public bool tanshoFlg;
            public bool fukushoFlg;
            public bool wakurenFlg;
            public String outReleaseTime;   //オッズ発表時間(月日時分)
            public int fukushoPayOutRank;   //複勝○着払い
            public bool PayOutFlg;
            public int DataKubun;
        }

        JV_OZ_HEADER JvOzHeader;

        public JvDbOzAllClass()
        {
            TANPUKU = new string[18,5];
            FUKUSHO = new string[18,2];
            WIDE = new string[153,2];
            UMAREN = new string[153,3];
            UMATAN = new string[306 ,3];
            SANRENPUKU = new string[816,3];
            SANRENTAN = new string[4896,3];
            WAKUREN = new string[36,3];
        }

        public JvDbOzAllClass(String Key)
        {
            this.Key = Key;
            JvOzHeader = new JV_OZ_HEADER();
            InitJvOzHeader(ref JvOzHeader);
            TANPUKU = new string[18,5];
            FUKUSHO = new string[18,2];
            WIDE = new string[153, 2];
            UMAREN = new string[153, 3];
            UMATAN = new string[306, 3];
            SANRENPUKU = new string[816, 3];
            SANRENTAN = new string[4896, 3];
            WAKUREN = new string[36, 3];
        }

        public int JvDbAllSetALLOddzData()
        {
            LOG.CONSOLE_TIME_MD(MD, "<< JvDbAllSetALLOddzData start!!! >>");

            List<String> param = new List<string>();
            dbAccess.dbConnect db = new dbAccess.dbConnect();

            int cnt = 0;
            int cnt2 = 0;
            int cnt3 = 0;
            int cnt4 = 0;
            int umaren = 0;
            int ret = 0;

            CountClass Cnt = new CountClass();

            int loopCount = 0;

            int ret2 = db.DbReadAllData(Key, "OZ", 0, ref param, Key, 0);
            if(param.Count == 0)
            {
                return 0;
            }

            while (loopCount < param.Count)
            {
                //ここで1行ごとにカンマ区切りにする
                var values = param[loopCount].Split(',');
                switch(values[0].Substring(0,2))
                {
                    case "O0":
                        //馬券発売情報ヘッダー
                        JvOzHeader.tanshoFlg = values[3] == "7" ? true : false;
                        JvOzHeader.fukushoFlg = values[4] == "7" ? true : false;
                        JvOzHeader.wakurenFlg = values[5] == "7" ? true : false;
                        JvOzHeader.outReleaseTime = values[1];
                        JvOzHeader.fukushoPayOutRank = Int32.Parse(values[6]);
                        if(values[7] == "3" || values[7] == "4" || values[7] == "5")
                        {
                            JvOzHeader.PayOutFlg = true;
                        }
                        JvOzHeader.DataKubun = Int32.Parse(values[7]);
                        break;
                    case "O1":
                        //単勝
                        TANPUKU[Cnt.Tansho, 0] = values[1];
                        TANPUKU[Cnt.Tansho, 1] = values[2];
                        TANPUKU[Cnt.Tansho, 2] = values[5];
                        TANPUKU[Cnt.Tansho, 3] = values[4];
                        TANPUKU[Cnt.Tansho, 4] = values[6];
                        Cnt.Tansho++;
                        break;
                    case "O2":
                        //馬連
                        UMAREN[Cnt.Umaren, 0] = values[1];
                        UMAREN[Cnt.Umaren, 1] = values[2];
                        UMAREN[Cnt.Umaren, 2] = values[3];
                        Cnt.Umaren++;
                        break;
                    case "O3":
                        //ワイド
                        WIDE[Cnt.Wide, 0] = values[1];
                        WIDE[Cnt.Wide, 1] = values[2];
                        Cnt.Wide++;
                        break;
                    case "O4":
                        //馬単
                        UMATAN[Cnt.Umatan, 0] = values[1];
                        UMATAN[Cnt.Umatan, 1] = values[2];
                        UMATAN[Cnt.Umatan, 2] = values[3];
                        Cnt.Umatan++;
                        break;
                    case "O5":
                        //3連複
                        SANRENPUKU[Cnt.Sanrenpuku, 0] = values[1];
                        SANRENPUKU[Cnt.Sanrenpuku, 1] = values[2];
                        SANRENPUKU[Cnt.Sanrenpuku, 2] = values[3];
                        Cnt.Sanrenpuku++;
                        break;
                    case "O6":
                        //3連単
                        SANRENTAN[Cnt.Sanrentan, 0] = values[1];
                        SANRENTAN[Cnt.Sanrentan, 1] = values[2];
                        SANRENTAN[Cnt.Sanrentan, 2] = values[3];
                        Cnt.Sanrentan++;
                        break;
                    case "O7":
                        //枠連
                        WAKUREN[Cnt.Wakuren, 0] = values[1];
                        WAKUREN[Cnt.Wakuren, 1] = values[2];
                        WAKUREN[Cnt.Wakuren, 2] = values[3];
                        Cnt.Wakuren++;
                        break;
                }
                loopCount++;
            }

#if false
            //単勝
            for (int i=1; i<=TANSHO.Length; i++)
            {
                param.Clear();
                //if (db.TextReader_Row(Key + String.Format("{0:00}",i), "O1", 0, ref param) != 0)
                if(db.TextReader_Col(Key, "O1", 0, ref param, Key + String.Format("{0:00}", i)) > 1)
                {
                    TANSHO[i - 1] = param[2];
                    FUKUSHO[i - 1] = param[4] + " - " + param[5];
                }
                else
                {
                    break;
                }
            }
            
            //枠連
            for(int i=1; i<=8; i++)
            {
                for(int j=1; j <= 8; j++)
                {
                    if(i <= j)
                    {
                        param.Clear();
                        if (db.TextReader_Col(Key, "O15", 0, ref param, Key + i.ToString() + j.ToString()) > 1)
                        {
                            cnt++;
                            WAKUREN[cnt - 1,0] = param[1];
                            WAKUREN[cnt - 1,1] = param[2];
                        }
                    }           
                    else
                    {
                        break;
                    }
                }            
            }

            cnt = 0;

            //馬連・馬単・ワイド
            for(int i=1; i<=18; i++)
            {
                for(int j = 1; j<= 18; j++)
                {
                    if(i == j)
                    {
                        //同番はスキップ
                        continue;
                    }

                    if(i < j)
                    {
                        param.Clear();
                        ret = db.TextReader_Col(Key, "OZ", 0, ref param, "O2" + String.Format("{0:00}", i) + String.Format("{0:00}", j));
                        if (ret > 1)
                        {
                            //馬連
                            UMAREN[cnt, 0] = param[1];
                            UMAREN[cnt, 1] = param[2];

                            param.Clear();
                            if (db.TextReader_Col(Key, "OZ", 0, ref param, "O3" + String.Format("{0:00}", i) + String.Format("{0:00}", j)) > 1)
                            {
                                if(ret > 1)
                                {
                                    //ワイド
                                    WIDE[cnt, 0] = param[1];
                                    WIDE[cnt, 1] = param[2];
                                }
                            }
                            cnt++;
                        }
                    }

                    param.Clear();
                    if (db.TextReader_Col(Key, "OZ", 0, ref param, "O4" + String.Format("{0:00}", i) + String.Format("{0:00}", j)) > 1)
                    {
                        //馬単
                        UMATAN[cnt2, 0] = param[1];
                        UMATAN[cnt2, 1] = param[2];
                        cnt2++;
                    }

                    for (int k = 1; k < 18; k++)
                    {
                        //同番はスキップ
                        if (j == k || i == k)
                        {
                            continue;
                        }

                        //3連複
                        if (i < j && j < k)
                        {
                            param.Clear();
                            if (db.TextReader_Col(Key, "OZ", 0, ref param, "O5" + String.Format("{0:00}", i) + String.Format("{0:00}", j) + String.Format("{0:00}", k)) > 1)
                            {
                                //馬単
                                SANRENPUKU[cnt3, 0] = param[1];
                                SANRENPUKU[cnt3, 1] = param[2];
                                cnt3++;
                            }
                        }

                        param.Clear();
                        if (db.TextReader_Col(Key, "OZ", 0, ref param, "O6" + String.Format("{0:00}", i) + String.Format("{0:00}", j) + String.Format("{0:00}", k)) > 1)
                        {
                            //馬単
                            SANRENTAN[cnt4, 0] = param[1];
                            SANRENTAN[cnt4, 1] = param[2];
                            cnt4++;
                        }
                    }
                }
                
            }
#endif

            LOG.CONSOLE_TIME_MD(MD, "<< JvDbAllSetALLOddzData Finish!!! >>");
            return 1;
        }

        public int JvOzAllGetData(ref String[,] OutParam, int kind)
        {
            int ret = 0;
            switch(kind)
            {
                case 1:
                    //単勝
                    OutParam = TANPUKU;
                    break;
                case 2:
                    //複勝
                    OutParam = TANPUKU;
                    break;
                case 3:
                    //枠連
                    OutParam = WAKUREN;
                    break;
                case 4:
                    //馬連
                    OutParam = UMAREN;
                    break;
                case 5:
                    //馬単
                    OutParam = UMATAN;
                    break;
                case 6:
                    //ワイド
                    OutParam = WIDE;
                    break;
                case 7:
                    //3連複
                    OutParam = SANRENPUKU;
                    break;
                case 8:
                    //3連単
                    OutParam = SANRENTAN;
                    break;
                default:
                    LOG.CONSOLE_MODULE(MD, "Error!! Kind Number Is Exception" + kind);
                    break;

            }

            return ret;
        }

        private int JvOzConvGetData(int kind, String[] InParam, ref String[,] OutParam)
        {
            switch(kind)
            {
                case 1:
                case 2:
                    for(int i=0; i<18; i++)
                    {
                        if(InParam[i] == null)
                        {
                            return i+1;
                        }
                        OutParam[i, 0] = InParam[i];
                        OutParam[i, 1] = (i + 1).ToString();
                    }
                    break;
            }
            return 0;
        }

        public double JvOzAllConvOddzText(String InParam)
        {
            if(InParam == "")
            {
                return 0;
            }

            try
            {
                return Double.Parse(InParam.Substring(0, InParam.Length - 1) + "." + InParam.Substring(InParam.Length - 1, 1));
            }
            catch(Exception)
            {
                return 0;
            }

        }

        //ワイドオッズ→String型
        public String JvOzWideOddzText(String InParam)
        {
            if(InParam == "")
            {
                return "";
            }


            try
            {
                return
                    (

                    //最低オッズ
                    Double.Parse(InParam.Substring(0, 3)) + "." + Double.Parse(InParam.Substring(3, 1)) + " - " + 
                    Double.Parse(InParam.Substring(7, 3)) + "." + Double.Parse(InParam.Substring(10, 1))
                    );
            }
            catch
            {
                LOG.CONSOLE_MODULE(MD, "InParam " + InParam);
                return "***.* - ***.*";
            }
        }

        public int JvOzWideOddzData(String InParam, ref double OutParam1, ref double OutParam2)
        {
            if (InParam == "")
            {
                return 0;
            }

            try
            {
                OutParam1 = Double.Parse(Double.Parse(InParam.Substring(0, 3)) + "." + Double.Parse(InParam.Substring(3, 1)));
                OutParam2 = Double.Parse(Double.Parse(InParam.Substring(7, 3)) + "." + Double.Parse(InParam.Substring(10, 1)));
                return 1;
            }
            catch
            {
                LOG.CONSOLE_MODULE(MD, "InParam " + InParam);
                return 0;
            }
        }

        public double JvOzAllGetRestrictData(String Kumiban, int kind)
        {
            switch(kind)
            {
                case 1:
                    for(int i=1; i<=18; i++)
                    {
                        return JvOzAllConvOddzText(TANPUKU[i - 1, 1]);
                    }
                    break;
                case 2:
                    for (int i = 1; i <= 18; i++)
                    {
                        return JvOzAllConvOddzText(TANPUKU[i - 1, 2]);
                    }
                    break;
            }
            return 0;
        }

        private void InitJvOzHeader(ref JV_OZ_HEADER inParam)
        {
            inParam.tanshoFlg = false;
            inParam.fukushoFlg = false;
            inParam.wakurenFlg = false;
            inParam.outReleaseTime = "00000000";
            inParam.fukushoPayOutRank = 0;
            inParam.DataKubun = 0;
            inParam.PayOutFlg = false;
        }

        public void GetJvOzHeader(ref JV_OZ_HEADER outParam)
        {
            outParam = JvOzHeader;
        }
    }

    class CountClass
    {
        private int tansho;
        private int fukushi;
        private int wakuren;
        private int wide;
        private int umaren;
        private int umatan;
        private int sanrenpuku;
        private int sanrentan;

        public int Tansho { get => tansho; set => tansho = value; }
        public int Fukushi { get => fukushi; set => fukushi = value; }
        public int Wakuren { get => wakuren; set => wakuren = value; }
        public int Wide { get => wide; set => wide = value; }
        public int Umaren { get => umaren; set => umaren = value; }
        public int Umatan { get => umatan; set => umatan = value; }
        public int Sanrenpuku { get => sanrenpuku; set => sanrenpuku = value; }
        public int Sanrentan { get => sanrentan; set => sanrentan = value; }
    }
}
