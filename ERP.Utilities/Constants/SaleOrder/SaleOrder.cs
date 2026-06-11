using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessObject.Constants
{
    public class SaleOrderDA
    {
        /// <summary>
        /// Sp names
        /// </summary>

        #region SPS
        public const string SP_GetSaleOrder = "SPPLN_WPR_ORDER_LIST_GET";
        public const string SP_SavePlanningDtl = "SPPLN_WPR_PLANNING_SAVE";
        public const string SP_SaveOrderList = "SPPLN_WPR_ORDER_LIST_SAVE";
        public static string GETSALEORDER = "SPSAL_ORDER_GET";
        public static string SPSAL_ORDER_GET_LISTING = "SPSAL_ORDER_GET_LISTING";
        public static string SAVESALEORDER = "SPSAL_ORDER_SAVE";
        public static string SAVESALEORDER_SHORT_CLS = "SPSAL_ORDER_SHORT_CLS";
        public static string SAVESALEORDERWKF = "SPSAL_ORDER_WKF_SAVE";
        public static string DELETESALEORDER = "SPSAL_ORDER_DELETE";
        public const string GETARCHIVE = "SPSAL_ORDER_ARCHIVE_GET_LIST";
        public const string SALEORDERCANCELCHECK = "SPSAL_ORDER_CANCEL_CHECK";
        public const string SPFIN_RECEIPT_CUS_TAX_DTL_GET_KV = "SPFIN_RECEIPT_CUS_TAX_DTL_GET_KV";
        public const string FIN_INVOICE_CUS_CUSTOMS_CHRG_GET_KV = "FIN_INVOICE_CUS_CUSTOMS_CHRG_GET_KV";
        public const string SP_SALE_ORDER_DTL_GET = "SPSAL_ORDER_DTL_GET";
        public const string SP_BIN_CARD_PACK_DTL_GET_KV = "SPPRD_BIN_CARD_PACK_DTL_GET_KV";
        public const string SPSAL_ORDER_MAIL_SAVE = "SPSAL_ORDER_MAIL_SAVE";
        public const string SPFIN_RECEIPT_CUS_RETURN_CHECK = "SPFIN_RECEIPT_CUS_RETURN_CHECK";
        public const string SPSAL_ORDER_UPDATE = "SPSAL_ORDER_UPDATE";
        public const string SPSAL_ORDER_DTL_REF_CHECK = "SPSAL_ORDER_DTL_REF_CHECK";
        public const string SPSAL_ORDER_COST_GET = "SPSAL_ORDER_COST_GET";
        public const string SPFIN_RECEIPT_CUS_SUSP_LIST = "SPFIN_RECEIPT_CUS_SUSP_LIST";
        public const string SPSAL_ORDER_HDR_SHIPMENT_TERM_GET = "SPSAL_ORDER_HDR_SHIPMENT_TERM_GET";
        public static string GETDEBITCREDITPOST = "SPFIN_CRDR_NOTE_VALIDATE";

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

        public const string P_WID = "P_WID";
        public const string P_RET_VALUE = "P_RET_VAL";
        public const string P_XML_ORD_LST = "P_XML_ORD_LST";
        public const string P_XML_PLD_DTL = "P_XML_PLD_DTL";
        public const string P_XML_PDT_MST = "P_XML_PDT_MST";
        public const string P_XML_SIZ_MST = "P_XML_SIZ_MST";
        public const string P_XML_CLR_MST = "P_XML_CLR_MST";
        public const string P_XML_PRO_MST = "P_XML_PRO_MST";
        public const string P_XML_ORD_HDR = "P_XML_ORD_HDR";
        public const string P_XML_PLN_SSN = "P_XML_PLN_SSN";
        public const string P_XML_PGP_MST = "P_XML_PGP_MST";
        public const string P_XML_CFG_LST = "P_XML_CFG_LST";       

        #endregion

        #region Fields



        #endregion
        /// <summary>
        
    }
}