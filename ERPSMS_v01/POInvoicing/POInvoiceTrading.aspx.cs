using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using BusinessObject.Common;
using ERPSMS_v01.UserControls;
using System.Linq;
using BusinessObject.CommonManagement;
using System.Data;
using System.Threading;
using System.Web.UI.HtmlControls;
using BusinessObject.AlertManagement;
using BusinessObject.PurchaseOrderManagement;
using System.IO;
using BusinessObject.POInvoicing;
using BusinessLogic.CommonManagement;
using BusinessObject;
using DataAccess.POInvoicing;

#region DB Summary
//TABLES:
//FIN_INVOICE_VND_HDR
//FIN_INVOICE_VND_DTL
//FIN_INVOICE_VND_TAX_HDR
//FIN_INVOICE_VND_TAX_DTL        
//FIN_INVOICE_VND_ADV_DED_DTL
//FIN_INVOICE_VND_TRX_MPG
//SP:
//SPFIN_INVOICE_VND_MULTIPLE_PO_TRADING_GET (Add to list Button Click & GET SP)
//SPFIN_INVOICE_VND_TRADING_SAVE
//SPFIN_INVOICE_VND_TRADING_WKF_SAVE
//SPFIN_INVOICE_VND_TRADING_DELETE
//SPFIN_INVOICE_VND_TRADING_GET_LIST
//SPFIN_PUR_DIR_INVOICE_NO_AUTO

#endregion
namespace ERPSMS_v01.POInvoicing
{
    public partial class POInvoiceTrading : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
       
        #region Properties
        /// <summary>
        /// To maintain keep PO Invoice Header 
        /// </summary>
        private DirectPOInvoiceHeader POInvoiceHeaderSession
        {
            get
            {
                return (DirectPOInvoiceHeader)Session["DirectPOInvoiceHeaderSession"];
            }
            set
            {
                Session["DirectPOInvoiceHeaderSession"] = value;
            }
        }
        /// <summary>
        /// To maintain keep Temp PO Invoice Header Session
        /// </summary>
        private DirectPOInvoiceHeader TempPOInvoiceHeaderSession
        {
            get
            {
                return (DirectPOInvoiceHeader)this.ViewState["DirectTempPOInvoiceHeaderSession"];
            }
            set
            {
                this.ViewState["DirectTempPOInvoiceHeaderSession"] = value;
            }
        }

