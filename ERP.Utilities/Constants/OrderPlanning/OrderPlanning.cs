using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Constants
{  /// <summary>
   /// 
   /// </summary>
    public class OrderPlanning
    {
        /// <summary>
        /// Stored Procedures
        /// </summary>
        #region SPS
        public const string SP_SPPRD_PLAN_PEND_SAL_ORDER_GET = "SPPRD_PLAN_PEND_SAL_ORDER_GET_NEW";
                                                                
        public const string SP_SPPRD_PLAN_ITEM_SPEC_GET = "SPPRD_PLAN_ITEM_SPEC_GET";
        public const string SP_SPPRD_PLAN_TRX_SAVE = "SPPRD_PLAN_TRX_SAVE_NEW";
        public const string SP_SPPRD_PLAN_ITEM_GROUP_MST_GET_KV = "SPPRD_PLAN_ITEM_GROUP_MST_GET_KV";
        public const string SP_SPPRD_PLAN_TRX_GET_XML = "SPPRD_PLAN_TRX_GET_XML_NEW";
        public const string SP_SPPRD_PLAN_TRX_GET_LIST = "SPPRD_PLAN_TRX_GET_LIST";
        public const string SP_SPPRD_PLAN_TRX_DELETE = "SPPRD_PLAN_TRX_DELETE";
        public const string SP_SPPRD_PLAN_TRX_REVERT_SAVE = "SPPRD_PLAN_TRX_REVERT_SAVE";
        public const string SP_SPPRD_PLAN_TRX_LINE_GET = "SPPRD_PLAN_TRX_LINE_GET";
        public const string SP_SPPRD_LINE_MST_GET_KV = "SPPRD_LINE_MST_GET_KV";
        public const string SP_SPPRD_PLAN_TRX_RPT = "SPPRD_PLAN_TRX_RPT";
        public const string SP_SPPRD_WIP_ALLOC_SAVE = "SPPRD_WIP_ALLOC_SAVE";
        public const string SP_SPPRD_ALLOCATION_BIN_SO_MAP_GET = "SPPRD_ALLOCATION_BIN_SO_MAP_GET";
        public const string SP_SPPRD_PLAN_TRX_HDR_ACTIVATE = "SPPRD_PLAN_TRX_HDR_ACTIVATE";
        public const string SP_SPPRD_PLAN_TRX_AUTO = "SPPRD_PLAN_TRX_AUTO";
        public const string SP_SPPRD_PLAN_TRX_LINE_SUMM_GET_XML = "SPPRD_PLAN_TRX_LINE_SUMM_GET_XML";
        public const string SP_SPPRD_WIP_DEALLOC_SAVE = "SPPRD_WIP_DEALLOC_SAVE";
        public const string SP_SPARC_PRD_PLAN_TRX_GET_KV = "SPARC_PRD_PLAN_TRX_GET_KV";
        public const string SP_SPARC_PRD_PLAN_TRX_RPT = "SPARC_PRD_PLAN_TRX_RPT";
        public const string SPPRD_PRDN_ALLOC_GET_LIST = "SPPRD_PRDN_ALLOC_GET_LIST";
        public const string SPPRD_PRDN_ALLOC_BIN_ALLOCATED_GET_XML = "SPPRD_PRDN_ALLOC_BIN_ALLOCATED_GET_XML";
        public const string SPPRD_PRDN_ALLOC_BIN_PRODUCED_GET_XML = "SPPRD_PRDN_ALLOC_BIN_PRODUCED_GET_XML";
        public const string SPPRD_PRDN_ALLOC_BIN_GET = "SPPRD_PRDN_ALLOC_BIN_GET";
        public const string SPPRD_PLAN_PRDN_PROG_LINE_RPT = "SPPRD_PLAN_PRDN_PROG_LINE_RPT";
        public const string SPPRD_WIP_ALLOC_SC_RELEASE_GET = "SPPRD_WIP_ALLOC_SC_RELEASE_GET";
        public const string SPPRD_WIP_ALLOC_SC_RELEASE_SAVE = "SPPRD_WIP_ALLOC_SC_RELEASE_SAVE";
        public const string SPPRD_WIP_ALLOC_SC_RELEASE_BIN_GET = "SPPRD_WIP_ALLOC_SC_RELEASE_BIN_GET";
        public const string SPPRD_PRDN_ALLOC_SAVE = "SPPRD_PRDN_ALLOC_SAVE";
        public const string SPPRD_PLAN_TRX_LINE_SUMM_GET = "SPPRD_PLAN_TRX_LINE_SUMM_GET";
        public const string SPPRD_PLAN_LINE_GET = "SPPRD_PLAN_LINE_GET";
        public const string SPPRD_PLAN_SCENARIO_GET = "SPPRD_PLAN_SCENARIO_GET";
        public const string SPPRD_PLAN_PRODUCT_LINE_GET = "SPPRD_PLAN_PRODUCT_LINE_GET";
        public const string SPPRD_PLAN_PEND_SAL_ORDER_GET_RPT = "SPPRD_PLAN_PEND_SAL_ORDER_GET_RPT";
        #endregion

        #region Fields
        public const string F_PK = "PK";
        public const string F_VALUE = "VALUE";
        public const string F_SOH_PK = "SOH_PK";
        public const string F_SOH_NO = "SOH_NO";
        public const string F_SOD_PK = "SOD_PK";
        public const string F_SOD_ITEM = "SOD_ITEM";
        public const string F_SOD_ITEM_CODE = "SOD_ITEM_CODE";
        public const string F_SOD_ITEM_TEXT = "SOD_ITEM_TEXT";
        public const string F_ISD_SIZE_TEXT = "ISD_SIZE_TEXT";
        public const string F_ISD_SIZE = "ISD_SIZE";
        public const string F_SOD_QTY = "SOD_QTY";
        public const string F_SOD_REQUIRED_BY = "SOD_REQUIRED_BY";
        public const string F_SOD_QTY_DISPATCHED = "SOD_QTY_DISPATCHED";
        public const string F_SOD_QTY_ALLOCATED = "SOD_QTY_ALLOCATED";
        public const string F_SOD_QTY_PLANNED = "SOD_QTY_PLANNED";
        public const string F_SOD_BAL_TO_PLAN = "SOD_BAL_TO_PLAN";
        public const string F_ITM_PLAN_GROUP = "ITM_PLAN_GROUP";
        public const string F_ITM_PLAN_GROUP_TEXT = "ITM_PLAN_GROUP_TEXT";
        public const string F_PIG_CODE = "PIG_CODE";
        public const string F_PIG_PK = "PIG_PK";
        public const string F_PIG_NAME = "PIG_NAME";
        public const string F_SOH_BIZUNIT = "SOH_BIZUNIT";
        public const string F_SOH_BIZUNIT_TEXT = "SOH_BIZUNIT_TEXT";
        public const string F_BZU_CODE = "BZU_CODE";
        public const string F_BZU_PK = "BZU_PK";
        public const string F_ISD_AGRADE_PER = "ISD_AGRADE_PER";
        public const string F_LNE_CODE = "LNE_CODE";
        public const string F_LNE_PK = "LNE_PK";
        public const string F_SOH_CUSTOMER_CODE = "SOH_CUSTOMER_CODE";
        public const string F_SOH_CUSTOMER_NAME = "SOH_CUSTOMER_NAME";
        public const string F_CUS_PK = "CUS_PK";
        public const string F_CUS_CODE = "CUS_CODE";
        public const string F_GRP_AVAILABLE_QTY = "GRP_AVAILABLE_QTY";
        public const string F_GRP_ALLOCATED_QTY = "GRP_ALLOCATED_QTY";
        public const string F_SOD_BAL_TO_ALLOCATE = "SOD_BAL_TO_ALLOCATE";
        public const string F_ISD_SIZE_SEQUENCE = "ISD_SIZE_SEQUENCE";
        #endregion

        #region Parameters
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_CNG_BIZUNIT = "P_CNG_BIZUNIT";
        public const string P_PAGE_NO = "P_PAGE_NO";
        public const string P_PAGE_SIZE = "P_PAGE_SIZE";
        public const string P_PIG_PK = "P_PIG_PK";
        public const string P_PNH_PK = "P_PNH_PK";
        public const string P_PNH_NAME = "P_PNH_NAME";
        public const string P_PNH_CODE = "P_PNH_CODE";
        public const string P_PNH_VERSION = "P_PNH_VERSION";
        public const string P_PLANNED_QTY = "P_PLANNED_QTY";
        public const string P_PRODUCED_QTY = "P_PRODUCED_QTY";
        public const string P_FROM_DT = "P_FROM_DT";
        public const string P_TO_DT = "P_TO_DT";
        public const string P_SOD_REQUIRED_BY = "P_SOD_REQUIRED_BY";
        public const string P_LAST_MOD_DT = "P_LAST_MOD_DT";
        public const string P_FLD_NAME = "P_FLD_NAME";
        public const string P_VALUE = "P_VALUE";
        public const string P_ITM_PLAN_GROUP = "P_ITM_PLAN_GROUP";
        public const string P_ISD_SIZE = "P_ISD_SIZE";
        public const string P_LINEPK = "P_LNE_PK";
        public const string P_LNE_ACTIVE = "P_LNE_ACTIVE";
        public const string P_ITEM_GP = "P_ITEM_GP";
        public const string P_PND_PK = "P_PND_PK";
        public const string P_TOTAL_QTY = "P_TOTAL_QTY";
        public const string P_PLAN_QTY = "P_PLAN_QTY";
        public const string P_SOD_PK = "P_SOD_PK";
        public const string P_PNH_GROUP = "P_PNH_GROUP";
        public const string P_PNH_GROUP_PK = "P_PNH_GROUP_PK";
        public const string P_WITH_STOCK = "P_WITH_STOCK";
        public const string P_IS_ALLOCATED = "P_IS_ALLOCATED";
        public const string P_IS_PRODUCED = "P_IS_PRODUCED";
        public const string P_SOH_CUSTOMER = "P_SOH_CUSTOMER";
        public const string P_SOH_PK = "P_SOH_PK";
        public const string P_ITM_PK = "P_ITM_PK";
        public const string P_BAL_TO_ALLOCATE = "P_BAL_TO_ALLOCATE";
        public const string P_BAL_TO_PLAN = "P_BAL_TO_PLAN";
        public const string P_TO_DATE = "P_TO_DATE";
        public const string P_PNH_IS_CANCEL = "P_PNH_IS_CANCEL";
        public const string P_MNU_DEPT = "P_MNU_DEPT";
        public const string P_XML = "P_XML";


        public const string P_SORT_EXPRESSION = "P_SORT_EXPRESSION";
        public const string P_SORT_DIRECTION = "P_SORT_DIRECTION";
        public const string P_SORT_BY = "P_SORT_BY";
        public const string P_SORT_DIR = "P_SORT_DIR"; 
        #endregion
    }
}
