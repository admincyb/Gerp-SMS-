using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder;

namespace ERPSMS_v01.Reports.ReportDataBuilders
{
    public sealed class NullReportBuilder : ReportDataBuilder
    {
        public NullReportBuilder(string rptType) : base(rptType) { }

        public override string GetReportTemplate(IReportParameter reportParameter)
        {
            string returnToList = "backToList('../journalize/JournalizeListing.aspx?Type=" + RptType + "');";
            return returnToList + "printVoucher('<div>DotMatrix Report Builder Not Implemented for <b>" + RptType + "</b> Report.</div>',{ width:'21cm', height:'14cm', windowWidth:'400px', windowHeight:'300px' }," + "'../Css/petty-cash-style.css'" + ");";
        }
    }
}