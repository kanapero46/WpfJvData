using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Class.com
{
    //文字列・画像などデザインに関わる部分を定義する
    class prog_def
    {

        JvComClass LOG = new JvComClass();
        
        //画像
        const String PCX_FILE_ENABLE = "ico/CheckEnable.jpg";   //画像：チェックあり
        const String PCX_FILE_DISABLE = "ico/CheckDisable2.jpg";    ///画像チェックなし
        const String PCX_FILE_RADIO_ENABLE = "ico/RadioEnable.jpg";   //画像：ラジオチェックあり
        const String PCX_FILE_RADIO_DISABLE = "ico/RadioDisable.jpg";    ///画像ラジオチェックなし

        //文字列
        const String STR_WAKU_NOTRELEASE1 = "発売なし";          //枠連「発売なし」
        const String STR_WAKU_NOTRELEASE2 = "発売なし(8頭以下)"; //枠連(詳細あり)「発売なし(8頭以下)」

        //文字列馬券種類
        const String STR_TUUJO = "通常";  //通常
        const String STR_RANK = "人気順"; //人気順
        const String STR_FORMATION = "フォーメーション"; //フォーメーション
        const String STR_BOX = "ボックス"; //ボックス
        const String STR_WHELL = "ながし"; //ながし
        const String STR_1WHELL = "軸1頭ながし"; //ながし
        const String STR_2WHELL = "軸2頭ながし"; //ながし
        const String STR_12WHELL = "1着・2着ながし"; //ながし
        const String STR_13WHELL = "1着・3着ながし"; //ながし
        const String STR_23WHELL = "2着・3着ながし"; //ながし
        const String STR_O1WHELL = "1着固定ながし"; //ながし
        const String STR_O2WHELL = "2着固定ながし"; //ながし
        const String STR_O3WHELL = "3着固定ながし"; //ながし

        //馬券発売時間
        const String STR_FIN_ODZZ = "最終オッズ";
        const String STR_ODZZ_NOW = "オッズ取得中";
        const String STR_STANDBY = "準備完了";

        //DataList
        const String STR_DTLT_FNDGET1 = "ファミリーナンバー取得中";
        const String STR_DTLT_FNDGET2 = "ファミリーナンバー取得完了";

        public prog_def()
        {
            //インスタンス時はなにもしない？
        }

        public String FILE_OPEN(String PATH)
        {
            switch(PATH)
            {
                case PCX_FILE_ENABLE: case "PCX_FILE_ENABLE":
                    return PCX_FILE_ENABLE;
                case PCX_FILE_DISABLE: case "PCX_FILE_DISABLE":
                    return PCX_FILE_DISABLE;
                case STR_WAKU_NOTRELEASE1: case "STR_WAKU_NOTRELEASE1":
                    return STR_WAKU_NOTRELEASE1;
                case STR_WAKU_NOTRELEASE2: case "STR_WAKU_NOTRELEASE2":
                    return STR_WAKU_NOTRELEASE2;
                case PCX_FILE_RADIO_ENABLE:
                case "PCX_FILE_RADIO_ENABLE":
                    return PCX_FILE_RADIO_ENABLE;
                case PCX_FILE_RADIO_DISABLE:
                case "PCX_FILE_RADIO_DISABLE":
                    return PCX_FILE_RADIO_DISABLE;
                case STR_TUUJO:
                case "STR_TUUJO":
                    return STR_TUUJO;
                case STR_RANK:
                case "STR_RANK":
                    return STR_RANK;
                case STR_FORMATION:
                case "STR_FORMATION":
                    return STR_FORMATION;
                case STR_WHELL:
                case "STR_WHELL":
                    return STR_WHELL;
                case STR_BOX:
                case "STR_BOX":
                    return STR_BOX;
                case STR_1WHELL:
                case "STR_1WHELL":
                    return STR_1WHELL;
                case STR_2WHELL:
                case "STR_2WHELL":
                    return STR_2WHELL;
                case STR_12WHELL:
                case "STR_12WHELL":
                    return STR_12WHELL;
                case STR_13WHELL:
                case "STR_13WHELL":
                    return STR_13WHELL;
                case STR_23WHELL:
                case "STR_23WHELL":
                    return STR_23WHELL;
                case STR_O1WHELL:
                case "STR_O1WHELL":
                    return STR_O1WHELL;
                case STR_O2WHELL:
                case "STR_O2WHELL":
                    return STR_O2WHELL;
                case STR_O3WHELL:
                case "STR_O3WHELL":
                    return STR_O3WHELL;
                case STR_FIN_ODZZ:
                case "STR_FIN_ODZZ":
                    return STR_FIN_ODZZ;
                case STR_ODZZ_NOW:
                case "STR_ODZZ_NOW":
                    return STR_ODZZ_NOW;
                case STR_STANDBY:
                case "STR_STANDBY":
                    return STR_STANDBY;
                case "STR_DTLT_FNDGET1":
                    return STR_DTLT_FNDGET1;
                case "STR_DTLT_FNDGET2":
                    return STR_DTLT_FNDGET2;
                default:
                    break;
            }

            //通常ありえないためアサートしておく
            LOG.ASSERT("NOT UNDEFINED FILE PATH!!!\nfile = " + PATH);
            return PATH;
        }
    }
}
