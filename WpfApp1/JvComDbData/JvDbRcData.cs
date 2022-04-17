using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    class JvDbRcData
    {
        private const String SPEC = "RC";

        private String DbWriteStr = "";
        private String DbG1WriteStr = "";
        private Boolean WriteCompFlg = false;

        //インスタンス初期化
        JVData_Struct.JV_RC_RECORD DbParam = new JVData_Struct.JV_RC_RECORD();
        Class.com.JvComClass LOG = new Class.com.JvComClass();

        //G1レース定義
        enum G1RACE
        {
            G1_NODATA = 0,
            G1_FEBURARY = 1,
            G1_TAKAMATSU,
            G1_OSAKAHAI,
            G1_OUKASHO,
            G1_SATSUKISHO,
            G1_TENNOSHO_S,
            G1_NHK_MILE,
            G1_VICTORIA,
            G1_OAKS,
            G1_DERBY,
            G1_YASUDA,
            G1_TAKARAZUKA,
            G1_SPRINTERS,
            G1_SHUKASHO,
            G1_KIKKASHO,
            G1_TENNOSHO_A,
            G1_ELISABETH,
            G1_MILE_CHANPION,
            G1_JAPANCUP,
            G1_JAPANCUP_DIRT,
            G1_CHAMPIONS,
            G1_JUVENILE_F,
            G1_FUTURITY,
            G1_ARIMAKINEN,
            G1_HOPEFULL,
            G1_NANBUHAI,
            G1_JBC_LADIES,
            G1_JBC_SPRINT,
            G1_JBC_CLASSIC,
        }

        public JvDbRcData()
        {
            InitMsg();
        }

        public void JvDbRcSetData(ref String buff)
        {
            String tmpDbParam = "";
            DbParam.SetDataB(ref buff);
            WriteCompFlg = true;    //書き込みフラグ

            //G1レコードの場合は先頭をGにする
            if (DbParam.RecInfoKubun == "2") 
            {
                tmpDbParam += "G";
                tmpDbParam += String.Format("{0:00}", CheckRaceName(DbParam.Hondai));
                tmpDbParam += ",";
            }
            else
            { 
                tmpDbParam += SetRaceSyubetuCD(DbParam.SyubetuCD);
                tmpDbParam += DbParam.id.JyoCD + DbParam.TrackCD + DbParam.Kyori + ","; //キー：種別コード（２歳(A)・3歳・古馬(B)・障害(C)）、競馬場コード、トラックコード、距離
            }

            tmpDbParam += DbParam.id.Year + DbParam.id.MonthDay + ",";
            tmpDbParam += DbParam.id.RaceNum + ",";
            tmpDbParam += DbParam.Hondai + ",";
            tmpDbParam += DbParam.GradeCD + ",";
                         tmpDbParam += DbParam.RecInfoKubun + ",";
            tmpDbParam += DbParam.Kyori + ",";
            tmpDbParam += DbParam.TenkoBaba.TenkoCD + ",";
            tmpDbParam += ((Int32.Parse(DbParam.TenkoBaba.SibaBabaCD)*1) + (Int32.Parse(DbParam.TenkoBaba.DirtBabaCD)*1)) + ",";
            tmpDbParam += DbParam.RecKubun + ",";
            tmpDbParam += DbParam.RecTime + ",";
            tmpDbParam += DbParam.RecUmaInfo[0].Bamei + ",";
            tmpDbParam += DbParam.RecUmaInfo[0].UmaKigoCD + ",";
            tmpDbParam += DbParam.RecUmaInfo[0].SexCD + ",";
            tmpDbParam += DbParam.RecUmaInfo[0].Futan + ",";
            tmpDbParam += DbParam.RecUmaInfo[0].KisyuName + ",";
 //           tmpDbParam += DbParam.TrackCD + ",";
            tmpDbParam += "\n";

            if(DbParam.RecInfoKubun == "1")
            {
                //通常レコード
                DbWriteStr += tmpDbParam;
            }
            else if(DbParam.RecInfoKubun == "2")
            {
                //G1レコード
                DbG1WriteStr += tmpDbParam;
            }
        }

        public int JvDbRcWriteData()
        {
            int DbReturn = 0;
            String WriteMesg = "";

            dbAccess.dbConnect db = new dbAccess.dbConnect();

            //旧データを削除
            db.DeleteCsv("RC_MST");
            WriteMesg = DbWriteStr + DbG1WriteStr;

            //書き込み
            db = new dbAccess.dbConnect("0", SPEC, ref WriteMesg, ref DbReturn);

            LOG.CONSOLE_TIME_MD(SPEC, "JvDbRcData DB Write -> ret(" + DbReturn + ")");
            InitMsg();

            return DbReturn;
        }

        //競走種別コードを変換する。
        public String SetRaceSyubetuCD(String SyubetuCD)
        {
            switch(SyubetuCD)
            {
                case "11":
                    return "A";
                case "12":
                case "13":
                case "14":
                    return "B";
                case "18":
                case "19":
                    return "C";
                default:
                    LOG.CONSOLE_MODULE(SPEC, "Assert!! SetRaceSyubetuCD relegurCase!! > " + SyubetuCD);
                    return "_";
            }

        }

        //データあり？
        public Boolean JvDbRcEnable()
        {
            return WriteCompFlg;
        }

        //メッセージ初期化
        private void InitMsg()
        {
            DbWriteStr = "";
            DbG1WriteStr = "";
            WriteCompFlg = false;
        }


        //６文字レース名から検索
        public int CheckRaceName6(String RaceName)
        {
            String tmp = RaceName.Trim();
            switch (tmp)
            {
                case "フェブラリー":
                    return (int)G1RACE.G1_FEBURARY;
                case "高松宮記念":
                    return (int)G1RACE.G1_TAKAMATSU;
                case "大阪杯":
                    return (int)G1RACE.G1_OSAKAHAI;
                case "桜花賞":
                    return (int)G1RACE.G1_OUKASHO;
                case "皐月賞":
                    return (int)G1RACE.G1_SATSUKISHO;
                case "天皇賞（春）":
                    return (int)G1RACE.G1_TENNOSHO_S;
                case "ＮＨＫマイル":
                    return (int)G1RACE.G1_NHK_MILE;
                case "ヴィクトリア":
                    return (int)G1RACE.G1_VICTORIA;
                case "優駿牝馬":
                    return (int)G1RACE.G1_OAKS;
                case "東京優駿":
                    return (int)G1RACE.G1_DERBY;
                case "安田記念":
                    return (int)G1RACE.G1_YASUDA;
                case "宝塚記念":
                    return (int)G1RACE.G1_TAKARAZUKA;
                case "スプリンター":
                    return (int)G1RACE.G1_SPRINTERS;
                case "秋華賞":
                    return (int)G1RACE.G1_OUKASHO;
                case "菊花賞":
                    return (int)G1RACE.G1_KIKKASHO;
                case "天皇賞（秋）":
                    return (int)G1RACE.G1_TENNOSHO_A;
                case "エリザベス杯":
                    return (int)G1RACE.G1_ELISABETH;
                case "マイルＣＳ":
                    return (int)G1RACE.G1_MILE_CHANPION;
                case "ジャパンＣ":
                    return (int)G1RACE.G1_JAPANCUP;
                case "チャンピオン":
                    return (int)G1RACE.G1_CHAMPIONS;
                case "阪神ＪＦ":
                    return (int)G1RACE.G1_JUVENILE_F;
                case "朝日杯ＦＳ":
                    return (int)G1RACE.G1_FUTURITY;
                case "有馬記念":
                    return (int)G1RACE.G1_ARIMAKINEN;
                case "ホープフルＳ":
                    return (int)G1RACE.G1_HOPEFULL;
                default:
                    break;
            }
            return (int)G1RACE.G1_NODATA;
        }

        //レース名からG1レコードの定数取得
        private int CheckRaceName(String RaceName)
        {
            String tmp = RaceName.Trim();
            switch (tmp)
            {
                case "フェブラリーステークス":
                    return (int)G1RACE.G1_FEBURARY;
                case "高松宮記念":
                    return (int)G1RACE.G1_TAKAMATSU;
                case "大阪杯":
                    return (int)G1RACE.G1_OSAKAHAI;
                case "桜花賞":
                    return (int)G1RACE.G1_OUKASHO;
                case "皐月賞":
                    return (int)G1RACE.G1_SATSUKISHO;
                case "天皇賞（春）":
                    return (int)G1RACE.G1_TENNOSHO_S;
                case "ＮＨＫマイルカップ":
                    return (int)G1RACE.G1_NHK_MILE;
                case "ヴィクトリアマイル":
                    return (int)G1RACE.G1_VICTORIA;
                case "優駿牝馬":
                    return (int)G1RACE.G1_OAKS ;
                case "東京優駿":
                    return (int)G1RACE.G1_DERBY;
                case "安田記念":
                    return (int)G1RACE.G1_YASUDA;
                case "宝塚記念":
                    return (int)G1RACE.G1_TAKARAZUKA;
                case "スプリンターズステークス":
                    return (int)G1RACE.G1_SPRINTERS;
                case "秋華賞":
                    return (int)G1RACE.G1_OUKASHO;
                case "菊花賞":
                    return (int)G1RACE.G1_KIKKASHO;
                case "天皇賞（秋）":
                    return (int)G1RACE.G1_TENNOSHO_A;
                case "エリザベス女王杯":
                    return (int)G1RACE.G1_ELISABETH;
                case "マイルチャンピオンシップ":
                    return (int)G1RACE.G1_MILE_CHANPION;
                case "ジャパンカップ":
                    return (int)G1RACE.G1_JAPANCUP;
                case "チャンピオンズカップ":
                    return (int)G1RACE.G1_CHAMPIONS;
                case "阪神ジュベナイルフィリーズ":
                    return (int)G1RACE.G1_JUVENILE_F;
                case "朝日杯フューチュリティステークス":
                    return (int)G1RACE.G1_FUTURITY;
                case "有馬記念":
                    return (int)G1RACE.G1_ARIMAKINEN;
                case "ホープフルステークス":
                    return (int)G1RACE.G1_HOPEFULL;
                default:
                    break;
            }
            return (int)G1RACE.G1_NODATA;
        }
    }
}
