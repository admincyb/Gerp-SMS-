using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using GTIService.Constants.Common;
using BusinessObject.HRMS.Employee;
using BusinessLogic.HRMS.Employee;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.CommonManagement;
using System.IO;
using ERP.Utilities.HRMS;
using System.Web.UI.HtmlControls;
using ERPSMS_v01;

namespace HRMS.Employees
{
    public partial class EmpDoc : ERP.Store.UI.MyBasePage
    {



        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
           if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            //          ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCheckOutControlContainer]','" + Resources.PageNameRes.CheckOut + "','900','520');", true);
        }
        #endregion



    }
}