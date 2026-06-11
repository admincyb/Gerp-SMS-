using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.HRMS.Payroll;

using BusinessObject.Common;
using ERP.Utilities;
using System.Data;
using BusinessObject.CommonManagement;
using System.IO;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using BusinessObject.AccountManagement;
using AjaxControlToolkit;
using System.Web.Services;
using ERP.Utilities.HRMS;
using ERPSMS_v01.UserControls;
using BusinessObject;
using BusinessLogic.HRMS.Employee;
using ERPManager;
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class AttendanceManagement : ERP.Store.UI.MyBasePage
    {
        #region Properties & Variables

        #region Properties
        /// <summary>
        /// Entry State for managing display status
        /// </summary>
        private EntryStatus EntryStatus
        {
            get
            {
                return this.ViewState[ERP.Utilities.ViewstateStrings.EntryState] == null ? EntryStatus.LISTMODE : (EntryStatus)(this.ViewState[ERP.Utilities.ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ERP.Utilities.ViewstateStrings.EntryState] = value;
            }
        }

        /// <summary>
        /// Hold data for AttendancePopupDetailsViewState  to bind Popup Gridview
        /// </summary>
        private List<AttendancePopupDetails> AttendancePopupDetailsViewState
        {
            get
            {
                return this.ViewState["AttendancePopupDetailsViewState"] == null ? new List<AttendancePopupDetails>() : (List<AttendancePopupDetails>)(this.ViewState["AttendancePopupDetailsViewState"]);
            }
            set
            {
                this.ViewState["AttendancePopupDetailsViewState"] = value;
            }
        }

        /// <summary>
        /// Holds AttendancePopupDetailsViewState for Bind Gridview
        /// </summary>
        private List<AttendancePopupDetails> AttendanceEntryGridViewList
        {
            get
            {
                return this.ViewState["AttendanceEntryGridViewList"] == null ? new List<AttendancePopupDetails>() : (List<AttendancePopupDetails>)(this.ViewState["AttendanceEntryGridViewList"]);
            }
            set
            {
                this.ViewState["AttendanceEntryGridViewList"] = value;
            }
        }
        /// <summary>
        /// To keep time mask in view state
        /// </summary>
        private string TimeMask
        {
            get
            {
                return this.ViewState["TimeMask"] == null ? string.Empty : (this.ViewState["TimeMask"]).ToString();
            }
            set
            {
                this.ViewState["TimeMask"] = value;
            }
        }
        /// <summary>
        /// To keep time mask validation expression in view state
        /// </summary>
        private string TimeMaskValidationExp
        {
            get
            {
                return this.ViewState["TimeMaskValidationExp"] == null ? string.Empty : (this.ViewState["TimeMaskValidationExp"]).ToString();
            }
            set
            {
                this.ViewState["TimeMaskValidationExp"] = value;
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
        /// To keep Page Index In View State
        /// </summary>
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
        /// To keep RowIndex in view state
        /// </summary>
        private int SlNo
        {
            get
            {
                return this.ViewState[ViewstateStrings.SlNo] == null ? -1 : (int)this.ViewState[ViewstateStrings.SlNo];
            }
            set
            {
                this.ViewState[ViewstateStrings.SlNo] = value;
            }
        }

        /// <summary>
        /// To maintain the SortExpression or sort By in viewstate
        /// </summary>
        private string SortBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortBy] = value;
            }
        }

        /// <summary>
        /// To maintain the Sort Direction in viewstate
        /// </summary>
        private string SortDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SortDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.SortDirection] = value;
            }
        }

        #endregion

        private DataTable dtResult;
        private DataTable dtCompany;
        private DataTable dtList;
        private BusinessObject.User currentUser;
        private EmployeeAttendance employeeAttendance;
        private ERPData.ADM_COMPANY_MST admCompanyMstObj;
        private List<ERPData.ADM_COMPANY_MST> admCompanyMstList;
        private FileInfo uploadInfo;
        private string extns;
        private string uploadPath;
        private string applicationPath;
        private string uploadName;
        private FileInfo attchInfo;
        private OleDbConnection connExcel;
        private OleDbCommand cmdExcel;
        private OleDbDataAdapter oleDbDataAdapter;
        private DataTable dtExcelSchema;
        private DataSet dsImportedAttendance;
        private string landingSheet;
        private string date;
        int attendancePk = 0;
        private int EmployeePopupPk;
        private DateTime AttendanceDate;

        TimeSpan timeSpanIn;
        TimeSpan timeSpanOut;

        TimeSpan timeSpanBOut1;
        TimeSpan timeSpanBIn1;
        TimeSpan timeSpanBOut2;
        TimeSpan timeSpanBIn2;

        private double totalDays = 0;
        private string prevDate;
        private int CompanyPk = 0;
        private int UpdateFlag = 0;
        private int EmpPk = 0;
        double EmpWorkHrs = 0;
        private string[] excelColumns;
        private string[] airColums_General = { "Date", "EmpID", "FirstIn", "LastOut", "TotalHrs" };
        private string[] airColums_IGCL = { "Date", "ID", "Type" };
        private DataTable dtAttendanceDetails;

        private StringBuilder sb;
        private string xmlLanding;
        #endregion

        #region PageEvents
        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();

        }
        #region Page_Init
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            uclPaging.CurrentPage = 1;
        }
        #endregion
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion

        #region Page_PreRender
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                hdfCurrPk.Value = CurrPK.ToString();
                if (grdEmployeeAttendance.Rows.Count > 0)
                {
                    ddlProcessModeHdr.Enabled = false;
                    hdfDetailsRowCount.Value = "1";
                }
                else
                {
                    ddlProcessModeHdr.Enabled = true;
                    hdfDetailsRowCount.Value = "0";
                }
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(2);});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideFilterSec(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideFilterSec('" + hdfShowHideFilterSec.Value + "');});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideFilterSec('" + hdfShowHideFilterSec.Value + "');});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ModifiedDatePnl.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                if (grdEmployeeAttendance.Rows.Count > 0)
                {
                    txtDate.Enabled = false;
                    txtDate.CssClass = "input-small input-disabled";
                }
                else
                {
                    txtDate.Enabled = true;
                    txtDate.CssClass = "input-small";
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #endregion

        #region ActionHandler

        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            //To check if department is different by opening in new tab
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            GridViewRow grvRow;
            bool bIsChecked = false;
            HiddenField hdfSlNo;

            try
            {
                int? result = null;
                #region Getting Command Action
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
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                }

                #endregion
                switch (commonActions)
                {
                    #region LIST
                    case ActionsEnum.LIST:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        ResetForm(ControlEnums.CLEARSEARCH);
                        ResetForm(ControlEnums.DTLCLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (AttendanceEntryGridViewList == null || AttendanceEntryGridViewList.Count == 0)//grdEmployeeAttendance.Rows.Count < 1
                        {
                            litErrorMsg.Text = (GetLocalResourceObject("Err_NoRecordFoundToSave")).ToString();// Resources.Messages.ActionFailedPleaseTryAgain;
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            //    + "','" + Resources.ErpRes.Information + "');", true);
                            //return;
                            throw new ApplicationException(litErrorMsg.Text);
                        }
                        string EmpName = string.Empty;
                        string trxNo = string.Empty;
                        employeeAttendance = (EmployeeAttendance)SetUIValuesToObject(ControlEnums.ATTENDANCEDETAILS);
                        employeeAttendance.WKF_FLAG = 1;
                        result = BusinessLogic.HRMS.Payroll.AttendanceBL.SaveAttendance(employeeAttendance, out EmpName, out trxNo);
                        if (result > 0)
                        {
                            //AttendanceEntryGridViewList = (List<AttendancePopupDetails>)SetUIValuesToObject(ControlEnums.ATTENDANCELIST);
                            //litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                            //litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                            lblTrxNo.Text = trxNo;
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                            object[] args = new object[2];
                            args[0] = Resources.PageNameRes.Attendance;
                            args[1] = trxNo;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                            ResetForm(ControlEnums.CLEAR);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlEnums.LIST);
                            SetFieldValues(ControlEnums.LIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                            //ResetForm(ActionsEnum.SAVE);
                            // GetFieldValues(ControlEnums.DEPRETRANLIST);
                            // SetFieldValues(ControlEnums.DEPRETRANLIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST || result == (int)DbSaveStatus.SHIFTEXIST)
                            {
                                if (string.IsNullOrEmpty(EmpName))
                                    litErrorMsg.Text = GetLocalResourceObject("AlreadyExist").ToString(); //Resources.PageNameRes.Attendance + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                else
                                    litErrorMsg.Text = GetLocalResourceObject("AlreadyAdded").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (SaveDbEnum.EMPWITHDIFFPROCESSMODE == (SaveDbEnum)(result))
                            {
                                litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_EmpWithDifferentPrcMode").ToString(), ddlProcessModeHdr.SelectedItem.Text) + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlEnums.CLEARADD);
                        break;
                    #endregion
                    #region SAVEIMPORT
                    case ActionsEnum.SAVEIMPORT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (fupImport.HasFile)
                            {
                                string conStr;
                                string filePath = SaveDetails(out conStr, fupImport, Convert.ToInt32(ddlAttnTemplate.SelectedValue));
                                if (!string.IsNullOrEmpty(filePath))
                                {
                                    if (Convert.ToInt32(ddlAttnTemplate.SelectedValue) == (int)AttendanceTemplateType.IGCL_TXT)//IGCL.txt
                                    {
                                        AttendanceImport AttendanceImportObj = GetAttendanceImport(filePath);
                                        if (AttendanceImportObj != null && AttendanceImportObj.ImportData != null && AttendanceImportObj.ImportData.Count > 0)
                                        {
                                            SaveAttendanceImport(CommonFunctions.XmlSerialize<AttendanceImport>(AttendanceImportObj));
                                        }
                                        else// Not in given format
                                        {
                                            litErrorMsg.Text = this.GetLocalResourceObject("Err_EnteredFileIncorrectFormat").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            break;
                                        }
                                    }
                                    else
                                        ImportToGrid(filePath, conStr);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_InvalidFile").ToString())
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = this.GetLocalResourceObject("AttachFile").ToString();
                                Page.ClientScript.RegisterStartupScript(typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region DELETE HDR
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.HRMS.Payroll.AttendanceBL.DeleteAttendance(CurrPK, null, LastModifiedTime);
                        if (result > 0)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                            ResetForm(ControlEnums.CLEAR);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlEnums.LIST);
                            SetFieldValues(ControlEnums.LIST);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED || result == (int)DbSaveStatus.REFNOEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.UsedInAnotherPlace;
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
                                litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.EditUsedByAnotherUser;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.AlreadyDeleted;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
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
                                ResetForm(ControlEnums.CLEARHDR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ResetForm(ControlEnums.CLEARADD);
                            ResetForm(ControlEnums.DTLCLEARSEARCH);
                            hdfShowHideFilterSec.Value = "0";
                            EntryStatus = EntryStatus.EDITMODE;
                            txtDate.Enabled = false;
                            GetFieldValues(ControlEnums.GETATTENDANCEBYPK);
                            SetFieldValues(ControlEnums.ATTENDANCEHDR);
                            SetFieldValues(ControlEnums.GETATTENDANCE);
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
                        CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        AttendanceEntryGridViewList = null;
                        AttendancePopupDetailsViewState = null;
                        ResetForm(ControlEnums.CLEARHDR);
                        txtDate.Enabled = true;
                        txtDate.Text = string.Empty; //DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                        SetFieldValues(ControlEnums.GETATTENDANCE);
                        GetFieldValues(ControlEnums.COMPANY);
                        SetFieldValues(ControlEnums.COMPANY);
                        ResetForm(ControlEnums.CLEARADD);
                        ResetForm(ControlEnums.DTLCLEARSEARCH);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        uclPaging.CurrentPage = 0;
                        this.PageIndexList = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region CLEAR SEARCH
                    case ActionsEnum.CLEARSEARCH:
                        ResetForm(ControlEnums.CLEARSEARCH);
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region VIEW ATTENDACE DETAILS
                    case ActionsEnum.VIEWATTENDANCEDETAILS:
                        grvRow = (GridViewRow)((sender as ImageButton).Parent.Parent);
                        HiddenField VattnPk = (HiddenField)grvRow.FindControl("hdfEAT_PK");
                        attendancePk = 0;
                        int.TryParse(VattnPk.Value, out attendancePk);
                        lblEmpNameCode.Text = string.Empty;
                        GetFieldValues(ControlEnums.VIEWATTENDANCEDETAILS);
                        SetFieldValues(ControlEnums.VIEWATTENDANCEDETAILS);
                        if (dtAttendanceDetails != null && dtAttendanceDetails.Rows.Count > 0)
                        {
                            lblBreak1.Text = string.Format(GetLocalResourceObject("Break1Text").ToString(), Convert.ToString(dtAttendanceDetails.Rows[0]["EAT_BREAK_1"]));
                            lblBreak2.Text = string.Format(GetLocalResourceObject("Break2Text").ToString(), Convert.ToString(dtAttendanceDetails.Rows[0]["EAT_BREAK_2"]));
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ClosePopup();ShowContainerDiv('[id$=divEmpAttendanceDetails]','Attendance Details','600','450');", true);
                        break;
                    #endregion
                    #region ADD
                    case ActionsEnum.ADD:
                        if (ConvertTimeStringToDouble(txtTotalHours.Text.Trim()) > 0)
                        {
                            if (AttendanceEntryGridViewList == null)
                                AttendanceEntryGridViewList = new List<AttendancePopupDetails>();
                            List<AttendancePopupDetails> objTempList = AttendanceEntryGridViewList;
                            date = txtDetailDate.Text;
                            DateTime dtFrom = Convert.ToDateTime(txtDate.Text);
                            DateTime dtTo = Convert.ToDateTime(txtDate.Text);
                            if (!ValidateDetailDateAndTime(ref dtFrom, ref dtTo))
                            {
                                string msgInvalidAttn = GetLocalResourceObject("Err_InvalidAttendance").ToString();
                                msgInvalidAttn = string.Format(msgInvalidAttn, dtFrom.ToString(Resources.Constants.HRMSDateFormatAttendance), dtTo.ToString(Resources.Constants.HRMSDateFormatAttendance));
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(msgInvalidAttn)
                                      + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }
                            if (SlNo > 0 && AttendanceEntryGridViewList.Count > 0)// update
                            {
                                //AttendancePopupDetails objTempattn = objTempList[RowIndex];
                                AttendancePopupDetails objTempattn = objTempList.SingleOrDefault(r => r.SlNo == SlNo);
                                if (!string.IsNullOrEmpty(txtInTime.Text.Trim()))
                                {
                                    objTempattn.InDt = date.ToString();
                                    TimeSpan.TryParse(txtInTime.Text.Trim(), out timeSpanIn);
                                    objTempattn.InDt = (Convert.ToDateTime(objTempattn.InDt).Add(timeSpanIn)).ToString();
                                }
                                else
                                    objTempattn.InDt = null;
                                if (!string.IsNullOrEmpty(txtBOutTime1.Text.Trim()))
                                {
                                    objTempattn.BOut1Dt = date;
                                    TimeSpan.TryParse(txtBOutTime1.Text.Trim(), out timeSpanBOut1);
                                    objTempattn.BOut1Dt = (Convert.ToDateTime(objTempattn.BOut1Dt).Add(timeSpanBOut1)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.BOut1Dt) - Convert.ToDateTime(objTempattn.InDt)).TotalDays;
                                    }
                                    if (!string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.BOut1Dt) - Convert.ToDateTime(objTempattn.InDt)).TotalDays;
                                        //if (totalDays < 0)
                                        //    objTempattn.BOut1Dt = Convert.ToDateTime(objTempattn.BOut1Dt).AddDays(1).ToString();
                                    }
                                    if (totalDays < 0)
                                        objTempattn.BOut1Dt = Convert.ToDateTime(objTempattn.BOut1Dt).AddDays(1).ToString();
                                }
                                else
                                    objTempattn.BOut1Dt = null;
                                if (!string.IsNullOrEmpty(txtBInTime1.Text.Trim()))
                                {
                                    objTempattn.BIn1Dt = date;
                                    TimeSpan.TryParse(txtBInTime1.Text.Trim(), out timeSpanBIn1);
                                    objTempattn.BIn1Dt = (Convert.ToDateTime(objTempattn.BIn1Dt).Add(timeSpanBIn1)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.BIn1Dt) - Convert.ToDateTime(objTempattn.InDt)).TotalDays;
                                    }
                                    if (!string.IsNullOrEmpty(objTempattn.BOut1Dt))
                                    {
                                        totalDays = (Convert.ToDateTime(objTempattn.BIn1Dt) - Convert.ToDateTime(objTempattn.BOut1Dt)).TotalDays;
                                        //if (totalDays < 0)
                                        //    objTempattn.BIn1Dt = Convert.ToDateTime(objTempattn.BIn1Dt).AddDays(1).ToString();
                                    }
                                    if (totalDays < 0)
                                        objTempattn.BIn1Dt = Convert.ToDateTime(objTempattn.BIn1Dt).AddDays(1).ToString();
                                }
                                else
                                    objTempattn.BIn1Dt = null;
                                if (!string.IsNullOrEmpty(txtBOutTime2.Text.Trim()))
                                {
                                    objTempattn.BOut2Dt = date;
                                    TimeSpan.TryParse(txtBOutTime2.Text.Trim(), out timeSpanBOut2);
                                    objTempattn.BOut2Dt = (Convert.ToDateTime(objTempattn.BOut2Dt).Add(timeSpanBOut2)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.BOut2Dt) - Convert.ToDateTime(objTempattn.InDt)).TotalDays;
                                    }
                                    if (!string.IsNullOrEmpty(objTempattn.BIn1Dt) || !string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        prevDate = !string.IsNullOrEmpty(objTempattn.BIn1Dt) ? objTempattn.BIn1Dt : objTempattn.InDt;
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.BOut2Dt) - Convert.ToDateTime(prevDate)).TotalDays;
                                        //if (totalDays < 0)
                                        //    objTempattn.BOut2Dt = Convert.ToDateTime(objTempattn.BOut2Dt).AddDays(1).ToString();
                                    }
                                    if (totalDays < 0)
                                        objTempattn.BOut2Dt = Convert.ToDateTime(objTempattn.BOut2Dt).AddDays(1).ToString();
                                }
                                else
                                    objTempattn.BOut2Dt = null;
                                if (!string.IsNullOrEmpty(txtBInTime2.Text.Trim()))
                                {
                                    objTempattn.BIn2Dt = date;
                                    TimeSpan.TryParse(txtBInTime2.Text.Trim(), out timeSpanBIn2);
                                    objTempattn.BIn2Dt = (Convert.ToDateTime(objTempattn.BIn2Dt).Add(timeSpanBIn2)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.BIn2Dt) - Convert.ToDateTime(objTempattn.InDt)).TotalDays;
                                    }
                                    if (!string.IsNullOrEmpty(objTempattn.BOut2Dt) || !string.IsNullOrEmpty(objTempattn.BOut1Dt))
                                    {
                                        prevDate = !string.IsNullOrEmpty(objTempattn.BOut2Dt) ? objTempattn.BOut2Dt : objTempattn.BOut1Dt;
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.BIn2Dt) - Convert.ToDateTime(prevDate)).TotalDays;
                                        if (totalDays < 0)
                                            objTempattn.BIn2Dt = Convert.ToDateTime(objTempattn.BIn2Dt).AddDays(1).ToString();
                                    }
                                    if (totalDays < 0)
                                        objTempattn.BIn2Dt = Convert.ToDateTime(objTempattn.BIn2Dt).AddDays(1).ToString();
                                }
                                else
                                    objTempattn.BIn2Dt = null;
                                if (!string.IsNullOrEmpty(txtOutTime.Text.Trim()))
                                {
                                    objTempattn.OutDt = date;
                                    TimeSpan.TryParse(txtOutTime.Text.Trim(), out timeSpanOut);
                                    objTempattn.OutDt = (Convert.ToDateTime(objTempattn.OutDt).Add(timeSpanOut)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.OutDt) - Convert.ToDateTime(objTempattn.InDt)).TotalDays;
                                    }
                                    if (!string.IsNullOrEmpty(objTempattn.BIn2Dt) || !string.IsNullOrEmpty(objTempattn.BIn1Dt) || !string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        if (!string.IsNullOrEmpty(objTempattn.BIn2Dt))
                                            prevDate = objTempattn.BIn2Dt;
                                        else if (!string.IsNullOrEmpty(objTempattn.BIn1Dt))
                                            prevDate = objTempattn.BIn1Dt;
                                        else
                                            prevDate = objTempattn.InDt;
                                        if (totalDays >= 0)
                                            totalDays = (Convert.ToDateTime(objTempattn.OutDt) - Convert.ToDateTime(prevDate)).TotalDays;
                                        //if (totalDays < 0)
                                        //    objTempattn.OutDt = Convert.ToDateTime(objTempattn.OutDt).AddDays(1).ToString();
                                    }
                                    if (totalDays < 0)
                                        objTempattn.OutDt = Convert.ToDateTime(objTempattn.OutDt).AddDays(1).ToString();
                                }
                                else
                                    objTempattn.OutDt = null;
                                objTempattn.TotalHrs = ConvertTimeStringToDouble(txtTotalHours.Text.Trim());
                                objTempattn.OTDtHrs = ConvertTimeStringToDouble(txtOTHours.Text.Trim());
                                objTempattn.ShortHrs = ConvertTimeStringToDouble(txtLateHrs.Text.Trim());
                                objTempattn.BreakHrs = hdfEmpBreakHours.Value;
                                objTempattn.HasOTFromPunching = !string.IsNullOrEmpty(hdfEmpHasOTFromPunching.Value) ? Convert.ToInt32(hdfEmpHasOTFromPunching.Value) : 0;
                                objTempattn.WorkingHrs = hdfEmpWorkHours.Value;
                                objTempattn.NormalHrs = ConvertTimeStringToDouble(txtNormalHours.Text.Trim());
                                objTempattn.EAT_STATUS = chkStatus.Checked ? (int)DbActiveStatus.ACTIVE : (int)DbActiveStatus.INACTIVE;
                                objTempattn.EmpPk = EmpPk = Convert.ToInt32(hdfEmployee.Value);
                                objTempattn.EmpCode = HttpUtility.HtmlEncode(txtEmployee.Text);
                                objTempattn.Remarks = string.IsNullOrEmpty(txtDetRemarks.Text.Trim()) ? null : HttpUtility.HtmlEncode(txtDetRemarks.Text);
                                objTempattn.UpdateFlag = 1;
                                objTempattn.DateDt = Convert.ToDateTime(txtDetailDate.Text);

                                if (!string.IsNullOrEmpty(objTempattn.BOut1Dt) && !string.IsNullOrEmpty(objTempattn.BIn1Dt))
                                {
                                    TimeSpan dtBreak1 = Convert.ToDateTime(objTempattn.BIn1Dt) - Convert.ToDateTime(objTempattn.BOut1Dt);
                                    if (GetDecimalFromTimeSpan(dtBreak1) > 0)
                                        objTempattn.EAT_BREAK1 = GetTime(GetDecimalFromTimeSpan(dtBreak1).ToString());
                                    else
                                        objTempattn.EAT_BREAK1 = GetLocalResourceObject("ZeroTime").ToString();
                                }
                                else
                                    objTempattn.EAT_BREAK1 = GetLocalResourceObject("ZeroTime").ToString();

                                if (!string.IsNullOrEmpty(objTempattn.BOut2Dt) && !string.IsNullOrEmpty(objTempattn.BIn2Dt))
                                {
                                    TimeSpan dtBreak2 = Convert.ToDateTime(objTempattn.BIn2Dt) - Convert.ToDateTime(objTempattn.BOut2Dt);
                                    if (GetDecimalFromTimeSpan(dtBreak2) > 0)
                                        objTempattn.EAT_BREAK2 = GetTime(GetDecimalFromTimeSpan(dtBreak2).ToString());
                                    else
                                        objTempattn.EAT_BREAK2 = GetLocalResourceObject("ZeroTime").ToString();
                                }
                                else
                                    objTempattn.EAT_BREAK2 = GetLocalResourceObject("ZeroTime").ToString();

                                GetFieldValues(ControlEnums.EMPLOYEEDETAIL);
                                if (dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    objTempattn.LocationPk = Convert.ToString(dtResult.Rows[0]["empBranch"]);
                                    objTempattn.Location = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empBranch_Text"]));
                                    objTempattn.LocationCode = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empBranch_Code"]));
                                    objTempattn.EmpDeptPk = Convert.ToString(dtResult.Rows[0]["empDepartment"]);
                                    objTempattn.EmpDepartment = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empDepartmentText"]));
                                    objTempattn.EmpDepartmentCode = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empDepartmentCode"]));
                                }


                            }
                            else // new
                            {
                                AttendancePopupDetails objTempattn = new AttendancePopupDetails();
                                if (!string.IsNullOrEmpty(txtInTime.Text.Trim()))
                                {
                                    objTempattn.InDt = date.ToString();
                                    TimeSpan.TryParse(txtInTime.Text.Trim(), out timeSpanIn);
                                    objTempattn.InDt = (Convert.ToDateTime(objTempattn.InDt).Add(timeSpanIn)).ToString();
                                }
                                else
                                    objTempattn.InDt = null;
                                if (!string.IsNullOrEmpty(txtBOutTime1.Text.Trim()))
                                {
                                    objTempattn.BOut1Dt = date;
                                    TimeSpan.TryParse(txtBOutTime1.Text.Trim(), out timeSpanBOut1);
                                    objTempattn.BOut1Dt = (Convert.ToDateTime(objTempattn.BOut1Dt).Add(timeSpanBOut1)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        totalDays = (Convert.ToDateTime(objTempattn.BOut1Dt) - Convert.ToDateTime(objTempattn.InDt)).TotalDays;
                                        if (totalDays < 0)
                                            objTempattn.BOut1Dt = Convert.ToDateTime(objTempattn.BOut1Dt).AddDays(1).ToString();
                                    }
                                }
                                else
                                    objTempattn.BOut1Dt = null;
                                if (!string.IsNullOrEmpty(txtBInTime1.Text.Trim()))
                                {
                                    objTempattn.BIn1Dt = date;
                                    TimeSpan.TryParse(txtBInTime1.Text.Trim(), out timeSpanBIn1);
                                    objTempattn.BIn1Dt = (Convert.ToDateTime(objTempattn.BIn1Dt).Add(timeSpanBIn1)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.BOut1Dt))
                                    {
                                        totalDays = (Convert.ToDateTime(objTempattn.BIn1Dt) - Convert.ToDateTime(objTempattn.BOut1Dt)).TotalDays;
                                        if (totalDays < 0)
                                            objTempattn.BIn1Dt = Convert.ToDateTime(objTempattn.BIn1Dt).AddDays(1).ToString();
                                    }
                                }
                                else
                                    objTempattn.BIn1Dt = null;
                                if (!string.IsNullOrEmpty(txtBOutTime2.Text.Trim()))
                                {
                                    objTempattn.BOut2Dt = date;
                                    TimeSpan.TryParse(txtBOutTime2.Text.Trim(), out timeSpanBOut2);
                                    objTempattn.BOut2Dt = (Convert.ToDateTime(objTempattn.BOut2Dt).Add(timeSpanBOut2)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.BIn1Dt) || !string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        prevDate = !string.IsNullOrEmpty(objTempattn.BIn1Dt) ? objTempattn.BIn1Dt : objTempattn.InDt;
                                        totalDays = (Convert.ToDateTime(objTempattn.BOut2Dt) - Convert.ToDateTime(prevDate)).TotalDays;
                                        if (totalDays < 0)
                                            objTempattn.BOut2Dt = Convert.ToDateTime(objTempattn.BOut2Dt).AddDays(1).ToString();
                                    }
                                }
                                else
                                    objTempattn.BOut2Dt = null;
                                if (!string.IsNullOrEmpty(txtBInTime2.Text.Trim()))
                                {
                                    objTempattn.BIn2Dt = date;
                                    TimeSpan.TryParse(txtBInTime2.Text.Trim(), out timeSpanBIn2);
                                    objTempattn.BIn2Dt = (Convert.ToDateTime(objTempattn.BIn2Dt).Add(timeSpanBIn2)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.BOut2Dt) || !string.IsNullOrEmpty(objTempattn.BOut1Dt))
                                    {
                                        prevDate = !string.IsNullOrEmpty(objTempattn.BOut2Dt) ? objTempattn.BOut2Dt : objTempattn.BOut1Dt;
                                        totalDays = (Convert.ToDateTime(objTempattn.BIn2Dt) - Convert.ToDateTime(prevDate)).TotalDays;
                                        if (totalDays < 0)
                                            objTempattn.BIn2Dt = Convert.ToDateTime(objTempattn.BIn2Dt).AddDays(1).ToString();
                                    }
                                }
                                else
                                    objTempattn.BIn2Dt = null;
                                if (!string.IsNullOrEmpty(txtOutTime.Text.Trim()))
                                {
                                    objTempattn.OutDt = date;
                                    TimeSpan.TryParse(txtOutTime.Text.Trim(), out timeSpanOut);
                                    objTempattn.OutDt = (Convert.ToDateTime(objTempattn.OutDt).Add(timeSpanOut)).ToString();
                                    if (!string.IsNullOrEmpty(objTempattn.BIn2Dt) || !string.IsNullOrEmpty(objTempattn.BIn1Dt) || !string.IsNullOrEmpty(objTempattn.InDt))
                                    {
                                        if (!string.IsNullOrEmpty(objTempattn.BIn2Dt))
                                            prevDate = objTempattn.BIn2Dt;
                                        else if (!string.IsNullOrEmpty(objTempattn.BIn1Dt))
                                            prevDate = objTempattn.BIn1Dt;
                                        else
                                            prevDate = objTempattn.InDt;
                                        totalDays = (Convert.ToDateTime(objTempattn.OutDt) - Convert.ToDateTime(prevDate)).TotalDays;
                                        if (totalDays < 0)
                                            objTempattn.OutDt = Convert.ToDateTime(objTempattn.OutDt).AddDays(1).ToString();
                                    }
                                }
                                else
                                    objTempattn.OutDt = null;
                                objTempattn.TotalHrs = ConvertTimeStringToDouble(txtTotalHours.Text.Trim());
                                objTempattn.OTDtHrs = ConvertTimeStringToDouble(txtOTHours.Text.Trim());
                                objTempattn.ShortHrs = ConvertTimeStringToDouble(txtLateHrs.Text.Trim());
                                objTempattn.BreakHrs = hdfEmpBreakHours.Value;
                                objTempattn.HasOTFromPunching = !string.IsNullOrEmpty(hdfEmpHasOTFromPunching.Value) ? Convert.ToInt32(hdfEmpHasOTFromPunching.Value) : 0;
                                objTempattn.WorkingHrs = string.IsNullOrEmpty(hdfEmpWorkHours.Value) ? null : GetNullableDecimal(hdfEmpWorkHours.Value).ToString();
                                objTempattn.NormalHrs = ConvertTimeStringToDouble(txtNormalHours.Text.Trim());
                                objTempattn.EAT_STATUS = chkStatus.Checked ? (int)DbActiveStatus.ACTIVE : (int)DbActiveStatus.INACTIVE;
                                objTempattn.EmpPk = EmpPk = Convert.ToInt32(hdfEmployee.Value);
                                objTempattn.EmpCode = HttpUtility.HtmlEncode(txtEmployee.Text);
                                objTempattn.Remarks = HttpUtility.HtmlEncode(txtDetRemarks.Text);
                                objTempattn.UpdateFlag = 1;
                                objTempattn.DateDt = Convert.ToDateTime(txtDetailDate.Text);

                                if (!string.IsNullOrEmpty(objTempattn.BOut1Dt) && !string.IsNullOrEmpty(objTempattn.BIn1Dt))
                                {
                                    TimeSpan dtBreak1 = Convert.ToDateTime(objTempattn.BIn1Dt) - Convert.ToDateTime(objTempattn.BOut1Dt);
                                    if (GetDecimalFromTimeSpan(dtBreak1) > 0)
                                        objTempattn.EAT_BREAK1 = GetTime(GetDecimalFromTimeSpan(dtBreak1).ToString());
                                    else
                                        objTempattn.EAT_BREAK1 = GetLocalResourceObject("ZeroTime").ToString();
                                }
                                else
                                    objTempattn.EAT_BREAK1 = GetLocalResourceObject("ZeroTime").ToString();

                                if (!string.IsNullOrEmpty(objTempattn.BOut2Dt) && !string.IsNullOrEmpty(objTempattn.BIn2Dt))
                                {
                                    TimeSpan dtBreak2 = Convert.ToDateTime(objTempattn.BIn2Dt) - Convert.ToDateTime(objTempattn.BOut2Dt);
                                    if (GetDecimalFromTimeSpan(dtBreak2) > 0)
                                        objTempattn.EAT_BREAK2 = GetTime(GetDecimalFromTimeSpan(dtBreak2).ToString());
                                    else
                                        objTempattn.EAT_BREAK2 = GetLocalResourceObject("ZeroTime").ToString();
                                }
                                else
                                    objTempattn.EAT_BREAK2 = GetLocalResourceObject("ZeroTime").ToString();

                                int SerialNumber = 1;
                                if (AttendanceEntryGridViewList != null && AttendanceEntryGridViewList.Count > 0)
                                    SerialNumber = AttendanceEntryGridViewList.Max(r => r.SlNo) + 1;
                                objTempattn.SlNo = SerialNumber;
                                GetFieldValues(ControlEnums.EMPLOYEEDETAIL);
                                if (dtResult != null && dtResult.Rows.Count > 0)
                                {
                                    objTempattn.LocationPk = Convert.ToString(dtResult.Rows[0]["empBranch"]);
                                    objTempattn.Location = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empBranch_Text"]));
                                    objTempattn.LocationCode = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empBranch_Code"]));
                                    objTempattn.EmpDeptPk = Convert.ToString(dtResult.Rows[0]["empDepartment"]);
                                    objTempattn.EmpDepartment = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empDepartmentText"]));
                                    objTempattn.EmpDepartmentCode = HttpUtility.HtmlEncode(Convert.ToString(dtResult.Rows[0]["empDepartmentCode"]));
                                }
                                objTempList.Add(objTempattn);
                            }
                            AttendanceEntryGridViewList = objTempList;
                            SetFieldValues(ControlEnums.GETATTENDANCE);
                            ResetForm(ControlEnums.CLEARADD);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ValidTotalHour").ToString())
                                       + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region GRID EDIT
                    case ActionsEnum.GRIDEDIT:
                        hdfShowHideFilterSec.Value = "1";
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        hdfSlNo = (HiddenField)grvRow.FindControl("hdfSlNo");
                        SlNo = Convert.ToInt32(hdfSlNo.Value);
                        //RowIndex = grvRow.RowIndex;
                        SetFieldValues(ControlEnums.GRIDEDIT);
                        break;
                    #endregion
                    #region GRID DELETE
                    case ActionsEnum.GRIDDELETE:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        hdfSlNo = (HiddenField)grvRow.FindControl("hdfSlNo");
                        SlNo = Convert.ToInt32(hdfSlNo.Value);
                        //RowIndex = grvRow.RowIndex;
                        SetFieldValues(ControlEnums.GRIDDELETE);
                        SetFieldValues(ControlEnums.GETATTENDANCE);
                        break;
                    #endregion
                    #region ATTENDANCE IMPORT
                    case ActionsEnum.ATTIMPORT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                //ResetForm(ControlEnums.CLEARHDR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "EMPATTND" + "&APPSUBTYPE= 0" + "&CurPK=" + CurrPK) + "&ISEXCELPRINT= 1" + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_SelectRow").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;

                    //     case ActionsEnum.ATTDETAILS:
                    ////         foreach (GridViewRow grdrow in grdList.Rows)

                    //         grvRow = ((LinkButton)sender).Parent.Parent as GridViewRow;

                    //             CurrPK = Convert.ToInt32(((LinkButton)sender).CommandArgument);


                    //         ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=1" + "&APPTYPE=" + "EMPATTND" + "&APPSUBTYPE= 1" + "&CurPK=" + CurrPK) + "&ISEXCELPRINT= 0" + "');", true);
                    //         break;

                    #endregion
                    #region IMPORT DETAILS
                    case ActionsEnum.IMPORTDETAILS:
                        if (fupDetImport.HasFile)
                        {
                            string conStr;
                            string filePath = SaveDetails(out conStr, fupDetImport, 0);
                            if (!string.IsNullOrEmpty(filePath))
                            {
                                ImportDetails(filePath, conStr);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_InvalidFile").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("AttachFile").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("AttachFile").ToString())
                                    + "','" + Resources.ErpRes.Information + "');", true);
                        }


                        break;
                    #endregion
                    #region DTL SEARCH
                    case ActionsEnum.DTLSEARCH:
                        //hdfShowHideDtlSearch.Value = "1";
                        ResetForm(ControlEnums.DTLSEARCH);
                        SetFieldValues(ControlEnums.GETATTENDANCE);
                        break;
                    #endregion
                    #region DTL CLEAR SEARCH
                    case ActionsEnum.DTLCLEARSEARCH:
                        //hdfShowHideDtlSearch.Value = "0";
                        ResetForm(ControlEnums.DTLCLEARSEARCH);
                        SetFieldValues(ControlEnums.GETATTENDANCE);
                        break;
                    #endregion


                }
            }
            catch (OleDbException ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

        }

        /// <summary>
        /// Method to validate detail date and intime with header date and configuration values
        /// </summary>
        /// <returns></returns>
        private bool ValidateDetailDateAndTime(ref DateTime dtFrom, ref DateTime dtTo)
        {
            bool blnResult = true;
            if (!string.IsNullOrEmpty(txtDetailDate.Text) && !string.IsNullOrEmpty(txtDate.Text))
            {
                DateTime hdrDate;
                DateTime dtlDate;
                TimeSpan timeSpanHdr;
                TimeSpan.TryParse(hdfShiftStartTime.Value, out timeSpanHdr);
                hdrDate = Convert.ToDateTime(txtDate.Text).Add(timeSpanHdr);
                if (!string.IsNullOrEmpty(txtInTime.Text))
                {
                    TimeSpan timeSpanInTime;
                    TimeSpan.TryParse(txtInTime.Text.Trim(), out timeSpanInTime);
                    dtlDate = Convert.ToDateTime(txtDetailDate.Text).Add(timeSpanInTime);
                    if (dtlDate > hdrDate.AddHours(Convert.ToDouble(hdfAdditionHrs.Value)))
                        blnResult = false;
                }
                else if (Convert.ToDateTime(txtDetailDate.Text) < hdrDate)
                    blnResult = false;

                dtFrom = hdrDate;
                dtTo = hdrDate.AddHours(Convert.ToDouble(hdfAdditionHrs.Value));
            }
            return blnResult;
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {

        }


        /// <summary>
        /// Method used to Handle all actions in the page with GridViewSort
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewSortEventArgs e)
        {
            try
            {
                if (((GridView)sender).ID == "grdEmployeeAttendance")
                {
                    SortBy = e.SortExpression;
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                    if (AttendanceEntryGridViewList != null && AttendanceEntryGridViewList.Count > 0)
                    {
                        IQueryable<AttendancePopupDetails> tempList = AttendanceEntryGridViewList.AsQueryable();
                        ServiceUtility utilityObj = new ServiceUtility();
                        if (!string.IsNullOrEmpty(SortBy))
                        {
                            utilityObj.SortBy = SortBy;
                            utilityObj.SortDirection = SortDirection;
                            utilityObj.CurrentPage = -1;
                            utilityObj.PageSize = -1;
                            AttendanceEntryGridViewList = tempList.SortRecords<AttendancePopupDetails>(utilityObj).ToList();
                            SetFieldValues(ControlEnums.GETATTENDANCE);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #region --- For Grid Actions----
        /// <summary>ON SORTING
        /// Sorting Event Handler for grd AssignProducts
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {

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
                    GetFieldValues(ControlEnums.LIST);
                    SetFieldValues(ControlEnums.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
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
        #region PageActionHandler
        private void PageActionHandler()
        {
            try
            {

                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    GetFieldValues(ControlEnums.EMPLOYMENTTYPE);
                    SetFieldValues(ControlEnums.EMPLOYMENTTYPE);
                    GetFieldValues(ControlEnums.COMPANY);
                    SetFieldValues(ControlEnums.COMPANY);
                    GetFieldValues(ControlEnums.ATTENDANCETEMPLATE);
                    SetFieldValues(ControlEnums.ATTENDANCETEMPLATE);
                    GetFieldValues(ControlEnums.PROCESSMODE);
                    SetFieldValues(ControlEnums.PROCESSMODE);
                    SetFieldValues(ControlEnums.PROCESSMODEFILTER);
                    SetFieldValues(ControlEnums.PROCESSMODEHDR);
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlEnums.LIST);
                    SetFieldValues(ControlEnums.LIST);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region ResetForm
        /// <summary>
        /// Method used to Reset Form Controls
        /// </summary>
        private void ResetForm(ControlEnums controlType)
        {
            int Branch = 0;
            int Dept = 0;
            switch (controlType)
            {
                case ControlEnums.CLEAR:
                    //txtDesignation.Text = string.Empty;
                    //hdfDesignation.Value = string.Empty;
                    if (!int.TryParse(hdfHdrBranchLocation.Value, out Branch) && Branch <= 0)
                    {
                        txtBranchLocation.Text = string.Empty;
                        hdfBranchLocation.Value = "-1";
                    }
                    if (!int.TryParse(hdfDepartment.Value, out Dept) && Dept <= 0)
                    {
                        txtDepartment.Text = string.Empty;
                        hdfDepartment.Value = "-1";
                    }

                    //ddlEmploymentType.SelectedIndex = ddlCompany.SelectedIndex = 0;
                    //ddlEmployee.SelectedIndex = 0;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = "-1";
                    dtResult = null;
                    //BindGrid(ControlEnums.GETATTENDANCE);                   
                    break;
                case ControlEnums.CLEARSEARCH:
                    txtFilterFromDate.Text = txtFilterToDate.Text = txtFilterBranch.Text = txtFilterDept.Text =txtFilterTrxNo.Text=txtEmpSearch.Text = string.Empty;
                    hdfFilterBranch.Value = hdfFilterDept.Value = hdfFilterTrxNo.Value = hdfEmpSearch.Value =string.Empty;
                    ddlFilterProcessMode.ClearSelection();
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    this.EntryStatus = EntryStatus.LISTMODE;
                    this.CurrPK = 0;
                    break;
                case ControlEnums.CLEARHDR:
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    txtHdrBranchLocation.Text = txtHdrDepartment.Text = txtRemarks.Text = txtDate.Text = string.Empty;
                    hdfHdrBranchLocation.Value = hdfHdrDepartment.Value = string.Empty;
                    ddlProcessModeHdr.ClearSelection();
                    //txtDesignation.Text = string.Empty;
                    //hdfDesignation.Value = string.Empty;
                    txtBranchLocation.Text = string.Empty;
                    hdfBranchLocation.Value = "-1";
                    txtDepartment.Text = string.Empty;
                    hdfDepartment.Value = "-1";
                    //ddlEmploymentType.SelectedIndex = ddlCompany.SelectedIndex = 0;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = "-1";
                    dtResult = null;
                    txtDetailDate.Text = string.Empty;
                    break;
                #region CLEARADD
                case ControlEnums.CLEARADD:
                    if (!int.TryParse(hdfHdrBranchLocation.Value, out Branch) && Branch <= 0)
                    {
                        txtBranchLocation.Text = string.Empty;
                        hdfBranchLocation.Value = "-1";
                    }
                    if (!int.TryParse(hdfHdrDepartment.Value, out Dept) && Dept <= 0)
                    {
                        txtDepartment.Text = string.Empty;
                        hdfDepartment.Value = "-1";
                    }
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = string.Empty;
                    txtInTime.Text = string.Empty;
                    txtBOutTime1.Text = string.Empty;
                    txtBInTime1.Text = string.Empty;
                    txtBOutTime2.Text = string.Empty;
                    txtBInTime2.Text = string.Empty;
                    txtOutTime.Text = string.Empty;
                    txtTotalHours.Text = string.Empty;
                    txtNormalHours.Text = string.Empty;
                    txtOTHours.Text = string.Empty;
                    txtLateHrs.Text = string.Empty;
                    hdfEmpWorkHours.Value = string.Empty;
                    hdfEmpHasOTFromPunching.Value = string.Empty;
                    chkStatus.Checked = true;
                    txtDetRemarks.Text = string.Empty;
                    //RowIndex = -1;
                    SlNo = -1;
                    break;
                #endregion
                case ControlEnums.CLEARIMPORTHDR:
                    txtImportDate.Text = string.Empty;
                    chkOverwrite.Checked = false;
                    txtImportRemarks.Text = string.Empty;
                    break;

                case ControlEnums.DTLCLEARSEARCH:
                    hdfEmpSearch.Value = "0";
                    txtBrLocDtlSearch.Text = txtEmpSearch.Text = string.Empty;
                    hdfBrLocDtlSearch.Value = hdfEmpSearch.Value = string.Empty;
                    ddlStatus.ClearSelection();
                    break;
            }
        }
        #endregion



        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlEnums type)
        {
            ERPService.AdmCompanyMstService admCompanyMstServiceClient;
            ERPManager.ServiceUtility serviceUtilityObj;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                FilterParameters objFilterParam;
                switch (type)
                {
                    #region GETATTENDANCE
                    case ControlEnums.GETATTENDANCE:
                        ////objFilterParam = new FilterParameters();
                        ////objFilterParam.ToDate = objFilterParam.FromDate = string.IsNullOrEmpty(txtDate.Text) ? (DateTime?)null : DateTime.Parse(txtDate.Text);
                        ////objFilterParam.PK = CurrPK;
                        ////objFilterParam.Active = 1;
                        ////objFilterParam.BizUnit = currentUser.SBUID;

                        //////objFilterParam.Designation = GetNullableInt(hdfDesignation.Value) > 0 ? GetNullableInt(hdfDesignation.Value) : null;
                        ////objFilterParam.BranchLocation = GetNullableInt(hdfBranchLocation.Value) > 0 ? GetNullableInt(hdfBranchLocation.Value) : null;
                        ////objFilterParam.Department = GetNullableInt(hdfDepartment.Value) > 0 ? GetNullableInt(hdfDepartment.Value) : null;
                        //////objFilterParam.EmployeeType = GetNullableInt(ddlEmploymentType.SelectedValue) > 0 ? GetNullableInt(ddlEmploymentType.SelectedValue) : null;
                        //////objFilterParam.Company = GetNullableInt(ddlCompany.SelectedValue) > 0 ? GetNullableInt(ddlCompany.SelectedValue) : null;
                        ////objFilterParam.Employee = GetNullableInt(hdfEmployee.Value) > 0 ? GetNullableInt(hdfEmployee.Value) : null;
                        ////objFilterParam.EmployeeCategory = (int)EmployeeCategory.HRMSEmployee;
                        ////employeeAttendance = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendance(objFilterParam);
                        ////if (employeeAttendance != null)
                        ////    AttendanceEntryGridViewList = employeeAttendance.AttnDetails.ToList();

                        //List<AttendancePopupDetails> tempList = new List<AttendancePopupDetails>();
                        //foreach (DataRow row in dtResult.Rows)
                        //{
                        //    AttendancePopupDetails attendance = new AttendancePopupDetails();
                        //    attendance.SlNo = 0;
                        //    attendance.Pk = GetNullableInt(Convert.ToString(row["EAT_PK"]));
                        //    attendance.EmpPk = GetNullableInt(Convert.ToString(row["EAT_EMPLOYEE_PK"])).Value;
                        //    attendance.WorkingHrs = GetNullableDecimal(Convert.ToString(row["EAT_WORK_HRS"]));
                        //    attendance.DateDt = GetNullableDate(Convert.ToString(row["EAT_DATE"]));
                        //    //attendance.EmpCode = (Convert.ToString(row["EAT_EMPLOYEE_TEXT"])).HtmlDecode();
                        //    //attendance.EmpName = (Convert.ToString(row["EAT_EMPLOYEE_NAME"])).HtmlDecode();
                        //    attendance.EmpName = (Convert.ToString(row["EAT_EMPLOYEE_TEXT"])).HtmlDecode();
                        //    attendance.Location = (Convert.ToString(row["empBranchText"])).HtmlDecode();
                        //    attendance.LocationCode = (Convert.ToString(row["empBranchCode"])).HtmlDecode();
                        //    attendance.EmpDepartment = (Convert.ToString(row["empDepartmentText"])).HtmlDecode();
                        //    attendance.EmpDepartmentCode = (Convert.ToString(row["DPT_CODE"])).HtmlDecode();
                        //    attendance.EmpDesignation = (Convert.ToString(row["empDesignationText"])).HtmlDecode();
                        //    attendance.InDt = GetNullableDate(Convert.ToString(row["EAT_IN_TIME"]));
                        //    attendance.OutDt = GetNullableDate(Convert.ToString(row["EAT_OUT_TIME"]));
                        //    //attendance.TotalDt = GetNullableDate(Convert.ToString(row["EAT_NORMAL_HRS"]));
                        //    //attendance.TotalHrs = GetNullableDecimal(Convert.ToString(row["EAT_NORMAL_HRS"]));
                        //    attendance.TotalDt = GetNullableDate(Convert.ToString(row["EAT_WORKED_HRS"]));
                        //    attendance.TotalHrs = GetNullableDecimal(Convert.ToString(row["EAT_WORKED_HRS"]));
                        //    attendance.ShortHrs = GetNullableDecimal(Convert.ToString(row["EAT_SHORT_HRS"]));
                        //    attendance.OTDtHrs = GetNullableDecimal(Convert.ToString(row["EAT_OT_HRS"]));
                        //    attendance.ModifiedDate = Convert.ToString(row["EAT_MOD_DT"]);
                        //    attendance.CheckedFlag = 0;
                        //    tempList.Add(attendance);
                        //}
                        //AttendanceEntryGridViewList = tempList;
                        break;
                    #endregion
                    #region EMPLOYMENTTYPE
                    case ControlEnums.EMPLOYMENTTYPE:
                        int commonPK = 0;
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentType(commonPK);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlEnums.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region EMPLOYEEDETAIL
                    case ControlEnums.EMPLOYEEDETAIL:
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmployeeDetailListHeader(EmpPk,
                            string.Empty, GetNullableInt(CommonConstants.HASPK).Value);
                        break;
                    #endregion
                    #region WORK HRS
                    case ControlEnums.WORKHRS:
                        EmpWorkHrs = BusinessLogic.HRMS.Payroll.AttendanceBL.GetEmpWorkHour(EmpPk, AttendanceDate);
                        break;
                    #endregion
                    #region ATTENDANCE TEMPLATE
                    case ControlEnums.ATTENDANCETEMPLATE:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "ATTENDANCE TEMPLATE");
                        break;
                    #endregion
                    #region LIST
                    case ControlEnums.LIST:
                        objFilterParam = new FilterParameters();
                        objFilterParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        objFilterParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtFilterFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtFilterToDate.Text) ? (DateTime?)null : DateTime.Parse(txtFilterToDate.Text);
                        objFilterParam.BizUnit = currentUser.SBUID;
                        objFilterParam.Department = string.IsNullOrEmpty(hdfFilterDept.Value) ? (int?)null : Convert.ToInt32(hdfFilterDept.Value);
                        objFilterParam.Employee = string.IsNullOrEmpty(hdfEmpSearch.Value) ? (int?)null : Convert.ToInt32(hdfEmpSearch.Value);
                        objFilterParam.BranchLocation = string.IsNullOrEmpty(hdfFilterBranch.Value) ? (int?)null : Convert.ToInt32(hdfFilterBranch.Value);
                        objFilterParam.TransactionNo = string.IsNullOrEmpty(hdfFilterTrxNo.Value) ? (int?)null : Convert.ToInt32(hdfFilterTrxNo.Value);
                        objFilterParam.Active = (int)DbActiveStatus.ACTIVE;
                        objFilterParam.ProcessMode = Convert.ToInt32(ddlFilterProcessMode.SelectedValue) > 0 ? Convert.ToInt32(ddlFilterProcessMode.SelectedValue) : (int?)null;
                        dtList = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendanceList(objFilterParam, currentUser.SBUID);
                        break;
                    #endregion
                    #region GET ATTENDANCE BY PK
                    case ControlEnums.GETATTENDANCEBYPK:
                        objFilterParam = new FilterParameters();
                        objFilterParam.ToDate = objFilterParam.FromDate = string.IsNullOrEmpty(txtDate.Text) ? (DateTime?)null : DateTime.Parse(txtDate.Text);
                        objFilterParam.PK = CurrPK;
                        objFilterParam.Active = 1;
                        objFilterParam.BizUnit = currentUser.SBUID;

                        //objFilterParam.Designation = GetNullableInt(hdfDesignation.Value) > 0 ? GetNullableInt(hdfDesignation.Value) : null;
                        objFilterParam.BranchLocation = GetNullableInt(hdfBranchLocation.Value) > 0 ? GetNullableInt(hdfBranchLocation.Value) : null;
                        objFilterParam.Department = GetNullableInt(hdfDepartment.Value) > 0 ? GetNullableInt(hdfDepartment.Value) : null;
                        //objFilterParam.EmployeeType = GetNullableInt(ddlEmploymentType.SelectedValue) > 0 ? GetNullableInt(ddlEmploymentType.SelectedValue) : null;
                        //objFilterParam.Company = GetNullableInt(ddlCompany.SelectedValue) > 0 ? GetNullableInt(ddlCompany.SelectedValue) : null;
                        objFilterParam.Employee = GetNullableInt(hdfEmployee.Value) > 0 ? GetNullableInt(hdfEmployee.Value) : null;
                        objFilterParam.EmployeeCategory = (int)EmployeeCategory.HRMSEmployee;
                        employeeAttendance = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendanceDetails(objFilterParam);
                        if (employeeAttendance != null)
                            AttendanceEntryGridViewList = employeeAttendance.AttnDetails.ToList();

                        break;
                    #endregion
                    #region VIEW ATTENDANCE DETAILS
                    case ControlEnums.VIEWATTENDANCEDETAILS:
                        dtAttendanceDetails = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendanceDetails(attendancePk);
                        if (dtAttendanceDetails.Rows.Count > 0)
                        {
                            if (grdEmpAttndance.Rows.Count == 0)
                            {
                                lblEmpNameCode.Text = string.Empty;
                            }
                            string code = dtAttendanceDetails.Rows[0]["empCode"].ToString();
                            string name = dtAttendanceDetails.Rows[0]["empName"].ToString();
                            lblEmpNameCode.Text = "<b>Employee : " + code + " - " + name + "</b>";
                        }
                        break;
                    #endregion
                    #region PROCESS MODE
                    case ControlEnums.PROCESSMODE:
                        dtResult = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "PAYROLL PROCESS MODE");
                        break;
                    #endregion

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Set Field Values
        /// <summary>
        /// All Field(Input controls, grids, dropdowns) values are assigned here.
        /// To Set all fields, pass "string.Empty()"
        /// </summary>
        private void SetFieldValues(ControlEnums controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlEnums.GETATTENDANCE:
                        BindGrid(ControlEnums.GETATTENDANCE);
                        break;
                    case ControlEnums.EMPLOYMENTTYPE:
                        BindDropDown(ControlEnums.EMPLOYMENTTYPE);
                        break;
                    case ControlEnums.COMPANY:
                        BindDropDown(ControlEnums.COMPANY);
                        break;
                    case ControlEnums.ATTENDANCETEMPLATE:
                        BindDropDown(ControlEnums.ATTENDANCETEMPLATE);
                        break;
                    case ControlEnums.LIST:
                        BindGrid(ControlEnums.LIST);
                        break;
                    case ControlEnums.ATTENDANCEHDR:
                        GetUIValuesFromObject(ControlEnums.ATTENDANCEHDR);
                        break;
                    case ControlEnums.VIEWATTENDANCEDETAILS:
                        BindGrid(ControlEnums.VIEWATTENDANCEDETAILS);
                        break;
                    case ControlEnums.GRIDEDIT:
                        GetUIValuesFromObject(ControlEnums.GRIDEDIT);
                        break;
                    case ControlEnums.GRIDDELETE:
                        GetUIValuesFromObject(ControlEnums.GRIDDELETE);
                        break;
                    case ControlEnums.PROCESSMODE:
                        BindDropDown(ControlEnums.PROCESSMODE);
                        break;
                    case ControlEnums.PROCESSMODEFILTER:
                        BindDropDown(ControlEnums.PROCESSMODEFILTER);
                        break;
                    case ControlEnums.PROCESSMODEHDR:
                        BindDropDown(ControlEnums.PROCESSMODEHDR);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region BindGrid
        /// <summary>
        /// This method is used to Binding Grids
        /// </summary>
        /// <param name="controlType"></param>
        private void BindGrid(ControlEnums controlType)
        {
            switch (controlType)
            {
                #region GET ATTENDANCE
                case ControlEnums.GETATTENDANCE:
                    int BrLoc = 0;
                    int Dept = 0;
                    int Status = -1;
                    int.TryParse(hdfBrLocDtlSearch.Value, out BrLoc);
                    int.TryParse(hdfEmpSearch.Value, out Dept);
                    int.TryParse(ddlStatus.SelectedValue, out Status);
                    grdEmployeeAttendance.DataSource = AttendanceEntryGridViewList.Where(r =>
                                                        r.LocationPk == (BrLoc > 0 ? BrLoc.ToString() : r.LocationPk)
                                                        && r.EmpPk == (Dept > 0 ? Dept : r.EmpPk)
                                                        && r.EAT_STATUS == (Status >= 0 ? Status : r.EAT_STATUS)
                                                        );
                    grdEmployeeAttendance.DataBind();
                    break;
                #endregion
                #region LIST
                case ControlEnums.LIST:
                    int rowCount = 0;
                    int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                    if (dtList.Rows.Count > 0)
                    {
                        rowCount = Convert.ToInt32(dtList.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                    }
                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                  (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                  (rowCount / pageSize) + 1;
                    PageIndexList = PageIndexList == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexList;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndexList);
                    grdList.DataSource = dtList;
                    grdList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
                #endregion
                #region VIEW ATTENDANCE DETAILS
                case ControlEnums.VIEWATTENDANCEDETAILS:
                    grdEmpAttndance.DataSource = dtAttendanceDetails;
                    grdEmpAttndance.DataBind();
                    break;
                #endregion
            }
        }
        #endregion

        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlEnums controlType)
        {
            Object retObject;
            retObject = null;
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                switch (controlType)
                {
                    #region ATTENDANCEDETAILS
                    case ControlEnums.ATTENDANCEDETAILS:
                        employeeAttendance = new EmployeeAttendance();
                        employeeAttendance.EAR_BRANCH = string.IsNullOrEmpty(hdfHdrBranchLocation.Value) ? null : hdfHdrBranchLocation.Value;
                        employeeAttendance.EAR_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                        employeeAttendance.EAR_EMPDEPARTMENT = string.IsNullOrEmpty(hdfHdrDepartment.Value) ? null : hdfHdrDepartment.Value;
                        employeeAttendance.EAR_FROM_DATE = Convert.ToDateTime(txtDate.Text);
                        employeeAttendance.EAR_TO_DATE = Convert.ToDateTime(txtDate.Text);
                        employeeAttendance.EAR_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        employeeAttendance.EAR_PK = CurrPK;
                        employeeAttendance.EAR_IN_FL = 0;
                        employeeAttendance.EAR_DEPT = currentUser.CurrentDeptPK;
                        employeeAttendance.BIZUNIT_PK = currentUser.SBUID;
                        employeeAttendance.ACTIVE = (int)DbActiveStatus.ACTIVE;
                        employeeAttendance.USER_PK = currentUser.PKUser;
                        employeeAttendance.LAST_MOD_DT = LastModifiedTime;
                        employeeAttendance.EAR_NO = lblTrxNo.Text;
                        employeeAttendance.IS_DEL_FL = 1;
                        employeeAttendance.EAR_ATT_MODE = Convert.ToInt32(ddlProcessModeHdr.SelectedValue);
                        //employeeAttendance.AttnDetails = (List<AttendancePopupDetails>)SetUIValuesToObject(ControlEnums.ATTENDANCELIST);//GetAttendanceDetailsFromUI();
                        employeeAttendance.AttnDetails = AttendanceEntryGridViewList;
                        retObject = employeeAttendance;
                        break;
                    #endregion
                    #region ATTENDANCE LIST
                    case ControlEnums.ATTENDANCELIST:
                        List<AttendancePopupDetails> AttendanceList = new List<AttendancePopupDetails>();
                        foreach (GridViewRow grdrow in grdEmployeeAttendance.Rows)
                        {
                            AttendancePopupDetails AttendanceEntry = new AttendancePopupDetails();
                            //CheckBox chkgrdStatus = (CheckBox)grdrow.FindControl("chkgrdStatus");
                            Label lblInTime = (Label)grdrow.FindControl("lblInTime");
                            Label lblBOutTime1 = (Label)grdrow.FindControl("lblBOutTime1");
                            Label lblBInTime1 = (Label)grdrow.FindControl("lblBInTime1");
                            Label lblBOutTime2 = (Label)grdrow.FindControl("lblBOutTime2");
                            Label lblBInTime2 = (Label)grdrow.FindControl("lblBInTime2");
                            Label lblOutTime = (Label)grdrow.FindControl("lblOutTime");
                            Label lblTotalHrs = (Label)grdrow.FindControl("lblTotalHrs");
                            Label lblNormalHrs = (Label)grdrow.FindControl("lblNormalHrs");
                            Label lblOTHrs = (Label)grdrow.FindControl("lblOTHrs");
                            Label lblLateHrs = (Label)grdrow.FindControl("lblLateHrs");
                            Label lblGrdRemarks = (Label)grdrow.FindControl("lblGrdRemarks");
                            HiddenField hdfWorkHours = (HiddenField)grdrow.FindControl("hdfWorkHours");
                            HiddenField hdfEAT_PK = (HiddenField)grdrow.FindControl("hdfEAT_PK");
                            HiddenField hdfEmployeePk = (HiddenField)grdrow.FindControl("hdfEmployeePk");
                            HiddenField hdfIsUpdate = (HiddenField)grdrow.FindControl("hdfIsUpdate");
                            UpdateFlag = 0;
                            totalDays = 0;
                            int.TryParse(hdfIsUpdate.Value, out UpdateFlag);
                            AttendanceEntry.Pk = GetNullableInt(((HiddenField)grdrow.FindControl("hdfEAT_PK")).Value) ?? 0;

                            date = txtDate.Text;
                            if (!string.IsNullOrEmpty(lblInTime.Text.Trim()))
                            {
                                AttendanceEntry.InDt = date;
                                TimeSpan.TryParse(lblInTime.Text.Trim(), out timeSpanIn);
                                AttendanceEntry.InDt = (Convert.ToDateTime(AttendanceEntry.InDt).Add(timeSpanIn)).ToString();
                            }
                            else
                                AttendanceEntry.InDt = null;
                            if (!string.IsNullOrEmpty(lblBOutTime1.Text.Trim()))
                            {
                                AttendanceEntry.BOut1Dt = date;
                                TimeSpan.TryParse(lblBOutTime1.Text.Trim(), out timeSpanBOut1);
                                AttendanceEntry.BOut1Dt = (Convert.ToDateTime(AttendanceEntry.BOut1Dt).Add(timeSpanBOut1)).ToString();
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BOut1Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BOut1Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.BOut1Dt = Convert.ToDateTime(AttendanceEntry.BOut1Dt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.BOut1Dt = Convert.ToDateTime(AttendanceEntry.BOut1Dt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.BOut1Dt = null;
                            if (!string.IsNullOrEmpty(lblBInTime1.Text.Trim()))
                            {
                                AttendanceEntry.BIn1Dt = date;
                                TimeSpan.TryParse(lblBInTime1.Text.Trim(), out timeSpanBIn1);
                                AttendanceEntry.BIn1Dt = (Convert.ToDateTime(AttendanceEntry.BIn1Dt).Add(timeSpanBIn1)).ToString();
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BIn1Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.BOut1Dt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BIn1Dt) - Convert.ToDateTime(AttendanceEntry.BOut1Dt)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.BIn1Dt = Convert.ToDateTime(AttendanceEntry.BIn1Dt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.BIn1Dt = Convert.ToDateTime(AttendanceEntry.BIn1Dt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.BIn1Dt = null;
                            if (!string.IsNullOrEmpty(lblBOutTime2.Text.Trim()))
                            {
                                AttendanceEntry.BOut2Dt = date;
                                TimeSpan.TryParse(lblBOutTime2.Text.Trim(), out timeSpanBOut2);
                                AttendanceEntry.BOut2Dt = (Convert.ToDateTime(AttendanceEntry.BOut2Dt).Add(timeSpanBOut2)).ToString();
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BOut2Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.BIn1Dt) || !string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    prevDate = !string.IsNullOrEmpty(AttendanceEntry.BIn1Dt) ? AttendanceEntry.BIn1Dt : AttendanceEntry.InDt;
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BOut2Dt) - Convert.ToDateTime(prevDate)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.BOut2Dt = Convert.ToDateTime(AttendanceEntry.BOut2Dt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.BOut2Dt = Convert.ToDateTime(AttendanceEntry.BOut2Dt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.BOut2Dt = null;
                            if (!string.IsNullOrEmpty(lblBInTime2.Text.Trim()))
                            {
                                AttendanceEntry.BIn2Dt = date;
                                TimeSpan.TryParse(lblBInTime2.Text.Trim(), out timeSpanBIn2);
                                AttendanceEntry.BIn2Dt = (Convert.ToDateTime(AttendanceEntry.BIn2Dt).Add(timeSpanBIn2)).ToString();
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BIn2Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.BOut2Dt) || !string.IsNullOrEmpty(AttendanceEntry.BOut1Dt))
                                {
                                    prevDate = !string.IsNullOrEmpty(AttendanceEntry.BOut2Dt) ? AttendanceEntry.BOut2Dt : AttendanceEntry.BOut1Dt;
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BIn2Dt) - Convert.ToDateTime(prevDate)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.BIn2Dt = Convert.ToDateTime(AttendanceEntry.BIn2Dt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.BIn2Dt = Convert.ToDateTime(AttendanceEntry.BIn2Dt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.BIn2Dt = null;
                            if (!string.IsNullOrEmpty(lblOutTime.Text.Trim()))
                            {
                                AttendanceEntry.OutDt = date;
                                TimeSpan.TryParse(lblOutTime.Text.Trim(), out timeSpanOut);
                                AttendanceEntry.OutDt = (Convert.ToDateTime(AttendanceEntry.OutDt).Add(timeSpanOut)).ToString();
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.OutDt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.BIn2Dt) || !string.IsNullOrEmpty(AttendanceEntry.BIn1Dt) || !string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (!string.IsNullOrEmpty(AttendanceEntry.BIn2Dt))
                                        prevDate = AttendanceEntry.BIn2Dt;
                                    else if (!string.IsNullOrEmpty(AttendanceEntry.BIn1Dt))
                                        prevDate = AttendanceEntry.BIn1Dt;
                                    else
                                        prevDate = AttendanceEntry.InDt;
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.OutDt) - Convert.ToDateTime(prevDate)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.OutDt = Convert.ToDateTime(AttendanceEntry.OutDt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.OutDt = Convert.ToDateTime(AttendanceEntry.OutDt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.OutDt = null;
                            AttendanceEntry.TotalHrs = ConvertTimeStringToDouble(lblTotalHrs.Text.Trim());
                            AttendanceEntry.NormalHrs = ConvertTimeStringToDouble(lblNormalHrs.Text.Trim());
                            AttendanceEntry.OTDtHrs = ConvertTimeStringToDouble(lblOTHrs.Text.Trim());
                            AttendanceEntry.ShortHrs = ConvertTimeStringToDouble(lblLateHrs.Text.Trim());
                            AttendanceEntry.WorkingHrs = GetNullableDecimal(hdfWorkHours.Value).ToString();
                            AttendanceEntry.EmpPk = Convert.ToInt32(hdfEmployeePk.Value);
                            AttendanceEntry.DateDt = Convert.ToDateTime(txtDetailDate.Text);
                            //AttendanceEntry.EAT_STATUS = chkgrdStatus.Checked ? (int)DbActiveStatus.ACTIVE : (int)DbActiveStatus.INACTIVE;
                            AttendanceEntry.Remarks = string.IsNullOrEmpty(lblGrdRemarks.ToolTip.Trim()) ? null : HttpUtility.HtmlEncode(lblGrdRemarks.ToolTip.Trim());
                            AttendanceEntry.UpdateFlag = UpdateFlag;
                            if (!string.IsNullOrEmpty(AttendanceEntry.BOut1Dt) && !string.IsNullOrEmpty(AttendanceEntry.BIn1Dt))
                            {
                                TimeSpan dtBreak1 = Convert.ToDateTime(AttendanceEntry.BIn1Dt) - Convert.ToDateTime(AttendanceEntry.BOut1Dt);
                                if (GetDecimalFromTimeSpan(dtBreak1) > 0)
                                    AttendanceEntry.EAT_BREAK1 = GetTime(GetDecimalFromTimeSpan(dtBreak1).ToString());
                                else
                                    AttendanceEntry.EAT_BREAK1 = GetLocalResourceObject("ZeroTime").ToString();
                            }
                            else
                                AttendanceEntry.EAT_BREAK1 = GetLocalResourceObject("ZeroTime").ToString();

                            if (!string.IsNullOrEmpty(AttendanceEntry.BOut2Dt) && !string.IsNullOrEmpty(AttendanceEntry.BIn2Dt))
                            {
                                TimeSpan dtBreak2 = Convert.ToDateTime(AttendanceEntry.BIn2Dt) - Convert.ToDateTime(AttendanceEntry.BOut2Dt);
                                if (GetDecimalFromTimeSpan(dtBreak2) > 0)
                                    AttendanceEntry.EAT_BREAK2 = GetTime(GetDecimalFromTimeSpan(dtBreak2).ToString());
                                else
                                    AttendanceEntry.EAT_BREAK2 = GetLocalResourceObject("ZeroTime").ToString();
                            }
                            else
                                AttendanceEntry.EAT_BREAK2 = GetLocalResourceObject("ZeroTime").ToString();
                            if (!string.IsNullOrEmpty(AttendanceEntry.InDt) && !string.IsNullOrEmpty(AttendanceEntry.OutDt))
                                AttendanceList.Add(AttendanceEntry);

                        }
                        retObject = AttendanceList;
                        break;
                    #endregion
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region "GetUIValuesFromObject"
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlEnums controlType)
        {
            try
            {

                switch (controlType)
                {
                    #region ATTENDANCE HDR
                    case ControlEnums.ATTENDANCEHDR:
                        if (employeeAttendance != null)
                        {
                            lblTrxNo.Text = string.IsNullOrEmpty(employeeAttendance.EAR_NO) ? Resources.ErpRes.Draft : employeeAttendance.EAR_NO;
                            txtDate.Text = employeeAttendance.EAR_TO_DATE.ToString(Resources.Constants.HRMSDateFormatShort);
                            txtHdrBranchLocation.Text = txtBranchLocation.Text = HttpUtility.HtmlDecode(employeeAttendance.EAR_BRANCH_TEXT);
                            hdfHdrBranchLocation.Value = hdfBranchLocation.Value = employeeAttendance.EAR_BRANCH;
                            txtHdrDepartment.Text = txtDepartment.Text = HttpUtility.HtmlDecode(employeeAttendance.EAR_EMPDEPARTMENT_TEXT);
                            hdfHdrDepartment.Value = hdfDepartment.Value = employeeAttendance.EAR_EMPDEPARTMENT;
                            //hdfHdrDepartmentCode.Value = hdfDepartmentCode.Value = employeeAttendance.EAR_EMPDEPARTMENT_c;
                            txtRemarks.Text = HttpUtility.HtmlDecode(employeeAttendance.EAR_REMARKS);
                            if (employeeAttendance.EAR_ATT_MODE > 0)
                                ddlProcessModeHdr.SelectedValue = employeeAttendance.EAR_ATT_MODE.ToString();
                            LastModifiedTime = employeeAttendance.LAST_MOD_DT;
                            CompanyPk = employeeAttendance.EAR_COMPANY;
                            GetFieldValues(ControlEnums.COMPANY);
                            SetFieldValues(ControlEnums.COMPANY);
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(CompanyPk.ToString()));
                        }
                        break;
                    #endregion
                    #region GRID EDIT
                    case ControlEnums.GRIDEDIT:
                        if (AttendanceEntryGridViewList != null && AttendanceEntryGridViewList.Count > 0 && SlNo > 0)
                        {
                            //AttendancePopupDetails objDet = AttendanceEntryGridViewList[RowIndex];
                            AttendancePopupDetails objDet = AttendanceEntryGridViewList.SingleOrDefault(r => r.SlNo == SlNo);
                            if (objDet != null)
                            {
                                //txtBranchLocation.Text = HttpUtility.HtmlDecode(objDet.Location);
                                //hdfBranchLocation.Value = objDet.LocationPk;
                                //hdfBranchLocationCode.Value = HttpUtility.HtmlDecode(objDet.LocationCode);
                                //txtDepartment.Text = HttpUtility.HtmlDecode(objDet.EmpDepartment);
                                //hdfDepartment.Value = objDet.EmpDeptPk;
                                //hdfDepartmentCode.Value = HttpUtility.HtmlDecode(objDet.EmpDepartmentCode);
                                txtEmployee.Text = HttpUtility.HtmlDecode(objDet.EmpCode);
                                hdfEmployee.Value = objDet.EmpPk.ToString();
                                txtDetRemarks.Text = HttpUtility.HtmlDecode(objDet.Remarks);
                                chkStatus.Checked = objDet.EAT_STATUS > 0 ? true : false;

                                txtInTime.Text = GetTimeSpan(objDet.InDt);
                                txtBOutTime1.Text = GetTimeSpan(objDet.BOut1Dt);
                                txtBInTime1.Text = GetTimeSpan(objDet.BIn1Dt);
                                txtBOutTime2.Text = GetTimeSpan(objDet.BOut2Dt);
                                txtBInTime2.Text = GetTimeSpan(objDet.BIn2Dt);
                                txtOutTime.Text = GetTimeSpan(objDet.OutDt);

                                txtTotalHours.Text = GetTime(Convert.ToString(objDet.TotalHrs));
                                txtNormalHours.Text = GetTime(Convert.ToString(objDet.NormalHrs));
                                txtOTHours.Text = GetTime(Convert.ToString(objDet.OTDtHrs));
                                txtLateHrs.Text = GetTime(Convert.ToString(objDet.ShortHrs));
                                hdfEmpWorkHours.Value = objDet.WorkingHrs;
                                hdfEmpBreakHours.Value = objDet.BreakHrs;
                                hdfEmpHasOTFromPunching.Value = objDet.HasOTFromPunching.ToString();

                                txtDepartment.Text = HttpUtility.HtmlDecode(objDet.EmpDepartment);
                                hdfDepartment.Value = objDet.EmpDeptPk;
                                txtBranchLocation.Text = HttpUtility.HtmlDecode(objDet.Location);
                                hdfBranchLocation.Value = objDet.LocationPk;

                                txtDetailDate.Text = objDet.DateDt.ToString(Resources.Constants.HRMSDateFormatShort);
                            }
                        }
                        break;
                    #endregion
                    #region GRID DELETE
                    case ControlEnums.GRIDDELETE:
                        if (AttendanceEntryGridViewList != null && AttendanceEntryGridViewList.Count > 0 && SlNo > 0)
                        {
                            List<AttendancePopupDetails> objTempDetalisList = AttendanceEntryGridViewList;
                            //AttendancePopupDetails objTemp = AttendanceEntryGridViewList[RowIndex];
                            AttendancePopupDetails objTemp = AttendanceEntryGridViewList.SingleOrDefault(r => r.SlNo == SlNo);
                            if (objTemp != null)
                            {
                                objTempDetalisList.Remove(objTemp);
                                AttendanceEntryGridViewList = objTempDetalisList;
                            }
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
        /// <summary>
        /// This method is used to Binding DropDoowns
        /// </summary>
        /// <param name="controlType"></param>
        private void BindDropDown(ControlEnums controlType)
        {
            switch (controlType)
            {
                #region EMPLOYMENTTYPE
                case ControlEnums.EMPLOYMENTTYPE:
                    //ddlEmploymentType.Items.Clear();
                    //if (dtResult != null && dtResult.Rows.Count > 0)
                    //{
                    //    ddlEmploymentType.DataSource = dtResult;
                    //    ddlEmploymentType.DataTextField = "CON_NAME";
                    //    ddlEmploymentType.DataValueField = "CON_PK";
                    //    ddlEmploymentType.DataBind();
                    //    ddlEmploymentType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    //    ddlEmploymentType.Items.HtmlDecode();
                    //    //ddlEmploymentTypePopUp.DataSource = dtResult;
                    //    //ddlEmploymentTypePopUp.DataTextField = "CON_NAME";
                    //    //ddlEmploymentTypePopUp.DataValueField = "CON_PK";
                    //    //ddlEmploymentTypePopUp.DataBind();
                    //    //ddlEmploymentTypePopUp.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

                    //}
                    break;
                #endregion
                #region COMPANY
                case ControlEnums.COMPANY:
                    ddlCompany.Items.Clear();
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
                #region ATTENDANCE TEMPLATE
                case ControlEnums.ATTENDANCETEMPLATE:
                    ddlAttnTemplate.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlAttnTemplate.DataSource = dtResult;
                        ddlAttnTemplate.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlAttnTemplate.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlAttnTemplate.DataBind();
                        //ddlAttnTemplate.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        ddlAttnTemplate.Items.HtmlDecode();
                    }
                    break;
                #endregion
                #region PROCESS MODE
                case ControlEnums.PROCESSMODE:
                    ddlProcessMode.Items.Clear();
                    ddlProcessMode.DataSource = dtResult;
                    ddlProcessMode.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                    ddlProcessMode.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                    ddlProcessMode.DataBind();
                    ddlProcessMode.Items.HtmlDecode();
                    //ddlProcessMode.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PROCESS MODE FILTER
                case ControlEnums.PROCESSMODEFILTER:
                    ddlFilterProcessMode.Items.Clear();
                    ddlFilterProcessMode.DataSource = dtResult;
                    ddlFilterProcessMode.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                    ddlFilterProcessMode.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                    ddlFilterProcessMode.DataBind();
                    ddlFilterProcessMode.Items.HtmlDecode();
                    ddlFilterProcessMode.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region PROCESS MODE HDR
                case ControlEnums.PROCESSMODEHDR:
                    ddlProcessModeHdr.Items.Clear();
                    ddlProcessModeHdr.DataSource = dtResult;
                    ddlProcessModeHdr.DataTextField = GTIService.Constants.Common.Fields.ADM_CFG_TEXT;
                    ddlProcessModeHdr.DataValueField = GTIService.Constants.Common.Fields.ADM_CFG_VALUE;
                    ddlProcessModeHdr.DataBind();
                    ddlProcessModeHdr.Items.HtmlDecode();
                    ddlProcessModeHdr.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
            }
        }
        #endregion
        #endregion

        #region Configuration Settings
        private void ConfigurationSettings()
        {
            DataTable dtConfig = new DataTable();
            dtConfig = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("HRMS SETTINGS", "ENTRY MODE");
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                hdfAttnEntryMode.Value = dtConfig.Rows[0]["ACF_VALUE"].ToString(); //1:Daily, 2:Cumulative                
            }
            dtConfig = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("HRMS SETTINGS", "TOTAL WORK HOUR");
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                hdfTotalHrsCalcMode.Value = dtConfig.Rows[0]["ACF_VALUE"].ToString(); //1:First in and Last Out, 2:Actual in time only           
            }

            dtConfig = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("HRMS DAILY SHIFT", string.Empty);
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                hdfShiftStartTime.Value = dtConfig.Rows[0]["ACF_DATA"].ToString(); //Shift start time   
                hdfAdditionHrs.Value = dtConfig.Rows[0]["ACF_SPEC_COND"].ToString(); //Addition hours
            }
        }

        #endregion

        #region HelperMethods

        public List<AttendancePopupDetails> GetAttendanceDetailsFromUI()
        {
            List<AttendancePopupDetails> attnDetList = new List<AttendancePopupDetails>();
            AttendancePopupDetails attnDet;
            TimeSpan timeSpanIn;
            TimeSpan timeSpanOut;
            foreach (GridViewRow grdrow in grdEmployeeAttendance.Rows)
            {
                CheckBox chk;
                chk = (CheckBox)grdrow.FindControl("chkSelect");
                if (chk.Checked)
                {

                    string date = txtDate.Text;
                    attnDet = new AttendancePopupDetails();
                    attnDet.Pk = GetNullableInt(((HiddenField)grdrow.FindControl("hdfEAT_PK")).Value) ?? 0;
                    attnDet.DateDt = Convert.ToDateTime(txtDate.Text);
                    attnDet.EmpPk = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmployeePk")).Value);

                    string inTimeText = ((TextBox)grdrow.FindControl("txtInTime")).Text;
                    attnDet.InDt = date; //txtDate.Text);
                    TimeSpan.TryParse(inTimeText, out timeSpanIn);
                    attnDet.InDt = Convert.ToDateTime(attnDet.InDt).Add(timeSpanIn).ToString();

                    string outTimeText = ((TextBox)grdrow.FindControl("txtOutTime")).Text;
                    attnDet.OutDt = date; //txtDate.Text);
                    TimeSpan.TryParse(outTimeText, out timeSpanOut);
                    attnDet.OutDt = Convert.ToDateTime(attnDet.OutDt).Add(timeSpanOut).ToString();

                    attnDet.TotalHrs = ConvertTimeStringToDouble(((TextBox)grdrow.FindControl("txtTotalHours")).Text.Trim());
                    attnDet.ShortHrs = ConvertTimeStringToDouble(((TextBox)grdrow.FindControl("txtShortHours")).Text.Trim());
                    attnDet.OTDtHrs = ConvertTimeStringToDouble(((TextBox)grdrow.FindControl("txtOTHours")).Text.Trim());

                    //attnDet.EAT_NORMAL_HRS = GetNullableDecimal(((HiddenField)grdrow.FindControl("hdfTotalHours")).Value);                   
                    //attnDet.EAT_SHORT_HRS = GetNullableDecimal(((HiddenField)grdrow.FindControl("hdfShortHours")).Value);                   
                    //attnDet.EAT_OT_HRS = GetNullableDecimal(((HiddenField)grdrow.FindControl("hdfOTHours")).Value);
                    attnDetList.Add(attnDet);

                }
            }
            return attnDetList;
        }

        private int? GetNullableInt(string str)
        {
            int result;
            if (int.TryParse(str, out result))
            {
                return (int?)result;
            }
            return null;
        }

        private decimal? GetNullableDecimal(string str)
        {
            decimal result;
            if (decimal.TryParse(str, out result))
            {
                return (decimal?)result;
            }
            return null;
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

        private DateTime? GetNullableDate(string str)
        {
            DateTime result;
            if (DateTime.TryParse(str, out result))
            {
                return (DateTime?)result;
            }
            return null;
        }

        /// <summary>
        /// This Methode is Used to Formating DateTime to Time Span
        /// </summary>
        /// <returns></returns>
        public string GetTimeSpan(object pdt)
        {
            if (string.IsNullOrEmpty(Convert.ToString(pdt)))
                return string.Empty;
            DateTime dt;
            try
            {
                dt = Convert.ToDateTime(pdt);
                return dt.ToString(CommonConstants.TIMEFORMAT);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string GetTime(string time)
        {
            string result = string.Empty;
            if (time == string.Empty) return result;

            string[] arr = time.Split('.');
            string h = arr[0];
            string m = string.Empty.PadRight(2, '0');
            if (arr.Length == 2)
            {
                if (arr[1].Length > 1) m = arr[1].Substring(0, 2);
                else m = arr[1].PadRight(2, '0');
            }
            decimal mm = Convert.ToDecimal(m);
            mm = Math.Round(((mm * 60) / 100));
            m = mm.ToString();
            if (h.Length < 2) h = "0" + h;
            if (m.Length < 2) m = "0" + m;
            result = h + ":" + m;
            return result;
        }

        private decimal GetDecimalFromTimeSpan(TimeSpan tms)
        {
            decimal result = 0;
            result = (tms.Minutes / Convert.ToDecimal(60)) + Convert.ToDecimal(tms.Hours);
            return Math.Round(result, 2);
        }

        /// <summary>
        /// Convert time string to deciam
        /// </summary>
        /// <param name="hourminutes">hh:mm or hhh:mm</param>
        /// <returns></returns>
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
                    result = (minutes / Convert.ToDouble(60)) + Convert.ToDouble(hours);
                    result = Math.Round(result, 2);
                }
                return result;
            }
            catch
            {
                return result;
            }
        }

        /// <summary>
        /// Methode used to save the excel file
        /// </summary>
        private string SaveDetails(out string conStr, FileUpload fupUpload, int Template)
        {

            //uploadInfo = new FileInfo(fupImport.PostedFile.FileName);
            //extns = uploadInfo.Extension;
            uploadPath = string.Empty;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
            {
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\AttendanceImports";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\AttendanceImports\\";
            }
            else
            {
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "AttendanceImports";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "AttendanceImports\\";
            }

            FileInfo tempFileInfoObj;
            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
            string attachmentFileFormat = tempFileInfoObj.Extension;

            string FileName = Guid.NewGuid().ToString() + attachmentFileFormat;
            conStr = CheckValidFileType(attachmentFileFormat);
            //int Template = Convert.ToInt32(ddlAttnTemplate.SelectedValue);
            if (Template == (int)AttendanceTemplateType.IGCL_TXT)
            {
                if (attachmentFileFormat.ToLower() != ".txt")
                {
                    return string.Empty;
                }
            }
            else //if (Template == (int)AttendanceTemplateType.GENERAL || Template == (int)AttendanceTemplateType.IGCL)
            {
                if (attachmentFileFormat.ToLower() != ".xls" && attachmentFileFormat.ToLower() != ".xlsx")
                {
                    return string.Empty;
                }
            }


            if ((!string.IsNullOrEmpty(conStr)) || (attachmentFileFormat.ToLower() == ".txt"))
            {
                fupUpload.SaveAs(uploadPath + FileName);
            }

            return uploadPath + FileName;

            //////uploadPath = ConfigurationManager.AppSettings["UploadPath"];
            //////uploadPath = uploadPath.Replace("~/", "");
            //////if (uploadPath[uploadPath.Length - 1] == '/') uploadPath = uploadPath.Remove(uploadPath.Length - 1, 1);
            //////applicationPath = HttpContext.Current.Request.PhysicalApplicationPath;

            ////uploadName = GenerateFileName(uploadInfo.Name) + extns;
            //////attchInfo = new FileInfo(applicationPath + uploadPath + "\\" + uploadName);
            ////attchInfo = new FileInfo(uploadPath + uploadName);
            ////conStr = CheckValidFileType(extns);
            ////if (!string.IsNullOrEmpty(conStr))
            ////{
            ////    //if (CheckFolderExists(applicationPath + uploadPath))
            ////    //{
            ////    fupImport.SaveAs(attchInfo.FullName);
            ////    //}
            ////}
            ////return attchInfo.FullName;
        }

        /// <summary>
        /// Methode used to generate the new file name 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GenerateFileName(string fileName)
        {
            string uploadName;
            string[] fileDtls;
            fileDtls = new string[2];
            fileDtls = fileName.Split('.');
            uploadName = fileDtls[0] + Guid.NewGuid().ToString();
            return uploadName;
        }

        /// <summary>
        /// Check File is valid or not , using File Extension , if valid then get the connection string
        /// </summary>
        /// <param name="extn"></param>
        /// <returns>bool : True - valid File, false - Invalid File</returns>
        private string CheckValidFileType(string extn)
        {
            string conStr;
            switch (extn.ToLower())
            {
                case ".xls": //Excel 97-03
                    conStr = ConfigurationManager.AppSettings["Excel03ConString"];
                    break;
                case ".xlsx": //Excel 07
                    conStr = ConfigurationManager.AppSettings["Excel07ConString"];
                    break;
                default: conStr = string.Empty; break;
            }
            return conStr;
        }

        /// <summary>
        /// Method to Check Folder Exists or not
        /// </summary>
        /// <param name="uploadUrl"></param>
        /// <returns>Bool</returns>
        public bool CheckFolderExists(string uploadUrl)
        {
            if (System.IO.Directory.Exists(uploadUrl))
                return true;
            else
                return false;

        }

        /// <summary>
        /// Methode used to read the excel data and save into grid
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="conStr"></param>
        private void ImportDetails(string filePath, string conStr)
        {
            try
            {
                int Pk = 0;
                string SheetName = GetLocalResourceObject("ExcelSheetName").ToString().ToLower(); // CommonConstants.EXELSHEETNAME.ToLower();
                excelColumns = GetLocalResourceObject("ExcelColumns").ToString().Split(',');
                conStr = String.Format(conStr, filePath);
                connExcel = new OleDbConnection(conStr);
                cmdExcel = new OleDbCommand();
                oleDbDataAdapter = new OleDbDataAdapter();
                dtExcelSchema = new DataTable();
                dsImportedAttendance = new DataSet();
                cmdExcel.Connection = connExcel;
                connExcel.Open();
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null && dtExcelSchema.Rows.Count > 0)
                {
                    string field1 = GetLocalResourceObject("ExcelColumnForEmptyCheck").ToString().Trim();// "EmpCode";
                    if (dtExcelSchema != null)
                    {
                        landingSheet = string.Empty;
                        foreach (DataRow dr in dtExcelSchema.Rows)
                        {
                            if (dr["TABLE_NAME"].ToString().ToLower() == SheetName.ToLower())
                            {
                                landingSheet = dr["TABLE_NAME"].ToString();
                            }
                        }
                        if (landingSheet == string.Empty)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_ExelsheetName").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, SheetName.Replace("$", ""));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                    }
                    cmdExcel.CommandText = "SELECT * From [" + landingSheet + "] where [" + field1 + "]<> Null ";
                    oleDbDataAdapter.SelectCommand = cmdExcel;
                    oleDbDataAdapter.Fill(dsImportedAttendance, "Landing");
                    connExcel.Close();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    throw new Exception(GetLocalResourceObject("Err_IncorrectFormat").ToString());
                }
                if (dsImportedAttendance != null && dsImportedAttendance.Tables.Count > 0)
                {

                    foreach (DataColumn item in dsImportedAttendance.Tables[0].Columns)
                    {
                        item.ColumnName = item.ColumnName.Replace(" ", "");
                    }
                    int cnt = (from p in excelColumns
                               where this.dsImportedAttendance.Tables[0].Columns.Contains(p)
                               select p).Count();
                    if (cnt != excelColumns.Length)
                    {
                        sb = new StringBuilder();
                        sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                        if (excelColumns != null && excelColumns.Count() > 0)
                        {
                            foreach (string item in excelColumns)
                            {
                                sb.Append("<ul><li>" + item + "</li></ul>");
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString())
                                           + "','" + Resources.ErpRes.Information + "');", true);
                        return;
                    }

                    DataTable dtImportData = dsImportedAttendance.Tables[0];
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                    employeeAttendance = new EmployeeAttendance();
                    employeeAttendance.EAR_BRANCH = string.IsNullOrEmpty(hdfHdrBranchLocation.Value) ? null : hdfHdrBranchLocation.Value;
                    GetFieldValues(ControlEnums.COMPANY);
                    employeeAttendance.EAR_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    employeeAttendance.EAR_EMPDEPARTMENT = string.IsNullOrEmpty(hdfHdrDepartment.Value) ? null : hdfHdrDepartment.Value;
                    employeeAttendance.EAR_FROM_DATE = Convert.ToDateTime(txtDate.Text);
                    employeeAttendance.EAR_TO_DATE = Convert.ToDateTime(txtDate.Text);
                    employeeAttendance.EAR_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                    employeeAttendance.EAR_PK = CurrPK;
                    employeeAttendance.EAR_IN_FL = 0;
                    employeeAttendance.EAR_DEPT = currentUser.CurrentDeptPK;
                    employeeAttendance.BIZUNIT_PK = currentUser.SBUID;
                    employeeAttendance.ACTIVE = (int)DbActiveStatus.ACTIVE;
                    employeeAttendance.USER_PK = currentUser.PKUser;
                    employeeAttendance.LAST_MOD_DT = LastModifiedTime;
                    employeeAttendance.EAR_NO = lblTrxNo.Text;
                    employeeAttendance.IS_DEL_FL = 1;
                    employeeAttendance.EAR_ATT_MODE = Convert.ToInt32(ddlProcessModeHdr.SelectedValue);
                    DateTime tempDateTime;
                    List<AttendancePopupDetails> importDetList = new List<AttendancePopupDetails>();
                    DataColumnCollection importColumns = dtImportData.Columns;
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        totalDays = 0;
                        AttendancePopupDetails AttendanceEntry = new AttendancePopupDetails();
                        string strType = string.Empty;
                        string date1 = string.Empty;// txtDate.Text;
                        if (importColumns.Contains("Date") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Date"]).Trim()))
                        {
                            date1 = Convert.ToDateTime(dtImportData.Rows[i]["Date"]).ToString(Resources.Constants.HRMSDateFormatShort);
                            AttendanceEntry.DateDt = Convert.ToDateTime(dtImportData.Rows[i]["Date"]);
                        }

                        if (!string.IsNullOrEmpty(date1.Trim()))
                        {
                            date = date1;
                            if (importColumns.Contains("IN") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["IN"]).Trim()))
                            {
                                AttendanceEntry.InDt = date;
                                TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["IN"]).Trim(), out timeSpanIn);
                                if (timeSpanIn.Hours == 0 && timeSpanIn.Minutes == 0 && Convert.ToString(dtImportData.Rows[i]["IN"]).Trim().Split(' ').Count() > 1)
                                {
                                    tempDateTime = Convert.ToDateTime(dtImportData.Rows[i]["IN"]);
                                    timeSpanIn = tempDateTime.TimeOfDay;
                                    //TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["IN"]).Trim().Split(' ')[1], out timeSpanIn);
                                }
                                AttendanceEntry.InDt = (Convert.ToDateTime(AttendanceEntry.InDt).Add(timeSpanIn)).ToString();

                            }
                            else
                                AttendanceEntry.InDt = null;

                            if (importColumns.Contains("B-OUT1") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["B-OUT1"]).Trim()))
                            {
                                AttendanceEntry.BOut1Dt = date;
                                TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["B-OUT1"]).Trim(), out timeSpanBOut1);
                                if (timeSpanBOut1.Hours == 0 && timeSpanBOut1.Minutes == 0 && Convert.ToString(dtImportData.Rows[i]["B-OUT1"]).Trim().Split(' ').Count() > 1)
                                {
                                    tempDateTime = Convert.ToDateTime(dtImportData.Rows[i]["B-OUT1"]);
                                    timeSpanBOut1 = tempDateTime.TimeOfDay;
                                    //TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["B-OUT1"]).Trim().Split(' ')[1], out timeSpanBOut1);
                                }
                                AttendanceEntry.BOut1Dt = (Convert.ToDateTime(AttendanceEntry.BOut1Dt).Add(timeSpanBOut1)).ToString();
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BOut1Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BOut1Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.BOut1Dt = Convert.ToDateTime(AttendanceEntry.BOut1Dt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.BOut1Dt = Convert.ToDateTime(AttendanceEntry.BOut1Dt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.BOut1Dt = null;
                            if (importColumns.Contains("B-IN1") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["B-IN1"]).Trim()))
                            {
                                AttendanceEntry.BIn1Dt = date;
                                TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["B-IN1"]).Trim(), out timeSpanBIn1);
                                if (timeSpanBIn1.Hours == 0 && timeSpanBIn1.Minutes == 0 && Convert.ToString(dtImportData.Rows[i]["B-IN1"]).Trim().Split(' ').Count() > 1)
                                {
                                    tempDateTime = Convert.ToDateTime(dtImportData.Rows[i]["B-IN1"]);
                                    timeSpanBIn1 = tempDateTime.TimeOfDay;
                                    //TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["B-IN1"]).Trim().Split(' ')[1], out timeSpanBIn1);
                                }
                                AttendanceEntry.BIn1Dt = (Convert.ToDateTime(AttendanceEntry.BIn1Dt).Add(timeSpanBIn1)).ToString();

                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BIn1Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.BOut1Dt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BIn1Dt) - Convert.ToDateTime(AttendanceEntry.BOut1Dt)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.BIn1Dt = Convert.ToDateTime(AttendanceEntry.BIn1Dt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.BIn1Dt = Convert.ToDateTime(AttendanceEntry.BIn1Dt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.BIn1Dt = null;
                            if (importColumns.Contains("B-OUT2") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["B-OUT2"]).Trim()))
                            {
                                AttendanceEntry.BOut2Dt = date;
                                TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["B-OUT2"]).Trim(), out timeSpanBOut2);
                                if (timeSpanBOut2.Hours == 0 && timeSpanBOut2.Minutes == 0 && Convert.ToString(dtImportData.Rows[i]["B-OUT2"]).Trim().Split(' ').Count() > 1)
                                {
                                    tempDateTime = Convert.ToDateTime(dtImportData.Rows[i]["B-OUT2"]);
                                    timeSpanBOut2 = tempDateTime.TimeOfDay;
                                    //TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["B-OUT2"]).Trim().Split(' ')[1], out timeSpanBOut2);
                                }
                                AttendanceEntry.BOut2Dt = (Convert.ToDateTime(AttendanceEntry.BOut2Dt).Add(timeSpanBOut2)).ToString();

                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BOut2Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.BIn1Dt) || !string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    prevDate = !string.IsNullOrEmpty(AttendanceEntry.BIn1Dt) ? AttendanceEntry.BIn1Dt : AttendanceEntry.InDt;
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BOut2Dt) - Convert.ToDateTime(prevDate)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.BOut2Dt = Convert.ToDateTime(AttendanceEntry.BOut2Dt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.BOut2Dt = Convert.ToDateTime(AttendanceEntry.BOut2Dt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.BOut2Dt = null;
                            if (importColumns.Contains("B-IN2") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["B-IN2"]).Trim()))
                            {
                                AttendanceEntry.BIn2Dt = date;
                                TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["B-IN2"]).Trim(), out timeSpanBIn2);
                                if (timeSpanBIn2.Hours == 0 && timeSpanBIn2.Minutes == 0 && Convert.ToString(dtImportData.Rows[i]["B-IN2"]).Trim().Split(' ').Count() > 1)
                                {
                                    tempDateTime = Convert.ToDateTime(dtImportData.Rows[i]["B-IN2"]);
                                    timeSpanBIn2 = tempDateTime.TimeOfDay;
                                    //TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["B-IN2"]).Trim().Split(' ')[1], out timeSpanBIn2);
                                }
                                AttendanceEntry.BIn2Dt = (Convert.ToDateTime(AttendanceEntry.BIn2Dt).Add(timeSpanBIn2)).ToString();

                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BIn2Dt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.BOut2Dt) || !string.IsNullOrEmpty(AttendanceEntry.BOut1Dt))
                                {
                                    prevDate = !string.IsNullOrEmpty(AttendanceEntry.BOut2Dt) ? AttendanceEntry.BOut2Dt : AttendanceEntry.BOut1Dt;
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.BIn2Dt) - Convert.ToDateTime(prevDate)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.BIn2Dt = Convert.ToDateTime(AttendanceEntry.BIn2Dt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.BIn2Dt = Convert.ToDateTime(AttendanceEntry.BIn2Dt).AddDays(1).ToString();
                            }
                            else
                                AttendanceEntry.BIn2Dt = null;
                            if (importColumns.Contains("OUT") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["OUT"]).Trim()))
                            {
                                AttendanceEntry.OutDt = date;
                                TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["OUT"]).Trim(), out timeSpanOut);
                                if (timeSpanOut.Hours == 0 && timeSpanOut.Minutes == 0 && Convert.ToString(dtImportData.Rows[i]["OUT"]).Trim().Split(' ').Count() > 1)
                                {
                                    tempDateTime = Convert.ToDateTime(dtImportData.Rows[i]["OUT"]);
                                    timeSpanOut = tempDateTime.TimeOfDay;
                                    //TimeSpan.TryParse(Convert.ToString(dtImportData.Rows[i]["OUT"]).Trim().Split(' ')[1], out timeSpanOut);
                                }
                                AttendanceEntry.OutDt = (Convert.ToDateTime(AttendanceEntry.OutDt).Add(timeSpanOut)).ToString();
                                if (!string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.OutDt) - Convert.ToDateTime(AttendanceEntry.InDt)).TotalDays;
                                }
                                if (!string.IsNullOrEmpty(AttendanceEntry.BIn2Dt) || !string.IsNullOrEmpty(AttendanceEntry.BIn1Dt) || !string.IsNullOrEmpty(AttendanceEntry.InDt))
                                {
                                    if (!string.IsNullOrEmpty(AttendanceEntry.BIn2Dt))
                                        prevDate = AttendanceEntry.BIn2Dt;
                                    else if (!string.IsNullOrEmpty(AttendanceEntry.BIn1Dt))
                                        prevDate = AttendanceEntry.BIn1Dt;
                                    else
                                        prevDate = AttendanceEntry.InDt;
                                    if (totalDays >= 0)
                                        totalDays = (Convert.ToDateTime(AttendanceEntry.OutDt) - Convert.ToDateTime(prevDate)).TotalDays;
                                    //if (totalDays < 0)
                                    //    AttendanceEntry.OutDt = Convert.ToDateTime(AttendanceEntry.OutDt).AddDays(1).ToString();
                                }
                                if (totalDays < 0)
                                    AttendanceEntry.OutDt = Convert.ToDateTime(AttendanceEntry.OutDt).AddDays(1).ToString();

                            }
                            else
                                AttendanceEntry.OutDt = null;
                            if (importColumns.Contains("Total") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Total"]).Trim()))
                            {
                                if (Convert.ToString(dtImportData.Rows[i]["Total"]).Trim().Split(' ').Count() > 1)
                                    AttendanceEntry.TotalHrs = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Total"]).Trim().Split(' ')[1]);
                                else
                                    AttendanceEntry.TotalHrs = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Total"]).Trim());
                            }
                            else
                                AttendanceEntry.TotalHrs = 0;
                            if (importColumns.Contains("Normal") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Normal"]).Trim()))
                            {
                                if (Convert.ToString(dtImportData.Rows[i]["Normal"]).Trim().Split(' ').Count() > 1)
                                    AttendanceEntry.NormalHrs = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Normal"]).Trim().Split(' ')[1]);
                                else
                                    AttendanceEntry.NormalHrs = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Normal"]).Trim());
                            }
                            else
                                AttendanceEntry.NormalHrs = 0;
                            if (importColumns.Contains("OT") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["OT"]).Trim()))
                            {
                                if (Convert.ToString(dtImportData.Rows[i]["OT"]).Trim().Split(' ').Count() > 1)
                                    AttendanceEntry.OTDtHrs = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["OT"]).Trim().Split(' ')[1]);
                                else
                                    AttendanceEntry.OTDtHrs = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["OT"]).Trim());
                            }
                            else
                                AttendanceEntry.OTDtHrs = 0;
                            if (importColumns.Contains("Late") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Late"]).Trim()))
                            {
                                if (Convert.ToString(dtImportData.Rows[i]["Late"]).Trim().Split(' ').Count() > 1)
                                    AttendanceEntry.ShortHrs = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Late"]).Trim().Split(' ')[1]);
                                else
                                    AttendanceEntry.ShortHrs = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Late"]).Trim());
                            }
                            else
                                AttendanceEntry.ShortHrs = 0;
                            if (importColumns.Contains("Break1") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Break1"]).Trim()))
                            {
                                if (Convert.ToString(dtImportData.Rows[i]["Break1"]).Trim().Split(' ').Count() > 1)
                                    AttendanceEntry.Break1 = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Break1"]).Trim().Split(' ')[1]);
                                else
                                    AttendanceEntry.Break1 = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Break1"]).Trim());
                            }
                            else
                                AttendanceEntry.Break1 = 0;
                            if (importColumns.Contains("Break2") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Break2"]).Trim()))
                            {
                                if (Convert.ToString(dtImportData.Rows[i]["Break2"]).Trim().Split(' ').Count() > 1)
                                    AttendanceEntry.Break2 = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Break2"]).Trim().Split(' ')[1]);
                                else
                                    AttendanceEntry.Break2 = ConvertTimeStringToDouble(Convert.ToString(dtImportData.Rows[i]["Break2"]).Trim());
                            }
                            else
                                AttendanceEntry.Break2 = 0;

                            // AttendanceEntry.WorkingHrs = GetNullableDecimal(hdfWorkHours.Value).ToString();
                            if (importColumns.Contains("l") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["l"]).Trim()))
                            {
                                Pk = 0;
                                int.TryParse(Convert.ToString(dtImportData.Rows[i]["l"]), out Pk);
                                AttendanceEntry.Pk = Pk;
                            }
                            else
                                AttendanceEntry.Pk = 0;
                            //AttendanceEntry.DateDt = Convert.ToDateTime(txtDate.Text);
                            if (importColumns.Contains("Status") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Status"]).Trim()))
                                AttendanceEntry.EAT_STATUS = Convert.ToString(dtImportData.Rows[i]["Status"]).Trim().ToLower().Equals("ok") ? (int)DbActiveStatus.ACTIVE : (int)DbActiveStatus.INACTIVE;
                            else
                                AttendanceEntry.EAT_STATUS = (int)DbActiveStatus.INACTIVE;
                            if (importColumns.Contains("Remarks") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Remarks"]).Trim()))
                                AttendanceEntry.Remarks = HttpUtility.HtmlEncode(Convert.ToString(dtImportData.Rows[i]["Remarks"]).Trim());
                            else
                                AttendanceEntry.Remarks = null;
                            if (importColumns.Contains("EmpCode") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["EmpCode"]).Trim()))
                                AttendanceEntry.EmpCode = Convert.ToString(dtImportData.Rows[i]["EmpCode"]).Trim();
                            else
                                AttendanceEntry.EmpCode = null;
                            if (importColumns.Contains("Recalculate") && !string.IsNullOrEmpty(Convert.ToString(dtImportData.Rows[i]["Recalculate"]).Trim()))
                            {
                                string Recalculate = Convert.ToString(dtImportData.Rows[i]["Recalculate"]).Trim().ToLower();
                                AttendanceEntry.RecalculateFlag = Recalculate.Equals("yes") ? 1 : 0;
                            }
                            else
                                AttendanceEntry.RecalculateFlag = 0;
                            AttendanceEntry.UpdateFlag = 0;
                            if (!string.IsNullOrEmpty(AttendanceEntry.EmpCode))
                                importDetList.Add(AttendanceEntry);

                        }
                    }

                    employeeAttendance.AttnDetails = importDetList;
                    string EmpName = string.Empty;
                    string trxNo = string.Empty;
                    employeeAttendance.WKF_FLAG = 1;
                    xmlLanding = (CommonFunctions.ObjectTOXmlForPrdPlan(employeeAttendance)).InnerXml;
                    int result = BusinessLogic.HRMS.Payroll.AttendanceBL.ImportAttendanceDetails(xmlLanding, out EmpName, out trxNo);
                    if (result > 0)
                    {
                        //lblTrxNo.Text = trxNo;
                        litErrorMsg.Text = GetLocalResourceObject("AttendanceImported").ToString();
                        //object[] args = new object[2];
                        //args[0] = Resources.PageNameRes.Attendance;
                        //args[1] = trxNo;
                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                        ResetForm(ControlEnums.CLEARADD);
                        hdfShowHideFilterSec.Value = "0";
                        EntryStatus = EntryStatus.EDITMODE;
                        txtDate.Enabled = false;
                        GetFieldValues(ControlEnums.GETATTENDANCEBYPK);
                        SetFieldValues(ControlEnums.ATTENDANCEHDR);
                        SetFieldValues(ControlEnums.GETATTENDANCE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);

                    }
                    else
                    {
                        ResetForm(ControlEnums.CLEARADD);                       
                        EntryStatus = EntryStatus.EDITMODE;                        
                        GetFieldValues(ControlEnums.GETATTENDANCEBYPK);
                        SetFieldValues(ControlEnums.ATTENDANCEHDR);
                        SetFieldValues(ControlEnums.GETATTENDANCE);

                        if (result == (int)DbSaveStatus.SQLERROR)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_IncorrectFile").ToString(); //Resources.Messages.ActionFailedPleaseTryAgain;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbSaveStatus.CONCURRENCY)
                        {
                            litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.EditUsedByAnotherUser;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                        {
                            litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.AlreadyDeleted;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        else if (result == (int)DbSaveStatus.CODEEXIST || result == (int)DbSaveStatus.SHIFTEXIST)
                        {
                            if (string.IsNullOrEmpty(EmpName))
                                litErrorMsg.Text = GetLocalResourceObject("AlreadyExist").ToString(); //Resources.PageNameRes.Attendance + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                            else
                                litErrorMsg.Text = GetLocalResourceObject("AlreadyAdded").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        else if (SaveDbEnum.EMPLOYEENOTFOUND == (SaveDbEnum)(result))
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_EmployeeNotFound").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (SaveDbEnum.HDRDIFF == (SaveDbEnum)(result))
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_diffHdr").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (SaveDbEnum.EMPWITHDIFFPROCESSMODE == (SaveDbEnum)(result))
                        {
                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_EmpWithDifferentPrcMode").ToString(), ddlProcessModeHdr.SelectedItem.Text) + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "','" + "');", true);
                        }
                        else if (SaveDbEnum.INVALIDATTENDANCE == (SaveDbEnum)(result))
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_InvalidAttendanceImport").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (SaveDbEnum.DIFFDEPT == (SaveDbEnum)(result))
                        {
                            litErrorMsg.Text = string.Format(this.GetLocalResourceObject("Err_EmpDiffDeptImport").ToString() , txtHdrDepartment.Text) + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                //sb = new StringBuilder();
                //sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                //foreach (string item in excelColumns)
                //{
                //    sb.Append("<ul><li>" + item + "</li></ul>");
                //}
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);

            }
            catch (Exception ex)
            {
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                //litErrorMsg.Text = this.GetLocalResourceObject("Err_IncorrectExcel").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        /// <summary>
        /// Methode used to read the excel data and save into grid
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="conStr"></param>
        private void ImportToGrid(string filePath, string conStr)
        {
            try
            {
                string SheetName = CommonConstants.EXELSHEETNAME.ToLower();
                excelColumns = airColums_General;
                if (Convert.ToInt32(ddlAttnTemplate.SelectedValue) == (int)AttendanceTemplateType.IGCL)
                {
                    excelColumns = airColums_IGCL;
                    SheetName = CommonConstants.EXELSHEETNAME_IGCL.ToLower();
                }
                conStr = String.Format(conStr, filePath);
                connExcel = new OleDbConnection(conStr);
                cmdExcel = new OleDbCommand();
                oleDbDataAdapter = new OleDbDataAdapter();
                dtExcelSchema = new DataTable();
                dsImportedAttendance = new DataSet();

                cmdExcel.Connection = connExcel;
                connExcel.Open();
                dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dtExcelSchema != null && dtExcelSchema.Rows.Count > 0)
                {
                    if (dtExcelSchema != null)
                    {
                        landingSheet = string.Empty;
                        foreach (DataRow dr in dtExcelSchema.Rows)
                        {
                            if (dr["TABLE_NAME"].ToString().ToLower() == SheetName.ToLower())
                            {
                                landingSheet = dr["TABLE_NAME"].ToString();
                            }
                        }
                        if (landingSheet == string.Empty)
                        {
                            litErrorMsg.Text = this.GetLocalResourceObject("Err_ExelsheetName").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, SheetName.Replace("$", ""));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                    }
                    cmdExcel.CommandText = "SELECT * From [" + landingSheet + "]";
                    oleDbDataAdapter.SelectCommand = cmdExcel;
                    oleDbDataAdapter.Fill(dsImportedAttendance, "Landing");
                    connExcel.Close();
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                else
                {
                    throw new Exception(GetLocalResourceObject("Err_IncorrectFormat").ToString());
                }
                if (dsImportedAttendance != null && dsImportedAttendance.Tables.Count > 0)
                {

                    foreach (DataColumn item in dsImportedAttendance.Tables[0].Columns)
                    {
                        item.ColumnName = item.ColumnName.Replace(" ", "");
                    }
                    int cnt = (from p in excelColumns
                               where this.dsImportedAttendance.Tables[0].Columns.Contains(p)
                               select p).Count();
                    if (cnt != excelColumns.Length)
                    {
                        sb = new StringBuilder();
                        sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                        if (excelColumns != null && excelColumns.Count() > 0)
                        {
                            foreach (string item in excelColumns)
                            {
                                sb.Append("<ul><li>" + item + "</li></ul>");
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString())
                                           + "','" + Resources.ErpRes.Information + "');", true);
                        return;
                    }

                    DataTable dtImportData = dsImportedAttendance.Tables[0];
                    #region DataValidation for General Template
                    if (Convert.ToInt32(ddlAttnTemplate.SelectedValue) == (int)AttendanceTemplateType.GENERAL)
                    {
                        int validationFlag = 1;
                        validationFlag = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "AttendanceValidation").ToString());
                        for (int i = 0; i < dtImportData.Rows.Count; i++)
                        {
                            string date = Convert.ToString(dtImportData.Rows[i]["Date"]);
                            string empId = Convert.ToString(dtImportData.Rows[i]["EmpID"]);

                            if (!string.IsNullOrEmpty(date.Trim()) && !string.IsNullOrEmpty(empId.Trim()))
                            {
                                if (string.IsNullOrWhiteSpace(date))
                                {
                                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_ExcelFieldRequired").ToString(), "Date", (i + 2));
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }
                                if (string.IsNullOrWhiteSpace(empId))
                                {
                                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_ExcelFieldRequired").ToString(), "Emp ID", (i + 2));
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    return;
                                }
                                switch (validationFlag)
                                {
                                    case 1: // InTime & OutTime are required                               
                                        string inTime = Convert.ToString(dtImportData.Rows[i]["FirstIn"]);
                                        string outTime = Convert.ToString(dtImportData.Rows[i]["LastOut"]);

                                        DateTime inDt;
                                        DateTime outDt;
                                        if (string.IsNullOrWhiteSpace(inTime))
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_ExcelFieldRequired").ToString(), "First In", (i + 2));
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }

                                        inDt = Convert.ToDateTime(inTime);
                                        if (string.IsNullOrWhiteSpace(outTime))
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_ExcelFieldRequired").ToString(), "Last Out", (i + 2));
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                        outDt = Convert.ToDateTime(outTime);
                                        if ((inDt == null || outDt == null) || (inDt >= outDt))
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_InvalidInTimeOutTime").ToString(), (i + 2));
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                        break;
                                    case 2: // TotalHrs required
                                        string totalHrs = Convert.ToString(dtImportData.Rows[i]["TotalHrs"]);
                                        if (string.IsNullOrWhiteSpace(totalHrs))
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_ExcelFieldRequired").ToString(), "Total Hrs", (i + 2));
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }

                                        decimal temp;
                                        if (!decimal.TryParse(totalHrs, out temp))
                                        {
                                            litErrorMsg.Text = string.Format(GetLocalResourceObject("Err_InvalidTotalTime").ToString(), (i + 2));
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                    #endregion DataValidation

                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                    AttendanceImport attendanceImport = new AttendanceImport();
                    //attendanceImport.ImportData = CommonFunctions.HtmlEncodeDataTable(dt, "BrandName");
                    attendanceImport.IS_UPDATE = 1;
                    attendanceImport.BIZUNIT_PK = currentUser.SBUID;
                    attendanceImport.USER_PK = currentUser.PKUser;
                    //attendanceImport.IS_UPDATE = chkUpdateImportAttendance.Checked ? 1 : 0;
                    attendanceImport.TEMPLATE_TYPE = Convert.ToInt32(ddlAttnTemplate.SelectedValue);
                    attendanceImport.EAR_REMARKS = HttpUtility.HtmlEncode(txtImportRemarks.Text);
                    attendanceImport.EAR_ACTIVE = (int)DbActiveStatus.ACTIVE;
                    attendanceImport.EAR_IN_FL = 1;
                    GetFieldValues(ControlEnums.COMPANY);
                    attendanceImport.EAR_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    List<BusinessObject.HRMS.Payroll.AttendanceImportDetail> importDet = new List<AttendanceImportDetail>();
                    DataColumnCollection importColumns = dtImportData.Columns;
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        DateTime attnDate = new DateTime();
                        string strType = string.Empty;
                        string date1 = Convert.ToString(dtImportData.Rows[i]["Date"]);
                        if (!string.IsNullOrEmpty(date1.Trim()))
                        {
                            //int sl = Convert.ToInt32(dtImportData.Rows[i]["SlNo"]);
                            int sl = i + 1;
                            string empid = string.Empty;// = Convert.ToString(dtImportData.Rows[i]["EmpID"]);
                            //string dtInTime = Convert.ToString(dt.Rows[i]["FirstIn"]);
                            //string dtOutTime = Convert.ToString(dt.Rows[i]["LastOut"]);
                            //string dtNormalTime = Convert.ToString(dt.Rows[i]["TotalHrs"]);
                            DateTime attDate = Convert.ToDateTime(date1);

                            DateTime? dtInTime = null;
                            DateTime? dtOutTime = null;
                            DateTime tempDt = new DateTime();
                            decimal normalHrs = 0;
                            DateTime.TryParse((dtImportData.Rows[i]["Date"]).ToString(), out attnDate);

                            if (importColumns.Contains("ID"))
                            {
                                empid = Convert.ToString(dtImportData.Rows[i]["ID"]).Trim();
                            }
                            if (importColumns.Contains("EmpID"))
                            {
                                empid = Convert.ToString(dtImportData.Rows[i]["EmpID"]).Trim();
                            }
                            if (importColumns.Contains("FirstIn") && DateTime.TryParse((dtImportData.Rows[i]["FirstIn"]).ToString(), out tempDt))
                            {
                                dtInTime = tempDt;
                                dtInTime = attDate.Add(tempDt.TimeOfDay);
                                tempDt = new DateTime();
                            }
                            if (importColumns.Contains("LastOut") && DateTime.TryParse((dtImportData.Rows[i]["LastOut"]).ToString(), out tempDt))
                            {
                                dtOutTime = tempDt;
                                dtOutTime = attDate.Add(tempDt.TimeOfDay);
                                tempDt = new DateTime();
                            }
                            if (importColumns.Contains("TotalHrs") && decimal.TryParse((dtImportData.Rows[i]["TotalHrs"]).ToString(), out normalHrs))
                            {
                                //dtNormalTime = tempDt;
                                ////dtNormalTime = attDate.Add(tempDt.TimeOfDay);    
                                //tempDt = new DateTime();

                                //normalHrs = GetDecimalFromTimeSpan(tempDt.TimeOfDay);
                            }
                            if (importColumns.Contains("Type"))
                                strType = Convert.ToString(dtImportData.Rows[i]["Type"]);

                            BusinessObject.HRMS.Payroll.AttendanceImportDetail objAttendanceDetail = new AttendanceImportDetail();
                            objAttendanceDetail.EAT_DATE = attnDate.ToString();
                            objAttendanceDetail.EAT_SL_NO = sl;
                            objAttendanceDetail.EAT_EMPLOYEE = empid;
                            objAttendanceDetail.EAT_IN_TIME = dtInTime.ToString();
                            if (Convert.ToInt32(ddlAttnTemplate.SelectedValue) == (int)AttendanceTemplateType.IGCL)
                                objAttendanceDetail.EAT_IN_TIME = attnDate.ToString();
                            objAttendanceDetail.EAT_OUT_TIME = dtOutTime.ToString();
                            objAttendanceDetail.EAT_NORMAL_HRS = normalHrs;
                            objAttendanceDetail.EAT_TYPE = strType;
                            if (!string.IsNullOrEmpty(empid))
                                importDet.Add(objAttendanceDetail);
                        }
                    }

                    attendanceImport.ImportData = importDet;// dt;
                    xmlLanding = (CommonFunctions.ObjectTOXmlForPrdPlan(attendanceImport)).InnerXml;
                    //xmlLanding = CommonFunctions.ObjectTOXml(attendanceImport).InnerXml;

                    DataTable dtOut = null;
                    int result = BusinessLogic.HRMS.Payroll.AttendanceBL.ImportAttendance(xmlLanding, ref dtOut);
                    // int  result = dsOut.Tables.Count > 0 ? Convert.ToInt32(dsOut.Tables[0].Rows[0][0]) : -1;
                    if (result > 0 && dsImportedAttendance.Tables.Count > 0)
                    {
                        ResetForm(ControlEnums.CLEARIMPORTHDR);
                        txtImportRemarks.Text = string.Empty;
                        LinkButton lnkList = new LinkButton();
                        lnkList.CommandName = "LIST";
                        ActionHandler(lnkList, new EventArgs());
                        litErrorMsg.Text = this.GetLocalResourceObject("AttendanceImported").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.SQLERROR)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Err_IncorrectFile").ToString(); //Resources.Messages.ActionFailedPleaseTryAgain;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                    {
                        litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.EditUsedByAnotherUser;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (SaveDbEnum.NOEXELROWS == (SaveDbEnum)(result))
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_NoExelRows").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (SaveDbEnum.ALREADYEXISTS == (SaveDbEnum)(result))
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_AlreadyExists").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (SaveDbEnum.CANNOTMODIFY == (SaveDbEnum)(result))
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_CannotModify").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (SaveDbEnum.ALREADYDELETED == (SaveDbEnum)(result))
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_RecordAlreadyDeleted").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (SaveDbEnum.EMPLOYEENOTFOUND == (SaveDbEnum)(result))
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_EmployeeNotFound").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.ALREADYEXIST)
                    {
                        litErrorMsg.Text = this.GetLocalResourceObject("Err_AlreadyExists").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
            catch (OleDbException ex)
            {
                //sb = new StringBuilder();
                //sb.Append(this.GetLocalResourceObject("Err_ExcelSheet").ToString());
                //foreach (string item in excelColumns)
                //{
                //    sb.Append("<ul><li>" + item + "</li></ul>");
                //}
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(sb.ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);

            }
            catch (Exception ex)
            {
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                //litErrorMsg.Text = this.GetLocalResourceObject("Err_IncorrectExcel").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }

        public AttendanceImport GetAttendanceImport(string filePath)
        {
            try
            {
                string fileContent = System.IO.File.ReadAllText(filePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                string[] eachRecord = fileContent.Split('\n');
                AttendanceImport attendanceImport = new AttendanceImport();
                #region attnImport Header
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                attendanceImport.IS_UPDATE = chkOverwrite.Checked ? 1 : 0;
                attendanceImport.BIZUNIT_PK = currentUser.SBUID;
                attendanceImport.USER_PK = currentUser.PKUser;
                attendanceImport.TEMPLATE_TYPE = Convert.ToInt32(ddlAttnTemplate.SelectedValue);
                attendanceImport.EAR_REMARKS = HttpUtility.HtmlEncode(txtImportRemarks.Text);
                attendanceImport.EAR_ACTIVE = (int)DbActiveStatus.ACTIVE;
                attendanceImport.EAR_IN_FL = 1;
                attendanceImport.EAR_FROM_DATE = Convert.ToDateTime(txtImportDate.Text.Trim());
                attendanceImport.EAR_ATT_MODE = Convert.ToInt32(ddlProcessMode.SelectedValue);
                GetFieldValues(ControlEnums.COMPANY);
                attendanceImport.EAR_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                #endregion
                List<BusinessObject.HRMS.Payroll.AttendanceImportDetail> importDet = new List<AttendanceImportDetail>();

                for (int i = 0; i < eachRecord.Length; i++)
                {
                    string[] eachItem = eachRecord[i].Split(new string[] { "\";\"" }, StringSplitOptions.None);//Replace with - ";"
                    AttendanceImportDetail objAttendanceDetail = new AttendanceImportDetail();
                    #region attnImport Details
                    if (eachItem.Length > 1)
                    {
                        objAttendanceDetail.EAT_SL_NO = i + 1; // Sl No
                        objAttendanceDetail.EAT_EMPBIOMETRICID = eachItem[0].ToString().Replace("\"", "").Trim();//Biometric id
                        objAttendanceDetail.EAT_EMPLOYEE = eachItem[1].ToString().Replace("\"", "").Trim();//Employee Code
                        objAttendanceDetail.EAT_IN_TIME = eachItem[3].ToString().Replace("\"", "").Trim(); // In Time / Date
                        objAttendanceDetail.EAT_TYPE = eachItem[4].ToString().Replace("\"", "").Trim();// In or Out
                        importDet.Add(objAttendanceDetail);
                    }
                    #endregion
                }
                attendanceImport.ImportData = importDet;
                return attendanceImport;
            }
            catch
            {
                return null;
            }
        }
        public void SaveAttendanceImport(string xmlString)
        {
            StringBuilder sbImport = new StringBuilder();
            DataTable dtOut = null;
            int result = BusinessLogic.HRMS.Payroll.AttendanceBL.ImportAttendance(xmlString, ref dtOut);
            // int  result = dsOut.Tables.Count > 0 ? Convert.ToInt32(dsOut.Tables[0].Rows[0][0]) : -1;
            if (result > 0)
            {
                ResetForm(ControlEnums.CLEARIMPORTHDR);
                txtImportRemarks.Text = string.Empty;
                LinkButton lnkList = new LinkButton();
                lnkList.CommandName = "LIST";
                ActionHandler(lnkList, new EventArgs());
                if (result == 2)
                    litErrorMsg.Text = this.GetLocalResourceObject("Msg_SomeEmpNotFound").ToString();
                else
                    litErrorMsg.Text = this.GetLocalResourceObject("AttendanceImported").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (result == (int)DbSaveStatus.SQLERROR)
            {
                litErrorMsg.Text = GetLocalResourceObject("Err_IncorrectFile").ToString(); //Resources.Messages.ActionFailedPleaseTryAgain;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (result == (int)DbSaveStatus.CONCURRENCY)
            {
                litErrorMsg.Text = Resources.PageNameRes.Attendance + " " + Resources.Messages.EditUsedByAnotherUser;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (SaveDbEnum.NOEXELROWS == (SaveDbEnum)(result))
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_NoExelRows").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (SaveDbEnum.ALREADYEXISTS == (SaveDbEnum)(result))
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_AlreadyExists").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (SaveDbEnum.CANNOTMODIFY == (SaveDbEnum)(result))
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_CannotModify").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (SaveDbEnum.ALREADYDELETED == (SaveDbEnum)(result))
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_RecordAlreadyDeleted").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (SaveDbEnum.EMPLOYEENOTFOUND == (SaveDbEnum)(result))
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_EmployeeNotFound").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (result == (int)DbSaveStatus.ALREADYEXIST)
            {
                litErrorMsg.Text = this.GetLocalResourceObject("Err_AlreadyExists").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (result == (int)DbSaveStatus.DUPLICATERECORDS)
            {
                sbImport.Append(this.GetLocalResourceObject("Err_DuplicateExists").ToString());
                if (dtOut != null)
                {
                    foreach (DataRow item in dtOut.Rows)
                    {
                        sbImport.Append("<ul><li>" + item[0].ToString() + "</li></ul>");
                    }
                }
                //litErrorMsg.Text = this.GetLocalResourceObject("Err_DuplicateExists").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + sbImport.ToString()
                + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (result == (int)DbSaveStatus.RECORDNOTFOUND)
            {
                litErrorMsg.Text = string.Format(this.GetLocalResourceObject("Err_AttnNotFound").ToString(), txtImportDate.Text);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                + "','" + Resources.ErpRes.Information + "');", true);
            }
            else if (result == (int)DbSaveStatus.EMPWITHDIFFPROCESSMODE)
            {
                sbImport.Append("<ul><li>" + string.Format(this.GetLocalResourceObject("Err_EmpWithDifferentPrcMode").ToString(), ddlProcessMode.SelectedItem.Text));
                if (dtOut != null)
                {
                    foreach (DataRow item in dtOut.Rows)
                    {
                        sbImport.Append("<br />" + item[0].ToString());
                    }
                }
                sbImport.Append("</li></ul>");
                //litErrorMsg.Text = this.GetLocalResourceObject("Err_DuplicateExists").ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + sbImport.ToString()
                + "','" + Resources.ErpRes.Information + "');", true);
            }
            else
            {
                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "');", true);
            }
        }


        #region Get Employee Work Hours
        [WebMethod]
        public static double GetEmpWorkHours(string EmployeePk, string AttenDate)
        {
            double Workhrs = 0;
            try
            {
                if (!string.IsNullOrEmpty(AttenDate))
                    Workhrs = BusinessLogic.HRMS.Payroll.AttendanceBL.GetEmpWorkHour(Convert.ToInt32(EmployeePk), DateTime.Parse(AttenDate));
            }
            catch { }
            return Workhrs;
        }
        #endregion

        #region Get Employee Break Hours
        [WebMethod]
        public static BusinessObject.HRMS.Employee.EmployeePayDetailsBO GetEmpBreakHours(string EmployeePk)
        {
            //double Breakhrs = 0;
            BusinessObject.HRMS.Employee.EmployeePayDetailsBO selectedEmployeePayDetails = new BusinessObject.HRMS.Employee.EmployeePayDetailsBO();
            try
            {
                User currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (!string.IsNullOrEmpty(EmployeePk))
                    selectedEmployeePayDetails = EmployeePayDetailsBL.GetEmployeePayDetailsByID(Convert.ToInt32(EmployeePk), (int)DbActiveStatus.ACTIVE);
                //if (!string.IsNullOrEmpty(selectedEmployeePayDetails.EPD_BREAK_TIME))
                //    Breakhrs = Convert.ToDouble(selectedEmployeePayDetails.EPD_BREAK_TIME);               
            }
            catch
            {

            }
            //return Breakhrs;
            return selectedEmployeePayDetails;

        }
        #endregion

        #endregion

        #region Page Control Enum
        enum ControlEnums
        {
            GETATTENDANCE,
            ATTENDANCEDETAILS,
            EMPLOYMENTTYPE,
            COMPANY,
            LIST,
            DETAIL,
            CLEAR,
            EMPLOYEEDETAIL,
            WORKHRS,
            ATTENDANCELIST,
            ATTENDANCETEMPLATE,
            ATTENDANCEHDR,
            CLEARSEARCH,
            CLEARHDR,
            GETATTENDANCEBYPK,
            VIEWATTENDANCEDETAILS,
            ADD,
            GRIDEDIT,
            CLEARADD,
            GRIDDELETE,
            CLEARIMPORTHDR,
            PROCESSMODE,
            PROCESSMODEFILTER,
            PROCESSMODEHDR,
            DTLCLEARSEARCH,
            DTLSEARCH
        }
        #endregion

        public enum SaveDbEnum
        {
            SQLERROR = -1,
            ALREADYEXISTS = -2,
            CANNOTMODIFY = -3,
            ALREADYDELETED = -5,
            NOEXELROWS = -6,
            EMPLOYEENOTFOUND = -17,
            HDRDIFF = -26,
            EMPWITHDIFFPROCESSMODE = -71,
            INVALIDATTENDANCE = -72,
            DIFFDEPT = -73
        }
    }
}