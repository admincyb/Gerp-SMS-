using GTIService.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPSMS_v01.Reports.UserControls
{
    public interface IFilterControl
    {
        ReportParameters GetReportParameters();
    }
}
