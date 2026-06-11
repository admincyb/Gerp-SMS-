using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.Common;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.StoreManagement
{
    public partial class StoreRequisitionSlipList : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Type"] != null) //for SBU Store request
                {
                    hdfType.Value = Request.QueryString["Type"];
                }
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                GetUserRights();
                ConfigurationSettings();

              
            }
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
            string PageUrl = Resources.PageURL.StoreRequisitionSlipURL;
            if (hdfType.Value=="2")
            {
                PageUrl = PageUrl + "?Type=" + hdfType.Value;
            }

            DataTable dtProcess = wrkfService.GetProcessID(PageUrl, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcId.Value = procId.ToString();
            }
            return procId;

        }

        private void GetUserRights()
        {
            BusinessLogic.CommonManagement.CommonBL userAuth = new BusinessLogic.CommonManagement.CommonBL();
            string PageUrl = Resources.PageURL.StoreRequisitionSlipURL;
            if (hdfType.Value == "2")
            {
                PageUrl = PageUrl + "?Type=" + hdfType.Value;
            }
            UserRightsBO usrRights = userAuth.GetWorkFlowUserRightsInfo(objUser.PKUser, PageUrl, objUser.CurrentDeptPK, 0);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "EDIT" && item.HasActionRight == true)
                    {
                        hdnModifyMR.Value = "1";
                    }
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true)
                    {
                        hdnCancelMR.Value = "1";
                    }
                }

            }

        }


    }
}