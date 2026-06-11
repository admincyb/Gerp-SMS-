using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.HRMS.Payroll;
using BusinessObject.Common;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using System.Threading;
using BusinessObject.AccountManagement;
using ERP.Utilities.HRMS;
using BusinessLogic.HRMS.Payroll;
using BusinessObject.CommonManagement;
using System.Web.UI.HtmlControls;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class SalaryAppraisal : ERP.Store.UI.WorkFlowBasePage//: System.Web.UI.Page
    {
        #region Variables and Properties
        #region Variables
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataTable dtEarningData;
        private DataTable dtDeductionData;
        private DataSet dsPageData;
        private int CompanyPk = 0;
        private BulkAppraisalBO.SalaryBulkAppraisalHeader objSalaryAppraisalHeader;
        private BulkAppraisalBO.EmpAppHeader objEmpHeader;
        DataTable dtErrorList = new DataTable();
        string strError = string.Empty;
        int payElementMode = 0;
        int payElementErnDeductPK = 0;
        private string refID;
        private string inboxFlag;
        private string prefID;
        #endregion
        #region Properties
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
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
        private string PageIndex
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.PageIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        /// <summary>
        /// To keep Leave Details List in view state
        /// </summary>
        private List<BulkAppraisalBO.SalaryBulkEmpDetails> AppraisalEMPDetailsList
        {
            get
            {
                return ViewState[ViewstateStrings.SalaryBulkEmpDetail] == null ? new List<BulkAppraisalBO.SalaryBulkEmpDetails>() : (List<BulkAppraisalBO.SalaryBulkEmpDetails>)ViewState[ViewstateStrings.SalaryBulkEmpDetail];
            }
            set
            {
                ViewState[ViewstateStrings.SalaryBulkEmpDetail] = value;
            }
        }
        /// <summary>
        /// To keep Leave Details List in view state
        /// </summary>
        private List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails> AppraisalPayDetailsList
        {
            get
            {
                return ViewState[ViewstateStrings.SalaryBulkPayAppraisalDetails] == null ? new List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails>() : (List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails>)ViewState[ViewstateStrings.SalaryBulkPayAppraisalDetails];
            }
            set
            {
                ViewState[ViewstateStrings.SalaryBulkPayAppraisalDetails] = value;
            }
        }
        /// <summary>
        /// To keep Current PK in view state
        /// </summary>
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
        /// <summary>
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
        /// 
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
        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }

        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState[ViewstateStrings.ReferanceID] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.ReferanceID].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.ReferanceID] = value;
            }
        }
        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState[ViewstateStrings.PageProcessID] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.PageProcessID]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PageProcessID] = value;
            }
        }

        private int Status
        {
            get
            {
                return this.ViewState[ViewstateStrings.Status] == null ? 0 : Convert.ToInt32((this.ViewState[ViewstateStrings.Status]));
            }
            set
            {
                this.ViewState[ViewstateStrings.Status] = value;
            }
        }
        #endregion
        #endregion

        #region Page Events
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);

            this.btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);

            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDelete.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            this.btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            this.btnEditforCancel.Load += new EventHandler(btnAction_Load);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
             base.CheckBtnVisibility(sender);
        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }
        #region Page Page_PreRender
        protected void Page_PreRender(Object sender, EventArgs e)
        {
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(3);", true);
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch(1);", true);
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (CurrPK == 0)
            {
                btnSubmit.Visible = false;
                btnCancelSubmit.Visible = false;
            }
        }
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
        #endregion

        #region Custom Pager Control Navigated Event
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    PageIndex = uclPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception )
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }
                    hdfAdvSearch.Value = "0";
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
    : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;
                    ReferanceID = string.IsNullOrEmpty(refID)
                          ? string.IsNullOrEmpty(prefID)
                                ? 0
                                : int.Parse(prefID)
                          : int.Parse(refID);
                    FillProcessID(1);
                    uclPaging.CurrentPage = 1;
                    //GetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.APPRAISALTYPES);
                    SetFieldValues(ControlsEnum.APPRAISALTYPESLIST);
                    GetFieldValues(ControlsEnum.APPLYAS);
                    SetFieldValues(ControlsEnum.APPLYAS);
                    GetFieldValues(ControlsEnum.INCRDECR);
                    SetFieldValues(ControlsEnum.INCRDECR);


                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        ////start
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            base.WkfRefID = ucrWrkf.RefID;
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            //if (pid.Equals("11"))
                            //    hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }
                        else if (pid.Equals("2") || pid.Equals("12"))
                        { }
                    }
                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                    }
                    if (CurrPK > 0)
                    {
                        //SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                            ucrWrkf.ViewType = 0;
                        GetFieldValues(ControlsEnum.SALARYDETAILSBYPK);
                        SetFieldValues(ControlsEnum.SALARYDETAILSBYPK);
                    }
                    else
                    {
                        uclPaging.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void ConfigurationSettings()
        {

            // UCEmpSalary.IsEmpAppraisal = Convert.ToInt32(VisbleStatusEnum.TRUE);
            // hdfNoOfLeaveValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveValidation").ToString();
        }
        #endregion
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            try
            {
                int? result;
                result = 0;
                bool bIsChecked = false;
                Label lblGdPayElmErn;
                Label lblGdPayElmDed;
                DropDownList ddlDeductPayElement;
                DropDownList ddlEarnPayElement;
                HiddenField hdfEarnSlNo;
                HiddenField hdfDeductSlNo;
                GridViewRow grvRow;
                HtmlControl imgPayMode;
                TextBox WrkfComments;
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
                strError = string.Empty;
                dtErrorList = new DataTable();
                List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails> tempList = new List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails>();
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if ((((DropDownList)sender).ID == "ddlApplyAs"))
                    {
                        commonActions = ActionsEnum.CHANGETYPE;
                    }
                    else if (((DropDownList)sender).ID == "ddlEarnPayElement")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.EARNINGCHANGE;
                    }
                    else if (((DropDownList)sender).ID == "ddlDeductPayElement")
                    {
                        commonActions = BusinessObject.AccountManagement.ActionsEnum.DEDUCTIONCHANGE;
                    }
                }
                switch (commonActions)
                {
                    #region New
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.CLEAR);
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.APPRAISALTYPES);
                        SetFieldValues(ControlsEnum.APPRAISALTYPES);
                        SetFieldValues(ControlsEnum.EMPLOYEE);
                        SetFieldValues(ControlsEnum.PAYELEMENTS);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        lblTrxNo.Focus();
                        //SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        int pelPK = 0;
                        int.TryParse(hdfCurPelPk.Value, out pelPK);
                        tempList = AppraisalPayDetailsList;
                        if (pelPK > 0)
                        {
                            List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails> objExtraDaysList = tempList.DeepClone();
                            BulkAppraisalBO.SalaryBulkPayAppraisalDetails objExtraDays = AppraisalPayDetailsList.Where(x => x.EBD_PAY_ELEMENT == pelPK).SingleOrDefault();
                            objExtraDays.EBD_PAY_ELEMENT = pelPK;
                            objExtraDays.EBD_TYPE = ddlApplyAs.SelectedValue;
                            objExtraDays.EBD_TYPE_TEXT = (ddlApplyAs.SelectedItem.Text);
                            objExtraDays.EBD_INC_DECR = ddlIncrDecr.SelectedValue;
                            objExtraDays.EBD_INC_DECR_TEXT = (ddlIncrDecr.SelectedItem.Text);
                            objExtraDays.EBD_VALUE = Convert.ToDouble(txtValue.Text);
                            if (objExtraDays.PEL_IS_DEDUCTION == 0)
                                GetFieldValues(ControlsEnum.EARNINGSPAYELEMENTS);
                            else
                                GetFieldValues(ControlsEnum.DEDUCTIONPAYELEMENTS);
                        }
                        AppraisalPayDetailsList = tempList;
                        ResetForm(ControlsEnum.ADDTOLIST);
                        SetFieldValues(ControlsEnum.PAYELEMENTS);
                        break;
                    #endregion
                    #region EDIT, DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdEmpAppList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                HiddenField hdfDept;
                                int dept;
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEBH_PK")).Value);
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                Status = Convert.ToInt32(hdfStatus.Value);
                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                hdfIsCancelled.Value = hdfDelStatus.Value;
                                lblTrxNo.Focus();
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            FillProcessID(1);
                            GetFieldValues(ControlsEnum.APPRAISALTYPES);
                            SetFieldValues(ControlsEnum.APPRAISALTYPES);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            GetFieldValues(ControlsEnum.SALARYDETAILSBYPK);
                            SetFieldValues(ControlsEnum.SALARYDETAILSBYPK);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objSalaryAppraisalHeader = new BulkAppraisalBO.SalaryBulkAppraisalHeader();
                            objSalaryAppraisalHeader = (BulkAppraisalBO.SalaryBulkAppraisalHeader)SetUIValuesToObject(ControlsEnum.SALARYAPPRAISALHDR);
                            if (objSalaryAppraisalHeader != null)
                            {
                                if (objSalaryAppraisalHeader.AppraisalEmpDetails != null && objSalaryAppraisalHeader.AppraisalEmpDetails.Count > 0)
                                {
                                    string TrxNo = string.Empty;
                                    objSalaryAppraisalHeader.WKF_FLAG = 0;
                                    objSalaryAppraisalHeader.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string xmlDoc = CommonFunctions.XmlSerialize<BulkAppraisalBO.SalaryBulkAppraisalHeader>(objSalaryAppraisalHeader);
                                    result = BulkAppraisalBL.SaveBulkAppraisalDetails(xmlDoc, out TrxNo, out dtErrorList);
                                    if (result > 0)
                                    {
                                        hdfIsSaveContYes.Value = "0";
                                        lblTrxNo.Text = TrxNo;
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        object[] args = new object[1];
                                        args[0] = Resources.PageNameRes.BulkAppraisalDetails;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                        ResetForm(ControlsEnum.CLEAR);
                                        EntryStatus = EntryStatus.LISTMODE;
                                        CurrPK = (int)result;
                                        GetFieldValues(ControlsEnum.LIST);
                                        SetFieldValues(ControlsEnum.LIST);
                                    }
                                    else
                                    {
                                        if (result == (int)DbSaveStatus.SQLERROR)
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.INCORRECT)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_RecordExist").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKMCPRINTER)  // Appraisal Approval is already pending for this employee
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalAprvPending").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKZBPRINTER)  // Appraisal should be after {Prev Appraisal date}
                                        {
                                            strError = string.Empty;
                                            if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtErrorList.Rows)
                                                {
                                                    strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["BED_EMPLOYEE_TEXT"]) + " - " + Convert.ToDateTime(dr["EMP_PREV_APPR_DATE"]).ToString(Resources.Constants.HRMSDateFormatShort));
                                                }
                                            }
                                            litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalAfterPrvDate").ToString() + strError;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)  // Warning slary effective date change
                                        {
                                            hdfSaveSubmitYes.Value = (Convert.ToInt32(WorkflowTransactionFlag.SAVE)).ToString();
                                            strError = string.Empty;
                                            if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                            {
                                                foreach (DataRow dr in dtErrorList.Rows)
                                                {
                                                    strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["EMPLOYEE_TEXT"]));
                                                }
                                            }
                                            litErrorMsg.Text = GetLocalResourceObject("MsgSaveConfirm").ToString() + strError;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){SaveConfirmationMsg('" + litErrorMsg.Text + "');});", true);
                                            return;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BulkAppraisalDetails);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsForSave").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region EMPSEARCH
                    case ActionsEnum.EMPSEARCH:
                        GetFieldValues(ControlsEnum.EMPLOYEE);
                        SetFieldValues(ControlsEnum.EMPLOYEE);
                        SetFieldValues(ControlsEnum.PAYELEMENTS);
                        break;
                    #endregion
                    #region LIST
                    case ActionsEnum.LIST:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = "1";
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        hdfAdvSearch.Value = "1";
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR SEARCH
                    case ActionsEnum.CLEARSEARCH:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);

                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CHANGE TYPE
                    case ActionsEnum.CHANGETYPE:
                        GetFieldValues(ControlsEnum.INCRDECR);
                        SetFieldValues(ControlsEnum.INCRDECR);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region ADD NEW EARNINGS
                    case BusinessObject.AccountManagement.ActionsEnum.ADDNEWEARNINGS:
                        SetUIValuesToObject(ControlsEnum.PAYELEMENTS);
                        if (AppraisalPayDetailsList == null || AppraisalPayDetailsList.Count == 0)
                        {
                            AppraisalPayDetailsList = new List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails>();
                        }
                        GetFieldValues(ControlsEnum.EARNINGSPAYELEMENTS);
                        BulkAppraisalBO.SalaryBulkPayAppraisalDetails objDtl = new BulkAppraisalBO.SalaryBulkPayAppraisalDetails();
                        objDtl.EBD_PK = 0;
                        objDtl.STS_FORMULA_CODE = string.Empty;
                        objDtl.PEL_IS_DEDUCTION = Convert.ToByte(DeductMode.Earn);
                        objDtl.STS_SL_NO = GetCurrSequenceNo();
                        objDtl.EBD_CALC_MODE = -1;
                        objDtl.PEL_IN_SALARY = 1;
                        objDtl.EBD_IS_NEW = 1;
                        AppraisalPayDetailsList.Add(objDtl);
                        BindGrid(ControlsEnum.EARNINGS);
                        break;
                    #endregion
                    #region ADD NEW DEDUCTION
                    case BusinessObject.AccountManagement.ActionsEnum.ADDNEWDEDUCTION:
                        SetUIValuesToObject(ControlsEnum.PAYELEMENTS);
                        if (AppraisalPayDetailsList == null || AppraisalPayDetailsList.Count == 0)
                        {
                            AppraisalPayDetailsList = new List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails>();
                        }
                        GetFieldValues(ControlsEnum.DEDUCTIONPAYELEMENTS);
                        BulkAppraisalBO.SalaryBulkPayAppraisalDetails objDedDtl = new BulkAppraisalBO.SalaryBulkPayAppraisalDetails();
                        objDedDtl.EBD_PK = 0;
                        objDedDtl.STS_FORMULA_CODE = string.Empty;
                        objDedDtl.PEL_IS_DEDUCTION = Convert.ToByte(DeductMode.Deduct);
                        objDedDtl.STS_SL_NO = GetCurrSequenceNo();
                        objDedDtl.EBD_CALC_MODE = -1;
                        objDedDtl.PEL_IN_SALARY = 1;
                        objDedDtl.EBD_IS_NEW = 1;
                        AppraisalPayDetailsList.Add(objDedDtl);
                        BindGrid(ControlsEnum.DEDUCTIONS);
                        break;
                    #endregion
                    #region CHANGE EARNING DROPDOWN
                    case BusinessObject.AccountManagement.ActionsEnum.EARNINGCHANGE:
                        grvRow = (sender as DropDownList).Parent.Parent as GridViewRow;
                        ddlEarnPayElement = grvRow.FindControl("ddlEarnPayElement") as DropDownList;
                        imgPayMode = grvRow.FindControl("divEarnMode") as HtmlControl;
                        HiddenField hdfPelCalcModeErn = grvRow.FindControl("hdfPelCalcModeErn") as HiddenField;
                        Label lblGdValueErn = grvRow.FindControl("lblGdValueErn") as Label;
                        HiddenField hdfEarnPayElmtInSalary = grvRow.FindControl("hdfEarnPayElmtInSalary") as HiddenField;
                        hdfEarnSlNo = grvRow.FindControl("hdfEarnSlNo") as HiddenField;
                        HiddenField hdfPelPkErn = grvRow.FindControl("hdfPelPkErn") as HiddenField;
                        lblGdPayElmErn = grvRow.FindControl("lblGdPayElmErn") as Label;
                        Button btnEditDetailsErn = grvRow.FindControl("btnEditDetailsErn") as Button;

                        lblGdValueErn.Text = GetFormattedCurrency(0);
                        payElementErnDeductPK = Convert.ToInt32(ddlEarnPayElement.SelectedValue);

                        hdfPelCalcModeErn.Value = CommonConstants.SELECTVAL;

                        if (((DropDownList)sender).SelectedValue != CommonConstants.SELECTVAL)
                        {
                            var varSalaryDtlLst = AppraisalPayDetailsList.Where(x => x.EBD_PAY_ELEMENT == payElementErnDeductPK).ToList();
                            if (varSalaryDtlLst != null && varSalaryDtlLst.Count > 0)
                            {
                                ddlEarnPayElement.SelectedValue = CommonConstants.SELECTVAL;
                                litErrorMsg.Text = string.Format(Resources.ErrorMessages.Msg_PayElementAlreadyExists, varSalaryDtlLst[0].EBD_PAY_ELEMENT_TEXT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                    CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                BulkAppraisalBO.SalaryBulkPayAppraisalDetails objSalDtl = AppraisalPayDetailsList.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value) && row.PEL_IS_DEDUCTION == (int)DeductMode.Earn).SingleOrDefault();
                                objSalDtl.EBD_PAY_ELEMENT = payElementErnDeductPK;
                                objSalDtl.EBD_PAY_ELEMENT_TEXT = ddlEarnPayElement.SelectedItem.Text.HtmlEncode();
                                hdfPelPkErn.Value = payElementErnDeductPK.ToString();
                                lblGdPayElmErn.Text = ddlEarnPayElement.SelectedItem.Text.HtmlEncode();
                                GetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                                if (dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    hdfEarnPayElmtInSalary.Value = dtResult.Rows[0]["PEL_IN_SALARY"].ToString();
                                    objSalDtl.PEL_IN_SALARY = Convert.ToInt32(dtResult.Rows[0]["PEL_IN_SALARY"]);
                                    if (Convert.ToInt32(hdfEarnPayElmtInSalary.Value) == 1)
                                        grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                                    else
                                        grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                                    hdfPelCalcModeErn.Value = dtResult.Rows[0]["PEL_CALC_MODE"].ToString();
                                    objSalDtl.EBD_CALC_MODE = Convert.ToInt32(dtResult.Rows[0]["PEL_CALC_MODE"]);
                                    if (objSalDtl.EBD_CALC_MODE == 0)   // Fixed Only
                                        btnEditDetailsErn.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            AppraisalPayDetailsList.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfEarnSlNo.Value) && row.PEL_IS_DEDUCTION == (int)DeductMode.Earn).SingleOrDefault().EBD_PAY_ELEMENT = 0;
                        }
                        int.TryParse(hdfPelCalcModeErn.Value, out payElementMode);
                        SetPayelementIconCss(payElementMode, imgPayMode);
                        break;
                    #endregion
                    #region CHANGE DEDUCTION DROPDOWN
                    case BusinessObject.AccountManagement.ActionsEnum.DEDUCTIONCHANGE:
                        grvRow = (sender as DropDownList).Parent.Parent as GridViewRow;
                        ddlDeductPayElement = grvRow.FindControl("ddlDeductPayElement") as DropDownList;
                        hdfDeductSlNo = grvRow.FindControl("hdfDeductSlNo") as HiddenField;
                        imgPayMode = grvRow.FindControl("divDeductMode") as HtmlControl;
                        HiddenField hdfPelCalcModeDed = grvRow.FindControl("hdfPelCalcModeDed") as HiddenField;
                        Label lblGdValueDed = grvRow.FindControl("lblGdValueDed") as Label;
                        HiddenField hdfDedPayElmtInSalary = grvRow.FindControl("hdfDedPayElmtInSalary") as HiddenField;
                        HiddenField hdfPelPkDed = grvRow.FindControl("hdfPelPkDed") as HiddenField;
                        lblGdPayElmDed = grvRow.FindControl("lblGdPayElmDed") as Label;
                        Button btnEditDetailsDed = grvRow.FindControl("btnEditDetailsDed") as Button;
                        lblGdValueDed.Text = GetFormattedCurrency(0);
                        payElementErnDeductPK = Convert.ToInt32(ddlDeductPayElement.SelectedValue);
                        hdfPelCalcModeDed.Value = CommonConstants.SELECTVAL;

                        if (((DropDownList)sender).SelectedValue != CommonConstants.SELECTVAL)
                        {
                            var varSalaryDtlLst = AppraisalPayDetailsList.Where(x => x.EBD_PAY_ELEMENT == payElementErnDeductPK && x.STS_SL_NO != Convert.ToInt32(hdfDeductSlNo.Value)).ToList();
                            if (varSalaryDtlLst != null && varSalaryDtlLst.Count > 0)
                            {
                                ddlDeductPayElement.SelectedValue = CommonConstants.SELECTVAL;
                                litErrorMsg.Text = string.Format(Resources.ErrorMessages.Msg_PayElementAlreadyExists, varSalaryDtlLst[0].EBD_PAY_ELEMENT_TEXT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                    CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                BulkAppraisalBO.SalaryBulkPayAppraisalDetails objSalDtl = AppraisalPayDetailsList.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value) && row.PEL_IS_DEDUCTION == (int)DeductMode.Deduct).SingleOrDefault();
                                if (objSalDtl != null)
                                {
                                    objSalDtl.EBD_PAY_ELEMENT = payElementErnDeductPK;
                                    objSalDtl.EBD_PAY_ELEMENT_TEXT = ddlDeductPayElement.SelectedItem.Text.HtmlEncode(); ;
                                    hdfPelPkDed.Value = payElementErnDeductPK.ToString();
                                    lblGdPayElmDed.Text = ddlDeductPayElement.SelectedItem.Text.HtmlEncode(); ;
                                    GetFieldValues(ControlsEnum.PAYELEMENTDETAILS);
                                    if (dtResult != null && dtResult.Rows.Count > 0)
                                    {
                                        hdfDedPayElmtInSalary.Value = dtResult.Rows[0]["PEL_IN_SALARY"].ToString();
                                        objSalDtl.PEL_IN_SALARY = Convert.ToInt32(dtResult.Rows[0]["PEL_IN_SALARY"]);
                                        if (Convert.ToInt32(hdfDedPayElmtInSalary.Value) == 1)
                                            grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                                        else
                                            grvRow.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                                        hdfPelCalcModeDed.Value = dtResult.Rows[0]["PEL_CALC_MODE"].ToString();
                                        objSalDtl.EBD_CALC_MODE = Convert.ToInt32(dtResult.Rows[0]["PEL_CALC_MODE"]);
                                        if (objSalDtl.EBD_CALC_MODE == 0) // Fixed Amount only
                                            btnEditDetailsDed.Visible = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            AppraisalPayDetailsList.Where(row => row.STS_SL_NO == Convert.ToInt32(hdfDeductSlNo.Value) && row.PEL_IS_DEDUCTION == (int)DeductMode.Deduct).SingleOrDefault().EBD_PAY_ELEMENT = 0;
                        }
                        int.TryParse(hdfPelCalcModeDed.Value, out payElementMode);
                        SetPayelementIconCss(payElementMode, imgPayMode);
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.ADDTOLIST);
                        break;
                    #endregion
                    #region CLEAR EMPLOYEE SEARCH
                    case ActionsEnum.CLEARDETAIL:
                        ResetForm(ControlsEnum.CLEAREMPSEARCH);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BulkAppraisalBL.DeleteSalaryAppraisal(CurrPK, LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BulkAppraisalDetails);
                            ResetForm(ControlsEnum.CLEAR);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.UsedInAnotherPlace;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BulkAppraisalDetails);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup   
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKF SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objSalaryAppraisalHeader = new BulkAppraisalBO.SalaryBulkAppraisalHeader();
                                objSalaryAppraisalHeader = (BulkAppraisalBO.SalaryBulkAppraisalHeader)SetUIValuesToObject(ControlsEnum.SALARYAPPRAISALHDR);
                                if (objSalaryAppraisalHeader != null)
                                {
                                    if (objSalaryAppraisalHeader.AppraisalEmpDetails != null && objSalaryAppraisalHeader.AppraisalEmpDetails.Count > 0)
                                    {
                                        string TrxNo = string.Empty;
                                        objSalaryAppraisalHeader.WKF_FLAG = 1;
                                        SaveTransaction(objSalaryAppraisalHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsForSave").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel Emp Appraisal
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.EBA))
                                {
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_EppAppDtls_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    ResetForm(ControlsEnum.CLEAR);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);

                                }
                            }
                            else//Submit
                            {
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }

                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdEmpAppList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            HiddenField hdfDept;
                            int dept;
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEBH_PK")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ucrWrkf.Reset();
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlsEnum.SALARYDETAILSBYPK);
                            SetFieldValues(ControlsEnum.SALARYDETAILSBYPK);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrWrkf.ViewType = 1;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #region Save Transaction
        /// <summary>
        /// Save With workflow submition
        /// </summary>
        /// <param name="objEmpAppraisal"></param>
        private void SaveTransaction(BulkAppraisalBO.SalaryBulkAppraisalHeader objEmpAppraisal, int workflowFlag)
        {
            int? result = 0;
            strError = string.Empty;
            dtErrorList = new DataTable();
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objEmpAppraisal == null)
                objEmpAppraisal = new BulkAppraisalBO.SalaryBulkAppraisalHeader();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objEmpAppraisal.USER_PK = wkfDetails.UserPK;
            objEmpAppraisal.WKF_APPLICATION = CurrPK;
            objEmpAppraisal.WKF_COMMENTS = wkfDetails.Comments;
            objEmpAppraisal.WKF_TRX_FLAG = workflowFlag;
            objEmpAppraisal.WKF_PROCESS = wkfDetails.ProcessID;
            objEmpAppraisal.WKF_REFERENCE = wkfDetails.ReferenceID;
            objEmpAppraisal.WKF_TASK = wkfDetails.TaskID;
            objEmpAppraisal.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<BulkAppraisalBO.SalaryBulkAppraisalHeader>(objEmpAppraisal);
            result = BulkAppraisalBL.SaveBulkAppraisalDetails(xmlDoc, out TrxNo, out dtErrorList);
            if (result > 0)
            {
                hdfIsSaveContYes.Value = "0";
                if (!string.IsNullOrEmpty(TrxNo))
                    lblTrxNo.Text = TrxNo;
                //ucrWrkf.ApplicationID = result.Value;
                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                {
                    FillProcessID(1);
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                }
                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                ((TextBox)ucrWrkf.FindControl("WrkfComments")).Text = string.Empty;//Clear Workflow comments

                object[] args = new object[2];
                args[0] = Resources.PageNameRes.BulkAppraisalDetails;
                args[1] = lblTrxNo.Text.Trim();
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                // Show Save Message and redired to listing page                                      
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                }
            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.BulkAppraisalDetails + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.INCORRECT)
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_RecordExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKMCPRINTER)  // Appraisal Approval is already pending for this employee
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalAprvPending").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKZBPRINTER)  // Appraisal should be after {Prev Appraisal date}
                {
                    strError = string.Empty;
                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtErrorList.Rows)
                        {
                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["BED_EMPLOYEE_TEXT"]) + " - " + Convert.ToDateTime(dr["EMP_PREV_APPR_DATE"]).ToString(Resources.Constants.HRMSDateFormatShort));
                        }
                    }
                    litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalAfterPrvDate").ToString() + strError;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKCURRENCYMASTER)  // Not allow already have pending transfer
                {
                    hdfSaveSubmitYes.Value = (Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT)).ToString();
                    strError = string.Empty;
                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtErrorList.Rows)
                        {
                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["EFD_EMPLOYEE"]));
                        }
                    }
                    litErrorMsg.Text = GetLocalResourceObject("Msg_TransferPending").ToString() + strError;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)  // Warning slary effective date change
                {
                    hdfSaveSubmitYes.Value = (Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT)).ToString();
                    strError = string.Empty;
                    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dtErrorList.Rows)
                        {
                            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["EMPLOYEE_TEXT"]));
                        }
                    }
                    litErrorMsg.Text = GetLocalResourceObject("MsgSaveConfirm").ToString() + strError;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){SaveConfirmationMsg('" + litErrorMsg.Text + "');});", true);
                  //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                    return;
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.BulkAppraisalDetails);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
            }
        }
        #endregion

        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (e.CommandName == "EDIT_ACTION")
            {
                if (senderGridView.ID == "grdEmpEarnings")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfPelPkErn = row.FindControl("hdfPelPkErn") as HiddenField;
                    Label lblGdPayElm = row.FindControl("lblGdPayElmErn") as Label;
                    HiddenField hdfApplyTypeErn = row.FindControl("hdfApplyTypeErn") as HiddenField;
                    HiddenField hdfIncDcrErn = row.FindControl("hdfIncDcrErn") as HiddenField;
                    Label lblGdValue = row.FindControl("lblGdValueErn") as Label;
                    HiddenField HdfPayElmntNewErn = row.FindControl("HdfPayElmntNewErn") as HiddenField;

                    hdfCurPelPk.Value = hdfPelPkErn.Value;
                    txtPayElementText.Text = lblGdPayElm.Text;
                    if (Convert.ToInt32(HdfPayElmntNewErn.Value) == 0)
                    {
                        ddlApplyAs.Enabled = true;
                        ddlIncrDecr.Enabled = true;
                        if (hdfApplyTypeErn.Value != string.Empty)
                            ddlApplyAs.SelectedValue = hdfApplyTypeErn.Value.ToString();
                        GetFieldValues(ControlsEnum.INCRDECR);
                        SetFieldValues(ControlsEnum.INCRDECR);
                        if (hdfIncDcrErn.Value != string.Empty)
                            ddlIncrDecr.SelectedValue = hdfIncDcrErn.Value.ToString();
                    }
                    else   // New Mode
                    {
                        ddlApplyAs.SelectedValue = GetLocalResourceObject("ValueApply").ToString();
                        ddlApplyAs.Enabled = false;
                        GetFieldValues(ControlsEnum.INCRDECR);
                        SetFieldValues(ControlsEnum.INCRDECR);
                        ddlIncrDecr.SelectedValue = GetLocalResourceObject("EqualIncrDec").ToString();
                        ddlIncrDecr.Enabled = false;
                    }
                    txtValue.Text = lblGdValue.Text.ToString();
                    SetFieldValues(ControlsEnum.PAYELEMENTS);

                }
                else if (senderGridView.ID == "grdEmpDeductions")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfPEL_PK = row.FindControl("hdfPelPkDed") as HiddenField;
                    Label lblGdPayElm = row.FindControl("lblGdPayElmDed") as Label;
                    HiddenField hdfApplyTypeDed = row.FindControl("hdfApplyTypeDed") as HiddenField;
                    HiddenField hdfTypIncDecDed = row.FindControl("hdfTypIncDecDed") as HiddenField;
                    Label lblGdValue = row.FindControl("lblGdValueDed") as Label;
                    HiddenField HdfPayElmntNewDed = row.FindControl("HdfPayElmntNewDed") as HiddenField;

                    hdfCurPelPk.Value = hdfPEL_PK.Value;
                    txtPayElementText.Text = lblGdPayElm.Text.ToString();
                    if (Convert.ToInt32(HdfPayElmntNewDed.Value) == 0)
                    {
                        ddlApplyAs.Enabled = true;
                        ddlIncrDecr.Enabled = true;
                        if (hdfApplyTypeDed.Value != string.Empty)
                            ddlApplyAs.SelectedValue = hdfApplyTypeDed.Value.ToString();
                        GetFieldValues(ControlsEnum.INCRDECR);
                        SetFieldValues(ControlsEnum.INCRDECR);
                        if (hdfTypIncDecDed.Value != string.Empty)
                            ddlIncrDecr.SelectedValue = hdfTypIncDecDed.Value.ToString();
                    }
                    else   // New Payelement Mode
                    {
                        ddlApplyAs.SelectedValue = GetLocalResourceObject("ValueApply").ToString();
                        ddlApplyAs.Enabled = false;
                        GetFieldValues(ControlsEnum.INCRDECR);
                        SetFieldValues(ControlsEnum.INCRDECR);
                        ddlIncrDecr.SelectedValue = GetLocalResourceObject("EqualIncrDec").ToString();
                        ddlIncrDecr.Enabled = false;
                    }
                    txtValue.Text = lblGdValue.Text.ToString();
                    SetFieldValues(ControlsEnum.PAYELEMENTS);
                }
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdEmpEarnings")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        DropDownList ddlEarnPayElement = e.Row.FindControl("ddlEarnPayElement") as DropDownList;
                        Label lblGdPayElmErn = e.Row.FindControl("lblGdPayElmErn") as Label;
                        HiddenField hdfPelPkErn = e.Row.FindControl("hdfPelPkErn") as HiddenField;
                        HtmlControl divEarnMode = e.Row.FindControl("divEarnMode") as HtmlControl;
                        HiddenField hdfPelCalcModeErn = e.Row.FindControl("hdfPelCalcModeErn") as HiddenField;
                        HiddenField hdfEarnPayElmtInSalary = e.Row.FindControl("hdfEarnPayElmtInSalary") as HiddenField;
                        HiddenField HdfPayElmntNewErn = e.Row.FindControl("HdfPayElmntNewErn") as HiddenField;

                        if (Convert.ToInt32(HdfPayElmntNewErn.Value) == 0 || Convert.ToInt32(hdfPelPkErn.Value) > 0)
                        {
                            ddlEarnPayElement.Visible = false;
                            lblGdPayElmErn.Visible = true;
                        }
                        else
                        {
                            ddlEarnPayElement.Visible = true;
                            lblGdPayElmErn.Visible = false;
                            BindDropDown(ControlsEnum.EARNINGSPAYELEMENTS, ddlEarnPayElement);
                        }
                        SetPayelementIconCss(Convert.ToInt32(hdfPelCalcModeErn.Value), divEarnMode);
                        if (Convert.ToInt32(hdfEarnPayElmtInSalary.Value) == 1)
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                        else
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                    }
                }
                if (((GridView)sender).ID == "grdEmpDeductions")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        DropDownList ddlDeductPayElement = e.Row.FindControl("ddlDeductPayElement") as DropDownList;
                        Label lblGdPayElmDed = e.Row.FindControl("lblGdPayElmDed") as Label;
                        HiddenField hdfPelPkDed = e.Row.FindControl("hdfPelPkDed") as HiddenField;
                        HiddenField hdfPelCalcModeErn = e.Row.FindControl("hdfPelCalcModeErn") as HiddenField;
                        HtmlControl divDeductMode = e.Row.FindControl("divDeductMode") as HtmlControl;
                        HiddenField hdfPelCalcModeDed = e.Row.FindControl("hdfPelCalcModeDed") as HiddenField;
                        HiddenField hdfDedPayElmtInSalary = e.Row.FindControl("hdfDedPayElmtInSalary") as HiddenField;
                        HiddenField HdfPayElmntNewDed = e.Row.FindControl("HdfPayElmntNewDed") as HiddenField;

                        if (Convert.ToInt32(HdfPayElmntNewDed.Value) == 0 || Convert.ToInt32(hdfPelPkDed.Value) > 0)
                        {
                            ddlDeductPayElement.Visible = false;
                            lblGdPayElmDed.Visible = true;
                        }
                        else
                        {
                            ddlDeductPayElement.Visible = true;
                            lblGdPayElmDed.Visible = false;
                            BindDropDown(ControlsEnum.DEDUCTIONPAYELEMENTS, ddlDeductPayElement);
                        }
                        SetPayelementIconCss(Convert.ToInt32(hdfPelCalcModeDed.Value), divDeductMode);
                        if (Convert.ToInt32(hdfDedPayElmtInSalary.Value) == 1)
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "RowColor").ToString());
                        else
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("Controls", "PayElmtNotInSalary").ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            try
            {
                FilterParameters objFilterParam;
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        objFilterParam = new FilterParameters();
                        objFilterParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtSrchFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtSrchFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtSrchToDate.Text) ? (DateTime?)null : DateTime.Parse(txtSrchToDate.Text);
                        objFilterParam.Type = Convert.ToInt32(ddlSrchType.SelectedValue) >= 0 ? Convert.ToInt32(ddlSrchType.SelectedValue) : (int?)null;
                        objFilterParam.UserPK = currentUser.PKUser;
                        objFilterParam.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        dsPageData = BulkAppraisalBL.GetEmployeeAppraisalList(objFilterParam);
                        break;
                    #endregion
                    #region EMPLOYEE
                    case ControlsEnum.EMPLOYEE:
                        objFilterParam = new FilterParameters();
                        objFilterParam.Department = GetNullableInt(hdfDepartment.Value) > 0 ? GetNullableInt(hdfDepartment.Value) : null;
                        objFilterParam.Designation = GetNullableInt(hdfDesignation.Value) > 0 ? GetNullableInt(hdfDesignation.Value) : null;
                        objFilterParam.EmpDoj = string.IsNullOrEmpty(txtEmpDoj.Text) ? (DateTime?)null : DateTime.Parse(txtEmpDoj.Text);
                        objFilterParam.EmpAppDate = string.IsNullOrEmpty(txtLastAppDate.Text) ? (DateTime?)null : DateTime.Parse(txtLastAppDate.Text);
                        objFilterParam.Date = string.IsNullOrEmpty(txtDate.Text) ? (DateTime?)null : DateTime.Parse(txtDate.Text);
                        objFilterParam.BizUnit = currentUser.SBUID;
                        objFilterParam.UserPK = currentUser.PKUser;
                        objEmpHeader = BulkAppraisalBL.GetEmployeeAppraisalDetails(objFilterParam);
                        AppraisalEMPDetailsList = objEmpHeader.EmpInfoDtl;
                        AppraisalPayDetailsList = objEmpHeader.EmpPayElementDtl;
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region Appraisal Type
                    case ControlsEnum.APPRAISALTYPES:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("AppraisalType").ToString(), GetLocalResourceObject("Spl_Cndtn_Bulk").ToString());
                        break;
                    #endregion
                    #region  SALARY DETAILS BY PK
                    case ControlsEnum.SALARYDETAILSBYPK:
                        objSalaryAppraisalHeader = BulkAppraisalBL.GetSalaryAppraisalByPk(CurrPK);
                        break;
                    #endregion
                    #region APPLY AS
                    case ControlsEnum.APPLYAS:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("BULKAPPRAISALCALCTYPE").ToString());
                        break;
                    #endregion
                    #region INCREMENT/ DECREMENT
                    case ControlsEnum.INCRDECR:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("BULKAPPRAISALCALCVALUE").ToString(),
                            Convert.ToInt32(ddlApplyAs.SelectedValue) == 1 ? GetLocalResourceObject("Percentage").ToString() : string.Empty);
                        break;
                    #endregion
                    #region EARNINGS PAYELEMENTS
                    case ControlsEnum.EARNINGSPAYELEMENTS:
                        dtEarningData = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetEarnDeductPayElements(payElementErnDeductPK, Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, Convert.ToInt32(DeductMode.Earn));
                        break;
                    #endregion
                    #region DEDUCTION PAYELEMENTS
                    case ControlsEnum.DEDUCTIONPAYELEMENTS:
                        dtDeductionData = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetEarnDeductPayElements(payElementErnDeductPK, Convert.ToInt32(CommonConstants.ACTIVE), currentUser.SBUID, Convert.ToInt32(DeductMode.Deduct));
                        break;
                    #endregion
                    #region PAY ELEMENT DETAILS
                    case ControlsEnum.PAYELEMENTDETAILS:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.PayElementsMasterBL.GetParentElement(payElementErnDeductPK, Convert.ToInt32(DbActiveStatus.HASPK), currentUser.SBUID, 0, 0, -1);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region EMPLOYEE
                    case ControlsEnum.EMPLOYEE:
                        BindGrid(ControlsEnum.EMPLOYEE);
                        break;
                    #endregion
                    #region PAYELEMENTS
                    case ControlsEnum.PAYELEMENTS:
                        BindGrid(ControlsEnum.EARNINGS);
                        BindGrid(ControlsEnum.DEDUCTIONS);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        // BindDropDown(ControlsEnum.COMPANY,ddl);
                        break;
                    #endregion
                    #region Appraisal Type
                    case ControlsEnum.APPRAISALTYPES:
                        BindDropDown(ControlsEnum.APPRAISALTYPES, ddlType);
                        break;
                    #endregion
                    #region Appraisal Type List
                    case ControlsEnum.APPRAISALTYPESLIST:
                        BindDropDown(ControlsEnum.APPRAISALTYPES, ddlSrchType);
                        break;
                    #endregion
                    #region APPLY AS
                    case ControlsEnum.APPLYAS:
                        BindDropDown(ControlsEnum.APPLYAS, ddlApplyAs);
                        break;
                    #endregion
                    #region INCREMENT/ DECREMENT
                    case ControlsEnum.INCRDECR:
                        BindDropDown(ControlsEnum.INCRDECR, ddlIncrDecr);
                        break;
                    #endregion
                    #region SALARY DETAILS BY PK
                    case ControlsEnum.SALARYDETAILSBYPK:
                        GetUIValuesFromObject(ControlsEnum.SALARYDETAILSBYPK);
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

        #region Sets the UI input controls from the object values
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EMPLOYEE SALARY DETAILS BY PK
                    case ControlsEnum.SALARYDETAILSBYPK:
                        if (objSalaryAppraisalHeader != null)
                        {
                            lblTrxNo.Text = string.IsNullOrEmpty(objSalaryAppraisalHeader.EBH_NO) ? Resources.ErpRes.Draft : objSalaryAppraisalHeader.EBH_NO;
                            txtDate.Text = objSalaryAppraisalHeader.EBH_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                            ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(objSalaryAppraisalHeader.EBH_TYPE.ToString()));
                            txtEffectFrom.Text = objSalaryAppraisalHeader.EBH_EFFECT_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                            txtRefNo.Text = objSalaryAppraisalHeader.EBH_REF_NO.HtmlDecode();
                            txtRefDate.Text = objSalaryAppraisalHeader.EBH_REF_DATE == null ? string.Empty : Convert.ToDateTime(objSalaryAppraisalHeader.EBH_REF_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtRemarks.Text = objSalaryAppraisalHeader.EBH_REMARKS.HtmlDecode();
                            txtComments.Text = objSalaryAppraisalHeader.EBH_DESC.HtmlDecode();
                            LastModifiedTime = objSalaryAppraisalHeader.LAST_MOD_DT;
                            Status = objSalaryAppraisalHeader.EIH_STATUS;
                            AppraisalEMPDetailsList = objSalaryAppraisalHeader.AppraisalEmpDetails;
                            AppraisalPayDetailsList = objSalaryAppraisalHeader.AppraisalPayDetails;
                            SetFieldValues(ControlsEnum.EMPLOYEE);
                            SetFieldValues(ControlsEnum.PAYELEMENTS);
                            LastModifiedTime = objSalaryAppraisalHeader.LAST_MOD_DT;
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

        #region BindDropDown
        private void BindDropDown(ControlsEnum controlType, DropDownList ddlControl)
        {
            switch (controlType)
            {
                #region Appraisal Type
                case ControlsEnum.APPRAISALTYPES:
                    ddlControl.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlControl.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                        ddlControl.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlControl.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlControl.DataBind();
                    }
                    ddlControl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Appraisal Type List
                case ControlsEnum.APPRAISALTYPESLIST:
                    ddlControl.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlControl.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                        ddlControl.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlControl.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlControl.DataBind();
                    }
                    ddlControl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region APPLYAS
                case ControlsEnum.APPLYAS:
                    ddlControl.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlControl.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                        ddlControl.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlControl.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlControl.DataBind();
                    }
                    // ddlApplyAs.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region INCREMENT/ DECREMENT
                case ControlsEnum.INCRDECR:
                    ddlControl.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlControl.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                        ddlControl.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlControl.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlControl.DataBind();
                    }
                    // ddlIncrDecr.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region EARNINGS PAYELEMENTS
                case ControlsEnum.EARNINGSPAYELEMENTS:
                    ddlControl.Items.Clear();
                    if (dtEarningData != null && dtEarningData.Rows.Count > 0)
                    {
                        ddlControl.DataSource = dtEarningData;
                        ddlControl.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                        ddlControl.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;
                        ddlControl.Items.HtmlDecode();
                        ddlControl.DataBind();
                    }
                    ddlControl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region DEDUCTION PAYELEMENTS
                case ControlsEnum.DEDUCTIONPAYELEMENTS:
                    ddlControl.Items.Clear();
                    if (dtDeductionData != null && dtDeductionData.Rows.Count > 0)
                    {
                        ddlControl.DataSource = dtDeductionData;
                        ddlControl.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_NAME;
                        ddlControl.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.F_PEL_PK;
                        ddlControl.Items.HtmlDecode();
                        ddlControl.DataBind();
                    }
                    ddlControl.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
            }
        }
        #endregion

        #region BindGrid
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        int rowCount = 0;
                        int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        if (dsPageData.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdEmpAppList.DataSource = dsPageData.Tables[0];
                        grdEmpAppList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion
                    #region EMPLOYEE LIST
                    case ControlsEnum.EMPLOYEE:
                        if (AppraisalEMPDetailsList != null && AppraisalEMPDetailsList.Count > 0)
                            grdEmpList.DataSource = AppraisalEMPDetailsList;
                        else
                            grdEmpList.DataSource = null;
                        grdEmpList.DataBind();
                        break;
                    #endregion
                    #region EARNINGS
                    case ControlsEnum.EARNINGS:
                        if (AppraisalPayDetailsList != null && AppraisalPayDetailsList.Count > 0)
                            grdEmpEarnings.DataSource = AppraisalPayDetailsList.Where(x => x.PEL_IS_DEDUCTION == 0);
                        else
                            grdEmpEarnings.DataSource = null;
                        grdEmpEarnings.DataBind();
                        break;
                    #endregion
                    #region DEDUCTIONS
                    case ControlsEnum.DEDUCTIONS:
                        if (AppraisalPayDetailsList != null && AppraisalPayDetailsList.Count > 0)
                            grdEmpDeductions.DataSource = AppraisalPayDetailsList.Where(x => x.PEL_IS_DEDUCTION == 1);
                        else
                            grdEmpDeductions.DataSource = null;
                        grdEmpDeductions.DataBind();
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

        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                // Should we disable the first link
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                // Should we disable the previous link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
                // Should we enable the next link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
                // Should we enable the last link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            }
        }
        #endregion

        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            object returnObject = new object();
            switch (controlType)
            {
                #region SALARYaAPPRAISALHDR
                case ControlsEnum.SALARYAPPRAISALHDR:
                    BulkAppraisalBO.SalaryBulkAppraisalHeader tempAppraisalMaster = new BulkAppraisalBO.SalaryBulkAppraisalHeader();
                    tempAppraisalMaster.EBH_PK = CurrPK;
                    tempAppraisalMaster.EBH_DATE = Convert.ToDateTime(txtDate.Text);
                    tempAppraisalMaster.EBH_NO = lblTrxNo.Text;
                    tempAppraisalMaster.EBH_REMARKS = txtRemarks.Text.HtmlEncode();
                    tempAppraisalMaster.EBH_TYPE = Convert.ToInt32(ddlType.SelectedValue);
                    tempAppraisalMaster.EBH_EFFECT_DATE = Convert.ToDateTime(txtEffectFrom.Text);
                    GetFieldValues(ControlsEnum.COMPANY);
                    tempAppraisalMaster.EBH_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    tempAppraisalMaster.USER_PK = currentUser.PKUser;
                    tempAppraisalMaster.EBH_DEPT = currentUser.CurrentDeptPK;
                    tempAppraisalMaster.EBH_BIZUNIT = currentUser.SBUID;
                    tempAppraisalMaster.LAST_MOD_DT = LastModifiedTime;
                    tempAppraisalMaster.EBH_DESC = txtComments.Text.HtmlEncode();
                    tempAppraisalMaster.EBH_REF_NO = txtRefNo.Text.HtmlEncode();
                    tempAppraisalMaster.EBH_REF_DATE = txtRefDate.Text == string.Empty ? null : txtRefDate.Text;
                    tempAppraisalMaster.EBH_EFCT_IS_FLAG = Convert.ToInt16(hdfIsSaveContYes.Value);
                    SetUIValuesToObject(ControlsEnum.EMPAPPRAISALDETAILS);
                    tempAppraisalMaster.AppraisalEmpDetails = AppraisalEMPDetailsList;
                    tempAppraisalMaster.AppraisalPayDetails = AppraisalPayDetailsList;
                    returnObject = tempAppraisalMaster;
                    break;
                #endregion
                #region EMP APPRAISALDETAILS
                case ControlsEnum.EMPAPPRAISALDETAILS:
                    AppraisalEMPDetailsList = new List<BulkAppraisalBO.SalaryBulkEmpDetails>();
                    foreach (GridViewRow grdRow in grdEmpList.Rows)
                    {
                        CheckBox chkEmpselect = (CheckBox)grdRow.FindControl("chkEmpselect");
                        HiddenField hdfBED_PK = (HiddenField)grdRow.FindControl("hdfBED_PK");
                        HiddenField hdfBED_EBH_PK = (HiddenField)grdRow.FindControl("hdfBED_EBH_PK");
                        HiddenField hdfBED_EMPLOYEE = (HiddenField)grdRow.FindControl("hdfBED_EMPLOYEE");
                        HiddenField hdfBED_EMP_DEPT = (HiddenField)grdRow.FindControl("hdfBED_EMP_DEPT");
                        HiddenField hdfBED_EMP_DESGN = (HiddenField)grdRow.FindControl("hdfBED_EMP_DESGN");
                        Label lblGdLastAppDate = (Label)grdRow.FindControl("lblGdLastAppDate");
                        BulkAppraisalBO.SalaryBulkEmpDetails objEmpSalary = new BulkAppraisalBO.SalaryBulkEmpDetails();
                        if (chkEmpselect.Checked == true)
                        {
                            objEmpSalary.BED_PK = Convert.ToInt32(hdfBED_PK.Value);
                            objEmpSalary.BED_EBH_PK = Convert.ToInt32(hdfBED_EBH_PK.Value);
                            objEmpSalary.BED_EMPLOYEE = Convert.ToInt32(hdfBED_EMPLOYEE.Value);
                            objEmpSalary.BED_EMP_DEPT = Convert.ToInt32(hdfBED_EMP_DEPT.Value);
                            objEmpSalary.BED_EMP_DESGN = Convert.ToInt32(hdfBED_EMP_DESGN.Value);
                            objEmpSalary.BED_EMP_PREV_APPR_DATE = lblGdLastAppDate.Text;
                            AppraisalEMPDetailsList.Add(objEmpSalary);
                        }
                    }
                    SetUIValuesToObject(ControlsEnum.PAYELEMENTS);
                    break;
                #endregion
                #region PAYELEMENTS
                case ControlsEnum.PAYELEMENTS:
                    AppraisalPayDetailsList = new List<BulkAppraisalBO.SalaryBulkPayAppraisalDetails>();
                    #region Earnings
                    foreach (GridViewRow grdRow in grdEmpEarnings.Rows)
                    {
                        HiddenField hdfEBDPkErn = (HiddenField)grdRow.FindControl("hdfEBDPkErn");
                        HiddenField hdfEBHPkErn = (HiddenField)grdRow.FindControl("hdfEBHPkErn");
                        HiddenField hdfPelPkErn = (HiddenField)grdRow.FindControl("hdfPelPkErn");
                        HiddenField hdfPelCalcModeErn = (HiddenField)grdRow.FindControl("hdfPelCalcModeErn");
                        HiddenField hdfEarnPayElmtInSalary = (HiddenField)grdRow.FindControl("hdfEarnPayElmtInSalary");
                        HiddenField hdfEarnSlNo = (HiddenField)grdRow.FindControl("hdfEarnSlNo");
                        HiddenField hdfPelDedc = (HiddenField)grdRow.FindControl("hdfPelDedc");
                        Label lblGdPayElmErn = (Label)grdRow.FindControl("lblGdPayElmErn");
                        HiddenField hdfApplyTypeErn = (HiddenField)grdRow.FindControl("hdfApplyTypeErn");
                        Label lblGdApplyAsErn = (Label)grdRow.FindControl("lblGdApplyAsErn");
                        HiddenField hdfIncDcrErn = (HiddenField)grdRow.FindControl("hdfIncDcrErn");
                        Label lblGdIncrDecrErn = (Label)grdRow.FindControl("lblGdIncrDecrErn");
                        Label lblGdValue = (Label)grdRow.FindControl("lblGdValueErn");
                        CheckBox chkPayElmDeleteErn = (CheckBox)grdRow.FindControl("chkPayElmDeleteErn");
                        HiddenField HdfPayElmntNewErn = (HiddenField)grdRow.FindControl("HdfPayElmntNewErn");
                        BulkAppraisalBO.SalaryBulkPayAppraisalDetails objPayErn = new BulkAppraisalBO.SalaryBulkPayAppraisalDetails();
                        // if (Convert.ToDouble( lblGdValue.Text)>0)
                        // {
                        objPayErn.EBD_PK = Convert.ToInt32(hdfEBDPkErn.Value);
                        objPayErn.EBD_EBH_PK = Convert.ToInt32(hdfEBHPkErn.Value);
                        objPayErn.EBD_PAY_ELEMENT = Convert.ToInt32(hdfPelPkErn.Value);
                        objPayErn.EBD_CALC_MODE = Convert.ToInt32(hdfPelCalcModeErn.Value);
                        objPayErn.PEL_IN_SALARY = Convert.ToInt32(hdfEarnPayElmtInSalary.Value);
                        objPayErn.STS_SL_NO = Convert.ToInt32(hdfEarnSlNo.Value);
                        objPayErn.PEL_IS_DEDUCTION = Convert.ToInt32(hdfPelDedc.Value);
                        objPayErn.EBD_PAY_ELEMENT_TEXT = lblGdPayElmErn.Text.HtmlEncode();
                        objPayErn.EBD_TYPE = hdfApplyTypeErn.Value == string.Empty ? null : hdfApplyTypeErn.Value;
                        objPayErn.EBD_TYPE_TEXT = lblGdApplyAsErn.Text;
                        objPayErn.EBD_INC_DECR = hdfIncDcrErn.Value == string.Empty ? null : hdfIncDcrErn.Value;
                        objPayErn.EBD_INC_DECR_TEXT = lblGdIncrDecrErn.Text;
                        objPayErn.EBD_VALUE = Convert.ToDouble(lblGdValue.Text);
                        objPayErn.EBD_DEL_STATUS = chkPayElmDeleteErn.Checked == true ? 1 : 0;
                        objPayErn.EBD_IS_NEW = Convert.ToInt32(HdfPayElmntNewErn.Value);
                        AppraisalPayDetailsList.Add(objPayErn);
                        //}
                    }
                    #endregion
                    #region Deductions
                    foreach (GridViewRow grdRow in grdEmpDeductions.Rows)
                    {
                        HiddenField hdfEBD_PK = (HiddenField)grdRow.FindControl("hdfEBD_PKDed");
                        HiddenField hdfEBHPkDed = (HiddenField)grdRow.FindControl("hdfEBHPkDed");
                        HiddenField hdfPEL_PK = (HiddenField)grdRow.FindControl("hdfPelPkDed");
                        HiddenField hdfPelCalcModeDed = (HiddenField)grdRow.FindControl("hdfPelCalcModeDed");
                        HiddenField hdfDedPayElmtInSalary = (HiddenField)grdRow.FindControl("hdfDedPayElmtInSalary");
                        HiddenField hdfDeductSlNo = (HiddenField)grdRow.FindControl("hdfDeductSlNo");
                        HiddenField hdfPelDedc = (HiddenField)grdRow.FindControl("hdfPelDedc");
                        Label lblGdPayElmDed = (Label)grdRow.FindControl("lblGdPayElmDed");
                        HiddenField hdfApplyTypeDed = (HiddenField)grdRow.FindControl("hdfApplyTypeDed");
                        Label lblGdApplyAsDed = (Label)grdRow.FindControl("lblGdApplyAsDed");
                        HiddenField hdfTypIncDecDed = (HiddenField)grdRow.FindControl("hdfTypIncDecDed");
                        Label lblGdIncrDecrDed = (Label)grdRow.FindControl("lblGdIncrDecrDed");
                        Label lblGdValueDed = (Label)grdRow.FindControl("lblGdValueDed");
                        CheckBox chkPayElmDelete = (CheckBox)grdRow.FindControl("chkPayElmDeleteDed");
                        HiddenField HdfPayElmntNewDed = (HiddenField)grdRow.FindControl("HdfPayElmntNewDed");

                        BulkAppraisalBO.SalaryBulkPayAppraisalDetails objPayDed = new BulkAppraisalBO.SalaryBulkPayAppraisalDetails();
                        // if (Convert.ToDouble(lblGdValueDed.Text) > 0)
                        // { 
                        objPayDed.EBD_PK = Convert.ToInt32(hdfEBD_PK.Value);
                        objPayDed.EBD_EBH_PK = Convert.ToInt32(hdfEBHPkDed.Value);
                        objPayDed.EBD_PAY_ELEMENT = Convert.ToInt32(hdfPEL_PK.Value);
                        objPayDed.EBD_CALC_MODE = Convert.ToInt32(hdfPelCalcModeDed.Value);
                        objPayDed.PEL_IN_SALARY = Convert.ToInt32(hdfDedPayElmtInSalary.Value);
                        objPayDed.STS_SL_NO = Convert.ToInt32(hdfDeductSlNo.Value);
                        objPayDed.PEL_IS_DEDUCTION = Convert.ToInt32(hdfPelDedc.Value);
                        objPayDed.EBD_PAY_ELEMENT_TEXT = lblGdPayElmDed.Text.HtmlEncode();
                        objPayDed.EBD_TYPE = hdfApplyTypeDed.Value == string.Empty ? null : hdfApplyTypeDed.Value;
                        objPayDed.EBD_TYPE_TEXT = lblGdApplyAsDed.Text;
                        objPayDed.EBD_INC_DECR = hdfTypIncDecDed.Value == string.Empty ? null : hdfTypIncDecDed.Value;
                        objPayDed.EBD_INC_DECR_TEXT = lblGdIncrDecrDed.Text;
                        objPayDed.EBD_VALUE = Convert.ToDouble(lblGdValueDed.Text);
                        objPayDed.EBD_DEL_STATUS = chkPayElmDelete.Checked == true ? 1 : 0;
                        objPayDed.EBD_IS_NEW = Convert.ToInt32(HdfPayElmntNewDed.Value);
                        AppraisalPayDetailsList.Add(objPayDed);
                        // }
                    }
                    #endregion
                    break;
                #endregion
            }
            return returnObject;
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region CLEAR
                case ControlsEnum.CLEAR:
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = "1";
                    CurrPK = 0;
                    txtSrchFromDate.Text = string.Empty;
                    txtSrchToDate.Text = string.Empty;
                    ddlSrchType.ClearSelection();
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    txtDate.Text = string.Empty;
                    ddlType.ClearSelection();
                    txtEffectFrom.Text = string.Empty;
                    txtRefNo.Text = string.Empty;
                    txtRefDate.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtComments.Text = string.Empty;
                    txtDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfDepartment.Value = CommonConstants.SELECTVAL;
                    txtDesignation.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfDesignation.Value = CommonConstants.SELECTVAL;
                    txtSrchFromDate.Text = string.Empty;
                    txtSrchToDate.Text = string.Empty;
                    ddlSrchType.ClearSelection();
                    txtEmpDoj.Text = string.Empty;
                    txtEffectFrom.Text = string.Empty;
                    AppraisalEMPDetailsList = null;
                    AppraisalPayDetailsList = null;
                    txtPayElementText.Text = string.Empty;
                    ddlApplyAs.SelectedIndex = 0;
                    ddlIncrDecr.SelectedIndex = 0;
                    txtValue.Text = string.Empty;
                    AppraisalEMPDetailsList = null;
                    AppraisalPayDetailsList = null;
                    base.WkfRefID = 0;
                    Status = 0;
                    hdfIsCancelled.Value = "0";
                    ddlStatus.ClearSelection();
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = "1";
                    txtSrchFromDate.Text = string.Empty;
                    txtSrchToDate.Text = string.Empty;
                    ddlSrchType.ClearSelection();
                    ddlStatus.ClearSelection();
                    break;
                #endregion
                #region  ADDTOLIST
                case ControlsEnum.ADDTOLIST:
                    ddlApplyAs.SelectedIndex = 0;
                    ddlIncrDecr.SelectedIndex = 0;
                    txtValue.Text = string.Empty;
                    txtPayElementText.Text = string.Empty;

                    break;
                #endregion
                #region CLEAR EMPLOYEE SEARCH
                case ControlsEnum.CLEAREMPSEARCH:
                    txtDesignation.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfDesignation.Value = CommonConstants.SELECTVAL;
                    txtDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfDepartment.Value = CommonConstants.SELECTVAL;
                    txtEmpDoj.Text = string.Empty;
                    txtLastAppDate.Text = string.Empty;
                    break;
                #endregion
            }
        }
        #endregion

        #region UtitlityMethods

        /// <summary>
        /// Set image css
        /// </summary>
        /// <param name="PayElementMode"></param>
        /// <param name="imgMode"></param>
        private void SetPayelementIconCss(int PayElementMode, HtmlControl imgMode)
        {
            switch (PayElementMode)
            {
                case (int)PayElementCalcMode.Custom:
                    imgMode.Attributes.Add("class", "custom-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Custom);
                    break;
                case (int)PayElementCalcMode.FixedAmount:
                    imgMode.Attributes.Add("class", "fixed-amount-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_FixedAmount);
                    break;
                case (int)PayElementCalcMode.Formula:
                    imgMode.Attributes.Add("class", "formula-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Formula);
                    break;
                case (int)PayElementCalcMode.Slab:
                    imgMode.Attributes.Add("class", "slab-icon");
                    imgMode.Attributes.Add("title", Resources.Controls.PayElementMode_Slab);
                    break;
                default:
                    imgMode.Attributes.Add("class", "hide");
                    break;
            }
        }

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
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            return (decimal.TryParse(str, out result) ? (decimal?)result : null);
        }
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            string format = "#0.0";
            string s = num.ToString(format);
            return s;
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
        public string GetSubstring(object str)
        {
            return ((string)str).Substring(0, 3);
        }
        private int GetCurrSequenceNo()
        {
            if (AppraisalPayDetailsList != null)
            {
                if (AppraisalPayDetailsList.Count > 0)
                    CurrSlNo = AppraisalPayDetailsList.Max(itm => itm.STS_SL_NO) + 1;
            }
            return CurrSlNo;
        }

        #endregion

        #region WorkFlow Methods

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
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(int pid)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    if (pid == 1)
                    {
                        PageProcessID = ucrWrkf.ProcessID;
                    }
                    base.WkfPageUrl = path;
                }
            }
        }
        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }
        private string GetUrl()
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            if (Request.QueryString[QueryStrings.PID] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=1";
                else
                    path = Request.Url.AbsolutePath.ToLower() + "?PID=1";
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                else
                    path = Request.Url.AbsolutePath.ToLower();
            }
            return path;
        }
        #endregion

        #region Deduct Mode
        public enum DeductMode
        {
            Earn = 0,
            Deduct = 1
        }
        #endregion
        #region Apply Mode
        public enum ApplyMode
        {
            Percentage = 0,
            Value = 1
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEE,
            PAYELEMENTS,
            DEDUCTIONS,
            EARNINGS,
            COMPANY,
            CLEAR,
            LIST,
            CLEARSEARCH,
            ADDTOLIST,
            CLEARELEMENTS,
            CLEAREMPSEARCH,
            APPRAISALTYPES,
            APPRAISALTYPESLIST,
            SALARYAPPRAISALHDR,
            EMPAPPRAISALDETAILS,
            SALARYDETAILSBYPK,
            APPLYAS,
            INCRDECR,
            EARNINGSPAYELEMENTS,
            DEDUCTIONPAYELEMENTS,
            PAYELEMENTDETAILS,

        }
        #endregion
    }
}