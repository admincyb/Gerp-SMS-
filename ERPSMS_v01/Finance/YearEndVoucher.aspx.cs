using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ERP.Utilities;
using BusinessObject.Common;
using BusinessObject.AccountManagement;
using BusinessObject;
using ERPManager;
using ERPSMS_v01.UserControls;
using System.Data;
using BusinessObject.CommonManagement;
using BusinessObject.SaleOrder;
using System.Xml;
using System.Text;
using System.Threading;
using ERPService;
using ERPData;
using System.Web.UI.HtmlControls;
using BusinessObject.AlertManagement;
using BusinessObject.Finance;
using ERP.Store.UI;



namespace ERPSMS_v01.Finance
{
    public partial class YearEndVoucher : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
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
        /// Is Otehr charge deducted from inv
        /// </summary>
        private bool IsOCded
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsOCded] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsOCded]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsOCded] = value;
            }
        }

        /// <summary>
        /// Is Other charge Edited
        /// </summary>
        private bool IsOCEdit
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsOCEdit] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsOCEdit]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsOCEdit] = value;
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
        /// GON PK
        /// </summary>
        private int DespatchID
        {
            get
            {
                return this.ViewState[ViewstateStrings.DespatchID] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.DespatchID]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.DespatchID] = value;
            }
        }
        /// <summary>
        /// Current Quotation PK
        /// </summary>
        private int CurrSOPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrSOPK] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.CurrSOPK]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrSOPK] = value;
            }
        }
        /// <summary>
        /// Receipt detail PK
        /// </summary>
        private int ReceiptMpgPK
        {
            get
            {
                return this.ViewState[ViewstateStrings.ReceiptMpgPK] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.ReceiptMpgPK]) : 0;
            }
            set
            {
                this.ViewState[ViewstateStrings.ReceiptMpgPK] = value;
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
        /// Sale Order TYPE
        /// </summary>
        private int SaleOrderType
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.SaleOrderType];
            }
            set
            {
                this.ViewState[ViewstateStrings.SaleOrderType] = value;
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
        /// To maintain keep SO Invoice Header Tax Splitting
        /// </summary>
        private SOInvoiceHeader SOInvoiceHeaderSession
        {
            get
            {
                return (SOInvoiceHeader)Session[ERP.Utilities.SessionStrings.SOInvoiceHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SOInvoiceHeaderSession] = value;
            }
        }
        /// <summary>
        /// To maintain keep PO Invoice Header Tax Splitting
        /// </summary>
        private SOInvoiceHeader TempInvoiceHeaderTemp
        {
            get
            {
                return (SOInvoiceHeader)Session[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep SO Invoice Header Tax Splitting
        /// </summary>
        private SOInvoiceHeader TempSOInvoiceHeaderSession
        {
            get
            {
                return (SOInvoiceHeader)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSession] = value;
            }
        }

        /// <summary>
        /// To maintain keep CustomerAll Pending Allocation
        /// </summary>
        private SOInvoiceHeader TempSOInvoiceHeaderSessionCustAll
        {
            get
            {
                return (SOInvoiceHeader)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSessionCustAll];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceHeaderSessionCustAll] = value;
            }
        }


        /// <summary>
        /// To maintain keep SO Invoice Dtl Tax deduction from line item
        /// </summary>       
        private List<SOInvoiceDetails> TempORGsoInvoiceDetailsList
        {
            get
            {
                return (List<SOInvoiceDetails>)this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.TempSOInvoiceDtlSession] = value;
            }
        }
        /// <summary>
        /// SO Invoice PK
        /// </summary>
        private int SOInvoicePK
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SOInvoicePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SOInvoicePK] = value;
            }
        }

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
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedSalesInvoices
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices] = value;
            }

        }
        /// <summary>
        /// To maintain keep selected Currency
        /// </summary>
        private long SelectedCurrency
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedInvoiceType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedInvoiceType] = value;
            }

        }
        /// <summary>
        /// To maintain keep selected Currency
        /// </summary>
        private long SelectedInvoiceType
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

        private long SelectedCustomers
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.SelectedCustomers]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCustomers] = value;
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
        private List<decimal> SelectedINVTax
        {
            get
            {
                return (List<decimal>)this.ViewState["SelectedINVTax"];
            }
            set
            {
                this.ViewState["SelectedINVTax"] = value;
            }

        }
        private decimal INVTax
        {
            get
            {
                return (decimal)this.ViewState["INVTax"];
            }
            set
            {
                this.ViewState["INVTax"] = value;
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

        private int SelectedRowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.CurrRow] == null ? -1 : Convert.ToInt32(this.ViewState[ViewstateStrings.CurrRow].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.CurrRow] = value;
            }
        }
        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        private ControlsEnum controlEnum;
        User currentUser;
        private ServiceUtility serviceUtilityObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        SOInvoiceDetails soDtlObj;
        string selectedVendor;
        DataSet dsInvHeader;
        DataTable dtTaxDetails;
        private DataSet dsPageData;
        private DataTable dtTransaction;
        private DataTable dtVoucherTypes;
        private DataTable dtVoucherList;
        private YearEndVoucherBO yearEndObj;

        DataTable dtCompany = new DataTable();

        private int CustomerTypeSelectedPk;
        private string CustomerSavedBranchId;
        private string CustomerSavedTaxId;

        DataSet dsDueDate;
        private int paymentTermPK;
        private int custPK;
        private string scPK;
        bool hasValidRate;
        private FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        List<SAL_DESPATCH_DTL> salDespatchDtlList;


        private ADM_CONFIG_MST admConfigMstObj;
        private List<ADM_CONFIG_MST> workflowStatusList;

        private List<ADM_CONFIG_MST> admConfigMstList;
        private ADM_CURRENCY_MST admCurrencyMstObj;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        private ADM_APP_TYPE_MST admAppTypeMstObj;
        private List<ADM_APP_TYPE_MST> admAppTypeMstList;
        DataSet dsAlertList;
        private int invPK;
        private string appType;
        private string TypeRef;
        string JournalType = string.Empty;

        private int JournalPK;
        private int shippingPlanPK;

        private string refID;
        private decimal totalAllocatedTax;
        private decimal totalAllocatedDiscount;
        private decimal amtAdjAdvDeduction = 0;
        private string inboxFlag;
        private decimal TotalHDRDiscount = 0;
        private int isHaveDiscount = 0;
        private long InvoicePk = 0;
        private decimal totalTaxSplitFooter = 0;
        private bool IsPageChanged = false;

        private List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
        private CommonService cm;

        private ADM_COMPANY_MST admCompanyMstObj;

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

            try
            {
                if (Session[ERP.Utilities.SessionStrings.TransactionType] == null)
                {
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }
                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                ucrWrkf.ViewType = 1;

                if (!IsPostBack)
                {
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);

                    ddlVoucherNo.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    ddlCustomer.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));



                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfCurrencyFormatWithSeperation.Value = "#" + currencysep + "#0.";
                    hdfJournalizeWorkFlow.Value = "0";
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                    }
                    hdfCurrencyFormat.Value = "#0.";
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
                    hdfExchangeRateFormat.Value = "#0.";
                    int exchrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.ExchRateDecimalDigit]));
                    for (int i = 0; i < exchrateDecimalDigits; i++)
                    {
                        hdfExchangeRateFormat.Value += "0";
                    }
                    FillProcessID(1);
                    txtPendingAsOnDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    GetFieldValues(ControlsEnum.TRANSACTIONS);
                    SetFieldValues(ControlsEnum.TRANSACTIONS);

                    GetFieldValues(ControlsEnum.VOUCHERTYPES);
                    SetFieldValues(ControlsEnum.VOUCHERTYPES);

                    GetFieldValues(ControlsEnum.VOUCHERLIST);
                    SetFieldValues(ControlsEnum.VOUCHERLIST);

                    //for Voucher ddl
                    GetFieldValues(ControlsEnum.VOUCHERDDL);
                    SetFieldValues(ControlsEnum.VOUCHERDDL);
                    //for Party ddl
                    GetFieldValues(ControlsEnum.PARTYDDL);
                    SetFieldValues(ControlsEnum.PARTYDDL);
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
        #endregion



        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            DateTime AsOnDate = DateTime.Today;
            AdmCompanyMstService admCompanyMstServiceClient;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                switch (type)
                {
                    #region TRANSACTIONS
                    case ControlsEnum.TRANSACTIONS:
                        dtTransaction = BusinessLogic.Finance.YearEndVoucherBL.GetTransactions(0, 1, "YEAREND");
                        break;
                    #endregion

                    #region PARTY   ddl
                    case ControlsEnum.PARTYDDL:

                        if (!string.IsNullOrEmpty(txtPendingAsOnDate.Text.Trim()))
                            DateTime.TryParse(txtPendingAsOnDate.Text.Trim(), out AsOnDate);
                        dtTransaction = BusinessLogic.Finance.YearEndVoucherBL.GetPartyOrVoucherDDL(Convert.ToInt32(ddlTransaction.SelectedValue), Convert.ToInt32(ddlVoucherType.SelectedValue), AsOnDate, Convert.ToInt32(ddlStatus.SelectedValue), currentUser.SBUID, ddlVoucherNo != null ? Convert.ToInt32(ddlVoucherNo.SelectedValue) : 0, ddlCustomer != null ? Convert.ToInt32(ddlCustomer.SelectedValue) : 0, "FTH_PARTY_PK");
                        break;
                    #endregion
                    #region  VOUCHER ddl
                    case ControlsEnum.VOUCHERDDL:

                        if (!string.IsNullOrEmpty(txtPendingAsOnDate.Text.Trim()))
                            DateTime.TryParse(txtPendingAsOnDate.Text.Trim(), out AsOnDate);
                        dtTransaction = BusinessLogic.Finance.YearEndVoucherBL.GetPartyOrVoucherDDL(Convert.ToInt32(ddlTransaction.SelectedValue), Convert.ToInt32(ddlVoucherType.SelectedValue), AsOnDate, Convert.ToInt32(ddlStatus.SelectedValue), currentUser.SBUID, ddlVoucherNo != null ? Convert.ToInt32(ddlVoucherNo.SelectedValue) : 0, ddlCustomer != null ? Convert.ToInt32(ddlCustomer.SelectedValue) : 0, "FTH_VOUCHER_NO");
                        break;
                    #endregion
                    #region VOUCHER TYPES
                    case ControlsEnum.VOUCHERTYPES:
                        dtVoucherTypes = BusinessLogic.Finance.YearEndVoucherBL.GetVoucherTypes(0, 1, (int)GroupTypeConstantValue.YearEnd, (int)GroupConstantValue.YearEndProcess);
                        break;
                    #endregion

                    #region VOUCHER LIST
                    case ControlsEnum.VOUCHERLIST:
                        serviceUtilityObj = new ServiceUtility();
                        if (!IsPageChanged)
                        {
                            PageIndex = "1";
                            uclPaging.CurrentPage = 1;
                        }
                        serviceUtilityObj.CurrentPage = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage;
                        serviceUtilityObj.PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                        serviceUtilityObj.TotalRecords = 0;

                        if (!string.IsNullOrEmpty(txtPendingAsOnDate.Text.Trim()))
                            DateTime.TryParse(txtPendingAsOnDate.Text.Trim(), out AsOnDate);
                        dtVoucherList = new DataTable();
                        dtVoucherList = BusinessLogic.Finance.YearEndVoucherBL.GetVoucherList(Convert.ToInt32(ddlTransaction.SelectedValue), Convert.ToInt32(ddlVoucherType.SelectedValue), AsOnDate, Convert.ToInt32(ddlStatus.SelectedValue),
                            currentUser.SBUID, ddlVoucherNo != null ? Convert.ToInt32(ddlVoucherNo.SelectedValue) : 0, ddlCustomer != null ? Convert.ToInt32(ddlCustomer.SelectedValue) : 0, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize);

                        serviceUtilityObj.TotalRecords = dtVoucherList.Rows.Count > 0 ? Convert.ToInt32(dtVoucherList.Rows[0]["REC_COUNT"].ToString()) : 0;
                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                    (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                    (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        DateTime asondate = DateTime.Now;
                        DateTime.TryParse(txtPendingAsOnDate.Text, out asondate);
                        int currencyPk = 0;
                        int.TryParse(hdfCurrency.Value, out currencyPk);
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(currencyPk, currentUser.BaseCurrency, asondate);
                        txtExchangeRate.Text = string.Empty;
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]) > 0)
                            {
                                txtExchangeRate.Text = Convert.ToDouble(dsExchangeRate.Tables[0].Rows[0][0]).ToString();
                            }
                        }
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
                    #region JOURNALIZATION TYPE
                    case ControlsEnum.JOURNALIZATIONTYPE:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        admAppTypeMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_APP_TYPE_MST>();
                        admAppTypeMstObj.APT_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admAppTypeMstObj.APT_SPL_COND = "J";
                        //if (Session[ERP.Utilities.SessionStrings.Type] != null)
                        //{
                        //    if (Session[ERP.Utilities.SessionStrings.Type].ToString() == ApplicationType.YE)
                        //        admAppTypeMstObj.APT_SPL_COND = "YEJ";
                        //}
                        admAppTypeMstList = CommonServiceClient.GetAppTypeValues(admAppTypeMstObj);
                        break;
                    #endregion
                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = GetLocalResourceObject("APPLICATION_STATUS").ToString();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        workflowStatusList = CommonServiceClient.GetConfigValues(admConfigMstObj);
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
                        //  dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
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

                    #region VOUCHER LIST
                    case ControlsEnum.VOUCHERLIST:
                        BindGrid(controlType);

                        break;
                    #endregion
                    #region TRANSACTIONS
                    case ControlsEnum.TRANSACTIONS:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region PARTY DDL
                    case ControlsEnum.PARTYDDL:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region VOUCHER DDL
                    case ControlsEnum.VOUCHERDDL:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region VOUCHER TYPES
                    case ControlsEnum.VOUCHERTYPES:
                        BindDropDown(controlType);
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
        #region Set UI Values To Object

        /// <summary>
        /// Assigns the object with corresponding input control values
        /// </summary>
        /// <returns></returns>
        private Object SetUIValuesToObject(ControlsEnum controlType)
        {
            Object retObject;
            retObject = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            YearEndVoucherBO yearEndVoucherObj = new YearEndVoucherBO();
            List<YearEndVoucherDetails> yearEndDetailsList = new List<YearEndVoucherDetails>();
            YearEndVoucherDetails yearEndDetails = new YearEndVoucherDetails();

            try
            {
                switch (controlType)
                {
                    #region YEARENDVOUCHERHDR
                    case ControlsEnum.YEARENDVOUCHER:
                        if (SelectedRowIndex >= 0)
                        {
                            double oldExchRate = 0;
                            Decimal BalanceAmt = 0;
                            DateTime yeddate = DateTime.Now;
                            GridViewRow grdrow = grdVoucherList.Rows[SelectedRowIndex];
                            HiddenField hdfTrxPk = (HiddenField)grdrow.FindControl("hdfTrxPk");
                            HiddenField hdfTrxType = (HiddenField)grdrow.FindControl("hdfTrxType");
                            Label lblBalance = (Label)grdrow.FindControl("lblBalance");
                            Label lblExchangeRate = (Label)grdrow.FindControl("lblExchangeRate");
                            double.TryParse(lblExchangeRate.Text.Replace(",", ""), out oldExchRate);
                            Decimal.TryParse(lblBalance.Text.Replace(",", ""), out BalanceAmt);

                            yearEndVoucherObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            yearEndVoucherObj.BIZUNIT_PK = Convert.ToInt16(currentUser.SBUID);
                            yearEndVoucherObj.LAST_MOD_DT = LastModifiedTime;
                            yearEndVoucherObj.USER_PK = currentUser.PKUser;
                            DateTime.TryParse(txtPendingAsOnDate.Text, out yeddate);
                            yearEndVoucherObj.YED_DATE = yeddate;
                            yearEndVoucherObj.YED_PROCESS = Convert.ToInt32(ddlVoucherType.SelectedValue);

                            yearEndDetails.YED_COMPANY = Convert.ToInt32(ddlCompany.SelectedValue);
                            yearEndDetails.YED_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            yearEndDetails.YED_DESC = string.Empty;
                            yearEndDetails.YED_EXCHG_RATE = Convert.ToDouble(txtExchangeRate.Text);
                            yearEndDetails.YED_PK = 0;
                            yearEndDetails.YED_TRX_PK = Convert.ToInt64(hdfTrxPk.Value);
                            yearEndDetails.YED_TRX_TYPE = Convert.ToInt32(ddlTransaction.SelectedValue);
                            yearEndDetails.YED_OLD_EXCHG_RATE = oldExchRate;
                            yearEndDetails.YED_AMOUNT = BalanceAmt;
                            yearEndDetailsList.Add(yearEndDetails);
                            yearEndVoucherObj.Details = yearEndDetailsList;
                            retObject = yearEndVoucherObj;
                        }
                        break;
                    #endregion

                    #region Journalize
                    case ControlsEnum.JOURNALIZE:

                        if (CurrPK > 0 && SelectedRowIndex >= 0)
                        {

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
                            Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                            //Journalize New sessions End

                            GridViewRow grdrow = grdVoucherList.Rows[SelectedRowIndex];
                            HiddenField hdfTrxPk = (HiddenField)grdrow.FindControl("hdfTrxPk");
                            HiddenField hdfTrxTypeYe = (HiddenField)grdrow.FindControl("hdfTrxTypeYe");
                            HiddenField hdfCurrencyPk = (HiddenField)grdrow.FindControl("hdfCurrencyPk");
                            Label lblTrxNo = (Label)grdrow.FindControl("lblTrxNo");
                            Label lblTrxDate = (Label)grdrow.FindControl("lblTrxDate");


                            DateTime trxDate = DateTime.Now;
                            //DateTime.TryParse(lblTrxDate.Text, out trxDate);
                            DateTime.TryParse(txtPendingAsOnDate.Text, out trxDate);
                            ucrJournalize.TransactionType = Convert.ToString(hdfTrxTypeYe.Value);
                            Session[ERP.Utilities.SessionStrings.TransactionType] = Convert.ToString(hdfTrxTypeYe.Value); ;
                            ucrJournalize.TransactionPK = CurrPK;// Convert.ToInt64(hdfTrxPk.Value);
                            Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;// Convert.ToInt32(hdfTrxPk.Value);
                            ucrJournalize.JournalizePK = 0;
                            Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                            Session[ERP.Utilities.SessionStrings.TransactionNo] = lblTrxNo.Text;
                            Session[ERP.Utilities.SessionStrings.TransactionDate] = trxDate;
                            Session[ERP.Utilities.SessionStrings.TransactionCurrency] = Convert.ToInt32(hdfCurrencyPk.Value);
                            //Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                            //Session[ERP.Utilities.SessionStrings.AccountPayablePK] = invoiceHeaderObj.ICH_CUSTOMER;
                            Session[ERP.Utilities.SessionStrings.JournalType] = Convert.ToString(hdfTrxTypeYe.Value);
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
                            EntryStatus = EntryStatus.ENTRYMODE;
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrJournalize.JournalizeRefPK = ucrWrkf.RefID;
                                ucrWrkf.ViewType = 1;
                                // Session[ERP.Utilities.SessionStrings.JournalMode] = EntryStatus.ENTRYMODE;
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

                            TextBox WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                            WrkfComments.Text = "";
                            Session[ERP.Utilities.SessionStrings.RemoveRowIndex] = null;
                            hdfJournalizeWorkFlow.Value = "1";
                            ucrJournalize.CallUserControl();

                            Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("YearEndVoucher").ToString();

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);

                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
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
                    default: ;
                        break;

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
                case ControlsEnum.TRANSACTIONS:
                    ddlTransaction.Items.Clear();
                    if (dtTransaction != null && dtTransaction.Rows.Count > 0)
                    {
                        ddlTransaction.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTransaction, "APT_NAME");
                        ddlTransaction.DataTextField = "APT_NAME";
                        ddlTransaction.DataValueField = "APT_PK";
                        ddlTransaction.DataBind();
                    }
                    //ddlTransaction.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.PARTYDDL:
                    ddlCustomer.Items.Clear();
                    if (dtTransaction != null && dtTransaction.Rows.Count > 0)
                    {
                        //Sorting DataTable
                        dtTransaction.DefaultView.Sort = "VALUE ASC";
                        ddlCustomer.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTransaction, "VALUE");
                        ddlCustomer.DataTextField = "VALUE";
                        ddlCustomer.DataValueField = "PK";
                        ddlCustomer.DataBind();
                    }
                    ddlCustomer.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.VOUCHERDDL:
                    ddlVoucherNo.Items.Clear();
                    if (dtTransaction != null && dtTransaction.Rows.Count > 0)
                    {
                        ddlVoucherNo.DataSource = CommonFunctions.HtmlDecodeDataTable(dtTransaction, "VALUE");
                        ddlVoucherNo.DataTextField = "VALUE";
                        ddlVoucherNo.DataValueField = "PK";
                        ddlVoucherNo.DataBind();
                    }
                    ddlVoucherNo.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                case ControlsEnum.VOUCHERTYPES:
                    ddlVoucherType.Items.Clear();
                    if (dtVoucherTypes != null && dtVoucherTypes.Rows.Count > 0)
                    {
                        ddlVoucherType.DataSource = dtVoucherTypes;
                        ddlVoucherType.DataTextField = "CON_NAME";
                        ddlVoucherType.DataValueField = "CON_PK";
                        ddlVoucherType.DataBind();
                    }
                    //ddlVoucherType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

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
                    //  ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    //if (dtCompany.Rows.Count > 0)
                    //{
                    //    ddlCompany.DataSource = dtCompany;

                    //    ddlCompany.DataSource = CommonFunctions.HtmlDecode(dtCompany);,Resources.DataFieldRes.CompanySpecs);
                    //    ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                    //    ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                    //    ddlCompany.DataBind();

                    //}
                    break;
                #endregion

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

                    case ControlsEnum.VOUCHERLIST:
                        if (dtVoucherList != null)
                        {


                            //GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            //PageIndex = PageIndex == null ? "0" : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);

                            grdVoucherList.PageIndex = Convert.ToInt32(PageIndex);
                            grdVoucherList.DataSource = dtVoucherList.DefaultView;
                            grdVoucherList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                            //For Setting/Resetting Colour of a selected InvoiceNo
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
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
        public string GetFormattedExchangeRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfExchangeRateFormat.Value);
        }
        public string GetCeiledInteger(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return Math.Ceiling(num).ToString();
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

            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            try
            {
                int isParty = (int)YearEndVoucherSearch.Other;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int? result;
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
                    if (((DropDownList)sender).ID == "ddlTransaction" || ((DropDownList)sender).ID == "ddlVoucherType" || ((DropDownList)sender).ID == "ddlStatus" || ((DropDownList)sender).ID == "ddlVoucherNo" || ((DropDownList)sender).ID == "ddlCustomer")
                    {
                        commonActions = ActionsEnum.SEARCH;
                    }
                    if (((DropDownList)sender).ID == "ddlCustomer")
                    {
                        isParty = (int)YearEndVoucherSearch.PartySelect;
                    }
                    else if (((DropDownList)sender).ID == "ddlVoucherNo")
                    {
                        isParty = (int)YearEndVoucherSearch.VoucherSelect;
                    }
                    else if (((DropDownList)sender).ID == "ddlTransaction")
                    {
                        isParty = (int)YearEndVoucherSearch.TransactionSelect;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtPendingAsOnDate")
                    {
                        commonActions = ActionsEnum.SEARCH;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    if (((LinkButton)sender).ID == "lnkVoucherNo")
                    {
                        commonActions = ActionsEnum.PRINT;
                    }
                }


                switch (commonActions)
                {
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow selectedGrdrow = (sender as RadioButton).Parent.Parent as GridViewRow;
                        HiddenField hdTrxPk = (HiddenField)selectedGrdrow.FindControl("hdTrxPk");
                        RadioButton rbtn = (RadioButton)selectedGrdrow.FindControl("rbtSelect");
                        HiddenField hdfCurrencyPk = (HiddenField)selectedGrdrow.FindControl("hdfCurrencyPk");
                        LinkButton lbtnVourNo = (LinkButton)selectedGrdrow.FindControl("lnkVoucherNo");
                        Label lblExRate = (Label)selectedGrdrow.FindControl("lblExchangeRate");
                        Label lblAmount = (Label)selectedGrdrow.FindControl("lblAmount");
                        hdfPopAppType.Value = ((HiddenField)selectedGrdrow.FindControl("hdfFthRefType")).Value;
                        hdfPopVoucherID.Value = ((HiddenField)selectedGrdrow.FindControl("hdfFthRefPk")).Value;
                        hdfPopPK.Value = ((HiddenField)selectedGrdrow.FindControl("hdfFthPk")).Value; 
                        lbtnVoucherNo.Text = lbtnVourNo.Text;
                        lblPopAmount.Text = lblAmount.Text;
                        lblPopRate.Text = lblExRate.Text;
                        hdfCurrency.Value = hdfCurrencyPk.Value;
                        SelectedRowIndex = selectedGrdrow.RowIndex;
                        //SetResetColour
                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        IsPageChanged = false;
                        if (isParty != (int)YearEndVoucherSearch.VoucherSelect)// not voucher
                        {
                            ddlVoucherNo.Items.Clear();
                            ddlVoucherNo.DataSource = null;
                            ddlVoucherNo.DataBind();
                            ddlVoucherNo.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        }
                        if (isParty == (int)YearEndVoucherSearch.TransactionSelect)//if transaction
                        {
                            ddlCustomer.Items.Clear();
                            ddlCustomer.DataSource = null;
                            ddlCustomer.DataBind();
                            ddlCustomer.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                        }

                        GetFieldValues(ControlsEnum.VOUCHERLIST);
                        SetFieldValues(ControlsEnum.VOUCHERLIST);

                        if (isParty == (int)YearEndVoucherSearch.PartySelect)//if  Customer select
                        {
                            GetFieldValues(ControlsEnum.VOUCHERDDL);
                            SetFieldValues(ControlsEnum.VOUCHERDDL);
                            //reset default
                            GetFieldValues(ControlsEnum.VOUCHERLIST);
                            SetFieldValues(ControlsEnum.VOUCHERLIST);
                        }
                        else if (isParty == (int)YearEndVoucherSearch.Other || isParty == (int)YearEndVoucherSearch.TransactionSelect)
                        {
                            //for Voucher ddl
                            GetFieldValues(ControlsEnum.VOUCHERDDL);
                            SetFieldValues(ControlsEnum.VOUCHERDDL);
                            //for Party ddl
                            GetFieldValues(ControlsEnum.PARTYDDL);
                            SetFieldValues(ControlsEnum.PARTYDDL);
                        }

                        break;
                    #endregion
                    #region OK
                    case ActionsEnum.OK:
                        divErrorLabel.Visible = false;
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else
                        {
                            if (grdVoucherList.Rows.Count > 0)
                            {

                                yearEndObj = new YearEndVoucherBO();
                                yearEndObj = (YearEndVoucherBO)SetUIValuesToObject(ControlsEnum.YEARENDVOUCHER);

                                if (yearEndObj != null && yearEndObj.Details != null)
                                {
                                    if (yearEndObj.Details[0].YED_EXCHG_RATE != yearEndObj.Details[0].YED_OLD_EXCHG_RATE)
                                    {

                                        string xmlDoc = CommonFunctions.XmlSerialize<YearEndVoucherBO>(yearEndObj);
                                        // save Process Control inspection details
                                        result = BusinessLogic.Finance.YearEndVoucherBL.YearEndVoucher(xmlDoc);
                                        if (result > 0) // Success !  redirect to listing page
                                        {
                                            CurrPK = Convert.ToInt32(result);
                                            hdfJournalizeWorkFlow.Value = "0";
                                            SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            SelectedRowIndex =  -1;
                                        }
                                        else
                                        {
                                            CurrPK = 0;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                        }
                                    }
                                    else
                                    {
                                        divErrorLabel.Visible = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divExchangeRate]','" + GetLocalResourceObject("ExchangeRate").ToString() + "','330','150');", true);
                                    }

                                }


                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_emptygrid").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm();
                        break;
                    #endregion
                    #region Print
                    case ActionsEnum.PRINT:
                        string company = string.Empty;
                        
                        GridViewRow grdrow = (sender as LinkButton).Parent.Parent as GridViewRow;
                        int RefPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfFthRefPk")).Value);
                        int TrxPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfFthPk")).Value);
                        company = ((HiddenField)grdrow.FindControl("hdfFthCompanyPk")).Value;
                        JournalType = ((HiddenField)grdrow.FindControl("hdfFthRefType")).Value;                      

                       
                        if (JournalType == ApplicationType.JV || JournalType == ApplicationType.PCS || JournalType == ApplicationType.DPVJ || JournalType == ApplicationType.PCVJ)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + TrxPK.ToString() + "&APPTYPE=" + JournalType +
                                "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + JournalType + "&COMPANY=" + company + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + RefPK.ToString() + "&APPTYPE=" + JournalType +
                                "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + JournalType + "&COMPANY=" + company + "');", true);// 

                        }
                       

                        break;

                    #endregion
                    #region PRINTPOPUP
                    case ActionsEnum.PRINTPOPUP:
                        if (hdfPopAppType.Value == ApplicationType.JV || hdfPopAppType.Value == ApplicationType.PCS || hdfPopAppType.Value == ApplicationType.DPVJ || hdfPopAppType.Value == ApplicationType.PCVJ)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + hdfPopPK.Value + "&APPTYPE=" + hdfPopAppType.Value +
                                "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + hdfPopAppType.Value + "');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + hdfPopVoucherID.Value.ToString() + "&APPTYPE=" + hdfPopAppType.Value +
                                "&APPSUBTYPE=" + hdfAppSubType.Value + "&TRXTYPE=" + hdfPopAppType.Value + "');", true);// 

                        }
                        if (SelectedRowIndex >= 0)
                        {
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            //SetFieldValues(ControlsEnum.POPUP);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divExchangeRate]','" + GetLocalResourceObject("ExchangeRate").ToString() + "','330','150');", true);
                        }

                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        divErrorLabel.Visible = false;
                        bool IsContinue = false;

                        foreach (GridViewRow grvRow in grdVoucherList.Rows)
                        {
                            RadioButton rbtSelect = (RadioButton)grvRow.FindControl("rbtSelect");
                            if (rbtSelect.Checked)
                            {
                                IsContinue = true;
                                break;
                            }
                        }
                        if (SelectedRowIndex >= 0 && IsContinue)
                        {
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            //SetFieldValues(ControlsEnum.POPUP);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divExchangeRate]','" + GetLocalResourceObject("ExchangeRate").ToString() + "','330','150');", true);
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Voucher").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:
                        ucrJournalize.ResetForm();
                        hdfJournalizeWorkFlow.Value = "0";
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        ucrWrkf.Reset();
                        FillProcessID(1);

                        GetFieldValues(ControlsEnum.VOUCHERLIST);
                        SetFieldValues(ControlsEnum.VOUCHERLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);

                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.VOUCHERLIST);
                        SetFieldValues(ControlsEnum.VOUCHERLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                        }
                        ucrWrkf.Reset();
                        FillProcessID(1);


                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.VOUCHERLIST);
                        SetFieldValues(ControlsEnum.VOUCHERLIST);
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "DELETE")
                            {
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);

                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.VOUCHERLIST);
                        SetFieldValues(ControlsEnum.VOUCHERLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);


                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.VOUCHERLIST);
                        SetFieldValues(ControlsEnum.VOUCHERLIST);
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
                salesInvoiceServiceClient = null;
            }
        }
        /// <summary>
        ///  Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            txtPendingAsOnDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            ddlCompany.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            ddlTransaction.SelectedIndex = 0;
            ddlVoucherType.SelectedIndex = 0;
            GetFieldValues(ControlsEnum.VOUCHERLIST);
            SetFieldValues(ControlsEnum.VOUCHERLIST);

        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {

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
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.VOUCHERLIST);
            SetFieldValues(ControlsEnum.VOUCHERLIST);
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
            uclPaging.CurrentPage = 1;
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);


            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
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
                IsPageChanged = true;
                PageIndex = uclPaging.CurrentPage.ToString();
                // Change Code As per the page
                GetFieldValues(ControlsEnum.VOUCHERLIST);
                SetFieldValues(ControlsEnum.VOUCHERLIST);
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing(1);});", true);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            base.OnLoadComplete(e);
            lblBreadCrum.Text = (GetLocalResourceObject("Breadcrumb").ToString() + " >> " + ddlTransaction.SelectedItem.ToString()).Replace(">>", "<label style='font-family:Arial; letter-spacing:-7px; padding:0 7px 0 2px'>&#9658;</label>"); 
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
                    PageProcessID = ucrWrkf.ProcessID;
                    base.WkfPageUrl = path;
                }
            }
        }

        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {

            EXCHANGERATE,
            JOURNALIZE,
            FINHEADER,
            FILLWORKFLOWSTATUS,
            COMPANY,
            TRANSACTIONS,
            VOUCHERTYPES,
            VOUCHERLIST,
            GETINVOICEPKBYJOURNALPK,
            YEARENDVOUCHER,
            JOURNALIZATIONTYPE,
            PARTYDDL,
            VOUCHERDDL

        }
        /// <summary>
        /// Define Activity Status Enum
        /// </summary>
        public enum WkfStatusEnum
        {
            DRAFTED = 0,
            APPROVED = 2
        }


        #endregion


    }
}
