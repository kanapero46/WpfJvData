using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.dbAccess;

namespace WpfApp1.Class.com
{
    class JvRcInfo
    {
        private Boolean EAST;
        private Boolean WEST;
        private Boolean LOCAL;

        private int EastRcNum;
        private int WestRcNum;
        private int LocalRcNum;

        public bool EAST1 { get => EAST; set => EAST = value; }
        public bool WEST1 { get => WEST; set => WEST = value; }
        public bool LOCAL1 { get => LOCAL; set => LOCAL = value; }
        public int EastRcNum1 { get => EastRcNum; set => EastRcNum = value; }
        public int WestRcNum1 { get => WestRcNum; set => WestRcNum = value; }
        public int LocalRcNum1 { get => LocalRcNum; set => LocalRcNum = value; }
    }

    class JvComRcInfo
    {
        public int JvComGetRcInfo(String Date, ref JvRcInfo Out)
        {

            dbAccess.dbConnect db = new dbConnect();

            if(Out == null)
            {
                return 0;
            }

            List<String> ArrayStr = new List<string>();
            List<int> RcDataInfo = new List<int>();
            Boolean SetFlag = false;

            if(db.TextReader_Row(Date, "RA", 2, ref ArrayStr) != 0)
            {
                if(ArrayStr.Count == 0)
                {
                    return 0;
                }

                for(int i=0; i<ArrayStr.Count; i++)
                {
                    SetFlag = false;
                    switch (ArrayStr[i])
                    {
                        case "05":
                        case "06":
                            Out.EAST1 = true;
                            break;
                        case "08":
                        case "09":
                            Out.WEST1 = true;
                            break;
                        case "01":
                        case "02":
                            Out.LOCAL1 = true;
                            break;
                    }

                    for(int j=0; j<RcDataInfo.Count; j++)
                    {
                        if(Int32.Parse(ArrayStr[i]) == RcDataInfo[j])
                        {
                            SetFlag = true;
                        }
                    }

                    if(!SetFlag)
                    {
                        RcDataInfo.Add(Int32.Parse(ArrayStr[i]));
                    }

                }

                for(int i=0; i<RcDataInfo.Count; i++)
                {
                    switch (RcDataInfo[i])
                    {
                        case 5:
                        case 6:
                            Out.EastRcNum1 = RcDataInfo[i];
                            break;
                        case 8:
                        case 9:
                            Out.WestRcNum1 = RcDataInfo[i];
                            break;
                        case 7:
                        case 10:
                            if (Out.WEST1)
                            { Out.LOCAL1 = true;  Out.LocalRcNum1 = RcDataInfo[i]; }
                            else
                            {
                                Out.WEST1 = true;
                                Out.WestRcNum1 = RcDataInfo[i];
                            }
                            break;
                        case 3:
                        case 4:
                            if (Out.EAST1)
                            { Out.LOCAL1 = true; Out.LocalRcNum1 = RcDataInfo[i]; }
                            else
                            {
                                Out.EAST1 = true;
                                Out.EastRcNum1 = RcDataInfo[i];
                            }
                            break;
                        case 1:
                        case 2:
                            Out.LOCAL1 = true;
                            Out.LocalRcNum1 = RcDataInfo[i];
                            break;
                    }
                }
                return 1;
            }

            return 0;
        }
    }
}
