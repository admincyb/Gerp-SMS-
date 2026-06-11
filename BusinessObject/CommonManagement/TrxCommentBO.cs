using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BusinessObject.CommonManagement
{
    public class TrxCommentBO
    {
        public int ACM_PK { get; set; }
        public string ACM_APP_TYPE { get; set; }
        public int ACM_APP_TRX_PK { get; set; }
        public string ACM_APP_TRX_CODE { get; set; }
        public string ACM_COMMENT { get; set; }
        public int USER_PK { get; set; }
        public int BIZUNIT { get; set; }
        public DateTime? ACM_DATE { get; set; }
        public DateTime LAST_MOD_DT { get; set; }

    }
}
