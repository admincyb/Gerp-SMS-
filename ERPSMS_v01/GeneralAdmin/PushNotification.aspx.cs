using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using BusinessObject.Common;
using ERP.Utilities;
using BusinessObject.Administration.Masters;
using ERPSMS_v01.UserControls;
using ERPSMS_v01.Administration.Masters;
using ERP.Utilities.HRMS;
using BusinessLogic.Administration.Masters;

namespace ERPSMS_v01.GeneralAdmin
{
    public partial class PushNotification : ERP.Store.UI.MyBasePage
    {
        #region Variables and Properties
        #region Variables
        private BusinessObject.User currentUser;
        private DataSet dslist;
        private DataTable dtCompany;
        private DataTable dtEmployeeFilterList;
        private int CompanyPk = 0;
        private PushNotificationBO.PushNotificationHd objPushHeader;
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
        private List<PushNotificationBO.PushNotificationDetails> PushDetailsList
        {
            get
            {
                return ViewState[ViewstateStrings.PushNotificationDetails] == null ? new List<PushNotificationBO.PushNotificationDetails>() : (List<PushNotificationBO.PushNotificationDetails>)ViewState[ViewstateStrings.PushNotificationDetails];
            }
            set
            {
                ViewState[ViewstateStrings.PushNotificationDetails] = value;
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
            if (EntryStatus == EntryStatus.ENTRYMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                // lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                //lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
            }
            else if (EntryStatus == EntryStatus.LISTMODE)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "ShowListing(1);", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch(1);", true);
               // lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
               // lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
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

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.CLEAREMPFILTER);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
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
                            objPushHeader = new PushNotificationBO.PushNotificationHd();
                            objPushHeader = (PushNotificationBO.PushNotificationHd)SetUIValuesToObject(ControlsEnum.RESENDPUSHNOTI);
                            if (objPushHeader != null)
                            {
                                if (objPushHeader.PushNotDts != null && objPushHeader.PushNotDts.Count > 0)
                                {
                                    // string TrxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<PushNotificationBO.PushNotificationHd>(objPushHeader);
                                    result = PushNotificationBL.SaveSendSMSDetails(xmlDoc);
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
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:

                        objPushHeader = new PushNotificationBO.PushNotificationHd();
                        objPushHeader = (PushNotificationBO.PushNotificationHd)SetUIValuesToObject(ControlsEnum.DELETEPUSHNOTI);
                        if (objPushHeader != null)
                        {
                            if (objPushHeader.PushNotDts != null && objPushHeader.PushNotDts.Count > 0)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<PushNotificationBO.PushNotificationHd>(objPushHeader);
                                result = PushNotificationBL.DeleteSMSDetails(xmlDoc);
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
                        dslist = PushNotificationBL.GetSendSMSList(objFilterParam);
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
                IList<string> MobileNos = new List<string> { };
                switch (controlType)
                {

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
            PushDetailsList = new List<PushNotificationBO.PushNotificationDetails>();
            PushNotificationBO.PushNotificationHd tempSendSMSMaster = new PushNotificationBO.PushNotificationHd();
            switch (controlType)
            {
                
                #region SEND SMS DETAILS FROM LIST (RE-SEND)
                case ControlsEnum.RESENDPUSHNOTI:
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
                            PushNotificationBO.PushNotificationDetails objSendSMSDetails = new PushNotificationBO.PushNotificationDetails();
                            objSendSMSDetails.SMQ_TO = Convert.ToString(lblMobNo.Text);
                            objSendSMSDetails.SMQ_MESSAGE = lblMessage.Text.HtmlEncode();
                            objSendSMSDetails.SMQ_STATUS = Convert.ToInt32(hdfSMQ_STATUS_List.Value);
                            objSendSMSDetails.SMQ_SMS_DATE = DateTime.Now;
                            objSendSMSDetails.LAST_MOD_DATE = DateTime.Parse(hdfLAST_MOD_DATE.Value);
                            PushDetailsList.Add(objSendSMSDetails);
                        }
                    }
                    tempSendSMSMaster.PushNotDts = PushDetailsList;
                    returnObject = tempSendSMSMaster;
                    break;
                #endregion
                #region SEND SMS DETAILS FROM LIST (Delete)
                case ControlsEnum.DELETEPUSHNOTI:
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
                            PushNotificationBO.PushNotificationDetails objSendSMSDetails = new PushNotificationBO.PushNotificationDetails();
                            objSendSMSDetails.SMQ_PK = Convert.ToInt32(hdfSMQ_PKList.Value);
                            objSendSMSDetails.SMQ_SMS_DATE = Convert.ToDateTime(lblSMQ_SMS_DATE.Text);
                            objSendSMSDetails.LAST_MOD_DATE = DateTime.Parse(hdfLAST_MOD_DATE.Value);
                            PushDetailsList.Add(objSendSMSDetails);
                        }
                    }
                    tempSendSMSMaster.PushNotDts = PushDetailsList;
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
                    PushDetailsList = null;
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



        #region ControlEnum
        public enum ControlsEnum
        {
            CLEAR,
            LIST,
            CLEAREMPFILTER,
            RESENDPUSHNOTI,
            DELETEPUSHNOTI
        }
        #endregion
    }
}