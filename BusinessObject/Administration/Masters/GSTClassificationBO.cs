using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    public class GSTClassificationBO
    {
        public int GCM_PK { get; set; }
        public string GCM_CODE { get; set; }
        public string GCM_NAME { get; set; }
        public string GCM_CRTD_BY { get; set; }
        public string GCM_MOD_BY { get; set; }
        public int GCM_ACTIVE { get; set; }
        public int GCM_TAXABILITY { get; set; }
        public int GCM_IS_NONGST { get; set; }
        public string GCM_DESC {get; set;}
        public int ACTIVE { get; set; }
        public int BIZUNIT_PK { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
    }
}
