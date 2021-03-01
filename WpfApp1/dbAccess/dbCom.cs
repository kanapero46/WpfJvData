/* dbConnectMapping DBを読み込んで処理する共通処理 */
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WpfApp1.dbAccess;
using WpfApp1.Class;
using System.Drawing;
using WpfApp1.JvComDbData;

namespace WpfApp1.dbCom1
{
    public class dbCom
    {

        dbConnect db = new dbConnect();

        //すべての血統情報を取得する */
        List<String> St = new List<string>();
        List<String> Hn = new List<string>();

        public dbCom()
        {
            db.DbReadAllData("0", "ST", 0, ref St, "0", 0);
            db.DbReadAllData("0", "HN", 0, ref Hn, "0", 0);
        }

        #region 血統タイプをDBから読み込み(1)
        public String DbComSearchBloodType(String name1)
        {
            String tmp = "";
            if (name1 == "")
            {
                return "";
            }

            if (DbComBloodType(name1, ref tmp))
            {
                return tmp;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region 血統タイプをDBから読み込み(2)
        public String DbComSearchBloodType(String name1, String name2)
        {
            String tmp = "";
            if (name1 == "" || name2 == "")
            {
                return "";
            }

            if (DbComBloodType(name1, ref tmp))
            {
                return tmp;
            }

            if (DbComBloodType(name2, ref tmp))
            {
                return tmp;
            }
            return "";
        }
        #endregion

        #region 血統タイプをDBから読み込み(3)
        public String DbComSearchBloodType(String name1, String name2, String name3)
        {
            String tmp = "";
            if (name1 == "" || name2 == "" || name3 == "")
            {
                return "";
            }

            if (DbComBloodType(name1, ref tmp))
            {
                return tmp;
            }

            if (DbComBloodType(name2, ref tmp))
            {
                return tmp;
            }

            if (DbComBloodType(name3, ref tmp))
            {
                return tmp;
            }

            return "";
        }
        #endregion

        #region 血統タイプをDBから読み込み(共通・馬名)
        private Boolean DbComBloodType(String name, ref String outParam)
        {
            String fBloodName;
            String tmp = "";
            int cnt = 0;

            /* １．設定データ(ST)からデータを取得する */
            if (db.TextReader_aCell("ST", name, "0", 2, ref tmp) == 1)
            {
                outParam = tmp;
                return true;
            }

            fBloodName = name;
            while (cnt < 7) /* 仕様変更#17 */
            {
                tmp = "";
                db.TextReader_aCell("HN", fBloodName, "0", 4, ref tmp);
                if (tmp == "")
                {
                    /* 父馬がヒットしなかったら検索続行しない */
                    outParam = "";
                    break;
                }
                else
                {
                    /* ヒットしたら父馬の血統登録番号で検索続行 */
                    fBloodName = tmp;
                    if (db.TextReader_aCell("ST", fBloodName, "0", 2, ref tmp) == 1)
                    {
                        /* 検索にヒットしたら処理中断 */
                        outParam = tmp;
                        return true;
                    }
                    else
                    {
                        /* 検索にヒットしない場合、カウンタアップして処理続行 */
                        cnt++;
                    }
                }
            }
            return false;
        }
        #endregion

        #region 血統タイプをDBから読み込み(1・色)
        public Color DbComSearchBloodColor(String name1)
        {
            Boolean ret = false;

            if (name1 == "")
            {
                return Color.White;
            }

            Color Clr = DbComBloodColor(name1, ref ret);
            return Clr;
        }
        #endregion

        #region 血統タイプをDBから読み込み(2)
        public Color DbComSearchBloodColor(String name1, String name2)
        {
            Boolean ret = false;
            if (name1 == "" || name2 == "")
            {
                return Color.White;
            }
            Color Clr = DbComBloodColor(name1, ref ret);
            if (ret)
            {
                return Clr;
            }

            Clr = DbComBloodColor(name2, ref ret);
            return Clr;
        }
        #endregion

        #region 血統タイプをDBから読み込み(3)
        public Color DbComSearchBloodColor(String name1, String name2, String name3)
        {
            Boolean ret = false;
            if (name1 == "" || name2 == "" || name3 == "")
            {
                return Color.White;
            }
            Color Clr = DbComBloodColor(name1, ref ret);
            if (ret)
            {
                return Clr;
            }

            Clr = DbComBloodColor(name2, ref ret);
            if (ret)
            {
                return Clr;
            }

            Clr = DbComBloodColor(name3, ref ret);
            return Clr;
        }
        #endregion

        #region 血統タイプをDBから読み込み(共通・色)
        private Color DbComBloodColor(String name, ref Boolean ret)
        {

            String fBloodName;
            String tmp = "";
            int cnt = 0;
            ret = false;
            int idx = 0;

#if false
            if (St.Count == 0) return Color.White;  //取得失敗は処理しない。
            if (Hn.Count == 0) return Color.White;  //取得失敗は処理しない。
            
            /* １．設定データ(ST)からデータを取得する */
            for (idx = 0; idx < St.Count; idx++)
            {
                var values = St[idx].Split(',');
                if(name == values[0])
                {
                    ret = true;
                    return g_FuncHorceKindColor(values[3]);
                }
            }

            fBloodName = name;

        re:
            while (cnt < 7) /* 仕様変更#17 */
            {
                for (idx = 0; idx < Hn.Count; idx++)
                {
                    var values = Hn[idx].Split(',');
                    if (fBloodName == values[4])
                    {
                        //父親を入れておく
                        fBloodName = values[4];
                        //検索にヒットしたら検索実行
                        for (int idx2 = 0; idx2 < St.Count; idx2++)
                        {
                            var value2 = St[idx2].Split(',');
                            if (value2[0] == values[4])
                            {
                                /* 検索にヒットしたら処理中断 */
                                ret = true;
                                return g_FuncHorceKindColor(value2[3]);
                            }
                        }
                        //検索に合致しない場合また父親探し
                        cnt++;
                        goto re;
                    }
                }
#else
#if false
                /* 検索にヒットしたら処理中断 */
                /* ヒットしたら父馬の血統登録番号で検索続行 */
                for (int idx2 = 0; idx2 < St.Count; idx2++)
                {
                    var value2 = St[idx2].Split(',');
                    if (value2[0] == values[4])
                    {
                        /* 検索にヒットしたら処理中断 */
                        ret = true;
                        return g_FuncHorceKindColor(value2[3]);
                    }
                }
                cnt++;
                continue;
                cnt++;
            }
#endif

#if true
            if (db.TextReader_aCell("ST", name, "0", 3, ref tmp) == 1)
            {
                ret = true;
                return g_FuncHorceKindColor(tmp);
            }

            fBloodName = name;



            while (cnt < 7) /* 仕様変更#17 */
            {
                tmp = "";
                db.TextReader_aCell("HN", fBloodName, "0", 4, ref tmp);
                if (tmp == "")
                {
                    /* 父馬がヒットしなかったら検索続行しない */
                    break;
                }
                else
                {
                    /* ヒットしたら父馬の血統登録番号で検索続行 */
                    fBloodName = tmp;
                    tmp = "";
                    if (db.TextReader_aCell("ST", fBloodName, "0", 3, ref tmp) == 1)
                    {
                        /* 検索にヒットしたら処理中断 */
                        ret = true;
                        return g_FuncHorceKindColor(tmp);
                    }
                    else
                    {
                        /* 検索にヒットしない場合、カウンタアップして処理続行 */
                        cnt++;
                    }
                }
            }
#endif

            //Console.WriteLine("あ");
#endif
            ret = false;
            return Color.White;
        }
#endregion

#region 種牡馬カラー判定
        private Color g_FuncHorceKindColor(String Kind)
        {
            /** 種牡馬色定義　0：その他、１：ノーザンテースト系、２：ナスルーラ、３：ヘイロー系、４：サンデー系、５：ネイティブダンサー系
             * ６：セントサイモン、７：ハンプトン系、８：テディ系、９：マンノウォー 、１０：マッチェム*/

            switch (Kind)
            {
                case "0":
                    return Color.Brown;
                case "1":
                    return Color.SkyBlue;
                case "2":
                    return Color.DeepPink;
                case "3":
                    return Color.LightGreen;
                case "4":
                    return Color.Yellow;
                case "5":
                    return Color.Orange;
                case "6":
                    return Color.MediumPurple;
                case "7":
                    return Color.DarkGreen;
                case "8":
                    return Color.LightGray;
                case "9":
                    return Color.DarkGoldenrod;
                case "10":
                    return Color.DarkGray;
                default:
                    return Color.White;
            }
        }
#endregion


#region 過去走データをDBから取得(マッピング)
        public int DbComGetOldRunDataMapping(String KettoNum, ref List<String> outParam, int RaceCount)
        {
            int ret = 0;
            int count = 1;
            List<String> tmp;
           
                tmp = new List<string>();
                //血統登録番号＋O走で検索をかける
                ret = DbComGetOldRunData(KettoNum, RaceCount, ref tmp);
                outParam = tmp;
          
            return ret; /* 仕様変更#15 */
        }
#endregion

#region 過去走データをDBから取得(共通)
        private int DbComGetOldRunData(String KettoNum, int RunNum, ref List<String> outParam)
        {
            List<String> tmp = new List<string>();
            List<String> LibTmp = new List<String>();   
            db.TextReader_Col("0", "SE", 7, ref LibTmp, KettoNum + String.Format("{0:00}",RunNum));
            if(LibTmp.Count == 0)
            {
                return 0;
            }

            outParam = LibTmp;

            int res = db.TextReader_Col("0", "RA", 0, ref tmp, LibTmp[0].Substring(0, 16));

            if (tmp.Count == 0)
            {
                //失敗していてもSEデータが成功しているため、returnは1で返す
                return 1;
            }
            for (int i = 0; i < tmp.Count; i++)
            {
                outParam.Add(tmp[i]);
            }

            return res;
        }
#endregion


#region 枠番からカラー算出
        public Color WakubanToColor(int Kind, String Wakuban)
        {
            switch(Wakuban)
            {
                case "1":
                    if(Kind == 0)
                    {
                        return Color.White;
                    }
                    else
                    {
                        return Color.Black;
                    }
                    
                case "2":
                    if(Kind == 0)
                    {
                        return Color.Black;
                    }
                    else
                    {
                        return Color.White;
                    }
                    
                case "3":
                    if (Kind == 0)
                    {
                        return Color.Red;
                    }
                    else
                    {
                        return Color.White;
                    }
                case "4":
                    if (Kind == 0)
                    {
                        return Color.Blue;
                    }
                    else
                    {
                        return Color.White;
                    }
                case "5":
                    if (Kind == 0)
                    {
                        return Color.Yellow;
                    }
                    else
                    {
                        return Color.Black;
                    }
                case "6":
                    if (Kind == 0)
                    {
                        return Color.Green;
                    }
                    else
                    {
                        return Color.White;
                    }
                 case "7":
                    if (Kind == 0)
                    {
                        return Color.Orange;
                    }
                    else
                    {
                        return Color.Black;
                    }
                case "8":
                    if (Kind == 0)
                    {
                        return Color.Pink;
                    }
                    else
                    {
                        return Color.Black;
                    }
            }

            if (Kind == 0)
            {
                return Color.White;
            }
            else
            {
                return Color.Black;
            }
           
        }
#endregion

        public Boolean GetJockeyChangeInfo(String RaKey, ref List<JvComDbData.JvDbJcData> retArray)
        {
            //List<JvComDbData.JvDbJcData> retArray = new List<JvComDbData.JvDbJcData>();
            List<JvComDbData.JvDbJcData> JvDbJcData = new List<JvComDbData.JvDbJcData>();
            List<String> tmp = new List<string>();
            Boolean SetFlag = false;
            JvComDbData.JvDbJcData cacheJcData;


            for (int i = 1; ; i++) //エラーになるまで継続
            {
                tmp.Clear();
                cacheJcData = new JvComDbData.JvDbJcData();
                if (db.TextReader_Col(RaKey.Substring(0, 8), "JC", 0, ref tmp, i.ToString()) != 0)
                {
                    if (cacheJcData.ReadData_AV(ref tmp) == 0)
                    {
                        break;
                    }
                    else
                    {
                        JvDbJcData.Add(cacheJcData);
                        Console.WriteLine("[INFO]InitChangeInfo TRUE!");
                    }
                }
                else
                {
                    //取得終了・または取得なし
                    return false;
                }
            }

            //騎手変更
            for (int i = 0; i < JvDbJcData.Count; i++)
            {
                SetFlag = false;
                if (JvDbJcData[i].Key1.Substring(0, 16) == RaKey)
                {
                    if (retArray.Count == 0)
                    {
                        SetFlag = true;
                        retArray.Add(JvDbJcData[i]);
                    }
                    else
                    {
                        for (int j = 0; j < retArray.Count; j++)
                        {
                            if (retArray[j].BeforeInfo1.JcokeyCode == JvDbJcData[i].BeforeInfo1.JcokeyCode)
                            {
                                //すでに登録済みの騎手の場合は上書き
                                retArray[j] = JvDbJcData[i];
                                SetFlag = true;
                            }
                        }
                        if (!SetFlag)
                        {
                            //for文の中で検索に引っかからない場合は追加
                            retArray.Add(JvDbJcData[i]);
                        }

                    }

                }

            }

            return (retArray.Count == 0 ? false : true);
        }

    }
}