using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WpfApp1.Class.com.windows
{
    public partial class JvComWindowsForm : Form
    {
        /* Windowsのポップアップ通知のアイコン */
        public const int NONE_STATUS = 0;  //無印
        public const int INFO_STATUS = 1;  //情報
        public const int WARN_STATUS = 2;  //警告
        public const int ERRO_STATUS = 3;  //エラー

        const String IconImgFilePath = "icon.ico";

        /* 通知したテキストの内容を保持しておく。    */
        /* トースト通知クリック後のイベントに使用する */
        String MessageText = "";

        #region WindowsForm自動生成
        public JvComWindowsForm()
        {
            InitializeComponent();
        }

        private void JvComWindowsForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            JvComNoticeShow(INFO_STATUS, "【確定通知：中京11R】高松宮記念(Ⅰ）", "[Ⅰ]10 [Ⅱ]9 [Ⅲ]1\n【単勝】10(\\560)\n【馬連】9-10(\\1240)\n【3連複】1-9-10(\\29100)");
        }
        #endregion

        #region Windowsへ通知を出す(秒数指定デフォルト)
        public int JvComNoticeShow(int kind, String title, String msg)
        {
            return JvComNoticeShow(kind, title, msg, 3000, "notifyIcon1");    //デフォルトで3病指定
        }

        public int JvComNoticeShow(int kind, String title, String msg, String text)
        {
            return JvComNoticeShow(kind, title, msg, 3000, text);
        }
        #endregion



        #region Windowsへ通知を出す(秒数指定あり)
        public int JvComNoticeShow(int kind, String title, String msg, int ShowTime, String text)
        {
            Boolean setEnable = true;
           // notifyIcon1 = new NotifyIcon();
            notifyIcon1.Icon = new Icon(@IconImgFilePath);

            /* アイコンの指定 */
            switch (kind)
            {
                case NONE_STATUS:
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.None;
                    break;
                case INFO_STATUS:
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    break;
                case WARN_STATUS:
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
                    break;
                case ERRO_STATUS:
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Error;
                    break;
                default:
                    setEnable = false;  //パラメーターエラー
                    break;
            }

            /* パラメータが設定されればWindowsへ通知を出す。 */
            try
            {
                if (setEnable)
                {
                    //表示時間を指定する。ただし0以下の場合はExceptionをはくため、
                    //0秒以下を指定した場合はデフォルト3mSecを指定する。
                    notifyIcon1.BalloonTipTitle = title;
                    notifyIcon1.BalloonTipText = msg;
                    notifyIcon1.Text = text;
                    MessageText = text;
                    notifyIcon1.ShowBalloonTip
                        (
                        (ShowTime >= 1 ? ShowTime : 3000)
                        );
                }
                else
                {
                    return 0; //失敗で返す
                }

            }
            catch(Exception e)
            {
                Class.com.JvComClass Log = new JvComClass();
                Log.CONSOLE_MODULE("WINDOWS", "WindowsNotice -> Exception!!!\n" + e.Message);
                return 0;
            }

            return 1;
        }
        #endregion

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Receive!!");
            Console.WriteLine(MessageText);


        }

        private void notifyIcon1_BalloonTipShown(object sender, EventArgs e)
        {
            Console.WriteLine("Sender Complete!!");
        }
    }
}
