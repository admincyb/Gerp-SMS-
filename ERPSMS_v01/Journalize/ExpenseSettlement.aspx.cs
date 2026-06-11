using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using BusinessObject.POInvoicing;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;
using System.IO;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.Journalize
{
    public partial class ExpenseSettlement : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        private int SelectedItemPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedItemPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedItemPK] = value;
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

        private List<BusinessObject.POInvoicing.POInvoiceUploads> POUploadList
        {
            get
            {
                return ViewState[ViewstateStrings.POUploadList] == null ? null : (List<BusinessObject.POInvoicing.POInvoiceUploads>)ViewState[ViewstateStrings.POUploadList];
            }
            set
            {
                ViewState[ViewstateStrings.POUploadList] = value;
            }
        }

        private int SelectedDtlPK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedDtlPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedDtlPK] = value;
            }
        }
        /// <summary>
        /// To maintain keep PoHeader List
        /// </summary>
        private List<PUR_ORDER_HDR> PoHeaderList
        {
            get
            {
                return (List<PUR_ORDER_HDR>)Session[ERP.Utilities.SessionStrings.PoHeaderList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PoHeaderList] = value;
            }

        }

        /// <summary>
        /// To Disable Item Tax
        /// </summary>
        private bool EnableItemTax
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemTax] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.DisableItemTax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemTax] = value;
            }
        }
        /// <summary>
        /// To Disable Item Discount
        /// </summary>
        private bool EnableItemDiscount
        {
            get
            {
                return this.ViewState[ViewstateStrings.DisableItemDiscount] == null ? true : Convert.ToBoolean(this.ViewState[ViewstateStrings.DisableItemDiscount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.DisableItemDiscount] = value;
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
        /// Current Sl No.
        /// </summary>
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
        /// To maintain keep Expense Tax Splitting
        /// </summary>
        private POInvoiceHeader ExpenseHeaderSession
        {
            get
            {
                return (POInvoiceHeader)Session[ERP.Utilities.SessionStrings.ExpenseHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.ExpenseHeaderSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep Expense Header Tax Splitting
        /// </summary>
        private POInvoiceHeader TempExpenseHeaderSession
        {
            get
            {
                return (POInvoiceHeader)this.ViewState[ERP.Utilities.SessionStrings.TempExpenseHeaderSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempExpenseHeaderSession] = value;
            }
        }
        /// <summary>
        /// To maintain keep Expense Header Tax Splitting
        /// </summary>
        private POInvoiceHeader EditTempExpenseHeaderSession
        {
            get
            {
                return (POInvoiceHeader)this.ViewState[ERP.Utilities.ViewstateStrings.EditTempExpenseHeaderSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.ViewstateStrings.EditTempExpenseHeaderSession] = value;
            }
        }
        /// <summary>
        /// Expense PK
        /// </summary>
        private int ExpensePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.ExpensePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ExpensePK] = value;
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
        private bool IsPartyNo
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsPartyNo] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsPartyNo]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsPartyNo] = value;
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
        /// Approved
        /// </summary>
        private int Group
        {
            get
            {
                return Convert.ToInt16(this.ViewState[ViewstateStrings.Group]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Group] = value;
            }
        }

        /// <summary>
        /// Expense
        /// </summary>
        private string Expense
        {
            get
            {
                return (string)this.ViewState[ViewstateStrings.Expense];
            }
            set
            {
                this.ViewState[ViewstateStrings.Expense] = value;
            }
        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedInvoices
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoices];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedInvoices] = value;
            }

        }

        /// <summary>
        /// To maintain keep selected Currency
        /// </summary>
        private long SelectedCurrency
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedCurrency]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrency] = value;
            }
        }

        private long SelectedVendors
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedVendors]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedVendors] = value;
            }
        }
        private List<ADM_CONFIG_MST> TempConfigMstDetails
        {
            get
            {
                return Session[ERP.Utilities.SessionStrings.TempConfigMstDetails] == null ? new List<ADM_CONFIG_MST>()
                    : (List<ADM_CONFIG_MST>)Session[ERP.Utilities.SessionStrings.TempConfigMstDetails];
            }
            set
            {
                if (value == null)
                    Session.Remove(ERP.Utilities.SessionStrings.TempConfigMstDetails);
                else
                    Session[ERP.Utilities.SessionStrings.TempConfigMstDetails] = value;
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
        /// To maintain keep All Information(Vendor,Currency etc) of InvoiceList (Multiple Paging)
        /// </summary>
        private List<SelectionInfo> SelectedInvoicesInfoLst
        {
            get
            {
                return (List<SelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] = value;
            }

        }

        /// <summary>
        /// To maintain keep selected invoices
        /// </summary>
        private List<long> SelectedInvoicesCrDr
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = value;
            }
        }

        /// <summary>
        /// To show or hide credit and debit allocation columns for expense invoice
        /// </summary>
        private bool ShowExpenseCrDr
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowExpenseCrDr] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.ShowExpenseCrDr]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowExpenseCrDr] = value;
            }
        }

        private decimal SettlementTotal
        {
            get
            {
                return this.ViewState["SettlementTotal"] == null ? 0 : Convert.ToDecimal(this.ViewState["SettlementTotal"]);
            }
            set
            {
                this.ViewState["SettlementTotal"] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsInvoiceGSTEnable
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsInvoiceGSTEnable] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsInvoiceGSTEnable].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsInvoiceGSTEnable] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private byte selectedModeValue = 0;
        private long InvoicePk = 0;
        private bool isCancelled = false;
        User currentUser;
        //page related Entity Object
        private POInvoiceHeader expenseHeaderObj;
        private POInvoiceDetails soExpenseDetailsObj;
        private POInvoiceTaxHdr soInvTaxHdrObj;
        List<POAdvDeductionDetails> deductionDtlList;
        List<POInvoiceDetails> soExpenseDetailsList;
        List<POInvoiceTaxHdr> taxHdrList;
        POInvoiceDetails soDtlObj;
        string selectedVendor;
        DataSet dsInvHeader;
        DataTable dtTaxDetails;
        DataTable dtInvoiceGstType;

        private DataSet dsAdsType;

        DataTable dtAdsType;
        DataTable dtAdsTypeDtl;
        DataTable dtExpenseAdvanced;
        bool hasValidRate;
        POInvoiceUploads poUploadObj;
        private POInvoiceHeader invoiceHeaderObj;
        string savePath = string.Empty;

        int JournalPK;

        DataSet dsPageData;
        private DataTable dtCurrency;
        private DataTable dtExpenseList;
        private DataTable dtPageData;
        private DataTable dtCustomTaxSet;
        private DataTable dtAmountDetails;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;

        private string refID;
        private string inboxFlag;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> workflowStatusList;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private DataTable dtCompany;



        private FIN_INVOICE_VND_HDR finExpenseVndHdrObj;

        private string[] _operators = { "-", "+", "/", "*", "^" };
        private Func<double, double, double>[] _operations = {
        (a1, a2) => a1 - a2,
        (a1, a2) => a1 + a2,
        (a1, a2) => a1 / a2,
        (a1, a2) => a1 * a2,
        (a1, a2) => Math.Pow(a1, a2)
    };

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
                    if (hdfIsShowAlert.Value == "0")
                        btnAlert.Visible = false;
                    GetFieldValues(ControlsEnum.BASECURRENCY);
                    SetFieldValues(ControlsEnum.BASECURRENCY);
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANYSRCH);

                    GetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                    SetFieldValues(ControlsEnum.INVOICEGSTTYPE);

                    if (IsInvoiceGSTEnable == true)
                    {
                        lblInvoiceGstType.Visible = true;
                        ddlInvoiceGstType.Visible = true;
                    }
                    else
                    {
                        lblInvoiceGstType.Visible = false;
                        ddlInvoiceGstType.Visible = false;
                    }

                    //Enable or disable custom tax 
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = "IVH_PK";
                    grdExpenseList.DataKeyNames = datakeyarray;

                    string[] itemkeyarrayUpload;
                    itemkeyarrayUpload = new string[1];
                    itemkeyarrayUpload[0] = "DOC_SEQ_NO";
                    grdUploads.DataKeyNames = itemkeyarrayUpload;

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "VID_SL_NO";
                    grdItemDetails.DataKeyNames = itemkeyarray;

                    // hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperation.Value = "#" + currencysep + "#0.";

                    hdfJournalizeWorkFlow.Value = "0";
                    hdfDecimalFormat.Value = "#0.";
                    int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                    for (int i = 0; i < NoDecimalDigitsP2P; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperation.Value += "0";
                    }
                    hdfNumberDigits.Value = NoDecimalDigitsP2P.ToString();
                    hdfCurrencyFormat.Value = "#0.";
                    hdfCurrencyFormatWithSeperation.Value = "#" + currencysep + "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithSeperation.Value += "0";
                    }
                    hdfRateFormat.Value = "#0.";
                    int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                    for (int i = 0; i < rateDecimalDigits; i++)
                    {
                        hdfRateFormat.Value += "0";
                    }

                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    SelectedInvoices = null;
                    SelectedInvoicesInfoLst = null;

                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    BindDefaultUOM();

                    AST_DOC_MODE.Value = "0";
                    ExpenseHeaderSession = new POInvoiceHeader();

                    EnableItemDiscount = true;
                    EnableItemTax = true;
                    lblItemDiscount.Visible = txtDiscount.Visible = imgDiscount.Visible = EnableItemDiscount;
                    lblItemTax.Visible = txtTax.Visible = imgTax.Visible = EnableItemTax;

                    ////start
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
                    //If Request From External(Report or Other page) other than Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        GetFieldValues(ControlsEnum.INVOICEGET);
                        SetFieldValues(ControlsEnum.INVOICEGET);
                    }
                    else
                    {
                        #region else region
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
                                    hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                            }
                            else if (pid.Equals("2") || pid.Equals("12"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETEXPENSEPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                        }
                        if (CurrPK > 0)
                        {
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }

                            GetFieldValues(ControlsEnum.EXPENSEHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempExpenseHeaderSession = ExpenseHeaderSession;
                        }
                        else
                        {
                            txtExpenseDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                            TempExpenseHeaderSession = new POInvoiceHeader();
                            EditTempExpenseHeaderSession = new POInvoiceHeader();
                            ExpenseHeaderSession = new POInvoiceHeader();

                            GetFieldValues(ControlsEnum.EXPENSELIST);
                            SetFieldValues(ControlsEnum.EXPENSELIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        AST_DOC_MODE.Value = GetDOCMODE();
                        AST_CODE.Value = ApplicationType.ES;
                        lblExpenseNo.Text = hdfExpenseNo.Value == string.Empty ? "[NEW]" : hdfExpenseNo.Value;

                        hdfAppType.Value = ApplicationType.ES;
                        hdfAppSubType.Value = string.Empty;
                        #endregion
                    }
                }
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Configuration Settings
        /// </summary>
        private void ConfigurationSettings()
        {
            ShowExpenseCrDr = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "CNDNFromExpenseInv")));
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            IsInvoiceGSTEnable = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableGST")));
            hdfIsShowAlert.Value = GetGlobalResourceObject("ConfigurationsRes", "IsShowAlertInPO").ToString();//For Setting Visibilty of Alert Button w. r. to client
            hdfInvDueDateDependsVenInvDate.Value = GetGlobalResourceObject("ConfigurationsRes", "InvDueDateDependsVendorInvDate").ToString();   //1=>Inv. Due Date calculation depends upon Vendor Inv. Date,Otherwise Invoice date
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
            DataSet dsTaxDetails;

            FinTrxService finTrxServiceClient;
            ServiceUtility serviceUtilityObj;
            AdmCompanyMstService admCompanyMstServiceClient;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                    case ControlsEnum.BASECURRENCY:
                        dtCurrency = CommonBL.GetCurrency(currentUser.BaseCurrency, currentUser.SBUID, Convert.ToInt16(DbActiveStatus.HASPK));
                        break;
                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:
                        int VatVendorPopupPk;
                        int.TryParse(hdfVendorDtl.Value, out VatVendorPopupPk);
                        if (VatVendorPopupPk > 0)
                            dsAdsType = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.ACTIVE), 0, VatVendorPopupPk, 0);
                        if (dsAdsType != null && dsAdsType.Tables.Count > 0 && dsAdsType.Tables[0].Rows.Count > 0)
                            dtAdsType = dsAdsType.Tables[0];
                        break;
                    #endregion
                    case ControlsEnum.EXPENSEADVANCES:
                        int venPK = 0;
                        Int32.TryParse(hdfVendor.Value, out venPK);
                        dtExpenseAdvanced = BusinessLogic.POInvoicing.POInvoiceBL.GetExpenseAdvances(venPK, CurrPK);
                        break;

                    #region VENDORDETAILS
                    case ControlsEnum.VENDORDETAILS:
                        dtPageData = BusinessLogic.VendorManagement.VendorRegistration.GetVendorData(Convert.ToInt32(hdfVendor.Value));
                        break;
                    #endregion
                    #region VENDORSELECTEDDTL
                    case ControlsEnum.VENDORSELECTEDDTL:

                        if (string.IsNullOrEmpty(hdfVendorDtl.Value))
                        {
                            dtPageData = BusinessLogic.VendorManagement.VendorMaster.GetVendor(currentUser, 0, Convert.ToInt16(DbActiveStatus.ACTIVE), txtVendorDtl.Text);
                        }


                        //dtPageData = BusinessLogic.VendorManagement.VendorMaster.GetVendor(Convert.ToInt32(hdfVendorDtl.Value));
                        break;
                    #endregion


                    #region EXPENSEHEADER
                    case ControlsEnum.EXPENSEHEADER:
                        expenseHeaderObj = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceHeader(0, CurrPK);
                        ExpenseHeaderSession = expenseHeaderObj;
                        if (expenseHeaderObj == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
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
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtExpenseDate.Text), 0, null, (int)TaxStatus.Include, 1);
                            }
                            else
                            {
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtExpenseDate.Text), 0, null, (int)TaxStatus.Include);
                            }
                        }
                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILS
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        {
                            int VncPk;
                            int venPk;
                            venPk = 0;
                            venPk = hdfVendorDtl.Value == "" ? (hdfVendor.Value == "" ? 0 : Convert.ToInt16(hdfVendor.Value)) : Convert.ToInt16(hdfVendorDtl.Value);
                            int.TryParse(hdfAddressType.Value, out VncPk);
                            if (venPk > 0)
                                dtAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, venPk, 0).Tables[0];
                            else
                                dtAdsTypeDtl = null;
                        }

                        //  int.TryParse(hdfAddressType.Value, out VncPk);
                        //int.TryParse(hdfVendorPopup.Value, out VatVendorPopupPk);
                        //if (VatVendorPopupPk > 0 && VncPk > 0)
                        //    dsAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, VatVendorPopupPk, 0);



                        break;
                    #endregion
                    #region VENDORCONTACTYPEDETAILSHDR
                    case ControlsEnum.VENDORCONTACTYPEDETAILSHDR:
                        {
                            int VncPk;
                            int venPk;
                            venPk = 0;
                            venPk = string.IsNullOrEmpty(hdfVendor.Value) ? 0 : Convert.ToInt16(hdfVendor.Value);
                            int.TryParse(hdfAddTypeHdr.Value, out VncPk);
                            if (venPk > 0)
                                dtAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, venPk, 0).Tables[0];
                            else
                                dtAdsTypeDtl = null;
                        }

                        //  int.TryParse(hdfAddressType.Value, out VncPk);
                        //int.TryParse(hdfVendorPopup.Value, out VatVendorPopupPk);
                        //if (VatVendorPopupPk > 0 && VncPk > 0)
                        //    dsAdsTypeDtl = BusinessLogic.POInvoicing.POInvoiceBL.GetVendorAddressTypes(currentUser, Convert.ToInt16(DbActiveStatus.HASPK), VncPk, VatVendorPopupPk, 0);



                        break;
                    #endregion

                    #region EXCHANGERATE
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtExpenseDate.Text.Trim()));
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
                    #region INVOICEGET
                    case ControlsEnum.INVOICEGET:
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        int statusfilter = 3;
                        dsPageData = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? "IVH_DATE" : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = SortBy == ThenBy || SortBy == "IVH_NO" ? string.Empty : string.IsNullOrEmpty(ThenBy) ? "IVH_NO" : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == "IVH_NO" ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                SearchBy = "IVH_PK",
                                SearchValue = GInvPk.ToString()
                            }, currentUser, 0, 0, 0, string.Empty, string.Empty, Resources.PageURL.PurchaseOrderInvoicing.Replace("~", ""), 0
                            , statusfilter, group: (byte)POInvoiceGroup.ExpenseSettilement, category: 0);

                        //, (ddlStatus.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlStatus.SelectedValue) : 0), group: (byte)POInvoiceGroup.Expense, category: 0);
                        if (dsPageData != null)
                        {
                            //string customer = string.IsNullOrEmpty(txtVendorSch.Text.Trim()) ? string.Empty : (txtVendorSch.Text.Trim() == "Select/Type" ? string.Empty : txtVendorSch.Text.Trim());
                            DataView dvExpense = dsPageData.Tables[1].DefaultView;
                            //if (customer != string.Empty)
                            //{
                            //    dvExpense.RowFilter = " IVH_VENDOR_TEXT='" + customer + "'";
                            //}
                            dtExpenseList = dvExpense.ToTable();
                        }

                        break;
                    #endregion
                    #region EXPENSELIST
                    case ControlsEnum.EXPENSELIST:
                        int cusID = String.IsNullOrEmpty(hdfVendorSch.Value.Trim()) ? 0 : Convert.ToInt32(hdfVendorSch.Value);
                        if (hdfVendorSch.Value != "" && hdfVendorSch.Value != "0")
                        {
                            Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorSch.Value;
                            Session[ERP.Utilities.SessionStrings.Vendor] = txtVendorSch.Text;
                        }
                        int InvPk = String.IsNullOrEmpty(hdfExpenseNumberPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfExpenseNumberPK.Value);
                        int CompanyPK = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        string customer = string.IsNullOrEmpty(txtVendorSch.Text.Trim()) ? string.Empty : (txtVendorSch.Text.Trim() == "Select/Type" ? string.Empty : txtVendorSch.Text.Trim());
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        dsPageData = BusinessLogic.POInvoicing.POInvoiceBL.GetPOInvoiceList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? "IVH_DATE" : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = SortBy == ThenBy || SortBy == "IVH_NO" ? string.Empty : string.IsNullOrEmpty(ThenBy) ? "IVH_NO" : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == "IVH_NO" ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                SearchBy = "IVH_NO",
                                SearchValue = string.IsNullOrEmpty(txtExpenseNumber.Text.Trim()) ? string.Empty : (txtExpenseNumber.Text.Trim() == "Select/Type" ? string.Empty : txtExpenseNumber.Text.Trim()),
                                //CompanyPK = cmpPk
                            }, currentUser, cusID, InvPk, 0, customer, string.Empty, Resources.PageURL.PurchaseOrderInvoicing.Replace("~", ""), 0
                            , Convert.ToInt32(ddlStatus.SelectedValue), group: (byte)POInvoiceGroup.ExpenseSettilement, category: 0, pending: null, grnNo: null, due: null, cmpPk: CompanyPK);

                        //, (ddlStatus.SelectedValue != CommonConstants.SELECTVAL ? Convert.ToInt32(ddlStatus.SelectedValue) : 0), group: (byte)POInvoiceGroup.Expense, category: 0);
                        if (dsPageData != null)
                        {
                            //string customer = string.IsNullOrEmpty(txtVendorSch.Text.Trim()) ? string.Empty : (txtVendorSch.Text.Trim() == "Select/Type" ? string.Empty : txtVendorSch.Text.Trim());
                            DataView dvExpense = dsPageData.Tables[1].DefaultView;
                            //if (customer != string.Empty)
                            //{
                            //    dvExpense.RowFilter = " IVH_VENDOR_TEXT='" + customer + "'";
                            //}
                            dtExpenseList = dvExpense.ToTable();
                        }

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
                        //To get the company related to current SBU
                        dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
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
                    case ControlsEnum.GETEXPENSEPKBYJOURNALPK:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        finTrxHdrObj.FTH_PK = JournalPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetfinTxtHdrList(finTrxHdrObj, serviceUtilityObj);
                        break;
                    case ControlsEnum.TAXSETTINGS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ItemWiseTaxSetting, string.Empty, currentUser.SBUID);
                        break;
                    case ControlsEnum.DEFAULTUOM:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ExpenseInvUOM, string.Empty, currentUser.SBUID);
                        break;

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

                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        dtAmountDetails = new DataTable();
                        dtAmountDetails = BusinessLogic.POInvoicing.POInvoiceBL.GetBalanceAmountDetails(InvoicePk);
                        break;
                    #endregion
                    #region Invoice Type GST Dropdown
                    case ControlsEnum.INVOICEGSTTYPE:
                        dtInvoiceGstType = BusinessLogic.CommonManagement.CommonBL.GetInvoiceGstType(Convert.ToInt16(CommonConstants.SELECT_VALUE_ZERO), Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt16(CommonConstants.SELECTVAL), Convert.ToInt16(GTIService.Constants.Common.InvoiceType.Purchase));
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
                finTrxServiceClient = null;
                admCompanyMstServiceClient = null;
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
                    case ControlsEnum.BASECURRENCY:
                        if (dtCurrency != null && dtCurrency.Rows.Count > 0)
                        {
                            txtCurrency.Text = dtCurrency.Rows[0]["CUR_CODE"].ToString();
                            hdfCurrency.Value = dtCurrency.Rows[0]["CUR_PK"].ToString();
                        }
                        break;
                    case ControlsEnum.EXPENSEADVANCES:
                        BindGrid(ControlsEnum.EXPENSEADVANCES);
                        break;
                    case ControlsEnum.INVOICEGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.EXPENSEHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;

                    case ControlsEnum.EXPENSEDETAIL:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.TAXTYPES:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.EXPENSELIST:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    case ControlsEnum.COMPANYSRCH:
                        BindDropDown(ControlsEnum.COMPANYSRCH);
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        if (expenseHeaderObj != null)
                            GetUIValuesFromObject(ControlsEnum.UPLOADEDFILES);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    case ControlsEnum.VENDORSELECTEDDTL:
                        GetUIValuesFromObject(ControlsEnum.VENDORSELECTEDDTL);
                        break;
                    case ControlsEnum.VENDORCONTACTYPE:
                        GetUIValuesFromObject(ControlsEnum.VENDORCONTACTYPE);
                        break;
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.INVOICEGSTTYPE:
                        BindDropDown(controlType);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Helper Methods
        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.EI, 0, DateTime.Now);
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
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            bool bIsChecked = false;

            int selectedExpense;
            int selectedCurrency;
            int selectedVendor;
            int approvedStatus;
            int selectedInvoice = 0;
            bool isPosted;

            selectedExpense = 0;
            approvedStatus = 0;
            selectedVendor = 0;
            selectedCurrency = 0;
            isPosted = false;
            bool InvalidPaymentItem = false;

            try
            {
                switch (controlType)
                {
                    #region Expense
                    case ControlsEnum.EXPENSEHEADER:
                        if (ExpenseHeaderSession != null)
                        {
                            expenseHeaderObj = ExpenseHeaderSession;
                            expenseHeaderObj.IVH_PK = CurrPK;
                            expenseHeaderObj.IVH_VERSION = 1;
                            expenseHeaderObj.IVH_STATUS = 0;
                            expenseHeaderObj.IVH_GROUP = (byte)POInvoiceGroup.ExpenseSettilement;
                            expenseHeaderObj.IVH_CATEGORY = (byte)POInvoiceCategory.Invoice;
                            expenseHeaderObj.IVH_AMOUNT_SET = string.IsNullOrEmpty(txtAmountToSettle.Text) ? 0 : double.Parse(txtAmountToSettle.Text);
                            //  PoHeaderList
                            expenseHeaderObj.IVH_NO = expenseHeaderObj.IVH_PK.ToString();
                            expenseHeaderObj.IVH_DATE = string.IsNullOrEmpty(txtExpenseDate.Text.Trim()) ?
                                DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtExpenseDate.Text.Trim();
                            expenseHeaderObj.IVH_VENDOR = HttpUtility.HtmlDecode(hdfVendor.Value);
                            expenseHeaderObj.IVH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                            if (!string.IsNullOrEmpty(hdfVendorInvType.Value))
                                expenseHeaderObj.IVH_TYPE = hdfVendorInvType.Value;

                            expenseHeaderObj.IVH_VENDOR_INV_NO = HttpUtility.HtmlDecode(txtVendorInvNO.Text);
                            if (!string.IsNullOrEmpty(txtVendorInvDate.Text))
                                expenseHeaderObj.IVH_VENDOR_INV_DATE = txtVendorInvDate.Text;
                            else
                                expenseHeaderObj.IVH_VENDOR_INV_DATE = string.Empty;
                            expenseHeaderObj.IVH_CREDIT_DAYS = txtCreditDays.Text;
                            expenseHeaderObj.IVH_DATE_PAY_BY = string.IsNullOrEmpty(txtInvoiceDueDate.Text.Trim()) ?
                                DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDueDate.Text.Trim();
                            expenseHeaderObj.IVH_ORGINAL_RCVD = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                            expenseHeaderObj.IVH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                expenseHeaderObj.IVH_COMPANY = Convert.ToInt16(ddlCompanyView.SelectedValue);
                            }
                            else
                            {
                                expenseHeaderObj.IVH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                            }

                            expenseHeaderObj.IVH_DISCOUNT_TC = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                            expenseHeaderObj.IVH_TAX_TC = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                            expenseHeaderObj.IVH_AMOUNT_ADJUST = string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim());
                            expenseHeaderObj.IVH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());
                            expenseHeaderObj.IVH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                            //  GetFieldValues(ControlsEnum.EXCHANGERATE);
                            expenseHeaderObj.IVH_EXCHG_RATE = 1;// string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                            expenseHeaderObj.IVH_AMOUNT_NET_BC = expenseHeaderObj.IVH_AMOUNT_NET_TC * expenseHeaderObj.IVH_EXCHG_RATE;

                            expenseHeaderObj.IVH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            expenseHeaderObj.IVH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            expenseHeaderObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            expenseHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            expenseHeaderObj.LAST_MOD_DT = LastModifiedTime;


                            expenseHeaderObj.IVH_TOTAL_QTY = 0;//Dummy
                            expenseHeaderObj.APT_CODE = ApplicationType.ES;
                            expenseHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;

                            expenseHeaderObj.IVH_VENDOR_CONTACT = hdfAddTypeHdr.Value;
                            expenseHeaderObj.IVH_BRANCH_TEXT = txtHdrBranchCode.Text;
                            expenseHeaderObj.IVH_TAX_ID = txtHdrTaxId.Text;
                            expenseHeaderObj.IVH_BRANCH_TYPE = HdrchkHO.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                            expenseHeaderObj.IVH_IS_OPENING = 0;
                            if (ddlInvoiceGstType.Items.Count > 0 && Convert.ToInt32(ddlInvoiceGstType.SelectedValue) > 0)
                                expenseHeaderObj.IVH_GST_TYPE = ddlInvoiceGstType.SelectedValue;
                            //Uploads
                            expenseHeaderObj.FileList = POUploadList;
                            expenseHeaderObj.IVH_AMOUNT_REFUND = Convert.ToDecimal(txtRefundAmount.Text.Trim());
                            //Expense Settilement
                            SettlementTotal = 0;
                            List<ExpenseSettilements> lstSettlements = new List<ExpenseSettilements>();
                            foreach (GridViewRow grdrow in grdExpenseAdv.Rows)
                            {
                                ExpenseSettilements objItem = new ExpenseSettilements();
                                if (((CheckBox)grdrow.FindControl("chkSelect")).Checked)
                                {
                                    objItem.IED_PK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfIedPK")).Value);
                                    objItem.IED_VOUCHER_HDR = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfVoucherHdr")).Value);
                                    objItem.IED_AMOUNT = Convert.ToDecimal(((Label)grdrow.FindControl("lblAmount")).Text);
                                    lstSettlements.Add(objItem);
                                    SettlementTotal += objItem.IED_AMOUNT;
                                }
                            }
                            expenseHeaderObj.ExpSettlements = lstSettlements;

                        }
                        retObject = expenseHeaderObj;
                        break;
                    #endregion
                    #region Items
                    case ControlsEnum.EXPENSEDETAIL:
                        if (CurrSlNo != 0)
                        {
                            if (soExpenseDetailsList != null)
                            {
                                soExpenseDetailsObj = soExpenseDetailsList.SingleOrDefault(itm => itm.VID_SL_NO == CurrSlNo);


                                soExpenseDetailsObj.VID_PK = Convert.ToInt32(hdfDetailPK.Value);
                                soExpenseDetailsObj.VID_INSTRUCTIONS = HttpUtility.HtmlEncode(txtDesc.Text);
                                soExpenseDetailsObj.VID_QTY_INVOICED = Math.Round(Convert.ToDouble(txtQty.Text), Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()));
                                soExpenseDetailsObj.VID_UOM = Convert.ToInt32(hdfUOM.Value);
                                soExpenseDetailsObj.VID_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                                soExpenseDetailsObj.VID_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                                soExpenseDetailsObj.VID_DISCOUNT = !string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                soExpenseDetailsObj.VID_AMOUNT = !string.IsNullOrEmpty(txtAmount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                soExpenseDetailsObj.VID_TAX = !string.IsNullOrEmpty(txtTax.Text.Trim()) ? Math.Round(Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                soExpenseDetailsObj.VID_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);
                                soExpenseDetailsObj.VID_REF_NO = HttpUtility.HtmlDecode(txtPartyNo.Text);
                                //soExpenseDetailsObj.VID_REF_DATE = string.IsNullOrEmpty(txtPartyInvDate.Text) ? string.Empty : txtPartyInvDate.Text;
                                if (!string.IsNullOrEmpty(txtPartyNo.Text))//If the Party Inv. No. is empty no need to save Party Inv. Dt. also (Bug ID:  41106)
                                {
                                    if (!string.IsNullOrEmpty(txtPartyInvDate.Text))
                                        soExpenseDetailsObj.VID_REF_DATE = txtPartyInvDate.Text;
                                }
                                else
                                {
                                    soExpenseDetailsObj.VID_REF_DATE = null;
                                }

                                soExpenseDetailsObj.VID_VENDOR_TEXT = HttpUtility.HtmlEncode(txtVendorDtl.Text);
                                soExpenseDetailsObj.VID_VENDOR = string.IsNullOrEmpty(hdfVendorDtl.Value) ? 0 : Convert.ToInt32(hdfVendorDtl.Value);
                                soExpenseDetailsObj.VID_BRANCH_TYPE = chkHO.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;

                                soExpenseDetailsObj.VID_BRANCH = string.IsNullOrEmpty(hdfAddressType.Value) ? "" : Convert.ToInt32(hdfAddressType.Value) > 0 ? hdfAddressType.Value : "";
                                soExpenseDetailsObj.VID_BRANCH_NAME = HttpUtility.HtmlEncode(txtAddressType.Text);

                                soExpenseDetailsObj.VID_TAX_ID = string.IsNullOrEmpty(txtVatTaxId.Text) ? "" : HttpUtility.HtmlEncode(txtVatTaxId.Text);
                                soExpenseDetailsObj.VID_BRANCH_TEXT = string.IsNullOrEmpty(txtBranchCode.Text) ? "" : HttpUtility.HtmlEncode(txtBranchCode.Text);

                                soExpenseDetailsObj.VID_REFUND_DATE = GetDateFromMonth();

                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (soExpenseDetailsList == null || soExpenseDetailsList.Count == 0)
                            {
                                soExpenseDetailsList = new List<POInvoiceDetails>();
                                slno = 1;
                            }
                            else
                            {
                                slno = soExpenseDetailsList.Max(itm => itm.VID_SL_NO);
                                slno++;
                            }
                            soExpenseDetailsObj = new POInvoiceDetails();
                            CurrSlNo = soExpenseDetailsObj.VID_SL_NO = slno;
                            soExpenseDetailsObj.VID_INSTRUCTIONS = HttpUtility.HtmlEncode(txtDesc.Text);
                            soExpenseDetailsObj.VID_QTY_INVOICED = Math.Round(Convert.ToDouble(txtQty.Text), Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()));
                            soExpenseDetailsObj.VID_UOM = Convert.ToInt32(hdfUOM.Value);
                            soExpenseDetailsObj.VID_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                            soExpenseDetailsObj.VID_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                            soExpenseDetailsObj.VID_DISCOUNT = Math.Round(Convert.ToDouble(string.IsNullOrEmpty(txtDiscount.Text) ? CommonConstants.SELECT_VALUE_ZERO : txtDiscount.Text)
                                , Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            soExpenseDetailsObj.VID_AMOUNT = Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            soExpenseDetailsObj.VID_TAX = Math.Round(Convert.ToDouble(string.IsNullOrEmpty(txtTax.Text) ? CommonConstants.SELECT_VALUE_ZERO : txtTax.Text)
                                , Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            soExpenseDetailsObj.VID_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);
                            soExpenseDetailsObj.VID_REF_NO = string.IsNullOrEmpty(txtPartyNo.Text) ? null : HttpUtility.HtmlDecode(txtPartyNo.Text);
                            if (!string.IsNullOrEmpty(txtPartyNo.Text))//If the Party Inv. No. is empty no need to save Party Inv. Dt. also (Bug ID:  41106)
                            {
                                if (!string.IsNullOrEmpty(txtPartyInvDate.Text))
                                    soExpenseDetailsObj.VID_REF_DATE = txtPartyInvDate.Text;
                            }
                            else
                            {
                                soExpenseDetailsObj.VID_REF_DATE = null;
                            }

                            soExpenseDetailsObj.VID_VENDOR_TEXT = HttpUtility.HtmlEncode(txtVendorDtl.Text);
                            soExpenseDetailsObj.VID_VENDOR = string.IsNullOrEmpty(hdfVendorDtl.Value) ? 0 : Convert.ToInt32(hdfVendorDtl.Value);

                            soExpenseDetailsObj.VID_BRANCH_TYPE = chkHO.Checked ? (byte)VendorContactTypeEnum.HeadOffice : (byte)VendorContactTypeEnum.Branch;
                            soExpenseDetailsObj.VID_TAX_ID = string.IsNullOrEmpty(txtVatTaxId.Text) ? "" : HttpUtility.HtmlEncode(txtVatTaxId.Text);
                            soExpenseDetailsObj.VID_BRANCH_TEXT = string.IsNullOrEmpty(txtBranchCode.Text) ? "" : HttpUtility.HtmlEncode(txtBranchCode.Text);
                            soExpenseDetailsObj.VID_BRANCH = string.IsNullOrEmpty(hdfAddressType.Value) ? "" : Convert.ToInt32(hdfAddressType.Value) > 0 ? hdfAddressType.Value : "";
                            soExpenseDetailsObj.VID_BRANCH_NAME = HttpUtility.HtmlEncode(txtAddressType.Text);

                            soExpenseDetailsObj.VID_REFUND_DATE = GetDateFromMonth();

                            //soExpenseDetailsObj.VID_REF_DATE = string.IsNullOrEmpty(txtPartyInvDate.Text) ? string.Empty : txtPartyInvDate.Text;
                            soExpenseDetailsList.Add(soExpenseDetailsObj);
                        }
                        retObject = soExpenseDetailsList;
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        ////Start
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            ////
                            foreach (GridViewRow grdrow in grdExpenseList.Rows)
                            {
                                CheckBox chkInvselect;
                                chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                                HiddenField hdfDept;
                                int dept;
                                if (chkInvselect.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExpenseID")).Value);
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);

                                    hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                    if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                    {
                                        Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                        base.SetUserDept();
                                    }
                                    break;
                                }
                            }
                            ////Start
                        }
                        else if (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.VIEWMODE)
                        {
                            bIsChecked = true;
                        }
                        ////
                        if (bIsChecked)
                        {
                            if (Approved == 2)
                            {
                                GetFieldValues(ControlsEnum.EXPENSEHEADER);

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

                                Expense = ApplicationType.ESJ;
                                ucrJournalize.TransactionType = Expense;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = Expense;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = expenseHeaderObj.IVH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = expenseHeaderObj.IVH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = expenseHeaderObj.IVH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = expenseHeaderObj.IVH_VENDOR;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.ESJ;
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
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
                                }
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                    //EntryStatus = EntryStatus.VIEWMODE;
                                    //Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.VIEWMODE;
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
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Expense_Journal").ToString();

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            if (EntryStatus == EntryStatus.NEWMODE)
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                            else
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Pick Expense for Paying
                    case ControlsEnum.PICKFORPAYMENT:
                        #region  For Picking like invoices from different pages
                        SetAllocationDetails();
                        if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] != null)
                        {
                            SelectedInvoicesInfoLst = (List<SelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst];
                            foreach (SelectionInfo item in SelectedInvoicesInfoLst)
                            {
                                if (item.chkChecked)
                                {
                                    if (item.ApprovedStatus == 2 && item.IsPosted)
                                    {
                                        if (SelectedCurrency == 0)
                                            SelectedCurrency = item.CurrencyPK;
                                        if (SelectedVendors == 0)
                                            SelectedVendors = item.VendorPK;
                                        if (SelectedCurrency != item.CurrencyPK)
                                        {
                                            InvalidPaymentItem = true;
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                        else if (SelectedVendors != item.VendorPK)
                                        {
                                            InvalidPaymentItem = true;
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                        //For Saving Selected Item PK
                                        hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + item.InvoicePK.ToString();
                                    }
                                    else
                                    {
                                        if (item.ApprovedStatus != 2)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                                        }
                                        else if (!item.IsPosted)  //Posted == false
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_PickPaymentPost_Msg").ToString();
                                        }
                                        InvalidPaymentItem = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }

                                }
                            }

                            if (!InvalidPaymentItem)
                            {
                                SelectedInvoicesInfoLst = SelectedInvoicesInfoLst.Where(f => f.chkChecked == true).ToList();//Filtering: remove unchecked Items
                                foreach (SelectionInfo item in SelectedInvoicesInfoLst)
                                {
                                    if (SelectedInvoices == null)
                                    {
                                        SelectedInvoices = new List<long>();
                                    }
                                    SelectedInvoices.Add(item.InvoicePK);
                                }
                                if (SelectedInvoices != null && SelectedInvoices.Count > 0)
                                {
                                    btnPickForPayment.Text = GetLocalResourceObject("PickPoForPayment").ToString() + "(" + SelectedInvoices.Count().ToString() + ")";
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                                    Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;
                                    Response.Redirect(Resources.PageURL.PoPayment, false);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("NoItemforPayment").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                ResetForm(ControlsEnum.RESETPAYMENT);
                            }
                        }
                        #endregion
                        break;
                    #endregion
                    #region Pick Inv & for Cr/Dr. Note
                    case ControlsEnum.PICKFORCRDRNOTE:
                        #region  For Picking like invoices from different pages
                        SetAllocationDetails();
                        if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] != null)
                        {
                            SelectedInvoicesInfoLst = (List<SelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst];
                            foreach (SelectionInfo item in SelectedInvoicesInfoLst)
                            {
                                if (item.chkChecked)
                                {
                                    if (item.ApprovedStatus == 2 && item.IsPosted)
                                    {
                                        if (SelectedCurrency == 0)
                                            SelectedCurrency = item.CurrencyPK;
                                        if (SelectedVendors == 0)
                                            SelectedVendors = item.VendorPK;
                                        if (SelectedCurrency != item.CurrencyPK)
                                        {
                                            InvalidPaymentItem = true;
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency_CNDN").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                        else if (SelectedVendors != item.VendorPK)
                                        {
                                            InvalidPaymentItem = true;
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor_CNDN").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                        //For Saving Selected Item PK
                                        hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + item.InvoicePK.ToString();
                                    }
                                    else
                                    {
                                        if (item.ApprovedStatus != 2)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("MsgApproveforCNDN").ToString();
                                        }
                                        else if (!item.IsPosted)  //Posted == false
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("MsgJournalizeforCNDN").ToString();
                                        }
                                        InvalidPaymentItem = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }

                                }
                            }

                            if (!InvalidPaymentItem)
                            {
                                SelectedInvoicesInfoLst = SelectedInvoicesInfoLst.Where(f => f.chkChecked == true).ToList();//Filtering: remove unchecked Items
                                foreach (SelectionInfo item in SelectedInvoicesInfoLst)
                                {
                                    if (SelectedInvoicesCrDr == null)
                                    {
                                        SelectedInvoicesCrDr = new List<long>();
                                    }
                                    SelectedInvoicesCrDr.Add(item.InvoicePK);
                                }
                                if (SelectedInvoicesCrDr != null && SelectedInvoicesCrDr.Count > 0)
                                {
                                    // btnPickForCrDrNote.Text = Resources.Controls.PickPoForCrDr;
                                    btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString() + "(" + SelectedInvoicesCrDr.Count().ToString() + ")";
                                    Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;
                                    //To Identify whether Purchase / Sales
                                    Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                                    Response.Redirect(Resources.PageURL.DrCrNote, false);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("NoItemforCNDN").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                ResetForm(ControlsEnum.RESETPAYMENT);
                            }
                        }
                        #endregion
                        break;
                    #endregion

                }
                return retObject;
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
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            int dept = 0;
            bool bIsChecked = false;
            try
            {
                switch (controlType)
                {
                    case ControlsEnum.INVOICEGET:
                        if (dtExpenseList != null && dtExpenseList.Rows.Count > 0)
                        {
                            bIsChecked = true;
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());//  Convert.ToInt32(dtExpenseList.Rows[0]["IVH_PK"].ToString());                           
                            Session[ERP.Utilities.SessionStrings.VendorPK] = dtExpenseList.Rows[0]["IVH_VND_PK"].ToString();
                            Session[ERP.Utilities.SessionStrings.Vendor] = dtExpenseList.Rows[0]["IVH_VENDOR_TEXT"].ToString();
                            ////start
                            Approved = Convert.ToInt32(dtExpenseList.Rows[0]["IVH_STATUS"].ToString());
                            Posted = Convert.ToBoolean(dtExpenseList.Rows[0]["IVH_HAS_JRNL_ENTRY"].ToString());
                            Group = Convert.ToInt16(dtExpenseList.Rows[0]["IVH_GROUP"].ToString());
                            ////                            
                            if (!string.IsNullOrEmpty(dtExpenseList.Rows[0]["IVH_DEPT"].ToString()) && int.TryParse(dtExpenseList.Rows[0]["IVH_DEPT"].ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
                            hdfIsInvCancelled.Value = dtExpenseList.Rows[0]["IVH_DEL_STATUS"].ToString();

                            if (Group == (int)POInvoiceGroup.AgtInvoice)
                            {
                                Response.Redirect(Resources.PageURL.AgtComm + "?Type=" + (int)POInvoiceGroup.AgtInvoice + "&fromPK=" + CurrPK);
                            }
                            else if (bIsChecked)
                            {
                                SetUIEditView(ActionsEnum.VIEW);
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
                                GetFieldValues(ControlsEnum.EXPENSEHEADER);
                                SetFieldValues(ControlsEnum.EXPENSEHEADER);
                                SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                                TempExpenseHeaderSession = ExpenseHeaderSession;
                            }

                        }
                        break;
                    #region VENDORCONTACTYPE
                    case ControlsEnum.VENDORCONTACTYPE:

                        if (dtAdsType != null && dtAdsType.Rows.Count > 0)
                        {
                            chkHO.Checked = false;
                            txtBranchCode.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtVatTaxId.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtVatTaxId.Text))
                                txtVatTaxId.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            hdfAddressType.Value = dtAdsType.Rows[0][Resources.DataFieldRes.VncPk].ToString();
                            txtAddressType.Text = dtAdsType.Rows[0][Resources.DataFieldRes.VncName].ToString();
                            if (Convert.ToInt32(dtAdsType.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                chkHO.Checked = true;
                            }
                            //hdfVendorContactType.Value = dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.vncType].ToString();
                        }
                        break;
                    #endregion

                    #region VENDORSELECTEDDTL
                    case ControlsEnum.VENDORSELECTEDDTL:
                        if (dtPageData != null && dtPageData.Rows.Count > 0)
                        {
                            hdfVendorDtl.Value = dtPageData.Rows[0][Resources.DataFieldRes.VendorPK].ToString();
                        }

                        break;
                    #endregion

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
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
                        {
                            chkHO.Checked = false;
                            txtBranchCode.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                            txtVatTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                            if (string.IsNullOrEmpty(txtVatTaxId.Text))
                                txtVatTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                            if (Convert.ToInt32(dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                chkHO.Checked = true;
                                //txtBranchCode.Enabled = false;
                                vrfBranchCode.Enabled = false;
                                // txtBranchCode.CssClass = "medium input-disabled";                                
                                //txtBranchCode.Text = "00000";
                            }
                            else
                            {

                                txtBranchCode.Enabled = true;
                                vrfBranchCode.Enabled = true;
                                //txtBranchCode.CssClass = "medium";
                            }


                            //hdfVendorContactType.Value = dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.vncType].ToString();
                        }

                        //if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
                        //{
                        //    txtBranchCode.Text = dtAdsTypeDtl.Rows[0]["VNC_TYPE_NAME"].ToString();
                        //    txtVatTaxId.Text = dtAdsTypeDtl.Rows[0]["VNC_TAX_NO"].ToString();
                        //}
                        break;
                    case ControlsEnum.EXPENSEHEADER:
                        if (expenseHeaderObj != null)
                        {
                            Approved = expenseHeaderObj.IVH_STATUS;
                            btnSave.Visible = expenseHeaderObj.IVH_DEL_STATUS == "0" ? true : false;
                            hdfExpensePK.Value = expenseHeaderObj.IVH_PK.ToString();
                            hdfExpenseNo.Value = expenseHeaderObj.IVH_NO == string.Empty ? "" : expenseHeaderObj.IVH_NO;
                            lblExpenseNo.Text = expenseHeaderObj.IVH_NO == string.Empty ? "[NEW]" : expenseHeaderObj.IVH_NO;
                            txtExpenseDate.Text = expenseHeaderObj.IVH_DATE;

                            txtVendor.Text = HttpUtility.HtmlDecode(expenseHeaderObj.IVH_VENDOR_NAME);
                            hdfVendor.Value = expenseHeaderObj.IVH_VENDOR;
                            hdfExchangeRate.Value = expenseHeaderObj.IVH_EXCHG_RATE.ToString();
                            //  hdfCurrency.Value = expenseHeaderObj.IVH_CURRENCY.ToString();
                            hdfVendorInvType.Value = expenseHeaderObj.IVH_TYPE;
                            //  txtCurrency.Text = HttpUtility.HtmlDecode(expenseHeaderObj.IVH_CURRENCY_TEXT);
                            txtVendorInvNO.Text = HttpUtility.HtmlDecode(expenseHeaderObj.IVH_VENDOR_INV_NO);
                            if (!string.IsNullOrEmpty(expenseHeaderObj.IVH_VENDOR_INV_DATE) &&
                                Convert.ToDateTime(expenseHeaderObj.IVH_VENDOR_INV_DATE).Year > 1900)
                                txtVendorInvDate.Text = expenseHeaderObj.IVH_VENDOR_INV_DATE;
                            txtCreditDays.Text = expenseHeaderObj.IVH_CREDIT_DAYS;
                            txtInvoiceDueDate.Text = expenseHeaderObj.IVH_DATE_PAY_BY;
                            txtRefundAmount.Text = expenseHeaderObj.IVH_AMOUNT_REFUND.ToString(hdfCurrencyFormat.Value);
                            txtAmountToSettle.Text = expenseHeaderObj.IVH_AMOUNT_SET.ToString(hdfCurrencyFormat.Value);
                            txtHdrDiscount.Text = expenseHeaderObj.IVH_DISCOUNT_TC.ToString(hdfCurrencyFormat.Value);
                            txtHdrTax.Text = expenseHeaderObj.IVH_TAX_TC.ToString(hdfCurrencyFormat.Value);
                            txtPriceAdj.Text = expenseHeaderObj.IVH_AMOUNT_ADJUST.ToString(hdfCurrencyFormat.Value);
                            txtHdrTotal.Text = expenseHeaderObj.IVH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                            chkOriginalinvoice.Checked = expenseHeaderObj.IVH_ORGINAL_RCVD == (byte)1;
                            txtRemarks.Text = HttpUtility.HtmlDecode(expenseHeaderObj.IVH_REMARKS);
                            ddlCompany.SelectedValue = expenseHeaderObj.IVH_COMPANY.ToString();
                            if (!string.IsNullOrEmpty(expenseHeaderObj.IVH_GST_TYPE))
                                ddlInvoiceGstType.SelectedValue = expenseHeaderObj.IVH_GST_TYPE;

                            ModifiedDatePnl.Visible = true;
                            LastModifiedTime = expenseHeaderObj.LAST_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);

                            txtHdrAddType.Text = string.IsNullOrEmpty(expenseHeaderObj.IVH_BRANCH_NAME) ? "Select/Type" : expenseHeaderObj.IVH_BRANCH_NAME;
                            if (Convert.ToInt32(expenseHeaderObj.IVH_BRANCH_TYPE) == (int)VendorContactTypeEnum.HeadOffice)
                            {
                                HdrchkHO.Checked = true;
                                vrfHdrBranchCode.Enabled = false;
                            }
                            txtHdrBranchCode.Text = string.IsNullOrEmpty(expenseHeaderObj.IVH_BRANCH_TEXT) ? string.Empty : expenseHeaderObj.IVH_BRANCH_TEXT;
                            txtHdrTaxId.Text = string.IsNullOrEmpty(expenseHeaderObj.IVH_TAX_ID) ? string.Empty : expenseHeaderObj.IVH_TAX_ID;
                            hdfAddTypeHdr.Value = expenseHeaderObj.IVH_VENDOR_CONTACT == null ? string.Empty : expenseHeaderObj.IVH_VENDOR_CONTACT.ToString();

                            //GetFieldValues(ControlsEnum.EXPENSEADVANCES);
                            //SetFieldValues(ControlsEnum.EXPENSEADVANCES);
                            //SettlementTotal = 0;
                            //foreach (DataRow dr in dtExpenseAdvanced.Rows)
                            //{
                            //    if (dr["FTH_EXP_ADV_FLAG"].ToString() == "1")
                            //        SettlementTotal += Convert.ToDecimal(dr["IED_AMOUNT"].ToString());
                            //}
                            //txtAmountToSettle.Text = GetFormattedCurrencyWithSeperation(SettlementTotal);
                            hdfIsNewParty.Value = "0";

                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = expenseHeaderObj.IVH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            }

                        }
                        break;
                    #region Line Item Details
                    case ControlsEnum.SELECTEDITEM:
                        if (soExpenseDetailsObj != null)
                        {
                            ExpensePK = soExpenseDetailsObj.VID_PK;
                            hdfDetailPK.Value = soExpenseDetailsObj.VID_PK.ToString();
                            CurrSlNo = soExpenseDetailsObj.VID_SL_NO;
                            txtDesc.Text = HttpUtility.HtmlDecode(soExpenseDetailsObj.VID_INSTRUCTIONS);
                            txtQty.Text = GetFormattedCurrency(soExpenseDetailsObj.VID_QTY_INVOICED);
                            hdfUOM.Value = soExpenseDetailsObj.VID_UOM.ToString();
                            txtUOM.Text = HttpUtility.HtmlDecode(soExpenseDetailsObj.VID_UOM_TEXT);
                            txtRate.Text = GetFormattedRate(soExpenseDetailsObj.VID_RATE);
                            txtDiscount.Text = GetFormattedCurrency(soExpenseDetailsObj.VID_DISCOUNT);
                            txtAmount.Text = GetFormattedCurrency(soExpenseDetailsObj.VID_AMOUNT);
                            txtTax.Text = GetFormattedCurrency(soExpenseDetailsObj.VID_TAX);
                            txtDtlRemark.Text = HttpUtility.HtmlDecode(soExpenseDetailsObj.VID_REMARKS);
                            if (!string.IsNullOrEmpty(soExpenseDetailsObj.VID_REF_DATE))
                                txtPartyInvDate.Text = Convert.ToDateTime(soExpenseDetailsObj.VID_REF_DATE).ToString(Resources.Constants.DateFormatShort);
                            txtPartyNo.Text = HttpUtility.HtmlDecode(soExpenseDetailsObj.VID_REF_NO);


                            txtVendorDtl.Text = HttpUtility.HtmlDecode(soExpenseDetailsObj.VID_VENDOR_TEXT);
                            hdfVendorDtl.Value = soExpenseDetailsObj.VID_VENDOR.ToString();


                            txtVatTaxId.Text = soExpenseDetailsObj.VID_TAX_ID == null ? "" : HttpUtility.HtmlDecode(soExpenseDetailsObj.VID_TAX_ID);
                            txtBranchCode.Text = soExpenseDetailsObj.VID_BRANCH_TEXT == null ? "" : HttpUtility.HtmlDecode(soExpenseDetailsObj.VID_BRANCH_TEXT);

                            txtAddressType.Text = soExpenseDetailsObj.VID_BRANCH_NAME == null ? "" : HttpUtility.HtmlDecode(soExpenseDetailsObj.VID_BRANCH_NAME);
                            hdfAddressType.Value = soExpenseDetailsObj.VID_BRANCH == null ? "" : soExpenseDetailsObj.VID_BRANCH;

                            if (Convert.ToInt32(soExpenseDetailsObj.VID_BRANCH_TYPE) == (int)VendorContactTypeEnum.HeadOffice)
                            {

                                chkHO.Checked = true;
                                //txtBranchCode.Enabled = false;
                                vrfBranchCode.Enabled = false;
                                //txtBranchCode.CssClass = "medium input-disabled";
                            }
                            else
                            {
                                chkHO.Checked = false;
                                txtBranchCode.Enabled = true;
                                vrfBranchCode.Enabled = true;
                                //txtBranchCode.CssClass = "medium";
                            }

                            txtTotAmt.Text = ((Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(txtTax.Text)) - (Convert.ToDecimal(txtDiscount.Text))).ToString();

                            if (soExpenseDetailsObj.VID_REFUND_DATE.HasValue)
                            {
                                string str = soExpenseDetailsObj.VID_REFUND_DATE.Value.ToString(Resources.Constants.DateFormatShort);
                                str = str.Remove(0, 3);
                                txtTaxRefundDate.Text = str;
                            }
                            else
                            {
                                string str = txtExpenseDate.Text;
                                str = str.Remove(0, 3);
                                txtTaxRefundDate.Text = str;
                            }

                        }
                        break;
                    #endregion
                    # region  FILE_UPLOAD
                    case ControlsEnum.UPLOADEDFILES:
                        CurrPK = expenseHeaderObj.IVH_PK;
                        POUploadList = expenseHeaderObj.FileList;
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
                #region TAXTYPES
                case ControlsEnum.TAXTYPES:
                    //Bind Tax dropdown
                    ddlPopupTaxType.Items.Clear();
                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count > 0)
                    {
                        ddlPopupTaxType.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTaxDetails, Resources.DataFieldRes.RFQResponseTaxHead);
                        ddlPopupTaxType.DataTextField = Resources.DataFieldRes.RFQResponseTaxHead;
                        ddlPopupTaxType.DataValueField = Resources.DataFieldRes.RFQResponseTaxPK;
                        ddlPopupTaxType.DataBind();
                    }
                    if (hdfTaxCategory.Value == ((int)TaxType.Tax).ToString())
                    {
                        if (IsCustomTaxEnabled)
                            ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    }
                    else
                    {
                        ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    }
                    break;
                #endregion
                #region VENDORCONTACTYPE
                case ControlsEnum.VENDORCONTACTYPE:
                    ddlAddressType.Items.Clear();
                    if (TempConfigMstDetails != null && TempConfigMstDetails.Count > 0)
                    {
                        ddlAddressType.DataSource = TempConfigMstDetails;
                        ddlAddressType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlAddressType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlAddressType.DataBind();
                        SetBranchCodeVisibility();
                    }
                    //ddlAddressType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Company Search
                case ControlsEnum.COMPANYSRCH:
                    ddlCompanySrch.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompanySrch.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CMP_DISPLAY_CODE);
                        ddlCompanySrch.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                        ddlCompanySrch.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanySrch.DataBind();
                    }
                    ddlCompanySrch.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Company
                case ControlsEnum.COMPANY:
                    ddlCompany.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlCompany.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompany.DataBind();

                        ddlCompanyView.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompanyView.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompanyView.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanyView.DataBind();
                    }
                    //To set company related to current SBU 
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }

                    break;
                #endregion
                #region Invoice Type GST
                case ControlsEnum.INVOICEGSTTYPE:
                    ddlInvoiceGstType.Items.Clear();
                    if (dtInvoiceGstType != null && dtInvoiceGstType.Rows.Count > 0)
                    {
                        ddlInvoiceGstType.DataSource = dtInvoiceGstType;
                        ddlInvoiceGstType.DataTextField = "FTM_CODE";
                        ddlInvoiceGstType.DataValueField = "FTM_PK";
                        ddlInvoiceGstType.DataBind();
                    }
                    ddlInvoiceGstType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                switch (controlType)
                {
                    case ControlsEnum.EXPENSEADVANCES:
                        grdExpenseAdv.DataSource = dtExpenseAdvanced;
                        grdExpenseAdv.DataBind();
                        break;
                    case ControlsEnum.EXPENSEDETAIL:
                        if (expenseHeaderObj != null)
                        {
                            soExpenseDetailsList = new List<POInvoiceDetails>();
                            soExpenseDetailsList = expenseHeaderObj.OrderDetail;
                            if (soExpenseDetailsList != null)
                            {
                                grdItemDetails.DataSource = soExpenseDetailsList;
                                grdItemDetails.DataBind();
                            }
                        }
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        if (IsHeaderTax)
                        {
                            taxHdrList = TempExpenseHeaderSession.TaxHdr == null ? new List<POInvoiceTaxHdr>() :
                                TempExpenseHeaderSession.TaxHdr.Where(tax => Convert.ToInt32(tax.VTL_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                        }
                        else
                        {
                            soDtlObj = EditTempExpenseHeaderSession.OrderDetail == null ? null :
                                EditTempExpenseHeaderSession.OrderDetail.SingleOrDefault(dtl => dtl.VID_PK == ExpensePK && dtl.VID_SL_NO == CurrSlNo);
                            if (soDtlObj != null)
                            {
                                taxHdrList = soDtlObj.TaxDtl == null ? new List<POInvoiceTaxHdr>() :
                                    soDtlObj.TaxDtl.Where(tax => Convert.ToInt32(tax.VTL_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                            else
                                taxHdrList = new List<POInvoiceTaxHdr>();
                        }
                        grdTaxDetails.DataSource = taxHdrList;
                        grdTaxDetails.DataBind();
                        break;
                    case ControlsEnum.EXPENSELIST:
                        if (dtExpenseList != null)
                        {
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdExpenseList.PageIndex = Convert.ToInt32(PageIndex);
                            grdExpenseList.DataSource = dtExpenseList.DefaultView;
                            grdExpenseList.DataBind();

                            //For Setting/Resetting Colour of a selected InvoiceNo
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
                        }
                        break;
                    case ControlsEnum.UPLOADEDFILES:
                        grdUploads.DataSource = POUploadList;
                        grdUploads.DataBind();
                        break;

                    case ControlsEnum.AMOUNTDETAILS:
                        if (dtAmountDetails != null)
                        {
                            grdPaidAmntSplitup.DataSource = dtAmountDetails;
                            grdPaidAmntSplitup.DataBind();
                        }
                        break;
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
        /// 
        private void BindDefaultUOM()
        {
            string defaultSetting;
            GetFieldValues(ControlsEnum.DEFAULTUOM);
            if (dtPageData != null && dtPageData.Rows.Count > 0)
            {
                defaultSetting = dtPageData.Rows[0]["ACF_DATA"].ToString();
                string[] uomSettings = defaultSetting.Split(',');
                if (uomSettings.Length == 2)
                {
                    hdfUOM.Value = uomSettings[0];
                    txtUOM.Text = uomSettings[1];
                }
            }
        }
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
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.VENDORCONTACTYPEDETAILS:
                    txtBranchCode.Text = string.Empty;
                    txtVatTaxId.Text = string.Empty;
                    hdfAddTypeHdr.Value = "0";
                    break;
                case ControlsEnum.TAXPOPUPGRID:
                    TaxPK = 0;
                    SelectedDtlPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    break;
                case ControlsEnum.EXPENSEHEADER:
                    CurrPK = 0;
                    break;
                case ControlsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
                    break;

                case ControlsEnum.EXPENSELIST:
                    hdfPartyNoCheck.Value = "0";
                    CurrPK = 0;
                    txtAmountToSettle.Text = "0.00";
                    txtRefundAmount.Text = "0.00";
                    SettlementTotal = 0;
                    hdfIsNewParty.Value = "1";
                    txtExpenseNumber.Text = "Select/Type";
                    txtVendorSch.Text = "Select/Type";
                    hdfExpenseNumberPK.Value = "";
                    hdfVendorSch.Value = "";
                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    ddlStatus.ClearSelection();
                    ddlCompanySrch.SelectedValue = CommonConstants.SELECTVAL;

                    lblExpenseNo.Text = string.Empty;
                    txtExpenseDate.Text = string.Empty;
                    txtVendor.Text = string.Empty;
                    hdfVendor.Value = string.Empty;

                    txtVendorDtl.Text = "Select/Type";

                    txtVendorDtl.Text = string.Empty;
                    hdfVendorDtl.Value = string.Empty;
                    ddlAddressType.DataSource = null;
                    ddlAddressType.DataBind();
                    txtVatTaxId.Text = string.Empty;

                    txtHdrAddType.Text = "Select/Type";
                    hdfAddTypeHdr.Value = string.Empty;
                    HdrchkHO.Checked = false;
                    txtHdrBranchCode.Text = string.Empty;
                    txtHdrTaxId.Text = string.Empty;

                    txtAddressType.Text = "Select/Type";
                    hdfAddressType.Value = string.Empty;
                    chkHO.Checked = false;
                    txtBranchCode.Text = string.Empty;
                    txtVatTaxId.Text = string.Empty;

                    // txtCurrency.Text = string.Empty;
                    // hdfCurrency.Value = string.Empty;
                    hdfVendorInvType.Value = string.Empty;
                    txtVendorInvNO.Text = string.Empty;
                    txtVendorInvDate.Text = string.Empty;
                    txtCreditDays.Text = string.Empty;
                    txtInvoiceDueDate.Text = string.Empty;
                    chkOriginalinvoice.Checked = false;
                    txtRemarks.Text = string.Empty;
                    txtHdrDiscount.Text = string.Empty;
                    txtHdrTax.Text = string.Empty;
                    txtPriceAdj.Text = string.Empty;
                    txtHdrTotal.Text = string.Empty;
                    ddlInvoiceGstType.SelectedValue = CommonConstants.SELECTVAL;

                    txtPartyInvDate.Text = string.Empty;
                    txtPartyNo.Text = string.Empty;

                    ModifiedDatePnl.Visible = false;
                    base.WkfRefID = 0;
                    grdItemDetails.DataSource = null;
                    grdItemDetails.DataBind();
                    ExpenseHeaderSession = null;
                    ResetForm(ControlsEnum.EXPENSEDETAIL);
                    FileDetailsList = null;
                    POUploadList = null;
                    ResetForm(ControlsEnum.ADDITEM);

                    break;
                case ControlsEnum.EXPENSEDETAIL:
                    ExpensePK = 0;
                    TempExpenseHeaderSession = ExpenseHeaderSession;
                    CurrSlNo = 0;
                    hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtDesc.Text = string.Empty;
                    txtQty.Text = CommonConstants.SELECT_VALUE_ONE;

                    //txtVendorDtl.Text = string.Empty;
                    //hdfVendorDtl.Value = string.Empty;
                    //ddlAddressType.ClearSelection();
                    // txtVatTaxId.Text = string.Empty;
                    //txtBranchCode.Text = string.Empty;
                    txtTotAmt.Text = string.Empty;
                    //hdfUOM.Value = string.Empty;
                    //txtUOM.Text = string.Empty;
                    BindDefaultUOM();
                    txtRate.Text = string.Empty;
                    txtDiscount.Text = string.Empty;
                    txtAmount.Text = string.Empty;
                    txtTax.Text = string.Empty;
                    txtDtlRemark.Text = string.Empty;
                    txtPartyNo.Text = string.Empty;
                    txtPartyInvDate.Text = string.Empty;
                    txtTaxRefundDate.Text = string.Empty;
                    break;
                case ControlsEnum.EXPENSEDETAILTAX:
                    ExpensePK = 0;
                    TempExpenseHeaderSession = ExpenseHeaderSession;
                    CurrSlNo = 0;
                    hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ZERO;
                    ////txtDesc.Text = string.Empty;
                    ////txtQty.Text = CommonConstants.SELECT_VALUE_ONE;
                    //hdfUOM.Value = string.Empty;
                    //txtUOM.Text = string.Empty;
                    BindDefaultUOM();
                    ////txtRate.Text = string.Empty;
                    ////txtDiscount.Text = string.Empty;
                    ////txtAmount.Text = string.Empty;
                    ////txtTax.Text = string.Empty;
                    ////txtDtlRemark.Text = string.Empty;
                    ////txtPartyNo.Text = string.Empty;
                    ////txtPartyInvDate.Text = string.Empty;
                    break;
                case ControlsEnum.RESETPAYMENT:
                    SelectedVendors = 0;
                    SelectedCurrency = 0;
                    SelectedInvoices = null;
                    SelectedInvoicesCrDr = null;
                    btnPickForPayment.Text = GetLocalResourceObject("PickPoForPayment").ToString();
                    //Resetting Color
                    hdfSelectedItemPk.Value = "0";
                    break;
            }
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
        private void SetDetailTax(POInvoiceHeader expenseHdr)
        {
            double quantity;
            double rate;
            quantity = 0;
            rate = 0;

            if (double.TryParse(txtRate.Text, out quantity) && double.TryParse(txtQty.Text, out rate))
            {
                if (txtAmount != null)
                {
                    txtAmount.Text = GetFormattedCurrency(rate * quantity);

                    ExpensePK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                    SetItemTax(expenseHdr);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        private bool SetItemTax(POInvoiceHeader expenseHdr)
        {
            double amount;
            double discount;
            double itmTax;
            double netAmount;
            amount = 0;
            discount = 0;
            netAmount = 0;
            itmTax = 0;
            Double.TryParse(txtAmount.Text.Trim(), out amount);
            if (amount >= 0)
            {
                if (expenseHdr != null)
                {
                    expenseHeaderObj = expenseHdr;
                    soExpenseDetailsObj = expenseHeaderObj.OrderDetail.SingleOrDefault(crt => crt.VID_PK == ExpensePK
                        && crt.VID_SL_NO == CurrSlNo);
                    if (soExpenseDetailsObj != null)
                    {
                        if (soExpenseDetailsObj.TaxDtl != null)
                        {
                            var discDetail = soExpenseDetailsObj.TaxDtl.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (POInvoiceTaxHdr taxHdrObj in discDetail)
                            {
                                string taxFormula = taxHdrObj.VTL_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    taxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                }
                            }
                            discount = soExpenseDetailsObj.TaxDtl.Where(ctr => ctr.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.VTL_TAX_AMT);
                        }
                        netAmount = amount - discount;
                        txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                        if (soExpenseDetailsObj.TaxDtl != null)
                        {
                            var taxDetail = soExpenseDetailsObj.TaxDtl.Where(ctr => ctr.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (POInvoiceTaxHdr taxHdrObj in taxDetail)
                            {
                                string taxFormula = taxHdrObj.VTL_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    //1 :- No need to create formula for manual entry of Item Tax amount in TAX POPUP.  
                                    //0 :- Create tax formula
                                    if (hdfApplyTax.Value == "0")
                                    {
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                        taxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                                    }
                                }
                            }
                            hdfApplyTax.Value = "0";
                            itmTax = soExpenseDetailsObj.TaxDtl.ToList().Where(ctr => ctr.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(ctr => ctr.VTL_TAX_AMT);
                        }
                        txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                        soExpenseDetailsObj.VID_AMOUNT = Math.Round(amount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soExpenseDetailsObj.VID_DISCOUNT = Math.Round(discount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soExpenseDetailsObj.VID_TAX = Math.Round(itmTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        soExpenseDetailsObj.VID_NET_AMOUNT = Math.Round(amount - discount + itmTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        txtTotal.Text = soExpenseDetailsObj.VID_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);



                        txtTotAmt.Text = ((Convert.ToDecimal(txtAmount.Text) + Convert.ToDecimal(txtTax.Text)) - Convert.ToDecimal(txtDiscount.Text)).ToString();
                    }
                }
                return true;
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                return false;
            }
        }
        private void SetSubTotal()
        {
            Label lblSubTotalFooter;
            ExpenseHeaderSession.IVH_AMOUNT_TC = ExpenseHeaderSession.OrderDetail.Sum(dtl => dtl.VID_NET_AMOUNT);
            if (grdItemDetails.FooterRow != null)
            {
                lblSubTotalFooter = grdItemDetails.FooterRow.FindControl("lblSubTotalFooter") as Label;
                if (lblSubTotalFooter != null)
                {
                    //lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = ExpenseHeaderSession.IVH_AMOUNT_TC.ToString(hdfCurrencyFormat.Value);
                    lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = GetFormattedCurrencyWithSeperation(ExpenseHeaderSession.IVH_AMOUNT_TC);

                }
            }
        }
        private bool SetHdrTax()
        {
            double amount;
            double discount;
            double adjust;
            amount = 0;
            adjust = 0;

            if (ExpenseHeaderSession != null)
            {
                expenseHeaderObj = ExpenseHeaderSession;
                amount = Convert.ToDouble(expenseHeaderObj.IVH_AMOUNT_TC);
                discount = 0;
                if (expenseHeaderObj.TaxHdr != null)
                {
                    var discHeader = expenseHeaderObj.TaxHdr.Where(hdr => hdr.VTL_TAX_CATEGORY == ((int)TaxType.Discount));
                    foreach (POInvoiceTaxHdr taxHdrObj in discHeader)
                    {
                        string taxFormula = taxHdrObj.VTL_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                        }
                    }
                    discount = expenseHeaderObj.TaxHdr.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.VTL_TAX_AMT);
                }
                expenseHeaderObj.IVH_DISCOUNT_TC = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);
                amount = amount - discount;
                if (expenseHeaderObj.TaxHdr != null)
                {
                    var taxHeader = expenseHeaderObj.TaxHdr.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Tax));
                    foreach (POInvoiceTaxHdr taxHdrObj in taxHeader)
                    {
                        string taxFormula = taxHdrObj.VTL_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            //1 :- No need to create formula for manual entry of Header Tax amount in TAX POPUP.  
                            //0 :- Create tax formula
                            if (hdfApplyHdrTax.Value == "0")
                            {
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                taxHdrObj.VTL_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                            }
                        }
                    }
                    hdfApplyHdrTax.Value = "0";
                    expenseHeaderObj.IVH_TAX_TC = expenseHeaderObj.TaxHdr.Where(quotation => quotation.VTL_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.VTL_TAX_AMT);
                }
                txtHdrTax.Text = txtHdrTax.ToolTip = expenseHeaderObj.IVH_TAX_TC.ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtPriceAdj.Text, out adjust);
                expenseHeaderObj.IVH_AMOUNT_ADJUST = adjust;
                expenseHeaderObj.IVH_AMOUNT_NET_TC = Convert.ToDouble(expenseHeaderObj.IVH_AMOUNT_TC) - expenseHeaderObj.IVH_DISCOUNT_TC + expenseHeaderObj.IVH_TAX_TC
                    + expenseHeaderObj.IVH_AMOUNT_ADJUST;

                txtHdrTotal.Text = txtHdrTotal.ToolTip = expenseHeaderObj.IVH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                ExpenseHeaderSession = expenseHeaderObj;
                TempExpenseHeaderSession = expenseHeaderObj;
                if (expenseHeaderObj.IVH_AMOUNT_NET_TC > 0 && SettlementTotal > 0)
                {
                    txtRefundAmount.Text = (SettlementTotal - (decimal)expenseHeaderObj.IVH_AMOUNT_NET_TC).ToString(hdfCurrencyFormat.Value);
                }

            }
            return true;
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
        public string GetFormattedNumber(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormat.Value);
        }
        public string GetFormattedNumberWithSeperation(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfDecimalFormatWithSeperation.Value);
        }
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }
        public string GetFormattedCurrencyWithSeperation(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperation.Value);
        }
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
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
        /// I exist Po in list
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsPkExist(List<long> lst, long pk)
        {
            bool flag = false;
            if (lst != null)
                foreach (long item in lst)
                    if (item == pk)
                    {
                        flag = true;
                        break;
                    }
            return flag;
        }

        public DateTime? GetDateFromMonth()
        {
            DateTime? dateTime = null;
            string str = txtTaxRefundDate.Text;
            if (!str.IsNullOrEmptyOrWhitespace())
            {
                str = "01-" + str;
                dateTime = Convert.ToDateTime(str);
                return dateTime;
            }
            str = txtExpenseDate.Text;
            if (!str.IsNullOrEmptyOrWhitespace())
            {
                dateTime = Convert.ToDateTime(str);
                return dateTime;
            }
            return dateTime;
        }

        public bool GetBalanceLinkVisibility(string amt, string bal)
        {
            bool balVisible = false;
            if (!string.IsNullOrEmpty(amt) && !string.IsNullOrEmpty(bal))
            {
                double amountPayable = Convert.ToDouble(amt);
                double balanceToPay = Convert.ToDouble(bal);
                //if (amountPayable > balanceToPay)
                if (amountPayable != balanceToPay)
                {
                    balVisible = true;
                }
            }
            return balVisible;
        }

        public bool GetBalanceLableVisibility(string amt, string bal)
        {
            bool balVisible = true;
            if (!string.IsNullOrEmpty(amt) && !string.IsNullOrEmpty(bal))
            {
                double amountPayable = Convert.ToDouble(amt);
                double balanceToPay = Convert.ToDouble(bal);
                //if (amountPayable > balanceToPay)
                if (amountPayable != balanceToPay)
                {
                    balVisible = false;
                }
            }
            return balVisible;
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
        #region Set BranchCode Visibility
        private void SetBranchCodeVisibility()
        {
            txtBranchCode.Text = string.Empty;
            txtVatTaxId.Text = string.Empty;

            GetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);
            SetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);

        }

        private void SetHDRBranchCodeVisibility()
        {
            txtHdrBranchCode.Text = string.Empty;
            txtHdrTaxId.Text = string.Empty;

            GetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILSHDR);
            if (dtAdsTypeDtl != null && dtAdsTypeDtl.Rows.Count > 0)
            {
                HdrchkHO.Checked = false;
                txtHdrBranchCode.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTypeName].ToString();
                txtHdrTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncTaxNo].ToString();
                if (string.IsNullOrEmpty(txtHdrTaxId.Text))
                    txtHdrTaxId.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VendorTaxId].ToString();
                hdfAddTypeHdr.Value = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncPk].ToString();
                txtHdrAddType.Text = dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.VncName].ToString();

                if (Convert.ToInt32(dtAdsTypeDtl.Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.HeadOffice)
                {
                    HdrchkHO.Checked = true;
                    //txtHdrBranchCode.Enabled = false;
                    vrfHdrBranchCode.Enabled = false;
                    //txtHdrBranchCode.CssClass = "medium input-disabled";
                    // txtHdrBranchCode.Text = "00000";
                }
                else
                {

                    txtHdrBranchCode.Enabled = true;
                    vrfHdrBranchCode.Enabled = true;
                    txtHdrBranchCode.CssClass = "medium";
                }
            }

        }
        #endregion

        #region Grd Status maintains
        //For sett allocation details
        private void SetAllocationDetails()
        {
            if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] != null)
                SelectedInvoicesInfoLst = (List<SelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst];
            else
                SelectedInvoicesInfoLst = new List<SelectionInfo>();
            CheckBox chbSelect;
            foreach (GridViewRow item in grdExpenseList.Rows)
            {
                chbSelect = (CheckBox)item.FindControl("chkInvselect");
                SelectionInfo objSaleOrderInfo = new SelectionInfo();
                objSaleOrderInfo.chkChecked = false;
                objSaleOrderInfo.InvoicePK = Convert.ToInt32(grdExpenseList.DataKeys[item.RowIndex].Value.ToString());
                if (chbSelect.Checked)
                {
                    objSaleOrderInfo.chkChecked = true;
                    objSaleOrderInfo.VendorPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfVendorPK")).Value);//E
                    objSaleOrderInfo.CurrencyPK = Convert.ToInt32(((HiddenField)item.FindControl("hdfPOCurrency")).Value);//E
                    objSaleOrderInfo.ApprovedStatus = Convert.ToInt32(((HiddenField)item.FindControl("hdfApproved")).Value);
                    objSaleOrderInfo.IsPosted = Convert.ToBoolean(((HiddenField)item.FindControl("hdfPosted")).Value);
                    objSaleOrderInfo.Tax = 0;//Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)item.FindControl("hdfTaxAmount")).Value) ? "0" : ((HiddenField)item.FindControl("hdfTaxAmount")).Value);
                    //LinkButton lbnBalAmt = item.FindControl("lbnBalAmt") as LinkButton;
                    objSaleOrderInfo.BalanceAmt = 0; //Convert.ToDecimal(lbnBalAmt.Text);
                }

                bool alreadyExists = SelectedInvoicesInfoLst.Exists(itemLst => itemLst.InvoicePK == objSaleOrderInfo.InvoicePK);
                if (alreadyExists)
                    ChangeItem(objSaleOrderInfo);
                else
                    SelectedInvoicesInfoLst.Add(objSaleOrderInfo);
            }

            Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] = SelectedInvoicesInfoLst;


        }

        //for change the status of checked items
        private void ChangeItem(SelectionInfo item)
        {
            if (SelectedInvoicesInfoLst.Count > 0)
                foreach (var Items in SelectedInvoicesInfoLst)
                    if (Items.InvoicePK == item.InvoicePK)
                    {
                        Items.chkChecked = item.chkChecked;
                        Items.VendorPK = item.VendorPK;
                        Items.CurrencyPK = item.CurrencyPK;
                        Items.ApprovedStatus = item.ApprovedStatus;
                        Items.IsPosted = item.IsPosted;
                        Items.Tax = item.Tax;
                        Items.BalanceAmt = item.BalanceAmt;
                    }
        }

        //For Reset grid status
        private void SetGridStatus()
        {
            if (Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst] != null)
            {
                SelectedInvoicesInfoLst = (List<SelectionInfo>)Session[ERP.Utilities.SessionStrings.SelectedInvoicesInfoLst];
                int sodPK;
                foreach (GridViewRow item in grdExpenseList.Rows)
                {
                    sodPK = Convert.ToInt32(grdExpenseList.DataKeys[item.RowIndex].Value.ToString());
                    if (SelectedInvoicesInfoLst.Exists(itemLst => itemLst.InvoicePK == sodPK && itemLst.chkChecked == true))
                        ((CheckBox)item.FindControl("chkInvselect")).Checked = true;

                }
            }

        }

        #endregion

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
            POInvoiceService poExpenseServiceClient;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                int? result;
                POInvoiceTaxHdr tempInvTaxSplitObj = null;
                bool bIsChecked = false;

                if (hdfIsShowAlert.Value == "0")
                    btnAlert.Visible = false;

                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                Label lblSubTotal;
                string action;
                int selectedItemPK;

                double totalAmt;
                double currentTotal;
                double taxAmt;
                bool isValidDisc = true;
                FileInfo tempFileInfoObj;

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
                    if (((DropDownList)sender).ID == "ddlPopupTaxType")
                    {
                        commonActions = ActionsEnum.TAXTYPECHANGED;
                    }
                    else if (((DropDownList)sender).ID == "ddlAddressType")
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
                else if (sender.GetType().IsEquivalentTo(typeof(CheckBox)))
                {
                    if (((CheckBox)sender).ID == "chkHO")
                    {
                        commonActions = ActionsEnum.CHECKEDCHANGED;
                    }
                    else if (((CheckBox)sender).ID == "HdrchkHO")
                    {
                        commonActions = ActionsEnum.CHECKEDCHANGEDHDR;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtVendorDtl")
                    {
                        commonActions = ActionsEnum.VENDORTEXTCHANGED;
                    }
                    if (((TextBox)sender).ID == "txtAddressType")
                    {
                        commonActions = ActionsEnum.CLEARDETAIL;
                    }

                }
                switch (commonActions)
                {
                    case ActionsEnum.EXPENSEADVAPPLY:
                        SettlementTotal = 0;
                        foreach (GridViewRow grdrow in grdExpenseAdv.Rows)
                        {
                            if (((CheckBox)grdrow.FindControl("chkSelect")).Checked)
                            {
                                SettlementTotal += Convert.ToDecimal(((Label)grdrow.FindControl("lblAmount")).Text);
                            }
                        }
                        txtAmountToSettle.Text = GetFormattedCurrencyWithSeperation(SettlementTotal);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEMUPLOAD:
                        if (POUploadList != null && POUploadList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                POUploadList = POUploadList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion
                    #region EDITITEM
                    case ActionsEnum.EDITITEMUPLOAD:
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

                    #region ADDITEM
                    case ActionsEnum.ADDITEMUPLOAD:
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
                                            POUploadList = new List<BusinessObject.POInvoicing.POInvoiceUploads>();
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

                                        poUploadObj = new POInvoiceUploads();
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

                                        // poUploadObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                        poUploadObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                        POUploadList.Add(poUploadObj);

                                    }
                                }
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                                //  ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ScrollDown();", true);
                            }
                            grdUploads.Focus();
                        }
                        break;
                    #endregion
                    #region Grid Item Selected
                    case ActionsEnum.ITEMSELECTED:
                        foreach (GridViewRow grdrow in grdExpenseList.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            int selectedInvPK;
                            int dept;
                            HiddenField hdfGroup = (HiddenField)grdrow.FindControl("hdfGroup");
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedInvPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExpenseID")).Value);
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = workflowCore.GetRefID(selectedInvPK, PageProcessID);
                                ////
                                if (Convert.ToInt32(hdfGroup.Value) == (int)POInvoiceGroup.AgtInvoice) // Agent commision invoice 
                                    btnEditforCancel.Visible = false;
                                break;
                            }
                        }

                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        //End
                        break;
                    #endregion
                    #region Vendor Selected
                    case ActionsEnum.VENDORSELECTEDDTL:
                        string d = hdfVendorDtl.Value;
                        GetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        SetFieldValues(ControlsEnum.VENDORSELECTEDDTL);
                        GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        SetHDRBranchCodeVisibility();
                        SetBranchCodeVisibility();
                        break;
                    #endregion
                    #region Vendor Selected
                    case ActionsEnum.VENDORSELECTED:
                        ResetForm(ControlsEnum.VENDORCONTACTYPEDETAILS);
                        if (!string.IsNullOrEmpty(hdfVendor.Value))
                        {
                            GetFieldValues(ControlsEnum.VENDORDETAILS);
                            if (dtPageData != null && dtPageData.Rows.Count > 0)
                            {
                                //if (!dtPageData.Rows[0]["VEN_CURRENCY"].Equals(DBNull.Value))
                                //{
                                //    txtCurrency.Text = dtPageData.Rows[0]["VEN_CURRENCY_CODE"].ToString();
                                //    hdfCurrency.Value = dtPageData.Rows[0]["VEN_CURRENCY"].ToString();
                                //}
                                hdfVendorInvType.Value = dtPageData.Rows[0]["VEN_PO_TYPE"].ToString();
                                txtCreditDays.Text = dtPageData.Rows[0]["VEN_CREDIT_DAYS"].ToString();

                                //txtVatTaxId.Text = dtPageData.Rows[0]["VEN_TIN1"].ToString();
                                txtVendorDtl.Text = txtVendor.Text;
                                hdfVendorDtl.Value = hdfVendor.Value;
                                GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                                SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                                SetHDRBranchCodeVisibility();
                                SetBranchCodeVisibility();
                            }
                            else
                            {
                                //txtCurrency.Text = string.Empty;
                                //hdfCurrency.Value = string.Empty;
                                hdfVendorInvType.Value = string.Empty;
                                txtCreditDays.Text = string.Empty;
                            }
                            //ResetForm(ControlsEnum.VENDORCONTACTYPEDETAILS);

                            //GetFieldValues(ControlsEnum.VENDORCONTACTYPEMAINFROMDB);
                            //SetFieldValues(ControlsEnum.VENDORCONTACTYPEMAINFROMDB);
                        }
                        else
                        {
                            // txtCurrency.Text = string.Empty;
                            // hdfCurrency.Value = string.Empty;
                            hdfVendorInvType.Value = string.Empty;
                            txtCreditDays.Text = string.Empty;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateDueDate", "CalculateDueDate();", true);
                        break;
                    #endregion
                    #region Add Item
                    case ActionsEnum.ADDITEM:
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.ErrorMessages.Msg_SavError;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else//valid
                        {

                            if (txtPartyNo.Text != string.Empty && txtPartyInvDate.Text != string.Empty || hdfAddItem.Value == "1")
                            {
                                ExpenseHeaderSession = TempExpenseHeaderSession;
                                soExpenseDetailsList = ExpenseHeaderSession.OrderDetail;
                                soExpenseDetailsList = (List<POInvoiceDetails>)SetUIValuesToObject(ControlsEnum.EXPENSEDETAIL);
                                if (soExpenseDetailsList != null && soExpenseDetailsList.Count > 0)
                                {
                                    ExpenseHeaderSession.OrderDetail = soExpenseDetailsList;
                                    //SetDetailTax(TempExpenseHeaderSession);
                                    hdfApplyTax.Value = "1";//For Avoiding Formula Calculation
                                    SetDetailTax(ExpenseHeaderSession);
                                    SetSubTotal();
                                    SetHdrTax();
                                    expenseHeaderObj = ExpenseHeaderSession;
                                    SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                                    ResetForm(ControlsEnum.EXPENSEDETAIL);
                                }
                                hdfAddItem.Value = "0";
                                grdItemDetails.Focus();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPartyNoCheckConfirming", "ShowPartyNoCheckConfirming();", true);
                            }
                        }
                        break;
                    #endregion
                    #region Remove Item
                    case ActionsEnum.REMOVEITEM:
                        if (ExpenseHeaderSession.OrderDetail != null && ExpenseHeaderSession.OrderDetail.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                ExpenseHeaderSession.OrderDetail = ExpenseHeaderSession.OrderDetail.Where(row => CurrSlNo != row.VID_SL_NO).ToList();
                                SetDetailTax(ExpenseHeaderSession);
                                SetSubTotal();
                                SetHdrTax();
                                expenseHeaderObj = ExpenseHeaderSession;
                                SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            }
                        }
                        ResetForm(ControlsEnum.EXPENSEDETAIL);
                        break;
                    #endregion
                    #region Edit Item
                    case ActionsEnum.EDITITEM:
                        ResetForm(ControlsEnum.EXPENSEDETAIL);
                        if (hdfIsShowAlert.Value == "0")
                            btnAlert.Visible = false;
                        if (ExpenseHeaderSession.OrderDetail != null && ExpenseHeaderSession.OrderDetail.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                soExpenseDetailsObj = ExpenseHeaderSession.OrderDetail.SingleOrDefault(row => CurrSlNo == row.VID_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDITEM);
                            }
                        }
                        hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                        txtDesc.Focus();
                        break;
                    #endregion
                    #region Clear Item
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.EXPENSEDETAIL);
                        break;
                    #endregion
                    #region save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (ExpenseHeaderSession.OrderDetail == null || ExpenseHeaderSession.OrderDetail.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            expenseHeaderObj = new POInvoiceHeader();
                            expenseHeaderObj = (POInvoiceHeader)SetUIValuesToObject(ControlsEnum.EXPENSEHEADER);



                            expenseHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                            if (expenseHeaderObj != null && expenseHeaderObj.OrderDetail != null)
                            {
                                #region Party Inovoice Duplicate No Checking
                                var grpRefNos = expenseHeaderObj.OrderDetail.GroupBy(i => i.VID_REF_NO);
                                if (grpRefNos != null && grpRefNos.Count() != expenseHeaderObj.OrderDetail.Count() && hdfPartyNoCheck.Value == "0")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPartyNoCheckConfirming", "InvoiceNoDheckConfirm('Save');", true);
                                    return;
                                }
                                #endregion

                                //if (SettlementTotal != (Convert.ToDecimal(txtHdrTotal.Text.Trim()) + Math.Abs((Convert.ToDecimal(txtRefundAmount.Text.Trim())))))
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("Err_SettlementNotTally").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                //    return;
                                //}
                                string xmlDoc = CommonFunctions.XmlSerialize<POInvoiceHeader>(expenseHeaderObj);
                                // save Process Control inspection details
                                string invNumber = string.Empty;
                                //result = BusinessLogic.POInvoicing.POInvoiceBL.SavePOInvoiceHeader(xmlDoc);
                                result = BusinessLogic.POInvoicing.POInvoiceBL.SaveExpSettlementInvoiceWkf(xmlDoc, out invNumber);
                                if (result > 0) // Success !  redirect to listing page
                                {
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

                                        foreach (POInvoiceUploads obj in POUploadList)
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

                                    // Show Save Message and redired to listing page                                        
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetLocalResourceObject("ExpenseInvoice").ToString());//Resources.PageNameRes.ExpensesSettlement
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.EXPENSELIST);
                                    GetFieldValues(ControlsEnum.EXPENSELIST);
                                    SetFieldValues(ControlsEnum.EXPENSELIST);
                                }
                                else
                                {
                                    if (result == (int)DbSaveStatus.SQLERROR)
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                                    {
                                        litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + GetLocalResourceObject("RefNoExist").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.INVNOEXISTS)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("InvNoExist").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ExpensesSettlement);
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                                //else
                                //{
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                //}
                            }

                        }
                        break;
                    #endregion
                    #region Expense Advances
                    case ActionsEnum.EXPENSEADVANCES:
                        if (hdfIsNewParty.Value == "1")
                        {
                            GetFieldValues(ControlsEnum.EXPENSEADVANCES);
                            SetFieldValues(ControlsEnum.EXPENSEADVANCES);
                            hdfIsNewParty.Value = "0";
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowExpenseAdvances('" + GetLocalResourceObject("ExpenseAdvances").ToString() + "');", true);
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
                            EditTempExpenseHeaderSession = TempExpenseHeaderSession;
                            soExpenseDetailsList = EditTempExpenseHeaderSession.OrderDetail;

                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            soExpenseDetailsObj = EditTempExpenseHeaderSession.OrderDetail == null ? null :
                                    EditTempExpenseHeaderSession.OrderDetail.SingleOrDefault(ctr => ctr.VID_PK == SelectedDtlPK && ctr.VID_SL_NO == CurrSlNo);
                            if (soExpenseDetailsObj == null)
                            {
                                soExpenseDetailsList = (List<POInvoiceDetails>)SetUIValuesToObject(ControlsEnum.EXPENSEDETAIL);
                                EditTempExpenseHeaderSession.OrderDetail = soExpenseDetailsList;
                            }

                            if (soExpenseDetailsList != null && soExpenseDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) :
                                    string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) :
                                    (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);

                                if (EditTempExpenseHeaderSession != null)
                                {
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;

                                            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtPopupAmount.Text = string.Empty;
                                        }
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                        {
                                            //txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            //txtPopupAmount.Enabled = false;
                                            txtPopupOther.Enabled = false;
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
                            EditTempExpenseHeaderSession = TempExpenseHeaderSession;
                            soExpenseDetailsList = EditTempExpenseHeaderSession.OrderDetail;
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);

                            soExpenseDetailsObj = EditTempExpenseHeaderSession.OrderDetail == null ? null :
                                    EditTempExpenseHeaderSession.OrderDetail.SingleOrDefault(ctr => ctr.VID_PK == SelectedDtlPK && ctr.VID_SL_NO == CurrSlNo);
                            if (soExpenseDetailsObj == null)
                            {
                                soExpenseDetailsList = (List<POInvoiceDetails>)SetUIValuesToObject(ControlsEnum.EXPENSEDETAIL);
                                EditTempExpenseHeaderSession.OrderDetail = soExpenseDetailsList;
                            }
                            if (soExpenseDetailsList != null && soExpenseDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                                hdfTaxFormula.Value = string.Empty;

                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);
                                if (EditTempExpenseHeaderSession != null)
                                {
                                    IsHeaderTax = false;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                                    GetFieldValues(ControlsEnum.TAXTYPES);
                                    SetFieldValues(ControlsEnum.TAXTYPES);
                                    if (ddlPopupTaxType.Items.Count > 0)
                                    {
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                        {
                                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                            GetFieldValues(ControlsEnum.TAXTYPES);
                                            TaxPK = 0;
                                            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                            {
                                                string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                                hdfTaxFormula.Value = taxFormula;
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                                SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                            }
                                        }
                                        else
                                        {
                                            SelectedTaxText = Resources.Report.Custom;
                                            txtPopupAmount.Text = string.Empty;
                                        }
                                        txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                        {
                                            txtPopupAmount.Enabled = true;
                                            txtPopupOther.Enabled = true;
                                        }
                                        else
                                        {
                                            txtPopupAmount.Enabled = false;
                                            txtPopupOther.Enabled = false;
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
                        if (ExpenseHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.EXPENSEDETAILTAX);
                            //BindDefaultUOM();
                            TempExpenseHeaderSession = ExpenseHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = grdItemDetails.FooterRow == null ? null : (grdItemDetails.FooterRow.FindControl("lblSubTotalFooter") as Label);
                            if (lblSubTotal != null)
                            {
                                double taxable = Convert.ToDouble(ExpenseHeaderSession.IVH_AMOUNT_TC) - ExpenseHeaderSession.IVH_DISCOUNT_TC;
                                txtPopupItemAmount.Text = taxable > 0 ? taxable.ToString(hdfCurrencyFormat.Value) : ((double)0).ToString(hdfCurrencyFormat.Value);

                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        //txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        //txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region DISCHEADER
                    case ActionsEnum.DISCHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (ExpenseHeaderSession != null)
                        {
                            ResetForm(ControlsEnum.EXPENSEDETAIL);
                            TempExpenseHeaderSession = ExpenseHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = grdItemDetails.FooterRow == null ? null : (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            if (lblSubTotal != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(lblSubTotal.Text.Replace(",", "").Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(lblSubTotal.Text.Replace(",", "")).ToString(hdfCurrencyFormat.Value);
                                if (ddlPopupTaxType.Items.Count > 0)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                            SelectedTaxText = HttpUtility.HtmlDecode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        SelectedTaxText = Resources.Report.Custom;
                                        txtPopupAmount.Text = string.Empty;
                                    }
                                    txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                    {
                                        txtPopupAmount.Enabled = true;
                                        txtPopupOther.Enabled = true;
                                    }
                                    else
                                    {
                                        txtPopupAmount.Enabled = false;
                                        txtPopupOther.Enabled = false;
                                    }
                                }
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("DiscountDetails").ToString() + "','600','300');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
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
                            ExpenseHeaderSession = TempExpenseHeaderSession;
                            SetSubTotal();
                            SetHdrTax();
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
                            TempExpenseHeaderSession = EditTempExpenseHeaderSession;
                            //ExpenseHeaderSession = TempExpenseHeaderSession;
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            soExpenseDetailsObj = TempExpenseHeaderSession.OrderDetail.SingleOrDefault(crt => crt.VID_PK == SelectedDtlPK
                            && crt.VID_SL_NO == CurrSlNo);
                            SetDetailTax(TempExpenseHeaderSession);
                        }
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region TAXADD
                    case ActionsEnum.TAXADD:

                        //SelectedDtlPK
                        //SelectedCusItemPK
                        //SelectedItemPK


                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (IsHeaderTax ? TempExpenseHeaderSession != null : EditTempExpenseHeaderSession != null)
                        //if (EditTempExpenseHeaderSession != null)
                        {
                            expenseHeaderObj = IsHeaderTax ? TempExpenseHeaderSession : EditTempExpenseHeaderSession;
                            //expenseHeaderObj = EditTempExpenseHeaderSession;
                            tempInvTaxSplitObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    tempInvTaxSplitObj = expenseHeaderObj.TaxHdr == null ? null :
                                        expenseHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.VTL_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && ctr.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempInvTaxSplitObj = expenseHeaderObj.TaxHdr == null ? null :
                                        expenseHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.VTL_NAME == txtPopupOther.Text.Trim() && ctr.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                soExpenseDetailsObj = expenseHeaderObj.OrderDetail == null ? null :
                                    expenseHeaderObj.OrderDetail.SingleOrDefault(ctr => ctr.VID_PK == SelectedDtlPK
                                    && ctr.VID_SL_NO == CurrSlNo);

                                if (soExpenseDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempInvTaxSplitObj = soExpenseDetailsObj.TaxDtl == null ? null :
                                            soExpenseDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempInvTaxSplitObj = soExpenseDetailsObj.TaxDtl == null ? null :
                                            soExpenseDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_NAME == txtPopupOther.Text.Trim() && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempInvTaxSplitObj == null)
                            {
                                taxHdrList = new List<POInvoiceTaxHdr>();

                                soInvTaxHdrObj = new POInvoiceTaxHdr();
                                try
                                {
                                    soInvTaxHdrObj.VTL_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    soInvTaxHdrObj.VTL_SL_NO = CurrSlNo;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        soInvTaxHdrObj.VTL_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    }
                                    soInvTaxHdrObj.VTL_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    soInvTaxHdrObj.VTL_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text.Trim());
                                    soInvTaxHdrObj.VTL_PK = 0;
                                    //quotationTaxHdrObj.VTL_TAX_CATEGORY_TEXT = "Tax";
                                    soInvTaxHdrObj.VTL_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    soInvTaxHdrObj.VTL_TYPE = 1;
                                    soInvTaxHdrObj.VTL_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    if (IsHeaderTax)
                                    {
                                        if (soInvTaxHdrObj.VTL_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(expenseHeaderObj.IVH_AMOUNT_TC);
                                            currentTotal = expenseHeaderObj.TaxHdr == null ? 0 :
                                                expenseHeaderObj.TaxHdr.Where(htx => htx.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.VTL_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(soInvTaxHdrObj.VTL_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = soInvTaxHdrObj.VTL_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                if (expenseHeaderObj.TaxHdr == null)
                                                    taxHdrList = new List<POInvoiceTaxHdr>();
                                                else
                                                    taxHdrList = expenseHeaderObj.TaxHdr.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                expenseHeaderObj.TaxHdr = taxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            if (expenseHeaderObj.TaxHdr == null)
                                                taxHdrList = new List<POInvoiceTaxHdr>();
                                            else
                                                taxHdrList = expenseHeaderObj.TaxHdr.ToList();
                                            taxHdrList.Add(soInvTaxHdrObj);
                                            expenseHeaderObj.TaxHdr = taxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        soExpenseDetailsObj = expenseHeaderObj.OrderDetail == null ? null :
                                            expenseHeaderObj.OrderDetail.SingleOrDefault(item => item.VID_PK == SelectedDtlPK
                                            && item.VID_SL_NO == CurrSlNo);
                                        if (soExpenseDetailsObj != null)
                                        {
                                            if (soInvTaxHdrObj.VTL_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = soExpenseDetailsObj.VID_AMOUNT;
                                                currentTotal = soExpenseDetailsObj.TaxDtl == null ? 0 :
                                                    soExpenseDetailsObj.TaxDtl.Where(dtx => dtx.VTL_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.VTL_TAX_AMT);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(soInvTaxHdrObj.VTL_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = soInvTaxHdrObj.VTL_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    taxHdrList = soExpenseDetailsObj.TaxDtl == null ? new List<POInvoiceTaxHdr>() : soExpenseDetailsObj.TaxDtl.ToList();
                                                    taxHdrList.Add(soInvTaxHdrObj);
                                                    expenseHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == SelectedDtlPK
                                                        && rfq.VID_SL_NO == CurrSlNo).TaxDtl = taxHdrList;
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                taxHdrList = soExpenseDetailsObj.TaxDtl == null ? new List<POInvoiceTaxHdr>() : soExpenseDetailsObj.TaxDtl.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                expenseHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == SelectedDtlPK
                                                    && rfq.VID_SL_NO == CurrSlNo).TaxDtl = taxHdrList;
                                            }
                                        }
                                    }
                                    if (IsHeaderTax)
                                        TempExpenseHeaderSession = expenseHeaderObj;
                                    else
                                        EditTempExpenseHeaderSession = expenseHeaderObj;
                                    //EditTempExpenseHeaderSession = expenseHeaderObj;
                                    SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                                }
                            }
                            else
                            {
                                errorTaxAdd = true;
                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    //txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    //txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                                if (!errorTaxAdd && !errorTaxAmount)
                                {
                                    txtPopupAmount.Text = string.Empty;
                                    txtPopupOther.Text = string.Empty;
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
                        if (IsHeaderTax ? TempExpenseHeaderSession != null : EditTempExpenseHeaderSession != null)
                        {
                            expenseHeaderObj = IsHeaderTax ? TempExpenseHeaderSession : EditTempExpenseHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                taxHdrList = new List<POInvoiceTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempInvTaxSplitObj = expenseHeaderObj.TaxHdr == null ? null :
                                            expenseHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.VTL_TAX == taxPK && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempInvTaxSplitObj = expenseHeaderObj.TaxHdr == null ? null :
                                                expenseHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.VTL_NAME == hdfTaxName.Value && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempInvTaxSplitObj != null)
                                    {
                                        taxHdrList = expenseHeaderObj.TaxHdr.ToList();
                                        taxHdrList.Remove(tempInvTaxSplitObj);
                                        expenseHeaderObj.TaxHdr = taxHdrList;
                                    }
                                }
                                else
                                {
                                    soExpenseDetailsObj = expenseHeaderObj.OrderDetail == null ? null :
                                        expenseHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == SelectedDtlPK
                                        && rfq.VID_SL_NO == CurrSlNo);
                                    if (soExpenseDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempInvTaxSplitObj = soExpenseDetailsObj.TaxDtl == null ? null :
                                                soExpenseDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_TAX == taxPK && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempInvTaxSplitObj = soExpenseDetailsObj.TaxDtl == null ? null :
                                                    soExpenseDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.VTL_NAME == hdfTaxName.Value && rfq.VTL_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        soExpenseDetailsObj = expenseHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == SelectedDtlPK
                                            && rfq.VID_SL_NO == CurrSlNo);
                                        if (soExpenseDetailsObj != null && soExpenseDetailsObj.TaxDtl != null)
                                        {
                                            taxHdrList = soExpenseDetailsObj.TaxDtl.ToList();
                                            taxHdrList.Remove(tempInvTaxSplitObj);
                                            expenseHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.VID_PK == SelectedDtlPK
                                                && rfq.VID_SL_NO == CurrSlNo).TaxDtl = taxHdrList;
                                        }
                                    }
                                }
                                if (IsHeaderTax)
                                    TempExpenseHeaderSession = expenseHeaderObj;
                                else
                                    EditTempExpenseHeaderSession = expenseHeaderObj;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    //txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                        GetFieldValues(ControlsEnum.TAXTYPES);
                                        TaxPK = 0;
                                        if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                        {
                                            string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                            hdfTaxFormula.Value = taxFormula;
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString(hdfCurrencyFormat.Value);
                                            SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    //txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                        break;
                    #endregion
                    #region TAXTYPECHANGED
                    case ActionsEnum.TAXTYPECHANGED:
                        if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                        {
                            TaxPK = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            TaxPK = 0;
                            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                            {
                                string taxFormula = dtTaxDetails.Rows[0]["TAX_FORMULA"].ToString();
                                hdfTaxFormula.Value = taxFormula;
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula));
                                SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                // Enable amount textbox in TAX POPUP & disable it in DISCOUNT POPUP
                                if (hdfTaxCategory.Value == ((int)TaxType.Tax).ToString())
                                {
                                    txtPopupAmount.Enabled = true;
                                }
                                else
                                {
                                    txtPopupAmount.Enabled = false;
                                }
                                //txtPopupAmount.Enabled = false;
                                txtPopupOther.Enabled = false;
                            }
                        }
                        else if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                        {
                            hdfTaxFormula.Value = string.Empty;
                            txtPopupAmount.Text = string.Empty;
                            SelectedTaxText = Resources.Report.Custom;
                            txtPopupOther.Text = string.Empty;
                            txtPopupAmount.Enabled = true;
                            txtPopupOther.Enabled = true;
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                        break;
                    #endregion
                    #region CALCULATEDTLTAX
                    case ActionsEnum.CALCULATEDTLTAX:
                        //SetDetailTax(TempExpenseHeaderSession);
                        ////SetHdrTax();
                        //ResetForm(ControlsEnum.TAXPOPUPGRID);
                        SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                        if (TempExpenseHeaderSession.OrderDetail != null)
                            soExpenseDetailsObj = TempExpenseHeaderSession.OrderDetail.SingleOrDefault(crt => crt.VID_PK == SelectedDtlPK
                            && crt.VID_SL_NO == CurrSlNo);
                        if (soExpenseDetailsObj != null)
                        {
                            soExpenseDetailsObj.VID_QTY_INVOICED = Math.Round(Convert.ToDouble(txtQty.Text), Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()));
                            soExpenseDetailsObj.VID_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));
                            soExpenseDetailsObj.VID_AMOUNT = !string.IsNullOrEmpty(txtAmount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;

                            SetDetailTax(TempExpenseHeaderSession);
                        }
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        IsHeaderTax = false;
                        IsEditMode = false;
                        txtRate.Focus();
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.EXPENSELIST);
                        CurrPK = 0;
                        hdfExpenseNumberPK.Value = "";
                        FillProcessID(1);
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        TempExpenseHeaderSession = new POInvoiceHeader();
                        EditTempExpenseHeaderSession = new POInvoiceHeader();
                        ExpenseHeaderSession = new POInvoiceHeader();
                        EntryStatus = EntryStatus.NEWMODE;
                        txtExpenseDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                        AST_DOC_MODE.Value = GetDOCMODE();
                        AST_CODE.Value = ApplicationType.ES;
                        //lblExpenseNo.Text = hdfExpenseNo.Value == string.Empty ? "[NEW]" : hdfExpenseNo.Value;
                        lblExpenseNo.Text = hdfExpenseNo.Value = "[NEW]";
                        hdfAppType.Value = ApplicationType.ES;
                        hdfAppSubType.Value = string.Empty;
                        hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        SetFieldValues(ControlsEnum.UPLOADEDFILES);
                        hdfIsInvCancelled.Value = "0";
                        txtExpenseDate.Focus();
                        if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                        {
                            lblCompanyView.Visible = true;
                            ddlCompanyView.Visible = true;
                            ddlCompanyView.Enabled = true;
                            GetFieldValues(ControlsEnum.COMPANY);
                            SetFieldValues(ControlsEnum.COMPANY);
                        }
                        else
                        {
                            lblCompanyView.Visible = false;
                            ddlCompanyView.Visible = false;
                        }
                        break;
                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdExpenseList.Rows)
                        {
                            CheckBox chkInvselect;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            HiddenField hdfDept;
                            int dept;
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExpenseID")).Value);
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                Group = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfGroup")).Value);
                                ////

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }

                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                hdfIsInvCancelled.Value = hdfDelStatus.Value;

                                break;
                            }
                        }
                        if (hdfIsShowAlert.Value == "0")
                            btnAlert.Visible = false;
                        if (Group == (int)POInvoiceGroup.AgtInvoice)
                        {
                            Response.Redirect(Resources.PageURL.AgtComm + "?Type=" + (int)POInvoiceGroup.AgtInvoice + "&fromPK=" + CurrPK);
                        }
                        else
                        {

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
                                    //EntryStatus = EntryStatus.VIEWMODE;

                                }
                                ucrWrkf.ViewAction();
                                ModifiedDatePnl.Visible = true;
                                GetFieldValues(ControlsEnum.EXPENSEHEADER);
                                SetFieldValues(ControlsEnum.EXPENSEHEADER);
                                SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                TempExpenseHeaderSession = ExpenseHeaderSession;
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Expense").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdExpenseList.Rows)
                        {
                            CheckBox chkInvselect;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            HiddenField hdfDept;
                            int dept;
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExpenseID")).Value);
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                Group = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfGroup")).Value);
                                ////
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }

                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                hdfIsInvCancelled.Value = hdfDelStatus.Value;
                                break;
                            }
                        }
                        if (hdfIsShowAlert.Value == "0")
                            btnAlert.Visible = false;
                        if (Group == (int)POInvoiceGroup.AgtInvoice)
                        {
                            Response.Redirect(Resources.PageURL.AgtComm + "?Type=" + (int)POInvoiceGroup.AgtInvoice + "&fromPK=" + CurrPK);
                        }
                        else if (bIsChecked)
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
                            GetFieldValues(ControlsEnum.EXPENSEHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            TempExpenseHeaderSession = ExpenseHeaderSession;
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Expense").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.POInvoicing.POInvoiceBL.DeletePOInvoiceDetails(CurrPK, LastModifiedTime, ApplicationType.ES, currentUser.PKUser.ToString());
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ExpensesSettlement);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.EXPENSELIST);
                                GetFieldValues(ControlsEnum.EXPENSELIST);
                                SetFieldValues(ControlsEnum.EXPENSELIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.EXPENSELIST);
                                    GetFieldValues(ControlsEnum.EXPENSELIST);
                                    SetFieldValues(ControlsEnum.EXPENSELIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.EXPENSELIST);
                                    GetFieldValues(ControlsEnum.EXPENSELIST);
                                    SetFieldValues(ControlsEnum.EXPENSELIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ExpensesSettlement);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        SelectedInvoicesInfoLst = null;
                        ResetForm(ControlsEnum.EXPENSELIST);
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        break;
                    #endregion
                    #region Expense List
                    case ActionsEnum.INVOICELIST:
                        ResetForm(ControlsEnum.EXPENSELIST);
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdExpenseList.Rows)
                        {
                            CheckBox chkInvselect;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            HiddenField hdfDept;
                            int dept;
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExpenseID")).Value);
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                Group = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfGroup")).Value);
                                ////
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }
                        if (Group == (int)POInvoiceGroup.AgtInvoice)
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_CancelExpInv").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            //
                            //Response.Redirect(Resources.PageURL.AgtComm + "?Type=" + (int)POInvoiceGroup.AgtInvoice + "&fromPK=" + CurrPK);
                        }
                        else if (bIsChecked)
                        {
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.EXPENSEHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            TempExpenseHeaderSession = ExpenseHeaderSession;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrWrkf.ViewType = 1;
                                //btnSave.Visible = true;
                                //divbtnSavePaymentSplit.Visible = true;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;                           
                                //btnSave.Visible = false;
                                //divbtnSavePaymentSplit.Visible = false;
                            }
                            //btnPrint.Visible = true;
                            ucrWrkf.ViewAction();
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Expense").ToString();
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
                    #region Expense Details
                    case ActionsEnum.INVOICEDETAIL:
                        foreach (GridViewRow grdrow in grdExpenseList.Rows)
                        {
                            CheckBox chkInvselect;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            HiddenField hdfDept;
                            int dept;
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExpenseID")).Value);
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                Group = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfGroup")).Value);
                                ////
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }

                                HiddenField hdfDelStatus = (HiddenField)grdrow.FindControl("hdfDelStatus");
                                hdfIsInvCancelled.Value = hdfDelStatus.Value;
                                break;
                            }
                        }
                        if (hdfIsShowAlert.Value == "0")
                            btnAlert.Visible = false;
                        if (Group == (int)POInvoiceGroup.AgtInvoice)
                        {
                            Response.Redirect(Resources.PageURL.AgtComm + "?Type=" + (int)POInvoiceGroup.AgtInvoice + "&fromPK=" + CurrPK);
                        }
                        else if (bIsChecked)
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
                                //EntryStatus = EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();
                            }
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.EXPENSEHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            TempExpenseHeaderSession = ExpenseHeaderSession;
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Expense").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        if (ExpenseHeaderSession.OrderDetail == null || ExpenseHeaderSession.OrderDetail.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        { //Show WorkFlow Popup
                            hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        break;
                    #endregion
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region WRKSubmit
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (ExpenseHeaderSession.OrderDetail == null || ExpenseHeaderSession.OrderDetail.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {
                            isCancelled = false;
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                expenseHeaderObj = new POInvoiceHeader();
                                expenseHeaderObj = (POInvoiceHeader)SetUIValuesToObject(ControlsEnum.EXPENSEHEADER);
                                //if (SettlementTotal != (Convert.ToDecimal(txtHdrTotal.Text.Trim()) + (Convert.ToDecimal(txtRefundAmount.Text.Trim()))))
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("Err_SettlementNotTally").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                //    return;
                                //}
                                //if (SettlementTotal != (Convert.ToDecimal(txtHdrTotal.Text.Trim()) + Math.Abs((Convert.ToDecimal(txtRefundAmount.Text.Trim())))))
                                //{
                                //    litErrorMsg.Text = GetLocalResourceObject("Err_SettlementNotTally").ToString();
                                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                //    return;
                                //}
                                if (hdfExchangeRate.Value != "-1")
                                {
                                    expenseHeaderObj.WKF_FLAG = 1;
                                    if (expenseHeaderObj != null && expenseHeaderObj.OrderDetail != null)
                                    {
                                        #region Party Inovoice Duplicate No Checking
                                        var grpRefNos = expenseHeaderObj.OrderDetail.GroupBy(i => i.VID_REF_NO);
                                        if (grpRefNos != null && grpRefNos.Count() != expenseHeaderObj.OrderDetail.Count() && hdfPartyNoCheck.Value == "0")
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPartyNoCheckConfirming", "InvoiceNoDheckConfirm('Submit');", true);
                                            return;
                                        }
                                        #endregion
                                        expenseHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                        SaveTransaction(expenseHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));

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
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.ES))
                                {
                                    //ucrWrkf.ApplicationID = (int)CurrPK;
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_EI_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    SelectedInvoices = null;
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.EXPENSELIST);
                                    GetFieldValues(ControlsEnum.EXPENSELIST);
                                    SetFieldValues(ControlsEnum.EXPENSELIST);
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        SelectedInvoicesInfoLst = null;
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        break;
                    #endregion
                    #region Pick for Paying
                    case ActionsEnum.PICKFORPAYMENT:
                        ResetForm(ControlsEnum.RESETPAYMENT);
                        SetUIValuesToObject(ControlsEnum.PICKFORPAYMENT);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                        break;
                    #endregion
                    #region Pick Inv & for Cr/Dr. Note
                    case ActionsEnum.PICKFORCRDRNOTE:
                        Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                        SetUIValuesToObject(ControlsEnum.PICKFORCRDRNOTE);
                        break;
                    #endregion
                    #region Reset
                    case ActionsEnum.RESET:
                        SelectedVendors = 0;
                        SelectedCurrency = 0;
                        SelectedInvoices = null;
                        SelectedInvoicesCrDr = null;
                        SelectedInvoicesInfoLst = null;
                        btnPickForPayment.Text = GetLocalResourceObject("PickPoForPayment").ToString();
                        //Resetting Color
                        hdfSelectedItemPk.Value = "0";
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        finExpenseVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        finExpenseVndHdrObj.IVH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.EXPENSEHEADER);
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
                        FillProcessID(1);
                        ResetForm(ControlsEnum.EXPENSELIST);
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.EXPENSELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                poExpenseServiceClient = new POInvoiceService();
                                poExpenseServiceClient = CommonFunctions.InitiateClient(poExpenseServiceClient);
                                result = (int)poExpenseServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                poExpenseServiceClient = new POInvoiceService();
                                poExpenseServiceClient = CommonFunctions.InitiateClient(poExpenseServiceClient);
                                result = (int)poExpenseServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);

                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    ResetForm(ControlsEnum.EXPENSELIST);
                        //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InboxURL));
                        //}
                        //else
                        //{
                        ResetForm(ControlsEnum.EXPENSELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        //}
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                poExpenseServiceClient = new POInvoiceService();
                                poExpenseServiceClient = CommonFunctions.InitiateClient(poExpenseServiceClient);
                                result = (int)poExpenseServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                poExpenseServiceClient = new POInvoiceService();
                                poExpenseServiceClient = CommonFunctions.InitiateClient(poExpenseServiceClient);
                                result = (int)poExpenseServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.EXPENSELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.EXPENSELIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                        break;
                    #endregion
                    #region Alert
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = ApplicationType.ES;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblExpenseNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtExpenseDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtExpenseDate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        ucrAlert.TypePartyName = txtVendor.Text;
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion
                    #region CHANGETYPE
                    case ActionsEnum.CHANGETYPE:
                        txtBranchCode.Text = string.Empty;
                        SetBranchCodeVisibility();
                        txtAddressType.Focus();
                        break;
                    #endregion
                    #region CHANGE HDR VENDOR TYPE
                    case ActionsEnum.CHANGE:
                        txtHdrBranchCode.Text = string.Empty;
                        SetHDRBranchCodeVisibility();

                        break;
                    #endregion
                    #region VENDORTEXTCHANGED
                    case ActionsEnum.VENDORTEXTCHANGED:
                        hdfAddressType.Value = string.Empty;
                        hdfVendorDtl.Value = string.Empty;
                        txtBranchCode.Enabled = true;
                        vrfBranchCode.Enabled = true;
                        chkHO.Checked = false;
                        //txtBranchCode.CssClass = "";
                        SetHDRBranchCodeVisibility();
                        SetBranchCodeVisibility();
                        if (hdfVendorDtl.Value == "")
                        {
                            txtVatTaxId.Text = string.Empty;
                            //txtBranchCode.Text = string.Empty;

                            chkHO.Checked = false;
                            txtBranchCode.Enabled = true;
                            vrfBranchCode.Enabled = true;
                            //txtBranchCode.CssClass = "medium";
                        }
                        txtVendor.Focus();
                        break;
                    #endregion

                    #region CLEARDETAIL
                    case ActionsEnum.CLEARDETAIL:
                        // hdfAddressType.Value = string.Empty;
                        txtBranchCode.Enabled = true;
                        vrfBranchCode.Enabled = true;
                        chkHO.Checked = false;
                        //txtBranchCode.CssClass = "";
                        SetBranchCodeVisibility();

                        if (hdfVendorDtl.Value == "")
                        {
                            txtVatTaxId.Text = string.Empty;
                            txtBranchCode.Text = string.Empty;

                            chkHO.Checked = false;
                            txtBranchCode.Enabled = true;
                            vrfBranchCode.Enabled = true;
                            //txtBranchCode.CssClass = "medium";
                        }

                        break;
                    #endregion
                    #region CHECKEDCHANGED
                    case ActionsEnum.CHECKEDCHANGED:
                        if (chkHO.Checked)
                        {
                            //txtBranchCode.Enabled = false;
                            txtBranchCode.Text = "";
                            vrfBranchCode.Enabled = false;
                            // txtBranchCode.CssClass = "medium input-disabled";
                        }
                        else
                        {
                            txtBranchCode.Enabled = true;
                            vrfBranchCode.Enabled = true;
                            //txtBranchCode.CssClass = "";
                            //txtBranchCode.CssClass = "medium";
                        }

                        break;
                    #endregion
                    #region CHECKED CHANGED HDR
                    case ActionsEnum.CHECKEDCHANGEDHDR:
                        if (HdrchkHO.Checked)
                        {
                            //txtBranchCode.Enabled = false;
                            txtHdrBranchCode.Text = "";
                            vrfHdrBranchCode.Enabled = false;
                            txtBranchCode.Text = "";
                            vrfBranchCode.Enabled = false;
                        }
                        else
                        {
                            txtHdrBranchCode.Enabled = true;
                            vrfHdrBranchCode.Enabled = true;
                            txtHdrBranchCode.CssClass = "";
                            txtHdrBranchCode.CssClass = "medium";
                            txtBranchCode.Enabled = true;
                            vrfBranchCode.Enabled = true;
                            //txtBranchCode.CssClass = "";
                            //txtBranchCode.CssClass = "medium";
                        }

                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.ES + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion
                    #region PRINTLISTING
                    case ActionsEnum.PRINTLISTING:
                        int Pk = 0;
                        HiddenField hdfType;
                        foreach (GridViewRow grdrow in grdExpenseList.Rows)
                        {
                            CheckBox chkInvselect;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                Pk = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfExpenseID")).Value);
                                hdfType = (HiddenField)grdrow.FindControl("hdfGroup");

                                if (hdfType.Value != string.Empty)
                                {
                                    if (Convert.ToInt32(hdfType.Value) == (int)POInvoiceGroup.ExpenseSettilement)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + Pk.ToString() + "&APPTYPE=" + ApplicationType.ES + "&APPSUBTYPE=") + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfType.Value) == (int)POInvoiceGroup.AgtInvoice)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + Pk.ToString() + "&APPTYPE=" + ApplicationType.ACI + "&APPSUBTYPE=0") + "');", true);

                                    }
                                }
                                break;
                            }
                        }
                        break;
                    #endregion

                    #region AMOUNTDETAILS
                    case ActionsEnum.AMOUNTDETAILS:
                        HiddenField hdfInvoiceID = (HiddenField)((GridViewRow)((LinkButton)(sender)).Parent.Parent).FindControl("hdfExpenseID");
                        long.TryParse(hdfInvoiceID.Value, out InvoicePk);
                        GetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        SetFieldValues(ControlsEnum.AMOUNTDETAILS);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalAmountSplit", "$(document).ready(function(){CalculateTotalAmountSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divPaidAmntSplitup]','" + GetLocalResourceObject("TrxDetails").ToString() + "','600','250');", true);
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                poExpenseServiceClient = null;
                CommonServiceClient = null;

            }
        }

        #region Workflow Submit
        /// <summary>
        /// Save and submit With workflow 
        /// </summary>
        /// <param name="objSalPayment"></param>
        private void SaveTransaction(POInvoiceHeader objExpence, int workflowFlag)
        {
            int? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (objExpence == null)
                objExpence = new POInvoiceHeader();
            #region Transaction Log and Application Code
            objExpence.ATL_APP_TYPE = ApplicationType.ES;
            objExpence.APT_CODE = ApplicationType.ES;
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            objExpence.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            objExpence.WKF_APPLICATION = CurrPK;
            objExpence.WKF_COMMENTS = wkfDetails.Comments;
            objExpence.WKF_TRX_FLAG = workflowFlag;
            objExpence.WKF_PROCESS = wkfDetails.ProcessID;
            objExpence.WKF_REFERENCE = wkfDetails.ReferenceID;
            objExpence.WKF_TASK = wkfDetails.TaskID;
            objExpence.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            if (isCancelled)
                objExpence.ATL_ACTION = (byte)LogAction.CANCEL;
            else
                objExpence.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string xmlDoc = CommonFunctions.XmlSerialize<POInvoiceHeader>(objExpence);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
            string invoiceNumber = string.Empty;
            result = BusinessLogic.POInvoicing.POInvoiceBL.SaveExpSettlementInvoiceWkf(xmlDoc, out invoiceNumber);
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

                        foreach (POInvoiceUploads obj in POUploadList)
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
                    ResetForm(ControlsEnum.EXPENSELIST);

                    #endregion
                }
                #endregion
                if (result.HasValue && result.Value > 0)
                {
                    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                    //Show Save success message and reset Contract Entry
                    if (isCancelled)
                    {
                        FillProcessID(1);
                        litErrorMsg.Text = Resources.Messages.Msg_Cancelled_Success;
                        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.CANCEL;
                    }
                    else
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.SUBMIT;
                    }
                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                    TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                    WrkfComments.Text = "";
                    CurrPK = (int)result;
                    GetFieldValues(ControlsEnum.EXPENSEHEADER);
                    if (string.IsNullOrEmpty(invoiceNumber))
                        invoiceNumber = lblExpenseNo.Text.Trim();
                    object[] args = new object[2];
                    args[0] = GetLocalResourceObject("ExpenseInvoice").ToString(); //Resources.PageNameRes.ExpensesSettlement;
                    args[1] = invoiceNumber;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm(ControlsEnum.EXPENSELIST);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm(ControlsEnum.EXPENSELIST);
                        GetFieldValues(ControlsEnum.EXPENSELIST);
                        SetFieldValues(ControlsEnum.EXPENSELIST);
                    }
                    #endregion
                }
                ucrWrkf.ApplicationID = result.Value;
            }
            else
            {
                if (result == (int)DbSaveStatus.SQLERROR)
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CONCURRENCY)
                {
                    litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.REFNOEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.ExpensesSettlement + " " + GetLocalResourceObject("RefNoExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                else if (result == (int)DbSaveStatus.INVNOEXISTS)
                {
                    litErrorMsg.Text = GetLocalResourceObject("InvNoExist").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                    + "','" + Resources.ErpRes.Information + "','" + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ExpensesSettlement);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
                return;
            }

        }
        #endregion
        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            Label lblItemTotalQty;
            Label lblItemTotalAmount;
            Label lblDiscountTotal;
            Label lblTaxTotal;
            Label lblSubTotalFooter;

            try
            {
                if (((GridView)sender).ID == "grdExpenseList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;
                        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;
                        if (dtExpenseList != null)
                        {
                            if (dtExpenseList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString() != "" || dtExpenseList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString() != string.Empty)
                            {
                                imgPosted.CssClass = dtExpenseList.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString();
                                imgPosted.ToolTip = dtExpenseList.Rows[e.Row.RowIndex]["FTH_STATUS_TEXT"].ToString();
                            }
                            else
                            {
                                imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                                imgPosted.ToolTip = Resources.Captions.NotPosted;
                            }
                        }
                        //if (Convert.ToBoolean(hdfPosted.Value) == true)
                        //{
                        //    imgPosted.CssClass = GetLocalResourceObject("posted").ToString();
                        //    imgPosted.ToolTip = Resources.Captions.Posted;
                        //}
                        //else
                        //{
                        //    imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                        //    imgPosted.ToolTip = Resources.Captions.NotPosted;
                        //}
                    }
                }
                else if ((sender as GridView).ID == "grdItemDetails")
                {
                    Label lbltype;
                    HiddenField hdfBtype;
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        lbltype = e.Row.FindControl("lbltype") as Label;
                        hdfBtype = e.Row.FindControl("hdfBtype") as HiddenField;

                        lbltype.Text = hdfBtype.Value == "0" ? "" : (Convert.ToInt32(hdfBtype.Value) == (int)VendorContactTypeEnum.Branch ? "Branch" : "Head Office");


                        e.Row.Cells[5].Visible = EnableItemDiscount;
                        e.Row.Cells[6].Visible = EnableItemTax;
                        e.Row.Cells[4].Visible = EnableItemDiscount || EnableItemTax;
                        Label lblItemPartyInvDate = e.Row.FindControl("lblItemPartyInvDate") as Label;
                        DateTime result;
                        if (DateTime.TryParse(lblItemPartyInvDate.Text, out result))
                            lblItemPartyInvDate.Text = result.ToString(CommonConstants.DATEFORMAT);
                        else
                            lblItemPartyInvDate.Text = string.Empty;


                    }
                    else if (e.Row.RowType == DataControlRowType.Footer && soExpenseDetailsList != null)
                    {
                        //lbltype = e.Row.FindControl("lbltype") as Label;
                        //hdfBtype = e.Row.FindControl("hdfBtype") as HiddenField;

                        //GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                        //SetFieldValues(ControlsEnum.VENDORCONTACTYPE);




                        e.Row.Cells[5].Visible = EnableItemDiscount;
                        e.Row.Cells[6].Visible = EnableItemTax;
                        e.Row.Cells[4].Visible = EnableItemDiscount || EnableItemTax;

                        lblItemTotalQty = e.Row.FindControl("lblItemTotalQty") as Label;
                        lblItemTotalAmount = e.Row.FindControl("lblItemTotalAmount") as Label;
                        lblDiscountTotal = e.Row.FindControl("lblDiscountTotal") as Label;
                        lblTaxTotal = e.Row.FindControl("lblTaxTotal") as Label;
                        lblSubTotalFooter = e.Row.FindControl("lblSubTotalFooter") as Label;

                        if (lblItemTotalQty != null)
                        {
                            //lblItemTotalQty.Text = lblItemTotalQty.ToolTip = GetFormattedNumber(soExpenseDetailsList.Sum(itm => itm.VID_QTY_INVOICED));
                            lblItemTotalQty.Text = lblItemTotalQty.ToolTip = GetFormattedNumberWithSeperation(soExpenseDetailsList.Sum(itm => itm.VID_QTY_INVOICED));
                        }
                        if (lblItemTotalAmount != null)
                        {
                            lblItemTotalAmount.Text = lblItemTotalAmount.ToolTip = soExpenseDetailsList.Sum(itm => itm.VID_AMOUNT).ToString("N");
                        }
                        if (lblDiscountTotal != null)
                        {
                            lblDiscountTotal.Text = lblDiscountTotal.ToolTip = soExpenseDetailsList.Sum(itm => itm.VID_DISCOUNT).ToString("N");
                        }
                        if (lblTaxTotal != null)
                        {
                            lblTaxTotal.Text = lblTaxTotal.ToolTip = soExpenseDetailsList.Sum(itm => itm.VID_TAX).ToString("N");
                        }
                        if (lblSubTotalFooter != null)
                        {
                            //lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = soExpenseDetailsList.Sum(itm => itm.VID_NET_AMOUNT).ToString(hdfCurrencyFormat.Value);
                            lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = GetFormattedCurrencyWithSeperation(soExpenseDetailsList.Sum(itm => itm.VID_NET_AMOUNT));
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[5].Visible = EnableItemDiscount;
                        e.Row.Cells[6].Visible = EnableItemTax;
                        e.Row.Cells[4].Visible = EnableItemDiscount || EnableItemTax;
                    }
                }
                int slno;
                if (((GridView)sender).ID == "grdUploads")
                {
                    if (EntryStatus == EntryStatus.VIEWMODE)
                    {
                        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.DataRow)
                        {
                            //e.Row.Cells[4].Visible = false;
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
            SetAllocationDetails();
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.EXPENSELIST);
            SetFieldValues(ControlsEnum.EXPENSELIST);
            EntryStatus = EntryStatus.LISTMODE;
            SetGridStatus();
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
                GetFieldValues(ControlsEnum.EXPENSELIST);
                SetFieldValues(ControlsEnum.EXPENSELIST);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
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
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteExp.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnPrintLst.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnAlert.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);

            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForPayment.PreRender += new EventHandler(btnAction_PreRender);
            //  btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);
            btnUpload.PreRender += new EventHandler(btnAction_PreRender);

            //lbnPOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnkInvoicing.PreRender += new EventHandler(btnAction_PreRender);
            //lbnExpenses.PreRender += new EventHandler(btnAction_PreRender);
            //lbnPOInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lnkPayment.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            btnApply.PreRender += new EventHandler(btnAction_PreRender);
            imgPopupAdd.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForCrDrNote.PreRender += new EventHandler(btnAction_PreRender);


            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteExp.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnPrintLst.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnAlert.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);

            btnNew.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            btnPickForPayment.Load += new EventHandler(btnAction_Load);
            // btnResetSelection.Load += new EventHandler(btnAction_Load);
            btnUpload.Load += new EventHandler(btnAction_Load);

            //lbnPOListing.Load += new EventHandler(btnAction_Load);
            //lnkInvoicing.Load += new EventHandler(btnAction_Load);
            //lbnExpenses.Load += new EventHandler(btnAction_Load);
            //lbnPOInvoice.Load += new EventHandler(btnAction_Load);
            //lnkPayment.Load += new EventHandler(btnAction_Load);
            //lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //lnbAcPayables.Load += new EventHandler(btnAction_Load);

            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);

            btnApply.Load += new EventHandler(btnAction_Load);
            imgPopupAdd.Load += new EventHandler(btnAction_Load);
            btnPickForCrDrNote.Load += new EventHandler(btnAction_Load);
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
                    //case NavigationEnum.PAGECHANGE:
                    //    uclPaging.CurrentPage = e.CurrentPage;
                    //    break;
                    //case NavigationEnum.FIRST:
                    //    if (e.CurrentPage > 1)
                    //        uclPaging.CurrentPage = 1;
                    //    break;
                    //case NavigationEnum.LAST:
                    //    if (e.CurrentPage <= e.TotalPages)
                    //        uclPaging.CurrentPage = e.TotalPages;
                    //    break;
                    //case NavigationEnum.NEXT:
                    //    // increment the current page index.
                    //    if (e.CurrentPage <= e.TotalPages)
                    //        uclPaging.CurrentPage++;
                    //    break;
                    //case NavigationEnum.PREVIOUS:
                    //    // Decrement the current page index.
                    //    if (e.CurrentPage > 1)
                    //        uclPaging.CurrentPage--;
                    //    break;


                }

                //PageIndex = uclPaging.CurrentPage.ToString();
                // Change Code As per the page
                //GetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                //SetFieldValues(ControlsEnum.PAYMENTHDRLIST);
                EntryStatus = EntryStatus.LISTMODE;
                //============================
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.gComsRes.Information + "');", true);
            }

        }

        /// <summary>
        /// Methord used to enable and Disable Page Navigation Controls
        /// </summary>
        /// <param name="iTotalPages"></param>
        private void EnableDisableButtons(int iTotalPages)
        {
            //    uclPaging.FirstButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;

            //    uclPaging.PreviousButtonEnabled = (uclPaging.CurrentPage == 1) ? false : true;

            //    uclPaging.NextButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;

            //    uclPaging.LastButtonEnabled = (uclPaging.CurrentPage < iTotalPages) ? true : false;
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
                if (ExpenseHeaderSession != null)
                {
                    hdfHasTax.Value = ((ExpenseHeaderSession.TaxHdr == null || ExpenseHeaderSession.TaxHdr.Count == 0)
                        && (ExpenseHeaderSession.OrderDetail == null || ExpenseHeaderSession.OrderDetail.Count == 0 ||
                        ExpenseHeaderSession.OrderDetail.All(dtl => (dtl.TaxDtl == null || dtl.TaxDtl.Count == 0))))
                        ? CommonConstants.SELECT_VALUE_ZERO : CommonConstants.SELECT_VALUE_ONE;
                }
                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "PageViewMode", "$(document).ready(function(){PageViewMode(2);});", true);
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
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                if (!ShowExpenseCrDr)
                    btnPickForCrDrNote.Visible = false;
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
            EXPENSEHEADER,
            EXPENSEDETAIL,
            SELECTEDITEM,
            TAXTYPES,
            TAXPOPUPGRID,
            TAXHEADER,
            EXCHANGERATE,
            EXPENSELIST,
            JOURNALIZE,
            FINHEADER,
            PICKFORPAYMENT,
            PICKFORCRDRNOTE,
            GETEXPENSEPKBYJOURNALPK,
            TAXSETTINGS,
            VENDORDETAILS,
            DEFAULTUOM,
            COMPANY,
            ADDITEM,
            UPLOADEDFILES,
            SELECTEDDOC,
            EXPENSEDETAILTAX,
            VENDORCONTACTYPE,
            VENDORSELECTEDDTL,
            CHKINVNO,
            VENDORCONTACTYPEFROMDB,
            VENDORCONTACTYPEDETAILS,
            VENDORCONTACTYPEMAINFROMDB,
            VENDORCONTACTYPEDETAILSHDR,
            CUSTOMTAXSETTINGS,
            RESETPAYMENT,
            AMOUNTDETAILS,
            INVOICEGET,
            COMPANYSRCH,
            INVOICEGSTTYPE,
            EXPENSEADVANCES,
            BASECURRENCY
        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }
        #region VendorContactTypes
        public enum VendorContactTypeEnum
        {
            HeadOffice = 4,
            Branch = 5
        }
        #endregion

        #endregion
    }
}