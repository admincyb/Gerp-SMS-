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
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class MonthlyLeave : ERP.Store.UI.MyBasePage
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
        // 18/4   //private ActionEnum ActionStatus
        //{
        //    get
        //    {
        //        return this.ViewState["ActionStatus"] == null ? ActionEnum.NEW_ACTION : (ActionEnum)(this.ViewState["ActionStatus"]);
        //    }
        //    set
        //    {
        //        this.ViewState["ActionStatus"] = value;
        //    }
        //}

        /// <summary>
        /// To keep Leave Details List in view state
        /// </summary>
        private List<MonthlyLeaveBO.LeaveDetails> LeaveDetailsList
        {
            get
            {
                return ViewState[ViewstateStrings.LeaveDetailsList] == null ? new List<MonthlyLeaveBO.LeaveDetails>() : (List<MonthlyLeaveBO.LeaveDetails>)ViewState[ViewstateStrings.LeaveDetailsList];
            }
            set
            {
                ViewState[ViewstateStrings.LeaveDetailsList] = value;
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
        /// To keep Leave Detail PK
        /// </summary>
        private int LeaveDtPk
        {
            get
            {
                return this.ViewState[ViewstateStrings.LeaveDtPk] == null ? 0 : (int)this.ViewState[ViewstateStrings.LeaveDtPk];
            }
            set
            {
                this.ViewState[ViewstateStrings.LeaveDtPk] = value;
            }
        }
        #endregion

        private BusinessObject.User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private int EmployeePk;
        private int LeaveTypePK = 0;
        private double LeaveBalance;
        DateTime? leaveDate;
        private int CompanyPk = 0;
        private int LvEldPk = 0;
        private MonthlyLeaveBO.LeaveEntryMaster objLeaveMaster;
        //int currPK;
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

        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            hdfCurrentPk.Value = CurrPK.ToString();
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
            if (EntryStatus == EntryStatus.EDITMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
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
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
            }
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
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
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                InitializeComponent();
                if (!IsPostBack)
                {
                    DateTime CDate = txtDate.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(txtDate.Text);
                    hdfNextYearDate.Value = CDate.AddMonths(-Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "hrmsLeaveEntryPrevMonthDiff").ToString())).ToString(Resources.Constants.HRMSDateFormatShort);
                    uclPaging.CurrentPage = 1;
                    SetFieldValues(ControlsEnum.LEAVETYPES);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
                    ConfigurationSettings();
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
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int result;
            XmlDocument xmlDoc;
            string RetNo = string.Empty;
            string empResigDate = string.Empty;
            // tempList = new List<MonthlyLeaveBO.LeaveDetails>();
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
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        //ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        if (RowIndex < 0)       // New Mode
                        {
                            if (LeaveDetailsList == null)
                                LeaveDetailsList = new List<MonthlyLeaveBO.LeaveDetails>();
                            hdfIscontYes.Value = CommonConstants.SELECT_VALUE_ZERO;
                        }
                        MonthlyLeaveBO.LeaveEntryMaster monthlyLeave = (MonthlyLeaveBO.LeaveEntryMaster)SetUIValuesToObject(ControlsEnum.LEAVEMASTERADD);
                        txtEmployee.Enabled = true;
                        if (monthlyLeave.LeaveDetails.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(monthlyLeave);
                        result = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.SaveEmployeeLeave(xmlDoc.InnerXml, out RetNo, out  empResigDate);
                        if (result > 0)
                        {
                            CurrPK = result;
                            ResetForm(ControlsEnum.ADDTOLIST);
                            hdfNoOfLeaveValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveValidation").ToString();
                            hdfHolidayLeaveVal.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveHolidayVal").ToString();
                            hdfOffDaySkip.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsOffDayLeaveSkip").ToString();
                            GetFieldValues(ControlsEnum.LEAVEDETAILSHDR);
                            SetFieldValues(ControlsEnum.LEAVEDETAILSHDR);
                            SetFieldValues(ControlsEnum.LEAVEDETAILSLIST);
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
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
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
                            else if (result == (int)DbSaveStatus.PENDINGEXIST)  // multiple leave
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
                            else if (result == (int)DbSaveStatus.CHECKMCPRINTER)  // payroll already processed in curent period
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("MsgPayrollExist").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }

                            else if (result == (int)DbSaveStatus.CHECKZBPRINTER)  // Leave count Exceeds than Balance
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveExceeds();});", true);
                                // litErrorMsg.Text = string.Format(GetLocalResourceObject("MsgPayrollExist").ToString(), RetNo);
                                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                // + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKPOUCHPRINTER)  // Leave count Exceeds than Balance
                            {
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveExceeds();});", true);
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_NoOfLeaves").ToString(), RetNo);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKPACKINGTYPE)  // Leave in Resignation date
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_EmpResignedDate").ToString(), empResigDate);
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
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (grdLeaveList.Rows.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        MonthlyLeaveBO.LeaveEntryMaster monthlyLeaveM = (MonthlyLeaveBO.LeaveEntryMaster)SetUIValuesToObject(ControlsEnum.LEAVEMASTER);
                        if (LeaveDetailsList.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }

                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(monthlyLeaveM);
                        result = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.SaveEmployeeLeave(xmlDoc.InnerXml, out RetNo, out  empResigDate);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            ResetForm(ControlsEnum.CLEARSEARCH);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MonthlyLeave);
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
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_DateOutSalaryYear").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            //else if (result == (int)DbSaveStatus.INCORRECT)// 
                            //{
                            //    string strError = string.Empty;
                            //    if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                            //    {
                            //        foreach (DataRow dr in dtErrorList.Rows)
                            //        {
                            //            strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["empText"])) + "-" + HttpUtility.HtmlDecode(Convert.ToString(dr["LEAVE_TYPE_TEXT"]));
                            //        }
                            //    }
                            //    litErrorMsg.Text = GetLocalResourceObject("Err_IncorrectSalaryPeriod").ToString() + strError;
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            //        + "','" + Resources.ErpRes.Information + "');", true);
                            //}
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
                        string monthYear = string.Empty;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmpLeavePk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.LEAVEDETAILSHDR);
                            SetFieldValues(ControlsEnum.LEAVEDETAILSHDR);
                            SetFieldValues(ControlsEnum.LEAVEDETAILSLIST);
                            //GetFieldValues(ControlsEnum.LEAVETYPES);
                            //SetFieldValues(ControlsEnum.LEAVETYPES);
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
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        SetFieldValues(ControlsEnum.LEAVEDETAILSLIST);
                        LeaveTypePK = 0;
                        GetFieldValues(ControlsEnum.LEAVETYPES);
                        SetFieldValues(ControlsEnum.LEAVETYPES);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.DeleteLeave(CurrPK, LastModifiedTime,Convert.ToInt16(hdfIsDeleteYes.Value));
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MonthlyLeave);
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
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.UsedInAnotherPlace;
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
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.AlreadyDeleted;
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
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MonthlyLeave);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region CLEAR ADD TO LIST
                    case ActionsEnum.CLEARADDTOLIST:
                        ResetForm(ControlsEnum.ADDTOLIST);
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
                        if (txtToDate.Text == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ToDate").ToString()) + "');", true);
                            ddlLeaveType.SelectedIndex = -1;
                            txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
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
                    #region  CANCEL POPUP
                    case ActionsEnum.CANCELPOPUP:
                        ResetForm(ControlsEnum.CLEARPOPUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    #region DELETE ITEM
                    case ActionsEnum.DELETEITEM:
                        MonthlyLeaveBO.LeaveEntryDeleteMaster monthlyLeaveDelete = (MonthlyLeaveBO.LeaveEntryDeleteMaster)SetUIValuesToObject(ControlsEnum.LEAVEDELETE);
                        if (monthlyLeaveDelete.LeaveDayDetails.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpLeaveDetails]','" + GetLocalResourceObject("EmployeeLeaveDts").ToString() + "','400','450');", true);
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForDelete")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(monthlyLeaveDelete);
                        result = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.DeleteLeaveDays(xmlDoc.InnerXml);
                        if (result > 0)
                        {
                            hdfIsDeleteYes.Value = "0";
                            GetFieldValues(ControlsEnum.LEAVEDETAILSHDR);
                            SetFieldValues(ControlsEnum.LEAVEDETAILSHDR);
                            SetFieldValues(ControlsEnum.LEAVEDETAILSLIST);
                            litErrorMsg.Text = GetLocalResourceObject("MsgLeaveDelete").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpLeaveDetails]','" + GetLocalResourceObject("EmployeeLeaveDts").ToString() + "','400','450');", true);
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.AMOUNTEXCEEDED)  // Leave Exsit After Delete Leave Date
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveDelete(2);});", true);
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
                    #region GRIDDELETE
                    case ActionsEnum.GRIDDELETE:
                        DeleteEmpLeave(LeaveDtPk);
                        ResetForm(ControlsEnum.ADDTOLIST);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }


        #endregion
        #region Delete Emp Leave
        private void DeleteEmpLeave(int eldPk)
        {
            List<MonthlyLeaveBO.LeaveDetails> tempList = LeaveDetailsList;
            int result = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.DeleteDtlLeave(eldPk, Convert.ToDateTime(hdfLastModeDateEmp.Value), Convert.ToInt32(hdfIsDeleteYes.Value));
            if (result > 0)
            {
                tempList.Remove(tempList[Convert.ToInt32(hdfCurRowIndex.Value)]);
                LeaveDetailsList = tempList;
                ResetForm(ControlsEnum.ADDTOLIST);
                SetFieldValues(ControlsEnum.LEAVEDETAILSLIST);
                litErrorMsg.Text = GetLocalResourceObject("MsgLeaveDelete").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "');", true);
            }
            else
            {
                if (result == (int)DbSaveStatus.REFERRED)
                {
                    litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave;
                    litErrorMsg.Text += " " + GetGlobalResourceObject("Messages", "UsedInAnotherPlace").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.AMOUNTEXCEEDED)  // Leave Exsit After Delete Leave Date
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){ShowConfirmMsgLeaveDelete(1);});", true);
                }
            }
        }
         #endregion
        #region --- For Grid Actions----
        /// <summary>ActionHandler
        /// Action Handler For GridViewCommandEventArgs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
            GridView senderGridView = (GridView)sender;
            if (senderGridView.ID == "grdLeaveList")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    #region EDIT_ACTION
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    MonthlyLeaveBO.LeaveDetails leaveData = LeaveDetailsList[row.RowIndex];
                    RowIndex = row.RowIndex;
                    LeaveDtPk = leaveData.ELD_PK;
                    hdfEmployee.Value = leaveData.ELD_EMPLOYEE.ToString();
                    txtEmployee.Text = leaveData.ELD_EMPLOYEE_TEXT.ToString();
                    txtFromDate.Text = leaveData.ELD_LV_FROM_DT.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtToDate.Text = leaveData.ELD_LV_TO_DT.ToString(Resources.Constants.HRMSDateFormatShort);
                    chkFromHalfDay.Checked = leaveData.ELD_LV_FROM_HALF == 1 ? true : false;
                    chkToHalfDay.Checked = leaveData.ELD_LV_TO_HALF == 1 ? true : false;
                    LeaveTypePK = leaveData.ELD_LEAVE_TYPE;
                    GetFieldValues(ControlsEnum.LEAVETYPES);
                    SetFieldValues(ControlsEnum.LEAVETYPES);
                    //10/05txtNoOfLeaves.Text = GetFormattedNumber(leaveData.ELD_LEAVE_COUNT);
                    ddlLeaveType.SelectedIndex = ddlLeaveType.Items.IndexOf(ddlLeaveType.Items.FindByValue(leaveData.ELD_LEAVE_TYPE.ToString()));
                    // 18/4 ActionStatus = ActionEnum.EDIT_ACTION;
                    ActionHandler(ddlLeaveType, EventArgs.Empty);
                   // txtDate.Text = leaveData.ELD_MONTH.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtReason.Text = HttpUtility.HtmlDecode(leaveData.ELD_REMARKS);
                    SetFieldValues(ControlsEnum.LEAVEDETAILSLIST);
                    txtBalance.Text = GetFormattedNumber(Convert.ToDecimal(txtBalance.Text) + leaveData.ELD_CREDIT_LEAVE_COUNT);
                    #endregion
                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    #region DELETE_ACTION
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfELD_PK = row.FindControl("hdfELD_PK") as HiddenField;
                    HiddenField hdfELD_MOD_DT = row.FindControl("hdfELD_MOD_DT") as HiddenField;
                    LeaveDtPk = Convert.ToInt32(hdfELD_PK.Value);
                    hdfCurRowIndex.Value =(row.RowIndex).ToString();
                    hdfLastModeDateEmp.Value = hdfELD_MOD_DT.Value;
                    DeleteEmpLeave(LeaveDtPk);
                    #endregion
                }

                else if (e.CommandName == "VIEW_ACTION")
                {
                    ResetForm(ControlsEnum.CLEARPOPUP);
                    int pK = 0;
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfELD_PK = row.FindControl("hdfELD_PK") as HiddenField;
                    int.TryParse(hdfELD_PK.Value, out pK);
                    LvEldPk = pK;
                    Label lblEmployeeName = row.FindControl("lblEmployeeName") as Label;
                    Label lblELD_LEAVE_TYPE_CODE_TEXT = row.FindControl("lblELD_LEAVE_TYPE_CODE_TEXT") as Label;
                    lblEmpNamePopup.Text = ERP.Utilities.CommonFunctions.GetShortString((string.Format(GetLocalResourceObject("Employee:").ToString(), lblEmployeeName.Text)), 50);
                    lblLeaveTypePopup.Text = ERP.Utilities.CommonFunctions.GetShortString((string.Format(GetLocalResourceObject("LeavesType:").ToString(), lblELD_LEAVE_TYPE_CODE_TEXT.Text)), 30);
                    SetFieldValues(ControlsEnum.LEAVEDAYDETAILS);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopUpLeaveDetails]','" + GetLocalResourceObject("EmployeeLeaveDts").ToString() + "','400','450');", true);
                    //LeaveDtPk = 0;
                }
            }
        }

      
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            #region grdEmpLeave_PopUp
            if ((sender as GridView).ID == "grdEmpLeave_PopUp")
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    HiddenField hdfLDD_LEAVE_IS_LOP = e.Row.FindControl("hdfLDD_LEAVE_IS_LOP") as HiddenField;
                    Label lblPayroll_Month_PopUp = e.Row.FindControl("lblPayroll_Month_PopUp") as Label;
                    if (hdfLDD_LEAVE_IS_LOP != null && !string.IsNullOrEmpty(hdfLDD_LEAVE_IS_LOP.Value))
                    {
                        if (Convert.ToInt16(hdfLDD_LEAVE_IS_LOP.Value) == 1)
                        {
                            lblPayroll_Month_PopUp.ForeColor = System.Drawing.Color.Red;
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetLocalResourceObject("LOPRowColor").ToString());
                        }
                    }
                }
            }
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
            //BusinessObject.GridPrams gridParam;
            int brLoc = 0;

            try
            {
                FilterParameters objFilterParam;
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        int employeePk = 0;
                        objFilterParam = new FilterParameters();
                        int.TryParse(hdfFilterBranch.Value, out brLoc);
                        int.TryParse(hdfEmployeeSearch.Value, out employeePk);
                        objFilterParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtFilterFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtFilterToDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterToDate.Text);
                        objFilterParam.BranchLocation = brLoc > 0 ? brLoc : (int?)null;
                        objFilterParam.Employee = employeePk > 0 ? employeePk : (int?)null;
                        objFilterParam.UserPK = currentUser.PKUser;
                        
                        dsPageData = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.GetLeaveList(objFilterParam);
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
                    case ControlsEnum.LEAVEDETAILSHDR:
                        objFilterParam = new FilterParameters();
                        objFilterParam.PK = CurrPK;
                        objFilterParam.Active = (int)DbActiveStatus.HASPK;
                        objLeaveMaster = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.GetLeaveDetails(objFilterParam);
                        if (objLeaveMaster != null)
                            LeaveDetailsList = objLeaveMaster.LeaveDetails;
                        break;
                    #endregion
                    #region LEAVETDETAILS
                    case ControlsEnum.LEAVETDETAILS:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.LeaveTypeMasterBL.GetLeaveType(Convert.ToInt32(ddlLeaveType.SelectedValue), string.Empty, string.Empty, (int)DbActiveStatus.HASPK, currentUser.SBUID, 0, 0, -1, GTIService.Constants.Configurations.Employees.Fields.LTM_CODE);
                        break;
                    #endregion
                    //#region GET LEAVES
                    //case ControlsEnum.GETLEAVES:
                    //    objFilterParam = new FilterParameters();
                    //    int.TryParse(hdfBrLoc.Value, out brLoc);
                    //    objFilterParam.PageNumber = 0;
                    //    objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    //    objFilterParam.ToDate = objFilterParam.FromDate = string.IsNullOrEmpty(txtDate.Text) ? (DateTime?)null : DateTime.Parse(txtDate.Text);
                    //    objFilterParam.BranchLocation = brLoc > 0 ? brLoc : (int?)null;
                    //    objFilterParam.UserPK = currentUser.PKUser;
                    //    dsPageData = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.GetLeaveList(objFilterParam);
                    //    break;
                    //#endregion
                    #region BALANCE
                    case ControlsEnum.BALANCELEAVE:
                        LeaveBalance = 0;
                        leaveDate = Convert.ToDateTime(txtToDate.Text);
                        EmployeePk = 0;
                        //if (!string.IsNullOrEmpty(txtMonth.Text.Trim()))
                        //    leaveDate = DateTime.Parse(txtMonth.Text.Trim());
                        int.TryParse(hdfEmployee.Value, out EmployeePk);
                        LeaveBalance = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.GetBalanceLeave(EmployeePk, Convert.ToInt32(ddlLeaveType.SelectedValue), leaveDate);
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
                    #region LEAVEDETAILSLIST
                    case ControlsEnum.LEAVEDETAILSLIST:
                        BindGrid(ControlsEnum.LEAVEDETAILSLIST);
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
                    case ControlsEnum.LEAVEDETAILSHDR:
                        GetUIValuesFromObject(ControlsEnum.LEAVEDETAILSHDR);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region LEAVEDAYDETAILS
                    case ControlsEnum.LEAVEDAYDETAILS:
                        BindGrid(ControlsEnum.LEAVEDAYDETAILS);
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
                    case ControlsEnum.LEAVEDETAILSHDR:
                        if (objLeaveMaster != null)
                        {
                            txtDate.Text = objLeaveMaster.ELR_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                            txtBrLoc.Text = HttpUtility.HtmlDecode(objLeaveMaster.ELR_BRANCH_TEXT);
                            hdfBrLoc.Value = objLeaveMaster.ELR_BRANCH.ToString();
                            txtRemarks.Text = HttpUtility.HtmlDecode(objLeaveMaster.ELR_REMARKS);
                            CompanyPk = objLeaveMaster.ELR_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));
                            LastModifiedTime = objLeaveMaster.LAST_MOD_DT;
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
                    #region LEAVEDETAILSLIST
                    case ControlsEnum.LEAVEDETAILSLIST:
                        if (LeaveDetailsList != null && LeaveDetailsList.Count > 0)
                            grdLeaveList.DataSource = LeaveDetailsList;
                        else
                            grdLeaveList.DataSource = null;
                        grdLeaveList.DataBind();
                        break;
                    #endregion
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
                    #region LEAVEDAYDETAILS
                    case ControlsEnum.LEAVEDAYDETAILS:
                        if (LeaveDetailsList != null && LeaveDetailsList.Count > 0)
                        {
                            //List<MonthlyLeaveBO.LeaveDetails> tempList = new List<MonthlyLeaveBO.LeaveDetails>();
                            var tempList = LeaveDetailsList.Where(x => x.ELD_PK == LvEldPk).ToList().FirstOrDefault();
                            grdEmpLeave_PopUp.DataSource = tempList.LeaveDayDetails;
                        }
                        else
                        {
                            grdEmpLeave_PopUp.DataSource = null;
                        }
                        grdEmpLeave_PopUp.DataBind();
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
                case ControlsEnum.LEAVEMASTER:
                    MonthlyLeaveBO.LeaveEntryMaster tempEmployeeLeaveMaster = new MonthlyLeaveBO.LeaveEntryMaster();
                    tempEmployeeLeaveMaster.ELD_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                    tempEmployeeLeaveMaster.USER_PK = currentUser.PKUser;
                    tempEmployeeLeaveMaster.BIZUNIT_PK = currentUser.SBUID;
                    tempEmployeeLeaveMaster.ELD_DEPT = currentUser.CurrentDeptPK;
                    tempEmployeeLeaveMaster.ELR_BRANCH = Convert.ToInt32(hdfBrLoc.Value);
                    tempEmployeeLeaveMaster.ELR_DATE = DateTime.Parse(txtDate.Text);
                    tempEmployeeLeaveMaster.ELR_PK = CurrPK;
                    tempEmployeeLeaveMaster.ELR_ACTIVE = (int)DbActiveStatus.ACTIVE;
                    tempEmployeeLeaveMaster.ELR_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                    tempEmployeeLeaveMaster.LAST_MOD_DT = LastModifiedTime;
                    tempEmployeeLeaveMaster.VALIDATE_LEAVE = Convert.ToInt32(hdfValidate_Leave.Value);
                    tempEmployeeLeaveMaster.LEAVE_SKIP = Convert.ToInt32(hdfNoOfLeaveValidation.Value);
                    tempEmployeeLeaveMaster.HOL_SKIP = Convert.ToInt32(hdfHolidayLeaveVal.Value);
                    tempEmployeeLeaveMaster.OFFDAY_SKIP = Convert.ToInt32(hdfOffDaySkip.Value);
                    // tempEmployeeLeaveMaster.LeaveDetails = LeaveDetailsList;
                    returnObject = tempEmployeeLeaveMaster;
                    break;
                #endregion

                #region LEAVEMASTERADD
                case ControlsEnum.LEAVEMASTERADD:
                    MonthlyLeaveBO.LeaveEntryMaster tempLeaveMaster = new MonthlyLeaveBO.LeaveEntryMaster();
                    tempLeaveMaster.ELD_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                    tempLeaveMaster.USER_PK = currentUser.PKUser;
                    tempLeaveMaster.BIZUNIT_PK = currentUser.SBUID;
                    tempLeaveMaster.ELD_DEPT = currentUser.CurrentDeptPK;
                    tempLeaveMaster.ELR_BRANCH = Convert.ToInt32(hdfBrLoc.Value);
                    tempLeaveMaster.ELR_DATE = DateTime.Parse(txtDate.Text);
                    tempLeaveMaster.ELR_PK = CurrPK;
                    tempLeaveMaster.ELR_ACTIVE = (int)DbActiveStatus.ACTIVE;
                    tempLeaveMaster.ELR_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                    tempLeaveMaster.LAST_MOD_DT = LastModifiedTime;
                    tempLeaveMaster.VALIDATE_LEAVE = Convert.ToInt32(hdfValidate_Leave.Value);
                    tempLeaveMaster.LEAVE_SKIP = Convert.ToInt32(hdfNoOfLeaveValidation.Value);
                    tempLeaveMaster.HOL_SKIP = Convert.ToInt32(hdfHolidayLeaveVal.Value);
                    tempLeaveMaster.OFFDAY_SKIP = Convert.ToInt32(hdfOffDaySkip.Value);
                    tempLeaveMaster.LeaveDetails = (List<MonthlyLeaveBO.LeaveDetails>)SetUIValuesToObject(ControlsEnum.LEAVEDETAILSADD);
                    returnObject = tempLeaveMaster;
                    break;
                #endregion

                #region LEAVEDETAILSADD
                case ControlsEnum.LEAVEDETAILSADD:
                    MonthlyLeaveBO.LeaveDetails leaveDataObj = new MonthlyLeaveBO.LeaveDetails();
                    List<MonthlyLeaveBO.LeaveDetails> tempList = new List<MonthlyLeaveBO.LeaveDetails>();
                    if (RowIndex < 0)       // New Mode
                    {
                        if (LeaveDetailsList.Count > 0)
                            RowIndex = (LeaveDetailsList.Max(x => x.ROW_NO));
                        else
                            RowIndex = 0;
                    }
                    leaveDataObj.ROW_NO = RowIndex + 1;
                    leaveDataObj.ELD_EMPLOYEE = GetNullableInt(hdfEmployee.Value).Value;
                    leaveDataObj.ELD_EMPLOYEE_TEXT = HttpUtility.HtmlEncode(txtEmployee.Text.Trim());
                    leaveDataObj.ELD_PK = LeaveDtPk;
                    leaveDataObj.ELD_LEAVE_TYPE = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                    leaveDataObj.ELD_LEAVE_TYPE_TEXT = HttpUtility.HtmlEncode(ddlLeaveType.SelectedItem.Text);
                    //10/05leaveDataObj.ELD_LEAVE_COUNT = GetNullableDecimal(txtNoOfLeaves.Text.Trim()).Value;
                    leaveDataObj.ELD_REMARKS = HttpUtility.HtmlEncode(txtReason.Text);
                    ///leaveDataObj.ELD_MONTH = DateTime.Parse(txtDate.Text.Trim());
                    leaveDataObj.ELD_LV_FROM_DT = DateTime.Parse(txtFromDate.Text);
                    leaveDataObj.ELD_LV_TO_DT = DateTime.Parse(txtToDate.Text);
                    leaveDataObj.ELD_LV_FROM_HALF = chkFromHalfDay.Checked == true ? 1 : 0;
                    leaveDataObj.ELD_LV_TO_HALF = chkToHalfDay.Checked == true ? 1 : 0;
                    tempList.Add(leaveDataObj);
                    returnObject = tempList;
                    break;
                #endregion

                #region LEAVEDELETE
                case ControlsEnum.LEAVEDELETE:
                    MonthlyLeaveBO.LeaveEntryDeleteMaster tempLeaveEntryDeleteMaster = new MonthlyLeaveBO.LeaveEntryDeleteMaster();
                    tempLeaveEntryDeleteMaster.ELR_PK = CurrPK;
                    tempLeaveEntryDeleteMaster.LV_EXIST = Convert.ToInt32(hdfIsDeleteYes.Value);
                    List<MonthlyLeaveBO.LeaveDayDetails> tempLeaveDelete = new List<MonthlyLeaveBO.LeaveDayDetails>();
                    foreach (GridViewRow grdRow in grdEmpLeave_PopUp.Rows)  // Day Details
                    {
                        HiddenField hdfLDD_PK = (HiddenField)grdRow.FindControl("hdfLDD_PK");
                        HiddenField hdfLDD_ELD_PK = (HiddenField)grdRow.FindControl("hdfLDD_ELD_PK");
                        HiddenField hdfLDD_LEAVE_IS_LOP = (HiddenField)grdRow.FindControl("hdfLDD_LEAVE_IS_LOP");
                        HiddenField hdfLDD_LEAVE_IS_HALF = (HiddenField)grdRow.FindControl("hdfLDD_LEAVE_IS_HALF");
                        Label lblPayroll_Month_PopUp = (Label)grdRow.FindControl("lblPayroll_Month_PopUp");
                        Label lblPeriod_PopUp = (Label)grdRow.FindControl("lblPeriod_PopUp");
                        CheckBox chkDeleteDayDetails = (CheckBox)grdRow.FindControl("chkDeleteDayDetails");
                        if (chkDeleteDayDetails.Checked)
                        {
                            MonthlyLeaveBO.LeaveDayDetails tempLeave = new MonthlyLeaveBO.LeaveDayDetails();
                            tempLeave.LDD_PK = Convert.ToInt32(hdfLDD_PK.Value);
                            tempLeave.LDD_ELD_PK = Convert.ToInt32(hdfLDD_ELD_PK.Value);
                            tempLeave.LDD_LEAVE_IS_LOP = Convert.ToInt32(hdfLDD_LEAVE_IS_LOP.Value);
                            tempLeave.LDD_LEAVE_IS_HALF = Convert.ToInt32(hdfLDD_LEAVE_IS_HALF.Value);
                            tempLeave.LDD_DATE = Convert.ToDateTime(lblPayroll_Month_PopUp.Text);
                            tempLeave.LDD_COUNT = Convert.ToDouble(lblPeriod_PopUp.Text);
                            tempLeaveDelete.Add(tempLeave);
                        }
                    }
                    tempLeaveEntryDeleteMaster.LeaveDayDetails = tempLeaveDelete;
                    returnObject = tempLeaveEntryDeleteMaster;
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
                    RowIndex = -1;
                    LeaveDtPk = 0;
                    CreditLeave = 0;
                    txtDate.Text = string.Empty;
                    txtBrLoc.Text = string.Empty;
                    hdfBrLoc.Value = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    ddlLeaveType.ClearSelection();
                    //10/05txtNoOfLeaves.Text = string.Empty;
                    txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
                    txtReason.Text = string.Empty;
                    LeaveDetailsList = null;
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    chkFromHalfDay.Checked = false;
                    chkToHalfDay.Checked = false;

                    break;
                #endregion
                #region ADDTOLIST
                case ControlsEnum.ADDTOLIST:
                    LeaveDtPk = 0;
                    RowIndex = -1;
                    CreditLeave = 0;
                    //10/05txtNoOfLeaves.Text = string.Empty;
                    ddlLeaveType.ClearSelection();
                    txtReason.Text = string.Empty;
                    txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    chkFromHalfDay.Checked = false;
                    chkToHalfDay.Checked = false;
                    hdfIsDeleteYes.Value = "0";
                    hdfLastModeDateEmp.Value = string.Empty;
                    hdfCurRowIndex.Value = "-1";
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    txtFilterBranch.Text = string.Empty;
                    hdfFilterBranch.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtEmployeeSearch.Text = string.Empty;
                    hdfEmployeeSearch.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtFilterFromDate.Text = string.Empty;
                    txtFilterToDate.Text = string.Empty;
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
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEES,
            LEAVETYPES,
            COMPANY,
            LEAVEDETAILSLIST,
            ADDTOLIST,
            LEAVEMASTER,
            CLEAR,
            SALARYPKLIST,
            LIST,
            CLEARSEARCH,
            BALANCELEAVE,
            LEAVETDETAILS,
            LEAVEDETAILSHDR,
            GETLEAVES,
            CLEARPOPUP,
            LEAVEMASTERADD,
            LEAVEDAYDETAILS,
            LEAVEDETAILSADD,
            LEAVEDELETE
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