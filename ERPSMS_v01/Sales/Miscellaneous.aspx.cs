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
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using ERPSMS_v01.UserControls;

using BusinessObject.SaleOrder;
using System.Xml;
using System.Web.UI.HtmlControls;
using BusinessObject.AlertManagement;
using BusinessObject.Sales;
using BusinessLogic.CommonManagement;


namespace ERPSMS_v01.Sales
{
    public partial class Miscellaneous : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
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
        private int CusPk
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.CusPk]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CusPk] = value;
            }
        }
        /// <summary>
        /// To maintain keep Expense Tax Splitting
        /// </summary>
        //private SOInvoiceHeader ExpenseHeaderSession
        //{
        //    get
        //    {
        //        return (SOInvoiceHeader)Session[ERP.Utilities.SessionStrings.SOInvoiceHeaderSession];
        //    }
        //    set
        //    {
        //        Session[ERP.Utilities.SessionStrings.SOInvoiceHeaderSession] = value;
        //    }
        //}

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
        ///  /// <summary>
        /// To Disable Item Other Charge
        /// </summary>
        private bool EnableItemOtherCharge
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
        /// Is Tax Add
        /// </summary>
        private int isTaxAdd
        {
            get
            {
                return this.ViewState["isTaxAdd"] == null ? 0 : Convert.ToInt32(this.ViewState["isTaxAdd"]);
            }
            set
            {
                this.ViewState["isTaxAdd"] = value;
            }
        }
        /// <summary>
        /// Is Discount Add
        /// </summary>
        private int isDiscountAdd
        {
            get
            {
                return this.ViewState["isDiscountAdd"] == null ? 0 : Convert.ToInt32(this.ViewState["isDiscountAdd"]);
            }
            set
            {
                this.ViewState["isDiscountAdd"] = value;
            }
        }
        public double TotalIssueQty
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.TotalIssueQty]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalIssueQty] = value;
            }
        }

        public double TotalIssueInvdQty
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.TotalIssueInvdQty]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalIssueInvdQty] = value;
            }
        }

        public double TotalIssueBalance
        {
            get
            {
                return Convert.ToDouble(this.ViewState[ViewstateStrings.TotalIssueBalance]);
            }
            set
            {
                this.ViewState[ViewstateStrings.TotalIssueBalance] = value;
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
        /// To maintain keep Expense Header Tax Splitting
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
        /// To maintain Issue List After Apply (ViewState)
        /// </summary>
        private List<IssueDetail> IssueListApplied
        {
            get
            {
                return (List<IssueDetail>)this.ViewState[ERP.Utilities.SessionStrings.IssueListApplied];
            }
            set
            {
                this.ViewState[ERP.Utilities.SessionStrings.IssueListApplied] = value;
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
        private bool IsTaxForOtherCharge
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsTaxForOtherCharge] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsTaxForOtherCharge].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.IsTaxForOtherCharge] = value;
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
        private long SelectedInvoiceType
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
        /// To maintain  Invoice with new node <SO></SO>
        /// </summary>
        private SOInvoiceHeaderMul invoiceHeaderMulObj
        {
            get
            {
                return (SOInvoiceHeaderMul)Session[ERP.Utilities.SessionStrings.invoiceHeaderMulObj];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.invoiceHeaderMulObj] = value;
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

        private int SelectedIssueTypePk
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.SelectedIssueTypePk]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedIssueTypePk] = value;
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

        #endregion
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        //private SOInvoiceHeader invoiceHeaderObj;


        private SOInvoiceDetails soMiscDetailsObj;
        private SOInvoiceTaxHdr soInvTaxHdrObj;
        private DueDateDetails objDueDateDtls;
        List<SOInvoiceDetails> soMiscDetailsList;
        List<SOInvoiceTaxHdr> taxHdrList;
        List<SOInvoiceTaxHdr> TemptaxHdrList;
        SOInvoiceDetails soDtlObj;
        DataTable dtTaxDetails;
        DataTable dtSaleCat;
        bool hasValidRate;
        DataTable dtSOData;
        DataTable dtIssueType;
        DataTable dtInvoiceGstType;
        private DataTable dtGstSubType;
        private DataTable dtAmountDetails;
        DataSet dsDueDate;
        int JournalPK;
        //  private int CusPk;
        long InvoicePk = 0;
        private int custPK;
        DataSet dsCustomerTypes;
        DataSet dsCustomerDetailsByType;
        private int CustomerTypeSelectedPk;
        private string CustomerSavedBranchId;
        private string CustomerSavedTaxId;
        private IssueDetail soIssueQtyObj;


        DataSet dsPageData;
        DataTable dtPaymentTerms;
        private int paymentTermPK;
        private DataTable dtMISCLIST;
        private DataTable dtPageData;
        private DataTable dtCusTaxDetails;
        private DataTable dtCustomTaxSet;

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

        private List<CRM_CUSTOMER_MST> crmCustomerMstList;
        private SaleContractBO saleOrderHeaderObj;
        SOInvoiceTaxHdr tempInvTaxSplitObj;

        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private int invPK;
        private string appType;
        DataSet dsAlertList;
        List<IssueDetail> issueList;
        List<IssueDetail> issueListForGrid;
        DataTable dtCompany = new DataTable();

        private FIN_INVOICE_VND_HDR finExpenseVndHdrObj;
        private SOInvoiceHeader invoiceHeaderObj;
        private SOHeaderBO SoHeaderObj;

        DataTable dtInvoiceType;

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
                    SelectedSalesInvoices = null;
                    ConfigurationSettings();
                    ConfigurationSettingsforTaxBC();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANYSRCH);
                    GetFieldValues(ControlsEnum.SALECATEGORY);
                    SetFieldValues(ControlsEnum.SALECATEGORY);
                    //Enable or disable custom tax 
                    GetFieldValues(ControlsEnum.CUSTOMTAXSETTINGS);
                    if (Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                    {
                        CurrSOPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERPK]);
                        Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                        ////start
                        EntryStatus = EntryStatus.NEWMODE;
                        ////
                        ddlInvoiceType.Enabled = true;
                    }
                    else
                    {
                        //ddlInvoiceType.Enabled = false;
                    }
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    GetFieldValues(ControlsEnum.INVOICETYPE);
                    SetFieldValues(ControlsEnum.INVOICETYPE);

                    GetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                    SetFieldValues(ControlsEnum.INVOICEGSTTYPE);

                    GetFieldValues(ControlsEnum.GSTSUBTYPE);
                    SetFieldValues(ControlsEnum.GSTSUBTYPE);
                    lblSubType.Visible = false;
                    ddlSubType.Visible = false;
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

                    if (EnableItemTax)
                    {
                        imgHdrDiscount.Visible = false;
                        imgHdrTax.Visible = false;
                    }
                    //GetFieldValues(ControlsEnum.FROMPORT);
                    //SetFieldValues(ControlsEnum.FROMPORT);

                    string[] datakeyarray;
                    datakeyarray = new string[1];
                    datakeyarray[0] = "ICH_PK";
                    grdMiscList.DataKeyNames = datakeyarray;

                    string[] itemkeyarray;
                    itemkeyarray = new string[1];
                    itemkeyarray[0] = "CID_SL_NO";
                    grdItemDetails.DataKeyNames = itemkeyarray;

                    hdfNumberDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits.ToString();
                    hdfCurrencyDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    string currencysep = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
                    hdfDecimalFormatWithSeperation.Value = "#" + currencysep + "#0.";
                    hdfJournalizeWorkFlow.Value = "0";
                    hdfDecimalFormat.Value = "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
                    {
                        hdfDecimalFormat.Value += "0";
                        hdfDecimalFormatWithSeperation.Value += "0";
                    }
                    // hdfCurrencyFormat.Value = "#0.";
                    hdfCurrencyFormat.Value = "#,#0.";
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

                    hdfMiscRateFormat.Value = "#0.";
                    int miscrateDecimalDigits = (Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit] == null
                        ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                        : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit]));
                    for (int i = 0; i < miscrateDecimalDigits; i++)
                    {
                        hdfMiscRateFormat.Value += "0";
                    }
                    hdfMiscNumberDigits.Value = miscrateDecimalDigits.ToString();
                    SelectedInvoices = null;

                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    BindDefaultUOM();

                    AST_DOC_MODE.Value = "0";
                    SOInvoiceHeaderSession = new SOInvoiceHeader();

                    vreRateTaxDate.DecimalDigits = vreRate.DecimalDigits = Convert.ToInt32(hdfMiscNumberDigits.Value);
                    //GetFieldValues(ControlsEnum.TAXSETTINGS);
                    //if (dtPageData != null && dtPageData.Rows.Count > 0)
                    //{
                    //    foreach (DataRow row in dtPageData.Rows)
                    //    {
                    //        if (row["ACF_DATA"].ToString().Equals("DISCOUNT"))
                    //        {
                    //            if (row["ACF_VALUE"].ToString().Equals("0"))
                    //            {
                    //                EnableItemDiscount = false;
                    //            }
                    //        }
                    //        else if (row["ACF_DATA"].ToString().Equals("TAX"))
                    //        {
                    //            if (row["ACF_VALUE"].ToString().Equals("0"))
                    //            {
                    //                EnableItemTax = false;
                    //            }
                    //        }
                    //    }
                    //}
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
                                    hfCancelInv.Value = "1";//For Showing Cancelled Stamp in Detail Page
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

                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            btnPrint.Visible = true;
                        }
                        else
                        {
                            btnPrint.Visible = false;
                            txtInvoiceDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                            //Set currentdate in new mode For ETD & ETA
                            txtETD.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                            txtETA.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                            txtInvoiceDueDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                            //txtInvoiceDueDate.Enabled = false;
                            //txtInvoiceDueDate.CssClass = "medium input-disabled";

                            TempSOInvoiceHeaderSession = new SOInvoiceHeader();
                            SOInvoiceHeaderSession = new SOInvoiceHeader();

                            GetFieldValues(ControlsEnum.MISCLIST);
                            SetFieldValues(ControlsEnum.MISCLIST);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                        AST_DOC_MODE.Value = GetDOCMODE();
                        AST_CODE.Value = ApplicationType.MSI;
                        lblInvoiceNo.Text = hdfInvoiceNo.Value == string.Empty ? "[NEW]" : hdfInvoiceNo.Value;

                        hdfAppType.Value = ApplicationType.MSI;
                        hdfAppSubType.Value = string.Empty;

                        EnableDisableInvoiceType();
                        if (hdfIsTaxPayable.Value == "1")
                        {
                            SetFieldValues(ControlsEnum.SHOWDIVTAXBC);
                        }
                        //  Exchange rate field is not editable(Domestic).ie,If selected currency is same as SBU base currency
                        if (!string.IsNullOrEmpty(hdfCurrency.Value))
                        {
                            if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value))
                            {
                                txtExchangeRate.Enabled = false;
                            }
                            else
                            {
                                txtExchangeRate.Enabled = true;
                            }
                        }
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
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            try
            {
                switch (type)
                {
                     case ControlsEnum.SALECATEGORY:
                        dtSaleCat = CommonBL.GetAppConfig(currentUser.SBUID, "SALES CATEGORY");
                        break;
                    #region CUSTOMER SELECTED
                    case ControlsEnum.CUSTOMERSELECTED:
                        crmCustomerMstList = new List<CRM_CUSTOMER_MST>();
                        CommonService cm = new CommonService();
                        CusPk = hdfCustomerID.Value != string.Empty ? Convert.ToInt32(hdfCustomerID.Value) : 0;
                        if (CusPk > 0)
                        {
                            crmCustomerMstList = cm.GetCustomers(CusPk);
                            dsPageData = BusinessLogic.Sales.CustomerProduct.GetCustomer(CusPk, string.Empty, currentUser.SBUID, 2);
                        }
                        break; 
                    #endregion
                    #region FROM PORT
                    case ControlsEnum.FROMPORT:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.Shipment, (int)ConstShipmentType.FromPort, 1, currentUser.SBUID);
                        break; 
                    #endregion
                    #region SO INV HEADER
                    case ControlsEnum.SOINVHEADER:
                        SoHeaderObj = new SOHeaderBO();
                        SoHeaderObj.SOList = new List<SOHeaderListBO>();
                        List<SOHeaderListBO> objItemList = new List<SOHeaderListBO>();
                        SOHeaderListBO objSoList;
                        objSoList = new SOHeaderListBO();
                        objSoList.SOH_PK = CurrSOPK;
                        objItemList.Add(objSoList);

                        SoHeaderObj.SOList = objItemList;
                        string xmlDocSO = "";
                        if (SoHeaderObj != null && SoHeaderObj.SOList != null && SoHeaderObj.SOList.Count > 0)
                        {
                            xmlDocSO = CommonFunctions.XmlSerialize<SOHeaderBO>(SoHeaderObj);
                        }
                        invoiceHeaderObj = BusinessLogic.Sales.SalesInvoiceBL.GetSalesInvoiceHeaderMUL(xmlDocSO, CurrPK).SOMainList.FirstOrDefault();
                        SOInvoiceHeaderSession = invoiceHeaderObj;
                        EnableDisableInvoiceType();
                        if (invoiceHeaderObj == null && CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowError", "ShowErrorMessage('" + Resources.ErrorMessages.Msg_Edit_Delete + "','" + Resources.Captions.Information + "');", true);
                        }
                        break; 
                    #endregion
                    #region INVOICE TYPE
                    case ControlsEnum.INVOICETYPE:
                        dtInvoiceType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SALES INVOICE TYPE", "Regular");
                        break; 
                    #endregion
                    #region INVOICE GST TYPE
                    case ControlsEnum.INVOICEGSTTYPE:
                        dtInvoiceGstType = BusinessLogic.CommonManagement.CommonBL.GetInvoiceGstType(Convert.ToInt16(CommonConstants.SELECT_VALUE_ZERO), Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, Convert.ToInt16(ddlInvoiceType.SelectedValue), Convert.ToInt16(GTIService.Constants.Common.InvoiceType.Sales));
                        break; 
                    #endregion
                    #region SOT YPE
                    case ControlsEnum.SOTYPE:
                        dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SO TYPE");
                        break; 
                    #endregion
                    #region TAX TYPES
                    case ControlsEnum.TAXTYPES:
                        string P_XML = "";
                        int category = 1;
                        int.TryParse(hdfTaxCategory.Value, out category);
                        if (TaxPK > 0)
                        {
                            dsTaxDetails = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetRFQTaxDetails(TaxPK, 0, currentUser.SBUID, Convert.ToByte(DbActiveStatus.HASPK), 0);
                            if (dsTaxDetails != null && dsTaxDetails.Tables.Count > 0)
                            {
                                dtTaxDetails = dsTaxDetails.Tables[0];
                            }
                        }
                        else
                        {
                            if ((int)TaxType.Shipping == category)//shipping is not under sales or Purchase
                            {
                                string CatXML = CommonFunctions.GetOtherChargeCatXML(GetLocalResourceObject("OtherChargeCategoryValues").ToString());               
                                if (CatXML != null || CatXML != string.Empty)
                                {
                                    category = 0;
                                }
                                dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category,  currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, TaxFilterType.SAL, 1,0,0,CatXML);                           
                            }
                            else
                            {
                                if ((int)TaxType.Discount != category)
                                {
                                    dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, TaxFilterType.SAL, 1, 0, 1);
                                }
                                else
                                {
                                    dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0);
                                }
                            }

                            //if ((int)TaxType.Discount != category)
                            //{
                            //    dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0, TaxFilterType.SAL, 1);

                            //    //  dtSaleOrderTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, (IsTaxInSBU == true ? 0 : currentUser.SBUID), Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtSaleOrderDate.Text), 0, TaxFilterType.SAL, 1);
                            //}
                            //else
                            //{
                            //    dtTaxDetails = BusinessLogic.Administration.Masters.TaxSettingsMaster.GetActiveTaxCategoryDateValue(category, currentUser.SBUID, Convert.ToByte(DbActiveStatus.ACTIVE), Convert.ToDateTime(txtInvoiceDate.Text), 0);
                            //}
                        }
                        break; 
                    #endregion
                    #region EXCHANGE RATE
                    case ControlsEnum.EXCHANGERATE:
                        DataSet dsExchangeRate = BusinessLogic.PurchaseOrderManagement.RequestForQuote.GetExchangeRate(Convert.ToInt32(hdfCurrency.Value), currentUser.BaseCurrency, Convert.ToDateTime(txtInvoiceDate.Text.Trim()));
                        if (dsExchangeRate != null && dsExchangeRate.Tables[0].Rows.Count > 0)
                        {
                            hdfExchangeRate.Value = dsExchangeRate.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            hdfExchangeRate.Value = "-1";
                        }
                        //  Exchange rate field is not editable(Domestic).ie,If selected currency is same as SBU base currency
                        if (currentUser.BaseCurrency == Convert.ToInt32(hdfCurrency.Value))
                        {
                            txtExchangeRate.Enabled = false;
                        }
                        else
                        {
                            txtExchangeRate.Enabled = true;
                        }
                        break; 
                    #endregion
                    #region INVOICE GET
                    case ControlsEnum.INVOICEGET:
                        #region MISCLIST
                        int GInvPk = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        int statusFilter = 3;
                        dsPageData = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.SalesInvoiceDate : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenBy) ? Resources.DataFieldRes.SalesInvoiceNo : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.Empty,
                                ToDate = string.Empty,
                                SearchBy = "ICH_PK",
                                SearchValue = GInvPk.ToString()
                            }, currentUser, 0, 0, 0, string.Empty, 0, txtSCno.Text, Resources.PageURL.Misc.Replace("~", ""), "", statusFilter, 0, group: (byte)SalesInvoiceGroup.Miscellaneous, category: 1);
                        if (dsPageData != null)
                        {
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            dtMISCLIST = dvInvoice.ToTable();
                        }

                        #endregion
                        break; 
                    #endregion
                    #region MISC LIST
                    case ControlsEnum.MISCLIST:
                        int cusID = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        int cmpPk = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                        }
                        int InvPk = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        int Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int InvType = Convert.ToInt32(ddlSaleOrderType.SelectedValue);
                        string customer = string.IsNullOrEmpty(txtCustomerLIST.Text.Trim()) ? string.Empty : (txtCustomerLIST.Text.Trim() == "Select/Type" ? string.Empty : txtCustomerLIST.Text.Trim());

                        dsPageData = BusinessLogic.Sales.SaleOrderBL.GetSalesInvoiceList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = string.IsNullOrEmpty(SortBy) ? Resources.DataFieldRes.SalesInvoiceDate : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                ThenBy = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenBy) ? Resources.DataFieldRes.SalesInvoiceNo : ThenBy,
                                ThenDirection = SortBy == ThenBy || SortBy == Resources.DataFieldRes.SalesInvoiceNo ? string.Empty : string.IsNullOrEmpty(ThenDirection) ? Resources.Report.SortDescending : ThenDirection,
                                FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim(),
                                SearchBy = "ICH_NO",
                                SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == "Select/Type" ? string.Empty : txtInvoiceNumber.Text.Trim()),
                                CompanyPK = cmpPk
                            }, currentUser, cusID, InvPk, 0, HttpUtility.HtmlEncode(customer), InvType, txtSCno.Text, Resources.PageURL.Misc.Replace("~", ""), "", Status, Convert.ToInt32(chkBalAmt.Checked), group: (byte)SalesInvoiceGroup.Miscellaneous, category: 1);
                        if (dsPageData != null)
                        {
                            //string customer = string.IsNullOrEmpty(txtCustomerLIST.Text.Trim()) ? string.Empty : (txtCustomerLIST.Text.Trim() == "Select/Type" ? string.Empty : txtCustomerLIST.Text.Trim());
                            DataView dvInvoice = dsPageData.Tables[1].DefaultView;
                            //if (customer != string.Empty)
                            //{
                            //    dvInvoice.RowFilter = " ICH_CUSTOMER_TEXT='" + customer + "'";
                            //}

                            //if (txtSCno.Text != string.Empty)
                            //{
                            //    dvInvoice.RowFilter = " SOH_NO like '%" + txtSCno.Text + "%'";
                            //}
                            ////if (ddlSaleOrderType.SelectedValue != CommonConstants.SELECTVAL)
                            ////{
                            ////    dvInvoice.RowFilter = " ICH_TYPE_TEXT='" + ddlSaleOrderType.SelectedItem.Text + "'";
                            ////}
                            dtMISCLIST = dvInvoice.ToTable();
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
                    #region GET EXPENSE PK BY JOURNAL PK
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
                    #endregion
                    #region TAX SETTINGS
                    case ControlsEnum.TAXSETTINGS:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ItemWiseTaxSetting, string.Empty, currentUser.SBUID);
                        break; 
                    #endregion
                    #region DEFAULT UOM
                    case ControlsEnum.DEFAULTUOM:
                        dtPageData = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration(CFGType.ExpenseInvUOM, string.Empty, currentUser.SBUID);
                        break; 
                    #endregion
                    #region PaymentTerms
                    case ControlsEnum.PAYMENTTERMS:
                        dtPaymentTerms = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, CusPk, (int)CustomerTermType.PaymentTerms, 1);
                        //dtPaymentTerms = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(0, CusPk, (int)CustomerTermType.PaymentTerms, paymentTermPK > 0 ? 2 : 1);
                        //dtPaymentTerms = BusinessLogic.Sales.CustomerProduct.GetCustomerTerms(paymentTermPK, CusPk, (int)CustomerTermType.PaymentTerms, 0);
                        break;
                    #endregion
                    #region DueDate
                    case ControlsEnum.GETDUEDATE:
                        objDueDateDtls = (DueDateDetails)SetUIValuesToObject(ControlsEnum.GETDUEDATE);
                        string xmlDoc = CommonFunctions.XmlSerialize<DueDateDetails>(objDueDateDtls);
                        dsDueDate = BusinessLogic.Sales.SalesInvoiceBL.GetDueDateByPaymentTerms(xmlDoc);
                        break;
                    #endregion
                    #region CustomerTypes
                    case ControlsEnum.CUSTOMERTYPES:
                        dsCustomerTypes = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(0, custPK, 1, null);

                        break;
                    #endregion
                    #region GetCustomerDetailsByCustomerType
                    case ControlsEnum.GETCUSTOMERDETAILSBYTYPE:
                        int CAD_PK = 0;
                        int.TryParse(ddlCustomerType.SelectedValue, out CAD_PK);
                        int CustomerPK = 0;
                        //int.TryParse(hdfCurrCustomerPK.Value, out CustomerPK);
                        dsCustomerDetailsByType = BusinessLogic.Sales.CustomerProduct.GetCustomerAddress(CAD_PK, CustomerPK, 2, 0);
                        break;
                    #endregion
                    #region CUSTOMERSUPPLYTAX
                    case ControlsEnum.CUSTOMERRELATEDTAX:
                        dtCusTaxDetails = BusinessLogic.CommonManagement.CommonBL.GetCustomerSupplyTax(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, string.IsNullOrEmpty(txtInvoiceDate.Text) ? DateTime.Now : Convert.ToDateTime(txtInvoiceDate.Text), Convert.ToInt32(CusPk), null);
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
                    #region ISSUE TYPE
                    case ControlsEnum.ISSUETYPE:
                        dtIssueType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "ISSUE TYPE");
                        break; 
                    #endregion
                    #region ISSUE DETAILS SPLIT UP
                    case ControlsEnum.ISSUEDETAILSSPLITUP:
                        int ItemPk = 0, customerPk = 0, issueType = 0;
                        int.TryParse(hdfItemID.Value, out ItemPk);
                        customerPk = hdfCustomerID.Value != string.Empty ? Convert.ToInt32(hdfCustomerID.Value) : 0;
                        issueType = ddlIssueType.SelectedValue == "-1" ? 0 : Convert.ToInt32(ddlIssueType.SelectedValue);
                        issueListForGrid = new List<IssueDetail>();
                        SOInvoiceHeaderMul invoiceHeadermulObj = new SOInvoiceHeaderMul();
                        invoiceHeadermulObj = BusinessLogic.Sales.SalesInvoiceBL.GetIssueDetail(customerPk, ItemPk, issueType);
                        if (invoiceHeadermulObj != null)
                            issueListForGrid = invoiceHeadermulObj.SOMainList[0].OrderDetail[0].IssueDtl;
                        break; 
                    #endregion
                    #region GST SUB TYPE
                    case ControlsEnum.GSTSUBTYPE:
                        dtGstSubType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "EXP TYPE");
                        break;
                    #endregion
                    #region AMOUNT DETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        dtAmountDetails = new DataTable();                        
                        dtAmountDetails = BusinessLogic.Sales.SalesInvoiceBL.GetInvCusReceivedAmntDetails(Convert.ToInt16(InvoicePk)).Tables[0];

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
                    #region INVOICE GET
                    case ControlsEnum.INVOICEGET:
                        GetUIValuesFromObject(controlType);
                        break; 
                    #endregion
                    #region INVOICE GST TYPE
                    case ControlsEnum.INVOICEGSTTYPE:
                    case ControlsEnum.SALECATEGORY:
                        BindDropDown(controlType);
                        break; 
                    #endregion
                    #region SO TYPE
                    case ControlsEnum.SOTYPE:
                        BindDropDown(controlType);
                        break; 
                    #endregion
                    #region INVOICE TYPE
                    case ControlsEnum.INVOICETYPE:
                        BindDropDown(controlType);
                        break; 
                    #endregion
                    #region FROM PORT
                    case ControlsEnum.FROMPORT:
                        BindDropDown(controlType);
                        break; 
                    #endregion
                    #region SO INV HEADER
                    case ControlsEnum.SOINVHEADER:
                        GetUIValuesFromObject(controlType);
                        break; 
                    #endregion
                    #region EXPENSE DETAIL
                    case ControlsEnum.EXPENSEDETAIL:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region TAX TYPES
                    case ControlsEnum.TAXTYPES:
                        BindDropDown(controlType);
                        break; 
                    #endregion
                    #region TAX POPUP GRID
                    case ControlsEnum.TAXPOPUPGRID:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region MISC LIST
                    case ControlsEnum.MISCLIST:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break; 
                    #endregion
                    #region COMPANY SRCH
                    case ControlsEnum.COMPANYSRCH:
                        BindDropDown(ControlsEnum.COMPANYSRCH);
                        break; 
                    #endregion
                    #region PAYMENT TERMS
                    case ControlsEnum.PAYMENTTERMS:
                        BindDropDown(ControlsEnum.PAYMENTTERMS);
                        break; 
                    #endregion
                    #region CUSTOMER SELECTED
                    case ControlsEnum.CUSTOMERSELECTED:
                        if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                        {
                            txtCurrency.Text = crmCustomerMstList[0].ADM_CURRENCY_MST.CUR_CODE + " - " + crmCustomerMstList[0].ADM_CURRENCY_MST.CUR_NAME;
                            hdfCurrency.Value = crmCustomerMstList[0].CUS_CURRENCY.ToString();
                            //txtInvoiceDueDate.Enabled = true;
                            //txtInvoiceDueDate.CssClass = "medium hasDatepicker";
                            chkDummy.Checked = divDummy.Visible = crmCustomerMstList[0].CUS_IS_DUMMY == 1 ? true : false;

                            if (dsPageData != null && dsPageData.Tables.Count > 0 && dsPageData.Tables[0].Rows.Count > 0)
                            {
                                // if (!dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].Equals(DBNull.Value) && ddlInvoiceType.Items.FindByValue(dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString()) != null)
                                ddlInvoiceType.SelectedValue = dsPageData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString();
                                txtShipTo.Text = HttpUtility.HtmlDecode((dsPageData.Tables[0].Rows[0]["CUS_ADDRESS"].ToString()
                                                 + "\n" + dsPageData.Tables[0].Rows[0]["CUS_ZIP"].ToString()
                                                 + " " + dsPageData.Tables[0].Rows[0]["CUS_COUNTRY_TEXT"].ToString()));
                                GetFieldValues(ControlsEnum.EXCHANGERATE);
                                txtExchangeRate.Text = Convert.ToDouble(hdfExchangeRate.Value).ToString();
                                if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == (int)SalesInvoiceType.Export && IsInvoiceGSTEnable)
                                {
                                    ddlSubType.Visible = true;
                                    lblSubType.Visible = true;
                                }
                                else
                                {
                                    ddlSubType.ClearSelection();
                                    ddlSubType.Visible = false;
                                    lblSubType.Visible = false;
                                }
                            }

                            //ddlInvoiceType.SelectedValue = crmCustomerMstList[0].SAL_ORDER_HDR. .CUS_TYPE.ToString();
                            //SAL_ORDER_HDR
                        }
                        break; 
                    #endregion
                    #region GET DUEDATE
                    case ControlsEnum.GETDUEDATE:
                        if (dsDueDate != null & dsDueDate.Tables[0].Rows.Count > 0)
                        {
                            txtInvoiceDueDate.Text = Convert.ToDateTime(dsDueDate.Tables[0].Rows[0]["DUE_DATE"].ToString()).ToString(Resources.Constants.DateFormatShort);
                        }
                        else
                        {
                            txtInvoiceDueDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        }
                        break; 
                    #endregion
                    #region CUSTOMER TYPES
                    case ControlsEnum.CUSTOMERTYPES:
                        BindDropDown(ControlsEnum.CUSTOMERTYPES);
                        break;
                    #endregion
                    #region OTHERCHARGELIST
                    case ControlsEnum.OTHERCHARGELIST:
                        BindGrid(ControlsEnum.OTHERCHARGELIST);
                        break;
                    #endregion
                    #region GET CUSTOMER DETAILS BY TYPE
                    case ControlsEnum.GETCUSTOMERDETAILSBYTYPE:
                        if (ddlCustomerType.SelectedValue == CommonConstants.SELECTVAL)
                        {

                            txtTypeID.Text = "";
                            txtTaxID.Text = "";
                        }
                        else
                        {
                            if (dsCustomerDetailsByType != null & dsCustomerDetailsByType.Tables[0].Rows.Count > 0)
                            {
                                txtTypeID.Text = dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE_NAME"].ToString();
                                txtTaxID.Text = HttpUtility.HtmlDecode(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TAX_NO"].ToString());

                                hdfCustomerTypeId.Value = Convert.ToString(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE"]);
                                if (Convert.ToInt32(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE"]) == (int)CustomerContactTypeEnum.Branch)
                                {
                                    txtTypeID.Enabled = true;
                                    vrfBranchCode.Enabled = true;
                                    txtTypeID.CssClass = "input-small";
                                    txtTypeID.Width = Unit.Pixel(115);
                                }
                                else
                                {
                                    txtTypeID.Enabled = false;
                                    txtTypeID.Text = "00000";
                                    vrfBranchCode.Enabled = false;
                                    txtTypeID.Width = Unit.Pixel(115);
                                    txtTypeID.CssClass = "input-small input-disabled";
                                }
                            }
                            else
                            {
                                txtTypeID.Text = "";
                                txtTaxID.Text = "";
                            }
                        }
                        break; 
                    #endregion
                    #region CUSTOMER RELATED TAX
                    case ControlsEnum.CUSTOMERRELATEDTAX:
                        hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                        if (dtCusTaxDetails != null && dtCusTaxDetails.Rows.Count > 0)
                        {
                            invoiceHeaderObj = TempSOInvoiceHeaderSession;
                            taxHdrList = new List<SOInvoiceTaxHdr>();

                            //taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                            for (int i = 0; i < dtCusTaxDetails.Rows.Count; i++)
                            {
                                soInvTaxHdrObj = new SOInvoiceTaxHdr();
                                soInvTaxHdrObj.CIT_TAX_AMT = invoiceHeaderObj.ICH_AMOUNT_TC;//Convert.ToDouble(invoiceHeaderObj.ICH_AMOUNT_TC) == 0 ? 0.00 :
                                soInvTaxHdrObj.CIT_TAX = Convert.ToInt32(dtCusTaxDetails.Rows[i]["TAX_PK"]);
                                soInvTaxHdrObj.CIT_TAX_TEXT = string.IsNullOrEmpty(dtCusTaxDetails.Rows[i]["TAX_HEAD"].ToString()) ? null : dtCusTaxDetails.Rows[i]["TAX_HEAD"].ToString();
                                soInvTaxHdrObj.CIT_NAME = string.IsNullOrEmpty(dtCusTaxDetails.Rows[i]["CMT_TAX_TEXT"].ToString()) ? null : dtCusTaxDetails.Rows[i]["CMT_TAX_TEXT"].ToString();
                                soInvTaxHdrObj.CIT_TAX_CATEGORY = Convert.ToInt32(dtCusTaxDetails.Rows[i]["TAX_CATEGORY"]);
                                //soInvTaxHdrObj.CIT_TYPE = Convert.ToInt32(dtCusTaxDetails.Rows[i]["TAX_TYPE"]);
                                soInvTaxHdrObj.CIT_TAX_FORMULA = dtCusTaxDetails.Rows[i]["TAX_FORMULA"].ToString();
                                soInvTaxHdrObj.CIT_TAX_CODE = dtCusTaxDetails.Rows[i]["TAX_CODE"].ToString();
                                soInvTaxHdrObj.CIT_TAX_RATE = string.IsNullOrEmpty(dtCusTaxDetails.Rows[i]["TAX_RATE"].ToString()) ? 0 : Convert.ToDouble(dtCusTaxDetails.Rows[i]["TAX_RATE"]);
                                taxHdrList.Add(soInvTaxHdrObj);
                            }
                            invoiceHeaderObj.TaxHdr = taxHdrList;
                            txtHdrTax.Text = "0.00";
                            TempSOInvoiceHeaderSession = invoiceHeaderObj;
                            //IsHeaderTax = true;                           
                            SOInvoiceHeaderSession = TempSOInvoiceHeaderSession;
                        }
                        else
                        {
                            invoiceHeaderObj.TaxHdr = null;
                            txtHdrTax.Text = "0.00";
                            TempSOInvoiceHeaderSession = invoiceHeaderObj;
                            //IsHeaderTax = true;                           
                            SOInvoiceHeaderSession = TempSOInvoiceHeaderSession;

                        }
                        break; 
                    #endregion
                    #region SHOW DIV TAX BC
                    case ControlsEnum.SHOWDIVTAXBC:
                        BindGrid(controlType);
                        break; 
                    #endregion
                    #region ISSUEDETAILSSPLITUP
                    case ControlsEnum.ISSUEDETAILSSPLITUP:
                        BindGrid(ControlsEnum.ISSUEDETAILSSPLITUP);
                        break;
                    #endregion
                    #region ISSUEDETAILSSPLITUPAPPLIED
                    case ControlsEnum.ISSUEDETAILSSPLITUPAPPLIED:
                        BindGrid(ControlsEnum.ISSUEDETAILSSPLITUPAPPLIED);
                        break;
                    #endregion
                    #region ISSUETYPE
                    case ControlsEnum.ISSUETYPE:
                        BindDropDown(ControlsEnum.ISSUETYPE);
                        break;
                    #endregion
                    #region GST SUB TYPE
                    case ControlsEnum.GSTSUBTYPE:
                        BindDropDown(ControlsEnum.GSTSUBTYPE);
                        break; 
                    #endregion
                    #region AMOUNT DETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
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
        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            btnDOPrint.Visible = false;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("EXPENSE ITEM WISE SETTINGS", string.Empty, currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                isTaxAdd = string.IsNullOrEmpty(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString()) ? 1 : Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "TAX")["ACF_VALUE"].ToString());
                isDiscountAdd = string.IsNullOrEmpty(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString()) ? 1 : Convert.ToInt32(dt.AsEnumerable().SingleOrDefault(stg => stg.Field<string>("ACF_DATA") == "DISCOUNT")["ACF_VALUE"].ToString());
                EnableItemTax = isTaxAdd == 1 ? true : false;
                EnableItemDiscount = isDiscountAdd == 1 ? true : false;
            }
            if (GetGlobalResourceObject("ConfigurationsRes", "DOPrintRequiredInMisc").ToString() == "1")
                btnDOPrint.Visible = true;
            hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
            IsInvoiceGSTEnable = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableGST")));
            IsTaxForOtherCharge = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales")));
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
            hdfEnableAddlOtherCharge.Value = GetGlobalResourceObject("ConfigurationsRes", "EnableSalesInvoiceOtherCharges").ToString();

            if (GetGlobalResourceObject("ConfigurationsRes", "OtherChargeVisible").ToString() == "0")
            {
                divOC.Visible = false;
            }

        }

        ///// <summary>
        ///// 
        ///// </summary>
        private void ConfigurationSettingsforTaxBC()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetCurrencyConfiguration("CURRENCY SETTINGS", "ShowAmountInBC", currentUser.SBUID);
            if (dt != null && dt.Rows.Count > 0)
            {
                hdfIsTaxPayable.Value = dt.Rows[0]["ACF_VALUE"].ToString();//If 1 Show Div taxpayable else hide  
            }
        }


        #region Set BranchID Enable/Disable
        private void SetBranchIDEnableDisable()
        {
            if (ddlCustomerType.SelectedValue != "")
            {
                GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
            }

        }
        #endregion


        /// <summary>
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            cm = new CommonService();
            AppTypeDetailsList = cm.GetReportParameters(ApplicationType.MSI, 0, DateTime.Now);
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
            decimal balamt = 0;
            decimal invValue = 0;
            int selectedInvoice;
            int selectedCurrency;
            int selectedInvoiceType;
            int selectedCustomer;
            int approvedStatus;
            bool isPosted;

            selectedInvoice = 0;
            approvedStatus = 0;
            selectedCustomer = 0;
            selectedCurrency = 0;
            selectedInvoiceType = 0;
            isPosted = false;
            AlertBO alertBoObj;


            try
            {
                switch (controlType)
                {
                    #region Misc
                    case ControlsEnum.SOINVHEADER:
                        if (SOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = SOInvoiceHeaderSession;
                            invoiceHeaderObj.ICH_PK = CurrPK;
                            //if (DespatchID > 0 && CurrPK == 0)
                            //{
                            //    invoiceHeaderObj.ICH_DESPATCH_HDR = DespatchID.ToString();
                            //}
                            //string.IsNullOrEmpty(hdfResponsePK.Value) ? 0 : Convert.ToInt32(hdfResponsePK.Value);
                            invoiceHeaderObj.ICH_NO = lblInvoiceNo.Text == "[NEW]" ? string.Empty : lblInvoiceNo.Text;
                            invoiceHeaderObj.ICH_VERSION = 1;
                            invoiceHeaderObj.ICH_STATUS = 0;

                            invoiceHeaderObj.ICH_DATE = string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDate.Text.Trim();
                            //invoiceHeaderObj.ICH_ = CurrPK;
                            invoiceHeaderObj.ICH_DATE_PAY_BY = string.IsNullOrEmpty(txtInvoiceDueDate.Text.Trim()) ? DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDueDate.Text.Trim();
                            invoiceHeaderObj.ICH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                            invoiceHeaderObj.ICH_SHIP_TO_ADDRESS = HttpUtility.HtmlEncode(txtShipTo.Text);
                            invoiceHeaderObj.ICH_REFERENCE = HttpUtility.HtmlEncode(txtReference.Text);
                            invoiceHeaderObj.ICH_CUSTOMER = HttpUtility.HtmlDecode(hdfCustomerID.Value);
                            invoiceHeaderObj.ICH_CUSTOMER_NAME = HttpUtility.HtmlDecode(txtCustomer.Text);
                            invoiceHeaderObj.ICH_CURRENCY = Convert.ToInt32(hdfCurrency.Value);
                            invoiceHeaderObj.ICH_TYPE = ddlInvoiceType.SelectedValue;
                            if (Convert.ToInt32(ddlInvoiceGstType.SelectedValue) > 0)
                                invoiceHeaderObj.ICH_SALES_CATEGORY = Convert.ToInt32(ddlSaleCategory.SelectedValue);
                         
                            if (Convert.ToInt32(ddlInvoiceGstType.SelectedValue) > 0)
                                invoiceHeaderObj.ICH_GST_TYPE = ddlInvoiceGstType.SelectedValue;
                            invoiceHeaderObj.ICH_INV_IS_DUMMY = (chkDummy.Checked) ? (short)1 : (short)0;
                            //Adding New fields  Branch/HeadOffice Pk,Type,Branch id and taxid
                            if (ddlCustomerType.Items.Count > 0)
                                invoiceHeaderObj.ICH_BRANCH = ddlCustomerType.SelectedValue;//selected Branch/HO Name PK
                            //Fetching Type with respect to selected Pk;
                            invoiceHeaderObj.ICH_BRANCH_TYPE = hdfCustomerTypeId.Value.ToString();//HO/Branch(4/5)
                            invoiceHeaderObj.ICH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtTypeID.Text);
                            invoiceHeaderObj.ICH_TAX_ID = HttpUtility.HtmlEncode(txtTaxID.Text);

                            invoiceHeaderObj.ICH_FEEDER_VESSEL = HttpUtility.HtmlEncode(txtFeederVessel.Text);
                            invoiceHeaderObj.ICH_MOTHER_VESSEL = HttpUtility.HtmlEncode(txtMotherVessel.Text);
                            invoiceHeaderObj.ICH_CONTAINER_NO = HttpUtility.HtmlEncode(txtContainerNo.Text);
                            invoiceHeaderObj.ICH_SHIPMENT_TERM = HttpUtility.HtmlEncode(txtShipmentTerm.Text);

                            //if (Convert.ToInt16(ddlFromPort.SelectedValue) > 0)
                            //{
                            //    invoiceHeaderObj.ICH_FROM_PORT = ddlFromPort.SelectedValue == "-1" ? "0" : ddlFromPort.SelectedValue;
                            //}

                            invoiceHeaderObj.ICH_FROM_PORT = hdfFromPortID.Value.ToString();
                            invoiceHeaderObj.ICH_TO_PORT = HttpUtility.HtmlEncode(txtFDest.Text);

                            invoiceHeaderObj.ICH_NET_WT = string.IsNullOrEmpty(txtNetWt.Text.Trim()) ? 0 : Convert.ToDouble(txtNetWt.Text.Trim());
                            invoiceHeaderObj.ICH_GROSS_WT = string.IsNullOrEmpty(txtGrossWt.Text.Trim()) ? 0 : Convert.ToDouble(txtGrossWt.Text.Trim());
                            invoiceHeaderObj.ICH_TERM1 = HttpUtility.HtmlEncode(txtTerm1.Text);
                            invoiceHeaderObj.ICH_TERM2 = HttpUtility.HtmlEncode(txtTerm2.Text);

                            invoiceHeaderObj.ICH_WT_UOM = string.IsNullOrEmpty(hdfUOMwt.Value.ToString()) ? 0 : Convert.ToInt32(hdfUOMwt.Value);

                            invoiceHeaderObj.ICH_DATE_PAY_BY = string.IsNullOrEmpty(txtInvoiceDueDate.Text.Trim()) ?
                                                           DateTime.Now.ToString(Resources.Constants.DateFormatShort) : txtInvoiceDueDate.Text.Trim();
                            invoiceHeaderObj.ICH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);

                            if (ddlPaymentTerms.SelectedValue != CommonConstants.SELECTVAL)
                            {
                                invoiceHeaderObj.ICH_PAYMENT_TERM = ddlPaymentTerms.SelectedValue;
                                invoiceHeaderObj.ICH_PAYMENT_TERM_TEXT = ddlPaymentTerms.SelectedItem.Text;
                            }
                            else
                            {
                                invoiceHeaderObj.ICH_PAYMENT_TERM = string.Empty;
                                invoiceHeaderObj.ICH_PAYMENT_TERM_TEXT = string.Empty;
                            }
                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                invoiceHeaderObj.ICH_COMPANY = Convert.ToInt16(ddlCompanyView.SelectedValue);
                            }
                            else
                            {
                                invoiceHeaderObj.ICH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);
                            }                          
                          
                            invoiceHeaderObj.ICH_DISCOUNT_TC = string.IsNullOrEmpty(txtHdrDiscount.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrDiscount.Text.Trim());
                            invoiceHeaderObj.ICH_SHIP_CHARGE = string.IsNullOrEmpty(txtShipping.Text.Trim()) ? 0 : Convert.ToDouble(txtShipping.Text.Trim());
                            invoiceHeaderObj.ICH_TAX_TC = string.IsNullOrEmpty(txtHdrTax.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTax.Text.Trim());
                            invoiceHeaderObj.ICH_AMOUNT_ADJUST = string.IsNullOrEmpty(txtPriceAdj.Text.Trim()) ? 0 : Convert.ToDouble(txtPriceAdj.Text.Trim());
                            invoiceHeaderObj.ICH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());
                            invoiceHeaderObj.ICH_NET_VALUE_TC = string.IsNullOrEmpty(txtHdrTotal.Text.Trim()) ? 0 : Convert.ToDouble(txtHdrTotal.Text.Trim());
                            invoiceHeaderObj.ICH_BASE_CURR = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                            //GetFieldValues(ControlsEnum.EXCHANGERATE);
                            invoiceHeaderObj.ICH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeRate.Value) ? 1 : Convert.ToDouble(hdfExchangeRate.Value);
                            invoiceHeaderObj.ICH_AMOUNT_NET_BC = invoiceHeaderObj.ICH_AMOUNT_NET_TC * invoiceHeaderObj.ICH_EXCHG_RATE;

                            if (!string.IsNullOrEmpty(txtETD.Text))
                            {
                                invoiceHeaderObj.ICH_ETD = txtETD.Text;
                            }
                            else
                            {
                                invoiceHeaderObj.ICH_ETD = DBNull.Value.ToString(); //DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            }
                            if (!string.IsNullOrEmpty(txtETA.Text))
                            {
                                invoiceHeaderObj.ICH_ETA = txtETA.Text;
                            }
                            else
                            {
                                invoiceHeaderObj.ICH_ETA = DBNull.Value.ToString(); // DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            }


                            invoiceHeaderObj.ICH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            invoiceHeaderObj.ICH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            invoiceHeaderObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            invoiceHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            invoiceHeaderObj.LAST_MOD_DT = LastModifiedTime;

                            invoiceHeaderObj.ICH_GROUP = (byte)SalesInvoiceGroup.Miscellaneous;
                            invoiceHeaderObj.ICH_CATEGORY = (byte)SalesInvoiceCategory.Invoice;
                            //invoiceHeaderObj.ICH_GROUP = (byte)SalesInvoiceGroup.Miscellaneous;
                            //invoiceHeaderObj.ICH_CATEGORY = (byte)SalesInvoiceCategory.Invoice;
                            invoiceHeaderObj.ICH_TYPE = Convert.ToInt16(ddlInvoiceType.SelectedValue).ToString();
                            if (Convert.ToInt32(ddlSaleCategory.SelectedValue) > 0)
                            invoiceHeaderObj.ICH_SALES_CATEGORY = Convert.ToInt16(ddlSaleCategory.SelectedValue);
                            if (Convert.ToInt32(ddlInvoiceGstType.SelectedValue) > 0)
                                invoiceHeaderObj.ICH_GST_TYPE = Convert.ToInt16(ddlInvoiceGstType.SelectedValue).ToString();
                            invoiceHeaderObj.ICH_IS_OPENING = 0;

                            int subtype = 0;
                            int.TryParse(ddlSubType.SelectedValue, out subtype);
                            if (subtype > 0)
                                invoiceHeaderObj.ICH_SUB_TYPE = subtype.ToString();
                            else
                                invoiceHeaderObj.ICH_SUB_TYPE = null;


                            invoiceHeaderObj.ICH_TOTAL_QTY = 0;//Dummy
                            invoiceHeaderObj.AST_CODE = ApplicationType.MSI;
                            invoiceHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;

                        }
                        invoiceHeaderMulObj = new SOInvoiceHeaderMul();
                        invoiceHeaderMulObj.SOMainList = new List<SOInvoiceHeader>();
                        invoiceHeaderMulObj.SOMainList.Add(invoiceHeaderObj);
                        retObject = invoiceHeaderMulObj;
                        break;
                    #endregion
                    #region Items
                    case ControlsEnum.EXPENSEDETAIL:
                        if (CurrSlNo != 0 && soMiscDetailsList != null)
                        {
                            soMiscDetailsObj = soMiscDetailsList.SingleOrDefault(itm => itm.CID_SL_NO == CurrSlNo);
                            if (soMiscDetailsObj != null)
                            {
                                soMiscDetailsObj.CID_PK = Convert.ToInt32(hdfDetailPK.Value);
                                int ItemId = 0;
                                int.TryParse(hdfItemID.Value, out ItemId);
                                soMiscDetailsObj.CID_ITEM = ItemId;
                                //soMiscDetailsObj.CID_ITEM = Convert.ToInt32(HttpUtility.HtmlEncode(hdfItemID.Value));
                                //soMiscDetailsObj.CID_ITEM_TEXT = HttpUtility.HtmlEncode(txtItem.Text);
                                soMiscDetailsObj.CID_ITEM_TEXT = (txtItem.Text.Equals("Select/Type") || txtItem.Text.Equals("Type min. 3 characters")) ? string.Empty : HttpUtility.HtmlEncode(txtItem.Text);
                                soMiscDetailsObj.CID_INSTRUCTIONS = HttpUtility.HtmlEncode(txtDesc.Text);
                                soMiscDetailsObj.CID_QTY_INVOICED = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                                soMiscDetailsObj.CID_UOM = Convert.ToInt32(hdfUOM.Value);
                                soMiscDetailsObj.CID_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                                //soMiscDetailsObj.CID_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                                //    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));

                                soMiscDetailsObj.CID_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit] == null
                                   ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit])));

                                soMiscDetailsObj.CID_DISCOUNT = !string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                soMiscDetailsObj.CID_AMOUNT = !string.IsNullOrEmpty(txtAmount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                //soMiscDetailsObj.CID_AMOUNT = !string.IsNullOrEmpty(txtAmount.Text.Trim()) ? Math.Round(Convert.ToDouble(txtAmount.Text), (Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit] == null
                                //   ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit]))) : 0;

                                soMiscDetailsObj.CID_TAX = !string.IsNullOrEmpty(txtTax.Text.Trim()) ? Math.Round(Convert.ToDouble(txtTax.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits) : 0;
                                soMiscDetailsObj.CID_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);

                                if (IssueListApplied != null && IssueListApplied.Count > 0)
                                {
                                    IssueListApplied.ForEach(dtl => dtl.CII_SL_NO = soMiscDetailsObj.CID_SL_NO);
                                    IssueListApplied.ForEach(dtl => dtl.CII_UOM = soMiscDetailsObj.CID_UOM);
                                    IssueListApplied.ForEach(dtl => dtl.CII_INVOICE_DTL = soMiscDetailsObj.CID_PK);
                                }
                                soMiscDetailsObj.IssueDtl = IssueListApplied;
                            }
                        }
                        else
                        {
                            int slno = 1;
                            if (soMiscDetailsList == null || soMiscDetailsList.Count == 0)
                            {
                                soMiscDetailsList = new List<SOInvoiceDetails>();
                                slno = 1;
                            }
                            else
                            {
                                slno = soMiscDetailsList.Max(itm => itm.CID_SL_NO);
                                slno++;
                            }
                            soMiscDetailsObj = new SOInvoiceDetails();
                            CurrSlNo = soMiscDetailsObj.CID_SL_NO = slno;
                            int ItemId = 0;
                            int.TryParse(hdfItemID.Value, out ItemId);
                            soMiscDetailsObj.CID_ITEM = ItemId;
                            //soMiscDetailsObj.CID_ITEM =Convert.ToInt32( HttpUtility.HtmlEncode(hdfItemID.Value));
                            soMiscDetailsObj.CID_ITEM_TEXT = (txtItem.Text.Equals("Select/Type") || txtItem.Text.Equals("Type min. 3 characters")) ? string.Empty : HttpUtility.HtmlEncode(txtItem.Text);
                            soMiscDetailsObj.CID_INSTRUCTIONS = HttpUtility.HtmlEncode(txtDesc.Text);
                            soMiscDetailsObj.CID_QTY_INVOICED = Math.Round(Convert.ToDouble(txtQty.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits);
                            soMiscDetailsObj.CID_UOM = Convert.ToInt32(hdfUOM.Value);
                            soMiscDetailsObj.CID_UOM_TEXT = HttpUtility.HtmlEncode(txtUOM.Text);
                            //soMiscDetailsObj.CID_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                            //    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit])));

                            soMiscDetailsObj.CID_RATE = Math.Round(Convert.ToDouble(txtRate.Text), (Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit] == null
                                ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit])));

                            soMiscDetailsObj.CID_DISCOUNT = Math.Round(Convert.ToDouble(string.IsNullOrEmpty(txtDiscount.Text) ? CommonConstants.SELECT_VALUE_ZERO : txtDiscount.Text)
                                , Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            soMiscDetailsObj.CID_AMOUNT = Math.Round(Convert.ToDouble(txtAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            //soMiscDetailsObj.CID_AMOUNT = Math.Round(Convert.ToDouble(txtAmount.Text), (Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit] == null
                            //    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.MiscRateDecimalDigit])));

                            soMiscDetailsObj.CID_TAX = Math.Round(Convert.ToDouble(string.IsNullOrEmpty(txtTax.Text) ? CommonConstants.SELECT_VALUE_ZERO : txtTax.Text)
                                , Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            soMiscDetailsObj.CID_REMARKS = HttpUtility.HtmlEncode(txtDtlRemark.Text);

                            if (IssueListApplied != null && IssueListApplied.Count > 0)
                            {
                                IssueListApplied.ForEach(dtl => dtl.CII_SL_NO = soMiscDetailsObj.CID_SL_NO);
                                IssueListApplied.ForEach(dtl => dtl.CII_UOM = soMiscDetailsObj.CID_UOM);
                                IssueListApplied.ForEach(dtl => dtl.CII_INVOICE_DTL = soMiscDetailsObj.CID_PK);
                            }
                            soMiscDetailsObj.IssueDtl = IssueListApplied;
                            soMiscDetailsList.Add(soMiscDetailsObj);
                        }
                        retObject = soMiscDetailsList;
                        break;
                    #endregion
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        ////Start
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            ////
                            foreach (GridViewRow grdrow in grdMiscList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
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
                                GetFieldValues(ControlsEnum.SOINVHEADER);

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

                                Expense = ApplicationType.MSIJ;
                                ucrJournalize.TransactionType = Expense;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = Expense;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = invoiceHeaderObj.ICH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = invoiceHeaderObj.ICH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = invoiceHeaderObj.ICH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.AP;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = invoiceHeaderObj.ICH_CUSTOMER;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.MSIJ;
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
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Purchase_Expense_Journal").ToString();

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
                    #region Pick Invoice for Receipt
                    case ControlsEnum.PICKFORRECEIPT:
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedInvoice = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                approvedStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                isPosted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                selectedCustomer = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                selectedCurrency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                selectedInvoiceType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                balamt = Convert.ToDecimal(((LinkButton)grdrow.FindControl("lbnBalAmt")).Text.Replace(",",""));
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (approvedStatus == 2 && isPosted)
                            {
                                if ((balamt > 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                                {
                                    if (SelectedCurrency == 0)
                                        SelectedCurrency = selectedCurrency;
                                    if (SelectedCustomers == 0)
                                        SelectedCustomers = selectedCustomer;
                                    if (SelectedInvoiceType == 0)
                                        SelectedInvoiceType = selectedInvoiceType;
                                    if (SelectedCurrency != selectedCurrency)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency_Payment").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (SelectedCustomers != selectedCustomer)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor_Payment").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (SelectedInvoiceType != selectedInvoiceType)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Err_InvoiceTypes_receipt").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (!IsExixtPk(SelectedSalesInvoices, selectedInvoice))
                                    {
                                        //Add Invoices
                                        if (SelectedSalesInvoices == null)
                                            SelectedSalesInvoices = new List<long>();
                                        SelectedSalesInvoices.Add(selectedInvoice);

                                        btnPickForReceipt.Text = Resources.Controls.PickPoForInvoicing;
                                        btnPickForReceipt.Text = GetLocalResourceObject("PickInvforReceipt").ToString() + "(" + SelectedSalesInvoices.Count().ToString() + ")";

                                        //For Saving Selected Item PK
                                        hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + selectedInvoice.ToString();
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    Iscont = false;
                                }
                                else
                                {
                                    Iscont = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowAlreadyPaid();});", true);
                                    break;
                                }
                            }
                            else
                            {
                                if (approvedStatus != 2)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                                }
                                else if (Posted == false)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_PickPaymentPost_Msg").ToString();
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }

                            //For Setting/Resetting Colour of a selected InvoiceNo
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Pick Inv & for Cr/Dr. Note
                    case ControlsEnum.PICKFORCRDRNOTE:
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedInvoice = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                approvedStatus = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                isPosted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                selectedCustomer = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                selectedCurrency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                selectedInvoiceType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);                                
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (approvedStatus == 2 && isPosted)
                            {
                                if (SelectedInvoicesCrDr == null || SelectedInvoicesCrDr.Count == 0)
                                {
                                    //if (SelectedCurrency == 0)
                                        SelectedCurrency = selectedCurrency;
                                    //if (SelectedCustomers == 0)
                                        SelectedCustomers = selectedCustomer;
                                    //if (SelectedInvoiceType == 0)
                                        SelectedInvoiceType = selectedInvoiceType;
                                }
                                if (selectedInvoiceType == (int)SalesInvoiceType.Proforma)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Proforma_CNDN").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (SelectedCurrency != selectedCurrency)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency_CNDN").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (SelectedCustomers != selectedCustomer)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer_CNDN").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (SelectedInvoiceType != selectedInvoiceType)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_InvoiceTypes_CNDN").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (!IsExixtPk(SelectedInvoicesCrDr, selectedInvoice))
                                {
                                    //Add Invoices
                                    if (SelectedInvoicesCrDr == null)
                                        SelectedInvoicesCrDr = new List<long>();
                                    SelectedInvoicesCrDr.Add(selectedInvoice);
                                    btnPickForCrDrNote.Text = GetLocalResourceObject("PickInvforCNDN").ToString() + "(" + SelectedInvoicesCrDr.Count().ToString() + ")";
                                    Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.MSI;
                                    //For Saving Selected Item PK
                                    hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + selectedInvoice.ToString();
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                //Iscont = false;
                            }
                            else
                            {
                                if (approvedStatus != 2)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                                }
                                else if (!Posted)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_PickPaymentPost_Msg").ToString();
                                }
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }

                            //For Setting/Resetting Colour of a selected InvoiceNo
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region Due Date
                    case ControlsEnum.GETDUEDATE:
                        objDueDateDtls = new DueDateDetails();
                        objDueDateDtls.TCH_PK = string.IsNullOrEmpty(ddlPaymentTerms.SelectedValue) ? 0 : Convert.ToInt32(ddlPaymentTerms.SelectedValue);
                        objDueDateDtls.TRX_TYPE = ApplicationType.MSI;
                        objDueDateDtls.TRX_PK = CurrPK;
                        objDueDateDtls.AMOUNT = 0;
                        if (txtETA.Text != string.Empty)
                        {
                            objDueDateDtls.ETA = txtETA.Text;
                        }
                        if (txtETD.Text != string.Empty)
                        {
                            objDueDateDtls.ETD = txtETD.Text;
                        }
                        if (txtInvoiceDate.Text != string.Empty)
                        {
                            objDueDateDtls.ICH_DATE = txtInvoiceDate.Text;
                        }
                        retObject = objDueDateDtls;
                        break;
                    #endregion
                    #region ALERT
                    case ControlsEnum.ALERTSAVE:
                        string Typename = Resources.Constants.SystemAlertType;
                        int AlertPk = 0;
                        alertBoObj = new AlertBO();
                        appType = ApplicationType.MSI;
                        if (invPK > 0)
                        {
                            GetFieldValues(ControlsEnum.ALERTLIST);
                            if (dsAlertList != null && dsAlertList.Tables[0].Rows.Count == 1)
                            {
                                AlertPk = Convert.ToInt32(dsAlertList.Tables[0].Rows[0]["ATH_PK"].ToString());
                            }
                        }
                        alertBoObj.ATH_PK = AlertPk;
                        alertBoObj.ATH_NO = "";
                        alertBoObj.ATH_DATE = DateTime.Now;
                        alertBoObj.ATH_TRX_TYPE = appType;
                        alertBoObj.ATH_TRX_PK = invPK;
                        alertBoObj.ATH_TRX_DATE = txtInvoiceDate.Text.Trim() == string.Empty ? DateTime.Now : Convert.ToDateTime(txtInvoiceDate.Text.Trim());
                        string duedays = (Math.Floor((DateTime.Now - alertBoObj.ATH_TRX_DATE).TotalDays)).ToString();
                        alertBoObj.ATH_DUE_DAYS = Convert.ToInt16(duedays);
                        alertBoObj.ATH_DUE_DATE = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                        alertBoObj.ATH_NAME = Typename;
                        alertBoObj.ATH_ALERT_TYPE = null;
                        GetFieldValues(ControlsEnum.ALERTBASIS);
                        if (admConfigMstList != null && admConfigMstList.Count > 0)
                        {
                            alertBoObj.ATH_BASIS = admConfigMstList[0].CFG_PK;
                            alertBoObj.ATH_NOTIFY_BFR_UOM = admConfigMstList[0].CFG_PK;
                            GetFieldValues(ControlsEnum.NOTIFICATIONDAYS);
                            if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                            {
                                alertBoObj.ATH_NOTIFY_BFR = admAppConstMstList[0].ACF_VALUE;
                            }
                            else
                            {
                                alertBoObj.ATH_NOTIFY_BFR = 0;
                            }
                        }
                        else
                        {
                            alertBoObj.ATH_BASIS = null;
                            alertBoObj.ATH_NOTIFY_BFR_UOM = null;
                            alertBoObj.ATH_NOTIFY_BFR = 0;
                        }
                        alertBoObj.ATH_REMARKS = string.Empty;
                        //if (!string.IsNullOrEmpty(TypeRef) && !TypeRef.Equals("[New]".ToLower()))
                        //{
                        alertBoObj.ATH_NARRATION = lblInvoiceNo.Text + " - " + txtCustomer.Text;// + " - " + lb.ToolTip;
                        //if (!string.IsNullOrEmpty(TypePartyName))
                        //{
                        //    alertBoObj.ATH_NARRATION = !string.IsNullOrEmpty(alertBoObj.ATH_NARRATION) ? alertBoObj.ATH_NARRATION + " - " + TypePartyName :
                        //        TypePartyName;
                        //}
                        //}
                        alertBoObj.ATH_NOTIFY_MESSAGE = true;
                        alertBoObj.ATH_NOTIFY_USER = Convert.ToInt16(currentUser.PKUser);
                        alertBoObj.ATH_STATUS = (byte)0;
                        alertBoObj.ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        alertBoObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                        alertBoObj.BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        alertBoObj.LAST_MOD_DT = LastModifiedTime;
                        retObject = alertBoObj;
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
            try
            {
                switch (controlType)
                {
                    #region INVOICE GET
                    case ControlsEnum.INVOICEGET:                       
                        if (dtMISCLIST != null && dtMISCLIST.Rows.Count > 0)
                        {
                            CurrPK = Convert.ToInt32(Request.QueryString["PK"].ToString());
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = dtMISCLIST.Rows[0]["ICH_CUS_PK"].ToString();
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = dtMISCLIST.Rows[0]["ICH_CUSTOMER_TEXT"].ToString();

                            Approved = Convert.ToInt32(dtMISCLIST.Rows[0]["ICH_STATUS"].ToString());
                            Posted = Convert.ToBoolean(dtMISCLIST.Rows[0]["ICH_HAS_JRNL_ENTRY"].ToString());

                            hfCancelInv.Value = dtMISCLIST.Rows[0]["ICH_DEL_STATUS"].ToString();
                            if (Convert.ToInt16(dtMISCLIST.Rows[0]["ICH_DEL_STATUS"].ToString()) == 1)
                            {
                                btnSave.Visible = false;
                                btnEditforCancel.Visible = false;
                                btnEdit.Visible = false;
                            }
                            else
                            {
                                btnEditforCancel.Visible = true;
                            }
                            //For Resolving Bug ID:  2579
                            if (Convert.ToInt16(dtMISCLIST.Rows[0]["ICH_STATUS"].ToString()) == 0)
                            {
                                btnEditforCancel.Visible = false;
                            }
                            if (!string.IsNullOrEmpty(dtMISCLIST.Rows[0]["ICH_DEPT"].ToString()) && int.TryParse(dtMISCLIST.Rows[0]["ICH_DEPT"].ToString(), out dept))
                            {
                                Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                base.SetUserDept();
                            }
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
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                        }
                        
                        break;
                        #endregion
                    #region SO INV HEADER
                    case ControlsEnum.SOINVHEADER:
                        if (invoiceHeaderObj != null)
                        {
                            CurrPK = invoiceHeaderObj.ICH_PK;
                            Approved = invoiceHeaderObj.ICH_STATUS;
                            paymentTermPK = Convert.ToInt32(invoiceHeaderObj.ICH_PAYMENT_TERM);
                            CusPk = Convert.ToInt32(invoiceHeaderObj.ICH_CUSTOMER);
                            GetFieldValues(ControlsEnum.PAYMENTTERMS);
                            SetFieldValues(ControlsEnum.PAYMENTTERMS);

                            //CustomerTypes
                            custPK = Convert.ToInt32(invoiceHeaderObj.ICH_CUSTOMER);
                            hdfCurrCustomerPK.Value = custPK.ToString();
                            CustomerTypeSelectedPk = string.IsNullOrEmpty(invoiceHeaderObj.ICH_BRANCH) ? 0 : Convert.ToInt32(invoiceHeaderObj.ICH_BRANCH);
                            CustomerSavedBranchId = string.IsNullOrEmpty(invoiceHeaderObj.ICH_BRANCH_TEXT) ? "" : invoiceHeaderObj.ICH_BRANCH_TEXT;
                            CustomerSavedTaxId = string.IsNullOrEmpty(invoiceHeaderObj.ICH_TAX_ID) ? "" : invoiceHeaderObj.ICH_TAX_ID;
                            GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                            SetFieldValues(ControlsEnum.CUSTOMERTYPES);
                            SetBranchIDEnableDisable();
                            if (CurrPK > 0)
                            {
                                //In Edit Mode For Showing Saving Data
                                if (CustomerTypeSelectedPk > 0 && ddlCustomerType.Items.FindByValue(CustomerTypeSelectedPk.ToString()) != null)
                                {
                                    ddlCustomerType.SelectedValue = CustomerTypeSelectedPk.ToString();
                                    txtTypeID.Text = CustomerSavedBranchId.ToString();
                                    txtTaxID.Text = HttpUtility.HtmlDecode(CustomerSavedTaxId.ToString());
                                }
                                else
                                {
                                    txtTypeID.Text = "";
                                    txtTaxID.Text = "";
                                }
                            }

                            //End


                            hdfInvoicePK.Value = invoiceHeaderObj.ICH_PK.ToString();
                            hdfInvoiceNo.Value = invoiceHeaderObj.ICH_NO == string.Empty ? "" : invoiceHeaderObj.ICH_NO;
                            lblInvoiceNo.Text = invoiceHeaderObj.ICH_NO == string.Empty ? "[NEW]" : invoiceHeaderObj.ICH_NO;
                            txtInvoiceDate.Text = invoiceHeaderObj.ICH_DATE;

                            txtCustomer.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_CUSTOMER_TEXT);
                            hdfCustomerID.Value = invoiceHeaderObj.ICH_CUSTOMER;
                            txtExchangeRate.Text = hdfExchangeRate.Value = invoiceHeaderObj.ICH_EXCHG_RATE.ToString();
                            hdfCurrency.Value = invoiceHeaderObj.ICH_CURRENCY.ToString();
                            if (!string.IsNullOrEmpty(invoiceHeaderObj.ICH_TYPE))
                                ddlInvoiceType.SelectedValue = invoiceHeaderObj.ICH_TYPE;
                            if (invoiceHeaderObj.ICH_SALES_CATEGORY!=0)
                                ddlSaleCategory.SelectedIndex = Convert.ToInt32(ddlSaleCategory.Items.IndexOf(ddlSaleCategory.Items.FindByValue(invoiceHeaderObj.ICH_SALES_CATEGORY.ToString())));
                            if (!string.IsNullOrEmpty(invoiceHeaderObj.ICH_GST_TYPE))
                                ddlInvoiceGstType.SelectedValue = invoiceHeaderObj.ICH_GST_TYPE;
                            txtCurrency.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_CURRENCY_TEXT);

                            txtInvoiceDueDate.Text = invoiceHeaderObj.ICH_DATE_PAY_BY;

                            txtHdrDiscount.Text = invoiceHeaderObj.ICH_DISCOUNT_TC.ToString(hdfCurrencyFormat.Value);
                            txtShipping.Text = invoiceHeaderObj.ICH_SHIP_CHARGE.ToString(hdfCurrencyFormat.Value);
                            txtHdrTax.Text = invoiceHeaderObj.ICH_TAX_TC.ToString(hdfCurrencyFormat.Value);
                            txtPriceAdj.Text = invoiceHeaderObj.ICH_AMOUNT_ADJUST.ToString();
                            txtHdrTotal.Text = invoiceHeaderObj.ICH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                            txtRemarks.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_REMARKS);
                            txtShipTo.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_SHIP_TO_ADDRESS);
                            ddlCompany.SelectedValue = invoiceHeaderObj.ICH_COMPANY.ToString();

                            txtReference.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_REF_NO);
                            txtETD.Text = invoiceHeaderObj.ICH_ETD == "null" ? "" : invoiceHeaderObj.ICH_ETD;
                            txtETA.Text = invoiceHeaderObj.ICH_ETA == "null" ? "" : invoiceHeaderObj.ICH_ETA;

                            txtFeederVessel.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_FEEDER_VESSEL);
                            txtMotherVessel.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_MOTHER_VESSEL);
                            txtContainerNo.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_CONTAINER_NO);

                            txtShipmentTerm.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_SHIPMENT_TERM);

                            hdfFromPortID.Value = invoiceHeaderObj.ICH_FROM_PORT;
                            txtFromPort.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_FROM_PORT_TEXT);

                            //ddlFromPort.SelectedValue = invoiceHeaderObj.ICH_FROM_PORT;

                            txtFDest.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TO_PORT);
                            txtNetWt.Text = invoiceHeaderObj.ICH_NET_WT.ToString(hdfCurrencyFormat.Value);
                            txtGrossWt.Text = invoiceHeaderObj.ICH_GROSS_WT.ToString(hdfCurrencyFormat.Value);
                            txtTerm1.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TERM1);
                            txtTerm2.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_TERM2);
                            hdfUOMwt.Value = invoiceHeaderObj.ICH_WT_UOM.ToString();
                            txtUOMwt.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_WT_UOM_TEXT);
                            ModifiedDatePnl.Visible = true;
                            LastModifiedTime = invoiceHeaderObj.LAST_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.ErpRes.LastModifiedOn + LastModifiedTime.ToString(Resources.ErpRes.LastModifiedDatetimeFormat);
                            if (hdfCurrencyFormat.Value != null || hdfCurrencyFormat.Value != "")
                            {
                                lblUOMGrosswt.Text = HttpUtility.HtmlDecode(invoiceHeaderObj.ICH_WT_UOM_TEXT);
                            }
                            chkDummy.Checked = invoiceHeaderObj.ICH_INV_IS_DUMMY == 1 ? true : false;
                            divDummy.Visible = invoiceHeaderObj.CUS_IS_DUMMY == 1 ? true : false;

                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = invoiceHeaderObj.ICH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            }

                            if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Export) && IsInvoiceGSTEnable)
                            {
                                ddlSubType.Visible = true;
                                lblSubType.Visible = true;
                                if (!string.IsNullOrEmpty(invoiceHeaderObj.ICH_SUB_TYPE))
                                    ddlSubType.SelectedValue = invoiceHeaderObj.ICH_SUB_TYPE;
                            }
                            else
                            {
                                ddlSubType.ClearSelection();
                                ddlSubType.Visible = false;
                                lblSubType.Visible = false;
                            }

                        }
                        break; 
                    #endregion
                    #region Line Item Details
                    case ControlsEnum.SELECTEDITEM:
                        if (soMiscDetailsObj != null)
                        {
                            ExpensePK = soMiscDetailsObj.CID_PK;
                            hdfDetailPK.Value = soMiscDetailsObj.CID_PK.ToString();
                            CurrSlNo = soMiscDetailsObj.CID_SL_NO;
                            txtItem.Text = HttpUtility.HtmlDecode(soMiscDetailsObj.CID_ITEM_TEXT);
                            hdfItemID.Value = Convert.ToString(soMiscDetailsObj.CID_ITEM);
                            txtDesc.Text = HttpUtility.HtmlDecode(soMiscDetailsObj.CID_INSTRUCTIONS);
                            txtQty.Text = GetFormattedNumber(soMiscDetailsObj.CID_QTY_INVOICED);
                            hdfUOM.Value = soMiscDetailsObj.CID_UOM.ToString();
                            txtUOM.Text = HttpUtility.HtmlDecode(soMiscDetailsObj.CID_UOM_TEXT);
                            txtRate.Text = soMiscDetailsObj.CID_RATE.ToString(hdfMiscRateFormat.Value);
                            txtDiscount.Text = GetFormattedCurrency(soMiscDetailsObj.CID_DISCOUNT);
                            txtAmount.Text = GetFormattedCurrency(soMiscDetailsObj.CID_AMOUNT).Replace(",", ""); //GetFormattedMiscRate(soMiscDetailsObj.CID_AMOUNT).Replace(",", ""); 
                            txtTax.Text = GetFormattedCurrency(soMiscDetailsObj.CID_TAX).Replace(",", "");
                            txtDtlRemark.Text = HttpUtility.HtmlDecode(soMiscDetailsObj.CID_REMARKS);

                            IssueListApplied = soMiscDetailsObj.IssueDtl;
                            if (IssueListApplied != null && IssueListApplied.Count > 0)
                                SelectedIssueTypePk = IssueListApplied[0].CII_TYPE;
                            //GetFieldValues(ControlsEnum.ISSUETYPE);
                            //SetFieldValues(ControlsEnum.ISSUETYPE); 
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
                #region SALE CATEGORY
                case ControlsEnum.SALECATEGORY:
                    //Bind SA
                    ddlSaleCategory.Items.Clear();
                    if (dtSaleCat != null && dtSaleCat.Rows.Count > 0)
                    {
                        ddlSaleCategory.DataSource = dtSaleCat;
                        ddlSaleCategory.DataTextField = "CFG_DATA";
                        ddlSaleCategory.DataValueField = "CFG_VALUE";
                        ddlSaleCategory.DataBind();
                    }
                    ddlSaleCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region INVOICE TYPE
                case ControlsEnum.INVOICETYPE:
                    ddlInvoiceType.Items.Clear();
                    ddlSaleOrderType.Items.Clear();
                    if (dtInvoiceType != null && dtInvoiceType.Rows.Count > 0)
                    {
                        ddlInvoiceType.DataSource = dtInvoiceType;
                        ddlInvoiceType.DataTextField = "CFG_DATA";
                        ddlInvoiceType.DataValueField = "CFG_VALUE";
                        ddlInvoiceType.DataBind();

                        ddlSaleOrderType.DataSource = dtInvoiceType;
                        ddlSaleOrderType.DataTextField = "CFG_DATA";
                        ddlSaleOrderType.DataValueField = "CFG_VALUE";
                        ddlSaleOrderType.DataBind();
                    }
                    ddlInvoiceType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    ddlSaleOrderType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;

                #endregion
                #region TAX TYPES
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
                        if (IsCustomTaxEnabled || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Discount) || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Shipping))
                       // if (IsCustomTaxEnabled || Convert.ToInt16(hdfTaxCategory.Value) == ((int)TaxType.Discount))
                        ddlPopupTaxType.Items.Add(new ListItem(Resources.Report.Custom, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region SO TYPE
                case ControlsEnum.SOTYPE:
                    ddlSaleOrderType.Items.Clear();
                    if (dtSOData != null)
                    {
                        ddlSaleOrderType.DataSource = dtSOData;
                        ddlSaleOrderType.DataTextField = "CFG_DATA";
                        ddlSaleOrderType.DataValueField = "CFG_VALUE";
                        ddlSaleOrderType.DataBind();
                    }
                    ddlSaleOrderType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                    if (dtCompany != null && dtCompany.Rows.Count > 0)
                    {
                        ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    }

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
                #region PaymentTerms
                case ControlsEnum.PAYMENTTERMS:
                    ddlPaymentTerms.Items.Clear();
                    if (dtPaymentTerms != null)
                    {
                        ddlPaymentTerms.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPaymentTerms, "TCH_NAME");
                        ddlPaymentTerms.DataTextField = "TCH_NAME";
                        ddlPaymentTerms.DataValueField = "TCH_PK";
                        ddlPaymentTerms.DataBind();
                    }
                    ddlPaymentTerms.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (paymentTermPK > 0 && ddlPaymentTerms.Items.FindByValue(paymentTermPK.ToString()) != null)
                        ddlPaymentTerms.SelectedValue = paymentTermPK.ToString();
                    break;
                #endregion
                #region FROM PORT
                case ControlsEnum.FROMPORT:
                    //ddlFromPort.Items.Clear();
                    //if (dtPageData != null)
                    //{
                    //    ddlFromPort.DataSource = CommonFunctions.HtmlDecodeDataTable(dtPageData, "CON_NAME");
                    //    ddlFromPort.DataTextField = "CON_NAME";
                    //    ddlFromPort.DataValueField = "CON_PK";
                    //    ddlFromPort.DataBind();
                    //}
                    //ddlFromPort.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Customer Types
                case ControlsEnum.CUSTOMERTYPES:
                    ddlCustomerType.Items.Clear();
                    if (dsCustomerTypes != null & dsCustomerTypes.Tables[0].Rows.Count > 0)
                    {
                        ddlCustomerType.DataSource = CommonFunctions.HtmlDecodeDataTable(dsCustomerTypes.Tables[0], "CAD_NAME");
                        ddlCustomerType.DataTextField = "CAD_NAME";
                        ddlCustomerType.DataValueField = "CAD_PK";
                        ddlCustomerType.DataBind();
                        SetBranchIDEnableDisable();
                    }
                    if (CustomerTypeSelectedPk > 0 && ddlCustomerType.Items.FindByValue(CustomerTypeSelectedPk.ToString()) != null)
                    {
                        ddlCustomerType.SelectedValue = CustomerTypeSelectedPk.ToString();
                    }
                    break;
                #endregion
                #region ISSUE TYPE
                case ControlsEnum.ISSUETYPE:
                    ddlIssueType.Items.Clear();
                    if (dtIssueType != null)
                    {
                        ddlIssueType.DataSource = dtIssueType;
                        ddlIssueType.DataTextField = "CFG_DATA";
                        ddlIssueType.DataValueField = "CFG_VALUE";
                        ddlIssueType.DataBind();
                    }
                    ddlIssueType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    if (SelectedIssueTypePk > 0 && ddlIssueType.Items.FindByValue(SelectedIssueTypePk.ToString()) != null)
                    {
                        ddlIssueType.SelectedValue = SelectedIssueTypePk.ToString();
                    }
                    break;
                #endregion
                #region Invoice Type Master
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
                #region GST SUB TYPE
                case ControlsEnum.GSTSUBTYPE:
                    ddlSubType.Items.Clear();
                    if (dtGstSubType != null && dtGstSubType.Rows.Count > 0)
                    {
                        ddlSubType.DataSource = dtGstSubType;
                        ddlSubType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlSubType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlSubType.DataBind();
                    }
                    ddlSubType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                    case ControlsEnum.EXPENSEDETAIL:
                        if (invoiceHeaderObj != null)
                        {
                            soMiscDetailsList = new List<SOInvoiceDetails>();
                            soMiscDetailsList = invoiceHeaderObj.OrderDetail;
                            if (soMiscDetailsList != null)
                            {
                                grdItemDetails.DataSource = soMiscDetailsList;
                                grdItemDetails.DataBind();
                            }
                        }
                        break;
                    case ControlsEnum.TAXPOPUPGRID:
                        if (IsHeaderTax)
                        {
                            string[] CatPKS = GetLocalResourceObject("OtherChargeCategoryValues").ToString().Split(',');// 2-> Shipment, 5->Duties, 6-> Surchages;
                            if (CatPKS.Contains(hdfTaxCategory.Value))
                            {
                                taxHdrList = TempSOInvoiceHeaderSession.TaxHdr == null ? new List<SOInvoiceTaxHdr>() :
                                  TempSOInvoiceHeaderSession.TaxHdr.Where(tax => CatPKS.Contains(tax.CIT_TAX_CATEGORY.ToString())).ToList();
                            }
                            else
                            {
                                taxHdrList = TempSOInvoiceHeaderSession.TaxHdr == null ? new List<SOInvoiceTaxHdr>() :
                                    TempSOInvoiceHeaderSession.TaxHdr.Where(tax => Convert.ToInt32(tax.CIT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                        }
                        else
                        {
                            soDtlObj = TempSOInvoiceHeaderSession.OrderDetail == null ? null :
                                TempSOInvoiceHeaderSession.OrderDetail.SingleOrDefault(dtl => dtl.CID_PK == ExpensePK && dtl.CID_SL_NO == CurrSlNo);
                            if (soDtlObj != null)
                            {
                                taxHdrList = soDtlObj.TaxDtl == null ? new List<SOInvoiceTaxHdr>() :
                                    soDtlObj.TaxDtl.Where(tax => Convert.ToInt32(tax.CIT_TAX_CATEGORY) == Convert.ToInt32(hdfTaxCategory.Value)).ToList();
                            }
                            else
                                taxHdrList = new List<SOInvoiceTaxHdr>();
                        }
                        grdTaxDetails.DataSource = taxHdrList;
                        grdTaxDetails.DataBind();
                        break;
                    case ControlsEnum.MISCLIST:
                        if (dtMISCLIST != null)
                        {
                            PageIndex = PageIndex == null ? "0" : PageIndex;
                            grdMiscList.PageIndex = Convert.ToInt32(PageIndex);
                            grdMiscList.DataSource = dtMISCLIST.DefaultView;
                            grdMiscList.DataBind();

                            //SetResetColour
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        }
                        break;

                    case ControlsEnum.SHOWDIVTAXBC:
                        taxHdrList = SOInvoiceHeaderSession.TaxHdr == null ? new List<SOInvoiceTaxHdr>() :
                                SOInvoiceHeaderSession.TaxHdr.ToList();
                        grdTaxBaseCUr.DataSource = taxHdrList.Where((rfq => rfq.CIT_TAX_CATEGORY == ((int)TaxType.Tax) && rfq.CIT_TAX_AMT > 0));
                        grdTaxBaseCUr.DataBind();
                        break;
                    #region ISSUEDETAILSSPLITUP
                    case ControlsEnum.ISSUEDETAILSSPLITUP:
                        TotalIssueQty = 0;
                        TotalIssueInvdQty = 0;
                        TotalIssueBalance = 0;
                        if (issueListForGrid != null)
                        {
                            TotalIssueQty = issueListForGrid.Sum(grn => grn.CII_QTY_ISSUED);
                            TotalIssueInvdQty = issueListForGrid.Sum(bal => bal.CII_QTY_PRV_INVOICED);
                            TotalIssueBalance = issueListForGrid.Sum(bal => bal.CII_QTY_BALANCE);//TotalIssueQty - TotalIssueInvdQty;
                            grdIssueDetails.DataSource = issueListForGrid;
                            grdIssueDetails.DataBind();
                        }
                        else
                        {
                            grdIssueDetails.DataSource = null;
                            grdIssueDetails.DataBind();
                        }
                        break;
                    case ControlsEnum.ISSUEDETAILSSPLITUPAPPLIED:
                        TotalIssueQty = 0;
                        TotalIssueInvdQty = 0;
                        TotalIssueBalance = 0;
                        if (IssueListApplied != null)
                        {
                            TotalIssueQty = IssueListApplied.Sum(grn => grn.CII_QTY_ISSUED);
                            TotalIssueInvdQty = IssueListApplied.Sum(bal => bal.CII_QTY_PRV_INVOICED);
                            TotalIssueBalance = IssueListApplied.Sum(bal => bal.CII_QTY_BALANCE);
                            grdIssueDetails.DataSource = IssueListApplied;
                            grdIssueDetails.DataBind();
                        }
                        else
                        {
                            grdIssueDetails.DataSource = null;
                            grdIssueDetails.DataBind();
                        }
                        break;
                    #endregion

                    

                    case ControlsEnum.AMOUNTDETAILS:
                        if (dtAmountDetails != null && dtAmountDetails.Rows.Count > 0)
                            grdPaidAmntSplitup.DataSource = dtAmountDetails;
                        else
                            grdPaidAmntSplitup.DataSource = null;
                        grdPaidAmntSplitup.DataBind();                      
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
                #region TAX POPUP GRID
                case ControlsEnum.TAXPOPUPGRID:
                    TaxPK = 0;
                    SelectedDtlPK = 0;
                    hdfTaxCategory.Value = string.Empty;
                    break; 
                #endregion
                #region SO INV HEADER
                case ControlsEnum.SOINVHEADER:
                    CurrPK = 0;
                    break; 
                #endregion
                #region MISC LIST
                case ControlsEnum.MISCLIST:
                    CurrPK = 0;
                    Posted = false;
                    txtInvoiceNumber.Text = "Select/Type";
                    txtCustomer.Text = "Select/Type";
                    hdfCustomerID.Value = "";
                    txtCustomerLIST.Text = "Select/Type";
                    hdfCustomerLIST.Value = "";
                    ddlSaleOrderType.SelectedIndex = 0;
                    ddlSaleCategory.SelectedIndex = 0;
                    txtSCno.Text = string.Empty;
                    hdfIVHPK.Value = "";
                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    ddlStatus.ClearSelection();
                    ddlCompanySrch.SelectedValue = CommonConstants.SELECTVAL;

                    lblInvoiceNo.Text = string.Empty;
                    hdfInvoiceNo.Value = string.Empty;
                    txtInvoiceDate.Text = string.Empty;
                    txtCustomer.Text = string.Empty;
                    hdfCustomerID.Value = string.Empty;
                    txtShipTo.Text = string.Empty;
                    txtCurrency.Text = string.Empty;
                    hdfCurrency.Value = string.Empty;

                    txtETA.Text = string.Empty;
                    txtETD.Text = string.Empty;
                    txtReference.Text = string.Empty;
                    txtFeederVessel.Text = string.Empty;
                    txtMotherVessel.Text = string.Empty;
                    txtContainerNo.Text = string.Empty;
                    ddlInvoiceType.ClearSelection();
                    ddlInvoiceGstType.ClearSelection();
                    txtShipmentTerm.Text = string.Empty;
                    txtExchangeRate.Text = string.Empty;

                    //ddlFromPort.ClearSelection();
                    txtFromPort.Text = string.Empty;
                    hdfFromPortID.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtFDest.Text = string.Empty;
                    txtNetWt.Text = string.Empty;
                    txtGrossWt.Text = string.Empty;
                    txtTerm1.Text = string.Empty;
                    txtTerm2.Text = string.Empty;
                    txtUOMwt.Text = string.Empty;
                    hdfUOMwt.Value = string.Empty;


                    txtInvoiceDueDate.Text = string.Empty;
                    txtRemarks.Text = string.Empty;
                    txtHdrDiscount.Text = string.Empty;
                    txtShipping.Text = string.Empty;
                    txtHdrTax.Text = string.Empty;
                    txtPriceAdj.Text = string.Empty;
                    txtHdrTotal.Text = string.Empty;

                    grdItemDetails.DataSource = null;
                    grdItemDetails.DataBind();

                    ModifiedDatePnl.Visible = false;
                    //base.WkfRefID = 0;

                    ddlCustomerType.Items.Clear();
                    txtTypeID.Text = "";
                    txtTaxID.Text = "";
                    chkBalAmt.Checked = true;
                    chkDummy.Checked = false;
                    divDummy.Visible = false;
                    ddlSubType.ClearSelection();
                    ResetForm(ControlsEnum.EXPENSEDETAIL);
                    break; 
                #endregion
                #region EXPENSE DETAIL
                case ControlsEnum.EXPENSEDETAIL:
                    ExpensePK = 0;
                    TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                    CurrSlNo = 0;
                    hdfDetailPK.Value = CommonConstants.SELECT_VALUE_ZERO;
                    txtItem.Text = "Type min. 3 characters";
                    hdfItemID.Value = "";
                    txtDesc.Text = string.Empty;
                    txtQty.Text = CommonConstants.SELECT_VALUE_ONE;
                    //hdfUOM.Value = string.Empty;
                    //txtUOM.Text = string.Empty;
                    BindDefaultUOM();
                    txtRate.Text = string.Empty;
                    txtDiscount.Text = string.Empty;
                    txtAmount.Text = string.Empty;
                    txtTax.Text = string.Empty;
                    txtDtlRemark.Text = string.Empty;
                    break; 
                #endregion
                #region INVOICE GRID
                case ControlsEnum.INVOICEGRID:
                    if (grdMiscList.Rows.Count > 0)
                    {
                        foreach (GridViewRow grvRow in grdMiscList.Rows)
                        {
                            RadioButton rbtSelect = (RadioButton)grvRow.FindControl("rbtSelect");
                            if (rbtSelect.Checked)
                                rbtSelect.Checked = false;
                        }
                    }
                    break; 
                #endregion
                #region ISSUE DETAILS SPLITUP
                case ControlsEnum.ISSUEDETAILSSPLITUP:
                    issueListForGrid = null;
                    IssueListApplied = null;
                    SelectedIssueTypePk = 0;
                    grdIssueDetails.DataSource = null;
                    grdIssueDetails.DataBind();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_EnableUOMAutocomplete", "EnableUOMAutocomplete();", true);
                    break; 
                #endregion
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
        /// <summary>
        /// I exist Po in list
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsExixtPk(List<long> lst, long pk)
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

        private void SetDetailTax(SOInvoiceHeader expenseHdr)
        {
            double quantity;
            double rate;
            quantity = 0;
            rate = 0;

            if (double.TryParse(txtRate.Text, out quantity) && double.TryParse(txtQty.Text, out rate))
            {
                if (txtAmount != null)
                {
                    txtAmount.Text = GetFormattedCurrency(rate * quantity).Replace(",", ""); //GetFormattedMiscRate(rate * quantity).Replace(",", ""); 
                    SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                    ExpensePK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                    SetItemTax(expenseHdr);
                }
            }
            else
            {
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Rate_Greater_Discount").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
            }
        }
        private bool SetItemTax(SOInvoiceHeader expenseHdr)
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
                    invoiceHeaderObj = expenseHdr;
                    soMiscDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(crt => crt.CID_PK == ExpensePK
                        && crt.CID_SL_NO == CurrSlNo);
                    if (soMiscDetailsObj != null)
                    {
                        if (soMiscDetailsObj.TaxDtl != null)
                        {
                            var discDetail = soMiscDetailsObj.TaxDtl.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Discount));
                            foreach (SOInvoiceTaxHdr taxHdrObj in discDetail)
                            {
                                string taxFormula = taxHdrObj.CIT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                                    taxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            discount = soMiscDetailsObj.TaxDtl.Where(ctr => ctr.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(ctr => ctr.CIT_TAX_AMT);
                        }
                        netAmount = amount - discount;
                        txtDiscount.Text = discount.ToString(hdfCurrencyFormat.Value);
                        if (soMiscDetailsObj.TaxDtl != null)
                        {
                            var taxDetail = soMiscDetailsObj.TaxDtl.Where(ctr => ctr.CIT_TAX_CATEGORY == ((int)TaxType.Tax));
                            foreach (SOInvoiceTaxHdr taxHdrObj in taxDetail)
                            {
                                string taxFormula = taxHdrObj.CIT_TAX_FORMULA;
                                if (!string.IsNullOrEmpty(taxFormula))
                                {
                                    taxFormula = taxFormula.Replace("#SUBTOTAL#", netAmount.ToString());
                                    taxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                }
                            }
                            itmTax = soMiscDetailsObj.TaxDtl.ToList().Where(ctr => ctr.CIT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(ctr => ctr.CIT_TAX_AMT);
                        }
                        txtTax.Text = itmTax.ToString(hdfCurrencyFormat.Value);
                        soMiscDetailsObj.CID_AMOUNT = amount;
                        soMiscDetailsObj.CID_DISCOUNT = discount;
                        soMiscDetailsObj.CID_TAX = itmTax;
                        soMiscDetailsObj.CID_NET_AMOUNT = (amount - discount + itmTax);
                        txtTotal.Text = soMiscDetailsObj.CID_NET_AMOUNT.ToString(hdfCurrencyFormat.Value);
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
            if (SOInvoiceHeaderSession.OrderDetail != null)
            {
                SOInvoiceHeaderSession.ICH_AMOUNT_TC = SOInvoiceHeaderSession.OrderDetail.Sum(dtl => dtl.CID_NET_AMOUNT);
            }
            if (grdItemDetails.FooterRow != null)
            {
                lblSubTotalFooter = grdItemDetails.FooterRow.FindControl("lblSubTotalFooter") as Label;
                if (lblSubTotalFooter != null)
                {
                    lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = SOInvoiceHeaderSession.ICH_AMOUNT_TC.ToString(hdfCurrencyFormat.Value);//hdfMiscRateFormat
                }
            }
        }
        private bool SetHdrTax()
        {
            double amount;
            double discount;
            double adjust;
            double shipping;
            amount = 0;
            adjust = 0;

            if (SOInvoiceHeaderSession != null)
            {
                invoiceHeaderObj = SOInvoiceHeaderSession;
                amount = Convert.ToDouble(invoiceHeaderObj.ICH_AMOUNT_TC);
                discount = 0;
                shipping = 0;
                if (invoiceHeaderObj.TaxHdr != null)
                {
                    var discHeader = invoiceHeaderObj.TaxHdr.Where(hdr => hdr.CIT_TAX_CATEGORY == ((int)TaxType.Discount));
                    foreach (SOInvoiceTaxHdr taxHdrObj in discHeader)
                    {
                        string taxFormula = taxHdrObj.CIT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    discount = invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(quotation => quotation.CIT_TAX_AMT);
                }
                invoiceHeaderObj.ICH_DISCOUNT_TC = discount;
                txtHdrDiscount.Text = txtHdrDiscount.ToolTip = discount.ToString(hdfCurrencyFormat.Value);             
                amount = amount - discount;

                if (invoiceHeaderObj.TaxHdr != null)
                {
                    string[] CatPKS = GetLocalResourceObject("OtherChargeCategoryValues").ToString().Split(',');// 2-> Shipment, 5->Duties, 6-> Surchages;
                    var OtherHeader = invoiceHeaderObj.TaxHdr.Where(hdr =>CatPKS.Contains( hdr.CIT_TAX_CATEGORY.ToString()));
                    foreach (SOInvoiceTaxHdr taxHdrObj in OtherHeader)
                    {
                        string taxFormula = taxHdrObj.CIT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula) && taxFormula!="0")
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            taxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                        }
                    }
                    shipping = invoiceHeaderObj.TaxHdr.Where(quotation =>CatPKS.Contains(quotation.CIT_TAX_CATEGORY.ToString())).Sum(quotation => quotation.CIT_TAX_AMT);
                }
                invoiceHeaderObj.ICH_SHIP_CHARGE = shipping;
                txtShipping.Text = txtShipping.ToolTip = shipping.ToString(hdfCurrencyFormat.Value);
                //  amount = amount - shipping;
                if (IsTaxForOtherCharge)
                {
                    amount += string.IsNullOrEmpty(txtShipping.Text) ? 0.00 : Convert.ToDouble(txtShipping.Text);
                }
                if (invoiceHeaderObj.TaxHdr != null)
                {
                    var taxHeader = invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Tax));
                    foreach (SOInvoiceTaxHdr taxHdrObj in taxHeader)
                    {
                        string taxFormula = taxHdrObj.CIT_TAX_FORMULA;
                        if (!string.IsNullOrEmpty(taxFormula))
                        {
                            taxFormula = taxFormula.Replace("#SUBTOTAL#", amount.ToString());
                            //taxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                            taxHdrObj.CIT_TAX_AMT = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1);
                        }
                    }
                    invoiceHeaderObj.ICH_TAX_TC = invoiceHeaderObj.TaxHdr.Where(quotation => quotation.CIT_TAX_CATEGORY == ((int)TaxType.Tax)).Sum(quotation => quotation.CIT_TAX_AMT);
                }
                txtHdrTax.Text = txtHdrTax.ToolTip = invoiceHeaderObj.ICH_TAX_TC.ToString(hdfCurrencyFormat.Value);
                double.TryParse(txtPriceAdj.Text, out adjust);
                invoiceHeaderObj.ICH_AMOUNT_ADJUST = adjust;
                invoiceHeaderObj.ICH_AMOUNT_NET_TC = Convert.ToDouble(invoiceHeaderObj.ICH_AMOUNT_TC) - invoiceHeaderObj.ICH_DISCOUNT_TC + invoiceHeaderObj.ICH_TAX_TC + +invoiceHeaderObj.ICH_SHIP_CHARGE
                    + invoiceHeaderObj.ICH_AMOUNT_ADJUST;

                txtHdrTotal.Text = txtHdrTotal.ToolTip = invoiceHeaderObj.ICH_AMOUNT_NET_TC.ToString(hdfCurrencyFormat.Value);
                SOInvoiceHeaderSession = invoiceHeaderObj;
                TempSOInvoiceHeaderSession = invoiceHeaderObj;

                //#region Other Charge

              
                //#endregion

             

                if (string.IsNullOrEmpty(txtNetWt.Text))
                    txtNetWt.Text = 0.ToString(hdfCurrencyFormat.Value);

                if (string.IsNullOrEmpty(txtGrossWt.Text))
                    txtGrossWt.Text = 0.ToString(hdfCurrencyFormat.Value);
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

        public string GetFormattedCurrencyWithComa(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperator.Value);
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
        public string GetFormattedRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfRateFormat.Value);
        }
        public string GetFormattedMiscRate(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfMiscRateFormat.Value);
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
        /// <summary>
        ///CheckDuplicateScrapIssueAllocation
        /// </summary>
        /// <param name="soMiscDetailsList">SOInvoiceDetails List</param>

        private bool IsHaveDuplicateScrapIssueAllocation(List<SOInvoiceDetails> soMiscDetailsList)
        {
            bool Isduplicate = false;
            if (soMiscDetailsList != null && soMiscDetailsList.Count > 0)
            {
                List<SOInvoiceDetails> orderDetailTempList = soMiscDetailsList.Where(f => f.CID_SL_NO != CurrSlNo).ToList();

                if (IssueListApplied != null && IssueListApplied.Count > 0)
                {
                    foreach (var orderDetailItem in orderDetailTempList)
                    {
                        List<IssueDetail> tempIssueDetail = new List<IssueDetail>();
                        tempIssueDetail = orderDetailItem.IssueDtl;
                        if (tempIssueDetail != null && tempIssueDetail.Count > 0)
                        {
                            foreach (var appliedItem in IssueListApplied)
                            {
                                foreach (var tempItem in tempIssueDetail)
                                {
                                    if (tempItem.CII_ISSUE_PK == appliedItem.CII_ISSUE_PK && tempItem.CII_QTY_INVOICED != 0 && appliedItem.CII_QTY_INVOICED != 0)
                                    {
                                        Isduplicate = true;
                                        return Isduplicate;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Isduplicate;
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
                    if (pid == 1 || pid == 3)
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

        #region EnableDisableInvoiceType
        /// <summary>
        /// Enable or disable dropdown list for Type (Domestic/Export/Profoma)
        /// </summary>
        private void EnableDisableInvoiceType()
        {
            ddlInvoiceType.Enabled = true;
            if (invoiceHeaderObj != null)
            {
                if (invoiceHeaderObj.ICH_STATUS == (int)WkfStatusEnum.DRAFTED)
                {
                    ddlInvoiceType.Enabled = true;
                }
                else
                {
                    ddlInvoiceType.Enabled = false;
                }
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
            //Session Logout on Department change
            if (!(this.Master as ERPSMS_2).ValidatePageDept())
                return;

            CommonService CommonServiceClient;
            CommonServiceClient = null;
            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            try
            {
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                int? result;
                int alertresult;
                //SOInvoiceTaxHdr tempInvTaxSplitObj = null;
                tempInvTaxSplitObj = null;
                bool bIsChecked = false;

                DropDownList ddlWkfAction;
                TextBox WrkfComments;
                Label lblSubTotal;
                string action;

                double totalAmt;
                double currentTotal;
                double taxAmt;
                bool isValidDisc = true;
                int selectedItemPK;
                

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
                    else if (((DropDownList)sender).ID == "ddlPaymentTerms")
                    {
                        commonActions = ActionsEnum.GETDUEDATE;
                    }
                    //CustomerTypes
                    if (((DropDownList)sender).ID == "ddlCustomerType")
                    {
                        commonActions = ActionsEnum.CUSTOMERTYPECHANGING;
                    }
                    if (((DropDownList)sender).ID == "ddlIssueType")
                    {
                        commonActions = ActionsEnum.SHOWISSUEDETAILS;
                    }
                    if (((DropDownList)sender).ID == "ddlInvoiceType")
                    {
                        commonActions = ActionsEnum.GETINVGSTTYPE;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtInvNow")
                    {
                        commonActions = ActionsEnum.CALCULATEDTLTAX;
                    }
                    //ExchangeRate text changing event
                    else if (((TextBox)sender).ID == "txtExchangeRate")
                    {
                        commonActions = ActionsEnum.CHANGEEXRATE;
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
                    #region Grid Item Selected
                    case ActionsEnum.ITEMSELECTED:
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            RadioButton rbtn;
                            HiddenField hdfDept;
                            HiddenField hdfDelStatus;
                            HiddenField hdfStatus;
                            int selectedInvPK;
                            int dept;

                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                selectedInvPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                                hdfStatus = grdrow.FindControl("hdfStatus") as HiddenField;
                                hdfSelRecordStatus.Value = hdfStatus.Value.ToString();
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                base.WkfRefID = workflowCore.GetRefID(selectedInvPK, PageProcessID);
                                hfCancelInv.Value = hdfDelStatus.Value;
                                if (Convert.ToInt32(hdfDelStatus.Value) == 1)
                                {
                                    btnSave.Visible = false;
                                    btnEditforCancel.Visible = false;
                                    btnEdit.Visible = false;
                                }
                                else
                                {
                                    //btnSave.Visible = true;
                                    btnEditforCancel.Visible = true;
                                    // btnEdit.Visible = true;
                                }
                                //For Resolving Bug ID:  2579
                                if (Convert.ToInt32(hdfStatus.Value) == 0)
                                {
                                    btnEditforCancel.Visible = false;
                                }
                                break;
                            }
                        }
                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
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
                            SOInvoiceHeaderSession = TempSOInvoiceHeaderSession;
                            if (Convert.ToDouble(txtRate.Text) == 0 && hdfIsRatecont.Value == "0")
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ConfirmRate", "$(document).ready(function(){ShowRateConfirm();});", true);
                            }
                            else
                            {
                                soMiscDetailsList = SOInvoiceHeaderSession.OrderDetail;
                                if (!IsHaveDuplicateScrapIssueAllocation(soMiscDetailsList))
                                {
                                    soMiscDetailsList = (List<SOInvoiceDetails>)SetUIValuesToObject(ControlsEnum.EXPENSEDETAIL);
                                    if (soMiscDetailsList != null && soMiscDetailsList.Count > 0)
                                    {
                                        SOInvoiceHeaderSession.OrderDetail = soMiscDetailsList;
                                        SetDetailTax(SOInvoiceHeaderSession);
                                        SetSubTotal();
                                        SetHdrTax();
                                        invoiceHeaderObj = SOInvoiceHeaderSession;
                                        SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                                        if (hdfIsTaxPayable.Value == "1")
                                        {
                                            SetFieldValues(ControlsEnum.SHOWDIVTAXBC);
                                        }
                                        ResetForm(ControlsEnum.EXPENSEDETAIL);
                                        ResetForm(ControlsEnum.ISSUEDETAILSSPLITUP);
                                        hdfIsRatecont.Value = "0";
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_DuplicateScrapIssue").ToString();//The selected Issue no already applied.
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Remove Item
                    case ActionsEnum.REMOVEITEM:
                        if (SOInvoiceHeaderSession.OrderDetail != null && SOInvoiceHeaderSession.OrderDetail.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                SOInvoiceHeaderSession.OrderDetail = SOInvoiceHeaderSession.OrderDetail.Where(row => CurrSlNo != row.CID_SL_NO).ToList();
                                SetDetailTax(SOInvoiceHeaderSession);
                                SetSubTotal();
                                SetHdrTax();
                                invoiceHeaderObj = SOInvoiceHeaderSession;
                                SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                                if (hdfIsTaxPayable.Value == "1")
                                {
                                    SetFieldValues(ControlsEnum.SHOWDIVTAXBC);
                                }
                            }
                        }
                        ResetForm(ControlsEnum.EXPENSEDETAIL);
                        break;
                    #endregion
                    #region Edit Item
                    case ActionsEnum.EDITITEM:
                        ResetForm(ControlsEnum.EXPENSEDETAIL);
                        if (SOInvoiceHeaderSession.OrderDetail != null && SOInvoiceHeaderSession.OrderDetail.Count > 0)
                        {
                            CurrSlNo = Convert.ToInt32(grdItemDetails.DataKeys[(((ImageButton)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (CurrSlNo > 0)
                            {
                                soMiscDetailsObj = SOInvoiceHeaderSession.OrderDetail.SingleOrDefault(row => CurrSlNo == row.CID_SL_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDITEM);
                                SetFieldValues(ControlsEnum.ISSUEDETAILSSPLITUPAPPLIED);
                            }
                        }
                        hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                        break;
                    #endregion
                    #region Clear Item
                    case ActionsEnum.CLEARITEM:
                        ResetForm(ControlsEnum.EXPENSEDETAIL);
                        ResetForm(ControlsEnum.ISSUEDETAILSSPLITUP);
                        break;
                    #endregion
                    #region save
                    case ActionsEnum.SAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                        }
                        else if (SOInvoiceHeaderSession.OrderDetail == null || SOInvoiceHeaderSession.OrderDetail.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else if (Posted)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_CannotEditJournalized").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else
                        {
                            hasValidRate = false;


                            if (ddlCustomerType.Items.Count <= 0)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                                return;
                            }



                            //invoiceHeaderObj = new SOInvoiceHeader();
                            //invoiceHeaderObj = (SOInvoiceHeader)SetUIValuesToObject(ControlsEnum.SOINVHEADER);

                            invoiceHeaderObj = new SOInvoiceHeader();
                            invoiceHeaderMulObj = (SOInvoiceHeaderMul)SetUIValuesToObject(ControlsEnum.SOINVHEADER);
                            invoiceHeaderObj.WKF_FLAG = 0;

                            if (invoiceHeaderObj != null && invoiceHeaderObj.OrderDetail != null)
                            {
                                string xmlDoc = CommonFunctions.XmlSerialize<SOInvoiceHeaderMul>(invoiceHeaderMulObj); //string xmlDoc = CommonFunctions.XmlSerialize<SOInvoiceHeader>(invoiceHeaderObj);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
                                // save Process Control inspection details
                                result = BusinessLogic.Sales.SalesInvoiceBL.SaveSalesInvoiceHeader(xmlDoc);
                                if (result > 0) // Success !  redirect to listing page
                                {
                                    #region Update dummy entry while modify invoice after approval
                                    if (Approved == (int)DbStatus.APPROVED) // Checking invoice stataus wheteher invoice approved or not
                                    {
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        string refType = string.Empty;
                                        refType = ApplicationType.MSIJ;
                                        long DummyResult = invoiceHeaderObj.ICH_PK;
                                        bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)invoiceHeaderObj.ICH_PK, 0);
                                        if (IsDummyEntry == true)
                                        {
                                            DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)invoiceHeaderObj.ICH_PK, 0);
                                        }
                                        if (DummyResult > 0)
                                        {
                                            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                            DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                            finTrxServiceClient = null;
                                        }
                                    }
                                    #endregion
                                    // Show Save Message and redired to listing page                                        
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Save_Success;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString());
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                    //    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.Invoicing) + "');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.MISCLIST);
                                    GetFieldValues(ControlsEnum.MISCLIST);
                                    SetFieldValues(ControlsEnum.MISCLIST);
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
                                        litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                    {
                                        litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else if (result == (int)DbSaveStatus.REFNOEXIST)
                                    {
                                        litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + GetLocalResourceObject("RefNoExist").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString());
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                            + "','" + Resources.ErpRes.Information + "');", true);
                                    }
                                }
                            }

                            hasValidRate = false;
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
                            soMiscDetailsList = TempSOInvoiceHeaderSession.OrderDetail;

                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);

                            soMiscDetailsObj = TempSOInvoiceHeaderSession.OrderDetail == null ? null :
                                    TempSOInvoiceHeaderSession.OrderDetail.SingleOrDefault(ctr => ctr.CID_PK == SelectedDtlPK && ctr.CID_SL_NO == CurrSlNo);
                            if (soMiscDetailsObj == null)
                            {
                                soMiscDetailsList = (List<SOInvoiceDetails>)SetUIValuesToObject(ControlsEnum.EXPENSEDETAIL);
                                TempSOInvoiceHeaderSession.OrderDetail = soMiscDetailsList;
                            }

                            if (soMiscDetailsList != null && soMiscDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Tax).ToString();
                                hdfTaxFormula.Value = string.Empty;
                                hdfTaxCode.Value = string.Empty;
                                hdfTaxRate.Value = string.Empty;

                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) :
                                    string.IsNullOrEmpty(txtDiscount.Text.Trim()) ? Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value) :
                                    (Convert.ToDouble(txtAmount.Text.Trim()) - Convert.ToDouble(txtDiscount.Text.Trim())).ToString(hdfCurrencyFormat.Value);

                                if (TempSOInvoiceHeaderSession != null)
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
                                                hdfTaxCode.Value = dtTaxDetails.Rows[0]["TAX_CODE"].ToString();
                                                hdfTaxRate.Value = dtTaxDetails.Rows[0]["TAX_RATE"].ToString();
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula)).Replace(",", "");
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
                            soMiscDetailsList = TempSOInvoiceHeaderSession.OrderDetail;
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);

                            soMiscDetailsObj = TempSOInvoiceHeaderSession.OrderDetail == null ? null :
                                    TempSOInvoiceHeaderSession.OrderDetail.SingleOrDefault(ctr => ctr.CID_PK == SelectedDtlPK && ctr.CID_SL_NO == CurrSlNo);
                            if (soMiscDetailsObj == null)
                            {
                                soMiscDetailsList = (List<SOInvoiceDetails>)SetUIValuesToObject(ControlsEnum.EXPENSEDETAIL);
                                TempSOInvoiceHeaderSession.OrderDetail = soMiscDetailsList;
                            }
                            if (soMiscDetailsList != null && soMiscDetailsList.Count > 0)
                            {
                                hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                                hdfTaxFormula.Value = string.Empty;
                                hdfTaxCode.Value = string.Empty;
                                hdfTaxRate.Value = string.Empty;

                                txtPopupItemAmount.Text = string.IsNullOrEmpty(txtAmount.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(txtAmount.Text).ToString(hdfCurrencyFormat.Value);
                                if (TempSOInvoiceHeaderSession != null)
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
                                                hdfTaxCode.Value = dtTaxDetails.Rows[0]["TAX_CODE"].ToString();
                                                hdfTaxRate.Value = dtTaxDetails.Rows[0]["TAX_RATE"].ToString();
                                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula)).Replace(",", "");
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
                        hdfTaxCode.Value = string.Empty;
                        hdfTaxRate.Value = string.Empty;
                        if (SOInvoiceHeaderSession != null)
                        {
                            //ResetForm(ControlsEnum.EXPENSEDETAIL);                           
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = grdItemDetails.FooterRow == null ? null : (grdItemDetails.FooterRow.FindControl("lblSubTotalFooter") as Label);
                            //if (lblSubTotal != null)
                            //{
                            double taxable = Convert.ToDouble(SOInvoiceHeaderSession.ICH_AMOUNT_TC) - SOInvoiceHeaderSession.ICH_DISCOUNT_TC;

                            //Add Other Charges Based on configuration
                            if (IsTaxForOtherCharge)
                            {
                                taxable += string.IsNullOrEmpty(txtShipping.Text) ? 0.00 : Convert.ToDouble(txtShipping.Text);
                            }
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
                                        hdfTaxCode.Value = dtTaxDetails.Rows[0]["TAX_CODE"].ToString();
                                        hdfTaxRate.Value = dtTaxDetails.Rows[0]["TAX_RATE"].ToString();
                                        taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                        txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula)).Replace(",", "");
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
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("TaxDetails").ToString() + "','600','300');", true);
                            //}
                            //else
                            //{
                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            //}
                        }
                        break;
                    #endregion
                    #region SHIPPINGHEADER
                    case ActionsEnum.SHIPPINGHEADER:
                      //  dvPerc.Visible = false;
                        hdfTaxCategory.Value = ((int)TaxType.Shipping).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        if (SOInvoiceHeaderSession != null)
                        {
                            //ResetForm(ControlsEnum.SALEORDERDETAIL);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;

                            if(TempSOInvoiceHeaderSession!=null && TempSOInvoiceHeaderSession.TaxHdr!=null)
                            {
                                // hdfTaxCategory.Value = TempSOInvoiceHeaderSession.TaxHdr.Where(x => x.CIT_TAX_CATEGORY > 0).Select(x => x.CIT_TAX_CATEGORY).ToString();

                              //  hdfTaxCategory.Value= TempSOInvoiceHeaderSession.TaxHdr.Select(x =>  x.CIT_TAX_CATEGORY).ToString());
                            }
                            
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = grdItemDetails.FooterRow == null ? null : (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            if (lblSubTotal != null)
                            {
                                double taxable = Convert.ToDouble(SOInvoiceHeaderSession.ICH_AMOUNT_TC) - SOInvoiceHeaderSession.ICH_DISCOUNT_TC;
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
                                            hdfTaxCode.Value = dtTaxDetails.Rows[0]["TAX_CODE"].ToString();
                                            hdfTaxRate.Value = dtTaxDetails.Rows[0]["TAX_RATE"].ToString();
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
                                    //txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                    txtPopupOther.Text = Resources.Report.Freight;
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
                               divTaxApplicableAmount.Attributes.Add("style", "display:none;");
                                IsEditMode = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + GetLocalResourceObject("OtherCharges").ToString() + "','645','300');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                            }
                        }
                        break;
                    #endregion
                    #region Other Charge Popup
                    case ActionsEnum.OTHERCHARGEHEADER:                       
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowotherchargePop", "ShowContainerDiv('[id$=divOtherchargeSplitUp]','" + GetLocalResourceObject("OtherchargeDetails").ToString() + "','800','300');", true);
                        break;
                    #endregion                    
                    #region DISCHEADER
                    case ActionsEnum.DISCHEADER:
                        hdfTaxCategory.Value = ((int)TaxType.Discount).ToString();
                        hdfTaxFormula.Value = string.Empty;
                        hdfTaxRate.Value = string.Empty; hdfTaxCode.Value = string.Empty;
                        if (SOInvoiceHeaderSession != null)
                        {
                            //ResetForm(ControlsEnum.EXPENSEDETAIL);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.TAXPOPUPGRID);
                            GetFieldValues(ControlsEnum.TAXTYPES);
                            SetFieldValues(ControlsEnum.TAXTYPES);
                            lblSubTotal = grdItemDetails.FooterRow == null ? null : (Label)grdItemDetails.FooterRow.FindControl("lblSubTotalFooter");
                            if (lblSubTotal != null)
                            {
                                txtPopupItemAmount.Text = string.IsNullOrEmpty(lblSubTotal.Text.Trim()) ? ((double)0).ToString(hdfCurrencyFormat.Value) : Convert.ToDouble(lblSubTotal.Text).ToString(hdfCurrencyFormat.Value);
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
                                            hdfTaxCode.Value = dtTaxDetails.Rows[0]["TAX_CODE"].ToString();
                                            hdfTaxRate.Value = dtTaxDetails.Rows[0]["TAX_RATE"].ToString();
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula)).Replace(",", "");
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
                            SOInvoiceHeaderSession = TempSOInvoiceHeaderSession;
                            SetSubTotal();
                            SetHdrTax();
                        }
                        else
                        {
                            SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                            soMiscDetailsObj = TempSOInvoiceHeaderSession.OrderDetail.SingleOrDefault(crt => crt.CID_PK == SelectedDtlPK
                            && crt.CID_SL_NO == CurrSlNo);
                            SetDetailTax(TempSOInvoiceHeaderSession);
                        }
                        if (SOInvoiceHeaderSession.TaxHdr != null && SOInvoiceHeaderSession.TaxHdr.Count() > 0)
                        {
                            foreach (SOInvoiceTaxHdr SoInvhdr in SOInvoiceHeaderSession.TaxHdr)
                            {
                                if (SoInvhdr.CIT_TAX > 0)
                                {
                                    hdfTxPk.Value = SoInvhdr.CIT_TAX.ToString();
                                    TaxPK = SoInvhdr.CIT_TAX;
                                    GetFieldValues(ControlsEnum.TAXTYPES);

                                    if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                                    {
                                        hdfTaxFrmDt.Value = Convert.ToDateTime(dtTaxDetails.Rows[0]["TAX_FROM_DT"]).ToString(Resources.ErpRes.DateFormat);
                                        hdfTaxToDt.Value = Convert.ToDateTime(dtTaxDetails.Rows[0]["TAX_TO_DT"]).ToString(Resources.ErpRes.DateFormat);


                                    }


                                    break;
                                }
                            }


                        }
                        if (hdfIsTaxPayable.Value == "1")
                        {
                            SetFieldValues(ControlsEnum.SHOWDIVTAXBC);
                        }
                        ResetForm(ControlsEnum.TAXPOPUPGRID);

                        IsHeaderTax = false;
                        IsEditMode = false;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region CUSTOMERSELECTED
                    case ActionsEnum.CUSTOMERSELECTED:
                        hdfSubTotal.Value = "0";
                        GetFieldValues(ControlsEnum.CUSTOMERSELECTED);
                        SetFieldValues(ControlsEnum.CUSTOMERSELECTED);
                        GetFieldValues(ControlsEnum.PAYMENTTERMS);
                        SetFieldValues(ControlsEnum.PAYMENTTERMS);
                        GetFieldValues(ControlsEnum.GETDUEDATE);
                        SetFieldValues(ControlsEnum.GETDUEDATE);

                        //Add Customer Types Fileds
                        custPK = CusPk;
                        hdfCurrCustomerPK.Value = custPK.ToString();
                        GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                        SetFieldValues(ControlsEnum.CUSTOMERTYPES);

                        GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetBranchIDEnableDisable();

                        invoiceHeaderObj = SOInvoiceHeaderSession;
                        SOInvoiceHeaderSession.TaxHdr = new List<SOInvoiceTaxHdr>();
                        GetFieldValues(ControlsEnum.CUSTOMERRELATEDTAX);
                        if (!EnableItemTax)
                        {
                            IsHeaderTax = true;
                            SetFieldValues(ControlsEnum.CUSTOMERRELATEDTAX);
                        }
                        if (hdfIsTaxPayable.Value == "1")
                        {
                            SetFieldValues(ControlsEnum.SHOWDIVTAXBC);
                        }
                        txtCustomer.Focus();
                        break;
                    #endregion
                    #region TAXADD
                    case ActionsEnum.TAXADD:

                        //SelectedDtlPK
                        //SelectedCusItemPK
                        //SelectedItemPK


                        bool errorTaxAdd = false;
                        bool errorTaxAmount = false;
                        if (TempSOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = TempSOInvoiceHeaderSession;
                            tempInvTaxSplitObj = null;
                            if (IsHeaderTax)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                {
                                    tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr == null ? null :
                                        invoiceHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.CIT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && ctr.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                                else
                                {
                                    tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr == null ? null :
                                        invoiceHeaderObj.TaxHdr.SingleOrDefault(ctr => ctr.CIT_NAME == txtPopupOther.Text.Trim() && ctr.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                }
                            }
                            else
                            {
                                soMiscDetailsObj = invoiceHeaderObj.OrderDetail == null ? null :
                                    invoiceHeaderObj.OrderDetail.SingleOrDefault(ctr => ctr.CID_PK == SelectedDtlPK
                                    && ctr.CID_SL_NO == CurrSlNo);

                                if (soMiscDetailsObj != null)
                                {
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        tempInvTaxSplitObj = soMiscDetailsObj.TaxDtl == null ? null :
                                            soMiscDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_TAX == Convert.ToInt32(ddlPopupTaxType.SelectedValue) && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        tempInvTaxSplitObj = soMiscDetailsObj.TaxDtl == null ? null :
                                            soMiscDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_NAME == txtPopupOther.Text.Trim() && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                }
                            }
                            if (tempInvTaxSplitObj == null)
                            {
                                taxHdrList = new List<SOInvoiceTaxHdr>();

                                soInvTaxHdrObj = new SOInvoiceTaxHdr();
                                try
                                {
                                    soInvTaxHdrObj.CIT_TAX_AMT = string.IsNullOrEmpty(txtPopupAmount.Text.Trim()) ? 0 : Convert.ToDouble(txtPopupAmount.Text.Trim());
                                }
                                catch
                                {
                                    errorTaxAmount = true;
                                }
                                if (!errorTaxAmount)
                                {
                                    soInvTaxHdrObj.CIT_SL_NO = CurrSlNo;
                                    if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                    {
                                        soInvTaxHdrObj.CIT_TAX = Convert.ToInt32(ddlPopupTaxType.SelectedValue);
                                    }
                                    soInvTaxHdrObj.CIT_TAX_TEXT = HttpUtility.HtmlEncode(SelectedTaxText);
                                    soInvTaxHdrObj.CIT_NAME = HttpUtility.HtmlEncode(txtPopupOther.Text.Trim());
                                    soInvTaxHdrObj.CIT_PK = 0;
                                    //quotationTaxHdrObj.CIT_TAX_CATEGORY_TEXT = "Tax";
                                    soInvTaxHdrObj.CIT_TAX_CATEGORY = Convert.ToInt32(hdfTaxCategory.Value);
                                    soInvTaxHdrObj.CIT_TYPE = 1;
                                    soInvTaxHdrObj.CIT_TAX_FORMULA = string.IsNullOrEmpty(hdfTaxFormula.Value) ? string.Empty : hdfTaxFormula.Value;
                                    soInvTaxHdrObj.CIT_TAX_CODE = string.IsNullOrEmpty(hdfTaxCode.Value) ? string.Empty : hdfTaxCode.Value;
                                    soInvTaxHdrObj.CIT_TAX_RATE = string.IsNullOrEmpty(hdfTaxRate.Value) ? 0 : Convert.ToDouble(hdfTaxRate.Value);
                                    if (IsHeaderTax)
                                    {
                                        if (soInvTaxHdrObj.CIT_TAX_CATEGORY == (int)TaxType.Discount)
                                        {
                                            totalAmt = 0;
                                            currentTotal = 0;
                                            taxAmt = 0;

                                            totalAmt = Convert.ToDouble(invoiceHeaderObj.ICH_AMOUNT_TC);
                                            currentTotal = invoiceHeaderObj.TaxHdr == null ? 0 :
                                                invoiceHeaderObj.TaxHdr.Where(htx => htx.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.CIT_TAX_AMT);
                                            if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                            {
                                                taxAmt = CalculateTaxFormula(soInvTaxHdrObj.CIT_TAX_FORMULA, totalAmt);
                                            }
                                            else
                                            {
                                                taxAmt = soInvTaxHdrObj.CIT_TAX_AMT;
                                            }
                                            if (totalAmt >= (currentTotal + taxAmt))
                                            {
                                                if (invoiceHeaderObj.TaxHdr == null)
                                                    taxHdrList = new List<SOInvoiceTaxHdr>();
                                                else
                                                    taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                invoiceHeaderObj.TaxHdr = taxHdrList;
                                            }
                                            else
                                            {
                                                isValidDisc = false;
                                            }
                                        }
                                        else
                                        {
                                            if (invoiceHeaderObj.TaxHdr == null)
                                                taxHdrList = new List<SOInvoiceTaxHdr>();
                                            else
                                                taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                            taxHdrList.Add(soInvTaxHdrObj);
                                            invoiceHeaderObj.TaxHdr = taxHdrList;
                                        }
                                    }
                                    else
                                    {
                                        soMiscDetailsObj = invoiceHeaderObj.OrderDetail == null ? null :
                                            invoiceHeaderObj.OrderDetail.SingleOrDefault(item => item.CID_PK == SelectedDtlPK
                                            && item.CID_SL_NO == CurrSlNo);
                                        if (soMiscDetailsObj != null)
                                        {
                                            if (soInvTaxHdrObj.CIT_TAX_CATEGORY == (int)TaxType.Discount)
                                            {
                                                totalAmt = 0;
                                                currentTotal = 0;
                                                taxAmt = 0;

                                                totalAmt = soMiscDetailsObj.CID_AMOUNT;
                                                currentTotal = soMiscDetailsObj.TaxDtl == null ? 0 :
                                                    soMiscDetailsObj.TaxDtl.Where(dtx => dtx.CIT_TAX_CATEGORY == ((int)TaxType.Discount)).Sum(disc => disc.CIT_TAX_AMT);
                                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) > 0)
                                                {
                                                    taxAmt = CalculateTaxFormula(soInvTaxHdrObj.CIT_TAX_FORMULA, totalAmt);
                                                }
                                                else
                                                {
                                                    taxAmt = soInvTaxHdrObj.CIT_TAX_AMT;
                                                }
                                                if (totalAmt >= (currentTotal + taxAmt))
                                                {
                                                    taxHdrList = soMiscDetailsObj.TaxDtl == null ? new List<SOInvoiceTaxHdr>() : soMiscDetailsObj.TaxDtl.ToList();
                                                    taxHdrList.Add(soInvTaxHdrObj);
                                                    invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SelectedDtlPK
                                                        && rfq.CID_SL_NO == CurrSlNo).TaxDtl = taxHdrList;
                                                }
                                                else
                                                {
                                                    isValidDisc = false;
                                                }
                                            }
                                            else
                                            {
                                                taxHdrList = soMiscDetailsObj.TaxDtl == null ? new List<SOInvoiceTaxHdr>() : soMiscDetailsObj.TaxDtl.ToList();
                                                taxHdrList.Add(soInvTaxHdrObj);
                                                invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SelectedDtlPK
                                                    && rfq.CID_SL_NO == CurrSlNo).TaxDtl = taxHdrList;
                                            }
                                        }
                                    }
                                    TempSOInvoiceHeaderSession = invoiceHeaderObj;
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
                                    txtPopupAmount.Enabled = true;
                                    txtPopupOther.Enabled = true;
                                }
                                else
                                {
                                    txtPopupAmount.Enabled = false;
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
                        if (TempSOInvoiceHeaderSession != null)
                        {
                            invoiceHeaderObj = TempSOInvoiceHeaderSession;
                            HiddenField hdfTaxPK = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxPK") as HiddenField);
                            HiddenField hdfTaxName = (((sender as ImageButton).Parent.Parent as GridViewRow).FindControl("hdfTaxName") as HiddenField);
                            if (hdfTaxPK != null)
                            {
                                int taxPK = string.IsNullOrEmpty(hdfTaxPK.Value) ? 0 : Convert.ToInt32(hdfTaxPK.Value);
                                taxHdrList = new List<SOInvoiceTaxHdr>();
                                if (IsHeaderTax)
                                {
                                    if (taxPK > 0)
                                    {
                                        tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr == null ? null :
                                            invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_TAX == taxPK && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                    }
                                    else
                                    {
                                        if (hdfTaxName != null)
                                        {
                                            tempInvTaxSplitObj = invoiceHeaderObj.TaxHdr == null ? null :
                                                invoiceHeaderObj.TaxHdr.SingleOrDefault(rfq => rfq.CIT_NAME == hdfTaxName.Value && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                    }
                                    if (tempInvTaxSplitObj != null)
                                    {
                                        taxHdrList = invoiceHeaderObj.TaxHdr.ToList();
                                        taxHdrList.Remove(tempInvTaxSplitObj);
                                        invoiceHeaderObj.TaxHdr = taxHdrList;
                                    }
                                }
                                else
                                {
                                    soMiscDetailsObj = invoiceHeaderObj.OrderDetail == null ? null :
                                        invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SelectedDtlPK
                                        && rfq.CID_SL_NO == CurrSlNo);
                                    if (soMiscDetailsObj != null)
                                    {
                                        if (taxPK > 0)
                                        {
                                            tempInvTaxSplitObj = soMiscDetailsObj.TaxDtl == null ? null :
                                                soMiscDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_TAX == taxPK && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                        }
                                        else
                                        {
                                            if (hdfTaxName != null)
                                            {
                                                tempInvTaxSplitObj = soMiscDetailsObj.TaxDtl == null ? null :
                                                    soMiscDetailsObj.TaxDtl.SingleOrDefault(rfq => rfq.CIT_NAME == hdfTaxName.Value && rfq.CIT_TAX_CATEGORY == Convert.ToInt32(hdfTaxCategory.Value));
                                            }
                                        }
                                        soMiscDetailsObj = invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SelectedDtlPK
                                            && rfq.CID_SL_NO == CurrSlNo);
                                        if (soMiscDetailsObj != null && soMiscDetailsObj.TaxDtl != null)
                                        {
                                            taxHdrList = soMiscDetailsObj.TaxDtl.ToList();
                                            taxHdrList.Remove(tempInvTaxSplitObj);
                                            invoiceHeaderObj.OrderDetail.SingleOrDefault(rfq => rfq.CID_PK == SelectedDtlPK
                                                && rfq.CID_SL_NO == CurrSlNo).TaxDtl = taxHdrList;
                                        }
                                    }
                                }

                                TempSOInvoiceHeaderSession = invoiceHeaderObj;
                                SetFieldValues(ControlsEnum.TAXPOPUPGRID);

                            }
                            if (ddlPopupTaxType.Items.Count > 0)
                            {
                                if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                                {
                                    SelectedTaxText = Resources.Report.Custom;
                                    txtPopupAmount.Enabled = true;
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
                                            hdfTaxCode.Value = dtTaxDetails.Rows[0]["TAX_CODE"].ToString();
                                            hdfTaxRate.Value = dtTaxDetails.Rows[0]["TAX_RATE"].ToString();
                                            taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                            txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula)).Replace(",", "");
                                            //txtPopupAmount.Text = Math.Round(StringToFormula(taxFormula), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits + 1).ToString(hdfCurrencyFormat.Value).Replace(",", "");
                                            SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                            txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                        }
                                    }
                                    txtPopupAmount.Enabled = false;
                                    txtPopupOther.Enabled = false;
                                }
                            }
                        }


                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','" + (Convert.ToInt32(hdfTaxCategory.Value) == ((int)TaxType.Discount) ? GetLocalResourceObject("DiscountDetails").ToString() : GetLocalResourceObject("TaxDetails").ToString()) + "','600','300');", true);
                        break;
                    ////apply deleted change
                    //if (IsHeaderTax)
                    //{
                    //    SOInvoiceHeaderSession = TempSOInvoiceHeaderSession;
                    //    SetSubTotal();
                    //    SetHdrTax();
                    //}
                    //else
                    //{
                    //    SelectedDtlPK = string.IsNullOrEmpty(hdfDetailPK.Value) ? 0 : Convert.ToInt32(hdfDetailPK.Value);
                    //    soMiscDetailsObj = TempSOInvoiceHeaderSession.OrderDetail.SingleOrDefault(crt => crt.CID_PK == SelectedDtlPK
                    //    && crt.CID_SL_NO == CurrSlNo);
                    //    SetDetailTax(TempSOInvoiceHeaderSession);
                    //}
                    //if (SOInvoiceHeaderSession.TaxHdr != null && SOInvoiceHeaderSession.TaxHdr.Count() > 0)
                    //{
                    //    foreach (SOInvoiceTaxHdr SoInvhdr in SOInvoiceHeaderSession.TaxHdr)
                    //    {
                    //        if (SoInvhdr.CIT_TAX > 0)
                    //        {
                    //            hdfTxPk.Value = SoInvhdr.CIT_TAX.ToString();
                    //            TaxPK = SoInvhdr.CIT_TAX;
                    //            GetFieldValues(ControlsEnum.TAXTYPES);

                    //            if (dtTaxDetails != null && dtTaxDetails.Rows.Count == 1)
                    //            {
                    //                hdfTaxFrmDt.Value = Convert.ToDateTime(dtTaxDetails.Rows[0]["TAX_FROM_DT"]).ToString(Resources.ErpRes.DateFormat);
                    //                hdfTaxToDt.Value = Convert.ToDateTime(dtTaxDetails.Rows[0]["TAX_TO_DT"]).ToString(Resources.ErpRes.DateFormat);


                    //            }


                    //            break;
                    //        }
                    //    }


                    //}
                    ////ResetForm(ControlsEnum.TAXPOPUPGRID);
                    //IsHeaderTax = false;
                    //IsEditMode = false;

                    ////close apply deleted change     
                    ////ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divItemTax]','Tax','600','300');", true);
                    //break;
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
                                hdfTaxCode.Value = dtTaxDetails.Rows[0]["TAX_CODE"].ToString();
                                hdfTaxRate.Value = dtTaxDetails.Rows[0]["TAX_RATE"].ToString();
                                hdfTaxCategory.Value = dtTaxDetails.Rows[0]["TAX_CATEGORY"].ToString();
                                taxFormula = taxFormula.Replace("#SUBTOTAL#", txtPopupItemAmount.Text.Trim());
                                txtPopupAmount.Text = GetFormattedCurrency(StringToFormula(taxFormula)).Replace(",", "");
                                SelectedTaxText = HttpUtility.HtmlEncode(dtTaxDetails.Rows[0]["TAX_HEAD"].ToString());
                                txtPopupOther.Text = ddlPopupTaxType.SelectedItem.Text;
                                if (taxFormula == "0")
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
                        }
                        else if (Convert.ToInt32(ddlPopupTaxType.SelectedValue) == -1)
                        {
                            hdfTaxFormula.Value = string.Empty;
                            hdfTaxCode.Value = string.Empty;
                            hdfTaxRate.Value = string.Empty;
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
                        SetDetailTax(TempSOInvoiceHeaderSession);
                        //SetHdrTax();
                        ResetForm(ControlsEnum.TAXPOPUPGRID);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        ResetForm(ControlsEnum.MISCLIST);
                        CurrPK = 0;
                        hdfCustomerID.Value = "";
                        ddlPaymentTerms.Items.Clear();
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
                        //GetFieldValues(ControlsEnum.PAYMENTTERMS);
                        SetFieldValues(ControlsEnum.PAYMENTTERMS);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Pick for Receipt
                    case ActionsEnum.PICKFORRECEIPT:
                        SetUIValuesToObject(ControlsEnum.PICKFORRECEIPT);
                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        TempSOInvoiceHeaderSession = new SOInvoiceHeader();
                        SOInvoiceHeaderSession = new SOInvoiceHeader();
                        EntryStatus = EntryStatus.NEWMODE;
                        txtInvoiceDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                        //Set currentdate in new mode For ETD & ETA
                        txtETD.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                        txtETA.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                        txtInvoiceDueDate.Text = DateTime.Now.ToString(Resources.ErpRes.DateFormat);
                        txtTerm2.Text = GetLocalResourceObject("Term2Text").ToString();

                        AST_DOC_MODE.Value = GetDOCMODE();
                        AST_CODE.Value = ApplicationType.MSI;
                        lblInvoiceNo.Text = hdfInvoiceNo.Value == string.Empty ? "[NEW]" : hdfInvoiceNo.Value;
                        hfCancelInv.Value = "0";
                        ddlInvoiceType.Enabled = true;
                        hdfAppType.Value = ApplicationType.MSI;
                        hdfAppSubType.Value = string.Empty;
                        hdfIsItemDetailsVisible.Value = CommonConstants.SELECT_VALUE_ONE;
                        base.WkfRefID = ucrWrkf.RefID = 0;
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        ucrWrkf.ViewType = 1;
                        ucrWrkf.ViewAction();
                        ResetForm(ControlsEnum.ISSUEDETAILSSPLITUP);
                        chkDummy.Checked = false;
                        divDummy.Visible = false;
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
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(1);
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
                            }
                            ucrWrkf.ViewAction();

                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            GetFieldValues(ControlsEnum.TAXSETTINGS);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);

                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            if (hdfIsTaxPayable.Value == "1")
                            {
                                SetFieldValues(ControlsEnum.SHOWDIVTAXBC);
                            }
                            //SetDetailTax(null);
                            //SetHdrTax();
                            if (grdMiscList.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }
                        }
                        else
                        {

                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////
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
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.SalesInvoiceBL.DeleteSalesInvoiceDetails(currentUser.PKUser.ToString(), CurrPK, LastModifiedTime, null, ApplicationType.MSI);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm(ControlsEnum.MISCLIST);
                                GetFieldValues(ControlsEnum.MISCLIST);
                                SetFieldValues(ControlsEnum.MISCLIST);
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
                                    litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.MISCLIST);
                                    GetFieldValues(ControlsEnum.MISCLIST);
                                    SetFieldValues(ControlsEnum.MISCLIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.MISCLIST);
                                    GetFieldValues(ControlsEnum.MISCLIST);
                                    SetFieldValues(ControlsEnum.MISCLIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString());
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm(ControlsEnum.MISCLIST);
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
                        break;
                    #endregion
                    #region Expense List
                    case ActionsEnum.INVOICELIST:
                        ResetForm(ControlsEnum.MISCLIST);
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Expense Details
                    case ActionsEnum.INVOICEDETAIL:
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////
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
                                //EntryStatus = EntryStatus.VIEWMODE;
                                ucrWrkf.ViewAction();
                            }
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);
                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            if (hdfIsTaxPayable.Value == "1")
                            {
                                SetFieldValues(ControlsEnum.SHOWDIVTAXBC);
                            }

                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
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
                    #region Submit
                    case ActionsEnum.SUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region Submit
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else if (SOInvoiceHeaderSession.OrderDetail == null || SOInvoiceHeaderSession.OrderDetail.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Min_Items").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                        }
                        else//valid
                        {
                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                invoiceHeaderObj = new SOInvoiceHeader();
                                invoiceHeaderMulObj = (SOInvoiceHeaderMul)SetUIValuesToObject(ControlsEnum.SOINVHEADER);

                                if (hdfExchangeRate.Value != "-1")
                                {
                                    invoiceHeaderObj.WKF_FLAG = 1;

                                    if (invoiceHeaderObj != null && invoiceHeaderObj.OrderDetail != null)
                                    {
                                        string xmlDoc = CommonFunctions.XmlSerialize<SOInvoiceHeaderMul>(invoiceHeaderMulObj); //string xmlDoc = CommonFunctions.XmlSerialize<SOInvoiceHeader>(invoiceHeaderObj);//CommonFunctions.ObjectTOXml(rfqResponseHeaderObj);
                                        // save Process Control inspection details
                                        //result = BusinessLogic.POInvoicing.POInvoiceBL.SaveSOInvoiceHeader(xmlDoc);
                                        result = BusinessLogic.Sales.SalesInvoiceBL.SaveSalesInvoiceHeader(xmlDoc);
                                        if (result.HasValue && result.Value > 0) // Success !  redirect to listing page
                                        {
                                            ucrWrkf.ApplicationID = result.Value;
                                            if (string.IsNullOrEmpty(lblInvoiceNo.Text.Trim())
                                           || lblInvoiceNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                            {
                                                CurrPK = ucrWrkf.ApplicationID;
                                                GetFieldValues(ControlsEnum.SOINVHEADER);
                                                lblInvoiceNo.Text = invoiceHeaderObj.ICH_NO;
                                                ucrAlert.TypeRef = lblInvoiceNo.Text.Trim();
                                            }

                                            #region ALERTSAVE
                                            GetFieldValues(ControlsEnum.ALERTCONFIG);
                                            int isAlert = 0;
                                            if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                                            {
                                                isAlert = admAppConstMstList[0].ACF_VALUE;
                                            }
                                            if (isAlert == 1)
                                            {

                                                invPK = (int)result;
                                                AlertBO alertBoObj = new AlertBO();
                                                alertBoObj = (AlertBO)SetUIValuesToObject(ControlsEnum.ALERTSAVE);
                                                if (alertBoObj != null)
                                                {
                                                    alertresult = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                                                }

                                            }
                                            #endregion
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
                                                litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + Resources.Messages.EditUsedByAnotherUser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.CODEEXIST)
                                            {
                                                litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                            }
                                            else if (result == (int)DbSaveStatus.REFNOEXIST)
                                            {
                                                litErrorMsg.Text = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString() + " " + GetLocalResourceObject("RefNoExist").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "','" + "');", true);
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString());
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                            }
                                            return;
                                        }
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
                            //|| (Request.QueryString[QueryStrings.PageType] != null &&
                            //Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Cancel))
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.SI))
                                {
                                    ucrWrkf.ApplicationID = CurrPK;
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_SI_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                                        FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm(ControlsEnum.MISCLIST);
                                    GetFieldValues(ControlsEnum.MISCLIST);
                                    SetFieldValues(ControlsEnum.MISCLIST);
                                }
                            }
                            else
                                ucrWrkf.ApplicationID = CurrPK;

                            if (ucrWrkf.ApplicationID > 0)
                            {
                                ddlWkfAction = (DropDownList)ucrWrkf.FindControl("WRKFACT_ID");
                                WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                //Do WorkFlow if WorkFlow has Actions
                                if (ddlWkfAction.Items.Count > 0)
                                {
                                    action = ddlWkfAction.SelectedItem.ToString();
                                    result = ucrWrkf.DoWorkFlow();
                                    if (result.HasValue && result.Value > 0)
                                    {
                                        ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                        if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
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
                                        WrkfComments.Text = "";
                                        //Show Save success message and reset Contract Entry
                                        // litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;

                                        string invoiceNo = string.Empty;
                                        if (string.IsNullOrEmpty(lblInvoiceNo.Text.Trim())
                                            || lblInvoiceNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                        {
                                            CurrPK = ucrWrkf.ApplicationID;
                                            GetFieldValues(ControlsEnum.SOINVHEADER);
                                            invoiceNo = invoiceHeaderObj.ICH_NO;
                                        }
                                        else
                                        {
                                            invoiceNo = lblInvoiceNo.Text.Trim();
                                        }

                                        object[] args = new object[2];
                                        args[0] = GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString();
                                        //   args[1] = lblInvoiceNo.Text.Trim() == "[NEW]" ? "" : lblInvoiceNo.Text;
                                        args[1] = invoiceNo;


                                        #region LOG SAVE
                                        CommonServiceClient = new CommonService();
                                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                                        //invoiceHeaderObj = CommonFunctions.Initilize<SOInvoiceHeader>();
                                        //invoiceHeaderObj = (SOInvoiceHeader)SetUIValuesToObject(ControlsEnum.SOINVHEADER);
                                        //if (invoiceHeaderObj != null)
                                        //{
                                        List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                        AdmTrxLogDet.ATL_APP_TRX_CODE = invoiceNo;
                                        AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.MSI;
                                        AdmTrxLogDet.ATL_MOD_BY = currentUser.PKUser;
                                        AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                                        AdmTrxLogDet.ATL_BIZUNIT = currentUser.SBUID;
                                        AdmTrxLogDet.ATL_APP_TRX_PK = ucrWrkf.ApplicationID;
                                        AdmTrxLogDet.ATL_PK = 0;
                                        AdmTrxLogList.Add(AdmTrxLogDet);
                                        CommonServiceClient.SaveLog(AdmTrxLogList);
                                        //}
                                        #endregion
                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                        // Show Save Message and redired to listing page                                        
                                        //litErrorMsg.Text = Resources.ErrorMessages.Msg_Submit_Success;
                                        //litErrorMsg.Text = string.Format(litErrorMsg.Text, GetGlobalResourceObject("PageNameRes", "miscellaneous").ToString());
                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.Invoicing) + "');", true);


                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm(ControlsEnum.MISCLIST);
                                            GetFieldValues(ControlsEnum.MISCLIST);
                                            SetFieldValues(ControlsEnum.MISCLIST);
                                        }
                                    }
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
                        break;
                    #endregion
                    #region Pick for Paying
                    case ActionsEnum.PICKFORPAYMENT:
                        SetUIValuesToObject(ControlsEnum.PICKFORPAYMENT);
                        break;
                    #endregion
                    #region Reset
                    case ActionsEnum.RESET:
                        SelectedVendors = 0;
                        SelectedCurrency = 0;
                        SelectedInvoices = null;
                        SelectedInvoiceType = 0;
                        SelectedCustomers = 0;
                        SelectedSalesInvoices = new List<long>();
                        SelectedInvoicesCrDr = new List<long>();
                        btnPickForReceipt.Text = GetLocalResourceObject("PickInvforReceipt").ToString();
                        btnPickForCrDrNote.Text = GetLocalResourceObject("PickInvforCNDN").ToString();
                        //Resetting Color
                        hdfSelectedItemPk.Value = "0";
                        ResetForm(ControlsEnum.INVOICEGRID);
                        ResetForm(ControlsEnum.ISSUEDETAILSSPLITUP);
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        finExpenseVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        finExpenseVndHdrObj.IVH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.SOINVHEADER);
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
                        ResetForm(ControlsEnum.MISCLIST);
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.MISCLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.MISCLIST);

                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        //    Response.Redirect(Resources.PageURL.InboxURL);
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
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
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = (int)salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.MISCLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm(ControlsEnum.MISCLIST);
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.MISCLIST);
                        SetFieldValues(ControlsEnum.MISCLIST);
                        break;
                    #endregion
                    #region Alert
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = ApplicationType.MSI;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblInvoiceNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtInvoiceDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvoiceDate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        ucrAlert.TypePartyName = txtCustomer.Text;
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        if (ddlInvoiceType.SelectedValue != string.Empty)
                        {
                            if (ddlInvoiceType.SelectedItem.Text.ToString().ToLower() == "domestic")
                            {
                                //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=1"), false);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=1") + "');", true);
                            }
                            else if (ddlInvoiceType.SelectedItem.Text.ToString().ToLower() == "export"|| ddlInvoiceType.SelectedItem.Text.ToString()== " Deemed Export")
                            {
                                //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=2"), false);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=2") + "');", true);

                            }
                            else if (ddlInvoiceType.SelectedItem.Text.ToString().ToLower() == "proforma")
                            {
                                //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=2"), false);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=3") + "');", true);

                            }
                        }
                        break;
                    #endregion
                    #region Printlisting
                    case ActionsEnum.PRINTLISTING:
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            HiddenField hdfInvTypeText;
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                hdfInvTypeText = (HiddenField)grdrow.FindControl("hdfInvTypeText");
                                if (hdfInvTypeText.Value != string.Empty)
                                {
                                    if (hdfInvTypeText.Value.ToString().ToLower() == "domestic")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=1") + "');", true);
                                    }
                                    else if (hdfInvTypeText.Value.ToString().ToLower() == "export"||hdfInvTypeText.Value.ToString()== " Deemed Export")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=2") + "');", true);
                                    }
                                    else if (hdfInvTypeText.Value.ToString().ToLower() == "proforma")
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=3") + "');", true);
                                    }
                                }

                                return;
                            }
                        }
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        break;
                    #endregion
                    #region DOPRINT
                    case ActionsEnum.DOPRINT:
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            //HiddenField hdfInvTypeText;
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                    ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.MSI + "&APPSUBTYPE=21") + "');", true);

                                return;
                            }
                        }
                        litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                ////
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(11);
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
                            }
                            ucrWrkf.ViewAction();

                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            GetFieldValues(ControlsEnum.SOINVHEADER);
                            SetFieldValues(ControlsEnum.SOINVHEADER);
                            GetFieldValues(ControlsEnum.TAXSETTINGS);
                            SetFieldValues(ControlsEnum.EXPENSEDETAIL);

                            TempSOInvoiceHeaderSession = SOInvoiceHeaderSession;
                            //SetDetailTax(null);
                            //SetHdrTax();
                            if (grdMiscList.Rows.Count > 0)
                            {
                                SetSubTotal();
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
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
                    #region Tab navigation
                    case ActionsEnum.DEFAULT:
                        CheckUserRightsAndRedirect(Resources.PageURL.SoListing);
                        //Response.Redirect(Resources.PageURL.SoListing);
                        break;
                    //case ActionsEnum.SALESINVOICE:
                    //    Response.Redirect(Resources.PageURL.SalesInvoicing);
                    //    break;
                    //case ActionsEnum.SALESRECEIPT:
                    //    Response.Redirect(Resources.PageURL.SalesReceipt);
                    //    break;
                    //case ActionsEnum.ACRECEIVABLE:
                    //    Response.Redirect(Resources.PageURL.AccountsReceivable);
                    //    break;
                    //case ActionsEnum.CRDRNOTE:
                    //    Response.Redirect(Resources.PageURL.DrCrNoteSales);
                    //    break;
                    case ActionsEnum.SALESINVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.SalesInvoicing);
                        //Response.Redirect(Resources.PageURL.SalesInvoicing);
                        break;
                    case ActionsEnum.INVOICE:
                        SOInvoiceHeaderSession = null;
                        CheckUserRightsAndRedirect(Resources.PageURL.Invoicing);
                        //Response.Redirect(Resources.PageURL.Invoicing);
                        break;
                    case ActionsEnum.DELIVERYORDER:
                        CheckUserRightsAndRedirect(Resources.PageURL.DeliveryOrder);
                        //Response.Redirect(Resources.PageURL.DeliveryOrder);
                        break;
                    case ActionsEnum.SALESRECEIPT:
                        CheckUserRightsAndRedirect(Resources.PageURL.SalesReceipt);
                        //Response.Redirect(Resources.PageURL.SalesReceipt);
                        break;
                    case ActionsEnum.ACRECEIVABLE:
                        RadioButton rbtnn;
                        foreach (GridViewRow grdrow in grdMiscList.Rows)
                        {
                            rbtnn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtnn.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                break;
                            }
                        }
                        CheckUserRightsAndRedirect(Resources.PageURL.AccountReceivable);
                        //Response.Redirect(Resources.PageURL.AccountsReceivable);
                        break;
                    case ActionsEnum.CRDRNOTE:
                        CheckUserRightsAndRedirect(Resources.PageURL.DrCrNoteSales);
                        //Response.Redirect(Resources.PageURL.DrCrNoteSales);
                        break;
                    case ActionsEnum.MISC:
                        CheckUserRightsAndRedirect(Resources.PageURL.MiscellaneousInv);
                        //Response.Redirect(Resources.PageURL.Misc);
                        break;
                    #endregion
                    #region DueDate
                    case ActionsEnum.GETDUEDATE:
                        GetFieldValues(ControlsEnum.GETDUEDATE);
                        SetFieldValues(ControlsEnum.GETDUEDATE);
                        break;
                    #endregion
                    #region
                    case ActionsEnum.CUSTOMERTYPECHANGING:
                        GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetBranchIDEnableDisable();
                        break;
                    #endregion
                    #region CURRENCYSELECTED
                    case ActionsEnum.CURRENCYSELECTED:
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        txtExchangeRate.Text = Convert.ToDouble(hdfExchangeRate.Value).ToString();
                        if (hdfIsTaxPayable.Value == "1")
                        {
                            SetFieldValues(ControlsEnum.SHOWDIVTAXBC);
                        }
                        break;
                    #endregion
                    #region CHANGEEXCHANGERATE
                    case ActionsEnum.CHANGEEXRATE:
                        hdfExchangeRate.Value = string.IsNullOrEmpty(txtExchangeRate.Text) ? "1" : txtExchangeRate.Text;
                        break;
                    #endregion
                    #region Issue SPLITUP POPUP
                    case ActionsEnum.SHOWISSUEPOPUP:
                        int ItemPk = 0, CustomerPk = 0;
                        int.TryParse(hdfItemID.Value, out ItemPk);
                        CustomerPk = hdfCustomerID.Value != string.Empty ? Convert.ToInt32(hdfCustomerID.Value) : 0;
                        if (CustomerPk > 0)
                        {
                            if (ItemPk > 0)
                            {
                                GetFieldValues(ControlsEnum.ISSUETYPE);
                                SetFieldValues(ControlsEnum.ISSUETYPE);
                                BindGrid(ControlsEnum.ISSUEDETAILSSPLITUPAPPLIED);
                                SelectedItemPK = string.IsNullOrEmpty(hdfItemID.Value) ? 0 : Convert.ToInt32(hdfItemID.Value);
                                hdfIssueExceed.Value = "0";
                                ddlIssueType.Focus();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalIssue", "$(document).ready(function(){CalculateTotalIssue();});", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divIssueQtySpilup]','" + GetLocalResourceObject("IssueAllocationDetails").ToString() + "','700','500');", true);
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Item").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Customer").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region SHOWISSUEDETAILS
                    case ActionsEnum.SHOWISSUEDETAILS:
                        //ResetForm(ControlsEnum.ISSUEDETAILSSPLITUP);                
                        GetFieldValues(ControlsEnum.ISSUEDETAILSSPLITUP);
                        SetFieldValues(ControlsEnum.ISSUEDETAILSSPLITUP);
                        // BindGrid(ControlsEnum.ISSUEDETAILSSPLITUP);
                        ddlIssueType.Focus();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalIssue", "$(document).ready(function(){CalculateTotalIssue();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divIssueQtySpilup]','" + GetLocalResourceObject("IssueAllocationDetails").ToString() + "','700','500');", true);
                        break;
                    #endregion
                    #region GETINVGSTTYPE
                    case ActionsEnum.GETINVGSTTYPE:
                        GetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                        SetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                        if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == (int)SalesInvoiceType.Export && IsInvoiceGSTEnable)
                        {
                            ddlSubType.Visible = true;
                            lblSubType.Visible = true;
                        }
                        else
                        {
                            ddlSubType.ClearSelection();
                            ddlSubType.Visible = false;
                            lblSubType.Visible = false;
                        }
                        break;
                    #endregion
                    #region ISSUEAPPLY (APPLY SCRAP/EMI SPLITUP POPUP DETAILS)
                    case ActionsEnum.ISSUEAPPLY:
                        //Checking for Inv. Now Qty entered exceeds Issue Qty
                        bool issueContinue = true;
                        if (hdfIssueExceed.Value == "0")
                        {
                            foreach (GridViewRow grdrowitem in grdIssueDetails.Rows)
                            {
                                TextBox txtInvNowIssueQty = (TextBox)grdrowitem.FindControl("txtInvNowIssueQty");
                                Label lblIssueBalance = (Label)grdrowitem.FindControl("lblIssueBalance");
                                double balQty = Convert.ToDouble(lblIssueBalance.Text.Replace(",", ""));
                                if (Convert.ToDouble(txtInvNowIssueQty.Text) > balQty)
                                {
                                    issueContinue = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowIssueNowQtyExceeds", "$(document).ready(function(){ShowIssueNowQtyExceeds();});", true);
                                    break;
                                }
                            }
                        }
                        if (issueContinue)
                        {
                            double totalIssueInvNow = 0;
                            IssueListApplied = new List<IssueDetail>();
                            int issuedPrdctUOMPK = 0;
                            string issuedPrdctUOMCode = "";
                            foreach (GridViewRow gvr in grdIssueDetails.Rows)
                            {
                                if (gvr.RowType == DataControlRowType.DataRow)
                                {
                                    TextBox txtInvNowIssueQty = gvr.FindControl("txtInvNowIssueQty") as TextBox;
                                    HiddenField hdfCII_PK = gvr.FindControl("hdfCII_PK") as HiddenField;
                                    HiddenField hdfIssueHdrPK = gvr.FindControl("hdfIssueHdrPK") as HiddenField;//(HBC_PK) in PRD_BIN_CARD_CONS_HDR
                                    HiddenField hdfIssueDtlPK = gvr.FindControl("hdfIssueDtlPK") as HiddenField;
                                    Label lblIssueDate = gvr.FindControl("lblIssueDate") as Label;
                                    Label lblIssueQty = gvr.FindControl("lblIssueQty") as Label;
                                    Label lblIssueInvdQty = gvr.FindControl("lblIssueInvdQty") as Label;
                                    Label lblIssueBalance = gvr.FindControl("lblIssueBalance") as Label;
                                    Label lnkISSUENO = gvr.FindControl("lnkISSUENO") as Label;
                                    //For setting default UOM as the UOM of Product used in the scrap issue allocation popup
                                    HiddenField hdfIssuedProductUOMPK = gvr.FindControl("hdfIssuedProductUOMPK") as HiddenField;
                                    issuedPrdctUOMPK = Convert.ToInt16(hdfIssuedProductUOMPK.Value);
                                    HiddenField hdfIssuedProductUOMCODE = gvr.FindControl("hdfIssuedProductUOMCODE") as HiddenField;
                                    issuedPrdctUOMCode = hdfIssuedProductUOMCODE.Value.ToString();
                                    if (txtInvNowIssueQty.Text != string.Empty)
                                    {
                                        totalIssueInvNow += Convert.ToDouble(txtInvNowIssueQty.Text);
                                    }
                                    //if (Convert.ToDouble(txtInvNowIssueQty.Text) > 0)
                                    //{
                                    soIssueQtyObj = new IssueDetail();
                                    soIssueQtyObj.CII_PK = 0;
                                    soIssueQtyObj.CII_TYPE = Convert.ToInt32(ddlIssueType.SelectedValue);
                                    soIssueQtyObj.CII_ISSUE_NO = lnkISSUENO.Text;
                                    soIssueQtyObj.CII_ISSUE_DATE = lblIssueDate.Text;

                                    if (Convert.ToInt32(ddlIssueType.SelectedValue) == 1)
                                    {
                                        soIssueQtyObj.CII_ITEM_CONS_DTL = Convert.ToInt32(hdfIssueDtlPK.Value);
                                    }
                                    else
                                    {
                                        soIssueQtyObj.CII_BC_CONS_DTL = Convert.ToInt32(hdfIssueDtlPK.Value);
                                    }
                                    soIssueQtyObj.CII_ISSUE_PK = Convert.ToInt32(hdfIssueHdrPK.Value);
                                    soIssueQtyObj.CII_ISSUE_DTL = Convert.ToInt32(hdfIssueDtlPK.Value);
                                    soIssueQtyObj.CII_QTY_ISSUED = Convert.ToDouble(lblIssueQty.Text);
                                    soIssueQtyObj.CII_QTY_PRV_INVOICED = Convert.ToDouble(lblIssueInvdQty.Text);
                                    soIssueQtyObj.CII_QTY_BALANCE = Convert.ToDouble(lblIssueBalance.Text);
                                    soIssueQtyObj.CII_QTY_INVOICED = Convert.ToDouble(txtInvNowIssueQty.Text);
                                    soIssueQtyObj.CII_UOM = Convert.ToInt16(hdfIssuedProductUOMPK.Value);
                                    soIssueQtyObj.CII_UOM_CODE = hdfIssuedProductUOMCODE.Value.ToString();
                                    soIssueQtyObj.CII_SL_NO = 0;

                                    IssueListApplied.Add(soIssueQtyObj);
                                    //}
                                }
                            }
                            if(grdIssueDetails.Rows.Count>0)
                            {
                            txtQty.Text = totalIssueInvNow.ToString();
                            SelectedIssueTypePk = Convert.ToInt32(ddlIssueType.SelectedValue);
                            hdfUOM.Value = issuedPrdctUOMPK.ToString();
                            txtUOM.Text = HttpUtility.HtmlDecode(issuedPrdctUOMCode);
                            }

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_DisableUOMauto", "DisableUOMAutocomplete();", true);

                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePagePopup", "ClosePopup();", true);
                        break;
                    #endregion
                    #region ResetIssueAllocation
                    case ActionsEnum.RESETISSUEALLOCATION:
                        ResetForm(ControlsEnum.ISSUEDETAILSSPLITUP);
                        break;
                    #endregion
                    #region Pick Inv & for Cr/Dr. Note
                    case ActionsEnum.PICKFORCRDRNOTE:
                        Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                        SetUIValuesToObject(ControlsEnum.PICKFORCRDRNOTE);
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
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                CommonServiceClient = null;
                salesInvoiceServiceClient = null;
            }
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            Label lblItemTotalQty;
            Label lblItemTotalAmount;
            Label lblDiscountTotal;
            Label lblTaxTotal;
            Label lblSubTotalFooter;
            Label lblSubTotal;
            Label lblTaxAmountBC;

            try
            {
                //if (((GridView)sender).ID == "grdMiscList")
                //{
                //    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                //    {
                //        Button imgPosted = e.Row.FindControl("imgPosted") as Button;
                //        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;
                //        if (dtMISCLIST != null)
                //        {
                //            if (dtMISCLIST.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString() != "" || dtMISCLIST.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString() != string.Empty)
                //            {
                //                imgPosted.CssClass = dtMISCLIST.Rows[e.Row.RowIndex]["FTH_CSS_CLASS"].ToString();
                //                imgPosted.ToolTip = dtMISCLIST.Rows[e.Row.RowIndex]["FTH_STATUS_TEXT"].ToString();
                //            }
                //            else
                //            {
                //                imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                //                imgPosted.ToolTip = Resources.Captions.NotPosted;
                //            }
                //        }
                //        //if (Convert.ToBoolean(hdfPosted.Value) == true)
                //        //{
                //        //    imgPosted.CssClass = GetLocalResourceObject("posted").ToString();
                //        //    imgPosted.ToolTip = Resources.Captions.Posted;
                //        //}
                //        //else
                //        //{
                //        //    imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                //        //    imgPosted.ToolTip = Resources.Captions.NotPosted;
                //        //}
                //    }
                //}
                if ((sender as GridView).ID == "grdItemDetails")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        e.Row.Cells[6].Visible = EnableItemDiscount;
                        e.Row.Cells[7].Visible = EnableItemTax;
                        e.Row.Cells[5].Visible = EnableItemDiscount || EnableItemTax;
                        //Hide delete button  from view mode
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            ImageButton btnRemoveItem = e.Row.FindControl("btnRemoveItem") as ImageButton;
                            btnRemoveItem.Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer && soMiscDetailsList != null)
                    {
                        e.Row.Cells[6].Visible = EnableItemDiscount;
                        e.Row.Cells[7].Visible = EnableItemTax;
                        e.Row.Cells[5].Visible = EnableItemDiscount || EnableItemTax;

                        lblItemTotalQty = e.Row.FindControl("lblItemTotalQty") as Label;
                        lblItemTotalAmount = e.Row.FindControl("lblItemTotalAmount") as Label;
                        lblDiscountTotal = e.Row.FindControl("lblDiscountTotal") as Label;
                        lblTaxTotal = e.Row.FindControl("lblTaxTotal") as Label;
                        lblSubTotalFooter = e.Row.FindControl("lblSubTotalFooter") as Label;

                        if (lblItemTotalQty != null)
                        {
                            lblItemTotalQty.Text = lblItemTotalQty.ToolTip = soMiscDetailsList.Sum(itm => itm.CID_QTY_INVOICED).ToString("N");
                        }
                        if (lblItemTotalAmount != null)
                        {
                            lblItemTotalAmount.Text = lblItemTotalAmount.ToolTip = soMiscDetailsList.Sum(itm => itm.CID_AMOUNT).ToString("N");
                        }
                        if (lblDiscountTotal != null)
                        {
                            lblDiscountTotal.Text = lblDiscountTotal.ToolTip = soMiscDetailsList.Sum(itm => itm.CID_DISCOUNT).ToString("N");
                        }
                        if (lblTaxTotal != null)
                        {
                            lblTaxTotal.Text = lblTaxTotal.ToolTip = soMiscDetailsList.Sum(itm => itm.CID_TAX).ToString("N");
                        }
                        if (lblSubTotalFooter != null)
                        {
                            hdfSubTotal.Value = lblSubTotalFooter.Text = lblSubTotalFooter.ToolTip = soMiscDetailsList.Sum(itm => itm.CID_NET_AMOUNT).ToString(hdfCurrencyFormat.Value);//(hdfMiscRateFormat.Value);
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        e.Row.Cells[6].Visible = EnableItemDiscount;
                        e.Row.Cells[7].Visible = EnableItemTax;
                        e.Row.Cells[5].Visible = EnableItemDiscount || EnableItemTax;
                    }
                }
                else if ((sender as GridView).ID == "grdTaxBaseCUr")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        lblSubTotal = e.Row.FindControl("lblSubTotal") as Label;
                        lblTaxAmountBC = e.Row.FindControl("lblTaxAmountBC") as Label;

                        lblSubTotal.Text = ((TempSOInvoiceHeaderSession.ICH_AMOUNT_TC) * Convert.ToDouble(hdfExchangeRate.Value)).ToString(hdfCurrencyFormat.Value);
                    }
                }
                else if (((GridView)sender).ID == "grdIssueDetails")
                {
                    if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalIssueQty = (Label)e.Row.FindControl("lblTotalIssueQty");
                        Label lblTotalIssueInvdQty = (Label)e.Row.FindControl("lblTotalIssueInvdQty");
                        Label lblTotalISSUEBalance = (Label)e.Row.FindControl("lblTotalISSUEBalance");
                        lblTotalIssueQty.Text = GetFormattedNumberWithSeperation(TotalIssueQty);
                        lblTotalIssueInvdQty.Text = GetFormattedNumberWithSeperation(TotalIssueInvdQty);
                        lblTotalISSUEBalance.Text = GetFormattedNumberWithSeperation(TotalIssueBalance);
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
            PageIndex = e.NewPageIndex.ToString();
            GetFieldValues(ControlsEnum.MISCLIST);
            SetFieldValues(ControlsEnum.MISCLIST);
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
                SortBy = e.SortExpression;
                if (SortDirection == Resources.Report.SortAscending)
                    SortDirection = Resources.Report.SortDescending;
                else
                    SortDirection = Resources.Report.SortAscending;
                GetFieldValues(ControlsEnum.MISCLIST);
                SetFieldValues(ControlsEnum.MISCLIST);
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
            btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            //btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnAlert.PreRender += new EventHandler(btnAction_PreRender);

            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForReceipt.PreRender += new EventHandler(btnAction_PreRender);
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForCrDrNote.PreRender += new EventHandler(btnAction_PreRender);

            //lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnbDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            //lnbSalesInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAdvanceInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lnbMiscellaneous.PreRender += new EventHandler(btnAction_PreRender);
            //lbnSalesReceipt.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);


            btnAddItem.PreRender += new EventHandler(btnAction_PreRender);
            btnClearItem.PreRender += new EventHandler(btnAction_PreRender);
            //btnEditItem.PreRender += new EventHandler(btnAction_PreRender);
            //btnRemoveItem.PreRender += new EventHandler(btnAction_PreRender);
            btnApply.PreRender += new EventHandler(btnAction_PreRender);
            imgPopupAdd.PreRender += new EventHandler(btnAction_PreRender);
            //imbTaxRemove.PreRender += new EventHandler(btnAction_PreRender);


            //Load += new EventHandler(btnAction_Load);

            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteNew.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            //btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnAlert.Load += new EventHandler(btnAction_Load);

            btnNew.Load += new EventHandler(btnAction_Load);
            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            btnPickForReceipt.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);
            btnPickForCrDrNote.Load += new EventHandler(btnAction_Load);

            //lbnSOListing.Load += new EventHandler(btnAction_Load);
            //lnbDeliveryOrder.Load += new EventHandler(btnAction_Load);
            //lnbSalesInvoice.Load += new EventHandler(btnAction_Load);
            //lnbAdvanceInvoice.Load += new EventHandler(btnAction_Load);
            //lnbMiscellaneous.Load += new EventHandler(btnAction_Load);
            //lbnSalesReceipt.Load += new EventHandler(btnAction_Load);
            //lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //lnbAcPayables.Load += new EventHandler(btnAction_Load);

            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);


            btnAddItem.Load += new EventHandler(btnAction_Load);
            btnClearItem.Load += new EventHandler(btnAction_Load);
            //btnEditItem.Load += new EventHandler(btnAction_Load);
            //btnRemoveItem.Load += new EventHandler(btnAction_Load);
            btnApply.Load += new EventHandler(btnAction_Load);
            imgPopupAdd.Load += new EventHandler(btnAction_Load);
            //imbTaxRemove.Load += new EventHandler(btnAction_Load);

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
                hdfgroup.Value = Convert.ToByte((byte)SalesInvoiceGroup.Miscellaneous).ToString();
                if (SOInvoiceHeaderSession != null)
                {
                    hdfHasTax.Value = ((SOInvoiceHeaderSession.TaxHdr == null || SOInvoiceHeaderSession.TaxHdr.Count == 0)
                        && (SOInvoiceHeaderSession.OrderDetail == null || SOInvoiceHeaderSession.OrderDetail.Count == 0 ||
                        SOInvoiceHeaderSession.OrderDetail.All(dtl => (dtl.TaxDtl == null || dtl.TaxDtl.Count == 0))))
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "TaxPayable", "$(document).ready(function () { ShowHideTaxPayableOuter();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveLink", "$(document).ready(function () { RemoveBalAmntHyperLink();});", true);

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
            //EXPENSEHEADER,
            EXPENSEDETAIL,
            SELECTEDITEM,
            TAXTYPES,
            TAXPOPUPGRID,
            OTHERCHARGELIST,
            TAXHEADER,
            EXCHANGERATE,
            MISCLIST,
            JOURNALIZE,
            FINHEADER,
            PICKFORPAYMENT,
            PICKFORCRDRNOTE,
            GETEXPENSEPKBYJOURNALPK,
            TAXSETTINGS,
            DEFAULTUOM,
            COMPANY,
            SOINVHEADER,
            INVOICETYPE,
            CUSTOMERSELECTED,
            SOTYPE,
            PICKFORRECEIPT,
            PAYMENTTERMS,
            GETDUEDATE,
            FROMPORT,
            CUSTOMERTYPES,
            GETCUSTOMERDETAILSBYTYPE,
            CUSTOMERRELATEDTAX,
            SHOWDIVTAXBC,
            ALERTCONFIG,
            ALERTSAVE,
            ALERTLIST,
            ALERTBASIS,
            NOTIFICATIONDAYS,
            CUSTOMTAXSETTINGS,
            INVOICEGRID,
            ISSUEDETAILSSPLITUP,
            ISSUETYPE,
            ISSUEDETAILSSPLITUPAPPLIED,
            INVOICEGET,
            COMPANYSRCH,
            INVOICEGSTTYPE,
            GSTSUBTYPE,
            AMOUNTDETAILS,SALECATEGORY

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

        #region CustomerContactTypes
        public enum CustomerContactTypeEnum
        {
            HeadOffice = 4,
            Branch = 5
        }
        #endregion
    }
}