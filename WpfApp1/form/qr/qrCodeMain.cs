using System;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.Windows.Threading;
using Windows.Devices.Enumeration;
using Windows.Devices.PointOfService;
using ZXing;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using ZXing.Common;
using System.IO;
using LibQrDataAnalyze;

namespace WpfApp1.form.qr
{
    public partial class qrCodeMain : Form
    {
#if true
        BarcodeScanner selectedScanner = null;
        string selector;
        ObservableCollection<BarcodeScannerInfo> barcodeScanners = new ObservableCollection<BarcodeScannerInfo>();
        DeviceWatcher watcher;

        //接続されている全てのビデオデバイス情報を格納する変数
        private FilterInfoCollection videoDevices;

        //使用するビデオデバイス
        private VideoCaptureDevice videoDevice;

        //QRコード解析用ライブラリ
        LibQrDataAnalyze.JvQrMain jvQrAnalyzeClass = new JvQrMain();
        JvQrAnalyzeClass QrDataClass = new JvQrAnalyzeClass();

        //ビデオデバイスの機能を格納する配列
        private VideoCapabilities[] arrayVideoCapabilities;

        //ログ
        Class.com.JvComClass LOG = new Class.com.JvComClass();

        //読み込みステータス
        int readStatus = 0;

        public qrCodeMain()
        {
            InitializeComponent();

            LOG.CONSOLE_TIME_MD("QR", "QRCodeMain() START!!");

            selector = BarcodeScanner.GetDeviceSelector();
            //qrcodeSubMain();

            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count != 0)
            {
                comboBox1.Items.Clear();
                LOG.CONSOLE_MODULE("QR", "----- deviceList -----");

                foreach (FilterInfo device in videoDevices)
                {
                    LOG.CONSOLE_MODULE("QR", "["+ device.Name + "]");
                    comboBox1.Items.Add(device.Name);
                }
                LOG.CONSOLE_MODULE("QR", "---------------------");
                comboBox1.SelectedIndex = 0;
                comboBox2.Enabled = true;
                button1.Enabled = true;
            }
            else
            {
                LOG.CONSOLE_MODULE("QR", "device Not Found");
                comboBox1.Items.Clear();
                comboBox1.Items.Add("デバイスが見つかりません");
                comboBox1.SelectedIndex = 0;
            }

            try
            {
                toolStripStatusLabel1.Text = LibQrDataAnalyze.JvQrMain.JvQrGetLibVersion();
            }
            catch(Exception)
            {
                LOG.ASSERT("QR");
                LOG.CONSOLE_TIME("libJvQrCode.dll Read Error!!!");
            }

        }


        private void qrcodeSubMain()
        {
            watcher = DeviceInformation.CreateWatcher(selector);
         //   watcher.Added += Watcher_Added;
           // watcher.Removed += Watcher_Removed;
          //  watcher.Updated += Watcher_Updated;
            watcher.Start();

        }

        private void Watcher_Added(DeviceWatcher spender, DeviceInformation args)
        {

            barcodeScanners.Add(new BarcodeScannerInfo(args.Name, args.Id));

            // Select the first scanner by default.
            if (barcodeScanners.Count == 1)
            {
                ScannerListBox.SelectedIndex = 0;
            }
        }

        private void qrCodeMain_Load(object sender, EventArgs e)
        {

            MultiFormatReader reader = new ZXing.MultiFormatReader();
#if false
            ---
            String filename;
            System.IO.FileStream fs = new System.IO.FileStream(
                filename,
                System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Drawing.Image img = System.Drawing.Image.FromStream(fs);
            fs.Close();
            // QRコードの解析
            ZXing.BarcodeReader reader = new ZXing.BarcodeReader();
            //ZXingに渡すのはBitmap
            ZXing.Result result = reader.Decode(new Bitmap(img));



            //これでQRコードのテキストが読める
            var text = result == null ? string.Empty : result.Text;
            //因みにresult.BarcodeFormatでコード種類が取れます。
            //QRコードならZXing.BarcodeFormat.QR_CODEのはずです。
#endif
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            videoDevice = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);
            arrayVideoCapabilities = videoDevice.VideoCapabilities;
            comboBox2.Items.Clear();
            foreach (VideoCapabilities capabilty in arrayVideoCapabilities)
            {
                comboBox2.Items.Add(string.Format("{0} x {1}", capabilty.FrameSize.Width, capabilty.FrameSize.Height));
            }
            comboBox2.SelectedIndex = 0;
            arrayVideoCapabilities = videoDevice.VideoCapabilities;
            comboBox2.Items.Clear();
            foreach (VideoCapabilities capabilty in arrayVideoCapabilities)
            {
                comboBox2.Items.Add(string.Format("{0} x {1}", capabilty.FrameSize.Width, capabilty.FrameSize.Height));
            }
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            videoDevice.NewFrame += new NewFrameEventHandler(videoDevice_NewFrame);
            videoDevice.Start();
            button1.Enabled = false;
            button2.Enabled = true;
            button3.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;

            timer1.Interval = 1000;
            timer1.Start();
        }

