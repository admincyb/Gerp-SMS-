using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.AlertManagement
{
    public class AlertBO
    {
        public int ATH_PK { get; set; }
        public string ATH_NO { get; set; }
        public DateTime ATH_DATE { get; set; }
        public string ATH_TRX_TYPE { get; set; }
        public int ATH_TRX_PK { get; set; }
        public DateTime ATH_TRX_DATE { get; set; }
        public short ATH_DUE_DAYS { get; set; }
        public DateTime ATH_DUE_DATE { get; set; }
        public string ATH_NAME { get; set; }
        public int? ATH_BASIS { get; set; }
        public int? ATH_ALERT_TYPE { get; set; }
        public short ATH_NOTIFY_BFR { get; set; }
        public int? ATH_NOTIFY_BFR_UOM { get; set; }
        public string ATH_REMARKS { get; set; }
        public string ATH_NARRATION { get; set; }
        public bool ATH_NOTIFY_MESSAGE { get; set; }
        public bool ATH_NOTIFY_EMAIL { get; set; }
        public bool ATH_NOTIFY_SMS { get; set; }
        public int? ATH_NOTIFY_USER { get; set; }
        public byte ATH_STATUS { get; set; }
        public byte ACTIVE { get; set; }
        public int USER_PK { get; set; }
        public int BIZUNIT { get; set; }
        public DateTime? LAST_MOD_DT { get; set; }
        public int RET_VAL { get; set; }

    }
}
