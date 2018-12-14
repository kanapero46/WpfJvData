using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using ADOX;

namespace WpfApp1.dbAccess
{
    public class dbConnect
    {
        Boolean boolInit;
        String sCS = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=data/";
        String sqlQuery;
        OleDbCommand command;
        OleDbConnection connection;

        /* SQL文の基礎 */
        String strInsert = "INSERT INTO ";

        /* csvエンコード */
        Encoding enc = Encoding.GetEncoding("utf-8");

        public dbConnect()
        {
            /* 引数なしクラス宣言はなにもしない */
        }

        public dbConnect(String dtSpec, ref String buff, ref int ret)
        {
            /* 日付指定なし(先頭行に書き込み日付書き込みVer) */
            String date = DateTime.Today.ToShortDateString();
            String file = @"" + dtSpec + "/" + dtSpec + ".csv";

            try
            {
                /* ディレクトリ存在有無の確認 */
                if (!Directory.Exists(dtSpec + "/"))
                {
                    Directory.CreateDirectory(dtSpec + "/");
                }

                File.AppendAllText(file, "#：" + date + "\r\n", enc);

                /* ファイルの存在有無の確認 */
                if (!Directory.Exists(file))
                {
#if DEBUG
                    //               File.AppendAllText(file, "#日付," + "\r\n", enc);
#else
                    File.AppendAllText(file, "#" + dtSpec + ":" + data + " encord:utf-8 \r\n", enc);
#endif
                }

                /* ファイルの書き込み */
                File.AppendAllText(file, buff + "\r\n", enc);
                ret = 1;
            }
            catch (IOException ex)
            {
                ret = 0;
                MessageBox.Show("ファイルの書き込みに失敗しました。");
            }
        }

        /* DBファイルに接続・書き込み */
        public dbConnect(String Date, String dtSpec, ref String buff, ref int ret)
        {
            int res;

            boolInit = false;

            /* 引数チェック */
            if(Date == "000000" || dtSpec == "") { ret = 0; return; }

            TextWriter(Date, dtSpec, buff, ref ret);

            /* 初期化完了 */
            boolInit = true;
         }


        public int dbConnectInit(String Date)
        {
            ADOX.Catalog cat = new ADOX.Catalog();
            ADODB.Connection db1;
            connection = new OleDbConnection();
            /* DB接続 */
            connection.ConnectionString = sCS + Date + ".accdb;";
            /* DBオープン　 */
            try
            {
                connection.Open();
            }
            catch(InvalidOperationException e)
            {
                cat.Create(sCS + Date + ".accdb;" + "Jet OLEDB:Engine Type=5");
                cat = null;
                return 1;
            }
            catch (OleDbException e)
            {
                db1 = cat.Create(sCS + Date + ".accdb;" + "Jet OLEDB:Engine Type=5");
                db1.Close();
                cat = null;
                return 0;
            }
            return 1;
        }

        private int dbConnectWriter(String Date, String dtSpec, String buff)
        {
            int res = 0;
            command = new OleDbCommand();
            command.CommandText = strInsert + dtSpec + " VALUES " + buff;
            command.Connection = connection;

            try
            {
                /* 書き込み */
                res = command.ExecuteNonQuery();
            }
            catch(InvalidOperationException ex)
            {

            }

            connection.Close();
            return res;
        }
       
        private String convDbQurey(String data)
        {
            int strlen = 0;
            String str = "";
            int Look;
        
            for (int i = 0; i < data.Length; i = strlen)
            {
                strlen += data.Substring(i,data.Length).IndexOf(",");
                str += "'" + data.Substring(i, strlen) + "'";
                strlen++;
            }

            data = data.Replace(",", "");
            return (str);
        }

        private void TextWriter(String data ,String dtSpec, String buff, ref int ret)
        {
            String file = @"" + dtSpec + "/" + data + "/" + dtSpec + data + ".csv";
            

            try
            {
                /* ディレクトリ存在有無の確認 */
                if (!Directory.Exists(dtSpec + "/" + data + "/"))
                {
                    Directory.CreateDirectory(dtSpec + "/" + data + "/"); 
                }

                /* ファイルの存在有無の確認 */
                if (!Directory.Exists(file))
                {
#if DEBUG
       //               File.AppendAllText(file, "#日付," + "\r\n", enc);
#else
                    File.AppendAllText(file, "#" + dtSpec + ":" + data + " encord:utf-8 \r\n", enc);
#endif
                }

                /* ファイルの書き込み */
                File.AppendAllText(file, buff + "\r\n", enc);
                ret = 1;
            }
            catch(IOException ex)
            {
                ret = 0;
                MessageBox.Show("ファイルの書き込みに失敗しました。");
            }
        
        }

        public int TextReader(String Date, String dtSpec, int Kind, ref List<String> srt)
        {
            ReadCsv(Date, dtSpec, Kind, ref srt);
            /**
          //  int strlen;
            string str = "";
            String[] tmp;
            String file = @"" + Date + "/" + dtSpec + ".csv";
            tmp = File.ReadAllLines(file, enc); 

            for(int i=0; i < tmp.Length; i++)
            {
                
                for(int j=0 ,strlen=0; j<tmp[i].Length; j = (strlen+1))
                {
                    strlen += tmp[i].Substring(j, tmp[i].Length).IndexOf(",");
                    str += "'" + tmp[i].Substring(j, strlen) + "'";
                    if(j == Kind) { srt.Add(str); break;  }
                }
            }
            */
            return 0;
        }

        static void ReadCsv(String Date, String dtSpec,int Kind, ref List<String> str)
        {
            int loop = 0;
            try
            {
                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(@"" + dtSpec + "/" + Date + "/" + dtSpec + Date +".csv"))
                {
                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split(',');
                        // 出力する
                        foreach (String value in values)
                        {
                            if (loop == Kind)
                            {
                                str.Add(value);
                                break;
                            }
                            loop++;
                        }
                        loop = 0;
                    }
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したとき
                System.Console.WriteLine(e.Message);
            }
        }

        public void DeleteCsv(String dtSpec)
        {
            String file = @"" + dtSpec + "/";
            try
            {
                Directory.Delete(file, true);
            }
            catch(IOException ex)
            {
                MessageBox.Show("データファイルにアクセス出来ませんでした。\n別プロセスで実行中です。");
                Console.WriteLine(ex);
            }
        }

        public void Read_KeyData(String dtSpec, String key, String date, int Kind, ref String tmp)
        {
            String file = @"" + dtSpec + "/" + date + "/" + dtSpec + date + ".csv";

            try
            {
                // csvファイルを開く
                using (var sr = new StreamReader(file))
                {
                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルから一行読み込む
                        var line = sr.ReadLine();

                        //一行もなければ終了
                        if (line.Length == 0) { return; }

                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split(',');

                        // キーと一致するかチェック
                        if (values[0] == key)
                        {
                            tmp = values[Kind];
                            return;
                        }
                    }
                }
            }
            catch(IOException ex)
            {
                MessageBox.Show("データファイルにアクセス出来ませんでした。\n別プロセスで実行中です。");
                Console.WriteLine(ex);
            }
        }



    }
}
