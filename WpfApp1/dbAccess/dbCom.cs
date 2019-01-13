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

namespace WpfApp1.dbCom1
{
    public class dbCom
    {

        dbConnect db = new dbConnect();

        #region �����^�C�v��DB����ǂݍ���(1)
        public String DbComSearchBloodType(String name1)
        {
            String tmp = "";
            if(name1 == "")
            {
                return "";
            }

            if(DbComBloodType(name1, ref tmp))
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
            if(name1 == "" || name2 == "")
            {
                return "";
            }
         
            if(DbComBloodType(name1, ref tmp))
            {
               return tmp;
            }

            if(DbComBloodType(name2, ref tmp))
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
            if(name1 == "" || name2 == "" || name3 == "")
            {
                return "";
            }
         
            if(DbComBloodType(name1, ref tmp))
            {
               return tmp;
            }

            if(DbComBloodType(name2, ref tmp))
            {
                return tmp;
            }

            if(DbComBloodType(name3, ref tmp))
            {
                return tmp;
            }

            return "";
        }
        #endregion

        #region �����^�C�v��DB����ǂݍ���(����)
        private Boolean DbComBloodType(String name, ref String outParam)
        {
            String fBloodName;
            String tmp = "";
            int cnt = 0;

            /* �P�D�ݒ�f�[�^(ST)����f�[�^���擾���� */
            if(db.TextReader_aCell("ST", name, "0", 2, ref tmp) == 1)
            {
                outParam = tmp;
                return true;
            }

            fBloodName = name;
            while(cnt < 5)
            {
                tmp = "";
                db.TextReader_aCell("HN", fBloodName, "0", 4, ref tmp);
                if(tmp == "")
                {
                    /* ���n���q�b�g���Ȃ������猟�����s���Ȃ� */
                    outParam = "";
                    break;
                }
                else
                {
                    /* �q�b�g�����畃�n�̌����o�^�ԍ��Ō������s */
                    fBloodName = tmp;
                    if(db.TextReader_aCell("ST", fBloodName, "0", 2, ref tmp)�@== 1)
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


        #region �ߋ����f�[�^��DB����擾(�}�b�s���O)
        public int DbComGetOldRunDataMapping(String KettoNum, ref List<String> outParam, int RaceCount)
        {
            int ret = 0;
            int count = 1;
            List<String> tmp;
            do
            {
                tmp = new List<string>();
                //�����o�^�ԍ��{O���Ō�����������
                ret = DbComGetOldRunData(KettoNum, count, ref tmp);
                outParam = tmp;
            } while (RaceCount != count && ret != 0);
            return 1;
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

            db.TextReader_Col("0", "RA", 0, ref tmp, LibTmp[0].Substring(0, 16));

            if (tmp.Count == 0)
            {
                //���s���Ă��Ă�SE�f�[�^���������Ă��邽�߁Areturn��1�ŕԂ�
                return 1;
            }
            for (int i = 0; i < tmp.Count; i++)
            {
                outParam.Add(tmp[i]);
            }

            return 1;
        }
        #endregion

    }
}