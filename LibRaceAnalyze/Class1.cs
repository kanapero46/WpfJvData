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
            return LIBRARY_VERSION;
        }

        //分析
        public bool RaceAnalyzeExec( HorceInfo hc, RaceInfo rc, ref float ret )
        {
            int[] num = new int[20];
            int[] rank = new int[4];
            float fNum = 0;
            bool fJockeyChange = false;


            
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

                //騎手：ジョッキーコード
                if(values[28] == hc.Jockey1.Value)
                {
                    //着順平均
                    num[0]++;   //全体数
                    num[1] += Int32.Parse(values[21]); //着順

                    //複勝率
                    if(Int32.Parse(values[21]) <= 3 && 1 <= Int32.Parse(values[21]))
                    {
                        num[2]++; //3着内回数
                    }

                    //該当競馬場平均着順
                    if( rc.Cource1 == values[4] )
                    {
                        num[3]++; //該当競馬場出走回数
                        num[4] += Int32.Parse(values[21]); //該当競馬場着順合計
                                                           //複勝率
                        if (Int32.Parse(values[21]) <= 3 && 1 <= Int32.Parse(values[21]))
                        {
                            num[5]++; //3着内回数
                        }
                    }
                }

                if (hc.Jockey1.Value == "")
                {
                    //前走データがない場合
                    //なにもしない
                }
                else if(hc.OldJockey1.Value == hc.Jockey1.Value)
                {
                    //前走から同じ騎手
                    fJockeyChange = true;
                }
                else if (values[28] == hc.OldJockey1.Value)
                {
                    //着順平均
                    num[6]++;   //全体数
                    num[7] += Int32.Parse(values[21]); //着順

                    //複勝率
                    if (Int32.Parse(values[21]) <= 3 && 1 <= Int32.Parse(values[21]))
                    {
                        num[8]++; //3着内回数
                    }

                    //該当競馬場平均着順
                    if (rc.Cource1 == values[4])
                    {
                        num[9]++; //該当競馬場出走回数
                        num[10] += Int32.Parse(values[21]); //該当競馬場着順合計
                                                            //複勝率
                        if (Int32.Parse(values[21]) <= 3 && 1 <= Int32.Parse(values[21]))
                        {
                            num[11]++; //3着内回数
                        }
                    }
                }
            }



            return true;
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
