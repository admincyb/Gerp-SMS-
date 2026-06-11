using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Production
{
    public class Parameters
    {
        public const string BCHID = "BCH_PK";
        public const string BCHXML = "P_BCH_XML";
        public const string RETVAL = "P_RET_BC_NO";
        public const string BINPK = "BCH_PK";

        //Compound Preparation
        public const string COMPMAX = "CTH_PK";
        public const string COMPPK = "P_COM_PK";
        public const string STRXML = "P_CTH_XML";
        public const string DEPTPKCMP = "P_DEPT";

        public const string ITEMCATEGORY = "ITEM_CATEGORY";
        public const string ITEMPK = "ITEM_PK";
        public const string ITEMBATCHPK = "TUD_BATCH";
        public const string BATCHPK = "";
        public const string COMPOUNDPK = "P_CTH_PK";
        public const string COMPOUNDTRXPK = "P_COM_PK";
        public const string BATCHNO = "P_RET_BATCH_NO";
        public const string DETAILPK = "P_CTD_PK ";
        public const string ITEMTYPE = "P_ITM_TYPE";
        public const string BIZUNIT = "P_BIZUNIT";

        public const string TNK_PK = "TNK_PK";
        public const string TNK_TYPE = "TNK_TYPE ";
        public const string TNK_BIZUNIT = "TNK_BIZUNIT";
        public const string TNK_ACTIVE = "TNK_ACTIVE";
    }

    public class Parameters_DispersionPreparation
    {
        public const string DISPPREPPK = "P_DTH_PK";
        public const string DISPPK = "P_DSP_PK";
        public const string DEPTPK = "P_DEPT";
        public const string DISPXML = "P_DTD_XML";
        public const string RETVAL = "P_RET_VAL";
        public const string DISPBATCHNO = "P_RET_NO";
        public const string BATCH = "P_BATCH_PK";
        public const string BATCHTYPE = "P_BATCH_TYPE";
        public const string TRXPK = "P_TIH_PK";
        public const string BIZUNIT = "P_TIH_BIZUNIT";
        public const string P_DTH_DEL_STATUS = "P_DTH_DEL_STATUS";
        public const string P_DTH_BAL_STATUS = "P_DTH_BAL_STATUS";
    }

    public class Parameters_TopUpRecord
    {
        public const string TOPUPPK = "TUH_PK";
        public const string TOPUPXML = "P_TUH_XML";

        public const string TOPUPSBU = "P_BIZUNIT";
        public const string TOPUPDTLSPK = "TUD_PK";
        public const string TOPUPQTYUOM = "TUD_QTY_UOM";
        public const string TOPUPQTY = "TUD_QTY";
        public const string TOPUPITEM = "TUD_ITEM";
        public const string TOPUPITEMTYPE = "TUD_ITEM_TYPE";

        public const string TOPUPTYPE ="TUD_TYPE";
        public const string TANKPK = "TNK_PK";

        public const string TOPUPTANKPK = "P_TNK_PK";
        public const string TOPUPCATG = "COM_TYPE";
    }

}
