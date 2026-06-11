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
using ERPSMS_v01;

namespace HRMS.Payroll
{
    public partial class Attendance : ERP.Store.UI.MyBasePage
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
        private List<AttendanceGenDetails> AttendancePopupDetailsViewState
        {
            get
            {
                return this.ViewState[ViewstateStrings.AttendancePopupDetailsViewState] == null ? new List<AttendanceGenDetails>() : (List<AttendanceGenDetails>)(this.ViewState[ViewstateStrings.AttendancePopupDetailsViewState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.AttendancePopupDetailsViewState] = value;
            }
        }

        /// <summary>
        /// Holds AttendancePopupDetailsViewState for Bind Gridview
        /// </summary>
        private List<AttendanceGenDetails> AttendanceEntryGridViewList
        {
            get
            {
                return this.ViewState[ViewstateStrings.AttendanceEntryGridViewList] == null ? new List<AttendanceGenDetails>() : (List<AttendanceGenDetails>)(this.ViewState[ViewstateStrings.AttendanceEntryGridViewList]);
            }
            set
            {
                this.ViewState[ViewstateStrings.AttendanceEntryGridViewList] = value;
            }
        }
        /// <summary>
        /// To keep time mask in view state
        /// </summary>
        private string TimeMask
        {
            get
            {
                return this.ViewState[ViewstateStrings.TimeMask] == null ? string.Empty : (this.ViewState[ViewstateStrings.TimeMask]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.TimeMask] = value;
            }
        }
        /// <summary>
        /// To keep time mask validation expression in view state
        /// </summary>
        private string TimeMaskValidationExp
        {
            get
            {
                return this.ViewState[ViewstateStrings.TimeMaskValidationExp] == null ? string.Empty : (this.ViewState[ViewstateStrings.TimeMaskValidationExp]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.TimeMaskValidationExp] = value;
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
                return (string)this.ViewState[ViewstateStrings.PageIndexList];
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndexList] = value;
            }
        }
        #endregion
        #region Variables
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataTable dtList;
        private DataTable dtAttendanceDetails;
        private DataTable dtExcelSchema;
        private OleDbConnection connExcel;
        private OleDbCommand cmdExcel;
        private OleDbDataAdapter oleDbDataAdapter;
        private DataSet dsImportedAttendance;
        private StringBuilder sb;
        private DateTime AttendanceDate;
        private AttendanceGen employeeAttendance;
        private BusinessObject.User currentUser;
        private string uploadPath;
        private string landingSheet;
        private string xmlLanding;
        private string[] excelColumns;
        private string[] airColums_General = { "Date", "EmpCode", "FirstIn", "LastOut", "TotalHrs" };
        private string[] airColums_IGCL = { "Date", "ID", "Type" };
        private int attendancePk = 0;
        private int EmpPk = 0;
        private double EmpWorkHrs = 0; 
        #endregion
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
                if (CurrPK > 0)
                    txtHdrBranchLocation.Enabled = false;
                else
                    txtHdrBranchLocation.Enabled = true;
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
                    if ((((DropDownList)sender).ID == "ddlEmployeePopUp"))
                    {
                        commonActions = ActionsEnum.CHANGE;
                    }
                    //if ((((DropDownList)sender).ID == "ddlLeaveType"))
                    //{
                    //    commonActions = ActionsEnum.CHANGETYPE;
                    //}
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
                        GetFieldValues(ControlEnums.LIST);
                        SetFieldValues(ControlEnums.LIST);
                        break;
                    #endregion
                    #region GO
                    case ActionsEnum.GO:
                        GetFieldValues(ControlEnums.GETATTENDANCE);
                        SetFieldValues(ControlEnums.GETATTENDANCE);
                        txtDate.Enabled = false;
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (grdEmployeeAttendance.Rows.Count < 1)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRecordFoundToSave").ToString())
                                   + "','" + Resources.ErpRes.Information + "');", true);
                            return;
                        }
                        else
                        {
                            HiddenField hdfBrLocPk = (HiddenField)grdEmployeeAttendance.Rows[0].FindControl("hdfBrLocPk");
                            if (hdfHdrBranchLocation.Value != hdfBrLocPk.Value)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_LocationMissMatch").ToString())
                                   + "','" + Resources.ErpRes.Information + "');", true);
                                return;
                            }

                        }
                        string EmpName = string.Empty;
                        string trxNo = string.Empty;
                        employeeAttendance = (AttendanceGen)SetUIValuesToObject(ControlEnums.ATTENDANCEDETAILS);
                        employeeAttendance.WKF_FLAG = 1;
                        result = BusinessLogic.HRMS.Payroll.AttendanceBL.SaveGeneralAttendance(employeeAttendance, out EmpName, out trxNo);
                        if (result > 0)
                        {
                            //AttendanceEntryGridViewList = (List<AttendanceGenDetails>)SetUIValuesToObject(ControlEnums.ATTENDANCELIST);
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
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                if (string.IsNullOrEmpty(EmpName))
                                    litErrorMsg.Text = GetLocalResourceObject("AlreadyExist").ToString(); //Resources.PageNameRes.Attendance + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                else
                                    litErrorMsg.Text = GetLocalResourceObject("AlreadyAdded").ToString() + "<br />" + HttpUtility.HtmlDecode(EmpName.Replace(",", "<br />"));
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
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlEnums.CLEAR);
                        GetFieldValues(ControlEnums.GETATTENDANCE);
                        SetFieldValues(ControlEnums.GETATTENDANCE);
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
                                string filePath = SaveDetails(out conStr);
                                if (!string.IsNullOrEmpty(filePath))
                                {
                                    //if (Convert.ToInt32(ddlAttnTemplate.SelectedValue) == (int)AttendanceTemplateType.IGCL_TXT)//IGCL.txt
                                    //{
                                    //    AttendanceGenImport AttendanceImportObj = GetAttendanceImport(filePath);
                                    //    if (AttendanceImportObj != null && AttendanceImportObj.ImportData != null && AttendanceImportObj.ImportData.Count > 0)
                                    //    {
                                    //        SaveAttendanceImport(CommonFunctions.XmlSerialize<AttendanceGenImport>(AttendanceImportObj));
                                    //    }
                                    //    else// Not in given format
                                    //    {
                                    //        litErrorMsg.Text = this.GetLocalResourceObject("Err_EnteredFileIncorrectFormat").ToString();
                                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    //            + "','" + Resources.ErpRes.Information + "');", true);
                                    //        break;
                                    //    }
                                    //}
                                    //else
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
                    #region DELETE ATTENDANCE DETAILS
                    case ActionsEnum.GRIDDELETE:
                        grvRow = (GridViewRow)((sender as ImageButton).Parent.Parent);
                        HiddenField attnPk = (HiddenField)grvRow.FindControl("hdfEAT_PK");
                        attendancePk = 0;
                        int.TryParse(attnPk.Value, out attendancePk);
                        if (attendancePk > 0)
                            result = BusinessLogic.HRMS.Payroll.AttendanceBL.DeleteAttendance(null, attendancePk, LastModifiedTime);
                        if (result > 0 || attendancePk == 0)
                        {
                            AttendanceEntryGridViewList = (List<AttendanceGenDetails>)SetUIValuesToObject(ControlEnums.ATTENDANCELIST);
                            AttendanceEntryGridViewList.RemoveAt(grvRow.RowIndex);
                            //GetFieldValues(ControlEnums.GETATTENDANCEBYPK);
                            SetFieldValues(ControlEnums.GETATTENDANCE);
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
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
                        int.TryParse(VattnPk.Value, out attendancePk);                       
                        GetFieldValues(ControlEnums.VIEWATTENDANCEDETAILS);
                        SetFieldValues(ControlEnums.VIEWATTENDANCEDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop4", "ShowContainerDiv('[id$=divEmpAttendanceDetails]','Attendance Details','500','400');", true);
                        break;
                    #endregion
                    #region ATTENDANCE IMPORT
                    case ActionsEnum.ATTDETAILS:
               //         foreach (GridViewRow grdrow in grdList.Rows)

                        grvRow = ((LinkButton)sender).Parent.Parent as GridViewRow;
                        
                            CurrPK = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                          
                        
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportPath + "?ID=0" + "&APPTYPE=" + "EMPATTND" + "&APPSUBTYPE= 1" + "&CurPK=" + CurrPK) + "&ISEXCELPRINT= 0" + "');", true);
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

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            if (((GridView)sender).ID == "grdEmployeeAttendance")
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    HiddenField hdfTotalHours = (HiddenField)e.Row.FindControl("hdfTotalHours");
                    TextBox txtTotalHours = (TextBox)e.Row.FindControl("txtTotalHours");
                    if (hdfTotalHours.Value != string.Empty)
                    {

                        txtTotalHours.Text = GetTime(hdfTotalHours.Value);
                    }
                    HiddenField hdfOTHours = (HiddenField)e.Row.FindControl("hdfOTHours");
                    if (hdfOTHours.Value != string.Empty)
                    {
                        TextBox txtOTHours = (TextBox)e.Row.FindControl("txtOTHours");
                        txtOTHours.Text = GetTime(hdfOTHours.Value);
                    }
                    HiddenField hdfShortHours = (HiddenField)e.Row.FindControl("hdfShortHours");
                    if (hdfShortHours.Value != string.Empty)
                    {
                        TextBox txtShortHours = (TextBox)e.Row.FindControl("txtShortHours");
                        txtShortHours.Text = GetTime(hdfShortHours.Value);
                    }
                    //HiddenField hdfCheckedFlag = (HiddenField)e.Row.FindControl("hdfCheckedFlag");
                    //if (hdfCheckedFlag.Value == 1.ToString())
                    //{
                    //    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    //    chkSelect.Checked = true;
                    //}

                    //#region Enable Required Validator Script
                    //CheckBox chkSlct = (CheckBox)e.Row.FindControl("chkSelect");
                    //RequiredFieldValidator vrfClrDate = (RequiredFieldValidator)e.Row.FindControl("vrfDateGridview");
                    //chkSlct.Attributes.Add("onclick", string.Format("SelectChange(this, '{0}');", vrfClrDate.ClientID));
                    //#endregion
                    txtTotalHours.Attributes.Add("onblur", string.Format("ResetShortAndOT('{0}');", txtTotalHours.ClientID));

                    //TextBox txtDateGridview = (TextBox)e.Row.FindControl("txtDateGridview");
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DatePickerCommon'" + txtDateGridview.ClientID + "'", "$(document).ready(function(){GrandScriptUtils.DatePickerCommon('" + txtDateGridview.ClientID + "');});", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RestrictedDatePicker'" + txtDateGridview.ClientID + "'", "$(document).ready(function(){GrandScriptUtils.RestrictedDatePicker('" + txtDateGridview.ClientID + "',false,true,true,'" + txtDateFrom.Text + "','" + txtDate.Text + "');});", true);
                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowDay'" + txtDateGridview.ClientID + "'", "$(document).ready(function(){ShowDay('" + txtDateGridview.ClientID + "');});", true);
                }
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
            switch (controlType)
            {
                case ControlEnums.CLEAR:
                    txtDesignation.Text = string.Empty;
                    hdfDesignation.Value = string.Empty;
                    ddlEmploymentType.SelectedIndex = ddlCompany.SelectedIndex = 0;
                    txtHdrDepartment.Text = string.Empty;
                    hdfHdrDepartment.Value = string.Empty;
                    //ddlEmployee.SelectedIndex = 0;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = "-1";
                    dtResult = null;
                    //BindGrid(ControlEnums.GETATTENDANCE);                   
                    break;
                case ControlEnums.CLEARSEARCH:
                    txtFilterFromDate.Text = txtFilterToDate.Text = txtFilterBranch.Text = string.Empty;
                    hdfFilterBranch.Value = string.Empty;
                    uclPaging.CurrentPage = 0;
                    this.PageIndexList = "1";
                    this.EntryStatus = EntryStatus.LISTMODE;
                    this.CurrPK = 0;
                    break;
                case ControlEnums.CLEARHDR:
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    txtHdrBranchLocation.Text = txtHdrDepartment.Text = txtRemarks.Text = txtDate.Text = string.Empty;
                    hdfHdrBranchLocation.Value = hdfHdrDepartment.Value = string.Empty;
                    txtDesignation.Text = string.Empty;
                    hdfDesignation.Value = string.Empty;
                    ddlEmploymentType.SelectedIndex = ddlCompany.SelectedIndex = 0;
                    txtEmployee.Text = string.Empty;
                    hdfEmployee.Value = "-1";
                    dtResult = null;
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
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                FilterParameters objFilterParam;
                switch (type)
                {
                    #region GETATTENDANCE
                    case ControlEnums.GETATTENDANCE:
                        objFilterParam = new FilterParameters();
                        objFilterParam.ToDate = objFilterParam.FromDate = string.IsNullOrEmpty(txtDate.Text) ? (DateTime?)null : DateTime.Parse(txtDate.Text);
                        objFilterParam.PK = CurrPK;
                        objFilterParam.Active = 1;
                        objFilterParam.BizUnit = currentUser.SBUID;
                        objFilterParam.Designation = GetNullableInt(hdfDesignation.Value) > 0 ? GetNullableInt(hdfDesignation.Value) : null;
                        objFilterParam.BranchLocation = GetNullableInt(hdfHdrBranchLocation.Value) > 0 ? GetNullableInt(hdfHdrBranchLocation.Value) : null;
                        objFilterParam.Department = GetNullableInt(hdfHdrDepartment.Value) > 0 ? GetNullableInt(hdfHdrDepartment.Value) : null;
                        objFilterParam.EmployeeType = GetNullableInt(ddlEmploymentType.SelectedValue) > 0 ? GetNullableInt(ddlEmploymentType.SelectedValue) : null;
                        objFilterParam.Company = GetNullableInt(ddlCompany.SelectedValue) > 0 ? GetNullableInt(ddlCompany.SelectedValue) : null;
                        objFilterParam.Employee = GetNullableInt(hdfEmployee.Value) > 0 ? GetNullableInt(hdfEmployee.Value) : null;
                        objFilterParam.EmployeeCategory = (int)EmployeeCategory.HRMSEmployee;
                        objFilterParam.ToDate = txtDate.Text == string.Empty ? (DateTime?)null : Convert.ToDateTime(txtDate.Text);
                        employeeAttendance = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendance(objFilterParam);
                        if (employeeAttendance != null)
                            AttendanceEntryGridViewList = employeeAttendance.AttnDetails.ToList();
                        break;
                    #endregion
                    #region EMPLOYMENTTYPE
                    case ControlEnums.EMPLOYMENTTYPE:
                        int commonPK = 0;
                        dtResult = BusinessLogic.HRMS.Employee.EmployeeBasicInfoBL.GetEmploymentType(commonPK);
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
                    #region COMPANY
                    case ControlEnums.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), 0, 0);
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
                        objFilterParam.Department = null;// string.IsNullOrEmpty(hdfFilterDept.Value) ? (int?)null : Convert.ToInt32(hdfFilterDept.Value);
                        objFilterParam.BranchLocation = string.IsNullOrEmpty(hdfFilterBranch.Value) ? (int?)null : Convert.ToInt32(hdfFilterBranch.Value);
                        objFilterParam.Active = (int)DbActiveStatus.ACTIVE;
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
                        objFilterParam.Designation = GetNullableInt(hdfDesignation.Value) > 0 ? GetNullableInt(hdfDesignation.Value) : null;
                        objFilterParam.BranchLocation = GetNullableInt(hdfHdrBranchLocation.Value) > 0 ? GetNullableInt(hdfHdrBranchLocation.Value) : null;
                        objFilterParam.Department = GetNullableInt(hdfHdrDepartment.Value) > 0 ? GetNullableInt(hdfHdrDepartment.Value) : null;
                        objFilterParam.EmployeeType = GetNullableInt(ddlEmploymentType.SelectedValue) > 0 ? GetNullableInt(ddlEmploymentType.SelectedValue) : null;
                        objFilterParam.Company = GetNullableInt(ddlCompany.SelectedValue) > 0 ? GetNullableInt(ddlCompany.SelectedValue) : null;
                        objFilterParam.Employee = GetNullableInt(hdfEmployee.Value) > 0 ? GetNullableInt(hdfEmployee.Value) : null;
                        objFilterParam.EmployeeCategory = (int)EmployeeCategory.HRMSEmployee;
                        employeeAttendance = BusinessLogic.HRMS.Payroll.AttendanceBL.GetGeneralAttendanceDetails(objFilterParam);
                        if (employeeAttendance != null)
                            AttendanceEntryGridViewList = employeeAttendance.AttnDetails.ToList();

                        break;
                    #endregion
                    #region VIEW ATTENDANCE DETAILS
                    case ControlEnums.VIEWATTENDANCEDETAILS:                     
                        if (attendancePk > 0)
                        {
                            dtAttendanceDetails = BusinessLogic.HRMS.Payroll.AttendanceBL.GetAttendanceDetails(attendancePk);
                        }
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
                    grdEmployeeAttendance.DataSource = AttendanceEntryGridViewList;
                    grdEmployeeAttendance.DataBind();
                    if (EntryStatus == EntryStatus.NEWMODE) // hide delete button column in new mode
                        grdEmployeeAttendance.Columns[grdEmployeeAttendance.Columns.Count - 1].Visible = false;
                    else
                        grdEmployeeAttendance.Columns[grdEmployeeAttendance.Columns.Count - 1].Visible = true;
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
                        employeeAttendance = new AttendanceGen();
                        employeeAttendance.EAR_BRANCH = string.IsNullOrEmpty(hdfHdrBranchLocation.Value) ? null : hdfHdrBranchLocation.Value;
                        GetFieldValues(ControlEnums.COMPANY);
                        employeeAttendance.EAR_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                        employeeAttendance.EAR_EMPDEPARTMENT = null;// string.IsNullOrEmpty(hdfHdrDepartment.Value) ? null : hdfHdrDepartment.Value;
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
                        employeeAttendance.AttnDetails = (List<AttendanceGenDetails>)SetUIValuesToObject(ControlEnums.ATTENDANCELISTFROMUI);//GetAttendanceDetailsFromUI();
                        retObject = employeeAttendance;
                        break;
                    #endregion
                    #region ATTENDANCE LIST FROM UI
                    case ControlEnums.ATTENDANCELISTFROMUI:
                        List<AttendanceGenDetails> attnDetList = new List<AttendanceGenDetails>();
                        AttendanceGenDetails attnDet;
                        TimeSpan timSpanIn;
                        TimeSpan timSpanOut;
                        foreach (GridViewRow grdrow in grdEmployeeAttendance.Rows)
                        {
                            CheckBox chk;
                            chk = (CheckBox)grdrow.FindControl("chkSelect");
                            if (chk.Checked)
                            {
                                string date = txtDate.Text;
                                attnDet = new AttendanceGenDetails();
                                attnDet.EAT_PK = GetNullableInt(((HiddenField)grdrow.FindControl("hdfEAT_PK")).Value) ?? 0;
                                attnDet.EAT_DATE = txtDate.Text;
                                attnDet.EAT_EMPLOYEE = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfEmployeePk")).Value);
                                string inTimeText = ((TextBox)grdrow.FindControl("txtInTime")).Text;
                                if (!string.IsNullOrEmpty(inTimeText))
                                {
                                    attnDet.EAT_IN_TIME = date; 
                                    TimeSpan.TryParse(inTimeText, out timSpanIn);
                                    attnDet.EAT_IN_TIME = Convert.ToDateTime(attnDet.EAT_IN_TIME).Add(timSpanIn).ToString();
                                }
                                else
                                    attnDet.EAT_IN_TIME = null;

                                string outTimeText = ((TextBox)grdrow.FindControl("txtOutTime")).Text;
                                if (!string.IsNullOrEmpty(outTimeText))
                                {
                                    attnDet.EAT_OUT_TIME = date; 
                                    TimeSpan.TryParse(outTimeText, out timSpanOut);
                                    attnDet.EAT_OUT_TIME = Convert.ToDateTime(attnDet.EAT_OUT_TIME).Add(timSpanOut).ToString();
                                    if (!string.IsNullOrEmpty(attnDet.EAT_IN_TIME))
                                    {
                                        double totalDays = (Convert.ToDateTime(attnDet.EAT_OUT_TIME) - Convert.ToDateTime(attnDet.EAT_IN_TIME)).TotalDays;
                                        if (totalDays < 0)
                                            attnDet.EAT_OUT_TIME = Convert.ToDateTime(attnDet.EAT_OUT_TIME).AddDays(1).ToString();
                                    }
                                }
                                else
                                    attnDet.EAT_OUT_TIME = null;

                                attnDet.EAT_WORKED_HRS = ConvertTimeStringToDouble(((TextBox)grdrow.FindControl("txtTotalHours")).Text.Trim());
                                attnDet.EAT_SHORT_HRS = ConvertTimeStringToDouble(((TextBox)grdrow.FindControl("txtShortHours")).Text.Trim());
                                attnDet.EAT_OT_HRS = ConvertTimeStringToDouble(((TextBox)grdrow.FindControl("txtOTHours")).Text.Trim());
                                attnDet.EAT_NORMAL_HRS = Convert.ToDouble(((HiddenField)grdrow.FindControl("hdfNormalHours")).Value);
                                attnDet.EAT_IS_UPDATE = 1;                             
                                attnDetList.Add(attnDet);
                            }
                        }
                        retObject = attnDetList;
                        break;
                    #endregion
                    #region ATTENDANCE LIST
                    case ControlEnums.ATTENDANCELIST:
                        List<AttendanceGenDetails> AttendanceList = AttendanceEntryGridViewList;
                        if (AttendanceList != null && AttendanceList.Count > 0)
                        {
                            AttendanceGenDetails AttendanceEntry;
                            TimeSpan timeSpanIn;
                            TimeSpan timeSpanOut;
                            foreach (GridViewRow grdrow in grdEmployeeAttendance.Rows)
                            {
                                CheckBox chkSelect = (CheckBox)grdrow.FindControl("chkSelect");
                                TextBox txtTotalHours = (TextBox)grdrow.FindControl("txtTotalHours");
                                TextBox txtShortHours = (TextBox)grdrow.FindControl("txtShortHours");
                                TextBox txtOTHours = (TextBox)grdrow.FindControl("txtOTHours");
                                TextBox txtInTime = (TextBox)grdrow.FindControl("txtInTime");
                                TextBox txtOutTime = (TextBox)grdrow.FindControl("txtOutTime");
                                HiddenField hdfEAT_PK = (HiddenField)grdrow.FindControl("hdfEAT_PK");
                                HiddenField hdfEmployeePk = (HiddenField)grdrow.FindControl("hdfEmployeePk");
                                HiddenField hdfNormalHours = (HiddenField)grdrow.FindControl("hdfNormalHours");
                                HiddenField hdfCheckedFlag = (HiddenField)grdrow.FindControl("hdfCheckedFlag");
                                Label lblEmployeeName = (Label)grdrow.FindControl("lblEmployeeName");
                                Label lblLocationGridView = (Label)grdrow.FindControl("lblLocationGridView");
                                Label lblDept = (Label)grdrow.FindControl("lblDept");

                                int detpk = 0;
                                int.TryParse(hdfEAT_PK.Value, out detpk);
                                AttendanceEntry = AttendanceList.SingleOrDefault(r => r.EAT_EMPLOYEE == Convert.ToInt32(hdfEmployeePk.Value) && r.EAT_PK == (detpk > 0 ? detpk : r.EAT_PK));
                                string date = txtDate.Text;                             

                                string inTimeText = txtInTime.Text;
                                if (!string.IsNullOrEmpty(inTimeText))
                                {
                                    AttendanceEntry.EAT_IN_TIME = date;
                                    TimeSpan.TryParse(inTimeText, out timeSpanIn);
                                    if (!string.IsNullOrEmpty(AttendanceEntry.EAT_IN_TIME))
                                        AttendanceEntry.EAT_IN_TIME = (Convert.ToDateTime(AttendanceEntry.EAT_IN_TIME).Add(timeSpanIn)).ToString();
                                }
                                else
                                    AttendanceEntry.EAT_IN_TIME = null;

                                string outTimeText = txtOutTime.Text;
                                if (!string.IsNullOrEmpty(outTimeText))
                                {
                                    AttendanceEntry.EAT_OUT_TIME = date;
                                    TimeSpan.TryParse(outTimeText, out timeSpanOut);
                                    if (!string.IsNullOrEmpty(AttendanceEntry.EAT_OUT_TIME))
                                        AttendanceEntry.EAT_OUT_TIME = (Convert.ToDateTime(AttendanceEntry.EAT_OUT_TIME).Add(timeSpanOut)).ToString();
                                    if (!string.IsNullOrEmpty(AttendanceEntry.EAT_IN_TIME))
                                    {
                                        double totalDays = (Convert.ToDateTime(AttendanceEntry.EAT_OUT_TIME) - Convert.ToDateTime(AttendanceEntry.EAT_IN_TIME)).TotalDays;
                                        if (totalDays < 0)
                                            AttendanceEntry.EAT_OUT_TIME = Convert.ToDateTime(AttendanceEntry.EAT_OUT_TIME).AddDays(1).ToString();
                                    }
                                }
                                else
                                    AttendanceEntry.EAT_OUT_TIME = null;

                                AttendanceEntry.EAT_WORKED_HRS = ConvertTimeStringToDouble(txtTotalHours.Text.Trim());
                                AttendanceEntry.EAT_SHORT_HRS = ConvertTimeStringToDouble(txtShortHours.Text.Trim());
                                AttendanceEntry.EAT_OT_HRS = ConvertTimeStringToDouble(txtOTHours.Text.Trim());
                                AttendanceEntry.EAT_NORMAL_HRS = Convert.ToDouble(hdfNormalHours.Value);
                                AttendanceEntry.EAT_IS_UPDATE = 1;                                
                            }
                            AttendanceEntryGridViewList = AttendanceList;
                            retObject = AttendanceEntryGridViewList;
                        }
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
                            txtHdrBranchLocation.Text = HttpUtility.HtmlDecode(employeeAttendance.EAR_BRANCH_TEXT);
                            hdfHdrBranchLocation.Value = employeeAttendance.EAR_BRANCH;
                            //txtHdrDepartment.Text = HttpUtility.HtmlDecode(employeeAttendance.EAR_EMPDEPARTMENT_TEXT);
                            //hdfHdrDepartment.Value = employeeAttendance.EAR_EMPDEPARTMENT;
                            txtRemarks.Text = HttpUtility.HtmlDecode(employeeAttendance.EAR_REMARKS);
                            LastModifiedTime = employeeAttendance.LAST_MOD_DT;
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
                    ddlEmploymentType.Items.Clear();
                    if (dtResult != null && dtResult.Rows.Count > 0)
                    {
                        ddlEmploymentType.DataSource = dtResult;
                        ddlEmploymentType.DataTextField = "CON_NAME";
                        ddlEmploymentType.DataValueField = "CON_PK";
                        ddlEmploymentType.DataBind();
                        ddlEmploymentType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                        ddlEmploymentType.Items.HtmlDecode();
                        //ddlEmploymentTypePopUp.DataSource = dtResult;
                        //ddlEmploymentTypePopUp.DataTextField = "CON_NAME";
                        //ddlEmploymentTypePopUp.DataValueField = "CON_PK";
                        //ddlEmploymentTypePopUp.DataBind();
                        //ddlEmploymentTypePopUp.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));

                    }
                    break;
                #endregion
                #region COMPANY
                case ControlEnums.COMPANY:
                    ddlCompany.Items.Clear();
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.DataSource = dtCompany;
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                        ddlCompany.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        ddlCompany.Items.HtmlDecode();
                    }
                    break;
                #endregion
                #region EMPLOYEES
                case ControlEnums.EMPLOYEES:
                    //ddlEmployee.Items.Clear();
                    //ddlEmployee.DataSource = dtResult;
                    //ddlEmployee.DataTextField = GTIService.Constants.Configurations.Employees.Fields.NAME;
                    //ddlEmployee.DataValueField = GTIService.Constants.Configurations.Employees.Fields.PK;
                    //ddlEmployee.DataBind();
                    //ddlEmployee.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    //ddlEmployee.Items.HtmlDecode();
                    //ddlEmployeePopUp.Items.Clear();
                    //ddlEmployeePopUp.DataSource = dtResult;
                    //ddlEmployeePopUp.DataTextField = GTIService.Constants.Configurations.Employees.Fields.NAME;
                    //ddlEmployeePopUp.DataValueField = GTIService.Constants.Configurations.Employees.Fields.PK;
                    //ddlEmployeePopUp.DataBind();
                    //ddlEmployeePopUp.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    //ddlEmployeePopUp.Items.HtmlDecode();
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
            }
        }
        #endregion
        #endregion

        #region Configuration Settings
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("HRMS SETTINGS", "ENTRY MODE");
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfAttnEntryMode.Value = dt.Rows[0]["ACF_VALUE"].ToString(); //1:Daily, 2:Cumulative                
            }
        }

        #endregion

        #region HelperMethods
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
        private double? GetNullableDouble(string str)
        {
            double result;
            if (double.TryParse(str, out result))
            {
                return (double?)result;
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
            DateTime dt;
            try
            {
                if (pdt != null)
                {
                    dt = Convert.ToDateTime(pdt);
                    return dt.ToString(CommonConstants.TIMEFORMAT);
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        private string GetTime(string time)
        {
            string result = string.Empty;
            if (time == string.Empty || time == "0") return result;

            string[] arr = time.Split('.');
            string h = arr[0];
            string m = string.Empty.PadRight(2, '0');
            if (arr.Length == 2)
            {
                if (arr[1].Length > 1) m = arr[1];
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
        private string SaveDetails(out string conStr)
        {           
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
            tempFileInfoObj = new FileInfo(fupImport.PostedFile.FileName);
            string attachmentFileFormat = tempFileInfoObj.Extension;

            string FileName = Guid.NewGuid().ToString() + attachmentFileFormat;
            conStr = CheckValidFileType(attachmentFileFormat);
            int Template = Convert.ToInt32(ddlAttnTemplate.SelectedValue);
            if (Template == (int)AttendanceTemplateType.IGCL_TXT)
            {
                if (attachmentFileFormat.ToLower() != ".txt")
                {
                    return string.Empty;
                }
            }
            else if (Template == (int)AttendanceTemplateType.GENERAL || Template == (int)AttendanceTemplateType.IGCL)
            {
                if (attachmentFileFormat.ToLower() != ".xls" && attachmentFileFormat.ToLower() != ".xlsx")
                {
                    return string.Empty;
                }
            }
            if ((!string.IsNullOrEmpty(conStr)) || (attachmentFileFormat.ToLower() == ".txt"))
            {
                fupImport.SaveAs(uploadPath + FileName);
            }

            return uploadPath + FileName;            
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
        private void ImportToGrid(string filePath, string conStr)
        {
            try
            {
                StringBuilder sbImport = new StringBuilder();
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
                            string empId = Convert.ToString(dtImportData.Rows[i]["EmpCode"]);

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
                    AttendanceGenImport attendanceImport = new AttendanceGenImport();
                    attendanceImport.IS_UPDATE = chkOverwrite.Checked ? 1 : 0;
                    attendanceImport.BIZUNIT_PK = currentUser.SBUID;
                    attendanceImport.USER_PK = currentUser.PKUser;
                    attendanceImport.TEMPLATE_TYPE = Convert.ToInt32(ddlAttnTemplate.SelectedValue);
                    attendanceImport.EAR_REMARKS = HttpUtility.HtmlEncode(txtImportRemarks.Text);
                    attendanceImport.EAR_ACTIVE = (int)DbActiveStatus.ACTIVE;
                    attendanceImport.EAR_IN_FL = 1;
                    attendanceImport.EAR_FROM_DATE = Convert.ToDateTime(txtImportDate.Text.Trim());
                    attendanceImport.EAR_BRANCH = Convert.ToInt32(hdfImportBranchLocation.Value);
                    GetFieldValues(ControlEnums.COMPANY);
                    attendanceImport.EAR_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                    List<BusinessObject.HRMS.Payroll.AttendanceGenImportDetail> importDet = new List<AttendanceGenImportDetail>();
                    DataColumnCollection importColumns = dtImportData.Columns;
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        DateTime attnDate = new DateTime();
                        string strType = string.Empty;
                        string date1 = Convert.ToString(dtImportData.Rows[i]["Date"]);
                        if (!string.IsNullOrEmpty(date1.Trim()))
                        {
                            int sl = i + 1;
                            string empid = string.Empty;
                            DateTime attDate = Convert.ToDateTime(date1);
                            DateTime? dtInTime = null;
                            DateTime? dtOutTime = null;
                            DateTime tempDt = new DateTime();
                            decimal totalHrs = 0;
                            DateTime.TryParse((dtImportData.Rows[i]["Date"]).ToString(), out attnDate);

                            if (importColumns.Contains("ID"))
                            {
                                empid = Convert.ToString(dtImportData.Rows[i]["ID"]).Trim();
                            }
                            if (importColumns.Contains("EmpCode"))
                            {
                                empid = Convert.ToString(dtImportData.Rows[i]["EmpCode"]).Trim();
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
                                if (dtInTime.HasValue && dtOutTime.HasValue)
                                {
                                    double totalDays = (Convert.ToDateTime(dtOutTime) - Convert.ToDateTime(dtInTime)).TotalDays;
                                    if (totalDays < 0)
                                        dtOutTime = Convert.ToDateTime(dtOutTime).AddDays(1);
                                }
                            }
                            if (importColumns.Contains("TotalHrs") && decimal.TryParse((dtImportData.Rows[i]["TotalHrs"]).ToString(), out totalHrs))
                            {
                                //dtNormalTime = tempDt;
                                ////dtNormalTime = attDate.Add(tempDt.TimeOfDay);    
                                //tempDt = new DateTime();

                                //totalHrs = GetDecimalFromTimeSpan(tempDt.TimeOfDay);
                            }
                            if (importColumns.Contains("Type"))
                                strType = Convert.ToString(dtImportData.Rows[i]["Type"]);

                            BusinessObject.HRMS.Payroll.AttendanceGenImportDetail objAttendanceDetail = new AttendanceGenImportDetail();
                            objAttendanceDetail.EAT_DATE = attnDate.ToString();
                            objAttendanceDetail.EAT_SL_NO = sl;
                            objAttendanceDetail.EAT_EMPLOYEE = empid;
                            if (!string.IsNullOrEmpty(Convert.ToString(dtInTime)))
                                objAttendanceDetail.EAT_IN_TIME = Convert.ToString(dtInTime);
                            else
                                objAttendanceDetail.EAT_IN_TIME = null;
                            if (!string.IsNullOrEmpty(Convert.ToString(dtOutTime)))
                                objAttendanceDetail.EAT_OUT_TIME = Convert.ToString(dtOutTime);
                            else
                                objAttendanceDetail.EAT_OUT_TIME = null;
                            objAttendanceDetail.EAT_WORKED_HRS = totalHrs;
                            objAttendanceDetail.EAT_TYPE = strType;
                            if (!string.IsNullOrEmpty(empid))
                                importDet.Add(objAttendanceDetail);
                        }
                    }

                    attendanceImport.ImportData = importDet;// dt;
                    xmlLanding = (CommonFunctions.ObjectTOXmlForPrdPlan(attendanceImport)).InnerXml;
                    //xmlLanding = CommonFunctions.ObjectTOXml(attendanceImport).InnerXml;

                    DataTable dtOut = null;
                    int result = BusinessLogic.HRMS.Payroll.AttendanceBL.GeneralAttendanceImport(xmlLanding, ref dtOut);
                    // int  result = dsOut.Tables.Count > 0 ? Convert.ToInt32(dsOut.Tables[0].Rows[0][0]) : -1;
                    if (result > 0 && dsImportedAttendance.Tables.Count > 0)
                    {
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
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
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
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowMessageFixed('" + sbImport.ToString()
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.RECORDNOTFOUND)
                    {
                        litErrorMsg.Text = string.Format(this.GetLocalResourceObject("Err_AttnNotFound").ToString(), txtImportDate.Text);
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

        public AttendanceGenImport GetAttendanceImport(string filePath)
        {
            try
            {
                string fileContent = System.IO.File.ReadAllText(filePath);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                string[] eachRecord = fileContent.Split('\n');
                AttendanceGenImport attendanceImport = new AttendanceGenImport();
                #region attnImport Header
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                attendanceImport.IS_UPDATE = 1;
                attendanceImport.BIZUNIT_PK = currentUser.SBUID;
                attendanceImport.USER_PK = currentUser.PKUser;
                attendanceImport.TEMPLATE_TYPE = Convert.ToInt32(ddlAttnTemplate.SelectedValue);
                attendanceImport.EAR_REMARKS = HttpUtility.HtmlEncode(txtImportRemarks.Text);
                attendanceImport.EAR_ACTIVE = (int)DbActiveStatus.ACTIVE;
                attendanceImport.EAR_IN_FL = 1;
                GetFieldValues(ControlEnums.COMPANY);
                attendanceImport.EAR_COMPANY = GetNullableInt(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()).Value;
                #endregion
                List<BusinessObject.HRMS.Payroll.AttendanceGenImportDetail> importDet = new List<AttendanceGenImportDetail>();

                for (int i = 0; i < eachRecord.Length; i++)
                {
                    string[] eachItem = eachRecord[i].Split(new string[] { "\";\"" }, StringSplitOptions.None);//Replace with - ";"
                    AttendanceGenImportDetail objAttendanceDetail = new AttendanceGenImportDetail();
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
            DataTable dtOut = null;
            int result = BusinessLogic.HRMS.Payroll.AttendanceBL.ImportAttendance(xmlString, ref dtOut);
            // int  result = dsOut.Tables.Count > 0 ? Convert.ToInt32(dsOut.Tables[0].Rows[0][0]) : -1;
            if (result > 0)
            {
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
                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
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

        #endregion

        #region Page Control Enum
        enum ControlEnums
        {
            GETATTENDANCE,
            ATTENDANCEDETAILS,
            EMPLOYMENTTYPE,
            COMPANY,
            EMPLOYEES,
            LIST,
            DETAIL,
            CLEAR,
            WORKHRS,
            ATTENDANCELIST,
            ATTENDANCETEMPLATE,
            ATTENDANCEHDR,
            CLEARSEARCH,
            CLEARHDR,
            GETATTENDANCEBYPK,
            VIEWATTENDANCEDETAILS,
            ATTENDANCELISTFROMUI
        }
        #endregion

        public enum SaveDbEnum
        {
            SQLERROR = -1,
            ALREADYEXISTS = -2,
            CANNOTMODIFY = -3,
            ALREADYDELETED = -5,
            NOEXELROWS = -6,
            EMPLOYEENOTFOUND = -17
        }
    }
}