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
using BusinessObject;
using System.Web.UI.HtmlControls;
using BusinessObject.AlertManagement;
using BusinessLogic.CommonManagement;

namespace ERPSMS_v01.Sales
{
    public partial class SalesInvoice : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
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
        /// Is Allow All Bizunit Currency
        /// </summary>
        private bool IsBizUnitCur
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsBizUnitCur] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsBizUnitCur]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsBizUnitCur] = value;
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

        private int RPTTYPE
        {
            get
            {
                return this.ViewState["RPTTYPE"] == null ? 1 : Convert.ToInt32(this.ViewState["RPTTYPE"]);
            }
            set
            {
                this.ViewState["RPTTYPE"] = value;
            }
        }
        private int SelectedInvPK
        {
            get
            {
                return this.ViewState["SelectedInvPK"] == null ? 1 : Convert.ToInt32(this.ViewState["SelectedInvPK"]);
            }
            set
            {
                this.ViewState["SelectedInvPK"] = value;
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
        private int InvoiceId
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.InvoiceId];
            }
            set
            {
                this.ViewState[ViewstateStrings.InvoiceId] = value;
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
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedSosForAdvInv
        {
            get
            {
                //return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv];
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInvoice];
            }
            set
            {
                //Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv] = value;
                Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInvoice] = value;
            }

        }
        /// <summary>
        /// To maintain count of selected pos
        /// </summary>
        private int SelectedInvoicesCount
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.SelectedInvoicesCount];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedInvoicesCount] = value;
            }
        }

        /// <summary>
        /// To maintain count of selected invoices
        /// </summary>
        private int SelectedInvoicesCrDrCount
        {
            get
            {
                return (int)this.ViewState[ViewstateStrings.SelectedInvoicesCrDrCount];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedInvoicesCrDrCount] = value;
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
        private List<long> SelectedCurrency
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCurrency];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedCurrency] = value;
            }

        }

        /// <summary>
        /// To maintain keep selected Invoice Type
        /// </summary>
        private List<long> SelectedInvoiceType
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedInvoiceType];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedInvoiceType] = value;
            }

        }

        private List<long> SelectedCustomers
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedCustomers];
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

        /// <summary>
        /// To maintain keep PoHeader List
        /// </summary>
        private List<SAL_ORDER_HDR> SalHeaderList
        {
            get
            {
                return (List<SAL_ORDER_HDR>)Session[ERP.Utilities.SessionStrings.SalHeaderList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SalHeaderList] = value;
            }

        }

        /// <summary>
        /// To maintain keep PoHeader List
        /// </summary>
        private string ProcessidDummy
        {
            get
            {
                //return (string)Session[ERP.Utilities.SessionStrings.ProcessidDummy];
                return ViewState[ERP.Utilities.SessionStrings.ProcessidDummy] == null ? "1" : (string)ViewState[ERP.Utilities.SessionStrings.ProcessidDummy];
            }
            set
            {
                //Session[ERP.Utilities.SessionStrings.ProcessidDummy] = value;
                ViewState[ERP.Utilities.SessionStrings.ProcessidDummy] = value;
            }

        }
        /// <summary>
        /// To maintain keep InvoiceMap List
        /// </summary>
        private List<FIN_INVOICE_CUS_TRX_MPG> InvoiceCusMapList
        {
            get
            {
                return (List<FIN_INVOICE_CUS_TRX_MPG>)Session[ERP.Utilities.SessionStrings.InvoiceCusMapList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.InvoiceCusMapList] = value;
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
        /// Customer Pk
        /// </summary>
        private int CustomerID
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerID] == null ? 0 : (int)this.ViewState[ViewstateStrings.CustomerID];
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerID] = value;
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
        /// Invoice Type
        /// </summary>
        private int InvType
        {
            get
            {
                return this.ViewState[ViewstateStrings.InvType] == null ? 0 : (int)this.ViewState[ViewstateStrings.InvType];
            }
            set
            {
                this.ViewState[ViewstateStrings.InvType] = value;
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
        /// To maintain keep selected Currency
        /// </summary>
        private long Selected_Currency
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.Selected_InvoiceType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Selected_InvoiceType] = value;
            }

        }
        /// <summary>
        /// To maintain keep selected Currency
        /// </summary>
        private long Selected_InvoiceType
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.Selected_Currency]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Selected_Currency] = value;
            }

        }

        private long Selected_Customers
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.Selected_Customers]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Selected_Customers] = value;
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
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;
        private FIN_INVOICE_CUS_TRX_MPG finInvoiceCusTrxMpgObj;
        private SAL_ORDER_HDR SalOrderHdrObj;
        private List<SAL_ORDER_HDR> SalOrderHdrList;
        private List<decimal> SelectedINVTaxList;

        private List<SAL_ORDER_HDR> SalOrderHdrListTaxSplitup;

        //List for binding details to controls  
        private List<FIN_INVOICE_CUS_HDR> finInvoiceCusHdrList;
        private List<FIN_INVOICE_CUS_TRX_MPG> finInvoiceCusTrxMpgList;
        private List<long> SelectedSOListForAdvInv;
        private List<decimal> lstBox;

        private ADM_CONFIG_MST admConfigMstObj;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;
        private List<ADM_CONFIG_MST> admConfigMstList;
        private SAL_ORDER_HDR objSalesOrderHeader;
        private List<SAL_ORDER_HDR> salesOrderHeaderList;

        private ADM_CURRENCY_MST admCurrencyMstObj;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private List<ADM_CONST_MST> admConstMstList;
        private ADM_APP_CONFIG_MST admAppConfigMstObj;
        private List<ADM_APP_CONFIG_MST> admAppConstMstList;
        DataSet dsAlertList;
        DataTable dtSOData;
        private int invPK;
        private string appType;
        private string TypeRef;
        User currentUser;
        DataTable dtCompany = new DataTable();
        DataTable dtInvoiceGstType;
        DataSet dsCustomerTypes;
        DataSet dsCustomerDetailsByType;
        private int CustomerTypeSelectedPk;
        private string CustomerSavedBranchId;
        private string CustomerSavedTaxId;
        private int custPK;
        private long InvoicePk = 0;
        DataTable dtAmountDetails;

        private List<long> SelectedSalesInvoiceList;
        private List<long> SelectedInvoiceCrDrList;
        private List<long> SelectedCurrencyList;
        private List<long> SelectedInvoiceTypeList;
        private List<long> SelectedCustomersList;

        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        DataTable dtTaxSettings;
        private DataTable dtGstSubType;

        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;

        private string ivoiceNo;
        private bool updateInvoice;

        bool isCancelled = false;
        bool ischanged = false;

        int JournalPK;

        private string refID;
        private string inboxFlag;
        private List<FIN_YEAR_MST> finYearMstList;

        private Dictionary<string, decimal> dicTempAmount = new Dictionary<string, decimal>();


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
                ddlPlantName.Visible = lblPlantName.Visible = GetConfigData().IsMultiplePlant;
                hdfIsMultiplePlant.Value = GetConfigData().IsMultiplePlant ? "1" : "0";
                hdfDecimalFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalDigits; i++)
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
                int rateDecimalDigits = (Session[ERP.Utilities.SessionStrings.RateDecimalDigit] == null
                    ? Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits
                    : Convert.ToInt32(Session[ERP.Utilities.SessionStrings.RateDecimalDigit]));
                for (int i = 0; i < rateDecimalDigits; i++)
                {
                    hdfRateFormat.Value += "0";
                }

                hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();

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
                    ConfigurationSettings();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.COMPANYNAME);
                    SetFieldValues(ControlsEnum.COMPANYNAME);
                    GetFieldValues(ControlsEnum.SOTYPE);
                    SetFieldValues(ControlsEnum.SOTYPE);

                    GetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                    SetFieldValues(ControlsEnum.INVOICEGSTTYPE);

                    GetFieldValues(ControlsEnum.GSTSUBTYPE);
                    SetFieldValues(ControlsEnum.GSTSUBTYPE);

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

                    ischanged = false;
                    hdfJournalizeWorkFlow.Value = "0";
                    Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    SelectedInvoicesCrDr = null;
                    SelectedSalesInvoices = null;

                    Department deptCurrency = BusinessLogic.SubDepartmentManagement.SubDepartmentMaster.GetDeptDetailsByID(
                        Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                    Session[BusinessObject.Common.SessionStrings.CurDeptCountry] = deptCurrency.BaseCountry;

                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    //txtFromDate.Text = string.Empty;
                    //hdfFromDate.Value = string.Empty;
                    //txtToDate.Text = string.Empty;
                    //hdfToDate.Value = string.Empty;
                    if (SelectedSosForAdvInv != null && SelectedSosForAdvInv.Count > 0 && IsAdvInvHasTax)
                    {
                        ddlInvoiceType.Enabled = true;
                    }
                    else
                    {
                        ddlInvoiceType.Enabled = false;
                    }

                    //GetFieldValues(ControlsEnum.FINPERIOD);
                    //SetFieldValues(ControlsEnum.FINPERIOD);

                    //hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

                    ////start
                    if (Session[ERP.Utilities.SessionStrings.SALEORDERPK] != null)
                    {
                        CurrSOPK = Convert.ToInt32(Session[ERP.Utilities.SessionStrings.SALEORDERPK]);
                        Session[ERP.Utilities.SessionStrings.SALEORDERPK] = null;
                        ////start
                        EntryStatus = EntryStatus.NEWMODE;
                        ////
                        if (IsAdvInvHasTax)
                            ddlInvoiceType.Enabled = true;
                    }
                    else
                    {
                        ddlInvoiceType.Enabled = false;
                    }

                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    ////
                    //Used for Integration purpose
                    //   FillProcessID(1);
                    int processId = 1;
                    if (CurrSOPK > 0)
                    {
                        GetFieldValues(ControlsEnum.SOINVHEADERBYPK);
                        ProcessidDummy = (processId = salesOrderHeaderList != null ? salesOrderHeaderList[0].SOH_TYPE == 1 ? 3 : processId : processId).ToString();
                    }
                    else
                    {
                        int.TryParse(pid, out processId);
                    }
                    if (ProcessidDummy == "3")
                    { processId = 3; }
                    if (processId == 3)
                        FillProcessID(3);
                    else
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
                        if (!string.IsNullOrEmpty(inboxFlag))
                        {
                            ucrWrkf.ViewType = 0;
                            EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;
                            //btnSubmit.Visible = false;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 1;
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        ////start
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("3") || pid.Equals("11"))
                        {
                            ucrWrkf.RefID = int.Parse(refID);
                            base.WkfRefID = ucrWrkf.RefID;
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            if (pid.Equals("11"))
                                hdfInvDelStatus.Value = "1";//For Showing Cancelled Stamp in Detail Page
                        }
                        else if (pid.Equals("2"))
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
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                        }
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICETYPE);
                        SetFieldValues(ControlsEnum.INVOICETYPE);
                        GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                        hdfIVHPK.Value = CurrPK.ToString();
                        GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                        GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                        SetFieldValues(ControlsEnum.POINVOICELIST);
                    }
                    else
                    {

                        string[] datakeyarray;
                        datakeyarray = new string[1];
                        datakeyarray[0] = Resources.DataFieldRes.SalesInvoicePK;
                        grdSalesInvoiceList.DataKeyNames = datakeyarray;
                        SelectedSosForAdvInv = (List<long>)Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv];
                        Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv] = null;
                        if (SelectedSosForAdvInv != null && SelectedSosForAdvInv.Count > 0)
                        {
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                            {
                                EntryStatus = EntryStatus.NEWMODE;
                                ucrWrkf.ViewType = 1;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                                //btnSave.Visible = false;
                                //btnSubmit.Visible = false;
                            }
                            SelectedSOListForAdvInv = SelectedSosForAdvInv;
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);

                            GetFieldValues(ControlsEnum.POINVOICELIST);
                            SetFieldValues(ControlsEnum.POINVOICELIST);

                            lblDispInvoiceNo.Text = "[NEW]";
                        }
                        else
                        {
                            SelectedSOListForAdvInv = new List<long>();
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICEHDR);
                            SetFieldValues(ControlsEnum.INVOICEHDR);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                    }
                    GetFieldValues(ControlsEnum.CUSTOMERTYPES);
                    SetFieldValues(ControlsEnum.CUSTOMERTYPES);
                    SetBranchIDEnableDisable();

                    #region Fill workflow details with invoice type
                    if (!IsAdvInvHasTax && CurrPK <= 0)
                    {
                        if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue))
                        {
                            if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic))
                                FillProcessID(3);
                            else
                                FillProcessID(1);
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.NEWMODE && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                        }
                    }
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
        #endregion
        #region Get Field Values
        /// <summary>
        /// Gets the data to bind/assign for the controls(inputs, grids, dropdowns etc.)
        /// pass string.empty to get all field values
        /// Assign it to the page level variables
        /// </summary>
        private void GetFieldValues(ControlsEnum type)
        {
            SaleOrderService saleOrderServiceClient;
            saleOrderServiceClient = null;

            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;

            AdmCompanyMstService admCompanyMstServiceClient;

            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            int? Status = null;
            SaleOrderService salesOrderServiceClient;

            try
            {
                switch (type)
                {
                    #region Invoice Hdr List
                    case ControlsEnum.INVOICEHDR:

                        salesInvoiceServiceClient = new SalesInvoiceService();
                        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdSalesInvoiceList.PageSize;
                        if (SortBy == "CUR_CODE")
                        {
                            SortBy = Resources.DataTableRes.CurrencyMst1 + "." + Resources.DataFieldRes.CurrencyCode;
                        }
                        //if (SortBy == "InvType1")
                        //{
                        //    SortBy = Resources.DataTableRes.ConfigMst + "." + Resources.DataFieldRes.cfgValue;
                        //}
                        serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.SalesInvoiceDate : SortBy;
                        serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.SalesInvoiceNo : ThenBy;
                        serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;


                        finInvoiceCusHdrObj.ICH_CUSTOMER = String.IsNullOrEmpty(hdfCustomerID.Value.Trim()) ? 0 : Convert.ToInt32(hdfCustomerID.Value);
                        if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                        }
                        finInvoiceCusHdrObj.ICH_PK = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        finInvoiceCusHdrObj.ICH_ACTIVE = 1;
                        finInvoiceCusHdrObj.ICH_DEL_STATUS = 0;
                        finInvoiceCusHdrObj.ICH_CATEGORY = 2;
                        finInvoiceCusHdrObj.ICH_CRTD_BY = currentUser.PKUser;
                        finInvoiceCusHdrObj.ICH_BIZUNIT = currentUser.SBUID;

                        if (hdfIsMultiplePlant.Value == "1")
                            finInvoiceCusHdrObj.ICH_COMPANY = Convert.ToInt32(ddlPlantName.SelectedValue);

                        //finInvoiceCusHdrObj.ICH_REFERENCE = txtSCno.Text.Trim();
                        serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text.Trim());
                        serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text.Trim());
                        string SIno = null;
                        if (txtSCno.Text != null)
                        { SIno = txtSCno.Text.Trim(); }
                        else { SIno = ""; }

                        Status = int.Parse(ddlStatus.SelectedValue);
                        serviceUtilityObj.InvoiceType = ddlSaleOrderType.SelectedItem.Value == null ? 0 : Convert.ToInt32(ddlSaleOrderType.SelectedItem.Value);
                        //if (ddlInvoiceType.SelectedItem.Value > 0)
                        //{ 
                        //    serviceUtilityObj.InvoiceType=ddlInvoiceType.
                        //}
                        finInvoiceCusHdrList = salesInvoiceServiceClient.GetSalesInvoiceHdr(finInvoiceCusHdrObj, serviceUtilityObj, Status, SIno);


                        //var list = finInvoiceCusHdrList.OrderBy(item => (item.ICH_AMOUNT_NET_TC) - (item.ICH_AMOUNT_RCVD_TC)).ToList();
                        //  lstBox.Items.Clear();
                        //foreach (ListItem listItem in list)
                        //{
                        //    lstBox.Items.Add(listItem);
                        //}



                        TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                                     (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                                     (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;
                        break;
                    #endregion
                    #region Invoice Hdr By PK
                    case ControlsEnum.INVOICEHDRBYPK:
                        salesInvoiceServiceClient = new SalesInvoiceService();
                        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        if (hdfCustomerID.Value != null && hdfCustomerID.Value != "0" && hdfCustomerID.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerID.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;
                        }
                        #region Old Code
                        //finInvoiceCusHdrObj.ICH_PK = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        //finInvoiceCusHdrObj.ICH_ACTIVE = 1;
                        //finInvoiceCusHdrObj.ICH_DEL_STATUS = ddlStatus.SelectedValue != "-1" ? (byte)0 : (byte)1;
                        //finInvoiceCusHdrList = salesInvoiceServiceClient.GetInvoiceCusHdrByPK(finInvoiceCusHdrObj); 
                        #endregion
                        finInvoiceCusHdrObj.ICH_PK = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        finInvoiceCusHdrList = salesInvoiceServiceClient.GetInvoiceCusHdrByPKOnly(finInvoiceCusHdrObj);
                        break;
                    #endregion
                    #region Get sale order List
                    case ControlsEnum.POINVOICELIST:
                        saleOrderServiceClient = new SaleOrderService();
                        saleOrderServiceClient = CommonFunctions.InitiateClient(saleOrderServiceClient);
                        SalOrderHdrObj = new SAL_ORDER_HDR();
                        serviceUtilityObj = new ServiceUtility();
                        SalOrderHdrObj.SOH_ACTIVE = 1;
                        SalOrderHdrList = saleOrderServiceClient.GetSelectedSaleOrders(SelectedSosForAdvInv, serviceUtilityObj);
                        SalHeaderList = SalOrderHdrList;
                        break;
                    #endregion
                    #region PO Invoice Details
                    case ControlsEnum.POINVOICEDETAILS:
                        saleOrderServiceClient = new SaleOrderService();
                        saleOrderServiceClient = CommonFunctions.InitiateClient(saleOrderServiceClient);
                        SalOrderHdrObj = new SAL_ORDER_HDR();
                        serviceUtilityObj = new ServiceUtility();
                        SalOrderHdrObj.SOH_ACTIVE = 1;
                        finInvoiceCusTrxMpgList = saleOrderServiceClient.GetInvoicedSaleOrders(CurrPK);
                        InvoiceCusMapList = finInvoiceCusTrxMpgList;
                        break;
                    #endregion
                    #region Invoice Hdr By PK
                    case ControlsEnum.SOINVHEADERBYPK:
                        salesOrderServiceClient = new SaleOrderService();
                        salesOrderServiceClient = CommonFunctions.InitiateClient(salesOrderServiceClient);
                        objSalesOrderHeader = CommonFunctions.Initilize<SAL_ORDER_HDR>();
                        objSalesOrderHeader.SOH_PK = CurrSOPK;
                        objSalesOrderHeader.SOH_ACTIVE = 1;
                        salesOrderHeaderList = salesOrderServiceClient.GetSaleOrderHdrByPK(objSalesOrderHeader);
                        break;
                    #endregion
                    #region Generate Invoice No
                    case ControlsEnum.INVOICENO:
                        //Generate Invoice No
                        salesInvoiceServiceClient = new SalesInvoiceService();
                        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        int appSubType;
                        if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic))
                        {
                            appSubType = (int)ApplicationSubType.INVOICE;
                        }
                        else if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Export))
                        {
                            appSubType = (int)ApplicationSubType.SALEADVINVOICEEXPORT;
                        }
                        else
                        {
                            appSubType = (int)ApplicationSubType.SALEADVINVOICEPROFORMA;
                        }

                        ivoiceNo = salesInvoiceServiceClient.GetInvoiceNo(ApplicationType.SI, appSubType, currentUser.CurrentDeptPK ,
                            string.IsNullOrEmpty(txtInvdate.Text) ? DateTime.Now : Convert.ToDateTime(txtInvdate.Text), currentUser.PKUser, updateInvoice, 0, Convert.ToInt32(ddlCompany.SelectedValue));
                        hdfInvoiceNo.Value = ivoiceNo;
                        break;
                    #endregion
                    #region Get Exchange Rate
                    case ControlsEnum.EXCHANGERATE:
                        //Get Exchange Rate
                        salesInvoiceServiceClient = new SalesInvoiceService();
                        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        double ExchgRate = salesInvoiceServiceClient.GetConversionFactor(
                                                             finInvoiceCusHdrObj.ICH_CURRENCY, finInvoiceCusHdrObj.ICH_BASE_CURR,
                                                             finInvoiceCusHdrObj.ICH_DATE, (IsBizUnitCur == true ? currentUser.SBUID : 0));
                        hdfExchangeRate.Value = ExchgRate.ToString();
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
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
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
                    #region FILLWORKFLOWSTATUS
                    case ControlsEnum.FILLWORKFLOWSTATUS:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.SI, (byte)AppSubTypeSOInvoice.ADVINVOICE, Convert.ToByte(CommonConstants.ACTIVE));
                        break;
                    #endregion
                    #region INVOICETYPE
                    case ControlsEnum.INVOICETYPE:
                        CommonServiceClient = new CommonService();
                        admConfigMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_CONFIG_MST>();
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        admConfigMstObj.CFG_TYPE = "SALES INVOICE TYPE";
                        admConfigMstObj.CFG_SPL_COND = "Advance";
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
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
                        admAppConfigMstObj.ACF_DATA = ApplicationType.SI;
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

                    #region SALES_INVOICE_TYPE
                    case ControlsEnum.SALES_INVOICE_TYPE:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admConfigMstObj = new ADM_CONFIG_MST();
                        admConfigMstObj.CFG_PK = 0;
                        admConfigMstObj.CFG_TYPE = Resources.Constants.SALES_INVOICE_TYPE;
                        admConfigMstObj.CFG_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admConfigMstList = CommonServiceClient.GetConfigValues(admConfigMstObj);
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
                        //dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    #endregion
                    case ControlsEnum.COMPANYNAME:
                        //gets Company List
                        admCompanyMstServiceClient = new AdmCompanyMstService();
                        admCompanyMstObj = ERP.Utilities.CommonFunctions.Initilize<ADM_COMPANY_MST>();
                        admCompanyMstObj.CMP_ACTIVE = 1;
                        serviceUtilityObj = new ServiceUtility();
                        admCompanyMstList = admCompanyMstServiceClient.GetCompanyList(admCompanyMstObj, serviceUtilityObj);
                        //  dtCompany = BusinessLogic.CommonManagement.CommonBL.GetCompanyDetails(Convert.ToInt32(DbActiveStatus.ACTIVE), currentUser.SBUID, 0);
                        break;
                    case ControlsEnum.SOTYPE:
                        dtSOData = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SALES INVOICE TYPE", "Regular");
                        break;
                    #region Invoice Type GST
                    case ControlsEnum.INVOICEGSTTYPE:
                        int InvoiceTypeValue = ddlInvoiceType.SelectedValue == "" ? Convert.ToInt16(CommonConstants.SELECTVAL) : Convert.ToInt16(ddlInvoiceType.SelectedValue);
                        dtInvoiceGstType = BusinessLogic.CommonManagement.CommonBL.GetInvoiceGstType(Convert.ToInt16(CommonConstants.SELECT_VALUE_ZERO), Convert.ToInt16(DbActiveStatus.ACTIVE), currentUser.SBUID, InvoiceTypeValue, Convert.ToInt16(GTIService.Constants.Common.InvoiceType.Sales));
                        break;
                    #endregion
                    #region CustomerTypes
                    case ControlsEnum.CUSTOMERTYPES:
                        int.TryParse(hdfCurrCustomerPK.Value, out custPK);
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
                    #region AMOUNTDETAILS
                    case ControlsEnum.AMOUNTDETAILS:
                        dtAmountDetails = new DataTable();
                        //salesInvoiceServiceClient = new SalesInvoiceService();
                        //salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        //dtAmountDetails = salesInvoiceServiceClient.GetInvCusReceivedAmntDetails(InvoicePk);
                        dtAmountDetails = BusinessLogic.Sales.SalesInvoiceBL.GetInvCusReceivedAmntDetails(Convert.ToInt16(InvoicePk)).Tables[0];
                        break;
                    #endregion
                    #region GST SUB TYPE
                    case ControlsEnum.GSTSUBTYPE:
                        dtGstSubType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "EXP TYPE");
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
                saleOrderServiceClient = null;
                salesInvoiceServiceClient = null;
                admCompanyMstServiceClient = null;
                finTrxServiceClient = null;
                CommonServiceClient = null;
                salesOrderServiceClient = null;
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
                    #region Company
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    case ControlsEnum.COMPANYNAME:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANYNAME);
                        break;
                    #region Invoice Hdr
                    case ControlsEnum.INVOICEHDR:
                        BindGrid(ControlsEnum.INVOICEHDR);
                        break;
                    #endregion
                    #region PO Invoice List
                    case ControlsEnum.POINVOICELIST:
                        BindGrid(ControlsEnum.POINVOICELIST);
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {
                            txtFromDate.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            hdfFromDate.Value = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            txtToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                            hdfToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                        }
                        break;
                    #endregion
                    #region InvoiceType
                    case ControlsEnum.INVOICETYPE:
                        BindDropDown(ControlsEnum.INVOICETYPE);
                        break;
                    #endregion
                    case ControlsEnum.INVOICEGSTTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.SOTYPE:
                        BindDropDown(controlType);
                        break;

                    case ControlsEnum.CUSTOMERTYPES:
                        BindDropDown(ControlsEnum.CUSTOMERTYPES);
                        break;

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
                                txtTypeID.Text = HttpUtility.HtmlDecode(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE_NAME"].ToString());
                                txtTaxID.Text = HttpUtility.HtmlDecode(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TAX_NO"].ToString());

                                hdfCustomerTypeId.Value = Convert.ToString(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE"]);
                                if (Convert.ToInt32(dsCustomerDetailsByType.Tables[0].Rows[0]["CAD_TYPE"]) == (int)CustomerContactTypeEnum.Branch)
                                {
                                    txtTypeID.Enabled = true;
                                    vrfBranchCode.Enabled = true;
                                    txtTypeID.CssClass = "input-small";
                                }
                                else
                                {
                                    txtTypeID.Enabled = false;
                                    txtTypeID.Text = "00000";
                                    vrfBranchCode.Enabled = false;
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
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break;
                    case ControlsEnum.GSTSUBTYPE:
                        BindDropDown(ControlsEnum.GSTSUBTYPE);
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
                int i = 0;
                bool bIsChecked = false;
                LinkButton lbnBalAmt;
                //Label lblBalAmt;
                decimal balamt;
                balamt = 0;
                AlertBO alertBoObj;
                bool InvalidReceiptItem = false;
                int invType = 1;

                switch (controlType)
                {
                    #region Invoice Header
                    case ControlsEnum.FINANCEINVOICEHDR:
                        finInvoiceCusHdrObj.ICH_PK = CurrPK;
                        finInvoiceCusHdrObj.ICH_NO = (string.IsNullOrEmpty(lblDispInvoiceNo.Text) || lblDispInvoiceNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                    : lblDispInvoiceNo.Text.Trim();
                        //if (Session[BusinessObject.Common.SessionStrings.CurDeptCountry].ToString() == hdfCustomerCountry.Value)
                        //{
                        //    finInvoiceCusHdrObj.ICH_TYPE = 0;
                        //}
                        //else
                        //{
                        //    finInvoiceCusHdrObj.ICH_TYPE = 1;
                        //}
                        finInvoiceCusHdrObj.ICH_TYPE = Convert.ToByte(ddlInvoiceType.SelectedValue);
                        finInvoiceCusHdrObj.ICH_DATE = string.IsNullOrEmpty(txtInvdate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvdate.Text.Trim());
                        finInvoiceCusHdrObj.ICH_REFERENCE = !string.IsNullOrEmpty(hdfInvoiceReference.Value) ? hdfInvoiceReference.Value : string.Empty;
                        if (SalHeaderList != null && SalHeaderList.Count > 0)
                        {
                            SalOrderHdrList = (List<SAL_ORDER_HDR>)SalHeaderList;
                            finInvoiceCusHdrObj.ICH_CUSTOMER = SalOrderHdrList[0].SOH_CUSTOMER;
                            finInvoiceCusHdrObj.ICH_CUSTOMER_ACCOUNT = null;
                            finInvoiceCusHdrObj.ICH_CURRENCY = SalOrderHdrList[0].SOH_CURRENCY.Value;
                            finInvoiceCusHdrObj.ICH_PAYMENT_TERM = SalOrderHdrList[0].SOH_PAYMENT_TERM;
                            if (EntryStatus == EntryStatus.NEWMODE)
                            {
                                for (int j = 0; j < SalOrderHdrList.Count; j++)
                                {
                                    //finInvoiceCusHdrObj.ICH_REFERENCE = string.IsNullOrEmpty(finInvoiceCusHdrObj.ICH_REFERENCE) ?
                                    //    GetLocalResourceObject("Ref1").ToString() + SalOrderHdrList[j].SOH_NO + GetLocalResourceObject("Ref2").ToString() + SalOrderHdrList[j].SOH_REFERENCE + GetLocalResourceObject("Ref3").ToString()
                                    //    : finInvoiceCusHdrObj.ICH_REFERENCE;
                                    string Reference = GetLocalResourceObject("Ref1").ToString() + SalOrderHdrList[j].SOH_NO + GetLocalResourceObject("Ref2").ToString() + SalOrderHdrList[j].SOH_REFERENCE + GetLocalResourceObject("Ref3").ToString();
                                    finInvoiceCusHdrObj.ICH_REFERENCE = string.IsNullOrEmpty(finInvoiceCusHdrObj.ICH_REFERENCE) ? Reference : finInvoiceCusHdrObj.ICH_REFERENCE + ", " + Reference;
                                }
                            }
                        }
                        else if (InvoiceCusMapList != null && InvoiceCusMapList.Count > 0)
                        {
                            finInvoiceCusTrxMpgList = (List<FIN_INVOICE_CUS_TRX_MPG>)InvoiceCusMapList;
                            finInvoiceCusHdrObj.ICH_CUSTOMER = finInvoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            finInvoiceCusHdrObj.ICH_CUSTOMER_ACCOUNT = null;
                            finInvoiceCusHdrObj.ICH_CURRENCY = finInvoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_CURRENCY.Value;
                            finInvoiceCusHdrObj.ICH_REFERENCE = string.IsNullOrEmpty(finInvoiceCusHdrObj.ICH_REFERENCE) ?
                                GetLocalResourceObject("Ref1").ToString() + finInvoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_NO + GetLocalResourceObject("Ref2").ToString() + finInvoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_REFERENCE + GetLocalResourceObject("Ref3").ToString()
                                : finInvoiceCusHdrObj.ICH_REFERENCE;
                        }


                        finInvoiceCusHdrObj.ICH_DATE_PAY_BY = string.IsNullOrEmpty(txtPaybydate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaybydate.Text.Trim());

                        finInvoiceCusHdrObj.ICH_AMOUNT_TC = string.IsNullOrEmpty(txtInvoiceAmt.Text) ? 0 : Convert.ToDecimal(txtInvoiceAmt.Text);
                        finInvoiceCusHdrObj.ICH_DISCOUNT_TC = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
                        finInvoiceCusHdrObj.ICH_TAX_TC = string.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
                        finInvoiceCusHdrObj.ICH_NET_VALUE_TC = finInvoiceCusHdrObj.ICH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
                        finInvoiceCusHdrObj.ICH_AMOUNT_ADV_DED_TC = 0;
                        finInvoiceCusHdrObj.ICH_BASE_CURR = currentUser.BaseCurrency;

                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        finInvoiceCusHdrObj.ICH_EXCHG_RATE = double.Parse(hdfExchangeRate.Value);
                        finInvoiceCusHdrObj.ICH_AMOUNT_NET_BC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(finInvoiceCusHdrObj.ICH_EXCHG_RATE) * Convert.ToDecimal(txtNetAmount.Text);
                        finInvoiceCusHdrObj.ICH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        finInvoiceCusHdrObj.ICH_STATUS = WkfStatus;
                        finInvoiceCusHdrObj.ICH_DEL_STATUS = Convert.ToByte(hdfInvDelStatus.Value);
                        finInvoiceCusHdrObj.ICH_ACTIVE = 1;
                        finInvoiceCusHdrObj.ICH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finInvoiceCusHdrObj.ICH_CRTD_DT = DateTime.Now;
                        finInvoiceCusHdrObj.ICH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finInvoiceCusHdrObj.ICH_MOD_DT = LastModifiedTime;
                        finInvoiceCusHdrObj.ICH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        finInvoiceCusHdrObj.ICH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finInvoiceCusHdrObj.ICH_AMOUNT_RCVD_TC = 0;
                        finInvoiceCusHdrObj.ICH_AMOUNT_DN_TC = 0;
                        finInvoiceCusHdrObj.ICH_AMOUNT_CN_TC = 0;
                        finInvoiceCusHdrObj.ICH_CATEGORY = 2;
                        //finInvoiceCusHdrObj.ICH_TYPE = hdfType.Value == "" ? (byte)0 : (byte)Convert.ToInt16(hdfType.Value);
                        finInvoiceCusHdrObj.ICH_CUSTOMER = Session["SOH_CUSTOMER"] == null ? 0 : Convert.ToInt16(Session["SOH_CUSTOMER"].ToString());
                        finInvoiceCusHdrObj.ICH_CUSTOMER_ADDRESS = Session["SOH_CUSTOMER_ADDRESS"] == null ? "" : Session["SOH_CUSTOMER_ADDRESS"].ToString();
                        if (Session["SOH_CUSTOMER_COUNTRY"] == null)
                        {
                            finInvoiceCusHdrObj.ICH_CUSTOMER_COUNTRY = null;
                        }
                        else
                        {
                            finInvoiceCusHdrObj.ICH_CUSTOMER_COUNTRY = Convert.ToInt16(Session["SOH_CUSTOMER_COUNTRY"].ToString());
                        }
                        finInvoiceCusHdrObj.ICH_CUSTOMER_EMAIL = Session["SOH_CUSTOMER_EMAIL"] == null ? "" : Session["SOH_CUSTOMER_EMAIL"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_FAX = Session["SOH_CUSTOMER_FAX"] == null ? "" : Session["SOH_CUSTOMER_FAX"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_MOBILE = Session["SOH_CUSTOMER_MOBILE"] == null ? "" : Session["SOH_CUSTOMER_MOBILE"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_NAME = Session["SOH_CUSTOMER_NAME"] == null ? "" : Session["SOH_CUSTOMER_NAME"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_PHONE = Session["SOH_CUSTOMER_PHONE"] == null ? "" : Session["SOH_CUSTOMER_PHONE"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_ZIP = Session["SOH_CUSTOMER_ZIP"] == null ? "" : Session["SOH_CUSTOMER_ZIP"].ToString();
                        if (Session["SOH_PAYMENT_TERM"] == null)
                        {
                            finInvoiceCusHdrObj.ICH_PAYMENT_TERM = null;
                        }
                        else
                        {
                            finInvoiceCusHdrObj.ICH_PAYMENT_TERM = Convert.ToInt16(Session["SOH_PAYMENT_TERM"].ToString());
                        }
                        finInvoiceCusHdrObj.ICH_PAYMENT_TERM_TEXT = Session["SOH_PAYMENT_TERM_TEXT"] == null ? "" : Session["SOH_PAYMENT_TERM_TEXT"].ToString();
                        finInvoiceCusHdrObj.ICH_SHIP_CHARGE = Convert.ToDecimal(hdfOCFooter.Value);
                        finInvoiceCusHdrObj.ICH_AMOUNT_ADJUST = 0;
                        finInvoiceCusHdrObj.ICH_DEL_TERM = null;
                        finInvoiceCusHdrObj.ICH_ORG_GOODS = null;
                        //finInvoiceCusHdrObj.ICH_PAYMENT_TERM = null;
                        finInvoiceCusHdrObj.ICH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);


                        //Adding New fields Branch/HeadOffice Pk,Type,Branch id and taxid
                        finInvoiceCusHdrObj.ICH_BRANCH = Convert.ToInt32(ddlCustomerType.SelectedValue);
                        if (Convert.ToInt32(ddlInvoiceGstType.SelectedValue) > 0)
                            finInvoiceCusHdrObj.ICH_GST_TYPE = Convert.ToInt32(ddlInvoiceGstType.SelectedValue);
                        finInvoiceCusHdrObj.ICH_BRANCH_TYPE = Convert.ToByte(hdfCustomerTypeId.Value);//HO/Branch(4/5)
                        finInvoiceCusHdrObj.ICH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtTypeID.Text);
                        finInvoiceCusHdrObj.ICH_TAX_ID = HttpUtility.HtmlEncode(txtTaxID.Text);
                        finInvoiceCusHdrObj.ICH_IS_OPENING = 0;

                        int subtype = 0;
                        int.TryParse(ddlSubType.SelectedValue, out subtype);
                        if (subtype > 0)
                            finInvoiceCusHdrObj.ICH_SUB_TYPE = (byte)subtype;
                        else
                            finInvoiceCusHdrObj.ICH_SUB_TYPE = (byte?)null;

                        finInvoiceCusTrxMpgList = new List<FIN_INVOICE_CUS_TRX_MPG>();
                        finInvoiceCusTrxMpgList = (List<FIN_INVOICE_CUS_TRX_MPG>)SetUIValuesToObject(ControlsEnum.FINANCEINVOICETRXMPG);

                        if (finInvoiceCusTrxMpgList != null && finInvoiceCusTrxMpgList.Count > 0)
                        {
                            finInvoiceCusTrxMpgList.ForEach(dtl => finInvoiceCusHdrObj.FIN_INVOICE_CUS_TRX_MPG.Add(dtl));
                        }
                        retObject = finInvoiceCusHdrObj;

                        break;
                    #endregion

                    #region Invoice Trx Mpg
                    case ControlsEnum.FINANCEINVOICETRXMPG:
                        int rowID = 0;
                        HiddenField hdfSONumber;
                        TextBox txtPayNow;
                        TextBox txtOthercharges;
                        HiddenField hdfSOTax;
                        HiddenField hdfSODiscount;
                        HiddenField hdfAdjustPerInvAmt;

                        foreach (GridViewRow grdrow in grdSalesList.Rows)
                        {
                            txtOthercharges = (TextBox)grdSalesList.Rows[rowID].FindControl("txtOthercharges");
                            hdfSOTax = (HiddenField)grdSalesList.Rows[rowID].FindControl("hdfSOTax");
                            hdfSODiscount = (HiddenField)grdSalesList.Rows[rowID].FindControl("hdfSODiscount");
                            hdfAdjustPerInvAmt = (HiddenField)grdSalesList.Rows[rowID].FindControl("hdfAdjustPerInvAmt");

                            finInvoiceCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_TRX_MPG>();
                            hdfSONumber = (HiddenField)grdSalesList.Rows[rowID].FindControl(GetLocalResourceObject("hdfSONumber").ToString());
                            finInvoiceCusTrxMpgObj.ICM_PK = CurrMpgPK;
                            finInvoiceCusTrxMpgObj.ICM_INVOICE_HDR = CurrPK;
                            finInvoiceCusTrxMpgObj.ICM_SO_HDR = hdfSONumber == null ? 0 : Convert.ToInt32(hdfSONumber.Value);
                            txtPayNow = (TextBox)grdSalesList.Rows[rowID].FindControl(GetLocalResourceObject("txtPayNow").ToString());
                            finInvoiceCusTrxMpgObj.ICM_AMOUNT = txtPayNow == null ? 0 : txtPayNow.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtPayNow.Text.Trim());
                            finInvoiceCusTrxMpgObj.ICM_ACTIVE = 1;

                            finInvoiceCusTrxMpgObj.ICM_OTHER_AMOUNT = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? txtOthercharges == null ? 0 : Convert.ToDecimal(txtOthercharges.Text) : 0;
                            finInvoiceCusTrxMpgObj.ICM_TAX_AMOUNT = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? hdfSOTax == null ? 0 : Convert.ToDecimal(hdfSOTax.Value) : 0;
                            finInvoiceCusTrxMpgObj.ICM_DISCOUNT_AMOUNT = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? hdfSODiscount == null ? 0 : Convert.ToDecimal(hdfSODiscount.Value) : 0;
                            finInvoiceCusTrxMpgObj.ICM_ADJUST_AMOUNT = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? hdfAdjustPerInvAmt == null ? 0 : Convert.ToDecimal(hdfAdjustPerInvAmt.Value) : 0;
                            finInvoiceCusTrxMpgList.Add(finInvoiceCusTrxMpgObj);
                            rowID++;
                        }
                        retObject = finInvoiceCusTrxMpgList;

                        break;
                    #endregion

                    #region Pick Invoice for Receipt
                    case ControlsEnum.PICKFORRECEIPT:

                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            CheckBox chkInvselect;
                            HiddenField hdfDept;
                            int dept;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            lbnBalAmt = grdrow.FindControl("lbnBalAmt") as LinkButton;
                            //lblBalAmt = grdrow.FindControl("lblBalAmt") as Label;
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                CustomerID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                Currency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                InvType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                isCancelled = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                                INVTax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)grdrow.FindControl("hdfTaxAmount")).Value) ? "0" : ((HiddenField)grdrow.FindControl("hdfTaxAmount")).Value);
                                balamt = Convert.ToDecimal(lbnBalAmt.Text);
                                //balamt = Convert.ToDecimal(lblBalAmt.Text);

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                //RPTTYPE = InvType;
                                //if (InvType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                //    FillProcessID(3);
                                //else
                                //    FillProcessID(1);
                                //WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                //base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID(CurrPK, ucrWrkf.ProcessID);                                
                                ////


                                if (!isCancelled)
                                {
                                    if (Approved == 2)// && Posted == true)
                                    {
                                        if ((balamt > 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                                        {
                                            // Iscont = false;
                                            if (!IsSameCurrency(SelectedCurrency, Currency))
                                            {
                                                InvalidReceiptItem = true;
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                            //else if (!IsMatcingTax(SelectedINVTax, INVTax))
                                            //{
                                            //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Tax").ToString();
                                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            //}
                                            else if (!IsSameCustomer(SelectedCustomers, CustomerID))
                                            {
                                                InvalidReceiptItem = true;
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                            else if (!IsSameInvTypes(SelectedInvoiceType, InvType))
                                            {
                                                InvalidReceiptItem = true;
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_InvTypes").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                            else if (!IsExixtPk(SelectedSalesInvoices, CurrPK))
                                            {
                                                //Add Tax
                                                if (SelectedINVTax != null)
                                                {
                                                    SelectedINVTaxList = SelectedINVTax;
                                                }
                                                else
                                                {
                                                    SelectedINVTaxList = new List<decimal>();
                                                }
                                                SelectedINVTaxList.Add(INVTax);
                                                SelectedINVTax = SelectedINVTaxList;
                                                //Add Currencies
                                                if (SelectedCurrency != null)
                                                {
                                                    SelectedCurrencyList = SelectedCurrency;
                                                }
                                                else
                                                {
                                                    SelectedCurrencyList = new List<long>();
                                                }
                                                SelectedCurrencyList.Add(Currency);
                                                SelectedCurrency = SelectedCurrencyList;

                                                //Add Customers
                                                if (SelectedCustomers != null)
                                                {
                                                    SelectedCustomersList = SelectedCustomers;
                                                }
                                                else
                                                {
                                                    SelectedCustomersList = new List<long>();
                                                }
                                                SelectedCustomersList.Add(CustomerID);
                                                SelectedCustomers = SelectedCustomersList;

                                                //Add Inv types
                                                if (SelectedInvoiceType != null)
                                                {
                                                    SelectedInvoiceTypeList = SelectedInvoiceType;
                                                }
                                                else
                                                {
                                                    SelectedInvoiceTypeList = new List<long>();
                                                }
                                                SelectedInvoiceTypeList.Add(InvType);
                                                SelectedInvoiceType = SelectedInvoiceTypeList;

                                                //Add Invoices
                                                if (SelectedSalesInvoices != null)
                                                {
                                                    SelectedSalesInvoiceList = SelectedSalesInvoices;
                                                }
                                                else
                                                {
                                                    SelectedSalesInvoiceList = new List<long>();
                                                }
                                                SelectedSalesInvoiceList.Add(CurrPK);
                                                SelectedSalesInvoices = SelectedSalesInvoiceList;
                                                SelectedInvoicesCount = SelectedSalesInvoices.Count;
                                                //btnPickForReceipt.Text = Resources.Controls.PickPoForInvoicing;
                                                //btnPickForReceipt.Text = GetLocalResourceObject("PickInvforReceipt").ToString() + "(" + SelectedInvoicesCount.ToString() + ")";

                                                //For Saving Selected Item PK
                                                hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + CurrPK.ToString();

                                            }
                                            else
                                            {
                                                InvalidReceiptItem = true;
                                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            InvalidReceiptItem = true;
                                            Iscont = true;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowAlreadyPaid();});", true);
                                            break;
                                        }

                                    }
                                    else
                                    {
                                        if (Approved != 2)
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("MsgApproveforReceipt").ToString();
                                            CurrPK = 0;
                                        }
                                        //else if (Posted == false)
                                        //{
                                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_PickPaymentPost_Msg").ToString();
                                        //}
                                        InvalidReceiptItem = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                }
                                else
                                {
                                    InvalidReceiptItem = true;
                                    litErrorMsg.Text = GetLocalResourceObject("MsgPickCancelledInv").ToString();//Msg_PickPaymentCancelled_Msg
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }

                            }
                        }

                        if (SelectedSalesInvoices != null)
                        {
                            if (SelectedSalesInvoices.Count > 0)
                            {
                                if (!InvalidReceiptItem)
                                {
                                    //btnPickForReceipt.Text = Resources.Controls.PickPoForInvoicing;
                                    btnPickForReceipt.Text = GetLocalResourceObject("PickInvforReceipt").ToString() + "(" + SelectedInvoicesCount.ToString() + ")";
                                    SelectedSosForAdvInv = null;
                                    Response.Redirect(Resources.PageURL.SalesReceipt);
                                }
                                else
                                {
                                    ResetForm(ControlsEnum.RESETRECEIPT);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        //else
                        //{
                        //    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}

                        //if (bIsChecked)
                        //{ 
                        //    //For Setting/Resetting Colour of a selected InvoiceNo
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                        //    //End
                        //}
                        //else
                        //{
                        //    litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //}

                        break;
                    #endregion

                    #region Pick Inv & for Cr/Dr. Note(old commented)
                    //case ControlsEnum.PICKFORCRDRNOTE:

                    //    foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                    //    {
                    //        RadioButton rbtn;
                    //        rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                    //        if (rbtn.Checked)
                    //        {
                    //            bIsChecked = true;
                    //            InvoiceId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                    //            Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                    //            //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                    //            HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                    //            if (hdfPosted != null)
                    //                Posted = Convert.ToBoolean(hdfPosted.Value);
                    //            CustomerID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                    //            Currency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                    //            InvType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                    //            isCancelled = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                    //            break;
                    //        }
                    //    }
                    //    if (bIsChecked)
                    //    {
                    //        if (!isCancelled)
                    //        {
                    //            if (Approved == 2)
                    //            {
                    //                if (!IsSameCurrency(SelectedCurrency, Currency))
                    //                {
                    //                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                    //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //                }
                    //                else if (!IsSameCustomer(SelectedCustomers, CustomerID))
                    //                {
                    //                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Customer").ToString();
                    //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //                }
                    //                else if (!IsSameInvTypes(SelectedInvoiceType, InvType))
                    //                {
                    //                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_InvTypes").ToString();
                    //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //                }
                    //                else if (!IsExixtPk(SelectedInvoicesCrDr, InvoiceId))
                    //                {
                    //                    //Add Currencies
                    //                    if (SelectedCurrency != null)
                    //                    {
                    //                        SelectedCurrencyList = SelectedCurrency;
                    //                    }
                    //                    else
                    //                    {
                    //                        SelectedCurrencyList = new List<long>();
                    //                    }
                    //                    SelectedCurrencyList.Add(Currency);
                    //                    SelectedCurrency = SelectedCurrencyList;

                    //                    //Add Customers
                    //                    if (SelectedCustomers != null)
                    //                    {
                    //                        SelectedCustomersList = SelectedCustomers;
                    //                    }
                    //                    else
                    //                    {
                    //                        SelectedCustomersList = new List<long>();
                    //                    }
                    //                    SelectedCustomersList.Add(CustomerID);
                    //                    SelectedCustomers = SelectedCustomersList;

                    //                    //Add Invoices
                    //                    if (SelectedInvoicesCrDr != null)
                    //                    {
                    //                        SelectedInvoiceCrDrList = SelectedInvoicesCrDr;
                    //                    }
                    //                    else
                    //                    {
                    //                        SelectedInvoiceCrDrList = new List<long>();
                    //                    }
                    //                    SelectedInvoiceCrDrList.Add(InvoiceId);
                    //                    SelectedInvoicesCrDr = SelectedInvoiceCrDrList;
                    //                    SelectedInvoicesCrDrCount = SelectedInvoicesCrDr.Count;

                    //                    //Add Inv types
                    //                    if (SelectedInvoiceType != null)
                    //                    {
                    //                        SelectedInvoiceTypeList = SelectedInvoiceType;
                    //                    }
                    //                    else
                    //                    {
                    //                        SelectedInvoiceTypeList = new List<long>();
                    //                    }
                    //                    SelectedInvoiceTypeList.Add(InvType);
                    //                    SelectedInvoiceType = SelectedInvoiceTypeList;

                    //                    btnPickForCrDrNote.Text = Resources.Controls.PickPoForCrDr;
                    //                    btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString() + "(" + SelectedInvoicesCrDrCount.ToString() + ")";
                    //                    Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.SI;
                    //                }
                    //                else
                    //                {
                    //                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                    //                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                    //                }
                    //            }
                    //            else
                    //            {
                    //                litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                    //                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            litErrorMsg.Text = GetLocalResourceObject("Msg_PickPaymentCancelled_Msg").ToString();
                    //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                    //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                    //    }
                    //    break;
                    #endregion

                    #region Pick Inv & for Cr/Dr. Note
                    case ControlsEnum.PICKFORCRDRNOTE:
                        Selected_Currency = 0;
                        Selected_InvoiceType = 0;
                        Selected_Customers = 0;
                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            CheckBox chkInvselect;
                            HiddenField hdfDept;
                            int dept;
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                InvoiceId = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //isPosted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                CustomerID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value);
                                Currency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSOCurrency")).Value);
                                InvType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                isCancelled = Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                                //InvoiceValue = Convert.ToDecimal(((Label)grdrow.FindControl("lblInvoiceValue")).Text.Replace(",", ""));

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }


                                if (!isCancelled)
                                {
                                    if (Approved == 2)
                                    {
                                        if (Selected_Currency == 0)
                                            Selected_Currency = Currency;
                                        if (Selected_InvoiceType == 0)
                                            Selected_InvoiceType = InvType;
                                        if (Selected_Customers == 0)
                                            Selected_Customers = CustomerID;

                                        //if (InvType != (int)SalesInvoiceType.Domestic)
                                        //{
                                        //    InvalidReceiptItem = true;
                                        //    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_InvType_CNDN").ToString();
                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        //    break;
                                        //}
                                        if (Selected_Currency != Currency)
                                        {
                                            InvalidReceiptItem = true;
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency_CNDN").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                        else if (Selected_Customers != CustomerID)
                                        {
                                            InvalidReceiptItem = true;
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Cust_CNDN").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                        else if (Selected_InvoiceType != InvType)
                                        {
                                            InvalidReceiptItem = true;
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_InvoiceTypes_CNDN").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                        else if (IsReceiptNotCreated(InvoiceId))
                                        {
                                            InvalidReceiptItem = true;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Receipt_NotCreated").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        else if (!IsExixtPk(SelectedInvoicesCrDr, InvoiceId))
                                        {
                                            //Add Invoices
                                            if (SelectedInvoicesCrDr == null)
                                                SelectedInvoicesCrDr = new List<long>();
                                            SelectedInvoicesCrDr.Add(InvoiceId);
                                        }
                                        else
                                        {
                                            InvalidReceiptItem = true;
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        InvalidReceiptItem = true;
                                        litErrorMsg.Text = GetLocalResourceObject("MsgApproveforCNDN").ToString();//Msg_PickPayment_Msg
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                }
                                else
                                {
                                    InvalidReceiptItem = true;
                                    litErrorMsg.Text = GetLocalResourceObject("MsgPickCancelledInvForCNDN").ToString();//Msg_PickPaymentCancelled_Msg
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }

                            }
                        }
                        if (SelectedInvoicesCrDr != null)
                        {
                            if (SelectedInvoicesCrDr.Count > 0)
                            {
                                if (!InvalidReceiptItem)
                                {
                                    //btnPickForCrDrNote.Text = Resources.Controls.PickPoForCrDr;
                                    btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString() + "(" + SelectedInvoicesCrDr.Count().ToString() + ")";
                                    Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.SI;
                                    Response.Redirect(Resources.PageURL.DrCrNoteSales, false);
                                }
                                else
                                {
                                    ResetForm(ControlsEnum.RESETRECEIPT);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        break;
                    #endregion

                    #region WRKFSUBMIT
                    case ControlsEnum.WRKFSUBMIT:
                        finInvoiceCusHdrObj.ICH_PK = CurrPK;
                        updateInvoice = true;
                        if (string.IsNullOrEmpty(lblDispInvoiceNo.Text.Trim()) || lblDispInvoiceNo.Text.Trim().ToLower().Equals("[NEW]".ToLower()))
                        {
                            GetFieldValues(ControlsEnum.INVOICENO);
                            lblDispInvoiceNo.Text = hdfInvoiceNo.Value;
                        }
                        finInvoiceCusHdrObj.ICH_NO = lblDispInvoiceNo.Text;
                        finInvoiceCusHdrObj.ICH_DATE = string.IsNullOrEmpty(txtInvdate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvdate.Text.Trim());
                        finInvoiceCusHdrObj.ICH_REFERENCE = !string.IsNullOrEmpty(hdfInvoiceReference.Value) ? hdfInvoiceReference.Value : string.Empty;
                        if (SalHeaderList != null && SalHeaderList.Count > 0)
                        {
                            SalOrderHdrList = (List<SAL_ORDER_HDR>)SalHeaderList;
                            finInvoiceCusHdrObj.ICH_CUSTOMER = SalOrderHdrList[0].SOH_CUSTOMER;
                            finInvoiceCusHdrObj.ICH_CUSTOMER_ACCOUNT = null;
                            finInvoiceCusHdrObj.ICH_CURRENCY = SalOrderHdrList[0].SOH_CURRENCY.Value;
                            if (EntryStatus == EntryStatus.NEWMODE)
                            {
                                for (int j = 0; j < SalOrderHdrList.Count; j++)
                                {
                                    //finInvoiceCusHdrObj.ICH_REFERENCE = string.IsNullOrEmpty(finInvoiceCusHdrObj.ICH_REFERENCE) ?
                                    //    GetLocalResourceObject("Ref1").ToString() + SalOrderHdrList[0].SOH_NO + GetLocalResourceObject("Ref2").ToString() + SalOrderHdrList[0].SOH_REFERENCE + GetLocalResourceObject("Ref3").ToString()
                                    //    : finInvoiceCusHdrObj.ICH_REFERENCE;
                                    string Reference = GetLocalResourceObject("Ref1").ToString() + SalOrderHdrList[j].SOH_NO + GetLocalResourceObject("Ref2").ToString() + SalOrderHdrList[j].SOH_REFERENCE + GetLocalResourceObject("Ref3").ToString();
                                    finInvoiceCusHdrObj.ICH_REFERENCE = string.IsNullOrEmpty(finInvoiceCusHdrObj.ICH_REFERENCE) ? Reference : finInvoiceCusHdrObj.ICH_REFERENCE + ", " + Reference;
                                }
                            }
                        }
                        else if (InvoiceCusMapList != null && InvoiceCusMapList.Count > 0)
                        {
                            finInvoiceCusTrxMpgList = (List<FIN_INVOICE_CUS_TRX_MPG>)InvoiceCusMapList;
                            finInvoiceCusHdrObj.ICH_CUSTOMER = finInvoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_CUSTOMER;
                            finInvoiceCusHdrObj.ICH_CUSTOMER_ACCOUNT = null;
                            finInvoiceCusHdrObj.ICH_CURRENCY = finInvoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_CURRENCY.Value;
                            finInvoiceCusHdrObj.ICH_REFERENCE = string.IsNullOrEmpty(finInvoiceCusHdrObj.ICH_REFERENCE) ?
                                GetLocalResourceObject("Ref1").ToString() + finInvoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_NO + GetLocalResourceObject("Ref2").ToString() + finInvoiceCusTrxMpgList[0].SAL_ORDER_HDR.SOH_REFERENCE + GetLocalResourceObject("Ref3").ToString()
                                : finInvoiceCusHdrObj.ICH_REFERENCE;
                        }

                        finInvoiceCusHdrObj.ICH_DATE_PAY_BY = string.IsNullOrEmpty(txtPaybydate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaybydate.Text.Trim());

                        finInvoiceCusHdrObj.ICH_AMOUNT_TC = string.IsNullOrEmpty(txtInvoiceAmt.Text) ? 0 : Convert.ToDecimal(txtInvoiceAmt.Text);
                        finInvoiceCusHdrObj.ICH_DISCOUNT_TC = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
                        finInvoiceCusHdrObj.ICH_TAX_TC = string.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
                        finInvoiceCusHdrObj.ICH_NET_VALUE_TC = finInvoiceCusHdrObj.ICH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
                        finInvoiceCusHdrObj.ICH_AMOUNT_ADV_DED_TC = 0;
                        finInvoiceCusHdrObj.ICH_BASE_CURR = currentUser.BaseCurrency;

                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        if (currentUser.BaseCurrency == finInvoiceCusHdrObj.ICH_CURRENCY)
                        {

                        }
                        finInvoiceCusHdrObj.ICH_EXCHG_RATE = double.Parse(hdfExchangeRate.Value);
                        finInvoiceCusHdrObj.ICH_AMOUNT_NET_BC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(finInvoiceCusHdrObj.ICH_EXCHG_RATE) * Convert.ToDecimal(txtNetAmount.Text);
                        finInvoiceCusHdrObj.ICH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text);
                        finInvoiceCusHdrObj.ICH_STATUS = WkfStatus;
                        finInvoiceCusHdrObj.ICH_DEL_STATUS = Convert.ToByte(hdfInvDelStatus.Value);
                        finInvoiceCusHdrObj.ICH_ACTIVE = 1;
                        finInvoiceCusHdrObj.ICH_CRTD_BY = Convert.ToInt16(currentUser.PKUser);
                        finInvoiceCusHdrObj.ICH_CRTD_DT = DateTime.Now;
                        finInvoiceCusHdrObj.ICH_MOD_BY = Convert.ToInt16(currentUser.PKUser);
                        finInvoiceCusHdrObj.ICH_MOD_DT = LastModifiedTime;
                        finInvoiceCusHdrObj.ICH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                        finInvoiceCusHdrObj.ICH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                        finInvoiceCusHdrObj.ICH_AMOUNT_RCVD_TC = 0;
                        finInvoiceCusHdrObj.ICH_AMOUNT_DN_TC = 0;
                        finInvoiceCusHdrObj.ICH_AMOUNT_CN_TC = 0;
                        finInvoiceCusHdrObj.ICH_CATEGORY = 2;
                        //finInvoiceCusHdrObj.ICH_TYPE = (byte)Convert.ToInt16(hdfType.Value);
                        finInvoiceCusHdrObj.ICH_TYPE = Convert.ToByte(ddlInvoiceType.SelectedValue);
                        finInvoiceCusHdrObj.ICH_CUSTOMER = Session["SOH_CUSTOMER"] == null ? 0 : Convert.ToInt16(Session["SOH_CUSTOMER"].ToString());
                        finInvoiceCusHdrObj.ICH_CUSTOMER_ADDRESS = Session["SOH_CUSTOMER_ADDRESS"] == null ? "" : Session["SOH_CUSTOMER_ADDRESS"].ToString();
                        if (Session["SOH_CUSTOMER_COUNTRY"] == null)
                        {
                            finInvoiceCusHdrObj.ICH_CUSTOMER_COUNTRY = null;
                        }
                        else
                        {
                            finInvoiceCusHdrObj.ICH_CUSTOMER_COUNTRY = Convert.ToInt16(Session["SOH_CUSTOMER_COUNTRY"].ToString());
                        }
                        finInvoiceCusHdrObj.ICH_CUSTOMER_EMAIL = Session["SOH_CUSTOMER_EMAIL"] == null ? "" : Session["SOH_CUSTOMER_EMAIL"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_FAX = Session["SOH_CUSTOMER_FAX"] == null ? "" : Session["SOH_CUSTOMER_FAX"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_MOBILE = Session["SOH_CUSTOMER_MOBILE"] == null ? "" : Session["SOH_CUSTOMER_MOBILE"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_NAME = Session["SOH_CUSTOMER_NAME"] == null ? "" : Session["SOH_CUSTOMER_NAME"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_PHONE = Session["SOH_CUSTOMER_PHONE"] == null ? "" : Session["SOH_CUSTOMER_PHONE"].ToString();
                        finInvoiceCusHdrObj.ICH_CUSTOMER_ZIP = Session["SOH_CUSTOMER_ZIP"] == null ? "" : Session["SOH_CUSTOMER_ZIP"].ToString();
                        if (Session["SOH_PAYMENT_TERM"] == null)
                        {
                            finInvoiceCusHdrObj.ICH_PAYMENT_TERM = null;
                        }
                        else
                        {
                            finInvoiceCusHdrObj.ICH_PAYMENT_TERM = Convert.ToInt16(Session["SOH_PAYMENT_TERM"].ToString());
                        }
                        finInvoiceCusHdrObj.ICH_PAYMENT_TERM_TEXT = Session["SOH_PAYMENT_TERM_TEXT"] == null ? "" : Session["SOH_PAYMENT_TERM_TEXT"].ToString();
                        finInvoiceCusHdrObj.ICH_SHIP_CHARGE = Convert.ToDecimal(hdfOCFooter.Value);
                        finInvoiceCusHdrObj.ICH_AMOUNT_ADJUST = 0;
                        finInvoiceCusHdrObj.ICH_DEL_TERM = null;
                        finInvoiceCusHdrObj.ICH_ORG_GOODS = null;
                        finInvoiceCusHdrObj.ICH_PAYMENT_TERM = null;
                        finInvoiceCusHdrObj.ICH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);

                        //Adding New fields Type,Branch id and taxid
                        finInvoiceCusHdrObj.ICH_BRANCH = Convert.ToInt32(ddlCustomerType.SelectedValue);
                        if (Convert.ToInt32(ddlInvoiceGstType.SelectedValue) > 0)
                            finInvoiceCusHdrObj.ICH_GST_TYPE = Convert.ToInt32(ddlInvoiceGstType.SelectedValue);
                        finInvoiceCusHdrObj.ICH_BRANCH_TYPE = Convert.ToByte(hdfCustomerTypeId.Value);//HO/Branch(4/5)
                        finInvoiceCusHdrObj.ICH_BRANCH_TEXT = HttpUtility.HtmlEncode(txtTypeID.Text);
                        finInvoiceCusHdrObj.ICH_TAX_ID = HttpUtility.HtmlEncode(txtTaxID.Text);
                        finInvoiceCusHdrObj.ICH_IS_OPENING = 0;

                        finInvoiceCusTrxMpgList = new List<FIN_INVOICE_CUS_TRX_MPG>();
                        finInvoiceCusTrxMpgList = (List<FIN_INVOICE_CUS_TRX_MPG>)SetUIValuesToObject(ControlsEnum.FINANCEINVOICETRXMPG);

                        if (finInvoiceCusTrxMpgList != null && finInvoiceCusTrxMpgList.Count > 0)
                        {
                            finInvoiceCusTrxMpgList.ForEach(dtl => finInvoiceCusHdrObj.FIN_INVOICE_CUS_TRX_MPG.Add(dtl));
                        }
                        retObject = finInvoiceCusHdrObj;

                        break;
                    #endregion

                    #region Journalize
                    case ControlsEnum.JOURNALIZE:

                        ////Start
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            ////

                            foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                            {
                                CheckBox chkInvselect;
                                chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                                if (chkInvselect.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                    HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                    if (hdfPosted != null)
                                        Posted = Convert.ToBoolean(hdfPosted.Value);
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;

                                    invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                    RPTTYPE = invType;
                                    if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                        FillProcessID(3);
                                    else
                                        FillProcessID(1);

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
                                Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeDDr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeCCr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeCr] = null;
                                Session[ERP.Utilities.SessionStrings.SubTypeDr] = null;
                                Session[ERP.Utilities.SessionStrings.JournalMode] = null;
                                Session[ERP.Utilities.SessionStrings.TrxPK] = null;
                                //Journalize New sessions start
                                Session[ERP.Utilities.SessionStrings.DrControls] = null;
                                Session[ERP.Utilities.SessionStrings.CrControls] = null;
                                Session[ERP.Utilities.SessionStrings.RemovedControls] = null;
                                Session[ERP.Utilities.SessionStrings.AccountType] = null;
                                Session[ERP.Utilities.SessionStrings.ControlInfo] = null;
                                Session[ERP.Utilities.SessionStrings.FinTrxPk] = null;
                                //Journalize New sessions End
                                Invoice = ApplicationType.SIJ;
                                ucrJournalize.TransactionType = Invoice;
                                Session[ERP.Utilities.SessionStrings.TransactionType] = Invoice;
                                ucrJournalize.TransactionPK = CurrPK;
                                Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                ucrJournalize.JournalizePK = 0;
                                Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                Session[ERP.Utilities.SessionStrings.TransactionNo] = finInvoiceCusHdrList[0].ICH_NO;
                                Session[ERP.Utilities.SessionStrings.TransactionDate] = finInvoiceCusHdrList[0].ICH_DATE;
                                Session[ERP.Utilities.SessionStrings.TransactionCurrency] = finInvoiceCusHdrList[0].ICH_CURRENCY;
                                Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                Session[ERP.Utilities.SessionStrings.AccountPayablePK] = finInvoiceCusHdrList[0].ICH_CUSTOMER;
                                Session[ERP.Utilities.SessionStrings.JournalType] = ApplicationType.SIJ;
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
                                Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Sales_Invoice_Journal").ToString();

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
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region ALERT
                    case ControlsEnum.ALERTSAVE:
                        string Typename = Resources.Constants.SystemAlertType;
                        int AlertPk = 0;
                        alertBoObj = new AlertBO();
                        appType = ApplicationType.SI;
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
                        alertBoObj.ATH_TRX_DATE = txtInvdate.Text.Trim() == string.Empty ? DateTime.Now : Convert.ToDateTime(txtInvdate.Text.Trim());
                        string duedays = (Math.Floor((DateTime.Now - alertBoObj.ATH_TRX_DATE).TotalDays)).ToString();
                        alertBoObj.ATH_DUE_DAYS = Convert.ToInt16(duedays);
                        alertBoObj.ATH_DUE_DATE = DateTime.Now;
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
                        if (!string.IsNullOrEmpty(TypeRef) && !TypeRef.Equals("[New]".ToLower()))
                        {
                            alertBoObj.ATH_NARRATION = TypeRef.Trim() + " - " + lblDispCustomerName.ToolTip;
                            //if (!string.IsNullOrEmpty(TypePartyName))
                            //{
                            //    alertBoObj.ATH_NARRATION = !string.IsNullOrEmpty(alertBoObj.ATH_NARRATION) ? alertBoObj.ATH_NARRATION + " - " + TypePartyName :
                            //        TypePartyName;
                            //}
                        }
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
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        private bool IsReceiptNotCreated(long InvPk)
        {
            SalesInvoiceService salesInvoiceServiceClient = new SalesInvoiceService();
            return salesInvoiceServiceClient.IsReceiptNotCreated(InvPk);
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
        /// Is Matcing Tax
        /// </summary>
        /// <param name="TaxList"></param>
        /// <param name="tax"></param>
        /// <returns></returns>
        private bool IsMatcingTax(List<decimal> TaxList, decimal tax)
        {

            bool flag = true;
            bool BaseType = true;
            bool CurType = true;
            if (TaxList != null)
            {
                BaseType = tax > 0 ? true : false;
                foreach (long var in TaxList)
                {
                    CurType = var > 0 ? true : false;
                    if (BaseType != CurType)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            return flag;
        }
        /// <summary>
        /// Is Same Customer
        /// </summary>
        /// <param name="Customers"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameCustomer(List<long> Customers, long pk)
        {

            bool flag = true;
            if (Customers != null)
                foreach (long ven in Customers)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }

        /// <summary>
        /// Is Same InvoiceTypes
        /// </summary>
        /// <param name="Customers"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameInvTypes(List<long> InvTypes, long pk)
        {

            bool flag = true;
            if (InvTypes != null)
                foreach (long ven in InvTypes)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }

        /// <summary>
        /// Is Same Currency
        /// </summary>
        /// <param name="Currencies"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameCurrency(List<long> Currencies, long pk)
        {

            bool flag = true;
            if (Currencies != null)
                foreach (long ven in Currencies)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }
        /// <summary>
        /// Is Same Vendor
        /// </summary>
        /// <param name="vendors"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameInvoice(List<long> invoices, long pk)
        {

            bool flag = true;
            if (invoices != null)
                foreach (long ven in invoices)
                    if (ven != pk)
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

        /// <summary>
        /// Sets the UI input controls from the object values
        /// </summary>
        private void GetUIValuesFromObject(ControlsEnum controlType)
        {
            try
            {

                switch (controlType)
                {
                    #region Invoice Header
                    case ControlsEnum.POINVOICEDETAILS:
                        if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                        {
                            CurrPK = int.Parse(finInvoiceCusHdrList[0].ICH_PK.ToString());
                            lblDispInvoiceNo.Text = finInvoiceCusHdrList[0].ICH_NO == "" ? "[NEW]" : finInvoiceCusHdrList[0].ICH_NO;
                            lblDispCustomerName.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusHdrList[0].CRM_CUSTOMER_MST.CUS_NAME, 46);
                            lblDispCustomerName.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusHdrList[0].CRM_CUSTOMER_MST.CUS_NAME, 327);
                            hdfCustomerCountry.Value = finInvoiceCusHdrList[0].CRM_CUSTOMER_MST.CUS_COUNTRY.ToString();
                            lblInvoiceAmt.Text = GetLocalResourceObject("InvoiceAmt").ToString() + " (" + finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                            lblDiscount.Text = GetLocalResourceObject("Discount").ToString() + " (" + finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                            lblTaxAmount.Text = GetLocalResourceObject("TaxAmount").ToString() + " (" + finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                            lblNetAmount.Text = GetLocalResourceObject("NetAmount").ToString() + " (" + finInvoiceCusHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                            txtPaybydate.Text = finInvoiceCusHdrList[0].ICH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                            hdfPaybydate.Value = finInvoiceCusHdrList[0].ICH_DATE_PAY_BY.ToString();

                            SaleOrderType = finInvoiceCusHdrList[0].ICH_TYPE;
                            // Convert.ToByte(finInvoiceCusHdrList[0].FIN_INVOICE_CUS_DTL.Select(d => d.SAL_ORDER_DTL.SAL_ORDER_HDR.SOH_TYPE).First()); 

                            txtInvoiceAmt.Text = GetFormattedCurrency(finInvoiceCusHdrList[0].ICH_AMOUNT_TC.ToString());
                            txtDiscount.Text = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? GetFormattedCurrency(finInvoiceCusHdrList[0].ICH_DISCOUNT_TC.ToString()) : "0";
                            txtInvdate.Text = finInvoiceCusHdrList[0].ICH_DATE.ToString(Resources.Constants.DateFormatShort);
                            hdfInvdate.Value = finInvoiceCusHdrList[0].ICH_DATE.ToString();
                            txtTaxAmount.Text = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? GetFormattedCurrency(finInvoiceCusHdrList[0].ICH_TAX_TC.ToString()) : "0";
                            txtNetAmount.Text = GetFormattedCurrency(finInvoiceCusHdrList[0].ICH_AMOUNT_NET_TC.ToString());
                            txtRemarks.Text = HttpUtility.HtmlDecode(finInvoiceCusHdrList[0].ICH_REMARKS);
                            LastModifiedTime = finInvoiceCusHdrList[0].ICH_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            hdfInvRcvdAmt.Value = finInvoiceCusHdrList[0].ICH_AMOUNT_RCVD_TC.ToString();
                            hdfIsJournalize.Value = finInvoiceCusHdrList[0].ICH_HAS_JRNL_ENTRY.ToString();
                            Approved = WkfStatus = finInvoiceCusHdrList[0].ICH_STATUS;
                            hdfInvDelStatus.Value = finInvoiceCusHdrList[0].ICH_DEL_STATUS.ToString();
                            hdfInvoiceReference.Value = finInvoiceCusHdrList[0].ICH_REFERENCE;


                            //hdfType.Value = finInvoiceCusHdrList[0].ICH_TYPE.ToString();
                            ddlInvoiceType.SelectedValue = finInvoiceCusHdrList[0].ICH_TYPE.ToString();
                            SaleOrderType = finInvoiceCusHdrList[0].ICH_TYPE;
                            ddlCompany.SelectedValue = finInvoiceCusHdrList[0].ICH_COMPANY.ToString();

                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = finInvoiceCusHdrList[0].ICH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            }

                            Session["SOH_CUSTOMER"] = finInvoiceCusHdrList[0].ICH_CUSTOMER;
                            Session["SOH_CUSTOMER_ADDRESS"] = finInvoiceCusHdrList[0].ICH_CUSTOMER_ADDRESS;
                            Session["SOH_CUSTOMER_COUNTRY"] = finInvoiceCusHdrList[0].ICH_CUSTOMER_COUNTRY;
                            Session["SOH_CUSTOMER_EMAIL"] = finInvoiceCusHdrList[0].ICH_CUSTOMER_EMAIL;
                            Session["SOH_CUSTOMER_FAX"] = finInvoiceCusHdrList[0].ICH_CUSTOMER_FAX;
                            Session["SOH_CUSTOMER_MOBILE"] = finInvoiceCusHdrList[0].ICH_CUSTOMER_MOBILE;
                            Session["SOH_CUSTOMER_NAME"] = finInvoiceCusHdrList[0].ICH_CUSTOMER_NAME;
                            Session["SOH_CUSTOMER_PHONE"] = finInvoiceCusHdrList[0].ICH_CUSTOMER_PHONE;
                            Session["SOH_CUSTOMER_ZIP"] = finInvoiceCusHdrList[0].ICH_CUSTOMER_ZIP;
                            Session["SOH_PAYMENT_TERM"] = finInvoiceCusHdrList[0].ICH_PAYMENT_TERM;
                            Session["SOH_PAYMENT_TERM_TEXT"] = finInvoiceCusHdrList[0].ICH_PAYMENT_TERM_TEXT;


                            //CustomerTypes
                            custPK = Convert.ToInt32(finInvoiceCusHdrList[0].ICH_CUSTOMER);
                            hdfCurrCustomerPK.Value = finInvoiceCusHdrList[0].ICH_CUSTOMER.ToString();
                            CustomerTypeSelectedPk = string.IsNullOrEmpty(finInvoiceCusHdrList[0].ICH_BRANCH.ToString()) ? 0 : Convert.ToInt32(finInvoiceCusHdrList[0].ICH_BRANCH);
                            CustomerSavedBranchId = string.IsNullOrEmpty(finInvoiceCusHdrList[0].ICH_BRANCH_TEXT) ? "" : finInvoiceCusHdrList[0].ICH_BRANCH_TEXT;
                            CustomerSavedTaxId = string.IsNullOrEmpty(finInvoiceCusHdrList[0].ICH_TAX_ID) ? "" : finInvoiceCusHdrList[0].ICH_TAX_ID;
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
                            if (!string.IsNullOrEmpty(finInvoiceCusHdrList[0].ICH_GST_TYPE.ToString()))
                                ddlInvoiceGstType.SelectedValue = finInvoiceCusHdrList[0].ICH_GST_TYPE.ToString();


                            if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Export) && IsInvoiceGSTEnable)
                            {
                                ddlSubType.Visible = true;
                                lblSubType.Visible = true;
                                if (finInvoiceCusHdrList[0].ICH_SUB_TYPE.HasValue)
                                    ddlSubType.SelectedValue = finInvoiceCusHdrList[0].ICH_SUB_TYPE.ToString();
                            }
                            else
                            {
                                ddlSubType.Visible = false;
                                lblSubType.Visible = false;
                            }
                            //End

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
                case ControlsEnum.INVOICETYPE:
                    ddlInvoiceType.Items.Clear();
                    if (admConfigMstList != null && admConfigMstList.Count > 0)
                    {
                        ddlInvoiceType.DataSource = admConfigMstList;
                        ddlInvoiceType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlInvoiceType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlInvoiceType.DataBind();
                    }
                    ddlInvoiceType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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

                        ddlCompanyView.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompanyView.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompanyView.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanyView.DataBind();
                    }


                    break;
                #endregion
                case ControlsEnum.COMPANYNAME:
                    ddlPlantName.Items.Clear();
                    if (admCompanyMstList != null && admCompanyMstList.Count > 0)
                    {
                        ddlPlantName.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CMP_DISPLAY_CODE);
                        ddlPlantName.DataTextField = Resources.DataFieldRes.CMP_DISPLAY_CODE;
                        ddlPlantName.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlPlantName.DataBind();
                    }
                    ddlPlantName.Items.Insert(0, new ListItem(Resources.ErpRes.SelectAll, CommonConstants.SELECTVAL));
                    break;
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

                #region SOTYPE
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
                        ddlCustomerType.SelectedValue = CustomerTypeSelectedPk.ToString();
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
                    #region Invoice Hdr
                    case ControlsEnum.INVOICEHDR:
                        if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                        {
                            GetFieldValues(ControlsEnum.SALES_INVOICE_TYPE);


                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdSalesInvoiceList.DataSource = finInvoiceCusHdrList;
                            grdSalesInvoiceList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();

                            //For Setting/Resetting Colour of a selected InvoiceNo
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
                        }
                        else
                        {
                            grdSalesInvoiceList.DataSource = null;
                            grdSalesInvoiceList.DataBind();
                            uclPaging.Visible = false;
                        }
                        break;
                    #endregion
                    #region PO Invoice List
                    case ControlsEnum.POINVOICELIST:

                        if (finInvoiceCusTrxMpgList != null && finInvoiceCusTrxMpgList.Count > 0)
                        {
                            grdSalesList.DataSource = finInvoiceCusTrxMpgList;
                            grdSalesList.DataBind();
                        }
                        else
                        {
                            grdSalesList.DataSource = null;
                            grdSalesList.DataBind();
                        }
                        if (SalOrderHdrList != null && SalOrderHdrList.Count > 0)
                        {
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(SalOrderHdrList[0].SOH_COMPANY.ToString()));
                            lblDispCustomerName.Text = ERP.Utilities.CommonFunctions.GetShortString(SalOrderHdrList[0].CRM_CUSTOMER_MST.CUS_NAME, 46);
                            lblDispCustomerName.ToolTip = ERP.Utilities.CommonFunctions.GetShortString(SalOrderHdrList[0].CRM_CUSTOMER_MST.CUS_NAME, 326);
                            SaleOrderType = SalOrderHdrList[0].SOH_TYPE;

                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = SalOrderHdrList[0].SOH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            }

                            hdfCurrCustomerPK.Value = SalOrderHdrList[0].CRM_CUSTOMER_MST.CUS_PK.ToString();
                            hdfCustomerCountry.Value = SalOrderHdrList[0].CRM_CUSTOMER_MST.CUS_COUNTRY.ToString();
                            lblInvoiceAmt.Text = GetLocalResourceObject("InvoiceAmt").ToString() + " (" + SalOrderHdrList[0].ADM_CURRENCY_MST.CUR_CODE + ")";
                            lblDiscount.Text = GetLocalResourceObject("Discount").ToString() + " (" + SalOrderHdrList[0].ADM_CURRENCY_MST.CUR_CODE + ")";
                            lblTaxAmount.Text = GetLocalResourceObject("TaxAmount").ToString() + " (" + SalOrderHdrList[0].ADM_CURRENCY_MST.CUR_CODE + ")";
                            lblNetAmount.Text = GetLocalResourceObject("NetAmount").ToString() + " (" + SalOrderHdrList[0].ADM_CURRENCY_MST.CUR_CODE + ")";
                            if (!ischanged)
                            {
                                if (SalOrderHdrList[0].SOH_TYPE == Convert.ToByte(SalesInvoiceType.Export))
                                {
                                    ddlInvoiceType.SelectedValue = Convert.ToByte(SalesInvoiceType.Proforma).ToString();
                                }
                                else if(SalOrderHdrList[0].SOH_TYPE == Convert.ToByte(SalesInvoiceType.Deemed))
                                {
                                     ddlInvoiceType.SelectedValue = Convert.ToByte(SalesInvoiceType.Deemed).ToString();
                                }
                                else
                                {
                                    ddlInvoiceType.SelectedValue = SalOrderHdrList[0].SOH_TYPE.ToString();
                                    if (!IsAdvInvHasTax)
                                        ddlInvoiceType.SelectedValue = Convert.ToByte(SalesInvoiceType.Proforma).ToString();
                                }
                            }

                            if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Export) && IsInvoiceGSTEnable)
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

                            grdSalesList.DataSource = SalOrderHdrList;
                            grdSalesList.DataBind();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);

                            //hdfType.Value = SalOrderHdrList[0].SOH_TYPE.ToString();

                            Session["SOH_CUSTOMER"] = SalOrderHdrList[0].SOH_CUSTOMER;
                            Session["SOH_CUSTOMER_ADDRESS"] = SalOrderHdrList[0].SOH_CUSTOMER_ADDRESS;
                            Session["SOH_CUSTOMER_COUNTRY"] = SalOrderHdrList[0].SOH_CUSTOMER_COUNTRY;
                            Session["SOH_CUSTOMER_EMAIL"] = SalOrderHdrList[0].SOH_CUSTOMER_EMAIL;
                            Session["SOH_CUSTOMER_FAX"] = SalOrderHdrList[0].SOH_CUSTOMER_FAX;
                            Session["SOH_CUSTOMER_MOBILE"] = SalOrderHdrList[0].SOH_CUSTOMER_MOBILE;
                            Session["SOH_CUSTOMER_NAME"] = SalOrderHdrList[0].SOH_CUSTOMER_NAME;
                            Session["SOH_CUSTOMER_PHONE"] = SalOrderHdrList[0].SOH_CUSTOMER_PHONE;
                            Session["SOH_CUSTOMER_ZIP"] = SalOrderHdrList[0].SOH_CUSTOMER_ZIP;
                            Session["SOH_PAYMENT_TERM"] = SalOrderHdrList[0].SOH_PAYMENT_TERM;
                            Session["SOH_PAYMENT_TERM_TEXT"] = SalOrderHdrList[0].SOH_PAYMENT_TERM_TEXT;

                        }

                        break;
                    #endregion
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
                    //btnSave.Visible = true;
                }
                else
                {
                    ucrWrkf.ViewType = 0;
                    //EntryStatus = EntryStatus.VIEWMODE;
                    //btnSave.Visible = false;
                }
                ucrWrkf.ViewAction();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Method used to Reset Form Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            txtInvoiceNumber.Text = "Select/Type";
            txtCustomer.Text = "Select/Type";
            hdfIVHPK.Value = "";
            hdfCustomerID.Value = "";
            txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
            //txtFromDate.Text = string.Empty;
            //hdfFromDate.Value = string.Empty;
            //txtToDate.Text = string.Empty;
            //hdfToDate.Value = string.Empty; 
            txtSCno.Text = string.Empty;
            ddlStatus.SelectedIndex = 0;
            //GetFieldValues(ControlsEnum.FINPERIOD);
            //SetFieldValues(ControlsEnum.FINPERIOD);
            ModifiedDatePnl.Visible = false;
            Session["SOH_CUSTOMER"] = null;
            Session["SOH_CUSTOMER_ADDRESS"] = null;
            Session["SOH_CUSTOMER_COUNTRY"] = null;
            Session["SOH_CUSTOMER_EMAIL"] = null;
            Session["SOH_CUSTOMER_FAX"] = null;
            Session["SOH_CUSTOMER_MOBILE"] = null;
            Session["SOH_CUSTOMER_NAME"] = null;
            Session["SOH_CUSTOMER_PHONE"] = null;
            Session["SOH_CUSTOMER_ZIP"] = null;
            Session["SOH_PAYMENT_TERM"] = null;
            Session["SOH_PAYMENT_TERM_TEXT"] = null;
            base.WkfRefID = 0;
            ddlInvoiceType.Enabled = false;
            hdfTaxSettings.Value = string.Empty;
            ddlSaleOrderType.SelectedIndex = 0;
            ddlPlantName.SelectedIndex = 0;
            ddlInvoiceGstType.SelectedIndex = 0;
            ddlSubType.ClearSelection();
        }

        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.RESETRECEIPT:
                    SelectedCurrency = new List<long>();
                    Currency = 0;
                    SelectedCustomers = new List<long>();
                    CustomerID = 0;
                    SelectedInvoiceType = new List<long>();
                    InvType = 0;
                    SelectedINVTax = null;
                    SelectedINVTaxList = new List<decimal>();
                    SelectedSalesInvoiceList = new List<long>();
                    SelectedSalesInvoices = new List<long>();
                    SelectedInvoiceCrDrList = new List<long>();
                    SelectedCurrencyList = new List<long>();
                    SelectedInvoiceTypeList = new List<long>();
                    SelectedCustomersList = new List<long>();
                    SelectedInvoicesCrDr = new List<long>();
                    SelectedInvoiceCrDrList = new List<long>();
                    Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices] = null;
                    Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = null;
                    btnPickForReceipt.Text = GetLocalResourceObject("PickInvforReceipt").ToString();
                    btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString();
                    //Resetting Color
                    hdfSelectedItemPk.Value = "0";
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

            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                BusinessObject.User currentUser;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

                GridViewRow gvr;
                bool bIsChecked = false;
                DropDownList ddlWkfAction;
                long result;
                int? alertresult;
                string action;
                TextBox WrkfComments;
                decimal TotalAmount = 0;
                decimal NetAmount = 0;
                Label lblTotalPayNowFooter;
                RadioButton rbtn;
                HiddenField hdfDept;
                int selectedInvPK;
                int dept;
                int invType = 1;
                int salesContractType = 1;
                string soPK;
                CheckBox chkInvselect;
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
                    if (((DropDownList)sender).ID == "ddlInvoiceType")
                    {
                        commonActions = ActionsEnum.SALESINVOICETYPECHANGED;
                    }
                    //CustomerTypes
                    if (((DropDownList)sender).ID == "ddlCustomerType")
                    {
                        commonActions = ActionsEnum.CUSTOMERTYPECHANGING;
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
                    #region ITEMSELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow selectedGrdrow = (sender as RadioButton).Parent.Parent as GridViewRow;
                        rbtn = sender as RadioButton;
                        bIsChecked = true;
                        selectedInvPK = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfInvoiceID")).Value);
                        //int apvd = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfApproved")).Value);
                        SelectedInvPK = selectedInvPK;

                        hdfDept = selectedGrdrow.FindControl("hdfDept") as HiddenField; //juno
                        int pid = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfInvType")).Value);
                        RPTTYPE = pid;
                        isCancelled = Convert.ToInt32(((HiddenField)selectedGrdrow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                        if (isCancelled)
                            btnSave.Visible = false;
                        else
                            btnSave.Visible = true;

                        if (pid == Convert.ToInt32(SalesInvoiceType.Domestic))
                            FillProcessID(3);
                        else
                            FillProcessID(1);
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = workflowCore.GetRefID(selectedInvPK, PageProcessID);
                        ////

                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                        break;
                    #endregion
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

                            if (ddlCustomerType.Items.Count <= 0)
                            {

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                                return;
                            }
                            if (grdSalesList.Rows.Count <= 0)
                            {

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_ErrSave_Invoice").ToString()) + "');", true);
                                return;
                            }


                            foreach (GridViewRow grdPOrow in grdSalesList.Rows)
                            {
                                Label lblTotalAmountSO = (Label)grdPOrow.FindControl("lblTotalAmount");
                                Label lblInvoicedSO = (Label)grdPOrow.FindControl("lblInvoiced");
                                TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtPayNow");
                                Label lblPInvoicedSO = (Label)grdPOrow.FindControl("lblProformaInvoiced");

                                TextBox txtOthercharges = (TextBox)grdPOrow.FindControl("txtOthercharges");
                                Label lblOtherAmount = (Label)grdPOrow.FindControl("lblOtherAmount");
                                HiddenField hdfOtherchargeOLD = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");
                                HiddenField hdfPayNow = (HiddenField)grdPOrow.FindControl("hdfPayNow");
                                if (hdfOtherchargeOLD.Value == "") { hdfOtherchargeOLD.Value = "0"; }
                                if (hdfPayNow.Value == "") { hdfPayNow.Value = "0"; }
                                if (lblTotalAmountSO.Text == "") { lblTotalAmountSO.Text = "0"; }
                                if (lblInvoicedSO.Text == "") { lblInvoicedSO.Text = "0"; }
                                if (txtPayNow.Text == "") { txtPayNow.Text = "0"; }
                                if (txtOthercharges.Text == "") { txtOthercharges.Text = "0"; }

                                if (ItemStatus == 0)
                                {
                                    if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic))
                                    {
                                        if (Convert.ToDecimal(lblTotalAmountSO.Text) >= Convert.ToDecimal(lblInvoicedSO.Text) + Convert.ToDecimal(txtPayNow.Text))
                                        {
                                            //if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text) + Convert.ToDecimal(hdfOtherchargeOLD.Value))
                                            if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text)) 
                                            {
                                                //if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblInvoicedSO.Text))) - (Convert.ToDecimal(txtOthercharges.Text) + (Convert.ToDecimal(hdfOtherchargeOLD.Value))) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                                if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblInvoicedSO.Text))) - (Convert.ToDecimal(txtOthercharges.Text)) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                                {
                                                    if ((Convert.ToDecimal(txtPayNow.Text)) >= (Convert.ToDecimal(txtOthercharges.Text)))
                                                    {
                                                        cont = true;
                                                    }

                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_7").ToString();
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        break;
                                                    }
                                                  
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
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(lblPInvoicedSO.Text))
                                            if (Convert.ToDecimal(txtPayNow.Text) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblPInvoicedSO.Text)))
                                            {
                                                cont = true;
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                  "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_5").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                    }
                                }
                                else
                                {
                                    if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic))
                                    {
                                        if (Convert.ToDecimal(lblTotalAmountSO.Text) >= (Convert.ToDecimal(lblInvoicedSO.Text) - Convert.ToDecimal(hdfPayNow.Value)) + Convert.ToDecimal(txtPayNow.Text))
                                        {
                                            if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text) + Convert.ToDecimal(hdfOtherchargeOLD.Value))
                                            {
                                                if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblInvoicedSO.Text) - Convert.ToDecimal(hdfPayNow.Value))) - (Convert.ToDecimal(txtOthercharges.Text) + (Convert.ToDecimal(hdfOtherchargeOLD.Value))) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                                {
                                                    if ((Convert.ToDecimal(txtPayNow.Text)) >= (Convert.ToDecimal(txtOthercharges.Text)))
                                                    {
                                                        cont = true;
                                                    }

                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                 "ClosePopup();", true);
                                                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_7").ToString();
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        break;
                                                    }
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
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(lblPInvoicedSO.Text))
                                            if (Convert.ToDecimal(txtPayNow.Text) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblPInvoicedSO.Text)))
                                            {
                                                cont = true;
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                  "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Err_msg_4").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }
                                    }
                                }
                            }
                            if (cont == true)
                            {
                                if (hdfIsJournalize.Value == "True")
                                {
                                    litErrorMsg.Text = Resources.Messages.Msg_Journalize;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else
                                {
                                    if (grdSalesList.Rows.Count > 0)
                                    {
                                        ///////
                                        byte invoiceType = Convert.ToByte(ddlInvoiceType.SelectedValue);
                                        bool isValidEntry = true;
                                        foreach (GridViewRow grdRow in grdSalesList.Rows)
                                        {
                                            Label lblTotal = (Label)grdRow.FindControl("lblTotalAmount");
                                            TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNow");
                                            HiddenField hdfInvoiced = (HiddenField)grdRow.FindControl("hdfInvoiced");
                                            if (lblTotal != null && !string.IsNullOrEmpty(lblTotal.Text)
                                                && txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text)
                                                && hdfInvoiced != null && !string.IsNullOrEmpty(hdfInvoiced.Value))
                                            {
                                                if (invoiceType != (byte)SalesInvoiceType.Proforma)
                                                {
                                                    if (hdfInvoiced != null && !string.IsNullOrEmpty(hdfInvoiced.Value))
                                                    {
                                                        if (Convert.ToDecimal(txtPayNow.Text.Trim().Replace(",", "")) > Convert.ToDecimal(lblTotal.Text.Trim().Replace(",", "")) - Convert.ToDecimal(Math.Round(decimal.Parse(hdfInvoiced.Value), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)))
                                                        {
                                                            isValidEntry = false;
                                                            break;
                                                        }
                                                    }

                                                }
                                                else
                                                {
                                                    HiddenField hdfProformaInvoiced = (HiddenField)grdRow.FindControl("hdfProformaInvoiced");
                                                    if (hdfProformaInvoiced != null && !string.IsNullOrEmpty(hdfProformaInvoiced.Value))
                                                    {
                                                        //if (Convert.ToDecimal(txtPayNow.Text.Trim().Replace(",", "")) > Convert.ToDecimal(lblTotal.Text.Trim().Replace(",", "")) -
                                                        //    ((Convert.ToDecimal(hdfInvoiced.Value) > Convert.ToDecimal(hdfProformaInvoiced.Value))
                                                        //        ? Convert.ToDecimal(hdfInvoiced.Value) : Convert.ToDecimal(hdfProformaInvoiced.Value)))
                                                        //{
                                                        //    isValidEntry = false;
                                                        //    break;
                                                        //}
                                                    }
                                                }
                                            }
                                        }
                                        ///////
                                        if (isValidEntry)
                                        {
                                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                                            lblTotalPayNowFooter = (Label)grdSalesList.FooterRow.FindControl("lblTotalPayNowFooter");
                                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                                            if (NetAmount == TotalAmount)
                                            {
                                                finInvoiceCusHdrList = new List<FIN_INVOICE_CUS_HDR>();
                                                salesInvoiceServiceClient = new SalesInvoiceService();
                                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                                finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                                                finInvoiceCusHdrObj = (FIN_INVOICE_CUS_HDR)SetUIValuesToObject(ControlsEnum.FINANCEINVOICEHDR);
                                                if (finInvoiceCusHdrObj != null)
                                                {
                                                    if (salesInvoiceServiceClient.CheckReceiptCreated(finInvoiceCusHdrObj))
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReceiptCreated").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + GetLocalResourceObject("Err_ReceiptCreated") + "','" + Resources.Messages.Information + "');", true);
                                                        return;
                                                    }
                                                    finInvoiceCusHdrList.Add(finInvoiceCusHdrObj);

                                                    int Archiveresult = 0;
                                                    if (finInvoiceCusHdrObj.ICH_PK > 0 && finInvoiceCusHdrObj.ICH_STATUS > 0)
                                                    {
                                                        //After getting entry into workflow, for each update keep version details of voucher for Audit trail 
                                                        Archiveresult = BusinessLogic.POInvoicing.POInvoiceBL.SaveSalesInvoiceArchiveDetails(finInvoiceCusHdrObj.ICH_PK);
                                                    }
                                                    if ((Archiveresult > 0 && finInvoiceCusHdrObj.ICH_STATUS > 0) || finInvoiceCusHdrObj.ICH_STATUS == 0)
                                                    {
                                                        result = salesInvoiceServiceClient.SaveSalesInvoiceHdr(finInvoiceCusHdrList);
                                                        if (result >= 0)
                                                        {
                                                            #region LOG SAVE
                                                            CommonServiceClient = new CommonService();
                                                            CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                                                            ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                                            List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                                            if (CurrPK > 0)
                                                            {
                                                                AdmTrxLogDet.ATL_ACTION = (byte)LogAction.UPDATE;
                                                            }
                                                            else
                                                            {
                                                                AdmTrxLogDet.ATL_ACTION = (byte)LogAction.NEW;
                                                            }
                                                            AdmTrxLogDet.ATL_APP_TRX_CODE = (string.IsNullOrEmpty(lblDispInvoiceNo.Text) || lblDispInvoiceNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                                                                : lblDispInvoiceNo.Text.Trim(); ;
                                                            AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.SI;
                                                            if (ddlInvoiceType.SelectedValue == "1")
                                                            {
                                                                AdmTrxLogDet.ATL_APP_SUB_TYPE = 11;
                                                            }
                                                            else if (ddlInvoiceType.SelectedValue == "2")
                                                            {
                                                                AdmTrxLogDet.ATL_APP_SUB_TYPE = 12;
                                                            }
                                                            else if (ddlInvoiceType.SelectedValue == "3")
                                                            {
                                                                AdmTrxLogDet.ATL_APP_SUB_TYPE = 13;
                                                            }
                                                            AdmTrxLogDet.ATL_MOD_BY = currentUser.PKUser;
                                                            AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                                                            AdmTrxLogDet.ATL_BIZUNIT = currentUser.SBUID;
                                                            AdmTrxLogDet.ATL_APP_TRX_PK = Convert.ToInt64(result);
                                                            AdmTrxLogDet.ATL_PK = 0;
                                                            AdmTrxLogList.Add(AdmTrxLogDet);
                                                            CommonServiceClient.SaveLog(AdmTrxLogList);
                                                            #endregion
                                                            litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.Invoice);
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                            EntryStatus = EntryStatus.LISTMODE;
                                                            ResetForm();
                                                            Session[ERP.Utilities.SessionStrings.SelectedSos] = null;
                                                            Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv] = null;
                                                            SelectedSosForAdvInv = null;
                                                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                                            GetFieldValues(ControlsEnum.INVOICEHDR);
                                                            SetFieldValues(ControlsEnum.INVOICEHDR);
                                                            btnNew.Focus();
                                                        }
                                                        else
                                                        {
                                                            if (result == (int)DbSaveStatus.SQLERROR)
                                                            {
                                                                litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                    + "','" + Resources.ErpRes.Information + "');", true);
                                                                return;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                    }

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
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid1").ToString();
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
                        break;
                    #endregion
                    #region New
                    case ActionsEnum.NEW:
                        if (SelectedSosForAdvInv != null)
                        {
                            SelectedSOListForAdvInv = SelectedSosForAdvInv;
                            ModifiedDatePnl.Visible = false;
                            GetFieldValues(ControlsEnum.POINVOICELIST);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            EntryStatus = EntryStatus.NEWMODE;
                            updateInvoice = false;
                            GetFieldValues(ControlsEnum.INVOICENO);
                            lblDispInvoiceNo.Text = hdfInvoiceNo.Value;
                        }
                        else
                        {
                            SelectedSOListForAdvInv = new List<long>();
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICEHDR);
                            SetFieldValues(ControlsEnum.INVOICEHDR);
                            EntryStatus = EntryStatus.LISTMODE;

                        }
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        PageIndex = "1";
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        break;
                    #endregion
                    #region Cancel
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;

                    #endregion
                    #region Edit
                    case ActionsEnum.EDIT:
                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            //rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            //if (rbtn.Checked)
                            //{                           
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                ItemStatus = Convert.ToInt16(hdfStatus.Value);

                                RPTTYPE = invType;
                                isCancelled = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                                if (isCancelled)
                                    btnSave.Visible = false;
                                else
                                    btnSave.Visible = true;

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;

                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                FillProcessID(3);
                            else
                                FillProcessID(1);

                            WorkflowCore.CoreService workflowCoreObj = new WorkflowCore.CoreService();
                            base.WkfRefID = workflowCoreObj.GetRefID(CurrPK, PageProcessID);

                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                            GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            //if (!IsAdvInvHasTax)
                            //{
                            //    if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                            //    {
                            //        int SohType = finInvoiceCusHdrList[0].FIN_INVOICE_CUS_TRX_MPG.ToList()[0].SAL_ORDER_HDR.SOH_TYPE;
                            //        if (SohType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //            FillProcessID(3);
                            //        else
                            //            FillProcessID(1);

                            //        WorkflowCore.CoreService workflowCoreObj1 = new WorkflowCore.CoreService();
                            //        base.WkfRefID = workflowCoreObj1.GetRefID(CurrPK, ucrWrkf.ProcessID);
                            //        ucrWrkf.FillWorkFlowDetails();
                            //        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            //            ucrWrkf.ViewType = 1;
                            //        else
                            //        {
                            //            ucrWrkf.ViewType = 0;
                            //        }
                            //    }
                            //}
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region View
                    case ActionsEnum.VIEW:
                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////
                                invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                RPTTYPE = invType;
                                if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                    FillProcessID(3);
                                else
                                    FillProcessID(1);

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                            finInvoiceCusHdrObj.ICH_PK = CurrPK;
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                            GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region Clear
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        break;
                    #endregion
                    #region Remove
                    case ActionsEnum.REMOVE:
                        int SOPk = int.Parse(((Button)sender).CommandArgument.ToString());
                        HiddenField hdfSONumber = null;
                        TextBox txtPay = null;
                        foreach (GridViewRow grdRow in grdSalesList.Rows)
                        {
                            hdfSONumber = grdRow.FindControl("hdfSONumber") as HiddenField;
                            txtPay = grdRow.FindControl("txtPayNow") as TextBox;
                            if (hdfSONumber != null && txtPay != null && !string.IsNullOrEmpty(hdfSONumber.Value) && !string.IsNullOrEmpty(txtPay.Text))
                            {
                                decimal d = 0;
                                decimal.TryParse(txtPay.Text, out d);
                                if (!dicTempAmount.ContainsKey(hdfSONumber.Value)
                                    && hdfSONumber.Value != SOPk.ToString())
                                {
                                    dicTempAmount.Add(hdfSONumber.Value, d);
                                }
                            }
                        }

                        if (CurrPK == 0)
                        {
                            SelectedSosForAdvInv.Remove(SOPk);
                            SelectedSOListForAdvInv = SelectedSosForAdvInv;
                            SalOrderHdrList = (List<SAL_ORDER_HDR>)SalHeaderList;
                            SalOrderHdrObj = SalOrderHdrList.SingleOrDefault(po => po.SOH_PK == SOPk);
                            SalOrderHdrList.Remove(SalOrderHdrObj);
                            SalHeaderList = SalOrderHdrList;
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                        }
                        else
                        {
                            finInvoiceCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_TRX_MPG>();
                            finInvoiceCusTrxMpgList = (List<FIN_INVOICE_CUS_TRX_MPG>)InvoiceCusMapList;
                            finInvoiceCusTrxMpgObj = finInvoiceCusTrxMpgList.SingleOrDefault(po => po.SAL_ORDER_HDR.SOH_PK == SOPk);
                            finInvoiceCusTrxMpgList.Remove(finInvoiceCusTrxMpgObj);
                            InvoiceCusMapList = finInvoiceCusTrxMpgList;
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                        }
                        break;
                    #endregion
                    #region Get Pk
                    case ActionsEnum.SHOWDETAILS:
                        gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        CurrPK = Convert.ToInt32(((HiddenField)gvr.FindControl("hdfInvoiceID")).Value);
                        break;
                    #endregion
                    #region Pick for Receipt
                    case ActionsEnum.PICKFORRECEIPT:
                        ResetForm(ControlsEnum.RESETRECEIPT);
                        SetUIValuesToObject(ControlsEnum.PICKFORRECEIPT);
                        //SetResetColour
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        break;
                    #endregion
                    #region Pick Inv & for Cr/Dr. Note
                    case ActionsEnum.PICKFORCRDRNOTE:
                        Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                        SetUIValuesToObject(ControlsEnum.PICKFORCRDRNOTE);
                        break;
                    #endregion
                    #region Tab navigation
                    case ActionsEnum.DEFAULT:
                        SelectedSosForAdvInv = null;
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
                        SelectedSosForAdvInv = null;
                        CheckUserRightsAndRedirect(Resources.PageURL.SalesInvoicing);
                        //Response.Redirect(Resources.PageURL.SalesInvoicing);
                        break;
                    case ActionsEnum.INVOICE:
                        SelectedSosForAdvInv = null;
                        CheckUserRightsAndRedirect(Resources.PageURL.Invoicing);
                        //Response.Redirect(Resources.PageURL.Invoicing);
                        break;
                    case ActionsEnum.DELIVERYORDER:
                        SelectedSosForAdvInv = null;
                        CheckUserRightsAndRedirect(Resources.PageURL.DeliveryOrder);
                        //Response.Redirect(Resources.PageURL.DeliveryOrder);
                        break;
                    case ActionsEnum.SALESRECEIPT:
                        SelectedSosForAdvInv = null;
                        CheckUserRightsAndRedirect(Resources.PageURL.SalesReceipt);
                        //Response.Redirect(Resources.PageURL.SalesReceipt);
                        break;
                    case ActionsEnum.ACRECEIVABLE:
                        SelectedSosForAdvInv = null;
                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }
                        CheckUserRightsAndRedirect(Resources.PageURL.AccountReceivable);
                        //Response.Redirect(Resources.PageURL.AccountsReceivable);
                        break;
                    case ActionsEnum.CRDRNOTE:
                        SelectedSosForAdvInv = null;
                        CheckUserRightsAndRedirect(Resources.PageURL.DrCrNoteSales);
                        //Response.Redirect(Resources.PageURL.DrCrNoteSales);
                        break;
                    case ActionsEnum.MISC:
                        SelectedSosForAdvInv = null;
                        CheckUserRightsAndRedirect(Resources.PageURL.MiscellaneousInv);
                        //Response.Redirect(Resources.PageURL.Misc);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (decimal.Parse(hdfInvRcvdAmt.Value) == 0)
                        {
                            salesInvoiceServiceClient = new SalesInvoiceService();
                            salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                            result = salesInvoiceServiceClient.DeleteSalesInvoice(CurrPK);
                            if (result > 0)
                            {
                                #region LOG SAVE
                                CommonServiceClient = new CommonService();
                                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                                ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                AdmTrxLogDet.ATL_ACTION = (byte)LogAction.DELETE;
                                AdmTrxLogDet.ATL_APP_TRX_CODE = (string.IsNullOrEmpty(lblDispInvoiceNo.Text) || lblDispInvoiceNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                                    : lblDispInvoiceNo.Text.Trim(); ;
                                AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.SI;
                                if (ddlInvoiceType.SelectedValue == "1")
                                {
                                    AdmTrxLogDet.ATL_APP_SUB_TYPE = 11;
                                }
                                else if (ddlInvoiceType.SelectedValue == "2")
                                {
                                    AdmTrxLogDet.ATL_APP_SUB_TYPE = 12;
                                }
                                else if (ddlInvoiceType.SelectedValue == "3")
                                {
                                    AdmTrxLogDet.ATL_APP_SUB_TYPE = 13;
                                }
                                AdmTrxLogDet.ATL_MOD_BY = currentUser.PKUser;
                                AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                                AdmTrxLogDet.ATL_BIZUNIT = currentUser.SBUID;
                                AdmTrxLogDet.ATL_APP_TRX_PK = CurrPK;
                                AdmTrxLogDet.ATL_PK = 0;
                                AdmTrxLogList.Add(AdmTrxLogDet);
                                CommonServiceClient.SaveLog(AdmTrxLogList);
                                #endregion

                                litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.InvoiceHdr);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                GetFieldValues(ControlsEnum.INVOICEHDR);
                                SetFieldValues(ControlsEnum.INVOICEHDR);
                                btnNew.Focus();
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Messages.CannotdeleteAlreadyasigned;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Inactive
                    case ActionsEnum.INACTIVE:
                        if (CurrPK > 0)
                        {
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                            if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count == 1)
                            {
                                if (finInvoiceCusHdrList[0].ICH_STATUS > 0)
                                {
                                    if (finInvoiceCusHdrList[0].ICH_HAS_JRNL_ENTRY)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Journalized").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (finInvoiceCusHdrList[0].ICH_AMOUNT_RCVD_TC > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Received").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (finInvoiceCusHdrList[0].ICH_AMOUNT_DN_TC > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Debit_Adjusted").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else if (finInvoiceCusHdrList[0].ICH_AMOUNT_CN_TC > 0)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Already_Credit_Adjusted").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                    else
                                    {
                                        salesInvoiceServiceClient = new SalesInvoiceService();
                                        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                        finInvoiceCusHdrObj = finInvoiceCusHdrList[0];
                                        finInvoiceCusHdrObj.ICH_MOD_DT = LastModifiedTime;
                                        finInvoiceCusHdrObj.ICH_REASON_FOR_DELETE = HttpUtility.HtmlEncode(txtReason.Text.Trim());
                                        result = salesInvoiceServiceClient.InActiveSalesInvoice(finInvoiceCusHdrObj);
                                        if (result > 0)
                                        {
                                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.InvoiceHdr);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm();
                                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                            GetFieldValues(ControlsEnum.INVOICEHDR);
                                            SetFieldValues(ControlsEnum.INVOICEHDR);
                                            btnNew.Focus();
                                        }
                                    }
                                }
                                else
                                {
                                    if (decimal.Parse(hdfInvRcvdAmt.Value) == 0)
                                    {
                                        salesInvoiceServiceClient = new SalesInvoiceService();
                                        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                        result = salesInvoiceServiceClient.DeleteSalesInvoice(CurrPK);
                                        if (result > 0)
                                        {
                                            litErrorMsg.Text = Resources.Messages.Msg_Delete_Success;
                                            litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.InvoiceHdr);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm();
                                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                            GetFieldValues(ControlsEnum.INVOICEHDR);
                                            SetFieldValues(ControlsEnum.INVOICEHDR);
                                            btnNew.Focus();
                                        }
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = Resources.Messages.CannotdeleteAlreadyasigned;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "closedeletepopup", "$(document).ready(function(){closeDeletePopup();});", true);

                        break;
                    #endregion
                    #region Invoice List
                    case ActionsEnum.INVOICELIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        hdfIVHPK.Value = "";
                        SelectedSOListForAdvInv = new List<long>();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region Invoice Details
                    case ActionsEnum.INVOICEDETAIL:

                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                // HiddenField hdfSalesContractType = (HiddenField)grdrow.FindControl("hdfSalesContractType");
                                salesContractType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfSalesContractType")).Value);
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////
                                //invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                RPTTYPE = invType;
                                isCancelled = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                                if (isCancelled)
                                    btnSave.Visible = false;
                                else
                                    btnSave.Visible = true;

                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            //if (invType == Convert.ToInt32(SalesInvoiceType.Proforma) || invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //    FillProcessID(3);
                            //else
                            //    FillProcessID(1);
                            //if (salesContractType == Convert.ToInt32(SalesInvoiceType.Domestic))
                            //    FillProcessID(3);
                            //else
                            //    FillProcessID(1);

                            if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                FillProcessID(3);
                            else
                                FillProcessID(1);

                            WorkflowCore.CoreService workflowCoreObj = new WorkflowCore.CoreService();
                            base.WkfRefID = workflowCoreObj.GetRefID(CurrPK, PageProcessID);

                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICEHDR);
                            finInvoiceCusHdrObj.ICH_PK = CurrPK;
                            GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                            GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                            SetFieldValues(ControlsEnum.POINVOICELIST);

                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        hdfIVHPK.Value = CurrPK.ToString();
                        finInvoiceCusHdrObj.ICH_PK = CurrPK;
                        GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                        SetUIValuesToObject(ControlsEnum.JOURNALIZE);
                        break;
                    #endregion
                    #region Journalize Update
                    case ActionsEnum.JOURNALIZEUPDATE:

                        ucrJournalize.ResetForm();
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        break;
                    #endregion
                    #region Journalize Save
                    case ActionsEnum.JOURNALIZESAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
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
                                result = salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                salesInvoiceServiceClient = new SalesInvoiceService();
                                salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                result = salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();

                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        //    Response.Redirect(Resources.PageURL.InboxURL);
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        //}
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
                                result = salesInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        break;
                    #endregion
                    #region Journalize Cancel
                    case ActionsEnum.JOURNALIZECANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        //Show WorkFlow Popup
                        if (grdSalesList.Rows.Count > 0)
                        {
                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                            lblTotalPayNowFooter = (Label)grdSalesList.FooterRow.FindControl("lblTotalPayNowFooter");
                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                            if (NetAmount == TotalAmount)
                            {
                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                                //Show WorkFlow Popup                       
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
                        if (grdSalesList.Rows.Count > 0)
                        {
                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                            lblTotalPayNowFooter = (Label)grdSalesList.FooterRow.FindControl("lblTotalPayNowFooter");
                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                            if (NetAmount == TotalAmount)
                            {
                                //Show WorkFlow Popup                       
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
                    case ActionsEnum.WRKFSUBMIT:
                        //Submit Activity
                        //validate Page
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
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
                                    if (ddlCustomerType.Items.Count <= 0)
                                    {

                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                                        return;
                                    }
                                    foreach (GridViewRow grdPOrow in grdSalesList.Rows)
                                    {
                                        Label lblTotalAmountSO = (Label)grdPOrow.FindControl("lblTotalAmount");
                                        Label lblInvoicedSO = (Label)grdPOrow.FindControl("lblInvoiced");
                                        Label lblPInvoicedSO = (Label)grdPOrow.FindControl("lblProformaInvoiced");
                                        TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtPayNow");

                                        TextBox txtOthercharges = (TextBox)grdPOrow.FindControl("txtOthercharges");
                                        Label lblOtherAmount = (Label)grdPOrow.FindControl("lblOtherAmount");
                                        HiddenField hdfOtherchargeOLD = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");
                                        if (hdfOtherchargeOLD.Value == "") { hdfOtherchargeOLD.Value = "0"; }
                                        if (lblTotalAmountSO.Text == "") { lblTotalAmountSO.Text = "0"; }
                                        if (lblInvoicedSO.Text == "") { lblInvoicedSO.Text = "0"; }
                                        if (txtPayNow.Text == "") { txtPayNow.Text = "0"; }
                                        if (txtOthercharges.Text == "") { txtOthercharges.Text = "0"; }

                                        if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic))
                                        {
                                            if (Convert.ToDecimal(lblTotalAmountSO.Text) >= Convert.ToDecimal(lblInvoicedSO.Text) + Convert.ToDecimal(txtPayNow.Text))
                                            {
                                                //if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text) + Convert.ToDecimal(hdfOtherchargeOLD.Value))
                                                if (Convert.ToDecimal(lblOtherAmount.Text) >= Convert.ToDecimal(txtOthercharges.Text))
                                                {
                                                    //if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblInvoicedSO.Text))) - (Convert.ToDecimal(txtOthercharges.Text) + (Convert.ToDecimal(hdfOtherchargeOLD.Value))) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                                    if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblInvoicedSO.Text))) - (Convert.ToDecimal(txtOthercharges.Text)) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblOtherAmount.Text)))
                                                    {
                                                        if ((Convert.ToDecimal(txtPayNow.Text)) >= (Convert.ToDecimal(txtOthercharges.Text)))
                                                        {
                                                            cont = true;
                                                        }

                                                        else
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                     "ClosePopup();", true);
                                                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_7").ToString();
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                            break;
                                                        }
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
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(lblPInvoicedSO.Text))
                                                if (Convert.ToDecimal(txtPayNow.Text) <= (Convert.ToDecimal(lblTotalAmountSO.Text) - Convert.ToDecimal(lblPInvoicedSO.Text)))
                                                {
                                                    cont = true;
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                      "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Err_msg_5").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    break;
                                                }
                                        }

                                    }
                                    if (cont == true)
                                    {
                                        if (grdSalesList.Rows.Count > 0)
                                        {
                                            byte invoiceType = Convert.ToByte(ddlInvoiceType.SelectedValue);
                                            bool isValidEntry = true;
                                            foreach (GridViewRow grdRow in grdSalesList.Rows)
                                            {
                                                Label lblTotal = (Label)grdRow.FindControl("lblTotalAmount");
                                                TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNow");
                                                HiddenField hdfInvoiced = (HiddenField)grdRow.FindControl("hdfInvoiced");
                                                if (lblTotal != null && !string.IsNullOrEmpty(lblTotal.Text)
                                                    && txtPayNow != null && !string.IsNullOrEmpty(txtPayNow.Text)
                                                    && hdfInvoiced != null && !string.IsNullOrEmpty(hdfInvoiced.Value))
                                                {
                                                    if (invoiceType != (byte)SalesInvoiceType.Proforma)
                                                    {
                                                        if (hdfInvoiced != null && !string.IsNullOrEmpty(hdfInvoiced.Value))
                                                        {
                                                            if (Convert.ToDecimal(txtPayNow.Text.Trim().Replace(",", "")) > Convert.ToDecimal(lblTotal.Text.Trim().Replace(",", "")) - Convert.ToDecimal(Math.Round(decimal.Parse(hdfInvoiced.Value), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits)))
                                                            {
                                                                isValidEntry = false;
                                                                break;
                                                            }
                                                        }

                                                    }
                                                    else
                                                    {
                                                        HiddenField hdfProformaInvoiced = (HiddenField)grdRow.FindControl("hdfProformaInvoiced");
                                                        if (hdfProformaInvoiced != null && !string.IsNullOrEmpty(hdfProformaInvoiced.Value))
                                                        {
                                                            //if (Convert.ToDecimal(txtPayNow.Text.Trim().Replace(",", "")) > Convert.ToDecimal(lblTotal.Text.Trim().Replace(",", "")) -
                                                            //    ((Convert.ToDecimal(hdfInvoiced.Value) > Convert.ToDecimal(hdfProformaInvoiced.Value))
                                                            //        ? Convert.ToDecimal(hdfInvoiced.Value) : Convert.ToDecimal(hdfProformaInvoiced.Value)))
                                                            //{
                                                            //    isValidEntry = false;
                                                            //    break;
                                                            //}
                                                        }
                                                    }
                                                }
                                            }
                                            ///////
                                            if (isValidEntry)
                                            {
                                                NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                                                TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                                                lblTotalPayNowFooter = (Label)grdSalesList.FooterRow.FindControl("lblTotalPayNowFooter");
                                                //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                                                lblTotalPayNowFooter.Text = GetFormattedCurrencyWithSeperator(TotalAmount);
                                                if (NetAmount == TotalAmount)
                                                {
                                                    finInvoiceCusHdrList = new List<FIN_INVOICE_CUS_HDR>();
                                                    salesInvoiceServiceClient = new SalesInvoiceService();
                                                    salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                                                    finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                                                    finInvoiceCusHdrObj = (FIN_INVOICE_CUS_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                                    if (hdfExchangeRate.Value != "-1")
                                                    {
                                                        if (finInvoiceCusHdrObj != null)
                                                        {
                                                            TypeRef = finInvoiceCusHdrObj.ICH_NO;
                                                            finInvoiceCusHdrList.Add(finInvoiceCusHdrObj);

                                                            int Archiveresult = 0;
                                                            if (finInvoiceCusHdrObj.ICH_PK > 0 && finInvoiceCusHdrObj.ICH_STATUS > 0)
                                                            {
                                                                //After getting entry into workflow, for each update keep version details of voucher for Audit trail 
                                                                Archiveresult = BusinessLogic.POInvoicing.POInvoiceBL.SaveSalesInvoiceArchiveDetails(finInvoiceCusHdrObj.ICH_PK);
                                                            }
                                                            if ((Archiveresult > 0 && finInvoiceCusHdrObj.ICH_STATUS > 0) || finInvoiceCusHdrObj.ICH_STATUS == 0)
                                                            {
                                                                result = salesInvoiceServiceClient.SaveSalesInvoiceHdr(finInvoiceCusHdrList, true);
                                                                if (result > 0)// Save Success ! do WorkFlow
                                                                {
                                                                    TypeRef = lblDispInvoiceNo.Text.Trim();
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
                                                                    #region WkfSummarySave
                                                                    int resultSummary = BusinessLogic.CommonManagement.CommonBL.SaveSummary(result, Convert.ToInt32(hdfProcessID.Value));
                                                                    if (resultSummary <= 0)
                                                                    {

                                                                        litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                                    }
                                                                    #endregion
                                                                    //Workflow submission
                                                                    ucrWrkf.ApplicationID = (int)result;
                                                                }
                                                                else
                                                                {
                                                                    if (result == (int)DbSaveStatus.SQLERROR)
                                                                    {
                                                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                                        return;
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                        "ClosePopup();", true);

                                                        litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                        return;
                                                    }
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                       "ClosePopup();", true);
                                                    litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid").ToString();
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                       "ClosePopup();", true);
                                                litErrorMsg.Text = GetLocalResourceObject("Msg_ErrAmount_NotValid1").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                            }
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                       "ClosePopup();", true);
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_ErrSave_Invoice").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                    }
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
                                    ResetForm();
                                    SelectedSosForAdvInv = null;
                                    GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                    GetFieldValues(ControlsEnum.INVOICEHDR);
                                    SetFieldValues(ControlsEnum.INVOICEHDR);
                                    btnNew.Focus();
                                    Session[ERP.Utilities.SessionStrings.SelectedSos] = null;
                                    Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv] = null;
                                    ViewState[ViewstateStrings.SelectedPosCount] = null;
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
                                    if (result > 0)
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

                                        object[] args = new object[2];
                                        args[0] = Resources.PageNameRes.Invoice;
                                        args[1] = lblDispInvoiceNo.Text;

                                        #region LOG SAVE

                                        CommonServiceClient = new CommonService();
                                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                                        List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                        AdmTrxLogDet.ATL_APP_TRX_CODE = lblDispInvoiceNo.Text;
                                        AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.SI;
                                        if (ddlInvoiceType.SelectedValue == "1")
                                        {
                                            AdmTrxLogDet.ATL_APP_SUB_TYPE = 11;
                                        }
                                        else if (ddlInvoiceType.SelectedValue == "2")
                                        {
                                            AdmTrxLogDet.ATL_APP_SUB_TYPE = 12;
                                        }
                                        else if (ddlInvoiceType.SelectedValue == "3")
                                        {
                                            AdmTrxLogDet.ATL_APP_SUB_TYPE = 13;
                                        }
                                        AdmTrxLogDet.ATL_MOD_BY = currentUser.PKUser;
                                        AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                                        AdmTrxLogDet.ATL_BIZUNIT = currentUser.SBUID;
                                        AdmTrxLogDet.ATL_APP_TRX_PK = ucrWrkf.ApplicationID;
                                        AdmTrxLogDet.ATL_PK = 0;
                                        AdmTrxLogList.Add(AdmTrxLogDet);
                                        CommonServiceClient.SaveLog(AdmTrxLogList);
                                        #endregion


                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                                        //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                        //    + "','" + Resources.ErpRes.Information + "');", true);

                                        if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                                        {
                                            ResetForm();
                                            SelectedSosForAdvInv = null;
                                            Session[ERP.Utilities.SessionStrings.SelectedSos] = null;
                                            Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv] = null;
                                            ViewState[ViewstateStrings.SelectedPosCount] = null;

                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                                + "','" + Resources.ErpRes.Information + "');", true);
                                            EntryStatus = EntryStatus.LISTMODE;
                                            ResetForm();
                                            SelectedSosForAdvInv = null;
                                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                            GetFieldValues(ControlsEnum.INVOICEHDR);
                                            SetFieldValues(ControlsEnum.INVOICEHDR);
                                            btnNew.Focus();
                                            Session[ERP.Utilities.SessionStrings.SelectedSos] = null;
                                            Session[ERP.Utilities.SessionStrings.SelectedSosForAdvInv] = null;
                                            ViewState[ViewstateStrings.SelectedPosCount] = null;
                                        }
                                    }
                                }
                            }

                        }
                        break;
                    #endregion
                    #region PRINT
                    case ActionsEnum.PRINT:
                        //    if (CurrPK > 0)
                        //    {

                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SIJ +
                        //               "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.SIJ + "');", true);

                        //    }
                        //    else
                        //    {

                        //        litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        //    }
                        //    break;
                        if (CurrPK > 0)
                        {
                            if (ddlInvoiceType.SelectedValue != null && Convert.ToInt32(ddlInvoiceType.SelectedIndex) > 0)
                            {
                                RPTTYPE = Convert.ToInt32(ddlInvoiceType.SelectedItem.Value);
                            }
                            if (RPTTYPE == 2 || RPTTYPE == 3)
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SIJ +
                                       "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.SIJ + "');", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                    CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=11") + "');", true);
                            }
                            // ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString()
                            // + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=11") + "');", true);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    case ActionsEnum.PRINTLISTING:
                        if (CurrPK == 0)
                        {
                            foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                            {
                                chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                                if (chkInvselect.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);

                                    if (bIsChecked)
                                    {
                                        HiddenField hdfInvType = (HiddenField)grdrow.FindControl("hdfInvType");

                                        if (hdfInvType.Value == "2" || hdfInvType.Value == "3")
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.SIJ +
                                                   "&APPSUBTYPE=" + Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.SIJ + "');", true);
                                        }
                                        else
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                                ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.SI + "&APPSUBTYPE=11") + "');", true);
                                        }
                                        CurrPK = 0;
                                    }
                                    return;
                                }
                            }

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                        }
                        break;
                    #endregion
                    #region Alert
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = ApplicationType.SI;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblDispInvoiceNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtInvdate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvdate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        hdfIVHPK.Value = CurrPK.ToString();
                        GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                        if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count == 1)
                        {
                            ucrAlert.TypePartyName = finInvoiceCusHdrList[0].CRM_CUSTOMER_MST.CUS_NAME;
                        }
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion
                    #region Reset
                    case ActionsEnum.RESET:
                        SelectedCurrency = new List<long>();
                        Currency = 0;
                        SelectedCustomers = new List<long>();
                        CustomerID = 0;
                        SelectedInvoiceType = new List<long>();
                        InvType = 0;
                        SelectedINVTax = null;
                        SelectedINVTaxList = new List<decimal>();
                        SelectedSalesInvoiceList = new List<long>();
                        SelectedSalesInvoices = new List<long>();
                        SelectedInvoiceCrDrList = new List<long>();
                        SelectedCurrencyList = new List<long>();
                        SelectedInvoiceTypeList = new List<long>();
                        SelectedCustomersList = new List<long>();
                        SelectedInvoicesCrDr = new List<long>();
                        SelectedInvoiceCrDrList = new List<long>();
                        Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices] = null;
                        Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = null;
                        btnPickForReceipt.Text = GetLocalResourceObject("PickInvforReceipt").ToString();
                        btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString();
                        //Resetting Color
                        hdfSelectedItemPk.Value = "0";
                        break;
                    #endregion
                    #region SALESINVOICETYPECHANGED
                    case ActionsEnum.SALESINVOICETYPECHANGED:
                        GetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                        SetFieldValues(ControlsEnum.INVOICEGSTTYPE);
                        Label lblTotalAmount;
                        lblTotalAmount = null;
                        Label lblInvoiced;
                        lblInvoiced = null;
                        Label lblProformaInvoiced;
                        lblProformaInvoiced = null;
                        Label lblBalancetoInvoice;
                        lblBalancetoInvoice = null;
                        TextBox txtPayNow1;
                        txtPayNow1 = null;
                        if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue) && ddlInvoiceType.SelectedValue == CommonConstants.SELECTVAL)
                        {
                            return;
                        }



                        if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Proforma))
                        {
                            if (!IsAdvInvHasTax)
                                ischanged = true;

                            grdSalesList.HeaderRow.Cells[9].Visible = true;
                            grdSalesList.FooterRow.Cells[9].Visible = true;
                            foreach (GridViewRow grdRow in grdSalesList.Rows)
                            {
                                grdRow.Cells[9].Visible = true;
                                lblTotalAmount = grdRow.FindControl("lblTotalAmount") as Label;
                                lblInvoiced = grdRow.FindControl("lblInvoiced") as Label;
                                lblProformaInvoiced = grdRow.FindControl("lblProformaInvoiced") as Label;
                                lblBalancetoInvoice = grdRow.FindControl("lblBalancetoInvoice") as Label;
                                txtPayNow1 = grdRow.FindControl("txtPayNow") as TextBox;
                                if (lblTotalAmount != null && lblInvoiced != null && lblProformaInvoiced != null && lblBalancetoInvoice != null && txtPayNow1 != null)
                                {
                                    double total = string.IsNullOrEmpty(lblTotalAmount.Text.Trim()) ? 0 : Convert.ToDouble(lblTotalAmount.Text.Trim().Replace(",", ""));
                                    double invoiced = string.IsNullOrEmpty(lblInvoiced.Text.Trim()) ? 0 : Convert.ToDouble(lblInvoiced.Text.Trim().Replace(",", ""));
                                    double pInvoiced = string.IsNullOrEmpty(lblProformaInvoiced.Text.Trim()) ? 0 : Convert.ToDouble(lblProformaInvoiced.Text.Trim().Replace(",", ""));
                                    double maxAllowedPInvoiced = invoiced > pInvoiced ? invoiced : pInvoiced;
                                    txtPayNow1.Text = GetFormattedCurrency(((total - maxAllowedPInvoiced) >= 0 ? (total - maxAllowedPInvoiced) : 0).ToString());
                                    lblBalancetoInvoice.Text = lblBalancetoInvoice.ToolTip = String.Format("{0:c}", ((total - maxAllowedPInvoiced) >= 0 ? (total - maxAllowedPInvoiced) : 0));
                                }
                            }
                            GetFieldValues(ControlsEnum.POINVOICELIST);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                        }
                        else
                        {
                            ischanged = true;
                            grdSalesList.HeaderRow.Cells[9].Visible = false;
                            grdSalesList.FooterRow.Cells[9].Visible = false;
                            foreach (GridViewRow grdRow in grdSalesList.Rows)
                            {
                                grdRow.Cells[9].Visible = false;
                                lblTotalAmount = grdRow.FindControl("lblTotalAmount") as Label;
                                lblInvoiced = grdRow.FindControl("lblInvoiced") as Label;
                                lblBalancetoInvoice = grdRow.FindControl("lblBalancetoInvoice") as Label;
                                txtPayNow1 = grdRow.FindControl("txtPayNow") as TextBox;
                                if (lblTotalAmount != null && lblInvoiced != null && lblBalancetoInvoice != null && txtPayNow1 != null)
                                {
                                    double total = string.IsNullOrEmpty(lblTotalAmount.Text.Trim()) ? 0 : Convert.ToDouble(lblTotalAmount.Text.Trim().Replace(",", ""));
                                    double invoiced = string.IsNullOrEmpty(lblInvoiced.Text.Trim()) ? 0 : Convert.ToDouble(lblInvoiced.Text.Trim().Replace(",", ""));
                                    txtPayNow1.Text = GetFormattedCurrency(((total - invoiced) >= 0 ? total - invoiced : 0).ToString());
                                    lblBalancetoInvoice.Text = lblBalancetoInvoice.ToolTip = String.Format("{0:c}", ((total - invoiced) >= 0 ? total - invoiced : 0));
                                }
                            }
                            GetFieldValues(ControlsEnum.POINVOICELIST);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                        }

                        #region Change workflow details with invoice type
                        if (!IsAdvInvHasTax && CurrPK <= 0)
                        {
                            if (!string.IsNullOrEmpty(ddlInvoiceType.SelectedValue))
                            {
                                if (Convert.ToInt32(ddlInvoiceType.SelectedValue) == Convert.ToInt32(SalesInvoiceType.Domestic))
                                    FillProcessID(3);
                                else
                                    FillProcessID(1);
                                SetCancelRef(CurrPK);
                                ucrWrkf.FillWorkFlowDetails();
                                if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.NEWMODE && ucrWrkf.HasPageTaskPermission)
                                    ucrWrkf.ViewType = 1;
                                else
                                {
                                    ucrWrkf.ViewType = 0;
                                }
                            }
                        }
                        #endregion

                        if (Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Export) && IsInvoiceGSTEnable)
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
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:

                        SelectedInvoicesCrDr = new List<long>();
                        SelectedInvoiceCrDrList = new List<long>();
                        Session[ERP.Utilities.SessionStrings.SelectedSalesInvoices] = null;
                        Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = null;
                        btnPickForReceipt.Text = GetLocalResourceObject("PickInvforReceipt").ToString();
                        btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString();

                        foreach (GridViewRow grdrow in grdSalesInvoiceList.Rows)
                        {
                            chkInvselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkInvselect.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                                Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                if (!string.IsNullOrEmpty(hdfStatus.Value) && Convert.ToInt32(hdfStatus.Value) == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_DraftedInv").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////
                                hdfDept = grdrow.FindControl("hdfDept") as HiddenField;
                                if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                                {
                                    Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                                    base.SetUserDept();
                                }

                                invType = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvType")).Value);
                                RPTTYPE = invType;
                                isCancelled = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1 ? true : false;
                                if (isCancelled)
                                {
                                    btnSave.Visible = false;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_alreadycancelled").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                else
                                {
                                    btnSave.Visible = true;
                                }

                                if (invType == Convert.ToInt32(SalesInvoiceType.Domestic))
                                    FillProcessID(3);
                                else
                                    FillProcessID(1);


                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(11);
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICETYPE);
                            SetFieldValues(ControlsEnum.INVOICETYPE);
                            GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                            GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
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

                    #region
                    case ActionsEnum.CUSTOMERTYPECHANGING:
                        GetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetFieldValues(ControlsEnum.GETCUSTOMERDETAILSBYTYPE);
                        SetBranchIDEnableDisable();
                        break;
                    #endregion

                    #region TAXSPLITUPPOPUP
                    case ActionsEnum.TAXDETAILSSPLITUP:
                        int POH_Pk = int.Parse(((LinkButton)sender).CommandArgument.ToString());
                        if (CurrPK == 0)
                        {
                            if (SalHeaderList != null)
                            {
                                SalOrderHdrListTaxSplitup = (List<SAL_ORDER_HDR>)SalHeaderList;
                                SalOrderHdrListTaxSplitup = SalOrderHdrListTaxSplitup.Where(f => f.SOH_PK == POH_Pk).ToList();
                                if (SalOrderHdrListTaxSplitup.Count > 0)
                                {
                                    SetTaxDetailsPopup(SalOrderHdrListTaxSplitup[0]);
                                }
                                else
                                {
                                    grdTaxSplitupDetails.DataSource = null;
                                    grdTaxSplitupDetails.DataBind();
                                }
                            }
                        }
                        else
                        {
                            if (InvoiceCusMapList != null)
                            {
                                List<FIN_INVOICE_CUS_TRX_MPG> finInvoice_CUS_TRX_MPGTemp = (List<FIN_INVOICE_CUS_TRX_MPG>)InvoiceCusMapList;
                                finInvoice_CUS_TRX_MPGTemp = finInvoice_CUS_TRX_MPGTemp.Where(f => f.ICM_SO_HDR == POH_Pk).ToList();
                                if (finInvoice_CUS_TRX_MPGTemp.Count > 0)
                                {
                                    SetTaxDetailsPopup(finInvoice_CUS_TRX_MPGTemp[0].SAL_ORDER_HDR);
                                }
                                else
                                {
                                    grdTaxSplitupDetails.DataSource = null;
                                    grdTaxSplitupDetails.DataBind();
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divTaxSplitupDetails]','" + GetLocalResourceObject("TaxDetails").ToString() + "','400','200');", true);

                        break;
                    #endregion
                    #region Show Popup
                    case ActionsEnum.SHOWPOPUP:
                        soPK = ((LinkButton)sender).CommandArgument;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + soPK + "&APPTYPE=" + ApplicationType.SO + "&APPSUBTYPE=") + "');", true);
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
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
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
                salesInvoiceServiceClient = null;
                CommonServiceClient = null;
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
            SPADM_APP_STATUS_CFG_GET_KV_Result wkfStatus;
            try
            {
                if (((GridView)sender).ID == "grdSalesInvoiceList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfBalamt = e.Row.FindControl("hdfBalamt") as HiddenField;
                        HiddenField hdfSalesContractType = e.Row.FindControl("hdfSalesContractType") as HiddenField;
                        LinkButton lbnBalAmt = e.Row.FindControl("lbnBalAmt") as LinkButton;
                        //Label lblBalAmt = e.Row.FindControl("lblBalAmt") as Label;


                        if (finInvoiceCusHdrList != null && finInvoiceCusHdrList.Count > 0)
                        {
                            List<FIN_INVOICE_CUS_TRX_MPG> invoiceCustrxMpglst = finInvoiceCusHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.ToList();
                            hdfSalesContractType.Value = invoiceCustrxMpglst[0].SAL_ORDER_HDR.SOH_TYPE.ToString();

                            decimal balAmount = 0;
                            balAmount = Convert.ToDecimal(finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_NET_TC) - (Convert.ToDecimal(finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_RCVD_TC) + Convert.ToDecimal(finInvoiceCusHdrList[e.Row.RowIndex].ICH_AMOUNT_CN_TC));
                            lbnBalAmt.Text = String.Format("{0:c}", balAmount < 0 ? 0 : balAmount);
                            lbnBalAmt.ToolTip = String.Format("{0:c}", balAmount);
                            //lblBalAmt.Text = String.Format("{0:c}", balAmount);
                            //lblBalAmt.ToolTip = String.Format("{0:c}", balAmount);
                        }
                    }
                }
                if (((GridView)sender).ID == "grdSalesList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        string SOdate = "";
                        //Label lblSONo = e.Row.FindControl("lblSONo") as Label;
                        LinkButton lnkSoNo = e.Row.FindControl("lnkSoNo") as LinkButton;
                        Label lblSODate = e.Row.FindControl("lblSODate") as Label;
                        // Label lblPlantCode = e.Row.FindControl("lblPlantCode") as Label;
                        // lblPlantCode.Visible = GetConfigData().IsMultiplePlant;
                        Label lblCustomerInv = e.Row.FindControl("lblCustomerInv") as Label;
                        Label lblCurrency = e.Row.FindControl("lblCurrency") as Label;
                        Label lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
                        Label lblInvoiced = e.Row.FindControl("lblInvoiced") as Label;
                        Label lblSOTax = e.Row.FindControl("lblSOTax") as Label;
                        Label lblProformaInvoiced = e.Row.FindControl("lblProformaInvoiced") as Label;
                        Label lblAdj = e.Row.FindControl("lblAdj") as Label;
                        HiddenField hdfInvoiced = e.Row.FindControl("hdfInvoiced") as HiddenField;
                        HiddenField hdfSODiscount = e.Row.FindControl("hdfSODiscount") as HiddenField;
                        Label lblOtherAmount = e.Row.FindControl("lblOtherAmount") as Label;
                        HiddenField hdfProformaInvoiced = e.Row.FindControl("hdfProformaInvoiced") as HiddenField;
                        Label lblBalancetoInvoice = e.Row.FindControl("lblBalancetoInvoice") as Label;
                        TextBox txtPayNow = e.Row.FindControl("txtPayNow") as TextBox;
                        HiddenField hdfSONumber = e.Row.FindControl("hdfSONumber") as HiddenField;
                        HiddenField hdfPayNow = e.Row.FindControl("hdfPayNow") as HiddenField;
                        Button lnkRemove = e.Row.FindControl("lnkRemove") as Button;
                        // Label lblTax = e.Row.FindControl("lblTax") as Label;
                        LinkButton lblTax = e.Row.FindControl("lblTax") as LinkButton;

                        Label lblDiscount = e.Row.FindControl("lblDiscount") as Label;
                        Label lblGrossAmount = e.Row.FindControl("lblGrossAmount") as Label;
                        TextBox txtOthercharges = e.Row.FindControl("txtOthercharges") as TextBox;
                        HiddenField hdfOtherchargeOLD = e.Row.FindControl("hdfOtherchargeOLD") as HiddenField;
                        HiddenField hdfPriceAdjustment = e.Row.FindControl("hdfPriceAdjustment") as HiddenField;
                        ImageButton imgTax = e.Row.FindControl("imgTax") as ImageButton;
                        decimal InvoicedAmount = 0;
                        decimal ProfomaInvAmnt = 0;
                        decimal InvoicedOtherCharge = 0;
                        decimal ProfomaInvoicedOtherCharge = 0;
                        decimal AdvInvAmount = 0;

                        if (SalOrderHdrList != null && SalOrderHdrList.Count > 0)
                        {
                            #region SalOrderHdrList Region
                            lnkRemove.CommandArgument = SalOrderHdrList[e.Row.RowIndex].SOH_PK.ToString();
                            hdfSONumber.Value = SalOrderHdrList[e.Row.RowIndex].SOH_PK.ToString();
                            SOdate = SalOrderHdrList[e.Row.RowIndex].SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lnkSoNo.Text = SalOrderHdrList[e.Row.RowIndex].SOH_NO;
                            lnkSoNo.ToolTip = SalOrderHdrList[e.Row.RowIndex].SOH_NO + "  " + SOdate;
                            lnkSoNo.CommandArgument = SalOrderHdrList[e.Row.RowIndex].SOH_PK.ToString();
                            //lblSONo.Text = SalOrderHdrList[e.Row.RowIndex].SOH_NO;
                            //lblSONo.ToolTip = SalOrderHdrList[e.Row.RowIndex].SOH_NO + "  " + SOdate;
                            lblSODate.Text = SalOrderHdrList[e.Row.RowIndex].SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = SalOrderHdrList[e.Row.RowIndex].SOH_DATE.ToString(Resources.Constants.DateFormatShort);

                            // lblPlantCode.Text = SalOrderHdrList[e.Row.RowIndex].ADM_COMPANY_MST.CMP_DISPLAY_CODE.ToString();
                            //lblPlantCode.ToolTip = SalOrderHdrList[e.Row.RowIndex].ADM_COMPANY_MST.CMP_DISPLAY_CODE.ToString();

                            lblCustomerInv.Text = ERP.Utilities.CommonFunctions.GetShortString(SalOrderHdrList[e.Row.RowIndex].CRM_CUSTOMER_MST.CUS_NAME, 10);
                            lblCustomerInv.ToolTip = SalOrderHdrList[e.Row.RowIndex].CRM_CUSTOMER_MST.CUS_NAME;
                            lblCurrency.Text = SalOrderHdrList[e.Row.RowIndex].ADM_CURRENCY_MST.CUR_CODE;
                            txtCurrr.Text = SalOrderHdrList[e.Row.RowIndex].ADM_CURRENCY_MST.CUR_CODE;
                            lblCurrency.ToolTip = SalOrderHdrList[e.Row.RowIndex].ADM_CURRENCY_MST.CUR_CODE;

                            decimal lineItemTax = SalOrderHdrList[e.Row.RowIndex].SAL_ORDER_DTL.Sum(ss => ss.SOD_TAX);
                            decimal lineItemDiscount = SalOrderHdrList[e.Row.RowIndex].SAL_ORDER_DTL.Sum(ss => ss.SOD_DISCOUNT);
                            //GrossTotal 
                            //lblGrossAmount.Text = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_AMT);
                            // lblGrossAmount.ToolTip = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_AMT);
                            decimal GrossAmount = 0;
                            if (SalOrderHdrList[e.Row.RowIndex].SAL_ORDER_DTL != null)
                            {
                                foreach (SAL_ORDER_DTL dtlObj in SalOrderHdrList[e.Row.RowIndex].SAL_ORDER_DTL)
                                {
                                    GrossAmount = GrossAmount + dtlObj.SOD_AMOUNT;
                                }
                            }
                            lblGrossAmount.Text = String.Format("{0:c}", GrossAmount);
                            lblGrossAmount.ToolTip = String.Format("{0:c}", GrossAmount);
                            //End GrossTotal

                            //for handling multiple SC
                            //lblInvoiced.Text = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_AMT_INVOICED);
                            //hdfInvoiced.Value = SalOrderHdrList[e.Row.RowIndex].SOH_AMT_INVOICED.ToString();

                            //For Resolving Bug ID:2675 (Deducting AdvanceDeduct amount from each Invoice Amount)
                            //InvoicedAmount = SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT);                            

                            //Issue: In the case of Performa Advance Invoice,Invoice amount shows incorrect.                            
                            //InvoicedAmount = SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.FIN_INVOICE_CUS_HDR.ICH_CATEGORY != (byte)SalesInvoiceCategory.Advanced).Sum(c => c.ICM_AMOUNT - (c.SAL_ORDER_HDR.FIN_INVOICE_CUS_ADV_DED_DTL.Where(d => d.IAD_INVOICE_HDR == c.ICM_INVOICE_HDR).Sum(alloc => alloc.IAD_AMOUNT - alloc.IAD_OTHER_AMOUNT - alloc.IAD_TAX_AMOUNT)) + c.ICM_TAX_AMOUNT - c.ICM_DISCOUNT_AMOUNT + c.ICM_ADJUST_AMOUNT + c.ICM_OTHER_AMOUNT);
                            if (SalOrderHdrList[e.Row.RowIndex].SOH_TYPE == (byte)SalesInvoiceType.Export)
                            {
                                InvoicedAmount = SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.FIN_INVOICE_CUS_HDR.ICH_CATEGORY != (byte)SalesInvoiceCategory.Advanced).Sum(c => c.ICM_AMOUNT + c.ICM_TAX_AMOUNT - c.ICM_DISCOUNT_AMOUNT + c.ICM_ADJUST_AMOUNT + c.ICM_OTHER_AMOUNT);
                            }
                            else
                            {
                                // Deducting AdvanceDeduct amount from each Invoice Amount
                                InvoicedAmount = SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.FIN_INVOICE_CUS_HDR.ICH_CATEGORY != (byte)SalesInvoiceCategory.Advanced).Sum(c => c.ICM_AMOUNT - (c.SAL_ORDER_HDR.FIN_INVOICE_CUS_ADV_DED_DTL.Where(d => d.IAD_INVOICE_HDR == c.ICM_INVOICE_HDR).Sum(alloc => alloc.IAD_AMOUNT - alloc.IAD_OTHER_AMOUNT - alloc.IAD_TAX_AMOUNT)) + c.ICM_TAX_AMOUNT - c.ICM_DISCOUNT_AMOUNT + c.ICM_ADJUST_AMOUNT + c.ICM_OTHER_AMOUNT);
                            }
                            AdvInvAmount = SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.FIN_INVOICE_CUS_HDR.ICH_CATEGORY == (byte)SalesInvoiceCategory.Advanced).Sum(c => c.ICM_AMOUNT);
                            InvoicedAmount += AdvInvAmount;


                            //InvoicedOtherCharge = 0;
                            //InvoicedOtherCharge = SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TAX_HDR.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 && r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 && r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.ISH_TAX_CATEGORY == (byte)TaxType.Shipping).Sum(tx => tx.ISH_TAX_AMT);
                            //InvoicedAmount += InvoicedOtherCharge;

                            lblInvoiced.Text = String.Format("{0:c}", InvoicedAmount);
                            hdfInvoiced.Value = InvoicedAmount.ToString();
                            //lblProformaInvoiced.Text = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_AMT_PINVOICED);
                            //hdfProformaInvoiced.Value = SalOrderHdrList[e.Row.RowIndex].SOH_AMT_PINVOICED.ToString();
                            ProfomaInvAmnt = SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE == (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT);

                            ProfomaInvoicedOtherCharge = 0;
                            ProfomaInvoicedOtherCharge = SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TAX_HDR.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 && r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 && r.FIN_INVOICE_CUS_HDR.ICH_TYPE == (byte)SalesInvoiceType.Proforma && r.ISH_TAX_CATEGORY == (byte)TaxType.Shipping).Sum(tx => tx.ISH_TAX_AMT);
                            ProfomaInvAmnt += ProfomaInvoicedOtherCharge;

                            lblProformaInvoiced.Text = String.Format("{0:c}", ProfomaInvAmnt);
                            hdfProformaInvoiced.Value = ProfomaInvAmnt.ToString();


                            //if (SalOrderHdrList[0].SOH_TYPE != (byte)SalesInvoiceType.Proforma)
                            byte soType = string.IsNullOrEmpty(ddlInvoiceType.SelectedValue) ? SalOrderHdrList[0].SOH_TYPE : Convert.ToByte(ddlInvoiceType.SelectedValue);
                            decimal balAmt = SalOrderHdrList[e.Row.RowIndex].SOH_NET_AMOUNT - decimal.Parse(lblInvoiced.Text.Replace(",", ""));
                            decimal ProBalAmt = SalOrderHdrList[e.Row.RowIndex].SOH_NET_AMOUNT - decimal.Parse(lblProformaInvoiced.Text.Replace(",", "")) - decimal.Parse(lblInvoiced.Text.Replace(",", ""));
                            if (soType != (byte)SalesInvoiceType.Proforma)
                            {
                                lblBalancetoInvoice.Text = String.Format("{0:c}", balAmt);
                            }
                            else
                            {
                                lblBalancetoInvoice.Text = String.Format("{0:c}", balAmt >= ProBalAmt ? ProBalAmt : balAmt);
                            }

                            //lblInvoiced.Text = SalOrderHdrList[e.Row.RowIndex].SOH_AMT_INVOICED.ToString();
                            //lblProformaInvoiced.Text = SalOrderHdrList[e.Row.RowIndex].SOH_AMT_PINVOICED.ToString();
                            //lblBalancetoInvoice.Text = (SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_AMT - decimal.Parse(lblInvoiced.Text)).ToString();
                            lblBalancetoInvoice.Text = Convert.ToDecimal(lblBalancetoInvoice.Text.Replace(",", "")) < 0 ? String.Format("{0:c}", 0) : lblBalancetoInvoice.Text;
                            if (dicTempAmount != null && dicTempAmount.Count > 0 && dicTempAmount.ContainsKey(hdfSONumber.Value))
                            {
                                txtPayNow.Text = dicTempAmount.SingleOrDefault(aa => aa.Key == hdfSONumber.Value).Value.ToString();
                            }
                            else
                            {
                                txtPayNow.Text = GetFormattedCurrency(Convert.ToDecimal(lblBalancetoInvoice.Text.Replace(",", "")));
                            }
                            hdfPayNow.Value = txtPayNow.Text;
                            txtInvdate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            hdfInvdate.Value = DateTime.Now.ToString();
                            txtPaybydate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            hdfPaybydate.Value = DateTime.Now.ToString();
                            lblTax.Text = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_TAX + lineItemTax);
                            lblTax.ToolTip = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_TAX + lineItemTax);
                            lblDiscount.Text = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_DISCOUNT + lineItemDiscount);
                            lblDiscount.ToolTip = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_DISCOUNT + lineItemDiscount);

                            // Setting Tax Details popup  

                            if (SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_TAX + lineItemTax > 0)
                            {
                                lblTax.CommandArgument = SalOrderHdrList[e.Row.RowIndex].SOH_PK.ToString();
                            }
                            else
                            {
                                lblTax.CommandArgument = "0";
                            }

                            lblTotalAmount.Text = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_NET_AMOUNT);
                            lblTotalAmount.ToolTip = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_NET_AMOUNT);

                            lblOtherAmount.Text = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_SHIP_CHARGE);
                            lblOtherAmount.ToolTip = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_SHIP_CHARGE);
                            //hdfOtherchargeOLD.Value = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(tm => tm.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0).Sum(tm => tm.ICM_OTHER_AMOUNT));
                            hdfOtherchargeOLD.Value = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].FIN_INVOICE_CUS_TRX_MPG.Where(tm => tm.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0).Sum(tm => tm.ICM_OTHER_AMOUNT - tm.SAL_ORDER_HDR.FIN_INVOICE_CUS_ADV_DED_DTL.Where(d => d.IAD_INVOICE_HDR == tm.ICM_INVOICE_HDR).Sum(amt => amt.IAD_OTHER_AMOUNT)));

                            txtOthercharges.Text = GetFormattedCurrency(Convert.ToDecimal(SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_SHIP_CHARGE) - Convert.ToDecimal(hdfOtherchargeOLD.Value));
                            hdfPriceAdjustment.Value = SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_ADJUST.ToString();
                            lblAdj.Text = lblAdj.ToolTip = String.Format("{0:c}", SalOrderHdrList[e.Row.RowIndex].SOH_TOTAL_ADJUST);
                            #endregion
                        }

                        if (finInvoiceCusTrxMpgList != null && finInvoiceCusTrxMpgList.Count > 0)
                        {

                            #region finInvoiceCusTrxMpgList
                            lnkRemove.CommandArgument = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            hdfSONumber.Value = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();


                            SOdate = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);

                            lnkSoNo.Text = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            lnkSoNo.ToolTip = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO + "  " + SOdate;
                            lnkSoNo.CommandArgument = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            //lblSONo.Text = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO;
                            //lblSONo.ToolTip = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NO + "  " + SOdate;

                            lblSODate.Text = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);
                            lblSODate.ToolTip = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_DATE.ToString(Resources.Constants.DateFormatShort);

                            // lblPlantCode.Text = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_COMPANY_MST.CMP_DISPLAY_CODE.ToString();
                            // lblPlantCode.ToolTip = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_COMPANY_MST.CMP_DISPLAY_CODE.ToString();

                            lblCustomerInv.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.CRM_CUSTOMER_MST.CUS_NAME, 10);
                            lblCustomerInv.ToolTip = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.CRM_CUSTOMER_MST.CUS_NAME;
                            lblCurrency.Text = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_CURRENCY_MST.CUR_CODE;
                            txtCurrr.Text = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_CURRENCY_MST.CUR_CODE;
                            lblCurrency.ToolTip = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.ADM_CURRENCY_MST.CUR_CODE;

                            decimal lineItemTax = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SAL_ORDER_DTL.Sum(sd => sd.SOD_TAX);
                            decimal lineItemDiscount = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SAL_ORDER_DTL.Sum(sd => sd.SOD_DISCOUNT);
                            //GrossTotal 
                            //lblGrossAmount.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_AMT - lineItemTax);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_AMT - lineItemTax);
                            decimal GrossAmount = 0;
                            if (finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SAL_ORDER_DTL != null)
                            {
                                foreach (SAL_ORDER_DTL dtlObj in finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SAL_ORDER_DTL)
                                {
                                    GrossAmount = GrossAmount + dtlObj.SOD_AMOUNT;
                                }
                            }
                            lblGrossAmount.Text = String.Format("{0:c}", GrossAmount);
                            lblGrossAmount.ToolTip = String.Format("{0:c}", GrossAmount);
                            //End GrossTotal

                            lblOtherAmount.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_SHIP_CHARGE);
                            lblOtherAmount.ToolTip = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_SHIP_CHARGE);


                            if (dicTempAmount != null && dicTempAmount.Count > 0 && dicTempAmount.ContainsKey(hdfSONumber.Value))
                            {
                                txtPayNow.Text = dicTempAmount.SingleOrDefault(aa => aa.Key == hdfSONumber.Value).Value.ToString();
                            }
                            else
                            {
                                txtPayNow.Text = GetFormattedCurrency(finInvoiceCusTrxMpgList[e.Row.RowIndex].ICM_AMOUNT.ToString());
                            }
                            hdfPayNow.Value = txtPayNow.Text;
                            lblTotalAmount.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT);
                            lblTotalAmount.ToolTip = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT);


                            lblSOTax.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].ICM_TAX_AMOUNT);
                            //hdfSODiscount.Value = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].ICM_DISCOUNT_AMOUNT);
                            txtOthercharges.Text = GetFormattedCurrency(finInvoiceCusTrxMpgList[e.Row.RowIndex].ICM_OTHER_AMOUNT);

                            decimal balAmt = 0;
                            decimal ProBalAmt = 0;

                            ////
                            if (finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma)
                            {
                                #region Not Equal To Proforma
                                // For handling multiple SC
                                //lblInvoiced.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED);

                                //For Resolving Bug ID:2675 (Deducting AdvanceDeduct amount from each Invoice Amount)                               
                                //InvoicedAmount = finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT) - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT);
                                InvoicedAmount = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.FIN_INVOICE_CUS_HDR.ICH_CATEGORY != (byte)SalesInvoiceCategory.Advanced).Sum(c => c.ICM_AMOUNT - (c.SAL_ORDER_HDR.FIN_INVOICE_CUS_ADV_DED_DTL.Where(d => d.IAD_INVOICE_HDR == c.ICM_INVOICE_HDR).Sum(amt => amt.IAD_AMOUNT - amt.IAD_TAX_AMOUNT - amt.IAD_OTHER_AMOUNT)) + c.ICM_TAX_AMOUNT - c.ICM_DISCOUNT_AMOUNT + c.ICM_ADJUST_AMOUNT + c.ICM_OTHER_AMOUNT);

                                AdvInvAmount = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.FIN_INVOICE_CUS_HDR.ICH_CATEGORY == (byte)SalesInvoiceCategory.Advanced).Sum(c => c.ICM_AMOUNT);
                                InvoicedAmount += AdvInvAmount;
                                InvoicedAmount = finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? InvoicedAmount
                                   - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : InvoicedAmount;

                                //InvoicedOtherCharge = finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.ISH_TAX_CATEGORY == (byte)TaxType.Shipping).Sum(c => c.ISH_TAX_AMT)
                                //    - (WkfStatus > 0 ? decimal.Parse(txtOthercharges.Text) : 0) : finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.ISH_TAX_CATEGORY == (byte)TaxType.Shipping).Sum(c => c.ISH_TAX_AMT);
                                //InvoicedAmount += InvoicedOtherCharge;

                                ProfomaInvAmnt = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE == (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT);

                                ProfomaInvoicedOtherCharge = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE == (byte)SalesInvoiceType.Proforma && r.ISH_TAX_CATEGORY == (byte)TaxType.Shipping).Sum(c => c.ISH_TAX_AMT);
                                ProfomaInvAmnt += ProfomaInvoicedOtherCharge;

                                lblInvoiced.Text = String.Format("{0:c}", InvoicedAmount);
                                //lblProformaInvoiced.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_PINVOICED);// - decimal.Parse(txtPayNow.Text)).ToString();
                                lblProformaInvoiced.Text = String.Format("{0:c}", ProfomaInvAmnt);// - decimal.Parse(txtPayNow.Text)).ToString();

                                balAmt = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT - decimal.Parse(lblInvoiced.Text.Replace(",", ""));
                                ProBalAmt = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT - decimal.Parse(lblProformaInvoiced.Text.Replace(",", ""));

                                lblBalancetoInvoice.Text = String.Format("{0:c}", balAmt); //lblBalancetoInvoice.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT - decimal.Parse(lblInvoiced.Text.Replace(",", "")));

                                //hdfInvoiced.Value = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED); //finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED.ToString();
                                hdfInvoiced.Value = String.Format("{0:c}", InvoicedAmount); //finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED.ToString();
                                //hdfProformaInvoiced.Value = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_PINVOICED);// - decimal.Parse(txtPayNow.Text)).ToString();// finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_PINVOICED.ToString();
                                hdfProformaInvoiced.Value = String.Format("{0:c}", ProfomaInvAmnt);// - decimal.Parse(txtPayNow.Text)).ToString();// finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_PINVOICED.ToString(); 
                                #endregion

                            }
                            else
                            {
                                #region Proforma
                                //lblInvoiced.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED);// - decimal.Parse(txtPayNow.Text)).ToString();
                                //lblProformaInvoiced.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_PINVOICED - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_PINVOICED);

                                // InvoicedAmount = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT);
                                InvoicedAmount = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT - (c.SAL_ORDER_HDR.FIN_INVOICE_CUS_ADV_DED_DTL.Where(d => d.IAD_INVOICE_HDR == c.ICM_INVOICE_HDR).Sum(alloc => alloc.IAD_AMOUNT - alloc.IAD_OTHER_AMOUNT - alloc.IAD_TAX_AMOUNT)) + c.ICM_TAX_AMOUNT - c.ICM_DISCOUNT_AMOUNT + c.ICM_ADJUST_AMOUNT + c.ICM_OTHER_AMOUNT);

                                InvoicedOtherCharge = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE != (byte)SalesInvoiceType.Proforma && r.ISH_TAX_CATEGORY == (byte)TaxType.Shipping).Sum(c => c.ISH_TAX_AMT);
                                InvoicedAmount += InvoicedOtherCharge;

                                ProfomaInvAmnt = finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE == (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT) - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE == (byte)SalesInvoiceType.Proforma).Sum(c => c.ICM_AMOUNT);
                                ProfomaInvoicedOtherCharge = finInvoiceCusTrxMpgList[e.Row.RowIndex].FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 ? finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE == (byte)SalesInvoiceType.Proforma && r.ISH_TAX_CATEGORY == (byte)TaxType.Shipping).Sum(c => c.ISH_TAX_AMT) - (WkfStatus > 0 ? decimal.Parse(txtOthercharges.Text) : 0) : finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TAX_HDR.Where(r => r.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0 & r.FIN_INVOICE_CUS_HDR.ICH_STATUS > 0 & r.FIN_INVOICE_CUS_HDR.ICH_TYPE == (byte)SalesInvoiceType.Proforma && r.ISH_TAX_CATEGORY == (byte)TaxType.Shipping).Sum(c => c.ISH_TAX_AMT);


                                lblInvoiced.Text = String.Format("{0:c}", InvoicedAmount);// - decimal.Parse(txtPayNow.Text)).ToString();
                                lblProformaInvoiced.Text = String.Format("{0:c}", ProfomaInvAmnt);


                                balAmt = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT - decimal.Parse(lblInvoiced.Text.Replace(",", ""));
                                ProBalAmt = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT - decimal.Parse(lblProformaInvoiced.Text.Replace(",", "")) - decimal.Parse(lblInvoiced.Text.Replace(",", ""));

                                lblBalancetoInvoice.Text = String.Format("{0:c}", balAmt >= ProBalAmt ? ProBalAmt : balAmt);//  lblBalancetoInvoice.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_NET_AMOUNT - decimal.Parse(lblProformaInvoiced.Text.Replace(",", "")));

                                hdfInvoiced.Value = String.Format("{0:c}", InvoicedAmount);// - decimal.Parse(txtPayNow.Text)).ToString();//finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED.ToString();
                                hdfProformaInvoiced.Value = String.Format("{0:c}", ProfomaInvAmnt);// finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_PINVOICED.ToString(); 
                                #endregion
                            }

                            //lblInvoiced.Text = (finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_INVOICED - decimal.Parse(txtPayNow.Text)).ToString();
                            //lblProformaInvoiced.Text = (finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_AMT_PINVOICED - decimal.Parse(txtPayNow.Text)).ToString();

                            //lblBalancetoInvoice.Text = (finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_AMT - decimal.Parse(lblInvoiced.Text)).ToString();
                            ////
                            lblBalancetoInvoice.Text = Convert.ToDecimal(lblBalancetoInvoice.Text.Replace(",", "")) < 0 ? String.Format("{0:c}", 0) : lblBalancetoInvoice.Text;
                            lblTax.Text = String.Format("{0:c}", lineItemTax + finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_TAX);
                            lblTax.ToolTip = String.Format("{0:c}", lineItemTax + finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_TAX);
                            lblDiscount.Text = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_DISCOUNT + lineItemDiscount);
                            lblDiscount.ToolTip = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_DISCOUNT + lineItemDiscount);

                            hdfOtherchargeOLD.Value = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.FIN_INVOICE_CUS_TRX_MPG.Where(tm => tm.FIN_INVOICE_CUS_HDR.ICH_DEL_STATUS == 0).Sum(tm => tm.ICM_OTHER_AMOUNT - tm.SAL_ORDER_HDR.FIN_INVOICE_CUS_ADV_DED_DTL.Where(d => d.IAD_INVOICE_HDR == tm.ICM_INVOICE_HDR).Sum(amt => amt.IAD_OTHER_AMOUNT)));

                            // Setting Tax Details popup   
                            if (lineItemTax + finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_TAX > 0)
                            {
                                lblTax.CommandArgument = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_PK.ToString();
                            }
                            else
                            {
                                lblTax.CommandArgument = "0";
                            }


                            hdfPriceAdjustment.Value = finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_ADJUST.ToString();
                            lblAdj.Text = lblAdj.ToolTip = String.Format("{0:c}", finInvoiceCusTrxMpgList[e.Row.RowIndex].SAL_ORDER_HDR.SOH_TOTAL_ADJUST);
                            #endregion
                        }

                        lblInvoiced.Text = String.Format("{0:c}", decimal.Parse(lblInvoiced.Text.Replace(",", "")));
                        lblProformaInvoiced.Text = String.Format("{0:c}", decimal.Parse(lblProformaInvoiced.Text.Replace(",", "")));
                        lblInvoiced.ToolTip = lblInvoiced.Text;
                        lblProformaInvoiced.ToolTip = lblProformaInvoiced.Text;
                        lblBalancetoInvoice.Text = String.Format("{0:c}", decimal.Parse(lblBalancetoInvoice.Text.Replace(",", "")));
                        lblBalancetoInvoice.ToolTip = lblBalancetoInvoice.Text;
                        txtPayNow.Text = Math.Round(decimal.Parse(txtPayNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        txtPayNow.Text = GetFormattedCurrency(Convert.ToDecimal(txtPayNow.Text) < 0 ? "0" : txtPayNow.Text);
                        if (ddlInvoiceType.SelectedValue != "-1" && Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                        {
                            e.Row.Cells[10].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[10].Visible = false;
                        }
                        if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) && IsAdvInvHasTax)
                        {
                            e.Row.Cells[14].Visible = true;
                            e.Row.Cells[15].Visible = true;
                            DivtaxFooter.Visible = true;
                            DivDiscFooter.Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[14].Visible = false;
                            e.Row.Cells[15].Visible = false;
                            DivtaxFooter.Visible = false;
                            DivDiscFooter.Visible = false;
                        }

                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        Label lblTotalPayNowFooter = e.Row.FindControl("lblTotalPayNowFooter") as Label;

                        decimal Amount = 0;
                        decimal TaxAmount = 0;
                        decimal DiscAmount = 0;
                        decimal InvAmount = 0;
                        decimal NetAmount = 0;
                        decimal TotalPayNow = 0;

                        if (finInvoiceCusTrxMpgList != null && finInvoiceCusTrxMpgList.Count > 0)
                        {
                            Amount = decimal.Parse(finInvoiceCusTrxMpgList.Sum(iv => iv.ICM_AMOUNT).ToString());
                            TaxAmount = decimal.Parse(finInvoiceCusTrxMpgList.Sum(iv => iv.SAL_ORDER_HDR.SOH_TOTAL_TAX).ToString());
                            DiscAmount = decimal.Parse(finInvoiceCusTrxMpgList.Sum(iv => iv.SAL_ORDER_HDR.SOH_TOTAL_DISCOUNT).ToString());
                            NetAmount = Amount;
                        }
                        if (SalOrderHdrList != null && SalOrderHdrList.Count > 0)
                        {
                            Amount = decimal.Parse(SalOrderHdrList.Sum(so => so.SOH_TOTAL_AMT).ToString()) -
                                     decimal.Parse(SalOrderHdrList.Sum(so => so.SOH_AMT_INVOICED).ToString());
                            TaxAmount = decimal.Parse(SalOrderHdrList.Sum(so => so.SOH_TOTAL_TAX).ToString());
                            DiscAmount = decimal.Parse(SalOrderHdrList.Sum(so => so.SOH_TOTAL_DISCOUNT).ToString());
                        }

                        TotalPayNow = Amount + TaxAmount - DiscAmount;

                        hdfTaxAmt.Value = TaxAmount.ToString();
                        hdfDiscAmt.Value = DiscAmount.ToString();


                        decimal Tax = 0;
                        decimal Discount = 0;
                        decimal.TryParse(txtTaxAmount.Text, out Tax);
                        decimal.TryParse(txtDiscount.Text, out Discount);

                        //InvAmount = NetAmount - Tax + Discount;
                        NetAmount = Amount;// +Tax - Discount;

                        //lblTotalPayNowFooter.Text = Amount.ToString();
                        //lblTotalPayNowFooter.Text = Convert.ToDecimal(lblTotalPayNowFooter.Text) < 0 ? "0" : lblTotalPayNowFooter.Text;
                        lblTotalPayNowFooter.Text = lblTotalPayNowFooter.ToolTip = Amount < 0 ? "0" : GetFormattedCurrencyWithSeperator(Amount);

                        txtNetAmount.Text = GetFormattedCurrency(NetAmount);
                        txtInvoiceAmt.Text = GetFormattedCurrency(Amount < 0 ? 0 : Amount);
                        txtTaxAmount.Text = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? GetFormattedCurrency(Tax) : "0";
                        txtDiscount.Text = GetFormattedCurrency(Discount);

                        txtTaxAmount.Text = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? GetFormattedCurrency(Math.Round(decimal.Parse(txtTaxAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()) : "0";
                        txtNetAmount.Text = GetFormattedCurrency(Math.Round(decimal.Parse(txtNetAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString());
                        txtDiscount.Text = SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) ? GetFormattedCurrency(Math.Round(decimal.Parse(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString()) : "0";
                        txtInvoiceAmt.Text = GetFormattedCurrency(Math.Round(decimal.Parse(txtInvoiceAmt.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString());

                        //lblTotalPayNowFooter.Text = lblTotalPayNowFooter.ToolTip = String.Format("{0:c}", decimal.Parse(lblTotalPayNowFooter.Text.Replace(",", "")));
                        hdfTotalPayNowFooter.Value = Amount < 0 ? "0" : Amount.ToString();

                        if (ddlInvoiceType.SelectedValue != "-1" && Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                        {
                            e.Row.Cells[10].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[10].Visible = false;
                        }
                        if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) && IsAdvInvHasTax)
                        {
                            e.Row.Cells[14].Visible = true;
                            e.Row.Cells[15].Visible = true;
                            DivtaxFooter.Visible = true;
                            DivDiscFooter.Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[14].Visible = false;
                            e.Row.Cells[15].Visible = false;
                            DivtaxFooter.Visible = false;
                            DivDiscFooter.Visible = false;
                        }

                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        if (ddlInvoiceType.SelectedValue != "-1" && Convert.ToByte(ddlInvoiceType.SelectedValue) == Convert.ToByte(SalesInvoiceType.Proforma))
                        {
                            e.Row.Cells[10].Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[10].Visible = false;
                        }
                        if (SaleOrderType == Convert.ToByte(SalesInvoiceType.Domestic) && IsAdvInvHasTax)
                        {
                            e.Row.Cells[14].Visible = true;
                            e.Row.Cells[15].Visible = true;
                            DivtaxFooter.Visible = true;
                            DivDiscFooter.Visible = true;
                        }
                        else
                        {
                            e.Row.Cells[14].Visible = false;
                            e.Row.Cells[15].Visible = false;
                            DivtaxFooter.Visible = false;
                            DivDiscFooter.Visible = false;
                        }

                    }
                }
                else if (((GridView)sender).ID == "grdSalesInvoiceList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;

                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;
                        HiddenField hdfInvType = e.Row.FindControl("hdfInvType") as HiddenField;
                        Label lblInvType = e.Row.FindControl("lblInvType") as Label;
                        lblInvType.Text = ERP.Utilities.CommonFunctions.GetShortString(admConfigMstList.SingleOrDefault(c => c.CFG_VALUE == Convert.ToInt16(hdfInvType.Value)).CFG_DATA, 3, "");
                        lblInvType.ToolTip = admConfigMstList.SingleOrDefault(c => c.CFG_VALUE == Convert.ToInt16(hdfInvType.Value)).CFG_DATA;
                        short appstatus = Convert.ToInt16(hdfApproved.Value);
                        if (workflowStatusList != null && workflowStatusList.Count > 0)
                        {
                            wkfStatus = workflowStatusList.SingleOrDefault(aa => aa.ASC_VALUE == appstatus);
                            if (wkfStatus != null)
                            {
                                imgApproved.ToolTip = wkfStatus.ASC_NAME;
                                imgApproved.CssClass = wkfStatus.ASC_CSS_CLASS;
                            }
                        }

                        if (hdfPosted != null)
                        {
                            if (Convert.ToBoolean(hdfPosted.Value) == true)
                            {
                                imgPosted.CssClass = GetLocalResourceObject("posted").ToString();
                                imgPosted.ToolTip = Resources.Captions.Posted;
                            }
                            else
                            {
                                imgPosted.CssClass = GetLocalResourceObject("unposted").ToString();
                                imgPosted.ToolTip = Resources.Captions.NotPosted;
                            }
                        }

                        if (!string.IsNullOrEmpty(hdfPostingSettings.Value) && hdfPostingSettings.Value.Equals("0"))
                        {
                            e.Row.Cells[13].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {
                        if (!string.IsNullOrEmpty(hdfPostingSettings.Value) && hdfPostingSettings.Value.Equals("0"))
                        {
                            e.Row.Cells[13].Visible = false;
                        }
                    }
                    else if (e.Row.RowType == DataControlRowType.Footer)
                    {
                        if (!string.IsNullOrEmpty(hdfPostingSettings.Value) && hdfPostingSettings.Value.Equals("0"))
                        {
                            e.Row.Cells[13].Visible = false;
                        }
                    }
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
            uclPaging.CurrentPage = 1;
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            btnPrint.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);
            btnAlert.PreRender += new EventHandler(btnAction_PreRender);
            btnListPrint.PreRender += new EventHandler(btnAction_PreRender);

            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForReceipt.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);

            //lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnbDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            //lnbSalesInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAdvanceInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lbnSalesReceipt.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);
            //lnbMiscellaneous.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            btnInActive.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteOK.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);


            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteNew.Load += new EventHandler(btnAction_Load);
            btnPrint.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);
            btnAlert.Load += new EventHandler(btnAction_Load);
            btnListPrint.Load += new EventHandler(btnAction_Load);

            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            btnPickForReceipt.Load += new EventHandler(btnAction_Load);
            btnPickForCrDrNote.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);

            //lbnSOListing.Load += new EventHandler(btnAction_Load);
            //lnbDeliveryOrder.Load += new EventHandler(btnAction_Load);
            //lnbSalesInvoice.Load += new EventHandler(btnAction_Load);
            //lnbAdvanceInvoice.Load += new EventHandler(btnAction_Load);
            //lbnSalesReceipt.Load += new EventHandler(btnAction_Load);
            //lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //lnbAcPayables.Load += new EventHandler(btnAction_Load);
            //lnbMiscellaneous.Load += new EventHandler(btnAction_Load);

            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);

            btnInActive.Load += new EventHandler(btnAction_Load);
            btnDeleteOK.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
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
                PageIndex = uclPaging.CurrentPage.ToString();
                GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                GetFieldValues(ControlsEnum.INVOICEHDR);
                SetFieldValues(ControlsEnum.INVOICEHDR);
                EnableDisableButtons(e.TotalPages);
                EntryStatus = EntryStatus.LISTMODE;

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
        private void EnableDisableButtons(int iTotalPages)
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
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "RemoveLink", "$(document).ready(function () { RemoveBalAmntHyperLink();});", true);

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

        /// <summary>
        /// 
        /// </summary>
        private void ConfigurationSettings()
        {
            DataTable dt = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CURRENCY");
            if (dt != null && dt.Rows.Count > 0)
            {
                IsBizUnitCur = dt.Rows[0]["ACF_VALUE"].ToString() == "1" ? false : true;
            }

            #region Advance Invoice Tax Settings
            DataTable dtTax = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("ADVANCE INVOICE SETTINGS", "TAX");
            if (dtTax != null && dtTax.Rows.Count > 0)
            {
                IsAdvInvHasTax = dtTax.Rows[0]["ACF_VALUE"].ToString() == "1" ? true : false;
            }
            #endregion

            hdfIsTaxForOtherCharge.Value = GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales").ToString();
            IsInvoiceGSTEnable = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableGST")));
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CUSTOMER");
            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUCustomer.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
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
                        //base.WkfPageUrl = path;
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
            WorkflowCore.CoreService wrkfService = new WorkflowCore.CoreService();
            string TYPE = Request.QueryString[QueryStrings.PageType] != null ? Request.QueryString[QueryStrings.PageType] : string.Empty;
            if (TYPE != "3")//Type 3 for cancelation
            {
                DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), Convert.ToInt32(Session[BusinessObject.Common.SessionStrings.CurDept].ToString()));
                //DataTable dtCancelProcess = wrkfService.GetProcessID(GetLocalResourceObject("CancelPageURL").ToString(), currentUser.CurrentDeptPK);
                if (dtCancelProcess != null && dtCancelProcess.Rows.Count > 0)
                {
                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                    ucrWrkf.CancelRefID = workflowCore.GetRefID(curPK, int.Parse(dtCancelProcess.Rows[0][CommonConstants.F_PROCESS].ToString()));
                }
            }
            #endregion
        }
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
            PICKFORRECEIPT,
            INVOICENO,
            PICKFORCRDRNOTE,
            EXCHANGERATE,
            JOURNALIZE,
            WRKFSUBMIT,
            FINHEADER,
            FINPERIOD,
            GETINVOICEPKBYJOURNALPK,
            FILLWORKFLOWSTATUS,
            INVOICETYPE,
            TAXCALCULATIONSETTINGS,
            NOTIFICATIONTYPES,
            ALERTBASIS,
            ALERTTYPES,
            NOTIFICATIONDAYS,
            ALERTCONFIG,
            ALERTLIST,
            ALERTSAVE,
            SALES_INVOICE_TYPE,
            COMPANY,
            COMPANYNAME,
            SOINVHEADERBYPK,
            SOTYPE,
            CUSTOMERTYPES,
            GETCUSTOMERDETAILSBYTYPE,
            TAXSPLITUP,
            AMOUNTDETAILS,
            RESETRECEIPT,
            INVOICEGSTTYPE,
            GSTSUBTYPE

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

        private void SetTaxDetailsPopup(SAL_ORDER_HDR SalOrderHdrListTaxSplitup)
        {

            List<SAL_ORDER_TAX_HDR> LstSAL_ORDER_TAX_HDR = new List<SAL_ORDER_TAX_HDR>();
            List<SAL_ORDER_TAX_DTL> LstSAL_ORDER_TAX_DTL = new List<SAL_ORDER_TAX_DTL>();
            List<SAL_ORDER_TAX_HDR> LstSAL_ORDER_TAX_Final = new List<SAL_ORDER_TAX_HDR>();
            List<SAL_ORDER_DTL> LstSAL_ORDER_DTL = new List<SAL_ORDER_DTL>();
            if (SalOrderHdrListTaxSplitup != null)
            {
                LstSAL_ORDER_DTL = SalOrderHdrListTaxSplitup.SAL_ORDER_DTL.ToList();
                // Adding Line Item Tax
                if (LstSAL_ORDER_DTL != null)
                {
                    foreach (var item in LstSAL_ORDER_DTL)
                    {
                        var taxDetail = item.SAL_ORDER_TAX_DTL.Where(rfq => rfq.SLT_TAX_CATEGORY == ((int)TaxType.Tax));
                        foreach (SAL_ORDER_TAX_DTL rfqTaxHdrObj in taxDetail)
                        {
                            LstSAL_ORDER_TAX_DTL.Add(rfqTaxHdrObj);
                        }
                    }

                }

                //Adding Header tax             
                var HeaderTax = SalOrderHdrListTaxSplitup.SAL_ORDER_TAX_HDR.Where(rfq => rfq.TSH_TAX_CATEGORY == ((int)TaxType.Tax));
                if (HeaderTax != null)
                {
                    foreach (SAL_ORDER_TAX_HDR rfqTaxHdrObj in HeaderTax)
                    {
                        LstSAL_ORDER_TAX_HDR.Add(rfqTaxHdrObj);
                    }
                }
            }

            #region Insert SAL_ORDER_TAX_Header AND SAL_ORDER_TAX_DTL to final List For binding grid
            //Insert SAL_ORDER_TAX_DTL to final List For binding grid
            for (int i = 0; i < LstSAL_ORDER_TAX_DTL.Count; i++)
            {
                SAL_ORDER_TAX_HDR objTaxFinal = new SAL_ORDER_TAX_HDR();
                objTaxFinal.TSH_PK = LstSAL_ORDER_TAX_DTL[i].SLT_PK;
                objTaxFinal.TSH_SO_HDR = LstSAL_ORDER_TAX_DTL[i].SLT_SO_DTL;
                objTaxFinal.TSH_TYPE = LstSAL_ORDER_TAX_DTL[i].SLT_TYPE;
                objTaxFinal.TSH_TAX = LstSAL_ORDER_TAX_DTL[i].SLT_TAX;
                objTaxFinal.TSH_TAX_CATEGORY = LstSAL_ORDER_TAX_DTL[i].SLT_TAX_CATEGORY;
                objTaxFinal.TSH_NAME = LstSAL_ORDER_TAX_DTL[i].SLT_NAME;
                objTaxFinal.TSH_TAX_AMT = LstSAL_ORDER_TAX_DTL[i].SLT_TAX_AMT;
                LstSAL_ORDER_TAX_Final.Add(objTaxFinal);
            }
            //Insert SAL_ORDER_TAX_Header to final List 
            for (int i = 0; i < LstSAL_ORDER_TAX_HDR.Count; i++)
            {
                SAL_ORDER_TAX_HDR objTaxFinal = new SAL_ORDER_TAX_HDR();
                objTaxFinal.TSH_PK = LstSAL_ORDER_TAX_HDR[i].TSH_PK;
                objTaxFinal.TSH_SO_HDR = LstSAL_ORDER_TAX_HDR[i].TSH_SO_HDR;
                objTaxFinal.TSH_TYPE = LstSAL_ORDER_TAX_HDR[i].TSH_TYPE;
                objTaxFinal.TSH_TAX = LstSAL_ORDER_TAX_HDR[i].TSH_TAX;
                objTaxFinal.TSH_TAX_CATEGORY = LstSAL_ORDER_TAX_HDR[i].TSH_TAX_CATEGORY;
                objTaxFinal.TSH_NAME = LstSAL_ORDER_TAX_HDR[i].TSH_NAME;
                objTaxFinal.TSH_TAX_AMT = LstSAL_ORDER_TAX_HDR[i].TSH_TAX_AMT;
                LstSAL_ORDER_TAX_Final.Add(objTaxFinal);
            }
            #endregion

            if (LstSAL_ORDER_TAX_Final.Count > 0)
            {
                var groupedTaxPayableList = LstSAL_ORDER_TAX_Final.GroupBy(f => f.TSH_TAX)
                    .Select(grp => new SAL_ORDER_TAX_HDR
                    {
                        TSH_TAX_AMT = grp.Sum(p => p.TSH_TAX_AMT),
                        TSH_NAME = grp.Min(p => p.TSH_NAME),
                        TSH_TAX = grp.Min(p => p.TSH_TAX),
                        TSH_PK = grp.Min(p => p.TSH_PK)
                    })
                   .ToList();

                grdTaxSplitupDetails.DataSource = groupedTaxPayableList;
                grdTaxSplitupDetails.DataBind();
            }
            else
            {
                grdTaxSplitupDetails.DataSource = null;
                grdTaxSplitupDetails.DataBind();
            }
        }

    }
}