using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.Class;

namespace WpfApp1.form.info
{
    /* InfomationFormのバックエンドクラス */
    public class BackEndInfomationForm
    {
        //イベント発表メソッド指定定数
        public const int JV_RT_EVENT_PAY = 1;
        public const int JV_RT_EVENT_JOCKEY_CHANGE = 2;
        public const int JV_RT_EVENT_WEATHER = 3;
        public const int JV_RT_EVENT_COURCE_CHANGE = 4;
        public const int JV_RT_EVENT_AVOID = 5;
        public const int JV_RT_EVENT_TIME_CHANGE = 6;
        public const int JV_RT_EVENT_WEIGHT = 7;

        public int JvInfoBackMain(int kind, String Key)
        {
            int ret = 0;
            switch(kind)
            {
                case JV_RT_EVENT_PAY:
                    ret = JvInfoBackEventPay(Key);
                    break;
            }


            return ret;
        }


        private int JvInfoBackEventPay(String Key)
        {
            JVData_Struct.JV_HR_PAY Pay = new JVData_Struct.JV_HR_PAY();
            JVForm JvForm = new JVForm();
            int ret = 0;

            JvForm.JvForm_JvInit();

            ret = JvForm.JvForm_JvRTOpen("0B15", Key);

            if(ret != 0)
            {
                Console.WriteLine("JVRTOPEN ERROR! JvInfoBackEventPay(" + ret + ")");
                return ret;
            }

            ret = 1;
            String buff = "";
            int size = 20000;
            String filename = "";

            JvComDbData.JvDbHRData HR = new JvComDbData.JvDbHRData();


            while(ret >= 1)
            {
                ret = JvForm.JvForm_JvRead(ref buff, out size, out filename);
                if(ret >= 1)
                {
                    switch(buff.Substring(0,2))
                    {
                        case "HR":
                            HR.SetHrData(ref buff);
                            break;
                    }
                }
                else if(ret == 0 || buff == "")
                {
                    //全ファイル読み込み終了
                    break;
                }
                else if(ret == -1)
                {
                    ret = 1;
                    continue;
                }
                else
                {

                }

            }
            JvForm.JvForm_JvClose();
            return 1;
        }
    }
}
