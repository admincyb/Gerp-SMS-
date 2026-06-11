using AjaxControlToolkit;
using BusinessLogic.CommonManagement;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using GTIService.Dashboard;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Objects;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using Telerik.ReportViewer.Html5.WebForms;


namespace ERPSMS_v01.TelReports
{
    public partial class TelerikRptViewer : ERP.Store.UI.MyBasePage //System.Web.UI.Page
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
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);

            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }
        private string ReportFile
        {
            get
            {
                return this.ViewState["ReportFile"] == null ? string.Empty : (this.ViewState["ReportFile"]).ToString();
            }
            set
            {
                this.ViewState["ReportFile"] = value;
            }
        }
        private string FromDate
        {
            get
            {
                return Convert.ToString(this.ViewState["FromDate"]);
            }
            set
            {
                this.ViewState["FromDate"] = value;
            }
        }
        private string ToDate
        {
            get
            {
                return Convert.ToString(this.ViewState["ToDate"]);
            }
            set
            {
                this.ViewState["ToDate"] = value;
            }
        }
        /// <summary>
        /// Current PK
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
        /// SelectedPK PK-- Used to keep the selected pk from a grid
        /// </summary>
        private int SelectedPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedPK] = value;
            }
        }
        /// <summary>
        /// SelectedParentPK PK--Used to keep the selected pk from a parent grid
        /// </summary>
        private int SelectedParentPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedParentPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedParentPK] = value;
            }
        }
        /// <summary>
        /// SelectedParentGrid--Used to keep the selected grid name of the parent grid
        /// </summary>
        private string SelectedParentGrid
        {
            get
            {
                return this.ViewState[ViewstateStrings.SelectedParentGrid] == null ? string.Empty : (this.ViewState[ViewstateStrings.SelectedParentGrid]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedParentGrid] = value;
            }
        }

        /// <summary>
        /// ControlPK--used to keep the PK of a control
        /// </summary>
        private int ControlPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.ControlPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ControlPK] = value;
            }
        }

        /// <summary>
        /// EntityName--used to keep the name of an Entity
        /// </summary>
        private string EntityName
        {
            get
            {
                return this.ViewState[ViewstateStrings.EntityName] == null ? string.Empty : (this.ViewState[ViewstateStrings.EntityName]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.EntityName] = value;
            }
        }


        /// <summary>
        /// RelatedControlID--Used to store the RelatedControlId of a control
        /// </summary>
        private string RelatedControlID
        {
            get
            {
                return this.ViewState[ViewstateStrings.RelatedControlID] == null ? string.Empty : (this.ViewState[ViewstateStrings.RelatedControlID]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.RelatedControlID] = value;
            }
        }

        /// <summary>
        /// RelatedControlID--Used to store the RelatedControlId of a control
        /// </summary>
        private Dictionary<string, string> AutoCompleteRelatedControlValue
        {
            get
            {
                return this.Session[ERP.Utilities.SessionStrings.AutoCompleteRelatedControlValue] == null
                    ? new Dictionary<string, string>()
                    : (Dictionary<string, string>)this.Session[ERP.Utilities.SessionStrings.AutoCompleteRelatedControlValue];
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.AutoCompleteRelatedControlValue] = value;
            }
        }
        /// <summary>
        /// ChildGridName-- used to store the name of a child grid
        /// </summary>
        private string ChildGridName
        {
            get
            {
                return (this.ViewState[ViewstateStrings.ChildGridName]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.ChildGridName] = value;
            }
        }
        /// <summary>
        /// TabCode--Used to store tab code
        /// </summary>
        private string TabCode
        {
            get
            {
                return (this.ViewState[ViewstateStrings.TabCode]).ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.TabCode] = value;
            }
        }

        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
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
        /// To maintain the LastModifiedTime in viewstate
        /// </summary>
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
        /// To maintain Attachment File Name
        /// </summary>
        private string AttachmentFileName
        {
            get
            {
                return Convert.ToString(this.ViewState[ViewstateStrings.AttachmentFileName]);
            }
            set
            {
                this.ViewState[ViewstateStrings.AttachmentFileName] = value;
            }
        }
        private bool IsUserControl
        {
            get
            {
                return Convert.ToBoolean(this.ViewState["IsUserControl"]);
            }
            set
            {
                this.ViewState["IsUserControl"] = value;
            }
        }
        private string UserControlFile
        {
            get
            {
                return Convert.ToString(this.ViewState["UserControlFile"]);
            }
            set
            {
                this.ViewState["UserControlFile"] = value;
            }
        }
        #endregion
        User currentUser;
        // Indicates the state as well as action
        private static int scriptCount = 1;
        string scriptName = string.Empty;
        private ReportCode rptCode;
        private ActionsEnum commonActions;
        private CRM_CUSTOMER_MST crmCustomerMstObj;
        ServiceUtility serviceUtilityObj;
        //page related Entity Objects
        //List for binding details to controls            
        private List<SPADM_REPORT_CONTROL_CFG_GET_Result> spAdmReportControlCfgGetResultList;
        private List<SPADM_FORM_TAB_CFG_GET_Result> spAdmFormTabCfgGetResultList;
        private List<ADM_REPORT_CFG> admReportCfgList;
        private ADM_REPORT_GROUP_CFG admReportGroupCfgObj;
        private List<ADM_REPORT_GROUP_CFG> admReportGroupCfgList;
        private List<CRM_CUSTOMER_MST> crmCustomerMstList;
        private List<ADM_FORM_TAB_CONTROL_DTL> admFormTabControlDtlList;
        private ADM_REPORT_USER_MAP admReportMap;
        CustomerRegistrationService customerRegistrationServiceClient;
        string retVal;
        Object retEntityObj = null;
        string PageScript = "";
        string PageScriptOuterInit = "";
        bool doSave = false;
        private string refID;
        private string inboxFlag;
        private int id = 0;
        private int rptPK = 0;
        //private string attachmentFilePath;
        //private string attachmentFileFormat;
        //private string attachmentFileContentType;
        //private string attachmentFileName;
        private string ReportName = string.Empty;
        private string ReportHeading = string.Empty;
        private string localCmpAddress = string.Empty;
        private decimal PNDEntries = 0;
        private decimal PNDrecordsPerPage = 6;
        decimal? TotalBalBF = 0;
        IList listObj;
        private ERPEntities currentEntity;
        string LogoPath = string.Empty;

        string CompanyName = "Company : All";
        int CompanyPK;
        private DataSet dsRptData;
        //  private DataSet dsParamSettings;
        private string spName;
        private DataTable dtRptData;
        private const string UserControlID = "userFilter";
        private ERPSMS_v01.Reports.UserControls.IFilterControl customControl = null;

        private string DecimalFormat = "#0.";
        private bool AutoInitComponents = false;
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (!IsPostBack)
                {
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    CheckUserRight();
                    scriptCount = 1;
                    AutoCompleteRelatedControlValue = null;
                    Session[ERP.Utilities.SessionStrings.REPORTPK] = null;
                    pnlControls.Controls.Clear();
                    //  divReportViewer.Visible = false;
                    divNodata.Visible = false;
                    //13-01------------------------------------------------------------------
                    if (Request.QueryString[ERP.Utilities.QueryStrings.RPT_PK] != null)
                    {
                        Int32.TryParse(Request.QueryString[ERP.Utilities.QueryStrings.RPT_PK], out rptPK);
                    }
                    if (rptPK > 0)
                    {
                        Session[ERP.Utilities.SessionStrings.REPORTPK] = rptPK.ToString();
                        //Get dynamic controls
                        GetFieldValues(ControlsEnum.DEFAULT);
                        GetFieldValues(ControlsEnum.REPORT);
                        SetFieldValues(ControlsEnum.REPORT);
                        //Load Controls to UI
                        LoadControls();
                        tblSearch.Visible = true;
                        hdfShowFilter.Value = "1";
                    }

                    //--------------------------------------------------------------------

                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                customerRegistrationServiceClient = null;
            }
        }
        #endregion

        #region Helper Methods
        private void CheckUserRight()
        {
            string path = "/reports/commonreportviewer.aspx";
            currentUser = GetUserIdentity();
            //if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            //    path = Request.Url.PathAndQuery.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            //else
            //    path = Request.Url.PathAndQuery;
            if (Request.QueryString[ERP.Utilities.QueryStrings.ReportType] != null)
                path += "?" + ERP.Utilities.QueryStrings.ReportType + "=" + Request.QueryString[ERP.Utilities.QueryStrings.ReportType].ToString();
            // / reports / commonreportviewer.aspx ? reporttype = 11
            // int lastIdex = path.ToLower().IndexOf("&dep=") > 0 ? path.ToLower().IndexOf("&dep=") : path.ToLower().IndexOf("?dep=");
            // base.CheckUserRight(path.ToLower().Substring(0, lastIdex));
            base.CheckUserRight(path);
        }
        public BusinessObject.User GetUserIdentity()
        {
            try
            {
                return (((BusinessObject.User)(HttpContext.Current.User.Identity)));
            }
            catch (Exception ex)
            {
                Session.Abandon();
                FormsAuthentication.SignOut();
                Response.Redirect("~/Login.aspx");
            }
            return null;
        }
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
                    case ControlsEnum.REPORTGROUP:
                        if (admReportGroupCfgList != null && admReportGroupCfgList.Count > 0)
                        {
                            txtReportGroup.Text = admReportGroupCfgList[0].RGC_NAME;
                            hdfReportGroup.Value = admReportGroupCfgList[0].RGC_PK.ToString();
                        }
                        break;
                    case ControlsEnum.REPORT:
                        if (admReportCfgList != null && admReportCfgList.Count > 0)
                        {
                            lblBreadCrum.Text = this.GetLocalResourceObject("Breadcrumb").ToString() + " >> " + admReportCfgList[0].RPT_HEADING;
                            if (admReportCfgList[0].RPT_HAS_PROCESS == 1)
                                btnProcess.Visible = true;
                            else
                                btnProcess.Visible = false;
                        }
                        break;
                    #region  REPORT BY PK
                    case ControlsEnum.REPORTBYPK:
                        if (admReportCfgList != null && admReportCfgList.Count > 0)
                        {
                            txtReport.Text = admReportCfgList[0].RPT_NAME.ToString();
                            hdfReport.Value = rptPK.ToString();
                        }
                        break;
                        #endregion
                        //#region  HTMLREPORT
                        //case ControlsEnum.HTMLREPORT:
                        //    GenerateReport(ReportCode.HTML_SC_PROFIT);
                        //    break;
                        //#endregion

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private DataTable ConfigurationSettings()
        {
            currentUser = GetUserIdentity();
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ReportCurrency, string.Empty, currentUser.SBUID);
            return dt;
        }
        /// <summary>
        /// Method for Load the dynamic controls
        /// </summary>
        public void LoadControls()
        {
            try
            {
                pnlControls.Controls.Clear();
                BindControls();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorNew", "ShowHideAdvancedSearch('" + hdfShowFilter.Value + "');", true);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ActionsEnum mode, Object srcObj)
        {
            object returnObj;
            returnObj = null;
            string value;
            value = string.Empty;
            try
            {
                switch (mode)
                {
                    case ActionsEnum.SAVE:
                        Type targetTable = srcObj.GetType();
                        foreach (PropertyInfo p in targetTable.GetProperties())
                        {
                            if (p.CanWrite)
                            {
                                string controlID = p.Name;
                                //Check the control is in UI, if it is Get the type of the control
                                HiddenField hdfType = (HiddenField)pnlControls.FindControl("hdf" + controlID + "Type");
                                if (hdfType != null && !string.IsNullOrEmpty(hdfType.Value))
                                {
                                    #region case
                                    switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), hdfType.Value)))
                                    {
                                        #region Text
                                        case ControlTypes.Text:
                                            System.Web.UI.WebControls.TextBox txtCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                            if (txtCtrlId != null)
                                            {
                                                SetValue(srcObj, p, txtCtrlId.Text.Trim());
                                            }
                                            break;
                                        #endregion
                                        #region HourText
                                        case ControlTypes.HourText:
                                            System.Web.UI.WebControls.TextBox hrtCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                            if (hrtCtrlId != null)
                                            {
                                                if (hrtCtrlId.Text != string.Empty)
                                                {
                                                    string hrt = hrtCtrlId.Text.Trim().Replace('_', '0');
                                                    string[] time = hrt.Split(':');
                                                    value = ((string.IsNullOrEmpty(time[0]) ? 0 : (Convert.ToInt32(time[0]) * 60)) + (string.IsNullOrEmpty(time[1]) ? 0 : Convert.ToInt32(time[1]))).ToString();
                                                    SetValue(srcObj, p, value);
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region Date
                                        case ControlTypes.Date:
                                            System.Web.UI.WebControls.TextBox dateCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                            if (dateCtrlId != null)
                                            {
                                                string date = string.IsNullOrEmpty(dateCtrlId.Text) ? string.Empty : dateCtrlId.Text.Trim();
                                                if (date != string.Empty)
                                                {
                                                    SetValue(srcObj, p, date);
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region DateRange
                                        case ControlTypes.DateRange:
                                            System.Web.UI.WebControls.TextBox dateRangeCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                            if (dateRangeCtrlId != null)
                                            {
                                                string date = string.IsNullOrEmpty(dateRangeCtrlId.Text) ? string.Empty : dateRangeCtrlId.Text.Trim();
                                                if (date != string.Empty)
                                                {
                                                    SetValue(srcObj, p, date);
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region DateTime
                                        case ControlTypes.DateTime:
                                            System.Web.UI.WebControls.TextBox dateTimeCtrlId1 = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                            System.Web.UI.WebControls.TextBox dateTimeCtrlId2 = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID + "_Time");
                                            if (dateTimeCtrlId1 != null && dateTimeCtrlId2 != null)
                                            {
                                                string dateTime = string.IsNullOrEmpty(dateTimeCtrlId1.Text) ? string.Empty : dateTimeCtrlId1.Text.Trim();
                                                dateTime = dateTime + " " + (string.IsNullOrEmpty(dateTimeCtrlId2.Text) ? string.Empty : dateTimeCtrlId2.Text.Trim());
                                                if (dateTime != string.Empty)
                                                {
                                                    SetValue(srcObj, p, dateTime);
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region TimePicker
                                        case ControlTypes.TimePicker:
                                            System.Web.UI.WebControls.TextBox txtTime = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                            if (txtTime != null && !string.IsNullOrEmpty(txtTime.Text.Trim()))
                                            {
                                                value = string.IsNullOrEmpty(txtTime.Text.Trim()) ? string.Empty : txtTime.Text.Trim();
                                                SetValue(srcObj, p, value);
                                            }
                                            break;
                                        #endregion
                                        #region Numeric
                                        case ControlTypes.Numeric:
                                            System.Web.UI.WebControls.TextBox txtNumCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                            if (txtNumCtrlId != null)
                                            {
                                                string numVal = string.IsNullOrEmpty(txtNumCtrlId.Text.Trim()) ? "0" : txtNumCtrlId.Text.Trim();
                                                SetValue(srcObj, p, numVal);
                                            }
                                            break;
                                        #endregion
                                        #region DropDown
                                        case ControlTypes.DropDown:
                                            DropDownList ddlCtrlId = (DropDownList)pnlControls.FindControl(controlID);
                                            if (ddlCtrlId != null)
                                            {
                                                int intOut;
                                                if (Int32.TryParse(ddlCtrlId.SelectedValue, out intOut))
                                                {
                                                    if (Convert.ToInt32(ddlCtrlId.SelectedValue) > 0)
                                                    {
                                                        SetValue(srcObj, p, ddlCtrlId.SelectedValue);
                                                    }
                                                    else
                                                    {
                                                        HiddenField hdfRelId = (HiddenField)pnlControls.FindControl("hdf" + controlID + "HasRelatedId");
                                                        if (hdfRelId != null)
                                                        {
                                                            p.SetValue(srcObj, null, null);
                                                        }
                                                    }
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region Label
                                        case ControlTypes.Label:
                                            Label lblCtrlId = (Label)pnlControls.FindControl(controlID);
                                            if (lblCtrlId != null)
                                            {
                                                SetValue(srcObj, p, lblCtrlId.Text.Trim());
                                            }
                                            break;
                                        #endregion
                                        #region HiddenField
                                        case ControlTypes.HiddenField:
                                            HiddenField hdfCtrlId = (HiddenField)pnlControls.FindControl(controlID);
                                            if (hdfCtrlId != null)
                                            {
                                                if (hdfCtrlId.ID.EndsWith("MOD_BY"))
                                                {
                                                    SetValue(srcObj, p, currentUser.PKUser.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("MOD_DT"))
                                                {
                                                    SetValue(srcObj, p, DateTime.Now.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("ACTIVE"))
                                                {
                                                    SetValue(srcObj, p, "1");
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("STATUS"))
                                                {
                                                    SetValue(srcObj, p, "0");
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("BIZUNIT"))
                                                {
                                                    SetValue(srcObj, p, currentUser.SBUID.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("CRTD_BY"))
                                                {
                                                    SetValue(srcObj, p, currentUser.PKUser.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("CRTD_DT"))
                                                {
                                                    SetValue(srcObj, p, DateTime.Now.ToString());
                                                }
                                                else if (hdfCtrlId.ID.EndsWith("DELETED"))
                                                {
                                                    SetValue(srcObj, p, false.ToString());
                                                }
                                                else
                                                {
                                                    if (!string.IsNullOrEmpty(hdfCtrlId.Value))
                                                    {
                                                        SetValue(srcObj, p, hdfCtrlId.Value);
                                                    }
                                                }
                                            }
                                            break;
                                        #endregion
                                        #region TextArea
                                        case ControlTypes.TextArea:
                                            System.Web.UI.WebControls.TextBox txtAreaCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                            if (txtAreaCtrlId != null)
                                            {
                                                SetValue(srcObj, p, txtAreaCtrlId.Text.Trim());
                                            }
                                            break;
                                        #endregion
                                        #region FileUpload
                                        case ControlTypes.FileUpload:
                                            FileUpload fileUploadCtrlId = (FileUpload)pnlControls.FindControl(controlID);
                                            if (fileUploadCtrlId != null)
                                            {

                                                //if (fileUploadCtrlId.HasFile)
                                                //{
                                                //SetValue(srcObj, p, fileUploadCtrlId.FileName);
                                                //}
                                                /////Test start
                                                HttpPostedFile po = fileUploadCtrlId.PostedFile;
                                                string SavePath;
                                                FileInfo tempFileInfoObj;
                                                string attachmentFileFormat;
                                                string attachmentFilePath;
                                                FileInfo attachedFileInfo;
                                                if (fileUploadCtrlId.HasFile)
                                                {
                                                    SetValue(srcObj, p, fileUploadCtrlId.FileName);

                                                    SavePath = HttpContext.Current.Request.PhysicalApplicationPath + "/";
                                                    tempFileInfoObj = new FileInfo(fileUploadCtrlId.PostedFile.FileName);

                                                    if (!Directory.Exists(SavePath + "Upload/"))
                                                        Directory.CreateDirectory(SavePath + "Upload/");
                                                    attachmentFileFormat = tempFileInfoObj.Extension;
                                                    AttachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                    attachmentFilePath = "Upload/" + AttachmentFileName;
                                                    attachedFileInfo = new FileInfo(SavePath + attachmentFilePath);

                                                    HttpContext.Current.Request.Files[0].SaveAs(attachedFileInfo.FullName);
                                                }
                                                //else
                                                //{
                                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage("No file!") + "','" + Resources.ErpRes.Information + "');", true);
                                                //}

                                                if (AttachmentFileName == Guid.Empty.ToString() || AttachmentFileName == string.Empty)
                                                {
                                                    AttachmentFileName = string.Empty;
                                                }
                                                ////Test End
                                            }
                                            break;
                                        #endregion
                                        #region System.Web.UI.WebControls.CheckBox
                                        case ControlTypes.CheckBox:
                                            System.Web.UI.WebControls.CheckBox chkCtrlId = (System.Web.UI.WebControls.CheckBox)pnlControls.FindControl(controlID);
                                            if (chkCtrlId != null)
                                            {
                                                if (p.PropertyType == typeof(bool))
                                                {
                                                    value = chkCtrlId.Checked == true ? "True" : "False";
                                                }
                                                else if (p.PropertyType == typeof(byte) || p.PropertyType == typeof(byte?))
                                                {
                                                    value = chkCtrlId.Checked == true ? "1" : "0";
                                                }
                                                else if (p.PropertyType == typeof(Int16) || p.PropertyType == typeof(Int16?))
                                                {
                                                    value = chkCtrlId.Checked == true ? "1" : "0";
                                                }
                                                SetValue(srcObj, p, value);
                                            }
                                            break;
                                        #endregion
                                        #region  Default
                                        default:
                                            break;
                                            #endregion
                                    }
                                    #endregion
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
                returnObj = srcObj;
                return returnObj;
            }
            catch
            {
                throw;
            }
            finally
            {
            }
        }

        /// <summary>
        /// Set Values To Object
        /// </summary>
        /// <param name="src"></param>
        /// <param name="p"></param>
        /// <param name="value"></param>
        private void SetValue(object src, PropertyInfo p, string value)
        {
            Type ptype = p.PropertyType;
            if (ptype == typeof(byte))
                p.SetValue(src, Convert.ToByte(value), null);
            if (ptype == typeof(string))
                p.SetValue(src, HttpUtility.HtmlEncode(value), null);
            else if (ptype == typeof(int) || ptype == typeof(int?))
                p.SetValue(src, Convert.ToInt32(value), null);
            else if (ptype == typeof(Int64) || ptype == typeof(Int64?))
                p.SetValue(src, Convert.ToInt64(value), null);
            else if (ptype == typeof(Int32) || ptype == typeof(Int32?))
                p.SetValue(src, Convert.ToInt32(value), null);
            else if (ptype == typeof(short) || ptype == typeof(short?))
                p.SetValue(src, Convert.ToInt16(value), null);
            else if (ptype == typeof(float) || ptype == typeof(float?))
                p.SetValue(src, float.Parse(value), null);
            else if (ptype == typeof(Double) || ptype == typeof(Double?))
                p.SetValue(src, Double.Parse(value), null);
            else if (ptype == typeof(decimal) || ptype == typeof(decimal?))
                p.SetValue(src, Convert.ToDecimal(value), null);
            else if (ptype == typeof(bool) || ptype == typeof(bool?))
                p.SetValue(src, Convert.ToBoolean(value), null);
            else if (ptype == typeof(DateTime) || ptype == typeof(DateTime?))
                p.SetValue(src, Convert.ToDateTime(value), null);

        }

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ActionsEnum mode, Object srcObj)
        {
            try
            {
                Type targetTable;
                targetTable = srcObj.GetType();
                foreach (PropertyInfo p in targetTable.GetProperties())
                {
                    if (p.CanWrite)
                    {
                        string controlID = p.Name;
                        //Check the control is in UI, if it is Get the type of the control
                        HiddenField hdfType = (HiddenField)pnlControls.FindControl("hdf" + controlID + "Type");
                        if (hdfType != null && !string.IsNullOrEmpty(hdfType.Value))
                        {
                            #region case
                            switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), hdfType.Value)))
                            {
                                #region Fill
                                #region Text
                                case ControlTypes.Text:
                                    System.Web.UI.WebControls.TextBox txtCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                    if (txtCtrlId != null)
                                    {
                                        txtCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : HttpUtility.HtmlDecode(p.GetValue(srcObj, null).ToString());
                                    }
                                    break;
                                #endregion
                                #region HourText
                                case ControlTypes.HourText:
                                    System.Web.UI.WebControls.TextBox hrtCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                    if (hrtCtrlId != null)
                                    {
                                        int hour = p.GetValue(srcObj, null) == null ? 0 : p.GetValue(srcObj, null).ToString() == string.Empty ? 0 : Convert.ToInt32(p.GetValue(srcObj, null)) / 60;
                                        int min = p.GetValue(srcObj, null) == null ? 0 : p.GetValue(srcObj, null).ToString() == string.Empty ? 0 : Convert.ToInt32(p.GetValue(srcObj, null)) % 60;
                                        hrtCtrlId.Text = hour + min > 0 ? hour + ":" + min : "000:00";
                                    }
                                    break;
                                #endregion
                                #region Date
                                case ControlTypes.Date:
                                    System.Web.UI.WebControls.TextBox dateCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                    if (dateCtrlId != null)
                                    {
                                        string ss = Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                        dateCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                    }
                                    break;
                                #endregion
                                #region DateRange
                                case ControlTypes.DateRange:
                                    System.Web.UI.WebControls.TextBox dateRangeCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                    if (dateRangeCtrlId != null)
                                    {
                                        string ss = Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                        dateRangeCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                    }
                                    break;
                                #endregion
                                #region DateTime
                                case ControlTypes.DateTime:
                                    System.Web.UI.WebControls.TextBox dateTimeCtrlId1 = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                    System.Web.UI.WebControls.TextBox dateTimeCtrlId2 = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID + "_Time");
                                    if (dateTimeCtrlId1 != null)
                                    {
                                        dateTimeCtrlId1.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                                    }
                                    if (dateTimeCtrlId2 != null)
                                    {
                                        dateTimeCtrlId2.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.TimeFormat);
                                    }
                                    break;
                                #endregion
                                #region TimePicker
                                case ControlTypes.TimePicker:
                                    System.Web.UI.WebControls.TextBox txtTime = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                    if (txtTime != null)
                                    {
                                        txtTime.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).Equals(string.Empty) ? string.Empty : Convert.ToDateTime(p.GetValue(srcObj, null)).ToString(Resources.ErpRes.TimeFormat);
                                    }
                                    break;
                                #endregion
                                #region Numeric
                                case ControlTypes.Numeric:
                                    System.Web.UI.WebControls.TextBox txtNumCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                    if (txtNumCtrlId != null)
                                    {
                                        string decValInDB = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).ToString();
                                        if (!string.IsNullOrEmpty(decValInDB))
                                        {
                                            HiddenField hdfDecimalPart = (HiddenField)pnlControls.FindControl("hdf" + controlID + "DecimalPart");
                                            if (hdfDecimalPart != null)
                                            {
                                                int decVal = string.IsNullOrEmpty(hdfDecimalPart.Value) ? 0 : Convert.ToInt32(hdfDecimalPart.Value);
                                                if (decVal == 0)
                                                {
                                                    txtNumCtrlId.Text = decValInDB;
                                                }
                                                else
                                                {
                                                    decimal val = Convert.ToDecimal(decValInDB);
                                                    txtNumCtrlId.Text = Math.Round(val, decVal).ToString();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            txtNumCtrlId.Text = string.Empty;
                                        }
                                    }
                                    break;
                                #endregion
                                #region DropDown
                                case ControlTypes.DropDown:
                                    DropDownList ddlCtrlId = (DropDownList)pnlControls.FindControl(controlID);
                                    if (ddlCtrlId != null)
                                    {
                                        ddlCtrlId.SelectedValue = p.GetValue(srcObj, null) == null ? CommonConstants.SELECTVAL : p.GetValue(srcObj, null).ToString();
                                    }
                                    break;
                                #endregion
                                #region Label
                                case ControlTypes.Label:
                                    Label lblCtrlId = (Label)pnlControls.FindControl(controlID);
                                    if (lblCtrlId != null)
                                    {
                                        lblCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).ToString();
                                    }
                                    break;
                                #endregion
                                #region HiddenField
                                case ControlTypes.HiddenField:
                                    HiddenField hdfCtrlId = (HiddenField)pnlControls.FindControl(controlID);
                                    if (hdfCtrlId != null)
                                    {
                                        hdfCtrlId.Value = p.GetValue(srcObj, null) == null ? string.Empty : p.GetValue(srcObj, null).ToString();
                                    }
                                    break;
                                #endregion
                                #region TextArea
                                case ControlTypes.TextArea:
                                    System.Web.UI.WebControls.TextBox txtAreaCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(controlID);
                                    if (txtAreaCtrlId != null)
                                    {
                                        txtAreaCtrlId.Text = p.GetValue(srcObj, null) == null ? string.Empty : HttpUtility.HtmlDecode(p.GetValue(srcObj, null).ToString());
                                    }
                                    break;
                                #endregion
                                #region FileUpload
                                case ControlTypes.FileUpload:
                                    FileUpload fileUploadCtrlId = (FileUpload)pnlControls.FindControl(controlID);
                                    if (fileUploadCtrlId != null)
                                    {
                                        //fileUploadCtrlId.FileName = p.GetValue(srcObj, null) == null ? string.Empty : (string)p.GetValue(srcObj, null);
                                    }
                                    break;
                                #endregion
                                #region System.Web.UI.WebControls.CheckBox
                                case ControlTypes.CheckBox:
                                    System.Web.UI.WebControls.CheckBox chkCtrlId = (System.Web.UI.WebControls.CheckBox)pnlControls.FindControl(controlID);
                                    if (chkCtrlId != null)
                                    {
                                        if (p.GetValue(srcObj, null) == null)
                                        {
                                            chkCtrlId.Checked = false;
                                        }
                                        else
                                        {
                                            if (p.PropertyType == typeof(byte) || p.PropertyType == typeof(byte?))
                                            {
                                                if (Convert.ToByte(p.GetValue(srcObj, null)) == 1)
                                                {
                                                    chkCtrlId.Checked = true;
                                                }
                                                else
                                                {
                                                    chkCtrlId.Checked = false;
                                                }
                                            }
                                            else if (p.PropertyType == typeof(Int16) || p.PropertyType == typeof(Int16?))
                                            {
                                                short intOut;
                                                if (Int16.TryParse(p.GetValue(srcObj, null).ToString(), out intOut))
                                                {
                                                    if (intOut == 1)
                                                    {
                                                        chkCtrlId.Checked = true;
                                                    }
                                                    else
                                                    {
                                                        chkCtrlId.Checked = false;
                                                    }
                                                }
                                            }
                                            else if (p.PropertyType == typeof(bool) || p.PropertyType == typeof(bool?))
                                            {
                                                if ((bool)p.GetValue(srcObj, null))
                                                {
                                                    chkCtrlId.Checked = true;
                                                }
                                            }
                                            else
                                            {
                                                chkCtrlId.Checked = false;
                                            }
                                        }
                                    }
                                    break;
                                #endregion
                                #region  Default
                                default:
                                    break;
                                    #endregion
                                    #endregion
                            }
                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Bind the dynamic controls 
        /// </summary>
        /// <returns></returns>  
        private void BindControls()
        {
            CommonService commonService;
            commonService = null;
            System.Web.UI.WebControls.Panel pnl = new System.Web.UI.WebControls.Panel();
            int Cols;
            string divGroupStyle = "";
            string divColStyle = "";
            short tabIndex = 1;
            HiddenField hdnType;
            System.Web.UI.WebControls.TableCell tempTableCell;
            HtmlGenericControl tempDiv;
            int tempSequence = 0;
            Dictionary<int, string> dicEntityGroup;
            dicEntityGroup = new Dictionary<int, string>();
            List<string> validationGroupList;
            validationGroupList = new List<string>();
            System.Web.UI.WebControls.TableCell hiddenTableCell;
            hiddenTableCell = new System.Web.UI.WebControls.TableCell();
            hiddenTableCell.Visible = false;
            System.Web.UI.WebControls.Table hiddenTable = new System.Web.UI.WebControls.Table();
            hiddenTable.Visible = false;
            TableRow hiddenTableRow = new TableRow();
            hiddenTableRow.Visible = false;
            string tempDivGroupStyle = "";
            string tempDivColStyle = "";
            bool isButtonGroup = true;
            string validaionGroup;
            string[] valdationGroupArray;
            Dictionary<string, Control> tempDict;
            //Get the column number of UI
            Cols = 2;//Convert.ToInt32(adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("ColLayoutActivity").ToString()).CNS_Value);
            if (Cols == 1)//One column UI
            {
                divColStyle = "divcolmiddle-S";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("SingleColStyle").ToString()).CNS_Data;
                tempDivColStyle = divColStyle;
                divGroupStyle = "fields-grpwrap single";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("SingleColGroupStyle").ToString()).CNS_Data;
                tempDivGroupStyle = divGroupStyle;
            }
            else//Greater than one column UI
            {
                divColStyle = GetLocalResourceObject("div2colstyle").ToString();//"div2col-report";//Old style -div2col-M // adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("DoubleColStyle").ToString()).CNS_Data;
                tempDivColStyle = divColStyle;
                divGroupStyle = "fields-grpwrap";// adAppConstCfgList.FirstOrDefault(cns => cns.CNS_Setting == GetLocalResourceObject("DoubleColGroupStyle").ToString()).CNS_Data;
                tempDivGroupStyle = divGroupStyle;
            }
            PageScript = " function InitComponents(flag) {";//starting of initcomponent script

            //For Autocomplete 
            //string virtualPath = getVirtualPathForAutocomplete();
            //PageScript = PageScript + virtualPath;
            string disableReportGrp = string.Empty; ;
            if (Request.QueryString[ERP.Utilities.QueryStrings.ReportType] != null)
            {
                Int32.TryParse(Request.QueryString[ERP.Utilities.QueryStrings.ReportType], out id);
            }
            if (id > 0)
            {
                GetFieldValues(ControlsEnum.REPORTGROUP);
                SetFieldValues(ControlsEnum.REPORTGROUP);
                disableReportGrp = "DisableAuto($('[id$=txtReportGroup]'), $('[id$=hdfReportGroup]'));";

            }

            string reportGroupAutoComplete = "GrandScriptUtils.MakeAutoCompleteDDL('txtReportGroup', url, 'hdfReportGroup', true, true, 'REPORTGROUP');";
            string reportNameAutoComplete = string.Empty;

            if (id > 0)
            {
                reportNameAutoComplete = "if ($('[id$=hdfReportGroup]').val() != '' && $('[id$=hdfReportGroup]').val() != '0') {" +
               "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url + '&Type=' + $('[id$=hdfReportGroup]').val(), 'hdfReport', true, true, 'REPORT');}" +
               "else {" +
               "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url, 'hdfReport', true, true, 'REPORT');}";
            }
            else
            {
                reportNameAutoComplete = "if ($('[id$=hdfReportGroup]').val() != '' && $('[id$=hdfReportGroup]').val() != '0') {" +
                    "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url + '?Type=' + $('[id$=hdfReportGroup]').val(), 'hdfReport', true, true, 'REPORT');}" +
                    "else {" +
                    "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url, 'hdfReport', true, true, 'REPORT');}BindSubReport();";
            }//string reportNameAutoComplete = "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url, 'hdfReport', true, true, 'REPORT');";
            if (rptPK > 0)
            {
                SetFieldValues(ControlsEnum.REPORTBYPK);
              //  disableReportGrp += "DisableAuto($('[id$=txtReport]'), $('[id$=hdfReport]'));";
            }
            PageScript = PageScript + reportGroupAutoComplete + reportNameAutoComplete + disableReportGrp;

            DataTable dtFieldControls = LINQToDataTable(spAdmReportControlCfgGetResultList);
            DataTable tempDT = dtFieldControls.Copy();
            try
            {
                #region BindControls
                if (dtFieldControls.Rows.Count > 0)
                {
                    tempDict = new Dictionary<string, Control>();
                    //Data Table Iterate
                    for (int i = 0; i <= dtFieldControls.Rows.Count; i++)
                    {
                        divGroupStyle = tempDivGroupStyle;
                        divColStyle = tempDivColStyle;
                        //create temp table cell
                        tempTableCell = new System.Web.UI.WebControls.TableCell();
                        //set temp div
                        tempDiv = new HtmlGenericControl("div");
                        tempSequence = 0;
                        //Get the group number of the controls
                        int order = int.Parse(dtFieldControls.Rows[0]["RCC_CONTROL_GROUP"].ToString());
                        //Filter table with the current group number
                        DataRow[] drFieldControls = dtFieldControls.Select("RCC_CONTROL_GROUP = " + order);
                        DataRow[] tempDrFieldControls = null;
                        HtmlGenericControl divMainGroup = new HtmlGenericControl("div");
                        //Set the Group Div ID
                        if (dtFieldControls.Rows[0]["RCG_CONTROL_ID"] != null && !string.IsNullOrEmpty(dtFieldControls.Rows[0]["RCG_CONTROL_ID"].ToString()))
                        {
                            divMainGroup.ID = dtFieldControls.Rows[0]["RCG_CONTROL_ID"].ToString();
                            divMainGroup.ClientIDMode = ClientIDMode.Static;
                        }

                        HtmlGenericControl divSubGroup = new HtmlGenericControl("div");

                        //divSubGroup.Attributes.Add("runat", "server");
                        //Get and Set the Div Group Style
                        if (dtFieldControls.Rows[0]["RCC_CONTROL_GROUP_STYLE"] != null && !string.IsNullOrEmpty(dtFieldControls.Rows[0]["RCC_CONTROL_GROUP_STYLE"].ToString()))
                        {
                            divGroupStyle = dtFieldControls.Rows[0]["RCC_CONTROL_GROUP_STYLE"].ToString();
                        }
                        divMainGroup.Attributes.Add("class", divGroupStyle);
                        divSubGroup.Attributes.Add("class", "fields-group");
                        if (drFieldControls.Count() > 0)
                        {
                            tempDrFieldControls = dtFieldControls.Copy().Select("RCC_CONTROL_GROUP = " + order);
                            //Set the header text of the main group
                            if (drFieldControls[0]["RCC_CONTROL_GROUP_TEXT"] != null && !string.IsNullOrEmpty(drFieldControls[0]["RCC_CONTROL_GROUP_TEXT"].ToString()))
                            {
                                HtmlGenericControl groupHead = new HtmlGenericControl("h1");
                                groupHead.InnerText = drFieldControls[0]["RCC_CONTROL_GROUP_TEXT"].ToString();
                                HtmlGenericControl divGroupHeadClear = new HtmlGenericControl("div");
                                divGroupHeadClear.Attributes.Add("class", GetLocalResourceObject("Class_Clear").ToString());
                                divMainGroup.Controls.Add(groupHead);
                                divMainGroup.Controls.Add(divGroupHeadClear);
                            }
                            //for Set the entity group session
                            if (drFieldControls[0]["RCG_QUERY_TEXT"] != null)
                            {
                                if (!string.IsNullOrEmpty(drFieldControls[0]["RCG_QUERY_TEXT"].ToString()))
                                {
                                    string myValue = string.Empty;
                                    myValue = dicEntityGroup.FirstOrDefault(x => x.Key == order).Value;
                                    if (string.IsNullOrEmpty(myValue))
                                    {
                                        //The dictionary contains Group number as the key and entity name as the value
                                        dicEntityGroup.Add(order, drFieldControls[0]["RCG_QUERY_TEXT"].ToString());
                                    }
                                }
                            }
                        }
                        //Data Row Iterate
                        string MonthPickerControlScript = string.Empty;
                        for (int k = 0; k < drFieldControls.Count();)
                        {
                            //Create new Table Object
                            System.Web.UI.WebControls.Table tbControls = new System.Web.UI.WebControls.Table();
                            if (drFieldControls[k]["RCC_IS_FULL_LENGTH"] != null && drFieldControls[k]["RCC_IS_FULL_LENGTH"].ToString() == "1")//If Full Length Control
                            {
                                tbControls.CssClass = "";
                                divColStyle = GetLocalResourceObject("div1colstyle").ToString();
                            }
                            else
                                if (drFieldControls.Count() > 1)
                            {
                                tbControls.CssClass = "table-devide";
                                divColStyle = tempDivColStyle;
                            }
                            else if (drFieldControls.Count() == 1 && Cols > 1
                                && ((drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()).Equals("TextArea")
                                || (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()).Equals("Label")))//TextArea and the Label can only come in full length row(column number >1)
                            {
                                tbControls.CssClass = "";
                                divColStyle = "";
                            }
                            else
                            {
                                tbControls.CssClass = "table-devide";
                                divColStyle = tempDivColStyle;
                            }
                            //Create new TableRow Object
                            TableRow trControls = new TableRow();
                            int j = 0;
                            //Column Iterate
                            for (j = 0; j < Cols && k < drFieldControls.Count();)
                            {
                                System.Web.UI.WebControls.TableCell tcControl;
                                HtmlGenericControl div;
                                tcControl = new System.Web.UI.WebControls.TableCell();
                                div = new HtmlGenericControl("div");
                                if (!(drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()).Equals("HiddenField"))
                                {
                                    if ((int.Parse(drFieldControls[k]["RCC_SEQUENCE"].ToString()) == tempSequence &&
                                        int.Parse(drFieldControls[k]["RCC_CONTROL_GROUP"].ToString()) == order))//If more than one controls have same sequence and same order, that controls sholud come in the same cell. 
                                    {
                                        tcControl = tempTableCell;//will not create new table cell, but keep the last cell as the current cell so that the new control can add to the same cell
                                        div = tempDiv;//Div-same as cell
                                    }
                                    else
                                    {
                                        //Create new table cell
                                        tcControl = new System.Web.UI.WebControls.TableCell();
                                        //create new inner div
                                        div = new HtmlGenericControl("div");
                                        bool isGridButton = false;
                                        if (k > 0 && (tempDrFieldControls[k - 1]["RCC_CONTROL_TEXT"].ToString().Equals("Button")
                                            || tempDrFieldControls[k - 1]["RCC_CONTROL_TEXT"].ToString().Equals("LinkButton")))//If control is not the first one in the current group and it is a button or link button
                                        {
                                            if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))//check the current control has a related control id
                                            {
                                                if (tempDrFieldControls[k - 1]["RCC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(tempDrFieldControls[k - 1]["RCC_REL_CONTROL_ID"].ToString()))//check the previous control has a related control id
                                                {
                                                    if (tempDrFieldControls[k - 1]["RCC_REL_CONTROL_ID"].ToString().Equals(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))//check the current and previous controls have the same related id
                                                    {
                                                        isGridButton = true;//It is a grid button
                                                    }
                                                    else
                                                    {
                                                        isGridButton = false;
                                                    }
                                                }
                                                else
                                                {
                                                    isGridButton = false;
                                                }
                                            }
                                            else
                                            {
                                                isGridButton = false;
                                            }

                                            //Check whether the group only contains buttons or linkbuttons or not
                                            DataRow[] tempDr = tempDT.Select("RCC_CONTROL_GROUP = " + order);
                                            isButtonGroup = true;
                                            for (int x = 0; x < tempDr.Count(); x++)
                                            {
                                                if (!tempDr[x]["RCC_CONTROL_TEXT"].ToString().Equals("Button")
                                                    && !tempDr[x]["RCC_CONTROL_TEXT"].ToString().Equals("LinkButton"))//check the control is a button or linkbutton. if any control is not a but or lnkbut, its not a button group
                                                {
                                                    isButtonGroup = false;
                                                    break;
                                                }
                                            }
                                        }
                                        else//either the control is the first control in the group or its not button or linkbutton
                                        {
                                            isGridButton = false;
                                            isButtonGroup = false;
                                        }

                                        if (isGridButton || isButtonGroup)//if true will not create new table cell, but keep the last cell as the current cell so that the new control can add to the same cell
                                        {
                                            tcControl = tempTableCell;
                                            div = tempDiv;
                                        }
                                        else//if false create new table cell and inner div
                                        {
                                            tempTableCell = new System.Web.UI.WebControls.TableCell();
                                            tempDiv = new HtmlGenericControl("div");
                                        }

                                        //To confirm its a button group or not .. if it is, then there will not be a label in front of the button.. 
                                        if (k == 0 && (tempDrFieldControls[k]["RCC_CONTROL_TEXT"].ToString().Equals("Button")
                                            || tempDrFieldControls[k]["RCC_CONTROL_TEXT"].ToString().Equals("LinkButton")))
                                        {
                                            DataRow[] tempDr = tempDT.Select("RCC_CONTROL_GROUP = " + order);
                                            isButtonGroup = true;
                                            for (int x = 0; x < tempDr.Count(); x++)
                                            {
                                                if (!tempDr[x]["RCC_CONTROL_TEXT"].ToString().Equals("Button")
                                                    && !tempDr[x]["RCC_CONTROL_TEXT"].ToString().Equals("LinkButton"))
                                                {
                                                    isButtonGroup = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                //set the temsequence
                                tempSequence = int.Parse(drFieldControls[k]["RCC_SEQUENCE"].ToString());
                                //set style for the inner div
                                div.Attributes.Add("class", divColStyle);
                                //Get Validation Group
                                if (drFieldControls[k]["RCC_VALD_GROUP"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_VALD_GROUP"].ToString()))
                                {
                                    validaionGroup = drFieldControls[k]["RCC_VALD_GROUP"].ToString();
                                    valdationGroupArray = validaionGroup.Split(',');
                                    foreach (string valGroup in valdationGroupArray)
                                    {
                                        if (!validationGroupList.Contains(valGroup))
                                        {
                                            validationGroupList.Add(valGroup);
                                        }
                                    }
                                }
                                else
                                {
                                    validaionGroup = string.Empty;
                                    valdationGroupArray = new string[0];
                                }
                                switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), drFieldControls[k]["RCC_CONTROL_TEXT"].ToString())))
                                {
                                    #region Controls
                                    case ControlTypes.UserControl:
                                        IsUserControl = true;
                                        UserControlFile = "~/Reports/" + drFieldControls[k]["RCC_CONTROL_ID"].ToString();

                                        Control myUsrControl = this.LoadControl(UserControlFile) as System.Web.UI.UserControl;
                                        myUsrControl.ID = UserControlID;
                                        customControl = myUsrControl as ERPSMS_v01.Reports.UserControls.IFilterControl;
                                        //getUserFilter aa = new getUserFilter(customControl.GetReportParameters);
                                        div.Controls.Add((Control)customControl);
                                        break;
                                    #region Header
                                    case ControlTypes.Header:
                                        div.Controls.Add(new HtmlGenericControl("h3")
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            InnerText = drFieldControls[k]["RCC_NAME"].ToString()
                                        });
                                        tabIndex--;
                                        break;
                                    #endregion
                                    #region Label
                                    case ControlTypes.Label:
                                        Label lbl = new Label()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                            AssociatedControlID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static
                                        };
                                        div.Controls.Add(lbl);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        tabIndex--;
                                        divSubGroup.Attributes.Add("class", "group-note");//Label have a defferent style
                                        break;
                                    #endregion
                                    #region Text
                                    case ControlTypes.Text:
                                        if (tcControl != tempTableCell)//add Label only if it is not related to any other control
                                        {
                                            if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                    Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                    AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString()
                                                });
                                            }
                                        }
                                        System.Web.UI.WebControls.TextBox txt = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            MaxLength = 50,
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                txt.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["RCC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_LENGTH"].ToString() != string.Empty)
                                                txt.MaxLength = Convert.ToInt32(drFieldControls[k]["RCC_LENGTH"].ToString());
                                        }
                                        div.Controls.Add(txt);
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrftxt = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = txt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrftxt);
                                                }
                                            }
                                        }
                                        HiddenField hdnText = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_PK"].ToString()
                                        };
                                        div.Controls.Add(hdnText);
                                        //Set the Regular expression for the text. ex: email
                                        if (drFieldControls[k]["RCC_FORMAT"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_FORMAT"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                RegularExpressionValidator regExObj = new RegularExpressionValidator()
                                                {
                                                    ID = "rev" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    CssClass = "star",
                                                    ControlToValidate = txt.ID,
                                                    SetFocusOnError = true,
                                                    ErrorMessage = "Invalid " + drFieldControls[k]["RCC_NAME"].ToString(),
                                                    ValidationExpression = drFieldControls[k]["RCC_FORMAT"].ToString().Trim(),
                                                    EnableClientScript = true,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*"
                                                };
                                                div.Controls.Add(regExObj);
                                            }
                                        }

                                        //add the compare validator if needed
                                        if (drFieldControls[k]["RCC_CMP_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_CMP_CONTROL_ID"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CompareValidator comValObj = new CompareValidator()
                                                {
                                                    ID = "comVal" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = drFieldControls[k]["RCC_CMP_CONTROL_ID"].ToString(),
                                                    ControlToCompare = txt.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ErrorMessage = "Mismatch " + drFieldControls[k]["RCC_NAME"].ToString()
                                                };
                                                div.Controls.Add(comValObj);
                                            }
                                        }

                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region HourText
                                    case ControlTypes.HourText:
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        System.Web.UI.WebControls.TextBox hrtxt = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                hrtxt.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["RCC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_LENGTH"].ToString() != string.Empty)
                                                hrtxt.MaxLength = Convert.ToInt32(drFieldControls[k]["RCC_LENGTH"].ToString());
                                        }
                                        hrtxt.Attributes.Add("onkeypress", "ValidateText(event,this)");
                                        div.Controls.Add(hrtxt);
                                        MaskedEditExtender cc1 = new MaskedEditExtender();
                                        cc1.ID = "mee" + drFieldControls[k]["RCC_CONTROL_ID"].ToString();
                                        cc1.AutoComplete = false;
                                        cc1.Mask = "999:99";
                                        cc1.MaskType = AjaxControlToolkit.MaskedEditType.Time;
                                        cc1.TargetControlID = hrtxt.ID;
                                        div.Controls.Add(cc1);
                                        for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                        {
                                            RegularExpressionValidator rev = new RegularExpressionValidator()
                                            {
                                                ID = "rev" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ValidationGroup = valdationGroupArray[valCount],
                                                CssClass = "star",
                                                ControlToValidate = hrtxt.ID,
                                                SetFocusOnError = true,
                                                ErrorMessage = "Invalid Time",
                                                ValidationExpression = "^([0-9]{0,3}):([0-5][0-9])?$",
                                                EnableClientScript = true,
                                                Display = ValidatorDisplay.Dynamic,
                                                Text = "*"
                                            };
                                            div.Controls.Add(rev);
                                        }
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfHrtxt = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = hrtxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrfHrtxt);
                                                }
                                            }
                                        }
                                        //set the PK of the control
                                        HiddenField hdnhrText = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnhrText);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region TextArea
                                    case ControlTypes.TextArea:
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        System.Web.UI.WebControls.TextBox txtArea = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            TextMode = TextBoxMode.MultiLine,
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                txtArea.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["RCC_LENGTH"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_LENGTH"].ToString()))
                                        {
                                            int maxLength = Convert.ToInt32(drFieldControls[k]["RCC_LENGTH"].ToString());
                                            txtArea.Attributes.Add("onkeydown", "limitText(this," + maxLength + ");");
                                            txtArea.Attributes.Add("onkeyup", "limitText(this," + maxLength + ");");
                                        }

                                        int cols = Convert.ToInt32(GetLocalResourceObject("TextAreaCols").ToString());
                                        txtArea.Columns = cols;
                                        div.Controls.Add(txtArea);
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfTxtArea = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = txtArea.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrfTxtArea);
                                                }
                                            }
                                        }
                                        HiddenField hdnTextArea = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnTextArea);
                                        //Set style for div if only the textarea comes under a group in 2 col style
                                        if (drFieldControls.Count() == 1 && Cols > 1)
                                        {
                                            div.Attributes.Add("class", "divcol-M");
                                        }
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region Date
                                    case ControlTypes.Date:
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        System.Web.UI.WebControls.TextBox date = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                date.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        tabIndex++;
                                        date.Attributes.Add("onkeydown", "return false");
                                        date.Attributes.Add("onpaste", "return false");
                                        date.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);
                                        div.Controls.Add(date);
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfDate = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = date.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString() + " Date"
                                                    };
                                                    div.Controls.Add(vrfDate);
                                                }
                                            }
                                        }

                                        HiddenField hdnDateP = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnDateP);
                                        HiddenField hdfDateP = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfDateP);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["RCC_CONTROL_TEXT"].ToString(), date.ID, null, null, null);
                                        break;
                                    #endregion
                                    #region DateRange
                                    case ControlTypes.DateRange:
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        System.Web.UI.WebControls.TextBox dateRange = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                dateRange.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        tabIndex++;
                                        dateRange.Attributes.Add("onkeydown", "return false");
                                        dateRange.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(dateRange);
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfDate = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = dateRange.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString() + " Date"
                                                    };
                                                    div.Controls.Add(vrfDate);
                                                }
                                            }
                                        }

                                        HiddenField hdnDateRange = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnDateRange);
                                        HiddenField hdfDateRange = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfDateRange);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        //0/0|0/-1/1
                                        string config = string.Empty;
                                        if (drFieldControls[k]["RCC_VALUE"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_VALUE"].ToString()))
                                        {
                                            config = !string.IsNullOrEmpty(drFieldControls[k]["RCC_VALUE"].ToString()) ? drFieldControls[k]["RCC_VALUE"].ToString() : string.Empty;
                                            dateRange.Text = CommonFunctions.SetDateWithConfiguration(config).ToString(Resources.ErpRes.DateFormatShort);
                                        }

                                        if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))
                                        {
                                            string fromDate = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString();
                                            string hdfFrmDate = "hdf" + drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString();
                                            PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["RCC_CONTROL_TEXT"].ToString(), dateRange.ID, hdfDateRange.ID, fromDate, hdfFrmDate);
                                            if (string.IsNullOrEmpty(dateRange.Text))
                                            {
                                                dateRange.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormatShort);//Current Date
                                            }
                                        }
                                        else//FromDate
                                        {
                                            if (string.IsNullOrEmpty(dateRange.Text))
                                            {
                                                dateRange.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                                            }
                                        }
                                        break;
                                    #endregion
                                    #region DateTime
                                    case ControlTypes.DateTime:
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        System.Web.UI.WebControls.TextBox dttxt = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                dttxt.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        tabIndex++;
                                        dttxt.Attributes.Add("onkeydown", "return false");
                                        dttxt.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(dttxt);
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfDate = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = dttxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString() + " Date"
                                                    };
                                                    div.Controls.Add(vrfDate);
                                                }
                                            }
                                        }

                                        System.Web.UI.WebControls.TextBox dttimetxt = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "_Time",
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",

                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                dttimetxt.CssClass = GetLocalResourceObject("TimePickerStyle_DateTime").ToString();
                                        }
                                        dttimetxt.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(dttimetxt);
                                        for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                        {
                                            RegularExpressionValidator vreTime = new RegularExpressionValidator()
                                            {
                                                ID = "vre" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ControlToValidate = dttimetxt.ID,
                                                CssClass = "star",
                                                SetFocusOnError = true,
                                                ValidationGroup = valdationGroupArray[valCount],
                                                EnableClientScript = true,
                                                ValidationExpression = "^([01]?[0-9]|2[0-3]):[0-5][0-9]?$",
                                                Display = ValidatorDisplay.Dynamic,
                                                Text = "*",
                                                ErrorMessage = "Enter valid " + drFieldControls[k]["RCC_NAME"].ToString() + " Time"
                                            };
                                            div.Controls.Add(vreTime);
                                        }
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfTime = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "_Time",
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = dttimetxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString() + " Time"
                                                    };
                                                    div.Controls.Add(vrfTime);
                                                }
                                            }
                                        }
                                        HiddenField hdnDate = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnDate);

                                        HiddenField hdnDateTime = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "_Time",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnDateTime);

                                        HiddenField hdfDate = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfDate);
                                        HiddenField hdfTime = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "_Time",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdfTime);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["RCC_CONTROL_TEXT"].ToString(), dttxt.ID, null, null, null);
                                        break;
                                    #endregion
                                    #region TimePicker
                                    case ControlTypes.TimePicker:
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        System.Web.UI.WebControls.TextBox tmtxt = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                tmtxt.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        tmtxt.Attributes.Add("onpaste", "return false");
                                        div.Controls.Add(tmtxt);
                                        for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                        {
                                            RegularExpressionValidator vreTimePick = new RegularExpressionValidator()
                                            {
                                                ID = "vre" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ControlToValidate = tmtxt.ID,
                                                CssClass = "star",
                                                SetFocusOnError = true,
                                                ValidationGroup = valdationGroupArray[valCount],
                                                EnableClientScript = true,
                                                ValidationExpression = "^([01]?[0-9]|2[0-3]):[0-5][0-9]?$",
                                                Display = ValidatorDisplay.Dynamic,
                                                Text = "*",
                                                ErrorMessage = "Enter valid " + drFieldControls[k]["RCC_NAME"].ToString()
                                            };
                                            div.Controls.Add(vreTimePick);
                                        }
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfTimePick = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = tmtxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrfTimePick);
                                                }
                                            }
                                        }
                                        HiddenField hdnTime = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnTime);
                                        //tmtxt.Attributes.Add("onkeydown", "return false");
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        PageScript = PageScript + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["RCC_CONTROL_TEXT"].ToString(), tmtxt.ID, null, null, null);
                                        break;
                                    #endregion
                                    #region Numeric
                                    case ControlTypes.Numeric:
                                        if (tcControl != tempTableCell)//add Label only if it is not related to any other control
                                        {
                                            if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                    Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                    AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                    ClientIDMode = ClientIDMode.Static
                                                });
                                            }
                                        }
                                        System.Web.UI.WebControls.TextBox nutxt = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            Text = "",
                                            MaxLength = 5,
                                            ClientIDMode = ClientIDMode.Static,
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                nutxt.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["RCC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_LENGTH"].ToString() != string.Empty)
                                                nutxt.MaxLength = Convert.ToInt32(drFieldControls[k]["RCC_LENGTH"].ToString());
                                        }
                                        div.Controls.Add(nutxt);
                                        //Set the integer and decimal part of the text in the numeric textbox
                                        string integerPart = "10";
                                        string decimalPart = "0";
                                        string regex = "^\\d{1,10}(?:\\.\\d{1,0}){0,1}$";
                                        if (drFieldControls[k]["RCC_FORMAT"] == null || string.IsNullOrEmpty(drFieldControls[k]["RCC_FORMAT"].ToString()))
                                        {
                                            if (drFieldControls[k]["RCC_LENGTH"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_LENGTH"].ToString()))
                                            {
                                                integerPart = drFieldControls[k]["RCC_LENGTH"].ToString();
                                            }
                                            decimalPart = "0";
                                        }
                                        else if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_FORMAT"].ToString()))
                                        {
                                            string[] intDec = drFieldControls[k]["RCC_FORMAT"].ToString().Split(',');//ex: 10,3  means 10 integer num and 3 decimal num
                                            if (intDec.Length == 2)
                                            {
                                                integerPart = intDec[0];
                                                decimalPart = intDec[1];
                                            }
                                            else
                                            {
                                                decimalPart = drFieldControls[k]["RCC_FORMAT"].ToString();
                                                if (drFieldControls[k]["RCC_LENGTH"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_LENGTH"].ToString()))
                                                {
                                                    integerPart = (Convert.ToInt32(drFieldControls[k]["RCC_LENGTH"].ToString()) - (Convert.ToInt32(decimalPart) + 1)).ToString();
                                                }
                                            }
                                        }
                                        if (decimalPart.Equals("0"))//if only the integer number
                                        {
                                            FilteredTextBoxExtender fte = new FilteredTextBoxExtender()
                                            {
                                                ID = "fte" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                TargetControlID = nutxt.ID,
                                                Enabled = true,
                                                FilterType = FilterTypes.Numbers
                                            };
                                            div.Controls.Add(fte);
                                        }
                                        else//if number with decimal part
                                        {
                                            regex = "^\\d{1," + integerPart + "}(?:\\.\\d{1," + decimalPart + "}){0,1}$";
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                RegularExpressionValidator regEx = new RegularExpressionValidator()
                                                {
                                                    ID = "rev" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    CssClass = "star",
                                                    ControlToValidate = nutxt.ID,
                                                    SetFocusOnError = true,
                                                    ErrorMessage = "Invalid " + drFieldControls[k]["RCC_NAME"].ToString(),
                                                    ValidationExpression = regex,
                                                    EnableClientScript = true,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*"
                                                };
                                                div.Controls.Add(regEx);
                                            }
                                        }
                                        //set the number of integerpart value for later use
                                        HiddenField hdnIntegerPart = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "IntegerPart",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = integerPart
                                        };
                                        div.Controls.Add(hdnIntegerPart);
                                        //set the number of decimalpart value for later use
                                        HiddenField hdnDecimalPart = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "DecimalPart",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = decimalPart
                                        };
                                        div.Controls.Add(hdnDecimalPart);

                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrf = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = nutxt.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrf);
                                                }
                                            }
                                        }
                                        //add the compare validator if needed
                                        if (drFieldControls[k]["RCC_CMP_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_CMP_CONTROL_ID"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CompareValidator comValObj = new CompareValidator()
                                                {
                                                    ID = "comVal" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = drFieldControls[k]["RCC_CMP_CONTROL_ID"].ToString(),
                                                    ControlToCompare = nutxt.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ErrorMessage = "Mismatch " + drFieldControls[k]["RCC_NAME"].ToString()
                                                };
                                                div.Controls.Add(comValObj);
                                            }
                                        }
                                        HiddenField hdnNum = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnNum);
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region DropDown
                                    case ControlTypes.DropDown:
                                        #region Assosiated Label
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("DDL", "Lbl"),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("DDL", "Lbl"),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        #endregion
                                        #region Dropdown
                                        DropDownList ddlDrop = new DropDownList()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            TabIndex = tabIndex
                                        };
                                        #endregion
                                        #region Action ( SelectedIndexChange Event)
                                        if (drFieldControls[k]["RCC_ACTION"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_ACTION"].ToString() != string.Empty)
                                            {
                                                ddlDrop.SelectedIndexChanged += new EventHandler(ActionHandler);
                                                ddlDrop.AutoPostBack = true;
                                                HiddenField hdfDDLAction = new HiddenField()
                                                {
                                                    ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Action",
                                                    ClientIDMode = ClientIDMode.Static,
                                                    Value = drFieldControls[k]["RCC_ACTION"].ToString()
                                                };
                                                div.Controls.Add(hdfDDLAction);
                                            }

                                        }
                                        #endregion
                                        #region Css Style
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                ddlDrop.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        #endregion
                                        #region Hidden Fields
                                        HiddenField hdnDrop = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "at",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        HiddenField hdnDropPk = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("DDL", "Txt"),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };

                                        HiddenField hdnDropUICPk = new HiddenField()
                                        {
                                            ID = "hdfUICPk_" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_PK"].ToString()
                                        };
                                        HiddenField hdnDropSelectedValue = new HiddenField()
                                        {
                                            ID = "hdnDropSelectedValue_" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_VALUE"] != null ? drFieldControls[k]["RCC_VALUE"].ToString() : string.Empty
                                        };
                                        #endregion

                                        #region Query Text
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_QUERY_TEXT"].ToString()))
                                        {
                                            //CommonService commonService;
                                            //commonService = null;
                                            string query = drFieldControls[k]["RCC_QUERY_TEXT"].ToString().Trim();
                                            string relId = string.Empty;
                                            string value = string.Empty;
                                            //Replace query with the values if any condition is there
                                            List<string> conditionList = new List<string>();
                                            string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                            if (splitWithAt.Count() > 1)
                                            {
                                                for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                {
                                                    conditionList.Add(splitWithAt[arrayCount].Trim());
                                                }
                                                if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null
                                                    && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))//If it is a child of any other control. ex:country-state
                                                {
                                                    relId = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString();
                                                    string controlType = (from dr in tempDT.AsEnumerable()
                                                                          where dr.Field<string>("RCC_CONTROL_ID") == relId
                                                                          select dr.Field<string>("RCC_CONTROL_TEXT")).First();
                                                    if (controlType == "DropDown")
                                                    {
                                                        // tempDict is added in while relate control is added
                                                        if (tempDict.ContainsKey(relId))
                                                        {
                                                            foreach (KeyValuePair<string, Control> control in tempDict)
                                                            {
                                                                if (control.Key == relId)
                                                                {
                                                                    DropDownList parent = (DropDownList)control.Value;
                                                                    if (parent != null && parent.Items.Count > 1)
                                                                    {
                                                                        if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                        {
                                                                            value = parent.SelectedValue;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                foreach (string condition in conditionList)
                                                {
                                                    if (Session[condition] != null)//parameter name is same as any session name
                                                    {
                                                        if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                            query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                    else if (condition == relId)// parameter is value of any other control
                                                    {
                                                        if (!string.IsNullOrEmpty(value))
                                                            query = query.Replace("@" + condition + "@", value);
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                    else if (condition == "USER_PK")// parameter is value of any other control
                                                    {
                                                        currentUser = GetUserIdentity();
                                                        string UserPK = currentUser.PKUser.ToString();
                                                        if (!string.IsNullOrEmpty(UserPK))
                                                            query = query.Replace("@" + condition + "@", UserPK);
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(query))
                                            {
                                                commonService = new CommonService();
                                                commonService = CommonFunctions.InitiateClient(commonService);
                                                //Execute query
                                                currentUser = GetUserIdentity();
                                                query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);
                                                ddlDrop.DataTextField = "Value";
                                                ddlDrop.DataValueField = "PK";
                                                ddlDrop.DataSource = CommonFunctions.HtmlDecode(ddlValues, "Value");
                                                ddlDrop.DataBind();
                                            }
                                            else
                                            {
                                                ddlDrop.Items.Clear();
                                            }
                                        }
                                        #endregion

                                        if (drFieldControls[k]["RCC_VALUE"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_VALUE"].ToString().Trim()))
                                        {
                                            int index = -2;
                                            if (Int32.TryParse(drFieldControls[k]["RCC_VALUE"].ToString().Trim(), out index))
                                            {
                                                if (index > -2)
                                                {
                                                    ListItem li = ddlDrop.Items.FindByValue(index.ToString());
                                                    if (li != null)
                                                    {
                                                        ddlDrop.SelectedValue = index.ToString();
                                                    }
                                                }
                                            }
                                            else if (drFieldControls[k]["RCC_VALUE"].ToString().Trim().Equals(GetLocalResourceObject("@Currency").ToString()))
                                            {
                                                currentUser = GetUserIdentity();
                                                ListItem li = ddlDrop.Items.FindByValue(currentUser.BaseCurrency.ToString());
                                                if (li != null)
                                                {
                                                    ddlDrop.SelectedValue = currentUser.BaseCurrency.ToString();//Convert.ToInt32(ddlDrop.Items.IndexOf(ddlDrop.Items.FindByText(Resources.Constants.BaseCurrency)));
                                                }
                                            }

                                            //if (drFieldControls[k]["RCC_VALUE"].ToString().Trim().Equals(GetLocalResourceObject("@Store").ToString()))
                                            //{
                                            //    ddlDrop.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                                            //}
                                            //else
                                            //{
                                            if (drFieldControls[k]["RCC_IS_MANDATORY"] != null && drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1")
                                                ddlDrop.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                                            else
                                                ddlDrop.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                                            //}
                                        }
                                        else
                                        {
                                            if (drFieldControls[k]["RCC_IS_MANDATORY"] != null && drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1")
                                                ddlDrop.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                                            else
                                                ddlDrop.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                                        }

                                        if (ddlDrop.Items.Count == 2)
                                        {
                                            ddlDrop.SelectedValue = ddlDrop.Items[1].Value;
                                        }


                                        string tempControlType = (from dr in tempDT.AsEnumerable()
                                                                  where dr.Field<string>("RCC_REL_CONTROL_ID") == ddlDrop.ID
                                                                  select dr.Field<string>("RCC_CONTROL_TEXT")).FirstOrDefault();
                                        //check the dropdown has the related control id of any text or numeric or textarea control. then add "Other" to the dropdown
                                        if (!string.IsNullOrEmpty(tempControlType) && tempControlType == "Text" || tempControlType == "Numeric" || tempControlType == "TextArea")
                                        {
                                            int otherValue = -2;
                                            ddlDrop.Items.Add(new ListItem("Other", otherValue.ToString()));
                                            //Create a hiddenfield to know it has a related control field. It is used when saving values from the dropdown. 
                                            //if it has a related id and it's value is less than zero, then will save null
                                            HiddenField hdfRelId = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "HasRelatedId",
                                                ClientIDMode = ClientIDMode.Static
                                            };
                                            div.Controls.Add(hdfRelId);
                                        }
                                        div.Controls.Add(ddlDrop);
                                        div.Controls.Add(hdnDrop);
                                        div.Controls.Add(hdnDropPk);
                                        div.Controls.Add(hdnDropUICPk);
                                        div.Controls.Add(hdnDropSelectedValue);
                                        tempDict.Add(ddlDrop.ID, ddlDrop);
                                        #region Mandatory
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfddl = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = ddlDrop.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString(),
                                                        InitialValue = CommonConstants.SELECTVAL
                                                    };
                                                    div.Controls.Add(vrfddl);
                                                }
                                            }
                                        }
                                        #endregion

                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        if (drFieldControls[k]["RCC_IS_FULL_LENGTH"] != null && drFieldControls[k]["RCC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                            }
                                            ////For style incriment j.
                                            j++;
                                        }
                                        break;
                                    #endregion
                                    #region Button
                                    case ControlTypes.Button:
                                        if (isButtonGroup)//Checking whether it is a in a buttongroup or not
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "button-fieldsgrp");
                                        }
                                        else
                                        {
                                            //Checking whether it is a grid button or not
                                            if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))
                                            {
                                                bool isFirstGridButton = false;
                                                if (k > 0 && tempDrFieldControls[k - 1]["RCC_CONTROL_TEXT"].ToString().Equals("Button"))//check previous control is button
                                                {
                                                    if (tempDrFieldControls[k - 1]["RCC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(tempDrFieldControls[k - 1]["RCC_REL_CONTROL_ID"].ToString()))//check previous button have related control 
                                                    {
                                                        if (tempDrFieldControls[k - 1]["RCC_REL_CONTROL_ID"].ToString().Equals(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))//check previous button's related control is same as the current button's related control
                                                        {
                                                            isFirstGridButton = false;//its not the first grid button
                                                        }
                                                        else//previous button's related control is not same as the current button's related control
                                                        {
                                                            if (j > 0)//if it is not in the first cell
                                                            {
                                                                isFirstGridButton = false;
                                                            }
                                                            else//if the button is in the first cell
                                                            {
                                                                isFirstGridButton = true;
                                                            }
                                                        }
                                                    }
                                                    else//previous button doesn't have related control 
                                                    {
                                                        if (j > 0)//if it is not in the first cell
                                                        {
                                                            isFirstGridButton = false;
                                                        }
                                                        else//if the button is in the first cell
                                                        {
                                                            isFirstGridButton = true;
                                                        }
                                                    }
                                                }
                                                else//previous control is not a button
                                                {
                                                    if (j > 0)//if it is not in the first cell
                                                    {
                                                        isFirstGridButton = false;
                                                    }
                                                    else//if the button is in the first cell
                                                    {
                                                        isFirstGridButton = true;
                                                    }
                                                }
                                                if (isFirstGridButton)//if it is the first grid button
                                                {
                                                    for (; j < Cols; j++)//if it is not in the first cell, fill the row with dummy cells
                                                    {
                                                        System.Web.UI.WebControls.TableCell tcControl2 = new System.Web.UI.WebControls.TableCell();
                                                        HtmlGenericControl dummydiv = new HtmlGenericControl("div");
                                                        div.Attributes.Add("class", divColStyle);
                                                        tcControl2.Controls.Add(dummydiv);
                                                        trControls.Cells.Add(tcControl2);
                                                    }
                                                    //add new table
                                                    tbControls = new System.Web.UI.WebControls.Table();
                                                    tbControls.CssClass = "";
                                                    //add new row
                                                    trControls = new TableRow();
                                                    //initialize j
                                                    j = 0;
                                                    //add new cell
                                                    tcControl = new System.Web.UI.WebControls.TableCell();
                                                    //add new innerdiv
                                                    div = new HtmlGenericControl("div");
                                                    div.Attributes.Add("class", "button-fieldsgrp");
                                                }
                                                else//if it is not the first grid button
                                                {
                                                    tbControls.CssClass = "";
                                                    div.Attributes.Add("class", "button-fieldsgrp");
                                                }
                                            }
                                            if (tcControl != tempTableCell)//The label wil add only if it as an individual button in the table
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                    Text = "",
                                                    AssociatedControlID = drFieldControls[k]["RCC_CONTROL_ID"].ToString()
                                                });
                                            }
                                        }

                                        if (drFieldControls.Count() == 1)//If the group contains only one control
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "button-fieldsgrp");
                                        }

                                        Button btn = new Button();
                                        btn.ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString();
                                        btn.ClientIDMode = ClientIDMode.Static;
                                        btn.Text = drFieldControls[k]["RCC_NAME"].ToString();
                                        btn.ToolTip = drFieldControls[k]["RCC_NAME"].ToString();
                                        if (drFieldControls[k]["RCC_VALD_GROUP"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_VALD_GROUP"].ToString()))
                                        {
                                            btn.ValidationGroup = drFieldControls[k]["RCC_VALD_GROUP"].ToString();
                                        }
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_ACTION"].ToString()))
                                        {
                                            btn.CommandName = drFieldControls[k]["RCC_ACTION"].ToString();
                                            btn.CausesValidation = true;
                                            btn.Click += new EventHandler(ActionHandler);
                                        }
                                        if (drFieldControls[k]["RCC_SCRIPT"] != null)
                                        {
                                            if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_SCRIPT"].ToString()))
                                            {
                                                btn.OnClientClick = drFieldControls[k]["RCC_SCRIPT"].ToString();
                                            }
                                        }
                                        btn.TabIndex = tabIndex;
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_STYLE"].ToString().Trim()))
                                        {
                                            btn.SkinID = drFieldControls[k]["RCC_STYLE"].ToString().Trim();
                                        }
                                        if (drFieldControls[k]["RCC_TOOLTIP"] != null
                                            && !string.IsNullOrEmpty(drFieldControls[k]["RCC_TOOLTIP"].ToString().Trim()))
                                        {
                                            btn.ToolTip = drFieldControls[k]["RCC_TOOLTIP"].ToString().Trim();
                                        }
                                        //Check the button is workflow related or not. if yes then add event for the current button.
                                        if (drFieldControls[k]["RCC_IS_WORKFLOW"] != null
                                            && !string.IsNullOrEmpty(drFieldControls[k]["RCC_IS_WORKFLOW"].ToString().Trim())
                                            && drFieldControls[k]["RCC_IS_WORKFLOW"].ToString().Equals("1"))
                                        {
                                            btn.CommandArgument = "PageAction_Entry";
                                            btn.PreRender += new EventHandler(btnAction_PreRender);
                                        }

                                        div.Controls.Add(btn);
                                        //For Grid related buttons "RCC_QUERY_TEXT" will be the grid name. and the other buttons it will be the entity name 
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_QUERY_TEXT"].ToString()))
                                        {
                                            HiddenField hdnEntity = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Entity",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["RCC_QUERY_TEXT"].ToString()
                                            };
                                            div.Controls.Add(hdnEntity);
                                        }
                                        //The group number of the button. it is used to find the entity name from the dictionary
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_CONTROL_GROUP"].ToString()))
                                        {
                                            HiddenField hdnGroup = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Group",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["RCC_CONTROL_GROUP"].ToString()
                                            };
                                            div.Controls.Add(hdnGroup);
                                        }
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region LinkButton
                                    case ControlTypes.LinkButton:

                                        if (drFieldControls.Count() == 1)//If the group contains only one control
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "tab-container-grp");
                                            divSubGroup.Attributes.Add("class", "");
                                        }

                                        if (isButtonGroup)//If it is a button group
                                        {
                                            tbControls.CssClass = "";
                                            div.Attributes.Add("class", "tab-container-grp");
                                            divSubGroup.Attributes.Add("class", "");
                                        }

                                        LinkButton lnkbtn = new LinkButton();
                                        lnkbtn.ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString();
                                        lnkbtn.ClientIDMode = ClientIDMode.Static;
                                        lnkbtn.Text = drFieldControls[k]["RCC_NAME"].ToString();
                                        lnkbtn.ToolTip = drFieldControls[k]["RCC_NAME"].ToString();
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_ACTION"].ToString()))
                                        {
                                            lnkbtn.CommandName = drFieldControls[k]["RCC_ACTION"].ToString();
                                            lnkbtn.CausesValidation = true;
                                            lnkbtn.Click += new EventHandler(ActionHandler);
                                        }
                                        if (drFieldControls[k]["RCC_SCRIPT"] != null)
                                        {
                                            if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_SCRIPT"].ToString()))
                                            {
                                                lnkbtn.OnClientClick = drFieldControls[k]["RCC_SCRIPT"].ToString();
                                            }
                                        }
                                        lnkbtn.TabIndex = tabIndex;
                                        div.Controls.Add(lnkbtn);
                                        //For Grid related buttons "RCC_QUERY_TEXT" will be the grid name. and the other buttons it will be the entity name 
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_QUERY_TEXT"].ToString()))
                                        {
                                            HiddenField hdnEntity = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Entity",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["RCC_QUERY_TEXT"].ToString()
                                            };
                                            div.Controls.Add(hdnEntity);
                                        }
                                        //The group number of the button. it is used to find the entity name from the dictionary
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_CONTROL_GROUP"].ToString()))
                                        {
                                            HiddenField hdnGroup = new HiddenField()
                                            {
                                                ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Group",
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["RCC_CONTROL_GROUP"].ToString()
                                            };
                                            div.Controls.Add(hdnGroup);
                                        }
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region Spacer
                                    case ControlTypes.Spacer:
                                        LiteralControl ltc = new LiteralControl()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "&nbsp"
                                        };
                                        div.Controls.Add(ltc);
                                        tabIndex--;
                                        break;
                                    #endregion
                                    #region GridView
                                    case ControlTypes.GridView:
                                        //The grid sholud be in 0th TD
                                        string gridStyle = "gridwrap grid-w930";
                                        //Get the grid style
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_STYLE"].ToString().Trim()))
                                        {
                                            gridStyle = drFieldControls[k]["RCC_STYLE"].ToString().Trim();
                                        }
                                        if (j != 0)//If the current cell is not 0
                                        {
                                            for (; j < Cols; j++)//add dummy column to the row
                                            {
                                                System.Web.UI.WebControls.TableCell tcControl2 = new System.Web.UI.WebControls.TableCell();
                                                HtmlGenericControl dummydiv = new HtmlGenericControl("div");
                                                div.Attributes.Add("class", divColStyle);
                                                tcControl2.Controls.Add(dummydiv);
                                                trControls.Cells.Add(tcControl2);
                                            }
                                            tbControls = new System.Web.UI.WebControls.Table();
                                            tbControls.CssClass = "";
                                            trControls = new TableRow();
                                            j = 0;
                                            tcControl = new System.Web.UI.WebControls.TableCell();
                                            div = new HtmlGenericControl("div");
                                            div.Attributes.Add("class", gridStyle);
                                        }
                                        div.Attributes.Remove("class");
                                        div.Attributes.Add("class", gridStyle);
                                        tbControls.CssClass = "";
                                        GridView grd = new GridView()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            PageSize = int.Parse("10"),
                                            AllowSorting = true,
                                            TabIndex = tabIndex
                                        };
                                        //set rowdatabound action
                                        grd.RowDataBound += new GridViewRowEventHandler(ActionHandler);
                                        div.Controls.Add(grd);
                                        //~Test start
                                        //set paging action
                                        //grd.AllowPaging = true;
                                        //grd.PageIndexChanging += new GridViewPageEventHandler(ActionHandler);
                                        //~Test end
                                        //Add Empty Grid Template
                                        TemplateBuilder tmpEmptyDataTemplate = new TemplateBuilder();
                                        tmpEmptyDataTemplate.AppendLiteralString("No Record Found");
                                        grd.EmptyDataTemplate = tmpEmptyDataTemplate;
                                        grd.EmptyDataRowStyle.ForeColor = Color.Black;
                                        grd.EmptyDataRowStyle.Font.Bold = true;
                                        grd.EmptyDataRowStyle.HorizontalAlign = HorizontalAlign.Center;
                                        grd.EmptyDataRowStyle.BackColor = ColorTranslator.FromHtml("#EFF0F1");
                                        grd.EmptyDataRowStyle.CssClass = "emptytable";
                                        HiddenField hdfGrid = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_PK"].ToString()
                                        };
                                        div.Controls.Add(hdfGrid);
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_QUERY_TEXT"].ToString()))
                                        {
                                            //Get the entity name of the grid
                                            string entityName = drFieldControls[k]["RCC_QUERY_TEXT"].ToString().Trim();
                                            if (!string.IsNullOrEmpty(entityName))
                                            {
                                                if (entityName.Equals("CRM_CUSTOMER_MST"))//if the entity is the master entity
                                                {
                                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = null;//clear the customerpk session
                                                }
                                                if (Session[ERP.Utilities.SessionStrings.CUSTOMERPK] != null)
                                                {
                                                    CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERPK].ToString());//get the master entity pk
                                                }
                                                //create new instance of service. This object should be maintain until all the operation is completed. This is for maintaining the entity object context
                                                customerRegistrationServiceClient = new CustomerRegistrationService();
                                                customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                                                //Get the customer master details
                                                GetFieldValues(ControlsEnum.CUSTOMER);
                                                if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                                                {
                                                    if (hdfGrid != null && !string.IsNullOrEmpty(hdfGrid.Value))
                                                    {
                                                        DataTable dtGrid = null;
                                                        DataRow drGrid;
                                                        System.Data.DataColumn dcGrid;
                                                        int countRow = 0;
                                                        int controlPK = Convert.ToInt32(hdfGrid.Value);
                                                        dtGrid = new DataTable();
                                                        //Get the grid reference table details. this will tell which all data to show in the grid, which shold hide,etc
                                                        admFormTabControlDtlList = customerRegistrationServiceClient.FormTabControlDtl(controlPK);
                                                        if (admFormTabControlDtlList != null && admFormTabControlDtlList.Count > 0)
                                                        {
                                                            //Check whether the grid is a child grid or not
                                                            bool bindGrid = false;
                                                            if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))
                                                            {
                                                                EntityName = entityName;
                                                                ControlPK = controlPK;
                                                                //Get the parent grid id
                                                                RelatedControlID = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString();
                                                                ChildGridName = grd.ID;
                                                                string[] entityArray = entityName.Split('.');
                                                                if (entityArray.Length > 1)
                                                                {
                                                                    if (entityArray[entityArray.Length - 2] != null)
                                                                    {
                                                                        string hdrEntity = entityArray[entityArray.Length - 2];
                                                                        if (Session[hdrEntity] != null)
                                                                        {
                                                                            bindGrid = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                bindGrid = true;
                                                            }
                                                            if (bindGrid)
                                                            {
                                                                //List for store the column numbers which should hide
                                                                List<int> hiddenColumnList = new List<int>();
                                                                //clear the HiddenColumnList session
                                                                Session["HiddenColumnList"] = null;
                                                                IEnumerable entityList = null;
                                                                Object customerObj = null;
                                                                if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                                                                {
                                                                    customerObj = crmCustomerMstList[0];//set the current selected customer master obj
                                                                }
                                                                retEntityObj = null;
                                                                Object retEntity = null;
                                                                if (entityName.Equals("CRM_CUSTOMER_MST"))
                                                                {
                                                                    //set the master entity list for bind the grid
                                                                    retEntity = crmCustomerMstList;
                                                                }
                                                                else
                                                                {
                                                                    if (customerObj != null)
                                                                    {
                                                                        //Get the EntityCollection that shold be bind to the grid
                                                                        retEntity = GetEntityCollection(customerObj, entityName);
                                                                    }
                                                                }
                                                                if (retEntity != null)//If EntityCollection is not null
                                                                {
                                                                    entityList = (IEnumerable)retEntity;
                                                                    foreach (Object enitityObj in entityList)
                                                                    {
                                                                        drGrid = dtGrid.NewRow();
                                                                        int columnCount = 0;
                                                                        foreach (ADM_FORM_TAB_CONTROL_DTL admFormTabControlDtlObj in admFormTabControlDtlList)
                                                                        {
                                                                            retVal = string.Empty;
                                                                            //Get the value 
                                                                            string value = GetPropertyValue(enitityObj, admFormTabControlDtlObj.ACD_CONTROL_ID);
                                                                            if (countRow == 0)//first row
                                                                            {
                                                                                dcGrid = new System.Data.DataColumn();
                                                                                dcGrid.ColumnName = admFormTabControlDtlObj.ACD_CONTROL_ID;//set column name
                                                                                dtGrid.Columns.Add(dcGrid);
                                                                            }
                                                                            //set value to the cell
                                                                            drGrid[admFormTabControlDtlObj.ACD_CONTROL_ID] = value;
                                                                            if (admFormTabControlDtlObj.ACD_IS_HIDDEN == 1)//check the cell is set to hidden or not
                                                                            {
                                                                                hiddenColumnList.Add(columnCount);//add to the hiddenfield list
                                                                            }
                                                                            //iterate column
                                                                            columnCount++;
                                                                        }
                                                                        //iterate row
                                                                        countRow++;
                                                                        //add rows to the data table
                                                                        dtGrid.Rows.Add(drGrid);
                                                                        dtGrid.AcceptChanges();
                                                                        if (hiddenColumnList.Count > 0)
                                                                        {
                                                                            Session["HiddenColumnList"] = hiddenColumnList;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        //Bind grid
                                                        //~Test Start
                                                        //PageIndex = PageIndex == null ? "0" : PageIndex;
                                                        //grd.PageIndex = Convert.ToInt32(PageIndex);
                                                        //~Test End
                                                        grd.DataSource = dtGrid;
                                                        grd.DataBind();
                                                        //For style incriment j. THe grid sholuld be shown in one row
                                                        j++;

                                                    }
                                                    #region pager
                                                    //string usercontrolpath = System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString() == "" ?
                                                    //                        "~/UserControls/PagerControl.ascx" :
                                                    //                        "/" + System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString() +
                                                    //                        "UserControls/PagerControl.ascx";
                                                    //string sql = "";
                                                    //string countsql = "";
                                                    //countsql = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString();
                                                    //if (Session["ParentPK"] != null)
                                                    //{
                                                    //    sql = sql.Replace("Key", Session["ParentPK"].ToString());
                                                    //}
                                                    //else
                                                    //{
                                                    //    sql = sql.Replace("Key", "0");
                                                    //}
                                                    //if (countsql == "")
                                                    //{
                                                    //    countsql = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString();
                                                    //}
                                                    //if (Session["ParentPK"] != null)
                                                    //{
                                                    //    countsql = countsql.Replace("Key", Session["ParentPK"].ToString());
                                                    //}
                                                    //else
                                                    //{
                                                    //    countsql = countsql.Replace("Key", "0");
                                                    //}
                                                    //div.Controls.Add(grd);
                                                    #endregion
                                                }
                                            }
                                        }
                                        break;
                                    #endregion
                                    #region System.Web.UI.WebControls.CheckBox
                                    case ControlTypes.CheckBox:
                                        System.Web.UI.WebControls.CheckBox chk = new System.Web.UI.WebControls.CheckBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                            TabIndex = tabIndex,
                                            TextAlign = TextAlign.Left

                                        };
                                        if (drFieldControls[k]["RCC_VALUE"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_VALUE"].ToString()))
                                        {
                                            if (drFieldControls[k]["RCC_VALUE"].ToString().Equals("1"))
                                                chk.Checked = true;
                                            else
                                                chk.Checked = false;
                                        }
                                        div.Controls.Add(chk);

                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region Iframe
                                    case ControlTypes.Iframe:
                                        HtmlGenericControl iframe = new HtmlGenericControl("iframe");
                                        iframe.ID = "frmReview";
                                        iframe.Attributes.Add("frameborder", "0");
                                        iframe.Attributes.Add("scrolling", "no");
                                        iframe.Attributes.Add("width", "100%");
                                        iframe.Attributes.Add("style", "border: none;min-height: 300px;");
                                        div.Controls.Add(iframe);
                                        tabIndex--;
                                        break;
                                    #endregion
                                    #region HiddenField
                                    case ControlTypes.HiddenField:
                                        HiddenField hdfPK = new HiddenField()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        if (drFieldControls[k]["RCC_VALUE"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_VALUE"].ToString()))
                                        {
                                            hdfPK.Value = drFieldControls[k]["RCC_VALUE"].ToString();
                                        }

                                        //Hidden field will add directly to the div sub group.
                                        divSubGroup.Controls.Add(hdfPK);
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        divSubGroup.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region FileUpload
                                    case ControlTypes.FileUpload:
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }

                                        FileUpload fup = new FileUpload()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            TabIndex = tabIndex
                                        };
                                        //Set PostBackTrigger for the buttons that related to the current fileupload
                                        //if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))
                                        //{
                                        //    //May be more than one button that related to the fileupload
                                        //    string[] triggerControlsArray = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString().Split(',');
                                        //    foreach (string controlId in triggerControlsArray)
                                        //    {
                                        //        //Create new postbacktrigger
                                        //        PostBackTrigger po = new PostBackTrigger();
                                        //        po.ControlID = controlId;
                                        //        bool hasTrigger = false;
                                        //        //check for duplication of postbacktrigger.if not then add it to the Trigger section
                                        //        foreach (PostBackTrigger trigger in aupdpnlCommonReportViewer.Triggers)
                                        //        {
                                        //            if (trigger.ControlID.Equals(po.ControlID))
                                        //            {
                                        //                hasTrigger = true;
                                        //                break;
                                        //            }
                                        //        }
                                        //        //Add postbacktrigger
                                        //        if (!hasTrigger)
                                        //            aupdpnlCommonReportViewer.Triggers.Add(po);
                                        //    }
                                        //}
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                fup.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        div.Controls.Add(fup);
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfTxtArea = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = fup.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrfTxtArea);
                                                }
                                            }
                                        }
                                        HiddenField hdnFileUpload = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        div.Controls.Add(hdnFileUpload);

                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        break;
                                    #endregion
                                    #region Tree
                                    case ControlTypes.TreeView:
                                        tbControls.CssClass = "tablelayout";
                                        tcControl.Style.Add("width", "50%");
                                        div.Attributes.Add("class", "tree-label2M");

                                        HtmlGenericControl DivInner = new HtmlGenericControl("div");
                                        div.Controls.Add(new Label()
                                        {
                                            ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("DDL", "Lbl"),
                                            AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("TRV", "Lbl"),
                                            ClientIDMode = ClientIDMode.Static
                                        });
                                        TreeView TrvTree = new TreeView()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            TabIndex = tabIndex,
                                            ShowLines = true,
                                            ExpandDepth = 0
                                        };
                                        TrvTree.Style.Add("display", "inline-block");
                                        //if (drFieldControls[k]["RCC_STYLE"] != null)
                                        //{
                                        //    if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                        //        TrvTree.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        //}

                                        if (drFieldControls[k]["RCC_ACTION"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_ACTION"].ToString() != string.Empty)
                                            {
                                                //TrvTree.SelectedIndexChanged += new EventHandler(ActionHandler);
                                                TrvTree.Attributes.Add("onclick", "postBackByTreeviewCheckBox(event)");
                                                TrvTree.TreeNodeCheckChanged += new TreeNodeEventHandler(ActionHandler);
                                                HiddenField hdfDDLAction = new HiddenField()
                                                {
                                                    ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Action",
                                                    ClientIDMode = ClientIDMode.Static,
                                                    Value = drFieldControls[k]["RCC_ACTION"].ToString()
                                                };
                                                div.Controls.Add(hdfDDLAction);
                                            }
                                        }

                                        HiddenField hdnTrv = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "at",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        HiddenField hdnTrvPk = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("DDL", "Txt"),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };

                                        HiddenField hdnTrvUICPk = new HiddenField()
                                        {
                                            ID = "hdfUICPk_" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_PK"].ToString()
                                        };

                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_QUERY_TEXT"].ToString()))
                                        {
                                            //CommonService commonService;
                                            //commonService = null;
                                            string query = drFieldControls[k]["RCC_QUERY_TEXT"].ToString().Trim();
                                            string relId = string.Empty;
                                            string value = string.Empty;
                                            //Replace query with the values if any condition is there
                                            List<string> conditionList = new List<string>();
                                            string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                            if (splitWithAt.Count() > 1)
                                            {
                                                for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                {
                                                    conditionList.Add(splitWithAt[arrayCount].Trim());
                                                }
                                                if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null
                                                    && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))//If it is a child of any other control. ex:country-state
                                                {
                                                    relId = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString();
                                                    string controlType = (from dr in tempDT.AsEnumerable()
                                                                          where dr.Field<string>("RCC_CONTROL_ID") == relId
                                                                          select dr.Field<string>("RCC_CONTROL_TEXT")).First();
                                                    if (controlType == "DropDown")
                                                    {
                                                        if (tempDict.ContainsKey(relId))
                                                        {
                                                            foreach (KeyValuePair<string, Control> control in tempDict)
                                                            {
                                                                if (IsDropdownList(control.Value))
                                                                {
                                                                    DropDownList parent = (DropDownList)control.Value;
                                                                    if (parent != null && parent.Items.Count > 1)
                                                                    {
                                                                        if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                        {
                                                                            value = parent.SelectedValue;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                foreach (string condition in conditionList)
                                                {
                                                    if (Session[condition] != null)//parameter name is same as any session name
                                                    {
                                                        if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                            query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                    else if (condition == relId)// parameter is value of any other control
                                                    {
                                                        if (!string.IsNullOrEmpty(value))
                                                            query = query.Replace("@" + condition + "@", value);
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            TreeNode child;
                                            TreeNode root;
                                            TrvTree.Nodes.Clear();
                                            if (drFieldControls[k]["RCC_NAME"].ToString() != "")
                                            {
                                                if (drFieldControls[k]["RCC_IS_MANDATORY"] != null && drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1")
                                                    // root = new TreeNode((GetLocalResourceObject("RootNodeName") + " " + drFieldControls[k]["RCC_NAME"].ToString()), "0"); -- BugID : 3589
                                                    root = new TreeNode((drFieldControls[k]["RCC_NAME"].ToString()), "0");
                                                else
                                                    root = new TreeNode(drFieldControls[k]["RCC_NAME"].ToString(), "0");
                                            }
                                            else
                                            {
                                                root = new TreeNode(GetLocalResourceObject("RootNodeName").ToString(), "0");
                                            }

                                            TrvTree.Nodes.Add(root);
                                            root.Collapse();

                                            if (!string.IsNullOrEmpty(query))
                                            {
                                                commonService = new CommonService();
                                                commonService = CommonFunctions.InitiateClient(commonService);
                                                //Execute query
                                                currentUser = GetUserIdentity();
                                                query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                List<DDLMaster> TrvValues = commonService.ExecuteQuery(query);
                                                if (TrvValues != null && TrvValues.Count > 0)
                                                {
                                                    root.ChildNodes.Clear();
                                                    foreach (DDLMaster item in TrvValues)
                                                    {
                                                        child = new TreeNode();
                                                        child.ShowCheckBox = true;
                                                        child.Text = item.Value;
                                                        child.ToolTip = item.Value;
                                                        child.Value = item.PK.ToString();
                                                        root.ChildNodes.Add(child);
                                                    }
                                                }
                                                else
                                                {
                                                    root.ChildNodes.Clear();
                                                }
                                            }
                                            else
                                            {
                                                root.ChildNodes.Clear();
                                            }
                                        }
                                        DivInner.Attributes.Add("Class", "treeview");
                                        DivInner.Controls.Add(TrvTree);
                                        div.Controls.Add(DivInner);
                                        div.Controls.Add(hdnTrv);
                                        div.Controls.Add(hdnTrvPk);
                                        div.Controls.Add(hdnTrvUICPk);
                                        //if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        //{
                                        //    if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                        //    {
                                        //        for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                        //        {
                                        //            RequiredFieldValidator vrfddl = new RequiredFieldValidator()
                                        //            {
                                        //                ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                        //                CssClass = "star",
                                        //                SetFocusOnError = true,
                                        //                ValidationGroup = valdationGroupArray[valCount],
                                        //                EnableClientScript = true,
                                        //                ControlToValidate = ddlDrop.ID,
                                        //                Display = ValidatorDisplay.Dynamic,
                                        //                Text = "*",
                                        //                ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString(),
                                        //                InitialValue = CommonConstants.SELECTVAL
                                        //            };
                                        //            div.Controls.Add(vrfddl);
                                        //        }
                                        //    }
                                        //}

                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        if (drFieldControls[k]["RCC_IS_FULL_LENGTH"] != null && drFieldControls[k]["RCC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                            }
                                            ////For style incriment j.
                                            j++;
                                        }
                                        break;
                                    #endregion
                                    #region Multi Level Tree
                                    case ControlTypes.MultiLevelTree:
                                        tbControls.CssClass = "tablelayout";
                                        tcControl.Style.Add("width", "50%");
                                        div.Attributes.Add("class", "tree-label2M");

                                        HtmlGenericControl divInnerMultiLevelTreeView = new HtmlGenericControl("div");
                                        #region Add Assosiated Label
                                        div.Controls.Add(new Label
                                        {
                                            ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("DDL", "Lbl"),
                                            AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("TRV", "Lbl"),
                                            ClientIDMode = ClientIDMode.Static
                                        });
                                        #endregion

                                        #region Control
                                        TreeView MultiTrvTree = new TreeView
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            TabIndex = tabIndex,
                                            ShowLines = true,
                                            ExpandDepth = 0,
                                        };
                                        MultiTrvTree.Style.Add("display", "inline-block");
                                        MultiTrvTree.Attributes.Add("onclick", "OnCheckBoxCheckChanged(event);");
                                        #endregion

                                        #region Attach Selected Index Change Event
                                        if (drFieldControls[k]["RCC_ACTION"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_ACTION"].ToString() != string.Empty)
                                            {
                                                //TrvTree.SelectedIndexChanged += new EventHandler(ActionHandler);
                                                MultiTrvTree.Attributes.Add("onclick", "postBackByTreeviewCheckBox(event)");
                                                MultiTrvTree.TreeNodeCheckChanged += new TreeNodeEventHandler(ActionHandler);
                                                HiddenField hdfDDLAction = new HiddenField()
                                                {
                                                    ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Action",
                                                    ClientIDMode = ClientIDMode.Static,
                                                    Value = drFieldControls[k]["RCC_ACTION"].ToString()
                                                };
                                                div.Controls.Add(hdfDDLAction);
                                            }
                                        }
                                        #endregion

                                        #region Hidden Fields
                                        HiddenField hdnMulTrv = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "at",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };
                                        HiddenField hdnMulTrvPk = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("DDL", "Txt"),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = ""
                                        };

                                        HiddenField hdnMulTrvUICPk = new HiddenField()
                                        {
                                            ID = "hdfUICPk_" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_PK"].ToString()
                                        };
                                        #endregion

                                        #region Data Binding
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_QUERY_TEXT"].ToString()))
                                        {
                                            //CommonService commonService;
                                            //commonService = null;
                                            string query = drFieldControls[k]["RCC_QUERY_TEXT"].ToString().Trim();
                                            string relId = string.Empty;
                                            string value = string.Empty;
                                            //Replace query with the values if any condition is there
                                            List<string> conditionList = new List<string>();
                                            string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                            if (splitWithAt.Count() > 1)
                                            {
                                                for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                {
                                                    conditionList.Add(splitWithAt[arrayCount].Trim());
                                                }
                                                if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null
                                                    && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))//If it is a child of any other control. ex:country-state
                                                {
                                                    relId = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString();
                                                    string controlType = (from dr in tempDT.AsEnumerable()
                                                                          where dr.Field<string>("RCC_CONTROL_ID") == relId
                                                                          select dr.Field<string>("RCC_CONTROL_TEXT")).First();
                                                    if (controlType == "DropDown")
                                                    {
                                                        if (tempDict.ContainsKey(relId))
                                                        {
                                                            foreach (KeyValuePair<string, Control> control in tempDict)
                                                            {
                                                                DropDownList parent = control.Value as DropDownList;
                                                                if (parent != null && parent.Items.Count > 1)
                                                                {
                                                                    if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                    {
                                                                        value = parent.SelectedValue;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                foreach (string condition in conditionList)
                                                {
                                                    if (Session[condition] != null)//parameter name is same as any session name
                                                    {
                                                        if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                            query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                    else if (condition == relId)// parameter is value of any other control
                                                    {
                                                        if (!string.IsNullOrEmpty(value))
                                                            query = query.Replace("@" + condition + "@", value);
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            //TreeNode child;
                                            TreeNode root;
                                            MultiTrvTree.Nodes.Clear();
                                            if (drFieldControls[k]["RCC_NAME"].ToString() != "")
                                            {
                                                if (drFieldControls[k]["RCC_IS_MANDATORY"] != null && drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1")
                                                    // root = new TreeNode((GetLocalResourceObject("RootNodeName") + " " + drFieldControls[k]["RCC_NAME"].ToString()), "0"); -- BugID : 3589
                                                    root = new TreeNode((drFieldControls[k]["RCC_NAME"].ToString()), "0");
                                                else
                                                    root = new TreeNode(drFieldControls[k]["RCC_NAME"].ToString(), "0");
                                            }
                                            else
                                            {
                                                root = new TreeNode(GetLocalResourceObject("RootNodeName").ToString(), "0");
                                            }

                                            root.ShowCheckBox = true;
                                            MultiTrvTree.Nodes.Add(root);
                                            root.Collapse();

                                            if (!string.IsNullOrEmpty(query))
                                            {
                                                commonService = new CommonService();
                                                commonService = CommonFunctions.InitiateClient(commonService);
                                                //Execute query
                                                List<TreeBinder> TrvValues = commonService.ExecuteTreeQuery(query);
                                                if (TrvValues != null && TrvValues.Count > 0)
                                                {
                                                    root.ChildNodes.Clear();
                                                    CreateTreeViewFromList(TrvValues, 0, root);
                                                }
                                                else
                                                {
                                                    root.ChildNodes.Clear();
                                                }
                                            }
                                            else
                                            {
                                                root.ChildNodes.Clear();
                                            }
                                        }
                                        #endregion

                                        #region Controls Added To the Container
                                        divInnerMultiLevelTreeView.Attributes.Add("Class", "treeview");
                                        divInnerMultiLevelTreeView.Controls.Add(MultiTrvTree);
                                        div.Controls.Add(divInnerMultiLevelTreeView);
                                        div.Controls.Add(hdnMulTrv);
                                        div.Controls.Add(hdnMulTrvPk);
                                        div.Controls.Add(hdnMulTrvUICPk);
                                        #endregion

                                        #region Control Type Hidden Field
                                        hdnType = new HiddenField
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        #endregion
                                        break;
                                    #endregion
                                    #region MonthPicker
                                    case ControlTypes.MonthPicker:
                                        if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                        {
                                            div.Controls.Add(new Label()
                                            {
                                                ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                ClientIDMode = ClientIDMode.Static
                                            });
                                        }
                                        System.Web.UI.WebControls.TextBox monthpicker = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = DateTime.Now.ToString("MMM-yyyy"),
                                            TabIndex = tabIndex
                                        };
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                monthpicker.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        tabIndex++;
                                        monthpicker.Attributes.Add("onkeydown", "return false");
                                        monthpicker.Attributes.Add("onpaste", "return false");
                                        if (drFieldControls[k]["RCC_VALUE"] != null && Convert.ToString(drFieldControls[k]["RCC_VALUE"]) == "-1")
                                            monthpicker.Text = DateTime.Now.AddMonths(-1).ToString("MMM-yyyy");
                                        else
                                            monthpicker.Text = DateTime.Now.ToString("MMM-yyyy");

                                        div.Controls.Add(monthpicker);
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrfDate = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = monthpicker.ID,
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString() + " Date"
                                                    };
                                                    div.Controls.Add(vrfDate);
                                                }
                                            }
                                        }

                                        CalendarExtender calExtender = new CalendarExtender();

                                        calExtender.Format = "MMM-yyyy";
                                        calExtender.ID = "mpk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString();
                                        calExtender.BehaviorID = "mpkbi" + drFieldControls[k]["RCC_CONTROL_ID"].ToString();
                                        calExtender.TargetControlID = monthpicker.ID;
                                        calExtender.OnClientShown = "onCalendarShown";
                                        calExtender.OnClientHidden = "onCalendarHidden";

                                        div.Controls.Add(calExtender);
                                        HiddenField hdnMonthP = new HiddenField()
                                        {
                                            ID = "hdfPk" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = DateTime.Now.ToString("MMM-yyyy")
                                        };
                                        div.Controls.Add(hdnMonthP);
                                        HiddenField hdnMonthPicker = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = DateTime.Now.ToString("MMM-yyyy")
                                        };
                                        div.Controls.Add(hdnMonthPicker);
                                        //set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + drFieldControls[k]["RCC_CONTROL_ID"].ToString() + "Type",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        div.Controls.Add(hdnType);
                                        //PageScriptOuterInit = PageScriptOuterInit + CommonFunctions.GenerateDynamicScript(drFieldControls[k]["RCC_CONTROL_TEXT"].ToString(), calExtender.BehaviorID, null, null, null);
                                        MonthPickerControlScript += (MonthPickerControlScript == string.Empty ? string.Empty : "|") + calExtender.BehaviorID;

                                        if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))
                                        {
                                            HiddenField hdfFrmMonth = new HiddenField()
                                            {
                                                ID = "hdfRange" + monthpicker.ID,
                                                ClientIDMode = ClientIDMode.Static,
                                                Value = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()
                                            };
                                            div.Controls.Add(hdfFrmMonth);
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CustomValidator cusVal = new CustomValidator()
                                                {
                                                    ID = "cus" + valCount + monthpicker.ID,
                                                    CssClass = "star",
                                                    EnableClientScript = true,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ControlToValidate = monthpicker.ID,
                                                    ClientValidationFunction = "CheckMonthRange",
                                                    ErrorMessage = GetLocalResourceObject("Msg_Err_Month").ToString(),
                                                    ValidationGroup = valdationGroupArray[valCount]
                                                };
                                                div.Controls.Add(cusVal);
                                            }


                                        }
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "FormatControl", CommonFunctions.GenerateDynamicScript(ControlTypes.MonthPicker.ToString(), MonthPickerControlScript, null, null, null), true);
                                        break;
                                    #endregion
                                    #region Auto Complete
                                    case ControlTypes.AutoComplete:
                                        // Auto Complete ControlID like txtCtrlID|hdnCtrlID ,
                                        // So we split it using | . indx 0 is txtBox id nd indx 1 is hdn id
                                        // hdn id is using as the related Contrl id of others.

                                        string[] autoCompleteIDArray = drFieldControls[k]["RCC_CONTROL_ID"].ToString().Split('|');

                                        #region Set Assosiated Label
                                        if (tcControl != tempTableCell)//add Label only if it is not related to any other control
                                        {
                                            if (drFieldControls[k]["RCC_CONTROL_TEXT"].ToString() != "")
                                            {
                                                div.Controls.Add(new Label()
                                                {
                                                    ID = string.Format("lbl{0}", autoCompleteIDArray[0]),
                                                    Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                                    AssociatedControlID = string.Format("txt{0}", autoCompleteIDArray[0])
                                                });
                                            }
                                        }
                                        #endregion
                                        #region Creating Text Box For Auto Complete
                                        System.Web.UI.WebControls.TextBox txtAuto = new System.Web.UI.WebControls.TextBox()
                                        {
                                            ID = string.Format("txt{0}", autoCompleteIDArray[0]), // if any changes here , updt assosiated ctrl id of the above lbl
                                            ClientIDMode = ClientIDMode.Static,
                                            Text = "",
                                            TabIndex = tabIndex
                                        };
                                        #endregion
                                        #region Set Style & Length
                                        if (drFieldControls[k]["RCC_STYLE"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_STYLE"].ToString() != string.Empty)
                                                txtAuto.CssClass = drFieldControls[k]["RCC_STYLE"].ToString();
                                        }
                                        if (drFieldControls[k]["RCC_LENGTH"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_LENGTH"].ToString() != string.Empty)
                                                txtAuto.MaxLength = Convert.ToInt32(drFieldControls[k]["RCC_LENGTH"].ToString());
                                        }
                                        #endregion
                                        #region Value Hideen Field
                                        //Auto complete value field
                                        HiddenField hdnTextAutoVal = new HiddenField()
                                        {
                                            //ID = "hdfAuto" + drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                            ID = string.Format("hdfAuto{0}", autoCompleteIDArray[1]),
                                            //ID="hdnAutoTextVal",
                                            ClientIDMode = ClientIDMode.Static
                                        };
                                        #endregion

                                        div.Controls.Add(hdnTextAutoVal);
                                        div.Controls.Add(txtAuto);

                                        #region Adding Requied Field
                                        if (drFieldControls[k]["RCC_IS_MANDATORY"] != null)
                                        {
                                            if ((drFieldControls[k]["RCC_IS_MANDATORY"].ToString() == "1"))
                                            {
                                                for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                                {
                                                    RequiredFieldValidator vrftxt = new RequiredFieldValidator()
                                                    {
                                                        ID = "vrf" + valCount + autoCompleteIDArray[0],
                                                        CssClass = "star",
                                                        SetFocusOnError = true,
                                                        ValidationGroup = valdationGroupArray[valCount],
                                                        EnableClientScript = true,
                                                        ControlToValidate = txtAuto.ID,
                                                        InitialValue = "Select/Type",
                                                        Display = ValidatorDisplay.Dynamic,
                                                        Text = "*",
                                                        ErrorMessage = "Enter " + drFieldControls[k]["RCC_NAME"].ToString()
                                                    };
                                                    div.Controls.Add(vrftxt);
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Control Pk
                                        HiddenField hdnTextAuto = new HiddenField()
                                        {
                                            ID = "hdfPk" + autoCompleteIDArray[0],
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_PK"].ToString()
                                        };
                                        #endregion

                                        div.Controls.Add(hdnTextAuto);

                                        #region add the compare validator if needed
                                        if (drFieldControls[k]["RCC_CMP_CONTROL_ID"] != null && !string.IsNullOrEmpty(drFieldControls[k]["RCC_CMP_CONTROL_ID"].ToString()))
                                        {
                                            for (int valCount = 0; valCount < valdationGroupArray.Length; valCount++)
                                            {
                                                CompareValidator comValObj = new CompareValidator()
                                                {
                                                    ID = "comVal" + valCount + autoCompleteIDArray[0],
                                                    CssClass = "star",
                                                    SetFocusOnError = true,
                                                    ValidationGroup = valdationGroupArray[valCount],
                                                    EnableClientScript = true,
                                                    ControlToValidate = drFieldControls[k]["RCC_CMP_CONTROL_ID"].ToString(),
                                                    ControlToCompare = txtAuto.ID,
                                                    Display = ValidatorDisplay.Dynamic,
                                                    Text = "*",
                                                    ErrorMessage = "Mismatch " + drFieldControls[k]["RCC_NAME"].ToString()
                                                };
                                                div.Controls.Add(comValObj);
                                            }
                                        }
                                        #endregion
                                        #region set the type of the control
                                        hdnType = new HiddenField()
                                        {
                                            ID = "hdf" + autoCompleteIDArray[0] + "Type",
                                            //ID = "hdfType",
                                            ClientIDMode = ClientIDMode.Static,
                                            Value = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()
                                        };
                                        #endregion

                                        div.Controls.Add(hdnType);
                                        if (drFieldControls[k]["RCC_IS_FULL_LENGTH"] != null && drFieldControls[k]["RCC_IS_FULL_LENGTH"].ToString() == "1")
                                        {
                                            if (Cols > 1)
                                            {
                                                div.Attributes.Add("class", GetLocalResourceObject("div1colstyle").ToString());
                                            }
                                            ////For style incriment j.
                                            j++;
                                        }

                                        #region Query Text
                                        string relAutoId = string.Empty;
                                        string autoValue = string.Empty;
                                        if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null
                                            && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))//If it is a child of any other control. ex:country-state
                                        {
                                            relAutoId = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString();
                                            string controlType = (from dr in tempDT.AsEnumerable()
                                                                  where dr.Field<string>("RCC_CONTROL_ID") == relAutoId
                                                                  select dr.Field<string>("RCC_CONTROL_TEXT")).First();
                                            #region Dropdown
                                            if (controlType == "DropDown")
                                            {
                                                // tempDict is added in while relate control is added
                                                if (tempDict.ContainsKey(relAutoId))
                                                {
                                                    foreach (KeyValuePair<string, Control> control in tempDict)
                                                    {
                                                        if (control.Key == relAutoId)
                                                        {
                                                            DropDownList parent = (DropDownList)control.Value;
                                                            if (parent != null && parent.Items.Count > 1)
                                                            {
                                                                if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                {
                                                                    autoValue = parent.SelectedValue;
                                                                    break;
                                                                }
                                                                else
                                                                {
                                                                    HiddenField hdnSelectedParentID = (HiddenField)pnlControls.FindControl("hdnDropSelectedValue_" + parent.ID);
                                                                    if (hdnSelectedParentID != null)
                                                                    {
                                                                        autoValue = parent.SelectedValue;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                            #region AutoComplete
                                            if (controlType == "AutoComplete")
                                            {
                                                // tempDict is added in while relate control is added
                                                if (tempDict.ContainsKey(relAutoId))
                                                {
                                                    foreach (KeyValuePair<string, Control> control in tempDict)
                                                    {
                                                        if (control.Key == relAutoId)
                                                        {
                                                            HiddenField parent = (HiddenField)control.Value;
                                                            if (parent != null)
                                                            {
                                                                autoValue = parent.Value;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            #endregion
                                        }

                                        #endregion

                                        #region After Selection Change
                                        if (drFieldControls[k]["RCC_ACTION"] != null)
                                        {
                                            if (drFieldControls[k]["RCC_ACTION"].ToString() != string.Empty)
                                            {
                                                // Dummy Button For getting Postback after select an item from auto complete
                                                ImageButton dummyBtn = new ImageButton();
                                                dummyBtn.ID = string.Format("btn{0}", autoCompleteIDArray[0]);
                                                dummyBtn.CommandName = drFieldControls[k]["RCC_ACTION"].ToString();
                                                //dummyBtn.Style.Add("display", "none");
                                                //juno
                                                dummyBtn.CommandArgument = hdnTextAutoVal.ID + "," + autoCompleteIDArray[0] + "|" + autoCompleteIDArray[1];
                                                dummyBtn.Attributes.Add("style", "display:none");
                                                dummyBtn.Click += ActionHandler;
                                                dummyBtn.ToolTip = autoCompleteIDArray[1];

                                                div.Controls.Add(dummyBtn);

                                                HiddenField hdfAutoAction = new HiddenField()
                                                {
                                                    ID = "hdfAction" + autoCompleteIDArray[0],
                                                    ClientIDMode = ClientIDMode.Static,
                                                    Value = autoCompleteIDArray[0] //drFieldControls[k]["RCC_ACTION"].ToString()
                                                };
                                                div.Controls.Add(hdfAutoAction);
                                            }
                                        }
                                        #endregion

                                        tempDict.Add(autoCompleteIDArray[0], hdnTextAutoVal);

                                        string controlName = Request.Params["__EVENTTARGET"];
                                        if (controlName != null)
                                            if (!controlName.EndsWith(relAutoId) || string.IsNullOrWhiteSpace(controlName) || string.IsNullOrWhiteSpace(relAutoId))
                                            {
                                                if (string.IsNullOrWhiteSpace(autoValue))
                                                {
                                                    if (AutoCompleteRelatedControlValue != null && AutoCompleteRelatedControlValue.ContainsKey(autoCompleteIDArray[0]))
                                                        autoValue = AutoCompleteRelatedControlValue[autoCompleteIDArray[0]];
                                                }

                                                string queryID = drFieldControls[k]["RCC_QUERY"].ToString();
                                                string qryStringParams = string.Format("{0}={1}&{2}={3}&{4}={5}"
                                                                                , RequestParameters.Query
                                                                                , queryID
                                                                                , RequestParameters.RelCtrl
                                                                                , relAutoId
                                                                                , RequestParameters.RelCtrlValue
                                                                                , autoValue
                                                                            );


                                                PageScript += "GrandScriptUtils.MakeAutoCompleteDDL('txt" + autoCompleteIDArray[0] + "', BuildAutoCompleteUrl('" + qryStringParams + "'), 'hdfAuto" + autoCompleteIDArray[1] + "', true, true, 'EXECUTEQUERY');";
                                            }
                                        break;
                                    #endregion
                                    #region Auto Check List
                                    case ControlTypes.AutoCheckList:
                                        //tbControls.CssClass = "tablelayout";
                                        //tcControl.Style.Add("width", "50%");
                                        div.Attributes.Add("class", "tree-label2M w600");

                                        HtmlGenericControl divInnerChkSearch = new HtmlGenericControl("div");
                                        #region Add Assosiated Label
                                        div.Controls.Add(new Label
                                        {
                                            ID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("DDL", "Lbl"),
                                            Text = drFieldControls[k]["RCC_NAME"].ToString(),
                                            AssociatedControlID = "lbl" + drFieldControls[k]["RCC_CONTROL_ID"].ToString().Replace("CHRV", "Lbl"),
                                            ClientIDMode = ClientIDMode.Static,
                                            CssClass = "lbl-30perc float-left"

                                        });
                                        #endregion
                                        #region Control
                                        CheckListSearchControl chkListSearch = LoadControl("~/UserControls/CheckListSearchControl.ascx") as CheckListSearchControl;

                                        // CheckListSearchControl MultiSearchTree = new CheckListSearchControl();
                                        chkListSearch.ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString();
                                        chkListSearch.ClientIDMode = ClientIDMode.Static;
                                        //{
                                        //    ID = drFieldControls[k]["RCC_CONTROL_ID"].ToString(),
                                        //};

                                        #region Query Text
                                        if (!string.IsNullOrEmpty(drFieldControls[k]["RCC_QUERY_TEXT"].ToString()))
                                        {
                                            //CommonService commonService;
                                            //commonService = null;
                                            string query = drFieldControls[k]["RCC_QUERY_TEXT"].ToString().Trim();
                                            string relId = string.Empty;
                                            string value = string.Empty;
                                            //Replace query with the values if any condition is there
                                            List<string> conditionList = new List<string>();
                                            string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                            if (splitWithAt.Count() > 1)
                                            {
                                                for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                {
                                                    conditionList.Add(splitWithAt[arrayCount].Trim());
                                                }
                                                if (drFieldControls[k]["RCC_REL_CONTROL_ID"] != null
                                                    && !string.IsNullOrEmpty(drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString()))//If it is a child of any other control. ex:country-state
                                                {
                                                    relId = drFieldControls[k]["RCC_REL_CONTROL_ID"].ToString();
                                                    string controlType = (from dr in tempDT.AsEnumerable()
                                                                          where dr.Field<string>("RCC_CONTROL_ID") == relId
                                                                          select dr.Field<string>("RCC_CONTROL_TEXT")).First();
                                                    if (controlType == "DropDown")
                                                    {
                                                        // tempDict is added in while relate control is added
                                                        if (tempDict.ContainsKey(relId))
                                                        {
                                                            foreach (KeyValuePair<string, Control> control in tempDict)
                                                            {
                                                                if (control.Key == relId)
                                                                {
                                                                    DropDownList parent = (DropDownList)control.Value;
                                                                    if (parent != null && parent.Items.Count > 1)
                                                                    {
                                                                        if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                        {
                                                                            value = parent.SelectedValue;
                                                                            break;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                foreach (string condition in conditionList)
                                                {
                                                    if (Session[condition] != null)//parameter name is same as any session name
                                                    {
                                                        if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                            query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                    else if (condition == relId)// parameter is value of any other control
                                                    {
                                                        if (!string.IsNullOrEmpty(value))
                                                            query = query.Replace("@" + condition + "@", value);
                                                        else
                                                        {
                                                            query = string.Empty;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            if (!string.IsNullOrEmpty(query))
                                            {
                                                commonService = new CommonService();
                                                commonService = CommonFunctions.InitiateClient(commonService);
                                                //Execute query
                                                currentUser = GetUserIdentity();
                                                query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                List<DDLMaster> chkValues = commonService.ExecuteQuery(query);
                                                chkListSearch.ListData = CommonFunctions.HtmlDecode(chkValues, "Value");
                                                chkListSearch.BindData();
                                            }
                                            else
                                            {
                                                chkListSearch.ClearData();
                                            }
                                        }
                                        #endregion
                                        #region Controls Added To the Container
                                        // divInnerMultiLevelSearchTreeView.Attributes.Add("Class", "treeview");
                                        divInnerChkSearch.Controls.Add(chkListSearch);
                                        div.Controls.Add(divInnerChkSearch);
                                        //div.Controls.Add(hdnMulTrv);
                                        //div.Controls.Add(hdnMulTrvPk);
                                        //div.Controls.Add(hdnMulTrvUICPk);
                                        #endregion
                                        #endregion

                                        //CheckListSearchControl 
                                        break;
                                        #endregion
                                        #endregion
                                }
                                if ((drFieldControls[k]["RCC_CONTROL_TEXT"].ToString()).Equals("HiddenField"))//If the control is a hidden field it is directly add to the divSubGroup. It doesn't need to add any cell
                                {
                                    //remove the current control from the control datatable
                                    dtFieldControls.Rows.RemoveAt(0);
                                    dtFieldControls.AcceptChanges();
                                    k++;
                                }
                                else
                                {
                                    //add inner div to the table cell
                                    tcControl.Controls.Add(div);
                                    //update temptable cell
                                    tempTableCell = tcControl;
                                    //update tempdiv
                                    tempDiv = div;
                                    //add table cell to table row
                                    trControls.Cells.Add(tcControl);
                                    //add table row to table
                                    tbControls.Rows.Add(trControls);
                                    //tempPanel.Controls.Add(tbControls);
                                    //add table to divsubgroup
                                    divSubGroup.Controls.Add(tbControls);
                                    tabIndex++;
                                    //remove the current control from the control datatable
                                    dtFieldControls.Rows.RemoveAt(0);
                                    dtFieldControls.AcceptChanges();
                                    k++;
                                    //Check any controls have same order and sequence
                                    if (k < drFieldControls.Count())
                                    {
                                        if (drFieldControls[k]["RCC_CONTROL_GROUP"].ToString() != order.ToString()
                                            || drFieldControls[k]["RCC_SEQUENCE"].ToString() != tempSequence.ToString())//increment column only if any coming control have the same sequence and order of the current control
                                        {
                                            j++;
                                        }
                                    }
                                    else
                                    {
                                        j++;
                                    }
                                }
                            }
                            if (tempDrFieldControls.Count() > 1)
                            {
                                for (; j < Cols; j++)//add the dummy cells to the last row of the table if needed
                                {
                                    System.Web.UI.WebControls.TableCell tcControl2 = new System.Web.UI.WebControls.TableCell();
                                    HtmlGenericControl div = new HtmlGenericControl("div");
                                    div.Attributes.Add("class", divColStyle);
                                    tcControl2.Controls.Add(div);
                                    trControls.Cells.Add(tcControl2);
                                }
                            }
                            else if (tempDrFieldControls.Count() == 1)
                            {
                                if (!tempDrFieldControls[0]["RCC_CONTROL_TEXT"].ToString().Equals("HiddenField")
                                    && !tempDrFieldControls[0]["RCC_CONTROL_TEXT"].ToString().Equals("TextArea")
                                    && !tempDrFieldControls[0]["RCC_CONTROL_TEXT"].ToString().Equals("Label")
                                    && !tempDrFieldControls[0]["RCC_CONTROL_TEXT"].ToString().Equals("GridView")
                                    )
                                {
                                    for (; j < Cols; j++)//add the dummy cells to the last row of the table if needed
                                    {
                                        System.Web.UI.WebControls.TableCell tcControl2 = new System.Web.UI.WebControls.TableCell();
                                        HtmlGenericControl div = new HtmlGenericControl("div");
                                        div.Attributes.Add("class", divColStyle);
                                        tcControl2.Controls.Add(div);
                                        trControls.Cells.Add(tcControl2);
                                    }
                                }
                            }
                            //Initialize i as 0
                            if (i >= dtFieldControls.Rows.Count)
                            {
                                i = 0;
                            }
                        }
                        // PageScriptOuterInit = PageScriptOuterInit + CommonFunctions.GenerateDynamicScript(ControlTypes.MonthPicker.ToString(), MonthPickerControlScript, null, null, null);

                        //Commented : Date picker is showing after selecting a month from month picker.The below script used inside the month picker section to avoide multiple calling.
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "FormatControl", CommonFunctions.GenerateDynamicScript(ControlTypes.MonthPicker.ToString(), MonthPickerControlScript, null, null, null), true);
                        //add divsubgroup to div main group
                        divMainGroup.Controls.Add(divSubGroup);
                        //add div main group to panel
                        if (IsUserControl)
                        {
                            pnlControls.Controls.Add((Control)customControl);
                        }
                        else
                            pnlControls.Controls.Add(divMainGroup);
                    }
                    //Add validation summary
                    divValidationSummary.Controls.Clear();
                    foreach (string valGroup in validationGroupList)
                    {
                        ValidationSummary vsObj = new ValidationSummary()
                        {
                            ID = "vsPage" + valGroup,
                            ValidationGroup = valGroup
                        };
                        divValidationSummary.Controls.Add(vsObj);
                    }
                    string script = "";
                    //Get the dynamic tab list
                    //GetFieldValues(ControlsEnum.DYNAMICTABS);
                    //if (spAdmFormTabCfgGetResultList != null && spAdmFormTabCfgGetResultList.Count > 0)
                    //{
                    //    foreach (SPADM_FORM_TAB_CFG_GET_Result tab in spAdmFormTabCfgGetResultList)
                    //    {
                    //        if (tab.ATC_CODE.Equals(TabCode))
                    //        {
                    //            //Get the script curresponding to the tab if any
                    //            script = string.IsNullOrEmpty(tab.ATC_SCRIPT) ? string.Empty : tab.ATC_SCRIPT;
                    //            break;
                    //        }
                    //    }
                    //}
                    PageScript = PageScript + "}";//End of initcomponents
                    //PageScript = PageScript + "$(document).ready(function () {" +
                    //            "InitComponents(true);  });";

                    //Register page script
                    //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "pagescript", PageScript, true);                    
                    //set the dictionary<order,entityname> to the session

                    PageScript = PageScript + PageScriptOuterInit;
                    //Juno, for solving script registering issue
                    scriptName = "pageInitScript" + (scriptCount++).ToString();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), scriptName, PageScript, true);
                    //ltrScriptContent.Text = "<script type=\"text/javascript\">" + PageScript + "</script>";
                    if (dicEntityGroup.Count > 0)
                    {
                        Session["EntityByGroup"] = dicEntityGroup;
                    }

                }
                else
                {
                    PageScript = PageScript + "}";
                    scriptName = "pageInitScript" + (scriptCount++).ToString();
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), scriptName, PageScript, true);
                    //ltrScriptContent.Text = "<script type=\"text/javascript\">" + PageScript + "</script>";
                }
                #endregion
            }
            catch
            {
                throw;
            }
            finally
            {
                commonService = null;
                customerRegistrationServiceClient = null;
            }
        }

        private void CreateTreeViewFromList(List<TreeBinder> source, int? parentID, TreeNode parentNode)
        {
            List<TreeBinder> newSource = source.Where(a => a.Parent.Equals(parentID)).ToList();
            foreach (var i in newSource)
            {
                TreeNode newnode = new TreeNode(i.Value, i.PK.ToString());
                newnode.ToolTip = i.Value;
                newnode.ShowCheckBox = true;
                parentNode.ChildNodes.Add(newnode);
                CreateTreeViewFromList(source, i.PK, newnode);
            }
        }

        /// <summary>
        /// Return the virtual path for autocpmplete
        /// </summary>
        /// <returns></returns>
        private string GetVirtualPathForAutocomplete()
        {
            return "var pageURL = window.document.URL;" +
            "var virtualPath = '<%= (System.Configuration.ConfigurationManager.AppSettings['VirtualDirectory'].ToString()) %>';" +
            "var url = pageURL.replace(location.pathname, virtualPath == '' ? '/Handlers/AutoComplete.ashx' : '/' + virtualPath + 'Handlers/AutoComplete.ashx');";
        }

        /// <summary>
        /// Method for Bind Grid
        /// </summary>
        /// <param name="controlPK"></param>
        /// <param name="entityName"></param>
        /// <param name="grd"></param>
        public void BindGrid(int controlPK, string entityName, GridView grd)
        {
            #region GridView
            if (!string.IsNullOrEmpty(entityName))
            {
                if (entityName.Equals("CRM_CUSTOMER_MST"))//if the entity is the master entity
                {
                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = null; ;//clear the customerpk session
                }
                if (Session[ERP.Utilities.SessionStrings.CUSTOMERPK] != null)
                {
                    CurrPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.CUSTOMERPK].ToString());//get the master entity pk
                }
                //create new instance of service. This object should be maintain until all the operation is completed. This is for maintaining the entity object context
                customerRegistrationServiceClient = new CustomerRegistrationService();
                customerRegistrationServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(customerRegistrationServiceClient);
                //Get the customer master details
                GetFieldValues(ControlsEnum.CUSTOMER);
                if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                {
                    DataTable dtGrid = null;
                    DataRow drGrid;
                    System.Data.DataColumn dcGrid;
                    int countRow = 0;
                    dtGrid = new DataTable();
                    //Get the grid reference table details. this will tell which all data to show in the grid, which shold hide,etc
                    admFormTabControlDtlList = customerRegistrationServiceClient.FormTabControlDtl(controlPK);
                    customerRegistrationServiceClient = null;
                    if (admFormTabControlDtlList != null && admFormTabControlDtlList.Count > 0)
                    {
                        //List for store the column numbers which should hide
                        List<int> hiddenColumnList = new List<int>();
                        //clear the HiddenColumnList session
                        Session["HiddenColumnList"] = null;
                        IEnumerable entityList = null;
                        Object customerObj = null;
                        if (crmCustomerMstList != null && crmCustomerMstList.Count == 1)
                        {
                            customerObj = crmCustomerMstList[0];//set the current selected customer master obj
                        }
                        retEntityObj = null;
                        Object retEntity = null;
                        if (entityName.Equals("CRM_CUSTOMER_MST"))
                        {
                            //set the master entity list for bind the grid
                            retEntity = crmCustomerMstList;
                        }
                        else
                        {
                            if (customerObj != null)
                            {
                                //Get the EntityCollection that shold be bind to the grid
                                retEntity = GetEntityCollection(customerObj, entityName);
                            }
                        }
                        if (retEntity != null)//If EntityCollection is not null
                        {
                            entityList = (IEnumerable)retEntity;
                            foreach (Object enitityObj in entityList)
                            {
                                drGrid = dtGrid.NewRow();
                                int columnCount = 0;
                                foreach (ADM_FORM_TAB_CONTROL_DTL admFormTabControlDtlObj in admFormTabControlDtlList)
                                {
                                    retVal = string.Empty;
                                    //Get the value 
                                    string value = GetPropertyValue(enitityObj, admFormTabControlDtlObj.ACD_CONTROL_ID);
                                    if (countRow == 0)//first row
                                    {
                                        dcGrid = new System.Data.DataColumn();
                                        dcGrid.ColumnName = admFormTabControlDtlObj.ACD_CONTROL_ID;//set column name
                                        dtGrid.Columns.Add(dcGrid);
                                    }
                                    //set value to the cell
                                    drGrid[admFormTabControlDtlObj.ACD_CONTROL_ID] = value;
                                    if (admFormTabControlDtlObj.ACD_IS_HIDDEN == 1)//check the cell is set to hidden or not
                                    {
                                        hiddenColumnList.Add(columnCount);//add to the hiddenfield list
                                    }
                                    //iterate column
                                    columnCount++;
                                }
                                //iterate row
                                countRow++;
                                //add rows to the data table
                                dtGrid.Rows.Add(drGrid);
                                dtGrid.AcceptChanges();
                                if (hiddenColumnList.Count > 0)
                                {
                                    Session["HiddenColumnList"] = hiddenColumnList;
                                }
                            }
                        }

                    }
                    //Bind grid
                    grd.DataSource = dtGrid;
                    grd.DataBind();
                    #region pager
                    //string usercontrolpath = System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString() == "" ?
                    //                        "~/UserControls/PagerControl.ascx" :
                    //                        "/" + System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToString() +
                    //                        "UserControls/PagerControl.ascx";
                    //string sql = "";
                    //string countsql = "";
                    //countsql = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString();
                    //if (Session["ParentPK"] != null)
                    //{
                    //    sql = sql.Replace("Key", Session["ParentPK"].ToString());
                    //}
                    //else
                    //{
                    //    sql = sql.Replace("Key", "0");
                    //}
                    //if (countsql == "")
                    //{
                    //    countsql = drFieldControls[k]["RCC_CONTROL_TEXT"].ToString();
                    //}
                    //if (Session["ParentPK"] != null)
                    //{
                    //    countsql = countsql.Replace("Key", Session["ParentPK"].ToString());
                    //}
                    //else
                    //{
                    //    countsql = countsql.Replace("Key", "0");
                    //}
                    //div.Controls.Add(grd);
                    #endregion
                }
            }
            #endregion
        }


        /// <summary>
        /// Method for creating datatable from list
        /// </summary>
        /// <returns></returns>      
        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();
            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others          will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }
                        dtReturn.Columns.Add(new System.Data.DataColumn(pi.Name, colType));
                    }
                }
                DataRow dr = dtReturn.NewRow();
                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }
                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// Get Entity Collection Object to be bind to the grid
        /// </summary>
        /// <param name="entityObj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private Object GetEntityCollection(Object entityObj, string property)
        {
            //Get type of the object
            Type objType = entityObj.GetType();
            string[] split = property.Split('.');
            PropertyInfo propObj;
            if (split.Length > 1)//If the property is referenced one
            {
                //Get the parent obj
                propObj = objType.GetProperties().AsEnumerable().SingleOrDefault(zz => zz.Name == split[0]);
                if (propObj != null && propObj.GetValue(entityObj, null) != null)//If an entity is there
                {
                    //Get the Ienumerable of the obj
                    IEnumerable tempEnum = (IEnumerable)propObj.GetValue(entityObj, null);
                    foreach (Object tempObj in tempEnum)
                    {
                        if (Session[split[0]] != null)//if the session is not null
                        {
                            PropertyInfo tempPropObj;
                            tempPropObj = null;
                            //Get the property that ends with "PK"
                            tempPropObj = tempObj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                            if (tempPropObj != null && tempPropObj.GetValue(tempObj, null) != null)
                            {
                                //If the session value matches to any of the object in the enumerable obj list
                                if (Convert.ToInt32(tempPropObj.GetValue(tempObj, null)) == Convert.ToInt32(Session[split[0]].ToString()))
                                {
                                    //Recursively call the same function with that selected obj
                                    GetEntityCollection(tempObj, property.Replace(property.Remove(property.IndexOf('.') + 1), ""));
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Recursively call the same function with the first obj in the list
                            GetEntityCollection(tempObj, property.Replace(property.Remove(property.IndexOf('.') + 1), ""));
                            break;
                        }
                    }
                }
                else//if not get any entity
                {
                    return null;
                }
            }
            else//if it the last level property
            {
                propObj = objType.GetProperties().AsEnumerable().SingleOrDefault(zz => zz.Name == property);
                if (propObj != null)
                {
                    //get and set the entity collection
                    retEntityObj = propObj.GetValue(entityObj, null) == null ? string.Empty : propObj.GetValue(entityObj, null);
                }
                else
                {
                    return null;
                }
            }
            return retEntityObj;
        }

        /// <summary>
        /// Get the property value to be show
        /// </summary>
        /// <param name="entityObj"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private string GetPropertyValue(Object entityObj, string property)
        {
            //Get the object type
            Type objType = entityObj.GetType();
            string[] split = property.Split('.');
            PropertyInfo propObj;
            if (split.Length > 1)//if it is not the last level property
            {
                // get the entity object
                propObj = objType.GetProperties().AsEnumerable().SingleOrDefault(prop => prop.Name == split[0]);
                if (propObj != null && propObj.GetValue(entityObj, null) != null)
                {
                    //recursively call the same function
                    GetPropertyValue(propObj.GetValue(entityObj, null), property.Replace(property.Remove(property.IndexOf('.') + 1), ""));
                }
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                // get the property obj
                propObj = objType.GetProperties().AsEnumerable().SingleOrDefault(prop => prop.Name == property);
                if (propObj != null)
                {
                    //get and set the value from the property obj
                    if (propObj.GetValue(entityObj, null) != null && !string.IsNullOrEmpty(propObj.GetValue(entityObj, null).ToString()))
                    {
                        if (propObj.GetValue(entityObj, null).GetType().Name.ToUpper().Equals(ControlTypes.DateTime.ToString().ToUpper())
                            || propObj.GetValue(entityObj, null).GetType().Name.ToUpper().Equals(ControlTypes.Date.ToString().ToUpper()))
                        {
                            //Format Datetime
                            retVal = Convert.ToDateTime(propObj.GetValue(entityObj, null)).ToString(Resources.ErpRes.DateFormatShort);
                        }
                        else
                        {
                            retVal = propObj.GetValue(entityObj, null).ToString();
                        }
                    }
                    else
                    {
                        retVal = string.Empty;
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
            return retVal;
        }
        /// <summary>
        /// Check the control is Dropdown
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool IsDropdownList(object val)
        {
            bool result = true;
            try
            {
                DropDownList r = (DropDownList)val;
            }
            catch
            {
                result = false;
            }
            return result;
        }
        /// <summary>
        /// Set the entity for Save or Update
        /// </summary>
        /// <param name="parentEntity"></param>
        /// <param name="entityName"></param>
        private void SaveOrUpdateEntity(Object parentEntity, string entityName)
        {
            string namespaceString = "ERPData";
            string[] entityNameArray = entityName.Split('.');
            string className = "";
            Object retEntity = null;
            if (entityNameArray.Length > 1)//it is not the last level entity
            {
                className = entityNameArray[0];
                className = namespaceString + "." + className;
                retEntityObj = null;
                //get the entitycollection object
                retEntity = GetEntityCollection(parentEntity, entityNameArray[0]);
                if (retEntity != null)//if has an entitycollection object
                {
                    int childPK;
                    if (Session[entityNameArray[0]] == null)//if session is null
                    {
                        //create new instance for the current level entity
                        Assembly currentAssembly = Assembly.Load(namespaceString);
                        Type baseEntity = currentAssembly.GetType(className);
                        object entityObj = Activator.CreateInstance(baseEntity, null);
                        if (entityObj != null)// if instance created
                        {
                            //set values from UI to the object
                            object entity = SetUIValuesToObject(ActionsEnum.SAVE, entityObj);
                            if (entity != null)
                            {
                                //get the entitycollection
                                IListSource entitySourceList = (IListSource)retEntity;
                                if (entitySourceList != null)
                                {
                                    //add the new instance to the current entitycollection
                                    entitySourceList.GetList().Add(entity);
                                    //Recurcively call the same function
                                    SaveOrUpdateEntity(entity, entityName.Replace(entityName.Remove(entityName.IndexOf('.') + 1), ""));
                                }
                            }
                        }
                    }
                    else//if session is not null
                    {
                        //get the pk from the session
                        childPK = Convert.ToInt32(Session[entityNameArray[0]].ToString());
                        IEnumerable entityEnumList = null;
                        entityEnumList = (IEnumerable)retEntity;
                        Object updateEntity = null;
                        if (entityEnumList != null)
                        {
                            foreach (Object entityobj in entityEnumList)
                            {
                                PropertyInfo propObj;
                                propObj = null;
                                //Get the property obj that ends with "PK"
                                propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                {
                                    if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == childPK)//if pk is same
                                    {
                                        updateEntity = entityobj;
                                        break;
                                    }
                                }
                            }
                            if (updateEntity != null)//if updation
                            {
                                //set values from UI to the object
                                object entity = SetUIValuesToObject(ActionsEnum.SAVE, updateEntity);
                                if (entity != null)
                                {
                                    //Recurcively call the same function
                                    SaveOrUpdateEntity(entity, entityName.Replace(entityName.Remove(entityName.IndexOf('.') + 1), ""));
                                }
                            }
                        }
                    }
                }
            }
            else if (entityNameArray.Length == 1)//If it is the last level entity
            {
                className = entityNameArray[0];
                className = namespaceString + "." + className;
                retEntityObj = null;
                //Get entitycollection obj
                retEntity = GetEntityCollection(parentEntity, entityNameArray[0]);
                if (retEntity != null)
                {
                    int childPK;
                    if (Session[entityNameArray[0]] == null)//If session is null
                    {
                        //create new instance for the current level entity
                        Assembly currentAssembly = Assembly.Load(namespaceString);
                        Type baseEntity = currentAssembly.GetType(className);
                        object entityObj = Activator.CreateInstance(baseEntity, null);
                        if (entityObj != null)
                        {
                            //set values from UI to the object
                            object entity = SetUIValuesToObject(ActionsEnum.SAVE, entityObj);
                            if (entity != null)
                            {
                                //get the entitycollection
                                IListSource entitySourceList = (IListSource)retEntity;
                                if (entitySourceList != null)
                                {
                                    //add the new instance to the current entitycollection
                                    entitySourceList.GetList().Add(entity);
                                    doSave = true;
                                }
                            }
                        }
                    }
                    else//If session is not null
                    {
                        //Get the PK value from session
                        childPK = Convert.ToInt32(Session[entityNameArray[0]].ToString());
                        IEnumerable entityEnumList = null;
                        entityEnumList = (IEnumerable)retEntity;
                        Object updateEntity = null;
                        if (entityEnumList != null)
                        {
                            foreach (Object entityobj in entityEnumList)
                            {
                                PropertyInfo propObj;
                                propObj = null;
                                //Get the property obj that ends with "PK"
                                propObj = entityobj.GetType().GetProperties().AsEnumerable().FirstOrDefault(zz => zz.Name.EndsWith("PK"));
                                if (propObj != null && propObj.GetValue(entityobj, null) != null)
                                {
                                    if (Convert.ToInt32(propObj.GetValue(entityobj, null)) == childPK)//if pk is same
                                    {
                                        updateEntity = entityobj;
                                        break;
                                    }
                                }
                            }
                            if (updateEntity != null)//if updation
                            {
                                //set values from UI to the object
                                object entity = SetUIValuesToObject(ActionsEnum.SAVE, updateEntity);
                                if (entity != null)
                                {
                                    doSave = true;
                                }
                            }
                        }
                    }
                }
            }
        }


        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(DecimalFormat);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        /// 
        private void GetFieldValues(ControlsEnum type)
        {
            //Services
            DynamicPageService DynamicPageServiceClient;
            DynamicPageServiceClient = null;
            int reportPK;
            ReportService reportServiceClient;
            reportServiceClient = null;
            ServiceUtility serviceUtilityObj;
            try
            {
                switch (type)
                {
                    #region Default
                    case ControlsEnum.DEFAULT:
                        //Get UI controls
                        DynamicPageServiceClient = new DynamicPageService();
                        DynamicPageServiceClient = CommonFunctions.InitiateClient(DynamicPageServiceClient);
                        //string tabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                        if (Session[ERP.Utilities.SessionStrings.REPORTPK] != null && Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK].ToString()) > 0)
                        {
                            reportPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK].ToString());
                            spAdmReportControlCfgGetResultList = DynamicPageServiceClient.GetReportControlsList(reportPK);
                        }
                        break;
                    #endregion
                    #region  REPORT
                    case ControlsEnum.REPORT:
                        ReportService reportService;
                        reportService = new ReportService();
                        if (Session[ERP.Utilities.SessionStrings.REPORTPK] != null && Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK].ToString()) > 0)
                        {
                            reportPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK].ToString());
                            admReportCfgList = reportService.GetReportCfg(reportPK);
                        }
                        break;
                    #endregion
                    #region Dynamic Tabs
                    case ControlsEnum.DYNAMICTABS:
                        //Get UI tabs
                        DynamicPageServiceClient = new DynamicPageService();
                        DynamicPageServiceClient = CommonFunctions.InitiateClient(DynamicPageServiceClient);
                        spAdmFormTabCfgGetResultList = DynamicPageServiceClient.GetFormTabList(FormType.CUS);
                        break;
                    #endregion
                    #region Customer
                    case ControlsEnum.CUSTOMER:
                        //Get customer master details
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        crmCustomerMstObj = ERP.Utilities.CommonFunctions.Initilize<CRM_CUSTOMER_MST>();
                        crmCustomerMstObj.CUS_PK = CurrPK;
                        crmCustomerMstObj.CUS_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        crmCustomerMstList = customerRegistrationServiceClient.GetCrmCustomerMst(crmCustomerMstObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region  ReportGroup
                    case ControlsEnum.REPORTGROUP:
                        reportServiceClient = new ReportService();
                        reportServiceClient = CommonFunctions.InitiateClient(reportServiceClient);
                        admReportGroupCfgObj = CommonFunctions.Initilize<ADM_REPORT_GROUP_CFG>();
                        // objPoHeader..VEN_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        admReportGroupCfgObj.RGC_ACTIVE = 1;
                        admReportGroupCfgObj.RGC_PK = id;
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = Convert.ToInt32(Resources.Constants.MediumCount);
                        serviceUtilityObj.FilterBy = Resources.DataFieldRes.ReportGroupName;
                        serviceUtilityObj.FilterValue = string.Empty;
                        admReportGroupCfgList = reportServiceClient.GetReportGroupAutoCompleteList(admReportGroupCfgObj, serviceUtilityObj);
                        break;
                        #endregion
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                DynamicPageServiceClient = null;
                reportServiceClient = null;
                admReportGroupCfgObj = null;
                serviceUtilityObj = null;
            }
        }
        #endregion

        #region PageLevel Events
        /// <summary>
        /// Page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }

        /// <summary>
        /// Get the User Rights, Checks Page Level Rights, 
        /// Hides sections in which user don't have access rights
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            if (!IsPostBack)
            {
                //Set breadcrumb
                if ((WebControl)(Page.Form.FindControl("MainContent").FindControl("lblBreadCrum")) != null)
                {
                    //string breadCrumb = string.Format(GetLocalResourceObject("Breadcrumb").ToString(), Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTEDTAB_NAME].ToString());
                    //breadCrumb = breadCrumb.Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
                    //((Label)(Page.Form.FindControl("MainContent").FindControl("lblBreadCrum"))).Text = breadCrumb;
                }
            }
        }
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {

        //        // Test();
        //        Test2();
        //    }
        //}
        #endregion
        string[] autuRelatedControlScript = new string[100];
        #region ActionHandler
        /// <summary>
        /// Handling control events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            long result;
            result = 0;
            HiddenField hdfGrid;
            Dictionary<int, string> dictionary;

            CommonService commonServiceClient;
            commonServiceClient = null;
            CommonService commonService;
            commonService = null;
            bool rbtnChecked;
            string message = string.Empty;
            DropDownList ddlWkfAction;
            string action;
            //string spName;
            ReportParameters reportParams;
            int itemPK = -1;
            string controlID = string.Empty;
            string commandAction = string.Empty;
            HiddenField hdfAction = null;
            try
            {
                currentUser = GetUserIdentity();
                //Get Action from CommandName
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TreeView)))
                {
                    commonActions = ActionsEnum.TREENODECHECKCHANGED;
                }

                //if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                {
                    //string tabCode = Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString();
                    switch (commonActions)
                    {
                        #region General
                        case ActionsEnum.PROCESS:
                            reportParams = SetUIValuesToXMLObject();
                            if (admReportCfgList[0].RPT_HAS_PROCESS == 1 && !string.IsNullOrEmpty(admReportCfgList[0].RPT_PROCESS_QRY))
                            {
                                string ProcessFilter = string.Empty;
                                spName = admReportCfgList[0].RPT_PROCESS_QRY;
                                ProcessFilter = reportParams.XmlSerialize();
                                //Sp Calling
                                int retVal = CommonBL.ExecuteNonQuerySP(spName, ProcessFilter);
                                if (retVal > 0) // Success !
                                {
                                    Button btn = new Button();
                                    btn.CommandName = "VIEW";
                                    ActionHandler(btn, EventArgs.Empty);
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Process_Success;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);

                                    //if (hdfReport.Value == "996") //Sale Contractwise Profit
                                    //{
                                    //    Button btn = new Button();
                                    //    btn.CommandName = "VIEW";
                                    //    ActionHandler(btn, EventArgs.Empty);
                                    //}
                                }
                                else
                                {
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }

                            break;
                        #region VIEW
                        case ActionsEnum.VIEW:
                            GetFieldValues(ControlsEnum.REPORT);
                            if (admReportCfgList == null)
                            {
                                divReportViewer.Visible = false;
                                divNodata.Visible = false;
                                txtReport.Text = string.Empty;
                                hdfReport.Value = CommonConstants.SELECT_VALUE_ZERO;
                                tblSearch.Visible = false;

                            }
                            else
                            {
                                if (IsUserControl)
                                {
                                    Control myUsrControl = pnlControls.FindControl(UserControlID) as System.Web.UI.UserControl;
                                    customControl = myUsrControl as ERPSMS_v01.Reports.UserControls.IFilterControl;
                                    reportParams = customControl.GetReportParameters();
                                    FromDate = reportParams.FromDate;
                                    ToDate = reportParams.ToDate;
                                    if (admReportCfgList[0].RPT_CODE == "STK_BATCH_MNTHWISE_RPT" || admReportCfgList[0].RPT_CODE == "STK_CARD_GRP_RPT_MNTWISE_RPT" || admReportCfgList[0].RPT_CODE == "STK_CARD_BATCH_RPT_DATA_MNTWISE")
                                    {                                        
                                        string Year = Convert.ToDateTime(FromDate).Year.ToString();
                                        string FinYear = reportParams.Parameters.Where(w => w.ParamName == "FIN_YEAR_TEXT").FirstOrDefault().Values[0].Value.ToString();
                                        if (FinYear != Year)
                                        {
                                            this.ReportViewer1.ReportSource = new Telerik.ReportViewer.Html5.WebForms.ReportSource();
                                            litErrorMsg.Text = Resources.Messages.SelectDateWithinRange;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                    }
                                }
                                else
                                    reportParams = SetUIValuesToXMLObject();
                                ///////////////////////////////////////////////for Generatting Excel  from Cost Center Wise Ledger Report-27-AUG-2019//////////////////
                                ReportParameterName paramAction = reportParams.Parameters.SingleOrDefault(s => s.ParamName == ActionsEnum.EXPORT_TO_EXCEL.ToString());
                                ActionsEnum reportAction;
                                if (paramAction != null && paramAction.Values[0].Value == "1")
                                {
                                    reportAction = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), paramAction.ParamName));
                                    switch (reportAction)
                                    {
                                        case ActionsEnum.EXPORT_TO_EXCEL:
                                            string CurrFilter = string.Empty;
                                            DataSet dsTablesById = new DataSet();
                                            dsRptData = new DataSet();
                                            if (admReportCfgList[0].RPT_STYLE == CommonConstants.IS_RPT_SP)
                                            {
                                                string spCollectionT = admReportCfgList[0].ADM_QUERIES_CFG.QRY_QUERY;
                                                string[] spNameCollection = spCollectionT.Split(',');
                                                spName = spNameCollection[0].Trim();
                                                CurrFilter = reportParams.XmlSerialize();
                                                //Sp Calling
                                                dsRptData = CommonBL.ExecuteSP(spName, CurrFilter);
                                                DataSet dsMain = dsRptData;

                                                var distinctValues = dsMain.Tables[0].AsEnumerable()
                                                    .Select(row => new
                                                    {
                                                        CostCenter = row.Field<string>("Cost Center"),
                                                    })
                                                    .Distinct();
                                                foreach (var ccRow in distinctValues)
                                                {
                                                    var resultTable = dsMain.Tables[0]
                                                     .AsEnumerable()
                                                     .Where(myRow => myRow.Field<string>("Cost Center") == ccRow.CostCenter);
                                                    DataTable dtCC = resultTable.CopyToDataTable<DataRow>();
                                                    if (dtCC != null && dtCC.Rows.Count > 0)
                                                    {
                                                        DataRow rowFooter = dtCC.NewRow();
                                                        rowFooter["Debit"] = dtCC.AsEnumerable().Sum(dr => dr.Field<decimal>("Debit"));
                                                        rowFooter["Credit"] = dtCC.AsEnumerable().Sum(dr => dr.Field<decimal>("Credit"));
                                                        dtCC.Rows.Add(rowFooter);
                                                        dtCC.AcceptChanges();

                                                    }
                                                    dtCC.TableName = ccRow.CostCenter;
                                                    dsTablesById.Tables.Add(dtCC);
                                                    dsTablesById.AcceptChanges();

                                                }

                                            }

                                            if (dsTablesById != null && dsTablesById.Tables.Count > 0)
                                                ExportDataSetToExcel(dsTablesById);
                                            break;

                                    }
                                }

                                ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                                else
                                {
                                    ReportFile = admReportCfgList[0].RPT_OP_FILE;
                                    switch (((string[])admReportCfgList[0].RPT_OP_FILE.Split('.'))[1].Trim().ToLower())
                                    {
                                        case ReportType.TelerikReport:
                                            var typeReportSource1 = new Telerik.ReportViewer.Html5.WebForms.ReportSource();
                                            bool IsTrdp = true; //Make this false for Excel export
                                            hdfShowFilter.Value = "0";
                                            hdfShowTelDev.Value = "1";
                                            string strFilter = string.Empty;
                                            strFilter = reportParams.XmlSerialize();
                                            if (IsTrdp)
                                            {
                                                typeReportSource1.Identifier = admReportCfgList[0].RPT_OP_FILE;
                                                typeReportSource1.IdentifierType = IdentifierType.TypeReportSource;
                                                typeReportSource1.Parameters.Add("P_XML", strFilter);

                                                #region Report Parameters
                                                if (IsUserControl)
                                                {
                                                    typeReportSource1.Parameters.Add("FromDate", FromDate);
                                                    typeReportSource1.Parameters.Add("ToDate", ToDate);
                                                }
                                                else
                                                {
                                                    System.Web.UI.WebControls.TextBox txtFromDate = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl("FromDate");
                                                    if (txtFromDate != null && !string.IsNullOrEmpty(txtFromDate.Text.Trim()))
                                                    {
                                                        typeReportSource1.Parameters.Add("FromDate", txtFromDate.Text.Trim());

                                                    }
                                                    System.Web.UI.WebControls.TextBox txtToDate = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl("ToDate");
                                                    if (txtToDate != null && !string.IsNullOrEmpty(txtToDate.Text.Trim()))
                                                    {
                                                        typeReportSource1.Parameters.Add("ToDate", txtToDate.Text.Trim());
                                                    }
                                                }

                                                if (pnlControls.FindControl("CMP_PK") != null)
                                                    GetCompanyDetails(Convert.ToInt32(((DropDownList)pnlControls.FindControl("CMP_PK")).SelectedValue));
                                                else
                                                    GetCompanyDetails(null, currentUser.SBUID);
                                                typeReportSource1.Parameters.Add("Logo", LogoPath);
                                                #endregion
                                                //this.telReportViewer.ReportSource = typeReportSource1;
                                                //typeReportSource1.Parameters.Add("P_XML", "<FilterParameters><FromDate>9-Mar-2021</FromDate><ToDate>9-Mar-2021</ToDate><BizUnit>1</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2026</RptPK><Currency>133</Currency><Parameter><ParamName>ITM_PK</ParamName></Parameter></FilterParameters>");

                                                this.ReportViewer1.ReportSource = typeReportSource1;
                                            }
                                            else//Excel Export
                                            {
                                                var reportPackager = new ReportPackager();
                                                Telerik.Reporting.Report rptPrint = null;
                                                using (var sourceStream = System.IO.File.OpenRead(Server.MapPath(admReportCfgList[0].RPT_OP_FILE)))
                                                {
                                                    rptPrint = (Telerik.Reporting.Report)reportPackager.UnpackageDocument(sourceStream);
                                                }

                                                #region Parameters
                                                if (rptPrint.ReportParameters.Where(w => w.Name == "P_XML") != null &&
                                                        rptPrint.ReportParameters.Where(w => w.Name == "P_XML").Any())
                                                {
                                                    rptPrint.ReportParameters.Where(w => w.Name == "P_XML").First().Value = strFilter;
                                                }

                                                if (rptPrint.ReportParameters.Where(w => w.Name == "FromDate") != null &&
                                                        rptPrint.ReportParameters.Where(w => w.Name == "FromDate").Any())
                                                {
                                                    System.Web.UI.WebControls.TextBox txt_FromDate = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl("FromDate");
                                                    if (txt_FromDate != null && !string.IsNullOrEmpty(txt_FromDate.Text.Trim()))
                                                    {
                                                        rptPrint.ReportParameters.Where(w => w.Name == "FromDate").First().Value = txt_FromDate.Text.Trim();
                                                    }
                                                    else
                                                    {
                                                        rptPrint.ReportParameters.Where(w => w.Name == "FromDate").First().Value = string.Empty;
                                                    }
                                                }

                                                if (rptPrint.ReportParameters.Where(w => w.Name == "ToDate") != null &&
                                                        rptPrint.ReportParameters.Where(w => w.Name == "ToDate").Any())
                                                {
                                                    System.Web.UI.WebControls.TextBox txt_ToDate = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl("ToDate");
                                                    if (txt_ToDate != null && !string.IsNullOrEmpty(txt_ToDate.Text.Trim()))
                                                    {
                                                        rptPrint.ReportParameters.Where(w => w.Name == "ToDate").First().Value = LogoPath;
                                                    }
                                                    else
                                                    {
                                                        rptPrint.ReportParameters.Where(w => w.Name == "ToDate").First().Value = string.Empty;
                                                    }
                                                }
                                                if (rptPrint.ReportParameters.Where(w => w.Name == "Logo") != null &&
                                                        rptPrint.ReportParameters.Where(w => w.Name == "Logo").Any())
                                                {
                                                    rptPrint.ReportParameters.Where(w => w.Name == "Logo").First().Value = LogoPath;
                                                }
                                                #endregion

                                                ExportReport(rptPrint);
                                            }                                            
                                            break;
                                    }
                                }
                            }

                            break;
                        #endregion
                        #region SELECTEDINDEXCHANGED
                        case ActionsEnum.SELECTEDINDEXCHANGED:
                            commonServiceClient = new CommonService();
                            commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                            string parentControlID = string.Empty;
                            string parentControl = string.Empty;
                            #region Selected Cotrol

                            if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                            {
                                itemPK = string.IsNullOrEmpty(((DropDownList)sender).SelectedValue) ? -1 : Convert.ToInt32(((DropDownList)sender).SelectedValue);
                                parentControl = controlID = ((DropDownList)sender).ID;
                                hdfAction = (HiddenField)pnlControls.FindControl("hdf" + ((DropDownList)sender).ID + "Action");
                                commandAction = (hdfAction != null && !string.IsNullOrEmpty(hdfAction.Value)) ? hdfAction.Value : string.Empty;
                            }
                            else
                                if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))//
                            {
                                //juno
                                string[] commandArgument = ((ImageButton)sender).CommandArgument.ToString().Split(',');
                                parentControlID = commandArgument[0];
                                parentControl = ((string[])commandArgument[1].Split('|'))[0];
                                itemPK = string.IsNullOrEmpty(((HiddenField)pnlControls.FindControl(parentControlID)).Value) ? -1 : Convert.ToInt32(((HiddenField)pnlControls.FindControl(parentControlID)).Value);
                                controlID = commandArgument[1];
                                commandAction = ((ImageButton)sender).CommandName;

                            }

                            if (itemPK > 0)
                            {
                                ////get the selected value from the dropdown
                                // itemPK = Convert.ToInt32(((DropDownList)sender).SelectedValue);
                                //Get the Spname curresponding to the dropdown

                                //if (hdfAction != null && !string.IsNullOrEmpty(hdfAction.Value))
                                if (!string.IsNullOrEmpty(commandAction))
                                {
                                    if (commandAction.Equals("SELECTEDINDEXCHANGED"))
                                    {
                                        //string ddlControlId = spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_REL_CONTROL_ID == ((DropDownList)sender).ID && rel.RCC_CONTROL_TEXT.Equals("DropDown")) == null
                                        //                        ? string.Empty : spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_REL_CONTROL_ID == ((DropDownList)sender).ID && rel.RCC_CONTROL_TEXT.Equals("DropDown")).RCC_CONTROL_ID;
                                        List<SPADM_REPORT_CONTROL_CFG_GET_Result> ddlControlIdList = null;
                                        List<SPADM_REPORT_CONTROL_CFG_GET_Result> treeControlIDList = null;
                                        List<SPADM_REPORT_CONTROL_CFG_GET_Result> autoControlIDList = null;
                                        List<SPADM_REPORT_CONTROL_CFG_GET_Result> multiTreeControlIDList = null;
                                        List<SPADM_REPORT_CONTROL_CFG_GET_Result> autoCheckList = null;

                                        ddlControlIdList = spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("DropDown")) == null
                                                                ? null : spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("DropDown")).ToList();
                                        treeControlIDList = spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("TreeView")) == null
                                                                ? null : spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("TreeView")).ToList();
                                        autoControlIDList = spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("AutoComplete")) == null
                                                                ? null : spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("AutoComplete")).ToList();
                                        multiTreeControlIDList = spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("MultiLevelTree")) == null
                                                                ? null : spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("MultiLevelTree")).ToList();
                                        autoCheckList = spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("AutoCheckList")) == null
                                                              ? null : spAdmReportControlCfgGetResultList.Where(rel => rel.RCC_REL_CONTROL_ID == controlID && rel.RCC_CONTROL_TEXT.Equals("AutoCheckList")).ToList();


                                        string ddlControlId = "";
                                        string treeControlID = "";
                                        string autoControlID = "";
                                        string multitreeControlID = "";
                                        string autoCheckListControlID = "";

                                        #region Dropdown List
                                        foreach (SPADM_REPORT_CONTROL_CFG_GET_Result obj in ddlControlIdList)
                                        {
                                            ddlControlId = obj.RCC_CONTROL_ID;
                                            if (!string.IsNullOrEmpty(ddlControlId))
                                            {
                                                DropDownList child = null;
                                                HiddenField hdnDropSelectedValue = null;
                                                if (!string.IsNullOrEmpty(ddlControlId))
                                                {
                                                    child = (DropDownList)pnlControls.FindControl(ddlControlId);
                                                    hdnDropSelectedValue = (HiddenField)pnlControls.FindControl("hdnDropSelectedValue_" + ddlControlId);
                                                }
                                                if (child != null)
                                                {
                                                    string query = "";
                                                    if (!string.IsNullOrEmpty(ddlControlId))
                                                    {
                                                        query = spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == ddlControlId) == null
                                                           ? string.Empty : spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == ddlControlId).RCC_QUERY_TEXT;
                                                    }

                                                    if (!string.IsNullOrEmpty(query))
                                                    {
                                                        //CommonService commonService;
                                                        //commonService = null;
                                                        string value = string.Empty;
                                                        //Replace query with the values if any condition is there
                                                        List<string> conditionList = new List<string>();
                                                        string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                                        if (splitWithAt.Count() > 1)
                                                        {
                                                            for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                            {
                                                                conditionList.Add(splitWithAt[arrayCount].Trim());
                                                            }
                                                            DropDownList parent = ((DropDownList)sender);
                                                            if (parent != null && parent.Items.Count > 1)
                                                            {
                                                                if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                {
                                                                    value = parent.SelectedValue;
                                                                }
                                                            }
                                                            foreach (string condition in conditionList)
                                                            {
                                                                if (Session[condition] != null)//parameter name is same as any session name
                                                                {
                                                                    if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                                        query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (condition == parent.ID)// parameter is value of any other control
                                                                {
                                                                    if (!string.IsNullOrEmpty(value))
                                                                        query = query.Replace("@" + condition + "@", value);
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (condition == "CMP_PK")// parameter is value of any other control
                                                                {
                                                                    DropDownList ddlCompany = (DropDownList)pnlControls.FindControl("CMP_PK");
                                                                    string CompanyPK = ddlCompany.SelectedItem.Value;
                                                                    if (!string.IsNullOrEmpty(CompanyPK))
                                                                        query = query.Replace("@" + condition + "@", CompanyPK);
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (condition == "USER_PK")// parameter is value of any other control
                                                                {
                                                                    currentUser = GetUserIdentity();
                                                                    string UserPK = currentUser.PKUser.ToString();
                                                                    if (!string.IsNullOrEmpty(UserPK))
                                                                        query = query.Replace("@" + condition + "@", UserPK);
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(query))
                                                        {
                                                            commonService = new CommonService();
                                                            commonService = CommonFunctions.InitiateClient(commonService);
                                                            //Execute query
                                                            query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                            List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);
                                                            if (child != null)
                                                            {
                                                                child.DataTextField = "Value";
                                                                child.DataValueField = "PK";
                                                                child.DataSource = CommonFunctions.HtmlDecode(ddlValues, "Value");
                                                                child.DataBind();
                                                                //set 
                                                                if (hdnDropSelectedValue != null)
                                                                {
                                                                    int index = -2;
                                                                    if (Int32.TryParse(hdnDropSelectedValue.Value, out index))
                                                                    {
                                                                        if (index > -2)
                                                                        {
                                                                            ListItem li = child.Items.FindByValue(index.ToString());
                                                                            if (li != null)
                                                                            {
                                                                                child.SelectedValue = index.ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (child != null)
                                                            {
                                                                child.Items.Clear();
                                                            }
                                                        }
                                                    }
                                                    //add "select" to the dropdown
                                                    if (child != null)
                                                    {
                                                        child.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Auto CheckList
                                        foreach (SPADM_REPORT_CONTROL_CFG_GET_Result obj in autoCheckList)
                                        {
                                            autoCheckListControlID = obj.RCC_CONTROL_ID;
                                            if (!string.IsNullOrEmpty(autoCheckListControlID))
                                            {
                                                CheckListSearchControl checkListSearchControl = null;
                                                if (!string.IsNullOrEmpty(autoCheckListControlID))
                                                {
                                                    checkListSearchControl = (CheckListSearchControl)pnlControls.FindControl(autoCheckListControlID);
                                                }
                                                if (checkListSearchControl != null)
                                                {
                                                    string query = "";
                                                    if (!string.IsNullOrEmpty(autoCheckListControlID))
                                                    {
                                                        query = spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == autoCheckListControlID) == null
                                                           ? string.Empty : spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == autoCheckListControlID).RCC_QUERY_TEXT;
                                                    }
                                                    if (!string.IsNullOrEmpty(query))
                                                    {
                                                        //CommonService commonService;
                                                        //commonService = null;
                                                        string value = string.Empty;
                                                        //Replace query with the values if any condition is there
                                                        List<string> conditionList = new List<string>();
                                                        string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                                        if (splitWithAt.Count() > 1)
                                                        {
                                                            for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                            {
                                                                conditionList.Add(splitWithAt[arrayCount].Trim());
                                                            }
                                                            DropDownList parent = ((DropDownList)sender);
                                                            if (parent != null && parent.Items.Count > 1)
                                                            {
                                                                if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                {
                                                                    value = parent.SelectedValue;
                                                                }
                                                            }
                                                            foreach (string condition in conditionList)
                                                            {
                                                                if (Session[condition] != null)//parameter name is same as any session name
                                                                {
                                                                    if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                                        query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (condition == parent.ID)// parameter is value of any other control
                                                                {
                                                                    if (!string.IsNullOrEmpty(value))
                                                                        query = query.Replace("@" + condition + "@", value);
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(query))
                                                        {
                                                            commonService = new CommonService();
                                                            commonService = CommonFunctions.InitiateClient(commonService);
                                                            //Execute query
                                                            currentUser = GetUserIdentity();
                                                            query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                            List<DDLMaster> chkValues = commonService.ExecuteQuery(query);
                                                            if (checkListSearchControl != null)
                                                            {
                                                                if (chkValues != null && chkValues.Count > 0)
                                                                {
                                                                    checkListSearchControl.ListData = CommonFunctions.HtmlDecode(chkValues, "Value");
                                                                    checkListSearchControl.BindData();
                                                                }
                                                                else
                                                                {
                                                                    checkListSearchControl.ClearData();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (checkListSearchControl != null)
                                                            {
                                                                checkListSearchControl.ClearData();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Tree List
                                        foreach (SPADM_REPORT_CONTROL_CFG_GET_Result obj in treeControlIDList)
                                        {
                                            treeControlID = obj.RCC_CONTROL_ID;
                                            if (!string.IsNullOrEmpty(treeControlID))
                                            {
                                                TreeView childTree = null;
                                                if (!string.IsNullOrEmpty(treeControlID))
                                                {
                                                    childTree = (TreeView)pnlControls.FindControl(treeControlID);
                                                }
                                                if (childTree != null)
                                                {
                                                    string query = "";
                                                    if (!string.IsNullOrEmpty(treeControlID))
                                                    {
                                                        query = spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == treeControlID) == null
                                                           ? string.Empty : spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == treeControlID).RCC_QUERY_TEXT;
                                                    }
                                                    if (!string.IsNullOrEmpty(query))
                                                    {
                                                        //CommonService commonService;
                                                        //commonService = null;
                                                        string value = string.Empty;
                                                        //Replace query with the values if any condition is there
                                                        List<string> conditionList = new List<string>();
                                                        string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                                        if (splitWithAt.Count() > 1)
                                                        {
                                                            for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                            {
                                                                conditionList.Add(splitWithAt[arrayCount].Trim());
                                                            }
                                                            DropDownList parent = ((DropDownList)sender);
                                                            if (parent != null && parent.Items.Count > 1)
                                                            {
                                                                if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                {
                                                                    value = parent.SelectedValue;
                                                                }
                                                            }
                                                            foreach (string condition in conditionList)
                                                            {
                                                                if (Session[condition] != null)//parameter name is same as any session name
                                                                {
                                                                    if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                                        query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (condition == parent.ID)// parameter is value of any other control
                                                                {
                                                                    if (!string.IsNullOrEmpty(value))
                                                                        query = query.Replace("@" + condition + "@", value);
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(query))
                                                        {
                                                            commonService = new CommonService();
                                                            commonService = CommonFunctions.InitiateClient(commonService);
                                                            //Execute query
                                                            currentUser = GetUserIdentity();
                                                            query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                            List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);
                                                            if (childTree != null)
                                                            {
                                                                TreeNode root = childTree.Nodes[0];
                                                                TreeNode childNode;
                                                                root.Collapse();
                                                                root.ChildNodes.Clear();
                                                                if (ddlValues != null && ddlValues.Count > 0)
                                                                {
                                                                    foreach (DDLMaster item in ddlValues)
                                                                    {
                                                                        childNode = new TreeNode();
                                                                        childNode.ShowCheckBox = true;
                                                                        childNode.Text = item.Value;
                                                                        childNode.ToolTip = item.Value;
                                                                        childNode.Value = item.PK.ToString();
                                                                        root.ChildNodes.Add(childNode);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    root.ChildNodes.Clear();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (childTree != null)
                                                            {
                                                                TreeNode root = childTree.Nodes[0];
                                                                root.ChildNodes.Clear();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region AutoCompleteList
                                        foreach (SPADM_REPORT_CONTROL_CFG_GET_Result obj in autoControlIDList)
                                        {
                                            autoControlID = obj.RCC_CONTROL_ID;
                                            string[] autoCompleteIDArray = autoControlID.Split('|');
                                            string value = string.Empty;
                                            if (!string.IsNullOrEmpty(autoControlID))
                                            {
                                                System.Web.UI.WebControls.TextBox txtchild = null;
                                                HiddenField hdnchild = null;
                                                // HiddenField hdnDropSelectedValue = null;
                                                if (autoCompleteIDArray.Length == 2)
                                                {
                                                    hdnchild = (HiddenField)pnlControls.FindControl("hdfAuto" + autoCompleteIDArray[1]);
                                                    txtchild = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl("txt" + autoCompleteIDArray[0]);
                                                    //hdnDropSelectedValue = (HiddenField)pnlControls.FindControl("hdnDropSelectedValue_" + aut);
                                                }
                                                if (hdnchild != null && txtchild != null)
                                                {
                                                    hdnchild.Value = string.Empty;
                                                    txtchild.Text = string.Empty;
                                                    string searchValue = !string.IsNullOrEmpty(txtchild.Text) ? "'%" + hdnchild.Value + "%'" : "'%%'";
                                                    string hdfParent = "hdfAuto";
                                                    #region Get Parent Dropdown Value
                                                    //juno2
                                                    if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                                                    {

                                                        DropDownList parent = ((DropDownList)sender);
                                                        if (parent != null && parent.Items.Count > 1)
                                                        {
                                                            if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                            {
                                                                value = parent.SelectedValue;
                                                                if (AutoCompleteRelatedControlValue == null)
                                                                    AutoCompleteRelatedControlValue = new Dictionary<string, string>();
                                                                if (AutoCompleteRelatedControlValue.ContainsKey(autoCompleteIDArray[0]))
                                                                {
                                                                    Dictionary<string, string> tmp = AutoCompleteRelatedControlValue;
                                                                    tmp[autoCompleteIDArray[0]] = value;
                                                                    AutoCompleteRelatedControlValue = tmp;
                                                                }
                                                                else
                                                                {
                                                                    Dictionary<string, string> tmp = AutoCompleteRelatedControlValue;
                                                                    tmp.Add(autoCompleteIDArray[0], value);
                                                                    AutoCompleteRelatedControlValue = tmp;
                                                                }
                                                            }
                                                        }
                                                        hdfParent += autoCompleteIDArray[1];
                                                    }
                                                    else
                                                        if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))//
                                                    {

                                                        if (!string.IsNullOrEmpty(parentControlID))
                                                        {
                                                            if (itemPK > 0)
                                                            {
                                                                value = itemPK.ToString();
                                                                if (AutoCompleteRelatedControlValue == null)
                                                                    AutoCompleteRelatedControlValue = new Dictionary<string, string>();
                                                                if (AutoCompleteRelatedControlValue.ContainsKey(autoCompleteIDArray[0]))
                                                                {
                                                                    Dictionary<string, string> tmp = AutoCompleteRelatedControlValue;
                                                                    tmp[autoCompleteIDArray[0]] = value;
                                                                    AutoCompleteRelatedControlValue = tmp;
                                                                }
                                                                else
                                                                {
                                                                    Dictionary<string, string> tmp = AutoCompleteRelatedControlValue;
                                                                    tmp.Add(autoCompleteIDArray[0], value);
                                                                    AutoCompleteRelatedControlValue = tmp;
                                                                }
                                                            }
                                                        }
                                                        hdfParent += parentControl;
                                                    }
                                                    #endregion
                                                    string queryID = Convert.ToString(obj.RCC_QUERY ?? 0);
                                                    string qryStringParams = string.Format("{0}={1}&{2}={3}&{4}={5}"
                                                                                    , RequestParameters.Query
                                                                                    , queryID
                                                                                    , RequestParameters.RelCtrl
                                                                                    , parentControl //parent.ID
                                                                                    , RequestParameters.RelCtrlValue
                                                                                    , value
                                                                                );

                                                    string autoCompleteScript = "function AutoInitComponents(flag) { GrandScriptUtils.MakeAutoCompleteDDL('txt" + autoCompleteIDArray[0] + "', BuildAutoCompleteUrl('" + qryStringParams + "'), 'hdfAuto" + autoCompleteIDArray[1] + "', true, true, 'EXECUTEQUERY');}";
                                                    autuRelatedControlScript[0] = autoCompleteScript;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AutoCmplt", autoCompleteScript, true);
                                                    AutoInitComponents = true;
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Multi Tree List
                                        foreach (SPADM_REPORT_CONTROL_CFG_GET_Result obj in multiTreeControlIDList)
                                        {
                                            multitreeControlID = obj.RCC_CONTROL_ID;
                                            if (!string.IsNullOrEmpty(multitreeControlID))
                                            {
                                                TreeView childTree = null;
                                                if (!string.IsNullOrEmpty(multitreeControlID))
                                                {
                                                    childTree = (TreeView)pnlControls.FindControl(multitreeControlID);
                                                }
                                                if (childTree != null)
                                                {
                                                    string query = "";
                                                    if (!string.IsNullOrEmpty(multitreeControlID))
                                                    {
                                                        query = spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == multitreeControlID) == null
                                                           ? string.Empty : spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == multitreeControlID).RCC_QUERY_TEXT;
                                                    }
                                                    if (!string.IsNullOrEmpty(query))
                                                    {
                                                        //CommonService commonService;
                                                        //commonService = null;
                                                        string value = string.Empty;
                                                        //Replace query with the values if any condition is there
                                                        List<string> conditionList = new List<string>();
                                                        string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                                        if (splitWithAt.Count() > 1)
                                                        {
                                                            for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                            {
                                                                conditionList.Add(splitWithAt[arrayCount].Trim());
                                                            }
                                                            DropDownList parent = ((DropDownList)sender);
                                                            if (parent != null && parent.Items.Count > 1)
                                                            {
                                                                if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                {
                                                                    value = parent.SelectedValue;
                                                                }
                                                            }
                                                            foreach (string condition in conditionList)
                                                            {
                                                                if (Session[condition] != null)//parameter name is same as any session name
                                                                {
                                                                    if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                                        query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (condition == parent.ID)// parameter is value of any other control
                                                                {
                                                                    if (!string.IsNullOrEmpty(value))
                                                                        query = query.Replace("@" + condition + "@", value);
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(query))
                                                        {
                                                            //commonService = new CommonService();
                                                            //commonService = CommonFunctions.InitiateClient(commonService);
                                                            //Execute query
                                                            //List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);

                                                            commonService = new CommonService();
                                                            commonService = CommonFunctions.InitiateClient(commonService);
                                                            //Execute query
                                                            List<TreeBinder> TrvValues = commonService.ExecuteTreeQuery(query);

                                                            if (childTree != null)
                                                            {
                                                                TreeNode root = childTree.Nodes[0];
                                                                root.Collapse();
                                                                root.ChildNodes.Clear();
                                                                root.ShowCheckBox = true;

                                                                #region Old Code
                                                                //if (ddlValues != null && ddlValues.Count > 0)
                                                                //{
                                                                //    foreach (DDLMaster item in ddlValues)
                                                                //    {
                                                                //        childNode = new TreeNode();
                                                                //        childNode.ShowCheckBox = true;
                                                                //        childNode.Text = item.Value;
                                                                //        childNode.ToolTip = item.Value;
                                                                //        childNode.Value = item.PK.ToString();
                                                                //        root.ChildNodes.Add(childNode);
                                                                //    }
                                                                //}
                                                                //else
                                                                //{
                                                                //    root.ChildNodes.Clear();
                                                                //} 
                                                                #endregion

                                                                if (TrvValues != null && TrvValues.Count > 0)
                                                                {
                                                                    root.ChildNodes.Clear();
                                                                    CreateTreeViewFromList(TrvValues, null, root);
                                                                }
                                                                else
                                                                {
                                                                    root.ChildNodes.Clear();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (childTree != null)
                                                            {
                                                                TreeNode root = childTree.Nodes[0];
                                                                root.ChildNodes.Clear();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //Get the sp name
                                        spName = hdfAction.Value;
                                        // set the parameters of the sp
                                        object[] methodParams = new object[] { itemPK };
                                        //Execute SP
                                        Object retObj = commonServiceClient.ExecuteSP(spName, methodParams);
                                        if (retObj != null)
                                        {
                                            //SP retuurns Objectresult
                                            ObjectResult objResult = (ObjectResult)retObj;
                                            int count = 0;
                                            foreach (Object srcObj in objResult)
                                            {
                                                //set the UI controls with the SP return list
                                                GetUIValuesFromObject(ActionsEnum.SELECTEDINDEXCHANGED, srcObj);
                                                count++;
                                            }
                                            if (count == 0)//If the sp return list is empty
                                            {
                                                //Create a new instance of SP Complex type
                                                string namespaceString = "ERPData";
                                                string className = objResult.ElementType.Name;
                                                className = namespaceString + "." + className;
                                                Assembly currentAssembly = Assembly.Load(namespaceString);
                                                Type baseEntity = currentAssembly.GetType(className);
                                                Object entityObj = Activator.CreateInstance(baseEntity, null);
                                                //set the UI controls with the SP Complextype object
                                                GetUIValuesFromObject(ActionsEnum.SELECTEDINDEXCHANGED, entityObj);
                                            }
                                        }
                                    }
                                }
                            }
                            #endregion
                            //Clear RDLC
                            divReportViewer.Visible = false;
                            divNodata.Visible = false;
                            break;
                        #endregion
                        #region TREENODECHECKCHANGED
                        case ActionsEnum.TREENODECHECKCHANGED:
                            commonServiceClient = new CommonService();
                            commonServiceClient = CommonFunctions.InitiateClient(commonServiceClient);
                            TreeView treeView = (TreeView)sender;
                            if (treeView.CheckedNodes.Count > 0)
                            {
                                //get the selected value from the dropdown
                                string selectedPkList = string.Empty;
                                foreach (TreeNode node in treeView.CheckedNodes)
                                {
                                    selectedPkList += string.Format("{0},", node.Value.Trim());
                                }

                                if (selectedPkList.EndsWith(",")) selectedPkList = selectedPkList.Remove(selectedPkList.Length - 1);
                                //Get the Spname curresponding to the dropdown
                                hdfAction = (HiddenField)pnlControls.FindControl("hdf" + treeView.ID + "Action");
                                if (hdfAction != null && !string.IsNullOrEmpty(hdfAction.Value))
                                {
                                    if (hdfAction.Value.Equals("TREENODECHECKCHANGED"))
                                    {
                                        List<SPADM_REPORT_CONTROL_CFG_GET_Result> ddlControlIdList = null;
                                        List<SPADM_REPORT_CONTROL_CFG_GET_Result> treeControlIDList = null;
                                        ddlControlIdList = spAdmReportControlCfgGetResultList
                                                            .Where(rel => rel.RCC_REL_CONTROL_ID == treeView.ID
                                                                && rel.RCC_CONTROL_TEXT.Equals("DropDown")) == null
                                                                       ? null
                                                                       : spAdmReportControlCfgGetResultList
                                                                         .Where(rel => rel.RCC_REL_CONTROL_ID == treeView.ID
                                                                             && rel.RCC_CONTROL_TEXT.Equals("DropDown"))
                                                                         .ToList();
                                        treeControlIDList = spAdmReportControlCfgGetResultList
                                                            .Where(rel => rel.RCC_REL_CONTROL_ID == treeView.ID
                                                                && rel.RCC_CONTROL_TEXT.Equals("TreeView")) == null
                                                                ? null
                                                                : spAdmReportControlCfgGetResultList
                                                                    .Where(rel => rel.RCC_REL_CONTROL_ID == treeView.ID
                                                                        && rel.RCC_CONTROL_TEXT.Equals("TreeView")).ToList();
                                        string ddlControlId = "";
                                        string treeControlID = "";
                                        #region Related DropDown Binding
                                        foreach (SPADM_REPORT_CONTROL_CFG_GET_Result obj in ddlControlIdList)
                                        {
                                            ddlControlId = obj.RCC_CONTROL_ID;
                                            if (!string.IsNullOrEmpty(ddlControlId))
                                            {
                                                DropDownList child = null;
                                                HiddenField hdnDropSelectedValue = null;
                                                if (!string.IsNullOrEmpty(ddlControlId))
                                                {
                                                    child = (DropDownList)pnlControls.FindControl(ddlControlId);
                                                    hdnDropSelectedValue = (HiddenField)pnlControls.FindControl("hdnDropSelectedValue_" + ddlControlId);
                                                }
                                                if (child != null)
                                                {
                                                    string query = "";
                                                    if (!string.IsNullOrEmpty(ddlControlId))
                                                    {
                                                        query = spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == ddlControlId) == null
                                                           ? string.Empty : spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == ddlControlId).RCC_QUERY_TEXT;
                                                    }

                                                    if (!string.IsNullOrEmpty(query))
                                                    {
                                                        //CommonService commonService;
                                                        //commonService = null;
                                                        //string value = string.Empty;
                                                        //Replace query with the values if any condition is there
                                                        List<string> conditionList = new List<string>();
                                                        string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                                        if (splitWithAt.Count() > 1)
                                                        {
                                                            for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                            {
                                                                conditionList.Add(splitWithAt[arrayCount].Trim());
                                                            }
                                                            /*TreeView parent = ((TreeView)sender);
                                                            if (parent != null && parent.Items.Count > 1)
                                                            {
                                                                if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                {
                                                                    value = parent.SelectedValue;
                                                                }
                                                            }*/
                                                            foreach (string condition in conditionList)
                                                            {
                                                                if (Session[condition] != null)//parameter name is same as any session name
                                                                {
                                                                    if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                                        query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (condition == treeView.ID)// parameter is value of any other control
                                                                {
                                                                    if (!string.IsNullOrEmpty(selectedPkList))
                                                                        query = query.Replace("@" + condition + "@", selectedPkList);
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(query))
                                                        {
                                                            commonService = new CommonService();
                                                            commonService = CommonFunctions.InitiateClient(commonService);
                                                            //Execute query
                                                            currentUser = GetUserIdentity();
                                                            query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                            List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);
                                                            if (child != null)
                                                            {
                                                                child.DataTextField = "Value";
                                                                child.DataValueField = "PK";
                                                                child.DataSource = CommonFunctions.HtmlDecode(ddlValues, "Value");
                                                                child.DataBind();
                                                                //set 
                                                                if (hdnDropSelectedValue != null)
                                                                {
                                                                    int index = -2;
                                                                    if (Int32.TryParse(hdnDropSelectedValue.Value, out index))
                                                                    {
                                                                        if (index > -2)
                                                                        {
                                                                            ListItem li = child.Items.FindByValue(index.ToString());
                                                                            if (li != null)
                                                                            {
                                                                                child.SelectedValue = index.ToString();
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (child != null)
                                                            {
                                                                child.Items.Clear();
                                                            }
                                                        }
                                                    }
                                                    //add "select" to the dropdown
                                                    if (child != null)
                                                    {
                                                        child.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Related Treeview Binding
                                        foreach (SPADM_REPORT_CONTROL_CFG_GET_Result obj in treeControlIDList)
                                        {
                                            treeControlID = obj.RCC_CONTROL_ID;
                                            if (!string.IsNullOrEmpty(treeControlID))
                                            {
                                                TreeView childTree = null;
                                                if (!string.IsNullOrEmpty(treeControlID))
                                                {
                                                    childTree = (TreeView)pnlControls.FindControl(treeControlID);
                                                }
                                                if (childTree != null)
                                                {
                                                    string query = "";
                                                    if (!string.IsNullOrEmpty(treeControlID))
                                                    {
                                                        query = spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == treeControlID) == null
                                                           ? string.Empty : spAdmReportControlCfgGetResultList.SingleOrDefault(rel => rel.RCC_CONTROL_ID == treeControlID).RCC_QUERY_TEXT;
                                                    }
                                                    if (!string.IsNullOrEmpty(query))
                                                    {
                                                        //CommonService commonService;
                                                        //commonService = null;
                                                        //string value = string.Empty;
                                                        //Replace query with the values if any condition is there
                                                        List<string> conditionList = new List<string>();
                                                        string[] splitWithAt = query.Trim().Split('@');//the parameter in query wil start with @
                                                        if (splitWithAt.Count() > 1)
                                                        {
                                                            for (int arrayCount = 1; arrayCount < splitWithAt.Count(); arrayCount += 2)
                                                            {
                                                                conditionList.Add(splitWithAt[arrayCount].Trim());
                                                            }
                                                            /*DropDownList parent = ((DropDownList)sender);
                                                            if (parent != null && parent.Items.Count > 1)
                                                            {
                                                                if (parent.SelectedValue != CommonConstants.SELECTVAL)
                                                                {
                                                                    value = parent.SelectedValue;
                                                                }
                                                            }*/
                                                            foreach (string condition in conditionList)
                                                            {
                                                                if (Session[condition] != null)//parameter name is same as any session name
                                                                {
                                                                    if (!string.IsNullOrEmpty(Session[condition].ToString()))
                                                                        query = query.Replace("@" + condition + "@", Session[condition].ToString());
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                                else if (condition == treeView.ID)// parameter is value of any other control
                                                                {
                                                                    if (!string.IsNullOrEmpty(selectedPkList))
                                                                        query = query.Replace("@" + condition + "@", selectedPkList);
                                                                    else
                                                                    {
                                                                        query = string.Empty;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                        if (!string.IsNullOrEmpty(query))
                                                        {
                                                            commonService = new CommonService();
                                                            commonService = CommonFunctions.InitiateClient(commonService);
                                                            //Execute query
                                                            currentUser = GetUserIdentity();
                                                            query = query.Replace("@BIZUNITPK@", currentUser.SBUID.ToString());
                                                            List<DDLMaster> ddlValues = commonService.ExecuteQuery(query);
                                                            if (childTree != null)
                                                            {
                                                                TreeNode root = childTree.Nodes[0];
                                                                TreeNode childNode;
                                                                root.Collapse();
                                                                root.ChildNodes.Clear();
                                                                if (ddlValues != null && ddlValues.Count > 0)
                                                                {
                                                                    foreach (DDLMaster item in ddlValues)
                                                                    {
                                                                        childNode = new TreeNode();
                                                                        childNode.ShowCheckBox = true;
                                                                        childNode.Text = item.Value;
                                                                        childNode.ToolTip = item.Value;
                                                                        childNode.Value = item.PK.ToString();
                                                                        root.ChildNodes.Add(childNode);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    root.ChildNodes.Clear();
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (childTree != null)
                                                            {
                                                                TreeNode root = childTree.Nodes[0];
                                                                root.ChildNodes.Clear();
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        /*
                                          //Get the sp name
                                           spName = hdfAction.Value;
                                           // set the parameters of the sp
                                           //object[] methodParams = new object[] { itemPK };
                                           object[] methodParams = new object[] { selectedPkList };
                                           //Execute SP
                                           Object retObj = commonServiceClient.ExecuteSP(spName, methodParams);
                                           if (retObj != null)
                                           {
                                               //SP retuurns Objectresult
                                               ObjectResult objResult = (ObjectResult)retObj;
                                               int count = 0;
                                               foreach (Object srcObj in objResult)
                                               {
                                                   //set the UI controls with the SP return list
                                                   GetUIValuesFromObject(ActionsEnum.SELECTEDINDEXCHANGED, srcObj);
                                                   count++;
                                               }
                                               if (count == 0)//If the sp return list is empty
                                               {
                                                   //Create a new instance of SP Complex type
                                                   string namespaceString = "ERPData";
                                                   string className = objResult.ElementType.Name;
                                                   className = namespaceString + "." + className;
                                                   Assembly currentAssembly = Assembly.Load(namespaceString);
                                                   Type baseEntity = currentAssembly.GetType(className);
                                                   Object entityObj = Activator.CreateInstance(baseEntity, null);
                                                   //set the UI controls with the SP Complextype object
                                                   GetUIValuesFromObject(ActionsEnum.SELECTEDINDEXCHANGED, entityObj);
                                               }
                                           }*/
                                    }
                                }
                            }
                            //Clear RDLC
                            divReportViewer.Visible = false;
                            divNodata.Visible = false;
                            // ClearCrystalReport();
                            break;
                        #endregion
                        #region AUTOCOMPLETESELECTED
                        case ActionsEnum.AUTOCOMPLETESELECTED:
                            break;
                        #endregion
                        #region REPORTCRITERIA
                        case ActionsEnum.REPORTCRITERIA:
                            AutoCompleteRelatedControlValue = null;
                            hdfShowTelDev.Value = "0";
                            if (!string.IsNullOrEmpty(hdfReport.Value) && Convert.ToInt32(hdfReport.Value) > 0)
                            {
                                Session[ERP.Utilities.SessionStrings.REPORTPK] = Convert.ToInt32(hdfReport.Value);
                            }
                            else
                            {
                                Session[ERP.Utilities.SessionStrings.REPORTPK] = null;
                            }
                            if (Session[ERP.Utilities.SessionStrings.REPORTPK] != null)
                            {
                                //Get dynamic controls
                                GetFieldValues(ControlsEnum.DEFAULT);
                                GetFieldValues(ControlsEnum.REPORT);
                                if (admReportCfgList != null)
                                {
                                    if (ReportType.TelerikReport != ((string[])admReportCfgList[0].RPT_OP_FILE.Split('.'))[1].Trim().ToLower())
                                    {
                                        if (Session[ERP.Utilities.SessionStrings.REPORTPK] != null && Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK].ToString()) > 0)
                                        {
                                            string reportType = string.Empty;
                                            if (Request.QueryString[ERP.Utilities.QueryStrings.ReportType] != null)
                                                reportType = Request.QueryString[ERP.Utilities.QueryStrings.ReportType].ToString();
                                            string redirectURL = Resources.PageURL.CommonRptURL + (string.IsNullOrEmpty(reportType) ? "" : ERP.Utilities.QueryStrings.ReportType + "=" + reportType)
                                                                + "&" + QueryStrings.RPT_PK + "=" +
                                                                Session[ERP.Utilities.SessionStrings.REPORTPK].ToString() +
                                                                "&Dep=" + currentUser.CurrentDeptPK.ToString();
                                            Response.Redirect(redirectURL,false);

                                            break;
                                        }
                                    }
                                }
                                SetFieldValues(ControlsEnum.REPORT);
                                //Load Controls to UI
                                LoadControls();
                                //rvViewReport.LocalReport.DataSources.Clear();
                                divReportViewer.Visible = false;
                                divNodata.Visible = false;
                                tblSearch.Visible = true;
                                hdfShowFilter.Value = "1";
                            }
                            else
                            {
                                pnlControls.Controls.Clear();
                                //rvViewReport.LocalReport.DataSources.Clear();
                                divReportViewer.Visible = false;
                                divNodata.Visible = false;
                                //RegisterInitComponents();
                            }
                            break;
                        #endregion
                        #endregion
                        #region EXCEL
                        case ActionsEnum.EXCEL:
                            GetFieldValues(ControlsEnum.REPORT);
                            if (admReportCfgList == null)
                            {
                                divReportViewer.Visible = false;
                                divNodata.Visible = false;
                                txtReport.Text = string.Empty;
                                hdfReport.Value = CommonConstants.SELECT_VALUE_ZERO;
                                tblSearch.Visible = false;

                            }
                            else
                            {
                                if (IsUserControl)
                                {
                                    Control myUsrControl = pnlControls.FindControl(UserControlID) as System.Web.UI.UserControl;
                                    customControl = myUsrControl as ERPSMS_v01.Reports.UserControls.IFilterControl;
                                    reportParams = customControl.GetReportParameters();
                                    FromDate = reportParams.FromDate;
                                    ToDate = reportParams.ToDate;
                                    if (admReportCfgList[0].RPT_CODE == "STK_BATCH_MNTHWISE_RPT" || admReportCfgList[0].RPT_CODE == "STK_CARD_GRP_RPT_MNTWISE_RPT" || admReportCfgList[0].RPT_CODE == "STK_CARD_BATCH_RPT_DATA_MNTWISE")
                                    {
                                        string Year = Convert.ToDateTime(FromDate).Year.ToString();
                                        string FinYear = reportParams.Parameters.Where(w => w.ParamName == "FIN_YEAR_TEXT").FirstOrDefault().Values[0].Value.ToString();
                                        if (FinYear != Year)
                                        {
                                            this.ReportViewer1.ReportSource = new Telerik.ReportViewer.Html5.WebForms.ReportSource();
                                            litErrorMsg.Text = Resources.Messages.SelectDateWithinRange;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            return;
                                        }
                                    }
                                }
                                else
                                    reportParams = SetUIValuesToXMLObject();

                                ActionsEnum reportAction;

                                ReportFile = admReportCfgList[0].RPT_OP_FILE;
                                var typeReportSource1 = new Telerik.ReportViewer.Html5.WebForms.ReportSource();
                                bool IsTrdp = true; //Make this false for Excel export
                                hdfShowFilter.Value = "0";
                                hdfShowTelDev.Value = "1";
                                string strFilter = string.Empty;
                                strFilter = reportParams.XmlSerialize();
    
                                    var reportPackager = new ReportPackager();
                                    Telerik.Reporting.Report rptPrint = null;
                                    using (var sourceStream = System.IO.File.OpenRead(Server.MapPath(admReportCfgList[0].RPT_OP_FILE)))
                                    {
                                        rptPrint = (Telerik.Reporting.Report)reportPackager.UnpackageDocument(sourceStream);
                                    }

                                    #region Parameters
                                    if (rptPrint.ReportParameters.Where(w => w.Name == "P_XML") != null &&
                                            rptPrint.ReportParameters.Where(w => w.Name == "P_XML").Any())
                                    {
                                        rptPrint.ReportParameters.Where(w => w.Name == "P_XML").First().Value = strFilter;
                                    }

                                    if (rptPrint.ReportParameters.Where(w => w.Name == "FromDate") != null &&
                                            rptPrint.ReportParameters.Where(w => w.Name == "FromDate").Any())
                                    {
                                        System.Web.UI.WebControls.TextBox txt_FromDate = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl("FromDate");
                                        if (txt_FromDate != null && !string.IsNullOrEmpty(txt_FromDate.Text.Trim()))
                                        {
                                            rptPrint.ReportParameters.Where(w => w.Name == "FromDate").First().Value = txt_FromDate.Text.Trim();
                                        }
                                        else
                                        {
                                            rptPrint.ReportParameters.Where(w => w.Name == "FromDate").First().Value = string.Empty;
                                        }
                                    }

                                    if (rptPrint.ReportParameters.Where(w => w.Name == "ToDate") != null &&
                                            rptPrint.ReportParameters.Where(w => w.Name == "ToDate").Any())
                                    {
                                        System.Web.UI.WebControls.TextBox txt_ToDate = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl("ToDate");
                                        if (txt_ToDate != null && !string.IsNullOrEmpty(txt_ToDate.Text.Trim()))
                                        {
                                            rptPrint.ReportParameters.Where(w => w.Name == "ToDate").First().Value = LogoPath;
                                        }
                                        else
                                        {
                                            rptPrint.ReportParameters.Where(w => w.Name == "ToDate").First().Value = string.Empty;
                                        }
                                    }
                                    if (rptPrint.ReportParameters.Where(w => w.Name == "Logo") != null &&
                                            rptPrint.ReportParameters.Where(w => w.Name == "Logo").Any())
                                    {
                                        rptPrint.ReportParameters.Where(w => w.Name == "Logo").First().Value = LogoPath;
                                    }
                                    #endregion

                                    ExportReport(rptPrint);
                                
                            }

                            break;
                        #endregion
                        #region default
                        default:
                            break;
                            #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                CommonBL.ExceptionWriting(ex.GetInnerExceptionMsg(), "Inner Exception");
                CommonBL.ExceptionWriting(ex.ToString(), "Telerik Report Viewer : " + ReportFile);
                if (ex.InnerException != null && ex.InnerException.StackTrace != null)
                {
                    CommonBL.ExceptionWriting(ex.InnerException.StackTrace, "Inner Stack Trace: ");
                }
                if (ex.StackTrace != null)
                {
                    CommonBL.ExceptionWriting(ex.StackTrace, "Stack Trace: ");
                }
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                ReportFile = string.Empty;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
                customerRegistrationServiceClient = null;
                commonServiceClient = null;
                commonService = null;
            }
        }
        private ReportDataSource GetCompanyDetails(int? CmpnyPk = null, int? sbuPK = null)
        {
            int companyPK = Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "CompanyPkForMIS").ToString());
            int companyIndex = -1;
            ReportDataSource CompanyDtls = null;
            currentEntity = new ERPEntities();
            List<SPADM_COMPANY_MST_GET_KV_Result> CompanyList = currentEntity.SPADM_COMPANY_MST_GET_KV(CmpnyPk, Convert.ToByte(DbActiveStatus.HASPK), null, sbuPK, null).ToList();
            foreach (SPADM_COMPANY_MST_GET_KV_Result result in CompanyList)
            {
                companyIndex++;
                if (result.CMP_PK == companyPK)
                    break;
            }
            if (CompanyList != null && CompanyList.Count > 0)
            {
                if (Convert.ToString(CompanyList[companyIndex].CMP_LOGO) != string.Empty)
                {
                    bool fileExists = false;

                    // parameters = new ReportParameter("ApprovedBySign", "file:///" + Server.MapPath(Resources.Controls.SignaturePath) + sa.AST_APPROVED_SIGN); 
                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                    {
                        if (File.Exists(Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[companyIndex].CMP_LOGO))
                        {
                            LogoPath = Server.MapPath(Resources.Controls.CompanyLogo) + CompanyList[companyIndex].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    else
                    {
                        if (File.Exists(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[companyIndex].CMP_LOGO))
                        {
                            LogoPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + CompanyList[companyIndex].CMP_LOGO;
                            fileExists = true;
                        }
                    }
                    if (!fileExists)
                    {
                        LogoPath = string.Empty;
                    }
                    CompanyList[companyIndex].CMP_LOGO = LogoPath;
                    CompanyName = CompanyList[companyIndex].CMP_NAME;
                }
            }
            // CompanyDtls = new ReportDataSource("CompanyDtls", CompanyList);
            return CompanyDtls;
        }
        private void SetReportParameters()
        { }
        void ActionHandler(object sender, TreeNodeEventArgs e)
        {
            ActionHandler(sender, EventArgs.Empty);
            /*TreeView tv = (TreeView)sender;

            foreach (TreeNode node in tv.CheckedNodes)
            {
                //node.
            }*/
        }
        private void ExcelExport()
        {
        }



        private void ExportDataSetToExcel(DataSet ds)
        {


            string UploadPath = string.Empty;
            try
            {
                string excelName = "RptExcel" + "_" + Guid.NewGuid().ToString() + GetLocalResourceObject("ExcelFormat").ToString();

                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                {
                    UploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                    if (!Directory.Exists(UploadPath))
                        Directory.CreateDirectory(UploadPath);
                    UploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                }
                else
                {
                    UploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();
                }

                string destination = UploadPath + excelName;
                using (var workbook = DocumentFormat.OpenXml.Packaging.SpreadsheetDocument.Create(destination, DocumentFormat.OpenXml.SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = workbook.AddWorkbookPart();
                    workbook.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();
                    workbook.WorkbookPart.Workbook.Sheets = new DocumentFormat.OpenXml.Spreadsheet.Sheets();

                    // Adding style
                    DocumentFormat.OpenXml.Packaging.WorkbookStylesPart stylePart = workbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorkbookStylesPart>();
                    stylePart.Stylesheet = GenerateStylesheet();
                    stylePart.Stylesheet.Save();

                    foreach (System.Data.DataTable table in ds.Tables)
                    {
                        var sheetPart = workbook.WorkbookPart.AddNewPart<DocumentFormat.OpenXml.Packaging.WorksheetPart>();
                        var sheetData = new DocumentFormat.OpenXml.Spreadsheet.SheetData();
                        sheetPart.Worksheet = new DocumentFormat.OpenXml.Spreadsheet.Worksheet(sheetData);
                        DocumentFormat.OpenXml.Spreadsheet.Sheets sheets = workbook.WorkbookPart.Workbook.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Sheets>();
                        string relationshipId = workbook.WorkbookPart.GetIdOfPart(sheetPart);
                        uint sheetId = 1;
                        if (sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Count() > 0)
                        {
                            sheetId =
                                sheets.Elements<DocumentFormat.OpenXml.Spreadsheet.Sheet>().Select(s => s.SheetId.Value).Max() + 1;
                        }
                        DocumentFormat.OpenXml.Spreadsheet.Sheet sheet = new DocumentFormat.OpenXml.Spreadsheet.Sheet() { Id = relationshipId, SheetId = sheetId, Name = table.TableName };
                        sheets.Append(sheet);
                        DocumentFormat.OpenXml.Spreadsheet.Row headerRow = new DocumentFormat.OpenXml.Spreadsheet.Row();

                        List<String> columns = new List<string>();
                        foreach (System.Data.DataColumn column in table.Columns)
                        {
                            columns.Add(column.ColumnName);
                            DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                            cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                            cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(column.ColumnName);
                            cell.StyleIndex = 2;
                            headerRow.AppendChild(cell);
                        }
                        sheetData.AppendChild(headerRow);
                        foreach (System.Data.DataRow dsrow in table.Rows)
                        {
                            DocumentFormat.OpenXml.Spreadsheet.Row newRow = new DocumentFormat.OpenXml.Spreadsheet.Row();
                            int colindex = 0;
                            foreach (String col in columns)
                            {
                                DocumentFormat.OpenXml.Spreadsheet.Cell cell = new DocumentFormat.OpenXml.Spreadsheet.Cell();
                                if (table.Columns[colindex].DataType == Type.GetType("System.Decimal") || table.Columns[colindex].DataType == Type.GetType("System.Int32") || table.Columns[colindex].DataType == Type.GetType("System.Int64"))
                                {
                                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.Number;
                                    cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString());
                                    cell.StyleIndex = 1;
                                }
                                else if (table.Columns[colindex].DataType == Type.GetType("System.DateTime"))
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(dsrow[col]).Trim()))
                                    {
                                        DateTime dateTime = DateTime.Parse(dsrow[col].ToString());
                                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dateTime.ToString(GetLocalResourceObject("GST_DateFormat").ToString()));
                                    }
                                    cell.DataType = new DocumentFormat.OpenXml.EnumValue<DocumentFormat.OpenXml.Spreadsheet.CellValues>(DocumentFormat.OpenXml.Spreadsheet.CellValues.String);
                                    cell.StyleIndex = 1;
                                }
                                else
                                {
                                    cell.DataType = DocumentFormat.OpenXml.Spreadsheet.CellValues.String;
                                    if (dsrow[col].ToString().StartsWith("="))//For identifying formula
                                        cell.CellFormula = new DocumentFormat.OpenXml.Spreadsheet.CellFormula(dsrow[col].ToString());
                                    else
                                        cell.CellValue = new DocumentFormat.OpenXml.Spreadsheet.CellValue(dsrow[col].ToString());
                                    cell.StyleIndex = 1;
                                }


                                newRow.AppendChild(cell);
                                colindex++;
                            }
                            sheetData.AppendChild(newRow);
                        }
                    }
                }

                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + excelName);
                Response.BinaryWrite(File.ReadAllBytes(destination));
                File.Delete(destination);
                Response.End();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorNew", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(ex.Message) + "','" + Resources.Captions.Information + "');", true);
            }

        }
        private DocumentFormat.OpenXml.Spreadsheet.Stylesheet GenerateStylesheet()
        {
            DocumentFormat.OpenXml.Spreadsheet.Stylesheet styleSheet = null;

            DocumentFormat.OpenXml.Spreadsheet.Fonts fonts = new DocumentFormat.OpenXml.Spreadsheet.Fonts(
                new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 0 - default
                    new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 }

                ),
                new DocumentFormat.OpenXml.Spreadsheet.Font( // Index 1 - header
                    new DocumentFormat.OpenXml.Spreadsheet.FontSize() { Val = 11 },
                    new DocumentFormat.OpenXml.Spreadsheet.Bold()//,
                                                                 // new Color() { Rgb = "FFFFFF" }

                ));

            DocumentFormat.OpenXml.Spreadsheet.Fills fills = new DocumentFormat.OpenXml.Spreadsheet.Fills(
                    new DocumentFormat.OpenXml.Spreadsheet.Fill(new DocumentFormat.OpenXml.Spreadsheet.PatternFill() { PatternType = DocumentFormat.OpenXml.Spreadsheet.PatternValues.None }), // Index 0 - default
                    new DocumentFormat.OpenXml.Spreadsheet.Fill(new DocumentFormat.OpenXml.Spreadsheet.PatternFill() { PatternType = DocumentFormat.OpenXml.Spreadsheet.PatternValues.Gray125 }), // Index 1 - default
                    new DocumentFormat.OpenXml.Spreadsheet.Fill(new DocumentFormat.OpenXml.Spreadsheet.PatternFill(new DocumentFormat.OpenXml.Spreadsheet.ForegroundColor { Rgb = new DocumentFormat.OpenXml.HexBinaryValue() { Value = "FFC14E" } }) { PatternType = DocumentFormat.OpenXml.Spreadsheet.PatternValues.Solid }) // Index 2 - header
                );

            DocumentFormat.OpenXml.Spreadsheet.Borders borders = new DocumentFormat.OpenXml.Spreadsheet.Borders(
                    new DocumentFormat.OpenXml.Spreadsheet.Border(), // index 0 default
                    new DocumentFormat.OpenXml.Spreadsheet.Border( // index 1 black border
                        new DocumentFormat.OpenXml.Spreadsheet.LeftBorder(new DocumentFormat.OpenXml.Spreadsheet.Color() { Auto = true }) { Style = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin },
                        new DocumentFormat.OpenXml.Spreadsheet.RightBorder(new DocumentFormat.OpenXml.Spreadsheet.Color() { Auto = true }) { Style = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin },
                        new DocumentFormat.OpenXml.Spreadsheet.TopBorder(new DocumentFormat.OpenXml.Spreadsheet.Color() { Auto = true }) { Style = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin },
                        new DocumentFormat.OpenXml.Spreadsheet.BottomBorder(new DocumentFormat.OpenXml.Spreadsheet.Color() { Auto = true }) { Style = DocumentFormat.OpenXml.Spreadsheet.BorderStyleValues.Thin },
                        new DocumentFormat.OpenXml.Spreadsheet.DiagonalBorder())
                );
            DocumentFormat.OpenXml.Spreadsheet.Alignment alignment = new DocumentFormat.OpenXml.Spreadsheet.Alignment();
            alignment.Horizontal = DocumentFormat.OpenXml.Spreadsheet.HorizontalAlignmentValues.Center;
            alignment.Vertical = DocumentFormat.OpenXml.Spreadsheet.VerticalAlignmentValues.Center;

            DocumentFormat.OpenXml.Spreadsheet.CellFormats cellFormats = new DocumentFormat.OpenXml.Spreadsheet.CellFormats(
                    new DocumentFormat.OpenXml.Spreadsheet.CellFormat(), //style index =0 default
                    new DocumentFormat.OpenXml.Spreadsheet.CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, //style index =1 body
                    new DocumentFormat.OpenXml.Spreadsheet.CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyBorder = true, ApplyFill = true, ApplyAlignment = true, Alignment = alignment }, // style index =2 header
                    new DocumentFormat.OpenXml.Spreadsheet.CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true, FormatId = 1, } //style index =3 body
                );

            styleSheet = new DocumentFormat.OpenXml.Spreadsheet.Stylesheet(fonts, fills, borders, cellFormats);
            return styleSheet;
        }
        private ReportParameters SetUIValuesToXMLObject()
        {
            ReportParameterName prms;
            ReportParameters tempReportParams = new GTIService.Dashboard.ReportParameters();
            tempReportParams.BizUnit = currentUser.SBUID;
            tempReportParams.Dept = currentUser.CurrentDeptPK;
            tempReportParams.UserPK = currentUser.PKUser;
            tempReportParams.RptPK = Session[ERP.Utilities.SessionStrings.REPORTPK] == null ? -1 : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.REPORTPK]);
            tempReportParams.Currency = currentUser.BaseCurrency;
            tempReportParams.Parameters = new List<GTIService.Dashboard.ReportParameterName>();
            foreach (var FieldControls in spAdmReportControlCfgGetResultList)
            {
                prms = new GTIService.Dashboard.ReportParameterName();
                prms.Values = new List<ReportParameterValues>();
                switch ((ControlTypes)(Enum.Parse(typeof(ControlTypes), FieldControls.RCC_CONTROL_TEXT)))
                {

                    #region Text
                    case ControlTypes.Text:
                        System.Web.UI.WebControls.TextBox txtCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (txtCtrlId != null && !string.IsNullOrEmpty(txtCtrlId.Text.Trim()))
                        {
                            prms.ParamName = txtCtrlId.ID;
                            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = txtCtrlId.Text.Trim()
                            });
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                    #endregion
                    #region TimePicker
                    case ControlTypes.TimePicker:
                        System.Web.UI.WebControls.TextBox txtTime = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (txtTime != null && !string.IsNullOrEmpty(txtTime.Text.Trim()))
                        {
                            prms.ParamName = txtTime.ID;
                            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = txtTime.Text.Trim()
                            });
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                    #endregion
                    #region Numeric
                    case ControlTypes.Numeric:
                        System.Web.UI.WebControls.TextBox txtNumCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (txtNumCtrlId != null && !string.IsNullOrEmpty(txtNumCtrlId.Text.Trim()))
                        {
                            prms.ParamName = txtNumCtrlId.ID;
                            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = txtNumCtrlId.Text.Trim()
                            });
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                    #endregion
                    #region DropDown
                    case ControlTypes.DropDown:
                        DropDownList ddlCtrlId = (DropDownList)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (ddlCtrlId != null && Convert.ToInt32(ddlCtrlId.SelectedValue) > 0)
                        {
                            prms.ParamName = ddlCtrlId.ID;
                            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = ddlCtrlId.SelectedValue
                            });
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                    #endregion
                    #region DateRange
                    case ControlTypes.DateRange:
                        System.Web.UI.WebControls.TextBox dateRangeCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        string dateTime = string.IsNullOrEmpty(dateRangeCtrlId.Text) ? string.Empty : dateRangeCtrlId.Text.Trim();
                        if (dateTime != string.Empty)
                        {
                            if (dateRangeCtrlId.ID.Equals("FromDate"))
                            {
                                tempReportParams.FromDate = dateTime;
                            }
                            else if (dateRangeCtrlId.ID.Equals("ToDate"))
                            {
                                tempReportParams.ToDate = dateTime;
                            }
                            else
                            {
                                prms.ParamName = dateRangeCtrlId.ID;
                                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = Convert.ToDateTime(dateTime).ToString()
                                });
                                tempReportParams.Parameters.Add(prms);
                            }
                        }
                        break;
                    #endregion
                    #region Date
                    case ControlTypes.Date:
                        System.Web.UI.WebControls.TextBox dateCtrlId = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        string date = string.IsNullOrEmpty(dateCtrlId.Text) ? string.Empty : dateCtrlId.Text.Trim();
                        if (date != string.Empty)
                        {
                            if (dateCtrlId.ID.Equals("FromDate"))
                            {
                                tempReportParams.FromDate = date;
                            }
                            else if (dateCtrlId.ID.Equals("ToDate"))
                            {
                                tempReportParams.ToDate = date;
                            }
                            else
                            {
                                prms.ParamName = dateCtrlId.ID;
                                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = Convert.ToDateTime(date).ToString()
                                });
                                tempReportParams.Parameters.Add(prms);
                            }
                        }
                        break;
                    #endregion
                    #region HiddenField
                    case ControlTypes.HiddenField:
                        HiddenField hdfCtrlId = (HiddenField)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (hdfCtrlId != null && !string.IsNullOrEmpty(hdfCtrlId.Value))
                        {
                            prms.ParamName = hdfCtrlId.ID;
                            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = hdfCtrlId.Value
                            });
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                    #endregion
                    #region CheckBox
                    case ControlTypes.CheckBox:
                        System.Web.UI.WebControls.CheckBox chkCtrlId = (System.Web.UI.WebControls.CheckBox)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (chkCtrlId != null)
                        {
                            prms.ParamName = chkCtrlId.ID;
                            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = chkCtrlId.Checked ? "1" : "0"
                            });
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                    #endregion
                    #region TreeView
                    case ControlTypes.TreeView:
                        TreeView TrvCtrlId = (TreeView)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (TrvCtrlId != null)
                        {
                            prms.ParamName = TrvCtrlId.ID;
                            foreach (TreeNode root in TrvCtrlId.Nodes)
                            {
                                foreach (TreeNode child in root.ChildNodes)
                                {
                                    if (child.Checked)
                                    {
                                        prms.Values.Add(
                                           new ReportParameterValues()
                                           {
                                               Value = child.Value
                                           });
                                    }
                                }
                            }
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;

                    #endregion
                    #region Auto CheckList
                    case ControlTypes.AutoCheckList:
                        CheckListSearchControl AutoCheckListId = (CheckListSearchControl)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (AutoCheckListId != null)
                        {
                            prms.ParamName = AutoCheckListId.ID;
                            //List<ListItem> GetCheckedItems()
                            foreach (ListItem item in AutoCheckListId.GetCheckedItems())
                            {
                                prms.Values.Add(
                                           new ReportParameterValues()
                                           {
                                               Value = item.Value
                                           });
                            }
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                        break;
                    #endregion
                    #region Multi Level Tree
                    case ControlTypes.MultiLevelTree:
                        TreeView MultiTrvCtrlId = (TreeView)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        if (MultiTrvCtrlId != null)
                        {
                            prms.ParamName = MultiTrvCtrlId.ID;
                            foreach (TreeNode root in MultiTrvCtrlId.Nodes)
                            {
                                foreach (TreeNode child in root.ChildNodes)
                                {
                                    if (child.Checked)
                                    {
                                        prms.Values.Add(
                                           new ReportParameterValues()
                                           {
                                               Value = child.Value
                                           });
                                    }
                                }
                            }
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                    #endregion
                    #region MonthPicker
                    case ControlTypes.MonthPicker:
                        System.Web.UI.WebControls.TextBox txtMonthPicker = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(FieldControls.RCC_CONTROL_ID);
                        string dateMP = string.IsNullOrEmpty(txtMonthPicker.Text) ? string.Empty : txtMonthPicker.Text.Trim();
                        if (dateMP != string.Empty)
                        {
                            if (txtMonthPicker.ID.Equals("FromDate"))
                            {
                                tempReportParams.FromDate = "01-" + dateMP;
                            }
                            else if (txtMonthPicker.ID.Equals("ToDate") || txtMonthPicker.ID.Equals("ForDate"))
                            {
                                tempReportParams.ToDate = "01-" + dateMP;
                                int totaldays = DateTime.DaysInMonth((Convert.ToDateTime(tempReportParams.ToDate)).Year, (Convert.ToDateTime(tempReportParams.ToDate)).Month);
                                tempReportParams.ToDate = totaldays.ToString() + "-" + dateMP;
                            }
                            else
                            {
                                prms.ParamName = txtMonthPicker.ID;
                                prms.Values.Add(
                                new ReportParameterValues()
                                {
                                    Value = "01-" + dateMP
                                });
                                tempReportParams.Parameters.Add(prms);
                            }
                        }
                        break;
                    #endregion
                    #region AutoComplete
                    case ControlTypes.AutoComplete:
                        string[] autoCompleteIDArray = FieldControls.RCC_CONTROL_ID.Split('|');
                        HiddenField autoCtrlId = (HiddenField)pnlControls.FindControl(string.Format("hdfAuto{0}", autoCompleteIDArray[1]));
                        System.Web.UI.WebControls.TextBox autoTxt = (System.Web.UI.WebControls.TextBox)pnlControls.FindControl(string.Format("txt{0}", autoCompleteIDArray[0]));
                        if (autoCtrlId != null && !string.IsNullOrEmpty(autoCtrlId.Value.Trim()) && autoCtrlId.Value.Trim() != "0" &&
                            (autoTxt.Text.Trim() != string.Empty && autoTxt.Text != Resources.ErpRes.AutoDefaultValue))
                        {
                            prms.ParamName = autoCompleteIDArray[0];
                            prms.Values.Add(
                            new ReportParameterValues()
                            {
                                Value = autoCtrlId.Value.Trim()
                            });
                            tempReportParams.Parameters.Add(prms);
                        }
                        break;
                        #endregion

                }
            }
            return tempReportParams;
        }

        /// <summary>
        /// Is any table contain records
        /// </summary>
        /// <param name="dsData"></param>
        /// <returns></returns>
        private bool IsExistDSRecord(DataSet dsData)
        {
            bool IsExist = false;
            foreach (DataTable dtRptData in dsData.Tables)
                if (dtRptData.Rows.Count > 0)
                {
                    IsExist = true;
                    break;
                }
            return IsExist;
        }



        #region --- For Grid Actions----
        /// <summary>
        /// Handling Grid events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (e.Row.Cells[0].Text.Trim().Equals(string.Empty) || e.Row.Cells[0].Text.Trim().Equals("&nbsp;"))//Check radio button column or not
                    {
                        RadioButton rbtSelect = new RadioButton()
                        {
                            ID = "rbtSelect" + e.Row.RowIndex,
                            GroupName = "SelectOne",
                            CssClass = "rdoSelection"
                        };
                        if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE] != null)
                        {
                            if (Session[ERP.Utilities.SessionStrings.DYNAMICTAB_SELECTED_CODE].ToString().Equals(TabType.CLST))//if the Master list tab
                            {
                                //set the selected pk to the hidden field
                                rbtSelect.Attributes.Add("onClick", "javascript:ShowSelectedRow(this);");
                            }
                            else
                            {
                                rbtSelect.Attributes.Add("onClick", "GrandScriptUtils.EnableRbtnGrouping(this)");
                            }
                        }
                        else
                        {
                            rbtSelect.Attributes.Add("onClick", "GrandScriptUtils.EnableRbtnGrouping(this)");
                        }
                        e.Row.Cells[0].Controls.Add(rbtSelect);
                        //set the pk of the record to a hidden field and add it to the grid
                        if (!string.IsNullOrEmpty(e.Row.Cells[1].Text))
                        {
                            HiddenField hdfPK = new HiddenField()
                            {
                                ID = "hdfPK" + e.Row.RowIndex,
                                ClientIDMode = ClientIDMode.Static,
                                Value = e.Row.Cells[1].Text
                            };
                            e.Row.Cells[0].Controls.Add(hdfPK);
                        }
                    }
                    //check hidden column list and hide the curresponding column of the grid
                    if (Session["HiddenColumnList"] != null)
                    {
                        List<int> hiddenColumnList = (List<int>)Session["HiddenColumnList"];
                        foreach (int columnIndex in hiddenColumnList)
                        {
                            e.Row.Cells[columnIndex].Visible = false;
                        }
                    }
                }
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    //Set the Header Text
                    if (admFormTabControlDtlList != null)
                    {
                        for (int colIndex = 0; colIndex < e.Row.Cells.Count; colIndex++)
                        {
                            string hdrText = (e.Row.Cells[colIndex] as DataControlFieldHeaderCell).ContainingField.HeaderText;
                            ADM_FORM_TAB_CONTROL_DTL admFormTabControlDtlObj = admFormTabControlDtlList.AsEnumerable().SingleOrDefault(xx => xx.ACD_CONTROL_ID == hdrText);
                            if (admFormTabControlDtlObj != null)
                            {
                                e.Row.Cells[colIndex].Text = admFormTabControlDtlObj.ACD_NAME;
                            }
                        }
                    }
                    //Hide the columns
                    if (Session["HiddenColumnList"] != null)
                    {
                        List<int> hiddenColumnList = (List<int>)Session["HiddenColumnList"];
                        foreach (int columnIndex in hiddenColumnList)
                        {
                            e.Row.Cells[columnIndex].Visible = false;
                        }
                    }

                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Page Index Handler for grdCurrency
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            //GetFieldValues(ControlsEnum.DEFAULT);
            //SetFieldValues(ControlsEnum.DEFAULT);
        }

        #endregion
        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            #region For Crystal report
            //If any report document exist, need to dispose the object
            if (!IsPostBack)
            {
                Session[ERP.Utilities.SessionStrings.CRReportData] = null;
            }

        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //base.CheckBtnVisibility(sender);
        }
        /// <summary>
        /// Method Used to initialize the Pager Control
        /// </summary>
        private void InitializeComponent()
        {
            this.Init += new EventHandler(this.Page_Init);
        }
        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeComponent();

            //Session[ERP.Utilities.SessionStrings.REPORTPK] = 2;
            if (Session[ERP.Utilities.SessionStrings.REPORTPK] != null)
            {
                //Get dynamic controls
                GetFieldValues(ControlsEnum.DEFAULT);
                GetFieldValues(ControlsEnum.REPORT);
                //Load Controls to UI
                LoadControls();

            }
            else
            {
                RegisterInitComponents();

            }

        }
        private void RegisterInitComponents()
        {
            string disableReportGrp = string.Empty; ;
            if (Request.QueryString[ERP.Utilities.QueryStrings.ReportType] != null)
            {
                Int32.TryParse(Request.QueryString[ERP.Utilities.QueryStrings.ReportType], out id);
            }
            if (id > 0)
            {
                GetFieldValues(ControlsEnum.REPORTGROUP);
                SetFieldValues(ControlsEnum.REPORTGROUP);
                disableReportGrp = "DisableAuto($('[id$=txtReportGroup]'), $('[id$=hdfReportGroup]'));";

            }

            PageScript = " function InitComponents(flag) {";//starting of initcomponent script
            //For Autocomplete 
            //string virtualPath = getVirtualPathForAutocomplete();

            //PageScript = virtualPath + PageScript;
            string reportGroupAutoComplete = "GrandScriptUtils.MakeAutoCompleteDDL('txtReportGroup', url, 'hdfReportGroup', true, true, 'REPORTGROUP');";

            string reportNameAutoComplete = string.Empty;
            if (id > 0)
            {
                reportNameAutoComplete = "if ($('[id$=hdfReportGroup]').val() != '' && $('[id$=hdfReportGroup]').val() != '0') {" +
                     "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url + '&Type=' + $('[id$=hdfReportGroup]').val(), 'hdfReport', true, true, 'REPORT');}" +
                     "else {" +
                     "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url, 'hdfReport', true, true, 'REPORT');}";
            }
            else
            {
                reportNameAutoComplete = "if ($('[id$=hdfReportGroup]').val() != '' && $('[id$=hdfReportGroup]').val() != '0') {" +
                                   "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url + '?Type=' + $('[id$=hdfReportGroup]').val(), 'hdfReport', true, true, 'REPORT');}" +
                                   "else {" +
                                   "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url, 'hdfReport', true, true, 'REPORT');}";
            }


            // reportGroupAutoComplete = string.Empty;
            //string reportNameAutoComplete = "GrandScriptUtils.MakeAutoCompleteDDL('txtReport', url, 'hdfReport', true, true, 'REPORT');";
            PageScript = PageScript + reportGroupAutoComplete + reportNameAutoComplete + disableReportGrp;
            PageScript = PageScript + "}";// +
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "pagescript1", PageScript, true);
            //ltrScriptContent.Text = "<script type=\"text/javascript\">" + PageScript + "</script>";
            //"$(document).ready(function () {" +
            //"InitComponents(true);  });";

            //Register page script
            //ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "pagescript", PageScript, true);            
        }
        protected void Page_PreRenderComplete(Object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
        }
        /// <summary>
        /// To Handle Page_PreRender
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                //ltrScriptContent.Text = "<script type=\"text/javascript\">" + PageScript + "</script>";
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                }
                //else if (EntryStatus == EntryStatus.SAVEONLY)
                //{
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(3);});", true);
                //}
                if (AutoInitComponents)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();AutoInitComponents();});", true);
                    AutoInitComponents = false;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializepopupComponents", "$(document).ready(function(){InitPopupComponents();});", true);
                //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "AutoCmplt2", autuRelatedControlScript[0], true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #endregion
        private void Test2()
        {
            var typeReportSource1 = new Telerik.ReportViewer.Html5.WebForms.ReportSource();
            typeReportSource1.Identifier = "CompoundDetails.trdp";
            typeReportSource1.IdentifierType = IdentifierType.TypeReportSource;
            typeReportSource1.Parameters.Add("P_XML", "<FilterParameters><FromDate>9-Mar-2021</FromDate><ToDate>9-Mar-2021</ToDate><BizUnit>1</BizUnit><Dept>8</Dept><UserPK>1</UserPK><RptPK>2026</RptPK><Currency>133</Currency><Parameter><ParamName>ITM_PK</ParamName></Parameter></FilterParameters>");
            this.ReportViewer1.ReportSource = typeReportSource1;

        }

        void ExportReport(Telerik.Reporting.Report reportToExport)
        {
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
            instanceReportSource.ReportDocument = reportToExport;
            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

            Telerik.Reporting.Processing.RenderingResult result = reportProcessor.RenderReport("XLSX", instanceReportSource, null);

            string fileName = result.DocumentName + "." + result.Extension;

            Response.Clear();
            Response.ContentType = result.MimeType;
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Expires = -1;
            Response.Buffer = true;

            Response.AddHeader("Content-Disposition",
                               string.Format("{0};FileName=\"{1}\"",
                                             "attachment",
                                             fileName));

            Response.BinaryWrite(result.DocumentBytes);
            //Response.End();
            HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
            HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
            HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event.
        }

        #region Enum
        /// <summary>
        /// Define Controltype Enum
        /// </summary>
        enum ControlTypes
        {
            Page,
            Label,
            Text,
            DropDown,
            DateTime,
            Numeric,
            Button,
            Spacer,
            GridView,
            CheckBox,
            TextArea,
            TimePicker,
            Header,
            Table,
            Iframe,
            HiddenField,
            HourText,
            FileUpload,
            Date,
            LinkButton,
            ImageButton,
            ValidationSummary,
            DateRange,
            TreeView,
            MonthPicker,
            AutoComplete,
            MultiLevelTree,
            AutoCheckList,
            UserControl
        }
        /// <summary>
        /// Define Common Enum
        /// </summary>
        public enum ControlsEnum
        {
            DEFAULT,
            DYNAMICTABS,
            CUSTOMER,
            REPORT,
            REPORTGROUP,
            REPORTBYPK,
            EXCEL,
            HTMLREPORT
        }
        public enum ReportCode
        {
            AP_VCH_LST_SUM,
            AP_VCH_LST_DET,
            PU_VCH_LST_SUM,
            PU_VCH_LST_DET,
            AR_VCH_LST_SUM,
            AR_VCH_LST_DET,
            SU_VCH_LST_SUM,
            SU_VCH_LST_DET,
            JV_VCH_LST_SUM,
            JV_VCH_LST_DET,
            AP_LDR_LST_SUM,
            AP_LDR_LST_SET,
            AR_LDR_LST_SUM,
            AR_LDR_LST_SET,
            CREITOR_LST,
            DEBTOR_LST,
            INVOICE_LST,
            DEBTOR_AGE_LST,
            CREITOR_AGELST,
            SI_VCH_LST_SUM,
            SI_VCH_LST_DET,
            EI_VCH_LST_SUM,
            EI_VCH_LST_DET,
            APRVD_VND_LST,
            MIN_STK_RPT,
            WC_RPT,
            VND_REG,
            STK_CRY_FWD_RPT,
            STK_HST_CARD,
            INV_YM,
            CCL_ISSUE,
            CMP_RPT_YM,
            RT_MAT_CMP,
            STK_CARD_RPT,
            STK_CARD_GRP_RPT,
            HISTORY_VND_RPT,
            PUR_RPT_YM,
            CMP_TOT_PRICE_RPT,
            PAYMNT_CNTRL_RPT,
            OUT_DUE_RPT,
            PUR_ST_PACK_MST,
            CUS_ADV_LST,
            STK_EXT_ISS_RPT,
            STT_AR,
            FIN_AP_MVMT,
            FIN_AP_MVMT_2,
            FIN_AR_MVMT,
            BL_RPT,
            CHK_PAID_LST,
            CMP_TOT_QTY_RPT,
            INV_STT_TAX_IN,
            SHPING_PLAN,
            SHIPMENT_WK_RPT,
            SHIPMENT_MONTH_RPT,
            P_L_M_T_Y,
            VEN_EVAL_PERFORM,
            VAT_AD_TAX_0,
            VAT_AD_TAX_7,
            SAL_DEL_PLAN,
            VAT_AD_TAX_7BUY,
            CNT_EVAL_RPT,
            MNT_SALE_RPT,
            VEN_EVAL_RPT,
            AP_OBVCH_LST_SUM,
            AP_OBVCH_LST_DET,
            PROFIT_RPT,
            SO_STAUS_QTY,
            SO_STAUS_AMT,
            PO_STATUS_QTY,
            PO_STATUS_AMT,
            PAY_CAL_RPT,
            REC_CAL_RPT,
            SO_PLN_RPT,
            MNT_SALE_SP_RPT,
            CHK_RCVD_LST,
            VCH_LST,
            VEN_ITM_SAMPLE,
            SALE_FORECAST,
            INV_AGEING,
            FC_HOLD_LIST,
            VEN_ADV_LST,
            VAT_RET,
            ITEM_LIST,
            VND_ITEM_LIST,
            TENTATIVE_LATEX,
            VND_INV_LST,
            PUR_ORD_EXPCTD_DEL,
            RPT_PND3,
            RPT_PND3_DTL,
            RPT_PND2,
            RPT_PND2_DTL,
            RPT_PND53,
            RPT_PND53_DTL,
            VAT_RET_THAI,
            VAT_RET_THAI_AT,
            EIPJ_VCH_LST_SUM,
            MSIJ_VCH_LST_SUM,
            PCBJ_VCH_LST_SUM,
            RCBJ_VCH_LST_SUM,
            DNJ_VCH_LST_SUM,
            PCVJ_VCH_LST_SUM,
            DPVJ_VCH_LST_SUM,
            MSIRJ_VCH_LST_SUM,
            PDCCJ_VCH_LST_SUM,
            PPCCJ_VCH_LST_SUM,
            FCHRJ_VCH_LST_SUM,
            SIPJ_VCH_LST_SUM,
            CNJ_VCH_LST_SUM,
            EIPJ_VCH_LST_DET,
            MSIJ_VCH_LST_DET,
            PCBJ_VCH_LST_DET,
            RCBJ_VCH_LST_DET,
            DNJ_VCH_LST_DET,
            PCVJ_VCH_LST_DET,
            DPVJ_VCH_LST_DET,
            MSIRJ_VCH_LST_DET,
            PDCCJ_VCH_LST_DET,
            PPCCJ_VCH_LST_DET,
            FCHRJ_VCH_LST_DET,
            SIPJ_VCH_LST_DET,
            CNJ_VCH_LST_DET,
            PO_SO_DTL_LIST,
            PR_SO_DTL_LIST,
            TRX_AUDIT_LOG,
            SO_BOOKED_RPT,
            TEST_ENTRY,
            STK_VAL_RPT,
            STK_RPT,
            DLY_STK_RPT,
            STR_STK_STM_D,
            STR_STK_STM_M,
            PUR_ORD_REG,
            GRN_RPT,
            MIN_RPT,
            STK_LDGR_RPT,
            STK_MVT_RPT,
            STR_STK_STM_V,
            SHPING_PLAN_SM,
            STR_STK_STM_V_WA,
            STK_LDGR_RPT_WA,
            VND_AC_STAT,
            WHT_SPEC_ACC_TAX,
            RPT_PND54,
            SC_WISE_RECEIPT,
            ITEM_RATE,
            VEN_WISE_RATE,
            CUS_BRAND_PFL,
            BR_RATE_CUS_WISE,
            DUTY_SLIP_DTL,
            PRD_PRE_AUDIT,
            DS_RPT_SM,
            DAILY_MCHN_RPT,
            PRD_PROC_PARAM,
            PRD_CHLOR_BC,
            PRD_BC_AT,
            PRD_BC_CHLOR,
            PRD_BC_GLIDE,
            PRD_BC_INSP,
            PRD_BC_LEACH,
            PRD_BC_MOULD,
            PRD_BC_PA,
            PRD_BC_PACK,
            PRD_BC_TOY,
            PRD_BC_TUMB,
            PRD_BC_WASH,
            PRD_BC_WT,
            PRD_BC_WTT,
            PRD_BC_PRODUCTION,
            SC_PROFIT_REPORT,
            PROFIT_RPT_FIN
        }
        #endregion
    }
}