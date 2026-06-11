using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessLogic.AccountManagement;
using BusinessObject;
using BusinessObject.Common;

namespace ERPSMS_v01.StoreManagement
{
    public partial class GRNList : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {

                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";

                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                hdfAppType.Value = ApplicationType.GRN;
                hdfAppSubType.Value = string.Empty;
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
            string path = "/storemanagement/grncreate.aspx";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcId.Value = procId.ToString();
            }
            return procId;

        }

        private void GetUserRights()
        {
            string path = "/storemanagement/grncreate.aspx";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(objUser.PKUser, path, objUser.SBUID, objUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnModifyGRN.Value = "1";
                    }
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnCancelGRN.Value = "1";
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
    
    }
}