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

        struct DB_DATA_STRUCT
        {
            public long offset;
            public long pickUp;
        };

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
                    //File.AppendAllText(file, "#" + dtSpec + ":" + date + " encord:utf-8 \r\n", enc);
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

        private int dbConnectWriter(String Date, String dtSpec, String buff)
        {
            return 0;
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
            String file;
            try
            {
                file = ReadCsvComFileName(dtSpec, data);
                if (file == "")
                {
                    return;
                }
                else if (data == "0")
                {
                    /* ディレクトリ存在有無の確認 */
                    if (!Directory.Exists(dtSpec + "_MST/"))
                    {
                        Directory.CreateDirectory(dtSpec + "_MST/");
                    }
                }
                else if(data.Length == 8)
                {
                    /* ディレクトリ存在有無の確認 */
                    if (!Directory.Exists(dtSpec + "/" + data + "/"))
                    {
                        Directory.CreateDirectory(dtSpec + "/" + data + "/");
                    }
                }
                else
                {
                    /* ファイル名とフォルダ名が異なる場合 */ /* 仕様変更#12 */
                    /* フォルダ名は日付 */
                    /* ファイル名はレース毎にする場合 */
                    /* ディレクトリ存在有無の確認 */
                    if (!Directory.Exists(dtSpec + "/" + data.Substring(0,8) + "/"))
                    {
                        Directory.CreateDirectory(dtSpec + "/" + data.Substring(0, 8) + "/");
                    }
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

        /**************************************************
         * @func  先頭列(データキー)と合致するデータ提供関数(全て)のラッピング関数
         * @event 関数コール
         * @inPrm   dtSpec：データ種
         *          Key：キー(JvData仕様書で指定のもの)
         *          date：レース開催日
         *          ※Masterデータは「０」を指定
         *          Kind：取得したいデータの列数(配列(0~))
         *          tmp：データ保管場所の参照(ref)
         * @OutPrm  int：取得結果　０：失敗・プロセス実行中
         *                      　１：取得成功
         **************************************************/  
        public int TextReader_Row(String Date, String dtSpec, int Kind, ref List<String> srt)
        {
            /* たて列ぜんぶ */
            return ReadCsv(Date, dtSpec, Kind, ref srt, 0, "", 0);
        }

        public int TextReader_Col(String Date, String dtSpec, int Kind, ref List<String> srt, String Key)
        {
            /* よこ列ぜんぶ */
            if (Kind == 0)
            {
                return ReadCsv(Date, dtSpec, Kind, ref srt, 1, Key, 0);
            }
            else
            {
                return ReadCsv(Date, dtSpec, Kind, ref srt, 3, Key, 0);
            }
        }

        /**************************************************
         * @func  先頭列(データキー)と合致するデータ提供関数(一つだけ)のラッピング関数
         * @event 関数コール
         * @inPrm   dtSpec：データ種
         *          Key：キー(JvData仕様書で指定のもの)
         *          date：レース開催日
         *          ※Masterデータは「０」を指定
         *          Kind：取得したいデータの列数(配列(0~))
         *          tmp：データ保管場所の参照(ref)
         * @OutPrm  int：取得結果　０：失敗・プロセス実行中
         *                      　１：取得成功
         **************************************************/  
        public int TextReader_aCell(String dtSpec, String Key, String Date, int Kind, ref String str)
        {
            int ret = 0;
            List<String> ArrayStr = new List<string>();
            ret = ReadCsv(Date, dtSpec, Kind, ref ArrayStr, 2, Key, 0);

            /* 取得データなし */
            if(ArrayStr.Count == 0)
            {
                str = "";
                return 0;
            }

            str = ArrayStr[0];
            return 1;
        }

        /**************************************************
         * @func  先頭列(データキー)と合致するデータ提供関数
         *   ３つの関数を取りまとめ
         * @event 関数コール
         * @inPrm   dtSpec：データ種
         *          Key：キー(JvData仕様書で指定のもの)
         *          date：レース開催日
         *          ※Masterデータは「０」を指定
         *          Kind：取得したいデータの列数(配列(0~))もしくは行数
         *          tmp：データ保管場所の参照(ref)
         *          Key：取得するキー
         *          offset：指定行から読み込む
         * @OutPrm  int：取得結果　０：失敗・プロセス実行中
         *          TextReader_Row：１：成功
         *          それ以外：１～：成功（検索合致行が戻る）
         **************************************************/
        static int ReadCsv(String Date, String dtSpec,int Kind, ref List<String> str, int AllData, String Key, long offset)
        {
            String file;
            int loop = 0;           //列カウンタ
            int ColLoopCount = 0;   //行カウンタ 


            file = ReadCsvComFileName(dtSpec, Date);
            if (file == "")
            {
                return 0;
            }
            
            try
            {
                FileStream fs = File.OpenRead(file);

                if(offset != 0)
                {
                    fs.Seek(offset, SeekOrigin.Begin);
                }
                
                // csvファイルを開く
                using (var sr = new System.IO.StreamReader(fs))
                {
                    // ストリームの末尾まで繰り返す
                    while (!sr.EndOfStream)
                    {
                        // ファイルを指定行から１行読み込む
                        var line = sr.ReadLine();
                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split(',');

                        ColLoopCount++;

                        //先頭が#から始まる場合は読み込み対象外：ファイル情報などを記載
                        if(values[0].Substring(0,1) == "#")
                        {
							continue;
						}
                        
                        // 出力する
                        foreach (String value in values)
                        {
                            if(AllData == 0)
                            {
                                /* たて一列(TextReader_Row) */
                                if (loop == Kind)
                                {
                                    str.Add(value);
                                    break;
                                }
                            }
                            else if(AllData == 1)
                            {
                                 // よこ一列(TextReader_Col)
                                if (values[0] == Key)
                                {
                                    for(int i = 0; i < values.Length; i++)
                                    {
                                        str.Add(values[i]);
                                    }
                                    return ColLoopCount;
                                }
                            }
                            else if(AllData == 2)
                            {
                                 // １セルだけ(TextReader_aCell)
                                if (values[0] == Key)
                                {
                                    str.Add(values[Kind]);
                                    return ColLoopCount;
                                }
                            }
                            else if (AllData == 3)
                            {
                                // よこ一列(TextReader_Col)→キー指定
                                if (values[Kind] == Key)
                                {
                                    for (int i = 0; i < values.Length; i++)
                                    {
                                        str.Add(values[i]);
                                    }
                                    return ColLoopCount;
                                }
                            }
                            loop++;
                        }
                        loop = 0;
                    }
                    return 1;
                }
            }
            catch (System.Exception e)
            {
                // ファイルを開くのに失敗したとき
                System.Console.WriteLine(e.Message);
                return 0;
            }
        }

        #region ファイル名を検索する共通メソッド
        static String ReadCsvComFileName(String dtSpec, String Date)
        {
            if(Date == null || dtSpec == null || Date == "" || dtSpec == "") { return ""; }

            switch(Date.Length)
            {
                case 1:
                    /* マスターデータ */
                    return @"" + dtSpec + "_MST/" + dtSpec + ".csv";
                case 8:
                    /* レース開催日 */
                    return @"" + dtSpec + "/" + Date + "/" + dtSpec + Date + ".csv";
                case 16:
                    /* レース毎 */
                    return @"" + dtSpec + "/" + Date.Substring(0,8) + "/" + dtSpec + Date + ".csv";
            }
            return "";
        }
        #endregion

        /**************************************************
         * @func  ファイル(DB)の削除
         * @event 関数コール
         * @inPrm   dtSpec：削除するデータ種
         * @OutPrm  int：結果　　　０：失敗・プロセス実行中
         *                      　１：取得成功
         **************************************************/
        public int DeleteCsv(String dtSpec)
        {
            return DeleteCsv(dtSpec, "", true);
        }

        public int DeleteCsv(String dtSpec, String filename)
        {
            return DeleteCsv(dtSpec, filename, true);
        }

        public int DeleteCsv(String dtSpec, String filename, Boolean DirectoryFlag)
        {
            String file = @"" + dtSpec + "/" + filename;
            int ret = 0;

            try
            {
                if (DirectoryFlag)
                {
                    /* ファイル削除 */
                    File.Delete(file);
                    ret = 1;
                }
                else
                {
                    /* 一時的にコピー */
                    File.Copy(file, @"" + "COPY/" + dtSpec + filename);

                    /* ディレクトリ削除 */
                    Directory.Delete(@"" + dtSpec + "/", true);

                    /* ファイルを元に戻す */
                    File.Copy(@"" + "COPY/" + dtSpec + filename, file);

                    ret = 1;
                }
            } 
            catch (IOException e)
            {
                Console.WriteLine(e);   
                ret = 0;
            }
            return ret;
        }

        /**************************************************
         * @func  先頭列(データキー)と合致するデータ提供関数(単語毎)
         *　！！！！！！！この関数は使用しないこと！！！！！！！
         * @event 関数コール
         * @inPrm   dtSpec：データ種
         *          Key：キー(JvData仕様書で指定のもの)
         *          date：レース開催日
         *          ※Masterデータは「０」を指定
         *          Kind：取得したいデータの列数(配列(0~))
         *          tmp：データ保管場所の参照(ref)
         * @OutPrm  int：取得結果　０：失敗・プロセス実行中
         *                      　１：取得成功
         **************************************************/
        public int Read_KeyData(String dtSpec, String key, String date, int Kind, ref String tmp)
        {
            String file;
            if(date == "0")
            {
                file = @"" + dtSpec + "_MST/" + dtSpec + ".csv";
            }
            else
            {
                file = @"" + dtSpec + "/" + date + "/" + dtSpec + date + ".csv";
            }

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
                        if (line.Length == 0) { return -1; }

                        // 読み込んだ一行をカンマ毎に分けて配列に格納する
                        var values = line.Split(',');

                        // キーと一致するかチェック
                        if (values[0] == key)
                        {
                            tmp = values[Kind];
                            return 1;
                        }
                    }
                    return 0;
                }
            }
            catch(IOException ex)
            {
                MessageBox.Show("データファイルにアクセス出来ませんでした。\n別プロセスで実行中です。");
                Console.WriteLine(ex);
                return 0;
            }
        }



    }
}
