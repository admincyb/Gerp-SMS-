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

namespace HRMS.Payroll
{
    public partial class MonthlyLeave_old : ERP.Store.UI.MyBasePage
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
        private List<MonthlyLeaveBO.MonthlyLeaveData> MonthlyLeaveDataList
        {
            get
            {
                return ViewState["MonthlyLeaveDataList"] == null ? new List<MonthlyLeaveBO.MonthlyLeaveData>() : (List<MonthlyLeaveBO.MonthlyLeaveData>)ViewState["MonthlyLeaveDataList"];
            }
            set
            {
                ViewState["MonthlyLeaveDataList"] = value;
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
        private ActionEnum ActionStatus
        {
            get
            {
                return this.ViewState["ActionStatus"] == null ? ActionEnum.NEW_ACTION : (ActionEnum)(this.ViewState["ActionStatus"]);
            }
            set
            {
                this.ViewState["ActionStatus"] = value;
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
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
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
                    uclPaging.CurrentPage = 1;
                    //ResetForm(ControlsEnum.CLEAR);
                    //GetFieldValues(ControlsEnum.EMPLOYEES);
                    //SetFieldValues(ControlsEnum.EMPLOYEES);
                    GetFieldValues(ControlsEnum.LEAVETYPES);
                    SetFieldValues(ControlsEnum.LEAVETYPES);
                    GetFieldValues(ControlsEnum.SALARYPKLIST);
                    SetFieldValues(ControlsEnum.SALARYPKLIST);
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
            List<MonthlyLeaveBO.MonthlyLeaveData> tempList = MonthlyLeaveDataList;
            bool bIsChecked = false;
            int rowNow;
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
                    if ((((DropDownList)sender).ID == "ddlEmployee"))
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                    if ((((DropDownList)sender).ID == "ddlLeaveType"))
                    {
                        commonActions = ActionsEnum.CHANGETYPE;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if ((((TextBox)sender).ID == "txtMonth"))
                    {
                        commonActions = ActionsEnum.CHANGE;
                        txtEmployee.Enabled = true;
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
                    #region CHANGE
                    case ActionsEnum.CHANGE:
                        BindDetailList();
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        if (hdfIscontYes.Value != "1" && MonthlyLeaveDataList != null && MonthlyLeaveDataList.Where(r => r.ELD_PK == 0).Count() > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowMsgUnsaved();});", true);
                            return;
                        }
                        BindDetailList();
                        hdfIscontYes.Value = CommonConstants.SELECT_VALUE_ZERO;
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        tempList = MonthlyLeaveDataList;
                        if (hdfCurrentELD_PK.Value == string.Empty && hdfCurrentROW_NO.Value == string.Empty) // New
                        {
                            int employeeID = GetNullableInt(hdfEmployee.Value).Value;
                            string employeeText = txtEmployee.Text.Trim();
                            int leaveType = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                            DateTime date = DateTime.Parse(txtMonth.Text.Trim());
                            MonthlyLeaveBO.MonthlyLeaveData existLeave = tempList.
                                  Where(x => x.ELD_EMPLOYEE == employeeID && x.ELD_LEAVE_TYPE == leaveType && x.ELD_MONTH == date && x.IS_DELETED != 1)
                                  .FirstOrDefault();
                            if (existLeave != null)
                            {
                                litErrorMsg.Text = string.Format("Date:{0}; Employee:{1}; Leave Type:{2}; Already exists in list",
                                    date.ToString(Resources.Constants.DateFormatShort), existLeave.ELD_EMPLOYEE_TEXT, existLeave.ELD_LEAVE_TYPE_CODE_TEXT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                    CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            MonthlyLeaveBO.MonthlyLeaveData leaveDataObj = new MonthlyLeaveBO.MonthlyLeaveData();
                            if (tempList.Count > 0) leaveDataObj.ROW_NO = (tempList.Max(x => x.ROW_NO) + 1);
                            else leaveDataObj.ROW_NO = 1;
                            leaveDataObj.ELD_EMPLOYEE = employeeID;
                            leaveDataObj.ELD_EMPLOYEE_TEXT = HttpUtility.HtmlEncode(employeeText);
                            leaveDataObj.ELD_PK = 0;
                            leaveDataObj.ELD_LEAVE_TYPE = leaveType;
                            leaveDataObj.ELD_LEAVE_TYPE_TEXT = HttpUtility.HtmlEncode(ddlLeaveType.SelectedItem.Text);
                            leaveDataObj.ELD_LEAVE_COUNT = GetNullableDecimal(txtNoOfLeaves.Text.Trim()).Value;
                            leaveDataObj.ELD_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                            leaveDataObj.ELD_MONTH = DateTime.Parse(txtMonth.Text);
                            //leaveDataObj.ELD_ACTIVE = 1;
                            tempList.Add(leaveDataObj);
                        }
                        else // Update
                        {
                            int eldPk = GetNullableInt(hdfCurrentELD_PK.Value).Value;
                            int rowNo = GetNullableInt(hdfCurrentROW_NO.Value).Value;

                            DateTime date = DateTime.Parse(txtMonth.Text.Trim());
                            MonthlyLeaveBO.MonthlyLeaveData existLeave;

                            MonthlyLeaveBO.MonthlyLeaveData leaveDataObj = tempList
                                .Where(x => x.ROW_NO == rowNo && x.ELD_PK == eldPk)
                                .Single();
                            int leaveType = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                            if (EntryStatus == EntryStatus.NEWMODE)
                            {
                                existLeave = tempList.Where(x => x.ELD_EMPLOYEE == leaveDataObj.ELD_EMPLOYEE && x.ELD_LEAVE_TYPE == leaveType
                                       && x.ROW_NO != rowNo && x.ELD_PK == eldPk && x.ELD_MONTH == date && x.IS_DELETED != 1).FirstOrDefault();
                            }
                            else
                            {
                                existLeave = tempList.Where(x => x.ELD_EMPLOYEE == leaveDataObj.ELD_EMPLOYEE && x.ELD_LEAVE_TYPE == leaveType
                                       && x.ROW_NO != rowNo && x.ELD_PK != eldPk && x.ELD_MONTH == date && x.IS_DELETED != 1).FirstOrDefault();
                            }
                            if (existLeave != null)
                            {
                                litErrorMsg.Text = string.Format("Date:{0}; Employee:{1}; Leave Type:{2}; Already exists in list",
                                        date.ToString(Resources.Constants.DateFormatShort), existLeave.ELD_EMPLOYEE_TEXT, existLeave.ELD_LEAVE_TYPE_CODE_TEXT);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                    CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }

                            leaveDataObj.ELD_LEAVE_TYPE = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                            leaveDataObj.ELD_LEAVE_TYPE_TEXT = HttpUtility.HtmlEncode(ddlLeaveType.SelectedItem.Text);
                            leaveDataObj.ELD_LEAVE_COUNT = GetNullableDecimal(txtNoOfLeaves.Text.Trim()).Value;
                            leaveDataObj.ELD_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                            leaveDataObj.ELD_MONTH = date;
                        }
                        MonthlyLeaveDataList = tempList;
                        txtEmployee.Enabled = true;
                        ResetForm(ControlsEnum.ADDTOLIST);
                        SetFieldValues(ControlsEnum.MONTHLYLEAVELIST);
                        ActionStatus = ActionEnum.NEW_ACTION;
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        MonthlyLeaveBO.MonthlyLeaveMaster monthlyLeaveM = (MonthlyLeaveBO.MonthlyLeaveMaster)SetUIValuesToObject(ControlsEnum.MONTHLYLEAVEMASTER);
                        if (monthlyLeaveM.MonthlyLeaveDatas.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }

                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(monthlyLeaveM);
                        result = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.SaveEmployeeMonthlyLeaveMaster(xmlDoc.InnerXml);
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
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.MonthlyLeave + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
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
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_DateOutSalaryYear").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
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
                                ResetForm(ControlsEnum.CLEAR);
                                //currPK 
                                rowNow = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRowNoListPage")).Value);
                                monthYear = ((Label)grdrow.FindControl("lblMonthListPage")).Text;
                                //monthYear = ((Label)grdrow.FindControl("lblMonthListPage")).Text + "-" + ((HiddenField)grdrow.FindControl("hdfYearListPage")).Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            //ddlEmployee.SelectedIndex = 0;
                            hdfEmployee.Value = CommonConstants.SELECT_VALUE_ZERO;
                            txtEmployee.Text = GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString();
                            //txtMonth.Text = monthYear;
                            DateTime leaveDate = DateTime.Parse(monthYear);
                            txtMonth.Text = new DateTime(leaveDate.Year, leaveDate.Month, DateTime.DaysInMonth(leaveDate.Year, leaveDate.Month)).ToString(Resources.Constants.HRMSDateFormatShort);
                            ActionHandler(btnChange, EventArgs.Empty);
                            EntryStatus = EntryStatus.EDITMODE;
                            txtEmployee.Enabled = true;
                            LeaveTypePK = 0;
                            GetFieldValues(ControlsEnum.LEAVETYPES);
                            SetFieldValues(ControlsEnum.LEAVETYPES);
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
                        MonthlyLeaveDataList = new List<MonthlyLeaveBO.MonthlyLeaveData>();
                        ResetForm(ControlsEnum.CLEAR);
                        //txtMonth.Text = DateTime.Now.Date.ToString("MMM-yyyy");
                        //ddlEmployee.SelectedIndex = 0;
                        txtMonth.Text = string.Empty; //DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        txtEmployee.Enabled = true;
                        hdfEmployee.Value = CommonConstants.SELECT_VALUE_ZERO;
                        txtEmployee.Text = GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString();
                        SetFieldValues(ControlsEnum.MONTHLYLEAVELIST);
                        //ActionHandler(btnChange, EventArgs.Empty);
                        LeaveTypePK = 0;
                        GetFieldValues(ControlsEnum.LEAVETYPES);
                        SetFieldValues(ControlsEnum.LEAVETYPES);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
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
                    #region CHANGE LEAVE TYPE
                    case ActionsEnum.CHANGETYPE:
                        cmpNoOfLeaves.Enabled = false;
                        GetFieldValues(ControlsEnum.LEAVETDETAILS);
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(dtResult.Rows[0]["LTM_CREDIT"]) == 1 && hdfNoOfLeaveValidation.Value == "1")
                                cmpNoOfLeaves.Enabled = true;
                        }
                        GetFieldValues(ControlsEnum.BALANCELEAVE);
                        SetFieldValues(ControlsEnum.BALANCELEAVE);
                        decimal total = tempList.Where(x => x.ELD_EMPLOYEE == GetNullableInt(hdfEmployee.Value).Value && x.ELD_LEAVE_TYPE == GetNullableInt(ddlLeaveType.SelectedValue).Value && x.IS_DELETED != 1).Sum(X => X.ELD_LEAVE_COUNT);
                        decimal delTotal = tempList.Where(x => x.ELD_EMPLOYEE == GetNullableInt(hdfEmployee.Value).Value && x.ELD_LEAVE_TYPE == GetNullableInt(ddlLeaveType.SelectedValue).Value && x.IS_DELETED == 1).Sum(X => X.ELD_LEAVE_COUNT);
                        if (EntryStatus != EntryStatus.EDITMODE && ActionStatus != ActionEnum.EDIT_ACTION)
                            txtBalance.Text = (Convert.ToDecimal(txtBalance.Text) - total).ToString();
                        else if (EntryStatus == EntryStatus.EDITMODE && (ActionStatus == ActionEnum.EDIT_ACTION))
                            txtBalance.Text = GetFormattedNumber(Convert.ToDecimal(txtBalance.Text) + delTotal + Convert.ToDecimal(txtNoOfLeaves.Text)).ToString();
                        else if (EntryStatus == EntryStatus.EDITMODE && (ActionStatus == ActionEnum.NEW_ACTION))
                        {
                             
                            //int eldPk = GetNullableInt(hdfCurrentELD_PK.Value).Value;
                            //total = tempList.Where(x => x.ELD_EMPLOYEE == GetNullableInt(hdfEmployee.Value).Value && x.ELD_LEAVE_TYPE == GetNullableInt(ddlLeaveType.SelectedValue).
                            //    Value && x.IS_DELETED != 1 && x.ELD_PK != eldPk).Sum(X => X.ELD_LEAVE_COUNT);
                            //txtBalance.Text = GetFormattedNumber(Convert.ToDecimal(txtBalance.Text) - total+ delTotal + Convert.ToDecimal(txtNoOfLeaves.Text)).ToString();
                        }
                            SetFieldValues(ControlsEnum.MONTHLYLEAVELIST);
                        break;
                    #endregion
                    #region CHANGE EMPLOYEE
                    case ActionsEnum.CHANGEEMPLOYEE:
                        GetFieldValues(ControlsEnum.LEAVETYPES);
                        SetFieldValues(ControlsEnum.LEAVETYPES);
                        ActionStatus = ActionEnum.NEW_ACTION;
                        txtBalance.Text = "0";
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// For binding detail grid view
        /// </summary>
        private void BindDetailList()
        {
            hdfCurrentELD_PK.Value = hdfCurrentROW_NO.Value = string.Empty;
            EmployeePk = 0;
            if (string.IsNullOrEmpty(txtEmployee.Text.Trim()) || txtEmployee.Text.Trim().Equals(GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString()))
                hdfEmployee.Value = CommonConstants.SELECT_VALUE_ZERO;
            int.TryParse(hdfEmployee.Value, out EmployeePk);
            //if (EmployeePk >= 0 && txtMonth.Text.Trim() != string.Empty)//ddlEmployee.SelectedIndex > -1
            //{
            GetFieldValues(ControlsEnum.MONTHLYLEAVELIST);
            SetFieldValues(ControlsEnum.MONTHLYLEAVELIST);
            //}
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
            int rowNo;
            int employeeLeaveDataID;
            if (senderGridView.ID == "grdLeaveList")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfELD_PK = row.FindControl("hdfELD_PK") as HiddenField;
                    HiddenField hdfROW_NO = row.FindControl("hdfROW_NO") as HiddenField;                   
                    Label lblgrdRemarks = row.FindControl("lblgrdRemarks") as Label;                    
                    hdfCurrentROW_NO.Value = hdfROW_NO.Value;
                    hdfCurrentELD_PK.Value = hdfELD_PK.Value;
                    employeeLeaveDataID = GetNullableInt(hdfELD_PK.Value).Value;
                    rowNo = GetNullableInt(hdfCurrentROW_NO.Value).Value;
                    MonthlyLeaveBO.MonthlyLeaveData leaveData = MonthlyLeaveDataList
                        .Where(x => x.ELD_PK == employeeLeaveDataID && x.ROW_NO == rowNo)
                        .Single();
                    //ddlEmployee.SelectedIndex = ddlEmployee.Items.IndexOf(ddlEmployee.Items.FindByValue(leaveData.ELD_EMPLOYEE.ToString()));
                    hdfEmployee.Value = leaveData.ELD_EMPLOYEE.ToString();
                    txtEmployee.Text = leaveData.ELD_EMPLOYEE_TEXT.ToString();
                    txtEmployee.Enabled = false;
                    LeaveTypePK = leaveData.ELD_LEAVE_TYPE;
                    GetFieldValues(ControlsEnum.LEAVETYPES);
                    SetFieldValues(ControlsEnum.LEAVETYPES);
                    txtNoOfLeaves.Text = leaveData.ELD_LEAVE_COUNT.ToString();
                    ddlLeaveType.SelectedIndex = ddlLeaveType.Items.IndexOf(ddlLeaveType.Items.FindByValue(leaveData.ELD_LEAVE_TYPE.ToString()));
                    ActionStatus = ActionEnum.EDIT_ACTION;
                    ActionHandler(ddlLeaveType, EventArgs.Empty);
                    txtMonth.Text = leaveData.ELD_MONTH.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtRemarks.Text = HttpUtility.HtmlDecode(leaveData.ELD_REMARKS);
                    SetFieldValues(ControlsEnum.MONTHLYLEAVELIST); 
                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfELD_PK = row.FindControl("hdfELD_PK") as HiddenField;
                    HiddenField hdfROW_NO = row.FindControl("hdfROW_NO") as HiddenField;
                    hdfCurrentROW_NO.Value = hdfROW_NO.Value;
                    hdfCurrentELD_PK.Value = hdfELD_PK.Value;
                    employeeLeaveDataID = GetNullableInt(hdfELD_PK.Value).Value;
                    rowNo = GetNullableInt(hdfCurrentROW_NO.Value).Value;
                    MonthlyLeaveBO.MonthlyLeaveData leaveData = MonthlyLeaveDataList
                        .Where(x => x.ELD_PK == employeeLeaveDataID && x.ROW_NO == rowNo)
                        .Single();
                    List<MonthlyLeaveBO.MonthlyLeaveData> tempList = MonthlyLeaveDataList;
                    if (employeeLeaveDataID == 0) tempList.Remove(leaveData);
                    else leaveData.IS_DELETED = 1;
                    MonthlyLeaveDataList = tempList;
                    ActionStatus = ActionEnum.EDIT_ACTION;
                    ResetForm(ControlsEnum.AFTERDELETE);
                    SetFieldValues(ControlsEnum.MONTHLYLEAVELIST);
                }
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (((GridView)sender).ID == "grdLeaveList")
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    Label lblDateGridview = (Label)e.Row.FindControl("lblDate");
                    Label lblWeekDay = (Label)e.Row.FindControl("lblWeekDay");
                    lblWeekDay.Text = (Convert.ToDateTime(lblDateGridview.Text).DayOfWeek.ToString()).Substring(0,3).ToUpper();
                    lblWeekDay.ToolTip = Convert.ToDateTime(lblDateGridview.Text).DayOfWeek.ToString();
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
            BusinessObject.GridPrams gridParam;
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.SearchBy = string.Empty;
                        //gridParam.SearchValue = hdfSearchEmployee.Value;
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        // gridParam.Fields = GTIService.Constants.DirectStockTransfer.Parameters.GridParmeters;
                        //"[GRH_PK],[GRH_NO],[GRH_DATE],[GRH_STATUS],[GRH_STATUS_TEXT],[REF_ID],[DPT_NAME],[GRH_VENDOR_TEXT],[GRH_PO_NO],[GRH_VND_REF_NO]";
                        // gridParam.SortBy = GTIService.Constants.DirectStockTransfer.Fields.GRH_PK_SortBy;
                        //  gridParam.SortDirection = "DESC";
                        //gridParam.FromDate = txtFromDate.Text;
                        //gridParam.ToDate = txtToDate.Text;
                        //gridParam.FilterStatus = ddlStatus.SelectedValue;                     

                        dsPageData = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.GetYearlyLeaveListListingPage(gridParam, GetNullableInt(ddlYearListPage.SelectedValue).Value);
                        break;
                    #endregion

                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, 0);
                        break;
                    #endregion
                    //#region EMPLOYEES
                    //case ControlsEnum.EMPLOYEES:
                    //    dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDetailListHeader(0, GTIService.Constants.Configurations.Employees.Fields.NAME, 1);
                    //    break;
                    //#endregion
                    #region LEAVETYPES
                    case ControlsEnum.LEAVETYPES:
                        int empPk = 0;
                        int.TryParse(hdfEmployee.Value, out empPk);
                        dtResult = BusinessLogic.HRMS.Admin.Masters.LeaveTypeMasterBL.GetLeaveTypeDDL(LeaveTypePK, string.Empty, string.Empty, 1, currentUser.SBUID, 0, 0, -1, GTIService.Constants.Configurations.Employees.Fields.LTM_CODE, null,  empPk);
                        break;
                    #endregion
                    #region LEAVETDETAILS
                    case ControlsEnum.LEAVETDETAILS:
                        dtResult = BusinessLogic.HRMS.Admin.Masters.LeaveTypeMasterBL.GetLeaveType(Convert.ToInt32(ddlLeaveType.SelectedValue), string.Empty, string.Empty, (int)DbActiveStatus.HASPK, currentUser.SBUID, 0, 0, -1, GTIService.Constants.Configurations.Employees.Fields.LTM_CODE);
                        break;
                    #endregion
                    #region SALARYPKLIST
                    case ControlsEnum.SALARYPKLIST:
                        dsPageData = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.GetSalaryPkList(0, 1, currentUser.SBUID);
                        break;
                    #endregion

                    #region MONTHLYLEAVELIST
                    case ControlsEnum.MONTHLYLEAVELIST:
                        leaveDate = null;
                        if (!string.IsNullOrEmpty(txtMonth.Text.Trim()))
                            leaveDate = DateTime.Parse(txtMonth.Text.Trim());
                        string xmlData = BusinessLogic.HRMS.Payroll.MonthlyLeaveBL.GetEmployeeMonthlyLeaveList(GetNullableInt(hdfEmployee.Value).Value
                            , leaveDate);
                        List<MonthlyLeaveBO.MonthlyLeaveData> lstTempLeaveList;
                        if (xmlData == "<Root/>")
                        {
                            lstTempLeaveList = new List<MonthlyLeaveBO.MonthlyLeaveData>();
                        }
                        else
                        {
                            MonthlyLeaveBO.MonthlyLeaveDataRoot root = CommonFunctions.XmlDeserialize<MonthlyLeaveBO.MonthlyLeaveDataRoot>(xmlData);
                            lstTempLeaveList = root.MonthlyLeaveDatas;
                        }
                        //foreach (var item in lstTempLeaveList) item.ELD_MONTH = txtMonth.Text.Trim();
                        MonthlyLeaveDataList = lstTempLeaveList;
                        break;
                    #endregion
                    #region BALANCE
                    case ControlsEnum.BALANCELEAVE:
                        LeaveBalance = 0;
                        leaveDate = null;
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
                    //#region EMPLOYEES
                    //case ControlsEnum.EMPLOYEES:
                    //    BindDropDown(ControlsEnum.EMPLOYEES);
                    //    break;
                    //#endregion
                    #region LEAVETYPES
                    case ControlsEnum.LEAVETYPES:
                        BindDropDown(ControlsEnum.LEAVETYPES);
                        break;
                    #endregion
                    #region MONTHLYLEAVELIST
                    case ControlsEnum.MONTHLYLEAVELIST:
                        BindGrid(ControlsEnum.MONTHLYLEAVELIST);
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
                        txtBalance.Text = LeaveBalance.ToString();
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
                #region EMPLOYEES
                case ControlsEnum.EMPLOYEES:
                    //ddlEmployee.Items.Clear();
                    //ddlEmployee.DataSource = dtResult;
                    //ddlEmployee.DataTextField = GTIService.Constants.Configurations.Employees.Fields.NAME;
                    //ddlEmployee.DataValueField = GTIService.Constants.Configurations.Employees.Fields.PK;
                    //ddlEmployee.DataBind();
                    //ddlEmployee.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    //ddlEmployee.Items.HtmlDecode();
                    break;
                #endregion
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
                #region SALARYPKLIST
                case ControlsEnum.SALARYPKLIST:
                    ddlYearListPage.Items.Clear();
                    ddlYearListPage.DataSource = null;
                    if (dsPageData != null && dsPageData.Tables.Count > 0)
                    {
                        ddlYearListPage.DataSource = dsPageData.Tables[0];
                        ddlYearListPage.DataTextField = GTIService.Constants.HRMS.Payroll.Fields.HYR_NAME;
                        ddlYearListPage.DataValueField = GTIService.Constants.HRMS.Payroll.Fields.HYR_PK;
                    }
                    ddlYearListPage.DataBind();
                    ddlYearListPage.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    ddlYearListPage.SelectedIndex = ddlYearListPage.Items.Count > 1 ? 1 : 0;
                    ddlYearListPage.Items.HtmlDecode();
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
                    #region MONTHLYLEAVELIST
                    case ControlsEnum.MONTHLYLEAVELIST:
                        if (MonthlyLeaveDataList != null && MonthlyLeaveDataList.Count > 0)
                        {
                            grdLeaveList.DataSource = MonthlyLeaveDataList
                                .Where(x => x.IS_DELETED == 0);
                            grdLeaveList.DataBind();
                        }
                        else
                        {
                            grdLeaveList.DataSource = null;
                            grdLeaveList.DataBind();
                        }
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
                #region MONTHLYLEAVEMASTER
                case ControlsEnum.MONTHLYLEAVEMASTER:
                    MonthlyLeaveBO.MonthlyLeaveMaster tempEmployeeLeaveMaster = new MonthlyLeaveBO.MonthlyLeaveMaster();
                    
                //Edit Record
                    if (hdfCurrentELD_PK.Value != string.Empty)
                    {
                        MonthlyLeaveBO.MonthlyLeaveData objCurrentRow = MonthlyLeaveDataList.Where(r => r.ELD_PK == Convert.ToInt32(hdfCurrentELD_PK.Value)).SingleOrDefault();
                        objCurrentRow.ELD_EMPLOYEE = GetNullableInt(hdfEmployee.Value).Value;
                        objCurrentRow.ELD_EMPLOYEE_TEXT = HttpUtility.HtmlEncode(txtEmployee.Text.Trim());
                        objCurrentRow.ELD_PK = Convert.ToInt32(hdfCurrentELD_PK.Value);
                        objCurrentRow.ELD_LEAVE_TYPE = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                        objCurrentRow.ELD_LEAVE_TYPE_TEXT = HttpUtility.HtmlEncode(ddlLeaveType.SelectedItem.Text);
                        objCurrentRow.ELD_LEAVE_COUNT = GetNullableDecimal(txtNoOfLeaves.Text.Trim()).Value;
                        objCurrentRow.ELD_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        objCurrentRow.ELD_MONTH = DateTime.Parse(txtMonth.Text);
                    }

                    tempEmployeeLeaveMaster.ELD_DEPT = currentUser.CurrentDeptPK;

                    GetFieldValues(ControlsEnum.COMPANY);

                    tempEmployeeLeaveMaster.ELD_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    tempEmployeeLeaveMaster.USER_PK = currentUser.PKUser;
                    tempEmployeeLeaveMaster.BIZUNIT_PK = currentUser.SBUID;
                    tempEmployeeLeaveMaster.MonthlyLeaveDatas = MonthlyLeaveDataList;
                    //tempEmployeeLeaveMaster.ELD_MONTH = DateTime.Parse(txtMonth.Text.Trim());
                    //if (MonthlyLeaveDataList.Count > 0) tempEmployeeLeaveMaster.ELD_MONTH = MonthlyLeaveDataList[0].ELD_MONTH;
                  
                    
                    returnObject = tempEmployeeLeaveMaster;
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
                    hdfCurrentELD_PK.Value = hdfCurrentROW_NO.Value = string.Empty;
                    ddlLeaveType.SelectedIndex = -1;
                    // txtDate.Text = DateTime.Now.Date.ToString("dd-MMM-yyyy");
                    txtNoOfLeaves.Text = string.Empty; // txtRemarks.Text =
                    //currPK = 0;
                    txtEmployee.Enabled = true;
                    hdfEmployee.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtEmployee.Text = GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString();
                    txtRemarks.Text = string.Empty;
                    txtBalance.Text = string.Empty;
                    txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
                    MonthlyLeaveDataList.Clear();
                    break;
                #endregion
                #region ADDTOLIST
                case ControlsEnum.ADDTOLIST:
                    hdfCurrentELD_PK.Value = hdfCurrentROW_NO.Value = txtNoOfLeaves.Text = string.Empty;
                    txtEmployee.Enabled = true;
                    ddlLeaveType.SelectedIndex = -1;
                    txtRemarks.Text = string.Empty;
                    txtBalance.Text = string.Empty;
                    txtBalance.Text = CommonConstants.SELECT_VALUE_ZERO;
                    break;
                #endregion
                #region AFTERDELETE
                case ControlsEnum.AFTERDELETE:
                    hdfCurrentELD_PK.Value = hdfCurrentROW_NO.Value = string.Empty;
                    txtNoOfLeaves.Text = string.Empty;
                    txtEmployee.Enabled = true;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    ddlYearListPage.SelectedIndex = 1;
                    PageIndexList = 1.ToString();
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
            string format = "#0.0";
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
            MONTHLYLEAVELIST,
            ADDTOLIST,
            AFTERDELETE,
            MONTHLYLEAVEMASTER,
            CLEAR,
            SALARYPKLIST,
            LIST,
            CLEARSEARCH,
            BALANCELEAVE,
            LEAVETDETAILS 
        }
        #endregion

        public enum ActionEnum
        {
            EDIT_ACTION,
            DELETE_ACTION,
            NEW_ACTION,
        }
    }
}