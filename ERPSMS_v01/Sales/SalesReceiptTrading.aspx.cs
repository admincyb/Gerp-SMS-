using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BusinessObject.AccountManagement;
using ERPManager;
using ERPData;
using ERP.Utilities;
using ERPSMS_v01.UserControls;
using BusinessObject.Common;
using ERPService;
using BusinessObject.CommonManagement;
using BusinessObject;
using System.Data;
using System.Threading;
using System.Web.UI.HtmlControls;
using BusinessObject.Journalize;
using BusinessObject.SaleOrder;
using ERPManager.Sales;
using BusinessLogic.CommonManagement;
using BusinessObject.Sales;

namespace ERPSMS_v01.Sales
{
    public partial class SalesReceiptTrading : ERP.Store.UI.WorkFlowBasePage
    {
        #region Variables and Properties
        #region Properties
        /// <summary>
        /// 
        /// </summary>
        private List<ReceiptSuspendDetails> lstReceiptSuspendDetails
        {
            get { return this.ViewState["ReceiptSuspendDetails"] == null ? new List<ReceiptSuspendDetails>() : (List<ReceiptSuspendDetails>)this.ViewState["ReceiptSuspendDetails"]; }
            set { this.ViewState["ReceiptSuspendDetails"] = value; }
        }
        /// <summary>
        /// Keep Config Value
        /// </summary>
        private List<ADM_CONFIG_MST> admConfigMstList
        {
            get
            {
                return this.ViewState["AdmConfigMstList"] == null ? new List<ADM_CONFIG_MST>() : (List<ADM_CONFIG_MST>)this.ViewState["AdmConfigMstList"];
            }
            set
            {
                this.ViewState["AdmConfigMstList"] = value;
            }
        }
        private int SelectedBankAccount
        {
            get
            {
                return this.ViewState["SelectedBankAccount"] == null ? 0 : Convert.ToInt32(this.ViewState["SelectedBankAccount"]);
            }
            set
            {
                this.ViewState["SelectedBankAccount"] = value;
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
        /// Set SBU ID
        /// </summary>
        private int SBUID
        {
            get
            {
                return this.ViewState[ViewstateStrings.SBUID] == null ? -1 : Convert.ToInt32(this.ViewState[ViewstateStrings.SBUID]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SBUID] = value;
            }

        }
        /// <summary>
        /// Payment Type from PI
        /// </summary>
        private SalesInvoiceGroup SOGroup
        {
            get
            {
                return (this.ViewState[ViewstateStrings.SOGroup] == null ? (SalesInvoiceGroup)Enum.Parse(typeof(SalesInvoiceGroup),
                    CommonConstants.SELECT_VALUE_ZERO) : (SalesInvoiceGroup)this.ViewState[ViewstateStrings.SOGroup]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SOGroup] = value;
            }
        }


        /// <summary>
        /// Payment category From PI
        /// </summary>
        private SalesInvoiceCategory SICategory
        {
            get
            {
                return (this.ViewState[ViewstateStrings.SICategory] == null ? (SalesInvoiceCategory)Enum.Parse(typeof(SalesInvoiceCategory),
                    CommonConstants.SELECT_VALUE_ONE) : (SalesInvoiceCategory)this.ViewState[ViewstateStrings.SICategory]);
            }
            set
            {
                this.ViewState[ViewstateStrings.SICategory] = value;
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
        /// Save Receipt Type
        /// </summary>
        private int recipetType
        {
            get
            {
                return this.ViewState["recipetType"] == null ? 0 : Convert.ToInt32(this.ViewState["recipetType"]);
            }
            set
            {
                this.ViewState["recipetType"] = value;
            }
        }
        private decimal adjAmtFooter
        {
            get
            {
                return this.ViewState["adjAmtFooter"] == null ? Convert.ToDecimal(0) : Convert.ToDecimal(this.ViewState["adjAmtFooter"]);
            }
            set
            {
                this.ViewState["adjAmtFooter"] = value;
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
                return this.ViewState[ViewstateStrings.TotalPages] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.TotalPages].ToString());
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
        /// VendorPK
        /// </summary>
        private long ReceiptMpgPK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.ReceiptMpgPK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ReceiptMpgPK] = value;
            }
        }

        /// <summary>
        /// TrxIndex
        /// </summary>
        private int TrxIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.TrxIndex] == null ? -1 : Convert.ToInt32(this.ViewState[ViewstateStrings.TrxIndex].ToString());
            }
            set
            {
                this.ViewState[ViewstateStrings.TrxIndex] = value;
            }
        }

        /// <summary>
        /// VendorPK
        /// </summary>
        private decimal PayNowAmount
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.PayNowAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.PayNowAmount] = value;
            }
        }

        /// <summary>
        /// VendorPK
        /// </summary>
        private decimal AdjnNowAmount
        {
            get
            {
                return Convert.ToDecimal(this.ViewState[ViewstateStrings.AdjnNowAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.AdjnNowAmount] = value;
            }
        }

        /// <summary>
        /// To maintain the Adjustment amount in viewstate
        /// </summary>
        private decimal AdjustmentAmount
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.AdjustmentAmount] == null ? 0 : (decimal)ViewState[ERP.Utilities.ViewstateStrings.AdjustmentAmount];
            }
            set
            {
                this.ViewState[ViewstateStrings.AdjustmentAmount] = value;
            }
        }

        /// <summary>
        /// VendorPK
        /// </summary>
        private long InvoicePK
        {
            get
            {
                return Convert.ToInt64(this.ViewState[ViewstateStrings.invoicePK]);
            }
            set
            {
                this.ViewState[ViewstateStrings.invoicePK] = value;
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
        /// IsApply
        /// </summary>
        private int IsApply
        {
            get
            {
                return Convert.ToInt32(this.ViewState[ViewstateStrings.IsApply]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsApply] = value;
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
        /// Is receipt cancelled or not
        /// </summary>
        private bool IsDeleted
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsDeleted] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.IsDeleted]);
            }
            set
            {
                this.ViewState[ViewstateStrings.IsDeleted] = value;
            }
        }

        /// <summary>
        /// Show or hide tax for Miscellaneous Invoice
        /// </summary>
        private bool ShowTaxForMiscInv
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowTaxForMiscInv] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.ShowTaxForMiscInv]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowTaxForMiscInv] = value;
            }
        }

        /// <summary>
        /// To maintain keep selected pos
        /// </summary>
        private List<AdjAllocation> ReceiptAdjnList
        {

            get
            {
                return this.ViewState[ViewstateStrings.ReceiptAdjnList] != null ? (List<AdjAllocation>)this.ViewState[ViewstateStrings.ReceiptAdjnList] : new List<AdjAllocation>();
            }
            set
            {
                if (value == null)
                    ViewState.Remove(ViewstateStrings.ReceiptAdjnList);
                else
                    this.ViewState[ViewstateStrings.ReceiptAdjnList] = value;
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
        /// To maintain the tax in viewstate
        /// </summary>
        private decimal Tax
        {
            get
            {
                return this.ViewState[ViewstateStrings.Tax] == null ? 0 : Convert.ToDecimal(this.ViewState[ViewstateStrings.Tax]);
            }
            set
            {
                this.ViewState[ViewstateStrings.Tax] = value;
            }
        }
        /// <summary>
        /// To maintain the Paynow in viewstate
        /// </summary>
        private decimal PayNow
        {
            get
            {
                return ViewState[ERP.Utilities.ViewstateStrings.PayNow] == null ? 0 : (decimal)ViewState[ERP.Utilities.ViewstateStrings.PayNow];
            }
            set
            {
                this.ViewState[ViewstateStrings.PayNow] = value;
            }
        }

        /// <summary>
        /// To maintain the other charge in viewstate
        /// </summary>
        private decimal InvOtherCharge
        {
            get
            {
                return this.ViewState[ViewstateStrings.POTotalOtherAmount] == null ? 0 : Convert.ToDecimal(this.ViewState[ViewstateStrings.POTotalOtherAmount]);
            }
            set
            {
                this.ViewState[ViewstateStrings.POTotalOtherAmount] = value;
            }
        }

        /// <summary>
        /// To keep Invoice pk list for split applied invoices
        /// </summary>
        private List<long> AppliedInvPkList
        {
            get
            {
                return ViewState[ViewstateStrings.AppliedInvPkList] == null ? null : (List<long>)ViewState[ViewstateStrings.AppliedInvPkList];
            }
            set
            {
                this.ViewState[ViewstateStrings.AppliedInvPkList] = value;
            }
        }

        /// <summary>
        /// To maintain the CR/DR allocation list
        /// </summary>
        private List<ReceiptCrdrMpg> ReceiptCrdrList
        {

            get
            {
                return this.ViewState[ViewstateStrings.ReceiptCrdrList] != null ? (List<ReceiptCrdrMpg>)this.ViewState[ViewstateStrings.ReceiptCrdrList] : new List<ReceiptCrdrMpg>();
            }
            set
            {
                if (value == null)
                    ViewState.Remove(ViewstateStrings.ReceiptCrdrList);
                else
                    this.ViewState[ViewstateStrings.ReceiptCrdrList] = value;
            }

        }

        /// <summary>
        /// To keep row index
        /// </summary>
        private int CrdrRowIndex
        {
            get
            {
                return this.ViewState[ViewstateStrings.RowIndex] != null ? Convert.ToInt32(this.ViewState[ViewstateStrings.RowIndex]) : -1;
            }
            set
            {
                this.ViewState[ViewstateStrings.RowIndex] = value;
            }
        }

        /// <summary>
        /// Is debit note added
        /// </summary>
        private bool IsDebitNoteApplied
        {
            get
            {
                return this.ViewState[ViewstateStrings.IsDebitNoteApplied] == null ? false : (bool)this.ViewState[ViewstateStrings.IsDebitNoteApplied];
            }
            set
            {
                this.ViewState[ViewstateStrings.IsDebitNoteApplied] = value;
            }
        }

        /// <summary>
        /// To show or hide adj allocation column
        /// </summary>
        private bool ShowAdjColumn
        {
            get
            {
                return this.ViewState[ViewstateStrings.ShowAdjColumn] == null ? false : Convert.ToBoolean(this.ViewState[ViewstateStrings.ShowAdjColumn]);
            }
            set
            {
                this.ViewState[ViewstateStrings.ShowAdjColumn] = value;
            }
        }

        /// <summary>
        /// To maintain keep invoice and correspoding details in session
        /// </summary>
        private ReceiptHeader ReceiptHeaderSession
        {
            get
            {
                return (ReceiptHeader)Session[ERP.Utilities.SessionStrings.ReceiptHeaderSession];
            }
            set
            {
                Session[ERP.Utilities.SessionStrings.ReceiptHeaderSession] = value;
            }
        }

        private int CustomerType
        {
            get
            {
                return this.ViewState[ViewstateStrings.CustomerType] == null ? 0 : Convert.ToInt32(this.ViewState[ViewstateStrings.CustomerType]);
            }
            set
            {
                this.ViewState[ViewstateStrings.CustomerType] = value;
            }
        }
        #endregion
        private DataTable dtDiscountTypes;
        // Indicates the state as well as action
        private ActionsEnum commonActions;
        User currentUser;
        //page related Entity Object
        private ServiceUtility serviceUtilityObj;
        private FIN_RECEIPT_CUS_TRX_MPG finReceiptCusTrxMpgObj;
        private FIN_INVOICE_CUS_HDR finInvoiceCusHdrObj;
        private ADM_COMPANY_MST admCompanyMstObj;
        private List<ADM_COMPANY_MST> admCompanyMstList;
        private FIN_INVOICE_CUS_TRX_MPG FinInvoiceCusTrxMpgObj;
        private AdjAllocation FinReceiptCusAllocationObj;
        private FIN_INVOICE_CUS_HDR finInvoiceVndHdrObjForPaymentSplit;
        //List for binding details to controls  
        private List<FIN_RECEIPT_CUS_HDR> finReceiptCusHdrList;
        private List<FIN_RECEIPT_CUS_TRX_MPG> finReceiptCusTrxMpgList;
        private List<FIN_INVOICE_CUS_HDR> ObjFinInvoiceCusHdrList;
        private List<FIN_CASH_BANK_MST> finCashBankMstList;
        private List<FIN_CASH_BANK_MST> crmCustomerMstList;
        private List<ReceiptSOMapping> finReceiptCusSoMpgList;
        private List<FIN_INVOICE_CUS_TRX_MPG> FinInvoiceCusTrxMpgList;
        private List<FIN_INVOICE_CUS_TRX_MPG> FinInvoiceCusTrxMpgListForAutoAlcn;
        private List<AdjAllocation> FinReceiptCusAllocationList;
        private List<FIN_INVOICE_CUS_HDR> finInvoiceVndHdrListForPaymentSplit;
        private List<ReceiptCrdrMpg> FinReceiptCusCrdrMpgList;
        private List<FIN_CRDR_NOTE_MPG> FinCrdrMpgList;
        private DataSet dsPageData;
        private DataSet dsCustomerData;
        bool isCancelled = false;
        decimal taxAmnt = 0;
        decimal adjAmnt = 0;
        decimal otherAmnt = 0;
        decimal receiveAmnt = 0;
        decimal reductionAmnt = 0;
        int isSplitApply = 0;
        int JournalPK;
        private bool updateReceipt;
        private string refID;
        private string inboxFlag;
        private FIN_TRX_HDR finTrxHdrObj;
        private List<FIN_TRX_HDR> finTrxHdrList;
        private List<FIN_TRX_HDR> finTrxHdrPDCList;
        private List<SPADM_APP_STATUS_CFG_GET_KV_Result> workflowStatusList;
        private List<FIN_YEAR_MST> finYearMstList;
        private ADM_CURRENCY_MST admCurrencyMstObj;
        private List<ADM_CURRENCY_MST> admCurrencyMstList;
        private List<AdjAllocation> tempReceiptAdjnList;

        private int invPK;
        private int custPK;
        private string TypeRef;
        private bool InstrNoResult;
        private bool IsContReturn = false;
        private long ICH_PK;
        DataTable dtCompany = new DataTable();
        DataTable dtReceivedAmt = new DataTable();
        private DataTable dtPendingInvList;
        private DataTable dtInvoiceType;
        private DataTable dtPaymentModes;
        private DataTable dtPendingCrAdjn;
        private DataTable dtReceiptList;
        private DataTable dtInvCategory;
        private DataTable dtSuspenceList;
        private int recipetTypePDC;
        DataTable dtReceiptAlloc;
        private InvoiceBO InvListObj;
        private ReceiptHeader receiptHeaderObj;
        private ReceiptHeader tempReceiptHeader;
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
                    ucrWrkf.WrkfSubmit += new EventHandler(ActionHandler);
                    hdfJournalizeWorkFlow.Value = "0";
                }
                ucrWrkf.ViewType = 1;

                ucrJournalize.JournalizeSave += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeSubmit += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeDelete += new EventHandler(ActionHandler);
                ucrJournalize.JournalizeCancel += new EventHandler(ActionHandler);

                ucrJournalize.ReverseSave += new EventHandler(ActionHandler);
                ucrJournalize.ReverseSubmit += new EventHandler(ActionHandler);
                ucrJournalize.ReverseDelete += new EventHandler(ActionHandler);
                ucrJournalize.ReverseCancel += new EventHandler(ActionHandler);

                ucrJournalize.ReturnSave += new EventHandler(ActionHandler);
                ucrJournalize.ReturnSubmit += new EventHandler(ActionHandler);
                ucrJournalize.ReturnDelete += new EventHandler(ActionHandler);
                ucrJournalize.ReturnCancel += new EventHandler(ActionHandler);
                divErrorLabelAdjn.Visible = false;
                divErrorLabelAdjnTotamt.Visible = false;
                divBaltoAll.Visible = false;
                divWrongAdj.Visible = false;
                if (!IsPostBack)
                {

                    SetConfigValue();
                    GetFieldValues(ControlsEnum.COMPANY);
                    SetFieldValues(ControlsEnum.COMPANY);
                    GetFieldValues(ControlsEnum.INVOICETYPE);
                    SetFieldValues(ControlsEnum.INVOICETYPE);
                    GetFieldValues(ControlsEnum.INVCATEGORY);
                    SetFieldValues(ControlsEnum.INVCATEGORY);
                    GetFieldValues(ControlsEnum.BANKCURRENCY);
                    SetFieldValues(ControlsEnum.BANKCURRENCY);
                    GetFieldValues(ControlsEnum.DISCOUNTTYPE);
                    SetFieldValues(ControlsEnum.DISCOUNTTYPE);
                    ReceiptCrdrList = null;
                    hdfJournalizeWorkFlow.Value = "0";
                    Session[ERP.Utilities.SessionStrings.TransactionType] = null;
                    txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
                    txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                    hdfSearchDateTo.Value = DateTime.Now.ToString();
                    hdfDecimalDigits.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits.ToString();
                    hdfCurrencyGroup2.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[0].ToString();
                    hdfCurrencyGroup1.Value = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSizes[1].ToString();
                    hdfCurrencyFormat.Value = "#0.";
                    hdfCurrencyFormatWithSeperator.Value = "#" + Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator + "#0.";
                    for (int i = 0; i < Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits; i++)
                    {
                        hdfCurrencyFormat.Value += "0";
                        hdfCurrencyFormatWithSeperator.Value += "0";
                    }
                    GetFieldValues(ControlsEnum.PAYMODE);
                    SetFieldValues(ControlsEnum.PAYMODE);

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

                    //If Request From External(Report or Other page) otherthan Menu or Inbox
                    if (Request.QueryString[QueryStrings.FromExt] != null && Request.QueryString[QueryStrings.FromExt] == "T")
                    {
                        ucrWrkf.ViewType = 0;
                        EntryStatus = EntryStatus.VIEWMODE;
                        CurrPK = Convert.ToInt32(Request.QueryString[QueryStrings.PK]);
                        GetFieldValues(ControlsEnum.RECEIPTHEADER);
                        SetFieldValues(ControlsEnum.RECEIPTGET);
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
                                //btnSave.Visible = false;
                                // divbtnSavePaymentSplit.Visible = false;
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
                                {
                                    hdfIsCancelled.Value = "1";//For Showing Cancelled Stamp in Detail Page
                                    IsDeleted = true;
                                }
                            }
                            else if (pid.Equals("2") || pid.Equals("12") || pid.Equals("14") || pid.Equals("16"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETRECEIPTPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                                }
                            }
                            else if (pid.Equals("4"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETRECEIPTPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                                }
                            }
                            else if (pid.Equals("6"))
                            {
                                ucrWrkf.RefID = int.Parse(refID);
                                JournalPK = GetApplicationID(ucrWrkf.RefID);
                                GetFieldValues(ControlsEnum.GETRECEIPTPKBYJOURNALPK);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    CurrPK = (Int32)finTrxHdrList[0].FTH_REF_PK;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                                }
                            }
                            ////
                        }
                        else if (!string.IsNullOrEmpty(prefID))
                        {
                            base.WkfRefID = ucrWrkf.RefID = int.Parse(prefID);
                            CurrPK = GetApplicationID(ucrWrkf.RefID);
                        }
                        if (CurrPK > 0)
                        {
                            SetCancelRef((int)CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.NEWMODE) && ucrWrkf.HasPageTaskPermission)
                                ucrWrkf.ViewType = 1;
                            else
                            {
                                ucrWrkf.ViewType = 0;
                                //EntryStatus = EntryStatus.VIEWMODE;
                            }
                            SetReceiptMode();
                            GetFieldValues(ControlsEnum.RECEIPTHEADER);
                            SetFieldValues(ControlsEnum.RECEIPTHEADER);
                            SetFieldValues(ControlsEnum.RECEIPTMPGLIST);

                        }
                        else
                        {
                            //Sets data key for the gird
                            string[] datakeyarray;
                            datakeyarray = new string[1];
                            datakeyarray[0] = Resources.DataFieldRes.SalesReceiptPK;
                            grdReceiptList.DataKeyNames = datakeyarray;

                            FillProcessID(1);
                            ResetForm();
                            GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                            SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                            this.btnNew.Focus();
                            EntryStatus = EntryStatus.LISTMODE;
                            PageIndex = "1";
                            uclPaging.TotalPages = TotalPages;
                            uclPaging.CurrentPage = 1;
                            hdfReceiptReturnHide.Value = "1";
                        }

                        AST_CODE.Value = ApplicationType.CRT;
                        AST_DOC_MODE.Value = GetDOCMODE();

                        #endregion
                    }
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
            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;
            BankMstService bankMstServiceClient;
            bankMstServiceClient = null;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DateTime receiptDate;
            FinTrxService finTrxServiceClient;
            finTrxServiceClient = null;
            string xmlDoc = string.Empty;
            CommonService CommonServiceClient;
            CommonServiceClient = null;
            SalesInvoiceService salesInvoiceServiceClient;
            salesInvoiceServiceClient = null;
            AdmCompanyMstService admCompanyMstServiceClient;
            int TotalRecords = 0;
            try
            {
                switch (type)
                {
                    #region PEDING INV LIST
                    case ControlsEnum.PEDINGINVLIST:
                        custPK = 0;
                        int.TryParse(hdfCustomerPk.Value, out custPK);
                        int.TryParse(hdfPendingInvPk.Value, out invPK);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.CurrentPage = 0;
                        serviceUtilityObj.PageSize = 0;
                        DateTime? fromDate = string.IsNullOrEmpty(txtPendingFromDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingFromDate.Text.Trim());
                        DateTime? todate = string.IsNullOrEmpty(txtPendingToDate.Text.Trim()) ? (DateTime?)null : Convert.ToDateTime(txtPendingToDate.Text.Trim());
                        if (custPK > 0)
                            dtPendingInvList = BusinessLogic.Sales.SalesInvoiceBL.GetPendingInvoiceList(CurrPK, custPK, invPK, Convert.ToInt32(ddlPendingInvCategory.SelectedValue), Convert.ToInt32(ddlPendingInvType.SelectedValue), fromDate, todate, currentUser.SBUID, serviceUtilityObj.CurrentPage, serviceUtilityObj.PageSize);
                        break;
                    #endregion
                    #region INVOICETYPE
                    case ControlsEnum.INVOICETYPE:
                        dtInvoiceType = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SALES INVOICE TYPE", "Regular");
                        break;
                    #endregion
                    #region INV CATEGORY
                    case ControlsEnum.INVCATEGORY:
                        dtInvCategory = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, "SALES INVOICE LIST TYPE");
                        break;
                    #endregion
                    #region CUSTOMER DETAILS
                    case ControlsEnum.CUSTOMERDETAILS:
                        custPK = 0;
                        int.TryParse(hdfCustomerPk.Value, out custPK);
                        dsCustomerData = BusinessLogic.Sales.CustomerProduct.GetCustomer(custPK, string.Empty, currentUser.SBUID, (int)DbActiveStatus.HASPK);
                        break;
                    #endregion
                    #region RECEIPT HEADER
                    case ControlsEnum.RECEIPTHEADER:
                        xmlDoc = string.Empty;
                        if (InvListObj != null && InvListObj.InvList != null && InvListObj.InvList.Count > 0)
                        {
                            xmlDoc = CommonFunctions.XmlSerialize<InvoiceBO>(InvListObj);
                        }

                        receiptHeaderObj = BusinessLogic.Sales.SalesInvoiceBL.GetTradingSalesReceiptHeaderMUL(xmlDoc, !string.IsNullOrEmpty(xmlDoc) ? 0 : CurrPK);
                        if (ReceiptHeaderSession == null || ReceiptHeaderSession.ReceiptTrxMapping == null || ReceiptHeaderSession.ReceiptTrxMapping.Count == 0)
                            ReceiptHeaderSession = receiptHeaderObj.DeepClone();
                        else if (receiptHeaderObj != null)
                        {
                            List<string> objInvList = ReceiptHeaderSession.ReceiptTrxMapping.Select(r => r.RCM_INVOICE_HDR.ToString()).Distinct().ToList();
                            ReceiptHeaderSession.ReceiptTrxMapping.AddRange(receiptHeaderObj.ReceiptTrxMapping.Where(r => !objInvList.Contains(r.RCM_INVOICE_HDR.ToString())).ToList());
                        }

                        #region Setting serial numbers
                        tempReceiptHeader = ReceiptHeaderSession;
                        if (tempReceiptHeader != null)
                        {
                            int rcm_sl_no = 1;
                            tempReceiptHeader.ReceiptTrxMapping.ForEach(dtl =>
                                {
                                    dtl.RCM_SL_NO = rcm_sl_no;
                                    int rso_sl_no = 1;
                                    if (dtl.ReceiptSOMapping != null && dtl.ReceiptSOMapping.Count > 0)
                                        dtl.ReceiptSOMapping.ForEach(sompg =>
                                        {
                                            sompg.RSO_RCM_SL_NO = rcm_sl_no;
                                            sompg.RSO_SL_NO = rso_sl_no;
                                            rso_sl_no++;
                                        });
                                    if (dtl.DebitNoteAllocation != null && dtl.DebitNoteAllocation.Count > 0)
                                        dtl.DebitNoteAllocation.ForEach(dn =>
                                        {
                                            dn.RNM_RCM_SL_NO = rcm_sl_no;
                                        });
                                    if (dtl.AdjAllocation != null && dtl.AdjAllocation.Count > 0)
                                        dtl.AdjAllocation.ForEach(adj =>
                                        {
                                            adj.RAD_RCM_SL_NO = rcm_sl_no;
                                        });
                                    if (dtl.ReceiptTaxDetails != null && dtl.ReceiptTaxDetails.Count > 0)
                                        dtl.ReceiptTaxDetails.ForEach(tax =>
                                        {
                                            tax.RDT_RCM_SL_NO = rcm_sl_no;
                                        });
                                    rcm_sl_no++;
                                });

                        }
                        #endregion

                        break;
                    #endregion
                    #region RECEIPT CUS ADJN
                    case ControlsEnum.RECEIPTCUSADJN:
                        dtPendingCrAdjn = BusinessLogic.Sales.SalesInvoiceBL.GetCustomerAdjAllocation(Convert.ToInt32(hdfCustomerPk.Value), CurrPK);
                        if (ReceiptAdjnList == null || ReceiptAdjnList.Count == 0)
                        {
                            tempReceiptAdjnList = new List<AdjAllocation>();
                            for (int i = 0; i < dtPendingCrAdjn.Rows.Count; i++)
                            {
                                AdjAllocation receiptTrxAdjnObj = new AdjAllocation();
                                receiptTrxAdjnObj.RAD_AMOUNT = Convert.ToDecimal(dtPendingCrAdjn.Rows[i]["RAD_AMOUNT"]);
                                receiptTrxAdjnObj.RAD_ALCN_CDH = Convert.ToInt64(dtPendingCrAdjn.Rows[i]["RAD_ALCN_CDH"]);
                                receiptTrxAdjnObj.RAD_NO = Convert.ToString(dtPendingCrAdjn.Rows[i]["RAD_NO"]);
                                receiptTrxAdjnObj.RAD_ALCN_RECEIPT_TRX = Convert.ToInt64(dtPendingCrAdjn.Rows[i]["RAD_ALCN_RECEIPT_TRX"]);
                                receiptTrxAdjnObj.RAD_DATE = Convert.ToDateTime(dtPendingCrAdjn.Rows[i]["RAD_DATE"]);
                                receiptTrxAdjnObj.RAD_TYPE = Convert.ToString(dtPendingCrAdjn.Rows[i]["RAD_TYPE"]);
                                receiptTrxAdjnObj.RAD_AMOUNT_BAL = Convert.ToDecimal(dtPendingCrAdjn.Rows[i]["RAD_AMOUNT_BAL"]);
                                receiptTrxAdjnObj.RAD_AMOUNT_RCVD = Convert.ToDecimal(dtPendingCrAdjn.Rows[i]["RAD_AMOUNT_RCVD"]);
                                receiptTrxAdjnObj.RAD_AMOUNT_PREV_RCVD = Convert.ToDecimal(dtPendingCrAdjn.Rows[i]["RAD_AMOUNT_RCVD"]);
                                receiptTrxAdjnObj.RAD_AMOUNT_TOTAL = Convert.ToDecimal(dtPendingCrAdjn.Rows[i]["RAD_AMOUNT_TOTAL"]);
                                tempReceiptAdjnList.Add(receiptTrxAdjnObj);
                            }
                            ReceiptAdjnList = tempReceiptAdjnList;
                        }
                        break;
                    #endregion
                    #region RECEIPT HDR LIST
                    case ControlsEnum.RECEIPTHDRLIST:
                        TotalPages = 0;
                        TotalRecords = 0;
                        int CustomerPk = string.IsNullOrEmpty(hdfCustomerSearchPk.Value) ? 0 : Convert.ToInt32(hdfCustomerSearchPk.Value);
                        if (hdfCustomerSearchPk.Value != null && hdfCustomerSearchPk.Value != "0" && hdfCustomerSearchPk.Value != "")
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerSearchPk.Value;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomerSearch.Text;
                        }
                        long ReceiptPk = string.IsNullOrEmpty(hdfReceiptPK.Value) ? 0 : Convert.ToInt64(hdfReceiptPK.Value);
                        string invNo = string.Empty;
                        invNo = string.IsNullOrEmpty(txtSINo.Text) ? string.Empty : txtSINo.Text.Trim();
                        dsPageData = BusinessLogic.Sales.ReceiptBL.GetReceiptList(
                            new BusinessObject.GridPrams()
                            {
                                SortBy = SortBy = SortBy == null ? Resources.DataFieldRes.ReceiptDate : SortBy,
                                SortDirection = string.IsNullOrEmpty(SortDirection) ? Resources.Report.SortDescending : SortDirection,
                                FromDate = string.IsNullOrEmpty(txtSearchDateFrom.Text.Trim()) ? string.Empty : txtSearchDateFrom.Text.Trim(),
                                ToDate = string.IsNullOrEmpty(txtSearchDateTo.Text.Trim()) ? string.Empty : txtSearchDateTo.Text.Trim(),
                                SearchBy = "RCH_NO",
                                SearchValue = string.IsNullOrEmpty(txtReceiptNumber.Text.Trim()) ? string.Empty : (txtReceiptNumber.Text.Trim() == "Select/Type" ? string.Empty : txtReceiptNumber.Text.Trim()),
                                PageNumber = uclPaging.CurrentPage == 0 ? 1 : uclPaging.CurrentPage,
                                PageSize = Convert.ToInt32(GetLocalResourceObject("PageSize"))
                            }, currentUser, CustomerPk, ReceiptPk, invNo, Resources.PageURL.ReceiptTrading.Replace("~", ""), Convert.ToInt32(ddlStatus.SelectedValue), Convert.ToInt32(ddlPDCStatus.SelectedValue));

                        if (dsPageData != null && dsPageData.Tables.Count > 0)
                        {
                            DataView dvReceipt = dsPageData.Tables[1].DefaultView;
                            dtReceiptList = dvReceipt.ToTable();
                            int pagsize = Convert.ToInt32(GetLocalResourceObject("PageSize"));
                            TotalRecords = dsPageData.Tables[0].Rows.Count > 0 ? Convert.ToInt32(dsPageData.Tables[0].Rows[0][0].ToString()) : 0;
                            TotalPages = TotalRecords == 0 ? 1 : (TotalRecords <= pagsize) ? 1 :
                                        (TotalRecords % pagsize) == 0 ? (TotalRecords / pagsize) :
                                        (TotalRecords / pagsize) + 1;
                        }
                        break;
                    #endregion
                    #region Invoice Hdr List
                    case ControlsEnum.ISADVDEDUCTED:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        IsContReturn = salesReceiptServiceClient.IsReceiptAdvDeducted((int)CurrPK);

                        break;
                    #endregion
                    #region Cash Bank List
                    case ControlsEnum.BANK:
                        bankMstServiceClient = new BankMstService();
                        bankMstServiceClient = CommonFunctions.InitiateClient(bankMstServiceClient);
                        short bankPk = string.IsNullOrEmpty(hdfBank.Value) ? Convert.ToInt16(0) : Convert.ToInt16(hdfBank.Value);
                        finCashBankMstList = bankMstServiceClient.GetFinBankMstByPK(bankPk);
                        break;
                    #endregion
                    #region CUSTOMERBANK
                    case ControlsEnum.CUSTOMERBANK:
                        crmCustomerMstList = new List<FIN_CASH_BANK_MST>();
                        CommonService cm = new CommonService();
                        int CusPk = hdfCustomerPk.Value != string.Empty ? Convert.ToInt32(hdfCustomerPk.Value) : 0;
                        int? BankPk = null;
                        if (CurrPK > 0)
                        {
                            BankPk = hdfSavedBankPk.Value != string.Empty ? Convert.ToInt32(hdfSavedBankPk.Value) : (int?)null;
                        }
                        if (CusPk > 0)
                        {
                            crmCustomerMstList = cm.GetCusBankDT(CusPk, BankPk);

                        }
                        break;
                    #endregion
                    #region Receipt Hdr List
                    case ControlsEnum.RECEIPTHDRENTRYBYPK:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        finReceiptCusHdrList = salesReceiptServiceClient.GetReceiptHdr(CurrPK);
                        break;
                    #endregion
                    #region Generate Receipt No
                    case ControlsEnum.RECEIPTNO:
                        //Generate Receipt No
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        hdfReceiptNo.Value = salesReceiptServiceClient.GetReceiptNo(ApplicationType.CR, 0, 1,
                            string.IsNullOrEmpty(txtReceiptDate.Text) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text), currentUser.PKUser, updateReceipt, 0, Convert.ToInt32(ddlCompany.SelectedValue));
                        break;
                    #endregion
                    #region Get Exchange Rate
                    case ControlsEnum.EXCHANGERATE:
                        //Get Exchange Rate
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        int toCurrency = string.IsNullOrEmpty(hdfReceiptCurrency.Value) ? currentUser.BaseCurrency : Convert.ToInt32(hdfReceiptCurrency.Value);
                        receiptDate = string.IsNullOrEmpty(txtReceiptDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
                        hdfExchangeCurr.Value = salesReceiptServiceClient.GetConversionFactor(
                                                             Convert.ToInt32(hdfInvoiceCurr.Value), toCurrency,
                                                             receiptDate, SBUID).ToString();
                        break;
                    #endregion
                    #region Get Exchange Rate in Base Currency
                    case ControlsEnum.EXCHANGERATEINBASECURRENCY:
                        //Get Exchange Rate in Base Currency
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        int fromCurrency = string.IsNullOrEmpty(hdfReceiptCurrency.Value) ? currentUser.BaseCurrency : Convert.ToInt32(hdfReceiptCurrency.Value);
                        receiptDate = string.IsNullOrEmpty(txtReceiptDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
                        hdfExchangeCurrBC.Value = salesReceiptServiceClient.GetConversionFactor(
                                                             fromCurrency, currentUser.BaseCurrency,
                                                             receiptDate, SBUID).ToString();
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
                    #region FIN HEADER STATUS
                    case ControlsEnum.FINHEADERSTATUS:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        if (recipetType == 0 || recipetType == 1)
                        {
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.CRTJ;
                        }
                        else if (recipetType == 3)
                        {
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.MSIRTJ;
                        }
                        finTrxHdrObj.FTH_REF_PK = CurrPK;
                        finTrxHdrObj.FTH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finTrxHdrList = finTrxServiceClient.GetSatusByAppPK(finTrxHdrObj);
                        break;
                    #endregion
                    #region FIN HEADER STATUS REVERSE
                    case ControlsEnum.FINHEADERSTATUSREVERSE:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = CommonFunctions.InitiateClient(finTrxServiceClient);
                        serviceUtilityObj = new ServiceUtility();
                        serviceUtilityObj.PageSize = 10;
                        finTrxHdrObj = ERP.Utilities.CommonFunctions.Initilize<FIN_TRX_HDR>();
                        if (recipetTypePDC == 4)
                        {
                            finTrxHdrObj.FTH_REF_TYPE = ApplicationType.PDCCTJ;
                            finTrxHdrObj.FTH_REF_PK = CurrPK;
                        }
                        finTrxHdrPDCList = finTrxServiceClient.GetSatusByAppPK(finTrxHdrObj);
                        break;
                    #endregion
                    #region INVOICEVNDMPGLIST
                    case ControlsEnum.INVOICEVNDMPGLIST:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        FinInvoiceCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_TRX_MPG>();
                        FinInvoiceCusTrxMpgObj.ICM_INVOICE_HDR = InvoicePK;
                        FinInvoiceCusTrxMpgList = salesReceiptServiceClient.GetInvoiceTrxMpg(FinInvoiceCusTrxMpgObj);
                        break;
                    #endregion
                    #region INVOICEVNDMPGLISTFORAUTOALCN
                    case ControlsEnum.INVOICEVNDMPGLISTFORAUTOALCN:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        FinInvoiceCusTrxMpgObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_TRX_MPG>();
                        FinInvoiceCusTrxMpgObj.ICM_INVOICE_HDR = InvoicePK;
                        FinInvoiceCusTrxMpgListForAutoAlcn = salesReceiptServiceClient.GetInvoiceTrxMpg(FinInvoiceCusTrxMpgObj);
                        break;
                    #endregion
                    #region INVOICEVNDHDR
                    case ControlsEnum.INVOICEVNDHDR:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        finInvoiceVndHdrObjForPaymentSplit = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        finInvoiceVndHdrObjForPaymentSplit.ICH_PK = InvoicePK;
                        finInvoiceVndHdrObjForPaymentSplit.ICH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                        finInvoiceVndHdrListForPaymentSplit = salesReceiptServiceClient.GetInvoiceCusHdrByPK(finInvoiceVndHdrObjForPaymentSplit);
                        break;
                    #endregion
                    #region Payment Mode
                    case ControlsEnum.PAYMODE:
                        dtPaymentModes = BusinessLogic.CommonManagement.CommonBL.GetAppConfig(currentUser.SBUID, GetLocalResourceObject("PAYMENT_MODE").ToString());
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        finTrxServiceClient = new FinTrxService();
                        finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                        finYearMstList = finTrxServiceClient.GetCurrentFinPeriod(DateTime.Now, currentUser.SBUID);
                        break;
                    #endregion
                    #region GETRECEIPTPKBYJOURNALPK
                    case ControlsEnum.GETRECEIPTPKBYJOURNALPK:
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
                        workflowStatusList = CommonServiceClient.GetWorkFlowStatus(ApplicationType.CR, null, Convert.ToByte(CommonConstants.ACTIVE));
                        break;
                    #endregion
                    #region ADJTYPE
                    case ControlsEnum.DISCOUNTTYPE:
                        dtDiscountTypes = BusinessLogic.CommonManagement.CommonBL.GetConstMstValues(0, 0, ConstGroupType.ReceiptAdjustments, (int)Adjustments.Receipt, 1, currentUser.SBUID);
                        break;
                    #endregion
                    #region BANKCURRENCY
                    case ControlsEnum.BANKCURRENCY:
                        CommonServiceClient = new CommonService();
                        CommonServiceClient = CommonFunctions.InitiateClient(CommonServiceClient);
                        admCurrencyMstObj = new ADM_CURRENCY_MST();
                        admCurrencyMstObj.CUR_PK = currentUser.BaseCurrency;
                        admCurrencyMstObj.CUR_ACTIVE = Convert.ToByte(CommonConstants.ACTIVE);
                        admCurrencyMstList = CommonServiceClient.GetCurrency(admCurrencyMstObj);
                        break;
                    #endregion
                    #region Get Exchange Rate for Bank Charge
                    case ControlsEnum.EXCHANGERATEBANK:
                        //Get Exchange Rate in Base Currency
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        int fromCurrencyBank = string.IsNullOrEmpty(hdfReceiptCurrency.Value) ? currentUser.BaseCurrency : Convert.ToInt32(hdfReceiptCurrency.Value);
                        receiptDate = string.IsNullOrEmpty(txtReceiptDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
                        hdfExchRate.Value = salesReceiptServiceClient.GetConversionFactor(
                                                             fromCurrencyBank, currentUser.BaseCurrency,
                                                             receiptDate, SBUID).ToString();
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
                    #region CHECKINSTRNO
                    case ControlsEnum.CHKINSTRNO:
                        //Check Instr No
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        string instrNo = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                        InstrNoResult = Convert.ToBoolean(salesReceiptServiceClient.GetReceiptInstrNo(instrNo, CurrPK));
                        break;
                    #endregion
                    #region RECEIVEDAMOUTSPLIT
                    case ControlsEnum.RECEIVEDAMTSPLITUP:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        dtReceivedAmt = salesReceiptServiceClient.GetReceiptAmtSplitUp(InvoicePK, CurrPK, Resources.ErpRes.Draft);
                        break;
                    #endregion
                    #region CR/DR MPG LIST
                    case ControlsEnum.CRDRMPGLIST:
                        salesReceiptServiceClient = new SalesReceiptService();
                        salesReceiptServiceClient = CommonFunctions.InitiateClient(salesReceiptServiceClient);
                        FinCrdrMpgList = salesReceiptServiceClient.GetCrDrMpgList(InvoicePK);
                        break;

                    #endregion
                    #region Invoice Hdr By PK
                    case ControlsEnum.INVOICEHDRBYPK:
                        salesInvoiceServiceClient = new SalesInvoiceService();
                        salesInvoiceServiceClient = CommonFunctions.InitiateClient(salesInvoiceServiceClient);
                        finInvoiceCusHdrObj = CommonFunctions.Initilize<FIN_INVOICE_CUS_HDR>();
                        finInvoiceCusHdrObj.ICH_PK = ICH_PK;
                        ObjFinInvoiceCusHdrList = salesInvoiceServiceClient.GetInvoiceCusHdrByPKOnly(finInvoiceCusHdrObj);
                        break;
                    #endregion
                    #region CheckReceiptAllocationInvoice
                    case ControlsEnum.CheckReceiptAllocation:
                        dtReceiptAlloc = BusinessLogic.Sales.SaleOrderBL.CheckReceiptAllocationInvoice(CurrPK);
                        break;
                    #endregion
                    #region SUSPENCE LIST
                    case ControlsEnum.SUSPENCELIST:
                        dtSuspenceList = BusinessLogic.Sales.SaleOrderBL.GetSuspenceListforReciept(Convert.ToInt32(hdfBank.Value), CurrPK);
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
                salesReceiptServiceClient = null;
                bankMstServiceClient = null;
                finTrxServiceClient = null;
                CommonServiceClient = null;
                salesInvoiceServiceClient = null;
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
                    #region PEDING INV LIST
                    case ControlsEnum.PEDINGINVLIST:
                        BindGrid(ControlsEnum.PEDINGINVLIST);
                        break;
                    #endregion
                    #region INVOICE TYPE
                    case ControlsEnum.INVOICETYPE:
                        BindDropDown(ControlsEnum.INVOICETYPE);
                        break;
                    #endregion
                    #region INV CATEGORY
                    case ControlsEnum.INVCATEGORY:
                        BindDropDown(controlType);
                        break;
                    #endregion
                    #region CUSTOMER DETAILS
                    case ControlsEnum.CUSTOMERDETAILS:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region UPDATE GRID VAL TO OBJECT
                    case ControlsEnum.UPDATEGRIDVALTOOBJECT:
                        SetUIValuesToObject(controlType);
                        break;
                    #endregion
                    #region RECEIPT HEADER
                    case ControlsEnum.RECEIPTHEADER:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region RECEIPT MPG LIST
                    case ControlsEnum.RECEIPTMPGLIST:
                        BindGrid(ControlsEnum.RECEIPTMPGLIST);
                        break;
                    #endregion
                    #region RECEIPT CUS ADJN
                    case ControlsEnum.RECEIPTCUSADJN:
                        BindGrid(ControlsEnum.RECEIPTCUSADJN);
                        break;
                    #endregion
                    #region CR DR ALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        BindGrid(ControlsEnum.CRDRALLOCATION);
                        break;
                    #endregion
                    #region RECEIPT GET
                    case ControlsEnum.RECEIPTGET:
                        GetUIValuesFromObject(controlType);
                        break;
                    #endregion
                    #region COMPANY
                    case ControlsEnum.COMPANY:
                        BindDropDown(ControlsEnum.COMPANY);
                        break;
                    #endregion
                    #region RECEIPT HDR LIST
                    case ControlsEnum.RECEIPTHDRLIST:
                        BindGrid(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region PAYMENT SPLIT LIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        BindGrid(ControlsEnum.PAYMENTSPLITLIST);
                        break;
                    #endregion
                    #region Payment Mode
                    case ControlsEnum.PAYMODE:
                        BindDropDown(ControlsEnum.PAYMODE);
                        break;
                    #endregion
                    #region Fin Transaction Year
                    case ControlsEnum.FINPERIOD:
                        if (finYearMstList != null && finYearMstList.Count > 0)
                        {
                            txtSearchDateFrom.Text = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            hdfSearchDateFrom.Value = finYearMstList[0].FYR_DATE_FROM.ToString(Resources.Constants.DateFormatShort);
                            txtSearchDateTo.Text = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                            hdfSearchDateTo.Value = finYearMstList[0].FYR_DATE_TO.ToString(Resources.Constants.DateFormatShort);
                        }
                        break;
                    #endregion
                    #region DISCOUNT TYPE
                    case ControlsEnum.DISCOUNTTYPE:
                        BindDropDown(ControlsEnum.DISCOUNTTYPE);
                        break;
                    #endregion
                    #region BANK CURRENCY
                    case ControlsEnum.BANKCURRENCY:
                        if (admCurrencyMstList != null && admCurrencyMstList.Count > 0)
                        {
                            ddlBankChargeCurrency.Items.Insert(0, (new ListItem(admCurrencyMstList[0].CUR_CODE + " - " + admCurrencyMstList[0].CUR_NAME, admCurrencyMstList[0].CUR_PK.ToString())));
                        }
                        break;
                    #endregion
                    #region RECEIVEDAMTSPLITUP
                    case ControlsEnum.RECEIVEDAMTSPLITUP:
                        BindGrid(ControlsEnum.RECEIVEDAMTSPLITUP);
                        break;
                    #endregion
                    #region SUSPENCE LIST
                    case ControlsEnum.SUSPENCELIST:
                        BindGrid(ControlsEnum.SUSPENCELIST);
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
        /// Funtion used get doc mode
        /// </summary>
        private string GetDOCMODE()
        {
            CommonService cm = new CommonService();
            List<SPADM_APP_SUB_TYPE_DATA_GET_Result> AppTypeDetailsList = cm.GetReportParameters(AST_CODE.Value, 0, DateTime.Now);
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

            try
            {
                Object retObject;
                retObject = null;
                int rowID;
                currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
                HiddenField hdfReceiptMpgPK;
                hdfReceiptMpgPK = null;
                HiddenField hdfInvoicePK;
                hdfInvoicePK = null;
                TextBox txtAmount;
                Label lblTax;
                TextBox txtAdjustments;
                Label lblOtherchargesFooter;
                bool bIsChecked = false;
                HiddenField hdfReceiptSplitPK = null;
                HiddenField hdfSOPK = null;
                TextBox txtPayNowSplit;
                TextBox txtOtherChargesSplit;
                HiddenField hdfTaxSplit;
                HiddenField hdfOtherChargesSplit;
                DateTime? InstrDate = null;
                //decimal taxAmnt;
                //decimal adjAmnt;
                //decimal reductionAmnt;
                //decimal otherAmnt;
                //decimal receiveAmnt;

                switch (controlType)
                {
                    #region Receipt Hdr
                    case ControlsEnum.RECEIPTHEADER:
                        if (ReceiptHeaderSession != null)
                        {
                            receiptHeaderObj = new ReceiptHeader();
                            receiptHeaderObj = ReceiptHeaderSession;
                            receiptHeaderObj.RCH_PK = CurrPK;
                            receiptHeaderObj.RCH_NO = (string.IsNullOrEmpty(lblReceiptNo.Text) || lblReceiptNo.Text.Trim().Equals(Resources.ErpRes.Draft)) ? string.Empty
                                                        : lblReceiptNo.Text.Trim();
                            receiptHeaderObj.RCH_DATE = String.IsNullOrEmpty(txtReceiptDate.Text.Trim()) ? DateTime.Now : Convert.ToDateTime(txtReceiptDate.Text.Trim());
                            receiptHeaderObj.RCH_CUSTOMER = string.IsNullOrEmpty(hdfCustomerPk.Value) ? 1 : Convert.ToInt32(hdfCustomerPk.Value);
                            receiptHeaderObj.RCH_CATEGORY = (byte)SICategory;
                            receiptHeaderObj.RCH_GROUP = (byte)SOGroup;
                            receiptHeaderObj.RCH_CUSTOMER_ACCOUNT = string.IsNullOrEmpty(hdfCustomerAccountNo.Value) ? null : hdfCustomerAccountNo.Value;
                            receiptHeaderObj.RCH_MODE = ddlMode.SelectedValue == "-1" ? Convert.ToByte(0) : Convert.ToByte(ddlMode.SelectedValue);
                            receiptHeaderObj.RCH_BANK = string.IsNullOrEmpty(hdfBank.Value) ? null : hdfBank.Value;
                            receiptHeaderObj.RCH_OTHER_AMOUNT = Convert.ToDecimal(hdfTotalOtherCharges.Value.Trim());
                            if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
                            {

                                receiptHeaderObj.RCH_INSTR_NO = HttpUtility.HtmlEncode(txtInstrumentNo.Text.Trim());
                                receiptHeaderObj.RCH_INSTR_DATE = String.IsNullOrEmpty(txtInstrumentDate.Text.Trim()) ? ((Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.OTHERS) ? InstrDate.ToString() : DateTime.Now.ToString()) : Convert.ToDateTime(txtInstrumentDate.Text.Trim()).ToString();
                            }
                            receiptHeaderObj.RCH_BANK_CASH_ACCOUNT = string.IsNullOrEmpty(hdfBankAccount.Value) ? null : hdfBankAccount.Value;
                            receiptHeaderObj.RCH_CURRENCY = string.IsNullOrEmpty(hdfReceiptCurrency.Value) ? 1 : Convert.ToInt32(hdfReceiptCurrency.Value);
                            receiptHeaderObj.RCH_RCVD_AMOUNT = Convert.ToDecimal(txtReceivedAmount.Text.Trim());
                            receiptHeaderObj.RCH_BANK_OF_CHEQUE = HttpUtility.HtmlEncode(txtbankOfCheque.Text.Trim());
                            receiptHeaderObj.RCH_REMARKS = HttpUtility.HtmlEncode(txtRemarks.Text.Trim());
                            receiptHeaderObj.RCH_BASE_CURR = currentUser.BaseCurrency;
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            GetFieldValues(ControlsEnum.EXCHANGERATEINBASECURRENCY);
                            receiptHeaderObj.RCH_EXCHG_RATE = string.IsNullOrEmpty(hdfExchangeCurrBC.Value) ? 1 : Convert.ToDouble(hdfExchangeCurrBC.Value);
                            receiptHeaderObj.RCH_RCVD_AMOUNT_BC = Convert.ToDecimal(txtReceivedAmount.Text.Trim()) * Convert.ToDecimal(receiptHeaderObj.RCH_EXCHG_RATE);
                            receiptHeaderObj.RCH_STATUS = WkfStatus;
                            receiptHeaderObj.RCH_DEL_STATUS = Convert.ToByte(hdfDelStatus.Value);
                            receiptHeaderObj.RCH_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);

                            #region For default split allocaation apply
                            foreach (ReceiptTrxMapping receiptmpg in receiptHeaderObj.ReceiptTrxMapping)
                            {
                                if (AppliedInvPkList == null || !AppliedInvPkList.Contains(Convert.ToInt64(receiptmpg.RCM_INVOICE_HDR)))
                                {
                                    InvoiceDetails(Convert.ToInt64(receiptmpg.RCM_INVOICE_HDR));
                                    ReceiptSplitSave(false);
                                }
                            }
                            #endregion

                            receiptHeaderObj.RCH_DEPT = Convert.ToInt16(currentUser.CurrentDeptPK);
                            receiptHeaderObj.RCH_BIZUNIT = Convert.ToInt16(currentUser.SBUID);
                            receiptHeaderObj.USER_PK = Convert.ToInt16(currentUser.PKUser);
                            receiptHeaderObj.LAST_MOD_DT = LastModifiedTime;
                            receiptHeaderObj.RCH_COMPANY = Convert.ToInt16(ddlCompany.SelectedValue);

                            if (ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                                receiptHeaderObj.RCH_DISCOUNT = null;
                            else
                                receiptHeaderObj.RCH_DISCOUNT = ddlAdjType.SelectedValue;
                            if (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.CHEQUE)
                            {
                                receiptHeaderObj.RCH_PDC = chkPDC.Checked == true ? (byte)1 : (byte)0;
                            }
                            else
                            {
                                receiptHeaderObj.RCH_PDC = 0;
                            }
                            receiptHeaderObj.RCH_DISC_AMOUNT = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                            receiptHeaderObj.RCH_TAX_AMOUNT = hdfTottaxHDR.Value != string.Empty ? Convert.ToDecimal(hdfTottaxHDR.Value) : 0;
                            if (ddlBankChargeCurrency.Items.Count > 0)
                                receiptHeaderObj.RCH_BANK_CHARGE_CURR = ddlBankChargeCurrency.SelectedValue;
                            else
                                receiptHeaderObj.RCH_BANK_CHARGE_CURR = null;
                            receiptHeaderObj.RCH_BANK_CHARGE = txtBankCharge.Text != string.Empty ? Convert.ToDecimal(txtBankCharge.Text) : 0;

                            //****************Return Region*****************
                            receiptHeaderObj.RCH_RETURN_STATUS = Convert.ToByte(chkReturn.Checked);
                            receiptHeaderObj.RCH_RETURN_DATE = String.IsNullOrEmpty(txtReturnDate.Text.Trim()) ? DateTime.Now.ToString() : Convert.ToDateTime(txtReturnDate.Text.Trim()).ToString();
                            receiptHeaderObj.RCH_RETURN_REMARKS = HttpUtility.HtmlEncode(txtReturnRemarks.Text.Trim());
                            //**********End return*******************************
                            receiptHeaderObj.APT_CODE = ApplicationType.CRT;
                            receiptHeaderObj.AST_DOC_MODE = AST_DOC_MODE.Value == "1" ? 1 : 0;
                            #region Setting Tax Slno
                            receiptHeaderObj.ReceiptTrxMapping.ForEach(dtl =>
                            {
                                if (dtl.AdjAllocation != null && dtl.AdjAllocation.Count > 0)
                                {
                                    dtl.AdjAllocation = dtl.AdjAllocation.Where(f => f.RAD_AMOUNT > 0).ToList();//Should not save allocation details which have zero amount
                                    dtl.AdjAllocation.ForEach(f => f.RAD_RCM_SL_NO = dtl.RCM_SL_NO);
                                }
                                #region Calculate Receipt Tax
                                //In the case of Advance Invoice we need to pass RDT_RSO_SL_NO (For saving RDT_CUS_SO_MPG in FIN_RECEIPT_CUS_TAX_DTL table)
                                if (dtl.ReceiptSOMapping != null && dtl.ReceiptSOMapping.Count > 0)
                                {
                                    foreach (ReceiptSOMapping item in dtl.ReceiptSOMapping)
                                    {
                                        if (SICategory == SalesInvoiceCategory.Advanced)
                                        {
                                            dtl.ReceiptTaxDetails.ForEach(f => f.RDT_RSO_SL_NO = item.RSO_SL_NO);//For saving RDT_CUS_SO_MPG in FIN_RECEIPT_CUS_TAX_DTL table                                          
                                        }
                                        CalculateReceiptTax(dtl, Convert.ToInt32(item.RSO_SO_HDR), item.RSO_TAX_AMOUNT, (byte)SICategory, dtl.RCM_INVOICE_HDR, item.RSO_RECEIVED_AMOUNT);
                                    }
                                }
                                else //In the case of Miscellaneous invoice (there is no SO)
                                {
                                    CalculateReceiptTax(dtl, 0, dtl.ICH_TAX_TC, (byte)SICategory, dtl.RCM_INVOICE_HDR, dtl.ICH_DISCOUNT_TOTAL);
                                }

                                #endregion
                            });

                            #endregion

                            retObject = receiptHeaderObj;
                        }
                        break;
                    #endregion
                    #region JOURNALIZE/REVERSE/CHEQUERETURN
                    case ControlsEnum.REVERSE:
                    case ControlsEnum.JOURNALIZE:
                    case ControlsEnum.CHEQUERETURN:
                        if (ReceiptHeaderSession != null)
                        {
                            if (ReceiptHeaderSession.RCH_STATUS == 2)
                            {
                                if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping.Count() > 0)
                                {
                                    if (ReceiptHeaderSession.RCH_RCVD_AMOUNT == 0)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Journalize_Zero_Amnt").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                        return null;
                                    }

                                    Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = hdfCustomerPk.Value;
                                    Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = txtCustomer.Text;

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

                                    if (controlType == ControlsEnum.JOURNALIZE)
                                    {

                                        Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = SOGroup == SalesInvoiceGroup.Miscellaneous ? ApplicationType.MSIRTJ
                                         : ApplicationType.CRTJ;

                                        ucrJournalize.JournalType = (int)JournalTypeEnum.Voucher;
                                    }
                                    else if (controlType == ControlsEnum.REVERSE)
                                    {
                                        Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.PDCCTJ;
                                        ucrJournalize.JournalType = (int)JournalTypeEnum.Reverse;
                                    }
                                    else if (controlType == ControlsEnum.CHEQUERETURN)
                                    {
                                        Session[ERP.Utilities.SessionStrings.TransactionType] = ucrJournalize.TransactionType = ApplicationType.RCBTJ;
                                        ucrJournalize.JournalType = (int)JournalTypeEnum.Return;
                                    }
                                    ucrJournalize.TransactionPK = (int)CurrPK;
                                    Session[ERP.Utilities.SessionStrings.TransactionPK] = CurrPK;
                                    ucrJournalize.JournalizePK = 0;
                                    Session[ERP.Utilities.SessionStrings.JournalizePK] = null;
                                    GetFieldValues(ControlsEnum.RECEIPTHDRENTRYBYPK);
                                    Session[ERP.Utilities.SessionStrings.TransactionNo] = ReceiptHeaderSession.RCH_NO;
                                    Session[ERP.Utilities.SessionStrings.TransactionDate] = ReceiptHeaderSession.RCH_DATE;
                                    Session[ERP.Utilities.SessionStrings.TransactionCurrency] = ReceiptHeaderSession.RCH_CURRENCY;
                                    Session[ERP.Utilities.SessionStrings.AccountPayable] = ApplicationType.CRT;
                                    Session[ERP.Utilities.SessionStrings.AccountPayablePK] = ReceiptHeaderSession.RCH_CUSTOMER;
                                    Session[ERP.Utilities.SessionStrings.JournalType] = SOGroup == SalesInvoiceGroup.Miscellaneous ? ApplicationType.MSIRTJ
                                          : ApplicationType.CRTJ;

                                    ucrWrkf.WrkfSubmit -= ActionHandler;
                                    ucrWrkf.Reset();

                                    ucrWrkf.ViewType = 1;
                                    if (controlType == ControlsEnum.JOURNALIZE)
                                    {
                                        FillProcessID(2);
                                    }
                                    else if (controlType == ControlsEnum.REVERSE)
                                    {
                                        //if (EntryStatus == EntryStatus.LISTMODE)
                                        //    FillProcessID(1);
                                        //else
                                        FillProcessID(4);
                                    }
                                    else if (controlType == ControlsEnum.CHEQUERETURN)
                                    {
                                        //if (EntryStatus == EntryStatus.LISTMODE)
                                        //    FillProcessID(1);
                                        //else
                                        FillProcessID(6);
                                    }
                                    GetFieldValues(ControlsEnum.FINHEADER);
                                    //EntryStatus = EntryStatus.ENTRYMODE;
                                    WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                                    if (finTrxHdrList != null && finTrxHdrList.Count > 0)
                                    {
                                        ucrWrkf.RefID = workflowCore.GetRefID((int)finTrxHdrList[0].FTH_PK, ucrWrkf.ProcessID);
                                        base.WkfRefID = ucrWrkf.RefID;
                                    }
                                    SetCancelRef((int)CurrPK);
                                    ucrWrkf.FillWorkFlowDetails();
                                    if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.LISTMODE) && ucrWrkf.HasPageTaskPermission)
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
                                    Session[ERP.Utilities.SessionStrings.JournalMode] = (EntryStatus == EntryStatus.ENTRYMODE || EntryStatus == EntryStatus.LISTMODE) ? EntryStatus.ENTRYMODE : EntryStatus.VIEWMODE;
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
                                    if (controlType == ControlsEnum.JOURNALIZE)
                                    {
                                        Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalName.Value = GetLocalResourceObject("Customer_Receipt_Journal").ToString();
                                    }
                                    else if (controlType == ControlsEnum.REVERSE)
                                    {
                                        Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalName.Value = GetLocalResourceObject("Receipt_PDC_Voucher").ToString();
                                    }
                                    else if (controlType == ControlsEnum.CHEQUERETURN)
                                    {
                                        Session[ERP.Utilities.SessionStrings.JournalHead] = hdfJournalName.Value = GetLocalResourceObject("Receipt_Return_Voucher").ToString();
                                    }
                                    //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "','1000','550');", true);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowCommonCotainerDiv('[id$=divJournalize]','" + Session[ERP.Utilities.SessionStrings.JournalHead].ToString() + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Journalize_Msg").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                        }
                        else
                        {
                            if (EntryStatus == EntryStatus.NEWMODE)
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Journalize_Msg").ToString();
                            else
                                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;

                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                        }
                        break;
                    #endregion
                    #region Payment Split
                    case ControlsEnum.PAYMENTSPLITLIST:
                        //rowID = 0;
                        finReceiptCusSoMpgList = new List<ReceiptSOMapping>();
                        tempReceiptHeader = ReceiptHeaderSession;
                        if (tempReceiptHeader != null && tempReceiptHeader.ReceiptTrxMapping != null)
                        {
                            ReceiptTrxMapping objTrxMpg = tempReceiptHeader.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK);
                            if (objTrxMpg != null && objTrxMpg.ReceiptSOMapping != null && objTrxMpg.ReceiptSOMapping.Count > 0)
                            {
                                foreach (GridViewRow grdrow in grdReceiptSplit.Rows)
                                {
                                    hdfSOPK = (HiddenField)grdrow.FindControl("hdfSOPK");
                                    ReceiptSOMapping objSoMpg = objTrxMpg.ReceiptSOMapping.SingleOrDefault(r => r.RSO_SO_HDR == Convert.ToInt32(hdfSOPK.Value));
                                    hdfTaxSplit = (HiddenField)grdrow.FindControl("hdfTaxSplit");
                                    hdfOtherChargesSplit = (HiddenField)grdrow.FindControl("hdfOtherChargesSplit");
                                    txtOtherChargesSplit = (TextBox)grdrow.FindControl("txtOtherChargesSplit");
                                    hdfReceiptSplitPK = (HiddenField)grdrow.FindControl("hdfReceiptSplitPK");
                                    objSoMpg.RSO_PK = hdfReceiptSplitPK == null ? 0 : Convert.ToInt64(hdfReceiptSplitPK.Value);
                                    objSoMpg.RSO_RECEIPT_HDR = CurrPK;
                                    objSoMpg.RSO_RECEIPT_TRX_MPG = ReceiptMpgPK;

                                    txtPayNowSplit = (TextBox)grdrow.FindControl("txtPayNowSplit");
                                    objSoMpg.RSO_RECEIVED_AMOUNT = txtPayNowSplit == null ? 0 : txtPayNowSplit.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtPayNowSplit.Text.Trim());
                                    objSoMpg.RSO_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                                    objSoMpg.RSO_TAX_AMOUNT = hdfTaxSplit == null ? 0 : hdfTaxSplit.Value.Trim() == string.Empty ? 0 : Convert.ToDecimal(hdfTaxSplit.Value.Trim());
                                    objSoMpg.RSO_OTHER_AMOUNT = hdfOtherChargesSplit.Value == null ? 0 : hdfOtherChargesSplit.Value.Trim() == string.Empty ? 0 : Convert.ToDecimal(hdfOtherChargesSplit.Value.Trim());
                                    finReceiptCusSoMpgList.Add(objSoMpg);
                                }
                            }
                        }
                        ReceiptHeaderSession = tempReceiptHeader;
                        retObject = finReceiptCusSoMpgList;
                        break;
                    #endregion
                    #region ADJN SPLIT LIST
                    case ControlsEnum.ADJNSPLITLIST:
                        //rowID = 0;

                        HiddenField hdfCrDrPK;
                        HiddenField hdfReceiptAdjnPK;
                        HiddenField hdfAdjnPK;
                        HiddenField hdfInvPK;
                        Label lblBalanceAdjn;
                        Label lblAdjNo;
                        TextBox txtAllocateAdjn;

                        FinReceiptCusAllocationList = new List<AdjAllocation>();
                        foreach (GridViewRow grdrow in grdReceiptSplitAdjn.Rows)//
                        {
                            hdfReceiptMpgPK = (HiddenField)grdrow.FindControl("hdfReceiptMpgPK");
                            hdfCrDrPK = (HiddenField)grdrow.FindControl("hdfCrDrPK");
                            hdfReceiptAdjnPK = (HiddenField)grdrow.FindControl("hdfReceiptAdjnPK");
                            hdfAdjnPK = (HiddenField)grdrow.FindControl("hdfAdjnPK");
                            hdfInvPK = (HiddenField)grdrow.FindControl("hdfInvPK");
                            txtAllocateAdjn = (TextBox)grdrow.FindControl("txtAllocateAdjn");
                            lblBalanceAdjn = (Label)grdrow.FindControl("lblBalanceAdjn");
                            lblAdjNo = (Label)grdrow.FindControl("lblAdjNo");
                            FinReceiptCusAllocationObj = new AdjAllocation();
                            //hdfReceiptSplitPK = (HiddenField)grdReceiptSplit.Rows[rowID].FindControl("hdfReceiptSplitPK");
                            FinReceiptCusAllocationObj.RAD_PK = string.IsNullOrEmpty(hdfAdjnPK.Value) ? 0 : Convert.ToInt64(hdfAdjnPK.Value);
                            FinReceiptCusAllocationObj.RAD_RECEIPT_TRX = string.IsNullOrEmpty(hdfReceiptMpgPK.Value) ? 0 : Convert.ToInt32(hdfReceiptMpgPK.Value);
                            FinReceiptCusAllocationObj.RAD_ALCN_CDH = string.IsNullOrEmpty(hdfCrDrPK.Value) ? 0 : Convert.ToInt32(hdfCrDrPK.Value);
                            FinReceiptCusAllocationObj.RAD_ALCN_RECEIPT_TRX = string.IsNullOrEmpty(hdfReceiptAdjnPK.Value) ? 0 : Convert.ToInt32(hdfReceiptAdjnPK.Value);
                            FinReceiptCusAllocationObj.RAD_AMOUNT = txtAllocateAdjn == null ? 0 : txtAllocateAdjn.Text.Trim() == string.Empty ? 0 : Convert.ToDecimal(txtAllocateAdjn.Text.Trim());
                            FinReceiptCusAllocationObj.RAD_ACTIVE = Convert.ToByte(DbActiveStatus.ACTIVE);
                            FinReceiptCusAllocationObj.RAD_RCM_INVOICE_HDR = Convert.ToInt64(hdfInvPK.Value);
                            FinReceiptCusAllocationObj.RAD_AMOUNT_BAL = string.IsNullOrEmpty(lblBalanceAdjn.Text) ? 0 : Convert.ToDecimal(lblBalanceAdjn.Text.Replace(",", ""));
                            FinReceiptCusAllocationObj.RAD_NO = lblAdjNo.Text;
                            //if (FinReceiptCusAllocationObj.RAD_AMOUNT > 0)
                            //{
                            FinReceiptCusAllocationList.Add(FinReceiptCusAllocationObj);
                            //}
                            //rowID++;
                        }
                        retObject = FinReceiptCusAllocationList;
                        break;
                    #endregion

                    #region CR/DR Split
                    case ControlsEnum.CRDRSPLITLIST:
                        FinReceiptCusCrdrMpgList = new List<ReceiptCrdrMpg>();
                        foreach (GridViewRow grdrow in grdCrdrAllocation.Rows)
                        {
                            ReceiptCrdrMpg finReceiptCusCrdrMpgObj = new ReceiptCrdrMpg();
                            TextBox txtCrdrReceiveNow = (TextBox)grdrow.FindControl("txtCrdrReceiveNow");
                            TextBox txtCrdrAdjAmount = (TextBox)grdrow.FindControl("txtCrdrAdjAmount");
                            HiddenField hdfCrdrMpgPk = (HiddenField)grdrow.FindControl("hdfCrdrMpgPk");
                            decimal CrdrReceiveNow = 0;
                            decimal CrdrAdjAmnt = 0;
                            decimal.TryParse(txtCrdrReceiveNow.Text, out CrdrReceiveNow);
                            decimal.TryParse(txtCrdrAdjAmount.Text, out CrdrAdjAmnt);
                            finReceiptCusCrdrMpgObj.RNM_PAID_AMOUNT = CrdrReceiveNow;
                            finReceiptCusCrdrMpgObj.RNM_ADJ_AMOUNT = CrdrAdjAmnt;
                            finReceiptCusCrdrMpgObj.RNM_CRDR_MPG = string.IsNullOrEmpty(hdfCrdrMpgPk.Value) ? (long?)null : Convert.ToInt64(hdfCrdrMpgPk.Value);
                            FinReceiptCusCrdrMpgList.Add(finReceiptCusCrdrMpgObj);
                        }
                        retObject = FinReceiptCusCrdrMpgList;
                        break;
                    #endregion
                    #region UPDATE GRID VAL TO OBJECT
                    case ControlsEnum.UPDATEGRIDVALTOOBJECT:
                        if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping != null)
                        {
                            tempReceiptHeader = ReceiptHeaderSession;
                            foreach (GridViewRow grdrow in grdInvoiceList.Rows)
                            {
                                taxAmnt = 0;
                                reductionAmnt = 0;
                                otherAmnt = 0;
                                receiveAmnt = 0;
                                adjAmnt = 0;
                                hdfInvoicePK = (HiddenField)grdrow.FindControl(GetLocalResourceObject("hdfInvoicePK").ToString());
                                TextBox txtReceivedNow = (TextBox)grdrow.FindControl(GetLocalResourceObject("txtReceivedNow").ToString());
                                txtAdjustments = (TextBox)grdrow.FindControl("txtAdjustments");
                                ReceiptTrxMapping objMapping = tempReceiptHeader.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == Convert.ToInt32(hdfInvoicePK.Value));
                                HiddenField hdfTotalTax = (HiddenField)grdrow.FindControl("hdfTotalTax");
                                TextBox txtOthercharges = (TextBox)grdrow.FindControl("txtOthercharges");
                                Label lblAdjAmount = (Label)grdrow.FindControl("lblAdjAmount");

                                if (txtReceivedNow != null)
                                    decimal.TryParse(txtReceivedNow.Text, out receiveAmnt);
                                objMapping.RCM_RCVD_AMOUNT = receiveAmnt;
                                if (txtOthercharges != null)
                                    decimal.TryParse(txtOthercharges.Text, out otherAmnt);
                                objMapping.RCM_OTHER_AMOUNT = otherAmnt;
                                if (hdfTotalTax != null)
                                    decimal.TryParse(hdfTotalTax.Value, out taxAmnt);
                                objMapping.RCM_TAX_AMOUNT = taxAmnt;
                                if (txtAdjustments != null)
                                    decimal.TryParse(txtAdjustments.Text, out reductionAmnt);
                                if (lblAdjAmount != null)
                                    decimal.TryParse(lblAdjAmount.Text, out adjAmnt);
                                objMapping.RCM_ADJUST_AMOUNT = adjAmnt;
                            }
                            ReceiptHeaderSession = tempReceiptHeader;
                        }
                        retObject = ReceiptHeaderSession;
                        break;
                    #endregion
                    #region SUSPENSE LIST
                    case ControlsEnum.SUSPENCELIST:
                        List<ReceiptSuspendDetails> lstDetails = new List<ReceiptSuspendDetails>();
                        foreach (GridViewRow grdrow in grdSuspenceList.Rows)
                        {
                            ReceiptSuspendDetails finReceiptSuspObj = new ReceiptSuspendDetails();
                            CheckBox chkItem = (CheckBox)grdrow.FindControl("chkItem");
                            if (chkItem.Checked)
                            {
                                HiddenField hdfFTHPK = (HiddenField)grdrow.FindControl("hdfFTHPK");
                                HiddenField hdfEntryPK = (HiddenField)grdrow.FindControl("hdfEntryPK");
                                Label lblAmount = (Label)grdrow.FindControl("lblAmount");
                                finReceiptSuspObj.RSC_PK = hdfEntryPK.Value != string.Empty ? Convert.ToInt32(hdfEntryPK.Value) : 0;
                                finReceiptSuspObj.RSC_RECEIPT_HDR = CurrPK;
                                finReceiptSuspObj.RSC_VOUCHER_HDR = Convert.ToInt32(hdfFTHPK.Value);
                                finReceiptSuspObj.RSC_AMOUNT = Convert.ToDecimal(lblAmount.Text);
                                lstDetails.Add(finReceiptSuspObj);
                            }
                        }
                        retObject = lstDetails;
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

        private void CalculateReceiptTax(ReceiptTrxMapping receiptInvTrxMpg, int soHdr, decimal splitTaxAMOUNT, byte Category, long invoiceHdr, decimal rsoPaidAmount)
        {
            List<ReceiptTaxDetails> finReceiptTaxList = new List<ReceiptTaxDetails>();

            if (soHdr > 0)
            {
                finReceiptTaxList = receiptInvTrxMpg.ReceiptTaxDetails.Where(f => f.RDT_SO_HDR == soHdr && f.RDT_INVOICE_HDR == invoiceHdr).ToList();
            }
            else
            {
                finReceiptTaxList = receiptInvTrxMpg.ReceiptTaxDetails.Where(f => f.RDT_INVOICE_HDR == invoiceHdr).ToList();
            }

            if (finReceiptTaxList.Count > 0)
            {

                foreach (ReceiptTaxDetails objTax in finReceiptTaxList)
                {
                    decimal splitDiscAMOUNT = rsoPaidAmount;
                    #region Advance Invoice
                    if (Category != (Byte)(SalesInvoiceCategory.Invoice))
                    {

                        if (objTax.RDT_TAX_CATEGORY != (byte)TaxType.Discount)
                        {
                            objTax.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(Convert.ToDouble(splitTaxAMOUNT) * objTax.RDT_TAX_PERC)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        }
                        else if (objTax.RDT_TAX_CATEGORY == (byte)TaxType.Discount)
                        {
                            objTax.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(Convert.ToDouble(splitDiscAMOUNT) * objTax.RDT_TAX_PERC)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        }
                    }
                    #endregion
                    #region Invoice
                    else if (Category == (Byte)(POInvoiceCategory.Invoice))
                    {
                        if (objTax.RDT_TAX_CATEGORY != (byte)TaxType.Discount)
                        {
                            objTax.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(Convert.ToDouble(splitTaxAMOUNT) * objTax.RDT_TAX_PERC)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        }
                        else if (objTax.RDT_TAX_CATEGORY == (byte)TaxType.Discount)
                        {
                            objTax.RDT_AMOUNT = Convert.ToDecimal(ERP.Utilities.CommonFunctions.DoubleFormatRound(Convert.ToDouble(Convert.ToDecimal(Convert.ToDouble(splitDiscAMOUNT) * objTax.RDT_TAX_PERC)), Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits));
                        }
                    }
                    #endregion

                }
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
                    #region Receipt Header
                    case ControlsEnum.RECEIPTHEADER:
                        receiptHeaderObj = ReceiptHeaderSession;
                        if (receiptHeaderObj != null && grdInvoiceList.Rows.Count <= 0)
                        {
                            if (CurrPK > 0)
                            {
                                lblReceiptNo.Text = string.IsNullOrEmpty(receiptHeaderObj.RCH_NO) ? Resources.ErpRes.Draft : receiptHeaderObj.RCH_NO;
                                txtReceiptDate.Text = receiptHeaderObj.RCH_DATE.ToString(Resources.Constants.DateFormatShort);
                                txtCustomer.Text = HttpUtility.HtmlDecode(receiptHeaderObj.RCH_CUSTOMER_TEXT);
                                hdfCustomerPk.Value = receiptHeaderObj.RCH_CUSTOMER.ToString();
                            }
                            SOGroup = (SalesInvoiceGroup)Enum.Parse(typeof(SalesInvoiceGroup), receiptHeaderObj.RCH_GROUP.ToString());
                            SICategory = (SalesInvoiceCategory)Enum.Parse(typeof(SalesInvoiceCategory), receiptHeaderObj.RCH_CATEGORY.ToString());
                            ddlMode.SelectedValue = receiptHeaderObj.RCH_MODE == 0 ? CommonConstants.SELECTVAL : receiptHeaderObj.RCH_MODE.ToString();
                            hdfRecStatus.Value = receiptHeaderObj.RCH_STATUS.ToString();
                            hdfRecWKFStatus.Value = Convert.ToInt16(receiptHeaderObj.RCH_HAS_JRNL_ENTRY).ToString();
                            //When coming from inbox managing return button,Otherwise its in editmode,and itemselect                           
                            int pdc = 0;
                            pdc = receiptHeaderObj.RCH_PDC;
                            recipetType = receiptHeaderObj.RCH_GROUP;

                            int payMode = 0;
                            bool app = ucrWrkf.IsWkfCompleted;

                            payMode = Convert.ToInt16(receiptHeaderObj.RCH_MODE);
                            GetFieldValues(ControlsEnum.FINHEADERSTATUS);
                            if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                            {
                                if (pdc != 0)
                                    if (finTrxHdrList[0].FTH_STATUS == 2)
                                    {
                                        hdfShowPDC.Value = pdc.ToString();
                                    }
                                    else
                                        hdfShowPDC.Value = "0";
                                else
                                    hdfShowPDC.Value = "0";
                            }

                            if (payMode == (int)PaymentModeEnum.Cheque)
                            {
                                if (pdc != 0)
                                    hdfShowChequeReturn.Value = pdc.ToString();
                                else
                                    hdfShowChequeReturn.Value = "1";
                            }
                            else
                            {
                                hdfShowChequeReturn.Value = "1";
                            }


                            //show 'return' only after pdc is completed
                            recipetTypePDC = 4;
                            GetFieldValues(ControlsEnum.FINHEADERSTATUSREVERSE);
                            if (finTrxHdrPDCList != null && finTrxHdrPDCList.Count == 1)
                            {
                                if (finTrxHdrPDCList[0].FTH_STATUS == 2)
                                {
                                    hdfShowChequeReturn.Value = "5";
                                    btnReturnDetail.Visible = true;
                                    btnReturn.Visible = true;
                                }
                                else
                                {
                                    hdfShowChequeReturn.Value = "6";
                                    btnReturnDetail.Visible = false;
                                    btnReverseDetail.Visible = true;
                                    btnReturn.Visible = false;
                                }
                            }

                            txtBank.Text = receiptHeaderObj.RCH_BANK_TEXT;
                            hdfBank.Value = hdfSavedBankPk.Value = Convert.ToString(receiptHeaderObj.RCH_BANK);
                           
                            txtReceiptCurrency.Text = receiptHeaderObj.RCH_CURRENCY_TEXT;
                            hdfReceiptCurrency.Value = receiptHeaderObj.RCH_CURRENCY.ToString();
                            ddlCompany.SelectedValue = receiptHeaderObj.RCH_COMPANY.ToString();

                            GetFieldValues(ControlsEnum.EXCHANGERATEBANK);
                            if ((ddlBankChargeCurrency.Items[0].Value != receiptHeaderObj.RCH_CURRENCY.ToString()) && !string.IsNullOrEmpty(Convert.ToString(receiptHeaderObj.RCH_BANK_CHARGE_CURR)))
                            {
                                ddlBankChargeCurrency.Items.Insert(1, (new ListItem(receiptHeaderObj.RCH_BANK_CHARGE_CURR_TEXT, receiptHeaderObj.RCH_BANK_CHARGE_CURR.ToString())));
                            }
                            if (!string.IsNullOrEmpty(hdfBank.Value) && Convert.ToInt32(hdfBank.Value.ToString()) > 0)
                            {
                                //GetFieldValues(ControlsEnum.BANK);

                                //if (finCashBankMstList != null && finCashBankMstList.Count > 0)
                                //{
                                if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
                                {
                                    txtAccountNo.Text = receiptHeaderObj.CBM_ACC_NO;
                                    txtBranch.Text = receiptHeaderObj.CBM_BRANCH;
                                }
                                hdfBankAccount.Value = Convert.ToString(receiptHeaderObj.CBM_ACCOUNT);
                                if (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.BANK)
                                {
                                    imbSuspencelist.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
                                }
                                //}
                                //else
                                //{
                                //    txtAccountNo.Text = string.Empty;
                                //    txtBranch.Text = string.Empty;
                                //    hdfBankAccount.Value = string.Empty;
                                //}
                            }
                            txtInstrumentNo.Text = HttpUtility.HtmlDecode(receiptHeaderObj.RCH_INSTR_NO);
                            txtInstrumentDate.Text = string.IsNullOrEmpty(receiptHeaderObj.RCH_INSTR_DATE) ? string.Empty :
                                Convert.ToDateTime(receiptHeaderObj.RCH_INSTR_DATE).ToString(Resources.Constants.DateFormatShort);
                            txtbankOfCheque.Text = HttpUtility.HtmlDecode(receiptHeaderObj.RCH_BANK_OF_CHEQUE);
                            txtRemarks.Text = HttpUtility.HtmlDecode(receiptHeaderObj.RCH_REMARKS);

                            LastModifiedTime = receiptHeaderObj.LAST_MOD_DT;
                            lblLastModifiedHDR.Text = Resources.Report.LastModifiedOn + LastModifiedTime.ToString(Resources.Report.LastModifiedDatetimeFormat);
                            Approved = WkfStatus = receiptHeaderObj.RCH_STATUS;
                            hdfDelStatus.Value = receiptHeaderObj.RCH_DEL_STATUS.ToString();
                            if (!string.IsNullOrEmpty(receiptHeaderObj.RCH_DISCOUNT))
                                ddlAdjType.SelectedIndex = Convert.ToInt32(ddlAdjType.Items.IndexOf(ddlAdjType.Items.FindByValue(receiptHeaderObj.RCH_DISCOUNT.ToString())));
                            txtAdjAmount.Text = Math.Round(receiptHeaderObj.RCH_DISC_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            txtTaxAmount.Text = Math.Round(receiptHeaderObj.RCH_TAX_AMOUNT, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                            chkPDC.Checked = receiptHeaderObj.RCH_PDC >= 1 ? true : false;
                            if (GetGlobalResourceObject("ConfigurationsRes", "PDCReconciliation").ToString() == "1")
                            {
                                chkPDC.Checked = true;
                                chkPDC.Disabled = true;
                            }

                            if (!string.IsNullOrEmpty(receiptHeaderObj.RCH_BANK_CHARGE_CURR) && Convert.ToInt32(receiptHeaderObj.RCH_BANK_CHARGE_CURR) > 0)
                            {
                                ddlBankChargeCurrency.SelectedIndex = Convert.ToInt32(ddlBankChargeCurrency.Items.IndexOf(ddlBankChargeCurrency.Items.FindByValue(receiptHeaderObj.RCH_BANK_CHARGE_CURR.ToString())));
                            }
                            txtBankCharge.Text = Math.Round(receiptHeaderObj.RCH_BANK_CHARGE, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                            chkReturn.Checked = Convert.ToBoolean(receiptHeaderObj.RCH_RETURN_STATUS);
                            if (!string.IsNullOrEmpty(receiptHeaderObj.RCH_RETURN_DATE))
                                txtReturnDate.Text = Convert.ToDateTime(receiptHeaderObj.RCH_RETURN_DATE).ToString(Resources.Constants.DateFormatShort);
                            txtReturnRemarks.Text = receiptHeaderObj.RCH_RETURN_REMARKS;


                            hdfCategoryDtl.Value = receiptHeaderObj.RCH_CATEGORY.ToString();
                            hdfTypeDtl.Value = receiptHeaderObj.RCH_TYPE.ToString();
                            hdfInvoiceCurr.Value = receiptHeaderObj.RCH_CURRENCY.ToString();
                            SelectedBankAccount = 0;
                            if (receiptHeaderObj.receiptSuspendDetails != null && receiptHeaderObj.receiptSuspendDetails.Count > 0)
                            {
                                SelectedBankAccount = Convert.ToInt32(receiptHeaderObj.RCH_BANK);
                                lstReceiptSuspendDetails = receiptHeaderObj.receiptSuspendDetails;
                                GetFieldValues(ControlsEnum.SUSPENCELIST);
                                SetFieldValues(ControlsEnum.SUSPENCELIST);
                                txtSuspenceAmt.Text = GetFormattedCurrency(lstReceiptSuspendDetails.Sum(x => x.RSC_AMOUNT));

                            }
                           
                        }
                        break;
                    #endregion
                    #region RECEIPTGET
                    case ControlsEnum.RECEIPTGET:
                        if (ReceiptHeaderSession != null)
                        {
                            Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ReceiptHeaderSession.RCH_CUSTOMER;
                            Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ReceiptHeaderSession.RCH_CUSTOMER_TEXT;
                            hdfShowPDC.Value = "0";
                            hdfShowChequeReturn.Value = "1";
                            if (ReceiptHeaderSession.RCH_DEL_STATUS == 1)
                            {
                                btnEditforCancel.Visible = false;
                                btnSave.Visible = false;
                                btnEdit.Visible = false;
                                IsDeleted = true;
                            }
                            else
                            {
                                btnEditforCancel.Visible = true;
                                btnSave.Visible = true;
                                IsDeleted = false;
                            }
                            if (ReceiptHeaderSession.RCH_RETURN_STATUS == 1)//It was a returned receipt, show return region for viewing saved details
                            {
                                hdfReceiptReturnHide.Value = "0";
                                hdfIsReceiptReturned.Value = "1";//For showing saved details in return region                            
                            }
                            if (ReceiptHeaderSession.RCH_HAS_JRNL_ENTRY)
                            {
                                bool app = ucrWrkf.IsWkfCompleted;
                                GetFieldValues(ControlsEnum.FINHEADERSTATUS);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {

                                    if (finTrxHdrList[0].FTH_STATUS == 2)
                                    {
                                        hdfShowPDC.Value = ReceiptHeaderSession.RCH_PDC.ToString();
                                    }
                                    else
                                        hdfShowPDC.Value = "0";
                                }
                            }
                            if (ReceiptHeaderSession.RCH_MODE == (int)PaymentModeEnum.Cheque)
                            {
                                hdfShowChequeReturn.Value = ReceiptHeaderSession.RCH_PDC.ToString();
                            }
                            else
                            {
                                hdfShowChequeReturn.Value = "1";
                            }
                            // Get ReceiptDetails
                            GetFieldValues(ControlsEnum.RECEIPTHEADER);
                            SetFieldValues(ControlsEnum.RECEIPTHEADER);
                            SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                            EntryStatus = EntryStatus.VIEWMODE;
                            WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                            base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                            SetCancelRef((int)CurrPK);
                            ucrWrkf.FillWorkFlowDetails();
                            if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE) && ucrWrkf.HasPageTaskPermission)
                            {
                                ucrWrkf.ViewType = 1;
                            }
                            else
                            {
                                ucrWrkf.ViewType = 0;
                            }
                            ucrWrkf.ViewAction();

                            ModifiedDatePnl.Visible = true;
                            SetReceiptMode();
                            hdfIsPendingInvVisible.Value = "0";
                        }

                        break;
                    #endregion
                    #region PAYMENTSPLITLIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping != null && ReceiptHeaderSession.ReceiptTrxMapping.Count > 0 && InvoicePK > 0)
                        {
                            ReceiptTrxMapping objTrxMpg = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK);
                            if (objTrxMpg != null)
                            {
                                lblInvSplitNo.Text = lblInvSplitNo_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(objTrxMpg.ICH_NO, 15);
                                lblInvSplitNo.ToolTip = lblInvSplitNo_CrdrAlcn.ToolTip = objTrxMpg.ICH_NO;

                                lblInvSplitDate.Text = lblInvSplitDate_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(objTrxMpg.ICH_DATE.ToString(Resources.Constants.DateFormatShort), 13);
                                lblInvSplitDate.ToolTip = lblInvSplitDate_CrdrAlcn.ToolTip = objTrxMpg.ICH_DATE.ToString(Resources.Constants.DateFormatShort);

                                lblInvSplitSupplier.Text = lblInvSplitSupplier_CrdrAlcn.Text = ERP.Utilities.CommonFunctions.GetShortString(txtCustomer.Text, 13);
                                lblInvSplitSupplier.ToolTip = lblInvSplitSupplier_CrdrAlcn.ToolTip = ERP.Utilities.CommonFunctions.GetDecodedString(txtCustomer.Text);

                                lblInvSplitAmount.Text = GetFormattedCurrencyWithComma(objTrxMpg.ICH_AMOUNT_NET_TC);
                                lblInvSplitAmount.ToolTip = GetFormattedCurrencyWithComma(objTrxMpg.ICH_AMOUNT_NET_TC);
                                lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = GetFormattedCurrencyWithComma(objTrxMpg.ICH_AMOUNT_RCVD_TC);
                                //if (finReceiptCusSoMpgList != null && finReceiptCusSoMpgList.Count > 0)
                                //{
                                //    if (EntryStatus != EntryStatus.NEWMODE)
                                //    {
                                //        lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = finReceiptCusSoMpgList[0].RSO_BOUNCED == 0 ? String.Format("{0:c}", objTrxMpg.ICH_AMOUNT_RCVD_TC - PayNowAmount)
                                //            : String.Format("{0:c}", objTrxMpg.ICH_AMOUNT_RCVD_TC);
                                //    }
                                //    else
                                //    {                                        
                                //        lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = String.Format("{0:c}", objTrxMpg.ICH_AMOUNT_RCVD_TC);
                                //    }
                                //}
                                //else
                                //{
                                //    if (EntryStatus != EntryStatus.NEWMODE)
                                //    {

                                //        lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = String.Format("{0:c}", objTrxMpg.ICH_AMOUNT_RCVD_TC - PayNowAmount);
                                //    }
                                //    else
                                //    {                                        
                                //        lblInvSplitReceived.Text = lblInvSplitReceived.ToolTip = String.Format("{0:c}", objTrxMpg.ICH_AMOUNT_RCVD_TC);
                                //    }
                                //}

                                lblInvSplitReceiveNow.Text = GetFormattedCurrencyWithComma(PayNowAmount);
                                lblInvSplitReceiveNow.ToolTip = GetFormattedCurrencyWithComma(PayNowAmount);
                            }
                        }
                        break;
                    #endregion
                    #region SPLITPAYNOWFORMULISO
                    case ControlsEnum.SPLITPAYNOWFORMULISO:
                        if (grdReceiptSplit.Rows.Count > 0 && InvoicePK > 0)
                        {
                            GetFieldValues(ControlsEnum.INVOICEVNDMPGLISTFORAUTOALCN);
                            decimal InvoiceSubTotal = 0;
                            decimal PayNowSplit = 0;
                            decimal ExcessAmount = 0;
                            decimal InvTotalOtherCharge = 0;
                            decimal TotalOtherCharge = InvOtherCharge;
                            decimal InvAllocatedCNAmnt = 0;

                            if (FinInvoiceCusTrxMpgListForAutoAlcn != null && FinInvoiceCusTrxMpgListForAutoAlcn.Count > 0)
                            {
                                InvoiceSubTotal = FinInvoiceCusTrxMpgListForAutoAlcn.Sum(r => r.ICM_AMOUNT);
                                //if (ReceiptCrdrList != null && ReceiptCrdrList.Count > 0)
                                //{
                                //    long InvPk = FinInvoiceCusTrxMpgListForAutoAlcn[0].ICM_INVOICE_HDR;
                                //    InvAllocatedCNAmnt = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == InvPk).Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                                //    PayNow = (PayNow - InvAllocatedCNAmnt) < 0 ? 0 : (PayNow - InvAllocatedCNAmnt);
                                //}
                                if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping.Count > 0)
                                {
                                    long InvPk = FinInvoiceCusTrxMpgListForAutoAlcn[0].ICM_INVOICE_HDR;
                                    ReceiptTrxMapping objTrxmpg = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvPk);
                                    if (objTrxmpg != null && objTrxmpg.DebitNoteAllocation != null && objTrxmpg.DebitNoteAllocation.Count > 0)
                                    {
                                        InvAllocatedCNAmnt = objTrxmpg.DebitNoteAllocation.Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                                        PayNow = (PayNow - InvAllocatedCNAmnt) < 0 ? 0 : (PayNow - InvAllocatedCNAmnt);
                                    }
                                }
                                if (InvoiceSubTotal > 0)
                                {
                                    foreach (GridViewRow grdRow in grdReceiptSplit.Rows)
                                    {
                                        HiddenField hdfSOPK = (HiddenField)grdRow.FindControl("hdfSOPK");
                                        Label lblBalanceSplit = (Label)grdRow.FindControl("lblBalanceSplit");
                                        decimal InvAmount = 0;
                                        decimal PoBalanceAmount = 0;
                                        decimal InvOtherAmount = 0;
                                        decimal OtherAmount = 0;
                                        int SoPk = 0;
                                        int.TryParse(hdfSOPK.Value, out SoPk);
                                        decimal.TryParse(lblBalanceSplit.Text.Replace(",", "").Trim(), out PoBalanceAmount);
                                        InvAmount = FinInvoiceCusTrxMpgListForAutoAlcn.Where(iv => iv.ICM_SO_HDR == SoPk).Sum(inv => inv.ICM_AMOUNT);
                                        InvOtherAmount = FinInvoiceCusTrxMpgListForAutoAlcn.Where(iv => iv.ICM_SO_HDR == SoPk).Sum(inv => inv.ICM_OTHER_AMOUNT);
                                        InvTotalOtherCharge = FinInvoiceCusTrxMpgListForAutoAlcn.Sum(inv => inv.ICM_OTHER_AMOUNT);
                                        if (FinInvoiceCusTrxMpgListForAutoAlcn[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                        {
                                            InvAmount = FinInvoiceCusTrxMpgListForAutoAlcn.Where(iv => iv.ICM_SO_HDR == SoPk).Sum(inv => inv.ICM_AMOUNT);
                                        }

                                        TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNowSplit");
                                        TextBox txtOtherChargesSplit = (TextBox)grdRow.FindControl("txtOtherChargesSplit");
                                        HiddenField hdfOtherChargesSplit = (HiddenField)grdRow.FindControl("hdfOtherChargesSplit");
                                        Label lblTotalTaxSplit = (Label)grdRow.FindControl("lblTotalTaxSplit");
                                        HiddenField hdfTaxSplit = (HiddenField)grdRow.FindControl("hdfTaxSplit");

                                        if (IsDebitNoteApplied || InvAllocatedCNAmnt > 0 || (string.IsNullOrEmpty(txtPayNow.Text) || Convert.ToDecimal(txtPayNow.Text) == 0))
                                        {
                                            PayNowSplit = (InvAmount / InvoiceSubTotal) * PayNow;
                                            PayNowSplit = Math.Round(PayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            if (PayNowSplit > PoBalanceAmount)
                                            {
                                                ExcessAmount += PayNowSplit - PoBalanceAmount;
                                                PayNowSplit = PoBalanceAmount;
                                            }
                                            txtPayNow.Text = Math.Round(PayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                            if (InvTotalOtherCharge > 0)
                                            {
                                                OtherAmount = (InvOtherAmount / InvTotalOtherCharge) * TotalOtherCharge;
                                            }
                                            else
                                            {
                                                OtherAmount = 0;
                                            }
                                            txtOtherChargesSplit.Text = Math.Round(OtherAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                            hdfOtherChargesSplit.Value = OtherAmount.ToString();

                                            decimal TaxAmnt = 0;
                                            decimal TaxPer = 1;
                                            decimal.TryParse(hdfTaxPer.Value, out TaxPer);
                                            TaxAmnt = PayNowSplit * TaxPer;
                                            lblTotalTaxSplit.Text = hdfTaxSplit.Value = Math.Round(TaxAmnt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                        }
                                    }

                                    // Distribute excess amount for payment split
                                    decimal TotalSplitAmnt = 0;
                                    decimal SplitAmnt = 0;
                                    decimal TotalSplitTaxAmnt = 0;
                                    decimal SplitTaxAmnt = 0;
                                    foreach (GridViewRow grdRow in grdReceiptSplit.Rows)
                                    {
                                        HiddenField hdfSOPK = (HiddenField)grdRow.FindControl("hdfSOPK");
                                        Label lblBalanceSplit = (Label)grdRow.FindControl("lblBalanceSplit");
                                        decimal InvAmount = 0;
                                        decimal PoBalanceAmount = 0;
                                        decimal InvOtherAmount = 0;
                                        decimal OtherAmount = 0;
                                        int SoPk = 0;
                                        int.TryParse(hdfSOPK.Value, out SoPk);
                                        decimal.TryParse(lblBalanceSplit.Text.Replace(",", "").Trim(), out PoBalanceAmount);
                                        InvAmount = FinInvoiceCusTrxMpgListForAutoAlcn.Where(iv => iv.ICM_SO_HDR == SoPk).Sum(inv => inv.ICM_AMOUNT);
                                        InvOtherAmount = FinInvoiceCusTrxMpgListForAutoAlcn.Where(iv => iv.ICM_SO_HDR == SoPk).Sum(inv => inv.ICM_OTHER_AMOUNT);
                                        InvTotalOtherCharge = FinInvoiceCusTrxMpgListForAutoAlcn.Sum(inv => inv.ICM_OTHER_AMOUNT);
                                        if (FinInvoiceCusTrxMpgListForAutoAlcn[0].FIN_INVOICE_CUS_HDR.ICH_CATEGORY == (byte)POInvoiceCategory.Invoice)
                                        {
                                            InvAmount = FinInvoiceCusTrxMpgListForAutoAlcn.Where(iv => iv.ICM_SO_HDR == SoPk).Sum(inv => inv.ICM_AMOUNT);
                                        }

                                        TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNowSplit");
                                        TextBox txtOtherChargesSplit = (TextBox)grdRow.FindControl("txtOtherChargesSplit");
                                        HiddenField hdfOtherChargesSplit = (HiddenField)grdRow.FindControl("hdfOtherChargesSplit");
                                        Label lblTotalTaxSplit = (Label)grdRow.FindControl("lblTotalTaxSplit");
                                        HiddenField hdfTaxSplit = (HiddenField)grdRow.FindControl("hdfTaxSplit");
                                        if (string.IsNullOrEmpty(txtPayNow.Text) || Convert.ToDecimal(txtPayNow.Text) != PoBalanceAmount)
                                        {
                                            PayNowSplit = (InvAmount / InvoiceSubTotal) * PayNow;
                                            PayNowSplit = Math.Round(PayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits);
                                            if (PayNowSplit < PoBalanceAmount && ExcessAmount > 0)
                                            {
                                                decimal Excess = ExcessAmount;
                                                ExcessAmount -= PoBalanceAmount - PayNowSplit;
                                                PayNowSplit += (ExcessAmount < 0) ? Excess : (PoBalanceAmount - PayNowSplit);
                                                txtPayNow.Text = Math.Round(PayNowSplit, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                                if (InvTotalOtherCharge > 0)
                                                {
                                                    OtherAmount = (InvOtherAmount / InvTotalOtherCharge) * TotalOtherCharge;
                                                }
                                                else
                                                {
                                                    OtherAmount = 0;
                                                }
                                                txtOtherChargesSplit.Text = Math.Round(OtherAmount, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                                hdfOtherChargesSplit.Value = OtherAmount.ToString();

                                                decimal TaxAmnt = 0;
                                                decimal TaxPer = 1;
                                                decimal.TryParse(hdfTaxPer.Value, out TaxPer);
                                                TaxAmnt = PayNowSplit * TaxPer;
                                                lblTotalTaxSplit.Text = hdfTaxSplit.Value = Math.Round(TaxAmnt, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                            }
                                        }
                                        decimal.TryParse(txtPayNow.Text, out SplitAmnt);
                                        TotalSplitAmnt += SplitAmnt;
                                        decimal.TryParse(lblTotalTaxSplit.Text, out SplitTaxAmnt);
                                        TotalSplitTaxAmnt += SplitTaxAmnt;

                                    }

                                    int RowIndex = grdReceiptSplit.Rows.Count - 1;
                                    if (PayNow != TotalSplitAmnt)
                                    {
                                        for (int rowcount = grdReceiptSplit.Rows.Count - 1; rowcount > 0; rowcount--)
                                        {
                                            GridViewRow grdSplitRow = grdReceiptSplit.Rows[rowcount];
                                            TextBox txtPayNowSplit = (TextBox)grdSplitRow.FindControl("txtPayNowSplit");
                                            Label lblBalanceSplit = (Label)grdSplitRow.FindControl("lblBalanceSplit");
                                            decimal paynowsplitlast = 0;
                                            decimal.TryParse(txtPayNowSplit.Text, out paynowsplitlast);

                                            decimal balancesplitlast = 0;
                                            decimal.TryParse(lblBalanceSplit.Text, out balancesplitlast);
                                            if (balancesplitlast >= ((PayNow - TotalSplitAmnt) + paynowsplitlast))
                                            {
                                                paynowsplitlast = paynowsplitlast + (PayNow - TotalSplitAmnt);
                                                txtPayNowSplit.Text = Math.Round(paynowsplitlast, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                                RowIndex = rowcount;
                                                break;
                                            }
                                        }

                                    }
                                    if (Tax != TotalSplitTaxAmnt)
                                    {
                                        GridViewRow grdSplitRow = grdReceiptSplit.Rows[RowIndex];
                                        Label lblTotalTaxSplit = (Label)grdSplitRow.FindControl("lblTotalTaxSplit");
                                        HiddenField hdfTaxSplit = (HiddenField)grdSplitRow.FindControl("hdfTaxSplit");
                                        TextBox txtPayNowSplit = (TextBox)grdSplitRow.FindControl("txtPayNowSplit");
                                        decimal TaxSplitlast = 0;
                                        decimal.TryParse(lblTotalTaxSplit.Text, out TaxSplitlast);
                                        decimal PayNowsplt = 0;
                                        decimal.TryParse(txtPayNowSplit.Text, out PayNowsplt);
                                        if (PayNowsplt > 0)
                                        {
                                            if ((Tax - TotalSplitTaxAmnt) < 1)
                                            {
                                                TaxSplitlast = TaxSplitlast + (Tax - TotalSplitTaxAmnt);
                                                lblTotalTaxSplit.Text = hdfTaxSplit.Value = Math.Round(TaxSplitlast, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();
                                            }
                                        }
                                    }

                                    decimal TotalPayNow = 0;
                                    decimal TotalOtherCharges = 0;
                                    decimal TotalTax = 0;
                                    foreach (GridViewRow grdRow in grdReceiptSplit.Rows)
                                    {

                                        decimal PayNowAmnt = 0;
                                        decimal OtherChargesAmnt = 0;
                                        decimal TaxAmnt = 0;



                                        TextBox txtPayNow = (TextBox)grdRow.FindControl("txtPayNowSplit");
                                        TextBox txtOtherChargesSplit = (TextBox)grdRow.FindControl("txtOtherChargesSplit");
                                        Label lblTotalTaxSplit = (Label)grdRow.FindControl("lblTotalTaxSplit");

                                        decimal.TryParse(txtPayNow.Text, out PayNowAmnt);
                                        TotalPayNow += PayNowAmnt;
                                        decimal.TryParse(txtOtherChargesSplit.Text, out OtherChargesAmnt);
                                        TotalOtherCharges += OtherChargesAmnt;
                                        decimal.TryParse(lblTotalTaxSplit.Text, out TaxAmnt);
                                        TotalTax += TaxAmnt;
                                    }
                                    Label lblTotalPayNowFooterSplit = (Label)grdReceiptSplit.FooterRow.FindControl("lblTotalPayNowFooterSplit");
                                    HiddenField hdfTotalPayNowFooterSplit = (HiddenField)grdReceiptSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit");
                                    lblTotalPayNowFooterSplit.Text = hdfTotalPayNowFooterSplit.Value = Math.Round(TotalPayNow, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                    Label lblTotalOtherChargesFooterSplit = (Label)grdReceiptSplit.FooterRow.FindControl("lblTotalOtherChargesFooterSplit");
                                    HiddenField hdfTotalOtherChargesFooterSplit = (HiddenField)grdReceiptSplit.FooterRow.FindControl("hdfTotalOtherChargesFooterSplit");
                                    lblTotalOtherChargesFooterSplit.Text = hdfTotalOtherChargesFooterSplit.Value = Math.Round(TotalOtherCharges, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                    Label lblTotalTaxFooterSplit = (Label)grdReceiptSplit.FooterRow.FindControl("lblTotalTaxFooterSplit");
                                    lblTotalTaxFooterSplit.Text = Math.Round(TotalTax, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();

                                }
                            }
                        }
                        break;
                    #endregion
                    #region CUSTOMER DETAILS
                    case ControlsEnum.CUSTOMERDETAILS:
                        if (dsCustomerData != null && dsCustomerData.Tables != null && dsCustomerData.Tables.Count > 0 && dsCustomerData.Tables[0].Rows.Count > 0)
                        {
                            CustomerType = Convert.ToInt32(dsCustomerData.Tables[0].Rows[0]["CUS_TYPE_VALUE"]);
                            ddlPendingInvType.SelectedIndex = ddlPendingInvType.Items.IndexOf(ddlPendingInvType.Items.FindByValue(dsCustomerData.Tables[0].Rows[0]["CUS_TYPE_VALUE"].ToString()));
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
        ///check duplicate based on bank + InstrNo + InstrDate
        /// </summary>
        private bool CheckReceipt(FIN_RECEIPT_CUS_HDR finReceiptHdrObj)
        {
            bool success = true;
            //SalesReceiptService salesReceiptServiceClient;
            //salesReceiptServiceClient = null;
            //try
            //{
            //    if (finReceiptCusHdrObj.RCH_STATUS != 0)
            //    {
            //        if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
            //        {
            //            salesReceiptServiceClient = new SalesReceiptService();
            //            salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
            //            success = salesReceiptServiceClient.CheckReceiptHdr(finReceiptHdrObj);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}

            //finally
            //{
            //    salesReceiptServiceClient = null;
            //}
            return success;
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
                    // ddlCompany.SelectedIndex = ddlCompany.Items.IndexOf(ddlCompany.Items.FindByValue(dtCompany.Rows[0][Resources.DataFieldRes.CompanyMstPK].ToString()));
                    //if (dtCompany.Rows.Count > 0)
                    //{
                    //    ddlCompany.DataSource = dtCompany;

                    //    //ddlCompany.DataSource = CommonFunctions.HtmlDecode(dtCompany);,Resources.DataFieldRes.CompanySpecs);
                    //    ddlCompany.DataTextField = Resources.DataFieldRes.CompanySpecs;
                    //    ddlCompany.DataValueField = Resources.DataFieldRes.CompanyMstPK;
                    //    ddlCompany.DataBind();

                    //}
                    break;
                #endregion
                #region INVOICE TYPE
                case ControlsEnum.INVOICETYPE:
                    ddlPendingInvType.Items.Clear();
                    if (dtInvoiceType != null && dtInvoiceType.Rows.Count > 0)
                    {
                        ddlPendingInvType.DataSource = dtInvoiceType;
                        ddlPendingInvType.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlPendingInvType.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlPendingInvType.DataBind();
                    }
                    //ddlPendingInvType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region INV CATEGORY
                case ControlsEnum.INVCATEGORY:
                    ddlPendingInvCategory.Items.Clear();
                    if (dtInvCategory != null && dtInvCategory.Rows.Count > 0)
                    {
                        ddlPendingInvCategory.DataSource = dtInvCategory;
                        ddlPendingInvCategory.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlPendingInvCategory.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlPendingInvCategory.DataBind();
                    }
                    //ddlPendingInvCategory.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Payment Mode
                case ControlsEnum.PAYMODE:
                    ddlMode.Items.Clear();
                    if (dtPaymentModes != null && dtPaymentModes.Rows.Count > 0)
                    {
                        ddlMode.DataSource = dtPaymentModes;
                        ddlMode.DataTextField = Resources.DataFieldRes.cfgData;
                        ddlMode.DataValueField = Resources.DataFieldRes.cfgValue;
                        ddlMode.DataBind();
                    }
                    ddlMode.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
                    break;
                #endregion
                #region Adjustment Types
                case ControlsEnum.DISCOUNTTYPE:
                    ddlAdjType.Items.Clear();
                    if (dtDiscountTypes != null && dtDiscountTypes.Rows.Count > 0)
                    {
                        ddlAdjType.DataSource = dtDiscountTypes;
                        ddlAdjType.DataTextField = Resources.DataFieldRes.ConstName;
                        ddlAdjType.DataValueField = Resources.DataFieldRes.ConstPK;
                        ddlAdjType.DataBind();
                    }
                    ddlAdjType.Items.Insert(0, new ListItem(Resources.ErpRes.Select, CommonConstants.SELECTVAL));
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
                    #region PEDING INV LIST
                    case ControlsEnum.PEDINGINVLIST:
                        if (dtPendingInvList != null && dtPendingInvList.Rows.Count > 0)
                            grdPendingInvList.DataSource = dtPendingInvList;
                        else
                            grdPendingInvList.DataSource = null;
                        grdPendingInvList.DataBind();
                        if (Convert.ToInt32(ddlPendingInvCategory.SelectedValue) == (int)SalesInvoiceGroup.Miscellaneous)
                        {
                            grdPendingInvList.Columns[6].Visible = false;//SC No
                            grdPendingInvList.Columns[7].Visible = false;//SC Date
                        }
                        else
                        {
                            grdPendingInvList.Columns[6].Visible = true;
                            grdPendingInvList.Columns[7].Visible = true;
                        }
                        break;
                    #endregion
                    #region RECEIPT MPG LIST
                    case ControlsEnum.RECEIPTMPGLIST:
                        if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping.Count > 0)
                        {
                            grdInvoiceList.DataSource = ReceiptHeaderSession.ReceiptTrxMapping.ToList();
                            ddlPendingInvCategory.SelectedValue = ReceiptHeaderSession.ReceiptTrxMapping[0].ICH_GROUP == (int)SalesInvoiceGroup.Miscellaneous ? ReceiptHeaderSession.ReceiptTrxMapping[0].ICH_GROUP.ToString() : ReceiptHeaderSession.ReceiptTrxMapping[0].ICH_CATEGORY.ToString();
                            ddlPendingInvType.SelectedValue = ReceiptHeaderSession.ReceiptTrxMapping[0].ICH_TYPE.ToString();
                        }
                        else
                            grdInvoiceList.DataSource = null;
                        grdInvoiceList.DataBind();

                        #region Set grid view column visibility
                        if (ReceiptHeaderSession != null)
                        {
                            if (ReceiptHeaderSession.RCH_CATEGORY == (int)SalesInvoiceCategory.Advanced)
                            {
                                if (ReceiptHeaderSession.RCH_TYPE == (int)SalesInvoiceType.Domestic)
                                {
                                    grdInvoiceList.Columns[6].Visible = false;
                                    grdInvoiceList.Columns[7].Visible = false;
                                    grdInvoiceList.Columns[13].Visible = true;
                                    grdInvoiceList.Columns[14].Visible = true;
                                    divTax.Visible = true;
                                }
                                else
                                {
                                    grdInvoiceList.Columns[6].Visible = false;
                                    grdInvoiceList.Columns[7].Visible = false;
                                    grdInvoiceList.Columns[13].Visible = false;
                                    grdInvoiceList.Columns[14].Visible = false;
                                    divTax.Visible = false;
                                }
                                grdInvoiceList.Columns[11].Visible = false;
                                grdInvoiceList.Columns[12].Visible = false;
                            }
                            else
                            {
                                if (!ShowAdjColumn)
                                {
                                    grdInvoiceList.Columns[6].Visible = false;
                                    grdInvoiceList.Columns[7].Visible = false;
                                }
                                else
                                {
                                    grdInvoiceList.Columns[6].Visible = true;
                                    grdInvoiceList.Columns[7].Visible = true;
                                }
                                grdInvoiceList.Columns[13].Visible = false;
                                grdInvoiceList.Columns[11].Visible = true;
                                grdInvoiceList.Columns[12].Visible = true;
                                grdInvoiceList.Columns[14].Visible = false;
                                divTax.Visible = false;
                                if (ShowTaxForMiscInv && ReceiptHeaderSession.RCH_GROUP == (int)SalesInvoiceGroup.Miscellaneous)
                                {
                                    grdInvoiceList.Columns[14].Visible = true;
                                    divTax.Visible = true;
                                }
                            }
                        }
                        #endregion
                        break;
                    #endregion
                    #region RECEIPT CUS ADJN
                    case ControlsEnum.RECEIPTCUSADJN:
                        if (ReceiptAdjnList != null && ReceiptAdjnList.Count > 0)
                            grdReceiptSplitAdjn.DataSource = ReceiptAdjnList.Where(r => r.RAD_AMOUNT_BAL > 0).ToList();
                        else
                            grdReceiptSplitAdjn.DataSource = null;
                        grdReceiptSplitAdjn.DataBind();
                        break;
                    #endregion
                    #region CR DR ALLOCATION
                    case ControlsEnum.CRDRALLOCATION:
                        if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping.Count > 0)
                        {
                            grdCrdrAllocation.DataSource = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK).DebitNoteAllocation.ToList();
                        }
                        else
                            grdCrdrAllocation.DataSource = null;
                        grdCrdrAllocation.DataBind();
                        break;
                    #endregion
                    #region RECEIPT HDR LIST
                    case ControlsEnum.RECEIPTHDRLIST:
                        //Paging Properties                       
                        uclPaging.TotalPages = TotalPages;
                        PageIndex = string.IsNullOrEmpty(PageIndex) ? CommonConstants.SELECT_VALUE_ONE : PageIndex;
                        uclPaging.CurrentPage = Convert.ToInt32(PageIndex);
                        if (dtReceiptList != null)
                        {
                            grdReceiptList.PageIndex = Convert.ToInt32(PageIndex);
                            grdReceiptList.DataSource = dtReceiptList.DefaultView;
                            grdReceiptList.DataBind();

                            uclPaging.Visible = true;
                            uclPaging.BindPager();
                        }
                        else
                        {
                            grdReceiptList.DataSource = null;
                            grdReceiptList.DataBind();
                            uclPaging.Visible = false;
                        }
                        break;
                    #endregion
                    #region PAYMENT SPLIT LIST
                    case ControlsEnum.PAYMENTSPLITLIST:
                        if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping.Count > 0)
                        {
                            grdReceiptSplit.DataSource = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK).ReceiptSOMapping.ToList();
                        }
                        else
                            grdReceiptSplit.DataSource = null;
                        grdReceiptSplit.DataBind();
                        #region Set grid view column visibility
                        if (ReceiptHeaderSession != null)
                        {
                            if (ReceiptHeaderSession.RCH_CATEGORY == (int)SalesInvoiceCategory.Advanced)
                            {
                                if (ReceiptHeaderSession.RCH_TYPE == (int)SalesInvoiceType.Domestic)
                                {
                                    grdReceiptSplit.Columns[9].Visible = true;
                                    grdReceiptSplit.Columns[10].Visible = true;
                                }
                                else
                                {
                                    grdReceiptSplit.Columns[9].Visible = false;
                                    grdReceiptSplit.Columns[10].Visible = false;
                                }
                            }
                            else
                            {
                                grdReceiptSplit.Columns[9].Visible = false;
                                grdReceiptSplit.Columns[10].Visible = false;
                            }
                        }
                        #endregion
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                        break;
                    #endregion
                    #region RECEIVED AMT SPLIT UP
                    case ControlsEnum.RECEIVEDAMTSPLITUP:
                        grdReceivedAmtSplitup.DataSource = dtReceivedAmt;
                        grdReceivedAmtSplitup.DataBind();
                        break;
                    #endregion
                    #region SUSPENCE LIST
                    case ControlsEnum.SUSPENCELIST:
                        if (lstReceiptSuspendDetails != null && lstReceiptSuspendDetails.Count > 0 )
                        {
                            foreach (DataRow row in dtSuspenceList.Rows)
                            {
                                row["FTH_SUSP_FLAG"] = lstReceiptSuspendDetails.Where(x => x.RSC_VOUCHER_HDR == Convert.ToInt64(row["FTH_PK"])).Count();
                            }
                        }
                        grdSuspenceList.DataSource = dtSuspenceList;
                        grdSuspenceList.DataBind();
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
                HiddenField hdfPosted;
                HiddenField hdfMode;
                bool posted;
                HiddenField hdfPDC;
                int pdc = 0;
                int payMode = 0;
                foreach (GridViewRow grdrow in grdReceiptList.Rows)
                {
                    RadioButton rbtn = (RadioButton)grdrow.FindControl(Resources.Report.RadioButtonID);
                    // check row selected or not
                    if (rbtn.Checked)
                    {
                        // get pk from the grid and assign to CurrPk
                        CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfReceiptID")).Value);//Convert.ToInt32(grdReceiptList.DataKeys[grdrow.RowIndex].Values[0]);
                        Session[ERP.Utilities.SessionStrings.CUSTOMERPK] = ((HiddenField)grdrow.FindControl("hdfCustomerPK")).Value;
                        Session[ERP.Utilities.SessionStrings.CUSTOMERNAME] = ((Label)grdrow.FindControl("lblCustomer")).Text;
                        Approved = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfApproved")).Value);


                        hdfShowPDC.Value = "0";
                        hdfShowChequeReturn.Value = "1";
                        hdfPosted = grdrow.FindControl("hdfPosted") as HiddenField;
                        hdfMode = grdrow.FindControl("hdfMode") as HiddenField;
                        hdfPDC = grdrow.FindControl("hdfPDC") as HiddenField;
                        HiddenField hdfReturnStatus = grdrow.FindControl("hdfReturnStatus") as HiddenField;

                        hdfDelStatus = grdrow.FindControl("hdfDelStatus") as HiddenField;
                        if (Convert.ToInt16(hdfDelStatus.Value) == 1)
                        {
                            btnEditforCancel.Visible = false;
                            btnSave.Visible = false;
                            btnEdit.Visible = false;
                            IsDeleted = true;

                        }
                        else
                        {
                            btnEditforCancel.Visible = true;
                            btnSave.Visible = true;
                            IsDeleted = false;
                        }
                        if (Convert.ToInt16(hdfReturnStatus.Value) == 1)//It was a returned receipt, show return region for viewing saved details
                        {
                            hdfReceiptReturnHide.Value = "0";
                            hdfIsReceiptReturned.Value = "1";//For showing saved details in return region                            
                        }
                        if (hdfPosted != null && bool.TryParse(hdfPosted.Value, out posted))
                        {
                            if (posted)
                            {

                                bool app = ucrWrkf.IsWkfCompleted;

                                GetFieldValues(ControlsEnum.FINHEADERSTATUS);
                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    if (hdfPDC != null && int.TryParse(hdfPDC.Value, out pdc))
                                        if (finTrxHdrList[0].FTH_STATUS == 2)
                                        {
                                            hdfShowPDC.Value = pdc.ToString();
                                        }
                                        else
                                            hdfShowPDC.Value = "0";
                                    else
                                        hdfShowPDC.Value = "0";
                                }


                            }
                            if (hdfMode != null && int.TryParse(hdfMode.Value, out payMode))
                            {
                                if (payMode == (int)PaymentModeEnum.Cheque)
                                {
                                    if (hdfPDC != null && int.TryParse(hdfPDC.Value, out pdc))
                                        hdfShowChequeReturn.Value = pdc.ToString();
                                    else
                                        hdfShowChequeReturn.Value = "1";
                                }
                                else
                                {
                                    hdfShowChequeReturn.Value = "1";
                                }
                            }
                            else
                            {
                                hdfShowChequeReturn.Value = "1";
                            }
                        }

                        // Get ReceiptDetails
                        GetFieldValues(ControlsEnum.RECEIPTHEADER);
                        SetFieldValues(ControlsEnum.RECEIPTHEADER);
                        SetFieldValues(ControlsEnum.RECEIPTMPGLIST);

                        if (Mode == ActionsEnum.EDITFORCANCEL)
                        {
                            FillProcessID(11);
                        }

                        if (Mode == ActionsEnum.VIEW)
                        {
                            EntryStatus = EntryStatus.VIEWMODE;
                        }
                        else
                        {
                            EntryStatus = EntryStatus.ENTRYMODE;
                        }
                        WorkflowCore.CoreService workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = ucrWrkf.RefID = workflowCore.GetRefID((int)CurrPK, ucrWrkf.ProcessID);
                        SetCancelRef((int)CurrPK);
                        ucrWrkf.FillWorkFlowDetails();

                        if (ucrWrkf.HasActions && ucrWrkf.IsWkfCompleted == false && (EntryStatus == EntryStatus.ENTRYMODE) && ucrWrkf.HasPageTaskPermission)
                        {
                            ucrWrkf.ViewType = 1;
                        }
                        else
                        {
                            ucrWrkf.ViewType = 0;
                        }
                        ucrWrkf.ViewAction();
                        return;
                    }
                }
                // if no items selected, Show Error Message
                litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                EntryStatus = EntryStatus.LISTMODE;
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
            SelectedBankAccount = 0;
            lstReceiptSuspendDetails = null;
            txtSuspenceAmt.Text = string.Empty;
            ReceiptHeaderSession = null;
            ReceiptAdjnList = null;
            AppliedInvPkList = null;
            SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
            SetFieldValues(ControlsEnum.PEDINGINVLIST);
            hdfIsPendingInvVisible.Value = "0";
            CurrPK = 0;
            adjAmtFooter = 0;
            ddlMode.ClearSelection();
            ReceiptModeChanged();
            IsDeleted = false;

            txtCustomerSearch.Text = string.Empty;
            hdfCustomerSearchPk.Value = string.Empty;
            txtCustomer.Text = string.Empty;
            hdfCustomerPk.Value = string.Empty;
            txtReceiptNumber.Text = string.Empty;
            hdfReceiptPK.Value = string.Empty;
            txtReceiptDate.Text = string.Empty;
            hdfBankAccount.Value = string.Empty;

            hdfBankAccount.Value = string.Empty;

            txtSINo.Text = string.Empty;



            txtRemarks.Text = string.Empty;
            txtbankOfCheque.Text = string.Empty;
            txtReceivedAmount.Text = string.Empty;
            hdfExchangeCurr.Value = string.Empty;
            hdfExchangeCurrBC.Value = string.Empty;
            hdfInvoiceCurr.Value = string.Empty;
            hdfCustomerAccountNo.Value = string.Empty;

            hdfReceiptCurrency.Value = string.Empty;
            ModifiedDatePnl.Visible = false;
            lblLastModifiedHDR.Text = string.Empty;
            grdInvoiceList.DataSource = null;
            grdInvoiceList.DataBind();
            txtSearchDateFrom.Text = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateFrom.Value = (DateTime.Now.AddDays(1 - DateTime.Now.Day)).AddMonths(-1).ToString();
            txtSearchDateTo.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
            hdfSearchDateTo.Value = DateTime.Now.ToString();
            //txtSearchDateFrom.Text = string.Empty;
            //hdfSearchDateFrom.Value = string.Empty;
            //txtSearchDateTo.Text = string.Empty;
            //hdfSearchDateTo.Value = string.Empty;

            ddlBankChargeCurrency.Items.Clear();
            GetFieldValues(ControlsEnum.BANKCURRENCY);
            SetFieldValues(ControlsEnum.BANKCURRENCY);
            txtBankCharge.Text = string.Empty;
            //GetFieldValues(ControlsEnum.FINPERIOD);
            //SetFieldValues(ControlsEnum.FINPERIOD);
            hdfShowPDC.Value = "0";
            hdfShowChequeReturn.Value = "1";
            hdfPDCStatusWKF.Value = "0";
            hdfExchRate.Value = string.Empty;
            base.WkfRefID = 0;
            ddlStatus.SelectedIndex = 0;
            ddlPDCStatus.SelectedIndex = 0;

            chkReturn.Checked = false;
            txtReturnDate.Text = string.Empty;
            txtReturnRemarks.Text = string.Empty;
            hdfEditForReturn.Value = "0";
            hdfIsReceiptReturned.Value = "0";
            grdInvoiceList.Enabled = true;
            txtReceiptDate.Enabled = true;


            ddlAdjType.Enabled = true;
            ddlBankChargeCurrency.Enabled = true;
            txtBankCharge.Enabled = true;
            txtRemarks.Enabled = true;


            Session[ERP.Utilities.SessionStrings.TransactionType] = null;

            //Detail search reset
            txtPendingFromDate.Text = string.Empty;
            txtPendingToDate.Text = string.Empty;
            ddlPendingInvCategory.ClearSelection();

            if (CustomerType > 0)
                ddlPendingInvType.SelectedIndex = ddlPendingInvType.Items.IndexOf(ddlPendingInvType.Items.FindByValue(CustomerType.ToString()));
            else
                ddlPendingInvType.ClearSelection();
            txtPendingInvNumber.Text = string.Empty;
            hdfPendingInvPk.Value = string.Empty;


        }

        /// <summary>
        /// Format an amount with currency
        /// </summary>
        /// <param name="number">Amount to format</param>
        /// <returns>Formatted amount</returns>
        public string GetFormattedCurrency(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormat.Value);
        }

        /// <summary>
        /// Format an amount with currency seperation
        /// </summary>
        /// <param name="number">Amount to format</param>
        /// <returns>Formatted amount</returns>
        public string GetFormattedCurrencyWithComma(object number)
        {
            double num = 0;
            double.TryParse(Convert.ToString(number), out num);
            return num.ToString(hdfCurrencyFormatWithSeperator.Value);
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
        /// To check receipt is allocated in Invoice
        /// </summary>       
        public void CheckReceiptAllocated(int SalesInvoiceCategory, int returnStatus)
        {
            GetFieldValues(ControlsEnum.CheckReceiptAllocation);
            if (returnStatus == 1)//It is a returned receipt, show return region for viewing saved details
            {
                hdfReceiptReturnHide.Value = "0";
                //hdfEditForReturn.Value = "1";//For showing saved details in return region.ie,In ShowListing() this flag condition checks
            }
            else
            {
                if (dtReceiptAlloc.Rows.Count > 0 || SalesInvoiceCategory == 1)//Show Return region only for Advance Invoice.(1--Invoice,2--Adv.Invoice)
                {
                    hdfReceiptReturnHide.Value = "1";
                    btnEditForReturn.Visible = false;
                }
                else
                {
                    hdfReceiptReturnHide.Value = "0";

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

            SalesReceiptService salesReceiptServiceClient;
            salesReceiptServiceClient = null;
            int bankPK;
            int mode;
            bool bIsChecked = false;
            DropDownList ddlWkfAction;

            Label lblBaltoRec;

            string action;
            WorkflowCore.CoreService workflowCore;
            int receiptMpgCount;
            //bool isSplitSaved;
            HiddenField hdfInvoicePK;
            HiddenField hdfReceiptMpgPK;
            HiddenField hdfIsApply;


            Label lblTotalAmount;
            Label lblCrdrAlcnAmount;
            HiddenField hdfBaltoReceive;
            GridViewRow grvRow;
            TextBox txtAdjustments;
            TextBox txtReceivedNow;


            bool isContinue;
            List<AdjAllocation> tempReceiptAdjnList;

            decimal BalReceiveTot = 0;
            decimal ReceiveNowTot = 0;
            decimal TotalAdjAmt = 0;
            decimal TotalDebitAmnt = 0;

            bool GenDummy = false;
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));

            try
            {
                long? result;
                result = 0;
                long? DummyResult;
                DummyResult = 0;



                if (sender.GetType().IsEquivalentTo(typeof(Button)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((Button)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(ImageButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((ImageButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(LinkButton)))
                {
                    commonActions = (ActionsEnum)(Enum.Parse(typeof(ActionsEnum), ((LinkButton)sender).CommandName));
                }
                else if (sender.GetType().IsEquivalentTo(typeof(DropDownList)))
                {
                    if (((DropDownList)sender).ID == "ddlMode")
                    {
                        commonActions = ActionsEnum.PAYMENT_MODE_INDEX_CHANGED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(RadioButton)))
                {
                    if (((RadioButton)sender).ID == "rbtSelect")
                    {
                        commonActions = ActionsEnum.ITEMSELECTED;
                    }
                }
                else if (sender.GetType().IsEquivalentTo(typeof(TextBox)))
                {
                    if (((TextBox)sender).ID == "txtAdjustments")
                    {
                        commonActions = ActionsEnum.CHECKAMT;
                    }
                }
                switch (commonActions)
                {
                    #region NEW
                    case ActionsEnum.NEW:
                        ResetForm();
                        ReceiptHeaderSession = null;
                        ModifiedDatePnl.Visible = false;
                        EntryStatus = EntryStatus.NEWMODE;
                        updateReceipt = false;
                        lblReceiptNo.Text = hdfReceiptNo.Value = Resources.ErpRes.Draft;
                        txtReceiptDate.Text = DateTime.Now.ToString(Resources.Constants.DateFormatShort);
                        SetReceiptMode();
                        break;
                    #endregion
                    #region CUSTOMER CHANGE
                    case ActionsEnum.CUSTOMERCHANGE:
                        GetFieldValues(ControlsEnum.CUSTOMERDETAILS);
                        SetFieldValues(ControlsEnum.CUSTOMERDETAILS);
                        GetFieldValues(ControlsEnum.PEDINGINVLIST);
                        SetFieldValues(ControlsEnum.PEDINGINVLIST);
                        hdfIsPendingInvVisible.Value = "1";
                        break;
                    #endregion
                    #region DTL SEARCH
                    case ActionsEnum.DTLSEARCH:
                        GetFieldValues(ControlsEnum.PEDINGINVLIST);
                        SetFieldValues(ControlsEnum.PEDINGINVLIST);
                        hdfIsPendingInvVisible.Value = "1";
                        break;
                    #endregion
                    #region DTL CLEAR SEARCH
                    case ActionsEnum.DTLCLEARSEARCH:
                        txtPendingFromDate.Text = string.Empty;
                        txtPendingToDate.Text = string.Empty;
                        ddlPendingInvCategory.ClearSelection();
                        if (CustomerType > 0)
                            ddlPendingInvType.SelectedIndex = ddlPendingInvType.Items.IndexOf(ddlPendingInvType.Items.FindByValue(CustomerType.ToString()));
                        else
                            ddlPendingInvType.ClearSelection();
                        txtPendingInvNumber.Text = string.Empty;
                        hdfPendingInvPk.Value = string.Empty;
                        GetFieldValues(ControlsEnum.PEDINGINVLIST);
                        SetFieldValues(ControlsEnum.PEDINGINVLIST);
                        hdfIsPendingInvVisible.Value = "1";
                        break;
                    #endregion
                    #region ADD TO LIST
                    case ActionsEnum.ADDTOLIST:
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        InvListObj = new InvoiceBO();
                        InvListObj.InvList = new List<InvoiceDetails>();
                        List<InvoiceDetails> objItemList = new List<InvoiceDetails>();
                        InvoiceDetails objInvDet;
                        //HiddenField hdfInvoicePK;
                        HiddenField hdfInvCustomerPK;
                        HiddenField hdfType;
                        HiddenField hdfInvCurrency;
                        HiddenField hdfPndInvCategory;
                        HiddenField hdfPndInvGroup;
                        if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping != null)
                        {
                            ReceiptHeaderSession.ReceiptTrxMapping.ForEach(dtl =>
                            {
                                objInvDet = new InvoiceDetails();
                                objInvDet.ICH_PK = dtl.RCM_INVOICE_HDR;
                                objInvDet.ICH_CUSTOMER = Convert.ToInt32(ReceiptHeaderSession.RCH_CUSTOMER);
                                objInvDet.ICH_TYPE = dtl.ICH_TYPE;
                                objInvDet.ICH_CURRENCY = ReceiptHeaderSession.RCH_CURRENCY;
                                objInvDet.ICH_CATEGORY = dtl.ICH_CATEGORY;
                                objInvDet.ICH_GROUP = dtl.ICH_GROUP;
                                objItemList.Add(objInvDet);
                            });
                        }
                        foreach (GridViewRow grdrow in grdPendingInvList.Rows)
                        {
                            CheckBox chkSCselect = (CheckBox)grdrow.FindControl("chkInvselect");
                            if (chkSCselect.Checked)
                            {
                                hdfInvoicePK = (HiddenField)grdrow.FindControl("hdfInvoicePk");
                                hdfInvCustomerPK = (HiddenField)grdrow.FindControl("hdfInvCustomerPK");
                                hdfType = (HiddenField)grdrow.FindControl("hdfInvType");
                                hdfInvCurrency = (HiddenField)grdrow.FindControl("hdfInvCurrency");
                                hdfPndInvCategory = (HiddenField)grdrow.FindControl("hdfPndInvCategory");
                                hdfPndInvGroup = (HiddenField)grdrow.FindControl("hdfPndInvGroup");
                                objInvDet = new InvoiceDetails();
                                objInvDet.ICH_PK = Convert.ToInt32(hdfInvoicePK.Value);
                                objInvDet.ICH_CUSTOMER = Convert.ToInt32(hdfInvCustomerPK.Value);
                                objInvDet.ICH_TYPE = Convert.ToInt32(hdfType.Value);
                                objInvDet.ICH_CURRENCY = Convert.ToInt32(hdfInvCurrency.Value);
                                objInvDet.ICH_CATEGORY = Convert.ToByte(hdfPndInvCategory.Value);
                                objInvDet.ICH_GROUP = Convert.ToByte(hdfPndInvGroup.Value);
                                if (objItemList != null && (objItemList.Where(r => r.ICH_CUSTOMER != Convert.ToInt32(hdfInvCustomerPK.Value)).Count() > 0
                                                            || objItemList.Where(r => r.ICH_TYPE != Convert.ToInt32(hdfType.Value)).Count() > 0
                                                            || objItemList.Where(r => r.ICH_CURRENCY != Convert.ToInt32(hdfInvCurrency.Value)).Count() > 0
                                                            || objItemList.Where(r => r.ICH_CATEGORY != Convert.ToByte(hdfPndInvCategory.Value)).Count() > 0
                                                            || objItemList.Where(r => r.ICH_GROUP != Convert.ToByte(hdfPndInvGroup.Value)).Count() > 0
                                                            )
                                    )
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Msg_Err_Inv").ToString()) + "');", true);
                                    return;
                                }
                                if (objItemList != null && objItemList.Where(r => r.ICH_PK == Convert.ToInt32(hdfInvoicePK.Value)).Count() <= 0)
                                    objItemList.Add(objInvDet);
                            }
                        }
                        if (objItemList.Count == 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_NoRecordsSelected").ToString()) + "');", true);
                            return;
                        }
                        InvListObj.InvList = objItemList;
                        //Avoid already added Invoice. No need to get that Invoice details again
                        if (objItemList != null && objItemList.Count > 0 && ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping != null)
                        {
                            List<string> objPoPkList = ReceiptHeaderSession.ReceiptTrxMapping.Select(r => r.RCM_INVOICE_HDR.ToString()).Distinct().ToList();
                            InvListObj.InvList = objItemList.Where(r => !objPoPkList.Contains(r.ICH_PK.ToString())).ToList();
                        }
                        GetFieldValues(ControlsEnum.RECEIPTHEADER);
                        SetFieldValues(ControlsEnum.RECEIPTHEADER);
                        SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                        hdfIsPendingInvVisible.Value = "0";
                        break;
                    #endregion
                    #region ADJN INVOICE DETAIL
                    case ActionsEnum.ADJNINVOICEDETAIL:
                        hdfReceiptMpgPK = (HiddenField)((((Button)sender).Parent).FindControl("hdfReceiptMpgPK"));
                        hdfIsApply = (HiddenField)((((Button)sender).Parent).FindControl("hdfIsApply"));
                        lblTotalAmount = (Label)((((Button)sender).Parent).FindControl("lblTotalAmount"));
                        hdfBaltoReceive = (HiddenField)((((Button)sender).Parent).FindControl("hdfBaltoReceive"));
                        hdfTotalAmtDtl.Value = string.IsNullOrEmpty(lblTotalAmount.Text) ? "0" : lblTotalAmount.Text;
                        hdfBaltoAlloc.Value = string.IsNullOrEmpty(hdfBaltoReceive.Value) ? "0" : hdfBaltoReceive.Value;
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));

                        InvoicePK = Convert.ToInt32(hdfInvoicePK.Value);
                        TrxIndex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;

                        lblCusname.Text = lblCusname.ToolTip = txtCustomer.Text;
                        lblcurrencyname.Text = lblcurrencyname.ToolTip = txtReceiptCurrency.Text;
                        ReceiptMpgPK = Convert.ToInt64(hdfReceiptMpgPK.Value);
                        IsApply = Convert.ToInt32(hdfIsApply.Value);
                        GetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                        decimal adjRcvdAmnt = 0;
                        tempReceiptAdjnList = ReceiptAdjnList;
                        if (tempReceiptAdjnList != null && tempReceiptAdjnList.Count > 0 && ReceiptHeaderSession != null)
                        {
                            List<AdjAllocation> objAdjAlcnList = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK).AdjAllocation.ToList();
                            tempReceiptAdjnList.ForEach(alcn =>
                            {
                                alcn.RAD_RCM_INVOICE_HDR = InvoicePK;
                                alcn.RAD_RECEIPT_TRX = ReceiptMpgPK;
                                alcn.RAD_AMOUNT = 0;
                                adjRcvdAmnt = 0;
                                ReceiptHeaderSession.ReceiptTrxMapping.ForEach(mpg =>
                                {
                                    if (mpg.RCM_INVOICE_HDR != InvoicePK)
                                    {
                                        adjRcvdAmnt += mpg.AdjAllocation.Where(r => r.RAD_NO == alcn.RAD_NO).Sum(s => s.RAD_AMOUNT);
                                    }
                                });
                                alcn.RAD_AMOUNT_RCVD = alcn.RAD_AMOUNT_PREV_RCVD + adjRcvdAmnt;
                                alcn.RAD_AMOUNT_BAL = alcn.RAD_AMOUNT_TOTAL - alcn.RAD_AMOUNT_RCVD;

                                if (objAdjAlcnList != null && objAdjAlcnList.Count > 0)
                                {
                                    AdjAllocation objAlcn = objAdjAlcnList.SingleOrDefault(r => r.RAD_NO == alcn.RAD_NO);
                                    if (objAlcn != null)
                                    {
                                        alcn.RAD_AMOUNT = objAlcn.RAD_AMOUNT;
                                        alcn.RAD_PK = objAlcn.RAD_PK;
                                    }
                                }
                            });
                        }
                        ReceiptAdjnList = tempReceiptAdjnList;
                        SetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                        ////InvoicePOSplitList = PaymentInvMappingDetails.Where(f => f.IVH_PK == InvoicePK).SingleOrDefault().POMpg.ToList();
                        ////ReceiptCrdrList = ReceiptHeaderSession.ReceiptTrxMapping.Where(f => f.RCM_INVOICE_HDR == InvoicePK).SingleOrDefault().CRDRMpg.ToList();
                        //if (hdfReceiptMpgPK != null && !string.IsNullOrEmpty(hdfReceiptMpgPK.Value) && !hdfReceiptMpgPK.Value.Equals("0"))
                        //{
                        //    ReceiptAdjnList = ReceiptHeaderSession.ReceiptTrxMapping.Where(f => f.RCM_INVOICE_HDR == InvoicePK).SingleOrDefault().AdjAllocation.ToList();
                        //    ReceiptAdjnList.ForEach(dtl =>
                        //    {
                        //        dtl.RAD_RCM_INVOICE_HDR = InvoicePK;
                        //    });
                        //}
                        //GetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                        //SetFieldValues(ControlsEnum.RECEIPTCUSADJN);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotalAdjn", "$(document).ready(function(){CalculateTotalAdjn();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("ReceiptAlloc").ToString() + "','800','300');", true);
                        break;
                    #endregion
                    #region ADJN SPLIT SAVE
                    case ActionsEnum.ADJNSPLITSAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (grdReceiptSplitAdjn.Rows.Count >= 1)
                            {
                                Label lblTotalAllocateAdjn = (Label)(grdReceiptSplitAdjn.FooterRow.FindControl("lblTotalAllocateAdjn"));
                                HiddenField hdfTotalAllocateAdjn = (HiddenField)(grdReceiptSplitAdjn.FooterRow.FindControl("hdfTotalAllocateAdjn"));
                                HiddenField hdfBalanceAdjn = (HiddenField)(grdReceiptSplitAdjn.FooterRow.FindControl("hdfBalanceAdjn"));
                                HiddenField hdfReceivedNow = (HiddenField)(grdReceiptSplitAdjn.FooterRow.FindControl("hdfReceivedNow"));
                                if (Convert.ToDecimal(hdfBalanceAdjn.Value) >= Convert.ToDecimal(hdfTotalAllocateAdjn.Value))
                                {
                                    if (Convert.ToDecimal(hdfTotalAmtDtl.Value) >= Convert.ToDecimal(hdfTotalAllocateAdjn.Value))
                                    {
                                        if (Convert.ToDecimal(hdfBaltoAlloc.Value) >= Convert.ToDecimal(hdfTotalAllocateAdjn.Value))
                                        {
                                            divErrorLabel.Visible = false;
                                            FinReceiptCusAllocationList = new List<AdjAllocation>();
                                            FinReceiptCusAllocationList = (List<AdjAllocation>)SetUIValuesToObject(ControlsEnum.ADJNSPLITLIST);
                                            int rowID = 0;
                                            bool isCon = true;
                                            #region check any other trx have already taken the amount or not
                                            //if (FinReceiptCusAllocationList != null && FinReceiptCusAllocationList.Count > 0)
                                            //{
                                            //    List<AdjAllocation> objAdjAlcnList = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK).AdjAllocation.ToList();
                                            //    foreach (AdjAllocation dtl in FinReceiptCusAllocationList)
                                            //    {
                                            //        decimal alreadyalctdAmnt = 0;
                                            //        ReceiptHeaderSession.ReceiptTrxMapping.ForEach(mpg =>
                                            //        {
                                            //            if (mpg.AdjAllocation != null && mpg.AdjAllocation.Count > 0)
                                            //            {
                                            //                alreadyalctdAmnt += mpg.AdjAllocation.Where(r => r.RAD_NO == dtl.RAD_NO && r.RAD_RCM_INVOICE_HDR != dtl.RAD_RCM_INVOICE_HDR).Sum(s => s.RAD_AMOUNT);
                                            //            }

                                            //        });
                                            //        if (((alreadyalctdAmnt + dtl.RAD_AMOUNT) > dtl.RAD_AMOUNT_BAL) && dtl.RAD_AMOUNT > 0)
                                            //        {
                                            //            isCon = false;
                                            //            break;
                                            //        }
                                            //    }
                                            //}                                            
                                            #endregion
                                            if (isCon == true)
                                            {
                                                if (FinReceiptCusAllocationList != null && FinReceiptCusAllocationList.Count > 0)
                                                {
                                                    tempReceiptHeader = ReceiptHeaderSession;
                                                    if (tempReceiptHeader != null)
                                                    {
                                                        List<AdjAllocation> objAdjAlcnList = tempReceiptHeader.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK).AdjAllocation.ToList();
                                                        FinReceiptCusAllocationList.ForEach(dtl =>
                                                        {
                                                            AdjAllocation objDtl = objAdjAlcnList.SingleOrDefault(r => r.RAD_NO == dtl.RAD_NO);
                                                            if (objDtl == null)//new
                                                            {
                                                                objAdjAlcnList.Add(dtl);
                                                            }
                                                            else//update
                                                            {
                                                                if (dtl.RAD_AMOUNT == 0)
                                                                    objAdjAlcnList.Remove(objDtl);
                                                                else
                                                                    objDtl.RAD_AMOUNT = dtl.RAD_AMOUNT;
                                                            }
                                                        });
                                                        tempReceiptHeader.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK).AdjAllocation = objAdjAlcnList;

                                                    }

                                                    if (TrxIndex >= 0)
                                                    {
                                                        Label lblAdjAmount = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblAdjAmount");
                                                        Label BalancetoPay = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblBaltoReceive");
                                                        hdfBaltoReceive = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfBaltoReceive");
                                                        hdfIsApply = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfIsApply");
                                                        hdfInvoicePK = (HiddenField)grdInvoiceList.Rows[TrxIndex].FindControl("hdfInvoicePK");
                                                        txtReceivedNow = (TextBox)grdInvoiceList.Rows[TrxIndex].FindControl("txtReceivedNow");
                                                        lblCrdrAlcnAmount = (Label)grdInvoiceList.Rows[TrxIndex].FindControl("lblCrdrAlcnAmount");

                                                        lblAdjAmount.Text = lblAdjAmount.ToolTip = GetFormattedCurrencyWithComma(hdfTotalAllocateAdjn.Value);
                                                        hdfIsApply.Value = "1";
                                                        if (lblAdjAmount != null && !string.IsNullOrEmpty(lblAdjAmount.Text.Trim()))
                                                        {
                                                            AdjnNowAmount = Convert.ToDecimal(lblAdjAmount.Text.Trim());
                                                            decimal BalPay = Convert.ToDecimal(hdfBaltoReceive.Value.Replace(",", "")) - AdjnNowAmount;
                                                            decimal AmountRec_Old = Convert.ToDecimal(BalancetoPay.Text);
                                                            if (BalPay != AmountRec_Old)//for delete invoice split
                                                            {
                                                                if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) || !hdfInvoicePK.Value.Equals("0"))
                                                                {
                                                                    InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                                                                }
                                                            }
                                                            BalancetoPay.Text = BalancetoPay.ToolTip = String.Format("{0:c}", (BalPay < 0 ? 0 : BalPay));
                                                            txtReceivedNow.Text = txtReceivedNow.ToolTip = Math.Round(BalPay < 0 ? 0 : BalPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString();// AdjnNowAmount > 0 ? Math.Round(BalPay < 0 ? 0 : BalPay, Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyDecimalDigits).ToString() : txtReceivedNow.Text;
                                                        }

                                                        if (tempReceiptHeader != null && tempReceiptHeader.ReceiptTrxMapping != null && tempReceiptHeader.ReceiptTrxMapping.Count > 0)
                                                        {
                                                            ReceiptTrxMapping objTrxMpg = tempReceiptHeader.ReceiptTrxMapping.SingleOrDefault(dtl => dtl.RCM_INVOICE_HDR == InvoicePK);
                                                            if (objTrxMpg != null)
                                                            {
                                                                objTrxMpg.RCM_ADJUST_AMOUNT = Convert.ToDecimal(hdfTotalAllocateAdjn.Value);
                                                                #region Reset Receipt Split after adj apply
                                                                if (objTrxMpg.ReceiptSOMapping != null && objTrxMpg.ReceiptSOMapping.Count > 0)
                                                                {
                                                                    objTrxMpg.ReceiptSOMapping.ForEach(dtl =>
                                                                       {
                                                                           dtl.RSO_RECEIVED_AMOUNT = 0;
                                                                           dtl.RSO_TAX_AMOUNT = 0;
                                                                           dtl.RSO_OTHER_AMOUNT = 0;
                                                                       });
                                                                }
                                                                #endregion

                                                                #region Reset CR/DR Allocation
                                                                if (objTrxMpg.DebitNoteAllocation != null && objTrxMpg.DebitNoteAllocation.Count > 0)
                                                                {
                                                                    objTrxMpg.DebitNoteAllocation.ForEach(dtl =>
                                                                    {
                                                                        dtl.RNM_ADJ_AMOUNT = 0;
                                                                        dtl.RNM_PAID_AMOUNT = 0;
                                                                    });

                                                                    decimal TotalCrdrAmnt = 0;
                                                                    TotalCrdrAmnt = objTrxMpg.DebitNoteAllocation.Sum(r => r.RNM_PAID_AMOUNT + r.RNM_ADJ_AMOUNT);
                                                                    lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", TotalCrdrAmnt);

                                                                }
                                                                #endregion

                                                                #region Reset Receipt SC Allocation
                                                                InvoiceDetails(InvoicePK);
                                                                ReceiptSplitSave(false);
                                                                #endregion
                                                            }
                                                        }
                                                        ReceiptHeaderSession = tempReceiptHeader;
                                                    }
                                                }
                                            }
                                            else
                                            {


                                                divWrongAdj.Visible = true;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("adj_Allocated").ToString() + "','800','300');", true);

                                            }
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                   "ClosePopup();", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);

                                        }
                                        else
                                        {
                                            divBaltoAll.Visible = true;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                                        }
                                    }
                                    else
                                    {
                                        divErrorLabelAdjnTotamt.Visible = true;
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                                    }
                                }
                                else
                                {
                                    divErrorLabelAdjn.Visible = true;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUpAdjn]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                                }
                            }
                            else
                            {

                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                    "ClosePopup();", true);
                            }

                        }
                        break;
                    #endregion
                    #region INVOICE DETAIL
                    case ActionsEnum.INVOICEDETAIL:
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        }
                        else
                            InvoicePK = 0;
                        InvoiceDetails(InvoicePK);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                        break;
                    #endregion
                    #region PAYMENT SPLIT SAVE
                    case ActionsEnum.PAYMENTSPLITSAVE:
                        ReceiptSplitSave(true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                        break;
                    #endregion
                    #region CR/DR ALLOCATION
                    case ActionsEnum.CRDRALLOCATION:
                        divCrdrErrorMsg.Visible = false;
                        FinReceiptCusCrdrMpgList = null;
                        hdfInvoicePK = (HiddenField)((((Button)sender).Parent).FindControl("hdfInvoicePK"));
                        lblTotalAmount = (Label)((((Button)sender).Parent).FindControl("lblTotalAmount"));
                        txtReceivedNow = (TextBox)((((Button)sender).Parent).FindControl("txtReceivedNow"));
                        LinkButton lnkReceived = (LinkButton)((((Button)sender).Parent).FindControl("lnkReceived"));
                        LinkButton lnkInvoiceNo = (LinkButton)((((Button)sender).Parent).FindControl("lnkInvoiceNo"));
                        CrdrRowIndex = ((GridViewRow)((sender as Control)).NamingContainer).RowIndex;
                        if (txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
                        {
                            PayNowAmount = Convert.ToDecimal(txtReceivedNow.Text.Trim());
                        }
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) && !hdfInvoicePK.Value.Equals("0"))
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                        else
                            InvoicePK = 0;

                        if (InvoicePK > 0)
                        {
                            GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
                            lblInvSplitAmount_CrdrAlcn.Text = lblInvSplitAmount_CrdrAlcn.ToolTip = lblTotalAmount.Text;
                            lblInvSplitReceived_CrdrAlcn.Text = lblInvSplitReceived_CrdrAlcn.ToolTip = lnkReceived.Text;
                            lblInvSplitReceiveNow_CrdrAlcn.Text = lblInvSplitReceiveNow_CrdrAlcn.ToolTip = String.Format("{0:c}", PayNowAmount);
                        }
                        SetFieldValues(ControlsEnum.CRDRALLOCATION);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCreditSplitFooter", "$(document).ready(function(){CalculateTotalDebitSplit();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrdrAllocation]','" + GetLocalResourceObject("DebitAllocation").ToString() + "','850','300');", true);
                        break;
                    #endregion
                    #region CR/DR ALLOCATION SAVE
                    case ActionsEnum.CRDRALLOCATIONSAVE:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            if (grdCrdrAllocation.Rows.Count >= 1)
                            {
                                IsDebitNoteApplied = false;
                                divErrorLabel.Visible = false;
                                FinReceiptCusCrdrMpgList = new List<ReceiptCrdrMpg>();
                                FinReceiptCusCrdrMpgList = (List<ReceiptCrdrMpg>)SetUIValuesToObject(ControlsEnum.CRDRSPLITLIST);
                                if (FinReceiptCusCrdrMpgList != null)
                                {
                                    decimal CrAlcnAmount = 0;
                                    decimal TotalCrReceiveNow = 0;
                                    decimal TotalCrAdj = 0;
                                    tempReceiptHeader = ReceiptHeaderSession;
                                    FinReceiptCusCrdrMpgList.ForEach(dtl =>
                                    {
                                        DebitNoteAllocation objDNAlcn = tempReceiptHeader.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK).DebitNoteAllocation.SingleOrDefault(d => d.RNM_CRDR_MPG == dtl.RNM_CRDR_MPG);
                                        objDNAlcn.RNM_ADJ_AMOUNT = dtl.RNM_ADJ_AMOUNT;
                                        objDNAlcn.RNM_PAID_AMOUNT = dtl.RNM_PAID_AMOUNT;
                                        CrAlcnAmount += (dtl.RNM_ADJ_AMOUNT + dtl.RNM_PAID_AMOUNT);
                                        TotalCrReceiveNow += dtl.RNM_PAID_AMOUNT;
                                        TotalCrAdj += dtl.RNM_ADJ_AMOUNT;
                                    });
                                    ReceiptHeaderSession = tempReceiptHeader;
                                    if (CrdrRowIndex >= 0)
                                    {
                                        decimal InvAdjAmnt = 0;
                                        decimal InvReceiveNow = 0;
                                        lblCrdrAlcnAmount = (Label)grdInvoiceList.Rows[CrdrRowIndex].FindControl("lblCrdrAlcnAmount");
                                        Label lblAdjAmount = (Label)grdInvoiceList.Rows[CrdrRowIndex].FindControl("lblAdjAmount");
                                        txtReceivedNow = (TextBox)grdInvoiceList.Rows[CrdrRowIndex].FindControl("txtReceivedNow");
                                        decimal.TryParse(txtReceivedNow.Text, out InvReceiveNow);
                                        if (lblAdjAmount != null)
                                            decimal.TryParse(lblAdjAmount.Text, out InvAdjAmnt);
                                        if (TotalCrReceiveNow > InvReceiveNow)
                                        {
                                            divCrdrErrorMsg.Visible = true;
                                            lblSplitErrorMessage_crdrAllocation.Text = GetLocalResourceObject("Err_ExcessCrReceivenow").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCreditSplitFooter", "$(document).ready(function(){CalculateTotalDebitSplit();});", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrdrAllocation]','" + GetLocalResourceObject("DebitAllocation").ToString() + "','850','300');", true);
                                        }
                                        else if (TotalCrAdj > InvAdjAmnt)
                                        {
                                            divCrdrErrorMsg.Visible = true;
                                            lblSplitErrorMessage_crdrAllocation.Text = GetLocalResourceObject("Err_ExcessCrAdj").ToString();
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalCreditSplitFooter", "$(document).ready(function(){CalculateTotalDebitSplit();});", true);
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divCrdrAllocation]','" + GetLocalResourceObject("DebitAllocation").ToString() + "','850','300');", true);
                                        }
                                        else
                                        {
                                            lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", CrAlcnAmount);
                                            IsDebitNoteApplied = true;
                                            InvoiceDetails(InvoicePK);
                                            ReceiptSplitSave(false);
                                            IsDebitNoteApplied = false;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
                                        }
                                    }
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                }
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            }
                        }
                        break;
                    #endregion
                    #region PAYNOW CHANGE
                    case ActionsEnum.CHANGERECEIVENOW:
                        grvRow = (sender as Button).Parent.Parent as GridViewRow;
                        hdfInvoicePK = (HiddenField)grvRow.FindControl("hdfInvoicePK");
                        lblCrdrAlcnAmount = (Label)grvRow.FindControl("lblCrdrAlcnAmount");
                        if (hdfInvoicePK != null && !string.IsNullOrEmpty(hdfInvoicePK.Value) || !hdfInvoicePK.Value.Equals("0"))
                        {
                            InvoicePK = Convert.ToInt64(hdfInvoicePK.Value);
                            tempReceiptHeader = ReceiptHeaderSession;
                            if (tempReceiptHeader != null && tempReceiptHeader.ReceiptTrxMapping != null && tempReceiptHeader.ReceiptTrxMapping.Count > 0)
                            {
                                ReceiptTrxMapping objTrxMpg = tempReceiptHeader.ReceiptTrxMapping.SingleOrDefault(dtl => dtl.RCM_INVOICE_HDR == InvoicePK);
                                if (objTrxMpg != null)
                                {
                                    #region Reset Trx Mpg Values
                                    taxAmnt = 0;
                                    adjAmnt = 0;
                                    otherAmnt = 0;
                                    receiveAmnt = 0;
                                    reductionAmnt = 0;
                                    hdfInvoicePK = (HiddenField)grvRow.FindControl("hdfInvoicePK");
                                    txtReceivedNow = (TextBox)grvRow.FindControl("txtReceivedNow");
                                    txtAdjustments = (TextBox)grvRow.FindControl("txtAdjustments");
                                    HiddenField hdfTotalTax = (HiddenField)grvRow.FindControl("hdfTotalTax");
                                    TextBox txtOthercharges = (TextBox)grvRow.FindControl("txtOthercharges");
                                    Label lblAdjAmount = (Label)grvRow.FindControl("lblAdjAmount");

                                    if (txtReceivedNow != null)
                                        decimal.TryParse(txtReceivedNow.Text, out receiveAmnt);
                                    objTrxMpg.RCM_RCVD_AMOUNT = receiveAmnt;
                                    if (txtOthercharges != null)
                                        decimal.TryParse(txtOthercharges.Text, out otherAmnt);
                                    objTrxMpg.RCM_OTHER_AMOUNT = otherAmnt;
                                    if (hdfTotalTax != null)
                                        decimal.TryParse(hdfTotalTax.Value, out taxAmnt);
                                    objTrxMpg.RCM_TAX_AMOUNT = taxAmnt;
                                    if (txtAdjustments != null)
                                        decimal.TryParse(txtAdjustments.Text, out reductionAmnt);
                                    objTrxMpg.RCM_ADJUST_AMOUNT = reductionAmnt;
                                    if (lblAdjAmount != null)
                                        decimal.TryParse(lblAdjAmount.Text, out adjAmnt);
                                    objTrxMpg.RCM_ADJUST_AMOUNT = adjAmnt;
                                    #endregion

                                    #region Reset Trx SO Mpg Values
                                    if (objTrxMpg.ReceiptSOMapping != null && objTrxMpg.ReceiptSOMapping.Count > 0)
                                    {
                                        objTrxMpg.ReceiptSOMapping.ToList().ForEach(dtl =>
                                        {
                                            dtl.RSO_RECEIVED_AMOUNT = 0;
                                            dtl.RSO_TAX_AMOUNT = 0;
                                            dtl.RSO_OTHER_AMOUNT = 0;
                                        });
                                    }
                                    #endregion

                                    if (AppliedInvPkList != null && AppliedInvPkList.Count > 0)
                                        AppliedInvPkList.Remove(InvoicePK);

                                    #region Reset CR/DR Allocation

                                    if (objTrxMpg.DebitNoteAllocation != null && objTrxMpg.DebitNoteAllocation.Count > 0)
                                    {
                                        objTrxMpg.DebitNoteAllocation.ForEach(dtl =>
                                        {
                                            dtl.RNM_ADJ_AMOUNT = 0;
                                            dtl.RNM_PAID_AMOUNT = 0;
                                        });

                                        decimal TotalCrdrAmnt = 0;
                                        TotalCrdrAmnt = objTrxMpg.DebitNoteAllocation.Sum(r => r.RNM_PAID_AMOUNT + r.RNM_ADJ_AMOUNT);
                                        lblCrdrAlcnAmount.Text = lblCrdrAlcnAmount.ToolTip = string.Format("{0:c}", TotalCrdrAmnt);

                                    }
                                    #endregion

                                    ReceiptHeaderSession = tempReceiptHeader;
                                }
                            }
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CloseMsgPopup1", "CloseMsgPopup();", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                        break;
                    #endregion
                    #region SAVE
                    // Do Action for , when click save button
                    case ActionsEnum.SAVE:
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        //Checks for Instr No
                        if (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.CHEQUE || (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.BANK))
                        {
                            if (string.IsNullOrEmpty(txtInstrumentNo.Text))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_InstrumentNo").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
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
                            //Checks duplicate entry for Instr No.
                            if (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.CHEQUE || (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.BANK))
                            {
                                if (!string.IsNullOrEmpty(txtInstrumentNo.Text))
                                {
                                    GetFieldValues(ControlsEnum.CHKINSTRNO);
                                    if (InstrNoResult == false)
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("DuplicateInstrumentNo").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                        return;
                                    }
                                }
                            }
                            //
                            if (!IsValidPayment())
                            {
                                return;
                            }
                            else
                            {
                                if (grdInvoiceList.Rows.Count >= 1)
                                {

                                    foreach (GridViewRow gv in grdInvoiceList.Rows)
                                    {
                                        lblBaltoRec = gv.FindControl("lblBaltoReceive") as Label;
                                        TextBox txtReceiveNow = gv.FindControl("txtReceivedNow") as TextBox;
                                        Label lblAdjAmount = gv.FindControl("lblAdjAmount") as Label;
                                        BalReceiveTot = BalReceiveTot + Convert.ToDecimal(lblBaltoRec.Text);
                                        ReceiveNowTot = ReceiveNowTot + Convert.ToDecimal(txtReceiveNow.Text);
                                        TotalAdjAmt = TotalAdjAmt + Convert.ToDecimal(lblAdjAmount.Text);
                                    }
                                }
                                //Receipt Amount is Zero. Voucher not required
                                if (ReceiveNowTot == 0 & TotalAdjAmt == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_AllocAmtZero").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }

                                decimal adjAmt = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                                if (grdInvoiceList.Rows.Count >= 1)
                                {
                                    isContinue = true;
                                    if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
                                    {
                                        if (hdfSaveWithoutBankCharge.Value == "0")
                                        {
                                            double d = 0;
                                            double.TryParse(txtBankCharge.Text, out d);

                                            if (d <= 0 && (BalReceiveTot != 0 && ReceiveNowTot != 0))
                                            {
                                                isContinue = false;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutBankChargeConfirm('" + (sender as Button).ID + "');", true);
                                            }
                                        }
                                    }
                                    if (isContinue)
                                    {
                                        int zeroCount = 0;
                                        bool IsValidPayNow = true;
                                        //chek bal pmt
                                        foreach (GridViewRow gv in grdInvoiceList.Rows)
                                        {

                                            lblBaltoRec = gv.FindControl("lblBaltoReceive") as Label;
                                            TextBox txtReceiveNow = gv.FindControl("txtReceivedNow") as TextBox;
                                            if (Convert.ToDecimal(lblBaltoRec.Text.Replace(",", "")) > 0 && Convert.ToDecimal(txtReceiveNow.Text.Replace(",", "")) == 0)
                                            {
                                                IsValidPayNow = false;
                                                break;
                                            }
                                            if (Convert.ToDecimal(lblBaltoRec.Text.Replace(",", "")) < Convert.ToDecimal(txtReceiveNow.Text.Replace(",", "")))
                                            {
                                                zeroCount = zeroCount + 1;
                                            }
                                            //if (Convert.ToDecimal(lblBaltoRec.Text) == Convert.ToDecimal(0))
                                            //{
                                            //    zeroCount = zeroCount + 1;
                                            //}
                                        }
                                        if (!IsValidPayNow)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptAmount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                        if ((zeroCount <= 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                                        {
                                            Iscont = false;

                                            if (adjAmt > 0 && ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_AdjType").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                            else
                                            {
                                                //finReceiptCusHdrList = new List<FIN_RECEIPT_CUS_HDR>();
                                                //salesReceiptServiceClient = new SalesReceiptService();
                                                //salesReceiptServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(salesReceiptServiceClient);
                                                //finReceiptCusHdrObj = CommonFunctions.Initilize<FIN_RECEIPT_CUS_HDR>();
                                                receiptHeaderObj = (ReceiptHeader)SetUIValuesToObject(ControlsEnum.RECEIPTHEADER);
                                                if (receiptHeaderObj != null)
                                                {
                                                    receiptHeaderObj.WKF_TRX_FLAG = Convert.ToInt32(WorkflowTransactionFlag.SAVE);
                                                    TypeRef = receiptHeaderObj.RCH_NO;
                                                    receiptMpgCount = 0;
                                                    receiptMpgCount = receiptHeaderObj.ReceiptTrxMapping.ToList().Count;
                                                    if (receiptMpgCount > 0)
                                                    {
                                                        //if (CheckReceipt(receiptHeaderObj))
                                                        //{
                                                        if (isSplitApply != 5)
                                                        {
                                                            bool isCont = true;

                                                            if (Convert.ToInt16(hdfRecStatus.Value) > 0)
                                                            {
                                                                if (hdfEditForReturn.Value != "1")//If Edit For Return ,no need to check record have journal entry.
                                                                {
                                                                    if (Convert.ToInt16(hdfRecWKFStatus.Value) == 0)
                                                                    {

                                                                        isCont = true;
                                                                        GenDummy = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        isCont = false;
                                                                        litErrorMsg.Text = GetLocalResourceObject("Err_SR_Cancel").ToString();
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.ErpRes.Information + "');", true);
                                                                        EntryStatus = EntryStatus.LISTMODE;
                                                                        ResetForm();
                                                                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                                        btnNew.Focus();
                                                                        break;
                                                                    }
                                                                }

                                                                foreach (ReceiptTrxMapping trxObj in receiptHeaderObj.ReceiptTrxMapping)
                                                                {
                                                                    ICH_PK = trxObj.RCM_INVOICE_HDR;
                                                                    GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                                                                    if (ObjFinInvoiceCusHdrList != null && ObjFinInvoiceCusHdrList.Count > 0 && ObjFinInvoiceCusHdrList[0].ICH_IS_OPENING == 1)
                                                                    {
                                                                        isCont = true;
                                                                    }
                                                                    else
                                                                    {
                                                                        TotalDebitAmnt = 0;
                                                                        if (trxObj.RCM_RCVD_AMOUNT > 0)
                                                                        {
                                                                            if (trxObj.DebitNoteAllocation != null && trxObj.DebitNoteAllocation.Count > 0)
                                                                            {
                                                                                TotalDebitAmnt = trxObj.DebitNoteAllocation.Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                                                                                if ((trxObj.RCM_RCVD_AMOUNT + trxObj.RCM_ADJUST_AMOUNT) - TotalDebitAmnt <= 0)
                                                                                {
                                                                                    isCont = true;
                                                                                    break;
                                                                                }
                                                                            }
                                                                            if (trxObj.ReceiptSOMapping != null && trxObj.ReceiptSOMapping.Count > 0)
                                                                            {
                                                                                if (trxObj.ReceiptSOMapping.Sum(dtl => dtl.RSO_RECEIVED_AMOUNT) == 0)
                                                                                {
                                                                                    isCont = false;
                                                                                    break;
                                                                                }
                                                                                if (trxObj.ReceiptSOMapping.Sum(dtl => dtl.RSO_RECEIVED_AMOUNT) > ((trxObj.RCM_RCVD_AMOUNT + trxObj.RCM_ADJUST_AMOUNT) - TotalDebitAmnt))
                                                                                {
                                                                                    isCont = false;
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }
                                                                    }
                                                                }


                                                            }
                                                            if (isCont)
                                                            {
                                                                string ReceiptNo = string.Empty;
                                                                if (lstReceiptSuspendDetails != null && lstReceiptSuspendDetails.Count > 0)
                                                                    receiptHeaderObj.receiptSuspendDetails = lstReceiptSuspendDetails;
                                                                string xmlDoc = CommonFunctions.XmlSerialize<ReceiptHeader>(receiptHeaderObj);
                                                                result = BusinessLogic.Sales.ReceiptBL.SaveReceipt(xmlDoc, out ReceiptNo);
                                                                if (result > 0)
                                                                {
                                                                    CurrPK = (int)result;
                                                                    #region Generate dummy entry while modify receipt after approval
                                                                    if (GenDummy == true)
                                                                    {
                                                                        FinTrxService finTrxServiceClient;
                                                                        finTrxServiceClient = new FinTrxService();
                                                                        string refType = "";
                                                                        if (recipetType == 0 || recipetType == 1)
                                                                        {
                                                                            refType = ApplicationType.CRTJ;

                                                                        }
                                                                        else if (recipetType == 3)
                                                                        {
                                                                            refType = ApplicationType.MSIRTJ;
                                                                        }
                                                                        bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)result, 0);
                                                                        if (IsDummyEntry == true)
                                                                        {
                                                                            DummyResult = finTrxServiceClient.DeleteFinTrx(refType, (int)result, 0);
                                                                        }
                                                                        else
                                                                        {
                                                                            DummyResult = (int)result;
                                                                        }
                                                                        if (DummyResult > 0)
                                                                        {
                                                                            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                                                            DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                                                            finTrxServiceClient = null;
                                                                        }
                                                                    }
                                                                    #endregion

                                                                    if (string.IsNullOrEmpty(lblReceiptNo.Text.Trim()) || lblReceiptNo.Text.Trim() == Resources.Messages.DocGenerationNew)
                                                                    {
                                                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Saved_Success").ToString();
                                                                    }
                                                                    else
                                                                    {
                                                                        litErrorMsg.Text = GetLocalResourceObject("Msg_Save_Success").ToString();
                                                                        object[] args = new object[2];
                                                                        args[0] = Resources.PageNameRes.ReceiptTrading;
                                                                        args[1] = lblReceiptNo.Text.Trim();
                                                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                                                                    }

                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                                                    EntryStatus = EntryStatus.LISTMODE;
                                                                    ResetForm();
                                                                    GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                                                    SetFieldValues(ControlsEnum.RECEIPTHDRLIST);

                                                                }
                                                                else
                                                                {
                                                                    if (result == (int)DbSaveStatus.SQLERROR)
                                                                    {
                                                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                                    }
                                                                    else if (result == (int)DbSaveStatus.CONCURRENCY)
                                                                    {
                                                                        litErrorMsg.Text = Resources.PageNameRes.ReceiptTrading + " " + Resources.Messages.EditUsedByAnotherUser;
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                                    }
                                                                    else if (result == (int)DbSaveStatus.CODEEXIST)
                                                                    {
                                                                        litErrorMsg.Text = Resources.PageNameRes.ReceiptTrading + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                        + "','" + Resources.ErpRes.Information + "');", true);
                                                                    }
                                                                    else
                                                                    {
                                                                        litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                                                        litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ReceiptTrading);
                                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                                                            + "','" + Resources.ErpRes.Information + "');", true);
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_allocation_tally").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                            }
                                                        }
                                                        else
                                                        {

                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                                            litErrorMsg.Text = GetLocalResourceObject("SplitMand").ToString();
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                                                            break;
                                                        }


                                                        //}
                                                        //else
                                                        //{
                                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptAmount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Iscont = true;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaid", "$(document).ready(function(){ShowAlreadyPaid();});", true);

                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SAVESUBMIT
                    case ActionsEnum.SAVESUBMIT:
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        //Checks for Instr No
                        if (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.CHEQUE || (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.BANK))
                        {
                            if (string.IsNullOrEmpty(txtInstrumentNo.Text))
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Err_InstrumentNo").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                return;
                            }
                        }


                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                        if (EntryStatus == EntryStatus.VIEWMODE)
                        {
                            ucrWrkf.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        }
                        else
                        {
                            //Check for duplicate Instr No
                            if (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.CHEQUE || (Convert.ToInt32(ddlMode.SelectedValue) == (int)ReceiptModeEnum.BANK))
                            {
                                GetFieldValues(ControlsEnum.CHKINSTRNO);
                                if (InstrNoResult == false)
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("DuplicateInstrumentNo").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                            }
                            //

                            //Show WorkFlow Popup
                            if (!IsValid)
                            {
                                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            }
                            else//valid
                            {
                                if (grdInvoiceList.Rows.Count >= 1)
                                {

                                    foreach (GridViewRow gv in grdInvoiceList.Rows)
                                    {
                                        lblBaltoRec = gv.FindControl("lblBaltoReceive") as Label;
                                        TextBox txtReceiveNow = gv.FindControl("txtReceivedNow") as TextBox;
                                        Label lblAdjAmount = gv.FindControl("lblAdjAmount") as Label;
                                        BalReceiveTot = BalReceiveTot + Convert.ToDecimal(lblBaltoRec.Text);
                                        ReceiveNowTot = ReceiveNowTot + Convert.ToDecimal(txtReceiveNow.Text);
                                        TotalAdjAmt = TotalAdjAmt + Convert.ToDecimal(lblAdjAmount.Text);
                                    }
                                }
                                // Receipt Amount is Zero. Voucher not required
                                if (ReceiveNowTot == 0 & TotalAdjAmt == 0)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_AllocAmtZero").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                    return;
                                }
                                decimal adjAmt = txtAdjAmount.Text != string.Empty ? Convert.ToDecimal(txtAdjAmount.Text) : 0;
                                if (grdInvoiceList.Rows.Count >= 1)
                                {
                                    isContinue = true;
                                    if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
                                    {
                                        if (hdfSaveWithoutBankCharge.Value == "0")
                                        {
                                            double d = 0;
                                            double.TryParse(txtBankCharge.Text, out d);
                                            if (d <= 0 && (BalReceiveTot != 0 && ReceiveNowTot != 0))
                                            {
                                                isContinue = false;
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop3", "ShowSaveWithoutBankChargeConfirm('" + (sender as Button).ID + "');", true);
                                            }
                                        }
                                    }
                                    if (isContinue)
                                    {

                                        int zeroCount = 0;
                                        bool IsValidPayNow = true;
                                        //chek bal pmt
                                        foreach (GridViewRow gv in grdInvoiceList.Rows)
                                        {

                                            lblBaltoRec = gv.FindControl("lblBaltoReceive") as Label;
                                            TextBox txtReceiveNow = gv.FindControl("txtReceivedNow") as TextBox;
                                            if (Convert.ToDecimal(lblBaltoRec.Text.Replace(",", "")) > 0 && Convert.ToDecimal(txtReceiveNow.Text.Replace(",", "")) == 0)
                                            {
                                                IsValidPayNow = false;
                                                break;
                                            }
                                            if (Convert.ToDecimal(lblBaltoRec.Text.Replace(",", "")) < Convert.ToDecimal(txtReceiveNow.Text.Replace(",", "")))
                                            {
                                                zeroCount = zeroCount + 1;
                                            }
                                        }
                                        if (!IsValidPayNow)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptAmount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            return;
                                        }
                                        if ((zeroCount <= 0) || (Iscont == true && (hdfIscontYes.Value == "1")))
                                        {
                                            Iscont = false;


                                            if (adjAmt > 0 && ddlAdjType.SelectedValue == CommonConstants.SELECTVAL)
                                            {
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_AdjType").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            }
                                            else
                                            {
                                                bool tally = true;
                                                receiptHeaderObj = (ReceiptHeader)SetUIValuesToObject(ControlsEnum.RECEIPTHEADER);
                                                if (hdfExchangeCurrBC.Value != "-1")
                                                {
                                                    if (receiptHeaderObj != null)
                                                    {
                                                        //if (CheckReceipt(receiptHeaderObj))
                                                        //{
                                                        TypeRef = receiptHeaderObj.RCH_NO;
                                                        receiptMpgCount = 0;
                                                        receiptMpgCount = receiptHeaderObj.ReceiptTrxMapping.ToList().Count;
                                                        if (receiptMpgCount > 0)
                                                        {
                                                            if (SOGroup == SalesInvoiceGroup.Miscellaneous)
                                                            {
                                                                hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ONE;
                                                                ucrWrkf.Visible = true;
                                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                                                            }
                                                            else
                                                            {
                                                                foreach (ReceiptTrxMapping trxObj in receiptHeaderObj.ReceiptTrxMapping)
                                                                {
                                                                    ICH_PK = trxObj.RCM_INVOICE_HDR;
                                                                    GetFieldValues(ControlsEnum.INVOICEHDRBYPK);
                                                                    if (ObjFinInvoiceCusHdrList != null && ObjFinInvoiceCusHdrList.Count > 0 && ObjFinInvoiceCusHdrList[0].ICH_IS_OPENING == 1)
                                                                    {
                                                                        tally = true;
                                                                    }
                                                                    else
                                                                    {


                                                                        TotalDebitAmnt = 0;
                                                                        if ((trxObj.RCM_RCVD_AMOUNT > 0) || ((zeroCount <= 0) || (Iscont == true && (hdfIscontYes.Value == "1"))))
                                                                        {
                                                                            if (trxObj.DebitNoteAllocation != null && trxObj.DebitNoteAllocation.Count > 0)
                                                                            {
                                                                                TotalDebitAmnt = trxObj.DebitNoteAllocation.Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                                                                                if ((trxObj.RCM_RCVD_AMOUNT + trxObj.RCM_ADJUST_AMOUNT) - TotalDebitAmnt <= 0)
                                                                                {
                                                                                    tally = true;
                                                                                    break;
                                                                                }
                                                                            }

                                                                            if ((trxObj.ReceiptSOMapping != null && trxObj.ReceiptSOMapping.Count > 0))
                                                                            {
                                                                                //if ((trxObj.RCM_RCVD_AMOUNT - TotalDebitAmnt) != trxObj.FIN_RECEIPT_CUS_SO_MPG.Sum(dtl => dtl.RSO_RECEIVED_AMOUNT))
                                                                                //{
                                                                                //    tally = false;
                                                                                //    break;
                                                                                //}
                                                                                if (trxObj.ReceiptSOMapping.Sum(dtl => dtl.RSO_RECEIVED_AMOUNT) > ((trxObj.RCM_RCVD_AMOUNT + trxObj.RCM_ADJUST_AMOUNT) - TotalDebitAmnt))
                                                                                {
                                                                                    tally = false;
                                                                                    break;
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                tally = false;
                                                                                break;
                                                                            }
                                                                        }
                                                                        else// if (paymentTrxObj.PVM_PAID_AMOUNT <= 0)
                                                                        {
                                                                            tally = false;
                                                                            break;
                                                                        }
                                                                    }
                                                                }

                                                                if (tally)
                                                                {

                                                                    ucrWrkf.Visible = true;
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                                                                }
                                                                else
                                                                {
                                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("msg_allocation_tally").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptAmount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                        }
                                                        //}
                                                        //else
                                                        //{
                                                        //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                                        //}
                                                    }
                                                }
                                                else
                                                {
                                                    litErrorMsg.Text = Resources.Messages.Msg_ErrConversionFactor;
                                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                }

                                            }
                                        }
                                        else
                                        {
                                            Iscont = true;
                                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowAlreadyPaidWKF", "$(document).ready(function(){ShowAlreadyPaidWKF();});", true);

                                        }
                                    }
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_InvoiceCount").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                }

                            }
                        }
                        break;
                    #endregion
                    #region SUBMIT
                    case ActionsEnum.SUBMIT:
                        hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
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
                            if (!IsValidPayment() && (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE))
                            {
                                return;
                            }

                            ucrWrkf.ApplicationID = 0;
                            if (hdfIsSaveSubmit.Value == CommonConstants.SELECT_VALUE_ONE)
                            {
                                receiptHeaderObj = (ReceiptHeader)SetUIValuesToObject(ControlsEnum.RECEIPTHEADER);
                                if (lstReceiptSuspendDetails != null && lstReceiptSuspendDetails.Count > 0)
                                    receiptHeaderObj.receiptSuspendDetails = lstReceiptSuspendDetails;
                                if (hdfExchangeCurrBC.Value != "-1")
                                {
                                    if (receiptHeaderObj != null)
                                    {
                                        receiptMpgCount = 0;
                                        receiptMpgCount = receiptHeaderObj.ReceiptTrxMapping.ToList().Count;
                                        if (receiptMpgCount > 0)
                                        {
                                            //if (CheckReceipt(finReceiptCusHdrObj))
                                            //{
                                            if (isSplitApply != 5)
                                            {
                                                receiptHeaderObj.WKF_FLAG = 1;
                                                receiptHeaderObj.ATL_ACTION = (byte)LogAction.NEW;
                                                SaveTransaction(receiptHeaderObj, Convert.ToInt32(WorkflowTransactionFlag.SAVEANDSUBMIT));
                                            }
                                            else
                                            {
                                                litErrorMsg.Text = GetLocalResourceObject("SplitMand").ToString();
                                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                                break;
                                            }

                                            //}
                                            //else
                                            //{
                                            //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(GetLocalResourceObject("Err_Save_ReceiptDuplicate").ToString()) + "','" + Resources.Messages.Information + "');", true);
                                            //}
                                        }
                                    }
                                }
                            }
                            else if (hdfIsSaveSubmit.Value == CommonConstants.SELECTVAL)
                            {
                                if (BusinessLogic.CommonManagement.CommonBL.ValidationForCancellation((int)CurrPK, ApplicationType.CRT))
                                {
                                    ucrWrkf.ApplicationID = (int)CurrPK;
                                    isCancelled = true;
                                    SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Err_SR_Cancel").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                    TextBox WrkfComments;
                                    WrkfComments = (TextBox)ucrWrkf.FindControl("WrkfComments");
                                    FillProcessID(1);
                                    hdfIsSaveSubmit.Value = CommonConstants.SELECT_VALUE_ZERO;
                                    WrkfComments.Text = "";
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                    SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                    btnNew.Focus();
                                }
                            }
                            else
                                SaveTransaction(null, Convert.ToInt32(WorkflowTransactionFlag.SUBMIT));

                        }
                        break;
                    #endregion
                    #region REMOVE
                    case ActionsEnum.REMOVE:
                        SetFieldValues(ControlsEnum.UPDATEGRIDVALTOOBJECT);
                        grvRow = ((Button)sender).Parent.Parent as GridViewRow;
                        HiddenField hdfInvoice = grvRow.FindControl("hdfInvoicePK") as HiddenField;
                        if (ReceiptHeaderSession != null
                            && ReceiptHeaderSession.ReceiptTrxMapping != null
                            && ReceiptHeaderSession.ReceiptTrxMapping.Count > 0
                            && !string.IsNullOrEmpty(hdfInvoice.Value))
                        {
                            tempReceiptHeader = ReceiptHeaderSession;
                            ReceiptTrxMapping objTrxMpg = tempReceiptHeader.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == Convert.ToInt64(hdfInvoice.Value));
                            tempReceiptHeader.ReceiptTrxMapping.Remove(objTrxMpg);
                            ReceiptHeaderSession = tempReceiptHeader;
                            SetFieldValues(ControlsEnum.RECEIPTMPGLIST);
                        }
                        break;
                    #endregion
                    #region DELETE
                    case ActionsEnum.DELETE:
                        if (CurrPK > 0)
                        {
                            result = 0;
                            result = BusinessLogic.Sales.ReceiptBL.DeleteReceipt(CurrPK, currentUser.PKUser, LastModifiedTime, ApplicationType.CRT);
                            if (result > 0) // Success ! re-initialize the page
                            {
                                //Show Save success message and reset Contract Entry
                                litErrorMsg.Text = Resources.ErrorMessages.Msg_Delete_Success;
                                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ReceiptTrading);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + litErrorMsg.Text + "','" + Resources.Messages.Information + "');", true);
                                EntryStatus = EntryStatus.LISTMODE;
                                ResetForm();
                                GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
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
                                    litErrorMsg.Text = Resources.PageNameRes.ReceiptTrading + " " + Resources.Messages.EditUsedByAnotherUser;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                    SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                }
                                else if (result == (int)DbDeleteStatus.REFERRED)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ReceiptTrading + " " + Resources.Messages.UsedInAnotherPlace;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                    + "','" + Resources.ErpRes.Information + "');", true);
                                }
                                else if (result == (int)DbDeleteStatus.DELETECONCURRENCY)
                                {
                                    litErrorMsg.Text = Resources.PageNameRes.ReceiptTrading + " " + Resources.Messages.AlreadyDeleted;
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                    EntryStatus = EntryStatus.LISTMODE;
                                    ResetForm();
                                    GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                    SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                                }
                                else
                                {
                                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ReceiptTrading);
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                                        + "','" + Resources.ErpRes.Information + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region RECEIPT LIST
                    case ActionsEnum.RECEIPTLIST:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region ITEM SELECTED
                    case ActionsEnum.ITEMSELECTED:
                        GridViewRow gvr;
                        HiddenField hdfDept;
                        int dept;
                        HiddenField hdfReceiptID;
                        HiddenField hdfPosted;
                        HiddenField hdfDelStatus;
                        bool posted;
                        int pk;
                        gvr = ((RadioButton)sender).Parent.Parent as GridViewRow;
                        hdfReceiptID = gvr.FindControl("hdfReceiptID") as HiddenField;
                        HiddenField hdfRcptStauts = gvr.FindControl("hdfRcptStatus") as HiddenField;
                        HiddenField hdfReceiptStatus = gvr.FindControl("hdfReceiptStatus") as HiddenField;
                        HiddenField hdfSalesInvoiceCategory = gvr.FindControl("hdfSalesInvoiceCategory") as HiddenField;
                        HiddenField hdfReturnStatus = gvr.FindControl("hdfReturnStatus") as HiddenField;
                        hdfShowCancel.Value = hdfReceiptStatus.Value;
                        recipetType = Convert.ToInt32(hdfRcptStauts.Value);
                        hdfCurrStatus.Value = hdfReceiptStatus.Value;
                        if (hdfReceiptID != null && int.TryParse(hdfReceiptID.Value, out pk))
                        {
                            CurrPK = pk;
                        }
                        hdfDept = gvr.FindControl("hdfDept") as HiddenField;
                        if (hdfDept != null && int.TryParse(hdfDept.Value, out dept))
                        {
                            Session[BusinessObject.Common.SessionStrings.CurDept] = dept;
                            base.SetUserDept();
                        }

                        hdfShowPDC.Value = "0";
                        hdfShowChequeReturn.Value = "1";
                        hdfPDCStatusWKF.Value = "0";
                        hdfPosted = gvr.FindControl("hdfPosted") as HiddenField;
                        hdfDelStatus = gvr.FindControl("hdfDelStatus") as HiddenField;
                        if (Convert.ToInt16(hdfDelStatus.Value) == 1)
                        {
                            btnEditforCancel.Visible = false;
                            btnSave.Visible = false;
                            btnEdit.Visible = false;
                            IsDeleted = true;

                        }
                        else
                        {
                            btnEditforCancel.Visible = true;
                            btnSave.Visible = true;
                            IsDeleted = false;
                        }
                        if (hdfPosted != null && bool.TryParse(hdfPosted.Value, out posted))
                        {
                            if (posted)
                            {
                                HiddenField hdfPDC;
                                HiddenField hdfMode;
                                int pdc = 0;


                                int payMode = 0;
                                bool app = ucrWrkf.IsWkfCompleted;
                                hdfPDC = gvr.FindControl("hdfPDC") as HiddenField;
                                hdfMode = gvr.FindControl("hdfMode") as HiddenField;
                                GetFieldValues(ControlsEnum.FINHEADERSTATUS);

                                btnReturn.Visible = false;
                                btnReturnDetail.Visible = false;

                                if (finTrxHdrList != null && finTrxHdrList.Count == 1)
                                {
                                    if (hdfPDC != null && int.TryParse(hdfPDC.Value, out pdc))
                                        if (finTrxHdrList[0].FTH_STATUS == 2)
                                        {
                                            hdfShowPDC.Value = pdc.ToString();
                                        }
                                        else
                                            hdfShowPDC.Value = "0";
                                    else
                                        hdfShowPDC.Value = "0";
                                }

                                if (hdfMode != null && int.TryParse(hdfMode.Value, out payMode))
                                {
                                    if (payMode == (int)PaymentModeEnum.Cheque)
                                    {
                                        if (hdfPDC != null && int.TryParse(hdfPDC.Value, out pdc))

                                            hdfShowChequeReturn.Value = pdc.ToString();
                                        else
                                            hdfShowChequeReturn.Value = "1";

                                        if (pdc == 0 && finTrxHdrList != null && finTrxHdrList.Count > 0 && finTrxHdrList[0].FTH_STATUS == 2)
                                        {
                                            btnReturn.Visible = true;
                                            btnReturnDetail.Visible = true;
                                        }

                                    }
                                    else
                                    {
                                        hdfShowChequeReturn.Value = "1";
                                    }
                                }
                                else
                                {
                                    hdfShowChequeReturn.Value = "1";
                                }

                                //show 'return' only after pdc is completed
                                recipetTypePDC = 4;



                                GetFieldValues(ControlsEnum.FINHEADERSTATUSREVERSE);
                                if (finTrxHdrPDCList != null && finTrxHdrPDCList.Count == 1)
                                {
                                    if (finTrxHdrPDCList[0].FTH_STATUS == 2)
                                    {
                                        btnReturn.Visible = true;
                                        btnReturnDetail.Visible = true;
                                    }

                                }
                            }
                        }
                        workflowCore = new WorkflowCore.CoreService();
                        base.WkfRefID = workflowCore.GetRefID((int)CurrPK, PageProcessID);
                        //btnReverse.Visible = BusinessLogic.CommonManagement.CommonManagement.GetInitialTaskPermission(currentUser.PKUser, PageProcessID);                    
                        if (Convert.ToInt16(hdfSalesInvoiceCategory.Value) == 2 && Convert.ToInt16(hdfDelStatus.Value) != 1)//Show Return receipt button only for advance invoice receipt && it should not be in cancelled status.(1--Invoice,2--Adv.Invoice)  
                        {
                            btnEditForReturn.Visible = true;
                        }
                        else
                        {
                            btnEditForReturn.Visible = false;
                        }
                        CheckReceiptAllocated(Convert.ToInt16(hdfSalesInvoiceCategory.Value), Convert.ToInt16(hdfReturnStatus.Value)); //To check receipt is allocated in Invoice or not (For setting receipt return region visibility)
                        break;
                    #endregion
                    #region SEARCH
                    case ActionsEnum.SEARCH:
                        this.PageIndex = "1";
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region CLEAR
                    case ActionsEnum.CLEAR:
                        ResetForm();
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region CANCEL
                    case ActionsEnum.CANCEL:
                        FillProcessID(1);
                        ResetForm();
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        this.btnNew.Focus();
                        EntryStatus = EntryStatus.LISTMODE;
                        break;
                    #endregion
                    #region EDIT/RECEIPT DETAIL/VIEW
                    case ActionsEnum.EDIT:
                    case ActionsEnum.RECEIPTDETAIL:
                    case ActionsEnum.VIEW:
                        FillProcessID(1);
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        SetReceiptMode();
                        hdfIsPendingInvVisible.Value = "0";
                        break;
                    #endregion
                    #region SEARCH ACCOUNT NO
                    case ActionsEnum.SEARCHACCOUNTNO:
                        //if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH && !string.IsNullOrEmpty(hdfBank.Value) && Convert.ToInt32(hdfBank.Value.ToString()) > 0)
                        if (!string.IsNullOrEmpty(hdfBank.Value) && Convert.ToInt32(hdfBank.Value.ToString()) > 0)
                        {
                            GetFieldValues(ControlsEnum.BANK);
                            if (finCashBankMstList != null && finCashBankMstList.Count > 0)
                            {
                                if (Convert.ToInt32(ddlMode.SelectedValue) != (int)ReceiptModeEnum.CASH)
                                {
                                    txtAccountNo.Text = finCashBankMstList[0].CBM_ACC_NO;
                                    txtBranch.Text = finCashBankMstList[0].CBM_BRANCH;
                                }
                                hdfBankAccount.Value = finCashBankMstList[0].CBM_ACCOUNT.ToString();
                            }
                            else
                            {
                                txtAccountNo.Text = string.Empty;
                                txtBranch.Text = string.Empty;
                                hdfBankAccount.Value = string.Empty;
                            }
                        }
                        else
                        {
                            txtAccountNo.Text = string.Empty;
                            txtBranch.Text = string.Empty;
                            hdfBankAccount.Value = string.Empty;
                        }
                        break;
                    #endregion
                    #region PAYMENT MODE CHANGE
                    case ActionsEnum.PAYMENT_MODE_INDEX_CHANGED:
                        ReceiptModeChanged();
                        break;

                    #endregion
                    #region EXCHANGE RATE
                    case ActionsEnum.EXCHANGERATE: //"EXCHANGERATE":
                        if (!string.IsNullOrEmpty(hdfReceiptCurrency.Value))
                        {
                            GetFieldValues(ControlsEnum.EXCHANGERATE);
                            double exchangeRate = string.IsNullOrEmpty(hdfExchangeCurr.Value) ? 1 : Convert.ToDouble(hdfExchangeCurr.Value);
                            double receivedAmount = string.IsNullOrEmpty(txtReceivedAmount.Text.Trim()) ? 0 * exchangeRate : Convert.ToDouble(txtReceivedAmount.Text.Trim()) * exchangeRate;
                            txtReceivedAmount.Text = receivedAmount.ToString();
                        }
                        break;
                    #endregion
                    #region CHECK AMT in detail Grid
                    case ActionsEnum.CHECKAMT:
                        //bool isValidQty = true;
                        //double maxQty = 0;

                        //Label lblAmount;
                        //lblAmount = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblAmount") as Label);


                        //Label lblReversed;
                        //lblReversed = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("lblReversed") as Label);


                        TextBox txtReceivedNowG;
                        txtReceivedNowG = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtReceivedNow") as TextBox);

                        txtAdjustments = (((sender as TextBox).Parent.Parent as GridViewRow).FindControl("txtAdjustments") as TextBox);



                        if (grdInvoiceList.Rows.Count > 0 || grdInvoiceList != null)
                        {
                            if (txtReceivedNowG != null && txtReceivedNowG.Text != "" && txtAdjustments != null && txtAdjustments.Text != "")
                            {


                                if (Convert.ToDouble(txtAdjustments.Text) > Convert.ToDouble(txtReceivedNowG.Text))
                                {
                                    txtAdjustments.Text = "0.00";
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_RecQty").ToString();
                                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Convert.ToDouble(txtReceivedNowG.Text));
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                                //else
                                //{ litErrorMsg.Text = GetLocalResourceObject("Msg_InvoiceQty_Zero").ToString(); }

                            }
                        }




                        break;
                    #endregion
                    #region Journalize
                    case ActionsEnum.JOURNALIZE:
                        hdfJournalizeWorkFlow.Value = "0";
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
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region Journalize/Reverse/Return Save
                    case ActionsEnum.REVERSESAVE:
                    case ActionsEnum.JOURNALIZESAVE:
                    case ActionsEnum.RETURNSAVE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region Journalize/Reverse/Return Cancel
                    case ActionsEnum.REVERSECANCEL:
                    case ActionsEnum.JOURNALIZECANCEL:
                    case ActionsEnum.RETURNCANCEL:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region Journalize Submit
                    case ActionsEnum.JOURNALIZESUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region Journalize Delete
                    case ActionsEnum.JOURNALIZEDELETE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region REVERSE
                    case ActionsEnum.REVERSE:
                        GetFieldValues(ControlsEnum.RECEIPTHEADER);
                        if (ReceiptHeaderSession != null)
                        {
                            if (ReceiptHeaderSession.RCH_PDC >= 1)
                            {
                                if (ReceiptHeaderSession.RCH_HAS_JRNL_ENTRY)
                                {
                                    if (ReceiptHeaderSession.RCH_PDC == 1)
                                    {
                                        #region Update dummy entry
                                        FinTrxService finTrxServiceClient;
                                        finTrxServiceClient = new FinTrxService();
                                        string refType = string.Empty;
                                        refType = ApplicationType.PDCCTJ;
                                        long DummyResults = (long)CurrPK;
                                        bool IsDummyEntry = finTrxServiceClient.IsDummyEntry(refType, (int)CurrPK, 0);
                                        if (IsDummyEntry == true)
                                        {
                                            DummyResults = finTrxServiceClient.DeleteFinTrx(refType, (int)CurrPK, 0);
                                        }
                                        if (DummyResults > 0)
                                        {
                                            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                            DummyResult = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, refType);
                                        }
                                        #endregion
                                    }
                                    SetUIValuesToObject(ControlsEnum.REVERSE);
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("MsgErr_ReverseEntry_Not_Journalized").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                }
                            }
                            else
                            {
                                litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReverseEntry_Not_PDC").ToString();
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                break;
                            }
                        }
                        break;
                    #endregion
                    #region CHEQUE RETURN
                    case ActionsEnum.CHEQUERETURN:
                        //Check if adv deducted
                        GetFieldValues(ControlsEnum.ISADVDEDUCTED);
                        if (IsContReturn == true)
                        {
                            GetFieldValues(ControlsEnum.RECEIPTHEADER);
                            if (ReceiptHeaderSession != null)
                            {
                                if (ReceiptHeaderSession.RCH_PDC != 1)
                                {
                                    if (ReceiptHeaderSession.RCH_HAS_JRNL_ENTRY)
                                    {
                                        if (ReceiptHeaderSession.RCH_BOUNCED == 0)
                                        {
                                            FinTrxService finTrxServiceClient;
                                            finTrxServiceClient = new FinTrxService();
                                            finTrxServiceClient = ERP.Utilities.CommonFunctions.InitiateClient(finTrxServiceClient);
                                            result = finTrxServiceClient.GenerateDummyEntry((int)CurrPK, ApplicationType.RCBTJ);
                                        }
                                        SetUIValuesToObject(ControlsEnum.CHEQUERETURN);
                                    }
                                    else
                                    {
                                        litErrorMsg.Text = GetLocalResourceObject("MsgErr_ReturnEntry_Not_Journalized").ToString();
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    }
                                }
                                else
                                {
                                    litErrorMsg.Text = GetLocalResourceObject("Msg_Err_ReturnEntry_Is_PDC").ToString();
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Msg_Err_blockReturn").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            break;
                        }
                        break;
                    #endregion
                    #region REVERSE/Return Submit
                    case ActionsEnum.REVERSESUBMIT:
                    case ActionsEnum.RETURNSUBMIT:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region REVERSE/Return Delete
                    case ActionsEnum.REVERSEDELETE:
                    case ActionsEnum.RETURNDELETE:
                        hdfJournalizeWorkFlow.Value = "0";
                        ucrWrkf.Reset();
                        FillProcessID(1);
                        ResetForm();
                        EntryStatus = EntryStatus.LISTMODE;
                        GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                        break;
                    #endregion
                    #region PRINT LISTING
                    case ActionsEnum.PRINTLISTING:
                        if (CurrPK == 0)
                        {
                            foreach (GridViewRow grdrow in grdReceiptList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    bIsChecked = true;
                                    CurrPK = Convert.ToInt32(((HiddenField)grdrow.FindControl("hdfReceiptID")).Value);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            foreach (GridViewRow grdrow in grdReceiptList.Rows)
                            {
                                RadioButton rbtn;
                                rbtn = (RadioButton)grdrow.FindControl("rbtSelect");
                                if (rbtn.Checked)
                                {
                                    if (Convert.ToInt16(((HiddenField)grdrow.FindControl("hdfDelStatus")).Value) == 1)
                                    {
                                        btnEditforCancel.Visible = false;
                                        btnSave.Visible = false;
                                        btnEdit.Visible = false;

                                    }
                                    else
                                    {
                                        btnEditforCancel.Visible = true;
                                        btnSave.Visible = true;
                                    }
                                    bIsChecked = true;
                                    break;
                                }
                            }

                        }
                        if (bIsChecked)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.CRTJ +
                                   "&APPSUBTYPE=1&TRXTYPE=" + ApplicationType.CRTJ + "');", true);
                            //CurrPK = 0; //bug : 5734
                        }
                        else
                        {

                            litErrorMsg.Text = Resources.Report.Msg_Not_Selected;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }

                        break;
                    #endregion
                    #region PRINT ENTRY
                    case ActionsEnum.PRINT:
                        if (CurrPK != 0)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" + CurrPK.ToString() + "&APPTYPE=" + ApplicationType.CRTJ +
                                   "&APPSUBTYPE=1&TRXTYPE=" + ApplicationType.CRTJ + "');", true);
                        }

                        break;
                    #endregion
                    #region EDIT FOR CANCEL
                    case ActionsEnum.EDITFORCANCEL:
                        SetReceiptMode();
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;
                        break;
                    #endregion
                    #region DELETESUBMIT POPUP
                    case ActionsEnum.DELETESUBMIT:
                        //Show WorkFlow Popup
                        hdfIsSaveSubmit.Value = CommonConstants.SELECTVAL;
                        ucrWrkf.Visible = true;
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowWkfSubmit", "ShowContainerDivWkf('#divWkfSubmit','" + Resources.ErpRes.Submit + "','700');", true);
                        break;
                    #endregion
                    #region SHOW POPUP
                    case ActionsEnum.SHOWACCOUNT:
                        btnListApply.Visible = EntryStatus == EntryStatus.VIEWMODE ? false : true;
                        if (SelectedBankAccount != Convert.ToInt32(hdfBank.Value))
                        {
                            SelectedBankAccount = Convert.ToInt32(hdfBank.Value);
                            GetFieldValues(ControlsEnum.SUSPENCELIST);
                            SetFieldValues(ControlsEnum.SUSPENCELIST);
                        }
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSuspenceList]','" + GetLocalResourceObject("SuspenceList").ToString() + "','600','300');", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateSuspenceGridTotal", "CalculateSuspenceGridTotal();", true);
                        break;
                    case ActionsEnum.SHOWPOPUP:
                        grvRow = ((GridViewRow)((LinkButton)(sender)).Parent.Parent);

                        HiddenField hdfCategory = (HiddenField)grvRow.FindControl("hdfCategory");//To sep: Adv Inv & Inv
                        HiddenField hdfInvType = (HiddenField)grvRow.FindControl("hdfInvType");//To Sep: type(Dom,Exp,Per)
                        HiddenField hdfGroup = (HiddenField)grvRow.FindControl("hdfGroup");//Group 3(Misc Inv)
                        hdfInvoicePK = (HiddenField)grvRow.FindControl("hdfInvoicePK");
                        if (hdfGroup.Value == "3")// Misc Invoice
                        {
                            if (hdfInvType.Value != string.Empty)
                            {
                                if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Domestic)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfInvoicePK.Value + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE= " + (int)SalesInvoiceType.Domestic) + "');", true);
                                }
                                else if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Export)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfInvoicePK.Value + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                }
                                else if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Proforma)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfInvoicePK.Value + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                }
                            }
                        }
                        else
                        {
                            if (hdfCategory.Value == "1")//Invoice Category
                            {
                                if (hdfInvType.Value != string.Empty)
                                {
                                    if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Domestic)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfInvoicePK.Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic) + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Export)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfInvoicePK.Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfInvType.Value.ToString()) == (int)SalesInvoiceType.Proforma)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfInvoicePK.Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                    }
                                }
                            }
                            else if (hdfCategory.Value == "2")//Advance invoice Category
                            {
                                if (hdfInvType.Value == "2" || hdfInvType.Value == "3")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" +
                                        hdfInvoicePK.Value + "&APPTYPE=" + ApplicationType.DSIJ + "&APPSUBTYPE=" +
                                        Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.DSIJ + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfInvoicePK.Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=11") + "');", true);
                                }
                            }
                        }

                        break;
                    #endregion
                    #region RECEIVED AMOUNT SPLITUP
                    case ActionsEnum.RECEIVEDAMTSPLITUP:
                        long InvPk = 0;
                        HiddenField hdfInvoiceID = (HiddenField)((GridViewRow)((LinkButton)(sender)).Parent.Parent).FindControl("hdfInvoicePK");
                        long.TryParse(hdfInvoiceID.Value, out InvPk);
                        InvoicePK = InvPk;
                        GetFieldValues(ControlsEnum.RECEIVEDAMTSPLITUP);
                        SetFieldValues(ControlsEnum.RECEIVEDAMTSPLITUP);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalTotalRecdAmtSplitUp", "$(document).ready(function(){CalTotalRecdAmtSplitUp();});", true);
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divReceivedDetails]','" + "Received Amount" + "','600','200');", true);
                        break;
                    #endregion
                    #region EDIT FOR RETURN
                    case ActionsEnum.EDITFORRETURN:
                        SetUIEditView(commonActions);
                        ModifiedDatePnl.Visible = true;

                        hdfEditForReturn.Value = "1";
                        grdInvoiceList.Enabled = false;

                        txtReceiptDate.Enabled = false;
                        ddlMode.Enabled = false;
                        txtBank.Enabled = false;
                        txtBank.CssClass = "input-half input-disabled";
                        ddlAdjType.Enabled = false;
                        ddlBankChargeCurrency.Enabled = false;
                        txtBankCharge.Enabled = false;
                        txtRemarks.Enabled = false;
                        vrfBankName.Enabled = false;
                        vrfBranch.Enabled = false;
                        vrfAccountNo.Enabled = false;
                        vrfInstrumentNo.Enabled = false;
                        vrfInstrumentDate.Enabled = false;
                        txtInstrumentNo.Enabled = false;
                        txtInstrumentNo.CssClass = "input-small input-disabled";
                        txtInstrumentDate.Enabled = false;
                        txtInstrumentDate.CssClass = "input-small input-disabled";
                        vrfBankOfCheque.Enabled = false;



                        break;
                    #endregion
                    #region PRINT INVOICE
                    case ActionsEnum.PRINTINVOICE:
                        HiddenField hdfinvPK = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListinvPK"));
                        HiddenField hdfListInvType = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListInvType"));  //To Sep: type(Dom,Exp,Per)
                        HiddenField hdfinvCategory = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListinvCategory"));  //To sep: Adv Inv & Inv
                        HiddenField hdfinvGroup = (HiddenField)((((LinkButton)sender).Parent).FindControl("hdfListGroup")); //Group 3(Misc Inv)
                        if (hdfinvGroup.Value == "3")// Misc Invoice
                        {
                            if (hdfListInvType.Value != string.Empty)
                            {
                                if (Convert.ToInt32(hdfListInvType.Value.ToString()) == (int)SalesInvoiceType.Domestic)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE= " + (int)SalesInvoiceType.Domestic) + "');", true);
                                }
                                else if (Convert.ToInt32(hdfListInvType.Value.ToString()) == (int)SalesInvoiceType.Export)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                }
                                else if (Convert.ToInt32(hdfListInvType.Value.ToString()) == (int)SalesInvoiceType.Proforma)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.MSIT + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                }
                            }
                        }
                        else
                        {
                            if (hdfinvCategory.Value == "1")//Invoice Category
                            {
                                if (hdfListInvType.Value != string.Empty)
                                {
                                    if (Convert.ToInt32(hdfListInvType.Value.ToString()) == (int)SalesInvoiceType.Domestic)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfinvPK.Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Domestic) + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfListInvType.Value.ToString()) == (int)SalesInvoiceType.Export)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfinvPK.Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Export) + "');", true);
                                    }
                                    else if (Convert.ToInt32(hdfListInvType.Value.ToString()) == (int)SalesInvoiceType.Proforma)
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                            hdfinvPK.Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=" + (int)SalesInvoiceType.Proforma) + "');", true);
                                    }
                                }
                            }
                            else if (hdfinvCategory.Value == "2")//Advance invoice Category
                            {
                                if (hdfListInvType.Value == "2" || hdfListInvType.Value == "3")
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + "../Reports/GenerateReport.aspx?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.DSIJ + "&APPSUBTYPE=" +
                                        Convert.ToInt32(ApplicationSubType.ADVANCEINVOICE) + "&TRXTYPE=" + ApplicationType.DSIJ + "');", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "Openwindow", "OpenPDF('" + Page.ResolveClientUrl(Resources.PageURL.ReportUrl + "?ID=" +
                                        hdfinvPK.Value + "&APPTYPE=" + ApplicationType.DSI + "&APPSUBTYPE=11") + "');", true);
                                }
                            }
                        }
                        break;
                    #endregion
                    #region SUSPENCE LIST APPLY
                    case ActionsEnum.APPLY:
                        if (!IsValid)
                        {
                            litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        }
                        else//valid
                        {
                            lstReceiptSuspendDetails = new List<ReceiptSuspendDetails>();
                            lstReceiptSuspendDetails = (List<ReceiptSuspendDetails>)SetUIValuesToObject(ControlsEnum.SUSPENCELIST);
                            txtSuspenceAmt.Text = GetFormattedCurrency(lstReceiptSuspendDetails.Sum(x => x.RSC_AMOUNT));
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                        }
                        break;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
            }

            finally
            {
                salesReceiptServiceClient = null;
            }
        }

        private void ReceiptModeChanged()
        {
            txtBank.Text = string.Empty;
            hdfBank.Value = string.Empty;
            txtBranch.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtInstrumentNo.Text = string.Empty;
            txtInstrumentDate.Text = string.Empty;
            chkPDC.Checked = false;
            if (GetGlobalResourceObject("ConfigurationsRes", "PDCReconciliation").ToString() == "1")
            {
                chkPDC.Checked = true;
                chkPDC.Disabled = true;
            }
            hdfBankAccount.Value = string.Empty;
            SetReceiptMode();

        }

        private void SaveTransaction(ReceiptHeader receiptHeaderObj, int workflowFlag)
        {
            long? result = 0;
            string savePath = string.Empty;
            WorkflowDetails wkfDetails = null;
            string TrxNo = string.Empty;
            string action = string.Empty;
            if (receiptHeaderObj == null)
                receiptHeaderObj = new ReceiptHeader();
            #region Transaction Log and Application Code
            receiptHeaderObj.ATL_APP_TYPE = ApplicationType.CRT;
            #endregion
            #region New workflow Submition
            wkfDetails = ucrWrkf.GetWorkflowDetails();
            receiptHeaderObj.USER_PK = Convert.ToInt16(wkfDetails.UserPK);
            receiptHeaderObj.WKF_APPLICATION = CurrPK;
            receiptHeaderObj.WKF_COMMENTS = wkfDetails.Comments;
            receiptHeaderObj.WKF_TRX_FLAG = workflowFlag;
            receiptHeaderObj.WKF_PROCESS = wkfDetails.ProcessID;
            receiptHeaderObj.WKF_REFERENCE = wkfDetails.ReferenceID;
            receiptHeaderObj.WKF_TASK = wkfDetails.TaskID;
            receiptHeaderObj.WKF_TASK_ACTION = wkfDetails.ActionID;
            action = wkfDetails.ActionText;
            if (isCancelled)
                receiptHeaderObj.ATL_ACTION = (byte)LogAction.CANCEL;
            else
                receiptHeaderObj.ATL_ACTION = (byte)LogAction.SUBMIT;
            #endregion
            string ReceiptNo = string.Empty;
            string xmlDoc = CommonFunctions.XmlSerialize<ReceiptHeader>(receiptHeaderObj);
            result = BusinessLogic.Sales.ReceiptBL.SaveReceipt(xmlDoc, out ReceiptNo);
            if (result > 0)// Save Success ! do WorkFlow
            {
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
                if (string.IsNullOrEmpty(ReceiptNo))
                    ReceiptNo = lblReceiptNo.Text.Trim();
                object[] args = new object[2];
                args[0] = Resources.PageNameRes.ReceiptTrading;
                args[1] = ReceiptNo;
                litErrorMsg.Text = string.Format(litErrorMsg.Text, args);
                litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ReceiptTrading);

                CurrPK = (int)result;
                ucrWrkf.ApplicationID = (int)CurrPK;

                #region WkfSummarySave
                //int resultSummary = BusinessLogic.CommonManagement.CommonBL.SaveSummary(result.Value, Convert.ToInt32(hdfProcessID.Value));
                //if (resultSummary <= 0)
                //{
                //    litErrorMsg.Text = Resources.Messages.SummaryActionFailed;
                //    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "');", true);
                //}
                #endregion

                #region Inbox Redirection
                if (GetGlobalResourceObject("ConfigurationsRes", "IsGoToInbox").ToString() == "1" && base.WkfRefID > 0)
                {
                    ResetForm();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                            + "','" + Resources.ErpRes.Information + "','" + Page.ResolveClientUrl(Resources.PageURL.InboxURL) + "');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + litErrorMsg.Text
                        + "','" + Resources.ErpRes.Information + "');", true);
                    EntryStatus = EntryStatus.LISTMODE;
                    ResetForm();
                    GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                    SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                    btnNew.Focus();
                }
                #endregion

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
                    litErrorMsg.Text = Resources.PageNameRes.ReceiptTrading + " " + Resources.Messages.EditUsedByAnotherUser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else if (result == (int)DbSaveStatus.CODEEXIST)
                {
                    litErrorMsg.Text = Resources.PageNameRes.ReceiptTrading + " " + Resources.Messages.Itemsalreadyaddedbyanotheruser;
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                    + "','" + Resources.ErpRes.Information + "');", true);
                }
                else
                {
                    litErrorMsg.Text = Resources.Messages.ActionFailedPleaseTryAgain;
                    litErrorMsg.Text = string.Format(litErrorMsg.Text, Resources.PageNameRes.ReceiptTrading);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text)
                        + "','" + Resources.ErpRes.Information + "');", true);
                }
            }
        }

        private void SetReceiptMode()
        {
            imbSuspencelist.Visible = false;
            int mode = String.IsNullOrEmpty(ddlMode.SelectedValue) ? 0 : Convert.ToInt32(ddlMode.SelectedValue);
            switch (mode)
            {
                case (int)ReceiptModeEnum.CASH:
                    vrfBankName.Enabled = false;
                    vrfBranch.Enabled = false;
                    vrfAccountNo.Enabled = false;
                    vrfInstrumentNo.Enabled = false;
                    vrfInstrumentDate.Enabled = false;
                    txtInstrumentNo.Enabled = false;
                    txtInstrumentNo.CssClass = "input-small input-disabled";
                    txtInstrumentDate.Enabled = false;
                    txtInstrumentDate.CssClass = "input-small input-disabled";
                    txtbankOfCheque.Text = string.Empty;
                    txtbankOfCheque.Enabled = false;
                    txtbankOfCheque.CssClass = "input-half input-disabled";
                    break;
                case (int)ReceiptModeEnum.BANK:
                    imbSuspencelist.Visible = Convert.ToBoolean(GetGlobalResourceObject("ConfigurationsRes", "ShowSuspenceListButton"));
                    vrfBankName.Enabled = true;
                    vrfBranch.Enabled = true;
                    vrfAccountNo.Enabled = true;
                    vrfInstrumentNo.Enabled = true;
                    vrfInstrumentDate.Enabled = false;
                    txtInstrumentNo.Enabled = true;
                    txtInstrumentNo.CssClass = "input-small";
                    txtInstrumentDate.Enabled = true;
                    txtInstrumentDate.CssClass = "input-small";
                    txtbankOfCheque.Text = string.Empty;
                    txtbankOfCheque.Enabled = false;
                    txtbankOfCheque.CssClass = "input-half input-disabled";
                    GetFieldValues(ControlsEnum.CUSTOMERBANK);
                    if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                    {
                        txtBank.Text = crmCustomerMstList[0].CBM_NAME == null ? string.Empty : crmCustomerMstList[0].CBM_CODE + " - " + crmCustomerMstList[0].CBM_NAME; //RCH_BANK_TEXT;
                        hdfBank.Value = crmCustomerMstList[0].CBM_PK.ToString() == null ? string.Empty : crmCustomerMstList[0].CBM_PK.ToString();
                        txtAccountNo.Text = crmCustomerMstList[0].CBM_ACC_NO;
                        txtBranch.Text = crmCustomerMstList[0].CBM_BRANCH;
                        hdfBankAccount.Value = crmCustomerMstList[0].CBM_ACCOUNT.ToString();
                    }
                    break;
                case (int)ReceiptModeEnum.CHEQUE:
                    vrfBankName.Enabled = true;
                    vrfBranch.Enabled = true;
                    vrfAccountNo.Enabled = true;
                    vrfInstrumentNo.Enabled = true;
                    vrfInstrumentDate.Enabled = true;
                    txtInstrumentNo.Enabled = true;
                    txtInstrumentNo.CssClass = "input-small";
                    txtInstrumentDate.Enabled = true;
                    txtInstrumentDate.CssClass = "input-small";
                    txtbankOfCheque.Enabled = true;
                    txtbankOfCheque.CssClass = "input-half input-half";
                    GetFieldValues(ControlsEnum.CUSTOMERBANK);
                    if (crmCustomerMstList != null && crmCustomerMstList.Count > 0)
                    {
                        txtBank.Text = crmCustomerMstList[0].CBM_NAME == null ? string.Empty : crmCustomerMstList[0].CBM_CODE + " - " + crmCustomerMstList[0].CBM_NAME; //RCH_BANK_TEXT;
                        hdfBank.Value = crmCustomerMstList[0].CBM_PK.ToString() == null ? string.Empty : crmCustomerMstList[0].CBM_PK.ToString();
                        txtAccountNo.Text = crmCustomerMstList[0].CBM_ACC_NO;
                        txtBranch.Text = crmCustomerMstList[0].CBM_BRANCH;
                        hdfBankAccount.Value = crmCustomerMstList[0].CBM_ACCOUNT.ToString();
                    }
                    break;
                case (int)ReceiptModeEnum.OTHERS:
                    vrfBankName.Enabled = false;
                    vrfBranch.Enabled = false;
                    vrfAccountNo.Enabled = false;
                    vrfInstrumentNo.Enabled = false;
                    vrfInstrumentDate.Enabled = false;
                    txtInstrumentNo.Enabled = false;
                    txtInstrumentNo.CssClass = "input-small input-disabled";
                    txtInstrumentDate.Enabled = false;
                    txtInstrumentDate.CssClass = "input-small input-disabled";
                    txtbankOfCheque.Text = string.Empty;
                    txtbankOfCheque.Enabled = false;
                    vrfBankOfCheque.Enabled = false;
                    txtbankOfCheque.CssClass = "input-half input-disabled";
                    txtBank.Text = string.Empty;
                    txtBank.Enabled = false;
                    txtBank.CssClass = "input-half input-disabled";
                    hdfBank.Value = string.Empty;
                    break;
                default:
                    vrfBankName.Enabled = true;
                    vrfBranch.Enabled = true;
                    vrfAccountNo.Enabled = true;
                    vrfInstrumentNo.Enabled = false;
                    vrfInstrumentDate.Enabled = true;
                    txtInstrumentNo.Enabled = true;
                    txtInstrumentNo.CssClass = "input-small";
                    txtInstrumentDate.Enabled = true;
                    txtInstrumentDate.CssClass = "input-small";
                    txtbankOfCheque.Text = string.Empty;
                    txtbankOfCheque.Enabled = false;
                    txtbankOfCheque.CssClass = "input-half input-disabled";
                    break;
            }
        }

        /// <summary>
        /// Receipt split apply
        /// </summary>
        private void ReceiptSplitSave(bool ShowMsg)
        {

            if (!IsValid)
            {
                litErrorMsg.Text = Resources.Report.Msg_Save_Error;
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
            }
            else
            {
                if (grdReceiptSplit.Rows.Count >= 1)
                {
                    HiddenField lblTotalPayNowFooterSplit = (HiddenField)(grdReceiptSplit.FooterRow.FindControl("hdfTotalPayNowFooterSplit"));

                    decimal TotalReceiveNowSplit = 0;
                    decimal CrAllocatedAmnt = 0;
                    decimal InvReceiveNow = 0;
                    foreach (GridViewRow grvRow in grdReceiptSplit.Rows)
                    {
                        TextBox txtPayNowSplit = (TextBox)grvRow.FindControl("txtPayNowSplit");
                        decimal RecvenowSplit = 0;
                        decimal.TryParse(txtPayNowSplit.Text, out RecvenowSplit);
                        TotalReceiveNowSplit += RecvenowSplit;
                    }
                    if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping != null)
                    {
                        ReceiptTrxMapping objTrxMpg = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == InvoicePK);
                        if (objTrxMpg != null && objTrxMpg.DebitNoteAllocation != null && objTrxMpg.DebitNoteAllocation.Count > 0)
                            CrAllocatedAmnt = objTrxMpg.DebitNoteAllocation.Sum(sm => sm.RNM_PAID_AMOUNT + sm.RNM_ADJ_AMOUNT);
                    }
                    lblTotalPayNowFooterSplit.Value = TotalReceiveNowSplit.ToString();
                    if (lblTotalPayNowFooterSplit != null && !string.IsNullOrEmpty(lblTotalPayNowFooterSplit.Value) && !string.IsNullOrEmpty(lblInvSplitReceiveNow.Text))
                    {
                        InvReceiveNow = Convert.ToDecimal(lblInvSplitReceiveNow.Text.Trim().Replace(",", ""));
                        InvReceiveNow += AdjustmentAmount;
                        InvReceiveNow = (InvReceiveNow - CrAllocatedAmnt) < 0 ? 0 : (InvReceiveNow - CrAllocatedAmnt);
                        if (Convert.ToDecimal(lblTotalPayNowFooterSplit.Value) <= InvReceiveNow)
                        {
                            divErrorLabel.Visible = false;
                            finReceiptCusSoMpgList = (List<ReceiptSOMapping>)SetUIValuesToObject(ControlsEnum.PAYMENTSPLITLIST);
                            if (finReceiptCusSoMpgList != null)
                            {

                                if (AppliedInvPkList == null)
                                    AppliedInvPkList = new List<long>();
                                if (!AppliedInvPkList.Contains(InvoicePK))
                                {
                                    AppliedInvPkList.Add(InvoicePK);  // To keep applied invoice Pks
                                }


                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);

                            }
                        }
                        else
                        {

                            if (ShowMsg)
                            {
                                divErrorLabel.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                                return;
                            }
                        }

                    }
                    else
                    {
                        if (ShowMsg)
                        {
                            divErrorLabel.Visible = true;
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowPop1", "ShowContainerDiv('[id$=divSoSplitUp]','" + GetLocalResourceObject("ReceiptSplit").ToString() + "','800','300');", true);
                            return;
                        }
                    }
                }
                else
                {

                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1", "ClosePopup();", true);
                }
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1","ClosePopup();", true);
            }
        }

        /// <summary>
        /// Receipt split popup details
        /// </summary>
        /// <param name="InvPK"></param>
        private void InvoiceDetails(long InvPK)
        {
            InvoicePK = InvPK;
            decimal AdjAmnt = 0;
            HiddenField hdfInvoicePK;
            Tax = 0;
            PayNow = 0;
            InvOtherCharge = 0;
            AdjustmentAmount = 0;

            divErrorLabel.Visible = false;
            GridViewRow GrdInvRow = null;
            foreach (GridViewRow grdRow in grdInvoiceList.Rows)
            {
                hdfInvoicePK = (HiddenField)grdRow.FindControl("hdfInvoicePK");
                if (Convert.ToInt64(hdfInvoicePK.Value) == InvoicePK)
                {
                    GrdInvRow = grdRow;
                    break;
                }
            }

            HiddenField hdfReceiptMpgPK = (HiddenField)GrdInvRow.FindControl("hdfReceiptMpgPK");
            TextBox txtReceivedNow = (TextBox)GrdInvRow.FindControl("txtReceivedNow");
            HiddenField hdfTotalTax = (HiddenField)GrdInvRow.FindControl("hdfTotalTax");
            TextBox txtOtherCharges = (TextBox)GrdInvRow.FindControl("txtOtherCharges");
            Label lblAdjAmount = (Label)GrdInvRow.FindControl("lblAdjAmount");
            hdfTaxPer.Value = "1";
            hdfOtherPer.Value = "1";
            if (hdfTotalTax != null && !string.IsNullOrEmpty(hdfTotalTax.Value.Trim()) && txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
            {
                if (Convert.ToDecimal(txtReceivedNow.Text.Trim()) > 0)
                {
                    hdfTaxPer.Value = (Convert.ToDecimal(hdfTotalTax.Value.Trim()) / Convert.ToDecimal(txtReceivedNow.Text.Trim())).ToString();
                    Tax = Convert.ToDecimal(Convert.ToDecimal(hdfTotalTax.Value.Trim()));
                }
            }
            if (txtOtherCharges != null && !string.IsNullOrEmpty(txtOtherCharges.Text.Trim()) && txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
            {
                InvOtherCharge = Convert.ToDecimal(txtOtherCharges.Text.Replace(",", "").Trim());
                if (Convert.ToDecimal(txtReceivedNow.Text.Trim()) > 0)
                    hdfOtherPer.Value = (Convert.ToDecimal(txtOtherCharges.Text.Trim()) / Convert.ToDecimal(txtReceivedNow.Text.Trim())).ToString();
            }
            if (txtReceivedNow != null && !string.IsNullOrEmpty(txtReceivedNow.Text.Trim()))
            {
                PayNowAmount = Convert.ToDecimal(txtReceivedNow.Text.Trim());
                PayNow = PayNowAmount;
            }

            if (lblAdjAmount != null && !string.IsNullOrEmpty(lblAdjAmount.Text.Trim()))
            {
                decimal.TryParse(lblAdjAmount.Text.Replace(",", ""), out AdjAmnt);
                AdjustmentAmount = AdjAmnt;
                PayNow += AdjAmnt;
            }
            GetUIValuesFromObject(ControlsEnum.PAYMENTSPLITLIST);
            SetFieldValues(ControlsEnum.PAYMENTSPLITLIST);
            GetUIValuesFromObject(ControlsEnum.SPLITPAYNOWFORMULISO);
            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);
        }

        #region --- For Grid Actions----
        protected void ActionHandler(object sender, GridViewRowEventArgs e)
        {
            try
            {
                #region Grid Data Row
                if (e.Row.RowType == DataControlRowType.DataRow && e.Row.DataItem != null)
                {
                    #region grdReceiptList
                    if (((GridView)sender).ID == "grdReceiptList")
                    {
                        LinkButton lnkInvnos = e.Row.FindControl("lnkInvnos") as LinkButton;
                        HiddenField hdfInvNos = e.Row.FindControl("hdfInvNos") as HiddenField;
                        if (hdfInvNos.Value.Split(',').Count() > 1)
                        {
                            lnkInvnos.Attributes.Add("onClick", "return false");
                            lnkInvnos.CssClass = "removelinkPopup";
                        }
                        else
                        {
                            lnkInvnos.Attributes.Add("onclick", "return true;");
                        }
                    }
                    #endregion

                }
                #endregion
                #region Grid Header Row
                if (e.Row.RowType == DataControlRowType.Header)
                {

                }
                #endregion
                #region Grid Footer Row
                if (e.Row.RowType == DataControlRowType.Footer)
                {

                }
                #endregion
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
                GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                EntryStatus = EntryStatus.LISTMODE;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
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
            btnSaveSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnSave.PreRender += new EventHandler(btnAction_PreRender);
            btnDeleteNew.PreRender += new EventHandler(btnAction_PreRender);
            btnCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnJournalize.PreRender += new EventHandler(btnAction_PreRender);

            btnNew.PreRender += new EventHandler(btnAction_PreRender);
            btnEdit.PreRender += new EventHandler(btnAction_PreRender);
            btnView.PreRender += new EventHandler(btnAction_PreRender);

            //lbnSOListing.PreRender += new EventHandler(btnAction_PreRender);
            //lnbDeliveryOrder.PreRender += new EventHandler(btnAction_PreRender);
            //lnbSalesInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAdvanceInvoice.PreRender += new EventHandler(btnAction_PreRender);
            //lbnSalesReceipt.Load += new EventHandler(btnAction_Load);
            //lnbCrDrNote.PreRender += new EventHandler(btnAction_PreRender);
            //lnbAcPayables.PreRender += new EventHandler(btnAction_PreRender);
            //lnbMiscellaneous.PreRender += new EventHandler(btnAction_PreRender);

            lnkList.PreRender += new EventHandler(btnAction_PreRender);
            lnkDetail.PreRender += new EventHandler(btnAction_PreRender);

            btnSavePaymentSplit.PreRender += new EventHandler(btnAction_PreRender);
            btnReverse.PreRender += new EventHandler(btnAction_PreRender);
            btnReverseDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnReturnDetail.PreRender += new EventHandler(btnAction_PreRender);
            btnReturn.PreRender += new EventHandler(btnAction_PreRender);
            btnEditforCancel.PreRender += new EventHandler(btnAction_PreRender);
            btnCancelSubmit.PreRender += new EventHandler(btnAction_PreRender);
            btnEditForReturn.PreRender += new EventHandler(btnAction_PreRender);



            btnSaveSubmit.Load += new EventHandler(btnAction_Load);
            btnSubmit.Load += new EventHandler(btnAction_Load);
            btnSave.Load += new EventHandler(btnAction_Load);
            btnDeleteNew.Load += new EventHandler(btnAction_Load);
            btnCancel.Load += new EventHandler(btnAction_Load);
            btnJournalize.Load += new EventHandler(btnAction_Load);

            btnEdit.Load += new EventHandler(btnAction_Load);
            btnView.Load += new EventHandler(btnAction_Load);
            btnNew.Load += new EventHandler(btnAction_Load);

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

            btnSavePaymentSplit.Load += new EventHandler(btnAction_Load);
            btnReverse.Load += new EventHandler(btnAction_Load);
            btnReverseDetail.Load += new EventHandler(btnAction_Load);
            btnReturnDetail.Load += new EventHandler(btnAction_Load);
            btnReturn.Load += new EventHandler(btnAction_Load);
            btnEditforCancel.Load += new EventHandler(btnAction_Load);
            btnCancelSubmit.Load += new EventHandler(btnAction_Load);
            btnEditForReturn.Load += new EventHandler(btnAction_Load);
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
                // Change Code As per the page
                GetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                SetFieldValues(ControlsEnum.RECEIPTHDRLIST);
                EntryStatus = EntryStatus.LISTMODE;
                //============================
                EnableDisableButtons(e.TotalPages);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.ErpRes.Information + "');", true);
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
                custPK = 0;
                int.TryParse(hdfCustomerPk.Value, out custPK);
                if (custPK > 0)
                    txtPendingInvNumber.Enabled = true;
                else
                    txtPendingInvNumber.Enabled = false;

                hdfSaveWithoutBankCharge.Value = "0";
                if (grdInvoiceList.Rows != null)
                {
                    if (grdInvoiceList.Rows.Count > 0 || CurrPK > 0)
                        txtCustomer.Enabled = false;
                    else
                        txtCustomer.Enabled = true;

                    foreach (GridViewRow gvr in grdInvoiceList.Rows)
                    {
                        Button lnkAllocation = gvr.FindControl("lnkAllocation") as Button;
                        if (lnkAllocation != null)
                        {
                            lnkAllocation.Visible = SOGroup != SalesInvoiceGroup.Miscellaneous;
                        }
                        //Button btnCrdrAllocation = gvr.FindControl("btnCrdrAllocation") as Button;
                        //if (btnCrdrAllocation != null)
                        //{
                        //    btnCrdrAllocation.Visible = SOGroup != SalesInvoiceGroup.Miscellaneous;
                        //}
                    }
                }

                if (EntryStatus == EntryStatus.VIEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ModeAutoComplete", "ModeAutoComplete();", true);
                }
                else if (EntryStatus == EntryStatus.NEWMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ViewMode", "$(document).ready(function(){ViewMode(2);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ModeAutoComplete", "ModeAutoComplete();", true);
                }
                else if (EntryStatus == EntryStatus.ENTRYMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "HideListing", "$(document).ready(function(){ShowListing();});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabInactive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabActive").ToString();
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ModeAutoComplete", "ModeAutoComplete();", true);
                }
                else if (EntryStatus == EntryStatus.LISTMODE)
                {
                    ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowListing", "$(document).ready(function(){ShowListing(1);});", true);
                    lnkList.CssClass = GetLocalResourceObject("TabActive").ToString();
                    lnkDetail.CssClass = GetLocalResourceObject("TabInactive").ToString();
                }

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "InitializeComponents", "$(document).ready(function(){InitComponents();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_HideAdvance", "$(document).ready(function(){ShowHideAdvancedSearch();});", true);
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotal", "$(document).ready(function(){CalculateTotal();});", true);
                //ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "_CalculateTotalSplit", "$(document).ready(function(){CalculateTotalSplit();});", true);

                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "CalculateAdjnFooter", "$(document).ready(function(){CalculateAdjnFooter();});", true);
                if (IsDeleted)
                {
                    btnSave.Visible = false;
                    hdfIsCancelled.Value = "1";
                }
                else
                    hdfIsCancelled.Value = "0";
                btnEntryPrint.Visible = CurrPK > 0 ? true : false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.ProcessException(ex) + "','" + Resources.Messages.Information + "');", true);
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
        /// Set Config Value
        /// </summary>
        private void SetConfigValue()
        {
            currentUser = ((BusinessObject.User)(HttpContext.Current.User.Identity));
            DataTable dtConfig = BusinessLogic.CommonManagement.CommonBL.GetApplicaitonConfiguaration("BIZUNIT SETTINGS", "CURRENCY");
            if (dtConfig != null && dtConfig.Rows.Count > 0)
                SBUID = dtConfig.Rows[0]["ACF_VALUE"].ToString() == "1" ? -1 : currentUser.SBUID;
            hdfIsTaxForOtherCharge.Value = GetGlobalResourceObject("ConfigurationsRes", "OtherChargeTaxSales").ToString();
            ShowTaxForMiscInv = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowTaxInRcptForMisInv").ToString()));
            ShowAdjColumn = Convert.ToBoolean(Convert.ToInt32(GetGlobalResourceObject("ConfigurationsRes", "ShowAdjColumnReceipt").ToString()));
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
        /// <summary>
        /// Allow to exceed more than inv amt
        /// </summary>
        /// <returns></returns>
        private bool IsValidPayment()
        {
            bool result = false;

            if (Convert.ToInt32(hdfCategoryDtl.Value) == (int)SalesInvoiceCategory.Advanced)
            {
                if (Convert.ToInt32(hdfTypeDtl.Value) != (int)SalesInvoiceType.Domestic)
                {
                    result = true;
                }
                else
                {
                    result = false;
                    foreach (GridViewRow grdPOrow in grdInvoiceList.Rows)
                    {
                        Label lblTotalAmount = (Label)grdPOrow.FindControl("lblTotalAmount");
                        //Label lblPaid = (Label)grdPOrow.FindControl("lblReceived");
                        LinkButton lnkReceived = (LinkButton)grdPOrow.FindControl("lnkReceived");
                        TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtReceivedNow");

                        TextBox txtOtherCharges = (TextBox)grdPOrow.FindControl("txtOtherCharges");
                        Label lblOtherCharges = (Label)grdPOrow.FindControl("lblOtherAmount");
                        HiddenField hdfOtherChargesPrev = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");

                        HiddenField hdfInvoicePK = (HiddenField)grdPOrow.FindControl("hdfInvoicePK");
                        Label lblCrdrAlcnAmount = (Label)grdPOrow.FindControl("lblCrdrAlcnAmount");
                        Label lblAdjAmount = (Label)grdPOrow.FindControl("lblAdjAmount");
                        Label lblBaltoReceive = (Label)grdPOrow.FindControl("lblBaltoReceive");

                        if (Convert.ToDecimal(lblTotalAmount.Text.Replace(",", "")) >= Convert.ToDecimal(lnkReceived.Text.Replace(",", "")) + Convert.ToDecimal(txtPayNow.Text))
                        {
                            result = true;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                  "ClosePopup();", true);
                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            result = false; break;
                        }
                        string s = lblOtherCharges.Text;
                        s = txtOtherCharges.Text;
                        s = hdfOtherChargesPrev.Value;

                        if (Convert.ToDecimal(lblOtherCharges.Text.Replace(",", "")) >= Convert.ToDecimal(txtOtherCharges.Text) + Convert.ToDecimal(hdfOtherChargesPrev.Value))
                        {
                            result = true;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                  "ClosePopup();", true);
                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            result = false;
                            break;
                        }

                        if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lnkReceived.Text.Replace(",", "")))) - (Convert.ToDecimal(txtOtherCharges.Text) + (Convert.ToDecimal(hdfOtherChargesPrev.Value))) <= (Convert.ToDecimal(lblTotalAmount.Text.Replace(",", "")) - Convert.ToDecimal(lblOtherCharges.Text.Replace(",", ""))))
                        {
                            result = true;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
                                                                  "ClosePopup();", true);
                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

                            result = false; break;
                        }

                        #region Validation for Debit Note Amount
                        decimal Payable = 0;
                        decimal Paid = 0;
                        // decimal PaidCNAmount = 0;
                        decimal AllocatedCN = 0;
                        decimal AdjAmount = 0;
                        decimal InitialPayable = 0;
                        decimal BalanceDN = 0;
                        decimal BalanceToReceive = 0;
                        decimal DraftedDNAmount = 0;
                        decimal.TryParse(lblTotalAmount.Text, out Payable);
                        decimal.TryParse(lnkReceived.Text, out Paid);
                        decimal.TryParse(lblCrdrAlcnAmount.Text, out AllocatedCN);
                        decimal.TryParse(lblAdjAmount.Text, out AdjAmount);
                        decimal.TryParse(lblBaltoReceive.Text, out BalanceToReceive);
                        //if (ReceiptCrdrList != null)
                        //{
                        //    BalanceDN = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value)).Sum(sm => sm.RNM_BALANCE_AMOUNT - (sm.RNM_ADJ_AMOUNT + sm.RNM_PAID_AMOUNT));
                        //}
                        if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping != null)
                        {
                            ReceiptTrxMapping objTrxMpg = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value));
                            if (objTrxMpg != null && objTrxMpg.DebitNoteAllocation != null && objTrxMpg.DebitNoteAllocation.Count > 0)
                                BalanceDN = objTrxMpg.DebitNoteAllocation.Sum(sm => (sm.RNM_CRDR_AMOUNT - sm.RNM_ALLOCATED_AMOUNT) - (sm.RNM_ADJ_AMOUNT + sm.RNM_PAID_AMOUNT));
                        }

                        GetFieldValues(ControlsEnum.CRDRMPGLIST);
                        if (FinCrdrMpgList != null && FinCrdrMpgList.Count > 0)
                        {
                            DraftedDNAmount = FinCrdrMpgList.Sum(r => r.CDM_AMOUNT);
                        }
                        InitialPayable = BalanceToReceive + AdjAmount;
                        decimal PayNowWithAdj = Convert.ToDecimal(txtPayNow.Text) + AdjAmount;
                        decimal PayableWithoutBlnDN = InitialPayable - BalanceDN - DraftedDNAmount;
                        if (PayNowWithAdj > PayableWithoutBlnDN && (BalanceDN > 0 || DraftedDNAmount > 0))
                        {
                            litErrorMsg.Text = GetLocalResourceObject("Err_msg_DNAmount").ToString();
                            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                            result = false;
                            break;
                        }
                        #endregion

                    }
                }
            }
            else
            {
                result = true;
                foreach (GridViewRow grdPOrow in grdInvoiceList.Rows)
                {
                    Label lblTotalAmount = (Label)grdPOrow.FindControl("lblTotalAmount");
                    //Label lblPaid = (Label)grdPOrow.FindControl("lblReceived");
                    LinkButton lnkReceived = (LinkButton)grdPOrow.FindControl("lnkReceived");
                    TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtReceivedNow");

                    TextBox txtOtherCharges = (TextBox)grdPOrow.FindControl("txtOtherCharges");
                    Label lblOtherCharges = (Label)grdPOrow.FindControl("lblOtherAmount");
                    HiddenField hdfOtherChargesPrev = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");

                    HiddenField hdfInvoicePK = (HiddenField)grdPOrow.FindControl("hdfInvoicePK");
                    Label lblCrdrAlcnAmount = (Label)grdPOrow.FindControl("lblCrdrAlcnAmount");
                    Label lblAdjAmount = (Label)grdPOrow.FindControl("lblAdjAmount");
                    Label lblBaltoReceive = (Label)grdPOrow.FindControl("lblBaltoReceive");




                    #region Validation for Debit Note Amount
                    decimal Payable = 0;
                    decimal Paid = 0;
                    // decimal PaidCNAmount = 0;
                    decimal AllocatedCN = 0;
                    decimal AdjAmount = 0;
                    decimal InitialPayable = 0;
                    decimal BalanceDN = 0;
                    decimal BalanceToReceive = 0;
                    decimal DraftedDNAmount = 0;
                    decimal.TryParse(lblTotalAmount.Text, out Payable);
                    decimal.TryParse(lnkReceived.Text, out Paid);
                    decimal.TryParse(lblCrdrAlcnAmount.Text, out AllocatedCN);
                    decimal.TryParse(lblAdjAmount.Text, out AdjAmount);
                    decimal.TryParse(lblBaltoReceive.Text, out BalanceToReceive);
                    //if (ReceiptCrdrList != null)
                    //{
                    //    BalanceDN = ReceiptCrdrList.Where(r => r.RNM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value)).Sum(sm => sm.RNM_BALANCE_AMOUNT - (sm.RNM_ADJ_AMOUNT + sm.RNM_PAID_AMOUNT));
                    //}
                    if (ReceiptHeaderSession != null && ReceiptHeaderSession.ReceiptTrxMapping != null)
                    {
                        ReceiptTrxMapping objTrxMpg = ReceiptHeaderSession.ReceiptTrxMapping.SingleOrDefault(r => r.RCM_INVOICE_HDR == Convert.ToInt64(hdfInvoicePK.Value));
                        if (objTrxMpg != null && objTrxMpg.DebitNoteAllocation != null && objTrxMpg.DebitNoteAllocation.Count > 0)
                            BalanceDN = objTrxMpg.DebitNoteAllocation.Sum(sm => (sm.RNM_CRDR_AMOUNT - sm.RNM_ALLOCATED_AMOUNT) - (sm.RNM_ADJ_AMOUNT + sm.RNM_PAID_AMOUNT));
                    }
                    GetFieldValues(ControlsEnum.CRDRMPGLIST);
                    if (FinCrdrMpgList != null && FinCrdrMpgList.Count > 0)
                    {
                        DraftedDNAmount = FinCrdrMpgList.Sum(r => r.CDM_AMOUNT);
                    }
                    InitialPayable = BalanceToReceive + AdjAmount;
                    decimal PayNowWithAdj = Convert.ToDecimal(txtPayNow.Text) + AdjAmount;
                    decimal PayableWithoutBlnDN = InitialPayable - BalanceDN - DraftedDNAmount;
                    if (PayNowWithAdj > PayableWithoutBlnDN && (BalanceDN > 0 || DraftedDNAmount > 0))
                    {
                        litErrorMsg.Text = GetLocalResourceObject("Err_msg_DNAmount").ToString();
                        ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ClosePopup();ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
                        result = false;
                        break;
                    }
                    #endregion

                }
            }
            return result;
        }
        #region doesn't allow to exceed more than inv amt coomented
        //private bool IsValidPayment()
        //{
        //    bool result = false;
        //    foreach (GridViewRow grdPOrow in grdInvoiceList.Rows)
        //    {
        //        Label lblTotalAmount = (Label)grdPOrow.FindControl("lblTotalAmount");
        //        Label lblPaid = (Label)grdPOrow.FindControl("lblReceived");
        //        TextBox txtPayNow = (TextBox)grdPOrow.FindControl("txtReceivedNow");

        //        TextBox txtOtherCharges = (TextBox)grdPOrow.FindControl("txtOtherCharges");
        //        Label lblOtherCharges = (Label)grdPOrow.FindControl("lblOtherAmount");
        //        HiddenField hdfOtherChargesPrev = (HiddenField)grdPOrow.FindControl("hdfOtherchargeOLD");

        //        if (Convert.ToDecimal(lblTotalAmount.Text) >= Convert.ToDecimal(lblPaid.Text) + Convert.ToDecimal(txtPayNow.Text))
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
        //                                                  "ClosePopup();", true);
        //            litErrorMsg.Text = GetLocalResourceObject("Err_msg_1").ToString();
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);
        //            result = false; break;
        //        }
        //        string s = lblOtherCharges.Text;
        //        s = txtOtherCharges.Text;
        //        s = hdfOtherChargesPrev.Value;

        //        if (Convert.ToDecimal(lblOtherCharges.Text) >= Convert.ToDecimal(txtOtherCharges.Text) + Convert.ToDecimal(hdfOtherChargesPrev.Value))
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
        //                                                  "ClosePopup();", true);
        //            litErrorMsg.Text = GetLocalResourceObject("Err_msg_2").ToString();
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

        //            result = false;
        //            break;
        //        }
        //        if ((Convert.ToDecimal(txtPayNow.Text) + (Convert.ToDecimal(lblPaid.Text))) - (Convert.ToDecimal(txtOtherCharges.Text) + (Convert.ToDecimal(hdfOtherChargesPrev.Value))) <= (Convert.ToDecimal(lblTotalAmount.Text) - Convert.ToDecimal(lblOtherCharges.Text)))
        //        {
        //            result = true;
        //        }
        //        else
        //        {
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ClosePop1",
        //                                                  "ClosePopup();", true);
        //            litErrorMsg.Text = GetLocalResourceObject("Err_msg_3").ToString();
        //            ScriptManager.RegisterStartupScript(this.Page, typeof(Page), "ShowErrorMsg", "ShowErrorMessage('" + CommonFunctions.FormatErrorMessage(litErrorMsg.Text) + "','" + Resources.Messages.Information + "');", true);

        //            result = false; break;
        //        }

        //    }
        //    return result;
        //}
        #endregion
        #endregion
        #region Enum
        /// <summary>
        ///Page Controls Enum 
        /// </summary>
        public enum ControlsEnum
        {
            RECEIPTHDRLIST,
            RECEIPTMPGLIST,
            BANK,
            RECEIPTHDRENTRYBYPK,
            RECEIPTNO,
            EXCHANGERATE,
            EXCHANGERATEINBASECURRENCY,
            JOURNALIZE,
            WRKFSUBMIT,
            FINHEADER,
            PAYMENTSPLITLIST,
            INVOICEVNDHDR,
            INVOICEVNDMPGLIST,
            PAYMODE,
            FINPERIOD,
            GETRECEIPTPKBYJOURNALPK,
            FILLWORKFLOWSTATUS,
            DISCOUNTTYPE,
            REVERSE,
            BANKCURRENCY,
            EXCHANGERATEBANK,
            CHEQUERETURN,
            COMPANY,
            CUSTOMERBANK,
            FINHEADERSTATUS,
            CHKINSTRNO,
            FINHEADERSTATUSREVERSE,
            RECEIPTCUSADJN,
            ADJNSPLITLIST,
            ISADVDEDUCTED,
            SPLITPAYNOWFORMULISO,
            INVOICEVNDMPGLISTFORAUTOALCN,
            CRDRALLOCATION,
            CRDRSPLITLIST,
            RECEIVEDAMTSPLITUP,
            CRDRMPGLIST,
            INVOICEHDRBYPK,
            RECEIPTGET,
            CheckReceiptAllocation,
            PEDINGINVLIST,
            INVOICETYPE,
            UPDATEGRIDVALTOOBJECT,
            RECEIPTHEADER,
            INVCATEGORY,
            CUSTOMERDETAILS,
            SUSPENCELIST


        }
        /// <summary>
        ///Receipt Mode Enum 
        /// </summary>
        public enum ReceiptModeEnum
        {
            CASH = 1,
            CHEQUE,
            DD,
            BANK,
            GENERAL,
            OTHERS
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