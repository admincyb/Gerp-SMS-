using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.Common;
using BusinessLogic.AccountManagement;
using BusinessObject.CommonManagement;
using BusinessObject;

namespace ERPSMS_v01.StoreManagement
{
    public partial class MaterialAcceptList : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                GetUserRights();
                ConfigurationSettings();                
            }
            btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
            hdfAppType.Value = ApplicationType.STA;
            hdfAppSubType.Value = string.Empty;
            BIZUNIT.Value = objUser.CurrentSBUPK.ToString();
            USER_PK.Value = objUser.PKUser.ToString();

        }
        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;            
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(Resources.PageURL.MaterialAcceptURL, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
            }
            return procId;

        }

        private void GetUserRights()
        {                    
            BusinessLogic.CommonManagement.CommonBL userAuth = new BusinessLogic.CommonManagement.CommonBL();
            UserRightsBO usrRights = userAuth.GetWorkFlowUserRightsInfo(objUser.PKUser, Resources.PageURL.MaterialAcceptURL, objUser.CurrentDeptPK, 0);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight == true)
                    {
                        hdnModify.Value = "1";
                    }
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true)
                    {
                        hdnCancel.Value = "1";
                    }
                }

            }

        }
    }
}