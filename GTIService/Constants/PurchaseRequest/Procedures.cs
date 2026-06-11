using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.PurchaseRequest
{
    public class Procedures
    {
        // For Listing Page
        public const string GETPURCHASERQSTDTLS = "SPPUR_REQUEST_LIST";
        public const string GETMATERIALRQSTDTLS = "SPINV_MATERIAL_REQUEST_LIST";

        public const string GETPURCHASERQSTDTLSREPORT = "SPPUR_REQUEST_GET_RPT";
        public const string DELETEPURCHASERQST = "SPPUR_REQUEST_DELETE";
        public const string DELETEMRRQST = "SPINV_MATERIAL_REQUEST_DELETE";
        public const string DELETEMRRQSTP2P = "SPINV_MATERIAL_REQUEST_INTER_PLANT_DELETE";
        public const string DELETEMATERIALRQST = "SPINV_MATERIAL_REQUEST_DELETE";
        public const string PURCHASERQSTAUTO = "SPPUR_REQUEST_AUTO";
        public const string MATERIALRQSTAUTO = "SPINV_MATERIAL_REQUEST_AUTO";
        // For Request Creation
        public const string GETPURCHASERQSTDETAILS = "SPPUR_REQUEST_GET";
        public const string GETPURCHASERQSTDETAILSPLAN = "SPPUR_PRD_PLAN_ITEM_DTL_REQUEST_GET"; 
        public const string GETMATERIALRQSTDETAILSREPORT = "SPINV_MATERIAL_REQUEST_RPT";
        public const string GETMATERIALRQSTDETAILS = "SPINV_MATERIAL_REQUEST_GET";
        public const string GETPRNO = "SPPUR_REQUEST_NO_GET";
        public const string GETPURCHASEPENDINGREQUEST = "SPINV_ITEM_REORDER_LIST";
        public const string SPINV_PACK_MAT_REQ_GET = "SPINV_PACK_MAT_REQ_GET";
        public const string SPINV_ITEM_CUST_ITEM_REQ_GET = "SPINV_ITEM_CUST_ITEM_REQ_GET";
        public const string SPINV_STOCK_VALIDATE = "SPINV_STOCK_VALIDATE";

        public const string SAVEPURCHASEREQUEST = "SPPUR_REQUEST_SAVE";
        public const string SAVEMATERIALREQUEST = "SPINV_MATERIAL_REQUEST_SAVE";
        public const string SAVEMATERIALREQUESTP2P = "SPINV_MATERIAL_REQUEST_INTER_PLANT_SAVE";
        //For Purchase Request Trading List
        public const string GETPURCHASERQSTTRDLISTDTLS = "SPPUR_REQUEST_TRD_GET_LIST"; //SPPUR_REQUEST_LIST(old)
        public const string DELETEPURCHASERQSTTRDLIST = "SPPUR_REQUEST_TRD_DELETE";//SPPUR_REQUEST_DELETE(old)
        public const string PURCHASERQSTTRDLISTAUTO = "SPPUR_REQUEST_TRD_AUTO";//SPPUR_REQUEST_AUTO(old)
        //For Purchase Request Trading
        public const string GETPURCHASERQSTTRDDETAILS = "SPPUR_REQUEST_TRD_GET";//SPPUR_REQUEST_GET
        public const string GETPURCHASEPENDINGREQUESTTRD = "SPPUR_REQUEST_TRD_REORDER_LIST";//SPINV_ITEM_REORDER_LIST
        public const string SAVEPURCHASEREQUESTTRD = "SPPUR_REQUEST_TRD_SAVE";//SPPUR_REQUEST_SAVE
        public const string SPSAL_ORDER_INT_PEND_GET_KV = "SPSAL_ORDER_INT_PEND_GET_KV";
        public const string SPPUR_REQUEST_LIST_DTL_GET = "SPPUR_REQUEST_LIST_DTL_GET";

        public const string SPPRD_BIN_CARD_COMP_BATCH_DTL_RPT = "SPPRD_BIN_CARD_COMP_BATCH_DTL_RPT";
        public const string SPFIN_COSTCENTERWISE_LEDGER_DETAILS = "SPFIN_COSTCENTERWISE_LEDGER_DETAILS";
        public const string SPPRD_BIN_CARD_COMP_BATCH_DTL_RPT_NEW = "SPPRD_BIN_CARD_COMP_BATCH_DTL_RPT_NEW";

        public const string SPPUR_BUDGET_BALANCE_GET = "SPPUR_BUDGET_BALANCE_GET";
    }
}
