using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.StoreManagement
{
    public class StoreAudit
    {
        public int SAH_PK { get; set; }
        public string SAH_NO { get; set; }
        public DateTime SAH_DATE { get; set; }
        public int SAH_DEPT_STORE { get; set; }
        public int SAH_BIZUNIT { get; set; }
        public List<StoreMaterialDtls> ItemList { get; set; }
        public List<DamageDetailsDtls> DamageStock { get; set; }
        public string DEPT_NAME { get; set; }
        public int SDH_PK { get; set; }
    }
    public class StoreMaterialDtls
    {
        public int SAD_PK { get; set; }
        public int SDD_PK { get; set; }
        public int SAD_ITEM_CATEGORY { get; set; }
        public string SAD_ITEM_CATEGORY_TEXT { get; set; }
        public int SAD_ITEM { get; set; }
        public int SADTXT_ITEM { get; set; }
        public int SAD_ITEM_BATCH { get; set; }
        public string SAD_ITEM_BATCH_TEXT { get; set; }
        public float SAD_CUR_STK { get; set; }
        public float SAD_ACT_STK { get; set; }
        public string ITM_TEXT { get; set; }
        public string SAD_REMARKS { get; set; }
        public float SDD_QTY_ADJ { get; set; }
        public string SDD_REMARKS { get; set; }
    }

    public class DamageDetailsDtls
    {
        public int SDD_PK { get; set; }
        public int SDD_ITEM { get; set; }
        public int SDD_DMG_TYPE { get; set; }
        public int SDD_DMG_TYPE_TEXT { get; set; }
        public float SDD_DMG_QTY { get; set; }
    }

}
