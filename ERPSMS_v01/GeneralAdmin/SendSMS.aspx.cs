using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERPSMS_v01.UserControls;
using ERP.Utilities;
using System.Data;
using BusinessObject;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject.CommonManagement;
using BusinessObject.Administration.Masters;
using BusinessLogic.CommonManagement;
using System.Configuration;
using ERP.Utilities.HRMS;
using BusinessLogic.Administration.Masters;
using System.IO;
using System.Data.OleDb;
using System.Text;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class SendSMS : ERP.Store.UI.MyBasePage              //System.Web.UI.Page
    {
        #region Variables and Properties
        #region Variables
        private BusinessObject.User currentUser;
        private DataSet  dslist;
        private DataTable dtCompany;
        private DataTable dtEmployeeFilterList;
        private int CompanyPk = 0;
        private SendSMSBO.SendSMSHeader objSendSMSHeader;
        private string uploadPath;
        private StringBuilder sb;
        private string xmlLanding;
        private string[] excelColumns;
        private string[] airColums_General = { Resources.Constants.ExlSMSMobCoulmn, Resources.Constants.ExlSMSMsgCoulmn };
        private FileInfo attchInfo;
        private OleDbConnection connExcel;
        private OleDbCommand cmdExcel;
        private OleDbDataAdapter oleDbDataAdapter;
        private DataTable dtExcelSchema;
        private DataSet dsImportedAttendance;
        private string landingSheet;
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
        private List<SendSMSBO.SendSMSDetails> SendSMSDetailsList
        {
            get
            {
                return ViewState[ViewstateStrings.SendSMSDetails] == null ? new List<SendSMSBO.SendSMSDetails>() : (List<SendSMSBO.SendSMSDetails>)ViewState[ViewstateStrings.SendSMSDetails];
            }
            set
            {
                ViewState[ViewstateStrings.SendSMSDetails] = value;
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
           // base.CheckBtnVisibility(sender);
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
                lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch(1);", true);
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
                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    uclPaging.CurrentPage = 1;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
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

           // UCEmpSalary.IsEmpAppraisal = Convert.ToInt32(VisbleStatusEnum.TRUE);
            // hdfNoOfLeaveValidation.Value = GetGlobalResourceObject("ConfigurationsRes", "HrmsLeaveValidation").ToString();
        }
        #endregion
        #endregion

        #region ActionHandler
        protected void ActionHandler(object sender, EventArgs e)
        {
            try
            {
                int? result;
                result = 0;
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
                    #region LIST
                    case ActionsEnum.LIST:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = "1";
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region FILTER
                    case ActionsEnum.FILTER:
                        uclPaging.CurrentPage = 0;
                        this.PageIndex = "1";
                        this.EntryStatus = EntryStatus.LISTMODE;
                        this.CurrPK = 0;
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEAREMPFILTER);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.CLEAR);
                        SetUIEditView(commonActions);
                        txtMobNo.Focus();
                        break;
                    #endregion
                    #region SEND
                    case ActionsEnum.SEND:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objSendSMSHeader = new SendSMSBO.SendSMSHeader();
                            objSendSMSHeader = (SendSMSBO.SendSMSHeader)SetUIValuesToObject(ControlsEnum.SENDSMSHDR);
                            if (objSendSMSHeader != null)
                            {
                                if (objSendSMSHeader.SendSMSDts != null && objSendSMSHeader.SendSMSDts.Count > 0)
                                {
                                    // string TrxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<SendSMSBO.SendSMSHeader>(objSendSMSHeader);
                                    result = SendSMSBL.SaveSendSMSDetails(xmlDoc);
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Send_Success").ToString();
                                        object[] args = new object[1];
                                        args[0] = Resources.PageNameRes.SENDSMS;
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
                                            litErrorMsg.Text = Resources.PageNameRes.SENDSMS + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.SENDSMS + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SENDSMS);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);

                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsForSend").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region RESEND
                    case ActionsEnum.RESEND:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            objSendSMSHeader = new SendSMSBO.SendSMSHeader();
                            objSendSMSHeader = (SendSMSBO.SendSMSHeader)SetUIValuesToObject(ControlsEnum.EMPLOYEEDETAILSLISTRESEND);
                            if (objSendSMSHeader != null)
                            {
                                if (objSendSMSHeader.SendSMSDts != null && objSendSMSHeader.SendSMSDts.Count > 0)
                                {
                                    // string TrxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<SendSMSBO.SendSMSHeader>(objSendSMSHeader);
                                    result = SendSMSBL.SaveSendSMSDetails(xmlDoc);
                                    if (result > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_ReSend_Success").ToString();
                                        object[] args = new object[1];
                                        args[0] = Resources.PageNameRes.SENDSMS;
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
                                            litErrorMsg.Text = Resources.PageNameRes.SENDSMS + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.SENDSMS + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SENDSMS);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);

                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsForResend").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CLEAR SEARCH
                    case ActionsEnum.CLEARSEARCH:
                        ResetForm(ControlsEnum.ADDTOLIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupEmployeeDetails]','" + GetLocalResourceObject("EmployeeDetails").ToString() + "','820','540');", true);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        ResetForm(ControlsEnum.ADDTOLIST);
                        SetFieldValues(ControlsEnum.EMPLOYEEDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupEmployeeDetails]','" + GetLocalResourceObject("EmployeeDetails").ToString() + "','820','540');", true);
                        break;
                    #endregion
                    #region  CLOSE EMPLOYEE POPUP
                    case ActionsEnum.CANCELDPOPUP:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
                        break;
                    #endregion
                    #region  EMPLOYEE SEARCH
                    case ActionsEnum.DTLSEARCH:
                        GetFieldValues(ControlsEnum.EMPLOYEEDETAILS);
                        SetFieldValues(ControlsEnum.EMPLOYEEDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPopupEmployeeDetails]','" + GetLocalResourceObject("EmployeeDetails").ToString() + "','820','540');", true);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:

                        objSendSMSHeader = new SendSMSBO.SendSMSHeader();
                        objSendSMSHeader = (SendSMSBO.SendSMSHeader)SetUIValuesToObject(ControlsEnum.EMPLOYEEDETAILSLISTDELETE);
                        if (objSendSMSHeader != null)
                        {
                            if (objSendSMSHeader.SendSMSDts != null && objSendSMSHeader.SendSMSDts.Count > 0)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<SendSMSBO.SendSMSHeader>(objSendSMSHeader);
                                result = SendSMSBL.DeleteSMSDetails(xmlDoc);
                                if (result > 0)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_SMS_Delete").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SENDSMS);
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
                                        litErrorMsg.Text = Resources.PageNameRes.SENDSMS + " " + Resources.Messages.UsedInAnotherPlace;
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
                                        litErrorMsg.Text = Resources.PageNameRes.SENDSMS + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.ALREADYDELETED)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.SENDSMS + " " + Resources.Messages.AlreadyDeleted;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.SENDSMS);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                            + "','" + Resources.ErpRes.Information + "');", true);

                                    }
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsForDelete").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region  ADD MOB NO FROM POPUP
                    case ActionsEnum.ADD:
                        GetUIValuesFromObject(ControlsEnum.EMPLOYEEDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();", true);
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
                                string filePath = SaveDetails(out conStr, fupImport);
                                if (!string.IsNullOrEmpty(filePath))
                                {
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
                    #region COPY
                    case ActionsEnum.COPY:
                        int smqPK = int.Parse(((Button)sender).CommandArgument.ToString());
                         ResetForm(ControlsEnum.CLEAR);
                         foreach (GridViewRow grdrow in grdSMSList.Rows)
                         {
                             HiddenField hdfSMQ_PKList = (HiddenField)grdrow.FindControl("hdfSMQ_PKList");
                             Label lblMobNo = (Label)grdrow.FindControl("lblMobNo");
                             Label lblMessage = (Label)grdrow.FindControl("lblMessage");
                             if (smqPK == Convert.ToInt32(hdfSMQ_PKList.Value))
                             {
                                 txtMobNo.Text = Convert.ToString(lblMobNo.Text);
                                 txtMessage.Text = lblMessage.Text.HtmlDecode();
                             }
                         }
                        SetUIEditView(ActionsEnum.NEW);
                        txtMobNo.Focus();
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
                        objFilterParam.FromDate = string.IsNullOrEmpty(txtSrchFromDate.Text) ? (DateTime?)null : DateTime.Parse(txtSrchFromDate.Text);
                        objFilterParam.ToDate = string.IsNullOrEmpty(txtSrchToDate.Text) ? (DateTime?)null : DateTime.Parse(txtSrchToDate.Text);
                        objFilterParam.MobileNo = string.IsNullOrEmpty(txtFilterMobileNo.Text) ? null : (txtFilterMobileNo.Text);
                        objFilterParam.UserPK = currentUser.PKUser;
                        objFilterParam.BizUnit = currentUser.SBUID;
                        dslist = SendSMSBL.GetSendSMSList(objFilterParam);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(BusinessObject.CommonManagement.DbActiveStatus.ACTIVE), 0, CompanyPk);
                        break;
                    #endregion
                    #region EMPLOYEE OT DETAILS
                    case ControlsEnum.EMPLOYEEDETAILS:
                        objFilterParam = new FilterParameters();
                        objFilterParam.Company = GetNullableInt(ddlCompanyHd.SelectedValue) > 0 ? GetNullableInt(ddlCompanyHd.SelectedValue) : null;
                        objFilterParam.BranchLocation = GetNullableInt(hdfHdBranchLocation.Value) > 0 ? GetNullableInt(hdfHdBranchLocation.Value) : null;
                        objFilterParam.Department = GetNullableInt(hdfHdDepartment.Value) > 0 ? GetNullableInt(hdfHdDepartment.Value) : null;
                        objFilterParam.Employee = GetNullableInt(hdfEmployee.Value) > 0 ? GetNullableInt(hdfEmployee.Value) : null;
                        objFilterParam.EmployeeCategory = (int)EmployeeCategory.HRMSEmployee;
                        objFilterParam.Active = Convert.ToInt32(CommonConstants.ACTIVE);
                        objFilterParam.BizUnit = currentUser.SBUID;
                        dtEmployeeFilterList = SendSMSBL.GetEmployeeDetails(objFilterParam);
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
                    #region EMPLOYEEOTDETAILS
                    case ControlsEnum.EMPLOYEEDETAILS:
                        BindGrid(ControlsEnum.EMPLOYEEDETAILS);
                        break;
                    #endregion                     
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
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
                IList<string> MobileNos = new List<string> {};
                switch (controlType)
                {
                    #region EMPLOYEE DETAILS
                    case ControlsEnum.EMPLOYEEDETAILS:
                        foreach (GridViewRow grdrow in grdEmployeePopUpList.Rows)
                        {
                            CheckBox chkEmp_popUp = (CheckBox)grdrow.FindControl("chkEmp_popUp");
                            Label lblMobNoPopUp= (Label)grdrow.FindControl("lblMobNoPopUp");
                            if (chkEmp_popUp.Checked)
                                MobileNos.Add(lblMobNoPopUp.Text);
                        }
                        txtMobNo.Text = (txtMobNo.Text != string.Empty ? txtMobNo.Text + "," + string.Join(",", MobileNos) : string.Join(",", MobileNos));
                        
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
                #region COMPANY
                case ControlsEnum.COMPANY:
                    ddlCompanyHd.DataTextField = Resources.DataFieldRes.CMP_NAME;
                    ddlCompanyHd.DataValueField = Resources.DataFieldRes.CMP_PK;
                    ddlCompanyHd.DataSource = dtCompany;
                    ddlCompanyHd.DataBind();
                    ddlCompanyHd.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    ddlCompanyHd.Items.HtmlDecode();
                    if (dtCompany != null && dtCompany.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                        ddlCompanyHd.SelectedIndex = ddlCompanyHd.Items.IndexOf(ddlCompanyHd.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
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
                        if (dslist.Tables[0].Rows.Count > 0)
                        {
                            rowCount = Convert.ToInt32(dslist.Tables[0].Rows[0]["TOTAL_ROW_COUNT"].ToString());
                        }
                        uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        grdSMSList.DataSource = dslist.Tables[0];
                        grdSMSList.DataBind();
                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                        break;
                    #endregion
                    #region EMPLOYEEOTDETAILS
                    case ControlsEnum.EMPLOYEEDETAILS:
                        if (dtEmployeeFilterList != null)
                        {
                            grdEmployeePopUpList.DataSource = dtEmployeeFilterList;
                            grdEmployeePopUpList.DataBind();
                        }
                        else
                        {
                            grdEmployeePopUpList.DataSource = null;
                            grdEmployeePopUpList.DataBind();
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
            SendSMSDetailsList = new List<SendSMSBO.SendSMSDetails>();
            SendSMSBO.SendSMSHeader tempSendSMSMaster = new SendSMSBO.SendSMSHeader();
            switch (controlType)
            {
                #region EMPLOYEE SALARY HDR
                case ControlsEnum.SENDSMSHDR:
                    tempSendSMSMaster.SMQ_TRX_TYPE = Convert.ToString(GetLocalResourceObject("SMQ_TRX_TYPE"));
                    tempSendSMSMaster.USER_PK = currentUser.PKUser;
                    tempSendSMSMaster.SMQ_BIZUNIT = currentUser.SBUID;
                    SetUIValuesToObject(ControlsEnum.SENDSMSETAILS);
                    tempSendSMSMaster.SendSMSDts = SendSMSDetailsList;
                    returnObject = tempSendSMSMaster;
                    break;
                #endregion
                #region SEND SMS ETAILS
                case ControlsEnum.SENDSMSETAILS:
                    string[] strArray = txtMobNo.Text.Split(',');
                    foreach (object obj in strArray)
                    {
                        SendSMSBO.SendSMSDetails objSendSMSDetails = new SendSMSBO.SendSMSDetails();
                       // objSendSMSDetails.SMQ_FROM = Convert.ToString(GetLocalResourceObject("SMQ_FROM"));
                        objSendSMSDetails.SMQ_TO = Convert.ToString(obj);
                        objSendSMSDetails.SMQ_MESSAGE = txtMessage.Text.HtmlEncode();
                        objSendSMSDetails.SMQ_SMS_DATE = DateTime.Now;
                        objSendSMSDetails.LAST_MOD_DATE = LastModifiedTime;
                        SendSMSDetailsList.Add(objSendSMSDetails);
                    }
                    break;
                #endregion
                #region SEND SMS DETAILS FROM LIST (RE-SEND)
                case ControlsEnum.EMPLOYEEDETAILSLISTRESEND:
                    tempSendSMSMaster.SMQ_TRX_TYPE = Convert.ToString(GetLocalResourceObject("SMQ_TRX_TYPE"));
                    tempSendSMSMaster.USER_PK = currentUser.PKUser;
                    tempSendSMSMaster.SMQ_BIZUNIT = currentUser.SBUID;
                    foreach (GridViewRow grdrow in grdSMSList.Rows)
                    {
                        CheckBox chkSMQ_PK_List = (CheckBox)grdrow.FindControl("chkSMQ_PK_List");
                        HiddenField hdfSMQ_STATUS_List = (HiddenField)grdrow.FindControl("hdfSMQ_STATUS_List");
                        HiddenField hdfLAST_MOD_DATE = (HiddenField)grdrow.FindControl("hdfLAST_MOD_DATE");
                        Label lblMobNo = (Label)grdrow.FindControl("lblMobNo");
                        Label lblMessage = (Label)grdrow.FindControl("lblMessage");
                        if (chkSMQ_PK_List.Checked)
                        {
                            SendSMSBO.SendSMSDetails objSendSMSDetails = new SendSMSBO.SendSMSDetails();
                            objSendSMSDetails.SMQ_TO = Convert.ToString(lblMobNo.Text);
                            objSendSMSDetails.SMQ_MESSAGE = lblMessage.Text.HtmlEncode();
                            objSendSMSDetails.SMQ_STATUS = Convert.ToInt32(hdfSMQ_STATUS_List.Value);
                            objSendSMSDetails.SMQ_SMS_DATE = DateTime.Now;
                            objSendSMSDetails.LAST_MOD_DATE = DateTime.Parse(hdfLAST_MOD_DATE.Value);
                            SendSMSDetailsList.Add(objSendSMSDetails);
                        }
                    }
                    tempSendSMSMaster.SendSMSDts = SendSMSDetailsList;
                    returnObject = tempSendSMSMaster;
                    break;
                #endregion
                #region SEND SMS DETAILS FROM LIST (Delete)
                case ControlsEnum.EMPLOYEEDETAILSLISTDELETE:
                    tempSendSMSMaster.SMQ_TRX_TYPE = Convert.ToString(GetLocalResourceObject("SMQ_TRX_TYPE"));
                    tempSendSMSMaster.USER_PK = currentUser.PKUser;
                    tempSendSMSMaster.SMQ_BIZUNIT = currentUser.SBUID;
                    foreach (GridViewRow grdrow in grdSMSList.Rows)
                    {
                        CheckBox chkSMQ_PK_List = (CheckBox)grdrow.FindControl("chkSMQ_PK_List");
                        HiddenField hdfSMQ_PKList = (HiddenField)grdrow.FindControl("hdfSMQ_PKList");
                        HiddenField hdfSMQ_STATUS_List = (HiddenField)grdrow.FindControl("hdfSMQ_STATUS_List");
                        HiddenField hdfLAST_MOD_DATE = (HiddenField)grdrow.FindControl("hdfLAST_MOD_DATE");
                        Label lblSMQ_SMS_DATE = (Label)grdrow.FindControl("lblSMQ_SMS_DATE");
                        if (chkSMQ_PK_List.Checked)
                        {
                            SendSMSBO.SendSMSDetails objSendSMSDetails = new SendSMSBO.SendSMSDetails();
                            objSendSMSDetails.SMQ_PK = Convert.ToInt32(hdfSMQ_PKList.Value);
                            objSendSMSDetails.SMQ_SMS_DATE = Convert.ToDateTime(lblSMQ_SMS_DATE.Text);
                            objSendSMSDetails.LAST_MOD_DATE = DateTime.Parse(hdfLAST_MOD_DATE.Value);
                            SendSMSDetailsList.Add(objSendSMSDetails);
                        }
                    }
                    tempSendSMSMaster.SendSMSDts = SendSMSDetailsList;
                    returnObject = tempSendSMSMaster;
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
                    txtMobNo.Text = string.Empty;
                    txtMessage.Text = string.Empty;
                    txtHdDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfHdDepartment.Value = CommonConstants.SELECTVAL;
                    txtHdBranchLocation.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfHdBranchLocation.Value = CommonConstants.SELECTVAL;
                    ddlCompanyHd.SelectedIndex = 0;
                    txtEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfEmployee.Value = CommonConstants.SELECTVAL;
                    txtImportDate.Text = string.Empty;
                    SendSMSDetailsList = null;

                    break;
                #endregion
                #region CLEAR EMP FILTER
                case ControlsEnum.CLEAREMPFILTER:
                    uclPaging.CurrentPage = 0;
                    this.PageIndex = "1";
                    txtFilterMobileNo.Text = string.Empty;
                    txtSrchFromDate.Text = string.Empty;
                    txtSrchToDate.Text = string.Empty;
                    break;
                #endregion
                #region ADDTOLIST
                case ControlsEnum.ADDTOLIST:
                    txtHdDepartment.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfHdDepartment.Value = CommonConstants.SELECTVAL;
                    txtHdBranchLocation.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfHdBranchLocation.Value = CommonConstants.SELECTVAL;
                    ddlCompanyHd.SelectedIndex = 0;
                    txtEmployee.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfEmployee.Value = CommonConstants.SELECTVAL;
                    SendSMSDetailsList = null;
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
                else if (Mode == ActionsEnum.EDIT)
                {
                    EntryStatus = EntryStatus.EDITMODE;
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

        #region Helper Methods

        #region Excel Import

        /// <summary>
        /// Methode used to read the excel data and save into grid
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="conStr"></param>
        private void ImportToGrid(string filePath, string conStr)
        {
            try
            {
                string SheetName = Resources.Constants.ExlSMSSheetName;
                excelColumns = airColums_General;
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
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                    SendSMSBO.SendSMSHeader SMSHeaderImport = new SendSMSBO.SendSMSHeader();
                    SMSHeaderImport.SMQ_TRX_TYPE = Convert.ToString(GetLocalResourceObject("SMQ_TRX_TYPE"));
                    SMSHeaderImport.USER_PK = currentUser.PKUser;
                    SMSHeaderImport.USER_PK = currentUser.PKUser;
                    SMSHeaderImport.SMQ_BIZUNIT = currentUser.SBUID;
                    SMSHeaderImport.SMS_IMPORT = 1;
                    List<SendSMSBO.SendSMSDetails> SMSDetailsimportDet = new List<SendSMSBO.SendSMSDetails>();
                    DataColumnCollection importColumns = dtImportData.Columns;
                    for (int i = 0; i < dtImportData.Rows.Count; i++)
                    {
                        DateTime attnDate = new DateTime();
                        string mobNo = string.Empty;
                        string message = string.Empty;
                        mobNo = Convert.ToString(dtImportData.Rows[i][Resources.Constants.ExlSMSMobCoulmn]);
                        message = Convert.ToString(dtImportData.Rows[i][Resources.Constants.ExlSMSMsgCoulmn]).Trim();
                        if (!string.IsNullOrEmpty(mobNo.Trim()) && !string.IsNullOrEmpty(message.Trim()))
                        {
                            DateTime.TryParse(txtImportDate.Text, out attnDate);
                            SendSMSBO.SendSMSDetails objAttendanceDetail = new SendSMSBO.SendSMSDetails();
                            objAttendanceDetail.SMQ_TO = mobNo.ToString();
                            objAttendanceDetail.SMQ_MESSAGE = message.HtmlEncode();
                            objAttendanceDetail.SMQ_SMS_DATE = attnDate;
                            objAttendanceDetail.LAST_MOD_DATE = LastModifiedTime;
                            SMSDetailsimportDet.Add(objAttendanceDetail);
                        }
                    }
                    SMSHeaderImport.SendSMSDts = SMSDetailsimportDet;// dt;

                    if (SMSHeaderImport.SendSMSDts != null && SMSHeaderImport.SendSMSDts.Count > 0)
                    {
                        string xmlDoc = CommonFunctions.XmlSerialize<SendSMSBO.SendSMSHeader>(SMSHeaderImport);
                        int? result = SendSMSBL.SaveSendSMSDetails(xmlDoc);
                        if (result > 0 && dsImportedAttendance.Tables.Count > 0)
                        {
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                            litErrorMsg.Text = GetLocalResourceObject("SMSImported").ToString();
                            object[] args = new object[1];
                            args[0] = Resources.PageNameRes.SENDSMS;
                            //args[1] = TrxNo;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

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
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.Attendance);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                        }
                    }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Err_NoRecordsForImport").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                    }
                }
            }
            catch (OleDbException ex)
            {
                litErrorMsg.Text = CommonFunctions.ProcessException(ex);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);

            }
        }
        #endregion
        #region SaveDetails

        /// <summary>
        /// Methode used to save the excel file
        /// </summary>
        private string SaveDetails(out string conStr, FileUpload fupUpload)
        {

            //uploadInfo = new FileInfo(fupImport.PostedFile.FileName);
            //extns = uploadInfo.Extension;
            uploadPath = string.Empty;
            if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
            {
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\SMS";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\SMS\\";
            }
            else
            {
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "SMS";
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                uploadPath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower() + "SMS\\";
            }

            FileInfo tempFileInfoObj;
            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
            string attachmentFileFormat = tempFileInfoObj.Extension;

            string FileName = Guid.NewGuid().ToString() + attachmentFileFormat;
            conStr = CheckValidFileType(attachmentFileFormat);

            if (attachmentFileFormat.ToLower() != ".xls" && attachmentFileFormat.ToLower() != ".xlsx")
            {
                return string.Empty;
            }
            if ((!string.IsNullOrEmpty(conStr)) || (attachmentFileFormat.ToLower() == ".txt"))
            {
                fupUpload.SaveAs(uploadPath + FileName);
            }

            return uploadPath + FileName;

        }
        #endregion
        #region CheckValidFileType
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
        #endregion
        #endregion

        #region ControlEnum
        public enum ControlsEnum
        {
            COMPANY,
            CLEAR,
            LIST,
            CLEARSEARCH,
            CLEAREMPFILTER,
            SENDSMSHDR,
            SENDSMSETAILS,
            EMPLOYEESALARYDETAILS,
            CLEAREMPLOYEESEARCH,

            EMPLOYEEDETAILS,
            ADDTOLIST,
            EMPLOYEEDETAILSLISTRESEND,
            EMPLOYEEDETAILSLISTDELETE


        }
        #endregion
    }
}