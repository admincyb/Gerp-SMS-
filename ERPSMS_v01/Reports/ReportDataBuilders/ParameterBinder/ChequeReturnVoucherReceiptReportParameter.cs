using System;
using System.Collections.Generic;
using System.Data;
using ERPData;

namespace ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder
{
    public sealed class ChequeReturnVoucherReceiptReportParameter:IReportParameter
    {
        public DataTable dataTable { get; set; }
        public string TrxRefType { get; set; }
        public string PrintedUser { get; set; }
        public List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList { get; set; }
        public List<SPFIN_RECEIPT_VOUCHER_RPT_Result> HeaderDtls { get; set; }
        public List<SPFIN_TRX_VOUCHER_RPT_Result> AccountDtls { get; set; }
    }
}