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

namespace WpfApp1.dbCom
{
    public class dbCom
    {

        DBConnect db = new DBConnect();

        #region �����^�C�v��DB����ǂݍ���(1)
        public Boolean DbComSearchBloodType(String name1)
        {
            String tmp = "";
            if(name1 == "")
            {
                return "";
            }

            return DbComBloodType(name1, ref tmp);
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
        private boolean DbComBloodType(String name, ref String outParam)
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
    }
}