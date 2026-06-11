using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    public class CompanyMasterBO
    {
        public int P_CMP_PK { get; set; }
        public string P_CMP_CODE { get; set; }
        public string P_CMP_NAME { get; set; }
        public string P_CMP_DESC { get; set; }
        public string P_CMP_ADDR1 { get; set; }
        public string P_CMP_ADDR2 { get; set; }
        public string P_CMP_CITY { get; set; }
        public string P_CMP_PHONE { get; set; }
        public string P_CMP_MOBIL { get; set; }
        public string P_CMP_FAX { get; set; }
        public string P_CMP_EMAIL { get; set; }
        public int P_CMP_STATE { get; set; }
        public int P_CMP_CNTRY { get; set; }
        public int P_CMP_CURRENCY { get; set; }
        public string P_CMP_TAX_NO { get; set; }
        public string P_CMP_LOGO { get; set; }
        public int P_ACTIVE { get; set; }
        public int P_USER_PK { get; set; }
        public int P_BIZUNIT { get; set; }
        public DateTime P_LAST_MOD_DT { get; set; }
    }
}
