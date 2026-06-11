using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.StoreManagement
{
    public class StockTransferBO
    {
        public int SFH_PK { get; set; }    //
        public string SFH_NO { get; set; } //
        public string SFH_DATE { get; set; } // 
        public int SFH_DEPT { get; set; } //

        public int SFH_BIZUNIT { get; set; }

        public int SFH_SUBMITTED_BY { get; set; }
        public DateTime SFH_SUBMITTED_DATE { get; set; }

        public int SFH_APPROVED_BY { get; set; }
        public DateTime SFH_APPROVED_DATE { get; set; }

        public int SFH_CRTD_BY { get; set; }
        public DateTime SFH_CRTD_DT { get; set; }

        public int SFH_MOD_BY { get; set; }
        public DateTime SFH_MOD_DT { get; set; }


        public List<GINItemsDetails> GINList { get; set; }

        public List<POsTransferQty> POList { get; set; }

        public List<PRsTransferQty> PRList { get; set; }

        public List<AllocatedQtyList> AllocatedAdditionalList { get; set; }
        public List<GINPKList> GINPKList { get; set; }
        public List<POPKList> POPKList { get; set; }




       
    }
    /// <summary>
    /// For GIN  Item for Transfer
    /// </summary>
    public class GINItemsDetails
    {
        public int GID_PK { get; set; }
        public int GID_ITEM { get; set; }
        public string GIH_NO { get; set; }
        public string GIH_DATE { get; set; }
        public string GID_ITEM_TEXT { get; set; }
        public double GID_QTY_APPROVED { get; set; }
        public string GID_UOM_TEXT { get; set; }
    }
    /// <summary>
    /// For POs Transfer Quantity Details
    /// </summary>
    public class POsTransferQty
    {
        public int GID_PK { get; set; }
        public int POD_PK { get; set; }
        public string POH_NO { get; set; }
        public int POR_ITEM { get; set; }
        public string POR_ITEM_NAME { get; set; }
        public double QtyOrdered { get; set; }
        public double QtyAdditional { get; set; }
        public double BAL_PO_QTY { get; set; }
        public double BAL_PO_ADDL_QTY { get; set; }
        public int POR_UOM { get; set; }
        public string POR_UOM_NAME { get; set; }
        public double ALLOCATE_PO_QTY { get; set; }
        public double ALLOCATE_PO_ADDL_QTY { get; set; }
    }
    /// <summary>
    /// For PR s Transfer Quantity Details
    /// </summary>
    public class PRsTransferQty
    {


        public int SFD_PK { get; set; }
        public int GID_PK { get; set; }
        public int PRD_DEPT { get; set; }
        public string PRD_DEPT_NAME { get; set; }
        public int POD_PK { get; set; }
        public string POH_NO { get; set; }
        public int PRH_PK { get; set; }
        public string PRH_NO { get; set; }
        public int POR_ITEM { get; set; }
        public string POR_ITEM_NAME { get; set; }
        public double POR_QTY_ORDERED { get; set; }
        public int POR_UOM { get; set; }
        public string POR_UOM_NAME { get; set; }
        public double ALLOCATE_PR_QTY { get; set; }
        public double POR_QTY_BAL { get; set; }
    }
    /// <summary>
    /// Fro Additinal Item Details
    /// </summary>
    public class AllocatedQtyList
    {
        public int POR_ITEM { get; set; }
        public string POR_ITEM_NAME { get; set; }
        public int PRD_DEPT { get; set; }
        public string PRD_DEPT_NAME { get; set; }
        public int POR_UOM { get; set; }
        public string POR_UOM_NAME { get; set; }
        public double ALLOCATE_ADDL_PR_QTY { get; set; }
    }
    //For Selected Items
    public class GINPKList
    {
        public int xmlPK { get; set; }
    }
    public class POPKList
    {
        public int xmlPK { get; set; }
    }
   
}
