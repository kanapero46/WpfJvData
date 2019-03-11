using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WpfApp1.Class;
using WpfApp1.Class.com;
using WpfApp1.form.info.backClass;

namespace WpfApp1.form.info
{
    /* InfomationForm�̃o�b�N�G���h�N���X */
    public class BackEndInfomationForm
    {
        //�C�x���g���\���\�b�h�w��萔
        public const int JV_RT_EVENT_PAY = 1;
        public const int JV_RT_EVENT_JOCKEY_CHANGE = 2;
        public const int JV_RT_EVENT_WEATHER = 3;
        public const int JV_RT_EVENT_COURCE_CHANGE = 4;
        public const int JV_RT_EVENT_AVOID = 5;
        public const int JV_RT_EVENT_TIME_CHANGE = 6;
        public const int JV_RT_EVENT_WEIGHT = 7;

        //�V��E�n���ԍ\����
        List<baclClassInfo> backInfo = new List<baclClassInfo>();
 

        Class.com.JvComClass JvCom = new JvComClass();

        JvComDbData.JvDbHRData HR = new JvComDbData.JvDbHRData();


        public int JvInfoBackMain(int kind, String Key)
        {
            int ret = 0;
            switch(kind)
            {
                case JV_RT_EVENT_PAY:   //���ߊm����
                    ret = JvInfoBackJvRead("0B15", Key);
                    break;
                case JV_RT_EVENT_JOCKEY_CHANGE: //�R��ύX
                    ret = JvInfoBackJvRead("0B16", Key);
                    break;
                case JV_RT_EVENT_WEATHER: //�V��E�n���ԕύX���
                    break;
            }


            return ret;
        }


        private int JvInfoBackJvRead(String Spec, String Key)
        {
            JVData_Struct.JV_HR_PAY Pay = new JVData_Struct.JV_HR_PAY();
            JVForm JvForm = new JVForm();
            int ret = 0;

            JvForm.JvForm_JvInit();

            ret = JvForm.JvForm_JvRTOpen(Spec, Key);

            if(ret != 0)
            {
                Console.WriteLine("JVRTOPEN ERROR! JvInfoBackJvRead["+ Spec +"](" + ret + ")");
                JvForm.JvForm_JvClose();
                return ret;
            }

            ret = 1;
            String buff = "";
            int size = 20000;
            String filename = "";
            
            while(ret >= 1)
            {
                ret = JvForm.JvForm_JvRead(ref buff, out size, out filename);
                if(ret >= 1)
                {
                    switch(buff.Substring(0,2))
                    {
                        case "HR":
                            HR.SetHrData(ref buff);
                            break;
                    }
                }
                else if(ret == 0 || buff == "")
                {
                    //�S�t�@�C���ǂݍ��ݏI��
                    break;
                }
                else if(ret == -1)
                {
                    ret = 1;
                    continue;
                }
                else
                {

                }

            }
            JvForm.JvForm_JvClose();
            return 1;
        }

        #region ���ߋ��m�苣�n��E�ꖼ�擾
        public void BackEndoGetPayCource(ref String Cource, ref int Racenum)
        {
            HR.GetPayInfo(ref Cource, ref Racenum);
        }
        #endregion

        unsafe public String BackendMappingCourceName(String JyoCd)
        {
            int libCode = 0;
            String retTmp = "";
            libCode = LibJvConv.LibJvConvFuncClass.COURCE_CODE;
            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&libCode, JyoCd, ref retTmp);
            return retTmp;
        }

        unsafe public void BackEndGetKaisaiInfo(String Date)
        {

            JVForm Jv = new JVForm();

            Jv.JvForm_JvInit();
            Jv.JvForm_JVWatchEvent();
            int ret = Jv.JvForm_JvRTOpen("0B14", Date);

            if(ret != 0)
            {
                JvCom.CONSOLE_MODULE("BACKEND", "KaisaiInfo Error RTOPEN[" + ret + "]");
                return;
            }

            String fileName = "";
            int size = 20000;
            String buff = "";
            ret = 1;

            while(ret >= 1)
            {
                ret = Jv.JvForm_JvRead(ref buff, out size, out fileName);

                if(ret == 0)
                {
                    break;
                }
                else if(ret == -3)
                {
                    ret = 1;
                    continue;
                }
                else if(ret == -202)
                {
                    break;
                }
                else if(ret == -1)
                {
                    ret = 1;
                    continue;
                }

                if (buff == "")
                {
                    ret = 0;
                    break;
                }

                switch(buff.Substring(0,2))
                {
                    case "WE":
                        break;
                }
            


            }


        }


