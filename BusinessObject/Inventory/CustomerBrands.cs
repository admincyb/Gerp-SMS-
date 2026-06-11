using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Inventory
{
    public class CustomerBrands
    {
        public int CIM_PK { get; set; }
        public int CIM_CUSTOMER { get; set; }
        public string CIM_BRAND_CODE { get; set; }
        public string CIM_BRAND_NAME { get; set; }
        public int? CIM_ITEM { get; set; }
        public string ITM_CODE { get; set; }
        public string ITM_NAME { get; set; }
        public string CUS_CODE { get; set; }
        public string CUS_NAME { get; set; }
        public int CIM_ACTIVE { get; set; }
        public int CIM_STATUS { get; set; }
        public int CIM_MOD_BY { get; set; }
        public DateTime CIM_MOD_DT { get; set; }
    }
}
