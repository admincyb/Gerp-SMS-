using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ERPSMS_v01.UserControls;
using BusinessObject.AccountManagement;
using System.Data;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using System.Threading;
using System.Text;
using System.IO;
using BusinessLogic.CommonManagement;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject;
using BusinessObject.HRMS.Payroll;
using BusinessObject.Common;
using BusinessObject.AssetService;
using BusinessLogic.AssetService;
using System.Drawing;


namespace ERPSMS_v01.AssetService
{
    public partial class AssetServiceOrderReceipt : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region  Properties
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

        /// <summary>
        /// To keep RowIndex in view state
        /// </summary>
        private int RowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }
        private int ItemRowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.ItemRowIndex] == null ? -1 : (int)this.ViewState[ViewstateStrings.ItemRowIndex];
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemRowIndex] = value;
            }
        }

        /// <summary>
        /// To keep popup RowIndex in view state
        /// </summary>
        private int RowIndexPopup
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndexPopup] == null ? -1 : (int)this.ViewState[ViewstateStrings.RowIndexPopup];
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndexPopup] = value;
            }
        }

        /// <summary>
        /// To keep popup display mode
        /// </summary>
        private bool PopupViewMode
        {
            get
            {
                return this.ViewState[ViewstateStrings.PopupViewMode] == null ? false : (bool)this.ViewState[ViewstateStrings.PopupViewMode];
            }
            set
            {
                this.ViewState[ViewstateStrings.PopupViewMode] = value;
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

        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private int PageProcessID
        {
            get
            {
                return this.ViewState["PageProcessID"] == null ? 0 : Convert.ToInt32(this.ViewState["PageProcessID"]);
            }
            set
            {
                this.ViewState["PageProcessID"] = value;
            }
        }

        /// <summary>
        /// Approved
        /// </summary>
        private int Approved
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.Approved]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Approved] = value;
            }
        }

        private int Status
        {
            get
            {
                return this.ViewState[ViewstateStrings.Status] == null ? 0 : Convert.ToInt32((this.ViewState[ViewstateStrings.Status]));
            }
            set
            {
                this.ViewState[ViewstateStrings.Status] = value;
            }
        }
        /// <summary>
        /// To maintain keep Asset ServiceOrderReceipt Header
        /// </summary>
        private AssetServiceOrderReceiptHeader AssetServiceOrderReceiptHeaderSession
        {
            get
            {
                return (AssetServiceOrderReceiptHeader)ViewState[ViewstateStrings.AssetServiceOrderReceiptHeader];
            }
            set
            {
                ViewState[ViewstateStrings.AssetServiceOrderReceiptHeader] = value;
            }
        }
        /// <summary>
        /// To maintain keep Temp Asset ServiceOrderReceiptReceipt Header
        /// </summary>
        private AssetServiceOrderReceiptHeader TempAssetServiceOrderReceiptHeaderSession
        {
            get
            {
                return (AssetServiceOrderReceiptHeader)ViewState["TempAssetServiceOrderReceiptHeaderSession"];
            }
            set
            {
                ViewState["TempAssetServiceOrderReceiptHeaderSession"] = value;
            }
        }
        /// <summary>
        /// To maintain keep Expense Header Tax Splitting
        /// </summary>
        private AssetServiceOrderReceiptHeader EditTempAssetServiceOrderReceiptHeaderSession
        {
            get
            {
                return (AssetServiceOrderReceiptHeader)this.ViewState["EditTempAssetServiceOrderReceiptHeaderSession"];
            }
            set
            {
                this.ViewState["EditTempAssetServiceOrderReceiptHeaderSession"] = value;
            }
        }
        /// <summary>
        /// To keep details in view state
        /// </summary>
        private List<AssetServiceOrderReceiptDetails> AssetServiceOrderReceiptDetailList
        {
            get
            {
                return (List<AssetServiceOrderReceiptDetails>)ViewState[ViewstateStrings.AssetServiceRequestDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.AssetServiceRequestDetailList] = value;
            }
        }
        /// <summary>
        /// To keep ServiceOrderReceiptReceiptDetailList Temporary in view state
        /// </summary>
        private List<AssetServiceOrderReceiptDetails> TempAssetServiceOrderReceiptDetailList
        {
            get
            {
                return (List<AssetServiceOrderReceiptDetails>)ViewState["TempAssetServiceOrderReceiptDetailList"];
            }
            set
            {
                ViewState["TempAssetServiceOrderReceiptDetailList"] = value;
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
        private int ITEM_SL_NO
        {
            get
            {
                return ViewState["ITEM_SL_NO"] == null ? 0 : (int)ViewState["ITEM_SL_NO"];
            }
            set
            {
                ViewState["ITEM_SL_NO"] = value;
            }
        }

        private int SID_SL_NO
        {
            get
            {
                return ViewState[ViewstateStrings.CurrDocSlNo] == null ? 0 : (int)ViewState[ViewstateStrings.CurrDocSlNo];
            }
            set
            {
                ViewState[ViewstateStrings.CurrDocSlNo] = value;
            }
        }
        /// <summary>
        /// Duty slip Detail PK
        /// </summary>
        private int OSDPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.PoDtlPK]);//[SRD_PK]
            }
            set
            {
                this.ViewState[ViewstateStrings.PoDtlPK] = value;
            }
        }
        /// <summary>
        /// Tax PK
        /// </summary>
        private int TaxPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.TaxPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TaxPK] = value;
            }
        }
        /// <summary>
        /// Property used for to store PedningPO List
        /// </summary>
        private List<PendingServiceOrder> PendingSOList
        {
            get
            {
                return this.ViewState["PendingSOList"] == null ? new List<PendingServiceOrder>() : (List<PendingServiceOrder>)(this.ViewState["PendingSOList"]);
            }
            set
            {
                this.ViewState["PendingSOList"] = value;
            }
        }
        /// <summary>
        /// To keep selected Service request Dtl PKs 
        /// </summary>
        private List<SelectedServiceOrdrDtls> SelectedOSDetailPKList
        {
            get
            {
                return this.ViewState["SelectedOSDetailPKList"] == null ? new List<SelectedServiceOrdrDtls>() : (List<SelectedServiceOrdrDtls>)(this.ViewState["SelectedOSDetailPKList"]);
            }
            set
            {
                this.ViewState["SelectedOSDetailPKList"] = value;
            }
        }
        private bool IsHeaderTax
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsHeaderTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsHeaderTax] = value;
            }
        }
        private string SelectedTaxText
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.SelectedTaxText];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedTaxText] = value;
            }
        }
        private bool IsEditMode
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.IsEditMode]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsEditMode] = value;
            }
        }
        /// <summary>
        /// To set custom tax config value
        /// </summary>
        private bool IsCustomTaxEnabled
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsCustomTaxEnabled] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsCustomTaxEnabled]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsCustomTaxEnabled] = value;
            }
        }
        #endregion

        #region  Variables
        private BusinessObject.AccountManagement.ActionsEnum commonActions;
        User currentUser;
        private DataTable dtResult;
        private DataTable dtCompany;
        private DataTable dtDept;
        DataTable dtTaxDetails;

        private string strResult = string.Empty;
        private string refID;
        private string inboxFlag;
        private string prefID;
        private int CompanyPk = 0;
        private int dptType;
        DataSet dsPageData;
        DataTable dtPageData;
        DataTable dtSO;
        private DataTable dtCustomTaxSet;
        private MultipleSOHeader MultipleSOHeaderObj;
        private AssetServiceOrderReceiptHeader objAssetServiceOrderReceiptHeader;
        private AssetServiceOrderReceiptDetails assetServiceOrderReceiptDetailsObj;
        private AssetServiceOrderReceiptItemDetails assetServiceOrderReceiptItemDetObj;
        List<AssetServiceOrderReceiptDetails> soDetailsList;
        List<AssetServiceOrderReceiptItemDetails> assetServiceOrderReceiptItemDetList;
        List<AssetServiceOrderReceiptTaxHdr> taxHdrList;
        private AssetServiceOrderReceiptTaxHdr soInvTaxHdrObj;
        AssetServiceOrderReceiptDetails soDetailsObj;
        AssetServiceOrderReceiptItemDetails soDetailsItemObj;
        List<SelectedServiceOrdrDtls> tempSelectedOSDetailPKList;
        GridViewRow grvRow;
        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };
        #endregion
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

        #endregion

        #region PageActionHandler
        /// <summary>
        /// Handle All page Related Events
        /// </summary>
        private void PageActionHandler()
        {
            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }

                ucrWrkf.ViewType = 1;
                // InitializeComponent();
                if (!IsPostBack)
                {
                    //Enable or disable custom tax 
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);
                    ConfigurationSettings();
                    PageIndex = Convert.ToInt16(CommonConstants.SELECT_VALUE_ONE);
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);                    
                    uclPaging.TotalPages = TotalPages;
                    uclPaging.CurrentPage = 1;
                    hdfDecimalFormat.Value = "#0.";
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperation.Value += "0";
                    }
                    hdfNumberDigits.Value = NoDecimalDigitsP2P.ToString();

                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfCurrencyFormatWithComma.Value = "#" + currencysep + "#0.";
                    hdfCurrencyFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithComma.Value += "0";
                    }
                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }

                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                       : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    refID = Request.QueryString[QueryStrings.RefID] != null ? Request.QueryString[QueryStrings.RefID]
                        : Session[ERP.Utilities.SessionStrings.RefID] != null ? Session[ERP.Utilities.SessionStrings.RefID].ToString().Split('=')[1] : string.Empty;
                    prefID = Request.QueryString[QueryStrings.PRefID] != null ? Request.QueryString[QueryStrings.PRefID]
                        : Session[ERP.Utilities.SessionStrings.PRefID] != null ? Session[ERP.Utilities.SessionStrings.PRefID].ToString().Split('=')[1] : string.Empty;
                    inboxFlag = Request.QueryString[QueryStrings.Flag] != null ? Request.QueryString[QueryStrings.Flag]
                    : Session[ERP.Utilities.SessionStrings.InboxFlag] != null ? Session[ERP.Utilities.SessionStrings.InboxFlag].ToString() : string.Empty;

                    ReferanceID = string.IsNullOrEmpty(refID)
                           ? string.IsNullOrEmpty(prefID)
                                 ? 0
                                 : int.Parse(prefID)
                           : int.Parse(refID);
                    FillProcessID(1);

                    hdfAdvSearch.Value = "0";

                    string[] datakeyarray;
                    datakeyarray = new string[2];
                    datakeyarray[0] = Resources.DataFieldRes.RSD_PK;
                    datakeyarray[1] = "RSD_SL_NO";
                    grdAssetDetails.DataKeyNames = datakeyarray;
                    AssetServiceOrderReceiptHeaderSession = new AssetServiceOrderReceiptHeader();
                    AssetServiceOrderReceiptDetailList = new List<AssetServiceOrderReceiptDetails>();

                    EntryStatus = EntryStatus.LISTMODE;
                    GetFieldValues(ControlsEnum.REQUESTINGSTORE);
                    SetFieldValues(ControlsEnum.REQUESTINGSTORE);
                    GetFieldValues(ControlsEnum.ASSETSERVICETYPE);
                    SetFieldValues(ControlsEnum.ASSETSERVICETYPE);
                    ddlRequestingStore.SelectedValue = currentUser.CurrentDeptPK.ToString();
                    #region Work Flow
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
                        ////start
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            base.WkfRefID = ucrWrkf.RefID;
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            if (pid.Equals("11"))
                                hdfIsCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }
                    }
                    //else if (!string.IsNullOrEmpty(prefID))
                    //{
                    //    ResetForm(ControlsEnum.CLEAR);
                    //    base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                    //    CurrPK = GetApplicationID(ucrWrkf.RefID);
                    //    // dptType = (int)DeptTypeEnum.HRMS;
                    //    GetFieldValues(ControlsEnum.DEPARTMENTBYTYPE);
                    //    if (dtDept != null && dtDept.Rows.Count > 0)
                    //    {
                    //        Session[BusinessObject.Common.SessionStrings.CurDept] = Convert.ToInt32(dtDept.Rows[0]["DPT_PK"]);
                    //        base.SetUserDept();
                    //    }
                    //}
                    if (CurrPK > 0)
                    {
                        EntryStatus = EntryStatus.ENTRYMODE;
                        FillProcessID(1);
                        WorkflowCore.CoreService objWorkflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = objWorkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        SetUIEditView(commonActions);
                        GetFieldValues(ControlsEnum.EDIT);
                        SetFieldValues(ControlsEnum.EDIT);
                        ResetForm(ControlsEnum.CLEARADD);
                        divSOPendingListing.Visible = true;
                        txtDate.Focus();
                    }
                    else
                    {
                        TempAssetServiceOrderReceiptHeaderSession = new AssetServiceOrderReceiptHeader();
                        EditTempAssetServiceOrderReceiptHeaderSession = new AssetServiceOrderReceiptHeader();
                        AssetServiceOrderReceiptHeaderSession = new AssetServiceOrderReceiptHeader();
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                    if (!string.IsNullOrEmpty(prefID))
                    {
                        DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetTransactionID(Convert.ToInt32(prefID));
                        if (dt.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(dt.Rows[0]["appPK"]);
                            FillTransactionData();
                        }
                        else
                        {
                            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                            DataTable dtApplication = wrkfService.GetApplicationID(int.Parse(prefID));
                            if (dtApplication != null)
                            {
                                if (dtApplication.Rows.Count > 0)
                                {
                                    OS_PK.Value = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                                    GetFieldValues(ControlsEnum.SOFROMINBOX);
                                    SetFieldValues(ControlsEnum.SOFROMINBOX);
                                    EntryStatus = EntryStatus.NEWMODE;
                                }
                            }
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        #endregion

        #region Helper Methods
        #region Get Field Values
        private void GetFieldValues(ControlsEnum type)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            BusinessObject.GridPrams gridParam;
            int vendorPK, storeId, serviceTypeId, sohPK = 0;
            DataSet dsTaxDetails;
            try
            {
                switch (type)
                {
                    #region LIST
                    case ControlsEnum.LIST:
                        gridParam = new BusinessObject.GridPrams();
                        gridParam.SearchBy = string.Empty;
                        gridParam.SearchValue = string.Empty;
                        gridParam.PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        gridParam.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        gridParam.FromDate = string.IsNullOrEmpty(txtListFromDate.Text) ? string.Empty : txtListFromDate.Text.Trim();
                        gridParam.ToDate = string.IsNullOrEmpty(txtListToDate.Text) ? string.Empty : txtListToDate.Text.Trim();
                        gridParam.FilterStatus = ddlStatus.SelectedValue == "-1" ? string.Empty : ddlStatus.SelectedValue;

                        string trxNo = txtListTrxNo.Text.Trim() == "Select/Type" ? string.Empty : txtListTrxNo.Text.Trim();
                        int VendorPK = txtListVendorName.Text.Trim() == "Select/Type" || txtListVendorName.Text.Trim() == string.Empty ? 0 : Convert.ToInt32(hdfListVendor.Value);
                        int serviceType = string.IsNullOrEmpty(ddlListServiceType.SelectedValue) ? 0 : Convert.ToInt32(ddlListServiceType.SelectedValue);
                        dtResult = BusinessLogic.AssetService.ServiceOrderReceiptBL.GetServiceOrderReceiptList(gridParam, currentUser, trxNo, VendorPK, serviceType);
                        break;
                    #endregion
        #endregion
                    #region REQUESTINGSTORE
                    case ControlsEnum.REQUESTINGSTORE:
                        dtPageData = BusinessLogic.AssetService.ServiceRequestBL.GetRequestingStores(currentUser, currentUser.SBUID, 0, 0);
                        break;
                    #endregion
                    #region ASSETSERVICETYPE
                    case ControlsEnum.ASSETSERVICETYPE:
                        dtPageData = BusinessLogic.AssetService.ServiceRequestBL.GetAssetServiceType(currentUser, 0, 1);
                        break;
                    #endregion
                    #region Edit
                    case ControlsEnum.EDIT:
                        objAssetServiceOrderReceiptHeader = ServiceOrderReceiptBL.GetServiceOrderReceiptByPK(CurrPK);
                        AssetServiceOrderReceiptHeaderSession = objAssetServiceOrderReceiptHeader;
                        break;
                    #endregion
                    #region Pending Requests
                    case ControlsEnum.PENDINGORDERS:
                        vendorPK = storeId = serviceTypeId = 0;
                        vendorPK = string.IsNullOrEmpty(hdfVendor.Value) ? 0 : Convert.ToInt32(hdfVendor.Value);
                        if (ddlRequestingStore.SelectedIndex > 0)
                            storeId = Convert.ToInt32(ddlRequestingStore.SelectedValue);
                        serviceTypeId = string.IsNullOrEmpty(ddlServiceType.SelectedValue) ? 0 : Convert.ToInt32(ddlServiceType.SelectedValue);
                        sohPK = CurrPK;
                        dsPageData = BusinessLogic.AssetService.ServiceOrderReceiptBL.GetServiceOrderPending(currentUser.SBUID, storeId, vendorPK, serviceTypeId, sohPK);
                        dtPageData = new DataTable();
                        dtPageData = dsPageData.Tables[0];
                        PendingSOList = dtPageData
                            .AsEnumerable()
                            .Select(x => new PendingServiceOrder
                            {
                                OSD_OSH_HDR = x.Field<int>(GTIService.Constants.AssetService.Fields.OSD_OSH_HDR),
                                OSD_PK = x.Field<int>(GTIService.Constants.AssetService.Fields.OSD_PK),
                                OSH_CURRENCY = x.Field<int>(GTIService.Constants.AssetService.Fields.OSH_CURRENCY),
                                OSH_CURRENCY_TEXT = x.Field<string>(GTIService.Constants.AssetService.Fields.OSH_CURRENCY_TEXT),
                                OSH_DEPT = x.Field<int>(GTIService.Constants.AssetService.Fields.OSH_DEPT),
                                OSH_DEPT_STORE = x.Field<int>(GTIService.Constants.AssetService.Fields.OSH_DEPT_STORE),  
                                OSD_OSH_NO = x.Field<string>(GTIService.Constants.AssetService.Fields.OSD_OSH_NO),
                                OSH_VENDOR_TEXT = x.Field<string>(GTIService.Constants.AssetService.Fields.OSH_VENDOR_TEXT),
                                OSH_VENDOR = x.Field<int>(GTIService.Constants.AssetService.Fields.OSH_VENDOR),
                                OSD_ASSET_TYPE_TEXT = x.Field<string>(GTIService.Constants.AssetService.Fields.OSD_ASSET_TYPE_TEXT),
                                OSH_SERVICE_TYPE = x.Field<Int16>(GTIService.Constants.AssetService.Fields.OSH_SERVICE_TYPE),
                                OSD_ASSET_TEXT = x.Field<string>(GTIService.Constants.AssetService.Fields.OSD_ASSET_TEXT),
                                OSD_AMOUNT = x.Field<decimal>(GTIService.Constants.AssetService.Fields.OSD_AMOUNT)
                            })
                            .ToList();
                        break;
                    #endregion
                    #region Selected Service Order Details
                    case ControlsEnum.SELECTEDSODETAILS:
                        List<AssetServiceOrderReceiptDetails> soDetailsLst = new List<AssetServiceOrderReceiptDetails>();                      
                        AssetServiceOrderReceiptDetails objSODetails;
                        AssetServiceOrderReceiptTaxHdr objSOTaxHdr;
                        List<AssetServiceOrderReceiptTaxHdr> soReceiptTaxHdrLst = new List<AssetServiceOrderReceiptTaxHdr>();

                        SelectedServiceOrderRoot objSelectedServiceOrderRoot = new SelectedServiceOrderRoot();
                        objSelectedServiceOrderRoot.OSDPKList = tempSelectedOSDetailPKList;
                        string xmlDoc = CommonFunctions.XmlSerialize<SelectedServiceOrderRoot>(objSelectedServiceOrderRoot);                      
                        MultipleSOHeaderObj = ServiceOrderReceiptBL.GetSelectedServiceOrderDetail(xmlDoc);
                        if (MultipleSOHeaderObj != null && MultipleSOHeaderObj.MultipleOSList != null && MultipleSOHeaderObj.MultipleOSList.Count > 0)
                        {                           
                            foreach (AssetServiceOrderReceiptHeader objOS in MultipleSOHeaderObj.MultipleOSList)
                            {
                                #region SOReceiptDetails
                                foreach (AssetServiceOrderReceiptDetails tDtl in objOS.Details)
                                {
                                    objSODetails = new AssetServiceOrderReceiptDetails();
                                    objSODetails = tDtl.DeepClone();
                                    soDetailsLst.Add(objSODetails);
                                }
                                #endregion
                                #region AssetServiceOrderReceiptTaxHdr
                                foreach (AssetServiceOrderReceiptTaxHdr tTax in objOS.Tax_HDR)
                                {
                                    objSOTaxHdr = new AssetServiceOrderReceiptTaxHdr();
                                    objSOTaxHdr = tTax.DeepClone();
                                    soReceiptTaxHdrLst.Add(objSOTaxHdr);
                                }

                                #endregion
                            }
                            soDetailsLst.ForEach(dtl =>
                            {
                                if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                {
                                    dtl.Item_details.ForEach(itm => itm.RID_NET_AMOUNT = itm.RID_AMOUNT - itm.RID_DISCOUNT + itm.RID_TAX);
                                }
                            });
                            //AssetServiceOrderReceiptDetailList = soDetailsLst;
                            if (AssetServiceOrderReceiptDetailList != null && AssetServiceOrderReceiptDetailList.Count > 0)
                            {
                                AssetServiceOrderReceiptDetailList.AddRange(soDetailsLst);
                            }
                            else
                            {
                                AssetServiceOrderReceiptDetailList = soDetailsLst;
                            }

                            #region Resetting the slno,because there is chance for duplicating slno
                            int newSlno = 1;
                            AssetServiceOrderReceiptDetailList.ForEach(dtl =>
                            {
                                dtl.RSD_SL_NO = newSlno;
                                if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                {
                                    #region Setting Item Slno (RID_SL_NO only)
                                    foreach (AssetServiceOrderReceiptItemDetails itm in dtl.Item_details)
                                    {
                                        itm.RID_SL_NO = newSlno;
                                    }
                                    #endregion
                                }
                                newSlno++;
                            });
                            #endregion
                          
                            #region Group Tax/discount/OtherCharge
                            if (soReceiptTaxHdrLst.Count > 0)
                            {
                                var groupedTaxHdrList = soReceiptTaxHdrLst.GroupBy(f => new { f.TRD_TAX, f.TRD_TAX_CATEGORY })
                                .Select(grp => new AssetServiceOrderReceiptTaxHdr
                                {
                                    TRD_RID_PK = grp.Min(p => p.TRD_RID_PK),
                                    TRD_TAX_AMT = grp.Sum(p => p.TRD_TAX_AMT),
                                    TRD_NAME = grp.Min(p => p.TRD_NAME),
                                    TRD_TAX_TEXT = grp.Min(p => p.TRD_TAX_TEXT),
                                    TRD_PK = grp.Min(p => p.TRD_PK),                                  
                                    TRD_SL_NO = grp.Min(p => p.TRD_SL_NO),
                                    TRD_TAX = grp.Min(p => p.TRD_TAX),
                                    TRD_TAX_CATEGORY = grp.Min(p => p.TRD_TAX_CATEGORY),
                                    TRD_TAX_CATEGORY_TEXT = grp.Min(p => p.TRD_TAX_CATEGORY_TEXT),
                                    TRD_TAX_FORMULA = grp.Min(p => p.TRD_TAX_FORMULA),
                                    TRD_TYPE = grp.Min(p => p.TRD_TYPE)
                                })
                               .ToList();
                                soReceiptTaxHdrLst = groupedTaxHdrList;
                            } 
                            #endregion

                            if (AssetServiceOrderReceiptHeaderSession == null)
                                AssetServiceOrderReceiptHeaderSession = new AssetServiceOrderReceiptHeader();
                            AssetServiceOrderReceiptHeaderSession.Details = AssetServiceOrderReceiptDetailList;
                            AssetServiceOrderReceiptHeaderSession.Tax_HDR = soReceiptTaxHdrLst;
                        }
                        break;
                    #endregion
                    #region TAXTYPES
                    case ControlsEnum.TAXTYPES:
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsTaxDetails = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQTaxDetails(TaxPK, category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK), 0);
                            if (dsTaxDetails != null && dsTaxDetails.Tables.Count > 0)
                            {
                                dtTaxDetails = dsTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            if ((int)TaxType.Tax == category)
                            {
                                //dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtExpenseDate.Text), 0, TaxFilterType.PUR, (int)TaxStatus.Include);
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtDate.Text), 0, null, (int)TaxStatus.Include, 1);
                            }
                            else
                            {
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtDate.Text), 0, null, (int)TaxStatus.Include);
                            }
                        }
                        break;
                    #endregion
                    #region CUSTOM TAX SETTINGS
                    case ControlsEnum.CUSTOMTAXSETTINGS:
                        IsCustomTaxEnabled = true;
                        dtCustomTaxSet = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("CUSTOM TAX SETTINGS", "TAX REQUIRED");
                        if (dtCustomTaxSet != null && dtCustomTaxSet.Rows.Count > 0)
                        {
                            int cfgval = Convert.ToInt32(dtCustomTaxSet.Rows[0]["ACF_VALUE"]);
                            if (cfgval == 0)
                            {
                                IsCustomTaxEnabled = false;
                            }
                        }
                        break;
                    #endregion
                    #region ITEMDETAILS
                    case ControlsEnum.ITEMDETAILS:
                        int itemPk = string.IsNullOrEmpty(hdfPopupItemPk.Value) ? 0 : Convert.ToInt32(hdfPopupItemPk.Value);
                        dtPageData = CommonBL.GetAssetItemsAuto(string.Empty, 0, itemPk, 2, currentUser.SBUID);
                        break;
                    #endregion
                    #region SOFROMINBOX
                    case ControlsEnum.SOFROMINBOX:
                        dtSO = BusinessLogic.AssetService.ServiceOrderReceiptBL.GetSOVendorStoreDetails(GetNullableInt(OS_PK.Value) ?? 0);
                        break;
                    #endregion
                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            hdfExchangeRate.Value = "-1";
                        }
                        break; 
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
            finally { }
        }
        #endregion
        #region Set Field Values
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
                    #region CURRENCY
                    case ControlsEnum.CURRENCY:
                        GetUIValuesFromObject(ControlsEnum.CURRENCY);
                        break;
                    #endregion
                    case ControlsEnum.REQUESTINGSTORE:
                        BindDropDown(ControlsEnum.REQUESTINGSTORE);
                        break;
                    case ControlsEnum.ASSETSERVICETYPE:
                        BindDropDown(ControlsEnum.ASSETSERVICETYPE);
                        break;
                    case ControlsEnum.ASSETSERVICEORDERRECEIPTDTL:
                        BindGrid(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                        break;
                    case ControlsEnum.ASSETSERVICEORDERRECEIPT_ITEMDTL:
                        BindGrid(ControlsEnum.ASSETSERVICEORDERRECEIPT_ITEMDTL);
                        break;
                    case ControlsEnum.ITEMDETAILS:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            txtPopupUOM.Text = dtPageData.Rows[0]["ATM_UOM_TEXT"].ToString();
                            hdfPopupUomPk.Value = dtPageData.Rows[0]["ATM_UOM"].ToString();
                        }
                        break;
                    #region EDIT
                    case ControlsEnum.EDIT:
                        GetUIValuesFromObject(ControlsEnum.EDIT);
                        break;
                    #endregion
                    #region PENDINGORDERS
                    case ControlsEnum.PENDINGORDERS:
                        BindGrid(ControlsEnum.PENDINGORDERS);
                        break;
                    #endregion
                    #region SELECTEDSODETAILS
                    case ControlsEnum.SELECTEDSODETAILS:
                        BindGrid(ControlsEnum.SELECTEDSODETAILS);
                        break;
                    #endregion
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXTYPES:
                        BindDropDown(controlType);
                        break;
                    #region POSFROMINBOX
                    case ControlsEnum.SOFROMINBOX:
                        txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        if (dtSO != null && dtSO.Rows.Count > 0)
                        {
                            lblTrxNo.Text = GetGlobalResourceObject("Messages", "DocGenerationNew").ToString();
                            txtVendorName.Text =  HttpUtility.HtmlDecode(dtSO.Rows[0]["OSH_VENDOR_TEXT"].ToString());
                            hdfVendor.Value = dtSO.Rows[0]["OSH_VENDOR"].ToString();
                            ddlRequestingStore.SelectedValue = dtSO.Rows[0]["OSH_DEPT_STORE"].ToString();
                            ddlServiceType.SelectedValue = dtSO.Rows[0]["OSH_SERVICE_TYPE"].ToString();
                            //Disabling Vendor,Servicetype & Requesting Store Controls                           
                            hdfDisableVendorStoreSType.Value = "1"; 

                            ActionHandler(ddlRequestingStore, EventArgs.Empty);
                            int OSHPk;
                            OSHPk = GetNullableInt(dtSO.Rows[0]["OSH_PK"].ToString()) ?? 0;
                            foreach (GridViewRow grdrow in grdPendingSOItems.Rows)
                            {
                                int srId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSH_PK")).Value);
                                if (srId == OSHPk)
                                {
                                    CheckBox chk;
                                    chk = (CheckBox)grdrow.FindControl("chkSelectSOList");
                                    chk.Checked = true;
                                }
                            }
                            ActionHandler(btnAddToSOList, EventArgs.Empty);
                        }

                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Set UIValues To Object
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (controlType)
                {
                    #region AssetServiceOrderReceiptHDR
                    case ControlsEnum.ASSETSERVICEORDERRECEIPTHDR:
                        if (AssetServiceOrderReceiptHeaderSession != null)
                        {
                            objAssetServiceOrderReceiptHeader = AssetServiceOrderReceiptHeaderSession;
                            objAssetServiceOrderReceiptHeader.RSH_PK = CurrPK;
                            objAssetServiceOrderReceiptHeader.RSH_NO = lblTrxNo.Text;
                            objAssetServiceOrderReceiptHeader.RSH_DATE = Convert.ToDateTime(txtDate.Text);
                            objAssetServiceOrderReceiptHeader.RSH_VENDOR = String.IsNullOrEmpty(hdfVendor.Value) ? 0 : Convert.ToInt32(hdfVendor.Value);
                            objAssetServiceOrderReceiptHeader.RSH_SERVICE_TYPE = String.IsNullOrEmpty(ddlServiceType.SelectedValue) ? 0 : Convert.ToInt32(ddlServiceType.SelectedValue);
                            objAssetServiceOrderReceiptHeader.RSH_REMARKS = HttpUtility.HtmlEncode(txtWorkDescription.Text);
                            objAssetServiceOrderReceiptHeader.RSH_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                            objAssetServiceOrderReceiptHeader.RSH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            objAssetServiceOrderReceiptHeader.RSH_DEPT_STORE = Convert.ToInt16(ddlRequestingStore.SelectedValue);
                            objAssetServiceOrderReceiptHeader.RSH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);

                            objAssetServiceOrderReceiptHeader.RSH_SUB_TOTAL = string.IsNullOrEmpty(txtHdrSubTotal.Text) ? 0 : Convert.ToDouble(txtHdrSubTotal.Text);
                            objAssetServiceOrderReceiptHeader.RSH_DISCOUNT = string.IsNullOrEmpty(txtHdrDiscount.Text) ? 0 : Convert.ToDouble(txtHdrDiscount.Text);
                            objAssetServiceOrderReceiptHeader.RSH_TAX = string.IsNullOrEmpty(txtHdrTax.Text) ? 0 : Convert.ToDouble(txtHdrTax.Text);
                            objAssetServiceOrderReceiptHeader.RSH_OTH_CHARGE = string.IsNullOrEmpty(txtHdrOtherCharges.Text) ? 0 : Convert.ToDouble(txtHdrOtherCharges.Text);
                            objAssetServiceOrderReceiptHeader.RSH_PRICE_ADJ = string.IsNullOrEmpty(txtHdrPriceAdj.Text) ? 0 : Convert.ToDouble(txtHdrPriceAdj.Text);
                            objAssetServiceOrderReceiptHeader.RSH_NET_TOTAL = string.IsNullOrEmpty(txtHdrGrandTotal.Text) ? 0 : Convert.ToDouble(txtHdrGrandTotal.Text);//GrandTotal
                            objAssetServiceOrderReceiptHeader.RSH_CURRENCY_BC = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            objAssetServiceOrderReceiptHeader.RSH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                            objAssetServiceOrderReceiptHeader.RSH_NET_TOTAL_BC = objAssetServiceOrderReceiptHeader.RSH_NET_TOTAL * objAssetServiceOrderReceiptHeader.RSH_EXCHG_RATE;

                            objAssetServiceOrderReceiptHeader.RSH_INVOICE_NO = txtInvoiceNO.Text;                           
                            if (String.IsNullOrEmpty(txtInvoiceDate.Text.Trim()))
                                objAssetServiceOrderReceiptHeader.RSH_INVOICE_DATE = null;
                            else
                                objAssetServiceOrderReceiptHeader.RSH_INVOICE_DATE = txtInvoiceDate.Text.Trim();

                            objAssetServiceOrderReceiptHeader.RSH_BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                            objAssetServiceOrderReceiptHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                            objAssetServiceOrderReceiptHeader.RSH_CRTD_BY = Convert.ToInt32(currentUser.PKUser);
                            objAssetServiceOrderReceiptHeader.RSH_CRTD_DT = DateTime.Now;
                            objAssetServiceOrderReceiptHeader.LAST_MOD_DT = LastModifiedTime;

                            AssetServiceOrderReceiptDetailList.ForEach(dtl =>
                            {
                                double osdAmount = 0;
                                if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                {
                                    osdAmount = dtl.Item_details.Sum(r => r.RID_NET_AMOUNT);
                                    #region Setting Tax Slno
                                    foreach (AssetServiceOrderReceiptItemDetails itm in dtl.Item_details)
                                    {
                                        if (itm.Tax_DTL != null && itm.Tax_DTL.Count > 0)
                                            itm.Tax_DTL.ForEach(f => f.TRD_ITEM_SL_NO = itm.RID_ITEM_SL_NO);
                                    }
                                    #endregion
                                }
                                dtl.RSD_AMOUNT = osdAmount;
                                //For Encoding
                                dtl.RSD_ASSET_TYPE_TEXT = HttpUtility.HtmlEncode(dtl.RSD_ASSET_TYPE_TEXT);
                                dtl.RSD_ASSET_TEXT = HttpUtility.HtmlEncode(dtl.RSD_ASSET_TEXT);
                            });
                            objAssetServiceOrderReceiptHeader.Details = AssetServiceOrderReceiptDetailList;
                        }
                        retObject = objAssetServiceOrderReceiptHeader;
                        break;
                    #endregion
                    #region AssetServiceOrderReceiptDTL
                    case ControlsEnum.ASSETSERVICEORDERRECEIPTDTL:
                        if (CurrSlNo != 0 && AssetServiceOrderReceiptDetailList != null)
                        {
                            assetServiceOrderReceiptDetailsObj = AssetServiceOrderReceiptDetailList.SingleOrDefault(itm => itm.RSD_SL_NO == CurrSlNo);
                            if (assetServiceOrderReceiptDetailsObj != null)
                            {
                                assetServiceOrderReceiptDetailsObj.RSD_PK = OSDPK;
                                assetServiceOrderReceiptDetailsObj.RSD_RSH_HDR = CurrPK;
                                assetServiceOrderReceiptDetailsObj.RSD_ASSET_TYPE = Convert.ToInt32(hdfAssetType.Value);
                                assetServiceOrderReceiptDetailsObj.RSD_ASSET_TYPE_TEXT = HttpUtility.HtmlEncode(txtAssetType.Text);
                                assetServiceOrderReceiptDetailsObj.RSD_ASSET = Convert.ToInt32(hdfAsset.Value);
                                assetServiceOrderReceiptDetailsObj.RSD_ASSET_TEXT = HttpUtility.HtmlEncode(txtAsset.Text);
                                assetServiceOrderReceiptDetailsObj.RSD_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                                assetServiceOrderReceiptDetailsObj.RSD_RSH_NO = lblTrxNo.Text;
                                assetServiceOrderReceiptDetailsObj.RSD_RSH_DATE = Convert.ToDateTime(txtDate.Text);

                                retObject = assetServiceOrderReceiptDetailsObj;
                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (AssetServiceOrderReceiptDetailList == null || AssetServiceOrderReceiptDetailList.Count == 0)
                            {
                                AssetServiceOrderReceiptDetailList = new List<AssetServiceOrderReceiptDetails>();
                                slno = 1;
                            }
                            else
                            {
                                slno = AssetServiceOrderReceiptDetailList.Max(itm => itm.RSD_SL_NO);
                                slno++;
                            }
                            assetServiceOrderReceiptDetailsObj = new AssetServiceOrderReceiptDetails();
                            CurrSlNo = assetServiceOrderReceiptDetailsObj.RSD_SL_NO = slno;
                            assetServiceOrderReceiptDetailsObj.RSD_RSH_HDR = CurrPK;
                            assetServiceOrderReceiptDetailsObj.RSD_PK = 0;
                            assetServiceOrderReceiptDetailsObj.RSD_ASSET_TYPE = Convert.ToInt32(hdfAssetType.Value);
                            assetServiceOrderReceiptDetailsObj.RSD_ASSET_TYPE_TEXT = HttpUtility.HtmlEncode(txtAssetType.Text);
                            assetServiceOrderReceiptDetailsObj.RSD_ASSET = Convert.ToInt32(hdfAsset.Value);
                            assetServiceOrderReceiptDetailsObj.RSD_ASSET_TEXT = HttpUtility.HtmlEncode(txtAsset.Text);
                            assetServiceOrderReceiptDetailsObj.RSD_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                            assetServiceOrderReceiptDetailsObj.RSD_RSH_NO = lblTrxNo.Text;
                            assetServiceOrderReceiptDetailsObj.RSD_RSH_DATE = Convert.ToDateTime(txtDate.Text);

                            AssetServiceOrderReceiptDetailList.Add(assetServiceOrderReceiptDetailsObj);
                        }
                        retObject = AssetServiceOrderReceiptDetailList;
                        break;
                    #endregion
                    #region AssetServiceOrderReceipt_ITEMDTL
                    case ControlsEnum.ASSETSERVICEORDERRECEIPT_ITEMDTL:
                        if (AssetServiceOrderReceiptDetailList != null)
                        {
                            if (CurrSlNo != 0)
                            {
                                AssetServiceOrderReceiptItemDetails ItemDetailsObj;
                                AssetServiceOrderReceiptDetails assetServiceOrderReceiptDetailsObj = AssetServiceOrderReceiptDetailList.SingleOrDefault(itm => itm.RSD_SL_NO == CurrSlNo);
                                if (assetServiceOrderReceiptDetailsObj.Item_details == null)
                                {
                                    assetServiceOrderReceiptDetailsObj.Item_details = new List<AssetServiceOrderReceiptItemDetails>();
                                }
                                if (assetServiceOrderReceiptDetailsObj.Item_details.Count > 0 && ItemRowIndex >= 0)
                                {
                                    ItemDetailsObj = assetServiceOrderReceiptDetailsObj.Item_details[ItemRowIndex];
                                    ItemDetailsObj.RID_ASR_ITEM = string.IsNullOrEmpty(hdfPopupItemPk.Value) ? 0 : Convert.ToInt32(hdfPopupItemPk.Value);
                                    ItemDetailsObj.RID_ASR_ITEM_TEXT = txtPopupItems.Text;
                                    ItemDetailsObj.RID_QTY = string.IsNullOrEmpty(txtPopupQty.Text) ? 0 : Convert.ToDouble(txtPopupQty.Text);
                                    ItemDetailsObj.RID_RATE = Math.Round(Convert.ToDouble(txtPopupRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                                     ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P])));
                                    ItemDetailsObj.RID_UOM = string.IsNullOrEmpty(hdfPopupUomPk.Value) ? 0 : Convert.ToInt32(hdfPopupUomPk.Value);
                                    ItemDetailsObj.RID_UOM_TEXT = txtPopupUOM.Text;
                                    ItemDetailsObj.RID_AMOUNT = string.IsNullOrEmpty(txtPopupAmount.Text) ? 0 : Convert.ToDouble(txtPopupAmount.Text);
                                    ItemDetailsObj.RID_DISCOUNT = !string.IsNullOrEmpty(txtPopupDiscount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtPopupDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                    ItemDetailsObj.RID_TAX = !string.IsNullOrEmpty(txtPopupTax.Text.Trim()) ? Math.Round(Convert.ToDouble(txtPopupTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                    ItemDetailsObj.RID_NET_AMOUNT = Math.Round(ItemDetailsObj.RID_AMOUNT - ItemDetailsObj.RID_DISCOUNT + ItemDetailsObj.RID_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    ItemDetailsObj.RID_REMARKS = HttpUtility.HtmlEncode(txtPopupRemarks.Text);
                                }
                                else
                                {
                                    int slno = 1;
                                    if (assetServiceOrderReceiptDetailsObj.Item_details == null || assetServiceOrderReceiptDetailsObj.Item_details.Count == 0)
                                    {
                                        assetServiceOrderReceiptDetailsObj.Item_details = new List<AssetServiceOrderReceiptItemDetails>();
                                        slno = 1;
                                        int assetCount = 0, j = 0;
                                        assetCount = AssetServiceOrderReceiptDetailList.Count;
                                        if (assetCount > 0)
                                        {
                                            j = assetCount;
                                            do
                                            {
                                                j--;
                                                if (j >= 0)
                                                {
                                                    if (AssetServiceOrderReceiptDetailList[j].Item_details.Count > 0)
                                                    {
                                                        slno = AssetServiceOrderReceiptDetailList[j].Item_details.Max(itm => itm.RID_ITEM_SL_NO);
                                                        slno++;
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            while (AssetServiceOrderReceiptDetailList[j].Item_details.Count == 0 && j >= 0);
                                        }
                                    }
                                    else
                                    {
                                        slno = assetServiceOrderReceiptDetailsObj.Item_details.Max(itm => itm.RID_ITEM_SL_NO);
                                        slno++;
                                    }
                                    ItemDetailsObj = new AssetServiceOrderReceiptItemDetails();
                                    ItemDetailsObj.RID_PK = 0;
                                    ItemDetailsObj.RID_SL_NO = CurrSlNo;
                                    ItemDetailsObj.RID_RSD_PK = OSDPK;
                                    ITEM_SL_NO = ItemDetailsObj.RID_ITEM_SL_NO = slno;
                                    ItemRowIndex = assetServiceOrderReceiptDetailsObj.Item_details.Count;//Setting ItemRowIndex.This is used in Tax & Discount Adding
                                    ItemDetailsObj.RID_ASR_ITEM = string.IsNullOrEmpty(hdfPopupItemPk.Value) ? 0 : Convert.ToInt32(hdfPopupItemPk.Value);
                                    ItemDetailsObj.RID_ASR_ITEM_TEXT = txtPopupItems.Text;
                                    ItemDetailsObj.RID_QTY = string.IsNullOrEmpty(txtPopupQty.Text) ? 0 : Convert.ToDouble(txtPopupQty.Text);
                                    ItemDetailsObj.RID_RATE = Math.Round(Convert.ToDouble(txtPopupRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                                      ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P])));
                                    ItemDetailsObj.RID_UOM = string.IsNullOrEmpty(hdfPopupUomPk.Value) ? 0 : Convert.ToInt32(hdfPopupUomPk.Value);
                                    ItemDetailsObj.RID_UOM_TEXT = txtPopupUOM.Text;
                                    ItemDetailsObj.RID_AMOUNT = string.IsNullOrEmpty(txtPopupAmount.Text) ? 0 : Convert.ToDouble(txtPopupAmount.Text);
                                    ItemDetailsObj.RID_DISCOUNT = !string.IsNullOrEmpty(txtPopupDiscount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtPopupDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                    ItemDetailsObj.RID_TAX = !string.IsNullOrEmpty(txtPopupTax.Text.Trim()) ? Math.Round(Convert.ToDouble(txtPopupTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                    ItemDetailsObj.RID_NET_AMOUNT = Math.Round(ItemDetailsObj.RID_AMOUNT - ItemDetailsObj.RID_DISCOUNT + ItemDetailsObj.RID_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    ItemDetailsObj.RID_REMARKS = HttpUtility.HtmlEncode(txtPopupRemarks.Text);

                                    assetServiceOrderReceiptDetailsObj.Item_details.Add(ItemDetailsObj);
                                }
                            }
                            AssetServiceOrderReceiptDetailList.ForEach(dtl =>
                            {
                                if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                {
                                    dtl.RSD_AMOUNT = dtl.Item_details.Sum(r => r.RID_NET_AMOUNT);
                                }
                            });
                        }
                        retObject = AssetServiceOrderReceiptDetailList;
                        break;
                    #endregion

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
        #region Get UIValues From Object
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {
                switch (controlType)
                {
                    #region EDIT
                    case ControlsEnum.EDIT:
                        if (objAssetServiceOrderReceiptHeader != null)
                        {
                            txtDate.Text = Convert.ToDateTime(objAssetServiceOrderReceiptHeader.RSH_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            lblTrxNo.Text = string.IsNullOrEmpty(objAssetServiceOrderReceiptHeader.RSH_NO) ? Resources.ErpRes.Draft : objAssetServiceOrderReceiptHeader.RSH_NO;
                            hdfVendor.Value = objAssetServiceOrderReceiptHeader.RSH_VENDOR.ToString();
                            txtVendorName.Text = HttpUtility.HtmlDecode(objAssetServiceOrderReceiptHeader.RSH_VENDOR_TEXT.ToString());
                            ddlServiceType.SelectedValue = objAssetServiceOrderReceiptHeader.RSH_SERVICE_TYPE.ToString();
                            ddlRequestingStore.SelectedValue = objAssetServiceOrderReceiptHeader.RSH_DEPT_STORE.ToString();
                            hdfCurrency.Value = objAssetServiceOrderReceiptHeader.RSH_CURRENCY.ToString();
                            txtWorkDescription.Text = HttpUtility.HtmlDecode(objAssetServiceOrderReceiptHeader.RSH_REMARKS);
                            //Disabling Vendor,Servicetype & Requesting Store Controls                           
                            hdfDisableVendorStoreSType.Value = "1";
                            
                            txtInvoiceNO.Text = string.IsNullOrEmpty(objAssetServiceOrderReceiptHeader.RSH_INVOICE_NO) ? string.Empty : objAssetServiceOrderReceiptHeader.RSH_INVOICE_NO;
                            txtInvoiceDate.Text = string.IsNullOrEmpty(objAssetServiceOrderReceiptHeader.RSH_INVOICE_DATE) ? string.Empty : Convert.ToDateTime(objAssetServiceOrderReceiptHeader.RSH_INVOICE_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            ActionHandler(ddlRequestingStore, EventArgs.Empty);
                            #region Adding Saved SO to SelectedOSDetailPKList
                            if (SelectedOSDetailPKList == null)
                                SelectedOSDetailPKList = new List<SelectedServiceOrdrDtls>();
                            List<SelectedServiceOrdrDtls> tempSelectedSODetailPKList = new List<SelectedServiceOrdrDtls>();
                            foreach (var item in objAssetServiceOrderReceiptHeader.Details)
                            {
                                SelectedServiceOrdrDtls objselSOList = new SelectedServiceOrdrDtls();
                                objselSOList.OSH_PK = Convert.ToInt32(objAssetServiceOrderReceiptHeader.RSH_PK);
                                objselSOList.OSD_PK = Convert.ToInt32(item.RSD_OSD_DTL);
                                objselSOList.OSH_CURRENCY = Convert.ToInt32(objAssetServiceOrderReceiptHeader.RSH_CURRENCY);
                                objselSOList.RSD_PK = Convert.ToInt32(item.RSD_PK);
                                tempSelectedSODetailPKList.Add(objselSOList);

                                if (PendingSOList != null && PendingSOList.Count > 0)
                                {
                                    PendingServiceOrder tempPendingSO = PendingSOList.Where(x => x.OSD_PK == objselSOList.OSD_PK).Single();
                                    tempPendingSO.AddedToStockList = true;
                                    tempPendingSO.CheckBoxChecked = true;
                                }
                            }
                            if (tempSelectedSODetailPKList != null && tempSelectedSODetailPKList.Count > 0)
                            {
                                SelectedOSDetailPKList = tempSelectedSODetailPKList;
                            }
                            #endregion

                            txtHdrSubTotal.Text = objAssetServiceOrderReceiptHeader.RSH_SUB_TOTAL.ToString();
                            txtHdrDiscount.Text = objAssetServiceOrderReceiptHeader.RSH_DISCOUNT.ToString();
                            txtHdrTax.Text = objAssetServiceOrderReceiptHeader.RSH_TAX.ToString();
                            txtHdrOtherCharges.Text = objAssetServiceOrderReceiptHeader.RSH_OTH_CHARGE.ToString();
                            txtHdrPriceAdj.Text = objAssetServiceOrderReceiptHeader.RSH_PRICE_ADJ.ToString();
                            txtHdrGrandTotal.Text = objAssetServiceOrderReceiptHeader.RSH_NET_TOTAL.ToString();
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            if (CurrPK > 0)
                            {
                                hdfExchangeRate.Value = objAssetServiceOrderReceiptHeader.RSH_EXCHG_RATE.ToString();
                            }

                            LastModifiedTime = objAssetServiceOrderReceiptHeader.RSH_MOD_DT;
                            Status = objAssetServiceOrderReceiptHeader.RSH_STATUS;                            
                            hdfIsCancelled.Value = Convert.ToString(objAssetServiceOrderReceiptHeader.RSH_DEL_STATUS);

                            AssetServiceOrderReceiptDetailList = objAssetServiceOrderReceiptHeader.Details;
                            TempAssetServiceOrderReceiptDetailList = AssetServiceOrderReceiptDetailList;                           
                            SetFieldValues(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                            SetSubTotal();
                            SetHdrTax();
                        }
                        break;
                    #endregion
                    #region FILLASSETDETAILS
                    case ControlsEnum.FILLASSETDETAILS:
                        if (assetServiceOrderReceiptDetailsObj != null)
                        {

                            CurrSlNo = assetServiceOrderReceiptDetailsObj.RSD_SL_NO;
                            txtAsset.Text = HttpUtility.HtmlDecode(assetServiceOrderReceiptDetailsObj.RSD_ASSET_TEXT);
                            hdfAsset.Value = assetServiceOrderReceiptDetailsObj.RSD_ASSET.ToString();
                            txtAssetType.Text = HttpUtility.HtmlDecode(assetServiceOrderReceiptDetailsObj.RSD_ASSET_TYPE_TEXT);
                            hdfAssetType.Value = assetServiceOrderReceiptDetailsObj.RSD_ASSET_TYPE.ToString();
                            txtDescription.Text = HttpUtility.HtmlDecode(assetServiceOrderReceiptDetailsObj.RSD_DESC);
                        }
                        break;
                    #endregion
                    #region FILLITEMDETAILS
                    case ControlsEnum.FILLITEMDETAILS:
                        if (assetServiceOrderReceiptDetailsObj != null)
                        {
                            SID_SL_NO = assetServiceOrderReceiptItemDetObj.RID_SL_NO;
                            txtPopupItems.Text = HttpUtility.HtmlDecode(assetServiceOrderReceiptItemDetObj.RID_ASR_ITEM_TEXT);
                            hdfPopupItemPk.Value = assetServiceOrderReceiptItemDetObj.RID_ASR_ITEM.ToString();
                            txtPopupQty.Text = assetServiceOrderReceiptItemDetObj.RID_QTY.ToString();
                            txtPopupRate.Text = GetFormattedRate(assetServiceOrderReceiptItemDetObj.RID_RATE);
                            txtPopupAmount.Text = GetFormattedCurrency(assetServiceOrderReceiptItemDetObj.RID_AMOUNT);
                            txtPopupDiscount.Text = GetFormattedCurrency(assetServiceOrderReceiptItemDetObj.RID_DISCOUNT);
                            txtPopupTax.Text = GetFormattedCurrency(assetServiceOrderReceiptItemDetObj.RID_TAX);
                            txtPopupTotAmt.Text = GetFormattedCurrency(assetServiceOrderReceiptItemDetObj.RID_NET_AMOUNT);
                            txtPopupUOM.Text = assetServiceOrderReceiptItemDetObj.RID_UOM_TEXT.ToString();
                            hdfPopupUomPk.Value = assetServiceOrderReceiptItemDetObj.RID_UOM.ToString();
                            txtPopupRemarks.Text = assetServiceOrderReceiptItemDetObj.RID_REMARKS;
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
        #region Bind DropDown
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region COMPANY
                case ControlsEnum.COMPANY:

                    break;
                #endregion
                #region RECIEVINGSTORE
                case ControlsEnum.REQUESTINGSTORE:
                    ddlRequestingStore.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlRequestingStore.DataValueField = GTIService.Constants.DirectStockTransfer.Fields.DPT_PK;
                        ddlRequestingStore.DataTextField = GTIService.Constants.DirectStockTransfer.Fields.DPT_NAME;
                        ddlRequestingStore.DataSource = dtPageData;
                        ddlRequestingStore.DataBind();
                    }
                    ddlRequestingStore.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlRequestingStore.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    break;
                #endregion
                #region ASSETSERVICETYPE
                case ControlsEnum.ASSETSERVICETYPE:
                    ddlServiceType.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlServiceType.DataValueField = GTIService.Constants.AssetService.Fields.VTP_PK;
                        ddlServiceType.DataTextField = GTIService.Constants.AssetService.Fields.VTP_Name;
                        ddlServiceType.DataSource = dtPageData;
                        ddlServiceType.DataBind();
                    }
                    ddlServiceType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlServiceType.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    #region Listing ServiceType
                    ddlListServiceType.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlListServiceType.DataValueField = GTIService.Constants.AssetService.Fields.VTP_PK;
                        ddlListServiceType.DataTextField = GTIService.Constants.AssetService.Fields.VTP_Name;
                        ddlListServiceType.DataSource = dtPageData;
                        ddlListServiceType.DataBind();
                    }
                    ddlListServiceType.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECT_VALUE_ZERO));
                    foreach (ListItem item in ddlListServiceType.Items)
                    {
                        item.Text = HttpUtility.HtmlDecode(item.Text);
                    }
                    #endregion
                    break;
                #endregion
                #region TAXTYPES
                case ControlsEnum.TAXTYPES:
                    //Bind Tax dropdown
                    ddlTaxPopupTaxType.Items.Clear();
                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                    {
                        ddlTaxPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTaxDetails, Resources.DataFieldRes.RFQResponseTaxHead);
                        ddlTaxPopupTaxType.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                        ddlTaxPopupTaxType.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                        ddlTaxPopupTaxType.DataBind();
                    }
                    if (hdfTaxCategory.Value == ((int)TaxType.Tax).ToString())
                    {
                        if (IsCustomTaxEnabled)
                            ddlTaxPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    }
                    else
                    {
                        ddlTaxPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion
                default:
                    break;
            }
        }
        #endregion
        #region BindGrid
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
                    #region PENDINGORDERS
                    case ControlsEnum.PENDINGORDERS:
                        grdPendingSOItems.DataSource = PendingSOList;
                        grdPendingSOItems.DataBind();
                        break;
                    #endregion
                    #region AssetServiceOrderReceiptDTL
                    case ControlsEnum.ASSETSERVICEORDERRECEIPTDTL:
                        if (AssetServiceOrderReceiptHeaderSession != null)
                            AssetServiceOrderReceiptHeaderSession.Details = AssetServiceOrderReceiptDetailList;
                        grdAssetDetails.DataSource = AssetServiceOrderReceiptDetailList;
                        grdAssetDetails.DataBind();
                        #region If there is no Asset Details in Grid,Enabling Vendor,ServiceType,Store Controls
                        if (AssetServiceOrderReceiptDetailList != null)
                        {
                            if (AssetServiceOrderReceiptDetailList.Count == 0)
                            {
                                hdfDisableVendorStoreSType.Value = "0";
                            }
                            else
                            {
                                hdfDisableVendorStoreSType.Value = "1";
                            }
                        }
                        #endregion
                        break;
                    #endregion
                    #region TAXPOPUPGRID
                    case ControlsEnum.TAXPOPUPGRID:
                        if (IsHeaderTax)
                        {
                            taxHdrList = TempAssetServiceOrderReceiptHeaderSession.Tax_HDR == null ? new List<AssetServiceOrderReceiptTaxHdr>() :
                                TempAssetServiceOrderReceiptHeaderSession.Tax_HDR.Where(tax => Convert.ToInt32(tax.TRD_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            soDetailsObj = EditTempAssetServiceOrderReceiptHeaderSession.Details == null ? null :
                                       EditTempAssetServiceOrderReceiptHeaderSession.Details.SingleOrDefault(row => CurrSlNo == row.RSD_SL_NO);
                            if (ItemRowIndex >= 0)
                            {
                                assetServiceOrderReceiptItemDetObj = soDetailsObj.Item_details[ItemRowIndex];
                            }
                            if (assetServiceOrderReceiptItemDetObj != null)
                            {
                                taxHdrList = assetServiceOrderReceiptItemDetObj.Tax_DTL == null ? new List<AssetServiceOrderReceiptTaxHdr>() :
                                    assetServiceOrderReceiptItemDetObj.Tax_DTL.Where(tax => Convert.ToInt32(tax.TRD_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                            else
                                taxHdrList = new List<AssetServiceOrderReceiptTaxHdr>();
                        }
                        grdTaxDetails.DataSource = taxHdrList;
                        grdTaxDetails.DataBind();
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex) { throw ex; }
        }
        #endregion
        #region Reset Form
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                # region CLEARSERACH
                case ControlsEnum.CLEARSEARCH:
                    CurrPK = 0;
                    txtListFromDate.Text = string.Empty;// DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtListToDate.Text = string.Empty;//DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtListTrxNo.Text = string.Empty;                    
                    txtListVendorName.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfListVendor.Value = string.Empty;
                    ddlStatus.SelectedValue = "-1";
                    ddlListServiceType.SelectedValue = "0";
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;

                    Status = 0;
                    break;
                #endregion
                # region CLEAR
                case ControlsEnum.CLEAR:
                    CurrPK = 0;
                    txtDate.Text = DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    lblTrxNo.Text = Resources.ErpRes.Draft;
                    uclPaging.CurrentPage = 0;
                    PageIndex = 1;
                    txtListTrxNo.Text = string.Empty;
                    hdfTrxPk.Value = string.Empty;
                    txtVendorName.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfVendor.Value = string.Empty;
                    ddlRequestingStore.SelectedValue = "0";
                    ddlServiceType.SelectedValue = "0";
                    // txtCurrency.Text = Resources.ErpRes.AutoDefaultValue;
                    //hdfCurrency.Value = string.Empty;
                    txtWorkDescription.Text = string.Empty;
                    txtHdrSubTotal.Text = string.Empty;
                    txtHdrDiscount.Text = string.Empty;
                    txtHdrTax.Text = string.Empty;
                    txtHdrOtherCharges.Text = string.Empty;
                    txtHdrPriceAdj.Text = string.Empty;
                    txtHdrGrandTotal.Text = string.Empty;
                    Status = 0;
                    hdfSelRecordStatus.Value = "0";
                    AssetServiceOrderReceiptHeaderSession = null;
                    TempAssetServiceOrderReceiptHeaderSession = null;
                    EditTempAssetServiceOrderReceiptHeaderSession = null;
                    AssetServiceOrderReceiptDetailList = null;
                    TempAssetServiceOrderReceiptDetailList = null;
                    SelectedOSDetailPKList = null;
                    PendingSOList = null;
                    hdfDisableVendorStoreSType.Value = "0"; 
                    txtInvoiceNO.Text =txtInvoiceDate.Text= string.Empty;                                  
                    ResetForm(ControlsEnum.CLEARADD);
                    ResetForm(ControlsEnum.CLEARPOPUPDETAILS);

                    break;
                #endregion
                # region CLEAR ADD
                case ControlsEnum.CLEARADD:
                    txtAssetType.Text = string.Empty;
                    hdfAssetType.Value = string.Empty;
                    txtDescription.Text = string.Empty;
                    txtAsset.Text = string.Empty;
                    hdfAsset.Value = string.Empty;
                    CurrSlNo = 0;
                    SID_SL_NO = 0;
                    ITEM_SL_NO = 0;
                    RowIndex = -1;
                    break;
                #endregion
                # region CLEAR POPUPDETAILS
                case ControlsEnum.CLEARPOPUPDETAILS:
                    hdfPopupItemPk.Value = string.Empty;
                    txtPopupItems.Text = string.Empty;
                    txtPopupQty.Text = "0";
                    txtPopupRate.Text = string.Empty;
                    txtPopupUOM.Text = string.Empty;
                    hdfPopupUomPk.Value = string.Empty;
                    txtPopupAmount.Text = "0";
                    txtPopupDiscount.Text = "0";
                    txtPopupTax.Text = "0";
                    txtPopupRemarks.Text = string.Empty;
                    txtPopupTotAmt.Text = "0";
                    CurrSlNo = 0;
                    SID_SL_NO = 0;
                    ITEM_SL_NO = 0;
                    ItemRowIndex = -1;
                    break;
                #endregion
                #region TAXPOPUPGRID
                case ControlsEnum.TAXPOPUPGRID:
                    TaxPK = 0;
                    //SelectedDtlPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    break;
                #endregion
            }
        }
        #endregion
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithComma.Value);
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
        private int? GetNullableInt(string str)
        {
            int result;
            return (int.TryParse(str, out result) ? (int?)result : null);
        }

        private double CalculateTaxFormula(string taxFormula, double amount)
        {
            double taxAmt;
            taxAmt = 0;
            if (!string.IsNullOrEmpty(taxFormula))
            {
                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                taxAmt = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
            }
            return taxAmt;
        }
        public double StringToFormula(string expression)
        {
            List<string> tokens = getTokens(expression);
            Stack<double> operandStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();
            int tokenIndex = 0;
            try
            {
                while (tokenIndex < tokens.Count)
                {
                    string token = tokens[tokenIndex];
                    if (token == "(")
                    {
                        string subExpr = getSubExpression(tokens, ref tokenIndex);
                        operandStack.Push(StringToFormula(subExpr));
                        continue;
                    }
                    if (token == ")")
                    {
                        throw new ArgumentException("Mis-matched parentheses in expression");
                    }
                    //If this is an operator  
                    if (Array.IndexOf(_operators, token) >= 0)
                    {
                        while (operatorStack.Count > 0 && Array.IndexOf(_operators, token) < Array.IndexOf(_operators, operatorStack.Peek()))
                        {
                            string op = operatorStack.Pop();
                            double arg2 = operandStack.Pop();
                            double arg1 = operandStack.Pop();
                            operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                        }
                        operatorStack.Push(token);
                    }
                    else
                    {
                        operandStack.Push(double.Parse(token));
                    }
                    tokenIndex += 1;
                }

                while (operatorStack.Count > 0)
                {
                    string op = operatorStack.Pop();
                    double arg2 = operandStack.Pop();
                    double arg1 = operandStack.Pop();
                    operandStack.Push(_operations[Array.IndexOf(_operators, op)](arg1, arg2));
                }
                return operandStack.Pop();
            }
            catch
            {
                return 0;
            }
        }
        private string getSubExpression(List<string> tokens, ref int index)
        {
            StringBuilder subExpr = new StringBuilder();
            int parenlevels = 1;
            index += 1;
            while (index < tokens.Count && parenlevels > 0)
            {
                string token = tokens[index];
                if (tokens[index] == "(")
                {
                    parenlevels += 1;
                }

                if (tokens[index] == ")")
                {
                    parenlevels -= 1;
                }

                if (parenlevels > 0)
                {
                    subExpr.Append(token);
                }

                index += 1;
            }

            if ((parenlevels > 0))
            {
                throw new ArgumentException("Mis-matched parentheses in expression");
            }
            return subExpr.ToString();
        }
        private List<string> getTokens(string expression)
        {
            string operators = "()^*/+-";
            List<string> tokens = new List<string>();
            StringBuilder sb = new StringBuilder();

            foreach (char c in expression.Replace(" ", string.Empty))
            {
                if (operators.IndexOf(c) >= 0)
                {
                    if ((sb.Length > 0))
                    {
                        tokens.Add(sb.ToString());
                        sb.Length = 0;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    sb.Append(c);
                }
            }

            if ((sb.Length > 0))
            {
                tokens.Add(sb.ToString());
            }
            return tokens;
        }
        private void SetDetailTax(AssetServiceOrderReceiptHeader expenseHdr)
        {
            double quantity;
            double rate;
            quantity = 0;
            rate = 0;

            if (double.TryParse(txtPopupRate.Text, out quantity) && double.TryParse(txtPopupQty.Text, out rate))
            {
                if (txtPopupAmount != null)
                {
                    txtPopupAmount.Text = GetFormattedCurrency(rate * quantity);

                    //ExpensePK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                    SetItemTax(expenseHdr);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        private bool SetItemTax(AssetServiceOrderReceiptHeader expenseHdr)
        {
            double amount;
            double discount;
            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            Double.TryParse(txtPopupAmount.Text.Trim(), out amount);
            if (amount >= 0)
            {
                if (expenseHdr != null)
                {
                    objAssetServiceOrderReceiptHeader = expenseHdr;
                    soDetailsObj = objAssetServiceOrderReceiptHeader.Details.SingleOrDefault(crt => crt.RSD_SL_NO == CurrSlNo);
                    if (ItemRowIndex >= 0)
                        soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                    if (soDetailsItemObj != null)
                    {
                        if (soDetailsItemObj.Tax_DTL != null)
                        {
                            var discDetail = soDetailsItemObj.Tax_DTL.Where(quotation => quotation.TRD_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (AssetServiceOrderReceiptTaxHdr taxHdrObj in discDetail)
                            {
                                string taxFormula = taxHdrObj.TRD_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    taxHdrObj.TRD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                }
                            }
                            discount = soDetailsItemObj.Tax_DTL.Where(ctr => ctr.TRD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.TRD_TAX_AMT);
                        }
                        netAmount = amount - discount;
                        txtPopupDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                        if (soDetailsItemObj.Tax_DTL != null)
                        {
                            var taxDetail = soDetailsItemObj.Tax_DTL.Where(ctr => ctr.TRD_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (AssetServiceOrderReceiptTaxHdr taxHdrObj in taxDetail)
                            {
                                string taxFormula = taxHdrObj.TRD_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    //1 :- No need to create formula for manual entry of Item Tax amount in TAX POPUP.  
                                    //0 :- Create tax formula
                                    if (hdfApplyTax.Value == "0")
                                    {
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                        taxHdrObj.TRD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                    }
                                }
                            }
                            hdfApplyTax.Value = "0";
                            itmTax = soDetailsItemObj.Tax_DTL.ToList().Where(ctr => ctr.TRD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(ctr => ctr.TRD_TAX_AMT);
                        }
                        txtPopupTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                        soDetailsItemObj.RID_AMOUNT = Math.Round(amount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soDetailsItemObj.RID_DISCOUNT = Math.Round(discount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soDetailsItemObj.RID_TAX = Math.Round(itmTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soDetailsItemObj.RID_NET_AMOUNT = Math.Round(amount - discount + itmTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        txtTotal.Text = soDetailsItemObj.RID_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);



                        txtPopupTotAmt.Text = ((Convert.ToDecimal(txtPopupAmount.Text) + Convert.ToDecimal(txtPopupTax.Text)) - Convert.ToDecimal(txtPopupDiscount.Text)).ToString();
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool SetHdrTax()
        {
            double amount;
            double discount, OtherCharge;
            double adjust;
            amount = 0;
            adjust = 0;
            OtherCharge = 0;
            if (AssetServiceOrderReceiptHeaderSession != null)
            {
                objAssetServiceOrderReceiptHeader = AssetServiceOrderReceiptHeaderSession;
                amount = Convert.ToDouble(objAssetServiceOrderReceiptHeader.RSH_SUB_TOTAL);
                discount = 0;
                if (objAssetServiceOrderReceiptHeader.Tax_HDR != null)
                {
                    var discHeader = objAssetServiceOrderReceiptHeader.Tax_HDR.Where(hdr => hdr.TRD_TAX_CATEGORY == ((int)TaxType.Discount));
                    foreach (AssetServiceOrderReceiptTaxHdr taxHdrObj in discHeader)
                    {
                        string taxFormula = taxHdrObj.TRD_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.TRD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                        }
                    }
                    discount = objAssetServiceOrderReceiptHeader.Tax_HDR.Where(quotation => quotation.TRD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.TRD_TAX_AMT);
                }
                objAssetServiceOrderReceiptHeader.RSH_DISCOUNT = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;


                #region OtherCharge
                if (objAssetServiceOrderReceiptHeader.Tax_HDR != null)
                {
                    var otherChargeHeader = objAssetServiceOrderReceiptHeader.Tax_HDR.Where(hdr => hdr.TRD_TAX_CATEGORY == ((int)TaxType.Shipping));
                    foreach (AssetServiceOrderReceiptTaxHdr taxHdrObj in otherChargeHeader)
                    {
                        string taxFormula = taxHdrObj.TRD_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.TRD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                        }
                    }
                    OtherCharge = objAssetServiceOrderReceiptHeader.Tax_HDR.Where(quotation => quotation.TRD_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(quotation => quotation.TRD_TAX_AMT);
                }
                objAssetServiceOrderReceiptHeader.RSH_OTH_CHARGE = OtherCharge;
                txtHdrOtherCharges.Text = txtHdrOtherCharges.ToolTip = OtherCharge.ToString(hdfCurrencyFormat.Value);
                if (IsTaxForOtherCharge.Value == "1")
                {
                    amount = amount + OtherCharge;
                }
                #endregion


                if (objAssetServiceOrderReceiptHeader.Tax_HDR != null)
                {
                    var taxHeader = objAssetServiceOrderReceiptHeader.Tax_HDR.Where(quotation => quotation.TRD_TAX_CATEGORY == ((int)TaxType.Tax));
                    foreach (AssetServiceOrderReceiptTaxHdr taxHdrObj in taxHeader)
                    {
                        string taxFormula = taxHdrObj.TRD_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            //1 :- No need to create formula for manual entry of Header Tax amount in TAX POPUP.  
                            //0 :- Create tax formula
                            if (hdfApplyHdrTax.Value == "0")
                            {
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                taxHdrObj.TRD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            }
                        }
                    }
                    hdfApplyHdrTax.Value = "0";
                    objAssetServiceOrderReceiptHeader.RSH_TAX = objAssetServiceOrderReceiptHeader.Tax_HDR.Where(quotation => quotation.TRD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.TRD_TAX_AMT);
                }
                txtHdrTax.Text = txtHdrTax.ToolTip = objAssetServiceOrderReceiptHeader.RSH_TAX.ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtHdrPriceAdj.Text, out adjust);
                objAssetServiceOrderReceiptHeader.RSH_PRICE_ADJ = adjust;
                objAssetServiceOrderReceiptHeader.RSH_NET_TOTAL = Convert.ToDouble(objAssetServiceOrderReceiptHeader.RSH_SUB_TOTAL) - objAssetServiceOrderReceiptHeader.RSH_DISCOUNT + objAssetServiceOrderReceiptHeader.RSH_OTH_CHARGE + objAssetServiceOrderReceiptHeader.RSH_TAX
                     + objAssetServiceOrderReceiptHeader.RSH_PRICE_ADJ;

                txtHdrGrandTotal.Text = txtHdrGrandTotal.ToolTip = objAssetServiceOrderReceiptHeader.RSH_NET_TOTAL.ToString(hdfCurrencyFormat.Value);
                AssetServiceOrderReceiptHeaderSession = objAssetServiceOrderReceiptHeader;
                TempAssetServiceOrderReceiptHeaderSession = objAssetServiceOrderReceiptHeader;
            }
            return true;
        }
        private void SetSubTotal()
        {
            if (AssetServiceOrderReceiptHeaderSession != null)
            {
                AssetServiceOrderReceiptHeaderSession.RSH_SUB_TOTAL = AssetServiceOrderReceiptHeaderSession.Details.Sum(dtl => dtl.RSD_AMOUNT);
                txtHdrSubTotal.ToolTip = txtHdrSubTotal.Text = AssetServiceOrderReceiptHeaderSession.RSH_SUB_TOTAL.ToString(hdfCurrencyFormat.Value);
                txtHdrPriceAdj.ToolTip = txtHdrPriceAdj.Text = AssetServiceOrderReceiptHeaderSession.RSH_PRICE_ADJ.ToString(hdfCurrencyFormat.Value);
                txtHdrDiscount.ToolTip = txtHdrDiscount.Text = AssetServiceOrderReceiptHeaderSession.RSH_DISCOUNT.ToString(hdfCurrencyFormat.Value);
                txtHdrTax.ToolTip = txtHdrTax.Text = AssetServiceOrderReceiptHeaderSession.RSH_TAX.ToString(hdfCurrencyFormat.Value);
                txtHdrOtherCharges.ToolTip = txtHdrOtherCharges.Text = AssetServiceOrderReceiptHeaderSession.RSH_OTH_CHARGE.ToString(hdfCurrencyFormat.Value);
                decimal subTotal = Convert.ToDecimal(txtHdrSubTotal.Text);
                decimal hdrAdjamt = Convert.ToDecimal(txtHdrPriceAdj.Text);
                decimal hdrDiscount = string.IsNullOrEmpty(txtHdrDiscount.Text) ? 0 : Convert.ToDecimal(txtHdrDiscount.Text);
                decimal hdrTax = string.IsNullOrEmpty(txtHdrTax.Text) ? 0 : Convert.ToDecimal(txtHdrTax.Text);
                decimal hdrOtherCharge = string.IsNullOrEmpty(txtHdrOtherCharges.Text) ? 0 : Convert.ToDecimal(txtHdrOtherCharges.Text);
                txtHdrGrandTotal.Text = txtHdrGrandTotal.ToolTip = ((subTotal - hdrDiscount) + hdrTax + hdrOtherCharge + hdrAdjamt).ToString(hdfCurrencyFormat.Value);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            // OtherCharge is needed for Tax Calculation            
            IsTaxForOtherCharge.Value = (GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase")).ToString();

        }
        private bool IsItemExists(string itemName)
        {
            bool IsExists = false;
            if (AssetServiceOrderReceiptDetailList != null)
            {
                List<AssetServiceOrderReceiptItemDetails> lstASRItemDetails = new List<AssetServiceOrderReceiptItemDetails>();
                AssetServiceOrderReceiptDetails assetServiceOrderDetailsObj = AssetServiceOrderReceiptDetailList.SingleOrDefault(itm => itm.RSD_SL_NO == CurrSlNo);
                if (assetServiceOrderDetailsObj.Item_details != null)
                {
                    lstASRItemDetails = assetServiceOrderDetailsObj.Item_details.DeepClone();
                    if (lstASRItemDetails.Count > 0 && ItemRowIndex >= 0)
                    {
                        lstASRItemDetails.RemoveAt(ItemRowIndex);
                    }
                    foreach (AssetServiceOrderReceiptItemDetails item in lstASRItemDetails)
                    {
                        if (item.RID_ASR_ITEM_TEXT == itemName)
                        {
                            IsExists = true;
                            break;
                        }
                    }
                }
            }
            return IsExists;
        }
        private void FillTransactionData()
        {
            SetUIEditView(commonActions);
            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
                ucrWrkf.ViewAction();
            }
            GetFieldValues(ControlsEnum.EDIT);
            SetFieldValues(ControlsEnum.EDIT);
            ResetForm(ControlsEnum.CLEARADD);
            divSOPendingListing.Visible = true;
            txtDate.Focus();
        }
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

                int? result;
                bool bIsChecked = false;
                GridViewRow grvRow;
                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                string TrxNo = string.Empty;
                string action;
                int selSRDetId;
                AssetServiceOrderReceiptTaxHdr tempInvTaxSplitObj = null;
                double totalAmt;
                double currentTotal;
                double taxAmt;
                bool isValidDisc = true;
                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.LISTITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlRequestingStore")
                    {
                        commonActions = ActionsEnum.PENDINGORDERS;
                    }
                    else if (((DropDownList)sender).ID == "ddlServiceType")
                    {
                        commonActions = ActionsEnum.PENDINGORDERS;
                    }
                    else if (((DropDownList)sender).ID == "ddlTaxPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                }

                switch (commonActions)
                {
                    #region Grid Item Selected
                    case ActionsEnum.LISTITEMSELECTED:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            HiddenField hdfDelStatus;
                            HiddenField hdfStatus;
                            int selectedPK;
                            int dept;

                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRSH_PK")).Value);
                                hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                                hdfStatus = grdrow.FindControl("hdfStatus") as HiddenField;
                                hdfSelRecordStatus.Value = hdfStatus.Value.ToString();//For btnEditforCancel show/Hide
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = workflowCore.GetRefID(selectedPK, PageProcessID);
                                if (Convert.ToInt32(hdfDelStatus.Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    btnEditforCancel.Visible = false;
                                    btnEdit.Visible = false;
                                }
                                else
                                {
                                    btnEditforCancel.Visible = true;
                                }
                                if (Convert.ToInt32(hdfStatus.Value) == 0)
                                {
                                    btnEditforCancel.Visible = false;
                                }
                                break;
                            }
                        }
                        break;
                    #endregion
                    #region NEW
                    case ActionsEnum.NEW:
                        btnPrint.Visible = false;
                        EntryStatus = EntryStatus.NEWMODE;
                        ResetForm(ControlsEnum.CLEAR); //Contains:CLEARADD,CLEARPOPUPDETAILS 
                        SetFieldValues(ControlsEnum.PENDINGORDERS);
                        SetFieldValues(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                        SetSubTotal();
                        SetHdrTax();
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        //SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        hdfIsCancelled.Value = "0";
                        txtDate.Focus();
                        break;
                    #endregion
                    #region Asset(Add/Edit/Remove)
                    #region Add
                    case ActionsEnum.ADD:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            // insert duty slip details 
                            if (AssetServiceOrderReceiptDetailList == null)
                                AssetServiceOrderReceiptDetailList = new List<AssetServiceOrderReceiptDetails>();
                            AssetServiceOrderReceiptDetailList = (List<AssetServiceOrderReceiptDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                            if (AssetServiceOrderReceiptDetailList != null && AssetServiceOrderReceiptDetailList.Count > 0)
                            {
                                SetFieldValues(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                                SetSubTotal();
                                SetHdrTax();
                                ResetForm(ControlsEnum.CLEARADD);
                            }
                        }
                        break;
                    #endregion
                    #region EDITASSET
                    //To Edit Asset Details
                    case ActionsEnum.EDITASSET:
                        EntryStatus = EntryStatus.ENTRYMODE;
                        ResetForm(ControlsEnum.CLEARADD);
                        if (AssetServiceOrderReceiptDetailList != null && AssetServiceOrderReceiptDetailList.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdAssetDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][1]);
                            if (CurrSlNo > 0)
                            {
                                assetServiceOrderReceiptDetailsObj = AssetServiceOrderReceiptDetailList.SingleOrDefault(row => CurrSlNo == row.RSD_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.FILLASSETDETAILS);
                            }
                        }
                        break;
                    #endregion
                    #region REMOVEASSET
                    case ActionsEnum.REMOVEASSET:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        if (AssetServiceOrderReceiptDetailList != null && AssetServiceOrderReceiptDetailList.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                int osdPk = 0;
                                osdPk = AssetServiceOrderReceiptDetailList.Where(row => CurrSlNo == row.RSD_SL_NO).SingleOrDefault().RSD_OSD_DTL;
                                AssetServiceOrderReceiptDetailList = AssetServiceOrderReceiptDetailList.Where(row => CurrSlNo != row.RSD_SL_NO).ToList();
                                SelectedOSDetailPKList = SelectedOSDetailPKList.Where(row => osdPk != row.OSD_PK).ToList();
                                SetFieldValues(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                                SetSubTotal();
                                SetHdrTax();
                            }
                        }
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #endregion
                    #region PENDINGORDERS
                    case ActionsEnum.PENDINGORDERS:
                        int vendorPK = 0;
                        vendorPK = string.IsNullOrEmpty(hdfVendor.Value) ? 0 : Convert.ToInt32(hdfVendor.Value);
                        if (ddlRequestingStore.SelectedIndex > 0 && ddlServiceType.SelectedIndex > 0 && vendorPK > 0)
                        {
                            GetFieldValues(ControlsEnum.PENDINGORDERS);
                            SetFieldValues(ControlsEnum.PENDINGORDERS);
                        }
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        if (SelectedOSDetailPKList == null)
                            SelectedOSDetailPKList = new List<SelectedServiceOrdrDtls>();
                        tempSelectedOSDetailPKList = new List<SelectedServiceOrdrDtls>();                       
                        foreach (GridViewRow grdrow in grdPendingSOItems.Rows)
                        {
                            CheckBox chk;
                            chk = (CheckBox)grdrow.FindControl("chkSelectSOList");
                            if (chk.Checked)
                            {
                                SelectedServiceOrdrDtls objselOSList = new SelectedServiceOrdrDtls();
                                objselOSList.OSH_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSH_PK")).Value);
                                objselOSList.OSD_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSD_PK")).Value);
                                objselOSList.OSH_CURRENCY = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSH_CURRENCY")).Value);
                                objselOSList.RSD_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRSD_PK")).Value);
                                bool alreadyExists = SelectedOSDetailPKList.Exists(itemLst => itemLst.OSD_PK == objselOSList.OSD_PK);
                                if (!alreadyExists)
                                {
                                    tempSelectedOSDetailPKList.Add(objselOSList);
                                    hdfCurrency.Value = objselOSList.OSH_CURRENCY.ToString();
                                }
                            }
                        }
                        if (tempSelectedOSDetailPKList != null && tempSelectedOSDetailPKList.Count > 0)
                        {    
                            if (SelectedOSDetailPKList.Count > 0)
                            {
                                SelectedOSDetailPKList.AddRange(tempSelectedOSDetailPKList);
                            }
                            else
                            {
                                SelectedOSDetailPKList = tempSelectedOSDetailPKList;
                            }
                            GetFieldValues(ControlsEnum.SELECTEDSODETAILS);
                            SetFieldValues(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                            SetSubTotal();
                            SetHdrTax();
                        }
                        divSOPendingListing.Visible = true;
                        break;
                    #endregion
                    #region Transactions (SAVE/SAVESUBMIT/SUBMIT/WRKF SUBMIT)
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (AssetServiceOrderReceiptDetailList == null || AssetServiceOrderReceiptDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else
                        {
                            objAssetServiceOrderReceiptHeader = new AssetServiceOrderReceiptHeader();
                            objAssetServiceOrderReceiptHeader = (AssetServiceOrderReceiptHeader)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERRECEIPTHDR);
                            if (objAssetServiceOrderReceiptHeader != null)
                            {
                                if (objAssetServiceOrderReceiptHeader.Details != null && objAssetServiceOrderReceiptHeader.Details.Count > 0)
                                {
                                    objAssetServiceOrderReceiptHeader.WKF_FLAG = 0;
                                    objAssetServiceOrderReceiptHeader.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string trxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<AssetServiceOrderReceiptHeader>(objAssetServiceOrderReceiptHeader);
                                    result = ServiceOrderReceiptBL.SaveServiceOrderReceiptDetails(xmlDoc, out trxNo);
                                    if (result > 0)
                                    {

                                        lblTrxNo.Text = trxNo;
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
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
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceOrderReceipt);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
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
                    #region WRKF SUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (AssetServiceOrderReceiptDetailList == null || AssetServiceOrderReceiptDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objAssetServiceOrderReceiptHeader = new AssetServiceOrderReceiptHeader();
                                objAssetServiceOrderReceiptHeader = (AssetServiceOrderReceiptHeader)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERRECEIPTHDR);
                                if (objAssetServiceOrderReceiptHeader != null)
                                {
                                    if (objAssetServiceOrderReceiptHeader.Details != null && objAssetServiceOrderReceiptHeader.Details.Count > 0)
                                    {
                                        TrxNo = string.Empty;
                                        objAssetServiceOrderReceiptHeader.WKF_FLAG = 1;
                                        SaveTransaction(objAssetServiceOrderReceiptHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)//Cancel 
                            {
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }
                            else//Submit
                            {
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }

                        }
                        break;
                    #endregion 
                    #endregion
                    #region SHOWPOPUP (Item Popup)
                    case ActionsEnum.SHOWPOPUP:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                            CurrSlNo = Convert.ToInt32(grdAssetDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][1]);
                            grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                            RowIndex = grvRow.RowIndex;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                        }
                        break;
                    #endregion
                    #region Item(Add/Edit/Remove)
                    #region ADDITEM
                    case ActionsEnum.ADDITEM:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            // insert Item details                              
                            AssetServiceOrderReceiptHeaderSession = TempAssetServiceOrderReceiptHeaderSession;
                            AssetServiceOrderReceiptDetailList = AssetServiceOrderReceiptHeaderSession.Details;
                            if (!IsItemExists(txtPopupItems.Text.Trim()))
                            {
                                AssetServiceOrderReceiptDetailList = (List<AssetServiceOrderReceiptDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERRECEIPT_ITEMDTL);
                                if (AssetServiceOrderReceiptDetailList != null && AssetServiceOrderReceiptDetailList.Count > 0)
                                {
                                    SetFieldValues(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                                    SetSubTotal();
                                    SetHdrTax();
                                    ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_AlreadyAdded").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:                      
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        ItemRowIndex = grvRow.RowIndex;
                        if (AssetServiceOrderReceiptDetailList != null && AssetServiceOrderReceiptDetailList.Count > 0)
                        {
                            TempAssetServiceOrderReceiptHeaderSession = AssetServiceOrderReceiptHeaderSession;
                            CurrSlNo = SID_SL_NO = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                assetServiceOrderReceiptDetailsObj = AssetServiceOrderReceiptDetailList.SingleOrDefault(row => CurrSlNo == row.RSD_SL_NO);
                                assetServiceOrderReceiptItemDetObj = assetServiceOrderReceiptDetailsObj.Item_details[ItemRowIndex];
                                GetUIValuesFromObject(ControlsEnum.FILLITEMDETAILS);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                            }
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        ItemRowIndex = grvRow.RowIndex;
                        if (AssetServiceOrderReceiptDetailList != null && AssetServiceOrderReceiptDetailList.Count > 0)
                        {
                            CurrSlNo = SID_SL_NO = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                assetServiceOrderReceiptDetailsObj = AssetServiceOrderReceiptDetailList.SingleOrDefault(row => CurrSlNo == row.RSD_SL_NO);
                                assetServiceOrderReceiptDetailsObj.Item_details.RemoveAt(ItemRowIndex);
                                AssetServiceOrderReceiptDetailList.ForEach(dtl =>
                                {
                                    double osdAmount = 0;
                                    if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                    {
                                        osdAmount = dtl.Item_details.Sum(r => r.RID_NET_AMOUNT);
                                    }
                                    dtl.RSD_AMOUNT = osdAmount;
                                });
                                SetFieldValues(ControlsEnum.ASSETSERVICEORDERRECEIPTDTL);
                                SetSubTotal();
                                SetHdrTax();
                            }
                        }
                        ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                        break;
                    #endregion
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRSH_PK")).Value);
                                //grdListRowDeptId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDept")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ucrWrkf.Reset();
                            FillProcessID(12);//12-->For Cancel
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                                ucrWrkf.ViewType = 0;
                            ucrWrkf.ViewAction();

                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            ResetForm(ControlsEnum.CLEARADD);
                            divSOPendingListing.Visible = true;
                            txtDate.Focus();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT (CANCEL SUBMIT Button Click)
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region DETAIL/EDIT/VIEW
                    case ActionsEnum.DETAIL:
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                        btnPrint.Visible = true;
                        Status = 0;
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                ResetForm(ControlsEnum.CLEAR);
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRSH_PK")).Value);
                                //Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                Status = Convert.ToInt32(hdfStatus.Value);
                                hdfIsCancelled.Value = hdfDelStatus.Value;
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                ucrWrkf.ViewAction();
                            }
                            GetFieldValues(ControlsEnum.EDIT);
                            SetFieldValues(ControlsEnum.EDIT);
                            ResetForm(ControlsEnum.CLEARADD);
                            divSOPendingListing.Visible = true;
                            txtDate.Focus();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
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
                    #region CLEARSEARCH
                    case ActionsEnum.CLEARSEARCH:
                        FillProcessID(1);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region CANCEL /LIST
                    case ActionsEnum.CANCEL:
                    case ActionsEnum.LIST:
                        base.WkfRefID = 0;
                        FillProcessID(1);
                        CurrPK = 0;
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.CLEARSEARCH);
                        ResetForm(ControlsEnum.CLEAR);
                        GetFieldValues(ControlsEnum.LIST);
                        SetFieldValues(ControlsEnum.LIST);
                        break;
                    #endregion
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GetFieldValues(ControlsEnum.ITEMDETAILS);
                        SetFieldValues(ControlsEnum.ITEMDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                        break;
                    #endregion
                    #region CLEARADD
                    case ActionsEnum.CLEARADD:
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        result = ServiceOrderReceiptBL.DeleteServiceOrderReceipt(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = Convert.ToInt32(PageIndex) - 1;
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceOrderReceipt);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            EntryStatus = EntryStatus.LISTMODE;
                            ResetForm(ControlsEnum.CLEAR);
                            GetFieldValues(ControlsEnum.LIST);
                            SetFieldValues(ControlsEnum.LIST);
                        }
                        else
                        {
                            if (result == (int)DbSaveStatus.REFERRED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt;
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
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceOrderReceipt);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region TAXDETAILS
                    case ActionsEnum.TAXDETAILS:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            EditTempAssetServiceOrderReceiptHeaderSession = TempAssetServiceOrderReceiptHeaderSession;
                            soDetailsList = EditTempAssetServiceOrderReceiptHeaderSession.Details;
                            soDetailsObj = EditTempAssetServiceOrderReceiptHeaderSession.Details == null ? null :
                                    EditTempAssetServiceOrderReceiptHeaderSession.Details.SingleOrDefault(ctr => ctr.RSD_SL_NO == CurrSlNo);
                            if (ItemRowIndex >= 0)
                                soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                            if (soDetailsItemObj == null)
                            {
                                soDetailsList = (List<AssetServiceOrderReceiptDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERRECEIPT_ITEMDTL);
                                EditTempAssetServiceOrderReceiptHeaderSession.Details = soDetailsList;
                            }

                            if (soDetailsList != null && soDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                                hdfTaxFormula.Value = string.Empty;
                                txtTaxPopupItemAmount.Text = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) :
                                    string.IsNullOrEmpty(txtPopupDiscount.Text.Trim()) ? Convert.ToDouble(txtPopupAmount.Text).ToString(hdfCurrencyFormat.Value) :
                                    (Convert.ToDouble(txtPopupAmount.Text.Trim()) - Convert.ToDouble(txtPopupDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);

                                if (EditTempAssetServiceOrderReceiptHeaderSession != null)
                                {
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    if (ddlTaxPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;

                                            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtTaxPopupItemAmount.Text.Trim());
                                                txtTaxPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtTaxPopupAmount.Text = string.Empty;
                                        }
                                        txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) == -1)
                                        {
                                            txtTaxPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            txtTaxPopupOther.Enabled = false;
                                        }
                                    }
                                    IsEditMode = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region DISCDETAILS
                    case ActionsEnum.DISCDETAILS:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {
                            EditTempAssetServiceOrderReceiptHeaderSession = TempAssetServiceOrderReceiptHeaderSession;
                            soDetailsList = EditTempAssetServiceOrderReceiptHeaderSession.Details;
                            soDetailsObj = EditTempAssetServiceOrderReceiptHeaderSession.Details == null ? null :
                                    EditTempAssetServiceOrderReceiptHeaderSession.Details.SingleOrDefault(ctr => ctr.RSD_SL_NO == CurrSlNo);
                            if (ItemRowIndex >= 0)
                                soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                            if (soDetailsItemObj == null)
                            {
                                soDetailsList = (List<AssetServiceOrderReceiptDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERRECEIPT_ITEMDTL);
                                EditTempAssetServiceOrderReceiptHeaderSession.Details = soDetailsList;
                            }
                            if (soDetailsList != null && soDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtTaxPopupItemAmount.Text = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtPopupAmount.Text).ToString(hdfCurrencyFormat.Value);
                                if (EditTempAssetServiceOrderReceiptHeaderSession != null)
                                {
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    if (ddlTaxPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;
                                            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtTaxPopupItemAmount.Text.Trim());
                                                txtTaxPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtTaxPopupAmount.Text = string.Empty;
                                        }
                                        txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) == -1)
                                        {
                                            txtTaxPopupAmount.Enabled = true;
                                            txtTaxPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            txtTaxPopupAmount.Enabled = false;
                                            txtTaxPopupOther.Enabled = false;
                                        }
                                    }
                                    IsEditMode = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region TAXHEADER
                    case ActionsEnum.TAXHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (AssetServiceOrderReceiptHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                            TempAssetServiceOrderReceiptHeaderSession = AssetServiceOrderReceiptHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            // lblSubTotal = grdItemDetails.FooterRow == null ? null : (grdItemDetails.FooterRow.FindControl("lblSubTotalFooter") as Label);
                            if (Convert.ToDouble(txtHdrSubTotal.Text) > 0)
                            {
                                double taxable = 0;
                                //Is Othercharge is need for Tax Calulation
                                if (IsTaxForOtherCharge.Value == "1")
                                {
                                    taxable = Convert.ToDouble(AssetServiceOrderReceiptHeaderSession.RSH_SUB_TOTAL) - AssetServiceOrderReceiptHeaderSession.RSH_DISCOUNT + AssetServiceOrderReceiptHeaderSession.RSH_OTH_CHARGE;
                                }
                                else
                                {
                                    taxable = Convert.ToDouble(AssetServiceOrderReceiptHeaderSession.RSH_SUB_TOTAL) - AssetServiceOrderReceiptHeaderSession.RSH_DISCOUNT;
                                }

                                txtTaxPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);

                                if (ddlTaxPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtTaxPopupItemAmount.Text.Trim());
                                            txtTaxPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtTaxPopupAmount.Text = string.Empty;
                                    }
                                    txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) == -1)
                                    {
                                        //txtPopupAmount.Enabled = true;
                                        txtTaxPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        //txtPopupAmount.Enabled = false;
                                        txtTaxPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region DISCHEADER
                    case ActionsEnum.DISCHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (AssetServiceOrderReceiptHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                            TempAssetServiceOrderReceiptHeaderSession = AssetServiceOrderReceiptHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            if (Convert.ToDouble(txtHdrSubTotal.Text) > 0)
                            {
                                txtTaxPopupItemAmount.Text = string.IsNullOrEmpty(txtHdrSubTotal.Text.Replace(",", "").Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtHdrSubTotal.Text.Replace(",", "")).ToString(hdfCurrencyFormat.Value);
                                if (ddlTaxPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtTaxPopupItemAmount.Text.Trim());
                                            txtTaxPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtTaxPopupAmount.Text = string.Empty;
                                    }
                                    txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtTaxPopupAmount.Enabled = true;
                                        txtTaxPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtTaxPopupAmount.Enabled = false;
                                        txtTaxPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region OTHERCHARGEHEADER
                    case ActionsEnum.OTHERCHARGEHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Shipping).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (AssetServiceOrderReceiptHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                            TempAssetServiceOrderReceiptHeaderSession = AssetServiceOrderReceiptHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            if (Convert.ToDouble(txtHdrSubTotal.Text) > 0)
                            {
                                txtTaxPopupItemAmount.Text = string.IsNullOrEmpty(txtHdrSubTotal.Text.Replace(",", "").Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtHdrSubTotal.Text.Replace(",", "")).ToString(hdfCurrencyFormat.Value);
                                if (ddlTaxPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtTaxPopupItemAmount.Text.Trim());
                                            txtTaxPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtTaxPopupAmount.Text = string.Empty;
                                    }
                                    txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtTaxPopupAmount.Enabled = true;
                                        txtTaxPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtTaxPopupAmount.Enabled = false;
                                        txtTaxPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("OtherchargeDetails").ToString() + "','600','300');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region TAXAPPLY
                    case ActionsEnum.TAXAPPLY:
                        if (IsHeaderTax)
                        {
                            if (hdfTaxCategory.Value == ((int)TaxType.Tax).ToString())
                            {
                                hdfApplyHdrTax.Value = "1";
                            }
                            else
                            {
                                hdfApplyHdrTax.Value = "0";
                            }
                            AssetServiceOrderReceiptHeaderSession = TempAssetServiceOrderReceiptHeaderSession;
                            SetSubTotal();
                            SetHdrTax();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        }
                        else
                        {
                            if (hdfTaxCategory.Value == ((int)TaxType.Tax).ToString())
                            {
                                hdfApplyTax.Value = "1";
                            }
                            else
                            {
                                hdfApplyTax.Value = "0";
                            }
                            TempAssetServiceOrderReceiptHeaderSession = EditTempAssetServiceOrderReceiptHeaderSession;
                            //AssetServiceOrderReceiptHeaderSession = TempAssetServiceOrderReceiptHeaderSession;
                            // SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            soDetailsObj = TempAssetServiceOrderReceiptHeaderSession.Details.SingleOrDefault(crt => crt.RSD_SL_NO == CurrSlNo);
                            SetDetailTax(TempAssetServiceOrderReceiptHeaderSession);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ClosePopup();ShowContainerDiv('[id$=divItemPopup]','" + GetLocalResourceObject("ItemDetails").ToString() + "','800','280');", true);
                        }
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        break;
                    #endregion
                    #region TAXADD
                    case ActionsEnum.TAXADD:

                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (IsHeaderTax ? TempAssetServiceOrderReceiptHeaderSession != null : EditTempAssetServiceOrderReceiptHeaderSession != null)
                        {
                            objAssetServiceOrderReceiptHeader = IsHeaderTax ? TempAssetServiceOrderReceiptHeaderSession : EditTempAssetServiceOrderReceiptHeaderSession;
                            tempInvTaxSplitObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                {
                                    tempInvTaxSplitObj = objAssetServiceOrderReceiptHeader.Tax_HDR == null ? null :
                                        objAssetServiceOrderReceiptHeader.Tax_HDR.SingleOrDefault(ctr => ctr.TRD_TAX == Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) && ctr.TRD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempInvTaxSplitObj = objAssetServiceOrderReceiptHeader.Tax_HDR == null ? null :
                                        objAssetServiceOrderReceiptHeader.Tax_HDR.SingleOrDefault(ctr => ctr.TRD_NAME == txtTaxPopupOther.Text.Trim() && ctr.TRD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                soDetailsObj = objAssetServiceOrderReceiptHeader.Details == null ? null :
                                    objAssetServiceOrderReceiptHeader.Details.SingleOrDefault(ctr => ctr.RSD_SL_NO == CurrSlNo);
                                if (ItemRowIndex >= 0)
                                {
                                    soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                                }
                                if (soDetailsItemObj != null)
                                {
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempInvTaxSplitObj = soDetailsItemObj.Tax_DTL == null ? null :
                                            soDetailsItemObj.Tax_DTL.SingleOrDefault(rfq => rfq.TRD_TAX == Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) && rfq.TRD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempInvTaxSplitObj = soDetailsItemObj.Tax_DTL == null ? null :
                                            soDetailsItemObj.Tax_DTL.SingleOrDefault(rfq => rfq.TRD_NAME == txtTaxPopupOther.Text.Trim() && rfq.TRD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempInvTaxSplitObj == null)
                            {
                                taxHdrList = new List<AssetServiceOrderReceiptTaxHdr>();

                                soInvTaxHdrObj = new AssetServiceOrderReceiptTaxHdr();
                                try
                                {
                                    soInvTaxHdrObj.TRD_TAX_AMT = string.IsNullOrEmpty(txtTaxPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtTaxPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    soInvTaxHdrObj.TRD_SL_NO = CurrSlNo;
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                    {
                                        soInvTaxHdrObj.TRD_TAX = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                                    }
                                    soInvTaxHdrObj.TRD_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    soInvTaxHdrObj.TRD_NAME = HttpUtility.HtmlEncode(txtTaxPopupOther.Text.Trim());
                                    soInvTaxHdrObj.TRD_PK = 0;
                                    soInvTaxHdrObj.TRD_RID_PK = CurrPK;
                                    soInvTaxHdrObj.TRD_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    soInvTaxHdrObj.TRD_TYPE = 1;
                                    soInvTaxHdrObj.TRD_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (soInvTaxHdrObj.TRD_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(objAssetServiceOrderReceiptHeader.RSH_SUB_TOTAL);
                                            currentTotal = objAssetServiceOrderReceiptHeader.Tax_HDR == null ? 0 :
                                                objAssetServiceOrderReceiptHeader.Tax_HDR.Where(htx => htx.TRD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.TRD_TAX_AMT);
                                            if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(soInvTaxHdrObj.TRD_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = soInvTaxHdrObj.TRD_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                if (objAssetServiceOrderReceiptHeader.Tax_HDR == null)
                                                    taxHdrList = new List<AssetServiceOrderReceiptTaxHdr>();
                                                else
                                                    taxHdrList = objAssetServiceOrderReceiptHeader.Tax_HDR.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                objAssetServiceOrderReceiptHeader.Tax_HDR = taxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            if (objAssetServiceOrderReceiptHeader.Tax_HDR == null)
                                                taxHdrList = new List<AssetServiceOrderReceiptTaxHdr>();
                                            else
                                                taxHdrList = objAssetServiceOrderReceiptHeader.Tax_HDR.ToList();
                                            taxHdrList.Add(soInvTaxHdrObj);
                                            objAssetServiceOrderReceiptHeader.Tax_HDR = taxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        soDetailsObj = objAssetServiceOrderReceiptHeader.Details == null ? null :
                                            objAssetServiceOrderReceiptHeader.Details.SingleOrDefault(item => item.RSD_SL_NO == CurrSlNo);
                                        if (ItemRowIndex >= 0)
                                        {
                                            soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                                        }
                                        if (soDetailsItemObj != null)
                                        {
                                            if (soInvTaxHdrObj.TRD_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = soDetailsObj.RSD_AMOUNT;
                                                currentTotal = soDetailsItemObj.Tax_DTL == null ? 0 :
                                                    soDetailsItemObj.Tax_DTL.Where(dtx => dtx.TRD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.TRD_TAX_AMT);
                                                if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(soInvTaxHdrObj.TRD_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = soInvTaxHdrObj.TRD_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    taxHdrList = soDetailsItemObj.Tax_DTL == null ? new List<AssetServiceOrderReceiptTaxHdr>() : soDetailsItemObj.Tax_DTL.ToList();
                                                    taxHdrList.Add(soInvTaxHdrObj);
                                                    if (ItemRowIndex >= 0)
                                                    {
                                                        objAssetServiceOrderReceiptHeader.Details.SingleOrDefault(rfq => rfq.RSD_SL_NO == CurrSlNo).Item_details[ItemRowIndex].Tax_DTL = taxHdrList;
                                                    }
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                taxHdrList = soDetailsItemObj.Tax_DTL == null ? new List<AssetServiceOrderReceiptTaxHdr>() : soDetailsItemObj.Tax_DTL.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                objAssetServiceOrderReceiptHeader.Details.SingleOrDefault(rfq => rfq.RSD_SL_NO == CurrSlNo).Item_details[ItemRowIndex].Tax_DTL = taxHdrList;
                                            }
                                        }
                                    }
                                    if (IsHeaderTax)
                                        TempAssetServiceOrderReceiptHeaderSession = objAssetServiceOrderReceiptHeader;
                                    else
                                        EditTempAssetServiceOrderReceiptHeaderSession = objAssetServiceOrderReceiptHeader;
                                    //EditTempAssetServiceOrderReceiptHeaderSession = objAssetServiceOrderReceiptHeader;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                }
                            }
                            else
                            {
                                errorTaxAdd = true;
                            }
                            if (ddlTaxPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) == -1)
                                {
                                    //txtPopupAmount.Enabled = true;
                                    txtTaxPopupOther.Enabled = true;
                                }
                                else
                                {
                                    //txtPopupAmount.Enabled = false;
                                    txtTaxPopupOther.Enabled = false;
                                }
                                if (!errorTaxAdd && !errorTaxAmount)
                                {
                                    txtTaxPopupAmount.Text = string.Empty;
                                    txtTaxPopupOther.Text = string.Empty;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        if (errorTaxAdd)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Add").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (errorTaxAmount)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Tax_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (!isValidDisc)
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Discount_Amount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        break;
                    #endregion
                    #region TAXDELETE
                    case ActionsEnum.TAXDELETE:
                        if (IsHeaderTax ? TempAssetServiceOrderReceiptHeaderSession != null : EditTempAssetServiceOrderReceiptHeaderSession != null)
                        {
                            objAssetServiceOrderReceiptHeader = IsHeaderTax ? TempAssetServiceOrderReceiptHeaderSession : EditTempAssetServiceOrderReceiptHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                taxHdrList = new List<AssetServiceOrderReceiptTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempInvTaxSplitObj = objAssetServiceOrderReceiptHeader.Tax_HDR == null ? null :
                                            objAssetServiceOrderReceiptHeader.Tax_HDR.SingleOrDefault(rfq => rfq.TRD_TAX == taxPK && rfq.TRD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempInvTaxSplitObj = objAssetServiceOrderReceiptHeader.Tax_HDR == null ? null :
                                                objAssetServiceOrderReceiptHeader.Tax_HDR.SingleOrDefault(rfq => rfq.TRD_NAME == hdfTaxName.Value && rfq.TRD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempInvTaxSplitObj != null)
                                    {
                                        taxHdrList = objAssetServiceOrderReceiptHeader.Tax_HDR.ToList();
                                        taxHdrList.Remove(tempInvTaxSplitObj);
                                        objAssetServiceOrderReceiptHeader.Tax_HDR = taxHdrList;
                                    }
                                }
                                else
                                {
                                    soDetailsObj = objAssetServiceOrderReceiptHeader.Details == null ? null :
                                        objAssetServiceOrderReceiptHeader.Details.SingleOrDefault(rfq => rfq.RSD_SL_NO == CurrSlNo);
                                    if (ItemRowIndex >= 0)
                                    {
                                        soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                                    }
                                    if (soDetailsItemObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempInvTaxSplitObj = soDetailsItemObj.Tax_DTL == null ? null :
                                                soDetailsItemObj.Tax_DTL.SingleOrDefault(rfq => rfq.TRD_TAX == taxPK && rfq.TRD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempInvTaxSplitObj = soDetailsItemObj.Tax_DTL == null ? null :
                                                    soDetailsItemObj.Tax_DTL.SingleOrDefault(rfq => rfq.TRD_NAME == hdfTaxName.Value && rfq.TRD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        soDetailsObj = objAssetServiceOrderReceiptHeader.Details.SingleOrDefault(rfq => rfq.RSD_SL_NO == CurrSlNo);
                                        if (ItemRowIndex >= 0)
                                        {
                                            soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                                        }
                                        if (soDetailsItemObj != null && soDetailsItemObj.Tax_DTL != null)
                                        {
                                            taxHdrList = soDetailsItemObj.Tax_DTL.ToList();
                                            taxHdrList.Remove(tempInvTaxSplitObj);
                                            objAssetServiceOrderReceiptHeader.Details.SingleOrDefault(rfq => rfq.RSD_SL_NO == CurrSlNo).Item_details[ItemRowIndex].Tax_DTL = taxHdrList;
                                        }
                                    }
                                }
                                if (IsHeaderTax)
                                    TempAssetServiceOrderReceiptHeaderSession = objAssetServiceOrderReceiptHeader;
                                else
                                    EditTempAssetServiceOrderReceiptHeaderSession = objAssetServiceOrderReceiptHeader;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                            }
                            if (ddlTaxPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) == -1)
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    //txtTaxPopupAmount.Enabled = true;
                                    txtTaxPopupOther.Enabled = true;
                                }
                                else
                                {
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtTaxPopupItemAmount.Text.Trim());
                                            txtTaxPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    txtTaxPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        break;
                    #endregion
                    #region TAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                        {
                            TaxPK = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            TaxPK = 0;
                            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                hdfTaxFormula.Value = taxFormula;
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtTaxPopupItemAmount.Text.Trim());
                                txtTaxPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                txtTaxPopupOther.Text = ddlTaxPopupTaxType.SelectedItem.Text;
                                // Enable amount textbox in TAX POPUP & disable it in DISCOUNT POPUP
                                if (hdfTaxCategory.Value == ((int)TaxType.Tax).ToString())
                                {
                                    txtTaxPopupAmount.Enabled = true;
                                }
                                else
                                {
                                    txtTaxPopupAmount.Enabled = false;
                                }
                                //txtTaxPopupAmount.Enabled = false;
                                txtTaxPopupOther.Enabled = false;
                            }
                        }
                        else if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) == -1)
                        {
                            hdfTaxFormula.Value = string.Empty;
                            txtTaxPopupAmount.Text = string.Empty;
                            SelectedTaxText = Resources.Report.Custom;
                            txtTaxPopupOther.Text = string.Empty;
                            txtTaxPopupAmount.Enabled = true;
                            txtTaxPopupOther.Enabled = true;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        foreach (GridViewRow grdrow in grdList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfRSH_PK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + "SRA" + "&APPSUBTYPE=0") + "');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Record").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally { }
        }
        private void ResetPageAndWorkflow()
        {
            ResetForm(ControlsEnum.CLEAR);
            EntryStatus = EntryStatus.LISTMODE;
            GetFieldValues(ControlsEnum.LIST);
            SetFieldValues(ControlsEnum.LIST);
            // GetUIValuesFromObject(ControlsEnum.SALARYPAYMENTDETAILS);
            #region Reset Workflow
            //base.ExtUserDept = 0;
            ucrWrkf.Reset();
            FillProcessID(1);
            WorkflowCore.CoreService objworkflowCore = new WorkflowCore.CoreService();
            base.WkfRefID = ucrWrkf.RefID = objworkflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
            SetCancelRef(CurrPK);
            ucrWrkf.FillWorkFlowDetails();
            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.LISTMODE && ucrWrkf.HasPageTaskPermission)
                ucrWrkf.ViewType = 1;
            else
            {
                ucrWrkf.ViewType = 0;
                ucrWrkf.ViewAction();
            }
            #endregion
        }

        /// <summary>
        /// Save With workflow submition
        /// </summary>
        /// <param name="objServiceRequestHeader"></param>
        private void SaveTransaction(AssetServiceOrderReceiptHeader objServiceRequestHeader, int workflowFlag)
        {
            int? result = 0;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objServiceRequestHeader == null)
                objServiceRequestHeader = new AssetServiceOrderReceiptHeader();
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objServiceRequestHeader.USER_PK = wkfDetails.UserPK;
            objServiceRequestHeader.WKF_APPLICATION = CurrPK;
            objServiceRequestHeader.WKF_COMMENTS = wkfDetails.Comments;
            objServiceRequestHeader.WKF_TRX_FLAG = workflowFlag;
            objServiceRequestHeader.WKF_PROCESS = wkfDetails.ProcessID;
            objServiceRequestHeader.WKF_REFERENCE = wkfDetails.ReferenceID;
            objServiceRequestHeader.WKF_TASK = wkfDetails.TaskID;
            objServiceRequestHeader.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<AssetServiceOrderReceiptHeader>(objServiceRequestHeader);
            result = ServiceOrderReceiptBL.SaveServiceOrderReceiptDetails(xmlDoc, out TrxNo);
            if (result > 0)
            {
                if (!string.IsNullOrEmpty(TrxNo))
                    lblTrxNo.Text = TrxNo;
                //ucrWrkf.ApplicationID = result.Value;
                if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                {
                    FillProcessID(1);
                    litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                }
                else
                {
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                }
                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                ((TextBox)ucrWrkf.FindControl("WrkfComments")).Text = string.Empty;//Clear Workflow comments

                object[] args = new object[2];
                args[0] = Resources.PageNameRes.AssetServiceOrderReceipt;
                args[1] = lblTrxNo.Text.Trim();
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                // Show Save Message and redired to listing page                                      
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                    ResetForm(ControlsEnum.CLEAR);
                    EntryStatus = EntryStatus.LISTMODE;
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.LIST);
                    SetFieldValues(ControlsEnum.LIST);
                }

            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrderReceipt + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceOrderReceipt);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
            }
        }

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

        #region ActionHandler
        protected void ActionHandler(object sender, GridViewCommandEventArgs e)
        {

        }
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                GridView senderGridView = (GridView)sender;
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    if (senderGridView.ID == "grdAssetDetails")
                    {
                        if (AssetServiceOrderReceiptDetailList != null)
                        {
                            HiddenField hdfSRDSlNo = e.Row.FindControl("hdfRSDSlNo") as HiddenField;
                            Label lblTotal = e.Row.FindControl("lblTotal") as Label;
                            int RSD_SL_NO = Convert.ToInt32(hdfSRDSlNo.Value);
                            assetServiceOrderReceiptItemDetList = new List<AssetServiceOrderReceiptItemDetails>();
                            assetServiceOrderReceiptItemDetList = AssetServiceOrderReceiptDetailList.SingleOrDefault(c => c.RSD_SL_NO == RSD_SL_NO).Item_details;
                            if (assetServiceOrderReceiptItemDetList != null)
                            {
                                GridView grdItems = e.Row.FindControl("grdItems") as GridView;
                                grdItems.DataSource = assetServiceOrderReceiptItemDetList;
                                grdItems.DataBind();
                                if (lblTotal != null)
                                {
                                    lblTotal.Text = lblTotal.ToolTip = GetFormattedCurrencyWithComma(assetServiceOrderReceiptItemDetList.Sum(itm => itm.RID_NET_AMOUNT));
                                }
                            }
                        }

                    }
                    else if (senderGridView.ID == "grdItems")
                    {
                        HiddenField hdfRID_OID_DTL = e.Row.FindControl("hdfRID_OID_DTL") as HiddenField;
                        foreach (TableCell cell in e.Row.Cells)
                        {
                            if (Convert.ToInt32(hdfRID_OID_DTL.Value) > 0)
                            {
                                string selectedRowColor = Resources.ErpRes.selectedRowColor;
                                cell.BackColor = System.Drawing.ColorTranslator.FromHtml(selectedRowColor);      
                            }
                        } 
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Pager Methods + Init
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

        protected void Page_PreRender(Object sender, EventArgs e)
        {
            try
            {
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
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideAdvance", "ShowHideAdvancedSearch();", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInActive").ToString();
                }
                else if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "ShowListing();", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "ViewMode(1);", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch(1);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "DisableVendorStoreServType", "$(document).ready(function(){DisableVendorStoreServType();});", true);

                if (CurrPK == 0)
                    btnSubmit.Visible = false;
                if (CurrPK == 0 || Status == 0)
                    btnCancelSubmit.Visible = false;

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
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
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }
        }
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
                    appId = Convert.ToInt32((dtApplication.Rows[0][CommonConstants.F_APP_PK] == DBNull.Value) ? 0 : dtApplication.Rows[0][CommonConstants.F_APP_PK]);
                }
            }
            return appId;
        }

        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(int pid)
        {
            string path = string.Empty;
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();

            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
            {
                WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
                DataTable dtProcess = wrkfService.GetProcessID(path, Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                if (dtProcess != null && dtProcess.Rows.Count > 0)
                {
                    ucrWrkf.PageUrl = path;
                    ucrWrkf.ProcessID = int.Parse(dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString());
                    hdfProcessID.Value = dtProcess.Rows[0][CommonConstants.F_PROCESS].ToString();
                    if (pid == 1)
                    {
                        PageProcessID = ucrWrkf.ProcessID;
                    }
                    base.WkfPageUrl = path;
                }
            }
        }
        /// <summary>
        /// For Bind Cancelation comment on workflow user control
        /// </summary>
        /// <param name="curPK"></param>
        private void SetCancelRef(int curPK)
        {
            #region Cancel ref Setting
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
            if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
            {
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
            }
            #endregion
        }
        private string GetUrl()
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            if (Request.QueryString[QueryStrings.PID] == null)
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=1";
                else
                    path = Request.Url.AbsolutePath.ToLower() + "?PID=1";
            }
            else
            {
                if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                    path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
                else
                    path = Request.Url.AbsolutePath.ToLower();
            }
            return path;
        }
        #endregion
        #region Page Events
        /// <summary>
        /// PageInit Used to set the Pager current Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, System.EventArgs e)
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            this.btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSave.PreRender += new EventHandler(btnAction_PreRender);
            this.btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnNew.PreRender += new EventHandler(btnAction_PreRender);
            this.btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            this.btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);


            this.btnSubmit.Load += new EventHandler(btnAction_Load);
            this.btnSave.Load += new EventHandler(btnAction_Load);
            this.btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            this.btnDeleteNew.Load += new EventHandler(btnAction_Load);
            this.btnNew.Load += new EventHandler(btnAction_Load);
            this.btnEdit.Load += new EventHandler(btnAction_Load);
            this.btnView.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);

        }
        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = GetLocalResourceObject("Breadcrumb").ToString().Replace(">>",
                "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>");
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
            base.CheckBtnVisibility(sender);
        }
        #endregion
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            CURRENCY,
            LIST,
            CLEAR,
            PAYMENTMODE,
            EDIT,
            CLEARPOPUPDETAILS,
            COMPANY,
            BRANCH,
            PROCESSMODE,
            CLEARADD,
            GRIDEDIT,
            GRIDDELETE,
            DATDETAILS,
            DEPARTMENTBYTYPE,
            REQUESTINGSTORE,
            ASSETSERVICEORDERRECEIPTHDR,
            ASSETSERVICEORDERRECEIPTDTL,
            ASSETSERVICEORDERRECEIPT_ITEMDTL,
            ASSETSERVICETYPE,
            FILLASSETDETAILS,
            ITEMDETAILS,
            FILLITEMDETAILS,
            CLEARSEARCH,
            PENDINGORDERS,
            SELECTEDSODETAILS,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXHEADER,
            CUSTOMTAXSETTINGS,
            SOFROMINBOX,
            EXCHANGERATE

        }
        #endregion
    }
}