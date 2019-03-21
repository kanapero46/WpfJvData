using System;
using System.Collections.Generic;
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
            Console.WriteLine("(" + Md + ")\t" + msg);
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
                Console.WriteLine("(" + md + ") [" + time + "]\t" + msg);
            }          
        }
    }
}
