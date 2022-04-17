using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibQrDataAnalyze.data
{
    class ExactaClass
    {
        private int key;
        private JvQrAnaBetKindClass jvQrAnaBet;
        private List<int> betKind;     /* 馬券種類 */
        private List<JvQrAnaDetailsClass> kumiban;   /* 組番 */
        private List<int> price;     /* 金額 */

        public int Key { get => key; set => key = value; }
        public JvQrAnaBetKindClass JvQrAnaBet { get => jvQrAnaBet; set => jvQrAnaBet = value; }
        public List<int> BetKind { get => betKind; set => betKind = value; }
        public List<JvQrAnaDetailsClass> Kumiban { get => kumiban; set => kumiban = value; }
        public List<int> Price { get => price; set => price = value; }

        public void SetKumiban(int Param1, int Param2, int Param3)
        {
            JvQrAnaDetailsClass tmp = new JvQrAnaDetailsClass();
            tmp.Kumiban1 = Param1;
            tmp.Kumiban2 = Param2;
            tmp.Kumiban3 = Param3;
            kumiban.Add(tmp);
        }
    }
}
