using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.CommonManagement;//using gERPDiscrete.BO.CommonManagement;
using BusinessObject;//using gErpConstruction.BO;
using BusinessLogic.CommonManagement;//using gErpConstruction.BL.CommonManagement;
using ERP.Utilities;//using gErpConstruction.Utilities;
using BusinessLogic.WorkOrder;
using System.Threading;
using CustomerPortal.UserControls;
using ERPData;
using ERPService;
using ERPManager;
using System.IO;
using System.Configuration;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject.WorkOrder;
using ERPSMS_v01;
using ERPSMS_v01.UserControls;
using BusinessLogic.AccountManagement;

namespace CustomerPortal.Projects
{
    public partial class Project : ERP.Store.UI.WorkFlowBasePage //System.Web.UI.Page  //
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// Current PK
        /// </summary>
        private int CurrPK  // this stores the primary key while editing,deleting and selecting
        {
            get
            {
                return Session[BusinessObject.Common.SessionStrings.WorkOrderPk] != null ? Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.WorkOrderPk]) : 0;
            }
            set
            {
                Session[BusinessObject.Common.SessionStrings.WorkOrderPk] = value;
            }
        }
        //refID,WoNo
        private int GridrefID
        {
            get { return ViewState["GridrefID"] == null ? 0 : (int)ViewState["GridrefID"]; }
            set { ViewState["GridrefID"] = value; }
        }
        private int UserStatus
        {
            get { return ViewState["UserStatus"] == null ? 0 : (int)ViewState["UserStatus"]; }
            set { ViewState["UserStatus"] = value; }
        }
        private string GridWoNo
        {
            get { return ViewState["GridWoNo"] == null ? null : (string)ViewState["GridWoNo"]; }
            set { ViewState["GridWoNo"] = value; }
        }
        /// <summary>
        /// Current PK View State
        /// </summary>
        private int CurrPKViewState  // this stores the primary key for Save
        {
            get
            {
                return ViewState[BusinessObject.Common.SessionStrings.WorkOrderPk] != null ? Convert.ToInt32(ViewState[BusinessObject.Common.SessionStrings.WorkOrderPk]) : 0;
            }
            set
            {
                ViewState[BusinessObject.Common.SessionStrings.WorkOrderPk] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private int PageIndex //-- for grid
        {
            get
            {
                return (this.ViewState[ViewstateStrings.PageIndex] == null ? 0 : (int)this.ViewState[ViewstateStrings.PageIndex]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PageIndex] = value;
            }
        }
        private int ProcessID
        {
            get
            {
                return this.ViewState["ProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["ProcessID"].ToString());
            }
            set
            {
                this.ViewState["ProcessID"] = value;
            }
        }

        /// <summary>
        /// To maintain the total pages in viewstate
        /// </summary>
        private int TotalPages //---grid dropdown paging values
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
        private string SortBy //--for grid
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
        private string ThenBy //-- grid
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
        private string SortDirection //--grid
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
        private int CurrSlNo
        {
            get
            {
                return ViewState[ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ViewstateStrings.CurrSlNo] = value;
            }
        }
        /// <summary>
        /// Aplication referance ID
        /// </summary>
        private int ReferanceID
        {
            get
            {
                return this.ViewState["ReferanceID"] == null ? 0 : Convert.ToInt32(this.ViewState["ReferanceID"].ToString());
            }
            set
            {
                this.ViewState["ReferanceID"] = value;
            }
        }
        ///// <summary>
        ///// SaveAndContinue
        ///// </summary>
        //private int SaveAndContinue  // this stores the SaveAndContinue flag
        //{
        //    get
        //    {
        //        return ViewState[ViewstateStrings.SaveAndContinue] != null ? Convert.ToInt32(ViewState[ViewstateStrings.SaveAndContinue]) : 0;
        //    }
        //    set
        //    {
        //        ViewState[ViewstateStrings.SaveAndContinue] = value;
        //    }
        //}
        private int IsWOAmend
        {
            get
            {
                return this.Session[ERP.Utilities.SessionStrings.IsWOAmend] != null ? Convert.ToInt32(this.Session[ERP.Utilities.SessionStrings.IsWOAmend]) : 0;
            }
            set
            {
                this.Session[ERP.Utilities.SessionStrings.IsWOAmend] = value;
            }
        }
        private int TypeCurrPK
        {
            get
            {
                return ViewState[ViewstateStrings.TypeCurrPK] == null ? 0 : (int)ViewState[ViewstateStrings.TypeCurrPK];
            }
            set
            {
                ViewState[ViewstateStrings.TypeCurrPK] = value;
            }
        }
        /// <summary>
        /// To maintain Line Limit
        /// </summary>
        private int WorkOrderLimit
        {
            get
            {
                return this.ViewState["WorkOrderLimit"] == null ? 1 : Convert.ToInt32(this.ViewState["WorkOrderLimit"]);
            }
            set
            {
                this.ViewState["WorkOrderLimit"] = value;
            }
        }
        #endregion
        #region Variables
        private BusinessObject.User currentUser;
        private ActionsEnum commonActions;
        DataTable dtPageData;
        DataSet dsWoList;
        double exchangeRate;
        private string refID;
        private int processPK;
        private string inboxFlag;
        #endregion
        #endregion
        #region WorkFlow Methods
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessID(bool IsAmend)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();

            if (IsAmend)
                path += "?AMEND=1";

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                ProcessID = ucrWrkf.ProcessID;
                ucrWrkf.FillWorkFlowDetails();
            }
            return ProcessID;
        }
        /// <summary>
        /// Set values to the control when edit details
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        private int GetApplicationID(int refId)
        {
            int appId = 0;
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtApplication = wrkfService.GetApplicationID(refId);
            if (dtApplication != null)
            {
                if (dtApplication.Rows.Count > 0)
                {
                    processPK = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PROCESS] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PROCESS]);
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }
        #endregion
        #region Page Level Events
        /// <summary>
        /// handles page load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion
        #region PageActionHandler
        /// <summary>
        /// Handles page load
        /// </summary>
        private void PageActionHandler()
        {
            string prefID;
            int referenceID;
            int appId;
            int preferenceID;
            try
            {
                ucrWrkf.ViewType = 1;
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                InitializeComponent();
                if (!IsPostBack)
                {
                    GetUserRights();
                    SetConfigValue();
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.TRANSACTIONSTATUS);
                    SetFieldValues(ControlsEnum.TRANSACTIONSTATUS);
                    this.PageIndex = 0;
                    CurrPKViewState = CurrPK;
                    #region Process & Workflow                   
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                      : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {

                        //ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        EnableDisablControls(EntryStatus);

                        ReferanceID = int.Parse(refID);
                        ucrWrkf.RefID = int.Parse(refID);
                        base.WkfRefID = ucrWrkf.RefID;
                        CurrPK = GetApplicationID(ucrWrkf.RefID);

                        ucrWrkf.ViewType = 1;

                        ucrWrkf.FillWorkFlowDetails();
                        btnSave.Visible = false;

                        if (CurrPK > 0)
                        {
                            IsWOAmend = 0;
                            EntryStatus = EntryStatus.EDITMODE;
                            GetFieldValues(ControlsEnum.TYPE);
                            SetFieldValues(ControlsEnum.TYPE);
                            GetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                            SetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                            GetFieldValues(ControlsEnum.CURRENCY);
                            SetFieldValues(ControlsEnum.CURRENCY);
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            GetFieldValues(ControlsEnum.WORKORDEREDITINFO);
                            SetFieldValues(ControlsEnum.WORKORDEREDITINFO);
                            ShowHideButtons(UserStatus);
                            EnableDisablControls(EntryStatus.EDITMODE);
                        }
                    }

                    else
                    {
                        ucrWrkf.ViewType = 1;

                        ucrWrkf.FillWorkFlowDetails();
                    }
                    //ucrWrkf.FillWorkFlowDetails();
                    //if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                    //    ucrWrkf.ViewType = 1;
                    //else
                    //    ucrWrkf.ViewType = 0;
                    #endregion

                    if (EntryStatus == EntryStatus.LISTMODE)
                    {
                        GetFieldValues(ControlsEnum.TYPE);
                        SetFieldValues(ControlsEnum.TYPE);
                        GetFieldValues(ControlsEnum.WORKORDERLIST);
                        SetFieldValues(ControlsEnum.WORKORDERLIST);
                    }
                }
            }
            catch { throw; }
        }

        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void SetConfigValue()
        {
            DataTable dtConfig = CommonBL.GetApplicaitonConfiguaration("USER LIMIT SETTINGS", string.Empty, currentUser.CurrentSBUPK);
            if (dtConfig != null && dtConfig.Rows.Count > 0)
            {
                ERP.Utilities.CryptoServices crypto = new ERP.Utilities.CryptoServices();
                string retVal = crypto.DecryptString(dtConfig.Rows[0]["ACF_DATA"].ToString(),
                                        ConfigurationManager.AppSettings["SYSKEY"]);
                try
                {
                    if (retVal.Length > 0)
                        WorkOrderLimit = Convert.ToInt32((retVal.Split('-')[(int)UserLimtSettingsIndex.Construction]));
                }
                catch (Exception ex)
                {
                    WorkOrderLimit = 0;
                }
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "ProjectBudgetRequired").ToString() == "1")
            {
                vrfBudget.Visible = true;
                vrfBudget.Enabled = true;
                lblProjectBudjet.Text = GetLocalResourceObject("ProjectBudgetStar").ToString();
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "CustomerRequired").ToString() == "1")
            {
                vrfCustomer.Visible = true;
                vrfCustomer.Enabled = true;
                lblCustomer.Text = GetLocalResourceObject("CustomerStar").ToString();
            }
        }
        private void GetUserRights()
        {
            #region ShortClose
            string path = GetLocalResourceObject("ProjectUrl").ToString();
            UserAuthBL userAuth = new UserAuthBL();
            UserRightsBO usrRights = userAuth.GetUserRights(currentUser.PKUser, path, currentUser.SBUID, currentUser.CurrentDeptPK);
            if (usrRights.Rights.Count > 0)
            {
                for (int i = 0; i < usrRights.Rights.Count; i++)
                {
                    if (usrRights.Rights[i].ActionName == "CANCEL" && usrRights.Rights[i].HasActionRight == true && usrRights.Rights[i].UserDeptRight == true)
                    {
                        hdfCloseWO.Value = "1";
                        break;
                    }
                }
            }
            #endregion
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
            //Session Logout on Department change
            //if (!(this.Master as ERPSMS_2).ValidatePageDept("../../login.aspx"))
            //    return;
            TextBox WrkfComments;
            string xmlDoc;
            bool bIsChecked = false;
            int result = 0;
            int hdfPk;
            int hdfVersion;
            int rowIndex;
            WorkflowCore.CoreService workflowCore;
            try
            {
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlCurrency")
                    {
                        commonActions = ActionsEnum.EXCHANGERATE;
                    }
                }

                switch (commonActions)
                {
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        CurrPKViewState = this.CurrPK = 0;
                        foreach (GridViewRow grdrow in grdWorkOrderList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPKViewState = this.CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfWorkOrderPk")).Value);
                                HiddenField hdfUserStatus = (HiddenField)grdrow.FindControl("hdfUserStatus");
                                UserStatus = Convert.ToInt32(hdfUserStatus.Value);
                                if (UserStatus != 0)
                                {
                                    GridrefID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRefId")).Value);
                                    GridWoNo = Convert.ToString(((HiddenField)grdrow.FindControl("hdfWoNo")).Value);
                                }
                                //btnNew.Visible = false;
                                if (hdfCloseWO.Value == "1" && (UserStatus == 2 || UserStatus == 7 || UserStatus == 1 || UserStatus == 6))//approved,more info submitted,submitted,request for more info
                                {
                                    btnShortClose.Visible = true;
                                }
                                else
                                {
                                    btnShortClose.Visible = false;
                                }
                                break;
                            }
                        }
                        ShowHideButtons(UserStatus);
                        break;
                    #endregion
                    #region SHORTCLOSE
                    case ActionsEnum.SHORTCLOSE:
                        int WoId = 0, ref_ID = 0, UStatus = 0;
                        string WoNo = string.Empty;
                        //WoId = CurrPK;
                        ref_ID = GridrefID;
                        UStatus = UserStatus;
                        WoNo = GridWoNo.ToString();
                        ResetShortClose();
                        lblWoNo.Text = WoNo;
                        PRJID.Value = CurrPK.ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divShortClose]','" + Resources.Controls.ShortClose + "','" + "480" + "','" + "230" + "');", true);
                        break;
                    #endregion

                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT Popup
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKFSubmit
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            //isCancelled = false;
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                ProjectBO projectObj = (ProjectBO)SetUIValuesToObject(ControlsEnum.WORKORDER);
                                projectObj.WKF_FLAG = 1;
                                projectObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT);
                                projectObj.WOH_LIC_LIMIT = WorkOrderLimit;
                                if (GetGlobalResourceObject("ConfigurationsRes", "WorkOrderLimiBasedOn").ToString() == "1")
                                    projectObj.WOH_LIC_ACTIVE_ONLY = "1";
                                if (projectObj != null)
                                {
                                    SaveTransaction(projectObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                }

                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                {
                                    FillProcessID(false);
                                }
                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                WrkfComments.Text = "";
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.WORKORDER);
                                GetFieldValues(ControlsEnum.WORKORDERLIST);
                                SetFieldValues(ControlsEnum.WORKORDERLIST);
                                UserStatus = 0;
                                //}
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        ProjectBO workOrderBOObj = (ProjectBO)SetUIValuesToObject(ControlsEnum.WORKORDER);
                        string refNo = string.Empty;
                        workOrderBOObj.WKF_FLAG = 0;
                        workOrderBOObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                        workOrderBOObj.WOH_LIC_LIMIT = WorkOrderLimit;
                        if (GetGlobalResourceObject("ConfigurationsRes", "WorkOrderLimiBasedOn").ToString() == "1")
                            workOrderBOObj.WOH_LIC_ACTIVE_ONLY = "1";
                        result = BusinessLogic.WorkOrder.WorkOrderBL.Save(workOrderBOObj, out refNo);
                        if (result > 0)
                        {
                            CurrPKViewState = CurrPK = result;
                            litErrorMsg.Text = GetGlobalResourceObject("ErrorMessages", "Msg_Save_Success_refNo").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("WorkOrder"), refNo);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ResetForm(ControlsEnum.WORKORDERLIST);
                            GetFieldValues(ControlsEnum.WORKORDERLIST);
                            SetFieldValues(ControlsEnum.WORKORDERLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CONCURRENCY)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("WorkOrder").ToString() + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ProjectCodeAlreadyExsists").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("WorkOrder").ToString() + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.LICENSEEECEEDS)
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Limi_Exeed;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("WorkOrder").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region NEW
                    case ActionsEnum.NEW:
                        IsWOAmend = 0;
                        CurrPKViewState = this.CurrPK = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.WORKORDER);
                        UserStatus = 0;
                        FillProcessID(false);
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        GetFieldValues(ControlsEnum.TYPE);
                        SetFieldValues(ControlsEnum.TYPE);
                        GetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                        SetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        EnableDisablControls(EntryStatus.NEWMODE);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.WorkOrder.WorkOrderBL.DeleteWorkOrder(CurrPKViewState, this.LastModifiedTime);
                        //0    Foreign key violation  
                        if (result > 0)
                        {
                            litErrorMsg.Text = GetGlobalResourceObject("ErrorMessages", "Msg_Delete_Success").ToString();
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("WorkOrder").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ResetForm(ControlsEnum.WORKORDERLIST);
                            GetFieldValues(ControlsEnum.WORKORDERLIST);
                            SetFieldValues(ControlsEnum.WORKORDERLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("WorkOrder").ToString();
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
                                litErrorMsg.Text = GetLocalResourceObject("WorkOrder").ToString() + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("WorkOrder").ToString() + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("WorkOrder").ToString() + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("WorkOrder").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        FillProcessID(false);
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        CurrPKViewState = CurrPK = PageIndex = 0;
                        GetFieldValues(ControlsEnum.WORKORDERLIST);
                        SetFieldValues(ControlsEnum.WORKORDERLIST);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        CurrPKViewState = CurrPK = PageIndex = 0;
                        GetFieldValues(ControlsEnum.WORKORDERLIST);
                        SetFieldValues(ControlsEnum.WORKORDERLIST);
                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ActionsEnum.EXCHANGERATE:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        SetFieldValues(ControlsEnum.EXCHANGERATE);
                        break;
                    #endregion
                    #region VIEW
                    case ActionsEnum.VIEW:
                        IsWOAmend = 0;
                        if (CurrPK == 0)
                        {
                            litErrorMsg.Text = GetGlobalResourceObject("Messages", "Msg_Select_Any_Row").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            return;
                        }
                        EntryStatus = EntryStatus.VIEWMODE;
                        FillProcessID(false);
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();
                        }
                        GetFieldValues(ControlsEnum.COMPANY);
                        SetFieldValues(ControlsEnum.COMPANY);
                        GetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                        SetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                        GetFieldValues(ControlsEnum.CURRENCY);
                        SetFieldValues(ControlsEnum.CURRENCY);
                        GetFieldValues(ControlsEnum.WORKORDEREDITINFO);
                        SetFieldValues(ControlsEnum.WORKORDEREDITINFO);
                        EnableDisablControls(EntryStatus.VIEWMODE);
                        break;
                    #endregion
                    #region EXCELDETAILS
                    case ActionsEnum.EXCELDETAILS:
                        if (Session[BusinessObject.Common.SessionStrings.WorkOrderPk] != null && Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.WorkOrderPk]) > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + Session[BusinessObject.Common.SessionStrings.WorkOrderPk].ToString() + "&APPTYPE=" + ApplicationType.PRJ + "&APPSUBTYPE=") + "');", true);
                        }
                        break;
                    #endregion
                    #region PRINT PDF
                    case ActionsEnum.PRINT:
                        if (Session[BusinessObject.Common.SessionStrings.WorkOrderPk] != null && Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.WorkOrderPk]) > 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + Session[BusinessObject.Common.SessionStrings.WorkOrderPk].ToString() + "&APPTYPE=" + ApplicationType.PRJ + "&APPSUBTYPE=4") + "');", true);
                        }
                        break;
                    #endregion
                    #region PRINTLIST PDF
                    case ActionsEnum.PRINTLIST:
                        foreach (GridViewRow grdrow in grdWorkOrderList.Rows)
                        {
                            HiddenField hdfWorkOrderPk;
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                hdfWorkOrderPk = (HiddenField)grdrow.FindControl("hdfWorkOrderPk");
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfWorkOrderPk.Value + "&APPTYPE=" + ApplicationType.PRJ + "&APPSUBTYPE=4") + "');", true);
                                return;
                            }
                        }
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "Msg_Select_Any_Row").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        break;
                    #endregion
                    #region EXCELPRINT
                    case ActionsEnum.EXCELPRINT:
                        foreach (GridViewRow grdrow in grdWorkOrderList.Rows)
                        {
                            HiddenField hdfWorkOrderPk;
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                hdfWorkOrderPk = (HiddenField)grdrow.FindControl("hdfWorkOrderPk");
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfWorkOrderPk.Value + "&APPTYPE=" + ApplicationType.PRJ + "&APPSUBTYPE=") + "');", true);
                                return;
                            }
                        }
                        litErrorMsg.Text = GetGlobalResourceObject("Messages", "Msg_Select_Any_Row").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        break;
                    #endregion
                    #region Amend
                    case ActionsEnum.AMEND:
                        if (CurrPK > 0)
                        {
                            ucrWrkf.ViewType = 1;
                            ucrWrkf.Visible = true;
                            ucrWrkf.RefID = 0;
                            FillProcessID(true);
                            ucrWrkf.ViewAction();
                            workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);//Convert.ToInt32(hdfWORefID.Value); //
                            ucrWrkf.FillWorkFlowDetails();
                            if (!ucrWrkf.HasActions)
                            {
                                ucrWrkf.ViewType = 0;
                                ucrWrkf.ViewAction();
                            }
                            if (UserStatus == 1)
                            {
                                btnSaveSubmit.Visible = false;
                                btnSave.Visible = false;
                                btnDelete.Visible = false;
                                if (EntryStatus == EntryStatus.LISTMODE)///new
                                {
                                    btnSaveSubmit.Visible = true;
                                    btnSubmit.Visible = false;
                                }

                            }
                            if (UserStatus == 2)
                            {
                                btnSubmit.Visible = false;
                                btnSave.Visible = false;
                                btnDelete.Visible = false;
                                btnSaveSubmit.Visible = true;

                            }
                            IsWOAmend = 1;
                            result = 0;
                            result = BusinessLogic.WorkOrder.WorkOrderBL.AmendSave(Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.WorkOrderPk]));
                            if (result > 0)
                            {
                                EntryStatus = EntryStatus.EDITMODE;
                                GetFieldValues(ControlsEnum.COMPANY);
                                SetFieldValues(ControlsEnum.COMPANY);
                                GetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                                SetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                                GetFieldValues(ControlsEnum.CURRENCY);
                                SetFieldValues(ControlsEnum.CURRENCY);
                                GetFieldValues(ControlsEnum.WORKORDEREDITINFO);
                                SetFieldValues(ControlsEnum.WORKORDEREDITINFO);

                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetGlobalResourceObject("Messages", "Msg_Select_Any_Row").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('"
                                + ERP.Utilities.CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region History
                    case ActionsEnum.HISTORY:
                        GetFieldValues(ControlsEnum.HISTORY);
                        SetFieldValues(ControlsEnum.HISTORY);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divHistoryDetail]','" + GetLocalResourceObject("HistoryDetail").ToString() + "','" + GetLocalResourceObject("popWidth") + "','" + GetLocalResourceObject("popHeight") + "');", true);
                        break;
                    #endregion
                    #region Print History
                    case ActionsEnum.PRINTREPORT:
                        rowIndex = Convert.ToInt32(((LinkButton)sender).CommandArgument);
                        GridViewRow row = grdHistoryList.Rows[rowIndex];
                        hdfPk = Convert.ToInt32(((HiddenField)row.FindControl("hdfPk")).Value);
                        hdfVersion = Convert.ToInt32(((HiddenField)row.FindControl("hdfVersion")).Value);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfPk + "&APPTYPE=" + ApplicationType.PRJ + "&VERSION=" + hdfVersion + "&APPSUBTYPE=1" + "") + "');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divHistoryDetail]','" + GetLocalResourceObject("HistoryDetail").ToString() + "','" + GetLocalResourceObject("popWidth") + "','" + GetLocalResourceObject("popHeight") + "');", true);
                        break;
                    #endregion
                    #region Menu Tabs
                    #region WORKORDER
                    case ActionsEnum.WORKORDER:
                        if ((sender.GetType().IsEquivalentTo(typeof(LinkButton))) && CurrPK == 0)
                        {
                            ActionHandler(btnNew, EventArgs.Empty);
                            EntryStatus = EntryStatus.NEWMODE;
                        }
                        else if (CurrPK > 0)
                        {
                            IsWOAmend = 0;
                            SetUIEditView(commonActions);
                            FillProcessID(false);
                            workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                ucrWrkf.ViewAction();
                            }
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                            GetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                            SetFieldValues(ControlsEnum.DEFECTLIABILITYUOM);
                            GetFieldValues(ControlsEnum.CURRENCY);
                            SetFieldValues(ControlsEnum.CURRENCY);
                            GetFieldValues(ControlsEnum.WORKORDEREDITINFO);
                            SetFieldValues(ControlsEnum.WORKORDEREDITINFO);
                            //if (UserStatus == 0)
                            //    EntryStatus = EntryStatus.EDITMODE;
                            //else
                            //    EntryStatus = EntryStatus.VIEWMODE;
                            EnableDisablControls(EntryStatus);
                        }
                        else
                        {
                            litErrorMsg.Text = GetGlobalResourceObject("Messages", "Msg_Select_Any_Row").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('"
                                + ERP.Utilities.CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + GetGlobalResourceObject("Messages", "Information").ToString() + "');", true);
                        }
                        break;
                    #endregion
                    #region WORKORDERLIST
                    case ActionsEnum.WORKORDERLIST:
                        Session[BusinessObject.Common.SessionStrings.WorkOrderPk] = 0;
                        Response.Redirect(Resources.PageURL.WorkOrder);
                        break;
                    #endregion

                    #endregion
                    #region SHORTCLOSESAVE
                    case ActionsEnum.SHORTCLOSESAVE:
                        ProjectCancel woCancel = new ProjectCancel();
                        woCancel.P_WOH_PK = Convert.ToInt32(PRJID.Value);
                        woCancel.P_WOH_SHORT_CLS_REFNO = RefNo.Text;
                        woCancel.P_WOH_SHORT_CLS_REASON = Remarks.Text;
                        woCancel.P_USER_PK = currentUser.PKUser;

                        int shortCloseResult = WorkOrderBL.CancelProject(woCancel);

                        if (shortCloseResult == 1)
                        {
                            PRJID.Value = "0";
                            litErrorMsg.Text = GetGlobalResourceObject("Messages", "PROJECTCancelledSuccessfully").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            ActionHandler(btnClear, EventArgs.Empty);
                            btnShortClose.Visible = false;
                        }
                        else
                        {
                            if (shortCloseResult == -2)
                            {
                                litErrorMsg.Text = Resources.Messages.PRExsists;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            if (shortCloseResult == -3)
                            {
                                litErrorMsg.Text = Resources.Messages.MaterialRequestExsist;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("WorkOrder").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region ACTIVATE
                    // Do Action if click Activate Button
                    case ActionsEnum.ACTIVATE:
                        currentUser = (User)HttpContext.Current.User.Identity;
                        GridViewRow grdwolist = ((ImageButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)grdWorkOrderList.Rows[grdwolist.RowIndex].FindControl("hdfWorkOrderPk")).Value);
                        LastModifiedTime = Convert.ToDateTime(((HiddenField)grdWorkOrderList.Rows[grdwolist.RowIndex].FindControl("hdfLastModDate")).Value);

                        // check activated or not : Success - Return PK

                        result = BusinessLogic.WorkOrder.WorkOrderBL.UpdateProjectStatus(CurrPK, 1, Convert.ToInt32(currentUser.PKUser.ToString()), LastModifiedTime);

                        if (result > 0)
                        {
                            //ClearSearch();
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Activate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("WorkOrder").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }

                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("WorkOrder").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("WorkOrder").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        GetFieldValues(ControlsEnum.WORKORDERLIST);
                        SetFieldValues(ControlsEnum.WORKORDERLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region DEACTIVATE
                    // Do Action if click DeActivate 
                    case ActionsEnum.DEACTIVATE:
                        currentUser = (User)HttpContext.Current.User.Identity;
                        GridViewRow grdworkorder = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        CurrPK = Convert.ToInt32((grdworkorder.FindControl("hdfWorkOrderPk") as HiddenField).Value);
                        LastModifiedTime = Convert.ToDateTime((grdworkorder.FindControl("hdfLastModDate") as HiddenField).Value);
                        // check activated or not : Success - Return PK

                        result = BusinessLogic.WorkOrder.WorkOrderBL.UpdateProjectStatus(CurrPK, 0, Convert.ToInt32(currentUser.PKUser.ToString()), LastModifiedTime);
                        if (result > 0)
                        {
                            //ClearSearch();
                            EntryStatus = EntryStatus.LISTMODE;
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_InActivate;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("WorkOrder").ToString());
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                        }
                        else
                        {
                            // if error or exception occur
                            DBActiveInactiveStatus dBActiveInactiveStatus = (DBActiveInactiveStatus)(result);
                            switch (dBActiveInactiveStatus)
                            {
                                // For Sql Error
                                case DBActiveInactiveStatus.SQLERROR:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;

                                case DBActiveInactiveStatus.CONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Modified;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("WorkOrder").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                case DBActiveInactiveStatus.DELETECONCURRENCY:
                                    //Scrip register for hiding the Details Part
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Error_Concurrent;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, this.GetLocalResourceObject("WorkOrder").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                                default:
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Sql_Error;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                    break;
                            }
                        }
                        GetFieldValues(ControlsEnum.WORKORDERLIST);
                        SetFieldValues(ControlsEnum.WORKORDERLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErro-rMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(ex.GetExceptionMessage()) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region
        /// <summary>
        /// Method used to Handle all Command actions of gridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {
        }

        #endregion
        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {

        }
        #endregion
        #region SetUIValuesToObject
        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>      
        private object SetUIValuesToObject(ControlsEnum controltype)
        {
            object returnObj;
            returnObj = null;
            ProjectBO workOrderObj = new ProjectBO();
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            switch (controltype)
            {
                #region WORKORDER
                case ControlsEnum.WORKORDER:
                    workOrderObj.WOH_PK = CurrPKViewState;//CurrPK;
                    workOrderObj.WOH_NO = HttpUtility.HtmlEncode(txtWoNo.Text.Trim());
                    workOrderObj.WOH_DATE = txtDate.Text.Trim();
                    workOrderObj.WOH_CUSTOMER = GetNullableInt(hdfCustomer.Value) ?? 0;
                    workOrderObj.WOH_CONSULTANT = GetNullableInt(hdfConsultant.Value) ?? 0;
                    workOrderObj.WOH_TYPE = Convert.ToInt32(ddlType.SelectedValue) > 0 ? Convert.ToInt32(ddlType.SelectedValue) : 0;
                    workOrderObj.WOH_WORK = HttpUtility.HtmlEncode(txtWorkProject.Text.Trim());
                    workOrderObj.WOH_SCOPE = HttpUtility.HtmlEncode(txtScope.Text.Trim());
                    workOrderObj.WOH_PROJECT_BUDGET = txtProjectBudjet.Text != string.Empty ? Convert.ToDouble(txtProjectBudjet.Text.Trim()) : 0;
                    workOrderObj.WOH_DESC = HttpUtility.HtmlEncode(txtDescription.Text.Trim());
                    workOrderObj.WOH_SITE = HttpUtility.HtmlEncode(txtSite.Text.Trim());
                    workOrderObj.WOH_REF_NO = HttpUtility.HtmlEncode(txtRefNo.Text.Trim());
                    workOrderObj.WOH_REF_DATE = txtReffDate.Text.Trim();
                    workOrderObj.WOH_LOCATION = HttpUtility.HtmlEncode(txtLocation.Text.Trim());
                    workOrderObj.WOH_EST_START_DATE = txtEstimatedStart.Text.Trim();
                    workOrderObj.WOH_EST_END_DATE = txtEstimatedEnd.Text.Trim();
                    workOrderObj.WOH_ACT_START_DATE = txtActualStart.Text.Trim();
                    workOrderObj.WOH_ACT_END_DATE = txtActualEnd.Text.Trim();
                    workOrderObj.WOH_DFCT_LBTY = txtDefectLiability.Text != string.Empty
                        ? txtDefectLiability.Text.Trim()
                        : null;
                    workOrderObj.WOH_DFCT_LBTY_UOM = txtDefectLiability.Text != string.Empty && ddlDefectLiabilityUOM.SelectedIndex != Convert.ToInt32(CommonConstants.SELECTVAL)
                        ? ddlDefectLiabilityUOM.SelectedItem.Value
                        : null;
                    workOrderObj.WOH_CURRENCY = GetNullableInt(ddlCurrency.SelectedValue) ?? 0;
                    workOrderObj.WOH_EXCHG_RATE = GetNullableDecimal(txtExchangeRate.Text.Trim()) ?? 1;
                    workOrderObj.WOH_RET_PERC = txtRetention.Text != string.Empty ? Convert.ToDouble(txtRetention.Text.Trim()) : 0;
                    workOrderObj.WOH_RET_LIMIT = txtLimit.Text != string.Empty ? Convert.ToDouble(txtLimit.Text.Trim()) : 0;
                    workOrderObj.WOH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                    workOrderObj.WOH_DEPT = currentUser.CurrentDeptPK;
                    workOrderObj.WOH_IS_LUMP_SUM = chkLumpSumPrj.Checked == true ? 1 : 0;
                    workOrderObj.WOH_LUMP_SUM_VALUE = txtLumpSumPrjValue.Text != string.Empty ? Convert.ToDouble(txtLumpSumPrjValue.Text.Trim()) : 0;
                    workOrderObj.BIZUNIT_PK = currentUser.SBUID;
                    workOrderObj.ACTIVE = chkStatus.Checked == true ? 1 : 0;
                    workOrderObj.USER_PK = currentUser.PKUser;
                    workOrderObj.LAST_MOD_DT = this.LastModifiedTime; // hdfLastModifiedDate.Value;
                    workOrderObj.WOH_CURRENCY_BC = currentUser.BaseCurrency;
                    workOrderObj.WOH_COMPANY = GetNullableInt(ddlCompany.SelectedValue) ?? 0;
                    workOrderObj.AMEND_FLAG = IsWOAmend;
                    if (IsWOAmend == 1)
                    {
                        workOrderObj.WOH_AMENDMENT_NO = txtAmendmentNo.Text.Trim();
                        workOrderObj.WOH_AMENDMENT_DATE = txtAmendmentDate.Text.Trim();
                    }
                    returnObj = workOrderObj;
                    break;
                    #endregion
            }
            return returnObj;
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
                    #region TRANSACTIONSTATUS
                    case ControlsEnum.TRANSACTIONSTATUS:
                        dtPageData = CommonBL.GetAppStatus("SWO", string.Empty);
                        break;
                    #endregion
                    #region TYPE
                    case ControlsEnum.TYPE:
                        this.dtPageData = CommonBL.GetCategoryByGroup(Convert.ToInt32(GroupType.Project), Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE)
                            , TypeCurrPK, Convert.ToInt32(CommonConstants.ACTIVE));
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        this.dtPageData = CommonBL.GetCurrencyList(currentUser.SBUID);
                        break;
                    #endregion
                    #region WORKORDERLIST
                    case ControlsEnum.WORKORDERLIST:
                        dsWoList = WorkOrderBL.GetWorkOrderList((PageIndex == 0 ? 1 : PageIndex), Convert.ToInt32(GetLocalResourceObject("PageSize"))
                            , currentUser.SBUID, txtFromDateSearch.Text.Trim(), txtToDateSearch.Text.Trim(), GetNullableInt(hdfCustomerSearch.Value) ?? 0
                            , GetNullableInt(ddlTypeSearch.SelectedValue) ?? 0, WorkOrderType.WO.ToString(), 0, null
                            , txtFilterProjectNo.Text != string.Empty ? txtFilterProjectNo.Text.Trim() : null
                            , txtFilterProjectCode.Text != string.Empty ? txtFilterProjectCode.Text.Trim() : null, null, currentUser.PKUser
                            , Convert.ToInt32(ddlTranStatus.SelectedValue) >= 0 ? Convert.ToInt32(ddlTranStatus.SelectedValue) : -1, Resources.PageURL.ProjectUrl.ToString(), Convert.ToInt32(ddlStatus.SelectedValue));
                        break;
                    #endregion
                    #region WORKORDEREDITINFO
                    case ControlsEnum.WORKORDEREDITINFO:
                        dtPageData = WorkOrderBL.GetWorkOrder(this.CurrPK, (GetNullableInt(CommonConstants.HASPK) ?? 0), currentUser.SBUID, 0, WorkOrderType.WO.ToString(),
                            Convert.ToInt32(CommonConstants.SELECT_VALUE_ZERO), currentUser.PKUser);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        //dsPageData = gErpConstruction.BL.Administration.Masters.CompanyMasterBL.GetCompanyMaster(0, 1, 0); // All companies
                        dtPageData = BusinessLogic.Administration.Masters.CompanyMasterBL.GetCompanyMaster(0, 1, currentUser.SBUID).Tables[0];
                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        exchangeRate = -1;
                        using (DataTable dt = CommonBL.GetExchangeRate(GetNullableInt(ddlCurrency.SelectedValue) ?? 0, currentUser.BaseCurrency,
                            txtDate.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(txtDate.Text.Trim())))
                        {
                            if (dt != null && dt.Rows.Count > 0)
                                exchangeRate = string.IsNullOrEmpty(dt.Rows[0][0].ToString()) ? -1 : Convert.ToDouble(dt.Rows[0][0].ToString());
                            if (exchangeRate < 0)
                                exchangeRate = Convert.ToDouble(CommonConstants.SELECT_VALUE_ONE);
                        }
                        break;
                    #endregion
                    #region History
                    case ControlsEnum.HISTORY:
                        dtPageData = WorkOrderBL.GetHistory(this.CurrPK);
                        break;
                    #endregion
                    #region DEFECTLIABILITYUOM
                    case ControlsEnum.DEFECTLIABILITYUOM:
                        this.dtPageData = CommonBL.GetAppConfig(currentUser.SBUID, CommonConstants.DEFECT_LIABILITY_PERIOD);
                        break;
                        #endregion

                }
            }
            catch { throw; }
        }
        #endregion
        #region GetUIValuesFromObject
        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region WORKORDEREDITINFO
                case ControlsEnum.WORKORDEREDITINFO:
                    if (dtPageData != null)
                    {
                        btnHistory.Visible = dtPageData.Rows[0]["WOH_VERSION"] != null
                            ? dtPageData.Rows[0]["WOH_VERSION"].ToString() != CommonConstants.SELECT_VALUE_ZERO
                                ? true
                                : false
                            : false;
                        if (dtPageData.Rows[0]["WOH_VERSION"] != null)//Checks Amended flag is there
                        {
                            if (dtPageData.Rows[0]["WOH_VERSION"].ToString() != CommonConstants.SELECT_VALUE_ZERO)//Already Amended
                            {
                                divAmendment.Visible = true;
                                txtAmendmentNo.Enabled = false;
                                txtAmendmentDate.Enabled = false;
                                txtAmendmentNo.CssClass = "group-txtbx disbldfield";
                                txtAmendmentDate.CssClass = "group-txtbx disbldfield";
                                rfvAmendmentNo.Enabled = false;
                                rfvAmendmentDate.Enabled = false;
                            }
                        }
                        if (IsWOAmend == 1)//Currently Amending
                        {
                            divAmendment.Visible = true;
                            txtAmendmentNo.Enabled = true;
                            txtAmendmentDate.Enabled = true;
                            txtAmendmentNo.CssClass = "group-txtbx";
                            txtAmendmentDate.CssClass = "group-txtbx";
                            rfvAmendmentNo.Enabled = true;
                            rfvAmendmentDate.Enabled = true;
                        }
                        txtWoNo.Text = dtPageData.Rows[0]["WOH_NO"].ToString().HtmlDecode();
                        txtDate.Text = (dtPageData.Rows[0]["WOH_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        txtCustomer.Text = dtPageData.Rows[0]["WOH_CUSTOMER_TEXT"].ToString().HtmlDecode();
                        hdfCustomer.Value = dtPageData.Rows[0]["WOH_CUSTOMER"].ToString();
                        txtConsultant.Text = dtPageData.Rows[0]["WOH_CONSULTANT_TEXT"].ToString().HtmlDecode();
                        hdfConsultant.Value = dtPageData.Rows[0]["WOH_CONSULTANT"].ToString();
                        txtRefNo.Text = dtPageData.Rows[0]["WOH_REF_NO"].ToString().HtmlDecode();
                        txtReffDate.Text = (dtPageData.Rows[0]["WOH_REF_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        txtWorkProject.Text = dtPageData.Rows[0]["WOH_WORK"].ToString().HtmlDecode();
                        txtScope.Text = dtPageData.Rows[0]["WOH_SCOPE"].ToString().HtmlDecode();
                        txtProjectBudjet.Text = Convert.ToDouble(dtPageData.Rows[0]["WOH_PROJECT_BUDGET"]).ToString();
                        txtDescription.Text = dtPageData.Rows[0]["WOH_DESC"].ToString().HtmlDecode();
                        txtSite.Text = dtPageData.Rows[0]["WOH_SITE"].ToString().HtmlDecode();
                        txtLocation.Text = dtPageData.Rows[0]["WOH_LOCATION"].ToString().HtmlDecode();
                        txtEstimatedStart.Text = (dtPageData.Rows[0]["WOH_EST_START_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        hdfEstimatedStartDate.Value = (dtPageData.Rows[0]["WOH_EST_START_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        txtEstimatedEnd.Text = (dtPageData.Rows[0]["WOH_EST_END_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        hdfEstimatedEndDate.Value = (dtPageData.Rows[0]["WOH_EST_END_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        txtActualStart.Text = (dtPageData.Rows[0]["WOH_ACT_START_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        hdfActualStartDate.Value = (dtPageData.Rows[0]["WOH_ACT_START_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        txtActualEnd.Text = (dtPageData.Rows[0]["WOH_ACT_END_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        hdfActualEndDate.Value = (dtPageData.Rows[0]["WOH_ACT_END_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT);
                        ddlCurrency.SelectedValue = dtPageData.Rows[0]["WOH_CURRENCY"].ToString();
                        if (dtPageData.Rows[0]["WOH_IS_BUDGETED"].ToString() == CommonConstants.SELECT_VALUE_ONE)
                        {
                            txtEstimatedStart.Enabled = false;
                            txtEstimatedEnd.Enabled = false;
                            txtEstimatedStart.CssClass += " input-disabled";
                            txtEstimatedEnd.CssClass += " input-disabled";
                        }

                        ActionHandler(ddlCurrency, EventArgs.Empty);
                        if (dtPageData.Rows[0]["WOH_IS_BOQ"] != null) //WOH_IS_BOQ =1 ? Has assigned Boq or add activity against WO
                        {
                            ddlCurrency.Enabled = dtPageData.Rows[0]["WOH_IS_BOQ"].ToString() == CommonConstants.SELECT_VALUE_ONE ? false : true;
                            ddlCurrency.CssClass = dtPageData.Rows[0]["WOH_IS_BOQ"].ToString() == CommonConstants.SELECT_VALUE_ONE
                                ? "group-select select-disabled"
                                : "group-select";
                        }
                        txtDefectLiability.Text = dtPageData.Rows[0]["WOH_DFCT_LBTY"] != null
                            ? dtPageData.Rows[0]["WOH_DFCT_LBTY"].ToString()
                            : string.Empty;
                        ddlDefectLiabilityUOM.SelectedIndex = dtPageData.Rows[0]["WOH_DFCT_LBTY_UOM"] != null ?
                            ddlDefectLiabilityUOM.Items.IndexOf(ddlDefectLiabilityUOM.Items.FindByValue(dtPageData.Rows[0]["WOH_DFCT_LBTY_UOM"].ToString()))
                            : 0;
                        txtExchangeRate.Text = dtPageData.Rows[0]["WOH_EXCHG_RATE"] != null
                                                    ? Convert.ToInt64(dtPageData.Rows[0]["WOH_EXCHG_RATE"]) > 0
                                                        ? CommonFunctions.GetFormattedExchangeRate(dtPageData.Rows[0]["WOH_EXCHG_RATE"].ToString())
                                                        : CommonConstants.SELECT_VALUE_ONE
                                                    : CommonConstants.SELECT_VALUE_ONE;
                        txtRetention.Text = dtPageData.Rows[0]["WOH_RET_PERC"] != null
                                                ? dtPageData.Rows[0]["WOH_RET_PERC"].ToString() != string.Empty
                                                    ? Convert.ToDouble(dtPageData.Rows[0]["WOH_RET_PERC"]) > 0
                                                        ? CommonFunctions.GetFormattedNumber(dtPageData.Rows[0]["WOH_RET_PERC"].ToString())
                                                        : string.Empty
                                                    : string.Empty
                                                : string.Empty;
                        txtLimit.Text = dtPageData.Rows[0]["WOH_RET_LIMIT"] != null
                                                ? dtPageData.Rows[0]["WOH_RET_LIMIT"].ToString() != string.Empty
                                                    ? Convert.ToDouble(dtPageData.Rows[0]["WOH_RET_LIMIT"]) > 0
                                                        ? CommonFunctions.GetFormattedNumber(dtPageData.Rows[0]["WOH_RET_LIMIT"].ToString())
                                                        : string.Empty
                                                    : string.Empty
                                                : string.Empty;
                        txtRemarks.Text = dtPageData.Rows[0]["WOH_REMARKS"].ToString().HtmlDecode();
                        chkStatus.Checked = dtPageData.Rows[0]["WOH_ACTIVE"].ToString() == CommonConstants.SELECT_VALUE_ONE ? true : false;
                        txtAmendmentNo.Text = dtPageData.Rows[0]["WOH_AMENDMENT_NO"] != null
                            ? dtPageData.Rows[0]["WOH_AMENDMENT_NO"].ToString().HtmlDecode()
                            : string.Empty;
                        txtAmendmentDate.Text = dtPageData.Rows[0]["WOH_AMENDMENT_DATE"] != null
                            ? dtPageData.Rows[0]["WOH_AMENDMENT_DATE"].ToString() != string.Empty
                                ? (dtPageData.Rows[0]["WOH_AMENDMENT_DATE"]).GetFormatedDateString(CommonConstants.DATEFORMAT)
                                : string.Empty
                            : string.Empty;
                        this.LastModifiedTime = Convert.ToDateTime(dtPageData.Rows[0]["LAST_MOD_DT"]);
                        ddlCompany.SelectedValue = dtPageData.Rows[0]["WOH_COMPANY"].ToString();
                        lblLastModifiedHDR.Text = string.Format("{0} {1}", GetGlobalResourceObject("Constants", "LastModifiedOn"),
                                    LastModifiedTime.ToString(GetGlobalResourceObject("Constants", "LastModDateFormat").ToString()));

                        chkLumpSumPrj.Checked = dtPageData.Rows[0]["WOH_IS_LUMP_SUM"].ToString() == CommonConstants.SELECT_VALUE_ONE ? true : false;
                        txtLumpSumPrjValue.Text = dtPageData.Rows[0]["WOH_LUMP_SUM_VALUE"].ToString();
                        UserStatus = Convert.ToInt32(dtPageData.Rows[0]["WOH_Status"]);
                        TypeCurrPK = Convert.ToInt32(dtPageData.Rows[0]["WOH_TYPE"].ToString());
                        GetFieldValues(ControlsEnum.TYPE);
                        SetFieldValues(ControlsEnum.TYPE);
                        ddlType.SelectedValue = TypeCurrPK.ToString();
                        TypeCurrPK = 0;

                    }
                    break;
                    #endregion
            }
        }
        #endregion
        #region Set Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void SetFieldValues(ControlsEnum type)
        {
            try
            {
                switch (type)
                {
                    case ControlsEnum.TRANSACTIONSTATUS:
                        BindDropDown(ControlsEnum.TRANSACTIONSTATUS);
                        break;
                    #region TYPE
                    case ControlsEnum.TYPE:
                        BindDropDown(ControlsEnum.TYPE);
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        BindDropDown(ControlsEnum.CURRENCY);
                        break;
                    #endregion
                    #region WORKORDEREDITINFO
                    case ControlsEnum.WORKORDEREDITINFO:
                        ResetForm(ControlsEnum.WORKORDER);
                        GetUIValuesFromObject(ControlsEnum.WORKORDEREDITINFO);
                        break;
                    #endregion
                    #region WORKORDERLIST
                    case ControlsEnum.WORKORDERLIST:
                        BindGrid(ControlsEnum.WORKORDERLIST);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        txtExchangeRate.Text = CommonFunctions.GetFormattedExchangeRate(exchangeRate.ToString());
                        break;
                    #endregion
                    #region History
                    case ControlsEnum.HISTORY:
                        BindGrid(ControlsEnum.HISTORY);
                        break;
                    #endregion
                    #region DEFECTLIABILITYUOM
                    case ControlsEnum.DEFECTLIABILITYUOM:
                        BindDropDown(ControlsEnum.DEFECTLIABILITYUOM);
                        break;
                        #endregion
                }
            }
            catch { }
        }
        #endregion
        #region HelperMethods
        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(ProjectBO objProject, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            //if (objWorkorder == null)
            //    objWorkorder = new WorkOrderBO();
            //else
            //    objWorkorder.AST_DOC_MODE = GetDOCMODE();

            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            if (objProject == null)
                objProject = new ProjectBO();
            objProject.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            objProject.WKF_APPLICATION = CurrPK;
            objProject.WKF_COMMENTS = wkfDetails.Comments;
            objProject.WKF_TRX_FLAG = workflowFlag;
            objProject.WKF_PROCESS = wkfDetails.ProcessID;
            objProject.WKF_REFERENCE = wkfDetails.ReferenceID;
            objProject.WKF_TASK = wkfDetails.TaskID;
            objProject.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;

            //objWorkorder.APT_CODE = ApplicationType.SUBWOD;


            #endregion
            //string xmlDoc = CommonFunctions.XmlSerialize<ProjectBO>(objProject);
            string woNumber = string.Empty;
            result = BusinessLogic.WorkOrder.WorkOrderBL.Save(objProject, out woNumber);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                WrkfComments.Text = "";
                if (string.IsNullOrEmpty(woNumber))
                    woNumber = txtWoNo.Text.Trim();
                object[] args = new object[2];
                args[0] = GetGlobalResourceObject("PageNameRes", "WorkOrder").ToString(); //Resources.PageNameRes.WorkOrder;
                args[1] = woNumber;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                //Show Save Message and redired to listing page                                        
                //litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.WorkOrder);
                #region Inbox or Listing Page Redirection
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm(ControlsEnum.WORKORDER);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ResetForm(ControlsEnum.WORKORDER);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.WORKORDERLIST);
                    SetFieldValues(ControlsEnum.WORKORDERLIST);
                }
                #endregion
                //}
                ucrWrkf.ApplicationID = result.Value;
            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = GetLocalResourceObject("WorkOrder").ToString() + " " +
                        GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "ProjectCodeAlreadyExsists").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.ALREADYDELETED)
                {
                    litErrorMsg.Text = GetLocalResourceObject("WorkOrder").ToString() + " " +
                        GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.ALREADYEXIST)
                {
                    litErrorMsg.Text = GetLocalResourceObject("NoAlreadyExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                //else if (result == (int)DbSaveStatus.LICENSEEECEEDS)
                //{
                //    litErrorMsg.Text = Resources.ErrorMessages.Msg_Limi_Exeed;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                //    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                //}
                else
                {
                    litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("WorkOrder").ToString());
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
            }

        }
        #endregion

        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region WORKORDERLIST
                    case ControlsEnum.WORKORDERLIST:
                        //grdWorkOrderList.DataSource = dtPageData.ta;
                        //uclPaging.Visible = false;
                        //if (dtPageData != null)
                        //{
                        //    int rowCount = 0;
                        //    int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));

                        //    if (dtPageData.Rows.Count > 0)
                        //        rowCount = Convert.ToInt32(dtPageData.Rows[0][0]);
                        //    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                        //          (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                        //          (rowCount / pageSize) + 1;
                        //    uclPaging.CurrentPage = PageIndex;
                        //    uclPaging.Visible = true;
                        //    uclPaging.BindPager();
                        //}
                        //grdWorkOrderList.DataBind();
                        if (dsWoList != null)
                        {

                            int rowCount = 0;
                            int pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                            if (dsWoList != null && dsWoList.Tables[1].Rows.Count > 0)
                            {
                                rowCount = Convert.ToInt32(dsWoList.Tables[0].Rows[0][0].ToString());
                            }
                            TotalPages = uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                          (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                          (rowCount / pageSize) + 1;
                            PageIndex = PageIndex == 0 ? 1 : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdWorkOrderList.DataSource = dsWoList.Tables[1];
                            grdWorkOrderList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdWorkOrderList.DataSource = null;
                            grdWorkOrderList.DataBind();
                        }
                        break;
                    #endregion
                    #region History
                    case ControlsEnum.HISTORY:
                        grdHistoryList.DataSource = dtPageData;
                        grdHistoryList.DataBind();
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        ///<summary>Function To reset the short close details
        ///</summary>
        private void ResetShortClose()
        {
            Remarks.Text = string.Empty;
            RefNo.Text = string.Empty;
        }
        public void ResetForm(ControlsEnum controltype)
        {
            switch (controltype)
            {
                #region WORKORDER
                case ControlsEnum.WORKORDER:
                    btnNew.Visible = true;
                    //UserStatus = 0;
                    //txtWoNo.Text = GetLocalResourceObject("[NEW]").ToString();
                    txtWoNo.Text = txtDate.Text = txtCustomer.Text = txtConsultant.Text = txtRefNo.Text = txtReffDate.Text = txtWorkProject.Text = txtDefectLiability.Text = string.Empty;
                    txtScope.Text = txtDescription.Text = txtSite.Text = txtLocation.Text = txtEstimatedStart.Text = txtProjectBudjet.Text = string.Empty;
                    txtEstimatedEnd.Text = txtActualStart.Text = txtActualEnd.Text = txtExchangeRate.Text = string.Empty;
                    txtRetention.Text = txtLimit.Text = txtRemarks.Text = string.Empty;
                    chkStatus.Checked = true;
                    hdfCustomer.Value = hdfConsultant.Value = hdfEstimatedStartDate.Value = hdfEstimatedEndDate.Value = string.Empty;
                    hdfActualStartDate.Value = hdfActualEndDate.Value = string.Empty;
                    ddlType.SelectedIndex = -1;
                    ddlCurrency.SelectedValue = currentUser.BaseCurrency.ToString();
                    txtDate.Text = Convert.ToDateTime(DateTime.Now).ToString(Resources.Constants.DateFormatShort);//Set default as current date
                    divAmendment.Visible = false;
                    txtAmendmentNo.Text = txtAmendmentDate.Text = string.Empty;
                    txtAmendmentNo.Enabled = txtAmendmentDate.Enabled = false;
                    txtAmendmentNo.CssClass = txtAmendmentDate.CssClass = "group-txtbx";
                    rfvAmendmentNo.Enabled = rfvAmendmentDate.Enabled = false;
                    this.LastModifiedTime = DateTime.Now;
                    ActionHandler(ddlCurrency, EventArgs.Empty);
                    chkLumpSumPrj.Checked = false;
                    txtLumpSumPrjValue.Text = string.Empty;
                    txtEstimatedStart.Enabled = true;
                    txtEstimatedEnd.Enabled = true;
                    txtEstimatedStart.CssClass = "group-txtbx";
                    txtEstimatedEnd.CssClass = "group-txtbx";
                    base.WkfRefID = 0;
                    break;
                #endregion
                #region WORKORDERLIST
                case ControlsEnum.WORKORDERLIST:
                    CurrPKViewState = CurrPK = 0;
                    IsWOAmend = 0;
                    PageIndex = 0;
                    ResetForm(ControlsEnum.WORKORDER);
                    UserStatus = 0;
                    GridWoNo = null;
                    GridrefID = 0;
                    break;
                #endregion
                #region CLEARSEARCH
                case ControlsEnum.CLEARSEARCH:
                    txtCustomerSearch.Text = txtFromDateSearch.Text = txtToDateSearch.Text = txtFilterProjectNo.Text = txtFilterProjectCode.Text = string.Empty;
                    hdfCustomerSearch.Value = hdfFromDateSearch.Value = hdfToDateSearch.Value = string.Empty;
                    ddlTypeSearch.SelectedIndex = ddlTranStatus.SelectedIndex = 0;
                    ddlStatus.SelectedIndex = -1;
                    break;
                    #endregion
            }
        }
        public void ShowHideButtons(int userstatus)
        {
            switch (userstatus)
            {
                case 0://draft
                    btnNew.Visible = true;
                    btnEdit.Visible = true;
                    btnView.Visible = true;
                    btnDelete.Visible = true;
                    btnAmend.Visible = false;
                    btnSave.Visible = true;
                    btnEXCELDETAILS.Visible = true;
                    btnSaveSubmit.Visible = true;
                    break;
                case 1://submitted
                    btnNew.Visible = true;
                    btnEdit.Visible = true;
                    btnView.Visible = true;
                    btnExport.Visible = true;
                    btnPrint.Visible = true;
                    btnAmend.Visible = false;
                    btnDelete.Visible = false;
                    btnSubmit.Visible = true;//now
                    btnEXCELDETAILS.Visible = true;
                    break;
                case 2://approved
                    btnNew.Visible = true;
                    btnEdit.Visible = true;
                    btnView.Visible = true;
                    btnDelete.Visible = false;
                    btnExport.Visible = true;
                    btnPrint.Visible = true;
                    btnAmend.Visible = true;
                    //btnEdit.Visible = false;
                    btnSubmit.Visible = false;//
                    btnSaveSubmit.Visible = true;//
                    break;
                case 3://Rejected
                    btnAmend.Visible = false;
                    btnEdit.Visible = false;
                    btnView.Visible = true;
                    break;
                case 4://cancelled
                    btnEdit.Visible = false;
                    btnAmend.Visible = false;
                    btnNew.Visible = true;
                    btnCancel.Visible = true;
                    btnSubmit.Visible = true;
                    btnView.Visible = true;
                    break;
                case 6://requested for more info
                    btnAmend.Visible = false;
                    btnSubmit.Visible = false;
                    btnSaveSubmit.Visible = true;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    break;
                case 7://more infot submitted
                    btnAmend.Visible = false;
                    btnView.Visible = true;
                    btnEdit.Visible = true;
                    btnSubmit.Visible = true;
                    break;
                case 11://verified
                    btnAmend.Visible = false;
                    btnView.Visible = true;
                    btnEdit.Visible = true;
                    btnSubmit.Visible = true;
                    btnPrint.Visible = true;
                    break;
                    //case 101:
                    //    btnEdit.Visible = true;
                    //    btnView.Visible = true;
                    //    btnDelete.Visible = false;
                    //    btnExport.Visible = true;
                    //    btnPrint.Visible = true;
                    //    btnAmend.Visible = true;
                    //    btnEdit.Visible = false;
                    //    btnSubmit.Visible = false;//
                    //    btnSaveSubmit.Visible = true;//
                    //    btnSave.Visible = false;
                    //    break;
            }

        }
        protected void ZeroQtyValidation_Validate(object source, ServerValidateEventArgs args)
        {
            var qty = Convert.ToDecimal(args.Value);

            if (qty <= 0)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        protected void DefectLiability_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtDefectLiability.Text == string.Empty && ddlDefectLiabilityUOM.SelectedItem.Value != CommonConstants.SELECTVAL)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        protected void DefectLiabilityUOM_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtDefectLiability.Text != string.Empty && ddlDefectLiabilityUOM.SelectedItem.Value == CommonConstants.SELECTVAL)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        protected void Percentage_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (txtRetention.Text == string.Empty)
                args.IsValid = true;
            else
            {
                decimal percentage = 0;
                Decimal.TryParse(txtRetention.Text, out percentage);
                if (percentage > 100)
                    args.IsValid = false;
                else
                    args.IsValid = true;
            }
        }
        /// <summary>
        /// Method for DropDown binding
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region TRANSACTIONSTATUS
                    case ControlsEnum.TRANSACTIONSTATUS:
                        ddlTranStatus.Items.Clear();
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            ddlTranStatus.DataValueField = GTIService.Constants.Common.Fields.ASC_VALUE;
                            ddlTranStatus.DataTextField = GTIService.Constants.Common.Fields.ASC_NAME;
                            ddlTranStatus.DataSource = dtPageData;
                            ddlTranStatus.DataBind();
                        }
                        ddlTranStatus.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        //foreach (ListItem item in ddlTrnStatus.Items)
                        //{
                        //    item.Text = HttpUtility.HtmlDecode(item.Text);
                        //}
                        break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        ddlCurrency.Items.Clear();
                        ddlCurrency.DataSource = dtPageData;
                        ddlCurrency.DataTextField = "CUR_CODE";
                        ddlCurrency.DataValueField = "CUR_PK";
                        ddlCurrency.DataBind();
                        ddlCurrency.Items.HtmlDecode();
                        ddlCurrency.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        ddlCurrency.SelectedValue = currentUser.BaseCurrency.ToString();
                        break;
                    #endregion
                    #region TYPE
                    case ControlsEnum.TYPE:
                        ddlType.Items.Clear();
                        ddlType.DataSource = dtPageData;
                        ddlType.DataValueField = "CON_PK";
                        ddlType.DataTextField = "CON_NAME";
                        ddlType.DataBind();
                        ddlType.Items.HtmlDecode();
                        ddlType.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));

                        ddlTypeSearch.Items.Clear();
                        ddlTypeSearch.DataSource = dtPageData;
                        ddlTypeSearch.DataValueField = "CON_PK";
                        ddlTypeSearch.DataTextField = "CON_NAME";
                        ddlTypeSearch.DataBind();
                        ddlTypeSearch.Items.HtmlDecode();
                        ddlTypeSearch.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        ddlCompany.Items.Clear();
                        ddlCompany.DataSource = dtPageData;
                        ddlCompany.DataValueField = "CMP_PK";
                        ddlCompany.DataTextField = "CMP_NAME";
                        ddlCompany.DataBind();
                        ddlCompany.Items.HtmlDecode();
                        if (dtPageData != null && dtPageData.Rows.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(dtPageData.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK])))
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtPageData.Rows[0][Resources.DataFieldRes.CMP_DEFAULT_PK].ToString()));
                        break;
                    #endregion
                    #region DEFECTLIABILITYUOM
                    case ControlsEnum.DEFECTLIABILITYUOM:
                        ddlDefectLiabilityUOM.Items.Clear();
                        ddlDefectLiabilityUOM.DataSource = dtPageData;
                        ddlDefectLiabilityUOM.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlDefectLiabilityUOM.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlDefectLiabilityUOM.DataBind();
                        ddlDefectLiabilityUOM.Items.HtmlDecode();
                        ddlDefectLiabilityUOM.Items.Insert(0, new ListItem(CommonConstants.SELECTTEXT, CommonConstants.SELECTVAL));

                        break;
                        #endregion
                }
            }
            catch { throw; }
        }
        private void EnableDisablControls(EntryStatus mode)
        {
            try
            {
                switch (mode)
                {
                    case EntryStatus.NEWMODE:
                        txtWoNo.Enabled = true;
                        txtSite.Enabled = true;
                        txtDate.Enabled = true;
                        ddlCompany.Enabled = true;
                        ddlType.Enabled = true;
                        txtConsultant.Enabled = true;
                        txtWorkProject.Enabled = true;
                        txtScope.Enabled = true;
                        txtDescription.Enabled = true;
                        txtLocation.Enabled = true;
                        //txtCustomer.Enabled = true;
                        txtEstimatedStart.Enabled = true;
                        txtEstimatedEnd.Enabled = true;
                        txtActualEnd.Enabled = true;
                        txtActualStart.Enabled = true;
                        txtDefectLiability.Enabled = true;
                        ddlDefectLiabilityUOM.Enabled = true;
                        ddlCurrency.Enabled = true;
                        txtExchangeRate.Enabled = true;
                        chkLumpSumPrj.Enabled = true;
                        chkStatus.Enabled = true;
                        txtRemarks.Enabled = true;
                        txtReffDate.Enabled = true;
                        txtRefNo.Enabled = true;
                        txtProjectBudjet.Enabled = true;
                        //btnSaveSubmit.Visible = true;
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        //btnDelete.Visible = false;
                        btnAmend.Visible = false;
                        btnSubmit.Visible = false;
                        btnEXCELDETAILS.Visible = false;
                        break;
                    case EntryStatus.ENTRYMODE:
                        if (UserStatus != 0 && UserStatus != 6)
                        {
                            txtWoNo.Enabled = false;
                            txtSite.Enabled = false;
                            txtDate.Enabled = false;
                            ddlCompany.Enabled = false;
                            ddlType.Enabled = false;
                            txtConsultant.Enabled = false;
                            txtWorkProject.Enabled = false;
                            txtScope.Enabled = false;
                            txtDescription.Enabled = false;
                            txtLocation.Enabled = false;
                            //txtCustomer.Enabled = false;
                            txtEstimatedStart.Enabled = false;
                            txtEstimatedEnd.Enabled = false;
                            txtActualEnd.Enabled = false;
                            txtActualStart.Enabled = false;
                            txtDefectLiability.Enabled = false;
                            ddlDefectLiabilityUOM.Enabled = false;
                            ddlCurrency.Enabled = false;
                            txtExchangeRate.Enabled = false;
                            chkLumpSumPrj.Enabled = false;
                            chkStatus.Enabled = false;
                            txtReffDate.Enabled = false;
                            txtRefNo.Enabled = false;
                            txtRemarks.Enabled = false;
                            txtLumpSumPrjValue.Enabled = false;

                            //txtCustomer.Enabled = false;
                            txtProjectBudjet.Enabled = false;
                            //btnSaveSubmit.Visible = false;
                            if (UserStatus != 0)
                                btnSave.Visible = false;
                            // btnDelete.Visible = false;
                            btnAmend.Visible = false;
                            if (UserStatus != 6)
                                btnSubmit.Visible = true;//ne w
                                                         //if (UserStatus == 0 && EntryStatus == EntryStatus.VIEWMODE)
                                                         //{
                                                         //    btnSaveSubmit.Visible = false;
                                                         //}
                        }
                        if (UserStatus == 6)
                        {
                            txtWoNo.Enabled = true;
                            txtSite.Enabled = true;
                            txtDate.Enabled = true;
                            ddlCompany.Enabled = true;
                            ddlType.Enabled = true;
                            txtConsultant.Enabled = true;
                            txtWorkProject.Enabled = true;
                            txtScope.Enabled = true;
                            txtDescription.Enabled = true;
                            txtLocation.Enabled = true;
                            //txtCustomer.Enabled = true;
                            txtEstimatedStart.Enabled = true;
                            txtEstimatedEnd.Enabled = true;
                            txtActualEnd.Enabled = true;
                            txtActualStart.Enabled = true;
                            txtDefectLiability.Enabled = true;
                            ddlDefectLiabilityUOM.Enabled = true;
                            ddlCurrency.Enabled = true;
                            txtExchangeRate.Enabled = true;
                            chkLumpSumPrj.Enabled = true;
                            chkStatus.Enabled = true;
                            txtReffDate.Enabled = true;
                            txtRefNo.Enabled = true;
                            txtRemarks.Enabled = true;
                            txtLumpSumPrjValue.Enabled = true;

                            //txtCustomer.Enabled = true;
                            txtProjectBudjet.Enabled = true;
                            //btnSaveSubmit.Visible = true;
                            if (UserStatus != 0 && UserStatus != 6)
                                btnSave.Visible = true;
                            // btnDelete.Visible = true;
                            btnAmend.Visible = true;
                            if (UserStatus != 6)
                                btnSubmit.Visible = true;//ne w

                        }
                        break;
                    case EntryStatus.VIEWMODE:
                        txtWoNo.Enabled = false;
                        txtSite.Enabled = false;
                        txtDate.Enabled = false;
                        ddlCompany.Enabled = false;
                        ddlType.Enabled = false;
                        txtConsultant.Enabled = false;
                        txtWorkProject.Enabled = false;
                        txtScope.Enabled = false;
                        txtDescription.Enabled = false;
                        txtLocation.Enabled = false;
                        //txtCustomer.Enabled = false;
                        txtEstimatedStart.Enabled = false;
                        txtEstimatedEnd.Enabled = false;
                        txtActualEnd.Enabled = false;
                        txtActualStart.Enabled = false;
                        txtDefectLiability.Enabled = false;
                        ddlDefectLiabilityUOM.Enabled = false;
                        ddlCurrency.Enabled = false;
                        txtExchangeRate.Enabled = false;
                        chkLumpSumPrj.Enabled = false;
                        chkStatus.Enabled = false;
                        txtReffDate.Enabled = false;
                        txtRefNo.Enabled = false;
                        txtRemarks.Enabled = false;
                        txtLumpSumPrjValue.Enabled = false;

                        //txtCustomer.Enabled = false;
                        txtProjectBudjet.Enabled = false;
                        if (UserStatus == 6)
                            btnSaveSubmit.Visible = false;
                        if (UserStatus != 0)
                            btnSave.Visible = false;
                        // btnDelete.Visible = false;
                        btnAmend.Visible = false;
                        if (UserStatus != 6)
                            btnSubmit.Visible = true;//ne w
                        if (UserStatus == 0 && EntryStatus == EntryStatus.VIEWMODE)
                        {
                            btnSaveSubmit.Visible = false;
                        }
                        //if (UserStatus == 2)
                        //    btnSubmit.Visible = true;
                        //if (UserStatus == 1||(UserStatus==2&&EntryStatus==EntryStatus.VIEWMODE))
                        //{
                        //    btnSubmit.Visible = true;
                        //    //btnExport.Visible = true;

                        //}
                        break;
                    case EntryStatus.EDITMODE:
                        if (UserStatus == 1 || UserStatus == 4 || UserStatus == 7)//disabling controls if redirected from inbox
                        {
                            txtWoNo.Enabled = false;
                            txtSite.Enabled = false;
                            txtDate.Enabled = false;
                            ddlCompany.Enabled = false;
                            ddlType.Enabled = false;
                            txtConsultant.Enabled = false;
                            txtWorkProject.Enabled = false;
                            txtScope.Enabled = false;
                            txtDescription.Enabled = false;
                            txtLocation.Enabled = false;
                            //txtCustomer.Enabled = false;
                            txtEstimatedStart.Enabled = false;
                            txtEstimatedEnd.Enabled = false;
                            txtActualEnd.Enabled = false;
                            txtActualStart.Enabled = false;
                            txtDefectLiability.Enabled = false;
                            ddlDefectLiabilityUOM.Enabled = false;
                            ddlCurrency.Enabled = false;
                            txtExchangeRate.Enabled = false;
                            chkLumpSumPrj.Enabled = false;
                            chkStatus.Enabled = false;
                            txtReffDate.Enabled = false;
                            txtRefNo.Enabled = false;
                            txtRemarks.Enabled = false;
                            txtLumpSumPrjValue.Enabled = false;
                            //txtCustomer.Enabled = false;
                            txtProjectBudjet.Enabled = false;
                            btnSaveSubmit.Visible = false;
                            btnSave.Visible = false;
                            // btnDelete.Visible = false;
                            btnAmend.Visible = false;
                        }
                        else
                        {
                            txtWoNo.Enabled = true;
                            txtSite.Enabled = true;
                            txtDate.Enabled = true;
                            ddlCompany.Enabled = true;
                            ddlType.Enabled = true;
                            txtConsultant.Enabled = true;
                            txtWorkProject.Enabled = true;
                            txtScope.Enabled = true;
                            txtDescription.Enabled = true;
                            txtLocation.Enabled = true;
                            //txtCustomer.Enabled = true;
                            txtEstimatedStart.Enabled = true;
                            txtEstimatedEnd.Enabled = true;
                            txtActualEnd.Enabled = true;
                            txtActualStart.Enabled = true;
                            txtDefectLiability.Enabled = true;
                            ddlDefectLiabilityUOM.Enabled = true;
                            ddlCurrency.Enabled = true;
                            txtExchangeRate.Enabled = true;
                            chkStatus.Enabled = false;
                            txtRefNo.Enabled = false;
                            txtRemarks.Enabled = false;
                            txtLumpSumPrjValue.Enabled = false;


                            btnSubmit.Visible = true;
                            btnSave.Visible = true;
                            btnDelete.Visible = true;
                            btnAmend.Visible = false;
                        }
                        break;
                }

                if (UserStatus >= 1 && UserStatus != 6)//(UserStatus!=6&&EntryStatus!=EntryStatus.ENTRYMODE)
                {
                    btnSaveSubmit.Visible = false;
                    btnSave.Visible = false;
                    btnDelete.Visible = false;

                }
                else if (UserStatus == 0)
                {
                    btnSubmit.Visible = false;
                }
                string viewMode = ((int)EntryStatus).ToString();
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_ViewMode", "PageViewMode(" + viewMode + ","+UserStatus+");", true);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private int? GetNullableInt(string str)
        {
            int i;
            return int.TryParse(str, out i) ? (int?)i : null;
        }
        private decimal? GetNullableDecimal(string str)
        {
            decimal i;
            return Decimal.TryParse(str, out i) ? (decimal?)i : null;
        }
        #endregion
        #region Pager Methods + Init
        /// <summary>
        /// To handle OnInit event Used to assign the Event for all the actions used in this page        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
        }
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
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            //this.lnkFuel.PreRender += new EventHandler(btnAction_PreRender);
            uclPaging.CurrentPage = 1;

            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEXCELDETAILS.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDelete.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnAmend.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnPrintList.PreRender += new EventHandler(btnAction_PreRender);
            btnClear.PreRender += new EventHandler(btnAction_PreRender);
            btnSearch.PreRender += new EventHandler(btnAction_PreRender);
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            //btnNew.Load += new EventHandler(btnAction_Load);
            //btnSubmit.Load += new EventHandler(btnAction_Load);
            //btnSave.Load += new EventHandler(btnAction_Load);
            //btnDelete.Load += new EventHandler(btnAction_Load);
            //btnCancel.Load += new EventHandler(btnAction_Load);
            //btnPrint.Load += new EventHandler(btnAction_Load);
        }
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
                switch (e.Action)
                {
                    case NavigationEnum.PAGECHANGE:
                        uclPaging.CurrentPage = e.CurrentPage;
                        break;
                    case NavigationEnum.FIRST:
                        // Assignment the first page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        // Assignment the last page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // Increment the next page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the previous page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage;
                GetFieldValues(ControlsEnum.WORKORDERLIST);
                SetFieldValues(ControlsEnum.WORKORDERLIST);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        #endregion
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            // Should we disable the first link?
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we disable the previous link?
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            // Should we enable the next link?
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            // Should we enable the last link?
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }

        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage        
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnAction_PreRender(object sender, EventArgs e)
        {
            //(this.Page as MyBasePage).CheckBtnVisibility(sender);
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
                if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ModifiedDatePnl.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ModifiedDatePnl.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(2);});", true);
                }
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                }

                else if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ModifiedDatePnl.Visible = true;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);

                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ModifiedDatePnl.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(ex.GetExceptionMessage()) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion
        #region Enum
        /// <summary>
        /// Controls Enum for the page
        /// </summary>
        public enum ControlsEnum
        {
            TRANSACTIONSTATUS,
            TYPE,
            CURRENCY,
            WORKORDEREDITINFO,
            WORKORDER,
            WORKORDERLIST,
            COMPANY,
            CLEARSEARCH,
            EXCHANGERATE,
            ACTIVATE,
            DEACTIVATE,
            HISTORY,
            DEFECTLIABILITYUOM
        }
        #endregion
    }
}