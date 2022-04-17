using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WpfApp1.lib.extSoft
{
    //TARGET Frontier JVに関連した定義・クラス
    class LibTargetClass
    {
        //TARGETで定義しているチェック種牡馬カラー(2021年2月現在のカラー)
        public Color TargetBooldTypeColor(String Type)
        {
            switch (Type)
            {
                case "サンデーサイレンス系":
                case "SS系ディープ系":
                case "Pサンデー系":
                case "Tサンデー系":
                case "Lサンデー系":
                case "Dサンデー系":
                    return Color.Yellow;
                case "ロベルト系":
                case "他のターントゥ系":
                    return Color.FromArgb(0, 255, 0);
                case "ミスプロ系":
                case "キングマンボ系":
                case "フォーティナイナー系":
                case "他のネイティヴダンサー系":
                    return Color.FromArgb(255, 128, 0);
                case "ボールドルーラー系":
                case "プリンスリーギフト系":
                case "ネヴァーベンド系":
                case "グレイソヴリン系":
                case "レッドゴッド系":
                case "BL系ｴｰﾋﾟｰｲﾝﾃﾞｨ系":
                    return Color.FromArgb(255, 128, 255);
                case "トップサンダー系":
                case "フェアリーキング系":
                case "ノーザンテースト系":
                case "ヌレイエフ系":
                case "ニジンスキー系":
                case "リファール系":
                case "サドラーズウェルズ系":
                case "他のノーザンダンサー系":
                    return Color.FromArgb(128, 255, 255);
                case "ヴァイスリージェント系":
                case "ストームバード系":
                case "ダンチヒ系":
                case "ND系ｽﾄｰﾑｷｬｯﾄ系":
                    return Color.FromArgb(0, 183, 183);
                case "セントサイモン系":
                    return Color.FromArgb(128, 128, 192);
                case "テディ系":
                    return Color.FromArgb(0, 128, 192);
                case "ハンプトン系":
                    return Color.FromArgb(0, 128, 128);
                case "ヘロド系":
                    return Color.FromArgb(128, 128, 128);
                case "マッチェム系":
                    return Color.FromArgb(192, 192, 192);
                default:
                    break;
            }
            return Color.White;
        }



    }
}
