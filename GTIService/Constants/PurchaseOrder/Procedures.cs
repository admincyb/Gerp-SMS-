using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.PurchaseOrder
{
    public class Procedures
    {
        public const string GETPURCHSENO = "SPPUR_ORDER_NO_GET";
        public const string SAVEPURCHASEORDER = "SPPUR_ORDER_SAVE";
        public const string GETPURCHASEORDER = "SPPUR_ORDER_GET";
        public const string GETPRDETAILS = "SPPUR_REQ_ITEMS_GET";

       


        //For Listing
        public const string GETSEARCHVALUE = "SPPUR_ORDER_AUTO";
        public const string GETPODETILSLIST = "SPPUR_ORDER_GET_LIST_WRKF";
        public const string DELETEPODETAILS = "SPPUR_ORDER_DELETE";
        public const string POSHORTCLOSE = "SPPUR_ORDER_SHORT_CLS";
        public const string GETPODETILSVIEW = "SPPUR_ORDER_ITEMS_VIEW";

        public const string GETPENDINGPOLIST = "SPPUR_ORDER_PEND_LIST";
        public const string SPPUR_ORDER_VENDOR_GET = "SPPUR_ORDER_VENDOR_GET";
        public const string SPPUR_ORDER_GET = "SPPUR_ORDER_GET";
        public const string SPPUR_ORDER_LIST_DTL_GET = "SPPUR_ORDER_LIST_DTL_GET";

        #region Trading
        public const string GETPODETILSLISTTRADING = "SPPUR_ORDER_TRD_GET_LIST";
        public const string GETPURCHASEORDERTRADING = "SPPUR_ORDER_TRD_GET";
        #endregion

        public const string SPPUR_ORDER_HDR_PROJECT_BUDGET_BAL_GET = "SPPUR_ORDER_HDR_PROJECT_BUDGET_BAL_GET";
    }
}
