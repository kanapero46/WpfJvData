using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WpfApp1.Class
{
    public class InfomationFormSettingClass
    {
        public struct ShowStatus
        {
            public Boolean East;
            public Boolean West;
            public Boolean Local;
        };

        public Boolean AutoGetFlag { get; set; }

        public String RaKey { get; set; }

        public ShowStatus[] KaisaiStatus { get; set; }
    }
}