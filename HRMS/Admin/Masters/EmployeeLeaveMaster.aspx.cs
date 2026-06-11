using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.Common;
using ERP.Utilities;
using System.Data;

using BusinessObject.HRMS.Admin.Masters;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using System.Xml;
using ERPSMS_v01.UserControls;

namespace HRMS.Admin.Masters
{
    public partial class EmployeeLeaveMaster : ERP.Store.UI.MyBasePage
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

        private List<EmployeeLeaveMasterBO.EmployeeLeaveDetail> EmployeeLeaveDetailList
        {
            get
            {
                return ViewState["EmployeeLeaveDetailList"] == null ? new List<EmployeeLeaveMasterBO.EmployeeLeaveDetail>() : (List<EmployeeLeaveMasterBO.EmployeeLeaveDetail>)ViewState["EmployeeLeaveDetailList"];
            }
            set
            {
                ViewState["EmployeeLeaveDetailList"] = value;
            }
        }

        private EmployeeLeaveMasterBO.EmployeeLeaveMaster EmployeeLeaveMasterViewState
        {
            get
            {
                return ViewState["EmployeeLeaveMasterViewState"] == null ? new EmployeeLeaveMasterBO.EmployeeLeaveMaster() : (EmployeeLeaveMasterBO.EmployeeLeaveMaster)ViewState["EmployeeLeaveMasterViewState"];
            }
            set
            {
                ViewState["EmployeeLeaveMasterViewState"] = value;
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

        #endregion

        private BusinessObject.User currentUser;
        private DataTable dtResult;
        decimal currentELH_LEAVE_BAL;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private int currPK { get; set; }
        private string dummyPK { get; set; }
        private int EmpTypePk = 0;
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
            uclEmpDetListPaging.CurrentPage = 1;
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

            this.uclEmpDetListPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclEmpDetListPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclEmpDetListPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclEmpDetListPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclEmpDetListPaging.PageChanged += new ActionHandler(this.ActionHandler);
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
                else if (senderId == "uclEmpDetListPaging")
                {
                    PageIndexList = uclEmpDetListPaging.CurrentPage.ToString();
                    SetFieldValues(ControlsEnum.LEAVECREDITLIST);
                    EnableDisableButtons(e.TotalPages, "uclEmpDetListPaging");
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
                    uclEmpDetListPaging.CurrentPage = 1;
                    ResetForm(ControlsEnum.CLEAR);
                    GetFieldValues(ControlsEnum.LEAVETYPES);
                    SetFieldValues(ControlsEnum.LEAVETYPES);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.BRANCH);
                    SetFieldValues(ControlsEnum.BRANCH);
                    GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
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
            DataTable dtErrorList = new DataTable();
            List<EmployeeLeaveMasterBO.EmployeeLeaveDetail> tempList = EmployeeLeaveDetailList;
            Dictionary<int, string> Employees = new Dictionary<int, string>();
            bool bIsChecked = false;
            string strError = string.Empty;
            int dept = 0;
            int empType = 0;
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
                    if ((((DropDownList)sender).ID == "ddlEmployee") || (((DropDownList)sender).ID == "ddlBranchLocation"))
                    {
                        txtDetailsEmployee.Text = CommonConstants.ALL;
                    }
                    else if ((((DropDownList)sender).ID == "ddlLeaveType"))
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
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CHANGETYPE
                    case ActionsEnum.CHANGETYPE:
                        if (ddlLeaveType.SelectedIndex > -1)
                        {
                            SetFieldValues(ControlsEnum.CURRENTCREDITBYLEAVETYPE);
                        }
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        int empPK = 0;
                        int.TryParse(hdfDetailsEmployee.Value, out empPK);
                        int branchPK = 0;
                        int.TryParse(hdfBranchLocation.Value, out branchPK);
                        int.TryParse(hdfDepartment.Value, out dept);
                        int.TryParse(ddlEmployeeType.SelectedValue, out empType);
                        dtResult = BusinessLogic.HRMS.Admin.Masters.EmployeeLeaveMasterBL.GetOBLeaveEmployeeListByFilter((empPK > 0 ? empPK : (int?)null),  branchPK > 0 ? branchPK : (int?)null,
                                                 dept > 0 ? dept : (int?)null, empType > 0 ? empType : (int?)null, GetNullableInt(ddlLeaveType.SelectedValue).Value, txtDate.Text);
                        string date = txtDate.Text.Trim();
                        if (hdfCurrentELH_PK.Value == string.Empty && hdfCurrentROW_NO.Value == string.Empty && dtResult != null && dtResult.Rows.Count > 0) // New
                        {
                            foreach (DataRow drEmp in dtResult.Rows)
                            {
                                int leaveType = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                                EmployeeLeaveMasterBO.EmployeeLeaveDetail existLeave = tempList.
                                  Where(x => x.ELH_EMPLOYEE == Convert.ToInt32(drEmp["empPK"]) && x.ELH_LEAVE_TYPE == leaveType && x.ELH_DATE == date && x.IS_DELETED == 0)
                                  .FirstOrDefault();
                                if (existLeave != null)
                                {
                                    litErrorMsg.Text = string.Format("Date:{0}; Employee:{1}; Leave Type:{2}; Already exists in list",
                                        date, existLeave.ELH_EMPLOYEE_TEXT, existLeave.ELH_LEAVE_TYPE_TEXT);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                        CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                EmployeeLeaveMasterBO.EmployeeLeaveDetail leaveDataObj = new EmployeeLeaveMasterBO.EmployeeLeaveDetail();
                                if (tempList.Count > 0) leaveDataObj.ROW_NO = (tempList.Max(x => x.ROW_NO) + 1);
                                else leaveDataObj.ROW_NO = 1;
                                leaveDataObj.ELH_EMPLOYEE = Convert.ToInt32(drEmp["empPK"]);
                                leaveDataObj.ELH_EMPLOYEE_TEXT = HttpUtility.HtmlEncode(Convert.ToString(drEmp["emptext"]));
                                leaveDataObj.ELH_PK = 0;
                                leaveDataObj.ELH_LEAVE_TYPE = leaveType;
                                leaveDataObj.ELH_LEAVE_TYPE_TEXT = HttpUtility.HtmlEncode(ddlLeaveType.SelectedItem.Text);
                                leaveDataObj.ELH_LEAVE_BAL = GetNullableDecimal(txtELH_LEAVE_BAL.Text.Trim()).Value;
                                leaveDataObj.ELH_REMARKS = txtRemarks.Text.Trim();
                                leaveDataObj.ELH_DATE = txtDate.Text;
                                leaveDataObj.ELH_ACTIVE = 1;
                                leaveDataObj.EmpBranch = Convert.ToInt32(drEmp["EmpBranch"]);
                                leaveDataObj.EmpBranch_Text = HttpUtility.HtmlEncode(Convert.ToString(drEmp["empBranch_Text"]));
                                leaveDataObj.empDepartment = Convert.ToInt32(drEmp["empDepartment"]);
                                leaveDataObj.empDepartment_Text = HttpUtility.HtmlEncode(Convert.ToString(drEmp["empDepartment_Text"]));
                                leaveDataObj.EPD_EMP_TYPE = Convert.ToInt32(drEmp["EPD_EMP_TYPE"]);
                                leaveDataObj.emptype_text = HttpUtility.HtmlEncode(Convert.ToString(drEmp["emptype_text"]));
                                tempList.Add(leaveDataObj);
                            }
                        }
                        else // Update
                        {
                            if (txtDetailsEmployee.Text != CommonConstants.ALL && dtResult != null && dtResult.Rows.Count > 0)
                            {
                                int elhPk = GetNullableInt(hdfCurrentELH_PK.Value).Value;
                                int rowNo = GetNullableInt(hdfCurrentROW_NO.Value).Value;
                                EmployeeLeaveMasterBO.EmployeeLeaveDetail leaveDataObj = tempList
                                    .Where(x => x.ROW_NO == rowNo && x.ELH_EMPLOYEE == elhPk)
                                    .Single();
                                int leaveType = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                                EmployeeLeaveMasterBO.EmployeeLeaveDetail existLeave = tempList.
                                    Where(x => x.ELH_EMPLOYEE == leaveDataObj.ELH_EMPLOYEE && x.ELH_LEAVE_TYPE == leaveType
                                        && x.ROW_NO != rowNo && x.ELH_DATE == date && x.IS_DELETED == 0
                                    )
                                    .FirstOrDefault();
                                if (existLeave != null)
                                {
                                    litErrorMsg.Text = string.Format("Date:{0}; Employee:{1}; Leave Type:{2}; Already exists in list",
                                            date, existLeave.ELH_EMPLOYEE_TEXT, existLeave.ELH_LEAVE_TYPE_TEXT);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" +
                                        CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }

                                leaveDataObj.ELH_LEAVE_TYPE = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                                leaveDataObj.ELH_LEAVE_TYPE_TEXT = ddlLeaveType.SelectedItem.Text;
                                leaveDataObj.ELH_LEAVE_BAL = GetNullableDecimal(txtELH_LEAVE_BAL.Text.Trim()).Value;
                                leaveDataObj.ELH_REMARKS = txtRemarks.Text.Trim();
                                leaveDataObj.ELH_DATE = txtDate.Text;
                                leaveDataObj.ELH_EMPLOYEE = elhPk;
                                leaveDataObj.ELH_EMPLOYEE_TEXT = txtDetailsEmployee.Text;

                                leaveDataObj.EmpBranch = Convert.ToInt32(dtResult.Rows[0]["EmpBranch"]);
                                leaveDataObj.EmpBranch_Text = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empBranch_Text"]));
                                leaveDataObj.empDepartment = Convert.ToInt32(dtResult.Rows[0]["empDepartment"]);
                                leaveDataObj.empDepartment_Text = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empDepartment_Text"]));
                                leaveDataObj.EPD_EMP_TYPE = Convert.ToInt32(dtResult.Rows[0]["EPD_EMP_TYPE"]);
                                leaveDataObj.emptype_text = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["emptype_text"]));
                            }
                        }
                        EmployeeLeaveDetailList = tempList;
                        EmployeeLeaveMasterViewState.EmployeeLeaveDetail = tempList;

                        ResetForm(ControlsEnum.ADDTOLIST);
                        SetFieldValues(ControlsEnum.LEAVECREDITLIST);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        EmployeeLeaveMasterViewState = (EmployeeLeaveMasterBO.EmployeeLeaveMaster)SetUIValuesToObject(ControlsEnum.LEAVEMASTER);
                        if (EmployeeLeaveMasterViewState.EmployeeLeaveDetail.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(EmployeeLeaveMasterViewState);
                        result = BusinessLogic.HRMS.Admin.Masters.EmployeeLeaveMasterBL.SaveEmployeeLeaveMaster(xmlDoc.InnerXml, out dtErrorList);
                        if (result > 0)
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            ResetForm(ControlsEnum.CLEARSEARCH);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString(); // Resources.ErrorMessages.Msg_SavSuccess;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveMaster);
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
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveMaster + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveMaster + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.Captions.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveMaster + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.Captions.Information + "','" + "');", true);
                            }
                            else if (result == -6)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveMaster + " " +
                                    GetGlobalResourceObject("Messages", "Err_CheckSalaryYear").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CHECKMCPRINTER)  // credited leave is greater than leave limit
                            {
                                strError = string.Empty;
                                if (dtErrorList != null && dtErrorList.Rows.Count > 0)
                                {
                                    foreach (DataRow dr in dtErrorList.Rows)
                                    {
                                        strError += "<br />" + HttpUtility.HtmlDecode(Convert.ToString(dr["EMPLOYEE_TEXT"]) + " - " + (dr["LTM_NAME"]) + " (" + (dr["BAL_LEAVE"]) + ")");
                                    }
                                }
                                litErrorMsg.Text = GetLocalResourceObject("MsgSaveConfirm").ToString() + strError;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "SaveConfirmation", "$(document).ready(function(){SaveConfirmationMsg('" + litErrorMsg.Text + "');});", true);
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
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.currPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region EDIT, DETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.DETAIL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                currPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEOHPkListPage")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            GetFieldValues(ControlsEnum.LEAVETYPES);
                            SetFieldValues(ControlsEnum.LEAVETYPES);
                            txtDate.Text = string.Empty;
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.LEAVECREDITLIST);
                            txtFromDate.Text = Convert.ToDateTime(EmployeeLeaveMasterViewState.EOH_DATE_FROM.ToString()).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtToDate.Text = Convert.ToDateTime(EmployeeLeaveMasterViewState.EOH_DATE_TO.ToString()).ToString(Resources.Constants.HRMSDateFormatShort);
                            txtFromDate.Focus();
                            txtDescription.Text = EmployeeLeaveMasterViewState.EOH_DESC;
                            currPK = EmployeeLeaveMasterViewState.EOH_PK;
                            LastModifiedTime = EmployeeLeaveMasterViewState.LAST_MOD_DT;
                            EmployeeLeaveDetailList = EmployeeLeaveMasterViewState.EmployeeLeaveDetail;
                            EmployeeLeaveMasterViewState.EmployeeLeaveDetail = EmployeeLeaveMasterViewState.EmployeeLeaveDetail;
                            SetFieldValues(ControlsEnum.LEAVECREDITLIST);
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
                        EmployeeLeaveMasterViewState = new EmployeeLeaveMasterBO.EmployeeLeaveMaster();
                        EmployeeLeaveDetailList = null;
                        ResetForm(ControlsEnum.CLEAR);
                        txtDetailsEmployee.Text = string.Empty;
                        hdfDetailsEmployee.Value = string.Empty;
                        GetFieldValues(ControlsEnum.LEAVETYPES);
                        SetFieldValues(ControlsEnum.LEAVETYPES);
                        BindGrid(ControlsEnum.LEAVECREDITLIST);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Admin.Masters.EmployeeLeaveMasterBL.DeleteEmployeeLeaveMaster(EmployeeLeaveMasterViewState.EOH_PK, EmployeeLeaveMasterViewState.LAST_MOD_DT.ToString());
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveMaster);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ActionHandler(lnkList, EventArgs.Empty);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveMaster;
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
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveMaster + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveMaster + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.EmployeeLeaveMaster + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.EmployeeLeaveMaster);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.currPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);

                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.currPK = 0;

                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region EMPCLEAR
                    case ActionsEnum.EMPCLEAR:
                        ResetForm(ControlsEnum.EMPCLEAR);
                        SetFieldValues(ControlsEnum.LEAVECREDITLIST);
                        break;
                    #endregion
                    #region EMPSEARCH
                    case ActionsEnum.EMPSEARCH:                       
                        SetFieldValues(ControlsEnum.LEAVECREDITLIST);
                        break;
                    #endregion
                    #region CHANGEEMPLOYEE
                    case ActionsEnum.CHANGEEMPLOYEE:
                        ResetForm(ControlsEnum.EMPLOYEECHANGE);
                        GetFieldValues(ControlsEnum.LEAVETYPES);
                        SetFieldValues(ControlsEnum.LEAVETYPES);
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
            int curLeaveDataID;
            if (senderGridView.ID == "grdLeaveList")
            {
                if (e.CommandName == "EDIT_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfELH_EMPLOYEE = row.FindControl("hdfELH_EMPLOYEE") as HiddenField;
                    HiddenField hdfROW_NO = row.FindControl("hdfROW_NO") as HiddenField;
                    hdfCurrentROW_NO.Value = hdfROW_NO.Value;
                    hdfCurrentELH_PK.Value = hdfELH_EMPLOYEE.Value;
                    curLeaveDataID = GetNullableInt(hdfELH_EMPLOYEE.Value).Value;
                    rowNo = GetNullableInt(hdfCurrentROW_NO.Value).Value;
                    EmployeeLeaveMasterBO.EmployeeLeaveDetail leaveData = EmployeeLeaveMasterViewState.EmployeeLeaveDetail
                        .Where(x => x.ELH_EMPLOYEE == curLeaveDataID && x.ROW_NO == rowNo && x.IS_DELETED == 0)
                        .Single();
                    if (leaveData != null)
                    {
                        hdfDetailsEmployee.Value = Convert.ToString(leaveData.ELH_EMPLOYEE);
                        txtDetailsEmployee.Text = HttpUtility.HtmlDecode(leaveData.ELH_EMPLOYEE_TEXT);
                        txtDate.Text = Convert.ToDateTime(leaveData.ELH_DATE.ToString()).ToString(Resources.Constants.HRMSDateFormatShort);
                        dummyPK = Convert.ToString(leaveData.ELH_LEAVE_TYPE);
                        GetFieldValues(ControlsEnum.LEAVETYPES);
                        SetFieldValues(ControlsEnum.LEAVETYPES);
                        ddlLeaveType.SelectedIndex = ddlLeaveType.Items.IndexOf(ddlLeaveType.Items.FindByValue(Convert.ToString(leaveData.ELH_LEAVE_TYPE)));
                        txtELH_LEAVE_BAL.Text = GetFormattedNumber(leaveData.ELH_LEAVE_BAL);
                        txtRemarks.Text = leaveData.ELH_REMARKS;

                        EmpTypePk = leaveData.EPD_EMP_TYPE;
                        GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                        SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                        ddlEmployeeType.SelectedValue = leaveData.EPD_EMP_TYPE.ToString();
                        txtBranchLocation.Text = HttpUtility.HtmlDecode(leaveData.EmpBranch_Text);
                        hdfBranchLocation.Value = leaveData.EmpBranch.ToString();                        
                        txtDepartment.Text = HttpUtility.HtmlDecode(leaveData.empDepartment_Text);
                        hdfDepartment.Value = leaveData.empDepartment.ToString();
                        //hdfDetailsEmployee.Value = Convert.ToString(leaveData.empDepartment);
                        //txtDepartment.Text = Convert.ToString(leaveData.empDepartment_Text);
                    }
                }
                else if (e.CommandName == "DELETE_ACTION")
                {
                    GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                    HiddenField hdfELH_EMPLOYEE = row.FindControl("hdfELH_EMPLOYEE") as HiddenField;
                    HiddenField hdfROW_NO = row.FindControl("hdfROW_NO") as HiddenField;
                    rowNo = GetNullableInt(hdfROW_NO.Value).Value;
                    curLeaveDataID = GetNullableInt(hdfELH_EMPLOYEE.Value).Value;
                    List<EmployeeLeaveMasterBO.EmployeeLeaveDetail> tempList = EmployeeLeaveMasterViewState.EmployeeLeaveDetail;
                    EmployeeLeaveMasterBO.EmployeeLeaveDetail leaveData = tempList
                        .Where(x => x.ELH_EMPLOYEE == Convert.ToInt32(hdfELH_EMPLOYEE.Value) && x.ROW_NO == rowNo
                       && x.IS_DELETED == 0).Single();
                    leaveData.IS_DELETED = 1;
                    EmployeeLeaveMasterViewState.EmployeeLeaveDetail = tempList;
                    EmployeeLeaveDetailList = EmployeeLeaveMasterViewState.EmployeeLeaveDetail;
                    ResetForm(ControlsEnum.AFTERDELETE);
                    SetFieldValues(ControlsEnum.LEAVECREDITLIST);
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
                        if (string.IsNullOrEmpty(txtSearchEmployee.Text.Trim()) || txtSearchEmployee.Text.Equals(GetGlobalResourceObject("ErpRes", "AutoDefaultValue").ToString()))
                            hdfSearchEmployee.Value = string.Empty;
                        gridParam.SearchValue = hdfSearchEmployee.Value;
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        dsPageData = BusinessLogic.HRMS.Admin.Masters.EmployeeLeaveMasterBL.GetEmployeeLeaveListListingPage(gridParam, currentUser.SBUID, txtFilterFromDate.Text.ToString(), txtFilterToDate.Text.ToString(), hdfSearchEmployee.Value);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
                        break;
                    #endregion
                    #region EMPLOYEES
                    case ControlsEnum.EMPLOYEES:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDetailListHeader(0, GTIService.Constants.Configurations.Employees.Fields.NAME, 1);
                        break;
                    #endregion
                    #region LEAVETYPES
                    case ControlsEnum.LEAVETYPES:
                        int empPk = 0;
                        int.TryParse(hdfDetailsEmployee.Value, out empPk);
                        dtResult = BusinessLogic.HRMS.Admin.Masters.LeaveTypeMasterBL.GetLeaveTypeDDL(Convert.ToInt32(dummyPK == string.Empty ? "0" : dummyPK), string.Empty, string.Empty, 1,
                            currentUser.SBUID, 0, 0, -1, GTIService.Constants.Configurations.Employees.Fields.LTM_CODE, 1, empPk);
                        break;
                    #endregion
                    #region LEAVECREDITLIST
                    case ControlsEnum.LEAVECREDITLIST:
                        string xmlData = BusinessLogic.HRMS.Admin.Masters.EmployeeLeaveMasterBL.GetEmployeeLeaveList(GetNullableInt(Convert.ToString(currPK)).Value, currentUser.SBUID);
                        EmployeeLeaveMasterBO.EmployeeLeaveMaster tempEmployeeLeaveMaster;
                        if (xmlData == "<Root/>")
                        {
                            tempEmployeeLeaveMaster = new EmployeeLeaveMasterBO.EmployeeLeaveMaster();
                        }
                        else
                        {
                            tempEmployeeLeaveMaster = CommonFunctions.XmlDeserialize<EmployeeLeaveMasterBO.EmployeeLeaveMaster>(xmlData);
                        }
                        EmployeeLeaveMasterViewState = tempEmployeeLeaveMaster;
                        break;
                    #endregion
                    #region CURRENTCREDIT
                    case ControlsEnum.CURRENTCREDIT:
                        //23  currentELH_LEAVE_BAL = BusinessLogic.HRMS.Admin.Masters.EmployeeLeaveMasterBL.GetCurrentLeaveCredit(
                        //23 GetNullableInt(ddlEmployee.SelectedValue).Value, GetNullableInt(ddlLeaveType.SelectedValue).Value, txtDate.Text.Trim());
                        currentELH_LEAVE_BAL = BusinessLogic.HRMS.Admin.Masters.EmployeeLeaveMasterBL.GetCurrentLeaveCredit(
                      GetNullableInt(hdfDetailsEmployee.Value).Value, GetNullableInt(ddlLeaveType.SelectedValue).Value, txtDate.Text.Trim());
                        break;
                    #endregion
                    #region BRANCH / LOCATION
                    case ControlsEnum.BRANCH:
                        //dtResult = BusinessLogic.HRMS.Payroll.LoansAndAdvancesBL.GetBranchLocation();
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeeTypeGetKV(EmpTypePk, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
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
                    #region EMPLOYEES
                    case ControlsEnum.EMPLOYEES:
                        BindDropDown(ControlsEnum.EMPLOYEES);
                        break;
                    #endregion
                    #region LEAVETYPES
                    case ControlsEnum.LEAVETYPES:
                        GetUIValuesFromObject(ControlsEnum.LEAVETYPES);
                        break;
                    #endregion
                    #region LEAVECREDITLIST
                    case ControlsEnum.LEAVECREDITLIST:
                        GetUIValuesFromObject(ControlsEnum.LEAVECREDITLIST);
                        break;
                    #endregion
                    #region CURRENTCREDITBYLEAVETYPE
                    case ControlsEnum.CURRENTCREDITBYLEAVETYPE:
                        GetUIValuesFromObject(ControlsEnum.CURRENTCREDITBYLEAVETYPE);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CURRENTCREDIT
                    case ControlsEnum.CURRENTCREDIT:
                        GetUIValuesFromObject(ControlsEnum.CURRENTCREDIT);
                        break;
                    #endregion
                    #region BRANCH / LOCATION
                    case ControlsEnum.BRANCH:
                        BindDropDown(ControlsEnum.BRANCH);
                        break;
                    #endregion
                    case ControlsEnum.EMPLOYEETYPE:
                        BindDropDown(ControlsEnum.EMPLOYEETYPE);
                        break;
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
                    //ddlEmployee.Items.Insert(0, new ListItem("ALL", CommonConstants.SELECT_ALL_VAL));
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
                #region BRANCH / LOCATION
                case ControlsEnum.BRANCH:
                    //ddlBranchLocation.Items.Clear();
                    //if (dtResult != null && dtResult.Rows.Count > 0)
                    //{
                    //    ddlBranchLocation.DataSource = dtResult;
                    //    ddlBranchLocation.DataTextField = GTIService.Constants.HRMS.Employee.Fields.HRM_CON_TEXT;
                    //    ddlBranchLocation.DataValueField = GTIService.Constants.HRMS.Employee.Fields.ADM_CNST_VALUE;
                    //    ddlBranchLocation.DataBind();
                    //}
                    //ddlBranchLocation.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region EMPLOYEE TYPE
                case ControlsEnum.EMPLOYEETYPE:
                    ddlEmployeeType.Items.Clear();
                    ddlEmployeeType.DataSource = dtResult;
                    ddlEmployeeType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                    ddlEmployeeType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                    ddlEmployeeType.DataBind();
                    ddlEmployeeType.Items.HtmlDecode();
                    ddlEmployeeType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

                    ddlDetSearchEmployeeType.Items.Clear();
                    ddlDetSearchEmployeeType.DataSource = dtResult;
                    ddlDetSearchEmployeeType.DataTextField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_NAME;
                    ddlDetSearchEmployeeType.DataValueField = GTIService.Constants.HRMS.Admin.Masters.Fields.EMT_PK;
                    ddlDetSearchEmployeeType.DataBind();
                    ddlDetSearchEmployeeType.Items.HtmlDecode();
                    ddlDetSearchEmployeeType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));

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
                    #region LEAVECREDITLIST
                    case ControlsEnum.LEAVECREDITLIST:
                        if (EmployeeLeaveMasterViewState.EmployeeLeaveDetail != null && EmployeeLeaveMasterViewState.EmployeeLeaveDetail.Count > 0)
                        {
                            //PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                            //uclEmpDetListPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                            int PagNumber = uclEmpDetListPaging.CurrentPage == 0 ? 1 : uclEmpDetListPaging.CurrentPage;
                            int PagSize = Convert.ToInt32(GetLocalResourceObject("PageSizeEmpDetList"));

                            int SearchEmpPk = 0;
                            int.TryParse(hdfDetSerachEmployee.Value, out SearchEmpPk);
                            int SearchDeptPk = 0;
                            int.TryParse(hdfDetSearchDepartment.Value, out SearchDeptPk);
                            int SearchBranchPk = 0;
                            int.TryParse(hdfDetSearchBranchLocation.Value, out SearchBranchPk);

                            List<EmployeeLeaveMasterBO.EmployeeLeaveDetail> EmpLeaveDetail = EmployeeLeaveMasterViewState.EmployeeLeaveDetail.Where(x =>
                                                                                                x.IS_DELETED == 0
                                                                                                && (SearchEmpPk > 0 ? x.ELH_EMPLOYEE == SearchEmpPk : true)
                                                                                                && (SearchDeptPk > 0 ? x.empDepartment == SearchDeptPk : true)
                                                                                                && (SearchBranchPk > 0 ? x.EmpBranch == SearchBranchPk : true)
                                                                                                ).ToList();
                            int recCount = EmpLeaveDetail.Count();
                            uclEmpDetListPaging.TotalPages = recCount == 0 ? 1 : (recCount <= PagSize) ? 1 :
                                      (recCount % PagSize) == 0 ? (recCount / PagSize) :
                                      (recCount / PagSize) + 1;

                            //grdLeaveList.DataSource = EmployeeLeaveMasterViewState.EmployeeLeaveDetail
                            //    .Where(x => x.IS_DELETED == 0);   
                            grdLeaveList.DataSource = GetPagedList(EmpLeaveDetail, PagNumber, PagSize);
                        }
                        else
                        {
                            grdLeaveList.DataSource = null;
                            uclEmpDetListPaging.TotalPages = 0;
                        }
                        PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                        uclEmpDetListPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                        grdLeaveList.DataBind();
                        uclEmpDetListPaging.Visible = true;
                        uclEmpDetListPaging.BindPager();
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

        private object GetPagedList(List<EmployeeLeaveMasterBO.EmployeeLeaveDetail> EmpLeaveDetail, int PagNumber, int PagSize)
        {

            int skipCount = ((PagNumber - 1) < 0) ? 0 : (PagNumber - 1) * PagSize;
            return EmpLeaveDetail.Skip(skipCount).Take(PagSize).ToList();
        }
        #endregion

        #region "GetUIValuesFromObject"
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                int leaveTypeID;
                switch (controlType)
                {
                    #region LEAVETYPES
                    case ControlsEnum.LEAVETYPES:
                        BindDropDown(ControlsEnum.LEAVETYPES);
                        break;
                    #endregion
                    #region CURRENTCREDITBYLEAVETYPE
                    case ControlsEnum.CURRENTCREDITBYLEAVETYPE:
                        leaveTypeID = GetNullableInt(ddlLeaveType.SelectedValue).Value;
                        int empPK = 0;
                        int.TryParse(hdfDetailsEmployee.Value, out empPK);
                        dtResult = BusinessLogic.HRMS.Admin.Masters.EmployeeLeaveMasterBL.GetLeaveCredt(leaveTypeID, (empPK > 0 ? empPK : (int?)null), (int)DbActiveStatus.ACTIVE, currentUser.CurrentSBUPK);
                        // dtResult = BusinessLogic.HRMS.Admin.Masters.LeaveTypeMasterBL.GetLeaveType(leaveTypeID, string.Empty, string.Empty, 2, currentUser.SBUID, 0, 0, -1, GTIService.Constants.Configurations.Employees.Fields.LTM_CODE);
                        txtELH_LEAVE_BAL.Text = string.Empty;
                        foreach (DataRow dtRow in dtResult.Rows)
                        {
                            txtELH_LEAVE_BAL.Text = dtRow[GTIService.Constants.HRMS.Admin.Masters.Fields.ELV_LIMIT].ToString();
                        }
                        break;
                    #endregion
                    #region LEAVECREDITLIST
                    case ControlsEnum.LEAVECREDITLIST:
                        BindGrid(ControlsEnum.LEAVECREDITLIST);
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
                    List<EmployeeLeaveMasterBO.EmployeeLeaveDetail> tempDetails = EmployeeLeaveMasterViewState.EmployeeLeaveDetail;
                    if (tempDetails == null) tempDetails = new List<EmployeeLeaveMasterBO.EmployeeLeaveDetail>();
                    EmployeeLeaveMasterBO.EmployeeLeaveMaster tempEmployeeLeaveMaster = EmployeeLeaveMasterViewState;
                    tempEmployeeLeaveMaster.EOH_PK =  tempEmployeeLeaveMaster.EOH_PK;
                    tempEmployeeLeaveMaster.EOH_DESC = tempEmployeeLeaveMaster.EOH_DESC;
                    tempEmployeeLeaveMaster.EOH_DATE_FROM = txtFromDate.Text.ToString();
                    tempEmployeeLeaveMaster.EOH_DATE_TO = txtToDate.Text.ToString();
                    tempEmployeeLeaveMaster.EOH_DESC = txtDescription.Text.ToString();
                    tempEmployeeLeaveMaster.EOH_DEPT = currentUser.CurrentDeptPK;
                    GetFieldValues(ControlsEnum.COMPANY);
                    tempEmployeeLeaveMaster.EOH_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    tempEmployeeLeaveMaster.EOH_BIZUNIT = currentUser.SBUID;
                    tempEmployeeLeaveMaster.EOH_LEAVE_TYPE = 1;
                    tempEmployeeLeaveMaster.EOH_ACTIVE = tempEmployeeLeaveMaster.EOH_ACTIVE;
                    tempEmployeeLeaveMaster.LAST_MOD_DT = LastModifiedTime;

                    foreach (var EL in tempDetails)
                    {
                        EL.ELH_DEPT = currentUser.CurrentDeptPK;
                        EL.ELH_BIZUNIT = currentUser.SBUID;
                        EL.ELH_COMPANY = tempEmployeeLeaveMaster.EOH_COMPANY;
                        EL.LAST_MOD_DT = LastModifiedTime;
                    }
                    tempEmployeeLeaveMaster.EmployeeLeaveDetail = tempDetails;
                    tempEmployeeLeaveMaster.EOH_CREDIT = 0;
                    tempEmployeeLeaveMaster.USER_PK = currentUser.PKUser;
                    tempEmployeeLeaveMaster.IS_CHECK = Convert.ToInt32(hdfIsSaveYes.Value);
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
                    hdfCurrentELH_PK.Value = hdfCurrentROW_NO.Value = string.Empty;
                    txtFromDate.Focus();
                    ddlLeaveType.SelectedIndex = 0;
                    txtBranchLocation.Text = CommonConstants.ALL;
                    hdfBranchLocation.Value = string.Empty;
                    ddlEmployeeType.ClearSelection();
                    txtDate.Text = string.Empty;
                    txtELH_LEAVE_BAL.Text = txtRemarks.Text = string.Empty;
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    txtDescription.Text = string.Empty;
                    currPK = 0;
                    txtDetailsEmployee.Text = CommonConstants.ALL;
                    hdfDetailsEmployee.Value = string.Empty;
                    txtDepartment.Text = CommonConstants.ALL;
                    hdfDepartment.Value = string.Empty;
                    hdfIsSaveYes.Value = CommonConstants.SELECT_VALUE_ZERO;
                    ResetForm(ControlsEnum.EMPCLEAR);
                    break;
                #endregion
                #region ADDTOLIST
                case ControlsEnum.ADDTOLIST:
                    hdfCurrentELH_PK.Value = hdfCurrentROW_NO.Value = txtELH_LEAVE_BAL.Text = txtRemarks.Text = string.Empty;
                    ddlLeaveType.ClearSelection();
                    break;
                #endregion
                #region AFTERDELETE
                case ControlsEnum.AFTERDELETE:
                    hdfCurrentELH_PK.Value = hdfCurrentROW_NO.Value = string.Empty;
                    txtELH_LEAVE_BAL.Text = txtRemarks.Text = string.Empty;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtSearchEmployee.Text = hdfSearchEmployee.Value = string.Empty;
                    txtFilterFromDate.Text = string.Empty;
                    txtFilterToDate.Text = string.Empty;
                    break;
                #endregion
                #region EMPCLEAR
                case ControlsEnum.EMPCLEAR:
                    uclEmpDetListPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    txtDetSearchBranchLocation.Text = CommonConstants.ALL;
                    hdfDetSearchBranchLocation.Value = string.Empty;
                    ddlDetSearchEmployeeType.ClearSelection();
                    txtDetSearchDepartment.Text = CommonConstants.ALL;
                    hdfDetSearchDepartment.Value = string.Empty;
                    txtDetSerachEmployee.Text = CommonConstants.ALL;
                    hdfDetSerachEmployee.Value = string.Empty;
                    break;
                #endregion
                #region EMPLOYEECHANGE
                case ControlsEnum.EMPLOYEECHANGE:
                    txtELH_LEAVE_BAL.Text=string.Empty;
                    ddlLeaveType.ClearSelection();
                    break;
                #endregion
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
            else if (pagerId == "uclEmpDetListPaging")
            {
                // Should we disable the first link
                uclEmpDetListPaging.FirstButtonEnabled = (uclEmpDetListPaging.CurrentPage == 1) ? false : true;
                // Should we disable the previous link
                uclEmpDetListPaging.PreviousButtonEnabled = (uclEmpDetListPaging.CurrentPage == 1) ? false : true;
                // Should we enable the next link
                uclEmpDetListPaging.NextButtonEnabled = (uclEmpDetListPaging.CurrentPage < iTotalPages) ? true : false;
                // Should we enable the last link
                uclEmpDetListPaging.LastButtonEnabled = (uclEmpDetListPaging.CurrentPage < iTotalPages) ? true : false;
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
            string format = GetLocalResourceObject("LeaveNumberFormat").ToString();
            string s = num.ToString(format);
            return s;
        }
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEES,
            LEAVETYPES,
            LEAVECREDITLIST,
            LEAVEMASTER,
            CURRENTCREDIT,
            CURRENTCREDITBYLEAVETYPE,
            AFTERDELETE,
            COMPANY,
            CLEARSEARCH,
            CLEAR,
            LIST,
            ADDTOLIST,
            BRANCH,
            EMPLOYEETYPE,
            EMPCLEAR,
            EMPSEARCH,
            EMPLOYEECHANGE
        }
        #endregion
    }
}