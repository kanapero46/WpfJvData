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
        const int JV_RT_EVENT_PAY = 1;
        const int JV_RT_EVENT_JOCKEY_CHANGE = 2;
        const int JV_RT_EVENT_WEATHER = 3;
        const int JV_RT_EVENT_COURCE_CHANGE = 4;
        const int JV_RT_EVENT_AVOID = 5;
        const int JV_RT_EVENT_TIME_CHANGE = 6;
        const int JV_RT_EVENT_WEIGHT = 7;

        public int JvInfoBackMain(int kind, String key, ref List<String> array)
        {



            return 0;
        }
    }
}
