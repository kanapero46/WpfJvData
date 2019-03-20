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
    }
}
