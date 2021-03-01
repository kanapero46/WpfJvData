/* dbConnectMapping DB��ǂݍ���ŏ������鋤�ʏ��� */
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using WpfApp1.dbAccess;
using WpfApp1.Class;
using System.Drawing;
using WpfApp1.JvComDbData;

namespace WpfApp1.dbCom1
{
    public class dbCom
    {

        dbConnect db = new dbConnect();

        //���ׂĂ̌��������擾���� */
        List<String> St = new List<string>();
        List<String> Hn = new List<string>();

        public dbCom()
        {
            db.DbReadAllData("0", "ST", 0, ref St, "0", 0);
            db.DbReadAllData("0", "HN", 0, ref Hn, "0", 0);
        }

        #region �����^�C�v��DB����ǂݍ���(1)
        public String DbComSearchBloodType(String name1)
        {
            String tmp = "";
            if (name1 == "")
            {
                return "";
            }

            if (DbComBloodType(name1, ref tmp))
            {
                return tmp;
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region �����^�C�v��DB����ǂݍ���(2)
        public String DbComSearchBloodType(String name1, String name2)
        {
            String tmp = "";
            if (name1 == "" || name2 == "")
            {
                return "";
            }

            if (DbComBloodType(name1, ref tmp))
            {
                return tmp;
            }

            if (DbComBloodType(name2, ref tmp))
            {
                return tmp;
            }
            return "";
        }
        #endregion

        #region �����^�C�v��DB����ǂݍ���(3)
        public String DbComSearchBloodType(String name1, String name2, String name3)
        {
            String tmp = "";
            if (name1 == "" || name2 == "" || name3 == "")
            {
                return "";
            }

            if (DbComBloodType(name1, ref tmp))
            {
                return tmp;
            }

            if (DbComBloodType(name2, ref tmp))
            {
                return tmp;
            }

            if (DbComBloodType(name3, ref tmp))
            {
                return tmp;
            }

            return "";
        }
        #endregion

        #region �����^�C�v��DB����ǂݍ���(���ʁE�n��)
        private Boolean DbComBloodType(String name, ref String outParam)
        {
            String fBloodName;
            String tmp = "";
            int cnt = 0;

            /* �P�D�ݒ�f�[�^(ST)����f�[�^���擾���� */
            if (db.TextReader_aCell("ST", name, "0", 2, ref tmp) == 1)
            {
                outParam = tmp;
                return true;
            }

            fBloodName = name;
            while (cnt < 7) /* �d�l�ύX#17 */
            {
                tmp = "";
                db.TextReader_aCell("HN", fBloodName, "0", 4, ref tmp);
                if (tmp == "")
                {
                    /* ���n���q�b�g���Ȃ������猟�����s���Ȃ� */
                    outParam = "";
                    break;
                }
                else
                {
                    /* �q�b�g�����畃�n�̌����o�^�ԍ��Ō������s */
                    fBloodName = tmp;
                    if (db.TextReader_aCell("ST", fBloodName, "0", 2, ref tmp) == 1)
                    {
                        /* �����Ƀq�b�g�����珈�����f */
                        outParam = tmp;
                        return true;
                    }
                    else
                    {
                        /* �����Ƀq�b�g���Ȃ��ꍇ�A�J�E���^�A�b�v���ď������s */
                        cnt++;
                    }
                }
            }
            return false;
        }
        #endregion

        #region �����^�C�v��DB����ǂݍ���(1�E�F)
        public Color DbComSearchBloodColor(String name1)
        {
            Boolean ret = false;

            if (name1 == "")
            {
                return Color.White;
            }

            Color Clr = DbComBloodColor(name1, ref ret);
            return Clr;
        }
        #endregion

        #region �����^�C�v��DB����ǂݍ���(2)
        public Color DbComSearchBloodColor(String name1, String name2)
        {
            Boolean ret = false;
            if (name1 == "" || name2 == "")
            {
                return Color.White;
            }
            Color Clr = DbComBloodColor(name1, ref ret);
            if (ret)
            {
                return Clr;
            }

            Clr = DbComBloodColor(name2, ref ret);
            return Clr;
        }
        #endregion

        #region �����^�C�v��DB����ǂݍ���(3)
        public Color DbComSearchBloodColor(String name1, String name2, String name3)
        {
            Boolean ret = false;
            if (name1 == "" || name2 == "" || name3 == "")
            {
                return Color.White;
            }
            Color Clr = DbComBloodColor(name1, ref ret);
            if (ret)
            {
                return Clr;
            }

            Clr = DbComBloodColor(name2, ref ret);
            if (ret)
            {
                return Clr;
            }

            Clr = DbComBloodColor(name3, ref ret);
            return Clr;
        }
        #endregion

        #region �����^�C�v��DB����ǂݍ���(���ʁE�F)
        private Color DbComBloodColor(String name, ref Boolean ret)
        {

            String fBloodName;
            String tmp = "";
            int cnt = 0;
            ret = false;
            int idx = 0;

#if false
            if (St.Count == 0) return Color.White;  //�擾���s�͏������Ȃ��B
            if (Hn.Count == 0) return Color.White;  //�擾���s�͏������Ȃ��B
            
            /* �P�D�ݒ�f�[�^(ST)����f�[�^���擾���� */
            for (idx = 0; idx < St.Count; idx++)
            {
                var values = St[idx].Split(',');
                if(name == values[0])
                {
                    ret = true;
                    return g_FuncHorceKindColor(values[3]);
                }
            }

            fBloodName = name;

        re:
            while (cnt < 7) /* �d�l�ύX#17 */
            {
                for (idx = 0; idx < Hn.Count; idx++)
                {
                    var values = Hn[idx].Split(',');
                    if (fBloodName == values[4])
                    {
                        //���e�����Ă���
                        fBloodName = values[4];
                        //�����Ƀq�b�g�����猟�����s
                        for (int idx2 = 0; idx2 < St.Count; idx2++)
                        {
                            var value2 = St[idx2].Split(',');
                            if (value2[0] == values[4])
                            {
                                /* �����Ƀq�b�g�����珈�����f */
                                ret = true;
                                return g_FuncHorceKindColor(value2[3]);
                            }
                        }
                        //�����ɍ��v���Ȃ��ꍇ�܂����e�T��
                        cnt++;
                        goto re;
                    }
                }
#else
#if false
                /* �����Ƀq�b�g�����珈�����f */
                /* �q�b�g�����畃�n�̌����o�^�ԍ��Ō������s */
                for (int idx2 = 0; idx2 < St.Count; idx2++)
                {
                    var value2 = St[idx2].Split(',');
                    if (value2[0] == values[4])
                    {
                        /* �����Ƀq�b�g�����珈�����f */
                        ret = true;
                        return g_FuncHorceKindColor(value2[3]);
                    }
                }
                cnt++;
                continue;
                cnt++;
            }
#endif

#if true
            if (db.TextReader_aCell("ST", name, "0", 3, ref tmp) == 1)
            {
                ret = true;
                return g_FuncHorceKindColor(tmp);
            }

            fBloodName = name;



            while (cnt < 7) /* �d�l�ύX#17 */
            {
                tmp = "";
                db.TextReader_aCell("HN", fBloodName, "0", 4, ref tmp);
                if (tmp == "")
                {
                    /* ���n���q�b�g���Ȃ������猟�����s���Ȃ� */
                    break;
                }
                else
                {
                    /* �q�b�g�����畃�n�̌����o�^�ԍ��Ō������s */
                    fBloodName = tmp;
                    tmp = "";
                    if (db.TextReader_aCell("ST", fBloodName, "0", 3, ref tmp) == 1)
                    {
                        /* �����Ƀq�b�g�����珈�����f */
                        ret = true;
                        return g_FuncHorceKindColor(tmp);
                    }
                    else
                    {
                        /* �����Ƀq�b�g���Ȃ��ꍇ�A�J�E���^�A�b�v���ď������s */
                        cnt++;
                    }
                }
            }
#endif

            //Console.WriteLine("��");
#endif
            ret = false;
            return Color.White;
        }
#endregion

#region �퉲�n�J���[����
        private Color g_FuncHorceKindColor(String Kind)
        {
            /** �퉲�n�F��`�@0�F���̑��A�P�F�m�[�U���e�[�X�g�n�A�Q�F�i�X���[���A�R�F�w�C���[�n�A�S�F�T���f�[�n�A�T�F�l�C�e�B�u�_���T�[�n
             * �U�F�Z���g�T�C�����A�V�F�n���v�g���n�A�W�F�e�f�B�n�A�X�F�}���m�E�H�[ �A�P�O�F�}�b�`�F��*/

            switch (Kind)
            {
                case "0":
                    return Color.Brown;
                case "1":
                    return Color.SkyBlue;
                case "2":
                    return Color.DeepPink;
                case "3":
                    return Color.LightGreen;
                case "4":
                    return Color.Yellow;
                case "5":
                    return Color.Orange;
                case "6":
                    return Color.MediumPurple;
                case "7":
                    return Color.DarkGreen;
                case "8":
                    return Color.LightGray;
                case "9":
                    return Color.DarkGoldenrod;
                case "10":
                    return Color.DarkGray;
                default:
                    return Color.White;
            }
        }
#endregion


#region �ߋ����f�[�^��DB����擾(�}�b�s���O)
        public int DbComGetOldRunDataMapping(String KettoNum, ref List<String> outParam, int RaceCount)
        {
            int ret = 0;
            int count = 1;
            List<String> tmp;
           
                tmp = new List<string>();
                //�����o�^�ԍ��{O���Ō�����������
                ret = DbComGetOldRunData(KettoNum, RaceCount, ref tmp);
                outParam = tmp;
          
            return ret; /* �d�l�ύX#15 */
        }
#endregion

#region �ߋ����f�[�^��DB����擾(����)
        private int DbComGetOldRunData(String KettoNum, int RunNum, ref List<String> outParam)
        {
            List<String> tmp = new List<string>();
            List<String> LibTmp = new List<String>();   
            db.TextReader_Col("0", "SE", 7, ref LibTmp, KettoNum + String.Format("{0:00}",RunNum));
            if(LibTmp.Count == 0)
            {
                return 0;
            }

            outParam = LibTmp;

            int res = db.TextReader_Col("0", "RA", 0, ref tmp, LibTmp[0].Substring(0, 16));

            if (tmp.Count == 0)
            {
                //���s���Ă��Ă�SE�f�[�^���������Ă��邽�߁Areturn��1�ŕԂ�
                return 1;
            }
            for (int i = 0; i < tmp.Count; i++)
            {
                outParam.Add(tmp[i]);
            }

            return res;
        }
#endregion


#region �g�Ԃ���J���[�Z�o
        public Color WakubanToColor(int Kind, String Wakuban)
        {
            switch(Wakuban)
            {
                case "1":
                    if(Kind == 0)
                    {
                        return Color.White;
                    }
                    else
                    {
                        return Color.Black;
                    }
                    
                case "2":
                    if(Kind == 0)
                    {
                        return Color.Black;
                    }
                    else
                    {
                        return Color.White;
                    }
                    
                case "3":
                    if (Kind == 0)
                    {
                        return Color.Red;
                    }
                    else
                    {
                        return Color.White;
                    }
                case "4":
                    if (Kind == 0)
                    {
                        return Color.Blue;
                    }
                    else
                    {
                        return Color.White;
                    }
                case "5":
                    if (Kind == 0)
                    {
                        return Color.Yellow;
                    }
                    else
                    {
                        return Color.Black;
                    }
                case "6":
                    if (Kind == 0)
                    {
                        return Color.Green;
                    }
                    else
                    {
                        return Color.White;
                    }
                 case "7":
                    if (Kind == 0)
                    {
                        return Color.Orange;
                    }
                    else
                    {
                        return Color.Black;
                    }
                case "8":
                    if (Kind == 0)
                    {
                        return Color.Pink;
                    }
                    else
                    {
                        return Color.Black;
                    }
            }

            if (Kind == 0)
            {
                return Color.White;
            }
            else
            {
                return Color.Black;
            }
           
        }
#endregion

        public Boolean GetJockeyChangeInfo(String RaKey, ref List<JvComDbData.JvDbJcData> retArray)
        {
            //List<JvComDbData.JvDbJcData> retArray = new List<JvComDbData.JvDbJcData>();
            List<JvComDbData.JvDbJcData> JvDbJcData = new List<JvComDbData.JvDbJcData>();
            List<String> tmp = new List<string>();
            Boolean SetFlag = false;
            JvComDbData.JvDbJcData cacheJcData;


            for (int i = 1; ; i++) //�G���[�ɂȂ�܂Ōp��
            {
                tmp.Clear();
                cacheJcData = new JvComDbData.JvDbJcData();
                if (db.TextReader_Col(RaKey.Substring(0, 8), "JC", 0, ref tmp, i.ToString()) != 0)
                {
                    if (cacheJcData.ReadData_AV(ref tmp) == 0)
                    {
                        break;
                    }
                    else
                    {
                        JvDbJcData.Add(cacheJcData);
                        Console.WriteLine("[INFO]InitChangeInfo TRUE!");
                    }
                }
                else
                {
                    //�擾�I���E�܂��͎擾�Ȃ�
                    return false;
                }
            }

            //�R��ύX
            for (int i = 0; i < JvDbJcData.Count; i++)
            {
                SetFlag = false;
                if (JvDbJcData[i].Key1.Substring(0, 16) == RaKey)
                {
                    if (retArray.Count == 0)
                    {
                        SetFlag = true;
                        retArray.Add(JvDbJcData[i]);
                    }
                    else
                    {
                        for (int j = 0; j < retArray.Count; j++)
                        {
                            if (retArray[j].BeforeInfo1.JcokeyCode == JvDbJcData[i].BeforeInfo1.JcokeyCode)
                            {
                                //���łɓo�^�ς݂̋R��̏ꍇ�͏㏑��
                                retArray[j] = JvDbJcData[i];
                                SetFlag = true;
                            }
                        }
                        if (!SetFlag)
                        {
                            //for���̒��Ō����Ɉ���������Ȃ��ꍇ�͒ǉ�
                            retArray.Add(JvDbJcData[i]);
                        }

                    }

                }

            }

            return (retArray.Count == 0 ? false : true);
        }

    }
}