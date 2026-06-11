using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPData;
using System.Data;

namespace ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder
{
    public class CommonReportParameter : IReportParameter
    {
        public DataTable dataTable { get; set; }
        public string TrxRefType { get; set; }
        public string PrintedUser { get; set; }
        public List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList { get; set; }
        public List<SPFIN_TRX_VOUCHER_RPT_Result> reportDataSourceList { get; set; }
    }
}