using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.StoreManagement
{
    public  class GoodsIssueNote
    {
        public int GIH_PK { get; set; }
        public string GIH_NO { get; set; }
        public string GIH_DATE { get; set; }
        public int GIH_DEPT { get; set; }
        public int GDD_DEPT_STORE { get; set; } 
        public List<GoodsIssueNoteList> GINList { get; set; }
        public int UserPk { get; set; }
    }

    public class GoodsIssueNoteList
    {
        public int GID_SL_NO { get; set; } // SL No

        public int GID_PK { get; set; }    // GIN Detail PK

        public int GRD_PK { get; set; }    // GRN Details PK

        public int GID_GI { get; set; }    // GIN PK

        public string GID_NO { get; set; } // GIN Number

        public int GID_ITEM { get; set; }
        public string GID_ITEM_NAME { get; set; }

        public int GRH_PK { get; set; }
        public string GRH_NO { get; set; }
        
        public int GID_UOM { get; set; }
        public string UOM_CODE { get; set; }

        public float GRD_QTY_BALANCE { get; set; }
       
        public float GID_QTY_REJECTED { get; set; }

        public float GID_QTY_INSPECTED { get; set; }

        public float GID_QTY_INSPECTED_LAST { get; set; }

        public string GID_REMARKS { get; set; }

        public float GRD_QTY_APPROVED { get; set; }
        public int GDD_GRN_DTL { get; set; }
        public List<DamageDetails> DamageList { get; set; }
    }

    public class GRNList
    {
        public List<GRNMaterialList> GRNMaterialList { get; set; }
    }

    public class GRNMaterialList
    {

        public int GID_GR { get; set; }
        public int GID_SL_NO { get; set; }
        public int GID_ITEM { get; set; }
        public int GID_ITEM_NAME { get; set; }

        public int MaterialPk { get; set; }
        public int UOMPk { get; set; }
        public int Store { get; set; }
        public int Vendor { get; set; }
        public float QtyRecieved { get; set; }
        
    }

    public class DamageDetails
    {
        public int GDD_GRN { get; set; }
        public int GDD_ITEM { get; set; }
        public string GDD_ITEM_NAME { get; set; }
        public int GDD_DMG_TYPE { get; set; }
        public string GDD_DMG_TYPE_TEXT { get; set; }
        public float GDD_DMG_QTY { get; set; }
        public int GDD_GRN_DTL { get; set; }


        public int GDD_DEPT_STORE { get; set; }
        public string GDD_DEPT_STORE_NAME { get; set; }
    }

}
