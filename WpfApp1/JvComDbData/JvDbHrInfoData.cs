using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    //払戻金情報
    class JvDbHRInfoData
    {
        struct TAN_PARAM
        {
            public String Umaban;
            public String Pay;
            public String Ninki;
        }

        public struct MULTI_PARAM
        {
            public String Kumiban;
            public String Pay;
            public String Ninki;
        }

        public struct EXT_PARAM
        {
            public Boolean ExtFuseirituFlag;
            public Boolean ExtSpPayFlag;
            public Boolean ExtHenkanFlag;
            public MULTI_PARAM PayInfo;
        }

        public enum PAY_BASE
        {
            TANSHO,
            FUKUSHO,
            WAKUREN,
            UMAREN,
            WIDE,
            UMATAN,
            SANRENPUKU,
            SANRENTAN,
            MAX
        }

        Boolean[] PayFlag;
        Boolean[] FuseirituFlag;
        Boolean[] SpPayFlag;
        Boolean[] HenaknFlag;
        List<TAN_PARAM> TANSHO = new List<TAN_PARAM>();
        List<TAN_PARAM> FUKUSHO = new List<TAN_PARAM>();
        List<MULTI_PARAM> WAKUREN = new List<MULTI_PARAM>();
        List<MULTI_PARAM> UMAREN = new List<MULTI_PARAM>();
        List<MULTI_PARAM> WIDE = new List<MULTI_PARAM>();
        List<MULTI_PARAM> UMATAN = new List<MULTI_PARAM>();
        List<MULTI_PARAM> SANRENPUKU = new List<MULTI_PARAM>();
        List<MULTI_PARAM> SANRENTAN = new List<MULTI_PARAM>();

        public bool[] PayFlag1 { get => PayFlag; set => PayFlag = value; }
        public bool[] FuseirituFlag1 { get => FuseirituFlag; set => FuseirituFlag = value; }
        public bool[] HenaknFlag1 { get => HenaknFlag; set => HenaknFlag = value; }
        private List<TAN_PARAM> TANSHO1 { get => TANSHO; set => TANSHO = value; }
        private List<TAN_PARAM> FUKUSHO1 { get => FUKUSHO; set => FUKUSHO = value; }
        private List<MULTI_PARAM> WAKUREN1 { get => WAKUREN; set => WAKUREN = value; }
        private List<MULTI_PARAM> UMAREN1 { get => UMAREN; set => UMAREN = value; }
        private List<MULTI_PARAM> WIDE1 { get => WIDE; set => WIDE = value; }
        private List<MULTI_PARAM> UMATAN1 { get => UMATAN; set => UMATAN = value; }
        private List<MULTI_PARAM> SANRENPUKU1 { get => SANRENPUKU; set => SANRENPUKU = value; }
        private List<MULTI_PARAM> SANRENTAN1 { get => SANRENTAN; set => SANRENTAN = value; }

        readonly public int MAX_HR_DATA = 40;

        public JvDbHRInfoData()
        {
            PayFlag1 = new bool[8];
            FuseirituFlag1 = new bool[8];
            SpPayFlag = new bool[8];
            HenaknFlag1 = new bool[8];
            
        }

        public void InitJvDbHrInfoData()
        {
            PayFlag1 = new bool[8];
            FuseirituFlag1 = new bool[8];
            SpPayFlag = new bool[8];
            HenaknFlag1 = new bool[8];
            TANSHO.Clear();
            FUKUSHO.Clear();
            UMAREN.Clear();
            WAKUREN.Clear();
            WIDE.Clear();
            UMATAN.Clear();
            SANRENPUKU.Clear();
            SANRENTAN.Clear();

        }

        public void SetPayInfo(ref List<String> In)
        {
            String[] tmpArray = new string[MAX_HR_DATA];
            InitCsvTopString(ref tmpArray);
            TAN_PARAM Param = new TAN_PARAM();

            //エラーチェック
            if(In.Count == 0)
            {
                return;
            }

            for (int i = 0; i < tmpArray.Length; i++)
            {
               
                if(In[0] == tmpArray[i])
                {
                    switch(i)
                    {
                        case 0:
                            //単勝
                            FuseirituFlag[0] = (In[5] == "1" ? true : false);
                            SpPayFlag[0] = (In[6] == "1" ? true : false);
                            HenaknFlag[0] = (In[7] == "1" ? true : false);
                            Set_TANSHO(ref In);
                            break;
                        case 1:
                        case 2:
                            //単勝
                            if(In[2] == "")
                            {
                                break;  //同着なし
                            }
                            else
                            {
                                Set_TANSHO(ref In);
                            }                           
                            break;
                        case 3:
                            //複勝
                            FuseirituFlag[1] = (In[5] == "1" ? true : false);
                            SpPayFlag[1] = (In[6] == "1" ? true : false);
                            HenaknFlag[1] = (In[7] == "1" ? true : false);
                            Set_FUKUSHO(ref In);
                            break;
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                            if (In[2] == "")
                            {
                                break;  //的中票なし
                            }
                            else
                            {
                                Set_FUKUSHO(ref In);
                            }
                            break;
                        case 8:
                        case 9:
                        case 10:
                            //枠連
                            break;
                        case 11:
                            //馬連
                            FuseirituFlag[3] = (In[5] == "1" ? true : false);
                            SpPayFlag[3] = (In[6] == "1" ? true : false);
                            HenaknFlag[3] = (In[7] == "1" ? true : false);
                            SetMultiPayInfo(3, ref In);
                            break;
                        case 12:
                        case 13:
                            if (In[2] == "")
                            {
                                break;  //的中票なし
                            }
                            else
                            {
                                SetMultiPayInfo(3, ref In);
                            }
                            break;
                        case 14:
                            //ワイド
                            break;
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            break;
                        case 21:
                            //馬単
                            break;
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                            break;
                        case 27:
                            //3連複
                            FuseirituFlag[6] = (In[5] == "1" ? true : false);
                            SpPayFlag[6] = (In[6] == "1" ? true : false);
                            HenaknFlag[6] = (In[7] == "1" ? true : false);
                            SetMultiPayInfo(6, ref In);
                            break;
                        case 28:
                        case 29:
                            if (In[2] == "")
                            {
                                break;  //的中票なし
                            }
                            else
                            {
                                SetMultiPayInfo(6, ref In);
                            }
                            break;
                        case 30:
                            //3連単
                            FuseirituFlag[7] = (In[5] == "1" ? true : false);
                            SpPayFlag[7] = (In[6] == "1" ? true : false);
                            HenaknFlag[7] = (In[7] == "1" ? true : false);
                            SetMultiPayInfo(7, ref In);
                            break;
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                            if (In[2] == "")
                            {
                                break;  //的中票なし
                            }
                            else
                            {
                                SetMultiPayInfo(7, ref In);
                            }
                            break;
                    }
                }
            }
        }

        private void Set_TANSHO(ref List<String> In)
        {
            TAN_PARAM Param = new TAN_PARAM();
            if(In.Count == 0)
            {
                return; //エラーチェック
            }

            Param.Umaban = In[2];
            Param.Pay = In[3];
            Param.Ninki = In[4];
            TANSHO.Add(Param);
        }

        private void Set_FUKUSHO(ref List<String> In)
        {
            TAN_PARAM Param = new TAN_PARAM();
            if (In.Count == 0)
            {
                return; //エラーチェック
            }

            Param.Umaban = In[2];
            Param.Pay = In[3];
            Param.Ninki = In[4];
            FUKUSHO.Add(Param);
        }

        private void SetMultiPayInfo(int kind, ref List<String> In)
        {
            MULTI_PARAM Param = new MULTI_PARAM();
            if (In.Count == 0)
            {
                return; //エラーチェック
            }

            Param.Kumiban = In[2];
            Param.Pay = In[3];
            Param.Ninki = In[4];
            
            switch (kind)
            {
                case 2:
                    //枠連
                    WAKUREN.Add(Param);
                    break;
                case 3:
                    //馬連
                    UMAREN.Add(Param);
                    break;
                case 4:
                    //ワイド
                    WIDE.Add(Param);
                    break;
                case 5:
                    //馬単
                    UMATAN.Add(Param);
                    break;
                case 6:
                    //３連複
                    SANRENPUKU.Add(Param);
                    break;
                case 7:
                    //３連単
                    SANRENTAN.Add(Param);
                    break;
                    
            }
        }

        #region 払戻金情報の取得（複勝・ワイドなどは複数回呼ぶ必要あり）
        /** 
         * @param Basenum：取得する払戻金情報（定義はこのファイルにある）
         * @Param Kind：取得する払戻情報(基本は0、ワイド・複勝など複数個の払戻がある場合は1,2・・・をコールする)
         * @Param Out：払戻情報セットクラス
         */
        protected void GetPayInfo(PAY_BASE BaseNum, int kind, ref EXT_PARAM Out)
        {
            MULTI_PARAM Param = new MULTI_PARAM();
            switch(BaseNum)
            {
                case PAY_BASE.TANSHO:
                    Out.ExtFuseirituFlag = FuseirituFlag[0];
                    Out.ExtSpPayFlag = SpPayFlag[0];
                    Out.ExtHenkanFlag = HenaknFlag[0];
                    //払戻情報
                    Param.Kumiban = TANSHO[kind].Umaban;
                    Param.Ninki = TANSHO[kind].Ninki;
                    Param.Pay = TANSHO[kind].Pay;
                    break;
                case PAY_BASE.FUKUSHO:
                    //複勝
                    Out.ExtFuseirituFlag = FuseirituFlag[1];
                    Out.ExtSpPayFlag = SpPayFlag[1];
                    Out.ExtHenkanFlag = HenaknFlag[1];
                    //払戻情報
                    Param.Kumiban = FUKUSHO[kind].Umaban;
                    Param.Ninki = FUKUSHO[kind].Ninki;
                    Param.Pay = FUKUSHO[kind].Pay;
                    break;
                case PAY_BASE.WAKUREN:
                    //枠連
                    Out.ExtFuseirituFlag = FuseirituFlag[2];
                    Out.ExtSpPayFlag = SpPayFlag[2];
                    Out.ExtHenkanFlag = HenaknFlag[2];
                    //払戻情報
                    Param.Kumiban = WAKUREN[kind].Kumiban;
                    Param.Ninki = WAKUREN[kind].Ninki;
                    Param.Pay = WAKUREN[kind].Pay;
                    break;
                case PAY_BASE.UMAREN:
                    //馬連
                    Out.ExtFuseirituFlag = FuseirituFlag[3];
                    Out.ExtSpPayFlag = SpPayFlag[3];
                    Out.ExtHenkanFlag = HenaknFlag[3];
                    //払戻情報
                    Param.Kumiban = UMAREN[kind].Kumiban;
                    Param.Ninki = UMAREN[kind].Ninki;
                    Param.Pay = UMAREN[kind].Pay;
                    break;
                case PAY_BASE.WIDE:
                    //ワイド
                    Out.ExtFuseirituFlag = FuseirituFlag[4];
                    Out.ExtSpPayFlag = SpPayFlag[4];
                    Out.ExtHenkanFlag = HenaknFlag[4];
                    //払戻情報
                    Param.Kumiban = WIDE[kind].Kumiban;
                    Param.Ninki = WIDE[kind].Ninki;
                    Param.Pay = WIDE[kind].Pay;
                    break;
                case PAY_BASE.UMATAN:
                    //馬単
                    Out.ExtFuseirituFlag = FuseirituFlag[5];
                    Out.ExtSpPayFlag = SpPayFlag[5];
                    Out.ExtHenkanFlag = HenaknFlag[5];
                    //払戻情報
                    Param.Kumiban = UMATAN[kind].Kumiban;
                    Param.Ninki = UMATAN[kind].Ninki;
                    Param.Pay = UMATAN[kind].Pay;
                    break;
                case PAY_BASE.SANRENPUKU:
                    //3連複
                    Out.ExtFuseirituFlag = FuseirituFlag[6];
                    Out.ExtSpPayFlag = SpPayFlag[6];
                    Out.ExtHenkanFlag = HenaknFlag[6];
                    //払戻情報
                    Param.Kumiban = SANRENPUKU[kind].Kumiban;
                    Param.Ninki = SANRENPUKU[kind].Ninki;
                    Param.Pay = SANRENPUKU[kind].Pay;
                    break;
                case PAY_BASE.SANRENTAN:
                    //3連単
                    Out.ExtFuseirituFlag = FuseirituFlag[7];
                    Out.ExtSpPayFlag = SpPayFlag[7];
                    Out.ExtHenkanFlag = HenaknFlag[7];
                    //払戻情報
                    Param.Kumiban = SANRENTAN[kind].Kumiban;
                    Param.Ninki = SANRENTAN[kind].Ninki;
                    Param.Pay = SANRENTAN[kind].Pay;
                    break;
            }

            Out.PayInfo = Param;     
        }
        #endregion


        //InitCsvTopStringを追加した場合はMAX_HR_DATAの数を増やすこと
        protected void InitCsvTopString(ref String[] In)
        {
            In[0] = "P10";
            In[1] = "P11";
            In[2] = "P12";
            In[3] = "P20";
            In[4] = "P21";
            In[5] = "P22";
            In[6] = "P23";
            In[7] = "P24";
            In[8] = "P30";
            In[9] = "P31";
            In[10] = "P32";
            In[11] = "P40";
            In[12] = "P41";
            In[13] = "P42";
            In[14] = "P50";
            In[15] = "P51";
            In[16] = "P52";
            In[17] = "P53";
            In[18] = "P54";
            In[19] = "P55";
            In[20] = "P56";
            In[21] = "P60";
            In[22] = "P61";
            In[23] = "P62";
            In[24] = "P63";
            In[25] = "P64";
            In[26] = "P65";
            In[27] = "P70";
            In[28] = "P71";
            In[29] = "P72";
            In[30] = "P80";
            In[31] = "P81";
            In[32] = "P82";
            In[33] = "P83";
            In[34] = "P84";
            In[35] = "P85";
            In[36] = "P100"; //返還馬番
            In[37] = "P101"; //返還枠番
            In[38] = "P102"; //返還同枠
        }


    }

}
