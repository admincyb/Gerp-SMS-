using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ERPData;
using System.Data;
using BusinessObject.Reports;
using System.Collections;
using ERPSMS_v01.Reports.ReportDataBuilders.ParameterBinder;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.Reports.ReportDataBuilders
{
    public abstract class ReportDataBuilder// <T> where T :IList
    {
        protected string RptType;
        public ReportDataBuilder(string rptType)
        {
            this.RptType = rptType;
        }
        public abstract string GetReportTemplate(IReportParameter reportParameter);
    }
}
