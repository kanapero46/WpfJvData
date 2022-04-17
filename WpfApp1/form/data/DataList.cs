using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.dbCom1;
using WpfApp1.lib.site;
using WpfApp1.lib.extSoft;

namespace WpfApp1.form.data
{
    public partial class DataList : Form
    {

        List<String> Data = new List<string>();
        JvComDbData.JvFamilyNoData FnoData;
        dbCom ComMain = new dbCom();

        Class.com.JvComClass LOG = new Class.com.JvComClass();

        //文字列
        Class.com.prog_def STR = new Class.com.prog_def();

        //TARGET用インスタンス
        LibTargetClass libTarget = new LibTargetClass();

        //ファミリーナンバー保持クラス
        siteNetKeiba NK = new siteNetKeiba();

        //ファミリーナンバー描画済みフラグ
        private Boolean InitFlg = false;

        //ファミリーナンバー手動書き込みフラグ
        private Boolean FnManualChangeFlg = false;

        //パスなし起動
        public DataList()
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.Font = new Font("Meiryo UI", 12);
        }


        //データ読み込みあり起動
        public DataList(String Path)
        {
            InitializeComponent();
            dataGridView1.DefaultCellStyle.Font = new Font("Meiryo UI", 12);
            InitDataList(Path);
        }

