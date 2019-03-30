using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.dbAccess;

namespace WpfApp1.JvComDbData
{
    class JvDbUmData
    {
        struct DB_WRITE_STRUCT
        {
            public String Date;
            public String WriteStr;
        }

        DB_WRITE_STRUCT DbStruct = new DB_WRITE_STRUCT();
        Class.com.JvComClass LOG = new Class.com.JvComClass();
        const String SPEC = "UM";

        public JvDbUmData()
            {

            DbStruct.Date = "";
            DbStruct.WriteStr = "";
        }

        #region JVデータを読み込み
        public void JvDbUmDataRead(ref String buff)
        {
            String tmp = "";
            JVData_Struct.JV_UM_UMA JV_UMA = new JVData_Struct.JV_UM_UMA();

            JV_UMA.SetDataB(ref buff);

            tmp += JV_UMA.KettoNum + ","; //血統登録番号キー
            tmp += JV_UMA.Bamei.Trim() + ",";
            tmp += JV_UMA.BameiEng.Trim() + ",";
            tmp += JV_UMA.UmaKigoCD + ",";
            tmp += JV_UMA.SexCD + ",";
            tmp += JV_UMA.KeiroCD + ",";
            tmp += JV_UMA.Ketto3Info[0].Bamei.Trim() + ","; //父
            tmp += JV_UMA.Ketto3Info[1].Bamei.Trim() + ","; //母
            tmp += JV_UMA.Ketto3Info[2].Bamei.Trim() + ","; //父父
            tmp += JV_UMA.Ketto3Info[4].Bamei.Trim() + ","; //母父
            tmp += JV_UMA.Ketto3Info[12].Bamei.Trim() + ","; //母母父
            tmp += JV_UMA.Kyakusitu[0] + ",";
            tmp += JV_UMA.Kyakusitu[1] + ",";
            tmp += JV_UMA.Kyakusitu[2] + ",";
            tmp += JV_UMA.Kyakusitu[3] + ",";
            tmp += JV_UMA.Ketto3Info[0].HansyokuNum + ",";  //父の系統
            tmp += JV_UMA.Ketto3Info[4].HansyokuNum + ",";  //母父の系統
            tmp += JV_UMA.Ketto3Info[12].HansyokuNum + ",";  //母母父の系統
            tmp += JV_UMA.Ketto3Info[2].HansyokuNum + ",";  //父父の系統
            tmp += JV_UMA.Ketto3Info[6].HansyokuNum + ",";  //父父父の系統
            tmp += JV_UMA.Ketto3Info[10].HansyokuNum + ","; //母父父の血統

            DbStruct.WriteStr += tmp + "\n\r";
        }
        #endregion

        #region 保存したデータをDBに書き込み
        public int ExecUmData()
        {
            int DbReturn = 0;
            dbConnect db = new dbConnect();

            /* エラーチェック */
            if (DbStruct.WriteStr.Length == 0)
            {
                return 0;
            }

            //マスターデータ
            db.DeleteCsv("UM_MST");
            db = new dbConnect("0", SPEC, ref DbStruct.WriteStr, ref DbReturn);

            LOG.CONSOLE_TIME_MD("UM", "JvDbUmData DB Write -> " + DbStruct.Date + " ret(" + DbReturn + ")");
            DbStruct.Date = "";
            DbStruct.WriteStr = "";

            return DbReturn;

        }
        #endregion

        #region DBデータ書き込みフラグ
        public Boolean JvDbUmEnable()
        {
            if(DbStruct.WriteStr == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion


    }
}
