using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.POInvoicing
{
    public class Procedures
    {
        //PO Invoice
        public const string GETINVOICEDETAILS = "SPFIN_INVOICE_VND_GET";
        public const string SAVEINVOICEDETAILS = "SPFIN_INVOICE_VND_SAVE";
        public const string SAVEINVOICEDETAILSWKF = "SPFIN_INVOICE_VND_WKF_SAVE";
        public const string SAVEEXPSETTLEMENTWKF = "SPFIN_EXPENSE_SET_WKF_SAVE";
        public const string DELETEINVOICEDETAILS = "SPFIN_INVOICE_VND_DELETE";
        public const string GETINVOICELIST = "SPFIN_INVOICE_VND_GET_LIST";
        public const string GETADDRESSTYPE = "SPPUR_VENDOR_CONTACT_GET_KV";
        public const string SPFIN_PAYMENT_VND_TAX_DTL_GET_KV = "SPFIN_PAYMENT_VND_TAX_DTL_GET_KV";
        public const string SPCHECKVALIDPOs = "SPFIN_INV_PO_VALD";
        public const string SPMULTIPLEPOGET = "SPFIN_INVOICE_VND_MULTIPLE_PO_GET";
        public const string SPMULTIPLEADVPOGET = "SPFIN_INVOICE_VND_ADV_TRADING_GET";
        public const string GETPAYMENTTYPE = "SPADM_CONFIG_MST_GET_KV";
        public const string GETEXPENSEADVANCE = "SPFIN_EXPENSE_SET_ADV_LIST";
        public const string SPPUR_ORDER_DTL_WO_ITEM_BOM_DTL_GET_LIST = "SPPUR_ORDER_DTL_WO_ITEM_BOM_DTL_GET_LIST";
        public const string SPINV_GRN_DTL_PO_WO_GET_LIST = "SPINV_GRN_DTL_PO_WO_GET_LIST";

        public const string GETBALANCEDETAILS = "SPFIN_INVOICE_VND_ALCN_GET";
        public const string SPPUR_ORDER_WORK_ORDER_AUTO = "SPPUR_ORDER_WORK_ORDER_AUTO"; 
        public const string SPFIN_INVOICE_VND_ARCHIVE_SAVE = "SPFIN_INVOICE_VND_ARCHIVE_SAVE";
        public const string SPFIN_INVOICE_CUS_ARCHIVE_SAVE = "SPFIN_INVOICE_CUS_ARCHIVE_SAVE";
        public const string SPFIN_CRDR_NOTE_ARCHIVE_SAVE = "SPFIN_CRDR_NOTE_ARCHIVE_SAVE";
        public const string SPPUR_ORDER_INVOICE_GROUP_VAL = "SPPUR_ORDER_INVOICE_GROUP_VAL";

        public static string SPFIN_INVOICE_VND_PYMT_GET_LIST = "SPFIN_INVOICE_VND_PYMT_GET_LIST";
        public static string SPPUR_ORDER_INVOICE_VND_GET_LIST = "SPPUR_ORDER_INVOICE_VND_GET_LIST";

        public static string SPPUR_ORDER_GET_LIST = "SPPUR_ORDER_GET_LIST";
        public const string SPFIN_INVOICE_VND_PAYMENT_GET = "SPFIN_INVOICE_VND_PAYMENT_GET";
        public const string SPFIN_INV_COST_CENTER_GET = "SPFIN_INV_COST_CENTER_GET";
        public const string SPINV_INVOICE_CONVERT_SAVE = "SPINV_INVOICE_CONVERT_SAVE";

        public const string SPPUR_ORDER_DTL_NOS_GET = "SPPUR_ORDER_DTL_NOS_GET";

        public const string SPFIN_INVOICE_VND_ADV_WKF_SAVE = "SPFIN_INVOICE_VND_ADV_WKF_SAVE";
        public const string SPFIN_INVOICE_VND_ADV_GET = "SPFIN_INVOICE_VND_ADV_GET";
        public const string SPFIN_INVOICE_VND_ADV_GET_LIST = "SPFIN_INVOICE_VND_ADV_GET_LIST";
        public const string SP_GET_DAILY_INSPECTION_DETAILS_REPORT = "SPQUC_TEST_TRX_ITEM_DTL_RPT";
        public const string SPQUC_TEST_TRX_ITEM_PM_DTL_RPT = "SPQUC_TEST_TRX_ITEM_PM_DTL_RPT";
        public const string SPFIN_CRDR_NOTE_PURCHASE_VALIDATE = "SPFIN_CRDR_NOTE_PURCHASE_VALIDATE";


        #region Purchase Invoice Trading
        public const string SPFIN_INVOICE_VND_TRADING_PEND_PO_GET = "SPFIN_INVOICE_VND_TRADING_PEND_PO_GET";
        public const string SPFIN_INVOICE_VND_TRADING_PEND_PO_AUTO = "SPFIN_INVOICE_VND_TRADING_PEND_PO_AUTO";
        #endregion


        #region Trading
        public const string SPFIN_INVOICE_VND_TRADING_WKF_SAVE = "SPFIN_INVOICE_VND_TRADING_WKF_SAVE";
		public const string SPFIN_INVOICE_VND_TRADING_DELETE = "SPFIN_INVOICE_VND_TRADING_DELETE";
        public const string SPFIN_INVOICE_VND_TRADING_GET = "SPFIN_INVOICE_VND_TRADING_GET";
        public const string SPFIN_INVOICE_VND_TRADING_GET_LIST = "SPFIN_INVOICE_VND_TRADING_GET_LIST";
        public const string SPFIN_PUR_DIR_INVOICE_NO_AUTO = "SPFIN_PUR_DIR_INVOICE_NO_AUTO";
        public const string SPFIN_INVOICE_VND_MULTIPLE_PO_TRADING_GET = "SPFIN_INVOICE_VND_MULTIPLE_PO_TRADING_GET";
         
	    #endregion

        #region Debit/Credit Trading
        public const string SPFIN_INVOICE_VND_TRADING_CRDR_PENDING_GET = "SPFIN_INVOICE_VND_TRADING_CRDR_PENDING_GET";
        public const string SPFIN_CRDR_NOTE_VND_GET="SPFIN_CRDR_NOTE_VND_GET";
        public const string SPFIN_INVOICE_VND_TRADING_CRDR_AUTO = "SPFIN_INVOICE_VND_TRADING_CRDR_AUTO";
        public const string SPFIN_CRDR_VND_WKF_SAVE="SPFIN_CRDR_VND_WKF_SAVE";
        public const string SPFIN_CRDR_VND_GET_LIST="SPFIN_CRDR_VND_GET_LIST";
        public const string SPFIN_CRDR_NOTE_HDR_DELETE="SPFIN_CRDR_NOTE_HDR_DELETE";
        public const string SPFIN_CRDR_NOTE_NO_AUTO = "SPFIN_CRDR_NOTE_NO_AUTO";
        public const string SPFIN_CRDR_NOTE_VND_CAN_CHECK = "SPFIN_CRDR_NOTE_VND_CAN_CHECK";       
	    #endregion        
       #region Payment Trading
		 public const string SPFIN_PAYMENT_VND_PEND_INVOICE_GET="SPFIN_PAYMENT_VND_PEND_INVOICE_GET";
         public const string SPFIN_PAYMENT_VND_HDR_GET = "SPFIN_PAYMENT_VND_HDR_GET";
         public const string SPFIN_PAYMENT_VND_WKF_SAVE = "SPFIN_PAYMENT_VND_WKF_SAVE";
         public const string SPFIN_PAYMENT_VND_ALCN_GET="SPFIN_PAYMENT_VND_ALCN_GET";
         public const string SPFIN_PAYMENT_VND_HDR_GET_LIST="SPFIN_PAYMENT_VND_HDR_GET_LIST";
         public const string SPFIN_PAYMENT_TRADING_NO_AUTO = "SPFIN_PAYMENT_TRADING_NO_AUTO";
         public const string SPFIN_PAYMENT_VND_HDR_DELETE="SPFIN_PAYMENT_VND_HDR_DELETE" ;
         public const string SPFIN_INVOICE_VND_TRADING_INVOICE_NO_AUTO="SPFIN_INVOICE_VND_TRADING_INVOICE_NO_AUTO";
        
	   #endregion

         
    }
}
