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

namespace ERPSMS_v01.AssetService
{
    public partial class AssetServiceOrder : ERP.Store.UI.WorkFlowBasePage
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
        /// To maintain keep Asset ServiceOrder Header
        /// </summary>
        private AssetServiceOrderHeader AssetServiceOrderHeaderSession
        {
            get
            {
                return (AssetServiceOrderHeader)ViewState[ViewstateStrings.AssetServiceOrderHeader];
            }
            set
            {
               ViewState[ViewstateStrings.AssetServiceOrderHeader] = value;
            }
        }
        /// <summary>
        /// To maintain keep Temp Asset ServiceOrder Header
        /// </summary>
        private AssetServiceOrderHeader TempAssetServiceOrderHeaderSession
        {
            get
            {
                return (AssetServiceOrderHeader)ViewState["TempAssetServiceOrderHeaderSession"];
            }
            set
            {
                ViewState["TempAssetServiceOrderHeaderSession"] = value;
            }
        }
        /// <summary>
        /// To maintain keep Expense Header Tax Splitting
        /// </summary>
        private AssetServiceOrderHeader EditTempAssetServiceOrderHeaderSession
        {
            get
            {
                return (AssetServiceOrderHeader)this.ViewState["EditTempAssetServiceOrderHeaderSession"];
            }
            set
            {
                this.ViewState["EditTempAssetServiceOrderHeaderSession"] = value;
            }
        }
        /// <summary>
        /// To keep details in view state
        /// </summary>
        private List<AssetServiceOrderDetails> AssetServiceOrderDetailList
        {
            get
            {
                return (List<AssetServiceOrderDetails>)ViewState[ViewstateStrings.AssetServiceRequestDetailList];
            }
            set
            {
                ViewState[ViewstateStrings.AssetServiceRequestDetailList] = value;
            }
        }
        /// <summary>
        /// To keep ServiceOrderDetailList Temporary in view state
        /// </summary>
        private List<AssetServiceOrderDetails> TempAssetServiceOrderDetailList
        {
            get
            {
                return (List<AssetServiceOrderDetails>)ViewState["TempAssetServiceOrderDetailList"];
            }
            set
            {
                ViewState["TempAssetServiceOrderDetailList"] = value;
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
        private List<PendingServiceRequest> PendingSRList
        {
            get
            {
                return this.ViewState["PendingSRList"] == null ? new List<PendingServiceRequest>() : (List<PendingServiceRequest>)(this.ViewState["PendingSRList"]);
            }
            set
            {
                this.ViewState["PendingSRList"] = value;
            }
        }
        /// <summary>
        /// To keep selected Service request Dtl PKs 
        /// </summary>
        private List<SelectedServiceReqDtls> SelectedSRDetailPKList
        {
            get
            {
                return this.ViewState["SelectedSRDetailPKList"] == null ? new List<SelectedServiceReqDtls>() : (List<SelectedServiceReqDtls>)(this.ViewState["SelectedSRDetailPKList"]);
            }
            set
            {
                this.ViewState["SelectedSRDetailPKList"] = value;
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
        DataTable dtSR;
        private DataTable dtCustomTaxSet;
        private MultipleSRHeader multipleSRHeaderObj;
        private AssetServiceOrderHeader objAssetServiceOrderHeader;
        private AssetServiceOrderDetails assetServiceOrderDetailsObj;
        private AssetServiceOrderItemDetails assetServiceOrderItemDetObj;
        List<AssetServiceOrderDetails> soDetailsList;
        List<AssetServiceOrderItemDetails> assetServiceOrderItemDetList;
        List<AssetServiceOrderTaxHdr> taxHdrList;
        private AssetServiceOrderTaxHdr soInvTaxHdrObj;
        AssetServiceOrderDetails soDetailsObj;
        AssetServiceOrderItemDetails soDetailsItemObj;
        List<SelectedServiceReqDtls> tempSelectedSRDetailPKList;
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
                    datakeyarray[0] = Resources.DataFieldRes.OSD_PK;
                    datakeyarray[1] = "OSD_SL_NO";
                    grdAssetDetails.DataKeyNames = datakeyarray;
                    AssetServiceOrderHeaderSession = new AssetServiceOrderHeader();
                    AssetServiceOrderDetailList = new List<AssetServiceOrderDetails>();

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
                        TempAssetServiceOrderHeaderSession = new AssetServiceOrderHeader();
                        EditTempAssetServiceOrderHeaderSession = new AssetServiceOrderHeader();
                        AssetServiceOrderHeaderSession = new AssetServiceOrderHeader();
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
                                     SR_PK.Value = (dtApplication.Rows[0]["refApplication"] == DBNull.Value) ? "0" : dtApplication.Rows[0]["refApplication"].ToString();
                                     GetFieldValues(ControlsEnum.SRFROMINBOX);
                                     SetFieldValues(ControlsEnum.SRFROMINBOX);
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
            int vendorPK, storeId,serviceTypeId,sohPK=0;
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
                        dtResult = BusinessLogic.AssetService.ServiceOrderBL.GetServiceOrderList(gridParam, currentUser, trxNo, VendorPK, serviceType);
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
                        objAssetServiceOrderHeader = ServiceOrderBL.GetServiceOrderByPK(CurrPK);
                        AssetServiceOrderHeaderSession = objAssetServiceOrderHeader;
                        break;
                    #endregion      
                    #region Pending Requests
                    case ControlsEnum.PENDINGREQUESTS:
                        vendorPK = storeId=serviceTypeId = 0;
                        vendorPK = string.IsNullOrEmpty(hdfVendor.Value)?0:Convert.ToInt32(hdfVendor.Value);                       
                        if (ddlRequestingStore.SelectedIndex > 0)
                            storeId = Convert.ToInt32(ddlRequestingStore.SelectedValue);
                        serviceTypeId = string.IsNullOrEmpty(ddlServiceType.SelectedValue) ? 0 : Convert.ToInt32(ddlServiceType.SelectedValue);
                        sohPK = CurrPK;
                        dsPageData = BusinessLogic.AssetService.ServiceOrderBL.GetServiceRequestPending(currentUser.SBUID, storeId, vendorPK, serviceTypeId, sohPK);
                        dtPageData = new DataTable();
                        dtPageData = dsPageData.Tables[0];
                        PendingSRList = dtPageData
                            .AsEnumerable()
                            .Select(x => new PendingServiceRequest
                            {
                                SRD_SRH_HDR = x.Field<int>(GTIService.Constants.AssetService.Fields.SRD_SRH_HDR),
                                SRD_PK = x.Field<int>(GTIService.Constants.AssetService.Fields.SRD_PK),
                                SRH_CURRENCY = x.Field<int>(GTIService.Constants.AssetService.Fields.SRH_CURRENCY),
                                SRH_CURRENCY_TEXT = x.Field<string>(GTIService.Constants.AssetService.Fields.SRH_CURRENCY_TEXT),
                                SRH_DEPT = x.Field<int>(GTIService.Constants.AssetService.Fields.SRH_DEPT),
                                SRH_DEPT_STORE = x.Field<int>(GTIService.Constants.AssetService.Fields.SRH_DEPT_STORE),               
                                SRD_SRH_NO = x.Field<string>(GTIService.Constants.AssetService.Fields.SRD_SRH_NO),
                                SRH_VENDOR_TEXT = x.Field<string>(GTIService.Constants.AssetService.Fields.SRH_VENDOR_TEXT),
                                SRH_VENDOR = x.Field<int>(GTIService.Constants.AssetService.Fields.SRH_VENDOR),
                                SRD_ASSET_TYPE_TEXT = x.Field<string>(GTIService.Constants.AssetService.Fields.SRD_ASSET_TYPE_TEXT),
                                SRH_SERVICE_TYPE = x.Field<Int16>(GTIService.Constants.AssetService.Fields.SRH_SERVICE_TYPE),
                                SRD_ASSET_TEXT = x.Field<string>(GTIService.Constants.AssetService.Fields.SRD_ASSET_TEXT),
                                SRD_AMOUNT = x.Field<decimal>(GTIService.Constants.AssetService.Fields.SRD_AMOUNT)                               
                            })
                            .ToList();
                        break;
                    #endregion
                    #region Selected SR Details
                    case ControlsEnum.SELECTEDSRDETAILS:
                        List<AssetServiceOrderDetails> soDetailsLst = new List<AssetServiceOrderDetails>();
                        AssetServiceOrderHeader objPOInvoiceHeaderMultiple = new AssetServiceOrderHeader();
                        AssetServiceOrderDetails objSODetails;

                        SelectedServiceRequestRoot objSelectedServiceRequestRoot = new SelectedServiceRequestRoot();
                        objSelectedServiceRequestRoot.SRDPKList = tempSelectedSRDetailPKList;//SelectedSRDetailPKList;
                        string xmlDoc = CommonFunctions.XmlSerialize<SelectedServiceRequestRoot>(objSelectedServiceRequestRoot);
                        //AssetServiceOrderHeader objAssetServiceOrderHeader = new AssetServiceOrderHeader();
                        multipleSRHeaderObj = ServiceOrderBL.GetSelectedServiceRequestDetail(xmlDoc);
                        if (multipleSRHeaderObj != null && multipleSRHeaderObj.MultipleSRList != null && multipleSRHeaderObj.MultipleSRList.Count > 0)
                        {
                            objPOInvoiceHeaderMultiple = multipleSRHeaderObj.MultipleSRList[0].DeepClone();
                            foreach (AssetServiceOrderHeader objPO in multipleSRHeaderObj.MultipleSRList)
                            {
                                #region POInvoiceDetails
                                foreach (AssetServiceOrderDetails tDtl in objPO.Details)
                                {
                                    objSODetails = new AssetServiceOrderDetails();
                                    objSODetails = tDtl.DeepClone();
                                    soDetailsLst.Add(objSODetails);
                                }
                                #endregion                                                               
                            }
                            soDetailsLst.ForEach(dtl =>
                            {
                                if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                {
                                    dtl.Item_details.ForEach(itm =>itm.OID_NET_AMOUNT= itm.OID_AMOUNT - itm.OID_DISCOUNT + itm.OID_TAX);                                    
                                }
                            });
                            if (AssetServiceOrderDetailList!=null && AssetServiceOrderDetailList.Count > 0)
                            {
                                AssetServiceOrderDetailList.AddRange(soDetailsLst);
                            }
                            else
                            {
                                AssetServiceOrderDetailList = soDetailsLst;
                            }
                          
                            #region Resetting the slno,because there is chance for duplicating slno
                            int newSlno = 1;
                            AssetServiceOrderDetailList.ForEach(dtl =>
                                               {
                                                   dtl.OSD_SL_NO = newSlno;
                                                   if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                                   {
                                                       #region Setting Item Slno (OID_SL_NO only)
                                                       foreach (AssetServiceOrderItemDetails itm in dtl.Item_details)
                                                       {
                                                           itm.OID_SL_NO = newSlno;                                                           
                                                       }
                                                       #endregion
                                                   }
                                                   newSlno++;
                                               }); 
                            #endregion

                            if(AssetServiceOrderHeaderSession==null)
                                AssetServiceOrderHeaderSession = new AssetServiceOrderHeader();
                            AssetServiceOrderHeaderSession.Details = AssetServiceOrderDetailList;
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
                    #region SRFROMINBOX
                    case ControlsEnum.SRFROMINBOX:
                        dtSR = BusinessLogic.AssetService.ServiceOrderBL.GetSRVendorStoreDetails(GetNullableInt(SR_PK.Value) ?? 0);
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
                    case ControlsEnum.ASSETSERVICEORDERDTL:
                        BindGrid(ControlsEnum.ASSETSERVICEORDERDTL);
                        break;
                    case ControlsEnum.ASSETSERVICEORDER_ITEMDTL:
                        BindGrid(ControlsEnum.ASSETSERVICEORDER_ITEMDTL);
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
                    #region PENDINGREQUESTS
                    case ControlsEnum.PENDINGREQUESTS:
                        BindGrid(ControlsEnum.PENDINGREQUESTS);
                        break; 
                    #endregion
                    #region SELECTEDSRDETAILS
                    case ControlsEnum.SELECTEDSRDETAILS:
                        BindGrid(ControlsEnum.SELECTEDSRDETAILS);
                        break;
                    #endregion
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXTYPES:
                        BindDropDown(controlType);
                        break;
                    #region POSFROMINBOX
                    case ControlsEnum.SRFROMINBOX:
                        txtDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        if (dtSR != null && dtSR.Rows.Count > 0)
                        {
                            lblTrxNo.Text = GetGlobalResourceObject("Messages", "DocGenerationNew").ToString();
                            txtVendorName.Text =  HttpUtility.HtmlDecode(dtSR.Rows[0]["SRH_VENDOR_TEXT"].ToString());
                            hdfVendor.Value = dtSR.Rows[0]["SRH_VENDOR"].ToString(); 
                            ddlRequestingStore.SelectedValue = dtSR.Rows[0]["SRH_DEPT_STORE"].ToString();
                            ddlServiceType.SelectedValue = dtSR.Rows[0]["SRH_SERVICE_TYPE"].ToString();
                            //Disabling Vendor,Servicetype & Requesting Store Controls                           
                            hdfDisableVendorStoreSType.Value = "1"; 

                            ActionHandler(ddlRequestingStore, EventArgs.Empty);
                            int srhPk;
                            srhPk = GetNullableInt(dtSR.Rows[0]["SRH_PK"].ToString()) ?? 0;
                            foreach (GridViewRow grdrow in grdPendingSRItems.Rows)
                            {
                                int srId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSRH_PK")).Value);
                                if (srId == srhPk)
                                {
                                    CheckBox chk;
                                    chk = (CheckBox)grdrow.FindControl("chkSelectPOList");
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
                    #region ASSETSERVICEORDERHDR
                    case ControlsEnum.ASSETSERVICEORDERHDR:
                        if (AssetServiceOrderHeaderSession != null)
                        {
                            objAssetServiceOrderHeader = AssetServiceOrderHeaderSession;
                            objAssetServiceOrderHeader.OSH_PK = CurrPK;
                            objAssetServiceOrderHeader.OSH_NO = lblTrxNo.Text;
                            objAssetServiceOrderHeader.OSH_DATE = Convert.ToDateTime(txtDate.Text);
                            objAssetServiceOrderHeader.OSH_VENDOR = String.IsNullOrEmpty(hdfVendor.Value) ? 0 : Convert.ToInt32(hdfVendor.Value);
                            objAssetServiceOrderHeader.OSH_SERVICE_TYPE = String.IsNullOrEmpty(ddlServiceType.SelectedValue) ? 0 : Convert.ToInt32(ddlServiceType.SelectedValue);
                            objAssetServiceOrderHeader.OSH_REMARKS = HttpUtility.HtmlEncode(txtWorkDescription.Text);
                            objAssetServiceOrderHeader.OSH_ACTIVE = Convert.ToInt32(DbActiveStatus.ACTIVE);
                            objAssetServiceOrderHeader.OSH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            objAssetServiceOrderHeader.OSH_DEPT_STORE = Convert.ToInt16(ddlRequestingStore.SelectedValue);
                            objAssetServiceOrderHeader.OSH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);

                            objAssetServiceOrderHeader.OSH_SUB_TOTAL = string.IsNullOrEmpty(txtHdrSubTotal.Text) ? 0 : Convert.ToDouble(txtHdrSubTotal.Text);
                            objAssetServiceOrderHeader.OSH_DISCOUNT = string.IsNullOrEmpty(txtHdrDiscount.Text) ? 0 : Convert.ToDouble(txtHdrDiscount.Text);
                            objAssetServiceOrderHeader.OSH_TAX = string.IsNullOrEmpty(txtHdrTax.Text) ? 0 : Convert.ToDouble(txtHdrTax.Text);
                            objAssetServiceOrderHeader.OSH_OTH_CHARGE = string.IsNullOrEmpty(txtHdrOtherCharges.Text) ? 0 : Convert.ToDouble(txtHdrOtherCharges.Text);
                            objAssetServiceOrderHeader.OSH_PRICE_ADJ = string.IsNullOrEmpty(txtHdrPriceAdj.Text) ? 0 : Convert.ToDouble(txtHdrPriceAdj.Text);
                            objAssetServiceOrderHeader.OSH_NET_TOTAL = string.IsNullOrEmpty(txtHdrGrandTotal.Text) ? 0 : Convert.ToDouble(txtHdrGrandTotal.Text);//GrandTotal

                            objAssetServiceOrderHeader.OSH_BIZUNIT = Convert.ToInt32(currentUser.SBUID);
                            objAssetServiceOrderHeader.USER_PK = Convert.ToInt32(currentUser.PKUser);
                            objAssetServiceOrderHeader.OSH_CRTD_BY = Convert.ToInt32(currentUser.PKUser);
                            objAssetServiceOrderHeader.OSH_CRTD_DT = DateTime.Now;
                            objAssetServiceOrderHeader.LAST_MOD_DT = LastModifiedTime;

                            AssetServiceOrderDetailList.ForEach(dtl =>
                            {
                                double osdAmount = 0;
                                if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                {
                                    osdAmount = dtl.Item_details.Sum(r => r.OID_NET_AMOUNT);
                                    #region Setting Tax Slno
                                    foreach (AssetServiceOrderItemDetails itm in dtl.Item_details)
                                    {
                                        if (itm.Tax_DTL != null && itm.Tax_DTL.Count > 0)
                                        itm.Tax_DTL.ForEach(f => f.SSD_ITEM_SL_NO = itm.OID_ITEM_SL_NO);
                                    }
                                    #endregion
                                }
                                dtl.OSD_AMOUNT = osdAmount;
                                //For Encoding
                                dtl.OSD_ASSET_TYPE_TEXT = HttpUtility.HtmlEncode(dtl.OSD_ASSET_TYPE_TEXT);
                                dtl.OSD_ASSET_TEXT = HttpUtility.HtmlEncode(dtl.OSD_ASSET_TEXT);
                            });
                            objAssetServiceOrderHeader.Details = AssetServiceOrderDetailList;
                        }
                        retObject = objAssetServiceOrderHeader;
                        break;
                    #endregion
                    #region ASSETSERVICEORDERDTL
                    case ControlsEnum.ASSETSERVICEORDERDTL:
                        if (CurrSlNo != 0 && AssetServiceOrderDetailList != null)
                        {
                            assetServiceOrderDetailsObj = AssetServiceOrderDetailList.SingleOrDefault(itm => itm.OSD_SL_NO == CurrSlNo);
                            if (assetServiceOrderDetailsObj != null)
                            {
                                assetServiceOrderDetailsObj.OSD_PK = OSDPK;
                                assetServiceOrderDetailsObj.OSD_OSH_HDR = CurrPK;                               
                                assetServiceOrderDetailsObj.OSD_ASSET_TYPE = Convert.ToInt32(hdfAssetType.Value);
                                assetServiceOrderDetailsObj.OSD_ASSET_TYPE_TEXT = HttpUtility.HtmlEncode(txtAssetType.Text);
                                assetServiceOrderDetailsObj.OSD_ASSET = Convert.ToInt32(hdfAsset.Value);
                                assetServiceOrderDetailsObj.OSD_ASSET_TEXT = HttpUtility.HtmlEncode(txtAsset.Text);
                                assetServiceOrderDetailsObj.OSD_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                                assetServiceOrderDetailsObj.OSD_OSH_NO = lblTrxNo.Text;
                                assetServiceOrderDetailsObj.OSD_OSH_DATE = Convert.ToDateTime(txtDate.Text);                              

                                retObject = assetServiceOrderDetailsObj;
                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (AssetServiceOrderDetailList == null || AssetServiceOrderDetailList.Count == 0)
                            {
                                AssetServiceOrderDetailList = new List<AssetServiceOrderDetails>();
                                slno = 1;
                            }
                            else
                            {
                                slno = AssetServiceOrderDetailList.Max(itm => itm.OSD_SL_NO);
                                slno++;
                            }
                            assetServiceOrderDetailsObj = new AssetServiceOrderDetails();
                            CurrSlNo = assetServiceOrderDetailsObj.OSD_SL_NO = slno;
                            assetServiceOrderDetailsObj.OSD_OSH_HDR = CurrPK;
                            assetServiceOrderDetailsObj.OSD_PK = 0;
                            assetServiceOrderDetailsObj.OSD_ASSET_TYPE = Convert.ToInt32(hdfAssetType.Value);
                            assetServiceOrderDetailsObj.OSD_ASSET_TYPE_TEXT = HttpUtility.HtmlEncode(txtAssetType.Text);
                            assetServiceOrderDetailsObj.OSD_ASSET = Convert.ToInt32(hdfAsset.Value);
                            assetServiceOrderDetailsObj.OSD_ASSET_TEXT = HttpUtility.HtmlEncode(txtAsset.Text);
                            assetServiceOrderDetailsObj.OSD_DESC = HttpUtility.HtmlEncode(txtDescription.Text);
                            assetServiceOrderDetailsObj.OSD_OSH_NO = lblTrxNo.Text;
                            assetServiceOrderDetailsObj.OSD_OSH_DATE = Convert.ToDateTime(txtDate.Text);                          

                            AssetServiceOrderDetailList.Add(assetServiceOrderDetailsObj);
                        }
                        retObject = AssetServiceOrderDetailList;
                        break;
                    #endregion
                    #region ASSETSERVICEORDER_ITEMDTL
                    case ControlsEnum.ASSETSERVICEORDER_ITEMDTL:
                        if (AssetServiceOrderDetailList != null)
                        {
                            if (CurrSlNo != 0)
                            {
                                AssetServiceOrderItemDetails ItemDetailsObj;
                                AssetServiceOrderDetails assetServiceOrderDetailsObj = AssetServiceOrderDetailList.SingleOrDefault(itm => itm.OSD_SL_NO == CurrSlNo);
                                if (assetServiceOrderDetailsObj.Item_details == null)
                                {
                                    assetServiceOrderDetailsObj.Item_details = new List<AssetServiceOrderItemDetails>();
                                }
                                if (assetServiceOrderDetailsObj.Item_details.Count > 0 && ItemRowIndex >= 0)
                                {
                                    ItemDetailsObj = assetServiceOrderDetailsObj.Item_details[ItemRowIndex];
                                    ItemDetailsObj.OID_ASR_ITEM = string.IsNullOrEmpty(hdfPopupItemPk.Value) ? 0 : Convert.ToInt32(hdfPopupItemPk.Value);
                                    ItemDetailsObj.OID_ASR_ITEM_TEXT = txtPopupItems.Text;
                                    ItemDetailsObj.OID_QTY = string.IsNullOrEmpty(txtPopupQty.Text) ? 0 : Convert.ToDouble(txtPopupQty.Text);
                                    ItemDetailsObj.OID_RATE =Math.Round(Convert.ToDouble(txtPopupRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                                     ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P])));                                    
                                    ItemDetailsObj.OID_UOM = string.IsNullOrEmpty(hdfPopupUomPk.Value) ? 0 : Convert.ToInt32(hdfPopupUomPk.Value);
                                    ItemDetailsObj.OID_UOM_TEXT = txtPopupUOM.Text;
                                    ItemDetailsObj.OID_AMOUNT = string.IsNullOrEmpty(txtPopupAmount.Text) ? 0 : Convert.ToDouble(txtPopupAmount.Text);
                                    ItemDetailsObj.OID_DISCOUNT = !string.IsNullOrEmpty(txtPopupDiscount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtPopupDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                    ItemDetailsObj.OID_TAX = !string.IsNullOrEmpty(txtPopupTax.Text.Trim()) ? Math.Round(Convert.ToDouble(txtPopupTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                    ItemDetailsObj.OID_NET_AMOUNT = Math.Round(ItemDetailsObj.OID_AMOUNT - ItemDetailsObj.OID_DISCOUNT + ItemDetailsObj.OID_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    ItemDetailsObj.OID_REMARKS = HttpUtility.HtmlEncode(txtPopupRemarks.Text);
                                }
                                else
                                {
                                    int slno = 1;
                                    if (assetServiceOrderDetailsObj.Item_details == null || assetServiceOrderDetailsObj.Item_details.Count == 0)
                                    {
                                        assetServiceOrderDetailsObj.Item_details = new List<AssetServiceOrderItemDetails>();
                                        slno = 1;
                                        int assetCount = 0,j=0;
                                        assetCount = AssetServiceOrderDetailList.Count;
                                        if (assetCount > 0)
                                        {
                                            j = assetCount;
                                            do
                                            {
                                                j--;
                                                if (j >= 0)
                                                {
                                                    if (AssetServiceOrderDetailList[j].Item_details.Count > 0)
                                                    {
                                                        slno = AssetServiceOrderDetailList[j].Item_details.Max(itm => itm.OID_ITEM_SL_NO);
                                                        slno++;
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            while (AssetServiceOrderDetailList[j].Item_details.Count == 0 && j>=0);                                          
                                        }
                                    }
                                    else
                                    {
                                        slno = assetServiceOrderDetailsObj.Item_details.Max(itm => itm.OID_ITEM_SL_NO);
                                        slno++;
                                    }                                  
                                    ItemDetailsObj = new AssetServiceOrderItemDetails();
                                    ItemDetailsObj.OID_PK = 0;
                                    ItemDetailsObj.OID_SL_NO = CurrSlNo;
                                    ItemDetailsObj.OID_OSD_PK = OSDPK;
                                    ITEM_SL_NO=ItemDetailsObj.OID_ITEM_SL_NO = slno;
                                    ItemRowIndex = assetServiceOrderDetailsObj.Item_details.Count;//Setting ItemRowIndex.This is used in Tax & Discount Adding
                                    ItemDetailsObj.OID_ASR_ITEM = string.IsNullOrEmpty(hdfPopupItemPk.Value) ? 0: Convert.ToInt32(hdfPopupItemPk.Value) ;
                                    ItemDetailsObj.OID_ASR_ITEM_TEXT = txtPopupItems.Text;
                                    ItemDetailsObj.OID_QTY = string.IsNullOrEmpty(txtPopupQty.Text) ? 0 : Convert.ToDouble(txtPopupQty.Text);
                                    ItemDetailsObj.OID_RATE = Math.Round(Convert.ToDouble(txtPopupRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                                      ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P])));
                                    ItemDetailsObj.OID_UOM = string.IsNullOrEmpty(hdfPopupUomPk.Value) ? 0 : Convert.ToInt32(hdfPopupUomPk.Value);
                                    ItemDetailsObj.OID_UOM_TEXT = txtPopupUOM.Text;
                                    ItemDetailsObj.OID_AMOUNT = string.IsNullOrEmpty(txtPopupAmount.Text) ? 0 : Convert.ToDouble(txtPopupAmount.Text);
                                    ItemDetailsObj.OID_DISCOUNT = !string.IsNullOrEmpty(txtPopupDiscount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtPopupDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                    ItemDetailsObj.OID_TAX = !string.IsNullOrEmpty(txtPopupTax.Text.Trim()) ? Math.Round(Convert.ToDouble(txtPopupTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                    ItemDetailsObj.OID_NET_AMOUNT = Math.Round(ItemDetailsObj.OID_AMOUNT - ItemDetailsObj.OID_DISCOUNT + ItemDetailsObj.OID_TAX, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                    ItemDetailsObj.OID_REMARKS = HttpUtility.HtmlEncode(txtPopupRemarks.Text);                                   

                                    assetServiceOrderDetailsObj.Item_details.Add(ItemDetailsObj);
                                }
                            }
                            AssetServiceOrderDetailList.ForEach(dtl =>
                            {
                                if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                {
                                    dtl.OSD_AMOUNT = dtl.Item_details.Sum(r => r.OID_NET_AMOUNT);
                                }
                            });
                        }
                        retObject = AssetServiceOrderDetailList;
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
                        if (objAssetServiceOrderHeader != null)
                        {
                            txtDate.Text = Convert.ToDateTime(objAssetServiceOrderHeader.OSH_DATE).ToString(Resources.Constants.HRMSDateFormatShort);
                            lblTrxNo.Text = string.IsNullOrEmpty(objAssetServiceOrderHeader.OSH_NO) ? Resources.ErpRes.Draft : objAssetServiceOrderHeader.OSH_NO;
                            hdfVendor.Value = objAssetServiceOrderHeader.OSH_VENDOR.ToString();
                            txtVendorName.Text = HttpUtility.HtmlDecode(objAssetServiceOrderHeader.OSH_VENDOR_TEXT.ToString());
                            ddlServiceType.SelectedValue = objAssetServiceOrderHeader.OSH_SERVICE_TYPE.ToString();
                            ddlRequestingStore.SelectedValue = objAssetServiceOrderHeader.OSH_DEPT_STORE.ToString();                            
                            hdfCurrency.Value = objAssetServiceOrderHeader.OSH_CURRENCY.ToString();
                            txtWorkDescription.Text = HttpUtility.HtmlDecode(objAssetServiceOrderHeader.OSH_REMARKS);
                            //Disabling Vendor,Servicetype & Requesting Store Controls                           
                            hdfDisableVendorStoreSType.Value = "1"; 

                            ActionHandler(ddlRequestingStore, EventArgs.Empty);
                            #region Adding Saved SR to SelectedSRDetailPKList 
                            if (SelectedSRDetailPKList == null)
                                SelectedSRDetailPKList = new List<SelectedServiceReqDtls>();
                            List<SelectedServiceReqDtls> tempSelectedSRDetailPKList = new List<SelectedServiceReqDtls>();
                            foreach (var item in objAssetServiceOrderHeader.Details)
                            {
                                SelectedServiceReqDtls objselSRList = new SelectedServiceReqDtls();
                                objselSRList.SRH_PK = Convert.ToInt32(objAssetServiceOrderHeader.OSH_PK);
                                objselSRList.SRD_PK = Convert.ToInt32(item.OSD_SRD_DTL);
                                objselSRList.SRH_CURRENCY = Convert.ToInt32(objAssetServiceOrderHeader.OSH_CURRENCY);
                                objselSRList.OSD_PK = Convert.ToInt32(item.OSD_PK);
                                tempSelectedSRDetailPKList.Add(objselSRList);

                                if (PendingSRList != null && PendingSRList.Count > 0)
                                {
                                    PendingServiceRequest tempPendingSR = PendingSRList.Where(x => x.SRD_PK == objselSRList.SRD_PK).Single();
                                    tempPendingSR.AddedToStockList = true;
                                    tempPendingSR.CheckBoxChecked = true;
                                }
                            }
                            if (tempSelectedSRDetailPKList != null && tempSelectedSRDetailPKList.Count > 0)
                            {
                                SelectedSRDetailPKList = tempSelectedSRDetailPKList;
                            }
                            #endregion

                            txtHdrSubTotal.Text = objAssetServiceOrderHeader.OSH_SUB_TOTAL.ToString();
                            txtHdrDiscount.Text = objAssetServiceOrderHeader.OSH_DISCOUNT.ToString();
                            txtHdrTax.Text = objAssetServiceOrderHeader.OSH_TAX.ToString();
                            txtHdrOtherCharges.Text = objAssetServiceOrderHeader.OSH_OTH_CHARGE.ToString();
                            txtHdrPriceAdj.Text = objAssetServiceOrderHeader.OSH_PRICE_ADJ.ToString();
                            txtHdrGrandTotal.Text = objAssetServiceOrderHeader.OSH_NET_TOTAL.ToString();

                            LastModifiedTime = objAssetServiceOrderHeader.OSH_MOD_DT;
                            Status = objAssetServiceOrderHeader.OSH_STATUS;                           
                            hdfIsCancelled.Value = Convert.ToString(objAssetServiceOrderHeader.OSH_DEL_STATUS);

                            AssetServiceOrderDetailList = objAssetServiceOrderHeader.Details;
                            TempAssetServiceOrderDetailList = AssetServiceOrderDetailList;
                            SetFieldValues(ControlsEnum.ASSETSERVICEORDERDTL);
                            SetSubTotal();
                            SetHdrTax();
                        }
                        break;
                    #endregion                   
                    #region FILLASSETDETAILS
                    case ControlsEnum.FILLASSETDETAILS:
                        if (assetServiceOrderDetailsObj != null)
                        {

                            CurrSlNo = assetServiceOrderDetailsObj.OSD_SL_NO;
                            txtAsset.Text = HttpUtility.HtmlDecode(assetServiceOrderDetailsObj.OSD_ASSET_TEXT);
                            hdfAsset.Value = assetServiceOrderDetailsObj.OSD_ASSET.ToString();
                            txtAssetType.Text = HttpUtility.HtmlDecode(assetServiceOrderDetailsObj.OSD_ASSET_TYPE_TEXT);
                            hdfAssetType.Value = assetServiceOrderDetailsObj.OSD_ASSET_TYPE.ToString();
                            txtDescription.Text = HttpUtility.HtmlDecode(assetServiceOrderDetailsObj.OSD_DESC);
                        }
                        break; 
                    #endregion
                    #region FILLITEMDETAILS
                    case ControlsEnum.FILLITEMDETAILS:
                        if (assetServiceOrderItemDetObj != null)
                        {
                            SID_SL_NO = assetServiceOrderItemDetObj.OID_SL_NO;
                            txtPopupItems.Text = HttpUtility.HtmlDecode(assetServiceOrderItemDetObj.OID_ASR_ITEM_TEXT);
                            hdfPopupItemPk.Value = assetServiceOrderItemDetObj.OID_ASR_ITEM.ToString();
                            txtPopupQty.Text = assetServiceOrderItemDetObj.OID_QTY.ToString();
                            txtPopupRate.Text = GetFormattedRate(assetServiceOrderItemDetObj.OID_RATE);
                            txtPopupAmount.Text = GetFormattedCurrency(assetServiceOrderItemDetObj.OID_AMOUNT);
                            txtPopupDiscount.Text = GetFormattedCurrency(assetServiceOrderItemDetObj.OID_DISCOUNT);
                            txtPopupTax.Text = GetFormattedCurrency(assetServiceOrderItemDetObj.OID_TAX);
                            txtPopupTotAmt.Text = GetFormattedCurrency(assetServiceOrderItemDetObj.OID_NET_AMOUNT);
                            txtPopupUOM.Text = assetServiceOrderItemDetObj.OID_UOM_TEXT.ToString();
                            hdfPopupUomPk.Value = assetServiceOrderItemDetObj.OID_UOM.ToString();
                            txtPopupRemarks.Text = assetServiceOrderItemDetObj.OID_REMARKS;                         
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
                    #region PENDINGREQUESTS
                    case ControlsEnum.PENDINGREQUESTS:
                        grdPendingSRItems.DataSource = PendingSRList;
                        grdPendingSRItems.DataBind();
                        break;
                    #endregion
                    #region ASSETSERVICEORDERDTL
                    case ControlsEnum.ASSETSERVICEORDERDTL:
                        if (AssetServiceOrderHeaderSession != null)
                            AssetServiceOrderHeaderSession.Details = AssetServiceOrderDetailList;
                        grdAssetDetails.DataSource = AssetServiceOrderDetailList;
                        grdAssetDetails.DataBind();                       
                        #region If there is no Asset Details in Grid,Enabling Vendor,ServiceType,Store Controls
                        if (AssetServiceOrderDetailList != null)
                        {
                            if (AssetServiceOrderDetailList.Count == 0)
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
                            taxHdrList = TempAssetServiceOrderHeaderSession.Tax_HDR == null ? new List<AssetServiceOrderTaxHdr>() :
                                TempAssetServiceOrderHeaderSession.Tax_HDR.Where(tax => Convert.ToInt32(tax.SSD_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            soDetailsObj = EditTempAssetServiceOrderHeaderSession.Details == null ? null :
                                       EditTempAssetServiceOrderHeaderSession.Details.SingleOrDefault(row => CurrSlNo == row.OSD_SL_NO);
                            if (ItemRowIndex >= 0)
                            {
                                assetServiceOrderItemDetObj = soDetailsObj.Item_details[ItemRowIndex];
                            }                          
                            if (assetServiceOrderItemDetObj != null)
                            {
                                taxHdrList = assetServiceOrderItemDetObj.Tax_DTL == null ? new List<AssetServiceOrderTaxHdr>() :
                                    assetServiceOrderItemDetObj.Tax_DTL.Where(tax => Convert.ToInt32(tax.SSD_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                            else
                                taxHdrList = new List<AssetServiceOrderTaxHdr>();
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
                    txtListFromDate.Text =string.Empty;// DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtListToDate.Text = string.Empty;//DateTime.Now.ToString(Resources.Constants.HRMSDateFormatShort);
                    txtListTrxNo.Text = string.Empty;                    
                    txtListVendorName.Text = Resources.ErpRes.AutoDefaultValue;
                    hdfListVendor.Value = string.Empty;
                    ddlListServiceType.SelectedValue = "0";
                    ddlStatus.SelectedValue = "-1";
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
                    AssetServiceOrderHeaderSession = null;
                    TempAssetServiceOrderHeaderSession = null;
                    EditTempAssetServiceOrderHeaderSession = null;
                    AssetServiceOrderDetailList = null;
                    TempAssetServiceOrderDetailList = null;
                    SelectedSRDetailPKList = null;
                    PendingSRList = null;
                    hdfDisableVendorStoreSType.Value = "0";
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
                    RowIndex =-1;                   
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
        private void SetDetailTax(AssetServiceOrderHeader expenseHdr)
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
        private bool SetItemTax(AssetServiceOrderHeader expenseHdr)
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
                    objAssetServiceOrderHeader = expenseHdr;
                    soDetailsObj = objAssetServiceOrderHeader.Details.SingleOrDefault(crt =>crt.OSD_SL_NO == CurrSlNo);
                    if (ItemRowIndex >= 0)
                    soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                    if (soDetailsItemObj != null)
                    {
                        if (soDetailsItemObj.Tax_DTL != null)
                        {
                            var discDetail = soDetailsItemObj.Tax_DTL.Where(quotation => quotation.SSD_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (AssetServiceOrderTaxHdr taxHdrObj in discDetail)
                            {
                                string taxFormula = taxHdrObj.SSD_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    taxHdrObj.SSD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                }
                            }
                            discount = soDetailsItemObj.Tax_DTL.Where(ctr => ctr.SSD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.SSD_TAX_AMT);
                        }
                        netAmount = amount - discount;
                        txtPopupDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                        if (soDetailsItemObj.Tax_DTL != null)
                        {
                            var taxDetail = soDetailsItemObj.Tax_DTL.Where(ctr => ctr.SSD_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (AssetServiceOrderTaxHdr taxHdrObj in taxDetail)
                            {
                                string taxFormula = taxHdrObj.SSD_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    //1 :- No need to create formula for manual entry of Item Tax amount in TAX POPUP.  
                                    //0 :- Create tax formula
                                    if (hdfApplyTax.Value == "0")
                                    {
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                        taxHdrObj.SSD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                    }
                                }
                            }
                            hdfApplyTax.Value = "0";
                            itmTax = soDetailsItemObj.Tax_DTL.ToList().Where(ctr => ctr.SSD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(ctr => ctr.SSD_TAX_AMT);
                        }
                        txtPopupTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                        soDetailsItemObj.OID_AMOUNT = Math.Round(amount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soDetailsItemObj.OID_DISCOUNT = Math.Round(discount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soDetailsItemObj.OID_TAX = Math.Round(itmTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soDetailsItemObj.OID_NET_AMOUNT = Math.Round(amount - discount + itmTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        txtTotal.Text = soDetailsItemObj.OID_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);



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
            double discount,OtherCharge;
            double adjust;
            amount = 0;
            adjust = 0;
            OtherCharge = 0;
            if (AssetServiceOrderHeaderSession != null)
            {
                objAssetServiceOrderHeader = AssetServiceOrderHeaderSession;
                amount = Convert.ToDouble(objAssetServiceOrderHeader.OSH_SUB_TOTAL);
                discount = 0;
                if (objAssetServiceOrderHeader.Tax_HDR != null)
                {
                    var discHeader = objAssetServiceOrderHeader.Tax_HDR.Where(hdr => hdr.SSD_TAX_CATEGORY == ((int)TaxType.Discount));
                    foreach (AssetServiceOrderTaxHdr taxHdrObj in discHeader)
                    {
                        string taxFormula = taxHdrObj.SSD_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SSD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                        }
                    }
                    discount = objAssetServiceOrderHeader.Tax_HDR.Where(quotation => quotation.SSD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.SSD_TAX_AMT);
                }
                objAssetServiceOrderHeader.OSH_DISCOUNT = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;


                #region OtherCharge
                if (objAssetServiceOrderHeader.Tax_HDR != null)
                {
                    var otherChargeHeader = objAssetServiceOrderHeader.Tax_HDR.Where(hdr => hdr.SSD_TAX_CATEGORY == ((int)TaxType.Shipping));
                    foreach (AssetServiceOrderTaxHdr taxHdrObj in otherChargeHeader)
                    {
                        string taxFormula = taxHdrObj.SSD_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.SSD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                        }
                    }
                    OtherCharge = objAssetServiceOrderHeader.Tax_HDR.Where(quotation => quotation.SSD_TAX_CATEGORY == ((int)TaxType.Shipping)).Sum(quotation => quotation.SSD_TAX_AMT);
                }
                objAssetServiceOrderHeader.OSH_OTH_CHARGE = OtherCharge;
                txtHdrOtherCharges.Text = txtHdrOtherCharges.ToolTip = OtherCharge.ToString(hdfCurrencyFormat.Value);
                if (IsTaxForOtherCharge.Value == "1")
                {
                    amount = amount + OtherCharge;
                }
                #endregion


                if (objAssetServiceOrderHeader.Tax_HDR != null)
                {
                    var taxHeader = objAssetServiceOrderHeader.Tax_HDR.Where(quotation => quotation.SSD_TAX_CATEGORY == ((int)TaxType.Tax));
                    foreach (AssetServiceOrderTaxHdr taxHdrObj in taxHeader)
                    {
                        string taxFormula = taxHdrObj.SSD_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            //1 :- No need to create formula for manual entry of Header Tax amount in TAX POPUP.  
                            //0 :- Create tax formula
                            if (hdfApplyHdrTax.Value == "0")
                            {
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                taxHdrObj.SSD_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            }
                        }
                    }
                    hdfApplyHdrTax.Value = "0";
                    objAssetServiceOrderHeader.OSH_TAX = objAssetServiceOrderHeader.Tax_HDR.Where(quotation => quotation.SSD_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.SSD_TAX_AMT);
                }
                txtHdrTax.Text = txtHdrTax.ToolTip = objAssetServiceOrderHeader.OSH_TAX.ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtHdrPriceAdj.Text, out adjust);
                objAssetServiceOrderHeader.OSH_PRICE_ADJ = adjust;
                objAssetServiceOrderHeader.OSH_NET_TOTAL = Convert.ToDouble(objAssetServiceOrderHeader.OSH_SUB_TOTAL) - objAssetServiceOrderHeader.OSH_DISCOUNT + objAssetServiceOrderHeader.OSH_OTH_CHARGE + objAssetServiceOrderHeader.OSH_TAX
                     + objAssetServiceOrderHeader.OSH_PRICE_ADJ;

                txtHdrGrandTotal.Text = txtHdrGrandTotal.ToolTip = objAssetServiceOrderHeader.OSH_NET_TOTAL.ToString(hdfCurrencyFormat.Value);
                AssetServiceOrderHeaderSession = objAssetServiceOrderHeader;
                TempAssetServiceOrderHeaderSession = objAssetServiceOrderHeader;
            }
            return true;
        }
        private void SetSubTotal()
        {
            if (AssetServiceOrderHeaderSession != null)
            {
                AssetServiceOrderHeaderSession.OSH_SUB_TOTAL = AssetServiceOrderHeaderSession.Details.Sum(dtl => dtl.OSD_AMOUNT);
                txtHdrSubTotal.ToolTip = txtHdrSubTotal.Text = AssetServiceOrderHeaderSession.OSH_SUB_TOTAL.ToString(hdfCurrencyFormat.Value);
                txtHdrPriceAdj.ToolTip = txtHdrPriceAdj.Text = AssetServiceOrderHeaderSession.OSH_PRICE_ADJ.ToString(hdfCurrencyFormat.Value);
                txtHdrDiscount.ToolTip = txtHdrDiscount.Text = AssetServiceOrderHeaderSession.OSH_DISCOUNT.ToString(hdfCurrencyFormat.Value);
                txtHdrTax.ToolTip = txtHdrTax.Text = AssetServiceOrderHeaderSession.OSH_TAX.ToString(hdfCurrencyFormat.Value);
                txtHdrOtherCharges.ToolTip = txtHdrOtherCharges.Text = AssetServiceOrderHeaderSession.OSH_OTH_CHARGE.ToString(hdfCurrencyFormat.Value);
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
            if (AssetServiceOrderDetailList != null)
            {
                List<AssetServiceOrderItemDetails> lstASRItemDetails = new List<AssetServiceOrderItemDetails>();
                AssetServiceOrderDetails assetServiceOrderDetailsObj = AssetServiceOrderDetailList.SingleOrDefault(itm => itm.OSD_SL_NO == CurrSlNo);
                if (assetServiceOrderDetailsObj.Item_details != null)
                {
                    lstASRItemDetails = assetServiceOrderDetailsObj.Item_details.DeepClone();
                    if (lstASRItemDetails.Count > 0 && ItemRowIndex >= 0)
                    {
                        lstASRItemDetails.RemoveAt(ItemRowIndex);
                    }
                    foreach (AssetServiceOrderItemDetails item in lstASRItemDetails)
                    {
                        if (item.OID_ASR_ITEM_TEXT == itemName)
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
            SetUIEditView(ActionsEnum.EDIT);
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
                AssetServiceOrderTaxHdr tempInvTaxSplitObj = null;
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
                        commonActions = ActionsEnum.PENDINGREQUESTS;
                    }
                    else if (((DropDownList)sender).ID == "ddlServiceType")
                    {
                        commonActions = ActionsEnum.PENDINGREQUESTS;
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
                                selectedPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSH_PK")).Value);
                                hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                                hdfStatus = grdrow.FindControl("hdfStatus") as HiddenField;
                                hdfSelRecordStatus.Value = hdfStatus.Value.ToString();//For btnEditforCancel show/Hide
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
                        SetFieldValues(ControlsEnum.PENDINGREQUESTS);         
                        SetFieldValues(ControlsEnum.ASSETSERVICEORDERDTL);
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
                            if (AssetServiceOrderDetailList == null)
                                AssetServiceOrderDetailList = new List<AssetServiceOrderDetails>();
                            AssetServiceOrderDetailList = (List<AssetServiceOrderDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERDTL);
                            if (AssetServiceOrderDetailList != null && AssetServiceOrderDetailList.Count > 0)
                            {
                                SetFieldValues(ControlsEnum.ASSETSERVICEORDERDTL);
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
                        if (AssetServiceOrderDetailList != null && AssetServiceOrderDetailList.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdAssetDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][1]);
                            if (CurrSlNo > 0)
                            {
                                assetServiceOrderDetailsObj = AssetServiceOrderDetailList.SingleOrDefault(row => CurrSlNo == row.OSD_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.FILLASSETDETAILS);
                            }
                        }
                        break;
                    #endregion
                    #region REMOVEASSET
                    case ActionsEnum.REMOVEASSET:
                        grvRow = (GridViewRow)((ImageButton)(sender)).Parent.Parent;
                        RowIndex = grvRow.RowIndex;
                        if (AssetServiceOrderDetailList != null && AssetServiceOrderDetailList.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                int srdPk = 0;
                                srdPk = AssetServiceOrderDetailList.Where(row => CurrSlNo == row.OSD_SL_NO).SingleOrDefault().OSD_SRD_DTL;
                                AssetServiceOrderDetailList = AssetServiceOrderDetailList.Where(row => CurrSlNo != row.OSD_SL_NO).ToList();
                                SelectedSRDetailPKList = SelectedSRDetailPKList.Where(row => srdPk != row.SRD_PK).ToList();
                                SetFieldValues(ControlsEnum.ASSETSERVICEORDERDTL);
                                SetSubTotal();
                                SetHdrTax();
                            }
                        }
                        ResetForm(ControlsEnum.CLEARADD);
                        break;
                    #endregion
                    #endregion
                    #region PENDINGREQUESTS
                    case ActionsEnum.PENDINGREQUESTS:
                        int vendorPK = 0;
                        vendorPK = string.IsNullOrEmpty(hdfVendor.Value) ? 0 : Convert.ToInt32(hdfVendor.Value);
                        if (ddlRequestingStore.SelectedIndex > 0 && ddlServiceType.SelectedIndex > 0 && vendorPK>0)
                        {
                            GetFieldValues(ControlsEnum.PENDINGREQUESTS);
                            SetFieldValues(ControlsEnum.PENDINGREQUESTS);
                        }
                        break;
                    #endregion
                    #region ADDTOLIST
                    case ActionsEnum.ADDTOLIST:
                        if (SelectedSRDetailPKList == null)
                            SelectedSRDetailPKList = new List<SelectedServiceReqDtls>();
                        tempSelectedSRDetailPKList = new List<SelectedServiceReqDtls>();
                        //tempSelectedSRDetailPKList = SelectedSRDetailPKList;
                        foreach (GridViewRow grdrow in grdPendingSRItems.Rows)
                        {
                            CheckBox chk;
                            chk = (CheckBox)grdrow.FindControl("chkSelectPOList");
                            if (chk.Checked)
                            {
                                SelectedServiceReqDtls objselSRList = new SelectedServiceReqDtls();
                                objselSRList.SRH_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSRH_PK")).Value);
                                objselSRList.SRD_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSRD_PK")).Value);
                                objselSRList.SRH_CURRENCY = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSRH_CURRENCY")).Value);
                                objselSRList.OSD_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSD_PK")).Value);
                                bool alreadyExists = SelectedSRDetailPKList.Exists(itemLst => itemLst.SRD_PK == objselSRList.SRD_PK);
                                if (!alreadyExists)
                                {
                                    tempSelectedSRDetailPKList.Add(objselSRList);
                                    hdfCurrency.Value = objselSRList.SRH_CURRENCY.ToString();
                                }
                            }
                        }
                        if (tempSelectedSRDetailPKList != null && tempSelectedSRDetailPKList.Count > 0)
                        {
                            if (SelectedSRDetailPKList.Count > 0)
                            {
                                SelectedSRDetailPKList.AddRange(tempSelectedSRDetailPKList);
                            }
                            else
                            {
                                SelectedSRDetailPKList = tempSelectedSRDetailPKList;
                            }
                            GetFieldValues(ControlsEnum.SELECTEDSRDETAILS);
                            SetFieldValues(ControlsEnum.ASSETSERVICEORDERDTL);
                            SetSubTotal();
                            SetHdrTax();
                        }
                        divSOPendingListing.Visible = true;
                        break;
                    #endregion
                    #region SAVE
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (AssetServiceOrderDetailList == null || AssetServiceOrderDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else
                        {
                            objAssetServiceOrderHeader = new AssetServiceOrderHeader();
                            objAssetServiceOrderHeader = (AssetServiceOrderHeader)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERHDR);
                            if (objAssetServiceOrderHeader != null)
                            {
                                if (objAssetServiceOrderHeader.Details != null && objAssetServiceOrderHeader.Details.Count > 0)
                                {
                                    objAssetServiceOrderHeader.WKF_FLAG = 0;
                                    objAssetServiceOrderHeader.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                    string trxNo = string.Empty;
                                    string xmlDoc = CommonFunctions.XmlSerialize<AssetServiceOrderHeader>(objAssetServiceOrderHeader);
                                    result = ServiceOrderBL.SaveServiceOrderDetails(xmlDoc, out trxNo);
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
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " + Resources.Messages.EditUsedByAnotherUser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                        }
                                        else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " + Resources.Messages.AlreadyDeleted;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                        }
                                        else if (result == (int)DbSaveStatus.CODEEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                                        }
                                        else if (result == (int)DbSaveStatus.REFNOEXIST)
                                        {
                                            litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceOrder);
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
                        else if (AssetServiceOrderDetailList == null || AssetServiceOrderDetailList.Count == 0)
                        {
                            litErrorMsg.Text = Resources.Messages.AddAtleastOneItem;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                objAssetServiceOrderHeader = new AssetServiceOrderHeader();
                                objAssetServiceOrderHeader = (AssetServiceOrderHeader)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDERHDR);
                                if (objAssetServiceOrderHeader != null)
                                {
                                    if (objAssetServiceOrderHeader.Details != null && objAssetServiceOrderHeader.Details.Count > 0)
                                    {
                                        TrxNo = string.Empty;
                                        objAssetServiceOrderHeader.WKF_FLAG = 1;
                                        SaveTransaction(objAssetServiceOrderHeader, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
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
                                if (ServiceOrderBL.ValidationForCancellationASO(CurrPK))
                                {
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;

                                    this.CurrPK = base.WkfRefID = 0;
                                    this.ModifiedDatePnl.Visible = false;
                                    GetFieldValues(ControlsEnum.LIST);
                                    SetFieldValues(ControlsEnum.LIST);
                                }

                            }
                            else//Submit
                            {
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }

                        }
                        break;
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
                            AssetServiceOrderHeaderSession = TempAssetServiceOrderHeaderSession;
                            AssetServiceOrderDetailList = AssetServiceOrderHeaderSession.Details;
                            if (!IsItemExists(txtPopupItems.Text.Trim()))
                            {                             
                                AssetServiceOrderDetailList = (List<AssetServiceOrderDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDER_ITEMDTL);
                                if (AssetServiceOrderDetailList != null && AssetServiceOrderDetailList.Count > 0)
                                {
                                    SetFieldValues(ControlsEnum.ASSETSERVICEORDERDTL);
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
                        if (AssetServiceOrderDetailList != null && AssetServiceOrderDetailList.Count > 0)
                        {
                            TempAssetServiceOrderHeaderSession = AssetServiceOrderHeaderSession;
                            CurrSlNo = SID_SL_NO = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                assetServiceOrderDetailsObj = AssetServiceOrderDetailList.SingleOrDefault(row => CurrSlNo == row.OSD_SL_NO);
                                assetServiceOrderItemDetObj = assetServiceOrderDetailsObj.Item_details[ItemRowIndex];
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
                        if (AssetServiceOrderDetailList != null && AssetServiceOrderDetailList.Count > 0)
                        {
                            CurrSlNo = SID_SL_NO = Convert.ToInt32(((ImageButton)sender).CommandArgument.ToString());
                            if (CurrSlNo > 0)
                            {
                                assetServiceOrderDetailsObj = AssetServiceOrderDetailList.SingleOrDefault(row => CurrSlNo == row.OSD_SL_NO);
                                assetServiceOrderDetailsObj.Item_details.RemoveAt(ItemRowIndex);
                                AssetServiceOrderDetailList.ForEach(dtl =>
                                {
                                    double osdAmount = 0;
                                    if (dtl.Item_details != null && dtl.Item_details.Count > 0)
                                    {
                                        osdAmount = dtl.Item_details.Sum(r => r.OID_NET_AMOUNT);                                      
                                    }
                                    dtl.OSD_AMOUNT = osdAmount;
                                });                                
                                SetFieldValues(ControlsEnum.ASSETSERVICEORDERDTL);
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
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSH_PK")).Value);
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
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSH_PK")).Value);
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
                        result = ServiceOrderBL.DeleteServiceOrder(CurrPK, Convert.ToString(this.LastModifiedTime));
                        if (result > 0)
                        {
                            if (grdList.Rows.Count == 1 && Convert.ToInt32(PageIndex) > 1)
                            {
                                PageIndex = Convert.ToInt32(PageIndex) - 1;
                            }
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                            litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceOrder);
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
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder;
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
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " +
                                    GetGlobalResourceObject("Messages", "EditUsedByAnotherUser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.CODEEXIST)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " +
                                    GetGlobalResourceObject("Messages", "Itemsalreadyaddedbyanotheruser").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else if (result == (int)DbSaveStatus.ALREADYDELETED)
                            {
                                litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " +
                                    GetGlobalResourceObject("Messages", "AlreadyDeleted").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetGlobalResourceObject("Messages", "ActionFailedPleaseTryAgain").ToString();
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceOrder);
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
                            EditTempAssetServiceOrderHeaderSession = TempAssetServiceOrderHeaderSession;
                            soDetailsList = EditTempAssetServiceOrderHeaderSession.Details;
                            soDetailsObj = EditTempAssetServiceOrderHeaderSession.Details == null ? null :
                                    EditTempAssetServiceOrderHeaderSession.Details.SingleOrDefault(ctr => ctr.OSD_SL_NO == CurrSlNo);
                            if (ItemRowIndex >= 0)
                                soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                            if (soDetailsItemObj == null)
                            {
                                soDetailsList = (List<AssetServiceOrderDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDER_ITEMDTL);
                                EditTempAssetServiceOrderHeaderSession.Details = soDetailsList;
                            }

                            if (soDetailsList != null && soDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                                hdfTaxFormula.Value = string.Empty;
                                txtTaxPopupItemAmount.Text = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) :
                                    string.IsNullOrEmpty(txtPopupDiscount.Text.Trim()) ? Convert.ToDouble(txtPopupAmount.Text).ToString(hdfCurrencyFormat.Value) :
                                    (Convert.ToDouble(txtPopupAmount.Text.Trim()) - Convert.ToDouble(txtPopupDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);

                                if (EditTempAssetServiceOrderHeaderSession != null)
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
                            EditTempAssetServiceOrderHeaderSession = TempAssetServiceOrderHeaderSession;
                            soDetailsList = EditTempAssetServiceOrderHeaderSession.Details;
                            soDetailsObj = EditTempAssetServiceOrderHeaderSession.Details == null ? null :
                                    EditTempAssetServiceOrderHeaderSession.Details.SingleOrDefault(ctr => ctr.OSD_SL_NO == CurrSlNo);
                            if (ItemRowIndex >= 0)
                                soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                            if (soDetailsItemObj == null)
                            {
                                soDetailsList = (List<AssetServiceOrderDetails>)SetUIValuesToObject(ControlsEnum.ASSETSERVICEORDER_ITEMDTL);
                                EditTempAssetServiceOrderHeaderSession.Details = soDetailsList;
                            }
                            if (soDetailsList != null && soDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtTaxPopupItemAmount.Text = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtPopupAmount.Text).ToString(hdfCurrencyFormat.Value);
                                if (EditTempAssetServiceOrderHeaderSession != null)
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
                        if (AssetServiceOrderHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                            TempAssetServiceOrderHeaderSession = AssetServiceOrderHeaderSession;
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
                                    taxable = Convert.ToDouble(AssetServiceOrderHeaderSession.OSH_SUB_TOTAL) - AssetServiceOrderHeaderSession.OSH_DISCOUNT + AssetServiceOrderHeaderSession.OSH_OTH_CHARGE;
                                }
                                else
                                {
                                    taxable = Convert.ToDouble(AssetServiceOrderHeaderSession.OSH_SUB_TOTAL) - AssetServiceOrderHeaderSession.OSH_DISCOUNT;
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
                        if (AssetServiceOrderHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                            TempAssetServiceOrderHeaderSession = AssetServiceOrderHeaderSession;
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
                        if (AssetServiceOrderHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.CLEARPOPUPDETAILS);
                            TempAssetServiceOrderHeaderSession = AssetServiceOrderHeaderSession;
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
                            AssetServiceOrderHeaderSession = TempAssetServiceOrderHeaderSession;
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
                            TempAssetServiceOrderHeaderSession = EditTempAssetServiceOrderHeaderSession;
                            //AssetServiceOrderHeaderSession = TempAssetServiceOrderHeaderSession;
                            // SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            soDetailsObj = TempAssetServiceOrderHeaderSession.Details.SingleOrDefault(crt => crt.OSD_SL_NO == CurrSlNo);
                            SetDetailTax(TempAssetServiceOrderHeaderSession);
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
                        if (IsHeaderTax ? TempAssetServiceOrderHeaderSession != null : EditTempAssetServiceOrderHeaderSession != null)
                        {
                            objAssetServiceOrderHeader = IsHeaderTax ? TempAssetServiceOrderHeaderSession : EditTempAssetServiceOrderHeaderSession;
                            tempInvTaxSplitObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                {
                                    tempInvTaxSplitObj = objAssetServiceOrderHeader.Tax_HDR == null ? null :
                                        objAssetServiceOrderHeader.Tax_HDR.SingleOrDefault(ctr => ctr.SSD_TAX == Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) && ctr.SSD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempInvTaxSplitObj = objAssetServiceOrderHeader.Tax_HDR == null ? null :
                                        objAssetServiceOrderHeader.Tax_HDR.SingleOrDefault(ctr => ctr.SSD_NAME == txtTaxPopupOther.Text.Trim() && ctr.SSD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                soDetailsObj = objAssetServiceOrderHeader.Details == null ? null :
                                    objAssetServiceOrderHeader.Details.SingleOrDefault(ctr => ctr.OSD_SL_NO == CurrSlNo);
                                if (ItemRowIndex >= 0)
                                {
                                    soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                                }                               
                                if (soDetailsItemObj != null)
                                {
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempInvTaxSplitObj = soDetailsItemObj.Tax_DTL == null ? null :
                                            soDetailsItemObj.Tax_DTL.SingleOrDefault(rfq => rfq.SSD_TAX == Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) && rfq.SSD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempInvTaxSplitObj = soDetailsItemObj.Tax_DTL == null ? null :
                                            soDetailsItemObj.Tax_DTL.SingleOrDefault(rfq => rfq.SSD_NAME == txtTaxPopupOther.Text.Trim() && rfq.SSD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempInvTaxSplitObj == null)
                            {
                                taxHdrList = new List<AssetServiceOrderTaxHdr>();

                                soInvTaxHdrObj = new AssetServiceOrderTaxHdr();
                                try
                                {
                                    soInvTaxHdrObj.SSD_TAX_AMT = string.IsNullOrEmpty(txtTaxPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtTaxPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    soInvTaxHdrObj.SSD_SL_NO = CurrSlNo;
                                    if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                    {
                                        soInvTaxHdrObj.SSD_TAX = Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue);
                                    }
                                    soInvTaxHdrObj.SSD_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    soInvTaxHdrObj.SSD_NAME = HttpUtility.HtmlEncode(txtTaxPopupOther.Text.Trim());
                                    soInvTaxHdrObj.SSD_PK = 0;
                                    soInvTaxHdrObj.SSD_OID_PK = CurrPK;
                                    soInvTaxHdrObj.SSD_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    soInvTaxHdrObj.SSD_TYPE = 1;
                                    soInvTaxHdrObj.SSD_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (soInvTaxHdrObj.SSD_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(objAssetServiceOrderHeader.OSH_SUB_TOTAL);
                                            currentTotal = objAssetServiceOrderHeader.Tax_HDR == null ? 0 :
                                                objAssetServiceOrderHeader.Tax_HDR.Where(htx => htx.SSD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.SSD_TAX_AMT);
                                            if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(soInvTaxHdrObj.SSD_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = soInvTaxHdrObj.SSD_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                if (objAssetServiceOrderHeader.Tax_HDR == null)
                                                    taxHdrList = new List<AssetServiceOrderTaxHdr>();
                                                else
                                                    taxHdrList = objAssetServiceOrderHeader.Tax_HDR.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                objAssetServiceOrderHeader.Tax_HDR = taxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            if (objAssetServiceOrderHeader.Tax_HDR == null)
                                                taxHdrList = new List<AssetServiceOrderTaxHdr>();
                                            else
                                                taxHdrList = objAssetServiceOrderHeader.Tax_HDR.ToList();
                                            taxHdrList.Add(soInvTaxHdrObj);
                                            objAssetServiceOrderHeader.Tax_HDR = taxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        soDetailsObj = objAssetServiceOrderHeader.Details == null ? null :
                                            objAssetServiceOrderHeader.Details.SingleOrDefault(item => item.OSD_SL_NO == CurrSlNo);
                                        if (ItemRowIndex >= 0)
                                        {
                                            soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                                        }                                        
                                        if (soDetailsItemObj != null)
                                        {
                                            if (soInvTaxHdrObj.SSD_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = soDetailsObj.OSD_AMOUNT;
                                                currentTotal = soDetailsItemObj.Tax_DTL == null ? 0 :
                                                    soDetailsItemObj.Tax_DTL.Where(dtx => dtx.SSD_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.SSD_TAX_AMT);
                                                if (Convert.ToInt32(ddlTaxPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(soInvTaxHdrObj.SSD_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = soInvTaxHdrObj.SSD_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    taxHdrList = soDetailsItemObj.Tax_DTL == null ? new List<AssetServiceOrderTaxHdr>() : soDetailsItemObj.Tax_DTL.ToList();
                                                    taxHdrList.Add(soInvTaxHdrObj);
                                                    if (ItemRowIndex >= 0)
                                                    {
                                                        objAssetServiceOrderHeader.Details.SingleOrDefault(rfq => rfq.OSD_SL_NO == CurrSlNo).Item_details[ItemRowIndex].Tax_DTL = taxHdrList;
                                                    }
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                taxHdrList = soDetailsItemObj.Tax_DTL == null ? new List<AssetServiceOrderTaxHdr>() : soDetailsItemObj.Tax_DTL.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                objAssetServiceOrderHeader.Details.SingleOrDefault(rfq => rfq.OSD_SL_NO == CurrSlNo).Item_details[ItemRowIndex].Tax_DTL = taxHdrList;
                                            }
                                        }
                                    }
                                    if (IsHeaderTax)
                                        TempAssetServiceOrderHeaderSession = objAssetServiceOrderHeader;
                                    else
                                        EditTempAssetServiceOrderHeaderSession = objAssetServiceOrderHeader;
                                    //EditTempAssetServiceOrderHeaderSession = objAssetServiceOrderHeader;
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
                        if (IsHeaderTax ? TempAssetServiceOrderHeaderSession != null : EditTempAssetServiceOrderHeaderSession != null)
                        {
                            objAssetServiceOrderHeader = IsHeaderTax ? TempAssetServiceOrderHeaderSession : EditTempAssetServiceOrderHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                taxHdrList = new List<AssetServiceOrderTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempInvTaxSplitObj = objAssetServiceOrderHeader.Tax_HDR == null ? null :
                                            objAssetServiceOrderHeader.Tax_HDR.SingleOrDefault(rfq => rfq.SSD_TAX == taxPK && rfq.SSD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempInvTaxSplitObj = objAssetServiceOrderHeader.Tax_HDR == null ? null :
                                                objAssetServiceOrderHeader.Tax_HDR.SingleOrDefault(rfq => rfq.SSD_NAME == hdfTaxName.Value && rfq.SSD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempInvTaxSplitObj != null)
                                    {
                                        taxHdrList = objAssetServiceOrderHeader.Tax_HDR.ToList();
                                        taxHdrList.Remove(tempInvTaxSplitObj);
                                        objAssetServiceOrderHeader.Tax_HDR = taxHdrList;
                                    }
                                }
                                else
                                {
                                    soDetailsObj = objAssetServiceOrderHeader.Details == null ? null :
                                        objAssetServiceOrderHeader.Details.SingleOrDefault(rfq => rfq.OSD_SL_NO == CurrSlNo);
                                    if (ItemRowIndex >= 0)
                                    {
                                        soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                                    }                                   
                                    if (soDetailsItemObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempInvTaxSplitObj = soDetailsItemObj.Tax_DTL == null ? null :
                                                soDetailsItemObj.Tax_DTL.SingleOrDefault(rfq => rfq.SSD_TAX == taxPK && rfq.SSD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempInvTaxSplitObj = soDetailsItemObj.Tax_DTL == null ? null :
                                                    soDetailsItemObj.Tax_DTL.SingleOrDefault(rfq => rfq.SSD_NAME == hdfTaxName.Value && rfq.SSD_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        soDetailsObj = objAssetServiceOrderHeader.Details.SingleOrDefault(rfq => rfq.OSD_SL_NO == CurrSlNo);
                                        if (ItemRowIndex >= 0)
                                        {
                                            soDetailsItemObj = soDetailsObj.Item_details[ItemRowIndex];
                                        }                                       
                                        if (soDetailsItemObj != null && soDetailsItemObj.Tax_DTL != null)
                                        {
                                            taxHdrList = soDetailsItemObj.Tax_DTL.ToList();
                                            taxHdrList.Remove(tempInvTaxSplitObj);
                                            objAssetServiceOrderHeader.Details.SingleOrDefault(rfq => rfq.OSD_SL_NO == CurrSlNo).Item_details[ItemRowIndex].Tax_DTL = taxHdrList;
                                        }
                                    }
                                }
                                if (IsHeaderTax)
                                    TempAssetServiceOrderHeaderSession = objAssetServiceOrderHeader;
                                else
                                    EditTempAssetServiceOrderHeaderSession = objAssetServiceOrderHeader;
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
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfOSH_PK")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK + "&APPTYPE=" + "SOA" + "&APPSUBTYPE=0") + "');", true);
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
        private void SaveTransaction(AssetServiceOrderHeader objServiceRequestHeader, int workflowFlag)
        {
            int? result = 0;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objServiceRequestHeader == null)
                objServiceRequestHeader = new AssetServiceOrderHeader();
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
            string xmlDoc = CommonFunctions.XmlSerialize<AssetServiceOrderHeader>(objServiceRequestHeader);
            result = ServiceOrderBL.SaveServiceOrderDetails(xmlDoc, out TrxNo);
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
                args[0] = Resources.PageNameRes.AssetServiceOrder;
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
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                }
                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " + Resources.Messages.AlreadyDeleted;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Captions.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.AssetServiceOrder + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.AssetServiceOrder);
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
                        if (AssetServiceOrderDetailList != null)
                        {
                            HiddenField hdfSRDSlNo = e.Row.FindControl("hdfSRDSlNo") as HiddenField;
                            Label lblTotal = e.Row.FindControl("lblTotal") as Label;
                            int OSD_SL_NO = Convert.ToInt32(hdfSRDSlNo.Value);
                            assetServiceOrderItemDetList = new List<AssetServiceOrderItemDetails>();
                            assetServiceOrderItemDetList = AssetServiceOrderDetailList.SingleOrDefault(c => c.OSD_SL_NO == OSD_SL_NO).Item_details;
                            if (assetServiceOrderItemDetList != null)
                            {
                                GridView grdItems = e.Row.FindControl("grdItems") as GridView;
                                grdItems.DataSource = assetServiceOrderItemDetList;
                                grdItems.DataBind();
                                if (lblTotal != null)
                                {
                                    lblTotal.Text = lblTotal.ToolTip = GetFormattedCurrencyWithComma(assetServiceOrderItemDetList.Sum(itm => itm.OID_NET_AMOUNT));
                                }
                            }
                        }

                    }
                    else if (senderGridView.ID == "grdItems")
                    {
                        HiddenField hdfOID_SID_DTL = e.Row.FindControl("hdfOID_SID_DTL") as HiddenField;
                        foreach (TableCell cell in e.Row.Cells)
                        {
                            if (Convert.ToInt32(hdfOID_SID_DTL.Value) > 0)
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
            ASSETSERVICEORDERHDR,
            ASSETSERVICEORDERDTL,
            ASSETSERVICEORDER_ITEMDTL,
            ASSETSERVICETYPE,
            FILLASSETDETAILS,          
            ITEMDETAILS,
            FILLITEMDETAILS,
            CLEARSEARCH,
            PENDINGREQUESTS,
            SELECTEDSRDETAILS,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXHEADER,
            CUSTOMTAXSETTINGS,
            SRFROMINBOX
            
        }
        #endregion
    }
}