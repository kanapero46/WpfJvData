using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WpfApp1.Class.com;
using WpfApp1.JvComDbData;

namespace WpfApp1.lib.site
{
    class siteNetKeiba
    {
        //netkeibaと通信する(スクレイピング)クラス
        List<JvFamilyNo> FamilyNuber;

        //netkeibaのスクレイピングに必要な定数・定義
        public const String DEF_NETKEIBA_SITE_URL_HEAD = "https://db.netkeiba.com/horse/ped/";
        public const String DEF_SELECTER = "#contents > div.db_main_deta > diary_snap > table > tbody > tr:nth-child(17) > td.b_fml > br:nth-child(6)";

        //ログ出力
        JvComClass LOG = new JvComClass();
        private const String DEF_SPEC = "SC";

        //JRAで決まっている定義
        private const int DEF_KETTO_LEN = 10;

        //戻り
        private const int RET_SUCCESS = 1;
        private const int RET_FAILED = 0;
        private const int RET_PARAMETER_ERROR = -1;
        private const int RET_CONNECT_ERROR = -2;
        private const int RET_NO_DATA = -2;

        //状態
        private int Stat = 0;
        private int DataCounter = 0;
        private int ResultReason = 0;

        public siteNetKeiba()
        {
            FamilyNuber = new List<JvFamilyNo>();
        }

        //この関数を呼ぶ前にInitScrapingを呼ぶこと！
        public async void GetFamilyNumber(String KettoNumber)
        {
            JvFamilyNo tmpFamilyClass = new JvFamilyNo();

            if (Stat == 0)
            {
                //Initを呼ばなければならに
                return;
            }

            if (KettoNumber.Length != DEF_KETTO_LEN)
            {
                //パラメーターエラー：血統登録番号は10文字固定
                ResultReason = RET_PARAMETER_ERROR;
                goto FAILANY;
            }

            try
            {
                Stat = 99;
                var doc = default(IHtmlDocument);
                using (var client = new HttpClient())
                using (var stream = await client.GetStreamAsync(new Uri(JvFamilyNo.NETKEIBA_SITE_URL_HEAD + KettoNumber + "/")))
                {
                    // AngleSharp.Html.Parser.HtmlParserオブジェクトにHTMLをパースさせる
                    var parser = new HtmlParser();
                    //doc = await parser.ParseAsync(stream); →現在使用不可
                    doc = await parser.ParseDocumentAsync(stream);
                }

                var param = doc.QuerySelector(JvFamilyNo.SELECTER);

                if (param == null)
                {
                    //データがない
                    //netkeibaにデータが存在しないときはエラーを返す。
                    ResultReason = RET_NO_DATA;
                    goto FAILANY;
                }

                //FNo.を探す。
                if (param.ParentElement.InnerHtml.Contains("FNo."))
                {
                    int StrNumber = param.ParentElement.InnerHtml.IndexOf("FNo.");
                    int EndNumber = param.ParentElement.InnerHtml.IndexOf("]", StrNumber) - (StrNumber + 5);


                    String tmpString = param.ParentElement.InnerHtml.Substring(StrNumber + 5, EndNumber);
                    LOG.CONSOLE_TIME_MD(DEF_SPEC, ": Fno:" + tmpString);
                    tmpFamilyClass.KettoNumber1 = KettoNumber;
                    tmpFamilyClass.FamilyNumber1 = tmpString;

                    FamilyNuber.Add(tmpFamilyClass);
                    goto FAILANY;   //成功
                }
                else
                {
                    LOG.CONSOLE_MODULE(DEF_SPEC, "NotConnect...");
                    ResultReason = RET_CONNECT_ERROR;
                    goto FAILANY;
                }


            }
            catch (HttpRequestException)
            {
                LOG.CONSOLE_TIME_MD(DEF_SPEC, "disconnect... DomeinNameServerError db.netkeiba.com");
                ResultReason = RET_CONNECT_ERROR;
                goto FAILANY; //ネットにつながらない場合、即終了。
            }

        FAILANY:
            Stat = 0;   //Initを呼び出すようにする


        }

