using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.JvComDbData
{
    class JvFamilyNo
    {
        public const String NETKEIBA_SITE_URL_HEAD = "https://db.netkeiba.com/horse/ped/";
        public const String SELECTER = "#contents > div.db_main_deta > diary_snap > table > tbody > tr:nth-child(17) > td.b_fml > br:nth-child(6)";

        private String KettoNumber;
        private String FamilyNumber;

        public string FamilyNumber1 { get => FamilyNumber; set => FamilyNumber = value; }
        public string KettoNumber1 { get => KettoNumber; set => KettoNumber = value; }
    }
}
