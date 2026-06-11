using System;
using System.Data;
using System.Web;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StoreAuditListing : ERP.Store.UI.MyBasePage
    {
        #region Method
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // asssign User details to object
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // set add button visiblity by initial task permission 
                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
            }
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            // set entry screen path
            string path = GTIService.Constants.StockTransfer.Fields.STORE_Audit_Entry_Screen;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            // get process details by path and dept PK
            DataTable dtProcess = wrkfService.GetProcessID(path, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                // assign procID
                procId = int.Parse(dtProcess.Rows[0][GTIService.Constants.StockTransfer.Fields.PROCESSPK].ToString());
                hdfProcId.Value = procId.ToString();
            }
            // return ProcID
            return procId;

        }
        #endregion
    }
}