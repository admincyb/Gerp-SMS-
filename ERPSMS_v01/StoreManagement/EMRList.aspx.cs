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
    public partial class EMRList : ERP.Store.UI.MyBasePage
    {
        #region Variables
        BusinessObject.User objUser;
        public string ReceivingStore = string.Empty;
        public string ReceiptNo = string.Empty;
        public string ReceiveFrom = string.Empty;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                objUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                // imbAdd.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(objUser.PKUser, FillProcessId());
                if (Request.QueryString["TYPE"] != null)
                {
                    transactionType.Value = Request.QueryString["TYPE"].ToString();
                }
                SetResourse(transactionType.Value);
                SetSearchByddl(transactionType.Value);
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
            hdfSortColumn.Value = GetGlobalResourceObject("ConfigurationsRes", "SortColumn").ToString();
            hdfSortColumn1.Value = GetGlobalResourceObject("ConfigurationsRes", "SortColumn1").ToString();
            hdfSortOrder.Value = GetGlobalResourceObject("ConfigurationsRes", "SortOrder").ToString();
            hdfSortOrder1.Value = GetGlobalResourceObject("ConfigurationsRes", "SortOrder1").ToString();
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

        private void GetUserRights()
        {
            //string path = "/StoreManagement/ExternalMaterialReceive.aspx";
            string path = GetLocalResourceObject("PageURL").ToString();// +"?TYPE=" + transactionType.Value;
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(objUser.PKUser, path, objUser.SBUID, objUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                foreach (var item in usrRights.Rights)
                {
                    if (item.ActionName == "CANCEL" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdfCancelFlag.Value = "1";
                    }
                    if (item.ActionName == "MODIFY" && item.HasActionRight == true && item.UserDeptRight == true)
                    {
                        hdnModifyFlag.Value = "1";
                    }
                }
            }

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
                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() == "4")
                {
                    if (this.GetLocalResourceObject("BreadcrumbOpeningStockList") != null)
                    {
                        string breadCrumb;
                        breadCrumb = this.GetLocalResourceObject("BreadcrumbOpeningStockList").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                        lblBreadCrum.Text = breadCrumb;
                        Page.Title = GetLocalResourceObject("OpeningStockTitleList").ToString();
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
                case "2":
                    ReceivingStore = Resources.Controls.ReceivingStore;
                    ReceiptNo = Resources.Controls.MaterialReceiptNo;
                    ReceiveFrom = Resources.Controls.ReceiveFrom;
                    break;
                case "4":
                    ReceivingStore = Resources.Controls.Store;
                    ReceiptNo = Resources.Controls.OpeningStockNo;
                    ReceiveFrom = Resources.Controls.ReceiveFrom;
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
                case "2":
                    SearchType.Items.Add(new ListItem(Resources.Controls.ReceiptNo, "ICH_NO"));
                    break;
                case "4":
                    SearchType.Items.Add(new ListItem(Resources.Controls.OpeningStockNo, "ICH_NO"));
                    break;
            }
        }
    }
}