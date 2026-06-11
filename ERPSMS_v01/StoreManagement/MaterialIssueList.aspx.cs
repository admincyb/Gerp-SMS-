using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic.AccountManagement;
using BusinessObject;

namespace ERPSMS_v01.StoreManagement
{
    public partial class MaterialIssueList : ERP.Store.UI.MyBasePage
    {
        public int PageType
        {
            get
            {
                return this.ViewState["PageType"] == null ? 0 : (int)(this.ViewState["PageType"]);
            }
            set
            {
                this.ViewState["PageType"] = value;
            }
        }
        BusinessObject.User objUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            if (Request.QueryString["Type"] != null)

            PageType = Convert.ToInt32(Request.QueryString["Type"]);
           
            if (!IsPostBack)
            {
                GetUserRights();
                ConfigurationSettings();
            }
            btnAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
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
        /// Assign breadCrumb
        /// </summary>
        public void AssignLocalBreadCrumb()
        {
            try
            {
                //Breadcrumb WO
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "1")
                {
                    if (this.GetLocalResourceObject("BreadcrumbWO") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbWO").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("WOTitle").ToString();
                    }
                }
                else if(Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "3")
                {
                    if (this.GetLocalResourceObject("BreadcrumbSBUReturnList") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbSBUReturnList").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("Title").ToString();
                    }
                }
                else
                {
                    if (this.GetLocalResourceObject("Breadcrumb") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("Title").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// Get Configuration Value
        /// </summary>
        private void ConfigurationSettings()
        {
            string MIH_NO_Text = Resources.BindValues.STNo;
            string MIH_MRH_NO_Text = Resources.BindValues.SR;
            if (Request.QueryString["Type"] != null)
            {
                MIH_NO_Text = Request.QueryString["Type"] == "1" ? Resources.BindValues.WOTNo : Resources.BindValues.STNo; //MIH_NO
                MIH_MRH_NO_Text = Request.QueryString["Type"] == "1" ? Resources.BindValues.WOR : Resources.BindValues.SR;
            }
            SearchType.Items.Insert(0, new ListItem(MIH_NO_Text, "MIH_NO"));
            SearchType.Items.Insert(4, new ListItem(MIH_MRH_NO_Text, "MIH_MRH_NO"));

            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            string type = string.Empty;
            string path = GetLocalResourceObject("MaterialIssueURL").ToString();// "/storemanagement/materialissue.aspx";
            if (Request.QueryString["Type"] != null)
            {
                path += "?Type=" + Request.QueryString["Type"].ToString();
                hdfMenuType.Value= Request.QueryString["Type"].ToString();
            }
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
            if (Request.QueryString["Type"] != null)
            {
                path += "?Type=" + Request.QueryString["Type"].ToString();
            }
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