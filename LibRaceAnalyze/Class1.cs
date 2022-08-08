using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibRaceAnalyze
{
    public class RaceAnalyzeMain
    {
        const String LIBRARY_VERSION = "00.01-00";
        FileManageCleass file;

        //過去データの読み込み
        public RaceAnalyzeMain( String Path, String Name )
        {
            InitializeClass();
            file.Path1 = Path;
            file.FileName = Name;
        }

        public RaceAnalyzeMain(String PathFullPath)
        {
            InitializeClass();
            file.FullPath1 = PathFullPath;
        }


        public

        void InitializeClass()
        {
            file = new FileManageCleass();

        }

        //ファイルの読み込み開始
        public String RaceAnalyzeInitExec()
        {
            file.ReadData();
            PrintConsole(LIBRARY_VERSION);
            return LIBRARY_VERSION;
        }

        //分析
        public bool RaceAnalyzeExec( HorceInfo hc, RaceInfo rc, int type ,ref float ret )
        {
            int[] num = new int[20];
            
            int[] boold = new int[10];//血統用データ
            int[] traner = new int[10]; //調教師データ
            float fNum = 0;
            bool fJockeyChange = false;
            bool fOldJockeyNoneFlg = false;

            int iRank = 0;


            
            //過去のレースデータが存在するかチェック
            if(file.isReadFileData() == 0)
            {
                return false;
            }

            List<String> Param = new List<string>();
            Param = file.GetParam();



            //全体着順
            for( int idx = 0; idx < Param.Count; idx++ )
            {
                //配列を分解
                String[] values = Param[idx].Split(',');

                //着順だけ入れておく
                if(!Int32.TryParse(values[21], out iRank))
                {
                    //文字列が入らないが、0を入れておく(中止・除外)
                    iRank = 0;
                }
                
                if(iRank == 0)
                {
                    //競走除外・競走中止・出走取消は除外する。(暫定)
                    continue;
                }

                num[15]++; //件数
                num[16] += iRank; //全着順(intのMaxはいかないはず)

                //騎手：ジョッキーコード
                if (values[38] == hc.Jockey1.Value)
                {
                    //着順平均
                    num[0]++;   //全体数
                    num[1] += Int32.Parse(values[21]); //着順

                    //複勝率
                    if(iRank <= 3 && 1 <= iRank)
                    {
                        num[2]++; //3着内回数
                    }

                    if(iRank == 1)
                    {
                        num[6]++;
                    }

                    //該当競馬場平均着順
                    if( rc.Cource1 == values[4] )
                    {
                        num[3]++; //該当競馬場出走回数
                        num[4] += iRank; //該当競馬場着順合計
                                                           //複勝率
                        if (iRank <= 3 && 1 <= iRank)
                        {
                            num[5]++; //3着内回数
                            
                            if(iRank == 1)
                            {
                                num[13]++; //勝率
                            }
                        }
                    }
                }

                //前回騎手情報
                if (hc.Jockey1.Value == "")
                {
                    fOldJockeyNoneFlg = true;
                }
                else
                {
                    //前走データがない場合
                    //なにもしない
                    if (hc.OldJockey1.Value == hc.Jockey1.Value)
                    {
                        //前走から同じ騎手
                        fJockeyChange = true;
                    }
                    else if (values[38] == hc.OldJockey1.Value)
                    {
                        //着順平均
                        num[7]++;   //全体数
                        num[8] += iRank; //着順

                        //複勝率
                        if (iRank <= 3 && 1 <= iRank)
                        {
                            num[9]++; //3着内回数
                            
                            if(iRank == 1)
                            {
                                num[14]++;
                            }
                        }


                        //該当競馬場平均着順
                        if (rc.Cource1 == values[4])
                        {
                            num[10]++; //該当競馬場出走回数
                            num[11] += iRank; //該当競馬場着順合計
                                              //複勝率
                            if (iRank <= 3 && 1 <= iRank)
                            {
                                num[12]++; //3着内回数

                                if(iRank == 1)
                                {
                                    num[15]++;
                                }
                            }
                        }
                    }
                }
                
                //調教師
                if(hc.Trainer1.Value == values[39])
                {
                    //出馬回数
                    traner[0]++;

                    if (iRank <= 3 && 1 <= iRank)
                    {
                        //3着内回数
                        traner[1]++;

                        if (iRank == 1)
                        {
                            //１着回数
                            traner[2]++;
                        }
                    }

                    if (values[4] == rc.Cource1)
                    {
                        //競馬場・コース別出走回数
                        boold[3]++;

                        if (iRank <= 3 && 1 <= iRank)
                        {
                            //複勝回数
                            boold[4]++;

                            if (iRank == 1)
                            {
                                //単勝回数
                                boold[5]++;
                            }
                        }
                    }
                }

                //種牡馬
                if(hc.Sire1.Name == values[43])
                {
                    //出走回数
                    boold[0]++;

                    if(iRank <= 3 && 1 <= iRank)
                    {
                        //3着内回数
                        boold[1]++;

                        if(iRank == 1)
                        {
                            //１着回数
                            boold[2]++;
                        }
                    }

                    //競馬場別(芝・ダ分ける)
                    if (values[4] == rc.Cource1 && values[9] == rc.Baba1)
                    {
                        //競馬場・コース別出走回数
                        boold[3]++;

                        if (iRank <= 3 && 1 <= iRank)
                        {
                            //複勝回数
                            boold[4]++;

                            if(iRank == 1)
                            {
                                //単勝回数
                                boold[5]++;
                            }
                        }
                    }
                }

            }

            /* ============================================ */
            /*               計算の実施                     */
            /* ============================================ */
            
            /***** 騎手 *****/
            float fJChangeCof = 0;
            float fJWinRatio = 0;
            float fJRankAve = 0;
            float fJRank3Ave = 0;
            float fJCourceRankAve = 0;
            float fJCourceRank = 0;
            float fJCourceWinAve = 0;

            float fJAllRankAve = 0;

            float[] Diff = new float[6];

            //乗り替わり係数
            //今回のジョッキー：共通処理
            fJWinRatio = num[6] / num[0]; //勝率(1.00 <--> 0.00)
            fJRankAve = num[1] / num[0]; //平均着順(1.00 <--> 18.00)
            fJRank3Ave = num[2] / num[0]; //複勝率(1.00 <--> 0.00)
            fJCourceRank = num[4] / num[3]; //競馬場平均着順(1.00 <--> 18.00)
            fJCourceRankAve = num[5] / num[3]; //競馬場複勝率(1.00 <--> 0.00)
            fJCourceWinAve = num[13] / num[3]; //競馬場勝率(1.00 <--> 0.00)

            if (fOldJockeyNoneFlg || fJockeyChange)
            {
                //新馬戦の場合 or 乗り替わりなし
                //今回の騎手の係数を使うため、なにもしない。
            }
            else
            {
                //乗り替わり
                //今回騎手を100としたときの増加/減少を調べる
                Diff[0] = (num[14] / num[7]) / fJWinRatio; //勝率
                Diff[1] =  (num[8] / num[7]) / fJRankAve; //平均着順(低い方がいい)
                Diff[2] = (num[9] / num[7]) / fJRank3Ave; //複勝率
                Diff[3] = (num[11] / num[10]) / fJCourceRank; //競馬場平均着順
                Diff[4] = (num[12] / num[10]) / fJCourceRankAve; //競馬場複勝率
                Diff[5] = (num[14] / num[10]) / fJCourceWinAve; //競馬場勝率
            }

            /***** 調教師 *****/
            float fTWinAve = 0; //勝率
            float fT3Ave = 0; //複勝率
            float fTCourceWin = 0; //競馬場勝率
            float fTCource3Ave = 0; //競馬場複勝率

            fTWinAve = traner[2] / traner[0]; //勝率
            fT3Ave = traner[1] / traner[0]; //複勝率
            fTCourceWin = traner[5] / traner[3]; //競馬場勝率
            fTCource3Ave = traner[4] / traner[3]; //競馬場勝率

            /***** 種牡馬 *****/
            float fBWinAve = 0;
            float fB3Ave = 0; //複勝率
            float fBCourceWin = 0; //競馬場勝率
            float fBCource3Ave = 0; //競馬場複勝率
            fBWinAve = boold[2] / boold[0]; //勝率
            fB3Ave = boold[1] / boold[0]; //複勝率
            fBCourceWin = boold[5] / boold[3]; //競馬場勝率
            fBCource3Ave = boold[4] / boold[3]; //競馬場勝率


            float[] rank = new float[7];
            //1着の計算
            //平均着順
            rank[0] = (float)(((fJRankAve * 1.0) + (fJCourceRank * 0.9)) / 2 
                * (fOldJockeyNoneFlg || fJockeyChange ? 1 : Diff[1])
                * (fOldJockeyNoneFlg || fJockeyChange ? 1 : Diff[3])
                ); //該当競馬場は多めに設定(値を低くすると、値は高くでる)
            rank[0] = fJAllRankAve / rank[0]; //rank[0]には騎手平均着順指数をいれる(1.00中央値。大きいければ大きいほどよい)

            //勝率の計算
            rank[1] = (float)((fJWinRatio + fJCourceRankAve * 1.1)
                + (fOldJockeyNoneFlg || fJockeyChange ? 0 : Diff[0])
                + (fOldJockeyNoneFlg || fJockeyChange ? 0 : Diff[5] * 1.1)
                / (fOldJockeyNoneFlg || fJockeyChange ? 2 : 4)
                );

            //複勝率の計算
            rank[2] = (float)((fJRank3Ave + fJCourceWinAve * 1.1)
                + (fOldJockeyNoneFlg || fJockeyChange ? 0 : Diff[2])
                + (fOldJockeyNoneFlg || fJockeyChange ? 0 : Diff[4] * 1.1)
                / (fOldJockeyNoneFlg || fJockeyChange ? 2 : 4)
                );


            //調教師の勝率計算
            rank[3] = (float)(fTWinAve + fTCourceWin * 1.1);
            //調教師の3着内計算
            rank[4] = (float)(fT3Ave + fTCource3Ave * 1.1);

            //種牡馬勝率
            rank[5] = (float)(fBWinAve + fBCourceWin * 1.1);
            rank[6] = (float)(fB3Ave + fBCource3Ave * 1.1);

            if(type == 1)
            {
                ret = (rank[1] + rank[3] + rank[5]) * rank[0];
            }
            else if(type == 2)
            {
                ret = (rank[1] + rank[3] + rank[5]) + (rank[2] + rank[4] + rank[6]) * rank[0];
            }
            else
            {
                return false;
            }

            return true;
        }

        static void PrintConsole( String Log )
        {
#if DEBUG
            DateTime time = DateTime.Now;
            Console.WriteLine("[" + time.ToString("yyyy/MM/dd HH:mm:ss.ffffff") + "][AN]... " + Log);
#else
            Console.WriteLine("[ANA]... " + Log);
#endif
        }
    }
    

    class Details
    {
        private String name;
        private String value;

        public string Name { get => name; set => name = value; }
        public string Value { get => value; set => this.value = value; }
    }

    //競走馬データ
    public class HorceInfo
    {
        private int Umaban;
        private Details Jockey;
        private float Futan;

        private Details OldJockey;
        private Details Trainer;
        private Details Sire;

        public int Umaban1 { get => Umaban; set => Umaban = value; }
        public float Futan1 { get => Futan; set => Futan = value; }
        internal Details Jockey1 { get => Jockey; set => Jockey = value; }
        internal Details OldJockey1 { get => OldJockey; set => OldJockey = value; }
        internal Details Trainer1 { get => Trainer; set => Trainer = value; }
        internal Details Sire1 { get => Sire; set => Sire = value; }
    }

    public class RaceInfo
    {
        private String Cource;
        private String Baba;
        private int Distance;
        private String Courcetag;

        public string Cource1 { get => Cource; set => Cource = value; }
        public string Baba1 { get => Baba; set => Baba = value; }
        public int Distance1 { get => Distance; set => Distance = value; }
        public string Courcetag1 { get => Courcetag; set => Courcetag = value; }
    }

    class FileManageCleass
    {
        private String Path;
        private String fileName;
        private String FullPath;

        List<String> ArrayReadData;

        public string Path1 { get => Path; set => Path = value; }
        public string FileName { get => fileName; set => fileName = value; }
        public string FullPath1 { get => FullPath; set => FullPath = value; }


        public void ReadData()
        {
            ArrayReadData = new List<string>();

            if(!FileManageExist())
            {
                //ファイル存在チェック
                return;
            }

            StreamReader sr = new StreamReader(FullPath, System.Text.Encoding.GetEncoding("shift-jis"));
            

            while (!sr.EndOfStream)
            {
                //1行ずつ読み込む
                String line = sr.ReadLine();

                if(line.Substring(0, 1) == "#")
                {
                    //メモ行なので、スキップする
                    continue;
                }

                ArrayReadData.Add(line);
            }
        }

        public int isReadFileData()
        {
            int ret = 0;
            try
            {
                ret = ArrayReadData.Count;
            }
            catch(Exception)
            {
                
            }
            return ret;
        }

        public List<String> GetParam()
        {
            return ArrayReadData;
        }

        public bool FileManageExist()
        {
            FileAccessClass Ace = new FileAccessClass();
            return Ace.FileExist(FullPath);
        }
    }

    class FileAccessClass
    {
        public bool FileExist( String FilePath )
        {
            return File.Exists(FilePath);
        }
        
    }
}
