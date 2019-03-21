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
            public Boolean SpPayFlag; //特払い
            public String Umaban;
            public String Pay;
            public String Ninki;
        }

        struct MULTI_PARAM
        {
            Boolean SpPayFlag; //特払い
            String Kumiban;
            String Pay;
        }

        Boolean[] PayFlag;
        Boolean[] FuseirituFlag;
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

        public JvDbHRInfoData()
        {
            PayFlag1 = new bool[8];
            FuseirituFlag1 = new bool[8];
            HenaknFlag1 = new bool[8];
        }

        public void SetPayInfo(ref List<String> In)
        {
            String[] tmpArray = new string[37];
            InitCsvTopString(ref tmpArray);
            for (int i = 0; i < tmpArray.Length; i++)
            {
                if(In[0] == tmpArray[i])
                {
                    switch(i)
                    {
                        case 0:
                            //単勝
                            
                            break;
                        case 1:
                        case 2:
                            //単勝

                            break;
                    }
                }
            }
        }

        private void Set_TANSHO(ref List<String> In)
        {
            TAN_PARAM Param = new TAN_PARAM();
            Param.Umaban = In[5];
            Param.Pay = In[6];
            Param.Ninki = In[7];
        }
        
        protected void InitCsvTopString(ref String[] In)
        {
            In[0] = "単勝0";
            In[1] = "単勝1";
            In[2] = "単勝2";
            In[3] = "複勝0";
            In[4] = "複勝1";
            In[5] = "複勝2";
            In[6] = "複勝3";
            In[7] = "複勝4";
            In[8] = "枠連0";
            In[9] = "枠連1";
            In[10] = "枠連2";
            In[11] = "馬連0";
            In[12] = "馬連1";
            In[13] = "馬連2";
            In[14] = "ワイド0";
            In[15] = "ワイド1";
            In[16] = "ワイド2";
            In[17] = "ワイド3";
            In[18] = "ワイド4";
            In[19] = "ワイド5";
            In[20] = "ワイド6";
            In[21] = "馬単0";
            In[22] = "馬単1";
            In[23] = "馬単2";
            In[24] = "馬単3";
            In[25] = "馬単4";
            In[26] = "馬単5";
            In[27] = "3連複0";
            In[28] = "3連複1";
            In[29] = "3連複2";
            In[30] = "3連単0";
            In[31] = "3連単1";
            In[32] = "3連単2";
            In[33] = "3連単3";
            In[34] = "3連単4";
            In[35] = "3連単5";
        }
    }

}
