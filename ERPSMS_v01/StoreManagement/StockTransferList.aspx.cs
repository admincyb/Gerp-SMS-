using System;
using System.Data;
using System.Web;
using BusinessLogic.AccountManagement;
using BusinessObject;
using BusinessObject.Common;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StockTransferList : ERP.Store.UI.MyBasePage
    {

        #region Methods
        BusinessObject.User objUser;
        /// <summary>
        /// For Page Load Action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // asssign User details to object
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                // set add button visiblity by initial task permission 
                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                GetUserRights();
                ConfigurationSettings();
            }
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            // set entry screen path
            string path = GTIService.Constants.StockTransfer.Fields.ST_Entry_Screen;
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
        private void GetUserRights()
        {
            string path = "/StoreManagement/StockTransfer.aspx";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(objUser.PKUser, path, objUser.SBUID, objUser.CurrentDeptPK);          
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnModifySA.Value = "1";
                    }
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnCancelSA.Value = "1";
                    }
                }
            }
        }

        /// <summary>
        /// ConfigurationSettings
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
        }       
        #endregion
    }
}