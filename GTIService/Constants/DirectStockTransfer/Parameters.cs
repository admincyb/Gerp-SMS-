using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.DirectStockTransfer
{
    public class Parameters
    {
        public const string XML_VALUES = "P_XML";
        public const string P_RET_VAL = "P_RET_VAL";
        public const string P_GRH_XML = "P_GRH_XML";

        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_DEPT = "P_DEPT";
        public const string P_VENDOR = "P_VENDOR";
        public const string P_MAT_RET = "P_MAT_RET"; 
        public const string P_SER_NAME = "P_SER_NAME";
        public const string P_SER_VAL = "P_SER_VAL";
        public const string P_FIELDS = "P_FIELDS";
        public const string P_PENDING_WO = "P_PENDING_WO";
        public const string P_FROM_DT = "P_FROM_DT";
        public const string P_TO_DT = "P_TO_DT";
        public const string P_GRH_PK = "P_GRH_PK";
        public const string P_GRH_COMPANY = "P_GRH_COMPANY";

        public const string P_POH_PK = "P_POH_PK";
        public const string P_PAGE_NO = "P_PAGE_NO";
        public const string P_PAGE_SIZE = "P_PAGE_SIZE";
        public const string P_SORT_BY = "P_SORT_BY";
        public const string P_SORT_DIR = "P_SORT_DIR";
        public const string P_PROC_ID = "P_PROC_ID";
        public const string P_USER_PK = "P_USER_PK";

        public const string P_RET_NO = "P_RET_NO";
        public const string P_GRH_NO = "P_GRH_NO";
        public const string P_POH_NO = "P_POH_NO";

        public const string GridParmeters = "[GRH_PK],[GRH_NO],[GRH_DATE],[GRH_STATUS],[GRH_STATUS_TEXT],[REF_ID],[DPT_NAME],[GRH_VENDOR_TEXT],[GRH_PO_NO],[GRH_VND_REF_NO],[GRH_INV_DEPT],[CMP_DISPLAY_CODE],[CMP_LINE_COLOUR],[GRH_IS_CONVERSION_REQD],[GRH_IS_CONVERTED]";
        public const string GridParametersPOS = "POH_PK,POD_PK,POD_ITEM,POD_UOM,POD_GRN_FLAG,POH_NO,ITM_NAME,UOM_CODE,POD_QTY_APPROVED,POD_QTY_RECEIVED,BALANCE_QTY,POH_COMPANY, ITM_CODE,POD_RATE,WIH_ITEM_TYPE,WIH_ITEM_TYPE_TEXT,POD_QTY_RETURNED";

        public const string P_LAST_MOD_DT = "P_LAST_MOD_DT";
        public const string P_FLD_NAME = "P_FLD_NAME";
        public const string P_VALUE = "P_VALUE";
        public const string P_WIH_PK = "P_WIH_PK";
        public const string P_MENUTYPE = "P_MENUTYPE";
    }
}
