using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.StoreManagement
{
    
    public class StoreAdjustment
    {
        public int SADH_PK { get; set; }           // Store Adjustment PK
        public int SAH_PK { get; set; }            // Store Audit Pk
        public string SAH_NO { get; set; }         // Store Audit NO Format
        public string SAH_DATE { get; set; }      // Store Auidt Date
        public int SAH_DEPT_STORE { get; set; }   // Store PK
        public int SAH_BIZUNIT { get; set; }      // SBU
        public int UserPK { get; set; }
        public List<StoreAdjustmentMaterialDtls> ItemList { get; set; }
    }
    public class StoreAdjustmentMaterialDtls
    {
        public int SAD_PK { get; set; }               // Audit Item Dtls PK
        public int SAD_STK_AUD { get; set; }               // Audit Item  PK
        public int SAD_ITEM { get; set; }           // Item NAME
        public int DEPT_PK { get; set; }               // Dept PK
        public int SL_NO { get; set; }           // Item Code
        public string ITM_NAME { get; set; }          // Dept Name
        public string ITM_CODE { get; set; }          // Dept Name
        public string DEPT_NAME { get; set; }          // Dept Name
        public float SAD_LED_STK { get; set; }        // ledgr Stock
        public float SADD_ACT_STK { get; set; }        // Actual Stock
        public string SAD_REMARKS { get; set; }       // Remarks
    }
}
