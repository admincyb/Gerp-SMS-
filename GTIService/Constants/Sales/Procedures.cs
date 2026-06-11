using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Sales
{
    public class Procedures
    {
        public const string GET_BRAND_DETAILS = "SPCRM_CUST_ITEM_MAP_GET";
        public const string GET_CUSTOMERPRODUCT = "SPCRM_CUST_ITEM_MAP_GET_KV";
        public const string SPINV_ITEM_CATEGORY_GET = "SPINV_ITEM_CATEGORY_GET";
        public const string GET_CUSTOMER = "SPCRM_CUSTOMER_MST_GET_KV";
        public const string GET_CUSTOMERBRANDLIST = "SPCRM_CUST_ITEM_RATE_CUS_GET";
        public const string GET_ITMBRANDLIST = "SPCRM_CUST_ITEM_RATE_ITM_GET";
        public const string GET_MAILQ_PARTY = "SPADM_MAIL_QUEUE_PARTY_GET";
        public const string SAVE_ENQUIRY = "SPCRM_ENQUIRY_SAVE";
        public const string GET_ENQUIRYNO_AUTO = "SPCRM_ENQUIRY_AUTO";
        public const string SAVEQUOTATION="SPCRM_QUOTATION_SAVE";
        public const string GETQUOTATIONARCHIVE = "SPCRM_QUOTATION_ARCHIVE_GET_LIST";
        public const string GET_ENQUIRY_LIST = "SPCRM_ENQUIRY_GET_LIST";
        public const string GET_ENQUIRY_DTL = "SPCRM_ENQUIRY_GET";
        public const string DELETEENQUIRY = "SPCRM_ENQUIRY_DELETE";
        public const string GETCUSTOMERADDRESS = "SPCRM_CUST_ADDRESS_GET_KV";
        public const string GENERATESALEORDER = "SPSAL_ORDER_CONV_SAVE";
        public const string SPFIN_AGENT_CUSTOMER_MAP_GET_KV = "SPFIN_AGENT_CUSTOMER_MAP_GET_KV";
        public const string GETITEMDETAILS = "SPINV_ITEM_MST_GET";
        public const string GET_ITEM_RATES = "SPCRM_CUSTOMER_ITEM_RATES_GET_LIST";
        public const string GETCUSTOMERTERMS = "SPCRM_CUST_TERM_HDR_GET_KV";
        public const string GETBANKDETAILS = "SPFIN_CASH_BANK_MST_GET_KV";

        public const string GETCBMWEIGHT = "SPCRM_CBM_WGT_GET";

        public const string GET_BRANDSBYSC = "SPSAL_ORDER_SHIP_BRAND_GET";
        public const string GET_LOTNOBYLOADPLANPK = "SPSAL_LOADING_PLAN_DTL_GET_LIST";
        public const string GET_LOTNOBYSHPPLANPK = "SPSAL_SHIPPING_PLAN_DTL_GET_KV";
        //Sales Invoice
        public const string GETINVOICEDETAILS = "SPFIN_INVOICE_CUS_GET";
        public const string SAVEINVOICEDETAILS = "SPFIN_INVOICE_CUS_SAVE";
        public const string DELETEINVOICEDETAILS = "SPFIN_INVOICE_CUS_DELETE";
        public const string GETSALESINVOICELIST = "SPFIN_INVOICE_CUS_GET_LIST";
        public const string GET_ITEM_RATE_LIST = "SPCRM_CUST_ITEM_RATE_HDR_GET_KV";
        public const string GET_CUSTOMER_LIST = "SPCRM_CUSTOMERS_MST_LIST_GET";
        public const string GET_DUEDATE_BY_PAYMENTTERMS = "SPCRM_CUST_TERM_CALC";
        public const string SPFIN_INVOICE_CUS_ALCN_GET = "SPFIN_INVOICE_CUS_ALCN_GET";
        public const string GETINVOICEDETAILSMUL = "SPFIN_INVOICE_CUS_MULTIPLE_SO_GET";
        public const string SPFIN_INVOICE_CUS_ITEM_ISSUE_GET = "SPFIN_INVOICE_CUS_ITEM_ISSUE_GET";
        public const string SPFIN_INVOICE_CUS_PENDING_GET = "SPFIN_INVOICE_CUS_PENDING_GET";
        public const string SPFIN_INVOICE_CUS_MULTIPLE_SO_TRADING_GET = "SPFIN_INVOICE_CUS_MULTIPLE_SO_TRADING_GET";
        public const string SPFIN_INVOICE_CUS_TRADING_WKF_SAVE = "SPFIN_INVOICE_CUS_TRADING_WKF_SAVE";
        public const string SPFIN_INVOICE_CUS_TRADING_DELETE = "SPFIN_INVOICE_CUS_TRADING_DELETE";
        public const string SPFIN_INVOICE_CUS_PENDING_DO_AUTO = "SPFIN_INVOICE_CUS_PENDING_DO_AUTO";
        public const string SPFIN_INVOICE_CUS_TRADING_GET_LIST = "SPFIN_INVOICE_CUS_TRADING_GET_LIST";
        public const string GETVERIFICATIONDETAILSMUL = "SPFIN_INVOICE_TAX_GET";


        //Art Work
        public const string GETARTWORK = "SPCRM_CUST_ITEM_ART_DTL_GET_KV";

        //Misc Invoice
        public const string GETCUSTOMERSUPPLYTAX = "SPCRM_CUST_TAX_DTL_GET_KV";
     
        //AGENT COMMISSION
        public const string SPFIN_INVOICE_CUS_AGENT_GET_LIST = "SPFIN_INVOICE_CUS_AGENT_GET_LIST";
        public const string SPFIN_INVOICE_CUS_AGENT_AUTO = "SPFIN_INVOICE_CUS_AGENT_AUTO";
        public const string SPFIN_INVOICE_VND_AGENT_GET = "SPFIN_INVOICE_VND_AGENT_GET";
        public const string SPFIN_INVOICE_VND_AGENT_SAVE = "SPFIN_INVOICE_VND_AGENT_SAVE";

        public const string SPFIN_INVOICE_VND_AGENT_DTL_VAL = "SPFIN_INVOICE_VND_AGENT_DTL_VAL";
        public const string SPFIN_INVOICE_VND_AGENT_GET_LIST = "SPFIN_INVOICE_VND_AGENT_GET_LIST";
        public const string SPFIN_INVOICE_VND_AGENT_DELETE = "SPFIN_INVOICE_VND_AGENT_DELETE";
        public const string SPFIN_INVOICE_VND_AGENT_AUTO = "SPFIN_INVOICE_VND_AGENT_AUTO";
        public const string SPFIN_INVOICE_VND_AGENT_PRINT = "SPFIN_INVOICE_VND_AGENT_RPT";
        //Mul DO pick for invoicing
        public const string SPFIN_INV_SO_VALD = "SPFIN_INV_SO_VALD";
        
        //DIRECT SALE ORDER
        public const string SPSAL_ORDER_DIR_WKF_SAVE = "SPSAL_ORDER_DIR_WKF_SAVE";
        public const string SPSAL_ORDER_DIR_GET_LIST = "SPSAL_ORDER_DIR_GET_LIST";
        public const string SPSAL_ORDER_DIR_GET = "SPSAL_ORDER_DIR_GET";
        public const string SPSAL_ORDER_DIR_DELETE = "SPSAL_ORDER_DIR_DELETE";
        public const string SPSAL_ORDER_DIR_DTL_GET = "SPSAL_ORDER_DIR_DTL_GET";
        public const string SPSAL_ORDER_DIR_AUTO = "SPSAL_ORDER_DIR_AUTO";
        public const string SPSAL_SO_DIRECT_CANCEL_CHECK = "SPSAL_SO_DIRECT_CANCEL_CHECK";
        public const string SPSAL_ORDER_DIR_PEND_SO_GET = "SPSAL_ORDER_DIR_PEND_SO_GET";
        public const string SPSAL_ORDER_DIR_SO_PEND_AUTO = "SPSAL_ORDER_DIR_SO_PEND_AUTO";
        public const string SAVEDIRECTSALEORDER_SHORT_CLS = "SPSAL_ORDER_DIR_SHORT_CLS";
        public const string SPFIN_SAL_DIR_INVOICE_NO_AUTO = "SPFIN_SAL_DIR_INVOICE_NO_AUTO";
        public const string SPFIN_INVOICE_TRADING_PEND_INV_GET = "SPFIN_INVOICE_TRADING_PEND_INV_GET";
        public const string SPFIN_RECEIPT_CUS_HDR_GET = "SPFIN_RECEIPT_CUS_HDR_GET";
        public const string SPFIN_RECEIPT_CUS_ALCN_GET = "SPFIN_RECEIPT_CUS_ALCN_GET";
        public const string SPFIN_RECEIPT_CUS_WKF_SAVE = "SPFIN_RECEIPT_CUS_WKF_SAVE";
        public const string SPFIN_RECEIPT_CUS_HDR_DELETE = "SPFIN_RECEIPT_CUS_HDR_DELETE";
        public const string SPFIN_RECEIPT_CUS_GET_LIST = "SPFIN_RECEIPT_CUS_GET_LIST";
        public const string SPFIN_CRDR_CUS_PEND_INVOICE_GET = "SPFIN_CRDR_CUS_PEND_INVOICE_GET";
        public const string SPFIN_CRDR_NOTE_CUS_TRD_GET = "SPFIN_CRDR_NOTE_CUS_TRD_GET";
        public const string SPFIN_CRDR_CUS_TRD_WKF_SAVE = "SPFIN_CRDR_CUS_TRD_WKF_SAVE";
        public const string SPFIN_CRDR_CUS_TRD_GET_LIST = "SPFIN_CRDR_CUS_TRD_GET_LIST";
        public const string SPCRM_CUST_ITEM_MAP_BD_GET_LIST = "SPCRM_CUST_ITEM_MAP_BD_GET_LIST";
        public const string SPSAL_INVOICE_CUS_AGENT_AUTO = "SPSAL_INVOICE_CUS_AGENT_AUTO";

        //CRM
        public const string SPCRM_LEAD_QUOTE_HDR_AUTO = "SPCRM_LEAD_QUOTE_HDR_AUTO";
        public const string SPCRM_LEAD_QUOTE_HDR_SO_MPG_GET = "SPCRM_LEAD_QUOTE_HDR_SO_MPG_GET";
        public const string SPSAL_ORDER_CRM_CUSTOMER_VALIDATE = "SPSAL_ORDER_CRM_CUSTOMER_VALIDATE";
    }
}
