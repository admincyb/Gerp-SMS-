using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Administration.Masters
{
    public class TaxSettings
    {
        public int TAX_PK { get; set; }
        public string TAX_HEAD { get; set; }
        public int TAX_CATEGORY { get; set; }
        public string TAX_Formula { get; set; }
        public string TAX_FROM_DT { get; set; }
        public string TAX_TO_DT { get; set; }
        //public bool TAX_DISC { get; set; }
        //public bool TAX_EXT { get; set; }
        public int TAX_TYPE { get; set; }
        public string DISC_FROM { get; set; }
        public string TAX_DESC { get; set; }
        public string EXT_FROM { get; set; }
        public string EXT_TO { get; set; }
        public int UserID { get; set; }
        public int BIZUNIT { get; set; }
        public int TAX_ACCOUNT { get; set; }
        public int? TAX_SUB_CATEGORY { get; set; }
        public string TAX_NOT_DUE { get; set; }
        public string TAX_DISP_NAME { get; set; }
        public string TAX_CODE { get; set; }
        public string TAX_IS_RETURN { get; set; }
        public string TAX_IS_SALE { get; set; }
        public string TAX_IS_PURCHASE { get; set; }
        public string TAX_RATE { get; set; }
        public string TAX_ACTIVE { get; set; }
        public string TAX_GST_GROUP { get; set; }

        public string TAX_IS_FOB_CAL { get; set; }
        public string TAX_AUTO_OTHER_ENABLE { get; set; }
    }
}
