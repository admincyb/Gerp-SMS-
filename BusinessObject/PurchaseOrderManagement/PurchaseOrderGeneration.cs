using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.PurchaseOrderGeneration
{
    public class PurchaseOrder
    {
        public int POH_PK { get; set; }
        public string POH_NO { get; set; }
        public DateTime POH_DATE { get; set; }
        public int POH_SHIPPING { get; set; }
        public int POH_BILLING { get; set; }
        public int POH_STATUS { get; set; }
        public List<PurchaseOrderMaterials> MaterialDetails { get; set; }
        


    }

    public class PurchaseOrderMaterials
    {
        public int POMaterialID { get; set; }
        public int POH_PK { get; set; }
        public int POD_ITEM { get; set; }
        public string ITV_NAME { get; set; }
        public string ITM_CODE { get; set; }
        public float BaseConversion { get; set; }
        public float POD_RATE { get; set; }
        public float POD_QTY_REQUESTED { get; set; }
        public int POD_UOM { get; set; }
        public string POD_UOM_TEXT { get; set; }
        public float POD_DISC_AMT { get; set; }
        public string POD_REMARKS { get; set; }
        public float POD_AMT_VALUE { get; set; }
        public float AdditionalQty { get; set; }
        public float TaxPerPiece { get; set; }
        public List<PurchaseRequest> PRDetails { get; set; }
    }

    public class PurchaseRequest
    {
        public int PRH_PK{ get; set; }
        public string PRH_NO { get; set; }
        public DateTime PRH_DATE { get; set; }
        public float PRD_QTY_APPROVED { get; set; }
        public float QTY_BALANCE { get; set; }
        public string PRD_UOM { get; set; }
        public string UOM_NAME { get; set; }
        public float QTY_ORDER{ get; set; }
       
    }

    public class POShortClose
    {
        public int POID { get; set; }
        public string Remarks { get; set; }
        public string RefNo { get; set; }
        public int UserPk { get; set; }
        public int CheckFlag { get; set; }//For Checking it is a non Stock PO or not.While shortclosing a non stock PO,we want to show a warning msg
    }
}
