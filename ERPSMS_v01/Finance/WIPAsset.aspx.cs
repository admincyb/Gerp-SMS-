using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.AssetService;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ERPSMS_v01.Finance
{
    public partial class WIPAsset : ERP.Store.UI.WorkFlowBasePage //ERP.Store.UI.MyBasePage
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
        /// Current PK
        /// </summary>
        public int CurrPK
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

        private int Status
        {
            get { return ViewState["Status"] == null ? 0 : (int)ViewState["Status"]; }
            set { ViewState["Status"] = value; }
        }

        private string CWIPAssetV
        {
            get
            {
                return (string)this.ViewState["CWIPAssetV"];
            }
            set
            {
                this.ViewState["CWIPAssetV"] = value;
            }
        }

        private CWIPAsset CWIPAssetHeader
        {
            get
            {
                return ViewState["CWIPAssetHeader"] == null ? null : (CWIPAsset)ViewState["CWIPAssetHeader"];
            }
            set
            {
                if (ViewState["CWIPAssetHeader"] != null)
                    ViewState.Remove("CWIPAssetHeader");
                ViewState.Add("CWIPAssetHeader", value);
            }
        }
        /// <summary>
        /// Keep Selected List
        /// </summary>
        private List<int> SelectedList
        {
            get
            {
                return this.ViewState["SelectedList"] == null ? null : (List<int>)this.ViewState["SelectedList"];
            }
            set
            {
                this.ViewState["SelectedList"] = value;
            }
        }
        /// <summary>
        /// For partial save
        /// </summary>
        private Boolean SelectAllFlag
        {
            get
            {
                return this.ViewState["SelectAllFlag"] == null ? false : Convert.ToBoolean(this.ViewState["SelectAllFlag"]);
            }
            set
            {
                this.ViewState["SelectAllFlag"] = value;
            }
        }
        //private Double DebitTotal
        //{
        //    get
        //    {
        //        return (Double?)this.ViewState["DebitTotal"] == null ? 0 : (Double)this.ViewState["DebitTotal"];
        //    }
        //    set
        //    {
        //        this.ViewState["DebitTotal"] = value;
        //    }
        //}
        //private Double CreditTotal
        //{
        //    get
        //    {
        //        return (Double?)this.ViewState["CreditTotal"] == null ? 0 : (Double)this.ViewState["CreditTotal"];
        //    }
        //    set
        //    {
        //        this.ViewState["CreditTotal"] = value;
        //    }
        //}
        #endregion

        private ActionsEnum commonActions;
        private User currentUser;
        private DataTable dtCurrency;
        private DataTable dtResult;

        private int processPK;
        private string refID;
        private string inboxFlag;

        int JournalPK;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        //Double assetCostDefference = 0;

        #endregion

        #region PageLevel Events
        protected void Page_Load(object sender, EventArgs e)
        {
            PageActionHandler();
        }
        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            string prefID;
            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    hdfJournalizeWorkFlow.Value = "0";
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                }
                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                ucrWrkf.ViewType = 1;
                //ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                if (!IsPostBack)
                {
                    this.DataBind();
                    currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                    #region Decimal points
                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
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

                    EntryStatus = EntryStatus.LISTMODE;
                    txtCWIPDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    GetFieldValues(ControlsEnum.ACCGROUP);
                    SetFieldValues(ControlsEnum.ACCGROUP);
                    GetFieldValues(ControlsEnum.CWIPACCOUNTSEARCH);
                    SetFieldValues(ControlsEnum.CWIPACCOUNTSEARCH);
                    //GetFieldValues(ControlsEnum.CWIPACCOUNT);
                    //SetFieldValues(ControlsEnum.CWIPACCOUNT);
                    //GetFieldValues(ControlsEnum.CURRENCY);
                    //SetFieldValues(ControlsEnum.CURRENCY);
                    GetFieldValues(ControlsEnum.TRANSACTIONS);
                    SetFieldValues(ControlsEnum.TRANSACTIONS);
                    GetFieldValues(ControlsEnum.SEARCH);
                    SetFieldValues(ControlsEnum.SEARCH);

                    #region Process & Workflow   
                    FillProcessID();
                    string PageType = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType]
                         : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                      : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }

                        ReferanceID = int.Parse(refID);
                        if (string.IsNullOrEmpty(PageType) || PageType.Equals("1") || PageType.Equals("11") || PageType.Equals("21") || PageType.Equals("31"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            base.WkfRefID = ucrWrkf.RefID;
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            if (PageType.Equals("11") || PageType.Equals("31"))
                                hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }
                        else if (PageType.Equals("2") || PageType.Equals("12"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            JournalPK = GetApplicationID(ucrWrkf.RefID);
                            GetFieldValues(ControlsEnum.GETINVOICEPKBYJOURNALPK);
                            if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                            {
                                CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            }
                        }
                     
                        //base.WkfRefID = ucrWrkf.RefID = ReferanceID;
                        //CurrPK = GetApplicationID(ucrWrkf.RefID);

                        //ucrWrkf.FillWorkFlowDetails();
                    }
                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                        ReferanceID = int.Parse(prefID);
                    }
                    //else
                    //{
                    //    ucrWrkf.ViewType = 1;
                    //    ucrWrkf.FillWorkFlowDetails();
                    //}

                    if (CurrPK > 0)
                    {
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        EntryStatus = EntryStatus.ENTRYMODE;
                        GetFieldValues(ControlsEnum.ACCGROUP);
                        SetFieldValues(ControlsEnum.ACCGROUP);
                        GetFieldValues(ControlsEnum.CWIPASSET);
                        SetFieldValues(ControlsEnum.CWIPASSET);
                    }
                    #endregion

                    if (CurrPK == 0)
                    {
                        lblCWIPNo.Text = "[NEW]";
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            string XmlResult = string.Empty;
            string pageUrl = string.Empty;

            FinTrxService finTrxServiceClient;
            ServiceUtility serviceUtilityObj;
            try
            {
                switch (type)
                {
                    #region SEARCH
                    case ControlsEnum.SEARCH:
                        GridPrams gridParamObj = new GridPrams();
                        gridParamObj.PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        gridParamObj.PageSize = grdCWIPAssetList.PageSize;
                        gridParamObj.SortBy = SortBy = SortBy == null ? GetLocalResourceObject("SortBy").ToString() : SortBy;
                        gridParamObj.SortDirection = SortDirection = SortDirection == null ? Resources.ErpRes.SortDescending : SortDirection;
                        gridParamObj.SearchValue = Convert.ToInt32(hdfCWIPNoSearch.Value) > 0 ? txtCWIPNoSearch.Text : "%%";
                        gridParamObj.UserPK = currentUser.PKUser;
                        gridParamObj.FromDate = txtFromDate.Text;
                        gridParamObj.ToDate = txtToDate.Text;
                        gridParamObj.statusPk = Convert.ToInt32(ddlStatus.SelectedValue);
                        gridParamObj.PoNumber = (!string.IsNullOrEmpty(txtPoNum.Text.Trim())) ? "%" + HttpUtility.HtmlEncode(txtPoNum.Text.Trim()) + "%" : "%%";
                        pageUrl = Resources.PageURL.CWIPAsset;
                        int AccPK = Convert.ToInt32(ddlCWIPAccSearch.SelectedValue);


                        DataSet dsResult = new DataSet();
                        dsResult = BusinessLogic.Finance.CommSetupBL.GetCWIPAssetList(gridParamObj, currentUser.SBUID, pageUrl, AccPK);
                        gridParamObj.TotalRecords = Convert.ToInt32(dsResult.Tables[0].Rows.Count > 0 ? dsResult.Tables[0].Rows[0][0] : 0);
                        TotalPages = (gridParamObj.TotalRecords == 0) ? 1 : (gridParamObj.TotalRecords <= gridParamObj.PageSize) ? 1 : (gridParamObj.TotalRecords % gridParamObj.PageSize) == 0 ? (gridParamObj.TotalRecords / gridParamObj.PageSize) : (gridParamObj.TotalRecords / gridParamObj.PageSize) + 1;
                        dtResult = dsResult.Tables[1];
                        break;
                    #endregion
                    #region ACCGROUP
                    case ControlsEnum.ACCGROUP:
                        dtResult = BusinessLogic.Finance.CommSetupBL.GetAccountGroup(currentUser.SBUID);
                        break;
                    #endregion
                    #region CWIPACCOUNT - Commented
                    //case ControlsEnum.CWIPACCOUNT:
                    //    dtResult = BusinessLogic.Finance.CommSetupBL.GetCOAByParent(Convert.ToInt32(ddlAccGroup.SelectedValue), CurrPK);
                    //    break;
                    #endregion
                    #region  CWIPACCOUNTSEARCH
                    case ControlsEnum.CWIPACCOUNTSEARCH:
                        dtResult = BusinessLogic.Finance.CommSetupBL.GetCWIPFieldValues("CWH_ACCOUNT", string.Empty, currentUser.SBUID, currentUser.CurrentDeptPK);
                        break;
                    #endregion
                    #region  FCHold
                    //case ControlsEnum.FCHold:
                    //    dtResult = BusinessLogic.Sales.Enquiry.GetBankDetails(0, 1, currentUser.SBUID, (int)CashBankType.Bank, 1);
                    //    break;
                    #endregion
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        dtCurrency = BusinessLogic.CommonManagement.CommonBL.GetCurrencyHold(Convert.ToInt32(hdfCWIPAccPK.Value));
                        break;
                    #endregion
                    #region TRANSACTIONS
                    case ControlsEnum.TRANSACTIONS:
                        string poNumber = (!string.IsNullOrEmpty(txtPoNumberDetail.Text.Trim())) ? HttpUtility.HtmlEncode(txtPoNumberDetail.Text.Trim()) : null;
                        XmlResult = BusinessLogic.Finance.CommSetupBL.GetCOATransactionList(Convert.ToInt32(hdfCWIPAccPK.Value), CurrPK, poNumber);
                        CWIPAssetHeader = CommonFunctions.XmlDeserialize<CWIPAsset>(XmlResult);
                        break;
                    #endregion
                    #region CWIPASSET
                    case ControlsEnum.CWIPASSET:
                        XmlResult = BusinessLogic.Finance.CommSetupBL.GetCWIPAsset(CurrPK);
                        CWIPAssetHeader = CommonFunctions.XmlDeserialize<CWIPAsset>(XmlResult);
                        break;
                    #endregion
                    #region FIN HEADER
                    case ControlsEnum.FINHEADER:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_REF_TYPE = Session[ERP.Utilities.SessionStrings.TransactionType].ToString();
                        finTrxHdrObj.FTH_REF_PK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString());
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region GETINVOICEPKBYJOURNALPK
                    case ControlsEnum.GETINVOICEPKBYJOURNALPK:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = JournalPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    #endregion
                }
            }
            catch(Exception ex)
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
                    #region ACCGROUP
                    case ControlsEnum.ACCGROUP:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region CWIPACCOUNT - Commented
                    //case ControlsEnum.CWIPACCOUNT:
                    //    BindDropDown(controlType);
                    //    break;
                    #endregion
                    //case ControlsEnum.FCHold:
                    //    BindDropDown(controlType);
                    //    break;
                    case ControlsEnum.CURRENCY:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.TRANSACTIONS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.SEARCH:
                        BindGrid(controlType);
                        break;
                    #region CWIPASSET
                    case ControlsEnum.CWIPASSET:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region  CWIPACCOUNTSEARCH
                    case ControlsEnum.CWIPACCOUNTSEARCH:
                        BindDropDown(controlType);
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

        #region Helper Methods
        private void SaveTransaction(CWIPAsset objCWIPAsset, int workflowFlag, object sender)
        {
            try
            {
                int retRefID;
                int? result = 0;
                string transNo = string.Empty;
                string savePath = string.Empty;
                WorkflowDetails wkfDetails = null;
                string TrxNo = string.Empty;
                string action = string.Empty;
                if (objCWIPAsset == null)
                    objCWIPAsset = new CWIPAsset();

                #region New workflow Submition
                wkfDetails = ucrWrkf.GetWorkflowDetails();
                objCWIPAsset.USER_PK = wkfDetails.UserPK;
                objCWIPAsset.WKF_APPLICATION = CurrPK;
                objCWIPAsset.WKF_COMMENTS = wkfDetails.Comments;
                objCWIPAsset.WKF_TRX_FLAG = workflowFlag;
                objCWIPAsset.WKF_PROCESS = wkfDetails.ProcessID;
                objCWIPAsset.WKF_REFERENCE = wkfDetails.ReferenceID;
                objCWIPAsset.WKF_TASK = wkfDetails.TaskID;
                objCWIPAsset.WKF_TASK_ACTION = wkfDetails.ActionID;
                action = wkfDetails.ActionText;
                #endregion
                objCWIPAsset.APT_CODE = ApplicationType.CWIP;

                objCWIPAsset.CWIPDate = Convert.ToDateTime(txtCWIPDate.Text);
                string xmlDoc = CommonFunctions.XmlSerialize<CWIPAsset>(objCWIPAsset);
                result = BusinessLogic.Finance.CommSetupBL.SaveCWIPAssetWkf(xmlDoc, out retRefID, out transNo);
                if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
                {
                    CurrPK = (int)result;
                    litErrorMsg.Text = string.Format(GetLocalResourceObject("Msg_Submit_Success").ToString(), transNo);

                    FillProcessID();
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm(ControlsEnum.SAVE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ResetForm(ControlsEnum.SAVE);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                    }
                    #endregion
                    ucrWrkf.ApplicationID = result.Value;
                }
                else
                {
                    if (result == (int)DbSaveStatus.SQLERROR)
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("AccAlreadyExist").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.DATEOVERLAP)//Cost differ & depr exist
                    {
                        litErrorMsg.Text = GetLocalResourceObject("DeprExist").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result == (int)DbSaveStatus.ALREADYDELETED)//-5 Cost difference & voucher exist
                    {
                        litErrorMsg.Text = GetLocalResourceObject("VoucherExistCannotRefresh").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SAVE:
                        CWIPAssetHeader.CWIPAsset_PK = CurrPK;
                        CWIPAssetHeader.AssetCost = Convert.ToDecimal(txtAssetCost.Text == "" ? "0" : txtAssetCost.Text);
                        CWIPAssetHeader.AccGroup = Convert.ToInt32(ddlAccGroup.SelectedValue);
                        CWIPAssetHeader.CWIPAccount = Convert.ToInt32(hdfCWIPAccPK.Value);
                        CWIPAssetHeader.CWIPDate = Convert.ToDateTime(txtCWIPDate.Text);
                        CWIPAssetHeader.CWIPNo = lblCWIPNo.Text == "[NEW]" ? string.Empty : lblCWIPNo.Text;
                        CWIPAssetHeader.CreatedUserPK = currentUser.PKUser;
                        CWIPAssetHeader.BizunitPK = currentUser.SBUID;
                        CWIPAssetHeader.DeptPK = currentUser.CurrentDeptPK;

                        //if (SelectedList != null && SelectedList.Count > 0 && SelectAllFlag)
                        //{
                        //    List<TransactionList> SelectesTranDetails = new List<TransactionList>();
                        //    SelectedList.ForEach(itm =>
                        //    {
                        //        SelectesTranDetails.Add(CWIPAssetHeader.TranDetails.SingleOrDefault(i => i.FTR_PK == itm));
                        //    });

                        //    CWIPAssetHeader.TranDetails = SelectesTranDetails;
                        //}

                        if (SelectAllFlag)
                            retObject = CWIPAssetHeader;
                        else
                        {
                            var detail = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1).ToList();
                            CWIPAssetHeader.TranDetails = detail;
                            retObject = CWIPAssetHeader;
                        }
                        break;
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        //    if (EntryStatus == EntryStatus.LISTMODE)
                        //    {
                        //        foreach (GridViewRow grdrow in grdFCRList.Rows)
                        //        {
                        //            RadioButton rbtn;
                        //            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                        //            if (rbtn.Checked)
                        //            {
                        //                bIsChecked = true;
                        //                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfHRHpk")).Value);
                        //                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                        //                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                        //                break;
                        //            }
                        //        }
                        //    }
                        //    else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        //    {
                        //        bIsChecked = true;
                        //    }

                        //    if (bIsChecked)
                        //    {
                        if (Status == 2)
                        {
                            //GetFieldValues(ControlsEnum.FCRHEADER);
                            Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                            Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                            Session[ERP.Utilities.SessionStrings.JournalMode] = null;

                            //Journalize New sessions start
                            Session[ERP.Utilities.SessionStrings.DrControls] = null;
                            Session[ERP.Utilities.SessionStrings.CrControls] = null;
                            Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                            Session[ERP.Utilities.SessionStrings.AccountType] = null;
                            Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                            Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                            //Journalize New sessions End

                            CWIPAssetV = ApplicationType.CWIPJ;
                            ucrJournalize.TransactionType = CWIPAssetV;
                            Session[ERP.Utilities.SessionStrings.TransactionType] = CWIPAssetV;
                            ucrJournalize.TransactionPK = CurrPK;
                            Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                            ucrJournalize.JournalizePK = 0;
                            Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                            Session[ERP.Utilities.SessionStrings.TransactionNo] = CWIPAssetHeader.CWIPNo;// dsPageData.Tables[0].Rows[0]["HRH_NO"];
                            Session[ERP.Utilities.SessionStrings.TransactionDate] = CWIPAssetHeader.CWIPDate.ToString();// dsPageData.Tables[0].Rows[0]["HRH_DATE"].ToString();
                            //Session[ERP.Utilities.SessionStrings.TransactionCurrency] = dsPageData.Tables[0].Rows[0]["HRH_CURRENCY"].ToString();
                            Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                            //Session[ERP.Utilities.SessionStrings.AccountPayablePK] = FCHoldReverseObj.IVH_VENDOR;
                            Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.CWIPJ;

                            ucrWrkf.WrkfSubmit -= ActionHandler;
                            ucrWrkf.Reset();
                            ucrWrkf.ViewType = 1;
                            FillProcessID(2);
                            GetFieldValues(ControlsEnum.FINHEADER);
                            //EntryStatus = EntryStatus.ENTRYMODE;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                            {
                                ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                base.WkfRefID = ucrWrkf.RefID;
                            }
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                ucrWrkf.ViewType = 1;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrJournalize.HasWkfPermission = ucrWrkf.HasPageTaskPermission;


                            Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus == EntryStatus.ENTRYMODE ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;

                            ucrWrkf.ViewAction();

                            HiddenField hdfExchangeRateJV = (HiddenField)ucrJournalize.FindControl("hdfExchangeRateJV");
                            hdfExchangeRateJV.Value = "";

                            TextBox txtJournalExchangeRate = (TextBox)ucrJournalize.FindControl("txtJournalExchangeRate");
                            txtJournalExchangeRate.Text = "";

                            TextBox txtNarration = (TextBox)ucrJournalize.FindControl("txtNarration");
                            txtNarration.Text = "";

                            TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                            WrkfComments.Text = "";

                            Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                            hdfJournalizeWorkFlow.Value = "1";

                            ucrJournalize.CallUserControl();
                            Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("CWIPJournal").ToString();

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        //    }
                        //    else
                        //    {
                        //        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //    }
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

        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            string TYPE = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType] : string.Empty;
            if (TYPE != "3")//Type 3 for cancelation
            {
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
            #endregion
        }

        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch(controlType)
                {
                    case ControlsEnum.CWIPASSET:
                        lblCWIPNo.Text = CWIPAssetHeader.CWIPNo == string.Empty ? "[NEW]" : CWIPAssetHeader.CWIPNo;
                        txtCWIPDate.Text = CWIPAssetHeader.CWIPDate.ToString(Resources.ErpRes.DateFormat);
                        ddlAccGroup.SelectedIndex = ddlAccGroup.Items.IndexOf(ddlAccGroup.Items.FindByValue(CWIPAssetHeader.AccGroup.ToString()));
                        Status = CWIPAssetHeader.UserStatus;

                        //GetFieldValues(ControlsEnum.CWIPACCOUNT);
                        //SetFieldValues(ControlsEnum.CWIPACCOUNT);

                        //ddlCWIPAccount.SelectedIndex = ddlCWIPAccount.Items.IndexOf(ddlCWIPAccount.Items.FindByValue(CWIPAssetHeader.CWIPAccount.ToString()));

                        txtCWIPAccount.Text = CWIPAssetHeader.CWIPAccountText;
                        hdfCWIPAccPK.Value = CWIPAssetHeader.CWIPAccount.ToString();

                        BindGrid(ControlsEnum.TRANSACTIONS);
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void SetViewMode(EntryStatus status)
        {
            try
            {
                switch (status)
                {
                    case EntryStatus.NEWMODE:
                        btnSave.Visible = true;
                        btnDeleteCWIP.Visible = false;
                        btnPrint.Visible = false;
                        break;
                    case EntryStatus.VIEWMODE:
                        btnSave.Visible = false;
                        btnDeleteCWIP.Visible = false;
                        btnSaveSubmit.Visible = false;
                        btnPrint.Visible = true;
                        break;
                    case EntryStatus.ENTRYMODE:
                        btnSave.Visible = false;
                        btnDeleteCWIP.Visible = false;
                        btnPrint.Visible = true;
                        if (Status == 0)//Drafted
                        {
                            btnDeleteCWIP.Visible = true;
                            btnSave.Visible = true;
                           
                        }
                        else if (hdfRefreshClicked.Value == "1") //approved
                            btnSave.Visible = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void EnableDisableControls()
        {
            try
            {
                if (Status == 2) //stats-2-approved
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EnableDisable", "EnableDisableCWIPAccount('0');", true);
                else
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "EnableDisable", "EnableDisableCWIPAccount('1');", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ShowHideControls()
        {
            imbRefresh.Visible = false;
            //if (CWIPAssetHeader.InvoiceAmendFlag == 1) //CWIPAssetHeader.HasDepreciation == 0
            //    imbRefresh.Visible = true;

            lblDeprCalculated.Visible = false;
            if (CWIPAssetHeader.HasDepreciation == 1)
                lblDeprCalculated.Visible = true;
        }

        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.ACCGROUP:
                    ddlAccGroup.Items.Clear();
                    if (dtResult != null)
                    {
                        ddlAccGroup.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, "COA_NAME");
                        ddlAccGroup.DataTextField = "COA_NAME";
                        ddlAccGroup.DataValueField = "COA_PK";
                        ddlAccGroup.DataBind();
                    }
                    ddlAccGroup.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #region  CWIPACCOUNTSEARCH
                case ControlsEnum.CWIPACCOUNTSEARCH:
                    ddlCWIPAccSearch.Items.Clear();
                    if (dtResult != null)
                    {
                        ddlCWIPAccSearch.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, "VALUE");
                        ddlCWIPAccSearch.DataTextField = "VALUE";
                        ddlCWIPAccSearch.DataValueField = "PK";
                        ddlCWIPAccSearch.DataBind();
                    }
                    ddlCWIPAccSearch.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                //case ControlsEnum.FCHold:
                //    ddlHold.Items.Clear();
                //    if (dtResult != null)
                //    {
                //        ddlHold.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, "CBM_NAME");
                //        ddlHold.DataTextField = "CBM_NAME";
                //        ddlHold.DataValueField = "CBM_PK";
                //        ddlHold.DataBind();
                //    }
                //    ddlHold.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                //    break;
                //case ControlsEnum.CWIPACCOUNT:
                //    ddlCWIPAccount.Items.Clear();
                //    if (dtResult != null)
                //    {
                //        ddlCWIPAccount.DataSource = CommonFunctions.HtmlDecodeDataTable(dtResult, "COA_TEXT");
                //        ddlCWIPAccount.DataTextField = "COA_TEXT";
                //        ddlCWIPAccount.DataValueField = "COA_PK";
                //        ddlCWIPAccount.DataBind();
                //    }
                //    ddlCWIPAccount.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                //    break;
                //case ControlsEnum.CURRENCY:
                //    ddlCurrency.Items.Clear();
                //    if (dtCurrency != null && dtCurrency.Rows.Count > 0)
                //    {
                //        ddlCurrency.DataSource = dtCurrency;
                //        ddlCurrency.DataTextField = Resources.DataFieldRes.Currency;
                //        ddlCurrency.DataValueField = Resources.DataFieldRes.CurrencyPK;
                //        ddlCurrency.DataBind();
                //    }
                //    if (ddlCurrency.Items.Count <= 0)
                //        ddlCurrency.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                //    break;
            }
        }
        private void BindGrid(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.TRANSACTIONS:
                    grdCOATransactions.DataSource = CWIPAssetHeader.TranDetails;
                    grdCOATransactions.DataBind();
                    if (CWIPAssetHeader != null && CWIPAssetHeader.TranDetails != null && CWIPAssetHeader.TranDetails.Any())
                    {
                        decimal DebitAmt = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1 && w.IsDebit == 1).Sum(s => s.TransNow);
                        decimal CreditAmt = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1 && w.IsDebit == 0).Sum(s => s.TransNow);
                        CWIPAssetHeader.AssetCost = DebitAmt - CreditAmt;
                        txtAssetCost.Text = CWIPAssetHeader.AssetCost.ToString("C");
                    }
                    break;
                case ControlsEnum.SEARCH:
                    TotalPages = TotalPages;
                    uclPaging.TotalPages = TotalPages;
                    PageIndex = PageIndex == null ? "1" : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    grdCWIPAssetList.DataSource = dtResult;
                    grdCWIPAssetList.DataBind();
                    uclPaging.Visible = true;
                    uclPaging.BindPager();
                    break;
            }
        }
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetFormattedExchangeRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
        }
        public void ResetForm(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.SAVE:
                        CurrPK = 0;
                        Status = 0;
                        hdfRefreshClicked.Value = "0";
                        txtAssetCost.Text = "0";
                        lblCWIPNo.Text = "[NEW]";
                        ddlAccGroup.SelectedIndex = 0;
                        txtCWIPAccount.Text = "Select/Type";
                        hdfCWIPAccPK.Value = "0";
                        break;
                    case ControlsEnum.CWIPASSET:
                        CurrPK = 0;
                        Status = 0;
                        hdfRefreshClicked.Value = "0";
                        txtAssetCost.Text = "0";
                        lblCWIPNo.Text = "[NEW]";
                        ddlAccGroup.SelectedIndex = 0;
                        txtCWIPAccount.Text = "Select/Type";
                        hdfCWIPAccPK.Value = "0";
                        GetFieldValues(ControlsEnum.TRANSACTIONS);
                        SetFieldValues(ControlsEnum.TRANSACTIONS);
                        break;
                    case ControlsEnum.SEARCH:
                        txtFromDate.Text = string.Empty;
                        txtToDate.Text = string.Empty;
                        ddlCWIPAccSearch.SelectedIndex = 0;
                        txtCWIPNoSearch.Text = "Select/Type";
                        hdfCWIPNoSearch.Value = "0";
                        txtPoNum.Text = string.Empty;
                        break;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private bool IsCheckedAny()
        {
            bool IsChecked = false;
            foreach (GridViewRow grdrow in grdCOATransactions.Rows)
            {
                CheckBox chkInvoice = (CheckBox)grdrow.FindControl("chkInvoice");
                if (chkInvoice.Checked)
                {
                    IsChecked = true;
                    break;
                }
            }
            return IsChecked;
        }

        #endregion

        #region WorkFlow Methods

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

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private int FillProcessID(int type = 0)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            else
                path = Request.Url.AbsolutePath.ToLower();
            if (type > 0)
                path += "?TYPE=" + type;

            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtProcess = wrkfService.GetProcessID(path, currentUser.CurrentDeptPK);
            if (dtProcess != null && dtProcess.Rows.Count > 0)
            {
                ucrWrkf.PageUrl = path;
                ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                ProcessID = ucrWrkf.ProcessID;

                base.WkfPageUrl = path;

                ucrWrkf.FillWorkFlowDetails();
            }
            return ProcessID;
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
            int retRefID;
            int? result;
            string transNo = string.Empty;
            bool IsChecked = false;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            WorkflowCore.CoreService workflowCore;
            GridViewRow grvEditRow;
            decimal DebitAmt = 0;
            decimal CreditAmt = 0;
            try
            {
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
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    commonActions = ActionsEnum.SHOWDETAILS;
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlCurrency")
                    {
                        commonActions = ActionsEnum.BANKCURRENCY;
                    }
                    //if (((DropDownList)sender).ID == "ddlCWIPAccount")
                    //{
                    //    commonActions = ActionsEnum.CHANGEVALUE;
                    //}
                    if (((DropDownList)sender).ID == "ddlAccGroup")
                    {
                        commonActions = ActionsEnum.SELECTEDINDEXCHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {                   
                    if (((CheckBox)sender).ID == "chkInvoice")
                    {
                        commonActions = ActionsEnum.ADDSELECTION;
                    }
                    if (((CheckBox)sender).ID == "chkSelectAllInvoice")
                    {
                        commonActions = ActionsEnum.SELECTALL;
                    }
                }
                switch (commonActions)
                {
                    #region SHOWDETAILS
                    case ActionsEnum.SHOWDETAILS:
                        foreach (GridViewRow grdrow in grdCWIPAssetList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn != null && rbtn.Checked)
                            {
                                IsChecked = true;
                                CurrPK = Convert.ToInt32((grdrow.FindControl("hdfCWIPAssetPK") as HiddenField).Value);
                                Status = Convert.ToInt32((grdrow.FindControl("hdfStatus") as HiddenField).Value);
                                break;
                            }
                        }
                        grvEditRow = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        HiddenField hdfStatus = ((HiddenField)grvEditRow.FindControl("hdfStatus"));
                        Status = Convert.ToInt32((grvEditRow.FindControl("hdfStatus") as HiddenField).Value);
                        List<string> wkfsatus = new List<string> { "1", "2", "6", "7" }; //1.Submitted, 2.Approved(Final Approval), 6.Requested for more Info, 7.More Info Submitted
                        if (!wkfsatus.Contains(hdfStatus.Value)) // (Convert.ToInt16(hdfDelStatus.Value) == 1 || )
                        {
                            btnEditforCancel.Visible = false;
                        }
                        else
                        {
                            btnEditforCancel.Visible = true;
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        Status = 0;
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CWIPASSET);

                        FillProcessID();
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();

                        break;
                    #endregion
                    #region EDIT
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdCWIPAssetList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn != null && rbtn.Checked)
                            {
                                IsChecked = true;
                                CurrPK = Convert.ToInt32((grdrow.FindControl("hdfCWIPAssetPK") as HiddenField).Value);
                                Status = Convert.ToInt32((grdrow.FindControl("hdfStatus") as HiddenField).Value);
                                break;
                            }
                        }

                        if (!IsChecked)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }

                        EntryStatus = EntryStatus.ENTRYMODE;
                        FillProcessID();
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        ucrWrkf.ViewAction();

                        GetFieldValues(ControlsEnum.ACCGROUP);
                        SetFieldValues(ControlsEnum.ACCGROUP);
                        GetFieldValues(ControlsEnum.CWIPASSET);
                        SetFieldValues(ControlsEnum.CWIPASSET);

                        //if(Status == 2)//stats-2-approved
                        //    ddlCWIPAccount.Enabled = false;

                        break;
                    #endregion
                    #region VIEW
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdCWIPAssetList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn != null && rbtn.Checked)
                            {
                                IsChecked = true;
                                CurrPK = Convert.ToInt32((grdrow.FindControl("hdfCWIPAssetPK") as HiddenField).Value);
                                Status = Convert.ToInt32((grdrow.FindControl("hdfStatus") as HiddenField).Value);
                                break;
                            }
                        }

                        if (!IsChecked)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }

                        EntryStatus = EntryStatus.VIEWMODE;
                        //FillProcessID();
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
                        GetFieldValues(ControlsEnum.ACCGROUP);
                        SetFieldValues(ControlsEnum.ACCGROUP);
                        GetFieldValues(ControlsEnum.CWIPASSET);
                        SetFieldValues(ControlsEnum.CWIPASSET);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = BusinessLogic.Finance.CommSetupBL.DeleteCWIPAsset(CurrPK);
                        if (result > 0)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_DeleteSuccess").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                            ResetForm(ControlsEnum.CWIPASSET);
                            EntryStatus = EntryStatus.LISTMODE;
                            GetFieldValues(ControlsEnum.SEARCH);
                            SetFieldValues(ControlsEnum.SEARCH);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.SQLERROR)
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region LIST
                    case ActionsEnum.LIST:
                        FillProcessID();
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        break;
                    #endregion
                    #region DETAILS
                    case ActionsEnum.DETAILS:
                        foreach (GridViewRow grdrow in grdCWIPAssetList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn != null && rbtn.Checked)
                            {
                                IsChecked = true;
                                CurrPK = Convert.ToInt32((grdrow.FindControl("hdfCWIPAssetPK") as HiddenField).Value);
                                Status = Convert.ToInt32((grdrow.FindControl("hdfStatus") as HiddenField).Value);
                                break;
                            }
                        }

                        if (!IsChecked)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }

                        EntryStatus = EntryStatus.ENTRYMODE;
                        FillProcessID();
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        ucrWrkf.ViewAction();

                        GetFieldValues(ControlsEnum.ACCGROUP);
                        SetFieldValues(ControlsEnum.ACCGROUP);
                        GetFieldValues(ControlsEnum.CWIPASSET);
                        SetFieldValues(ControlsEnum.CWIPASSET);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.SEARCH);
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        break;
                    #endregion
                    #region ADDSELECTION
                    case ActionsEnum.ADDSELECTION:
                        int selectedItemId = 0;
                        grvEditRow = (((CheckBox)sender).Parent.Parent as GridViewRow);
                        CheckBox chkInvoice = (CheckBox)grvEditRow.FindControl("chkInvoice");
                        HiddenField hdfInvoicePK = (HiddenField)grvEditRow.FindControl("hdfInvoice");
                        Label lblDebit = (Label)grvEditRow.FindControl("lblDrAmount");
                        Label lblCredit = (Label)grvEditRow.FindControl("lblCrAmount");
                        
                        //selectedItemId = Convert.ToInt32(hdfInvoicePK.Value);
                        //if (SelectedList == null)
                        //    SelectedList = new List<int>();
                        //if (SelectedList.Contains(selectedItemId) && chkInvoice.Checked == false)
                        //    SelectedList.Remove(selectedItemId);
                        //else
                        //    SelectedList.Add(selectedItemId);

                        if (chkInvoice.Checked)
                        {
                            CWIPAssetHeader.TranDetails.Where(w => w.FTR_PK == Convert.ToInt32(hdfInvoicePK.Value)).FirstOrDefault().FTR_SELECTED = 1;
                            //DebitTotal = (!string.IsNullOrEmpty(lblDebit.Text) && lblDebit.Text != "0.00") ? (DebitTotal + (!string.IsNullOrEmpty(lblDebit.Text) ?Convert.ToDouble(lblDebit.Text):0)): DebitTotal;
                            //CreditTotal = (!string.IsNullOrEmpty(lblCredit.Text) && lblCredit.Text != "0.00") ? (CreditTotal + (!string.IsNullOrEmpty(lblCredit.Text) ? Convert.ToDouble(lblCredit.Text):0)): CreditTotal;
                        }
                        else
                        {
                            CWIPAssetHeader.TranDetails.Where(w => w.FTR_PK == Convert.ToInt32(hdfInvoicePK.Value)).FirstOrDefault().FTR_SELECTED = 0;
                            //DebitTotal = (!string.IsNullOrEmpty(lblDebit.Text) && lblDebit.Text != "0.00") ? (DebitTotal - (!string.IsNullOrEmpty(lblDebit.Text) ? Convert.ToDouble(lblDebit.Text) : 0)) : DebitTotal;
                            //CreditTotal = (!string.IsNullOrEmpty(lblCredit.Text) && lblCredit.Text != "0.00") ? (CreditTotal - (!string.IsNullOrEmpty(lblCredit.Text) ? Convert.ToDouble(lblCredit.Text) : 0)) : CreditTotal;
                        }

                        //assetCostDefference = (DebitTotal- CreditTotal);
                        //if(CWIPAssetHeader.TranDetails.Count(c =>c.FTR_SELECTED ==1) > 0)
                        //{
                        //    txtAssetCost.Text = assetCostDefference.ToString();
                        //}
                        //else
                        //{
                        //    if (CWIPAssetHeader != null && CWIPAssetHeader.TranDetails != null && CWIPAssetHeader.TranDetails.Any())
                        //    txtAssetCost.Text = CWIPAssetHeader.AssetCost.ToString("C");
                        //}
                        DebitAmt = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1 && w.IsDebit == 1).Sum(s => s.TransNow);
                        CreditAmt = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1 && w.IsDebit == 0).Sum(s => s.TransNow);
                        CWIPAssetHeader.AssetCost = DebitAmt - CreditAmt;
                        txtAssetCost.Text = CWIPAssetHeader.AssetCost.ToString("C");

                        SelectAllFlag = true;
                        if (CWIPAssetHeader.TranDetails.Any(a => a.FTR_SELECTED == 0))
                            SelectAllFlag = false;

                        if (SelectAllFlag)
                            ((CheckBox)grdCOATransactions.HeaderRow.FindControl("chkSelectAllInvoice")).Checked = true;
                        else
                            ((CheckBox)grdCOATransactions.HeaderRow.FindControl("chkSelectAllInvoice")).Checked = false;

                        break;
                    #endregion
                    #region SELECTALL
                    case ActionsEnum.SELECTALL:
                        CheckBox cbHeader = (CheckBox)grdCOATransactions.HeaderRow.FindControl("chkSelectAllInvoice");
                        SelectAllFlag = cbHeader.Checked;
                        if (SelectAllFlag)
                            CWIPAssetHeader.TranDetails.ForEach(f => f.FTR_SELECTED = 1);
                        else
                            CWIPAssetHeader.TranDetails.ForEach(f => f.FTR_SELECTED = 0);
                        BindGrid(ControlsEnum.TRANSACTIONS);
                        if (SelectAllFlag)
                        {
                            ((CheckBox)grdCOATransactions.HeaderRow.FindControl("chkSelectAllInvoice")).Checked = true;
                            //DebitTotal = CWIPAssetHeader.TranDetails.Sum(x => Convert.ToDouble(x.DebitAmtBC));
                            //CreditTotal = CWIPAssetHeader.TranDetails.Sum(x => Convert.ToDouble(x.CreditAmtBC));
                            DebitAmt = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1 && w.IsDebit == 1).Sum(s => s.TransNow);
                            CreditAmt = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1 && w.IsDebit == 0).Sum(s => s.TransNow);
                            CWIPAssetHeader.AssetCost = DebitAmt - CreditAmt;
                            //assetCostDefference = (DebitTotal - CreditTotal);
                            txtAssetCost.Text = CWIPAssetHeader.AssetCost.ToString("C");
                        }
                        else
                        {
                            //DebitTotal = CreditTotal = 0;
                        }

                        break;
                    #endregion
                    #region CHANGEVALUE - ddlCWIPAccount
                    case ActionsEnum.CHANGEVALUE:
                        GetFieldValues(ControlsEnum.TRANSACTIONS);
                        //Partial Save Added
                        //if (CWIPAssetHeader.AccountExist == 1)
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("AccAlreadyExist").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
                        SetFieldValues(ControlsEnum.TRANSACTIONS);
                        //GetFieldValues(ControlsEnum.FCREXCHANGERATEINBASECURRENCY);
                        //SetFieldValues(ControlsEnum.FCREXCHANGERATEINBASECURRENCY);
                        //FillddlBankCharge();
                        break;
                    #endregion
                    #region REFRESH
                    case ActionsEnum.REFRESH:
                        GetFieldValues(ControlsEnum.TRANSACTIONS);
                        //if (CWIPAssetHeader.VoucherExist == 1)
                        //{
                        //    litErrorMsg.Text = GetLocalResourceObject("VoucherExist").ToString();
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}
                        SetFieldValues(ControlsEnum.TRANSACTIONS);
                        hdfRefreshClicked.Value = "1";
                        CWIPAssetHeader.Refresh = 1;
                        break;
                    case ActionsEnum.SEARCHPO:
                        GetFieldValues(ControlsEnum.TRANSACTIONS);
                        SetFieldValues(ControlsEnum.TRANSACTIONS);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        this.btnNew.Focus();
                        FillProcessID();
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        ResetForm(ControlsEnum.CWIPASSET);
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
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
                            if (!IsCheckedAny())
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_CheckAnyItem").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                return;
                            }
                            SetUIValuesToObject(ControlsEnum.SAVE);
                            CWIPAssetHeader.WKF_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                            CWIPAssetHeader.USER_PK = currentUser.PKUser;
                            // save Process Control inspection details
                            string xmlDoc = CommonFunctions.XmlSerialize<CWIPAsset>(CWIPAssetHeader);
                            result = BusinessLogic.Finance.CommSetupBL.SaveCWIPAssetWkf(xmlDoc, out retRefID, out transNo);
                            if (result > 0)
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "');", true);
                                ResetForm(ControlsEnum.CWIPASSET);
                                EntryStatus = EntryStatus.LISTMODE;
                                GetFieldValues(ControlsEnum.SEARCH);
                                SetFieldValues(ControlsEnum.SEARCH);
                            }
                            else
                            {
                                if (result == (int)DbSaveStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("AccAlreadyExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.DATEOVERLAP)//Cost differ & depr exist
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("DeprExist").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbSaveStatus.ALREADYDELETED)//-5 Cost difference & voucher exist
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("VoucherExistCannotRefresh").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT popup
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKFSUBMIT
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
                            if (!IsCheckedAny())
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_CheckAnyItem").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                return;
                            }
                            ucrWrkf.ApplicationID = 0;
                            SetUIValuesToObject(ControlsEnum.SAVE);
                            CWIPAssetHeader.WKF_FLAG = 1;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                SaveTransaction(CWIPAssetHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT), sender);
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL) //DELETESUBMIT
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.CWIP))
                                {
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);
                                }
                                else
                                {
                                    ResetForm(ControlsEnum.SAVE);
                                    litErrorMsg.Text = GetLocalResourceObject("Err_CWIP_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    GetFieldValues(ControlsEnum.SEARCH);
                                    SetFieldValues(ControlsEnum.SEARCH);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT), sender);
                        }
                        break;
                    #endregion
                    #region SELECTEDINDEXCHANGED
                    case ActionsEnum.SELECTEDINDEXCHANGED:
                        //GetFieldValues(ControlsEnum.CWIPACCOUNT);
                        //SetFieldValues(ControlsEnum.CWIPACCOUNT);
                        txtCWIPAccount.Text = "Select/Type";
                        hdfCWIPAccPK.Value = "0";
                        GetFieldValues(ControlsEnum.TRANSACTIONS);
                        SetFieldValues(ControlsEnum.TRANSACTIONS);
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdCWIPAssetList.Rows)
                        {
                            RadioButton rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn != null && rbtn.Checked)
                            {
                                IsChecked = true;
                                CurrPK = Convert.ToInt32((grdrow.FindControl("hdfCWIPAssetPK") as HiddenField).Value);
                                Status = Convert.ToInt32((grdrow.FindControl("hdfStatus") as HiddenField).Value);
                                break;
                            }
                        }

                        if (!IsChecked)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            return;
                        }

                        EntryStatus = EntryStatus.ENTRYMODE;
                        FillProcessID(1);
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        ucrWrkf.ViewAction();

                        GetFieldValues(ControlsEnum.ACCGROUP);
                        SetFieldValues(ControlsEnum.ACCGROUP);
                        GetFieldValues(ControlsEnum.CWIPASSET);
                        SetFieldValues(ControlsEnum.CWIPASSET);
                        break;
                    #endregion
                    #region DELETESUBMIT
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrJournalize.ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();                        
                        FillProcessID();
                        ResetForm(ControlsEnum.CWIPASSET);
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID();
                        ResetForm(ControlsEnum.CWIPASSET);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        break;
                    #endregion
                    #region JOURNALIZESUBMIT
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                            }
                            else if (Transaction == "DELETE")
                            {
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID();
                        ResetForm(ControlsEnum.CWIPASSET);

                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.SEARCH);
                        SetFieldValues(ControlsEnum.SEARCH);
                        break;
                    #endregion
                    #region CALCULATE
                    case ActionsEnum.CALCULATE:
                        var FTRPK = Convert.ToInt32(hdfTrnsRowPK.Value == "" ? "0" : hdfTrnsRowPK.Value);
                        var Amount = Convert.ToDecimal(hdfTrnsNowNew.Value);
                        if(CWIPAssetHeader.TranDetails!=null&& CWIPAssetHeader.TranDetails.Any())
                        {
                            if (Amount <= CWIPAssetHeader.TranDetails.Where(w => w.FTR_PK == FTRPK).FirstOrDefault().TransAmount)
                            {
                                CWIPAssetHeader.TranDetails.Where(w => w.FTR_PK == FTRPK).FirstOrDefault().TransNow = Amount;
                            }
                            else
                            {
                                foreach (GridViewRow grdrow in grdCOATransactions.Rows)
                                {
                                    HiddenField hdfInvoice = (HiddenField)grdrow.FindControl("hdfInvoice");
                                    if (hdfInvoice.Value == FTRPK.ToString())
                                    {
                                        ((TextBox)grdrow.FindControl("txtTrnsNow")).Text = CWIPAssetHeader.TranDetails.Where(w => w.FTR_PK == FTRPK).FirstOrDefault().TransAmount.ToString("c");
                                        break;
                                    }
                                }
                            }
                        }

                        hdfTrnsRowPK.Value = "0";

                        DebitAmt = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1 && w.IsDebit == 1).Sum(s => s.TransNow);
                        CreditAmt = CWIPAssetHeader.TranDetails.Where(w => w.FTR_SELECTED == 1 && w.IsDebit == 0).Sum(s => s.TransNow);
                        CWIPAssetHeader.AssetCost = DebitAmt - CreditAmt;
                        txtAssetCost.Text = CWIPAssetHeader.AssetCost.ToString("C");
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        ScriptManager.RegisterStartupScript(this.Page,
                            typeof(Page),
                            "Openwindow",
                            "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.CWIP) + "');",
                            true);
                        break;
                    #endregion
                    #region PRINTLISTING
                    case ActionsEnum.PRINTLISTING:
                        if (CurrPK == 0)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.Report.Msg_Not_Selected) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                        ScriptManager.RegisterStartupScript(this.Page,
                                typeof(Page),
                                "Openwindow",
                                "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.CWIP) + "');",
                                true);
                        break;
                        #endregion
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if ((sender as GridView).ID == "grdCWIPAssetList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button imgInvAmend = e.Row.FindControl("imgInvAmend") as Button;
                        imgInvAmend.CssClass = GetLocalResourceObject("InvAmendIcon").ToString();
                    }
                }
                if ((sender as GridView).ID == "grdCOATransactions")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Button imgInvAmend = e.Row.FindControl("imgInvAmend") as Button;
                        imgInvAmend.CssClass = GetLocalResourceObject("InvAmendIcon").ToString();
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            TextBox trnsNow = e.Row.FindControl("txtTrnsNow") as TextBox;
                            trnsNow.Enabled = false;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void ActionHandler(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdCOATransactions.DataSource = CWIPAssetHeader.TranDetails;
                grdCOATransactions.PageIndex = e.NewPageIndex;
                grdCOATransactions.DataBind();

                if (SelectAllFlag)
                    ((CheckBox)grdCOATransactions.HeaderRow.FindControl("chkSelectAllInvoice")).Checked = true;
                #region PartilSaveOld
                //foreach (GridViewRow row in grdCOATransactions.Rows)
                //{
                //    CheckBox chkInvoice = (CheckBox)row.FindControl("chkInvoice");
                //    HiddenField hdfInvoicePK = (HiddenField)row.FindControl("hdfInvoice");
                //    int Id = Convert.ToInt32(hdfInvoicePK.Value);
                //    if (SelectAllFlag)
                //    {
                //        chkInvoice.Checked = true;
                //        //SelectAllFlag = true;
                //    }
                //    else
                //    {
                //        if (SelectedList.Contains(Id))
                //        {
                //            chkInvoice.Checked = true;
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Pager Methods + Init
        /// <summary>
        /// Methord  For PageInite Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnWIPSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteCWIP.PreRender += new EventHandler(btnAction_PreRender);
            //btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            //btnView.PreRender += new EventHandler(btnAction_PreRender);
            //lnkList.PreRender += new EventHandler(btnAction_PreRender);
            //lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnWIPSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnDeleteCWIP.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            //btnEdit.Load += new EventHandler(btnAction_Load);
            //btnView.Load += new EventHandler(btnAction_Load);
            //lnkList.Load += new EventHandler(btnAction_Load);
            //lnkDetail.Load += new EventHandler(btnAction_Load);
        }

        /// <summary>
        /// Button Load event
        /// Resets visibility
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_Load(object sender, EventArgs e)
        {
            (sender as Control).Visible = true;
        }
        /// <summary>
        /// To handle the visibility of the action corresponding to the users Privilage
        /// Leave this section if using Master Screens
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAction_PreRender(object sender, EventArgs e)
        {
            base.CheckBtnVisibility(sender);
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
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage = 1;
                        break;
                    case NavigationEnum.LAST:
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage = e.TotalPages;
                        break;
                    case NavigationEnum.NEXT:
                        // increment the current page index.
                        if (e.CurrentPage <= e.TotalPages)
                            uclPaging.CurrentPage++;
                        break;
                    case NavigationEnum.PREVIOUS:
                        // Decrement the current page index.
                        if (e.CurrentPage > 1)
                            uclPaging.CurrentPage--;
                        break;
                }
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.SEARCH);
                SetFieldValues(ControlsEnum.SEARCH);
                EntryStatus = EntryStatus.LISTMODE;
                EnableDisableButtons(e.TotalPages);
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
            uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;
            uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
            uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
        }

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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                SetViewMode(EntryStatus);
                EnableDisableControls();
                ShowHideControls();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum
        /// </summary>
        public enum ControlsEnum
        {
            SEARCH,
            CLEARSEARCHS,
            CURRENCY,
            FCHold,
            CWIPACCOUNT,
            ACCGROUP,
            TRANSACTIONS,
            SAVE,
            CANCEL,
            CWIPASSET,
            CWIPACCOUNTSEARCH,
            JOURNALIZE,
            FINHEADER,
            GETINVOICEPKBYJOURNALPK,
           
        }

        private class workflowType
        {
            public const string Cancel = "1";
        }
        #endregion
    }
}