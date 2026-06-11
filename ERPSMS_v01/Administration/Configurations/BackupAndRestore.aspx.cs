using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject;
using ERPSMS_v01.UserControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;


namespace ERPSMS_v01.Administration.Configurations
{
    public partial class BackupAndRestore : ERP.Store.UI.MyBasePage
    {
        #region Properties & Variables
        #region Properties

        /// <summary>
        /// Sql Server Connection builder
        /// </summary>
        public string BackUpDevice
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.BackUpDevice];
            }
            set
            {
                this.ViewState[ViewstateStrings.BackUpDevice] = value;
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
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.TotalPages];
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalPages] = value;
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
        /// To maintain the SortExpression or then By in viewstate
        /// </summary>
        private string ThenBy
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenBy];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenBy] = value;
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
        /// <summary>
        /// To maintain the Then Direction in viewstate
        /// </summary>
        private string ThenDirection
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.ThenDirection];
            }
            set
            {
                this.ViewState[ViewstateStrings.ThenDirection] = value;
            }
        }
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

        /// <summary>
        /// Currency Format String
        /// </summary>
        private string CurrencyFormatString
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] == null ?
                    String.Format("{{0:n{0}}}", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)
                    : (string)ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrencyFormatString] = value;
            }
        }
        #endregion

        #region Variables
        User currentUser;
        private ActionsEnum commonActions;
        List<BackupFileInfo> backupFileInfoList;
        private SqlConnectionStringBuilder ConnectionStringBuilder;
        private SqlConnectionStringBuilder RestoreConnectionStringBuilder;
        string backUpFileName = string.Empty;
        BackupFileInfo selectedBackupInfo = null;
        #endregion
        #endregion

        #region Page Events
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
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
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Grid Events & Helper Methods
        /// <summary>
        /// Action Handlers For Pager Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, DataNavigatorEventArgs e)
        {

        }
        /// <summary>
        /// Page Index Handler for grd
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex.ToString();
            EntryStatus = EntryStatus.LISTMODE;
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
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;
                GetFieldValues(ControlsEnum.RESTORELIST);
                SetFieldValues(ControlsEnum.RESTORELIST);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            //uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            //uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            //uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }


        #endregion

        #region Common Methods

        #region PageActionHandler
        private void PageActionHandler()
        {
            try
            {
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                if (!IsPostBack)
                {
                    this.CurrencyFormatString = String.Format("{{0:n{0}}}", Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.RESTORELIST);
                    SetFieldValues(ControlsEnum.RESTORELIST);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region ActionHandler
        /// <summary>
        /// Button actions events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                short result;
                #region Getting Command Action
                commonActions = ActionsEnum.UNKNOWN;
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
                #endregion
                switch (commonActions)
                {
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.NEW);
                        break;
                    case ActionsEnum.LIST:
                        ResetForm(ControlsEnum.LIST);
                        break;
                    #region Backup
                    case ActionsEnum.BACKUP:

                        ConfigurationChecking();
                        SetUIValuesToObject(ControlsEnum.BACKUP);

                        //validate fileName
                        if (backUpFileName.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) != -1 || !backUpFileName.ToUpper().EndsWith(".BAK"))
                            throw new InvalidOperationException(GetLocalResourceObject("Err_Msg_InvalidBackupFilename").ToString());

                        result = SqlServerBackUpResoteUtilityManager.BackUp(this.ConnectionStringBuilder, this.BackUpDevice, backUpFileName);
                        if (result > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Msg_BackupComplete") + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_Msg_BackupFailed").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_BackupFailed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                        }
                        ResetForm(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region Restore
                    case ActionsEnum.RESTORE:

                        SetUIValuesToObject(ControlsEnum.RESTORE);
                        if (this.selectedBackupInfo != null)
                        {
                            ConfigurationChecking(); 
                            GetFieldValues(ControlsEnum.DEFAULT);
                            string restoreDbName = ConnectionStringBuilder.InitialCatalog;

                            //LiveCode
                            result = SqlServerBackUpResoteUtilityManager.Restore(this.RestoreConnectionStringBuilder, restoreDbName, this.selectedBackupInfo);
                            //LiveCode

                            //Test code
                            //selectedBackupInfo.BackupFileFullName = "E:\\System Administrator\\Biju\\DbBackup\\gERP_150217_Backup.bak";
                            //result = SqlServerBackUpResoteUtilityManager.Restore(this.RestoreConnectionStringBuilder, "gERP_150217", this.selectedBackupInfo);
                            //Test code

                            if (result > 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + GetLocalResourceObject("Msg_RestoreComplete") + "','" + Resources.Messages.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_Msg_RestoreFailed").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Msg_RestoreFailed").ToString()) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                        ResetForm(ControlsEnum.LIST);
                        break;
                    #endregion
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.LIST);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
                switch (type)
                {
                    case ControlsEnum.DEFAULT:
                        if (ConfigurationManager.AppSettings["BackUpDevice"] == null
                            || ConfigurationManager.AppSettings["BackUpDevice"].ToString().IsNullOrEmptyOrWhitespace())
                            throw new ApplicationException("BackUpDevice not found in AppSettings");                      

                        this.ConnectionStringBuilder = GetConnnectionStringBuilder("BackupConnectionString");
                        this.RestoreConnectionStringBuilder = GetConnnectionStringBuilder("RestoreConnectionString");
                        this.BackUpDevice = ConfigurationManager.AppSettings["BackUpDevice"];
                        break;
                    case ControlsEnum.RESTORELIST:
                        backupFileInfoList = SqlServerBackUpResoteUtilityManager.GetRestoreList(this.BackUpDevice);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
                    case ControlsEnum.DEFAULT:
                        GetUIValuesFromObject(ControlsEnum.DEFAULT);
                        break;
                    case ControlsEnum.RESTORELIST:
                        BindGrid(ControlsEnum.RESTORELIST);
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
                case ControlsEnum.NEW:
                    this.EntryStatus = EntryStatus.ENTRYMODE;
                    this.txtBackupFileName.Text = string.Empty;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    this.AssignBreadCrumb();
                    break;
                case ControlsEnum.LIST:
                    this.EntryStatus = BusinessObject.Common.EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.DEFAULT);
                    SetFieldValues(ControlsEnum.DEFAULT);
                    GetFieldValues(ControlsEnum.RESTORELIST);
                    SetFieldValues(ControlsEnum.RESTORELIST);
                    ClearGridSelection();
                    this.AssignBreadCrumb();
                    break;
            }
        }
        #endregion

        private void ClearGridSelection()
        {
            foreach (GridViewRow grdrow in grdBackupList.Rows)
            {
                RadioButton rbtn;
                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                if (rbtn.Checked)
                {
                    rbtn.Checked = false;
                    break;
                }
            }
        }

        #region GetUIValuesFromObject
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.DEFAULT:
                        if (this.ConnectionStringBuilder != null)
                        {
                            txtServerName.Text = this.ConnectionStringBuilder.DataSource;
                            txtDatabase.Text = this.ConnectionStringBuilder.InitialCatalog;
                            txtBackupFileName.Text = String.Format("{0}_{1}.bak",
                                                       ConnectionStringBuilder.InitialCatalog,
                                                       DateTime.Now.ToString("yyMMdd"));
                        }
                        else
                        {
                            txtServerName.Text = string.Empty;
                            txtDatabase.Text = string.Empty;
                            txtBackupFileName.Text = string.Empty;
                        }
                        txtBackUpTo.Text = this.BackUpDevice;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region SetUIEditView
        private void SetUIEditView(ActionsEnum Mode)
        {
            try
            {
                if (Mode == ActionsEnum.VIEW)
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
        #endregion


        private void ConfigurationChecking()
        {
            bool hasError = false;

            if (ConfigurationManager.AppSettings["BackUpDevice"] == null
               || ConfigurationManager.AppSettings["BackUpDevice"].ToString().IsNullOrEmptyOrWhitespace())
                hasError = true;

            if (hasError)
            {
                throw new ApplicationException("Invalid Configurations");
            }
        }

        /// <summary>
        /// This Methode is Used to Formating Currency fields in HTML 
        /// </summary>
        /// <returns></returns>
        public string GetCurrencyFormat()
        {
            return this.CurrencyFormatString;
        }

        private SqlConnectionStringBuilder GetConnnectionStringBuilder(string key)
        {
            if (ConfigurationManager.ConnectionStrings[key] == null) throw new ApplicationException("Backup ConnectionString not found");
            var connectionString = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            return new SqlConnectionStringBuilder(connectionString);
        }

        private void BindGrid(ControlsEnum controlsEnum)
        {
            switch (controlsEnum)
            {
                case ControlsEnum.RESTORELIST:
                    grdBackupList.DataSource = backupFileInfoList;
                    grdBackupList.DataBind();
                    break;
            }
        }

        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;

            try
            {
                switch (controlType)
                {
                    case ControlsEnum.BACKUP:
                        this.ConnectionStringBuilder = GetConnnectionStringBuilder("BackupConnectionString");
                        this.BackUpDevice = ConfigurationManager.AppSettings["BackUpDevice"];
                        this.backUpFileName = txtBackupFileName.Text.IsNullOrEmptyOrWhitespace()
                                                ? String.Format("{0}_{1}.bak", ConnectionStringBuilder.InitialCatalog, DateTime.Now.ToString("yyMMdd"))
                                                : txtBackupFileName.Text.Trim().ToUpper().EndsWith(".BAK")
                                                    ? txtBackupFileName.Text.Trim()
                                                    : txtBackupFileName.Text + ".bak";
                        break;
                    case ControlsEnum.RESTORE:
                        foreach (GridViewRow grdrow in grdBackupList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                this.selectedBackupInfo = new BackupFileInfo
                                {
                                    BackupFileFullName = ((HiddenField)grdrow.FindControl("hdfBackupFileFullName")).Value.Trim(),
                                    BackupFileName = ((Label)grdrow.FindControl("lblBackupFileName")).Text.Trim(),
                                    BackupFileSize = ((Label)grdrow.FindControl("lblBackupFileSize")).Text.Trim(),
                                    CreatedDate = Convert.ToDateTime(((Label)grdrow.FindControl("lblCreatedDate")).Text.Trim())
                                };
                                break;
                            }
                        }
                        break;
                    default:
                        break;
                }
                return retObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion

        public override void AssignBreadCrumb()
        {
            try
            {
                if (this.EntryStatus == BusinessObject.Common.EntryStatus.LISTMODE)
                {
                    base.AssignBreadCrumb("BreadcrumbRestore");
                }
                else
                {
                    base.AssignBreadCrumb("BreadcrumbBackup");
                }
            }
            catch (Exception ex)
            {

            }
        }

        #region Page Control Enum
        enum ControlsEnum
        {
            RESTORELIST,
            BACKUP,
            CANCEL,
            RESTORE,
            NEW,
            DEFAULT,
            LIST
        }
        #endregion
    }
}