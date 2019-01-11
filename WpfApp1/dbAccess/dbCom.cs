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

namespace WpfApp1.dbCom
{
    public class dbCom
    {

        DBConnect db = new DBConnect();

        #region 血統タイプをDBから読み込み(1)
        public Boolean DbComSearchBloodType(String name1)
        {
            String tmp = "";
            if(name1 == "")
            {
                return "";
            }

            return DbComBloodType(name1, ref tmp);
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
        private boolean DbComBloodType(String name, ref String outParam)
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
    }
}