using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.PurchaseOrderGenerates
{
    public class Procedures
    {
        public const string GETPENDINGPR = "SPPUR_ORDER_REQ_MAP_GET_LIST";
        public const string GETVENDORPRITEM = "SPPUR_VENDOR_PO_GET_KV";
        public const string SPPUR_ORDER_DTL_GET = "SPPUR_ORDER_DTL_GET";
        public const string SAVEPURCHASEORDER = "SPPUR_ORDER_SAVE";
        public const string SPPUR_ORDER_WKF_SAVE = "SPPUR_ORDER_WKF_SAVE";
        public const string GETSEARCHAUTO = "SPPUR_ORDER_REQ_MAP_AUTO";
        public const string GETCONFIGMASTER = "SPADM_CONFIG_MST_GET_KV";
        public const string GETVENDORITEMRATES = "SPPUR_ORDER_ITEM_PRICE_GET";
        public const string SPPUR_ORDER_GRN_QTY_CHECK = "SPPUR_ORDER_GRN_QTY_CHECK";
        public const string GETREVISIONHISTORY = "SPPUR_ORDER_ARCHIVE_GET_LIST";
        public const string GETPURORDERREQMAP = "SPPUR_ORDER_REQ_MAP_GET";


        public const string SPFIN_CRDR_INV_DEPT_GET = "SPFIN_CRDR_INV_DEPT_GET";
        public const string SPADM_PO_TYPE_GET_KV = "SPADM_PO_TYPE_GET_KV";

        #region Trading
        public const string SPPUR_ORDER_TRD_WKF_SAVE = "SPPUR_ORDER_TRD_WKF_SAVE";
        public const string GETPENDINGPRTRADING = "SPPUR_ORDER_TRD_REQ_MAP_GET_LIST";
        public const string GETSEARCHAUTOTRADING = "SPPUR_ORDER_TRD_REQ_MAP_AUTO";
        #endregion 
    
       
    }
}
