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
    }
}