        Reader reader = new MultiFormatReader();
        String gCacheQrData1 = ""; //QRコードデータ1
        String gCacheQrData2 = "";
        
        //2つのQRコードが読み取り完了したか？
        bool gFlgData1 = false;
        bool gGlfData2 = false;



        //新しいフレームが来た時
        void videoDevice_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap img = (Bitmap)eventArgs.Frame.Clone();
            ZXing.Result result = null;
            

            int ret = 0;

            try
            {
                //ここで画像処理
                pictureBox1.Image = img;

                if (pictureBox1.Image != null)
                {
                    Bitmap bmp = new Bitmap(pictureBox1.Image);

                    // QRコードの解析
                    ZXing.BarcodeReader reader = new ZXing.BarcodeReader();
                    //ZXingに渡すのはBitmap
                    result = reader.Decode(new Bitmap(bmp));

                    //これでQRコードのテキストが読める
                    var text = result == null ? string.Empty : result.Text;

                    if(result == null || text == "")
                    {
                        return;
                    }

                    //QRコードが読み取れた場合かつ前回のデータと異なる場合は、スキップ
                    if(gCacheQrData1 == text )
                    {
                        //スキップ
                        return;
                    }
                    else if(!gFlgData1)
                    {
                        //1回目データ
                        gCacheQrData1 = text;
                        gFlgData1 = true;
                        Console.WriteLine("Data1\n" + text);
                    }
                    else if(text != "0" || text != "")
                    {
                        //別データのため、処理呼び出し
                        gCacheQrData2 = text;

                        Console.WriteLine("Dt1:" + gCacheQrData1);
                        Console.WriteLine("Dt2:" + gCacheQrData2);

                        //ラベルに読み取りOKのステータスにする
                        readStatus = 1;

                        //フォームを更新
                        //this.Update(); → InvalidOperationException()になる

                        //2つ揃ったら、ライブラリでデータを処理する。
                        //ライブラリにデータを提供し、処理する。
                        ret = JvQrMain.JvQrAnalyzeMain2(gCacheQrData1, gCacheQrData2, ref QrDataClass);

                        Console.WriteLine(text + "\n" + ret);

                        //フラグを初期化
                        gFlgData1 = false;
                        gCacheQrData1 = "";
                        gCacheQrData2 = "";

                        //2つ揃ったら、1秒間待機させる⇒処理が早すぎる為？エラーになるので、待ち時間を設けた。
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        //1回目データと同じ→スキップ
                        return;
                    }
               
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
            catch (Exception)
            {
                LOG.ASSERT("QR");
               return;
            }           
        }

        private void ShowStatusLabel1(int Flg)
        {
            label1.ForeColor = Color.White;
            switch(Flg)
            {
                case 1:
                    //OK
                    label1.Text = "OK";
                    label1.BackColor = Color.FromArgb(0xff, 0x2e, 0x8b, 0x57);
                    break;
                case -1:
                    label1.Text = "NG";
                    label1.BackColor = Color.FromArgb(0xFF, 0xFF, 0x14, 0x93);
                    break;
                case 0:
                    label1.Text = "";
                    label1.BackColor = Color.White;
                    break;
            }
        }

        private static byte[] ToByteArray(Image img)
        {
            byte[] byteArray = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;

        }



            private void button2_Click(object sender, EventArgs e)
        {
            if (videoDevice.IsRunning)
            {
                videoDevice.SignalToStop();
                videoDevice.WaitForStop();
            }

            //タイマーストップ
            timer1.Stop();

            pictureBox1.Image = null;
            button1.Enabled = true;
            button2.Enabled = false;
            button3.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count != 0)
            {
                comboBox1.Items.Clear();
                foreach (FilterInfo device in videoDevices)
                {
                    comboBox1.Items.Add(device.Name);
                }
                comboBox1.SelectedIndex = 0;
                comboBox2.Enabled = true;
                button1.Enabled = true;
            }
            else
            {
                comboBox1.Items.Clear();
                comboBox1.Items.Add("デバイスが見つかりません");
                comboBox1.SelectedIndex = 0;
            }
        }

        private void qrCodeMain_Load_1(object sender, EventArgs e)
        {

        }

        private void qrCodeMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (videoDevice.IsRunning)
            {
                videoDevice.SignalToStop();
                videoDevice.WaitForStop();
            }
            LOG.CONSOLE_TIME_MD("QR", "QRCodeMain CloseEventHandler!");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //1秒ごとにステータスを更新
            try
            {
                ShowStatusLabel1(readStatus);
            }
            catch(Exception)
            {

            }
            
        }
    }

    public class BarcodeScannerInfo
    {
        public BarcodeScannerInfo()
        {

        }

        public BarcodeScannerInfo(String deviceName, String deviceId)
        {
            DeviceName = deviceName;
            DeviceId = deviceId;
        }

        public String Name => $"{DeviceName} ({DeviceId})";
        public String DeviceId { get; private set; }
        private string DeviceName;
#endif
            }

}
