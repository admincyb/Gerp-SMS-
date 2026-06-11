using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessObject.Constants
{
    public class ShippingPlanDA
    {
        /// <summary>
        /// Sp names
        /// </summary>

        #region SPS
        public const string SP_DeleteShippingPlan = "SPSAL_SHIPPING_PLAN_HDR_DELETE";
        public const string SP_GetShippingPlan = "SPSAL_SHIPPING_PLAN_HDR_GET_KV";
        public const string SP_GetShippingPlanDetails = "SPSAL_SHIPPING_PLAN_DTL_GET_KV";
        public const string SP_SHIPPING_PLAN_SUMMARY_SAVE = "SPSAL_SHIPPING_PLAN_SUMMARY_SAVE";
        public const string SP_GetShippingOrders = "SPSAL_ORDER_LIST_GET";
        public const string SP_SaveShippingPlan = "SPSAL_SHIPPING_PLAN_HDR_SAVE";
        public const string SP_GetShippingPlanList = "SPSAL_SHIPPING_PLAN_HDR_GET_LIST";

        public const string SP_GetOrderTracker = "SPSAL_ORDER_TRACKER_DTL";
        public const string SP_ShippingStatusList = "SPSAL_SHIPPING_PLAN_STATUS_GET";

        public const string SP_Questionnaire = "SPADM_QUESTIONNAIRE_CFG_GET_KV";
        public const string SP_GetCommericalInvoice = "SPSAL_SHIPPING_PLAN_INVOICE_GET";

        public const string SPSAL_SHIPPING_PLAN_SO_GET = "SPSAL_SHIPPING_PLAN_SO_GET";
        public const string SPSHIPPING_PLAN_INFO_GET = "SPSHIPPING_PLAN_INFO_GET";
        public const string SPSAL_SHIPPING_PLAN_CANCEL_CHECK = "SPSAL_SHIPPING_PLAN_CANCEL_CHECK";
        public const string SPSAL_ORDER_SHIP_PLAN_CHECK = "SPSAL_ORDER_SHIP_PLAN_CHECK";
        public const string SPSAL_ORDER_CUSTOMER_GET = "SPSAL_ORDER_CUSTOMER_GET";
        public const string SPSAL_ORDER_SHIPPINGBRAND_GET = "SPSAL_ORDER_SHIPPING_DTL_GET";
        public const string SPCHECKVALIDSHIPPINGSOs = "SPSAL_SHIP_SO_VALD";

        public const string SPSAL_SHIPPING_PLAN_INVOICE_RPT = "SPSAL_SHIPPING_PLAN_INVOICE_RPT";

        //get previous saved company for each level
        public const string SPSAL_GET_PREV_COMPANY = "SPSAL_SHIPPING_PLAN_PREV_COMP_GET";

        public const string SPPRD_PLN_PRODUCT_STK_TRX_UPDATE = "SPPRD_PLN_PRODUCT_STK_TRX_UPDATE";

        //modify after creating invoice,this sp will reset the invoice
        public const string SPSAL_DESPATCH_UPDATE = "SPSAL_DESPATCH_UPDATE";

        public const string SPFIN_INVOICE_CUS_CRDR_EXISTS_CHECK = "SPFIN_INVOICE_CUS_CRDR_EXISTS_CHECK"; 

        public const string SPSAL_ORDER_DTL_LIST_GET = "SPSAL_ORDER_DTL_LIST_GET";
        public const string SPSAL_CONTAINER_RELEASE_CARTON_EXISTS = "SPSAL_CONTAINER_RELEASE_CARTON_EXISTS";
        public const string SPSAL_SHIPPING_PLAN_PACK_LIST_GET_RPT = "SPSAL_SHIPPING_PLAN_PACK_LIST_GET_RPT";


        #endregion
        /// <summary>
        /// Bind Fields  //Can avoid
        /// </summary>
        #region Fields
        public const string F_PK = "PK";
        public const string F_VALUE = "VALUE";
        public const string F_TEXT = "TEXT";

        #endregion
        /// <summary>
        /// Procedure Parameters
        /// </summary>
        #region Parameters

        public const string P_PAGE_NO = "P_PAGE_NO";
        public const string P_PAGE_SIZE = "P_PAGE_SIZE";

        public const string P_SNH_PK = "P_SNH_PK";
        public const string P_SNH_PROCESS = "P_SNH_PROCESS";
        public const string P_ACTIVE = "P_ACTIVE";
        public const string P_BIZUNIT = "P_BIZUNIT";
        public const string P_SOD_PK = "P_SOD_PK";
        public const string P_SND_PK = "P_SND_PK";
        public const string P_XML = "P_XML";
        public const string P_SC_VALIDATION = "P_SC_VALIDATION";
        public const string RETVAL = "P_RET_VAL";
        public const string P_RET_NO = "P_RET_NO";

        public const string STATUS = "P_STATUS";
        public const string CUSTOMER = "P_SNH_CUSTOMER";
        public const string SOPK = "P_SO_PK";

        public const string SOHPK = "P_SOH_PK";
        public const string CUSPK = "P_CUS_PK";


        public const string QSTPK = "P_QST_PK";
        public const string P_SHIPPING_PLAN = "P_SHIPPING_PLAN";

        public const string SER_NAME = "P_SER_NAME";
        public const string PLAN_NO = "P_SER_VAL";
        public const string SC_NO = "P_SOH_NO";
        public const string DO_NO = "P_DPH_NO";
        public const string DPH_PK = "P_DPH_PK";
        public const string CUSTPO_NO = "P_SOH_REFERENCE";

        public const string SNH_PK = "P_SNH_PK";
        public const string P_USER_PK = "P_USER_PK";

        public const string P_TRX_PK = "P_TRX_PK";
        public const string P_MODULE = "P_MODULE";
        public const string P_MODE = "P_MODE";
        public const string P_FROM_ERP = "P_FROM_ERP";

        public const string P_SOH_PK = "P_SOH_PK";
        public const string P_SOH_CUSTOMER = "P_SOH_CUSTOMER";
        public const string P_HIDE_DRAFT = "P_HIDE_DRAFT";
        public const string P_SNH_COMPANY = "P_SNH_COMPANY";

        public const string P_ITM_CODE = "P_ITM_CODE";
        public const string P_SOD_QTY = "P_SOD_QTY";

        public const string P_CDR_ALLOC_STATUS = "P_CDR_ALLOC_STATUS";

        #endregion

        #region Fields



        #endregion
    }
}
