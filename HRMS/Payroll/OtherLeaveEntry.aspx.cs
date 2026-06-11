using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using BusinessObject.HRMS.Payroll;
using System.Xml;
using BusinessObject.CommonManagement;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.AccountManagement;
using BusinessLogic.HRMS.Payroll;
using ERP.Utilities.HRMS;
using ERPSMS_v01;
using BusinessLogic.CommonManagement;
using System.Configuration;
using System.IO;
using System.Data.OleDb;

namespace HRMS.Payroll
{
    public partial class OtherLeaveEntry : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
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
        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
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
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }
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
        private List<OvertimeCalculatorBO.MonthlyOTData> OTDataList
        {
            get
            {
                return ViewState["OTDataList"] == null ? new List<OvertimeCalculatorBO.MonthlyOTData>() : (List<OvertimeCalculatorBO.MonthlyOTData>)ViewState["OTDataList"];
            }
            set
            {
                ViewState["OTDataList"] = value;
            }
        }
        private int CurrPK
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrPK] = value;
            }
        }
        //private int LeaveType
        //{
        //    get
        //    {
        //        return ViewState["LeaveType"] == null ? 0 : (int)ViewState["LeaveType"];
        //    }
        //    set
        //    {
        //        ViewState["LeaveType"] = value;
        //    }
        //}
        //private int LeaveDay
        //{
        //    get
        //    {
        //        return ViewState["LeaveDay"] == null ? 0 : (int)ViewState["LeaveDay"];
        //    }
        //    set
        //    {
        //        ViewState["LeaveDay"] = value;
        //    }
        //}
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
        #endregion
        private BusinessObject.User currentUser;
        private OvertimeCalculatorBO.MonthlyOTMaster objOTMaster;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataSet dsPageData;
        private int EmployeePk;
        private int CompanyPk = 0;
        private string uploadPath;
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch();", true);
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
                    PageIndex = uclPaging.CurrentPage;
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
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    hdfAdvSearch.Value = "0";
                    ConfigurationSettings();
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                    SetFieldValues(ControlsEnum.EMPLOYMENTTYPE);
                    GetFieldValues(ControlsEnum.LEAVETYPES);
                    SetFieldValues(ControlsEnum.LEAVETYPES);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    SetFieldValues(ControlsEnum.EMPLOYEETYPE);
                    GetFieldValues(ControlsEnum.SPECIALHOLIDAYTYPE);
                    SetFieldValues(ControlsEnum.SPECIALHOLIDAYTYPE);
                    ddlQuarter.Enabled = false;
                    txtMonth.Enabled = false;
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
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            int? result = null;
            XmlDocument xmlDoc;
            List<OvertimeCalculatorBO.MonthlyOTData> tempList = OTDataList;
            bool bIsChecked = false;
            GridViewRow grvRow;
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
                //else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                //{
                //    if ((((DropDownList)sender).ID == "ddlEmployee"))
                //    {
                //        commonActions = ActionsEnum.CHANGE;
                //    }
                //    //if ((((DropDownList)sender).ID == "ddlLeaveType"))
                //    //{
                //    //    commonActions = ActionsEnum.CHANGETYPE;
                //    //}
                //}
                //else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                //{
                //    if ((((TextBox)sender).ID == "txtDate"))
                //    {
                //        commonActions = ActionsEnum.CHANGE;
                //        txtEmployee.Enabled = true;
                //    }
                //}
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    commonActions = ActionsEnum.CHANGE;
                }
                #endregion
                switch (commonActions)
                {
                    #region CLEARADD
                    case ActionsEnum.CLEARADD:
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #region Add
                    case ActionsEnum.ADD:
                        if (OTDataList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SelectEmployee").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        else if (chkMonthly.Checked == false && chkYearly.Checked == false&&chkSpecilHoliday.Checked==false)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_LeaveType").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        else if (chkMonthly.Checked == true && txtMonth.Text == string.Empty)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SelectMonth").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            //txtMonth.Focus();
                            return;
                        }
                        else if (chkSpecilHoliday.Checked == true && ddlQuarter.SelectedIndex == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SelectQuarter").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            ddlQuarter.Focus();
                            return;
                        }
                        else if (chkSun.Checked == false && chkMon.Checked == false && chkTue.Checked == false && chkWed.Checked == false && chkThu.Checked == false && chkFri.Checked == false && chkSat.Checked == false)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_SelectDay").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }

                        else
                        {
                            OTDataList.ToList().ForEach(u =>
                            {
                                u.LeaveType = chkMonthly.Checked == true ? "Monthly" : (chkYearly.Checked == true ? "Yearly" : "");
                                u.SpecialHoliday = chkSpecilHoliday.Checked == true ? "Yes" : "No";
                            });
                            BindGrid(ControlsEnum.EMPLOYEEOTDETAILS);
                        }

                        break;
                    #endregion
                    #region IMPORT
                    case ActionsEnum.IMPORT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (fupImport.HasFile)
                            {
                                //string conStr;
                                //string filePath = SaveDetails(out conStr, fupImport);
                                //if (!string.IsNullOrEmpty(filePath))
                                //{
                                //    ImportToGrid(filePath, conStr);
                                //}
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_InvalidFile").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }

                        }
                        break;
                    #endregion                    
                    #region change
                    case ActionsEnum.CHANGE:
                        if (((CheckBox)sender).ID == "chkYearly")
                        {
                            EnableDisableControls(ControlsEnum.YEARLY);
                        }
                        else if (((CheckBox)sender).ID == "chkMonthly")
                        {
                            EnableDisableControls(ControlsEnum.MONTHLY);

                        }
                        else if (((CheckBox)sender).ID == "chkSpecilHoliday")
                        {
                            EnableDisableControls(ControlsEnum.SPECIALHOLIDAY);
                        }
                        else if (((CheckBox)sender).ID == "chkSun")
                        { CheckUncheckControls(1); }
                        else if (((CheckBox)sender).ID == "chkMon")
                        { CheckUncheckControls(2); }
                        else if (((CheckBox)sender).ID == "chkTue")
                        { CheckUncheckControls(3); }
                        else if (((CheckBox)sender).ID == "chkWed")
                        { CheckUncheckControls(4); }
                        else if (((CheckBox)sender).ID == "chkThu")
                        { CheckUncheckControls(5); }
                        else if (((CheckBox)sender).ID == "chkFri")
                        { CheckUncheckControls(6); }
                        else if (((CheckBox)sender).ID == "chkSat")
                        { CheckUncheckControls(7); }
                        if (((CheckBox)sender).ID == "chkYearly" || ((CheckBox)sender).ID == "chkMonthly" || ((CheckBox)sender).ID == "chkSpecilHoliday")
                        {
                            OTDataList.ToList().ForEach(u =>
                            {
                                u.LeaveType = null;
                                u.SpecialHoliday = null;
                            });
                            BindGrid(ControlsEnum.EMPLOYEEOTDETAILS);
                        }
                        break;
                    #endregion
                    #region Go
                    case ActionsEnum.GO:
                        GetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
                        SetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
                        break;
                    #endregion
                    #region LIST,CANCEL
                    case ActionsEnum.LIST:
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        OvertimeCalculatorBO.MonthlyOTMaster OtherLeaveEntry = (OvertimeCalculatorBO.MonthlyOTMaster)SetUIValuesToObject(ControlsEnum.SAVE);
                        if (OtherLeaveEntry.MonthlyOTDatas.Count == 0)
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordsForSave")).ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.Captions.Information + "');", true);
                            return;
                        }
                        foreach (var oled in OtherLeaveEntry.MonthlyOTDatas)
                        {
                            if (oled.SpecialHoliday == null)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + this.GetLocalResourceObject("Err_LeaveDetails") + "','" + Resources.ErpRes.Information + "');", true);
                                return;

                            }
                        }
                            string EmpName = string.Empty;
                        string TrxNo = string.Empty;
                        OtherLeaveEntry.WKF_FLAG = 1;
                        xmlDoc = CommonFunctions.ObjectTOXmlForPrdPlan(OtherLeaveEntry);
                        result = OvertimeCalculatorBL.SaveOtherLeaveEntry(xmlDoc.InnerXml, out EmpName, out TrxNo);
                        if (result > 0)
                        {
                            lblTrxNo.Text = TrxNo;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                            object[] args = new object[2];
                            args[0] = Resources.PageNameRes.OtherLeaveEntry;
                            args[1] = TrxNo;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            this.EntryStatus = EntryStatus.LISTMODE;
                            //litErrorMsg.Text = GetLocalResourceObject("Msg_SavSuccess").ToString();
                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OvertimeCalculator);
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
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("AlreadyAdded").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Modify_Conformation").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);

                            }
                            else if (result == (int)DbSaveStatus.DATEOVERLAP)//-4
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_MonthlyEntryExsists").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)//-5
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_YearlyEntryExsists").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
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
                    #region DETAIL,EDIT
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                ResetForm(ControlsEnum.CLEAR);
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEOLPKList")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            txtDate.Focus();
                            hdfShowHideFilterSec.Value = "0";
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK);
                            SetFieldValues(ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETE OT  DETAILS
                    case ActionsEnum.GRIDDELETE:
                        grvRow = (GridViewRow)((sender as ImageButton).Parent.Parent);
                        HiddenField attnPk = (HiddenField)grvRow.FindControl("hdfEOT_PK");
                        OTDataList.RemoveAt(grvRow.RowIndex);
                        SetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OtherLeaveEntry);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                        //int otePk = 0;
                        //int.TryParse(attnPk.Value, out otePk);
                        //if (otePk > 0)
                        //    result = OvertimeCalculatorBL.DeleteOtherLeaveEntryDetails(null, otePk, LastModifiedTime);
                        //if (result > 0 || otePk == 0)
                        //{
                        //    OTDataList.RemoveAt(grvRow.RowIndex);
                        //    SetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
                        //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OtherLeaveEntry);
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        //        + "','" + Resources.ErpRes.Information + "');", true);
                        //}
                        //else
                        //{
                        //    if (result == (int)DbSaveStatus.REFERRED || result == (int)DbSaveStatus.REFNOEXIST)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.OvertimeCalculator + " " + Resources.Messages.UsedInAnotherPlace;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.SQLERROR)
                        //    {
                        //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.CONCURRENCY)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.OvertimeCalculator + " " + Resources.Messages.EditUsedByAnotherUser;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        //        + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //    else if (result == (int)DbSaveStatus.ALREADYDELETED)
                        //    {
                        //        litErrorMsg.Text = Resources.PageNameRes.OvertimeCalculator + " " + Resources.Messages.AlreadyDeleted;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        //        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        //    }
                        //    else
                        //    {
                        //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OvertimeCalculator);
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        //            + "','" + Resources.ErpRes.Information + "');", true);
                        //    }
                        //}
                        break;
                    #endregion
                    #region DELETE HDR
                    case ActionsEnum.DELETE:
                        result = OvertimeCalculatorBL.DeleteOtherLeaveEntryDetails(CurrPK, null, LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OtherLeaveEntry);
                            ResetForm(ControlsEnum.CLEAR);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED || result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OvertimeCalculator + " " + Resources.Messages.UsedInAnotherPlace;
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
                                litErrorMsg.Text = Resources.PageNameRes.OvertimeCalculator + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.OvertimeCalculator + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.OvertimeCalculator);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region New
                    case ActionsEnum.NEW:
                        EntryStatus = EntryStatus.NEWMODE;
                        OTDataList = new List<OvertimeCalculatorBO.MonthlyOTData>();
                        ResetForm(ControlsEnum.CLEAR);
                        EnableDisableControls(ControlsEnum.CLEAR);
                        SetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        txtDate.Text = DateTime.Now.Date.ToString(Resources.Constants.HRMSDateFormatShort);
                        txtDate.Focus();
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = 1;
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Filter Clear
                    case ActionsEnum.CLEARDETAIL:
                        ResetForm(ControlsEnum.CLEARDETAIL);
                        if (CurrPK > 0)
                        {
                            GetFieldValues(ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK);
                            if (objOTMaster != null)
                                OTDataList = objOTMaster.MonthlyOTDatas;
                        }
                        //GetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
                        SetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
                        break;
                    #endregion
                    #region List SEARCH Clear
                    case ActionsEnum.CLEARSEARCH:
                        this.EntryStatus = EntryStatus.ENTRYMODE;
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
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
        //protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        //{
        //    GridView senderGridView = (GridView)sender;
        //    int rowNo;
        //    int employeeLeaveDataID;
        //    if (senderGridView.ID == "grdOTList")
        //    {
        //         if (e.CommandName == "DELETE_ACTION")
        //        {
        //            GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
        //            HiddenField hdfEOT_PK = row.FindControl("hdfEOT_PK") as HiddenField;
        //            HiddenField hdfROW_NO = row.FindControl("hdfROW_NO") as HiddenField;
        //            hdfCurrentROW_NO.Value = hdfROW_NO.Value;
        //            hdfCurrentELD_PK.Value = hdfEOT_PK.Value;
        //            employeeLeaveDataID = GetNullableInt(hdfEOT_PK.Value).Value;
        //            rowNo = GetNullableInt(hdfCurrentROW_NO.Value).Value;
        //            OvertimeCalculatorBO.MonthlyOTData otData = OTDataList
        //                 .Where(x => x.EOT_PK == employeeLeaveDataID && x.ROW_NO == rowNo)
        //                 .Single();

        //            List<OvertimeCalculatorBO.MonthlyOTData> tempList = OTDataList;
        //            if (employeeLeaveDataID == 0) tempList.Remove(otData);
        //            else otData.IS_DELETED = 1;

        //            OTDataList = tempList;
        //            ResetForm(ControlsEnum.AFTERDELETE);
        //            SetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
        //        }
        //    }
        //}
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
            //  EntryStatus = EntryStatus.LISTMODE;
        }
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (((GridView)sender).ID == "grdOTList")
            {
                //if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                //{
                //    if (Convert.ToInt32(hdfOTEntry.Value) <= 1)
                //    {
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("OTHours").ToString()).SingleOrDefault()).Visible = true;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col1").ToString()).SingleOrDefault()).Visible = false;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col2").ToString()).SingleOrDefault()).Visible = false;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col3").ToString()).SingleOrDefault()).Visible = false;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col4").ToString()).SingleOrDefault()).Visible = false;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col5").ToString()).SingleOrDefault()).Visible = false;

                //        TextBox txtOTHours = (TextBox)e.Row.FindControl("txtOTHours");
                //        HiddenField hdfOTHours = (HiddenField)e.Row.FindControl("hdfOTHours");
                //        if (hdfOTHours.Value != string.Empty)
                //        {
                //            txtOTHours.Text = GetTime(hdfOTHours.Value);
                //        }
                //    }
                //    else
                //    {
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("OTHours").ToString()).SingleOrDefault()).Visible = false;

                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col1").ToString()).SingleOrDefault()).Visible = true;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col2").ToString()).SingleOrDefault()).Visible = true;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col3").ToString()).SingleOrDefault()).Visible = true;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col4").ToString()).SingleOrDefault()).Visible = true;
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col5").ToString()).SingleOrDefault()).Visible = true;

                //        //ConfigurationsRes - HrmsOTCol 
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col1").ToString()).SingleOrDefault()).Visible = Convert.ToBoolean(Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsOTCol1")));
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col2").ToString()).SingleOrDefault()).Visible = Convert.ToBoolean(Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsOTCol2")));
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col3").ToString()).SingleOrDefault()).Visible = Convert.ToBoolean(Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsOTCol3")));
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col4").ToString()).SingleOrDefault()).Visible = Convert.ToBoolean(Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsOTCol4")));
                //        ((DataControlField)grdOTList.Columns.Cast<DataControlField>().Where(fld => fld.HeaderText == GetLocalResourceObject("Col5").ToString()).SingleOrDefault()).Visible = Convert.ToBoolean(Convert.ToInt16(GetGlobalResourceObject("ConfigurationsRes", "HrmsOTCol5")));


                //        TextBox txtOTHours1 = (TextBox)e.Row.FindControl("txtOTHours1");
                //        HiddenField hdfOTHours1 = (HiddenField)e.Row.FindControl("hdfOTHours1");
                //        if (hdfOTHours1.Value != string.Empty)
                //        {
                //            txtOTHours1.Text = GetTime(hdfOTHours1.Value);
                //        }
                //        TextBox txtOTHours2 = (TextBox)e.Row.FindControl("txtOTHours2");
                //        HiddenField hdfOTHours2 = (HiddenField)e.Row.FindControl("hdfOTHours2");
                //        if (hdfOTHours2.Value != string.Empty)
                //        {
                //            txtOTHours2.Text = GetTime(hdfOTHours2.Value);
                //        }
                //        TextBox txtOTHours3 = (TextBox)e.Row.FindControl("txtOTHours3");
                //        HiddenField hdfOTHours3 = (HiddenField)e.Row.FindControl("hdfOTHours3");
                //        if (hdfOTHours3.Value != string.Empty)
                //        {
                //            txtOTHours3.Text = GetTime(hdfOTHours3.Value);
                //        }
                //        TextBox txtOTHours4 = (TextBox)e.Row.FindControl("txtOTHours4");
                //        HiddenField hdfOTHours4 = (HiddenField)e.Row.FindControl("hdfOTHours4");
                //        if (hdfOTHours4.Value != string.Empty)
                //        {
                //            txtOTHours4.Text = GetTime(hdfOTHours4.Value);
                //        }
                //        TextBox txtOTHours5 = (TextBox)e.Row.FindControl("txtOTHours5");
                //        HiddenField hdfOTHours5 = (HiddenField)e.Row.FindControl("hdfOTHours5");
                //        if (hdfOTHours5.Value != string.Empty)
                //        {
                //            txtOTHours5.Text = GetTime(hdfOTHours5.Value);
                //        }
                //    }

                //}
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
            FilterParameters objFilterParam;
            try
            {
                switch (type)
                {
                    #region special holiday type
                    case ControlsEnum.SPECIALHOLIDAYTYPE:
                        dtResult = CommonBL.GetAppConfig(currentUser.SBUID, "HRMS_Special_Holyday_Type");
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region EMPLOYMENTTYPE
                    case ControlsEnum.EMPLOYMENTTYPE:
                        int commonPK = 0;
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentType(commonPK);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        objFilterParam = new FilterParameters();
                        int EOLPK = 0;
                        int.TryParse(hdfTrxPk.Value, out EOLPK);

                        objFilterParam.PageNumber = PageIndex == 0 ? 1 : PageIndex;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtFilterFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtFilterToDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterToDate.Text);
                        objFilterParam.BizUnit = currentUser.SBUID;
                        objFilterParam.Department = Convert.ToInt32(hdfFilterDept.Value) < 0 ? (int?)null : Convert.ToInt32(hdfFilterDept.Value);
                        objFilterParam.BranchLocation = Convert.ToInt32(hdfFilterBranch.Value) < 0 ? (int?)null : Convert.ToInt32(hdfFilterBranch.Value);
                        objFilterParam.Employee = Convert.ToInt32(hdfEmployeeSearch.Value) < 0 ? (int?)null : Convert.ToInt32(hdfEmployeeSearch.Value);
                        objFilterParam.Active = (int)DbActiveStatus.ACTIVE;
                        objFilterParam.PK = EOLPK > 0 ? EOLPK : (int?)null;
                        dsPageData = OvertimeCalculatorBL.GetYearlyOtherLeaveEntryListingPage(objFilterParam);
                        break;
                    #endregion
                    #region EMPLOYEE OT DETAILS
                    case ControlsEnum.EMPLOYEEOTDETAILS:
                        objFilterParam = new FilterParameters();
                        objFilterParam.ToDate = objFilterParam.FromDate = string.IsNullOrEmpty(txtDate.Text) ? (DateTime?)null : DateTime.Parse(txtDate.Text);
                        objFilterParam.Designation = GetNullableInt(hdfDesignation.Value) > 0 ? GetNullableInt(hdfDesignation.Value) : null;
                        objFilterParam.Department= GetNullableInt(hdfHdDepartment.Value) > 0 ? GetNullableInt(hdfHdDepartment.Value) : null;
                        objFilterParam.Employee = GetNullableInt(hdfEmployee.Value) > 0 ? GetNullableInt(hdfEmployee.Value) : null;
                        objFilterParam.BranchLocation = Convert.ToInt32(hdfHdBranchLocation.Value) < 0 ? (int?)null : Convert.ToInt32(hdfHdBranchLocation.Value);
                        objFilterParam.EmployeeCategory = (int)EmployeeCategory.HRMSEmployee;
                        objFilterParam.PK = CurrPK;
                        objFilterParam.Active = Convert.ToInt32(CommonConstants.ACTIVE);
                        objFilterParam.BizUnit = currentUser.SBUID;

                        string xmlData = OvertimeCalculatorBL.GetMonthlyOtList(objFilterParam);
                        List<OvertimeCalculatorBO.MonthlyOTData> lstTempOtList;
                        if (xmlData == "<Root/>")
                        {
                            lstTempOtList = new List<OvertimeCalculatorBO.MonthlyOTData>();
                        }
                        else
                        {
                            OvertimeCalculatorBO.MonthlyOTMaster root = CommonFunctions.XmlDeserialize<OvertimeCalculatorBO.MonthlyOTMaster>(xmlData);
                            lstTempOtList = root.MonthlyOTDatas;
                        }
                        OTDataList = lstTempOtList;
                        break;
                    #endregion
                    #region GET Other Leave Entry DETAILS BY PK
                    case ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK:
                        objOTMaster = OvertimeCalculatorBL.OTHERLEAVEENTRYDETAILSBYPK(currentUser.SBUID, Convert.ToInt32(CommonConstants.ACTIVE), CurrPK);
                        if (objOTMaster == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "AlertMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region EMPLOYEE TYPE
                    case ControlsEnum.EMPLOYEETYPE:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeePayDetailsBL.GetEmployeeTypeGetKV(0, currentUser.SBUID, (int)DbActiveStatus.ACTIVE);
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
                    case ControlsEnum.SPECIALHOLIDAYTYPE:
                        BindDropDown(ControlsEnum.SPECIALHOLIDAYTYPE);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.EMPLOYMENTTYPE:
                        BindDropDown(ControlsEnum.EMPLOYMENTTYPE);
                        break;
                    #region EMPLOYEE OT DETAILS
                    case ControlsEnum.EMPLOYEEOTDETAILS:
                        BindGrid(ControlsEnum.EMPLOYEEOTDETAILS);
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(ControlsEnum.LIST);
                        break;
                    #endregion
                    case ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK:
                        GetUIValuesFromObject(ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK);
                        break;
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
        #region CheckUncheckControls()
        private void CheckUncheckControls(int day)
        {
            switch (day)
            {
                case 1://sun
                    if (chkSun.Checked)
                    {
                        chkSun.Checked = true;
                        //LeaveDay = (int)Day.SUN;
                    }
                    else
                        chkSun.Checked = false;
                    chkMon.Checked = chkTue.Checked = chkWed.Checked = chkThu.Checked = chkFri.Checked = chkSat.Checked = false;
                    break;
                case 2://mon
                    if (chkMon.Checked)
                    {
                        chkMon.Checked = true;
                        //LeaveDay = (int)Day.MON;
                    }
                    else
                        chkMon.Checked = false;
                    chkSun.Checked = chkTue.Checked = chkWed.Checked = chkThu.Checked = chkFri.Checked = chkSat.Checked = false;
                    break;
                case 3://tue
                    if (chkTue.Checked)
                    {
                        //LeaveDay = (int)Day.TUE;
                        chkTue.Checked = true;
                    }
                    else
                        chkTue.Checked = false;
                    chkSun.Checked = chkMon.Checked = chkWed.Checked = chkThu.Checked = chkFri.Checked = chkSat.Checked = false;
                    break;
                case 4://wed
                    if (chkWed.Checked)
                    {
                        chkWed.Checked = true;
                        //LeaveDay = (int)Day.WED;
                    }
                    else
                        chkWed.Checked = false;
                    chkSun.Checked = chkMon.Checked = chkTue.Checked = chkThu.Checked = chkFri.Checked = chkSat.Checked = false;
                    break;
                case 5://thu
                    if (chkThu.Checked)
                    {
                        chkThu.Checked = true;
                        //LeaveDay = (int)Day.THU;

                    }
                    else
                        chkThu.Checked = false;
                    chkSun.Checked = chkTue.Checked = chkWed.Checked = chkMon.Checked = chkFri.Checked = chkSat.Checked = false;
                    break;
                case 6://fri
                    if (chkFri.Checked)
                    {
                        chkFri.Checked = true;
                        //LeaveDay = (int)Day.FRI;

                    }
                    else
                        chkFri.Checked = false;
                    chkSun.Checked = chkMon.Checked = chkWed.Checked = chkThu.Checked = chkTue.Checked = chkSat.Checked = false;
                    break;
                case 7://sat
                    if (chkSat.Checked)
                    {
                        chkSat.Checked = true;
                        //LeaveDay = (int)Day.SAT;

                    }
                    else
                        chkSat.Checked = false;
                    chkSun.Checked = chkMon.Checked = chkWed.Checked = chkThu.Checked = chkFri.Checked = chkTue.Checked = false;
                    break;
            }
        }
        #endregion
        #region EnableDisableControls
        private void EnableDisableControls(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region new mode
                case ControlsEnum.CLEAR:
                    chkMonthly.Checked = false;
                    //chkYearly.Enabled = true;
                    chkSpecilHoliday.Checked = false;
                    txtMonth.Enabled = false;
                    ddlQuarter.Enabled = false;
                    ddlQuarter.SelectedIndex = 0;
                    btnGo.Enabled = btnAddItem.Enabled = btnClear.Enabled = btnClearDetails.Enabled = true;
                    chkMonthly.Enabled = chkYearly.Enabled = chkSpecilHoliday.Enabled = true;
                    txtHdBranchLocation.Enabled = txtHdDepartment.Enabled = txtDesignation.Enabled = txtEmployee.Enabled = txtDate.Enabled = true;

                    break;
                #endregion
                #region Yearly
                case ControlsEnum.YEARLY:
                    //LeaveType = (int)OtherLeaveType.Yearly;
                    chkMonthly.Checked = false;
                    chkSpecilHoliday.Checked = false;
                    txtMonth.Enabled = false;
                    ddlQuarter.Enabled = false;
                    ddlQuarter.SelectedIndex = 0;
                    txtMonth.Text = string.Empty;
                    //rfvMonth.Enabled= rfvQuarter.Enabled = false;
                    //rfvMonth.Visible= rfvQuarter.Visible = false;
                    break;
                #endregion
                #region Monthly
                case ControlsEnum.MONTHLY:
                    if (chkMonthly.Checked == false)
                    {
                        txtMonth.Enabled = false;
                        txtMonth.Text = string.Empty;
                        //rfvQuarter.Enabled= rfvQuarter.Visible = false;
                    }
                    else
                    {
                        txtMonth.Enabled = true;
                        //LeaveType = (int)OtherLeaveType.Monthly;
                        //rfvMonth.Enabled= rfvMonth.Visible = true;
                    }
                    chkYearly.Checked = false;
                    //chkMonthly.Enabled = true;
                    chkSpecilHoliday.Checked = false;
                    ddlQuarter.Enabled = false;
                    ddlQuarter.SelectedIndex = 0;
                    break;
                #endregion
                #region Special Holiday
                case ControlsEnum.SPECIALHOLIDAY:
                    if (chkSpecilHoliday.Checked == false)
                    {
                        ddlQuarter.SelectedIndex = 0;
                        ddlQuarter.Enabled = false;
                        //rfvMonth.Enabled= rfvMonth.Visible = false;
                    }
                    else
                    {
                        ddlQuarter.Enabled = true;
                        //LeaveType = (int)OtherLeaveType.SpecialHolidayInMonth;
                        //rfvQuarter.Enabled=rfvQuarter.Visible = true;

                    }
                    chkYearly.Checked = false;
                    chkMonthly.Checked = false;
                    txtMonth.Enabled = false;
                    txtMonth.Text = string.Empty;
                    //chkSpecilHoliday.Enabled = true;
                    break;
                #endregion
                #region edit
                case ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK:
                    btnGo.Enabled = btnAddItem.Enabled = btnClear.Enabled = btnClearDetails.Enabled = false;
                    txtMon.Enabled = ddlQuarter.Enabled = false;
                    chkMonthly.Enabled = chkYearly.Enabled = chkSpecilHoliday.Enabled = false;
                    txtHdBranchLocation.Enabled = txtHdDepartment.Enabled = txtDesignation.Enabled = txtEmployee.Enabled = txtDate.Enabled = false;
                    break;
                    #endregion

            }
        }
        #endregion
        #region BindDropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region SPECIALHOLIDAYTYPE
                case ControlsEnum.SPECIALHOLIDAYTYPE:
                    ddlQuarter.DataTextField = Resources.DataFieldRes.cfgData;
                    ddlQuarter.DataValueField = Resources.DataFieldRes.cfgValue;
                    ddlQuarter.DataSource = dtResult;
                    ddlQuarter.DataBind();
                    ddlQuarter.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion

                #region COMPANY
                case ControlsEnum.COMPANY:

                    ddlCompanyHd.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompanyHd.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompanyHd.DataSource = dtCompany;
                    ddlCompanyHd.DataBind();
                    ddlCompanyHd.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompanyHd.Items.HtmlDecode();
                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                    {
                        ddlCompanyHd.SelectedIndex = ddlCompanyHd.Items.IndexOf(ddlCompanyHd.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));

                    }
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
                    #region EMPLOYEE OT DETAILS
                    case ControlsEnum.EMPLOYEEOTDETAILS:
                        if (OTDataList != null && OTDataList.Count > 0)
                        {
                            grdOTList.DataSource = OTDataList;
                            grdOTList.DataBind();
                        }
                        else
                        {
                            grdOTList.DataSource = null;
                            grdOTList.DataBind();
                        }
                        break;
                    #endregion
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dsPageData != null && dsPageData.Tables[0].Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dsPageData;
                            grdList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdList.DataSource = null;
                            grdList.DataBind();
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
                #region SAVE
                case ControlsEnum.SAVE:
                    OvertimeCalculatorBO.MonthlyOTMaster tempMonthlyOtMaster = new OvertimeCalculatorBO.MonthlyOTMaster();
                    tempMonthlyOtMaster.EOE_PK = CurrPK;
                    tempMonthlyOtMaster.EOE_TO_DATE = txtDate.Text;
                    tempMonthlyOtMaster.EOL_SPECIAL_HOLYDAY_TYPE = Convert.ToInt16(ddlQuarter.SelectedValue) > 0 ? Convert.ToInt16(ddlQuarter.SelectedValue) : 0;
                    tempMonthlyOtMaster.EOL_OTHER_LEAVE_TYPE = chkMonthly.Checked == true ? (int)OtherLeaveType.Monthly : (chkYearly.Checked == true ? (int)OtherLeaveType.Yearly : (int)OtherLeaveType.SpecialHolidayInMonth);
                    tempMonthlyOtMaster.EOL_BRANCH_Text = txtHdBranchLocation.Text == "Select/Type" ? null : txtHdBranchLocation.Text;
                    tempMonthlyOtMaster.EOL_BRANCH = Convert.ToInt16(hdfHdBranchLocation.Value) > 0 ? Convert.ToInt16(hdfHdBranchLocation.Value) : 0;
                    tempMonthlyOtMaster.EOL_LV_MONTH = string.IsNullOrEmpty(txtMonth.Text) ? null : DateTime.Parse(txtMonth.Text).ToString();
                    tempMonthlyOtMaster.EOE_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                    tempMonthlyOtMaster.EOE_NO = lblTrxNo.Text;
                    tempMonthlyOtMaster.EOE_COMPANY = Convert.ToInt32(ddlCompanyHd.SelectedValue);
                    //tempMonthlyOtMaster.EOD_LV_DAY = LeaveDay;
                    if (chkSun.Checked == true)
                        tempMonthlyOtMaster.EOD_LV_DAY = 1;
                    else if (chkMon.Checked == true)
                        tempMonthlyOtMaster.EOD_LV_DAY = 2;
                    else if (chkTue.Checked == true)
                        tempMonthlyOtMaster.EOD_LV_DAY = 3;
                    else if (chkWed.Checked == true)
                        tempMonthlyOtMaster.EOD_LV_DAY = 4;
                    else if (chkThu.Checked == true)
                        tempMonthlyOtMaster.EOD_LV_DAY = 5;
                    else if (chkFri.Checked == true)
                        tempMonthlyOtMaster.EOD_LV_DAY = 6;
                    else if (chkSat.Checked == true)
                        tempMonthlyOtMaster.EOD_LV_DAY = 7;

                    if (Convert.ToInt32(hdfHdBranchLocation.Value) > 0)
                    {
                        tempMonthlyOtMaster.EOE_BRANCH = Convert.ToString(hdfHdBranchLocation.Value);
                    }
                    if (Convert.ToInt32(hdfHdDepartment.Value) > 0)
                    {
                        tempMonthlyOtMaster.EOE_empDepartment = Convert.ToString(hdfHdDepartment.Value);
                    }
                    tempMonthlyOtMaster.USER_PK = currentUser.PKUser;
                    tempMonthlyOtMaster.BIZUNIT_PK = currentUser.SBUID;
                    tempMonthlyOtMaster.EOE_DEPT = Convert.ToInt32(currentUser.CurrentDeptPK);
                    tempMonthlyOtMaster.LAST_MOD_DT = LastModifiedTime;
                    tempMonthlyOtMaster.MonthlyOTDatas = OTDataList;
                    returnObject = tempMonthlyOtMaster;
                    break;
                    #endregion
            }
            return returnObject;
        }
        #endregion
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK:
                        if (objOTMaster != null)
                        {
                            lblTrxNo.Text = string.IsNullOrEmpty(objOTMaster.EOE_NO) ? Resources.ErpRes.Draft : objOTMaster.EOE_NO;
                            txtDate.Text = Convert.ToDateTime(objOTMaster.EOE_TO_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            if (Convert.ToInt16(objOTMaster.EOE_empDepartment) > 0)
                            {
                                txtHdDepartment.Text = objOTMaster.EOE_empDepartment_TEXT;
                                hdfHdDepartment.Value = objOTMaster.EOE_empDepartment.ToString();

                            }

                            CompanyPk = objOTMaster.EOE_COMPANY;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            ddlCompanyHd.SelectedIndex = ddlCompanyHd.Items.IndexOf(ddlCompanyHd.Items.FindByValue(CompanyPk.ToString()));

                            LastModifiedTime = Convert.ToDateTime(objOTMaster.LAST_MOD_DT);
                            OTDataList = objOTMaster.MonthlyOTDatas;
                            SetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);
                            if (Convert.ToInt16(objOTMaster.EOL_BRANCH) > 0)
                            {
                                txtHdBranchLocation.Text = objOTMaster.EOL_BRANCH_Text;
                                hdfHdBranchLocation.Value = objOTMaster.EOL_BRANCH.ToString();

                            }
                            if (objOTMaster.EOL_OTHER_LEAVE_TYPE == 1)//1-Yearly
                            {
                                chkYearly.Checked = true;
                                chkMonthly.Checked = chkSpecilHoliday.Checked = false;
                            }
                            else if (objOTMaster.EOL_OTHER_LEAVE_TYPE == 2)//2-Monthly
                            {
                                chkMonthly.Checked = true;
                                chkYearly.Checked = chkSpecilHoliday.Checked = false;
                                txtMonth.Text = Convert.ToDateTime(objOTMaster.EOL_LV_MONTH).ToString(Resources.Constants.DateFormatMonthYear);
                            }
                            else if (objOTMaster.EOL_OTHER_LEAVE_TYPE == 3)//3-Special Holiday
                            {
                                chkSpecilHoliday.Checked = true;
                                chkMonthly.Checked = chkYearly.Checked = false;
                                ddlQuarter.SelectedIndex = ddlQuarter.Items.IndexOf(ddlQuarter.Items.FindByValue(objOTMaster.EOL_SPECIAL_HOLYDAY_TYPE.ToString()));
                            }
                            //CheckUncheckControls(objOTMaster.EOD_LV_DAY);
                            if (objOTMaster.EOD_LV_DAY == 1)
                                chkSun.Checked = true;
                            else if (objOTMaster.EOD_LV_DAY == 2)
                                chkMon.Checked = true;
                            else if (objOTMaster.EOD_LV_DAY == 3)
                                chkTue.Checked = true;
                            else if (objOTMaster.EOD_LV_DAY == 4)
                                chkWed.Checked = true;
                            else if (objOTMaster.EOD_LV_DAY == 5)
                                chkThu.Checked = true;
                            else if (objOTMaster.EOD_LV_DAY == 6)
                                chkFri.Checked = true;
                            else if (objOTMaster.EOD_LV_DAY == 7)
                                chkSat.Checked = true;

                            EnableDisableControls(ControlsEnum.OTHERLEAVEENTRYDETAILSBYPK);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                #region CLEARADD
                case ControlsEnum.CLEARADD:
                    chkYearly.Checked = true;
                    txtMonth.Enabled = ddlQuarter.Enabled = false;
                    txtMonth.Text = string.Empty;
                    ddlQuarter.SelectedIndex = 0;
                    chkMonthly.Checked = chkSpecilHoliday.Checked = chkMon.Checked = chkTue.Checked = chkWed.Checked = chkThu.Checked = chkFri.Checked = chkSat.Checked = chkSun.Checked = false;
                    //LeaveType = 1;
                    //LeaveDay = 0;
                    OTDataList.ToList().ForEach(u =>
                    {
                        u.LeaveType = null;
                        u.SpecialHoliday =null;
                    });
                    BindGrid(ControlsEnum.EMPLOYEEOTDETAILS);
                    break;
                #endregion
                #region CLEAR
                case ControlsEnum.CLEAR:
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    hdfCurrentELD_PK.Value = hdfCurrentROW_NO.Value = string.Empty;
                    txtFilterBranch.Text = txtFilterDept.Text = txtFilterFromDate.Text = txtFilterToDate.Text = txtMon.Text = string.Empty;
                    hdfFilterBranch.Value = hdfFilterDept.Value = CommonConstants.SELECTVAL;

                    txtDate.Text = hdfLastSelectedDate.Value = txtHdBranchLocation.Text = txtHdDepartment.Text = string.Empty;
                    hdfHdBranchLocation.Value = hdfHdDepartment.Value = CommonConstants.SELECTVAL;
                    CurrPK = 0;
                    //LeaveType = 1;
                    hdfTrxPk.Value = string.Empty;
                    txtTrxNo.Text = string.Empty;
                    txtDesignation.Text = txtEmployee.Text = string.Empty;
                    hdfDesignation.Value = hdfEmployee.Value = CommonConstants.SELECTVAL;
                    hdfImportVisible.Value = "0";
                    PageIndex = 1;
                    uclPaging.CurrentPage = 0;

                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    OTDataList = null;
                    SetFieldValues(ControlsEnum.EMPLOYEEOTDETAILS);

                    chkYearly.Checked = true;
                    txtMonth.Enabled = ddlQuarter.Enabled = false;
                    txtMonth.Text = string.Empty;
                    ddlQuarter.SelectedIndex = 0;
                    chkMonthly.Checked = chkSpecilHoliday.Checked = chkMon.Checked = chkTue.Checked = chkWed.Checked = chkThu.Checked = chkFri.Checked = chkSat.Checked = chkSun.Checked = false;
                    //LeaveType = 1;
                    //LeaveDay = 0;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtFilterBranch.Text = txtFilterDept.Text = txtFilterFromDate.Text = txtFilterToDate.Text = string.Empty;
                    hdfFilterBranch.Value = hdfFilterDept.Value = CommonConstants.SELECTVAL;
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = 1;
                    this.EntryStatus = EntryStatus.LISTMODE;
                    this.CurrPK = 0;
                    //LeaveType = 1;
                    hdfTrxPk.Value = string.Empty;
                    txtTrxNo.Text = string.Empty;
                    break;
                #endregion
                #region CLEARDETAIL
                case ControlsEnum.CLEARDETAIL:
                    txtDesignation.Text = txtEmployee.Text = txtHdBranchLocation.Text = txtHdDepartment.Text = string.Empty;
                    hdfDesignation.Value = hdfEmployee.Value = hdfHdBranchLocation.Value = hdfHdDepartment.Value = CommonConstants.SELECTVAL;
                    OTDataList = null;
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
        public int GetInt(string str)
        {
            int result = 0;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            return result;
        }
        /// <summary>
        /// Convert time string to deciam
        /// </summary>
        /// <param name="hourminutes">hh:mm or hhh:mm</param>
        /// <returns></returns>
        private decimal? ConvertTimeStringToDecimal(string hourminutes)
        {
            decimal? result = null;
            try
            {
                if (!string.IsNullOrEmpty(hourminutes))
                {
                    string[] arrTime = hourminutes.Split(':');
                    decimal hours = Convert.ToDecimal(arrTime[0]);
                    decimal minutes = 0;
                    if (arrTime[1].Length > 1)
                        minutes = Convert.ToDecimal(arrTime[1]);
                    result = (minutes / Convert.ToDecimal(60)) + Convert.ToDecimal(hours);
                    result = Math.Round(result.Value, 2);
                }
                return result;
            }
            catch
            {
                return result;
            }
        }
        private double ConvertTimeStringToDouble(string hourminutes)
        {
            double result = 0;
            try
            {
                if (!string.IsNullOrEmpty(hourminutes))
                {
                    string[] arrTime = hourminutes.Split(':');
                    double hours = Convert.ToDouble(arrTime[0]);
                    double minutes = 0;
                    if (arrTime[1].Length > 1)
                        minutes = Convert.ToDouble(arrTime[1]);
                    //result = (minutes / Convert.ToDouble(60)) + Convert.ToDouble(hours);
                    result = Convert.ToDouble(hours + "." + minutes);
                    result = Math.Round(result, 2);
                }
                return result;
            }
            catch
            {
                return result;
            }
        }
        private string GetTime(string time)
        {
            string result = string.Empty;
            if (time == string.Empty) return result;

            string[] arr = time.Split('.');
            string h = arr[0];
            string m = string.Empty.PadRight(2, '0');
            if (arr.Length == 2)
            {
                if (arr[1].Length > 1) m = arr[1];
                else m = arr[1].PadRight(2, '0');
            }
            decimal mm = Convert.ToDecimal(m);
            //  mm = Math.Round(((mm * 60) / 100));
            mm = Math.Round(mm);
            m = mm.ToString();
            if (h.Length < 2) h = "0" + h;
            if (m.Length < 2) m = "0" + m;
            result = h + ":" + m;
            return result;
        }
        #endregion
        #region Configuration Settings
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("HRMS SETTINGS", "OT ENTRY");
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfOTEntry.Value = string.Empty;
                hdfOTEntry.Value = Convert.ToString(dt.Rows[0]["ACF_VALUE"]); //1:OT Hours, 2:OT 1-3(5) hr                
            }
        }
        #endregion
        #region ControlEnum
        public enum ControlsEnum
        {
            EMPLOYEES,
            LEAVETYPES,
            COMPANY,
            EMPLOYEEOTDETAILS,
            ADDTOLIST,
            AFTERDELETE,
            OTCALCULATERMASTER,
            CLEAR,
            LIST,
            CLEARSEARCH,
            EMPLOYMENTTYPE,
            CLEARDETAIL,
            OTHERLEAVEENTRYDETAILSBYPK,
            EMPLOYEEOTDETAILSEDIT,
            EMPLOYEETYPE,
            SPECIALHOLIDAYTYPE,
            MONTHLY,
            YEARLY,
            SPECIALHOLIDAY,
            CLEARADD,
            SAVE
        }
        #endregion
    }
}