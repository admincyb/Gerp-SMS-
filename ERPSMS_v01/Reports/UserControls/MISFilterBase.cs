using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GTIService.Dashboard;

namespace ERPSMS_v01.Reports.UserControls
{
    public class MISFilterBase : System.Web.UI.UserControl, IFilterControl
    {
        #region EventHandler

        #endregion

        #region Methods

        public virtual ReportParameters GetReportParameters()
        {
            return null;
        }

        #endregion
    }
}