        /// <summary>
        /// PO Type from PO Grid
        /// </summary>
        private POInvoiceGroup POGroup
        {
            get
            {
                return (this.ViewState[ViewstateStrings.POGroup] == null ? (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup),
                    CommonConstants.SELECT_VALUE_ONE) : (POInvoiceGroup)this.ViewState[ViewstateStrings.POGroup]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POGroup] = value;
            }
        }

        /// <summary>
        /// Current vendor PK
        /// </summary>
        private int vPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.vndpk]);
            }
            set
            {
                this.ViewState[ViewstateStrings.vndpk] = value;
            }
        }

        /// <summary>
        /// Is continue
        /// </summary>
        private bool Iscont
        {
            get
            {
                return this.ViewState[ViewstateStrings.Iscont] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.Iscont]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Iscont] = value;
            }
        }


        /// <summary>
        /// Process ID of the Page
        /// </summary>
        private byte WkfStatus
        {
            get
            {
                return this.ViewState["WkfStatus"] == null ? Convert.ToByte(0) : Convert.ToByte(this.ViewState["WkfStatus"]);
            }
            set
            {
                this.ViewState["WkfStatus"] = value;
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
        /// Current PK
        /// </summary>
        private int CurrMpgPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CurrMpgPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrMpgPK] = value;
            }
        }
        /// <summary>
        /// Currency
        /// </summary>
        private int Currency
        {
            get
            {
                return this.ViewState[ViewstateStrings.Currency] == null ? 0 : (int)this.ViewState[ViewstateStrings.Currency];
            }
            set
            {
                this.ViewState[ViewstateStrings.Currency] = value;
            }
        }

        /// <summary>
        /// Vendor Pk
        /// </summary>
        private int VendorID
        {
            get
            {
                return this.ViewState[ViewstateStrings.VendorID] == null ? 0 : (int)this.ViewState[ViewstateStrings.VendorID];
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorID] = value;
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

        /// <summary>
        /// Status
        /// </summary>
        private int ItemStatus
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.ItemStatus]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ItemStatus] = value;
            }
        }

        /// <summary>
        /// Approved
        /// </summary>
        private bool Posted
        {
            get
            {
                return Convert.ToBoolean(this.ViewState[ViewstateStrings.Posted]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Posted] = value;
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
        /// Invoice
        /// </summary>
        private string Invoice
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.Invoice];
            }
            set
            {
                this.ViewState[ViewstateStrings.Invoice] = value;
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
                return this.ViewState[ViewstateStrings.EntryState] == null ? EntryStatus.ENTRYMODE : (EntryStatus)(this.ViewState[ViewstateStrings.EntryState]);
            }
            set
            {
                this.ViewState[ViewstateStrings.EntryState] = value;
            }
        }

        /// <summary>
        /// VendorPK
        /// </summary>
        private int VendorPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.VendorPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorPK] = value;
            }
        }   

        private int CurrSlNo
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] == null ? 0 : (int)ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo];
            }
            set
            {
                ViewState[ERP.Utilities.ViewstateStrings.CurrSlNo] = value;
            }
        }

        private List<BusinessObject.POInvoicing.DirectPOInvoiceUploads> POUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.POUploadList] == null ? null : (List<BusinessObject.POInvoicing.DirectPOInvoiceUploads>)ViewState[ViewstateStrings.POUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.POUploadList] = value;
            }
        }
        private List<BusinessObject.POInvoicing.FileDetails> FileDetailsList
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.FilePODetailsList] == null ? null : (List<BusinessObject.POInvoicing.FileDetails>)Session[ERP.Utilities.SessionStrings.FilePODetailsList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.FilePODetailsList] = value;
            }
        }

        /// <summary>
        /// Selected TaxDetails PoPK
        /// </summary>
        private int SelectedTaxPOPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState["SelectedTaxPOPK"]);
            }
            set
            {
                this.ViewState["SelectedTaxPOPK"] = value;
            }
        }
        /// <summary>
        /// To maintain the PageIndex in viewstate
        /// </summary>
        private string PageIndexPO
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
        private DataTable dtPendingPOList
        {
            get
            {
                return this.ViewState[ViewstateStrings.OtherDetailList] == null ? new DataTable() : (DataTable)this.ViewState[ViewstateStrings.OtherDetailList];
            }
            set
            {
                this.ViewState[ViewstateStrings.OtherDetailList] = value;
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
        /// Tax calculation is enabled or disabled for advance invoice .
        /// </summary>
        private bool IsAdvInvHasTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsAdvInvHasTax] == null ? false : (bool)this.ViewState[ViewstateStrings.IsAdvInvHasTax];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsAdvInvHasTax] = value;
            }
        }

        #endregion

        private DirectPOInvoiceHeader soInvoiceDetailsObj;
        List<DirectPOInvoiceHeader> soInvoiceDetailsList;
        DirectPOInvoiceUploads poUploadObj; 
        private DirectPOInvoiceHeader invoiceHeaderObj;     
        private DirectPOHeaderBO PoHeaderObj;

        // Indicates the state as well as action
        private ActionsEnum commonActions;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;       

        DataTable dtAdsType;
        DataTable dtAdsTypeDtl;

        DataSet dsAdsType;
        DataSet dsAdsTypeDtl;

        private int vendPK = 0;
        private int purchaseOrderPK = 0;

        private PUR_ORDER_HDR PurOrderHdrObj;
        private List<PUR_ORDER_HDR> PurOrderHdrList;

        private List<PUR_ORDER_HDR> PurOrderHdrListTaxSplitup;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;

        private List<ADM_CONFIG_MST> admConfigMstList;
        private ADM_CURRENCY_MST admCurrencyMstObj;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        DataSet dsAlertList;
        private int invPK;
        private int VncPk = 0;
        private string TypeRef;
        private string appType;
        DataSet dsPageData;      
        private DataTable dtPageData;
        private DataTable dtInvoiceList;
        //List for binding details to controls  
        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrList;
        private List<FIN_INVOICE_VND_TRX_MPG> finInvoiceVndTrxMpgList;
      
        private List<ADM_COMPANY_MST> admCompanyMstList;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private int vndPK;
        private string ivoiceNo;
        private bool updateInvoice;
        int JournalPK;
        private string refID;
        private string inboxFlag;

        private long InvoicePk = 0;
        private DataTable dtAmountDetails;

        private ADM_COMPANY_MST admCompanyMstObj;
        private DataTable dtCompany;

        private BusinessObject.User currentUser;
        private bool postflag = false;
        DataTable dtTaxSettings;

        private List<FIN_YEAR_MST> finYearMstList;
        private Dictionary<string, decimal> dicTempAmount = new Dictionary<string, decimal>();

        int numberGenerationSubType;

        ADM_DOC_ATTACH admDocAttachObj;

        private PUR_ORDER_TAX_HDR PurOrderTaxDetailsObj;
        private DataTable dtPurOrderTaxDetails;
        private CommonService cm;
        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        bool isCancelled = false;        
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
            string prefID;
            try
            {
                #region Number.Currency Format
                hdfDecimalFormat.Value = "#0.";
                int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                for (int i = 0; i < NoDecimalDigitsP2P; i++)
                {
                    hdfDecimalFormat.Value += "0";
                }
                string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                hdfCurrencyFormatWithSeperator.Value = "#" + currencysep + "#0.";
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                    hdfCurrencyFormatWithSeperator.Value += "0";
                }

                hdfRateFormat.Value = "#0.";
                int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P] == null
                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigitP2P]));
                for (int i = 0; i < rateDecimalDigits; i++)
                {
                    hdfRateFormat.Value += "0";
                }

                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                #endregion
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
                if (!IsPostBack)
                {
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANYSRCH);
                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarray;
                    hdfJournalizeWorkFlow.Value = "0";
                    Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;                  
                    FileDetailsList = null;
                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    
                    uclPaging.CurrentPage = 1;
                    uclPOPaging.CurrentPage = 1;

                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;

                    //Used for Integration purpose
                    FillProcessID(1);
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

                    //If Has RefID (from Inbox)
                    if (!string.IsNullOrEmpty(refID))
                    {
                        #region  Has RefID
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
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            if (pid.Equals("11"))
                                hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }
                        #endregion
                    }
                    else if (!string.IsNullOrEmpty(prefID))
                    {
                        base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                        CurrPK = GetApplicationID(ucrWrkf.RefID);
                    }
                    GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                    if (!string.IsNullOrEmpty(hdfPostingSettings.Value) && hdfPostingSettings.Value.Equals("0"))
                    {
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("0"));
                        ddlStatus.Items.Remove(ddlStatus.Items.FindByValue("1"));
                    }

                    if (CurrPK > 0)
                    {
                        #region Have CurrPK
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;                     
                        GetFieldValues(ControlsEnum.POINVHEADER);
                        SetFieldValues(ControlsEnum.POINVHEADER);
                        SetFieldValues(ControlsEnum.POINVOICELIST);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        GetFieldValues(ControlsEnum.PENDINGPOLIST);
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        #endregion
                    }
                    else
                    {
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                    }
                }
                #region MultiplePlant 
                if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                {
                    lblCompany.Visible = true;
                    ddlCompanySrch.Visible = true;
                }
                else
                {
                    lblCompany.Visible = false;
                    ddlCompanySrch.Visible = false;
                } 
                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {
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
            int TotalRecords = 0;
            POListService poListServiceClient;
            poListServiceClient = null;

            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            string xmlDocPO = string.Empty;
            try
            {
                switch (type)
                {
                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                         int cusID = String.IsNullOrEmpty(hdfVendorID.Value.Trim()) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorID.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtVendor.Text;
                        }
                        int InvPk = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        string customer = string.IsNullOrEmpty(txtVendor.Text.Trim()) ? string.Empty : (txtVendor.Text.Trim() == "Select/Type" ? string.Empty : txtVendor.Text.Trim());
                        string poNumber=string.IsNullOrEmpty(txtpoNo.Text.Trim())? string.Empty :txtpoNo.Text.Trim();
                        dsPageData = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceTradingList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? "IVH_DATE" : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = SortBy == ThenBy || SortBy == "IVH_NO" ? string.Empty : string.IsNullOrEmpty(ThenBy) ? "IVH_NO" : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == "IVH_NO" ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                SearchBy = "IVH_NO",
                                SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == "Select/Type" ? string.Empty : txtInvoiceNumber.Text.Trim()),
                                PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage,
                                PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"))
                            }, currentUser, cusID, InvPk, 0, customer, poNumber, Resources.PageURL.PoInvoicingTrading.Replace("~", ""), 0
                            , Convert.ToInt32(ddlStatus.SelectedValue), 0, (byte)POInvoiceCategory.Advanced);
                       
                        if (dsPageData != null)
                        {   
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            dtInvoiceList = dvInvoice.ToTable();                        
                        }
                        break;
                    #endregion
                    #region PENDINGPOLIST
                    case ControlsEnum.PENDINGPOLIST:
                        TotalPages = 0;
                        vendPK = 0;
                        TotalRecords = 0;
                        purchaseOrderPK = 0;
                        int.TryParse(hdfVendorHd.Value, out vendPK);
                        int.TryParse(hdfPoPK.Value, out purchaseOrderPK);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = string.IsNullOrEmpty(PageIndexPO) ? 1 : Convert.ToInt32(PageIndexPO);
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_POList"));
                        DateTime? fromDate = string.IsNullOrEmpty(txtPendingFromDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingFromDate.Text.Trim());
                        DateTime? todate = string.IsNullOrEmpty(txtPendingToDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingToDate.Text.Trim());                     
                        dtPendingPOList = BusinessLogic.POInvoicing.POInvoiceBL.GetPendingPOList(vendPK, purchaseOrderPK, CurrPK, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize,fromDate,todate);                       
                        break;
                    #endregion
                    #region POINVHEADER (Getting Details of selected MultiplePOPKs)
                    case ControlsEnum.POINVHEADER:                      
                        xmlDocPO = string.Empty;
                        if (PoHeaderObj != null && PoHeaderObj.POList != null && PoHeaderObj.POList.Count > 0)
                        {
                            xmlDocPO = CommonFunctions.XmlSerialize<DirectPOHeaderBO>(PoHeaderObj);
                        }
                        invoiceHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetDirectPurchaseInvoiceHeaderMUL(xmlDocPO, !string.IsNullOrEmpty(xmlDocPO) ? 0 : CurrPK,1);

                        if (POInvoiceHeaderSession == null)
                            POInvoiceHeaderSession = invoiceHeaderObj.DeepClone();
                        else if (invoiceHeaderObj != null && POInvoiceHeaderSession != null)
                        {
                            List<int> objPoList = new List<int>();
                            if (POInvoiceHeaderSession.POMappingDetails != null)
                                objPoList = POInvoiceHeaderSession.POMappingDetails.Select(r => r.IVM_PO_HDR).Distinct().ToList();
                            List<DirectPOInvoiceMappingDetails> objMpgList = invoiceHeaderObj.POMappingDetails.Where(r => !objPoList.Contains(r.IVM_PO_HDR)).ToList();
                            if (objMpgList != null && objMpgList.Count > 0)
                            {
                                objMpgList.ForEach(dtl =>
                                {
                                    POInvoiceHeaderSession.POMappingDetails.Add(dtl);
                                });
                            }
                        }
                        break;
                    #endregion  
                    #region Generate Exchange Rate
                    case ControlsEnum.EXCHANGERATE:                        
                        poInvoiceServiceClient = new POInvoiceService();
                        poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        double ExchgRate = poInvoiceServiceClient.GetConversionFactor(
                                                             invoiceHeaderObj.IVH_CURRENCY, invoiceHeaderObj.IVH_BASE_CURR,
                                                              Convert.ToDateTime(invoiceHeaderObj.IVH_DATE), currentUser.SBUID);
                        hdfExchangeRate.Value = ExchgRate.ToString();
                        break;
                    #endregion                 
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
                        break;
                    #endregion                  
                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.TPI, (byte)AppSubTypePOInvoice.ADVINVOICE, Convert.ToByte(CommonConstants.ACTIVE));
                        break;
                    #endregion
                    #region Tax Calculation Settings
                    case ControlsEnum.TAXCALCULATIONSETTINGS:
                        dtTaxSettings = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", string.Empty, currentUser.SBUID);
                        if (dtTaxSettings != null && dtTaxSettings.Rows.Count > 0)
                        {
                            DataRow drTaxSettings = dtTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "TAX");
                            if (drTaxSettings != null)
                            {
                                hdfTaxSettings.Value = drTaxSettings["ACF_VALUE"].ToString();
                            }
                            drTaxSettings = dtTaxSettings.AsEnumerable().SingleOrDefault(aa => aa.Field<string>("ACF_DATA").Trim() == "POSTING REQUIRED");
                            if (drTaxSettings != null)
                            {
                                hdfPostingSettings.Value = drTaxSettings["ACF_VALUE"].ToString();
                            }
                        }
                        break;
                    #endregion

                    #region NOTIFICATIONTYPES
                    case ControlsEnum.NOTIFICATIONTYPES:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = Resources.Constants.ALERT_NOTIFICATION_TYPES;
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region ALERTBASIS
                    case ControlsEnum.ALERTBASIS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = Resources.Constants.ALERT_BASIS;
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
                        break;
                    #endregion
                    #region ALERTTYPES
                    case ControlsEnum.ALERTTYPES:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConstMstList = CommonServiceClient.GetConstMstValues(null, null, null, 16, 1, currentUser.SBUID);
                        break;
                    #endregion
                    #region NOTIFICATIONDAYS
                    case ControlsEnum.NOTIFICATIONDAYS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                        admAppConfigMstObj.ACF_PK = 0;
                        admAppConfigMstObj.ACF_SETTING = Resources.Constants.ALERT_NOTIFY_BEFORE;
                        admAppConfigMstObj.ACF_DATA = ApplicationType.TPI;
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                        break;
                    #endregion
                    #region ALERTCONFIG
                    case ControlsEnum.ALERTCONFIG:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admAppConfigMstObj = new ADM_APP_CONFIG_MST();
                        admAppConfigMstObj.ACF_PK = 0;
                        admAppConfigMstObj.ACF_SETTING = Resources.Constants.AUTO_ALERT_FROM_TRX;
                        admAppConfigMstObj.ACF_DATA = "1";
                        admAppConfigMstObj.ACF_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admAppConstMstList = CommonServiceClient.GetAlertNotify(admAppConfigMstObj);
                        break;
                    #endregion
                    #region ALERTDETAILS
                    case ControlsEnum.ALERTLIST:
                        dsAlertList = BusinessLogic.AlertManagement.Alerts.GetAlertDetails(0, Convert.ToByte(DbActiveStatus.ACTIVE), null, appType, invPK, currentUser.SBUID, currentUser.PKUser, (int)AlertType.System);
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        break;
                    #endregion
                    #region VENDORBRANCH
                    case ControlsEnum.VENDORBRANCH:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetVendorContactList(0, Convert.ToByte(DbActiveStatus.ACTIVE), vndPK, currentUser.SBUID);                      
                        break;
                    #endregion                 
                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:
                        if (vPK > 0)
                            dsAdsType = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.ACTIVE), 0, vPK, 0);
                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        if (ddlAddressType.SelectedValue != CommonConstants.SELECTVAL)
                            int.TryParse(ddlAddressType.SelectedValue, out VncPk);

                        if (vPK > 0)
                            dsAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, vPK, 0);

                        break;
                    #endregion

                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        dtAmountDetails = new DataTable();                 
                        dtAmountDetails = BusinessLogic.POInvoicing.POInvoiceBL.GetBalanceAmountDetails(InvoicePk);
                        break;
                    #endregion
                    #region PO Tax Details
                    case ControlsEnum.POTAXDETAILS:
                        poListServiceClient = new POListService();
                        poListServiceClient = CommonFunctions.InitiateClient(poListServiceClient);
                        dtPurOrderTaxDetails = new DataTable();
                        dtPurOrderTaxDetails = poListServiceClient.GetPurOrderTaxDetails(SelectedTaxPOPK);
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
                poListServiceClient = null;
                poInvoiceServiceClient = null;
                admCompanyMstServiceClient = null;
                finTrxServiceClient = null;
                CommonServiceClient = null;
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
                    #region INVOICELIST
                    case ControlsEnum.INVOICELIST:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region POINVOICELIST
                    case ControlsEnum.POINVOICELIST:
                        BindGrid(ControlsEnum.POINVOICELIST);
                        break; 
                    #endregion
                    #region PENDINGPOLIST
                    case ControlsEnum.PENDINGPOLIST:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region POINVHEADER
                    case ControlsEnum.POINVHEADER:
                        GetUIValuesFromObject(controlType);
                        break; 
                    #endregion
                    #region POINVDETAIL
                    case ControlsEnum.POINVDETAIL:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region UPDATEGRIDVALTOOBJECT
                    case ControlsEnum.UPDATEGRIDVALTOOBJECT:
                        SetUIValuesToObject(controlType);
                        break; 
                    #endregion

                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {
                            txtFromDate.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            hdfFromDate.Value = finYearMstList[0].FYR_DATE_FROM.ToString();
                            txtToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                            hdfToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString();
                        }
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:                       
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region VendorBranches
                    case ControlsEnum.VENDORBRANCH:                 
                        BindDropDown(ControlsEnum.VENDORBRANCH);
                        break;
                    #endregion                  
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        GetUIValuesFromObject(controlType);
                        break; 
                    #endregion
                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:
                        BindDropDown(controlType);
                        break; 
                    #endregion
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region POTAXDETAILS
                    case ControlsEnum.POTAXDETAILS:
                        if (dtPurOrderTaxDetails != null && dtPurOrderTaxDetails.Rows.Count > 0)
                        {
                            grdTaxSplitupDetails.DataSource = dtPurOrderTaxDetails;
                            grdTaxSplitupDetails.DataBind();
                        }
                        else
                        {
                            grdTaxSplitupDetails.DataSource = null;
                            grdTaxSplitupDetails.DataBind();
                        }
                        break;
                    #endregion
                    #region Company AdvSrch
                    case ControlsEnum.COMPANYSRCH:
                        BindDropDown(ControlsEnum.COMPANYSRCH);
                        break;
                    #endregion                    
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                      if (invoiceHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
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

        /// <ConfigurationSettings>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            hdfIsTaxForOtherCharge.Value = GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxPurchase").ToString();
            #region Advance Invoice Tax Settings
            DataTable dtTax = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", "TAX");
            if (dtTax != null && dtTax.Rows.Count > 0)
            {
                IsAdvInvHasTax = dtTax.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;
            }
            #endregion
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
        }

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private object SetUIValuesToObject(ControlsEnum controlType)
        {
            try
            {
                Object retObject;
                retObject = null;
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));   
                TextBox txtPayNow;
                TextBox txtOthercharges;
                HiddenField hdfPONumber;
                HiddenField hdfPOTax;
                HiddenField hdfPODiscount;
                HiddenField hdfAdjustPerInvAmt;
                double taxAmnt;
                double discAmnt;
                double adjAmnt;
                double otherAmnt;
                switch (controlType)
                {
                    #region Invoice Header
                    case ControlsEnum.FINANCEINVOICEHDR:                     
                        if (POInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = POInvoiceHeaderSession;
                            invoiceHeaderObj.IVH_PK = CurrPK;
                            invoiceHeaderObj.IVH_NO = lblInvoiceNo.Text.Trim();                            
                            invoiceHeaderObj.IVH_DATE = string.IsNullOrEmpty(txtInvdate.Text.Trim()) ? DateTime.Now.ToString() : txtInvdate.Text.Trim();
                            invoiceHeaderObj.IVH_VENDOR = hdfVendorHd.Value;
                            invoiceHeaderObj.IVH_VENDOR_NAME = HttpUtility.HtmlEncode(txtVendorHd.Text);

                            //For avoiding XML parsing error : illegal name character (&)
                            invoiceHeaderObj.IVH_VENDOR_TEXT = HttpUtility.HtmlEncode(invoiceHeaderObj.IVH_VENDOR_TEXT);
                            invoiceHeaderObj.IVH_VENDOR_ADDRESS = HttpUtility.HtmlEncode(invoiceHeaderObj.IVH_VENDOR_ADDRESS);
                            invoiceHeaderObj.IVH_VENDOR_COUNTRY_TEXT = HttpUtility.HtmlEncode(invoiceHeaderObj.IVH_VENDOR_COUNTRY_TEXT);
                            invoiceHeaderObj.IVH_CURRENCY_TEXT = HttpUtility.HtmlEncode(invoiceHeaderObj.IVH_CURRENCY_TEXT);

                            invoiceHeaderObj.IVH_VENDOR_INV_NO = txtSupplierInvNo.Text;  
                            invoiceHeaderObj.IVH_REFERENCE = string.Empty ;
                            invoiceHeaderObj.IVH_DATE_RECEIVED = string.IsNullOrEmpty(txtInvoiceReceivedon.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceReceivedon.Text.Trim();
                            invoiceHeaderObj.IVH_DATE_PAY_BY = string.IsNullOrEmpty(txtPaybydate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtPaybydate.Text.Trim();                                                                
                            invoiceHeaderObj.IVH_ORGINAL_RCVD = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                            invoiceHeaderObj.IVH_AMOUNT_TC = string.IsNullOrEmpty(txtInvoiceAmt.Text) ? 0 : Convert.ToDouble(txtInvoiceAmt.Text);
                            invoiceHeaderObj.IVH_DISCOUNT_TC = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDouble(txtDiscount.Text);
                            invoiceHeaderObj.IVH_TAX_TC = string.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDouble(txtTaxAmount.Text);
                            invoiceHeaderObj.IVH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDouble(txtNetAmount.Text);
                            invoiceHeaderObj.IVH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                            invoiceHeaderObj.IVH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);                           
                            int branch = 0;
                            if (int.TryParse(ddlVendorBranch.SelectedValue, out branch))
                            {
                                invoiceHeaderObj.IVH_VENDOR_CONTACT = Convert.ToString(ddlVendorBranch.SelectedValue);
                            }
                            else
                            {
                                invoiceHeaderObj.IVH_VENDOR_CONTACT = null;
                            }
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            invoiceHeaderObj.IVH_EXCHG_RATE = double.Parse(hdfExchangeRate.Value);
                            invoiceHeaderObj.IVH_AMOUNT_NET_BC = invoiceHeaderObj.IVH_AMOUNT_NET_TC * Convert.ToDouble(invoiceHeaderObj.IVH_EXCHG_RATE);                          
                            invoiceHeaderObj.IVH_SHIP_CHARGE = Convert.ToDouble(hdfOCFooter.Value);
                            invoiceHeaderObj.IVH_AMOUNT_ADJUST = 0;
                            if (ddlAddressType.Items.Count > 0)
                                invoiceHeaderObj.IVH_VENDOR_CONTACT = Convert.ToString(ddlAddressType.SelectedValue);
                            invoiceHeaderObj.IVH_BRANCH_TYPE = string.IsNullOrEmpty(hdfVendorContactType.Value) ? Convert.ToByte(0) : Convert.ToByte(hdfVendorContactType.Value);
                            invoiceHeaderObj.IVH_TAX_ID = string.IsNullOrEmpty(txtVatTaxId.Text) ? string.Empty : HttpUtility.HtmlEncode(txtVatTaxId.Text);
                            invoiceHeaderObj.IVH_BRANCH_TEXT = txtBranchCode.Text == null ? string.Empty : HttpUtility.HtmlEncode(txtBranchCode.Text);
                            invoiceHeaderObj.IVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);  
                            //Encoding TaxHeader  eg: Tax name "P & H" leades XML parsing error
                            foreach (var item in invoiceHeaderObj.TaxHdr)
                            {
                                item.VTL_TAX_TEXT = HttpUtility.HtmlEncode(item.VTL_TAX_TEXT);
                                item.VTL_NAME = HttpUtility.HtmlEncode(item.VTL_NAME);
                            }                          
                            //Do you want to continue with duplicate vendor invoice no
                            if (hdfIsContDupVenInvNo.Value == "1")
                            {
                                invoiceHeaderObj.IVH_ALLOW_DUP_INV_NO = 1; //allow to save Duplicate vendor invoice no.ie,no need for checking if vendor invoice no already exist or not
                            }
                            else
                            {
                                invoiceHeaderObj.IVH_ALLOW_DUP_INV_NO = 0; //check if vendor invoice no already exist or not
                            }

                            invoiceHeaderObj.IVH_CATEGORY = (byte)POInvoiceCategory.Advanced;
                            invoiceHeaderObj.IVH_GROUP = ((byte)POGroup) == (byte)0 ? (byte)1 : (byte)POGroup;
                            invoiceHeaderObj.IVH_STATUS = WkfStatus;
                            invoiceHeaderObj.IVH_ACTIVE = "1";
                            invoiceHeaderObj.IVH_DEPT = Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                            invoiceHeaderObj.IVH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);                           
                            invoiceHeaderObj.IVH_TYPE = hdfType.Value == "" ? "0" : Convert.ToString(hdfType.Value);
                            invoiceHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            invoiceHeaderObj.LAST_MOD_DT = LastModifiedTime == DateTime.MinValue ? System.DateTime.Now : LastModifiedTime;
                            #region Application Code,AST_VALUE,AST_DOC_MODE
                            invoiceHeaderObj.ATL_APP_TYPE = ApplicationType.TPI;
                            int numberGenerationSubType;
                            if ((byte)POGroup == (byte)POInvoiceGroup.Services)
                            {
                                numberGenerationSubType = (int)AppSubTypeCNPurchase.NONSTOCK;//12
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(hdfPOItemType.Value))
                                    numberGenerationSubType = Convert.ToInt16(hdfPOItemType.Value) == (Int16)POItemType.Others ? (int)AppSubTypeCNPurchase.NONSTOCK : (int)AppSubTypeCNPurchase.STOCK;
                                else
                                    numberGenerationSubType = (int)AppSubTypeCNPurchase.STOCK;
                            }
                            invoiceHeaderObj.APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPI : POGroup == POInvoiceGroup.Services ? ApplicationType.TPSI : ApplicationType.EI;
                            invoiceHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                            invoiceHeaderObj.AST_VALUE = numberGenerationSubType.ToString();
                            #endregion

                            invoiceHeaderObj.FileList = POUploadList;// //Uploads
                            invoiceHeaderObj.POMappingDetails = (List<DirectPOInvoiceMappingDetails>)SetUIValuesToObject(ControlsEnum.FINANCEINVOICETRXMPG);
                        }
                        retObject = invoiceHeaderObj;
                        break;
                    #endregion                   
                    #region FINANCEINVOICETRXMPG
                    case ControlsEnum.FINANCEINVOICETRXMPG:
                        List<DirectPOInvoiceMappingDetails> objMappingDetailsList = new List<DirectPOInvoiceMappingDetails>();
                        foreach (GridViewRow grdrow in grdPOList.Rows)
                        {
                            taxAmnt = 0;
                            discAmnt = 0;
                            adjAmnt = 0;
                            otherAmnt = 0;
                            DirectPOInvoiceMappingDetails objMappingDetails = new DirectPOInvoiceMappingDetails();
                            txtOthercharges = (TextBox)grdrow.FindControl("txtOthercharges");
                            hdfPOTax = (HiddenField)grdrow.FindControl("hdfPOTax");
                            hdfPODiscount = (HiddenField)grdrow.FindControl("hdfPODiscount");
                            hdfAdjustPerInvAmt = (HiddenField)grdrow.FindControl("hdfAdjustPerInvAmt");
                            hdfPONumber = (HiddenField)grdrow.FindControl(GetLocalResourceObject("hdfPONumber").ToString());
                            objMappingDetails.IVM_PK = CurrMpgPK;
                            objMappingDetails.IVM_INVOICE_HDR = CurrPK;
                            objMappingDetails.IVM_PO_HDR = hdfPONumber == null ? 0 : Convert.ToInt32(hdfPONumber.Value);
                            txtPayNow = (TextBox)grdrow.FindControl(GetLocalResourceObject("txtPayNow").ToString());
                            objMappingDetails.IVM_AMOUNT = string.IsNullOrEmpty(txtPayNow.Text.Trim()) ? 0 : Convert.ToDouble(txtPayNow.Text.Trim());
                            objMappingDetails.IVM_ACTIVE = 1;
                            if (txtOthercharges != null)
                                double.TryParse(txtOthercharges.Text, out otherAmnt);
                            if (IsAdvInvHasTax == false)
                            {
                                objMappingDetails.IVM_OTHER_AMOUNT = otherAmnt;
                            }
                            else
                            {
                                objMappingDetails.IVM_OTHER_AMOUNT = string.IsNullOrEmpty(txtOthercharges.Text) ? 0 : Convert.ToDouble(txtOthercharges.Text);
                            }                            
                            
                            if (hdfPOTax != null)
                                double.TryParse(hdfPOTax.Value, out taxAmnt);
                            objMappingDetails.IVM_TAX_AMOUNT = taxAmnt;
                            if (hdfPODiscount != null)
                                double.TryParse(hdfPODiscount.Value, out discAmnt);
                            objMappingDetails.IVM_DISCOUNT_AMOUNT = discAmnt;
                            if (hdfAdjustPerInvAmt != null)
                                double.TryParse(hdfAdjustPerInvAmt.Value, out adjAmnt);
                            objMappingDetails.IVM_ADJUST_AMOUNT = adjAmnt;
                            if (objMappingDetails.IVM_AMOUNT > 0)
                            {
                                objMappingDetailsList.Add(objMappingDetails);
                            }                            
                        }
                        retObject = objMappingDetailsList;
                        break;
                    #endregion   
                    #region UPDATE GRID VAL TO OBJECT
                    case ControlsEnum.UPDATEGRIDVALTOOBJECT:
                        if (POInvoiceHeaderSession != null && POInvoiceHeaderSession.POMappingDetails != null)
                        {
                            foreach (GridViewRow grdrow in grdPOList.Rows)
                            {
                                taxAmnt = 0;
                                discAmnt = 0;
                                adjAmnt = 0;
                                otherAmnt = 0;
                                hdfPONumber = (HiddenField)grdrow.FindControl(GetLocalResourceObject("hdfPONumber").ToString());
                                txtPayNow = (TextBox)grdrow.FindControl(GetLocalResourceObject("txtPayNow").ToString());
                                txtOthercharges = (TextBox)grdrow.FindControl("txtOthercharges");
                                DirectPOInvoiceMappingDetails objMapping = POInvoiceHeaderSession.POMappingDetails.SingleOrDefault(r => r.IVM_PO_HDR == Convert.ToInt32(hdfPONumber.Value));
                                hdfPOTax = (HiddenField)grdrow.FindControl("hdfPOTax");
                                hdfPODiscount = (HiddenField)grdrow.FindControl("hdfPODiscount");
                                hdfAdjustPerInvAmt = (HiddenField)grdrow.FindControl("hdfAdjustPerInvAmt");
                                objMapping.IVM_INVOICE_HDR = CurrPK;
                                objMapping.IVM_PO_HDR = hdfPONumber == null ? 0 : Convert.ToInt32(hdfPONumber.Value);
                                objMapping.IVM_AMOUNT = string.IsNullOrEmpty(txtPayNow.Text.Trim()) ? 0 : Convert.ToDouble(txtPayNow.Text.Trim());
                                objMapping.IVM_ACTIVE = 1;
                                if (txtOthercharges != null)
                                    double.TryParse(txtOthercharges.Text, out otherAmnt);
                                objMapping.IVM_OTHER_AMOUNT = otherAmnt;
                                if (hdfPOTax != null)
                                    double.TryParse(hdfPOTax.Value, out taxAmnt);
                                objMapping.IVM_TAX_AMOUNT = taxAmnt;
                                if (hdfPODiscount != null)
                                    double.TryParse(hdfPODiscount.Value, out discAmnt);
                                objMapping.IVM_DISCOUNT_AMOUNT = discAmnt;
                                if (hdfAdjustPerInvAmt != null)
                                    double.TryParse(hdfAdjustPerInvAmt.Value, out adjAmnt);
                                objMapping.IVM_ADJUST_AMOUNT = adjAmnt;
                            }
                        }
                        retObject = POInvoiceHeaderSession;
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

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {

                switch (controlType)
                {
                    #region POINVHEADER
                    case ControlsEnum.POINVHEADER:
                        invoiceHeaderObj = POInvoiceHeaderSession;
                        if (invoiceHeaderObj != null)
                        {
                            CurrPK = int.Parse(invoiceHeaderObj.IVH_PK.ToString());
                            btnSave.Visible = invoiceHeaderObj.IVH_DEL_STATUS == "1" ? false : true;
                            POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), invoiceHeaderObj.IVH_GROUP.ToString());
                            lblInvoiceNo.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_NO) ? Resources.Messages.DocGenerationNew : invoiceHeaderObj.IVH_NO;
                            txtVendorHd.Text = ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.IVH_VENDOR_NAME);
                            hdfVendorHd.Value = invoiceHeaderObj.IVH_VENDOR.ToString();   
                            txtSupplierInvNo.Text = invoiceHeaderObj.IVH_VENDOR_INV_NO;
                            txtInvoiceReceivedon.Text = invoiceHeaderObj.IVH_DATE_RECEIVED;
                            txtPaybydate.Text = invoiceHeaderObj.IVH_DATE;
                            lblInvoiceAmt.Text = GetLocalResourceObject("InvoiceAmt").ToString() + " (" + ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.IVH_CURRENCY_TEXT) + ")";
                            lblDiscount.Text = GetLocalResourceObject("InvDiscount").ToString() + " (" + ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.IVH_CURRENCY_TEXT) + ")";
                            lblTaxAmount.Text = GetLocalResourceObject("TaxAmount").ToString() + " (" + ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.IVH_CURRENCY_TEXT) + ")";
                            lblNetAmount.Text = GetLocalResourceObject("NetAmount").ToString() + " (" + ERP.Utilities.CommonFunctions.GetDecodedString(invoiceHeaderObj.IVH_CURRENCY_TEXT) + ")";
                            txtInvoiceAmt.Text = GetFormattedCurrency(invoiceHeaderObj.IVH_AMOUNT_TC);
                            txtDiscount.Text = GetFormattedCurrency(invoiceHeaderObj.IVH_DISCOUNT_TC);
                            txtInvdate.Text = invoiceHeaderObj.IVH_DATE;
                            txtTaxAmount.Text = GetFormattedCurrency(invoiceHeaderObj.IVH_TAX_TC);
                            txtNetAmount.Text = GetFormattedCurrency(invoiceHeaderObj.IVH_AMOUNT_NET_TC);
                            txtInvoiceAmt.Text = GetFormattedCurrency(invoiceHeaderObj.IVH_AMOUNT_NET_TC);
                            txtTaxAmount.Text = GetFormattedCurrency(invoiceHeaderObj.IVH_TAX_TC);
                            txtRemarks.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.IVH_REMARKS);                          
                            hdfInvPaidAmt.Value = invoiceHeaderObj.IVH_AMOUNT_NET_TC.ToString();
                            hdfIsJournalize.Value = invoiceHeaderObj.IVH_HAS_JRNL_ENTRY.ToString();
                            ddlCompany.SelectedValue = invoiceHeaderObj.IVH_COMPANY.ToString();
                            chkOriginalinvoice.Checked = invoiceHeaderObj.IVH_ORGINAL_RCVD == (byte)1;   
                            vndPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);
                            vPK = Convert.ToInt32(invoiceHeaderObj.IVH_VENDOR);
                            divVendorBranch.Visible = true;
                            GetFieldValues(ControlsEnum.VENDORBRANCH);
                            SetFieldValues(ControlsEnum.VENDORBRANCH);
                            hdfPOItemType.Value = invoiceHeaderObj.POH_ITEM_TYPE.ToString();
                            if (invoiceHeaderObj.IVH_VENDOR_CONTACT != null)
                            {
                                ddlVendorBranch.SelectedValue = invoiceHeaderObj.IVH_VENDOR_CONTACT.ToString();
                            }
                            hdfType.Value = invoiceHeaderObj.IVH_TYPE.ToString(); 
                            GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            SetFieldValues(ControlsEnum.VENDORCONTACTYPE); 
                            txtVatTaxId.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_TAX_ID) ? string.Empty : invoiceHeaderObj.IVH_TAX_ID.ToString();
                            txtBranchCode.Text = string.IsNullOrEmpty(invoiceHeaderObj.IVH_BRANCH_TEXT) ? string.Empty : invoiceHeaderObj.IVH_BRANCH_TEXT.ToString();
                            hdfVendorContactType.Value = invoiceHeaderObj.IVH_BRANCH_TYPE.ToString();
                            LastModifiedTime = invoiceHeaderObj.LAST_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                                                        
                        }                       
                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        if (dsAdsTypeDtl != null && dsAdsTypeDtl.Tables.Count > 0)
                        {
                            txtBranchCode.Text = dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtVatTaxId.Text = dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtVatTaxId.Text))
                                txtVatTaxId.Text = dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            hdfVendorContactType.Value = dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.vncType].ToString();
                        }
                        break; 
                    #endregion                 
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = (int)invoiceHeaderObj.IVH_PK;
                        POUploadList = invoiceHeaderObj.FileList;
                        break;
                    #endregion
                    #region SELECTED DOC
                    case ControlsEnum.SELECTEDDOC:
                        if (poUploadObj != null)
                        {
                            CurrSlNo = poUploadObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = poUploadObj.DOC_NAME;
                            anchorFile.HRef = poUploadObj.DOC_PATH;
                            if (FileDetailsList != null && FileDetailsList.Where(fle => fle.SlNo == CurrSlNo).Count() > 0)
                            {
                                anchorFile.Attributes.Add("onclick", "return false;");
                            }
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

        /// <summary>
        /// function used to bind Drop Downs Corresponding to the Drop Down passed
        /// </summary>
        private void BindDropDown(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region Company
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();
                    }
                    break;
                #endregion
                #region VENDORBRANCH
                case ControlsEnum.VENDORBRANCH:
                    ddlVendorBranch.Items.Clear();
                    if (dtPageData != null && dtPageData.Rows.Count > 0)
                    {
                        ddlVendorBranch.DataSource = dtPageData;
                        ddlVendorBranch.DataTextField = "VNC_NAME";
                        ddlVendorBranch.DataValueField = "VNC_PK";
                        ddlVendorBranch.DataBind();
                    }
                    break;
                #endregion
                #region VENDORCONTACTYPE
                case ControlsEnum.VENDORCONTACTYPE:
                    ddlAddressType.Items.Clear();
                    if (dsAdsType != null && dsAdsType.Tables.Count > 0)
                    {
                        ddlAddressType.DataSource = dsAdsType.Tables[0];
                        ddlAddressType.DataTextField = Resources.DataFieldRes.VncName;
                        ddlAddressType.DataValueField = Resources.DataFieldRes.VncPk;
                        ddlAddressType.DataBind();
                        SetBranchCodeVisibility();
                    }
                    //ddlAddressType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Company AdvSrch
                case ControlsEnum.COMPANYSRCH:
                    ddlCompanySrch.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompanySrch.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecsCode);
                        ddlCompanySrch.DataTextField = Resources.DataFieldRes.CompanySpecsCode;
                        ddlCompanySrch.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanySrch.DataBind();
                    }
                    ddlCompanySrch.Items.Insert(0, new ListItem(Resources.Report.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                default:
                    break;
            }
        }
        /// <summary>
        /// Method for Grid binding
        /// </summary>
        public void BindGrid(ControlsEnum controlType)
        {
            try
            {
                int rowCount = 0;
                int pageSize = 0;
                switch (controlType)
                {
                    #region INVOICELIST (Listing grid binding)
                    case ControlsEnum.INVOICELIST:
                    rowCount = 0;
                    pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_InvList"));
                    if (dsPageData.Tables[0].Rows.Count > 0)
                    {
                        rowCount = Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString());
                    }
                    uclPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                  (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                  (rowCount / pageSize) + 1;
                    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                    if (dtInvoiceList != null)
                    {                        
                        grdPOInvoiceList.PageIndex = Convert.ToInt32(PageIndex);
                        grdPOInvoiceList.DataSource = dtInvoiceList.DefaultView;
                        grdPOInvoiceList.DataBind();

                        uclPaging.Visible = true;
                        uclPaging.BindPager();
                    }
                    else
                    {
                        grdPOInvoiceList.DataSource = null;
                        grdPOInvoiceList.DataBind();
                        uclPaging.Visible = false;
                    }
                        break;
                    #endregion
                    #region PENDINGPOLIST
                    case ControlsEnum.PENDINGPOLIST:
                       #region Paging Properties
		                rowCount = 0;
                        pageSize = Convert.ToInt32(GetLocalResourceObject("PageSize_POList"));
                        rowCount = dtPendingPOList.Rows.Count > 0 ? Convert.ToInt32(dtPendingPOList.Rows[0]["TOTAL_ROW_COUNT"].ToString()) : 0;
                        uclPOPaging.TotalPages = rowCount == 0 ? 1 : (rowCount <= pageSize) ? 1 :
                                      (rowCount % pageSize) == 0 ? (rowCount / pageSize) :
                                      (rowCount / pageSize) + 1;
                        PageIndexPO = PageIndexPO == null ? CommonConstants.SELECT_VALUE_ONE : PageIndexPO;
                        uclPOPaging.CurrentPage = Convert.ToInt32(PageIndexPO); 
	                 #endregion
                        if (dtPendingPOList != null && dtPendingPOList.Rows.Count > 0)
                        {
                            grdPendingPoList.DataSource = dtPendingPOList;
                            grdPendingPoList.DataBind();
                            uclPOPaging.Visible = true;
                            uclPOPaging.BindPager();
                        }
                        else
                        {
                            grdPendingPoList.DataSource = null;
                            grdPendingPoList.DataBind();
                            uclPOPaging.Visible = false;
                        }      
                        break;
                    #endregion                   
                    #region POINVOICELIST
                    case ControlsEnum.POINVOICELIST:
                       if (POInvoiceHeaderSession != null && POInvoiceHeaderSession.POMappingDetails != null && POInvoiceHeaderSession.POMappingDetails.Count > 0)
                            grdPOList.DataSource = POInvoiceHeaderSession.POMappingDetails.ToList();
                        else
                            grdPOList.DataSource = null;
                        grdPOList.DataBind();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "$(document).ready(function(){CalculateTotal(0);});", true);
                        break;
                    #endregion
                    #region UPLOADED FILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (POUploadList != null)
                        {
                            grdUploads.DataSource = POUploadList;
                            grdUploads.DataBind();
                        }
                        break;
                    #endregion
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        if (dtAmountDetails != null)
                        {
                            grdPaidAmntSplitup.DataSource = dtAmountDetails;
                            grdPaidAmntSplitup.DataBind();
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
                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();             
                base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                SetCancelRef(CurrPK);
                ucrWrkf.FillWorkFlowDetails();
                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                {
                    ucrWrkf.ViewType = 1;                 
                }
                else
                {
                    ucrWrkf.ViewType = 0;                
                }
                ucrWrkf.ViewAction();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;          
            POInvoiceHeaderSession = null;
            hdfIVHPK.Value = "";
            ModifiedDatePnl.Visible = false;        
            txtInvoiceNumber.Text = "Select/Type";
            hdfIVHPK.Value = "";
            txtVendor.Text = "Select/Type";
            txtpoNo.Text = string.Empty;
            hdfVendorID.Value = "";            
            vPK=0;
            dtPendingPOList = null;

            ddlAddressType.ClearSelection();
            txtInvdate.Text = string.Empty;
            txtSupplierInvNo.Text = string.Empty;
            txtVatTaxId.Text = string.Empty;
            txtInvoiceReceivedon.Text = string.Empty;
            txtPaybydate.Text = string.Empty;
            txtRemarks.Text = string.Empty;

            hdfIsContDupVenInvNo.Value = "0";

            txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
            chkOriginalinvoice.Checked = false;   
            txtPaybydateFrom.Text = string.Empty;
            txtPayByToDate.Text = string.Empty;
            ddlStatus.SelectedValue = "3";
            base.WkfRefID = 0;
            hdfTaxSettings.Value = string.Empty;
            POUploadList = null;
            FileDetailsList = null;         
            ResetForm(ControlsEnum.ADDITEM);
            ddlCompanySrch.SelectedIndex = -1;
            LastModifiedTime = System.DateTime.Now;
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ResetRadioButtonSelection", "ResetSelection();", true);
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                #region ADDITEM
                case ControlsEnum.ADDITEM:                 
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break; 
                #endregion
                #region POINVHEADER
                case ControlsEnum.POINVHEADER:
                    CurrPK = 0;                  
                    POInvoiceHeaderSession = null;
                    lblInvoiceNo.Text = Resources.Messages.DocGenerationNew;
                    hdfInvoiceNo.Value = string.Empty;
                    txtVendorHd.Text = string.Empty;
                    hdfVendorHd.Value = string.Empty;
                    txtPONumber.Text = string.Empty;
                    hdfPoPK.Value = string.Empty;
                    txtInvdate.Text = string.Empty;
                    txtPaybydate.Text = string.Empty;  
                    txtInvoiceAmt.Text = string.Empty;
                    txtNetAmount.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    dtPendingPOList = null;
                    soInvoiceDetailsList = null;
                    POUploadList = null;
                    FileDetailsList = null;         
                    grdPendingPoList.DataSource = null;
                    grdPendingPoList.DataBind();
                    grdPOList.DataSource = null;
                    grdPOList.DataBind();
                    grdUploads.DataSource = null;
                    grdUploads.DataBind();

                    txtPONumber.Text = string.Empty;
                    hdfPoPK.Value = string.Empty;
                    txtPendingFromDate.Text = string.Empty;
                    txtPendingToDate.Text = string.Empty;

                    base.WkfRefID = ucrWrkf.RefID = 0;
                    SetCancelRef(CurrPK);
                    ucrWrkf.FillWorkFlowDetails();
                    ucrWrkf.ViewType = 1;
                    ucrWrkf.ViewAction();
                    break;
                #endregion
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
        public string GetFormattedCurrencyWithSeperator(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperator.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetCeiledInteger(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return Math.Ceiling(num).ToString();
        }

        /// <summary>
        /// Check the uploaded file is valid
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private bool IsValidExtension(string extension)
        {
            string BlockedExtensions = "dll";
            if (System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower() != string.Empty)
            {
                BlockedExtensions = System.Configuration.ConfigurationManager.AppSettings["BlockedExtensions"].ToLower();
            }
            bool flag = true;
            string[] extensionList = BlockedExtensions.Split(',');
            for (int i = 0; i < extensionList.Length; i++)
                if (("." + extensionList[i]) == extension)
                {
                    flag = false;
                    break;
                }
            return flag;
        }

        /// <summary>
        /// Redirect to a page if user has permission on that page.
        /// </summary>
        /// <param name="RedirectUrl">Page Url</param>
        private void CheckUserRightsAndRedirect(string RedirectUrl)
        {
            CommonBL userAuth = new CommonBL();
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, RedirectUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
                Response.Redirect(RedirectUrl, false);
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;             
            try
            {       
                GridViewRow gridRow;

                bool bIsChecked = false;
             
                long? result;            
                int selectedItemPK;             
                TextBox WrkfComments;
                decimal TotalAmount = 0;
                decimal NetAmount = 0;
                Label lblTotalPayNowFooter;
                FileInfo tempFileInfoObj;
                RadioButton rbtn;
                HiddenField hdfDept;
                int selectedInvPK;
                int dept;
                string savePath = string.Empty;
                string poPK;
                POInvoiceService poInvoiceServiceClient;
                poInvoiceServiceClient = null;

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
                    if (((DropDownList)sender).ID == "ddlAddressType")
                    {
                        commonActions = ActionsEnum.CHANGETYPE;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                switch (commonActions)
                {
                    #region Item Selected
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow selectedGrdrow = (sender as RadioButton).Parent.Parent as GridViewRow;
                        rbtn = sender as RadioButton;
                        bIsChecked = true;
                        HiddenField hdfStatus = (HiddenField)selectedGrdrow.FindControl("hdfStatus");
                        hdfSelRecordStatus.Value = hdfStatus.Value.ToString();
                        selectedInvPK = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfInvoiceID")).Value);
                        hdfDept = selectedGrdrow.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = workflowCore.GetRefID(selectedInvPK, PageProcessID);
                        ////
                        if (Convert.ToInt16(((HiddenField)selectedGrdrow.FindControl("hdfDelStatus")).Value) == 1)
                        {
                            btnSave.Visible = false;
                            btnEditforCancel.Visible = false;
                            hdfIsInvCancelled.Value = "1";
                        }
                        else
                        {
                            btnSave.Visible = true;
                            btnEditforCancel.Visible = true;
                            hdfIsInvCancelled.Value = "0";
                        }
                        if (Convert.ToInt32(hdfStatus.Value) == 0)
                        {
                            btnEditforCancel.Visible = false;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        AST_DOC_MODE.Value = GetDOCMODE();
                        AST_CODE.Value = ApplicationType.TPI;
                        ResetForm(ControlsEnum.POINVHEADER);                       
                        EntryStatus = EntryStatus.NEWMODE;
                        FillProcessID(1);
                        TotalPages = 0;
                        hdfIsPendingPOVisible.Value = "0";
                        PageIndexPO = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetFieldValues(ControlsEnum.VENDORCONTACTYPE); 
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        LastModifiedTime = System.DateTime.Now;
                        hdfIsInvCancelled.Value = "0";
                        break;
                    #endregion                  
                    #region VENDOR CHANGE/DTL SEARCH
                    case ActionsEnum.DTLSEARCH:
                    case ActionsEnum.VENDORSELECTED:
                        POInvoiceHeaderSession = null;
                        dtPendingPOList = null;
                        PageIndexPO = CommonConstants.SELECT_VALUE_ONE;
                        GetFieldValues(ControlsEnum.PENDINGPOLIST);
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        hdfIsPendingPOVisible.Value = "1";
                        break;
                    #endregion
                    #region ADD TO LIST
                    case ActionsEnum.ADDTOLIST:
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        PoHeaderObj = new DirectPOHeaderBO();
                        PoHeaderObj.POList = new List<DirectPOHeaderListBO>();
                        List<DirectPOHeaderListBO> objItemList = new List<DirectPOHeaderListBO>();
                        DirectPOHeaderListBO objPoList;                       
                        HiddenField hdfPOPk;
                        HiddenField hdfPOVendor;
                        HiddenField hdfPOType;
                        HiddenField hdfPOCurrency;
                        if (POInvoiceHeaderSession != null && POInvoiceHeaderSession.POMappingDetails != null)
                        {
                            POInvoiceHeaderSession.POMappingDetails.ForEach(dtl =>
                            {
                                objPoList = new DirectPOHeaderListBO();
                                objPoList.POH_PK = dtl.IVM_PO_HDR;
                                objPoList.POH_VENDOR = Convert.ToInt32(POInvoiceHeaderSession.IVH_VENDOR);
                                objPoList.POH_TYPE = Convert.ToInt32(POInvoiceHeaderSession.IVH_TYPE);
                                objPoList.POH_CURRENCY = POInvoiceHeaderSession.IVH_CURRENCY;
                                objItemList.Add(objPoList);
                            });
                        }
                        #region grdPendingPoList
		                foreach (GridViewRow grdrow in grdPendingPoList.Rows)
                        {
                            CheckBox chkSCselect = (CheckBox)grdrow.FindControl("chkPoPendSelect");
                            if (chkSCselect.Checked)
                            {
                                hdfPOPk = (HiddenField)grdrow.FindControl("hdfPOPk");
                                hdfPOVendor = (HiddenField)grdrow.FindControl("hdfPOVendorPK");
                                hdfPOType = (HiddenField)grdrow.FindControl("hdfPOType");
                                hdfPOCurrency = (HiddenField)grdrow.FindControl("hdfPOCurrency");
                                objPoList = new DirectPOHeaderListBO();
                                objPoList.POH_PK = Convert.ToInt32(hdfPOPk.Value);
                                objPoList.POH_VENDOR = Convert.ToInt32(hdfPOVendor.Value);
                                objPoList.POH_TYPE = Convert.ToInt32(hdfPOType.Value);
                                objPoList.POH_CURRENCY = Convert.ToInt32(hdfPOCurrency.Value);
                                if (objItemList != null && (  objItemList.Where(r => r.POH_VENDOR != Convert.ToInt32(hdfPOVendor.Value)).Count() > 0
                                                            ||objItemList.Where(r => r.POH_TYPE != Convert.ToInt32(hdfPOType.Value)).Count() > 0
                                                            ||objItemList.Where(r => r.POH_CURRENCY != Convert.ToInt32(hdfPOCurrency.Value)).Count() > 0
                                                           )
                                    )
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Muliple_PO").ToString()) + "');", true);
                                    return;
                                }
                                if (objItemList != null && objItemList.Where(r => r.POH_PK == Convert.ToInt32(hdfPOPk.Value)).Count() <= 0)
                                    objItemList.Add(objPoList);
                            }
                        } 
	                   #endregion
                        if (objItemList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRecordsSelected").ToString()) + "');", true);
                            return;
                        }
                        PoHeaderObj.POList = objItemList;
                      
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.POINVHEADER);                       
                        SetFieldValues(ControlsEnum.POINVHEADER);  
                        SetFieldValues(ControlsEnum.POINVOICELIST);
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);                  
                        hdfIsPendingPOVisible.Value = "0";
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        break;
                    #endregion
                    #region SAVE,DELETE,SAVESUBMIT,SUBMIT,WKFSUBMIT,EDITFORCANCEL,DELETESUBMIT
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            bool cont = false;
                            #region Check Head Office/Branch selection & Have PO Items
                            if (ddlAddressType.Items.Count <= 0)
                            {
                                cont = false;

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);//Please Select Head Office/Branch
                                return;
                            }
                            if (grdPOList.Rows.Count <= 0)
                            {

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_ErrSave_Invoice").ToString()) + "');", true);//There should be atleast one PO item for saving.
                                return;
                            }
                            #endregion
                            #region grdPOList Validations
                            foreach (GridViewRow grdPOrow in grdPOList.Rows)
                            {
                                Label lblTotalAmount = (Label)grdPOrow.FindControl("lblTotalAmount");
                                Label lblInvoiced = (Label)grdPOrow.FindControl("lblInvoiced");
                                TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtPayNow");
                                TextBox txtOthercharges = (TextBox)grdPOrow.FindControl("txtOthercharges");
                                Label lblOtherAmount = (Label)grdPOrow.FindControl("lblOtherAmount");
                                HiddenField hdfOtherchargeOLD = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");
                                HiddenField hdfPayNow = (HiddenField)grdPOrow.FindControl("hdfPayNow");
                                HiddenField hdfOtherChargesPrev = (HiddenField)grdPOrow.FindControl("hdfOtherChargesPrev");

                                decimal InvNow = 0;
                                decimal OtherChrg = 0;
                                decimal.TryParse(txtPayNow.Text, out InvNow);
                                decimal.TryParse(txtOthercharges.Text, out OtherChrg);
                                if (OtherChrg > InvNow)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_msg_Otherchrg").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }
                                #region Invalid Other Charge Checking  //If Advance Invoice have no tax then hide Tax and OtherCharge Columns.Hence no need for this checking
                                if (IsAdvInvHasTax == true)
                                {
                                    if (txtOthercharges.Text != "")
                                    {
                                        if (Convert.ToDecimal(lblOtherAmount.Text.Replace(",", "")) < Convert.ToDecimal(txtOthercharges.Text.Replace(",", "")) + Convert.ToDecimal(hdfOtherChargesPrev.Value))
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                            "ClosePopup();", true);
                                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                          "ClosePopup();", true);
                                        litErrorMsg.Text = GetLocalResourceObject("Err_Invalid_OC").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                }
                                #endregion

                                if (ItemStatus == 0)
                                {
                                    if ((string.IsNullOrEmpty(lblTotalAmount.Text) ? 0 : Convert.ToDecimal(lblTotalAmount.Text)) >= (string.IsNullOrEmpty(lblInvoiced.Text) ? 0 : Convert.ToDecimal(lblInvoiced.Text)) + (string.IsNullOrEmpty(txtPayNow.Text) ? 0 : Convert.ToDecimal(txtPayNow.Text)))
                                    {
                                        #region Is Advance Invoice Have tax then  othercharge is considered. Otherwise othercharge is zero.
                                        if (IsAdvInvHasTax == true)
                                        {
                                            if ((string.IsNullOrEmpty(lblOtherAmount.Text) ? 0 : Convert.ToDecimal(lblOtherAmount.Text)) >= (string.IsNullOrEmpty(txtOthercharges.Text) ? 0 : Convert.ToDecimal(txtOthercharges.Text)) + (string.IsNullOrEmpty(hdfOtherChargesPrev.Value) ? 0 : Convert.ToDecimal(hdfOtherChargesPrev.Value)))
                                            {
                                                if (((string.IsNullOrEmpty(txtPayNow.Text) ? 0 : Convert.ToDecimal(txtPayNow.Text)) + (string.IsNullOrEmpty(lblInvoiced.Text) ? 0 : Convert.ToDecimal(lblInvoiced.Text))) - ((string.IsNullOrEmpty(txtOthercharges.Text) ? 0 : Convert.ToDecimal(txtOthercharges.Text)) + (Convert.ToDecimal(hdfOtherChargesPrev.Value))) <= ((string.IsNullOrEmpty(lblTotalAmount.Text) ? 0 : Convert.ToDecimal(lblTotalAmount.Text)) - (string.IsNullOrEmpty(lblOtherAmount.Text) ? 0 : Convert.ToDecimal(lblOtherAmount.Text))))
                                                {
                                                    cont = true;
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                    "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                    "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (((string.IsNullOrEmpty(txtPayNow.Text) ? 0 : Convert.ToDecimal(txtPayNow.Text)) + (string.IsNullOrEmpty(lblInvoiced.Text) ? 0 : Convert.ToDecimal(lblInvoiced.Text))) <= ((string.IsNullOrEmpty(lblTotalAmount.Text) ? 0 : Convert.ToDecimal(lblTotalAmount.Text))))
                                            {
                                                cont = true;
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("InvAmtNotTallied").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                break;
                                            }

                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                }
                                else
                                {
                                    #region MyRegion
                                    if (Convert.ToDecimal(lblTotalAmount.Text) >= (Convert.ToDecimal(lblInvoiced.Text) - Convert.ToDecimal(hdfPayNow.Value)) + Convert.ToDecimal(txtPayNow.Text))
                                    {
                                        if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text) + Convert.ToDecimal(hdfOtherChargesPrev.Value))
                                        {
                                            if ((Convert.ToDecimal(txtPayNow.Text) + ((Convert.ToDecimal(lblInvoiced.Text) - Convert.ToDecimal(hdfPayNow.Value)))) - (Convert.ToDecimal(txtOthercharges.Text) + (Convert.ToDecimal(hdfOtherChargesPrev.Value))) <= (Convert.ToDecimal(lblTotalAmount.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                            {
                                                cont = true;
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                break;
                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                "ClosePopup();", true);
                                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                            break;
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                "ClosePopup();", true);
                                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                            if (cont == true)
                            {
                                if (hdfIsJournalize.Value == "True")
                                {
                                    litErrorMsg.Text = Resources.Messages.Msg_Journalize;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    #region WorkflowTransactionFlag.SAVE
                                    if (grdPOList.Rows.Count > 0)
                                    {

                                        NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                                        TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                                        lblTotalPayNowFooter = (Label)grdPOList.FooterRow.FindControl("lblTotalPayNowFooter");
                                        lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                                        if (NetAmount == TotalAmount)
                                        {
                                            invoiceHeaderObj = new DirectPOInvoiceHeader();
                                            invoiceHeaderObj = (DirectPOInvoiceHeader)SetUIValuesToObject(ControlsEnum.FINANCEINVOICEHDR);
                                            invoiceHeaderObj.WKF_FLAG = 0;
                                            invoiceHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                            string xmlDoc = CommonFunctions.XmlSerialize<DirectPOInvoiceHeader>(invoiceHeaderObj);
                                            // save Process Control inspection details
                                            string invNumber = string.Empty;
                                            result = BusinessLogic.POInvoicing.POInvoiceBL.SavePOInvoiceTradingWkf(xmlDoc, out invNumber);//SPFIN_INVOICE_VND_TRADING_WKF_SAVE
                                            if (result > 0)
                                            {
                                                #region Success region
                                                #region ATTACHMENT SAVE
                                                if (POUploadList != null && POUploadList.Count > 0)
                                                {
                                                    savePath = string.Empty;
                                                    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                                    {
                                                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                        if (!Directory.Exists(savePath))
                                                            Directory.CreateDirectory(savePath);
                                                        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                                    }
                                                    else
                                                    {
                                                        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                                                    }

                                                    foreach (DirectPOInvoiceUploads obj in POUploadList)
                                                    {
                                                        string filePath = savePath + obj.AttachmentFileName;
                                                        FileInfo attachedFileInfo = new FileInfo(filePath);
                                                        if (FileDetailsList != null)
                                                        {
                                                            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                            if (fileDetailsObj != null)
                                                            {
                                                                fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                                            }
                                                        }
                                                    }
                                                }
                                                #endregion
                                                if (string.IsNullOrEmpty(lblInvoiceNo.Text.Trim()) || lblInvoiceNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Saved_Success").ToString();
                                                }
                                                else
                                                {
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                    object[] args = new object[2];
                                                    args[0] = Resources.PageNameRes.AdvanceInvoice;
                                                    args[1] = lblInvoiceNo.Text.Trim();
                                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                                }
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);                                                
                                                FillProcessID(1);
                                                ResetForm();
                                                GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                                GetFieldValues(ControlsEnum.INVOICELIST);
                                                SetFieldValues(ControlsEnum.INVOICELIST);
                                                this.btnNew.Focus();
                                                EntryStatus = EntryStatus.LISTMODE;
                                                #endregion
                                            }
                                            else
                                            {
                                                #region Error/Validation Message Region
                                                if (result == (int)DbSaveStatus.SQLERROR)
                                                {
                                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                        + "','" + Resources.ErpRes.Information + "');", true);

                                                }                                               
                                                else if (result == -6)
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "$(document).ready(function(){ShowDuplicateVendorInvNoContinue(1);});", true);
                                                }
                                                else if (result == -9)//need checking for payment created or not
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PaymentCreated").ToString()) + "','" + Resources.Messages.Information + "');", true);//Payment created against this invoice.
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                }
                                                #endregion
                                            }
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    #endregion
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.POInvoicing.POInvoiceBL.DeletePOInvoiceTradingDetails(CurrPK, LastModifiedTime, ApplicationType.TPI, currentUser.PKUser.ToString());
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Invoice Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.POInvoice);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                FillProcessID(1);
                                ResetForm();
                                GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                GetFieldValues(ControlsEnum.INVOICELIST);
                                SetFieldValues(ControlsEnum.INVOICELIST);
                                this.btnNew.Focus();
                                EntryStatus = EntryStatus.LISTMODE;
                            }
                            else
                            {
                                #region ShowErrorMessages
                                if (result == (int)DbDeleteStatus.SQLERROR)
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.CONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.INVOICELIST);
                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.INVOICELIST);
                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.POInvoice);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                #endregion
                            }
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        if (ddlAddressType.Items.Count <= 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                            return;
                        }
                        if (grdPOList.Rows.Count > 0)
                        {
                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                            lblTotalPayNowFooter = (Label)grdPOList.FooterRow.FindControl("lblTotalPayNowFooter");
                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                            if (NetAmount == TotalAmount)
                            {
                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                                ucrWrkf.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        if (grdPOList.Rows.Count > 0)
                        {
                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                            lblTotalPayNowFooter = (Label)grdPOList.FooterRow.FindControl("lblTotalPayNowFooter");
                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                            if (NetAmount == TotalAmount)
                            {
                                ucrWrkf.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region WRKFSUBMIT
                    case ActionsEnum.WRKFSUBMIT:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                if (hdfIsJournalize.Value == "True")
                                {
                                    litErrorMsg.Text = Resources.Messages.Msg_Journalize;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    bool cont = false;
                                    foreach (GridViewRow grdPOrow in grdPOList.Rows)
                                    {
                                        Label lblTotalAmount = (Label)grdPOrow.FindControl("lblTotalAmount");
                                        Label lblInvoiced = (Label)grdPOrow.FindControl("lblInvoiced");
                                        TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtPayNow");
                                        TextBox txtOthercharges = (TextBox)grdPOrow.FindControl("txtOthercharges");
                                        Label lblOtherAmount = (Label)grdPOrow.FindControl("lblOtherAmount");
                                        HiddenField hdfOtherchargeOLD = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");
                                        HiddenField hdfOtherChargesPrev = (HiddenField)grdPOrow.FindControl("hdfOtherChargesPrev");

                                        decimal InvNow = 0;
                                        decimal OtherChrg = 0;
                                        decimal.TryParse(txtPayNow.Text, out InvNow);
                                        decimal.TryParse(txtOthercharges.Text, out OtherChrg);
                                        if (OtherChrg > InvNow)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_msg_Otherchrg").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                        #region Invalid Other Charge Checking  //If Advance Invoice have no tax then hide Tax and OtherCharge Columns.Hence no need for this checking
                                        if (IsAdvInvHasTax == true)
                                        {
                                            if (txtOthercharges.Text != "")
                                            {
                                                if (Convert.ToDecimal(lblOtherAmount.Text.Replace(",", "")) < Convert.ToDecimal(txtOthercharges.Text.Replace(",", "")) + Convert.ToDecimal(hdfOtherChargesPrev.Value))
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                                    "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                                  "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_Invalid_OC").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                return;
                                            }
                                        }
                                        #endregion
                                        if ((string.IsNullOrEmpty(lblTotalAmount.Text) ? 0 : Convert.ToDecimal(lblTotalAmount.Text)) >= (string.IsNullOrEmpty(lblInvoiced.Text) ? 0 : Convert.ToDecimal(lblInvoiced.Text)) + (string.IsNullOrEmpty(txtPayNow.Text) ? 0 : Convert.ToDecimal(txtPayNow.Text)))
                                        {
                                            #region Is Advance Invoice Have tax then  othercharge is considered. Otherwise othercharge is zero.
                                            if (IsAdvInvHasTax == true)
                                            {
                                                if ((string.IsNullOrEmpty(lblOtherAmount.Text) ? 0 : Convert.ToDecimal(lblOtherAmount.Text)) >= (string.IsNullOrEmpty(txtOthercharges.Text) ? 0 : Convert.ToDecimal(txtOthercharges.Text)) + (string.IsNullOrEmpty(txtOthercharges.Text) ? 0 : Convert.ToDecimal(hdfOtherChargesPrev.Value)))
                                                {
                                                    if (((string.IsNullOrEmpty(txtPayNow.Text) ? 0 : Convert.ToDecimal(txtPayNow.Text)) + (string.IsNullOrEmpty(lblInvoiced.Text) ? 0 : Convert.ToDecimal(lblInvoiced.Text))) - ((string.IsNullOrEmpty(txtOthercharges.Text) ? 0 : Convert.ToDecimal(txtOthercharges.Text)) + (Convert.ToDecimal(hdfOtherChargesPrev.Value))) <= ((string.IsNullOrEmpty(lblTotalAmount.Text) ? 0 : Convert.ToDecimal(lblTotalAmount.Text)) - (string.IsNullOrEmpty(lblOtherAmount.Text) ? 0 : Convert.ToDecimal(lblOtherAmount.Text))))
                                                    {
                                                        cont = true;
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        break;
                                                    }
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (((string.IsNullOrEmpty(txtPayNow.Text) ? 0 : Convert.ToDecimal(txtPayNow.Text)) + (string.IsNullOrEmpty(lblInvoiced.Text) ? 0 : Convert.ToDecimal(lblInvoiced.Text))) <= ((string.IsNullOrEmpty(lblTotalAmount.Text) ? 0 : Convert.ToDecimal(lblTotalAmount.Text))))
                                                {
                                                    cont = true;
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                             "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    break;
                                                }
                                            }
                                            #endregion
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                             "ClosePopup();", true);
                                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                    }
                                    if (cont == true)
                                    {
                                        if (grdPOList.Rows.Count > 0)
                                        {
                                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                                            lblTotalPayNowFooter = (Label)grdPOList.FooterRow.FindControl("lblTotalPayNowFooter");
                                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                                            if (NetAmount == TotalAmount)
                                            {
                                                invoiceHeaderObj = new DirectPOInvoiceHeader();
                                                invoiceHeaderObj = (DirectPOInvoiceHeader)SetUIValuesToObject(ControlsEnum.FINANCEINVOICEHDR);
                                                invoiceHeaderObj.WKF_FLAG = 1;
                                                if (hdfExchangeRate.Value != "-1")
                                                {
                                                    if (invoiceHeaderObj != null)
                                                    {
                                                        invoiceHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                                        SaveTransaction(invoiceHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                                    }
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                    litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.TPI))
                                {
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_PI_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                    GetFieldValues(ControlsEnum.INVOICELIST);
                                    SetFieldValues(ControlsEnum.INVOICELIST);
                                    btnNew.Focus();
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion 
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        ddlVendorBranch.Items.Clear();
                        ddlVendorBranch.DataSource = null;
                        ddlVendorBranch.DataBind();
                        foreach (GridViewRow grdrow in grdPOInvoiceList.Rows)
                        {
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfIVHPK.Value = CurrPK.ToString();
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;                           
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);                             
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);                           

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(11);                           
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;                           
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            GetFieldValues(ControlsEnum.PENDINGPOLIST);
                            SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region DELETESUBMIT popup
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #endregion
                    #region EDIT/VIEW/INVOICEDETAIL
                    case ActionsEnum.EDIT:
                    case ActionsEnum.VIEW:
                    case ActionsEnum.INVOICEDETAIL:
                        bIsChecked = false;
                        gridRow = null;
                        foreach (GridViewRow grdrow in grdPOInvoiceList.Rows)
                        {
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                gridRow = grdrow;
                                break;
                            }
                        }
                        if (bIsChecked && gridRow != null)
                        {
                            ResetForm(ControlsEnum.POINVHEADER);
                            CurrPK = Convert.ToInt32(((HiddenField)gridRow.FindControl("hdfInvoiceID")).Value);
                            Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)gridRow.FindControl("hdfVendorPK")).Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)gridRow.FindControl("lblVendor")).Text;
                            Approved = Convert.ToInt32(((HiddenField)gridRow.FindControl("hdfApproved")).Value);
                            HiddenField hdfPosted = (HiddenField)gridRow.FindControl("hdfPosted");
                            if (hdfPosted != null)
                                Posted = Convert.ToBoolean(hdfPosted.Value);                         
                            ItemStatus = Convert.ToInt16(((HiddenField)gridRow.FindControl("hdfStatus")).Value);
                            hdfDept = gridRow.FindControl("hdfDept") as HiddenField;
                            if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                            FillProcessID(1);
                            WorkflowCore.CoreService workflowCoreObj = new WorkflowCore.CoreService();
                            base.WkfRefID = workflowCoreObj.GetRefID(CurrPK, PageProcessID);
                            SetUIEditView(commonActions);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();
                            ModifiedDatePnl.Visible = true;

                            GetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            GetFieldValues(ControlsEnum.PENDINGPOLIST);
                            SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:                        
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;

                    #endregion                   
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region DTL CLEAR SEARCH
                    case ActionsEnum.DTLCLEARSEARCH:
                        txtPONumber.Text = string.Empty;
                        hdfPoPK.Value = string.Empty;
                        txtPendingFromDate.Text = string.Empty;
                        txtPendingToDate.Text = string.Empty;
                        PageIndexPO = CommonConstants.SELECT_VALUE_ONE;
                        dtPendingPOList = null;
                        GetFieldValues(ControlsEnum.PENDINGPOLIST);
                        SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        hdfIsPendingPOVisible.Value = "1";
                        break;
                    #endregion                                   
                    #region INVOICELIST
                    case ActionsEnum.INVOICELIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        hdfIVHPK.Value = "";
                        ResetForm();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        EntryStatus = EntryStatus.LISTMODE;                    
                        
                        break;
                    #endregion                             
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        break;
                    #endregion
                    #region REMOVE
                    case ActionsEnum.REMOVE:
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        gridRow = ((Button)sender).Parent.Parent as GridViewRow;
                        HiddenField hdfPurchaseOrderPk = (HiddenField)gridRow.FindControl("hdfPONumber");
                        int POPK = 0;
                        int.TryParse(hdfPurchaseOrderPk.Value, out POPK);
                        if (POInvoiceHeaderSession != null && POInvoiceHeaderSession.POMappingDetails != null)
                        {
                            DirectPOInvoiceMappingDetails objMappingDtl = POInvoiceHeaderSession.POMappingDetails.SingleOrDefault(r => r.IVM_PO_HDR == POPK);
                            POInvoiceHeaderSession.POMappingDetails.Remove(objMappingDtl);
                            SetFieldValues(ControlsEnum.POINVHEADER);
                            SetFieldValues(ControlsEnum.POINVOICELIST);                          
                            SetFieldValues(ControlsEnum.PENDINGPOLIST);
                        }
                        break;
                    #endregion                   
                    #region PRINT PO
                    case ActionsEnum.PRINTPO:
                        GridViewRow grwPoLst = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                        HiddenField hdfPoPk = grwPoLst.FindControl("hdfPoPk") as HiddenField;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfPoPk.Value + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion
                    #region UPLOAD (ADDITEM,EDITITEM,REMOVEITEM)
                    #region ADDITEMUPLOAD
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {

                            tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                            if (!IsValidExtension(tempFileInfoObj.Extension))
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                            else
                            {
                                if (CurrSlNo != 0)
                                {
                                    if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                    {
                                        poUploadObj = POUploadList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                        if (poUploadObj != null)
                                        {
                                            if (FileDetailsList == null)
                                            {
                                                FileDetailsList = new List<FileDetails>();
                                            }
                                            if (fupUpload.HasFile)
                                            {

                                                tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                                string attachmentFileFormat = tempFileInfoObj.Extension;
                                                string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;
                                                poUploadObj.AttachmentFileName = attachmentFileName;
                                                poUploadObj.FileExtension = tempFileInfoObj.Extension;
                                                poUploadObj.DOC_NAME = fupUpload.FileName;
                                                poUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                {
                                                    poUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                }
                                                else
                                                {
                                                    poUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                                }
                                                FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == CurrSlNo);
                                                if (fileDetailsObj == null)
                                                {
                                                    FileDetailsList.Add(new FileDetails() { SlNo = CurrSlNo, PoFile = HttpContext.Current.Request.Files[0] });
                                                }
                                                else
                                                {
                                                    fileDetailsObj.PoFile = HttpContext.Current.Request.Files[0];
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    if (fupUpload.HasFile)
                                    {

                                        int slno = 1;
                                        if (POUploadList == null || POUploadList.Count == 0)
                                        {
                                            POUploadList = new List<BusinessObject.POInvoicing.DirectPOInvoiceUploads>();
                                            slno = 1;
                                        }
                                        else
                                        {
                                            slno = POUploadList.Max(itm => itm.DOC_SEQ_NO);
                                            slno++;
                                        }
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<FileDetails>();
                                        }

                                        poUploadObj = new DirectPOInvoiceUploads();
                                        poUploadObj.DOC_PK = 0;
                                        poUploadObj.DOC_SEQ_NO = slno;
                                        tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                        string attachmentFileFormat = tempFileInfoObj.Extension;
                                        string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                        poUploadObj.AttachmentFileName = attachmentFileName;
                                        poUploadObj.FileExtension = tempFileInfoObj.Extension;
                                        poUploadObj.DOC_NAME = fupUpload.FileName;
                                        poUploadObj.DOC_TYPE = tempFileInfoObj.Extension;
                                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                        {
                                            poUploadObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                        }
                                        else
                                        {
                                            poUploadObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                        }
                                        poUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                        POUploadList.Add(poUploadObj);

                                    }
                                }
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                            grdUploads.Focus();
                        }
                        break;
                    #endregion
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (POUploadList != null && POUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                POUploadList = POUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();                              
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (POUploadList != null && POUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                poUploadObj = POUploadList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        break;
                    #endregion
                    #endregion
                    #region CHANGETYPE
                    case ActionsEnum.CHANGETYPE:
                        SetBranchCodeVisibility();
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        if (CurrPK > 0)
                        {
                            poInvoiceServiceClient = new POInvoiceService();
                            poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                            finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                            serviceUtilityObj = new ServiceUtility();
                            finInvoiceVndHdrObj.IVH_PK = CurrPK;
                            finInvoiceVndHdrObj.IVH_ACTIVE = 1;
                            finInvoiceVndHdrList = poInvoiceServiceClient.GetInvoiceHdrByPK(finInvoiceVndHdrObj);
                            if (finInvoiceVndHdrList[0].IVH_TYPE.ToString() == "2")
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=13") + "');", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=12") + "');", true);                            
                        }
                        break;
                    #endregion                    
                    #region Printlisting
                    case ActionsEnum.PRINTLISTING:

                        if (CurrPK == 0)
                        {
                            foreach (GridViewRow grdrow in grdPOInvoiceList.Rows)
                            {
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                    poInvoiceServiceClient = new POInvoiceService();
                                    poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                    finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                                    serviceUtilityObj = new ServiceUtility();
                                    finInvoiceVndHdrObj.IVH_PK = CurrPK;
                                    finInvoiceVndHdrObj.IVH_ACTIVE = 1;
                                    finInvoiceVndHdrList = poInvoiceServiceClient.GetInvoiceHdrByPK(finInvoiceVndHdrObj);
                                    if (finInvoiceVndHdrList[0].IVH_TYPE.ToString() == "2")
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=13") + "');", true);
                                    else
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.TPI + "&APPSUBTYPE=12") + "');", true);
                                    CurrPK = 0;
                                    return;
                                }
                            }                            
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region TAXSPLITUPPOPUP
                    case ActionsEnum.TAXDETAILSSPLITUP: 
                        int POH_Pk = int.Parse(((LinkButton)sender).CommandArgument.ToString());
                        if (POInvoiceHeaderSession != null && POInvoiceHeaderSession.POMappingDetails != null)
                        {
                            DirectPOInvoiceMappingDetails objMapping = POInvoiceHeaderSession.POMappingDetails.SingleOrDefault(r => r.IVM_PO_HDR == POH_Pk);
                            if (objMapping != null && objMapping.POTaxList != null)
                                grdTaxSplitupDetails.DataSource = objMapping.POTaxList.ToList();
                            else
                                grdTaxSplitupDetails.DataSource = null;
                            grdTaxSplitupDetails.DataBind();
                        }
                        else
                        {
                            grdTaxSplitupDetails.DataSource = null;
                            grdTaxSplitupDetails.DataBind();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divTaxSplitupDetails]','" + GetLocalResourceObject("TaxDetails").ToString() + "','400','200');", true);
                        break;
                    #endregion

                    #region AMOUNTDETAILS
                    case ActionsEnum.AMOUNTDETAILS:
                        HiddenField hdfInvoiceID = (HiddenField)((GridViewRow)((LinkButton)(sender)).Parent.Parent).FindControl("hdfInvoiceID");
                        long.TryParse(hdfInvoiceID.Value, out InvoicePk);
                        GetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        SetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPaidAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','200');", true);
                        break;
                    #endregion
                  
                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        poPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + poPK + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion                  
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {           
               
            }
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
                if (SortBy == e.SortExpression)
                {
                    //Toggle the sort expression
                    if (SortDirection == Resources.Report.SortAscending)
                        SortDirection = Resources.Report.SortDescending;
                    else
                        SortDirection = Resources.Report.SortAscending;
                }
                else
                {
                    SortBy = e.SortExpression;
                    SortDirection = Resources.Report.SortAscending;

                }
                this.PageIndex = "1";
                GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                GetFieldValues(ControlsEnum.INVOICEHDR);
                SetFieldValues(ControlsEnum.INVOICEHDR);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        /// <summary>
        /// Method used to Handle all actions in the page with GridView Row Bindinw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {           
            try
            {
                if (((GridView)sender).ID == "grdPOInvoiceList")
                {
                    #region grdPOInvoiceList
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        #region imgPosted CssClass & ToolTip
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;
                        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;
                        if (dtInvoiceList != null)
                        {
                            if (dtInvoiceList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString() != "" || dtInvoiceList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString() != string.Empty)
                            {
                                imgPosted.CssClass = dtInvoiceList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString();
                                imgPosted.ToolTip = dtInvoiceList.Rows[e.Row.RowIndex]["FTH_STATUS_TEXT"].ToString();
                            }
                            else
                            {
                                imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                                imgPosted.ToolTip = Resources.Captions.NotPosted;
                            }
                        }
                        #endregion
                        if (!string.IsNullOrEmpty(hdfPostingSettings.Value) && hdfPostingSettings.Value.Equals("0"))
                        {
                            e.Row.Cells[10].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        if (!string.IsNullOrEmpty(hdfPostingSettings.Value) && hdfPostingSettings.Value.Equals("0"))
                        {
                            e.Row.Cells[10].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        if (!string.IsNullOrEmpty(hdfPostingSettings.Value) && hdfPostingSettings.Value.Equals("0"))
                        {
                            e.Row.Cells[10].Visible = false;
                        }
                    }
                    #endregion
                }
                else if (((GridView)sender).ID == "grdPOList")
                {
                    #region grdPOList
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        #region DataRow
                        HiddenField hdfPurchaseOrderType = e.Row.FindControl("hdfPurchaseOrderType") as HiddenField;

                        if (Convert.ToByte(hdfPurchaseOrderType.Value) == (byte)PurchaseType.Import || !IsAdvInvHasTax)
                        {
                            grdPOList.Columns[10].Visible = true; //AdvInvoiced
                        }
                        else
                        {
                            grdPOList.Columns[10].Visible = false;
                        }

                        if (IsAdvInvHasTax == false) //If Advance Invoice have no tax then hide Tax and OtherCharge Columns
                        {
                            e.Row.Cells[13].Visible = false;
                            e.Row.Cells[14].Visible = false;
                        }
                        else
                        {
                            e.Row.Cells[13].Visible = true;
                            e.Row.Cells[14].Visible = true;
                        } 
                        #endregion
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        #region Footer
                        if (IsAdvInvHasTax == false) //If Advance Invoice have no tax then hide Tax and OtherCharge Columns
                        {
                            e.Row.Cells[13].Visible = false;
                            e.Row.Cells[14].Visible = false;
                        }
                        else
                        {
                            e.Row.Cells[13].Visible = true;
                            e.Row.Cells[14].Visible = true;
                        } 
                        #endregion
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        #region Header
                        if (IsAdvInvHasTax == false) //If Advance Invoice have no tax then hide Tax and OtherCharge Columns
                        {
                            e.Row.Cells[13].Visible = false;
                            e.Row.Cells[14].Visible = false;
                        }
                        else
                        {
                            e.Row.Cells[13].Visible = true;
                            e.Row.Cells[14].Visible = true;
                        } 
                        #endregion
                    } 
                    #endregion
                }  
                else if (((GridView)sender).ID == "grdUploads")
                {
                    int slno;
                    #region grdUploads
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {                          
                            e.Row.Cells[3].Visible = false;
                            e.Row.Cells[4].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        slno = Convert.ToInt32(grdUploads.DataKeys[e.Row.RowIndex][0]);
                        if (slno > 0)
                        {
                            e.Row.FindControl("fileView").Visible = FileDetailsList == null || FileDetailsList.Where(fle => fle.SlNo == slno).Count() == 0;
                        }
                    } 
                    #endregion
                }
                else if (((GridView)sender).ID == "grdPendingPoList")
                {
                    #region DataRow                   
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfPOPk = e.Row.FindControl("hdfPOPk") as HiddenField;
                        CheckBox chkPoPendSelect = e.Row.FindControl("chkPoPendSelect") as CheckBox;
                        if (POInvoiceHeaderSession != null && POInvoiceHeaderSession.POMappingDetails.Where(r => r.IVM_PO_HDR == Convert.ToInt32(hdfPOPk.Value)).Count() > 0)
                        {
                            //selectedRowColor
                            e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml(GetGlobalResourceObject("ErpRes", "selectedRowColor").ToString());
                            chkPoPendSelect.Checked = true;
                            chkPoPendSelect.Enabled = false;
                        }
                    } 
                    #endregion
                }                
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
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
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            uclPaging.CurrentPage = 1;
            uclPOPaging.CurrentPage = 1;

            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteInv.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnAlert.PreRender += new EventHandler(btnAction_PreRender);

            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnListPrint.PreRender += new EventHandler(btnAction_PreRender);

            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);       
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);
        
            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnAddItem.PreRender += new EventHandler(btnAction_PreRender);
            btnNew.PreRender += new EventHandler(btnAction_PreRender);


            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteInv.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnAlert.Load += new EventHandler(btnAction_Load);

            btnPrint.Load += new EventHandler(btnAction_Load);
            btnListPrint.Load += new EventHandler(btnAction_Load);

            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);  
            btnResetSelection.Load += new EventHandler(btnAction_Load);         
            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnAddItem.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);
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

            this.uclPOPaging.FirstPage += new ActionHandler(this.ActionHandler);
            this.uclPOPaging.PreviousPage += new ActionHandler(this.ActionHandler);
            this.uclPOPaging.NextPage += new ActionHandler(this.ActionHandler);
            this.uclPOPaging.LastPage += new ActionHandler(this.ActionHandler);
            this.uclPOPaging.PageChanged += new ActionHandler(this.ActionHandler);

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
                    GetFieldValues(ControlsEnum.INVOICELIST);
                    SetFieldValues(ControlsEnum.INVOICELIST);
                    EntryStatus = EntryStatus.LISTMODE;
                    EnableDisableButtons(e.TotalPages, "uclPaging");
                }
                else if (senderId == "uclPOPaging")
                {
                    PageIndexPO = uclPOPaging.CurrentPage.ToString();
                    GetFieldValues(ControlsEnum.PENDINGPOLIST);
                    SetFieldValues(ControlsEnum.PENDINGPOLIST);
                    EnableDisableButtons(e.TotalPages, "uclPOPaging");
                }   
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

        }      

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
            else if (pagerId == "uclPOPaging")
            {
                // Should we disable the first link
                uclPOPaging.FirstButtonEnabled = (uclPOPaging.CurrentPage == 1) ? false : true;
                // Should we disable the previous link
                uclPOPaging.PreviousButtonEnabled = (uclPOPaging.CurrentPage == 1) ? false : true;
                // Should we enable the next link
                uclPOPaging.NextButtonEnabled = (uclPOPaging.CurrentPage < iTotalPages) ? true : false;
                // Should we enable the last link
                uclPOPaging.LastButtonEnabled = (uclPOPaging.CurrentPage < iTotalPages) ? true : false;
            }
            
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
                vendPK = 0;
                int.TryParse(hdfVendorHd.Value, out vendPK);                

                hdfInvCategory.Value = Convert.ToByte((byte)POInvoiceCategory.Advanced).ToString();
                if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                {
                    if (Session[ERP.Utilities.SessionStrings.Transaction].ToString() == "CANCEL")
                    {
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.Transaction] = null;
                    }
                }
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "$(document).ready(function(){CalculateTotal(0);});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveLink", "$(document).ready(function () { RemoveBalAmntHyperLink();});", true);

                if (grdPOList.Rows.Count > 0)
                {                   
                    txtVendorHd.Enabled = false;
                }
                else
                {                   
                    txtVendorHd.Enabled = true;
                }     
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }
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
        private void SetBranchCodeVisibility()
        {
            txtBranchCode.Text = string.Empty;
            txtBranchCode.Text = string.Empty;     
            GetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);
            if (dsAdsTypeDtl != null && dsAdsTypeDtl.Tables.Count > 0)
            {
                if (dsAdsTypeDtl.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.Branch)
                    {
                        txtBranchCode.Enabled = true;
                        vrfBranchCode.Enabled = true;                      
                    }
                    else
                    {                       
                        vrfBranchCode.Enabled = false;                       
                    }
                    SetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);
                }
            }
        }
        /// <summary>
        /// Method to Fill Process ID
        /// </summary>
        private void FillProcessID(int pid)
        {
            string path = string.Empty;
            // if one page containes two process (pageurl?PID=1,pageurl?PID=2)
            //if (Request.QueryString[QueryStrings.PID] == null)
            //{
            if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
                path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "") + "?PID=" + pid.ToString();
            else
                path = Request.Url.AbsolutePath.ToLower() + "?PID=" + pid.ToString();
            //}
            //else
            //{
            //    if (System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower() != string.Empty)
            //        path = Request.Url.AbsolutePath.ToLower().Replace(System.Configuration.ConfigurationManager.AppSettings["VirtualDirectory"].ToLower(), "");
            //    else
            //        path = Request.Url.AbsolutePath.ToLower();
            //}

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
            if (Session[BusinessObject.Common.SessionStrings.CurDept] != null)
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
        }

        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(DirectPOInvoiceHeader invoiceHeaderObj, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (invoiceHeaderObj == null)
                invoiceHeaderObj = new DirectPOInvoiceHeader();
            #region Transaction Log and Application Code
            invoiceHeaderObj.ATL_APP_TYPE = ApplicationType.TPI;
            int numberGenerationSubType;
            if ((byte)POGroup == (byte)POInvoiceGroup.Services)
            {
                numberGenerationSubType = (int)AppSubTypeCNPurchase.NONSTOCK;//12
            }
            else
            {
                if (!string.IsNullOrEmpty(hdfPOItemType.Value))
                    numberGenerationSubType = Convert.ToInt16(hdfPOItemType.Value) == (Int16)POItemType.Others ? (int)AppSubTypeCNPurchase.NONSTOCK : (int)AppSubTypeCNPurchase.STOCK;
                else
                    numberGenerationSubType = (int)AppSubTypeCNPurchase.STOCK;
            }
            invoiceHeaderObj.APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.TPI : POGroup == POInvoiceGroup.Services ? ApplicationType.TPSI : ApplicationType.EI;
            invoiceHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
            invoiceHeaderObj.AST_VALUE = numberGenerationSubType.ToString();
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            invoiceHeaderObj.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            invoiceHeaderObj.WKF_APPLICATION = CurrPK;
            invoiceHeaderObj.WKF_COMMENTS = wkfDetails.Comments;
            invoiceHeaderObj.WKF_TRX_FLAG = workflowFlag;
            invoiceHeaderObj.WKF_PROCESS = wkfDetails.ProcessID;
            invoiceHeaderObj.WKF_REFERENCE = wkfDetails.ReferenceID;
            invoiceHeaderObj.WKF_TASK = wkfDetails.TaskID;
            invoiceHeaderObj.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            if (isCancelled)
                invoiceHeaderObj.ATL_ACTION = (byte)LogAction.CANCEL;
            else
                invoiceHeaderObj.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<DirectPOInvoiceHeader>(invoiceHeaderObj);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
            string invoiceNumber = string.Empty;
            result = BusinessLogic.POInvoicing.POInvoiceBL.SavePOInvoiceTradingWkf(xmlDoc, out invoiceNumber);
            if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
            {
                #region File Upload
                if (workflowFlag == (int)(WorkflowTransactionFlag.SAVEANDSUBMIT))
                {
                    #region File Uploads
                    if (POUploadList != null && POUploadList.Count > 0)
                    {
                        savePath = string.Empty;
                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                        {
                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                            if (!Directory.Exists(savePath))
                                Directory.CreateDirectory(savePath);
                            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                        }
                        else
                        {
                            savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                        }

                        foreach (DirectPOInvoiceUploads obj in POUploadList)
                        {
                            string filePath = savePath + obj.AttachmentFileName;
                            FileInfo attachedFileInfo = new FileInfo(filePath);
                            if (FileDetailsList != null)
                            {
                                FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                if (fileDetailsObj != null)
                                {
                                    fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                }
                            }
                        }
                    }                   

                    #endregion
                }
                #endregion
                if (result.HasValue && result.Value > 0)
                {                 
                    //Show Save success message and reset Contract Entry
                    if (isCancelled)
                    {
                        FillProcessID(1);
                        litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;                       
                    }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();                       
                    }
                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                    TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                    WrkfComments.Text = "";
                    if (string.IsNullOrEmpty(invoiceNumber))
                        invoiceNumber = lblInvoiceNo.Text.Trim();
                    object[] args = new object[2];
                    args[0] = Resources.PageNameRes.AdvanceInvoice;
                    args[1] = invoiceNumber;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICELIST);
                        SetFieldValues(ControlsEnum.INVOICELIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);                       
                    }
                    #endregion
                }
                ucrWrkf.ApplicationID = result.Value;
            }
            else
            {
                #region Error/Validation Message Region
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.ErpRes.Information + "');", true);

                }                
                else if (result == -6)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "ClosePopup();$(document).ready(function(){ShowDuplicateVendorInvNoContinue(2);});", true);
                }
                else if (result == -9)//need checking for payment created or not
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_PaymentCreated").ToString()) + "','" + Resources.Messages.Information + "');", true);//Payment created against this invoice.
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                }
                #endregion
                return;
            }
        }
        #endregion
        #endregion

        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            INVOICEHDR,
            INVOICEHDRBYPK,
            POINVOICELIST,
            POINVOICEDETAILS,
            FINANCEINVOICEHDR,
            FINANCEINVOICETRXMPG,
            PICKFORPAYMENT,
            PICKFORCRDRNOTE,
            INVOICENO,
            EXCHANGERATE,
            JOURNALIZE,
            INVOICE,
            WRKFSUBMIT,
            FINHEADER,
            FINPERIOD,
            GETINVOICEPKBYJOURNALPK,
            FILLWORKFLOWSTATUS,
            TAXCALCULATIONSETTINGS,
            POSTINGSETTINGS,
            NOTIFICATIONTYPES,
            ALERTBASIS,
            ALERTTYPES,
            NOTIFICATIONDAYS,
            ALERTCONFIG,
            ALERTLIST,
            ALERTSAVE,
            COMPANY,
            VENDORBRANCH,
            ADDITEM,
            UPLOADEDFILES,
            SELECTEDDOC,
            VENDORCONTACTYPE,
            VENDORCONTACTYPEDETAILS,
            AMOUNTDETAILS,
            POTAXDETAILS,
            COMPANYSRCH,
            INVOICELIST,
            PENDINGPOLIST,
            POINVHEADER,
            POINVDETAIL,
            UPDATEGRIDVALTOOBJECT

        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }

        /// <summary>
        /// Attachment Module Enum
        /// </summary>
        public enum DocModuleEnum
        {
            PURCHASE = 1

        }
        /// <summary>
        /// Attachment Task Enum
        /// </summary>
        public enum DocTaskEnum
        {
            PURCHASEINVOICE = 9,
            EXPENSEINVOICE = 10

        }
        #region VendorContactTypes
        public enum VendorContactTypeEnum
        {
            HeadOffice = 4,
            Branch = 5
        }
        #endregion

        #endregion
        #region Helper Method
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.TPI, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }
        /// <summary>
        /// Get Multiple PO Invoice Details
        /// </summary>      
        /// <returns></returns>
        public static MultiplePOInvoiceHeader GetMultiplePOInvoiceHeader(int poPK, int invPK, string strxml)
        {
            try
            {
                MultiplePOInvoiceHeader invoiceHeaderObj = new MultiplePOInvoiceHeader();
                string invoice = POInvoiceDL.GetMultiplePOInvoiceHeader(poPK, invPK, strxml);
                if (invoice != string.Empty)
                {
                    invoiceHeaderObj = (MultiplePOInvoiceHeader)CommonFunctions.DeserializeObject(invoice, invoiceHeaderObj);
                    return invoiceHeaderObj;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }
      
        #endregion
    }
}