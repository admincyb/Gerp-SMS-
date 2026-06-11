using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Finance.Administration.Masters
{
    public class InvoiceTypeBO
    {
        public int FTM_PK { get; set; }
        public string FTM_CODE { get; set; }
        public string FTM_NAME { get; set; }
        public string FTM_CRTD_BY { get; set; }
        public string FTM_MOD_BY { get; set; }
        public int FTM_ACTIVE { get; set; }
        public int FTM_TRX_TYPE { get; set; }
        public int FTM_INVOICE_TYPE { get; set; }
        public string FTM_DESC { get; set; }
        public int ACTIVE { get; set; }
        public int BIZUNIT { get; set; }
        public DateTime LAST_MOD_DT { get; set; }
        public int USER_PK { get; set; }
    }
}
