using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using BusinessObject.CommonManagement;

namespace ERPSMS_v01.PurchaseOrderManagement
{
    public partial class PurchaseOrderNonStockListing : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                hdnShortCloseGroup.Value = ConfigurationManager.AppSettings["ShortCloseGroup"].ToString();
                hdnRoleID.Value = objUser.Roles;
                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                hdfAppType.Value = ApplicationType.PO;
                hdfAppSubType.Value = ((int)AppSubTypePO.NONSTOCK).ToString();
            }
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            string path = "/PurchaseOrderManagement/PurchaseOrderNonStock.aspx?TYPE=1";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcId.Value = procId.ToString();
            }
            return procId;
        }
    }
}