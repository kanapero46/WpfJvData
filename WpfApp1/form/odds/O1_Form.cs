using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.JvComDbData;
using static WpfApp1.JvComDbData.JvDbOzData;

namespace WpfApp1.form.odds
{
    public partial class O1_Form : Form
    {
        private String Key;
        dbAccess.dbConnect db = new dbAccess.dbConnect();
        Class.com.JvComClass LOG = new Class.com.JvComClass();
        JvComDbData.JvDbOzAllClass PayAllData = new JvDbOzAllClass();
        int JomeiColor = 0;


        O1Param Param = new O1Param();

        const String MD = "OF";

        const int JV_OF_PAY_CLOSE_PRETIME = 1;  //締め切り前分数

        int MaxTosu = 0;    //最大頭数

        public O1_Form()
        {
            InitializeComponent();
        }

        public O1_Form(String RaKey)
        {
            InitializeComponent();
            Key = RaKey;
        }

        public O1_Form(string RaKey, int color)
        {
            InitializeComponent();
            Key = RaKey;
            JomeiColor = color;
        }

        private void O1_Form_Load(object sender, EventArgs e)
        {
            if (Key == "" || Key == null)
            {
                this.Close();
                MessageBox.Show("パラメーターエラー");
                LOG.CONSOLE_MODULE("O1", "O1 Key Param ERROR!!");
                return;
            }

            InitDataGridView();
            int ret = GetRaceData();

            //カラー処理
            panel1.BackColor = LOG.JvSysJomeiNumtoColor(JomeiColor);
            flowLayoutPanel4.BackColor = LOG.JvSysJomeiNumtoColor(JomeiColor);

            if (ret == 0)
            {
                MessageBox.Show("パラメーターエラー", "RAデータエラー");
                this.Close();
                return;
            }

            if(!GetHorceData())
            {
                MessageBox.Show("パラメーターエラー", "SEデータエラー");
                this.Close();
                return;
            }

            long key = 0;
            if (long.TryParse(Key, out key))
            {
                GetAllOddzData(key);
            }
            else
            {
                MessageBox.Show("パラメーターエラーのため、処理を終了します。", "Keyエラー");
            }

            //全オッズデータ取得
            PayAllData = new JvDbOzAllClass(Key);
            PayAllData.JvDbAllSetALLOddzData();
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int GetRaceData()
        {
            List<String> tmpArray = new List<string>();

            db.TextReader_Col(Key.Substring(0, 8), "RA", 0, ref tmpArray, Key);

            if(tmpArray.Count == 0)
            {
                return 0;
            }

            //レースデータセット
            JvComDbData.JvDbRaData RA = new JvComDbData.JvDbRaData();
            RA.setData(ref tmpArray);

            String tmpStr = RA.getRaceCource();
            Date.Text = RA.ConvertDateToDate(RA.getRaceDate());
            Kaisai.Text = "第" + RA.getRaceKaiji() + "回" + LOG.JvSysMappingFunction( LibJvConv.LibJvConvFuncClass.COURCE_CODE, ref tmpStr) + "競馬" + RA.getRaceNichiji() + "日目";
            label4.Text = RA.ConvertTimeToString(RA.getRaceStartTime());
            weLabel.Text = RA.getWeather();
            TrackDistance.Text = RA.getTrackStatus();

            RaceNum.Text = LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.COURCE_CODE, ref tmpStr);
            label2.Text = Int32.Parse(RA.getRaceNum()) + "R";

            if(RA.getRaceGradeKai() != 0)
            {
                kaiji.Text = "第" + RA.getRaceGradeKai() + "回";
            }

            //レース開催中止
            if (RA.DataKubun1 == "9")
            {
                this.racename.Text = "【中止】 " + RA.getRaceNameFukus() + (RA.getRaceNameFukus().Length >= 1 ? " " : "") + RA.getRaceName() + (RA.getRaceNameEnd() == "" ? "" : "(" + RA.getRaceNameEnd() + ")");
            }
            else
            {
                this.racename.Text = RA.getRaceNameFukus() + (RA.getRaceNameFukus().Length >= 1 ? " " : "") + RA.getRaceName() + (RA.getRaceNameEnd() == "" ? "" : "(" + RA.getRaceNameEnd() + ")");
            }

            this.raceNameEng.Text = " " + RA.getRaceNameEng();


            //2019年6月～クラス名称変更対応
            if (RA.getRaceGrade() == "一般" &&
                (RA.getRaceClass() == "005" || RA.getRaceClass() == "010" || RA.getRaceClass() == "016"))
            {
                tmpStr = RA.getRaceClass();
                racename.Text += "（" + LOG.JvSysMappingFunction(20071, ref tmpStr) + "）";
            }

            if (RA.getRaceGradeKai() != 0 || RA.getRaceGrade() == "Ｌ") //リステッド競走対応
            {
                racename.Text += " （" + RA.getRaceGrade() + "）";
            }

            tmpStr = RA.getOldYear();
            this.OldYear.Text = LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.RACE_SHUBETSU_LONG_CODE, ref tmpStr);

