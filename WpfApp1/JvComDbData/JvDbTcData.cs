using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    class JvDbTcData
    {
        int Count = 0;
        private String Date;
        private List<String> Key;
        private List<String> HappyoTime;
        private List<String> OldTime;
        private List<String> AfterTime;

        public List<string> Key1 { get => Key; set => Key = value; }
        public List<string> HappyoTime1 { get => HappyoTime; set => HappyoTime = value; }
        public List<string> OldTime1 { get => OldTime; set => OldTime = value; }
        public List<string> AfterTime1 { get => AfterTime; set => AfterTime = value; }
        public int Count1 { get => Count; set => Count = value; }

        public JvDbTcData()
        {
            InitJvTcData();
        }


        public void InitJvTcData()
        {
            Count = 0;
            Date = "";
            Key = new List<string>();
            HappyoTime = new List<string>();
            OldTime = new List<string>();
            AfterTime = new List<string>();
        }

        public void SetJvTcData(ref String buff)
        {
            JVData_Struct.JV_TC_INFO Tc = new JVData_Struct.JV_TC_INFO();
            Tc.SetDataB(ref buff);

            int i = Key.Count;
            Count1++;

            String tmpStr = Tc.id.Year + Tc.id.MonthDay + Tc.id.JyoCD + Tc.id.Kaiji + Tc.id.Nichiji + Tc.id.RaceNum;
            Key.Add(tmpStr);

            tmpStr = Tc.HappyoTime.Month + Tc.HappyoTime.Day + Tc.HappyoTime.Hour + Tc.HappyoTime.Minute;
            HappyoTime.Add(tmpStr);

            tmpStr = Tc.TCInfoBefore.Ji + Tc.TCInfoBefore.Fun;
            OldTime.Add(tmpStr);

            tmpStr = Tc.TCInfoAfter.Ji + Tc.TCInfoAfter.Fun;
            AfterTime.Add(tmpStr);
            
        }

        /** 発走時間変更をDB[に書き込み　[返り値] 1：書き込み完了　０：書き込み変更なし　－１：エラー */
        public int WriteJvDbData()
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> libArray = new List<string>();
            int ret = 0;
            if(Count == 0)
            {
                //データなし
                return -1;
            }

            if(db.TextReader_Col(Date, "TC", 0, ref libArray, "TC") != 0)
            {

                if(libArray[1] == HappyoTime[Count - 1])
                {
                    //前回の発表時間とゲットした最新の時間が一致したら書き込まない
                    ret = 0;
                }
                else
                {
                    String WriteStr = "";
                    //最終発表時刻をキーとして拾えるようにスペックと発表時刻を１行目に書き込み
                    WriteStr += "TC," + HappyoTime[Count - 1];
                    for (int i = 0; i < Count; i++)
                    {
                        WriteStr += (i+1) + ",";
                        WriteStr += Key[i] + ",";
                        WriteStr += HappyoTime[i] + ",";
                        WriteStr += OldTime[i] + ",";
                        WriteStr += AfterTime[i] + ",";
                        WriteStr += "\r\n";
                    }

                    db = new dbAccess.dbConnect(Date, "TC", ref WriteStr, ref ret);
                }
            }
            else
            {
                ret = -1;
            }
            
            return ret;
        }

        
    }
}
