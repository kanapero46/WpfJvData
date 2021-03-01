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
using WpfApp1.JvComDbData;

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

        Class.com.JvComClass LOG = new JvComClass();

        JvComDbData.JvDbHRData HR = new JvComDbData.JvDbHRData();
        JvDbWEData WE = new JvDbWEData();
        JvDbW5Data W5 = new JvDbW5Data();
        JvDbTcData TC = new JvDbTcData();
        JvDbWhData WH = new JvDbWhData();
        JvDbJcData JC = new JvDbJcData();

        public int JvInfoBackMain(int kind, String Key)
        {
            int ret = 0;
            LOG.CONSOLE_MODULE("BE_INFO", "[" + kind + "]" + Key);
            switch(kind)
            {
                case JV_RT_EVENT_PAY:   //���ߊm����
                    ret = JvInfoBackJvRead("0B15", Key);
                    break;
                case JV_RT_EVENT_JOCKEY_CHANGE: //�R��ύX
                case JV_RT_EVENT_WEATHER: //�V��E�n���ԕύX���
                case JV_RT_EVENT_TIME_CHANGE:
                    ret = JvInfoBackJvRead("0B16", Key);
                    break;
                case JV_RT_EVENT_WEIGHT:
                    ret = WH.JvDbWhLinkData(Key);
                    break;
            }


            return ret;
        }


        //����n�o�b�N�G���h����
        private int JvInfoBackJvRead(String Spec, String Key)
        {
            JVData_Struct.JV_HR_PAY Pay = new JVData_Struct.JV_HR_PAY();
            JVData_Struct.JV_WE_WEATHER Weather = new JVData_Struct.JV_WE_WEATHER();
            JVData_Struct.JV_JC_INFO Jc = new JVData_Struct.JV_JC_INFO();


            JVForm JvForm = new JVForm();
            int ret = 0;

            Boolean HRWriteFlag = false;
            Boolean TCWriteFlag = false;

            JvForm.JvForm_JvInit();

            ret = JvForm.JvForm_JvRTOpen(Spec, Key);

            if(ret != 0)
            {
                LOG.CONSOLE_MODULE("BE_INFO", "JVRTOPEN ERROR! JvInfoBackJvRead["+ Spec +"](" + ret + ")");
                JvForm.JvForm_JvClose();
                return ret;
            }

            ret = 1;
            String buff = "";
            int size = 20000;
            String filename = "";

            //�C���X�^���X�̏��������K�v�Ȃ��̂͂����ŏ���������B
            TC = new JvDbTcData();
            JvDbJcData JcData = new JvDbJcData();

            while(ret >= 1)
            {
                ret = JvForm.JvForm_JvRead(ref buff, out size, out filename);
                if(ret >= 1)
                {
                    switch(buff.Substring(0,2))
                    {
                        case "HR":
                            HR.SetHrData(ref buff);
                            HRWriteFlag = true;
                            break;
                        case "WE":  //�V��E�n����
                            WE.JvDbWeSetData(ref buff);
                            break;
                        case "TC":
                            TC.SetJvTcData(ref buff);
                            TCWriteFlag = true;
                            break;
                        case "JC":
                            JC.RestrictJCData(ref buff);
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

                if(TCWriteFlag)
                {
                    TC.ExecJvTcData();
                }

            }
            JvForm.JvForm_JvClose();

            //DB�ɏ�������
            if(HRWriteFlag)
            {
                ret = HR.JvWriteHrData();
            }
            else if(TCWriteFlag)
            {
                ret = TC.WriteJvDbData();
            }

            return 1;
        }

        #region ���ߋ��m�苣�n��E�ꖼ�擾
        public void BackEndoGetPayCource(ref String Key, ref String Cource, ref int Racenum)
        {
            HR.GetPayInfo(ref Key, ref Cource, ref Racenum);
        }
        #endregion

        #region ���ߏ���Windows�֒ʒm���邽�߂̃}�b�s���O�֐�
        public void BackEndPayInfoNotice()
        {
            HR.JvHrNoticeWindows();
        }
        #endregion

        #region JvData���C�u�������狣�n�ꖼ���擾
        unsafe public String BackendMappingCourceName(String JyoCd)
        {
            int libCode = 0;
            String retTmp = "";
            libCode = LibJvConv.LibJvConvFuncClass.COURCE_CODE;
            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&libCode, JyoCd, ref retTmp);
            return retTmp;
        }
        #endregion

        #region ���������ύX�����擾 0:�擾�Ȃ��@1:�擾����
        public int BackEndHassouTimeChangeInfo(int idx, String Date, ref List<String> Out)
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> tmp = new List<string>();
            if (db.TextReader_Col(Date, "TC", 0, ref tmp, idx.ToString()) == 0)
            {
                return 0;
            }

            Out = tmp;
            return 1;           
        }
        #endregion

        #region ���������̍Ō�̌������擾
        public int BackEndHassouTimeChangeInfoCounter(String Date)
        {
            dbAccess.dbConnect db = new dbAccess.dbConnect();
            List<String> tmp = new List<string>();
            int ret = 0;

            for(int i=1; ;i++)
            {
                if(db.TextReader_Col(Date, "TC", 0, ref tmp, i.ToString()) >= 1 )
                {
                    if(tmp.Count == 0)
                    {
                        return i;
                    }
                    else
                    {
                        ret++;
                    }
                }
                else
                {
                    break;
                }
            }
            return ret;
        }
        #endregion

        unsafe public void BackEndGetKaisaiInfo(String Date)
        {

            JVForm Jv = new JVForm();

            Jv.JvForm_JvInit();
            Jv.JvForm_JVWatchEvent();
            int ret = Jv.JvForm_JvRTOpen("0B14", Date);

            if(ret != 0)
            {
                LOG.CONSOLE_MODULE("BE_INFO", "KaisaiInfo Error RTOPEN[" + ret + "]");
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
        /* @return -1�F�G���[�i�擾�Ɏ��s�j�@0�F�������@1:������ 2:���� 3�F���[�X���~ */
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
                        //���[�X���~����ǉ����邽�߁Aif���ɕύX
                        if (O1.head.DataKubun == "1" || O1.head.DataKubun == "3") res = 1;  //������
                        else if (O1.head.DataKubun == "9") res = 3;                         //���[�X���~
                        else res = 2;                                                       //��������
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

        #region �ύX���擾
        public int BackEndWeatherCondInfo(String Spec, String Key, ref List<backClass.baclClassInfo> InfoClass)
        {
            int ret = 0;

            JVForm JvForm = new JVForm();
            backClass.baclClassInfo tmpWeatherCond = new baclClassInfo();


            JvDbWEData JvWeData = new JvDbWEData();

            JvForm.JvForm_JvInit();
            JvForm.JvForm_JVWatchEvent();
            ret = JvForm.JvForm_JvRTOpen(Spec, Key);

            if (ret != 0)
            {
                LOG.CONSOLE_MODULE("BE_INFO", "JVRTOPEN ERROR! WeatherCondeInfo[" + Spec + "](" + ret + ")");
                JvForm.JvForm_JVWatchEventClose();
                JvForm.JvForm_JvClose();
                return -1;
            }

            String buff = "";
            String fname = "";
            int size = 20000;
            ret = 1;

            JVData_Struct.JV_WE_WEATHER we = new JVData_Struct.JV_WE_WEATHER();
            //JVData_Struct.WF_PAY_INFO W5 = new JVData_Struct.WF_PAY_INFO();

            JvDbW5Data W5 = new JvDbW5Data();

            while (ret >= 1)
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
                        JvWeData.JvDbWeSetData(ref buff);
                        break;
                    case "AV":
                        we.SetDataB(ref buff);
                        //InfoClass.SetDNSInfo();
                        break;
                    case "JC":  //�R��ύX
                        //InfoClass.SetJockeyInfo();
                        // JcData = new JvDbJcData( ref buff, false,  )
                        break;
                    case "TC":  //���������ύX
                      //  InfoClass.SetTimeInfo();
                        break;
                    case "CC":  //�R�[�X�ύX���
                     //   InfoClass.SetCourceInfo();
                        break;
                    case "WF":  //WIN5�f�[�^
                        W5.SetW5Data(ref buff);
                        break;
                    default:
                        break;
                }

               
            }

            //����
            backClass.baclClassInfo tmpInfoClass;

            if (Spec == "0B14" || Spec == "0B16")
            {
                WeatherCourceStatus weatherStatus = new WeatherCourceStatus();
                

                for (int i = 0; i < JvWeData.JvDbWeGetCount(); i++)
                {
                    tmpInfoClass = new baclClassInfo();
                    ret = JvWeData.JvDbWeGetDataMapping(i, ref weatherStatus);
                    BackEndConvWeatherStatusClassInfo(ref weatherStatus, ref tmpInfoClass);
                    InfoClass.Add(tmpInfoClass);
                }
            }
            else if(Spec == "0B51")
            {
                tmpInfoClass = new baclClassInfo();
                tmpInfoClass.W51 = (JvDbW5Data)W5;
                InfoClass.Add(tmpInfoClass);
            }



            JvForm.JvForm_JVWatchEventClose();
            JvForm.JvForm_JvClose();
            return 1;
        }
        #endregion

        #region WIN5�̃o�b�N�G���h����
        public int BackEndGetWin5Info(String key)
        {
            List<baclClassInfo> classInfo = new List<baclClassInfo>();
            if (BackEndWeatherCondInfo("0B51", key, ref classInfo) != 0)
            {
                if(classInfo.Count != 0)
                {
                    W5 = classInfo[0].W51;
                    LOG.CONSOLE_MODULE("BE_INFO", "WIN5 Enable Status(" + classInfo[0].W51.Win5Status + ")");
                    return 1;
                }
            }
            return 0;
        }
        #endregion

        #region WIN5�f�[�^�̎擾
        public void BackEndGetWin5(ref JvDbW5Data Out)
        {
            Out = W5;
        }
        #endregion

        public void GetJcData(ref InfoData info)
        {
            info.InfoRaceKey1 = JC.Key1;
            info.Type1 = InfoData.ID_TYPE_JOCKEY_CHANGE;
            info.Time1 = JC.Time1;

            //�ύX��
            String tmp = JC.AfterInfo1.MinaraiCd;
            info.ChangeAfterName1 = LOG.JvSysMappingFunction(2303, ref tmp);
            info.ChangeAfterName1 += JC.AfterInfo1.MinaraiCd + "(" + JC.AfterInfo1.Futan.Substring(0, 2) + "." + JC.AfterInfo1.Futan.Substring(2, 1) + "kg)";

            //�ύX�O
            tmp = JC.BeforeInfo1.MinaraiCd;
            info.InfoName1 = LOG.JvSysMappingFunction(2303, ref tmp);
            info.InfoName1 += JC.BeforeInfo1.MinaraiCd + "(" + JC.BeforeInfo1.Futan.Substring(0, 2) + "." + JC.BeforeInfo1.Futan.Substring(2, 1) + "kg)";
        }

        unsafe public String BackEndLibMappingFunction(int Code, String Cd)
        {
            int LibCode = Code;
            String libStr = "";
            LibJvConv.LibJvConvFuncClass.jvSysConvFunction(&LibCode, Cd, ref libStr);
            return libStr;
        }

        private void BackEndConvWeatherStatusClassInfo(ref WeatherCourceStatus In, ref backClass.baclClassInfo Out)
        {
            try
            {
                //�V��E�n���Ԃ�InformationForm�p�ɐ���
                Out.Key1 = In.Key;
                Out.WeatherFlag1 = true;
                Out.Weather = In.Weather;
                Out.TurfStatus = In.Turf;
                Out.DirtStatus = In.Dirt;
            }
            catch(Exception e)
            {
                LOG.CONSOLE_MODULE("BE_INFO", "ConvWeatherStatusInfo ConvertError!");
                LOG.CONSOLE_MODULE("BE_INFO", e.Message);
            }
        }
    }
}
