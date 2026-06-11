using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject;
using System.Data;
using System.Threading;
using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using ERP.Utilities.HRMS;
using BusinessLogic.Finance;
using BusinessObject.Finance;
using BusinessObject.CommonManagement;
using ERPService;
using ERPData;

namespace ERPSMS_v01.Finance
{
    public partial class MonthlyProduction : ERP.Store.UI.WorkFlowBasePage //System.Web.UI.Page
    {
        #region Variables & Properties

        #region Properties

        private int CurrPK
        {
            get
            {
                return ViewState[ViewstateStrings.CurrPK] == null ? 0 : (int)ViewState[ViewstateStrings.CurrPK];
            }
            set
            {
                ViewState[ViewstateStrings.CurrPK] = value;
            }
        }
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
        private int PageSize
        {
            get
            {
                return Convert.ToInt32(GetLocalResourceObject("PageSize").ToString());
            }
        }
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
        private int PageProcessID
        {
            get
            {
                return this.ViewState["PageProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["PageProcessID"].ToString());
            }
            set
            {
                this.ViewState["PageProcessID"] = value;
            }
        }
        #endregion

        #region Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        private List<MonthlyProductionDtl> objMonthlyProductionDtlList;
        private MonthlyProductionHdr objMonthlyProductionHdr;
        private List<MonthlyProductionDtl> objPlantDtlList;

        TextBox WrkfComments;
        private string action;
        private string refID;
        private string inboxFlag;
        private int processPK;
        bool isCancelled = false;

        private CommonService cm;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;

        #endregion

        #endregion

        #region Page Level Events
        #region Page Init
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

        #region Page Load

        protected void Page_Load(object sender, EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            InitializeComponent();
            if (!IsPostBack)
            {
                PageActionHandler();
            }
        }

        #endregion

        #region PageActionHandler

        private void PageActionHandler()
        {
            string prefID;
            int referenceID;
            int processID;
            int appId;

            referenceID = 0;
            processID = 0;
            appId = 0;

            PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);

            try
            {
                ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;

                    AST_DOC_MODE.Value = "0";
                    AST_DOC_MODE.Value = GetDOCMODE();

                    #region Number Formats
                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }

                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }
                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }
                    #endregion

                    #region Workflow
                    //FillProcessID(0, 1, true);
                    //processID = FillProcessID(0, 1);
                    //ucrWrkf.ProcessID = processID;

                    //if (ucrWrkf.ProcessID > 0)
                    //    hdfProcessID.Value = ucrWrkf.ProcessID.ToString();
                    //prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                    //    : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    //refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                    //    : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    //inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    //: Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    //ReferanceID = string.IsNullOrEmpty(refID)
                    //               ? string.IsNullOrEmpty(prefID)
                    //                   ? 0
                    //                   : int.Parse(prefID)
                    //               : int.Parse(refID);

                    ////If Has RefID (from Inbox)
                    //if (!string.IsNullOrEmpty(refID))
                    //{
                    //    if (!string.IsNullOrEmpty(inboxFlag))
                    //    {
                    //        ucrWrkf.ViewType = 0;
                    //        EntryStatus = EntryStatus.VIEWMODE;
                    //    }
                    //    else
                    //    {
                    //        ucrWrkf.ViewType = 1;
                    //        EntryStatus = EntryStatus.ENTRYMODE;
                    //    }
                    //    referenceID = int.Parse(refID);
                    //    appId = GetApplicationID(referenceID);
                    //    if (processPK == ucrWrkf.ProcessID)
                    //    {
                    //        CurrPK = appId;
                    //        base.WkfRefID = ucrWrkf.RefID = referenceID;
                    //    }
                    //}
                    //uclPaging.CurrentPage = 1;

                    //if (CurrPK > 0)
                    //{
                    //    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    //    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                    //    SetCancelRef(CurrPK); //uncomment on 13-July-2017
                    //    ucrWrkf.FillWorkFlowDetails();
                    //    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                    //        ucrWrkf.ViewType = 1;
                    //    else
                    //    {
                    //        ucrWrkf.ViewType = 0;
                    //    }
                    //    GetFieldValues(ControlsEnum.GETDETAILS);
                    //    SetFieldValues(ControlsEnum.EDITDETAILS);
                    //    EntryStatus = EntryStatus.EDITMODE;
                    //}
                    //else
                    //{
                    //    GetFieldValues(ControlsEnum.LIST);
                    //    SetFieldValues(ControlsEnum.LIST);
                    //    ResetForm(ControlsEnum.LIST);
                    //    EntryStatus = EntryStatus.LISTMODE;
                    //}
                    #endregion

