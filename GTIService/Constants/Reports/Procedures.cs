using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GTIService.Constants.Reports
{
    public class Procedures
    {
        public const string GET_GRN_STOCK_DTLS_REPORT = "SPINV_STK_GRN_RPT";
        public const string GET_DAILY_STOCK_REPORT = "SPINV_STK_REGISTER_RPT";
        public const string GET_STORE_NAME = "SpwkfDepartmentGetKV";
        public const string SPADM_USER_DEPT_GET_KV = "SPADM_USER_DEPT_GET_KV";
        public const string SPINV_STK_TRX_VALUE_RPT = "SPINV_STK_TRX_VALUE_RPT";

        #region Purchase Request Report Procedures

        public const string GET_PR_REPORT = "SPPUR_REQUEST_RPT";
        public const string GET_PR_STATUS = "SPPUR_REQUEST_STATUS_GET_KV";
        public const string GET_PR_ITEMS = "SPPUR_REQUEST_DTL_GET";
        public const string GET_PR_VENDOUR = "SPPUR_REQUEST_VENDOR_GET";
        public const string GET_PR_CASHFLOW = "SPFIN_CASH_FLOW_GET";
        public const string RPT_PR_CASHFLOW = "SPFIN_CASH_FLOW_RPT";

        #endregion
    }
}