            tmpStr = RA.getCourceTrack();
            int GetKind = (LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.TRACK_CODE_SHORT, ref tmpStr) == "芝" ? 2 : 3);
            TrackLabel.Text = (GetKind == 2 ? "芝" : "ダート");
            TrackLabel.BackColor = (GetKind == 2 ? Color.LightGreen : Color.Tan);

            tmpStr = RA.getRaceClass();
            ClassLabel.Text = LOG.JvSysMappingFunction(2007, ref tmpStr);

            tmpStr = RA.getRaceKindKigo();
            KigoLabel.Text = LOG.JvSysMappingFunction(2006, ref tmpStr);

            tmpStr = RA.getRaceHandCap();
            KigoLabel.Text = LOG.JvSysMappingFunction(2008, ref tmpStr);

            DistanceLabel.Text = RA.getDistance();

            tmpStr = RA.getCourceTrack();
            TrackNameLabel.Text = "（" + LOG.JvSysMappingFunction(LibJvConv.LibJvConvFuncClass.TRACK_CODE, ref tmpStr) + "）";

            //別メソッドで必要なパラメーターを設定
            Param.RaceStartTime1 = RA.getRaceDate() + RA.getRaceStartTime();

            //レース開催中止
            if (RA.DataKubun1 == "9")
            {
                return -1;
            }