        #region �P���I�b�Y�ǂݍ��݁iDB�������݂Ȃ��j
        /* @return -1�F�G���[�i�擾�Ɏ��s�j�@0�F�������@1:������ 2:���� */
        public int BackEndGetOddsInfo(String Key)
        {
            JVForm JVForm = new JVForm();

            if(Key == "")
            {
                return -1;
            }

            JVForm.JvForm_JvInit();
            int ret = JVForm.JvForm_JvRTOpen("0B31", Key);

            if(ret != 0)
            {
                JVForm.JvForm_JvClose();
                return -1;
            }

            ret = 1;
            String fname = "";
            String buff = "";
            int size = 20000;

            JVData_Struct.JV_O1_ODDS_TANFUKUWAKU O1 = new JVData_Struct.JV_O1_ODDS_TANFUKUWAKU();
            int res = -1;
                       
            while(ret >= 1)
            {
                ret = JVForm.JvForm_JvRead(ref buff, out size, out fname);

                if(ret == 0)
                {
                    //EOF
                    break;
                }
                else if(ret == -1)
                {
                    //�t�@�C���؂�ւ�
                    continue;
                }
                else if(ret == -3)
                {
                    //DL��
                    continue;
                }
                else if(buff == "")
                {
                    continue;
                }
                else if(ret <= 0)
                {
                    res = -1;
                    break;
                }

                switch(buff.Substring(0,2))
                {
                    case "O1":
                        O1.SetDataB(ref buff);
                        res = (O1.head.DataKubun == "1" || O1.head.DataKubun == "3" ? 1 : 2);
                        break;
                }
      
            }

            JVForm.JvForm_JvClose();
            return res;
        }
        #endregion

        #region �J�É񎟁E������DB����擾
        public String BackEndGetKaijiNichi(String Date, int JyoCd)
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();

            List<String> ArrayStr = new List<string>();

            if(db.TextReader_Row(Date, "RA", 0, ref ArrayStr) != 0)
            {
                for(int i=0; i<ArrayStr.Count; i++)
                {
                    if(ArrayStr[i].Substring(0,10) == (Date + String.Format("{0:00}",JyoCd)))
                    {
                        return ArrayStr[i];
                    }
                }
            }

            return "";
        }
        #endregion

        public int BackEndWeatherCondInfo(String Spec, String Key, ref backClass.baclClassInfo InfoClass)
        {
            int ret = 0;

            JVForm JvForm = new JVForm();

            JvForm.JvForm_JvInit();
            JvForm.JvForm_JVWatchEvent();
            ret = JvForm.JvForm_JvRTOpen(Spec, Key);
            if(ret != 0)
            {
                return -1;
            }

            String buff = "";
            String fname = "";
            int size = 20000;
            ret = 1;

            JVData_Struct.JV_WE_WEATHER we = new JVData_Struct.JV_WE_WEATHER();

            while(ret >= 1)
            {
                ret = JvForm.JvForm_JvRead(ref buff, out size, out fname);
                if (ret == 0)
                {
                    //EOF
                    break;
                }
                else if (ret == -1)
                {
                    //�t�@�C���؂�ւ�
                    continue;
                }
                else if (ret == -3)
                {
                    //DL��
                    continue;
                }
                else if (buff == "")
                {
                    continue;
                }
                else if (ret <= 0)
                {
                    ret = -1;
                    break;
                }

                switch(buff.Substring(0,2))
                {
                    case "WE":
                        we.SetDataB(ref buff);
                        InfoClass.WeatherFlag1 = true;
                        InfoClass.Weather = we.TenkoBaba.TenkoCD;
                        InfoClass.TurfStatus = we.TenkoBaba.SibaBabaCD;
                        InfoClass.DirtStatus = we.TenkoBaba.DirtBabaCD;
                        break;
                    case "AV":
                        we.SetDataB(ref buff);
                        InfoClass.SetDNSInfo();
                        break;
                    case "JC":
                        InfoClass.SetJockeyInfo();
                        break;
                    case "TC":
                        InfoClass.SetTimeInfo();
                        break;
                    case "CC":
                        InfoClass.SetCourceInfo();
                        break;
                    default:
                        break;
                }

               
            }

            JvForm.JvForm_JVWatchEventClose();
            JvForm.Close();
            return 1;
        }

        unsafe public String BackEndLibMappingFunction(int Code, String Cd)
        {
            int LibCode = Code;
            String libStr = "";
            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&LibCode, Cd, ref libStr);
            return libStr;
        }
    }
}
