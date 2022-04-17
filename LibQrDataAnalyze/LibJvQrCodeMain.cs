using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**
 * 勝馬投票券のQRコード解析ライブラリ
 * @date:2020/02/16
 * @version
 * v00.01-00 新規作成
 * 
 * */

namespace LibQrDataAnalyze
{
   
    public class JvQrAnaDetailsClass
    {
        public int Kumiban1;
        public int Kumiban2;
        public int Kumiban3;
    }

    public class JvQrAnaBetKindClass
    {
        public bool FormationFlg;
        public bool Head1AsixFlg;
        public bool Head2AsixFlg;

    }

    public class JvQrAnalyzeClass
    {
        public int Key;
        public JvQrAnaBetKindClass jvQrAnaBet; 
        public List<int> BetKind;     /* 馬券種類 */
        public List<JvQrAnaDetailsClass> Kumiban;   /* 組番 */
        public List<int> Price;     /* 金額 */
    }



    public class JvQrMain
    {
        /* 共通定義 */
        const String LIB_QR_VERSION = "v00.03-00";
        /** バージョン履歴
         * v00.03-00：3連複ながし対応
         * v00.02-00：馬単ながし・フォーメーション・マルチ対応
         * 
         * */

        /* 馬券定義 */
        const int REGULAR = 0;
        const int BOX = 1;
        const int WHELL = 2;
        const int FORMATION = 3;

        const int QrDataLen = 95;

        public static String JvQrGetLibVersion()
        {
            return LIB_QR_VERSION;
        }

        public static int HOGEHOGELIB()
        {
            return 0;
        }

        public static int JvQrAnlyzeMain( String inParam1, String inParam2)
        {
            //Console.WriteLine(inParam1 + "\n" + inParam2);
            return 1;
        }

        public static int JvQrAnalyzeMain2( String inParam1, String inParam2, ref JvQrAnalyzeClass outParam )
        {
            String ConvData = "";
            bool HeaderData1Enable = false;

            LGCO("JvQrAnalyzeMain!!");
            //2つのQRコードが必要
            if (inParam1 == null || inParam2 == null || inParam1 == "" || inParam2 == "")
            {
                LGCO("Invalid Paramter1 Error!!");
                return 0;
            }

            //データが規定範囲内
            if(inParam1.Length != QrDataLen || inParam2.Length != QrDataLen)
            {
                LGCO("[LIB_QR] Invalid Paramter2 Error!!");
                return 0;   //エラー
            }

            //data1 と data2のどちらが先頭データかを判断する。
            //data1が先頭データならtrueを返す。
            HeaderData1Enable = JugdeHeaderData1(inParam1);

            //outParamの配列を1で初期化
            outParam.BetKind = new List<int>();
            outParam.Kumiban = new List<JvQrAnaDetailsClass>();
            outParam.Price = new List<int>();

            //ヘッダーデータ(場名レース番号など)をセット
            if (HeaderData1Enable)
            {
                //Data1にヘッダーあり
                SetHeaderData(inParam1, ref outParam);

                //馬券データのセット
                ConvData = inParam1 + inParam2;

            }
            else
            {
                SetHeaderData(inParam2, ref outParam);

                ConvData = inParam2 + inParam1;
            }


            //先頭5をチェックする
            if(Int32.Parse(ConvData.Substring(0,1)) != 5)
            {
                //基本マークシート
                //3連系・応援馬券以外はこのif文に入る
                //馬券種チェック
                switch (Int32.Parse(ConvData.Substring(42, 1)))
                {
                    case 1:
                    case 2:
                        //単勝・複勝基本マークシートのみ
                        SetData_Common(ConvData, false, ref outParam);
                        break;
                }


            }

            //5以外の場合

            //馬券の種類から処理を分ける。
            switch (Int32.Parse(ConvData.Substring(14, 1)))
            {
                case REGULAR:
                    //基本マークシート
                    SetData_Common(ConvData, false, ref outParam);
                    break;
                case BOX:
                    break;
                case WHELL:
                    break;
                case FORMATION:
                    break;
            }


            //馬単(6)
            if(Int32.Parse(ConvData.Substring(0,1)) == 1)
            {

            }
            switch(Int32.Parse(ConvData.Substring(42, 1)))
            {
                case 6: //馬単
                    SetData_6(ConvData, ref outParam);
                    break;
                case 7: //ワイド
                    break;  
                case 8:
                    //3連複フォーメーション用処理
                    if (Int32.Parse(ConvData.Substring(43, 1)) == 0)
                    {
                        SetData_3Formation(ConvData, false, ref outParam);
                    }
                    else
                    {
                        //ながし
                        SetData_3Whell(ConvData, false, ref outParam);
                    }
                    break;
                case 9:
                    //3連単用処理
                    if (Int32.Parse(ConvData.Substring(43, 1)) == 0)
                    {
                        SetData_3Formation(ConvData, true, ref outParam);
                    }
                    else
                    {
                        //ながし
                        SetData_3Whell(ConvData, true, ref outParam);
                    }
                    break;
            }
   
            return 1;
        }

