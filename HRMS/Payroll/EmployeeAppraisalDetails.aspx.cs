using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using BusinessObject.HRMS.Payroll;
using System.Xml;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERP.Utilities.HRMS;
using BusinessLogic.HRMS.Payroll;
using System.Threading;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class EmployeeAppraisalDetails : ERP.Store.UI.WorkFlowBasePage//: System.Web.UI.Page
    {
        #region Variables and Properties
        #region Variables
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private int CompanyPk = 0;
        private string refID;
        private string inboxFlag;
        private string prefID;
        private EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader objEmpAppraisalHeader;
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
        private List<EmployeeAppraisalDetailsBO.EmployeeAppraisalDetails> EMPAppraisaDetailsList
        {
            get
            {
                return ViewState[ViewstateStrings.EMPAppraisaDetailsList] == null ? new List<EmployeeAppraisalDetailsBO.EmployeeAppraisalDetails>() : (List<EmployeeAppraisalDetailsBO.EmployeeAppraisalDetails>)ViewState[ViewstateStrings.EMPAppraisaDetailsList];
            }
            set
            {
                ViewState[ViewstateStrings.EMPAppraisaDetailsList] = value;
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
        /// To keep Current Emp PK in view state
        /// </summary>
        private int CurrEmployeePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.EmployeePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EmployeePK] = value;
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch();", true);
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
            if (CurrPK == 0)
            {
                btnSubmit.Visible = false;
                btnCancelSubmit.Visible = false;
            }

            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
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
            catch (Exception ex)
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
                    // hdfExchangeRateFormat.Value = "#0.";
                    //int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                    //    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    //    : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    //for (int i = 0; i < exchrateDecimalDigits; i++)
                    //{
                    //    hdfExchangeRateFormat.Value += "0";
                    //}
                    ReferanceID = string.IsNullOrEmpty(refID)
                           ? string.IsNullOrEmpty(prefID)
                                 ? 0
                                 : int.Parse(prefID)
                           : int.Parse(refID);
                    FillProcessID(1);

                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.APPRAISALTYPES);
                    SetFieldValues(ControlsEnum.APPRAISALTYPES);

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
                        GetFieldValues(ControlsEnum.EMPLOYEESALARYDETAILSBYPK);
                        SetFieldValues(ControlsEnum.EMPLOYEESALARYDETAILSBYPK);
                    }
                    else
                    {
                        uclPaging.CurrentPage = 1;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                    lblTrxNo.Focus();
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

            UCEmpSalary.IsEmpAppraisal = Convert.ToInt32(VisbleStatusEnum.TRUE);
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
                int EffDay = 0;
                bool bIsChecked = false;
                TextBox WrkfComments;
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
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
                switch (commonActions)
                {
                    #region New
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.CLEAR);
                        hdfNewMode.Value = "0";
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.APPRAISALTYPES);
                        SetFieldValues(ControlsEnum.APPRAISALTYPES);
                        SetFieldValues(ControlsEnum.EMPLOYEESALARY);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        //SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        UCEmpSalary.EffectTo= DateTime.Now.AddYears(10).ToString();
                        
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
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEIH_PK")).Value);
                                CurrEmployeePK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEIH_EMPLOYEEt")).Value);
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
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            hdfNewMode.Value = "1";
                            FillProcessID(1);
                            SetUIEditView(commonActions);
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
                            GetFieldValues(ControlsEnum.EMPLOYEESALARYDETAILSBYPK);
                            SetFieldValues(ControlsEnum.EMPLOYEESALARYDETAILSBYPK);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region CHANGEEMPLOYEE
                    case ActionsEnum.CHANGEEMPLOYEE:
                        ResetForm(ControlsEnum.CLEARELEMENTS);
                        GetFieldValues(ControlsEnum.EMPLOYEESALARY);
                        SetFieldValues(ControlsEnum.EMPLOYEESALARY);
                        GetUIValuesFromObject(ControlsEnum.EMPLOYEESALARYHDRBYEMPPK);
                        ddlType.Focus();
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        int.TryParse(DateTime.Parse(txtEffectFrom.Text).ToString("dd"), out EffDay);
                        if (UCEmpSalary.MonthlyStartDay > 0 && EffDay != UCEmpSalary.MonthlyStartDay && hdfIsSaveContYes.Value != "1")
                        {
                            hdfSaveSubmitYes.Value = "0";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){SaveConfirmationMsg();});", true);
                            return;
                        }
                        else
                        {
                            objEmpAppraisalHeader = new EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader();
                            objEmpAppraisalHeader = (EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader)SetUIValuesToObject(ControlsEnum.EMPLOYEESALARYHDR);
                            if (objEmpAppraisalHeader != null)
                            {
                                if (objEmpAppraisalHeader.EmployeeAppraisalDtl != null && objEmpAppraisalHeader.EmployeeAppraisalDtl.Count > 0)
                                {
                                    string TrxNo = string.Empty;
                                    objEmpAppraisalHeader.WKF_FLAG = 0;
                                    objEmpAppraisalHeader.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string xmlDoc = CommonFunctions.XmlSerialize<EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader>(objEmpAppraisalHeader);
                                    result = EmployeeAppraisalDetailsBL.SaveEmployeeAppraisalDetails(xmlDoc, out TrxNo);
                                    if (result > 0)
                                    {
                                        hdfIsSaveContYes.Value = "0";
                                        lblTrxNo.Text = TrxNo;
                                        litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                        object[] args = new object[1];
                                        args[0] = Resources.PageNameRes.EmployeeAppraisalDetails;
                                        //args[1] = TrxNo;
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
                                            litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
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
                                            litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalAfterPrv").ToString();
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, lblPrevAppDateText.Text);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)  // dept Can't changed to prevoius date
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalDeptAfterPrv").ToString();
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, TrxNo);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.CHECKBOXPRINTER)  // Desingation Can't changed to prevoius date
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalDesigAfterPrv").ToString();
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, TrxNo);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeAppraisalDetails);
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
                    #region CLEARITEM
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.CLEAREMPLOYEESEARCH);
                        // GetFieldValues(ControlsEnum.EMPLOYEESALARY);
                        SetFieldValues(ControlsEnum.EMPLOYEESALARY);
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
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR SEARCH
                    case ActionsEnum.CLEARSEARCH:
                        EntryStatus = EntryStatus.LISTMODE;
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
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = EmployeeAppraisalDetailsBL.DeleteEmployeeAppraisal(CurrPK, LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeAppraisalDetails);
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
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.UsedInAnotherPlace;
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
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeAppraisalDetails);
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
                        int.TryParse(DateTime.Parse(txtEffectFrom.Text).ToString("dd"), out EffDay);
                        if (UCEmpSalary.MonthlyStartDay > 0 && EffDay != UCEmpSalary.MonthlyStartDay && hdfIsSaveContYes.Value != "1")
                        {
                            hdfSaveSubmitYes.Value = "1";
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){SaveConfirmationMsg();});", true);
                            return;
                        }
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
                                objEmpAppraisalHeader = new EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader();
                                objEmpAppraisalHeader = (EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader)SetUIValuesToObject(ControlsEnum.EMPLOYEESALARYHDR);
                                if (objEmpAppraisalHeader != null)
                                {
                                    if (objEmpAppraisalHeader.EmployeeAppraisalDtl != null && objEmpAppraisalHeader.EmployeeAppraisalDtl.Count > 0)
                                    {
                                        string TrxNo = string.Empty;
                                        objEmpAppraisalHeader.WKF_FLAG = 1;
                                        SaveTransaction(objEmpAppraisalHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
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
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.EAP))
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
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEIH_PK")).Value);
                                CurrEmployeePK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEIH_EMPLOYEEt")).Value);
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
                            GetFieldValues(ControlsEnum.EMPLOYEESALARYDETAILSBYPK);
                            SetFieldValues(ControlsEnum.EMPLOYEESALARYDETAILSBYPK);
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
        #endregion

        #region Save Transaction
        /// <summary>
        /// Save With workflow submition
        /// </summary>
        /// <param name="objEmpAppraisal"></param>
        private void SaveTransaction(EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader objEmpAppraisal, int workflowFlag)
        {
            int? result = 0;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objEmpAppraisal == null)
                objEmpAppraisal = new EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader();
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
            string xmlDoc = CommonFunctions.XmlSerialize<EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader>(objEmpAppraisal);
            result = EmployeeAppraisalDetailsBL.SaveEmployeeAppraisalDetails(xmlDoc, out TrxNo);
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
                args[0] = Resources.PageNameRes.EmployeeAppraisalDetails;
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
                    litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.EmployeeAppraisalDetails + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
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
                    litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalAfterPrv").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, lblPrevAppDateText.Text);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)  // dept Can't changed to prevoius date
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalDeptAfterPrv").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, TrxNo);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKBOXPRINTER)  // Desingation Can't changed to prevoius date
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_AppraisalDesigAfterPrv").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, TrxNo);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeAppraisalDetails);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                }
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
                        int empSrchPk = 0;
                        int.TryParse(hdfSrchEmployee.Value, out empSrchPk);
                        objFilterParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtSrchFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtSrchFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtSrchToDate.Text) ? (DateTime?)null : DateTime.Parse(txtSrchToDate.Text);
                        objFilterParam.Employee = empSrchPk > 0 ? empSrchPk : (int?)null;
                        objFilterParam.Type = Convert.ToInt32(ddlSrchType.SelectedValue) >= 0 ? Convert.ToInt32(ddlSrchType.SelectedValue) : (int?)null;
                        objFilterParam.UserPK = currentUser.PKUser;
                        objFilterParam.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        dsPageData = EmployeeAppraisalDetailsBL.GetEmployeeAppraisalDetails(objFilterParam);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region EMPLOYEE SALARY Details
                    case ControlsEnum.EMPLOYEESALARY: // Get employee salary details
                        UCEmpSalary.EmployeePK = Convert.ToInt32(hdfEmployee.Value);
                        UCEmpSalary.trxDate = txtDate.Text;
                        UCEmpSalary.GetFieldValues(BusinessObject.HRMS.Employee.ActionsEnum.EMPLOYEESALARY);
                        break;
                    #endregion
                    #region Appraisal Type
                    case ControlsEnum.APPRAISALTYPES:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("AppraisalType").ToString());
                        break;
                    #endregion
                    #region EMPLOYEE SALARY DETAILS BY PK
                    case ControlsEnum.EMPLOYEESALARYDETAILSBYPK:
                        objEmpAppraisalHeader = EmployeeAppraisalDetailsBL.GetEmployeeAppraisalByPk(CurrPK, CurrEmployeePK);
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
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region EMPLOYEESALARY
                    case ControlsEnum.EMPLOYEESALARY:
                        UCEmpSalary.SetFieldValues(BusinessObject.HRMS.Employee.ActionsEnum.EMPLOYEESALARY);
                        break;
                    #endregion
                    #region Appraisal Type
                    case ControlsEnum.APPRAISALTYPES:
                        BindDropDown(ControlsEnum.APPRAISALTYPES);
                        break;
                    #endregion
                    #region EMPLOYEE SALARY DETAILS BY PK
                    case ControlsEnum.EMPLOYEESALARYDETAILSBYPK:
                        GetUIValuesFromObject(ControlsEnum.EMPLOYEESALARYDETAILSBYPK);
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
                    #region EMPLOYEESALARYHDR BY EMPPK
                    case ControlsEnum.EMPLOYEESALARYHDRBYEMPPK:
                        if (UCEmpSalary.EmpSalaryHdr != null)
                        {
                            lblPrevAppDateText.Text = UCEmpSalary.EmpSalaryHdr.EIH_PREV_APPR_DATE == null ? string.Empty : Convert.ToDateTime(UCEmpSalary.EmpSalaryHdr.EIH_PREV_APPR_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            hdfDepartmentOld.Value = UCEmpSalary.EmpSalaryHdr.empDept.ToString();
                            lblDepartmentOldText.Text = ERP.Utilities.CommonFunctions.GetShortString(UCEmpSalary.EmpSalaryHdr.EMP_DEPT_TEXT, 25);
                            lblDepartmentOldText.ToolTip = UCEmpSalary.EmpSalaryHdr.EMP_DEPT_TEXT.HtmlDecode();
                            hdfDesignationOld.Value = UCEmpSalary.EmpSalaryHdr.empDesignation.ToString();
                            lblDesignationOldText.Text = ERP.Utilities.CommonFunctions.GetShortString(UCEmpSalary.EmpSalaryHdr.EMP_DESIGNATION_TEXT, 25);
                            lblDesignationOldText.ToolTip = UCEmpSalary.EmpSalaryHdr.EMP_DESIGNATION_TEXT.HtmlDecode();
                        }
                        break;
                    #endregion
                    #region EMPLOYEE SALARY DETAILS BY PK
                    case ControlsEnum.EMPLOYEESALARYDETAILSBYPK:
                        if (objEmpAppraisalHeader != null)
                        {
                            lblTrxNo.Text = string.IsNullOrEmpty(objEmpAppraisalHeader.EIH_NO) ? Resources.ErpRes.Draft : objEmpAppraisalHeader.EIH_NO;
                            txtDate.Text = objEmpAppraisalHeader.EIH_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                            txtEmployee.Text = objEmpAppraisalHeader.EIH_EMPLOYEE_TEXT.HtmlDecode(); ;
                            hdfEmployee.Value = Convert.ToString(objEmpAppraisalHeader.EIH_EMPLOYEE);
                            ddlType.SelectedIndex = ddlType.Items.IndexOf(ddlType.Items.FindByValue(objEmpAppraisalHeader.EIH_TYPE.ToString()));
                            txtEffectFrom.Text = objEmpAppraisalHeader.EIH_EFFECT_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                            lblPrevAppDateText.Text = objEmpAppraisalHeader.EIH_PREV_APPR_DATE == null ? string.Empty : Convert.ToDateTime(objEmpAppraisalHeader.EIH_PREV_APPR_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtRefNo.Text = objEmpAppraisalHeader.EIH_REF_NO.HtmlDecode();
                            txtRefDate.Text = objEmpAppraisalHeader.EIH_REF_DATE == null ? string.Empty : Convert.ToDateTime(objEmpAppraisalHeader.EIH_REF_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtRemarks.Text = objEmpAppraisalHeader.EIH_TRN_NAME.HtmlDecode();
                            txtComments.Text = objEmpAppraisalHeader.EIH_DESC.HtmlDecode();
                            txtDepartment.Text = objEmpAppraisalHeader.EIH_EMP_DEPT_TEXT.HtmlDecode();
                            hdfDepartment.Value = objEmpAppraisalHeader.EIH_EMP_DEPT == null ? CommonConstants.SELECTVAL : Convert.ToString(objEmpAppraisalHeader.EIH_EMP_DEPT);
                            txtDesignation.Text = objEmpAppraisalHeader.EIH_EMP_DESGN_TEXT.HtmlDecode();
                            hdfDesignation.Value = objEmpAppraisalHeader.EIH_EMP_DESGN == null ? CommonConstants.SELECTVAL : Convert.ToString(objEmpAppraisalHeader.EIH_EMP_DESGN);
                            lblDesignationOldText.Text = ERP.Utilities.CommonFunctions.GetShortString(objEmpAppraisalHeader.EIH_PREV_DESGN_TEXT, 25);
                            lblDesignationOldText.ToolTip = objEmpAppraisalHeader.EIH_PREV_DESGN_TEXT.HtmlDecode();
                            hdfDesignationOld.Value = objEmpAppraisalHeader.EIH_PREV_DESGN == null ? CommonConstants.SELECTVAL : Convert.ToString(objEmpAppraisalHeader.EIH_PREV_DESGN);
                            lblDepartmentOldText.Text = ERP.Utilities.CommonFunctions.GetShortString(objEmpAppraisalHeader.EIH_PREV_DEPT_TEXT, 25);
                            lblDepartmentOldText.ToolTip = objEmpAppraisalHeader.EIH_PREV_DEPT_TEXT.HtmlDecode();
                            hdfDepartmentOld.Value = objEmpAppraisalHeader.EIH_PREV_DEPT == null ? CommonConstants.SELECTVAL : Convert.ToString(objEmpAppraisalHeader.EIH_PREV_DEPT);
                            Status = objEmpAppraisalHeader.EIH_STATUS;
                            GetFieldValues(ControlsEnum.EMPLOYEESALARY);
                            UCEmpSalary.EmpSalaryHdr.SalaryDtl = null;
                            UCEmpSalary.EmpSalaryHdr.SalaryDtl = objEmpAppraisalHeader.EmployeeAppraisalDtl;
                            SetFieldValues(ControlsEnum.EMPLOYEESALARY);
                            LastModifiedTime = objEmpAppraisalHeader.LAST_MOD_DT;
                            UCEmpSalary.MonthlyStartDay = objEmpAppraisalHeader.PTM_PAYRL_START;
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
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Appraisal Type
                case ControlsEnum.APPRAISALTYPES:
                    ddlType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlSrchType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                        ddlSrchType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlSrchType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlSrchType.DataBind();

                        ddlType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD);
                        ddlType.DataTextField = GTIService.Constants.Common.Common.CFG_DATA_TEXT_FIELD;
                        ddlType.DataValueField = GTIService.Constants.Common.Common.CFG_VALUE_VALUE_FIELD;
                        ddlType.DataBind();
                    }
                    ddlType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlSrchType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
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
                #region EMPLOYEE SALARY HDR
                case ControlsEnum.EMPLOYEESALARYHDR:
                    EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader tempEmployeeAppraisalMaster = new EmployeeAppraisalDetailsBO.EmployeeAppraisalHeader();
                    tempEmployeeAppraisalMaster.EIH_PK = CurrPK;
                    tempEmployeeAppraisalMaster.EIH_DATE = Convert.ToDateTime(txtDate.Text);
                    tempEmployeeAppraisalMaster.EIH_EMPLOYEE = Convert.ToInt32(hdfEmployee.Value);
                    tempEmployeeAppraisalMaster.EIH_NO = lblTrxNo.Text;
                    tempEmployeeAppraisalMaster.EIH_TRN_NAME = txtRemarks.Text.HtmlEncode();
                    tempEmployeeAppraisalMaster.EIH_TYPE = Convert.ToInt32(ddlType.SelectedValue);
                    tempEmployeeAppraisalMaster.EIH_EFFECT_DATE = Convert.ToDateTime(txtEffectFrom.Text);
                    GetFieldValues(ControlsEnum.COMPANY);
                    tempEmployeeAppraisalMaster.EIH_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    tempEmployeeAppraisalMaster.USER_PK = currentUser.PKUser;
                    tempEmployeeAppraisalMaster.EIH_DEPT = currentUser.CurrentDeptPK;
                    tempEmployeeAppraisalMaster.BIZUNIT = currentUser.SBUID;
                    tempEmployeeAppraisalMaster.LAST_MOD_DT = LastModifiedTime;
                    tempEmployeeAppraisalMaster.EIH_EMP_DESGN = Convert.ToInt32(hdfDesignation.Value) > 0 ? hdfDesignation.Value : null;
                    tempEmployeeAppraisalMaster.EIH_EMP_DEPT = Convert.ToInt32(hdfDepartment.Value) > 0 ? hdfDepartment.Value : null;
                    tempEmployeeAppraisalMaster.EIH_DESC = txtComments.Text.HtmlEncode();
                    tempEmployeeAppraisalMaster.EIH_REF_NO = txtRefNo.Text.HtmlEncode();
                    tempEmployeeAppraisalMaster.EIH_REF_DATE = txtRefDate.Text == string.Empty ? null : txtRefDate.Text;
                    tempEmployeeAppraisalMaster.EIH_PREV_DESGN = Convert.ToInt32(hdfDesignationOld.Value) > 0 ? hdfDesignationOld.Value : null;
                    tempEmployeeAppraisalMaster.EIH_PREV_DEPT = Convert.ToInt32(hdfDepartmentOld.Value) > 0 ? hdfDepartmentOld.Value : null;
                    UCEmpSalary.SetUIValuesToObject(BusinessObject.HRMS.Employee.ActionsEnum.EMPLOYEESALARY);
                    tempEmployeeAppraisalMaster.EmployeeAppraisalDtl = UCEmpSalary.EmpSalaryHdr.SalaryDtl;
                    SetUIValuesToObject(ControlsEnum.EMPLOYEESALARYDETAILS);
                    //tempEmployeeAppraisalMaster.EmployeeAppraisalDtl = EMPAppraisaDetailsList;
                    returnObject = tempEmployeeAppraisalMaster;
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
                    CurrEmployeePK = 0;
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    txtDate.Text = string.Empty;
                    txtEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfEmployee.Value = CommonConstants.SELECTVAL;
                    ddlType.ClearSelection();
                    txtEffectFrom.Text = string.Empty;
                    lblPrevAppDateText.Text = string.Empty;
                    txtRefNo.Text = string.Empty;
                    txtRefDate.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtComments.Text = string.Empty;
                    txtDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    lblDepartmentOldText.Text = string.Empty;
                    hdfDepartment.Value = CommonConstants.SELECTVAL;
                    hdfDepartmentOld.Value = CommonConstants.SELECTVAL;
                    txtDesignation.Text = Resources.ErpRes.AutoDefaultValue;
                    lblDesignationOldText.Text = string.Empty;
                    hdfDesignationOld.Value = Resources.ErpRes.AutoDefaultValue;
                    hdfDesignation.Value = CommonConstants.SELECTVAL;
                    txtOldCTC.Text = string.Empty;
                    txtOldGrossSalary.Text = string.Empty;
                    txtOldNetSalary.Text = string.Empty;
                    txtEmpNetSalary.Text = string.Empty;
                    txtTotalGross.Text = string.Empty;
                    txtTotalCTC.Text = string.Empty;
                    txtDiffCTC.Text = string.Empty;
                    txtDiffGrossSalary.Text = string.Empty;
                    txtDiffNetSalary.Text = string.Empty;
                    txtSrchEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfSrchEmployee.Value = CommonConstants.SELECTVAL;
                    txtSrchFromDate.Text = string.Empty;
                    txtSrchToDate.Text = string.Empty;
                    ddlSrchType.ClearSelection();
                    UCEmpSalary.EmpSalaryHdr = new BusinessObject.HRMS.Employee.EmpTemplateHeader();
                    EMPAppraisaDetailsList = null;
                    base.WkfRefID = 0;
                    Status = 0;
                    hdfIsCancelled.Value = "0";
                    //txtTrxNo.Text = string.Empty;
                    //hdfTrxPk.Value = string.Empty;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = "1";
                    txtSrchEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfSrchEmployee.Value = CommonConstants.SELECTVAL;
                    txtSrchFromDate.Text = string.Empty;
                    txtSrchToDate.Text = string.Empty;
                    ddlSrchType.ClearSelection();
                    break;
                #endregion
                #region CLEAR ELEMENTS
                case ControlsEnum.CLEARELEMENTS:
                    EMPAppraisaDetailsList = null;
                    lblPrevAppDateText.Text = string.Empty;
                    break;
                #endregion
                #region CLEAR EMPLOYEE SEARCH
                case ControlsEnum.CLEAREMPLOYEESEARCH:
                    EMPAppraisaDetailsList = null;
                    UCEmpSalary.EmpSalaryHdr.SalaryDtl = null;
                    lblPrevAppDateText.Text = string.Empty;
                    CurrEmployeePK = 0;
                    txtEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfEmployee.Value = CommonConstants.SELECTVAL;
                    txtDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    lblDepartmentOldText.Text = string.Empty;
                    hdfDepartment.Value = CommonConstants.SELECTVAL;
                    hdfDepartmentOld.Value = CommonConstants.SELECTVAL;
                    txtDesignation.Text = Resources.ErpRes.AutoDefaultValue;
                    lblDesignationOldText.Text = string.Empty;
                    hdfDesignationOld.Value = Resources.ErpRes.AutoDefaultValue;
                    hdfDesignation.Value = CommonConstants.SELECTVAL;
                    txtOldCTC.Text = string.Empty;
                    txtOldGrossSalary.Text = string.Empty;
                    txtOldNetSalary.Text = string.Empty;
                    txtEmpNetSalary.Text = string.Empty;
                    txtTotalGross.Text = string.Empty;
                    txtTotalCTC.Text = string.Empty;
                    txtDiffCTC.Text = string.Empty;
                    txtDiffGrossSalary.Text = string.Empty;
                    txtDiffNetSalary.Text = string.Empty;
                    break;
                #endregion
            }
        }
        #endregion

        #region UtitlityMethods

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
                //else if (Mode == ActionsEnum.EDIT)
                //{
                //    EntryStatus = EntryStatus.EDITMODE;
                //}
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
        public string GetSubstring(object str)
        {
            return ((string)str).Substring(0, 3);
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

        #region ControlEnum
        public enum ControlsEnum
        {
            COMPANY,
            CLEAR,
            LIST,
            CLEARSEARCH,
            CLEARELEMENTS,
            EMPLOYEESALARY,
            APPRAISALTYPES,
            EMPLOYEESALARYHDR,
            EMPLOYEESALARYDETAILS,
            EMPLOYEESALARYDETAILSBYPK,
            EMPLOYEESALARYHDRBYEMPPK,
            CLEAREMPLOYEESEARCH
        }
        #endregion
    }
}