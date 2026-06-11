using BusinessLogic.CommonManagement;
using BusinessObject;
using BusinessObject.AccountManagement;
using BusinessObject.AlertManagement;
using BusinessObject.Common;
using BusinessObject.CommonManagement;
using ERP.Utilities;
using ERPData;
using ERPManager;
using ERPService;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.POInvoicing;
using ERPSMS_v01.UserControls;
using BusinessObject.PurchaseOrderManagement;
using System.Xml.Linq;

namespace ERPSMS_v01.POInvoicing
{
    public partial class AdvanceInvoice : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
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
        /// <summary>
        /// VendorCode
        /// </summary>
        private string VendorCode
        {
            get
            {
                return this.ViewState[ViewstateStrings.VendorCode].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorCode] = value;
            }
        }
        /// <summary>
        /// VendorName
        /// </summary>
        private string VendorName
        {
            get
            {
                return this.ViewState[ViewstateStrings.VendorName].ToString();
            }
            set
            {
                this.ViewState[ViewstateStrings.VendorName] = value;
            }
        }

        /// <summary>
        /// IsVendorSelected
        /// </summary>
        private bool IsVendorSelected
        {
            get
            {
                return (bool)this.ViewState[ViewstateStrings.IsVendorSelected];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsVendorSelected] = value;
            }
        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<long> SelectedPos
        {
            get
            {
                return (List<long>)Session[ERP.Utilities.SessionStrings.SelectedPos];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.SelectedPos] = value;
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

        private List<long> SelectedVendors
        {
            get
            {
                return (List<long>)this.ViewState[ViewstateStrings.SelectedVendors];
            }
            set
            {
                this.ViewState[ViewstateStrings.SelectedVendors] = value;
            }

        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<object> PurOrderHeaderList
        {
            get
            {
                return (List<object>)Session[ERP.Utilities.SessionStrings.PurOrderHeaderList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PurOrderHeaderList] = value;
            }

        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<object> PurOrderHeaderMappingList
        {
            get
            {
                return (List<object>)Session[ERP.Utilities.SessionStrings.PurOrderHeaderMappingList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.PurOrderHeaderMappingList] = value;
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

        private List<PurchaseOrderHeader> POList
        {
            get
            {
                return (List<PurchaseOrderHeader>)Session[ERP.Utilities.SessionStrings.POList];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.POList] = value;
            }

        }

        /// <summary>
        /// To maintain keep InvoiceMap List
        /// </summary>
        private List<FIN_INVOICE_VND_TRX_MPG> InvoiceMapList
        {
            get
            {
                return (List<FIN_INVOICE_VND_TRX_MPG>)ViewState["InvoiceMapList"];
            }
            set
            {
                ViewState["InvoiceMapList"] = value;
            }

        }

        private List<InvoiceVendorMapping> InvoiceMappingList
        {
            get { return (List<InvoiceVendorMapping>)ViewState["InvoiceMappingList"]; }
            set { ViewState["InvoiceMappingList"] = value; }
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

        private List<ADM_DOC_ATTACH> DocAttachList
        {
            get
            {
                return ViewState[ViewstateStrings.DocAttachList] == null ? null : (List<ADM_DOC_ATTACH>)ViewState[ViewstateStrings.DocAttachList];
            }
            set
            {
                ViewState[ViewstateStrings.DocAttachList] = value;
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
        private List<long> SelectedPOListValidate
        {
            get
            {
                return this.ViewState["SelectedPOListValidate"] == null ? null : (List<long>)this.ViewState["SelectedPOListValidate"];
            }
            set
            {
                this.ViewState["SelectedPOListValidate"] = value;
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

        private FIN_INVOICE_VND_HDR finInvoiceVndHdrObj;
        private FIN_INVOICE_VND_TRX_MPG finInvoiceVndTrxMpgObj;
        private InvoiceHdr InvoiceHeaderObj;
        private InvoiceVendorMapping InvoiceVendorMappingObj;
        DataTable dtAdsType;
        DataTable dtAdsTypeDtl;

        DataSet dsAdsType;
        DataSet dsAdsTypeDtl;

        DataSet dsPageData;

        //private PUR_ORDER_HDR PurOrderHdrObj;
        private List<PUR_ORDER_HDR> PurOrderHdrList; //tobecomment
        private PurchaseOrderHeader PurchaseOrderHeaderObj;
        private List<PurchaseOrderHeader> PurchaseOrderHeaderList;

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
        DataTable dtInvoiceGstType;
        private int invPK;
        private int VncPk = 0;
        private string TypeRef;
        private string appType;
        private DataTable dtPageData;
        //List for binding details to controls  

        private List<FIN_INVOICE_VND_HDR> finInvoiceVndHdrList;
        private List<FIN_INVOICE_VND_TRX_MPG> finInvoiceVndTrxMpgList;
        private List<InvoiceHdr> InvoiceHeaderList;
        private List<InvoiceVendorMapping> InvoiceVendorMappingList;
        private InvoiceHdrBO invoiceHdrBO;

        private List<long> SelectedPOList;

        private List<long> SelectedInvoiceList;
        private List<long> SelectedInvoiceCrDrList;
        private List<long> SelectedCurrencyList;
        private List<decimal> SelectedINVTaxList;
        private List<long> SelectedVendorsList;
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
        private SelectedItems objSelectedItem;

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
                hdfDecimalFormat.Value = "#0.";
                int NoDecimalDigitsP2P = Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P] != null ? Convert.ToInt32(Session[ERP.Utilities.SessionStrings.NumberDecimalDigitsP2P].ToString()) : 2;
                for (int i = 0; i < NoDecimalDigitsP2P; i++)
                {
                    hdfDecimalFormat.Value += "0";
                }
                hdfCurrencyFormat.Value = "#0.";
                for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                {
                    hdfCurrencyFormat.Value += "0";
                }
                //hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();

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
                    SelectedInvoicesCrDr = null;

                    FileDetailsList = null;
                    DocAttachList = null;

                    txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfToDate.Value = DateTime.Now.ToString();
                    //txtFromDate.Text = string.Empty;
                    //hdfFromDate.Value = string.Empty;
                    //txtToDate.Text = string.Empty;
                    //hdfToDate.Value = string.Empty;

                    GetFieldValues(ControlsEnum.FINPERIOD);
                    //if (finYearMstList != null && finYearMstList.Count > 0)
                    //{
                    //    txtPaybydateFrom.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                    //    hdfPaybydateFrom.Value = finYearMstList[0].FYR_DATE_FROM.ToString();
                    //    txtPayByToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                    //    hdfPayByToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString();
                    //}
                    //else
                    //{
                    //    //txtPaybydateFrom.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    //    //hdfPaybydateFrom.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString();
                    //    //txtPayByToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    //    //hdfPayByToDate.Value = DateTime.Now.ToString();
                    //}

                    //GetFieldValues(ControlsEnum.FINPERIOD);
                    //SetFieldValues(ControlsEnum.FINPERIOD);

                    ////start
                    string pid = Request.QueryString[QueryStrings.PID] != null ? Request.QueryString[QueryStrings.PID]
                        : Session[ERP.Utilities.SessionStrings.PID] != null ? Session[ERP.Utilities.SessionStrings.PID].ToString().Split('=')[1] : string.Empty;
                    ////

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
                        if (string.IsNullOrEmpty(pid) || pid.Equals("1") || pid.Equals("11"))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(refID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                            if (pid.Equals("11"))
                                hdfIsInvCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
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

                        ////

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
                        SetCancelRef(CurrPK);
                        ucrWrkf.FillWorkFlowDetails();
                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            ucrWrkf.ViewType = 1;
                        else
                        {
                            ucrWrkf.ViewType = 0;
                            //EntryStatus = EntryStatus.VIEWMODE;
                            //btnSave.Visible = false;
                        }
                        if (EntryStatus == EntryStatus.VIEWMODE)
                            SetUIEditView(ActionsEnum.VIEW);
                        else
                            SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        //GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                        finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        finInvoiceVndHdrObj.IVH_PK = CurrPK;
                        hdfIVHPK.Value = CurrPK.ToString();
                        GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                        GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                        SetFieldValues(ControlsEnum.POINVOICELIST);
                    }
                    else
                    {
                        postflag = true;
                        //Sets data key for the gird
                        string[] datakeyarray;
                        datakeyarray = new string[1];
                        datakeyarray[0] = Resources.DataFieldRes.POInvoicePK;
                        grdPOInvoiceList.DataKeyNames = datakeyarray;
                        if (SelectedPos != null)
                        {
                            SetCancelRef(CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && EntryStatus == EntryStatus.ENTRYMODE && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrWrkf.ViewType = 1;
                                EntryStatus = EntryStatus.NEWMODE;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                                //btnSave.Visible = false;
                                //btnSubmit.Visible = false;
                            }
                            //GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            SelectedPOList = SelectedPos;
                            SelectedPOListValidate = SelectedPos;
                            GetFieldValues(ControlsEnum.POINVOICELIST);
                            //if (PurOrderHdrList != null && PurOrderHdrList.Count > 0)
                            //    POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), PurOrderHdrList.First().POH_GROUP.ToString());
                            if (PurchaseOrderHeaderList != null && PurchaseOrderHeaderList.Count > 0)
                                POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), PurchaseOrderHeaderList.First().GroupPK.ToString());
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            lblDispInvoiceNo.Text = "[NEW]";
                        }
                        else
                        {
                            SelectedInvoices = null;
                            SelectedPOList = new List<long>();
                            //GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICEHDR);
                            SetFieldValues(ControlsEnum.INVOICEHDR);
                            EntryStatus = EntryStatus.LISTMODE;
                        }
                    }
                    GetFieldValues(ControlsEnum.UPLOADEDFILES);
                    SetFieldValues(ControlsEnum.UPLOADEDFILES);

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
                }

                if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                {
                    lblCompany.Visible = true;
                    ddlCompanySrch.Visible = true;
                    ddlCompanyView.Visible = true;
                    lblCompanyView.Visible = true;
                }
                else
                {
                    lblCompany.Visible = false;
                    ddlCompanySrch.Visible = false;
                    ddlCompanyView.Visible = false;
                    lblCompanyView.Visible = false;
                }

                //Session[ERP.Utilities.SessionStrings.RefID] = null;
                //Session[ERP.Utilities.SessionStrings.InboxFlag] = null;
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
            POListService poListServiceClient;
            poListServiceClient = null;

            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;
            BusinessObject.User currentUser;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;

            int? Status = null;

            CommonService CommonServiceClient;
            CommonServiceClient = null;

            try
            {
                switch (type)
                {
                    #region Invoice Header List
                    case ControlsEnum.INVOICEHDR:
                        //poInvoiceServiceClient = new POInvoiceService();
                        //poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        //finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        serviceUtilityObj.PageSize = grdPOInvoiceList.PageSize;
                        //serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.POInvoiceDate : SortBy;
                        //serviceUtilityObj.ThenBy = ThenBy = ThenBy == null ? Resources.DataFieldRes.InvoiceNo : ThenBy;
                        //serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        //serviceUtilityObj.FilterBy = HttpUtility.HtmlEncode(string.Empty);

                        //if (txtVendor.Text.Trim() == string.Empty)
                        //{
                        //    finInvoiceVndHdrObj.IVH_VENDOR = 0;
                        //    hdfVendorID.Value = "0";
                        //}
                        //else
                        //{
                        //    finInvoiceVndHdrObj.IVH_VENDOR = String.IsNullOrEmpty(hdfVendorID.Value.Trim()) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        //}
                        //if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        //{
                        //    Session[ERP.Utilities.SessionStrings.VendorPK] = hdfVendorID.Value;
                        //    Session[ERP.Utilities.SessionStrings.Vendor] = txtVendor.Text;
                        //}
                        //finInvoiceVndHdrObj.IVH_PK = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        //finInvoiceVndHdrObj.IVH_ACTIVE = 1;
                        //finInvoiceVndHdrObj.IVH_CATEGORY = 2;
                        //finInvoiceVndHdrObj.IVH_BIZUNIT = currentUser.SBUID;
                        //finInvoiceVndHdrObj.IVH_CRTD_BY = currentUser.PKUser;

                        //serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtFromDate.Text.Trim());
                        //serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtToDate.Text.Trim());

                        //DateTime? PayByDateFrom = string.IsNullOrEmpty(txtPaybydateFrom.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPaybydateFrom.Text.Trim());
                        //DateTime? PayByDateTo = string.IsNullOrEmpty(txtPayByToDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPayByToDate.Text.Trim());
                        //Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        //string poNo = null;
                        //if (txtpoNo.Text != null)
                        //{ poNo = txtpoNo.Text.Trim(); }
                        //else { poNo = ""; }
                        //finInvoiceVndHdrObj.IVH_COMPANY = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        //finInvoiceVndHdrList = poInvoiceServiceClient.GetInvoiceHdr(finInvoiceVndHdrObj, serviceUtilityObj, PayByDateFrom, PayByDateTo, Status, poNo);

                        //TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                        //              (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                        //              (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        #region New

                        int VenID = 0;
                        int group = 0;
                        int InvPk = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        Status = Convert.ToInt32(ddlStatus.SelectedValue);
                        int cmpPk = Convert.ToInt32(ddlCompanySrch.SelectedValue);
                        int TotalRecords = 0;
                        string Vendor = "";

                        if (hdfVendorID.Value != "" && hdfVendorID.Value != "0")
                        {
                            VenID = Convert.ToInt32( hdfVendorID.Value);
                            Vendor = txtVendor.Text;
                        }


                        GridPrams gridParamObj = new GridPrams();
                        gridParamObj.PageNumber = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                        gridParamObj.PageSize = serviceUtilityObj.PageSize;
                        gridParamObj.SortBy = string.IsNullOrEmpty(SortBy) ? "IVH_DATE" : SortBy;
                        gridParamObj.SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection;
                        gridParamObj.ThenBy = SortBy == ThenBy || SortBy == "IVH_NO" ? string.Empty : string.IsNullOrEmpty(ThenBy) ? "IVH_NO" : ThenBy;
                        gridParamObj.ThenDirection = "";
                        gridParamObj.FromDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? string.Empty : txtFromDate.Text.Trim();
                        gridParamObj.ToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? string.Empty : txtToDate.Text.Trim();
                        gridParamObj.SearchBy = "IVH_NO";
                        gridParamObj.SearchValue = string.IsNullOrEmpty(txtInvoiceNumber.Text.Trim()) ? string.Empty : (txtInvoiceNumber.Text.Trim() == "Select/Type" ? string.Empty : txtInvoiceNumber.Text.Trim());
                        gridParamObj.POType = 0;
                        gridParamObj.ConvertTo = 0;
                        gridParamObj.SCNo = "";
                        string PayByDateFrom = string.IsNullOrEmpty(txtPaybydateFrom.Text.Trim()) ? string.Empty : txtPaybydateFrom.Text.Trim();
                        string PayByDateTo = string.IsNullOrEmpty(txtPayByToDate.Text.Trim()) ? string.Empty : txtPayByToDate.Text.Trim();
                        string strXmlResult = BusinessLogic.POInvoicing.POInvoiceBL.GetAdvInvoiceList(gridParamObj, currentUser, VenID, InvPk, CurrPK, Vendor, txtpoNo.Text
                        , Resources.PageURL.PoInvoicing.Replace("~", ""), PayByDateFrom, PayByDateTo, out TotalRecords, 0, Convert.ToInt32(Status), group, (byte)POInvoiceCategory.Advanced
                        , 0, null, string.Empty, cmpPk);
                        if (strXmlResult != "")
                        {
                            invoiceHdrBO = CommonFunctions.XmlDeserialize<InvoiceHdrBO>(strXmlResult);
                            InvoiceHeaderList = invoiceHdrBO.InvoiceHdrList;
                        }                      
                        gridParamObj.TotalRecords = TotalRecords;
                        TotalPages = (gridParamObj.TotalRecords == 0) ? 1 : (gridParamObj.TotalRecords <= gridParamObj.PageSize) ? 1 : (gridParamObj.TotalRecords % gridParamObj.PageSize) == 0 ? (gridParamObj.TotalRecords / gridParamObj.PageSize) : (gridParamObj.TotalRecords / gridParamObj.PageSize) + 1;

                        #endregion

                        break;
                    #endregion
                    #region Invoice Header By Pk
                    case ControlsEnum.INVOICEHDRBYPK:
                        poInvoiceServiceClient = new POInvoiceService();
                        poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        serviceUtilityObj = new ServiceUtility();
                        //serviceUtilityObj.CurrentPage = 1;
                        //serviceUtilityObj.PageSize = grdPOInvoiceList.PageSize;
                        //serviceUtilityObj.SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.InvoiceNo : SortBy;
                        //serviceUtilityObj.SortDirection = SortDirection = SortDirection == null ? Resources.Report.SortDescending : SortDirection;
                        //serviceUtilityObj.FilterBy = HttpUtility.HtmlEncode(string.Empty);                       


                        // finInvoiceVndHdrObj.IVH_VENDOR = String.IsNullOrEmpty(hdfVendorID.Value.Trim()) ? 0 : Convert.ToInt32(hdfVendorID.Value);
                        finInvoiceVndHdrObj.IVH_PK = String.IsNullOrEmpty(hdfIVHPK.Value.Trim()) ? 0 : Convert.ToInt32(hdfIVHPK.Value);
                        finInvoiceVndHdrObj.IVH_ACTIVE = 1;
                        //serviceUtilityObj.FilterDate = string.IsNullOrEmpty(txtFromDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtFromDate.Text.Trim());
                        //serviceUtilityObj.FilterToDate = string.IsNullOrEmpty(txtToDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtToDate.Text.Trim());
                        //PayByDateFrom = string.IsNullOrEmpty(txtPaybydateFrom.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtPaybydateFrom.Text.Trim());
                        //PayByDateTo = string.IsNullOrEmpty(txtPayByToDate.Text.Trim()) ? DateTime.MinValue : Convert.ToDateTime(txtPayByToDate.Text.Trim());
                        finInvoiceVndHdrList = poInvoiceServiceClient.GetInvoiceHdrByPK(finInvoiceVndHdrObj);
                        //finInvoiceVndHdrList = poInvoiceServiceClient.GetInvoiceHdr(finInvoiceVndHdrObj, serviceUtilityObj, PayByDateFrom, PayByDateTo);

                        //TotalPages = serviceUtilityObj.TotalRecords == 0 ? 1 : (serviceUtilityObj.TotalRecords <= serviceUtilityObj.PageSize) ? 1 :
                        //              (serviceUtilityObj.TotalRecords % serviceUtilityObj.PageSize) == 0 ? (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) :
                        //              (serviceUtilityObj.TotalRecords / serviceUtilityObj.PageSize) + 1;

                        break;
                    #endregion
                    #region PO Header List
                    case ControlsEnum.POINVOICELIST:
                        poListServiceClient = new POListService();
                        poListServiceClient = CommonFunctions.InitiateClient(poListServiceClient);
                        PurchaseOrderHeaderObj = new PurchaseOrderHeader();
                        serviceUtilityObj = new ServiceUtility();
                        PurchaseOrderHeaderObj.Active = 1;
                        PurOrderHdrList = poListServiceClient.GetSelectedPOs(SelectedPos, serviceUtilityObj);

                        POHeaderBO po = new POHeaderBO();
                        List<POHeaderListBO> poList = new List<POHeaderListBO>();
                        POHeaderListBO _po;
                        foreach (long item in SelectedPos)
                        {
                            _po = new POHeaderListBO();
                            _po.POPK = Convert.ToInt32(item);
                            if (Request.QueryString["POHGROUP"] != null)
                                _po.POHGROUP = Convert.ToInt32(Request.QueryString["POHGROUP"]);// 6=WO
                            poList.Add(_po);
                        }
                        po.POList = poList;
                        string strxml = CommonFunctions.XmlSerialize<POHeaderBO>(po);
                        string XmlResult = BusinessLogic.POInvoicing.POInvoiceBL.GetPOListByPK(strxml, currentUser.SBUID);
                        //string XmlResult = "<Root><PO><POH_PK> 134 </POH_PK><POH_VERSION> 1 </POH_VERSION><POH_NO> WO / 2103 / 0131 </POH_NO><POH_DATE> 2021 - 03 - 09 </POH_DATE><POH_REFERENCE> 12 </POH_REFERENCE><POH_REF_DATE> 1900 - 01 - 01 </POH_REF_DATE><POH_DELY_DATE> 1900 - 01 - 01 </POH_DELY_DATE><POH_STATUS> 2 </POH_STATUS><POH_CURRENCY> 63 </POH_CURRENCY><POH_VENDOR> 34 </POH_VENDOR><POH_CONTRACT_REF_NO /><POH_SHIPPING /><POH_BILLING /><POH_TOTAL_QTY> 1.000000000000000e+002 </POH_TOTAL_QTY><POH_SUB_TOTAL> 1200.0000 </POH_SUB_TOTAL><POH_DISC_PERC> 0.00 </POH_DISC_PERC><POH_DISC_AMT> 0.0000 </POH_DISC_AMT><POH_NET_TOTAL> 1200.0000 </POH_NET_TOTAL><POH_SHIP_CHARGE> 0.0000 </POH_SHIP_CHARGE><POH_SALES_TAX_PERC> 0 </POH_SALES_TAX_PERC><POH_SALES_TAX_AMT> 0 </POH_SALES_TAX_AMT><POH_ADD_TAX_PERC> 0 </POH_ADD_TAX_PERC><POH_ADD_TAX_AMT> 0 </POH_ADD_TAX_AMT><POH_PRICE_ADJUST> 0.0000 </POH_PRICE_ADJUST><POH_TOTAL_VALUE> 1200.0000 </POH_TOTAL_VALUE><POH_CURRENCY_BC> 63 </POH_CURRENCY_BC><POH_EXCHG_RATE> 1.000000000000000e+000 </POH_EXCHG_RATE><POH_TOTAL_VALUE_BC> 1200.0000 </POH_TOTAL_VALUE_BC><POH_AMT_INVOICED> 0.0000 </POH_AMT_INVOICED><POH_AMT_PAID> 0.0000 </POH_AMT_PAID><POH_REMARKS /><POH_COMMENTS /><POH_VENDOR_TERMS> 0 </POH_VENDOR_TERMS><POH_TERMS> 0 </POH_TERMS><POH_TYPE> 2 </POH_TYPE><POH_ITEM_TYPE> 3 </POH_ITEM_TYPE><POH_GROUP> 6 </POH_GROUP><POH_ACTIVE> 1 </POH_ACTIVE><POH_DEPT> 28 </POH_DEPT><POH_BIZUNIT> 1 </POH_BIZUNIT><POH_COMPANY> 1 </POH_COMPANY><POH_CRTD_BY> 1 </POH_CRTD_BY><POH_CRTD_DT> 2021 - 03 - 09T16: 41:49.900 </POH_CRTD_DT><POH_MOD_BY> 1 </POH_MOD_BY><POH_MOD_DT> 2021 - 03 - 09T16: 41:49.900 </POH_MOD_DT><POH_AMEND_DATE> 1900 - 01 - 01T00: 00:00 </POH_AMEND_DATE><POH_DEL_STATUS> 0 </POH_DEL_STATUS><POH_VENDOR_TERMS_TEXT> Payment 30 days </POH_VENDOR_TERMS_TEXT><POH_TERMS_TEXT> WO term 1 </POH_TERMS_TEXT><POH_FROM_PORT_TEXT /><POH_IS_GLOVE> 0 </POH_IS_GLOVE><POH_CONVERTED> 0 </POH_CONVERTED></PO></Root>";

                        BusinessObject.POInvoicing.PurchaseOrder poHeader = CommonFunctions.XmlDeserialize<BusinessObject.POInvoicing.PurchaseOrder>(XmlResult);
                        PurchaseOrderHeaderList = poHeader.PurchaseOrderList;
                        POList = PurchaseOrderHeaderList;

                        PoHeaderList = PurOrderHdrList;
                        break;
                    #endregion
                    #region PO Invoice Details List
                    case ControlsEnum.POINVOICEDETAILS:
                        poListServiceClient = new POListService();
                        poListServiceClient = CommonFunctions.InitiateClient(poListServiceClient);
                        PurchaseOrderHeaderObj = new PurchaseOrderHeader();
                        serviceUtilityObj = new ServiceUtility();
                        PurchaseOrderHeaderObj.Active = 1;

                        finInvoiceVndTrxMpgList = poListServiceClient.GetInvoicedPOs(CurrPK);
                        InvoiceMapList = finInvoiceVndTrxMpgList;

                        string xmlResult = BusinessLogic.POInvoicing.POInvoiceBL.GetAdvInvoiceHeader(0, CurrPK);
                        VendorInvMpg mpg = new VendorInvMpg();
                        mpg = CommonFunctions.XmlDeserialize<VendorInvMpg>(xmlResult);
                        InvoiceVendorMappingList = mpg.InvoiceVendorMappingList;
                        InvoiceMappingList = InvoiceVendorMappingList;

                        break;
                    #endregion
                    #region Generate Invoice No
                    case ControlsEnum.INVOICENO:
                        //Generate Invoice No
                        poInvoiceServiceClient = new POInvoiceService();
                        poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        //ivoiceNo = poInvoiceServiceClient.GetInvoiceNo(POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                        //    : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI,
                        //    (int)AppSubTypePOInvoice.ADVINVOICE, 1,
                        //    DateTime.Now, currentUser.PKUser, updateInvoice, 0);
                        ivoiceNo = poInvoiceServiceClient.GetInvoiceNo(POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI,
                            numberGenerationSubType, currentUser.CurrentDeptPK,
                            Convert.ToDateTime(txtInvdate.Text), currentUser.PKUser, updateInvoice, 0, Convert.ToInt32(ddlCompany.SelectedValue));
                        hdfInvoiceNo.Value = ivoiceNo;
                        break;
                    #endregion
                    #region Generate Exchange Rate
                    case ControlsEnum.EXCHANGERATE:
                        //Generate Exchange Rate
                        poInvoiceServiceClient = new POInvoiceService();
                        poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        double ExchgRate = poInvoiceServiceClient.GetConversionFactor(
                                                             InvoiceHeaderObj.IVH_CURRENCY, InvoiceHeaderObj.BaseCurrencyPK,
                                                             InvoiceHeaderObj.IVH_DATE, currentUser.SBUID);
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
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.PI, (byte)AppSubTypePOInvoice.ADVINVOICE, Convert.ToByte(CommonConstants.ACTIVE));
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
                        admAppConfigMstObj.ACF_DATA = ApplicationType.PI;
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
                        //if (vndPK != null || vndPK > 0)
                        //{
                        //    //dtPageData = BusinessLogic.CommonManagement.CommonBL.GetVendorContactList(vndPK, Convert.ToByte(DbActiveStatus.HASPK), 0, currentUser.SBUID);
                        //    dtPageData = BusinessLogic.CommonManagement.CommonBL.GetVendorContactList(0, Convert.ToByte(DbActiveStatus.ACTIVE), vndPK, currentUser.SBUID);
                        //}
                        //else
                        //{
                        //    dtPageData = BusinessLogic.CommonManagement.CommonBL.GetVendorContactList(0, Convert.ToByte(DbActiveStatus.ACTIVE), vndPK, currentUser.SBUID);
                        //}
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        if (finInvoiceVndHdrObj != null)
                        {
                            poInvoiceServiceClient = new POInvoiceService();
                            poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                            admDocAttachObj = CommonFunctions.Initilize<ADM_DOC_ATTACH>();
                            serviceUtilityObj = new ServiceUtility();
                            //serviceUtilityObj.CurrentPage = PageIndex == null ? 1 : Convert.ToInt32(PageIndex);
                            //serviceUtilityObj.PageSize = grdPOInvoiceList.PageSize;
                            admDocAttachObj.DOC_TASK_ID = (int)finInvoiceVndHdrObj.IVH_PK;
                            admDocAttachObj.DOC_TASK = (int)DocTaskEnum.PURCHASEINVOICE;
                            DocAttachList = poInvoiceServiceClient.GetDocAttachments(admDocAttachObj, serviceUtilityObj);

                        }


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
                        //poInvoiceServiceClient = new POInvoiceService();
                        //poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                        //dtAmountDetails = poInvoiceServiceClient.GetInvVndReceivedAmntDetails(InvoicePk);
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
                    #region nvoice GST Type
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
                    case ControlsEnum.INVOICEHDR:
                        BindGrid(ControlsEnum.INVOICEHDR);
                        break;
                    case ControlsEnum.POINVOICELIST:
                        BindGrid(ControlsEnum.POINVOICELIST);
                        break;
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {
                            txtFromDate.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            hdfFromDate.Value = finYearMstList[0].FYR_DATE_FROM.ToString();
                            txtToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                            hdfToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString();

                            //txtPaybydateFrom.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            //hdfPaybydateFrom.Value = finYearMstList[0].FYR_DATE_FROM.ToString();
                            //txtPayByToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                            //hdfPayByToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString();
                        }
                        break;
                    #endregion
                    #region Company
                    case ControlsEnum.COMPANY:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region VendorBranches
                    case ControlsEnum.VENDORBRANCH:
                        //Bind Company List in DDL
                        BindDropDown(ControlsEnum.VENDORBRANCH);
                        break;
                    #endregion
                    #region UPLOADEDFILES
                    case ControlsEnum.UPLOADEDFILES:
                        //if (finInvoiceVndHdrList != null)
                        //    CurrPK = int.Parse(finInvoiceVndHdrList[0].IVH_PK.ToString());
                        BindGrid(ControlsEnum.UPLOADEDFILES);
                        break;
                    #endregion
                    case ControlsEnum.VENDORCONTACTYPEDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    case ControlsEnum.VENDORCONTACTYPE:
                        BindDropDown(controlType);
                        break;
                    case ControlsEnum.AMOUNTDETAILS:
                        BindGrid(controlType);
                        break;
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
                    #region Invoice GST Type
                    case ControlsEnum.INVOICEGSTTYPE:
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

            IsInvoiceGSTEnable = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "EnableGST")));
            DataTable dtAppConfigs = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "VENDOR");
            hdfShowInvestor.Value = (GetGlobalResourceObject("ConfigurationsRes", "ShowInvestor")).ToString();
            if (GetGlobalResourceObject("ConfigurationsRes", "ShowInvestor").ToString() == "1")
            {
                txtInvestor.Visible = true;
                lblInvestor.Visible = true;
            }
            else
            {
                txtInvestor.Visible = false;
                lblInvestor.Visible = false;
            }

            if (dtAppConfigs != null && dtAppConfigs.Rows.Count > 0)
            {
                hdfIsSBUVendor.Value = dtAppConfigs.Rows[0]["ACF_VALUE"].ToString() == "0" ? "true" : "false";
            }
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
                int i = 0;
                AlertBO alertBoObj;
                decimal balamt;
                balamt = 0;
                LinkButton lnkBalAmt;
                //Label lblBalAmt;
                bool bIsChecked = false;
                int isDelete = 0;

                switch (controlType)
                {
                    #region Invoice Header
                    case ControlsEnum.FINANCEINVOICEHDR:
                        InvoiceHeaderObj.IVH_PK = CurrPK;
                        InvoiceHeaderObj.IVH_NO = (string.IsNullOrEmpty(lblDispInvoiceNo.Text) || lblDispInvoiceNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                    : lblDispInvoiceNo.Text.Trim();
                        InvoiceHeaderObj.IVH_DATE = string.IsNullOrEmpty(txtInvdate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvdate.Text.Trim());
                        if (POList != null)
                        {
                            PurchaseOrderHeaderList = (List<PurchaseOrderHeader>)POList;
                            InvoiceHeaderObj.IVH_VENDOR = PurchaseOrderHeaderList[0].VendorPK;
                            InvoiceHeaderObj.VendorInvoiceNo = txtSupplierInvNo.Text;
                            InvoiceHeaderObj.InvoiceCategory = (byte)POInvoiceCategory.Advanced;
                            InvoiceHeaderObj.InvoiceGroup = ((byte)POGroup) == (byte)0 ? (byte)1 : (byte)POGroup;

                            InvoiceHeaderObj.VendorAccount = PurchaseOrderHeaderList[0].Vendor.AccountPK;
                            InvoiceHeaderObj.IVH_CURRENCY = PurchaseOrderHeaderList[0].CurrencyPK;
                            if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                            {
                                InvoiceHeaderObj.Investor = PurchaseOrderHeaderList[0].Investor;
                            }

                            InvoiceHeaderObj.VendorName = PurchaseOrderHeaderList[0].Vendor.VEN_NAME;
                            InvoiceHeaderObj.VendorAddress = PurchaseOrderHeaderList[0].Vendor.Address1;
                            InvoiceHeaderObj.VendorCountryPK = PurchaseOrderHeaderList[0].Vendor.CountryPK;
                            InvoiceHeaderObj.VendorEmail = PurchaseOrderHeaderList[0].Vendor.Email;
                            InvoiceHeaderObj.VendorFAX = PurchaseOrderHeaderList[0].Vendor.Fax;
                            InvoiceHeaderObj.VendorMobile = PurchaseOrderHeaderList[0].Vendor.Mobile;
                            InvoiceHeaderObj.VendorPhone = PurchaseOrderHeaderList[0].Vendor.Phone;
                            InvoiceHeaderObj.VendorZIP = PurchaseOrderHeaderList[0].Vendor.PIN;
                        }
                        if (InvoiceMappingList != null)
                        {
                            InvoiceVendorMappingList = (List<InvoiceVendorMapping>)InvoiceMappingList;
                            InvoiceHeaderObj.IVH_VENDOR = InvoiceVendorMappingList[0].PurchaseOrder.VendorPK;
                            InvoiceHeaderObj.VendorInvoiceNo = txtSupplierInvNo.Text;
                            InvoiceHeaderObj.VendorAccount = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.AccountPK;
                            InvoiceHeaderObj.IVH_CURRENCY = InvoiceVendorMappingList[0].PurchaseOrder.CurrencyPK;
                            InvoiceHeaderObj.InvoiceGroup = ((byte)POGroup) == (byte)0 ? (byte)1 : (byte)POGroup;
                            if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                            {
                                InvoiceHeaderObj.Investor = InvoiceVendorMappingList[0].PurchaseOrder.Investor;
                            }
                            InvoiceHeaderObj.VendorName = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.VEN_NAME;
                            InvoiceHeaderObj.VendorAddress = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Address1;
                            InvoiceHeaderObj.VendorCountryPK = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.CountryPK;
                            InvoiceHeaderObj.VendorEmail = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Email;
                            InvoiceHeaderObj.VendorFAX = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Fax;
                            InvoiceHeaderObj.VendorMobile = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Mobile;
                            InvoiceHeaderObj.VendorPhone = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Phone;
                            InvoiceHeaderObj.VendorZIP = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.PIN;
                        }
                        InvoiceHeaderObj.Reference = "";
                        InvoiceHeaderObj.ReceivedDate = string.IsNullOrEmpty(txtInvoiceReceivedon.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvoiceReceivedon.Text.Trim());
                        InvoiceHeaderObj.IVH_DATE_PAY_BY = string.IsNullOrEmpty(txtPaybydate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaybydate.Text.Trim());
                        InvoiceHeaderObj.OriginalReceived = chkOriginalinvoice.Checked ? (byte)1 : (byte)0;
                        InvoiceHeaderObj.AmountTC = string.IsNullOrEmpty(txtInvoiceAmt.Text) ? 0 : Convert.ToDecimal(txtInvoiceAmt.Text);
                        InvoiceHeaderObj.DiscountTC = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
                        InvoiceHeaderObj.IVH_TAX_TC = string.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
                        InvoiceHeaderObj.IVH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
                        InvoiceHeaderObj.BaseCurrencyPK = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;
                        InvoiceHeaderObj.CompanyPK = Convert.ToInt16(ddlCompany.SelectedValue);
                        int branch = 0;
                        if (int.TryParse(ddlVendorBranch.SelectedValue, out branch))
                        {
                            InvoiceHeaderObj.VendorContact = Convert.ToInt16(ddlVendorBranch.SelectedValue);
                        }
                        else
                        {
                            InvoiceHeaderObj.VendorContact = null;
                        }
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        InvoiceHeaderObj.ExchangeRate = double.Parse(hdfExchangeRate.Value);
                        InvoiceHeaderObj.NetAmountBC = InvoiceHeaderObj.IVH_AMOUNT_NET_TC * Convert.ToDecimal(InvoiceHeaderObj.ExchangeRate);
                        InvoiceHeaderObj.Remarks = HttpUtility.HtmlEncode(txtRemarks.Text);
                        InvoiceHeaderObj.IVH_STATUS = WkfStatus;
                        InvoiceHeaderObj.IVH_DEL_STATUS = Convert.ToByte(hdfDelStatus.Value);
                        InvoiceHeaderObj.Active = 1;
                        InvoiceHeaderObj.CreatedUserPK = Convert.ToInt16(currentUser.PKUser);
                        InvoiceHeaderObj.CreatedDate = DateTime.Now;
                        InvoiceHeaderObj.ModifiedUserPK = Convert.ToInt16(currentUser.PKUser);
                        InvoiceHeaderObj.ModifiedDate = LastModifiedTime;
                        InvoiceHeaderObj.IVH_DEPT = Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                        InvoiceHeaderObj.BizUnitPK = Convert.ToInt16(currentUser.SBUID);
                        InvoiceHeaderObj.PaidAmountTC = 0;
                        InvoiceHeaderObj.DebitAmountTC = 0;
                        InvoiceHeaderObj.CreditAmountTC = 0;
                        InvoiceHeaderObj.ShippingCharge = Convert.ToDecimal(hdfOCFooter.Value);
                        InvoiceHeaderObj.AmountAdjust = 0;

                        if (ddlAddressType.Items.Count > 0)
                            InvoiceHeaderObj.VendorContact = Convert.ToInt32(ddlAddressType.SelectedValue);

                        if (ddlInvoiceGstType.Items.Count > 0 && Convert.ToInt32(ddlInvoiceGstType.SelectedValue) > 0)
                            InvoiceHeaderObj.GSTType = Convert.ToInt32(ddlInvoiceGstType.SelectedValue);

                        InvoiceHeaderObj.BranchType = string.IsNullOrEmpty(hdfVendorContactType.Value) ? Convert.ToByte(0) : Convert.ToByte(hdfVendorContactType.Value);
                        InvoiceHeaderObj.TaxID = string.IsNullOrEmpty(txtVatTaxId.Text) ? string.Empty : HttpUtility.HtmlEncode(txtVatTaxId.Text);
                        InvoiceHeaderObj.BranchName = txtBranchCode.Text == null ? string.Empty : HttpUtility.HtmlEncode(txtBranchCode.Text);

                        InvoiceHeaderObj.InvoiceCategory = 2;
                        InvoiceHeaderObj.InvoiceType = hdfType.Value == "" ? (byte)0 : (byte)Convert.ToInt16(hdfType.Value);
                        //InvoiceHeaderObj.IVH_VENDOR
                        InvoiceVendorMappingList = new List<InvoiceVendorMapping>();
                        InvoiceVendorMappingList = (List<InvoiceVendorMapping>)SetUIValuesToObject(ControlsEnum.FINANCEINVOICETRXMPG);

                        if (InvoiceVendorMappingList != null && InvoiceVendorMappingList.Count > 0)
                        {
                            InvoiceVendorMappingList.ForEach(dtl => InvoiceHeaderObj.InvoiceVendorMappingList.Add(dtl));
                            InvoiceHeaderObj.InvoiceVendorMappingList = InvoiceVendorMappingList;
                        }
                        retObject = InvoiceHeaderObj;

                        break;
                    #endregion
                    #region Invoice Header WorkFlow
                    case ControlsEnum.WRKFSUBMIT:
                        if (Convert.ToInt32(hdfPayByDtContinue.Value) == 0)
                        {
                            if (DateTime.Parse(txtPaybydate.Text) < DateTime.Parse(txtInvdate.Text))
                            {
                                string ErrMsg = string.Format(GetLocalResourceObject("PayByDateConfirmation").ToString());
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "PayByDateConfirm('" + ErrMsg + "');", true);
                                return 0;
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "PayByDateConfirm('" + (sender as Button).ID + "');", true);
                                //return 0;
                            }
                        }
                        InvoiceHeaderObj.IVH_PK = CurrPK;
                        updateInvoice = true;
                        if (string.IsNullOrEmpty(lblDispInvoiceNo.Text.Trim()) || lblDispInvoiceNo.Text.Trim().Equals("[NEW]"))
                        {
                            if ((byte)POGroup == (byte)POInvoiceGroup.Services)
                            {
                                numberGenerationSubType = (int)AppSubTypeCNPurchase.NONSTOCK;
                            }
                            else
                            {
                                if (POList != null)
                                {
                                    PurchaseOrderHeaderList = (List<PurchaseOrderHeader>)POList;
                                    numberGenerationSubType = (PurchaseOrderHeaderList[0].ItemType == (byte)POItemType.Others) ?
                                    (int)AppSubTypeCNPurchase.NONSTOCK :
                                    (int)AppSubTypeCNPurchase.STOCK;
                                    PurchaseOrderHeaderList[0].AST_VALUE = numberGenerationSubType;
                                }
                                else if (InvoiceMappingList != null)
                                {
                                    InvoiceVendorMappingList = (List<InvoiceVendorMapping>)InvoiceMappingList;
                                    numberGenerationSubType = (InvoiceVendorMappingList[0].PurchaseOrder.ItemType == (byte)POItemType.Others) ?
                                    (int)AppSubTypeCNPurchase.NONSTOCK :
                                    (int)AppSubTypeCNPurchase.STOCK;
                                }
                            }
                            //GetFieldValues(ControlsEnum.INVOICENO);
                            //lblDispInvoiceNo.Text = hdfInvoiceNo.Value;
                        }
                        InvoiceHeaderObj.IVH_NO = lblDispInvoiceNo.Text;
                        InvoiceHeaderObj.IVH_DATE = string.IsNullOrEmpty(txtInvdate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvdate.Text.Trim());
                        if (PoHeaderList != null)
                        {
                            PurchaseOrderHeaderList = (List<PurchaseOrderHeader>)POList;
                            InvoiceHeaderObj.IVH_VENDOR = PurchaseOrderHeaderList[0].VendorPK;
                            InvoiceHeaderObj.VendorInvoiceNo = txtSupplierInvNo.Text;
                            InvoiceHeaderObj.InvoiceCategory = (byte)POInvoiceCategory.Advanced;
                            //finInvoiceVndHdrObj.IVH_GROUP = (byte)POGroup;
                            InvoiceHeaderObj.InvoiceGroup = ((byte)POGroup) == (byte)0 ? (byte)1 : (byte)POGroup;
                            InvoiceHeaderObj.VendorAccount = PurchaseOrderHeaderList[0].Vendor.AccountPK;
                            InvoiceHeaderObj.IVH_CURRENCY = PurchaseOrderHeaderList[0].CurrencyPK;

                            if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                            {
                                InvoiceHeaderObj.Investor = PurchaseOrderHeaderList[0].Investor;
                            }

                            InvoiceHeaderObj.VendorName = PurchaseOrderHeaderList[0].Vendor.VEN_NAME;
                            InvoiceHeaderObj.VendorAddress = PurchaseOrderHeaderList[0].Vendor.Address1;
                            InvoiceHeaderObj.VendorCountryPK = PurchaseOrderHeaderList[0].Vendor.CountryPK;
                            InvoiceHeaderObj.VendorEmail = PurchaseOrderHeaderList[0].Vendor.Email;
                            InvoiceHeaderObj.VendorFAX = PurchaseOrderHeaderList[0].Vendor.Fax;
                            InvoiceHeaderObj.VendorMobile = PurchaseOrderHeaderList[0].Vendor.Mobile;
                            InvoiceHeaderObj.VendorPhone = PurchaseOrderHeaderList[0].Vendor.Phone;
                            InvoiceHeaderObj.VendorZIP = PurchaseOrderHeaderList[0].Vendor.PIN;
                        }
                        if (InvoiceMapList != null)
                        {
                            //finInvoiceVndTrxMpgList = (List<FIN_INVOICE_VND_TRX_MPG>)InvoiceMapList;
                            InvoiceVendorMappingList = (List<InvoiceVendorMapping>)InvoiceMappingList;

                            InvoiceHeaderObj.IVH_VENDOR = InvoiceVendorMappingList[0].PurchaseOrder.VendorPK;
                            InvoiceHeaderObj.VendorInvoiceNo = txtSupplierInvNo.Text;
                            InvoiceHeaderObj.VendorAccount = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.AccountPK;
                            InvoiceHeaderObj.IVH_CURRENCY = InvoiceVendorMappingList[0].PurchaseOrder.CurrencyPK;
                            if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                            {
                                InvoiceHeaderObj.Investor = InvoiceVendorMappingList[0].PurchaseOrder.Investor;
                            }
                            InvoiceHeaderObj.VendorName = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.VEN_NAME;
                            InvoiceHeaderObj.VendorAddress = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Address1;
                            InvoiceHeaderObj.VendorCountryPK = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.CountryPK;
                            InvoiceHeaderObj.VendorEmail = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Email;
                            InvoiceHeaderObj.VendorFAX = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Fax;
                            InvoiceHeaderObj.VendorMobile = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Mobile;
                            InvoiceHeaderObj.VendorPhone = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.Phone;
                            InvoiceHeaderObj.VendorZIP = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.PIN;

                        }
                        InvoiceHeaderObj.Reference = "";
                        InvoiceHeaderObj.ReceivedDate = string.IsNullOrEmpty(txtInvoiceReceivedon.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvoiceReceivedon.Text.Trim());
                        InvoiceHeaderObj.IVH_DATE_PAY_BY = string.IsNullOrEmpty(txtPaybydate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtPaybydate.Text.Trim());

                        InvoiceHeaderObj.AmountTC = string.IsNullOrEmpty(txtInvoiceAmt.Text) ? 0 : Convert.ToDecimal(txtInvoiceAmt.Text);
                        InvoiceHeaderObj.DiscountTC = string.IsNullOrEmpty(txtDiscount.Text) ? 0 : Convert.ToDecimal(txtDiscount.Text);
                        InvoiceHeaderObj.IVH_TAX_TC = string.IsNullOrEmpty(txtTaxAmount.Text) ? 0 : Convert.ToDecimal(txtTaxAmount.Text);
                        InvoiceHeaderObj.IVH_AMOUNT_NET_TC = string.IsNullOrEmpty(txtNetAmount.Text) ? 0 : Convert.ToDecimal(txtNetAmount.Text);
                        InvoiceHeaderObj.BaseCurrencyPK = currentUser.BaseCurrency <= 0 ? 1 : currentUser.BaseCurrency;

                        InvoiceHeaderObj.CompanyPK = Convert.ToInt16(ddlCompany.SelectedValue);
                        int branch1 = 0;
                        if (int.TryParse(ddlVendorBranch.SelectedValue, out branch1))
                        {
                            InvoiceHeaderObj.VendorContact = Convert.ToInt16(ddlVendorBranch.SelectedValue);
                        }
                        else
                        {
                            InvoiceHeaderObj.VendorContact = null;
                        }
                        GetFieldValues(ControlsEnum.EXCHANGERATE);
                        InvoiceHeaderObj.ExchangeRate = double.Parse(hdfExchangeRate.Value);
                        InvoiceHeaderObj.NetAmountBC = InvoiceHeaderObj.IVH_AMOUNT_NET_TC * Convert.ToDecimal(InvoiceHeaderObj.ExchangeRate);
                        InvoiceHeaderObj.Remarks = HttpUtility.HtmlEncode(txtRemarks.Text);
                        InvoiceHeaderObj.IVH_STATUS = WkfStatus;
                        InvoiceHeaderObj.IVH_DEL_STATUS = Convert.ToByte(hdfDelStatus.Value);
                        InvoiceHeaderObj.Active = 1;
                        InvoiceHeaderObj.CreatedUserPK = Convert.ToInt16(currentUser.PKUser);
                        InvoiceHeaderObj.CreatedDate = DateTime.Now;
                        InvoiceHeaderObj.ModifiedUserPK = Convert.ToInt16(currentUser.PKUser);
                        InvoiceHeaderObj.ModifiedDate = LastModifiedTime;
                        InvoiceHeaderObj.IVH_DEPT = Convert.ToInt16(Session[BusinessObject.Common.SessionStrings.CurDept].ToString());
                        InvoiceHeaderObj.BizUnitPK = Convert.ToInt16(currentUser.SBUID);
                        InvoiceHeaderObj.PaidAmountTC = 0;
                        InvoiceHeaderObj.DebitAmountTC = 0;
                        InvoiceHeaderObj.CreditAmountTC = 0;
                        InvoiceHeaderObj.ShippingCharge = Convert.ToDecimal(hdfOCFooter.Value);
                        InvoiceHeaderObj.AmountAdjust = 0;
                        InvoiceHeaderObj.InvoiceCategory = 2;
                        InvoiceHeaderObj.InvoiceType = hdfType.Value == "" ? (byte)0 : (byte)Convert.ToInt16(hdfType.Value);

                        if (ddlAddressType.Items.Count > 0)
                            InvoiceHeaderObj.VendorContact = Convert.ToInt32(ddlAddressType.SelectedValue);

                        InvoiceHeaderObj.BranchType = string.IsNullOrEmpty(hdfVendorContactType.Value) ? Convert.ToByte(0) : Convert.ToByte(hdfVendorContactType.Value);
                        InvoiceHeaderObj.TaxID = string.IsNullOrEmpty(txtVatTaxId.Text) ? string.Empty : HttpUtility.HtmlEncode(txtVatTaxId.Text);
                        InvoiceHeaderObj.BranchName = txtBranchCode.Text == null ? string.Empty : HttpUtility.HtmlEncode(txtBranchCode.Text);


                        if (chkOriginalinvoice.Checked == true)
                        {
                            InvoiceHeaderObj.OriginalReceived = 1;
                        }
                        else
                        {
                            InvoiceHeaderObj.OriginalReceived = 0;
                        }

                        //finInvoiceVndTrxMpgList = new List<FIN_INVOICE_VND_TRX_MPG>();
                        InvoiceVendorMappingList = new List<InvoiceVendorMapping>();
                        InvoiceVendorMappingList = (List<InvoiceVendorMapping>)SetUIValuesToObject(ControlsEnum.FINANCEINVOICETRXMPG);

                        if (InvoiceVendorMappingList != null && InvoiceVendorMappingList.Count > 0)
                        {
                            InvoiceVendorMappingList.ForEach(dtl => InvoiceHeaderObj.InvoiceVendorMappingList.Add(dtl));
                            InvoiceHeaderObj.InvoiceVendorMappingList = InvoiceVendorMappingList;
                        }

                        retObject = InvoiceHeaderObj;

                        break;
                    #endregion
                    #region Invoice Trx Mpg
                    case ControlsEnum.FINANCEINVOICETRXMPG:
                        int rowID = 0;
                        HiddenField hdfPONumber;
                        TextBox txtPayNow;
                        TextBox txtOthercharges;
                        HiddenField hdfPOTax;
                        HiddenField hdfPODiscount;
                        HiddenField hdfAdjustPerInvAmt;
                        foreach (GridViewRow grdrow in grdPOList.Rows)
                        {
                            //finInvoiceVndTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_VND_TRX_MPG>();
                            InvoiceVendorMappingObj = new InvoiceVendorMapping();

                            hdfPONumber = (HiddenField)grdPOList.Rows[rowID].FindControl(GetLocalResourceObject("hdfPONumber").ToString());
                            txtOthercharges = (TextBox)grdPOList.Rows[rowID].FindControl("txtOthercharges");
                            //  lblPOTax =  grdPOList.Rows[rowID].FindControl("lblPOTax") as Label;
                            hdfPOTax = (HiddenField)grdPOList.Rows[rowID].FindControl("hdfPOTax");
                            hdfPODiscount = (HiddenField)grdPOList.Rows[rowID].FindControl("hdfPODiscount");
                            hdfAdjustPerInvAmt = (HiddenField)grdPOList.Rows[rowID].FindControl("hdfAdjustPerInvAmt");
                            string s = hdfPOTax.Value;
                            InvoiceVendorMappingObj.InvoiceVendorMappingPK = CurrMpgPK;
                            InvoiceVendorMappingObj.InvoicePK = CurrPK;
                            InvoiceVendorMappingObj.PurchaseOrderPk = hdfPONumber == null ? 0 : Convert.ToInt32(hdfPONumber.Value);
                            txtPayNow = (TextBox)grdPOList.Rows[rowID].FindControl(GetLocalResourceObject("txtPayNow").ToString());
                            InvoiceVendorMappingObj.Amount = txtPayNow == null ? 0 : txtPayNow.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtPayNow.Text.Trim());
                            InvoiceVendorMappingObj.Active = 1;
                            if (IsAdvInvHasTax == false)
                            {
                                InvoiceVendorMappingObj.OtherAmount = 0;
                            }
                            else
                            {
                                InvoiceVendorMappingObj.OtherAmount = string.IsNullOrEmpty(txtOthercharges.Text) ? 0 : Convert.ToDecimal(txtOthercharges.Text);
                            }

                            InvoiceVendorMappingObj.TaxAmount = hdfPOTax == null ? 0 : Convert.ToDecimal(hdfPOTax.Value);
                            InvoiceVendorMappingObj.Discount = hdfPODiscount == null ? 0 : Convert.ToDecimal(hdfPODiscount.Value);
                            InvoiceVendorMappingObj.AdjustAmount = hdfAdjustPerInvAmt == null ? 0 : Convert.ToDecimal(hdfAdjustPerInvAmt.Value);
                            if (InvoiceVendorMappingObj.Amount > 0)
                            {
                                InvoiceVendorMappingList.Add(InvoiceVendorMappingObj);
                            }
                            rowID++;
                        }
                        retObject = InvoiceVendorMappingList;

                        break;
                    #endregion
                    #region Pick Invoice for Paying
                    case ControlsEnum.PICKFORPAYMENT:

                        foreach (GridViewRow grdrow in grdPOInvoiceList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            lnkBalAmt = grdrow.FindControl("lnkBalAmt") as LinkButton;
                            //lblBalAmt = grdrow.FindControl("lblBalAmt") as Label;

                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfDelete = (HiddenField)grdrow.FindControl("hdfDelete");
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                if (hdfDelete != null)
                                    isDelete = Convert.ToInt16(hdfDelete.Value);
                                VendorID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfVendorPK")).Value);
                                Currency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOCurrency")).Value);
                                INVTax = Convert.ToDecimal(string.IsNullOrEmpty(((HiddenField)grdrow.FindControl("hdfTaxAmount")).Value) ? "0" : ((HiddenField)grdrow.FindControl("hdfTaxAmount")).Value);
                                balamt = Convert.ToDecimal(lnkBalAmt.Text);
                                //balamt = Convert.ToDecimal(lblBalAmt.Text);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (isDelete != 1)
                            {
                                if (Approved == 2)// && Posted == true)
                                {
                                    if ((balamt > 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                                    {
                                        Iscont = false;
                                        if (!IsMatcingTax(SelectedINVTax, INVTax))
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Tax").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        else if (!IsSameCurrency(SelectedCurrency, Currency))
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        else if (!IsSameVendor(SelectedVendors, VendorID))
                                        {
                                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        }
                                        else if (!IsExixtPk(SelectedInvoices, CurrPK))
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

                                            //Add Vendors
                                            if (SelectedVendors != null)
                                            {
                                                SelectedVendorsList = SelectedVendors;
                                            }
                                            else
                                            {
                                                SelectedVendorsList = new List<long>();
                                            }
                                            SelectedVendorsList.Add(VendorID);
                                            SelectedVendors = SelectedVendorsList;

                                            //Add Invoices
                                            if (SelectedInvoices != null)
                                            {
                                                SelectedInvoiceList = SelectedInvoices;
                                            }
                                            else
                                            {
                                                SelectedInvoiceList = new List<long>();
                                            }
                                            SelectedInvoiceList.Add(CurrPK);
                                            SelectedInvoices = SelectedInvoiceList;
                                            SelectedInvoicesCount = SelectedInvoices.Count;
                                            btnPickForPayment.Text = Resources.Controls.PickPoForInvoicing;
                                            btnPickForPayment.Text = GetLocalResourceObject("PickPoForPayment").ToString() + "(" + SelectedInvoicesCount.ToString() + ")";
                                            Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                                            Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;


                                            //For Saving Selected Item PK
                                            hdfSelectedItemPk.Value = hdfSelectedItemPk.Value + "," + CurrPK.ToString();
                                        }
                                        else
                                        {
                                            litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                        }

                                    }
                                    else
                                    {
                                        Iscont = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowAlreadyPaid();});", true);
                                    }
                                }
                                else
                                {
                                    if (Approved != 2)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_Msg").ToString();
                                    }
                                    //else if (Posted == false)
                                    //{
                                    //    litErrorMsg.Text = GetLocalResourceObject("Msg_PickPaymentPost_Msg").ToString();
                                    //}
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_PickPayment_MsgDel").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }

                            //For Setting/Resetting Colour of a selected InvoiceNo
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                            //End
                        }
                        else
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Pick Inv & for Cr/Dr. Note
                    case ControlsEnum.PICKFORCRDRNOTE:

                        foreach (GridViewRow grdrow in grdPOInvoiceList.Rows)
                        {
                            RadioButton rbtn;
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                bIsChecked = true;
                                CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                VendorID = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfVendorPK")).Value);
                                Currency = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfPOCurrency")).Value);
                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            if (Approved == 2)
                            {
                                if (!IsSameCurrency(SelectedCurrency, Currency))
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Currency").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (!IsSameVendor(SelectedVendors, VendorID))
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_Vendor").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                else if (!IsExixtPk(SelectedInvoicesCrDr, CurrPK))
                                {
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

                                    //Add Vendors
                                    if (SelectedVendors != null)
                                    {
                                        SelectedVendorsList = SelectedVendors;
                                    }
                                    else
                                    {
                                        SelectedVendorsList = new List<long>();
                                    }
                                    SelectedVendorsList.Add(VendorID);
                                    SelectedVendors = SelectedVendorsList;

                                    //Add Invoices
                                    if (SelectedInvoicesCrDr != null)
                                    {
                                        SelectedInvoiceCrDrList = SelectedInvoicesCrDr;
                                    }
                                    else
                                    {
                                        SelectedInvoiceCrDrList = new List<long>();
                                    }
                                    SelectedInvoiceCrDrList.Add(CurrPK);
                                    SelectedInvoicesCrDr = SelectedInvoiceCrDrList;
                                    SelectedInvoicesCrDrCount = SelectedInvoicesCrDr.Count;
                                    btnPickForCrDrNote.Text = Resources.Controls.PickPoForCrDr;
                                    btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString() + "(" + SelectedInvoicesCrDrCount.ToString() + ")";
                                    Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;
                                    Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.ErrorMessages.Msg_Item_Added;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                }
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
                    #region Journalize
                    case ControlsEnum.JOURNALIZE:
                        ////Start
                        if (EntryStatus == EntryStatus.LISTMODE)
                        {
                            ////

                            foreach (GridViewRow grdrow in grdPOInvoiceList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value);
                                    Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                    //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                    HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                    if (hdfPosted != null)
                                        Posted = Convert.ToBoolean(hdfPosted.Value);
                                    Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                    Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).Text;
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
                                FIN_INVOICE_VND_HDR tempFinInvoiceVndHdrObj = finInvoiceVndHdrList.SingleOrDefault(aa => aa.IVH_PK == CurrPK);
                                if (tempFinInvoiceVndHdrObj != null)
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
                                    //Journalize New sessions End

                                    Invoice = POGroup == POInvoiceGroup.Goods ? ApplicationType.PIJ
                                        : POGroup == POInvoiceGroup.Services ? ApplicationType.PSIJ : ApplicationType.EIJ;
                                    ucrJournalize.TransactionType = Invoice;
                                    Session[ERP.Utilities.SessionStrings.TransactionType] = Invoice;
                                    ucrJournalize.TransactionPK = CurrPK;
                                    Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                    ucrJournalize.JournalizePK = 0;
                                    Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                    Session[ERP.Utilities.SessionStrings.TransactionNo] = tempFinInvoiceVndHdrObj.IVH_NO;
                                    Session[ERP.Utilities.SessionStrings.TransactionDate] = tempFinInvoiceVndHdrObj.IVH_DATE;
                                    Session[ERP.Utilities.SessionStrings.TransactionCurrency] = tempFinInvoiceVndHdrObj.IVH_CURRENCY;
                                    Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.VND;
                                    Session[ERP.Utilities.SessionStrings.AccountPayablePK] = tempFinInvoiceVndHdrObj.IVH_VENDOR;
                                    Session[ERP.Utilities.SessionStrings.JournalType] = POGroup == POInvoiceGroup.Goods ? ApplicationType.PIJ
                                        : POGroup == POInvoiceGroup.Services ? ApplicationType.PSIJ : ApplicationType.EIJ;
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
                                    if ((byte)POGroup == (byte)POInvoiceGroup.Services)
                                    {
                                        numberGenerationSubType = 0;// (int)AppSubTypeCNPurchase.NONSTOCK;//12
                                    }
                                    else
                                    {
                                        if (PoHeaderList != null)
                                        {
                                            PurchaseOrderHeaderList = (List<PurchaseOrderHeader>)POList;
                                            numberGenerationSubType = (PurchaseOrderHeaderList[0].ItemType == (byte)POItemType.Others) ?
                                            (int)AppSubTypeCNPurchase.NONSTOCK :
                                            (int)AppSubTypeCNPurchase.STOCK;
                                        }
                                        if (InvoiceMapList != null)
                                        {
                                            finInvoiceVndTrxMpgList = (List<FIN_INVOICE_VND_TRX_MPG>)InvoiceMapList;
                                            numberGenerationSubType = (finInvoiceVndTrxMpgList[0].PUR_ORDER_HDR.POH_ITEM_TYPE == (byte)POItemType.Others) ?
                                            (int)AppSubTypeCNPurchase.NONSTOCK :
                                            (int)AppSubTypeCNPurchase.STOCK;
                                        }
                                    }
                                    ucrJournalize.TypeForNumberGenaration = numberGenerationSubType.ToString();// ((int)AppSubTypeCNPurchase.NONSTOCK).ToString();
                                    ucrJournalize.CallUserControl();
                                    EntryStatus = EntryStatus.LISTMODE;
                                    Session["drcontrols"] = null;
                                    Session["crcontrols"] = null;
                                    Session["removedcontrols"] = null;
                                    Session[ERP.Utilities.SessionStrings.JournalHead] = GetLocalResourceObject("Purchase_Invoice_Journal").ToString();

                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                }

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
                        appType = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
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
                            alertBoObj.ATH_NARRATION = TypeRef.Trim();
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
        /// Is Same Vendor
        /// </summary>
        /// <param name="vendors"></param>
        /// <param name="pk"></param>
        /// <returns></returns>
        private bool IsSameVendor(List<long> vendors, long pk)
        {

            bool flag = true;
            if (vendors != null)
                foreach (long ven in vendors)
                    if (ven != pk)
                    {
                        flag = false;
                        break;
                    }
            return flag;
        }

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
                    #region Invoice Header
                    case ControlsEnum.POINVOICEDETAILS:
                        if (finInvoiceVndHdrList != null && finInvoiceVndHdrList.Count > 0)
                        {
                            CurrPK = int.Parse(finInvoiceVndHdrList[0].IVH_PK.ToString());
                            POGroup = (POInvoiceGroup)Enum.Parse(typeof(POInvoiceGroup), finInvoiceVndHdrList[0].IVH_GROUP.ToString());
                            lblDispInvoiceNo.Text = finInvoiceVndHdrList[0].IVH_NO == "" ? "[NEW]" : finInvoiceVndHdrList[0].IVH_NO;
                            lblDispSupplierName.Text = ERP.Utilities.CommonFunctions.GetShortString(finInvoiceVndHdrList[0].PUR_VENDOR_MST.VEN_NAME, 46);
                            lblDispSupplierName.ToolTip = finInvoiceVndHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                            txtSupplierInvNo.Text = finInvoiceVndHdrList[0].IVH_VENDOR_INV_NO;
                            txtPaybydate.Text = finInvoiceVndHdrList[0].IVH_DATE_PAY_BY.ToString(Resources.Constants.DateFormatShort);
                            lblInvoiceAmt.Text = GetLocalResourceObject("InvoiceAmt").ToString() + " (" + finInvoiceVndHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                            lblDiscount.Text = GetLocalResourceObject("InvDiscount").ToString() + " (" + finInvoiceVndHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                            lblTaxAmount.Text = GetLocalResourceObject("TaxAmount").ToString() + " (" + finInvoiceVndHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                            lblNetAmount.Text = GetLocalResourceObject("NetAmount").ToString() + " (" + finInvoiceVndHdrList[0].ADM_CURRENCY_MST1.CUR_CODE + ")";
                            txtInvoiceAmt.Text = GetFormattedCurrency(finInvoiceVndHdrList[0].IVH_AMOUNT_TC);
                            txtDiscount.Text = GetFormattedCurrency(finInvoiceVndHdrList[0].IVH_DISCOUNT_TC);
                            txtInvdate.Text = finInvoiceVndHdrList[0].IVH_DATE.ToString(Resources.Constants.DateFormatShort);
                            txtTaxAmount.Text = GetFormattedCurrency(finInvoiceVndHdrList[0].IVH_TAX_TC);
                            txtNetAmount.Text = GetFormattedCurrency(finInvoiceVndHdrList[0].IVH_AMOUNT_NET_TC);
                            txtRemarks.Text = HttpUtility.HtmlDecode(finInvoiceVndHdrList[0].IVH_REMARKS);
                            txtInvoiceReceivedon.Text = finInvoiceVndHdrList[0].IVH_DATE_RECEIVED.ToString(Resources.Constants.DateFormatShort);
                            LastModifiedTime = finInvoiceVndHdrList[0].IVH_MOD_DT;
                            if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                            {
                                txtInvestor.Text = finInvoiceVndHdrList[0].IVH_INVESTOR;
                            }
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            hdfInvPaidAmt.Value = finInvoiceVndHdrList[0].IVH_AMOUNT_PAID_TC.ToString();
                            hdfIsJournalize.Value = finInvoiceVndHdrList[0].IVH_HAS_JRNL_ENTRY.ToString();
                            ddlCompany.SelectedValue = finInvoiceVndHdrList[0].IVH_COMPANY.ToString();
                            chkOriginalinvoice.Checked = finInvoiceVndHdrList[0].IVH_ORGINAL_RCVD == (byte)1;

                            vndPK = finInvoiceVndHdrList[0].PUR_VENDOR_MST.VEN_PK;
                            vPK = finInvoiceVndHdrList[0].PUR_VENDOR_MST.VEN_PK;
                            divVendorBranch.Visible = true;
                            GetFieldValues(ControlsEnum.VENDORBRANCH);
                            SetFieldValues(ControlsEnum.VENDORBRANCH);

                            if (Convert.ToInt32(hdfIsMultiplePlant.Value) == 1)
                            {
                                lblCompanyView.Visible = true;
                                ddlCompanyView.Visible = true;
                                ddlCompanyView.Enabled = true;
                                ddlCompanyView.SelectedValue = finInvoiceVndHdrList[0].IVH_COMPANY.ToString();
                                ddlCompanyView.Enabled = false;
                            }
                            else
                            {
                                lblCompanyView.Visible = false;
                                ddlCompanyView.Visible = false;
                            }

                            if (ddlVendorBranch.Items.Count > 0)
                            {
                                ddlVendorBranch.SelectedValue = finInvoiceVndHdrList[0].IVH_VENDOR_CONTACT.ToString();
                            }
                            hdfType.Value = finInvoiceVndHdrList[0].IVH_TYPE.ToString();

                            Approved = WkfStatus = finInvoiceVndHdrList[0].IVH_STATUS;
                            hdfDelStatus.Value = finInvoiceVndHdrList[0].IVH_DEL_STATUS.ToString();

                            GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            SetFieldValues(ControlsEnum.VENDORCONTACTYPE);

                            if (ddlAddressType.Items.Count > 0)
                            {
                                if (finInvoiceVndHdrList[0].IVH_VENDOR_CONTACT > 0)
                                    ddlAddressType.SelectedValue = finInvoiceVndHdrList[0].IVH_VENDOR_CONTACT.ToString();
                            }

                            if (ddlInvoiceGstType.Items.Count > 0)
                            {
                                ddlInvoiceGstType.ClearSelection();
                                if (finInvoiceVndHdrList[0].IVH_GST_TYPE > 0)
                                    ddlInvoiceGstType.SelectedValue = finInvoiceVndHdrList[0].IVH_GST_TYPE.ToString();
                            }
                            SetBranchCodeVisibility();

                            txtVatTaxId.Text = string.IsNullOrEmpty(finInvoiceVndHdrList[0].IVH_TAX_ID) ? string.Empty : finInvoiceVndHdrList[0].IVH_TAX_ID.ToString();
                            txtBranchCode.Text = string.IsNullOrEmpty(finInvoiceVndHdrList[0].IVH_BRANCH_TEXT) ? string.Empty : finInvoiceVndHdrList[0].IVH_BRANCH_TEXT.ToString();
                            hdfVendorContactType.Value = finInvoiceVndHdrList[0].IVH_BRANCH_TYPE.ToString();

                        }
                        break;
                    #endregion

                    //#region UPLOADEDFILES
                    //case ControlsEnum.UPLOADEDFILES:
                    //    CurrPK = (int)finInvoiceVndHdrObj.IVH_PK;
                    //    //POUploadList = finInvoiceVndHdrObj.FileList;
                    //    break; 
                    //#endregion

                    #region SELECTED DOC
                    case ControlsEnum.SELECTEDDOC:
                        if (admDocAttachObj != null)
                        {
                            CurrSlNo = admDocAttachObj.DOC_SEQ_NO;
                            anchorFile.Visible = true;
                            vrfFileUpload.Enabled = false;
                            anchorFile.InnerHtml = admDocAttachObj.DOC_NAME;
                            anchorFile.HRef = admDocAttachObj.DOC_PATH;
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

                        ddlCompanyView.DataSource = CommonFunctions.HtmlDecode(admCompanyMstList, Resources.DataFieldRes.CompanySpecs);
                        ddlCompanyView.DataTextField = Resources.DataFieldRes.CompanySpecs;
                        ddlCompanyView.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                        ddlCompanyView.DataBind();
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
                    ddlCompanySrch.Items.Insert(0, new ListItem(Resources.Report.SelectAll, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Invoice GST Type
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
                    #region Bind Invoice Hdr List
                    case ControlsEnum.INVOICEHDR:
                        if (InvoiceHeaderList != null) //(finInvoiceVndHdrList != null)
                        {
                            GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                            uclPaging.TotalPages = TotalPages;
                            PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                            uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                            grdPOInvoiceList.DataSource = InvoiceHeaderList;// finInvoiceVndHdrList;
                            grdPOInvoiceList.DataBind();
                            uclPaging.Visible = true;
                            uclPaging.BindPager();


                            //For Setting/Resetting Colour of a selected InvoiceNo
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                            //End
                        }
                        else
                        {
                            grdPOInvoiceList.DataSource = null;// finInvoiceVndHdrList;
                            grdPOInvoiceList.DataBind();
                        }
                        //if (Convert.ToInt32(dsPageData.Tables[0].Rows[0][0]) > 0)
                        //{
                        //    GetFieldValues(ControlsEnum.FILLWORKFLOWSTATUS);
                        //    uclPaging.TotalPages = TotalPages;
                        //    PageIndex = PageIndex == null ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        //    uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        //    grdPOInvoiceList.DataSource = dsPageData.Tables[1];
                        //    grdPOInvoiceList.DataBind();
                        //    uclPaging.Visible = true;ctl00_MainContent_grdPOInvoiceList                        //    uclPaging.BindPager();

                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        //}
                        break;
                    #endregion
                    #region PO Invoice Details List
                    case ControlsEnum.POINVOICELIST:
                        if (InvoiceVendorMappingList != null && InvoiceVendorMappingList.Count > 0)
                        {
                            grdPOList.DataSource = InvoiceVendorMappingList;
                            grdPOList.DataBind();
                        }
                        else
                        {
                            grdPOList.DataSource = null;
                            grdPOList.DataBind();
                        }
                        if (PurchaseOrderHeaderList != null && PurchaseOrderHeaderList.Any()) //(PurOrderHdrList != null && PurOrderHdrList.Count > 0)
                        {
                            ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(PurchaseOrderHeaderList[0].CompanyPK.ToString()));
                            ddlCompanyView.SelectedIndex = ddlCompanyView.Items.IndexOf(ddlCompanyView.Items.FindByValue(PurchaseOrderHeaderList[0].CompanyPK.ToString()));
                            lblDispSupplierName.Text = ERP.Utilities.CommonFunctions.GetShortString(PurchaseOrderHeaderList[0].Vendor.VEN_NAME, 46);
                            lblDispSupplierName.ToolTip = PurchaseOrderHeaderList[0].Vendor.VEN_NAME;
                            lblInvoiceAmt.Text = GetLocalResourceObject("InvoiceAmt").ToString() + " (" + PurchaseOrderHeaderList[0].Currency.CUR_CODE + ")";
                            lblDiscount.Text = GetLocalResourceObject("InvDiscount").ToString() + " (" + PurchaseOrderHeaderList[0].Currency.CUR_CODE + ")";
                            lblTaxAmount.Text = GetLocalResourceObject("TaxAmount").ToString() + " (" + PurchaseOrderHeaderList[0].Currency.CUR_CODE + ")";
                            lblNetAmount.Text = GetLocalResourceObject("NetAmount").ToString() + " (" + PurchaseOrderHeaderList[0].Currency.CUR_CODE + ")";
                            grdPOList.DataSource = PurchaseOrderHeaderList;
                            grdPOList.DataBind();

                            if (Convert.ToInt32(hdfShowInvestor.Value) == 1)
                            {
                                txtInvestor.Text = PurchaseOrderHeaderList[0].Investor;
                            }
                            //if (PurOrderHdrList[0].POH_TYPE == (byte)PurchaseType.Local)
                            //{
                            vndPK = PurchaseOrderHeaderList[0].Vendor.VendorPK;
                            vPK = PurchaseOrderHeaderList[0].Vendor.VendorPK;
                            divVendorBranch.Visible = true;
                            GetFieldValues(ControlsEnum.VENDORBRANCH);
                            SetFieldValues(ControlsEnum.VENDORBRANCH);


                            GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            SetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            //}
                            //else
                            //{
                            //    //divVendorBranch.Visible = false;
                            //}
                            hdfType.Value = PurchaseOrderHeaderList[0].Type.ToString();

                        }
                        else if (InvoiceVendorMappingList != null && InvoiceVendorMappingList.Count > 0)
                        {
                            //if (InvoiceVendorMappingList[0].PUR_ORDER_HDR.POH_TYPE == (byte)PurchaseType.Local)
                            //{
                            vndPK = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.VendorPK;
                            vPK = InvoiceVendorMappingList[0].PurchaseOrder.Vendor.VendorPK;
                            divVendorBranch.Visible = true;
                            GetFieldValues(ControlsEnum.VENDORBRANCH);
                            SetFieldValues(ControlsEnum.VENDORBRANCH);

                            if (ddlVendorBranch.Items.Count > 0)
                            {
                                ddlVendorBranch.SelectedValue = InvoiceVendorMappingList[0].VendorHeader.VendorContact.ToString();
                            }


                            //GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
                            //SetFieldValues(ControlsEnum.VENDORCONTACTYPE);

                            //if (finInvoiceVndHdrList[0].IVH_BRANCH_TYPE > 0)
                            //    ddlAddressType.SelectedValue = finInvoiceVndHdrList[0].IVH_BRANCH_TYPE.ToString();
                            //txtVatTaxId.Text = finInvoiceVndHdrList[0].IVH_TAX_ID == null ? "" : HttpUtility.HtmlDecode(finInvoiceVndHdrList[0].IVH_TAX_ID.ToString());
                            //txtBranchCode.Text = finInvoiceVndHdrList[0].IVH_BRANCH_TEXT == null ? "" : HttpUtility.HtmlDecode(finInvoiceVndHdrList[0].IVH_BRANCH_TEXT.ToString());



                            //}
                            //else
                            //{
                            //    divVendorBranch.Visible = false;
                            //}
                            hdfType.Value = InvoiceVendorMappingList[0].PurchaseOrder.Type.ToString();
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotal", "$(document).ready(function(){CalculateTotal(0);});", true);
                        break;
                    #endregion
                    #region UPLOADED FILES
                    case ControlsEnum.UPLOADEDFILES:
                        //if (DocAttachList != null)
                        //{                    
                        grdUploads.DataSource = DocAttachList;
                        grdUploads.DataBind();
                        //}
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
                //FillProcessID(1);
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
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm()
        {
            CurrPK = 0;
            hdfIVHPK.Value = "";
            ModifiedDatePnl.Visible = false;
            txtSupplierInvNo.Text = "";
            txtInvoiceNumber.Text = "Select/Type";
            hdfIVHPK.Value = "";
            txtVendor.Text = "Select/Type";
            txtpoNo.Text = string.Empty;
            hdfVendorID.Value = "";

            hdfIsContDupVenInvNo.Value = "0";

            txtFromDate.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfFromDate.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfToDate.Value = DateTime.Now.ToString();
            chkOriginalinvoice.Checked = false;
            //txtFromDate.Text = string.Empty;
            //hdfFromDate.Value = string.Empty;
            //txtToDate.Text = string.Empty;
            //hdfToDate.Value = string.Empty;
            GetFieldValues(ControlsEnum.FINPERIOD);
            //if (finYearMstList != null && finYearMstList.Count > 0)
            //{
            //    txtPaybydateFrom.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
            //    hdfPaybydateFrom.Value = finYearMstList[0].FYR_DATE_FROM.ToString();
            //    txtPayByToDate.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
            //    hdfPayByToDate.Value = finYearMstList[0].FYR_DATE_TO.ToString();
            //}
            //else
            //{
            //    //txtPaybydateFrom.Text = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            //    //hdfPaybydateFrom.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).AddMonths(-1).ToString();
            //    //txtPayByToDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            //    //hdfPayByToDate.Value = DateTime.Now.ToString();
            //}
            //GetFieldValues(ControlsEnum.FINPERIOD);
            //SetFieldValues(ControlsEnum.FINPERIOD);
            txtPaybydateFrom.Text = string.Empty;
            txtPayByToDate.Text = string.Empty;
            ddlStatus.SelectedValue = "3";
            base.WkfRefID = 0;
            hdfTaxSettings.Value = string.Empty;
            FileDetailsList = null;
            DocAttachList = null;
            ResetForm(ControlsEnum.ADDITEM);
            ddlCompanySrch.SelectedIndex = -1;
            hdfIsCancel.Value = "0";
        }
        /// <summary>
        /// Method used to Reset Dorm Controls
        /// </summary>
        private void ResetForm(ControlsEnum controlType)
        {
            switch (controlType)
            {
                case ControlsEnum.ADDITEM:
                    //ddlType.ClearSelection();
                    anchorFile.Visible = false;
                    vrfFileUpload.Enabled = true;
                    CurrSlNo = 0;
                    anchorFile.Attributes.Remove("onclick");
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
        public string GetFormattedCurrencyWithComa(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            // return num.ToString(hdfCurrencyFormat.Value);
            string resultNum = string.Format("{0:c}", Convert.ToDecimal(num.ToString(hdfCurrencyFormat.Value)));
            return resultNum;
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
        private void CheckUserRightsAndRedirect(string RedirectUrl, string Url2 = null)
        {
            CommonBL userAuth = new CommonBL();
            if (currentUser == null)
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            UserRightsBO UsrRights = userAuth.GetUserPageRights(currentUser.PKUser, RedirectUrl.Replace("~", ""), currentUser.SBUID, currentUser.CurrentDeptPK, Url2);
            if (UsrRights != null && UsrRights.Rights.Count() > 0 && UsrRights.Rights[0].UserDeptRight)
                Response.Redirect(RedirectUrl, false);
            else
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErpRes.NoPressionMsg) + "','" + Resources.ErpRes.Information + "');", true);
        }

        private string GetDOCMODE()
        {
            CommonService cm = new CommonService();
            List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList;
            string APT_CODE = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
            AppTypeDetailsList = cm.GetReportParameters(APT_CODE, 0, DateTime.Now);
            if (AppTypeDetailsList.Count > 0)
            {
                return AppTypeDetailsList[0].AST_DOC_MODE.ToString();
            }
            else
            {
                return "0";
            }
        }

        private void SaveTransaction(InvoiceHdrBO invBo, int workflowFlag)
        {
            int? result = 0;
            string action;
            int alertresult;
            CommonService CommonServiceClient;
            string savePath = string.Empty;
            long? docSaveResult;
            POInvoiceService poInvoiceServiceClient;

            CommonServiceClient = null;
            try
            {
                WorkflowDetails wkfDetails = null;

                if (invBo == null)
                    invBo = new InvoiceHdrBO();

                #region New workflow Submition

                wkfDetails = ucrWrkf.GetWorkflowDetails();
                invBo.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
                invBo.WKF_APPLICATION = CurrPK;
                invBo.WKF_COMMENTS = wkfDetails.Comments;
                invBo.WKF_PROCESS = wkfDetails.ProcessID;
                invBo.WKF_TRX_FLAG = workflowFlag;
                invBo.WKF_REFERENCE = wkfDetails.ReferenceID;
                invBo.WKF_TASK = wkfDetails.TaskID;
                invBo.WKF_TASK_ACTION = wkfDetails.ActionID;
                action = wkfDetails.ActionText;

                #endregion

                invBo.APT_CODE = ApplicationType.PI;
                invBo.AST_VALUE = numberGenerationSubType;
                invBo.AST_DOC_MODE = GetDOCMODE();

                string xmlDoc = CommonFunctions.XmlSerialize<InvoiceHdrBO>(invBo);
                string invNumber = string.Empty;
                result = BusinessLogic.POInvoicing.POInvoiceBL.SaveAdvanceInvoiceWkf(xmlDoc, out invNumber);

                if (result.HasValue && result.Value > 0)
                {
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

                    #region ATTACHMENT SAVE
                    poInvoiceServiceClient = new POInvoiceService();
                    poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);

                    if (DocAttachList != null && DocAttachList.Count > 0)
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

                        foreach (ADM_DOC_ATTACH obj in DocAttachList)
                        {
                            string[] docName = obj.DOC_PATH.Split('/');
                            string filePath = savePath + obj.DOC_NAME;
                            if (docName.Length > 0)
                                filePath = savePath + docName[docName.Length - 1];
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
                        docSaveResult = poInvoiceServiceClient.SaveDocAttachemts(DocAttachList, (int)result);
                    }
                    #endregion

                    ucrWrkf.ApplicationID = result.Value;
                    if (hdfIsSaveSubmit.Value != CommonConstants.SELECT_VALUE_ONE)
                    {
                        GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                        invNumber = finInvoiceVndHdrList.First().IVH_NO;
                    }
                    litErrorMsg.Text = GetLocalResourceObject("Msg_Submit_Success").ToString();
                    object[] args = new object[2];
                    args[0] = Resources.PageNameRes.Invoice;
                    args[1] = invNumber;

                    #region LOG SAVE
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

                    CommonServiceClient = new CommonService();
                    CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                    List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                    AdmTrxLogDet.ATL_APP_TRX_CODE = lblDispInvoiceNo.Text;
                    AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.PI;
                    if (hdfType.Value == "2")
                    {
                        AdmTrxLogDet.ATL_APP_SUB_TYPE = 13;
                    }
                    else if (hdfType.Value == "1")
                    {
                        AdmTrxLogDet.ATL_APP_SUB_TYPE = 12;
                    }
                    AdmTrxLogDet.ATL_MOD_BY = currentUser.PKUser;
                    AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                    AdmTrxLogDet.ATL_BIZUNIT = currentUser.SBUID;
                    AdmTrxLogDet.ATL_APP_TRX_PK = ucrWrkf.ApplicationID;
                    AdmTrxLogDet.ATL_PK = 0;
                    AdmTrxLogList.Add(AdmTrxLogDet);
                    CommonServiceClient.SaveLog(AdmTrxLogList);

                    #endregion

                    if (workflowFlag == Convert.ToInt32(WorkflowTransactionFlag.SAVE))
                    {
                        litErrorMsg.Text = string.Format(Resources.Messages.Msg_Save_Success, Resources.PageNameRes.Invoice);
                    }
                    else
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);

                    #region Inbox or Listing Page Redirection
                    if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                    {
                        ResetForm();
                        Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                        ViewState[ViewstateStrings.SelectedPosCount] = null;

                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InboxURL));
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                                    + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup(); ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                        EntryStatus = EntryStatus.LISTMODE;
                        ResetForm();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        btnNew.Focus();
                        Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                        ViewState[ViewstateStrings.SelectedPosCount] = null;
                    }
                    #endregion
                }
                else if (result.HasValue)
                {
                    if (result.Value == (int)DbSaveStatus.SQLERROR)
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result.Value == (int)DbSaveStatus.CONCURRENCY)
                    {
                        litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result.Value == (int)DbSaveStatus.CODEEXIST)
                    {
                        litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result.Value == (int)DbSaveStatus.AMOUNTEXCEEDS)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("AmountExceeds").ToString())
                        + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    else if (result.Value == -21)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Msg_GroupDifference").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        return;
                    }
                    else if (result.Value == -22)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Err_AdvanceInvAmt").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        return;
                    }
                    else if (result.Value == (int)DbSaveStatus.AMOUNTEXCEEDED)
                    {
                        litErrorMsg.Text = GetLocalResourceObject("OtherChargesExceeds").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        return;
                    }

                    else
                    {
                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                            + "','" + Resources.ErpRes.Information + "');", true);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {

                throw ex;
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

            POListService poListServiceClient;
            poListServiceClient = null;

            POInvoiceService poInvoiceServiceClient;
            poInvoiceServiceClient = null;

            CommonService CommonServiceClient;
            CommonServiceClient = null;
            try
            {
                GridViewRow gvr;
                GridView grd;

                bool bIsChecked = false;
                DropDownList ddlWkfAction;
                long? result;
                long? docSaveResult;
                int alertresult;
                int selectedItemPK;
                string action;
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
                            hdfIsInvCancelled.Value = "1";
                        }
                        else
                        {
                            btnSave.Visible = true;
                            hdfIsInvCancelled.Value = "0";
                        }
                        //Edit 28-08_2014
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);

                        //End


                        break;
                    #endregion
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        if (Convert.ToInt32(hdfPayByDtContinue.Value) == 0)
                        {
                            if (DateTime.Parse(txtPaybydate.Text) < DateTime.Parse(txtInvdate.Text))
                            {
                                //string ErrMsg = string.Format(GetLocalResourceObject("PayByDateConfirmation").ToString());
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "PayByDateConfirm('" + ErrMsg + "');", true);
                                //return 0;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "PayByDateConfirm('" + (sender as Button).ID + "');", true);
                                return;
                            }
                        }
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            bool cont = false;
                            if (ddlAddressType.Items.Count <= 0)
                            {
                                cont = false;

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_ReqType").ToString()) + "');", true);
                                return;
                            }
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

                                if (Convert.ToBoolean(GetLocalResourceObject("OtherChargeVal")) && OtherChrg > InvNow)
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
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                "ClosePopup();", true);
                                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        break;
                                    }
                                }
                                else
                                {
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

                                    if (grdPOList.Rows.Count > 0)
                                    {

                                        NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                                        TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                                        lblTotalPayNowFooter = (Label)grdPOList.FooterRow.FindControl("lblTotalPayNowFooter");
                                        //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                                        lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComa(TotalAmount);
                                        if (NetAmount == TotalAmount)
                                        {
                                            InvoiceHeaderList = new List<InvoiceHdr>();
                                            poInvoiceServiceClient = new POInvoiceService();
                                            poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                            //finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                                            //finInvoiceVndHdrObj = (FIN_INVOICE_VND_HDR)SetUIValuesToObject(ControlsEnum.FINANCEINVOICEHDR);
                                            InvoiceHeaderObj = new InvoiceHdr();
                                            InvoiceHeaderObj = (InvoiceHdr)SetUIValuesToObject(ControlsEnum.FINANCEINVOICEHDR);

                                            if (InvoiceHeaderObj != null)
                                            {
                                                //Checking: allow to save vendor invoice no duplication or not
                                                if (hdfIsContDupVenInvNo.Value != "1")
                                                {
                                                    //Check For Vendor Invoice Exist
                                                    if (!string.IsNullOrEmpty(InvoiceHeaderObj.VendorInvoiceNo))
                                                        if (poInvoiceServiceClient.IsExistVendorInvoice(InvoiceHeaderObj.VendorInvoiceNo, InvoiceHeaderObj.IVH_VENDOR, InvoiceHeaderObj.IVH_PK))
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                  "ClosePopup();", true);
                                                            //litErrorMsg.Text = GetLocalResourceObject("Msg_VendorInvoiceExist").ToString();
                                                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "$(document).ready(function(){ShowDuplicateVendorInvNoContinue(1);});", true);
                                                            return;
                                                        }
                                                }

                                                if (InvoiceHeaderObj.InvoiceVendorMappingList.ToList().Count > 0)
                                                {
                                                    InvoiceHeaderList.Add(InvoiceHeaderObj);

                                                    int Archiveresult = 0;
                                                    if (InvoiceHeaderObj.IVH_PK > 0 && InvoiceHeaderObj.IVH_STATUS > 0)
                                                    {
                                                        //After getting entry into workflow, for each update keep version details of voucher for Audit trail 
                                                        Archiveresult = BusinessLogic.POInvoicing.POInvoiceBL.SaveInvoiceArchiveDetails(InvoiceHeaderObj.IVH_PK);
                                                    }
                                                    if ((Archiveresult > 0 && InvoiceHeaderObj.IVH_STATUS > 0) || InvoiceHeaderObj.IVH_STATUS == 0)
                                                    {
                                                        //result = poInvoiceServiceClient.SaveInvoiceHdr(finInvoiceVndHdrList, false, IsAdvInvHasTax);

                                                        InvoiceHdrBO invBo = new InvoiceHdrBO();
                                                        invBo.InvoiceHdrList = InvoiceHeaderList;
                                                        invBo.WKF_FLAG = 0;
                                                        SaveTransaction(invBo, Convert.ToInt32(WorkflowTransactionFlag.SAVE));

                                                        #region Commented
                                                        //if (result.HasValue && result.Value > 0)
                                                        //{
                                                        //    #region ATTACHMENT SAVE
                                                        //    if (DocAttachList != null && DocAttachList.Count > 0)
                                                        //    {
                                                        //        savePath = string.Empty;
                                                        //        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                                        //        {
                                                        //            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                        //            if (!Directory.Exists(savePath))
                                                        //                Directory.CreateDirectory(savePath);
                                                        //            savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                                        //        }
                                                        //        else
                                                        //        {
                                                        //            savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                                                        //        }

                                                        //        foreach (ADM_DOC_ATTACH obj in DocAttachList)
                                                        //        {
                                                        //            string[] docName = obj.DOC_PATH.Split('/');
                                                        //            string filePath = savePath + obj.DOC_NAME;
                                                        //            if (docName.Length > 0)
                                                        //                filePath = savePath + docName[docName.Length - 1];
                                                        //            FileInfo attachedFileInfo = new FileInfo(filePath);
                                                        //            if (FileDetailsList != null)
                                                        //            {
                                                        //                FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                        //                if (fileDetailsObj != null)
                                                        //                {
                                                        //                    fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                                        //                }
                                                        //            }
                                                        //        }
                                                        //        docSaveResult = poInvoiceServiceClient.SaveDocAttachemts(DocAttachList, (int)result);
                                                        //    }
                                                        //    #endregion

                                                        //    #region LOG SAVE
                                                        //    CommonServiceClient = new CommonService();
                                                        //    CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                                                        //    ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                                        //    List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                                        //    if (CurrPK > 0)
                                                        //    {
                                                        //        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.UPDATE;
                                                        //    }
                                                        //    else
                                                        //    {
                                                        //        AdmTrxLogDet.ATL_ACTION = (byte)LogAction.NEW;
                                                        //    }
                                                        //    AdmTrxLogDet.ATL_APP_TRX_CODE = (string.IsNullOrEmpty(lblDispInvoiceNo.Text) || lblDispInvoiceNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                        //                                        : lblDispInvoiceNo.Text.Trim(); ;
                                                        //    AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.PI;
                                                        //    if (hdfType.Value == "2")
                                                        //    {
                                                        //        AdmTrxLogDet.ATL_APP_SUB_TYPE = 13;
                                                        //    }
                                                        //    else if (hdfType.Value == "1")
                                                        //    {
                                                        //        AdmTrxLogDet.ATL_APP_SUB_TYPE = 12;
                                                        //    }
                                                        //    AdmTrxLogDet.ATL_MOD_BY = currentUser.PKUser;
                                                        //    AdmTrxLogDet.ATL_MOD_DT = DateTime.Now;
                                                        //    AdmTrxLogDet.ATL_BIZUNIT = currentUser.SBUID;
                                                        //    AdmTrxLogDet.ATL_APP_TRX_PK = Convert.ToInt64(result);
                                                        //    AdmTrxLogDet.ATL_PK = 0;
                                                        //    AdmTrxLogList.Add(AdmTrxLogDet);
                                                        //    CommonServiceClient.SaveLog(AdmTrxLogList);
                                                        //    #endregion

                                                        //    #region WkfSummarySave
                                                        //    int resultSummary = BusinessLogic.CommonManagement.CommonBL.SaveSummary(result.Value, Convert.ToInt32(hdfProcessID.Value));
                                                        //    if (resultSummary <= 0)
                                                        //    {

                                                        //        litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                                        //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                        //    }
                                                        //    #endregion

                                                        //    SelectedPos = null;
                                                        //    InvoiceMapList = null;
                                                        //    // Session[ERP.Utilities.SessionStrings.InvoiceMapList] = null;
                                                        //    litErrorMsg.Text = Resources.Messages.Msg_Save_Success;
                                                        //    litErrorMsg.Text = string.Format(litErrorMsg.Text.ToLower(), Resources.PageNameRes.InvoiceHdr);
                                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                        //    EntryStatus = EntryStatus.LISTMODE;
                                                        //    ResetForm();
                                                        //    GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                                        //    GetFieldValues(ControlsEnum.INVOICEHDR);
                                                        //    SetFieldValues(ControlsEnum.INVOICEHDR);
                                                        //    btnNew.Focus();
                                                        //    Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                                                        //    ViewState[ViewstateStrings.SelectedPosCount] = null;
                                                        //    GetFieldValues(ControlsEnum.UPLOADEDFILES);
                                                        //    SetFieldValues(ControlsEnum.UPLOADEDFILES);


                                                        //}
                                                        //else if (result.Value == (int)DbSaveStatus.AMOUNTEXCEEDS)
                                                        //{
                                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("AmountExceeds").ToString())
                                                        //    + "','" + Resources.ErpRes.Information + "');", true);
                                                        //}
                                                        #endregion
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                    }
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceNow").ToString()) + "','" + Resources.Messages.Information + "');", true);
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
                        if (SelectedPos != null)
                        {
                            SelectedPOList = SelectedPos;
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.POINVOICELIST);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            EntryStatus = EntryStatus.NEWMODE;
                            lblDispInvoiceNo.Text = "[NEW]";
                            ModifiedDatePnl.Visible = false;
                        }
                        else
                        {
                            SelectedPOList = new List<long>();
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICEHDR);
                            SetFieldValues(ControlsEnum.INVOICEHDR);
                            EntryStatus = EntryStatus.LISTMODE;

                        }

                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
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
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////
                                HiddenField hdfStatus = (HiddenField)grdrow.FindControl("hdfStatus");
                                ItemStatus = Convert.ToInt16(hdfStatus.Value);

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(1);
                            Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                            finInvoiceVndHdrObj.IVH_PK = CurrPK;
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                            GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFocus(txtInvdate);
                            SetFocus(lblDispInvoiceNo);
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region Invoice List
                    case ActionsEnum.INVOICELIST:
                        FillProcessID(1);
                        CurrPK = 0;
                        hdfIVHPK.Value = "";
                        SelectedPOList = new List<long>();
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        EntryStatus = EntryStatus.LISTMODE;
                        SelectedPos = null;
                        Response.Redirect(Resources.PageURL.PoInvoicing);
                        break;
                    #endregion
                    #region Invoice Details
                    case ActionsEnum.INVOICEDETAIL:
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
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(1);
                            Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                            finInvoiceVndHdrObj.IVH_PK = CurrPK;
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                            GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFocus(lblDispInvoiceNo);
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
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(1);
                            SetUIEditView(commonActions);
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                            finInvoiceVndHdrObj.IVH_PK = CurrPK;
                            hdfIVHPK.Value = CurrPK.ToString();
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.INVOICEHDR);
                            GetUIValuesFromObject(ControlsEnum.POINVOICEDETAILS);
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                            EntryStatus = EntryStatus.VIEWMODE;
                            GetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFieldValues(ControlsEnum.UPLOADEDFILES);
                            SetFocus(lblDispInvoiceNo);
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
                        int POPk = int.Parse(((Button)sender).CommandArgument.ToString());

                        HiddenField hdfPONumber = null;
                        TextBox txtPay = null;
                        foreach (GridViewRow grdRow in grdPOList.Rows)
                        {
                            hdfPONumber = grdRow.FindControl("hdfPONumber") as HiddenField;
                            txtPay = grdRow.FindControl("txtPayNow") as TextBox;
                            if (hdfPONumber != null && txtPay != null && !string.IsNullOrEmpty(hdfPONumber.Value) && !string.IsNullOrEmpty(txtPay.Text))
                            {
                                decimal d = 0;
                                decimal.TryParse(txtPay.Text, out d);
                                if (!dicTempAmount.ContainsKey(hdfPONumber.Value)
                                    && hdfPONumber.Value != POPk.ToString())
                                {
                                    dicTempAmount.Add(hdfPONumber.Value, d);
                                }
                            }
                        }


                        if (CurrPK == 0)
                        {

                            SelectedPos.Remove(POPk);
                            SelectedPOList = SelectedPos;
                            PurchaseOrderHeaderList = (List<PurchaseOrderHeader>)POList;
                            PurchaseOrderHeaderObj = PurchaseOrderHeaderList.SingleOrDefault(po => po.PurchaseOrderPK == POPk);
                            PurchaseOrderHeaderList.Remove(PurchaseOrderHeaderObj);
                            POList = PurchaseOrderHeaderList;
                            SetFieldValues(ControlsEnum.POINVOICELIST);
                        }
                        else
                        {
                            InvoiceVendorMappingObj = new InvoiceVendorMapping();// CommonFunctions.Initilize<InvoiceVendorMapping>();
                            InvoiceVendorMappingList = (List<InvoiceVendorMapping>)InvoiceMappingList;
                            InvoiceVendorMappingObj = InvoiceVendorMappingList.SingleOrDefault(po => po.PurchaseOrder.PurchaseOrderPK == POPk);
                            InvoiceVendorMappingList.Remove(InvoiceVendorMappingObj);
                            InvoiceMappingList = InvoiceVendorMappingList;
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
                    #region Pick for Paying
                    case ActionsEnum.PICKFORPAYMENT:
                        SetUIValuesToObject(ControlsEnum.PICKFORPAYMENT);
                        //Edit 28-08_2014
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowSelectedRow", "$(document).ready(function(){SetSelectedRowColor();});", true);
                        //End
                        break;
                    #endregion
                    #region Pick Inv & for Cr/Dr. Note
                    case ActionsEnum.PICKFORCRDRNOTE:
                        Session[ERP.Utilities.SessionStrings.CrDrType] = null;
                        SetUIValuesToObject(ControlsEnum.PICKFORCRDRNOTE);
                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                        finInvoiceVndHdrObj.IVH_PK = CurrPK;
                        hdfIVHPK.Value = CurrPK.ToString();
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
                        ((Button)sender).CommandName = ActionsEnum.SAVE.ToString();
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
                                poInvoiceServiceClient = new POInvoiceService();
                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                result = poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                poInvoiceServiceClient = new POInvoiceService();
                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                result = poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
                            }
                        }
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();

                        #region Inbox or Listing Page Redirection
                        //if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && ReferanceID > 0)
                        //{
                        //    Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.InboxURL));
                        //}
                        //else
                        //{
                        EntryStatus = EntryStatus.LISTMODE;
                        Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                        GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                        GetFieldValues(ControlsEnum.INVOICEHDR);
                        SetFieldValues(ControlsEnum.INVOICEHDR);
                        //}
                        #endregion
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        if (Session[ERP.Utilities.SessionStrings.Transaction] != null)
                        {
                            string Transaction = Session[ERP.Utilities.SessionStrings.Transaction].ToString();
                            if (Transaction == "SAVE")
                            {
                                poInvoiceServiceClient = new POInvoiceService();
                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                result = poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), true);
                            }
                            else if (Transaction == "DELETE")
                            {
                                poInvoiceServiceClient = new POInvoiceService();
                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                                result = poInvoiceServiceClient.UpdateInvoiceHdrJounalizeFlag(int.Parse(Session[ERP.Utilities.SessionStrings.TransactionPK].ToString()), false);
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
                    #region Tabs
                    case ActionsEnum.INVOICE:
                        CheckUserRightsAndRedirect(Resources.PageURL.PoInvoicing);
                        //Response.Redirect(Resources.PageURL.PoInvoicing);
                        break;
                    case ActionsEnum.EXPENSES:
                        CheckUserRightsAndRedirect(Resources.PageURL.ExpenseInvoice);
                        //Response.Redirect(Resources.PageURL.ExpenseInvoice);
                        break;
                    case ActionsEnum.PAYMENT:
                        CheckUserRightsAndRedirect(Resources.PageURL.PoPayment);
                        //Response.Redirect(Resources.PageURL.PoPayment);
                        break;
                    case ActionsEnum.DEFAULT:
                        CheckUserRightsAndRedirect(Resources.PageURL.PoListing);
                        //Response.Redirect(Resources.PageURL.PoListing);
                        break;
                    case ActionsEnum.CRDRNOTE:
                        Session[ERP.Utilities.SessionStrings.CrDrType] = ApplicationType.PI;
                        CheckUserRightsAndRedirect(Resources.PageURL.DrCrNote);
                        //Response.Redirect(Resources.PageURL.DrCrNote);
                        break;
                    case ActionsEnum.ACPAYABLES:
                        foreach (GridViewRow grdrow in grdPOInvoiceList.Rows)
                        {
                            rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                            if (rbtn.Checked)
                            {
                                Session[ERP.Utilities.SessionStrings.VendorPK] = ((HiddenField)grdrow.FindControl("hdfVendorPK")).Value;
                                Session[ERP.Utilities.SessionStrings.Vendor] = ((Label)grdrow.FindControl("lblVendor")).ToolTip;//For Showing name in vendorddl of AccountPayable Page Completely 
                                break;
                            }
                        }
                        CheckUserRightsAndRedirect(Resources.PageURL.AccountsPayable);
                        //Response.Redirect(Resources.PageURL.AccountsPayable);
                        break;
                    case ActionsEnum.POINVOICE:
                        string serviceUrl = GetGlobalResourceObject("ConfigurationsRes", "CheckURL2").ToString() == "1" ? Resources.PageURL.PurchaseOrderInvoicingService : string.Empty;
                        CheckUserRightsAndRedirect(Resources.PageURL.PurchaseOrderInvoicing, serviceUrl);
                        //Response.Redirect(Page.ResolveClientUrl(Resources.PageURL.PurchaseOrderInvoicing), false);
                        break;
                    #endregion
                    #region Delete
                    case ActionsEnum.DELETE:
                        if (decimal.Parse(hdfInvPaidAmt.Value) == 0)
                        {
                            poInvoiceServiceClient = new POInvoiceService();
                            poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);
                            result = poInvoiceServiceClient.DeleteInvoice(CurrPK);
                            if (result.HasValue && result.Value > 0)
                            {
                                #region LOG SAVE
                                CommonServiceClient = new CommonService();
                                CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);

                                ADM_APP_TRX_LOG AdmTrxLogDet = new ADM_APP_TRX_LOG();
                                List<ADM_APP_TRX_LOG> AdmTrxLogList = new List<ADM_APP_TRX_LOG>();
                                AdmTrxLogDet.ATL_ACTION = (byte)LogAction.DELETE;
                                AdmTrxLogDet.ATL_APP_TRX_CODE = (string.IsNullOrEmpty(lblDispInvoiceNo.Text) || lblDispInvoiceNo.Text.Trim().Equals("[NEW]")) ? string.Empty
                                                                    : lblDispInvoiceNo.Text.Trim(); ;
                                AdmTrxLogDet.ATL_APP_TYPE = ApplicationType.PI;
                                if (hdfType.Value == "2")
                                {
                                    AdmTrxLogDet.ATL_APP_SUB_TYPE = 13;
                                }
                                else if (hdfType.Value == "1")
                                {
                                    AdmTrxLogDet.ATL_APP_SUB_TYPE = 12;
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
                            litErrorMsg.Text = GetLocalResourceObject("CannotdeleteAlreadyasigned").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        if (Convert.ToInt32(hdfPayByDtContinue.Value) == 0)
                        {
                            if (DateTime.Parse(txtPaybydate.Text) < DateTime.Parse(txtInvdate.Text))
                            {
                                //string ErrMsg = string.Format(GetLocalResourceObject("PayByDateConfirmation").ToString());
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "PayByDateConfirm('" + ErrMsg + "');", true);
                                //return 0;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "PayByDateConfirm('" + (sender as Button).ID + "');", true);
                                return ;
                            }
                        }
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
                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComa(TotalAmount);
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
                        if (grdPOList.Rows.Count > 0)
                        {
                            NetAmount = decimal.Parse(txtInvoiceAmt.Text);
                            TotalAmount = decimal.Parse(hdfTotalPayNowFooter.Value);
                            lblTotalPayNowFooter = (Label)grdPOList.FooterRow.FindControl("lblTotalPayNowFooter");
                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComa(TotalAmount);
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
                            string invNumber = string.Empty;
                            long AdvResult = 0;
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

                                        if (Convert.ToBoolean(GetLocalResourceObject("OtherChargeVal")) && OtherChrg > InvNow)
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
                                            //lblTotalPayNowFooter.Text = hdfTotalPayNowFooter.Value;
                                            lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComa(TotalAmount);
                                            if (NetAmount == TotalAmount)
                                            {
                                                //finInvoiceVndHdrList = new List<FIN_INVOICE_VND_HDR>();
                                                InvoiceHeaderList = new List<InvoiceHdr>();

                                                poInvoiceServiceClient = new POInvoiceService();
                                                poInvoiceServiceClient = CommonFunctions.InitiateClient(poInvoiceServiceClient);

                                                //finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                                                InvoiceHeaderObj = new InvoiceHdr();
                                                //finInvoiceVndHdrObj = (FIN_INVOICE_VND_HDR)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);
                                                InvoiceHeaderObj = (InvoiceHdr)SetUIValuesToObject(ControlsEnum.WRKFSUBMIT);

                                                if (hdfExchangeRate.Value != "-1")
                                                {
                                                    if (InvoiceHeaderObj != null)
                                                    {
                                                        //Checking: allow to save vendor invoice no duplication or not
                                                        if (hdfIsContDupVenInvNo.Value != "1")
                                                        {
                                                            //Check For Vendor Invoice Exist
                                                            if (!string.IsNullOrEmpty(InvoiceHeaderObj.VendorInvoiceNo))
                                                                if (poInvoiceServiceClient.IsExistVendorInvoice(InvoiceHeaderObj.VendorInvoiceNo, InvoiceHeaderObj.IVH_VENDOR, InvoiceHeaderObj.IVH_PK))
                                                                {
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                          "ClosePopup();", true);
                                                                    //litErrorMsg.Text = GetLocalResourceObject("Msg_VendorInvoiceExist").ToString();
                                                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "VendorInvoiceDup", "$(document).ready(function(){ShowDuplicateVendorInvNoContinue(2);});", true);
                                                                    return;
                                                                }
                                                        }
                                                        TypeRef = InvoiceHeaderObj.IVH_NO;
                                                        if (InvoiceHeaderObj.InvoiceVendorMappingList.ToList().Count > 0)
                                                        {
                                                            InvoiceHeaderList.Add(InvoiceHeaderObj);
                                                            int Archiveresult = 0;
                                                            if (InvoiceHeaderObj.IVH_PK > 0 && InvoiceHeaderObj.IVH_STATUS > 0)
                                                            {
                                                                //For each update keep version details of voucher for Audit trail 
                                                                Archiveresult = BusinessLogic.POInvoicing.POInvoiceBL.SaveInvoiceArchiveDetails(InvoiceHeaderObj.IVH_PK);
                                                            }
                                                            if ((Archiveresult > 0 && InvoiceHeaderObj.IVH_STATUS > 0) || InvoiceHeaderObj.IVH_STATUS == 0)
                                                            {
                                                                //result = poInvoiceServiceClient.SaveInvoiceHdr(finInvoiceVndHdrList, true, IsAdvInvHasTax);
                                                                InvoiceHdrBO invBo = new InvoiceHdrBO();
                                                                invBo.InvoiceHdrList = InvoiceHeaderList;
                                                                invBo.WKF_FLAG = 1;
                                                                SaveTransaction(invBo, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));

                                                                #region Commented - Moved to SaveTransaction
                                                                //#region New workflow Submition
                                                                //wkfDetails = ucrWrkf.GetWorkflowDetails();
                                                                //invBo.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
                                                                //invBo.WKF_APPLICATION = CurrPK;
                                                                //invBo.WKF_COMMENTS = wkfDetails.Comments;
                                                                //invBo.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT);
                                                                //invBo.WKF_PROCESS = wkfDetails.ProcessID;
                                                                //invBo.WKF_REFERENCE = wkfDetails.ReferenceID;
                                                                //invBo.WKF_TASK = wkfDetails.TaskID;
                                                                //invBo.WKF_TASK_ACTION = wkfDetails.ActionID;
                                                                //action = wkfDetails.ActionText;
                                                                //invBo.APT_CODE = ApplicationType.PI;
                                                                //#endregion

                                                                //string xmlDoc = CommonFunctions.XmlSerialize<InvoiceHdrBO>(invBo);
                                                                ////string invNumber = string.Empty;
                                                                //result = BusinessLogic.POInvoicing.POInvoiceBL.SaveAdvanceInvoiceWkf(xmlDoc, out invNumber);
                                                                //AdvResult = Convert.ToInt64(result);

                                                                //if (result.HasValue && result.Value > 0)// Save Success ! do WorkFlow
                                                                //{
                                                                //    #region ALERTSAVE
                                                                //    GetFieldValues(ControlsEnum.ALERTCONFIG);
                                                                //    int isAlert = 0;
                                                                //    if (admAppConstMstList != null && admAppConstMstList.Count > 0)
                                                                //    {
                                                                //        isAlert = admAppConstMstList[0].ACF_VALUE;
                                                                //    }
                                                                //    if (isAlert == 1)
                                                                //    {
                                                                //        invPK = (int)result;
                                                                //        AlertBO alertBoObj = new AlertBO();
                                                                //        alertBoObj = (AlertBO)SetUIValuesToObject(ControlsEnum.ALERTSAVE);
                                                                //        if (alertBoObj != null)
                                                                //        {
                                                                //            alertresult = BusinessLogic.AlertManagement.Alerts.SaveAlertDetails(alertBoObj);
                                                                //        }
                                                                //    }
                                                                //    #endregion
                                                                //    //Workflow submission
                                                                //    ucrWrkf.ApplicationID = (int)result.Value;
                                                                //#region ATTACHMENT SAVE
                                                                //if (DocAttachList != null && DocAttachList.Count > 0)
                                                                //{
                                                                //    savePath = string.Empty;
                                                                //    if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower()))
                                                                //    {
                                                                //        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload";
                                                                //        if (!Directory.Exists(savePath))
                                                                //            Directory.CreateDirectory(savePath);
                                                                //        savePath = HttpContext.Current.Request.PhysicalApplicationPath + "Upload\\";
                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        savePath = System.Configuration.ConfigurationManager.AppSettings["UploadDirectory"].ToLower();//Server.MapPath("../Upload");
                                                                //    }

                                                                //    foreach (ADM_DOC_ATTACH obj in DocAttachList)
                                                                //    {
                                                                //        string[] docName = obj.DOC_PATH.Split('/');
                                                                //        string filePath = savePath + obj.DOC_NAME;
                                                                //        if (docName.Length > 0)
                                                                //            filePath = savePath + docName[docName.Length - 1];
                                                                //        FileInfo attachedFileInfo = new FileInfo(filePath);
                                                                //        if (FileDetailsList != null)
                                                                //        {
                                                                //            FileDetails fileDetailsObj = FileDetailsList.SingleOrDefault(aa => aa.SlNo == obj.DOC_SEQ_NO);
                                                                //            if (fileDetailsObj != null)
                                                                //            {
                                                                //                fileDetailsObj.PoFile.SaveAs(attachedFileInfo.FullName);

                                                                //            }
                                                                //        }
                                                                //    }
                                                                //    docSaveResult = poInvoiceServiceClient.SaveDocAttachemts(DocAttachList, (int)result);
                                                                //}
                                                                //#endregion

                                                                //    #region WkfSummarySave
                                                                //    //int resultSummary = BusinessLogic.CommonManagement.CommonBL.SaveSummary(result.Value, Convert.ToInt32(hdfProcessID.Value));
                                                                //    //if (resultSummary <= 0)
                                                                //    //{

                                                                //    //    litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                                                                //    //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                                                                //    //}
                                                                //    #endregion
                                                                //}
                                                                //else if (result.HasValue)
                                                                //{
                                                                //    if (result.Value == (int)DbSaveStatus.SQLERROR)
                                                                //    {
                                                                //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                                //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                //            + "','" + Resources.ErpRes.Information + "');", true);
                                                                //    }
                                                                //    else if (result.Value == (int)DbSaveStatus.CONCURRENCY)
                                                                //    {
                                                                //        litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.EditUsedByAnotherUser;
                                                                //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                //        + "','" + Resources.ErpRes.Information + "');", true);
                                                                //    }
                                                                //    else if (result.Value == (int)DbSaveStatus.CODEEXIST)
                                                                //    {
                                                                //        litErrorMsg.Text = Resources.PageNameRes.POInvoice + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                                //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                //        + "','" + Resources.ErpRes.Information + "');", true);
                                                                //    }
                                                                //    else if (result.Value == (int)DbSaveStatus.AMOUNTEXCEEDS)
                                                                //    {
                                                                //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("AmountExceeds").ToString())
                                                                //        + "','" + Resources.ErpRes.Information + "');", true);
                                                                //    }
                                                                //    else
                                                                //    {
                                                                //        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                                //        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.RFQ);
                                                                //        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                //            + "','" + Resources.ErpRes.Information + "');", true);
                                                                //    }
                                                                //}
                                                                #endregion
                                                            }
                                                            else
                                                            {
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save").ToString()) + "','" + Resources.ErpRes.Information + "');", true);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceNow").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                    "ClosePopup();", true);

                                                    litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
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
                            //|| (Request.QueryString[QueryStrings.PageType] != null &&
                            //Request.QueryString[QueryStrings.PageType] == SCWorkFlowType.Cancel))
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation(CurrPK, ApplicationType.PI))
                                {
                                    ucrWrkf.ApplicationID = CurrPK;
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
                                    SelectedPos = null;
                                    InvoiceMapList = null;
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                                    GetFieldValues(ControlsEnum.INVOICEHDR);
                                    SetFieldValues(ControlsEnum.INVOICEHDR);
                                    btnNew.Focus();
                                    Session[ERP.Utilities.SessionStrings.SelectedPos] = null;
                                    ViewState[ViewstateStrings.SelectedPosCount] = null;
                                }
                            }
                            else
                            {
                                ucrWrkf.ApplicationID = CurrPK;
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                            }
                        }
                        break;
                    #endregion
                    #region Reset Selection
                    case ActionsEnum.RESET:
                        SelectedCurrency = new List<long>();
                        Currency = 0;
                        SelectedVendors = new List<long>();
                        VendorID = 0;
                        SelectedInvoiceList = new List<long>();
                        SelectedINVTax = null;
                        SelectedINVTaxList = new List<decimal>();
                        SelectedPos = new List<long>();
                        SelectedCurrencyList = new List<long>();
                        SelectedVendorsList = new List<long>();
                        SelectedPOList = new List<long>();
                        SelectedInvoiceCrDrList = new List<long>();
                        Session[ERP.Utilities.SessionStrings.SelectedInvoices] = null;
                        Session[ERP.Utilities.SessionStrings.SelectedInvoicesCrDr] = null;
                        btnPickForPayment.Text = GetLocalResourceObject("PickPoForPayment").ToString();
                        btnPickForCrDrNote.Text = GetLocalResourceObject("PickForCrDrNote").ToString();

                        //Resetting Color
                        hdfSelectedItemPk.Value = "0";

                        break;
                    #endregion
                    #region Alert
                    case ActionsEnum.ALERT:
                        ucrAlert.TypeCode = POGroup == POInvoiceGroup.Goods ? ApplicationType.PI
                            : POGroup == POInvoiceGroup.Services ? ApplicationType.PSI : ApplicationType.EI;
                        ucrAlert.TypePK = CurrPK;
                        ucrAlert.TypeRef = lblDispInvoiceNo.Text.Trim();
                        ucrAlert.TrxDate = string.IsNullOrEmpty(txtInvdate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtInvdate.Text.Trim());
                        ucrAlert.TypeText = GetLocalResourceObject("Alert_Type_Text").ToString();
                        hdfIVHPK.Value = CurrPK.ToString();
                        GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                        if (finInvoiceVndHdrList != null && finInvoiceVndHdrList.Count == 1)
                        {
                            ucrAlert.TypePartyName = finInvoiceVndHdrList[0].PUR_VENDOR_MST.VEN_NAME;
                        }
                        ucrAlert.GetAlertList();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop2", "ShowContainerDiv('[id$=divAlert]','Alert','800','400');", true);
                        break;
                    #endregion
                    #region EDITFORCANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        ddlVendorBranch.Items.Clear();
                        ddlVendorBranch.DataSource = null;
                        ddlVendorBranch.DataBind();
                        hdfIsCancel.Value = "1";
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
                                ////start
                                Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);
                                //Posted = Convert.ToBoolean(((HiddenField)grdrow.FindControl("hdfPosted")).Value);
                                HiddenField hdfPosted = (HiddenField)grdrow.FindControl("hdfPosted");
                                if (hdfPosted != null)
                                    Posted = Convert.ToBoolean(hdfPosted.Value);
                                ////

                                break;
                            }
                        }
                        if (bIsChecked)
                        {
                            FillProcessID(11);
                            Session[ERP.Utilities.SessionStrings.JOURNALIZETAB_SELECTED_PK] = null;
                            SetUIEditView(commonActions);
                            ModifiedDatePnl.Visible = true;
                            GetFieldValues(ControlsEnum.TAXCALCULATIONSETTINGS);
                            GetFieldValues(ControlsEnum.POINVOICEDETAILS);
                            finInvoiceVndHdrObj = CommonFunctions.Initilize<FIN_INVOICE_VND_HDR>();
                            finInvoiceVndHdrObj.IVH_PK = CurrPK;
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

                    #region PRINT PO
                    case ActionsEnum.PRINTPO:
                        GridViewRow grwPoLst = (GridViewRow)((LinkButton)(sender)).Parent.Parent;
                        HiddenField hdfPoPk = grwPoLst.FindControl("hdfPoPk") as HiddenField;
                        HiddenField hdfInvType = grwPoLst.FindControl("hdfInvType") as HiddenField;
                        if(hdfInvType.Value.ToString() == "6")
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfPoPk.Value + "&APPTYPE=" + ApplicationType.SCWO + "&APPSUBTYPE=1") + "');", true);

                        else
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + hdfPoPk.Value + "&APPTYPE=" + ApplicationType.PO + "&APPSUBTYPE=") + "');", true);
                        break;
                    #endregion

                    #region ADDITEM
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
                                // litErrorMsg.Text = Resources.ErrorMessages.Msg_Valid_File;
                                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text
                                //    + "','" + Resources.ErpRes.Information + "');", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(Resources.ErrorMessages.Msg_Valid_File) + "','" + Resources.ErpRes.Information + "');", true);

                            }
                            else
                            {
                                if (CurrSlNo != 0)
                                {
                                    if (fupUpload.HasFile || !string.IsNullOrEmpty(anchorFile.HRef))
                                    {
                                        admDocAttachObj = DocAttachList.SingleOrDefault(itm => itm.DOC_SEQ_NO == CurrSlNo);
                                        if (admDocAttachObj != null)
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
                                                //admDocAttachObj.AttachmentFileName = attachmentFileName;
                                                //admDocAttachObj.FileExtension = tempFileInfoObj.Extension;
                                                admDocAttachObj.DOC_NAME = fupUpload.FileName;
                                                admDocAttachObj.DOC_TYPE = tempFileInfoObj.Extension;
                                                admDocAttachObj.DOC_CRTD_DT = DateTime.Now;
                                                admDocAttachObj.DOC_CRTD_BY = currentUser.PKUser;
                                                admDocAttachObj.DOC_MOD_DT = DateTime.Now;
                                                admDocAttachObj.DOC_MOD_BY = currentUser.PKUser;
                                                admDocAttachObj.DOC_BIZUNIT = currentUser.SBUID;
                                                admDocAttachObj.DOC_MODULE = (int)DocModuleEnum.PURCHASE;
                                                admDocAttachObj.DOC_TASK = (int)DocTaskEnum.PURCHASEINVOICE;
                                                if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                                {
                                                    admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                                }
                                                else
                                                {
                                                    admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
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
                                        if (DocAttachList == null || DocAttachList.Count == 0)
                                        {
                                            DocAttachList = new List<ADM_DOC_ATTACH>();
                                            slno = 1;
                                        }
                                        else
                                        {
                                            slno = DocAttachList.Max(itm => itm.DOC_SEQ_NO);
                                            slno++;
                                        }
                                        if (FileDetailsList == null)
                                        {
                                            FileDetailsList = new List<FileDetails>();
                                        }

                                        admDocAttachObj = new ADM_DOC_ATTACH();
                                        admDocAttachObj.DOC_PK = 0;
                                        admDocAttachObj.DOC_SEQ_NO = (short)slno;
                                        tempFileInfoObj = new FileInfo(fupUpload.PostedFile.FileName);
                                        string attachmentFileFormat = tempFileInfoObj.Extension;
                                        string attachmentFileName = Guid.NewGuid().ToString() + attachmentFileFormat;

                                        //admDocAttachObj.AttachmentFileName = attachmentFileName;
                                        //admDocAttachObj.FileExtension = tempFileInfoObj.Extension;
                                        admDocAttachObj.DOC_NAME = fupUpload.FileName;
                                        admDocAttachObj.DOC_TYPE = tempFileInfoObj.Extension;
                                        admDocAttachObj.DOC_CRTD_DT = DateTime.Now;
                                        admDocAttachObj.DOC_CRTD_BY = currentUser.PKUser;
                                        admDocAttachObj.DOC_MOD_DT = DateTime.Now;
                                        admDocAttachObj.DOC_MOD_BY = currentUser.PKUser;
                                        admDocAttachObj.DOC_BIZUNIT = currentUser.SBUID;
                                        admDocAttachObj.DOC_MODULE = (int)DocModuleEnum.PURCHASE;
                                        admDocAttachObj.DOC_TASK = (int)DocTaskEnum.PURCHASEINVOICE;
                                        if (string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower()))
                                        {
                                            admDocAttachObj.DOC_PATH = "~/Upload/" + attachmentFileName;
                                        }
                                        else
                                        {
                                            admDocAttachObj.DOC_PATH = System.Configuration.ConfigurationManager.AppSettings["UploadPath"].ToLower() + attachmentFileName;
                                        }

                                        // admDocAttachObj.SCD_FILE_PATH = "~/Upload/" + attachmentFileName;
                                        admDocAttachObj.DOC_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                        FileDetailsList.Add(new FileDetails() { SlNo = slno, PoFile = HttpContext.Current.Request.Files[0] });
                                        DocAttachList.Add(admDocAttachObj);

                                    }
                                }

                                SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                                //SetFieldValues(ControlsEnum.POINVOICELIST);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ScrollDown();", true);
                            }
                        }
                        break;
                    #endregion

                    #region REMOVEITEM
                    case ActionsEnum.REMOVEITEM:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                DocAttachList = DocAttachList.Where(row => selectedItemPK != row.DOC_SEQ_NO).ToList();
                                //SetFieldValues(ControlsEnum.UPLOADEDFILES);
                                BindGrid(ControlsEnum.UPLOADEDFILES);
                                ResetForm(ControlsEnum.ADDITEM);
                            }
                        }
                        break;
                    #endregion

                    #region EDITITEM
                    case ActionsEnum.EDITITEM:
                        if (DocAttachList != null && DocAttachList.Count > 0)
                        {
                            selectedItemPK = Convert.ToInt32(grdUploads.DataKeys[(((Button)sender).Parent.Parent as GridViewRow).RowIndex][0]);
                            if (selectedItemPK > 0)
                            {
                                admDocAttachObj = DocAttachList.SingleOrDefault(row => selectedItemPK == row.DOC_SEQ_NO);
                                GetUIValuesFromObject(ControlsEnum.SELECTEDDOC);
                            }
                        }
                        break;
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
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=13") + "');", true);
                            else
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=12") + "');", true);
                            //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.PI ) + "');", true);
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
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=13") + "');", true);
                                    else
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" + ((HiddenField)grdrow.FindControl("hdfInvoiceID")).Value + "&APPTYPE=" + ApplicationType.PI + "&APPSUBTYPE=12") + "');", true);
                                    CurrPK = 0;
                                    return;
                                }
                            }
                            //litErrorMsg.Text = GetLocalResourceObject("Msg_Select_Invoice").ToString();
                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        break;
                    #endregion

                    #region TAXSPLITUPPOPUP
                    case ActionsEnum.TAXDETAILSSPLITUP:
                        int POH_Pk = int.Parse(((LinkButton)sender).CommandArgument.ToString());
                        SelectedTaxPOPK = POH_Pk;
                        GetFieldValues(ControlsEnum.POTAXDETAILS);
                        SetFieldValues(ControlsEnum.POTAXDETAILS);
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
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(CommonFunctions.ProcessException(ex)) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                poListServiceClient = null;
                poInvoiceServiceClient = null;
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
                if (((GridView)sender).ID == "grdPOInvoiceList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        HiddenField hdfBalamt = e.Row.FindControl("hdfBalamt") as HiddenField;
                        LinkButton lnkBalAmt = e.Row.FindControl("lnkBalAmt") as LinkButton;
                        Label lblInvoiceValue = e.Row.FindControl("lblInvoiceValue") as Label;

                        if (InvoiceHeaderList != null && InvoiceHeaderList.Count > 0)
                        {

                            decimal balAmount = 0;
                            balAmount = Convert.ToDecimal(InvoiceHeaderList[e.Row.RowIndex].IVH_AMOUNT_NET_TC) - Convert.ToDecimal(InvoiceHeaderList[e.Row.RowIndex].PaidAmountTC);
                            lnkBalAmt.Text = String.Format("{0:c}", balAmount);
                            lnkBalAmt.ToolTip = String.Format("{0:c}", balAmount);
                            decimal InvAmount = Convert.ToDecimal(lblInvoiceValue.Text);
                            if (InvAmount > balAmount)
                            {
                                lnkBalAmt.Attributes.Add("onclick", "return true;");
                            }
                            else
                            {
                                lnkBalAmt.Attributes.Add("onclick", "return false;");
                                lnkBalAmt.CssClass = "removelinkPopup";
                            }
                        }
                    }
                }
                if (((GridView)sender).ID == "grdPOList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        string POdate = "";
                        //Label lblPONo = e.Row.FindControl("lblPONo") as Label;
                        LinkButton lbtnPONo = e.Row.FindControl("lbtnPONo") as LinkButton;
                        Label lblPODate = e.Row.FindControl("lblPODate") as Label;
                        Label lblVendorInv = e.Row.FindControl("lblVendorInv") as Label;
                        Label lblCurrency = e.Row.FindControl("lblCurrency") as Label;
                        Label lblGrossAmount = e.Row.FindControl("lblGrossAmount") as Label;
                        //Label lblTax = e.Row.FindControl("lblTax") as Label;
                        LinkButton lblTax = e.Row.FindControl("lblTax") as LinkButton;

                        Label lblPOTax = e.Row.FindControl("lblPOTax") as Label;
                        Label lblOtherAmount = e.Row.FindControl("lblOtherAmount") as Label;
                        Label lblDiscount = e.Row.FindControl("lblDiscount") as Label;
                        Label lblTotalAmount = e.Row.FindControl("lblTotalAmount") as Label;
                        Label lblInvoiced = e.Row.FindControl("lblInvoiced") as Label;
                        Label lblAdvInvoiced = e.Row.FindControl("lblAdvInvoiced") as Label;
                        Label lblBalancetoInvoice = e.Row.FindControl("lblBalancetoInvoice") as Label;
                        TextBox txtPayNow = e.Row.FindControl("txtPayNow") as TextBox;
                        TextBox txtOthercharges = e.Row.FindControl("txtOthercharges") as TextBox;
                        HiddenField hdfPONumber = e.Row.FindControl("hdfPONumber") as HiddenField;
                        HiddenField hdfPayNow = e.Row.FindControl("hdfPayNow") as HiddenField;
                        HiddenField hdfOtherchargeOLD = e.Row.FindControl("hdfOtherchargeOLD") as HiddenField;
                        HiddenField hdfPriceAdjustment = e.Row.FindControl("hdfPriceAdjustment") as HiddenField;
                        HiddenField hdfTaxPercentage = e.Row.FindControl("hdfTaxPercentage") as HiddenField;
                        HiddenField hdfPoPk = e.Row.FindControl("hdfPoPk") as HiddenField;
                        Button lnkRemove = e.Row.FindControl("lnkRemove") as Button;
                        //ImageButton imgTax = e.Row.FindControl("imgTax") as ImageButton;
                        HiddenField hdfOtherChargesPrev = e.Row.FindControl("hdfOtherChargesPrev") as HiddenField;


                        if (PurchaseOrderHeaderList != null && PurchaseOrderHeaderList.Count > 0)
                        {
                            lnkRemove.CommandArgument = PurchaseOrderHeaderList[e.Row.RowIndex].PurchaseOrderPK.ToString();
                            hdfPONumber.Value = PurchaseOrderHeaderList[e.Row.RowIndex].PurchaseOrderPK.ToString();
                            POdate = PurchaseOrderHeaderList[e.Row.RowIndex].PODate.ToString(Resources.Constants.DateFormatShort);

                            //lblPONo.Text = ERP.Utilities.CommonFunctions.GetShortString(PurchaseOrderHeaderList[e.Row.RowIndex].PONo, 15);
                            //lblPONo.ToolTip = PurchaseOrderHeaderList[e.Row.RowIndex].POH_NO + "  " + POdate;
                            lbtnPONo.Text = ERP.Utilities.CommonFunctions.GetShortString(PurchaseOrderHeaderList[e.Row.RowIndex].PONo, 15);
                            lbtnPONo.ToolTip = PurchaseOrderHeaderList[e.Row.RowIndex].PONo + "  " + POdate;
                            hdfPoPk.Value = PurchaseOrderHeaderList[e.Row.RowIndex].PurchaseOrderPK.ToString();

                            lblPODate.Text = PurchaseOrderHeaderList[e.Row.RowIndex].PODate.ToString(Resources.Constants.DateFormatShort);
                            lblPODate.ToolTip = PurchaseOrderHeaderList[e.Row.RowIndex].PODate.ToString(Resources.Constants.DateFormatShort);
                            lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(PurchaseOrderHeaderList[e.Row.RowIndex].Vendor.VEN_NAME, 5);
                            lblVendorInv.ToolTip = PurchaseOrderHeaderList[e.Row.RowIndex].Vendor.VEN_NAME;
                            lblCurrency.Text = PurchaseOrderHeaderList[e.Row.RowIndex].Currency.CUR_CODE;
                            lblCurrency.ToolTip = PurchaseOrderHeaderList[e.Row.RowIndex].Currency.CUR_CODE;

                            //GrossTotal 
                            //lblGrossAmount.Text = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].POH_SUB_TOTAL);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].POH_SUB_TOTAL);
                            decimal GrossAmount = 0;
                            //if (PurchaseOrderHeaderList[e.Row.RowIndex].PurchaseOrderDetailList != null)
                            //{
                            //    foreach (PurchaseOrdeDetails dtlObj in PurchaseOrderHeaderList[e.Row.RowIndex].PurchaseOrderDetailList)
                            //    {
                            //        //IN the case of Service Grossamount is from POD_AMT_VALUE
                            //        if (PurchaseOrderHeaderList[e.Row.RowIndex].GroupPK != 2)
                            //        {
                            //            GrossAmount = GrossAmount + dtlObj.Amount;
                            //        }
                            //        else
                            //        {
                            //            GrossAmount = GrossAmount + dtlObj.AmountValue;
                            //        }
                            //    }
                            //}

                            GrossAmount = PurchaseOrderHeaderList[e.Row.RowIndex].GrossAmount;

                            lblGrossAmount.Text = String.Format("{0:c}", GrossAmount);
                            lblGrossAmount.ToolTip = String.Format("{0:c}", GrossAmount);
                            //End GrossTotal


                            decimal tax = 0;
                            decimal disc = 0;
                            decimal.TryParse(Convert.ToString(PurchaseOrderHeaderList[e.Row.RowIndex].AdditionalTaxAmount), out tax);
                            decimal.TryParse(Convert.ToString(PurchaseOrderHeaderList[e.Row.RowIndex].DiscountAmount), out disc);
                            //if (PurchaseOrderHeaderList[e.Row.RowIndex].PurchaseOrderDetailList != null)
                            //{
                            //    foreach (PurchaseOrdeDetails dtlObj in PurchaseOrderHeaderList[e.Row.RowIndex].PurchaseOrderDetailList)
                            //    {

                            //        if (dtlObj.TaxDetailList != null)
                            //            tax += (decimal)dtlObj.TaxDetailList.Where(aa => aa.CategoryPK == 1).Sum(aa => aa.TaxAmount);
                            //        disc += (decimal)dtlObj.TaxDetailList.Where(aa => aa.CategoryPK == 3).Sum(aa => aa.TaxAmount);
                            //    }
                            //}

                            tax = PurchaseOrderHeaderList[e.Row.RowIndex].TaxAmount;
                            disc = PurchaseOrderHeaderList[e.Row.RowIndex].POHDiscountAmount;

                            lblTax.ToolTip = lblTax.Text = String.Format("{0:c}", tax);
                            lblDiscount.Text = lblDiscount.ToolTip = String.Format("{0:c}", disc);

                            // Setting Tax Details popup  
                            if (tax > 0)
                            {
                                lblTax.CommandArgument = PurchaseOrderHeaderList[e.Row.RowIndex].PurchaseOrderPK.ToString();
                            }
                            else
                            {
                                lblTax.CommandArgument = "0";
                            }


                            decimal otherAmount = 0;
                            decimal InvoicedAmount = 0;
                            decimal InvoicedOtherCharge = 0;
                            decimal InvoicedTax = 0;
                            decimal AdvInvoicedAmount = 0;
                            decimal AllocatedAdvAmount = 0;
                            decimal BalanceAmount = 0;
                            decimal.TryParse(Convert.ToString(PurchaseOrderHeaderList[e.Row.RowIndex].ShippingCharge), out otherAmount);

                            lblOtherAmount.Text = String.Format("{0:c}", otherAmount);
                            lblOtherAmount.ToolTip = String.Format("{0:c}", otherAmount);
                            lblTotalAmount.Text = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].TotalValue);
                            lblTotalAmount.ToolTip = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].TotalValue);

                            //InvoicedAmount = PurchaseOrderHeaderList[e.Row.RowIndex].FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0).Sum(c => c.IVM_AMOUNT);

                            //Invoiced amount getting -ve
                            //AdvInvoicedAmount = PurchaseOrderHeaderList[e.Row.RowIndex].FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT - c.IVM_OTHER_AMOUNT);

                            //AdvInvoicedAmount = PurchaseOrderHeaderList[e.Row.RowIndex].InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK == (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount);
                            //InvoicedAmount = PurchaseOrderHeaderList[e.Row.RowIndex].InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK != (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount - c.Discount + c.TaxAmount + c.OtherAmount);
                            AdvInvoicedAmount = PurchaseOrderHeaderList[e.Row.RowIndex].AdvanceInvoiceAmount;
                            InvoicedAmount = PurchaseOrderHeaderList[e.Row.RowIndex].InvoiceAmount;
                            //InvoicedAmount = InvoicedAmount + AdvInvoicedAmount;
                            AllocatedAdvAmount = PurchaseOrderHeaderList[e.Row.RowIndex].AllocatedAdvanceAmount;
                            BalanceAmount = PurchaseOrderHeaderList[e.Row.RowIndex].BalanceAmount;

                            if (PurchaseOrderHeaderList[e.Row.RowIndex].Type == (byte)PurchaseType.Import || !IsAdvInvHasTax)
                            {
                                //InvoicedAmount = PurchaseOrderHeaderList[e.Row.RowIndex].InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK != (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount - c.Discount + c.OtherAmount + c.TaxAmount);
                                //AdvInvoicedAmount = PurchaseOrderHeaderList[e.Row.RowIndex].InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK == (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount);
                                //AllocatedAdvAmount = PurchaseOrderHeaderList[e.Row.RowIndex].VendorAdvanceDeductionList.Where(r => r.VendorHeader.DeleteStatus == 0).Sum(alloc => alloc.Amount - alloc.TaxAmount - alloc.OtherAmount);
                                lblAdvInvoiced.Text = String.Format("{0:c}", AdvInvoicedAmount);
                                // InvoicedAmount = InvoicedAmount + AllocatedAdvAmount;
                                if (InvoicedAmount < 0)
                                    InvoicedAmount = 0;
                                lblInvoiced.Text = String.Format("{0:c}", InvoicedAmount);

                                //lblBalancetoInvoice.Text = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].POH_TOTAL_VALUE - PurchaseOrderHeaderList[e.Row.RowIndex].POH_AMT_INVOICED);
                                //if ((PurchaseOrderHeaderList[e.Row.RowIndex].TotalValue - InvoicedAmount) > (PurchaseOrderHeaderList[e.Row.RowIndex].TotalValue - AdvInvoicedAmount))
                                //{
                                //    BalanceAmount = PurchaseOrderHeaderList[e.Row.RowIndex].TotalValue - AdvInvoicedAmount;
                                //}
                                //else
                                //{
                                //    BalanceAmount = PurchaseOrderHeaderList[e.Row.RowIndex].TotalValue - InvoicedAmount;
                                //}
                                //lblBalancetoInvoice.Text = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].POH_TOTAL_VALUE - InvoicedAmount);
                                lblBalancetoInvoice.Text = String.Format("{0:c}", BalanceAmount);
                                lblBalancetoInvoice.Text = Convert.ToDecimal(lblBalancetoInvoice.Text.Replace(",", "")) < 0 ? String.Format("{0:c}", 0) : lblBalancetoInvoice.Text;

                                txtPayNow.Text = String.Format("{0:c}", BalanceAmount);

                            }
                            else
                            {
                                grdPOList.Columns[10].Visible = false;
                                //AllocatedAdvAmount = PurchaseOrderHeaderList[e.Row.RowIndex].VendorAdvanceDeductionList.Where(r => r.VendorHeader.DeleteStatus == 0).Sum(c => c.Amount - c.OtherAmount - c.TaxAmount);
                                InvoicedAmount -= AllocatedAdvAmount;
                                lblInvoiced.Text = String.Format("{0:c}", InvoicedAmount);
                                //lblBalancetoInvoice.Text = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].POH_TOTAL_VALUE - PurchaseOrderHeaderList[e.Row.RowIndex].POH_AMT_INVOICED);
                                lblBalancetoInvoice.Text = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].TotalValue - InvoicedAmount);
                                lblBalancetoInvoice.Text = Convert.ToDecimal(lblBalancetoInvoice.Text.Replace(",", "")) < 0 ? String.Format("{0:c}", 0) : lblBalancetoInvoice.Text;

                                if (dicTempAmount != null && dicTempAmount.Count > 0 && dicTempAmount.ContainsKey(hdfPONumber.Value))
                                {
                                    txtPayNow.Text = dicTempAmount.SingleOrDefault(aa => aa.Key == hdfPONumber.Value).Value.ToString();
                                }
                                else
                                {
                                    //txtPayNow.Text = GetFormattedCurrency(lblBalancetoInvoice.Text.Replace(",", ""));
                                    txtPayNow.Text = String.Format("{0:c}", Convert.ToDecimal(PurchaseOrderHeaderList[e.Row.RowIndex].TotalValue) - Convert.ToDecimal(lblInvoiced.Text));
                                }
                            }

                            hdfPriceAdjustment.Value = PurchaseOrderHeaderList[e.Row.RowIndex].PriceAdjust.ToString();
                            if (PurchaseOrderHeaderList[e.Row.RowIndex].TaxHeaderList != null && PurchaseOrderHeaderList[e.Row.RowIndex].TaxHeaderList.Count > 0)
                                hdfTaxPercentage.Value = PurchaseOrderHeaderList[e.Row.RowIndex].TaxHeaderList[0].Taxpercentage.ToString();
                            else
                                hdfTaxPercentage.Value = "0.00";


                            hdfPayNow.Value = txtPayNow.Text;
                            //***********Deducting Advance Othercharge with each Invoice othercharge***************
                            //hdfOtherchargeOLD.Value = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].FIN_INVOICE_VND_TRX_MPG.Where(tm => tm.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(tm => tm.IVM_OTHER_AMOUNT));

                            //hdfOtherchargeOLD.Value = String.Format("{0:c}", PurchaseOrderHeaderList[e.Row.RowIndex].InvoiceVendorMappingList.Where(tm => tm.VendorHeader.DeleteStatus == 0).Sum(tm => tm.OtherAmount - tm.PUR_ORDER_HDR.FIN_INVOICE_VND_ADV_DED_DTL.Where(d => d.VAD_INVOICE_HDR == tm.IVM_INVOICE_HDR).Sum(amt => amt.VAD_OTHER_AMOUNT)));
                            hdfOtherchargeOLD.Value = PurchaseOrderHeaderList[e.Row.RowIndex].OtherChargesAmount.ToString();

                            decimal otherCharges = Convert.ToDecimal(PurchaseOrderHeaderList[e.Row.RowIndex].ShippingCharge) - Convert.ToDecimal(hdfOtherchargeOLD.Value);
                            txtOthercharges.Text = GetFormattedCurrency(Math.Round(otherCharges, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            txtPayNow.Text = GetFormattedCurrency(Math.Round(decimal.Parse(txtPayNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                            txtInvdate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                            decimal prevotheramnt = 0;
                            if (PurchaseOrderHeaderList[e.Row.RowIndex].InvoiceVendorMappingList != null)
                            {
                                // prevotheramnt = PurchaseOrderHeaderList[e.Row.RowIndex].FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.IVM_OTHER_AMOUNT);
                                //prevotheramnt = PurchaseOrderHeaderList[e.Row.RowIndex].InvoiceVendorMappingList.Where(tm => tm.VendorHeader.DeleteStatus == 0).
                                //                                        Sum(tm => tm.OtherAmount - tm.PurchaseOrder.VendorAdvanceDeductionList.Where(d => d.InvoicePK == tm.InvoicePK).Sum(amt => amt.OtherAmount));
                                prevotheramnt = PurchaseOrderHeaderList[e.Row.RowIndex].OtherChargesPreviousAmount;
                            }
                            hdfOtherChargesPrev.Value = prevotheramnt.ToString();

                        }

                        if (InvoiceVendorMappingList != null && InvoiceVendorMappingList.Count > 0)
                        {
                            lnkRemove.CommandArgument = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PurchaseOrderPK.ToString();
                            hdfPONumber.Value = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PurchaseOrderPK.ToString();
                            POdate = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PODate.ToString(Resources.Constants.DateFormatShort);
                            ////lblPONo.Text = ERP.Utilities.CommonFunctions.GetShortString(InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.POH_NO, 15);
                            ////lblPONo.ToolTip = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.POH_NO + "  " + POdate;
                            lbtnPONo.Text = ERP.Utilities.CommonFunctions.GetShortString(InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PONo, 15);
                            lbtnPONo.ToolTip = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PONo + "  " + POdate;
                            hdfPoPk.Value = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PurchaseOrderPK.ToString();

                            lblPODate.Text = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PODate.ToString(Resources.Constants.DateFormatShort);
                            lblPODate.ToolTip = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PODate.ToString(Resources.Constants.DateFormatShort);
                            lblVendorInv.Text = ERP.Utilities.CommonFunctions.GetShortString(InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.Vendor.VEN_NAME, 5);
                            lblVendorInv.ToolTip = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.Vendor.VEN_NAME;
                            lblCurrency.Text = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.Currency.CUR_CODE;
                            lblCurrency.ToolTip = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.Currency.CUR_CODE;

                            //Gross Total 
                            //lblGrossAmount.Text = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.POH_SUB_TOTAL);
                            //lblGrossAmount.ToolTip = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.POH_SUB_TOTAL);
                            decimal GrossAmount = 0;
                            //if (InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PurchaseOrderDetailList != null)
                            //{
                            //    foreach (PurchaseOrdeDetails dtlObj in InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PurchaseOrderDetailList)
                            //    {
                            //        //IN the case of Service Grossamount is from POD_AMT_VALUE
                            //        if (InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.GroupPK != 2)
                            //        {
                            //            GrossAmount = GrossAmount + dtlObj.Amount;
                            //        }
                            //        else
                            //        {
                            //            GrossAmount = GrossAmount + dtlObj.AmountValue;
                            //        }
                            //    }
                            //}
                            GrossAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.GrossAmount;
                            lblGrossAmount.Text = String.Format("{0:c}", GrossAmount);
                            lblGrossAmount.ToolTip = String.Format("{0:c}", GrossAmount);
                            //End GrossTotal

                            decimal tax = 0;
                            decimal disc = 0;
                            decimal.TryParse(Convert.ToString(InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.AdditionalTaxAmount), out tax);
                            decimal.TryParse(Convert.ToString(InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.DiscountAmount), out disc);


                            //if (InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PurchaseOrderDetailList != null)
                            //{
                            //    foreach (PurchaseOrdeDetails dtlObj in InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PurchaseOrderDetailList)
                            //    {
                            //        if (dtlObj.TaxDetailList != null)
                            //            tax += (decimal)dtlObj.TaxDetailList.Where(aa => aa.CategoryPK == 1).Sum(aa => aa.TaxAmount);
                            //        disc += (decimal)dtlObj.TaxDetailList.Where(aa => aa.CategoryPK == 3).Sum(aa => aa.TaxAmount);
                            //    }
                            //}

                            tax = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TaxAmount;
                            disc = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.POHDiscountAmount;

                            lblTax.ToolTip = lblTax.Text = String.Format("{0:c}", tax);
                            lblDiscount.Text = lblDiscount.ToolTip = String.Format("{0:c}", disc);
                            decimal otherAmount = 0;
                            decimal InvoicedAmount = 0;
                            decimal InvoicedOtherCharge = 0;
                            decimal InvoicedDiscount = 0;
                            decimal InvoicedTax = 0;
                            decimal AdvInvoicedAmount = 0;
                            decimal AllocatedAdvAmount = 0;
                            decimal BalanceAmount = 0;
                            decimal.TryParse(Convert.ToString(InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.ShippingCharge), out otherAmount);

                            lblOtherAmount.Text = String.Format("{0:c}", otherAmount);
                            lblOtherAmount.ToolTip = String.Format("{0:c}", otherAmount);

                            lblTotalAmount.Text = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TotalValue);
                            lblTotalAmount.ToolTip = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TotalValue);

                            if (dicTempAmount != null && dicTempAmount.Count > 0 && dicTempAmount.ContainsKey(hdfPONumber.Value))
                            {
                                txtPayNow.Text = dicTempAmount.SingleOrDefault(aa => aa.Key == hdfPONumber.Value).Value.ToString();
                            }
                            else
                            {
                                txtPayNow.Text = GetFormattedCurrency(Convert.ToDecimal(InvoiceVendorMappingList[e.Row.RowIndex].Amount));
                                txtPayNow.Text = Convert.ToDecimal(txtPayNow.Text) < 0 ? GetFormattedCurrency(0) : txtPayNow.Text;
                            }
                            hdfPayNow.Value = txtPayNow.Text;

                            //InvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0).Sum(c => c.IVM_AMOUNT - c.IVM_DISCOUNT_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_OTHER_AMOUNT) - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : finInvoiceVndTrxMpgList[e.Row.RowIndex].PurchaseOrder.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0).Sum(c => c.IVM_AMOUNT - c.IVM_DISCOUNT_AMOUNT + c.IVM_TAX_AMOUNT + c.IVM_OTHER_AMOUNT);
                            //Invoiced amnt getting -ve if other charge exist
                            //AdvInvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 & r.FIN_INVOICE_VND_HDR.IVH_STATUS > 0 & r.FIN_INVOICE_VND_HDR.IVH_CATEGORY == (byte)POInvoiceCategory.Advanced).Sum(c => c.IVM_AMOUNT - c.IVM_OTHER_AMOUNT);

                            //AdvInvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK == (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount);
                            //InvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].VendorHeader.DeleteStatus == 0 ? InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK != (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount - c.Discount + c.TaxAmount + c.OtherAmount) : 0;
                            AdvInvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.AdvanceInvoiceAmount;
                            InvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceAmount;
                            //InvoicedAmount = WkfStatus > 0 ? (InvoicedAmount + AdvInvoicedAmount) - (decimal.Parse(txtPayNow.Text)) : InvoicedAmount + AdvInvoicedAmount;
                            InvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceAmount;
                            BalanceAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.BalanceAmount;
                            AllocatedAdvAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.AllocatedAdvanceAmount;

                            if (InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.Type == (byte)PurchaseType.Import || !IsAdvInvHasTax || CurrPK > 0)
                            {
                                //InvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK != (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount - c.Discount + c.TaxAmount + c.OtherAmount);
                                //AdvInvoicedAmount = InvoiceVendorMappingList[e.Row.RowIndex].VendorHeader.DeleteStatus == 0 ? InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK == (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount) - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceVendorMappingList.Where(r => r.VendorHeader.DeleteStatus == 0 & r.VendorHeader.Status > 0 & r.VendorHeader.CategoryPK == (byte)POInvoiceCategory.Advanced).Sum(c => c.Amount);
                                //AllocatedAdvAmount = InvoiceVendorMappingList[e.Row.RowIndex].VendorHeader.FIN_INVOICE_VND_ADV_DED_DTL.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.VAD_AMOUNT);
                                //InvoicedAmount = InvoicedAmount + AllocatedAdvAmount;
                                if (InvoicedAmount < 0)
                                    InvoicedAmount = 0;
                                lblInvoiced.Text = String.Format("{0:c}", InvoicedAmount);
                                lblAdvInvoiced.Text = String.Format("{0:c}", AdvInvoicedAmount);
                                //if ((InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TotalValue - InvoicedAmount) > (InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TotalValue - AdvInvoicedAmount))
                                //{
                                //    BalanceAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TotalValue - AdvInvoicedAmount;
                                //}
                                //else
                                //{
                                //    BalanceAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TotalValue - InvoicedAmount;
                                //}
                                lblBalancetoInvoice.Text = String.Format("{0:c}", BalanceAmount);
                                lblBalancetoInvoice.Text = Convert.ToDecimal(lblBalancetoInvoice.Text.Replace(",", "")) < 0 ? String.Format("{0:c}", 0) : lblBalancetoInvoice.Text;
                                grdPOList.Columns[10].Visible = true;
                            }
                            else
                            {

                                grdPOList.Columns[10].Visible = false;
                                //AllocatedAdvAmount = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.VendorAdvanceDeductionList.Where(r => r.VendorHeader.DeleteStatus == 0).Sum(c => c.Amount - c.OtherAmount - c.TaxAmount);
                                //lblInvoiced.Text = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.POH_AMT_INVOICED - (WkfStatus > 0 ? decimal.Parse(txtPayNow.Text) : 0) : finInvoiceVndTrxMpgList[e.Row.RowIndex].PurchaseOrder.POH_AMT_INVOICED);
                                InvoicedAmount -= AllocatedAdvAmount;
                                lblInvoiced.Text = String.Format("{0:c}", InvoicedAmount);
                                //AdvInvoicedAmount = 0;
                                //lblAdvInvoiced.Text = String.Format("{0:c}", AdvInvoicedAmount);
                                lblBalancetoInvoice.Text = (Convert.ToDecimal(InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TotalValue) - decimal.Parse(lblInvoiced.Text.Replace(",", ""))).ToString();
                                lblBalancetoInvoice.Text = Convert.ToDecimal(lblBalancetoInvoice.Text.Replace(",", "")) < 0 ? String.Format("{0:c}", 0) : lblBalancetoInvoice.Text;

                            }


                            lblPOTax.Text = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].TaxAmount);
                            //txtOthercharges.Text = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].IVM_OTHER_AMOUNT);
                            decimal otherCharges = Convert.ToDecimal(InvoiceVendorMappingList[e.Row.RowIndex].OtherAmount);
                            txtOthercharges.Text = GetFormattedCurrency(Math.Round(otherCharges, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));

                            //hdfOtherchargeOLD.Value = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0 ? InvoiceVendorMappingList[e.Row.RowIndex].IVM_OTHER_AMOUNT - (decimal.Parse(txtOthercharges.Text)) : finInvoiceVndTrxMpgList[e.Row.RowIndex].IVM_OTHER_AMOUNT);
                            //hdfOtherchargeOLD.Value = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceVendorMappingList.Where(tm => tm.VendorHeader.DeleteStatus == 0).Sum(tm => tm.OtherAmount - tm.PurchaseOrder.VendorAdvanceDeductionList.Where(d => d.InvoicePK == tm.InvoicePK).Sum(amt => amt.OtherAmount)));
                            hdfOtherchargeOLD.Value = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.OtherChargesAmount.ToString();

                            //lblInvoiced.Text = String.Format("{0:c}", InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.POH_AMT_INVOICED - decimal.Parse(txtPayNow.Text));
                            hdfPriceAdjustment.Value = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PriceAdjust.ToString();
                            hdfTaxPercentage.Value = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.TaxPercentage.ToString();
                            //hdfTaxPercentage.Value = InvoiceVendorMappingList[e.Row.RowIndex].t.PriceAdjust.ToString();
                            // Setting Tax Details popup  
                            if (tax > 0)
                            {
                                lblTax.CommandArgument = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.PurchaseOrderPK.ToString();
                            }
                            else
                            {
                                lblTax.CommandArgument = "0";
                            }

                            decimal prevotheramnt = 0;
                            //if (InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceVendorMappingList != null)
                            //{
                            //    // prevotheramnt = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.FIN_INVOICE_VND_TRX_MPG.Where(r => r.FIN_INVOICE_VND_HDR.IVH_DEL_STATUS == 0).Sum(c => c.IVM_OTHER_AMOUNT);
                            //    prevotheramnt = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.InvoiceVendorMappingList.Where(tm => tm.VendorHeader.DeleteStatus == 0).Sum(tm => tm.OtherAmount - tm.PurchaseOrder.VendorAdvanceDeductionList.Where(d => d.InvoicePK == tm.InvoicePK).Sum(amt => amt.OtherAmount));
                            //}
                            prevotheramnt = InvoiceVendorMappingList[e.Row.RowIndex].PurchaseOrder.OtherChargesPreviousAmount;

                            //For Resolving Bug ID:  2454
                            //hdfOtherChargesPrev.Value = (prevotheramnt - (WkfStatus > 0 ? decimal.Parse(txtOthercharges.Text) : 0)).ToString();
                            hdfOtherChargesPrev.Value = (prevotheramnt - decimal.Parse(txtOthercharges.Text)).ToString();
                        }

                        lblInvoiced.Text = String.Format("{0:c}", decimal.Parse(lblInvoiced.Text.Replace(",", "")));
                        lblInvoiced.ToolTip = lblInvoiced.Text;
                        lblBalancetoInvoice.Text = String.Format("{0:c}", decimal.Parse(lblBalancetoInvoice.Text.Replace(",", "")));
                        lblBalancetoInvoice.ToolTip = lblBalancetoInvoice.Text;
                        txtPayNow.Text = GetFormattedCurrency(Math.Round(decimal.Parse(txtPayNow.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        txtPayNow.Text = Convert.ToDecimal(txtPayNow.Text) < 0 ? GetFormattedCurrency(0) : txtPayNow.Text;
                        txtPayNow.Focus();

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

                        if (InvoiceVendorMappingList != null && InvoiceVendorMappingList.Count > 0)
                        {
                            //Amount = decimal.Parse(InvoiceVendorMappingList[0].IVM_AMOUNT.ToString());
                            Amount = decimal.Parse(InvoiceVendorMappingList.Sum(iv => iv.Amount).ToString());
                            TaxAmount = decimal.Parse(InvoiceVendorMappingList.Sum(iv => iv.PurchaseOrder.AdditionalTaxAmount).ToString());
                            DiscAmount = decimal.Parse(InvoiceVendorMappingList.Sum(iv => iv.PurchaseOrder.DiscountAmount).ToString());
                            NetAmount = Amount;
                        }
                        if (PurchaseOrderHeaderList != null && PurchaseOrderHeaderList.Count > 0)
                        {
                            Amount = decimal.Parse(PurchaseOrderHeaderList.Sum(po => po.TotalValue).ToString()) -
                                     decimal.Parse(PurchaseOrderHeaderList.Sum(po => po.InvoicedAmount).ToString());
                            TaxAmount = decimal.Parse(PurchaseOrderHeaderList.Sum(iv => iv.AdditionalTaxAmount).ToString());
                            DiscAmount = decimal.Parse(PurchaseOrderHeaderList.Sum(iv => iv.DiscountAmount).ToString());
                        }

                        TotalPayNow = Amount + TaxAmount - DiscAmount;

                        hdfTaxAmt.Value = TaxAmount.ToString();
                        hdfDiscAmt.Value = DiscAmount.ToString();

                        decimal Tax = 0;
                        decimal Discount = 0;
                        decimal.TryParse(txtTaxAmount.Text, out Tax);
                        decimal.TryParse(txtDiscount.Text, out Discount);

                        InvAmount = NetAmount - Tax + Discount;

                        //lblTotalPayNowFooter.Text = Amount.ToString();
                        //lblTotalPayNowFooter.Text = Convert.ToDecimal(lblTotalPayNowFooter.Text) < 0 ? "0" : lblTotalPayNowFooter.Text;
                        lblTotalPayNowFooter.Text = GetFormattedCurrencyWithComa(Amount);
                        lblTotalPayNowFooter.Text = Amount < 0 ? "0" : GetFormattedCurrencyWithComa(Amount);
                        //txtNetAmount.Text = NetAmount.ToString();
                        //txtInvoiceAmt.Text = InvAmount.ToString();
                        //txtTaxAmount.Text = Tax.ToString();
                        //txtDiscount.Text = Discount.ToString();
                        //txtTaxAmount.Text = Math.Round(decimal.Parse(txtTaxAmount.Text),Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        //txtNetAmount.Text = Math.Round(decimal.Parse(txtNetAmount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        //txtDiscount.Text = Math.Round(decimal.Parse(txtDiscount.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        //txtInvoiceAmt.Text = Math.Round(decimal.Parse(txtInvoiceAmt.Text), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                        lblTotalPayNowFooter.Text = String.Format("{0:c}", decimal.Parse(lblTotalPayNowFooter.Text.Replace(",", "")));
                        hdfTotalPayNowFooter.Value = Amount < 0 ? "0" : Amount.ToString();
                        lblTotalPayNowFooter.ToolTip = lblTotalPayNowFooter.Text;

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

                    }
                    else if (e.Row.RowType == DataControlRowType.Header)
                    {

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

                    }
                }
                else if (((GridView)sender).ID == "grdPOInvoiceList")
                {
                    if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                    {
                        //Approved & Posted Icons Assigning
                        Button imgApproved = e.Row.FindControl("imgApproved") as Button;
                        Button imgPosted = e.Row.FindControl("imgPosted") as Button;

                        HiddenField hdfApproved = e.Row.FindControl("hdfApproved") as HiddenField;
                        HiddenField hdfPosted = e.Row.FindControl("hdfPosted") as HiddenField;

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
            btnPickForPayment.PreRender += new EventHandler(btnAction_PreRender);
            btnPickForCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            btnResetSelection.PreRender += new EventHandler(btnAction_PreRender);

            //lbnPOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnkInvoicing.PreRender += new EventHandler(btnAction_PreRender);
            //lbnPOInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lbnExpenses.PreRender += new EventHandler(btnAction_PreRender);
            //lnkPayment.PreRender += new EventHandler(btnAction_PreRender);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnAddItem.PreRender += new EventHandler(btnAction_PreRender);


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
            btnPickForPayment.Load += new EventHandler(btnAction_Load);
            btnPickForCrDrNote.Load += new EventHandler(btnAction_Load);
            btnResetSelection.Load += new EventHandler(btnAction_Load);

            //lbnPOListing.Load += new EventHandler(btnAction_Load);
            //lnkInvoicing.Load += new EventHandler(btnAction_Load);
            //lbnPOInvoice.Load += new EventHandler(btnAction_Load);
            //lbnExpenses.Load += new EventHandler(btnAction_Load);
            //lnkPayment.Load += new EventHandler(btnAction_Load);
            //lnbCrDrNote.Load += new EventHandler(btnAction_Load);
            //lnbAcPayables.Load += new EventHandler(btnAction_Load);

            lnkList.Load += new EventHandler(btnAction_Load);
            lnkDetail.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnAddItem.Load += new EventHandler(btnAction_Load);
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

            //GetFieldValues(ControlsEnum.VENDORCONTACTYPE);
            GetFieldValues(ControlsEnum.VENDORCONTACTYPEDETAILS);
            if (dsAdsTypeDtl != null && dsAdsTypeDtl.Tables.Count > 0)
            {
                if (dsAdsTypeDtl.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsAdsTypeDtl.Tables[0].Rows[0][Resources.DataFieldRes.vncType]) == (int)VendorContactTypeEnum.Branch)
                    {
                        txtBranchCode.Enabled = true;
                        vrfBranchCode.Enabled = true;
                        //txtBranchCode.CssClass = "";
                    }
                    else
                    {
                        //txtBranchCode.Enabled = false;
                        //txtBranchCode.Text = "";
                        vrfBranchCode.Enabled = false;
                        //txtBranchCode.CssClass = "input-disabled";
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
            INVOICEGSTTYPE

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
    }
}