        //data1にヘッダーデータが入っているかをチェックする
        private static bool JugdeHeaderData1(String Data1)
        {
            //エラーチェックは省略
            if (Data1.Contains("0123456789"))
            {
                //123・・・が含まれている方がData2と判断する。
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void SetBakenData(String Header, String Data, ref JvQrAnalyzeClass outParam)
        {
            //馬券種判定
            //outParam.BetKind = Int32.Parse(Header.Substring(41, 1));
            
            switch(outParam.BetKind)
            {

            }
        }

        private static void SetHeaderData(String HeaderData, ref JvQrAnalyzeClass outParam)
        {
            //キーのセット：20XX年形式
            outParam.Key = 2000;
            outParam.Key += Int32.Parse(HeaderData.Substring(6, 2));

            //キーのセット：日付はないため、スキップ
            //回次・場名・日次・レース番号をセット
            outParam.Key = outParam.Key * 100000000; //8桁分右にずらす
            outParam.Key += Int32.Parse(HeaderData.Substring(8, 8));
        }

        #region 馬単ながし・フォーメーション対応
        //馬単
        private static void SetData_6(String Data, ref JvQrAnalyzeClass outParam)
        {
            List<JvQrAnaDetailsClass> tmpOutParam = new List<JvQrAnaDetailsClass>();
            JvQrAnaDetailsClass tmpAnaDetails = new JvQrAnaDetailsClass();

            List<SetParam> tmpArrayKumiban = new List<SetParam>(); //リスト型の組番退避用配列
            SetParam tmpKumiban = new SetParam(); //リスト型の組番退避用配列
            //tmpArrayKumiban.Kind1 = Int32.Parse( Data.Substring(42, 1));   //式別


            bool[] JIKU = new bool[18];
            bool[] AITE = new bool[18];
            bool Multi = (Data.Substring(43, 1) == "1" ? true : false); //マルチ

            int i = 0;
            int j = 0;

            for (i = 0; i < 18; i++)
            {
                JIKU[i] = Int32.Parse(Data.Substring(44 + i, 1)) == 1 ? true : false;
                AITE[i] = Int32.Parse(Data.Substring(80 + i, 1)) == 1 ? true : false;
            }

            for(i = 0; i<18; i++)
            {
                if(JIKU[i])
                {
                    //軸がtrue

                    for(j = 0; j<18; j++)
                    {
                        if(AITE[j])
                        {
                            //馬単該当
                            if(i + 1 == j +1)
                            {
                                //基本はありえない(マークシートでエラー)
                                continue;
                            }

                            Console.WriteLine(i + "-" + j);

                            tmpKumiban = JvQrNewSetParam(i + 1, j + 1, 0);
                            tmpKumiban.Price1 = Int32.Parse(Data.Substring(97, 6));
                            tmpKumiban.Kind1 = Int32.Parse(Data.Substring(42, 1));
                            //保存用に追加する
                            tmpArrayKumiban.Add(tmpKumiban);

                            tmpAnaDetails = new JvQrAnaDetailsClass();
                            tmpAnaDetails.Kumiban1 = i + 1;
                            tmpAnaDetails.Kumiban2 = j + 1;
                            tmpAnaDetails.Kumiban3 = 0;
                            tmpOutParam.Add(tmpAnaDetails);

                            //マルチの場合は、裏返す
                            if (Multi)
                            {
                                tmpKumiban = JvQrNewSetParam(j + 1, i + 1, 0);
                                tmpKumiban.Price1 = Int32.Parse(Data.Substring(97, 6));
                                tmpKumiban.Kind1 = Int32.Parse(Data.Substring(42, 1));

                                //保存用に追加する
                                tmpArrayKumiban.Add(tmpKumiban);

                                tmpAnaDetails = new JvQrAnaDetailsClass();
                                tmpAnaDetails.Kumiban1 = j + 1;
                                tmpAnaDetails.Kumiban2 = i + 1;
                                tmpAnaDetails.Kumiban3 = 0;
                                tmpOutParam.Add(tmpAnaDetails);
                            }

                            continue;
                        }
                    }

                }
            }


            //単式馬券はそのまま入れる            
            for (int n = 0; n < tmpArrayKumiban.Count; n++)
            {
                outParam.Price.Add(tmpArrayKumiban[n].Price1);
                outParam.BetKind.Add(tmpArrayKumiban[n].Kind1);
            }

            //組み合わせは最後に入れる
            outParam.Kumiban = tmpOutParam;
        }
        #endregion

        #region 3連複・3連単フォーメーション
        //3連複フォーメーション
        private static void SetData_3Formation(String Data, Boolean TrifectaFlg, ref JvQrAnalyzeClass outParam)
        {
            //軸馬1検索
            bool[] JIKU1 = new bool[18];
            bool[] JIKU2 = new bool[18]; //軸2頭ながし・相手
            bool[] AITE = new bool[18];  //

            List<SetParam> tmpArrayKumiban = new List<SetParam>(); //リスト型の組番退避用配列
            SetParam tmpKumiban = new SetParam();
            JvQrAnaBetKindClass BetkindDetails = new JvQrAnaBetKindClass();


            int i = 0;
            int j = 0;
            int n = 0;

            for (i = 0; i < 18; i++)
            {
                JIKU1[i] = Int32.Parse(Data.Substring(44 + i, 1)) == 1 ? true : false;
                JIKU2[i] = Int32.Parse(Data.Substring(62 + i, 1)) == 1 ? true : false;
                AITE[i]  = Int32.Parse(Data.Substring(80 + i, 1)) == 1 ? true : false;
            }

            //組み合わせ計算
            //先に3連単をつくる
            for (j = 0; j<18; j++)
            {

                if (JIKU1[j])
                {
                    //インスタンスを初期化する
                    tmpKumiban = new SetParam();

                    //配列添字に＋１した値(馬番)を入れる
                    //tmpKumiban.Kumiban11 = j + 1;

                    for (i = 0; i < 18; i++)
                    {
                        if (JIKU2[i])
                        {
                            if(j + 1 == i + 1)
                            {
                                //同じ組番はスキップ
                                continue;
                            }

                            //次ステップ
                            for (n = 0; n < 18; n++)
                            {
                                if (AITE[n])
                                {
                                    //組番と金額をとりあえずいれておく
                                    //フォーメーションだから、金額は全額一緒？
                                    tmpKumiban = JvQrNewSetParam(j+1, i+1, n+1);
                                    tmpKumiban.Price1 = Int32.Parse(Data.Substring(97, 6));
                                    tmpKumiban.Kind1 = Int32.Parse(Data.Substring(42, 1));

                                    if (j+ 1 == n + 1 || i+1 == n+1)
                                    {
                                        continue;
                                    }

                                    //保存用に追加する
                                    tmpArrayKumiban.Add(tmpKumiban);
                                    continue;
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                    }
                }
            }



            //1点あたりの金額
            //            outParam.Price = Int32.Parse(Data.Substring(94, 6));

            //3連複は小→大の順番にする処理
            if(!TrifectaFlg)
            {
                SetCom3renPuku(ref tmpArrayKumiban, ref outParam);
            }

            //馬券購入方法を入れる。(フォーメーション)
            outParam.jvQrAnaBet = new JvQrAnaBetKindClass();
            outParam.jvQrAnaBet.FormationFlg = true;
        }
        #endregion

        #region 3連複ながし
        //3連複ながし
        private static void SetData_3Whell(String Data, Boolean TrifectaFlg, ref JvQrAnalyzeClass outParam)
        {
            //軸馬1検索
            bool[] JIKU1 = new bool[18];
            bool[] JIKU2 = new bool[18]; //軸2頭ながし・相手
            bool[] AITE = new bool[18];  //

            List<SetParam> tmpArrayKumiban = new List<SetParam>(); //リスト型の組番退避用配列
            SetParam tmpKumiban = new SetParam();
            JvQrAnaBetKindClass BetkindDetails = new JvQrAnaBetKindClass();

            bool Whell2Flg = false; //2頭軸フラグ

            int i = 0;
            int j = 0;
            int n = 0;

            int Umaban = 0;

            for (i = 0; i < 18; i++)
            {
                JIKU1[i] = Int32.Parse(Data.Substring(44 + i, 1)) == 1 ? true : false;
                JIKU2[i] = Int32.Parse(Data.Substring(62 + i, 1)) == 1 ? true : false;
                Whell2Flg = JIKU2[i] || Whell2Flg ? true : false;                       //2頭軸をチェックする
                AITE[i] = Int32.Parse(Data.Substring(80 + i, 1)) == 1 ? true : false;
            }

                for(i = 0; i< 18; i++)
                {
                    if(JIKU1[i])
                    {
                        tmpKumiban = new SetParam();

                        if(!Whell2Flg)
                        {
                            //軸1頭なら相手から2頭をもってくる
                            for(j = 0; j < 18; j++)
                            {
                                if(!AITE[j])
                                {
                                    //相手馬番にいない場合はスキップ
                                    continue;
                                }

                                //
                                for (n = 0; n < 18; n++)
                                {
                                    if (AITE[j] != AITE[n] || !AITE[n] )
                                    {
                                        continue;
                                    }

                                    //組番と金額をとりあえずいれておく
                                    //フォーメーションだから、金額は全額一緒？
                                    tmpKumiban = JvQrNewSetParam(i + 1, j + 1, n + 1);
                                    tmpKumiban.Price1 = Int32.Parse(Data.Substring(97, 6));
                                    tmpKumiban.Kind1 = Int32.Parse(Data.Substring(42, 1));

                                    if (j + 1 == n + 1 || i + 1 == n + 1)
                                    {
                                        continue;
                                    }

                                    //保存用に追加する
                                    tmpArrayKumiban.Add(tmpKumiban);
                                }
                            }
                        }
                        else
                        {
                            //軸2頭ながし
                            for(j = 0; j<18; j++)
                            {
                                if(JIKU2[j])
                                {
                                    Umaban = j + 1;
                                    break;
                                }
                            }

                            for(n = 0; n<18; n++)
                            {
                                //相手との組み合わせ
                                if (Umaban == n+1 || !AITE[n] )
                                {
                                    //基本はマークシートでエラーとなる
                                    continue;
                                }

                                //組番と金額をとりあえずいれておく
                                //フォーメーションだから、金額は全額一緒？
                                tmpKumiban = JvQrNewSetParam(i + 1, Umaban, n + 1);
                                tmpKumiban.Price1 = Int32.Parse(Data.Substring(97, 6));
                                tmpKumiban.Kind1 = Int32.Parse(Data.Substring(42, 1));

                                if (Umaban == n + 1 || i + 1 == Umaban)
                                {
                                    continue;
                                }

                                //保存用に追加する
                                tmpArrayKumiban.Add(tmpKumiban);

                            }

                        }

                }
            }


            //3連複は小→大の順番にする処理
            if(!TrifectaFlg)
            {
                SetCom3renPuku(ref tmpArrayKumiban, ref outParam);
            }
            

            outParam.jvQrAnaBet = new JvQrAnaBetKindClass();
            //馬券購入方法を入れる。(フォーメーション)
            if (Whell2Flg)
            {
                outParam.jvQrAnaBet.Head2AsixFlg = true;
            }
            else
            {
                outParam.jvQrAnaBet.Head1AsixFlg = true;
            }
        }
        #endregion

        #region 共通マークシート
        private static void SetData_Common( String Data, Boolean fTrip ,ref JvQrAnalyzeClass outParam )
        {
            List<SetParam> tmpArrayKumiban = new List<SetParam>(); //リスト型の組番退避用配列
            SetParam tmpKumiban = new SetParam();

            //先に3連系・応援馬券の有無をチェックする(馬券データのフォーマットが違うため)
            if(!fTrip)
            {
                if(Int32.Parse(Data.Substring(0, 1)) == 5)
                {
                    //再帰関数
                    SetData_Common(Data, true, ref outParam);
                }
            }

            //共通マークシート(みどり)で購入可能は8通りまで、ループは8回までとする
            for (int i=0; i<8; i++)
            {
                switch(Int32.Parse(Data.Substring(43, 1)))
                {
                    //ただ未確定・3連系馬券があるとフォーマットが異なる
                    case 1://単勝
                    case 2://複勝
                    case 3://枠連
                    //4はなし？
                    case 5://馬連
                    case 6://馬単
                    case 7://ワイド
                        //再帰関数としてこの関数をもう1度呼ぶ
                        if (!fTrip)
                        {
                            tmpKumiban = SetData_Common_Param1(i, Data);
                        }
                        else
                        {
                            //3連系処理を行う
                            tmpKumiban = SetData_Common_Param2(i, Data);
                        }
                        break;
                    case 8://3連複
                    case 9://3連単
                        //再帰関数としてこの関数をもう1度呼ぶ
                        if(!fTrip)
                        {
                            //falseの場合は、3連系フォーマットとして読み込む必要があるため、
                            //もう1度再帰関数として呼ぶ
                            SetData_Common(Data, true, ref outParam);
                        }
                        else
                        {
                            //3連系処理を行う
                            tmpKumiban = SetData_Common_Param2(i, Data);
                        }
                        break;
                    default:
                        LGCO("Asertion!! DataError!\nret>Throw FormatException");
                        throw new FormatException();
                }
                tmpArrayKumiban.Add(tmpKumiban);
            }

            JvQrAnaDetailsClass tmpDetail = new JvQrAnaDetailsClass();

            for (int j=0; j<tmpArrayKumiban.Count; j++)
            {
                tmpDetail.Kumiban1 = tmpArrayKumiban[j].Kumiban11;
                tmpDetail.Kumiban2 = tmpArrayKumiban[j].Kumiban21;
                tmpDetail.Kumiban3 = tmpArrayKumiban[j].Kumiban31;

                outParam.BetKind.Add(tmpArrayKumiban[j].Kind1);
                outParam.Kumiban.Add(tmpDetail);
                outParam.Price.Add(tmpArrayKumiban[j].Price1);
            }
        }

        //基本マークシートの設定関数(小)
        private static SetParam SetData_Common_Param1( int idx, String Data )
        {
            SetParam tmpKumiban = new SetParam();

            int i = (idx * 11) + 43;
            tmpKumiban.Kind1 = Int32.Parse(Data.Substring(i, 1));
            tmpKumiban.Kumiban11 = Int32.Parse(Data.Substring(i + 2, 2));
            tmpKumiban.Kumiban21 = Int32.Parse(Data.Substring(i + 4, 2));
            tmpKumiban.Price1 = Int32.Parse(Data.Substring(i + 6, 6));
            return tmpKumiban;
        }

        //基本マークシートの設定関数(大)
        private static SetParam SetData_Common_Param2(int idx, String Data)
        {
            SetParam tmpKumiban = new SetParam();
            int i = (idx * 13) + 43;
            tmpKumiban.Kind1 = Int32.Parse(Data.Substring(i, 1));
            tmpKumiban.Kumiban11 = Int32.Parse(Data.Substring(i + 2, 2));
            tmpKumiban.Kumiban21 = Int32.Parse(Data.Substring(i + 4, 2));
            tmpKumiban.Kumiban31 = Int32.Parse(Data.Substring(i + 6, 2));
            tmpKumiban.Price1 = Int32.Parse(Data.Substring(i+8, 6));
            return tmpKumiban;
        }

        #endregion



        private static void SetCom3renPuku(ref List<SetParam> inParam, ref JvQrAnalyzeClass outParam)
        {
            JvQrAnaDetailsClass tmpKumiDetails = new JvQrAnaDetailsClass();
            SetParam tmpParam = new SetParam();
            int min = 0;
            int max = 0;

            if (inParam == null || inParam.Count == 0)
            {
                return;
            }

            for(int i=0; i<inParam.Count; i++)
            {
                tmpParam = inParam[i];
                tmpKumiDetails = new JvQrAnaDetailsClass();

                if (JvQrChkFlg(ref tmpParam))
                {
                    if (inParam[i].Kumiban31 == 0)
                    {
                        //2連勝馬券はここで終了
                        tmpKumiDetails.Kumiban1 = inParam[i].Kumiban11;
                        tmpKumiDetails.Kumiban2 = inParam[i].Kumiban21;
                    }
                    else
                    {
                        //3連系馬券は3つ目もチェック
                        tmpParam = inParam[i];
                        if (JvQrChkFlg(ref tmpParam))
                        {
                            //正常
                            tmpKumiDetails.Kumiban1 = inParam[i].Kumiban11;
                            tmpKumiDetails.Kumiban2 = inParam[i].Kumiban21;
                            tmpKumiDetails.Kumiban3 = inParam[i].Kumiban31;
                        }
                    }
                    //重複していないかをチェックする
                    if (JvQrArrayAdd(tmpKumiDetails, ref outParam))
                    {
                        outParam.BetKind.Add(inParam[i].Kind1);
                        outParam.Kumiban.Add(tmpKumiDetails);
                        outParam.Price.Add(inParam[i].Price1);
                    }
                }
                else
                {
                    //2連系の場合は、入れ替える
                    if(inParam[i].Kumiban31 == 0)
                    {
                        //2連勝馬券はここで終了
                        tmpKumiDetails.Kumiban1 = inParam[i].Kumiban21;
                        tmpKumiDetails.Kumiban2 = inParam[i].Kumiban11;
                    }
                    else
                    {
                        //3連系の場合は、3つ目もチェック
                        //3連系馬券は3つ目もチェック
                        tmpParam = inParam[i];
                        if(JvQrChkFlg(ref tmpParam))
                        {
                            //正常
                            tmpKumiDetails.Kumiban1 = inParam[i].Kumiban11;
                            tmpKumiDetails.Kumiban2 = inParam[i].Kumiban21;
                            tmpKumiDetails.Kumiban3 = inParam[i].Kumiban31;
                        }
                        else
                        {
                            //3連複の順番がばらばら
                            //最小値と最大値をもらう
                            JvQrChkMinMax(ref min, ref max, ref tmpParam);
                            tmpKumiDetails.Kumiban1 = min;
                            tmpKumiDetails.Kumiban3 = max;
                            if (min != inParam[i].Kumiban11 && max != inParam[i].Kumiban11) tmpKumiDetails.Kumiban2 = inParam[i].Kumiban11;
                            else if (min != inParam[i].Kumiban21 && max != inParam[i].Kumiban21) tmpKumiDetails.Kumiban2 = inParam[i].Kumiban21;
                            else if (min != inParam[i].Kumiban31 && max != inParam[i].Kumiban31) tmpKumiDetails.Kumiban2 = inParam[i].Kumiban31;
                        }
                    }

                    //重複していないかをチェックする
                    if (JvQrArrayAdd(tmpKumiDetails, ref outParam))
                    {
                        outParam.BetKind.Add(inParam[i].Kind1);
                        outParam.Kumiban.Add(tmpKumiDetails);
                    }
                }
            }
        }

        private static void JvQrChkMinMax(ref int min, ref int max, ref SetParam Param)
        {
            min = Math.Min(Param.Kumiban11, Param.Kumiban21);
            if(Param.Kumiban31 == 0)
            {
                //2連系
                max = (min == Param.Kumiban11 ? min : Param.Kumiban21);
                return;
            }
            else
            {
                //3連系
                min = Math.Min(min, Param.Kumiban31);
                max = Math.Max(Param.Kumiban11, Param.Kumiban21);
                max = Math.Max(max, Param.Kumiban31);
                return;
            }
        }
        
        //共通ながし
        private void SetData_Param_Whell( String Data,　ref JvQrAnalyzeClass outParam)
        {
            JvQrAnalyzeClass tmpOutParam = new JvQrAnalyzeClass();

            //馬券種毎に振り分け
            switch(Int32.Parse(Data.Substring(43, 1)))
            {
                case 1:
                case 2:
                    //単勝・複勝のながしはありえないため、アサート
                    LGCO("Assert!!! SetData_Param_Whell() ParamException!!");
                    return;
                case 3://枠連
                //4はなし？
                case 5://馬連
                case 6://馬単

                    break;
                case 7://ワイド
                case 8://3連複
                case 9://3連単
                    break;
            }


        }

        

        private static Boolean JvQrChkFlg(ref SetParam Param)
        {
            if(Param.Kumiban31 == 0)
            {
                if(Param.Kumiban11 < Param.Kumiban21)
                {
                    return true;
                }
            }
            else if(Param.Kumiban11 < Param.Kumiban21 && Param.Kumiban21 < Param.Kumiban31 && Param.Kumiban11 < Param.Kumiban31)
            {
                return true;
            }
            return false;
        }

        private static SetParam JvQrNewSetParam(int Param1, int Param2, int Param3)
        {
            return new SetParam(Param1, Param2, Param3);
        }

        //重複チェック
        private static Boolean JvQrArrayAdd(JvQrAnaDetailsClass Param, ref JvQrAnalyzeClass outParam)
        {
            if(outParam.Kumiban.Count == 0)
            {
                //outParamが0の場合は、trueを返す。
                return true;
            }
            
            //outParamとデータが重複していないかをチェックする
            for(int i=0; i<outParam.Kumiban.Count; i++)
            {
                if(outParam.Kumiban[i].Kumiban3 == 0)
                {
                    if (outParam.Kumiban[i].Kumiban1 == Param.Kumiban1 &&
                   outParam.Kumiban[i].Kumiban2 == Param.Kumiban2
                    )
                    {
                        //2連系の重複チェック
                        return false;
                    }
                }
                else
                {
                    if (outParam.Kumiban[i].Kumiban1 == Param.Kumiban1 &&
                   outParam.Kumiban[i].Kumiban2 == Param.Kumiban2 &&
                   outParam.Kumiban[i].Kumiban3 == Param.Kumiban3
                    )
                    {
                        //重複あり
                        return false;
                    }
                }
            }

            return true;
        }

        private static void LGCO(String log)
        {
            Console.WriteLine("[QRL]\t" + log);
        }
    }

    public class SetParam
    {
        private int Kind;
        private int Kumiban1;
        private int Kumiban2;
        private int Kumiban3;
        private int Price;

        public int Kumiban11 { get => Kumiban1; set => Kumiban1 = value; }
        public int Kumiban21 { get => Kumiban2; set => Kumiban2 = value; }
        public int Kumiban31 { get => Kumiban3; set => Kumiban3 = value; }
        public int Price1 { get => Price; set => Price = value; }
        public int Kind1 { get => Kind; set => Kind = value; }

        public SetParam() { }
        public SetParam(int Param1, int Param2, int Param3)
        {
            Kumiban1 = Param1;
            Kumiban2 = Param2;
            Kumiban3 = Param3;
        }
    }
}
