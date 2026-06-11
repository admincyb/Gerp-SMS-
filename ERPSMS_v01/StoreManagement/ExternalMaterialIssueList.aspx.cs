using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessLogic.AccountManagement;
using BusinessObject;
using BusinessObject.Common;


namespace ERPSMS_v01.StoreManagement
{
    public partial class ExternalMaterialIssueList : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User objUser;
        public string IssuingAgainst = string.Empty;
        public string IssueNo = string.Empty;
        public string IssueTo = string.Empty;
        public string IssuingStore = string.Empty;        
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // imbAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                if (Session[SessionStrings.CurDept] != null)
                    hdfDeptID.Value = Session[SessionStrings.CurDept].ToString();
                else
                    hdfDeptID.Value = "0";
                if (Request.QueryString["TYPE"] != null)
                {
                    transactionType.Value = Request.QueryString["TYPE"].ToString();
                }
                SetResourse(transactionType.Value);
                SetSearchByddl(transactionType.Value);
                GetUserRights();
                hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";

                hdfFrom.Value = DateTime.Now.AddMonths(-1).AddDays(1 - DateTime.Now.Day).ToString(Resources.Constants.DateFormatShort);
                hdfTo.Value = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            }
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessId()
        {
            int procId = 0;
            string path = "/storemanagement/storerequisitionslipcreation.aspx";
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, objUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                procId = int.Parse(dtProcess.Rows[0]["PROCESS_PK"].ToString());
                hdfProcId.Value = procId.ToString();
            }
            return procId;

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
                //Breadcrumb Material Return
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "3")
                {
                    if (this.GetLocalResourceObject("BreadcrumbMaterialReturnList") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbMaterialReturnList").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("MaterialReturnTitleList").ToString();
                    }
                }
                else if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "7")
                {
                    if (this.GetLocalResourceObject("BreadcrumbFormerIssue") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbFormerIssue").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("BreadcrumbFormerIssue").ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Set Label based on Type
        /// </summary>
        /// <param name="type"></param>
        private void SetResourse(string type)
        {
            switch (type)
            {
                case "1":
                case "7":
                    IssuingAgainst = Resources.Controls.IssueAgainst;
                    IssueNo = Resources.Controls.IssueNo;   
                    IssueTo = Resources.Controls.IssueTo;                  
                    IssuingStore = Resources.Controls.IssuingStore;                    
                    break;
                case "3":
                    IssuingAgainst = Resources.Controls.ReturnAgainst;
                    IssueNo = Resources.Controls.ReturnNo;  
                    IssueTo = Resources.Controls.ReturnTo;                
                    IssuingStore = Resources.Controls.ReturningStore;                   
                    break;
            }
        }
        /// <summary>
        /// Set Search By ddl Based on Type
        /// </summary>
        /// <param name="type"></param>
        private void SetSearchByddl(string type)
        {
            switch (type)
            {
                case "1":
                    SearchType.Items.Add(new ListItem(Resources.Controls.IssueNo, "ICH_NO"));                   
                    break;
                case "3":
                    SearchType.Items.Add(new ListItem(Resources.Controls.ReturnNo, "ICH_NO"));                   
                    break;
            }
        }
        /// <summary>
        /// Method to Get User Rights
        /// </summary>
        private void GetUserRights()
        {
            //string path = "/StoreManagement/ExternalMaterialIssue.aspx?TYPE=1";
            string path = GetLocalResourceObject("PageURL") + "?TYPE=" + transactionType.Value;
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(objUser.PKUser, path, objUser.SBUID, objUser.CurrentDeptPK);          
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "MODIFY" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnModifyEMI.Value = "1";
                    }
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnCancelEMI.Value = "1";
                    }
                }

            }

        }
    }
}