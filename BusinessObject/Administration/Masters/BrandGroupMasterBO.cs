using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    public class BrandGroupMasterBO
    {
        public int CIG_PK { get; set; }
        public string CIG_CODE { get; set; }
        public string CIG_NAME { get; set; }       
        public DateTime CIG_MOD_DT { get; set; }
        public int CIG_MOD_BY { get; set; }       
        public DateTime CIG_CRTD_DT { get; set; }
        public int CIG_CRTD_BY { get; set; }
        public int CIG_ACTIVE { get; set; }       
        public int CIG_BIZUNIT { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        public int USER_PK { get; set; }
    }
}
