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

namespace WpfApp1.dbCom1
{
    public class dbCom
    {

        dbConnect db = new dbConnect();

        #region 血統タイプをDBから読み込み(1)
        public String DbComSearchBloodType(String name1)
        {
            String tmp = "";
            if(name1 == "")
            {
                return "";
            }

            if(DbComBloodType(name1, ref tmp))
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
            if(name1 == "" || name2 == "")
            {
                return "";
            }
         
            if(DbComBloodType(name1, ref tmp))
            {
               return tmp;
            }

            if(DbComBloodType(name2, ref tmp))
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
            if(name1 == "" || name2 == "" || name3 == "")
            {
                return "";
            }
         
            if(DbComBloodType(name1, ref tmp))
            {
               return tmp;
            }

            if(DbComBloodType(name2, ref tmp))
            {
                return tmp;
            }

            if(DbComBloodType(name3, ref tmp))
            {
                return tmp;
            }

            return "";
        }
        #endregion

        #region 血統タイプをDBから読み込み(共通)
        private Boolean DbComBloodType(String name, ref String outParam)
        {
            String fBloodName;
            String tmp = "";
            int cnt = 0;

            /* １．設定データ(ST)からデータを取得する */
            if(db.TextReader_aCell("ST", name, "0", 2, ref tmp) == 1)
            {
                outParam = tmp;
                return true;
            }

            fBloodName = name;
            while(cnt < 5)
            {
                tmp = "";
                db.TextReader_aCell("HN", fBloodName, "0", 4, ref tmp);
                if(tmp == "")
                {
                    /* 父馬がヒットしなかったら検索続行しない */
                    outParam = "";
                    break;
                }
                else
                {
                    /* ヒットしたら父馬の血統登録番号で検索続行 */
                    fBloodName = tmp;
                    if(db.TextReader_aCell("ST", fBloodName, "0", 2, ref tmp)　== 1)
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


        #region 過去走データをDBから取得(マッピング)
        public int DbComGetOldRunDataMapping(String KettoNum, ref List<String> outParam, int RaceCount)
        {
            int ret = 0;
            int count = 1;
            List<String> tmp;
            do
            {
                tmp = new List<string>();
                //血統登録番号＋O走で検索をかける
                ret = DbComGetOldRunData(KettoNum, count, ref tmp);
                outParam = tmp;
            } while (RaceCount != count && ret != 0);
            return 1;
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

            db.TextReader_Col("0", "RA", 0, ref tmp, LibTmp[0].Substring(0, 16));

            if (tmp.Count == 0)
            {
                //失敗していてもSEデータが成功しているため、returnは1で返す
                return 1;
            }
            for (int i = 0; i < tmp.Count; i++)
            {
                outParam.Add(tmp[i]);
            }

            return 1;
        }
        #endregion

    }
}