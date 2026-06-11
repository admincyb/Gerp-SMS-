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
using BusinessObject.HRMS.ESS;
using System.Xml;
using BusinessObject.CommonManagement;
using BusinessObject.AccountManagement;
using ERP.Utilities.HRMS;
using BusinessLogic.HRMS.ESS;

namespace HRMS.ESS
{
    public partial class EmpLeaveRequest : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties

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

        private string PageIndexList
        {
            get
            {
                return (string)this.ViewState["PageIndexList"];
            }
            set
            {
                this.ViewState["PageIndexList"] = value;
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

        /// <summary>
        /// To keep CreditLeave status in view state
        /// </summary>
        private int CreditLeave
        {
            get
            {
                return this.ViewState[ViewstateStrings.CreditLeave] == null ? 0 : (int)this.ViewState[ViewstateStrings.CreditLeave];
            }
            set
            {
                this.ViewState[ViewstateStrings.CreditLeave] = value;
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

        #region Variables
        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private int EmployeePk;
        private int LeaveTypePK = 0;
        private double LeaveBalance;
        private double NoOfLeaves;
        DateTime? leaveDate;
        private int CompanyPk = 0;
        private string refID;
        private string inboxFlag;
        private string prefID;
        private EmpLeaveRequestBO.ESSLeaveRequestMaster ObjESSLeaveEntryMaster;
        //int currPK;
        #endregion
        #endregion

        #region PageEvents
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

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();
        }

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            else if (EntryStatus == EntryStatus.VIEWMODE)
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
                    PageIndexList = uclPaging.CurrentPage.ToString();
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
                //currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;
                // InitializeComponent();
                if (!IsPostBack)
                {
                    //hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    // string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    //hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    //hdfCurrencyFormat.Value = "#0.";
                    // for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    //{
                    //    hdfCurrencyFormat.Value += "0";
                    //    hdfCurrencyFormatWithComma.Value += "0";
                    //}
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


                    hdfLastYearDate.Value = DateTime.Now.AddYears(-1).ToString(Resources.Constants.HRMSDateFormatShort);
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    //GetFieldValues(ControlsEnum.LIST);
                    //SetFieldValues(ControlsEnum.LIST);
                    txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
                    ConfigurationSettings();

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
                        GetFieldValues(ControlsEnum.LEAVEDETAILSBYPK);
                        SetFieldValues(ControlsEnum.LEAVEDETAILSBYPK);
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
            hdfNoOfLeaveValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveValidation").ToString();
            hdfValidate_Leave.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsLeavePeriodValidation").ToString();
            hdfHolidayLeaveVal.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveHolidayVal").ToString();
            hdfOffDaySkip.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsOffDayLeaveSkip").ToString();
        }
        #endregion

        #region Action Handler
        /// <summary>
        /// For Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ActionHandler(object sender, EventArgs e)
        {
            int result;
            XmlDocument xmlDoc;
            string RetNo = string.Empty;
            string empResigDate = string.Empty;
            TextBox WrkfComments;
            bool bIsChecked = false;
            try
            {
                #region Getting Command Action
                ActionsEnum commonActions = ActionsEnum.UNKNOWN;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if ((((DropDownList)sender).ID == "ddlLeaveType"))
                    {
                        commonActions = ActionsEnum.CHANGETYPE;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                #endregion
                switch (commonActions)
                {
                    #region LIST
                    case ActionsEnum.LIST:
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        ObjESSLeaveEntryMaster = new EmpLeaveRequestBO.ESSLeaveRequestMaster();
                        ObjESSLeaveEntryMaster = (EmpLeaveRequestBO.ESSLeaveRequestMaster)SetUIValuesToObject(ControlsEnum.LEAVEDETAILSHDR);
                        ObjESSLeaveEntryMaster.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(ObjESSLeaveEntryMaster);
                        result = EmpLeaveRequestBL.SaveEmployeeLeaveESS(xmlDoc.InnerXml, out RetNo, out  empResigDate);
                        if (result > 0)
                        {
                            hdfNoOfLeaveValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveValidation").ToString();
                            ResetForm(ControlsEnum.CLEAR);
                            ResetForm(ControlsEnum.CLEARSEARCH);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveRequest);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_DateOutSalaryYear").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYCREATED)  // holiday
                            {
                                litErrorMsg.Text = GetLocalResourceObject("MsgHolidayExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.PENDINGEXIST)  // multiple leave in same date
                            {
                                litErrorMsg.Text = GetLocalResourceObject("MsgMultipleLeaveExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKPRINTER)  // off day
                            {
                                litErrorMsg.Text = GetLocalResourceObject("MsgOffDayExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKMCPRINTER)  // payroll
                            {
                                litErrorMsg.Text = GetLocalResourceObject("MsgPayrollExist").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKZBPRINTER)  // Leave count Exceeds than Balance
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveExceeds(1);});", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)  // Leave count Exceeds than Balance
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_NoOfLeaves").ToString(), RetNo);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKPACKINGTYPE)  // Leave in Resignation date
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_EmpResignedDate").ToString(), Convert.ToDateTime(empResigDate).ToString(Resources.Constants.HRMSDateFormatShort));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EDIT, DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.VIEW:
                        string monthYear = string.Empty;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                HiddenField hdfDept;
                                int dept;
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpLeavePk")).Value);
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
                            GetFieldValues(ControlsEnum.LEAVEDETAILSBYPK);
                            SetFieldValues(ControlsEnum.LEAVEDETAILSBYPK);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.CLEAR);
                        SetUIEditView(commonActions);
                        LeaveTypePK = 0;
                        hdfEmployee.Value = currentUser.PKEmployee.ToString();
                        txtEmployee.Text = currentUser.EmpCode + " - " + currentUser.EmpName;
                        GetFieldValues(ControlsEnum.LEAVETYPES);
                        SetFieldValues(ControlsEnum.LEAVETYPES);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        //SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = EmpLeaveRequestBL.DeleteESSLeave(CurrPK, LastModifiedTime);  //, Convert.ToInt16(hdfIsDeleteYes.Value)
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveRequest);
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
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.UsedInAnotherPlace;
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
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.INCORRECT) // salary processed
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_SalaryProcessed").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.AMOUNTEXCEEDED)  // Leave Exsit After Delete Leave Date
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveDelete(3);});", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveRequest);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        //this.currPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CHANGE LEAVE TYPE
                    case ActionsEnum.CHANGETYPE:
                        if (txtFromDate.Text == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_FromDate").ToString()) + "');", true);
                            ResetForm(ControlsEnum.CLEARLEAVE);

                        }
                        else if (txtToDate.Text == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ToDate").ToString()) + "');", true);
                            ResetForm(ControlsEnum.CLEARLEAVE);
                        }
                        else
                        {
                            CreditLeave = 0;
                            GetFieldValues(ControlsEnum.LEAVETDETAILS);
                            if (dtResult != null && dtResult.Rows.Count > 0)
                            {
                                if (Convert.ToInt32(dtResult.Rows[0]["LTM_CREDIT"]) == 1)
                                    CreditLeave = 1;
                            }
                            GetFieldValues(ControlsEnum.BALANCELEAVE);
                            SetFieldValues(ControlsEnum.BALANCELEAVE);
                            GetFieldValues(ControlsEnum.NOOFLEAVES);
                            SetFieldValues(ControlsEnum.NOOFLEAVES);
                            txtReason.Focus();
                        }
                        break;
                    #endregion
                    #region CHANGE EMPLOYEE
                    case ActionsEnum.CHANGEEMPLOYEE:
                        GetFieldValues(ControlsEnum.LEAVETYPES);
                        SetFieldValues(ControlsEnum.LEAVETYPES);
                        txtBalance.Text = "0";
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
                            ObjESSLeaveEntryMaster = new EmpLeaveRequestBO.ESSLeaveRequestMaster();
                            ObjESSLeaveEntryMaster = (EmpLeaveRequestBO.ESSLeaveRequestMaster)SetUIValuesToObject(ControlsEnum.LEAVEDETAILSHDR);
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (ObjESSLeaveEntryMaster != null)
                                {
                                    string TrxNo = string.Empty;
                                    ObjESSLeaveEntryMaster.WKF_FLAG = 1;
                                    SaveTransaction(ObjESSLeaveEntryMaster, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));

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
                                SaveTransaction(ObjESSLeaveEntryMaster, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }

                        }
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            HiddenField hdfDept;
                            int dept;
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEIH_PK")).Value);
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
                            GetFieldValues(ControlsEnum.LEAVEDETAILSBYPK);
                            SetFieldValues(ControlsEnum.LEAVEDETAILSBYPK);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #region SaveTransaction
        /// <summary>
        /// Save With workflow submition
        /// </summary>
        /// <param name="objESSLeaveEntry"></param>
        private void SaveTransaction(EmpLeaveRequestBO.ESSLeaveRequestMaster objESSLeaveEntry, int workflowFlag)
        {
            int? result = 0;
            XmlDocument xmlDoc;
            string RetNo = string.Empty;
            string empResigDate = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objESSLeaveEntry == null)
                objESSLeaveEntry = new EmpLeaveRequestBO.ESSLeaveRequestMaster();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objESSLeaveEntry.USER_PK = wkfDetails.UserPK;
            objESSLeaveEntry.WKF_APPLICATION = CurrPK;
            objESSLeaveEntry.WKF_COMMENTS = wkfDetails.Comments;
            objESSLeaveEntry.WKF_TRX_FLAG = workflowFlag;
            objESSLeaveEntry.WKF_PROCESS = wkfDetails.ProcessID;
            objESSLeaveEntry.WKF_REFERENCE = wkfDetails.ReferenceID;
            objESSLeaveEntry.WKF_TASK = wkfDetails.TaskID;
            objESSLeaveEntry.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion
            xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(objESSLeaveEntry);
            result = EmpLeaveRequestBL.SaveEmployeeLeaveESS(xmlDoc.InnerXml, out RetNo, out  empResigDate);
            if (result > 0)
            {
                //19 if (!string.IsNullOrEmpty(TrxNo))
                //19 lblTrxNo.Text = TrxNo;
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
                args[0] = Resources.PageNameRes.EmployeeLeaveRequest;
                //19 args[1] = lblTrxNo.Text.Trim();
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveRequest + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    litErrorMsg.Text = GetLocalResourceObject("Err_DateOutSalaryYear").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.ALREADYCREATED)  // holiday
                {
                    litErrorMsg.Text = GetLocalResourceObject("MsgHolidayExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.PENDINGEXIST)  // multiple leave in same date
                {
                    litErrorMsg.Text = GetLocalResourceObject("MsgMultipleLeaveExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKPRINTER)  // off day
                {
                    litErrorMsg.Text = GetLocalResourceObject("MsgOffDayExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKMCPRINTER)  // payroll
                {
                    litErrorMsg.Text = GetLocalResourceObject("MsgPayrollExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)  // Leave count Exceeds than Balance
                {
                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_NoOfLeaves").ToString(), RetNo);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKPACKINGTYPE)  // Leave in Resignation date
                {
                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_EmpResignedDate").ToString(), Convert.ToDateTime(empResigDate).ToString(Resources.Constants.HRMSDateFormatShort));
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.CHECKZBPRINTER)  // Leave count Exceeds than Balance
                {
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePopup", "ClosePopup();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveExceeds(2);});", true);

                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveMaster);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.Captions.Information + "');", true);
                }
            }
        }
        #endregion
        #endregion

        #region --- For Grid Actions----
        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
        }


        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            #region grdEmpLeave_PopUp
            #endregion
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
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtFilterFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtFilterToDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterToDate.Text);
                        objFilterParam.Date = string.IsNullOrEmpty(txtSrchDate.Text) ? (DateTime?)null : DateTime.Parse(txtSrchDate.Text);
                        objFilterParam.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        objFilterParam.UserPK = currentUser.PKUser;
                        objFilterParam.Employee = Convert.ToInt32(hdfEmployeeSearch.Value);
                        objFilterParam.LvOffDaySkip = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsOffDayLeaveSkip"));
                        objFilterParam.LvHolidaySkip = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveHolidayVal"));
                        dsPageData = EmpLeaveRequestBL.GetESSLeaveEntryList(objFilterParam);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region LEAVETYPES
                    case ControlsEnum.LEAVETYPES:
                        int empPk = 0;
                        int.TryParse(hdfEmployee.Value, out empPk);
                        dtResult = BusinessLogic.HRMS.Admin.Masters.LeaveTypeMasterBL.GetLeaveTypeDDL(LeaveTypePK, string.Empty, string.Empty, 1, currentUser.SBUID, 0, 0, -1, GTIService.Constants.Configurations.Employees.Fields.LTM_CODE, null, empPk);
                        break;
                    #endregion
                    #region LEAVE DETAILS HDR
                    case ControlsEnum.LEAVEDETAILSBYPK:
                        objFilterParam = new FilterParameters();
                        objFilterParam.PK = CurrPK;
                        objFilterParam.Active = (int)DbActiveStatus.HASPK;
                        ObjESSLeaveEntryMaster = EmpLeaveRequestBL.GetLeaveDetailsByPk(objFilterParam);
                        break;
                    #endregion
                    #region LEAVETDETAILS
                    case ControlsEnum.LEAVETDETAILS:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.LeaveTypeMasterBL.GetLeaveType(Convert.ToInt32(ddlLeaveType.SelectedValue), string.Empty, string.Empty, (int)DbActiveStatus.HASPK, currentUser.SBUID, 0, 0, -1, GTIService.Constants.Configurations.Employees.Fields.LTM_CODE);
                        break;
                    #endregion
                    #region BALANCE
                    case ControlsEnum.BALANCELEAVE:
                        LeaveBalance = 0;
                        leaveDate = Convert.ToDateTime(txtToDate.Text);
                        EmployeePk = 0;
                        int.TryParse(hdfEmployee.Value, out EmployeePk);
                        LeaveBalance = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.GetBalanceLeave(EmployeePk, Convert.ToInt32(ddlLeaveType.SelectedValue), leaveDate);
                        break;
                    #endregion
                    #region NO OF LEAVES
                    case ControlsEnum.NOOFLEAVES:
                        NoOfLeaves = 0;
                        leaveDate = Convert.ToDateTime(txtToDate.Text);
                        EmployeePk = 0;
                        int.TryParse(hdfEmployee.Value, out EmployeePk);
                        objFilterParam = new FilterParameters();
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtFromDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text);
                        objFilterParam.LvFromHalf = chkFromHalfDay.Checked ? Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt16(CommonConstants.SELECT_VALUE_ZERO);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtToDate.Text) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text);
                        objFilterParam.LvToHalf = chkToHalfDay.Checked ? Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE) : Convert.ToInt16(CommonConstants.SELECT_VALUE_ZERO);
                        objFilterParam.Employee = EmployeePk;
                        objFilterParam.LvHolidaySkip = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveHolidayVal"));
                        objFilterParam.LvOffDaySkip = Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsOffDayLeaveSkip"));
                        NoOfLeaves = EmpLeaveRequestBL.GetNoOfLeaves(objFilterParam);
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
                    #region LEAVETYPES
                    case ControlsEnum.LEAVETYPES:
                        BindDropDown(ControlsEnum.LEAVETYPES);
                        break;
                    #endregion
                    #region SALARYPKLIST
                    case ControlsEnum.SALARYPKLIST:
                        BindDropDown(ControlsEnum.SALARYPKLIST);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region BALANCELEAVE
                    case ControlsEnum.BALANCELEAVE:
                        GetUIValuesFromObject(ControlsEnum.BALANCELEAVE);
                        break;
                    #endregion
                    #region LEAVEDETAILSHDR
                    case ControlsEnum.LEAVEDETAILSBYPK:
                        GetUIValuesFromObject(ControlsEnum.LEAVEDETAILSBYPK);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region NOOFLEAVES
                    case ControlsEnum.NOOFLEAVES:
                        GetUIValuesFromObject(ControlsEnum.NOOFLEAVES);
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
                    #region BALANCELEAVE
                    case ControlsEnum.BALANCELEAVE:
                        if (CreditLeave == Convert.ToInt32(LeaveType.NONCREDIT))
                            txtBalance.Text = "0";
                        else
                            txtBalance.Text = LeaveBalance.ToString();
                        break;
                    #endregion
                    #region LEAVEDETAILSHDR
                    case ControlsEnum.LEAVEDETAILSBYPK:
                        if (ObjESSLeaveEntryMaster != null)
                        {
                            txtDate.Text = ObjESSLeaveEntryMaster.ESL_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                            CompanyPk = ObjESSLeaveEntryMaster.ESL_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));
                            hdfEmployee.Value = ObjESSLeaveEntryMaster.ESL_EMPLOYEE.ToString();
                            txtEmployee.Text = ObjESSLeaveEntryMaster.ESL_EMPLOYEE_TEXT.HtmlDecode();
                            txtFromDate.Text = ObjESSLeaveEntryMaster.ESL_LV_FROM_DT.ToString(Resources.Constants.HRMSDateFormatShort);
                            chkFromHalfDay.Checked = ObjESSLeaveEntryMaster.ESL_LV_FROM_HALF == 1 ? true : false;
                            txtToDate.Text = ObjESSLeaveEntryMaster.ESL_LV_TO_DT.ToString(Resources.Constants.HRMSDateFormatShort);
                            chkToHalfDay.Checked = ObjESSLeaveEntryMaster.ESL_LV_TO_HALF == 1 ? true : false;
                            LeaveTypePK = ObjESSLeaveEntryMaster.ESL_LEAVE_TYPE;
                            GetFieldValues(ControlsEnum.LEAVETYPES);
                            SetFieldValues(ControlsEnum.LEAVETYPES);
                            ddlLeaveType.SelectedIndex = ddlLeaveType.Items.IndexOf(ddlLeaveType.Items.FindByValue(ObjESSLeaveEntryMaster.ESL_LEAVE_TYPE.ToString()));
                            ActionHandler(ddlLeaveType, EventArgs.Empty);
                            txtReason.Text = ObjESSLeaveEntryMaster.ESL_REASON.HtmlDecode();
                            txtBalance.Text = GetFormattedNumber(Convert.ToDecimal(txtBalance.Text)); //+ ObjESSLeaveEntryMaster.ELD_CREDIT_LEAVE_COUNT);
                            Status = ObjESSLeaveEntryMaster.ESL_STATUS;
                            LastModifiedTime = ObjESSLeaveEntryMaster.LAST_MOD_DT;
                        }
                        break;
                    #endregion
                    #region NOOFLEAVES
                    case ControlsEnum.NOOFLEAVES:
                        txtNoOfLeave.Text = NoOfLeaves.ToString();
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
                #region LEAVETYPES
                case ControlsEnum.LEAVETYPES:
                    ddlLeaveType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.LTM_NAME;
                    ddlLeaveType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.LTM_PK;
                    ddlLeaveType.DataSource = dtResult;
                    ddlLeaveType.DataBind();
                    ddlLeaveType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    ddlLeaveType.Items.HtmlDecode();
                    break;
                #endregion
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompany.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompany.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompany.DataSource = dtCompany;
                    ddlCompany.DataBind();
                    ddlCompany.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompany.Items.HtmlDecode();
                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));

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
                        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                        grdList.DataSource = dsPageData.Tables[0];
                        grdList.DataBind();
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
                #region LEAVEMASTER
                case ControlsEnum.LEAVEDETAILSHDR:
                    EmpLeaveRequestBO.ESSLeaveRequestMaster tempEmpLeaveMaster = new EmpLeaveRequestBO.ESSLeaveRequestMaster();
                    tempEmpLeaveMaster.ESL_PK = CurrPK;
                    tempEmpLeaveMaster.ESL_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                    tempEmpLeaveMaster.ESL_DATE = DateTime.Parse(txtDate.Text);
                    tempEmpLeaveMaster.ESL_EMPLOYEE = Convert.ToInt32(hdfEmployee.Value);
                    tempEmpLeaveMaster.ESL_REASON = HttpUtility.HtmlEncode(txtReason.Text);
                    tempEmpLeaveMaster.ESL_LV_FROM_DT = DateTime.Parse(txtFromDate.Text);
                    tempEmpLeaveMaster.ESL_LV_TO_DT = DateTime.Parse(txtToDate.Text);
                    tempEmpLeaveMaster.ESL_LV_FROM_HALF = chkFromHalfDay.Checked == true ? 1 : 0;
                    tempEmpLeaveMaster.ESL_LV_TO_HALF = chkToHalfDay.Checked == true ? 1 : 0;
                    tempEmpLeaveMaster.ESL_LEAVE_TYPE = Convert.ToInt32(ddlLeaveType.SelectedValue);
                    tempEmpLeaveMaster.ESL_BIZUNIT = currentUser.SBUID;
                    tempEmpLeaveMaster.ESL_DEPT = currentUser.CurrentDeptPK;
                    tempEmpLeaveMaster.LAST_MOD_DT = LastModifiedTime;
                    tempEmpLeaveMaster.USER_PK = currentUser.PKUser;
                    tempEmpLeaveMaster.VALIDATE_LEAVE = Convert.ToInt32(hdfValidate_Leave.Value);
                    tempEmpLeaveMaster.LEAVE_SKIP = Convert.ToInt32(hdfNoOfLeaveValidation.Value);
                    tempEmpLeaveMaster.HOL_SKIP = Convert.ToInt32(hdfHolidayLeaveVal.Value);
                    tempEmpLeaveMaster.OFFDAY_SKIP = Convert.ToInt32(hdfOffDaySkip.Value);
                    returnObject = tempEmpLeaveMaster;
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
                    CurrPK = 0;
                    CreditLeave = 0;
                    txtDate.Text = string.Empty;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    ddlLeaveType.ClearSelection();
                    txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
                    txtReason.Text = string.Empty;
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    chkFromHalfDay.Checked = false;
                    chkToHalfDay.Checked = false;
                    base.WkfRefID = 0;
                    ddlStatus.ClearSelection();
                    Status = 0;
                    hdfIsCancelled.Value = "0";
                    txtNoOfLeave.Text = string.Empty;
                    //txtTrxNo.Text = string.Empty;
                    //hdfTrxPk.Value = string.Empty;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    txtFilterFromDate.Text = string.Empty;
                    txtFilterToDate.Text = string.Empty;
                    txtSrchDate.Text = string.Empty;
                    txtEmployeeSearch.Text = string.Empty;
                    hdfEmployeeSearch.Value = CommonConstants.SELECT_VALUE_ZERO;
                    ddlStatus.ClearSelection();
                    break;
                #endregion
                #region CLEARLEAVE
                case ControlsEnum.CLEARLEAVE:
                    ddlLeaveType.SelectedIndex = -1;
                    txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
                    txtNoOfLeave.Text = CommonConstants.SELECT_VALUE_ZERO;
                    break;
                #endregion
            }
        }
        #endregion

        #region UtitlityMethods
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
            string format = "#0.00";
            string s = num.ToString(format);
            return s;
        }

        public string GetSubstring(object str)
        {
            return ((string)str).Substring(0, 3);
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
            EMPLOYEES,
            LEAVETYPES,
            COMPANY,
            LEAVEDETAILSHDR,
            CLEAR,
            SALARYPKLIST,
            LIST,
            CLEARSEARCH,
            BALANCELEAVE,
            LEAVETDETAILS,
            LEAVEDETAILSBYPK,
            LEAVEDELETE,
            CLEARLEAVE,
            NOOFLEAVES
        }
        #endregion

        #region LeaveType Enum
        public enum LeaveType
        {
            NONCREDIT = 0,
            CREDIT = 1
        }
        #endregion
    }
}