        //読み込む前に呼ぶ
        public int InitScraping()
        {
            Stat = 1;

            //エラー状態をクリア
            ResultReason = 0;

            try
            {
                DataCounter = FamilyNuber.Count;
            }
            catch (Exception)
            {
                //初期化してない場合
                return 0;
            }

            return 1;
        }


        //実際の処理の後に成功したかを問い合わせる
        public int CheckScrapingStat()
        {
            if(Stat == 99)
            {
                //処理中のため、待機してもらう
                return 99;
            }

            if (DataCounter != FamilyNuber.Count)
            {
                //取得に成功した
                return 1;
            }
            //失敗・増えていない場合はエラーコードを返す。
            return ResultReason;

        }

        public List<JvFamilyNo> GetAllData()
        {
            return FamilyNuber;
        }

        //通常は呼ばなくてよいが、データなし(---)などを設定できるようにする
        public void SetNodata(String KettoNumber, String FamilyNumber)
        {
            JvFamilyNo tmpData = new JvFamilyNo();
            tmpData.FamilyNumber1 = FamilyNumber;
            tmpData.KettoNumber1 = KettoNumber;
            FamilyNuber.Add(tmpData);
        }

        public void GetIndexData(int Index, ref String KettoNumber, ref String FamilyNumber)
        {
            if(Index < 0 || FamilyNuber.Count == 0)
            {
                return;
            }

            FamilyNumber = FamilyNuber[Index].FamilyNumber1;
            KettoNumber = FamilyNuber[Index].KettoNumber1;
        }

        public async Task<JvFamilyNo> GetFamilyNumberToReturn(String KettoNumber)
        {

            JvFamilyNo tmpFamilyClass = new JvFamilyNo();

            try
            {
                var doc = default(IHtmlDocument);
                using (var client = new HttpClient())
                using (var stream = await client.GetStreamAsync(new Uri(JvFamilyNo.NETKEIBA_SITE_URL_HEAD + KettoNumber + "/")))
                {
                    // AngleSharp.Html.Parser.HtmlParserオブジェクトにHTMLをパースさせる
                    var parser = new HtmlParser();
                    //doc = await parser.ParseAsync(stream); →現在使用不可
                    doc = await parser.ParseDocumentAsync(stream);
                }

                var param = doc.QuerySelector(JvFamilyNo.SELECTER);

                if (param == null)
                {
                    //データがない
                    //netkeibaにデータが存在しないときはエラーを返す。
                    ResultReason = RET_NO_DATA;
                    return tmpFamilyClass;
                }

                //FNo.を探す。
                if (param.ParentElement.InnerHtml.Contains("FNo."))
                {
                    int StrNumber = param.ParentElement.InnerHtml.IndexOf("FNo.");
                    int EndNumber = param.ParentElement.InnerHtml.IndexOf("]", StrNumber) - (StrNumber + 5);


                    String tmpString = param.ParentElement.InnerHtml.Substring(StrNumber + 5, EndNumber);
                    LOG.CONSOLE_TIME_MD(DEF_SPEC, ": Fno:" + tmpString);
                    tmpFamilyClass.KettoNumber1 = KettoNumber;
                    tmpFamilyClass.FamilyNumber1 = tmpString;

                    FamilyNuber.Add(tmpFamilyClass);
                    return tmpFamilyClass;   //成功
                }
                else
                {
                    LOG.CONSOLE_MODULE(DEF_SPEC, "NotConnect...");
                    ResultReason = RET_CONNECT_ERROR;
                    return tmpFamilyClass;
                }
            }
            catch (HttpRequestException)
            {
                LOG.CONSOLE_TIME_MD(DEF_SPEC, "disconnect... DomeinNameServerError db.netkeiba.com");
                ResultReason = RET_CONNECT_ERROR;
                return tmpFamilyClass;
            }
        }
    }
}