                    objPlantDtlList = new List<MonthlyProductionDtl>();

                    ResetForm(ControlsEnum.LIST);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                imbShowFilter.Focus();
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }

        #endregion

        #region Pager Methods + Init

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                if (EntryStatus == EntryStatus.EDITMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
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
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        #endregion

        #endregion

        #region Action Handler

        protected void ActionHandler(object sender, EventArgs e)
        {
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;
            try
            {
                int? result;
                bool bIsChecked = false;
                string IsInventoryLocked = "";
                string LockUptoDate = string.Empty;
                hdfIsViewMode.Value = CommonConstants.SELECT_VALUE_ZERO;

                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                    if (commonActions.ToString() == "VIEW")
                        hdfIsViewMode.Value = CommonConstants.SELECT_VALUE_ONE;
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
                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm(ControlsEnum.NEW);
                        GetFieldValues(ControlsEnum.GETDETAILS);
                        SetFieldValues(ControlsEnum.GETDETAILS);
                        EntryStatus = EntryStatus.NEWMODE;
                        lblTrxNoTxt.Focus();
                        break;
                    #endregion

                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            #region Checking :Inventory Transaction Locking
                            IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtPeriod.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                            if (IsInventoryLocked == "1")
                            {
                                litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                            #endregion

                            string TrxNo = string.Empty;
                            objMonthlyProductionHdr = new MonthlyProductionHdr();
                            objMonthlyProductionHdr = (MonthlyProductionHdr)SetUIValuesToObject(ControlsEnum.MONTHLYPRODUCTIONHDR);

                            foreach (var item in objMonthlyProductionHdr.MonthlyProductionList)
                            {
                                if (item.FPD_QUANTITY == Convert.ToDouble(CommonConstants.SELECT_VALUE_ZERO))
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Enter_Qty").ToString();

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);

                                    return;
                                }
                            }

                            string xmlDoc = CommonFunctions.XmlSerialize<MonthlyProductionHdr>(objMonthlyProductionHdr);
                            result = MonthlyProductionBL.SaveMonthlyProductionDetails(xmlDoc, ref TrxNo);
                            if (result > 0) // Success !  redirect to listing page
                            {
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MonthlyProduction);

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);

                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.LIST);
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.ALREADYEXIST)
                                {
                                    litErrorMsg.Text = Resources.Messages.MonthAlreadyExist;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    //EntryStatus = EntryStatus.LISTMODE;
                                    //ResetForm(ControlsEnum.LIST);
                                    //GetFieldValues(ControlsEnum.LIST);
                                    //SetFieldValues(ControlsEnum.LIST);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.LIST);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.LIST);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MonthlyProduction);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region EDIT
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHdrPk")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.GETDETAILS);
                            SetFieldValues(ControlsEnum.EDITDETAILS);

                            if (hdfIsViewMode.Value ==  CommonConstants.SELECT_VALUE_ONE)
                            {
                                EntryStatus = EntryStatus.VIEWMODE;
                            }
                            else
                            {
                                #region Checking :Inventory Transaction Locking
                                IsInventoryLocked = BusinessLogic.CommonManagement.CommonBL.IsInventoryLocked(Convert.ToDateTime(txtPeriod.Text), currentUser.SBUID, Convert.ToByte(BusinessObject.CommonManagement.LockingModule.SMS), ref LockUptoDate);
                                if (IsInventoryLocked == "1")
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("ErrInvTransactionsLocked_Msg").ToString() + " " + (!string.IsNullOrEmpty(LockUptoDate) ? DateTime.Parse(LockUptoDate).ToString(Resources.Constants.DateFormatShort) : string.Empty);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                #endregion
                                EntryStatus = EntryStatus.EDITMODE;
                            }

                        }
                        break;
                    #endregion

                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.LIST);
                        ResetForm(ControlsEnum.NEW);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = MonthlyProductionBL.DeleteMonthlyProduction(CurrPK, LastModifiedTime);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MonthlyProduction);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.LIST);
                                GetFieldValues(ControlsEnum.LIST);
                                SetFieldValues(ControlsEnum.LIST);
                            }
                            else
                            {
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.LIST);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.LIST);
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MonthlyProduction);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion

                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.LIST);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion

                    #region SAVE SUBMIT POPUP
                    case ActionsEnum.SAVESUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region DELETE SUMBIT POPUP
                    case ActionsEnum.DELETESUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region SUBMIT POPUP
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion

                    #region WORKFLOW SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideWkfSubmit", "ClosePopup();", true);
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objMonthlyProductionHdr = new MonthlyProductionHdr();
                                objMonthlyProductionHdr = (MonthlyProductionHdr)SetUIValuesToObject(ControlsEnum.MONTHLYPRODUCTIONHDR);

                                if (objMonthlyProductionHdr != null && objMonthlyProductionHdr.MonthlyProductionList != null)
                                {
                                    SaveTransaction(objMonthlyProductionHdr, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel
                            {
                                #region Cancel Submit Codes

                                if (BusinessLogic.Sales.DirectSaleOrderBL.ValidationForCancellationSO(CurrPK))
                                {
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_MP_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(0, 1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;

                                    this.CurrPK = base.WkfRefID = 0;
                                    this.ModifiedDatePnl.Visible = false;
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                }
                                #endregion
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion

                    default: break;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }

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
            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            PageIndex = e.NewPageIndex;
        }
        #endregion

        #region GetFieldValues

        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        dtResult = MonthlyProductionBL.GetMonthlyProductionList(txtFromDate.Text, txtToDate.Text, currentUser.SBUID, PageIndex, Convert.ToInt32(GetLocalResourceObject("PageSize")));
                        break;
                    #endregion

                    #region GET DETAILS
                    case ControlsEnum.GETDETAILS:
                        objMonthlyProductionDtlList = new List<MonthlyProductionDtl>();
                        objMonthlyProductionHdr = null;
                        objMonthlyProductionHdr = MonthlyProductionBL.GetMonthlyProductionItemsList(CurrPK);
                        objMonthlyProductionDtlList = objMonthlyProductionHdr.MonthlyProductionList.ToList();
                        break;
                    #endregion

                    default: break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }

        #endregion

        #region SetFieldValues

        private void SetFieldValues(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        BindGrid(controlType);
                        break;
                    #endregion

                    #region GET DETAILS
                    case ControlsEnum.GETDETAILS:
                        BindGrid(controlType);
                        break;
                    #endregion

                    #region GET EDIT
                    case ControlsEnum.EDITDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    default: break;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region Set UI Values to Object

        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region Monthly Production Hdr
                    case ControlsEnum.MONTHLYPRODUCTIONHDR:
                        objMonthlyProductionHdr.FPH_PK = CurrPK;
                        objMonthlyProductionHdr.FPH_NO = lblTrxNoTxt.Text;
                        objMonthlyProductionHdr.FPH_TRX_DATE = DateTime.Parse(txtTxnDate.Text.ToString()).ToString();
                        var today = DateTime.Parse(txtPeriod.Text.ToString());
                        var CurrentMonth = new DateTime(today.Year, today.Month, 1);
                        DateTime Period = CurrentMonth.AddMonths(1).AddDays(-1);
                        objMonthlyProductionHdr.FPH_MONTH = Period.ToString();
                        objMonthlyProductionHdr.FPH_REMARKS = HttpUtility.HtmlEncode(txtDescHd.Text);
                        objMonthlyProductionHdr.FPH_DEPT = currentUser.CurrentDeptPK;
                        objMonthlyProductionHdr.FPH_COMPANY = currentUser.SBUID;
                        objMonthlyProductionHdr.BIZUNIT_PK = currentUser.SBUID;
                        objMonthlyProductionHdr.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        objMonthlyProductionHdr.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        objMonthlyProductionHdr.LAST_MOD_DT = LastModifiedTime;
                        objMonthlyProductionHdr.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                        objMonthlyProductionHdr.WKF_FLAG = 0;

                        objMonthlyProductionHdr.MonthlyProductionList = new List<MonthlyProductionDtl>();
                        List<MonthlyProductionDtl> detailList = new List<MonthlyProductionDtl>();
                        MonthlyProductionDtl objDetail;
                        foreach (GridViewRow gvRow in grdItemDetails.Rows)
                        {
                            objDetail = new MonthlyProductionDtl();
                            HiddenField hdfFpdPK = (HiddenField)gvRow.FindControl("hdfFpdPK");
                            HiddenField hdfPlantPK = (HiddenField)gvRow.FindControl("hdfPlantPK");
                            TextBox txtQtyGrd = (TextBox)gvRow.FindControl("txtQtyGrd");
                            TextBox txtRemarksGrd = (TextBox)gvRow.FindControl("txtRemarksGrd");

                            objDetail.FPD_PK = string.IsNullOrEmpty(hdfFpdPK.Value) ? 0 : Convert.ToInt32(hdfFpdPK.Value);
                            objDetail.FPD_PLANT = string.IsNullOrEmpty(hdfPlantPK.Value) ? 0 : Convert.ToInt32(hdfPlantPK.Value);
                            objDetail.FPD_QUANTITY = Convert.ToDouble(txtQtyGrd.Text);
                            objDetail.FPD_REMARKS = txtRemarksGrd.Text.ToString();
                            detailList.Add(objDetail);
                        }

                        objMonthlyProductionHdr.MonthlyProductionList = detailList;

                        objPlantDtlList = detailList;
                        retObject = objMonthlyProductionHdr;

                        break;
                    #endregion

                    default: break;
                }
                return retObject;
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

        #region Get UI Values from Object

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.EDITDETAILS:
                        if (objMonthlyProductionHdr != null)
                        {
                            lblTrxNoTxt.Text = string.IsNullOrEmpty(objMonthlyProductionHdr.FPH_NO) ? Resources.Messages.DocGenerationNew.ToString() : objMonthlyProductionHdr.FPH_NO.ToString();
                            txtTxnDate.Text = DateTime.Parse(objMonthlyProductionHdr.FPH_TRX_DATE).ToString(Resources.Constants.DateFormatShort);
                            txtPeriod.Text = DateTime.Parse(objMonthlyProductionHdr.FPH_MONTH).ToString(Resources.Constants.DateFormatMonthYear);
                            txtDescHd.Text = HttpUtility.HtmlDecode(objMonthlyProductionHdr.FPH_REMARKS);
                            LastModifiedTime = objMonthlyProductionHdr.LAST_MOD_DT;

                            objMonthlyProductionDtlList = new List<MonthlyProductionDtl>();
                            objMonthlyProductionDtlList = objMonthlyProductionHdr.MonthlyProductionList.ToList();
                            SetFieldValues(ControlsEnum.GETDETAILS);
                        }
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Bind Grid

        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        uclPaging.Visible = false;
                        if (dtResult != null && dtResult.Rows.Count > 0)
                        {
                            int rowCount = 0;
                            rowCount = Convert.ToInt32(dtResult.Rows[0]["TOTAL_ROW_COUNT"].ToString());
                            uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= this.PageSize) ? 1 :
                                              (rowCount % this.PageSize) == 0 ? (rowCount / this.PageSize) :
                                              (rowCount / this.PageSize) + 1;
                            PageIndex = PageIndex == null ? Convert.ToInt32(CommonConstants.SELECT_VALUE_ONE) : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdList.DataSource = dtResult;
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

                    #region GET DETAILS
                    case ControlsEnum.GETDETAILS:
                        if (objMonthlyProductionDtlList != null && objMonthlyProductionDtlList.Count > 0)
                        {
                            grdItemDetails.DataSource = objMonthlyProductionDtlList;
                            grdItemDetails.DataBind();
                        }
                        else
                        {
                            grdItemDetails.DataSource = null;
                            grdItemDetails.DataBind();
                        }
                        break;
                    #endregion

                    default: break;
                }
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region Reset

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.LIST:
                    CurrPK = 0;
                    ModifiedDatePnl.Visible = false;
                    LastModifiedTime = DateTime.Now;
                    lblLastModifiedHDR.Text = string.Empty;
                    txtFromDate.Text = string.Empty;
                    txtToDate.Text = string.Empty;
                    break;

                case ControlsEnum.NEW:
                    CurrPK = 0;
                    lblTrxNoTxt.Text = Resources.Messages.DocGenerationNew.ToString();
                    txtTxnDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    var today = DateTime.Today;
                    var CurrentMonth = new DateTime(today.Year, today.Month, 1);
                    DateTime Period = CurrentMonth;
                    txtPeriod.Text = Period.ToString(Resources.Constants.DateFormatMonthYear);
                    lblLastModifiedHDR.Text = string.Empty;
                    txtDescHd.Text = string.Empty;
                    break;
                default: break;
            }
        }

        #endregion

        #region Helper Methods

        #region Get Formatted Currency With Comma
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
        }
        #endregion

        #region Get Nullable Int
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }
        #endregion

        #region Convert TimeString To Double
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
        #endregion

        #region Get Time
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
            mm = Math.Round(((mm * 60) / 100));
            m = mm.ToString();
            if (h.Length < 2) h = "0" + h;
            if (m.Length < 2) m = "0" + m;
            result = h + ":" + m;
            return result;
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

        #region EnableDisableButtons
        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages, string pagerId)
        {
            if (pagerId == "uclPaging")
            {
                uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the first link
                uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;// Should we disable the previous link
                uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false; // Should we enable the next link
                uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;// Should we enable the last link
            }
        }
        #endregion

        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.FMP, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region Workflow Methods

        private void SaveTransaction(MonthlyProductionHdr objMonthlyProduction, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;

            if (objMonthlyProduction == null)
                objMonthlyProduction = new MonthlyProductionHdr();

            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objMonthlyProduction.USER_PK = wkfDetails.UserPK;
            objMonthlyProduction.WKF_APPLICATION = CurrPK;
            objMonthlyProduction.WKF_COMMENTS = wkfDetails.Comments;
            objMonthlyProduction.WKF_TRX_FLAG = workflowFlag;
            objMonthlyProduction.WKF_PROCESS = wkfDetails.ProcessID;
            objMonthlyProduction.WKF_REFERENCE = wkfDetails.ReferenceID;
            objMonthlyProduction.WKF_TASK = wkfDetails.TaskID;
            objMonthlyProduction.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion

            string strTrxNo = string.Empty;
            string xmlDoc = CommonFunctions.XmlSerialize<MonthlyProductionHdr>(objMonthlyProduction);
            result = MonthlyProductionBL.SaveMonthlyProductionDetails(xmlDoc, ref TrxNo);

            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                CurrPK = (int)result;

                #region After Workflow

                if (!isCancelled)
                {
                    GetFieldValues(ControlsEnum.GETDETAILS);
                    if (string.IsNullOrEmpty(strTrxNo))
                        strTrxNo = objMonthlyProduction.FPH_NO;
                    else
                        strTrxNo = lblTrxNoTxt.Text.Trim();
                }

                if (isCancelled)
                {
                    strTrxNo = lblTrxNoTxt.Text.Trim();
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Cancel_Success").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder, strTrxNo);
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.DirectSaleOrder, strTrxNo);
                }

                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ResetForm(ControlsEnum.LIST);
                    FillProcessID(0, 1);
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    this.EntryStatus = EntryStatus.LISTMODE;
                }
                ucrWrkf.ApplicationID = CurrPK = Convert.ToInt32(result);

                #endregion

            }
            else
            {
                #region Validation From SQL
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.MonthlyProduction + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.MonthlyProduction);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.Captions.Information + "');", true);
                    return;
                }
                #endregion
            }
        }

        //private void FillProcessID()
        //{
        //    string path = string.Empty;
        //    if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
        //        path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
        //    else
        //        path = Request.Url.AbsolutePath.ToLower();
        //    path = path + (Request.Url.Query.IndexOf('&') > 0 ? Request.Url.Query.Substring(0, Request.Url.Query.IndexOf('&')).ToLower()
        //        : Request.Url.Query.ToLower());

        //    WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
        //    DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
        //    if (dtProcess != null && dtProcess.Rows.Count > 0)
        //    {
        //        ucrWrkf.PageUrl = path;
        //        ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
        //        ProcessID = ucrWrkf.ProcessID;
        //    }

        //}

        private int FillProcessID(int deptID = 0, int pid = 0, bool SetProcessID = false)
        {
            int processID = 0;
            string path = string.Empty;

            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess;
            if (deptID > 0)
            {
                dtProcess = wrkfService.GetProcessID(path, deptID);
            }
            else
            {
                dtProcess = wrkfService.GetProcessID(path, Session[BusinessObject.Common.SessionStrings.CurDept] != null ? Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()) : 0);
            }
            if (SetProcessID && dtProcess != null && dtProcess.Rows.Count > 0)
            {
                hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
            }
            else
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    base.WkfPageUrl = ucrWrkf.PageUrl = path;
                    PageProcessID = ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    processID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                }
            return processID;
        }

        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }

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

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            LIST,
            CLEAR,
            EDIT,
            GETDETAILS,
            MONTHLYPRODUCTIONHDR,
            PAGE,
            NEW,
            EDITDETAILS
        }
        #endregion

    }
}