            return 1;
        }

        private Boolean GetHorceData()
        {
            List<String> tmpArray = new List<string>();
            int idx = 0;
            const int MAX_FOR_COUNT = 18;
            JvComDbData.JvDbSEData Se = new JvComDbData.JvDbSEData();
            Boolean ret = false;

            for(idx=1; idx <= MAX_FOR_COUNT; idx++)
            {
                tmpArray.Clear();
                db.TextReader_Col(Key.Substring(0, 8), "SE", 0, ref tmpArray, Key + String.Format("{0:00}", idx));

                if (tmpArray.Count == 0)
                {
                    return ret;
                }
                ret = true;

                Se.SetSEData(tmpArray);

                MaxTosu++;  //最大頭数をインクリメント
                dataGridView1.Rows.Add
                    (
                        Se.Waku1,
                        Se.Umaban1,
                        Se.Name1,
                        idx,
                        idx,
                        idx
                    );
            }
            return ret;
        }

        private void InitDataGridView()
        {
            /* フォントの変更 */
            dataGridView1.DefaultCellStyle.Font = new Font("Meiryo UI", 12);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Meiryo UI", 9);

            dataGridView1.RowTemplate.Height = 50;
        }

        #region テーブルボタン変更イベント
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Boolean F, S, T;

            switch(e.ColumnIndex)
            {
                case 3:
                    F = true;
                    S = false;
                    T = false;
                    break;
                case 4:
                    F = false;
                    S = true;
                    T = false;
                    break;
                case 5:
                    F = false;
                    S = false;
                    T = true;
                    break;
                default:
                    //エラー
                    return;
            }

            if (F)
            {
                if(e.RowIndex + 1 == Param.Second1)
                {
                    //すでに他の着順で選択済みの場合は置き換える
                    Param.Second1 = 0;
                }
                if (e.RowIndex + 1 == Param.Thaad1)
                {
                    //すでに他の着順で選択済みの場合は置き換える
                    Param.Thaad1 = 0;
                }

                Param.First1 = e.RowIndex + 1;
            }
            else if (S)
            {
                if (e.RowIndex + 1 == Param.First1)
                {
                    //すでに他の着順で選択済みの場合は置き換える
                    Param.First1 = 0;
                }
                if (e.RowIndex + 1 == Param.Thaad1)
                {
                    //すでに他の着順で選択済みの場合は置き換える
                    Param.Thaad1 = 0;
                }

                Param.Second1 = e.RowIndex + 1;
            }
            else if (T)
            {
                if (e.RowIndex + 1 == Param.First1)
                {
                    //すでに他の着順で選択済みの場合は置き換える
                    Param.First1 = 0;
                }
                if (e.RowIndex + 1 == Param.Second1)
                {
                    //すでに他の着順で選択済みの場合は置き換える
                    Param.Second1 = 0;
                }

                Param.Thaad1 = e.RowIndex + 1;
            }

            //設定した順位を表示
            ReLoadRank();
            ReLoadOddzData();
        }
        #endregion

        private void ReLoadRank()
        {
            if(Param.First1 != 0)
            {
                textFirst.Text = Param.First1.ToString();
            }
            else
            {
                textFirst.Text = "";
            }

            if(Param.Second1 != 0)
            {
                textSecond.Text = Param.Second1.ToString();
            }
            else
            {
                textSecond.Text = "";
            }


            if (Param.Thaad1 != 0)
            {
                textThaad.Text = Param.Thaad1.ToString();
            }
            else
            {
                textThaad.Text = "";
            }

        }

        private void ReLoadOddzData()
        {
            //単勝
            String[,] tmpOddz = new string[4896, 2];
            double oddz1 = 0;
            double Oddz2 = 0;

            if (textFirst.Text != "")
            {
                PayAllData.JvOzAllGetData(ref tmpOddz, 1);
                if(radioButton2.Checked)
                {
                    label14.Text = PayAllData.JvOzAllConvOddzText(tmpOddz[Int32.Parse(textFirst.Text) -1, 0]).ToString();
                }
                else
                {
                    label14.Text = PayAllData.JvOzAllConvOddzText(tmpOddz[Int32.Parse(textFirst.Text) - 1, 0]) * 100 + "円";
                }

                tmpOddz = new string[4896, 2];
                PayAllData.JvOzAllGetData(ref tmpOddz, 2);

                //複勝
                if (radioButton2.Checked)
                {
                    label11.Text =
                        (PayAllData.JvOzWideOddzData(tmpOddz[Int32.Parse(textFirst.Text) - 1, 0], ref oddz1, ref Oddz2) == 1 ?
                         oddz1 + " - " + Oddz2
                    : "***.* - ***.*"
                    );
                }
                else
                {
                    label11.Text =
                    (PayAllData.JvOzWideOddzData(tmpOddz[Int32.Parse(textFirst.Text) - 1, 0], ref oddz1, ref Oddz2) == 1 ?
                     (oddz1 * 100) + "円"
                    : "---円"
                    );
                }

            }

            //複勝：１着の処理がされていない場合は複勝データ取得処理を呼ぶ
            if(textFirst.Text == "" && MaxTosu > 4)
            {
                tmpOddz = new string[4896, 2];
                PayAllData.JvOzAllGetData(ref tmpOddz, 2);
            }

            if (textSecond.Text != "" && MaxTosu > 4)
            {
                if (radioButton2.Checked)
                {
                    label21.Text =
                        (PayAllData.JvOzWideOddzData(tmpOddz[Int32.Parse(textSecond.Text) - 1, 0], ref oddz1, ref Oddz2) == 1 ?
                         oddz1 + " - " + Oddz2
                    : "***.* - ***.*"
                    );
                }
                else
                {
                    label21.Text =
                    (PayAllData.JvOzWideOddzData(tmpOddz[Int32.Parse(textSecond.Text) - 1, 0], ref oddz1, ref Oddz2) == 1 ?
                     (oddz1 * 100) + "円"
                    : "---円"
                    );
                }
            }

            //複勝：１着の処理がされていない場合/３着払いがある場合 は複勝データ取得処理を呼ぶ
            if (textFirst.Text == "" && MaxTosu > 7)
            {
                tmpOddz = new string[4896, 2];
                PayAllData.JvOzAllGetData(ref tmpOddz, 2);
            }

            if (textSecond.Text != "" && MaxTosu > 7)
            {
                if (radioButton2.Checked)
                {
                    label21.Text =
                        (PayAllData.JvOzWideOddzData(tmpOddz[Int32.Parse(textSecond.Text) - 1, 0], ref oddz1, ref Oddz2) == 1 ?
                         oddz1 + " - " + Oddz2
                    : "***.* - ***.*"
                    );
                }
                else
                {
                    label21.Text =
                    (PayAllData.JvOzWideOddzData(tmpOddz[Int32.Parse(textSecond.Text) - 1, 0], ref oddz1, ref Oddz2) == 1 ?
                     (oddz1 * 100) + "円"
                    : "---円"
                    );
                }
            }

            //馬連
            if(textFirst.Text != "" && textSecond.Text != "" && textFirst.Text != textSecond.Text)
            {
                tmpOddz = new string[4896, 2];
                PayAllData.JvOzAllGetData(ref tmpOddz, 4);

                label40.Text = ConvMinMax(tmpOddz[CalcUmarenIndex(Int32.Parse(textFirst.Text), Int32.Parse(textSecond.Text)), 0]);
                if (radioButton2.Checked)
                {
                    label39.Text = PayAllData.JvOzAllConvOddzText(tmpOddz[CalcUmarenIndex(Int32.Parse(textFirst.Text), Int32.Parse(textSecond.Text)), 1]).ToString();
                }
                else
                {
                    label39.Text = PayAllData.JvOzAllConvOddzText(tmpOddz[CalcUmarenIndex(Int32.Parse(textFirst.Text), Int32.Parse(textSecond.Text)), 1]) * 100 + "円";

                }
            } 




        }

        public int GetAllOddzData(long RaceKey)
        {
            JVForm JV = new JVForm();
            int ret = 0;

            JV.JvForm_JvInit();

            ret = JV.JvForm_JvRTOpen("0B30", RaceKey.ToString());

            if(ret != 0)
            {
                switch(ret)
                {
                    case -202:  //JVCloseされていない
                        JV.JvForm_JvClose();
                        ret = JV.JvForm_JvRTOpen("0B30", RaceKey.ToString());
                        break;
                    default:
                        return -1;
                }

                if(ret != 0)
                {
                    return -1;
                }
            }

            String fname = "";
            String timeStamp = "";
            int size = 83285;
            String buff = "";
            ret = 1;
            WpfApp1.JvComDbData.JvDbOzData OzData = new JvComDbData.JvDbOzData();

            while(ret > 0)
            {
                ret = JV.JvForm_JvRead(ref buff, out size, out fname);

                if (buff == "")
                {
                    continue;
                }

                if (ret == -1)
                {
                    //ファイルなし
                    break;
                }

                ret = OzData.JvDbSetOzData(buff.Substring(0, 2), ref buff);

#if DEBUG
                if(ret == 0)
                {
                    LOG.CONSOLE_MODULE("DEBUG", "JvDbSetOzData ret = 0");
                }
#endif

                Param.OddzTime1 = OzData.JvOzDataGetOddzTime(false); //オッズ発表時間を整形済みデータで設定
                Param.PayFlag2 = OzData.JvOzDataGetPayInfo();
            }

            ret = OzData.JvOzDbWriteDbData();
#if DEBUG
            if (ret == 0)
            {
                LOG.CONSOLE_MODULE("DEBUG", "JvOzDbWriteDbData ret = " + ret);
            }
#endif
            return ret;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //タイム計測のため、時間ありログ
            LOG.CONSOLE_TIME_MD(MD, "< AllOddzData! Start!!!");
            GetAllOddzData(long.Parse(this.Key));

            OddzTimeCalc();
        }

        private int OddzTimeCalc()
        {
            String PayStatusInfoText = "";
            Color clr = new Color();
            int PayCloseMinutes = 0;

            LOG.CONSOLE_MODULE(MD, ">> PayStatus > " + Param.PayFlag2.PayStatus.ToString());

            switch(Param.PayFlag2.PayStatus)
            {
                case 2:
                    PayStatusInfoText = "前日最終";
                    clr = Color.Silver;
                    break;
                case 3:
                    PayStatusInfoText = "＊";
                    clr = Color.Red;
                    break;
                case 4:
                    PayStatusInfoText = "最終";
                    clr = Color.Silver;
                    break;
                case 5:
                    panel5.Visible = false;
                    break;
                case 9:
                    PayStatusInfoText = "中止";
                    clr = Color.Red;
                    break;
            }

            if (PayStatusInfoText == "")
            {
                　　  DateTime dt1 = new DateTime(Int32.Parse(Param.RaceStartTime1.Substring(0, 4)), Int32.Parse(Param.OddzTime1.Substring(0, 2)), Int32.Parse(Param.OddzTime1.Substring(2, 2)), Int32.Parse(Param.OddzTime1.Substring(4, 2)),
                    Int32.Parse(Param.OddzTime1.Substring(6, 2)), 0);

                if (Param.RaceStartTime1 == "")
                {
                    return -999;
                }

                //発走時間を取得
                DateTime dt2 = new DateTime(Int32.Parse(Param.RaceStartTime1.Substring(0, 4)),
                                            Int32.Parse(Param.RaceStartTime1.Substring(4, 2)),
                                            Int32.Parse(Param.RaceStartTime1.Substring(6, 2)),
                                            Int32.Parse(Param.RaceStartTime1.Substring(8, 2)),
                                            Int32.Parse(Param.RaceStartTime1.Substring(10, 2)),
                                            0);

                //締め切り時間を計算
                TimeSpan ts1 = new TimeSpan(0, JV_OF_PAY_CLOSE_PRETIME, 0);

                //締め切り時間を上書き
                DateTime dt3 = dt2 - ts1;

                //残り分数を計算
                DateTime dt4 = DateTime.Now;
                ts1 = dt3 - dt4;

                Console.WriteLine(ts1.TotalMinutes);

                PayCloseMinutes = (int)ts1.TotalMinutes;
                if(PayCloseMinutes >= 60)
                {
                    //レース番号を入れておく
                    label10.Text = label2.Text;
                }
                else if(PayCloseMinutes <= 59 && 5 < PayCloseMinutes)
                {
                    label10.Text = PayCloseMinutes + "分前";
                    panel5.BackColor = Color.Yellow;
                    label10.ForeColor = Color.Black;
                    WritePayCloseTimeText();
                }
                else if (PayCloseMinutes <= 5 && 0 < PayCloseMinutes)
                {
                    label10.Text = PayCloseMinutes + "分前";
                    panel5.BackColor = Color.Red;
                    label10.ForeColor = Color.White;
                    WritePayCloseTimeText();
                }
                else
                {
                    label10.Text = "＊";
                    panel5.BackColor = Color.Red;
                    label10.ForeColor = Color.White;
                    WritePayCloseTimeText();
                }

                return PayCloseMinutes;
            }
            else
            {
                label10.Text = PayStatusInfoText;
                panel5.BackColor = clr;

                //フォント色
                if (clr == Color.Red)
                {
                    label10.ForeColor = Color.White;
                }
                else if(clr == Color.Silver)
                {
                    label10.ForeColor = Color.Black;
                }
            }
            return 0;
        }

        private void WritePayCloseTimeText()
        {
            oddzTimeStatus.Visible = true;
            oddzTimeStatus.Text = "オッズ発表時間：" +
                                  Param.OddzTime1.Substring(0, 2) + "月" +
                                  Param.OddzTime1.Substring(2, 2) + "日 " +
                                  Param.OddzTime1.Substring(4, 2) + "時 " +
                                  Param.OddzTime1.Substring(5, 2) + "分現在";
        }

        private void Label11_Click(object sender, EventArgs e)
        {

        }

        private int CalcUmarenIndex(int min, int max)
        {
            //minの-1まではスキップする　14縦　5-7 14 * 4
            int Min = 0;
            Min = JudgeMinNumber(min, max);
            if(Min == 0)
            {
                return 0;
            }


            int tmpMinUnderNum = (MaxTosu - 2) * (Min - 1);

            //配列添字分(+1)/同番配慮分(+1)
            tmpMinUnderNum = (tmpMinUnderNum) + ((Min == min ? max : min) - 2);
            LOG.CONSOLW_DEBUG(tmpMinUnderNum.ToString());
            //tmpMinUnderNum + 1からが軸馬の配列添字
            return tmpMinUnderNum; 
        }

        private String ConvMinMax(String Param)
        {
            return ConvMinMax(Param.Substring(0, 2), Param.Substring(2, 2));
        }

        private String ConvMinMax(String Param1, String Param2)
        {
            int Num1 = 0;
            int Num2 = 0;
            int ret = 0;

            if (Int32.TryParse(Param1, out Num1) && Int32.TryParse(Param2, out Num2))
            {
                ret = Math.Min(Num1, Num2);
                if (ret == Num1)
                {
                    return (Int32.Parse(Param1) + " - " + Int32.Parse(Param2)).ToString();
                }
                else
                {
                    return (Int32.Parse(Param2) + " - " + Int32.Parse(Param1)).ToString();
                }
            }
            return "";
        }

        //小さい値を返す
        private int JudgeMinNumber(int num1, int num2)
        {
            try
            {
                return Math.Min(num1, num2);
            }
            catch
            {
                return 0;
            }
        }
    }

    class O1Param
    {
        private int First;
        private int Second;
        private int Thaad;

        private String RaceStartTime;
        private String OddzTime;

        private PAY_FLAG PayFlag;

        public int First1 { get => First; set => First = value; }
        public int Second1 { get => Second; set => Second = value; }
        public int Thaad1 { get => Thaad; set => Thaad = value; }
        public string OddzTime1 { get => OddzTime; set => OddzTime = value; }
        public string RaceStartTime1 { get => RaceStartTime; set => RaceStartTime = value; }
        internal PAY_FLAG PayFlag2 { get => PayFlag; set => PayFlag = value; }
    }
}
