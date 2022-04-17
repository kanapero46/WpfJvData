using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibJvMsAccessLib
{
    public class LibJvMsAccessLibrary
    {
        /* 必須データ */
        private String[] Headers;
        private String Md;
        private String Pass;
        private String Data;

        /* 詳細情報(任意) */
        private Boolean CompelWriteFlg;

        /* 出力データ */
        private String[] OutputData;



       /* Accessファイル作成 */
       public void JvLibAcsCreate( ref int ret )
        {
            //必須パラメータチェック
            if (Headers.Length == 0 || Md == "" || Pass == "")
            {
                ret = 0;
                return;
            }




        }

        /* ファイル更新 */
        public void JvLibAcsUpdate( ref int ret )
        {
            const String UPDATE = "SELECT * FROM";
#if false
            using (OdbcConnection con = new OdbcConnection(CONNECTION_STRING))
            {
                con.Open();

                stringsql = "INSERT INTO TBLCAT VALUES(10, '5', 'こなつ', '♀', '6', '03', '布団')";
                using (OdbcCommand cmd = new OdbcCommand(sql, con))
                {
                    intret = cmd.ExecuteNonQuery();
                    if (ret != 1)
                    {
                        MessageBox.Show("登録に失敗しました。");
                    }
                }
            }
#endif
        }

        /* ファイル削除 */
        public void JvLibAcsDelete( ref int ret )
        {
            const String UPDATE = "DELETE FROM";

        }

        /* ファイル全読み込み */
        public void JvLibAcsDataRead( ref int ret )
        {

        }
    }
}