        //Pathはrootから
        private void InitDataList(String Path)
        {
            if(Path == "")
            {
                return;
            }

            dbAccess.dbConnect db = new dbAccess.dbConnect();
            db.DbReadAllData(Path, ref Data);
            //データチェック
            if(Data.Count == 0)
            {
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DataList_Load(object sender, EventArgs e)
        {
            //インスタンスしたときにデータがあるかどうかをチェックする。
            if(Data.Count == 0)
            {
                //データなし、Path指定なし、
                textBox1.Text = "(データ指定なし)";
                return;
            }

            //データと列が同じかチェック
            if(dataGridView1.Columns.Count == Data.Count)
            {

            }
            else
            {
                MessageBox.Show("データフォーマットが不整合です。","フォーマットエラー", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbDialog = new FolderBrowserDialog();
            OpenFileDialog op = new OpenFileDialog();

            op.Title = "ファイルの選択";
            op.InitialDirectory = @"E:data.csv";
            op.FilterIndex = 1;
            DialogResult result = op.ShowDialog();

            DataTable table = new DataTable();

            //フォルダを選択するダイアログを表示する
            if (result == DialogResult.OK)
            {
                Console.WriteLine(op.FileName);
                String Path = op.FileName;
                Data.Clear();

                //列ヘッダーを表示
                dataGridView1.ColumnHeadersVisible = true;
            
                dbAccess.dbConnect db = new dbAccess.dbConnect();
                //TARGETのフォーマットはANSIのため、Defalutを指定する。
                db.DbReadAllData(Path, ref Data, Encoding.Default);
                if(Data.Count != 0)
                {
                    //データ情報をセット
                    List<DataGridViewRow> rowsArray = new List<DataGridViewRow>();
                    DataGridViewRow row = new DataGridViewRow();

                    toolStripProgressBar1.Visible = true;
                    toolStripProgressBar1.Maximum = Data.Count;
                    toolStripStatusLabel1.Text = STR.FILE_OPEN("STR_DTLT_FNDGET1");

                    //データの取得まで成功したら既存データは削除
                    //dataGridView1.Rows.Clear();

                    table.Clear();
                    table.Columns.Add("No.");
                    table.Columns.Add("日付");
                    table.Columns.Add("ﾚｰｽ名");
                    table.Columns.Add("場名");
                    table.Columns.Add("距離S");
                    table.Columns.Add("馬場");
                    table.Columns.Add("着順");
                    table.Columns.Add("人気");
                    table.Columns.Add("馬名");
                    table.Columns.Add("騎手");
                    table.Columns.Add("負担");
                    table.Columns.Add("FType");
                    table.Columns.Add("父");
                    table.Columns.Add("母");
                    table.Columns.Add("Color1");
                    table.Columns.Add("Fno");
                    table.Columns.Add("MFColor");
                    table.Columns.Add("母父");

                    for (int i=0; i<Data.Count; i++)
                    {
#if DEBUG
                        Console.WriteLine(i + "/" + Data.Count);

#endif

                        toolStripProgressBar1.Value = i;





                        //データ書き出し
                        //1行ずつのデータにする。

                        var rows = Data[i].Split(',');
                        row = new DataGridViewRow();
                        row.CreateCells(dataGridView1);
#if true
                        row.Cells[11].Style.BackColor = ComMain.DbComSearchBloodColor(HorceNameData(rows[14]));

                        //種牡馬と母父のタイプ
                        row.Cells[11].Style.BackColor = libTarget.TargetBooldTypeColor(rows[14]);
                        row.Cells[16].Style.BackColor = libTarget.TargetBooldTypeColor(rows[17]);
#endif

                        try
                        {

#if false
                            DataRow newNows = table.NewRow();
                            newNows["No."] = i;
                            newNows["日付"] = rows[1];
                            newNows["ﾚｰｽ名"] = rows[3];
                            newNows["場名"] = rows[2];
                            newNows["距離S"] = rows[4];
                            newNows["馬場"] = rows[20];
                            newNows["着順"] = rows[5];
                            newNows["人気"] = rows[6];
                            newNows["馬名"] = rows[9];
                            newNows["騎手"] = rows[12];
                            newNows["負担"] = rows[13];
                            newNows["FType"] = "";
                            newNows["父"] = HorceNameData(rows[15]);
                            newNows["母"] = HorceNameData(rows[16]);
                            newNows["Color1"] = "";
                            newNows["Fno"] = "";
                            newNows["MFColor"] = "";
                            newNows["母父"] = HorceNameData(rows[18]);

                            table.Rows.Add(newNows);

                            row.Cells[11].Style.BackColor = ComMain.DbComSearchBloodColor(HorceNameData(rows[14]));

                            //種牡馬と母父のタイプ
                            row.Cells[11].Style.BackColor = libTarget.TargetBooldTypeColor(rows[14]);
                            row.Cells[16].Style.BackColor = libTarget.TargetBooldTypeColor(rows[17]);

#else

                            //TARGETの出力仕様に合わせる
                            row.SetValues(new object[] {
                            i, //No
                            rows[1], //日付
                            rows[3],//レース名
                            rows[2],//場名
                            rows[4],//距離S
                            rows[20],
                            rows[5],//着順
                            rows[6],//人気
                            rows[9],//馬名
                            rows[12],//騎手
                            rows[13] + "kg",//騎手,
                            "",　//父タイプカラー
                            HorceNameData(rows[15]),//父
                            HorceNameData(rows[16]),//母
                            "",
                            "", //Fno
                            "", //母父タイプ・カラー
                            HorceNameData(rows[18]),//母父
                            });

                            rowsArray.Add(row);
#endif
                        }
                        catch(Exception)
                        {
                            MessageBox.Show("ファイルフォーマットが一致しません。");
                            return;
                        }

                        
                    }

#if true

                    if (rowsArray.Count != 0)
                    {
                        DataGridViewRow[] rows = new DataGridViewRow[Data.Count];
                        //List型に持っているものを配列型に入れる
                        for (int i = 0; i < rowsArray.Count; i++) rows[i] = rowsArray[i];
                        //DatagridViewに書き込む
                        dataGridView1.Rows.AddRange(rows);
                    }
#endif
                }

#if false
                this.dataGridView1.DataSource = table;
#endif

                //テキストボックスにパスを書き出す
                textBox1.Text = Path;
            }

            // オブジェクトを破棄する
            fbDialog.Dispose();

            //ステータスバーを非表示
            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Text = "準備完了";
        }
        
        //""を削除する。
        private String HorceNameData(String Name)
        {
            String tmp = "";
            if (Name.Contains('"'))
            {
                tmp = Name.Replace("\"", "");
            }
            else
            {
                return Name;
            }
            return tmp;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //F-No.取得
            if(Data.Count == 0)
            {
                return;
            }

            
            if(FnoData == null)
            {
                FnoData = new JvComDbData.JvFamilyNoData();
            }

            //ステータスバーを表示
            toolStripProgressBar1.Visible = true;
            toolStripStatusLabel1.Visible = true;

            //ステータス状態更新
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Maximum = Data.Count;
            toolStripStatusLabel1.Text = STR.FILE_OPEN("STR_DTLT_FNDGET1");

            //ファミリーナンバーを取得する
            for (int i=0; i<Data.Count; i++)
            {
                var split = Data[i].Split(',');

                //netkeibaからファミリーナンバーを取得する。
                Task<JvComDbData.JvFamilyNo> Param = Task.Run(() =>
                 {
                     return NK.GetFamilyNumberToReturn(split[19]);
                 });

                JvComDbData.JvFamilyNo msg = Param.Result;

                if (msg.FamilyNumber1 == null)
                {
                    NK.SetNodata(split[19], "---");
                    dataGridView1.Rows[i].Cells[15].Value = "---";

                }

                //ステータス更新
                toolStripProgressBar1.Value++;

               // this.Refresh();
            }

            String outKettoNumber = "";
            String outFmailyNumber = "";

            //Fno表示エリアを有効化
            dataGridView1.Columns["FNo"].Visible = true;
            dataGridView1.Columns["FnoColor"].Visible = true;

            //datagridviewに書き込む
            //DataGridViewには空1行含まれているため、-1する
            for (int i = 0; i < (dataGridView1.Rows.Count - 1); i++)
            {
                NK.GetIndexData(i, ref outKettoNumber, ref outFmailyNumber);
                if (outFmailyNumber == "")
                {
                    //セーブデータを確認し、あればそのファミリーナンバーを入れる
                    if(!FnoData.CheckSaveFamilyNumber(outKettoNumber, ref outFmailyNumber))
                    {
                        outFmailyNumber = "---";
                    }
                }
                else if(outFmailyNumber == "---")
                {
                    if (!FnoData.CheckSaveFamilyNumber(outKettoNumber, ref outFmailyNumber))
                    {
                        outFmailyNumber = "---";
                    }
                }

                dataGridView1.Rows[i].Cells[15].Value = outFmailyNumber;
                dataGridView1.Rows[i].Cells[14].Style.BackColor = (outFmailyNumber == "---" ? Color.White : FnoData.JvFnColorData(outFmailyNumber));
            }

            toolStripProgressBar1.Visible = false;
            toolStripStatusLabel1.Text = STR.FILE_OPEN("STR_DTLT_FNDGET2");

            InitFlg = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void するToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                dataGridView1.Columns["FNo"].ReadOnly = false;
            }
            else
            {
                dataGridView1.Columns["FNo"].ReadOnly = true;

                //ファミリーナンバーを書き込んだら、DB書き込み指示
                if (FnManualChangeFlg)
                {
                    FnoData.WritSaveBooldData();
                    FnManualChangeFlg = false;
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            String outKettoNumber = "";
            String outFmailyNumber = "";

            if (!InitFlg) return;

            //変更したセルがファミリーナンバー
            if (e.ColumnIndex == 14)
            {
                NK.GetIndexData(e.RowIndex, ref outKettoNumber, ref outFmailyNumber);
                if(outFmailyNumber != dataGridView1.Rows[e.RowIndex].Cells[15].Value.ToString())
                {
                    //ファミリーナンバーが変更された
                    if(FnoData == null)
                    {
                        //ファミリーナンバーインスタンスが初期化されてない。
                        LOG.CONSOLE_MODULE("DL", "FnoData NullException");
                        return;
                    }

                    FnoData.AddSaveBooldData(outKettoNumber, dataGridView1.Rows[e.RowIndex].Cells[15].Value.ToString());
                    FnManualChangeFlg = true;
                }
            }
        }

        private void DataList_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ファミリーナンバーを書き換え、保存されていない場合
            if(FnManualChangeFlg)
            {
                DialogResult result = MessageBox.Show("ファイルが編集されています。 \r\n保存しますか？", "WpfApp1", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information);
                switch (result)
                {
                    case DialogResult.Yes:
                        //保存する。
                        FnoData.WritSaveBooldData();
                        FnManualChangeFlg = false;
                        break;
                    case DialogResult.No:
                        //なにもせずに、フォームも閉じる。
                        break;
                    case DialogResult.Cancel:
                        //キャンセルした場合、フォームを閉じない(編集中)
                        e.Cancel = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void DataList_Activated(object sender, EventArgs e)
        {
            this.dataGridView1.ShowCellToolTips = true;
        }

        private void DataList_Deactivate(object sender, EventArgs e)
        {
            this.dataGridView1.ShowCellToolTips = false;
        }
    }
}
