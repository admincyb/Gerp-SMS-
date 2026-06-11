using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.CommonManagement;
using ERPManager;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.PurchaseRequestManagement
{
    public partial class PurchaseRequestListing : ERP.Store.UI.MyBasePage
    {

        BusinessObject.User objUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                hdfAppType.Value = ApplicationType.PR;
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
            string path = "/PurchaseRequestManagement/PurchaseRequestCreation.aspx";
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
            string path = "/PurchaseRequestManagement/PurchaseRequestCreation.aspx";
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(objUser.PKUser, path, objUser.SBUID);
            //if(usrRights.Rights.Count>0)
            //    if (usrRights.Rights[0].ActionName == "DELETE")
            //    {
            //        hdnClosePO.Value = "1";
            //    }
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight==true)
                    {
                        hdnModifyPR.Value = "1";
                    }
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true)//For Cancelling PR
                    {
                        hdnCancelPR.Value = "1";
                    } 
                }

            }

        }

        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {  
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";            
        }
    }
}