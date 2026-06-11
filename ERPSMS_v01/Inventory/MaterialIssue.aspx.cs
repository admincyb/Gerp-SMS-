using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using System.Data;
using ERPSMS_v01.Administration.Masters;
using BusinessObject.Common;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using BusinessObject.Finance.Administration.Masters;
using BusinessLogic.Finance.Administration.Masters;
using BusinessObject.CommonManagement;
using DataAccess.SubDepartmentManagement;
using BusinessObject.MaterialManagement;
using System.Xml;
using BusinessLogic.MaterialManagement;
using BusinessLogic.AccountManagement;
using System.Threading;

namespace ERPSMS_v01.Inventory
{
    public partial class MaterialIssue : ERP.Store.UI.MyBasePage
    {
        #region Variables & Properties

        #region Properties

        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }
        private DateTime LastModifiedTime
        {
            get
            {
                return this.ViewState[ViewstateStrings.LastModifiedTime] == null ? System.DateTime.Now : (DateTime)this.ViewState[ViewstateStrings.LastModifiedTime];
            }
            set
            {
                this.ViewState[ViewstateStrings.LastModifiedTime] = value;
            }
        }
        private int PageIndex
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }
        private int TotalPages
        {
            get
            {
                return (int)(this.ViewState[ViewstateStrings.TotalPages] ?? 1);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
            }
        }
        private int CurrPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrPK] = value;
            }
        }
        private int BatchNoPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.BatchNoPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.BatchNoPK] = value;
            }
        }
        /// <summary>
        /// To keep RowIndex in view state
        /// </summary>
        private int RowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }
        /// <summary>
        /// To keep Leave Details List in view state
        /// </summary>
        private List<EMIMultipleDetails> EMIMultipleDetailsList
        {
            get
            {
                return ViewState[ViewstateStrings.EMIMultipleDetailsList] == null ? new List<EMIMultipleDetails>() : (List<EMIMultipleDetails>)ViewState[ViewstateStrings.EMIMultipleDetailsList];
            }
            set
            {
                ViewState[ViewstateStrings.EMIMultipleDetailsList] = value;
            }
        }

        private int RowNumber
        {
            get
            {
                return this.ViewState["RowNumber"] == null ? 0 : Convert.ToInt32(this.ViewState["RowNumber"]);
            }
            set
            {
                this.ViewState["RowNumber"] = value;
            }
        }
        /// <summary>
        /// keep Row Edit Mode value
        /// </summary>
        private bool RowEditMode
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowEditMode] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.RowEditMode]);
            }
            set
            {
                this.ViewState[ViewstateStrings.RowEditMode] = value;
            }
        }
        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }

        private int ProcessID
        {
            get
            {
                return this.ViewState["ProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["ProcessID"].ToString());
            }
            set
            {
                this.ViewState["ProcessID"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool SendMailWithAttachment
        {
            get
            {
                return this.ViewState[ViewstateStrings.SendMailWithAttachment] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.SendMailWithAttachment].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.SendMailWithAttachment] = value;
            }
        }

        /// <summary>
        /// Currency Format String
        /// </summary>
        private string CurrencyFormatString
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] == null ?
                    String.Format("{{0:c{0}}}", GetLocalResourceObject("QtyDecimal")) //Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)
                    : (string)ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] = value;
            }
        }
        #endregion

        #region Variables

        User currentUser;
        public DataTable dtIssuingStore;
        public DataTable dtIssueAgainst;
        public DataTable dtType;
        public DataSet dsEMIMultipleList;
        private DataTable dtResult;
        private DataTable dt;
        private DataTable dtCurrentStock;
        public int result;
        private ActionsEnum commonActions;
        public string subType;
        private DataTable dtMaterialDtls;
        private DataTable dtMaterialUMODtls;
        private EMIMultiple objEMIMultiple;
        private string refID;
        private string inboxFlag;
        decimal EditQtyIssued = 0;
        private int processPK;
        private DataTable dtAssetDetails;

        #endregion

        #endregion

        #region Page Page_PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (!string.IsNullOrEmpty(txtQtyIssued.ID))
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtQtyIssued.ID + "", "$('[id$=" + txtQtyIssued.ID + "]').ForceNumericOnly();", true);
            if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(3);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.NEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing(0);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();

            }
            else if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(2);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            if (EntryStatus == EntryStatus.VIEWMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            if (EntryStatus == EntryStatus.SAVEONLY)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(4);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
        }
        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {

            PageActionHandler();

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }

        private void PageActionHandler()
        {
            string prefID;
            int cusPK;
            int referenceID;
            int preferenceID;
            int appId;
            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    this.CurrencyFormatString = String.Format("{{0:n{0}}}", GetLocalResourceObject("QtyDecimal"));
                    ConfigurationSettings();
                    GetUserRights();
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm(ControlEnum.CLEAR);
                    GetFieldValues(ControlEnum.ISSUINGSTORE);
                    SetFieldValues(ControlEnum.ISSUINGSTORE);
                    GetFieldValues(ControlEnum.ISSUEAGAINST);
                    SetFieldValues(ControlEnum.ISSUEAGAINST);
                    SetFieldValues(ControlEnum.ISSUEAGAINSTFILTER);
                    ddlFilterType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    //GetFieldValues(ControlEnum.TYPE);
                    //SetFieldValues(ControlEnum.TYPE);
                    GetFieldValues(ControlEnum.GRID);
                    SetFieldValues(ControlEnum.GRID);
                    //GetUserRights();
                    //txtCodeFilterList.Focus();
                    ddlIssuingStore.SelectedValue = currentUser.CurrentDeptPK.ToString();
                    ddlIssuingStore.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    processPK = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PROCESS] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PROCESS]);
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID()
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            {
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            }
            else
            {
                path = Request.Url.AbsolutePath.ToLower();
            }

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                ProcessID = ucrWrkf.ProcessID;
            }

        }
        #endregion


        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {

            try
            {
                #region Grid Fixed Columns
                if ((sender as GridView).ID == "grdMaterialIssueList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        ImageButton imbEdit = e.Row.FindControl("imbEdit") as ImageButton;
                        ImageButton imbDelete = e.Row.FindControl("imbDelete") as ImageButton;
                        ImageButton imbView = e.Row.FindControl("imbView") as ImageButton;
                        ImageButton imbPrint = e.Row.FindControl("imbPrint") as ImageButton;
                        ImageButton imbModify = e.Row.FindControl("imbModify") as ImageButton;
                        ImageButton imbPRShorClose = e.Row.FindControl("imbPRShorClose") as ImageButton;
                        ImageButton imbCancel = e.Row.FindControl("imbCancel") as ImageButton;
                        Button imgbtnPRHierarchyLevel = e.Row.FindControl("imgbtnPRHierarchyLevel") as Button;

                        int isModifyEMI = Convert.ToInt32(hdnModifyEMI.Value);
                        int isCancelEMI = Convert.ToInt32(hdnCancelEMI.Value);
                        int UserStatus = Convert.ToInt32((e.Row.FindControl("hdfUserStatus") as HiddenField).Value);
                        //int DELSTATUS = Convert.ToInt32((e.Row.FindControl("hdfPrhDelStatus") as HiddenField).Value);
                        int ICH_STATUS = Convert.ToInt32((e.Row.FindControl("hdfIchStatus") as HiddenField).Value);

                        //int prhLinkStatus = Convert.ToInt32((e.Row.FindControl("hdfprhLinkStatus") as HiddenField).Value);//We can Use this flwg instead of following 3 flag ie,isPOExist,isPOWkfExist,grnWkfExist
                        //prhLinkStatus : 0 => PR, 1=> PO draft, 2=> PO wkf started, 12 => GRN wkf Started.
                        #region Show/Hide grdMaterialIssueList image buttons w.r.to previlege
                        if (UserStatus == 1)
                        {//Action To perform for the logged in user
                            imbEdit.Visible = true;
                            imbDelete.Visible = false;
                            imbView.Visible = false;
                        }
                        else if (UserStatus == 0)
                        {//No Action to perform but he is a participent in the work flow
                            imbEdit.Visible = false;
                            imbDelete.Visible = false;
                        }
                        else if (UserStatus == 2)
                        {//Draft will have this status
                            imbEdit.Visible = true;
                            imbDelete.Visible = true;
                            imbView.Visible = false;
                        }
                        if (ICH_STATUS == 104)
                        { //Cancelled
                            imbEdit.Visible = false;
                            imbView.Visible = true;
                            imbDelete.Visible = false;
                            imbPrint.Visible = true;
                        }
                        if (isModifyEMI == 1 && ICH_STATUS == 1)
                        {   //PRH_STATUS=> 4 (ShortClosure),5(Closed)
                            imbModify.Visible = true;
                        }
                        else
                        {
                            imbModify.Visible = false;
                        }
                        //Cancel PR
                        if (isCancelEMI == 1 && UserStatus != 2 && UserStatus != 4 && ICH_STATUS != 4)
                        { //UserStatus: 2=>draft
                            imbCancel.Visible = true;
                        }
                        else
                        {
                            imbCancel.Visible = false;
                        }
                        #endregion
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all Command actions of gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            GridView senderGridView = (GridView)sender;
            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
            int pk = 0, refID = 0, UserStatus = 0;
            pk = Convert.ToInt32((row.FindControl("hdfItemPk") as HiddenField).Value);
            //refID = Convert.ToInt32((row.FindControl("hdfRefId") as HiddenField).Value);
            //UserStatus = Convert.ToInt32((row.FindControl("hdfUserStatus") as HiddenField).Value);
            int? result;
            string remarks = "";
            int HasWorkflow = 0;
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            if (senderGridView.ID == "grdMaterialIssueList")
            {
                switch (e.CommandName)
                {
                    case "EDITACTION":
                        #region EDITACTION
                        ResetForm(ControlEnum.CLEAR);
                        ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        CurrPK = Convert.ToInt32((row.FindControl("hdfItemPk") as HiddenField).Value);
                        //Fill Process and get WorkFlow RefID and fetch Application ID
                        FillProcessID();

                        ucrWrkf.ViewType = 1;
                        ucrWrkf.RefID = 0;
                        EntryStatus = EntryStatus.ENTRYMODE;
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewAction();

                        GetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                        SetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                        SetFieldValues(ControlEnum.EMIITEMLIST);
                        #endregion
                        break;
                    case "DELETEEMI":
                        #region DELETEEMI
                        CurrPK = Convert.ToInt32((row.FindControl("hdfItemPk") as HiddenField).Value);
                        result = Convert.ToInt32(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.DeleteExternalMaterialIssue(CurrPK, currentUser.PKUser, HasWorkflow));
                        if (result == 1)
                        {
                            litErrorMsg.Text = Resources.Messages.EMIMultipleDeletedSuccessfully;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            GetFieldValues(ControlEnum.GRID);
                            SetFieldValues(ControlEnum.GRID);
                        }
                        else
                        {
                            if (result == 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Assigned").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("EMIMultiple").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        #endregion
                        break;
                    case "CANCELEMI":
                        #region CANCELEMI
                        DateTime transactionDate;
                        int bizUnit = 0;
                        string LockUptoDate = string.Empty;
                        int Result;
                        int module = 0;
                        //transactionDate = dsEMIMultipleList.Tables[1].Rows[0]["ICH_DATE"].ToString() != null ? DateTime.Parse(Request.Params["Date"]) : DateTime.Now;
                        transactionDate = DateTime.Now;
                        bizUnit = currentUser.SBUID;
                        System.Collections.Generic.List<object> retvals = new System.Collections.Generic.List<object>();
                        Result = Convert.ToInt32(BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(transactionDate, bizUnit, 2, ref LockUptoDate));
                        if (Result == 0)
                        {
                            CurrPK = Convert.ToInt32((row.FindControl("hdfItemPk") as HiddenField).Value);
                            result = Convert.ToInt32(BusinessLogic.StoreManagement.ExternalMaterialIssueBL.DeleteExternalMaterialIssue(CurrPK, currentUser.PKUser, HasWorkflow));
                            litErrorMsg.Text = Resources.Messages.EMIMultipleCanceledSuccessfully;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                            GetFieldValues(ControlEnum.GRID);
                            SetFieldValues(ControlEnum.GRID);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("EMIMultiple").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        #endregion
                        break;
                    case "VIEW":
                        ResetForm(ControlEnum.CLEAR);
                        CurrPK = Convert.ToInt32((row.FindControl("hdfItemPk") as HiddenField).Value);
                        EntryStatus = EntryStatus.VIEWMODE;
                        FillProcessID();
                        GetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                        SetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                        SetFieldValues(ControlEnum.EMIITEMLIST);
                        //WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.EDITMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;

                        }
                        ucrWrkf.ViewAction();
                        break;
                    case "MODIFY":
                        ResetForm(ControlEnum.CLEAR);
                        ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        CurrPK = Convert.ToInt32((row.FindControl("hdfItemPk") as HiddenField).Value);
                        ICH_IS_EDIT.Value = CommonConstants.SELECT_VALUE_ONE;
                        EntryStatus = EntryStatus.SAVEONLY;
                        FillProcessID();
                        GetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                        SetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                        SetFieldValues(ControlEnum.EMIITEMLIST);
                        //WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.EDITMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;

                        }
                        ucrWrkf.ViewAction();
                        hdfIsModify.Value = "1";
                        break;
                    case "PRINT":
                        #region PRINT
                        CurrPK = Convert.ToInt32((row.FindControl("hdfItemPk") as HiddenField).Value);
                        string url = GetLocalResourceObject("EMIMultiplePRINTURL").ToString() + "?ID=" + CurrPK + "&APPTYPE=" + "EMI" + "&APPSUBTYPE=" + 3;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "OpenPDF", "OpenPDF('" + url + "');", true);
                        #endregion
                        break;
                }
            }
        }

        #region Action Handler
        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!sender.GetType().IsEquivalentTo(typeof(GridView)))//'OnPreRender' event registered in Gridview, so every action on page calling Action Handler
                if (!(this.Master as ERPSMS_2).ValidatePageDept())
                    return;

            string prefID;
            int referenceID;
            int preferenceID;
            int appId;


            try
            {
                int result;
                result = 0;
                bool bIsChecked = false;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else
                {
                    if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                    {
                        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                    }
                    else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                    {
                        commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                    }
                    else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                    {
                        if (((RadioButton)sender).ID == "rbtSelect")
                        {
                            commonActions = ActionsEnum.DETAIL;
                        }
                    }
                    else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                    {
                        if (((DropDownList)sender).ID == "ddlIssuingStore")
                        {
                            commonActions = ActionsEnum.CHANGE;
                        }
                    }
                }
                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlEnum.CLEAR);
                        ResetForm(ControlEnum.CLEARFILTER);
                        txtInvTypeCode.Focus();
                        lblMaterilaConsumptionNo.Text = Resources.Messages.DocGenerationNew;
                        txtDate.Text = DateTime.Today.ToString("dd-MMM-yyyy");
                        //ddlUOM.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                        ddlIssuingStore.SelectedValue = currentUser.CurrentDeptPK.ToString();
                        ddlIssuingStore.Enabled = false;
                        //Fill Process and get WorkFlow RefID and fetch Application ID
                        FillProcessID();
                        ucrWrkf.ViewType = 1;
                        EntryStatus = EntryStatus.ENTRYMODE;
                        ucrWrkf.FillWorkFlowDetails();

                        break;
                    #endregion
                    #region SAVESUBMIT popup
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT Popup
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WorkFlow Submit
                    case ActionsEnum.WRKFSUBMIT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideWkfSubmit", "ClosePopup();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                EMIMultiple EMIHeaderObj = new EMIMultiple();
                                EMIHeaderObj = (EMIMultiple)SetUIValuesToObject(ControlEnum.SAVE);
                                if (EMIHeaderObj.EMIMultipleDetails.Count == 0)
                                {
                                    litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.Captions.Information + "');", true);
                                    return;
                                }

                                if (EMIHeaderObj != null && EMIHeaderObj.EMIMultipleDetails != null)
                                {
                                    SaveTransaction(EMIHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                }

                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL ||
                                (Request.QueryString[QueryStrings.PageType] != null &&
                                Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Cancel))
                            {
                                if (BusinessLogic.Sales.SaleOrderBL.SaleContractCancelCheck(CurrPK))
                                {
                                    // ucrWrkf.ApplicationID = CurrPK;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_SalesOrder_Cancel").ToString() + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));

                        }
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        string strTrxNo = string.Empty;
                        EMIMultiple emiMultiple = (EMIMultiple)SetUIValuesToObject(ControlEnum.SAVE);
                        if (emiMultiple.EMIMultipleDetails.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        if (emiMultiple != null && emiMultiple.EMIMultipleDetails != null)
                        {
                            emiMultiple.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                            string xmlDoc = CommonFunctions.XmlSerialize<EMIMultiple>(emiMultiple);
                            List<object> lstResult = DataAccess.StoreManagement.ExternalMaterialIssueDL.SaveExternalMaterialIssueWkf(xmlDoc);
                            result = Convert.ToInt32(lstResult[0]);
                            if (result > 0 && lstResult.Count > 1)
                            {
                                if (!string.IsNullOrEmpty(lstResult[1].ToString()))
                                    strTrxNo = lstResult[1].ToString();
                                litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "ExternalMaterialIssue").ToString(), strTrxNo);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                ResetForm(ControlEnum.CLEAR);
                                ResetForm(ControlEnum.CLEARFILTER);
                                EntryStatus = EntryStatus.LISTMODE;
                                CurrPK = (int)result;
                                GetFieldValues(ControlEnum.GRID);
                                SetFieldValues(ControlEnum.GRID);
                            }
                            else
                            {
                                if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.INCORRECT)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + GetLocalResourceObject("CodeAlreadyExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.CODEEXIST)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + GetLocalResourceObject("NameAlreadyExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.INVALIDBATCH)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + GetLocalResourceObject("InvalidBatch").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.STOCKVALUECHECK)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("StockValueCheck").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("EMI").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region DETAIL
                    case ActionsEnum.DETAIL:
                        bIsChecked = false;
                        foreach (GridViewRow grdrow in grdMaterialIssueList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                bIsChecked = true;
                                ResetForm(ControlEnum.CLEAR);
                                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfItemPk")).Value);
                                int DetailStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfIchStatus")).Value);
                                if (DetailStatus == 0)
                                {
                                    ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                                    FillProcessID();
                                    ucrWrkf.ViewType = 1;
                                    ucrWrkf.RefID = 0;
                                    EntryStatus = EntryStatus.ENTRYMODE;
                                    ucrWrkf.FillWorkFlowDetails();
                                    ucrWrkf.ViewAction();
                                    GetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                                    SetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                                    SetFieldValues(ControlEnum.EMIITEMLIST);
                                }
                                else
                                {
                                    EntryStatus = EntryStatus.VIEWMODE;
                                    FillProcessID();
                                    GetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                                    SetFieldValues(ControlEnum.EMIMULTIPLEHDR);
                                    SetFieldValues(ControlEnum.EMIITEMLIST);
                                    ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                    ucrWrkf.FillWorkFlowDetails();
                                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.EDITMODE && ucrWrkf.HasPageTaskPermission)
                                        ucrWrkf.ViewType = 1;
                                    else
                                    {
                                        ucrWrkf.ViewType = 0;
                                    }
                                    ucrWrkf.ViewAction();
                                }
                                break;
                            }
                        }
                        if (!bIsChecked)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region CLEAR
                    //case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                    case ActionsEnum.CLEAR:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        ResetForm(ControlEnum.CLEARFILTER);
                        GetFieldValues(ControlEnum.GRID);
                        SetFieldValues(ControlEnum.GRID);
                        //txtCodeFilterList.Focus();
                        break;
                    #endregion

                    #region PRINT
                    //case ActionsEnum.CANCEL:
                    case ActionsEnum.PRINT:
                        #region PRINT

                        string url = GetLocalResourceObject("EMIMultiplePRINTURL").ToString() + "?ID=" + CurrPK + "&APPTYPE=" + "EMI" + "&APPSUBTYPE=" + 3;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "OpenPDF", "OpenPDF('" + url + "');", true);

                        #endregion
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        bIsChecked = false;
                        result = InvoiceTypeBL.DeleteInvoiceTypeDetails(CurrPK, LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("EMI").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ResetForm(ControlEnum.CLEAR);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlEnum.GRID);
                            SetFieldValues(ControlEnum.GRID);
                        }
                        else
                        {
                            if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("EMI").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.REFERRED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.INVALIDBATCH)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + GetLocalResourceObject("InvalidBatch").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.STOCKVALUECHECK)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("StockValueCheck").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("EMI").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region GROUP CHANGE
                    case ActionsEnum.CHANGE:
                        if (Convert.ToInt16(ddlIssuingStore.SelectedValue) > 0)
                        {
                            switch (ddlIssuingStore.SelectedValue)
                            {
                                case "1":
                                    subType = "SALES INVOICE TYPE";
                                    break;
                                case "2":
                                    subType = "PURCHASE INVOICE TYPE";
                                    break;
                            }
                            GetFieldValues(ControlEnum.ISSUEAGAINST);
                            SetFieldValues(ControlEnum.ISSUEAGAINST);
                        }
                        break;
                    #endregion

                    #region GETCURRENTSTOCK
                    case ActionsEnum.GETCURRENTSTOCK:
                        txtBatchNo.Enabled = false;
                        GetFieldValues(ControlEnum.GETCURRENTSTOCK);
                        SetFieldValues(ControlEnum.GETCURRENTSTOCK);

                        //GetFieldValues(ControlEnum.ITEMSUOM);
                        //SetFieldValues(ControlEnum.ITEMSUOM);
                        break;
                    #endregion

                    #region ITEMSTOCK
                    case ActionsEnum.ITEMSTOCK:
                        txtBatchNo.Enabled = true;
                        GetFieldValues(ControlEnum.ITEMSTOCK);
                        SetFieldValues(ControlEnum.ITEMSTOCK);
                        break;
                    #endregion

                    #region ITEMSTOCK
                    case ActionsEnum.ASSETDETAILS:
                        GetFieldValues(ControlEnum.ASSETDETAILS);
                        SetFieldValues(ControlEnum.ASSETDETAILS);
                        break;
                    #endregion

                    #region TYPE
                    case ActionsEnum.TYPE:
                        GetFieldValues(ControlEnum.TYPE);
                        SetFieldValues(ControlEnum.TYPE);
                        //ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        txtItemName.Text = "Select/Type";
                        hdfItemName.Value = string.Empty;
                        if (!string.IsNullOrEmpty(txtQtyIssued.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtQtyIssued.ID + "", "$('[id$=" + txtQtyIssued.ID + "]').ForceNumericOnly();", true);
                        if (txtBatchNo.Text != "")
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindBatchNo();BindItemNameDDL();});", true);
                        else
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindItemNameDDL();});", true);
                        break;
                    #endregion
                    #region TYPE
                    case ActionsEnum.ITEMNAME:
                        hdfIsEdit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        if (!string.IsNullOrEmpty(txtQtyIssued.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtQtyIssued.ID + "", "$('[id$=" + txtQtyIssued.ID + "]').ForceNumericOnly();", true);
                        if (txtBatchNo.Text != "")
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindBatchNo();BindItemNameDDL();});", true);
                        else
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindItemNameDDL();});", true);
                        break;
                    #endregion
                    #region TYPE FILTER
                    case ActionsEnum.TYPEFILTER:
                        GetFieldValues(ControlEnum.TYPEFILTER);
                        SetFieldValues(ControlEnum.TYPEFILTER);
                        ddlFilterType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        if (!string.IsNullOrEmpty(txtQtyIssued.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtQtyIssued.ID + "", "$('[id$=" + txtQtyIssued.ID + "]').ForceNumericOnly();", true);
                        if (hdfAdvancedSearchType.Value == CommonConstants.SELECT_VALUE_ONE)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPageFilter();ShowHideAdvancedSearch(1);BindFilterItemNameDDL();});", true);
                        else
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPageFilter();ShowHideAdvancedSearch();BindFilterItemNameDDL();});", true);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlEnum.GRID);
                        SetFieldValues(ControlEnum.GRID);
                        break;
                    #endregion

                    #region EDIT_ACTION
                    case ActionsEnum.EDIT_ACTION:
                        decimal CurrentStock = 0;
                        string RowNum = ((ImageButton)sender).CommandArgument;
                        var editItem = EMIMultipleDetailsList.Where(itm => itm.ROW_NO == Convert.ToInt32(RowNum)).FirstOrDefault();
                        if (editItem != null)
                        {
                            RowNumber = editItem.ROW_NO;
                            hdfIsEdit.Value = CommonConstants.SELECT_VALUE_ONE;
                            txtItemCategory.Text = HttpUtility.HtmlDecode(editItem.ICD_ITEM_CATEGORY_TEXT);
                            hdfItemCategory.Value = editItem.ICD_ITEM_CATEGORY.ToString();
                            txtItemCode.Text = HttpUtility.HtmlDecode(editItem.ICD_ITEM_TEXT);
                            hdfItemCode.Value = editItem.ICD_ITEM.ToString();
                            txtBatchNo.Text = HttpUtility.HtmlDecode(editItem.ICD_STK_BATCH_NO);
                            hdfBatchNo.Value = editItem.ICD_STK_BATCH.ToString();
                            if (txtBatchNo.Text != "")
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindBatchNo();BindItemNameDDL();});", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindItemNameDDL();});", true);
                            if (hdfIsModify.Value == "1")
                            {
                                CurrentStock = editItem.ICD_CURRENT_STK + editItem.ICD_QTY_CONSUMED;
                                txtStockValue.Text = CurrentStock.ToString("#.000");
                                if (Convert.ToInt32(CurrentStock) <= 1)
                                    //txtStockValue.Text = editItem.ICD_CURRENT_STK.ToString("0.000");
                                    txtStockValue.Text = String.Format(GetCurrencyFormat(), CurrentStock);
                            }
                            else
                            {
                                txtStockValue.Text = editItem.ICD_CURRENT_STK.ToString("#.000");
                                if (Convert.ToInt32(editItem.ICD_CURRENT_STK) <= 1)
                                    txtStockValue.Text = String.Format(GetCurrencyFormat(), editItem.ICD_CURRENT_STK);
                            }

                            txtStockValue.ToolTip = txtStockValue.Text;

                            //txtStockValue.Text = String.Format(GetCurrencyFormat(), editItem.ICD_CURRENT_STK);

                            ddlIssueAgainst.SelectedValue = editItem.ICD_ISS_RCV_TYPE.ToString();
                            //ddlUOM.SelectedValue = editItem.ICD_UOM.ToString();
                            hdfUOM.Value = editItem.ICD_UOM.ToString();
                            txtUOM.Text = editItem.ICD_UOM_TEXT.ToString();
                            txtUOM.ToolTip = txtUOM.Text;
                            GetFieldValues(ControlEnum.TYPE);
                            SetFieldValues(ControlEnum.TYPE);
                            ddlType.SelectedValue = editItem.ICD_ISS_RCV_SUB_TYPE.ToString();
                            txtItemName.Text = HttpUtility.HtmlDecode(editItem.ICD_ISS_RCV_NAME);
                            hdfItemName.Value = editItem.ICD_ISS_RCV_PK.ToString();

                            txtQtyIssued.Text = editItem.ICD_QTY_CONSUMED.ToString("#.000");
                            if (Convert.ToInt32(editItem.ICD_QTY_CONSUMED) <= 1)
                                txtQtyIssued.Text = String.Format(GetCurrencyFormat(), editItem.ICD_QTY_CONSUMED);

                            //txtQtyIssued.Text = String.Format(GetCurrencyFormat(), editItem.ICD_QTY_CONSUMED);

                            txtComments.Text = HttpUtility.HtmlDecode(editItem.ICD_REMARKS);
                            EditQtyIssued = Convert.ToDecimal(editItem.ICD_QTY_CONSUMED.ToString("#.000"));
                            hdfEditQtyIssued.Value = editItem.ICD_QTY_CONSUMED.ToString("#.000");
                            RowEditMode = true;
                            if (!string.IsNullOrEmpty(txtQtyIssued.ID))
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtQtyIssued.ID + "", "$('[id$=" + txtQtyIssued.ID + "]').ForceNumericOnly();", true);
                        }

                        break;
                    #endregion

                    #region DELETE_ACTION
                    case ActionsEnum.DELETE_ACTION:
                        string RowNo = ((ImageButton)sender).CommandArgument;
                        var deleteItem = EMIMultipleDetailsList.Where(itm => itm.ROW_NO == Convert.ToInt32(RowNo)).FirstOrDefault();
                        if (deleteItem != null)
                        {
                            EMIMultipleDetailsList.Remove(deleteItem);
                            SetFieldValues(ControlEnum.EMIITEMLIST);
                        }
                        break;
                    #endregion

                    #region ADD
                    case ActionsEnum.ADD:

                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            #region CommentMandatoryChecking
                            // In the case of EKK, While issuing  Item Category(Tyre) comment should be mandatory to fill the serial number(s)
                            if (hdfAddCommentMandValidation.Value == "1")
                            {
                                if (txtItemCategory.Text == GetLocalResourceObject("CommentMandatoryCategoryName").ToString())
                                {
                                    if (txtComments.Text == string.Empty)
                                    {
                                        litErrorMsg.Text = Resources.Messages.CommentRequired;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        return;
                                    }
                                }
                            } 
                            #endregion                         
                            if (!IsItemExist(EMIMultipleDetailsList, Convert.ToInt32(hdfItemCode.Value), Convert.ToInt32(hdfItemName.Value)) || RowEditMode == true)
                            {
                                EMIMultipleDetails objItem = (EMIMultipleDetails)SetUIValuesToObject(ControlEnum.ADD);
                                //if (objItem.ICD_CURRENT_STK < objItem.ICD_QTY_CONSUMED)
                                hdfBatchNoCheck.Value = objItem.ICD_STK_BATCH > 0 ? "1" : "0";
                                if (objItem.ICD_QTY_CONSUMED == 0)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("QtyIssuedCheck").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                }
                                else
                                {
                                    if (!IsValidQuantity(EMIMultipleDetailsList, objItem.ICD_STK_BATCH, objItem.ICD_CURRENT_STK, objItem.ICD_QTY_CONSUMED, objItem.ICD_ITEM))
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("StockValueCheck").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        if (EMIMultipleDetailsList == null || EMIMultipleDetailsList.Count == 0)
                                        {
                                            EMIMultipleDetailsList = new List<EMIMultipleDetails>();
                                            objItem.ROW_NO = 1;
                                            EMIMultipleDetailsList.Add(objItem);
                                        }
                                        else
                                        {
                                            if (!RowEditMode)
                                            {
                                                objItem.ROW_NO = EMIMultipleDetailsList.Max(r => r.ROW_NO) + 1;
                                                EMIMultipleDetailsList.Add(objItem);
                                            }
                                            else
                                            {
                                                var removeItem = EMIMultipleDetailsList.Where(itm => itm.ROW_NO == Convert.ToInt32(RowNumber)).FirstOrDefault();
                                                EMIMultipleDetailsList.Remove(removeItem);
                                                objItem.ROW_NO = RowNumber;
                                                EMIMultipleDetailsList.Add(objItem);
                                            }
                                        }
                                        SetFieldValues(ControlEnum.EMIITEMLIST);
                                        //ResetForm(ControlEnum.ADDTOLIST);
                                        ResetForm(ControlEnum.CLEAREMIDETAILS);
                                    }
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ItemAlreadyAdded;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    default: break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(EMIMultiple objemiMultiple, int workflowFlag)
        {
            int retRfID = 0;
            int? result = 0;
            string savePath = string.Empty;
            string strTrxNo = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objemiMultiple == null)
                objemiMultiple = new EMIMultiple();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objemiMultiple.USER_PK = wkfDetails.UserPK;
            objemiMultiple.WKF_APPLICATION = CurrPK;
            objemiMultiple.WKF_COMMENTS = wkfDetails.Comments;
            objemiMultiple.WKF_TRX_FLAG = workflowFlag;
            objemiMultiple.WKF_PROCESS = wkfDetails.ProcessID;
            objemiMultiple.WKF_REFERENCE = wkfDetails.ReferenceID;
            objemiMultiple.WKF_TASK = wkfDetails.TaskID;
            objemiMultiple.WKF_TASK_ACTION = wkfDetails.ActionID;
            objemiMultiple.WKF_FLAG = workflowFlag;
            objemiMultiple.AST_DOC_MODE = 1;
            objemiMultiple.AST_VALUE = 3;
            objemiMultiple.APT_CODE = "EMI";
            //objemiMultiple.WKF_MAIL_ATTACH = 0;
            action = wkfDetails.ActionText;
            #endregion

            string xmlDoc = CommonFunctions.XmlSerialize<EMIMultiple>(objemiMultiple);//CommonFunctions.ObjectTOXml(EMIHeaderObj);
            // save Process Control inspection details

            List<object> lstResult = DataAccess.StoreManagement.ExternalMaterialIssueDL.SaveExternalMaterialIssueWkf(xmlDoc);
            result = Convert.ToInt32(lstResult[0]);
            if (result > 0 && lstResult.Count > 1)
            {
                if (!string.IsNullOrEmpty(lstResult[1].ToString()))
                    strTrxNo = lstResult[1].ToString();
                litErrorMsg.Text = GetLocalResourceObject("Msg_SubmitSuccess").ToString();
                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("Messages", "ExternalMaterialIssue").ToString(), strTrxNo);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                ResetForm(ControlEnum.CLEAR);
                ResetForm(ControlEnum.CLEARFILTER);
                EntryStatus = EntryStatus.LISTMODE;
                CurrPK = (int)result;
                GetFieldValues(ControlEnum.GRID);
                SetFieldValues(ControlEnum.GRID);
            }
            else
            {
                if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)BusinessObject.CommonManagement.DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.INCORRECT)
                {
                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + GetLocalResourceObject("CodeAlreadyExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + GetLocalResourceObject("NameAlreadyExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.INVALIDBATCH)
                {
                    litErrorMsg.Text = GetLocalResourceObject("EMI").ToString() + " " + GetLocalResourceObject("InvalidBatch").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)BusinessObject.CommonManagement.DbSaveStatus.STOCKVALUECHECK)
                {
                    litErrorMsg.Text = GetLocalResourceObject("StockValueCheck").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("EMI").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
            }
        }
        #endregion

        #region Get Field Values
        private void GetFieldValues(ControlEnum type)
        {
            BusinessObject.GridPrams gridParam;
            int processId;
            
            string pageUrl = string.Empty;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {

                    #region ISSUINGSTORE
                    case ControlEnum.ISSUINGSTORE:
                        dtIssuingStore = new DataTable();
                        dtIssuingStore = DataAccess.SubDepartmentManagement.SubDepartmentMasterDL.GetStoresByType(Convert.ToInt32(CommonConstants.SELECT_ALL_VAL), Convert.ToInt32(CommonConstants.DEPRECIATIONINDEX), currentUser, currentUser.SBUID, Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE), Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO) );
                        break;
                    #endregion

                    #region ISSUE AGAINST
                    case ControlEnum.ISSUEAGAINST:
                        dtIssueAgainst = DataAccess.CommonManagement.CommonDL.GetIssueAgainst(currentUser.SBUID, "EXTERNAL ISS RCV TYPE", "DTL");
                        break;
                    #endregion

                    #region TYPE
                    case ControlEnum.TYPE:
                        if (Convert.ToInt32(ddlIssueAgainst.SelectedValue) > 0)
                            dtType = DataAccess.CommonManagement.CommonDL.GetType(currentUser.SBUID, Convert.ToInt32(ddlIssueAgainst.SelectedValue));
                        break;
                    #endregion

                    // Used to get Asset type, On selection of an Asset
                    #region ASSETDETAILS
                    case ControlEnum.ASSETDETAILS:
                        int asrPK = 0;
                        asrPK = Convert.ToInt32(hdfItemName.Value);
                        dtAssetDetails = DataAccess.CommonManagement.CommonDL.GetAssetDetails(asrPK);
                        break;
                    #endregion

                    #region TYPE FILTER
                    case ControlEnum.TYPEFILTER:
                        if (Convert.ToInt32(ddlFilterIssueAgainst.SelectedValue) > 0)
                            dtType = DataAccess.CommonManagement.CommonDL.GetType(currentUser.SBUID, Convert.ToInt32(ddlFilterIssueAgainst.SelectedValue));
                        break;
                    #endregion

                    #region LISTING GRID
                    case ControlEnum.GRID:
                        dsEMIMultipleList = new DataSet();
                        string FromDate = string.Empty;
                        string ToDate = string.Empty;
                        int Status;
                        int IssuingStore;
                        int ItemCategory;
                        int ItemPK;
                        string IssueNo;
                        int IssueAgainst;
                        int Type;
                        int ItemName;
                        //dsEMIMultipleList = BusinessLogic.MaterialManagement.MaterialMaster.GetEMIMultipleList(string.Empty, string.Empty, currentUser.CurrentSBUPK, currentUser.PKUser, PageIndex, PageSize);

                        gridParam = new BusinessObject.GridPrams();
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        //gridParam.Fields = GetLocalResourceObject("GridFields").ToString();
                        //gridParam.SortBy =GetLocalResourceObject("GridSortBy").ToString(); //PRH_DATE
                        //gridParam.SortDirection =GetLocalResourceObject("GridSortDirection").ToString();//DESC;
                        gridParam.FromDate = txtFromDate.Text;
                        gridParam.ToDate = txtToDate.Text;
                        //gridParam.FilterStatus = ddlStatus.SelectedValue;
                        gridParam.UserPK = currentUser.PKUser;
                        pageUrl = Resources.PageURL.EMIMultiple;

                        FromDate = txtFromDate.Text != "" ? txtFromDate.Text : string.Empty;
                        ToDate = txtToDate.Text != "" ? txtToDate.Text : string.Empty;
                        Status = Convert.ToInt32(ddlFilterStatus.SelectedValue);
                        IssuingStore = Convert.ToInt32(ddlFilterIssuingStore.SelectedValue);
                        ItemCategory = hdfFilterItemCategory.Value != "" ? Convert.ToInt32(hdfFilterItemCategory.Value) : 0;
                        ItemPK = hdfFilterItem.Value != "" ? Convert.ToInt32(hdfFilterItem.Value) : 0;
                        IssueNo = txtFilterIssueNo.Text;
                        IssueAgainst = Convert.ToInt32(ddlFilterIssueAgainst.SelectedValue);
                        Type = Convert.ToInt32(ddlFilterType.SelectedValue);
                        ItemName = hdfFilterItemName.Value != "" ? Convert.ToInt32(hdfFilterItemName.Value) : 0;
                        dsEMIMultipleList = BusinessLogic.MaterialManagement.MaterialMaster.GetEMIMultipleList(gridParam, pageUrl, FromDate, ToDate, Status, IssuingStore, ItemCategory, ItemPK, IssueNo, IssueAgainst, Type, ItemName, currentUser.CurrentSBUPK, currentUser.PKUser, PageIndex, PageSize);
                        //dsEMIMultipleList = BusinessLogic.MaterialManagement.MaterialMaster.GetEMIMultipleList(string.Empty, string.Empty, 1, 1, 1, 1, "", 1, 1, 1, currentUser.CurrentSBUPK, currentUser.PKUser, PageIndex, PageSize);
                        break;
                    #endregion

                    #region GETCURRENTSTOCK
                    case ControlEnum.GETCURRENTSTOCK:
                        dtCurrentStock = DataAccess.MaterialManagement.MaterialMasterDL.GetCurrentStockForStore(Convert.ToInt32(hdfItemCode.Value), currentUser.CurrentDeptPK, Convert.ToDateTime(txtDate.Text));
                        break;
                    #endregion
                    #region ITEM STOCK
                    case ControlEnum.ITEMSTOCK:
                        dtMaterialDtls = DataAccess.MaterialManagement.MaterialMasterDL.GetBatchDetails(Convert.ToInt32(hdfBatchNo.Value));
                        break;
                    #endregion

                    #region ITEM UOM
                    case ControlEnum.ITEMSUOM:
                        dtMaterialUMODtls = DataAccess.MaterialManagement.MaterialMasterDL.GetUOMConvExistsByMaterial(Convert.ToInt32(hdfItemCode.Value));
                        break;
                    #endregion

                    #region EMIMULTIPLEHDR
                    case ControlEnum.EMIMULTIPLEHDR:
                        objEMIMultiple = BusinessLogic.StoreManagement.ExternalMaterialIssueBL.GetEMIDetails(CurrPK);
                        if (objEMIMultiple != null)
                            EMIMultipleDetailsList = objEMIMultiple.EMIMultipleDetails;
                        break;
                    #endregion
                    default: break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region Set Field Values
        private void SetFieldValues(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnum.GETCURRENTSTOCK:
                        hdnItmNeedBatchStk.Value = dtCurrentStock.Rows[0]["ITM_NEED_BATCH_STK"].ToString();
                        txtItemCategory.Text = dtCurrentStock.Rows[0]["STD_ITEM_CATEGORY_TEXT"].ToString();
                        hdfItemCategory.Value = dtCurrentStock.Rows[0]["STD_ITEM_CATEGORY"].ToString();
                        hdfUOM.Value = dtCurrentStock.Rows[0]["STD_UOM"].ToString();
                        txtUOM.Text = dtCurrentStock.Rows[0]["STD_UOM_TEXT"].ToString();
                        txtUOM.ToolTip = txtUOM.Text;
                        if (hdfEnableBatch.Value == "0" || hdnItmNeedBatchStk.Value == "0")
                        {
                            txtStockValue.Text = Math.Round(Convert.ToDecimal(dtCurrentStock.Rows[0]["STD_QTY_IN_STOCK"].ToString()), 3).ToString();
                            txtStockValue.ToolTip = txtStockValue.Text;
                            txtBatchNo.Text = string.Empty;
                            hdfBatchNo.Value = string.Empty;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindItemNameDDL();});", true);
                        }
                        else
                        {
                            txtStockValue.Text = string.Empty;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindBatchNo();BindItemNameDDL();});", true);
                        }
                        if (!string.IsNullOrEmpty(txtQtyIssued.ID))
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtQtyIssued.ID + "", "$('[id$=" + txtQtyIssued.ID + "]').ForceNumericOnly();", true);
                        break;
                    #region ISSUINGSTORE
                    case ControlEnum.ISSUINGSTORE:
                        BindDropdown(controlType);
                        break;
                    #endregion

                    #region GRID LIST
                    case ControlEnum.GRID:
                        BindGrid(controlType);
                        break;
                    #endregion

                    #region ISSUEAGAINST
                    case ControlEnum.ISSUEAGAINST:
                        BindDropdown(controlType);
                        break;
                    #endregion

                    #region ISSUE AGAINST FILTER
                    case ControlEnum.ISSUEAGAINSTFILTER:
                        BindDropdown(controlType);
                        break;
                    #endregion

                    #region TYPE
                    case ControlEnum.TYPE:
                        BindDropdown(controlType);
                        break;
                    #endregion

                    #region ASSETDETAILS
                    // To set Asset type, On selection of an Asset
                    case ControlEnum.ASSETDETAILS:
                        if (dtAssetDetails != null && dtAssetDetails.Rows.Count > 0)
                        {
                            //txtAssetType.Text = HttpUtility.HtmlDecode(dtAssetDetails.Rows[0]["atpName"].ToString());
                            //hdfAssetType.Value = dtAssetDetails.Rows[0]["atpPK"].ToString();
                            ddlType.SelectedValue = HttpUtility.HtmlDecode(dtAssetDetails.Rows[0]["atpPK"].ToString());
                            if (txtBatchNo.Text != "")
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindBatchNo();BindAssetAuto()});", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindAssetAuto()});", true);
                        }
                        break;
                    #endregion

                    #region TYPEFILTER
                    case ControlEnum.TYPEFILTER:
                        BindDropdown(controlType);
                        break;
                    #endregion

                    #region ITEM TYPE
                    case ControlEnum.ITEMSTOCK:
                        if (dtMaterialDtls != null && dtMaterialDtls.Rows.Count > 0)
                        {
                            txtStockValue.Text = Math.Round(Convert.ToDecimal(dtMaterialDtls.Rows[0]["SBD_QTY_IN_STOCK"].ToString()), 3).ToString();
                            txtStockValue.ToolTip = txtStockValue.Text;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitDetailPage();BindBatchNo();BindItemNameDDL();});", true);
                            if (!string.IsNullOrEmpty(txtQtyIssued.ID))
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "" + txtQtyIssued.ID + "", "$('[id$=" + txtQtyIssued.ID + "]').ForceNumericOnly();", true);
                        }
                        break;
                    #endregion

                    #region TYPE
                    case ControlEnum.ITEMSUOM:
                        BindDropdown(controlType);
                        break;
                    #endregion

                    #region EMIMULTIPLEHDR
                    case ControlEnum.EMIMULTIPLEHDR:
                        GetUIValuesFromObject(ControlEnum.EMIMULTIPLEHDR);
                        break;
                    #endregion

                    #region EMI ITEM LIST
                    case ControlEnum.EMIITEMLIST:
                        BindGrid(ControlEnum.EMIITEMLIST);
                        break;
                    #endregion

                    default: break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlEnum controlType)
        {
            object returnObject = new object();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {

                    #region SAVE EMI
                    case ControlEnum.SAVE:
                        EMIMultiple tempEMIMultiple = new EMIMultiple();
                        tempEMIMultiple.USER_PK = currentUser.PKUser;
                        tempEMIMultiple.UserPk = currentUser.PKUser;
                        tempEMIMultiple.ICH_PK = CurrPK;
                        //tempEMIMultiple.LAST_MOD_DT = LastModifiedTime.ToString("dd-MMM-yyyy");
                        tempEMIMultiple.LAST_MOD_DT = CurrPK == 0 ? DateTime.Today.ToString("dd-MMM-yyyy") : LastModifiedTime.ToString();
                        tempEMIMultiple.ICH_TRX_TYPE = 5;
                        tempEMIMultiple.ICH_COMPANY = 1;
                        tempEMIMultiple.BizUnitPk = currentUser.SBUID;
                        tempEMIMultiple.ICH_DEPT = Convert.ToInt32(currentUser.CurrentDeptPK.ToString());
                        tempEMIMultiple.ICH_NO = lblMaterilaConsumptionNo.Text;
                        tempEMIMultiple.ICH_DATE = txtDate.Text.ToString();
                        tempEMIMultiple.ICH_ISS_RCV_PK = Convert.ToInt32(ddlIssuingStore.SelectedValue);
                        //tempEMIMultiple.ICH_ISS_RCV_NAME = txtReference.Text;
                        tempEMIMultiple.ICH_REF_NO = txtReference.Text.ToString();
                        tempEMIMultiple.EMIMultipleDetails = (List<EMIMultipleDetails>)EMIMultipleDetailsList;
                        //tempEMIMultiple.EMIMultipleDetails = (List<EMIMultipleDetails>)SetUIValuesToObject(ControlEnum.ADD);
                        tempEMIMultiple.ICH_IS_EDIT = Convert.ToInt32(ICH_IS_EDIT.Value);
                        returnObject = tempEMIMultiple;
                        break;
                    #endregion
                    #region EMIMULTIPLEDETAILSADD
                    case ControlEnum.ADD:
                        EMIMultipleDetails EMIMultipleDetailsObj = new EMIMultipleDetails();
                        //EMIMultipleDetailsObj.ROW_NO = RowIndex + 1;
                        EMIMultipleDetailsObj.ICD_ITEM = Convert.ToInt32(hdfItemCode.Value);
                        EMIMultipleDetailsObj.ICD_ITEM_CATEGORY_TEXT = HttpUtility.HtmlEncode(txtItemCategory.Text);
                        EMIMultipleDetailsObj.ICD_ITEM_CATEGORY = Convert.ToInt32(hdfItemCategory.Value);
                        EMIMultipleDetailsObj.ICD_ITEM_TEXT = HttpUtility.HtmlEncode(txtItemCode.Text);
                        EMIMultipleDetailsObj.ICD_STK_BATCH = hdfBatchNo.Value != "" ? Convert.ToInt32(hdfBatchNo.Value) : 0;
                        EMIMultipleDetailsObj.ICD_STK_BATCH_NO = HttpUtility.HtmlEncode(txtBatchNo.Text);
                        //EMIMultipleDetailsObj.ITM_NEED_BATCH_STK = Convert.ToDecimal(txtStockValue.Text);
                        EMIMultipleDetailsObj.ICD_CURRENT_STK = Convert.ToDecimal(txtStockValue.Text);
                        EMIMultipleDetailsObj.ICD_ISS_RCV_TYPE = Convert.ToInt32(ddlIssueAgainst.SelectedValue);
                        EMIMultipleDetailsObj.ICD_ISS_RCV_SUB_TYPE = Convert.ToInt32(ddlType.SelectedValue);
                        EMIMultipleDetailsObj.ICD_ISS_RCV_NAME = HttpUtility.HtmlEncode(txtItemName.Text);
                        EMIMultipleDetailsObj.ICD_ISS_RCV_PK = Convert.ToInt32(hdfItemName.Value);
                        //EMIMultipleDetailsObj.ICD_UOM = Convert.ToInt32(ddlUOM.SelectedValue);
                        EMIMultipleDetailsObj.ICD_UOM_TEXT = txtUOM.Text;
                        EMIMultipleDetailsObj.ICD_UOM = Convert.ToInt32(hdfUOM.Value);
                        //EMIMultipleDetailsObj.ICD_QTY_CONSUMED = Convert.ToDecimal(txtQtyIssued.Text);
                        EMIMultipleDetailsObj.ICD_QTY_CONSUMED = Math.Round(Convert.ToDecimal(txtQtyIssued.Text), 3);
                        EMIMultipleDetailsObj.ICD_REMARKS = HttpUtility.HtmlEncode(txtComments.Text);
                        returnObject = EMIMultipleDetailsObj;
                        break;
                    #endregion
                    default: break;
                }
                return returnObject;

            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion

        #region Set UI EditView
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.NEW)
                {
                    EntryStatus = EntryStatus.NEWMODE;
                }
                else if (Mode == ActionsEnum.VIEW)
                {
                    EntryStatus = EntryStatus.VIEWMODE;
                }
                else if (Mode == ActionsEnum.EDIT)
                {
                    EntryStatus = EntryStatus.EDITMODE;
                }
                else
                {
                    EntryStatus = EntryStatus.ENTRYMODE;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Helper Methods
        /// Method to Get User Rights
        /// </summary>
        //private void GetUserRights()
        //{
        //    //string path = "/StoreManagement/ExternalMaterialIssue.aspx?TYPE=1";
        //    string path = string.Empty;
        //    if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
        //    {
        //        path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
        //    }
        //    else
        //    {
        //        path = Request.Url.AbsolutePath.ToLower();
        //    }

        //    //string path = GetLocalResourceObject("PageURL") + "?TYPE=" + transactionType.Value;
        //    UserAuthBL userAuth = new UserAuthBL();
        //    UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID, currentUser.CurrentDeptPK);
        //    if (usrRights.Rights.Count > 0)
        //    {
        //        foreach (var item in usrRights.Rights)
        //        {
        //            if (item.ActionName == "MODIFY" && item.HasActionRight == true && item.UserDeptRight == true)
        //            {
        //                hdnModifyEMI.Value = "1";
        //            }
        //            if (item.ActionName == "CANCEL" && item.HasActionRight == true && item.UserDeptRight == true)
        //            {
        //                hdnCancelEMI.Value = "1";
        //            }
        //        }

        //    }

        //}
        /// <summary>
        /// For getting user action rights againist current record .1.Checking Verification Required .2. Show/Hide RateChangeHistoryGrid 
        /// </summary>
        private void GetUserRights()
        {
            BusinessLogic.CommonManagement.CommonBL userAuth;
            //string pageURL = this.WkfPageUrl;
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            {
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            }
            else
            {
                path = Request.Url.AbsolutePath.ToLower();
            }
            userAuth = new BusinessLogic.CommonManagement.CommonBL();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            //// get the user WorkFlow Rights
            UserRightsBO UserVerifyRights = userAuth.GetWorkFlowUserRightsInfo(currentUser.PKUser, path, currentUser.CurrentDeptPK, ucrWrkf.RefID);
            if (UserVerifyRights != null || UserVerifyRights.Rights.Count > 0)
            {
                for (int i = 0; i < UserVerifyRights.Rights.Count; i++)
                {
                    if (UserVerifyRights.Rights[i].ActionName == "MODIFY" && UserVerifyRights.Rights[i].HasActionRight == true)
                    {
                        hdnModifyEMI.Value = "1";
                    }
                    if (UserVerifyRights.Rights[i].ActionName == "CANCEL" && UserVerifyRights.Rights[i].HasActionRight == true)
                    {
                        hdnCancelEMI.Value = "1";
                    }
                }
            }
        }
        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void ConfigurationSettings()
        {
            dt = new DataTable();
            // Get Batch Allow Flag
            dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("INVENTORY SETTINGS", "EnableStockBatch", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                divBatchNo.Visible = true;
                BatchNoPK = Convert.ToInt32(dt.Rows[0]["ACF_VALUE"].ToString());
                hdfEnableBatch.Value = dt.Rows[0]["ACF_VALUE"].ToString();
            }
            else
            {
                divBatchNo.Visible = false;
                BatchNoPK = Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO);
                hdfEnableBatch.Value = "0";
            }
            hdfAddCommentMandValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "AddCommentMandValidation").ToString();
        }
        /// <summary>
        /// Is Item Exist In Added List
        /// </summary>
        /// <param name="lst">Added List </param>
        /// <param name="itemPK">new Item Added</param>
        /// <returns></returns>
        private bool IsItemExist(List<EMIMultipleDetails> lst, int itemPK, int itemName)
        {
            bool retVal = false;
            if (lst != null && lst.Count > 0)
                retVal = lst.Count(ls => ls.ICD_ITEM == itemPK && ls.ICD_ISS_RCV_PK == itemName) > 0 ? true : false;
            return retVal;
        }

        /// <summary>
        /// Is Valid Quantity
        /// </summary>
        /// <param name="lst">Added List </param>
        /// <param name="BatchPK">new Item Added</param>
        /// <param name="Stock">new Item Added</param>
        /// <param name="QtyIssued">new Item Added</param>
        /// <returns></returns>
        private bool IsValidQuantity(List<EMIMultipleDetails> lst, int BatchPK, decimal Stock, decimal QtyIssued, int ItemCodePK)
        {
            decimal totalQtyIssued = 0;
            decimal QuantityIssued = 0;
            bool retVal = false;
            if (lst != null && lst.Count > 0 && BatchPK > 0 && hdfBatchNoCheck.Value == "1")
                totalQtyIssued = lst.Where(bch => bch.ICD_STK_BATCH == BatchPK).Sum(ls => ls.ICD_QTY_CONSUMED);
            if (lst != null && lst.Count > 0 && BatchPK == 0 && hdfBatchNoCheck.Value == "0")
                totalQtyIssued = lst.Where(bch => bch.ICD_ITEM == ItemCodePK).Sum(ls => ls.ICD_QTY_CONSUMED);
            if (Convert.ToDecimal(hdfEditQtyIssued.Value) > 0 && RowEditMode == true)
            {
                QuantityIssued = totalQtyIssued + QtyIssued - Convert.ToDecimal(hdfEditQtyIssued.Value);
            }
            else
            {
                QuantityIssued = totalQtyIssued + QtyIssued;
            }
            if (QtyIssued > Stock)
                retVal = false;
            else
                if (QuantityIssued > Stock)
                    retVal = false;
                else
                    retVal = true;
            return retVal;
        }

        /// <summary>
        /// This Methode is Used to Formating Currency fields in HTML 
        /// </summary>
        /// <returns></returns>
        public string GetCurrencyFormat()
        {
            return this.CurrencyFormatString;
        }


        #region Bind Dropdown
        private void BindDropdown(ControlEnum controlType)
        {
            switch (controlType)
            {
                case ControlEnum.ISSUINGSTORE:
                    ddlFilterIssuingStore.Items.Clear();
                    if (dtIssuingStore != null && dtIssuingStore.Rows.Count > 0)
                    {
                        ddlFilterIssuingStore.DataSource = CommonFunctions.HtmlDecodeDataTable(dtIssuingStore, "DPT_NAME");
                        ddlFilterIssuingStore.DataTextField = "DPT_NAME";  //Resources.DataFieldRes.cfgData;
                        ddlFilterIssuingStore.DataValueField = "DPT_PK"; //Resources.DataFieldRes.cfgValue;
                        ddlFilterIssuingStore.DataBind();

                        ddlIssuingStore.DataSource = CommonFunctions.HtmlDecodeDataTable(dtIssuingStore, "DPT_NAME");
                        ddlIssuingStore.DataTextField = "DPT_NAME";  //Resources.DataFieldRes.cfgData;
                        ddlIssuingStore.DataValueField = "DPT_PK"; //Resources.DataFieldRes.cfgValue;
                        ddlIssuingStore.DataBind();
                    }
                    ddlFilterIssuingStore.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlEnum.ISSUEAGAINST:
                    ddlIssueAgainst.Items.Clear();
                    if (dtIssueAgainst != null && dtIssueAgainst.Rows.Count > 0)
                    {
                        ddlIssueAgainst.DataSource = CommonFunctions.HtmlDecodeDataTable(dtIssueAgainst, "CFG_DATA");
                        ddlIssueAgainst.DataTextField = "CFG_DATA";  //Resources.DataFieldRes.cfgData;
                        ddlIssueAgainst.DataValueField = "CFG_VALUE"; //Resources.DataFieldRes.cfgValue;
                        ddlIssueAgainst.DataBind();
                    }
                    ddlIssueAgainst.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlEnum.ISSUEAGAINSTFILTER:
                    ddlFilterIssueAgainst.Items.Clear();
                    if (dtIssueAgainst != null && dtIssueAgainst.Rows.Count > 0)
                    {
                        ddlFilterIssueAgainst.DataSource = CommonFunctions.HtmlDecodeDataTable(dtIssueAgainst, "CFG_DATA");
                        ddlFilterIssueAgainst.DataTextField = "CFG_DATA";  //Resources.DataFieldRes.cfgData;
                        ddlFilterIssueAgainst.DataValueField = "CFG_VALUE"; //Resources.DataFieldRes.cfgValue;
                        ddlFilterIssueAgainst.DataBind();
                    }
                    ddlFilterIssueAgainst.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlEnum.TYPE:
                    ddlType.Items.Clear();
                    if (dtType != null && dtType.Rows.Count > 0)
                    {
                        ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtType, "ICH_ISS_RCV_TYPE_TEXT");
                        ddlType.DataTextField = "ICH_ISS_RCV_TYPE_TEXT";  //Resources.DataFieldRes.cfgData;
                        ddlType.DataValueField = "ICH_ISS_RCV_TYPE_PK"; //Resources.DataFieldRes.cfgValue;
                        //ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtType, "ICH_ISS_RCV_TEXT");
                        //ddlType.DataTextField = "ICH_ISS_RCV_TEXT";  //Resources.DataFieldRes.cfgData;
                        //ddlType.DataValueField = "ICH_ISS_RCV_PK"; //Resources.DataFieldRes.cfgValue;
                        ddlType.DataBind();
                    }
                    ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlEnum.TYPEFILTER:
                    ddlFilterType.Items.Clear();
                    if (dtType != null && dtType.Rows.Count > 0)
                    {
                        ddlFilterType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtType, "ICH_ISS_RCV_TYPE_TEXT");
                        ddlFilterType.DataTextField = "ICH_ISS_RCV_TYPE_TEXT";  //Resources.DataFieldRes.cfgData;
                        ddlFilterType.DataValueField = "ICH_ISS_RCV_TYPE_PK"; //Resources.DataFieldRes.cfgValue;
                        //ddlFilterType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtType, "ICH_ISS_RCV_TEXT");
                        //ddlFilterType.DataTextField = "ICH_ISS_RCV_TEXT";  //Resources.DataFieldRes.cfgData;
                        //ddlFilterType.DataValueField = "ICH_ISS_RCV_PK"; //Resources.DataFieldRes.cfgValue;
                        ddlFilterType.DataBind();
                    }
                    ddlType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                case ControlEnum.ITEMSUOM:
                    //ddlUOM.Items.Clear();
                    //if (dtMaterialUMODtls != null && dtMaterialUMODtls.Rows.Count > 0)
                    //{
                    //    ddlUOM.DataSource = CommonFunctions.HtmlDecodeDataTable(dtMaterialUMODtls,"UOM_NAME");
                    //    ddlUOM.DataTextField = "UOM_NAME";
                    //    ddlUOM.DataValueField = "UOM_PK";
                    //    ddlUOM.DataBind();
                    //}
                    //ddlUOM.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                default: break;
            }
        }
        #endregion



        #region Sets the UI input controls from the object values
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EMIMULTIPLEHDR
                    case ControlEnum.EMIMULTIPLEHDR:
                        if (objEMIMultiple != null)
                        {
                            LastModifiedTime = Convert.ToDateTime(objEMIMultiple.LAST_MOD_DT);
                            lblMaterilaConsumptionNo.Text = objEMIMultiple.ICH_NO;
                            txtDate.Text = objEMIMultiple.ICH_DATE;
                            txtReference.Text = objEMIMultiple.ICH_REF_NO;
                            ddlFilterIssuingStore.SelectedValue = objEMIMultiple.ICH_DEPT.ToString();
                            lblMaterilaConsumptionNo.Text = objEMIMultiple.ICH_NO == string.Empty ? "[NEW]" : objEMIMultiple.ICH_NO;
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Bind Grid
        public void BindGrid(ControlEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LISTING GRID
                    case ControlEnum.GRID:

                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dsEMIMultipleList.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dsEMIMultipleList.Tables[0].Rows[0][0].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdMaterialIssueList.DataSource = dsEMIMultipleList.Tables[1];
                        grdMaterialIssueList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion

                    #region EMIMULTIPLELIST
                    case ControlEnum.EMIITEMLIST:
                        if (EMIMultipleDetailsList != null && EMIMultipleDetailsList.Count > 0)
                            grdEMIMultipleList.DataSource = EMIMultipleDetailsList.OrderBy(or => or.ROW_NO);
                        else
                            grdEMIMultipleList.DataSource = null;
                        grdEMIMultipleList.DataBind();
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region Reset Form

        private void ResetForm(ControlEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlEnum.CLEAR:
                    CurrPK = 0;
                    EMIMultipleDetailsList = null;
                    RowNumber = 0;
                    txtReference.Text = txtItemCategory.Text = txtItemCode.Text = txtBatchNo.Text = txtStockValue.Text = txtItemName.Text = txtQtyIssued.Text = txtComments.Text = string.Empty;
                    hdfItemCategory.Value = hdfItemCode.Value = hdfBatchNo.Value = hdfItemName.Value = string.Empty;
                    //chkNonGstItem.Checked = true;
                    ddlIssueAgainst.ClearSelection();
                    //ddlUOM.ClearSelection();
                    txtUOM.Text = string.Empty;
                    hdfUOM.Value = string.Empty;
                    ddlIssueAgainst.ClearSelection();
                    ddlType.ClearSelection();
                    LastModifiedTime = DateTime.Now;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    ucrWrkf.RefID = 0;
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 1;
                    ucrWrkf.ViewAction();

                    ICH_IS_EDIT.Value = CommonConstants.SELECT_VALUE_ZERO;
                    hdfIsEdit.Value = CommonConstants.SELECT_VALUE_ZERO;
                    hdfAdvancedSearchType.Value = CommonConstants.SELECT_VALUE_ZERO;
                    hdfEditQtyIssued.Value = CommonConstants.SELECT_VALUE_ZERO;
                    hdfBatchNoCheck.Value = CommonConstants.SELECT_VALUE_ZERO;
                    hdfIsModify.Value = CommonConstants.SELECT_VALUE_ZERO;
                    EditQtyIssued = 0;

                    grdEMIMultipleList.DataSource = null;
                    grdEMIMultipleList.DataBind();
                    break;
                #endregion

                #region CLEAR FILTER
                case ControlEnum.CLEARFILTER:
                    //txtCodeFilterList.Text = txtNameFilterList.Text = string.Empty;
                    CurrPK = 0;
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = 1;
                    this.EntryStatus = EntryStatus.LISTMODE;
                    txtFromDate.Text = txtToDate.Text = txtFilterItemCategory.Text = txtFilterItem.Text = txtFilterIssueNo.Text = txtFilterItemName.Text = string.Empty;
                    hdfFilterItemCategory.Value = hdfFilterItem.Value = hdfFilterIssueNo.Value = hdfFilterItemName.Value = string.Empty;
                    ddlFilterStatus.ClearSelection();
                    ddlFilterIssuingStore.ClearSelection();
                    ddlFilterIssueAgainst.ClearSelection();
                    ddlFilterType.ClearSelection();
                    break;
                #endregion

                #region ADD TO LIST
                case ControlEnum.ADDTOLIST:
                    //RowIndex = -1;
                    //hdfItemCategory.Value = string.Empty;
                    hdfItemCode.Value = string.Empty;
                    break;
                #endregion

                #region CLEAR EMI DETAILS
                case ControlEnum.CLEAREMIDETAILS:
                    RowEditMode = false;
                    //txtItemCategory.Text = string.Empty;
                    //hdfItemCategory.Value = string.Empty;
                    //txtItemCode.Text = string.Empty;
                    //hdfItemCode.Value = string.Empty;
                    txtBatchNo.Text = string.Empty;
                    hdfBatchNo.Value = string.Empty;
                    //txtStockValue.Text = string.Empty;
                    //ddlIssueAgainst.ClearSelection();
                    //ddlUOM.ClearSelection();
                    ddlType.ClearSelection();
                    txtItemName.Text = string.Empty;
                    hdfItemName.Value = string.Empty;
                    txtQtyIssued.Text = string.Empty;
                    txtComments.Text = string.Empty;
                    break;
                #endregion


                default: break;
            }
        }
        #endregion

        #endregion

        #region InitializeComponent
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.uclPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPaging.PageChanged += new ActionHandler(this.ActionHandler);
            this.Init += new EventHandler(this.Page_Init);
        }

        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
            }
        }

        #endregion

        #region ControlEnum

        public enum ControlEnum
        {
            GSTCLASSLIST,
            ISSUINGSTORE,
            NEW,
            CLEAR,
            CLEARFILTER,
            GRID,
            INVOICETYPEHDR,
            ISSUEAGAINST,
            TYPE,
            ITEMSTOCK,
            ITEMSUOM,
            SAVE,
            EMIMULTIPLEHDR,
            EMIITEMLIST,
            ADD,
            GETCURRENTSTOCK,
            CLEARPOPUP,
            ADDTOLIST,
            CLEAREMIDETAILS,
            ISSUEAGAINSTFILTER,
            TYPEFILTER,
            INVENTORYLOCKED,
            ASSETDETAILS
        }

        #endregion

        #region Pager Methods + Init
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }

        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {
            try
            {
                PgerControlNew pagerControl = (PgerControlNew)sender;
                string senderId = pagerControl.ID;
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        pagerControl.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        if (e.CurrentPage <= e.TotalPages)
                            pagerControl.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        if (e.CurrentPage > 1)
                            pagerControl.CurrentPage--;
                        break;
                }
                if (senderId == "uclPaging")
                {
                    PageIndex = uclPaging.CurrentPage;
                    GetFieldValues(ControlEnum.GRID);
                    SetFieldValues(ControlEnum.GRID);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
        }
        #endregion
    }
}