using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Class.com
{
    class JvComClass
    {
        public void CONSOLE_WRITER(String msg)
        {
            Console.WriteLine(msg);
        }

        public void CONSOLW_DEBUG(String msg)
        {
            Console.WriteLine("[DEBUG]" + msg);
        }

        public void CONSOLE_MODULE(String Md, String msg)
        {
            Console.WriteLine("[" + Md + "]\t" + msg);
        }

        public void CONSOLE_TIME(String msg)
        {
            CONSOLE_TIME_MD("", msg);
        }

        public void CONSOLE_TIME_MD(String md, String msg)
        {
            DateTime time = DateTime.Now;
            if(md == "")
            {
                Console.WriteLine("[" + time.ToString("yyyy/MM/dd HH:mm:ss.ffffff") + "]\t" + msg);
            }
            else
            {
                Console.WriteLine("[" + md + "] [" + time.ToString("yyyy/MM/dd HH:mm:ss.ffffff") + "]\t" + msg);
            }          
        }

        //時間を出力
        public void CONSOLE_TIME_OUT()
        {
            DateTime time = DateTime.Now;
            Console.WriteLine("[" + time + "]\t");
        }

        //故障・エラー・ソフトエラー
        public void ASSERT(String md)
        {
            DateTime time = DateTime.Now;

            if (md != "")
            {
                Console.WriteLine("■■■■■■ ASSERT!!! ■■■■■■");
                Console.WriteLine("■■■■■■ ASSERT TIME [" + time + "]");
                Console.WriteLine("■■■■■■ ASSERT MODULE [" + md + "]");
            }
            else
            {
                Console.WriteLine("■■■■■■ ASSERT!!! ■■■■■■");
                Console.WriteLine("■■■■■■ ASSERT TIME [" + time + "]");
            }
        }

        public unsafe void JvSysMappingFunction(int kind, ref String In, ref String Out)
        {
            int lib = kind;
            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&lib, In, ref Out);
        }

        public unsafe String JvSysMappingFunction(int kind, ref String In)
        {
            int lib = kind;
            String Out = "";
            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&lib, In, ref Out);
            return Out;
        }

        public Color JvSysJomeiNumtoColor(int color)
        {
            switch(color)
            {
                case 1:
                    return Color.Blue;
                case 2:
                    return Color.Green;
                case 3:
                    return Color.Purple;
                default:
                    return Color.White;
            }
        }

        //MAX関数
        public int MAX(params int[] nums)
        {
            // 引数が渡されない場合
            if (nums.Length == 0) return 0;

            int max = nums[0];
            for (int i = 1; i < nums.Length; i++)
            {
                max = max > nums[i] ? max : nums[i];
                // Minの場合は不等号を逆にすればOK
            }
            return max;
        }

        //String型でラッピング
        public Color NumberStrToColor(String WakubanStr) { return NumberToColor(Int32.Parse(WakubanStr)); }
        public Color NumberStrToForeColor(String WakubanStr) { return NumberToForeColor(Int32.Parse(WakubanStr)); }

        //枠番から枠色を返す
        public System.Drawing.Color NumberToColor(int Wakuban)
        {
            switch (Wakuban)
            {
                case 1:
                    return Color.White;
                case 2:
                    return Color.Black;
                case 3:
                    return Color.Red;
                case 4:
                    return Color.Blue;
                case 5:
                    return Color.Yellow;
                case 6:
                    return Color.Green;
                case 7:
                    return Color.Orange;
                case 8:
                    return Color.Pink;
            }
            return Color.White;
        }


        //枠番から枠字を返す
        public System.Drawing.Color NumberToForeColor(String Wakuban)
        {
            return NumberToForeColor(Int32.Parse(Wakuban));
        }

        public System.Drawing.Color NumberToForeColor(int Wakuban)
        {
            switch (Wakuban)
            {
                case 1:
                case 5:
                case 7:
                case 8:
                    return Color.Black;
                case 2:
                case 3:
                case 4:
                case 6:
                    return Color.White;
            }
            return Color.Black;
        }

        //JRA-VANDataLabのオッズから、String型を形成
        public String OddzStrToString(String O1)
        {
            //3連単は最大7桁の仕様に合わせた
            int tmp = 0;
            String outParam = "";
            try
            {
                outParam = (O1.Length <= 7 ? "0" : "") + (O1.Length <= 6 ? "0" : "") + (O1.Length <= 5 ? "0" : "") + (O1.Length <= 4 ? "0" : "") + (O1.Length <= 3 ? "0" : "") + (O1.Length <= 2 ? "0" : "") + (O1.Length <= 1 ? "0" : "") + O1;

                if (Int32.TryParse(outParam, out tmp))
                {
                    //int型に変換出来ることが条件
                    return Int32.Parse(outParam.Substring(0, 7)).ToString() + "." + outParam.Substring(7, 1);
                }
            }catch(Exception)
            {

            }

            return "***.*";
        }
        
        //intの２次元配列をソートする共通関数
        //indexは１次元(0)・２次元(1)の次元番号
        public void ArraySort(int[,] inParam, ref int[,] outParam, int Index)
        {
            
            
            try
            {
                int ArrayIndex = inParam.GetLength(Index);
                int[] tmpArray = new int[ArrayIndex];    //とりあえず１つ
                for (int i = 0; i < ArrayIndex; i++)
                {
                    if(Index == 0)
                    {
                        //1次元をソート
                        tmpArray[i] = inParam[i, 0];
                    }
                    else
                    {
                        //２次元をソート
                        tmpArray[i] = inParam[0, i];
                    }
                }
                   
            }
            catch(Exception)
            {
               // return 0;
            }

            //tmpArray = new int[ArrayIndex];

        }
    }

}
