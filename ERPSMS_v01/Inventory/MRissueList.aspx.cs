using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.Inventory
{
    public partial class MRissueList : ERP.Store.UI.MyBasePage
    {
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (!IsPostBack)
            {
                if (Request.QueryString["TYPE"] != null && Convert.ToInt32(Request.QueryString["TYPE"]) > 0)
                    hdfMenuType.Value = Request.QueryString["TYPE"];
                GetUserRights();
                ConfigurationSettings();
            }
            btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            if (!IsPostBack)
            {
                //assign Page BreadCrumb
                base.OnLoadComplete(e);
                AssignLocalBreadCrumb();
            }
        }

        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            if (GetGlobalResourceObject("ConfigurationsRes", "IsVisiblethCostCenter").ToString() == "1")
            {
                thCostCenter.Visible = true;
            }
            else
            {
                thCostCenter.Visible = false;
            }
        }

        public void AssignLocalBreadCrumb()
        {
            try
            {
                string _breadCrumb = string.Empty;
                string breadCrumb = string.Empty;
                if (GetGlobalResourceObject("ConfigurationsRes", "HasPlantwiseBreadcrumb").ToString() == "1")
                {
                    string plant = string.Empty;
                    if (objUser.CurrentDept.Split('-').ElementAtOrDefault(1) != null)
                        plant = objUser.CurrentDept.Split('-')[1].ToString().Trim();

                    if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() == "9")
                        _breadCrumb = string.Format(this.GetLocalResourceObject("BreadcrumbP2P_PName").ToString(), plant);
                    else
                        _breadCrumb = string.Format(this.GetLocalResourceObject("Breadcrumb").ToString(), plant);

                    breadCrumb = _breadCrumb.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    lblBreadCrum.Text = breadCrumb;
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            string path = GetLocalResourceObject("MaterialIssueURL").ToString();// "/storemanagement/materialissue.aspx";
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
            string path = GetLocalResourceObject("MaterialIssueURL").ToString();// "/storemanagement/materialissue.aspx";
            BusinessLogic.CommonManagement.CommonBL userAuth;
            userAuth = new BusinessLogic.CommonManagement.CommonBL();
            // get the user WorkFlow Rights
            UserRightsBO usrWorkflowRights = userAuth.GetWorkFlowUserRightsInfo(objUser.PKUser, path, objUser.CurrentDeptPK, 0);
            if (usrWorkflowRights != null || usrWorkflowRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrWorkflowRights.Rights.Count; i++)
                {
                    if (usrWorkflowRights.Rights[i].ActionName == "EDIT" && usrWorkflowRights.Rights[i].HasActionRight == true)
                    {
                        hdnModify.Value = "1";
                    }
                    if (usrWorkflowRights.Rights[i].ActionName == "CANCEL" && usrWorkflowRights.Rights[i].HasActionRight == true)
                    {
                        hdnCancel.Value = "1";
                    }
                }
            }

        }
    }
}