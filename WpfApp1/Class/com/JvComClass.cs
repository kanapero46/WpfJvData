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
                Console.WriteLine("[" + time + "]\t" + msg);
            }
            else
            {
                Console.WriteLine("[" + md + "] [" + time + "]\t" + msg);
            }          
        }

        //故障・エラー・ソフトエラー
        public void ASSERT(String md)
        {
            if(md != "")
            {
                Console.WriteLine("■■■■■■ ASSERT!!! ■■■■■■");
                Console.WriteLine("■ ASSERT MODULE [" + md + "] ■");
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


    }

}
