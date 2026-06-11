using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.SubDepartmentManagement
{
    public class SubDepartmentMaster
    {

        public int DPT_PK { get; set; }
        public int SBU { get; set; }
        public int DPT_PARENT { get; set; }
        public int DPT_TYPE { get; set; }
        public int DPT_CATEGORY { get; set; }
        public string DPT_NAME { get; set; }
        public string DPT_CODE { get; set; }
        public string DPT_DESC { get; set; }
        public string DPT_ADDR1 { get; set; }
        public string DPT_ADDR2 { get; set; }
        public string DPT_EMAIL { get; set; }
        public string DPT_PHONE { get; set; }
        public string DPT_IS_STOCK { get; set; }

        public int DPT_CNTRY { get; set; }
        public int DPT_STATE { get; set; }
        public string DPT_CITY { get; set; }
        public int DPT_CURR { get; set; }
        public int DPT_COMPANY { get; set; }
        public string DPT_ZIP { get; set; }
        public string DPT_MOBILE { get; set; }
        public string DPT_GST_NO { get; set; }

        public int UserPK { get; set; }
        public int Status { get; set; }
        public string DPT_ACTIVE { get; set; }

        public string DPT_PROJECT { get; set; }

    }
}
