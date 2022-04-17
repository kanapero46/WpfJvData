using System;
using System.Collections.Generic;
using System.Drawing;

namespace WpfApp1.JvComDbData
{
    class JvFamilyNoData
    {

        dbAccess.dbConnect db = new dbAccess.dbConnect();
        Class.com.JvComMain LOG = new Class.com.JvComMain();

        List<String> FamilyNoData = new List<string>();
        List<String> SaveData = new List<string>();

        String tmpData = "";

        //confファイルの場所
        private const String FN_DATA_PATH = "FN/FamilyNumberConf.csv";

        //Saveデータ
        private const String FN_SAVE_PATH = "FN_MST/FN.csv";

        public JvFamilyNoData()
        {
            InitFamilyNoData();
        }

        public Color JvFnColorData(String Fno)
        {
            if(Fno == "")
            {
                return Color.White;
            }

            if(FamilyNoData.Count == 0)
            {
                LOG.CONSOLE_MODULE("FND", "FamilyNoData Not Found...");
                return Color.White;
            }

            for(int i=0; i<FamilyNoData.Count; i++)
            {
                var values = FamilyNoData[i].Split(',');
                if(Fno == values[0])
                {
                    //ファミリーナンバーが一致した
                    switch (values[1])
                    {
                        case "PW":
                            return Color.DarkRed;
                        case "SP":
                            return Color.DarkGreen;
                        case "ST":
                            return Color.DeepSkyBlue;
                        case "PW/SP":
                        case "SP/PW":
                            return Color.Yellow;
                        case "SP/ST":
                        case "ST/SP":
                            return Color.SkyBlue;
                        case "PW/ST":
                        case "ST/PW":
                            return Color.DeepPink;
                        default:
                            return Color.White;
                    }
                }
            }
            return Color.White;



        }

        private void InitFamilyNoData()
        {
            FamilyNoData = new List<string>();

            db.DbReadAllData(FN_DATA_PATH, ref FamilyNoData);
            if(FamilyNoData.Count == 0)
            {
                LOG.CONSOLE_MODULE("FND", "No Data");
                return;
            }

            db.DbReadAllData(FN_SAVE_PATH, ref SaveData);
            if(SaveData.Count == 0)
            {
                LOG.CONSOLE_MODULE("FND", "SaveData NotFound");
                //エラーではない
            }
        }

        //馬名からファミリーナンバーを保持する。
        //trueになった場合のみ、Fnoに値をセットする
        public Boolean CheckSaveBooldData(String Name, ref String Fno)
        {
            for (int i = 0; i < SaveData.Count; i++)
            {
                //カンマ区切りにする
                var values = SaveData[i].Split(',');

                if(Name == values[0])
                {
                    Fno = values[1];
                    return true;
                }

            }

            return false;
        }

        //ファミリーナンバーの登録
        public void AddSaveBooldData(String Name, String Fno)
        {
            String dummy = "";
            //まず登録済みかをチェックする
            if(CheckSaveBooldData(Name, ref dummy))
            {
                //登録済みならなにもしない。(削除・更新はしない)
                //間違った場合、ファイルを直接更新する。
                return;
            }

            tmpData += Name + "," + Fno + ",\n";
            SaveData.Add(Name + "," + Fno);
        }

        public void WritSaveBooldData()
        {
            int ret = 999;
            String tmp = "";

            //db = new dbAccess.dbConnect("FN", ref tmp, ref ret);
            db = new dbAccess.dbConnect("0", "FN", ref tmpData, ref ret);
            LOG.CONSOLE_MODULE("FND", "DBAccess ret->" + ret);
        }

        //セーブデータからファミリーナンバーを保持する
        public Boolean CheckSaveFamilyNumber(String KettoNo, ref String Fno)
        {
            if(KettoNo.Length == 10)
            {
                for(int i=0; i<SaveData.Count; i++)
                {
                    var values = SaveData[i].Split(',');
                    if(values[0] == KettoNo)
                    {
                        Fno = values[1];
                        return true;
                    }
                }
            }

            return false;
        }


    }
}
