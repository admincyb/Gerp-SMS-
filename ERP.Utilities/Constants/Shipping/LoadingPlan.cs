using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.Utilities.Constants.Shipping
{
    public class LoadingPlan
    {
        /// <summary>
        /// Sp names
        /// </summary>
        #region SPS
        public const string SP_GetLoadingPlan = "SPSAL_LOADING_PLAN_HDR_GET_KV";
        public const string SP_SaveLoadingPlan = "SPSAL_LOADING_PLAN_HDR_SAVE";
        public const string SP_GetLoadingPlanReport = "SPSAL_LOADING_PLAN_RPT";
        //get sc details 
        public const string SP_GetSCDetails = "SPSAL_ORDER_SHIP_DESP_GET_KV";
        //get brand details
        public const string SP_GetBrandDetails = "SPSAL_ORDER_SHIP_BRAND_GET";
        #endregion

        /// <summary>
        /// Bind Fields  //Can avoid
        /// </summary>
        #region Fields
        public const string LPH_PK = "LPH_PK";
        public const string LPH_SHIPPING_PLAN = "LPH_SHIPPING_PLAN";
        public const string LPH_NO = "LPH_NO";
        public const string LPH_DATE = "LPH_DATE";
        public const string LPH_DESC = "LPH_DESC";
        public const string LPH_ACTIVE = "LPH_ACTIVE";
        public const string LPD_PK = "LPD_PK";
        public const string LPD_LOADING_HDR = "LPD_LOADING_HDR";
        public const string LPD_ROW_FROM = "LPD_ROW_FROM";
        public const string LPD_ROW_TO = "LPD_ROW_TO";
        public const string LPD_CARTON_FROM = "LPD_CARTON_FROM";
        public const string LPD_CARTON_TO = "LPD_CARTON_TO";
        public const string LPD_TYPE = "LPD_TYPE";
        public const string LPD_QTY_X = "LPD_QTY_X";
        public const string ROW_NO = "ROW_NO";
        public const string VALUE_ZERO = "0";
        public const string LPD_QTY_Y = "LPD_QTY_Y";
        public const string LPD_QTY = "LPD_QTY";
        public const string SNH_PK = "SNH_PK";
        public const string SNH_NO = "SNH_NO";
        public const string SNH_DATE = "SNH_DATE";
        public const string SNH_COMPANY = "SNH_COMPANY";
        public const string CSH_CONTAINER_NO = "CSH_CONTAINER_NO";
        public const string CSH_SEAL_NO = "CSH_SEAL_NO";
        public const string CSH_IN_TIME = "CSH_IN_TIME";
        public const string CVH_BOOKING_DATE = "CVH_BOOKING_DATE";
        public const string SNH_SHIP_TO_PORT = "SNH_SHIP_TO_PORT";
        public const string LPH_MOD_DT = "LPH_MOD_DT";
        public const string CONTAINER_NO = "CONTAINER_NO";
        public const string SHIP_TO_PORT = "SHIP_TO_PORT";
        public const string IN_TIME = "IN_TIME";
        public const string CUSTOMER_NAME = "CUSTOMER_NAME";
        public const string INV_NO = "INV_NO";
        public const string INV_DATE = "INV_DATE";
        public const string SNH_CONTAINER_TYPE_TEXT = "SNH_CONTAINER_TYPE_TEXT";
        

        public const string P_LPH_PK = "P_LPH_PK";
        public const string P_SNH_PK = "P_SNH_PK";
        public const string P_CIM_PK = "P_CIM_PK";
        public const string P_SOD_PK = "P_SOD_PK";
        public const string P_SOH_PK = "P_SOH_PK";

        public const string CUS_TEXT = "CUS_TEXT";
        public const string SC_NO = "SC_NO";
        public const string SC_DATE = "SC_DATE";


        public const string SHIPPING_PLAN_NO = "SHIPPING_PLAN_NO";
        public const string SHIPPING_PLAN_DATE = "SHIPPING_PLAN_DATE";

        public const string DPH_NO = "DPH_NO";
        public const string DPH_DATE = "DPH_DATE";
        public const string DPH_ETD = "DPH_ETD";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_BIZUNIT = "P_BIZUNIT";

        public const string LPH_SO_HDR = "LPH_SO_HDR";
        public const string LPH_SOH_NO = "LPH_SOH_NO";
        public const string LPD_SO_HDR = "LPD_SO_HDR";
        public const string LPH_SO_DTL = "LPH_SO_DTL";
        public const string LPH_BRAND = "LPH_BRAND";
        public const string LPH_BRAND_NAME = "LPH_BRAND_NAME";
        public const string SND_PLAN_QTY = "SND_PLAN_QTY";
        public const string LPH_SC_DATE = "LPH_SC_DATE";
        public const string LPH_EXP_DATE = "LPH_EXP_DATE";
        public const string LPH_NET_WT = "LPH_NET_WT";
        public const string LPH_CTN_NET_WT = "LPH_CTN_NET_WT";
        public const string LPH_GROSS_WT = "LPH_GROSS_WT";
        public const string LPH_CTN_GROSS_WT = "LPH_CTN_GROSS_WT";
        public const string LPH_IR_RADIATION_LOT_NO = "LPH_IR_RADIATION_LOT_NO";
        public const string CIM_PK = "CIM_PK";
        public const string SOD_PK = "SOD_PK";
        public const string CIM_BOX_NET_WT = "CIM_BOX_NET_WT";
        public const string CIM_CRTN_NET_WT = "CIM_CRTN_NET_WT";
        public const string CIM_BOX_GROSS_WT = "CIM_BOX_GROSS_WT";
        public const string CIM_CRTN_GROSS_WT = "CIM_CRTN_GROSS_WT";
        public const string CIM_LOT_TEXT = "CIM_LOT_TEXT";
        #endregion

        #region Parameters
        //public const string LPH_PK = "LPH_PK";
        //public const string LPD_PK = "LPD_PK";
        //public const string LPH_ACTIVE = "LPH_ACTIVE";
        #endregion
    }
}
